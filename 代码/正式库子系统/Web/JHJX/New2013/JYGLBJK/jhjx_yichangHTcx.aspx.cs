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

public partial class Web_JHJX_New2013_JYGLBJK_jhjx_yichangHTcx : System.Web.UI.Page
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
        ht_where["page_size"] = " 15 ";
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select Z_HTBH as '合同编号',(case when Z_YTHSL !=Z_ZBSL then '是' else '否' end ) as '买家是否违约' , (case when (select COUNT(Number) from AAA_THDYFHDXXB where F_DQZT in ('未生成发货单','已生成发货单') and ZBDBXXBBH = AAA_ZBDBXXB.Number)=0 then '否' else '是' end) as '卖家是否违约',Y_YSYDDDLYX as '买家账号',( select  I_JYFMC from AAA_DLZHXXB where B_DLYX=Y_YSYDDDLYX) as '买家名称',T_YSTBDDLYX as '卖家账号',( select  I_JYFMC from AAA_DLZHXXB where B_DLYX=T_YSTBDDLYX) as '卖家名称',Z_SPBH as '商品编码',Z_SPMC as '商品名称',Z_DBSJ as '合同开始时间',Z_HTJSRQ as '合同结束时间',isnull(Z_ZBSL,0) as '合同数量',isnull(Z_YTHSL,0) as '提货数量',(isnull(Z_ZBSL,0)-isnull(Z_YTHSL,0)) as '未提货数量',(select  ISNULL( sum(isnull(T_THSL,0)),0) from AAA_THDYFHDXXB where F_DQZT not in ('撤销','未生成发货单','已生成发货单') and ZBDBXXBBH = AAA_ZBDBXXB.Number) as '发货数量',(isnull(Z_YTHSL,0)-(select  ISNULL( sum(isnull(T_THSL,0)),0) from AAA_THDYFHDXXB where F_DQZT not in ('撤销','未生成发货单','已录入发货信息') and ZBDBXXBBH = AAA_ZBDBXXB.Number)) as '未发货数量' ,cast((case when  isnull (Z_ZBSL,0)=0 then 0 else  isnull(Z_YTHSL,0)/isnull (Z_ZBSL,0)  end )as numeric(18,2)) as '买家履约率' ,  cast((case when  isnull(Z_YTHSL,0)=0 then 0 else (select  ISNULL( sum(isnull(T_THSL,0)),0) from AAA_THDYFHDXXB where F_DQZT not in ('撤销','未生成发货单','已录入发货信息') and ZBDBXXBBH = AAA_ZBDBXXB.Number)/ isnull(Z_YTHSL,0) end ) as numeric(18,2) ) as '卖家履约率' , (case when Z_QPZT='清盘结束' then '是' else '否' end ) as '清盘是否完成', ((( select  ISNULL(sum(isnull(T_THSL,0)),0) from AAA_THDYFHDXXB where F_DQZT  in ('无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货') and ZBDBXXBBH =AAA_ZBDBXXB.Number)+ ( select  ISNULL(sum(isnull(F_BUYBFSHSJQSSL,0)),0) from AAA_THDYFHDXXB where F_DQZT ='部分收货' and ZBDBXXBBH =AAA_ZBDBXXB.Number))) as '实际收货数量', cast ((case when isnull(Z_ZBSL,0) = 0 then 0 else ( ((( select  ISNULL(sum(isnull(T_THSL,0)),0) from AAA_THDYFHDXXB where F_DQZT  in ('无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货') and ZBDBXXBBH =AAA_ZBDBXXB.Number)+ ( select  ISNULL(sum(isnull(F_BUYBFSHSJQSSL,0)),0) from AAA_THDYFHDXXB where F_DQZT ='部分收货' and ZBDBXXBBH =AAA_ZBDBXXB.Number)))/isnull(Z_ZBSL,0)) end ) as numeric(18,2) )as '有效履约率' from AAA_ZBDBXXB where Z_HTJSRQ is not null  and GETDATE()> Z_HTJSRQ and (Z_YTHSL !=Z_ZBSL or Number in ( select distinct a.Number  from AAA_ZBDBXXB as a  left join AAA_THDYFHDXXB  as b on a.Number = b.ZBDBXXBBH where b.F_DQZT in ('未生成发货单','已生成发货单') ) ) ) as tab ";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " 合同编号 ";  //所检索表的主键
        ht_where["search_paixu"] = " asc ";  //排序方式
        ht_where["search_paixuZD"] = " 合同结束时间";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_CSSPZLSH/JHJX_CKXQ.aspx";
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

            Repeater1.DataSource = NewDS;
            ts.Visible = true;

        }
        Repeater1.DataBind();
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();

        if (ddlyy != null && !ddlyy.SelectedValue.Trim().Equals("") && !ddlyy.SelectedValue.Trim().Equals("全部"))
        {
            if (ddlyy.SelectedValue.Trim().Equals("仅买家违约"))
            {
                HTwhere["search_str_where"] += " and  买家是否违约 = '是' and 卖家是否违约 ='否' ";
            }
            if (ddlyy.SelectedValue.Trim().Equals("仅卖家违约"))
            {
                HTwhere["search_str_where"] += " and  买家是否违约 = '否' and 卖家是否违约 ='是' ";
            }
            if (ddlyy.SelectedValue.Trim().Equals("买家卖家均有违约"))
            {
                HTwhere["search_str_where"] += " and  买家是否违约 = '是' and 卖家是否违约 ='是' ";
            }
        }
        if (!this.txtJYFMC.Text.Trim().Equals(""))
        {
            HTwhere["search_str_where"] += " and  (买家名称  like '%" + this.txtJYFMC.Text.Trim() + "%' or 卖家名称 like '%" + this.txtJYFMC.Text.Trim() + "%') ";
        }
        if (!this.txtBeginTime.Text.Trim().Equals(""))
        {
            HTwhere["search_str_where"] += " and 合同结束时间>='" + this.txtBeginTime.Text.Trim()+"'";
        }
        if (!this.txtEndTime.Text.Trim().Equals(""))
        {
            HTwhere["search_str_where"] += " and 合同结束时间<='" + this.txtEndTime.Text.Trim() + " 23:59:59.999'";
        }

       // string sql_where = " and 交易方账号 like '%" + txtJYFZH.Text.Trim() + "%' and 交易方名称 like '%" + txtJYFMC.Text.Trim() + "%'";

        this.hidwhereis.Value = HTwhere["search_tbname"].ToString();
        this.hidwhere.Value = HTwhere["search_str_where"].ToString();

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        if (this.Repeater1 != null && this.Repeater1.Items.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(" select '" + this.hidID.Value + "' as '收益时间',*  from ");
            stringBuilder.Append(" select * from  " + this.hidwhereis.Value + " where " + this.hidwhere.Value.ToString() + " order by 合同结束时间 asc ");
            // stringBuilder.Append(" where " + hidwhereis.Value.ToString());
            DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString());
            
            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "异常合同查询", "异常合同查询", 15);
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "列表中没有数据可导出！");
        }
    }
}