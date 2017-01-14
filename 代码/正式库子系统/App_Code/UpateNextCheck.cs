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
using System.Security;
using System.Data.SqlClient;
using System.Xml;
namespace FMOP.UNC
{

    /// <summary>
    /// 更新审核岗位
    /// </summary>

    public class UpateNextCheck
    {
        /// <summary>
        /// 更新审核岗位
        /// </summary>
        /// <param name="ModuleName">模块名称</param>
        /// <param name="Number">单号</param>
        /// <param name="nextCheckJobName">要审核的岗位名称</param>
        /// <param name="CheckState">是否通过审核(0:审批未完成或,1:审批完成)</param>
        /// <returns></returns>
        public static  bool UpateNextChecker(string ModuleName, string Number, string nextCheckJobName, int CheckState)
        {
            bool isfinish = false;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ");
                strSql.Append(ModuleName);
                strSql.Append(" set ");
                strSql.Append("NextChecker=@NextChecker");
                strSql.Append("CheckState=@CheckState,");
                strSql.Append(" where Number=@Number ");

                SqlParameter[] parameters = {
					new SqlParameter("@NextChecker", SqlDbType.VarChar,50),
					new SqlParameter("@CheckState", SqlDbType.Bit),
                    new SqlParameter ("@Number",SqlDbType.VarChar,50)};
                parameters[0].Value = GetNextChecker(ModuleName,nextCheckJobName);
                parameters[1].Value = CheckState;
                parameters[2].Value = Number;
                DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                isfinish = true;
            }
            catch
            {
                isfinish = false;
            }
            return isfinish;
        }


        public static string GetNextChecker(string module,string jobName)
        {
            XmlNodeList xlist = FMOP.XParse.xmlParse.XmlGetNode(module,"//WorkFlowModule/Check/Check-Role");
            string checker = "";
            if (xlist != null)
            {
                string[] roleName = new string[xlist.Count];
                for (int i = 0; i < xlist.Count; i++)
                {
                    if (FMOP.XHelp.XMLHelper.GetSingleString(xlist[i], "roleName", "").Equals(jobName))
                    {
                        if (i <= xlist.Count - 2)
                        {
                            checker = FMOP.XHelp.XMLHelper.GetSingleString(xlist[++i], "roleName", "");
                            break;
                        }
                        else
                        {
                            checker = FMOP.XHelp.XMLHelper.GetSingleString(xlist[i], "roleName", "");
                            break;
                        }
                    }
                }
            }
            else
            {
                return "";
            }
            return checker;
        }
    }
}