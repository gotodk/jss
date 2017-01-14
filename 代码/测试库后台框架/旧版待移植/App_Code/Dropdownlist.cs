using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using FMOP.MAKE;
using System.Xml;
using FMOP.XHelp;
using FMOP.Hidden;
/// <summary>
/// dropDownList 的摘要说明
/// </summary>
namespace FMOP.DDL
{
    public class Dropdownlist : Controls
    {
        public string strControl = "";
        public Dropdownlist()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public Dropdownlist(XmlNode itemlist, int Flag)
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
            htmlStr = MakeDropDownList(htmlStr);
            return htmlStr;
        }

        /// <summary>
        /// 创建DropDownList
        /// </summary>
        /// <returns></returns>
        public string MakeDropDownList(string htmlStr)
        {
            StringBuilder DropdownlistHtml = new StringBuilder();
            string tagID = "spryselect" + Name.ToUpper();

            //若html代码为空,则退出
            if (htmlStr.Equals(""))
            {
                return "";
            }

            //开始
            DropdownlistHtml.Append("\t<span id=\"" + tagID + "\">\r\n");

            //若标题不为空
            if (!Caption.Equals(""))
            {
                if (CanNull == false)
                {
                    DropdownlistHtml.Append("\t<span id=\"Caption" + Name + "\">*" + Caption + "</span>\r\n");
                }
                else
                {
                    DropdownlistHtml.Append("\t<span id=\"Caption" + Name + "\">" + Caption + "</span>\r\n");
                }
            }

            DropdownlistHtml.Append("\t<select name=\"" + Name + "\"");
            
            //判断是否只读
            if (ReadOnly == true)
            {
                DropdownlistHtml.Append(" disabled=\"disabled\"");
            }

            if (SubTable != "")
            {
                DropdownlistHtml.Append(" class =\"inputSubTable\"");
            }
            else
            {
                DropdownlistHtml.Append(" class =\"input1\"");
            }
            //若OnBlur事件不为空
            if (OnBlur != "" && OnBlur != "()")
            {
                DropdownlistHtml.Append(" onChange=\"return" + OnBlur + ";\"");
            }

            DropdownlistHtml.Append(">\r\n");

            //动态数据绑定
            //if (Choose != null && Choose.Count > 0)
            //{
            //    foreach (DictionaryEntry index in Choose)

            //        //如果设置了默认值，就设定它
            //        if (DefaultValue == index.Key.ToString())
            //        {
            //            DropdownlistHtml.Append("\t\t<option value=\"" + index.Key + "\" selected=\"selected\">" + index.Value + "</option>\r\n");
            //        }
            //        else
            //        {
            //            DropdownlistHtml.Append("\t\t<option value=\"" + index.Key + "\">" + index.Value + "</option>\r\n");
            //        }
            //}

            if (Choose != null && Choose.Count > 0)
            {
                for (int index = 0; index < Choose.Count; index++)
                {
                    if (DefaultValue == Choose[index].ToString())
                    {
                        DropdownlistHtml.Append("\t\t<option value=\"" + Choose[index].ToString() + "\" selected=\"selected\">" + Choose[index].ToString() + "</option>\r\n");
                    }
                    else
                    {
                        DropdownlistHtml.Append("\t\t<option value=\"" + Choose[index].ToString() + "\">" + Choose[index].ToString() + "</option>\r\n");
                    }
                }
            }
            //根据是否为空添加*
            DropdownlistHtml.Append("\t</select>\r\n");

            //不为空时，添加不为空的验证
            if ( CanNull == false )
            {
                if (SubTable == "")
                {
                    DropdownlistHtml.Append("\t<span class =\"selectRequiredMsg\"> * </span>\r\n");
                    Eventhanding = "var " + tagID + " = new Spry.Widget.ValidationSelect(\"" + tagID + "\", {validateOn:[\"blur\"]});\r\n";
                }
                else
                {
                    Eventhanding = "var " + tagID + " = new Spry.Widget.ValidationSelect(\"" + tagID + "\", {isRequired:false,validateOn:[\"blur\"]});\r\n";
                }
            }

            DropdownlistHtml.Append("\t</span>\r\n");

            //在文本框后面，存在一个选择按钮
            if (HasModelButton == true)
            {
                if (SubTable == "")
                {
                    DropdownlistHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "','400','300');\"/>\r\n");
                }
                else
                {
                    DropdownlistHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"smallbutton\" onclick=\"ShowDialog('ChildWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "&tableName=" + SubTable + "','400','300');\"/>\r\n");
                }
            }

            //向html文件内容中添加控件
            htmlStr = setAddressChar("</form>", DropdownlistHtml.ToString(), htmlStr);

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
            return htmlStr;
        }

        /// <summary>
        /// 解析Textbox控件的XML内容
        /// </summary>
        /// <param name="textNode"></param>
        public void ParseTextNode(XmlNode textNode)
        {
            //为共用的属性写值
            if (textNode["comboType"] != null)
            {
                if (textNode["comboType"].InnerXml == "static")
                {
                    //静态数据绑定
                    Choose = XMLHelper.GetArrayList(textNode, "items"); //XMLHelper.GetHashTable(textNode, "items");
                }
                else
                {
                    //执行动态绑定数据
                    Choose = XMLHelper.GetArrayList(textNode, "comboValueField", "comboSQL"); //XMLHelper.GetHashTable(textNode, "comboValueField", "comboSQL");
                }
            }

            if (textNode["length"] != null)
            {
                Length = XMLHelper.GetSingleInt(textNode, "length");
            }
        }
    }
}
