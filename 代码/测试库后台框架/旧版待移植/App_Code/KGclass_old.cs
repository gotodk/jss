using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using FMOP.DB;
/// <summary>
///可用剩余旧硒鼓统计分析类
/// </summary>
public class KGclass_old
{
    #region 测试信息
    public DataTable ceshidt1()
    {
        DataTable dte;
        dte = new DataTable("所有空鼓入库的信息");
        DataColumn x1 = new DataColumn("客户编号", typeof(string));
        DataColumn x2 = new DataColumn("空鼓类型", typeof(string));
        DataColumn x3 = new DataColumn("空鼓规格", typeof(string));
        DataColumn x4 = new DataColumn("剩余总量", typeof(int));
        DataColumn x5 = new DataColumn("空鼓品号", typeof(string));
        dte.Columns.Add(x1);
        dte.Columns.Add(x2);
        dte.Columns.Add(x3);
        dte.Columns.Add(x4);
        dte.Columns.Add(x5);
        dte.Rows.Add(new object[] { "1", "蓝装", "fm-2612", 100, "1010" });
        dte.Rows.Add(new object[] { "1", "绿装", "fm-2612", 200, "1010" });
        dte.Rows.Add(new object[] { "1", "原装", "fm-2612", 300, "1010" });
        return dte;

    }
    public DataTable ceshidt2()
    {
        DataTable dte;
        dte = new DataTable("销退单中所有退回押金的信息2410单别");
        DataColumn x1 = new DataColumn("客户编号", typeof(string));
        DataColumn x2 = new DataColumn("空鼓类型", typeof(string));
        DataColumn x3 = new DataColumn("空鼓规格", typeof(string));
        DataColumn x4 = new DataColumn("剩余总量", typeof(int));
        DataColumn x5 = new DataColumn("绿装旧鼓量", typeof(int));
        DataColumn x6 = new DataColumn("蓝装旧鼓量", typeof(int));
        DataColumn x7 = new DataColumn("原装旧鼓量", typeof(int));
        DataColumn x8 = new DataColumn("销退产品类型", typeof(string));
        DataColumn x9 = new DataColumn("销退产品数量", typeof(int));
        dte.Columns.Add(x1);
        dte.Columns.Add(x2);
        dte.Columns.Add(x3);
        dte.Columns.Add(x4);
        dte.Columns.Add(x5);
        dte.Columns.Add(x6);
        dte.Columns.Add(x7);
        dte.Columns.Add(x8);
        dte.Columns.Add(x9);
        dte.Rows.Add(new object[] { "1", "", "fm-2612", 0, 5, 5, 5, "fm-2612tt", 99 });

        return dte;

    }
    public DataTable ceshidt3()
    {
        DataTable dte;
        dte = new DataTable("订货单中所有已提交的循环提货的数据");
        DataColumn x1 = new DataColumn("客户编号", typeof(string));
        DataColumn x2 = new DataColumn("空鼓类型", typeof(string));
        DataColumn x3 = new DataColumn("空鼓规格", typeof(string));
        DataColumn x4 = new DataColumn("剩余总量", typeof(int));
        DataColumn x5 = new DataColumn("绿装旧鼓量", typeof(int));
        DataColumn x6 = new DataColumn("蓝装旧鼓量", typeof(int));
        DataColumn x7 = new DataColumn("原装旧鼓量", typeof(int));
        DataColumn x8 = new DataColumn("订单产品类型", typeof(string));
        DataColumn x9 = new DataColumn("订单产品数量", typeof(int));
        dte.Columns.Add(x1);
        dte.Columns.Add(x2);
        dte.Columns.Add(x3);
        dte.Columns.Add(x4);
        dte.Columns.Add(x5);
        dte.Columns.Add(x6);
        dte.Columns.Add(x7);
        dte.Columns.Add(x8);
        dte.Columns.Add(x9);
        dte.Rows.Add(new object[] { "1", "", "fm-2612", 0, 5, 5, 5, "fm-2612tt", 99 });
        return dte;

    }
    public DataTable ceshidt4()
    {
        DataTable dte;
        dte = new DataTable("积分兑换中所有使用旧硒鼓兑换的数据");
        DataColumn x1 = new DataColumn("客户编号", typeof(string));
        DataColumn x2 = new DataColumn("空鼓类型", typeof(string));
        DataColumn x3 = new DataColumn("空鼓规格", typeof(string));
        DataColumn x4 = new DataColumn("剩余总量", typeof(int));
        DataColumn x5 = new DataColumn("绿装旧鼓量", typeof(int));
        DataColumn x6 = new DataColumn("蓝装旧鼓量", typeof(int));
        DataColumn x7 = new DataColumn("原装旧鼓量", typeof(int));
        DataColumn x8 = new DataColumn("兑换产品类型", typeof(string));
        DataColumn x9 = new DataColumn("兑换产品数量", typeof(int));
        dte.Columns.Add(x1);
        dte.Columns.Add(x2);
        dte.Columns.Add(x3);
        dte.Columns.Add(x4);
        dte.Columns.Add(x5);
        dte.Columns.Add(x6);
        dte.Columns.Add(x7);
        dte.Columns.Add(x8);
        dte.Columns.Add(x9);
        dte.Rows.Add(new object[] { "1", "", "fm-2612", 0, 5, 5, 5, "fm-2612tt", 99 });
        return dte;

    }
    #endregion

    #region 空鼓运算想关
    #region 分公司未独立运行之前服务商的旧硒鼓数据

    // 获取所有空鼓入库的信息（k101+期初）    
    public DataTable GetTable1(string UserNumber)
    {
        //获取所有空鼓入库的信息（k101+期初）;

        //获取期初;
        string sqlQC = "select a.KHBH as 客户编号,b.KGLB as 空鼓类型,(case b.KGLB when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end)  as 空鼓规格,QCJYL as 剩余总量 from CWGL_KHKGSYQKQCSJ as a left join CWGL_KHKGSYQKQCSJ_KGQCXX as b on a.Number=b.parentNumber left join INVMB on ltrim(rtrim(MB001))=b.KGPH left join KGCPDZB on b.KGPH= KGCPDZB.KGPH where a.KHBH='" + UserNumber + "' and a.QCSJ='2011-03-01'";

        //获取所有空鼓入库的信息（k101）;
        string sqlRK = "select ltrim(rtrim(INVTB.UDF01)) as 客户编号, ltrim(rtrim(INVTB.TB029)) as 空鼓类型,(case ltrim(rtrim(INVTB.TB029)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end) as 空鼓规格,cast(TB007 as int) as 剩余总量 from INVTB left join INVTA on TB002=TA002 and TB001=TA001  left join  CMSME on TA004=ME001 left join COPMA on INVTB.UDF01=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k101' and ME002 like '%办事处%' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "' and TA006='Y'  and convert(datetime,TA003)>='2011-03-01 0:00:00' ";

        string sql = "select 客户编号,空鼓类型,空鼓规格, sum(剩余总量) as 剩余总量 from (" + sqlQC + " union all " + sqlRK + " ) as tab1 group by 客户编号,空鼓类型,空鼓规格";
        DataSet dt1 = GetErpData.GetDataSet(sql,"公司总部");
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    //获取k102中,来源(单头的UDF01字段)不为富美直通车的部分
    public DataTable GetTable1_2(string UserNumber)
    {         
        string sql = "select ltrim(rtrim(INVTB.UDF01)) as 客户编号, ltrim(rtrim(INVTB.TB029)) as 空鼓类型, (case ltrim(rtrim(INVTB.TB029)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end) as 空鼓规格,-cast(TB007 as int) as 剩余总量,LTrim(RTrim(TB004)) as 空鼓品号 from INVTB left join INVTA on TB002=TA002 and TB001=TA001 left join  CMSME on TA004=ME001 left join COPMA on INVTB.UDF01=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k102' and ME002 like '%办事处%' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "' and TA006='Y' and INVTA.UDF01 not like '%富美直通车%'  and convert(datetime,TA003)>='2011-03-01 0:00:00'";
        DataSet ds = GetErpData.GetDataSet(sql,"公司总部");
        if (ds != null && ds.Tables != null)
        {
            return ds.Tables[0];
        }
        return null;
    }
   
    //获取k301中来源是富美直通车的数据（单头UDF01为富美直通车），不管审核不审核。
    public DataTable GetTable1_3(string UserNumber)
    {         
        string sql = "select substring(PURMA.UDF01,0,charindex(',',PURMA.UDF01)) as 客户编号, TG005 as 供应商编号, ltrim(rtrim(TH072)) as 空鼓类型, (case ltrim(rtrim(TH072)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end) as 空鼓规格,-cast(TH007 as int) as 剩余总量,LTrim(RTrim(TH004)) as 空鼓品号 from PURTG left join PURTH on TG001=TH001 and TG002=TH002  left join  CMSME on TG049=ME001 left join PURMA on TG005=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TH004)) left join KGCPDZB on KGPH=LTrim(RTrim(TH004))  where TG001='k301' and ME002 like '%办事处%' and ltrim(rtrim(substring(PURMA.UDF01,0,charindex(',',PURMA.UDF01))))='" + UserNumber + "' and PURTG.UDF01 like '%富美直通车%' and convert(datetime,TG003)>='2011-03-01 0:00:00'";
        DataSet ds = GetErpData.GetDataSet(sql,"公司总部");
        if (ds != null && ds.Tables != null)
        {
            return ds.Tables[0];
        }
        return null;
    }
   
   /// 所有退回押金的信息
    public DataTable GetTable2(string UserNumber)
    {       
        string sql = "select ltrim(rtrim(fwsbh)) as 客户编号,'' as 空鼓类型,KGXH as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,cplb as 产品类别,cpsl as 产品数量,cpxh as 产品型号 from CWGL_KGSYMXJLB  where djlb in ('销退单','押金返还') and syfs='返还押金' and fwsbh='" + UserNumber + "'";
        DataSet dt1 = DbHelperSQL.Query(sql);
       
        if (dt1 != null && dt1.Tables!=null)
        {
            return dt1.Tables[0];
        }
        return null;
    }
   
    //订单中循环提货的产品数量
    public DataTable GetTable3(string UserNumber)
    {    
         string sql= "select fwsbh as 客户编号,'' as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,cplb as 订单产品类型,cpsl as 订单产品数量,cpxh as 订单产品型号 from CWGL_KGSYMXJLB where djlb='订单' and syfs='循环提货' and fwsbh='" + UserNumber + "'";
        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables!=null)
        {
            return dt1.Tables[0];
        }
        return null;
    }
    
