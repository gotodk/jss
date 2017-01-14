using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

public partial class Web_JHJX_New2013_JHJX_XYPJ_JYFXYPJ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            SetItems();

        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected void Initial()
    {
        lbjyfzh.Text = "";
        lbjyfmc.Text = "";

        lbgljjrzh.Text = "";
        lbjyzhlx.Text = "";
        lbzclb.Text = "";
        lblxrsjh.Text = "";
        lblxrxm.Text = "";
        lbssfgs.Text = "";
        lbssqy.Text = "";
        ddlwgsx.SelectedIndex = 0;
        ddlzhlb.Items.Clear();
        txthtbh.Text = "";
        txtqkjs.Text = "";     
        ddlzhlb.Items.Clear();
        ddlwgsx.Items.Clear();
        tab2.Visible = false;
        tab3.Visible = false;
        tabf.Visible = false;
        tabn.Visible = false;
    }

    protected void SetItems()
    {
        //DataSet ds = DbHelperSQL.Query("select SXNR from AAA_WGSXB");


        //ddlwgsx.DataSource = ds;
        //ddlwgsx.DataTextField = "SXNR";
        //ddlwgsx.DataValueField = "SXNR";
        //ddlwgsx.DataBind();
        ddlwgsx.Items.Insert(0, new ListItem("请选择", ""));
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
          //验证条件是否满足
        if (lbjyfzh.Text.Trim() == "")
        {
            MessageBox.Show(this,"请查询出要进行评分修改的交易账号！");
        }
        else if (lbjyzhlx.Text.Trim() == "" )
        {
            MessageBox.Show(this, "交易方尚未开通结算账户，不能进行处理！"); 
        }
        else if (ddlwgsx.SelectedValue.Trim() == "")
        {
            MessageBox.Show(this, "请选择评分变化的事项依据！");  
        }
     
       
        else if (txtqkjs.Text.Trim() == "")
        {
            MessageBox.Show(this, "请填写情况简述！");

        }
        else if (ddlzhlb.SelectedValue.Trim() == "请选择")
        {
            MessageBox.Show(this, "请选择账户类别！");
        }
        else
        {
            //评分项
            if (ddlwgsx.SelectedValue == "合同因争议进行法律诉讼，被判定为责任方")
            {
                if (txthtbh.Text.Trim() == "")
                {
                    MessageBox.Show(this, "请输入争议合同编号！");

                }
                else
                {
                    jhjx_JYFXYMX xypj = new jhjx_JYFXYMX();

                    string[] strs = new string[2];


                    if (ddlzhlb.SelectedValue == "买家")
                    {
                        strs = xypj.ZSZHMR_YZYJXFLSS(lbjyfzh.Text.Trim(), txthtbh.Text.Trim(), "买家", User.Identity.Name, txtqkjs.Text.Trim());
                    }
                    else  if (ddlzhlb.SelectedValue == "卖家")
                    {
                        strs = xypj.ZSZHMC_YZYJXFLSS(lbjyfzh.Text.Trim(), txthtbh.Text.Trim(), "卖家", User.Identity.Name, txtqkjs.Text.Trim());
                    }
                    else if (ddlzhlb.SelectedValue == "经纪人")
                    {
                        MessageBox.Show(this, "经纪人账户类型不能就此项进行评分加减！");
                        return;
                    }

                    List<string> ls = new List<string>();
                    ls.Add(strs[0]);
                    ls.Add(strs[1]);

                    if (DbHelperSQL.ExecuteSqlTran(ls) > 0)
                    {
                        MessageBox.ShowAndRedirect(this, "更新成功！", "JYFXYPJ.aspx");
                    }
                    else
                    {
                        MessageBox.Show(this, "更新失败！");
                    }

                }
 
            }
 

        }

    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        //先初始化
        Initial();
        if (txtjyfzh.Text.Trim() == "" && txtjyfmc.Text.Trim() == "")
        {
            MessageBox.Show(this, "请输入您要查找的交易方账号或者交易方名称！");

            return;

        }
        DataSet ds = DbHelperSQL.Query(GetSql(txtjyfzh.Text.Trim(), txtjyfmc.Text.Trim()));

        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {

            MessageBox.Show(this, "查询不到您输入的账户，请重新进行输入！");
        }
        else if (ds != null && ds.Tables[0].Rows.Count > 1)
        {

            MessageBox.Show(this, "查询到多个账户，请确认交易方账号和名称是否属于同一账户！");
        }
        else if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            lbjyfzh.Text = ds.Tables[0].Rows[0]["交易方账号"] == null ? "" : ds.Tables[0].Rows[0]["交易方账号"].ToString().Trim();
            lbjyfmc.Text = ds.Tables[0].Rows[0]["交易方名称"] == null ? "" : ds.Tables[0].Rows[0]["交易方名称"].ToString().Trim();
            lbjyzhlx.Text = ds.Tables[0].Rows[0]["交易账户类型"] == null ? "" : ds.Tables[0].Rows[0]["交易账户类型"].ToString().Trim();
            lbzclb.Text = ds.Tables[0].Rows[0]["注册类别"] == null ? "" : ds.Tables[0].Rows[0]["注册类别"].ToString().Trim();
            lblxrxm.Text = ds.Tables[0].Rows[0]["联系人姓名"] == null ? "" : ds.Tables[0].Rows[0]["联系人姓名"].ToString().Trim();
            lblxrsjh.Text = ds.Tables[0].Rows[0]["联系人手机号"] == null ? "" : ds.Tables[0].Rows[0]["联系人手机号"].ToString().Trim();
            lbssqy.Text = ds.Tables[0].Rows[0]["所属区域"] == null ? "" : ds.Tables[0].Rows[0]["所属区域"].ToString().Trim();
            lbgljjrzh.Text = ds.Tables[0].Rows[0]["关联经纪人账号"] == null ? "" : ds.Tables[0].Rows[0]["关联经纪人账号"].ToString().Trim();
            lbssfgs.Text = ds.Tables[0].Rows[0]["所属分公司"] == null ? "" : ds.Tables[0].Rows[0]["所属分公司"].ToString().Trim();
            lbdqpf.Text = ds.Tables[0].Rows[0]["信用评分"] == null ? "" : ds.Tables[0].Rows[0]["信用评分"].ToString().Trim();
            tabf.Visible = true;
            tabn.Visible = true;
            tab2.Visible = true;
            tab3.Visible = true;

        }
        if (lbjyzhlx.Text.Trim() == "买家卖家交易账户")
        {
            ddlzhlb.Items.Add(new ListItem("请选择", ""));
            ddlzhlb.Items.Add(new ListItem("卖家", "卖家"));
            ddlzhlb.Items.Add(new ListItem("买家", "买家"));

            ddlwgsx.Items.Clear();
            ddlwgsx.Items.Add(new ListItem("请选择",""));
            ddlwgsx.Items.Add(new ListItem("合同因争议进行法律诉讼，被判定为责任方", "合同因争议进行法律诉讼，被判定为责任方"));
           
        }
        if (lbjyzhlx.Text.Trim() == "经纪人交易账户")
        {
           
            ddlzhlb.Items.Add(new ListItem("经纪人", "经纪人"));
            ddlwgsx.Items.Clear();
            ddlwgsx.Items.Add(new ListItem("请选择", ""));
           
        }
    }

    protected string GetSql(string jyfzh, string jyfmc)
    {
        return "select * from (select B_DLYX as 交易方账号,I_JYFMC as 交易方名称,B_JSZHLX as 交易账户类型,I_ZCLB as 注册类别,B_ZHDQXYFZ as 信用评分, I_LXRXM as 联系人姓名,I_LXRSJH as 联系人手机号,isnull(I_SSQYS,'')+isnull(I_SSQYSHI,'')+isnull(I_SSQYQ,'') as 所属区域,(select GLJJRDLZH from AAA_MJMJJYZHYJJRZHGLB where DLYX=B_DLYX and SFDQMRJJR='是'and JJRSHZT='审核通过' and SFYX='是') as 关联经纪人账号 from AAA_DLZHXXB )a left join (select I_PTGLJG as 所属分公司, B_DLYX as 经纪人邮箱  from  AAA_DLZHXXB)b on a.关联经纪人账号=b.经纪人邮箱 where a.交易方账号='" + jyfzh + "' or a.交易方名称='" + jyfmc + "'";
    }
    protected void btnCancle_Click(object sender, EventArgs e)
    {
        Response.Redirect("JYFXYPJ.aspx");
    }

}