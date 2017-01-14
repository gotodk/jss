using FMOP.DB;
using Hesion.Brick.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_JHJX_New2013_JHJX_SPMM_jhjx_wltj : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            DisGrid();
        }
    }    

    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        DataTable dt = new DataTable();
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.ShowAlertAndBack(this, "查询超时，请重试或者修改查询条件！");
        }
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                //获取卖家、买家对应信息
                DataSet ds_jyfzh = DbHelperSQL.Query("select T_YSTBDDLYX as 卖家账号,Y_YSYDDDLYX as 买家账号 from AAA_ZBDBXXB where number='" + NewDS.Tables[0].Rows[i]["ZBDBXXBBH"].ToString() + "'");
                if (ds_jyfzh != null && ds_jyfzh.Tables[0].Rows.Count > 0)
                {
                    NewDS.Tables[0].Rows[i]["卖家账号"] = ds_jyfzh.Tables[0].Rows[0]["卖家账号"].ToString();
                    NewDS.Tables[0].Rows[i]["买家账号"] = ds_jyfzh.Tables[0].Rows[0]["买家账号"].ToString();
                    DataSet ds_saleinfo = DbHelperSQL.Query("select I_JYFMC,I_LXRXM,I_LXRSJH from AAA_DLZHXXB where B_DLYX='" + ds_jyfzh.Tables[0].Rows[0]["卖家账号"].ToString() + "' ");
                    if (ds_saleinfo != null && ds_saleinfo.Tables[0].Rows.Count > 0)
                    {
                        NewDS.Tables[0].Rows[i]["卖家名称"] = ds_saleinfo.Tables[0].Rows[0]["I_JYFMC"].ToString();
                        NewDS.Tables[0].Rows[i]["卖家联系人"] = ds_saleinfo.Tables[0].Rows[0]["I_LXRXM"].ToString();
                        NewDS.Tables[0].Rows[i]["卖家联系方式"] = ds_saleinfo.Tables[0].Rows[0]["I_LXRSJH"].ToString();
                    }
                    DataSet ds_buyinfo = DbHelperSQL.Query("select I_JYFMC,I_LXRXM,I_LXRSJH from AAA_DLZHXXB where B_DLYX='" + ds_jyfzh.Tables[0].Rows[0]["买家账号"].ToString() + "' ");
                    if (ds_saleinfo != null && ds_saleinfo.Tables[0].Rows.Count > 0)
                    {
                        NewDS.Tables[0].Rows[i]["买家名称"] = ds_buyinfo.Tables[0].Rows[0]["I_JYFMC"].ToString();
                        NewDS.Tables[0].Rows[i]["买家联系人"] = ds_buyinfo.Tables[0].Rows[0]["I_LXRXM"].ToString();
                        NewDS.Tables[0].Rows[i]["买家联系方式"] = ds_buyinfo.Tables[0].Rows[0]["I_LXRSJH"].ToString();
                    }
                }
            }
            ts.Visible = false;
            dt = NewDS.Tables[0];
            Repeater1.DataSource = dt.DefaultView;
        }
        else
        {
            Repeater1.DataSource = null;
            ts.Visible = true;
        }
        Repeater1.DataBind();
    }

    //设置初始默认值检索
    private Hashtable SetV()
    {
        //Hashtable ht_where = new Hashtable();
        //ht_where["page_size"] = " 15 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = " ( select a.Number as '发货单号', isnull( b.T_YSTBDDLYX,'无') as '卖家账号',(select  I_JYFMC from AAA_DLZHXXB where B_DLYX=b.T_YSTBDDLYX) as '卖家名称', (select  I_LXRXM from AAA_DLZHXXB where B_DLYX=b.T_YSTBDDLYX) as '卖家联系人', (select  I_LXRSJH from AAA_DLZHXXB where B_DLYX=b.T_YSTBDDLYX )  as '卖家联系方式',(b.Y_YSYDDDLYX) as '买家账号',(select  I_JYFMC from AAA_DLZHXXB where B_DLYX=b.Y_YSYDDDLYX) as '买家名称', isnull((select  I_LXRXM from AAA_DLZHXXB where B_DLYX=b.Y_YSYDDDLYX),'无') as '买家联系人', (select  I_LXRSJH from AAA_DLZHXXB where B_DLYX=b.Y_YSYDDDLYX )  as '买家联系方式',a.F_WLGSMC as '物流公司名称',a.F_WLDH as '物流单号',a.F_WLGSLXR as '物流联系人',a.F_WLGSDH as '物流联系电话',F_WLXXLRSJ  from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where F_WLXXLRSJ != '' and F_WLXXLRSJ  is not null  ) as tab1 ";  //检索的表
        //ht_where["search_mainid"] = "tab1.发货单号";  //所检索表的主键
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        //ht_where["search_paixu"] = " DESC ";  //排序方式
        //ht_where["search_paixuZD"] = "tab1.发货单号";  //用于排序的字段

        /*---shiyan 2013-12-26 进行数据获取优化。---*/
        Hashtable ht_where = new Hashtable();
       ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是  
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " Number as 发货单号,ZBDBXXBBH, '' as 卖家账号,'' as 卖家名称, '' as 卖家联系人,'' as 卖家联系方式,'' as 买家账号,'' as 买家名称, '' as 买家联系人, ''  as 买家联系方式,F_WLGSMC as 物流公司名称,F_WLDH as 物流单号,F_WLGSLXR as 物流联系人,F_WLGSDH as 物流联系电话 ";
        ht_where["search_tbname"] = " AAA_THDYFHDXXB ";  //检索的表
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_str_where"] = " F_WLXXLRSJ  is not null ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " Number ";  //用于排序的字段
        return ht_where;
    }
    public void DisGrid()
    {
        hidwhereis.Value = "";

        Hashtable HTwhere = SetV();
        if (!txtBeginTime.Text.Trim().Equals(""))
        {
            HTwhere["search_str_where"] += " and F_WLXXLRSJ >='" + txtBeginTime.Text.Trim()+"'";
        }
        if (!TextBox1.Text.Trim().Equals(""))
        {
            HTwhere["search_str_where"] += " and F_WLXXLRSJ <='" + TextBox1.Text.Trim() + " 23:59:59.999' ";
        }

        // HTwhere["search_str_where"] += "  and 所属分公司 = '" + this.ddlssfgs.SelectedValue.ToString() + "' and 经纪人编号 like '%" + txtjjrbh.Text.Trim() + "%' and 经纪人名称 like '%" + txtjjrxm.Text.Trim() + "%'";
       // hidwhere.Value = HTwhere["search_tbname"].ToString();
        hidwhereis.Value = HTwhere["search_str_where"].ToString();
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void btnDC_Click(object sender, EventArgs e)
    {
        //StringBuilder stringBuilder = new StringBuilder();
        //stringBuilder.Append(" select '" + this.hidID.Value + "' as '收益时间',*  from ");
       // stringBuilder.Append(" select *  from " + this.hidwhere.Value + " where " + this.hidwhereis.Value + "  order by tab1.发货单号 desc");
         //DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString());
        //dataSet.Tables[0].Columns.Remove("F_WLXXLRSJ");

        string sql = "select  Number as 发货单号,'' as 卖家账号,'' as 卖家名称, '' as 卖家联系人,'' as 卖家联系方式,'' as 买家账号,'' as 买家名称, '' as 买家联系人, ''  as 买家联系方式,F_WLGSMC as 物流公司名称,F_WLDH as 物流单号,F_WLGSLXR as 物流联系人,F_WLGSDH as 物流联系电话,ZBDBXXBBH as 合同编号 from AAA_THDYFHDXXB where " + hidwhereis.Value.ToString() + " order by Number desc";
        DataSet dataSet = DbHelperSQL.Query(sql);
        for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
        {
            //获取卖家、买家对应信息
            DataSet ds_jyfzh = DbHelperSQL.Query("select T_YSTBDDLYX as 卖家账号,Y_YSYDDDLYX as 买家账号 from AAA_ZBDBXXB where number='" + dataSet.Tables[0].Rows[i]["合同编号"].ToString() + "'");
            if (ds_jyfzh != null && ds_jyfzh.Tables[0].Rows.Count > 0)
            {
                dataSet.Tables[0].Rows[i]["卖家账号"] = ds_jyfzh.Tables[0].Rows[0]["卖家账号"].ToString();
                dataSet.Tables[0].Rows[i]["买家账号"] = ds_jyfzh.Tables[0].Rows[0]["买家账号"].ToString();
                DataSet ds_saleinfo = DbHelperSQL.Query("select I_JYFMC,I_LXRXM,I_LXRSJH from AAA_DLZHXXB where B_DLYX='" + ds_jyfzh.Tables[0].Rows[0]["卖家账号"].ToString() + "' ");
                if (ds_saleinfo != null && ds_saleinfo.Tables[0].Rows.Count > 0)
                {
                    dataSet.Tables[0].Rows[i]["卖家名称"] = ds_saleinfo.Tables[0].Rows[0]["I_JYFMC"].ToString();
                    dataSet.Tables[0].Rows[i]["卖家联系人"] = ds_saleinfo.Tables[0].Rows[0]["I_LXRXM"].ToString();
                    dataSet.Tables[0].Rows[i]["卖家联系方式"] = ds_saleinfo.Tables[0].Rows[0]["I_LXRSJH"].ToString();
                }
                DataSet ds_buyinfo = DbHelperSQL.Query("select I_JYFMC,I_LXRXM,I_LXRSJH from AAA_DLZHXXB where B_DLYX='" + ds_jyfzh.Tables[0].Rows[0]["买家账号"].ToString() + "' ");
                if (ds_saleinfo != null && ds_saleinfo.Tables[0].Rows.Count > 0)
                {
                    dataSet.Tables[0].Rows[i]["买家名称"] = ds_buyinfo.Tables[0].Rows[0]["I_JYFMC"].ToString();
                    dataSet.Tables[0].Rows[i]["买家联系人"] = ds_buyinfo.Tables[0].Rows[0]["I_LXRXM"].ToString();
                    dataSet.Tables[0].Rows[i]["买家联系方式"] = ds_buyinfo.Tables[0].Rows[0]["I_LXRSJH"].ToString();
                }
            }
        }
        dataSet.Tables[0].Columns.Remove("合同编号");
       
        MyXlsClass MXC = new MyXlsClass();

        MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "物流信息统计", "物流信息统计", 15);

    }
}