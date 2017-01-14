using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.XParse;
using Telerik.WebControls;
using Hesion.Brick.Core;
using FMOP.Module;
using FMOP.DB;
using FMOP.Tools;
using ShareLiu;
public partial class ChildWin : System.Web.UI.Page
{
    string module = string.Empty;
    string field = string.Empty;
    string tableName = string.Empty;
    string sqlCmd = string.Empty;
    string TotalType = string.Empty;
    string cfield = string.Empty; //不受干扰的纯字段名 刘杰 2010-09-27
    /// <summary>
    /// 页面加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ArrayList filedList = new ArrayList();
        DataSet result = new DataSet();

        //判断模块名和字段名是否存在
        if (Request["moduleName"] != null && Request["fieldName"] != null && Request["tableName"] != null)
        {
            try
            {
                module = Request["moduleName"].ToString();
                field = Request["fieldName"].ToString();
                tableName = Request["tableName"].ToString();
                //放在这里主要是兼容原来的
                if (Request["CfieldName"] != null)
                {
                    cfield = Request["CfieldName"].ToString();
                }
                else
                {
                    cfield = "";
                }
                //查找sql 语句
                sqlCmd = xmlParse.PopSubSql(module, field, tableName);
                //sqlCmd = this.FilterCitySelect(sqlCmd, module,field);

                //解析XML配置文件，取返回控件名称
                filedList = xmlParse.popXmlparse(module, field, tableName);

                //判断 若SqlCmd 中存在Where时，则添加 and 1=1,若不存在 则添加 where 1=1
                if (Request.Form["txtFlag"] != null && Request.Form["txtFlag"].ToString() == "1")
                {
                    TotalType = Request["typefield"].ToString();
                    if (Request.Form["txtCondition"].ToString() == "1")
                    {
                        ViewState["Condition"] = addSQL();
                        sqlCmd = SQLBuilder.AddWhereToSQL(sqlCmd, ViewState["Condition"].ToString()); 
                        this.txtCondition.Value = "0";
                        ViewState["cmdText"] = sqlCmd;
                    }
                    else
                    {
                        sqlCmd = ViewState["cmdText"].ToString();
                    }
                }

                //执行查询操作
                //原算法result = DbHelperSQL.Query(sqlCmd);
                //新算法，可以调用新的数据库参数
                string conTypes = xmlParse.GetSubTableNodeValue(module,tableName, cfield,"subconStrType");

                //执行查询操作
                if (conTypes == "YFERP"&&cfield!="") //单身的跨数据库调用 刘杰2010-09-27
                {
                    result = DbHelperSQL.ErpQuery(sqlCmd);

                }
                else
                {
                    result = DbHelperSQL.Query(sqlCmd);
                }

                //保存
                if (result != null && filedList.Count > 0)
                {
                    //保存
                    ViewState["setLocal"] = filedList;
                    ViewState["result"] = result;

                    //数据绑定
                    GridBind();
                    CreateQueryLayout(result, filedList);
                }

                //保存日志
                string dailiren = "无";
                if (Session["daililogin"] != null)
                {
                    dailiren = Session["daililogin"].ToString();
                }
                Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "ChildWin.aspx", "", sqlCmd,dailiren);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw ex;
            }
        }
    }

    /// <summary>
    /// 列表数据的绑定与显示
    /// </summary>
    /// <param name="ds">数据源</param>
    /// <param name="setLocal">设置要赋值的字段</param>
    private void GridBind()
    {
        //RadGrid1 要绑定显示的中英文字段名称
        string CNAME = string.Empty;
        string ENAME = string.Empty;
        ArrayList fiedList = new ArrayList();

        if (ViewState["result"] != null && (DataSet)ViewState["result"] != null)
        {
            fiedList = (ArrayList)ViewState["setLocal"];
            this.saleGrid.MasterTableView.AllowMultiColumnSorting = false;
            this.saleGrid.MasterTableView.AllowNaturalSort = false;
            #region 设定绑定列

            GridColumn tmp = saleGrid.Columns[0];
            saleGrid.Columns.Clear();
            saleGrid.Columns.Add(tmp);
            DataSet ulds = new DataSet();
            //判断要绑定字段列表是否为NULL
            if (fiedList != null)
            {
                //遍历每个要绑定的字段
                for (int index = 0; index < fiedList.Count; index++)
                {
                    //列表中格式 : source:中文名:locale
                   // if (fiedList[index].ToString().Split(':').Length == 3)  //原算法
                    if (fiedList[index].ToString().Split(':').Length == 7)   //新算法加入了映射项目
                    {
                        //取字段英文名称
                        ENAME = fiedList[index].ToString().Split(':')[0].ToString();
                        if (fiedList[index].ToString().Split(':')[1].ToString() != "")
                        {
                            //若中文字段不为空时，取中文字段
                            CNAME = fiedList[index].ToString().Split(':')[1].ToString();
                        }
                        else
                        {
                            //若中文字段为空，则headText 用英文字段名表示
                            CNAME = ENAME;
                        }
                    }
                    else
                    {
                        //若格式不正确,则字段取同样的值.
                        ENAME = fiedList[index].ToString();
                        CNAME = fiedList[index].ToString();
                    }

                    GridBoundColumn boundColumn = new GridBoundColumn();
                    boundColumn.UniqueName = ENAME;
                    boundColumn.DataField = ENAME;
                    Type type = Type.GetType("System.String");
                    boundColumn.DataType = type;
                    boundColumn.HeaderText = CNAME;
                    boundColumn.SortExpression = ENAME;
                    boundColumn.FilterListOptions = GridFilterListOptions.VaryByDataType;
                    this.saleGrid.MasterTableView.Columns.Add(boundColumn);
                }
            }
            #endregion
            //绑定Grid数据
#region //老算法，2011-01-17

            //ulds = (DataSet)ViewState["result"];
            //for (int j = 0; j < fiedList.Count; j++)   //遍历列配置
            //{
            //    if (fiedList[j].ToString().Split(':').Length == 7)
            //    {
            //        if (fiedList[j].ToString().Split(':')[3].ToString() != "" && fiedList[j].ToString().Split(':')[3].ToString() != null) //是否存在链接列配置
            //        {
            //            string urlstr = fiedList[j].ToString().Split(':')[3].ToString();
            //            string mname = fiedList[j].ToString().Split(':')[4].ToString();
            //            string ptext = fiedList[j].ToString().Split(':')[5].Trim().ToString();
            //            string poptext = fiedList[j].ToString().Split(':')[6].Trim().ToString();

            //            for (int p = 0; p < ulds.Tables[0].Rows.Count; p++) //遍历dataset
            //            {
            //                for (int q = 0; q < ulds.Tables[0].Columns.Count; q++) //遍历列并进行对应
            //                {
            //                    if (ulds.Tables[0].Columns[q].ColumnName.ToString() == fiedList[j].ToString().Split(':')[0].ToString())
            //                    {
            //                        if (ulds.Tables[0].Rows[p][q].ToString().IndexOf("<a  href=") >= 0)
            //                        {

            //                        }
            //                        else  //只能绑定一次
            //                        {
            //                            ulds.Tables[0].Rows[p][q] = "<a  href='" + urlstr + "?module=" + mname + "&number=" + ulds.Tables[0].Rows[p][q].ToString() + "&posttext=" + Server.UrlEncode(ptext) + "' title='" + poptext + "' target='_bank'>" + ulds.Tables[0].Rows[p][q].ToString() + "</a>";
            //                        }
            //                    }

            //                }


            //            }

            //        }
            //    }

            //}
#endregion 
            
             #region 加入一个连接列 并绑定

                ulds = (DataSet)ViewState["result"];
                DataColumn dc = new DataColumn("全链接"); //新建一列
                dc.DataType = Type.GetType("System.String");
                if (ulds.Tables[0].Columns.IndexOf("全链接") > 0)
                {

                }
                else
                {
                    ulds.Tables[0].Columns.Add(dc);
                }
                GridBoundColumn boundColumn2 = new GridBoundColumn();
                boundColumn2.UniqueName = "全链接";
                boundColumn2.DataField = "全链接";
                boundColumn2.DataType = Type.GetType("System.String");
                boundColumn2.HeaderText = "全链接操作";
                boundColumn2.SortExpression = "";
                boundColumn2.FilterListOptions = GridFilterListOptions.VaryByDataType;
                this.saleGrid.MasterTableView.Columns.Add(boundColumn2);
                #endregion
                #region 循环加入连接数据
                string allLinkString = "";
                string addLink = "否";  //是否启用全连接控制
                for (int p = 0; p < ulds.Tables[0].Rows.Count; p++) //遍历dataset，循环行
                {
                    for (int q = 0; q < ulds.Tables[0].Columns.Count; q++) //遍历列并进行对应
                    {
                        for (int j = 0; j < fiedList.Count; j++)   //遍历列配置。
                        {
                            if (fiedList[j].ToString().Split(':').Length == 7)
                            {
                                if (fiedList[j].ToString().Split(':')[3].ToString() != "" && fiedList[j].ToString().Split(':')[3].ToString() != null) //是否存在链接列配置
                                {
                                    string urlstr = fiedList[j].ToString().Split(':')[3].ToString();
                                    string mname = fiedList[j].ToString().Split(':')[4].ToString();
                                    string ptext = fiedList[j].ToString().Split(':')[5].Trim().ToString();
                                    string poptext = fiedList[j].ToString().Split(':')[6].Trim().ToString();
                                    if (ulds.Tables[0].Columns[q].ColumnName.ToString() == fiedList[j].ToString().Split(':')[0].ToString())
                                    {

                                        //Response.Write(allLinkString.ToString());
                                        if (ulds.Tables[0].Rows[p][q].ToString().IndexOf("<a  href=") >= 0)
                                        {

                                        }
                                        else  //只能绑定一次
                                        {
                                            allLinkString += ":" + mname + ":" + ulds.Tables[0].Rows[p][q].ToString();
                                            ulds.Tables[0].Rows[p][q] = "<a  href='" + urlstr + "?module=" + mname + "&number=" + ulds.Tables[0].Rows[p][q].ToString() + "&posttext=" + Server.UrlEncode(ptext) + "' title='" + poptext + "' target='_bank'>" + ulds.Tables[0].Rows[p][q].ToString() + "</a>";
                                            addLink = "是";
                                        }

                                    }

                                }

                            }
                        } //遍历配置结束  


                    }//循环ds列结束该行，执行下一行

                    //Response.Write(allLinkString);
                    if(addLink=="是")
                    {

                        if (ulds.Tables[0].Rows[p][ulds.Tables[0].Columns.Count - 1].ToString().IndexOf("<a href=") >= 0)
                        {

                        }
                        else
                        {
                            ulds.Tables[0].Rows[p][ulds.Tables[0].Columns.Count - 1] = "<a href='ViewAll.aspx?LinkString=" + allLinkString + "'  target='_bank'>" + "所有连接" + "</a>";
                        }
                        allLinkString = "";
                    }

                }
#endregion

            //saleGrid.DataSource = (DataSet)ViewState["result"]; //原
            saleGrid.DataSource =ulds; //新算法 liujie 2010-09-27
            saleGrid.DataBind();
        }
    }

    /// <summary>
    /// 分页实现
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void RadGrid1_PageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        saleGrid.CurrentPageIndex = e.NewPageIndex;
        this.GridBind();
    }

    /// <summary>
    /// 设置执行选择一行数据后的操作
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void saleGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        string local = string.Empty;
        string source = string.Empty;
        string returnParam = string.Empty;
        int index = 0;
        ArrayList arylist = new ArrayList();
        DataRowView chooseRow = (DataRowView)e.Item.DataItem;
        this.saleGrid.MasterTableView.AllowNaturalSort = false;
        this.saleGrid.MasterTableView.SortExpressions.AllowNaturalSort = false;

        if ((e.Item.ItemType == Telerik.WebControls.GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
        {
            ImageButton ImgBtn = (ImageButton)e.Item.FindControl("imgbtn");
            arylist = (ArrayList)ViewState["setLocal"];
            if (ImgBtn != null && arylist != null)
            {
                //循环字段，查找要返回的字段
                for (index = 0; index < arylist.Count; index++)
                {
                    //在产生时，以源字段英文名：源字段中文名：目的字段格式存放
                   // if (arylist[index].ToString().Split(':').Length == 3) //原算法
                  if (arylist[index].ToString().Split(':').Length == 7)
                    {
                        source = arylist[index].ToString().Split(':')[0].ToString();
                        local = arylist[index].ToString().Split(':')[2].ToString();
                        if (chooseRow[source] != null)
                        {
                            string strs = chooseRow[source].ToString();
                            strs = CheckALL.getQJStr(strs); //特殊处理 刘杰2010-09-27
                            if (index < arylist.Count - 1)
                            {
                               // returnParam += local + "=" + chooseRow[source] + ",";  //原
                                returnParam += local + "=" +strs.Trim()+ ",";
                            }
                            else
                            {
                              //   returnParam += local + "=" + chooseRow[source];     //原
                                returnParam += local + "=" +strs.Trim();
                            }
                        }
                    }
                }

                //注册客户端脚本事件
                ImgBtn.OnClientClick = "return setValue(\"" + returnParam.Trim() + "\")";
            }
        }
    }
    /// <summary>
    /// 产生查询的表格
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="filedList"></param>
    private void CreateQueryLayout(DataSet ds, ArrayList filedList)
    {
        int Total = 0;
        int arrayIndex = 0;
        string field = string.Empty;
        string CName = string.Empty;

        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        {
            Total = filedList.Count;
            string[] fieldType = new string[Total];
            string[] fieldName = new string[Total];
            string[] caption = new string[Total];

            if (filedList != null)
            {
                for (int i = 0; i < filedList.Count; i++)
                {
                    field = filedList[i].ToString().Split(':')[0].ToString();

                    CName = filedList[i].ToString().Split(':')[1].ToString();

                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        if (field.ToLower() == dc.ColumnName.ToString().ToLower())
                        {
                            fieldType[arrayIndex] = dc.DataType.ToString();
                            fieldName[arrayIndex] = dc.ColumnName.ToString();
                            TotalType += dc.ColumnName.ToString() + ":" + dc.DataType.ToString() + ",";
                            if (CName == "")
                            {
                                caption[arrayIndex] = dc.Caption.ToString();
                            }
                            else
                            {
                                caption[arrayIndex] = CName;
                            }
                            arrayIndex++;
                            break;
                        }
                    }
                }

                TotalType = TotalType.Substring(0, TotalType.Length - 1);
                this.query.InnerHtml = htmlWrite.addSelectTable(fieldType, fieldName, caption).Replace("formsubmit()", "executeQualification()") + formSubmit();

            }
        }
    }

    /// <summary>
    /// 添加JS代码
    /// </summary>
    private string formSubmit()
    {
        string jsCode = "";
        jsCode += "<script language=\"javaScript\" type=\"text/javascript\">\r\n";
        jsCode += "\tfunction formsubmit()\r\n";
        jsCode += "\t{\r\n";
        jsCode += "\t var Flag;\r\n";
        jsCode += "\t Flag = document.getElementById('txtFlag').value\r\n";
        jsCode += "\t if(Flag == \"1\")\r\n";
        jsCode += "\t {\r\n";
       // jsCode += "\tform1.action=\"ChildWin.aspx?moduleName=" + module + "&fieldName=" + field + "&tableName=" + tableName + "&typefield=" + TotalType + "\";\r\n";
        jsCode += "\tform1.action=\"ChildWin.aspx?moduleName=" + module + "&fieldName=" + field + "&CfieldName=" + cfield + "&tableName=" + tableName + "&typefield=" + TotalType + "\";\r\n";
        jsCode += "\t}\r\n";
        jsCode += "\t else\r\n";
        jsCode += "\t {\r\n";
       // jsCode += "\tform1.action=\"ChildWin.aspx?moduleName=" + module + "&fieldName=" + field + "&tableName=" + tableName + "\";\r\n";
        jsCode += "\tform1.action=\"ChildWin.aspx?moduleName=" + module + "&fieldName=" + field + "&CfieldName=" + cfield + "&tableName=" + tableName + "\";\r\n";
        jsCode += "\t}\r\n";
        jsCode += "\tform1.submit();\r\n";
        jsCode += "\t}\r\n";
        jsCode += "</script>\r\n";
        return jsCode;
    }

    /// <summary>
    /// 添加查找条件
    /// </summary>
    /// <returns></returns>
    private string addSQL()
    {
        string condition = string.Empty;
        string name = string.Empty;
        string type = string.Empty;

        try
        {
            string[] fieldName = TotalType.Split(',');
            #region 循环生成where语句
            for (int i = 0; i < fieldName.Length; i++)
            {
                name = fieldName[i].Split(':')[0].ToString();
                type = fieldName[i].Split(':')[1].ToString();
                if (Request["txt" + name] != null && Request["txt" + name].ToString() != string.Empty)
                {
                    string str = Request["txt" + name].ToString();
                    str = str.Replace("'", "\"");

                    #region 判断用户输入是否合法
                    if (!PageValidate.selectValidate(type, str))
                    {
                        throw new Exception();
                    }
                    #endregion

                    condition += " and ";
                    condition += name;
                    string mark = Request["sel" + name].ToString();
                    condition += PageValidate.selectValue(mark, str, type);
                }
            }
            #endregion
        }
        catch
        {
            MessageBox.Show(this, "您的输入条件有误！");
        }
        return condition;
    }

    /*
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    private string FilterCitySelect(string cmd, string moduleName,string fieldName)
    {
        string whereStr = "";
        string city = "";
        ArrayList whereList = new ArrayList();
        if (isExists(moduleName,fieldName))
        {
            city = GetCity();
            if (city.Contains("客户服务中心"))
            {
                whereStr = " khgx_khgl.city='" + city + "'";
                whereList.Add(whereStr);
                cmd = SQLBuilder.AddWhereToSQL(cmd, whereList);
            }
        }

        return cmd;
    }

    /// <summary>
    /// 是否存在于cityselect中
    /// </summary>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    private bool isExists(string moduleName,string fieldName)
    {
        int rowCount = 0;
        string cmdText = "select count(*) from cityselect where moduleName ='" + moduleName + "' and field='"+fieldName+"'";

        rowCount = (int)DbHelperSQL.GetSingle(cmdText);

        return rowCount > 0 ? true : false;
    }

    /// <summary>
    /// 取当前员工的城市中心
    /// </summary>
    /// <returns></returns>
    private string GetCity()
    {
        string city = "";
        string cmdText = @"  select hr_dept.DeptName from hr_employees inner join hr_jobs on  hr_employees.JobNo = hr_jobs.Number 
                            inner join hr_dept on hr_jobs.Dept_No = hr_dept.Number where hr_employees.Number='" + User.Identity.Name + "'";

        city = DbHelperSQL.GetSingle(cmdText).ToString();
        return city;
    }
     */
}