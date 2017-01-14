using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Xml;
//using FMOP.DB;
using System.IO;
//using ShareLiu.DXS;  //短信发送空间 刘杰2010-07


namespace Hesion.Brick.Core.WorkFlow
{



    /// <summary>
    /// 提醒类
    /// </summary>
    public class Warning
    {

        /// <summary>
        /// 提醒数据集
        /// </summary>
        private DataSet RoleWarning;

        /// <summary>
        /// 工作流模块基本属性
        /// </summary>
        private Property property;

        private XmlNode WarningNode;
        #region
        /// <summary>
        /// 通过工作流模块基本属性和Xml节点构造本类
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="Node"></param>
        public Warning(Property pro, XmlNode Node)
        {
            if (pro != null)
            {
                property = pro;
                DataSet ds = new DataSet();
                if (Node != null)
                {
                    string nodetext = "<Warning>" + Node.InnerXml + "</Warning>";

                    StringReader sr = new StringReader(nodetext);
                    XmlTextReader xtr = new XmlTextReader(sr);
                    ds.ReadXml(xtr);
                    RoleWarning = ds;
                    WarningNode = Node;
                }
            }
        }
        #endregion
        #region//2014.05.22--周丽作废--用不到，暂时注释掉，如果用到再释放并修改数据库访问

        //#region
        ///// <summary>
        ///// 当单据状态改变时，向相关岗位发送模块配置文件中对应类型的提醒信息
        ///// </summary>
        ///// <param name="Number">单号</param>
        ///// <param name="Type">事件类型 1:单据产生后 2:单据修改后 3:单据删除后 4:单据审核通过后</param>
        ///// <param name="UserNumber">信息产生者的员工编号</param>
        ///// <returns>1:成功 0:失败</returns>
        //public short CreateWarningToRole(string Number, int Type, string UserNumber)
        //{
            
        //    //不是工作流模块，不允许调用该方法
        //    if (property == null || RoleWarning == null || RoleWarning.Tables.Count <= 0 )
        //        return 0;
        //    int ModuleType = property.ModuleType;
        //    string Module_Url = null;
        //    string name = property.ModuleName;
        //    string page = "WorkFlow_View.aspx?module=";
        //    string filenName = "&number=";
        //    //拼接连接地址字符串
        //    Module_Url = page + name + filenName + Number;
        //    return CreateWarningToRole(Number, Type, UserNumber, Module_Url);
        //}
        ///// <summary>
        ///// 当单据状态改变时，向相关岗位发送模块配置文件中对应类型的提醒信息，主要适用于自定义模块。
        ///// </summary>
        ///// <param name="Number">单号</param>
        ///// <param name="Type">事件类型 1:单据产生后 2:单据修改后 3:单据删除后 4:单据审核通过后</param>
        ///// <param name="UserNumber">发送人的工号 主要指操作单子的用户</param>
        ///// <param name="ViewUrl">查看该单据的地址</param>
        ///// <returns>1:成功 0:失败</returns>
        //public short CreateWarningToRole(string Number, int Type, string UserNumber, string ViewUrl)
        //{
        //    //如果模块不存在，则直接返回0
        //    if (property == null || RoleWarning == null || RoleWarning.Tables.Count <= 0)
        //        return 0;
        //    int ModuleType = property.ModuleType;
        //    string name = property.ModuleName;
        //    string title = property.Title;
        //    DataRow[] row = RoleWarning.Tables[0].Select("warningType='" + Type + "'");
        //    if (row.Length == 0)
        //        return 0;
        //    try
        //    {
        //        SendWarningsToUsers(Number, UserNumber, title, ViewUrl, row);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error(e.Message);
        //        return 0;
        //    }
        //    return 1;

        //}

        ///// <summary>
        ///// 发送一组提醒
        ///// </summary>
        ///// <param name="Number">单号</param>
        ///// <param name="UserNumber">信息产生者编号</param>
        ///// <param name="title">模块名称</param>
        ///// <param name="Module_Url">查看地址</param>
        ///// <param name="row">提醒信息发送的岗位列表</param>

