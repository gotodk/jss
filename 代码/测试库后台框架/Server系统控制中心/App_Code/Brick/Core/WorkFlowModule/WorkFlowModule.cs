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
using FMDBHelperClass;


namespace Hesion.Brick.Core.WorkFlow{



	/// <summary>
	/// 工作流模块类
	/// </summary>
	public class WorkFlowModule{	

		/// <summary>
		/// </summary>
		public AdvProperty advProperty;

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

		/// <summary>
		/// </summary>
		public Check check;


		/// <summary>
		/// xml配置信息
		/// </summary>
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
        public WorkFlowModule(string ModuleName)
        {
            //查询字符串
            if (ModuleName != null)
            {
                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

                string sql = "SELECT Configuration FROM system_Modules WHERE name =@name";
                Hashtable ht = new Hashtable();
                ht["@name"] = ModuleName;
                Hashtable ht_touser = I_DBL.RunParam_SQL(sql, "", ht);
                if (!(bool)ht_touser["return_float"])
                {
                    throw ((Exception)ht_touser["return_errmsg"]);
                }
                DataSet ds_touser = (DataSet)ht_touser["return_ds"];

                if (ds_touser != null && ds_touser.Tables.Count>0 && ds_touser.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        //取得该模块的xml配置信息字符串
                        string xml = ds_touser.Tables[0].Rows[0]["Configuration"].ToString();

                        //创建XmlDataDocument对象
                        XmlDataDocument xmldoc = new XmlDataDocument();
                        xmldoc.LoadXml(xml);
                        configuration = xmldoc;
                        //对advProperty进行构造
                        if (configuration.SelectSingleNode("//WorkFlowModule") != null)
                        {
                            advProperty = new AdvProperty(configuration.SelectSingleNode("//WorkFlowModule"));
                        }
                        //对property进行构造
                        string sql2 = "select * from system_Modules where name=@name";
                        Hashtable ht2 = new Hashtable();
                        ht2["@name"] = ModuleName;
                        Hashtable ht_touser2 = I_DBL.RunParam_SQL(sql2, "", ht2);
                        if (!(bool)ht_touser2["return_float"])
                        {
                            throw ((Exception)ht_touser2["return_errmsg"]);
                        }
                        DataSet ds_touser2 = (DataSet)ht_touser["return_ds"];

                        property = new Property(ds_touser2);
                        //对numberFormat进行构造
                        if (property != null && configuration.SelectSingleNode("//NumberFormat") != null)
                        {
                            numberFormat = new NumberFormat(property, configuration.SelectSingleNode("//NumberFormat"));
                        }
                        //对warning进行构造
                        if (property != null && configuration.SelectSingleNode("//Warning") != null)
                        {
                            warning = new Warning(property, configuration.SelectSingleNode("//Warning"));
                        }
                        //对authentication进行构造
                        if (property != null && configuration.SelectSingleNode("//Authentication") != null)
                        {
                            authentication = new Authentication(property, configuration.SelectSingleNode("//Authentication"));
                        }
                        //对check进行构造
                        if (property != null && configuration.SelectSingleNode("//Check") != null)
                        {
                            check = new Check(property, configuration.SelectSingleNode("//Check"), warning);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw (ex);
                        //configuration = null;   
                    }

                }

            }
        }	
	}
}