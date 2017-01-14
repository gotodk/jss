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

public partial class Web_JHJX_New2013_JHJX_FGSYWCX_jhjx_jyzhywchax : System.Web.UI.Page
{
    string StrGettbdsql = "";
    string StrGettbdchaifen = "";
    string Stryddzhengchang = "";
    string Stryddchaidan = "";
    Hashtable ht_where = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
  

            setchengshi();
            DisGrid();
        }
    }


    protected void setchengshi()
    {
        string MainERP = DbHelperSQL.GetSingle("SELECT DYERP FROM SYSTEM_CITY_0 WHERE NAME='公司总部'").ToString();//总部对应的ERP名称
        DataSet ds = DbHelperSQL.Query("SELECT DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处' ");
        ddlssfgs.DataSource = ds.Tables[0].DefaultView;
        ddlssfgs.DataTextField = "DYFGSMC";
        ddlssfgs.DataValueField = "DYFGSMC";
        ddlssfgs.DataBind();
        ddlssfgs.Items.Insert(0, new ListItem("全部分公司", ""));
        string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
        string bm = DbHelperSQL.GetSingle(sql).ToString();
        if (bm.IndexOf("办事处") > 0)
        {
            sql = "select count(name) from system_city where name='" + bm + "'";
            string fgsSql = "select DYFGSMC from system_city where name='" + bm + "'";
            if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
            {
                string strFGSMC = DbHelperSQL.GetSingle(fgsSql).ToString();
                ddlssfgs.DataSource = null;
                ddlssfgs.DataBind();
                ddlssfgs.Items.Clear();
                ddlssfgs.Items.Add(strFGSMC);

            }
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
            tdEmpty.Visible = false;
            dt = NewDS.Tables[0];

            rptSPXX.DataSource = dt.DefaultView;
        }
        else { tdEmpty.Visible = true; }
        rptSPXX.DataBind();
    }

    //设置初始默认值检索
    private Hashtable SetV()
    {


        StrGettbdsql = "(select  a.DLYX as '交易方账号',(select I_JYFMC  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '交易方名称',(select I_PTGLJG  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as '所属分公司',(select B_JSZHLX  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '账户类型',('T'+a.Number) as Number, CONVERT(varchar(100),(case  a.ZT when '竞标' then case b.SFJRLJQ  when '是' then b.CreateTime else a.CreateTime  end end) , 20)  as '发生时间',SPMC as '商品名称',GG as '规格',a.HTQX as '合同期限',(isnull(TBNSL,0)-isnull(YZBSL,0)) as '数量',TBJE as '金额','卖出' as '交易类型',  case  a.ZT when '竞标' then case b.SFJRLJQ  when '是' then '竞标中（冷静期）' else '竞标中'  end end as '交易状态',case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != TBNSL then '是' when YZBSL>0 and YZBSL = TBNSL and (select Count(Number) from AAA_ZBDBXXB where T_YSTBDBH = a.Number) >1 then '是' else '否'  end  as '是否拆单','---' as '经纪人收益' from AAA_TBD as a left join AAA_LJQDQZTXXB as b on a.SPBH=b.SPBH and a.HTQX = b.HTQX   where  ZT= '竞标' ) ";

        StrGettbdchaifen = "(select   a.DLYX as '交易方账号',(select I_JYFMC  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '交易方名称',(select I_PTGLJG  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as '所属分公司', (select B_JSZHLX  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '账户类型',('T'+a.Number+c.Number) as Number, CONVERT(varchar(100),(case Z_HTZT when '中标' then c.Z_ZBSJ when '未定标废标' then c.Z_FBSJ when '定标合同终止' then c.Z_QPKSSJ when '定标' then c.Z_DBSJ when '定标合同到期' then c.Z_DBSJ when '定标执行完成' then c.Z_DBSJ  else c.CreateTime end  ) , 20)  as '发生时间', SPMC as '商品名称',GG as '规格',a.HTQX as '合同期限', c.Z_ZBSL as '数量',c.Z_ZBJE as '金额', '卖出' as '交易类型', case Z_HTZT when '中标' then '中标' when '未定标废标' then '废标'  when '定标合同终止' then '废标' when '定标' then '定标' when '定标合同到期' then '定标' when '定标执行完成' then '定标'  else '其他' end  as '交易状态' , case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != TBNSL then '是' when YZBSL>0 and YZBSL = TBNSL and (select Count(Number) from AAA_ZBDBXXB where T_YSTBDBH =a.Number)>1 then '是' else '否'  end  as '是否拆单',CONVERT(varchar(100),((isnull((select sum( isnull(JE,0.00))  from  AAA_ZKLSMXB  where XM='经纪人收益' and XZ='来自卖家收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and LYDH in ( select Number from AAA_THDYFHDXXB where ZBDBXXBBH = c.Number  ) and DLYX=c.T_YSTBDGLJJRYX),0.00)+isnull((select sum(isnull(JE,0.00))  from  AAA_ZKLSMXB  where XM='经纪人收益' and XZ='来自买卖收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and LYDH=c.Number and DLYX=c.T_YSTBDGLJJRYX),0.00))) , 20) as '经纪人收益'  from AAA_ZBDBXXB as c  left join AAA_LJQDQZTXXB as b on c.Z_SPBH=b.SPBH and c.Z_HTQX = b.HTQX left join AAA_TBD as a on a.Number = c.T_YSTBDBH) ";

        Stryddzhengchang = "(select a.DLYX as '交易方账号',(select I_JYFMC  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '交易方名称',(select I_PTGLJG  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 所属分公司, (select B_JSZHLX  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '账户类型',('Y'+a.Number) as Number,  CONVERT(varchar(100),(case  a.ZT when '竞标' then case b.SFJRLJQ  when '是' then b.CreateTime else a.CreateTime  end end) , 20) as '发生时间',SPMC as '商品名称',GG as '规格',a.HTQX as '合同期限', (isnull(NDGSL,0)-isnull(YZBSL,0)) as '数量',NDGJE as  金额, '买入' as '交易类型' , case  a.ZT when '竞标' then case b.SFJRLJQ  when '是' then '竞标中（冷静期）' else '竞标中' end   else  case Z_HTZT when '中标' then '中标' when '未定标废标' then '废标'  when '定标合同终止' then '废标' when '定标' then '定标' when '定标合同到期' then '定标' when '定标执行完成' then '定标'  else '其他' end end as '交易状态',  case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != NDGSL then '是' when YZBSL>0 and YZBSL = NDGSL and (select Count(Number) from AAA_ZBDBXXB where Y_YSYDDBH =a.Number)>1 then '是' else '否'  end  as '是否拆单', '---' as '经纪人收益'  from AAA_YDDXXB as a left join AAA_LJQDQZTXXB as b on a.SPBH=b.SPBH and a.HTQX = b.HTQX left join AAA_ZBDBXXB as c on a.Number = c.Y_YSYDDBH  where   ZT='竞标')";


        Stryddchaidan = "(select a.DLYX as '交易方账号',(select I_JYFMC  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '交易方名称',(select I_PTGLJG  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 所属分公司,(select B_JSZHLX  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '账户类型', ('Y'+a.Number+c.Number) as Number, CONVERT(varchar(100),(case Z_HTZT when '中标' then c.Z_ZBSJ when '未定标废标' then c.Z_FBSJ when '定标合同终止' then c.Z_QPKSSJ when '定标' then c.Z_DBSJ when '定标合同到期' then c.Z_DBSJ when '定标执行完成' then c.Z_DBSJ  else c.CreateTime end  ) , 20) as '发生时间',SPMC as '商品名称',GG as '规格',a.HTQX as '合同期限', c.Z_ZBSL as '数量',c.Z_ZBJE as  金额, '买入' as '交易类型',  case Z_HTZT when '中标' then '中标' when '未定标废标' then '废标'  when '定标合同终止' then '废标' when '定标' then '定标' when '定标合同到期' then '定标' when '定标执行完成' then '定标'  else '其他' end  as '交易状态', case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != NDGSL then '是' when YZBSL>0 and YZBSL = NDGSL and (select Count(Number) from AAA_ZBDBXXB where Y_YSYDDBH =a.Number)>1 then '是' else '否'  end  as '是否拆单', CONVERT(varchar(100),(isnull((select sum( isnull(JE,0.00))  from  AAA_ZKLSMXB  where XM='经纪人收益' and XZ='来自买家收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and LYDH in ( select Number from AAA_THDYFHDXXB where ZBDBXXBBH = c.Number  ) and DLYX=c.Y_YSYDDGLJJRYX),0.00)+isnull((select sum(isnull(JE,0.00))  from  AAA_ZKLSMXB  where XM='经纪人收益' and XZ='来自买家收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and LYDH=c.Number and DLYX=c.Y_YSYDDGLJJRYX),0.00)) , 20) as '经纪人收益' from AAA_ZBDBXXB as c left join AAA_LJQDQZTXXB as b on c.Z_SPBH=b.SPBH and c.Z_HTQX = b.HTQX left join AAA_YDDXXB as a on a.Number = c.Y_YSYDDBH  ) ";
        this.hidwhereis.Value = " select * from  (" + StrGettbdsql + " union " + StrGettbdchaifen + "  union " + Stryddzhengchang + " union " + Stryddchaidan + ") as tab  ";
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 15 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (" + StrGettbdsql + " union " + StrGettbdchaifen + "  union " + Stryddzhengchang + " union " + Stryddchaidan + ") as tab  ";  //检索的表
        ht_where["search_mainid"] = "tab.Number";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = "tab.交易类型,tab.发生时间";  //用于排序的字段
        return ht_where;
    }
    public void DisGrid()
    {


        Hashtable HTwhere = SetV();
        if (!txt_spmc.Text.Trim().Equals(""))
        {
            HTwhere["search_str_where"] += " and 商品名称 like '%" + txt_spmc.Text.Trim() + "%' ";
        }
        if (!ddl_djlb.SelectedItem.ToString().Trim().Equals("选择交易类型"))
        {
            HTwhere["search_str_where"] += " and 交易类型 like '%" + ddl_djlb.SelectedItem.ToString().Trim() + "%' ";
        }
        if (!ddl_jyzt.SelectedItem.ToString().Trim().Equals("选择交易状态"))
        {
            HTwhere["search_str_where"] += " and 交易状态 like '%" + ddl_jyzt.SelectedItem.ToString().Trim() + "%' ";
        }


        if (this.ddlssfgs.SelectedValue.ToString() != "" && this.ddlssfgs.SelectedValue.ToString() != "无")
        {
            HTwhere["search_str_where"] += "  and 所属分公司 = '" + this.ddlssfgs.SelectedValue.ToString() + "'";
        }

        if (txtjjrbh.Text.Trim() != "" && txtjjrbh.Text.Trim() != "无")
        {
            HTwhere["search_str_where"] += " and 交易方名称 like '%" + txtjjrbh.Text.Trim() + "%'";
        }

        if (txtjjrxm.Text.Trim() != "" && txtjjrxm.Text.Trim() != "无")
        {
            HTwhere["search_str_where"] += " and 交易方账号 like '%" + txtjjrxm.Text.Trim() + "%'";
        }



        //if (txtjjrbh.Text.Trim() != "" && txtjjrbh.Text.Trim() != "无")
        //{
        //    HTwhere["search_str_where"] += " and 交易方账号 like '%" + txtjjrbh.Text.Trim() + "%'";
        //}

        //if (txtjjrxm.Text.Trim() != "" && txtjjrxm.Text.Trim() != "无")
        //{
        //    HTwhere["search_str_where"] += " and 交易方名称 like '%" + txtjjrxm.Text.Trim() + "%'";
        //}


        // HTwhere["search_str_where"] += "  and 所属分公司 = '" + this.ddlssfgs.SelectedValue.ToString() + "' and 经纪人编号 like '%" + txtjjrbh.Text.Trim() + "%' and 经纪人名称 like '%" + txtjjrxm.Text.Trim() + "%'";
        this.hidwhere.Value = HTwhere["search_str_where"].ToString();
        //  hidwhereis.Value = HTwhere["search_str_where"].ToString();
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.rptSPXX != null && this.rptSPXX.Items.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(" select '" + this.hidID.Value + "' as '收益时间',*  from ");
            stringBuilder.Append(this.hidwhereis.Value + " where " + this.hidwhere.Value.ToString() + " order by tab.交易类型,tab.发生时间 desc ");
            // stringBuilder.Append(" where " + hidwhereis.Value.ToString());
            DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString());
            dataSet.Tables[0].Columns.Remove("Number");
         
            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "分公司所属的交易账户业务_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "分公司所属的交易账户业务", "分公司所属的交易账户业务", 15);
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "列表中没有数据可导出！");
        }

    }


}