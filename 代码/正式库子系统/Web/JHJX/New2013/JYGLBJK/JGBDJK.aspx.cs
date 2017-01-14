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

public partial class Web_JHJX_New2013_JYGLBJK_JGBDJK : System.Web.UI.Page
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
        string strsql = "select CONVERT(varchar(10),e.zbsj ,111) as 中标日期, e.spbh as 商品编码,e.spmc as 商品名称, e.spgg as 商品规格,e.zbjg as 本轮次中标价,f.zbjg as 上轮次定标价,case when f.zbjg is null then null else e.zbjg-f.zbjg end as 波动差值,convert(numeric(18,2), (case when (f.zbjg is null or f.zbjg=0) then null else 100*(e.zbjg-f.zbjg)/f.zbjg  end)) as 波动幅度,e.mjmc as 本轮中标卖家名称,h.buysl as 本轮中标买家数量 from (select a.Z_SPBH as spbh,a.Z_SPMC as spmc,a.Z_GG as spgg,a.zbsj as zbsj,b.Z_ZBJG as zbjg,b.T_YSTBDBH as ystbd,I_JYFMC as mjmc  from (select  Z_SPBH,[Z_SPMC],[Z_GG], max(Z_ZBSJ) as zbsj  from [AAA_ZBDBXXB]group by  Z_SPBH,Z_SPMC,Z_GG) a left join (select distinct Z_ZBJG, T_YSTBDBH, Z_SPBH,T_YSTBDDLYX, max(Z_ZBSJ) as zbsj from [AAA_ZBDBXXB]group by  Z_SPBH , T_YSTBDBH ,Z_ZBJG,T_YSTBDDLYX) b on a.Z_SPBH=b.Z_SPBH and a.zbsj=b.zbsj left join AAA_DLZHXXB on B_DLYX=b.T_YSTBDDLYX) e left join (select c.Z_SPBH as spbh,c.zbsj as zbsj,d.Z_ZBJG as zbjg from (select Z_SPBH, max(Z_ZBSJ) as zbsj from [AAA_ZBDBXXB] where T_YSTBDBH not in	(select b.T_YSTBDBH  from (select  Z_SPBH,[Z_SPMC],[Z_GG], max(Z_ZBSJ) as zbsj  from [AAA_ZBDBXXB]group by  Z_SPBH,Z_SPMC,Z_GG) a left join (select distinct Z_ZBJG, T_YSTBDBH, Z_SPBH,T_YSTBDDLYX, max(Z_ZBSJ) as zbsj from [AAA_ZBDBXXB]group by  Z_SPBH , T_YSTBDBH ,Z_ZBJG,T_YSTBDDLYX) b on a.Z_SPBH=b.Z_SPBH and a.zbsj=b.zbsj ) and Z_HTZT not in('中标','未定标废标') group by  Z_SPBH )c left join ( select Z_SPBH,Z_ZBJG, max(Z_ZBSJ) as zbsj from [AAA_ZBDBXXB] where T_YSTBDBH not in (select b.T_YSTBDBH  from (select  Z_SPBH,[Z_SPMC],[Z_GG], max(Z_ZBSJ) as zbsj  from [AAA_ZBDBXXB]group by  Z_SPBH,Z_SPMC,Z_GG) a left join (select distinct Z_ZBJG, T_YSTBDBH, Z_SPBH,T_YSTBDDLYX, max(Z_ZBSJ) as zbsj from [AAA_ZBDBXXB]group by  Z_SPBH , T_YSTBDBH ,Z_ZBJG,T_YSTBDDLYX) b on a.Z_SPBH=b.Z_SPBH and a.zbsj=b.zbsj )  and Z_HTZT not in('中标','未定标废标') group by  Z_SPBH ,Z_ZBJG)d on d.Z_SPBH=d.Z_SPBH and c.zbsj =d.zbsj ) f on e.spbh=f.spbh left join (select  count( *) as buysl, i.zbsj as zbsj,  i.T_YSTBDBH from (select  distinct Z_ZBSJ as zbsj, T_YSTBDBH,Y_YSYDDDLYX from AAA_ZBDBXXB) i group by  i.T_YSTBDBH,i.zbsj) h on e.ystbd=h.T_YSTBDBH  and e.zbsj=h. zbsj ";

      
      

        return strsql;
    }

    protected void btnDC_Click(object sender, EventArgs e)
    {
        string sql = "";
        if( txtfd.Text.ToString().Trim()!="")
            sql = GetSql() + " where convert(numeric(18,2),(case when (f.zbjg is null or f.zbjg=0) then null else abs(100*(e.zbjg-f.zbjg)/f.zbjg)  end)) > " + txtfd.Text.ToString().Trim() + "order by convert(numeric(18,2), (case when (f.zbjg is null or f.zbjg=0) then null else abs(100*(e.zbjg-f.zbjg)/f.zbjg)  end)) desc";
    

        DataSet dataSet = DbHelperSQL.Query(sql);


        if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
        {
            //dtable.Columns.Remove("Number");

            // dtable.Columns["MB001"].ColumnName = "品号";   
            //dtable.Columns["批号"].SetOrdinal(4);
            // DataTableToExcel(dtable);

            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "价格波动检测表", "价格波动检测表", 15);
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
        ht_where["search_tbname"] = "("+ GetSql()+") as tab";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " 商品编码 ";  //所检索表的主键
        ht_where["search_paixu"] = " asc ";  //排序方式
        ht_where["search_paixuZD"] = " -abs(波动幅度),中标日期, 商品编码  ";  //用于排序的字段
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

        if (txtfd.Text.Trim() != "")
        {
            string sql_where = " and  abs(波动幅度) > " + txtfd.Text.ToString().Trim();

            HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;

            commonpagernew1.HTwhere = HTwhere;
            commonpagernew1.GetFYdataAndRaiseEvent();

        }
        else
        {
            Repeater1.DataSource = null;
            Repeater1.DataBind();
            ts.Visible = true;
 
        }
       
    }


    protected void BtnCheck_Click(object sender, EventArgs e)
    {

        DisGrid();


    }  
}