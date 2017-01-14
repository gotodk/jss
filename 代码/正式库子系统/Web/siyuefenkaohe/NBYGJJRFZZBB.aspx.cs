using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;

public partial class Web_siyuefenkaohe_NBYGJJRFZZBB : System.Web.UI.Page
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
        string strSQL = "select distinct YGGH as 员工工号, ygxm as 员工姓名,0 as 累计,0 as 本周新增,0 as 自然人,0 as 单位,'是' as 超过三十个,0 as 奖励总额  from AAA_YGFZJJRXXB where SFYX='是' order by YGGH, YGXM";
        DataSet ds = DbHelperSQL.Query(strSQL);

        //string sql_baseinfo = "select B_DLYX as 登陆邮箱,J_JJRJSBH as 经纪人角色编号,I_ZCLB as 注册类别,b.SSHY as 所属行业,(case when I_ZCLB ='自然人' then '自然人' when I_ZCLB ='单位' then (case when isnull(b.SSHY,'')='' then '单位' else b.SSHY end) end) as 类别,I_PTGLJG as 所属分公司,(select top 1 convert(varchar(10),生效时间,120) from (select top 2 T_YSTBDDLYX as 交易方邮箱,T_YSTBDGLJJRYX as 关联经纪人邮箱,Y_YSYDDDLYX,'卖家' as 交易类别,Z_QPJSSJ as 生效时间 from AAA_ZBDBXXB  where Z_QPZT ='清盘结束' and T_YSTBDGLJJRYX=a.B_DLYX order by Z_QPJSSJ union all select top 2 Y_YSYDDDLYX as 交易方邮箱,Y_YSYDDGLJJRYX as 关联经纪人邮箱,Y_YSYDDDLYX,'买家' as 交易类别,Z_QPJSSJ as 生效时间 from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and Y_YSYDDGLJJRYX=a.B_DLYX order by Z_QPJSSJ) as tabsxsj order by 生效时间 DESC) as 生效时间,(select COUNT(*) from AAA_ZBDBXXB as aa left join AAA_MJMJJYZHYJJRZHGLB as bb on aa.T_YSTBDDLYX=bb.DLYX  where Z_QPZT ='清盘结束' and bb.GLJJRDLZH=a.B_DLYX) as 卖出次数,(select COUNT(*) from AAA_ZBDBXXB as aa left join AAA_MJMJJYZHYJJRZHGLB as bb on aa.Y_YSYDDDLYX=bb.DLYX  where Z_QPZT ='清盘结束' and bb.GLJJRDLZH=a.B_DLYX) as 买入次数 from AAA_DLZHXXB as a left join AAA_JJRSSHYTB as b on a.B_DLYX =b.DLYX where B_JSZHLX ='经纪人交易账户' and S_SFYBJJRSHTG ='是' and S_SFYBFGSSHTG ='是' and (select COUNT(*) from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and T_YSTBDGLJJRYX=a.B_DLYX)>=2 and (select COUNT(*) from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and Y_YSYDDGLJJRYX=a.B_DLYX) >=2 order by 所属分公司,生效时间";

        string sql_baseinfo = "select YGGH as 工号,YGXM as 姓名,GLJJRDLYX,GLJJRZCLB as 统计类别,(select top 1 convert(varchar(10),生效时间,120) from (select top 2 T_YSTBDDLYX as 交易方邮箱,T_YSTBDGLJJRYX as 关联经纪人邮箱,Y_YSYDDDLYX,'卖家' as 交易类别,Z_QPJSSJ as 生效时间 from AAA_ZBDBXXB  where Z_QPZT ='清盘结束' and T_YSTBDGLJJRYX=a.GLJJRDLYX order by Z_QPJSSJ union all select top 2 Y_YSYDDDLYX as 交易方邮箱,Y_YSYDDGLJJRYX as 关联经纪人邮箱,Y_YSYDDDLYX,'买家' as 交易类别,Z_QPJSSJ as 生效时间 from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and Y_YSYDDGLJJRYX=a.GLJJRDLYX order by Z_QPJSSJ) as tabsxsj order by 生效时间 DESC) as 生效时间 from  AAA_YGFZJJRXXB as a where SFYX='是' and (select COUNT(*) from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and T_YSTBDGLJJRYX=a.GLJJRDLYX)>=2 and (select COUNT(*) from AAA_ZBDBXXB where Z_QPZT ='清盘结束' and Y_YSYDDGLJJRYX=a.GLJJRDLYX) >=2 order by YGXM,生效时间";
        DataSet dsBase = DbHelperSQL.Query(sql_baseinfo);

        //获取本周、本月开始和结束时间
        DateTime now = DateTime.Now;  //当前时间
        DateTime startWeek = now.AddDays(1 - Convert.ToInt32(now.DayOfWeek.ToString("d")));  //本周周一
        DateTime endWeek = startWeek.AddDays(6);  //本周周日

        if (ds != null && ds.Tables[0].Rows.Count > 0 && dsBase != null && dsBase.Tables[0].Rows.Count > 0)
        {
            int TotalMoney = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int total = dsBase.Tables[0].Select("工号='" + dr["员工工号"].ToString() + "'").Length;
                if (total >= 0)
                {
                    dr["累计"] = total.ToString();
                    dr["本周新增"] = dsBase.Tables[0].Select("工号='" + dr["员工工号"].ToString() + "' and 生效时间>='" + startWeek.ToString("yyyy-MM-dd") + "' and 生效时间<='" + endWeek.ToString("yyyy-MM-dd") + "'").Length.ToString();
                    dr["自然人"] = dsBase.Tables[0].Select("统计类别='自然人' and 工号='" + dr["员工工号"].ToString() + "'").Length.ToString();
                    dr["单位"] = dsBase.Tables[0].Select("统计类别='单位' and 工号='" + dr["员工工号"].ToString() + "'").Length.ToString();                   

                    dr["超过三十个"] = total >= 30 ? "是" : "否"; 
                    
                    if (dr["超过三十个"].ToString() == "否")
                    {
                        dr["奖励总额"] = 0;
                    }                  
                    else
                    {
                        DataTable dtFGS = dsBase.Tables[0].Clone();
                        DataRow[] drFGS = dsBase.Tables[0].Select("工号='" + dr["员工工号"].ToString() + "'");
                        for (int i = 0; i < 30; i++)
                        {
                            dtFGS.ImportRow(drFGS[i]);
                        }                     
                        TotalMoney = (dtFGS.Select("统计类别='自然人'").Length * 20) + (dtFGS.Select("统计类别='单位'").Length * 50) ;

                        dr["奖励总额"] = TotalMoney.ToString();
                    }                   
                }
            }

            ds.Tables[0].DefaultView.Sort = "奖励总额 DESC, 员工姓名";
            Repeater1.DataSource = ds.Tables[0].DefaultView;
            Repeater1.DataBind();
        }
    }
}