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

public partial class Web_JHJX_PTFPYJXX_LRinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
 

            if (Request["Number"] != null && Request["Number"].ToString() != "")
            {
                spanNumber.InnerText = Request["Number"].ToString();
            }
            if (Request["type"] != null && Request["type"].ToString() != "")
            {
                spanType.InnerText = Request["type"].ToString();
            }
            BindData();

        }
    }
    private void BindData()
    {
        if (spanType.InnerText == "edit")
        {
            string sql_wlxx = "select * from AAA_PTFPYDXXB where Number='" + spanNumber.InnerText.Trim() + "'";
            DataSet ds_wlxx = DbHelperSQL.Query(sql_wlxx);
            if (ds_wlxx != null && ds_wlxx.Tables[0].Rows.Count > 0)
            {
                txtWLGS.Text = ds_wlxx.Tables[0].Rows[0]["wlgsmc"].ToString();
                txtWLDH.Text = ds_wlxx.Tables[0].Rows[0]["wldh"].ToString();
                spanFPDB.InnerText = ds_wlxx.Tables[0].Rows[0]["fpdb"].ToString();
                spanFPDH.InnerText = ds_wlxx.Tables[0].Rows[0]["fpdh"].ToString();
            }
        }

        string fpdbdh = "";
        if (spanType.InnerText == "edit")
        {
            fpdbdh = spanFPDB.InnerText + '_' + spanFPDH.InnerText;
        }
        else
        {
            fpdbdh = spanNumber.InnerText;
        }
        
        string sql_FP = "select ltrim(rtrim(TA001))+'_'+ltrim(rtrim(TA002)) as 编号, ltrim(rtrim(TA001)) as 单别,ltrim(rtrim(TA002)) as 单号,convert(varchar(10),convert(datetime,TA003),120) as 审核日期,ltrim(rtrim(TA004)) as 客户编号,ltrim(rtrim(TA008)) as 客户全称,TA070 as 平台管理机构编号,ltrim(rtrim(ME002)) as 平台管理机构,(case  ltrim(rtrim(TA011)) when 'A' then '增值税专用发票' when 'B' then '增值税普通发票'  else '其他' end) as 发票种类,cast(TA029+TA030 as numeric(18,2)) as 发票金额,(case when ltrim(rtrim(TA015))='' then '暂无' when TA015 is null then '暂无' else ltrim(rtrim(TA015)) end) as 发票号码,TA025 as 审核码 from ACRTA left join CMSME on TA070=ME001 where ltrim(rtrim(TA001))+'_'+ltrim(rtrim(TA002))='" + fpdbdh + "'";
        DataSet ds_FP = GetErpData.GetDataSet(sql_FP,"公司总部");
        if (ds_FP != null && ds_FP.Tables[0].Rows.Count > 0)
        {
            spanFPDB.InnerText = ds_FP.Tables[0].Rows[0]["单别"].ToString();
            spanFPDH.InnerText = ds_FP.Tables[0].Rows[0]["单号"].ToString();
            spanKHBH.InnerText = ds_FP.Tables[0].Rows[0]["客户编号"].ToString();
            spanKHQC.InnerText = ds_FP.Tables[0].Rows[0]["客户全称"].ToString();
            spanPTGLJG.InnerText = ds_FP.Tables[0].Rows[0]["平台管理机构"].ToString();
            spanFPLX.InnerText = ds_FP.Tables[0].Rows[0]["发票种类"].ToString();
            spanFPJE.InnerText = ds_FP.Tables[0].Rows[0]["发票金额"].ToString();
            spanSHSJ.InnerText = ds_FP.Tables[0].Rows[0]["审核日期"].ToString();
            spanFPHM.InnerText = ds_FP.Tables[0].Rows[0]["发票号码"].ToString();
        }
        else
        {
            spanFPBZ.InnerText = "ERP中未找到该单号的发票信息，请核实！";
        }

        string sql_ydxx = "select * from AAA_PTKPXXB where khbh='" + spanKHBH.InnerText + "' and zt='已生效'";
        DataSet ds_ydxx = DbHelperSQL.Query(sql_ydxx);
        if (ds_ydxx != null && ds_ydxx.Tables[0].Rows.Count > 0)
        {
            spanFPJSDWMC.InnerText = ds_ydxx.Tables[0].Rows[0]["fpjsdwmc"].ToString();
            spanFPJSDWDZ.InnerText = ds_ydxx.Tables[0].Rows[0]["fpjsdz"].ToString();
            spanFPJSLXR.InnerText = ds_ydxx.Tables[0].Rows[0]["fpjslxr"].ToString();
            spanFPJSLXRDH.InnerText = ds_ydxx.Tables[0].Rows[0]["fpjslxrdh"].ToString();
        }
        else
        {
            spanFPJSBZ.InnerText = "未找到该客户的有效发票邮递信息，请核实！";
        }
        
    }
    
    protected void btnCommit_Click(object sender, EventArgs e)
    {
        if (spanFPBZ.InnerText.Trim() != "")
        {
            MessageBox.ShowAlertAndBack(this, "ERP中未找到此单号的发票信息，请核实！");
        }
        if (spanFPJSBZ.InnerText.Trim() != "")
        {
            MessageBox.ShowAlertAndBack(this, "未找到该客户的有效发票邮递信息，请核实！");
        }
        if (txtWLGS.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请填写物流公司！");
        }
        if (txtWLDH.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请填写物流单号！");
        }

        if (spanType.InnerText == "commit")
        {
            WorkFlowModule WFM = new WorkFlowModule("AAA_PTFPYDXXB");
            string KeyNumber = WFM.numberFormat.GetNextNumber();

            ArrayList al_FMOP = new ArrayList();
            ArrayList al_ERP = new ArrayList();
            string sql_FMOP = "INSERT INTO [AAA_PTFPYDXXB]([Number],[FPDB],[FPDH],[PTGLJG],[KHBH],[KHMC],[FPLX],[FPJE],[FPH],[FPSCRQ],[SJFDWMC],[SJDZ],[SJR],[SJRDH],[SHZT],[WLGSMC],[WLDH],[ZHXGR],[ZHXGSJ],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber + "','" + spanFPDB.InnerText + "','" + spanFPDH.InnerText + "','" + spanPTGLJG.InnerText + "','" + spanKHBH.InnerText + "','" + spanKHQC.InnerText + "','" + spanFPLX.InnerText + "'," + spanFPJE.InnerText + ",'" + spanFPHM.InnerText + "','" + spanSHSJ.InnerText + "','" + spanFPJSDWMC.InnerText + "','" + spanFPJSDWDZ.InnerText + "','" + spanFPJSLXR.InnerText + "','" + spanFPJSLXRDH.InnerText + "','待审核','" + txtWLGS.Text.Trim() + "','" + txtWLDH.Text.Trim() + "','" + User.Identity.Name.ToString() + "','" + DateTime.Now.ToString() + "',1,'" + User.Identity.Name.ToString() + "','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
            al_FMOP.Add(sql_FMOP);
            string sql_ERP = "INSERT INTO [AAA_613ACRTA]([TA001],[TA002],[CreateTime],[CreateUser]) VALUES ('" + spanFPDB.InnerText + "','" + spanFPDH.InnerText + "' ,'" + DateTime.Now.ToString() + "','" + User.Identity.Name.ToString() + "')";
            al_ERP.Add(sql_ERP);

            bool suss = GetErpData.ExecuteSqlTran(al_ERP, al_FMOP, "公司总部");
            if (suss == false)
            {
                MessageBox.ShowAlertAndBack(this, "邮递信息保存失败！");
            }
            else
            {
                MessageBox.ShowAndRedirect(this, "物流信息保存成功！将为您跳转回列表页面！", "PTFPYJXX_LR.aspx");
            }
        }
        else if (spanType.InnerText == "edit")
        {
            string sql_update = "update AAA_PTFPYDXXB set [PTGLJG]='" + spanPTGLJG.InnerText + "',[KHBH]='" + spanKHBH.InnerText + "',[KHMC]='" + spanKHQC.InnerText + "',[FPLX]='" + spanFPLX.InnerText + "',[FPJE]='" + spanFPJE.InnerText + "',[FPH]='" + spanFPHM.InnerText + "',[FPSCRQ]='" + spanSHSJ.InnerText + "',[SJFDWMC]='" + spanFPJSDWMC.InnerText + "',[SJDZ]='" + spanFPJSDWDZ.InnerText + "',[SJR]='" + spanFPJSLXR.InnerText + "',[SJRDH]='" + spanFPJSLXRDH.InnerText + "',[WLGSMC]='" + txtWLGS.Text.Trim() + "',[WLDH]='" + txtWLDH.Text.Trim() + "',[ZHXGR]='" + User.Identity.Name.ToString() + "',[ZHXGSJ]='" + DateTime.Now.ToString() + "' where Number='"+spanNumber .InnerText +"'";
            try
            {
                DbHelperSQL.ExecuteSql(sql_update);
                MessageBox.ShowAndRedirect(this, "修改信息保存成功", "PTFPYJXX_LRinfo.aspx?type=" + spanType.InnerText + "&Number=" + spanNumber.InnerText);
            }
            catch (Exception ex)
            {
                MessageBox.ShowAlertAndBack(this, "修改信息保存失败！原因：" + ex.ToString());
            }
        }
    }

    //返回列表按钮操作    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (spanType.InnerText == "commit")
        {
            Response.Redirect("PTFPYJXX_LR.aspx");
        }
        else if (spanType.InnerText == "edit")
        {
            Response.Redirect("PTFPYJXX_Check.aspx");
        }
    }
}
