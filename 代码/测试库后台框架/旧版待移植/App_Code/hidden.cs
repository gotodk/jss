using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.MAKE;
using System.Xml;
/// <summary>
/// hidden 的摘要说明
/// </summary>
namespace FMOP.Hidden
{
    public class hidden : Controls
    {
        string hidCode = "";
        public hidden()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 实现创建控件的方法
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public void MakeHidden(string htmlStr)
        {
            hidCode = "\t<input type=\"hidden\" name=\"" + Name + "_hid\" id=\"" + Name + "_hid\" />\r\n";
            hidCode = setAddressChar("</form>", hidCode, htmlStr);
        }

        /// <summary>
        /// 返回Html代码
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public override string GetHtml(string htmlStr, XmlNode Node)
        {
            MakeHidden(htmlStr);
            return hidCode;
        }
    }
}