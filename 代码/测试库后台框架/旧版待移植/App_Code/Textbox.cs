using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using FMOP.MAKE;
using System.Xml;
using FMOP.XParse;
using FMOP.XHelp;
using FMOP.Hidden;

/// <summary>
/// textBox 的摘要说明
/// </summary>
namespace FMOP.TB
{
    public class Textbox : Controls
    {
        public Textbox()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public Textbox(XmlNode itemlist,int Flag):base(itemlist,Flag)
        {

        }

        /// <summary>
        /// 创建文本框
        /// </summary>
        /// <returns></returns>
        public override string GetHtml(string htmlStr, XmlNode textNode)
        {
            //解析Textbox 控件的XML属性
            ParseTextNode(textNode);
            htmlStr = MakeTextbox(htmlStr);
            return htmlStr;
        }

        /// <summary>
        /// 产生控件代码
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        private string MakeTextbox(string htmlStr)
        {
            StringBuilder TextboxHtml = new StringBuilder();
            string tagID = "sprytextfield" + Name.ToUpper();

            //开始
            TextboxHtml.Append("\t<span id=\"" + tagID + "\">\r\n");

            //若html代码为空,则退出
            if (htmlStr.Equals(""))
            {
                return "";
            }

            //若标题不为空
            if (!Caption.Equals(""))
            {
                //若是必输，则添加*号
                if ( CanNull == false)
                {
                    TextboxHtml.Append("\t<span id=\"Caption" + Name + "\">*" + Caption + "</span>\r\n");
                }
                else
                {
                    TextboxHtml.Append("\t<span id=\"Caption" + Name + "\">" + Caption + "</span>\r\n");
                }
            }

            //添加标记
            TextboxHtml.Append("\t<input type=\"text\" id =\"" + Name + "\" name=\"" + Name + "\" maxlength=\"" + Length + "\"");

            //判断是否有默认值
            if (DefaultValue != "")
            {
                TextboxHtml.Append(" value=\"" + DefaultValue + "\"");
            }

            //是否有样式
            if (Style != "")
            {
                TextboxHtml.Append(" class=\"" + Style + "\"");
            }
            if (SubTable == "")
            {
                TextboxHtml.Append(" class=\"input1\"");
            }
            else
            {
                TextboxHtml.Append(" class=\"inputSubTable\"");
            }

            //判读是否为只读
            if (ReadOnly == true)
            {
                TextboxHtml.Append(" style=\"ime-mode:Disabled\" onkeypress=\"return false;\"\r\n");
                //TextboxHtml.Append("readonly=\"readonly\"\r\n");
                //TextboxHtml.Append("onkeypress=\"return false;\"");
            }

            //若OnBlur事件不为空
            if (OnBlur != "")
            {
                TextboxHtml.Append(" onblur=\"return " + OnBlur + ";\"");
            }
            TextboxHtml.Append("/>\r\n");

            //若存在弹出窗口，则添加
            if (HasModelButton == true)
            {
                if (SubTable == "")
                {
                    TextboxHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "','600','450');\"/>\r\n");
                }
                else
                {
                    TextboxHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"smallbutton\" onclick=\"ShowDialog('ChildWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "&tableName=" + SubTable + "','600','450');\"/>\r\n");
                }
            }

            TextboxHtml.Append("\t <font color=\"red\" size=\"2\"><label id=\"" + Name +"label\"></label></font>");
            //若为子表,则不处理不为空的验证
            if (SubTable == "")
            {
                //若不为空,则添加为空时的提示信息
                if (CanNull == false)
                {
                    TextboxHtml.Append("\t<span class=\"textfieldRequiredMsg\"> * </span>\r\n");

                    Eventhanding = "var " + tagID + " = new Spry.Widget.ValidationTextField(\"" + tagID + "\", \"none\", {validateOn:[\"blur\"],useCharacterMasking:true});\r\n";
                }
                TextboxHtml.Append("\t</span>\r\n");
            }

            //向html文件内容中添加控件
            htmlStr = setAddressChar("</form>", TextboxHtml.ToString(), htmlStr);

            //向html文件内容中添加事件
            if (SubTable == "")
            {
                htmlStr = setAddressChar("//-->", Eventhanding, htmlStr);
            }

            //判断若是子表控件
            if (SubTable != "")
            {
                //添加隐藏控件;
                hidden hid = new hidden();
                hid.Name = Name;
                htmlStr = hid.GetHtml(htmlStr, null);
            }

            //返回文本框信息
            return htmlStr;
        }

        /// <summary>
        /// 解析Textbox控件的XML内容
        /// </summary>
        /// <param name="textNode"></param>
        public void ParseTextNode(XmlNode textNode)
        {
            //为共用的属性写值
            if (textNode["length"] != null)
            {
                Length = XMLHelper.GetSingleInt(textNode, "length");
            }
        }
    }
}
