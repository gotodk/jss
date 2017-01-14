using FMOP.DB;
using Hesion.Brick.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_JHJX_JJRSYGR_jhjx_jjrsycsmxb : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
     
            //hidwhereis.Value = " 1=1 ";
          //  setchengshi();
           // DisGrid();
            this.UCFWJGDetail1.initdefault();
        }
    }


    protected void setchengshi()
    {
        /*
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
         */
    }

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
        ht_where["serach_Row_str"] = " * ";
        ht_where["search_tbname"] = " (select  b.I_PTGLJG as 业务管理部门,b.I_YWGLBMFL as '业务管理部门分类' ,JSBH as '经纪人编号',b.I_JYFMC as '经纪人名称',b.I_ZCLB as '经纪人类别',(case XZ WHEN '来自买方收益'  then (case LYYWLX when 'AAA_THDYFHDXXB' then (select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end )   when '来自卖方收益'  then  (case LYYWLX when 'AAA_THDYFHDXXB' then (select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end ) else '' end )  as  '关联交易方编号',(select  I_JYFMC from AAA_DLZHXXB where (J_BUYJSBH=(case XZ WHEN '来自买方收益'  then (case LYYWLX when 'AAA_THDYFHDXXB' then (select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end )   when '来自卖方收益'  then  (case LYYWLX when 'AAA_THDYFHDXXB' then (select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end ) else '' end )  or J_SELJSBH = (case XZ WHEN '来自买方收益'  then (case LYYWLX when 'AAA_THDYFHDXXB' then (select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end )   when '来自卖方收益'  then  (case LYYWLX when 'AAA_THDYFHDXXB' then (select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end ) else '' end ) )) as '关联交易方名称',case XZ WHEN '来自买方收益' then '买方' when '来自卖方收益' then '卖方' else '数据异常' end as '关联交易方身份',LYDH as '来源业务单号',( case LYYWLX when 'AAA_THDYFHDXXB' then (select isnull(ZBDJ,0.00) * isnull(T_THSL,0.00)  from AAA_THDYFHDXXB where Number = a.LYDH ) when 'AAA_ZBDBXXB' then (select   case when B_HKJD>=SJZFJE then SJZFJE else B_HKJD end   from AAA_RGQPCLB where ZBDBXXBBH = a.LYDH) else 0.00 end  ) as '来源业务金额',JE as '税前收益金额', ZY as '业务摘要',XZ as '收益来源' , a.CreateTime as '收益产生时间'  from AAA_ZKLSMXB as a  left join AAA_DLZHXXB as b on a.JSBH = b.J_JJRJSBH  where XM='经纪人收益'  and SJLX = '预') tab1 ";  //检索的表
        ht_where["search_mainid"] = "tab1.经纪人编号";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = "tab1.经纪人编号";  //用于排序的字段
        return ht_where;
    }
    public void DisGrid()
    {
        hidwhereis.Value = "";

        Hashtable HTwhere = SetV();
        //if (this.UCFWJGDetail1 != null && this.UCFWJGDetail1.Value[0].ToString() != "" && this.UCFWJGDetail1.Value[1].ToString()!="")
        //{
        //    HTwhere["search_str_where"] += "  and 业务管理部门 = '" + this.UCFWJGDetail1.Value[1].ToString() + "' and  I_YWGLBMFL ='" + this.UCFWJGDetail1.Value[0].ToString() + "'";
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
        if (this.Txtkssj.Text != "")
        {
            HTwhere["search_str_where"] += " and 收益产生时间 >='" + Txtkssj.Text.Trim() + "'";
        }
        if (this.Txtjssj.Text != "")
        {
            HTwhere["search_str_where"] += " and 收益产生时间 <='" + Txtjssj.Text.Trim() + " 23:59:59 '";
        }
        if (!this.ddljjrlb.SelectedValue.Trim().Equals("") && !this.ddljjrlb.SelectedValue.Equals("无"))
        {
            HTwhere["search_str_where"] += " and 经纪人类别='" + this.ddljjrlb.SelectedValue.Trim() + "'";
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
        //stringBuilder.Append(" select '" + this.hidID.Value + "' as '收益时间',*  from ");
        stringBuilder.Append(" select *  from  (select  a.CreateTime as '收益产生时间', b.I_PTGLJG as 业务管理部门,b.I_YWGLBMFL as 业务管理部门分类 ,JSBH as '经纪人编号',b.I_JYFMC as '经纪人名称',b.I_ZCLB as '经纪人类别',(case XZ WHEN '来自买方收益'  then (case LYYWLX when 'AAA_THDYFHDXXB' then (select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end )   when '来自卖方收益'  then  (case LYYWLX when 'AAA_THDYFHDXXB' then (select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end ) else '' end )  as  '关联交易方编号',(select  I_JYFMC from AAA_DLZHXXB where (J_BUYJSBH=(case XZ WHEN '来自买方收益'  then (case LYYWLX when 'AAA_THDYFHDXXB' then (select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end )   when '来自卖方收益'  then  (case LYYWLX when 'AAA_THDYFHDXXB' then (select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end ) else '' end )  or J_SELJSBH = (case XZ WHEN '来自买方收益'  then (case LYYWLX when 'AAA_THDYFHDXXB' then (select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select Y_YSYDDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end )   when '来自卖方收益'  then  (case LYYWLX when 'AAA_THDYFHDXXB' then (select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number = (select ZBDBXXBBH from  AAA_THDYFHDXXB  where Number=a.LYDH ) ) when 'AAA_ZBDBXXB' then (  select T_YSTBDMJJSBH from  dbo.AAA_ZBDBXXB where Number=a.LYDH) else '' end ) else '' end ) )) as '关联交易方名称',case XZ WHEN '来自买方收益' then '买方' when '来自卖方收益' then '卖方' else '数据异常' end as '关联交易方身份',LYDH as '来源业务单号',( case LYYWLX when 'AAA_THDYFHDXXB' then (select isnull(ZBDJ,0.00) * isnull(T_THSL,0.00)  from AAA_THDYFHDXXB where Number = a.LYDH ) when 'AAA_ZBDBXXB' then (select   case when B_HKJD>=SJZFJE then SJZFJE else B_HKJD end   from AAA_RGQPCLB where ZBDBXXBBH = a.LYDH) else 0.00 end  ) as '来源业务金额',JE as '税前收益金额', ZY as '业务摘要',XZ as '收益来源'   from AAA_ZKLSMXB as a  left join AAA_DLZHXXB as b on a.JSBH = b.J_JJRJSBH  where XM='经纪人收益'  and SJLX = '预') tab1 ");
        if (!hidwhereis.Value.Trim().Equals(""))
        {
            stringBuilder.Append(" where " + hidwhereis.Value.ToString());
        }
        else
        {
            stringBuilder.Append(" where  1!=1" );
        }
        DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString());
        MyXlsClass MXC = new MyXlsClass();

        MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "经纪人收益产生明细表", "经纪人收益产生明细表", 15);


    }
}