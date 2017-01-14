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
/// Button 的摘要说明
/// </summary>
namespace FMOP.BUTTON
{
     public class ClientButton:Controls
    {
         private string submitJsEvent = string.Empty;
         public string SubmitJsEvent
         {
             set
             {
                 submitJsEvent = value;
             }
             get
             {
                 return submitJsEvent;
             }
         }

        string btnHtml = string.Empty;
        public ClientButton()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 产生普通控件Button
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public  string GetButton(string htmlStr)
        {
            btnHtml = "\t<input id=\"btnSubTable\" name=\"btnSubTable\" type =\"button\" value=\"保存\" class=\"button\"";
            if (JSClik != "") 
            {
                btnHtml += " onclick=\"" + JSClik + "\"";
            }
            btnHtml += "/>\r\n";

            btnHtml = setAddressChar("</form>", btnHtml, htmlStr);
            return btnHtml;
        }

         /// <summary>
         /// 创建submit按钮
         /// </summary>
         /// <param name="htmlStr"></param>
         /// <param name="name"></param>
         /// <param name="jsEvent"></param>
         /// <returns></returns>
         public string MakeSubmitButton(string htmlStr)
         {
             btnHtml = "\t<input type=\"submit\" name=\"" + Name  + "\" id=\"" + Name + "\" value=\"" + DefaultValue + "\"  class=\"button\"";
             if (JSClik != "")
             {
                 btnHtml += " onclick=\"" + JSClik + "\"";
             }
             btnHtml += "/>\r\n";
             btnHtml = setAddressChar("</form>", btnHtml, htmlStr);
             return btnHtml;
         }

        /// <summary>
        /// 返回Html代码
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
         public override string GetHtml(string htmlStr,XmlNode Node)
         {

             htmlStr = setAddressChar("</form>", btnHtml, htmlStr);
             return htmlStr;
         }
    }
}
