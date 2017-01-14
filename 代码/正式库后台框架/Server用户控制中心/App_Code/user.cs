using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;
using System.Configuration;

[WebService(Namespace = "http://user.ipc.com/", Description = "V1.00->用户控制中心业务类接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class user : System.Web.Services.WebService
{
    public user()
    {

        //如果使用设计的组件，请取消注释以下行
        //InitializeComponent(); 
    }

    /// <summary>
    /// 测试该接口是否还活着(每个接口必备)
    /// </summary>
    /// <param name="temp">随便传</param>
    /// <returns>返回ok就是接口正常</returns>
    [WebMethod(Description = "测试该接口是否还活着(每个接口必备)")]
    public string onlinetest(string temp)
    {
        //根据不同的传入值，后续可以检查不同的东西，比如这个接口所连接的数据库，比如进程池，服务器空间等等。。。
        return "ok";    

    }
    
    
    /// <summary>
    /// 用于检测或验证是否存在重复的登陆邮箱
    /// </summary>
    /// <param name="email">需要验证的邮箱</param>
    /// <returns>通过IsExistEmail方法重复返回“是”，否则返回“否”，如果出现错误，返回null</returns>
    /// Create by zzf  Date:2014.05.22
    [WebMethod(Description = "方法标记中文名：是否邮箱重复，用于检测或验证是否存在重复的登陆邮箱")]
    public string IsExistEmail_YHKZZX(string email)
    {
        try
        {
           return UserBasic.IsExistEmail(email); 
        }
        catch
        {
            return null;
        }      

    }

    /// <summary>
    /// 用于检测或验证除此之外，是否还存在同名的登陆邮箱
    /// </summary>
    /// <param name="email">需要验证的邮箱</param>
    /// <param name="number">此条记录的主键编号</param>
    /// <returns>通过IsExistEmail方法重复返回“是”，否则返回“否”，如果出现错误，返回null</returns>
    /// Create by zzf  Date:2014.05.22
    [WebMethod(Description = "方法标记中文名：是否邮箱除此重复，用于检测或验证除此之外，是否还存在同名的登陆邮箱")]
    public string IsExistEmailExThis_YHKZZX(string email,string number)
    {
        try
        {
            return UserBasic.IsExistEmail(email,number);
        }
        catch
        {
            return null;
        }

    }



    /// <summary>
    /// 用于检测或验证是否存在重复的用户名
    /// </summary>
    /// <param name="userName">需要验证的用户名</param>
    /// <returns>调用IsExistUname 重复返回“是”，否则返回“否”，读取数据库错误返回null </returns>
    /// Create by zzf  Date:2014.05.22
    [WebMethod(Description = "方法标记中文名：是否用户名重复，用于检测或验证是否存在重复的用户名")]
    public string IsExistUname_YHKZZX(string userName)
    {
        try
        {
            return UserBasic.IsExistUname(userName);
        }
        catch(Exception e)
        {
            //暂时
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, userName + "用户注册出错！" + e.ToString(), null);
            return null;
        }  
    }

    /// <summary>
    /// 用于检测或验证除此之外是否还存在同名的用户名
    /// </summary>
    /// <param name="userName">需要验证的用户名</param>
    /// <param name="number">本条记录的主键编号</param>
    /// <returns>调用IsExistUname 重复返回“是”，否则返回“否”，读取数据库错误返回null </returns>
    /// Create by zzf  Date:2014.05.22
    [WebMethod(Description = "方法标记中文名：是否用户名除此重复，用于检测或验证除此之外是否还存在同名的用户名")]
    public string IsExistUnameExthis_YHKZZX(string userName,string number)
    {
        try
        {
            return UserBasic.IsExistUname(userName,number);
        }
        catch
        {
            return null;
        }
    }  
   
    /// <summary>
    /// 用户注册
    /// 需要调用的前置服务：IsExistUname_YHKZZX(string userName)，string IsExistEmail_YHKZZX(string email)，生成主键 GetNextNumberZZ_XTKZ(string ModuleName, string HeadStr)
    /// SendEmailMes_YHKZZX，发送邮件信息
    /// </summary>
    /// <param name="uid">登录id，通常为邮箱</param>
    /// <param name="uname">用户名</param>
    /// <param name="pwd">密码</param>
    /// <param name="ip">ip地址</param>
    /// <param name="yzm">验证码</param>
    /// <returns>执行结果：包含：登录邮箱，用户名，密码，验证码，执行结果，原因</returns>
    /// Create by zzf  Date:2014.05.22
    [WebMethod(Description = "方法标记中文名：用户注册，用于用户注册第一步，使用之前先调用前置服务，参见方法说明")]
    public DataSet UserRegister_YHKZZX(string num, string uid, string uname, string pwd,string ip,string yzm)
    {
        try
        {
            return UserBasic.UserRegister(num,uid,uname,pwd,ip,yzm);
        }
        catch(Exception ex)
        {
            //暂时用于测试
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, uid + "用户注册出错！" + ex.ToString(), null);
            //FMipcClass.Log.WorkLog("用户注册", FMipcClass.Log.type.yewu, uid + "用户注册出错！" + ex.ToString(), null);
            return null;
        }  
    }

    /// <summary>
    /// 用于用户更换邮箱重新注册
    ///前置服务：IsExistEmailExThis_YHKZZX(string email,string number) IsExistUnameExthis_YHKZZX(string userName,string number)
    /// SendEmailMes_YHKZZX，发送邮件信息
    /// </summary>
    /// <param name="num"></param>
    /// <param name="uid">>登录id，通常为邮箱</param>
    /// <param name="uname">用户名</param>
    /// <param name="pwd">密码</param>
    /// <param name="yzm">验证码</param>
    /// <returns>执行结果</returns>
    [WebMethod(Description = "方法标记中文名：更换邮箱注册， 用于用户更换邮箱重新注册，参见方法说明")]
    public DataSet UserChangeRegister_YHKZZX(string num, string uid, string uname, string pwd,string yzm)
    {
        try
        {
            return UserBasic.UserChangeRegister(num, uid, uname, pwd,yzm);
        }
        catch
        {
            return null;
        }
    }


    /// <summary>
    /// 用于向用户的邮箱发送验证码等信息
    /// </summary>
    /// <param name="email">用户邮箱帐号</param>
    /// <param name="strSubject">邮件主题，例：中国商品批发交易平台 - 找回密码</param>
    /// <param name="valNum">验证码等信息</param>
    /// <param name="HtmlTemplatePath">邮件模版的路径，一般存放于用户控制中心HtmlTemplate文件夹下，例： "~/HtmlTemplate/EmailTemplate_Repwd.htm"</param>
    /// <returns></returns>
    /// Create by zzf  Date:2014.05.22
    [WebMethod(Description = "方法标记中文名：发送邮件信息，用于向用户的邮箱发送验证码等信息")]
    public DataSet SendEmailMes_YHKZZX(string email, string strSubject, string valNum,string HtmlTemplatePath)
    {
        try
        {
            return UserBasic.SendEmailMes(email, strSubject, valNum, HtmlTemplatePath);
 
        }
        catch
        {
            return null;
        }
 
    }


    /// <summary>
    /// 获取用户功能冻结信息
    /// </summary>
    /// <param name="userinfo">用户登录邮箱或者用户角色编号</param> 
    /// <returns>返回冻结信息,一个DataSet，包含：登录邮箱，是否冻结，冻结功能项</returns>
    /// Create by zzf  Date:2014.05.26
    [WebMethod(Description = "方法标记中文名：冻结信息，获取用户功能冻结信息")]
    public DataSet GetFrozenInfo_YHKZZX(string userinfo)
    {
        try
        {
            DataSet ds = new DataSet();
            if (userinfo.Trim() != "")
            ds=UserBasic.GetFrozenInfo(userinfo);

            return ds;
        }
        catch
        {
            return null;
        }
 
    }

    /// <summary>
    /// 判断具体的某项是否被冻结
    /// </summary>
    /// <param name="userinfo">登录邮箱或角色编号</param>
    /// <param name="item">要验证的项</param>
    /// <returns>是/否/""/null，冻结返回“是”，该用户不存在返回""，数据链接错误返回null</returns>
    /// Create by zzf  Date:2014.05.26
    [WebMethod(Description = "方法标记中文名：是否冻结，判断具体的一项是否冻结，如果要获取所有的冻结信息，请使用GetFrozenInfo_YHKZZX(string userinfo)服务方法")]
    public  string IsFrozen_YHKZZX(string userinfo, string item)
    {
        try
        {
            if (userinfo.Trim() != "")
                return UserBasic.IsFrozen(userinfo, item);

            else
                return "";
        }
        catch
        {
            return null;
        }
 
    }

    /// <summary>
    /// 获取用户所属管理部门
    /// </summary>
    /// <param name="userinfo">登录邮箱或角色编号</param>
    /// <returns>管理部门</returns>
    /// Create by zzf  Date:2014.05.26
    [WebMethod(Description = "方法标记中文名：管理部门，获取用户所属管理部门")]
    public string GetDepartment_YHKZZX(string userinfo)
    {
        try
        {
            if (userinfo.Trim() != "")
                return UserBasic.GetDepartment(userinfo);
            else
                return "";
        }
        catch
        {
            return null;
        }
 
    }

    /// <summary>
    /// 验证是否已经开通交易账户
    /// </summary>
    /// <param name="userinfo">角色编号或登录邮箱</param>
    /// <returns>结算账户类型，审核状态，如果找不到数据返回""，链接错误返回null</returns>
    /// Create by zzf  Date:2014.05.26
    [WebMethod(Description = "方法标记中文名：是否交易账户，验证是否已经开通交易账户")]
    public string IsDeelingAccount_YHKZZX(string userinfo)
    {
        try
        {
            if (userinfo.Trim() == "")
                return "";

            DataSet ds=UserBasic.GetDeelingAccount(userinfo);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count>0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows[0]["结算账户类型"].ToString().Trim() == "经纪人交易账户")
                {
                    if (dt.Rows[0]["是否已被分公司审核通过"].ToString() == "是")
                        return "结算账户类型：经纪人交易账户，审核：已通过";
                    else
                        return "结算账户类型：经纪人交易账户，审核：未通过";

                }
                else if (dt.Rows[0]["结算账户类型"].ToString().Trim() == "买家卖家交易账户")
                {
                    if (dt.Rows[0]["是否已被分公司审核通过"].ToString() == "是" && dt.Rows[0]["是否已被经纪人审核通过"].ToString() == "是")
                        return "结算账户类型：买家卖家交易账户，审核：已通过";
                    else
                        return "结算账户类型：买家卖家交易账户，审核：未通过";
                }
                else
                {
                    return "结算账户类型：尚未申请开通"; 
                }
                 
            }
            else
            return null;

        }
        catch
        {
            return null;
        }
 
    }

    /// <summary>
    /// 验证是否已经开通交易账户，这个方法返回的是一个数据集
    /// </summary>
    /// <param name="userinfo">角色编号或登录邮箱</param>
    /// <returns>一个结果数据集，含登录邮箱，结算账户类型，审核状态</returns>
    /// Create by zzf  Date:2014.06.11
    [WebMethod(Description = "方法标记中文名：交易账户开通状态，交易账户是否开通")]
    public DataSet StateOfDeelingAccount_YHKZZX(string userinfo)
    {

        try
        {
            DataSet dsReturn = new DataSet();
            if (userinfo.Trim() == "")
                return dsReturn;

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("登录邮箱", typeof(string));
            dtResult.Columns.Add("结算账户类型", typeof(string));
            dtResult.Columns.Add("审核状态", typeof(string));

            DataSet ds = UserBasic.GetDeelingAccount(userinfo);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows[0]["结算账户类型"].ToString().Trim() == "经纪人交易账户")
                {
                    if (dt.Rows[0]["分公司审核状态"].ToString() == "审核通过")
                    {
                        dtResult.Rows.Add(new object[] { dt.Rows[0]["登录邮箱"].ToString(), "经纪人交易账户", "已通过" });
                    }
                    else if (dt.Rows[0]["分公司审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dtResult.Rows.Add(new object[] { dt.Rows[0]["登录邮箱"].ToString(), "经纪人交易账户", "驳回" });
                    }
                    else if (dt.Rows[0]["分公司审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dtResult.Rows.Add(new object[] { dt.Rows[0]["登录邮箱"].ToString(), "经纪人交易账户", "审核中" });
                    }
                    else
                    {
                        dtResult.Rows.Add(new object[] { dt.Rows[0]["登录邮箱"].ToString(), "经纪人交易账户", "尚未申请开通" });
                    }


                }
                else if (dt.Rows[0]["结算账户类型"].ToString().Trim() == "买家卖家交易账户")
                {
                    if (dt.Rows[0]["经纪人审核状态"].ToString() == "审核通过" && dt.Rows[0]["分公司审核状态"].ToString() == "审核通过")
                    {
                        dtResult.Rows.Add(new object[] { dt.Rows[0]["登录邮箱"].ToString(), "买家卖家交易账户", "已通过" });
                    }
                    else if ((dt.Rows[0]["分公司审核状态"].ToString() == "审核通过" && dt.Rows[0]["经纪人审核状态"].ToString() == "审核中") || (dt.Rows[0]["分公司审核状态"].ToString() == "审核中" && dt.Rows[0]["经纪人审核状态"].ToString() == "审核通过") || (dt.Rows[0]["分公司审核状态"].ToString() == "审核中" && dt.Rows[0]["经纪人审核状态"].ToString() == "审核中"))
                    {
                        dtResult.Rows.Add(new object[] { dt.Rows[0]["登录邮箱"].ToString(), "买家卖家交易账户", "审核中" });
                    }
                    else if (dt.Rows[0]["分公司审核状态"].ToString().Trim().Equals("驳回") || dt.Rows[0]["经纪人审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dtResult.Rows.Add(new object[] { dt.Rows[0]["登录邮箱"].ToString(), "买家卖家交易账户", "驳回" });
                    }
                    else
                    {
                        dtResult.Rows.Add(new object[] { dt.Rows[0]["登录邮箱"].ToString(), "买家卖家交易账户", "尚未申请开通" });
                    }
                }
                else
                {
                    dtResult.Rows.Add(new object[] { dt.Rows[0]["登录邮箱"].ToString(), "", "尚未申请开通" });
                }

                dsReturn.Clear();
                dsReturn.Tables.Add(dtResult);
                return dsReturn;

            }
            else
            {
                dtResult.Rows.Add(new object[] { "", "", "尚未申请开通" });
                dsReturn.Tables.Add(dtResult);
                return dsReturn;
            }
        }
        catch
        {
            return null;
        }

    }

    /// <summary>
    /// 是否休眠，验证账户是否处于休眠状态
    /// </summary>
    /// <param name="useinfo">登录邮箱或角色编号</param>
    /// <returns>是/否，没有记录返回""，数据链接错误返回null</returns>
    /// create by zzf  Date:2014.05.27
    [WebMethod(Description = "方法标记中文名：是否休眠，验证账户是否处于休眠状态")]
    public string IsInSleep_YHKZZX(string useinfo)
    {
        try 
        {
            if (useinfo.Trim() != "")
                return UserBasic.IsInSleep(useinfo);
            else
                return "";
        }
        catch
        {
            return null;
        }
 
    }

    /// <summary>
    /// 获取用户基本信息
    /// 获取用户信息中的常用字段，若获取全部字段调用UserInfo(useinfo, "all")
    /// </summary>
    /// <param name="useinfo">角色编号或登录邮箱</param>
    /// <returns>用户基础数据集，列名为中文</returns>
    [WebMethod(Description = "方法标记中文名：用户基本信息，获取用户基本信息，如果对用户进行诸多验证，如冻结，休眠等等，可使用此方法")]
    public DataSet GetUserInfo_YHKZZX(string useinfo)
    {
        try
        {
            DataSet ds = new DataSet();
            if(useinfo.Trim()!="")
            ds= UserBasic.UserInfo(useinfo, "OFEN");

            return ds;
        }
        catch
        {
            return null;
        }

    }

    /// <summary>
    /// 获取用户基本信息，设置了三个标识，如果为OFEN，只返回常用字段，如果为ALL，返回全部字段，如果MANY返回多数字段
    /// </summary>
    /// <param name="useinfo">登录邮箱或角色编号</param>
    /// <param name="identifier">OFEN MANY ALL三选一</param>
    /// <returns></returns>
    [WebMethod(Description = "方法标记中文名：用户基本信息含标识，获取用户基本信息，含标识参数")]
    public DataSet GetUserInfoByIdfi_YHKZZX(string useinfo, string identifier)
    {
        try
        {
            DataSet ds = new DataSet();
            if (useinfo.Trim() != "" && identifier.Trim()!="")
            ds= UserBasic.UserInfo(useinfo, identifier);
            return ds;
        }
        catch
        {
            return null;
        }

    }

    /// <summary>
    /// 根据用户邮箱获取当前关联的默认有效经纪人信息
    /// 包含经纪人账户关联表中相关字段，以及下面的字段
    /// 当前默认经纪人是否被冻结/经纪人是否暂停新用户审核/经纪人暂停新用户审核时间/关联经纪人平台管理机构
    /// 经纪人资格证书编号/经纪人资格证书/经纪人资格证书有效期开始时间/经纪人资格证书有效期结束时间
    /// </summary>
    /// <param name="userEmail">用户登录邮箱</param>
    /// <returns>关联的经纪人信息，包含经纪人是否被冻结</returns>
    /// create by zzf  Date:2014.05.27
    [WebMethod(Description = "方法标记中文名：关联经纪人信息，根据用户邮箱获取当前关联的默认有效经纪人信息")]
    public DataSet GetJJRInfo_YHKZZX(string userEmail)
    {
        try
        {
            DataSet ds = new DataSet();
            if(userEmail.Trim()!="")
            ds= UserBasic.JJRInfo(userEmail);
            return ds;
        }
        catch
        {
            return null;
        }

    }

    /// <summary>
    /// 根据经纪人角色编号获取经纪人信息
    /// </summary>
    /// <param name="jjrJSBH">经纪人角色编号</param>
    /// <returns>经纪人信息数据集,基本涵盖经纪人的所有信息</returns>
    /// create by zzf  Date:2014.05.28
    [WebMethod(Description = "方法标记中文名：角色获取经纪信息，根据经纪人角色编号获取经纪人信息")]
    public DataSet GetJJRInfoByJSBH(string jjrJSBH)
    {
        try
        {
            DataSet ds = new DataSet();
            if(jjrJSBH.Trim()!="")
            ds= UserBasic.GetJJRinfoByKind(jjrJSBH,"JSBH");

            return ds;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 根据证书编号获取经纪人信息
    /// </summary>
    /// <param name="zsbh">证书编号</param>
    /// <returns>经纪人信息数据集,基本涵盖经纪人的所有信息</returns>
    /// create by zzf  Date:2014.05.28
    [WebMethod(Description = "方法标记中文名：证书获取经纪信息，根据证书编号获取经纪人信息")]
    public DataSet GetJJRInfoByZSBH(string zsbh)
    {
        try
        {
            DataSet ds = new DataSet();
            if(zsbh.Trim()!="")
            ds= UserBasic.GetJJRinfoByKind(zsbh, "ZSBH");

            return ds;
        }
        catch
        {
            return null;
        } 
    }

    /// <summary>
    /// 获取账户当前可用余额
    /// </summary>
    /// <param name="userinfo">登录邮箱或角色编号</param>
    /// <returns>当前账户可用余额</returns>
    /// create by zzf  Date:2014.05.29
    [WebMethod(Description = "方法标记中文名：账户当前可用余额，获取账户当前可用余额")]
    public string GetZHKYYE_YHKZZX(string userinfo)
    {
        try
        {
            if (userinfo != "")
            {
                DataSet ds= UserBasic.GetYE(userinfo);

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["账户当前可用余额"].ToString();
                }
                else
                    return "";
            }
               
            else
                return "";

        }
        catch
        {
            return null;
        } 
 
    }

    /// <summary>
    /// 获取第三方存管可用余额
    /// </summary>
    /// <param name="userinfo">登录邮箱或角色编号</param>
    /// <returns>第三方存管可用余额</returns>
    /// create by zzf  Date:2014.05.29
    [WebMethod(Description = "方法标记中文名：第三方存管可用余额，获取第三方存管可用余额")]
    public string GetDSFCGYE_YHKZZX(string userinfo)
    {
        try
        {
            if (userinfo != "")
            {
                DataSet ds = UserBasic.GetYE(userinfo);

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["第三方存管可用余额"].ToString();
                }
                else
                    return "";
            }

            else
                return "";

        }
        catch
        {
            return null;
        } 
 
    }

    /// <summary>
    /// 获取余额，包括第三方存管可用余额以及当前账户可用余额
    /// </summary>
    /// <param name="userinfo">登录邮箱或角色编号</param>
    /// <returns>余额</returns>
    /// create by zzf  Date:2014.05.29
    [WebMethod(Description = "方法标记中文名：获取余额，包括第三方存管可用余额以及当前账户可用余额")]
    public DataSet GetYE_YHKZZX(string userinfo)
    {
        try
        {
            DataSet ds = new DataSet();
            if (userinfo != "")
            {
                ds = UserBasic.GetYE(userinfo);
                               
            }
            return ds;
           
        }
        catch
        {
            return null;
        } 
 
    }

    /// <summary>
    /// 买卖家是否被审核通过
    /// </summary>
    /// <param name="strMMJDLYX">买卖家登录邮箱</param>
    /// <param name="strGLJJRBH">经纪人角色编号</param>
    /// <returns>是/否</returns>
    /// create by zzf  Date:2014.05.29
    [WebMethod(Description = "方法标记中文名：买卖家审核通过，判断买卖家是否被经纪人审核通过")]
    public  string IsJJRPassMMJ_YHKZZX(string strMMJDLYX, string strGLJJRBH)
    {
        try
        {
            if (strGLJJRBH != "" && strMMJDLYX != "")
                return UserBasic.IsJJRPassMMJ(strMMJDLYX, strGLJJRBH);
            else
                return "";
        }
        catch
        {
            return null; 
        }
 
    }

    /// <summary>
    /// 判断当前买卖家交易账户 是否更换了注册类别
    /// </summary>
    /// <param name="strSelerJSBH">卖家角色编号</param>
    /// <param name="strGLJJRBH">关联经纪人角色编号</param>
    /// <param name="strZCLB">当前注册类别</param>
    /// <returns>否 没有更换注册类别，是 已经更换了注册类别</returns>
    /// create by zzf  Date:2014.05.30
    [WebMethod(Description = "方法标记中文名：是否更换注册类别，判断交易账户是否更换了注册类别")]
    public string IsGHZCLB_YHKZZX(string strSelerJSBH, string strGLJJRBH, string strZCLB)
    {
        try
        {
            if (strSelerJSBH != "" && strGLJJRBH != "" && strZCLB != "")
            {
                return UserBasic.IsGHZCLB(strSelerJSBH, strGLJJRBH, strZCLB);
            }
            else
            {
                return "";
            }
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 判断交易账户是否更换了经纪人
    /// </summary>
    /// <param name="strSelerJSBH">卖家角色编号</param>
    /// <param name="strGLJJRBH">关联经纪人角色编号</param>
    /// <returns>是/否</returns>
    /// create by zzf  Date:2014.05.30
    [WebMethod(Description = "方法标记中文名：是否更换经纪人，判断交易账户是否更换了经纪人")]
    public string IsGHJJR_YHKZZX(string strSelerJSBH, string strGLJJRBH)
    {
        try
        {
            if (strSelerJSBH != "" && strGLJJRBH != "" )
            {
                return UserBasic.IsGHJJR(strSelerJSBH, strGLJJRBH);
            }
            else
            {
                return "";
            }
        }
        catch
        {
            return null;
        }
 
    }

    /// <summary>
    /// 判断经纪人是否暂停了交易账户的审核
    /// </summary>
    /// <param name="jjrinfo">登录邮箱或角色编号</param>
    /// <returns>结果：是/否</returns>
    [WebMethod(Description = "方法标记中文名：经纪人暂停审核，判断经纪人是否暂停了交易账户的审核")]
    public string IsJJRZTSH_YHKZZX(string jjrinfo)
    {
        try
        {
            if (jjrinfo != "" )
            {
                return UserBasic.IsJJRZTSH(jjrinfo);
            }
            else
            {
                return "";
            }
        }
        catch
        {
            return null;
        }
 
    }

    /// <summary>
    /// 获取审核状态，包括是否被经纪人审核通过，是否已被分公司审核通过
    /// </summary>
    /// <param name="userinfo">登录邮箱 2014.10不再支持角色编号，支持会多耗时81毫秒</param>
    /// <returns>一个数据集，包括 登录邮箱 结算账户类型，是否被经纪人审核通过，是否已被分公司审核通过</returns>
    /// create by zzf  Date:2014.05.30
    [WebMethod(Description = "方法标记中文名：审核状态，包括是否被经纪人审核通过，是否已被分公司审核通过")]
    public DataSet GetSHZT_YHKZZX(string userinfo)
    {
        try
        {
            DataSet ds = new DataSet();
            if (userinfo != "")
            {
                ds = UserBasic.GetSHZT(userinfo);

            }
            return ds;

        }
        catch
        {
            return null;
        }

    }

    /// <summary>
    /// 用于重新发送验证码时更新登陆账号信息表中的验证码信息
    /// 前置服务SendEmailMes_YHKZZX，用来发送邮件验证码
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="valNum">验证码</param>
    /// <returns>更新成功/更新失败/参数为空</returns>
    [WebMethod(Description = "方法标记中文名：更新注册验证码，重新发送验证码时更新登陆账号信息表中的验证码信息")]
    public string ReValNum_YHKZZX(string dlyx, string valNum)
    {
        try
        {
       
            if (dlyx != "" && valNum !="")
            {
                if (UserBasic.ReValNum(dlyx, valNum))
                    return "更新成功";
                else
                    return "更新失败";

            }
            else
                return "参数为空";

        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 忘记密码验证码，用户忘记密码时向忘记密码验证码表写入信息
    /// 后置服务SendEmailMes_YHKZZX，用来发送邮件验证码，或者向手机发送消息的服务
    /// </summary>
    /// <param name="user">登录邮箱</param>
    /// <param name="typeToSet">类型：email,phone</param>
    /// <param name="number">主键</param>
    /// <param name="RandomNumber">验证码</param>
    /// <param name="ip">ip地址</param>
    /// <returns>执行结果，一个DataSet，含RESULT列，成功为“SUCCESS”</returns>
     [WebMethod(Description = "方法标记中文名：忘记密码验证码，用户忘记密码时向忘记密码验证码表写入信息")]
    public DataSet FPwdValNumber_YHKZZX(string user, string typeToSet, string number, string RandomNumber, string ip)
    {
        try
        {
            DataSet ds = new DataSet();
            if (user != "" && typeToSet != "" && number != "" && RandomNumber != "" && ip != "")
            {
                ds = UserBasic.FPwdValNumber(user ,typeToSet,number, RandomNumber,ip);
            }

            return ds;
        }
        catch
        {
            return null;

        }
    }

     /// <summary>
     /// 用于密码的修改
     /// </summary>
     /// <param name="uid">用户登录邮箱</param>
     /// <param name="phoneoremail">找回密码的email或者手机号</param>
     /// <param name="pwd">新密码</param>
     /// <param name="valNum">验证码</param>
     /// <param name="typeToSet">找回密码的方式</param>
     /// <returns>结果数据集：Uid，Email，Phone，NewPwd，Results</returns>
     /// create by zzf  Date:2014.06.06
     [WebMethod(Description = "方法标记中文名：忘记密码修改，忘记密码时通过邮箱或者手机重新修改密码")]
     public DataSet ChangePassword_YHKZZX(string uid, string phoneoremail, string pwd, string valNum, string typeToSet)
     {
         try
         {
             DataSet ds = new DataSet();
             if (uid != "" && typeToSet != "" && phoneoremail != "" && pwd != "" && valNum != "")
             {
                 ds = UserBasic.ChangePassword(uid,phoneoremail,pwd,valNum,typeToSet);
             }

             return ds;
         }
         catch
         {
             return null;

         } 
     }

     /// <summary>
     /// 账户激活，用于用户注册第三步，账户的激活
     /// </summary>
     /// <param name="uid">用户登录邮箱</param>
     /// <returns>执行结果数据集DLYX，YHM，DLMM，RESULT，REASON</returns>
     /// create by zzf  Date:2014.06.06
     [WebMethod(Description = "方法标记中文名：账户激活，用于用户注册第三步，账户的激活")]
     public DataSet AccountID_YHKZZX(string uid)
     {
         try
         {
             DataSet ds = new DataSet();
             if (uid != "")
             {
                 ds = UserBasic.AccountID(uid);
             }

             return ds;
         }
         catch
         {
             return null;
         }

     }

     /// <summary>
     /// 登录密码修改，用于在登录账户之后重新修改密码，不同于忘记密码时的密码找回
     /// 前面需验证“是否休眠”IsInSleep_YHKZZX(string useinfo)
     /// </summary>
     /// <param name="dlyx"></param>
     /// <param name="xmm"></param>
     /// <returns>执行结果</returns>
     /// create by zzf  Date:2014.06.09
     [WebMethod(Description = "方法标记中文名：登录密码修改，用于在登录账户之后重新修改密码")]
     public  DataSet NPsdChange_YHKZZX(string dlyx, string xmm)
     {
         try
         {
             DataSet ds = new DataSet();
             if (dlyx != "" && xmm!="")
             {
                 ds = UserBasic.NPsdChange(dlyx,xmm);
             }

             return ds;
         }
         catch
         {
             return null;
         } 
     }

    /// <summary>
     /// 用于在登录账户之后修改资金交易密码
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="xmm">新密码</param>
    /// <returns>执行结果</returns>
     /// create by zzf  Date:2014.06.09
     [WebMethod(Description = "方法标记中文名：资金密码修改，用于在登录账户之后修改资金交易密码")]
     public  DataSet ZJPsdChange_YHKZZX(string dlyx, string xmm)
     {
         try
         {
             DataSet ds = new DataSet();
             if (dlyx != "" && xmm != "")
             {
                 ds = UserBasic.ZJPsdChange(dlyx, xmm);
             }

             return ds;
         }
         catch
         {
             return null;
         }  
     }


    /// <summary>
    /// 休眠账户的激活
    /// 前置服务：判断是否已被激活，防止重复 IsInSleep_YHKZZX(string useinfo)
    /// 前置服务：获取当前可用余额，是否大于100 GetZHKYYE_YHKZZX(string userinfo)
    /// 前置服务：系统控制中心“是否在服务时间”
    /// 前置服务：系统控制中心“获取帐款流水明细表的主键”，用于参数
    /// </summary>
    /// <param name="zklsnum">要写入的帐款流水明细表的主键，需要传入</param>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="jszhlx">结算账户类型</param>      
    /// <returns>一个执行结果数据集</returns>
    /// create by zzf  Date:2014.06.09
     [WebMethod(Description = "方法标记中文名：休眠激活，用于休眠账户的激活")]
     public DataSet AccountWakeUp_YHKZZX(string zklsnum, string dlyx, string jszhlx)
     {
         try
         {
             DataSet ds = new DataSet();
             if (dlyx != "" && zklsnum != "" && jszhlx!="" )
             {
                 ds = UserBasic.AccountWakeUp(zklsnum,dlyx,jszhlx);
             }

             return ds;
         }
         catch
         {
             return null;
         }  
 
     }

     /// <summary>
    /// 使帐户休眠
    /// 前置服务：IsInSleep_YHKZZX(string useinfo)，用来判断是否已经休眠
    /// </summary>
    /// <param name="email">用户登录邮箱</param>
    /// <returns>成功返回"SUCCESS"</returns>
     [WebMethod(Description = "方法标记中文名：使账户休眠，用于使账户休眠")]
     public string AccountSleep_YHKZZX(string email)
     {
         try
         {
             string s = "";
             if (email!= "" )
             {
                 s = UserBasic.UserSleep(email);
             }

             return s;
         }
         catch
         {
             return null;
         }  
 
     }

     /// <summary>
     /// 用于开通交易账户时的登录信息检查
     /// 前置服务：系统控制中心：验证登陆账号是否允许登陆GetOpen_XTKZ(string email)
     /// 前置服务：IsExistEmail_YHKZZX(string email)
     /// </summary>
     /// <param name="user">登录邮箱</param>
     /// <param name="pass">登录密码</param>
     /// <param name="ip">客户端ip</param>
     /// <returns>结果数据集，异常一般只包含结果信息，正常包含三个数据表：用户信息，关联信息，结果信息</returns>
     /// create by zzf  Date:2014.06.10
     [WebMethod(Description = "方法标记中文名：登录信息检查，用于开通交易账户时的登录信息检查")]
     public  DataSet CheckLoginIn_YHKZZX(string user, string pass, string ip)
     {
         try
         {
             DataSet ds = new DataSet();
             if (user != "" && pass!="" && ip !="")
             {
                 ds = UserBasic.CheckLoginIn(user,pass,ip);
             }

             return ds;
         }
         catch
         {
             return null;
         }   
     }

    /// <summary>
    /// 通过登录邮箱获取用户信息和关联信息
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <returns>数据集，包含用户信息表和关联信息表</returns>
    /// create by zzf  Date:2014.06.11
     [WebMethod(Description = "方法标记中文名：用户及关联信息，通过登录邮箱获取用户信息和关联信息")]
     public DataSet UserAndRelatedInfo_YHKZZX(string dlyx)
     {
         try
         {
             DataSet ds = new DataSet();
             if (dlyx != "" )
             {
                 ds = UserBasic.UserAndRelatedInfo(dlyx);
             }

             return ds;
         }
         catch
         {
             return null;
         }   
     }

     /// <summary>
     ///  提交，开通【经纪人、单位】的交易账户信息
     ///  前置服务：判断是否开通交易账户IsDeelingAccount_YHKZZX(string userinfo)
     ///  前置服务：调用系统控制中心的GetNextNumberZZ_XTKZ方法来生成“关联经纪人信息表AAA_MJMJJYZHYJJRZHGLB”的主键
     ///  执行此方法后，调用用户控制中心UserAndRelatedInfo_YHKZZX(string dlyx)重新获取用户信息与关联信息
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param>
     /// <param name="keyNumber">需要传入的“关联经纪人信息表”的主键</param>
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：开通单位经纪人账户，提交，开通单位类型经纪人的交易账户信息")]
     public DataSet SubmitJJRDW_YHKZZX(DataTable dataTable, string keyNumber)
     {
         try
         {
             DataSet ds = new DataSet();
             if (keyNumber != "" && dataTable!=null)
             {
                 ds = UserBasic.SubmitJJRDW(dataTable,keyNumber);
             }

             return ds;
         }
         catch (Exception ex)
         {
             FMipcClass.Log.WorkLog("开通单位经纪人账户", FMipcClass.Log.type.debug, ex.ToString(), new DataSet());
             return null;
         }   
 
     }


     /// <summary>
     ///  提交，开通【经纪人、个人】的交易账户信息
     ///  前置服务：判断是否开通交易账户IsDeelingAccount_YHKZZX(string userinfo)
     ///  前置服务：调用系统控制中心的GetNextNumberZZ_XTKZ方法来生成“关联经纪人信息表AAA_MJMJJYZHYJJRZHGLB”的主键
     ///  执行此方法后，调用用户控制中心UserAndRelatedInfo_YHKZZX(string dlyx)重新获取用户信息与关联信息
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param>
     /// <param name="keyNumber">需要传入的“关联经纪人信息表”的主键</param>
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：开通个人经纪人账户，提交，开通个人类型经纪人的交易账户信息")]
     public DataSet SubmitJJRGR_YHKZZX(DataTable dataTable, string keyNumber)
     {
         try
         {
             
             DataSet ds = new DataSet();
             if (keyNumber != "" && dataTable != null)
             {
                 ds = UserBasic.SubmitJJRGR(dataTable, keyNumber);
             }
             
             return ds;
         }
         catch(Exception ex)
         {
             //FMipcClass.Log.WorkLog("开通个人经纪人账户", FMipcClass.Log.type.debug, ex.ToString(),null);
             //暂时用于测试
             FMipcClass.Log.WorkLog(" C下达预订单", FMipcClass.Log.type.debug, ex.ToString(), null);
            
             return null;
         }   
     }


     /// <summary>
     ///  提交，开通【买卖家、单位】的交易账户信息
     ///  前置服务：判断是否开通交易账户IsDeelingAccount_YHKZZX(string userinfo)
     ///  前置服务：调用系统控制中心的GetNextNumberZZ_XTKZ方法来生成“关联经纪人信息表AAA_MJMJJYZHYJJRZHGLB”的主键
     ///  执行此方法后，调用用户控制中心UserAndRelatedInfo_YHKZZX(string dlyx)重新获取用户信息与关联信息
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param>
     /// <param name="keyNumber">需要传入的“关联经纪人信息表”的主键</param>
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：开通单位买卖家账户，提交，开通单位类型买卖家的交易账户信息")]
     public DataSet SubmitMMJDW_YHKZZX(DataTable dataTable, string keyNumber)
     {
         try
         {
             DataSet ds = new DataSet();
             if (keyNumber != "" && dataTable != null)
             {
                 ds = UserBasic.SubmitMMJDW(dataTable, keyNumber);
             }

             return ds;
         }
         catch(Exception ex)
         {
             FMipcClass.Log.WorkLog("开通单位买卖家账户", FMipcClass.Log.type.debug, ex.ToString(), new DataSet());
             return null;
         }   
     }

     /// <summary>
     ///  提交，开通【买卖家、个人】的交易账户信息
     ///  前置服务：判断是否开通交易账户IsDeelingAccount_YHKZZX(string userinfo)
     ///  前置服务：调用系统控制中心的GetNextNumberZZ_XTKZ方法来生成“关联经纪人信息表AAA_MJMJJYZHYJJRZHGLB”的主键
     ///  执行此方法后，调用用户控制中心UserAndRelatedInfo_YHKZZX(string dlyx)重新获取用户信息与关联信息
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param>
     /// <param name="keyNumber">需要传入的“关联经纪人信息表”的主键</param>
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：开通个人买卖家账户，提交，开通个人类型买卖家的交易账户信息")]
     public DataSet SubmitMMJGR_YHKZZX(DataTable dataTable, string keyNumber)
     { 
         try
         {
             DataSet ds = new DataSet();
             if (keyNumber != "" && dataTable != null)
             {
                 ds = UserBasic.SubmitMMJGR(dataTable, keyNumber);
             }

             return ds;
         }
         catch (Exception ex)
         {
             FMipcClass.Log.WorkLog("开通个人买卖家账户", FMipcClass.Log.type.debug, ex.ToString(), new DataSet());
             return null;
         }   
     }

     /// <summary>
     ///  修改【经纪人、单位】的交易账户信息
     ///  前置服务：判断是否休眠IsInSleep_YHKZZX(string useinfo)
     ///  前置服务：调用系统控制中心的GetNextNumberZZ_XTKZ方法来生成“关联经纪人信息表AAA_MJMJJYZHYJJRZHGLB”的主键
     ///  执行此方法后，调用用户控制中心UserAndRelatedInfo_YHKZZX(string dlyx)重新获取用户信息与关联信息
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param>
     /// <param name="keyNumber">需要传入的“关联经纪人信息表”的主键</param>
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：修改单位经纪人账户，修改单位类型经纪人的交易账户信息")]
     public DataSet UpdateJJRDW_YHKZZX(DataTable dataTable, string keyNumber)
     {
         try
         {
             DataSet ds = new DataSet();
             if ( dataTable != null)
             {
                 ds = UserBasic.UpdateJJRDW(dataTable, keyNumber);
             }

             return ds;
         }
         catch
         {
             return null;
         }   
     }


     /// <summary>
     ///  修改【经纪人、个人】的交易账户信息
     ///  前置服务：判断是否休眠IsInSleep_YHKZZX(string useinfo)
     ///  前置服务：调用系统控制中心的GetNextNumberZZ_XTKZ方法来生成“关联经纪人信息表AAA_MJMJJYZHYJJRZHGLB”的主键
     ///  执行此方法后，调用用户控制中心UserAndRelatedInfo_YHKZZX(string dlyx)重新获取用户信息与关联信息
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param>
     /// <param name="keyNumber">需要传入的“关联经纪人信息表”的主键</param>
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：修改个人经纪人账户，修改个人类型经纪人的交易账户信息")]
     public DataSet UpdateJJRGR_YHKZZX(DataTable dataTable, string keyNumber)
     {
         try
         {
             DataSet ds = new DataSet();
             if ( dataTable != null)
             {
                 ds = UserBasic.UpdateJJRGR(dataTable, keyNumber);
             }

             return ds;
         }
         catch
         {
             return null;
         }   
 
     }


     /// <summary>
     ///  修改【买卖家、单位】的交易账户信息
     ///  前置服务：判断是否休眠IsInSleep_YHKZZX(string useinfo)
     ///  前置服务：判断是否更换了经纪人IsGHJJR_YHKZZX(string strSelerJSBH, string strGLJJRBH)，如果更换了经纪人，则不再进行
     ///  前置服务：调用系统控制中心的GetNextNumberZZ_XTKZ方法来生成“关联经纪人信息表AAA_MJMJJYZHYJJRZHGLB”的主键
     ///  执行此方法后，调用用户控制中心UserAndRelatedInfo_YHKZZX(string dlyx)重新获取用户信息与关联信息
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param>
     /// <param name="keyNumber">需要传入的“关联经纪人信息表”的主键</param>
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：修改单位买卖家账户，修改单位类型买卖家交易账户信息")]
     public  DataSet UpdateMMJDW_YHKZZX(DataTable dataTable, string keyNumber)
     {
         try
         {
             DataSet ds = new DataSet();
             if ( dataTable != null)
             {
                 ds = UserBasic.UpdateMMJDW(dataTable, keyNumber);
             }

             return ds;
         }
         catch
         {
             return null;
         }   
     }

     /// <summary>
     ///  修改【买卖家、个人】的交易账户信息
     ///  前置服务：判断是否休眠IsInSleep_YHKZZX(string useinfo)
     ///  前置服务：判断是否更换了经纪人IsGHJJR_YHKZZX(string strSelerJSBH, string strGLJJRBH)，如果更换了经纪人，则不再进行
     ///  前置服务：调用系统控制中心的GetNextNumberZZ_XTKZ方法来生成“关联经纪人信息表AAA_MJMJJYZHYJJRZHGLB”的主键
     ///  执行此方法后，调用用户控制中心UserAndRelatedInfo_YHKZZX(string dlyx)重新获取用户信息与关联信息
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param>
     /// <param name="keyNumber">需要传入的“关联经纪人信息表”的主键</param>
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：修改个人买卖家账户，修改个人类型买卖家的交易账户信息")]
     public DataSet UpdateMMJGR_YHKZZX(DataTable dataTable, string keyNumber)
     {
         try
         {
             DataSet ds = new DataSet();
             if ( dataTable != null)
             {
                 ds = UserBasic.UpdateMMJGR(dataTable, keyNumber);
             }

             return ds;
         }
         catch
         {
             return null;
         }   
 
     }

     /// <summary>
     ///  经纪人审核通过买卖家
     ///  前置服务：判断是否休眠IsInSleep_YHKZZX(string useinfo)
     ///  前置服务：判断是否开通交易账户IsDeelingAccount_YHKZZX(string userinfo)   
     ///  执行此方法后，如果执行结果为ok，运行系统控制中心Sendmes_XTKZ(DataSet ds)服务向申请方发送提醒
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param> 
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：经纪人通过买卖家，经纪人通过对买卖家信息的审核")]
     public DataSet JJRPassMMJ_YHKZZX(DataTable dataTable)
     {
         try
         {
             DataSet ds = new DataSet();
             if (dataTable != null)
             {
                 ds = UserBasic.JJRPassMMJ(dataTable);
             }

             return ds;
         }
         catch
         {
             return null;
         }    
     }

     /// <summary>
     ///  经纪人驳回买卖家的申请
     ///  前置服务：判断是否休眠IsInSleep_YHKZZX(string useinfo)
     ///  前置服务：判断是否开通交易账户IsDeelingAccount_YHKZZX(string userinfo)   
     ///  执行此方法后，如果执行结果为ok，运行系统控制中心Sendmes_XTKZ(DataSet ds)服务向申请方发送提醒
     ///  后置服务：驳回扣减申请方分数0.5
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param> 
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：经纪人驳回买卖家，经纪人驳回了对买卖家的申请")]
     public  DataSet JJRRejectMMJ_YHKZZX(DataTable dataTable)
     {
         try
         {
             DataSet ds = new DataSet();
             if (dataTable != null)
             {
                 ds = UserBasic.JJRRejectMMJ(dataTable);
             }

             return ds;
         }
         catch
         {
             return null;
         }     
     }
    
     /// <summary>
     ///  账户维护选择关联经纪人
     ///  前置服务：判断是否休眠IsInSleep_YHKZZX(string useinfo)
     ///  前置服务：判断是否开通交易账户IsDeelingAccount_YHKZZX(string userinfo) 
     ///  前置服务：调用系统控制中心的GetNextNumberZZ_XTKZ方法来生成“关联经纪人信息表AAA_MJMJJYZHYJJRZHGLB”的主键
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param>
     /// <param name="keyNumber">需要传入的“关联经纪人信息表”的主键</param>
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：选择经纪人，账户维护时选择要关联的经纪人")]
     public DataSet ChooseJJR_YHKZZX(DataTable dataTable, string keyNumber)
     {
         try
         {
             DataSet ds = new DataSet();
             if (keyNumber != "" && dataTable != null)
             {
                 ds = UserBasic.ChooseJJR(dataTable, keyNumber);
             }

             return ds;
         }
         catch
         {
             return null;
         }   
 
     }

     /// <summary>
     ///  经纪人暂停或恢复新用户的审核
     ///  前置服务：判断是否休眠IsInSleep_YHKZZX(string useinfo)
     ///  前置服务：判断是否开通交易账户IsDeelingAccount_YHKZZX(string userinfo) 
     ///  前置服务：判断“经纪人暂停代理新业务”这一项是否冻结，string IsFrozen_YHKZZX(string userinfo, string item)
     ///  前置服务：IsJJRZTSH_YHKZZX(string jjrinfo)是否已暂停，防止并发
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param>  
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：暂停或恢复审核，经纪人暂停或恢复新用户的审核")]
     public DataSet ZTHFSH_YHKZZX(DataTable dataTable)
     {
         try
         {
             DataSet ds = new DataSet();
             if (dataTable != null)
             {
                 ds = UserBasic.ZTHFSH(dataTable);
             }

             return ds;
         }
         catch
         {
             return null;
         }   
     }

     /// <summary>
     ///  经纪人暂停或恢复用户的新业务
     ///  前置服务：判断是否休眠IsInSleep_YHKZZX(string useinfo)
     ///  前置服务：判断是否开通交易账户IsDeelingAccount_YHKZZX(string userinfo) 
     ///  前置服务：判断“经纪人暂停用户新业务”这一项是否冻结，string IsFrozen_YHKZZX(string userinfo, string item)     
     /// </summary>
     /// <param name="dataTable">包含一系列要传递的参数</param>  
     /// <returns>一个包含执行结果和提示文本的数据集</returns>
     [WebMethod(Description = "方法标记中文名：暂停或恢复新业务，经纪人暂停或恢复新用户的新业务")]
     public DataSet ZTHFYW_YHKZZX(DataTable dataTable)
     {
         try
         {
             DataSet ds = new DataSet();
             if (dataTable != null)
             {
                 ds = UserBasic.ZTHFYW(dataTable);
             }

             return ds;
         }
         catch
         {
             return null;
         }   
 
     }     

    /// <summary>
    /// 根据Number获取登陆表相应交易方名称、资料提交时间
    /// </summary>
    /// <param name="number">登录表主键number</param>
    /// <returns></returns>
     [WebMethod(MessageName = "获取交易方名称及资料提交时间", Description = "摸板-根据Number获取登陆表相应交易方名称、资料提交时间")]
     public DataSet GetJJFMCByNumber_YHKZZX(string number)
     {
         try
         {
             return UserBasic.GetJJFMCByNumber(number); ;
         }
         catch
         {
             return null;
         }

     }

    /// <summary>
    /// 用于经纪人审核买卖家时获取买卖家信息
    /// </summary>
    /// <param name="dtInfor">参数数据表</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.7.7
    [WebMethod(Description = "方法标记中文名：获取买卖家信息，用于经纪人审核买卖家是获取买卖家信息")]
     public DataSet GetMMJData_YHKZZX(DataTable dtInfor)
     {
         try
         {
             return UserBasic.GetMMJData(dtInfor); ;
         }
         catch
         {
             return null;
         }
     }
    /// <summary>
    /// 用于更换经纪人获取与之相关的信息
    /// </summary>
    /// <param name="dataTable">参数数据表</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.7.11
    [WebMethod(Description = "方法标记中文名：获取更换经纪人信息，用于更换经纪人获取与之相关的信息")]
    public DataSet GetSiftJJRData_YHKZZX(DataTable dataTable)
    {
        try
        {
            return UserBasic.GetSiftJJRData(dataTable);
        }
        catch
        {
            return null;
        }
 
    }

    /// <summary>
    ///提交银行人员信息表 wyh 2014.07.12 
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    [WebMethod(MessageName = "提交银行人员信息表", Description = "提交银行人员信息表")]
    public DataSet SubmitYHYHInfor_YHKZZX(DataTable dataTable)
    {
        try
        {
            return UserBasic.SubmitYHYHInfor(dataTable);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    ///修改银行人员信息表
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns>返回符合结构的dsreturn</returns>
    [WebMethod(MessageName = "修改银行人员信息表", Description = "修改银行人员信息表")]
    public DataSet ModifyYHYHInfor_YHKZZX(DataTable dataTable)
    {
        try
        {
            return UserBasic.ModifyYHYHInfor(dataTable);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 获取买卖家账户的基本信息
    /// </summary>
    /// <param name="jsbh">卖家角色编号</param>
    /// <returns>结果数据集</returns>
    [WebMethod(Description = "方法标记中文名：买卖家账户基本信息，获取买卖家账户基本信息，目前仅应用于维护账户")]
    public  DataSet MMJBasicData_YHKZZX(string jsbh)
    {
        try
        {
            if (jsbh != "")
                return UserBasic.MMJBasicData(jsbh);
            else
                return new DataSet();
        }
        catch
        {
            return null;
        } 
    }

    /// <summary>
    /// 获取经纪人账户基本信息
    /// </summary>
    /// <param name="jsbh">经纪人角色编号</param>
    /// <returns></returns>
    [WebMethod(Description = "方法标记中文名：经纪人账户基本信息，获取经纪人账户基本信息，目前仅应用于维护账户")]
    public  DataSet JJRBasicData_YHKZZX(string jsbh)
    {
        try
        {
            if (jsbh != "")
                return UserBasic.JJRBasicData(jsbh);
            else
                return new DataSet();
        }
        catch
        {
            return null;

        } 
    }


      /// <summary>
     /// 删除银行员工信息 
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns> </returns>
    [WebMethod(MessageName = "删除银行员工信息", Description = "删除银行员工信息")]
    public DataSet DeleteYHYHInfor_YHKZZX(DataTable dt)
    {
        try
        {

            return UserBasic.DeleteYHYHInfor(dt);
          
        }
        catch
        {
            return null;

        } 
    }
    /// <summary>
    /// 信用等级积分处理
    /// </summary>
    /// <param name="ywname">业务类型</param>
    /// <param name="dt">合同编号数据表</param>    /// 
    /// <returns></returns>
    [WebMethod(MessageName = "信用等级积分处理", Description = "根据业务点确定信用积分增加扣减并写数据库")]
    public bool userXYJF(string ywname,DataTable dt)
    {
        try
        {
            XYDJJFCL xyjf = new XYDJJFCL();
            return xyjf.DealXYJF(dt, ywname);
        }
        catch
        {
            return false;
        }
    }

}