using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hesion.Brick.Core.WorkFlow;
using System.Data;
using System.Collections;
using FMOP.DB;
using System.Web.UI.HtmlControls;

public partial class Web_JHJX_JHJX_SHMMJ : System.Web.UI.Page
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
            rptDLRSH.DataSource = DbHelperSQL.Query(" select '' as 'ROWID',MJ.JSBH,JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SJH,LG.ZCSJ,JS.FGSKTSHZT,JS.KTSHZT,MJJJR.SQGLSJ,DLR.SSFGS as 'SSFGS','审核' as 'CAOZUO' from ZZ_SELJBZLXXB as MJ left join ZZ_UserLogin as LG on MJ.DLYX=LG.DLYX and LG.YHM=MJ.YHM left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MJJJR on MJJJR.JSLX=JS.JSLX and MJJJR.JSBH=JS.JSBH  left join ZZ_DLRJBZLXXB as DLR on MJJJR.DLRBH=DLR.JSBH where DLR.SSFGS='山东分公司'  and MJJJR.GLZT='有效'  and  (JS.KTSHZT in('未审核','待审核')  or JS.FGSKTSHZT in('未审核','待审核')) and 1!=1").Tables[0].DefaultView;
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
            strSql += " and 用户名 like '%" + this.txtYHM.Text.Trim() + "%' ";
        }

        if (!String.IsNullOrEmpty(txtTimeStart.Text))
        {
            strSql += " and Convert(varchar(10),注册时间,120) >= '" + txtTimeStart.Text + "'";
        }
        if (!String.IsNullOrEmpty(txtTimeEnd.Text))
        {
            strSql += " and Convert(varchar(10),注册时间,120) <= '" + txtTimeEnd.Text + "'";
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
            //sql1 = "select '' as 'ROWID',MJ.JSBH,JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SJH,LG.ZCSJ,MJJJR.FGSSHZT,MJJJR.DLRSHZT,MJJJR.SQGLSJ,DLR.SSFGS as 'SSFGS','审核' as 'CAOZUO',MJJJR.DLRBH from ZZ_SELJBZLXXB as MJ left join ZZ_UserLogin as LG on MJ.DLYX=LG.DLYX and LG.YHM=MJ.YHM left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MJJJR on MJJJR.JSLX=JS.JSLX and MJJJR.JSBH=JS.JSBH  left join ZZ_DLRJBZLXXB as DLR on MJJJR.DLRBH=DLR.JSBH where DLR.SSFGS='" + dataTable.Rows[0]["DYFGSMC"] + "'  and MJJJR.GLZT='有效'  and  (MJJJR.DLRSHZT in('未审核','待审核')  or MJJJR.FGSSHZT in('未审核','待审核')) " + strSql + " union all select '' as 'ROWID',MJ.JSBH,JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SJH,LG.ZCSJ,MJJJR.FGSSHZT,MJJJR.DLRSHZT,MJJJR.SQGLSJ,DLR.SSFGS as 'SSFGS', '审核' as 'CAOZUO',MJJJR.DLRBH from ZZ_BUYJBZLXXB as MJ left join ZZ_UserLogin as LG on MJ.DLYX=LG.DLYX and LG.YHM=MJ.YHM left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MJJJR on MJJJR.JSLX=JS.JSLX and MJJJR.JSBH=JS.JSBH  left join ZZ_DLRJBZLXXB as DLR on MJJJR.DLRBH=DLR.JSBH where DLR.SSFGS='" + dataTable.Rows[0]["DYFGSMC"] + "'  and MJJJR.GLZT='有效'  and  (MJJJR.DLRSHZT in('未审核','待审核')  or MJJJR.FGSSHZT in('未审核','待审核')) " + strSql;
            sql1 = "select '' as 'ROWID',结算账户,SEL.YHM 用户名,登录邮箱,SEL.LXRXM 联系人姓名,SEL.SJH 手机号,申请关联时间,SEL.CreateTime 注册时间,经纪人审核状态,分公司审核状态,L.JSZHLX 结算账户类型,关联经纪人账号,SEL.JSBH 角色编号,SEL.SSFGS 所属分公司,'审核' as 'CAOZUO' from (select distinct JSDLZH 登录邮箱,JSZHLX 结算账户,DLRDLZH 关联经纪人账号,DLRSHZT 经纪人审核状态,FGSSHZT 分公司审核状态,SQGLSJ 申请关联时间 from ZZ_MMJYDLRZHGLB where DLRSHZT ='已审核' and FGSSHZT='待审核' and JSZHLX='卖家账户' and JSLX='卖家') as MMJ left join ZZ_SELJBZLXXB as SEL on MMJ.登录邮箱=SEL.DLYX left join ZZ_UserLogin as L on MMJ.登录邮箱=L.DLYX where SEL.SSFGS='" + dataTable.Rows[0]["DYFGSMC"] + "'" + strSql + " union select '' as 'ROWID',结算账户,BUY.YHM 用户名,登录邮箱,BUY.LXRXM 联系人姓名,BUY.SJH 手机号,申请关联时间,BUY.CreateTime 注册时间,经纪人审核状态,分公司审核状态,L.JSZHLX 结算账户类型,关联经纪人账号,BUY.JSBH 角色编号,BUY.SSFGS 所属分公司,'审核' as 'CAOZUO' from (select distinct JSDLZH 登录邮箱,JSZHLX 结算账户,DLRDLZH 关联经纪人账号,DLRSHZT 经纪人审核状态,FGSSHZT 分公司审核状态,SQGLSJ 申请关联时间 from ZZ_MMJYDLRZHGLB where DLRSHZT ='已审核' and FGSSHZT='待审核' and JSZHLX='买家账户' and JSLX='买家') as MMJ left join ZZ_BUYJBZLXXB as BUY on MMJ.登录邮箱=BUY.DLYX left join ZZ_UserLogin as L on MMJ.登录邮箱=L.DLYX  where BUY.SSFGS='" + dataTable.Rows[0]["DYFGSMC"] + "'" + strSql + " union select '' as 'ROWID',结算账户,DL.YHM 用户名,登录邮箱,DL.LXRXM 联系人姓名,DL.SJH 手机号,申请关联时间,DL.CreateTime 注册时间,经纪人审核状态,分公司审核状态,L.JSZHLX 结算账户类型,关联经纪人账号,DL.JSBH 角色编号,DL.SSFGS 所属分公司,'审核' as 'CAOZUO' from (select distinct JSDLZH 登录邮箱,JSZHLX 结算账户,DLRDLZH 关联经纪人账号,DLRSHZT 经纪人审核状态,FGSSHZT 分公司审核状态,SQGLSJ 申请关联时间 from ZZ_MMJYDLRZHGLB where DLRSHZT ='已审核' and FGSSHZT='待审核' and JSZHLX='经纪人账户' and JSLX='买家') as MMJ left join dbo.ZZ_DLRJBZLXXB as DL on MMJ.登录邮箱=DL.DLYX left join ZZ_UserLogin as L on MMJ.登录邮箱=L.DLYX where DL.SSFGS='" + dataTable.Rows[0]["DYFGSMC"] + "'" + strSql;
           
        }
        if (User.Identity.Name == "admin")
        {
            ViewState["当前账户所属公司"] = "分公司人员";

            //sql1 = "select '' as 'ROWID',MJ.JSBH,JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SJH,LG.ZCSJ,MJJJR.FGSSHZT,MJJJR.DLRSHZT,MJJJR.SQGLSJ,DLR.SSFGS as 'SSFGS','审核' as 'CAOZUO',MJJJR.DLRBH from ZZ_SELJBZLXXB as MJ left join ZZ_UserLogin as LG on MJ.DLYX=LG.DLYX and LG.YHM=MJ.YHM left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MJJJR on MJJJR.JSLX=JS.JSLX and MJJJR.JSBH=JS.JSBH  left join ZZ_DLRJBZLXXB as DLR on MJJJR.DLRBH=DLR.JSBH where  MJJJR.GLZT='有效'  and  (MJJJR.DLRSHZT in('未审核','待审核')  or MJJJR.FGSSHZT in('未审核','待审核')) " + strSql + " union all select '' as 'ROWID',MJ.JSBH,JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SJH,LG.ZCSJ,MJJJR.FGSSHZT,MJJJR.DLRSHZT,MJJJR.SQGLSJ,DLR.SSFGS as 'SSFGS', '审核' as 'CAOZUO',MJJJR.DLRBH from ZZ_BUYJBZLXXB as MJ left join ZZ_UserLogin as LG on MJ.DLYX=LG.DLYX and LG.YHM=MJ.YHM left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MJJJR on MJJJR.JSLX=JS.JSLX and MJJJR.JSBH=JS.JSBH  left join ZZ_DLRJBZLXXB as DLR on MJJJR.DLRBH=DLR.JSBH where MJJJR.GLZT='有效'  and  (MJJJR.DLRSHZT in('未审核','待审核')  or MJJJR.FGSSHZT in('未审核','待审核')) " + strSql;
            sql1 = "select '' as 'ROWID',结算账户,SEL.YHM 用户名,登录邮箱,SEL.LXRXM 联系人姓名,SEL.SJH 手机号,申请关联时间,SEL.CreateTime 注册时间,经纪人审核状态,分公司审核状态,L.JSZHLX 结算账户类型,关联经纪人账号,SEL.JSBH 角色编号,SEL.SSFGS 所属分公司,'审核' as 'CAOZUO' from (select distinct JSDLZH 登录邮箱,JSZHLX 结算账户,DLRDLZH 关联经纪人账号,DLRSHZT 经纪人审核状态,FGSSHZT 分公司审核状态,SQGLSJ 申请关联时间 from ZZ_MMJYDLRZHGLB where DLRSHZT ='已审核' and FGSSHZT='待审核' and JSZHLX='卖家账户' and JSLX='卖家') as MMJ left join ZZ_SELJBZLXXB as SEL on MMJ.登录邮箱=SEL.DLYX left join ZZ_UserLogin as L on MMJ.登录邮箱=L.DLYX where 1=1 " + strSql + " union select '' as 'ROWID',结算账户,BUY.YHM 用户名,登录邮箱,BUY.LXRXM 联系人姓名,BUY.SJH 手机号,申请关联时间,BUY.CreateTime 注册时间,经纪人审核状态,分公司审核状态,L.JSZHLX 结算账户类型,关联经纪人账号,BUY.JSBH 角色编号,BUY.SSFGS 所属分公司,'审核' as 'CAOZUO' from (select distinct JSDLZH 登录邮箱,JSZHLX 结算账户,DLRDLZH 关联经纪人账号,DLRSHZT 经纪人审核状态,FGSSHZT 分公司审核状态,SQGLSJ 申请关联时间 from ZZ_MMJYDLRZHGLB where DLRSHZT ='已审核' and FGSSHZT='待审核' and JSZHLX='买家账户' and JSLX='买家') as MMJ left join ZZ_BUYJBZLXXB as BUY on MMJ.登录邮箱=BUY.DLYX left join ZZ_UserLogin as L on MMJ.登录邮箱=L.DLYX  where 1=1 " + strSql+" union select '' as 'ROWID',结算账户,DL.YHM 用户名,登录邮箱,DL.LXRXM 联系人姓名,DL.SJH 手机号,申请关联时间,DL.CreateTime 注册时间,经纪人审核状态,分公司审核状态,L.JSZHLX 结算账户类型,关联经纪人账号,DL.JSBH 角色编号,DL.SSFGS 所属分公司,'审核' as 'CAOZUO' from (select distinct JSDLZH 登录邮箱,JSZHLX 结算账户,DLRDLZH 关联经纪人账号,DLRSHZT 经纪人审核状态,FGSSHZT 分公司审核状态,SQGLSJ 申请关联时间 from ZZ_MMJYDLRZHGLB where DLRSHZT ='已审核' and FGSSHZT='待审核' and JSZHLX='经纪人账户' and JSLX='买家') as MMJ left join dbo.ZZ_DLRJBZLXXB as DL on MMJ.登录邮箱=DL.DLYX left join ZZ_UserLogin as L on MMJ.登录邮箱=L.DLYX where 1=1 " + strSql;

        }
        //else
        //{
        //    ViewState["当前账户所属公司"] = "总部人员";
        //    //总部未审核的经纪人信息
        //    sql1 = " select '' AS ROWID,a.YHM,a.JSBH,a.DLYX,b.LXRXM,b.SJH,CAST (c.ZCSJ as datetime) as 'ZCSJ',a.FGSKTSHZT,a.KTSHZT from ZZ_YHJSXXB as a inner join ZZ_DLRJBZLXXB as b on  a.JSBH=b.JSBH  inner join ZZ_UserLogin as c on a.YHM =c.YHM  and a.DLYX=c.DLYX   where  c.SFYZYX='是' and  a.KTSHZT in('未审核','待审核')  or a.FGSKTSHZT in('未审核','待审核') " + strSql;
        //}

        Hashtable ht_where = new Hashtable();
        //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage2";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.
        //ht_where["this_dblink"] = "不赋值或ERP";//这个可以不设置,默认是FMOP,即连接业务平台数据库。如需连接ERP，则需要设置为ERP.
        //ht_where["page_index"] = "0";//这个可以不设置,如果需要指定进入页面后显示第几页，才需要设置。必须是数字
        //ht_where["search_fyshow"] = "5";//这个也可以不设置，默认就是5,即页数可点击的按钮是11个。必须是数字，不能是0。
        //ht_where["count_float"] = "特殊";  //这个可以不用设置，特殊情况设置为特殊
        ht_where["page_size"] = "10"; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (" + sql1 + ") as tabl ";  //检索的表
        ht_where["search_mainid"] = " 登录邮箱 ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " 注册时间 ";  //用于排序的字段
        // ht_where["returnlastpage_open"] = "_info.aspx"; //开启自动返回之前页数，留空或不设置时，不返回，若需要返回，需要设置详情页面网址关键字(如aaa.aspx)
        return ht_where;
    }

    public void DisGrid()
    {



      

        //应用实例

        //开始调用自定义控件
        Hashtable HTwhere = SetV();
       // HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and YHM like '%" + txtYHM.Text.ToString() + "%' " + strSql;

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
        string[] cs=e.CommandArgument.ToString().Split('#');
        string jsbh = cs[0].ToString();
        string jjrzh = cs[1].ToString();
        string dlyx = cs[2].ToString();
        switch (e.CommandName)
        {
            case "linkSee":
                HiddenField hidJSLX = (HiddenField)e.Item.FindControl("hidJSLX");
                //string strUrl = "JHJX_DLRSHXQ.aspx?ID=" + e.CommandArgument + "&LB=" + Server.UrlEncode(lbl.InnerText.ToString().Trim());
                string strUrl = "JHJX_MMJSHXQ.aspx?JSBH=" + jsbh + "&JSLX=" + Server.UrlEncode(hidJSLX.Value.ToString().Trim()) + "&JJRBH=" + jjrzh + "&DLYX=" + dlyx;
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
            //经纪人审核状态
            string strJJRKTSHZT = DataBinder.Eval(e.Item.DataItem, "经纪人审核状态").ToString();
            //分公司开通审核状态
            string strFGSKTSHZT = DataBinder.Eval(e.Item.DataItem, "分公司审核状态").ToString();

            if (ViewState["当前账户所属公司"].ToString() == "分公司人员")
            {

                if (strJJRKTSHZT == "待审核" || strJJRKTSHZT == "未审核" || strJJRKTSHZT == "驳回")
                {
                    LinkButton linkButton = (LinkButton)e.Item.FindControl("linkSee");
                    linkButton.Enabled = false;
                    linkButton.ToolTip = "经纪人审核通过后分公司才能对该信息进行审核操作！";
                }


                if (strFGSKTSHZT == "审核通过")
                {
                    LinkButton linkButton = (LinkButton)e.Item.FindControl("linkSee");
                    linkButton.Enabled = false;
                    linkButton.ToolTip = "分公司已经对该经纪人进行过审核操作，不允许再进行审核操作！";
                }

            }
        }
    }
  
}