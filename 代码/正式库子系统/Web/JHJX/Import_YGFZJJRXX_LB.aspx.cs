using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_Import_YGFZJJRXX_LB : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            DisGrid();
        }
    }

    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        //输出调试错误
        // Response.Write(ERRinfo);
        //Response.End();
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('服务器超时，请稍后重试！',function(){ window.location.href=window.location.href;});</script>");
        }

        // RadGrid1.DataSource = null;

        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
         
            rptDLRSH.DataSource = NewDS.Tables[0];
            tdEmpty.Visible = false;
        }
        else
        {            
            tdEmpty.Visible = true;
        }
        rptDLRSH.DataBind();
    }

    //设置初始默认值检索
    private Hashtable SetV()
    {       
        Hashtable ht_where = new Hashtable();
        //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage2";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.
        //ht_where["this_dblink"] = "不赋值或ERP";//这个可以不设置,默认是FMOP,即连接业务平台数据库。如需连接ERP，则需要设置为ERP.
        //ht_where["page_index"] = "0";//这个可以不设置,如果需要指定进入页面后显示第几页，才需要设置。必须是数字
        //ht_where["search_fyshow"] = "5";//这个也可以不设置，默认就是5,即页数可点击的按钮是11个。必须是数字，不能是0。
        //ht_where["count_float"] = "特殊";  //这个可以不用设置，特殊情况设置为特殊
        ht_where["page_size"] = "15"; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " YGGH,YGXM,GLJJRJYFMC,GLJJRDLYX,GLJJRZCLB,SFYX,BZ ";
        ht_where["search_tbname"] = " AAA_YGFZJJRXXB ";  //检索的表
        ht_where["search_mainid"] = " JSBH ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " CreateTime ";  //用于排序的字段

        if (txtYGGH.Text.Trim()!="")
        {
            ht_where["search_str_where"] += " and YGGH like '%" + txtYGGH.Text.Trim() + "%'";
        }
        if (txtYGXM.Text.Trim() != "")
        {
            ht_where["search_str_where"] += " and YGXM like '%" + txtYGXM.Text.Trim() + "%'";
        }
        if (txtGLJJRJYFMC.Text.Trim() != "")
        {
            ht_where["search_str_where"] += " and GLJJRJYFMC like '%" + txtGLJJRJYFMC.Text.Trim() + "%'";
        }
        if (txtGLJJRDLYX.Text.Trim() != "")
        {
            ht_where["search_str_where"] += " and GLJJRDLYX like '%" + txtGLJJRDLYX.Text.Trim() + "%'";
        }
        return ht_where;
    }

    public void DisGrid()
    {
        //应用实例

        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        // HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and YHM like '%" + txtYHM.Text.ToString() + "%' " + strSql;

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
}