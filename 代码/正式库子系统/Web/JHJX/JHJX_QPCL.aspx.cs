using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_JHJX_JHJX_QPCL : System.Web.UI.Page
{
    protected static  DataTable dt;
   
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

        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.ShowAlertAndBack(this, "查询超时，请重试或者修改查询条件！");
        }
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            /*---shiyan 2013-12-18 优化数据获取方式---*/
            DataSet ds_info = new DataSet();
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                ds_info = DbHelperSQL.Query("select SUM(T_DJHKJE) AS  争议金额,CAST(SUM(T_DJHKJE/ZBDJ) AS numeric(18,2)) AS 争议数量 from AAA_THDYFHDXXB where ZBDBXXBBH='" + NewDS.Tables[0].Rows[i]["主键"].ToString() + "' and F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货', '撤销','卖家主动退货')");
                if (ds_info != null && ds_info.Tables[0].Rows.Count > 0)
                {
                    NewDS.Tables[0].Rows[i]["争议金额"] = ds_info.Tables[0].Rows[0]["争议金额"].ToString();
                    NewDS.Tables[0].Rows[i]["争议数量"] = ds_info.Tables[0].Rows[0]["争议数量"].ToString();
                }
            }
            /*---shiyan结束---*/
            tdEmpty.Visible = false;
            dt = NewDS.Tables[0];
            rptSPXX.DataSource = dt.DefaultView;
        }
        else
        {
            tdEmpty.Visible = true;
            rptSPXX.DataSource = null;
        }
        rptSPXX.DataBind();
    }

    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = " (SELECT  AAA_ZBDBXXB.Number AS 主键 ,Z_QPZT AS 清盘状态,Z_QPKSSJ AS 清盘开始时间,Z_QPJSSJ AS 清盘结束时间, Z_HTBH AS 电子购货合同编号,Z_HTJSRQ AS 合同结束日期,Z_SPMC AS 商品名称,Z_SPBH AS 商品编号,Z_GG AS 规格,Z_ZBSL AS 合同数量,Z_ZBJG AS 单价,Z_ZBJE AS 合同金额,Z_LYBZJJE AS 履约保证金金额,  CAST( SUM( T_DJHKJE/ZBDJ) AS numeric(18,2)) AS 争议数量 ,SUM(T_DJHKJE) AS  争议金额,'人工清盘' AS 清盘类型,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END AS 清盘原因,T_YSTBDDLYX AS 卖方邮箱,Y_YSYDDDLYX AS 买方邮箱,Q_ZMSCFDLYX AS 证明上传方邮箱,Q_SFYQR AS 是否已确认,Q_ZMWJLJ AS 证明文件路径, Q_ZFLYZH AS 转付来源账户, Q_ZFMBZH AS 转付目标账户,Q_ZFJE AS 转付金额,Q_CLYJ AS 处理依据 ,Q_QRSJ AS 确认时间  FROM AAA_THDYFHDXXB JOIN AAA_ZBDBXXB ON AAA_ZBDBXXB.Number=AAA_THDYFHDXXB.ZBDBXXBBH WHERE   Z_QPZT IN( '清盘中','清盘结束')  AND F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货', '撤销','卖家主动退货')AND Q_SFYQR='是' GROUP BY  Z_HTBH,Z_HTJSRQ,Z_SPMC,Z_SPBH,Z_GG,Z_ZBSL,Z_ZBJG,Z_ZBJE,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END,AAA_ZBDBXXB.Number,Z_LYBZJJE,Z_QPZT,T_YSTBDDLYX ,Y_YSYDDDLYX,Z_QPKSSJ,Z_QPJSSJ,Q_ZMSCFDLYX,Q_SFYQR,Q_ZMWJLJ, Q_ZFLYZH , Q_ZFMBZH ,Q_ZFJE ,Q_CLYJ ,Q_QRSJ ) tab1 ";  //检索的表
        //ht_where["search_mainid"] = " 主键 ";  //所检索表的主键
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        //ht_where["search_paixu"] = " DESC ";  //排序方式
        //ht_where["search_paixuZD"] = " 确认时间 ";  //用于排序的字段

        /*---shiyan 2013-12-18 数据获取方式优化---*/
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";
        ht_where["serach_Row_str"] = "  Number AS 主键 ,Z_QPZT AS 清盘状态,Z_QPKSSJ AS 清盘开始时间,Z_QPJSSJ AS 清盘结束时间, Z_HTBH AS 电子购货合同编号,Z_HTJSRQ AS 合同结束日期,Z_SPMC AS 商品名称,Z_SPBH AS 商品编号,Z_GG AS 规格,Z_ZBSL AS 合同数量,Z_ZBJG AS 单价,Z_ZBJE AS 合同金额,Z_LYBZJJE AS 履约保证金金额, '' AS 争议数量 ,'' AS  争议金额,'人工清盘' AS 清盘类型,(CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END) AS 清盘原因,T_YSTBDDLYX AS 卖方邮箱,Y_YSYDDDLYX AS 买方邮箱,Q_ZMSCFDLYX AS 证明上传方邮箱,Q_SFYQR AS 是否已确认,Q_ZMWJLJ AS 证明文件路径, Q_ZFLYZH AS 转付来源账户, Q_ZFMBZH AS 转付目标账户,Q_ZFJE AS 转付金额,Q_CLYJ AS 处理依据 ,Q_QRSJ AS 确认时间 ";
        ht_where["search_tbname"] = " AAA_ZBDBXXB ";  //检索的表
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_str_where"] = " (Z_QPZT= '清盘中' or Z_QPZT='清盘结束')  AND  Q_SFYQR='是'  AND  exists (select 1 from AAA_THDYFHDXXB where ZBDBXXBBH=AAA_ZBDBXXB.Number and F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货', '撤销','卖家主动退货')) ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " Q_QRSJ ";  //用于排序的字段
        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();

        //HTwhere["search_str_where"] = HTwhere["search_str_where"] + "  and 清盘状态 like '%" + ddlqpzt.SelectedValue.ToString() + "%' and 商品名称 like '%" + txtspmc.Text.Trim() + "%' and 规格 like '%" + txtspgg.Text.Trim() + "%'  and 电子购货合同编号 like '%" + txthtbh.Text.Trim() + "%'";

        /*---shiyan 2013-12-18 优化数据获取方式---*/

        //商品名称
        if (txtspmc.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and Z_SPMC like '%" + txtspmc.Text.ToString().Trim() + "%' ";
        }
        if (txtspgg.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and Z_GG like '%" + txthtbh.Text.ToString().Trim() + "%' ";
        }
        //合同编号
        if (txthtbh.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and Z_HTBH like '%" + txthtbh.Text.ToString().Trim() + "%' ";
        }
        //发货单编号
        if (ddlqpzt.SelectedValue.ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and Z_QPZT='" + ddlqpzt.SelectedValue.ToString() + "' ";
        }       
        /*---shiyan 结束---*/

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    protected void rptSPXX_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        //LinkButton lb = (LinkButton)e.Item.FindControl("lsz");
        //if (e.CommandName == "linkbj")
        //{
        //    // Response.Redirect("JHJX_DLRSHXQ.aspx?JSBH=" + e.CommandArgument.ToString().Trim() + "&ck=OK");
        //    Response.Redirect("JHJX_SPBJ.aspx?ID=" + e.CommandArgument.ToString().Trim());
        //}
        //设置与无效
        if (e.CommandName == "lqp")
        {
            
           DataRow[] df = dt.Select("主键='" + e.CommandArgument.ToString().Trim() + "'");
            this.Context.Items.Add("Text", df);
            Server.Transfer("View_QPCL.aspx");

        }
    }

    protected void rptSPXX_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //Label qpzt = (Label)e.Item.FindControl("lbqpzt");
        //LinkButton link = (LinkButton)e.Item.FindControl("ledit");
        //if (qpzt.Text.Trim() == "清盘结束")
        //{
        //    link.Enabled = false; 
        //}

        Label lbgg = (Label)e.Item.FindControl("lbgg");
        Label lbspmc = (Label)e.Item.FindControl("lbspmc");   
        
        
        if (lbgg.Text.Length > 10)
            lbgg.Text = lbgg.Text.Substring(0, 10) + "...";
        if (lbspmc.Text.Length > 10)
            lbspmc.Text = lbspmc.Text.Substring(0, 10) + "...";
    }
}