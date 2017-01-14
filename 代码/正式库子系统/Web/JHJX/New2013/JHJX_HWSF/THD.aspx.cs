using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_New2013_JHJX_HWSF_THD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ViewState["Number"] = "";
            ViewState["GoBackUrl"] = "";
            if(Request.QueryString["Number"]!=null&&Request.QueryString["Number"].ToString()!="")
            {
                ViewState["Number"] = Request.QueryString["Number"].ToString().TrimStart('T'); ;
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
        string strSQL = "select t.F_DQZT,t.Number 提货单编号,t.T_THDXDSJ 提货单下达时间,d.Z_HTBH 电子购货合同编号,'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),t.T_ZCFHR 最迟发货日,'此前累计提货次数'=(select count(*) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),t.T_THSL 本次提货数量,'此前累计提货数量'=(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),d.Z_ZBSL 定标数量,'剩余可提货数量'=(isnull(d.Z_ZBSL,0)-isnull((select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),0)-isnull(t.T_THSL,0)),T_DJHKJE 本次提货金额,'此前累计提货金额'=(select isnull(sum(T_DJHKJE),0.00) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),isnull(isnull(d.Z_ZBSL,0)*isnull(Z_ZBJG,0),0) 定标金额,'剩余可提货金额'=(isnull((isnull(d.Z_ZBSL,0)-isnull((select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),0)-isnull(t.T_THSL,0))*t.ZBDJ,0.00)), d.Z_SPBH 商品编号,d.Z_SPMC 商品名称,d.Z_GG 规格,d.Z_JJDW 计价单位,t.T_THSL 提货数量,t.ZBDJ 定标价格,t.T_DJHKJE 金额,t.T_FPLX 发票类型,t.T_FPTT 发票抬头,t.T_SH 税号,t.T_KHYH 开户银行,t.T_YHZH 银行账号,t.T_DWDZ 单位地址,t.T_SHRXM 收货人,t.T_SHRLXFS 收货联系方式,(d.Y_YSYDDSHQYsheng+d.Y_YSYDDSHQYshi+t.T_SHXXDZ) 收货地址 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number  where  t.Number='" + ViewState["Number"].ToString().Trim() + "'";
        DataSet ds = DbHelperSQL.Query(strSQL);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            spanDH.InnerText = "单号：T" + ds.Tables[0].Rows[0]["提货单编号"].ToString();
            spanTHDTime.InnerText = ds.Tables[0].Rows[0]["提货单下达时间"].ToString();
            spanSelMC.InnerText = ds.Tables[0].Rows[0]["卖家名称"].ToString()+"：";
            spanDZGHHT.InnerText = "根据" + ds.Tables[0].Rows[0]["电子购货合同编号"].ToString() + "《电子购货合同》，我方现下达提货单，请贵方于";
            spanZCFHR.InnerText = Convert.ToDateTime(ds.Tables[0].Rows[0]["最迟发货日"].ToString()).ToString("yyyy年MM月dd日");
            tdTHDH.InnerText = "T" + ds.Tables[0].Rows[0]["提货单编号"].ToString();
            tdCQLJTHCS.InnerText = ds.Tables[0].Rows[0]["此前累计提货次数"].ToString();
            tdBCTHSL.InnerText = ds.Tables[0].Rows[0]["本次提货数量"].ToString();
            tdCQLJTHSL.InnerText = ds.Tables[0].Rows[0]["此前累计提货数量"].ToString();
            tdDBSL.InnerText = ds.Tables[0].Rows[0]["定标数量"].ToString();
            tdSYKTHSL.InnerText = ds.Tables[0].Rows[0]["剩余可提货数量"].ToString();
            tdBCTHJE.InnerText = ds.Tables[0].Rows[0]["本次提货金额"].ToString();
            tdCQLJTHJE.InnerText = ds.Tables[0].Rows[0]["此前累计提货金额"].ToString();
            tdDBJE.InnerText = ds.Tables[0].Rows[0]["定标金额"].ToString();
            tdSYKTHJE.InnerText = ds.Tables[0].Rows[0]["剩余可提货金额"].ToString();

            //数据列表部分

            spanSPBH.InnerText = ds.Tables[0].Rows[0]["商品编号"].ToString();
            ViewState["商品名称"] = ds.Tables[0].Rows[0]["商品名称"].ToString();
            ViewState["规格"] = ds.Tables[0].Rows[0]["规格"].ToString();
            //spanSPMC.InnerText = ds.Tables[0].Rows[0]["商品名称"].ToString();
            //spanGG.InnerText = ds.Tables[0].Rows[0]["规格"].ToString();
            spanJJDW.InnerText = ds.Tables[0].Rows[0]["计价单位"].ToString();
            spanTHSL.InnerText = ds.Tables[0].Rows[0]["提货数量"].ToString();
            spanDBJG.InnerText = ds.Tables[0].Rows[0]["定标价格"].ToString();
            spanJE.InnerText = ds.Tables[0].Rows[0]["金额"].ToString();


            tdFPLX.InnerText = ds.Tables[0].Rows[0]["发票类型"].ToString();
            tdFPTT.InnerText = ds.Tables[0].Rows[0]["发票抬头"].ToString();
            tdFPSH.InnerText = ds.Tables[0].Rows[0]["税号"].ToString();

            switch (ds.Tables[0].Rows[0]["发票类型"].ToString())
            {
                case "增值税普通发票":
                    SHAndYHZH.Visible = false;//开票税号、银行账号
                    KHYH.Visible = false;//开户银行
                    DWDZ.Visible = false;//单位地址
                    break;
                case "增值税专用发票":
                    SHAndYHZH.Visible = true;//开票税号、银行账号
                    KHYH.Visible = true;//开户银行
                    DWDZ.Visible = true;//单位地址
                    tdYHZH.InnerText = ds.Tables[0].Rows[0]["银行账号"].ToString();
                    tdKHYH.InnerText = ds.Tables[0].Rows[0]["开户银行"].ToString();
                    tdDWDZ.InnerText = ds.Tables[0].Rows[0]["单位地址"].ToString();
                    break;
                default:
                    break;
            }


            tdSHR.InnerText = ds.Tables[0].Rows[0]["收货人"].ToString();
            tdSHLXFS.InnerText = ds.Tables[0].Rows[0]["收货联系方式"].ToString();
            tdSHDZ.InnerText = ds.Tables[0].Rows[0]["收货地址"].ToString();
            spanBZ.InnerText = "1、本提货单作为" + ds.Tables[0].Rows[0]["电子购货合同编号"].ToString() + "《电子购货合同》的附件，与" + ds.Tables[0].Rows[0]["电子购货合同编号"].ToString() + "《电子购货合同》具有同等法律效力。";



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