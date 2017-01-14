<%@ WebHandler Language="C#" Class="CommonPageAttach" %>

using System;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
public class CommonPageAttach : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        ResponseInfo responseInfo = null;
        switch (context.Request.Params["Methord"])
        { 
            case "GetAllCommonFunctions":
             responseInfo=GetAllCommonFunctions(context);
              break;
            case "JudgeAuthority":
              responseInfo = JudgeAuthority(context);
              break;
            case "DeleteFunctions":
              responseInfo = DeleteFunctions(context);
              break;
            case "AddFunData":
              responseInfo = AddFunctionData(context);
              break;
        }
        string responseText = JsonConvert.SerializeObject(responseInfo);
        context.Response.Write(responseText);
    }

    /// <summary>
    /// 添加功能
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public ResponseInfo AddFunctionData(HttpContext context)
    {
        OperateInfo operateInfo = JsonConvert.DeserializeObject<OperateInfo>(context.Request.Params["operateInfoText"]);
        FunctionsInfo functionsInfo = JsonConvert.DeserializeObject<FunctionsInfo>(context.Request.Params["functionInfoText"]);
        ResponseInfo responseInfo = new ResponseInfo();
        WorkFlowModule WFM = new WorkFlowModule("HYYMGL");
        string KeyNumber = WFM.numberFormat.GetNextNumber();
        try
        {
            DbHelperSQL.ExecuteSql("insert into HYYMGL(Number,DLZH,GNXX,GNQC,SFSC,NAME,GNLB,GNTPLJ,PXZD,BH,CheckState,CreateUser,CreateTime,CheckLimitTime) " +
    " values('" + KeyNumber + "','" + operateInfo.OperUser + "','" + functionsInfo.FunDesc + "','" +functionsInfo.FunFullName+ "','未删除','" + functionsInfo.FunName + "','" + functionsInfo.FunCate + "','" + functionsInfo.FunPicPath + "','" + functionsInfo.FunIdent + "','" + functionsInfo.FunIdent + "','1','" + operateInfo.OperUser
    + "',GETDATE(),GETDATE())");
                responseInfo.IsSuccess = true;
                functionsInfo.FunID = KeyNumber;
                responseInfo.Data = functionsInfo;
        }
        catch (Exception exec)
        {
            responseInfo.IsSuccess = false;
            responseInfo.Msg = exec.Message;
        }

        return responseInfo;
    
    }
    /// <summary>
    /// 删除功能
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public ResponseInfo DeleteFunctions(HttpContext context)
    {
        OperateInfo operateInfo = JsonConvert.DeserializeObject<OperateInfo>(context.Request.Params["operateInfoText"]);
        FunctionsInfo functionsInfo = JsonConvert.DeserializeObject<FunctionsInfo>(context.Request.Params["functionInfoText"]);
        ResponseInfo responseInfo = new ResponseInfo();
        WorkFlowModule WFM = new WorkFlowModule("HYYMGL");
        string KeyNumber = WFM.numberFormat.GetNextNumber();
        try
        {
            if (functionsInfo.FunCate == "默认功能")
            {
                DbHelperSQL.ExecuteSql("insert into HYYMGL(Number,DLZH,GNXX,GNQC,SFSC,NAME,GNLB,MRGNID,GNTPLJ,PXZD,BH,CheckState,CreateUser,CreateTime,CheckLimitTime) " +
    " values('" + KeyNumber + "','" + operateInfo.OperUser + "','" + functionsInfo.FunDesc +"','" +functionsInfo.FunFullName+ "','已删除','" + functionsInfo.FunName + "','" + functionsInfo.FunCate + "','" + functionsInfo.FunID + "','" + functionsInfo.FunPicPath + "','" + functionsInfo.FunIdent + "','" + functionsInfo.FunIdent + "','1','" + operateInfo.OperUser
    + "',GETDATE(),GETDATE())");
                responseInfo.IsSuccess = true;
                responseInfo.Data = functionsInfo;
            }
            else if (functionsInfo.FunCate=="常用功能")
            {
                DbHelperSQL.ExecuteSql("update HYYMGL set SFSC='已删除' where Number='"+functionsInfo.FunID+"'");
                responseInfo.IsSuccess = true;
                responseInfo.Data = functionsInfo;
            }
        }
        catch (Exception exec)
        {
            responseInfo.IsSuccess = false;
            responseInfo.Msg = exec.Message;
        }

        return responseInfo;
        
    }
    /// <summary>
    /// 判断权限
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public ResponseInfo JudgeAuthority(HttpContext context)
    {
        OperateInfo operateInfo = JsonConvert.DeserializeObject<OperateInfo>(context.Request.Params["operateInfoText"]);
        FunctionsInfo functionsInfo = JsonConvert.DeserializeObject<FunctionsInfo>(context.Request.Params["functionInfoText"]);
        ResponseInfo responseInfo = new ResponseInfo();

        DefinedModule Dfmodule = new DefinedModule(functionsInfo.FunName);
        Authentication auth = Dfmodule.authentication;
        if (auth==null)
        {
            responseInfo.IsSuccess = false;
            responseInfo.Msg="没有权限查看";
        }
        if (!auth.GetAuthByUserNumber(operateInfo.OperUser).CanView)
        {
            responseInfo.IsSuccess = false;
            responseInfo.Msg = "没有权限查看";
        }
        else
        {
            
            DataSet dataSet = DbHelperSQL.Query("select id,parentid,title,px from system_ModuleGroup;select SmallClassId from dbo.system_Modules  where " + "name='" + functionsInfo .FunName+ "';");
            //目录
            DataTable tableModuleGroup = dataSet.Tables[0];
           //功能
            DataTable tableModule = dataSet.Tables[1];
            List<string> listStr = new List<string>();
            string strModuleID = tableModule.Rows[0]["SmallClassId"].ToString();
            listStr.Add(strModuleID);
           foreach (DataRow dataRow in tableModuleGroup.Rows)
           {
               if (strModuleID == "0")
               {
                   break;
               }
               if (dataRow["id"].ToString() == strModuleID)
               {
                   strModuleID = dataRow["parentid"].ToString();
                   listStr.Add(strModuleID);
               }
           }
          listStr.Reverse();
          string strModleColl = "";
          foreach (string str in listStr)
          {
              strModleColl += str + ",";
          }
          strModleColl = strModleColl.Substring(0,strModleColl.Length-1);

          /*
           判断查看页面的类型
           */
          DataSet dataSetPage = DbHelperSQL.Query("select distinct ModuleName,ModuleType "+
" from system_auth "+
" where ModuleName='"+functionsInfo.FunName+"' and  "+
" ((type=1 and role=(select GWMC from HR_Employees where Number='"+operateInfo.OperUser+"'))  "+
" or (type=2 and role=(select BM from HR_Employees where Number='"+operateInfo.OperUser+"')) "+
" or (type=3 and role='"+operateInfo.OperUser+"') or (role='全公司') )");
          functionsInfo.FunAuthrity = dataSetPage.Tables[0].Rows[0]["ModuleType"].ToString();
          functionsInfo.FunParIdColl = strModleColl;
         responseInfo.IsSuccess = true;
         responseInfo.Data = functionsInfo;
        }

        return responseInfo;
    
    }
    /// <summary>
    /// 查询出所有功能
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public ResponseInfo GetAllCommonFunctions(HttpContext context)
    {
        OperateInfo operateInfo = JsonConvert.DeserializeObject<OperateInfo>(context.Request.Params["operateInfoText"]);
        ResponseInfo responseInfo = new ResponseInfo();
        List<FunctionsInfo> listFunctionInfo = new List<FunctionsInfo>();
        try
        {
            DataTable dataTable = DbHelperSQL.Query("select Number,GNXX,GNQC,NAME,BH,GNTPLJ,GNLB from " +
" ( "+
" select Number,GNXX,GNQC,NAME,BH,GNTPLJ,GNLB='默认功能' from HYYMMRGNGL " +
" where Number not in (select MRGNID from HYYMGL  where DLZH='" + operateInfo.OperUser + "' and GNLB='默认功能' and SFSC='已删除') " +
" union "+
" select Number,GNXX,GNQC,NAME,BH,GNTPLJ,GNLB='常用功能' from HYYMGL  where DLZH='" + operateInfo.OperUser + "' and GNLB='常用功能' and SFSC='未删除' " +
" ) as tab "+
" order by BH asc").Tables[0];

            if (dataTable.Rows.Count > 0)
            { 
            foreach(DataRow dataRow in dataTable.Rows)
            {
                FunctionsInfo functionInfo = new FunctionsInfo();
                functionInfo.FunID = dataRow["Number"].ToString();
                functionInfo.FunDesc = dataRow["GNXX"].ToString();
                //object obj = dataRow["GNQC"];
                //if (obj is DBNull)
                //{
                //    obj = "";
                //}
                //else
                //{
                //    obj = dataRow["GNQC"].ToString();
                //}
                functionInfo.FunFullName = dataRow["GNQC"].ToString();
                functionInfo.FunName = dataRow["NAME"].ToString();
                functionInfo.FunIdent =Convert.ToInt32(dataRow["BH"].ToString());
                functionInfo.FunPicPath = dataRow["GNTPLJ"].ToString();
                functionInfo.FunCate = dataRow["GNLB"].ToString();
                listFunctionInfo.Add(functionInfo);
            }
            }
            responseInfo.IsSuccess = true;
            FunctionsInfo[] functionInfoArray = listFunctionInfo.ToArray();
            responseInfo.Data = functionInfoArray;
        }
        catch(Exception exec)
        {
            responseInfo.IsSuccess = false;
            responseInfo.Msg = exec.Message;
        }
            
        return responseInfo;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }
   
}

