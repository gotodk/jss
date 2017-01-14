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
using FMOP.XHelp;
using FMOP.Hidden;
/// <summary>
/// floatBox 的摘要说明
/// </summary>
namespace FMOP.FB
{
    public class FloatBox : Controls
    {
        public FloatBox()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public FloatBox(XmlNode itemlist, int Flag)
            : base(itemlist, Flag)
        {
        }
        /// <summary>
        /// 返回添加FloatBox后的Html代码.
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <param name="textNode"></param>
        /// <returns></returns>
        public override string GetHtml(string htmlStr, XmlNode textNode)
        {
            //解析Textbox 控件的XML属性
            ParseTextNode(textNode);
            htmlStr = MakeFloatBox(htmlStr);
            return htmlStr;
        }

        /// <summary>
        /// 添加FloatBox到Html代码中
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public string MakeFloatBox(string htmlStr)
        {
            StringBuilder FloatBoxHtml = new StringBuilder();
            string tagID = "sprytextfield" + Name.ToUpper();

            //开始
            FloatBoxHtml.Append("\t<span id=\"" + tagID + "\">\r\n");

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
                    FloatBoxHtml.Append("\t<span id=\"Caption" + Name + "\">*" + Caption + "</span>\r\n");
                }
                else
                {
                    FloatBoxHtml.Append("\t<span id=\"Caption" + Name + "\">" + Caption + "</span>\r\n");
                }
            }

            //添加标记
            FloatBoxHtml.Append("\t<input type=\"text\" name=\"" + Name + "\" id=\"" + Name + "\" maxlength=\""+ Length +"\"");

            //判断是否有默认值
            if (DefaultValue != "")
            {
                FloatBoxHtml.Append(" value=\"" + DefaultValue + "\"");
            }

            //是否有样式
            //if (Style != "")
            //{
            //    FloatBoxHtml.Append(" class=\"" + Style + "\"");
            //}
            if (SubTable != "")
            {
                FloatBoxHtml.Append(" class=\"inputSubTable\"");
            }
            else
            {
                FloatBoxHtml.Append(" class=\"input1\"");
            }
            //判读是否为只读
            if (ReadOnly == true)
            {
                FloatBoxHtml.Append("style=\"ime-mode:Disabled\" onkeypress=\"return false;\"\r\n");
            }

            //若OnBlur事件不为空
            if (OnBlur != "")
            {
                FloatBoxHtml.Append(" onblur=\"return " + OnBlur + ";\"");
            }

            //结束标记
            FloatBoxHtml.Append("/>\r\n");

            //在文本框后面，存在一个选择按钮
            if (HasModelButton == true)
            {
                if (SubTable == "")
                {
                    FloatBoxHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "','600','450');\"/>\r\n");
                }
                else
                {
                    FloatBoxHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"smallbutton\" onclick=\"ShowDialog('ChildWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "&tableName=" + SubTable + "','600','450');\"/>\r\n");
                }
            }

            //若不为空,则添加为空时的提示信息
            if (CanNull == false)
            {
                if (SubTable == "")
                {
                    FloatBoxHtml.Append("\t<span class=\"textfieldRequiredMsg\"> * </span>\r\n");
                    FloatBoxHtml.Append("<span class=\"textfieldInvalidFormatMsg\">小数</span>");
                    Eventhanding += "var " + tagID + " = new Spry.Widget.ValidationTextField(\"" + tagID + "\", \"real\", {validateOn:[\"blur\"],useCharacterMasking:true";

                    //判断是否存在最大值最小值
                    if (MinValue.ToString() != "")
                    {
                        FloatBoxHtml.Append("\t<span class=\"textfieldMinValueMsg\">*输入值小于所允许的最小值(" + MinValue.ToString() + ").</span>\r\n");
                        Eventhanding += ", minValue:" + MinValue.ToString();
                    }
                    if (MaxValue.ToString() != "")
                    {
                        FloatBoxHtml.Append("\t<span class=\"textfieldMaxValueMsg\">*输入值大于所允许的最大值(" + MaxValue.ToString() + ").</span>\r\n");
                        Eventhanding += ", maxValue:" + MaxValue.ToString();
                    }
                }
            }
            else
            {
                FloatBoxHtml.Append("<span class=\"textfieldInvalidFormatMsg\">小数</span>");
                Eventhanding += "var " + tagID + " = new Spry.Widget.ValidationTextField(\"" + tagID + "\", \"real\", {validateOn:[\"blur\"],isRequired:false,useCharacterMasking:true";

                //判断是否存在最大值最小值
                if (MinValue.ToString() != "" && MinValue.ToString() != "0")
                {
                    FloatBoxHtml.Append("\t<span class=\"textfieldMinValueMsg\">*输入值小于所允许的最小值(" + MinValue.ToString() + ").</span>\r\n");
                    Eventhanding += ", minValue:" + MinValue.ToString();
                }
                if (MaxValue.ToString() != "" && MaxValue.ToString() != "0")
                {
                    FloatBoxHtml.Append("\t<span class=\"textfieldMaxValueMsg\">*输入值大于所允许的最大值(" + MaxValue.ToString() + ").</span>\r\n");
                    Eventhanding += ", maxValue:" + MaxValue.ToString();
                }
            }

            if (Eventhanding != "")
            {
                Eventhanding += "});\r\n";
            }

            FloatBoxHtml.Append("\t</span>\r\n");

            //向html文件内容中添加控件
            htmlStr = setAddressChar("</form>", FloatBoxHtml.ToString(), htmlStr);

            //向html文件内容中添加事件
            htmlStr = setAddressChar("//-->", Eventhanding, htmlStr);

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
            if (textNode["maxValue"] != null)
            {
                MaxValue = XMLHelper.GetSingleFloat(textNode, "maxValue",float.MaxValue);
            }
            if (textNode["minValue"] != null)
            {
                MinValue = XMLHelper.GetSingleFloat(textNode, "minValue",float.MinValue);
            }
        }
    }
}