        //private  static void SendWarningsToUsers(string Number, string UserNumber, string title, string Module_Url, DataRow[] row)
        //{
        //    foreach (DataRow dc in row)
        //    {
        //        //取得岗位要提醒的岗位名称
        //        string jobname = dc["roleName"].ToString();
        //        //取得roleType 
        //        string roleType = dc["roleType"].ToString();
        //        //取得提醒信息的类型
        //        int Grade = int.Parse(dc["grade"].ToString());
        //        //获取warningContext
        //        string Context = title + ":" + dc["warningContext"].ToString() + "。单号为：" + Number;
        //        //声明一个存放是岗位名称／部门名称的变量
        //        string condition = null;
        //        //短信发送节点变量  2010-07-12 刘杰
        //        string mobileE = "";
        //        try
        //        {
        //             mobileE = dc["sMobileEnable"].ToString();
        //        }
        //        catch (Exception e1)
        //        {
        //            mobileE = "无";

        //        }

        //        //根据roleType判断是岗位名称还是部门名称
        //        if (roleType == "1")
        //        {
        //            condition = "HR_Jobs.JobName";
        //        }
        //        else if (roleType == "2")
        //        {
        //            condition = "HR_Employees.BM";
        //        }
        //        else if (roleType == "3")
        //        {
        //            condition = "HR_Employees.Number";
        //        }
        //        //读取符合该岗位的员工编号
        //        StringBuilder str_sql = new StringBuilder();
        //        str_sql.Append("SELECT HR_Employees.Number ");
        //        str_sql.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
        //        str_sql.Append("WHERE  ");
        //        str_sql.Append(condition);
        //        str_sql.Append("='" + jobname + "'");
        //        SqlDataReader dr_employees = DbHelperSQL.ExecuteReader(str_sql.ToString());
        //        while (dr_employees.Read())
        //        {
        //            string ToUser = dr_employees[0].ToString();
        //            //插入提醒信息
        //            AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
        //            //插入短信提醒 2010-07-12 刘杰
        //            if (mobileE == "1")
        //            {
                      
        //                SendDX sd = new SendDX();
        //                sd.SendToP(ToUser, Context);
        //            }
        //        }
        //        dr_employees.Close();
  





        //        //读取符合该岗位的员工编号
        //        StringBuilder str_sql2 = new StringBuilder();
        //        str_sql2.Append("SELECT HR_Employees.Number,(select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) AS SQWT  ");
        //        str_sql2.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
        //        str_sql2.Append("WHERE (select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) is not null  AND  ");
        //        str_sql2.Append(condition);
        //        str_sql2.Append("='" + jobname + "'");
        //        SqlDataReader dr_employees2 = DbHelperSQL.ExecuteReader(str_sql2.ToString());
        //        while (dr_employees2.Read())
        //        {
        //            string ToUser = dr_employees2[1].ToString();
        //            //插入提醒信息
        //            AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
        //        }
        //        dr_employees2.Close();
             

        //    }
        //}
        //#endregion

        //#region
        //  /// <summary>
        ///// 发送一组提醒
        ///// </summary>
        ///// <param name="Number">单号</param>
        ///// <param name="UserNumber">信息产生者编号</param>
        ///// <param name="title">模块名称</param>
        ///// <param name="Module_Url">查看地址</param>
        ///// <param name="row">提醒信息发送的岗位列表</param>

        //public static void SendSeeWarningsToUsers(string Number, string UserNumber, string title, string Module_Url, string[] JobName, int grade)
        //{
        //    if (JobName == null) return;
        //    foreach (string jbs in JobName)
        //    {
        //        //取得分割字符串的岗位
        //        string[] jb = FMOP.SS.SplitString.GetArralist(jbs, ":");
        //        //取得岗位要提醒的岗位名称
        //        string jobname =jb[1].ToString ();
        //        //取得roleType 
        //        string roleType = jb[0].ToString();
        //        //取得提醒信息的类型
        //        int Grade = grade;
        //        //获取warningContext
        //        string Context = title + ":" + "审批后驳回！" + "单号为：" + Number;
        //        //声明一个存放是岗位名称／部门名称的变量
        //        string condition = null;
        //        //根据roleType判断是岗位名称还是部门名称
        //        if (roleType == "1")
        //        {
        //            condition = "HR_Jobs.JobName";
        //        }
        //        else if (roleType == "2")
        //        {
        //            condition = "HR_Employees.BM";
        //        }
        //        else if (roleType == "3")
        //        {
        //            condition = "HR_Employees.Number";
        //        }
        //        //读取符合该岗位的员工编号
        //        StringBuilder str_sql = new StringBuilder();
        //        str_sql.Append("SELECT HR_Employees.Number ");
        //        str_sql.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
        //        str_sql.Append("WHERE  ");
        //        str_sql.Append(condition);
        //        str_sql.Append("='" + jobname + "'");
        //        SqlDataReader dr_employees = DbHelperSQL.ExecuteReader(str_sql.ToString());
        //        while (dr_employees.Read())
        //        {
        //            string ToUser = dr_employees[0].ToString();
        //            //插入提醒信息
        //            AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
        //        }
        //        dr_employees.Close();
        //    }
        //}
        //#endregion


