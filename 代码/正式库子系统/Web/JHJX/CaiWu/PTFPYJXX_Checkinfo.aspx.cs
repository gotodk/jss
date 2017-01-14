using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.Data;
using FMOP.DB;
using System.Collections;

public partial class Web_JHJX_PTFPYJXX_Checkinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
 

            if (Request["Number"] != null && Request["Number"].ToString() != "")
            {
                spanNumber.InnerText = Request["Number"].ToString();
            }
            BindData();

        }
    }

    private void BindData()
    {
        //string sql_FP = "select ltrim(rtrim(TA001))+'_'+ltrim(rtrim(TA002)) as 编号, ltrim(rtrim(TA001)) as 单别,ltrim(rtrim(TA002)) as 单号,convert(varchar(10),convert(datetime,TA003),120) as 审核日期,ltrim(rtrim(TA004)) as 客户编号,ltrim(rtrim(TA008)) as 客户全称,TA070 as 平台管理机构编号,ltrim(rtrim(ME002)) as 平台管理机构,(case  ltrim(rtrim(TA011)) when 'A' then '增值税专用发票' when 'B' then '增值税普通发票'  else '其他' end) as 发票种类,cast(TA029+TA030 as numeric(18,2)) as 发票金额,(case when ltrim(rtrim(TA015))='' then '暂无' when TA015 is null then '暂无' else ltrim(rtrim(TA015)) end) as 发票号码 from ACRTA left join CMSME on TA070=ME001 where ltrim(rtrim(TA001))+'_'+ltrim(rtrim(TA002))='" + spanNumber.InnerText + "'";
       // DataSet ds_FP = GetErpData.GetDataSet(sql_FP, "公司总部");
        // string sql_ydxx = "select * from AAA_PTKPXXB where khbh='" + spanKHBH.InnerText + "' and zt='已生效'";
        // DataSet ds_ydxx = DbHelperSQL.Query(sql_ydxx);  

        string sql = "select * from AAA_PTFPYDXXB where Number='" + spanNumber.InnerText + "'";
        DataSet ds_FP = DbHelperSQL.Query(sql);
        if (ds_FP != null && ds_FP.Tables[0].Rows.Count > 0)
        {
            spanFPDB.InnerText = ds_FP.Tables[0].Rows[0]["fpdb"].ToString();
            spanFPDH.InnerText = ds_FP.Tables[0].Rows[0]["fpdh"].ToString();
            spanKHBH.InnerText = ds_FP.Tables[0].Rows[0]["khbh"].ToString();
            spanKHQC.InnerText = ds_FP.Tables[0].Rows[0]["khmc"].ToString();
            spanPTGLJG.InnerText = ds_FP.Tables[0].Rows[0]["ptgljg"].ToString();
            spanFPLX.InnerText = ds_FP.Tables[0].Rows[0]["fplx"].ToString();
            spanFPJE.InnerText = Convert .ToDouble (ds_FP.Tables[0].Rows[0]["fpje"]).ToString("#0.00");
            spanSHSJ.InnerText = ds_FP.Tables[0].Rows[0]["fpscrq"].ToString();
            spanFPHM.InnerText = ds_FP.Tables[0].Rows[0]["fph"].ToString();

            spanFPJSDWMC.InnerText = ds_FP.Tables[0].Rows[0]["sjfdwmc"].ToString();
            spanFPJSDWDZ.InnerText = ds_FP.Tables[0].Rows[0]["sjdz"].ToString();
            spanFPJSLXR.InnerText = ds_FP.Tables[0].Rows[0]["sjr"].ToString();
            spanFPJSLXRDH.InnerText = ds_FP.Tables[0].Rows[0]["sjrdh"].ToString();

            spanWLGS .InnerText = ds_FP.Tables[0].Rows[0]["wlgsmc"].ToString();
            spanWLDH .InnerText = ds_FP.Tables[0].Rows[0]["wldh"].ToString();
            spanLRR.InnerText = Users.GetUserByNumber(ds_FP.Tables[0].Rows[0]["zhxgr"].ToString()).Name.ToString();
            spanLRSJ.InnerText= ds_FP.Tables[0].Rows[0]["zhxgsj"].ToString();

            spanSHZT.InnerText = ds_FP.Tables[0].Rows[0]["shzt"].ToString();
            if (ds_FP.Tables[0].Rows[0]["shzt"].ToString() != "待审核")
            {
                spanSHZT.InnerText = ds_FP.Tables[0].Rows[0]["shzt"].ToString() + "（" + Users.GetUserByNumber(ds_FP.Tables[0].Rows[0]["shr"].ToString()).Name.ToString() + "   " + ds_FP.Tables[0].Rows[0]["shsj"].ToString() + "）";
                txtSHXX.Text = ds_FP.Tables[0].Rows[0]["shxx"].ToString();
                txtSHXX.Enabled = false;
                btnSave.Visible =false;
                btnZuoFei.Visible = false;
            }
        }           
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //检查审核的发票信息是否与ERP的一致
        string sql_FP = "select ltrim(rtrim(TA001))+'_'+ltrim(rtrim(TA002)) as 编号, ltrim(rtrim(TA001)) as 单别,ltrim(rtrim(TA002)) as 单号,convert(varchar(10),convert(datetime,TA003),120) as 审核日期,ltrim(rtrim(TA004)) as 客户编号,ltrim(rtrim(TA008)) as 客户全称,TA070 as 平台管理机构编号,ltrim(rtrim(ME002)) as 平台管理机构,(case  ltrim(rtrim(TA011)) when 'A' then '增值税专用发票' when 'B' then '增值税普通发票'  else '其他' end) as 发票种类,cast(TA029+TA030 as numeric(18,2)) as 发票金额,(case when ltrim(rtrim(TA015))='' then '暂无' when TA015 is null then '暂无' else ltrim(rtrim(TA015)) end) as 发票号码,TA025 as 审核码 from ACRTA left join CMSME on TA070=ME001 where ltrim(rtrim(TA001))='"+spanFPDB.InnerText+"' and ltrim(rtrim(TA002))='" + spanFPDH .InnerText + "'";
         DataSet ds_FP = GetErpData.GetDataSet(sql_FP, "公司总部");
         if (ds_FP == null || ds_FP.Tables[0].Rows.Count <= 0)
         {
             MessageBox.ShowAlertAndBack(this, "ERP中未找到该单号对应的发票信息，请核实！");
         }
         else
         {
             if (ds_FP.Tables[0].Rows[0]["审核码"].ToString() != "Y")
             {
                 MessageBox.ShowAlertAndBack(this, "ERP中该单号对应的发票为未审核的状态，请核实！");
             }
             else if (ds_FP.Tables[0].Rows[0]["发票种类"].ToString() != spanFPLX.InnerText || ds_FP.Tables[0].Rows[0]["发票金额"].ToString() != spanFPJE.InnerText || ds_FP.Tables[0].Rows[0]["发票号码"].ToString() != spanFPHM.InnerText || ds_FP.Tables[0].Rows[0]["客户编号"].ToString() != spanKHBH.InnerText || ds_FP.Tables[0].Rows[0]["平台管理机构"].ToString() != spanPTGLJG.InnerText)
             {
                 MessageBox.ShowAlertAndBack(this, "ERP中的该单号对应的发票信息与当前显示的信息不一致，请核实修改！");
             }
         }

        //检查审核的信息是否与开票信息表中的收件信息一致
         string sql_ydxx = "select * from AAA_PTKPXXB where khbh='" + spanKHBH.InnerText + "' and zt='已生效'";
         DataSet ds_ydxx = DbHelperSQL.Query(sql_ydxx);
         if (ds_ydxx == null || ds_ydxx.Tables[0].Rows.Count <= 0)
         {
             MessageBox.ShowAlertAndBack(this, "未找到该客户的有效发票邮递信息，请核实！");
         }
         else
         {
             if (ds_ydxx.Tables[0].Rows[0]["fpjsdwmc"].ToString() != spanFPJSDWMC.InnerText || ds_ydxx.Tables[0].Rows[0]["fpjsdz"].ToString() != spanFPJSDWDZ.InnerText || ds_ydxx.Tables[0].Rows[0]["fpjslxr"].ToString() != spanFPJSLXR.InnerText || ds_ydxx.Tables[0].Rows[0]["fpjslxrdh"].ToString() != spanFPJSLXRDH.InnerText)
             {
                 MessageBox.ShowAlertAndBack(this, "该客户的有效发票邮递信息与当前显示的信息不一致，请核实修改！");
             }
         }

         if (txtSHXX.Text.Trim().Length > 150)
         {
             MessageBox.ShowAlertAndBack(this, "审核备注已经超过150个字，请修改！");
         }
        //检查通过的情况下，更新审核状态
         string sql_update = "update AAA_PTFPYDXXB set shzt='审核通过',shr='" + User.Identity.Name.ToString() + "',shsj='" + DateTime.Now.ToString() + "',shxx='" + txtSHXX.Text.Trim() + "' where number='" + spanNumber.InnerText + "'";
         try
         {
             DbHelperSQL.ExecuteSql(sql_update);
             MessageBox.ShowAndRedirect(this, "审核成功！将为您跳转回列表页面", "PTFPYJXX_Check.aspx");
         }
         catch (Exception ex)
         {
             MessageBox.ShowAlertAndBack(this, "审核失败！原因：" + ex.ToString());
         }
    }

    //返回列表按钮操作    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PTFPYJXX_Check.aspx");
    }

    protected void btnZuoFei_Click(object sender, EventArgs e)
    {
        if (txtSHXX.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请将作废原因填写在“审核备注”中！");
        }
        else if (txtSHXX.Text.Trim().Length > 150)
        {
            MessageBox.ShowAlertAndBack(this, "审核备注已经超过150个字，请修改！");
        }
        ArrayList al_FMOP = new ArrayList();
        ArrayList al_ERP = new ArrayList();
        string sql_update = "update AAA_PTFPYDXXB set shzt='作废',shr='" + User.Identity.Name.ToString() + "',shsj='" + DateTime.Now.ToString() + "',shxx='" + txtSHXX.Text.Trim() + "' where number='" + spanNumber.InnerText + "'";
        al_FMOP.Add(sql_update);
        string sql_erpdel = "delete from AAA_613ACRTA where TA001='" + spanFPDB.InnerText + "' and TA002='" + spanFPDH.InnerText + "'";
        al_ERP.Add(sql_erpdel);

        bool succ = GetErpData.ExecuteSqlTran(al_ERP, al_FMOP, "公司总部");
        if (succ == true)
        {
            MessageBox.ShowAndRedirect(this, "作废操作成功！将为您跳转回列表页面！", "PTFPYJXX_Check.aspx");
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "作废操作失败！");
        }
        
    }
}