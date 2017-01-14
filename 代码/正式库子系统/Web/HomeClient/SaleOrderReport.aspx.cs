using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Hesion.Brick.Core;
using FMOP.DB;
using System.Text;
using System.Data.SqlClient;

public partial class Web_HomeClient_SalerOrderReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //获取销售单号
        string Number = Request["Number"];
        //如果单号没有正常传递，则返回上一页
        if (string.IsNullOrEmpty(Number))
            MessageBox.ShowAlertAndBack(this, "参数不全");
        //如果销售单不存在，则退回上一页
        if (BindParameters(Number) < 0)
            MessageBox.ShowAlertAndBack(this, "销售单不存在！");
        CrystalReportViewer1.DataBind();
    }

    /// <summary>
    /// 绑定报表的各项参数
    /// </summary>
    private int BindParameters(string Number)
    {
        //如果不存在此销售单，则直接返回
        SqlDataReader dr = DbHelperSQL.ExecuteReader("select * from CSZX_XSD where Number='" + Number + "'");
        if (!dr.HasRows)
        {
            dr.Close();
            return -1;
        }

        //绑定报表的各项参数
        dr.Read();
        CrystalDecisions.Web.ParameterCollection param = CrystalReportSource1.Report.Parameters;
        param[0].DefaultValue = dr["Number"].ToString();
        param[1].DefaultValue = dr["DDH"].ToString();
        param[2].DefaultValue = dr["KHJL"].ToString();
        param[3].DefaultValue = dr["FKFS"].ToString();
        param[4].DefaultValue = dr["KHBH"].ToString();
        param[5].DefaultValue = dr["KHMC"].ToString();
        param[6].DefaultValue = dr["DZ"].ToString();
        param[7].DefaultValue = dr["LXR"].ToString();
        param[8].DefaultValue = dr["LXDH"].ToString();
        param[9].DefaultValue = dr["JEZJ"].ToString();
        param[10].DefaultValue = dr["createUser"].ToString();
        param[11].DefaultValue = dr["FHR"].ToString();
        param[12].DefaultValue = dr["THSHR"].ToString();
        param[13].DefaultValue = Hesion.Tools.RMB.ConvertToRMB(dr["JEZJ"].ToString());
        dr.Close();
        return 0;

    }


}
