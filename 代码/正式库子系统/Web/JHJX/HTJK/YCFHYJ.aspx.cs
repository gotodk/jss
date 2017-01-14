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

public partial class Web_JHJX_HTJK_YCFHYJ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);

        if (!IsPostBack)
        {
     
            SetTiaoJian();
            if (spanTiaoJian.InnerText.Trim() == "" || spanTiaoJian.InnerText.Trim() == "0")
            {
                MessageBox.ShowAndRedirect(this, "您尚未设置“延迟发货预警”的监控条件。将为您跳转到条件维护页面！", "YJTJ_Set.aspx");
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
            if (ds.Tables[0].Rows[0]["YCFHYJTS"].ToString() != "" && ds.Tables[0].Rows[0]["YCFHYJTS"].ToString() != "0")
            {
                spanTiaoJian.InnerText = ds.Tables[0].Rows[0]["YCFHYJTS"].ToString();
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
        ht_where["search_tbname"] = " (select a.Number ,'T'+a.Number as 提货单号,(case when F_FHDSCSJ is null then '无' else 'F'+a.Number end ) as 发货单号,CONVERT(varchar(10),T_ZCFHR,120) as 最迟发货日,b.Z_SPBH  as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG  as 规格,d.B_SFDJ as 卖家冻结状态,b.T_YSTBDDLYX as 卖家账号,d.I_JYFMC as 卖家名称,d.I_LXRXM as 卖家联系人,d.I_LXRSJH as  卖家联系方式,CONVERT(varchar(10),b.Z_HTJSRQ,120) as 合同结束时间,b.Z_HTBH as 合同编号,b.Z_ZBSL  as 合同数量,b.Y_YSYDDDLYX as 买家账号,c.I_JYFMC  as 买家名称,isnull(e.SFCL,'否') as 是否已处理 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH =b.Number left join AAA_DLZHXXB as c on c.B_DLYX=b.Y_YSYDDDLYX  left join AAA_DLZHXXB as d on d.B_DLYX=b.T_YSTBDDLYX left join (select * from AAA_DZGHTHJKTZCLB where JKLX ='延迟发货预警' and LYSJLX ='提货单') as e on e.LYDH ='T'+a.Number  where Z_HTZT='定标' and Z_HTQX<>'即时' and F_WLXXLRSJ is null  and CONVERT(varchar(10),b.Z_DBSJ,120)<=CONVERT(varchar(10),GETDATE (),120) and CONVERT(varchar(10),b.Z_HTJSRQ,120)>=CONVERT(varchar(10),GETDATE (),120) and convert(varchar(10),GETDATE(),120)>CONVERT(varchar(10), DATEADD (dd,"+(-Convert .ToInt16(spanTiaoJian .InnerText))+",a.T_ZCFHR),120) and convert(varchar(10),GETDATE(),120)<=CONVERT(varchar(10),a.T_ZCFHR ,120)) as tab ";  //检索的表
        ht_where["search_mainid"] = " 提货单号 ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " 最迟发货日,提货单号 ";  //用于排序的字段        

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
        string sql = " select * from  (select 'T'+a.Number as 提货单号,(case when F_FHDSCSJ is null then '无' else 'F'+a.Number end ) as 发货单号,CONVERT(varchar(10),T_ZCFHR,120) as 最迟发货日,b.Z_SPBH  as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG  as 规格,d.B_SFDJ as 卖家冻结状态,b.T_YSTBDDLYX as 卖家账号,d.I_JYFMC as 卖家名称,d.I_LXRXM as 卖家联系人,d.I_LXRSJH as  卖家联系方式,CONVERT(varchar(10),b.Z_HTJSRQ,120) as 合同结束时间,b.Z_HTBH as 合同编号,b.Z_ZBSL  as 合同数量,b.Y_YSYDDDLYX as 买家账号,c.I_JYFMC  as 买家名称,isnull(e.SFCL,'否') as 是否已处理 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH =b.Number left join AAA_DLZHXXB as c on c.B_DLYX=b.Y_YSYDDDLYX  left join AAA_DLZHXXB as d on d.B_DLYX=b.T_YSTBDDLYX left join (select * from AAA_DZGHTHJKTZCLB where JKLX ='延迟发货预警' and LYSJLX ='提货单') as e on e.LYDH ='T'+a.Number  where Z_HTZT='定标' and Z_HTQX<>'即时' and F_WLXXLRSJ is null  and CONVERT(varchar(10),b.Z_DBSJ,120)<=CONVERT(varchar(10),GETDATE (),120) and CONVERT(varchar(10),b.Z_HTJSRQ,120)>=CONVERT(varchar(10),GETDATE (),120) and convert(varchar(10),GETDATE(),120)>CONVERT(varchar(10), DATEADD (dd," + (-Convert.ToInt16(spanTiaoJian.InnerText)) + ",a.T_ZCFHR),120) and convert(varchar(10),GETDATE(),120)<=CONVERT(varchar(10),a.T_ZCFHR ,120)) as tab where " + ViewState["ht_where"].ToString() + " order by 最迟发货日,提货单号 ";
        DataSet ds = DbHelperSQL.Query(sql);

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(ds, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "延迟发货预警", "延迟发货预警", 25);
    }
    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "btnCL")
        {
            WorkFlowModule WFM = new WorkFlowModule("AAA_DZGHTHJKTZCLB");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            string sql = "INSERT INTO [AAA_DZGHTHJKTZCLB]([Number],[JKLX],[SFCL],[LYSJLX],[LYDH],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber + "' ,'延迟发货预警','是','提货单','" + e.CommandArgument.ToString() + "',1,'" + User.Identity.Name.ToString() + "','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";

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