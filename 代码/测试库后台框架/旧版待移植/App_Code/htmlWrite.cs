using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Xml;
using FMOP.XParse;
using FMOP.XHelp;

namespace FMOP.Module
{
    /// <summary>
    /// 根据配置信息动态写html页面
    /// 修改：gzl
    /// date：07.12.14
    /// </summary>
    public class htmlWrite
    {
        public htmlWrite()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //

        }


        /// <summary>
        /// 字符型 下拉列表 选择查询属性 写table 用于查询
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public static string addStringSelect(string name)
        {
            return "<td style=\"text-align:center\" width=\"25%\"><select name=\"sel" + name + "\" class=\"input1\" style=\"width: 100px\"><option value=\"=\">等于</option><option value=\"%%\">包含</option><option value=\"a%\">以此开头</option><option value=\"%a\">以此结尾</option></select></td>";
        }


        /// <summary>
        /// 数型和time型 下拉列表 选择查询属性 写table 用于查询
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public static string addOtherSelect(string name)
        {
            return "<td style=\"text-align:center\" width=\"25%\"><select name=\"sel" + name + "\" class=\"input1\" style=\"width: 100px\"><option value=\"=\">等于</option><option value=\">\">大于</option><option value=\"<\">小于</option><option value=\">=\">大于等于</option><option value=\"<=\">小于等于</option></select></td>";
        }


