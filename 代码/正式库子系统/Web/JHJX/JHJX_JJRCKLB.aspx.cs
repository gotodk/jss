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

public partial class Web_JHJX_JHJX_JJRCKLB : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            setchengshi();//绑定分公司
            DisGrid();
        }
    }
    //绑定分公司
    protected void setchengshi()
    {
        string MainERP = DbHelperSQL.GetSingle("SELECT DYERP FROM SYSTEM_CITY_0 WHERE NAME='公司总部'").ToString();//总部对应的ERP名称
        DataSet ds = DbHelperSQL.Query("SELECT DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处' and DYERP <>'" + MainERP + "'");
        ddrFGSMC.DataSource = ds.Tables[0].DefaultView;
        ddrFGSMC.DataTextField = "DYFGSMC";
        ddrFGSMC.DataValueField = "DYFGSMC";
        ddrFGSMC.DataBind();
        ddrFGSMC.Items.Insert(0, new ListItem("全部分公司", ""));
        string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
        string bm = DbHelperSQL.GetSingle(sql).ToString();
        if (bm.IndexOf("办事处") > 0)
        {
            sql = "select count(name) from system_city where name='" + bm + "'";
            string fgsSql = "select DYFGSMC from system_city where name='" + bm + "'";
            if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
            {
                string strFGSMC = DbHelperSQL.GetSingle(fgsSql).ToString();
                ddrFGSMC.DataSource = null;
                ddrFGSMC.DataBind();
                ddrFGSMC.Items.Clear();
                ddrFGSMC.Items.Add(strFGSMC);

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
            MessageBox.ShowAlertAndBack(this, "查询超时，请重试或者修改查询条件！");
        }
        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            int i=0;
            foreach(DataRow row in NewDS.Tables[0].Rows)
            {
                i++;
                row["RowID"] = i.ToString();
            }
            rptDLRSH.DataSource = NewDS.Tables[0].DefaultView;
        }
        else
        {
            rptDLRSH.DataSource = DbHelperSQL.Query("select '角色'='经纪人',RowID ='',b.Number,a.YHM,a.JSBH,a.DLYX,b.LXRXM,b.SJH,CAST (c.ZCSJ as datetime) as 'ZCSJ',a.FGSKTSHTGSJ,b.SSFGS from ZZ_YHJSXXB as a inner join ZZ_DLRJBZLXXB as b on  a.JSBH=b.JSBH  inner join ZZ_UserLogin as c on a.YHM =c.YHM  and a.DLYX=c.DLYX  where 1!=1"); ;
        }
        rptDLRSH.DataBind();
    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select '角色'='经纪人',RowID ='',b.Number,a.YHM,a.JSBH,a.DLYX,b.LXRXM,b.SJH,CAST (c.ZCSJ as datetime) as 'ZCSJ',a.FGSKTSHTGSJ,b.SSFGS from ZZ_YHJSXXB as a inner join ZZ_DLRJBZLXXB as b on  a.JSBH=b.JSBH  inner join ZZ_UserLogin as c on a.YHM =c.YHM  and a.DLYX=c.DLYX  where  c.SFYZYX='是' and a.FGSKTSHZT ='审核通过') as tab ";  //检索的表
        ht_where["search_mainid"] = " Number ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " ZCSJ ";  //用于排序的字段
        return ht_where;
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        HTwhere["search_str_where"] = HTwhere["search_str_where"] + "  and SSFGS like '%" + ddrFGSMC.SelectedValue.ToString() + "%'";
        if (txtTimeStart.Text.Trim() != "" && txtTimeEnd.Text.Trim() == "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + "  and ZCSJ > '" + Convert.ToDateTime(txtTimeStart.Text.Trim()) + "'";
        }
        else if (txtTimeStart.Text.Trim() == "" && txtTimeEnd.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + "  and ZCSJ < '" +
                Convert.ToDateTime(txtTimeEnd.Text.Trim()).AddDays(1) + "'";
        }
        else if (txtTimeStart.Text.Trim() != "" && txtTimeEnd.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + "  and ZCSJ between '" +
                Convert.ToDateTime(txtTimeStart.Text.Trim()) + "' and '" + Convert.ToDateTime(txtTimeEnd.Text.Trim()).AddDays(1) + "'";
        }
        if (txtYHM.Text.Trim()!="")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + "and YHM like '%" + txtYHM.Text.Trim() + "%'";
        }
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }
    protected void rptDLRSH_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "linkSee")
        {
            Response.Redirect("JHJX_DLRSHXQ.aspx?JSBH="+e.CommandArgument.ToString().Trim()+"&ck=OK");
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
}