using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.Data;
using FMOP.DB;

public partial class Web_JHJX_New2013_JHJX_FGSYWCX_FGSMJMJTJ_city : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //判断是否有权限


            GetData();
        }
    }

    private void GetData()
    {
        //获取本周、本月开始和结束时间
        DateTime now = DateTime.Now;  //当前时间
        DateTime startWeek = now.AddDays(1 - Convert.ToInt32(now.DayOfWeek.ToString("d")));  //本周周一
        DateTime endWeek = startWeek.AddDays(6);  //本周周日
        DateTime startMonth = now.AddDays(1 - now.Day);  //本月月初
        DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末 

        string startYear = "";
        string endYear = "";
        //获取本财年的开始结束时间
        if (now.Month >= 3)
        {
            startYear = now.Year.ToString() + "-03-01";
            endYear = now.AddYears(1).Year.ToString() + "-03-01";
        }
        else
        {
            startYear = now.AddYears(-1).Year.ToString() + "-03-01";
            endYear = now.Year.ToString() + "-03-01";
        }

        //获取所有分公司名称以及需要绑定的表结构
        string sql = "select distinct FGSname as 分公司,0 as 买家省会本日,0 as 买家省会本周,0 as 买家省会本月,0 as 买家省会财年,0 as 买家地市本日,0 as 买家地市本周,0 as 买家地市本月,0 as 买家地市财年,0 as 卖家省会本日,0 as 卖家省会本周,0 as 卖家省会本月,0 as 卖家省会财年,0 as 卖家地市本日,0 as 卖家地市本周,0 as 卖家地市本月,0 as 卖家地市财年 from AAA_CityList_FGS";
        DataSet ds_result = DbHelperSQL.Query(sql);       

        //string sql_zhxx = "select B_DLYX,I_ZCLB as 注册类别 ,I_SSQYSHI,b.sfshcs as 是否省会级,isnull(c.CountNum,0) as CountNum ,(case I_ZCLB when '自然人' then '买家' when '单位' then (case when ISNULL(c.CountNum ,0)>0 then '卖家' else '买家' end) end) as 统计身份,I_PTGLJG  as 平台管理机构,convert(varchar(10),FGSSHSJ,120) as 审核通过时间,(select top 1 convert(varchar(10),shsj,120) from AAA_CSSPB where SHZT='审核通过' and AAA_CSSPB .DLYX =a.B_DLYX order by SHSJ) as 卖家生效时间 from AAA_DLZHXXB as a left join AAA_CityList_City as b on a.I_SSQYSHI=b.c_namestr left join (select dlyx, COUNT(*) as CountNum from AAA_CSSPB where SHZT='审核通过' group by dlyx) as c on a.B_DLYX=c.DLYX left join (select DLYX,FGSSHSJ from AAA_MJMJJYZHYJJRZHGLB where SFSCGLJJR='是' and JSZHLX='买家卖家交易账户' and JJRSHZT='审核通过' and FGSSHZT='审核通过') as d on d.DLYX =a.B_DLYX where S_SFYBJJRSHTG='是' and  S_SFYBFGSSHTG='是' and B_JSZHLX='买家卖家交易账户'";

        string sql_zhxx = "select B_DLYX,I_ZCLB as 注册类别 ,I_SSQYSHI,b.sfshcs as 是否省会级,isnull(c.CountNum,0) as CountNum ,(case I_ZCLB when '自然人' then '买家' when '单位' then (case when ISNULL(c.CountNum ,0)>0 then '卖家' else '买家' end) end) as 统计身份,I_PTGLJG  as 平台管理机构,convert(varchar(10),FGSSHSJ,120) as 审核通过时间,(select convert(varchar(10),max(shsj),120) from AAA_CSSPB where SHZT='审核通过' and AAA_CSSPB .DLYX =a.B_DLYX) as 卖家生效时间 from AAA_DLZHXXB as a left join AAA_CityList_City as b on a.I_SSQYSHI=b.c_namestr left join (select dlyx, COUNT(*) as CountNum from AAA_CSSPB where SHZT='审核通过' group by dlyx) as c on a.B_DLYX=c.DLYX left join (select DLYX,FGSSHSJ from AAA_MJMJJYZHYJJRZHGLB where SFSCGLJJR='是' and JSZHLX='买家卖家交易账户' and JJRSHZT='审核通过' and FGSSHZT='审核通过') as d on d.DLYX =a.B_DLYX where S_SFYBJJRSHTG='是' and  S_SFYBFGSSHTG='是' and B_JSZHLX='买家卖家交易账户'";
        DataSet ds_info = DbHelperSQL.Query(sql_zhxx);
        if (ds_info != null && ds_info.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds_result.Tables[0].Rows.Count; i++)
            {
                //买家省会本日
                string tiaojian_BuyerSH_Day = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='买家' and 是否省会级='是' and 审核通过时间='" + now.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["买家省会本日"] = ds_info.Tables[0].Select(tiaojian_BuyerSH_Day).Length.ToString();

                //买家省会本周 
                string tiaojian_BuyerSH_Week = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='买家' and 是否省会级='是' and 审核通过时间>='" + startWeek.ToString("yyyy-MM-dd") + "' and 审核通过时间<='" + endWeek.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["买家省会本周"] = ds_info.Tables[0].Select(tiaojian_BuyerSH_Week).Length.ToString();

                //买家省会本月 
                string tiaojian_BuyerSH_Month = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='买家' and 是否省会级='是' and 审核通过时间>='" + startMonth.ToString("yyyy-MM-dd") + "' and 审核通过时间<='" + endMonth.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["买家省会本月"] = ds_info.Tables[0].Select(tiaojian_BuyerSH_Month).Length.ToString();

                //买家省会财年
                string tiaojian_BuyerSH_Year = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='买家' and 是否省会级='是' and 审核通过时间>='" + startYear + "' and 审核通过时间<'" + endYear + "'";
                ds_result.Tables[0].Rows[i]["买家省会财年"] = ds_info.Tables[0].Select(tiaojian_BuyerSH_Year).Length.ToString();

                //买家地市本日
                string tiaojian_BuyerDS_Day = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='买家' and 是否省会级='否' and 审核通过时间='" + now.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["买家地市本日"] = ds_info.Tables[0].Select(tiaojian_BuyerDS_Day).Length.ToString();

                //买家地市本周 
                string tiaojian_BuyerDS_Week = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='买家' and 是否省会级='否' and 审核通过时间>='" + startWeek.ToString("yyyy-MM-dd") + "' and 审核通过时间<='" + endWeek.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["买家地市本周"] = ds_info.Tables[0].Select(tiaojian_BuyerDS_Week).Length.ToString();

                //买家地市本月 
                string tiaojian_BuyerDS_Month = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='买家' and 是否省会级='否' and 审核通过时间>='" + startMonth.ToString("yyyy-MM-dd") + "' and 审核通过时间<='" + endMonth.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["买家地市本月"] = ds_info.Tables[0].Select(tiaojian_BuyerDS_Month).Length.ToString();

                //买家地市财年
                string tiaojian_BuyerDS_Year = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='买家' and 是否省会级='否' and 审核通过时间>='" + startYear + "' and 审核通过时间<'" + endYear + "'";
                ds_result.Tables[0].Rows[i]["买家地市财年"] = ds_info.Tables[0].Select(tiaojian_BuyerDS_Year).Length.ToString();


                //卖家省会本日
                string tiaojian_SalerSH_Day = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='卖家' and 是否省会级='是' and 卖家生效时间='" + now.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["卖家省会本日"] = ds_info.Tables[0].Select(tiaojian_SalerSH_Day).Length.ToString();

                //卖家省会本周 
                string tiaojian_SalerSH_Week = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='卖家' and 是否省会级='是' and 卖家生效时间>='" + startWeek.ToString("yyyy-MM-dd") + "' and 卖家生效时间<='" + endWeek.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["卖家省会本周"] = ds_info.Tables[0].Select(tiaojian_SalerSH_Week).Length.ToString();

                //卖家省会本月 
                string tiaojian_SalerSH_Month = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='卖家' and 是否省会级='是' and 卖家生效时间>='" + startMonth.ToString("yyyy-MM-dd") + "' and 卖家生效时间<='" + endMonth.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["卖家省会本月"] = ds_info.Tables[0].Select(tiaojian_SalerSH_Month).Length.ToString();

                //卖家省会财年
                string tiaojian_SalerSH_Year = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='卖家' and 是否省会级='是' and 卖家生效时间>='" + startYear + "' and 卖家生效时间<'" + endYear + "'";
                ds_result.Tables[0].Rows[i]["卖家省会财年"] = ds_info.Tables[0].Select(tiaojian_SalerSH_Year).Length.ToString();

                //卖家地市本日
                string tiaojian_SalerDS_Day = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='卖家' and 是否省会级='否' and 卖家生效时间='" + now.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["卖家地市本日"] = ds_info.Tables[0].Select(tiaojian_SalerDS_Day).Length.ToString();

                //卖家地市本周 
                string tiaojian_SalerDS_Week = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='卖家' and 是否省会级='否' and 卖家生效时间>='" + startWeek.ToString("yyyy-MM-dd") + "' and 卖家生效时间<='" + endWeek.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["卖家地市本周"] = ds_info.Tables[0].Select(tiaojian_SalerDS_Week).Length.ToString();

                //卖家地市本月 
                string tiaojian_SalerDS_Month = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='卖家' and 是否省会级='否' and 卖家生效时间>='" + startMonth.ToString("yyyy-MM-dd") + "' and 卖家生效时间<='" + endMonth.ToString("yyyy-MM-dd") + "'";
                ds_result.Tables[0].Rows[i]["卖家地市本月"] = ds_info.Tables[0].Select(tiaojian_SalerDS_Month).Length.ToString();

                //卖家地市财年
                string tiaojian_SalerDS_Year = "平台管理机构='" + ds_result.Tables[0].Rows[i]["分公司"].ToString() + "' and 统计身份='卖家' and 是否省会级='否' and 卖家生效时间>='" + startYear + "' and 卖家生效时间<'" + endYear + "'";
                ds_result.Tables[0].Rows[i]["卖家地市财年"] = ds_info.Tables[0].Select(tiaojian_SalerDS_Year).Length.ToString();
            }


            lblBuyerSH_day.Text = ds_result.Tables[0].Compute("sum(买家省会本日)", "true").ToString();
            lblBuyerSH_week.Text = ds_result.Tables[0].Compute("sum(买家省会本周)", "true").ToString();
            lblBuyerSH_month.Text = ds_result.Tables[0].Compute("sum(买家省会本月)", "true").ToString();
            lblBuyerSH_year.Text = ds_result.Tables[0].Compute("sum(买家省会财年)", "true").ToString();
            lblBuyerDS_day.Text = ds_result.Tables[0].Compute("sum(买家地市本日)", "true").ToString();
            lblBuyerDS_week.Text = ds_result.Tables[0].Compute("sum(买家地市本周)", "true").ToString();
            lblBuyerDS_month.Text = ds_result.Tables[0].Compute("sum(买家地市本月)", "true").ToString();
            lblBuyerDS_year.Text = ds_result.Tables[0].Compute("sum(买家地市财年)", "true").ToString();
            lblSalerSH_day.Text = ds_result.Tables[0].Compute("sum(卖家省会本日)", "true").ToString();
            lblSalerSH_week.Text = ds_result.Tables[0].Compute("sum(卖家省会本周)", "true").ToString();
            lblSalerSH_month.Text = ds_result.Tables[0].Compute("sum(卖家省会本月)", "true").ToString();
            lblSalerSH_year.Text = ds_result.Tables[0].Compute("sum(卖家省会财年)", "true").ToString();
            lblSalerDS_day.Text = ds_result.Tables[0].Compute("sum(卖家地市本日)", "true").ToString();
            lblSalerDS_week.Text = ds_result.Tables[0].Compute("sum(卖家地市本周)", "true").ToString();
            lblSalerDS_month.Text = ds_result.Tables[0].Compute("sum(卖家地市本月)", "true").ToString();
            lblSalerDS_year.Text = ds_result.Tables[0].Compute("sum(卖家地市财年)", "true").ToString();
        }
        rpt.DataSource = ds_result.Tables[0].DefaultView;
        rpt.DataBind();


    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToExcel(this.export);
    }
    // 将数据导出到excel，与下面的函数同时使用才能正常工作
    public void ToExcel(System.Web.UI.Control ctl)
    {
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=myfiles.xls");

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword   
        ctl.Page.EnableViewState = false;
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        ctl.RenderControl(hw);
        HttpContext.Current.Response.Write(tw.ToString());
        HttpContext.Current.Response.End();

        //HttpContext.Current.Response.Clear();

        //HttpContext.Current.Response.Charset = "";
        //string filename = System.DateTime.Now.ToString("_yyyyMMddHHmm");
        //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" +

        //System.Web.HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8) + ".xls");

        //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
        //HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword   

        //ctl.Page.EnableViewState = false;
        //System.IO.StringWriter tw = new System.IO.StringWriter();
        //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        //#region//asp.net 导出Excel时，解决纯数字字符串变成类似这样的 2.00908E+18 形式的代码
        //string strStyle = "<style>td{vnd.ms-excel.numberformat: @;}</style>";//导出的数字的样式
        //tw.WriteLine(strStyle);
        //#endregion

        //ctl.RenderControl(hw);
        //HttpContext.Current.Response.Write(tw.ToString());
        //HttpContext.Current.Response.End();
    }
    // 导出Exel时需重载此方法
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
}