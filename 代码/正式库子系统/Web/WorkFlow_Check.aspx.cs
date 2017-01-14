using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using FMOP.DB;
using FMOP.XHelp;
using FMOP.Tools;
using FMOP.Module;
using FMOP.Common;
public partial class Web_WorkFlow_Check : System.Web.UI.Page
{
    #region public variable
    private string[] fieldName;//字段名
    private string[] caption;//字段描述
    private string[] fieldType;//字段类型，根据字段查询数据库表读取

    private string module = string.Empty;//调用此页面所传参数
    private string columnstr = string.Empty;//所有字段的组合，字段从xml文档读取，查询数据库使用
    private int pagenumber = 1;//当前页数
    private int pagesize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());//分页记录数
    private string keyfield = "CreateTime";//排序字段，从xml文档中读取
    private string wherestr = "";//查询条件，没有where，如：1=1 and 2=2
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Globalization.CultureInfo ci = null;
        ci = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");
        System.Threading.Thread.CurrentThread.CurrentCulture = ci;

        if (!Page.IsPostBack)
        {
            if (Request["module"] != null)
            {
                module = Request["module"].ToString();
                addrole();
                if (xmlParse() == 0)
                {
                    Response.Clear();
                    Response.Write("<script language=javascript>window.alert('没有此项数据！');history.back();</script>");
                    Response.End();
                    return;
                }
                else
                {
					this.txtmodule.Text = module;
                    pageinit();
                    addSelect();
                }
            }
            else
            {
                Response.Clear();
                Response.Write("<script language=javascript>window.alert('没有获取到模块参数！');history.back();</script>");
                Response.End();
                return;
            }
        }
        else
        {
			module = this.txtmodule.Text;
			initpage();
            xmlParse();
            

            int flag = int.Parse(Request["txtflag"]);
            switch (flag)
            {
                case 1://查询
					pagenumber = 1;
					jumppage.Text = "1";
					txtpagenumber.Text = "1";
                    addSQL();
                    break;
                case 2://翻页
					wherestr = this.ViewState["wherestr"].ToString();
                    break;
            }
        }
        addTable();

    }

    /// <summary>
    /// 分页属性赋值 标签框
    /// </summary>
    protected void pageinit()
    {
        txtpagenumber.Text = pagenumber.ToString();
        jumppage.Text = pagenumber.ToString();


		Hesion.Brick.Core.User us = new Hesion.Brick.Core.User();
		us = Hesion.Brick.Core.Users.GetUserByNumber(User.Identity.Name);

    	

    	string check = " checkstate = 0 and (nextchecker ='1:" + us.JobName + "' or nextchecker ='2:" + us.DeptName + "' or nextchecker ='3:" + User.Identity.Name + "'";
		string sqr = AuthTransfer.getTransfer(User.Identity.Name);
		if (!string.IsNullOrEmpty(sqr))
		{
			us = Hesion.Brick.Core.Users.GetUserByNumber(sqr);
			check += "  or nextchecker ='1:" + us.JobName + "' or nextchecker ='2:" + us.DeptName + "' or nextchecker ='3:" + sqr + "'";
		}
		check += ")";
		ArrayList al = new ArrayList();
		al.Add(check);
		if (Request["number"] != null && Request["number"].ToString() != string.Empty)
		{
			string str = "number = '" + Request["number"].ToString() + "'";
			al.Add(str);
		}
		wherestr = SQLBuilder.AddToWhere(al);

		this.ViewState["wherestr"] = wherestr;

        Telerik.WebControls.Tab ss = new Telerik.WebControls.Tab(TabTool.getTitle(module) + "增加");
        ss.NavigateUrl = "WorkFLow_Add.aspx?module=" + module;
        RadTabStrip1.Tabs.Add(ss);
        ss = new Telerik.WebControls.Tab(TabTool.getTitle(module) + "查看");
        ss.NavigateUrl = "WorkFLow_View.aspx?module=" + module;
        RadTabStrip1.Tabs.Add(ss);
        ss = new Telerik.WebControls.Tab(TabTool.getTitle(module) + "修改、删除");
        ss.NavigateUrl = "WorkFlow_Edit.aspx?module=" + module;
        RadTabStrip1.Tabs.Add(ss);
        ss = new Telerik.WebControls.Tab(TabTool.getTitle(module) + "审核");
        ss.NavigateUrl = "WorkFlow_Check.aspx?module=" + module;
        ss.ForeColor = System.Drawing.Color.Red;
        RadTabStrip1.Tabs.Add(ss);
    }

    /// <summary>
    /// 分页属性取值
    /// </summary>
    protected void initpage()
    {
        pagenumber = int.Parse(this.txtpagenumber.Text);
    }

    /// <summary>
    /// 读取//WorkFlowModule/Data/Data-Field节点属性
    /// </summary>
    /// <returns>0:数据库没有xml文档记录</returns>
    protected int xmlParse()
    {
        XmlNodeList xlist = FMOP.XParse.xmlParse.XmlGetNode(module, "//WorkFlowModule/Data/Data-Field");
        if (xlist != null)
        {
            fieldName = new string[xlist.Count + 5];
            caption = new string[xlist.Count + 5];
            int i = 0;
            if (!FMOP.XParse.xmlParse.XmlGetNodeInnerXml(module, "//WorkFlowModule/NumberFormat/type").Equals("manual"))
            {
                fieldName[0] = "Number";
                caption[0] = "编号";
                columnstr = fieldName[0] + ",";
                i++;
            }
            foreach (XmlNode item in xlist)
            {
                if ((!XMLHelper.GetSingleString(item, "type", "").Equals("SubTable")) && (!XMLHelper.GetSingleString(item, "type", "").Equals("AddOnFiles")))
                {
                    if (bool.Parse(XMLHelper.GetSingleString(item, "isInListTable", "true")))
                    {
                        fieldName[i] = PageValidate.fieldDot(XMLHelper.GetSingleString(item, "name", ""));
                        caption[i] = XMLHelper.GetSingleString(item, "caption", "");
                        columnstr += fieldName[i] + ",";
                        i++;
                    }
                }
            }
            fieldName[i] = "CreateUser";
            caption[i] = "创建人";
            fieldName[++i] = "CreateTime";
            caption[i] = "创建时间";

            #region 2009.06.29 王永辉添加

            fieldName[++i] = "CreateUserName";
            caption[i] = "创建人姓名";


            fieldName[++i] = "UpdateUserLS";
            caption[i] = "最后修改人";

            #endregion
            columnstr += "CreateUser,CreateTime";
			DataSet dds = DbHelperSQL.Query("select top 1 " + columnstr + " from " + module);

            #region 王永辉 2009.06.29
            if (dds.Tables[0].Rows.Count > 0)
            {
                //添加历史记录======================================================
                string str_Number = dds.Tables[0].Rows[0]["Number"].ToString();
                DataSet dsLS = DbHelperSQL.Query("select top 1 * from SJGXLSJLB where MKYWM = '" + module + "' and BDDH = '" + str_Number + "'");
                DataColumn dcLS = new DataColumn();
                //dcLS.DataType = String.t;
                dcLS.ColumnName = "UpdateUserLS";
                if (dsLS != null && dsLS.Tables[0].Rows.Count > 0)
                {
                    dcLS.DefaultValue = dsLS.Tables[0].Rows[0]["XM"].ToString();
                }

                dds.Tables[0].Columns.Add(dcLS);
                //



                string CreateUser = dds.Tables[0].Rows[0]["CreateUser"].ToString();
                if (CreateUser != null && CreateUser != "")
                {
                    DataSet ds = DbHelperSQL.Query("select Employee_Name from HR_Employees where Number = '" + CreateUser + "'");
                    DataColumn dc = new DataColumn();
                    dc.DataType = ds.Tables[0].Columns["Employee_Name"].DataType;
                    dc.ColumnName = "CreateUserName";
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        dc.DefaultValue = ds.Tables[0].Rows[0]["Employee_Name"].ToString();
                    }
                    dds.Tables[0].Columns.Add(dc);

                }
            }
            #endregion

			fieldType = fieldTool.addfieldType(dds, fieldName);
            return 1;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 执行存储过程，生成DataSet
    /// </summary>
    /// <returns></returns>
    protected DataSet addParamters()
    {

        
        string sqlstr = "SP_pageview_create_xp";
        SqlParameter[] parmes = new SqlParameter[6];
        parmes[0] = new SqlParameter("@tableName", System.Data.SqlDbType.VarChar);
        parmes[0].Value = module;
        parmes[1] = new SqlParameter("@selectColumn", System.Data.SqlDbType.VarChar);
        parmes[1].Value = columnstr;
        parmes[2] = new SqlParameter("@wherestr", System.Data.SqlDbType.VarChar);
		parmes[2].Value = wherestr;
        parmes[3] = new SqlParameter("@keyField", System.Data.SqlDbType.VarChar);
        parmes[3].Value = keyfield;
        parmes[4] = new SqlParameter("@pageNumber", System.Data.SqlDbType.Int);
        parmes[4].Value = pagenumber;
        parmes[5] = new SqlParameter("@pageSize", System.Data.SqlDbType.Int);
        parmes[5].Value = pagesize;
        DataSet ds = DbHelperSQL.RunProcedure(sqlstr, parmes, module);

        #region   2009.06.29 王永辉 查看修改页面添加 添加创建人字段

        DataSet dsNew = null;
        if (ds.Tables[0].Rows.Count > 0)
        {
            /* 创建新的datatable*/
            DataTable dt = new DataTable("CheckTable");
            DataColumn dcNew = null;
            DataRow drNew = null;

            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                dcNew = new DataColumn();
                dcNew.ColumnName = dc.ColumnName;
                dcNew.DataType = dc.DataType;
                dcNew.Caption = dc.Caption;

                dt.Columns.Add(dcNew);

            }
            dcNew = new DataColumn();
            dcNew.ColumnName = "CreateUserName";
            dt.Columns.Add(dcNew);


            dcNew = new DataColumn();
            dcNew.ColumnName = "UpdateUserLS";
            dt.Columns.Add(dcNew);

            /*为新的datatable添加行*/
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                drNew = dt.NewRow();

                for (int j = 0; j < (ds.Tables[0].Columns.Count); j++)
                {

                    drNew[j] = dr[j];
                    /* 获得创建人姓名 */
                    if (dr["CreateUser"] != null && dr["CreateUser"].ToString() != "")
                    {
                        drNew["CreateUserName"] = CreatUserName(dr["CreateUser"].ToString());
                    }


                    string str_Number = dr["Number"].ToString();
                    DataSet dsLS = DbHelperSQL.Query("select top 1 * from SJGXLSJLB where MKYWM = '" + module + "' and BDDH = '" + str_Number + "'");

                    if (dsLS != null && dsLS.Tables[0].Rows.Count > 0)
                    {
                        drNew["UpdateUserLS"] = dsLS.Tables[0].Rows[0]["XM"].ToString();
                    }
                }

                dt.Rows.Add(drNew);
            }

            dsNew = new DataSet();
            dsNew.Tables.Add(dt);

        }
        #endregion

        string dailiren = "无";
        if (Session["daililogin"] != null)
        {
            dailiren = Session["daililogin"].ToString();
        }
        Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "WorkFlow_Check.aspx", "", sqlstr, dailiren);
        return dsNew;
    }

    /// <summary>
    /// 写数据table
    /// </summary>
    protected void addTable()
    {
        #region 生成table
        StringBuilder sb = new StringBuilder();
        //sb.Append(htmlWrite.addTitleTable("销售单"));//页面标题
        sb.Append(htmlWrite.addSelectButton());//查询显示按钮
        sb.Append(htmlWrite.addViewTable("test", addParamters(), caption, 3));
        sb.Append(htmlWrite.addTableScript("test"));
        this.table.InnerHtml = sb.ToString();
        #endregion
        this.txtpagecount.Text = DbHelperSQL.getPageCount(keyfield, pagesize, module, wherestr).ToString();//总页数
    }

    /// <summary>
    /// 写查询table
    /// </summary>
    protected void addSelect()
    {
		this.sele.InnerHtml = htmlWrite.xmlParse(module);
		//this.sele.InnerHtml = htmlWrite.addSelectTable(fieldType, fieldName, caption);
    }

	#region 不带子表sql
	/*/// <summary>
    /// 生成where条件
    /// </summary>
    protected void addSQL()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(" 1=1 ");//where条件头

        #region 循环生成where语句
        for (int i = 0; i < fieldName.Length; i++)
        {
            if (Request["txt" + fieldName[i]] != null && Request["txt" + fieldName[i]].ToString() != string.Empty)
            {
                string str = Request["txt" + fieldName[i]].ToString();
                str = str.Replace("'", "\"");

                #region 判断用户输入是否合法
                if (!PageValidate.selectValidate(fieldType[i], str))
                {
                    MessageBox.Show(this, "您输入有误！");
                    return;
                }
                #endregion

                sb.Append(" and ");
                sb.Append(fieldName[i]);
                string mark = Request["sel" + fieldName[i]].ToString();
                sb.Append(PageValidate.selectValue(mark, str, fieldType[i]));
            }
        }
        #endregion

        wherestr = sb.ToString();
        this.ViewState["wherestr"] = wherestr;
    }*/
	#endregion

	/// <summary>
	/// 生成where条件
	/// </summary>
	protected void addSQL()
	{
		ArrayList alWhere = new ArrayList();
		StringBuilder sb = new StringBuilder();
		//sb.Append(" 1=1 ");//where条件头
		XmlNodeList xlist = FMOP.XParse.xmlParse.XmlGetNode(module, "//WorkFlowModule/Data/Data-Field");
		if (xlist != null)
		{
			DataSet ds1 = FMOP.DB.DbHelperSQL.Query("select * from " + module);
			DataSet ds = new DataSet();
			foreach (XmlNode item in xlist)
			{
				#region 子表
				if (XMLHelper.GetSingleString(item, "type", "").Equals("SubTable"))
				{
					ArrayList al = new ArrayList();
					string subT = XMLHelper.GetSingleString(item, "name", "");
					ds = FMOP.DB.DbHelperSQL.Query("select * from " + subT);
					XmlNodeList sublist = item["subTable"].SelectNodes("Sub-Field");
					foreach (XmlNode item2 in sublist)
					{
						if (Request["txt" + subT + XMLHelper.GetSingleString(item2, "name", "")] != null && Request["txt" + subT + XMLHelper.GetSingleString(item2, "name", "")].ToString() != string.Empty)
						{
							string str = Request["txt" + subT + XMLHelper.GetSingleString(item2, "name", "")].ToString();
							str = str.Replace("'", "\"");

							#region 判断用户输入是否合法
							if (!PageValidate.selectValidate(ds.Tables[0].Columns[XMLHelper.GetSingleString(item2, "name", "")].DataType.ToString(), str))
							{
								MessageBox.Show(this, "您输入有误！");
								return;
							}
							#endregion

							sb = new StringBuilder();
							sb.Append(XMLHelper.GetSingleString(item2, "name", ""));
							string mark = Request["sel" + subT + XMLHelper.GetSingleString(item2, "name", "")].ToString();
							sb.Append(PageValidate.selectValue(mark, str, ds.Tables[0].Columns[XMLHelper.GetSingleString(item2, "name", "")].DataType.ToString()));
							al.Add(sb.ToString());
						}
					}
					if (al.Count > 0)
					{
						sb = new StringBuilder();
						sb.Append(" number in (");
						sb.Append(SQLBuilder.AddWhereToSQL("select parentNumber from " + subT, al));
						sb.Append(" )");
						alWhere.Add(sb.ToString());
					}

					ds.Clear();
				}
				#endregion

				#region 上传文件
				else if (XMLHelper.GetSingleString(item, "type", "").Equals("AddOnFiles"))
				{
					continue;
				}
				#endregion

				#region 主表
				else
				{
					if (Request["txt" + XMLHelper.GetSingleString(item, "name", "")] != null && Request["txt" + XMLHelper.GetSingleString(item, "name", "")].ToString() != string.Empty)
					{
						string str = Request["txt" + XMLHelper.GetSingleString(item, "name", "")].ToString();
						str = str.Replace("'", "\"");

						#region 判断用户输入是否合法
						if (!PageValidate.selectValidate(ds1.Tables[0].Columns[XMLHelper.GetSingleString(item, "name", "")].DataType.ToString(), str))
						{
							MessageBox.Show(this, "您输入有误！");
							return;
						}
						#endregion

						sb = new StringBuilder();
						sb.Append(XMLHelper.GetSingleString(item, "name", ""));
						string mark = Request["sel" + XMLHelper.GetSingleString(item, "name", "")].ToString();
						sb.Append(PageValidate.selectValue(mark, str, ds1.Tables[0].Columns[XMLHelper.GetSingleString(item, "name", "")].DataType.ToString()));
						alWhere.Add(sb.ToString());
					}
				}
				#endregion
			}
		}
		Hesion.Brick.Core.User us = new Hesion.Brick.Core.User();
		us = Hesion.Brick.Core.Users.GetUserByNumber(User.Identity.Name);
		string gdwhere = " checkstate = 0 and (nextchecker ='1:" + us.JobName + "' or nextchecker ='2:" + us.DeptName + "' or nextchecker ='3:" + User.Identity.Name + "')";
		alWhere.Add(gdwhere);
		wherestr = SQLBuilder.AddToWhere(alWhere);
		this.ViewState["wherestr"] = wherestr;
	}

    /// <summary>
    /// 获取该页面的访问权限
    /// </summary>
    protected void addrole()
    {
        string id = User.Identity.Name;
        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
        Hesion.Brick.Core.WorkFlow.UserModuleAuth wfr = new Hesion.Brick.Core.WorkFlow.UserModuleAuth();
        wfr = wf.authentication.GetAuthByUserNumber(User.Identity.Name);

        // FMOP.WF.WorkFlowRole wfr = new FMOP.WF.WorkFlowRole(module, id);
        if (!wf.check.GetCheckedContent())//FMOP.CI.CheckInfo.canCheck(module, id)判断是否有此岗位信息
        {
            if (!wfr.CanView)
            {
                if (!wfr.CanAdd)
                {
                    if (!wfr.CanModify)
                    {
                        Response.Clear();
                        Response.Write("<script language=javascript>window.alert('您没有此模块的权限！');history.back();</script>");
                        Response.End();
                    }
                    else
                    {
                        Page.Response.Redirect("WorkFlow_Edit.aspx?module=" + module);
                    }
                }
                else
                {
                    Page.Response.Redirect("WorkFlow_Add.aspx?module=" + module);
                }
            }
            else
            {
                Page.Response.Redirect("WorkFlow_View.aspx?module=" + module);
            }
        }
        else
        {

        }
    }


    /// <summary>
    /// 获得用创建人姓名
    /// </summary>
    /// <param name="UserID"></param>
    /// <returns></returns>
    protected string CreatUserName(string UserID)
    {
        string CreateUserName = "";
        string StrSql = "select Employee_Name from HR_Employees where Number = '" + UserID + "'";
        DataSet ds = DbHelperSQL.Query(StrSql);
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            CreateUserName = ds.Tables[0].Rows[0][0].ToString();
        }
        return CreateUserName;
    }

}