        /// <summary>
        /// 写tableButton 用于查询table的隐现
        /// </summary>
        /// <returns></returns>
        public static string addSelectButton()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table id=\"one\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td width=\"90%\" style=\"text-align:right;\"></td><td width=\"10%\" style=\"border:solid 1px;text-align:center\">");
            sb.Append("<a href=\"#\" onclick=\"disp()\" style=\"font-family: 宋体; text-decoration: none;color:#336699\">条件查询</a>");
            sb.Append("</td></tr></table>");
            return sb.ToString();
        }


        /// <summary>
        /// 圆角上边
        /// </summary>
        /// <returns></returns>
        public static string addRingTop()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<TABLE style=\"TABLE-LAYOUT: fixed\" height=\"28\" cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
            sb.Append("  <TBODY>");
            sb.Append("    <TR height=\"3\" width=\"100%\">");
            sb.Append("      <TD>");
            sb.Append("        <TABLE style=\"TABLE-LAYOUT: fixed\" height=\"3\" cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
            sb.Append("          <TBODY>");
            sb.Append("            <TR height=\"1\">");
            sb.Append("              <TD width=\"1\"></TD>");
            sb.Append("              <TD width=\"1\"></TD>");
            sb.Append("              <TD width=\"1\"></TD>");
            sb.Append("              <TD style=\"background-color:#908a47\"></TD>");
            sb.Append("              <TD width=\"1\"></TD>");
            sb.Append("              <TD width=\"1\"></TD>");
            sb.Append("              <TD width=\"1\"></TD>");
            sb.Append("            </TR>");
            sb.Append("            <TR height=\"1\">");
            sb.Append("              <TD></TD> ");
            sb.Append("              <TD style=\"background-color:#908a47\" colSpan=\"2\"></TD>");
            sb.Append("              <TD style=\"background-color:#f7f8f9\"></TD>");
            sb.Append("              <TD style=\"background-color:#908a47\" colSpan=\"2\"></TD>");
            sb.Append("              <TD></TD>");
            sb.Append("            </TR>");
            sb.Append("            <TR height=\"1\">");
            sb.Append("              <TD></TD>");
            sb.Append("              <TD style=\"background-color:#908a47\"></TD>");
            sb.Append("              <TD style=\"background-color:#f7f8f9\" colSpan=\"3\"></TD>");
            sb.Append("              <TD style=\"background-color:#908a47\"></TD>");
            sb.Append("              <TD></TD>");
            sb.Append("            </TR>");
            sb.Append("          </TBODY>");
            sb.Append("        </TABLE>");
            sb.Append("      </TD>");
            sb.Append("    </TR>");
            sb.Append("    <TR>");
            sb.Append("      <TD>");
            sb.Append("        <TABLE style=\"TABLE-LAYOUT: fixed\" height=\"100%\" width=\"100%\" cellSpacing=\"0\" cellPadding=\"0\" border=\"0\">");
            sb.Append("          <TBODY>");
            sb.Append("            <TR>");
            sb.Append("              <TD width=\"1\" style=\"background-color:#908a47\"></TD>");
            sb.Append("              <TD id=\"oINNER\" style=\"background-color:#f7f8f9\">");
            return sb.ToString();
        }   


        /// <summary>
        /// 圆角 下边
        /// </summary>
        /// <returns></returns>
        public static string addRingFoot()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("              </TD>");
            sb.Append("              <TD width=\"1\" style=\"background-color:#908a47\"></TD>");
            sb.Append("            </TR>");
            sb.Append("          </TBODY>");
            sb.Append("        </TABLE>");
            sb.Append("      </TD>");
            sb.Append("    </TR>");
            sb.Append("    <TR height=\"3\" width=\"100%\">");
            sb.Append("      <TD>");
            sb.Append("        <TABLE style=\"TABLE-LAYOUT: fixed\" height=\"3\" cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
            sb.Append("          <TBODY>");
            sb.Append("            <TR height=\"1\">");
            sb.Append("              <TD width=\"1\"></TD>");
            sb.Append("              <TD width=\"1\" style=\"background-color:#908a47\"></TD>");
            sb.Append("              <TD width=\"1\" style=\"background-color:#f7f8f9\"></TD>");
            sb.Append("              <TD style=\"background-color:#f7f8f9\"></TD>");
            sb.Append("              <TD width=\"1\" style=\"background-color:#f7f8f9\"></TD>");
            sb.Append("              <TD width=\"1\" style=\"background-color:#908a47\"></TD>");
            sb.Append("              <TD width=\"1\"></TD>");
            sb.Append("            </TR>");
            sb.Append("            <TR height=\"1\">");
            sb.Append("              <TD></TD>");
            sb.Append("              <TD style=\"background-color:#908a47\" colSpan=\"2\"></TD>");
            sb.Append("              <TD style=\"background-color:#f7f8f9\"></TD>");
            sb.Append("              <TD style=\"background-color:#908a47\" colSpan=\"2\"></TD>");
            sb.Append("              <TD></TD>");
            sb.Append("            </TR>");
            sb.Append("            <TR height=\"1\">");
            sb.Append("              <TD colSpan=\"3\"></TD>");
            sb.Append("              <TD style=\"background-color:#908a47\"></TD>");
            sb.Append("              <TD colSpan=\"3\"></TD>");
            sb.Append("            </TR>");
            sb.Append("          </TBODY>");
            sb.Append("        </TABLE>");
            sb.Append("      </TD>");
            sb.Append("    </TR>");
            sb.Append("  </TBODY>");
            sb.Append("</TABLE>");
            return sb.ToString();
        }


        /// <summary>
        /// 添加页面标题
        /// </summary>
        /// <param name="title">标题内容</param>
        /// <returns></returns>
        public static string addTitleTable(string title)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" id=\"RoleTb\">");
            sb.Append("  <tr>");
            sb.Append("    <td height=\"50\" class=\".aspTitle\">");
            sb.Append("      <strong style=\"font-size:18px;color:#111111\">" + title + "</strong>");
		    sb.Append("    </td>");
            sb.Append("  </tr>");
            sb.Append("</table>");
            return sb.ToString();
        }


        /// <summary>
        /// table的头
        /// </summary>
        /// <returns></returns>
        public static string addTableHeader(string Tableid)
        {
            return "<table id=\"" + Tableid + "\" width=\"100%\" border=\"0px\" cellpadding=\"0\" cellspacing=\"0\" >";
        }

        /// <summary>
        /// table的头 无边框
        /// </summary>
        /// <returns></returns>
        public static string addTableHeader2()
        {
            return "<table id=\"selecttable\" width= \"100%\" border=\"0px\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color:#f7f8f9\">";
        }


        /// <summary>
        /// 添加鼠标事件
        /// </summary>
        /// <param name="Tableid">table的di属性值</param>
        /// <returns></returns>
        public static string addTableScript(string Tableid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\" language=\"JavaScript\">");
            //sb.Append("<!--");
            sb.Append("	tigra_tables('" + Tableid + "', 1, 0, '#F3F3F3', '#ffffff', '#F1F6F9', '#E0EFF2');");
            //sb.Append("-->");
            sb.Append("</script>");
            return sb.ToString();
        }


        /// <summary>
        /// table 结束
        /// </summary>
        /// <returns></returns>
        public static string addTableFooter()
        {
            return "</table>";
        }


        /// <summary>
        /// 生成查询table
        /// </summary>
        /// <param name="fieldType">字段类型</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="caption">字段标题</param>
        /// <returns></returns>
        public static string addSelectTable(string[] fieldType,string[] fieldName,string[] caption)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(addRingTop());
            sb.Append(addTableHeader2());
            for (int i = 0; i < fieldName.Length; i++)
            {
                if (fieldName[i] == null || fieldName[i].Equals(string.Empty))
                {
                    break;
                }
                else
                {
                    sb.Append("<tr height=\"28\">");
                    sb.Append("<td width=\"15%\" style=\"font-size:14px;color:black; text-align:right\">" + caption[i] + "：</td>");
                    if (fieldType[i].Equals("System.String"))
                    {
                        sb.Append(addStringSelect(fieldName[i]));
                    }
                    else
                    {
                        sb.Append(addOtherSelect(fieldName[i]));
                    }
					sb.Append("<td width=\"25%\" style=\"text-align:left;\"><input type=\"text\" class=\"input1\" name=\"txt" + fieldName[i] + "\" size=\"27\" /></td>");
                    sb.Append("</tr>");
                }
            }
            sb.Append("<tr><td colspan=\"3\" style=\"border:1px;text-align:right\" height=\"28\"><input type=\"button\" class=\"button\" id=\"seleSubmit\"  value=\"查 询\" onclick=\"formsubmit();\"/>&nbsp;&nbsp;<input type=\"button\" class=\"button\" id=\"seleCancle\" onclick=\"nodisp()\"  value=\"取 消\"/>&nbsp;&nbsp;</td></tr>");
            sb.Append(addTableFooter());
            sb.Append(addRingFoot());
            return sb.ToString();
        }

        /// <summary>
        /// table的头
        /// </summary>
        /// <returns></returns>
        public static string addSubTableHeader(string Tableid)
        {
            return "<table id=\"" + Tableid + "\" width=\"90%\" border=\"1px\" cellpadding=\"0\" cellspacing=\"0\" >";
        }

        /// <summary>
        /// 生成gridview
        /// </summary>
        /// <param name="ds">数据</param>
        /// <param name="caption">表头</param>
        /// <returns></returns>
        public static string addViewTable(string title,DataSet ds,string[] caption,int flag)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            if (flag == 0)
            {
                sb.Append(addSubTableHeader(title));
            }
            else
            {
                sb.Append(addTableHeader(title));
            }
            
            sb.Append("<tr height=\"33\">");
            for (i = 0; i <= caption.Length; i++)
            {
                if (i == caption.Length)
                {
                    if (flag == 1)
                    {
                        sb.Append("<td class=\"tdImage\"><span class=\"TableTitle\"><strong>查看详情</strong></span></td>");
                    }
                    else if (flag == 2)
                    {
                        sb.Append("<td class=\"tdImage\"><span class=\"TableTitle\"><strong>修改</strong></span></td>");
                        sb.Append("<td class=\"tdImage\"><span class=\"TableTitle\"><strong>删除</strong></span></td>");
                    }
                    else if (flag == 3)
                    {
                        sb.Append("<td class=\"tdImage\"><span class=\"TableTitle\"><strong>审核</strong></span></td>");
                    }
                    break;
                }
                else 
                {
                    if (caption[i] != null && caption[i] != string.Empty)
                    {
                        sb.Append("<td class=\"tdImage\"><span class=\"TableTitle\"><strong>" + caption[i] + "</strong></span></td>");    
                    }
                }
            }
            sb.Append("</tr>");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append("<tr height=\"28\">");
                    for (i = 0; i <= caption.Length; i++)
                    {
                        if (i == caption.Length)
                        {
                            if (flag == 1)
                            {
                                sb.Append("<td style=\"color: black;text-align:center\"><a href=\"#\" style=\"text-decoration: none;\" onclick = \"openview('" + dr["Number"].ToString() + "');\"><img src=\"../images/jpg/zoom_16x16.gif\" /></a></td>");
                            }
                            else if (flag == 2)
                            {
								sb.Append("<td style=\"color: black;text-align:center\"><a href=\"#\" style=\"text-decoration: none;\" onclick = \"openedit('" + dr["Number"].ToString() + "');\"><img src=\"../images/jpg/edit_16x16.gif\" /></a></td>");
								sb.Append("<td style=\"color: black;text-align:center\"><a href=\"#\" style=\"text-decoration: none;\" onclick = \"opendelete('" + dr["Number"].ToString() + "');\"><img src=\"../images/jpg/delete_16x16.gif\" /></a></td>");
                            }
                            else if (flag == 3)
                            {
								sb.Append("<td style=\"color: black;text-align:center\"><a href=\"#\" style=\"text-decoration: none;\" onclick = \"opencheck('" + dr["Number"].ToString() + "');\"><img src=\"../images/jpg/check_16x16.gif\" /></a></td>");
                            }
                            break;
                        }
                        else
                        {
                            if (caption[i] != null && caption[i] != string.Empty)
                            {
								if (dr[i].ToString().Length > 20)
								{
									sb.Append("<td style=\"color: black;text-align:center\">" + dr[i].ToString().Substring(0,20) + "..." + "</td>");
									continue;
								}
								if (ds.Tables[0].Columns[i].DataType.ToString().Equals("System.DateTime") && ds.Tables[0].Columns[i].ColumnName != string.Format("CreateTime"))
								{
									sb.Append("<td style=\"color: black;text-align:center\">" + DateTime.Parse(dr[i].ToString()).ToShortDateString() + "</td>");
									continue;
								}
								if (ds.Tables[0].Columns[i].ColumnName == string.Format("CheckState"))
								{
									string state = "";
									string alt = "";
									switch (dr[i].ToString())
									{ 
										case "0":
											state = "0.gif";
											alt = "审批中";
											break;
										case "1":
											state = "1.gif";
											alt = "审批通过";
											break;
										case "2":
											state = "2.gif";
											alt = "审批驳回";
											break;
									}
									sb.Append("<td style=\"color: black;text-align:center\"><img src=\"../images/jpg/" + state + "\" alt=\"" + alt + "\" /></td>");
									continue;
								}
                                sb.Append("<td style=\"color: black;text-align:center\">" + dr[i].ToString() + "</td>");
                            }
                        }
                        
                    }
                    sb.Append("</tr>");
                }
            }
            sb.Append(addTableFooter());
            return sb.ToString();
        }        

        /// <summary>
        /// 生成审核详情table
        /// </summary>
        /// <param name="header">字段标题</param>
        /// <param name="ds">数据</param>
        /// <param name="Title">table id/param>
        /// <returns>table.innerHtml</returns>
        public static string addAuditTable(string[] header,DataSet ds,string Title)
        {
            int i;
            StringBuilder sb = new StringBuilder();
            
            sb.Append(addTableHeader2());
            sb.Append("<tr><td style=\"border:0px;text-align:right\"><a href=\"#\" onclick=\"nodisp()\"><img src=\"../images/jpg/delete_16x16.gif\" /></a></tr>");
            sb.Append(addTableFooter());

            sb.Append(addRingTop());
            sb.Append(addTableHeader(Title));
            sb.Append("<tr height=\"32\">");
            for (i = 0; i < header.Length; i++)
            {
                sb.Append("<td class=\"tdImage\"><span class=\"TableTitle\"><strong style=\"font-size:12px\">" + header[i] + "</strong></span></td>");
            }
            sb.Append("</tr>");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append("<tr height=\"26\">");
                    for (i = 0; i < header.Length; i++)
                    {
                        sb.Append("<td class=\"tdData\">" + dr[i].ToString() + "</td>");
                    }
                    sb.Append("</tr>");
                }
            }
            sb.Append(addTableFooter());
            sb.Append(addRingFoot());
            sb.Append(addTableScript(Title));
            return sb.ToString();
        }


		/// <summary>
		/// 读取//WorkFlowModule/Data/Data-Field节点属性
		/// </summary>
		/// <returns>html内容</returns>
		public static string xmlParse(string module)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(addRingTop());
			XmlNodeList xlist = FMOP.XParse.xmlParse.XmlGetNode(module, "//WorkFlowModule/Data/Data-Field");
			if (xlist != null)
			{
				sb.Append(addTableHeader2());
				DataSet ds1 = FMOP.DB.DbHelperSQL.Query("select * from " + module);
				DataSet ds = new DataSet();
				foreach(XmlNode item in xlist)
				{
					#region 子表
					if (XMLHelper.GetSingleString(item, "type", "").Equals("SubTable"))
					{
						sb.Append("<tr height=\"20\"><td  style=\"width:100%\" colspan=\"3\"><hr style=\"width:100%\"></td></tr>");
						sb.Append("<tr height=\"20\"><td  style=\"width:100%;text-align:center\" colspan=\"3\"><span style=\"font-weight:700\">" + XMLHelper.GetSingleString(item, "caption", "") + "--多项用逗号隔开--" + "</span></td></tr>");
						string subT = XMLHelper.GetSingleString(item, "name", "");
						ds = FMOP.DB.DbHelperSQL.Query("select * from "+subT);
						XmlNodeList sublist = item["subTable"].SelectNodes("Sub-Field");
						foreach (XmlNode item2 in sublist)
						{
							sb.Append("<tr height=\"28\">");
							sb.Append("<td width=\"15%\" style=\"font-size:14px;color:black; text-align:right\">" + XMLHelper.GetSingleString(item2, "caption", "") + "：</td>");
							if (ds.Tables[0].Columns[XMLHelper.GetSingleString(item2, "name", "")].DataType.ToString().Equals("System.String"))
							{
								sb.Append(addStringSelect(subT + XMLHelper.GetSingleString(item2, "name", "")));
							}
							else
							{
								sb.Append(addOtherSelect(subT + XMLHelper.GetSingleString(item2, "name", "")));
							}
							sb.Append("<td width=\"25%\" style=\"text-align:left;\"><input type=\"text\" class=\"input1\" name=\"txt" + subT + XMLHelper.GetSingleString(item2, "name", "") + "\" size=\"27\" /></td>");
							sb.Append("</tr>");
						}
						sb.Append("<tr height=\"20\"><td  style=\"width:100%\" colspan=\"3\"><hr style=\"width:100%\"></td></tr>");
						ds.Clear();
					}
					#endregion

					#region 上传文件
					else if (XMLHelper.GetSingleString(item, "type", "").Equals("AddOnFiles"))
					{
						continue;
					}
					#endregion

					#region 主表
					else
					{
						sb.Append("<tr height=\"28\">");
						sb.Append("<td width=\"15%\" style=\"font-size:14px;color:black; text-align:right\">" + XMLHelper.GetSingleString(item, "caption", "") + "：</td>");
						if (ds1.Tables[0].Columns[XMLHelper.GetSingleString(item, "name", "")].DataType.ToString().Equals("System.String"))
						{
							sb.Append(addStringSelect(XMLHelper.GetSingleString(item, "name", "")));
						}
						else
						{
							sb.Append(addOtherSelect(XMLHelper.GetSingleString(item, "name", "")));
						}
						sb.Append("<td width=\"25%\" style=\"text-align:left;\"><input type=\"text\" class=\"input1\" name=\"txt" + XMLHelper.GetSingleString(item, "name", "") + "\" size=\"27\" /></td>");
						sb.Append("</tr>");
					}
					#endregion
				}
				sb.Append("<tr><td colspan=\"3\" style=\"border:1px;text-align:right\" height=\"28\"><input type=\"button\" class=\"button\" id=\"seleSubmit\"  value=\"查 询\" onclick=\"formsubmit();\"/>&nbsp;&nbsp;<input type=\"button\" class=\"button\" id=\"seleCancle\" onclick=\"nodisp()\"  value=\"取 消\"/>&nbsp;&nbsp;</td></tr>");
     			sb.Append(htmlWrite.addTableFooter());				
			}
			sb.Append(htmlWrite.addRingFoot());
			return sb.ToString();
		}

    }
}
