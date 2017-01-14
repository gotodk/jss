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
using System.Text;
using System.Xml;
using FMOP.DB;

namespace Hesion.Brick.Core
{



    /// <summary>
    /// 模块所属栏目
    /// </summary>
    public abstract class ModuleClass
    {
        #region
        /// <summary>
        /// 通过小类编号获取大类名称
        /// </summary>
        /// <param name="SmallClassId"></param>
        /// <returns></returns>
        public static string GetBigClassNameBySmallClassId(int SmallClassId)
        {
            string bigClassName = null;
            try
            {
                StringBuilder str_sql = new StringBuilder();
                str_sql.Append("SELECT system_BigClass.name ");
                str_sql.Append("FROM system_SmallClass INNER JOIN ");
                str_sql.Append("system_BigClass ON system_SmallClass.BigClassId = system_BigClass.id ");
                str_sql.Append("WHERE system_SmallClass.Id=");
                str_sql.Append("'" + SmallClassId + "'");
                SqlDataReader dr_bigClassName = DbHelperSQL.ExecuteReader(str_sql.ToString());
                if (dr_bigClassName.Read())
                {
                    bigClassName = dr_bigClassName["system_BigClass.name"].ToString();
                }
                dr_bigClassName.Close();

            }
            catch
            {
                return bigClassName;
            }
            return bigClassName;
        }
        #endregion

        #region
        /// <summary>
        /// 通过小类编号获取小类名称
        /// </summary>
        /// <param name="SmallClassId"></param>
        /// <returns></returns>
        public static string GetSmallClassName(int SmallClassId)
        {
            string SmallClassName = null;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ClassName ");
            strSql.Append("from system_SmallClass ");
            strSql.Append("where id=");
            strSql.Append("'" + SmallClassId + "'");
            SqlDataReader dr_SmallClassName = DbHelperSQL.ExecuteReader(strSql.ToString());
            if (dr_SmallClassName.Read())
            {
                SmallClassName = dr_SmallClassName["ClassName"].ToString();
            }
            dr_SmallClassName.Close();

            return SmallClassName;
        }
        #endregion

        #region
        /// <summary>
        /// 通过大类编号获取大类名称
        /// </summary>
        /// <param name="BigClassId"></param>
        /// <returns></returns>
        public static string GetBigClassName(int BigClassId)
        {
            string BigClassName = null;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select name ");
            strSql.Append("from system_BigClass ");
            strSql.Append("where id=");
            strSql.Append("'" + BigClassId + "'");
            SqlDataReader dr_BigClassName = DbHelperSQL.ExecuteReader(strSql.ToString());
            if (dr_BigClassName.Read())
            {
                BigClassName = dr_BigClassName["ClassName"].ToString();
            }
            dr_BigClassName.Close();

            return BigClassName;
        }
        #endregion

    }
}