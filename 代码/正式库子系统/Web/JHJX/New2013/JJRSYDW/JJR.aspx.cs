using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using FMOP.DB;
using System.Data;
using System.Collections;
using Hesion.Brick.Core;

public partial class Web_JHJX_New2013_JJRSYDW_JJR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);

        System.Globalization.CultureInfo ci = null;
        ci = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");
        System.Threading.Thread.CurrentThread.CurrentCulture = ci;

        if (!Page.IsPostBack)
        {
 
            DisGrid();
        }      
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
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                //得到编号
                Hashtable ht_whereTablePage = this.commonpagernew1.HTwhere;
                //NewDS.Tables[0].Rows[i]["ROWID"] = Convert.ToInt32(ht_whereTablePage["page_index"]) * Convert.ToInt32(ht_whereTablePage["page_size"]) + i + 1;

            }
            Repeater1.DataSource = NewDS.Tables[0].DefaultView;
        }
        else
        {
            Repeater1.DataSource = DbHelperSQL.Query("select J_JJRJSBH 经纪人编号,I_JYFMC 经纪人名称,I_PTGLJG 所属分公司,'总收益'='','已支取收益'='','缺票收益金额'='',I_DSFCGZT 第三方存管状态 from AAA_DLZHXXB where 1!=1").Tables[0].DefaultView;
        }
        Repeater1.DataBind();
    }

    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();

        HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 经纪人编号 like '%" + txtJJRBH.Text.ToString().Trim() + "%' and 经纪人名称 like '%" + txtJJRMC.Text.ToString().Trim() + "%' ";
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

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
        ht_where["search_tbname"] = " (select J_JJRJSBH 经纪人编号,I_JYFMC 经纪人名称,I_PTGLJG 所属分公司,'总收益'=(select isnull(sum(JE),0.00) from AAA_ZKLSMXB where JSBH=a.J_JJRJSBH and XM='经纪人收益' and (XZ='来自买方收益' or XZ='来自卖方收益') and SJLX='预' ),'已支取收益'=(select isnull(sum(JE),0.00) from AAA_ZKLSMXB where JSBH=a.J_JJRJSBH and XM='经纪人收益' and XZ='发票核准后收益' and SJLX='实'),'缺票收益金额'=isnull((select isnull(sum(JE),0.00) from AAA_ZKLSMXB where JSBH=a.J_JJRJSBH and XM='经纪人收益' and (XZ='来自买方收益' or XZ='来自卖方收益') and SJLX='预' )-(select isnull(sum(JE),0.00) from AAA_ZKLSMXB where JSBH=a.J_JJRJSBH and XM='经纪人收益' and XZ='发票核准后收益' and SJLX='实'),0.00),I_DSFCGZT 第三方存管状态 from AAA_DLZHXXB a where B_JSZHLX='经纪人交易账户' and I_ZCLB='单位') as tabl ";  //检索的表
        ht_where["search_mainid"] = " 经纪人编号 ";  //所检索表的主键
        //ht_where["search_str_where"] = " 1=1 and SFYSX='是' ";  //检索条件
        ht_where["search_str_where"] = " 1=1 and 总收益>0 and 缺票收益金额<>0";  //检索条件
        ht_where["search_paixu"] = " desc ";  //排序方式
        ht_where["search_paixuZD"] = " 经纪人编号 ";  //用于排序的字段
        return ht_where;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
}