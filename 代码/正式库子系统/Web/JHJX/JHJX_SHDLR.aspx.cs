using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using FMOP.DB;
using System.Web.UI.HtmlControls;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_JHJX_JHJX_SHDLR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
           
            ViewState["当前账户所属公司"] = "";
            DisGrid();
            //setchengshi();
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

        // RadGrid1.DataSource = null;

        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                //得到编号
                Hashtable ht_whereTablePage = this.commonpagernew1.HTwhere;
                NewDS.Tables[0].Rows[i]["ROWID"] = Convert.ToInt32(ht_whereTablePage["page_index"]) * Convert.ToInt32(ht_whereTablePage["page_size"]) + i + 1;
            }

            rptDLRSH.DataSource = NewDS.Tables[0];
            tdEmpty.Visible = false;
        }
        else
        {
            rptDLRSH.DataSource = DbHelperSQL.Query(" select '' AS ROWID,a.YHM,a.JSBH,a.DLYX,b.LXRXM,b.SJH,CAST (c.ZCSJ as datetime) as 'ZCSJ',a.FGSKTSHZT,a.KTSHZT from ZZ_YHJSXXB as a inner join ZZ_DLRJBZLXXB as b on  a.JSBH=b.JSBH  inner join ZZ_UserLogin as c on a.YHM =c.YHM and a.DLYX=c.DLYX   where 1!=1").Tables[0].DefaultView;
            tdEmpty.Visible = true;
            // commonpagernew1.Visible = false;
        }
        rptDLRSH.DataBind();
    }

    //设置初始默认值检索
    private Hashtable SetV()
    {
       
        string strSql = "";
        if (!String.IsNullOrEmpty(txtYHM.Text))
        {
            strSql += " and YHM like '%"+this.txtYHM.Text.Trim()+"%' ";
        }
        if (!String.IsNullOrEmpty(txtTimeStart.Text))
        {
            strSql += " and Convert(varchar(10),ZCSJ,120) >= '" + txtTimeStart.Text + "'";
        }
        if (!String.IsNullOrEmpty(txtTimeEnd.Text))
        {
            strSql += " and Convert(varchar(10),ZCSJ,120) <= '" + txtTimeEnd.Text + "'";
        }
        string sql1 = "";
        string QX = "SELECT  BM from HR_EMPLOYEES WHERE NUMBER ='" + User.Identity.Name + "'";//判断权限
        //当前分公司下未审核的经纪人信息
        if ((DbHelperSQL.GetSingle(QX).ToString().IndexOf("办事处") > 0) && User.Identity.Name != "admin")
        {
            ViewState["当前账户所属公司"] = "分公司人员";
            string strSql2 = "SELECT name,DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处' and Name='" + DbHelperSQL.GetSingle(QX).ToString() + "'";
            DataTable dataTable = DbHelperSQL.Query(strSql2).Tables[0];
            //sql1 = " select '' AS ROWID,a.YHM,a.JSBH,a.DLYX,b.LXRXM,b.SJH,CAST (c.ZCSJ as datetime) as 'ZCSJ',a.FGSKTSHZT,a.KTSHZT from ZZ_YHJSXXB as a inner join ZZ_DLRJBZLXXB as b on  a.JSBH=b.JSBH  inner join ZZ_UserLogin as c on a.YHM =c.YHM  and a.DLYX=c.DLYX  where  c.SFYZYX='是' and b.SSFGS='" + dataTable.Rows[0]["DYFGSMC"] + "'  and  (a.KTSHZT in('未审核','待审核')  or a.FGSKTSHZT in('未审核','待审核'))  " + strSql;
            sql1 = " select '' AS ROWID,a.YHM,a.JSBH,a.DLYX,b.LXRXM,b.SJH,CAST (c.ZCSJ as datetime) as 'ZCSJ',a.FGSKTSHZT,a.KTSHZT from ZZ_YHJSXXB as a inner join ZZ_DLRJBZLXXB as b on  a.JSBH=b.JSBH  inner join ZZ_UserLogin as c on a.YHM =c.YHM  and a.DLYX=c.DLYX  where  c.SFYZYX='是' and b.SSFGS='" + dataTable.Rows[0]["DYFGSMC"] + "'  and  ( a.FGSKTSHZT in('未审核','待审核'))  " + strSql;
        }
        else
        {
            if (User.Identity.Name == "admin")
            {
                ViewState["当前账户所属公司"] = "分公司人员";

                //sql1 = " select '' AS ROWID,a.YHM,a.JSBH,a.DLYX,b.LXRXM,b.SJH,CAST (c.ZCSJ as datetime) as 'ZCSJ',a.FGSKTSHZT,a.KTSHZT from ZZ_YHJSXXB as a inner join ZZ_DLRJBZLXXB as b on  a.JSBH=b.JSBH  inner join ZZ_UserLogin as c on a.YHM =c.YHM  and a.DLYX=c.DLYX  where  c.SFYZYX='是' and b.SSFGS='" + dataTable.Rows[0]["DYFGSMC"] + "'  and  (a.KTSHZT in('未审核','待审核')  or a.FGSKTSHZT in('未审核','待审核'))  " + strSql;
                sql1 = " select '' AS ROWID,a.YHM,a.JSBH,a.DLYX,b.LXRXM,b.SJH,CAST (c.ZCSJ as datetime) as 'ZCSJ',a.FGSKTSHZT,a.KTSHZT from ZZ_YHJSXXB as a inner join ZZ_DLRJBZLXXB as b on  a.JSBH=b.JSBH  inner join ZZ_UserLogin as c on a.YHM =c.YHM  and a.DLYX=c.DLYX  where  c.SFYZYX='是'  and  ( a.FGSKTSHZT in('未审核','待审核'))  " + strSql;
            }

            //ViewState["当前账户所属公司"] = "总部人员";
            ////总部未审核的经纪人信息
            //sql1 = " select '' AS ROWID,a.YHM,a.JSBH,a.DLYX,b.LXRXM,b.SJH,CAST (c.ZCSJ as datetime) as 'ZCSJ',a.FGSKTSHZT,a.KTSHZT from ZZ_YHJSXXB as a inner join ZZ_DLRJBZLXXB as b on  a.JSBH=b.JSBH  inner join ZZ_UserLogin as c on a.YHM =c.YHM  and a.DLYX=c.DLYX   where  c.SFYZYX='是' and  a.KTSHZT in('未审核','待审核')  or a.FGSKTSHZT in('未审核','待审核') " + strSql;
        }
       
        Hashtable ht_where = new Hashtable();
        //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage2";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.
        //ht_where["this_dblink"] = "不赋值或ERP";//这个可以不设置,默认是FMOP,即连接业务平台数据库。如需连接ERP，则需要设置为ERP.
        //ht_where["page_index"] = "0";//这个可以不设置,如果需要指定进入页面后显示第几页，才需要设置。必须是数字
        //ht_where["search_fyshow"] = "5";//这个也可以不设置，默认就是5,即页数可点击的按钮是11个。必须是数字，不能是0。
        //ht_where["count_float"] = "特殊";  //这个可以不用设置，特殊情况设置为特殊
        ht_where["page_size"] = "10"; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (" + sql1 + ") as tabl ";  //检索的表
        ht_where["search_mainid"] = " number ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " ZCSJ ";  //用于排序的字段
        // ht_where["returnlastpage_open"] = "_info.aspx"; //开启自动返回之前页数，留空或不设置时，不返回，若需要返回，需要设置详情页面网址关键字(如aaa.aspx)
        return ht_where;
    }

    //protected void setchengshi()
    //{
    //    string MainERP = DbHelperSQL.GetSingle("SELECT DYERP FROM SYSTEM_CITY_0 WHERE NAME='公司总部'").ToString();//总部对应的ERP名称

    //    city.DataSource = DbHelperSQL.Query("SELECT name,DYFGSMC FROM System_City where DYFGSMC != '' and name<>'其他办事处' and DYERP <>'" + MainERP + "'");
    //    city.DataTextField = "DYFGSMC";
    //    city.DataValueField = "name";
    //    city.DataBind();
    //    city.Items.Insert(0, new ListItem("全部分公司", ""));
    //    string sql = "select bm from hr_employees where number='" + User.Identity.Name + "'";
    //    string bm = DbHelperSQL.GetSingle(sql).ToString();
    //    if (bm.IndexOf("办事处") > 0)
    //    {
    //        sql = "select name,dyfgsmc from system_city where name='" + bm + "'";
    //        DataSet ds = DbHelperSQL.Query(sql);
    //        if (ds != null && ds.Tables[0].Rows.Count > 0)
    //        {
    //            city.DataSource = null;
    //            city.DataBind();
    //            city.Items.Clear();
    //            city.Items.Insert(0, new ListItem(ds.Tables[0].Rows[0]["dyfgsmc"].ToString(), bm));
    //        }
    //    }
    //}

    public void DisGrid()
    {

        //应用实例

        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and YHM like '%" + txtYHM.Text.ToString() + "%' ";

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rptBHDDZXH_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "linkSee":
                //// DataRowView dataRow = (DataRowView)e.Item.DataItem;
                //HtmlGenericControl lbl = (HtmlGenericControl)e.Item.FindControl("bhdlx");

                //string strUrl = "JHJX_DLRSHXQ.aspx?ID=" + e.CommandArgument + "&LB=" + Server.UrlEncode(lbl.InnerText.ToString().Trim());
                string strUrl = "JHJX_DLRSHXQ.aspx?JSBH=" + e.CommandArgument;
                Response.Redirect(strUrl);
                break;
        }
    }

    /// <summary>
    ///处理操作中审核按钮的可用状态
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptDLRSH_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //总部开通审核状态
            string strZBKTSHZT =DataBinder.Eval(e.Item.DataItem, "KTSHZT").ToString();
            //分公司开通审核状态
            string  strFGSKTSHZT = DataBinder.Eval(e.Item.DataItem, "FGSKTSHZT").ToString();
         
            if (ViewState["当前账户所属公司"].ToString() == "分公司人员")
            {
                if (strFGSKTSHZT == "审核通过")
                {
                    LinkButton linkButton = (LinkButton)e.Item.FindControl("linkSee");
                    linkButton.Enabled = false;
                    linkButton.ToolTip = "分公司已经对该经纪人进行过审核操作，不允许再进行审核操作！";
                }
            
            }
            //else if (ViewState["当前账户所属公司"].ToString() == "总部人员")
            //{
            //    if (strFGSKTSHZT == "待审核" || strFGSKTSHZT == "未审核"||strFGSKTSHZT=="驳回")
            //    {
            //        LinkButton linkButton = (LinkButton)e.Item.FindControl("linkSee");
            //        linkButton.Enabled = false;
            //        linkButton.ToolTip = "分公司审核通过后总部才能对该信息进行审核操作！";
            //    }
              
            //    if (strZBKTSHZT == "审核通过")
            //    {
            //        LinkButton linkButton = (LinkButton)e.Item.FindControl("linkSee");
            //        linkButton.Enabled = false;
            //        linkButton.ToolTip = "总部已经对该经纪人进行过审核操作，不允许再进行审核操作！";
            //    }
            
            //}
        }
    }
   
}