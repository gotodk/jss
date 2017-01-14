using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Configuration;


public partial class Web_JHJX_New2013_JYGLBJK_DECJJK : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {           
            DisGrid();
        }

    }

    protected string GetSql()
    {
        string strsql = "";

        if (ddllb.SelectedValue.Trim()=="单次")
        {
            strsql = "(select I_JYFMC as 账户名称,AAA_ZKLSMXB.JSZHLX as 账户类型,I_ZCLB as 注册类别,I_LXRXM as 联系人,I_LXRSJH as 联系方式,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX=AAA_MJMJJYZHYJJRZHGLB.GLJJRDLZH) as 所属分公司 ,AAA_ZKLSMXB.CreateTime as 时间,1 as 出金次数, JE as 金额 from  AAA_ZKLSMXB join AAA_DLZHXXB on AAA_ZKLSMXB.DLYX=B_DLYX left join AAA_MJMJJYZHYJJRZHGLB on B_DLYX=AAA_MJMJJYZHYJJRZHGLB.DLYX where  XM ='转出资金' and SFDQMRJJR='是' and JE>" + (txtje.Text.Trim() == "" ? 999999999999 : Convert.ToDouble(txtje.Text.Trim()) * 10000) + " ) as tab";
        }
        else if (ddllb.SelectedValue.Trim() == "一天")
        {
            strsql = "(select I_JYFMC as 账户名称,AAA_ZKLSMXB.JSZHLX as 账户类型,I_ZCLB as 注册类别,I_LXRXM as 联系人,I_LXRSJH as 联系方式,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX=AAA_MJMJJYZHYJJRZHGLB.GLJJRDLZH) as 所属分公司 ,CONVERT(varchar(100), AAA_ZKLSMXB.CreateTime, 111)   as 时间, count(*)as 出金次数, sum(JE) as 金额 from  AAA_ZKLSMXB join AAA_DLZHXXB on AAA_ZKLSMXB.DLYX=B_DLYX left join AAA_MJMJJYZHYJJRZHGLB on B_DLYX=AAA_MJMJJYZHYJJRZHGLB.DLYX where  XM ='转出资金' and SFDQMRJJR='是' group by I_JYFMC ,AAA_ZKLSMXB.JSZHLX ,I_ZCLB ,I_LXRXM,I_LXRSJH,CONVERT(varchar(100), AAA_ZKLSMXB.CreateTime, 111), AAA_MJMJJYZHYJJRZHGLB.GLJJRDLZH  having  sum(JE)>" + (txtje.Text.Trim() == "" ? 999999999999 : Convert.ToDouble(txtje.Text.Trim()) * 10000) + ") as tab";  
        }

        return strsql;
    }
   
    protected void btnDC_Click(object sender, EventArgs e)
    {
        string sql = "";
        if (ddllb.SelectedValue.Trim() == "单次")
        {
            sql = "select I_JYFMC as 账户名称,replace(AAA_ZKLSMXB.JSZHLX,'交易账户','') as 账户类型,I_ZCLB as 注册类别,I_LXRXM as 联系人,I_LXRSJH as 联系方式,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX=AAA_MJMJJYZHYJJRZHGLB.GLJJRDLZH) as 所属分公司 ,AAA_ZKLSMXB.CreateTime as 时间,1 as 出金次数, JE as 金额 from  AAA_ZKLSMXB join AAA_DLZHXXB on AAA_ZKLSMXB.DLYX=B_DLYX left join AAA_MJMJJYZHYJJRZHGLB on B_DLYX=AAA_MJMJJYZHYJJRZHGLB.DLYX where  XM ='转出资金' and SFDQMRJJR='是' and JE>" + (txtje.Text.Trim() == "" ? 999999999999 : Convert.ToDouble(txtje.Text.Trim()) * 10000) + " order by AAA_ZKLSMXB.CreateTime desc";
        }
        else if (ddllb.SelectedValue.Trim() == "一天")
        {
            sql = "select I_JYFMC as 账户名称,replace(AAA_ZKLSMXB.JSZHLX,'交易账户','') as 账户类型,I_ZCLB as 注册类别,I_LXRXM as 联系人,I_LXRSJH as 联系方式,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX=AAA_MJMJJYZHYJJRZHGLB.GLJJRDLZH) as 所属分公司 ,CONVERT(varchar(100), AAA_ZKLSMXB.CreateTime, 111)   as 时间, count(*)as 出金次数, sum(JE) as 金额 from  AAA_ZKLSMXB join AAA_DLZHXXB on AAA_ZKLSMXB.DLYX=B_DLYX left join AAA_MJMJJYZHYJJRZHGLB on B_DLYX=AAA_MJMJJYZHYJJRZHGLB.DLYX where  XM ='转出资金' and SFDQMRJJR='是' group by I_JYFMC ,AAA_ZKLSMXB.JSZHLX ,I_ZCLB ,I_LXRXM,I_LXRSJH,CONVERT(varchar(100), AAA_ZKLSMXB.CreateTime, 111), AAA_MJMJJYZHYJJRZHGLB.GLJJRDLZH  having  sum(JE)>" + (txtje.Text.Trim() == "" ? 999999999999 : Convert.ToDouble(txtje.Text.Trim()) * 10000) + "order by CONVERT(varchar(100), AAA_ZKLSMXB.CreateTime, 111) desc";
        }

        DataSet dataSet = DbHelperSQL.Query(sql);
        DataTable dtable = dataSet.Tables[0];

        if (dtable != null && dtable.Rows.Count > 0)
        {
            //dtable.Columns.Remove("Number");

            // dtable.Columns["MB001"].ColumnName = "品号";   
            //dtable.Columns["批号"].SetOrdinal(4);
            // DataTableToExcel(dtable);

            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "大额出金检测表", "大额出金检测表", 15);
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "没有可以导出的数据，请重新进行查询！");

        }

    }


    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = GetSql();
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " 时间 ";  //所检索表的主键
        ht_where["search_paixu"] = " desc ";  //排序方式
        ht_where["search_paixuZD"] = " 时间 ";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_HWSF/FHD.aspx";
        return ht_where;
    }
    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
       
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


        //string sql_where = " and  金额 > " + txtje.Text.ToString().Trim()  ;

        //HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }


    protected void BtnCheck_Click(object sender, EventArgs e)
    {
         
             DisGrid();
       
       
    }  

   
}