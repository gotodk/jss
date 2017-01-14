using FMOP.DB;
using Hesion.Brick.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_JHJX_New2013_JHJX_SPMM_jhjix_ckxq : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Number"] != null && !Request.QueryString["Number"].ToString().Equals(""))
        {
            string StrNumber = Request.QueryString["Number"].ToString();
            InitPage(StrNumber);
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "查询出错！");
        }
    }

    private void InitPage( string StrNumber )
    {
         string Strsql = "   select Z_HTBH as 合同编号, ( select MJSDJJPL from AAA_TBD where Number = AAA_ZBDBXXB.T_YSTBDBH  ) as 经济批量,Z_RJZGFHL as 日均最高发货量,Z_ZBSL as 定标数量,( select I_JYFMC from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.T_YSTBDDLYX ) as 卖家名称,( select I_LXRSJH from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.T_YSTBDDLYX ) as 卖家联系方式,( select I_LXRXM from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.T_YSTBDDLYX ) as 卖家联系人,( select I_JYFMC from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.Y_YSYDDDLYX ) as  买家家名称,( select I_LXRSJH from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.Y_YSYDDDLYX ) as 买家联系方式,( select I_LXRXM from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.Y_YSYDDDLYX ) as 买家联系人  from AAA_ZBDBXXB where Number = '" + StrNumber + "'";
        DataSet ds = DbHelperSQL.Query(Strsql);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            this.divDH.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["合同编号"].ToString();
            this.divKHBH.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["日均最高发货量"].ToString();
            this.divKHMC.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["经济批量"].ToString();
            this.divLXDH.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["定标数量"].ToString();
            this.divCZHM.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["卖家名称"].ToString();

            this.divSHDZ.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["卖家联系方式"].ToString();
            this.divSHRXM.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["卖家联系人"].ToString();
            this.divSHRSJ.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["买家家名称"].ToString();
            this.divDZYX.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["买家联系方式"].ToString();
            this.divWLFS.InnerHtml = "&nbsp;" + ds.Tables[0].Rows[0]["买家联系人"].ToString();

           
        }
    }
}