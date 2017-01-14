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

public partial class Web_JHJX_HTJK_LYJSTX_Buyer : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
    
        if (!IsPostBack)
        {
 
            SetTiaoJian();
            if (spanTiaoJian.InnerText.Trim() == "" || spanTiaoJian.InnerText.Trim() == "0")
            {
                MessageBox.ShowAndRedirect(this, "您尚未设置“履约结束前买家提醒”的监控条件。将为您跳转到条件维护页面！", "YJTJ_Set.aspx");
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
            if (ds.Tables[0].Rows[0]["LYJSBJTXT"].ToString() != "" && ds.Tables[0].Rows[0]["LYJSBJTXT"].ToString() != "0")
            {
                spanTiaoJian.InnerText = ds.Tables[0].Rows[0]["LYJSBJTXT"].ToString();
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
        ht_where["search_tbname"] = " (select a.Number,Z_HTBH  as 合同编号,Z_SPBH as 商品编号,Z_SPMC as 商品名称,Z_GG as 规格,Z_ZBSL as 合同数量,(Z_ZBSL -isnull(Z_YTHSL,0)) as 未提货数量,ISNULL(e.未签收数量,'0') as 未签收数量,CONVERT(varchar(10),Z_DBSJ,120) as 合同开始时间,CONVERT(varchar(10),Z_HTJSRQ,120) as 合同结束时间,Y_YSYDDDLYX as 买家账号,c.I_JYFMC  as 买家名称,c.I_LXRXM as 买家联系人,c.I_LXRSJH as 买家联系方式,c.B_SFDJ as 买家冻结状态,T_YSTBDDLYX as 卖家账号,d.I_JYFMC as 卖家名称,d.I_LXRXM as 卖家联系人,d.I_LXRSJH as  卖家联系方式,isnull(b.SFCL,'否') as 是否已处理 from AAA_ZBDBXXB as a left join (select * from AAA_DZGHTHJKTZCLB where JKLX ='履约结束前买家提醒' and LYSJLX ='电子购货合同') as b on b.LYDH =a.Z_HTBH  left join AAA_DLZHXXB as c on c.B_DLYX=a.Y_YSYDDDLYX left join AAA_DLZHXXB as d on d.B_DLYX=a.T_YSTBDDLYX left join (select sum(T_THSL ) as 未签收数量,ZBDBXXBBH  as 编号 from AAA_THDYFHDXXB where F_BUYWYYSHCZSJ is null and F_BUYYYYSHCZSJ is null and F_BUYBFSHCZSJ is null and F_BUYQZXFHCZSJ is null group by ZBDBXXBBH) as e on e.编号 =a.Number where Z_HTZT ='定标' and Z_HTQX<>'即时' and convert(varchar(10),GETDATE(),120)>CONVERT(varchar(10), DATEADD (dd," + (-Convert.ToInt16(spanTiaoJian.InnerText)) + ",a.Z_HTJSRQ),120) and convert(varchar(10),GETDATE(),120)<=CONVERT(varchar(10),a.Z_HTJSRQ ,120)) as tab ";  //检索的表
        ht_where["search_mainid"] = " 合同编号 ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 and 未提货数量>0 ";  //检索条件  
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " 合同结束时间,合同编号 ";  //用于排序的字段        

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
        string sql = " select * from (select Z_HTBH  as 合同编号,Z_SPBH as 商品编号,Z_SPMC as 商品名称,Z_GG as 规格,Z_ZBSL as 合同数量,(Z_ZBSL -isnull(Z_YTHSL,0)) as 未提货数量,ISNULL(e.未签收数量,'0') as 未签收数量,CONVERT(varchar(10),Z_DBSJ,120) as 合同开始时间,CONVERT(varchar(10),Z_HTJSRQ,120) as 合同结束时间,c.B_SFDJ as 买家冻结状态,Y_YSYDDDLYX as 买家账号,c.I_JYFMC  as 买家名称,c.I_LXRXM as 买家联系人,c.I_LXRSJH as 买家联系方式,T_YSTBDDLYX as 卖家账号,d.I_JYFMC as 卖家名称,d.I_LXRXM as 卖家联系人,d.I_LXRSJH as  卖家联系方式,isnull(b.SFCL,'否') as 是否已处理 from AAA_ZBDBXXB as a left join (select * from AAA_DZGHTHJKTZCLB where JKLX ='履约结束前买家提醒' and LYSJLX ='电子购货合同') as b on b.LYDH =a.Z_HTBH  left join AAA_DLZHXXB as c on c.B_DLYX=a.Y_YSYDDDLYX left join AAA_DLZHXXB as d on d.B_DLYX=a.T_YSTBDDLYX left join (select sum(T_THSL ) as 未签收数量,ZBDBXXBBH  as 编号 from AAA_THDYFHDXXB where F_BUYWYYSHCZSJ is null and F_BUYYYYSHCZSJ is null and F_BUYBFSHCZSJ is null and F_BUYQZXFHCZSJ is null group by ZBDBXXBBH) as e on e.编号 =a.Number where Z_HTZT ='定标'  and Z_HTQX<>'即时' and convert(varchar(10),GETDATE(),120)>CONVERT(varchar(10), DATEADD (dd," + (-Convert.ToInt16(spanTiaoJian.InnerText)) + ",a.Z_HTJSRQ),120) and convert(varchar(10),GETDATE(),120)<=CONVERT(varchar(10),a.Z_HTJSRQ ,120)) as tab where " + ViewState["ht_where"].ToString() + " order by 合同结束时间,合同编号 ";
        DataSet ds = DbHelperSQL.Query(sql);

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(ds, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "履约结束前买家提醒", "履约结束前买家提醒", 25);
    }
    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "btnCL")
        {
            WorkFlowModule WFM = new WorkFlowModule("AAA_DZGHTHJKTZCLB");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            string sql = "INSERT INTO [AAA_DZGHTHJKTZCLB]([Number],[JKLX],[SFCL],[LYSJLX],[LYDH],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber + "' ,'履约结束前买家提醒','是','电子购货合同','" + e.CommandArgument.ToString() + "',1,'" + User.Identity.Name.ToString() + "','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";

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