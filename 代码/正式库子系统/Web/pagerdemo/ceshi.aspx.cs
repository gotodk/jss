using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;

public partial class Web_pagerdemo_ceshi : System.Web.UI.Page
{
    //调用演示如下：
    protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //开始使用树形菜单重设复合表头,若需要动态生成表头或多个表头切换，在调用前处理TreeView
        YHBRowHeader YRH = new YHBRowHeader();
        YRH.BeginReSetHand(e.Row, TreeView123456, sender);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        //加载一些测试数据
        DataSet ds = DbHelperSQL.Query("select top 5 'a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1','a'='1' from FWPT_FWSDHD_CPLB ");

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            GridView2.DataSource = ds.Tables[0].DefaultView;
            GridView2.DataBind();
        }
    }
}