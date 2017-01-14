using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class JHJXPT_webservices_fm8844show : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.Charset = "GB2312";

        
        if (Request["sp"].ToString() == "liebiao")
        {
            HomePage HP = new HomePage();
            string lujing = Context.Request.MapPath("../../bytefiles/HomePageSQLlist.txt");
            DataSet ds = HP.test_ds_Run("默认50",lujing);

            string html = "";
            html = html + "<table border='0' cellpadding='0' cellspacing='0' style='background-color:#000;font-weight:bold;font-size:14px;'>";
            html = html + "<tr>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF; height:40px; padding-left:10px;'>序号&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>商品编号&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>合同期限&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>商品名称&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>商品产地&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>规格标准&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>计价&nbsp;&nbsp;&nbsp;<br/>单位</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>卖方名称&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>供货区域&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>竞标轮次&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>状态&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>上轮定&nbsp;&nbsp;&nbsp;<br/>标价(元)</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>最低卖出&nbsp;&nbsp;&nbsp<br/>价</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>最高买入&nbsp;&nbsp;&nbsp<br/>价</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>升降幅%&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>达成率/&nbsp;&nbsp;&nbsp;<br/>中标率</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>拟售量&nbsp;&nbsp;&nbsp;</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>集合预订量&nbsp;&nbsp;&nbsp;</td>";

            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>拟订购&nbsp;&nbsp;&nbsp;<br/>总量</td>";

            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>经济&nbsp;&nbsp;&nbsp;<br/>批量</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>日均最高&nbsp;&nbsp;&nbsp;<br/>供货量</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>买方&nbsp;&nbsp;&nbsp;<br/>数量</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>买方&nbsp;&nbsp;&nbsp;<br/>新增</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>买方区域&nbsp;&nbsp;&nbsp;<br/>覆盖</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>卖方&nbsp;&nbsp;&nbsp;<br/>数量</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>卖方&nbsp;&nbsp;&nbsp;<br/>新增</td>";
            html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF'>卖方区域&nbsp;&nbsp;&nbsp;<br/>覆盖</td>";
            html = html + "</tr>";
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    html = html + "<tr>";
                    html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF; height:20px; padding-left:10px;'>" + ds.Tables[0].Rows[i]["序号"].ToString() + "</td>";
                    html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF;'>" + ds.Tables[0].Rows[i]["商品编号"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wHuang' >" + ds.Tables[0].Rows[i]["合同期限"].ToString() + "</td>";
                    html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF;'>" + ds.Tables[0].Rows[i]["商品名称"].ToString() + "</td>";
                    html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF;' title='" + ds.Tables[0].Rows[i]["当前商品产地"].ToString() + "'>" + (ds.Tables[0].Rows[i]["当前商品产地"].ToString().Length > 5 ? (ds.Tables[0].Rows[i]["当前商品产地"].ToString().Substring(0, 5) + "...") : ds.Tables[0].Rows[i]["当前商品产地"].ToString()) + "</td>";
                    html = html + "<td align='left' valign='middle' nowrap='nowrap' style='color:#FFF;' title='" + ds.Tables[0].Rows[i]["型号规格"].ToString() + "'>" + (ds.Tables[0].Rows[i]["型号规格"].ToString().Length > 5 ? (ds.Tables[0].Rows[i]["型号规格"].ToString().Substring(0, 5) + "...") : ds.Tables[0].Rows[i]["型号规格"].ToString()) + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' style='color:#FFF;'>" + ds.Tables[0].Rows[i]["计价单位"].ToString() + "</td>";
                    html = html + "<td align='left' valign='middle' nowrap='nowrap'  class='wHuang' title='" + ds.Tables[0].Rows[i]["当前卖方名称"].ToString() + "'>" + (ds.Tables[0].Rows[i]["当前卖方名称"].ToString().Length > 5 ? (ds.Tables[0].Rows[i]["当前卖方名称"].ToString().Substring(0, 5) + "...") : ds.Tables[0].Rows[i]["当前卖方名称"].ToString()) + "</td>";
                    html = html + "<td align='left' valign='middle' nowrap='nowrap'  class='wHuang' title='" + ds.Tables[0].Rows[i]["当前供货区域"].ToString() + "'>" + (ds.Tables[0].Rows[i]["当前供货区域"].ToString().Length > 2 ? (ds.Tables[0].Rows[i]["当前供货区域"].ToString().Substring(0, 2) + "...") : ds.Tables[0].Rows[i]["当前供货区域"].ToString()) + "</td>";

                    html = html + "<td align='left' valign='middle' nowrap='nowrap' class='wLv'>" + ds.Tables[0].Rows[i]["当前投标轮次"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wHuang'>" + ds.Tables[0].Rows[i]["竞标状态"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wLan'>" + ds.Tables[0].Rows[i]["上轮定标价"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wHuang'>" + ds.Tables[0].Rows[i]["当前卖家最低价"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wHuang'>" + ds.Tables[0].Rows[i]["当前买家最高价"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wHuang'>" + ds.Tables[0].Rows[i]["升降幅"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wHong'>" + ds.Tables[0].Rows[i]["达成率/中标率"].ToString() + "</td>";
                    html = html + "<td align='left' valign='middle' nowrap='nowrap' class='wHuang'>" + ds.Tables[0].Rows[i]["最低价标的投标拟售量"].ToString() + "</td>";
                    html = html + "<td align='left' valign='middle' nowrap='nowrap' class='wLan'>" + ds.Tables[0].Rows[i]["当前集合预订量"].ToString() + "</td>";

                    html = html + "<td align='left' valign='middle' nowrap='nowrap' class='wLan'>" + ds.Tables[0].Rows[i]["当前拟订购总量"].ToString() + "</td>";

                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wHuang'>" + ds.Tables[0].Rows[i]["最低价标的经济批量"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wHuang'>" + ds.Tables[0].Rows[i]["最低价标的日均最高供货量"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wLan'>" + ds.Tables[0].Rows[i]["买家当前数量"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wLan'>" + ds.Tables[0].Rows[i]["买家今日新增数量"].ToString() + "</td>";
                    html = html + "<td align='left' valign='middle' nowrap='nowrap' class='wLan'>" + ds.Tables[0].Rows[i]["买家区域覆盖率"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wLan'>" + ds.Tables[0].Rows[i]["卖家当前数量"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wLan'>" + ds.Tables[0].Rows[i]["卖家今日新增数量"].ToString() + "</td>";
                    html = html + " <td align='left' valign='middle' nowrap='nowrap' class='wLan'>" + ds.Tables[0].Rows[i]["卖家区域覆盖率"].ToString() + "</td>";
                    
                    html = html + "</tr>";
                }
            }
            html = html + "</table>";

            Response.Write(html);
        }

        if (Request["sp"].ToString() == "weibu")
        {

            HomePage HP = new HomePage();
            string[][] dataStrArrALL = HP.indexTJSJ_Run(Context.Request.MapPath("../../bytefiles/HomePageByte_tongji.txt"));


            string html = "";
            html = html + " <table width='2123px' border='0' cellpadding='0' cellspacing='0' bgcolor='#000000' id='weibu' >";
            html = html + "  <tr nowrap='nowrap'>";
            html = html + "   <td width='1%' align='left' valign='middle' style='color:#FFF; background-color:#F00; height:3px;'></td>";
            html = html + "   <td width='99%' style='color:#FFF; background-color:#F00; height:3px;'></td>";
            html = html + "  </tr>";
            html = html + "   <tr  nowrap='nowrap'>";
            html = html + "    <td height='25' align='left' valign='middle' nowrap='nowrap'style='color:#FFF; padding-left:10px;'><strong>当前交易统计</strong><span style='color:#F00'>|</span></td>";
            html = html + "    <td nowrap='nowrap'  class='hongbiankuang' style='color:#FFF'><table border='0' cellpadding='0' cellspacing='0' >";
            html = html + "    <tr  nowrap='nowrap'>";
            string[] dataStrArr1 = dataStrArrALL[0];
            for (int i = 0; i < dataStrArr1.Length; i++)
            {
                html = html + "      <td  nowrap='nowrap' style='color:#FF0; padding-left:10px; padding-right:5px;'>" + dataStrArr1[i] + "</td>";
                i++;
                html = html + "     <td  nowrap='nowrap' style='color:#3CF; padding-right:10px;'>" + dataStrArr1[i] + "</td>";
            }






            html = html + "    </tr>";
            html = html + "   </table></td>";
            html = html + "  </tr>";
            html = html + "  <tr  nowrap='nowrap'>";
            html = html + "    <td height='25' align='left' valign='middle' nowrap='nowrap'  class='hongbiankuang' style='color:#FFF; padding-left:10px;'><strong>今年新增统计</strong><span style='color:#F00'>|</span></td>";
            html = html + "    <td nowrap='nowrap'  class='hongbiankuang' style='color:#FFF'><table border='0' cellpadding='0' cellspacing='0' >";
            html = html + "    <tr  nowrap='nowrap'>";



            string[] dataStrArr2 = dataStrArrALL[1];
            for (int i = 0; i < dataStrArr2.Length; i++)
            {
                html = html + "      <td  nowrap='nowrap' style='color:#FF0; padding-left:10px; padding-right:5px;'>" + dataStrArr2[i] + "</td>";
                i++;
                html = html + "     <td  nowrap='nowrap' style='color:#3CF; padding-right:10px;'>" + dataStrArr2[i] + "</td>";
            }

            html = html + "    </tr>";
            html = html + "   </table></td>";
            html = html + "   </tr>";
            html = html + "  <tr  nowrap='nowrap'>";
            html = html + "   <td height='25' align='left' valign='middle' nowrap='nowrap' style='color:#FFF; padding-left:10px;'><strong>今年累计统计</strong><span style='color:#F00'>|</span></td>";
            html = html + "   <td nowrap='nowrap' style='color:#FFF'><table border='0' cellpadding='0' cellspacing='0' >";
            html = html + "    <tr  nowrap='nowrap'>";



            string[] dataStrArr3 = dataStrArrALL[2];
            for (int i = 0; i < dataStrArr3.Length; i++)
            {
                html = html + "      <td  nowrap='nowrap' style='color:#FF0; padding-left:10px; padding-right:5px;'>" + dataStrArr3[i] + "</td>";
                i++;
                html = html + "     <td  nowrap='nowrap' style='color:#3CF; padding-right:10px;'>" + dataStrArr3[i] + "</td>";
            }


            html = html + "    </tr>";
            html = html + "   </table></td>";
            html = html + "  </tr>";
            html = html + " </table>";

            Response.Write(html);
        }
        
    }
}