using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.DB;
using System.Text;
using System.Data.SqlClient;

namespace Hesion.Brick.Core
{

    /// <summary>
    /// 日志类
    /// 日志
    /// </summary>
    public abstract class Log
    {

        /// <summary>
        /// 写入一条错误信息
        /// </summary>
        /// <param name="Info">信息内容</param>
        public static void Error(string Info)
        {
            WriteLogToDB(Info, 2);
        }

        /// <summary>
        /// 写入一条调试信息
        /// </summary>
        /// <param name="Info">信息内容</param>
        public static void Debug(string Info)
        {
            WriteLogToDB(Info, 0);
        }

        /// <summary>
        /// 写入一条调试信息
        /// </summary>
        /// <param name="Info">信息内容</param>
        public static void Warning(string Info)
        {
            WriteLogToDB(Info, 1);
        }

        /// <summary>
        /// 写入一条调试信息
        /// </summary>
        /// <param name="Info">信息内容</param>
        /// <param name="Type">日志的级别。0:debug;1:warning;2:Error</param>
        private static void WriteLogToDB(string Info, int Type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into system_Logs(");
            strSql.Append("Info,Type)");
            strSql.Append(" values (");
            strSql.Append("@Info,@Type)");
            SqlParameter[] parameters = {
					new SqlParameter("@Info", SqlDbType.Text),
					new SqlParameter("@Type", SqlDbType.Int,4)};
            parameters[0].Value = Info;
            parameters[1].Value = Type;
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

        }

    }
}