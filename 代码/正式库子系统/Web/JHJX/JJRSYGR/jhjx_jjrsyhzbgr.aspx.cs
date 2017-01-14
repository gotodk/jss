using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;

using System.Collections;
using System.Data;
using Hesion.Brick.Core;
using FMOP.DB;
using System.Text;

public partial class Web_JHJX_JJRSYGR_jhjx_jjrsyhzbgr : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
 

           // setchengshi();
            this.UCFWJGDetail1.initdefault();
            DisGrid();
        }




    }

    /*
    protected void setchengshi()
    {
        string MainERP = DbHelperSQL.GetSingle("SELECT DYERP FROM SYSTEM_CITY_0 WHERE NAME='公司总部'").ToString();//总部对应的ERP名称
        DataSet ds = DbHelperSQL.Query("SELECT DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处' ");
        ddlssfgs.DataSource = ds.Tables[0].DefaultView;
        ddlssfgs.DataTextField = "DYFGSMC";
        ddlssfgs.DataValueField = "DYFGSMC";
        ddlssfgs.DataBind();
        ddlssfgs.Items.Insert(0, new ListItem("全部分公司", ""));
        string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
        string bm = DbHelperSQL.GetSingle(sql).ToString();
        if (bm.IndexOf("办事处") > 0)
        {
            sql = "select count(name) from system_city where name='" + bm + "'";
            string fgsSql = "select DYFGSMC from system_city where name='" + bm + "'";
            if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
            {
                string strFGSMC = DbHelperSQL.GetSingle(fgsSql).ToString();
                ddlssfgs.DataSource = null;
                ddlssfgs.DataBind();
                ddlssfgs.Items.Clear();
                ddlssfgs.Items.Add(strFGSMC);

            }
        }
    }
    */
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        DataTable dt = new DataTable();
        if (ERRinfo.IndexOf("超时") >= 0)
        {
            MessageBox.ShowAlertAndBack(this, "查询超时，请重试或者修改查询条件！");
        }
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            tdEmpty.Visible = false;
            dt = NewDS.Tables[0];

            rptSPXX.DataSource = dt.DefaultView;
        }
        else { tdEmpty.Visible = true; }
        rptSPXX.DataBind();
    }

    //设置初始默认值检索
    private Hashtable SetV()
    {

        
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " (SYKSNF+'年'+SYKSYF+'月') as 收益时间,* ";
        ht_where["search_tbname"] = " (select  b.I_PTGLJG as 业务管理部门,JJRJSBH as '经纪人编号',b.I_JYFMC as '经纪人名称',  isnull(KSSYJE,0.00) as '总收益金额',SKJE as '扣税金额', SHJE as '可支取收益',SYKSNF, SYKSYF,b.I_YWGLBMFL as 业务管理部门分类 from AAA_JJRSYKSMXB as a  left join AAA_DLZHXXB as b on a.JJRJSBH = b.J_JJRJSBH where SFYJJSWC='已计算') as tab1 ";  //检索的表
        ht_where["search_mainid"] = "tab1.经纪人编号 ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = "tab1.SYKSNF,tab1.SYKSYF";  //用于排序的字段
        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
       
        

        Hashtable HTwhere = SetV();

        if ((this.ddlnf.SelectedValue == "无" && this.ddlyf.SelectedValue == "无") || (this.ddlnf.SelectedValue == "" && this.ddlyf.SelectedValue == ""))
        {
            DateTime dt = DateTime.Now;
            HTwhere["search_str_where"] += " and SYKSNF  =" + dt.Year + " and SYKSYF = " + dt.AddMonths(-1).Month.ToString("00");
            this.ddlnf.SelectedValue = dt.Year.ToString();
            this.ddlyf.SelectedValue = dt.AddMonths(-1).Month.ToString("00");
        }

        if ( this.ddlnf.SelectedValue != "无" && this.ddlyf.SelectedValue != "无")
        {
            HTwhere["search_str_where"] += " and SYKSNF  =" + this.ddlnf.SelectedValue + " and SYKSYF = " + this.ddlyf.SelectedValue ;
        }

        //if (this.UCFWJGDetail1 != null && this.UCFWJGDetail1.Value[0].ToString() != "" && this.UCFWJGDetail1.Value[1].ToString() != "")
        //{
        //    HTwhere["search_str_where"] += "  and 业务管理部门 = '" + this.UCFWJGDetail1.Value[1].ToString() + "' and I_YWGLBMFL like '%" + this.UCFWJGDetail1.Value[0].ToString() + "%' ";
        //}

        if (this.UCFWJGDetail1 != null)
        {
            if (this.UCFWJGDetail1.Value[0].ToString().Trim() != "")
            {
                HTwhere["search_str_where"] += "  and  业务管理部门分类 ='" + this.UCFWJGDetail1.Value[0].ToString() + "'";
            }
            if (this.UCFWJGDetail1.Value[1].ToString().Trim() != "")
            {
                HTwhere["search_str_where"] += "  and 业务管理部门 = '" + this.UCFWJGDetail1.Value[1].ToString().Trim() + "' ";
            }
        }
        if (txtjjrbh.Text.Trim() != "" && txtjjrbh.Text.Trim() != "无")
        {
            HTwhere["search_str_where"] += " and 经纪人编号 like '%" + txtjjrbh.Text.Trim() + "%'";
        }

        if (txtjjrxm.Text.Trim() != "" && txtjjrxm.Text.Trim() != "无")
        {
            HTwhere["search_str_where"] += " and 经纪人名称 like '%" + txtjjrxm.Text.Trim() + "%'";
        }



       // HTwhere["search_str_where"] += "  and 所属分公司 = '" + this.ddlssfgs.SelectedValue.ToString() + "' and 经纪人编号 like '%" + txtjjrbh.Text.Trim() + "%' and 经纪人名称 like '%" + txtjjrxm.Text.Trim() + "%'";
        hidwhereis.Value = HTwhere["search_str_where"].ToString();
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(" select (SYKSNF+'年'+SYKSYF+'月') as 收益时间,*  from ");
        stringBuilder.Append("(select  b.I_PTGLJG as 业务管理部门,JJRJSBH as '经纪人编号',b.I_JYFMC as '经纪人名称',  isnull(KSSYJE,0.00) as '总收益金额',SKJE as '扣税金额', SHJE as '可支取收益',SYKSNF,SYKSYF, b.I_YWGLBMFL as 业务管理部门分类 from AAA_JJRSYKSMXB as a  left join AAA_DLZHXXB as b on a.JJRJSBH = b.J_JJRJSBH where SFYJJSWC='已计算') as  tab1");
        if (!hidwhereis.Value.Trim().Equals(""))
        {
            stringBuilder.Append(" where " + hidwhereis.Value.ToString());
        }
        else
        {
            stringBuilder.Append(" where  1!=1 ");
        }
       // stringBuilder.Append(" where " + hidwhereis.Value.ToString());
        DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString());
        dataSet.Tables[0].Columns.Remove("SYKSNF");
        dataSet.Tables[0].Columns.Remove("SYKSYF");
        MyXlsClass MXC = new MyXlsClass();

        MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "经纪人（自然人）收益汇总表", "经纪人（自然人）收益汇总表", 15);


    }
}