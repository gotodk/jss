using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FMDBHelperClass;
using System.Data;
using System.Configuration;
using System.Collections;
using FMPublicClass;
using FMipcClass;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


/// <summary>
/// 封装了一些基本方法，用于用户控制中心所包含的服务的调用
/// Create by zzf  Date:2014.05
/// </summary>
public class UserBasic
{
    public UserBasic()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 用于检测或验证是否存在重复的登陆邮箱
    /// </summary>
    /// <param name="email">需要验证的邮箱</param>
    /// <returns>重复返回“是”，否则返回“否”，程序出错返回null</returns>
    /// Create by zzf  Date:2014.05.22   
    public static string IsExistEmail(string email)
    {
        DBFactory I_DBF = new DBFactory();
        //I_Dblink I_DBL = I_DBF.DbLinkSqlMain(ConfigurationManager.ConnectionStrings["mainsqlserver"].ToString());
        //空值会使用webconfig默认链接字符串
        I_Dblink I_DBL = I_DBF.DbLinkSqlMain("");

        Hashtable input = new Hashtable();
        input["@mail"] = email; //跟sql语句中对应的参数。    

        Hashtable return_ht = I_DBL.RunParam_SQL(" SELECT Number FROM AAA_DLZHXXB WHERE B_DLYX=  @mail", "dtResult", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            if (((DataSet)return_ht["return_ds"]).Tables[0].Rows.Count > 0)
                return "是";
            else
                return "否";
        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
            return null;
        }
    }

    /// <summary>
    /// 用于检测或验证除此之外是否还存在同名的登陆邮箱
    /// </summary>
    /// <param name="email">需要验证的邮箱</param>
    /// <param name="number">此条记录的主键编号</param>
    /// <returns>重复返回“是”，否则返回“否”，程序出错返回null</returns>
    /// Create by zzf  Date:2014.06.03   
    public static string IsExistEmail(string email, string number)
    {
        DBFactory I_DBF = new DBFactory();
        //I_Dblink I_DBL = I_DBF.DbLinkSqlMain(ConfigurationManager.ConnectionStrings["mainsqlserver"].ToString());
        //空值会使用webconfig默认链接字符串
        I_Dblink I_DBL = I_DBF.DbLinkSqlMain("");

        Hashtable input = new Hashtable();
        input["@mail"] = email; //跟sql语句中对应的参数。
        input["@num"] = number;

        Hashtable return_ht = I_DBL.RunParam_SQL(" SELECT Number FROM AAA_DLZHXXB WHERE B_DLYX=  @mail AND Number <> @num ", "dtResult", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            if (((DataSet)return_ht["return_ds"]).Tables[0].Rows.Count > 0)
                return "是";
            else
                return "否";
        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
            return null;
        }
    }

