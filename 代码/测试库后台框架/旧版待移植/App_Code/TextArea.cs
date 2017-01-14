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
using FMOP.XHelp;
using FMOP.MAKE;
using System.Xml;
using FMOP.Hidden;
/// <summary>
/// TextArea 的摘要说明
/// </summary>
namespace FMOP.TA
{
    public class TextArea : Controls
    {
        public TextArea()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public TextArea(XmlNode itemlist, int Flag)
            : base(itemlist, Flag)
        {
        }
        /// <summary>
        /// 获取产生TextArea后的HTML总代码。
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <param name="textNode"></param>
        /// <returns></returns>
        public override string GetHtml(string htmlStr, XmlNode textNode)
        {
            //解析Textbox 控件的XML属性
            ParseTextNode(textNode);
            htmlStr = MakeTextArea(htmlStr);
            return htmlStr;
        }

        /// <summary>
        /// 创建文本框
        /// </summary>
        /// <returns></returns>
        public string MakeTextArea(string htmlStr)
        {
            StringBuilder TextAreaHtml = new StringBuilder();
            string tagID = "sprytextarea" + Name.ToUpper();

            //开始
            TextAreaHtml.Append("\t<span id=\"" + tagID + "\" style= \"line-height:200%\" height=\"25\">\r\n");

            //若html代码为空,则退出
            if (htmlStr.Equals(""))
            {
                return "";
            }

            //若标题不为空
            if (!Caption.Equals(""))
            {
                if (CanNull == false)
                {
                    TextAreaHtml.Append("\t<span  style= \"line-height:200%\" id=\"Caption" + Name + "\">*" + Caption + "</span>\r\n");
                }
                else
                {
                    TextAreaHtml.Append("\t<span  style= \"line-height:200%\" id=\"Caption" + Name + "\">" + Caption + "</span>\r\n");
                }
            }

            if (SubTable == "")
            {
                TextAreaHtml.Append("<br/>\r\n");
            }
            //添加标记
            //TextAreaHtml.Append("\t<textarea id=\"" + Name + "\" name=\"" + Name + "\" cols=\"" + Cols + "\" rows=\"" + Rows + "\"");
            TextAreaHtml.Append("\t<textarea id=\"" + Name + "\" name=\"" + Name + "\"");

            //判断是否有默认值
            //if (DefaultValue != "")
            //{
            //    TextAreaHtml.Append(" value=\"" + DefaultValue + "\"");
            //}

            //是否有样式
            if (SubTable == "")
            {
                TextAreaHtml.Append(" class=\"inputTextArea\"");
            }
            else
            {
                TextAreaHtml.Append(" class=\"inputSubTable\"");
            }

            if (ReadOnly == true)
            {
                TextAreaHtml.Append("style= \"ime-mode:Disabled\" onkeypress=\"return false;\"");
            }


            //若OnBlur事件不为空
            if (OnBlur != "")
            {
                TextAreaHtml.Append(" onblur=\"return " + OnBlur + ";\"");
            }

            if (DefaultValue == "")
            {
                TextAreaHtml.Append("></textarea>\r\n");
            }
            else
            {
                TextAreaHtml.Append(">"+ DefaultValue +"</textarea>\r\n");
            }
            //判断后面是否存在开窗按钮
            if (HasModelButton == true)
            {
                if (SubTable == "")
                {
                    TextAreaHtml.Append("<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "','600','450');\"/>");
                }
                else
                {
                    TextAreaHtml.Append("<input type=\"button\" name=\"openWin\" value=\"..\" class=\"smallbutton\" onclick=\"ShowDialog('ChildWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "&tableName=" + SubTable + "','600','450');\"/>");
                }
            }

            //若不为空,则添加为空时的提示信息
            if (CanNull == false)
            {
                if (SubTable == "")
                {
                    TextAreaHtml.Append("\t<span class=\"textareaRequiredMsg\"> * </span>\r\n");   
                    Eventhanding = "var " + tagID + " = new Spry.Widget.ValidationTextarea(\"" + tagID + "\", {validateOn:[\"blur\"]});\r\n";
                }
             
            }
            TextAreaHtml.Append("\t</span>\r\n");
            //向html文件内容中添加控件
            htmlStr = setAddressChar("</form>", TextAreaHtml.ToString(), htmlStr);

            //若是新增状态向html文件内容中添加事件
            //if (OperatorFlag != "Update" && SubTable != "")
            //{
            //    htmlStr = setAddressChar("//-->", Eventhanding, htmlStr);
            //}
            if (SubTable == "")
            {
                if (CanNull == false)
                {
                    htmlStr = setAddressChar("//-->", Eventhanding, htmlStr);
                }
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
            //为属性写值
           // Rows = XMLHelper.GetSingleInt(textNode, "Rows",);
        }
    }
}
