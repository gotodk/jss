using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.CollectionNode;
using System.Xml;
using System.Text;
using FMOP.SD;
using FMOP.Check;
using FMOP.Execute;
using FMOP.XParse;
using FMOP.DB;
using FMOP.FILE;
using FMOP.XHelp;
using System.Collections.Generic;
using Hesion.Brick.Core;

using Hesion.Brick.Core.WorkFlow;
public partial class WorkFlow_Update_Save : System.Web.UI.Page
{
    int MasterNo = 0;               //主表参数个数
    int SubNo = 0;                  //子表参数个数 
    int FileNo = 0;                 //保存上传控件个数
    string KeyNumber = "";          //主键单号
    string module = ""; //模块名称
    HttpFileCollection files = null;    //上传文件集合

    /// <summary>
    /// 修改页面事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //接收主键
            if (Request["module"] != null && Request["module"].ToString() != "" && Request["KeyNumber"] != null && Request["KeyNumber"].ToString() != "")
            {
                module = Request["module"].ToString();
                KeyNumber = Request["KeyNumber"].ToString();

                //判断是否审核过,再进行修改
                if (IsCheckState())
                {
                    //参数集合类对象
                    CollectionNodeObject NodeCollection = new CollectionNodeObject(module);

                    //取主表与从表要接收的参数个数 
                    SetParameterCount(NodeCollection);

                    //产生要接收的参数数组
                    SaveDepositary[] MasterNodeArray = new SaveDepositary[MasterNo];
                    SaveDepositary[] FilesNodeArray = new SaveDepositary[FileNo];

                    //生成参数数组对象
                    MasterNodeArray = NodeCollection.GetMasterCollection();
                    FilesNodeArray = NodeCollection.GetFileCollection();


                    //创建泛型对象
                    List<CommandInfo> updateToDb = new List<CommandInfo>();
                    //创建commandInfo对象和commandInfo数组
                    CommandInfo cmdInfo = new CommandInfo();
                    try
                    {
                        //要修改记录的主键编号
                        ExecuteDB.KeyNumber = KeyNumber;

                        //要修改记录的表名
                        ExecuteDB.modeName = module;

                        ExecuteDB.userName = User.Identity.Name;

                        //添加修改的审核
                        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
                        if (wf.check != null)
                        {
                            if (wf.check.GetCheckedContent())
                            {
                                ExecuteDB.checkRoleName = wf.check.GetFirstCheckRole(KeyNumber, User.Identity.Name);
                                ExecuteDB.minueInt = wf.check.GetFirstCheckRoleLimitTime();
                            }
                            else
                            {
                                ExecuteDB.checkRoleName = "";
                            }
                        }

                        //去掉Number
                        MasterNodeArray = RemoveName(MasterNodeArray, "Number");

                        //若主表参数不为NULL,则接值与验证
                        if (MasterNodeArray.Length > 0)
                        {
                            //接收主表参数
                            GetFormParameter(MasterNodeArray);

                            //验证主表参数
                            CheckMasterParameter(MasterNodeArray);
                            //更新验证 刘杰 2010-09-03
                            CheckNextPro();
                            //主表存库操作
                            cmdInfo = ExecuteDB.UpdateMaster(MasterNodeArray);
                            if (cmdInfo != null)
                            {
                                updateToDb.Add(cmdInfo);
                            }

                            //接受上传控件的参数
                            GetFormFileUp(FilesNodeArray);

                            //验证上传控件
                            if (FilesNodeArray.Length > 0)
                            {
                                CheckFileUp(FilesNodeArray);

                                //处理上传文件
                                CommandInfo[] FileInfo = ExecuteDB.InsertFileUp(FilesNodeArray, files);
                                if (FileInfo.Length != 0)
                                {
                                    for (int index = 0; index < FileInfo.Length; index++)
                                    {
                                        if (FileInfo[index] != null)
                                        {
                                            updateToDb.Add(FileInfo[index]);
                                        }
                                    }
                                }
                            }
                        }

                        //接受从表参数,并验证
                        updateToDb = GetFormSubParameter(updateToDb);

                        //执行修改的存储过程
                        cmdInfo = ExecuteDB.SP_OnExcute(2);
                        if (cmdInfo != null)
                        {
                            updateToDb.Add(cmdInfo);
                        }

                        //执行存库操作
                        int rowAffect = DbHelperSQL.ExecuteSqlTranWithIndentity(updateToDb);
                        if (rowAffect >= 0)
                        {
                            //保存上传文件
                            OperatingFile.upPath = Server.MapPath(Request.ApplicationPath + @"/Web/UpLoad/");
                            OperatingFile.SaveFile(ExecuteDB.saveFile);


                            //保存更新日志
                            string number0 = null;
                            WorkFlowModule wf0 = new WorkFlowModule("SJGXLSJLB");
                            number0 = wf0.numberFormat.GetNextNumber();
                            FMOP.DB.DbHelperSQL.QueryInt(" INSERT INTO SJGXLSJLB (Number,MKYWM,BDDH,GH,XM,CreateUser) values ('" + number0 + "','" + module + "','" + KeyNumber + "','" + User.Identity.Name + "','" + CreatUserName(User.Identity.Name) + "','" + User.Identity.Name + "')");
                            //保存日志

                            string dailiren = "无";
                            if (Session["daililogin"] != null)
                            {
                                dailiren = Session["daililogin"].ToString();
                            }
                            Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "WorkFlow_Update_Save.aspx", KeyNumber, "update", dailiren);
                            //产生提醒信息
                            Hesion.Brick.Core.WorkFlow.WorkFlowModule wfd = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
                            if (wfd.warning != null)
                            {
                                wfd.warning.CreateWarningToRole(KeyNumber, 2, User.Identity.Name);
                            }

                            string role = "";
                            //添加审核提醒
                            if (wfd.check != null)
                            {
                                role = wfd.check.GetFirstCheckRole(KeyNumber, User.Identity.Name);
                            }

              
                            string msg = "";
                            //添加审核提醒
                            if (role != null && role.Trim() != "")
                            {
                                msg = "单号为:" + KeyNumber + "的" + wf.property.Title + "已经修改完成,请尽快审核!";
                                //发送提醒信息
                                CustomWarning.CreateWarningToJobName(KeyNumber, msg, module, User.Identity.Name, role, 1,ExecuteDB.minueInt);
                            }
                            //else  //2008.10.20 10:05 
                            //{
                            //    msg = "单号为:" + KeyNumber + "的" + wf.property.Title + "无审批流程,系统已默认审批完成!";
                            //    string ModuleUrl = "WorkFlow_View.aspx?module=" + module + "&number=" + KeyNumber;
                            //    CustomWarning.AddWarning(msg, ModuleUrl, 1, User.Identity.Name, User.Identity.Name);
                            //}

                            MessageBox.ShowAndRedirect(this, "修改操作成功", RedirectPage());
                        }
                        else
                        {
                            Response.Write("<Script language ='javaScript'>alert('修改操作失败!');history.back();</Script>");
                            ApplicationInstance.CompleteRequest();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                        Response.Write("<Script language ='javaScript'>alert('" + ex.Message + "');history.back();</Script>");
                    }
                }
                else
                {
                    MessageBox.ShowAndRedirect(this, "审核尚未完成,无法执行修改", RedirectPage());
                }
            }
            else
            {
                Response.Clear();
                Response.Write("<Script language ='javaScript'>alert('模块名为空，无法操作!');history.back();</Script>");
            }
        }
    }

    /// <summary>
    /// 设置参数个数
    /// </summary>
    private void SetParameterCount(CollectionNodeObject NodeCollection)
    {
        //查询主表与子表的控件个数
        NodeCollection.GetNodeNo();

        //设定主表,子表,上传等控件的个数
        MasterNo = NodeCollection.MasterParamNo;
        SubNo = NodeCollection.SubParamNo;
        FileNo = NodeCollection.FileParamNo;
    }

    /// <summary>
    /// 接受来自于表单所传的主表参数
    /// </summary>
    /// <param name="NodeArray">主表参数列表</param>
    private void GetFormParameter(SaveDepositary[] NodeArray)
    {
        for (int i = 0; i < NodeArray.Length; i++)
        {
            if (NodeArray[i].Type != "AddOnFiles")
            {
                NodeArray[i].Text = Request.Form[NodeArray[i].Name];
            }
        }
    }

    /// <summary>
    /// 接收来自于上传文件中的参数
    /// </summary>
    /// <param name="NodeArray"></param>
    private void GetFormFileUp(SaveDepositary[] NodeArray)
    {
        files = HttpContext.Current.Request.Files;
        for (int i = 0; i < NodeArray.Length; i++)
        {
            if (files[NodeArray[i].Name].FileName != null)
            {
                NodeArray[i].Text = System.IO.Path.GetFileName(files[NodeArray[i].Name].FileName);
            }
            else
            {
                NodeArray[i].Text = "";
            }
        }
    }

    /// <summary>
    /// 处理子表存库
    /// </summary>
    private List<CommandInfo> GetFormSubParameter(List<CommandInfo> updateToDb)
    {
        XmlNodeList subList = null;
        string tableName = string.Empty;

        //查询所有的控件节点，并保存
        subList = xmlParse.GetXmlNodeList(module, "//WorkFlowModule/Data/Data-Field");

        try
        {
            //循环节点，查找子表的节点
            foreach (XmlNode subNode in subList)
            {
                if (subNode.SelectSingleNode("type") != null && XMLHelper.GetSingleString(subNode, "type") == "SubTable")
                {
                    //查找控件
                    tableName = XMLHelper.GetSingleString(subNode, "name");
                    XmlNodeList nlist = subNode.SelectNodes("subTable/Sub-Field");
                    SaveDepositary[] subArray = new SaveDepositary[nlist.Count];
                    for (int i = 0; i < nlist.Count; i++)
                    {
                        //给对象赋值
                        subArray[i] = new SaveDepositary();
                        subArray[i].Name = XMLHelper.GetSingleString(nlist[i], "name");
                        subArray[i].CanNull = XMLHelper.GetSingleBool(nlist[i], "canNull");
                        subArray[i].Type = XMLHelper.GetSingleString(nlist[i], "type");
                        subArray[i].Length = XMLHelper.GetSingleInt(nlist[i], "length", 50);
                        subArray[i].SubTable = tableName;
                    }

                    //接受参数
                    for (int i = 0; i < subArray.Length; i++)
                    {
                        if (Request[tableName + subArray[i].Name + "_hid"] != null)
                        {
                            //接收一个参数，保存到数组中.
                            subArray[i].Text = Request[tableName + subArray[i].Name + "_hid"].ToString();
                        }
                    }

                    //验证参数
                    //CheckSubParameter 函数返回: 0,1,2 0:有输入，但不完整.. 1.未输入(从未输入过子表，删除子表) ..2.输入完整..
                    switch (CheckParameters.CheckSubEmpty(subArray))
                    {
                        case "0":
                            Response.Write("<Script language ='javaScript'>alert('提交表单中填写内容不全面，请重新填写!');history.back();</Script>");
                            ApplicationInstance.CompleteRequest();
                            break;
                        case "1"://不进行新增子表无内容..
                            updateToDb.Add(ExecuteDB.DeleteSubTable(subArray));
                            break;
                        case "2":
                            updateToDb.Add(ExecuteDB.DeleteSubTable(subArray));
                            CommandInfo[] subInfo = ExecuteDB.InsertSubTable(subArray);

                            if (subInfo.Length > 0)
                            {
                                for (int length = 0; length < subInfo.Length; length++)
                                {
                                    if (subInfo[length] != null)
                                    {
                                        updateToDb.Add(subInfo[length]);
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            return updateToDb;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 验证参数,若未通过则返回上一页面.
    /// </summary>
    /// <param name="NodeArray"></param>
    private void CheckMasterParameter(SaveDepositary[] NodeArray)
    {
        //验证参数是否允许为空
        try
        {
            if (!CheckParameters.CheckEmpty(NodeArray))
            {

                Response.Write("<Script language ='javaScript'>alert('提交表单中填写内容不全面，请重新填写!');history.back();</Script>");
           

                Response.End();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// 更新流程验证 刘杰 2010-09-03
    /// </summary>
    private void CheckNextPro()
    {
        //验证参数是否允许为空
        try
        {
            //录入试产通知单、录入试产情况、品管检验、生产技术结论
            if (module=="LRSCTZD"||module=="LRSCQK"||module=="PGJY"||module=="SCJSJL")
            {
                DataSet ds= DbHelperSQL.Query("select SHRGH from " + module + " where Number='"+KeyNumber.Trim()+"'");
                if(ds.Tables[0].Rows[0][0].ToString()!="")
                {
                    Response.Write("<Script language ='javaScript'>alert('已经审核则不能进行更新操作了!');history.back();</Script>");
                   
                    Response.End();
                }
            }
            if(module=="ZPGL"||module=="QCLGL") //装配管理
            {
                DataSet ds = DbHelperSQL.Query("select SCTZD from " + module + " where Number='" + KeyNumber.Trim() + "'");
                if(ds.Tables[0].Rows[0][0].ToString()!="")
                {
                    ds = DbHelperSQL.Query("select Count(*) from " + module + " where SCTZD='" + ds.Tables[0].Rows[0][0].ToString() + "'");
                    if(ds.Tables[0].Rows[0][0].ToString()!="")
                    {
                        if(ds.Tables[0].Rows[0][0].ToString()!="0")
                        {
                           Response.Write("<Script language ='javaScript'>alert('已经有下级业务不能进行更新操作了!');history.back();</Script>");
                           
                           Response.End();
                        }


                    }

                }

            }
            //录入采购预算审核/录入请款信息审核
            if (module == "LRCGYS"|| module=="LRQKXX")
            {
                DataSet ds = DbHelperSQL.Query("select SHM from " + module + " where Number='" + KeyNumber.Trim() + "'");
                if (ds.Tables[0].Rows[0][0].ToString()=="Y")
                {
                    Response.Write("<Script language ='javaScript'>alert('已经审核通过则不能进行更新操作了!');history.back();</Script>");
                    Response.End();
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 判断是否可对子表执行新增
    /// </summary>
    /// <param name="subArray"></param>
    private bool CheckIsAdd(SaveDepositary[] subArray)
    {
        bool isAdd = false;
        for (int index = 0; index < subArray.Length; index++)
        {
            if (subArray[index].Text == "")
            {
                isAdd = true;
            }
            else
            {
                return false;
            }
        }

        return isAdd;
    }

    /// <summary>
    /// 验证上传
    /// </summary>
    private void CheckFileUp(SaveDepositary[] FilesNodeArray)
    {
        SqlCommand comm = new SqlCommand();
        bool isEmpty = false;

        for (int index = 0; index < FilesNodeArray.Length; index++)
        {
            if ( FilesNodeArray[index].CanNull == true )
            {
                isEmpty = true;
            }
            else
            {
                if (files[FilesNodeArray[index].Name].FileName != "" || CheckParameters.isExistsFile(KeyNumber))
                {
                    isEmpty = true;
                }
                else
                {
                    isEmpty = false;
                }
            }

            if (!isEmpty)
            {
                Response.Write("<Script language ='javaScript'>alert('" + FilesNodeArray[index].Name + "上传内容不能为空，请重新填写!');history.go(-1);</Script>");
                ApplicationInstance.CompleteRequest();
            }
        }
    }

    /// <summary>
    ///转向修改完成后的页面
    /// </summary>
    private string RedirectPage()
    {
        string Url = string.Empty;

        try
        {
            //取新增完成后的转向页面
            Url = xmlParse.XmlGetNodeText(module, "Update");

            //判断转向地址是否存在，若不存在，则不进行重定向
            if (Url != "")
            {
                //Url = Url + "?number=" + KeyNumber;
                Url = Url + "?" + Request.QueryString;
            }
            else
            {
               // Url  = "WorkFlow_Edit.aspx?module=" + module;
                Url = "WorkFlow_Edit.aspx?" + Request.QueryString + "&zzz=yes";
            }

            return Url;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 从NodeArray数组中，移除指定对象
    /// </summary>
    /// <param name="NodeArray"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static SaveDepositary[] RemoveName(SaveDepositary[] NodeArray, string name)
    {
        int Total = 0;
        bool Flag = false;
        for (int i = 0; i < NodeArray.Length; i++)
        {
            if (NodeArray[i].Name == name)
            {
                Flag = true;
                break;
            }
        }
        if (Flag == true)
        {
            SaveDepositary[] NewArray = new SaveDepositary[NodeArray.Length - 1];
            for (int i = 0; i < NodeArray.Length; i++)
            {
                if (NodeArray[i].Name != name)
                {
                    NewArray[Total] = NodeArray[i];
                    Total = Total + 1;
                }
            }
            return NewArray;
        }
        else
        {
            return NodeArray;
        }
    }

    /// <summary>
    /// 判断审核状态是否完成
    /// </summary>
    /// <returns></returns>
    public bool IsCheckState()
    {
        bool finish = false;

        if (module != null && KeyNumber != null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CheckState,NextChecker ");
            strSql.Append("from ");
            strSql.Append(module);
            strSql.Append(" where Number=");
            strSql.Append("'" + KeyNumber + "'");
            SqlDataReader dr = DbHelperSQL.ExecuteReader(strSql.ToString());
            if (dr.Read())
            {
                if (dr["NextChecker"] != null && dr["NextChecker"].ToString() != "")
                {
                    if (dr["CheckState"].ToString() == "0")
                    {
                        finish = false;
                    }
                    else
                    {
                        finish = true;
                    }
                }
                else
                {
                    finish = true;
                }
            }
            dr.Close();
        }
        return finish;
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