    //积分兑换中使用空鼓的数量
    public DataTable GetTable4(string UserNumber)
    {
        string sql = "select fwsbh as 客户编号,'' as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,cplb as 产品类别,cpsl as 产品数量,cpxh as 产品型号 from CWGL_KGSYMXJLB where djlb='积分兑换单' and syfs='提货价兑换' and fwsbh='" + UserNumber + "'";

        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    //旧版合格旧硒鼓转让单,包括增加和减少的
    public DataTable GetTable5(string UserNumber)
    {
       
        string sql = "select ZRFKHBH as 客户编号,ZRLX as 空鼓类型,ZRXGXH as 空鼓规格, -ZRSL as 剩余总量 from FWPT_HGJXGZRQRD where ZRFKHBH= '" + UserNumber + "' and  SFSX = '是' and ZRFQR= '是' and QGFQR = '是' union all select QGFKHBH as 客户编号,ZRLX as 空鼓类型,ZRXGXH as 空鼓规格, ZRSL as 剩余总量 from FWPT_HGJXGZRQRD where QGFKHBH='" + UserNumber + "' and  SFSX = '是' and ZRFQR= '是' and QGFQR = '是'";

        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    //合格旧硒鼓转让信息发布，空鼓冻结
    public DataTable GetTable6(string UserNumber)
    {        
        string sql = "select A.khbh as 客户编号,B.JXGLB as 空鼓类型,B.JXGGG as 空鼓规格, -(convert(int, B.JXGSL)-convert(int, B.yzrsl)) as 剩余总量  from FWPT_HGJXGCS as A join FWPT_HGJXGCS_JYXQ as B on A.Number=B.parentNumber  where B.gqsj>getdate() and A.khbh='" + UserNumber + "'  union all select MCFKHBH as 客户编号,LX as 空鼓类型,XH as 空鼓规格, -SYKMSL as 剩余总量   from Newtransfer where MCFKHBH='" + UserNumber + "' and JYZT <>'5' and JYZT <>'6' ";
        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    //新版合格旧硒鼓交易中心数据的计算
    public DataTable GetTable13(string UserNumber)
    {        
        string sql = "select MCKHBH as 客户编号,CJLX as 空鼓类型,CJXH as 空鼓规格, -CJSL as 剩余总量 from SuccedTrade where MCKHBH='" + UserNumber + "' union all select MRKHBH as 客户编号,CJLX as 空鼓类型,CJXH as 空鼓规格, CJSL as 剩余总量  from SuccedTrade where MRKHBH='" + UserNumber + "'";
        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

     //服务商空鼓调整表数据
    public DataTable GetTable14(string UserNumber)
    {
       
        string sql_ru = "select fwsbh as 客户编号,kglb as 空鼓类型,kggg as 空鼓规格,tzsl as 剩余总量 from CWGL_FWSKGTZB where tzlx='调入' and  fwsbh='" + UserNumber + "' and shzt='已审核'";

        string sql_chu = "select fwsbh as 客户编号,kglb as 空鼓类型,kggg as 空鼓规格,-tzsl as 剩余总量 from CWGL_FWSKGTZB where tzlx='调出' and fwsbh='" + UserNumber + "' and shzt='已审核'";

        string sql = "select 客户编号,空鼓类型,空鼓规格,sum(剩余总量) as 剩余总量 from ( " + sql_ru + " union all " + sql_chu + ") as tab group by 客户编号,空鼓类型,空鼓规格";
        DataSet dt = DbHelperSQL.Query(sql);
        if (dt != null && dt.Tables != null)
        {
            return dt.Tables[0];
        }
        return null;
    }
    
    //用户直通车用户选择服务商供货循环提货时转移至服务商的空鼓,因为转换的时候加了一遍负号，而这一项应该是加到空鼓里面去的，所以此处先加一遍负号，负负得正。
    public DataTable GetTable15(string UserNumber)
    {        
        string sql = "select fwsbh as 客户编号,'' as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,-yz as 原装旧鼓量,-lanz as 蓝装旧鼓量,-lvz as 绿装旧鼓量,cplb as 订单产品类型,cpsl as 订单产品数量,cpxh as 订单产品型号 from CWGL_KGSYMXJLB where djly='用户直通车' and djlb='服务商供货订单' and syfs='用户循环提货' and fwsbh='" + UserNumber + "'";
        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    /// <summary>
    /// 合并多个数据表，根据特定规则
    /// </summary>   
    public DataTable MergeTable(DataTable dt1, DataTable dt1_2, DataTable dt1_3, DataTable dt2, DataTable dt3, DataTable dt4, DataTable dt5, DataTable dt6, DataTable dt7, DataTable dt8, DataTable dt9, DataTable dt10, DataTable dt11, DataTable dt12, DataTable dt13, DataTable dt14, DataTable dt15, DataTable dt15_2, DataTable dt15_3)
    {
        DataTable dtmt = dt1.Clone();
        dtmt.Merge(dt1, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt1_2, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt1_3, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt2, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt3, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt4, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt5, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt6, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt7, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt8, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt9, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt10, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt11, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt12, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt13, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt14, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt15, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt15_2, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt15_3, false, MissingSchemaAction.Ignore);
        return dtmt;
    }

    #endregion

    #region 分公司独立运行之后服务商的旧硒鼓数据
  
    //从业务操作平台中获取分公司上线时的期初数据
    public DataTable GetQiChu(string UserNumber)
    {
        string sqlQC = "select ltrim(rtrim(a.KHBH)) as 客户编号,KGLB as 空鼓类型, b.KGGG as 空鼓规格,sum(QCJYL) as 剩余总量 from CWGL_KHKGSYQKQCSJ_KGQCXX as b left join CWGL_KHKGSYQKQCSJ as a on b.parentNumber=a.Number left join system_city as c on a.SSBSC=c.name where a.QCSJ='2012-09-01' and a.KHBH='" + UserNumber + "' group by a.KHBH,KGLB,KGGG";
        DataSet ds = DbHelperSQL.Query(sqlQC);
        if (ds != null && ds.Tables != null)
        {
            return ds.Tables[0];
        }
        return null;
    }

    /// 获取空鼓的ERP入库、出库信息信息，只取分公司自己的账套系统中的数据   
    public DataTable GetERPChuAndRu(string UserNumber)
    {      
        //获取服务商所属办事处

        object objCity = DbHelperSQL.GetSingle("select ssbsc from FWPT_YHXXB where khbh='" + UserNumber + "'");
        string ssbsc = "";
        if (objCity != null && objCity.ToString() != "")
        {
            ssbsc = objCity.ToString();
        }
        //获取所有空鼓入库的信息（k101）;
        string sqlRK = "select ltrim(rtrim(INVTB.UDF01)) as 客户编号,ltrim(rtrim(TB029)) as 空鼓类型,(case ltrim(rtrim(TB029)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end) as 空鼓规格,cast(TB007 as int) as 剩余总量  from INVTB left join INVTA on TB002=TA002 and TB001=TA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k101' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "'  and TA006='Y'";

        //获取所有空鼓入库的信息（k102）;
        string sqlCK = "select ltrim(rtrim(INVTB.UDF01)) as 客户编号,ltrim(rtrim(TB029)) as 空鼓类型, (case ltrim(rtrim(TB029)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end) as 空鼓规格,-cast(TB007 as int) as 剩余总量 from INVTB left join INVTA on TB002=TA002 and TB001=TA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k102' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "' and TA006='Y'";


        //入库k101、出库k102的数据
        string sql = "select 客户编号,空鼓类型,空鼓规格,sum(剩余总量) as 剩余总量 from (" + sqlRK + " union all " + sqlCK + " ) as tab1  group by 客户编号,空鼓类型,空鼓规格";


        DataSet dt1 = GetErpData.GetDataSet(sql,ssbsc);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    //获取服务商空鼓使用明细表中的所有使用数据
    public DataTable GetKGSYMX(string UserNumber)
    {
        string sqlFHSQ = "select fwsbh as 客户编号,(case 空鼓类别 when 'YZ' then '原装' when 'LANZ' then '蓝装' when 'LVZ' then '绿装' end) as 空鼓类型,kgxh as 空鼓规格,-sum(空鼓数量) as 剩余总量   from (select * from CWGL_KGSYMXJLB) as t unpivot (空鼓数量 for 空鼓类别 in ([YZ],[LANZ],[LVZ])) as r where isnull(YWSLF,'')='' and fwsbh= '" + UserNumber + "' group by FWSBH,空鼓类别,kgxh having SUM(空鼓数量)>0";
        DataSet dt1 = DbHelperSQL.Query(sqlFHSQ);

        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }   

    //新版交易大厅的买入、卖出数据
    public DataTable GetJY(string UserNumber)
    {
        string sql_sale = "select MCKHBH as 客户编号,CJLX as 空鼓类型,CJXH as 空鼓规格,-CJSL as 结余 from SuccedTrade where isnull(MCYWSLF,'')='' and MCKHBH='" + UserNumber + "'";
        string sql_buy = "select  MRKHBH as 客户编号,CJLX as 空鼓类型,CJXH as 空鼓规格,CJSL as 结余 from SuccedTrade  where isnull(MRYWSLF,'')='' and  MRKHBH='" + UserNumber + "'";

        string sql = "select 客户编号,空鼓类型,空鼓规格,sum(结余) as 剩余总量 from(" + sql_sale + " union all " + sql_buy + ") as tab1 group by 客户编号,空鼓类型,空鼓规格";
        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    //交易冻结
    public DataTable GetJYDJ(string UserNumber)
    {
        string sql = "select MCFKHBH as 客户编号,LX as 空鼓类型,XH as 空鼓规格, -sum(SYKMSL) as 剩余总量 from Newtransfer where isnull(YWSLF,'')='' and  MCFKHBH='" + UserNumber + "' and JYZT <>'5' and JYZT <>'6' group by mcfkhbh,lx,xh";

        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    //空鼓调整数据
    public DataTable GetTiaoZheng(string UserNumber)
    {
        string sql_ru = "select fwsbh as 客户编号,kglb as 空鼓类型,kggg as 空鼓规格,tzsl as 结余 from CWGL_FWSKGTZB where tzlx='调入' and isnull(YWSLF,'')='' and fwsbh='" + UserNumber + "' and shzt='已审核'";

        string sql_chu = "select fwsbh as 客户编号,kglb as 空鼓类型,kggg as 空鼓规格,-tzsl as 结余 from CWGL_FWSKGTZB where tzlx='调出' and isnull(YWSLF,'')='' and fwsbh='" + UserNumber + "' and shzt='已审核'";

        string sql = "select 客户编号,空鼓类型,空鼓规格,sum(结余) as 剩余总量 from ( " + sql_ru + " union all " + sql_chu + ") as tab group by 客户编号,空鼓类型,空鼓规格";

        DataSet ds = DbHelperSQL.Query(sql);
        if (ds != null && ds.Tables != null)
        {
            return ds.Tables[0];
        }
        return null;
    }

    /// <summary>
    /// 合并多个数据表，根据特定规则
    /// </summary>
    /// <param name="dt1"></param>
    /// <param name="dt2"></param>
    /// <param name="dt3"></param>
    /// <param name="dt4"></param>
    /// <returns></returns>
    public DataTable MergeTable(DataTable dt1, DataTable dt2, DataTable dt3, DataTable dt4, DataTable dt5, DataTable dt6)
    {
        DataTable dtmt = dt1.Clone();
        dtmt.Merge(dt1, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt2, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt3, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt4, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt5, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt6, false, MissingSchemaAction.Ignore);      
        return dtmt;
    }

    #endregion

    /// <summary>
    /// 重新处理数据集，处理类型和剩余总量
    /// </summary>
    /// <param name="dtre">要处理的数据集</param>
    /// <param name="lx_str">要处理的类型</param>
    /// <returns></returns>
    public DataTable revalue(DataTable dtre, string lx_str)
    {
        DataTable dtre_copy = dtre.Copy();//空鼓类型
        for (int y = 0; y < dtre_copy.Rows.Count; y++)
        {
            dtre_copy.Rows[y]["空鼓类型"] = lx_str;
            string jgl = dtre.Rows[y][lx_str + "旧鼓量"].ToString();
            int fujgl = 0;
            if (jgl.Trim() != "")
            {
                fujgl = Convert.ToInt32(jgl);
            }
            dtre_copy.Rows[y]["剩余总量"] = -fujgl;
        }
        return dtre_copy;
    }
   

    /// <summary>
    /// 获取某客户所有可以使用的空鼓数量,列： 型号，类型，型号，数量
    /// </summary>
    /// <param name="UserNumber">服务商编号</param>
    ///  <param name="dt7">产品列表已验证的部分使用的空鼓信息</param>
    /// <returns></returns>
    public DataTable GetAllKGCanUseByUserNumber(string UserNumber,DataTable dtNow)
    {
        object objDB = DbHelperSQL.GetSingle("select dyERP from fwpt_yhxxb as a left join system_city_0 as b on a.ssbsc=b.name where a.khbh='" + UserNumber + "'");
        string DBname = "";
        if (objDB != null && objDB.ToString().Trim() != "")
        {
            DBname = objDB.ToString();
        }

        DataTable dt_mt = new DataTable();
        //如果对应数据库是fm，表示分公司尚未独立运行，按照老方法计算空鼓
        if (DBname == "fmlx2"||DBname =="fm")
        {
            DataTable dt1 = GetTable1(UserNumber);//空鼓入库和期初
            DataTable dt1_2 = GetTable1_2(UserNumber);//空鼓出库
            DataTable dt1_3 = GetTable1_3(UserNumber);//通过直通车出售给我公司的，k301

            DataTable dt2 = GetTable2(UserNumber);//销退单中押金退回的,2410
            DataTable dt3 = GetTable3(UserNumber);//订单中循环提货的空鼓使用数据
            DataTable dt4 = GetTable4(UserNumber);//积分兑换单中提货价兑换的空鼓使用数据
            DataTable dt5 = GetTable5(UserNumber);//旧版合格旧硒鼓交易中心的数据
            DataTable dt6 = GetTable6(UserNumber);//旧硒鼓交易转让冻结，包括旧版和新版的冻结
            DataTable dt13 = GetTable13(UserNumber);//新版合格旧硒鼓交易数据
            DataTable dt14 = GetTable14(UserNumber);//空鼓使用明细表中的数据
            DataTable dt15 = GetTable15(UserNumber);//用户转入服务商名下的空鼓数据

            //首先处理四个表，搞成跟第一个表一样的格式。
            DataTable dt2re1 = revalue(dt2, "蓝装");
            DataTable dt2re2 = revalue(dt2, "绿装");
            DataTable dt2re3 = revalue(dt2, "原装");

            DataTable dt3re1 = revalue(dt3, "蓝装");
            DataTable dt3re2 = revalue(dt3, "绿装");
            DataTable dt3re3 = revalue(dt3, "原装");

            DataTable dt4re1 = revalue(dt4, "蓝装");
            DataTable dt4re2 = revalue(dt4, "绿装");
            DataTable dt4re3 = revalue(dt4, "原装");

            DataTable dt15re1 = revalue(dt15, "蓝装");
            DataTable dt15re2 = revalue(dt15, "绿装");
            DataTable dt15re3 = revalue(dt15, "原装");
            //合并几个表格
            dt_mt = MergeTable(dt1, dt1_2, dt1_3, dt2re1, dt2re2, dt2re3, dt3re1, dt3re2, dt3re3, dt4re1, dt4re2, dt4re3, dt5, dt6, dt13, dt14, dt15re1, dt15re2, dt15re3);
        }
        else
        {
            DataTable dt1 = GetQiChu(UserNumber);
            DataTable dt2 = GetERPChuAndRu(UserNumber);
            DataTable dt3 = GetKGSYMX(UserNumber);           
            DataTable dt4 = GetJY(UserNumber);
            DataTable dt5 = GetJYDJ(UserNumber);
            DataTable dt6 = GetTiaoZheng(UserNumber);
            //合并几个表格
            dt_mt = MergeTable(dt1, dt2, dt3, dt4, dt5, dt6);
        }
        
        //已验证产品列表的空鼓使用情况加入到空鼓使用中。
        if (dtNow != null)
        {
            DataTable dtNowre1 = revalue(dtNow, "蓝装");
            DataTable dtNowre2 = revalue(dtNow, "绿装");
            DataTable dtNowre3 = revalue(dtNow, "原装");
            dt_mt.Merge(dtNowre1, false, MissingSchemaAction.Ignore);
            dt_mt.Merge(dtNowre2, false, MissingSchemaAction.Ignore);
            dt_mt.Merge(dtNowre3, false, MissingSchemaAction.Ignore);
        }
        
        DataTable dt_mt_js = dt_mt.Clone();
        //开始数量合并
        for (int i = 0; i < dt_mt.Rows.Count; i++)
        {
            //ERP空鼓中存在 FM-C912CC/435A(大)、FM-C912CC/435A(小)、FM-C912CC三种，但成品都是对应FM-C912CC，故都替换成FM-C912CC方便核销
            if (dt_mt.Rows[i]["空鼓规格"].ToString().IndexOf("FM-C912CC/435A") >= 0)
            {
                dt_mt.Rows[i]["空鼓规格"] = "FM-C912CC";
            }

            //是否需要更新新表
            bool needadd = true;
            if (dt_mt.Rows[i]["空鼓类型"].ToString() == "红装")
            {
                needadd = false;
            }
            else
            {
                for (int p = 0; p < dt_mt_js.Rows.Count; p++)
                {
                    //发现匹配列，开始更新新表原有数据。
                    if (dt_mt_js.Rows[p]["客户编号"].ToString() == dt_mt.Rows[i]["客户编号"].ToString() && dt_mt_js.Rows[p]["空鼓类型"].ToString() == dt_mt.Rows[i]["空鼓类型"].ToString() && dt_mt_js.Rows[p]["空鼓规格"].ToString() == dt_mt.Rows[i]["空鼓规格"].ToString() && dt_mt_js.Rows[p]["空鼓规格"].ToString() == dt_mt.Rows[i]["空鼓规格"].ToString())
                    {
                        dt_mt_js.Rows[p]["剩余总量"] = (Convert.ToInt32(dt_mt_js.Rows[p]["剩余总量"]) + Convert.ToInt32(dt_mt.Rows[i]["剩余总量"])).ToString();
                        //标记为无需添加新行,此行被合并计算了
                        needadd = false;
                    }
                }
            }
            if (needadd)
            {
                DataRow aDataRow = dt_mt_js.NewRow();
                aDataRow.ItemArray = dt_mt.Rows[i].ItemArray;
                dt_mt_js.Rows.Add(aDataRow);
            }
        }
        return dt_mt_js;
    }

    /// <summary>
    /// 获取某加上当前已验证的产品列表的拆分结果后的可用空鼓数量,列： 型号，类型，型号，数量
    /// 传入空值为不限制
    /// </summary>
    /// <param name="UserNumber"></param>
    /// <param name="lx"></param>
    /// <param name="xh"></param>
    /// <returns></returns>
    public DataTable GetAllKGCanUseByWhere(string UserNumber, string lx, string xh,DataTable dt7)
    {
        string sqltj = " ";
        if (lx != "")
        {
            sqltj = sqltj + " and 空鼓类型 like '%" + lx + "%' ";
        }
        if (xh != "")
        {
            sqltj = sqltj + " and 空鼓规格 like '%" + xh + "%' ";
        }

        DataTable dt = GetAllKGCanUseByUserNumber(UserNumber,dt7);
        DataRow[] dr = dt.Select(" 1=1 " + sqltj, " 空鼓规格 DESC ");

        DataTable ndt = new DataTable();
        ndt = dt.Clone();
        for (int i = 0; i <= dr.Length - 1; i++)
        {
            if (dr[i]["剩余总量"].ToString() != "0" && dr[i]["剩余总量"].ToString().Trim() != "")
            {
                ndt.Rows.Add(dr[i].ItemArray);
            }
            
        }
        return ndt;
    }


    /// <summary>
    /// 获取某客户某些条件的可以使用的空鼓数量,列： 型号，类型，型号，数量
    /// 传入空值为不限制
    /// </summary>
    /// <param name="UserNumber"></param>
    /// <param name="lx"></param>
    /// <param name="xh"></param>
    /// <returns></returns>
    public DataTable GetAllKGCanUseByWhere(string UserNumber, string lx, string xh)
    {
        string sqltj = " ";
        if (lx != "")
        {
            sqltj = sqltj + " and 空鼓类型 like '%" + lx + "%' ";
        }
        if (xh != "")
        {
            sqltj = sqltj + " and 空鼓规格 like '%" + xh + "%' ";
        }

        DataTable dt = GetAllKGCanUseByUserNumber(UserNumber, null);
        DataRow[] dr = dt.Select(" 1=1 " + sqltj, " 空鼓规格 DESC ");

        DataTable ndt = new DataTable();
        ndt = dt.Clone();
        for (int i = 0; i <= dr.Length - 1; i++)
        {
            if (dr[i]["剩余总量"].ToString() != "0" && dr[i]["剩余总量"].ToString().Trim() != "")
            {
                ndt.Rows.Add(dr[i].ItemArray);
            }

        }
        return ndt;
    }

    //获取认购鼓当前可用数量（富美市场字[2011]12号 关于向服务商提供低价合格旧硒鼓支持的通知2011.5）
    public DataTable  GetAllRGKGCanUseByWhere(string fwsbh, string cpxh)
    {
        string sqlRG = "select fwsbh as 服务商编号,JXGLB as 空鼓类别,JXGDYCPXH as 空鼓型号,RGSL as 空鼓总量,RGSL as 可用数量 from CWGL_HGJXGRGD as a left join CWGL_HGJXGRGD_HGJXGLB as b on a.number=b.parentnumber where fwsbh='" + fwsbh + "' and JXGLB='原装' and JXGDYCPXH like '%" + cpxh + "%' ";
        string sqlUsed = "select fwsbh as 服务商编号,'原装' as 空鼓类别,kgxh as 空鼓型号,0 as 空鼓总量, -yz as 可用数量  from CWGL_KGSYMXJLB where fwsbh='" + fwsbh + "' and cpxh like '%" + cpxh + "%' and djlb='特殊支持循环提货订单' ";

        string sql = "select 服务商编号,空鼓类别,空鼓型号,isnull(sum(空鼓总量),0) as 空鼓总量,isnull(sum(可用数量),0) as 可用数量 from  (" + sqlRG + " union all " + sqlUsed + " ) as tab1 group by 服务商编号,空鼓类别,空鼓型号";

        //当前可用认购鼓信息
        DataSet dsRG = DbHelperSQL.Query(sql);
        if (dsRG != null && dsRG.Tables!=null)
        {
            return dsRG.Tables[0];
        }
        return null; 
    }


    /// <summary>
    /// 普通型号的空鼓扣减拆分，如果空鼓足够，结果放入dtkg中，如果空鼓不足，返回msg提示字符串
    /// </summary>
    public void ComputeKG(string fwsbh, string cplb, string cpxh, int cpsl, ref DataTable dtkg, ref string msg)
    {
        int yuanz = 0;//使用的原装空鼓量
        int lanz = 0;//使用的蓝装空鼓量
        int lvz = 0;//使用的绿装空鼓量

        //获取该型号的空鼓各种类别的当前可用量       
        DataTable dtky = GetAllKGCanUseByWhere(fwsbh, "", cpxh, dtkg);
        int lvz_ky = 0;//绿装空鼓可用量
        int yuanz_ky = 0;//原装空鼓可用量
        int lanz_ky = 0;//蓝装空鼓可用量
        for (int i = 0; i < dtky.Rows.Count; i++)
        {
            string kglx = dtky.Rows[i]["空鼓类型"].ToString();
            int SYZL = Convert.ToInt32(dtky.Rows[i]["剩余总量"].ToString());
            if (SYZL > 0)
            {
                if (kglx == "绿装")
                {
                    lvz_ky = SYZL;
                }
                if (kglx == "蓝装")
                {
                    lanz_ky = SYZL;
                }
                if (kglx == "原装")
                {
                    yuanz_ky = SYZL;
                }
            }
        }

        //根据蓝、绿装进行空鼓拆分
        if (cplb == "蓝装")//如果产品为蓝装，则拆分顺序为：蓝装空鼓、原装空鼓
        {
            if (lanz_ky >= cpsl)//如果蓝装空鼓够用，则直接扣减蓝装
            {
                lanz = cpsl;
            }
            else//如果蓝装空鼓不够用，则同时扣减蓝装、原装空鼓
            {
                if (yuanz_ky >= cpsl - lanz_ky)
                {
                    lanz = lanz_ky;
                    yuanz = cpsl - lanz_ky;
                }
                else
                {
                    msg = msg + cplb + cpxh + "，差" + (cpsl - lanz_ky - yuanz_ky).ToString() + "支\\n\\r";
                    lanz = lanz_ky;
                    yuanz = yuanz_ky;
                }
            }
        }
        if (cplb == "绿装")//如果产品为绿装，则拆分顺序为：绿装空鼓、原装空鼓、蓝装空鼓
        {
            if (lvz_ky >= cpsl)//如果绿装空鼓够用，则直接扣减绿装空鼓。
            {
                lvz = cpsl;
            }
            else//如果绿装空鼓不够用
            {
                if (yuanz_ky >= cpsl - lvz_ky)//判断原装空鼓，如果够用则扣减绿装、原装两种空鼓
                {
                    lvz = lvz_ky;
                    yuanz = cpsl - lvz_ky;
                }
                else//如果绿装+原装也不够用，则扣减绿装、原装、蓝装空鼓
                {
                    if (lanz_ky >= cpsl - lvz_ky - yuanz_ky)
                    {
                        lvz = lvz_ky;
                        yuanz = yuanz_ky;
                        lanz = cpsl - lvz_ky - yuanz_ky;
                    }
                    else
                    {
                        msg = msg + cplb + cpxh + "，差" + (cpsl - lvz_ky - yuanz_ky - lanz_ky).ToString() + "支\\n\\r";
                        lvz = lvz_ky;
                        yuanz = yuanz_ky;
                        lanz = lanz_ky;
                    }
                }
            }
        }

        //DataSet dt_KG = DbHelperSQL.Query("select fwsbh as 客户编号,''as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量 from CWGL_KGSYMXJLB where 1!=1");
        if (yuanz + lanz + lvz > 0)
        { 
            dtkg.Rows.Add(new object[] { fwsbh, "", cpxh, 0, yuanz, lanz, lvz, cplb, cpxh, cpsl });
        }   

    }

    /// <summary>
    /// 特殊型号的空鼓扣减拆分
    /// </summary>   
    public void ComputeTSKG(string fwsbh, string cplb, string cpxh, int cpsl, string kgxh2, ref  DataTable dtkg, ref string msg)
    {
        int yuanzA = 0;//使用的同型号原装空鼓量
        int lanzA = 0;//使用的同型号蓝装空鼓量
        int lvzA = 0;//使用的同型号绿装空鼓量

        int yuanzX = 0;//使用的另一型号原装空鼓量
        int lanzX = 0;//使用的另一型号蓝装空鼓量
        int lvzX = 0;//使用的另一型号绿装空鼓量

        //获取同型号的空鼓各种类别的当前可用量    
        string kgxh = cpxh;
        //针对特殊的型号，进行特别的处理
        if (cpxh == "FM-CC388AH(医院专供)")
        {
            kgxh = "FM-CC388AH";
        }
        else if (cpxh == "FM-CB436AH(医院专供)")
        {
            kgxh = "FM-CB436AH";
        }

        DataTable dtA = GetAllKGCanUseByWhere(fwsbh, "", kgxh, dtkg);
        int lvz_kyA = 0;//绿装空鼓可用量
        int yuanz_kyA = 0;//原装空鼓可用量
        int lanz_kyA = 0;//蓝装空鼓可用量
        for (int i = 0; i < dtA.Rows.Count; i++)
        {
            string kglx = dtA.Rows[i]["空鼓类型"].ToString();
            int A_SYZL = Convert.ToInt32(dtA.Rows[i]["剩余总量"].ToString());
            if (A_SYZL > 0)
            {
                if (kglx == "绿装")
                {
                    lvz_kyA = A_SYZL;
                }
                if (kglx == "蓝装")
                {
                    lanz_kyA = A_SYZL;
                }
                if (kglx == "原装")
                {
                    yuanz_kyA = A_SYZL;
                }
            }
        }

        int yuanz_kyX = 0;//X规格原装可用空鼓量
        int lanz_kyX = 0;//X规格蓝装可用空鼓量
        int lvz_kyX = 0;//X规格绿装可用空鼓量
        DataTable dtX = GetAllKGCanUseByWhere(fwsbh, "", kgxh2, dtkg);//获取X规格各类型的可用空鼓量
        for (int i = 0; i < dtX.Rows.Count; i++)
        {
            string kglx = dtX.Rows[i]["空鼓类型"].ToString();
            int X_SYZL = Convert.ToInt32(dtX.Rows[i]["剩余总量"].ToString());
            if (X_SYZL > 0)
            {
                if (kglx == "绿装")
                {
                    lvz_kyX = X_SYZL;
                }
                if (kglx == "蓝装")
                {
                    lanz_kyX = X_SYZL;
                }
                if (kglx == "原装")
                {
                    yuanz_kyX = X_SYZL;
                }
            }
        }

        //根据蓝、绿装进行空鼓拆分
        if (cplb == "蓝装")//如果产品为蓝装，则拆分顺序为：同规格蓝装、原装空鼓，X规格蓝装、原装空鼓
        {
            int totalA = lanz_kyA + yuanz_kyA;
            if (totalA >= cpsl)//如果同规格的蓝装、原装空鼓够用，直接扣减同规格
            {
                if (lanz_kyA >= cpsl)//如果同规格蓝装空鼓够用，则直接扣减蓝装
                {
                    lanzA = cpsl;
                }
                else //如果同规格蓝装空鼓不够用，判断同规格的原装是否够用，如果不够，则进一步扣减另一规格的空鼓
                {
                    lanzA = lanz_kyA;
                    yuanzA = cpsl - lanzA;
                }
            }
            else//如果同规格空鼓不够，则继续扣减X规格的空鼓
            {
                lanzA = lanz_kyA;//先将同规格空鼓扣减掉
                yuanzA = yuanz_kyA;
                if (lanz_kyX >= cpsl - totalA)//如果X的蓝装空鼓够用
                {
                    lanzX = cpsl - totalA;
                }
                else//如果X的蓝装不够，则继续扣减X的原装
                {
                    if (yuanz_kyX >= cpsl - totalA - lanz_kyX)
                    {
                        lanzX = lanz_kyX;
                        yuanzX = cpsl - totalA - lanzX;
                    }
                    else
                    {
                        msg = msg + cplb + cpxh + "，差" + (cpsl - totalA - lanz_kyX - yuanz_kyX).ToString() + "支\\n\\r";
                        lanzA = lanz_kyA;
                        yuanzA = yuanz_kyA;
                        lanzX = lanz_kyX;
                        yuanzX = yuanz_kyX;
                    }

                }
            }
        }
        if (cplb == "绿装")//如果产品为绿装，则拆分顺序为：绿装空鼓、原装空鼓、蓝装空鼓
        {
            int totalA = lvz_kyA + yuanz_kyA + lanz_kyA;

            if (totalA >= cpsl)//如果同型号的空鼓数量足够
            {
                if (lvz_kyA >= cpsl)//如果绿装空鼓够用，则直接扣减绿装空鼓。
                {
                    lvzA = cpsl;
                }
                else
                {
                    if (yuanz_kyA >= cpsl - lvz_kyA)//判断原装空鼓，如果够用则扣减绿装、原装两种空鼓
                    {
                        lvzA = lvz_kyA;
                        yuanzA = cpsl - lvzA;
                    }
                    else//如果绿装+原装也不够用，则扣减绿装、原装、蓝装空鼓
                    {
                        lvzA = lvz_kyA;
                        yuanzA = yuanz_kyA;
                        lanzA = cpsl - lvzA - yuanzA;
                    }
                }
            }
            else//如果同型号的空鼓不够，则开始扣减第二个型号的空鼓
            {
                lvzA = lvz_kyA;//先将同规格的空鼓扣减掉
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                if (lvz_kyX >= cpsl - totalA)//如果X的绿装够用，则扣减X的绿装
                {
                    lvzX = cpsl - totalA;
                }
                else
                {
                    if (yuanz_kyX >= cpsl - totalA - lvz_kyX)//如果X的绿装不够，继续扣减X的原装
                    {
                        lvzX = lvz_kyX;
                        yuanzX = cpsl - totalA - lvzX;
                    }
                    else
                    {
                        if (lanz_kyX >= cpsl - totalA - lvz_kyX - yuanz_kyX)//如果X的原装还不够，则继续扣减X的蓝装
                        {
                            lvzX = lvz_kyX;
                            yuanzX = yuanz_kyX;
                            lanzX = cpsl - totalA - lvzX - yuanzX;
                        }
                        else
                        {
                            msg = msg + cplb + cpxh + "，差" + (cpsl - totalA - lvz_kyX - yuanz_kyX - lanz_kyX).ToString() + "支\\n\\r";
                            lvzA = lvz_kyA;
                            yuanzA = yuanz_kyA;
                            lanzA = lanz_kyA;
                            lvzX = lvz_kyX;
                            yuanzX = yuanz_kyX;
                            lanzX = lanz_kyX;
                        }

                    }
                }
            }
        }

        //添加同型号拆分结果
        //DataSet dt_KG = DbHelperSQL.Query("select fwsbh as 客户编号,''as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量 from CWGL_KGSYMXJLB where 1!=1");

        if (yuanzA + lanzA + lvzA > 0)
        { 
            dtkg.Rows.Add(new object[] { fwsbh, "", kgxh, 0, yuanzA, lanzA, lvzA, cplb, cpxh, cpsl });
        }

        //添加X型号的扣减结果
        if (yuanzX + lanzX + lvzX > 0)
        { 
            dtkg.Rows.Add(new object[] { fwsbh, "", kgxh2, 0, yuanzX, lanzX, lvzX, cplb, cpxh, cpsl });
        }
    }
    #endregion


    //根据服务商所属分公司目前的情况，确定通过什么方式获取信用额度并返回计算结果
    public double GetCreditMoney(string UserNumber)
    {
        double Money=0.00;
        object objDB = DbHelperSQL.GetSingle("select dyERP from fwpt_yhxxb as a left join system_city_0 as b on a.ssbsc=b.name where a.khbh='" + UserNumber + "'");
        string DBname = "";
        if (objDB != null && objDB.ToString().Trim() != "")
        {
            DBname = objDB.ToString();
        }
        if(DBname =="fm"||DBname =="fmlx2")
        {
            Money =GetCreditMoney_ZB(UserNumber);
        }
        else
        {
            Money =GetCreditMoney_FGS(UserNumber );
        }

        return Money ;
    }

    /// <summary>
    /// 获取分公司未上线前服务商当前可用信用额度
    /// </summary>
    /// <param name="UserNumber">服务商编号</param>
    /// <returns></returns>
    public double GetCreditMoney_ZB(string UserNumber)
    {
        //订单中未填写过销货单的总金额 
        string sql1 = "select TC004 as 客户编号,sum(cast((TD008-TD009)*TD011 as numeric(18,2))) as 金额 from COPTD left join COPTC on TC001=TD001 and TC002=TD002  where ltrim(rtrim(TC004))='" + UserNumber + "' and TC027='Y' and TD016='N' group by TC004";

        //销货单中未开过销售发票的总金额        

        string sql2 = "select TG004 as 客户编号,sum((case cast(TH008 as int) when 0 then cast((TH035+TH036) as numeric(18,2)) else cast((TH008-TH042)*TH012 as numeric(18,2)) end)) as 金额 from COPTG left join COPTH on TH001=TG001 and TH002=TG002  where ltrim(rtrim(TG004)) = '" + UserNumber + "' and TG023='Y' and TH026='N' group by TG004";

        //销退单中未开过发票的金额       
        string sql3 = "select TI004 as 客户编号,sum((case cast(TJ007 as int) when 0 then cast(TJ012 as numeric(18,2)) else cast((TJ007-TJ037)*TJ011 as numeric(18,2)) end) ) as 金额 from COPTI left join COPTJ on TI001=TJ001 and TI002=TJ002  where ltrim(rtrim(TI004))='" + UserNumber + "' and TI019='Y' and TJ024='N' group by TI004";

        //开过的发票总额
        string sql4 = "select TA004 as 客户编号,sum(TA041+TA042-TA098) as 金额 from ACRTA where ltrim(rtrim(TA004))='" + UserNumber + "' and TA025='Y' and TA079='1' group by TA004";
        string sql4_2 = "select TA004 as 客户编号, -sum(TA041+TA042-TA098) as 金额 from ACRTA where ltrim(rtrim(TA004))='" + UserNumber + "' and TA025='Y' and TA079='2' group by TA004";


        //信用额度初始值
        string sql5 = "select MA001 as 客户编号,MA033*(1+MA034) as 金额 from COPMA where ltrim(rtrim(MA001))='" + UserNumber + "' and MA097='1'";

        //收款单中未核销的部分的总额
        string sql6 = "select TK004 as 客户编号,sum(TK033+TK035+TK036-TK038) as 金额 from ACRTK where ltrim(rtrim(TK004))= '" + UserNumber + "' and TK020='Y' and TK030!=3 group by TK004";

        //退款单中未核销部分的总额
        string sql7 = "select TK004 as 客户编号,sum(TK033+TK035+TK036-TK038) as 金额 from ACRTK where ltrim(rtrim(TK004))= '" + UserNumber + "' and TK020='Y' and TK001='6D' and TK030!=3 group by TK004";

        //业务平台中未录入销货单的订单金额
        string sql8 = "select number as 订单号,thfs as 提货方式,cppl as 产品品类,cpxh as 产品型号,isnull(cpsl,0) as 产品数量,0 as 已交数量,isnull(thj,0) as 提货价,isnull(yj,0) as 押金,isnull(cast((thj+yj)*cpsl as numeric(18,2)),0) as 剩余金额 from FWPT_FWSDHD as a left join  FWPT_FWSDHD_CPLB as b on a.number=b.parentnumber where fwsbh= '" + UserNumber + "'  and isnull(qzjs,'')<>'是'";


        //新版合格旧硒鼓交易中心ERP中未审核的收款单，因审核后的收款单转入了“收款单中未核销部分的总额中计算了”
        string sql9 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6C1' and TK004='" + UserNumber + "' and TK020<>'Y'";
        //新版合格旧硒鼓交易中心ERP中未审核的退款单
        string sql10 = "select -isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6D1' and TK004='" + UserNumber + "' and TK020<>'Y'";
        //新版合格旧硒鼓交易中心被冻结的额度(剩余可交易量总额)
        string sql11 = "select  -isnull(sum(SYKMSL*DJ),0) as 金额  from NewPurchase where MRFKHBH='" + UserNumber + "' and JYZT <>'5' and JYZT <>'6'";
        //新版合格旧硒鼓交易中心在成功交易明细表中的收款额度(转换到ERP之前的临时数据)
        string sql12 = "select  isnull(sum(CJZJ),0) as 金额  from CGJYMXB where KHBH='" + UserNumber + "' and ZHZT ='0' and MMLX ='卖出'";
        //新版合格旧硒鼓交易中心在成功交易明细表中的退款额度(转换到ERP之前的临时数据)
        string sql13 = "select  -isnull(sum(CJZJ),0) as 金额  from CGJYMXB where KHBH='" + UserNumber + "' and ZHZT ='0' and MMLX ='买入'";
        //新版合格旧硒鼓交易中心提现申请中的信用额度扣减(只扣减已生效的，其他状态，将在ERP信用额度中体现)
        string sql14 = "select  -isnull(sum(TXED),0) as 金额  from SQTX where KHBH='" + UserNumber + "' and ZT ='已生效' ";

        //中转收款单中的数据
        string sql15 = "select isnull(sum(case jeys when 'ADD' then JE when 'DEL' then -JE end),0) as 金额 from FM_ZZSKD where KHBH='" + UserNumber + "' and isnull(ZHERPZT,'')<>'是' and isnull(JLZT,'')<>'作废'";

        //中转收款单中扣减交易大厅服务费转入ERP未审核的数据
        string sql15_1 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6C4' and TK004='" + UserNumber + "' and TK020<>'Y'";


        DataSet ds_1 = GetErpData.GetDataSet(sql1,"公司总部");
        DataSet ds_2 = GetErpData.GetDataSet(sql2,"公司总部");
        DataSet ds_3 = GetErpData.GetDataSet(sql3,"公司总部");
        DataSet ds_4 = GetErpData.GetDataSet(sql4, "公司总部");
        DataSet ds_4_2 = GetErpData.GetDataSet(sql4_2, "公司总部");
        DataSet ds_5 = GetErpData.GetDataSet(sql5, "公司总部");
        DataSet ds_6 = GetErpData.GetDataSet(sql6, "公司总部");
        DataSet ds_7 = GetErpData.GetDataSet(sql7, "公司总部");

        DataSet ds_9 = GetErpData.GetDataSet(sql9, "公司总部");
        DataSet ds_10 = GetErpData.GetDataSet(sql10, "公司总部");
        DataSet ds_11 = DbHelperSQL.Query(sql11);
        DataSet ds_12 = DbHelperSQL.Query(sql12);
        DataSet ds_13 = DbHelperSQL.Query(sql13);
        DataSet ds_14 = DbHelperSQL.Query(sql14);
        DataSet ds_15 = DbHelperSQL.Query(sql15);
        DataSet ds_15_1 = GetErpData.GetDataSet(sql15_1, "公司总部");

        DataSet ds8 = DbHelperSQL.Query(sql8);

        //获取该服务商所有的销货单信息      
        //string sql_XHD = "select COPTG.UDF01 as 订单号,TG004 as 客户编号, TH001 as 单别,TH002 as 单号,(case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end) as 类别,ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005)))) as 型号, cast(TH008 as int) as 数量,cast(TH012 as numeric(18,2)) as 提货价,0 as 押金 from COPTH left join COPTG on TH001=TG001 and TH002=TG002  where TG004='" + UserNumber + "' and TG023='Y' and TH001 in ('2313','2301','2302') union all select a.订单号,a.客户编号, a.TH001 as 单别,a.TH002 as 单号,a.TH004 as 类别,a.TH005 as 型号,cast(a.TH008 as int) as 数量,a.提货价 ,b.押金 from (select COPTG.UDF01 as 订单号,TG004 as 客户编号, TH001,TH002,(case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end)  as TH004,ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005)))) as TH005,TH008,TH012 as 提货价,0 as 押金,TH013 from COPTH left join COPTG on TG001=TH001 and TG002=TH002  where TG023='Y' and TH001='2312' and TH008>0 and TG004='" + UserNumber + "') as a left join (select COPTG.UDF01 as 订单号, TG004 as 客户编号, TH001,TH002,(case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end)  as TH004,ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005)))) as TH005,0 as 提货价,(TH013/TH012) as TH008,TH012 as 押金,TH013 from COPTH  left join COPTG on TG001=TH001 and TG002=TH002  where TG023='Y' and TH001='2312' and TH008=0 and TG004='" + UserNumber + "' ) as b on a.TH001=b.TH001 and a.TH002=b.TH002 and a.TH004=b.TH004 and a.TH005=b.TH005 and a.TH008=b.TH008 and a.订单号=b.订单号 and a.客户编号=b.客户编号";
        //DataSet dsXHD = GetErpData.GetDataSet(sql_XHD, "公司总部");

        //if (ds8 != null && ds8.Tables[0].Rows.Count > 0)
        //{
        //    for (int i = 0; i < ds8.Tables[0].Rows.Count; i++)
        //    {
        //        string ddh = ds8.Tables[0].Rows[i]["订单号"].ToString();
        //        string thfs = ds8.Tables[0].Rows[i]["提货方式"].ToString();
        //        string yjje = ds8.Tables[0].Rows[i]["押金"].ToString();
        //        string cpjg = ds8.Tables[0].Rows[i]["提货价"].ToString();
        //        string cppl = ds8.Tables[0].Rows[i]["产品品类"].ToString();
        //        string cpxh = ds8.Tables[0].Rows[i]["产品型号"].ToString();

        //        string db = "";
        //        if (thfs == "循环提货")
        //        {
        //            db = "2313";
        //        }
        //        else if (thfs == "押金提货")
        //        {
        //            if (yjje == "0.00")
        //            {
        //                db = "2302','2301";
        //            }
        //            else
        //            {
        //                db = "2312";
        //            }
        //        }
        //        else if (thfs == "无押金提货")
        //        {
        //            db = "2301";
        //        }
        //        string yjfsl = "0";
        //        if (thfs == "循环提货" || thfs == "无押金提货" || (thfs == "押金提货" && yjje == "0.00"))
        //        {
        //            string tiaojian = "订单号='" + ddh + "' and 类别='" + cppl + "' and 单别 in ('" + db + "') and 型号='" + cpxh + "' and 提货价=" + cpjg + "";
        //            yjfsl = dsXHD.Tables[0].Compute("sum(数量)", tiaojian).ToString();
        //        }
        //        else if (thfs == "押金提货" && yjje != "0.00")
        //        {
        //            string tiaojian = "订单号='" + ddh + "' and 类别='" + cppl + "' and 单别 in ('" + db + "') and 型号='" + cpxh + "' and 提货价=" + cpjg + " and 押金=" + yjje + "";
        //            yjfsl = dsXHD.Tables[0].Compute("sum(数量)", tiaojian).ToString();

        //        }

        //        if (yjfsl.ToString() != "")
        //        {
        //            ds8.Tables[0].Rows[i]["已交数量"] = yjfsl;
        //            ds8.Tables[0].Rows[i]["剩余金额"] = (Convert.ToInt32(ds8.Tables[0].Rows[i]["产品数量"]) - Convert.ToInt32(yjfsl)) * (Convert.ToDecimal(ds8.Tables[0].Rows[i]["提货价"]) + Convert.ToDecimal(ds8.Tables[0].Rows[i]["押金"]));
        //            if (Convert.ToDouble(ds8.Tables[0].Rows[i]["剩余金额"]) < 0)
        //            {
        //                ds8.Tables[0].Rows[i]["剩余金额"] = 0.00;
        //            }
        //        }
        //    }
        //}

        if (ds8 != null && ds8.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds8.Tables[0].Rows.Count; i++)
            {
                string ddh = ds8.Tables[0].Rows[i]["订单号"].ToString();
                string thfs = ds8.Tables[0].Rows[i]["提货方式"].ToString();
                string yjje = ds8.Tables[0].Rows[i]["押金"].ToString();
                string cpjg = ds8.Tables[0].Rows[i]["提货价"].ToString();
                string cppl = ds8.Tables[0].Rows[i]["产品品类"].ToString();
                string cpxh = ds8.Tables[0].Rows[i]["产品型号"].ToString();

                string db = "";
                if (thfs == "循环提货")
                {
                    db = "2313";
                }
                else if (thfs == "押金提货")
                {
                    if (yjje == "0.00")
                    {
                        db = "2302','2301";
                    }
                    else
                    {
                        db = "2312";
                    }
                }
                else if (thfs == "无押金提货")
                {
                    db = "2301";
                }
                string sqlXH = "";
                if (thfs == "循环提货" || thfs == "无押金提货" || (thfs == "押金提货" && yjje == "0.00"))
                {
                    sqlXH = "select isnull(sum(cast(TH008 as int)),0) as 已交付数量,isnull(sum(cast(TH013 as numeric(18,2))),0.00) as 已交金额 from COPTG left join COPTH on TG001=TH001 and TG002=TH002 where TG004='" + UserNumber + "' and COPTG.UDF01='" + ddh + "' and TG001 in ('" + db + "') and (case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end)='" + cppl + "' and ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005))))='" + cpxh + "' and TG023='Y' and cast(TH012 as numeric(18,2))=" + cpjg + "";
                }
                else if (thfs == "押金提货" && yjje != "0.00")
                {
                    sqlXH = "select isnull(sum(cast(已交付数量 as int)),0) as 已交付数量,isnull(sum(cast(提货金合计+押金合计 as numeric(18,2))),0.00) as 已交金额 from (select TH004,TH008 as 已交付数量,TH013 as 提货金合计 from COPTG left join COPTH on TG001=TH001 and TG002=TH002 where TG004='" + UserNumber + "' and COPTG.UDF01='" + ddh + "' and TG001 in ('" + db + "') and (case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end)='" + cppl + "' and substring(TH005,charindex(' ',TH005),len(TH005)) like '%" + cpxh + "%' and TG023='Y' and  cast(TH012 as numeric(18,2))='" + cpjg + "') as a left join (select distinct TH004 ,isnull(cast(TH013/TH012 as int),0) as 数量,isnull(cast(TH013 as numeric(18,2)),0.00) as 押金合计 from COPTG left join COPTH on TG001=TH001 and TG002=TH002 where TG004='" + UserNumber + "' and COPTG.UDF01='" + ddh + "' and TG001 in ('" + db + "') and (case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end)='" + cppl + "' and ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005))))='" + cpxh + "' and TG023='Y' and  cast(TH012 as numeric(18,2))='" + yjje + "') as b on a.TH004=b.TH004 and a.已交付数量=b.数量";
                }


                DataSet dsERP = GetErpData.GetDataSet(sqlXH,"公司总部");
                if (dsERP != null && dsERP.Tables[0].Rows.Count > 0)
                {
                    ds8.Tables[0].Rows[i]["已交数量"] = dsERP.Tables[0].Rows[0]["已交付数量"];
                    ds8.Tables[0].Rows[i]["剩余金额"] = (Convert.ToInt32(ds8.Tables[0].Rows[i]["产品数量"]) - Convert.ToInt32(dsERP.Tables[0].Rows[0]["已交付数量"])) * (Convert.ToDecimal(ds8.Tables[0].Rows[i]["提货价"]) + Convert.ToDecimal(ds8.Tables[0].Rows[i]["押金"]));
                    if (Convert.ToDouble(ds8.Tables[0].Rows[i]["剩余金额"]) < 0)
                    {
                        ds8.Tables[0].Rows[i]["剩余金额"] = 0.00;
                    }
                }
            }
        }

        double d1 = 0.00;
        double d2 = 0.00;
        double d3 = 0.00;
        double d4 = 0.00;
        double d4_2 = 0.00;
        double d5 = 0.00;
        double d6 = 0.00;
        double d7 = 0.00;
        double d8 = 0.00;
        double d9 = 0.00;
        double d10 = 0.00;
        double d11 = 0.00;
        double d12 = 0.00;
        double d13 = 0.00;
        double d14 = 0.00;
        double d15 = 0.00;
        double d15_1 = 0.00;

        if (ds_1 != null && ds_1.Tables[0].Rows.Count > 0)
        {
            d1 = Convert.ToDouble(ds_1.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_2 != null & ds_2.Tables[0].Rows.Count > 0)
        {
            d2 = Convert.ToDouble(ds_2.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_3 != null & ds_3.Tables[0].Rows.Count > 0)
        {
            d3 = Convert.ToDouble(ds_3.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_4 != null & ds_4.Tables[0].Rows.Count > 0)
        {
            d4 = Convert.ToDouble(ds_4.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_4_2 != null & ds_4_2.Tables[0].Rows.Count > 0)
        {
            d4_2 = Convert.ToDouble(ds_4_2.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_5 != null & ds_5.Tables[0].Rows.Count > 0)
        {
            d5 = Convert.ToDouble(ds_5.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_6 != null & ds_6.Tables[0].Rows.Count > 0)
        {
            d6 = Convert.ToDouble(ds_6.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_7 != null && ds_7.Tables[0].Rows.Count > 0)
        {
            d7 = Convert.ToDouble(ds_7.Tables[0].Rows[0]["金额"].ToString());
        }

        if (ds_9 != null && ds_9.Tables[0].Rows.Count > 0)
        {
            d9 = Convert.ToDouble(ds_9.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_10 != null && ds_10.Tables[0].Rows.Count > 0)
        {
            d10 = Convert.ToDouble(ds_10.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_11 != null && ds_11.Tables[0].Rows.Count > 0)
        {
            d11 = Convert.ToDouble(ds_11.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_12 != null && ds_12.Tables[0].Rows.Count > 0)
        {
            d12 = Convert.ToDouble(ds_12.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_13 != null && ds_13.Tables[0].Rows.Count > 0)
        {
            d13 = Convert.ToDouble(ds_13.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_14 != null && ds_14.Tables[0].Rows.Count > 0)
        {
            d14 = Convert.ToDouble(ds_14.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_15 != null && ds_15.Tables[0].Rows.Count > 0)
        {
            d15 = Convert.ToDouble(ds_15.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_15_1 != null && ds_15_1.Tables[0].Rows.Count > 0)
        {
            d15_1 = Convert.ToDouble(ds_15_1.Tables[0].Rows[0]["金额"].ToString());
        }

        if (ds8 != null && ds8.Tables[0].Rows.Count > 0)
        {
            d8 = Convert.ToDouble(ds8.Tables[0].Compute("sum(剩余金额)", "true"));
        }

        double xyed = d5 + d6 - d7 - (d1 + (d2 - d3) + (d4 + d4_2)) - d8 + d9 + d10 + d11 + d12 + d13 + d14 + d15 + d15_1;
        return xyed;
    }

    //分公司上线后计算服务商当期信用额度
    public double GetCreditMoney_FGS(string UserNumber)
    {
     //获取客户名称
        DataSet dsMC = DbHelperSQL.Query("select b.dyfgsmc from fwpt_yhxxb as a left join system_city as b on a.ssbsc=b.name where a.khbh='" + UserNumber  + "'");
        string city="";
        if (dsMC != null && dsMC.Tables[0].Rows.Count > 0)
        {           
            city = dsMC.Tables[0].Rows[0]["dyfgsmc"].ToString();
        }      

        #region ERP中信用额度计算逻辑
        //订单中未填写过销货单的总金额 
        string sql_1 = "select TC004 as 客户编号,-sum(cast((TD008-TD009)*TD011 as numeric(18,2))) as 金额 from COPTD left join COPTC on TC001=TD001 and TC002=TD002 where TC004='" + UserNumber  + "' and TC027='Y' and TD016='N' group by TC004";

        //销货单中未开过销售发票的总金额 
        string sql_2 = "select TG004 as 客户编号,-sum((case TH008 when 0 then cast((TH035+TH036) as numeric(18,2)) else cast((TH008-TH042)*TH012 as numeric(18,2)) end)) as 金额 from COPTG left join COPTH on TH001=TG001 and TH002=TG002 where TG004='" + UserNumber  + "' and TG023='Y' and TH026='N' group by TG004";

        //销退单中未开过发票的金额       
        string sql_3 = "select TI004 as 客户编号,sum((case TJ007 when 0 then cast(TJ012 as numeric(18,2)) else cast((TJ007-TJ037)*TJ011 as numeric(18,2)) end) ) as 金额 from COPTI left join COPTJ on TI001=TJ001 and TI002=TJ002 where TI004='" + UserNumber  + "' and TI019='Y' and TJ024='N'  group by TI004";

        //开过的发票总额
        string sql_4 = "select TA004 as 客户编号,-sum(TA041+TA042-TA098) as 金额 from ACRTA where TA004='" + UserNumber  + "' and TA025='Y'and TA079='1' group by TA004";
        string sql_4_2 = "select TA004 as 客户编号, sum(TA041+TA042-TA098) as 金额 from ACRTA where TA004='" + UserNumber  + "' and TA025='Y' and TA079='2' group by TA004";

        //信用额度初始值
        string sql_5 = "select MA001 as 客户编号,MA033*(1+MA034) as 金额 from COPMA where MA001='" + UserNumber  + "' and MA097='1'";

        //收款单中未核销的部分的总额
        string sql_6 = "select TK004 as 客户编号,sum(TK033+TK035+TK036-TK038) as 金额 from ACRTK where TK004='" + UserNumber  + "' and TK020='Y' and TK030!=3 group by TK004";

        //退款单中未核销部分的总额
        string sql_7 = "select TK004 as 客户编号,-sum(TK033+TK035+TK036-TK038) as 金额 from ACRTK where TK004='" + UserNumber  + "' and TK020='Y' and TK001='6D' and TK030!=3 group by TK004";

        // lblERP.Text = (d5 + d6 - d7 - (d1 + (d2 - d3) + (d4 + d4_2))).ToString("#0.00");
        string sql_ERP = "select 客户编号,sum(金额) as 金额 from (" + sql_1 + " union all " + sql_2 + " union all " + sql_3 + " union all " + sql_4 + " union all " + sql_4_2 + " union all " + sql_5 + " union all " + sql_6 + " union all " + sql_7 + ") as tab group by 客户编号";

        #endregion


        //业务平台中未录入销货单的订单金额
        string sqlDD = "select number as 订单号,thfs as 提货方式,cppl as 产品品类,cpxh as 产品型号,cpsl as 产品数量,0 as 已交数量,thj as 提货价,yj as 押金,cast(yfkhj as numeric(18,2)) as 剩余金额 from FWPT_FWSDHD as a left join  FWPT_FWSDHD_CPLB as b on a.number=b.parentnumber where isnull(YWSLF,'')='' and  fwsbh='" + UserNumber  + "' and (isnull(qzjs,'')='' or isnull(qzjs,'')='否')";
       
        //新版合格旧硒鼓交易中心ERP中未审核的收款单，因审核后的收款单转入了“收款单中未核销部分的总额中计算了”
        string sql9 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6C1' and TK004='" + UserNumber  + "' and TK020<>'Y'";
        //新版合格旧硒鼓交易中心ERP中未审核的退款单
        string sql10 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6D1' and TK004='" + UserNumber  + "' and TK020<>'Y'";
        //新版合格旧硒鼓交易中心被冻结的额度(剩余可交易量总额)
        string sql11 = "select  isnull(sum(SYKMSL*DJ),0) as 金额  from NewPurchase where MRFKHBH='" + UserNumber  + "' and JYZT <>'5' and JYZT <>'6' and isnull(YWSLF,'')=''";
        //新版合格旧硒鼓交易中心在成功交易明细表中的收款额度(转换到ERP之前的临时数据)
        string sql12 = "select  isnull(sum(CJZJ),0) as 金额  from CGJYMXB where KHBH='" + UserNumber  + "' and ZHZT ='0' and MMLX ='卖出' and isnull(YWSLF,'')=''";
        //新版合格旧硒鼓交易中心在成功交易明细表中的退款额度(转换到ERP之前的临时数据)
        string sql13 = "select  isnull(sum(CJZJ),0) as 金额  from CGJYMXB where KHBH='" + UserNumber  + "' and ZHZT ='0' and MMLX ='买入'  and isnull(YWSLF,'')=''";

        //新版合格旧硒鼓交易中心提现申请中的信用额度扣减(只扣减已生效的，其他状态，将在ERP信用额度中体现)
        string sql14 = "select  isnull(sum(TXED),0) as 金额  from SQTX where KHBH='" + UserNumber  + "' and ZT ='已生效'  and isnull(YWSLF,'')='' ";

        //中转收款单中的数据
        string sql15 = "select isnull(sum(case jeys when 'ADD' then JE when 'DEL' then -JE end),0) as 金额 from FM_ZZSKD where KHBH='" + UserNumber  + "' and isnull(ZHERPZT,'')<>'是' and isnull(JLZT,'')<>'作废'  and isnull(YWSLF,'')=''";
        //中转收款单中扣减交易大厅服务费转入ERP未审核的数据
        string sql15_1 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6C4' and TK004='" + UserNumber  + "' and TK020<>'Y'";

        DataSet dsERP = GetErpData.GetDataSet(sql_ERP,city);

        //获取订单未交金额
        DataSet ds8 = DbHelperSQL.Query(sqlDD);

        //if (ds8 != null && ds8.Tables[0].Rows.Count > 0)
        //{
        //    //获取该服务商所有的销货单信息      
        //    string sql_XHD = "select COPTG.UDF01 as 订单号,TG004 as 客户编号, TH001 as 单别,TH002 as 单号,(case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end) as 类别,ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005)))) as 型号, cast(TH008 as int) as 数量,cast(TH012 as numeric(18,2)) as 提货价,0 as 押金 from COPTH left join COPTG on TH001=TG001 and TH002=TG002  where TG004='" + UserNumber + "' and TG023='Y' and TH001 in ('2313','2301','2302') union all select a.订单号,a.客户编号, a.TH001 as 单别,a.TH002 as 单号,a.TH004 as 类别,a.TH005 as 型号,cast(a.TH008 as int) as 数量,a.提货价 ,b.押金 from (select COPTG.UDF01 as 订单号,TG004 as 客户编号, TH001,TH002,(case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end)  as TH004,ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005)))) as TH005,TH008,TH012 as 提货价,0 as 押金,TH013 from COPTH left join COPTG on TG001=TH001 and TG002=TH002  where TG023='Y' and TH001='2312' and TH008>0 and TG004='" + UserNumber + "') as a left join (select COPTG.UDF01 as 订单号, TG004 as 客户编号, TH001,TH002,(case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end)  as TH004,ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005)))) as TH005,0 as 提货价,(TH013/TH012) as TH008,TH012 as 押金,TH013 from COPTH  left join COPTG on TG001=TH001 and TG002=TH002  where TG023='Y' and TH001='2312' and TH008=0 and TG004='" + UserNumber + "' ) as b on a.TH001=b.TH001 and a.TH002=b.TH002 and a.TH004=b.TH004 and a.TH005=b.TH005 and a.TH008=b.TH008 and a.订单号=b.订单号 and a.客户编号=b.客户编号";
        //    DataSet dsXHD = GetErpData.GetDataSet(sql_XHD, city);


        //    for (int i = 0; i < ds8.Tables[0].Rows.Count; i++)
        //    {
        //        string ddh = ds8.Tables[0].Rows[i]["订单号"].ToString();
        //        string thfs = ds8.Tables[0].Rows[i]["提货方式"].ToString();
        //        string yjje = ds8.Tables[0].Rows[i]["押金"].ToString();
        //        string cpjg = ds8.Tables[0].Rows[i]["提货价"].ToString();
        //        string cppl = ds8.Tables[0].Rows[i]["产品品类"].ToString();
        //        string cpxh = ds8.Tables[0].Rows[i]["产品型号"].ToString();

        //        string db = "";
        //        if (thfs == "循环提货")
        //        {
        //            db = "2313";
        //        }
        //        else if (thfs == "押金提货")
        //        {
        //            if (yjje == "0.00")
        //            {
        //                db = "2302','2301";
        //            }
        //            else
        //            {
        //                db = "2312";
        //            }
        //        }
        //        else if (thfs == "无押金提货")
        //        {
        //            db = "2301";
        //        }
        //        string yjfsl = "";
        //        if (thfs == "循环提货" || thfs == "无押金提货" || (thfs == "押金提货" && yjje == "0.00"))
        //        {
        //            string tiaojian = "订单号='" + ddh + "' and 类别='" + cppl + "' and 单别 in ('" + db + "') and 型号='" + cpxh + "' and 提货价=" + cpjg + "";
        //            yjfsl = dsXHD.Tables[0].Compute("sum(数量)", tiaojian).ToString();
        //        }
        //        else if (thfs == "押金提货" && yjje != "0.00")
        //        {
        //            string tiaojian = "订单号='" + ddh + "' and 类别='" + cppl + "' and 单别 in ('" + db + "') and 型号='" + cpxh + "' and 提货价=" + cpjg + " and 押金=" + yjje + "";
        //            yjfsl = dsXHD.Tables[0].Compute("sum(数量)", tiaojian).ToString();
        //        }


        //        if (yjfsl.ToString() != "")
        //        {
        //            ds8.Tables[0].Rows[i]["已交数量"] = yjfsl;
        //            ds8.Tables[0].Rows[i]["剩余金额"] = (Convert.ToInt32(ds8.Tables[0].Rows[i]["产品数量"]) - Convert.ToInt32(yjfsl)) * (Convert.ToDecimal(ds8.Tables[0].Rows[i]["提货价"]) + Convert.ToDecimal(ds8.Tables[0].Rows[i]["押金"]));
        //            if (Convert.ToDouble(ds8.Tables[0].Rows[i]["剩余金额"]) < 0)
        //            {
        //                ds8.Tables[0].Rows[i]["剩余金额"] = 0.00;
        //            }
        //        }
        //    }
        //}

        if (ds8 != null && ds8.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds8.Tables[0].Rows.Count; i++)
            {
                string ddh = ds8.Tables[0].Rows[i]["订单号"].ToString();
                string thfs = ds8.Tables[0].Rows[i]["提货方式"].ToString();
                string yjje = ds8.Tables[0].Rows[i]["押金"].ToString();
                string cpjg = ds8.Tables[0].Rows[i]["提货价"].ToString();
                string cppl = ds8.Tables[0].Rows[i]["产品品类"].ToString();
                string cpxh = ds8.Tables[0].Rows[i]["产品型号"].ToString();

                string db = "";
                if (thfs == "循环提货")
                {
                    db = "2313";
                }
                else if (thfs == "押金提货")
                {
                    if (yjje == "0.00")
                    {
                        db = "2302','2301";
                    }
                    else
                    {
                        db = "2312";
                    }
                }
                else if (thfs == "无押金提货")
                {
                    db = "2301";
                }
                string sqlXH = "";
                if (thfs == "循环提货" || thfs == "无押金提货" || (thfs == "押金提货" && yjje == "0.00"))
                {
                    sqlXH = "select isnull(sum(cast(TH008 as int)),0) as 已交付数量,isnull(sum(cast(TH013 as numeric(18,2))),0.00) as 已交金额 from COPTG left join COPTH on TG001=TH001 and TG002=TH002 and COPTG.TSFM_bsc=COPTH.TSFM_bsc where TG004='" + UserNumber + "' and COPTG.UDF01='" + ddh + "' and TG001 in ('" + db + "') and (case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end)='" + cppl + "' and ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005))))='" + cpxh + "' and TG023='Y' and cast(TH012 as numeric(18,2))=" + cpjg + "";
                }
                else if (thfs == "押金提货" && yjje != "0.00")
                {
                    sqlXH = "select isnull(sum(cast(已交付数量 as int)),0) as 已交付数量,isnull(sum(cast(提货金合计+押金合计 as numeric(18,2))),0.00) as 已交金额 from (select TH004,TH008 as 已交付数量,TH013 as 提货金合计 from COPTG left join COPTH on TG001=TH001 and TG002=TH002 and COPTG.TSFM_bsc=COPTH.TSFM_bsc where TG004='" + UserNumber + "' and COPTG.UDF01='" + ddh + "' and TG001 in ('" + db + "') and (case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end)='" + cppl + "' and substring(TH005,charindex(' ',TH005),len(TH005)) like '%" + cpxh + "%' and TG023='Y' and  cast(TH012 as numeric(18,2))='" + cpjg + "') as a left join (select distinct TH004 ,isnull(cast(TH013/TH012 as int),0) as 数量,isnull(cast(TH013 as numeric(18,2)),0.00) as 押金合计 from COPTG left join COPTH on TG001=TH001 and TG002=TH002 and COPTG.TSFM_bsc=COPTH.TSFM_bsc where TG004='" + UserNumber + "' and COPTG.UDF01='" + ddh + "' and TG001 in ('" + db + "') and (case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end)='" + cppl + "' and ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005))))='" + cpxh + "' and TG023='Y' and  cast(TH012 as numeric(18,2))='" + yjje + "') as b on a.TH004=b.TH004 and a.已交付数量=b.数量";
                }


                DataSet dsERP_XH = GetErpData.GetDataSet(sqlXH);
                if (dsERP_XH != null && dsERP_XH.Tables[0].Rows.Count > 0)
                {
                    ds8.Tables[0].Rows[i]["已交数量"] = dsERP_XH.Tables[0].Rows[0]["已交付数量"];
                    ds8.Tables[0].Rows[i]["剩余金额"] = (Convert.ToInt32(ds8.Tables[0].Rows[i]["产品数量"]) - Convert.ToInt32(dsERP_XH.Tables[0].Rows[0]["已交付数量"])) * (Convert.ToDecimal(ds8.Tables[0].Rows[i]["提货价"]) + Convert.ToDecimal(ds8.Tables[0].Rows[i]["押金"]));
                    if (Convert.ToDouble(ds8.Tables[0].Rows[i]["剩余金额"]) < 0)
                    {
                        ds8.Tables[0].Rows[i]["剩余金额"] = 0.00;
                    }
                }
            }
        }

        DataSet ds9 = GetErpData.GetDataSet(sql9, city);
        DataSet ds10 = GetErpData.GetDataSet(sql10,city);
        DataSet ds11 = DbHelperSQL.Query(sql11);
        DataSet ds12 = DbHelperSQL.Query(sql12);
        DataSet ds13 = DbHelperSQL.Query(sql13);
        DataSet ds14 = DbHelperSQL.Query(sql14);
        DataSet ds15 = DbHelperSQL.Query(sql15);
        DataSet ds15_1 = GetErpData.GetDataSet(sql15_1, city);

      
        double dERP = 0.00;
        double d8 = 0.00;
        double d9 = 0.00;
        double d10 = 0.00;
        double d11 = 0.00;
        double d12 = 0.00;
        double d13 = 0.00;
        double d14 = 0.00;
        double d15 = 0.00;
        double d15_1 = 0.00;
        if (dsERP != null && dsERP.Tables[0].Rows.Count > 0)
        {
            dERP = Convert.ToDouble(dsERP.Tables[0].Rows[0]["金额"].ToString());
        } 
        if (ds8 != null && ds8.Tables[0].Rows.Count > 0)
        {
            d8 = Convert.ToDouble(ds8.Tables[0].Compute("sum(剩余金额)", "true"));
        }
        if (ds9 != null && ds9.Tables[0].Rows.Count > 0)
        {
            d9 = Convert.ToDouble(ds9.Tables[0].Rows[0]["金额"]);
        }
        if (ds10 != null && ds10.Tables[0].Rows.Count > 0)
        {
            d10 = Convert.ToDouble(ds10.Tables[0].Rows[0]["金额"]);
        }
        if (ds11 != null && ds11.Tables[0].Rows.Count > 0)
        {
            d11 = Convert.ToDouble(ds11.Tables[0].Rows[0]["金额"]);
        }
        if (ds12 != null && ds12.Tables[0].Rows.Count > 0)
        {
            d12 = Convert.ToDouble(ds12.Tables[0].Rows[0]["金额"]);
        }
        if (ds13 != null && ds13.Tables[0].Rows.Count > 0)
        {
            d13 = Convert.ToDouble(ds13.Tables[0].Rows[0]["金额"]);
        }
        if (ds14 != null && ds14.Tables[0].Rows.Count > 0)
        {
            d14 = Convert.ToDouble(ds14.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds15 != null && ds15.Tables[0].Rows.Count > 0)
        {
            d15 = Convert.ToDouble(ds15.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds15_1 != null && ds15_1.Tables[0].Rows.Count > 0)
        {
            d15_1 = Convert.ToDouble(ds15_1.Tables[0].Rows[0]["金额"].ToString());
        }
      
        double xyed = dERP - d8 + (d9 + d12) - d11 - (d10 + d13) - d14 + d15 + d15_1;
        return xyed;
    
    }

}
