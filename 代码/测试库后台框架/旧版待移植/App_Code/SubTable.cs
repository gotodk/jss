using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using FMOP.MAKE;
/// <summary>
/// SubTable 的摘要说明
/// </summary>
namespace FMOP.STABLE //创建子表
{
    public class SubTable:Controls
    {
        public SubTable()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public string CaptionTable(string htmlStr,string caption)
        {
            StringBuilder tabcode = new StringBuilder();
            tabcode.Append("<table class=\"TableTitle\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");
            tabcode.Append("<tr height=\"33\"><td align=\"center\">" + caption + "</td></tr>");
            tabcode.Append("</table>");
            htmlStr = setAddressChar("</form>", tabcode.ToString(), htmlStr);
            return htmlStr;
        }

        public string CaptionTable(string htmlStr, string caption,string display)
        {
            StringBuilder tabcode = new StringBuilder();
            tabcode.Append("<table class=\"TableTitle\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\""+ display +"\">");
            tabcode.Append("<tr height=\"33\"><td align=\"center\">" + caption + "</td></tr>");
            tabcode.Append("</table>");
            htmlStr = setAddressChar("</form>", tabcode.ToString(), htmlStr);
            return htmlStr;
        }
        /// <summary>
        /// 添加表格头部
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public string TableHead(string htmlStr)
        {
            StringBuilder TableCode = new StringBuilder();
            TableCode.Append("<table style=\"width:100%\" cellpadding=\"0\" cellspacing=\"0\"");
            if (SubTable != "")
            {
                TableCode.Append(" id=\"" + SubTable + "\" class=\"TableTitle\"");
            }
            else
            {
                TableCode.Append(" class=\"FormView\"");
            }

            TableCode.Append(">\r\n");
            htmlStr = setAddressChar("</form>", TableCode.ToString(), htmlStr);

            return htmlStr;
        }

        /// <summary>
        /// 添加表格头部-隐藏
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public string TableHead(string htmlStr,string display)
        {
            StringBuilder TableCode = new StringBuilder();
            TableCode.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + display + "\"");
            if (SubTable != "")
            {
                TableCode.Append(" id=\"" + SubTable + "\" class=\"TableTitle\"");
            }
            else
            {
                TableCode.Append(" class=\"FormView\"");
            }

            TableCode.Append(">\r\n");
            htmlStr = setAddressChar("</form>", TableCode.ToString(), htmlStr);

            return htmlStr;
        }
        /// <summary>
        /// 添加TR
        /// </summary>
        /// <param name="htmlStr">在htmlStr中添加TR</param>
        /// <returns></returns>
        public string TRAdd(string htmlStr)
        {
            StringBuilder trCode = new StringBuilder();
            trCode.Append("<tr height=\"25\">\r\n");
            htmlStr = setAddressChar("</form>", trCode.ToString(), htmlStr);
            return htmlStr; 
        }

        public string subTRAdd(string htmlStr)
        {
            StringBuilder trCode = new StringBuilder();
            trCode.Append("<tr height=\"33\">\r\n");
            htmlStr = setAddressChar("</form>", trCode.ToString(), htmlStr);
            return htmlStr;
        }
        /// <summary>
        /// 给TD添加文本内容后再添加到Html文件中
        /// </summary>
        /// <param name="htmlStr">Html文件</param>
        /// <returns></returns>
        public string TDAdd(string htmlStr,string Text)
        {
            StringBuilder tdCode = new StringBuilder();

            tdCode.Append("<td nowrap");

            if (LengType == 2)
            {
                tdCode.Append(" colspan=\"" + 2 + "\" width=\"100%\"");
            }
            else
            {
                if (TdColsPan != "")
                {
                    tdCode.Append(" colspan=\"" + TdColsPan + "\"");
                }
                if (TdWidth != "")
                {
                    tdCode.Append(" width=\""+TdWidth +"\"");
                }
            }

            tdCode.Append(">\r\n");
            htmlStr = setAddressChar("</form>", tdCode.ToString(), htmlStr);

            htmlStr = setAddressChar("</form>", Text, htmlStr);

            htmlStr = setAddressChar("</form>", "</td>\r\n", htmlStr);

            return htmlStr;
        }