/// <summary>
/// 消息类
/// </summary>
public class ResponseInfo
{
    private bool _isSuccess;
    private string _msg;
    private object _data;
    // 是否成功
    public bool IsSuccess { 
        get { return _isSuccess; } 
        set { _isSuccess = value; } 
    }

    // 消息
    public string Msg {
        get { return _msg; }
        set { _msg = value; }
    }

    // 数据
    public object Data {
        get { return _data; }
        set { _data = value; }
    }
}
/// <summary>
/// 功能类
/// </summary>
public class FunctionsInfo
{
    private string _funID;
    private string _funDesc;
    private string _funName;
    private int _funIdent;
    private string _funPicPath;
    private string _funCate;
    private string _funParIdColl;
    private string _funAuthrity;
    private string _funFullName;
    
    /// <summary>
    ///功能ID
    /// </summary>
    public string FunID { 
        get { return _funID; } 
        set { _funID = value; }
    }

    /// <summary>
    /// 功能描述
    /// </summary>
    public string FunDesc { 
        get { return _funDesc; } 
        set { _funDesc = value; } 
    }

    /// <summary>
    /// 功能NAME
    /// </summary>
    public string FunName { 
        get { return _funName; } 
        set { _funName = value; } 
    }

    /// <summary>
    /// 功能编号
    /// </summary>
    public int FunIdent { 
        get { return _funIdent; } 
        set { _funIdent = value; } 
    }

