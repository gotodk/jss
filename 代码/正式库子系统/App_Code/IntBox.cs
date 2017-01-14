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
/// IntBox 的摘要说明
/// </summary>
namespace FMOP.IB
{
    public class IntBox : Controls
    {
        public IntBox()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public IntBox(XmlNode itemlist, int Flag)
            : base(itemlist, Flag)
        {
        }

        /// <summary>
        /// 获取加入IntBox控件后，页面的总HTML代码
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <param name="textNode"></param>
        /// <returns></returns>
        public override string GetHtml(string htmlStr, XmlNode textNode)
        {
            //解析Textbox 控件的XML属性
            ParseTextNode(textNode);
            htmlStr = MakeIntBox(htmlStr);
            return htmlStr;
        }

        /// <summary>
        /// 创建整数文本框
        /// </summary>
        /// <returns></returns>
        public string MakeIntBox(string htmlStr)
        {
            StringBuilder IntBoxHtml = new StringBuilder();
            string tagID = "sprytextfield" + Name.ToUpper();

            //若html代码为空,则退出
            if (htmlStr.Equals(""))
            {
                return "";
            }

            //开始
            IntBoxHtml.Append("\t<span id=\"" + tagID + "\">\r\n");

            //若标题不为空
            if (!Caption.Equals(""))
            {
                if (CanNull == false)
                {
                    IntBoxHtml.Append("\t<span id=\"Caption" + Name + "\">*" + Caption + "</span>\r\n");
                }
                else
                {
                    if (IsDisPlayF == true) //若为空并前台不显示，则不显示 刘杰 2010-09-20
                    {
                        IntBoxHtml.Append("\t<span id=\"Caption" + Name + "\" style=\"display:none;\">" + Caption + "</span>\r\n");
                    }
                    else
                    {
                        IntBoxHtml.Append("\t<span id=\"Caption" + Name + "\">" + Caption + "</span>\r\n");
                    }
                    
                }
            }

            //添加标记
            IntBoxHtml.Append("\t<input type=\"text\" id =\"" + Name + "\" name=\"" + Name + "\" maxlength=\""+ Length +"\"");

            //判断是否有默认值
            if (DefaultValue != "")
            {
                IntBoxHtml.Append(" value=\"" + DefaultValue + "\"");
            }

            //控件的前台显示控制 刘杰2010-09-10
            if (!Caption.Equals(""))
            {
                if (CanNull == true)   //可为空
                {
                    if (IsDisPlayF == true) //可前台不显示
                    {
                        IntBoxHtml.Append(" style=\"display:none;\"");
                    }
                }
            }

            //是否有样式
            //if (Style != "")
            //{
            //    IntBoxHtml.Append(" class=\"" + Style + "\"");
            //}
            if (SubTable == "")
            {
                IntBoxHtml.Append(" class=\"input1\"");
            }
            else
            {
                IntBoxHtml.Append(" class=\"inputSubTable\"");
            }
            IntBoxHtml.Append(" style=\"ime-mode:Disabled\"");
            //判读是否为只读
            if (ReadOnly == true)
            {
                IntBoxHtml.Append(" style=\"ime-mode:Disabled\" ondragstart='return false' onselectstart ='return false' onselect='document.selection.empty();return false;' onpaste='return false'  oncopy='document.selection.empty();return false;' onbeforecopy='return false' onmouseup='document.selection.empty();return false;' onkeypress=\"return false;\"\r\n");
            }

            //若OnBlur事件不为空
            if (OnBlur != "")
            {
                IntBoxHtml.Append(" onblur=\"return " + OnBlur + ";\"");
            }

            IntBoxHtml.Append("/>\r\n");

            //在文本框后面，存在一个选择按钮
            if (HasModelButton == true)
            {
                if (SubTable == "")
                {

                    //原算法备份IntBoxHtml.Append("<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "','600','450');\"/>");
                    //  //可前台不显示  刘杰2010-09-20
                    IntBoxHtml.Append("<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "','600','450');\"");
                    if (CanNull == true && IsDisPlayF == true)   //可为空\前台不显示  刘杰2010-09-20
                    {

                        IntBoxHtml.Append(" style=\"display:none;\"/>");

                    }
                    else
                    {
                        IntBoxHtml.Append("/>");
                    }
                   
                       
                        
                 }
                else
                {
                    //原算法 IntBoxHtml.Append("<input type=\"button\" name=\"openWin\" value=\"..\" class=\"smallbutton\" onclick=\"ShowDialog('ChildWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "&tableName=" + SubTable + "','600','450');\"/>");
                    //新算法 加入不受干扰的CName liujie 2010-09-27
                    IntBoxHtml.Append("<input type=\"button\" name=\"openWin\" value=\"..\" class=\"smallbutton\" onclick=\"ShowDialog('ChildWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "&CfieldName=" + CName + "&tableName=" + SubTable + "','600','450');\"/>");
                }
            }

            //若不为空,则添加为空时的提示信息
            if (CanNull == false)
            {

                if (SubTable == "")
                {
                    IntBoxHtml.Append("\t<span class=\"textfieldRequiredMsg\"> * </span>\r\n");
                    IntBoxHtml.Append("<span class=\"textfieldInvalidFormatMsg\">整数</span>");
                    Eventhanding += "var " + tagID + " = new Spry.Widget.ValidationTextField(\"" + tagID + "\", \"integer\", {validateOn:[\"blur\"],useCharacterMasking:true";

                    //判断
                    if (MinValue.ToString() != "")
                    {
                        IntBoxHtml.Append("\t<span class=\"textfieldMinValueMsg\">*输入值小于所允许的最小值(" + Convert.ToInt32(MinValue) + ").</span>\r\n");
                        Eventhanding += ", minValue:" + Convert.ToInt32(MinValue);
                    }
                    if (MaxValue.ToString() != "")
                    {
                        IntBoxHtml.Append("\t<span class=\"textfieldMaxValueMsg\">*输入值大于所允许的最大值(" + Convert.ToInt32(MaxValue) + ").</span>\r\n");

                        Eventhanding += ", maxValue:" + Convert.ToInt32(MaxValue);
                    }
                }
            }
            else
            {
                IntBoxHtml.Append("<span class=\"textfieldInvalidFormatMsg\">整数</span>");
                Eventhanding += "var " + tagID + " = new Spry.Widget.ValidationTextField(\"" + tagID + "\", \"integer\", {validateOn:[\"blur\"],isRequired:false,useCharacterMasking:true";

                //判断
                if (MinValue.ToString() != "" && MaxValue.ToString() != "0")
                {
                    IntBoxHtml.Append("\t<span class=\"textfieldMinValueMsg\">*输入值小于所允许的最小值(" + Convert.ToInt32(MinValue) + ").</span>\r\n");
                    Eventhanding += ", minValue:" + Convert.ToInt32(MinValue);
                }
                if (MaxValue.ToString() != "" && MaxValue.ToString() != "0")
                {
                    IntBoxHtml.Append("\t<span class=\"textfieldMaxValueMsg\">*输入值大于所允许的最大值(" + Convert.ToInt32(MaxValue) + ").</span>\r\n");

                    Eventhanding += ", maxValue:" + Convert.ToInt32(MaxValue);
                }
            }

            if (Eventhanding != "")
            {
                Eventhanding += "});\r\n";
            }

            IntBoxHtml.Append("\t</span>\r\n");

            //向html文件内容中添加控件
            htmlStr = setAddressChar("</form>", IntBoxHtml.ToString(), htmlStr);

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
                MaxValue = XMLHelper.GetSingleInt(textNode, "maxValue",int.MaxValue);
            }
            if (textNode["minValue"] != null)
            {
                MinValue = XMLHelper.GetSingleInt(textNode, "minValue",int.MinValue);
            }
        }
    }
}
