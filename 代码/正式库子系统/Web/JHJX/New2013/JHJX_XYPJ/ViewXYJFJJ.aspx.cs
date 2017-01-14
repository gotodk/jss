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

public partial class Web_JHJX_New2013_JHJX_XYPJ_ViewXYJFJJ : System.Web.UI.Page
{  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["number"] != null && Request["number"].ToString() != "")
            {
                iptNum.Value = Request["number"].ToString();
            }
            Initial(iptNum.Value);
        }
        //if (Context.Handler.ToString() == "ASP.web_jhjx_new2013_jhjx_xypj_jyfxyjfjj_aspx")
        //{
        //    DataRow[] drs = this.Context.Items["Text"] as DataRow[];
        //    InitialData(drs[0]);
        //}        
    }

    private void Initial(string number)
    {
        string sql = "select * from  ( select 'S'+[AAA_ZBDBXXB].Number as 信息表编号,Z_HTBH as 合同编号,Z_DBSJ as 定标时间, T_YSTBDDLYX as 交易方账号, '卖方' as 交易账户类型,B_JSZHLX as 角色类型, T_YSTBDGLJJRYX as 关联经纪人账号,isnull([I_YWGLBMFL],'') as 部门分类, isnull(I_PTGLJG,'' ) as 所属分公司,[I_LXRXM] as 联系人,[I_LXRSJH] as 联系方式, [I_SSQYS]+[I_SSQYSHI]+[I_SSQYQ] as 所属区域,[I_JYFMC] as 交易方名称,[I_ZCLB]as 注册类别, isnull( B_ZHDQXYFZ,0) as 信用分值 from  [AAA_ZBDBXXB] left join AAA_DLZHXXB  on [B_DLYX]=T_YSTBDDLYX where Z_DBSJ is not null union select 'B'+[AAA_ZBDBXXB].Number as 信息表编号,Z_HTBH as 合同编号,Z_DBSJ as 定标时间, Y_YSYDDDLYX as 交易方账号, '买方' as 交易账户类型,B_JSZHLX as 角色类型, Y_YSYDDGLJJRYX as 关联经纪人账号,isnull([I_YWGLBMFL],'') as 部门分类,isnull(I_PTGLJG,'' ) as 所属分公司,[I_LXRXM] as 联系人,[I_LXRSJH] as 联系方式, [I_SSQYS]+[I_SSQYSHI]+[I_SSQYQ] as 所属区域,[I_JYFMC] as 交易方名称,[I_ZCLB]as 注册类别, isnull( B_ZHDQXYFZ,0) as 信用分值 from  [AAA_ZBDBXXB] left join AAA_DLZHXXB  on [B_DLYX]=Y_YSYDDDLYX where Z_DBSJ is not null ) tab1  where 信息表编号='" + number + "'";
        DataSet ds = DbHelperSQL.Query(sql);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            InitialData(dr);
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected void  InitialData(DataRow dr)
    {
        lbjyfzh.Text = dr["交易方账号"] == null ? "" : dr["交易方账号"].ToString().Trim();
        lbjyfmc.Text = dr["交易方名称"] == null ? "" : dr["交易方名称"].ToString().Trim();
        lbjyzhlx.Text = dr["交易账户类型"] == null ? "" : dr["交易账户类型"].ToString().Trim();
        lbzclb.Text = dr["注册类别"] == null ? "" : dr["注册类别"].ToString().Trim();
        lblxrxm.Text = dr["联系人"] == null ? "" : dr["联系人"].ToString().Trim();
        lblxrsjh.Text = dr["联系方式"] == null ? "" : dr["联系方式"].ToString().Trim();
        lbssqy.Text = dr["所属区域"] == null ? "" : dr["所属区域"].ToString().Trim();
        lbgljjrzh.Text = dr["关联经纪人账号"] == null ? "" : dr["关联经纪人账号"].ToString().Trim();
        lbssfgs.Text = dr["所属分公司"] == null ? "" : dr["所属分公司"].ToString().Trim();
        lbdqpf.Text = dr["信用分值"] == null ? "" : dr["信用分值"].ToString().Trim();
        lbhtbh.Text = dr["合同编号"] == null ? "" : dr["合同编号"].ToString().Trim();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //验证条件是否满足
        if (lbjyfzh.Text.Trim() == "")
        {
            MessageBox.Show(this, "请查询出要进行评分修改的交易账号！");
            return;
        }
        else if (lbjyzhlx.Text.Trim() == "")
        {
            MessageBox.Show(this, "交易方尚未开通结算账户，不能进行处理！");
            return;
        }
        else if (ddlwgsx.SelectedValue.Trim() == "")
        {
            MessageBox.Show(this, "请选择评分变化的事项依据！");
            return;
        }
        else if (txtqkjs.Text.Trim() == "")
        {
            MessageBox.Show(this, "请填写情况简述！");
            return;
        }
        else if (txtqkjs.Text.Trim().Length > 250)
        {
            MessageBox.Show(this, "字数请勿超出250！");
            return;
        }
        else
        {
            //评分项
            if (ddlwgsx.SelectedValue == "履约中被判定为违约责任方")
            {
                jhjx_JYFXYMX xypj = new jhjx_JYFXYMX();
                string[] strs = new string[2];
                string zhlb = lbjyzhlx.Text.Trim();
                if (zhlb == "买方")
                {
                    strs = xypj.ZSZHMR_YZYJXFLSS(lbjyfzh.Text.Trim(), lbhtbh.Text.Trim(), "买家", User.Identity.Name, txtqkjs.Text.Trim());
                }
                else if (zhlb == "卖方")
                {
                    strs = xypj.ZSZHMC_YZYJXFLSS(lbjyfzh.Text.Trim(), lbhtbh.Text.Trim(), "卖家", User.Identity.Name, txtqkjs.Text.Trim());
                }
                else if (zhlb == "经纪人")
                {
                    MessageBox.Show(this, "经纪人账户类型不能就此项进行评分加减！");
                    return;
                }
                List<string> ls = new List<string>();
                ls.Add(strs[0]);
                ls.Add(strs[1]);

                if (ls.Count > 0)
                {
                    if (DbHelperSQL.ExecuteSqlTran(ls) > 0)
                    {                       
                        MessageBox.ShowAndRedirect(this, "更新成功！", "JYFXYJFJJ.aspx");
                    }
                    else
                    {
                        MessageBox.Show(this, "更新失败！");
                        return;
                    }
                }
                else
                {
                    MessageBox.ShowAlertAndBack(this, "没有需要执行的语句！");
                }
            }
        }
    }
    protected void btnCancle_Click(object sender, EventArgs e)
    {
        //string script = " <script language='javascript'>window.open('JYFXYJFJJ.aspx','_self');</script>";

        //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "CloseWindow", script);

        //string script = "<script language='javascript'>  window.history.go(-2);  </script>";
        //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "CloseWindow", script);

        Response.Redirect("JYFXYJFJJ.aspx");

    }
}