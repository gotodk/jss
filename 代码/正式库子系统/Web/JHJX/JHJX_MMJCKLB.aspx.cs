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

public partial class Web_JHJX_JHJX_MMJCKLBt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            setchengshi();
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
            this.ClientScript.RegisterStartupScript(GetType(), "message", "<script>window.top.Dialog.WarningConfirm('服务器超时，请稍后重试！',function(){ window.location.href=window.location.href;});</script>");
        }

        // RadGrid1.DataSource = null;

        if (NewDS != null && NewDS.Tables[0].Rows.Count > 0)
        {
            int j=0;
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                ////得到编号
                //Hashtable ht_whereTablePage = this.commonpagernew1.HTwhere;
                //NewDS.Tables[0].Rows[i]["ROWID"] = Convert.ToInt32(ht_whereTablePage["page_index"]) * Convert.ToInt32(ht_whereTablePage["page_size"]) + i + 1;
                j++;
                NewDS.Tables[0].Rows[i]["ROWID"] = j.ToString();
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
            strSql += " and MJ.YHM like '%" + this.txtYHM.Text.Trim() + "%' ";
        }

        if (!String.IsNullOrEmpty(txtTimeStart.Text))
        {
            strSql += " and Convert(varchar(10),LG.ZCSJ,120) >= '" + txtTimeStart.Text + "'";
        }
        if (!String.IsNullOrEmpty(txtTimeEnd.Text))
        {
            strSql += " and Convert(varchar(10),LG.ZCSJ,120) <= '" + txtTimeEnd.Text + "'";
        }
        if (drpJSFL.SelectedValue.Trim() != "所有分类")
        {
            strSql += " and JS.JSLX like '%" + drpJSFL.SelectedValue.Trim() + "%'";
        }

        string sql1 = "select '' as 'ROWID',MJ.JSBH,JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SJH,LG.ZCSJ,JS.FGSKTSHZT,JS.FGSKTSHTGSJ,JS.KTSHZT,JS.KTSHTGSJ,MJJJR.SQGLSJ,DLR.SSFGS as 'SSFGS','审核' as 'CAOZUO' from ZZ_SELJBZLXXB as MJ left join ZZ_UserLogin as LG on MJ.DLYX=LG.DLYX and LG.YHM=MJ.YHM left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MJJJR on MJJJR.JSLX=JS.JSLX and MJJJR.JSBH=JS.JSBH  left join ZZ_DLRJBZLXXB as DLR on MJJJR.DLRBH=DLR.JSBH where DLR.SSFGS like '%" + ddrFGSMC.SelectedValue.ToString().Trim() + "%'  and MJJJR.GLZT='有效'  and  (JS.KTSHZT='审核通过' and JS.FGSKTSHZT='审核通过') " + strSql + " union all select '' as 'ROWID',MJ.JSBH,JS.JSLX,MJ.YHM,MJ.DLYX,MJ.LXRXM,MJ.SJH,LG.ZCSJ,JS.FGSKTSHZT,JS.FGSKTSHTGSJ,JS.KTSHZT,JS.KTSHTGSJ,MJJJR.SQGLSJ,DLR.SSFGS as 'SSFGS', '审核' as 'CAOZUO' from ZZ_BUYJBZLXXB as MJ left join ZZ_UserLogin as LG on MJ.DLYX=LG.DLYX and LG.YHM=MJ.YHM left join ZZ_YHJSXXB as JS  on MJ.JSBH=JS.JSBH left join ZZ_MMJYDLRZHGLB as MJJJR on MJJJR.JSLX=JS.JSLX and MJJJR.JSBH=JS.JSBH  left join ZZ_DLRJBZLXXB as DLR on MJJJR.DLRBH=DLR.JSBH where DLR.SSFGS like '%" + ddrFGSMC.SelectedValue.ToString().Trim() + "%'  and MJJJR.GLZT='有效'  and  (JS.KTSHZT='审核通过' and JS.FGSKTSHZT='审核通过') " + strSql;

        Hashtable ht_where = new Hashtable();
        //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage2";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.
        //ht_where["this_dblink"] = "不赋值或ERP";//这个可以不设置,默认是FMOP,即连接业务平台数据库。如需连接ERP，则需要设置为ERP.
        //ht_where["page_index"] = "0";//这个可以不设置,如果需要指定进入页面后显示第几页，才需要设置。必须是数字
        //ht_where["search_fyshow"] = "5";//这个也可以不设置，默认就是5,即页数可点击的按钮是11个。必须是数字，不能是0。
        //ht_where["count_float"] = "特殊";  //这个可以不用设置，特殊情况设置为特殊
        ht_where["page_size"] = "10"; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (" + sql1 + ") as tabl ";  //检索的表
        ht_where["search_mainid"] = " JSBH ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = " ZCSJ ";  //用于排序的字段
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
}