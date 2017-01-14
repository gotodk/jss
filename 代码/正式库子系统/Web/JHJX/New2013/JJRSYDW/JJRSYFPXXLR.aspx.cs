using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_New2013_JJRSYDW_JJRSYFPXXLR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            if (Request["number"] != null && Request["number"].ToString() != "")
            {
                Tab1.Text = "发票信息录入修改";
                txtJJRBH.Visible = false;
                txtJJRBH1.Visible = true;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                btnUpdate.Visible = true;
                ViewState["Number"] = Request["number"].ToString();                
                btnGoBack.Visible = true;
                Bind();
            }
            else
            {
                txtJJRBH.Visible = true;
                txtJJRBH1.Visible = false;
                btnSave.Click += btnSave_Click;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnUpdate.Visible = false;
            }
        }
        
    }
    protected void Bind()
    {
        string str = "select a.*,isnull(SYZJE-YZQJE,0.00) 缺票收益金额,b.I_DSFCGZT from  AAA_DWJJRSYZQMXB a left join AAA_DLZHXXB b on a.JJRBH=b.J_JJRJSBH  where a.Number='" + ViewState["Number"].ToString() + "'";
        DataSet ds = DbHelperSQL.Query(str);
        if (ds != null&&ds.Tables[0].Rows.Count>0)
        {
            txtJJRBH.Text = ds.Tables[0].Rows[0]["JJRBH"].ToString();
            txtJJRBH1.Text = ds.Tables[0].Rows[0]["JJRBH"].ToString();
            txtJJRMC.Text = ds.Tables[0].Rows[0]["JJRMC"].ToString();
            txtSSFGS.Text = ds.Tables[0].Rows[0]["SSFGS"].ToString();
            txtDSFCGZT.Text = ds.Tables[0].Rows[0]["I_DSFCGZT"].ToString();
            txtQPSYJE.Text = ds.Tables[0].Rows[0]["缺票收益金额"].ToString();
            txtHGFPJE.Text = ds.Tables[0].Rows[0]["BCYSHGFPJE"].ToString();
            txtKZQJE.Text = ds.Tables[0].Rows[0]["BCKZQJE"].ToString();
            txtFPSQTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["SQFPRQ"].ToString()).ToString("yyyy-MM-dd"); ;
            dropFPSFYRZ.SelectedValue = ds.Tables[0].Rows[0]["FPSFYRZ"].ToString();
            txtBZ.Text = ds.Tables[0].Rows[0]["BZ"].ToString();
            hidSYZJE.Value = ds.Tables[0].Rows[0]["SYZJE"].ToString();
            hidYZQJE.Value = ds.Tables[0].Rows[0]["YZQJE"].ToString();
        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('未查询到编号为" + ViewState["Number"].ToString() + "的数据，不能进行修改！');</script>");
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnGoBack.Visible = true;
            return;
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {       
        if (txtBZ.Text.Contains("'"))
        {
            txtBZ.Text=txtBZ.Text.Replace("'","‘");
        }
        try
        {

            string strSelect = "select I_DSFCGZT from AAA_DLZHXXB where J_JJRJSBH='" + txtJJRBH.Text.Trim() + "'";
            object jsbh = DbHelperSQL.GetSingle(strSelect);
            if (jsbh != null && jsbh.ToString() != "")
            {
                if (jsbh.ToString() != "开通")
                {
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('经纪人编号为" + txtJJRBH.Text.Trim() + "的经纪人尚未开通第三方存管，不能提交！');</script>");
                    return;
                }
                else
                {
                    string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_DWJJRSYZQMXB", ""); //生成主键
                    string str = "INSERT INTO [AAA_DWJJRSYZQMXB] ([Number],[SSFGS],[JJRBH],[JJRMC],[SYZJE],[YZQJE],[BCYSHGFPJE],[BCKZQJE],[SQFPRQ],[FPSFYRZ],SHZT,[BZ],NextChecker,[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES('" + Number + "','" + txtSSFGS.Text.Trim() + "','" + txtJJRBH.Text.Trim() + "','" + txtJJRMC.Text.Trim() + "','" + Convert.ToDouble(hidSYZJE.Value.Trim()).ToString("#0.00") + "','" + Convert.ToDouble(hidYZQJE.Value.Trim()).ToString("#0.00") + "'," +Convert.ToDouble(txtHGFPJE.Text.Trim()).ToString("#0.00") + "," + Convert.ToDouble(txtKZQJE.Text.Trim()).ToString("#0.00") + ",'" + txtFPSQTime.Text.Trim() + "','" + dropFPSFYRZ.SelectedValue.Trim() + "','待审核','" + txtBZ.Text.Trim() + "','" + User.Identity.Name.ToString() + "',1,'" + User.Identity.Name.ToString() + "',getdate(),getdate())";
                    int i = DbHelperSQL.ExecuteSql(str);
                    if (i > 0)
                    {
                        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('发票信息录入成功！');</script>");
                        txtJJRBH.Text = "";
                        txtJJRMC.Text = "";
                        txtSSFGS.Text = "";
                        txtDSFCGZT.Text = "";
                        txtQPSYJE.Text = "";
                        txtHGFPJE.Text = "";
                        txtKZQJE.Text = "";
                        txtFPSQTime.Text = "";
                        dropFPSFYRZ.SelectedValue = "是";
                        txtBZ.Text = "";
                    }
                    else
                    {
                        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('发票信息录入失败！');</script>");
                    }
                }
            }
            else
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('未查询到" + txtJJRBH.Text.Trim() + "经纪人的信息，不能提交！');</script>");
                return;
            }
        }
        catch(Exception ex)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('" + ex.Message + "');</script>");
        }
        
        //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('请将信息添加完整！');</script>");
        //this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Alert.confirm('恭喜您提交成功！！！', function () {  window.location.href='dingdanxiangqing.aspx';  });</script>");
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (txtBZ.Text.Contains("'"))
        {
            txtBZ.Text = txtBZ.Text.Replace("'", "‘");
        }
        try
        {
            string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_DWJJRSYZQMXB", ""); //生成主键
            string str = "Update [AAA_DWJJRSYZQMXB] set SSFGS='" + txtSSFGS.Text.Trim() + "',JJRBH='" + txtJJRBH.Text.Trim() + "',JJRMC='" + txtJJRMC.Text.Trim() + "',SYZJE='" + Convert.ToDouble(hidSYZJE.Value.Trim()).ToString("#0.00") + "',YZQJE='" + Convert.ToDouble(hidYZQJE.Value.Trim()).ToString("#0.00") + "',BCYSHGFPJE=" + Convert.ToDouble(txtHGFPJE.Text.Trim()).ToString("#0.00") + ",BCKZQJE=" + Convert.ToDouble(txtKZQJE.Text.Trim()).ToString("#0.00") + ",SQFPRQ='" + txtFPSQTime.Text.Trim() + "',FPSFYRZ='" + dropFPSFYRZ.SelectedValue.Trim() + "',BZ='" + txtBZ.Text.Trim() + "',ZHXGR='" + User.Identity.Name.ToString() + "',ZHXGSJ=getdate() where Number='"+ViewState["Number"].ToString()+"'";
            int i = DbHelperSQL.ExecuteSql(str);
            if (i > 0)
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('发票信息录入修改成功！');</script>");
            }
            else
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('发票信息录入修改失败！');</script>");
            }
        }
        catch (Exception ex)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('" + ex.Message + "');</script>");
        }

        //this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('请将信息添加完整！');</script>");
        //this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' >  window.top.Alert.confirm('恭喜您提交成功！！！', function () {  window.location.href='dingdanxiangqing.aspx';  });</script>");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtJJRBH.Text = "";
        txtJJRMC.Text = "";
        txtSSFGS.Text = "";
        txtDSFCGZT.Text = "";
        txtQPSYJE.Text = "";
        txtHGFPJE.Text = "";
        txtKZQJE.Text = "";
        txtFPSQTime.Text = "";
        dropFPSFYRZ.SelectedValue = "是";
        txtBZ.Text = "";
    }
    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("JJRSYZQMXB.aspx");
    }
}