        //#region
        ///// <summary>
        ///// 对岗位创建提醒信息
        ///// </summary>
        ///// <param name="Number">单号</param>
        ///// <param name="Context">内容</param>
        ///// <param name="Module">模块名</param>
        ///// <param name="UserNumber">员工编号</param>
        ///// <returns></returns>
        //public short CreateWarningToJobName(string Number, string Context, string Module, string UserNumber, string JobName, int Grade)
        //{
        //    if (Number != "" && Context != "" && Module != "" && UserNumber != "" && JobName != "")
        //    {
        //        string page = "WorkFlow_Check.aspx?module=";
        //        string filenName = "&number=";
        //        string Module_Url = page + Module + filenName + Number;
        //        string jobname = JobName;
        //        string[] str = jobname.Split(':');
        //        string roleType = str[0].ToString();
        //        string NextChecker = str[1].ToString();
        //        //声明一个存放是岗位名称／部门名称的变量
        //        string condition = null;
        //        //根据roleType判断是岗位名称还是部门名称
        //        if (roleType == "1")
        //        {
        //            condition = "HR_Jobs.JobName";
        //        }
        //        else if (roleType == "2")
        //        {
        //            condition = "HR_Employees.BM";
        //        }
        //        else if (roleType == "3")
        //        {
        //            condition = "HR_Employees.Number";
        //        }
        //        StringBuilder str_sql = new StringBuilder();
        //        str_sql.Append("SELECT HR_Employees.Number ");
        //        str_sql.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
        //        str_sql.Append("WHERE  ");
        //        str_sql.Append(condition);
        //        str_sql.Append("='" + NextChecker + "'");
        //        SqlDataReader dr_employees = DbHelperSQL.ExecuteReader(str_sql.ToString());
        //        while (dr_employees.Read())
        //        {
        //            string ToUser = dr_employees[0].ToString();
        //            //插入提醒信息
        //            AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
        //            Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new WorkFlowModule(Module);
        //            int su=wf.check.UpdateNextChecker(Number, jobname);
        //        }
        //        dr_employees.Close();
        //        //插入委托提醒
        //        StringBuilder str_sql2 = new StringBuilder();
        //        str_sql2.Append("SELECT HR_Employees.Number,(select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) AS SQWT  ");
        //        str_sql2.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
        //        str_sql2.Append("WHERE (select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) is not null  AND  ");
        //        str_sql2.Append(condition);
        //        str_sql2.Append("='" + NextChecker + "'");
        //        SqlDataReader dr_employees2 = DbHelperSQL.ExecuteReader(str_sql2.ToString());
        //        while (dr_employees2.Read())
        //        {
        //            string ToUser = dr_employees2[0].ToString();
        //            //插入提醒信息
        //            AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
        //            Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new WorkFlowModule(Module);
        //            int su = wf.check.UpdateNextChecker(Number, jobname);
        //        }
        //        dr_employees2.Close();



        //        return 1;
        //    }
        //    return 0;
        //}

        //#endregion

