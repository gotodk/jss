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
using System.Collections.Generic;
using System.Xml;
using System.Text;
using FMOP.SD;
using FMOP.Check;
using FMOP.Execute;
using FMOP.XParse;
using FMOP.DB;
using FMOP.XHelp;
using FMOP.FILE;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;
using ShareLiu.DXS;   //liujie 2010-7-9 短信控件引用
public partial class WorkFlow_Add_Save : System.Web.UI.Page
{
    int MasterNo = 0;               //主表参数个数
    int SubNo = 0;                  //子表参数个数 
    int FileNo = 0;                 //保存上传控件个数
    string KeyNumber = "";          //主键单号
    string module = "";             //模块名称
    HttpFileCollection files = null;
    Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = null;
    string role = string.Empty;
    string role2 = string.Empty;
    /// <summary>
    /// 页面加载事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request["module"] != null && Request["module"].ToString() != "")
            {
                module = Request["module"].ToString();
                //参数集合类对象
                CollectionNodeObject NodeCollection = new CollectionNodeObject(module);

                //取主表与从表要接收的参数个数 
                SetParameterCount(NodeCollection);

                //产生要接收的参数数组
                SaveDepositary[] MasterNodeArray = new SaveDepositary[MasterNo];
                SaveDepositary[] SubNodeArray = new SaveDepositary[SubNo];
                SaveDepositary[] FilesNodeArray = new SaveDepositary[FileNo];

                //生成参数数组对象
                MasterNodeArray = NodeCollection.GetMasterCollection();
                FilesNodeArray = NodeCollection.GetFileCollection();

                //创建泛型对象
                List<CommandInfo> saveToDb = new List<CommandInfo>();
                //创建commandInfo对象和commandInfo数组
                CommandInfo cmdInfo = new CommandInfo();
                try
                {
                    //产生主键编号
                    if (xmlParse.IsAuto(module))
                    {
                        ExecuteDB.NumberType = "auto";
                        WorkFlowModule WFM = new WorkFlowModule(module);
                        KeyNumber = WFM.numberFormat.GetNextNumber();
                    }
                    else
                    {
                        ExecuteDB.NumberType = "manual";
                        KeyNumber = Request["Number"].ToString();

                        //从数组中移除Number
                        MasterNodeArray = RemoveName(MasterNodeArray, "Number");

                        //判断定单号是否重复
                        checkIsRepeat();
                    }

                    ExecuteDB.KeyNumber = KeyNumber;
                    ExecuteDB.userName = User.Identity.Name;
                    ExecuteDB.modeName = module;
                    ExecuteDB.minueInt = 0; //初始化
                    //添加新增的审核
                    wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
                    if (wf.check != null)
                    {
                        
                        role = wf.check.GetFirstCheckRole(KeyNumber, User.Identity.Name);  //原算法不包含短信节点
                        role2 = wf.check.GetFirstCheckRole2(KeyNumber, User.Identity.Name);  //刘杰 2010-07-12 加入包含短信节点
                        ExecuteDB.minueInt = wf.check.GetFirstCheckRoleLimitTime();
                    }
                    else
                    {
                        role = "";
                        role2 = "";
                    }
                    if (role != null && role != "")
                    {
                        ExecuteDB.checkRoleName = role;
                    }
                    else
                    {
                        ExecuteDB.checkRoleName = "";
                    }

                    //若主表参数不为NULL,则接值与验证
                    if (MasterNodeArray.Length > 0)
                    {
                        //1.接收主表参数
                        GetFormParameter(MasterNodeArray);

                        //2.验证主表参数
                        CheckMasterParameter(MasterNodeArray);

                        //接受上传控件的参数
                        GetFormFileUp(FilesNodeArray);

                        //主表存库操作,添加主表的SQL 命令与对象
                        cmdInfo = ExecuteDB.InsertMaster(MasterNodeArray);
                        if (cmdInfo != null)
                        {
                            saveToDb.Add(cmdInfo);
                        }

                        //若存在上传文件,则验证上传
                        CheckFileUp(FilesNodeArray);

                        //添加上传存库的命令
                        CommandInfo[] FileInfo = ExecuteDB.InsertFileUp(FilesNodeArray, files);
                        if (FileInfo.Length != 0)
                        {
                            for (int index = 0; index < FileInfo.Length; index++)
                            {
                                if (FileInfo[index] != null)
                                {
                                    saveToDb.Add(FileInfo[index]);
                                }
                            }
                        }
                    }
                    else
                    {
                        //主表存库操作,添加主表的SQL 命令与对象
                        cmdInfo = ExecuteDB.InsertNullMaster();
                        if (cmdInfo != null)
                        {
                            saveToDb.Add(cmdInfo);
                        }
                    }

                    //保存子表的操作
                    saveToDb = GetFormSubParameter(saveToDb);


                    //执行存储过程
                    cmdInfo = ExecuteDB.SP_OnExcute(0);
                    if (cmdInfo != null)
                    {
                        saveToDb.Add(cmdInfo);
                    }

                    //执行存库操作
                    if (saveToDb.Count >= 1)
                    {
                        int rowAffect = DbHelperSQL.ExecuteSqlTranWithIndentity(saveToDb);
                        if (rowAffect >= 0)
                        {
                            //保存上传文件
                            OperatingFile.upPath = Server.MapPath(Request.ApplicationPath + @"/Web/UpLoad/");
                            OperatingFile.SaveFile(ExecuteDB.saveFile);

                            //保存日志

                            string dailiren = "无";
                            if (Session["daililogin"] != null)
                            {
                                dailiren = Session["daililogin"].ToString();
                            }
                            Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "WorkFlow_Add_Save.aspx", KeyNumber, "insert", dailiren);

                            //产生提醒信息
                            if (wf.warning != null)
                            {
                                wf.warning.CreateWarningToRole(KeyNumber, 1, User.Identity.Name);
                            }
                          

                            string msg = string.Empty;
                            //添加审核提醒
                            if (role != null && role.Trim() != "")
                            {
                                if (module != "temp_employee") //特殊模块不立刻发送审批提醒
                                {
                                    msg = "单号为:" + KeyNumber + "的" + wf.property.Title + "已经填写完成,请尽快审核!";
                                    //发送提醒信息
                                    CustomWarning.CreateWarningToJobName(KeyNumber, msg, module, User.Identity.Name, role, 1, ExecuteDB.minueInt); 
                                    //启用短信发送的处理 刘杰 2010-07-12
                                    CustomWarning.SendDXSH(KeyNumber, msg, module, User.Identity.Name, role2);  


                                }
                                
                            }
                            //else  //2008.10.20 10:04分
                            //{
                            //    msg = "单号为:" + KeyNumber + "的" + wf.property.Title + "无审批流程,系统已默认审批完成!";
                            //    string ModuleUrl = "WorkFlow_View.aspx?module=" + module + "&number="+KeyNumber;
                            //    CustomWarning.AddWarning(msg, ModuleUrl, 1, User.Identity.Name, User.Identity.Name);
                            //}

                            //跳转页面 
                            if (module == "FWPT_HGJXGZRQRD")
                            {
                                Response.Redirect(RedirectPage());
                                Response.End();
                            }
                            else
                            {
                                MessageBox.ShowAndRedirect(this, "新增操作成功", RedirectPage());
                            }
                        }
                        else
                        {
                            Response.Write("<Script language ='javaScript'>alert('新增操作失败!');history.back();</Script>");
                            Response.End();
                        }
                    }
                    else
                    {
                        Response.Write("<Script language ='javaScript'>alert('无任何数据可进行操作!');history.back();</Script>");
                        Response.End();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    Response.Write("<div style=' display:none'>"+ex.ToString()+"</div>");
                    //Response.Write("<Script language ='javaScript'>alert(\"新增操作失败!\");history.back();</Script>");
                    Response.End();
                }
            }
            else
            {
                Response.Write("<Script language ='javaScript'>alert('模块名为空,无法执行操作!');history.back();</Script>");
                Response.End();
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
        try
        {
            for (int i = 0; i < NodeArray.Length; i++)
            {
                if (NodeArray[i].Type != "AddOnFiles")
                {
                    NodeArray[i].Text = Request.Form[NodeArray[i].Name];
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
    private List<CommandInfo> GetFormSubParameter(List<CommandInfo> saveSubInfo)
    {
        XmlNodeList subList = null;
        string tableName = string.Empty;
        int length = 0;
        //查询所有的控件节点，并保存
        subList = xmlParse.GetXmlNodeList(module, "//WorkFlowModule/Data/Data-Field");
        CommandInfo[] subInfo = null;
        try
        {
            //循环节点，查找子表的节点
            foreach (XmlNode subNode in subList)
            {
                if (subNode.SelectSingleNode("type") != null && XMLHelper.GetSingleString(subNode, "type") == "SubTable")
                {
                    tableName = XMLHelper.GetSingleString(subNode, "name");
                    //查找控件
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
                    switch (CheckParameters.CheckSubEmpty(subArray))
                    {
                        case "0":
                            Response.Write("<Script language ='javaScript'>alert('提交表单中填写内容不全面，请重新填写!');history.back();</Script>");
                            Response.End();
                            Response.End();
                            break;
                        case "1"://不进行新增子表无内容..
                            break;
                        case "2":
                            subInfo = ExecuteDB.InsertSubTable(subArray);
                            if (subInfo.Length > 0)
                            {
                                for (length = 0; length < subInfo.Length; length++)
                                {
                                    if (subInfo[length] != null)
                                    {
                                        saveSubInfo.Add(subInfo[length]);
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            return saveSubInfo;
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
                Response.End();
            }

            //进行特殊验证
            ArrayList altemp = new ArrayList();
            altemp = CheckParameters.CheckSpecial(NodeArray, module);
            if (altemp.Count > 0 && altemp[0].ToString() == "不通过")
            {
                Response.Write("<Script language ='javaScript'>alert('" + altemp[1].ToString() + "');history.back();</Script>");
                Response.End();
                Response.End();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void CheckFileUp(SaveDepositary[] NodeArray)
    {
        //验证参数是否允许为空
        for (int index = 0; index < NodeArray.Length; index++)
        {
            if (NodeArray[index].CanNull == false)
            {
                if (files[NodeArray[index].Name].FileName == "")
                {
                    Response.Write("<Script language ='javaScript'>alert('" + NodeArray[index].Name + "上传内容不能为空,请选择上传文件!');history.back();</Script>");
                    Response.End();
                    Response.End();
                }
            }
        }
    }

    /// <summary>
    /// 判断定单号是否重复
    /// </summary>
    private void checkIsRepeat()
    {
        int total = 0;
        try
        {
            total = DbHelperSQL.QueryInt("select count(*) from " + module + " where Number ='" + KeyNumber + "'");
            if (total > 0)
            {
                Response.Write("<Script language ='javaScript'>alert('编号已经存在，请重新填写!');history.back();</Script>");
                Response.End();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    ///转向新增完成后的页面
    /// </summary>
    private string RedirectPage()
    {
        string Url = string.Empty;

        //取新增完成后的转向页面
        Url = xmlParse.XmlGetNodeText(module, "Add");

        //判断转向地址是否存在，若不存在，则不进行重定向
        if (Url != "")
        {
            Url = Url + "?number=" + KeyNumber;
        }
        else
        {
            Url = "WorkFlow_Add.aspx?module=" + module;
        }
        return Url;
    }

    /// <summary>
    /// 验证控件
    /// </summary>
    /// <param name="paramArray"></param>
    /// <returns></returns>
    public static bool CheckEmpty(SaveDepositary[] paramArray)
    {
        for (int index = 0; index < paramArray.Length; index++)
        {
            if (paramArray[index].Type != "AddOnFiles")
            {
                if (paramArray[index].CanNull == false && paramArray[index].Text != "")
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (HttpContext.Current.Request.Files[paramArray[index].Name].FileName != "")
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
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

}
