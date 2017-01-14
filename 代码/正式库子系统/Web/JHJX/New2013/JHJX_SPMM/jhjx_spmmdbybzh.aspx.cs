using FMOP.DB;
using Hesion.Brick.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_JHJX_New2013_JHJX_SPMM_jhjx_spmmdbybzh : System.Web.UI.Page
{
    string StrGettbdsql = "";
    string StrGettbdchaifen = "";
    string Stryddzhengchang = "";
    string Stryddchaidan = "";
    Hashtable ht_where = new Hashtable();
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
         * */
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
            /*---shiyan 2013-12-23 进行数据获取优化。---*/
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                if (NewDS.Tables[0].Rows[i]["单据类型"].ToString() == "投标单")
                {
                    //获取履约保证金剩余比率
                    object obj_lybzj = DbHelperSQL.GetSingle("select sum(a.JE) from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.number where SJLX='预' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息') and LYYWLX='AAA_THDYFHDXXB' and b.ZBDBXXBBH='" + NewDS.Tables[0].Rows[i]["中标定标编号"].ToString() + "'");
                    if (obj_lybzj != null && obj_lybzj.ToString() != "")
                    {
                        NewDS.Tables[0].Rows[i]["履约保证金冻结剩余比例"] = ((1 - Convert.ToDouble(obj_lybzj.ToString()) / Convert.ToDouble(NewDS.Tables[0].Rows[i]["履约保证金冻结金额"])) * 100.00).ToString("#0.00");
                    }                             
                }
            }
            /*---shiyan 结束---*/
            tdEmpty.Visible = false;
            dt = NewDS.Tables[0];
            rptSPXX.DataSource = dt.DefaultView;
        }
        else
        {
            rptSPXX.DataSource = null;
            tdEmpty.Visible = true;
        }
        rptSPXX.DataBind();
    }

    //设置初始默认值检索
    private Hashtable SetV()
    {
       // StrGettbdsql = "(select  a.DLYX as '交易方账号',(select I_JYFMC  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '交易方名称',(select I_PTGLJG  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门,(select I_YWGLBMFL  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门分类 , 'T'+a.Number as Number , CONVERT(varchar(100),b.Z_DBSJ, 20)  as 定标时间,b.Z_HTBH as 合同编号 , b.Number as 中标定标号, CONVERT(varchar(100),a.CreateTime, 20)   as  下单时间,	'投标单' as  单据类型,	a.Number as 单据编号,	 a.SPBH as  商品编号, a.SPMC as 商品名称,a.GG as 规格, b.Z_ZBSL as 定标数量,b.Z_ZBJG as 定标价格,b.Z_ZBJE as 定标金额,	 cast(b.T_YSTBDDJTBBZJ as varchar(200)) as 投标保证金解冻金额,'---' as	订金冻结金额, CAST (b.Z_LYBZJJE as varchar(200) ) as	履约保证金冻结金额,	'违约扣罚记录' as 违约扣罚记录,case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != TBNSL then '是' when YZBSL>0 and YZBSL = TBNSL and (select Count(Number) from AAA_ZBDBXXB where T_YSTBDBH =a.Number)>1 then '是' else '否'  end   as 是否拆单, CAST( cast(1-((isnull(( select   sum(JE) from AAA_ZKLSMXB where LYDH in(  select Number  from  AAA_THDYFHDXXB where  ZBDBXXBBH = b.Number) and SJLX = '预' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息')),0.00))/b.Z_LYBZJJE) as  numeric(18, 2))*100 as varchar(200) ) as 履约保证金冻结剩余比例 from AAA_ZBDBXXB as b left join AAA_TBD as a on a.Number = b.T_YSTBDBH where b.Z_HTZT='定标' )";
       // Stryddzhengchang = "(select a.DLYX as '交易方账号',(select I_JYFMC  from  AAA_DLZHXXB where B_DLYX = a.DLYX) as '交易方名称',(select I_PTGLJG  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门, (select I_YWGLBMFL  from  AAA_DLZHXXB where B_DLYX =  (select distinct GLJJRDLZH from  AAA_MJMJJYZHYJJRZHGLB where DLYX = a.DLYX and SFDQMRJJR='是')) as 业务管理部门分类, 'Y'+a.Number+b.Number as Number , CONVERT(varchar(100),b.Z_DBSJ, 20)  as 定标时间,b.Z_HTBH as 合同编号 , b.Number as 中标定标号 ,  CONVERT(varchar(100),a.CreateTime, 20) as  下单时间,	'预订单' as  单据类型,	a.Number as 单据编号,	 a.SPBH as  商品编号, a.SPMC as 商品名称,a.GG as 规格, b.Z_ZBSL as 定标数量,b.Z_ZBJG as 定标价格,b.Z_ZBJE as 定标金额,	'---' as 投标保证金解冻金额,cast(b.Z_DJJE as varchar(200) ) as 订金冻结金额,'---' as	履约保证金冻结金额,	'违约扣罚记录' as 违约扣罚记, case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != NDGSL then '是' when YZBSL>0 and YZBSL = NDGSL and (select Count(Number) from AAA_ZBDBXXB where Y_YSYDDBH =a.Number)>1 then '是' else '否'  end as 是否拆单, '---' as 履约保证金冻结剩余比例  from AAA_ZBDBXXB as b left join AAA_YDDXXB as a on a.Number = b.Y_YSYDDBH  where b.Z_HTZT='定标' )";


        //this.hidwhereis.Value = "  select * from  (" + StrGettbdsql + " union " + Stryddzhengchang + " ) as tab  ";
        //Hashtable ht_where = new Hashtable();
        //ht_where["page_size"] = " 15 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = " (" + StrGettbdsql + " union " + Stryddzhengchang + " ) as tab  "; //检索的表
        //ht_where["search_mainid"] = "tab.Number";  //所检索表的主键
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        //ht_where["search_paixu"] = " DESC ";  //排序方式
        //ht_where["search_paixuZD"] = "tab.下单时间";  //用于排序的字段

        /*---shiyan 2013-12-23 进行数据获取优化。---*/
        Hashtable ht_where = new Hashtable();
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是       
        ht_where["page_size"] = " 10 "; //必须设置,每页的数据量。必须是数字。不能是0。     
        ht_where["serach_Row_str"] = "  tab.*,b.I_JYFMC as 交易方名称,b.I_PTGLJG as 业务管理部门,b.I_YWGLBMFL as 业务管理部门分类,(case tab.单据类型 when '预订单' then '--' else '违约扣罚记录' end) as 违约扣罚记录,(case tab.单据类型 when '预订单' then '--' else '100.00' end) as 履约保证金冻结剩余比例 ";
        ht_where["search_tbname"] = " AAA_View_SPDingBiao as tab left join AAA_DLZHXXB as b on tab.交易方账号=b.B_DLYX  "; //检索的表
        ht_where["search_mainid"] = "tab.中标定标编号+tab.单据类型";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件  
        ht_where["search_paixu"] = " ASC ";  //排序方式
        ht_where["search_paixuZD"] = "tab.定标时间";  //用于排序的字段
        return ht_where;
    }
    public void DisGrid()
    {
        Hashtable HTwhere = SetV();
        if (!txt_spmc.Text.Trim().Equals(""))
        {
            HTwhere["search_str_where"] += " and tab.商品名称 like '%" + txt_spmc.Text.Trim() + "%' ";
        }
        if (!ddl_djlb.SelectedItem.ToString().Trim().Equals("选择单据类别"))
        {
            HTwhere["search_str_where"] += " and tab.单据类型 ='" + ddl_djlb.SelectedItem.ToString().Trim() + "' ";
        }
        //if (this.ddlssfgs.SelectedValue.ToString() != "" && this.ddlssfgs.SelectedValue.ToString() != "无")
        //{
        //    HTwhere["search_str_where"] += "  and 所属分公司 = '" + this.ddlssfgs.SelectedValue.ToString() + "'";
        //}

        if (this.UCFWJGDetail1 != null )
        {
            if (this.UCFWJGDetail1.Value[0].ToString().Trim() != "")
            {
                HTwhere["search_str_where"] += " and b.I_YWGLBMFL='" + this.UCFWJGDetail1.Value[0].ToString() + "'";
            }
            if (this.UCFWJGDetail1.Value[1].ToString().Trim() != "")
            {
                HTwhere["search_str_where"] += " and b.I_PTGLJG='" + this.UCFWJGDetail1.Value[1].ToString().Trim() + "' ";
            }
        }

        if (txtjjrbh.Text.Trim() != "" && txtjjrbh.Text.Trim() != "无")
        {
            HTwhere["search_str_where"] += " and b.I_JYFMC like '%" + txtjjrbh.Text.Trim() + "%'";
        }

        if (txtjjrxm.Text.Trim() != "" && txtjjrxm.Text.Trim() != "无")
        {
            HTwhere["search_str_where"] += " and tab.交易方账号 like '%" + txtjjrxm.Text.Trim() + "%'";
        }

        // HTwhere["search_str_where"] += "  and 所属分公司 = '" + this.ddlssfgs.SelectedValue.ToString() + "' and 经纪人编号 like '%" + txtjjrbh.Text.Trim() + "%' and 经纪人名称 like '%" + txtjjrxm.Text.Trim() + "%'";
        this.hidwhere.Value = HTwhere["search_str_where"].ToString();
        //  hidwhereis.Value = HTwhere["search_str_where"].ToString();
        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DisGrid();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.rptSPXX != null && this.rptSPXX.Items.Count > 0)
        {
            //StringBuilder stringBuilder = new StringBuilder();

            //stringBuilder.Append(this.hidwhereis.Value);
            //if (!this.hidwhere.Value.ToString().Equals(""))
            //{
            //    stringBuilder.Append(" where " + this.hidwhere.Value.ToString());

            //}
            //else
            //{
            //    stringBuilder.Append(" where 1!=1 ");
            //}
            //stringBuilder.Append(" order by 下单时间 desc ");
            //// stringBuilder.Append(" where " + hidwhereis.Value.ToString());
            //DataSet dataSet = DbHelperSQL.Query(stringBuilder.ToString());

            string sql = "select b.I_PTGLJG as 业务管理部门,tab.交易方账号,b.I_JYFMC as 交易方名称,tab.定标时间,tab.合同编号,tab.下单时间,tab.单据类型,tab.单据编号,tab.商品编号,tab.商品名称,tab.规格,tab.合同期限,tab.定标数量,tab.定标价格,tab.投标保证金解冻金额,tab.履约保证金冻结金额,(case tab.单据类型 when '预订单' then '--' else '100.00' end) as 履约保证金冻结剩余比例,tab.是否拆单,tab.中标定标编号,b.I_YWGLBMFL as 业务管理部门分类 from AAA_View_SPDingBiao as tab left join AAA_DLZHXXB as b on tab.交易方账号=b.B_DLYX where " + hidwhere.Value.ToString() + " order by tab.定标时间";
            DataSet dataSet = DbHelperSQL.Query(sql);
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                if (dataSet.Tables[0].Rows[i]["单据类型"].ToString() == "投标单")
                {
                    //获取履约保证金剩余比率
                    object obj_lybzj = DbHelperSQL.GetSingle("select sum(a.JE) from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.number where SJLX='预' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息') and LYYWLX='AAA_THDYFHDXXB' and b.ZBDBXXBBH='" + dataSet.Tables[0].Rows[i]["中标定标编号"].ToString() + "'");
                    if (obj_lybzj != null && obj_lybzj.ToString() != "")
                    {
                        dataSet.Tables[0].Rows[i]["履约保证金冻结剩余比例"] = ((1 - Convert.ToDouble(obj_lybzj.ToString()) / Convert.ToDouble(dataSet.Tables[0].Rows[i]["履约保证金冻结金额"])) * 100.00).ToString("#0.00");
                    }
                }
            }           
            dataSet.Tables[0].Columns.Remove("业务管理部门分类");
            dataSet.Tables[0].Columns.Remove("中标定标编号");
            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "定标与保证函", "定标与保证函", 15);
        }
        else
        {
            MessageBox.Show(this, "列表中没有数据可导出！");
        }


    }
    protected void rptSPXX_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Linkht")//电子购货合同
        {
            string url = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/pingtaiservices/moban/FMPTDZGHHT.aspx?Number=" + e.CommandArgument.ToString();
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script language='javascript' >window.open('" + url + "');</script>");
        }
        if (e.CommandName == "linkbzh")
        {
            string str = "http://" + ConfigurationManager.ConnectionStrings["onlineJHJX"].ToString() + "/pingtaiservices/moban/BZH.aspx?moban=BZH.htm&Number=" + e.CommandArgument.ToString().Trim();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "", " <script type='text/JavaScript'>window.open('" + str + "'); </script>");
        }
        if (e.CommandName == "linkSee")
        {
            //查看详情
            string Strurl = "jhjix_ckxq.aspx?Number="+ e.CommandArgument.ToString();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", " <script type='text/JavaScript'>window.open('" + Strurl + "'); </script>");

        }
        if (e.CommandName == "linwykf")
        {
            //违约记录
            Label lb = null;
            lb = e.Item.FindControl("labdjlx") as Label;

            if (lb.Text.Equals("投标单"))
            {
                lb = e.Item.FindControl("labdjbl") as Label;
                if (Convert.ToDouble(lb.Text) != 100)
                {
                    string Strurl = " jhjx_wykfjuk.aspx?Number=" + e.CommandArgument.ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", " <script type='text/JavaScript'>window.open('" + Strurl + "'); </script>");
                }
                else
                {
                    MessageBox.Show(this, "单据没有违约记录可查询！");
                }
            }
            else
            {
                MessageBox.Show(this, "预订单没有记录可查询！");
            }
        }

    }
}