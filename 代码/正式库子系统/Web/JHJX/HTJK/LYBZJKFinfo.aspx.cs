using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.Data;
using System.Collections;
using FMOP.DB;

public partial class Web_JHJX_HTJK_LYBZJKFinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);

        System.Globalization.CultureInfo ci = null;
        ci = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");
        System.Threading.Thread.CurrentThread.CurrentCulture = ci;

        if (!Page.IsPostBack)
        {
 

            if (Request["Number"] != null && Request["Number"].ToString() != "")
            {
                spanHTBH.InnerText = Request["Number"].ToString();
            }
            DisGrid();
        }
    }

    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.
        //ht_where["this_dblink"] = "不赋值或ERP";//这个可以不设置,默认是FMOP,即连接业务平台数据库。如需连接ERP，则需要设置为ERP.
        //ht_where["page_index"] = "0";//这个可以不设置,如果需要指定进入页面后显示第几页，才需要设置。必须是数字
        //ht_where["search_fyshow"] = "5";//这个也可以不设置，默认就是5,即页数可点击的按钮是11个。必须是数字，不能是0。
        //ht_where["count_float"] = "特殊";  //这个可以不用设置，特殊情况设置为特殊
        ht_where["page_size"] = "10"; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select a.Number, LSCSSJ, convert(varchar(10),LSCSSJ,120) as 扣罚时间 ,JE as 扣罚金额,ZY as 扣罚原因,c.Y_YSYDDDLYX as 赔偿买家账号,d.I_JYFMC as 赔偿买家名称,LYDH,b.ZBDBXXBBH,c.Z_HTBH as 合同编号 from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.Number left join AAA_ZBDBXXB as c on c.Number =b.ZBDBXXBBH left join AAA_DLZHXXB as d on d.B_DLYX=c.Y_YSYDDDLYX  where XM ='违约赔偿金' and (XZ='超过最迟发货日后录入发货信息' or XZ ='发货单生成后超过5日内未录入发票邮寄信息') and SJLX ='预' and LYYWLX ='AAA_THDYFHDXXB') as tabl ";  //检索的表
        ht_where["search_mainid"] = " Number ";  //所检索表的主键       
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " LYDH,LSCSSJ ";  //用于排序的字段
        return ht_where;
    }

    public void DisGrid()
    {
        //应用实例

        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 合同编号='" + spanHTBH.InnerText + "'";
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }
    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        //输出调试错误
        // Response.Write(ERRinfo);
        //Response.End();
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.ShowAlertAndBack(this, "查询超时，请重试或者修改查询条件！");
        }

        //RadGrid1.DataSource = null;

        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            tdEmpty.Visible = false;
            Repeater1.DataSource = NewDS.Tables[0].DefaultView;
        }
        else
        {
            tdEmpty.Visible = true;
        }
        Repeater1.DataBind();
    }
}