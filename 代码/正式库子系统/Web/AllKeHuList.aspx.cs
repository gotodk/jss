using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;

public partial class Web_AllKeHuList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bangding();
    }
    private void bangding()
    {
        if (Request["cs"] != null && Request["cs"].ToString() != "")
        {            
            string cs = Server.UrlDecode(Request["cs"].ToString());
            string sql_fws = "SELECT a.Number,a.KHMC, a.DZ,a.LXDH from KHGL_New as a left join KHGL_FXSJBXX as b on a.number=b.number where a.XSQD in('服务商','门店服务商') and a.Number LIKE '%" + fwsbh.Text.Trim() + "%' AND a.KHMC LIKE '%" + fwsmc.Text.Trim() + "%' AND a.SSXSQY='" + cs + "' and b.hzzt not like '%解约%'  union all select a.number,a.dkrxm as khmc,b.dz as dz,a.dkrlxdh as lxdh from khgl_fwsdkrxx as a left join khgl_new as b on a.ssfwsbh=b.number where a.ssbsc like '" + cs + "' and  a.dkrxm like '%" + fwsmc.Text.Trim() + "%' and a.number like '%" + fwsbh.Text.Trim() + "%' ";
            string sql_ZDYH = "SELECT Number,KHMC,dz,lxdh from KHGL_New where XSQD in('终端客户','服务商客户') and Number LIKE '%" + fwsbh.Text.Trim() + "%' AND KHMC LIKE '%" + fwsmc.Text.Trim() + "%' AND SSXSQY='" + cs + "' AND SFZSKH='1'";

            string sql = sql_fws + " union all " + sql_ZDYH;
            
            DataSet ds = DbHelperSQL.Query(sql);
            RadGrid1.DataSource = ds.Tables[0].DefaultView;           
            RadGrid1.DataBind();
        }
    }
    protected void EditButton_Click(object sender, EventArgs e)
    {
        bangding();
    }
    protected void RadGrid1_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
    {
        this.RadGrid1.CurrentPageIndex = e.NewPageIndex;
    }
}