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
using System.Configuration;
public partial class Web_JHJX_PTKPXX_Checkinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
 

            if (Request ["Number"]!=null&& Request["Number"].ToString() != "")
            {
                spanNumber.InnerText = Request["Number"].ToString();
            }
            BindData();
            
        }
    }
    private void BindData()
    {
        string sql = "select a.*,b.*,c.I_JYFMC from AAA_PTKPXXB as a left join AAA_DLZHXXB as c on a.dlyx=c.B_DLYX left join (select top 1 * from AAA_PTKPXXB_TJSHXX where parentnumber='"+spanNumber.InnerText +"' order by tjsj desc) as b  on a.number=b.parentnumber where a.number='" + spanNumber.InnerText + "'";
        DataSet ds = DbHelperSQL.Query(sql);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            spanPTGLJG.InnerText = ds.Tables[0].Rows[0]["ptgljg"].ToString();
            spanKHBH.InnerText = ds.Tables[0].Rows[0]["khbh"].ToString().Trim() == "" ? "无" : ds.Tables[0].Rows[0]["khbh"].ToString().Trim();
            spanDLYX.InnerText = ds.Tables[0].Rows[0]["dlyx"].ToString();
            spanJYFMC.InnerText = ds.Tables[0].Rows[0]["I_JYFMC"].ToString();
            spanJYZHLX.InnerText = ds.Tables[0].Rows[0]["jyzhlx"].ToString();
            spanZCLB.InnerText = ds.Tables[0].Rows[0]["zclb"].ToString();
            spanXXTJSJ.InnerText = ds.Tables[0].Rows[0]["zhgxsj"].ToString();
            spanZT.InnerText = ds.Tables[0].Rows[0]["zt"].ToString();
            spanFPLX.InnerText = ds.Tables[0].Rows[0]["fplx"].ToString();
            spanDWMC.InnerText = ds.Tables[0].Rows[0]["dwmc"].ToString();
            if (ds.Tables[0].Rows[0]["fplx"].ToString() == "增值税专用发票")
            {
                spanNSRSBH.InnerText = ds.Tables[0].Rows[0]["ybnsrsbh"].ToString();
                spanDWDZ.InnerText = ds.Tables[0].Rows[0]["dwdz"].ToString();                
                spanLXDH.InnerText = ds.Tables[0].Rows[0]["lxdh"].ToString();
                spanKHH.InnerText = ds.Tables[0].Rows[0]["khh"].ToString();
                spanKHZH.InnerText = ds.Tables[0].Rows[0]["khzh"].ToString();
                linkYBNSRZGZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["ybnsrzgz"].ToString().Replace(@"\", "/");
                tableZYFPXX.Visible = true;
            }
            spanFPJSDWMC.InnerText = ds.Tables[0].Rows[0]["fpjsdwmc"].ToString();
            spanFPJSDWDZ.InnerText = ds.Tables[0].Rows[0]["fpjsdz"].ToString();
            spanFPJSLXR.InnerText = ds.Tables[0].Rows[0]["fpjslxr"].ToString();
            spanFPJSLXRDH.InnerText = ds.Tables[0].Rows[0]["fpjslxrdh"].ToString();

            if (ds.Tables[0].Rows[0]["shr"].ToString() != "")
            {
                spanSHJG.InnerText = ds.Tables[0].Rows[0]["shjg"].ToString();
                spanSHXX.InnerText = ds.Tables[0].Rows[0]["shxx"].ToString();
                spanSHR.InnerText = ds.Tables[0].Rows[0]["shr"].ToString();
                spanSHSJ.InnerText = ds.Tables[0].Rows[0]["shsj"].ToString();
            }
            else
            {
                spanSHJG.InnerText = "无";
            }
            if (ds.Tables[0].Rows[0]["zt"].ToString() != "待审核")
            {
                btnSave.Enabled = false;
                btnSave.ToolTip = "只有“待审核”状态才可进行审核操作";
                btnBoHui.Enabled = false;
                btnBoHui.ToolTip = "只有“待审核”状态才可进行驳回操作";
                txtSHXX.Enabled = false;
            }
        }
        else
        {
            btnSave.Enabled = false;
            btnBoHui.Enabled = false;
            txtSHXX.Enabled = false;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //判断当前审核的是不是最新的信息
        object objZHGXSJ = DbHelperSQL.GetSingle("select zhgxsj from AAA_PTKPXXB where number='" + spanNumber.InnerText.Trim() + "'");
        if (objZHGXSJ != null && Convert.ToDateTime(objZHGXSJ.ToString()) != Convert.ToDateTime(spanXXTJSJ.InnerText.Trim()))
        {
            MessageBox.ShowAndRedirect(this, "当前信息已经被修改过，请重新审核！", "PTKPXX_Checkinfo.aspx?number=" + spanNumber.InnerText);
        }
        if (txtSHXX.Text.Trim().Length > 150)
        {
            MessageBox.ShowAlertAndBack(this, "“本次审核备注”已经超过了150个字，请修改！");
        }

        //审核通过，更新表中状态字段，同时在提交审核表中增加一条审核信息。
        ArrayList al = new ArrayList();
        string sql_insert = "INSERT INTO [AAA_PTKPXXB_TJSHXX] ([parentNumber],[TJSJ],[SHR],[SHSJ],[SHJG],[SHXX]) VALUES('" + spanNumber.InnerText + "','" + spanXXTJSJ.InnerText + "' ,'" + User.Identity.Name.ToString() + "','" + DateTime.Now.ToString() + "','审核通过','" + txtSHXX.Text.Trim() + "')";
        al.Add(sql_insert);
        string sql_update = "UPDATE AAA_PTKPXXB set ZT='已生效' where number='" + spanNumber.InnerText + "' ";
        al.Add(sql_update);
        //集合经销平台平台提醒  
        string KeyNumber = jhjx_PublicClass.GetNextNumberZZ("AAA_PTTXXXB", "");
        string txwb = "尊敬的"+spanJYFMC .InnerText .Trim ()+"，您提交的开票/邮递信息已于"+DateTime .Now .Year.ToString ()+"年"+DateTime .Now .Month .ToString ()+"月"+DateTime.Now.Day .ToString ()+"日审核通过。";
        string Insert = " insert into  AAA_PTTXXXB ( Number, TXDXDLYX,TXDXYHM,TXDXJSZHLX,TXDXJSBH,TXDXJSLX,TXSJ,TXNRWB,SFXSG,CreateUser ) values ( '" + KeyNumber + "','" + spanDLYX.InnerText.Trim() + "','" + spanJYFMC.InnerText .Trim () + "','" + spanJYZHLX.InnerText.Trim() + "','" + spanKHBH.InnerText.Trim() + "','" + spanJYZHLX.InnerText.Trim() + "','" + DateTime.Now.ToString() + "','" + txwb + "','否','" + User.Identity.Name.ToString() + "' ) ";
       al.Add(Insert);

        try
        {
            DbHelperSQL.ExecSqlTran(al);
            MessageBox.ShowAndRedirect(this, "审核操作成功，已向交易方发送审核通过提醒！", "PTKPXX_Checkinfo.aspx?Number=" + spanNumber.InnerText);
        }
        catch (Exception ex)
        {
            MessageBox.ShowAlertAndBack(this, "审核操作失败！" + ex);
        }

    }
    protected void btnBoHui_Click(object sender, EventArgs e)
    {
        //判断当前审核的是不是最新的信息
        object objZHGXSJ = DbHelperSQL.GetSingle("select zhgxsj from AAA_PTKPXXB where number='" + spanNumber.InnerText.Trim() + "'");
        if (objZHGXSJ != null && Convert.ToDateTime(objZHGXSJ.ToString()) != Convert.ToDateTime(spanXXTJSJ.InnerText.Trim()))
        {
            MessageBox.ShowAndRedirect(this, "当前信息已经被修改过，请重新审核！", "PTKPXX_Checkinfo.aspx?number=" + spanNumber.InnerText);
        }

        //驳回核通过，更新表中状态字段，同时在提交审核表中增加一条审核信息。
        if (txtSHXX.Text.Trim() == "")
        {
            MessageBox.ShowAlertAndBack(this, "请先将驳回原因填写在“本次审核备注”中！");
        }
        else if (txtSHXX.Text.Trim().Length > 150)
        {
            MessageBox.ShowAlertAndBack(this, "“本次审核备注”已经超过了150个字，请修改！");
        }
        else
        {
            ArrayList al = new ArrayList();
            string sql_insert = "INSERT INTO [AAA_PTKPXXB_TJSHXX] ([parentNumber],[TJSJ],[SHR],[SHSJ],[SHJG],[SHXX]) VALUES('" + spanNumber.InnerText + "','" + spanXXTJSJ.InnerText + "' ,'" + User.Identity.Name.ToString() + "','" + DateTime.Now.ToString() + "','驳回','" + txtSHXX.Text.Trim() + "')";
            al.Add(sql_insert);
            string sql_update = "UPDATE AAA_PTKPXXB set ZT='驳回' where number='" + spanNumber.InnerText + "' ";
            al.Add(sql_update);
            //集合经销平台平台提醒  
            string KeyNumber = jhjx_PublicClass.GetNextNumberZZ("AAA_PTTXXXB", "");
            string txwb = "尊敬的" + spanJYFMC.InnerText.Trim() + "，您的开票/发票邮递信息未通过交易平台审核，请修改后重新提交。未通过原因：" + txtSHXX.Text.Trim();
            string Insert = " insert into  AAA_PTTXXXB ( Number, TXDXDLYX,TXDXYHM,TXDXJSZHLX,TXDXJSBH,TXDXJSLX,TXSJ,TXNRWB,SFXSG,CreateUser ) values ( '" + KeyNumber + "','" + spanDLYX.InnerText.Trim() + "','" + spanJYFMC.InnerText.Trim() + "','" + spanJYZHLX.InnerText.Trim() + "','" + spanKHBH.InnerText.Trim() + "','" + spanJYZHLX.InnerText.Trim() + "','" + DateTime.Now.ToString() + "','" + txwb + "','否','" + User.Identity.Name.ToString() + "' ) ";
            al.Add(Insert);
            try
            {
                DbHelperSQL.ExecSqlTran(al);
                MessageBox.ShowAndRedirect(this, "驳回操作成功，已向交易方发送审核不通过提醒！", "PTKPXX_Checkinfo.aspx?Number=" + spanNumber.InnerText);
            }
            catch (Exception ex)
            {
                MessageBox.ShowAlertAndBack(this, "驳回操作失败！" + ex);
            }
        }
    }
    protected void btnCancle_Click(object sender, EventArgs e)
    {
        Response.Redirect("PTKPXX_Check.aspx");
    }
}