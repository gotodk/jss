using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using FMOP.DB;
using System.Data.SqlClient;
using ShareLiu.DXS;  //刘杰 2010-07-12 加入:短信发送空间
/// <summary>
/// CustomWarning 的摘要说明
/// </summary>
namespace Hesion.Brick.Core
{
    public class CustomWarning
    {
        public CustomWarning()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static void CreateWarningToJobName(string Number, string Context, string Module, string UserNumber, string JobName, int Grade)
        {
            if (Number != null && Context != null && Module != null && UserNumber != null && JobName != null)
            {
                string page = "WorkFlow_Check.aspx?module=";
                string filenName = "&number=";
                string Module_Url = page + Module + filenName + Number;
                string jobname = JobName;
                string[] str = jobname.Split(':');
                string roleType = str[0].ToString();
                string NextChecker = str[1].ToString();
                //声明一个存放是岗位名称／部门名称的变量
                string condition = null;
                //根据roleType判断是岗位名称还是部门名称
                if (roleType == "1")
                {
                    condition = "HR_Jobs.JobName";
                }
                else if (roleType == "2")
                {
                    condition = "HR_Employees.BM";
                }
                else if (roleType == "3")
                {
                    condition = "HR_Employees.Number";
                }
                StringBuilder str_sql = new StringBuilder();
                str_sql.Append("SELECT HR_Employees.Number ");
                str_sql.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
                str_sql.Append("WHERE  ");
                str_sql.Append(condition);
                str_sql.Append("='" + NextChecker + "'");
                SqlDataReader dr_employees = DbHelperSQL.ExecuteReader(str_sql.ToString());
                while (dr_employees.Read())
                {
                    string ToUser = dr_employees[0].ToString();
                    //插入提醒信息
                    AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
                    UpdateNextChecker(Module,Number, jobname);
                }
                dr_employees.Close();
                //插入授权提醒
                StringBuilder str_sql2 = new StringBuilder();
                str_sql2.Append("SELECT HR_Employees.Number,(select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) AS SQWT  ");
                str_sql2.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
                str_sql2.Append("WHERE (select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) is not null  AND  ");
                str_sql2.Append(condition);
                str_sql2.Append("='" + NextChecker + "'");
                SqlDataReader dr_employees2 = DbHelperSQL.ExecuteReader(str_sql2.ToString());
                while (dr_employees2.Read())
                {
                    string ToUser = dr_employees2[1].ToString();
                    //插入提醒信息
                    AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
                }
                dr_employees2.Close();

            }
        }




