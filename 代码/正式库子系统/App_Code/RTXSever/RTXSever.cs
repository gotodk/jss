/*
 * 创建时间 ：2009.07.09
 * 创建人：王永辉
 * 代码作用： RTX 操作
 */


using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.InteropServices;
using RTXServerApi;
/// <summary>
///RTXSever 的摘要说明
/// </summary>
public class RTXSevera
{
    public RTXSevera()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }


    #region 方法
    /// <summary>
    /// 生成客户端登录的vbscript代码
    /// </summary>
    /// <param name="strUserNameOrID">用户名称</param>
    /// <param name="strServerIp">服务器名称</param>
    /// <returns>生成的vbscript代码</returns>
    public string GenerateClientLoginScript(string strUserNameOrID, string strServerIp)
    {
        //生成客户端登录的vbscript代码
        string script = @"
							<script language=""vbscript"">
							<!--
							ClientLogin
							Sub ClientLogin
                               
								Dim obj1
								Dim objProp
								Dim strKey,strUserName,strServerIp";
        script += @"
								strKey =""" + GetSessionKey(strUserNameOrID) + @"""";
        script += @"
								strUserName =""" + strUserNameOrID + @"""";
        script += @"
								strServerIp =""" + strServerIp + @"""";
        script += @"             
								On error resume next
								Set obj1 = CreateObject(""RTXClient.RTXAPI"") 
								if( err.number <> 0) then
									if(msgbox(""没有检测到RTX客户端软件，是否下载安装？"",1)=1) then
										document.url = ""http://"" & strServerIp & "":8012/quickdeploy/rtxcsetup.exe""
									end if
									exit sub
								end if
                                
								Set objProp = obj1.GetObject(""Property"")
								objProp.Value(""RTXUsername"") =   strUserName 
								objProp.Value(""LoginSessionKey"") = strKey 
								objProp.Value(""ServerAddress"") = strServerIp
								objProp.Value(""ServerPort"") = 8000
								'登录指定的服务器
								obj1.Call 2, objProp
                             
							End Sub

							-->
							</script>";

        return script;
    }

    #region 系统类别方法

    /// <summary>
    /// 当使用单点登录时，从Server端获取登录的密钥
    /// </summary>
    /// <param name="strUserNameOrID">登录者，RTX中的用户名称或用户号码</param>
    /// <returns>错误抛出异常，否则返回登录密钥字符串</returns>
    public String GetSessionKey(String strUserNameOrID)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        string strKey = string.Empty;

        RTXObj.Name = "SYSTOOLS";
        RTXParams.Add("UserName", strUserNameOrID);//用户名称，RTX中的用户名称或用户号码
        try
        {
            strKey = RTXObj.Call2(RTXServerApi.enumCommand_.PRO_SYS_USERLOGIN, RTXParams).ToString();// PRO_SYS_USERLOGIN=0x2000,为获取SessionKey的指令
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return strKey;
    }

    /// <summary>
    /// 检查用户名称或用户号码是否存在。
    /// </summary>
    /// <param name="strUserNameOrID">用户名称，RTX中的用户名称或用户号码</param>
    /// <returns>错误抛出异常，存在用户返回true，不存在用户返回false。</returns>
    public bool CheckUserIsExist(String strUserNameOrID)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        int intResult = 1;
        RTXObj.Name = "SYSTOOLS";
        RTXParams.Add("USERNAME", strUserNameOrID);//用户名称，RTX中的用户名称或用户号码
        try
        {
            //错误抛出异常，否则返回 0表示存在，其它值表示不存在。
            intResult = Convert.ToInt16(RTXObj.Call2(RTXServerApi.enumCommand_.PRO_IFEXISTUSER, RTXParams));// PRO_SYS_SENDIM=0x8,检查用户名称或用户号码是否存在。
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        if (intResult == 0)
            return true;
        else
            return false;
    }


    /// <summary>
    /// 发送即时消息(IM)到RTX客户端。
    /// </summary>
    /// <param name="strSender">发送者，RTX中的用户名称或用户号码</param>
    /// <param name="strReceivers">接收者，RTX中的用户名称或用户号码，多人以“,”分隔。</param>
    /// <param name="strMsg">消息内容。</param>
    /// <param name="strPassword">发送者密码。</param>
    /// <returns>错误抛出异常，返回true表示成功。</returns>
    public bool SendIM(String strSender, String strReceivers, String strMsg, String strPassword)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        RTXObj.Name = "SYSTOOLS";
        RTXParams.Add("SENDER", strSender);//发送者，RTX中的用户名称或用户号码
        RTXParams.Add("RECVUSERS", strReceivers);//接收者，RTX中的用户名称或用户号码，多人以“,”分隔。
        RTXParams.Add("IMMSG", strMsg);//消息内容。
        RTXParams.Add("SDKPASSWORD", strPassword);//发送者密码。
        try
        {
            RTXObj.Call2(RTXServerApi.enumCommand_.PRO_SYS_SENDIM, RTXParams);// PRO_SYS_SENDIM=0x2002,为发送普通的IM消息
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return true;
    }


    /// <summary>
    /// 采用RTXServerSDK向RTX Client 发送消息提醒。使用隐式的URL链接方式内容类似为: [ 腾讯公司 | http://rtx.tencent.com ]。
    ///	消息提醒内容的长度没有限制，通常以消息提醒框能够合适的看到内容为宜。
    /// </summary>
    /// <param name="strReceivers">接收者。用户名称或号码。</param>
    /// <param name="strMsg">消息提醒内容。</param>
    /// <param name="strTitle">消息提醒标题。</param>
    /// <returns>错误抛出异常。返回true表示成功。</returns>
    public bool SendNotify(String strReceivers, String strMsg, String strTitle)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        RTXObj.Name = "SYSTOOLS";
        RTXParams.Add("USERNAME", strReceivers);//接收者。用户名称或号码。 必填参数。
        RTXParams.Add("MSGINFO", strMsg);//消息提醒内容。 必填参数。
        RTXParams.Add("TITLE", strTitle);//消息提醒标题。
        try
        {
            RTXObj.Call2(RTXServerApi.enumCommand_.PRO_EXT_NOTIFY, RTXParams);// PRO_EXT_NOTIFY=0x2100,为发送消息提醒
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return true;
    }

    #endregion


    #region 用户管理类别方法

    /// <summary>
    /// 添加用户到RTX中
    /// </summary>
    /// <param name="strDeptID">用户所在部门编号</param>
    /// <param name="strNick">用户ID，具有唯一性</param>
    /// <param name="strPassword">用户密码</param>
    /// <param name="strName">用户姓名</param>
    /// <param name="strMobile">手机号码</param>
    /// <param name="strUIN">1001到9999，用户的RTX号码</param>
    /// <returns>错误抛出异常，返回true表示成功。</returns>
    public bool AddUser(String strDeptID, String strNick, String strPassword, String strName, String strMobile, String strUIN)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        RTXObj.Name = "USERMANAGER";
        RTXParams.Add("DEPTID", strDeptID);//部门编号。 必填参数。 
        RTXParams.Add("NICK", strDeptID);//用户名称。 必填参数。 
        RTXParams.Add("PWD", strDeptID);//登录密码。 
        RTXParams.Add("NAME", strDeptID);//用户姓名。  
        RTXParams.Add("MOBILE", strDeptID);//手机号码。  
        RTXParams.Add("UIN", strDeptID);//用户的RTX号码，必填参数。  
        try
        {
            RTXObj.Call2(RTXServerApi.enumCommand_.PRO_ADDUSER, RTXParams);// PRO_ADDDEPT=0x1,添加用户到RTX中
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return true;
    }


    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="strNick">用户ID，具有唯一性</param>
    /// <returns>错误抛出异常，返回true表示成功。</returns>
    public bool DelUser(String strNick)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        RTXObj.Name = "USERMANAGER";
        RTXParams.Add("USERNAME", strNick);//用户ID，具有唯一性。 
        try
        {
            RTXObj.Call2(RTXServerApi.enumCommand_.PRO_DELUSER, RTXParams);// PRO_DELUSER=0x2,删除用户
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return true;
    }

    /// <summary>
    /// 修改RTX中用户简单资料
    /// </summary>
    /// <param name="strDeptID">用户所在部门编号</param>
    /// <param name="strNick">用户ID，具有唯一性</param>
    /// <param name="strPassword">用户密码</param>
    /// <param name="strName">用户姓名</param>
    /// <param name="strMobile">手机号码</param>
    /// <param name="strUIN">1001到9999，用户的RTX号码</param>
    /// <returns>错误抛出异常，返回true表示成功。</returns>
    public bool SetSimpleUser(String strDeptID, String strNick, String strPassword, String strName, String strMobile, String strUIN)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        RTXObj.Name = "USERMANAGER";
        RTXObj.Name = "USERMANAGER";
        RTXParams.Add("DEPTID", strDeptID);//用户所在部门编号。 必填参数。 
        RTXParams.Add("NICK", strDeptID);//用户名称。 必填参数。 
        RTXParams.Add("PWD", strDeptID);//登录密码。 
        RTXParams.Add("NAME", strDeptID);//用户姓名。  
        RTXParams.Add("MOBILE", strDeptID);//手机号码。  
        RTXParams.Add("UIN", strDeptID);//1001到9999，用户的RTX号码。必填参数。  
        try
        {
            RTXObj.Call2(RTXServerApi.enumCommand_.PRO_SETUSERSMPLINFO, RTXParams);// PRO_ADDDEPT=0x03,修改用户简单信息
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return true;
    }


    /// <summary>
    /// 修改用户权限
    ///  	权限列表，可以逻辑组合下表的值。
    ///		例子：iRight = 1+0x10
    ///		值 描叙 
    ///		1 创建讨论组 
    ///		2 创建会议 
    ///		4 修改用户姓名 
    ///		8 修改用户名称 
    ///		0x10(16进制) 查看手机号码 
    ///		0x20(16进制) 发起5人以上的会话 
    ///		0x40(16进制) 允许向他人添加备忘录 
    ///		0x80(16进制) 允许出差支持 
    ///		0x100(16进制) 允许出差支持并允许发短消息 
    ///		0x200(16进制) 允许出差支持并允许发送文件 
    ///		0x400(16进制) 允许B2B发送文件 
    /// </summary>
    /// <param name="strNick">用户ID，具有唯一性</param>
    /// <param name="strAllow">允许的权限列表</param>
    /// <param name="strDeny">拒绝的权限列表</param>
    /// <returns>错误抛出异常，返回true表示成功。</returns>
    public bool SetUserPrivilege(String strNick, String strAllow, String strDeny)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        RTXObj.Name = "USERMANAGER";
        RTXParams.Add("USERNAME", strNick);//用户名称或RTX号码。
        RTXParams.Add("ALLOW", strAllow);// 0 权限列表。允许的权限列表。
        RTXParams.Add("DENY", strDeny);// 0 权限列表。拒绝的权限列表。
        try
        {
            RTXObj.Call2(RTXServerApi.enumCommand_.PRO_SETUSERPRIVILEGE, RTXParams);// PRO_ADDDEPT=0x07,修改用户权限
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return true;
    }

    #endregion


    #region 部门管理类别方法

    /// <summary>
    /// 添加部门到RTX中
    /// </summary>
    /// <param name="strParentDeptID">父部门编号,0表示根节点</param>
    /// <param name="strDeptName">部门名称</param>
    /// <param name="strDeptID">部门编号</param>
    /// <param name="strDeptInfo">部门描述</param>
    /// <returns>错误抛出异常，返回true表示成功。</returns>
    public bool AddDept(String strParentDeptID, String strDeptName, String strDeptID, String strDeptInfo)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        RTXObj.Name = "DEPTMANAGER";
        RTXParams.Add("PDEPTID", strParentDeptID);//父部门编号。 必填参数。
        RTXParams.Add("NAME", strDeptName);//部门名称。 必填参数。
        RTXParams.Add("DEPTID", strDeptID);//部门编号。 可选。 
        RTXParams.Add("INFO", strDeptInfo);//部门描述。 可选。
        try
        {
            RTXObj.Call2(RTXServerApi.enumCommand_.PRO_ADDDEPT, RTXParams);// PRO_ADDDEPT=0x101,添加部门到RTX中
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return true;
    }


    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="strDeptID">部门编号</param>
    /// <param name="strCompleteDelBS">是否删除子部门与所有的用户，1删除，0不删除</param>
    /// <returns>错误抛出异常，返回true表示成功。</returns>
    public bool DelDept(String strDeptID, String strCompleteDelBS)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        RTXObj.Name = "DEPTMANAGER";
        RTXParams.Add("DEPTID", strDeptID);//部门编号。 
        RTXParams.Add("COMPLETEDELBS", strCompleteDelBS);// 是否删除子部门与所有的用户，1删除，0不删除。 
        try
        {
            RTXObj.Call2(RTXServerApi.enumCommand_.PRO_DELDEPT, RTXParams);// PRO_ADDDEPT=0x102,删除部门
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return true;
    }


    /// <summary>
    /// 修改RTX中部门资料
    /// </summary>
    /// <param name="strParentDeptID">父部门编号,0表示根节点</param>
    /// <param name="strDeptName">部门名称</param>
    /// <param name="strDeptID">部门编号</param>
    /// <param name="strDeptInfo">部门描述</param>
    /// <returns>错误抛出异常，返回true表示成功。</returns>
    public bool SetDept(String strParentDeptID, String strDeptName, String strDeptID, String strDeptInfo)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        RTXObj.Name = "DEPTMANAGER";
        RTXParams.Add("PDEPTID", strParentDeptID);//父部门编号。 必填参数。
        RTXParams.Add("NAME", strDeptName);//部门名称。 必填参数。
        RTXParams.Add("DEPTID", strDeptID);//部门编号。 可选。 
        RTXParams.Add("INFO", strDeptInfo);//部门描述。 可选。
        try
        {
            RTXObj.Call2(RTXServerApi.enumCommand_.PRO_SETDEPT, RTXParams);// PRO_ADDDEPT=0x103,修改RTX中部门资料
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return true;
    }


    /// <summary>
    /// 修改部门权限
    ///  	权限列表，可以逻辑组合下表的值。
    ///		例子：iRight = 1+0x10
    ///		值 描叙 
    ///		1 创建讨论组 
    ///		2 创建会议 
    ///		4 修改用户姓名 
    ///		8 修改用户名称 
    ///		0x10(16进制) 查看手机号码 
    ///		0x20(16进制) 发起5人以上的会话 
    ///		0x40(16进制) 允许向他人添加备忘录 
    ///		0x80(16进制) 允许出差支持 
    ///		0x100(16进制) 允许出差支持并允许发短消息 
    ///		0x200(16进制) 允许出差支持并允许发送文件 
    ///		0x400(16进制) 允许B2B发送文件 
    /// </summary>
    /// <param name="strDeptID">部门编号</param>
    /// <param name="strAllow">允许的权限列表</param>
    /// <param name="strDeny">拒绝的权限列表</param>
    /// <returns>错误抛出异常，返回true表示成功。</returns>
    public bool SetDeptPrivilege(String strDeptID, String strAllow, String strDeny)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        RTXObj.Name = "DEPTMANAGER";
        RTXParams.Add("DEPTID", strDeptID);//部门编号。
        RTXParams.Add("ALLOW", strAllow);// 0 权限列表。允许的权限列表。
        RTXParams.Add("DENY", strDeny);// 0 权限列表。拒绝的权限列表。
        try
        {
            RTXObj.Call2(RTXServerApi.enumCommand_.PRO_SETDEPTPRIVILEGE, RTXParams);// PRO_ADDDEPT=0x106,修改部门权限
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return true;
    }


    #endregion


    #region 组织机构同步方法
    /// <summary>
    /// 将XML格式的用户部门信息导入到RTX的组织机构中，具体格式如下
    /// <?xml version="1.0" encoding="gb2312" ?>
    /// <enterprise name="">
    /// 	 <departments>
    ///			<department name="XX软件公司" describe="软件开发">
    ///				 <user uid="总经理" name="1001"/>
    ///				<department name="开发部" describe="">
    ///					<user uid="项目经理" name="1002"/>
    ///				 </department>
    ///			</department>
    ///		</departments>
    /// </enterprise>
    /// </summary>
    /// <param name="strUserXml">XML格式字符串</param>
    /// <returns>错误抛出异常，返回true表示成功。</returns>

    public bool SyncUser(String strUserXml)
    {
        //初始化类
        RTXObjectClass RTXObj = new RTXObjectClass();
        RTXCollectionClass RTXParams = new RTXCollectionClass();
        //初始化变量
        RTXObj.Name = "USERSYNC";
        RTXParams.Add("TEXTFORMAT", "0");//1 表示文本格式。0 表示XML格式。 系统默认为XML格式。
        RTXParams.Add("DEPTNAME", "临时部门");//默认人员的归属部门。
        RTXParams.Add("DATA", strUserXml);
        RTXParams.Add("MODIFYMODE", "0");//  1：表示修改模式，0 表示RTX组织机构同导入的信息完全一致。
        RTXParams.Add("SYNCPASSWORD", "1");// 1：同步密码，0 不同步密码。
        try
        {
            //RTXObj.Call2 (RTXServerApi.enumCommand_., RTXParams);// cmdSaveUserInfoToRTX=0x1,将XML或文本格式的用户与部门信息写到RTX中。
        }
        catch (COMException e)
        {
            throw e;
        }
        finally
        {
            RTXObj = null;
            RTXParams = null;
        }
        return true;
    }

    #endregion
    #endregion

}
