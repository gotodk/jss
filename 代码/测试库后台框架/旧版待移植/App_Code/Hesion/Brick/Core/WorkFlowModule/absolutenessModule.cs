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
/// <summary>
/// absolutenessModule 的摘要说明
/// </summary>
namespace Hesion.Brick.Core.WorkFlow
{
    public class absolutenessModule
    {
        public bool CanAdd = false;

        public absolutenessModule()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public absolutenessModule(string module)
        {
            IsCanAdd(module);
        }

        public void IsCanAdd(string module)
        {
            string cmdText = "";
            string countNo = "";
            cmdText = "select count(*) from absolutenessModule where moduleName='" + module + "'";
            SqlDataReader dr = DbHelperSQL.ExecuteReader(cmdText);
            if (dr.HasRows)
            {
                dr.Read();
                countNo = dr[0].ToString();
            }
            dr.Close();

            if (countNo == "0")
            {
                CanAdd = false;
            }
            else
            {
                CanAdd = true;
            }
        }
    }
}
