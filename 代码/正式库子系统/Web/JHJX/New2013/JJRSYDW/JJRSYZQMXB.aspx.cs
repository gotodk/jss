using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using FMOP.Common;
using System.Collections;
using System.Data;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_JHJX_New2013_JJRSYDW_JJRSYZQMXB : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            UCFWJGDetail1.initdefault();
            //setchengshi();
            DisGrid();
            
        }
        ZW();
    }
    //根据登陆人账号,判断是否有审核权限；审核后，"验证"、"审核"功能不可用
    protected void ZW()
    {
        try
        {
            //string strGetZW = "select GWSMSMC from hr_employees where YGZT !='离职' and YGZT !='无' and Number='" + User.Identity.Name.ToString().Trim() + "'";
            string strGetZW = "select BM from hr_employees where YGZT !='离职' and YGZT !='无' and Number='" + User.Identity.Name.ToString().Trim() + "'";
            object zw = DbHelperSQL.GetSingle(strGetZW);
            if (zw != null)
            {
                if (zw.ToString() == "财务中心" || User.Identity.Name.ToString().Trim() == "admin")
                {
                    for (int i = 0; i < Repeater1.Items.Count; i++)
                    {
                        HiddenField hidNumber = (HiddenField)Repeater1.Items[i].FindControl("hidNumber");
                        string strSFSH = "select SHZT from AAA_DWJJRSYZQMXB where Number='" +
                            hidNumber.Value.ToString() + "'";
                        object sfsh = DbHelperSQL.GetSingle(strSFSH);
                        if (sfsh.ToString() == "待审核")
                        {
                            Button btnSH = (Button)Repeater1.Items[i].FindControl("btnSH");
                            btnSH.Enabled = true;
                        }
                        else
                        {
                            Button btnSH = (Button)Repeater1.Items[i].FindControl("btnSH");
                            btnSH.Enabled = false;
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < Repeater1.Items.Count; i++)
                    {
                        Button btnSH = (Button)Repeater1.Items[i].FindControl("btnSH");
                        btnSH.Enabled = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Repeater1.Items.Count; i++)
                {
                    Button btnSH = (Button)Repeater1.Items[i].FindControl("btnSH");
                    btnSH.Enabled = false;
                }
            }

        }
        catch (Exception e)
        {
            MessageBox.Show(this, e.ToString());
        }
    }

    #region//作废
    //// 设定所属办事处下拉框内容
    //protected void setchengshi()
    //{
    //    //string MainERP = DbHelperSQL.GetSingle("SELECT DYERP FROM SYSTEM_CITY_0 WHERE NAME='公司总部'").ToString();//总部对应的ERP名称
    //    //DataSet ds = DbHelperSQL.Query("SELECT DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处' and DYERP <>'" + MainERP + "'");
    //    DataSet ds = DbHelperSQL.Query("SELECT DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处'");
    //    city.DataSource = ds;
    //    city.DataTextField = "DYFGSMC";
    //    city.DataValueField = "DYFGSMC";
    //    city.DataBind();
    //    city.Items.Insert(0, new ListItem("全部分公司", ""));
    //    string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
    //    string bm = DbHelperSQL.GetSingle(sql).ToString();
    //    if (bm.IndexOf("办事处") > 0)
    //    {
    //        sql = "select count(name) from system_city where name='" + bm + "'";
    //        //bm = bm.Replace("办事处", "分公司");
    //        if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
    //        {
    //            string strfgs = "select DYFGSMC FROM System_City where Name='" + bm + "'";
    //            string fgs = DbHelperSQL.GetSingle(strfgs).ToString();
    //            city.DataSource = null;
    //            city.DataBind();
    //            city.Items.Clear();
    //            city.Items.Add(fgs);
    //        }
    //    }
    //}
    #endregion

    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " ( select a.*,isnull(SYZJE-YZQJE,0.00) 缺票收益金额,b.I_DSFCGZT,b.I_PTGLJG 平台管理机构,b.I_YWGLBMFL 业务管理部门分类,'发票收取时间'= convert(char,SQFPRQ,111) from AAA_DWJJRSYZQMXB a left join AAA_DLZHXXB b on a.JJRBH=b.J_JJRJSBH ) as tab ";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_paixu"] = " desc ";  //排序方式
        ht_where["search_paixuZD"] = " CreateTime";  //用于排序的字段
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
            //Repeater2.DataSource = NewDS.Tables[0].DefaultView;
            ts.Visible = false;
            //exprestTD.Visible = false;
        }
        else
        {
            DataSet ds = DbHelperSQL.Query("select *,'缺票收益金额'='','I_DSFCGZT'='','平台管理机构'='','业务管理部门分类'='','发票收取时间'='' from AAA_DWJJRSYZQMXB where 1!=1");
            Repeater1.DataSource = ds.Tables[0].DefaultView;
            Repeater2.DataSource = ds.Tables[0].DefaultView;
            ts.Visible = true;
            exprestTD.Visible = true;

        }
        Repeater1.DataBind();
        Repeater2.DataBind();
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();


        string sql_where = " and 业务管理部门分类 like '%" + UCFWJGDetail1.Value[0].ToString() + "%' and 平台管理机构 like '%" + UCFWJGDetail1.Value[1].ToString() + "%' and SHZT like '%" + drpSFSH.SelectedValue.Trim() + "%' and JJRBH like '%" + txtJJRBH.Text.Trim() + "%' and JJRMC like '%" + txtJJRMC.Text.Trim() + "%'";

        string StartTime = txtBeginTime.Text.Trim();
        string EndTime = txtEndTime.Text.Trim();
        if (StartTime != "" && EndTime == "")
        {
            sql_where = sql_where + " and SHSJ >= '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (StartTime == "" && EndTime != "")
        {
            sql_where = sql_where + " and  SHSJ <= '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        else if (StartTime != "" && EndTime != "")
        {
            sql_where = sql_where + " and SHSJ between '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }       

        HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandArgument == "CK")
        {
            string url = "JJRSYFPXXLRCKAndSH.aspx?number=" + e.CommandName;
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.open('" + url + "');</script>");
        }
        else if (e.CommandArgument == "XG")
        {
            string strSQL = "select SHZT from AAA_DWJJRSYZQMXB where Number='" + e.CommandName + "'";
            object shzt = DbHelperSQL.GetSingle(strSQL);
            if (shzt.ToString() == "已审核")
            {
                this.ClientScript.RegisterClientScriptBlock(GetType(), "message", "<script>window.top.Dialog.alert('单号为：" + e.CommandName + "的数据已审核，不能进行修改！');</script>");
                return;
            }
            Response.Redirect("JJRSYFPXXLR.aspx?number=" + e.CommandName);
        }
        else if (e.CommandArgument == "SH")
        {
            string strSQL = "select SHZT from AAA_DWJJRSYZQMXB where Number='" + e.CommandName.ToString() + "'";
            DataSet ds = DbHelperSQL.Query(strSQL);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["SHZT"].ToString() == "待审核")
                {
                    Response.Redirect("JJRSYFPXXLRCKAndSH.aspx?sh=ok&number=" + e.CommandName.ToString());
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.alert('单号为：" + e.CommandName.ToString() + "的数据已审核，不能进行重复操作！');</script>");
                    return;
                }

            }
        }
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void btnDC_Click(object sender, EventArgs e)
    {
        string sql_where = "select a.*,isnull(SYZJE-YZQJE,0.00) 缺票收益金额,b.I_DSFCGZT,b.I_PTGLJG 平台管理机构,b.I_YWGLBMFL 业务管理部门分类,'发票收取时间'= convert(char,SQFPRQ,111) from AAA_DWJJRSYZQMXB a left join AAA_DLZHXXB b on a.JJRBH=b.J_JJRJSBH where b.I_YWGLBMFL like '%" + UCFWJGDetail1.Value[0].ToString() + "%' and b.I_PTGLJG like '%" + UCFWJGDetail1.Value[1].ToString() + "%' and SHZT like '%" + drpSFSH.SelectedValue.Trim() + "%' and JJRBH like '%" + txtJJRBH.Text.Trim() + "%' and JJRMC like '%" + txtJJRMC.Text.Trim() + "%'";
        
        string StartTime = txtBeginTime.Text.Trim();
        string EndTime = txtEndTime.Text.Trim();
        if (StartTime != "" && EndTime == "")
        {
            sql_where += sql_where + " and SHSJ >= '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' ";
        }
        else if (StartTime == "" && EndTime != "")
        {
            sql_where += sql_where + " and  SHSJ <= '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        else if (StartTime != "" && EndTime != "")
        {
            sql_where += sql_where + " and SHSJ between '" + Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(EndTime).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
        }
        sql_where += " order by CreateTime desc";
        DataSet ds = DbHelperSQL.Query(sql_where);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Repeater2.DataSource = ds.Tables[0].DefaultView;
            exprestTD.Visible = false;
        }
        else
        {
            Repeater2.DataSource = null;
            exprestTD.Visible = true;
        }
        Repeater2.DataBind();
        DisGrid();
        ToExcel(export);
    }
    // 将数据导出到excel，与下面的函数同时使用才能正常工作
    public void ToExcel(System.Web.UI.Control ctl)
    {
        HttpContext.Current.Response.Clear();

        HttpContext.Current.Response.Charset = "";
        string filename = "JYPT_JJRSYDW" + System.DateTime.Now.ToString("_yyyyMMddHHmm");
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" +

        System.Web.HttpUtility.UrlEncode(filename, System.Text.Encoding.GetEncoding("GB2312")) + ".xls");

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword   

        ctl.Page.EnableViewState = false;
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        #region//asp.net 导出Excel时，解决纯数字字符串变成类似这样的 2.00908E+18 形式的代码
        string strStyle = "<style>td{vnd.ms-excel.numberformat: @;}</style>";//导出的数字的样式
        tw.WriteLine(strStyle);
        #endregion

        ctl.RenderControl(hw);
        HttpContext.Current.Response.Write(tw.ToString());
        HttpContext.Current.Response.End();
    }
    // 导出Exel时需重载此方法
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
}