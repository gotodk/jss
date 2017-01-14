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
using FMOP.DB;
using System.Text;

namespace FMOP.RCI
{
    /// <summary>
    /// ReadCheckInfo 的摘要说明
    /// 查询CheckInfo返回datareader
    /// </summary>
    public class ReadCheckInfo
    {
        /// <summary>
        /// 查询system_CheckInfo表模块名和定单名返回datareader
        /// </summary>
        /// <param name="ModuleName">模块名</param>
        /// <param name="Number">单号</param>
        /// <returns>sqldatareader</returns>
        public static SqlDataReader sqlReadCheckInfo(string ModuleName, string Number)
        {
            SqlDataReader dr = null;
            //拼接sql查询字符串
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ");
            strSql.Append("system_CheckInfo ");
            strSql.Append("WHERE (");
            strSql.Append("(ModuleName='");
            strSql.Append(ModuleName);
            strSql.Append("') ");
            strSql.Append("AND ");
            strSql.Append("(Number='");
            strSql.Append(Number);
            strSql.Append("')) ");
            strSql.Append("ORDER BY ");
            strSql.Append("CheckTime ");
            dr = DbHelperSQL.ExecuteReader(strSql.ToString());
            return dr;
        }

        /// <summary>
        /// 查询system_CheckInfo表模块名和定单名返回DataSet
        /// </summary>
        /// <param name="ModuleName">模块名</param>
        /// <param name="Number">单号</param>
        /// <returns>sqldatareader</returns>
        public static DataSet sqlCheckInfo(string ModuleName, string Number)
        {
            //拼接sql查询字符串
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT checker,checktime,case when ispass = 1 then '通过' else '驳回' end as ispass,remark FROM ");
            strSql.Append("system_CheckInfo ");
            strSql.Append("WHERE (");
            strSql.Append("(ModuleName='");
            strSql.Append(ModuleName);
            strSql.Append("') ");
            strSql.Append("AND ");
            strSql.Append("(Number='");
            strSql.Append(Number);
            strSql.Append("')) ");
            strSql.Append("ORDER BY ");
            strSql.Append("CheckTime ");
            return DbHelperSQL.Query(strSql.ToString());
        }

    }
}