    /// <summary>
    /// 功能图片路径
    /// </summary>
    public string FunPicPath {
        get { return _funPicPath; } 
        set { _funPicPath = value; } 
    }

    /// <summary>
    /// 功能类别
    /// </summary>
    public string FunCate { 
        get { return _funCate; } 
        set { _funCate = value; } 
    }

    /// <summary>
    /// 功能父级ID集合
    /// </summary>
    public string FunParIdColl {
        get { return _funParIdColl; }
        set { _funParIdColl = value; }
    }

    /// <summary>
    /// 功能权限
    /// </summary>
    public string FunAuthrity
    {
        get { return _funAuthrity; }
        set { _funAuthrity = value;
        _funAuthrity = "";
        switch (value)
        {
            case "1":
                _funAuthrity = "WorkFlow_Add.aspx";
                break;
            case "2":
                _funAuthrity = "TextReport.aspx";
                break;
            case "3":
                _funAuthrity = "ChartReport.aspx";
                break;
            case "4":
                _funAuthrity = "CustormModule.aspx";
                break;
        }
        }
       }
    /// <summary>
    /// 功能全称
    /// </summary>
    public string FunFullName
    {
        get { return _funFullName; }
        set {
            if (value == null)
            {
                _funFullName = "";
            }
            else
            {
                _funFullName = value.ToString();
            }
        }
    }
    
}

/// <summary>
/// 操作类
/// </summary>
public class OperateInfo
{
    private string _operUser;
    private string _operCate;
    private string _operProper;
    
    /// <summary>
    /// 操作人员
    /// </summary>
    public string OperUser { 
        get { return _operUser; } 
        set { _operUser = value; } 
    }
    
    /// <summary>
    /// 操作类别“默认功能还是普通功能”
    /// </summary>
    public string OperCate { 
        get { return _operCate; } 
        set { _operCate = value; }
    }

    /// <summary>
    /// 操作性质“是删除还是增加”
    /// </summary>
    public string OperProper {
        get { return _operProper; } 
        set { _operProper = value; } 
    }

}