        public static void CreateWarningToJobName(string Number, string Context, string Module, string UserNumber, string JobName, int Grade,int LimitMinutes)
        {
            if (Number != null && Context != null && Module != null && UserNumber != null && JobName != null)
            {
                string page = "WorkFlow_Check.aspx?module=";
                string filenName = "&number=";
                string Module_Url = page + Module + filenName + Number;
                string jobname = JobName;
                string[] str = jobname.Split(':');
                string roleType = str[0].ToString();
                string NextChecker = str[1].ToString();
                //声明一个存放是岗位名称／部门名称的变量
                string condition = null;
                //根据roleType判断是岗位名称还是部门名称
                if (roleType == "1")
                {
                    condition = "HR_Jobs.JobName";
                }
                else if (roleType == "2")
                {
                    condition = "HR_Employees.BM";
                }
                else if (roleType == "3")
                {
                    condition = "HR_Employees.Number";
                }
                StringBuilder str_sql = new StringBuilder();
                str_sql.Append("SELECT HR_Employees.Number ");
                str_sql.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
                str_sql.Append("WHERE  ");
                str_sql.Append(condition);
                str_sql.Append("='" + NextChecker + "'");
                SqlDataReader dr_employees = DbHelperSQL.ExecuteReader(str_sql.ToString());
                while (dr_employees.Read())
                {
                    string ToUser = dr_employees[0].ToString();
                    //插入提醒信息
                    AddWarning(Context, Module_Url, Grade, UserNumber, ToUser,LimitMinutes);
                    UpdateNextChecker(Module, Number, jobname);
                }
                dr_employees.Close();
                   //插入授权提醒
                StringBuilder str_sql2 = new StringBuilder();
                str_sql2.Append("SELECT HR_Employees.Number,(select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) AS SQWT  ");
                str_sql2.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
                str_sql2.Append("WHERE (select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) is not null  AND  ");
                str_sql2.Append(condition);
                str_sql2.Append("='" + NextChecker + "'");
                SqlDataReader dr_employees2 = DbHelperSQL.ExecuteReader(str_sql2.ToString());
                while (dr_employees2.Read())
                {
                    string ToUser = dr_employees2[1].ToString();
                    //插入提醒信息
                    AddWarning(Context, Module_Url, Grade, UserNumber, ToUser,LimitMinutes);
                }
                dr_employees2.Close();
            }
        }
     //重载 加入发送短信的变量 刘杰 2010-07-14
        public static void CreateWarningToJobName(string Number, string Context, string Module, string UserNumber, string JobName, int Grade, int LimitMinutes,string dx)
        {
            if (Number != null && Context != null && Module != null && UserNumber != null && JobName != null)
            {
                string page = "WorkFlow_Check.aspx?module=";
                string filenName = "&number=";
                string Module_Url = page + Module + filenName + Number;
                string jobname = JobName;
                string[] str = jobname.Split(':');
                string roleType = str[0].ToString();
                string NextChecker = str[1].ToString();
                //声明一个存放是岗位名称／部门名称的变量
                string condition = null;
                //根据roleType判断是岗位名称还是部门名称
                if (roleType == "1")
                {
                    condition = "HR_Jobs.JobName";
                }
                else if (roleType == "2")
                {
                    condition = "HR_Employees.BM";
                }
                else if (roleType == "3")
                {
                    condition = "HR_Employees.Number";
                }
                StringBuilder str_sql = new StringBuilder();
                str_sql.Append("SELECT HR_Employees.Number ");
                str_sql.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
                str_sql.Append("WHERE  ");
                str_sql.Append(condition);
                str_sql.Append("='" + NextChecker + "'");
                SqlDataReader dr_employees = DbHelperSQL.ExecuteReader(str_sql.ToString());
                while (dr_employees.Read())
                {
                    string ToUser = dr_employees[0].ToString();
                    //插入提醒信息
                    AddWarning(Context, Module_Url, Grade, UserNumber, ToUser, LimitMinutes);

                    if (dx== "1")
                    {
                        SendDX sd = new SendDX();
                        sd.SendToP(ToUser, Context);
                    }
                    UpdateNextChecker(Module, Number, jobname);
                }
                dr_employees.Close();
                //插入授权提醒
                StringBuilder str_sql2 = new StringBuilder();
                str_sql2.Append("SELECT HR_Employees.Number,(select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) AS SQWT  ");
                str_sql2.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
                str_sql2.Append("WHERE (select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) is not null  AND  ");
                str_sql2.Append(condition);
                str_sql2.Append("='" + NextChecker + "'");
                SqlDataReader dr_employees2 = DbHelperSQL.ExecuteReader(str_sql2.ToString());
                while (dr_employees2.Read())
                {
                    string ToUser = dr_employees2[1].ToString();
                    //插入提醒信息
                    AddWarning(Context, Module_Url, Grade, UserNumber, ToUser, LimitMinutes);
                }
                dr_employees2.Close();
            }
        }
        /// <summary>
        /// 给第一个审核人发送短信
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="Context"></param>
        /// <param name="Module"></param>
        /// <param name="UserNumber"></param>
        /// <param name="JobName"></param>
        public static void SendDXSH(string Number, string Context, string Module, string UserNumber, string JobName)
        {
            if (Number != null && Context != null && Module != null && UserNumber != null && JobName != null)
            {
     
                string jobname = JobName;
                string[] str = jobname.Split(':');
                string roleType = str[0].ToString();
                string NextChecker = str[1].ToString();
                string mobileE = str[2].ToString();
                //声明一个存放是岗位名称／部门名称的变量
                string condition = null;
                //根据roleType判断是岗位名称还是部门名称
                if (roleType == "1")
                {
                    condition = "HR_Jobs.JobName";
                }
                else if (roleType == "2")
                {
                    condition = "HR_Employees.BM";
                }
                else if (roleType == "3")
                {
                    condition = "HR_Employees.Number";
                }
                StringBuilder str_sql = new StringBuilder();
                str_sql.Append("SELECT HR_Employees.Number ");
                str_sql.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
                str_sql.Append("WHERE  ");
                str_sql.Append(condition);
                str_sql.Append("='" + NextChecker + "'");
                SqlDataReader dr_employees = DbHelperSQL.ExecuteReader(str_sql.ToString());
                while (dr_employees.Read())
                {
                    string ToUser = dr_employees[0].ToString();

                    if (mobileE == "1")
                    {
                        SendDX sd = new SendDX();
                        sd.SendToP(ToUser.Trim().ToString(),Context);                    
                    }
                }
                dr_employees.Close();
            }
        }