    /// <summary>
    /// 用于检测或验证是否存在重复的用户名
    /// </summary>
    /// <param name="userName">需要验证的用户名</param>
    /// <returns>重复返回“是”，否则返回“否”</returns>
    /// Create by zzf  Date:2014.05.22    
    public static string IsExistUname(string userName)
    {
        DBFactory I_DBF = new DBFactory();
        //I_Dblink I_DBL = I_DBF.DbLinkSqlMain(ConfigurationManager.ConnectionStrings["mainsqlserver"].ToString());
        //空值会使用webconfig默认链接字符串
        I_Dblink I_DBL = I_DBF.DbLinkSqlMain("");

        Hashtable input = new Hashtable();
        input["@uname"] = userName; //跟sql语句中对应的参数。


        Hashtable return_ht = I_DBL.RunParam_SQL(" SELECT Number FROM AAA_DLZHXXB WHERE B_YHM=  @uname", "dtResult", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            if (((DataSet)return_ht["return_ds"]).Tables[0].Rows.Count > 0)
                return "是";
            else
                return "否";
        }
        else
        {
            //读取数据库过程中发生程序错误了。。。

            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, userName + "用户注册出错！" + return_ht["return_errmsg"].ToString(), null);
            return null;
        }

    }

    /// <summary>
    /// 用于检测或验证除此之外是否还存在同名的用户名
    /// </summary>
    /// <param name="userName">需要验证的用户名</param>
    /// <param name="userName">本条记录的主键编号</param>
    /// <returns>重复返回“是”，否则返回“否”</returns>
    /// Create by zzf  Date:2014.06.03  
    public static string IsExistUname(string userName, string number)
    {
        DBFactory I_DBF = new DBFactory();
        //I_Dblink I_DBL = I_DBF.DbLinkSqlMain(ConfigurationManager.ConnectionStrings["mainsqlserver"].ToString());
        //空值会使用webconfig默认链接字符串
        I_Dblink I_DBL = I_DBF.DbLinkSqlMain("");

        Hashtable input = new Hashtable();
        input["@uname"] = userName; //跟sql语句中对应的参数。
        input["@num"] = number;

        Hashtable return_ht = I_DBL.RunParam_SQL(" SELECT Number FROM AAA_DLZHXXB WHERE B_YHM=  @uname AND Number <> @num ", "dtResult", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            if (((DataSet)return_ht["return_ds"]).Tables[0].Rows.Count > 0)
                return "是";
            else
                return "否";
        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
            return null;
        }

    }

    /// <summary>
    /// 用户注册第一步，向数据表中写入注册信息
    /// </summary>
    /// <param name="num">主键编号</param>
    /// <param name="uid">用户帐号</param>
    /// <param name="uname">用户名</param>
    /// <param name="pwd">密码</param>
    /// <param name="ip">客户ip地址，由客户端获得</param>
    /// <param name="yzm">验证码</param>
    /// <returns></returns>
    public static DataSet UserRegister(string num, string uid, string uname, string pwd, string ip, string yzm)
    {

        try
        {
            //初始验证码
            string YXYZM = yzm;
            string YZMGQSJ = DateTime.Now.AddDays(1).ToString();//验证码过期时间
            string ZCSJ = DateTime.Now.ToString();
            string DLCS = "0";
            string ZHDQKYYE = "0.00";

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            DataSet ds = new DataSet();
            Hashtable param = new Hashtable();

            param.Add("@num", num);
            param.Add("@DLYX", uid);
            param.Add("@YHM", uname);
            param.Add("@DLMM", pwd);
            param.Add("@YXYZM", YXYZM);
            param.Add("@YZMGQSJ", YZMGQSJ);
            param.Add("@ZCSJ", ZCSJ);
            param.Add("@ZCIP", ip);
            param.Add("@DLCS", DLCS);
            param.Add("@ZHDQKYYE", ZHDQKYYE);

            Hashtable return_ht = new Hashtable();

            return_ht = I_DBL.RunParam_SQL("INSERT INTO AAA_DLZHXXB(Number,B_DLYX,B_YHM,B_DLMM,B_JSZHMM,B_YXYZM,B_YZMGQSJ,B_SFYZYX,B_SFYXDL,B_SFDJ,B_SFXM,B_ZCSJ,B_ZCIP,B_DLCS,B_ZHDQKYYE,S_SFYBJJRSHTG,S_SFYBFGSSHTG,CreateTime,CreateUser,B_DSFCGKYYE) VALUES(@num,@DLYX ,@YHM,@DLMM,@DLMM,@YXYZM ,@YZMGQSJ,'否','否','否','否',@ZCSJ ,@ZCIP,@DLCS,@ZHDQKYYE,'否','否',@ZCSJ,@num ,0)", param);

            if ((bool)(return_ht["return_float"]))
            {
                DataTable dt = new DataTable();//要返回的Datatable
                dt.Columns.Add("NUMBER", typeof(string));//num
                dt.Columns.Add("DLYX", typeof(string));//登录邮箱
                dt.Columns.Add("YHM", typeof(string));//用户名
                dt.Columns.Add("DLMM", typeof(string));//密码
                dt.Columns.Add("YZM", typeof(string));//验证码
                dt.Columns.Add("RESULT", typeof(string));//执行结果
                dt.Columns.Add("REASON", typeof(string));//原因

                dt.Rows.Add(new object[] { num, uid, uname, pwd, YXYZM, "SUCCESS", "SUCCESS" });
                dt.TableName = "执行结果";
                ds.Tables.Clear();
                ds.Tables.Add(dt);

                return ds;
            }
            else
            {
                //FMipcClass.Log.WorkLog("用户注册", FMipcClass.Log.type.yewu, uid + "22用户注册插入时出错！" + num + return_ht["return_errmsg"].ToString(), null);
                FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, uid + "22用户注册插入时出错！" + num + return_ht["return_errmsg"].ToString(), null);

                return null;
            }
        }
        catch (Exception ex)
        {
            //FMipcClass.Log.WorkLog("用户注册", FMipcClass.Log.type.yewu, uid + "用户注册插入时出错！11" + num + ex.ToString(), null);

            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, uid + "用户注册插入时出错！11" + num + ex.ToString(), null);

            return null;
 
        }

    }

    /// <summary>
    /// 用户注册，更换邮箱后更新信息
    /// </summary>
    /// <param name="num">主键编号</param>
    /// <param name="uid">登录邮箱</param>
    /// <param name="uname">用户名</param>
    /// <param name="pwd">密码</param>
    /// <param name="yzm">验证码</param>
    /// <returns></returns>
    public static DataSet UserChangeRegister(string num, string uid, string uname, string pwd, string yzm)
    {
        //初始验证码
        string YXYZM = yzm;
        string YZMGQSJ = DateTime.Now.AddDays(1).ToString();//验证码过期时间
        string ZCSJ = DateTime.Now.ToString();


        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();

        param.Add("@num", num);
        param.Add("@DLYX", uid);
        param.Add("@YHM", uname);
        param.Add("@DLMM", pwd);
        param.Add("@YXYZM", YXYZM);
        param.Add("@YZMGQSJ", YZMGQSJ);


        Hashtable return_ht = new Hashtable();

        return_ht = I_DBL.RunParam_SQL("UPDATE AAA_DLZHXXB SET B_DLYX=@DLYX, B_YHM=@YHM, B_DLMM=@DLMM, B_YXYZM=@YXYZM ,B_YZMGQSJ=@YZMGQSJ WHERE Number=@num ", param);

        if ((bool)(return_ht["return_float"]))
        {
            DataTable dt = new DataTable();//要返回的Datatable
            dt.Columns.Add("NUMBER", typeof(string));//num
            dt.Columns.Add("DLYX", typeof(string));//登录邮箱
            dt.Columns.Add("YHM", typeof(string));//用户名
            dt.Columns.Add("DLMM", typeof(string));//密码
            dt.Columns.Add("YZM", typeof(string));//验证码
            dt.Columns.Add("RESULT", typeof(string));//执行结果
            dt.Columns.Add("REASON", typeof(string));//原因

            dt.Rows.Add(new object[] { num, uid, uname, pwd, YXYZM, "SUCCESS", "SUCCESS" });
            dt.TableName = "执行结果";
            ds.Tables.Clear();
            ds.Tables.Add(dt);

            return ds;
        }
        else
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
    /// <returns>一个名为执行结果的数据集</returns>    
    public static DataSet SendEmailMes(string email, string strSubject, string valNum, string HtmlTemplatePath)
    {
        ClassEmail CE = new ClassEmail();
        string modHtmlFileName = HttpContext.Current.Server.MapPath(HtmlTemplatePath);
        Hashtable ht = new Hashtable();
        ht["[[Email]]"] = email;
        ht["[[valNumbs]]"] = valNum;
        ht["[[DateTime]]"] = DateTime.Now.ToString();

        CE.SendSPEmail(email.Trim(), strSubject, modHtmlFileName, ht);

        DataTable dt = new DataTable();//要返回的Datatable

        dt.Columns.Add("EMAIL", typeof(string));//用户邮箱
        dt.Columns.Add("VALNUM", typeof(string));//验证码等信息
        dt.Columns.Add("TIME", typeof(string));//时间      
        dt.Columns.Add("RESULT", typeof(string));//执行结果      

        dt.Rows.Add(new object[] { email, valNum, DateTime.Now.ToString(), "SUCCESS" });
        dt.TableName = "执行结果";

        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        return ds;
    }


    /// <summary>
    /// 获取用户冻结信息
    /// </summary>
    /// <param name="userinfo">登录邮箱或角色编号</param>
    /// <returns>一个DataSet，包含：登录邮箱，是否冻结，冻结功能项</returns>
    public static DataSet GetFrozenInfo(string userinfo)
    {

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();

        param.Add("@userinfo", userinfo);


        



        //string strsql = " select B_DLYX as '登录邮箱' , isnull(B_SFDJ,'否') as '是否冻结',DJGNX as '冻结功能项' from    dbo.AAA_DLZHXXB where (B_DLYX = @userinfo or J_SELJSBH=@userinfo or J_BUYJSBH=@userinfo or J_JJRJSBH=@userinfo ) ";  //and DJGNX like '%" + DJgn + "%'

        string strsql = " select B_DLYX as '登录邮箱' , isnull(B_SFDJ,'否') as '是否冻结',DJGNX as '冻结功能项' from  dbo.AAA_DLZHXXB  ";


        string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        if (Regex.IsMatch(userinfo, expression, RegexOptions.Compiled)) //是否符合邮箱格式
        {
            strsql += " where B_DLYX = @userinfo ";
        }

        else
        {
            if (userinfo.StartsWith("B"))
            {
                strsql += " where J_SELJSBH = @userinfo ";
            }
            else if (userinfo.StartsWith("C"))
            {
                strsql += " where J_BUYJSBH = @userinfo  ";
            }
            else if (userinfo.StartsWith("J"))
            {
                strsql += " where J_JJRJSBH=@userinfo  ";
            }
            else
            {
                return null;
            }

        }




        Hashtable return_ht = I_DBL.RunParam_SQL(strsql, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            return (DataSet)return_ht["return_ds"];
        }
        else
            return null;

    }

    /// <summary>
    /// 判断具体的某项是否被冻结
    /// </summary>
    /// <param name="userinfo">登录邮箱或角色编号</param>
    /// <param name="item">要验证的项</param>
    /// <returns>是/否/""/null，冻结返回“是”，该用户不存在返回""，数据链接错误返回null</returns>
    public static string IsFrozen(string userinfo, string item)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();

        param.Add("@userinfo", userinfo);


        //string strsql = " select  isnull(B_SFDJ,'否') as '是否冻结',DJGNX as '冻结功能项' from    dbo.AAA_DLZHXXB where (B_DLYX = @userinfo or J_SELJSBH=@userinfo or J_BUYJSBH=@userinfo or J_JJRJSBH=@userinfo ) ";  //and DJGNX like '%" + DJgn + "%'
     
        string strsql = " select  isnull(B_SFDJ,'否') as '是否冻结',DJGNX as '冻结功能项' from    dbo.AAA_DLZHXXB ";
        string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        if (Regex.IsMatch(userinfo, expression, RegexOptions.Compiled)) //是否符合邮箱格式
        {
            strsql += " where B_DLYX = @userinfo ";
        }

        else
        {
            if (userinfo.StartsWith("B"))
            {
                strsql += " where J_SELJSBH = @userinfo ";
            }
            else if (userinfo.StartsWith("C"))
            {
                strsql += " where J_BUYJSBH = @userinfo  ";
            }
            else if (userinfo.StartsWith("J"))
            {
                strsql += " where J_JJRJSBH=@userinfo  ";
            }
            else
            {
                return null;
            }

        }




        Hashtable return_ht = I_DBL.RunParam_SQL(strsql, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count ==1)
            {
                if (ds.Tables[0].Rows[0]["是否冻结"].ToString() == "是" || ds.Tables[0].Rows[0]["冻结功能项"].ToString().Trim()!="")
                {
                    string s = ds.Tables[0].Rows[0]["冻结功能项"].ToString();
                    if (s.Contains(item))

                        return "是";
                    else
                        return "否";

                }
                else
                    return "否";
            }
            else
                return "";
        }
        else
            return null;

    }

    /// <summary>
    /// 获取用户所属管理部门
    /// </summary>
    /// <param name="userinfo">用户邮箱或者角色编号</param>
    /// <returns>管理部门</returns>
    public static string GetDepartment(string userinfo)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();

        param.Add("@userinfo", userinfo);

        //string strsql = " select B_DLYX as '登录邮箱' ,I_PTGLJG as '管理部门' from    dbo.AAA_DLZHXXB where (B_DLYX = @userinfo or J_SELJSBH=@userinfo or J_BUYJSBH=@userinfo or J_JJRJSBH=@userinfo ) ";

        string strsql = " select B_DLYX as '登录邮箱' ,I_PTGLJG as '管理部门' from  dbo.AAA_DLZHXXB  ";

        string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        if (Regex.IsMatch(userinfo, expression, RegexOptions.Compiled)) //是否符合邮箱格式
        {
            strsql += " where B_DLYX = @userinfo ";
        }

        else
        {
            if (userinfo.StartsWith("B"))
            {
                strsql += " where J_SELJSBH = @userinfo ";
            }
            else if (userinfo.StartsWith("C"))
            {
                strsql += " where J_BUYJSBH = @userinfo  ";
            }
            else if (userinfo.StartsWith("J"))
            {
                strsql += " where J_JJRJSBH=@userinfo  ";
            }
            else
            {
                return null;
            }

        }

        Hashtable return_ht = I_DBL.RunParam_SQL(strsql, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                string s = ds.Tables[0].Rows[0]["管理部门"].ToString();
                return s;

            }
            else
                return "";
        }
        else
            return null;
    }

    /// <summary>
    /// 获取用于验证是否开通交易账户的基础数据
    /// </summary>
    /// <param name="userinfo">登录邮箱或用户角色</param>
    /// <returns>DataSet，包含 是否已被经纪人审核通过, 是否已被分公司审核通过</returns>
    public static DataSet GetDeelingAccount(string userinfo)
    {
       
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();

        param.Add("@userinfo", userinfo);
        param.Add("@SFYX", "是");
        param.Add("@SFDQMRJJR", "是");


        //string strsql = " select B_DLYX as 登录邮箱, B_JSZHLX as  结算账户类型, S_SFYBJJRSHTG as 是否已被经纪人审核通过, S_SFYBFGSSHTG as 是否已被分公司审核通过 from dbo.AAA_DLZHXXB where (B_DLYX = @userinfo or J_SELJSBH=@userinfo or J_BUYJSBH=@userinfo or J_JJRJSBH=@userinfo ) ";
        string strsql = "select B_DLYX as 登录邮箱, B_JSZHLX as  结算账户类型, S_SFYBJJRSHTG as 是否已被经纪人审核通过, S_SFYBFGSSHTG as 是否已被分公司审核通过,b.Number ,b.JJRSHZT as 经纪人审核状态,b.FGSSHZT as 分公司审核状态 from dbo.AAA_DLZHXXB as a left join AAA_MJMJJYZHYJJRZHGLB as b on a.B_DLYX=b.DLYX  where  b.SFYX =@SFYX and b.SFDQMRJJR=@SFDQMRJJR  ";


        //2014.10.20 wyh add
       
        string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        if (Regex.IsMatch(userinfo, expression, RegexOptions.Compiled)) //是否符合邮箱格式
        {
            strsql += " and B_DLYX = @userinfo ";
        }

        else
        {
            if (userinfo.StartsWith("B"))
            {
                strsql += " and J_SELJSBH = @userinfo ";
            }
            else if (userinfo.StartsWith("C"))
            {
                strsql += " and J_BUYJSBH = @userinfo  ";
            }
            else if (userinfo.StartsWith("J"))
            {
                strsql += " and J_JJRJSBH=@userinfo  ";
            }
            else
            {
                return null;
            }

        }


        
        Hashtable return_ht = I_DBL.RunParam_SQL(strsql, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
           
            return (DataSet)return_ht["return_ds"];
        }
        else
            return null;

    }

    /// <summary>
    /// 判断账户是否休眠
    /// </summary>
    /// <param name="userinfo">登录邮箱或者角色编号</param>
    /// <returns>是/否 无数据返回""，连接失败返回null</returns>
    public static string IsInSleep(string userinfo)
    {
       
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();

        param.Add("@userinfo", userinfo);

        

       // string strsql = " select B_DLYX as 登录邮箱,  B_SFXM as 是否休眠 from dbo.AAA_DLZHXXB where (B_DLYX = @userinfo or J_SELJSBH=@userinfo or J_BUYJSBH=@userinfo or J_JJRJSBH=@userinfo ) ";

        string strsql = " select B_DLYX as 登录邮箱,  B_SFXM as 是否休眠 from dbo.AAA_DLZHXXB ";
        //string Strwhere = "";
        string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        if (Regex.IsMatch(userinfo, expression, RegexOptions.Compiled)) //是否符合邮箱格式
        {
            strsql += " where B_DLYX = @userinfo ";
        }

        else
        {
            if (userinfo.StartsWith("B"))
            {
                strsql += " where J_SELJSBH = @userinfo ";
            }
            else if (userinfo.StartsWith("C"))
            {
                strsql += " where J_BUYJSBH = @userinfo  ";
            }
            else if (userinfo.StartsWith("J"))
            {
                strsql += " where J_JJRJSBH=@userinfo  ";
            }
            else
            {
                return null;
            }

        }

        Hashtable return_ht = I_DBL.RunParam_SQL(strsql, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                string s = ds.Tables[0].Rows[0]["是否休眠"].ToString();
                
                return s;
                

            }
            else
                return "";
        }
        else
            return null;


    }

    /// <summary>
    /// 获取用户基础信息，设置了三个标识，如果为OFEN，只返回常用字段，如果为ALL，返回全部字段，如果MANY返回多数字段
    /// </summary>
    /// <param name="userinfo">角色编号或登录邮箱</param>
    /// <param name="identifier">标识：OFEN MANY ALL</param>
    /// <returns>一个用户登录信息数据集</returns>
    public static DataSet UserInfo(string userinfo, string identifier)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();

        param.Add("@info", userinfo);

        //常用字段
        string ofen = "SELECT Number,B_DLYX AS 登录邮箱,B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,B_SFYZYX AS 是否验证邮箱, B_SFYXDL AS 是否允许登录, B_SFDJ AS 是否冻结, B_SFXM AS 是否休眠, B_ZCSJ AS 注册时间,B_ZHYCDLSJ AS 最后一次登录时间,isnull(B_ZHDQXYFZ,0) '账户当前信用分值',B_ZHDQKYYE AS 账户当前可用余额,B_YZMGQSJ AS 验证码过期时间,B_YXYZM AS 邮箱验证码,J_SELJSBH AS 卖家角色编号, J_BUYJSBH AS 买家角色编号, J_JJRJSBH AS 经纪人角色编号, J_JJRSFZTXYHSH AS 经纪人是否暂停新用户审核, J_JJRZGZSBH AS 经纪人资格证书编号,S_SFYBJJRSHTG AS 是否已被经纪人审核通过, S_SFYBFGSSHTG AS 是否已被分公司审核通过, DJGNX AS 冻结功能项,I_ZCLB AS 注册类别, I_JYFMC AS 交易方名称,  I_PTGLJG AS 平台管理机构,B_DSFCGKYYE as '第三方存管可用余额', I_DSFCGZT AS '第三方存管状态',(case charindex('-',I_ZQZJZH) when '0' then I_ZQZJZH else ''  end) AS '证券资金账号',I_JJRFL AS 经纪人分类";
        //不常用字段
        string nofen = ", JJRZGZS AS 经纪人资格证书, I_YYZZZCH AS 营业执照注册号, I_YYZZSMJ AS 营业执照扫描件, I_SFZH AS 身份证号, I_SFZSMJ AS 身份证扫描件, I_SFZFMSMJ AS 身份证反面扫描件, I_ZZJGDMZDM AS 组织机构代码证代码, I_ZZJGDMZSMJ AS 组织机构代码证扫描件, I_SWDJZSH AS 税务登记证税号, I_SWDJZSMJ AS 税务登记证扫描件, I_YBNSRZGZSMJ AS 一般纳税人资格证扫描件, I_KHXKZH AS 开户许可证号, I_KHXKZSMJ AS 开户许可证扫描件, I_FDDBRXM AS 法定代表人姓名, I_FDDBRSFZH AS 法定代表人身份证号, I_FDDBRSFZSMJ AS 法定代表人身份证扫描件, I_FDDBRSFZFMSMJ AS 法定代表人身份证反面扫描件, I_FDDBRSQS AS 法定代表人授权书, I_JYFLXDH AS 交易方联系电话, I_SSQYS AS 省, I_SSQYSHI AS 市, I_SSQYQ AS 区, I_XXDZ AS 详细地址, I_LXRXM AS 联系人姓名, I_LXRSJH AS 联系人手机号 , I_KHYH AS 开户银行, I_YHZH AS 银行账号 ";

        //很少用字段
        string less = ",B_DLMM AS 登录密码,B_JSZHMM AS 结算账户密码";

        //条件
       // string fromwhere = " FROM AAA_DLZHXXB where (B_DLYX = @info or J_SELJSBH=@info or J_BUYJSBH=@info or J_JJRJSBH=@info )";
        string fromwhere = " FROM AAA_DLZHXXB ";
        string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        if (Regex.IsMatch(userinfo, expression, RegexOptions.Compiled)) //是否符合邮箱格式
        {
            fromwhere += " where B_DLYX = @info ";
        }

        else
        {
            if (userinfo.StartsWith("B"))
            {
                fromwhere += " where J_SELJSBH = @info ";
            }
            else if (userinfo.StartsWith("C"))
            {
                fromwhere += " where J_BUYJSBH = @info  ";
            }
            else if (userinfo.StartsWith("J"))
            {
                fromwhere += " where J_JJRJSBH=@info  ";
            }
            else
            {
                return null;
            }

        }



        string all = "";
        if (identifier == "OFEN")
        {
            all = ofen + fromwhere;

        }
        else if (identifier == "MANY")
        {
            all = ofen + nofen + fromwhere;
        }
        else if (identifier == "ALL")
        {
            all = ofen + nofen + less + fromwhere;
        }

        Hashtable return_ht = I_DBL.RunParam_SQL(all, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            return (DataSet)return_ht["return_ds"];
        }
        else
            return null;

    }

    /// <summary>
    /// 根据用户登录邮箱获取关联经纪人信息
    /// </summary>
    /// <param name="userEmail">用户登录邮箱</param>
    /// <returns></returns>
    public static DataSet JJRInfo(string userEmail)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();

        param.Add("@userinfo", userEmail);
        param.Add("@SFDQMRJJR", "是");
        param.Add("@SFYX", "是");

        //string sqlstr = "SELECT  G.Number,DLYX AS 登录邮箱,'当前默认经纪人是否被冻结'=isnull((select top 1 SubL.B_SFDJ from AAA_DLZHXXB as SubL where SubL.B_DLYX=G.GLJJRDLZH ),'否')   FROM AAA_DLZHXXB as L left join AAA_MJMJJYZHYJJRZHGLB as G on L.B_DLYX = G.DLYX WHERE SFDQMRJJR='是'  and SFYX='是' and B_DLYX = @userinfo ";

        string sqlstr = "SELECT  G.Number,DLYX AS 登录邮箱,JSZHLX AS 结算账号类型,GLJJRDLZH AS 关联经纪人登陆账号,GLJJRBH AS 关联经纪人编号, GLJJRYHM AS 关联经纪人用户名, SQSJ AS 申请时间, JJRSHZT AS 经纪人审核状态, JJRSHSJ AS 经纪人审核时间, JJRSHYJ AS 经纪人审核意见, FGSSHZT AS 分公司审核状态, FGSSHR AS 分公司审核人, FGSSHSJ AS 分公司审核时间, FGSSHYJ AS 分公司审核意见, SFZTYHXYW AS 是否暂停用户新业务,'当前默认经纪人是否被冻结'=isnull(J.B_SFDJ ,'否'), '当前默认经纪人是否休眠'=isnull(J.B_SFXM,'否'),'经纪人是否暂停新用户审核'=isnull( J.J_JJRSFZTXYHSH ,'否'),'经纪人暂停新用户审核时间'= J.J_JJRZTXYHSHSJ ,'经纪人资格证书编号'=isnull( J.J_JJRZGZSBH ,''),'经纪人资格证书'=isnull( J.JJRZGZS ,''),'经纪人资格证书有效期开始时间'=J.J_JJRZGZSYXQKSSJ ,'经纪人资格证书有效期结束时间'= J.J_JJRZGZSYXQJSSJ ,'关联经纪人平台管理机构'=J.I_PTGLJG ,'资料提交时间'= convert(varchar(10),G.CreateTime,120)    FROM AAA_DLZHXXB as L left join AAA_MJMJJYZHYJJRZHGLB as G on L.B_DLYX = G.DLYX left join  AAA_DLZHXXB as J on J.B_DLYX=G.GLJJRDLZH  WHERE SFDQMRJJR=@SFDQMRJJR  and SFYX=@SFYX and L.B_DLYX = @userinfo ";

        Hashtable return_ht = I_DBL.RunParam_SQL(sqlstr, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            return (DataSet)return_ht["return_ds"];
        }
        else
            return null;

    }

    /// <summary>
    /// 根据证书编号或者经纪人角色编号获取经纪人信息
    /// </summary>
    /// <param name="BH">编号</param>
    /// <param name="kind">类型："ZSBH" "JSBH" ，传入"ZSBH"标识证书编号，传入标识"JSBH"角色编号</param>
    /// <returns></returns>
    public static DataSet GetJJRinfoByKind(string BH, string kind)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();

        param.Add("@BH", BH);


        string sqlstr = "select B_DLYX '登录邮箱', I_JYFMC '经纪人名称',I_LXRSJH '经纪人联系电话',S_SFYBFGSSHTG as '分公司开通审核状态',B_JSZHLX '结算账户类型', J_JJRJSBH '经纪人角色编号', J_JJRSFZTXYHSH '经纪人是否暂停新用户审核', J_JJRZTXYHSHSJ '经纪人暂停新用户审核时间', J_JJRZGZSBH '经纪人资格证书编号',JJRZGZS '经纪人资格证书', B_SFYXDL '是否允许登陆',B_SFDJ '是否冻结',B_SFXM '是否休眠',CONVERT(varchar(10),J_JJRZGZSYXQKSSJ,120) '经纪人资格证书有效期开始时间',convert(varchar(10),J_JJRZGZSYXQJSSJ,120) '经纪人资格证书有效期结束时间' ,I_JJRFL 经纪人分类  ";

        sqlstr += ",B_YHM AS 用户名,B_SFYZYX AS 是否验证邮箱, B_ZCSJ AS 注册时间,B_ZHYCDLSJ AS 最后一次登录时间,isnull(B_ZHDQXYFZ,0) '账户当前信用分值',B_ZHDQKYYE AS 账户当前可用余额,B_YZMGQSJ AS 验证码过期时间,B_YXYZM AS 邮箱验证码,J_SELJSBH AS 卖家角色编号, J_BUYJSBH AS 买家角色编号,  DJGNX AS 冻结功能项,I_ZCLB AS 注册类别,  I_PTGLJG AS 平台管理机构,B_DSFCGKYYE as '第三方存管可用余额', I_DSFCGZT AS '第三方存管状态',(case charindex('-',I_ZQZJZH) when '0' then I_ZQZJZH else ''  end) AS '证券资金账号', I_YYZZZCH AS 营业执照注册号, I_YYZZSMJ AS 营业执照扫描件, I_SFZH AS 身份证号, I_SFZSMJ AS 身份证扫描件, I_SFZFMSMJ AS 身份证反面扫描件, I_ZZJGDMZDM AS 组织机构代码证代码, I_ZZJGDMZSMJ AS 组织机构代码证扫描件, I_SWDJZSH AS 税务登记证税号, I_SWDJZSMJ AS 税务登记证扫描件, I_YBNSRZGZSMJ AS 一般纳税人资格证扫描件, I_KHXKZH AS 开户许可证号, I_KHXKZSMJ AS 开户许可证扫描件, I_FDDBRXM AS 法定代表人姓名, I_FDDBRSFZH AS 法定代表人身份证号, I_FDDBRSFZSMJ AS 法定代表人身份证扫描件, I_FDDBRSFZFMSMJ AS 法定代表人身份证反面扫描件, I_FDDBRSQS AS 法定代表人授权书, I_JYFLXDH AS 交易方联系方式, I_SSQYS AS 省, I_SSQYSHI AS 市, I_SSQYQ AS 区, I_XXDZ AS 详细地址, I_KHYH AS 开户银行, I_YHZH AS 银行账号 ";

        sqlstr += " from AAA_DLZHXXB  where  B_JSZHLX='经纪人交易账户' ";


        string sqlzhbh = " and J_JJRZGZSBH = @BH";
        string sqljsbh = " and J_JJRJSBH = @BH";

        if (kind == "JSBH")
        {
            sqlstr = sqlstr + sqljsbh;
        }
        else if (kind == "ZSBH")
        {
            sqlstr = sqlstr + sqlzhbh;
        }

        Hashtable return_ht = I_DBL.RunParam_SQL(sqlstr, "经纪人用户信息", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            return (DataSet)return_ht["return_ds"];
        }
        else
            return null;


    }
    /// <summary>
    /// 获取余额
    /// </summary>
    /// <param name="userinfo">登录邮箱或角色编号</param>
    /// <returns>当前账户可用余额/第三方存管余额</returns>
    public static DataSet GetYE(string userinfo)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();

        param.Add("@userinfo", userinfo);


        //string strsql = " select B_DLYX as 登录邮箱, B_ZHDQKYYE as 账户当前可用余额, B_DSFCGKYYE as 第三方存管可用余额 from dbo.AAA_DLZHXXB where (B_DLYX = @userinfo or J_SELJSBH=@userinfo or J_BUYJSBH=@userinfo or J_JJRJSBH=@userinfo ) ";
        string strsql = " select B_DLYX as 登录邮箱, B_ZHDQKYYE as 账户当前可用余额, B_DSFCGKYYE as 第三方存管可用余额 from dbo.AAA_DLZHXXB ";


        string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        if (Regex.IsMatch(userinfo, expression, RegexOptions.Compiled)) //是否符合邮箱格式
        {
            strsql += " where B_DLYX = @userinfo ";
        }

        else
        {
            if (userinfo.StartsWith("B"))
            {
                strsql += " where J_SELJSBH = @userinfo ";
            }
            else if (userinfo.StartsWith("C"))
            {
                strsql += " where J_BUYJSBH = @userinfo  ";
            }
            else if (userinfo.StartsWith("J"))
            {
                strsql += " where J_JJRJSBH=@userinfo  ";
            }
            else
            {
                return null;
            }

        }


        Hashtable return_ht = I_DBL.RunParam_SQL(strsql, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            return (DataSet)return_ht["return_ds"];
        }
        else
            return null;

    }

    /// <summary>
    /// 判断当前经纪人是否已经把当前买卖家审核通过
    /// </summary>
    /// <param name="strMMJDLYX">买卖家登录邮箱</param>
    /// <param name="strGLJJRBH">关联经纪人角色编号</param>
    /// <returns>是/否</returns>
    public static string IsJJRPassMMJ(string strMMJDLYX, string strGLJJRBH)
    {

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();

        param.Add("@mmjdlyx", strMMJDLYX);
        param.Add("@jjrjsbh", strGLJJRBH);
        param.Add("@SFYX", "是");
        param.Add("@JJRSHZT", "审核通过");
        param.Add("@SFSCGLJJR", "是");
        param.Add("@SFDQMRJJR", "是");

        //string strSql = "select * from AAA_MJMJJYZHYJJRZHGLB where DLYX=@mmjdlyx and GLJJRBH=@jjrjsbh and SFYX='是' and JJRSHZT='审核通过' and SFSCGLJJR='是' and SFDQMRJJR='是'"; wyh  没有必要查询所有数据 2014.10.14
        string strSql = "select Number from AAA_MJMJJYZHYJJRZHGLB where DLYX=@mmjdlyx and GLJJRBH=@jjrjsbh and SFYX=@SFYX and JJRSHZT=@JJRSHZT and SFSCGLJJR=@SFSCGLJJR and SFDQMRJJR=@SFDQMRJJR ";
        Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                return "是";

            else
                return "否";
        }
        else
            return null;

    }

    /// <summary>
    /// 判断当前买卖家交易账户 是否更换了注册类别
    /// </summary>
    /// <param name="strSelerJSBH">卖家角色编号</param>
    /// <param name="strGLJJRBH">关联经纪人角色编号</param>
    /// <param name="strZCLB">当前注册类别</param>
    /// <returns>否 没有更换注册类别，是 已经更换了注册类别</returns>
    public static string IsGHZCLB(string strSelerJSBH, string strGLJJRBH, string strZCLB)
    {

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();

        param.Add("@mjjsbh", strSelerJSBH);
        param.Add("@jjrjsbh", strGLJJRBH);
        param.Add("@B_JSZHLX", "买家卖家交易账户");
        param.Add("@SFYX", "是");
        param.Add("@SFSCGLJJR", "是");
        param.Add("@SFDQMRJJR", "是");

        string strSql = "select b.I_ZCLB '注册类别' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX=@B_JSZHLX and a.SFYX=@SFYX and a.GLJJRBH=@jjrjsbh and b.J_SELJSBH=@mjjsbh and a.SFSCGLJJR=@SFSCGLJJR and a.SFDQMRJJR=@SFDQMRJJR ";

        Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {

                DataTable dt = ds.Tables[0];
                if (dt.Rows[0]["注册类别"].ToString().Trim() == strZCLB.Trim())
                    return "否";
                else
                    return "是";

            }

            else
                return "";
        }
        else
            return null;


    }

    /// <summary>
    /// 判断当前交易账户是否选择更换了经纪人
    /// </summary>
    /// <param name="strSelerJSBH">卖家角色编号</param>
    /// <param name="strGLJJRBH"></param>
    /// <returns>是/否</returns>
    public static string IsGHJJR(string strSelerJSBH, string strGLJJRBH)
    {

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); 
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();

        param.Add("@mjjsbh", strSelerJSBH);
        param.Add("@jjrjsbh", strGLJJRBH);
        param.Add("@B_JSZHLX", "买家卖家交易账户");
        param.Add("@SFYX", "是");
        param.Add("@SFSCGLJJR", "否");
        param.Add("@FGSSHZT", "审核通过");
        //string strSql = "select * from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and a.GLJJRBH=@jjrjsbh and b.J_SELJSBH=@mjjsbh and a.SFSCGLJJR='否' and a.FGSSHZT='审核通过'"; wyh edit 2014.10.13 发现没必要返回所有字段，返回一个字段即可故修改。
        string strSql = "select a.Number from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX=@B_JSZHLX and a.SFYX=@SFYX and a.GLJJRBH=@jjrjsbh and b.J_SELJSBH=@mjjsbh and a.SFSCGLJJR=@SFSCGLJJR and a.FGSSHZT=@FGSSHZT ";

        Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            stopwatch.Stop(); //  停止监视
            TimeSpan hs = stopwatch.Elapsed;

            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "C 判断当前交易账户是否选择更换了经纪人：" + hs.TotalMilliseconds.ToString("#.#####"), null);
            ds = (DataSet)return_ht["return_ds"];
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return "是";
            }
            else
                return "否";
        }
        else
            return null;

    }

    /// <summary>
    /// 检查经纪人是否暂停审核
    /// </summary>
    /// <param name="jjrinfo">经纪人的登录邮箱或经纪人角色编号</param>
    /// <returns>是/否</returns>
    public static string IsJJRZTSH(string jjrinfo)
    {

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();

        param.Add("@userinfo", jjrinfo);

        string strSql = "select  J_JJRSFZTXYHSH from  AAA_DLZHXXB  where  (B_DLYX = @userinfo or J_JJRJSBH=@userinfo )";

        Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];


                return dt.Rows[0]["J_JJRSFZTXYHSH"].ToString().Trim();
            }
            else
                return "";
        }
        else
            return null;

    }

    /// <summary>
    /// 获取审核状态，包括是否被经纪人审核通过，是否已被分公司审核通过
    /// </summary>
    /// <param name="userinfo">登录邮箱，2014.10不再支持角色编号，支持会多耗时81毫秒</param>
    /// <returns>一个数据集，包括 登录邮箱 结算账户类型，是否被经纪人审核通过，是否已被分公司审核通过</returns>
    public static DataSet GetSHZT(string userinfo)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();

        param.Add("@userinfo", userinfo);


        //string strsql = " select B_DLYX as 登录邮箱, S_SFYBJJRSHTG '是否已被经纪人审核通过',S_SFYBFGSSHTG '是否已被分公司审核通过',B_JSZHLX '结算账户类型' from dbo.AAA_DLZHXXB where (B_DLYX = @userinfo or J_SELJSBH=@userinfo or J_BUYJSBH=@userinfo or J_JJRJSBH=@userinfo ) ";

        string strsql = " select B_DLYX as 登录邮箱, S_SFYBJJRSHTG '是否已被经纪人审核通过',S_SFYBFGSSHTG '是否已被分公司审核通过',B_JSZHLX '结算账户类型' from dbo.AAA_DLZHXXB   ";


        string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        if (Regex.IsMatch(userinfo, expression, RegexOptions.Compiled)) //是否符合邮箱格式
        {
            strsql += " where B_DLYX = @userinfo ";
        }

        else
        {
            if (userinfo.StartsWith("B"))
            {
                strsql += " where J_SELJSBH = @userinfo ";
            }
            else if (userinfo.StartsWith("C"))
            {
                strsql += " where J_BUYJSBH = @userinfo  ";
            }
            else if (userinfo.StartsWith("J"))
            {
                strsql += " where J_JJRJSBH=@userinfo  ";
            }
            else
            {
                return null;
            }

        }




        Hashtable return_ht = I_DBL.RunParam_SQL(strsql, "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            return (DataSet)return_ht["return_ds"];
        }
        else
            return null;

    }

    /// <summary>
    /// 判断当前账户的审核状态和数据库中当前的状态是否一致
    /// </summary>
    /// <param name="strLB">账户类别</param>
    /// <param name="hashTableVerify">当前状态</param>
    /// <returns>不一致为true，一致为false</returns>
    public static bool IsVierifySame(string strLB, string strDLYX, Hashtable hashTableVerify)
    {
        DataSet ds = GetSHZT(strDLYX);
        DataTable dataTable = new DataTable();
        if (ds != null)
        { dataTable = ds.Tables[0]; }

        if (strLB == "经纪人交易账户")
        {
            if (hashTableVerify["是否已经被分公司审核通过"].ToString() == "否" && dataTable.Rows[0]["是否已被分公司审核通过"].ToString() == "是")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else//买家卖家交易账户
        {
            if (hashTableVerify["是否已经被经纪人审核通过"].ToString() == "否" || hashTableVerify["是否已经被分公司审核通过"].ToString() == "否")
            {
                if (dataTable.Rows[0]["是否已被经纪人审核通过"].ToString() == "是" && dataTable.Rows[0]["是否已被分公司审核通过"].ToString() == "是")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }

    /// <summary>
    /// 更新注册验证码
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="valNum">验证码</param>
    /// <returns></returns>
    public static bool ReValNum(string dlyx, string valNum)
    {

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();

        param.Add("@dlyx", dlyx);
        param.Add("@yzm", valNum);
        param.Add("@time", DateTime.Now.AddDays(1).ToString());

        string UpdateSql = "UPDATE AAA_DLZHXXB SET B_YXYZM=@yzm, B_YZMGQSJ=@time WHERE B_DLYX=@dlyx ";//要执行的更新语句

        Hashtable return_ht = new Hashtable();

        return_ht = I_DBL.RunParam_SQL(UpdateSql, param);

        if ((bool)(return_ht["return_float"]))
        {
            return true;
        }
        else
            return false;

    }

    /// <summary>
    /// 忘记密码验证码，用户忘记密码时向忘记密码验证码表写入信息
    /// </summary>
    /// <param name="user">登录邮箱</param>
    /// <param name="typeToSet">类型：email,phone</param>
    /// <param name="number">主键</param>
    /// <param name="RandomNumber">验证码</param>
    /// <param name="ip">ip地址</param>
    /// <returns>执行结果</returns>
    public static DataSet FPwdValNumber(string user, string typeToSet, string number, string RandomNumber, string ip)
    {
        DataSet dsreturn = new DataSet();

        DataTable dtresult = new DataTable();//要返回的Datatable
        dtresult.Columns.Add("EMAIL", typeof(string));//用户邮箱
        dtresult.Columns.Add("Type", typeof(string));//用户邮箱
        dtresult.Columns.Add("VALNUM", typeof(string));//验证码等信息
        dtresult.Columns.Add("TIME", typeof(string));//时间      
        dtresult.Columns.Add("RESULT", typeof(string));//执行结果    

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable param = new Hashtable();

        param.Add("@user", user);
        param.Add("@number", number);
        param.Add("@RandomNumber", RandomNumber);
        param.Add("@ip", ip);
        param.Add("@type", typeToSet);


        string strsql = " SELECT * FROM AAA_DLZHXXB WHERE B_DLYX=@user AND B_SFYZYX='是' ";

        string InsertSqls = "";
        if (typeToSet == "email")
        {
            InsertSqls = "INSERT INTO AAA_WJMMYZMB(Number,subType,userEmail,emailValNumber,phoneValNumber,ipAddress,isUsed,CreateTime) VALUES(@number,@type,@user,@RandomNumber,'',@ip,'false',GETDATE())";//插入验证码表 
        }
        else if (typeToSet == "phone")
        {
            InsertSqls = "INSERT INTO AAA_WJMMYZMB(Number,subType,userEmail,emailValNumber,phoneValNumber,ipAddress,isUsed,CreateTime) VALUES(@number,@type,@user,'', @RandomNumber,@ip,'false',GETDATE())";//插入验证码表 
        }

        Hashtable return_ht = I_DBL.RunParam_SQL(strsql, "查询结果", param);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            ds = (DataSet)return_ht["return_ds"];
            DataTable dt = ds.Tables[0];

            if (dt != null && dt.Rows.Count == 1)
            {
                Hashtable ht = I_DBL.RunParam_SQL(InsertSqls, param);

                if ((bool)(ht["return_float"]))
                {
                    dtresult.Rows.Add(new object[] { user, typeToSet, RandomNumber, DateTime.Now.ToString(), "SUCCESS" });
                }
                else
                {
                    dsreturn = null;

                }

            }
            else
            {
                dtresult.Rows.Add(new object[] { user, typeToSet, RandomNumber, DateTime.Now.ToString(), "用户不存在" });

            }
            dtresult.TableName = "执行结果";
            dsreturn.Tables.Clear();
            dsreturn.Tables.Add(dtresult);

            return dsreturn;

        }
        else
            return null;
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
    public static DataSet ChangePassword(string uid, string phoneoremail, string pwd, string valNum, string typeToSet)
    {
        DataSet dsresult = new DataSet();//要返回的结果数据集

        DataTable dtresult = new DataTable();//要返回的Datatable
        dtresult.Columns.Add("Uid", typeof(string));//用户邮箱
        dtresult.Columns.Add("Email", typeof(string));//找回密码的邮箱
        dtresult.Columns.Add("Phone", typeof(string));//找回密码的手机号
        dtresult.Columns.Add("NewPwd", typeof(string));//新密码      
        dtresult.Columns.Add("Result", typeof(string));//执行结果    

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsmail = new DataSet();
        DataSet dsphone = new DataSet();
        Hashtable param = new Hashtable();

        //参数
        param.Add("@uid", uid);
        param.Add("@pwd", pwd);

        string SelectValEmail = "SELECT TOP 1 * FROM AAA_WJMMYZMB WHERE subType='email' AND userEmail=@uid ORDER BY Number DESC";

        string SelectValPhone = "SELECT TOP 1 * FROM AAA_WJMMYZMB WHERE subType='phone' AND userEmail=@uid ORDER BY Number DESC";

        string UpdateSql = "UPDATE AAA_DLZHXXB SET B_DLMM=@pwd WHERE B_DLYX=@uid ";//要被执行的更新语句
        //使用邮箱修改密码
        if (typeToSet == "email")
        {
            Hashtable return_htmail = I_DBL.RunParam_SQL(SelectValEmail, "dtmail", param);

            if ((bool)(return_htmail["return_float"])) //说明执行完成
            {
                dsmail = (DataSet)return_htmail["return_ds"];
                if (dsmail.Tables[0] != null && dsmail.Tables[0].Rows.Count == 1)
                {
                    if (Convert.ToDateTime(dsmail.Tables[0].Rows[0]["CreateTime"]).AddDays(1) > DateTime.Now)
                    {
                        if (dsmail.Tables[0].Rows[0]["emailValNumber"].ToString() != valNum.Trim())
                        {
                            dtresult.Rows.Add(new object[] { uid, phoneoremail, "", "", "验证码不匹配" });
                            dsresult.Tables.Clear();
                            dsresult.Tables.Add(dtresult);
                        }
                        else
                        {
                            Hashtable return_htpwd = I_DBL.RunParam_SQL(UpdateSql, param);

                            if ((bool)(return_htpwd["return_float"]))
                            {
                                dtresult.Rows.Add(new object[] { uid, phoneoremail, "", pwd, "成功" });
                                dsresult.Tables.Clear();
                                dsresult.Tables.Add(dtresult);
                            }
                            else
                            {
                                dsresult.Tables.Clear();
                                dtresult.Rows.Add(new object[] { uid, phoneoremail, "", "", "服务器繁忙，请稍后再试" });
                                dsresult.Tables.Add(dtresult);
                            }
                        }

                    }
                    else
                    {
                        dtresult.Rows.Add(new object[] { uid, phoneoremail, "", "", "验证失败，您的邮箱验证码已失效，已重新发送验证邮件，请重新输入验证码！" });
                        dsresult.Tables.Clear();
                        dsresult.Tables.Add(dtresult);
                    }
                }
            }
            else
            {
                dsresult = null;
            }

        }

        //手机修改密码
        else if (typeToSet == "phone")
        {
            Hashtable return_htphone = I_DBL.RunParam_SQL(SelectValPhone, "dtphone", param);

            if ((bool)(return_htphone["return_float"])) //说明执行完成
            {
                dsphone = (DataSet)return_htphone["return_ds"];
                if (dsphone.Tables[0] != null && dsphone.Tables[0].Rows.Count == 1)
                {
                    if (dsphone.Tables[0].Rows[0]["phoneValNumber"].ToString() != valNum.Trim())
                    {
                        dtresult.Rows.Add(new object[] { uid, phoneoremail, "", "", "验证码不匹配" });
                        dsresult.Tables.Clear();
                        dsresult.Tables.Add(dtresult);
                    }
                    else
                    {
                        Hashtable return_htpwd = I_DBL.RunParam_SQL(UpdateSql, param);

                        if ((bool)(return_htpwd["return_float"]))
                        {
                            dtresult.Rows.Add(new object[] { uid, phoneoremail, "", pwd, "成功" });
                            dsresult.Tables.Clear();
                            dsresult.Tables.Add(dtresult);
                        }
                        else
                        {
                            dsresult.Tables.Clear();
                            dtresult.Rows.Add(new object[] { uid, phoneoremail, "", "", "服务器繁忙，请稍后再试" });
                            dsresult.Tables.Add(dtresult);
                        }
                    }
                }
            }
            else
            {
                dsresult = null;
            }

        }

        return dsresult;

    }

    /// <summary>
    /// 激活帐户（注册第三步）
    /// </summary>
    /// <param name="id">登录邮箱</param>
    /// <returns>执行结果数据集DLYX，YHM，DLMM，RESULT，REASON</returns>
    public static DataSet AccountID(string id)
    {
        DataSet dsreturn = new DataSet();
        #region 初始化数据表结构
        DataTable dt = new DataTable();//要返回的Datatable
        dt.Columns.Add("DLYX", typeof(string));//登录邮箱
        dt.Columns.Add("YHM", typeof(string));//用户名
        dt.Columns.Add("DLMM", typeof(string));//登录密码
        dt.Columns.Add("RESULT", typeof(string));//执行结果
        dt.Columns.Add("REASON", typeof(string));//原因
        
        #endregion

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();

        Hashtable param = new Hashtable();

        //参数
        param.Add("@uid", id);

        //验证验证码时间是否过期
        //Hashtable return_ht = I_DBL.RunParam_SQL("SELECT * FROM AAA_DLZHXXB WHERE B_DLYX=@uid ", "dtResult", param);
        Hashtable return_ht = I_DBL.RunParam_SQL("SELECT B_YZMGQSJ FROM AAA_DLZHXXB WHERE B_DLYX=@uid ", "dtResult", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            ds = (DataSet)return_ht["return_ds"];
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DateTime ValEndTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["B_YZMGQSJ"].ToString());
                if (ValEndTime < DateTime.Now)
                {
                    dt.Rows.Add(new object[] { id, "", "", "FALSE", "验证失败，您的邮箱验证码已失效，已重新发送验证邮件，请重新输入验证码！" });
                    dsreturn.Tables.Clear();
                    dsreturn.Tables.Add(dt);

                    return dsreturn;
                }
            }
            else
            {
                return null;
            }
            string UpdateSql = "UPDATE AAA_DLZHXXB SET B_SFYZYX='是',B_SFYXDL='是',B_SFDJ='否',B_SFXM='否' WHERE B_DLYX=@uid";
            try
            {
                Hashtable return_htup = I_DBL.RunParam_SQL(UpdateSql, param);
                if ((bool)(return_htup["return_float"])) //说明执行完成               
                {
                    string SelectSql = "SELECT B_DLYX,B_YHM,B_DLMM,'SUCCESS' AS RESULT,'SUCCESS' AS REASON FROM AAA_DLZHXXB WHERE B_DLYX=@uid";

                    DataSet st = new DataSet();

                    Hashtable return_htst = I_DBL.RunParam_SQL(SelectSql, "dtResult", param);
                    if ((bool)(return_htst["return_float"])) //说明执行完成
                    {
                        st = (DataSet)return_htst["return_ds"];
                        if (st.Tables[0] != null && st.Tables[0].Rows.Count > 0)
                        {
                            dt = null;
                            dt = st.Tables[0].Copy();
                            dsreturn.Tables.Clear();
                            dsreturn.Tables.Add(dt);
                        }
                    }
                }
                else
                {
                    dsreturn = null;
                }
            }
            catch
            {
                dsreturn = null;
            }
        }
        else
        {
            dsreturn = null;
        }

        return dsreturn;
    }

    /// <summary>
    /// 账户维护部分的密码修改，非忘记密码时的密码找回
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="xmm">新密码</param>
    /// <returns></returns>
    public static DataSet NPsdChange(string dlyx, string xmm)
    {
        DataSet dsreturn = new DataSet();

        DataTable dt = new DataTable();
        dt.Columns.Add("执行结果", typeof(string));
        dt.Columns.Add("提示文本", typeof(string));
        dt.Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        //参数
        param.Add("@uid", dlyx);
        param.Add("@xmm", xmm);

        string str = "UPDATE AAA_DLZHXXB SET B_DLMM=@xmm WHERE B_DLYX=@uid ";

        Hashtable ht = I_DBL.RunParam_SQL(str, param);
        if ((bool)(ht["return_float"]))
        {
            dt.Rows[0]["执行结果"] = "ok";
            dt.Rows[0]["提示文本"] = "您的密码修改成功！";
            dt.TableName = "返回值单条";
            dsreturn.Tables.Add(dt);
        }
        else
        {
            dsreturn = null;

        }
        return dsreturn;
    }

    /// <summary>
    /// 用于资金交易密码的修改
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="xmm"></param>
    /// <returns></returns>
    public static DataSet ZJPsdChange(string dlyx, string xmm)
    {
        DataSet dsreturn = new DataSet();

        DataTable dt = new DataTable();
        dt.Columns.Add("执行结果", typeof(string));
        dt.Columns.Add("提示文本", typeof(string));
        dt.Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        //参数
        param.Add("@uid", dlyx);
        param.Add("@xmm", xmm);

        string str = "UPDATE AAA_DLZHXXB SET B_JSZHMM=@xmm WHERE B_DLYX=@uid";

        Hashtable ht = I_DBL.RunParam_SQL(str, param);
        if ((bool)(ht["return_float"]))
        {
            dt.Rows[0]["执行结果"] = "ok";
            dt.Rows[0]["提示文本"] = "您的交易资金密码修改成功！";
            dt.TableName = "返回值单条";
            dsreturn.Tables.Add(dt);
        }
        else
        {
            dsreturn = null;

        }
        return dsreturn;
    }

    /// <summary>
    /// 休眠账户的激活
    /// </summary>
    /// <param name="zklsnum">要写入的帐款流水明细表的主键，需要传入</param>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="jszhlx">结算账户类型</param>
    /// <param name="buyjsbh">买家角色编号</param>    
    /// <returns>一个执行结果数据集</returns>
    public static DataSet AccountWakeUp(string zklsnum, string dlyx, string jszhlx)
    {
        ArrayList alist = new ArrayList();

        DataSet dsreturn = new DataSet();

        DataTable dt = new DataTable();
        dt.Columns.Add("执行结果", typeof(string));
        dt.Columns.Add("提示文本", typeof(string));
        dt.Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@uid", dlyx);
        param.Add("@zklsnum", zklsnum);
        param.Add("@jszhlx", jszhlx);
       
        //param.Add("@yhzh", yhzh);

        string strtext = "select B_XMJCJL,J_BUYJSBH from AAA_DLZHXXB where B_DLYX=@uid";
        Hashtable httxt = I_DBL.RunParam_SQL(strtext, "dttxt", param);
        DataTable txt = ((DataSet)httxt["return_ds"]).Tables["dttxt"];
        
        string text = "",bh="";
        if (txt != null && txt.Rows.Count > 0)
        { 
            text = txt.Rows[0]["B_XMJCJL"].ToString().Trim();
            bh = txt.Rows[0]["J_BUYJSBH"].ToString().Trim(); 
        }
        param.Add("@buyjsbh", bh);

        string textsm = text + DateTime.Now.ToString() + "用户激活休眠账户 ";

        param.Add("@textsm", textsm);

        string strdzb = "select * from AAA_moneyDZB where Number='1304000049'";

        Hashtable htdzb = I_DBL.RunProc(strdzb, "dtzdb");
        if ((bool)(htdzb["return_float"]))
        {
            DataTable dzb = ((DataSet)htdzb["return_ds"]).Tables["dtzdb"];


            if (dzb != null && dzb.Rows.Count > 0)
            {
                string xm = dzb.Rows[0]["XM"].ToString().Trim();
                string xz = dzb.Rows[0]["XZ"].ToString().Trim();
                string zy = dzb.Rows[0]["ZY"].ToString().Trim();
                string sjlx = dzb.Rows[0]["SJLX"].ToString().Trim();
                string yslx = dzb.Rows[0]["YSLX"].ToString().Trim();

                param.Add("@xm", xm);
                param.Add("@xz", xz);
                param.Add("@zy", zy);
                param.Add("@sjlx", sjlx);
                param.Add("@yslx", yslx);

                //写入账款流水明细表
                string strinsert = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],[CheckState],[CreateUser],[CreateTime]) VALUES ( @zklsnum,@uid,@jszhlx,@buyjsbh,'AAA_DLZHXXB','',getdate(), @yslx ,100,   @xm , @xz,@zy ,'接口编号',@sjlx,0,@uid, GETDATE())";

                alist.Add(strinsert);

                //更新登录账号信息表                
                string strupdate = "UPDATE AAA_DLZHXXB SET B_SFXM='否',B_ZHDQKYYE=B_ZHDQKYYE-100, B_JCXMSJ=GETDATE(),B_XMJCJL= @textsm  WHERE B_DLYX= @uid";

                alist.Add(strupdate);

                if (jszhlx == "经纪人交易账户")
                {
                    string strsql = "update AAA_MJMJJYZHYJJRZHGLB set SFYX='否' where SFSCGLJJR='否' and SFDQMRJJR='否' and (JJRSHZT='审核中' or JJRSHZT='驳回') and DLYX in (select DLYX from AAA_MJMJJYZHYJJRZHGLB where SFDQMRJJR='是' and JJRSHZT='审核通过' and GLJJRDLZH=@uid )";
                    alist.Add(strsql);
                }

                Hashtable result = I_DBL.RunParam_SQL(alist, param);
                if ((bool)(result["return_float"]))
                {
                    dt.Rows[0]["执行结果"] = "ok";
                    dt.Rows[0]["提示文本"] = "您的账户已经成功激活！";

                    dt.TableName = "返回值单条";
                    dsreturn.Clear();
                    dsreturn.Tables.Add(dt);
                }
                else
                {
                    dsreturn = null;
                }

            }

        }
        else
        {
            dsreturn = null;
        }

        return dsreturn;
    }

    /// <summary>
    /// 使帐户休眠
    /// </summary>
    /// <param name="email">用户登录邮箱</param>
    /// <returns>成功返回"SUCCESS"</returns>
    public static string UserSleep(string email)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@uid", email);
        param.Add("@B_SFXM", "是");
        string sql = "UPDATE AAA_DLZHXXB SET B_SFXM=@B_SFXM WHERE B_DLYX =@uid";

        Hashtable ht = I_DBL.RunParam_SQL(sql, param);
        if ((bool)(ht["return_float"]) && (int)ht["return_other"]==1)
        {
            return "SUCCESS";
        }

        else
        {
            return null;

        }

    }

    /// <summary>
    /// 更新用户最后一次登录时间、IP等信息
    /// </summary>
    /// <param name="email">登录邮箱</param>
    /// <param name="ip">客户ip地址</param>
    /// <returns>成功返回"SUCCESS"</returns>
    protected static string UserUpdateLastSignIn(string email, string ip)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@B_DLYX", email);
        param.Add("@B_ZHYCDLSJ", DateTime.Now.ToString());
        param.Add("@B_ZHYCDLIP", ip);

        string sql = "UPDATE AAA_DLZHXXB SET B_ZHYCDLSJ=@B_ZHYCDLSJ, B_ZHYCDLIP=@B_ZHYCDLIP, B_DLCS = B_DLCS+1 WHERE B_DLYX=@B_DLYX";

        Hashtable ht = I_DBL.RunParam_SQL(sql, param);
        if ((bool)(ht["return_float"]))
        {
            return "SUCCESS";
        }

        else
        {
            return null;

        }

    }

    /// <summary>
    /// 用于开通交易账户时的登录信息检查
    /// </summary>
    /// <param name="user">登录邮箱</param>
    /// <param name="pass">登录密码</param>
    /// <param name="ip">客户端ip</param>
    /// <returns>结果数据集，异常一般只包含返回值单条，正常包含三个数据表：用户信息，关联信息，返回值单条</returns>
    public static DataSet CheckLoginIn(string user, string pass, string ip)
    {
        //要返回的数据集
        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable dt = new DataTable();
        dt.TableName = "返回值单条";
        dt.Columns.Add("执行结果", typeof(string));
        dt.Columns.Add("提示文本", typeof(string));
        dt.Rows.Add(new string[] { "err", "初始化" });
        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        //参数
        param.Add("@B_DLYX", user);
        param.Add("@B_DLMM ", pass);

        Hashtable htsql = GetUserRelatedSql();

        //用户信息查询语句
        string sql = htsql["用户信息"].ToString();

        sql += " WHERE B_DLYX=@B_DLYX AND B_DLMM=@B_DLMM COLLATE  Chinese_PRC_CS_AS  ";//登录帐号&密码

        Hashtable ht = I_DBL.RunParam_SQL(sql, "dtResult", param);
        if ((bool)(ht["return_float"]))
        {
            DataSet ds = (DataSet)ht["return_ds"];
            if (ds != null && ds.Tables["dtResult"] != null && ds.Tables["dtResult"].Rows.Count == 1)
            {

                if (ds.Tables["dtResult"].Rows[0]["是否验证邮箱"].ToString() == "否" || ds.Tables["dtResult"].Rows[0]["是否允许登录"].ToString() == "否")//是否验证邮箱
                {
                    dt.Rows[0]["执行结果"] = "err";
                    if (ds.Tables["dtResult"].Rows[0]["是否验证邮箱"].ToString() == "否")
                    {
                        dt.Rows[0]["提示文本"] = "您的邮箱尚未通过验证！";
                        DataTable dtUInfo = new DataTable();
                        dtUInfo = ds.Tables["dtResult"].Copy();//用户信息表
                        dtUInfo.TableName = "用户信息";
                        dsReturn.Clear();
                        dsReturn.Tables.Add(dtUInfo);
                    }
                    else
                    {
                        dt.Rows[0]["提示文本"] = "您的帐号被我公司禁止登录，请与平台服务人员联系。";
                    }
                }

                else
                {

                    DateTime UserTime;
                    //若最后一次登录时间为NULL，说明用户没登录过，则按注册时间
                    if (ds.Tables["dtResult"].Rows[0]["最后一次登录时间"] != DBNull.Value)
                        UserTime = Convert.ToDateTime(ds.Tables["dtResult"].Rows[0]["最后一次登录时间"].ToString());
                    else
                        UserTime = Convert.ToDateTime(ds.Tables["dtResult"].Rows[0]["注册时间"].ToString());
                    double seconds = (DateTime.Now.AddMonths(-12) - UserTime).TotalSeconds;//当前时间-12个月-最后一次登录时间如果还大于或者等于0，说明用户超过12个月未登录了。休眠帐户
                    if (seconds >= 0)
                    {
                        UserBasic.UserSleep(ds.Tables["dtResult"].Rows[0]["登录邮箱"].ToString());//休眠帐号
                        ds.Tables["dtResult"].Rows[0]["是否休眠"] = "是";
                    }
                    DataTable dtUInfo = new DataTable();
                    dtUInfo = ds.Tables["dtResult"].Copy();//用户信息表
                    dtUInfo.TableName = "用户信息";

                    DataTable dtUGL = new DataTable();

                    //关联经纪人查询语句
                    string ugl = htsql["关联信息"].ToString();

                    ugl += " and DLYX=@B_DLYX  ";//关联信息表

                    Hashtable uglht = I_DBL.RunParam_SQL(ugl, "dtugl", param);
                    if ((bool)(uglht["return_float"]))
                    {

                        DataSet dl = (DataSet)uglht["return_ds"];
                        DataTable dtUGL_Copys = dl.Tables["dtugl"]; //关联信息表
                        dtUGL = dtUGL_Copys.Copy();
                        dtUGL.TableName = "关联信息";
                        dsReturn.Tables.Add(dtUInfo);
                        dsReturn.Tables.Add(dtUGL);

                        UserBasic.UserUpdateLastSignIn(dtUInfo.Rows[0]["登录邮箱"].ToString(), ip);//更新最后一次登录时间、登录次数等信息
                        dt.Rows[0]["执行结果"] = "ok";
                    }
                    else
                    {
                        dt.Rows[0]["执行结果"] = "err";
                        dt.Rows[0]["提示文本"] = "查询失败:查询关联经纪人信息出现错误，请检查网络，或稍后再试";

                    }
                }

            }
            else
            {
                dt.Rows[0]["执行结果"] = "err";
                dt.Rows[0]["提示文本"] = "认证失败：您输入的账号与密码不匹配，请重新输入！";
            }
        }
        else
        {
            dt.Rows[0]["执行结果"] = "err";
            dt.Rows[0]["提示文本"] = "查询失败:查询用户信息出现错误，请检查网络，或稍后再试！";

        }
        dsReturn.Tables.Add(dt);
        return dsReturn;


    }

    /// <summary>
    /// 获取用户信息与关联信息
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <returns></returns>
    public static DataSet UserAndRelatedInfo(string dlyx)
    {
        DataSet dsReturn = new DataSet();


        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        //参数
        param.Add("@B_DLYX", dlyx);

        Hashtable htsql = GetUserRelatedSql();
        string sql = htsql["用户信息"].ToString();
        sql += " WHERE B_DLYX=@B_DLYX  ";//登录帐号

        Hashtable ht = I_DBL.RunParam_SQL(sql, "dtResult", param);
        if ((bool)(ht["return_float"]))
        {
            DataSet ds = (DataSet)ht["return_ds"];

            //开始验证
            if (ds != null && ds.Tables["dtResult"].Rows.Count == 1)
            {
                DataTable dtUInfo = new DataTable();
                dtUInfo = ds.Tables[0].Copy();//用户信息表
                dtUInfo.TableName = "用户信息";
                DataTable dtUGL = new DataTable();

                string ugl = htsql["关联信息"].ToString();

                ugl += " and DLYX=@B_DLYX  ";//关联信息表

                Hashtable htugl = I_DBL.RunParam_SQL(ugl, "dtugl", param);
                if ((bool)(htugl["return_float"]))
                {
                    DataSet du = (DataSet)ht["return_ds"];
                    if (du != null && du.Tables["dtugl"].Rows.Count == 1)
                    {
                        DataTable dtUGL_Copys = new DataTable();
                        dtUGL_Copys = du.Tables["dtugl"];

                        dtUGL = dtUGL_Copys.Copy();
                        dtUGL.TableName = "关联信息";
                        dsReturn.Tables.Add(dtUInfo);
                        dsReturn.Tables.Add(dtUGL);
                    }

                }
                else
                {
                    dsReturn = null;
                }

            }
        }
        else
        {
            dsReturn = null;
        }


        return dsReturn;

    }

    /// <summary>
    /// 返回常用的用户以及关联信息查询语句
    /// </summary>
    /// <returns></returns>
    public static Hashtable GetUserRelatedSql()
    {
        Hashtable ht = new Hashtable();

        //用户信息
        string sql = "SELECT Number,B_DLYX AS 登录邮箱,B_DLMM AS 登录密码,B_JSZHMM AS 结算账户密码,B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,B_SFYZYX AS 是否验证邮箱, B_SFYXDL AS 是否允许登录, B_SFDJ AS 是否冻结,DJGNX AS 冻结功能项, B_SFXM AS 是否休眠, B_ZCSJ AS 注册时间,B_ZHYCDLSJ AS 最后一次登录时间,isnull(B_ZHDQXYFZ,0) '账户当前信用分值', B_ZHDQKYYE AS 账户当前可用余额,B_YZMGQSJ AS 验证码过期时间,B_YXYZM AS 邮箱验证码, ";//用户帐号基础信息部分

        sql += " J_SELJSBH AS 卖家角色编号, J_BUYJSBH AS 买家角色编号, J_JJRJSBH AS 经纪人角色编号, J_JJRSFZTXYHSH AS 经纪人是否暂停新用户审核, J_JJRZGZSBH AS 经纪人资格证书编号, JJRZGZS AS 经纪人资格证书, ";//平台角色部分

        sql += " S_SFYBJJRSHTG AS 是否已被经纪人审核通过, S_SFYBFGSSHTG AS 是否已被分公司审核通过, ";//是否可进行业务flag

        sql += " I_ZCLB AS 注册类别, I_JYFMC AS 交易方名称, I_YYZZZCH AS 营业执照注册号, I_YYZZSMJ AS 营业执照扫描件, I_SFZH AS 身份证号, I_SFZSMJ AS 身份证扫描件,I_SFZFMSMJ 身份证反面扫描件, I_ZZJGDMZDM AS 组织机构代码证代码, I_ZZJGDMZSMJ AS 组织机构代码证扫描件, I_SWDJZSH AS 税务登记证税号, I_SWDJZSMJ AS 税务登记证扫描件, I_YBNSRZGZSMJ AS 一般纳税人资格证扫描件, I_KHXKZH AS 开户许可证号, I_KHXKZSMJ AS 开户许可证扫描件, I_FDDBRXM AS 法定代表人姓名, I_FDDBRSFZH AS 法定代表人身份证号, I_FDDBRSFZSMJ AS 法定代表人身份证扫描件,I_FDDBRSFZFMSMJ 法定代表人身份证反面扫描件, I_FDDBRSQS AS 法定代表人授权书, I_JYFLXDH AS 交易方联系电话, I_SSQYS AS 省, I_SSQYSHI AS 市, I_SSQYQ AS 区, I_XXDZ AS 详细地址, I_LXRXM AS 联系人姓名, I_LXRSJH AS 联系人手机号, I_KHYH AS 开户银行, I_YHZH AS 银行账号, I_PTGLJG AS 平台管理机构,I_DSFCGZT AS '第三方存管状态',(case charindex('-',I_ZQZJZH) when '0' then I_ZQZJZH else ''  end) AS '证券资金账号',I_JJRFL AS 经纪人分类 FROM AAA_DLZHXXB ";//提交资质详细信息

        ht["用户信息"] = sql;

        //关联信息
        string ugl = "SELECT Number,DLYX AS 登录邮箱,JSZHLX AS 结算账号类型,GLJJRDLZH AS 关联经纪人登陆账号,GLJJRBH AS 关联经纪人编号, GLJJRYHM AS 关联经纪人用户名, SQSJ AS 申请时间, JJRSHZT AS 经纪人审核状态, JJRSHSJ AS 经纪人审核时间, JJRSHYJ AS 经纪人审核意见, FGSSHZT AS 分公司审核状态, FGSSHR AS 分公司审核人, FGSSHSJ AS 分公司审核时间, FGSSHYJ AS 分公司审核意见, SFZTYHXYW AS 是否暂停用户新业务 FROM AAA_MJMJJYZHYJJRZHGLB WHERE SFDQMRJJR='是'  and SFYX='是' ";//关联信息表


        ht["关联信息"] = ugl;

        return ht;
    }


    /// <summary>
    /// 根据登录邮箱获取，角色编号
    /// </summary>  
    /// <param name="strPrefix"></param>
    /// <param name="strDLYX"></param>
    /// <returns></returns>
    public static string GetNumberPrefix(string strDLYX, string strPrefix)
    {
        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        //参数
        param.Add("@B_DLYX", strDLYX);

        Hashtable ht = I_DBL.RunParam_SQL("select Number from dbo.AAA_DLZHXXB  where B_DLYX=@B_DLYX ", "dtResult", param);
        if ((bool)(ht["return_float"]))
        {
            DataTable dt = ((DataSet)ht["return_ds"]).Tables["dtResult"];
            if (dt != null && dt.Rows.Count == 1)
            {
                return strPrefix+dt.Rows[0]["Number"].ToString().Trim();
            }
            else
            {
                return "";
            }
        }
        else
        { return null; }


    }

    /// <summary>
    ///  提交，开通【经纪人、单位】的交易账户信息
    /// </summary>
    /// <param name="dataTable">包含一系列要传递的参数</param>
    /// <param name="keyNumber">需要传入的“关联经纪人信息表”的主键</param>
    /// <returns>一个包含执行结果和提示文本的数据集</returns>
    public static DataSet SubmitJJRDW(DataTable dataTable, string keyNumber)
    {
        try
        {
            
            ArrayList arrayList = new ArrayList();

            DataSet dsReturn = new DataSet();
            //要返回的数据集中的数据表
            DataTable d = new DataTable();
            d.TableName = "返回值单条";
            d.Columns.Add("执行结果", typeof(string));
            d.Columns.Add("提示文本", typeof(string));
            d.Rows.Add(new string[] { "err", "初始化" });

            //如果用户没有选择管理机构 根据省、市、区自动选择平台管理机构
            if (dataTable.Rows[0]["经纪人单位平台管理机构"].ToString() == "请选择平台管理机构")
            {
                DataTable dtsd = new DataTable();
                dtsd.Columns.Add("省份", typeof(string));
                dtsd.Columns.Add("地市", typeof(string));

                dtsd.Rows.Add(new object[] { dataTable.Rows[0]["经纪人单位所属省份"].ToString(), dataTable.Rows[0]["经纪人单位所属地市"].ToString() });

                DataSet dssd = new DataSet();
                dssd.Tables.Add(dtsd);

                object[] re = IPC.Call("获取平台管理机构 ", new object[] { dssd });
                //远程方法执行成功（这里的成功，仅仅是程序上的运行成功。只代表程序运行没有出错，不代表业务成功。业务是否成功，需要根据每个方法自己定义的返回值规则来定。按照预定应该是yewuok:xxx或者yewuerr:xxxx）
                if (re[0].ToString() == "ok" && (string)(re[1]) != "没有对应的数据")
                {
                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                    string reFF = (string)(re[1]);
                    dataTable.Rows[0]["经纪人单位平台管理机构"] = reFF;
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。               
                    FMipcClass.Log.WorkLog("开通单位经纪人账户", FMipcClass.Log.type.debug, "获取平台管理机构" + dataTable.Rows[0]["经纪人单位登录邮箱"].ToString() + re[1].ToString(), null);
                    d.Rows[0]["执行结果"] = "err";
                    d.Rows[0]["提示文本"] = "获取平台管理机构数据失败！";
                    dsReturn.Clear();
                    dsReturn.Tables.Add(d);
                    return dsReturn;

                }

            }


            //生成买家角色编号
            string strBuyerJSBH = GetNumberPrefix(dataTable.Rows[0]["经纪人单位登录邮箱"].ToString(), "C");

            //生成经纪人角色编号     
            string strJJRJSBH = strBuyerJSBH.Replace("C", "J");

            //经纪人资格证书编号  此处先空着,暂用这这个W开头代替   此处经纪人资格证是系统生成的      
            string strJJRZGZSBH = strBuyerJSBH.Replace("C", "D");
            //获取关联表的Number值
            string strNumber = keyNumber;


            //链接
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable param = new Hashtable();

            //参数
            param.Add("@B_JSZHLX", dataTable.Rows[0]["经纪人单位交易账户类别"].ToString());
            param.Add("@J_BUYJSBH", strBuyerJSBH);

            param.Add("@J_JJRJSBH", strJJRJSBH);
            param.Add("@strJJRZGZSBH", strJJRZGZSBH);
            param.Add("@I_ZCLB", dataTable.Rows[0]["经纪人单位交易注册类别"].ToString());
            param.Add("@I_JYFMC", dataTable.Rows[0]["经纪人单位交易方名称"].ToString());
            param.Add("@I_YYZZZCH", dataTable.Rows[0]["经纪人单位营业执照注册号"].ToString());
            param.Add("@I_YYZZSMJ", dataTable.Rows[0]["经纪人单位营业执照扫描件"].ToString());
            param.Add("@I_ZZJGDMZDM", dataTable.Rows[0]["经纪人单位组织机构代码证代码证"].ToString());
            param.Add("@I_ZZJGDMZSMJ", dataTable.Rows[0]["经纪人单位组织机构代码证代码证扫描件"].ToString());
            param.Add("@I_SWDJZSH", dataTable.Rows[0]["经纪人单位税务登记证税号"].ToString());
            param.Add("@I_SWDJZSMJ", dataTable.Rows[0]["经纪人单位税务登记证扫描件"].ToString());
            param.Add("@I_YBNSRZGZSMJ", dataTable.Rows[0]["经纪人单位一般纳税人资格证扫描件"].ToString());
            param.Add("@I_KHXKZH", dataTable.Rows[0]["经纪人单位开户许可证号"].ToString());
            param.Add("@I_KHXKZSMJ", dataTable.Rows[0]["经纪人单位开户许扫描件"].ToString());
            param.Add("@I_YLYJK", dataTable.Rows[0]["经纪人单位预留印鉴卡扫描件"].ToString());
            param.Add("@I_FDDBRXM", dataTable.Rows[0]["经纪人单位法定代表人姓名"].ToString());
            param.Add("@I_FDDBRSFZH", dataTable.Rows[0]["经纪人单位法定代表人身份证号"].ToString());
            param.Add("@I_FDDBRSFZSMJ", dataTable.Rows[0]["经纪人单位法定代表人身份证扫描件"].ToString());
            param.Add("@I_FDDBRSFZFMSMJ", dataTable.Rows[0]["经纪人单位法定代表人身份证反面扫描件"].ToString());
            param.Add("@I_FDDBRSQS", dataTable.Rows[0]["经纪人单位法定代表人授权书"].ToString());
            param.Add("@I_JYFLXDH", dataTable.Rows[0]["经纪人单位交易方联系电话"].ToString());
            param.Add("@I_SSQYS", dataTable.Rows[0]["经纪人单位所属省份"].ToString());
            param.Add("@I_SSQYSHI", dataTable.Rows[0]["经纪人单位所属地市"].ToString());
            param.Add("@I_SSQYQ", dataTable.Rows[0]["经纪人单位所属区县"].ToString());
            param.Add("@I_XXDZ", dataTable.Rows[0]["经纪人单位详细地址"].ToString());
            param.Add("@I_LXRXM", dataTable.Rows[0]["经纪人单位联系人姓名"].ToString());
            param.Add("@I_LXRSJH", dataTable.Rows[0]["经纪人单位联系人手机号"].ToString());
            param.Add("@I_KHYH", dataTable.Rows[0]["经纪人单位开户银行"].ToString());
            param.Add("@I_YHZH", dataTable.Rows[0]["经纪人单位银行账号"].ToString());
            param.Add("@I_PTGLJG", dataTable.Rows[0]["经纪人单位平台管理机构"].ToString());
            param.Add("@I_ZLTJSJ", DateTime.Now.ToString());
            param.Add("@B_JSZHMM", dataTable.Rows[0]["经纪人单位证券资金密码"].ToString());
            param.Add("@I_YXBH", dataTable.Rows[0]["经纪人单位院系编号"].ToString());
            param.Add("@I_JJRFL", dataTable.Rows[0]["经纪人单位经纪人分类"].ToString());
            param.Add("@I_YWGLBMFL", dataTable.Rows[0]["经纪人单位业务管理部门分类"].ToString());
            param.Add("@B_DLYX", dataTable.Rows[0]["经纪人单位登录邮箱"].ToString());

            param.Add("@strNumber", strNumber);
            param.Add("@strJJRJSBH", strJJRJSBH);
            param.Add("@jjrdwyhm", dataTable.Rows[0]["经纪人单位用户名"].ToString());

            //登录表
            string strDLZHXXB = "update AAA_DLZHXXB set B_JSZHLX=@B_JSZHLX,J_BUYJSBH=@J_BUYJSBH,J_JJRJSBH=@J_JJRJSBH,J_JJRSFZTXYHSH='否',J_JJRZGZSBH=@strJJRZGZSBH,JJRZGZS='',S_SFYBJJRSHTG='是',S_SFYBFGSSHTG='否',I_ZCLB=@I_ZCLB,I_JYFMC=@I_JYFMC,I_YYZZZCH=@I_YYZZZCH,I_YYZZSMJ=@I_YYZZSMJ,I_ZZJGDMZDM=@I_ZZJGDMZDM,I_ZZJGDMZSMJ=@I_ZZJGDMZSMJ,I_SWDJZSH=@I_SWDJZSH,I_SWDJZSMJ=@I_SWDJZSMJ,I_YBNSRZGZSMJ=@I_YBNSRZGZSMJ,I_KHXKZH=@I_KHXKZH,I_KHXKZSMJ=@I_KHXKZSMJ,I_YLYJK=@I_YLYJK,I_FDDBRXM=@I_FDDBRXM,I_FDDBRSFZH=@I_FDDBRSFZH,I_FDDBRSFZSMJ=@I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ=@I_FDDBRSFZFMSMJ,I_FDDBRSQS=@I_FDDBRSQS,I_JYFLXDH=@I_JYFLXDH,I_SSQYS=@I_SSQYS,I_SSQYSHI=@I_SSQYSHI,I_SSQYQ=@I_SSQYQ,I_XXDZ=@I_XXDZ,I_LXRXM=@I_LXRXM,I_LXRSJH=@I_LXRSJH,I_KHYH=@I_KHYH,I_YHZH=@I_YHZH,I_PTGLJG=@I_PTGLJG,I_ZLTJSJ=@I_ZLTJSJ,B_JSZHMM=@B_JSZHMM,I_YXBH=@I_YXBH,I_JJRFL=@I_JJRFL,I_YWGLBMFL=@I_YWGLBMFL where B_DLYX=@B_DLYX ";

            arrayList.Add(strDLZHXXB);
            //插入关联表   
            string strGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,JJRSHSJ,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values (@strNumber,@B_DLYX,@B_JSZHLX,@B_DLYX,@strJJRJSBH,@jjrdwyhm,'是','" + DateTime.Now.ToString() + "','审核通过','" + DateTime.Now.ToString() + "','审核中','否','是','是')";

            arrayList.Add(strGLB);
            
            Hashtable ht = I_DBL.RunParam_SQL(arrayList, param);
            if ((bool)(ht["return_float"]))
            {
                d.Rows[0]["执行结果"] = "ok";
                d.Rows[0]["提示文本"] = "您的开户申请已提交，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";

            }
            else
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "交易账户信息提交失败！";
                FMipcClass.Log.WorkLog("开通个人经纪人账户", FMipcClass.Log.type.debug, strNumber+ht["return_errmsg"].ToString(), null);
            }

            
            dsReturn.Clear();
            dsReturn.Tables.Add(d);
            return dsReturn;
        }
        catch (Exception ex)
        {
            FMipcClass.Log.WorkLog("开通单位经纪人账户", FMipcClass.Log.type.debug, ex.ToString(), null);
            return null;
        }

    }

    /// <summary>
    /// 提交，开通【经纪人、个人】的交易账户信息
    /// </summary>
    /// <param name="dataTable">包含一系列要传递的参数</param>
    /// <param name="keyNumber">写入的经纪人关联表的主键</param>
    /// <returns>一个包含执行结果和提示文本的数据集</returns>
    public static DataSet SubmitJJRGR(DataTable dataTable, string keyNumber)
    {
        try
        {
            string test = dataTable.Rows[0]["经纪人个人登录邮箱"].ToString();
            //FMipcClass.Log.WorkLog("开通个人经纪人账户", FMipcClass.Log.type.debug, test + "之前", null);

            ArrayList arrayList = new ArrayList();

            DataSet dsReturn = new DataSet();
            //要返回的数据集中的数据表
            DataTable d = new DataTable();
            d.TableName = "返回值单条";
            d.Columns.Add("执行结果", typeof(string));
            d.Columns.Add("提示文本", typeof(string));
            d.Rows.Add(new string[] { "err", "初始化" });

            //如果用户没有选择管理机构 根据省、市、区自动选择平台管理机构
            if (dataTable.Rows[0]["经纪人个人平台管理机构"].ToString() == "请选择平台管理机构")
            {
                DataTable dtsd = new DataTable();
                dtsd.Columns.Add("省份", typeof(string));
                dtsd.Columns.Add("地市", typeof(string));

                dtsd.Rows.Add(new object[] { dataTable.Rows[0]["经纪人个人所属省份"].ToString(), dataTable.Rows[0]["经纪人个人所属地市"].ToString() });

                DataSet dssd = new DataSet();
                dssd.Tables.Add(dtsd);

                object[] re = IPC.Call("获取平台管理机构 ", new object[] { dssd });
                //远程方法执行成功（这里的成功，仅仅是程序上的运行成功。只代表程序运行没有出错，不代表业务成功。业务是否成功，需要根据每个方法自己定义的返回值规则来定。按照预定应该是yewuok:xxx或者yewuerr:xxxx）
                if (re[0].ToString() == "ok" && (string)(re[1]) != "没有对应的数据")
                {
                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                    string reFF = (string)(re[1]);
                    dataTable.Rows[0]["经纪人单位平台管理机构"] = reFF;
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。               
                    FMipcClass.Log.WorkLog("开通个人经纪人账户", FMipcClass.Log.type.debug, "获取平台管理机构" + re[1].ToString(), null);
                    d.Rows[0]["执行结果"] = "err";
                    d.Rows[0]["提示文本"] = "获取平台管理机构数据失败！";
                    dsReturn.Clear();
                    dsReturn.Tables.Add(d);
                    return dsReturn;

                }

            }

            //生成买家角色编号
            string strBuyerJSBH = GetNumberPrefix(dataTable.Rows[0]["经纪人个人登录邮箱"].ToString(), "C");

            //生成经纪人角色编号       
            string strJJRJSBH = strBuyerJSBH.Replace("C", "J");
            //经纪人资格证书编号          
            string strJJRZGZSBH = strBuyerJSBH.Replace("C", "D");
            //获取关联表的Number值
            string strNumber = keyNumber;

            //链接
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable param = new Hashtable();
            //参数
            param.Add("@B_JSZHLX", dataTable.Rows[0]["经纪人个人交易账户类别"].ToString());
            param.Add("@J_BUYJSBH", strBuyerJSBH);
            param.Add("@J_JJRJSBH", strJJRJSBH);
            param.Add("@strJJRZGZSBH", strJJRZGZSBH);
            param.Add("@I_ZCLB", dataTable.Rows[0]["经纪人个人交易注册类别"].ToString());
            param.Add("@I_JYFMC", dataTable.Rows[0]["经纪人个人交易方名称"].ToString());
            param.Add("@I_SFZH", dataTable.Rows[0]["经纪人个人身份证号"].ToString());
            param.Add("@I_SFZSMJ", dataTable.Rows[0]["经纪人个人身份证扫描件"].ToString());
            param.Add("@I_SFZFMSMJ", dataTable.Rows[0]["经纪人个人身份证反面扫描件"].ToString());
            param.Add("@I_JYFLXDH", dataTable.Rows[0]["经纪人个人交易方联系电话"].ToString());
            param.Add("@I_SSQYS", dataTable.Rows[0]["经纪人个人所属省份"].ToString());
            param.Add("@I_SSQYSHI", dataTable.Rows[0]["经纪人个人所属地市"].ToString());
            param.Add("@I_SSQYQ", dataTable.Rows[0]["经纪人个人所属区县"].ToString());
            param.Add("@I_XXDZ", dataTable.Rows[0]["经纪人个人详细地址"].ToString());
            param.Add("@I_LXRXM", dataTable.Rows[0]["经纪人个人联系人姓名"].ToString());
            param.Add("@I_LXRSJH", dataTable.Rows[0]["经纪人个人联系人手机号"].ToString());
            param.Add("@I_KHYH", dataTable.Rows[0]["经纪人个人开户银行"].ToString());
            param.Add("@I_YHZH", dataTable.Rows[0]["经纪人个人银行账号"].ToString());
            param.Add("@I_PTGLJG", dataTable.Rows[0]["经纪人个人平台管理机构"].ToString());
            param.Add("@I_ZLTJSJ", DateTime.Now.ToString());
            param.Add("@B_JSZHMM", dataTable.Rows[0]["经纪人个人证券资金密码"].ToString());
            param.Add("@I_YXBH", dataTable.Rows[0]["经纪人个人院系编号"].ToString());
            param.Add("@I_JJRFL", dataTable.Rows[0]["经纪人个人经纪人分类"].ToString());
            param.Add("@I_YWGLBMFL", dataTable.Rows[0]["经纪人个人业务管理部门分类"].ToString());
            param.Add("@B_DLYX", dataTable.Rows[0]["经纪人个人登录邮箱"].ToString());

            param.Add("@strNumber", strNumber);
            param.Add("@jjryhm", dataTable.Rows[0]["经纪人个人用户名"].ToString());

            //登录表       
            string strDLZHXXB = "update AAA_DLZHXXB set B_JSZHLX=@B_JSZHLX,J_BUYJSBH=@J_BUYJSBH,J_JJRJSBH=@J_JJRJSBH,J_JJRSFZTXYHSH='否',J_JJRZGZSBH=@strJJRZGZSBH,JJRZGZS='',S_SFYBJJRSHTG='是',S_SFYBFGSSHTG='否',I_ZCLB=@I_ZCLB,I_JYFMC=@I_JYFMC,I_SFZH=@I_SFZH,I_SFZSMJ=@I_SFZSMJ,I_SFZFMSMJ=@I_SFZFMSMJ,I_JYFLXDH=@I_JYFLXDH,I_SSQYS=@I_SSQYS,I_SSQYSHI=@I_SSQYSHI,I_SSQYQ=@I_SSQYQ,I_XXDZ=@I_XXDZ,I_LXRXM=@I_LXRXM,I_LXRSJH=@I_LXRSJH,I_KHYH=@I_KHYH,I_YHZH=@I_YHZH,I_PTGLJG=@I_PTGLJG ,I_ZLTJSJ=@I_ZLTJSJ,B_JSZHMM=@B_JSZHMM,I_YXBH=@I_YXBH,I_JJRFL=@I_JJRFL,I_YWGLBMFL=@I_YWGLBMFL  where B_DLYX=@B_DLYX ";

            arrayList.Add(strDLZHXXB);
            //插入关联表   
            string strGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,JJRSHSJ,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values (@strNumber,@B_DLYX,@B_JSZHLX,@B_DLYX,@J_JJRJSBH,@jjryhm,'是','" + DateTime.Now.ToString() + "','审核通过','" + DateTime.Now.ToString() + "','审核中','否','是','是')";
            arrayList.Add(strGLB);
            //FMipcClass.Log.WorkLog("开通个人经纪人账户", FMipcClass.Log.type.debug, test + "执行SQL之前", null);
            Hashtable ht = I_DBL.RunParam_SQL(arrayList, param);
            if ((bool)(ht["return_float"]))
            {
                d.Rows[0]["执行结果"] = "ok";
                d.Rows[0]["提示文本"] = "您的开户申请已提交，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";

            }
            else
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "交易账户信息提交失败！";
                FMipcClass.Log.WorkLog("开通个人经纪人账户", FMipcClass.Log.type.debug, strNumber+ht["return_errmsg"].ToString(), null);
            }

           // FMipcClass.Log.WorkLog("开通个人经纪人账户", FMipcClass.Log.type.debug, test + "执行SQL后", null);
            dsReturn.Clear();
            dsReturn.Tables.Add(d);
            return dsReturn;
        }
        catch (Exception ex)
        {
            //FMipcClass.Log.WorkLog("开通个人经纪人账户", FMipcClass.Log.type.debug, ex.ToString(), null);
            //暂时用于测试
            FMipcClass.Log.WorkLog(" C下达预订单", FMipcClass.Log.type.debug, ex.ToString(), null);
            return null;
        }


    }

    /// <summary>
    /// 提交，开通【买卖家、单位】的交易账户信息
    /// </summary>
    /// <param name="dataTable">包含参数的数据表</param>
    /// <param name="keyNumber">用于写入关联经纪人信息表的主键</param>
    /// <returns>执行结果</returns>
    public static DataSet SubmitMMJDW(DataTable dataTable, string keyNumber)
    {

        try
        {
            ArrayList arrayList = new ArrayList();

            DataSet dsReturn = new DataSet();
            //要返回的数据集中的数据表
            DataTable d = new DataTable();
            d.TableName = "返回值单条";
            d.Columns.Add("执行结果", typeof(string));
            d.Columns.Add("提示文本", typeof(string));
            d.Rows.Add(new string[] { "err", "初始化" });

            //生成买家角色编号
            string strBuyerJSBH = GetNumberPrefix(dataTable.Rows[0]["买卖家单位登录邮箱"].ToString(), "C");

            //生成卖家角色编号       
            string strSelerJSBH = strBuyerJSBH.Replace("C", "B");
            //获取关联表的Number值
            string strNumber = keyNumber;

            //链接
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable param = new Hashtable();

            DataTable dataGLJJR = new DataTable();
            //参数
            param.Add("@B_JSZHLX", dataTable.Rows[0]["买卖家单位交易账户类别"].ToString());
            param.Add("@J_BUYJSBH", strBuyerJSBH);
            param.Add("@J_SELJSBH", strSelerJSBH);

            param.Add("@strJJRZGZSBH", dataTable.Rows[0]["买卖家单位经纪人资格证书编号"].ToString());

            param.Add("@I_ZCLB", dataTable.Rows[0]["买卖家单位交易注册类别"].ToString());
            param.Add("@I_JYFMC", dataTable.Rows[0]["买卖家单位交易方名称"].ToString());
            param.Add("@I_YYZZZCH", dataTable.Rows[0]["买卖家单位营业执照注册号"].ToString());
            param.Add("@I_YYZZSMJ", dataTable.Rows[0]["买卖家单位营业执照扫描件"].ToString());
            param.Add("@I_ZZJGDMZDM", dataTable.Rows[0]["买卖家单位组织机构代码证代码证"].ToString());
            param.Add("@I_ZZJGDMZSMJ", dataTable.Rows[0]["买卖家单位组织机构代码证代码证扫描件"].ToString());
            param.Add("@I_SWDJZSH", dataTable.Rows[0]["买卖家单位税务登记证税号"].ToString());
            param.Add("@I_SWDJZSMJ", dataTable.Rows[0]["买卖家单位税务登记证扫描件"].ToString());
            param.Add("@I_YBNSRZGZSMJ", dataTable.Rows[0]["买卖家单位一般纳税人资格证扫描件"].ToString());
            param.Add("@I_KHXKZH", dataTable.Rows[0]["买卖家单位开户许可证号"].ToString());
            param.Add("@I_KHXKZSMJ", dataTable.Rows[0]["买卖家单位开户许扫描件"].ToString());
            param.Add("@I_YLYJK", dataTable.Rows[0]["买卖家单位预留印鉴卡扫描件"].ToString());
            param.Add("@I_FDDBRXM", dataTable.Rows[0]["买卖家单位法定代表人姓名"].ToString());
            param.Add("@I_FDDBRSFZH", dataTable.Rows[0]["买卖家单位法定代表人身份证号"].ToString());
            param.Add("@I_FDDBRSFZSMJ", dataTable.Rows[0]["买卖家单位法定代表人身份证扫描件"].ToString());
            param.Add("@I_FDDBRSFZFMSMJ", dataTable.Rows[0]["买卖家单位法定代表人身份证反面扫描件"].ToString());
            param.Add("@I_FDDBRSQS", dataTable.Rows[0]["买卖家单位法定代表人授权书"].ToString());
            param.Add("@I_JYFLXDH", dataTable.Rows[0]["买卖家单位交易方联系电话"].ToString());
            param.Add("@I_SSQYS", dataTable.Rows[0]["买卖家单位所属省份"].ToString());
            param.Add("@I_SSQYSHI", dataTable.Rows[0]["买卖家单位所属地市"].ToString());
            param.Add("@I_SSQYQ", dataTable.Rows[0]["买卖家单位所属区县"].ToString());
            param.Add("@I_XXDZ", dataTable.Rows[0]["买卖家单位详细地址"].ToString());
            param.Add("@I_LXRXM", dataTable.Rows[0]["买卖家单位联系人姓名"].ToString());
            param.Add("@I_LXRSJH", dataTable.Rows[0]["买卖家单位联系人手机号"].ToString());
            param.Add("@I_KHYH", dataTable.Rows[0]["买卖家单位开户银行"].ToString());
            param.Add("@I_YHZH", dataTable.Rows[0]["买卖家单位银行账号"].ToString());


            param.Add("@I_ZLTJSJ", DateTime.Now.ToString());
            param.Add("@B_JSZHMM", dataTable.Rows[0]["买卖家单位证券资金密码"].ToString());
            param.Add("@I_GLYHGZRYGH", dataTable.Rows[0]["买卖家单位银行工作人员工号"].ToString());

            param.Add("@I_GLYH", dataTable.Rows[0]["买卖家单位关联银行"].ToString());
            param.Add("@I_YWFWBM", dataTable.Rows[0]["买卖家单位业务服务部门"].ToString());

            param.Add("@B_DLYX", dataTable.Rows[0]["买卖家单位登录邮箱"].ToString());
            param.Add("@strNumber", strNumber);

            string strgl = "select B_DLYX '关联经纪人登录邮箱',B_YHM '关联经纪人用户名',J_JJRJSBH '关联经纪人角色编号',I_YWGLBMFL '业务管理部门分类',I_PTGLJG '平台管理机构',* from AAA_DLZHXXB  where J_JJRZGZSBH=@strJJRZGZSBH ";

            Hashtable htjjr = I_DBL.RunParam_SQL(strgl, "dtjjr", param);
            if ((bool)(htjjr["return_float"]))
            {

                dataGLJJR = ((DataSet)htjjr["return_ds"]).Tables["dtjjr"]; ;
                if (dataGLJJR == null || dataGLJJR.Rows.Count <= 0)
                {
                    d.Rows[0]["执行结果"] = "err";
                    d.Rows[0]["提示文本"] = "根据关联的经纪人资格证书编号，无法获取相应的经纪人信息！";
                    dsReturn.Clear();
                    dsReturn.Tables.Add(d);
                    return dsReturn;
                }
            }
            else
            {
                return null;

            }

            param.Add("@I_YWGLBMFL", dataGLJJR.Rows[0]["业务管理部门分类"].ToString());
            param.Add("@I_PTGLJG", dataGLJJR.Rows[0]["平台管理机构"].ToString());
            param.Add("@jjryx", dataGLJJR.Rows[0]["关联经纪人登录邮箱"].ToString());
            param.Add("@jjrjsbh", dataGLJJR.Rows[0]["关联经纪人角色编号"].ToString());
            param.Add("@jjryhm", dataGLJJR.Rows[0]["关联经纪人用户名"].ToString());


            //登录表I_PTGLJG

            string strDLZHXXB = "update AAA_DLZHXXB set B_JSZHLX=@B_JSZHLX,J_SELJSBH=@J_SELJSBH,J_BUYJSBH=@J_BUYJSBH,S_SFYBJJRSHTG='否',S_SFYBFGSSHTG='否',I_ZCLB=@I_ZCLB,I_JYFMC=@I_JYFMC,I_YYZZZCH=@I_YYZZZCH,I_YYZZSMJ=@I_YYZZSMJ,I_ZZJGDMZDM=@I_ZZJGDMZDM,I_ZZJGDMZSMJ=@I_ZZJGDMZSMJ,I_SWDJZSH=@I_SWDJZSH,I_SWDJZSMJ=@I_SWDJZSMJ,I_YBNSRZGZSMJ=@I_YBNSRZGZSMJ,I_KHXKZH=@I_KHXKZH,I_KHXKZSMJ=@I_KHXKZSMJ,I_YLYJK=@I_YLYJK,I_FDDBRXM=@I_FDDBRXM,I_FDDBRSFZH=@I_FDDBRSFZH,I_FDDBRSFZSMJ=@I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ=@I_FDDBRSFZFMSMJ,I_FDDBRSQS=@I_FDDBRSQS,I_JYFLXDH=@I_JYFLXDH,I_SSQYS=@I_SSQYS,I_SSQYSHI=@I_SSQYSHI,I_SSQYQ=@I_SSQYQ,I_XXDZ=@I_XXDZ,I_LXRXM=@I_LXRXM,I_LXRSJH=@I_LXRSJH,I_KHYH=@I_KHYH,I_YHZH=@I_YHZH ,I_ZLTJSJ=@I_ZLTJSJ,B_JSZHMM=@B_JSZHMM,I_YWGLBMFL=@I_YWGLBMFL,I_PTGLJG=@I_PTGLJG,I_GLYHGZRYGH=@I_GLYHGZRYGH,I_GLYH=@I_GLYH,I_YWFWBM=@I_YWFWBM  where B_DLYX=@B_DLYX";

            arrayList.Add(strDLZHXXB);
            //插入关联表   
            string strGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values (@strNumber ,@B_DLYX,@B_JSZHLX,@jjryx,@jjrjsbh,@jjryhm,'是','" + DateTime.Now.ToString() + "','审核中','审核中','否','是','是')";

            arrayList.Add(strGLB);

            Hashtable ht = I_DBL.RunParam_SQL(arrayList, param);
            if ((bool)(ht["return_float"]))
            {
                d.Rows[0]["执行结果"] = "ok";
                d.Rows[0]["提示文本"] = "您的开户申请已提交，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";

            }
            else
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "交易账户信息提交失败！";
                FMipcClass.Log.WorkLog("开通个人经纪人账户", FMipcClass.Log.type.debug, strNumber+ht["return_errmsg"].ToString(), new DataSet());
            }

            dsReturn.Clear();
            dsReturn.Tables.Add(d);
            return dsReturn;
        }
        catch (Exception ex)
        {
            FMipcClass.Log.WorkLog("开通单位买卖家账户", FMipcClass.Log.type.debug, ex.ToString(), null);
            return null;
        }
    }


    /// <summary>
    /// 提交，开通【买卖家、个人】的交易账户信息
    /// </summary>   
    /// <param name="dataTable">包含参数的数据表</param>
    /// <param name="keyNumber">要写入关联表的主键</param>
    /// <returns></returns>
    public static DataSet SubmitMMJGR(DataTable dataTable, string keyNumber)
    {
        try
        {
            ArrayList arrayList = new ArrayList();

            DataSet dsReturn = new DataSet();
            //要返回的数据集中的数据表
            DataTable d = new DataTable();
            d.TableName = "返回值单条";
            d.Columns.Add("执行结果", typeof(string));
            d.Columns.Add("提示文本", typeof(string));
            d.Rows.Add(new string[] { "err", "初始化" });

            //生成买家角色编号
            string strBuyerJSBH = GetNumberPrefix(dataTable.Rows[0]["买卖家个人登录邮箱"].ToString(), "C");

            //生成卖家角色编号       
            string strSelerJSBH = strBuyerJSBH.Replace("C", "B");


            //获取关联表的Number值
            string strNumber = keyNumber;

            //链接
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable param = new Hashtable();

            DataTable dataGLJJR = new DataTable();
            //参数
            param.Add("@B_JSZHLX", dataTable.Rows[0]["买卖家个人交易账户类别"].ToString());
            param.Add("@J_BUYJSBH", strBuyerJSBH);
            param.Add("@J_SELJSBH", strSelerJSBH);

            param.Add("@strJJRZGZSBH", dataTable.Rows[0]["买卖家个人经纪人资格证书编号"].ToString());

            param.Add("@I_ZCLB", dataTable.Rows[0]["买卖家个人交易注册类别"].ToString());
            param.Add("@I_JYFMC", dataTable.Rows[0]["买卖家个人交易方名称"].ToString());

            param.Add("@I_SFZH", dataTable.Rows[0]["买卖家个人身份证号"].ToString());
            param.Add("@I_SFZSMJ", dataTable.Rows[0]["买卖家个人身份证扫描件"].ToString());
            param.Add("@I_SFZFMSMJ", dataTable.Rows[0]["买卖家个人身份证反面扫描件"].ToString());
            param.Add("@I_JYFLXDH", dataTable.Rows[0]["买卖家个人交易方联系电话"].ToString());
            param.Add("@I_SSQYS", dataTable.Rows[0]["买卖家个人所属省份"].ToString());
            param.Add("@I_SSQYSHI", dataTable.Rows[0]["买卖家个人所属地市"].ToString());
            param.Add("@I_SSQYQ", dataTable.Rows[0]["买卖家个人所属区县"].ToString());
            param.Add("@I_XXDZ", dataTable.Rows[0]["买卖家个人详细地址"].ToString());
            param.Add("@I_LXRXM", dataTable.Rows[0]["买卖家个人联系人姓名"].ToString());
            param.Add("@I_LXRSJH", dataTable.Rows[0]["买卖家个人联系人手机号"].ToString());
            param.Add("@I_KHYH", dataTable.Rows[0]["买卖家个人开户银行"].ToString());
            param.Add("@I_YHZH", dataTable.Rows[0]["买卖家个人银行账号"].ToString());

            param.Add("@I_ZLTJSJ", DateTime.Now.ToString());
            param.Add("@B_JSZHMM", dataTable.Rows[0]["买卖家个人证券资金密码"].ToString());
            param.Add("@B_DLYX", dataTable.Rows[0]["买卖家个人登录邮箱"].ToString());


            param.Add("@I_GLYHGZRYGH", dataTable.Rows[0]["买卖家个人关联银行工作人员工号"].ToString());
            param.Add("@I_GLYH", dataTable.Rows[0]["买卖家个人关联银行"].ToString());
            param.Add("@I_YWFWBM", dataTable.Rows[0]["买卖家个人业务服务部门"].ToString());

            param.Add("@strNumber", strNumber);

            string strgl = "select B_DLYX '关联经纪人登录邮箱',B_YHM '关联经纪人用户名',J_JJRJSBH '关联经纪人角色编号',I_YWGLBMFL '业务管理部门分类',I_PTGLJG '平台管理机构',* from AAA_DLZHXXB  where J_JJRZGZSBH=@strJJRZGZSBH ";

            Hashtable htjjr = I_DBL.RunParam_SQL(strgl, "dtjjr", param);
            if ((bool)(htjjr["return_float"]))
            {

                dataGLJJR = ((DataSet)htjjr["return_ds"]).Tables["dtjjr"]; ;
                if (dataGLJJR == null || dataGLJJR.Rows.Count <= 0)
                {
                    d.Rows[0]["执行结果"] = "err";
                    d.Rows[0]["提示文本"] = "根据关联的经纪人资格证书编号，无法获取相应的经纪人信息！";
                    dsReturn.Clear();
                    dsReturn.Tables.Add(d);
                    return dsReturn;
                }
            }
            else
            {
                return null;

            }

            param.Add("@I_YWGLBMFL", dataGLJJR.Rows[0]["业务管理部门分类"].ToString());
            param.Add("@I_PTGLJG", dataGLJJR.Rows[0]["平台管理机构"].ToString());
            param.Add("@jjryx", dataGLJJR.Rows[0]["关联经纪人登录邮箱"].ToString());
            param.Add("@jjrjsbh", dataGLJJR.Rows[0]["关联经纪人角色编号"].ToString());
            param.Add("@jjryhm", dataGLJJR.Rows[0]["关联经纪人用户名"].ToString());

            //登录表
            string strDLZHXXB = "update AAA_DLZHXXB set B_JSZHLX=@B_JSZHLX,J_SELJSBH=@J_SELJSBH,J_BUYJSBH=@J_BUYJSBH,S_SFYBJJRSHTG='否',S_SFYBFGSSHTG='否',I_ZCLB=@I_ZCLB,I_JYFMC=@I_JYFMC,I_SFZH=@I_SFZH,I_SFZSMJ=@I_SFZSMJ,I_SFZFMSMJ=@I_SFZFMSMJ,I_JYFLXDH=@I_JYFLXDH,I_SSQYS=@I_SSQYS,I_SSQYSHI=@I_SSQYSHI,I_SSQYQ=@I_SSQYQ,I_XXDZ=@I_XXDZ,I_LXRXM=@I_LXRXM ,I_LXRSJH=@I_LXRSJH,I_KHYH=@I_KHYH,I_YHZH=@I_YHZH ,I_ZLTJSJ=@I_ZLTJSJ,B_JSZHMM=@B_JSZHMM,I_YWGLBMFL=@I_YWGLBMFL,I_PTGLJG=@I_PTGLJG,I_GLYHGZRYGH=@I_GLYHGZRYGH,I_GLYH=@I_GLYH,I_YWFWBM=@I_YWFWBM  where B_DLYX=@B_DLYX ";


            arrayList.Add(strDLZHXXB);
            //插入关联表   
            string strGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values (@strNumber,@B_DLYX,@B_JSZHLX,@jjryx,@jjrjsbh,@jjryhm,'是','" + DateTime.Now.ToString() + "','审核中','审核中','否','是','是')";

            arrayList.Add(strGLB);

            Hashtable ht = I_DBL.RunParam_SQL(arrayList, param);
            if ((bool)(ht["return_float"]))
            {
                d.Rows[0]["执行结果"] = "ok";
                d.Rows[0]["提示文本"] = "您的开户申请已提交，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";

            }
            else
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "交易账户信息提交失败！";
                FMipcClass.Log.WorkLog("开通个人经纪人账户", FMipcClass.Log.type.debug, strNumber+ht["return_errmsg"].ToString(), new DataSet());
            }

            dsReturn.Clear();
            dsReturn.Tables.Add(d);
            return dsReturn;
        }
        catch (Exception ex)
        {
            FMipcClass.Log.WorkLog("开通个人买卖家账户", FMipcClass.Log.type.debug, ex.ToString(), new DataSet());
            return null;
        }
    }

    /// <summary>
    /// 修改【经纪人、单位】的交易账户信息
    /// </summary>
    /// <param name="dataTable">包含参数的数据表</param>
    /// <param name="keyNumber">写入关联经纪人信息表的主键</param>
    /// <returns>返回结果</returns>
    public static DataSet UpdateJJRDW(DataTable dataTable, string keyNumber)
    {

        ArrayList arrayList = new ArrayList();

        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" });

        string strSqlZFGLB = "";//作废关联表  "是否有效"字段
        string strSqlCRGLB = "";//插入关联表
        string strSqlGXDLB = "";//更新登录表

        //待拼接的SQL语句，注意要保留空格
        string stra = "update AAA_DLZHXXB set ";

        string strb = " I_SFZH='',I_SFZSMJ='', ";
        string strc = "  I_ZCLB=@I_ZCLB,I_JYFMC=@I_JYFMC,I_YYZZZCH=@I_YYZZZCH,I_YYZZSMJ=@I_YYZZSMJ,I_ZZJGDMZDM=@I_ZZJGDMZDM,I_ZZJGDMZSMJ=@I_ZZJGDMZSMJ,I_SWDJZSH=@I_SWDJZSH,I_SWDJZSMJ=@I_SWDJZSMJ,I_YBNSRZGZSMJ=@I_YBNSRZGZSMJ,I_KHXKZH=@I_KHXKZH,I_KHXKZSMJ=@I_KHXKZSMJ,I_YLYJK=@I_YLYJK,I_FDDBRXM=@I_FDDBRXM,I_FDDBRSFZH=@I_FDDBRSFZH,I_FDDBRSFZSMJ=@I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ=@I_FDDBRSFZFMSMJ,I_FDDBRSQS=@I_FDDBRSQS,I_JYFLXDH=@I_JYFLXDH,I_SSQYS=@I_SSQYS,I_SSQYSHI=@I_SSQYSHI,I_SSQYQ=@I_SSQYQ,I_XXDZ=@I_XXDZ,I_LXRXM=@I_LXRXM,I_LXRSJH=@I_LXRSJH,I_KHYH=@I_KHYH,I_YHZH=@I_YHZH,I_PTGLJG=@I_PTGLJG,I_YXBH=@I_YXBH,I_JJRFL=@I_JJRFL,I_YWGLBMFL=@I_YWGLBMFL where J_JJRJSBH=@J_JJRJSBH ";

        Hashtable hashTableVierfy = new Hashtable();
        hashTableVierfy["是否已经被分公司审核通过"] = dataTable.Rows[0]["经纪人单位是否已被分公司审核通过"].ToString();
        hashTableVierfy["是否已经被经纪人审核通过"] = "";
        if (IsVierifySame("经纪人交易账户", dataTable.Rows[0]["经纪人单位登录邮箱"].ToString(), hashTableVierfy))
        {
            d.Rows[0]["执行结果"] = "ok";
            d.Rows[0]["提示文本"] = "您开通交易账户的申请已审核通过，可以进行相关业务操作，若需要请重新修改资料！";

            dsReturn.Clear();
            dsReturn.Tables.Add(d);
            return dsReturn;

        }

        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        //参数
        param.Add("@B_JSZHLX", dataTable.Rows[0]["经纪人单位交易账户类别"].ToString());
        //param.Add("@J_BUYJSBH", strBuyerJSBH);

        param.Add("@J_JJRJSBH", dataTable.Rows[0]["经纪人单位经纪人角色编号"].ToString());
        //param.Add("@strJJRZGZSBH", strJJRZGZSBH);
        param.Add("@I_ZCLB", dataTable.Rows[0]["经纪人单位交易注册类别"].ToString());
        param.Add("@I_JYFMC", dataTable.Rows[0]["经纪人单位交易方名称"].ToString());
        param.Add("@I_YYZZZCH", dataTable.Rows[0]["经纪人单位营业执照注册号"].ToString());
        param.Add("@I_YYZZSMJ", dataTable.Rows[0]["经纪人单位营业执照扫描件"].ToString());
        param.Add("@I_ZZJGDMZDM", dataTable.Rows[0]["经纪人单位组织机构代码证代码证"].ToString());
        param.Add("@I_ZZJGDMZSMJ", dataTable.Rows[0]["经纪人单位组织机构代码证代码证扫描件"].ToString());
        param.Add("@I_SWDJZSH", dataTable.Rows[0]["经纪人单位税务登记证税号"].ToString());
        param.Add("@I_SWDJZSMJ", dataTable.Rows[0]["经纪人单位税务登记证扫描件"].ToString());
        param.Add("@I_YBNSRZGZSMJ", dataTable.Rows[0]["经纪人单位一般纳税人资格证扫描件"].ToString());
        param.Add("@I_KHXKZH", dataTable.Rows[0]["经纪人单位开户许可证号"].ToString());
        param.Add("@I_KHXKZSMJ", dataTable.Rows[0]["经纪人单位开户许扫描件"].ToString());
        param.Add("@I_YLYJK", dataTable.Rows[0]["经纪人单位预留印鉴卡扫描件"].ToString());
        param.Add("@I_FDDBRXM", dataTable.Rows[0]["经纪人单位法定代表人姓名"].ToString());
        param.Add("@I_FDDBRSFZH", dataTable.Rows[0]["经纪人单位法定代表人身份证号"].ToString());
        param.Add("@I_FDDBRSFZSMJ", dataTable.Rows[0]["经纪人单位法定代表人身份证扫描件"].ToString());
        param.Add("@I_FDDBRSFZFMSMJ", dataTable.Rows[0]["经纪人单位法定代表人身份证反面扫描件"].ToString());
        param.Add("@I_FDDBRSQS", dataTable.Rows[0]["经纪人单位法定代表人授权书"].ToString());
        param.Add("@I_JYFLXDH", dataTable.Rows[0]["经纪人单位交易方联系电话"].ToString());
        param.Add("@I_SSQYS", dataTable.Rows[0]["经纪人单位所属省份"].ToString());
        param.Add("@I_SSQYSHI", dataTable.Rows[0]["经纪人单位所属地市"].ToString());
        param.Add("@I_SSQYQ", dataTable.Rows[0]["经纪人单位所属区县"].ToString());
        param.Add("@I_XXDZ", dataTable.Rows[0]["经纪人单位详细地址"].ToString());
        param.Add("@I_LXRXM", dataTable.Rows[0]["经纪人单位联系人姓名"].ToString());
        param.Add("@I_LXRSJH", dataTable.Rows[0]["经纪人单位联系人手机号"].ToString());
        param.Add("@I_KHYH", dataTable.Rows[0]["经纪人单位开户银行"].ToString());
        param.Add("@I_YHZH", dataTable.Rows[0]["经纪人单位银行账号"].ToString());
        param.Add("@I_PTGLJG", dataTable.Rows[0]["经纪人单位平台管理机构"].ToString());
        param.Add("@I_ZLTJSJ", DateTime.Now.ToString());
        //param.Add("@B_JSZHMM", dataTable.Rows[0]["经纪人单位证券资金密码"].ToString());
        param.Add("@I_YXBH", dataTable.Rows[0]["经纪人单位院系编号"].ToString());
        param.Add("@I_JJRFL", dataTable.Rows[0]["经纪人单位经纪人分类"].ToString());
        param.Add("@I_YWGLBMFL", dataTable.Rows[0]["经纪人单位业务管理部门分类"].ToString());
        param.Add("@B_DLYX", dataTable.Rows[0]["经纪人单位登录邮箱"].ToString());

        param.Add("@jjrdwyhm", dataTable.Rows[0]["经纪人单位用户名"].ToString());

        //获取关联表的Number值
        string strNumber = keyNumber;
        param.Add("@strNumber", strNumber);

        if (dataTable.Rows[0]["经纪人单位是否更换平台管理机构"].ToString() == "是")//更换了平台管理机构
        {
            //1、作废关联表
            strSqlZFGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否', SFYX='否' where DLYX=@B_DLYX and JSZHLX='经纪人交易账户'";
            arrayList.Add(strSqlZFGLB);

            //2、插入关联表   
            strSqlCRGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,JJRSHSJ,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values (@strNumber,@B_DLYX,@B_JSZHLX,@B_DLYX,@J_JJRJSBH,@jjrdwyhm,'是','" + DateTime.Now.ToString() + "','审核通过','" + DateTime.Now.ToString() + "','审核中','否','是','是')";

            arrayList.Add(strSqlCRGLB);
            if (dataTable.Rows[0]["经纪人单位是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //3、更新登录表
                strSqlGXDLB = stra + strb + strc;
                arrayList.Add(strSqlGXDLB);
            }
            else//没有更换注册类别
            {
                strSqlGXDLB = stra + strc;
                arrayList.Add(strSqlGXDLB);
            }
        }
        else//没有更换平台管理机构
        {
            if (dataTable.Rows[0]["经纪人单位是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //4、更新登录表
                strSqlGXDLB = stra + strb + strc;

            }
            else//没有更换注册类别
            {
                strSqlGXDLB = stra + strc;
            }
            arrayList.Add(strSqlGXDLB);
            //判断当前交易账户的审核状态
            Hashtable hts = new Hashtable();
            hts = I_DBL.RunParam_SQL("select FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX=@B_DLYX  and SFDQMRJJR='是' and SFYX='是'", "dts", param);
            DataTable dataTabeUser = ((DataSet)hts["return_ds"]).Tables["dts"];

            if (dataTabeUser != null && dataTabeUser.Rows[0]["分公司审核状态"].ToString() == "驳回")
            {
                string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是',ZHXGSJ='" + DateTime.Now.ToString() + "' where DLYX=@B_DLYX  and SFDQMRJJR='是' and SFYX='是'";
                arrayList.Add(strGXGLB);
            }

        }

        Hashtable ht = I_DBL.RunParam_SQL(arrayList, param);
        if ((bool)(ht["return_float"]))
        {
            d.Rows[0]["执行结果"] = "ok";
            d.Rows[0]["提示文本"] = "您的交易账户信息修改成功！";

        }
        else
        {
            d.Rows[0]["执行结果"] = "err";
            d.Rows[0]["提示文本"] = "交易账户信息修改失败！";
        }

        dsReturn.Clear();
        dsReturn.Tables.Add(d);
        return dsReturn;
    }

    /// <summary>
    /// 修改【经纪人、个人】的交易账户信息
    /// </summary>
    /// <param name="dataTable">参数数据表</param>
    /// <param name="keyNumber">关联经纪人信息表需要的主键</param>
    /// <returns>结果数据集</returns>
    public static DataSet UpdateJJRGR(DataTable dataTable, string keyNumber)
    {

        ArrayList arrayList = new ArrayList();

        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" });

        string strSqlZFGLB = "";//作废关联表  "是否有效"字段
        string strSqlCRGLB = "";//插入关联表
        string strSqlGXDLB = "";//更新登录表

        //待拼接的SQL语句，注意要保留空格
        string stra = "update AAA_DLZHXXB set ";

        string strb = "   I_YYZZZCH='',I_YYZZSMJ='',I_ZZJGDMZDM='',I_ZZJGDMZSMJ='',I_SWDJZSH='',I_SWDJZSMJ='',I_YBNSRZGZSMJ='',I_KHXKZH='',I_KHXKZSMJ='',I_FDDBRXM='',I_FDDBRSFZH='',I_FDDBRSFZSMJ='',I_FDDBRSQS='', ";
        string strc = " I_ZCLB=@I_ZCLB,I_JYFMC=@I_JYFMC,I_SFZH=@I_SFZH,I_SFZSMJ=@I_SFZSMJ,I_SFZFMSMJ=@I_SFZFMSMJ,I_JYFLXDH=@I_JYFLXDH,I_SSQYS=@I_SSQYS,I_SSQYSHI=@I_SSQYSHI,I_SSQYQ=@I_SSQYQ,I_XXDZ=@I_XXDZ,I_LXRXM=@I_LXRXM,I_LXRSJH=@I_LXRSJH,I_KHYH=@I_KHYH,I_YHZH=@I_YHZH,I_PTGLJG=@I_PTGLJG ,I_YXBH=@I_YXBH,I_JJRFL=@I_JJRFL,I_YWGLBMFL=@I_YWGLBMFL  where J_JJRJSBH=@J_JJRJSBH ";

        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();
        //参数
        param.Add("@B_JSZHLX", dataTable.Rows[0]["经纪人个人交易账户类别"].ToString());
        //param.Add("@J_BUYJSBH", strBuyerJSBH);
        param.Add("@J_JJRJSBH", dataTable.Rows[0]["经纪人个人经纪人角色编号"].ToString());
        //param.Add("@strJJRZGZSBH", strJJRZGZSBH);
        param.Add("@I_ZCLB", dataTable.Rows[0]["经纪人个人交易注册类别"].ToString());
        param.Add("@I_JYFMC", dataTable.Rows[0]["经纪人个人交易方名称"].ToString());
        param.Add("@I_SFZH", dataTable.Rows[0]["经纪人个人身份证号"].ToString());
        param.Add("@I_SFZSMJ", dataTable.Rows[0]["经纪人个人身份证扫描件"].ToString());
        param.Add("@I_SFZFMSMJ", dataTable.Rows[0]["经纪人个人身份证反面扫描件"].ToString());
        param.Add("@I_JYFLXDH", dataTable.Rows[0]["经纪人个人交易方联系电话"].ToString());
        param.Add("@I_SSQYS", dataTable.Rows[0]["经纪人个人所属省份"].ToString());
        param.Add("@I_SSQYSHI", dataTable.Rows[0]["经纪人个人所属地市"].ToString());
        param.Add("@I_SSQYQ", dataTable.Rows[0]["经纪人个人所属区县"].ToString());
        param.Add("@I_XXDZ", dataTable.Rows[0]["经纪人个人详细地址"].ToString());
        param.Add("@I_LXRXM", dataTable.Rows[0]["经纪人个人联系人姓名"].ToString());
        param.Add("@I_LXRSJH", dataTable.Rows[0]["经纪人个人联系人手机号"].ToString());
        param.Add("@I_KHYH", dataTable.Rows[0]["经纪人个人开户银行"].ToString());
        param.Add("@I_YHZH", dataTable.Rows[0]["经纪人个人银行账号"].ToString());
        param.Add("@I_PTGLJG", dataTable.Rows[0]["经纪人个人平台管理机构"].ToString());
        param.Add("@I_ZLTJSJ", DateTime.Now.ToString());
        //param.Add("@B_JSZHMM", dataTable.Rows[0]["经纪人个人证券资金密码"].ToString());
        param.Add("@I_YXBH", dataTable.Rows[0]["经纪人个人院系编号"].ToString());
        param.Add("@I_JJRFL", dataTable.Rows[0]["经纪人个人经纪人分类"].ToString());
        param.Add("@I_YWGLBMFL", dataTable.Rows[0]["经纪人个人业务管理部门分类"].ToString());
        param.Add("@B_DLYX", dataTable.Rows[0]["经纪人个人登录邮箱"].ToString());

        param.Add("@strNumber", keyNumber);
        param.Add("@jjryhm", dataTable.Rows[0]["经纪人个人用户名"].ToString());

        Hashtable hashTableVierfy = new Hashtable();
        hashTableVierfy["是否已经被分公司审核通过"] = dataTable.Rows[0]["经纪人个人是否已被分公司审核通过"].ToString();
        hashTableVierfy["是否已经被经纪人审核通过"] = "";
        if (IsVierifySame("经纪人交易账户", dataTable.Rows[0]["经纪人个人登录邮箱"].ToString(), hashTableVierfy))
        {
            d.Rows[0]["执行结果"] = "ok";
            d.Rows[0]["提示文本"] = "您开通交易账户的申请已审核通过，可以进行相关业务操作，若需要请重新修改资料！";

            dsReturn.Clear();
            dsReturn.Tables.Add(d);
            return dsReturn;
        }


        if (dataTable.Rows[0]["经纪人个人是否更换平台管理机构"].ToString() == "是")//更换了平台管理机构
        {
            //作废关联表
            strSqlZFGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否', SFYX='否' where DLYX=@B_DLYX and JSZHLX='经纪人交易账户'";
            arrayList.Add(strSqlZFGLB);

            //插入关联表   
            strSqlCRGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,JJRSHSJ,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values (@strNumber,@B_DLYX,@B_JSZHLX,@B_DLYX,@J_JJRJSBH,@jjryhm,'是','" + DateTime.Now.ToString() + "','审核通过','" + DateTime.Now.ToString() + "','审核中','否','是','是')";
            arrayList.Add(strSqlCRGLB);
            if (dataTable.Rows[0]["经纪人个人是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //更新登录表         
                strSqlGXDLB = stra + strb + strc;
                arrayList.Add(strSqlGXDLB);
            }
            else//没有更换注册类别
            {
                strSqlGXDLB = stra + strc;
                arrayList.Add(strSqlGXDLB);
            }
        }
        else//没有更换平台管理机构
        {
            if (dataTable.Rows[0]["经纪人个人是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //更新登录表
                strSqlGXDLB = stra + strb + strc;

            }
            else//没有更换注册类别
            {
                strSqlGXDLB = stra + strc;
            }
            arrayList.Add(strSqlGXDLB);

            //判断当前交易账户的审核状态
            Hashtable hts = new Hashtable();
            hts = I_DBL.RunParam_SQL("select FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX=@B_DLYX  and SFDQMRJJR='是' and SFYX='是'", "dts", param);
            DataTable dataTabeUser = ((DataSet)hts["return_ds"]).Tables["dts"];

            if (dataTabeUser != null && dataTabeUser.Rows[0]["分公司审核状态"].ToString() == "驳回")
            {
                string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是',ZHXGSJ='" + DateTime.Now.ToString() + "' where DLYX=@B_DLYX  and SFDQMRJJR='是' and SFYX='是'";
                arrayList.Add(strGXGLB);
            }

        }

        Hashtable ht = I_DBL.RunParam_SQL(arrayList, param);
        if ((bool)(ht["return_float"]))
        {
            d.Rows[0]["执行结果"] = "ok";
            d.Rows[0]["提示文本"] = "您的交易账户信息修改成功！";

        }
        else
        {
            d.Rows[0]["执行结果"] = "err";
            d.Rows[0]["提示文本"] = "交易账户信息修改失败！";
        }

        dsReturn.Clear();
        dsReturn.Tables.Add(d);
        return dsReturn;
    }


    /// <summary>
    /// 修改【买卖家、单位】的交易账户信息
    /// </summary>
    /// <param name="dataTable">参数数据表</param>
    /// <param name="keyNumber">作为关联经纪人信息表主键</param>
    /// <returns>包含执行结果和提示文本的数据集</returns>
    public static DataSet UpdateMMJDW(DataTable dataTable, string keyNumber)
    {

        ArrayList arrayList = new ArrayList();

        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" });

        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        DataTable dataGLJJR = new DataTable();
        //参数
        param.Add("@B_JSZHLX", dataTable.Rows[0]["买卖家单位交易账户类别"].ToString());
        param.Add("@strJJRZGZSBH", dataTable.Rows[0]["买卖家单位经纪人资格证书编号"].ToString());
        param.Add("@I_ZCLB", dataTable.Rows[0]["买卖家单位交易注册类别"].ToString());
        param.Add("@I_JYFMC", dataTable.Rows[0]["买卖家单位交易方名称"].ToString());
        param.Add("@I_YYZZZCH", dataTable.Rows[0]["买卖家单位营业执照注册号"].ToString());
        param.Add("@I_YYZZSMJ", dataTable.Rows[0]["买卖家单位营业执照扫描件"].ToString());
        param.Add("@I_ZZJGDMZDM", dataTable.Rows[0]["买卖家单位组织机构代码证代码证"].ToString());
        param.Add("@I_ZZJGDMZSMJ", dataTable.Rows[0]["买卖家单位组织机构代码证代码证扫描件"].ToString());
        param.Add("@I_SWDJZSH", dataTable.Rows[0]["买卖家单位税务登记证税号"].ToString());
        param.Add("@I_SWDJZSMJ", dataTable.Rows[0]["买卖家单位税务登记证扫描件"].ToString());
        param.Add("@I_YBNSRZGZSMJ", dataTable.Rows[0]["买卖家单位一般纳税人资格证扫描件"].ToString());
        param.Add("@I_KHXKZH", dataTable.Rows[0]["买卖家单位开户许可证号"].ToString());
        param.Add("@I_KHXKZSMJ", dataTable.Rows[0]["买卖家单位开户许扫描件"].ToString());
        param.Add("@I_YLYJK", dataTable.Rows[0]["买卖家单位预留印鉴卡扫描件"].ToString());
        param.Add("@I_FDDBRXM", dataTable.Rows[0]["买卖家单位法定代表人姓名"].ToString());
        param.Add("@I_FDDBRSFZH", dataTable.Rows[0]["买卖家单位法定代表人身份证号"].ToString());
        param.Add("@I_FDDBRSFZSMJ", dataTable.Rows[0]["买卖家单位法定代表人身份证扫描件"].ToString());
        param.Add("@I_FDDBRSFZFMSMJ", dataTable.Rows[0]["买卖家单位法定代表人身份证反面扫描件"].ToString());
        param.Add("@I_FDDBRSQS", dataTable.Rows[0]["买卖家单位法定代表人授权书"].ToString());
        param.Add("@I_JYFLXDH", dataTable.Rows[0]["买卖家单位交易方联系电话"].ToString());
        param.Add("@I_SSQYS", dataTable.Rows[0]["买卖家单位所属省份"].ToString());
        param.Add("@I_SSQYSHI", dataTable.Rows[0]["买卖家单位所属地市"].ToString());
        param.Add("@I_SSQYQ", dataTable.Rows[0]["买卖家单位所属区县"].ToString());
        param.Add("@I_XXDZ", dataTable.Rows[0]["买卖家单位详细地址"].ToString());
        param.Add("@I_LXRXM", dataTable.Rows[0]["买卖家单位联系人姓名"].ToString());
        param.Add("@I_LXRSJH", dataTable.Rows[0]["买卖家单位联系人手机号"].ToString());
        param.Add("@I_KHYH", dataTable.Rows[0]["买卖家单位开户银行"].ToString());
        param.Add("@I_YHZH", dataTable.Rows[0]["买卖家单位银行账号"].ToString());
        param.Add("@I_ZLTJSJ", DateTime.Now.ToString());
        param.Add("@I_GLYHGZRYGH", dataTable.Rows[0]["买卖家单位银行工作人员工号"].ToString());
        param.Add("@I_GLYH", dataTable.Rows[0]["买卖家单位关联银行"].ToString());
        param.Add("@I_YWFWBM", dataTable.Rows[0]["买卖家单位业务服务部门"].ToString());
        param.Add("@B_DLYX", dataTable.Rows[0]["买卖家单位登录邮箱"].ToString());
        param.Add("@strNumber", keyNumber);
        param.Add("@glnum", dataTable.Rows[0]["买卖家单位买家卖家与经纪人关联表Number"].ToString().Trim());

        string strSqlZFGLB = "";//作废关联表  "是否有效"字段
        string strSqlCRGLB = "";//插入关联表
        string strSqlGXDLB = "";//更新登录表
        string stra = "update AAA_DLZHXXB set ";
        string strb = " I_SFZH='',I_SFZSMJ='', ";
        string strc = " I_ZCLB=@I_ZCLB,I_JYFMC=@I_JYFMC,I_YYZZZCH=@I_YYZZZCH,I_YYZZSMJ=@I_YYZZSMJ,I_ZZJGDMZDM=@I_ZZJGDMZDM,I_ZZJGDMZSMJ=@I_ZZJGDMZSMJ,I_SWDJZSH=@I_SWDJZSH,I_SWDJZSMJ=@I_SWDJZSMJ,I_YBNSRZGZSMJ=@I_YBNSRZGZSMJ,I_KHXKZH=@I_KHXKZH,I_KHXKZSMJ=@I_KHXKZSMJ,I_YLYJK=@I_YLYJK,I_FDDBRXM=@I_FDDBRXM,I_FDDBRSFZH=@I_FDDBRSFZH,I_FDDBRSFZSMJ=@I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ=@I_FDDBRSFZFMSMJ,I_FDDBRSQS=@I_FDDBRSQS,I_JYFLXDH=@I_JYFLXDH,I_SSQYS=@I_SSQYS,I_SSQYSHI=@I_SSQYSHI,I_SSQYQ=@I_SSQYQ,I_XXDZ=@I_XXDZ,I_LXRXM=@I_LXRXM,I_LXRSJH=@I_LXRSJH,I_KHYH=@I_KHYH,I_YHZH=@I_YHZH ,I_YWGLBMFL=@I_YWGLBMFL,I_PTGLJG=@I_PTGLJG,I_GLYHGZRYGH=@I_GLYHGZRYGH,I_GLYH=@I_GLYH,I_YWFWBM=@I_YWFWBM  where B_DLYX=@B_DLYX";

        Hashtable hashTableVierfy = new Hashtable();
        hashTableVierfy["是否已经被分公司审核通过"] = dataTable.Rows[0]["买卖家单位是否已被分公司审核通过"].ToString();
        hashTableVierfy["是否已经被经纪人审核通过"] = dataTable.Rows[0]["买卖家单位是否已被经纪人审核通过"].ToString();
        if (IsVierifySame("经纪人交易账户", dataTable.Rows[0]["买卖家单位登录邮箱"].ToString(), hashTableVierfy))
        {
            d.Rows[0]["执行结果"] = "ok";
            d.Rows[0]["提示文本"] = "您开通交易账户的申请已经审核通过，可以进行相关业务操作。若有需要，请重新修改资料！";
            dsReturn.Clear();
            dsReturn.Tables.Add(d);
            return dsReturn;
        }

        string strgl = "select B_DLYX '关联经纪人登录邮箱',B_YHM '关联经纪人用户名',J_JJRJSBH '关联经纪人角色编号',I_YWGLBMFL '业务管理部门分类',I_PTGLJG '平台管理机构',* from AAA_DLZHXXB  where J_JJRZGZSBH=@strJJRZGZSBH ";

        Hashtable htjjr = I_DBL.RunParam_SQL(strgl, "dtjjr", param);
        if ((bool)(htjjr["return_float"]))
        {

            dataGLJJR = ((DataSet)htjjr["return_ds"]).Tables["dtjjr"]; ;
            if (dataGLJJR == null || dataGLJJR.Rows.Count <= 0)
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "根据关联的经纪人资格证书编号，无法获取相应的经纪人信息！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }
        }
        else
        {
            return null;
        }
        param.Add("@I_YWGLBMFL", dataGLJJR.Rows[0]["业务管理部门分类"].ToString());
        param.Add("@I_PTGLJG", dataGLJJR.Rows[0]["平台管理机构"].ToString());
        param.Add("@jjryx", dataGLJJR.Rows[0]["关联经纪人登录邮箱"].ToString());
        param.Add("@jjrjsbh", dataGLJJR.Rows[0]["关联经纪人角色编号"].ToString());
        param.Add("@jjryhm", dataGLJJR.Rows[0]["关联经纪人用户名"].ToString());


        if (dataTable.Rows[0]["买卖家单位是否更换经纪人资格证书编号"].ToString() == "是")//更换了经纪人资格证书
        {
            //作废关联表
            strSqlZFGLB = "update  AAA_MJMJJYZHYJJRZHGLB set  SFDQMRJJR='否',SFYX='否'  where DLYX=@B_DLYX and JSZHLX='买家卖家交易账户' and SFSCGLJJR='是'  ";
            arrayList.Add(strSqlZFGLB);

            if (dataTable.Rows[0]["买卖家单位是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //更新登录表      
                strSqlGXDLB = stra + strb + strc;
                arrayList.Add(strSqlGXDLB);
            }
            else//没有更换注册类别
            {
                strSqlGXDLB = stra + strc;
                arrayList.Add(strSqlGXDLB);
            }
            //插入关联表 
            strSqlCRGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values (@strNumber,@B_DLYX,@B_JSZHLX,@jjryx,@jjrjsbh,@jjryhm,'是','" + DateTime.Now.ToString() + "','审核中','审核中','否','是','是')";
            arrayList.Add(strSqlCRGLB);
        }
        else//没有更换平台管理机构
        {

            if (dataTable.Rows[0]["买卖家单位是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //更新登录表
                strSqlGXDLB = stra + strb + strc;

            }
            else//没有更换注册类别
            {
                strSqlGXDLB = stra + strc;
            }
            arrayList.Add(strSqlGXDLB);

            Hashtable hts = I_DBL.RunParam_SQL("select FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX=@B_DLYX and SFDQMRJJR='是' and SFYX='是' ", "dts", param);
            DataTable dataTabeUser = ((DataSet)hts["return_ds"]).Tables["dts"];
            if (dataTabeUser != null && (dataTabeUser.Rows[0]["经纪人审核状态"].ToString() == "驳回" || dataTabeUser.Rows[0]["分公司审核状态"].ToString() == "驳回"))
            {
                string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是',ZHXGSJ='" + DateTime.Now.ToString() + "' where DLYX=@B_DLYX  and SFDQMRJJR='是' and SFYX='是'";
                arrayList.Add(strGXGLB);
            }

            Hashtable htsfxg = I_DBL.RunParam_SQL("select  Number,FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX=@B_DLYX and SFSCGLJJR='否'  and SFYX='是' ", "dtsfxg", param);

            DataTable dataTabeBHHSFXG = ((DataSet)hts["return_ds"]).Tables["dtsfxg"];
            if (dataTabeBHHSFXG != null && dataTabeBHHSFXG.Rows.Count > 0)
            {
                for (int i = 0; i < dataTabeBHHSFXG.Rows.Count; i++)
                {
                    if (dataTabeBHHSFXG.Rows[i]["经纪人审核状态"].ToString() == "驳回")
                    {
                        string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是', ZHXGSJ='" + DateTime.Now.ToString() + "' where Number='" + dataTabeBHHSFXG.Rows[i]["Number"].ToString() + "' ";
                        arrayList.Add(strGXGLB);
                    }
                }
            }

            string strUpdateMMJGLB_FZB = "update AAA_MJMJJYZHYJJRZHGLB_FZB set GLJJRXSYGGH=@I_GLYHGZRYGH where Number=@glnum ";
            arrayList.Add(strUpdateMMJGLB_FZB);

        }

        Hashtable ht = I_DBL.RunParam_SQL(arrayList, param);
        if ((bool)(ht["return_float"]))
        {
            d.Rows[0]["执行结果"] = "ok";
            d.Rows[0]["提示文本"] = "您的交易账户信息修改成功！";

        }
        else
        {
            d.Rows[0]["执行结果"] = "err";
            d.Rows[0]["提示文本"] = "交易账户信息修改失败！";
        }

        dsReturn.Clear();
        dsReturn.Tables.Add(d);
        return dsReturn;
    }

    /// <summary>
    /// 修改【买卖家、个人】的交易账户信息
    /// </summary>    
    /// <param name="dataTable">参数数据表</param>
    /// <param name="keyNumber">写入关联信息表的主键</param>
    /// <returns>包含执行结果和提示文本的数据集</returns>
    public static DataSet UpdateMMJGR(DataTable dataTable, string keyNumber)
    {
        ArrayList arrayList = new ArrayList();

        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" });

        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        DataTable dataGLJJR = new DataTable();

        //参数
        param.Add("@B_JSZHLX", dataTable.Rows[0]["买卖家个人交易账户类别"].ToString());
        param.Add("@strJJRZGZSBH", dataTable.Rows[0]["买卖家个人经纪人资格证书编号"].ToString());

        param.Add("@I_ZCLB", dataTable.Rows[0]["买卖家个人交易注册类别"].ToString());
        param.Add("@I_JYFMC", dataTable.Rows[0]["买卖家个人交易方名称"].ToString());

        param.Add("@I_SFZH", dataTable.Rows[0]["买卖家个人身份证号"].ToString());
        param.Add("@I_SFZSMJ", dataTable.Rows[0]["买卖家个人身份证扫描件"].ToString());
        param.Add("@I_SFZFMSMJ", dataTable.Rows[0]["买卖家个人身份证反面扫描件"].ToString());
        param.Add("@I_JYFLXDH", dataTable.Rows[0]["买卖家个人交易方联系电话"].ToString());
        param.Add("@I_SSQYS", dataTable.Rows[0]["买卖家个人所属省份"].ToString());
        param.Add("@I_SSQYSHI", dataTable.Rows[0]["买卖家个人所属地市"].ToString());
        param.Add("@I_SSQYQ", dataTable.Rows[0]["买卖家个人所属区县"].ToString());
        param.Add("@I_XXDZ", dataTable.Rows[0]["买卖家个人详细地址"].ToString());
        param.Add("@I_LXRXM", dataTable.Rows[0]["买卖家个人联系人姓名"].ToString());
        param.Add("@I_LXRSJH", dataTable.Rows[0]["买卖家个人联系人手机号"].ToString());
        param.Add("@I_KHYH", dataTable.Rows[0]["买卖家个人开户银行"].ToString());
        param.Add("@I_YHZH", dataTable.Rows[0]["买卖家个人银行账号"].ToString());
        param.Add("@I_ZLTJSJ", DateTime.Now.ToString());
        //param.Add("@B_JSZHMM", dataTable.Rows[0]["买卖家个人证券资金密码"].ToString());
        param.Add("@B_DLYX", dataTable.Rows[0]["买卖家个人登录邮箱"].ToString());       

        param.Add("@I_GLYHGZRYGH", dataTable.Rows[0]["买卖家个人关联银行工作人员工号"].ToString());
        param.Add("@I_GLYH", dataTable.Rows[0]["买卖家个人关联银行"].ToString());
        param.Add("@I_YWFWBM", dataTable.Rows[0]["买卖家个人业务服务部门"].ToString());
        param.Add("@strNumber", keyNumber);
        param.Add("@sellerjsbh", dataTable.Rows[0]["买卖家个人卖家角色编号"].ToString());
        param.Add("@glnum", dataTable.Rows[0]["买卖家个人买家卖家与经纪人关联表Number"].ToString().Trim());

        string strSqlZFGLB = "";//作废关联表  "是否有效"字段
        string strSqlCRGLB = "";//插入关联表
        string strSqlGXDLB = "";//更新登录表

        string stra = "update AAA_DLZHXXB set ";
        string strb = " I_YYZZZCH='',I_YYZZSMJ='',I_ZZJGDMZDM='',I_ZZJGDMZSMJ='',I_SWDJZSH='',I_SWDJZSMJ='',I_YBNSRZGZSMJ='',I_KHXKZH='',I_KHXKZSMJ='',I_FDDBRXM='',I_FDDBRSFZH='',I_FDDBRSFZSMJ='',I_FDDBRSQS='',";
        string strc = " I_ZCLB=@I_ZCLB,I_JYFMC=@I_JYFMC,I_SFZH=@I_SFZH,I_SFZSMJ=@I_SFZSMJ,I_SFZFMSMJ=@I_SFZFMSMJ,I_JYFLXDH=@I_JYFLXDH,I_SSQYS=@I_SSQYS,I_SSQYSHI=@I_SSQYSHI,I_SSQYQ=@I_SSQYQ,I_XXDZ=@I_XXDZ,I_LXRXM=@I_LXRXM ,I_LXRSJH=@I_LXRSJH,I_KHYH=@I_KHYH,I_YHZH=@I_YHZH ,I_YWGLBMFL=@I_YWGLBMFL,I_PTGLJG=@I_PTGLJG,I_GLYHGZRYGH=@I_GLYHGZRYGH,I_GLYH=@I_GLYH,I_YWFWBM=@I_YWFWBM  where J_SELJSBH=@sellerjsbh";

        Hashtable hashTableVierfy = new Hashtable();
        hashTableVierfy["是否已经被分公司审核通过"] = dataTable.Rows[0]["买卖家个人是否已被分公司审核通过"].ToString();
        hashTableVierfy["是否已经被经纪人审核通过"] = dataTable.Rows[0]["买卖家个人是否已被经纪人审核通过"].ToString();
        if (IsVierifySame("经纪人交易账户", dataTable.Rows[0]["买卖家个人登录邮箱"].ToString(), hashTableVierfy))
        {
            d.Rows[0]["执行结果"] = "ok";
            d.Rows[0]["提示文本"] = "您开通交易账户的申请已审核通过，可以进行相关业务操作，若需要请重新修改资料！";

            dsReturn.Clear();
            dsReturn.Tables.Add(d);
            return dsReturn;
        }

        string strgl = "select B_DLYX '关联经纪人登录邮箱',B_YHM '关联经纪人用户名',J_JJRJSBH '关联经纪人角色编号',I_YWGLBMFL '业务管理部门分类',I_PTGLJG '平台管理机构',* from AAA_DLZHXXB  where J_JJRZGZSBH=@strJJRZGZSBH ";

        Hashtable htjjr = I_DBL.RunParam_SQL(strgl, "dtjjr", param);
        if ((bool)(htjjr["return_float"]))
        {
            dataGLJJR = ((DataSet)htjjr["return_ds"]).Tables["dtjjr"]; ;
            if (dataGLJJR == null || dataGLJJR.Rows.Count <= 0)
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "根据关联的经纪人资格证书编号，无法获取相应的经纪人信息！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }
        }
        else
        {
            return null;
        }

        param.Add("@I_YWGLBMFL", dataGLJJR.Rows[0]["业务管理部门分类"].ToString());
        param.Add("@I_PTGLJG", dataGLJJR.Rows[0]["平台管理机构"].ToString());
        param.Add("@jjryx", dataGLJJR.Rows[0]["关联经纪人登录邮箱"].ToString());
        param.Add("@jjrjsbh", dataGLJJR.Rows[0]["关联经纪人角色编号"].ToString());
        param.Add("@jjryhm", dataGLJJR.Rows[0]["关联经纪人用户名"].ToString());

        if (dataTable.Rows[0]["买卖家个人是否更换经纪人资格证书编号"].ToString() == "是")//更换了经纪人资格证书编号
        {
            //作废关联表
            strSqlZFGLB = "update  AAA_MJMJJYZHYJJRZHGLB set  SFDQMRJJR='否',SFYX='否'  where DLYX=@B_DLYX and JSZHLX='买家卖家交易账户' and SFSCGLJJR='是'  ";
            arrayList.Add(strSqlZFGLB);


            if (dataTable.Rows[0]["买卖家个人是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //更新登录表                
                strSqlGXDLB = stra + strb + strc;
                arrayList.Add(strSqlGXDLB);
            }
            else//没有更换注册类别
            {
                strSqlGXDLB = stra + strc;
                arrayList.Add(strSqlGXDLB);
            }
            strSqlCRGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values (@strNumber,@B_DLYX,@B_JSZHLX,@jjryx,@jjrjsbh,@jjryhm,'是','" + DateTime.Now.ToString() + "','审核中','审核中','否','是','是')";
            arrayList.Add(strSqlCRGLB);
        }
        else//没有更换经纪人资格证书编号
        {
            if (dataTable.Rows[0]["买卖家个人是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //更新登录表
                strSqlGXDLB = stra + strb + strc;
            }
            else//没有更换注册类别
            {
                strSqlGXDLB = stra + strc;
            }
            arrayList.Add(strSqlGXDLB);


            Hashtable hts = I_DBL.RunParam_SQL("select FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX=@B_DLYX and SFDQMRJJR='是' and SFYX='是' ", "dts", param);
            DataTable dataTabeUser = ((DataSet)hts["return_ds"]).Tables["dts"];
            if (dataTabeUser != null && (dataTabeUser.Rows[0]["经纪人审核状态"].ToString() == "驳回" || dataTabeUser.Rows[0]["分公司审核状态"].ToString() == "驳回"))
            {
                string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是',ZHXGSJ='" + DateTime.Now.ToString() + "' where DLYX=@B_DLYX  and SFDQMRJJR='是' and SFYX='是'";
                arrayList.Add(strGXGLB);
            }

            Hashtable htsfxg = I_DBL.RunParam_SQL("select  Number,FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX=@B_DLYX and SFSCGLJJR='否'  and SFYX='是' ", "dtsfxg", param);

            DataTable dataTabeBHHSFXG = ((DataSet)hts["return_ds"]).Tables["dtsfxg"];
            if (dataTabeBHHSFXG != null && dataTabeBHHSFXG.Rows.Count > 0)
            {
                for (int i = 0; i < dataTabeBHHSFXG.Rows.Count; i++)
                {
                    if (dataTabeBHHSFXG.Rows[i]["经纪人审核状态"].ToString() == "驳回")
                    {
                        string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是', ZHXGSJ='" + DateTime.Now.ToString() + "' where Number='" + dataTabeBHHSFXG.Rows[i]["Number"].ToString() + "' ";
                        arrayList.Add(strGXGLB);
                    }
                }
            }

            string strUpdateMMJGLB_FZB = "update AAA_MJMJJYZHYJJRZHGLB_FZB set GLJJRXSYGGH=@I_GLYHGZRYGH where Number=@glnum ";
            arrayList.Add(strUpdateMMJGLB_FZB);

        }
        Hashtable ht = I_DBL.RunParam_SQL(arrayList, param);
        if ((bool)(ht["return_float"]))
        {
            d.Rows[0]["执行结果"] = "ok";
            d.Rows[0]["提示文本"] = "您的交易账户信息修改成功！";

        }
        else
        {
            d.Rows[0]["执行结果"] = "err";
            d.Rows[0]["提示文本"] = "交易账户信息修改失败！";
        }

        dsReturn.Clear();
        dsReturn.Tables.Add(d);
        return dsReturn;

    }

    /// <summary>
    /// 经纪人审核买卖家的数据信息
    /// </summary>    
    /// <param name="dataTable">参数数据表</param>
    /// <returns></returns>
    public static DataSet JJRPassMMJ(DataTable dataTable)
    {

        ArrayList arrayList = new ArrayList();

        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Columns.Add("附件信息1",typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" ,""});

        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@seljsbh", dataTable.Rows[0]["JSBH"].ToString());
        param.Add("@dlyx", dataTable.Rows[0]["DLYX"].ToString());
        param.Add("@shyj", dataTable.Rows[0]["SHYJ"].ToString());
        param.Add("@glnum", dataTable.Rows[0]["关联表Number"].ToString());
        param.Add("@jjrjsbh", dataTable.Rows[0]["GLJJRBH"].ToString());


        if (IsGHJJR(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()) == "是")//当前执行审核操作的经纪人是  选择的经纪人 
        {
            string strSqlMRJJR_GH = "select a.GLJJRBH '关联经纪人编号',c.B_SFDJ '是否冻结',c.B_SFXM '是否休眠' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH=@seljsbh and a.SFDQMRJJR='是' and a.SFYX='是' and a.SFSCGLJJR='否'";

            Hashtable hts = I_DBL.RunParam_SQL(strSqlMRJJR_GH, "dts", param);

            DataTable dataTable_GH = ((DataSet)hts["return_ds"]).Tables["dts"];
            if (dataTable_GH != null && (dataTable_GH.Rows.Count <= 0 || dataTable_GH.Rows[0]["是否冻结"].ToString() == "是" || dataTable_GH.Rows[0]["是否休眠"].ToString() == "是"))//该买卖家账户关联的所的经纪人账户尚未对该账户进行审核操作，或者有些经济人对此账户执行了驳回操作，或者//当前更换的默认经纪人已经被冻结或者休眠
            {

                //1、更新原来交易账户的默认经纪人为否 （处理的是第一次关联的经纪人）
                string strSqlFirst = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否' where DLYX=@dlyx and JSZHLX='买家卖家交易账户' and SFSCGLJJR='是' and JJRSHZT='审核通过'";
                arrayList.Add(strSqlFirst);
                //2、更新所有更换的经纪人在关联表中的数据位作废状态（根据【是否首次关联的经纪人】字段来确认）
                string strSqlSecond = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否',SFYX='否' where DLYX=@dlyx and JSZHLX='买家卖家交易账户' and SFSCGLJJR='否' and  JJRSHZT in('审核中','驳回') ";
                arrayList.Add(strSqlSecond);
                //2.1更新所有已经关联过的审核通过的经纪人
                string strSqlSecond1 = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否' where DLYX=@dlyx and JSZHLX='买家卖家交易账户' and SFSCGLJJR='否' and  JJRSHZT in('审核通过') ";
                arrayList.Add(strSqlSecond1);
                //3、跟新当前关联表中Number值的记录为有效状态，并且设为默人经纪人，并且数据是有效状态
                string strSqlThird = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='是',SFYX='是',JJRSHZT='审核通过',JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ=@shyj where Number=@glnum ";
                arrayList.Add(strSqlThird);

                //4、更新当前买卖家账户的“业务服务部门”字段值为：当前关联经纪人的经纪人分类 ；
                string strJJRFL = "select I_JJRFL,I_JYFMC from AAA_DLZHXXB where J_JJRJSBH=@jjrjsbh ";

                Hashtable htj = I_DBL.RunParam_SQL(strJJRFL, "dtj", param);
                DataSet dsJJRFL = (DataSet)htj["return_ds"];

                string strUpdateYWFWBM = "";
                string strUpdateGLB_FZB = "";
                if (dsJJRFL != null && dsJJRFL.Tables[0].Rows.Count > 0)
                {
                    string jjrfl = dsJJRFL.Tables[0].Rows[0]["I_JJRFL"].ToString();
                    string jjfmc = dsJJRFL.Tables[0].Rows[0]["I_JYFMC"].ToString();
                    if (jjrfl == "银行")
                    {
                        strUpdateYWFWBM = "update AAA_DLZHXXB set I_YWFWBM='" + jjrfl.Trim() + "',I_GLYH='" + jjfmc.Trim() + "',I_GLYHGZRYGH='无' where J_SELJSBH=@seljsbh ";

                        #region//变更经纪人的时候，往买卖家关联表里插入了数据，此时写入关联表辅助表员工工号的数据是从登陆表里取得，而这时登陆表里员工工号还是原来默认经纪人时的数据，所以在这个节点往买卖家关联表辅助表插入的员工工号是错误的，所以在经纪人审核的的时候把辅助表里员工工号更新成正确的（“即如果审核的经纪人是银行类型的，则员工工号为‘无’；如果审核的经纪人是一般经纪人，则员工工号是空”）

                        strUpdateGLB_FZB = "update AAA_MJMJJYZHYJJRZHGLB_FZB set GLJJRXSYGGH='无' where number=@glnum ";
                        #endregion
                    }
                    else
                    {
                        strUpdateYWFWBM = "update AAA_DLZHXXB set I_YWFWBM='" + jjrfl.Trim() + "',I_GLYH='',I_GLYHGZRYGH='' where J_SELJSBH=@seljsbh ";
                        #region//变更经纪人的时候，往买卖家关联表里插入了数据，此时写入关联表辅助表员工工号的数据是从登陆表里取得，而这时登陆表里员工工号还是原来默认经纪人时的数据，所以在这个节点往买卖家关联表辅助表插入的员工工号是错误的，所以在经纪人审核的的时候把辅助表里员工工号更新成正确的（“即如果审核的经纪人是银行类型的，则员工工号为‘无’；如果审核的经纪人是一般经纪人，则员工工号是空”）
                        strUpdateGLB_FZB = "update AAA_MJMJJYZHYJJRZHGLB_FZB set GLJJRXSYGGH='' where number=@glnum ";
                        #endregion
                    }
                }
                else
                {
                    d.Rows[0]["执行结果"] = "err";
                    d.Rows[0]["提示文本"] = "未查询到当前关联经纪人的信息！";
                    dsReturn.Clear();
                    dsReturn.Tables.Add(d);
                    return dsReturn;
                }

                arrayList.Add(strUpdateYWFWBM);
                arrayList.Add(strUpdateGLB_FZB);

                Hashtable ht = I_DBL.RunParam_SQL(arrayList, param);
                if ((bool)(ht["return_float"]))
                {
                    d.Rows[0]["执行结果"] = "ok";
                    d.Rows[0]["提示文本"] = "您的审核信息提交成功！";
                }
                else
                {
                    d.Rows[0]["执行结果"] = "err";
                    d.Rows[0]["提示文本"] = "您的审核信息提交失败！";
                }

                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }

            else if (!dataTable_GH.Rows[0]["关联经纪人编号"].ToString().Equals(dataTable.Rows[0]["GLJJRBH"].ToString()))//该买卖家交易账户已被其他经纪人审核通过
            {
                d.Rows[0]["执行结果"] = "warr";
                d.Rows[0]["提示文本"] = "此交易账户已被其它经纪人审核通过，无需审核！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }
            else if (dataTable_GH.Rows[0]["关联经纪人编号"].ToString().Equals(dataTable.Rows[0]["GLJJRBH"].ToString()))//该买卖家交易账户已被审核通过
            {
                d.Rows[0]["执行结果"] = "warr";
                d.Rows[0]["提示文本"] = "您已审核通过该交易账户，无需审核！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;

            }
        }
        else  //当前的经纪人是首次关联的经纪人
        {
            d.Rows[0]["附件信息1"] = "首次关联";
            string strsql = "select a.Number from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and a.GLJJRBH=@jjrjsbh and b.J_SELJSBH=@seljsbh and a.SFSCGLJJR='是' and a.SFDQMRJJR='是'";

            Hashtable htm = I_DBL.RunParam_SQL(strsql, "dtm", param);
            DataTable dtm = ((DataSet)htm["return_ds"]).Tables["dtm"];
            if (!(dtm != null && dtm.Rows.Count > 0))
            {
                d.Rows[0]["执行结果"] = "warr";
                d.Rows[0]["提示文本"] = "当前交易账户已更换经纪人，无需审核！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }


            Hashtable htpars = new TaskGetData().GetData(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString(), dataTable.Rows[0]["注册类别"].ToString(), dataTable.Rows[0]["DLYX"].ToString());
            if (htpars == null || htpars.Count != 2)
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "您的审核信息提交失败！";
            }

            if (htpars["是否注册类别"].ToString().Trim() == "是")
            {
                d.Rows[0]["执行结果"] = "Reject";
                d.Rows[0]["提示文本"] = "当前交易账户已更换注册类别，请重新审核！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }
            if (htpars["是否审核通过"].ToString().Trim() == "是")
            {
                d.Rows[0]["执行结果"] = "warr";
                d.Rows[0]["提示文本"] = "您已经审核通过该交易账户，不能进行驳回操作！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }

                string strSql1 = "";
                string strSql2 = "";

                strSql1 = "update AAA_DLZHXXB set S_SFYBJJRSHTG='是' where B_JSZHLX='买家卖家交易账户' and  J_SELJSBH=@seljsbh  ";
                strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT='审核通过',JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ=@shyj    where JSZHLX='买家卖家交易账户' and DLYX=@dlyx and  GLJJRBH=@jjrjsbh and SFYX='是'  ";
                arrayList.Add(strSql1);
                arrayList.Add(strSql2);
            
        }

        Hashtable htc = I_DBL.RunParam_SQL(arrayList, param);
        if ((bool)(htc["return_float"]))
        {
            d.Rows[0]["执行结果"] = "ok";
            d.Rows[0]["提示文本"] = "您的审核信息提交成功！";
        }
        else
        {
            d.Rows[0]["执行结果"] = "err";
            d.Rows[0]["提示文本"] = "您的审核信息提交失败！";

        }
        dsReturn.Clear();
        dsReturn.Tables.Add(d);
        return dsReturn;
    }

    /// <summary>
    /// 经纪人驳回 买卖家的数据信息
    /// </summary>
    /// <param name="dataTable">参数数据表</param>
    /// <returns>包含执行结果和提示文本的数据集</returns>
    public static DataSet JJRRejectMMJ(DataTable dataTable)
    {

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //  开始监
        ArrayList arrayList = null;//new ArrayList();

        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Columns.Add("附件信息1", typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" });

        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@seljsbh", dataTable.Rows[0]["JSBH"].ToString());
        param.Add("@dlyx", dataTable.Rows[0]["DLYX"].ToString());
        param.Add("@shyj", dataTable.Rows[0]["SHYJ"].ToString());
        param.Add("@glnum", dataTable.Rows[0]["关联表Number"].ToString());
        param.Add("@jjrjsbh", dataTable.Rows[0]["GLJJRBH"].ToString());

        if (IsGHJJR(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()) == "是")//当前执行审核操作的经纪人是  选择的经纪人 
        {

            arrayList = new ArrayList();
            string strSqlMRJJR_GH = "select a.GLJJRBH '关联经纪人编号',c.B_SFDJ '是否冻结',c.B_SFXM '是否休眠' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH=@seljsbh and a.SFDQMRJJR='是' and a.SFYX='是' and a.SFSCGLJJR='否'";


            Hashtable hts = I_DBL.RunParam_SQL(strSqlMRJJR_GH, "dts", param);
            DataTable dataTable_GH = ((DataSet)hts["return_ds"]).Tables["dts"];
            
            string strSql1 = "";
            if (dataTable_GH == null || dataTable_GH.Rows.Count <= 0)//该买卖家账户关联的所的经纪人账户尚未对该账户进行审核操作，或者有些经济人对此账户执行了驳回操作
            {
                strSql1 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT='驳回',SFXG='否',JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ=@shyj  where JSZHLX='买家卖家交易账户' and DLYX=@dlyx and  Number=@glnum ";
            }
            else if (dataTable_GH != null && (dataTable_GH.Rows[0]["是否冻结"].ToString() == "是" || dataTable_GH.Rows[0]["是否休眠"].ToString() == "是"))//当前更换的默认经纪人已经被冻结或者休眠
            {
                strSql1 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT='驳回',SFXG='否',JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ=@shyj   where JSZHLX='买家卖家交易账户' and DLYX=@dlyx and  Number=@glnum  ";
            }
            else if (!dataTable_GH.Rows[0]["关联经纪人编号"].ToString().Equals(dataTable.Rows[0]["GLJJRBH"].ToString()))//该买卖家交易账户已被其他经纪人审核通过
            {
                d.Rows[0]["执行结果"] = "warr";
                d.Rows[0]["提示文本"] = "此交易账户已被其它经纪人审核通过，无需审核！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }
            else if (dataTable_GH.Rows[0]["关联经纪人编号"].ToString().Equals(dataTable.Rows[0]["GLJJRBH"].ToString()))//该买卖家交易账户已被审核通过
            {
                d.Rows[0]["执行结果"] = "warr";
                d.Rows[0]["提示文本"] = "您已审核通过该交易账户，无需审核！";

                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }

            arrayList.Add(strSql1);
            Hashtable ht = I_DBL.RunParam_SQL(arrayList, param);
            if ((bool)(ht["return_float"]))
            {
                d.Rows[0]["执行结果"] = "ok";
                d.Rows[0]["提示文本"] = "您的审核信息提交成功！";
            }
            else
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "您的审核信息提交失败！";
            }
            dsReturn.Clear();
            dsReturn.Tables.Add(d);

           
            return dsReturn;


        }
        else   //当前的经纪人是首次关联的经纪人
        {
            arrayList = new ArrayList();
            d.Rows[0]["附件信息1"] = "首次关联";
           // string strsql = "select * from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and a.GLJJRBH=@jjrjsbh and b.J_SELJSBH=@seljsbh and a.SFSCGLJJR='是' and a.SFDQMRJJR='是'"; wyh dele 没有必要检索所有字段
            string strsql = "select a.Number,GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and a.GLJJRBH=@jjrjsbh and b.J_SELJSBH=@seljsbh and a.SFSCGLJJR='是' and a.SFDQMRJJR='是'";
            Hashtable htm = I_DBL.RunParam_SQL(strsql, "dtm", param);
            DataTable dtm = ((DataSet)htm["return_ds"]).Tables["dtm"];
            
            if (!(dtm != null && dtm.Rows.Count > 0))
            {
                d.Rows[0]["执行结果"] = "warr";
                d.Rows[0]["提示文本"] = "当前交易账户已更换经纪人，无需审核！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            stopwatch.Restart();
            Hashtable htpars = new TaskGetData().GetData(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString(), dataTable.Rows[0]["注册类别"].ToString(), dataTable.Rows[0]["DLYX"].ToString());
            if (htpars == null || htpars.Count != 2)
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "您的审核信息提交失败！";
            }

            stopwatch.Stop();
            TimeSpan ts11 = stopwatch.Elapsed;
            TimeSpan tsjifen = new TimeSpan(); ;
            Hashtable htpar = new Hashtable();
            if (htpars["是否注册类别"].ToString().Trim() == "是")
            {
                d.Rows[0]["执行结果"] = "Reject";
                d.Rows[0]["提示文本"] = "当前交易账户已更换注册类别，请重新审核！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }
            if (htpars["是否审核通过"].ToString().Trim() == "是")
            {
                d.Rows[0]["执行结果"] = "warr";
                d.Rows[0]["提示文本"] = "您已经审核通过该交易账户，不能进行驳回操作！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }
            else
            {
              
                htpar.Add("@S_SFYBJJRSHTG", "否");
                htpar.Add("@B_JSZHLX", "买家卖家交易账户");
                htpar.Add("@J_SELJSBH", dataTable.Rows[0]["JSBH"].ToString());
                htpar.Add("@JJRSHZT", "驳回");
                htpar.Add("@SFXG", "否");
                htpar.Add("@JJRSHSJ", DateTime.Now.ToString());
                htpar.Add("@JJRSHYJ", dataTable.Rows[0]["SHYJ"].ToString());
                htpar.Add("@DLYX", dataTable.Rows[0]["DLYX"].ToString());
                htpar.Add("@GLJJRBH", dataTable.Rows[0]["GLJJRBH"].ToString());
                htpar.Add("@SFYX", "是");
                string strSql1 = "";
                string strSql2 = "";
                //strSql1 = "update AAA_DLZHXXB set S_SFYBJJRSHTG='否' where B_JSZHLX='买家卖家交易账户' and  J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";
                strSql1 = "update AAA_DLZHXXB set S_SFYBJJRSHTG=@S_SFYBJJRSHTG where B_JSZHLX=@B_JSZHLX and  J_SELJSBH=@J_SELJSBH";
                //strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT='驳回',SFXG='否' ,JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ='" + dataTable.Rows[0]["SHYJ"].ToString() + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and  GLJJRBH='" + dataTable.Rows[0]["GLJJRBH"].ToString() + "' and SFYX='是' ";
                strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT=@JJRSHZT,SFXG=@SFXG ,JJRSHSJ=@JJRSHSJ,JJRSHYJ=@JJRSHYJ   where JSZHLX=@B_JSZHLX and DLYX=@DLYX and  GLJJRBH=@GLJJRBH and SFYX=@SFYX ";

                arrayList.Add(strSql1);
                arrayList.Add(strSql2);

                #region 积分计算 wyh 2017.07.28 
                stopwatch.Restart();
               
                DataTable dtjf = new DataTable();
                DataColumn dc = new DataColumn("登录邮箱", typeof(string));
                dtjf.Columns.Add(dc);
                DataColumn dc1 = new DataColumn("经纪人邮箱", typeof(string));
                dtjf.Columns.Add(dc1);
                dtjf.Clear();
                DataRow drht = dtjf.NewRow(); ;//dt.NewRow();

                drht["登录邮箱"] = dataTable.Rows[0]["DLYX"].ToString();
                drht["经纪人邮箱"] = dtm.Rows[0]["GLJJRDLZH"].ToString();

                dtjf.Rows.Add(drht);
                dtjf.AcceptChanges();
                
               // XYDJJFCL xyjf = new XYDJJFCL();
              
               // xyjf.DealXYJF(dtjf, "经纪人驳回买卖家");

                new TaskGetData().CountJifen(dtjf);
                stopwatch.Stop();
                 tsjifen= stopwatch.Elapsed;
                 stopwatch.Restart();
                #endregion 
            }

            Hashtable ht = I_DBL.RunParam_SQL(arrayList, htpar);
            
            if ((bool)(ht["return_float"]))
            {

                d.Rows[0]["执行结果"] = "ok";
                d.Rows[0]["提示文本"] = "您的审核信息提交成功！";
            }
            else
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "您的审核信息提交失败！";
            }

            stopwatch.Stop();
            TimeSpan tsaa = stopwatch.Elapsed;
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "C 经纪人驳回买卖家前段执行时间：" + ts.TotalMilliseconds.ToString("#.#####") + "*多线程查询数据库时间验证获取时间：" + ts11.TotalMilliseconds.ToString("#.#####") + ";执行update语句时间" + tsaa.TotalMilliseconds.ToString("#.#####") + "积分操作：" + tsjifen.TotalMilliseconds.ToString("#.#####") , null);
            dsReturn.Clear();
            dsReturn.Tables.Add(d);

            
            return dsReturn;
        }
    }


    /// <summary>
    /// 当选择经纪人时获取要判断的数据信息
    /// </summary>    
    /// <param name="dataTable">参数数据表</param>
    /// <param name="keyNumber">写入关联经纪人信息表的主键</param>
    /// <returns>一个包含执行结果和提示文本的数据集</returns>
    public static DataSet ChooseJJR(DataTable dataTable, string keyNumber)
    {
        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" });

        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@JSBH", dataTable.Rows[0]["卖家角色编号"].ToString());
        param.Add("@MMJDLYX", dataTable.Rows[0]["买卖家登录邮箱"].ToString());
        param.Add("@JSZHLX", dataTable.Rows[0]["结算账户类型"].ToString());
        param.Add("@strWhere", dataTable.Rows[0]["搜索经纪人资格证书编号"].ToString());
        param.Add("@strGLJJRDLYX", dataTable.Rows[0]["关联经纪人登录邮箱"].ToString());
        param.Add("@strGLJJRJSBH", dataTable.Rows[0]["关联经纪人角色编号"].ToString());
        param.Add("@strGLJJRYHM", dataTable.Rows[0]["关联经纪人用户名"].ToString());
        param.Add("@strNumber", keyNumber);

        //查询当前经纪人的状态信息
        string strSqlDQJJRZT = "select c.I_PTGLJG '经纪人_平台管理机构',c.B_SFDJ '经纪人_冻结状态',c.B_SFXM '经纪人_休眠状态',b.I_SSQYS '买卖家_所属区域省',b.I_SSQYSHI '买卖家_所属区域市',b.I_SSQYQ '买卖家_所属区域区'  from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH   where a.JSZHLX=@JSZHLX  and a.SFDQMRJJR='是'  and a.SFYX='是' and a.JJRSHZT='审核通过' and a.FGSSHZT='审核通过' and a.DLYX=@MMJDLYX";

        Hashtable htdqzt = I_DBL.RunParam_SQL(strSqlDQJJRZT, "dts", param);
        DataTable dataTableDQJJRZT = ((DataSet)htdqzt["return_ds"]).Tables["dts"];

        string strSqlJJRSHZT = "select a.DLYX '买卖家登录邮箱',c.J_JJRZGZSBH '经纪人资格证书编号',c.I_JYFMC '经纪人交易方名称',c.B_DLYX '经纪人登录邮箱',a.JJRSHZT '经纪人审核状态',a.JJRSHSJ '经纪人审核时间',a.JJRSHYJ '经纪人审核意见',a.SQSJ '申请时间' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH where a.JSZHLX='买家卖家交易账户' and a.SFYX='是' and  a.DLYX=@MMJDLYX and a.SFSCGLJJR='否' and c.B_SFDJ='否' and c.B_SFXM='否' and a.JJRSHZT='审核通过' and a.SFDQMRJJR='是' order by a.SQSJ desc";

        Hashtable htshzt = I_DBL.RunParam_SQL(strSqlJJRSHZT, "dth", param);
        DataTable dataTableGLGJJR = ((DataSet)htshzt["return_ds"]).Tables["dth"];

        //查询当前关联的经纪人的记录信息，用于防止重复选择单个经纪人
        string strSqlFZCFXZJJR = "select * from AAA_MJMJJYZHYJJRZHGLB where DLYX= @MMJDLYX  and GLJJRBH=@strGLJJRJSBH  and SFDQMRJJR='否' and SFSCGLJJR='否' and SFYX='是' and JJRSHZT in('审核中','驳回')";
        Hashtable htjl = I_DBL.RunParam_SQL(strSqlFZCFXZJJR, "dtl", param);
        DataTable dataFZCFXZJJR = ((DataSet)htjl["return_ds"]).Tables["dtl"];

        //查询当前经纪人的数据信息
        string strSqlJJR = "select a.DLYX '经纪人登录邮箱',b.J_JJRZGZSBH '经纪人资格证书编号',b.I_JYFMC '经纪人名称',b.J_JJRJSBH '经纪人角色编号',b.B_YHM '经纪人用户名','是' '是否当前分公司下经纪人','是' '是否阶段一筛选结果', b.I_PTGLJG '平台管理机构',b.I_SSQYS '经纪人_所属区域省',b.I_SSQYSHI '经纪人_所属区域市',b.I_SSQYQ '经纪人_所属区域区',b.I_SSQYS+b.I_SSQYSHI  '经纪人_所属区域省市',a.FGSSHSJ '分公司审核时间',a.FGSSHZT '分公司审核状态',b.B_SFDJ '是否冻结', b.B_SFXM '是否休眠',b.B_SFYXDL '是否允许登录',b.J_JJRSFZTXYHSH '经纪人是否暂停新用户审核',b.DJGNX as 冻结功能项 from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where a.JSZHLX='经纪人交易账户' and a.JJRSHZT='审核通过' and a.FGSSHZT='审核通过' and a.SFYX='是' and b.J_JJRJSBH=@strGLJJRJSBH ";

        Hashtable htjjr = I_DBL.RunParam_SQL(strSqlJJR, "dtj", param);
        DataTable dataTableJJR = ((DataSet)htjjr["return_ds"]).Tables["dtj"];

        if (dataTableDQJJRZT != null && dataTableDQJJRZT.Rows.Count >= 0 && dataTableGLGJJR != null)
        {
            if (dataTableDQJJRZT.Rows[0]["经纪人_冻结状态"].ToString() == "否" && dataTableDQJJRZT.Rows[0]["经纪人_休眠状态"].ToString() == "否")//判断当前关联经纪人的中表状态
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "您当前关联的经纪人账户状态正常，不能选择其它经纪人。";

                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;

            }
            if (dataTableGLGJJR.Rows.Count > 0)
            {
                if (dataTableGLGJJR.Rows[0]["经纪人审核状态"].ToString() == "审核通过")//获取当前卖家家关联经纪人的审核问题
                {
                    d.Rows[0]["执行结果"] = "err";
                    d.Rows[0]["提示文本"] = "您已被关联过的经纪人审核通过，不能选择其它经纪人。";
                    dsReturn.Clear();
                    dsReturn.Tables.Add(d);
                    return dsReturn;
                }
            }
            if (dataFZCFXZJJR.Rows.Count > 0)
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "您已经关联过此经纪人，不能再选择此经纪人。";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;

            }

            List<string> listStrArray = new List<string>();
            if (dataTableJJR.Rows.Count > 0)
            {
                if (dataTableJJR.Rows[0]["是否冻结"].ToString() == "是" || !dataTableJJR.Rows[0]["是否冻结"].ToString().ToString().Equals(""))
                {
                    listStrArray.Add("冻结状态");
                }
                if (dataTableJJR.Rows[0]["是否休眠"].ToString() == "是")
                {
                    listStrArray.Add("休眠状态");
                }
                if (dataTableJJR.Rows[0]["是否允许登录"].ToString() == "否")
                {
                    listStrArray.Add("不允许登录状态");
                }
                if (dataTableJJR.Rows[0]["经纪人是否暂停新用户审核"].ToString() == "是")
                {
                    listStrArray.Add("暂停新用户审核状态");
                    //d.Rows[0]["提示文本"] = "您选择的经纪人当前状态为：暂停新用户审核状态，请重新选择！";
                }
                if (listStrArray.Count > 0)
                {
                    d.Rows[0]["执行结果"] = "err";
                    d.Rows[0]["提示文本"] = "您选择的经纪人当前状态为：" + string.Join("|", listStrArray.ToArray()).ToString() + "，请重新选择！";
                    dsReturn.Clear();
                    dsReturn.Tables.Add(d);
                    return dsReturn;
                }
            }

            string strExecSql = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR,CreateTime,CheckLimitTime)values (@strNumber ,@MMJDLYX,@JSZHLX, @strGLJJRDLYX,@strGLJJRJSBH, @strGLJJRYHM ,'否','" + DateTime.Now.ToString() + "','审核中','审核通过','否','是','否','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";

            Hashtable ht = I_DBL.RunParam_SQL(strExecSql, param);
            if ((bool)(ht["return_float"]))
            {
                d.Rows[0]["执行结果"] = "ok";
                d.Rows[0]["提示文本"] = "您已成功关联此经纪人！";

            }
            else
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "关联此经纪人失败！";

            }
            dsReturn.Clear();
            dsReturn.Tables.Add(d);
            return dsReturn;
        }
        else
        {
            d.Rows[0]["执行结果"] = "err";
            d.Rows[0]["提示文本"] = "获取经纪人数据信息失败！";
            dsReturn.Clear();
            dsReturn.Tables.Add(d);
            return dsReturn;

        }

    }

    /// <summary>
    /// 暂停或恢复新用户审核，仅用于经纪人交易账户的时候
    /// </summary>
    /// <param name="dataTable">参数数据表</param>
    /// <returns>一个包含执行结果和提示文本的数据集</returns>
    public static DataSet ZTHFSH(DataTable dataTable)
    {
        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" });

        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@JSBH", dataTable.Rows[0]["经纪人角色编号"].ToString());
        param.Add("@DLYX", dataTable.Rows[0]["登录邮箱"].ToString());
        param.Add("@JSZHLX", dataTable.Rows[0]["结算账户类型"].ToString());
        param.Add("@CZLX", dataTable.Rows[0]["操作类型"].ToString());

        Hashtable htb = I_DBL.RunParam_SQL("select J_JRZTXYHBZ from AAA_DLZHXXB  where B_DLYX=@DLYX  and B_JSZHLX=@JSZHLX ", "dtb", param);
        DataTable dtb = ((DataSet)htb["return_ds"]).Tables["dtb"];

        string strSourceValue = "";//原始值
        if (dtb != null && dtb.Rows.Count > 0)
        {
            if (dtb.Rows[0]["J_JRZTXYHBZ"] != null)
            {
                strSourceValue = dtb.Rows[0]["J_JRZTXYHBZ"].ToString();
            }
        }

        ArrayList arryList = new ArrayList();
        string strSql = "";

        if (dataTable.Rows[0]["操作类型"].ToString() == "暂停")
        {
            strSql = "update AAA_DLZHXXB set J_JJRSFZTXYHSH='是',J_JJRZTXYHSHSJ='" + DateTime.Now.ToString() + "' where B_DLYX=@DLYX and B_JSZHLX=@JSZHLX ";
            arryList.Add(strSql);
            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：暂停新用户审核" + "◆";
            string strCZValue = "update AAA_DLZHXXB set J_JRZTXYHBZ='" + strCZ + "' where  B_DLYX=@DLYX and B_JSZHLX=@JSZHLX ";
            arryList.Add(strCZValue);
        }
        else
        {
            strSql = "update AAA_DLZHXXB set J_JJRSFZTXYHSH='否',J_JJRZTXYHSHSJ=null where B_DLYX=@DLYX and B_JSZHLX=@JSZHLX ";
            arryList.Add(strSql);
            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：恢复新用户审核" + "◆";
            string strCZValue = "update AAA_DLZHXXB set J_JRZTXYHBZ='" + strCZ + "' where  B_DLYX=@DLYX  and B_JSZHLX=@JSZHLX ";
            arryList.Add(strCZValue);
        }

        Hashtable ht = I_DBL.RunParam_SQL(arryList, param);
        if ((bool)(ht["return_float"]))
        {
            if (dataTable.Rows[0]["操作类型"].ToString() == "暂停")
            {
                d.Rows[0]["执行结果"] = "ok";
                d.Rows[0]["提示文本"] = "您已成功执行暂停新交易方审核操作！";
            }
            else
            {
                d.Rows[0]["执行结果"] = "ok";
                d.Rows[0]["提示文本"] = "您已成功执行恢复新交易方审核操作！";
            }
        }
        else
        {
            d.Rows[0]["执行结果"] = "err";
            d.Rows[0]["提示文本"] = "当前操作失败！";

        }

        dsReturn.Clear();
        dsReturn.Tables.Add(d);
        return dsReturn;
    }
    
    /// <summary>
    /// 暂停恢复用户的新业务  仅用于买卖家交易账户
    /// </summary>
    /// <param name="dataTable">参数数据表</param>
    /// <returns>一个包含执行结果和提示文本的数据集</returns>
    public static DataSet ZTHFYW(DataTable dataTable)
    {
        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" });

        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@JSBH", dataTable.Rows[0]["经纪人角色编号"].ToString());
        //param.Add("@DLYX", dataTable.Rows[0]["登录邮箱"].ToString());
        //param.Add("@JSZHLX", dataTable.Rows[0]["结算账户类型"].ToString());
        param.Add("@CZLX", dataTable.Rows[0]["操作类型"].ToString());
        param.Add("@MMJDLYX",dataTable.Rows[0]["买卖家登录邮箱"].ToString());
        param.Add("@strLY", dataTable.Rows[0]["理由"].ToString());

        string strSql = "";

        Hashtable htc = I_DBL.RunParam_SQL("select SFZTYHXYW '是否暂停用户新业务' from AAA_MJMJJYZHYJJRZHGLB where DLYX=@MMJDLYX  and GLJJRBH=@JSBH  and SFYX='是' and SFDQMRJJR='是' and JSZHLX='买家卖家交易账户' and JJRSHZT='审核通过' and FGSSHZT='审核通过'", "dtc", param);
        DataTable dataTableInfor = ((DataSet)htc["return_ds"]).Tables["dtc"];

        if (dataTableInfor!=null && dataTableInfor.Rows.Count > 0)
        {

            if (dataTableInfor.Rows[0]["是否暂停用户新业务"].ToString() == "是" && dataTable.Rows[0]["操作类型"].ToString() == "暂停")
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "该用户的新业务处于暂停状态，请不要再执行暂停操作！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }
            else if (dataTableInfor.Rows[0]["是否暂停用户新业务"].ToString() == "否" && dataTable.Rows[0]["操作类型"].ToString() == "恢复")
            {
                d.Rows[0]["执行结果"] = "err";
                d.Rows[0]["提示文本"] = "该用户的新业务处于恢复状态，请不要再执行恢复操作！";
                dsReturn.Clear();
                dsReturn.Tables.Add(d);
                return dsReturn;
            }
        }

        Hashtable htb = I_DBL.RunParam_SQL("select ZTYHXYWBZ from AAA_MJMJJYZHYJJRZHGLB where DLYX= @MMJDLYX  and GLJJRBH=@JSBH  and SFYX='是' and SFDQMRJJR='是' ", "dtb", param);
        DataTable dtb = ((DataSet)htb["return_ds"]).Tables["dtb"];
       
        string strSourceValue = "";//原始值
        if (dtb!= null && dtb.Rows.Count>0)
        {
            if (dtb.Rows[0]["ZTYHXYWBZ"] != null)
                strSourceValue = dtb.Rows[0]["ZTYHXYWBZ"].ToString();
        }
      
        ArrayList arrayList = new ArrayList();
        if (dataTable.Rows[0]["操作类型"].ToString() == "暂停")
        {
            strSql = "update  AAA_MJMJJYZHYJJRZHGLB set  SFZTYHXYW='是', ZTXYWSJ='" + DateTime.Now.ToString() + "'   where DLYX=@MMJDLYX  and GLJJRBH=@JSBH and SFYX='是' and SFDQMRJJR='是'  and JJRSHZT='审核通过' and FGSSHZT='审核通过'";
            arrayList.Add(strSql);
            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：暂停该用户新业务 操作理由：@strLY ◆";
            string strCZValue = "update AAA_MJMJJYZHYJJRZHGLB set ZTYHXYWBZ='" + strCZ + "' where DLYX=@MMJDLYX  and GLJJRBH=@JSBH and SFYX='是' and SFDQMRJJR='是' ";
            arrayList.Add(strCZValue);
        }
        else
        {
            strSql = "update  AAA_MJMJJYZHYJJRZHGLB set  SFZTYHXYW='否',ZTXYWSJ=null   where DLYX= @MMJDLYX  and GLJJRBH=@JSBH  and SFYX='是' and SFDQMRJJR='是'  and JJRSHZT='审核通过' and FGSSHZT='审核通过'";
            arrayList.Add(strSql);
            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：恢复该用户新业务  操作理由： @strLY ◆";
            string strCZValue = "update AAA_MJMJJYZHYJJRZHGLB set ZTYHXYWBZ='" + strCZ + "' where DLYX=@MMJDLYX  and GLJJRBH=@JSBH  and SFYX='是' and SFDQMRJJR='是' ";
            arrayList.Add(strCZValue);
        }

        Hashtable ht = I_DBL.RunParam_SQL(arrayList, param);
        if ((bool)(ht["return_float"]))
        {
            if (dataTable.Rows[0]["操作类型"].ToString() == "暂停")
            {
                d.Rows[0]["执行结果"] = "ok";
                d.Rows[0]["提示文本"] = "您已成功暂停该用户的新业务！";

            }
            else
            {
                d.Rows[0]["执行结果"] = "ok";
                d.Rows[0]["提示文本"] = "您已成功恢复该用户的新业务！";
            }

        }
        else
        {
            d.Rows[0]["执行结果"] = "err";
            d.Rows[0]["提示文本"] = "当前操作失败！";

        }

         dsReturn.Clear();
         dsReturn.Tables.Add(d);
         return dsReturn;
    }

    /// <summary>
    /// 获取余额，冻结金额以及经纪人收益
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="type">类型：经纪人</param>
    /// <returns></returns>
    public static Hashtable GetTongJi(string dlyx, string type)
    {               

        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@dlyx",dlyx);
                
        Hashtable htresult = new Hashtable();
        htresult["执行结果"] = "ok";
        htresult["提示文本"] = "";
        htresult["当前冻结"] = "0.00";
        htresult["当前余额"] = "0.00";
        htresult["当前收益"] = "0.00";
        
        //获取当前冻结余额
        try
        {
            string sql_dj = "select isnull(sum(case yslx when '冻结' then je when '解冻' then -je end),0.00) as 当前冻结 from AAA_ZKLSMXB where dlyx=@dlyx  and yslx in ('冻结','解冻') ";


            Hashtable htb = I_DBL.RunParam_SQL(sql_dj, "dtb", param);
            DataTable dtb = ((DataSet)htb["return_ds"]).Tables["dtb"];

            if (dtb!= null && dtb.Rows.Count>0)
            {
                htresult["当前冻结"] = dtb.Rows[0]["当前冻结"].ToString();
            }
        }
        catch
        {
            htresult["执行结果"] = "err";
            htresult["提示文本"] = "未获取到当前冻结金额！";
        }
        //获取当前可用资金余额
        try
        {
            string strye=" select B_DLYX as 登录邮箱, B_ZHDQKYYE as 账户当前可用余额, B_DSFCGKYYE as 第三方存管可用余额 from dbo.AAA_DLZHXXB where B_DLYX = @dlyx"  ;           
            
            Hashtable hty = I_DBL.RunParam_SQL(strye, "dty", param);
            DataTable dty = ((DataSet)hty["return_ds"]).Tables["dty"];

            if (dty!= null && dty.Rows.Count>0)
            {
                htresult["当前余额"] = dty.Rows[0]["账户当前可用余额"].ToString();
            }
           
        }
        catch
        {
            htresult["执行结果"] = "err";
            htresult["提示文本"] = htresult["提示文本"] + " 未获取到当前可用资金余额！";
        }

        //获取当前经纪人收益金额
        if (type.IndexOf("经纪人") >= 0)
        {
            try
            {
                string sql_sy = "select isnull(sum(case yslx when '增加' then je when '扣减' then -je end),0.00) as 当前收益 from AAA_ZKLSMXB where dlyx=@dlyx  and yslx in ('增加','扣减') and sjlx='实' and xm='经纪人收益'  ";

                Hashtable htsy = I_DBL.RunParam_SQL(sql_sy , "dtsy", param);
                DataTable dtsy = ((DataSet)htsy["return_ds"]).Tables["dtsy"];
                if (dtsy != null && dtsy.Rows.Count > 0)
                {
                    htresult["当前收益"] = dtsy.Rows[0]["当前收益"].ToString();
                }                
                
            }
            catch
            {
                htresult["执行结果"] = "err";
                htresult["提示文本"] = htresult["提示文本"] + " 未获取经纪人可支取收益！";
            }
        }
        return htresult;
    }
    /// <summary>
    /// 根据Number获取登陆表相应交易方名称、资料提交时间
    /// </summary>
    /// <param name="number">登录表主键number</param>
    /// <returns></returns>
    public static DataSet GetJJFMCByNumber(string number)
    {
        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        dsReturn.Tables.Add(d);
        dsReturn.Tables[0].Rows.Add(new string[] { "err", "初始化" });
        try
        {
            if (string.IsNullOrEmpty(number))
            {
                dsReturn.Tables[0].Rows[0]["执行结果"] = "err";
                dsReturn.Tables[0].Rows[0]["提示文本"] = "参数不能为空！";
                return dsReturn;
            }

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            string str = "select I_JYFMC '交易方名称',convert(varchar(10),I_ZLTJSJ,120) '资料提交时间' from dbo.AAA_DLZHXXB where Number=@number";
            Hashtable htcs = new Hashtable();
            htcs["@number"] = number;
            Hashtable returnHT = I_DBL.RunParam_SQL(str, "", htcs);
            if ((bool)returnHT["return_float"])
            {
                if (returnHT["return_ds"] != null && ((DataSet)returnHT["return_ds"]).Tables.Count > 0)
                {
                    DataTable dataTable = ((DataSet)returnHT["return_ds"]).Tables[0].Copy();
                    dataTable.TableName = "用户信息";
                    dsReturn.Tables.Add(dataTable);
                    dsReturn.Tables[0].Rows[0]["执行结果"] = "ok";
                    dsReturn.Tables[0].Rows[0]["提示文本"] = "获取数据成功！";                   
                }
                else
                {
                    dsReturn.Tables[0].Rows[0]["执行结果"] = "err";
                    dsReturn.Tables[0].Rows[0]["提示文本"] = "获取数据失败！";
                }
            }
            else
            {
                dsReturn.Tables[0].Rows[0]["执行结果"] = "err";
                dsReturn.Tables[0].Rows[0]["提示文本"] = (string)returnHT["return_errmsg"];
            }
           
        }
        catch(Exception ex)
        {
            dsReturn.Tables[0].Rows[0]["执行结果"] = "err";
            dsReturn.Tables[0].Rows[0]["提示文本"] = ex.ToString();
        }
        return dsReturn;
    }

    /// <summary>
    ///  用于经纪人审核买卖家是获取买卖家信息
    /// </summary>
    /// <param name="dataTable">参数数据表</param>
    /// <returns></returns>
    public static DataSet GetMMJData(DataTable dataTable)
    {
        //链接
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@jsbh", dataTable.Rows[0]["JSBH"].ToString());//卖家角色编号
        param.Add("@num", dataTable.Rows[0]["关联表Number"].ToString());
        param.Add("@jjrjsbh", dataTable.Rows[0]["GLJJRBH"].ToString());//经纪人角色编号

        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        dsReturn.Tables.Add(d);
        dsReturn.Tables[0].Rows.Add(new string[] { "err", "初始化" });

        #region//zhouli 作废 2014.08.06 变更新的SQL，去掉where语句中a.SFYX='是'条件，增加查询字段 a.SFYX，原因：验证是否变更经纪人
        //string strSql = "select b.Number '登录_Number', B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,a.Number '关联_Number',GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',(case JJRSHZT when  '审核中' then '--' else CONVERT(varchar(10),ZHXGSJ,120) end) '驳回修改时间'   from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and b.J_SELJSBH=@jsbh and a.Number=@num ";
        #endregion

        string strSql = "select  a.SFYX,b.Number '登录_Number', B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,a.Number '关联_Number',GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',(case JJRSHZT when  '审核中' then '--' else CONVERT(varchar(10),ZHXGSJ,120) end) '驳回修改时间'   from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH=@jsbh and a.Number=@num ";

        Hashtable htone = I_DBL.RunParam_SQL(strSql, "dataTableONE", param);
        DataTable dataTableONE = ((DataSet)htone["return_ds"]).Tables["dataTableONE"];

        string strSqlTwo = "select J_JJRZGZSBH,JJRZGZS,I_JYFMC,I_JYFLXDH from AAA_DLZHXXB  where J_JJRJSBH=@jjrjsbh ";

        Hashtable httwo = I_DBL.RunParam_SQL(strSqlTwo, "dataTableTWO", param);
        DataTable dataTableTWO = ((DataSet)httwo["return_ds"]).Tables["dataTableTWO"];
      
        if (dataTableONE != null && dataTableONE.Rows.Count > 0 && dataTableTWO != null && dataTableTWO.Rows.Count > 0)
        {
            #region//zhouli 2014.08.06 add 验证是否变更经纪人
            if (dataTableONE.Rows[0]["SFYX"].ToString().Trim() != "是")
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前交易账户已更换经纪人，无需审核！";
                return dsReturn;
            }
            #endregion

            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "买卖家的数据信息获取成功！";
            DataTable dataTableCopy = dataTableONE.Copy();
            dsReturn.Tables.Add(dataTableCopy);
            dsReturn.Tables[1].TableName = "买家卖家交易账户基本信息";
            DataTable dataTableTWOCopy = dataTableTWO.Copy();
            dsReturn.Tables.Add(dataTableTWOCopy);
            dsReturn.Tables[2].TableName = "买家卖家关联经纪人账户基本信息";
        }
        else
        {
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "买卖家的数据信息获取失败！";
        }
        return dsReturn;
    }


    /// <summary>
    /// 获取更换经纪人时获取的经纪人交易账户数据信息
    /// </summary>
    /// <param name="dataTable">参数列表</param>
    /// <returns>结果数据集</returns>
    public static DataSet GetSiftJJRData( DataTable dataTable)
    {
        DataSet dsreturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        dsreturn.Tables.Add(d);
        dsreturn.Tables[0].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();

        param.Add("@JSBH", dataTable.Rows[0]["卖家角色编号"].ToString());//卖家角色编号
        param.Add("@MMJDLYX", dataTable.Rows[0]["买卖家登录邮箱"].ToString());
        param.Add("@JSZHLX", dataTable.Rows[0]["结算账户类型"].ToString());
             
        string strWhere = dataTable.Rows[0]["搜索经纪人资格证书编号"].ToString();//搜所经济人角色编号内容
     

        //查询当前经纪人的状态信息
        string strSqlDQJJRZT = "select c.I_PTGLJG '经纪人_平台管理机构',c.B_SFDJ '经纪人_冻结状态',c.B_SFXM '经纪人_休眠状态',c.I_JYFMC '经纪人交易方名称',b.I_SSQYS '买卖家_所属区域省',b.I_SSQYSHI '买卖家_所属区域市',b.I_SSQYQ '买卖家_所属区域区'  from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH   where a.JSZHLX=@JSZHLX and a.SFDQMRJJR='是'  and a.SFYX='是' and a.JJRSHZT='审核通过' and a.FGSSHZT='审核通过' and a.DLYX=@MMJDLYX ";

        Hashtable htdqjjr = I_DBL.RunParam_SQL(strSqlDQJJRZT, "dataTableDQJJRZT", param);
        DataTable dataTableDQJJRZT = ((DataSet)htdqjjr["return_ds"]).Tables["dataTableDQJJRZT"];
               
        DataTable dataTableXZJJR = null;
        if (dataTableDQJJRZT != null && dataTableDQJJRZT.Rows.Count > 0)
        {
            string strSqlAll = "";

            param.Add("@PTGLJG", dataTableDQJJRZT.Rows[0]["经纪人_平台管理机构"].ToString());
            param.Add("@strwhere",strWhere);

            //选择与当前关联经纪人所属分公司相同的经纪人
            if (strWhere != "")
            {

                strSqlAll = "select  * from ( select a.DLYX '经纪人登录邮箱',b.J_JJRZGZSBH '经纪人资格证书编号',b.I_JYFMC '经纪人名称',b.J_JJRJSBH '经纪人角色编号',b.B_YHM '经纪人用户名','是' '是否当前分公司下经纪人','是' '是否阶段一筛选结果', b.I_PTGLJG '平台管理机构',b.I_SSQYS '经纪人_所属区域省',b.I_SSQYSHI '经纪人_所属区域市',b.I_SSQYQ '经纪人_所属区域区',b.I_SSQYS+b.I_SSQYSHI  '经纪人_所属区域省市',a.FGSSHSJ '分公司审核时间',a.FGSSHZT '分公司审核状态',b.B_SFDJ '是否冻结', b.B_SFXM '是否休眠',b.B_SFYXDL '是否允许登录',b.J_JJRSFZTXYHSH '经纪人是否暂停新用户审核' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where a.JSZHLX='经纪人交易账户' and a.JJRSHZT='审核通过' and a.FGSSHZT='审核通过' and a.SFYX='是' and b.J_JJRZGZSBH=@strwhere  ) as tab";
            }
            else
            {
                strSqlAll = "select top 15 * from ( select a.DLYX '经纪人登录邮箱',b.J_JJRZGZSBH '经纪人资格证书编号',b.I_JYFMC '经纪人名称',b.J_JJRJSBH '经纪人角色编号',b.B_YHM '经纪人用户名','是' '是否当前分公司下经纪人','是' '是否阶段一筛选结果', b.I_PTGLJG '平台管理机构',b.I_SSQYS '经纪人_所属区域省',b.I_SSQYSHI '经纪人_所属区域市',b.I_SSQYQ '经纪人_所属区域区',b.I_SSQYS+b.I_SSQYSHI  '经纪人_所属区域省市',a.FGSSHSJ '分公司审核时间' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where a.JSZHLX='经纪人交易账户' and a.JJRSHZT='审核通过' and a.FGSSHZT='审核通过' and b.I_PTGLJG=@PTGLJG and b.B_SFDJ='否' and b.B_SFXM='否' and b.B_SFYXDL='是' and b.J_JJRSFZTXYHSH='否'   ) as tab  order by case 经纪人_所属区域省市 when '" + dataTableDQJJRZT.Rows[0]["买卖家_所属区域省"].ToString() + dataTableDQJJRZT.Rows[0]["买卖家_所属区域市"].ToString() + "' then 1  else 3 end asc,case 经纪人_所属区域省 when '" + dataTableDQJJRZT.Rows[0]["买卖家_所属区域省"].ToString() + "' then 1  else 3 end asc,分公司审核时间 asc";
            }

            Hashtable htXZJJR = I_DBL.RunParam_SQL(strSqlAll, "dataTableXZJJR", param);
            dataTableXZJJR = ((DataSet)htXZJJR["return_ds"]).Tables["dataTableXZJJR"];
                                  
        }

        string strgljjr = "select a.DLYX '买卖家登录邮箱',c.J_JJRZGZSBH '经纪人资格证书编号',c.I_JYFMC '经纪人交易方名称',c.B_DLYX '经纪人登录邮箱',c.B_SFDJ '经纪人_冻结状态',c.B_SFXM '经纪人_休眠状态',a.JJRSHZT '经纪人审核状态',a.JJRSHSJ '经纪人审核时间',a.JJRSHYJ '经纪人审核意见',a.SQSJ '申请时间' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH where a.JSZHLX='买家卖家交易账户' and a.SFYX='是' and  a.DLYX=@MMJDLYX and a.SFSCGLJJR='否' and a.JJRSHZT='审核通过' and a.SFDQMRJJR='是' order by a.SQSJ desc";

        Hashtable htl = I_DBL.RunParam_SQL(strgljjr, "dataTableGLGJJR", param);
        DataTable dataTableGLGJJR = ((DataSet)htl["return_ds"]).Tables["dataTableGLGJJR"];
        
        if (strWhere != "" && dataTableDQJJRZT != null && dataTableDQJJRZT.Rows.Count >= 0 && dataTableGLGJJR != null)//点击搜索按钮时的信息处理
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okErr";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "";
           
            if (dataTableXZJJR.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请填写正确的经纪人资格证书编号！";
            }
            else if (!dataTableXZJJR.Rows[0]["平台管理机构"].ToString().Equals(dataTableDQJJRZT.Rows[0]["经纪人_平台管理机构"].ToString()))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请选择与您同一个业务管理部门的经纪人！";
            }
            else
            {

                List<string> listStrArray = new List<string>();

                if (dataTableXZJJR.Rows[0]["是否冻结"].ToString() == "是")
                {
                    listStrArray.Add("冻结");
                }
                if (dataTableXZJJR.Rows[0]["是否休眠"].ToString() == "是")
                {
                    listStrArray.Add("休眠");
                }
                if (dataTableXZJJR.Rows[0]["是否允许登录"].ToString() == "否")
                {
                    listStrArray.Add("不允许登录");
                }
                if (dataTableXZJJR.Rows[0]["经纪人是否暂停新用户审核"].ToString() == "是")
                {
                    listStrArray.Add("暂停新用户审核");
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您选择的经纪人当前状态为：暂停新用户审核，请重新选择！";
                }
                if (listStrArray.Count > 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您选择的经纪人当前状态为：" + string.Join("|", listStrArray.ToArray()).ToString() + "，请重新选择！";
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功获取可选经纪人数据信息！";
                }
            }
            
            DataTable dataTableDQJJRZTCopy = dataTableDQJJRZT.Copy();
            dsreturn.Tables.Add(dataTableDQJJRZTCopy);
            dsreturn.Tables[1].TableName = "买卖家关联的当前经纪人信息";
            DataTable dataTableXZJJRCopy = dataTableXZJJR.Copy();
            dsreturn.Tables.Add(dataTableXZJJRCopy);
            dsreturn.Tables[2].TableName = "买卖家可选经纪人信息";
            DataTable dataTableGLGJJRCopy = dataTableGLGJJR.Copy();
            dsreturn.Tables.Add(dataTableGLGJJRCopy);
            dsreturn.Tables[3].TableName = "买卖家关联过的经纪人信息";

        }
        else if (dataTableDQJJRZT != null && dataTableDQJJRZT.Rows.Count >= 0 && dataTableXZJJR != null && dataTableGLGJJR != null)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功获取可选经纪人数据信息！";
            DataTable dataTableDQJJRZTCopy = dataTableDQJJRZT.Copy();
            dsreturn.Tables.Add(dataTableDQJJRZTCopy);
            dsreturn.Tables[1].TableName = "买卖家关联的当前经纪人信息";
            DataTable dataTableXZJJRCopy = dataTableXZJJR.Copy();
            dsreturn.Tables.Add(dataTableXZJJRCopy);
            dsreturn.Tables[2].TableName = "买卖家可选经纪人信息";
            DataTable dataTableGLGJJRCopy = dataTableGLGJJR.Copy();
            dsreturn.Tables.Add(dataTableGLGJJRCopy);
            dsreturn.Tables[3].TableName = "买卖家关联过的经纪人信息";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取可选经纪人数据信息失败！";

        }
        return dsreturn;
    }

    /// <summary>
    ///提交银行人员信息表 wyh 2014.07.12 
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public static DataSet SubmitYHYHInfor(DataTable dataTable)
    {
        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();
        string YGGH = dataTable.Rows[0]["员工工号"].ToString();
        string BankDLYX = dataTable.Rows[0]["银行登录账号"].ToString();


        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();
        param.Add("@YHDLZH", BankDLYX.Trim());
        param.Add("@YGGH", YGGH.Trim());
        #region//判断同一银行登录账号下是否已存在此员工工号
        string strYGGH = "select * from AAA_YHRYXXB where YHDLZH =@YHDLZH and YGGH=@YGGH";
        Hashtable ht_return = I_DBL.RunParam_SQL(strYGGH, "is", param);
        DataTable dataTableRepeat = null;
        if ((bool)(ht_return["return_float"]))
        {
            DataSet dsre = (DataSet)ht_return["return_ds"];
            if (dsre != null)
            {
                dataTableRepeat = dsre.Tables[0];
            }
        }

        if (dataTableRepeat != null && dataTableRepeat.Rows.Count > 0)
        {
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "员工工号" + YGGH + "已经被占用，请修改员工工号后重新提交！";
            return dsReturn;
        }
        #endregion
        string strNumber = "";
        object[] rezkls = IPC.Call("获取主键", new object[] { "AAA_YHRYXXB", "" });

        if (rezkls[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (rezkls[1] != null && !rezkls[1].ToString().Trim().Equals(""))
            {
                strNumber = (string)(rezkls[1]);
            }

        }

        if (strNumber.Equals(""))
        {
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "主键获取出错！";
            return dsReturn;
        }

        Hashtable htpars = new Hashtable();
        htpars.Add("@Number", strNumber);
        htpars.Add("@YHDLZH", dataTable.Rows[0]["银行登录账号"].ToString());
        htpars.Add("@YGLSJG", dataTable.Rows[0]["员工隶属机构"].ToString());
        htpars.Add("@YGXM", dataTable.Rows[0]["员工姓名"].ToString());
        htpars.Add("@YGGH", dataTable.Rows[0]["员工工号"].ToString());
        htpars.Add("@LXFS", dataTable.Rows[0]["联系方式"].ToString());
        htpars.Add("@LXDZ", dataTable.Rows[0]["联系地址"].ToString());
        htpars.Add("@SFYX", "是");
        htpars.Add("@TJSJ", DateTime.Now.ToString());

        //插入数据
        string strSql = "insert AAA_YHRYXXB(Number,YHDLZH,YGLSJG,YGXM,YGGH,LXFS,LXDZ,SFYX,TJSJ) values(@Number,@@YHDLZH,@YGLSJG,@YGXM,@YGGH,@LXFS,@LXDZ,@SFYX,@TJSJ) ";

        ht_return = I_DBL.RunParam_SQL(strSql, htpars);
        if ((bool)(ht_return["return_float"]))
        {
            int dsre = (int)ht_return["return_other"];
            if (dsre > 0)
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息提交成功！";
            }
            else
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息提交失败！";
            }
        }
        else
        {
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息提交失败！";
        }


        return dsReturn;

    }
        
    /// <summary>
    ///修改银行人员信息表
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public static DataSet ModifyYHYHInfor(DataTable dataTable)
    {
        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();
        string YGGH = dataTable.Rows[0]["员工工号"].ToString();
        string DLYX = dataTable.Rows[0]["银行登录账号"].ToString();

        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        d.Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable htpars = new Hashtable();


        htpars.Add("@Number", dataTable.Rows[0]["银行人员表Number"].ToString());
        htpars.Add("@YHDLZH", dataTable.Rows[0]["银行登录账号"].ToString());
        htpars.Add("@YGLSJG", dataTable.Rows[0]["员工隶属机构"].ToString());
        htpars.Add("@YGXM", dataTable.Rows[0]["员工姓名"].ToString());

        htpars.Add("@LXFS", dataTable.Rows[0]["联系方式"].ToString());
        htpars.Add("@LXDZ", dataTable.Rows[0]["联系地址"].ToString());
        htpars.Add("@SFYX", "是");
        htpars.Add("@TJSJ", DateTime.Now.ToString());
        //插入数据
        string strSql = "update AAA_YHRYXXB set YHDLZH=@YHDLZH,YGLSJG=@YGLSJG,YGXM=@YGXM,LXFS=@LXFS,LXDZ=@LXDZ where  Number=@Number";


        Hashtable ht_return = I_DBL.RunParam_SQL(strSql, htpars);
        if ((bool)(ht_return["return_float"]))
        {
            int dsre = (int)ht_return["return_other"];
            if (dsre > 0)
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息修改成功！";
            }
            else
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息修改失败！";
            }
        }
        else
        {
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息修改失败！";
        }

        return dsReturn;

    }
        
    /// <summary>
    /// 获取买卖家账户的基本信息
    /// </summary>
    /// <param name="jsbh">卖家角色编号</param>
    /// <returns></returns>
    public static DataSet MMJBasicData(string jsbh)
    {
        DataSet dsreturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        dsreturn.Tables.Add(d);
        dsreturn.Tables[0].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();        
        param.Add("@JSBH", jsbh);//卖家角色编号

        string strSql = "select b.Number '登录_Number', B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,a.Number '关联_Number',GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',I_YXBH,I_JJRFL,I_YWGLBMFL,I_GLYH,I_GLYHGZRYGH,I_YWFWBM  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and b.J_SELJSBH=@JSBH  and a.SFSCGLJJR='是'";
       
        Hashtable htone = I_DBL.RunParam_SQL(strSql, "dataTableONE", param);
        DataTable dataTableONE = ((DataSet)htone["return_ds"]).Tables["dataTableONE"];
      
        string strSqlTwo = "select a.Number,c.B_DLYX, c.J_JJRZGZSBH,c.JJRZGZS,c.I_JYFMC,c.I_JYFLXDH,c.J_JJRJSBH,c.I_YXBH,c.I_JJRFL,c.I_YWGLBMFL from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH   where b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH=@JSBH  and a.SFDQMRJJR='是'";

        Hashtable httwo = I_DBL.RunParam_SQL(strSqlTwo, "dataTableTWO", param);
        DataTable dataTableTWO = ((DataSet)httwo["return_ds"]).Tables["dataTableTWO"];

        if (dataTableONE != null && dataTableONE.Rows.Count > 0 && dataTableTWO != null && dataTableTWO.Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "买卖家的数据信息获取成功！";
            DataTable dataTableCopy = dataTableONE.Copy();
            dsreturn.Tables.Add(dataTableCopy);
            dsreturn.Tables[1].TableName = "买家卖家交易账户基本信息";
            DataTable dataTableTWOCopy = dataTableTWO.Copy();
            dsreturn.Tables.Add(dataTableTWOCopy);
            dsreturn.Tables[2].TableName = "买家卖家关联经纪人账户基本信息";
                       
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "买卖家的数据信息获取失败！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 获取经纪人账户基本信息
    /// </summary>
    /// <param name="jsbh">经纪人角色编号</param>
    /// <returns></returns>
    public static DataSet JJRBasicData(string jsbh)
    {
        DataSet dsreturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        dsreturn.Tables.Add(d);
        dsreturn.Tables[0].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();
        param.Add("@JSBH", jsbh);//经纪人角色编号

        string strSql = "select b.Number '登录_Number',B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,a.Number '关联_Number',GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),a.CreateTime,120) '资料提交时间',I_YXBH,I_JJRFL,I_YWGLBMFL   from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='经纪人交易账户' and a.SFYX='是' and b.J_JJRJSBH=@JSBH ";

        Hashtable htone = I_DBL.RunParam_SQL(strSql, "dataTableONE", param);
        DataTable dataTableONE = ((DataSet)htone["return_ds"]).Tables["dataTableONE"];

        if (dataTableONE != null && dataTableONE.Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人的数据信息获取成功！";
            DataTable dataTableCopy = dataTableONE.Copy();
            dsreturn.Tables.Add(dataTableCopy);
            dsreturn.Tables[1].TableName = "经纪人交易账户基本信息";
                        
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人的数据信息获取失败！";
        }
        return dsreturn;
    }

    /// <summary>
    ///删除银行人员信息表 2017.07.14
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public static DataSet DeleteYHYHInfor(DataTable dataTable)
    {

        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();
        string BankDLYX = dataTable.Rows[0]["银行登录账号"].ToString();
        string YGGH = dataTable.Rows[0]["员工工号"].ToString();


        DataSet dsReturn = new DataSet();
        //要返回的数据集中的数据表
        DataTable d = new DataTable();
        d.TableName = "返回值单条";
        d.Columns.Add("执行结果", typeof(string));
        d.Columns.Add("提示文本", typeof(string));
        dsReturn.Tables.Add(d);
        dsReturn.Tables[0].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();

        param.Add("@B_DLYX", dataTable.Rows[0]["银行登录账号"].ToString());
        param.Add("@Number", dataTable.Rows[0]["银行人员表Number"].ToString());
        #region//判断此员工是否有关联交易方
        string strSqlExits = "select * from dbo.AAA_DLZHXXB where I_GLYH = (select top 1 I_JYFMC from dbo.AAA_DLZHXXB where B_DLYX=@B_DLYX) and I_GLYHGZRYGH =(select YGGH from AAA_YHRYXXB where Number=@Number)";
        DataTable dataTableExits = null;//DbHelperSQL.Query(strSqlExits).Tables[0];
        Hashtable ht_return = I_DBL.RunParam_SQL(strSqlExits, "gl", param);
        if ((bool)(ht_return["return_float"]))
        {
            DataSet dsre = (DataSet)ht_return["return_ds"];
            if (dsre!=null&& dsre.Tables.Count>0)
            {
                dataTableExits = dsre.Tables["gl"];
            }
           
        }
        else
        {
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "判断此员工是否有关联交易方时出现异常不能删除！";
            return dsReturn;
        }

        if (dataTableExits != null && dataTableExits.Rows.Count > 0)
        {
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "已经有交易方关联此员工，不能删除！";
            return dsReturn;

        }
        #endregion

        #region//判断此员工是否已经产生业务

        Hashtable htye = new Hashtable();
        htye.Add("@GLJJRXSYGGH", YGGH.Trim());
        htye.Add("@GLJJRYX", BankDLYX.Trim());

        string strYW = "select fzb.GLJJRXSYGGH,ydd.GLJJRYX from AAA_TBDAndYDD_FZB fzb left join AAA_YDDXXB ydd on fzb.Number=ydd.Number where fzb.GLSJBM='预订单' and ydd.ZT!='撤销' and fzb.GLJJRXSYGGH=@GLJJRXSYGGH and ydd.GLJJRYX=@GLJJRYX 	union all select fzb.GLJJRXSYGGH,tbd.GLJJRYX from AAA_TBDAndYDD_FZB fzb left join AAA_TBD tbd on fzb.Number=tbd.Number where fzb.GLSJBM='投标单' and tbd.ZT!='撤销' and fzb.GLJJRXSYGGH=@GLJJRXSYGGH and tbd.GLJJRYX=@GLJJRYX";

        DataSet dsYW = null;//DbHelperSQL.Query(strYW);

        ht_return = I_DBL.RunParam_SQL(strYW, "yw", param);
        if ((bool)(ht_return["return_float"]))
        {
            dsYW = (DataSet)ht_return["return_ds"];
           
        }
        else
        {
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "判断此员工是否已经产生业务时出错！";
            return dsReturn;
        }

        if (dsYW != null && dsYW.Tables["yw"].Rows.Count > 0)
        {
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "已经有交易方关联此员工，不能删除！";
            return dsReturn;
        }
        #endregion

        //删除数据
        Hashtable htdele = new Hashtable();
        htdele.Add("@Number", dataTable.Rows[0]["银行人员表Number"].ToString());
        string strSql = "delete from AAA_YHRYXXB where Number=@Number";
        ht_return = I_DBL.RunParam_SQL(strSql, htdele);
        if ((bool)(ht_return["return_float"]))
        {
            int dsre = (int)ht_return["return_other"];
            if (dsre > 0)
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息删除成功！";
            }
            else
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息删除失败！";
            }
        }
        else
        {
            dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息删除失败！";
        }


        return dsReturn;

    }
    


}