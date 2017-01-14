using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using FMOP.DB;

namespace Hesion.Brick.Core.WorkFlow
{
    /// <summary>
    /// DefinedModule 自定义模块类
    /// </summary>
    public class DefinedModule
    {
        public DefinedProperty definedProperty;

        /// <summary>
        /// </summary>
        public NumberFormat numberFormat;

        /// <summary>
        /// </summary>
        public Warning warning;

        private XmlDocument configuration;

        /// <summary>
        /// </summary>
        public Property property;

        /// <summary>
        /// </summary>
        public Authentication authentication;


        public Check check;

        public XmlDocument Configuration{	
			get{
				return this.configuration;
			}	
			set{
				this.configuration = value;
			}	
		}
	

		/// <summary>
		 /// 通过模块名称构造本类
		/// </summary>
		/// <param name="ModuleName"></param>
        public DefinedModule(string ModuleName)
        {
            //查询字符串
            if (ModuleName != null)
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT Configuration ");
                sql.Append("FROM system_Modules ");
                sql.Append("WHERE (name =");
                sql.Append(" '" + ModuleName + "') ");
                //执行查询
                SqlDataReader dr = DbHelperSQL.ExecuteReader(sql.ToString());
                if (dr.Read())
                {
                    try
                    {
                        //取得该模块的xml配置信息字
                        string xml = dr["Configuration"].ToString();
                        dr.Close();
                        //创建XmlDataDocument对象
                        XmlDataDocument xmldoc = new XmlDataDocument();
                        xmldoc.LoadXml(xml);
                        configuration = xmldoc;
                        //对advProperty进行构造
                        definedProperty = new DefinedProperty(configuration.SelectSingleNode("//AddOnModule"));
                        //对property进行构造
                        property = new Property(DbHelperSQL.Query("select * from system_Modules where name='" + ModuleName + "'"));
                        //对numberFormat进行构造
                        if (configuration.SelectSingleNode("//NumberFormat") != null)
                        {
                            numberFormat = new NumberFormat(property, configuration.SelectSingleNode("//NumberFormat"));
                        }
                        //对warning进行构造
                        if (configuration.SelectSingleNode("//Warning") != null)
                        {
                            warning = new Warning(property, configuration.SelectSingleNode("//Warning"));
                        }

                        //对authentication进行构造
                        if (configuration.SelectSingleNode("//Authentication") != null ||property.AuthType ==0)
                        {
                            authentication = new Authentication(property, configuration.SelectSingleNode("//Authentication"));
                        }
                        //对check进行构造
                        if (configuration.SelectSingleNode("//Check") != null)
                        {
                            check = new Check(property, configuration.SelectSingleNode("//Check"), warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        dr.Close();
                        throw (ex);
                        //configuration = null;   
                    }
                   
                }
                dr.Close();
            }
		}	
	}
}