        ///// <summary>
        ///// 对订单创建者提醒
        ///// </summary>
        ///// <param name="Number">单号</param>
        ///// <param name="Type">提醒类别</param>
        ///// <param name="UserNumber">当前员工编号</param>
        ///// <param name="ToUserNumber">创建订单的员工编号</param>
        ///// <returns></returns>
        //public  short CreateWarningToUser(string Number, int Type, string UserNumber, string ToUserNumber)
        //{
        //    if (property != null && RoleWarning != null && RoleWarning.Tables.Count > 0)
        //    {
        //        //取得模块名
        //        string name = property.ModuleName;
        //        string title = property.Title;
        //        //取得模块类型
        //        int ModuleType = property.ModuleType;
        //        if (ModuleType == 1 || ModuleType == 4)
        //        {
        //            string Module_Url = null;
        //            if (ModuleType == 1)
        //            {
        //                string page = "WorkFlow_Check.aspx?module=";
        //                string filenName = "&number=";
        //                //拼接连接地址字符串
        //                Module_Url = page + name + filenName + Number;
        //                string msg = "单号为:" + Number + "的" + title + "请求审批!";
        //                AddWarning(msg, Module_Url, 1, UserNumber, ToUserNumber);
        //            }
        //            //else if (ModuleType == 4)
        //            //{
        //            //    //string UrlXml = dr["Url"].ToString();
        //            //    //XmlNode UrlNode = UrlDcmt.SelectSingleNode("//AddOnModule");
        //            //    //Module_Url = UrlNode["Url"].InnerXml;
        //            //}
        //            DataRow[] row = RoleWarning.Tables[0].Select("warningType='" + Type + "'");
        //            if (row.Length == 1)
        //            {
        //                foreach (DataRow dc in row)
        //                {
        //                    //取得岗位要提醒的岗位名称
        //                    string jobname = dc["roleName"].ToString();
        //                    //取得提醒信息的类型
        //                    int Grade = int.Parse(dc["grade"].ToString());
        //                    //获取warningContext
        //                    string Context = title + ":" + dc["warningContext"].ToString() + "。单号为：" + Number;
        //                    //插入提醒信息
        //                    AddWarning(Context, Module_Url, Grade, UserNumber, ToUserNumber);
        //                    return 1;
        //                }
        //            }
        //        }
        //    }
        //    return 0;
        //}

        ///// <summary>
        ///// 将提醒信息写入数据库
        ///// </summary>
        ///// <param name="Context">提醒内容</param>
        ///// <param name="Module_Url">查看地址</param>
        ///// <param name="Grade">提醒类别：1：提醒2：报警</param>
        ///// <param name="FromUser">信息发送人的工号</param>
        ///// <param name="ToUser">信息接收人的工号</param>
        //public static void AddWarning(string Context, string Module_Url, int Grade, string FromUser, string ToUser)
        //{
        //    try
        //    {
        //        StringBuilder strSql = new StringBuilder();
        //        strSql.Append("insert into User_Warnings(");
        //        strSql.Append("Context,Module_Url,Grade,FromUser,ToUser)");
        //        strSql.Append(" values (");
        //        strSql.Append("@Context,@Module_Url,@Grade,@FromUser,@ToUser)");
        //        SqlParameter[] parameters = {
        //            new SqlParameter("@Context", SqlDbType.VarChar,4000),
        //            new SqlParameter("@Module_Url", SqlDbType.VarChar,500),
        //            new SqlParameter("@Grade", SqlDbType.SmallInt,2),
        //            new SqlParameter("@FromUser", SqlDbType.VarChar,50),
        //            new SqlParameter("@ToUser", SqlDbType.VarChar,50)};
        //        parameters[0].Value = Context;
        //        parameters[1].Value = Module_Url;
        //        parameters[2].Value = Grade;
        //        parameters[3].Value = FromUser;
        //        parameters[4].Value = ToUser;
        //        DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

        //    }
        //    catch (Exception EX)
        //    {
        //        throw (EX);
        //    }

        //}

        ///// <summary>
        ///// 获取报警信息的记录数
        ///// </summary>
        ///// <param name="EmployeeNo"></param>
        ///// <returns></returns>
        //public static int GetWarningCount(string EmployeeNo)
        //{
        //    if (EmployeeNo != null)
        //    {
        //        StringBuilder sql = new StringBuilder();
        //        #region 原始版本  
        //        //sql.Append("SELECT count(HR_Employees.Employee_Name) ");
        //        //sql.Append("FROM  User_Warnings INNER JOIN HR_Employees ON User_Warnings.ToUser = HR_Employees.Number ");
        //        //sql.Append("where isRead=0 and Grade=2 and touser ='" + EmployeeNo + "' ");
        //        #endregion 
              
