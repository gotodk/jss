using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_JHJX_New2013_UCFWJG_GXTWDetail : System.Web.UI.Page
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
                NewDS.Tables[0].Rows[i]["编号"] = Convert.ToInt32(ht_whereTablePage["page_index"]) * Convert.ToInt32(ht_whereTablePage["page_size"]) + i + 1;

            }
            Repeater1.DataSource = NewDS.Tables[0].DefaultView;
            this.tdEmpty.Visible = false;
        }
        else
        {
            Repeater1.DataSource = DbHelperSQL.Query("select Number,'' '编号',GLBMMC '高校名称',CreateTime '创建时间' from AAA_PTGLJGB where SFYX='是' and GLBMFLMC='高校团委' and 1!=1").Tables[0].DefaultView;
            this.tdEmpty.Visible = true;
        }
        Repeater1.DataBind();
    }

    public void DisGrid()
    {
        //应用实例

        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        string strXQNF = ""; //需求年份
        string strXQYF = "";//需求月份
        //得到年份
        //strXQNF = this.ddXQNF.SelectedValue.ToString();
        //得到月份
        //strXQYF = this.ddXQYF.SelectedValue.ToString();
        //HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and ssbsc like '%" + ddrFGSMC.SelectedValue.ToString() + "%' and khbh like '%" + txtKHBH.Text+ "%' ";

        HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and 高校名称 like '%" + this.txtKHBH.Text.Trim() + "%'  ";
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
        ht_where["search_tbname"] = " (select Number,'' '编号',GLBMMC '高校名称',CreateTime '创建时间' from AAA_PTGLJGB where SFYX='是' and GLBMFLMC='高校团委') as tabl ";  //检索的表
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        //ht_where["search_str_where"] = " 1=1 and SFYSX='是' ";  //检索条件
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_paixu"] = " desc ";  //排序方式
        ht_where["search_paixuZD"] = " 创建时间 ";  //用于排序的字段
        return ht_where;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
}