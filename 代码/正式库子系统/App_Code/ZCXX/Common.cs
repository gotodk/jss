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

/// <summary>
/// Common 的摘要说明
/// </summary>
namespace FMOP.ZCXX
{
    public class Common
    {
        public Common()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static string GetKeyNumber(string module, string field)
        {
            return DbHelperSQL.GetLastNumber(field, module);
        }
        /// <summary>
        /// 发送提醒
        /// </summary>
        public static void SendMsg(string module, string KeyNumber, string userName)
        {
            string role;

            Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
            if (wf.warning != null)
            {
                wf.warning.CreateWarningToRole(KeyNumber, 1, userName);
            }
        }
    }
}
