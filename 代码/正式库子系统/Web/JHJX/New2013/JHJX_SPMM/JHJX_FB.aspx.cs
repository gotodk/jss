using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Hesion.Brick.Core;

public partial class Web_JHJX_New2013_JHJX_SPMM_JHJX_FB : System.Web.UI.Page
{
    protected static DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpagernew1.OnNeedLoadData += new Web_pagerdemo_commonpagernew.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        if (!IsPostBack)
        {
            UCFWJGDetail1.initdefault();
            DisGrid();

        }

    }

    //protected string GetSql()
    //{
    //    //return " (( select 'T'+a.Number+b.Number as 主键, 'T'+a.Number as Number , case b.Z_HTZT when '未定标废标' then CONVERT(varchar(100),b.Z_FBSJ , 20)  when '定标合同终止' then  CONVERT(varchar(100),Z_QPKSSJ, 20) end  as 废标时间,case b.Z_HTZT when '未定标废标' then '卖家未履行定标程序' when '定标合同终止' then '卖家履约保证金不足60%' end as 废标原因,b.Z_HTBH as 合同编号 , b.Number as 中标定标号, CONVERT(varchar(100),a.CreateTime , 20) as  下单时间,	'投标单' as  单据类型,	a.Number as 单据编号,	 a.SPBH as  商品编号, a.SPMC as 商品名称,a.GG as 规格, (case b.Z_HTZT when '未定标废标' then b.Z_ZBSL when '定标合同终止' then  b.Z_ZBSL-( select  isnull(sum (T_THSL),0) from  AAA_THDYFHDXXB  where ZBDBXXBBH = b.Number and F_DQZT not  in ( '撤销','未生成发货单','已生成发货单')  ) end  ) as 废标数量,b.Z_ZBJG as 废标价格,b.Z_ZBJG* (case b.Z_HTZT when '未定标废标' then b.Z_ZBSL when '定标合同终止' then  b.Z_ZBSL-( select  isnull(sum (T_THSL),0) from  AAA_THDYFHDXXB  where ZBDBXXBBH = b.Number and F_DQZT not  in ( '撤销','未生成发货单','已生成发货单')  ) end  ) as 废标金额,	(case  b.Z_HTZT when '未定标废标' then cast(b.T_YSTBDDJTBBZJ as varchar(200)) when '定标合同终止' then '--' end )   as 投标保证金扣罚金额,'--' as 订金解冻金额, cast((isnull(cast(( select   sum(JE) from AAA_ZKLSMXB where LYDH in(b.Number) and SJLX = '实' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息')) as varchar(200)),'--')) as varchar(200)) as	履约保证金扣罚金额, cast(cast(((isnull(( select   sum(JE) from AAA_ZKLSMXB where LYDH in (  select Number  from  AAA_THDYFHDXXB where  ZBDBXXBBH = b.Number) and SJLX = '预' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息')),0.00)/b.Z_LYBZJJE)*100) as numeric(18, 2)) as varchar(200))  as '履约保证金扣罚比率',case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != TBNSL then '是' when YZBSL>0 and YZBSL = TBNSL and (select Count(Number) from AAA_ZBDBXXB where T_YSTBDBH =a.Number)>1 then '是' else '否'  end   as  是否拆单,    (select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 交易方名称 ,a.GLJJRPTGLJG as 所属分公司 ,a.DLYX as 交易方账号  from AAA_ZBDBXXB as b left join AAA_TBD as a on a.Number = b.T_YSTBDBH   where (b.Z_HTZT='未定标废标' or b.Z_HTZT='定标合同终止'  ) and a.MJJSBH = b.T_YSTBDMJJSBH ) union  (select 'Y'+a.Number+b.Number as 主键,  'Y'+a.Number as Number ,  case b.Z_HTZT when '未定标废标' then CONVERT(varchar(100),b.Z_FBSJ , 20)  when '定标合同终止' then CONVERT(varchar(100),Z_QPKSSJ, 20) end  as 废标时间,case b.Z_HTZT when '未定标废标' then '卖家未履行定标程序' when '定标合同终止' then '卖家履约保证金不足60%' end as 废标原因,b.Z_HTBH as 合同编号 , b.Number as 中标定标号 ,  CONVERT(varchar(100),a.CreateTime , 20) as  下单时间,	'预订单' as  单据类型,	a.Number as 单据编号,a.SPBH as  商品编号, a.SPMC as 商品名称,a.GG as 规格, (case b.Z_HTZT when '未定标废标' then b.Z_ZBSL when '定标合同终止' then  b.Z_ZBSL-( select  isnull(sum (T_THSL),0) from  AAA_THDYFHDXXB  where ZBDBXXBBH = b.Number and F_DQZT not  in ( '撤销','未生成发货单','已生成发货单')  ) end  ) as 废标数量,b.Z_ZBJG as 废标价格,b.Z_ZBJG* (case b.Z_HTZT when '未定标废标' then b.Z_ZBSL when '定标合同终止' then  b.Z_ZBSL-( select  isnull(sum (T_THSL),0) from  AAA_THDYFHDXXB  where ZBDBXXBBH = b.Number and F_DQZT not  in ( '撤销','未生成发货单','已生成发货单')  ) end  ) as 废标金额,'--' as 投标保证金扣罚金额,cast( ISNULL( a.DJDJ ,0.00) as varchar(200) ) as 订金解冻金额,'--' as	履约保证金扣罚金额,	'--' as 履约保证金扣罚比率,(case when a.NDGSL=a.YZBSL then '否' else '是' end ) as 是否拆单,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 交易方名称,a.GLJJRPTGLJG as 买方所属分公司,a.DLYX as 交易方账号 from AAA_ZBDBXXB as b left join AAA_YDDXXB as a on a.Number = b.Y_YSYDDBH  where (b.Z_HTZT='未定标废标' or b.Z_HTZT='定标合同终止'  ) and b.Y_YSYDDMJJSBH=a.MJJSBH)) as tab ";

    //    //优化后的语句
    //    return "(( select 'T'+a.Number+b.Number as 主键, 'T'+a.Number as Number ,  case b.Z_HTZT when '未定标废标' then CONVERT(varchar(100),b.Z_FBSJ , 20)  when '定标合同终止' then  CONVERT(varchar(100),Z_QPKSSJ, 20) end  as 废标时间,case b.Z_HTZT when '未定标废标' then '卖家未履行定标程序' when '定标合同终止' then '卖家履约保证金不足60%' end as 废标原因,b.Z_HTBH as 合同编号 , b.Number as 中标定标号,	CONVERT(varchar(100),a.CreateTime , 20) as  下单时间,'投标单' as  单据类型,	a.Number as 单据编号, a.SPBH as  商品编号, 	a.SPMC as 商品名称,	a.GG as 规格, isnull((case b.Z_HTZT when '未定标废标' then  b.Z_ZBSL when '定标合同终止' then isnull( b.Z_ZBSL,0)-c.sjsl end  ) ,0) as 废标数量,b.Z_ZBJG as 废标价格,	b.Z_ZBJG* isnull((case b.Z_HTZT when '未定标废标' then  b.Z_ZBSL when '定标合同终止' then isnull( b.Z_ZBSL,0)-c.sjsl end  ) ,0) as 废标金额,(case  b.Z_HTZT when '未定标废标' then b.T_YSTBDDJTBBZJ  when '定标合同终止' then 0.00 end )   as 投标保证金扣罚金额,0.00 as 订金解冻金额, isnull(d.ylkfje,0.00) as	履约保证金扣罚金额,   cast((isnull(e.kf,0.00)/b.Z_LYBZJJE)*100 as  numeric(18, 2))   as 履约保证金扣罚比率,	   case  when YZBSL =0 then '否' when YZBSL>0 and YZBSL != TBNSL then '是' when YZBSL>0 and YZBSL = TBNSL and (select Count(Number) from AAA_ZBDBXXB where T_YSTBDBH =a.Number)>1 then '是' else '否'  end   as  是否拆单,  AAA_DLZHXXB.I_JYFMC  as 交易方名称 ,isnull(I_YWGLBMFL,'') as 部门分类, a.GLJJRPTGLJG as 所属分公司 ,a.DLYX as 交易方账号  from AAA_ZBDBXXB as b left join AAA_TBD as a on a.Number = b.T_YSTBDBH left join (select ZBDBXXBBH, isnull(sum (T_THSL),0) as sjsl from  AAA_THDYFHDXXB  where F_DQZT not  in ( '撤销','未生成发货单','已生成发货单') group by ZBDBXXBBH) c on c.ZBDBXXBBH = b.Number left join ( select LYDH , isnull (sum(JE),0) as ylkfje from AAA_ZKLSMXB where  SJLX = '实' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息') group by LYDH) d on d.LYDH= b.Number left join (select ZBDBXXBBH, isnull (sum(JE),0) as kf from AAA_ZKLSMXB left join AAA_THDYFHDXXB on LYDH= AAA_THDYFHDXXB.Number  where  SJLX = '预' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息') group by ZBDBXXBBH) e on e.ZBDBXXBBH =b.Number left join AAA_DLZHXXB on B_DLYX= b.T_YSTBDDLYX   where (b.Z_HTZT='未定标废标' or b.Z_HTZT='定标合同终止'  ) and a.MJJSBH = b.T_YSTBDMJJSBH ) union (select 'Y'+a.Number+b.Number as 主键,  'Y'+a.Number as Number , case b.Z_HTZT when '未定标废标' then CONVERT(varchar(100),b.Z_FBSJ , 20)  when '定标合同终止' then CONVERT(varchar(100),Z_QPKSSJ, 20) end  as 废标时间,	case b.Z_HTZT when '未定标废标' then '卖家未履行定标程序' when '定标合同终止' then '卖家履约保证金不足60%' end as 废标原因,b.Z_HTBH as 合同编号 ,	b.Number as 中标定标号 , CONVERT(varchar(100),a.CreateTime , 20) as  下单时间,	'预订单' as  单据类型,a.Number as 单据编号,a.SPBH as  商品编号, a.SPMC as 商品名称,a.GG as 规格, isnull((case b.Z_HTZT when '未定标废标' then b.Z_ZBSL when '定标合同终止' then  isnull(b.Z_ZBSL,0)-c.sjsl  end  ),0)  as 废标数量,b.Z_ZBJG as 废标价格,	b.Z_ZBJG* isnull((case b.Z_HTZT when '未定标废标' then b.Z_ZBSL when '定标合同终止' then  isnull(b.Z_ZBSL,0)-c.sjsl  end  ),0)  as 废标金额,0.00 as 投标保证金扣罚金额,ISNULL( a.DJDJ ,0.00) as 订金解冻金额,0.00 as	履约保证金扣罚金额,	0.00  as 履约保证金扣罚比率,	(case when a.NDGSL=a.YZBSL then '否' else '是' end ) as 是否拆单, I_JYFMC as 交易方名称,isnull(I_YWGLBMFL,'') as 部门分类, a.GLJJRPTGLJG as 所属分公司,a.DLYX as 交易方账号 from AAA_ZBDBXXB as b left join AAA_YDDXXB as a on a.Number = b.Y_YSYDDBH   left join (select ZBDBXXBBH, isnull(sum (T_THSL),0) as sjsl from  AAA_THDYFHDXXB  where F_DQZT not  in ( '撤销','未生成发货单','已生成发货单') group by ZBDBXXBBH) c on c.ZBDBXXBBH = b.Number  left join AAA_DLZHXXB on B_DLYX= b.Y_YSYDDDLYX   where (b.Z_HTZT='未定标废标' or b.Z_HTZT='定标合同终止'  ) and b.Y_YSYDDMJJSBH=a.MJJSBH)) as tab ";

    //}

    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        //ht_where["serach_Row_str"] = " * ";
        //ht_where["search_tbname"] = GetSql();
        //ht_where["search_str_where"] = " 1=1 ";  //检索条件
        //ht_where["search_mainid"] = " 主键 ";  //所检索表的主键
        //ht_where["search_paixu"] = " desc ";  //排序方式
        //ht_where["search_paixuZD"] = " 废标时间";  //用于排序的字段
        //ht_where["returnlastpage_open"] = "New2013/JHJX_SPMM/JHJX_FB.aspx";
        
        /*---shiyan 2013-12-16 进行数据获取优化。---*/
        ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage";//这个可以不设置,默认是
        ht_where["serach_Row_str"] = "  tab.*, b.I_JYFMC as 交易方名称,b.I_PTGLJG as 所属分公司,b.I_YWGLBMFL as 业务管理部门,'--' as 履约保证金扣罚金额 ";
        ht_where["search_tbname"] = " AAA_View_SPFeiBiao as tab left join AAA_DLZHXXB as b on tab.交易方账号=b.B_DLYX ";
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_mainid"] = " tab.中标定标编号+tab.单据类型 ";  //所检索表的主键
        ht_where["search_paixu"] = " desc ";  //排序方式
        ht_where["search_paixuZD"] = " tab.废标时间 ";  //用于排序的字段

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

        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            for (int i = 0; i < NewDS.Tables[0].Rows.Count; i++)
            {
                if (NewDS.Tables[0].Rows[i]["合同状态"].ToString() == "定标合同终止" && NewDS.Tables[0].Rows[i]["单据类型"].ToString() == "投标单")
                {
                    //此种情况需要计算履约保证金扣罚金
                    object objLYBZJ = DbHelperSQL.GetSingle("select cast(sum(JE) as numeric(18,2)) as 履约保证金扣罚金额 from AAA_ZKLSMXB where SJLX='实' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息'  or XZ='电子购货合同履约保证金不足废标扣罚' or XZ='电子购货合同期满卖方未录入发货信息扣罚') and LYYWLX='AAA_ZBDBXXB' and LYDH='" + NewDS.Tables[0].Rows[i]["中标定标编号"].ToString() + "'");
                    if (objLYBZJ != null && objLYBZJ.ToString() != "")
                    {
                        NewDS.Tables[0].Rows[i]["履约保证金扣罚金额"] = objLYBZJ.ToString();
                    }
                }
            }
            tdEmpty.Visible = false;
            dt = NewDS.Tables[0];
            rptfb.DataSource = dt.DefaultView;
        }
        else 
        {
            rptfb.DataSource = null;
            tdEmpty.Visible = true;
        }
        rptfb.DataBind();
    }
    public void DisGrid()
    {
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        //string sql_where = " and 部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%'   and 交易方账号 like '%" + txtjyfzh.Text.ToString().Trim() + "%'  and 所属分公司 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 交易方名称 like '%" + txtjyfmc.Text.Trim() + "%' and 单据类型 like '%" + ddldjxz.SelectedValue.ToString().Trim() + "%' and 商品名称 like '%" + txtspmc.Text.Trim() + "%'";
       // HTwhere["search_str_where"] = HTwhere["search_str_where"] + sql_where;

        /*---shiyan 2013-12-19 优化数据获取方式---*/
        //交易方业务管理机构
        if (UCFWJGDetail1.Value[0].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.I_YWGLBMFL='" + UCFWJGDetail1.Value[0].ToString().Trim() + "' ";
        }
        if (UCFWJGDetail1.Value[1].ToString() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.I_PTGLJG='" + UCFWJGDetail1.Value[1].ToString().Trim() + "' ";
        }
       
        //买家名称
        if (txtjyfmc.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and b.I_JYFMC like '%" + txtjyfmc.Text.ToString().Trim() + "%' ";
        }
        //卖家名称
        if (txtjyfzh.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and tab.交易方账号 like '%" + txtjyfzh.Text.ToString().Trim() + "%' ";
        }
        //电子购货合同编号
        if (txtspmc.Text.Trim() != "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and tab.商品名称 like '%" + txtspmc.Text.ToString().Trim() + "%' ";
        }
        //单据类别
        if (ddldjxz.SelectedValue.ToString()!= "")
        {
            HTwhere["search_str_where"] = HTwhere["search_str_where"] + " and tab.单据类型='" + ddldjxz.SelectedValue.ToString() + "' ";
        }    
        ViewState["ht_where"] = HTwhere["search_str_where"];
        /*---shiyan 结束---*/

        commonpagernew1.HTwhere = HTwhere;
        commonpagernew1.GetFYdataAndRaiseEvent();
    }


    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        DisGrid();

    }
    protected void btnDC_Click(object sender, EventArgs e)
    {
        //DisGrid(); 
        //string sql = "select * from" + GetSql() + " where  部门分类 like '%" + UCFWJGDetail1.Value[0].ToString().Trim() + "%' and  交易方账号 like '%" + txtjyfzh.Text.ToString().Trim() + "%'  and 所属分公司 like '%" + UCFWJGDetail1.Value[1].ToString().Trim() + "%' and 交易方名称 like '%" + txtjyfmc.Text.Trim() + "%' and 单据类型 like '%" + ddldjxz.SelectedValue.ToString().Trim() + "%' and 商品名称 like '%" + txtspmc.Text.Trim() + "%' ";
        /*---shiyan 2013-12-19 优化数据获取方式---*/
        string sql = "select b.I_PTGLJG as 所属分公司,tab.交易方账号,b.I_JYFMC as 交易方名称,tab.合同编号 as 电子购货合同编号,tab.废标时间,tab.废标原因,tab.下单时间,tab.商品编号,tab.商品名称,tab.规格,tab.合同期限,tab.数量,tab.价格,tab.金额,tab.单据类型,tab.单据编号,tab.订金解冻金额,tab.投标保证金扣罚金额,'--' as 履约保证金扣罚金额,tab.是否拆单,b.I_YWGLBMFL as 业务管理部门,tab.合同状态,tab.中标定标编号 from AAA_View_SPFeiBiao as tab left join AAA_DLZHXXB as b on tab.交易方账号=b.B_DLYX where " + ViewState["ht_where"].ToString() + "order by 废标时间 DESC";
        DataSet dataSet = DbHelperSQL.Query(sql);

        DataTable dtable = dataSet.Tables[0];

        if (dtable != null && dtable.Rows.Count > 0)
        {
            
          
            for (int i = 0; i < dtable.Rows.Count; i++)
            {
                if (dtable.Rows[i]["合同状态"].ToString() == "定标合同终止" && dtable.Rows[i]["单据类型"].ToString() == "投标单")
                {
                    //此种情况需要计算履约保证金扣罚金
                    object objLYBZJ = DbHelperSQL.GetSingle("select sum(JE) as 履约保证金扣罚金额 from AAA_ZKLSMXB where SJLX='实' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息'  or XZ='电子购货合同履约保证金不足废标扣罚' or XZ='电子购货合同期满卖方未录入发货信息扣罚') and LYYWLX='AAA_ZBDBXXB' and LYDH='" + dtable.Rows[i]["中标定标编号"].ToString() + "'");
                    if (objLYBZJ != null && objLYBZJ.ToString() != "")
                    {
                        dtable.Rows[i]["履约保证金扣罚金额"] = objLYBZJ.ToString();
                    }
                }
            }


            //dtable.Columns.Remove("主键");
            //dtable.Columns.Remove("Number");
            //dtable.Columns.Remove("履约保证金扣罚比率");
            //dtable.Columns.Remove("合同编号");
            //dtable.Columns.Remove("中标定标号");
            //dtable.Columns.Remove("部门分类");            
          
            dtable.Columns.Remove("合同状态");
            dtable.Columns.Remove("业务管理部门");
            dtable.Columns.Remove("中标定标编号");
           // dtable.Columns["MB001"].ColumnName = "品号";   
            //dtable.Columns["批号"].SetOrdinal(4);
           // DataTableToExcel(dtable);
         

            MyXlsClass MXC = new MyXlsClass();

            MXC.goxls(dataSet, "Report_" + DateTime.Now.ToString("yyyy-MM-dd:hh-mm-sss") + ".xls", "废标", "废标", 15);
        }
        else
        {
            MessageBox.ShowAlertAndBack(this, "没有可以导出的数据，请重新进行查询！");

        }


    }

    // 将数据导出到excel，与下面的函数同时使用才能正常工作
    public void ToExcel(System.Web.UI.Control ctl)
    {
        HttpContext.Current.Response.Clear();

        HttpContext.Current.Response.Charset = "";
        string filename = "JYPT_YXDTHD" + System.DateTime.Now.ToString("_yyyyMMddHHmm");
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" +

        System.Web.HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8) + ".xls");

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
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

    public static void DataTableToExcel(System.Data.DataTable dtData)
    {
        System.Web.UI.WebControls.DataGrid dgExport = null;
        // 当前对话
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        // IO用于导出并返回excel文件
        System.IO.StringWriter strWriter = null;
        System.Web.UI.HtmlTextWriter htmlWriter = null;

        if (dtData != null)
        {
            // 设置编码和附件格式

         
            string style = @"<style> .text { mso-number-format:\@; } </script> "; //设置格式，解决电话不显示开头是0的问题
            string FileName = "";
            FileName = "Report-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
            curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
            curContext.Response.Charset = "UTF-8";
            curContext.Response.ContentEncoding = System.Text.Encoding.Default;
            curContext.Response.ContentType = "application/ms-excel";

            // 导出excel文件
            strWriter = new System.IO.StringWriter();
            htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

            // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid
            dgExport = new System.Web.UI.WebControls.DataGrid();
            dgExport.DataSource = dtData.DefaultView;
            dgExport.AllowPaging = false;
            dgExport.DataBind();

            // 返回客户端
            dgExport.RenderControl(htmlWriter);
            curContext.Response.Write(style);//调用格式化字符串
            curContext.Response.Write(strWriter.ToString());
            curContext.Response.End();
        }
    }

    protected void rptfb_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lbspmc = (Label)e.Item.FindControl("lbspmc");
        Label lbfbyy = (Label)e.Item.FindControl("lbfbyy");
        Label lbjyfmc = (Label)e.Item.FindControl("lbjyfmc");
        

        if (lbspmc.Text.Length > 15)
            lbspmc.Text = lbspmc.Text.Substring(0, 15) + "...";
        if (lbfbyy.Text.Length > 15)
            lbfbyy.Text = lbfbyy.Text.Substring(0, 15) + "...";
        if (lbjyfmc.Text.Length > 15)
            lbjyfmc.Text = lbjyfmc.Text.Substring(0, 15) + "...";
        

    }


    protected void txtjyfmc_TextChanged(object sender, EventArgs e)
    {

    }
}