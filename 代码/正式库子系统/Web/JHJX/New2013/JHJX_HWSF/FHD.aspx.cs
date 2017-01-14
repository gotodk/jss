using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_New2013_JHJX_HWSF_FHD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ViewState["Number"] = "";
            ViewState["GoBackUrl"] = "";
            if(Request.QueryString["Number"]!=null&&Request.QueryString["Number"].ToString()!="")
            {
                ViewState["Number"] = Request.QueryString["Number"].ToString().TrimStart('F'); ;
            }
            if (Request.QueryString["GoBackUrl"] != null && Request.QueryString["GoBackUrl"].ToString()!="")
            {
                ViewState["GoBackUrl"] = Request.QueryString["GoBackUrl"].ToString();
            }

            Bind();
        }
    }

    protected void Bind()
    {
        string strSQL = " select t.Number 发货单编号,t.F_FHDSCSJ 发货单下达时间,d.Z_HTBH 电子购货合同编号,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.Y_YSYDDDLYX where DDB.Number=d.Number),'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),'此前累计发货次数'=(select count(*) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT !='撤销'  and a.F_FHDSCSJ < t.F_FHDSCSJ),t.T_THSL 本次发货数量,'此前累计发货数量'=(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT in ('已录入发货信息','无异议收货','默认无异议收货','部分收货','已录入补发备注','补发货物无异议收货','有异议收货','有异议收货后无异议收货','卖家主动退货','请重新发货') and a.F_FHDSCSJ < t.F_FHDSCSJ), d.Z_SPBH 商品编号,d.Z_SPMC 商品名称,d.Z_GG 规格,d.Z_JJDW 计价单位,t.T_THSL 发货数量,t.ZBDJ 定标价格,t.T_DJHKJE 金额,t.T_SHRXM 收货人姓名,t.T_SHRLXFS 收货人联系电话,(d.Y_YSYDDSHQYsheng+d.Y_YSYDDSHQYshi+t.T_SHXXDZ) 收货地址,'发货人姓名'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),'发货人联系电话'=(select DL.I_LXRSJH from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),t.F_FPHM 发票编号,t.F_FPSFSHTH 发票是否随货同行,t.F_SFYWLTSYQ 特殊要求,t.F_WLTSYQSM 特殊要求说明,t.F_BFBZ 补发备注 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where t.Number='" + ViewState["Number"].ToString() + "'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            spanDH.InnerText = "单号：F" + ds.Tables[0].Rows[0]["发货单编号"].ToString();
            spanFHDTime.InnerText = ds.Tables[0].Rows[0]["发货单下达时间"].ToString();
            spanBuyMC.InnerText = ds.Tables[0].Rows[0]["买家名称"].ToString()+"：";
            spanDZGHHT.InnerText = "根据" + ds.Tables[0].Rows[0]["电子购货合同编号"].ToString() + "《电子购货合同》，及贵方T" + ds.Tables[0].Rows[0]["发货单编号"].ToString() + "提货单，我方已按要求将货物发出。贵方验收时以此发货单为准。";
            tdFHDH.InnerText = "T" + ds.Tables[0].Rows[0]["发货单编号"].ToString();

            tdCQLJTHCS.InnerText = ds.Tables[0].Rows[0]["此前累计发货次数"].ToString();
            tdBCFHSL.InnerText = ds.Tables[0].Rows[0]["本次发货数量"].ToString();
            tdCQLJFHSL.InnerText = ds.Tables[0].Rows[0]["此前累计发货数量"].ToString();            

            //数据列表部分

            spanSPBH.InnerText = ds.Tables[0].Rows[0]["商品编号"].ToString();
            ViewState["商品名称"] = ds.Tables[0].Rows[0]["商品名称"].ToString();
            ViewState["规格"] = ds.Tables[0].Rows[0]["规格"].ToString();
            //spanSPMC.InnerText = ds.Tables[0].Rows[0]["商品名称"].ToString();
            //spanGG.InnerText = ds.Tables[0].Rows[0]["规格"].ToString();
            spanJJDW.InnerText = ds.Tables[0].Rows[0]["计价单位"].ToString();
            spanTHSL.InnerText = ds.Tables[0].Rows[0]["发货数量"].ToString();
            spanDBJG.InnerText = ds.Tables[0].Rows[0]["定标价格"].ToString();
            spanJE.InnerText = ds.Tables[0].Rows[0]["金额"].ToString();


            tdSHRXM.InnerText = ds.Tables[0].Rows[0]["收货人姓名"].ToString();
            tdSHRLXDH.InnerText = ds.Tables[0].Rows[0]["收货人联系电话"].ToString();
            tdSHDZ.InnerText = ds.Tables[0].Rows[0]["收货地址"].ToString();
            tdFHRXM.InnerText = ds.Tables[0].Rows[0]["发货人姓名"].ToString();
            tdFHRLXDH.InnerText = ds.Tables[0].Rows[0]["发货人联系电话"].ToString();
            tdFPBH.InnerText = ds.Tables[0].Rows[0]["发票编号"].ToString();
            tdFPSFSHTX.InnerText = ds.Tables[0].Rows[0]["发票是否随货同行"].ToString();
            if (ds.Tables[0].Rows[0]["特殊要求"].ToString() == "是")
            {
                tdYSTSYQ.InnerText = ds.Tables[0].Rows[0]["特殊要求说明"].ToString();
            }
            else
            {
                tdYSTSYQ.InnerText = ds.Tables[0].Rows[0]["特殊要求"].ToString();
            }

            tdBFBZ.InnerText = ds.Tables[0].Rows[0]["补发备注"].ToString();

            spanBZ1.InnerText = "1、本发货单作为" + ds.Tables[0].Rows[0]["电子购货合同编号"].ToString() + "《电子购货合同》的附件，与" + ds.Tables[0].Rows[0]["电子购货合同编号"].ToString() + "《电子购货合同》具有同等法律效力。";
            spanBZ2.InnerText = "2、本次发货由" + ds.Tables[0].Rows[0]["卖家名称"].ToString() + "自主安排物流并承担相关费用及责任，运费已包含运险费。";
            
            ts.Visible = false;
            tbodyTR.Visible = true;
        }
        else
        {
            ts.Visible = true;
            tbodyTR.Visible = false;
        }
    }
    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(ViewState["GoBackUrl"].ToString());
    }
}