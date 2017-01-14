using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.Data;
using FMOP.DB;
using System.Configuration;
using System.Collections;

public partial class Web_JHJX_PTKPXXChange_Checkinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
 

            if (Request["id"] != null && Request["id"].ToString() != "")
            {
                spanID.InnerText = Request["id"].ToString();
            }
            BindData();

        }
    }
    private void BindData()
    {
        string sql = "select a.*,b.*,c.I_JYFMC from AAA_PTKPXX_BGSQ as a left join AAA_PTKPXXB as b on a.parentnumber=b.number left join AAA_DLZHXXB as c on b.dlyx=c.B_DLYX where id='" + spanID.InnerText + "'";
        DataSet ds = DbHelperSQL.Query(sql);
        
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            spanNumber.InnerText = ds.Tables[0].Rows[0]["parentnumber"].ToString();
            spanPTGLJG.InnerText = ds.Tables[0].Rows[0]["ptgljg"].ToString();
            spanKHBH.InnerText = ds.Tables[0].Rows[0]["khbh"].ToString().Trim() == "" ?"无":ds.Tables[0].Rows[0]["khbh"].ToString().Trim();
            spanDLYX.InnerText = ds.Tables[0].Rows[0]["dlyx"].ToString();
            spanJYFMC.InnerText = ds.Tables[0].Rows[0]["I_JYFMC"].ToString();
            spanJYZHLX.InnerText = ds.Tables[0].Rows[0]["jyzhlx"].ToString();
            spanZCLB.InnerText = ds.Tables[0].Rows[0]["zclb"].ToString();
            spanXXTJSJ.InnerText = ds.Tables[0].Rows[0]["sqtjsj"].ToString();
            spanCLZT.InnerText = ds.Tables[0].Rows[0]["clzt"].ToString();
            spanFPLX.InnerText = ds.Tables[0].Rows[0]["fplx"].ToString();
            spanDWMC.InnerText = ds.Tables[0].Rows[0]["dwmc"].ToString();
            this.linkBGXXSMJ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables [0].Rows[0]["bgsqsmj"].ToString().Replace(@"\", "/");
            if (ds.Tables[0].Rows[0]["ybnsrzgzm"].ToString().Trim() != "")
            {
                this.linkYBNSRZGZM.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["ybnsrzgzm"].ToString().Replace(@"\", "/");               
                spanXTPMC.InnerText = ds.Tables[0].Rows[0]["ybnsrzgzm"].ToString();
            }
            else
            {
                linkYBNSRZGZM.InnerText = "未上传新文件";
                linkYBNSRZGZM.HRef = "";
            }
                    
            if (ds.Tables[0].Rows[0]["clzt"].ToString() != "待处理")
            {
                btnSave.Enabled = false;
                btnSave.ToolTip = "只有“待处理”状态才可进行审核操作";
                btnBoHui.Enabled = false;
                btnBoHui.ToolTip = "只有“待处理”状态才可进行驳回操作";
                txtSHXX.Enabled = false;
            }

            //给开票信息的部分赋值
            rblFPLX.SelectedValue = ds.Tables[0].Rows[0]["fplx"].ToString();
            txtDWMC.Text = ds.Tables[0].Rows[0]["dwmc"].ToString();
            txtNSRSBH.Text = ds.Tables[0].Rows[0]["ybnsrsbh"].ToString();
            txtDWDZ.Text = ds.Tables[0].Rows[0]["dwdz"].ToString();
            txtLXDH.Text = ds.Tables[0].Rows[0]["lxdh"].ToString();
            txtKHH.Text = ds.Tables[0].Rows[0]["khh"].ToString();
            txtKHZH.Text = ds.Tables[0].Rows[0]["khzh"].ToString();
            if (ds.Tables[0].Rows[0]["ybnsrzgz"].ToString().Trim() != "")
            {
                linkYBNSRZGZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["ybnsrzgz"].ToString().Replace(@"\", "/");
                spanTPMC.InnerText = ds.Tables[0].Rows[0]["ybnsrzgz"].ToString();                
            }
            else
            {
                linkYBNSRZGZ.InnerText = "无原文件";
                linkYBNSRZGZ.HRef = "";                
            }
            txtSJDWMC.Text = ds.Tables[0].Rows[0]["fpjsdwmc"].ToString();
            txtSJDZ.Text = ds.Tables[0].Rows[0]["fpjsdz"].ToString();
            txtSJR.Text = ds.Tables[0].Rows[0]["fpjslxr"].ToString();
            txtSJRDH.Text = ds.Tables[0].Rows[0]["fpjslxrdh"].ToString();
            if (spanTPMC.InnerText.Trim() == "" && spanXTPMC.InnerText == "")
            {
                rblYBNSRZGZ.SelectedIndex = -1;
                rblYBNSRZGZ.Enabled = false;
            }
            else if (spanTPMC.InnerText != "" && spanXTPMC.InnerText == "")
            {
                rblYBNSRZGZ.SelectedIndex = 0;
                rblYBNSRZGZ.Enabled = false;
            }
            else if (spanTPMC.InnerText == "" && spanXTPMC.InnerText != "")
            {
                rblYBNSRZGZ.SelectedIndex = 1;
                rblYBNSRZGZ.Enabled = false;
            }
            else if (spanTPMC.InnerText != "" && spanXTPMC.InnerText != "")
            {
                rblYBNSRZGZ.SelectedIndex = 1;
                rblYBNSRZGZ.Enabled = true;
            }
        }
        else
        {
            btnSave.Enabled = false;
            btnBoHui.Enabled = false;
            txtSHXX.Enabled = false;
        }
    }

    //审核通过按钮操作
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //判断当前审核的是不是最新的信息
        object objZHGXSJ = DbHelperSQL.GetSingle("select sqtjsj from AAA_PTKPXX_BGSQ where id='" + spanID.InnerText.Trim() + "'");
        if (objZHGXSJ != null && Convert.ToDateTime(objZHGXSJ.ToString()) != Convert.ToDateTime(spanXXTJSJ.InnerText.Trim()))
        {
            MessageBox.ShowAndRedirect(this, "当前变更信息已经被修改过，请重新审核！", "PTKPXXChange_Checkinfo.aspx?id=" + spanID.InnerText.Trim());
        }

        btnSave.Enabled = false;
        btnBoHui.Enabled = false;
        tableJBXX .Visible =true;
        tableYDXX .Visible =true ;
        if (spanFPLX.InnerText == "增值税专用发票")
        {
            tableZYFPXX.Visible = true;
        } 
    }

    //驳回按钮操作
    protected void btnBoHui_Click(object sender, EventArgs e)
    {
        //判断当前审核的是不是最新的信息
        object objZHGXSJ = DbHelperSQL.GetSingle("select sqtjsj from AAA_PTKPXX_BGSQ where id='" + spanID.InnerText.Trim() + "'");
        if (objZHGXSJ != null && Convert.ToDateTime(objZHGXSJ.ToString()) != Convert.ToDateTime(spanXXTJSJ.InnerText.Trim()))
        {
            MessageBox.ShowAndRedirect(this, "当前变更信息已经被修改过，请重新审核！", "PTKPXXChange_Checkinfo.aspx?id=" + spanID.InnerText.Trim());
        }

        //驳回核通过，更新表中状态字段，同时在提交审核表中增加一条审核信息。
        if (txtSHXX.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请先将驳回原因填写在“审核备注”中！");
        }
        else if (txtSHXX.Text.Trim().Length > 150)
        {
            MessageBox.ShowAlertAndBack(this, "“审核备注”超过了150个字，请修改！");
        }
        else
        {
            ArrayList al = new ArrayList();
            string sql_updateBG = "update AAA_PTKPXX_BGSQ set slr='" + User.Identity.Name.ToString() + "',slsj='" + DateTime.Now.ToString() + "',slbz='" + txtSHXX.Text.Trim() + "',clzt='驳回' where id='" + spanID.InnerText + "'";
            al.Add(sql_updateBG);
            //集合经销平台平台提醒  
            string KeyNumber = jhjx_PublicClass.GetNextNumberZZ("AAA_PTTXXXB", "");
            string txwb = "尊敬的" + spanJYFMC.InnerText.Trim() + "，您的开票/邮递信息变更申请未通过交易平台审核，请修改后重提交。未通过原因：" + txtSHXX.Text.Trim();
            string Insert = " insert into  AAA_PTTXXXB ( Number, TXDXDLYX,TXDXYHM,TXDXJSZHLX,TXDXJSBH,TXDXJSLX,TXSJ,TXNRWB,SFXSG,CreateUser ) values ( '" + KeyNumber + "','" + spanDLYX.InnerText.Trim() + "','" + spanJYFMC.InnerText.Trim() + "','" + spanJYZHLX.InnerText.Trim() + "','" + spanKHBH.InnerText.Trim() + "','" + spanJYZHLX.InnerText.Trim() + "','" + DateTime.Now.ToString() + "','" + txwb + "','否','" + User.Identity.Name.ToString() + "' ) ";
            al.Add(Insert);
            try
            {
                DbHelperSQL.ExecSqlTran(al);
                MessageBox.ShowAndRedirect(this, "驳回操作成功，已向交易方发送审核不通过提醒！", "PTKPXXChange_Checkinfo.aspx?id=" + spanID.InnerText);
            }
            catch (Exception ex)
            {
                MessageBox.ShowAlertAndBack(this, "驳回操作失败！" + ex);
            }
        }
    }

    //返回列表按钮操作
    protected void btnCancle_Click(object sender, EventArgs e)
    {
        Response.Redirect("PTKPXXChange_Check.aspx");
    }
    protected void rblFPLX_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblFPLX.SelectedValue.ToString() == "增值税专用发票")
        {
            if (spanZCLB.InnerText.Trim() == "自然人")
            {
                MessageBox.Show(this, "该交易方注册类别为“自然人”，不能选择“增值税专用发票”。");
                rblFPLX.SelectedIndex = 0;
            }
            else if (spanTPMC.InnerText.Trim() == "" && spanXTPMC.InnerText.Trim() == "")
            {
                MessageBox.Show(this, "未上传一般纳税人资格证明文件，不能选择“增值税专用发票”");
                rblFPLX.SelectedIndex = 0;
            }
            else
            {
                tableZYFPXX.Visible = true;
            }
        }
        else if (rblFPLX.SelectedValue.ToString() == "增值税普通发票"  )
        {
            if (linkYBNSRZGZ.HRef.ToString().IndexOf("http://") >= 0)
            {
                MessageBox.Show(this, "该交易方注册类别为“单位”，且上传了一般纳税人资格证明，不能选择“增值税普通发票”。");
                rblFPLX.SelectedIndex = 1;
            }
            else
            {
                tableZYFPXX.Visible = false;
            }
        }            
    }
    //提交开票信息修改
    protected void btnQRXG_Click(object sender, EventArgs e)
    {
        //判断当前审核的是不是最新的信息
        object objZHGXSJ = DbHelperSQL.GetSingle("select sqtjsj from AAA_PTKPXX_BGSQ where id='" + spanID.InnerText.Trim() + "'");
        if (objZHGXSJ != null && Convert.ToDateTime(objZHGXSJ.ToString()) != Convert.ToDateTime(spanXXTJSJ.InnerText.Trim()))
        {
            MessageBox.ShowAndRedirect(this, "当前变更信息已经被修改过，请重新审核！", "PTKPXXChange_Checkinfo.aspx?id=" + spanID.InnerText.Trim());
        }

        if (rblFPLX.SelectedValue.ToString() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请选择发票类型");
        }
        if (txtDWMC.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请填写单位名称");
        }
        if (rblFPLX.SelectedValue.ToString() == "增值税专用发票")
        {
            if (linkYBNSRZGZ.HRef.ToString().Trim() == "" && linkYBNSRZGZM.HRef.ToString() == "")
            {
                MessageBox.ShowAlertAndBack(this, "增值税专用发票必须上传一般纳税人资格证明，当前没有此文件！");
            }
            if (txtNSRSBH.Text.Trim() == "")
            {
                MessageBox.ShowAlertAndBack(this, "请填写纳税人识别号");
            }
            if (txtDWDZ.Text.Trim() == "")
            {
                MessageBox.ShowAlertAndBack(this, "请填写单位地址");
            }
            if (txtLXDH.Text.Trim() == "")
            {
                MessageBox.ShowAlertAndBack(this, "请填写联系电话");
            }
            if (txtKHH.Text.Trim() == "")
            {
                MessageBox.ShowAlertAndBack(this, "请填写开户行");
            }
            if (txtKHZH.Text.Trim() == "")
            {
                MessageBox.ShowAlertAndBack(this, "请填写开户账号");
            }
            if (rblYBNSRZGZ.SelectedValue.ToString() == "")
            {
                MessageBox.ShowAlertAndBack(this, "请选择要使用的一般纳税人资格证明文件");
            }
        }
        if (txtSJDWMC.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请填写收件单位名称");
        }
        if (txtSJDZ.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请填写收件地址");
        }
        if (txtSJR.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请填写收件人名称");
        }
        if (txtSJRDH.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请填写收件人电话");
        }

        //执行修改程序
        ArrayList al = new ArrayList();
        string sql_updateKPXX = "";
        if (rblFPLX.SelectedValue == "增值税专用发票")
        {
            string ybnsrzgz = spanTPMC.InnerText.Trim();
            if (rblYBNSRZGZ.SelectedValue.ToString() == "使用新文件")
            {
                ybnsrzgz = spanXTPMC.InnerText.Trim();
            }

            sql_updateKPXX = "UPDATE [AAA_PTKPXXB]  SET [FPLX]='" + rblFPLX.SelectedValue.ToString() + "',[DWMC]='" + txtDWMC.Text.Trim() + "',[YBNSRSBH]='" + txtNSRSBH.Text.Trim() + "', [DWDZ] = '" + txtDWDZ.Text.Trim() + "',[LXDH] ='" + txtLXDH.Text.Trim() + "',[KHH] = '" + txtKHH.Text.Trim() + "',[KHZH] ='" + txtKHZH.Text.Trim() + "',[YBNSRZGZ] = '" + ybnsrzgz + "',[FPJSDWMC] = '" + txtSJDWMC.Text.Trim() + "',[FPJSDZ] ='" + txtSJDZ.Text.Trim() + "',[FPJSLXR] = '" + txtSJR.Text.Trim() + "',[FPJSLXRDH] = '" + txtSJRDH.Text.Trim() + "',[ZT] ='已生效',[ZHGXSJ] = '" + DateTime.Now.ToString() + "' WHERE Number='" + spanNumber.InnerText.Trim() + "'";
        }
        else
        {
            sql_updateKPXX = "UPDATE [AAA_PTKPXXB]  SET [FPLX]='" + rblFPLX.SelectedValue.ToString() + "',[DWMC]='" + txtDWMC.Text.Trim() + "',[YBNSRSBH]='" + txtNSRSBH.Text.Trim() + "', [DWDZ] = '',[LXDH] ='',[KHH] = '',[KHZH] ='',[YBNSRZGZ] = '',[FPJSDWMC] = '" + txtSJDWMC.Text.Trim() + "',[FPJSDZ] ='" + txtSJDZ.Text.Trim() + "',[FPJSLXR] = '" + txtSJR.Text.Trim() + "',[FPJSLXRDH] = '" + txtSJRDH.Text.Trim() + "',[ZT] ='已生效',[ZHGXSJ] = '" + DateTime.Now.ToString() + "' WHERE Number='" + spanNumber.InnerText.Trim() + "'";
        }
        al.Add(sql_updateKPXX);

        string sql_updateBG = "update AAA_PTKPXX_BGSQ set clzt='已修改',slr='" + User.Identity.Name.ToString() + "',slsj='" + DateTime.Now.ToString() + "',slbz='" + txtSHXX.Text.Trim() + "' where parentnumber='" + spanNumber.InnerText.Trim() + "' and id='" + spanID.InnerText.Trim() + "'";
        al.Add(sql_updateBG);

        //集合经销平台平台提醒  
        string KeyNumber = jhjx_PublicClass.GetNextNumberZZ("AAA_PTTXXXB", "");
        string txwb = "尊敬的" + spanJYFMC.InnerText.Trim() + "，您提交的开票/邮递信息更改申请已受理，交易平台已于" + DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日完成修改。如有异议，请致电400-688-8844。";
        string Insert = " insert into  AAA_PTTXXXB ( Number, TXDXDLYX,TXDXYHM,TXDXJSZHLX,TXDXJSBH,TXDXJSLX,TXSJ,TXNRWB,SFXSG,CreateUser ) values ( '" + KeyNumber + "','" + spanDLYX.InnerText.Trim() + "','" + spanJYFMC.InnerText.Trim() + "','" + spanJYZHLX.InnerText.Trim() + "','" + spanKHBH.InnerText.Trim() + "','" + spanJYZHLX.InnerText.Trim() + "','" + DateTime.Now.ToString() + "','" + txwb + "','否','" + User.Identity.Name.ToString() + "' ) ";
        al.Add(Insert);

        try
        {
            DbHelperSQL.ExecSqlTran(al);
            MessageBox.ShowAndRedirect(this, "发票/邮递信息修改成功。本条变更申请处理完成！", "PTKPXXChange_Checkinfo.aspx?id=" + spanID.InnerText.Trim());
        }
        catch (Exception ex)
        {
            MessageBox.ShowAlertAndBack(this, "发票/邮递信息修改失败！原因" + ex.ToString());
        }
    }
    //重置开票信息修改中自动带出的内容
    protected void btnQXXG_Click(object sender, EventArgs e)
    {
        BindData();
        btnSave.Enabled = false;
        btnBoHui.Enabled = false;
        tableJBXX.Visible = true;
        tableYDXX.Visible = true;
        if (spanFPLX.InnerText == "增值税专用发票")
        {
            tableZYFPXX.Visible = true;
        } 
    }

    //退出开票信息修改
    protected void btnTCXG_Click(object sender, EventArgs e)
    {
        BindData();
        btnSave.Enabled = true;
        btnBoHui.Enabled = true;
        tableJBXX.Visible = false;
        tableYDXX.Visible = false;
        tableZYFPXX.Visible = false;

    }
}