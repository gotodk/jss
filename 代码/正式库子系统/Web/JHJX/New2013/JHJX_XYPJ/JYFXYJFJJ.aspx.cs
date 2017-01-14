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

public partial class Web_JHJX_New2013_JHJX_XYPJ_JYFXYJFJJ : System.Web.UI.Page
{
    public static DataTable dt;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            UCFWJGDetail1.initdefault();           
            DisGrid();
        }
    }
    
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {

        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.ShowAlertAndBack(this, "查询超时，请重试或者修改查询条件！");
        }
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            tdEmpty.Visible = false;
            dt = NewDS.Tables[0];
            rpt.DataSource = dt.DefaultView;
        }
        else { tdEmpty.Visible = true; }
        rpt.DataBind();
    }
    protected string GetSql()
    {
        //return " ( select 'S'+[AAA_ZBDBXXB].Number as 信息表编号,Z_HTBH as 合同编号,Z_DBSJ as 定标时间, T_YSTBDDLYX as 交易方账号, '卖家' as 交易账户类型,B_JSZHLX as 角色类型, T_YSTBDGLJJRYX as 关联经纪人账号,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = T_YSTBDGLJJRYX) as 所属分公司,[I_LXRXM] as 联系人,[I_LXRSJH] as 联系方式, [I_SSQYS]+[I_SSQYSHI]+[I_SSQYQ] as 所属区域,[I_JYFMC] as 交易方名称,[I_ZCLB]as 注册类别, isnull( B_ZHDQXYFZ,0) as 信用分值 from  [AAA_ZBDBXXB] left join AAA_DLZHXXB  on [B_DLYX]=T_YSTBDDLYX where Z_DBSJ is not null union select 'B'+[AAA_ZBDBXXB].Number as 信息表编号,Z_HTBH as 合同编号,Z_DBSJ as 定标时间, Y_YSYDDDLYX as 交易方账号, '买家' as 交易账户类型,B_JSZHLX as 角色类型, Y_YSYDDGLJJRYX as 关联经纪人账号,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX = Y_YSYDDGLJJRYX) as 所属分公司,[I_LXRXM] as 联系人,[I_LXRSJH] as 联系方式, [I_SSQYS]+[I_SSQYSHI]+[I_SSQYQ] as 所属区域,[I_JYFMC] as 交易方名称,[I_ZCLB]as 注册类别, isnull( B_ZHDQXYFZ,0) as 信用分值 from  [AAA_ZBDBXXB] left join AAA_DLZHXXB  on [B_DLYX]=Y_YSYDDDLYX where Z_DBSJ is not null ) tab1 ";

        return " ( select 'S'+[AAA_ZBDBXXB].Number as 信息表编号,Z_HTBH as 合同编号,Z_DBSJ as 定标时间, T_YSTBDDLYX as 交易方账号, '卖方' as 交易账户类型,B_JSZHLX as 角色类型, T_YSTBDGLJJRYX as 关联经纪人账号,isnull([I_YWGLBMFL],'') as 部门分类, isnull(I_PTGLJG,'' ) as 所属分公司,[I_LXRXM] as 联系人,[I_LXRSJH] as 联系方式, [I_SSQYS]+[I_SSQYSHI]+[I_SSQYQ] as 所属区域,[I_JYFMC] as 交易方名称,[I_ZCLB]as 注册类别, isnull( B_ZHDQXYFZ,0) as 信用分值 from  [AAA_ZBDBXXB] left join AAA_DLZHXXB  on [B_DLYX]=T_YSTBDDLYX where Z_DBSJ is not null union select 'B'+[AAA_ZBDBXXB].Number as 信息表编号,Z_HTBH as 合同编号,Z_DBSJ as 定标时间, Y_YSYDDDLYX as 交易方账号, '买方' as 交易账户类型,B_JSZHLX as 角色类型, Y_YSYDDGLJJRYX as 关联经纪人账号,isnull([I_YWGLBMFL],'') as 部门分类,isnull(I_PTGLJG,'' ) as 所属分公司,[I_LXRXM] as 联系人,[I_LXRSJH] as 联系方式, [I_SSQYS]+[I_SSQYSHI]+[I_SSQYQ] as 所属区域,[I_JYFMC] as 交易方名称,[I_ZCLB]as 注册类别, isnull( B_ZHDQXYFZ,0) as 信用分值 from  [AAA_ZBDBXXB] left join AAA_DLZHXXB  on [B_DLYX]=Y_YSYDDDLYX where Z_DBSJ is not null ) tab1 ";
    }


    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = GetSql();  //检索的表
        ht_where["search_mainid"] = " 信息表编号 ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " 定标时间 ";  //用于排序的字段
        ht_where["returnlastpage_open"] = " ViewXYJFJJ.aspx ";

        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();

        HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%'  and 所属分公司 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 交易方账号 like '%" + txtjyfzh.Text.Trim() + "%' and 交易方名称 like '%" + txtjyfmc.Text.Trim() + "%' and 合同编号 like '%" + txthtbh.Text.Trim() + "%'  ";

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    } 

    protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lbjyfmc = (Label)e.Item.FindControl("lbjyfmc");
        if (lbjyfmc.Text.Length > 15)
        {
            lbjyfmc.Text = lbjyfmc.Text.Substring(0, 15) + "...";
        }
    }
    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "lck")
        {
            //DataRow[] df = dt.Select("信息表编号='" + e.CommandArgument.ToString().Trim() + "'");
            //this.Context.Items.Add("Text", df);
            //Server.Transfer("ViewXYJFJJ.aspx");

            Response.Redirect("ViewXYJFJJ.aspx?number=" + e.CommandArgument.ToString().Trim() + "");

        }
    }
}