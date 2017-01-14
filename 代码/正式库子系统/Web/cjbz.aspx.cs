using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_cjbz : System.Web.UI.Page
{
    string tablename = "";
    string number = "";
    string bt_module = "";
    string lmctxt = "", lyctxt = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Globalization.CultureInfo ci = null;
        ci = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");
        System.Threading.Thread.CurrentThread.CurrentCulture = ci;

        if (!IsPostBack)
        {
            bt_module = Request["module"].ToString();
            number = Request["number"].ToString();
            this.mmm.Text = bt_module;
            this.iii.Text = number;            
            switch (bt_module)
            {
                case "OnLineOrder":
                    this.mctxt.Text = "【网站产品需求单】";
                    this.yctxt.Text = "OnLineOrder";

                    break;
                case "Web_WLZZFW_ZXZXFHY":
                    this.mctxt.Text = "【在线咨询】";
                    this.yctxt.Text = "Web_WLZZFW_ZXZXFHY";
                    break;
                case "Web_WLZZFW_ZXTSFHY":
                    this.mctxt.Text = "【在线投诉】";
                    this.yctxt.Text = "Web_WLZZFW_ZXTSFHY";
                    break;
                case "Web_WLZZFW_ZXJY":
                    this.mctxt.Text = "【在线建议】";
                    this.yctxt.Text = "Web_WLZZFW_ZXJY";
                    break;
                case "Web_HBHS_FHY":
                    this.mctxt.Text = "【环保回收】";
                    this.yctxt.Text = "Web_HBHS_FHY";
                    break;
                case "Web_GZBX_FHY":
                    this.mctxt.Text = "【故障报修】";
                    this.yctxt.Text = "Web_GZBX_FHY";
                    break;
                case "FWPT_YJYJJFK":
                    this.mctxt.Text = "【直通车意见与建议反馈】";
                    this.yctxt.Text = "FWPT_YJYJJFK";
                    break;
                case "FM_YJYJY":
                    this.mctxt.Text = "【用户直通车意见与建议反馈】";
                    this.yctxt.Text = "FM_YJYJY";
                    break;
                case "FWPT_XHDYHFK":
                    this.mctxt.Text = "【销货单用户反馈】";
                    this.yctxt.Text = "FWPT_XHDYHFK";
                    break;
                case "FM_YHJFJXG":
                    this.mctxt.Text = "【用户交付旧硒鼓】";
                    this.yctxt.Text = "FM_YHJFJXG";
                    break;
                case "FM_GZBX":
                    this.mctxt.Text = "【用户直通车故障报修】";
                    this.yctxt.Text = "FM_GZBX";
                    break;
                case "FM_XHDPJYQS":
                    this.mctxt.Text = "【用户直通车销货单评价与签收】";
                    this.yctxt.Text = "FM_XHDPJYQS";
   
                    break;
                default:
                    break;
            }
       
             string    upsql = "select * from  " + this.yctxt.Text + "  where Number='" + this.iii.Text + "' ";
        
            DataSet dsdsd = DbHelperSQL.Query(upsql);
            this.cljgbz.Text = dsdsd.Tables[0].Rows[0]["CLJGBZ"].ToString();
            ViewState["nrnr"] = this.cljgbz.Text;
            this.LB_clzt.Text = dsdsd.Tables[0].Rows[0]["CLZT"].ToString();
            ViewState["nrnr1"] = this.LB_clzt.Text;
            this.LB_clzt0.Text = dsdsd.Tables[0].Rows[0]["YXX"].ToString();
            ViewState["nrnr2"] = this.LB_clzt0.Text;
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string whow = "";
        string sql = "select Employee_Name,BM,GWMC from HR_Employees where Number='" + User.Identity.Name + "'";
        DataSet dswho = DbHelperSQL.Query(sql);
        whow = dswho.Tables[0].Rows[0]["Employee_Name"].ToString() + "-" + dswho.Tables[0].Rows[0]["BM"].ToString() + "-" + dswho.Tables[0].Rows[0]["GWMC"].ToString();
        string upsql = "";
        if (ViewState["nrnr"].ToString() == this.cljgbz.Text && ViewState["nrnr1"].ToString() == this.LB_clzt.Text && ViewState["nrnr2"].ToString() == this.LB_clzt0.Text)
        {
            Response.Write("<Script language ='javaScript'>alert('未修改任何信息，取消保存!');window.location.href='WorkFlow_View.aspx?module=" + this.mmm.Text + "';</Script>");
            Response.End();
        }
        else
        {
            upsql = "update  " + this.yctxt.Text + "  set CLJGBZ='" + this.cljgbz.Text + "\r\n以上信息由：[" + whow + "]\r\n编辑于：" + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString() + "\r\n=====================\r\n',CLZT='" + this.LB_clzt.Text + "',YXX='" + this.LB_clzt0.Text + "',CheckState=1 where Number='" + this.iii.Text + "' ";
        }
        
        DbHelperSQL.ExecuteSql(upsql);

        Response.Write("<Script language ='javaScript'>alert('保存成功!');window.location.href='WorkFlow_View.aspx?module=" + this.mmm.Text + "';</Script>");
        Response.End();
    }
}
