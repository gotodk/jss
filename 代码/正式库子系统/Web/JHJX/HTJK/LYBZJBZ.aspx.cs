using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.Data;
using FMOP.DB;
using System.Collections;

public partial class Web_JHJX_HTJK_LYBZJBZ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);

        if (!IsPostBack)
        {
 
            SetTiaoJian();
            if (spanTiaoJian.InnerText.Trim() == "" || spanTiaoJian.InnerText.Trim() == "0")
            {
                MessageBox.ShowAndRedirect(this, "您尚未设置“履约保证金不足卖家”的监控条件。将为您跳转到条件维护页面！", "YJTJ_Set.aspx");
            }
            else
            {
                DisGrid();
            }
        }
    }

    private void SetTiaoJian()
    {
        string sql = "select * from AAA_YJTJWHB";
        DataSet ds = DbHelperSQL.Query(sql);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["LYBZJXYDYJEBFB"].ToString() != "" && ds.Tables[0].Rows[0]["LYBZJXYDYJEBFB"].ToString() != "0")
            {
                spanTiaoJian.InnerText = ds.Tables[0].Rows[0]["LYBZJXYDYJEBFB"].ToString();
            }
        }
    }

    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERPinfo)
    {

        //if (ERRinfo.IndexOf("超时") >= 0)
        //{
        //    MessageBox.ShowAlertAndBack(this, "查询超时，请重试或者修改查询条件！");
        //}
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            tdEmpty.Visible = false;
            DataTable dt = NewDS.Tables[0];
            rpt.DataSource = dt.DefaultView;
        }
        else { tdEmpty.Visible = true; }
        rpt.DataBind();
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();

        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select isnull(c.SFCL,'否')  as 是否已处理,b.B_SFDJ as 卖家冻结状态,T_YSTBDDLYX as 卖家账号, b.I_JYFMC  as 卖家名称,b.I_LXRXM as 卖家联系人,b.I_LXRSJH as 卖家联系方式,Z_SPBH as 商品编号,Z_SPMC as 商品名称,Z_GG as 规格,convert(varchar(10),Z_HTJSRQ,120) as 合同结束时间,Z_HTBH as 合同编号,Z_LYBZJJE as 履约保证金原金额,isnull(d.扣罚,0.00) as 已扣罚履约保证金,cast((Z_LYBZJJE-ISNULL(d.扣罚,0.00))/Z_LYBZJJE*100 as numeric(18,2)) as 剩余履约保证金比率 from AAA_ZBDBXXB as a left join AAA_DLZHXXB as b on a.T_YSTBDDLYX =b.B_DLYX left join (select * from AAA_DZGHTHJKTZCLB where JKLX ='履约保证金不足卖家' and LYSJLX='电子购货合同') as c on c.LYDH =a.Z_HTBH left join (select DLYX as 卖家账号,sum(JE) as 扣罚,b.ZBDBXXBBH as 编号,c.Z_HTBH as 合同编号  from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.Number left join AAA_ZBDBXXB as c on c.Number =b.ZBDBXXBBH  where XM ='违约赔偿金' and (XZ='超过最迟发货日后录入发货信息' or XZ ='发货单生成后超过5日内未录入发票邮寄信息') and SJLX ='预' and LYYWLX ='AAA_THDYFHDXXB' group by DLYX,b.ZBDBXXBBH ,c.Z_HTBH) as d on d.合同编号=a.Z_HTBH where (Z_HTZT ='定标' or Z_HTZT='定标合同终止') and (Z_QPZT ='未开始清盘' or Z_QPZT='清盘中' ) and CONVERT(varchar(10),Z_DBSJ,120)<=CONVERT(varchar(10),GETDATE (),120) and CONVERT(varchar(10),Z_HTJSRQ,120)>=CONVERT(varchar(10),GETDATE (),120) and cast((Z_LYBZJJE-ISNULL(d.扣罚,0.00))/Z_LYBZJJE*100 as numeric(18,2)) <="+Convert.ToDouble(spanTiaoJian.InnerText.Trim ())+") as tab ";  //检索的表
        ht_where["search_mainid"] = " 合同编号 ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " 剩余履约保证金比率,合同结束时间 ";  //用于排序的字段        

        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        if (txtHTBH.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 合同编号 like '%" + txtHTBH.Text.Trim() + "%'";
        }
        if (ddlCLZT.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 是否已处理='" + ddlCLZT.SelectedValue.ToString() + "'";
        }

        ViewState["ht_where"] = HTwhere["search_str_where"];
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string sql = " select * from (select isnull(c.SFCL,'否')  as 是否已处理,b.B_SFDJ as 卖家冻结状态,T_YSTBDDLYX as 卖家账号, b.I_JYFMC  as 卖家名称,b.I_LXRXM as 卖家联系人,b.I_LXRSJH as 卖家联系方式,Z_SPBH as 商品编号,Z_SPMC as 商品名称,Z_GG as 规格,convert(varchar(10),Z_HTJSRQ,120) as 合同结束时间,Z_HTBH as 合同编号,Z_LYBZJJE as 履约保证金原金额,isnull(d.扣罚,0.00) as 已扣罚履约保证金 from AAA_ZBDBXXB as a left join AAA_DLZHXXB as b on a.T_YSTBDDLYX =b.B_DLYX left join (select * from AAA_DZGHTHJKTZCLB where JKLX ='履约保证金不足卖家' and LYSJLX='电子购货合同') as c on c.LYDH =a.Z_HTBH left join (select DLYX as 卖家账号,sum(JE) as 扣罚,b.ZBDBXXBBH as 编号,c.Z_HTBH as 合同编号  from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.Number left join AAA_ZBDBXXB as c on c.Number =b.ZBDBXXBBH  where XM ='违约赔偿金' and (XZ='超过最迟发货日后录入发货信息' or XZ ='发货单生成后超过5日内未录入发票邮寄信息') and SJLX ='预' and LYYWLX ='AAA_THDYFHDXXB' group by DLYX,b.ZBDBXXBBH ,c.Z_HTBH) as d on d.合同编号=a.Z_HTBH where (Z_HTZT ='定标' or Z_HTZT='定标合同终止') and (Z_QPZT ='未开始清盘' or Z_QPZT='清盘中' ) and CONVERT(varchar(10),Z_DBSJ,120)<=CONVERT(varchar(10),GETDATE (),120) and CONVERT(varchar(10),Z_HTJSRQ,120)>=CONVERT(varchar(10),GETDATE (),120) and cast((Z_LYBZJJE-ISNULL(d.扣罚,0.00))/Z_LYBZJJE*100 as numeric(18,2)) <=" + Convert.ToDouble(spanTiaoJian.InnerText.Trim()) + ") as tab where " + ViewState["ht_where"].ToString() + " order by 合同结束时间,合同编号 ";
        DataSet ds = DbHelperSQL.Query(sql);

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(ds, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "履约保证金不足卖家", "履约保证金不足卖家", 25);
    }
    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "btnCL")
        {
            WorkFlowModule WFM = new WorkFlowModule("AAA_DZGHTHJKTZCLB");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            string sql = "INSERT INTO [AAA_DZGHTHJKTZCLB]([Number],[JKLX],[SFCL],[LYSJLX],[LYDH],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber + "' ,'履约保证金不足卖家','是','电子购货合同','" + e.CommandArgument.ToString() + "',1,'" + User.Identity.Name.ToString() + "','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";

            try
            {
                DbHelperSQL.ExecuteSql(sql);
                MessageBox.Show(this, "通知处理操作成功！");
                DisGrid();
            }
            catch (Exception ex)
            {
                MessageBox.ShowAlertAndBack(this, "通知处理操作失败！原因：" + ex.ToString());
            }
        }
    }
}