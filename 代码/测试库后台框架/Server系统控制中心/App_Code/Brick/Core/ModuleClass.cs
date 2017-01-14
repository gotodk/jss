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
using FMDBHelperClass;
using System.Collections;

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
                str_sql.Append("WHERE system_SmallClass.Id=@SmallClassId");
                //str_sql.Append("'" + SmallClassId + "'");
                Hashtable htCS = new Hashtable();
                htCS["@SmallClassId"] = SmallClassId;
                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
                Hashtable ht = I_DBL.RunParam_SQL(str_sql.ToString(), "", htCS);
                if (!(bool)ht["return_float"])
                {
                    return bigClassName;
                }
                DataSet ds = (DataSet)ht["return_ds"];
                if (ds != null&&ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
                {
                    DataRow dr_bigClassName=ds.Tables[0].Rows[0];
                    bigClassName = dr_bigClassName["system_BigClass.name"].ToString();
                }
               

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
            strSql.Append("where id=@id");
            //strSql.Append("'" + SmallClassId + "'");
            Hashtable htCS = new Hashtable();
            htCS["@id"] = SmallClassId;
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable ht = I_DBL.RunParam_SQL(strSql.ToString(), "", htCS);
            if (!(bool)ht["return_float"])
            {
                return SmallClassName;
            }
            DataSet ds = (DataSet)ht["return_ds"];

            if (ds != null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr_SmallClassName = ds.Tables[0].Rows[0];
                SmallClassName = dr_SmallClassName["ClassName"].ToString();
            }
            
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
            strSql.Append("where id=@id");           

            Hashtable htCS = new Hashtable();
            htCS["@id"] = BigClassId;
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable ht = I_DBL.RunParam_SQL(strSql.ToString(), "", htCS);
            if (!(bool)ht["return_float"])
            {
                return BigClassName;
            }
            DataSet ds = (DataSet)ht["return_ds"];

            if (ds != null &&ds.Tables.Count>0&& ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr_BigClassName = ds.Tables[0].Rows[0];
                BigClassName = dr_BigClassName["ClassName"].ToString();
            }
            return BigClassName;
        }
        #endregion

    }
}