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
using Infragistics.WebUI.UltraWebGrid;

public partial class WorkFlow_View_Detail : System.Web.UI.Page
{
    #region 全局变量
    /*private string[] caption = new string[] { };
    private string[] SubTable = new string[] { };
    private string[] AddOnFiles = new string[] { };
    private Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
    private string[] subcolumn;
    private string keyfield = "Number";
    private string columnstr;

    private int[] Nodetype;*/

    private string module;
    private string number;
    private string from = "view";

    private bool flag;
    
    #endregion

    #region <%=   %>
    private string endAudit;
    private string notAudit;
    private string auditState;

    public string EndAudit
    {
        get 
        {
            return endAudit;
        }
        set 
        {
            endAudit = value;
        }
    }

    public string NotAudit
    {
        get
        {
            return notAudit;
        }
        set
        {
            notAudit = value;
        }
    }

    public string AuditState
    {
        get
        {
            return auditState;
        }
        set
        {
            auditState = value;
        }
    }

    #endregion

    /// <summary>
    /// 页面加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string gaoji = "";
        if (Request["gaoji"] == null)
        {
            gaoji = "";
        }
        else
        {
            gaoji = Request["gaoji"].ToString();
        }

        if (!Page.IsPostBack || gaoji == "")

        {
            if (Request["module"] != null && Request["module"].ToString() != string.Empty)
            {
                module = Request["module"].ToString();
                if (Request["number"] != null && Request["number"].ToString() != string.Empty)
                {
                    number = Request["number"].ToString();
                    if (Request["from"] != null)
                    {
                        from = Request["from"].ToString();
                    }
                    
                    if (gaoji != "")
                    {
                        //显示高级报表;
                        //Response.Write(gaoji);
                        showsubtable( Convert.ToInt32(gaoji));
                        UltraWebGrid1.Visible = true;
                        Label_title.Visible = true;
                        //this.Label1.Visible = true;
                        if (User.Identity.Name == "admin")
                        {
                            this.btnOut.Visible = true;
                        }
                    }
                    else
                    {
                        if (xmlParse() != 0)
                        {
                            pageinit();
                            addtable();
                            /*if (from.Equals("view"))
                            {
                                addtable();
                            }*/
                        }
                        else
                        {
                            Response.Clear();
                            Response.Write("<script language=javascript>window.alert('没有此项报表！');window.close();</script>");
                            Response.End();
                            return;
                        }
                    }
                    if(Page.IsPostBack && gaoji == "")
                    {
                        module = this.txtmodule.Text;
                        number = this.txtnumber.Text;
                        docheck();
                    }

                }
                else
                {
                    Response.Clear();
                    Response.Write("<script language=javascript>window.alert('没有获取到单号参数！');window.close();</script>");
                    Response.End();
                    return;
                }
            }
            else
            {
                Response.Clear();
                Response.Write("<script language=javascript>window.alert('没有获取到模块参数！');window.close();</script>");
                Response.End();
                return;
            }
        }
        else
        {
            if (gaoji == "")
            {
                module = this.txtmodule.Text;
                number = this.txtnumber.Text;
                docheck();
            }
        }
    }

    /// <summary>
    /// 属性保存
    /// </summary>
    protected void pageinit()
    {
        this.txtnumber.Text = number;
        this.txtmodule.Text = module;
    }


    /// <summary>
    /// 显示子表,新版控件,by galaxy
    /// </summary>
    /// <returns></returns>
    protected int showsubtable(int i_str)
    {
        string upsql = "select sourcename,localname from fileup where number ='" + number + "' and moduleName = '" + module + "'";
        DataSet ds = DbHelperSQL.Query(upsql);

        string dailiren = "无";
        if (Session["daililogin"] != null)
        {
            dailiren = Session["daililogin"].ToString();
        }
        //Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "WorkFlow_View_Detail.aspx", number, upsql,dailiren);
        XmlNodeList xlist = FMOP.XParse.xmlParse.XmlGetNode(module, "//WorkFlowModule/Data/Data-Field");
        if (xlist != null)
        {
            for (int i = 0; i < xlist.Count; i++)
            {
                XmlNode item = xlist[i];

                #region 绑定子表
                if (i == i_str)
                {

                    string subT = XMLHelper.GetSingleString(item, "name", "");
                    XmlNodeList sublist = item["subTable"].SelectNodes("Sub-Field");
                    string[] subcaption = new string[sublist.Count];
                    string subC = "";
                    int j = 0;
                    foreach (XmlNode item2 in sublist)
                    {
                        subcaption[j] = XMLHelper.GetSingleString(item2, "caption", "");
                        subC += PageValidate.fieldDot(XMLHelper.GetSingleString(item2, "name", "")) + ",";
                        j++;
                    }
                    subC = subC.Trim().Substring(0, subC.Length - 1);

                    DataSet dstemp = new DataSet();
                    dstemp = addSubData(subC, subT);
                    
                    for (int p = 0; p < dstemp.Tables[0].Columns.Count; p++ )
                    {
                        dstemp.Tables[0].Columns[p].Caption = subcaption[p].ToString();
                    }
                    UltraWebGrid1.DataSource = dstemp;
                    UltraWebGrid1.DataBind();
                    Label_title.Text = XMLHelper.GetSingleString(item, "caption", "");
                    //addSubData(subC, subT)这是数据集
                    //subcaption这是列名
                    //XMLHelper.GetSingleString(item, "caption", "")这是标题
                    break;
                }
                #endregion







            }
            return 1;
        }
        else
        {
            return 0;
        }
    }




    /// <summary>
    /// 读取//WorkFlowModule/Data/Data-Field节点属性
    /// </summary>
    /// <returns>0:数据库没有xml文档记录</returns>
    protected int xmlParse()
    {
        string upsql = "select sourcename,localname from fileup where number ='" + number + "' and moduleName = '" + module + "'";
        DataSet ds = DbHelperSQL.Query(upsql);

        string dailiren = "无";
        if (Session["daililogin"] != null)
        {
            dailiren = Session["daililogin"].ToString();
        }
        Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "WorkFlow_View_Detail.aspx", number, upsql, dailiren);
        XmlNodeList xlist = FMOP.XParse.xmlParse.XmlGetNode(module, "//WorkFlowModule/Data/Data-Field");
        if (xlist != null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(htmlWrite.addRingTop());
            sb.Append(htmlWrite.addTableHeader2());
            for (int i = 0; i < xlist.Count; i++)
            {
                XmlNode item = xlist[i];

                #region 子表
                if (XMLHelper.GetSingleString(item, "type", "").Equals("SubTable"))
                {
                    sb.Append("<tr>");
                    sb.Append("<td align=\"center\" border=\"0px\" colspan=\"4\">");

                    string subT = XMLHelper.GetSingleString(item, "name", "");
                    XmlNodeList sublist = item["subTable"].SelectNodes("Sub-Field");
                    string[] subcaption = new string[sublist.Count];
                    string subC = "";
                    int j = 0;
                    foreach (XmlNode item2 in sublist)
                    {
                        subcaption[j] = XMLHelper.GetSingleString(item2, "caption", "");
                        subC += PageValidate.fieldDot(XMLHelper.GetSingleString(item2, "name", "")) + ",";
                        j++;
                    }
                    subC = subC.Trim().Substring(0, subC.Length - 1);
                    sb.Append(XMLHelper.GetSingleString(item, "caption", "") + " - <a href=\"#\" style=\"text-decoration: none;\" onclick = \"openview_gaoji('" + number + "','" + i.ToString() + "');\">(查看高级报表)</a><br />");
                    sb.Append(htmlWrite.addViewTable("test" + i.ToString(), addSubData(subC, subT), subcaption, 0));
                    sb.Append(htmlWrite.addTableScript("test" + i.ToString()));

                    sb.Append("</td>");
                    sb.Append("</tr>");
					continue;
                }
                #endregion

                #region 上传文件
                if (XMLHelper.GetSingleString(item, "type", "").Equals("AddOnFiles"))
                {
                    string url = @"Upload\";
                    sb.Append("<tr height=\"30px\">");
                    sb.Append("<td class=\"tdLeft\">" + XMLHelper.GetSingleString(item, "caption", "") + "：</td>");
                    sb.Append("<td style=\"text-align:left\" border=\"0px\" colspan=\"3\">");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int desc = 0; desc < ds.Tables[0].Rows.Count; desc++)
                        {
                            sb.Append("<a target=\"_blank\" href=\"" + url + ds.Tables[0].Rows[desc]["localname"].ToString() + "\">" + ds.Tables[0].Rows[desc]["sourcename"].ToString() + "</a>&nbsp; &nbsp; &nbsp; &nbsp;");
                        }                        
                    }
                    sb.Append("</td>");
                    sb.Append("</tr>");
					continue;
                }
                #endregion

                #region 主表
                if (XMLHelper.GetSingleString(item, "type", "").Equals("MutiLineTextBox"))
                {
                    sb.Append("<tr height=\"30px\">");
                    sb.Append("<td class=\"tdLeft\">" + XMLHelper.GetSingleString(item, "caption", "") + "：</td>");
                    sb.Append("<td style=\"text-align:left;font-size:14px;color:green\" width=\"80%\"  colspan=\"3\">" + addN(XMLHelper.GetSingleString(item, "name", "")) + "</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    if(i<xlist.Count-1)
                    {
                        if (typeComp(XMLHelper.GetSingleString(xlist[i + 1], "type", "")))
                        {
                            sb.Append("<tr height=\"30px\">");
                            sb.Append("<td class=\"tdLeft\">" + XMLHelper.GetSingleString(item, "caption", "") + "：</td>");
                            sb.Append("<td class=\"tdRight\">" + addN(XMLHelper.GetSingleString(item, "name", "")) + "</td>");
                            sb.Append("<td class=\"tdLeft\">" + XMLHelper.GetSingleString(xlist[i + 1], "caption", "") + "：</td>");
                            sb.Append("<td class=\"tdRight\">" + addN(XMLHelper.GetSingleString(xlist[i + 1], "name", "")) + "</td>");
                            sb.Append("</tr>");
                            i++;
                        }
                        else
                        {
                            sb.Append("<tr height=\"30px\">");
                            sb.Append("<td class=\"tdLeft\">" + XMLHelper.GetSingleString(item, "caption", "") + "：</td>");
                            sb.Append("<td class=\"tdRight\">" + addN(XMLHelper.GetSingleString(item, "name", "")) + "</td>");
                            sb.Append("<td class=\"tdLeft\"></td>");
                            sb.Append("<td class=\"tdRight\"></td>");
                            sb.Append("</tr>");
                        }
                    }
                    else
                    {
                        sb.Append("<tr height=\"30px\">");
                        sb.Append("<td class=\"tdLeft\">" + XMLHelper.GetSingleString(item, "caption", "") + "：</td>");
                        sb.Append("<td class=\"tdRight\">" + addN(XMLHelper.GetSingleString(item, "name", "")) + "</td>");
                        sb.Append("<td class=\"tdLeft\"></td>");
                        sb.Append("<td class=\"tdRight\"></td>");
                        sb.Append("</tr>");
                    }
                }
                #endregion

                

            }
            #region 审批项

            Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
            if (wf.check != null)
            {
                flag = wf.check.GetCheckedContent();
            }
            //flag = FMOP.CI.CheckInfo.ifEnsChecker(module);
            if (flag)
            {
                if (from.Equals("view"))
                {

                    addAudit();
                    sb.Append("<tr  height=\"30px\">");
                    sb.Append("<td class=\"tdLeft\">已审人员：</td>");
                    sb.Append("<td class=\"tdRight\">" + EndAudit + "</td>");
                    sb.Append("<td class=\"tdLeft\">未审人员：</td>");
                    sb.Append("<td class=\"tdRight\">" + NotAudit + "</td>");
                    sb.Append("</tr><tr>");
                    sb.Append("<td class=\"tdLeft\">审批状态：</td>");
                    sb.Append("<td class=\"tdRight\">" + AuditState + "</td>");
                    sb.Append("<td style=\"text-align:right\" colspan=\"2\"><input type=\"button\" id=\"disButton\" name=\"disButton\" value=\"审批详情\" onclick=\"disp()\"/>&nbsp; &nbsp; &nbsp; &nbsp;</td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr  height=\"30px\">");
					sb.Append("<td class=\"tdLeft\">审批意见：</td>");
                    sb.Append("<td style=\"text-align:left\" colspan=\"3\"><textarea id=\"remark\" name=\"remark\" rows=\"5\" style=\"border: black 1px groove;width:90%\"></textarea></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr  height=\"30px\">");
					sb.Append("<td style=\"text-align:right\" colspan=\"4\"><input type=\"button\" id=\"disButton\" name=\"disButton\" value=\"审批详情\" onclick=\"disp()\"/>&nbsp; &nbsp;<input type=\"button\" id=\"Button11\" value=\"通过\" onclick=\"passed()\"/>&nbsp; &nbsp;<input type=\"button\" id=\"Button22\" value=\"驳回\" onclick=\"unpass()\"/>&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;</td>");
                    sb.Append("</tr>");
                }
            }
            #endregion
            sb.Append(htmlWrite.addTableFooter());
            sb.Append(htmlWrite.addRingFoot());
            this.table.InnerHtml = sb.ToString();
            return 1;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 子表数据
    /// </summary>
    /// <param name="subcolunmn"></param>
    /// <param name="subT"></param>
    /// <returns></returns>
    protected DataSet addSubData(string subcolunmn, string subT)
    {
        string subsql = "select " + subcolunmn + " from " + subT + " where parentnumber = '" + number + "'";

        string dailiren = "无";
        if (Session["daililogin"] != null)
        {
            dailiren = Session["daililogin"].ToString();
        }
        //Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "WorkFlow_View_Detail.aspx", number, subsql, dailiren);
        return DbHelperSQL.Query(subsql);
    }

    /// <summary>
    /// 查询字段内容
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    protected string addN(string column) 
    {
        string sql = "select " +column+ " from " +module+ " where number = '" + number+ "'";

        string dailiren = "无";
        if (Session["daililogin"] != null)
        {
            dailiren = Session["daililogin"].ToString();
        }
        //Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "WorkFlow_View_Detail.aspx", number, sql, dailiren);
        object ob = DbHelperSQL.GetSingle(sql);
        if (ob != null)
        {

			return FMOP.Tools.PageValidate.output(ob);
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 判断类型
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    protected bool typeComp(string str)
    {
        string[] typeName = new string[] { "SubTable", "AddOnFiles", "MutiLineTextBox" };
        int equal = 0;
        for(int i = 0;i<3;i++)
        {
            if(!str.Equals(typeName[i]))
            {
                equal+=1;
            }
        }
        if (equal == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    protected void addAudit()
    {
        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
        if (wf.check != null)
        {
            EndAudit = wf.check.GetCheckedUserName(number);
            NotAudit = wf.check.GetUnCheckedJobName(number, 1);
            AuditState = wf.check.GetCheckState(number);
        }
        //FMOP.CG.CheckinfoGather cg = new FMOP.CG.CheckinfoGather(module, number);
        //EndAudit = cg.AllCheckJobs;
        //NotAudit = cg.NotCheckRole;
        //AuditState = cg.CheckState;
    }


    protected void addtable()
    {
        if (flag)
        {
            string[] strhead = new string[] { "审批人", "时间", "结果", "审批意见" };
            this.sele.InnerHtml = htmlWrite.addAuditTable(strhead, FMOP.RCI.ReadCheckInfo.sqlCheckInfo(module, number), "test1");
        }
        else
        { }
    }


    /// <summary>
    /// 审批流程
    /// </summary>
    protected void docheck()
    {
        string Dailiren = "无";
       

        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(this.txtmodule.Text);
        bool ispass = bool.Parse (this.IsPass.Text);
        if (wf.check != null)
        {
          
            //2009.06.05 王永辉 添加参数 Dailiren
            if (Session["daililogin"] != null)
            {
                Dailiren = Session["daililogin"].ToString();
            }
           // wf.check.DoCheck(this.txtnumber.Text, User.Identity.Name, ispass, Request["remark"].ToString(), Dailiren); //原程序
            wf.check.DoCheck2(this.txtnumber.Text, User.Identity.Name, ispass, Request["remark"].ToString(), Dailiren);   //新程序，执行短信处理 刘杰 2010-07-12
        }

        //string dailiren = "无";
        //if (Session["daililogin"] != null)
        //{
        //    dailiren = Session["daililogin"].ToString();
        //}
        //Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "WorkFlow_View_Detail.aspx", number, "Hesion.Brick.Core.WorkFlow.WorkFlowModule.check.DoCheck()", Dailiren);
        /////第一步
        //FMOP.CI.CheckInfo.AddCheckInfo(this.txtmodule.Text, this.txtnumber.Text, User.Identity.Name, int.Parse(this.IsPass.Text), Request["remark"].ToString());
        /////第二步
        //FMOP.UNC.UpateNextCheck.UpateNextChecker(this.txtmodule.Text, this.txtnumber.Text, FMOP.CI.CheckInfo.getJobName(User.Identity.Name), int.Parse(this.IsPass.Text));
        /////第三步
        //FMOP.WN.Warning.CreateWarning(this.txtmodule.Text, this.txtnumber.Text, 4, getCreateUser(this.txtmodule.Text, this.txtnumber.Text));
        /////第四步
        //FMOP.Common.MessageBox.Show(this, "审批流程结束！");
     //   Response.Clear();
        //Response.Write("<script>window.close();</script>");
        //Response.Write("<script language=\"javascript\">debugger;alert('审批流程结束！');self.close();</script>");
        //ApplicationInstance.CompleteRequest();
       // Response.End();
        Response.Clear();
        //Response.Write("<script language=javascript>window.parent.Dialog.alert('审批流程结束！');window.parent.rightFrame.location.reload();</script>");
        Response.Write("<script language=javascript>parent.document.getElementById('_DialogDiv_Diag1').style.display = 'none';parent.document.getElementById('_DialogBGDiv').style.display = 'none';window.parent.Dialog.alert('审批流程结束！');window.parent.rightFrame.location.reload();</script>");
        Response.End();
    }

    #region 2009.04.23 王永辉添加 作用：导出Excelwen文件
    /// <summary>
    /// 导出按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        //this.UltraWebGridExcelExporter1.ExcelStartRow = 1;
        this.UltraWebGridExcelExporter1.DownloadName = "report.xls";
        this.UltraWebGridExcelExporter1.Export(this.UltraWebGrid1);

       

    }

   /// <summary>
   /// 在执行导出操作前触发此事件
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void UltraWebGridExcelExporter1_BeginExport(object sender, Infragistics.WebUI.UltraWebGrid.ExcelExport.BeginExportEventArgs e)
    {
        //// Add a title to the report 为报表添加标题
        //e.CurrentWorksheet.Rows[0].Cells[0].Value = "Active customers (" + DateTime.Now.Year + ")";
        //e.CurrentWorksheet.Rows[0].Cells[0].CellFormat.Font.Color = System.Drawing.Color.Crimson;
        //e.CurrentWorksheet.Rows[0].Cells[0].CellFormat.Font.Height = 400;

        //// Add a caption with some more information
        //e.CurrentWorksheet.Rows[1].Cells[0].Value = "A listing of customers that have purchased products in the first quarter of " + DateTime.Now.Year;

        //// Set UltraWebGridExcelExporter1.ExcelStartRow in the event that
        //// trigger the export to choose which line of the workbook that 
        //// the grid export starts on

    }
    protected void UltraWebGridExcelExporter1_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.ExcelExport.ExcelExportInitializeRowEventArgs e)
    {
        //  添加额外的样式去描述文件中的行
        //if ((string)e.Row.Cells.FromKey("年份").Value == "2009") 
        //{
        //    // 当此行符合要求的条件时显示高亮样式（颜色pink）
        //    e.Row.Style.BackColor = System.Drawing.Color.Pink;
        //}
    }
    #endregion
    protected void UltraWebGrid1_InitializeLayout(object sender, Infragistics.WebUI.UltraWebGrid.LayoutEventArgs e)
    {
        // enable load on demand so sorting and filtering can be done without a postback
        UltraWebGrid1.Browser = Infragistics.WebUI.UltraWebGrid.BrowserLevel.Auto;
        e.Layout.LoadOnDemand =Infragistics.WebUI.UltraWebGrid.LoadOnDemand.Automatic;

        // enable the filter row
        e.Layout.FilterOptionsDefault.AllowRowFiltering = RowFiltering.OnServer;
        e.Layout.FilterOptionsDefault.FilterUIType = FilterUIType.FilterRow;

        // enable sorting and grouping
        e.Layout.AllowSortingDefault = AllowSorting.Yes;
        e.Layout.ViewType = ViewType.OutlookGroupBy;

        // enable column rearranging
        e.Layout.AllowColumnMovingDefault = AllowColumnMoving.OnClient;

    }


   
}
