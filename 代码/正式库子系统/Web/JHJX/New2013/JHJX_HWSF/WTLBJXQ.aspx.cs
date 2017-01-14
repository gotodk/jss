using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Configuration;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;
using System.Threading;

public partial class Web_JHJX_New2013_JHJX_HWSF_WTLBJXQ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.lwjck.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dr["证明文件路径"].ToString().Replace(@"\", "/");

        if (!IsPostBack)
        {
            ViewState["Number"] = "";
            ViewState["GoBackUrl"] = "";
            if (Request.QueryString["Number"] != null && Request.QueryString["Number"].ToString() != "")
            {
                ViewState["Number"] = Request.QueryString["Number"].ToString().TrimStart('T'); ;
            }
            if (Request.QueryString["GoBackUrl"] != null && Request.QueryString["GoBackUrl"].ToString() != "")
            {
                ViewState["GoBackUrl"] = Request.QueryString["GoBackUrl"].ToString();
            }
            InitalData(ViewState["Number"].ToString().Trim());
           
        }
    }

    protected void InitalData(string num)
    {
        DataTable dt = DbHelperSQL.Query("select * from AAA_THDYFHDXXB where 'F'+Number='"+num+"'").Tables[0];
        if (dt != null && dt.Rows.Count > 0)
        {
            string fhzt = dt.Rows[0]["F_DQZT"].ToString().Trim();
            switch (fhzt)
            {
                case "请重新发货":
                case "撤销":
                    this.ysszp.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dt.Rows[0]["F_BUYQZXFHYSZP"].ToString().Replace(@"\", "/");
                    
                    txtqksm.Text = dt.Rows[0]["F_BUYQZXFHQKSM"].ToString().Trim();
                    lbcljg.Text = "同意重新发货";
                    tryyj.Visible = false;
                    trsl.Visible = false;
                    
                    break;
                case "部分收货":
                case "已录入补发备注":
                case "补发货物无异议收货":
                    this.ysszp.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dt.Rows[0]["F_BUYBFSHYSZP"].ToString().Replace(@"\", "/");
                    this.fhdyyj.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dt.Rows[0]["F_BUYBFSHFHDWLDYYJ"].ToString().Replace(@"\", "/");
                    lbqssl.Text = dt.Rows[0]["F_BUYBFSHSJQSSL"].ToString().Trim();
                    lbcljg.Text = dt.Rows[0]["F_DQZT"].ToString().Trim();
                    trsm.Visible = false;
                    break;

                case "有异议收货":
                case "有异议收货后无异议收货":
                case "卖家主动退货":
                    this.ysszp.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dt.Rows[0]["F_BUYYYYYSZP"].ToString().Replace(@"\", "/");
                    this.fhdyyj.HRef = "../../../picView/picView.aspx?path=http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + dt.Rows[0]["F_BUYYYYSHFHDWLDYYJ"].ToString().Replace(@"\", "/");
                    txtqksm.Text = dt.Rows[0]["F_BUYYYYQKSM"].ToString().Trim();
                    lbcljg.Text = dt.Rows[0]["F_DQZT"].ToString().Trim();
                    trsl.Visible = false;
                    break;

                default:
                    break;


            }
        }
 
    }
    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(ViewState["GoBackUrl"].ToString());
    }
}