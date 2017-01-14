using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
using System.Collections;
using System.Configuration;
using ShareLiu.DXS;

public partial class Web_JHJX_JHJX_KPXXBGSH : System.Web.UI.Page
{
   public static string u = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string number = "";
           
            if (Request["number"] != null && Request["number"].ToString() != null)
            {
                number = Request["number"].ToString();
            }

            iptnumber.Value = number;

            //显示申请表内容
            DataSet ds = DbHelperSQL.Query("select * from ZZ_KPXXBGSQB where number='" + number + "'");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                txtYHM.Text = ds.Tables[0].Rows[0]["YHM"].ToString();
                txtDLYX.Text = ds.Tables[0].Rows[0]["DLZH"].ToString();
                txtZHLX.Text = ds.Tables[0].Rows[0]["JSZHLX"].ToString();
                txtSQSJ.Text = ds.Tables[0].Rows[0]["createtime"].ToString();
                txtDQFPLX.Text = ds.Tables[0].Rows[0]["YFPLX"].ToString();
                txtDQKPXX.Text = ds.Tables[0].Rows[0]["YKPXX"].ToString();
                txtXFPLX.Text = ds.Tables[0].Rows[0]["XFPLX"].ToString();
                txtXKPXX.Text = ds.Tables[0].Rows[0]["XKPXX"].ToString();
                txtSHZT.Text = ds.Tables[0].Rows[0]["SHZT"].ToString();
                u = ds.Tables[0].Rows[0]["FilePath"].ToString().Trim();

            }
            if (txtSHZT.Text.Trim() != "待审核")
            {
                if(ds.Tables[0].Rows.Count>0 && ds.Tables[0].Rows[0]["SHJG"]!=null)
                rblCLJG.SelectedValue = ds.Tables[0].Rows[0]["SHJG"].ToString();
                rblCLJG.Enabled = false;
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["BZ"] != null)
                txtCLBZ.Text = ds.Tables[0].Rows[0]["BZ"].ToString();
                txtCLBZ.Enabled = false;
                btnPass.Enabled = false;
                btnPass.ToolTip = "已处理完成";
            }
            
            if (txtXFPLX.Text.Trim() == "增值税专用发票")
            {
                nsrzz.Visible = true;
                btnChakan.Visible = true;
            }
            else
            {
                nsrzz.Visible = false;
                btnChakan.Visible = false;

            }
        }
    }
    protected void btnPass_Click(object sender, EventArgs e)
    {
        if (rblCLJG.SelectedValue.ToString() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请选择处理结果！");
        }

        string num = iptnumber.Value.ToString();
        string gh = User.Identity.Name.ToString();
        string xm = Users.GetUserByNumber(gh).Name.ToString();
        string time = DateTime.Now.ToString();
       

        string sql1 = "update ZZ_KPXXBGSQB set BZ='" + txtCLBZ.Text.Trim() + "',SHZT='"+txtSHZT.Text.Trim()+"',SHJG='" + rblCLJG.SelectedValue.ToString() + "',checkstate=1,SHR='" + xm + "',SHSJ='" + time + "' where number='" + num + "'";

        List<string> ls = new List<string>();
        ls.Add(sql1);

        if (txtZHLX.Text=="买家账户")
        {
            string sql2 = "update ZZ_BUYJBZLXXB set YXKPLX='"+txtXFPLX.Text.Trim()+"',YXKPXX='"+txtXKPXX.Text.Trim()+"' where DLYX='"+txtDLYX.Text.Trim()+"'";
            ls.Add(sql2);
        }
        else if (txtZHLX.Text == "卖家账户")
        {
           string sql2 = "update ZZ_BUYJBZLXXB set YXKPLX='" + txtXFPLX.Text.Trim() + "',YXKPXX='" + txtXKPXX.Text.Trim() + "' where DLYX='" + txtDLYX.Text.Trim() + "'";
           string sql3 = "update ZZ_SELJBZLXXB set YXKPLX='" + txtXFPLX.Text.Trim() + "',YXKPXX='" + txtXKPXX.Text.Trim() + "' where DLYX='" + txtDLYX.Text.Trim() + "'";
           ls.Add(sql2);
           ls.Add(sql3);

        }
        else if(txtZHLX.Text == "经纪人账户")
        {
            string sql2 = "update ZZ_BUYJBZLXXB set YXKPLX='" + txtXFPLX.Text.Trim() + "',YXKPXX='" + txtXKPXX.Text.Trim() + "' where DLYX='" + txtDLYX.Text.Trim() + "'";
            //string sql3 = "update ZZ_SELJBZLXXB set YXKPLX='" + txtXFPLX.Text.Trim() + "',YXKPXX='" + txtXKPXX.Text.Trim() + "' where DLYX='" + txtDLYX.Text.Trim() + "'";
            string sql4 = "update ZZ_DLRJBZLXXB set YXKPLX='" + txtXFPLX.Text.Trim() + "',YXKPXX='" + txtXKPXX.Text.Trim() + "' where DLYX='" + txtDLYX.Text.Trim() + "'";
            ls.Add(sql2);
            //ls.Add(sql3);
            ls.Add(sql4);
        }
        int count = DbHelperSQL.ExecuteSqlTran(ls);
        if (count > 0)
        {
            Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('处理完成，申请单状态已更新为“"+txtSHZT.Text+"”！');window.top.frames['rightFrame'].pagesubmit('jump');window.top.frames['rightFrame'].my_closediag();</script>");
            Response.End();
        }        
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=\"javascript\" type=\"text/javascript\">window.top.frames['rightFrame'].pagesubmit('jump');window.top.frames['rightFrame'].my_closediag();</script>");
        Response.End();

    }

    protected void txtXFPLX_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnChakan_Click(object sender, EventArgs e)
    {
      
        string url = "http://" +ConfigurationManager.ConnectionStrings["onlineFWPT"].ToString()+"/JHJXPT/SaveDir/" + u.Replace(@"\", "/");
        Response.Write("<script language='javascript'>window.open('"+ url+"');</script>");
       
    }
    protected void rblCLJG_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblCLJG.SelectedIndex == 0)
        {
            txtSHZT.Text = "待审核";
 
        }
        else if (rblCLJG.SelectedIndex == 1)
        {
            txtSHZT.Text = "已审核";
        }
        else
        {
            txtSHZT.Text = "未通过";
        }
    }
}