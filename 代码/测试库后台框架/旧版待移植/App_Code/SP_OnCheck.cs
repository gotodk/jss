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
using FMOP.XParse;
using System.Data.SqlClient;
using System.Xml;


namespace FMOP.SP_OnChecks
{
    /// <summary>
    /// SP_OnCheck 的摘要说明
    /// </summary>
    public class SP_OnCheck
    {
        public SP_OnCheck()
        {

        }
        /// <summary>
        /// 执行审批后的存储过程
        /// </summary>
        /// <param name="modeName">模块名称</param>
        /// <param name="KeyNumber">单号</param>
        public static void do_oncheck(string modeName, string KeyNumber)
        {
            try
            {
                string sp_Name = "";
                string sqlCmd = "SELECT Configuration.query('/WorkFlowModule') from system_Modules where name ='" + modeName + "'";
                string strXml = DbHelperSQL.GetSingle(sqlCmd).ToString();
                XmlNode resultNode = xmlParse.XmlFirstNode(strXml, "//SP_OnCheck");
                if (resultNode != null)
                {
                    sp_Name = xmlParse.XmlFirstNode(strXml, "//SP_OnCheck").InnerXml;
                }
                if (sp_Name != "")
                {
                    SqlParameter[] spParameter = new SqlParameter[1];
                    spParameter[0] = new SqlParameter("@Number", SqlDbType.VarChar, 20, "Number");
                    spParameter[0].Value = KeyNumber;
                    //spParameter[0].Direction = ParameterDirection.Input;
                    int aa;
                    DbHelperSQL.RunProcedure(sp_Name,spParameter); 
                    
                    //spInfo = MakeObj(sp_Name, spParameter, CommandType.StoredProcedure);
                }
                //return spInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}