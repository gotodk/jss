using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_New2013_JJRSYDW_JJRSYFPXXLRCKAndSH : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            if (Request["sh"] != null && Request["sh"].ToString() == "ok")
            {
                content_zw.Style.Clear();
                content_zw.Style.Add("width", "1000px");
                txtSHR.Visible = true;
                spanSHR.Visible = false;
                //txtSHTime.Visible = true;
                tdSHTime.Visible = false;
                spanSHTime.Visible = false;
                btnSH.Visible = true;
                btnGoBack.Visible = true;
                button_yl.Visible = false;
                if (Request["number"] != null && Request["number"].ToString() != "")
                {
                    ViewState["Number"] = Request["number"].ToString();
                }
                else
                {
                    ViewState["Number"] = "";
                }
                Bind();
            }
            else
            {
                content_zw.Style.Clear();
                content_zw.Style.Add("margin", "auto auto");
                content_zw.Style.Add("width", "800px");
                txtSHR.Visible = false;
                spanSHR.Visible = true;
                //txtSHTime.Visible = false;
                tdSHTime.Visible = true;
                spanSHTime.Visible = true;
                btnSH.Visible = false;
                btnGoBack.Visible = false;
                button_yl.Visible = true;
                if (Request["number"] != null && Request["number"].ToString() != "")
                {
                    ViewState["Number"] = Request["number"].ToString();
                }
                else
                {
                    ViewState["Number"] = "";
                }
                Bind();
            }
            
        }
        
    }
    protected void Bind()
    {
        string str = "select a.*,isnull(SYZJE-YZQJE,0.00) 缺票收益金额,b.I_DSFCGZT,'发票收取时间'= convert(char,SQFPRQ,111) from  AAA_DWJJRSYZQMXB a left join AAA_DLZHXXB b on a.JJRBH=b.J_JJRJSBH  where a.Number='" + ViewState["Number"].ToString() + "'";
        DataSet ds = DbHelperSQL.Query(str);
        if (ds != null&&ds.Tables[0].Rows.Count>0)
        {
            if (ds.Tables[0].Rows[0]["SHZT"].ToString() == "已审核")
            {
                btnSH.Enabled = false;
            }
            else
            {
                btnSH.Enabled = true;
            }
            spanDH.InnerText = ds.Tables[0].Rows[0]["Number"].ToString();
            spanZT.InnerText = ds.Tables[0].Rows[0]["SHZT"].ToString();
            spanFPLRTime.InnerText = ds.Tables[0].Rows[0]["CreateTime"].ToString();

            spanJJRBH.InnerText = ds.Tables[0].Rows[0]["JJRBH"].ToString();
            spanJJRMC.InnerText = ds.Tables[0].Rows[0]["JJRMC"].ToString();
            spanSSFGS.InnerText = ds.Tables[0].Rows[0]["SSFGS"].ToString();
            spanDSF.InnerText = ds.Tables[0].Rows[0]["I_DSFCGZT"].ToString();
            spanQPSY.InnerText = ds.Tables[0].Rows[0]["缺票收益金额"].ToString();
            spanBCHGFPJE.InnerText = ds.Tables[0].Rows[0]["BCYSHGFPJE"].ToString();
            spanBCKZQSYJE.InnerText = ds.Tables[0].Rows[0]["BCKZQJE"].ToString();
            spanFPSQTime.InnerText = ds.Tables[0].Rows[0]["发票收取时间"].ToString();
            spanFPSFYRZ.InnerText = ds.Tables[0].Rows[0]["FPSFYRZ"].ToString();
            spanBZ.InnerText = ds.Tables[0].Rows[0]["BZ"].ToString();
            spanSHR.InnerText = ds.Tables[0].Rows[0]["SHR"].ToString();
            spanSHTime.InnerText = ds.Tables[0].Rows[0]["SHSJ"].ToString();  
        }
        else
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('未查询到编号为" + ViewState["Number"].ToString() + "的数据，不能进行修改！');</script>");
            return;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {        
        try
        {
            string strSelect = "select *,'缺票收益金额'=isnull((select isnull(sum(JE),0.00) from AAA_ZKLSMXB where JSBH=b.J_JJRJSBH and XM='经纪人收益' and (XZ='来自买方收益' or XZ='来自卖方收益') and SJLX='预' )-(select isnull(sum(JE),0.00) from AAA_ZKLSMXB where JSBH=b.J_JJRJSBH and XM='经纪人收益' and XZ='发票核准后收益' and SJLX='实'),0.00) from AAA_DWJJRSYZQMXB a left join AAA_DLZHXXB b  on a.JJRBH=b.J_JJRJSBH where a.Number='" + ViewState["Number"].ToString() + "' ";
            DataSet ds = DbHelperSQL.Query(strSelect);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["I_DSFCGZT"].ToString() == "开通")
                {
                    if (ds.Tables[0].Rows[0]["SHZT"].ToString() == "已审核")
                    {
                        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('单号为：" + ViewState["Number"].ToString() + "的数据已审核通过，不能进行重复操作！');</script>");
                        return;
                    }
                    else
                    {
                        #region//验证缺票收益必须大于等于可支取收益
                        if (Convert.ToDouble(ds.Tables[0].Rows[0]["缺票收益金额"].ToString()) < Convert.ToDouble(ds.Tables[0].Rows[0]["BCKZQJE"].ToString()))
                        {
                            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('单号为：" + ViewState["Number"].ToString() + "数据的可支取收益大于缺票收益金额，不能进行审核操作！');</script>");
                            return;
                        }
                        #endregion

                        ArrayList list = new ArrayList();                        

                        #region//修改单位经纪人收益支取明细表
                        string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_DWJJRSYZQMXB", ""); //生成主键
                        string str = "Update [AAA_DWJJRSYZQMXB] set SHR='" + txtSHR.Text.Trim() + "',SHSJ=getdate(),SHZT='已审核' where Number='" + ViewState["Number"].ToString() + "'";
                        list.Add(str);
                        #endregion

                        #region//插入资金流水明细
                        string StrZK = " select  Number,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number ='1304000046'";
                        DataSet dsZK =  DbHelperSQL.Query(StrZK);
                        string xm = "";//项目
                        string xz = "";//性质
                        string zy = "";//摘要
                        string sjlx = "";//数据类型
                        string yslx = "";//运算类型                       
                        if (dsZK != null && dsZK.Tables[0].Rows.Count > 0)
                        {
                            DataRow row1=dsZK.Tables[0].Rows[0];
                            xm = row1["XM"].ToString();//项目
                            xz = row1["XZ"].ToString();//性质
                            zy = row1["ZY"].ToString();//摘要
                            sjlx = row1["SJLX"].ToString();//数据类型
                            yslx = row1["YSLX"].ToString();//运算类型
                        }
                        string DLYX=ds.Tables[0].Rows[0]["B_DLYX"].ToString();
                        string JSZHLX=ds.Tables[0].Rows[0]["B_JSZHLX"].ToString();
                        string JJRJSBH=ds.Tables[0].Rows[0]["J_JJRJSBH"].ToString();
                        string ZKNumber = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", ""); //生成主键
                        double BCKZQJE = Convert.ToDouble(ds.Tables[0].Rows[0]["BCKZQJE"].ToString());//本次可支取金额

                        string insertZK= "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + ZKNumber + "','" + DLYX + "','" + JSZHLX + "','" + JJRJSBH + "','AAA_DWJJRSYZQMXB','" + ViewState["Number"].ToString().Trim() + "',getdate(),'" + yslx + "'," + BCKZQJE + ",'" + xm + "','" + xz + "','" + zy + "','','','" + sjlx + "',1,'" + User.Identity.Name.ToString() + "',getdate(),getdate())";
                        list.Add(insertZK);
                        #endregion

                        #region//更改登录表余额
                        string strUpdateDLB="update AAA_DLZHXXB set B_ZHDQKYYE=B_ZHDQKYYE+"+BCKZQJE+" where J_JJRJSBH='"+JJRJSBH+"'";
                        list.Add(strUpdateDLB);
                        #endregion

                        bool i = DbHelperSQL.ExecSqlTran(list);
                        if (i)
                        {
                            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('发票信息录入审核成功！');</script>");
                            btnSH.Enabled = false;
                        }
                        else
                        {
                            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('发票信息录入审核失败！');</script>");
                        }
                    }
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('当前经纪人的第三方存管尚未开通，不能进行审核操作！');</script>");
                    return;
                }
            }
            else
            {
                this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('未查询到单号为：" + ViewState["Number"].ToString() + "的数据，不能进行审核操作！');</script>");
                return;
            }
            
        }
        catch (Exception ex)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('" + ex.Message + "');</script>");
        }
    }
    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("JJRSYZQMXB.aspx");
    }
}