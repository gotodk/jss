using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_New2013_JHJX_JHJX_ZJZZ_JHJX_JJRSYFSJL : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            this.UCFWJGDetail1.initdefault();
            DisGrid();
            //设置项目和性质
            setXMXZ();
        }
    }

  
    /// <summary>
    /// 设置项目和性质
    /// </summary>
    protected void setXMXZ()
    {
        DataSet ds = DbHelperSQL.Query("select XM, XZ as XZvalue,(case when LEN (XZ)>10 then SUBSTRING (xz,0,11)+'...' else XZ end) as XZtext from AAA_moneyDZB where 1=1  and dymkbh='9' and   sjlx='预' order by dymkbh");
        ViewState["XMXZ"] = ds;
        setXM(ds.Tables[0]);

        setXZ(ds.Tables[0]);

    }

    public void setXM(DataTable dt)
    {
        DataTable dtXMDistinct = dt.DefaultView.ToTable(true, new string[] { "XM" });
        ddXM.DataSource = dtXMDistinct.DefaultView;
        ddXM.DataTextField = "XM";
        ddXM.DataValueField = "XM";
        ddXM.DataBind();
        ddXM.Items.Insert(0, new ListItem("请选择项目", ""));
    }

    public void setXZ(DataTable dt)
    {
        ddXZ.DataSource = dt.DefaultView;
        ddXZ.DataTextField = "XZtext";
        ddXZ.DataValueField = "XZvalue";
        ddXZ.DataBind();
        ddXZ.Items.Insert(0, new ListItem("请选择性质", ""));
    }

    /// <summary>
    /// 性质改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddXZ_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ddXZ.SelectedItem.Text == "请选择性质")
        {
            setXMXZ();
        }
        else
        {
            ddXM.Items.Clear();
            ddXM.Items.Add(new ListItem("请选择项目", ""));
            DataSet dataSet = (DataSet)ViewState["XMXZ"];
            DataTable dtXMXZ = dataSet.Tables[0];
            foreach (DataRow dr in dtXMXZ.Select("XZvalue='" + ddXZ.SelectedValue.ToString() + "'"))
            {

                ddXM.Items.Add(new ListItem(dr["XM"].ToString(), dr["XM"].ToString()));
            }
            if (ddXM.Items.Count >= 2)
            {
                ddXM.SelectedIndex = 1;
            }
            else
            {
                ddXM.SelectedIndex = 0;
            }
        }
    }
    /// <summary>
    /// 项目改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddXM_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ddXM.SelectedItem.Text == "请选择项目")
        {
            setXMXZ();
        }
        else
        {
            ddXZ.Items.Clear();
            ddXZ.Items.Add(new ListItem("请选择性质", ""));
            DataSet dataSet = (DataSet)ViewState["XMXZ"];
            DataTable dtXMXZ = dataSet.Tables[0];
            foreach (DataRow dr in dtXMXZ.Select("XM='" + ddXM.SelectedValue.ToString() + "'"))
            {

                ddXZ.Items.Add(new ListItem(dr["XZtext"].ToString(), dr["XZvalue"].ToString()));
            }
            if (ddXZ.Items.Count == 2)
            {
                ddXZ.SelectedIndex = 1;
            }
            else
            {
                ddXZ.SelectedIndex = 0;
            }

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
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('服务器超时，请稍后重试！',function(){ window.location.href=window.location.href;});</script>");
        }
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {

            rptZJYEBDMX.DataSource = NewDS.Tables[0].DefaultView;
            this.tdEmpty.Visible = false;
        }
        else
        {
           // rptZJYEBDMX.DataSource = DbHelperSQL.Query("select a.Number,a.LSCSSJ as 变动时间,DDZH.I_JYFMC '交易方名称',DDZH.B_DLYX '交易方账号',DDZH.I_YWGLBMFL '业务管理部门分类',DDZH.I_PTGLJG '分公司名称',a.ZY as 摘要,a.XM as 项目,a.XZ as 性质,a.JE as 金额 from   AAA_ZKLSMXB as a inner join AAA_moneyDZB as b on a.xm=b.xm and a.xz=b.xz  and a.sjlx=b.sjlx  inner join AAA_DLZHXXB as DDZH on a.DLYX=DDZH.B_DLYX where  a.sjlx='预' and isnull(b.dymkbh,'')='9' and 1!=1");
            rptZJYEBDMX.DataSource = null;
            this.tdEmpty.Visible = true;
        }
        rptZJYEBDMX.DataBind();
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = " (select a.Number,a.LSCSSJ as 变动时间,DDZH.I_JYFMC '交易方名称',DDZH.B_DLYX '交易方账号',DDZH.I_YWGLBMFL '业务管理部门分类',DDZH.I_PTGLJG '分公司名称',a.ZY as 摘要,a.XM as 项目,a.XZ as 性质,a.JE as 金额 from   AAA_ZKLSMXB as a inner join AAA_moneyDZB as b on a.xm=b.xm and a.xz=b.xz  and a.sjlx=b.sjlx  inner join AAA_DLZHXXB as DDZH on a.DLYX=DDZH.B_DLYX where  a.sjlx='预' and isnull(b.dymkbh,'')='9' ) as tab";  //检索的表
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是  
        ht_where["serach_Row_str"] = " a.Number,a.LSCSSJ as 变动时间,DDZH.I_JYFMC '交易方名称',DDZH.B_DLYX '交易方账号',DDZH.I_YWGLBMFL '业务管理部门分类',DDZH.I_PTGLJG '分公司名称',a.ZY as 摘要,a.XM as 项目,a.XZ as 性质,a.JE as 金额 ";
        ht_where["search_tbname"] = " AAA_ZKLSMXB as a inner join AAA_moneyDZB as b on a.xm=b.xm and a.xz=b.xz  and a.sjlx=b.sjlx  inner join AAA_DLZHXXB as DDZH on a.DLYX=DDZH.B_DLYX ";  //检索的表

        ht_where["search_mainid"] = " a.Number ";  //所检索表的主键
        ht_where["search_str_where"] = " a.sjlx='预' and isnull(b.dymkbh,'')='9' ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = "a.LSCSSJ DESC,a.Number  ";  //用于排序的字段
        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        //string sql_where = " and 业务管理部门分类 like '%" + this.UCFWJGDetail1.Value[0].ToString() + "%' and 分公司名称 like '%" + this.UCFWJGDetail1.Value[1].ToString() + "%' and 交易方名称 like '%" + this.txtJYFMC.Text.Trim() + "%' and 交易方账号 like '%" + this.txtJYFZH.Text.Trim() + "%' and 项目 like '%" + ddXM.SelectedValue.ToString() + "%' and  性质 like '%" + this.ddXZ.SelectedValue.ToString() + "%' ";

        //string StartTime = txtBeginTime.Text.Trim();
        //string EndTime = txtEndTime.Text.Trim();
        //if (StartTime != "" && EndTime == "")
        //{
        //    sql_where = sql_where + " and 变动时间 >= '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' ";
        //}
        //else if (StartTime == "" && EndTime != "")
        //{
        //    sql_where = sql_where + " and  变动时间 <= '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        //}
        //else if (StartTime != "" && EndTime != "")
        //{
        //    sql_where = sql_where + " and 变动时间 between '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        //}

        /*---shiyan 2013-12-26 数据获取优化---*/
        string sql_where = "";
        if (UCFWJGDetail1.Value[0].ToString() != "")
        {
            sql_where = sql_where + " and DDZH.I_YWGLBMFL='" + this.UCFWJGDetail1.Value[0].ToString() + "'  ";
        }
        if (UCFWJGDetail1.Value[1].ToString() != "")
        {
            sql_where = sql_where + " and DDZH.I_PTGLJG='" + this.UCFWJGDetail1.Value[1].ToString() + "'  ";
        }
        if (txtJYFMC.Text.Trim() != "")
        {
            sql_where = sql_where + " and DDZH.I_JYFMC like '%" + this.txtJYFMC.Text.Trim() + "%' ";
        }
        if (txtJYFZH.Text.Trim() != "")
        {
            sql_where = sql_where + " and a.DLYX like '%" + this.txtJYFZH.Text.Trim() + "%' ";
        }
        if (ddXM.SelectedValue.ToString() != "")
        {
            sql_where = sql_where + " and a.XM='" + ddXM.SelectedValue.ToString() + "' ";
        }
        if (ddXZ.SelectedValue.ToString() != "")
        {
            sql_where = sql_where + " and a.XZ='" + ddXZ.SelectedValue.ToString() + "' ";
        }

        string StartTime = txtBeginTime.Text.Trim();
        string EndTime = txtEndTime.Text.Trim();
        if (StartTime != "")
        {
            sql_where = sql_where + " and convert(varchar(10),a.LSCSSJ,120) >= '" + StartTime + "' ";
        }
        if (EndTime != "")
        {
            sql_where = sql_where + " and  convert(varchar(10),a.LSCSSJ,120) <= '" + EndTime + "' ";
        }
        /*---shiyan 结束---*/
        HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;

        ViewState["ht_where"] = HTwhere["search_str_where"];
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }


    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();
    }

    /// <summary>
    /// 导出到Excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        //string sql_where = " and 业务管理部门分类 like '%" + this.UCFWJGDetail1.Value[0].ToString() + "%' and 分公司名称 like '%" + this.UCFWJGDetail1.Value[1].ToString() + "%' and 交易方名称 like '%" + this.txtJYFMC.Text.Trim() + "%' and 交易方账号 like '%" + this.txtJYFZH.Text.Trim() + "%' and 项目 like '%" + ddXM.SelectedValue.ToString() + "%' and  性质 like '%" + this.ddXZ.SelectedValue.ToString() + "%' ";

        //string StartTime = txtBeginTime.Text.Trim();
        //string EndTime = txtEndTime.Text.Trim();
        //if (StartTime != "" && EndTime == "")
        //{
        //    sql_where = sql_where + " and 变动时间 >= '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' ";
        //}
        //else if (StartTime == "" && EndTime != "")
        //{
        //    sql_where = sql_where + " and  变动时间 <= '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        //}
        //else if (StartTime != "" && EndTime != "")
        //{
        //    sql_where = sql_where + " and 变动时间 between '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        //}

        //string sql1 = "select * from (select a.Number,a.LSCSSJ as 变动时间,DDZH.I_JYFMC '交易方名称',DDZH.B_DLYX '交易方账号',DDZH.I_YWGLBMFL '业务管理部门分类',DDZH.I_PTGLJG '分公司名称',a.ZY as 摘要,a.XM as 项目,a.XZ as 性质,a.JE as 金额 from   AAA_ZKLSMXB as a inner join AAA_moneyDZB as b on a.xm=b.xm and a.xz=b.xz  and a.sjlx=b.sjlx  inner join AAA_DLZHXXB as DDZH on a.DLYX=DDZH.B_DLYX where  a.sjlx='预' and isnull(b.dymkbh,'')='9' ) as tab where 1=1 " + sql_where + " order by 变动时间 DESC,Number desc";

        string sql1 = "select a.Number,a.LSCSSJ as 变动时间,DDZH.I_JYFMC '交易方名称',DDZH.B_DLYX '交易方账号',DDZH.I_YWGLBMFL '业务管理部门分类',DDZH.I_PTGLJG '分公司名称',a.ZY as 摘要,a.XM as 项目,a.XZ as 性质,a.JE as 金额 from   AAA_ZKLSMXB as a inner join AAA_moneyDZB as b on a.xm=b.xm and a.xz=b.xz  and a.sjlx=b.sjlx  inner join AAA_DLZHXXB as DDZH on a.DLYX=DDZH.B_DLYX where" + ViewState["ht_where"].ToString() + " order by a.LSCSSJ desc,a.number desc";
        DataSet NewDS = DbHelperSQL.Query(sql1);

        NewDS.Tables[0].Columns.Remove("Number");
        NewDS.Tables[0].Columns.Remove("业务管理部门分类");

        MyXlsClass MXC = new MyXlsClass();
        MXC.goxls(NewDS, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "经纪人收益发生记录", "经纪人收益发生记录", 15);
    }
}