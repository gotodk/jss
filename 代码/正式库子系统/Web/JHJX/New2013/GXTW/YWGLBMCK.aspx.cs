using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.Common;
using FMOP.DB;

public partial class Web_JHJX_New2013_GXTW_YWGLBMCK : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            DisGrid();            
        }
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {        
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = "15"; //必须设置,每页的数据量。必须是数字。不能是0。
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " AAA_PTGLJGB ";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_paixu"] = " desc ";  //排序方式
        ht_where["search_paixuZD"] = " CreateTIme ";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_CSSPZLSH/JHJX_CKXQ.aspx";
        if(txtGLBMMC.Text.Trim()!="")
        {
            ht_where["search_str_where"] += " and GLBMMC like '%" + txtGLBMMC.Text.Trim() + "%' ";
        }
        if (txtGLBMZH.Text.Trim() != "")
        {
            ht_where["search_str_where"] += " and GLBMZH like '%" + txtGLBMZH.Text.Trim() + "%' ";
        }
        if (drpGLBMFL.SelectedValue != "全部")
        {
            ht_where["search_str_where"] += " and GLBMFLMC= '" + drpGLBMFL.SelectedValue.Trim() + "' ";
        }

        return ht_where;
    }
    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        //输出调试错误
        // Response.Write(ERRinfo);
        //Response.End();
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.Show(this, "查询超时，请重试或者修改查询条件！");
        }

        Repeater1.DataSource = null;

        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            Repeater1.DataSource = NewDS.Tables[0].DefaultView;
            ts.Visible = false;
        }
        else
        {           
            ts.Visible = true;

        }
        Repeater1.DataBind();
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if(e.CommandArgument=="XG")
        {
            Response.Redirect("YWGLBMAdd.aspx?Number="+e.CommandName.Trim());
        }
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();  
    }
}