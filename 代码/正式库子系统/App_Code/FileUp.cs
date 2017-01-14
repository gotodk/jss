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
/// <summary>
/// FileUp 的摘要说明
/// </summary>
namespace FMOP.FU
{
    public class FileUp : Controls
    {
        public FileUp()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public FileUp(XmlNode itemlist, int Flag)
            : base(itemlist, Flag)
        {
        }

        /// <summary>
        /// 获取添加FileUP后的Html代码
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <param name="textNode"></param>
        /// <returns></returns>
        public override string GetHtml(string htmlStr, XmlNode textNode)
        {
            //解析Textbox 控件的XML属性
          //  ParseTextNode(textNode);
            htmlStr = MakeFileUp(htmlStr);
            return htmlStr;
        }

        /// <summary>
        /// 创建上传
        /// </summary>
        /// <returns></returns>
        public string MakeFileUp(string htmlStr)
        {
            StringBuilder FileUpHtml = new StringBuilder();
            string tagID = "spryfilefield" + Name;
            FileUpHtml.Append("\t<span id=\"" + tagID + "\">\r\n");

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
                    FileUpHtml.Append("\t<span id=\"Caption" + Name + "\">*" + Caption + "</span>\r\n");
                }
                else
                {
                    if (IsDisPlayF == true) //若为空并前台不显示，则不显示 刘杰 2010-09-20
                    {
                        FileUpHtml.Append("\t<span id=\"Caption" + Name + "\" style=\"display:none;\">" + Caption + "</span>\r\n");
                    }
                    else
                    {
                        //原算法
                        FileUpHtml.Append("\t<span id=\"Caption" + Name + "\">" + Caption + "</span>\r\n");
                    }
                    
                }
                if (IsDisPlayF == true) //若为空并前台不显示，则不显示 刘杰 2010-09-20
                {
                    FileUpHtml.Append("\t<input type=\"button\"   style=\"display:none;\" name=\"btnUp\" value=\"增加上传\" class=\"largebutton\" onclick=\"addFile('" + tagID + "','" + Name + "','" + CanNull.ToString().ToLower() + "')\" />\r\n");
                
                }
                else
                {
                    FileUpHtml.Append("\t<input type=\"button\" name=\"btnUp\" value=\"增加上传\" class=\"largebutton\" onclick=\"addFile('" + tagID + "','" + Name + "','" + CanNull.ToString().ToLower() + "')\" />\r\n");
                }
               // FileUpHtml.Append("\t<input type=\"button\" name=\"btnUp\" value=\"增加上传\" class=\"largebutton\" onclick=\"addFile('" + tagID + "','" + Name + "','" + CanNull.ToString().ToLower() + "')\" />\r\n");

                FileUpHtml.Append("<br/>");
            }

            //设置验证时显示错误信息
            if (IsDisPlayF == true) //若为空并前台不显示，则不显示 刘杰 2010-09-25
            {
                FileUpHtml.Append("\t<input type =\"file\" name=\"" + Name + "\" id=\"" + Name + "\" size=\"100%\" style=\"display:none;\" ");
            }
            else
            {
              //原算法
                FileUpHtml.Append("\t<input type =\"file\" name=\"" + Name + "\" id=\"" + Name + "\" size=\"100%\"");
            }

            FileUpHtml.Append(" onchange=\"checkUpFile('" + Name + "','" + CanNull.ToString().ToLower() + "');\"");

            //若OnBlur事件不为空
            if (OnBlur != "" && OnBlur != "()")
            {
                FileUpHtml.Append(" onblur=\"return" + OnBlur + ";\"");
            }

            FileUpHtml.Append("/>\r\n");
            if (HasModelButton == true)
            {
                if (SubTable == "")
                {
                   //原算法 FileUpHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "','600','450');\"/>\r\n");

                    #region //新算法，控制button的显示 刘杰2010-09-25
                    FileUpHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "','600','450');\"");               

                    if (CanNull == true && IsDisPlayF == true)   //可为空\前台不显示  刘杰2010-09-20
                    {

                        FileUpHtml.Append(" style=\"display:none;\"/>\r\n");

                    }
                    else
                    {
                        FileUpHtml.Append("/>\r\n");
                    }
                    #endregion
               
                
                
                }
                else
                {
                   //原算法 FileUpHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "&tableName=" + SubTable + "','600','450');\"/>\r\n");
                   //新算法 加入不受干扰的CName liujie 2010-09-27
                    FileUpHtml.Append("\t<input type=\"button\" name=\"openWin\" value=\"..\" class=\"button\" onclick=\"ShowDialog('MasterWin.aspx?moduleName=" + ModuleName + "&fieldName=" + Name + "&CfieldName=" + CName + "&tableName=" + SubTable + "','600','450');\"/>\r\n");
                }
            }

            FileUpHtml.Append("\t<span id=\"" + Name + "nullMsg\"  style=\"display:none\">&nbsp;&nbsp;上传文件不能为空.</span>\r\n");
            FileUpHtml.Append("\t<span id=\"" + Name + "exteMsg\"  style=\"display:none\">&nbsp;&nbsp;上传文件的扩展名不符合.</span>\r\n");
            FileUpHtml.Append("\t</span>\r\n");

            //添加控件
            htmlStr = setAddressChar("</form>", FileUpHtml.ToString(), htmlStr);
            //返回文本框信息
            return htmlStr;
        }

        /// 添加前台事件方法
        /// </summary>
        /// <param name="eventName">验证的事件名称</param>
        /// <param name="method">验证的事件方法</param>
        /// <param name="Id">验证的控件</param>
        /// <param name="type">验证的类型</param>
        /// <param name="empty">是否验证为空</param>
        /// <param name="length">最大长度</param>
        /// <returns></returns>
        public string AddjsEvent(string eventName, string method, string Id, int type, bool empty, int length)
        {
            StringBuilder eventString = new StringBuilder();
            eventString.Append(" " + eventName + "=\"return " + method + "('" + Name.ToString() + "','" + type + "','" + empty.ToString().ToLower() + "','" + Length + "','','');\"");
            return eventString.ToString();
        }

        /// <summary>
        /// 解析FileUP节点的XML信息
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
