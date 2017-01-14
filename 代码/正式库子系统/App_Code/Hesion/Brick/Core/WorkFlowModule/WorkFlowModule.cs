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


namespace Hesion.Brick.Core.WorkFlow{



	/// <summary>
	/// ������ģ����
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
		/// xml������Ϣ
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
		 /// ͨ��ģ�����ƹ��챾��
		/// </summary>
		/// <param name="ModuleName"></param>
		public WorkFlowModule(string ModuleName){
            //��ѯ�ַ���
            if (ModuleName != null)
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT Configuration ");
                sql.Append("FROM system_Modules ");
                sql.Append("WHERE (name =");
                sql.Append(" '" + ModuleName + "') ");
                //ִ�в�ѯ
                SqlDataReader dr = DbHelperSQL.ExecuteReader(sql.ToString());
                if (dr.Read())
                {
                    try
                    {
                        //ȡ�ø�ģ���xml������Ϣ�ַ���
                        string xml = dr["Configuration"].ToString();
                        dr.Close();
                        //����XmlDataDocument����
                        XmlDataDocument xmldoc = new XmlDataDocument();
                        xmldoc.LoadXml(xml);
                        configuration = xmldoc;
                        //��advProperty���й���
                        if (configuration.SelectSingleNode("//WorkFlowModule") != null)
                        {
                            advProperty = new AdvProperty(configuration.SelectSingleNode("//WorkFlowModule"));
                        }
                        //��property���й���
                        property = new Property(DbHelperSQL.Query("select * from system_Modules where name='" + ModuleName + "'"));
                        //��numberFormat���й���
                        if (property != null && configuration.SelectSingleNode("//NumberFormat") != null)
                        {
                            numberFormat = new NumberFormat(property, configuration.SelectSingleNode("//NumberFormat"));
                        }
                        //��warning���й���
                        if (property != null && configuration.SelectSingleNode("//Warning") != null)
                        {
                            warning = new Warning(property, configuration.SelectSingleNode("//Warning"));
                        }
                        //��authentication���й���
                        if (property != null && configuration.SelectSingleNode("//Authentication") != null)
                        {
                            authentication = new Authentication(property, configuration.SelectSingleNode("//Authentication"));
                        }
                        //��check���й���
                        if (property != null && configuration.SelectSingleNode("//Check") != null)
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