        /// <summary>
        /// 将提醒信息写入数据库
        /// </summary>
        /// <param name="Context">提醒内容</param>
        /// <param name="Module_Url">查看地址</param>
        /// <param name="Grade">提醒类别：1：提醒2：报警</param>
        /// <param name="FromUser">信息发送人的工号</param>
        /// <param name="ToUser">信息接收人的工号</param>
        public static void AddWarning(string Context, string Module_Url, int Grade, string FromUser, string ToUser)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into User_Warnings(");
                strSql.Append("Context,Module_Url,Grade,FromUser,ToUser)");
                strSql.Append(" values (");
                strSql.Append("@Context,@Module_Url,@Grade,@FromUser,@ToUser)");
                SqlParameter[] parameters = {
					new SqlParameter("@Context", SqlDbType.VarChar,4000),
					new SqlParameter("@Module_Url", SqlDbType.VarChar,500),
					new SqlParameter("@Grade", SqlDbType.SmallInt,2),
					new SqlParameter("@FromUser", SqlDbType.VarChar,50),
					new SqlParameter("@ToUser", SqlDbType.VarChar,50)};
                parameters[0].Value = Context;
                parameters[1].Value = Module_Url;
                parameters[2].Value = Grade;
                parameters[3].Value = FromUser;
                parameters[4].Value = ToUser;
                DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

            }
            catch (Exception EX)
            {
                throw (EX);
            }

        }



        /// <summary>
        /// 将提醒信息写入数据库
        /// </summary>
        /// <param name="Context">提醒内容</param>
        /// <param name="Module_Url">查看地址</param>
        /// <param name="Grade">提醒类别：1：提醒2：报警</param>
        /// <param name="FromUser">信息发送人的工号</param>
        /// <param name="ToUser">信息接收人的工号</param>
        /// <param name="LimitMinutes">限制的分钟数</param>
        public static void AddWarning(string Context, string Module_Url, int Grade, string FromUser, string ToUser,int LimitMinutes)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into User_Warnings(");
                strSql.Append("Context,Module_Url,Grade,FromUser,ToUser,IsCheckInfo,LimitTime)");
                strSql.Append(" values (");
                strSql.Append("@Context,@Module_Url,@Grade,@FromUser,@ToUser,1,getdate()+@LimitTime/60.0/24.0)");
                SqlParameter[] parameters = {
					new SqlParameter("@Context", SqlDbType.VarChar,4000),
					new SqlParameter("@Module_Url", SqlDbType.VarChar,500),
					new SqlParameter("@Grade", SqlDbType.SmallInt,2),
					new SqlParameter("@FromUser", SqlDbType.VarChar,50),
					new SqlParameter("@ToUser", SqlDbType.VarChar,50),
                new SqlParameter("@LimitTime", SqlDbType.Int)};
                parameters[0].Value = Context;
                parameters[1].Value = Module_Url;
                parameters[2].Value = Grade;
                parameters[3].Value = FromUser;
                parameters[4].Value = ToUser;
                parameters[5].Value = LimitMinutes;
                DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

            }
            catch (Exception EX)
            {
                throw (EX);
            }

        }

        public static void UpdateNextChecker(string moduleName,string OrderNumber, string CheckerJobName)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ");
                strSql.Append(moduleName);
                strSql.Append(" set ");
                strSql.Append("NextChecker=@NextChecker");
                strSql.Append(" where Number=@Number ");
                SqlParameter[] parameters = {
                new SqlParameter("@NextChecker", SqlDbType.VarChar,50),
                new SqlParameter ("@Number",SqlDbType.VarChar,50)};
                parameters[0].Value = CheckerJobName;
                parameters[1].Value = OrderNumber;
                DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
    }
}
