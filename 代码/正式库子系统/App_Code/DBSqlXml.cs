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
using System.Xml.XPath;
using FMOP.DB;
namespace FMOP.DX
{
    /// <summary>
    /// 对数据库中xml数据类型列的查询
    /// </summary>
    public class DBSqlXml
    {
        /// <summary>
        /// 接受查询参数返回指定string类型xml的节点
        /// </summary>
        /// <param name="XmlTier">数据库中XML数据类型的列</param>
        /// <param name="XmlFatherNodes">要查询的父节点</param>
        /// <param name="XmlSonNode">该父节点下的子节点</param>
        /// <param name="AsXmlTier">自定义列名</param>
        /// <param name="TableName">要查询的表名</param>
        /// <param name="TableTier">该表主键列名</param>
        /// <param name="TableTierValue">主键列的属性</param>
        public static String DBStringXml(string XmlTier, string XmlFatherNodes, string XmlSonNode, string AsXmlTier, string TableName, string TableTier, string TableTierValue)
        {
            string xml =null;
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(XmlTier);
            sql.Append(".query ");
            sql.Append("('//");
            sql.Append(XmlFatherNodes);
            sql.Append("/");
            sql.Append(XmlSonNode);
            sql.Append("')");
            sql.Append(" AS ");
            sql.Append(AsXmlTier);
            sql.Append(" FROM ");
            sql.Append(TableName);
            sql.Append(" WHERE (");
            sql.Append(TableTier);
            sql.Append(" = '");
            sql.Append(TableTierValue);
            sql.Append("') ");
            sql.Append("And ");
            sql.Append("(");
            sql.Append(XmlTier);
            sql.Append(".exist ");
            sql.Append("('//");
            sql.Append(XmlFatherNodes);
            sql.Append("/");
            sql.Append(XmlSonNode);
            sql.Append("') ");
            sql.Append("= 1)");
            SqlDataReader dr = DbHelperSQL.ExecuteReader(sql.ToString());
            if (dr.Read())
            {
                xml = dr[AsXmlTier].ToString();
            }
            else
            {
                return xml;
            }
            dr.Close();
            return xml;
        }
        /// <summary>
        /// 接受查询参数返回指定string类型xml的第一个节点
        /// </summary>
        /// <param name="XmlTier">数据库中XML数据类型的列</param>
        /// <param name="XmlFatherNodes">要查询的父节点</param>
        /// <param name="XmlSonNode">该父节点下的子节点</param>
        /// <param name="NodeSeveral">第几个子节点</param>
        /// <param name="AsXmlTier">自定义列名</param>
        /// <param name="TableName">要查询的表名</param>
        /// <param name="TableTier">该表主键列名</param>
        /// <param name="TableTierValue">主键列的属性</param>
        public static string FirstSqlXml(string XmlTier, string XmlFatherNodes, string XmlSonNode, string NodeSeveral, string AsXmlTier, string TableName, string TableTier, string TableTierValue)
        {
            string xml =null;
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(XmlTier);
            sql.Append(".query ");
            sql.Append("('//");
            sql.Append(XmlFatherNodes);
            sql.Append("/");
            sql.Append(XmlSonNode);
            sql.Append("[");
            sql.Append(NodeSeveral);
            sql.Append("]");
            sql.Append("') ");
            sql.Append(" AS ");
            sql.Append(AsXmlTier);
            sql.Append(" FROM ");
            sql.Append(TableName);
            sql.Append(" WHERE (");
            sql.Append(TableTier);
            sql.Append(" = '");
            sql.Append(TableTierValue);
            sql.Append("') ");
            sql.Append("And ");
            sql.Append("(");
            sql.Append(XmlTier);
            sql.Append(".exist ");
            sql.Append("('//");
            sql.Append(XmlFatherNodes);
            sql.Append("/");
            sql.Append(XmlSonNode);
            sql.Append("[");
            sql.Append(NodeSeveral);
            sql.Append("]");
            sql.Append("') ");
            sql.Append("= 1)");
            SqlDataReader dr = DbHelperSQL.ExecuteReader(sql.ToString());
            if (dr.Read())
            {
                string ReadTier = AsXmlTier;
                xml = dr[ReadTier].ToString();
            }
            else
            {
                return xml;
            }
            dr.Close();
            return xml;
        }
        /// <summary>
        /// 接受查询参数返回指定string类型xml的第一个节点
        /// </summary>
        /// <param name="XmlTier">数据库中XML数据类型的列</param>
        /// <param name="XmlFatherNodes">要查询的父节点</param>
        /// <param name="XmlSonNode">该父节点下的子节点</param>
        /// <param name="NodeSeveral">第几个子节点</param>
        /// <param name="AsXmlTier">自定义列名</param>
        /// <param name="TableName">要查询的表名</param>
        /// <param name="TableTier">该表主键列名</param>
        /// <param name="TableTierValue">主键列的属性</param>
        public static string NextSqlXml(string XmlTier, string XmlFatherNodes, string XmlSonNodeElement, string AsXmlTier, string TableName, string TableTier, string TableTierValue)
        {
            string xml =null;
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(XmlTier);
            sql.Append(".query ");
            sql.Append("(");
            sql.Append("'<");
            sql.Append(AsXmlTier);
            sql.Append(">{for $r in");
            sql.Append("//");
            sql.Append(XmlFatherNodes);
            sql.Append("/*/");
            sql.Append(XmlSonNodeElement);
            sql.Append(" return $r }</");
            sql.Append(AsXmlTier);
            sql.Append(">') ");
            sql.Append(" AS ");
            sql.Append(AsXmlTier);
            sql.Append(" FROM ");
            sql.Append(TableName);
            sql.Append(" WHERE (");
            sql.Append(TableTier);
            sql.Append(" = '");
            sql.Append(TableTierValue);
            sql.Append("') ");
            sql.Append("And ");
            sql.Append("(");
            sql.Append(XmlTier);
            sql.Append(".exist ");
            sql.Append("('for $r in");
            sql.Append("//");
            sql.Append(XmlFatherNodes);
            sql.Append("/*/");
            sql.Append(XmlSonNodeElement);
            sql.Append(" return $r ");
            sql.Append("') ");
            sql.Append("= 1)");
            SqlDataReader dr = DbHelperSQL.ExecuteReader(sql.ToString());
            if (dr.Read())
            {
                //string ReadTier = '"' + AsXmlTier + '"';
                xml = dr[AsXmlTier].ToString();
            }
            else
            {
                return xml;
            }
            dr.Close();
            return xml;
        }


    }
}