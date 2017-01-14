using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Hesion.Brick.Core;
using System.Configuration;

public partial class Web_JHJX_New2013_JYGLBJK_KFCSJK : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            DisGrid();
        }


    }


    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    protected string[] GetSql()
    {
        string[] sqlarry = new string[]{"",""};       

        string StartTime = txtBeginTime.Text.Trim();
        string EndTime = txtEndTime.Text.Trim();
        string cs = txtcs.Text.Trim();

        if ((StartTime == "" && EndTime == "") || cs=="")
        {
            return sqlarry;
        }
        else
        {
            string str = "";
            if (StartTime != "" && EndTime == "")
            {
                str = " and AAA_ZKLSMXB.CreateTime>= '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' ";
            }
            else if (StartTime == "" && EndTime != "")
            {
                str = " and AAA_ZKLSMXB.CreateTime <= '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }
            else if (StartTime != "" && EndTime != "")
            {
                str = " and AAA_ZKLSMXB.CreateTime between '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            }
            string strsql1 = "select B_DLYX as 邮箱, I_JYFMC as 账户名称,REPLACE(AAA_ZKLSMXB.JSZHLX,'交易账户','') as 账户类型,I_ZCLB as 注册类别,I_LXRXM as 联系人,I_LXRSJH as 联系方式,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX=AAA_MJMJJYZHYJJRZHGLB.GLJJRDLZH) as 所属分公司 ,(select I_JYFMC from AAA_DLZHXXB where B_DLYX=AAA_MJMJJYZHYJJRZHGLB.GLJJRDLZH) as 关联经纪人, count(*)as 罚款次数, sum(JE) as 金额 from  AAA_ZKLSMXB join AAA_DLZHXXB on AAA_ZKLSMXB.DLYX=B_DLYX left join AAA_MJMJJYZHYJJRZHGLB on B_DLYX=AAA_MJMJJYZHYJJRZHGLB.DLYX where XM='违约赔偿金' and SJLX='实' and XZ not in('其他违约责任扣罚','卖家中标后未定标扣罚') and SFDQMRJJR='是' "+str+" group by I_JYFMC ,AAA_ZKLSMXB.JSZHLX ,I_ZCLB ,I_LXRXM,I_LXRSJH, AAA_MJMJJYZHYJJRZHGLB.GLJJRDLZH,B_DLYX having   count(*)>"+cs;

            string strsql2 = "select  ROW_NUMBER() over(order by AAA_ZKLSMXB.CreateTime) as 序号, AAA_ZKLSMXB.Number as 编号, B_DLYX as 交易方账号, I_JYFMC as 交易方名称,(select I_PTGLJG from AAA_DLZHXXB where B_DLYX=AAA_MJMJJYZHYJJRZHGLB.GLJJRDLZH) as 分公司名称 ,AAA_ZKLSMXB.CreateTime as 时间,XM as 项目,ZY as 摘要,XZ as 性质, JE as 金额 from  AAA_ZKLSMXB join AAA_DLZHXXB on AAA_ZKLSMXB.DLYX=B_DLYX left join AAA_MJMJJYZHYJJRZHGLB on B_DLYX=AAA_MJMJJYZHYJJRZHGLB.DLYX where   XM='违约赔偿金' and SJLX='实' and XZ not in('其他违约责任扣罚','卖家中标后未定标扣罚') and SFDQMRJJR='是' " + str;


            sqlarry[0] = strsql1;
            sqlarry[1] = strsql2;
            return sqlarry;
 
        }
       
    }

    protected void btnDC_Click(object sender, EventArgs e)
    {
        string sql = GetSql()[0];
        
        DataSet dataSet = DbHelperSQL.Query(sql);

        if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
        {
            //dtable.Columns.Remove("Number");

            // dtable.Columns["MB001"].ColumnName = "品号";   
            //dtable.Columns["批号"].SetOrdinal(4);
            // DataTableToExcel(dtable);

            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "扣罚次数检测表", "扣罚次数检测表", 15);
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "没有可以导出的数据，请重新进行查询！");

        }

    }


    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = "(" + GetSql()[0] + ") as tab";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " 邮箱 ";  //所检索表的主键
        ht_where["search_paixu"] = " asc ";  //排序方式
        ht_where["search_paixuZD"] = " 账户名称 ";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_HWSF/FHD.aspx";
        return ht_where;
    }
    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {

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
            ts.Visible = true;
        }
        Repeater1.DataBind();

    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();


        //string sql_where = " and  金额 > " + txtje.Text.ToString().Trim()  ;

        //HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "CK")
        {
            string str = GetSql()[1] + "and B_DLYX='" + e.CommandArgument.ToString().Trim() + "'  order by AAA_ZKLSMXB.CreateTime";

            DataTable df = DbHelperSQL.Query(str).Tables[0];
            this.Context.Items.Add("Text", df);
            Server.Transfer("View_KFCS.aspx");

        }
    }
}
   