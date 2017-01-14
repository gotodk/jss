using System;
using System.Text;
namespace Hesion.Brick.Core
{

    /// <summary>
    /// 显示消息提示对话框。
    /// </summary>
    public abstract class MessageBox
    {
        protected MessageBox()
        {
        }

        /// <summary>
        /// 显示消息提示对话框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void Show(System.Web.UI.Page page, string msg)
        {
            //方法过时
            //page.RegisterStartupScript("message", "<script language='javascript' defer>alert('" + msg.ToString() + "');</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>alert('" + msg.ToString() + "');</script>");
        }

        /// <summary>
        /// 控件点击 消息确认提示框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void ShowConfirm(System.Web.UI.WebControls.WebControl control, string msg)
        {
            //control.Attributes.Add("onClick","if (!window.confirm('"+msg+"')){return false;}");
            control.Attributes.Add("onclick", "return confirm('" + msg + "');");
        }

        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public static void ShowAndRedirect(System.Web.UI.Page page, string msg, string url)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script language='javascript'>");
            Builder.AppendFormat("alert('{0}');", msg);
            Builder.AppendFormat("window.location='{0}'", url);
            Builder.Append("</script>");
            //方法过时
            page.Response.Write(Builder.ToString());
            page.Response.End();
            //page.RegisterStartupScript("message", Builder.ToString());
           // page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());

        }
        /// <summary>
        /// 输出自定义脚本信息
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="script">输出脚本</param>
        public static void ResponseScript(System.Web.UI.Page page, string script)
        {
            //本方法过时，被下行方法代替
            //page.RegisterStartupScript("message", "<script language='javascript' defer>" + script + "</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>" + script + "</script>");
            
        }




        /// <summary>
        /// 显示提示信息并退回到上一页
        /// </summary>
        /// <param name="msg">提示信息内容</param>
        public static void ShowAlertAndBack(System.Web.UI.Page page,string msg)
        {
            StringBuilder sb = new StringBuilder("<script lanugae=\"javascript\">");
            sb.Append("\n");
            sb.Append("alert('");
            sb.Append(msg);
            sb.Append("');history.back();");
            sb.Append("\n");
            sb.Append("</script>");
            page.Response.Write(sb.ToString());
            page.Response.End();
        }

    }
}
