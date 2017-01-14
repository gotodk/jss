using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace FMOP.Module
{
    /// <summary>
    /// TabTool 的摘要说明
    /// </summary>
    public class TabTool
    {
        public TabTool()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static string getTitle(string module)
        {
            string sql = "select title from system_modules where name= '" + module + "'";
            object ob = FMOP.DB.DbHelperSQL.GetSingle(sql);
            if (ob == null)
                return "";
            if (ob != DBNull.Value)
            {
                return ob.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