        //        #region  王永辉 2009.06.15
        //        sql.Append("select count(*) from ( SELECT User_Warnings.* ");
        //        sql.Append("FROM  User_Warnings INNER JOIN HR_Employees ON User_Warnings.ToUser = HR_Employees.Number ");
        //        sql.Append("where touser ='" + EmployeeNo + "' ");
        //        sql.Append(SqlServiceMemcountdtr(true,EmployeeNo));
        //        sql.Append(") as c where isRead=0 and Grade=2");

        //        #endregion
        //        object ob = DbHelperSQL.GetSingle(sql.ToString());
        //        if (ob != null)
        //        {
        //            return int.Parse(ob.ToString());
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    return 0;
        //}
        ///// <summary>
        ///// 获取提醒信息的记录数
        ///// </summary>
        ///// <param name="EmployeeNo"></param>
        ///// <returns></returns>
        //public static int GetRemindCount(string EmployeeNo)
        //{
        //    if (EmployeeNo != null)
        //    {
        //        StringBuilder sql = new StringBuilder();

        //        #region 原始版本
        //        //sql.Append("SELECT count(HR_Employees.Employee_Name) ");
        //        //sql.Append("FROM  User_Warnings INNER JOIN HR_Employees ON User_Warnings.ToUser = HR_Employees.Number ");
        //        //sql.Append("where isRead=0 and Grade=1 and touser ='" + EmployeeNo + "' ");
        //        #endregion

        //        #region 王永辉 2009.06.15 修改
        //        sql.Append("select count(*) from ( SELECT User_Warnings.* ");
        //        sql.Append("FROM  User_Warnings INNER JOIN HR_Employees ON User_Warnings.ToUser = HR_Employees.Number ");
        //        sql.Append("where touser ='" + EmployeeNo + "' ");
        //        sql.Append(SqlServiceMemcountdtr(true, EmployeeNo));
        //        sql.Append(") as c where isRead=0 and Grade=1");
        //        #endregion
        //        object ob = DbHelperSQL.GetSingle(sql.ToString());
        //        if (ob != null)
        //        {
        //            return int.Parse(ob.ToString());
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    return 0;
        //}

        ///// <summary>
        ///// 判断用户是否是服务中心人员 2009.06.10 王永辉
        ///// </summary>
        ///// <param name="UserID">当前用户ID</param>
        ///// <returns></returns>
        //private static bool SqlServiceMemcount(string UserID)
        //{
        //    bool lable = false ;
        //    string strSql = "select bm ,number from dbo.HR_Employees where Number = '" + UserID+"'";
        //    DataSet ds = DbHelperSQL.Query(strSql);
        //    if (ds != null)
        //    {
        //        if (ds.Tables[0] != null)
        //        {
        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                if (dr[0].ToString().Trim() == "服务中心")
        //                {
        //                   lable = true ;
        //                    break ;
        //                }
        //            }
        //        }
        //    }

        //    return lable ;
            
        //}


        ///// <summary>
        ///// 服务中心人员提醒和警告特殊处理 2009.06.10
        ///// </summary>
        ///// <param name="lable"></param>
        ///// <param name="UserID"></param>
        ///// <returns></returns>
        //public static string  SqlServiceMemcountdtr(bool lable,string UserID )
        //{
        //    StringBuilder strb = new StringBuilder(" ");
        //    if (lable)
        //    {
        //        string StrSql = "select * from dbo.RightFenpei as a left join QXSZXXXX as b on a.Number = b.ParentNumber where a.YGBH = '" + UserID+"'";
        //        DataSet ds = DbHelperSQL.Query(StrSql);
        //        if (ds != null)
        //        {
        //            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //            {
        //                strb.Append(" or ");
        //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //                {
        //                    if (i == ds.Tables[0].Rows.Count-1)
        //                    {
        //                        strb.Append(" touser = '" + ds.Tables[0].Rows[i]["SLRBH"].ToString() + "'");
        //                    }
        //                    else
        //                    {
        //                        strb.Append(" touser = '" + ds.Tables[0].Rows[i]["SLRBH"].ToString() + "' or");
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return strb.ToString();
        //}

        #endregion
    }
}