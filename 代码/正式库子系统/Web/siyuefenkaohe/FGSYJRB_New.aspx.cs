using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;

public partial class Web_siyuefenkaohe_FGSYJRB_New : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            SetData();
        }
    }

    private void SetData()
    {
        string strSQL = "select DYFGSMC as 分公司,0 as 累计,0 as 今日新增,0 as 自然人,0 as 单位,0 as 行业协会,0 as 媒体,0 as 银行,'否' as 自然人超标,isnull(b.TYBGSFHG,'否') as 调研报告合格,'是' as 一千五以下,'是' as 五百以下,0 as 奖励总额,Rpt_Number from system_city as a left join AAA_FGSDYBGDBYLB as b on a.dyfgsmc=b.fgsmc where dyfgsmc<>'其他分公司' order by Rpt_Number";
        DataSet ds = DbHelperSQL.Query(strSQL);

        //string sql_baseinfo = "select B_DLYX as 登陆邮箱,J_JJRJSBH as 经纪人角色编号,I_ZCLB as 注册类别,b.SSHY as 所属行业,(case when I_ZCLB ='自然人' then '自然人' when I_ZCLB ='单位' then (case when isnull(b.SSHY,'')='' then '单位' else b.SSHY end) end) as 类别,I_PTGLJG as 所属分公司,(select top 1 convert(varchar(10),生效时间,120) from (select top 2 T_YSTBDDLYX as 交易方邮箱,T_YSTBDGLJJRYX as 关联经纪人邮箱,Y_YSYDDDLYX,'卖家' as 交易类别,Z_QPJSSJ as 生效时间 from AAA_ZBDBXXB  where Z_QPZT ='清盘结束' and T_YSTBDGLJJRYX=a.B_DLYX order by Z_QPJSSJ union all select top 2 Y_YSYDDDLYX as 交易方邮箱,Y_YSYDDGLJJRYX as 关联经纪人邮箱,Y_YSYDDDLYX,'买家' as 交易类别,Z_QPJSSJ as 生效时间 from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and Y_YSYDDGLJJRYX=a.B_DLYX order by Z_QPJSSJ) as tabsxsj order by 生效时间 DESC) as 生效时间,(select COUNT(*) from AAA_ZBDBXXB as aa left join AAA_MJMJJYZHYJJRZHGLB as bb on aa.T_YSTBDDLYX=bb.DLYX  where Z_QPZT ='清盘结束' and bb.GLJJRDLZH=a.B_DLYX) as 卖出次数,(select COUNT(*) from AAA_ZBDBXXB as aa left join AAA_MJMJJYZHYJJRZHGLB as bb on aa.Y_YSYDDDLYX=bb.DLYX  where Z_QPZT ='清盘结束' and bb.GLJJRDLZH=a.B_DLYX) as 买入次数 from AAA_DLZHXXB as a left join AAA_JJRSSHYTB as b on a.B_DLYX =b.DLYX where B_JSZHLX ='经纪人交易账户' and S_SFYBJJRSHTG ='是' and S_SFYBFGSSHTG ='是' and (select COUNT(*) from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and T_YSTBDGLJJRYX=a.B_DLYX)>=2 and (select COUNT(*) from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and Y_YSYDDGLJJRYX=a.B_DLYX) >=2 order by 所属分公司,生效时间";

        string sql_baseinfo = "select a.B_DLYX,(case when I_ZCLB ='自然人' then '自然人' when I_ZCLB ='单位' then (case when isnull(b.SSHY,'')='' then '单位' else b.SSHY end) end) as 统计类别,I_PTGLJG as 所属分公司,(select top 1 convert(varchar(10),生效时间,120) from (select top 2 Z_QPJSSJ as 生效时间 from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and T_YSTBDGLJJRYX=a.B_DLYX order by Z_QPJSSJ union all select top 2 Z_QPJSSJ as 生效时间 from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and Y_YSYDDGLJJRYX=a.B_DLYX order by Z_QPJSSJ) as tabsxsj order by 生效时间 DESC) as 生效时间 from AAA_DLZHXXB as a left join AAA_JJRSSHYTB as b on a.B_DLYX =b.DLYX where B_JSZHLX ='经纪人交易账户' and S_SFYBJJRSHTG ='是' and S_SFYBFGSSHTG ='是' and (select COUNT(*) from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and T_YSTBDGLJJRYX=a.B_DLYX)>=2 and (select COUNT(*) from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and Y_YSYDDGLJJRYX=a.B_DLYX) >=2 and B_DLYX not in (select distinct GLJJRDLYX from AAA_YGFZJJRXXB) order by 所属分公司,生效时间";

        DataSet dsBase = DbHelperSQL.Query(sql_baseinfo);        
        if (ds != null && ds.Tables[0].Rows.Count > 0&&dsBase!=null&&dsBase.Tables [0].Rows .Count >0)
        {
            int TotalMoney = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int total=dsBase.Tables[0].Select("所属分公司='" + dr["分公司"].ToString() + "'").Length;
                if (total > 0)
                {
                    dr["累计"] = total.ToString();
                    dr["今日新增"] = dsBase.Tables[0].Select("所属分公司='" + dr["分公司"].ToString() + "' and 生效时间='" + DateTime.Now.ToString("yyyy-MM-dd") + "'").Length.ToString();
                    dr["自然人"] = dsBase.Tables[0].Select("统计类别='自然人' and 所属分公司='" + dr["分公司"].ToString() + "'").Length.ToString();
                    dr["单位"] = dsBase.Tables[0].Select("统计类别='单位' and 所属分公司='" + dr["分公司"].ToString() + "'").Length.ToString();
                    dr["行业协会"] = dsBase.Tables[0].Select("统计类别='行业协会' and 所属分公司='" + dr["分公司"].ToString() + "'").Length.ToString();
                    dr["媒体"] = dsBase.Tables[0].Select("统计类别='媒体' and 所属分公司='" + dr["分公司"].ToString() + "'").Length.ToString();
                    dr["银行"] = dsBase.Tables[0].Select("统计类别='银行' and 所属分公司='" + dr["分公司"].ToString() + "'").Length.ToString();

                    dr["一千五以下"] = total < 1500? "是" : "否";
                    dr["五百以下"] = total < 500 ? "是" : "否";

                    if (total <= 5000)
                    {                       
                        dr["自然人超标"] = (Convert.ToDouble(dr["自然人"].ToString ()) /Convert .ToDouble (total) * 100.00) > 85.00 ? "是" : "否";

                        TotalMoney = Convert.ToInt32(dr["自然人"]) * 20 + Convert.ToInt32(dr["单位"]) * 50 + Convert.ToInt32(dr["行业协会"]) * 70 + Convert.ToInt32(dr["媒体"]) * 100 + Convert.ToInt32(dr["银行"]) * 150;
                    }
                    else
                    {
                        DataTable dtFGS = dsBase.Tables[0].Clone();
                        DataRow[] drFGS = dsBase.Tables[0].Select("所属分公司='" + dr["分公司"].ToString() + "'");
                        for (int i = 0; i < 5000; i++)
                        {
                            dtFGS.ImportRow(drFGS[i]);
                        }

                        dr["自然人超标"] = ((Convert .ToDouble (dtFGS.Select("统计类别='自然人'").Length) /Convert .ToDouble ( dtFGS.Rows.Count)) * 100.00) > 85.00 ? "是" : "否";
                        TotalMoney = (dtFGS.Select("统计类别='自然人'").Length * 20) + (dtFGS.Select("统计类别='单位'").Length * 50) + (dtFGS.Select("统计类别='行业协会'").Length * 70) + (dtFGS.Select("统计类别='媒体'").Length * 100) + (dtFGS.Select("统计类别='银行'").Length * 150);
                    }
                    if (dr["一千五以下"].ToString() == "是")
                    {
                        dr["奖励总额"] = 0;
                    }
                    else if (dr["自然人超标"].ToString () == "是" || dr["调研报告合格"].ToString () == "否")
                    {
                        dr["奖励总额"] = (TotalMoney / 2).ToString();
                    }
                    else
                    {
                        dr["奖励总额"] = TotalMoney.ToString();   
                    }
                }

            }           
        }

        ds.Tables[0].DefaultView.Sort = "奖励总额 DESC, Rpt_Number";
        Repeater1.DataSource = ds.Tables[0].DefaultView;
        Repeater1.DataBind();
    }
}