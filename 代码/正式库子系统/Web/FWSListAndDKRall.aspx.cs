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

public partial class Web_FWSListAndDKRall : System.Web.UI.Page
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
            sql = "select * from ( (SELECT 'Number'=NK1.Number,'KHMC'=NK1.KHMC, 'DZ'=NK1.DZ,'LX'='服务商','SSBSC' = NK1.SSXSQY from KHGL_New as NK1 where XSQD like '%服务商%') union all  (SELECT             'Number'=KF.Number,'KHMC'=KF.DKRXM, 'DZ'='' ,'LX'='打款人','SSBSC' = KF.SSBSC from KHGL_FWSDKRXX as KF join KHGL_New as NK2 on KF.SSFWSBH = NK2.Number  where FWSXSQD like '%服务商%') ) as  FWSHDKR where Number like '%" + fwsbh.Text.Trim() + "%' and  KHMC like '%" + fwsmc.Text.Trim() + "%' ";
            //sql = "SELECT Number,KHMC, DZ,LXDH,Sheng_str,Shi_str,QuXian_str from KHGL_New where XSQD='服务商' and Number LIKE '%" + fwsbh.Text.Trim() + "%' AND KHMC LIKE '%" + fwsmc.Text.Trim() + "%' AND SSXSQY='" + cs + "'";
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