        /// <summary>
        /// 添加子表的TD
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public string subTdHead(string htmlStr, string Text)
        {
            StringBuilder tdCode = new StringBuilder();

            tdCode.Append("<td nowrap");

            if (LengType == 2)
            {
                tdCode.Append(" colspan=\"" + 2 + "\" width=\"100%\"");
            }
            else
            {
                if (TdColsPan != "")
                {
                    tdCode.Append(" colspan=\"" + TdColsPan + "\"");
                }
                if (TdWidth != "")
                {
                    tdCode.Append(" width=\"" + TdWidth + "\"");
                }
            }
            //tdCode.Append(">\r\n");
            tdCode.Append(" class=\"tdImage\">\r\n");
            htmlStr = setAddressChar("</form>", tdCode.ToString(), htmlStr);

            htmlStr = setAddressChar("</form>", Text, htmlStr);

            htmlStr = setAddressChar("</form>", "</td>\r\n", htmlStr);

            return htmlStr;
        }

        /// <summary>
        /// 添加TH
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public string THAdd(string htmlStr, string Text)
        {
            StringBuilder THCode = new StringBuilder();
            THCode.Append("<TH>" +Text +"</TH>");
            htmlStr = setAddressChar("</form>", THCode.ToString(), htmlStr);
            return htmlStr;
        }
        /// <summary>
        /// 添加TD标记开始部分
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public string TDAdd(string htmlStr)
        {
            StringBuilder tdCode = new StringBuilder();

            tdCode.Append("<td nowrap");

            if (LengType == 2)
            {
                tdCode.Append(" colspan=\"" + 2 + "\" width=\"100%\"");
            }
            else
            {
                if (TdColsPan != "")
                {
                    tdCode.Append(" colspan=\"" + TdColsPan + "\"");
                }
                if (TdWidth != "")
                {
                    tdCode.Append(" width=\"" + TdWidth + "\"");
                }
            }
            
            tdCode.Append(">\r\n");
            htmlStr = setAddressChar("</form>", tdCode.ToString(), htmlStr);
            return htmlStr;
        }

        /// <summary>
        /// 添加TD标记结束部分
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public string TDEnd(string htmlStr)
        {
            string tag = "</td>\r\n";
            htmlStr = setAddressChar("</form>", tag, htmlStr);
            return htmlStr;
        }
        /// <summary>
        /// 结束TR标记部分
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public string TREnd(string htmlStr)
        {
            string tag = "</tr>\r\n";
            htmlStr = setAddressChar("</form>", tag, htmlStr);
            return htmlStr;
        }

        /// <summary>
        /// 结束Table标记
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public string TableEnd(string htmlStr)
        {
            string tag = "</table>\r\n";
            htmlStr = setAddressChar("</form>", tag, htmlStr);
            return htmlStr;
        }

        /// <summary>
        /// 返回html代码，继承方法
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <param name="controlNode"></param>
        /// <returns></returns>
        public override string GetHtml(string htmlStr, System.Xml.XmlNode controlNode)
        {
            return ""; 
        }

        /// <summary>
        /// 创建DIV标记
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public string StartDiv(string htmlStr)
        {
            string divTag = "<div id=\"" + SubTable + "_div\" style=\"overflow:auto\">\r\n";
            htmlStr = setAddressChar("</form>", divTag, htmlStr);

            return htmlStr;
        }

        /// <summary>
        /// 结束DIV标记
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public string EndDiv(string htmlStr)
        {
            string divTag = "</div>\r\n";
            htmlStr = setAddressChar("</form>", divTag, htmlStr);
            return htmlStr;
        }
    }
}
