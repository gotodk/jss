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
/// DateControl 的摘要说明
/// </summary>
namespace FMOP.DATE
{
    public class DateControl : Controls
    {
        public DateControl()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public DateControl(XmlNode itemlist, int Flag)
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
        //  ParseTextNode(textNode);
            htmlStr = MakeDate(htmlStr);
            return htmlStr;
        }

        /// <summary>
        /// 创建日期控件
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public string MakeDate(string htmlStr)
        {
            StringBuilder DateHtml = new StringBuilder();
            string tagID = "sprytextfield" + Name.ToUpper();

            //开始
            DateHtml.Append("\t<span id=\"" + tagID + "\">\r\n");

            //若html代码为空,则退出
            if (htmlStr.Equals(""))
            {
                return "";
            }

            //若标题不为空
            if (!Caption.Equals(""))
            {
                if ( CanNull == false)
                {
                    DateHtml.Append("\t<span id=\"Caption" + Name + "\">*" + Caption + "</span>\r\n");
                }
                else
                {
                    if (IsDisPlayF == true) //若为空并前台不显示，则不显示 刘杰 2010-09-20
                    {
                        DateHtml.Append("\t<span id=\"Caption" + Name + "\" style=\"display:none;\">" + Caption + "</span>\r\n");
                    }
                    else
                    {
                        DateHtml.Append("\t<span id=\"Caption" + Name + "\">" + Caption + "</span>\r\n");
                    }
                    
                }
            }
            if (ReadOnly == true)
            {
                DateHtml.Append("\t<input type =\"text\" id=\"" + Name + "\" name=\"" + Name + "\" maxlength=\"" + Length + "\"  size=\"7\"");
            }
            else
            {
                DateHtml.Append("\t<input type =\"text\" id=\"" + Name + "\" name=\"" + Name + "\" maxlength=\"" + Length + "\" onfocus = \"setday(this)\" size=\"7\"");
            }
            
            //DateHtml.Append("\t<input type =\"text\" id=\"" + Name + "\" name=\"" + Name + "\" maxlength=\"" + Length + "\"  size=\"7\"");
            DateHtml.Append(" class=\"input1\"");

            if (DefaultValue != "")
            {
                DateHtml.Append(" value=\"" + Convert.ToDateTime(DefaultValue).ToString("yyyy-MM-dd") + "\"");
            }
            DateHtml.Append(" style=\"ime-mode:Disabled\"");

            DateHtml.Append(" onblur=\" ");
            //若OnBlur事件不为空
            if (OnBlur != "" && OnBlur != "()")
            {
                //DateHtml.Append("(" + OnBlur + " && closeDateWindow());\" ");
                DateHtml.Append("closeDateWindow(); " + OnBlur + ";\" ");
            }
            else
            {
                DateHtml.Append("return closeDateWindow();\" ");
            }

            if (ReadOnly == true)
            {
              
                DateHtml.Append(" style=\"ime-mode:Disabled\" ondragstart='return false' onselectstart ='return false' onselect='document.selection.empty();return false;' onpaste='return false'  oncopy='document.selection.empty();return false;' onbeforecopy='return false' onmouseup='document.selection.empty();return false;' onkeypress=\"return false;\"\r\n");
            }
            //控件的前台显示控制 刘杰2010-09-10
            if (!Caption.Equals(""))
            {
                if (CanNull == true)   //可为空
                {
                    if (IsDisPlayF == true) //可前台不显示
                    {
                        DateHtml.Append(" style=\"display:none;\"");
                    }
                }

            }

            DateHtml.Append("/>\r\n");


            if (HasModelButton == true)
            {
                if (SubTable == "")
                {
                  //原代码  DateHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "','400','300');\"/>\r\n");
                    //可为空\前台不显示  刘杰2010-09-20
                    DateHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "','400','300');\"");             
                    if (CanNull == true && IsDisPlayF == true)   
                    {

                        DateHtml.Append(" style=\"display:none;\"/>\r\n");

                    }
                    else
                    {
                        DateHtml.Append("/>\r\n");
                    }
                }
                else
                {
                    //原算法 DateHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"smallbutton\" onclick=\"ShowDialog('ChildWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "&tableName=" + SubTable + "','400','300');\"/>\r\n");
                    //新算法 加入不受干扰的CName liujie 2010-09-27
                    DateHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"smallbutton\" onclick=\"ShowDialog('ChildWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "&CfieldName=" + CName + "&tableName=" + SubTable + "','400','300');\"/>\r\n");
                }
               
            }

            //若为子表,则不处理不为空的验证
            if (SubTable == "")
            {
                //若不为空,则添加为空时的提示信息
                if (CanNull == false)
                {
                    DateHtml.Append("\t<span class=\"textfieldRequiredMsg\"> * </span>\r\n");
                    Eventhanding = "var " + tagID + " = new Spry.Widget.ValidationTextField(\"" + tagID + "\", \"date\", {validateOn:[\"blur\"],format:\"yyyy-mm-dd\"});\r\n";
                }
                else
                {
                    Eventhanding = "var " + tagID + " = new Spry.Widget.ValidationTextField(\"" + tagID + "\", \"date\", {validateOn:[\"blur\"],format:\"yyyy-mm-dd\",isRequired:false});\r\n";
                }
                DateHtml.Append("\t<span class=\"textfieldInvalidFormatMsg\">日期格式无效</span>\r\n");       
                DateHtml.Append("\t</span>\r\n");
            }

            //向html文件内容中添加控件
            htmlStr = setAddressChar("</form>", DateHtml.ToString(), htmlStr);

            //向html文件内容中添加事件
            if (SubTable == "" && Eventhanding != "")
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
            Name = XMLHelper.GetSingleString(textNode, "name");
            Caption = XMLHelper.GetSingleString(textNode, "caption");
            CanNull = XMLHelper.GetSingleBool(textNode, "canNull", false);
            IsInListTable = XMLHelper.GetSingleBool(textNode, "isInListTable", true);
            Style = XMLHelper.GetSingleString(textNode, "Style", "");

            if (textNode["OnBlur"] != null)
            {
                OnBlur = XMLHelper.GetSingleString(textNode, "OnBlur") + "()"; //例: validate();
            }

            ReadOnly = XMLHelper.GetSingleBool(textNode, "readonly", false);

            //是否有弹出窗口，和弹出窗口的SQL语句
            HasModelButton = XMLHelper.GetSingleBool(textNode, "hasModelButton", false);
        }
    }
}