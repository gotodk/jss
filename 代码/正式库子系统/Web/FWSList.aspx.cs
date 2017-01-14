using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;

public partial class Web_FWSList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bangding();
    }
    private void bangding()
    {
        if (Request["cs"] != null && Request["cs"].ToString() != "")
        {
            string sql = "";
            string cs = Server.UrlDecode(Request["cs"].ToString());
            sql = "SELECT a.Number,a.KHMC, a.DZ,a.LXDH,a.Sheng_str,a.Shi_str,a.QuXian_str from KHGL_New as a left join KHGL_FXSJBXX as b on a.number=b.number where a.XSQD in('服务商','门店服务商') and a.Number LIKE '%" + fwsbh.Text.Trim() + "%' AND a.KHMC LIKE '%" + fwsmc.Text.Trim() + "%' AND a.SSXSQY='" + cs + "' and b.hzzt not like '%解约%'  union all select a.number,a.dkrxm as khmc,b.dz as dz,a.dkrlxdh as lxdh,b.sheng_str,b.shi_str,b.quxian_str from khgl_fwsdkrxx as a left join khgl_new as b on a.ssfwsbh=b.number where a.ssbsc like '" + cs + "' and  a.dkrxm like '%" + fwsmc.Text.Trim() + "%' and a.number like '%" + fwsbh.Text.Trim() + "%'";
            SqlDataSource1.SelectCommand = sql;
            SqlDataSource1.DataBind();
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
