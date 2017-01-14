using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_New2013_JHJX_CSSPZLSH_JHJX_CKXQ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["sh"] != null && Request.QueryString["sh"].ToString() != "")
            {
                btnSave.Visible = true;
             
                btnBoHui.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
                btnBoHui.Visible = false;
            }
            //ViewState["DLYX"] = "";
            //ViewState["SPBH"] = "";
            ViewState["number"] = "";
            ViewState["GoBackUrl"] = "";
            //if (Request.QueryString["dlyx"] != null && Request.QueryString["dlyx"].ToString() != "")
            //{
            //    ViewState["DLYX"] = Request.QueryString["dlyx"].ToString();
            //}
            //if (Request.QueryString["spbh"] != null && Request.QueryString["spbh"].ToString() != "")
            //{
            //    ViewState["SPBH"] = Request.QueryString["spbh"].ToString();
            //}

            if (Request.QueryString["number"] != null && Request.QueryString["number"].ToString() != "")
            {
                ViewState["number"] = Request.QueryString["number"].ToString();
            }

            if (Request.QueryString["GoBackUrl"] != null && Request.QueryString["GoBackUrl"].ToString() != "")
            {
                ViewState["GoBackUrl"] = Request.QueryString["GoBackUrl"].ToString();
            }

            Bind();
        }
    }

    protected void Bind()
    {
        string str = "select a.DLYX, a.SPBH 商品编号,c.SPMC 商品名称,c.GG 执行标准,c.JJDW 计价单位,c.JJPL 平台设定最大经济批量,c.SPMS 商品描述,a.DLYX 交易方账号,b.I_JYFMC 交易方名称,b.I_LXRXM 联系人姓名,b.I_LXRSJH 联系人手机号,b.I_JYFLXDH 交易方联系电话,b.I_YBNSRZGZSMJ 一般人纳税人资格证明,a.ZLBZYZM 质量标准与证明,a.CPJCBG 产品检测报告,a.PGZFZRFLCNS 品管总负责人法律承诺书,a.FDDBRCNS 法定代表人承诺书,a.SHFWGDYCN 售后服务规定与承诺,a.CPSJSQS 产品送检授权书,c.SCZZYQ 特殊资质,a.ZZ1,a.ZZ2,a.ZZ3,a.ZZ4,a.ZZ5,a.ZZ6,a.ZZ7,a.ZZ8,a.ZZ9,a.ZZ10,a.SHYJ 审核意见,b.J_SELJSBH 卖家角色编号 from AAA_CSSPB a left join AAA_DLZHXXB b on a.DLYX=b.B_DLYX left join AAA_PTSPXXB c on a.SPBH=c.SPBH where a.Number='" + ViewState["number"].ToString() + "'";
        DataSet ds = DbHelperSQL.Query(str);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            #region//商品基本信息
            ViewState["DLYX"] = ds.Tables[0].Rows[0]["DLYX"].ToString();
            spanSPBH.InnerText = ds.Tables[0].Rows[0]["商品编号"].ToString();
            spanSPMC.InnerText = ds.Tables[0].Rows[0]["商品名称"].ToString();
            ViewState["SPBH"] = ds.Tables[0].Rows[0]["商品编号"].ToString();
            spanZXBZ.InnerText = ds.Tables[0].Rows[0]["执行标准"].ToString();
            spanJJDW.InnerText = ds.Tables[0].Rows[0]["计价单位"].ToString();
            spanPTSDZDJJPL.InnerText = ds.Tables[0].Rows[0]["平台设定最大经济批量"].ToString();
            spanSPMS.InnerText = ds.Tables[0].Rows[0]["商品描述"].ToString();
            #endregion

            #region//交易方基本信息
            spanJYFZH.InnerText = ds.Tables[0].Rows[0]["交易方账号"].ToString();
            spanJYFMC.InnerText = ds.Tables[0].Rows[0]["交易方名称"].ToString();
            ViewState["交易方名称"] = ds.Tables[0].Rows[0]["交易方名称"].ToString();
            ViewState["卖家角色编号"] = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();
            spanLXRXM.InnerText = ds.Tables[0].Rows[0]["联系人姓名"].ToString();
            spanLXRSJH.InnerText = ds.Tables[0].Rows[0]["联系人手机号"].ToString();
            spanJYFLXDH.InnerText = ds.Tables[0].Rows[0]["交易方联系电话"].ToString();
            #endregion

            #region//投标资质
            //AYBNSR.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["一般人纳税人资格证明"].ToString().Replace(@"\", "/");
            AZLBZ.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["质量标准与证明"].ToString().Replace(@"\", "/");
            ACPJCBG.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["产品检测报告"].ToString().Replace(@"\", "/");
            APGZFZRFLCNS.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["品管总负责人法律承诺书"].ToString().Replace(@"\", "/");
            AFDDBRCRS.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["法定代表人承诺书"].ToString().Replace(@"\", "/");
            ASHFWGD.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["售后服务规定与承诺"].ToString().Replace(@"\", "/");
            ACPSJSQS.HRef = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["产品送检授权书"].ToString().Replace(@"\", "/");
            #endregion

            #region//特殊资质
            if (ds.Tables[0].Rows[0]["ZZ1"].ToString() != "")
            {
                divTSZZ.Visible = true;
                if (ds.Tables[0].Rows[0]["特殊资质"].ToString() != "" && ds.Tables[0].Rows[0]["特殊资质"].ToString().Contains("|") == true)
                {
                    string[] tszz = ds.Tables[0].Rows[0]["特殊资质"].ToString().Split('|');
                    divTSZZ.InnerHtml = "<table width='700px' class='Message' cellspacing='0' cellpadding='0'>";
                    for (int i = 1; i <= tszz.Length; i++)
                    {
                        divTSZZ.InnerHtml += "<tr><td style='width: 150px; text-align: right; height: 25px; padding-left: 15px'>" + tszz[i - 1].ToString() + "：</td><td style='text-align: left; width: 550px;'>&nbsp;&nbsp;<a class='link' href='http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["ZZ" + i].ToString().Replace(@"\", "/") + "' target='blank'>查看</a></td></tr>";
                    }
                    divTSZZ.InnerHtml += "</table>";
                }
                else if (ds.Tables[0].Rows[0]["特殊资质"].ToString() != "" && ds.Tables[0].Rows[0]["特殊资质"].ToString().Contains("|") == false)
                {
                    divTSZZ.InnerHtml = "<table width='700px' class='Message' cellspacing='0' cellpadding='0'><tr><td style='width: 150px; text-align: right; height: 25px; padding-left: 15px'>" + ds.Tables[0].Rows[0]["特殊资质"].ToString() + "：</td><td style='text-align: left; width: 550px'>&nbsp;&nbsp;<a class='link' href='http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/JHJXPT/SaveDir/" + ds.Tables[0].Rows[0]["ZZ1"].ToString().Replace(@"\", "/") + "' target='blank'>查看</a></td></tr></table>";
                }
                else
                {
                    divTSZZ.Visible = false;
                }
            }
            else
            {
                divTSZZ.Visible = false;
            }


            #endregion

            #region//服务中心审核
            txtSHYJ.Text = ds.Tables[0].Rows[0]["审核意见"].ToString();
            #endregion
        }


    }

    protected void btnCancle_Click(object sender, EventArgs e)
    {
        Response.Redirect(ViewState["GoBackUrl"].ToString());
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtSHYJ.Text.Trim().Contains("'"))
        {
            txtSHYJ.Text = txtSHYJ.Text.Trim().Replace("'", "‘");
        }
        string strUpdate = "update AAA_CSSPB set SHZT='审核通过',SHR='" + User.Identity.Name.ToString() + "',SHSJ=getdate(),SHYJ='" + txtSHYJ.Text.Trim() + "' where number='" + ViewState["number"].ToString() + "'";
        int i = DbHelperSQL.ExecuteSql(strUpdate);
        if (i > 0)
        {
            Hashtable ht = new Hashtable();
            ht["type"] = "集合集合经销平台";
            ht["提醒对象登陆邮箱"] = ViewState["DLYX"].ToString();
            ht["提醒对象用户名"] = ViewState["交易方名称"].ToString();
            ht["提醒对象结算账户类型"] = "买家卖家交易账户";
            ht["提醒对象角色编号"] = ViewState["卖家角色编号"].ToString();
            ht["提醒对象角色类型"] = "卖家";
            ht["提醒内容文本"] = "您拟出售" + ViewState["SPBH"].ToString() +spanSPMC.InnerText+ "商品的申请，平台审核已通过，现在您可以对该商品下达投标单。";//待确定
            ht["创建人"] = User.Identity.Name.ToString();
            List<Hashtable> list = new List<Hashtable>();
            list.Add(ht);
            JHJX_SendRemindInfor.Sendmes(list);
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script> var a = window.location.href;    var b = a.split('JHJX_CSSPZLSH');        var url = b[0].toString() + 'JHJX_CSSPZLSH/" + ViewState["GoBackUrl"].ToString() + "'; window.top.Dialog.alert('审核操作成功！',function(){location.href=url});</script>");
            return;
        }
        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核操作失败！');</script>");
        return;

    }
    protected void btnBoHui_Click(object sender, EventArgs e)
    {
        if (txtSHYJ.Text.Trim()=="")
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('审核意见不能为空！');</script>");
            return;
        }
        if (txtSHYJ.Text.Trim().Contains("'"))
        {
            txtSHYJ.Text = txtSHYJ.Text.Trim().Replace("'", "‘");
        }
        string strUpdate = "update AAA_CSSPB set SHZT='驳回',SHR='" + User.Identity.Name.ToString() + "',SHSJ=getdate(),SHYJ='" + txtSHYJ.Text.Trim() + "' where number='" + ViewState["number"].ToString() + "'";
        int i = DbHelperSQL.ExecuteSql(strUpdate);
        if (i > 0)
        {
            Hashtable ht = new Hashtable();
            ht["type"] = "集合集合经销平台";
            ht["提醒对象登陆邮箱"] = ViewState["DLYX"].ToString();
            ht["提醒对象用户名"] = ViewState["交易方名称"].ToString();
            ht["提醒对象结算账户类型"] = "买家卖家交易账户";
            ht["提醒对象角色编号"] = ViewState["卖家角色编号"].ToString();
            ht["提醒对象角色类型"] = "卖家";
            ht["提醒内容文本"] = "您拟出售" + ViewState["SPBH"].ToString() + spanSPMC.InnerText + "商品的申请，平台审核未通过，可进入“查看申请出售商品”界面查看详情。";//待确定
            ht["创建人"] = User.Identity.Name.ToString();
            List<Hashtable> list = new List<Hashtable>();
            list.Add(ht);
            JHJX_SendRemindInfor.Sendmes(list);
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script> var a = window.location.href;    var b = a.split('JHJX_CSSPZLSH');        var url = b[0].toString() + 'JHJX_CSSPZLSH/" + ViewState["GoBackUrl"].ToString() + "'; window.top.Dialog.alert('驳回操作成功！',function(){location.href=url});</script>");
            return;
        }
        this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('驳回操作失败！');</script>");
        return;
    }
}