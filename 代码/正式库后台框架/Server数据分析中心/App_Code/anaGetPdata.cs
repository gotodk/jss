using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using FMDBHelperClass;
using FMipcClass;
using FMPublicClass;
using System.Collections;

/// <summary>
/// anaGetPdata 的摘要说明
/// </summary>
public class anaGetPdata
{
	public anaGetPdata()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 初始化返回值数据集,执行结果只有两种ok和err(大多数情况是这个标准)
    /// </summary>
    /// <returns></returns>
    private DataSet initReturnDataSet()
    {
        DataSet ds = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.TableName = "返回值单条";
        auto2.Columns.Add("执行结果");
        auto2.Columns.Add("提示文本");
        auto2.Columns.Add("附件信息1");
        auto2.Columns.Add("附件信息2");
        auto2.Columns.Add("附件信息3");
        auto2.Columns.Add("附件信息4");
        auto2.Columns.Add("附件信息5");
        ds.Tables.Add(auto2);
        return ds;
    }

    /// <summary>
    /// 获取资金转账C区分页数据
    /// </summary>
    /// <param name="ds_page">分页参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqZJZZ(DataSet ds_page,Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " a.XM as 项目,a.XZ as 性质,a.ZY as 摘要,a.JE as 金额,case when  a.YSLX in ('增加','解冻') then '+'+convert(varchar(20),convert(decimal(18,2),a.JE)) else '-'+convert(varchar(20),convert(decimal(18,2),a.JE)) end as 带符号金额,a.LSCSSJ as 时间 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_ZKLSMXB as a left join AAA_moneyDZB as b on a.xm=b.xm and a.xz=b.xz and a.sjlx=b.sjlx ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " a.LSCSSJ Desc,a.Number ";  //用于排序的字段(必须设置)
            // ds_page.Tables[0].Rows[0]["method_retreatment"] = "类名|方法名"; //用于二次处理时传入类名和方法名 


            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱") || !ht_tiaojian.Contains("数据类型") || !ht_tiaojian.Contains("模块编号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and a.dlyx='" + ht_tiaojian["用户邮箱"].ToString() + "' and a.sjlx='" + ht_tiaojian["数据类型"].ToString() + "'";

            if (ht_tiaojian["模块编号"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and isnull(b.dymkbh,'')='" + ht_tiaojian["模块编号"].ToString() + "' ";
            }

            if (ht_tiaojian.Contains("开始时间") && ht_tiaojian.Contains("结束时间"))
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and convert(varchar(10), a.LSCSSJ,120)>='" + ht_tiaojian["开始时间"].ToString() + "' and convert(varchar(10), a.LSCSSJ,120)<='" + ht_tiaojian["结束时间"].ToString() + "' ";
            }
            if (ht_tiaojian.Contains("项目"))
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and a.xm='" + ht_tiaojian["项目"].ToString() + "' ";
            }
            if (ht_tiaojian.Contains("性质"))
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and a.xz='" + ht_tiaojian["性质"].ToString() + "' ";
            }
            /*-----2014-10-9 shiyan 解决负载均衡自己调自己会卡一下的问题，改为直接调用----
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。                
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
             -----------------------------------------------------------------*/
            
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出资金转账C区中的数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>    
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquZJZZ(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //设置导出语句
        string sql = "select a.LSCSSJ as 时间,a.ZY as 摘要,a.XM as 项目,a.XZ as 性质,a.JE as 金额,case when  a.YSLX in ('增加','解冻') then '+'+convert(varchar(20),convert(decimal(18,2),a.JE)) else '-'+convert(varchar(20),convert(decimal(18,2),a.JE)) end as 带符号金额  from  AAA_ZKLSMXB as a left join AAA_moneyDZB as b on a.xm=b.xm and a.xz=b.xz  and a.sjlx=b.sjlx where 1=1 ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("用户邮箱") || !ht_tiaojian.Contains("数据类型") || !ht_tiaojian.Contains("模块编号"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }

        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@sjlx"] = ht_tiaojian["数据类型"].ToString();
        input["@dymkbh"] = ht_tiaojian["模块编号"].ToString();

        sql += " and a.dlyx=@dlyx and a.sjlx=@sjlx";

        if (ht_tiaojian["模块编号"].ToString().Trim() != "")
        {            
            sql += " and isnull(b.dymkbh,'')=@dymkbh ";
        }
        if (ht_tiaojian.Contains("开始时间") && ht_tiaojian.Contains("结束时间"))
        {
            input["@begintime"] = ht_tiaojian["开始时间"].ToString();
            input["@endtime"] = ht_tiaojian["结束时间"].ToString();

            sql += " and convert(varchar(10), a.LSCSSJ,120)>=@begintime and convert(varchar(10), a.LSCSSJ,120)<=@endtime ";
        }
        if (ht_tiaojian.Contains("项目"))
        {
            input["@xm"] = ht_tiaojian["项目"].ToString();
            sql += " and a.xm=@xm ";
        }
        if (ht_tiaojian.Contains("性质"))
        {
            input["@xz"] = ht_tiaojian["性质"].ToString();
            sql += " and a.xz=@xz ";
        }

        sql = sql + " order by a.lscssj desc,a.number desc";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

            //删除需要隐藏的列           
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
                if (ht_tiaojian["模块编号"].ToString() == "" || ht_tiaojian["模块编号"].ToString() == "3")
                {
                    ds_data.Tables[0].Columns.Remove("金额");
                    ds_data.Tables[0].Columns["带符号金额"].ColumnName = "金额";
                }
                else
                {
                    ds_data.Tables[0].Columns.Remove("带符号金额");
                }
               
                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 商品买卖C区获取分页数据
    /// </summary>
    /// <param name="ds_page">分页使用参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqSPMM(DataSet ds_page, Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        { 
            case "商品买卖概况":
                dsreturn = GetCqSPMMGK(ds_page, ht_tiaojian);
                break;
            case "竞标中":
                dsreturn = GetCqJBZ(ds_page,ht_tiaojian);
                break;
            case "冷静期":
                dsreturn = GetCqLJQ(ds_page, ht_tiaojian);
                break;
            case "中标":
                dsreturn = GetCqZB(ds_page, ht_tiaojian);
                break;
            case "定标与保证函":
                dsreturn = GetCqDBYBZH(ds_page, ht_tiaojian);
                break;
            case "废标":
                dsreturn = GetCqFB(ds_page, ht_tiaojian);
                break;
            case "清盘":
                dsreturn = GetCqQP(ds_page, ht_tiaojian);
                break;
            case "草稿箱":
                dsreturn = GetCqCGX(ds_page, ht_tiaojian);
                break;
                 case "违约扣罚记录":
                dsreturn =GetCqWYKFJL(ds_page,ht_tiaojian);
                break;
            default:
                break;
        }
        return dsreturn;
    }

    /// <summary>
    /// 商品买卖C区数据导出
    /// </summary>
    /// <param name="ht_tiaojian">查询条件</param>   
    /// <returns></returns>
    public DataSet Export_CquSPMM(Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "商品买卖概况":
                dsreturn = Export_CquSPMMGK(ht_tiaojian);
                break;
            case "竞标中":
                dsreturn = Export_CquJBZ(ht_tiaojian);
                break;
            case "冷静期":
                dsreturn = Export_CquLJQ(ht_tiaojian);
                break;
            case "中标":
                dsreturn = Export_CquZB(ht_tiaojian);
                break;
            case "定标与保证函":
                dsreturn = Export_CquDBYBZH(ht_tiaojian);
                break;
            case "废标":
                dsreturn = Export_CquFB(ht_tiaojian);
                break;
            case "清盘":
                dsreturn = Export_CquQP(ht_tiaojian);
                break;
            case "草稿箱":
                dsreturn = Export_CquCGX(ht_tiaojian);
                break;           
            default:
                break;
        }
        return dsreturn;
    }

    #region 商品买卖C区各标签获取分页数据及导出功能
    /// <summary>
    /// 商品买卖C区商品买卖概况
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqSPMMGK(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " *,'' as 当前状态,'--' as 当前卖方最低价,'--' as 最低价标的经济批量,'--' as [最低价标的达成率/中标率]  "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_View_SPMMGKinfo ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 编号 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 下单时间 ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and 交易方账号='" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

            if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 单据类型='" + ht_tiaojian["单据类型"].ToString() + "' ";
            }           
            /*
            //调用执行方法获取数据            
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                DataSet ds_old = (DataSet)re_ds[1];
                if (ds_old.Tables[0].TableName == "主要数据")
                {
                    //对其中的部分字段进行二次处理
                    anaPageRetreat apr = new anaPageRetreat();
                    ds_res = apr.SPMMGK(ds_old);
                }
                else
                {
                    ds_res = ds_old.Copy();
                }
            }
            else
            {
                ds_res = null;
            }
             */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            DataSet ds_old = pd.GetPagerDB(ds_page);
            if (ds_old.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }
            else if (ds_old.Tables[0].TableName == "主要数据")
            {//如果返回值中的表0为主要数据，则对其中的部分字段进行二次处理
                anaPageRetreat apr = new anaPageRetreat();
                ds_res = apr.SPMMGK(ds_old);
            }
            else
            {//如果有表，表0不是主要数据，则不做处理直接返回
                ds_res = ds_old.Copy();
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出商品买卖C区商品买卖概况数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>    
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquSPMMGK(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //设置导出语句
        string sql = "select a.下单时间,a.商品编号,a.商品名称,a.规格,a.合同期限,a.数量,a.价格,a.金额,a.单据类型,a.单据编号,(case a.单据状态 when '未定标废标' then '废标' when '定标合同终止' then '废标' when '定标合同到期' then '废标' when '定标执行完成' then '定标' when '竞标' then (case isnull(b.SFJRLJQ,'否') when '否' then '竞标中' when '是' then '竞标中（冷静期）' end) else a.单据状态 end) as 当前状态,a.是否拆单,a.供货或收货区域  from AAA_View_SPMMGKinfo as a left join AAA_LJQDQZTXXB as b on a.商品编号=b.SPBH and a.合同期限=b.HTQX where 1=1 ";      

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("用户邮箱"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }
        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();
        input["@djlx"] = ht_tiaojian["单据类型"].ToString();

        sql += " and 交易方账号=@dlyx ";

        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and 商品名称 like '%'+@spmc+'%' ";
        }

        if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
        {
            sql += " and 单据类型=@djlx ";
        }           

        sql = sql + " order by 下单时间 desc";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data",input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

                 
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
                //删除需要隐藏的列
                //ds_data.Tables[0].Columns.Remove("");               
                
                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }


    /// <summary>
    /// 商品买卖C区竞标中
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqJBZ(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " tab.*,'--' as 当前卖方最低价,'--' as 最低价标的经济批量,'--' as [最低价标的达成率/中标率] "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_View_SPJingBiao as tab left join AAA_LJQDQZTXXB  as ljq on ljq.SPBH=tab.商品编号 and ljq.HTQX=tab.合同期限 ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " tab.单据编号+tab.单据类型 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " isnull(ljq.SFJRLJQ,'否')='否' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " tab.下单时间 ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and 交易方账号='" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

            if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 单据类型='" + ht_tiaojian["单据类型"].ToString() + "' ";
            }
            /*
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                DataSet ds_old = (DataSet)re_ds[1];
                if (ds_old.Tables[0].TableName == "主要数据")
                {
                    //对其中的部分字段进行二次处理
                    anaPageRetreat apr = new anaPageRetreat();
                    ds_res = apr.JingBiao(ds_old);
                }
                else
                {
                    ds_res = ds_old.Copy();
                }
            }
            else
            {
                ds_res = null;
            }
             */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            DataSet ds_old = pd.GetPagerDB(ds_page);
            if (ds_old.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }
            else if (ds_old.Tables[0].TableName == "主要数据")
            {//如果返回值中的表0为主要数据，则对其中的部分字段进行二次处理
                anaPageRetreat apr = new anaPageRetreat();
                ds_res = apr.JingBiao(ds_old);
            }
            else
            {//如果有表，表0不是主要数据，则不做处理直接返回
                ds_res = ds_old.Copy();
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出商品买卖C区中竞标中数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>  
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquJBZ(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //设置导出语句
        string sql = "select tab.* from AAA_View_SPJingBiao as tab left join AAA_LJQDQZTXXB as ljq on ljq.SPBH=tab.商品编号 and ljq.HTQX=tab.合同期限 where isnull(ljq.SFJRLJQ,'否')='否' ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("用户邮箱"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }

        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();
        input["@djlx"] = ht_tiaojian["单据类型"].ToString();

        sql += " and 交易方账号=@dlyx ";

        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and 商品名称 like '%'+@spmc+'%' ";
        }

        if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
        {
            sql += " and 单据类型=@djlx ";
        }

        sql = sql + " order by tab.下单时间 desc ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

                     
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
                //删除需要隐藏的列
                ds_data.Tables[0].Columns.Remove("交易方账号");                   

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 商品买卖C区冷静期
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqLJQ(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " ljq.LJQJSSJ as 冷静期结束时间,tab.*, '--' as 当前卖方最低价,'--' as 最低价标的经济批量,'--' as [最低价标的达成率/中标率] "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_View_SPJingBiao as tab left join AAA_LJQDQZTXXB  as ljq on ljq.SPBH=tab.商品编号 and ljq.HTQX=tab.合同期限 ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " tab.单据编号+tab.单据类型 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " isnull(ljq.SFJRLJQ,'否')='是' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " tab.下单时间 ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and tab.交易方账号='" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and tab.商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

            if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and tab.单据类型='" + ht_tiaojian["单据类型"].ToString() + "' ";
            }
            /*------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                DataSet ds_old = (DataSet)re_ds[1];
                if (ds_old.Tables[0].TableName == "主要数据")
                {
                    //对其中的部分字段进行二次处理
                    anaPageRetreat apr = new anaPageRetreat();
                    ds_res = apr.LengJingQi(ds_old);
                }
                else
                {
                    ds_res = ds_old.Copy();
                }
            }
            else
            {
                ds_res = null;
            }
             --------------------------------------------------------------*/
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            DataSet ds_old = pd.GetPagerDB(ds_page);
            if (ds_old.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }
            else if (ds_old.Tables[0].TableName == "主要数据")
            {//如果返回值中的表0为主要数据，则对其中的部分字段进行二次处理           
                anaPageRetreat apr = new anaPageRetreat();
                ds_res = apr.LengJingQi(ds_old);
            }
            else
            {//如果有表，表0不是主要数据，则不做处理直接返回
                ds_res = ds_old.Copy();
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 导出商品买卖C区中冷静期数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>  
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquLJQ(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //设置导出语句
        string sql = "select ljq.LJQJSSJ as 冷静期结束时间,tab.* from AAA_View_SPJingBiao as tab left join AAA_LJQDQZTXXB  as ljq on ljq.SPBH=tab.商品编号 and ljq.HTQX=tab.合同期限 where isnull(ljq.SFJRLJQ,'否')='是' ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("用户邮箱"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }

        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();
        input["@djlx"] = ht_tiaojian["单据类型"].ToString();

        sql += " and 交易方账号=@dlyx ";

        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and 商品名称 like '%'+@spmc+'%' ";
        }

        if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
        {
            sql += " and 单据类型=@djlx ";
        }

        sql = sql + " order by tab.下单时间 desc ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

                       
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
               //删除需要隐藏的列
                ds_data.Tables[0].Columns.Remove("交易方账号");
                   
                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }


    /// <summary>
    /// 商品买卖C区中标
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqZB(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " * "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_View_SPZhongBiao ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 中标定标编号+单据类型 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 中标时间 DESC,下单时间 ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and 交易方账号='" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

            if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 单据类型='" + ht_tiaojian["单据类型"].ToString() + "' ";
            }
            /*-----------2014-10-9 shiyan 解决负载均衡调自己会卡一下的问题，改为直接调用------------------ 
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];               
            }
            else
            {
                ds_res = null;
            }
             */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 导出商品买卖C区中 中标数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>   
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquZB(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //设置导出语句
        string sql = "select 中标时间,下单时间,商品编号,商品名称,规格,合同期限,数量 as 中标数量,价格 as 中标价格,金额 as 中标金额,单据类型 as 单据类别,单据编号,订金冻结金额,投标保证金冻结金额,是否拆单,供货或收货区域 from AAA_View_SPZhongBiao where 1=1 ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("用户邮箱"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }

        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();
        input["@djlx"] = ht_tiaojian["单据类型"].ToString();

        sql += " and 交易方账号=@dlyx ";

        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and 商品名称 like '%'+@spmc+'%' ";
        }

        if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
        {
            sql += " and 单据类型=@djlx ";
        }

        sql = sql + " order by 中标时间 DESC,下单时间 ASC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

                       
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
               //删除需要隐藏的列
                //ds_data.Tables[0].Columns.Remove("");              

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 商品买卖C区定标与保证函
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqDBYBZH(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " *,'违约扣罚记录' as 违约扣罚记录,(case 单据类型 when '预订单' then '--' else '100.00' end) as 履约保证金冻结剩余比例 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_View_SPDingBiao as tab ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 中标定标编号+单据类型 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 定标时间 ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and 交易方账号='" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

            if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 单据类型='" + ht_tiaojian["单据类型"].ToString() + "' ";
            }
            /*---------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                DataSet ds_old = (DataSet)re_ds[1];
                if (ds_old.Tables[0].TableName == "主要数据")
                {
                    //对其中的部分字段进行二次处理
                    anaPageRetreat apr = new anaPageRetreat();
                    ds_res = apr.DingBiao(ds_old);
                }
                else
                {
                    ds_res = ds_old.Copy();
                }
            }
            else
            {
                ds_res = null;
            }
            ------------------------------------------------------------------- */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            DataSet ds_old = pd.GetPagerDB(ds_page);
            if (ds_old.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }
            else if (ds_old.Tables[0].TableName == "主要数据")
            {//如果返回值中的表0为主要数据，则对其中的部分字段进行二次处理
                anaPageRetreat apr = new anaPageRetreat();
                ds_res = apr.DingBiao(ds_old);
            }
            else
            {//如果有表，表0不是主要数据，则不做处理直接返回
                ds_res = ds_old.Copy();
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 导出商品买卖C区定标与保证函数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>   
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquDBYBZH(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //设置导出语句
        string sql = "select 合同编号,定标时间,下单时间,单据类型,单据编号,商品编号,商品名称,规格,合同期限,定标数量,定标价格,定标金额,投标保证金解冻金额,订金冻结金额,履约保证金冻结金额,(case 单据类型 when '预订单' then '--' else '100.00' end) as [履约保证金冻结剩余比例%],是否拆单,中标定标编号 from AAA_View_SPDingBiao as tab where 1=1 ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("用户邮箱"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }

        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();
        input["@djlx"] = ht_tiaojian["单据类型"].ToString();

        sql += " and tab.交易方账号=@dlyx ";

        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and tab.商品名称 like '%'+@spmc+'%' ";
        }

        if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
        {
            sql += " and tab.单据类型=@djlx ";
        }

        sql = sql + " order by 定标时间 asc ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
           DataSet  ds_old = (DataSet)ht_res["return_ds"];
           if (ds_old != null && ds_old.Tables[0].Rows.Count > 0)
            {
                anaPageRetreat apr = new anaPageRetreat();
                ds_data = apr.DingBiao(ds_old);
                
               //删除需要隐藏的列
                ds_data.Tables[0].Columns.Remove("中标定标编号");                    

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }


    /// <summary>
    /// 商品买卖C区废标
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqFB(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " *,'--' as 履约保证金扣罚金额 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = "  AAA_View_SPFeiBiao ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 中标定标编号+单据类型 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 废标时间 DESC,下单时间 ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and 交易方账号='" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

            if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 单据类型='" + ht_tiaojian["单据类型"].ToString() + "' ";
            }
            /*-----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                DataSet ds_old = (DataSet)re_ds[1];
                if (ds_old.Tables[0].TableName == "主要数据")
                {
                    //对其中的部分字段进行二次处理
                    anaPageRetreat apr = new anaPageRetreat();
                    ds_res = apr.FeiBiao(ds_old);
                }
                else
                {
                    ds_res = ds_old.Copy();
                }
            }
            else
            {
                ds_res = null;
            }
           --------------------------------------------------------------------*/
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            DataSet ds_old = pd.GetPagerDB(ds_page);
            if (ds_old.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }
            else if (ds_old.Tables[0].TableName == "主要数据")
            {//如果返回值中的表0为主要数据，则对其中的部分字段进行二次处理
                anaPageRetreat apr = new anaPageRetreat();
                ds_res = apr.FeiBiao(ds_old);
            }
            else
            {//如果有表，表0不是主要数据，则不做处理直接返回
                ds_res = ds_old.Copy();
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 导出商品买卖C区废标数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>    
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquFB(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //设置导出语句
        string sql = "select tab.废标时间,tab.废标原因,tab.下单时间,tab.商品编号,tab.商品名称,tab.规格,tab.合同期限,tab.数量,tab.价格,tab.金额,tab.单据类型,tab.单据编号,tab.订金解冻金额,tab.投标保证金扣罚金额,'--' as 履约保证金扣罚金额,tab.是否拆单,tab.中标定标编号 from AAA_View_SPFeiBiao as tab where 1=1 ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("用户邮箱"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }

        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();
        input["@djlx"] = ht_tiaojian["单据类型"].ToString();

        sql += " and tab.交易方账号=@dlyx ";

        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and tab.商品名称 like '%'+@spmc+'%' ";
        }

        if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
        {
            sql += " and tab.单据类型=@djlx ";
        }

        sql = sql + " order by tab.废标时间 desc,tab.下单时间 ASC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
           DataSet  ds_old = (DataSet)ht_res["return_ds"];
           if (ds_old != null && ds_old.Tables[0].Rows.Count > 0)
            {
                anaPageRetreat apr = new anaPageRetreat();
                ds_data = apr.FeiBiao(ds_old);
               //删除需要隐藏的列
                ds_data.Tables[0].Columns.Remove("中标定标编号");                   

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 商品买卖C区清盘
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqQP(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            string dlyx = ht_tiaojian["用户邮箱"].ToString();
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " Number,CONVERT(varchar(20),Z_QPKSSJ, 20) as '清盘开始时间',(case  when  Z_QPJSSJ IS NULL then '---' else CONVERT(varchar(20), Z_QPJSSJ, 20)   end) as '清盘结束时间',(CASE Z_HTZT WHEN '未定标废标' THEN '废标' WHEN '定标合同到期' THEN '合同期满' WHEN '定标合同终止' THEN '废标' WHEN '定标执行完成' THEN '合同完成' ELSE '其他' END) AS 清盘原因,(case when Z_QPKSSJ=Z_QPJSSJ then '自动清盘' else '人工清盘' end) as 清盘类型,Z_HTBH as 合同编号 , Z_QPZT as 清盘状态,(case Y_YSYDDDLYX when '" + dlyx + "' then convert(varchar(20),Z_DJJE) else '---' end) as 订金解冻金额 ,(case  when T_YSTBDDLYX='" + dlyx + "' and Z_QPZT='清盘结束' then convert(varchar(20),Z_LYBZJJE) else '---' end)  as 履约保证金解冻金额 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = "  AAA_ZBDBXXB ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " (Z_QPZT='清盘中' or Z_QPZT='清盘结束') ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " Z_QPKSSJ ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and (T_YSTBDDLYX='" + ht_tiaojian["用户邮箱"].ToString() + "' or Y_YSYDDDLYX='" + ht_tiaojian["用户邮箱"].ToString() + "') ";

            if (ht_tiaojian["合同编号"].ToString().Trim() != ""&&ht_tiaojian["合同编号"].ToString().Trim() !="电子购货合同编号")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and Z_HTBH like '%" + ht_tiaojian["合同编号"].ToString() + "%' ";
            }

            if (ht_tiaojian["清盘状态"].ToString() != "请选择清盘状态")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and Z_QPZT='" + ht_tiaojian["清盘状态"].ToString() + "' ";
            }
            if (ht_tiaojian["清盘类型"].ToString() != "请选择清盘类型")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and (case when Z_QPKSSJ=Z_QPJSSJ then '自动清盘' else '人工清盘' end)='" + ht_tiaojian["清盘类型"].ToString() + "' ";
            }
            /*----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                 ds_res = (DataSet)re_ds[1];              
            }
            else
            {
                ds_res = null;
            }
            ------------------------------------------------------------------------ */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }           
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 导出商品买卖C区清盘标数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>   
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquQP(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //设置导出语句
        string sql = "select CONVERT(varchar(20), Z_QPKSSJ, 20) as '清盘开始时间',(CASE Z_HTZT WHEN '未定标废标' THEN '废标' WHEN '定标合同到期' THEN '合同期满' WHEN '定标合同终止' THEN '废标' WHEN '定标执行完成' THEN '合同完成' ELSE '其他' END) AS 清盘原因,(case when Z_QPKSSJ=Z_QPJSSJ then '自动清盘' else '人工清盘' end) as 清盘类型, Z_QPZT as 清盘状态,(case  when  Z_QPJSSJ IS NULL then '---' else CONVERT(varchar(20), Z_QPJSSJ, 20)   end) as '清盘结束时间',Z_HTBH as 电子购货合同编号 ,(case Y_YSYDDDLYX when '" + ht_tiaojian["用户邮箱"].ToString() + "' then convert(varchar(20),Z_DJJE) else '---' end) as 订金解冻金额 ,(case  when T_YSTBDDLYX='" + ht_tiaojian["用户邮箱"].ToString() + "' and Z_QPZT='清盘结束' then convert(varchar(20),Z_LYBZJJE) else '---' end)  as 履约保证金解冻金额 from AAA_ZBDBXXB where (Z_QPZT='清盘中' or Z_QPZT='清盘结束') ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("用户邮箱"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }

        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@htbh"] = ht_tiaojian["合同编号"].ToString();
        input["@qpzt"] = ht_tiaojian["清盘状态"].ToString();
        input["@qplx"] = ht_tiaojian["清盘类型"].ToString();

       sql += " and (T_YSTBDDLYX=@dlyx or Y_YSYDDDLYX=@dlyx) ";

        if (ht_tiaojian["合同编号"].ToString().Trim() != "" && ht_tiaojian["合同编号"].ToString().Trim() != "电子购货合同编号")
        {
            sql += " and Z_HTBH like '%'+@htbh+'%' ";
        }

        if (ht_tiaojian["清盘状态"].ToString() != "请选择清盘状态")
        {
           sql += " and Z_QPZT=@qpzt ";
        }
        if (ht_tiaojian["清盘类型"].ToString() != "请选择清盘类型")
        {
            sql += " and (case when Z_QPKSSJ=Z_QPJSSJ then '自动清盘' else '人工清盘' end)=@qplx";
        }

        sql = sql + " order by Z_QPKSSJ ASC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

                       
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
               //删除需要隐藏的列
                //ds_data.Tables[0].Columns.Remove("");                    

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 商品买卖C区草稿箱
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqCGX(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。           
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " tab.*,b.SPMC as 商品名称,b.GG as 规格 "; //检索字段(必须设置)          
            ds_page.Tables[0].Rows[0]["search_tbname"] = "  (select DLYX, '投标单' as  '单据类型', Number as 单据编号,HTQX as '合同期限',SPBH as  '商品编号',TBNSL as '数量',TBJG as '价格',TBJE as '金额',CreateTime from AAA_TBDCGB  union all select DLYX,'预订单' as  '单据类型',Number as 单据编号,HTQX as '合同期限',SPBH as '商品编号',NDGSL as '数量',NMRJG as '价格',NDGJE as '金额',CreateTime from AAA_YDDCGB) as tab left join AAA_PTSPXXB as b on tab.商品编号 = b.SPBH ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " tab.单据编号+tab.单据类型 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " CreateTime ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and tab.DLYX = '" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.SPMC like '%" + ht_tiaojian["商品名称"].ToString () + "%' ";
            }

            if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 单据类型='" + ht_tiaojian["单据类型"].ToString() + "' ";
            }            
            /*----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            ----------------------------------------------------------------------- */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }           
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 导出商品买卖C区草稿箱数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>  
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquCGX(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //设置导出语句
        string sql = "select tab.单据类型,tab.商品编号,b.SPMC as 商品名称,b.GG as 规格,tab.合同期限,tab.数量,tab.价格,tab.金额,tab.CreateTime,tab.Number from (select DLYX, '投标单' as  '单据类型', Number,HTQX as '合同期限',SPBH as  '商品编号',TBNSL as '数量',TBJG as '价格',TBJE as '金额',CreateTime from AAA_TBDCGB  union all select DLYX,'预订单' as  '单据类型',Number,HTQX as '合同期限',SPBH as  '商品编号',NDGSL as '数量',NMRJG as '价格',NDGJE as '金额',CreateTime from AAA_YDDCGB) as tab left join  AAA_PTSPXXB as b on tab.商品编号 = b.SPBH where 1=1  ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("用户邮箱"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }

        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();
        input["@djlx"] = ht_tiaojian["单据类型"].ToString();        

        sql += " and tab.DLYX = @dlyx ";

        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and b.SPMC like '%'+@spmc+'%' ";
        }

        if (ht_tiaojian["单据类型"].ToString() != "请选择单据类别")
        {
            sql += " and 单据类型=@djlx ";
        }            

        sql = sql + " order by tab.CreateTime ASC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

                       
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
                //删除需要隐藏的列
                ds_data.Tables[0].Columns.Remove("CreateTime");
                ds_data.Tables[0].Columns.Remove("Number");   

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 商品买卖C区定标与保证函中的违约扣罚记录弹窗内容
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqWYKFJL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {  
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。           
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " LSCSSJ as 流水产生时间,YSLX as 运算类型,JE as 金额,XM as 项目,XZ as 性质 ,ZY as 摘要,SJLX as 数据类型, Number "; //检索字段(必须设置)          
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_ZKLSMXB ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " SJLX = '预' and (XZ='超过最迟发货日后录入发货信息' or XZ='发货单生成后5日内未录入发票邮寄信息') ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " LSCSSJ ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("来源单号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and LYDH in (select Number  from  AAA_THDYFHDXXB where  ZBDBXXBBH = '" + ht_tiaojian["来源单号"].ToString() + "') ";
         
            /*---------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            ----------------------------------------------------------------- */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }           
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
    #endregion

   /// <summary>
   /// 货物收发C区分页
   /// </summary>
   /// <param name="ds_page">分页参数</param>
   /// <param name="ht_tiaojian">查询条件</param>
   /// <returns></returns>
    public DataSet GetCqHWSF(DataSet ds_page, Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "货物收发概况":
                dsreturn = GetCqHWSFGK(ds_page, ht_tiaojian);
                break;
            case "已下达提货单":
                dsreturn = GetCqYXDTHD(ds_page, ht_tiaojian);
                break;
            case "货物签收":
                dsreturn = GetCqHWQS(ds_page, ht_tiaojian);
                break;
            case "货物发出":
                dsreturn = GetCqHWFC(ds_page, ht_tiaojian);
                break;
            case "问题与处理":
                dsreturn = GetCqWTYCL(ds_page, ht_tiaojian);
                break;
            default:
                break;
        }
        return dsreturn;
    }
    /// <summary>
    /// 货物收发C区导出
    /// </summary>
    /// <param name="ht_tiaojian">查询条件</param>   
    /// <returns></returns>
    public DataSet Export_CquHWSF(Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "货物收发概况":
                dsreturn = Export_CquHWSFGK(ht_tiaojian);
                break;
            case "已下达提货单":
                dsreturn = Export_CquYXDTHD(ht_tiaojian);
                break;
            case "货物签收":
                dsreturn = Export_CquHWQS(ht_tiaojian);
                break;
            case "货物发出":
                dsreturn = Export_CquHWFC(ht_tiaojian);
                break;
            case "问题与处理":
                dsreturn = Export_CquWTYCL(ht_tiaojian);
                break;
            default:
                break;
        }
        return dsreturn;
    }

    #region 货物收发C区各标签获取分页数据及导出功能
    /// <summary>
    /// 货物收发C区货物收发概况
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqHWSFGK(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " * "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " (select a.Number as 投标定标单号, a.Z_SPMC as 商品名称,a.Z_SPBH as 商品编号,a.Z_GG as 规格 ,a.Z_HTBH as 对应的电子购货合同编号,a.Z_ZBSL as 定标数量,a.Z_ZBJG as 定标价格,a.Z_ZBJE as 定标金额 , sum((case  when ( b.F_DQZT='无异议收货' or b.F_DQZT='默认无异议收货' or b.F_DQZT='补发货物无异议收货' or b.F_DQZT='有异议收货后无异议收货') then isnull(b.T_THSL,0) else 0 end )) as 无异议收货数量, sum((case when (b.F_DQZT='有异议收货' or b.F_DQZT='部分收货' ) then isnull(b.T_THSL,0) else 0 end) ) as 有异议收货数量,  case when a.Z_HTJSRQ is null then '---' else CONVERT(varchar(100), a.Z_HTJSRQ, 20) end as 对应电子购货合同结束日期, case a.Z_QPZT when '未开始清盘' then '定标' else case a.Z_HTZT when '定标合同到期' then '合同期满'+a.Z_QPZT when '定标合同终止' then '废标'+a.Z_QPZT when '定标执行完成' then '合同完成'+a.Z_QPZT else '异常'   end   end as '合同状态'  from   AAA_ZBDBXXB as a left join AAA_THDYFHDXXB as b on a.Number = b.ZBDBXXBBH  where   (a.Z_HTZT <> '中标' and a.Z_HTZT <> '未定标废标' ) and ((a.T_YSTBDMJJSBH='" + ht_tiaojian["卖家编号"].ToString () + "') or ( a.Y_YSYDDMJJSBH = '" + ht_tiaojian["买家编号"].ToString () + "' ) )   group by a.Number, a.Z_SPMC,a.Z_SPBH,a.Z_GG,a.Z_HTBH ,a.Z_ZBSL,a.Z_ZBJG,a.Z_ZBJE,a.Z_HTJSRQ,a.Z_QPZT,a.Z_HTZT) as tab ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 投标定标单号 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 对应电子购货合同结束日期 ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

            if (ht_tiaojian["合同编号"].ToString() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 对应的电子购货合同编号 like '%" + ht_tiaojian["合同编号"].ToString() + "%' ";
            }
            /*-----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];                
            }
            else
            {
                ds_res = null;
            }
            -------------------------------------------------------------------- */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }           
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出货物收发C区货物收发概况数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>    
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquHWSFGK(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件 
        Hashtable input = new Hashtable();
        input["@saler"] = ht_tiaojian["卖家编号"].ToString();
        input["@buyer"] = ht_tiaojian["买家编号"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();
        input["@htbh"] = ht_tiaojian["合同编号"].ToString();

        //设置导出语句
        string sql = "select * from (select a.Number as 投标定标单号, a.Z_SPMC as 商品名称,a.Z_SPBH as 商品编号,a.Z_GG as 规格 ,a.Z_HTBH as 对应的电子购货合同编号,a.Z_ZBSL as 定标数量,a.Z_ZBJG as 定标价格,a.Z_ZBJE as 定标金额 , sum( (case  when ( b.F_DQZT='无异议收货' or b.F_DQZT='默认无异议收货' or b.F_DQZT='补发货物无异议收货' or b.F_DQZT='有异议收货后无异议收货'  ) then isnull(b.T_THSL,0) else 0 end )) as 无异议收货数量, sum( (case when (b.F_DQZT='有异议收货' or b.F_DQZT='部分收货' ) then isnull(b.T_THSL,0) else 0 end) ) as 有异议收货数量,  (case when a.Z_HTJSRQ is null then '---' else CONVERT(varchar(100), a.Z_HTJSRQ, 20) end) as 对应电子购货合同结束日期,(case a.Z_QPZT when '未开始清盘' then '定标' else (case a.Z_HTZT when '定标合同到期' then '合同期满'+a.Z_QPZT when '定标合同终止' then '废标'+a.Z_QPZT when '定标执行完成' then '合同完成'+a.Z_QPZT else '异常'   end)   end) as 合同状态  from   AAA_ZBDBXXB as a left join AAA_THDYFHDXXB as b on a.Number = b.ZBDBXXBBH  where   (a.Z_HTZT <> '中标' and a.Z_HTZT <> '未定标废标' ) and ((a.T_YSTBDMJJSBH=@saler) or ( a.Y_YSYDDMJJSBH = @buyer) )  group by a.Number, a.Z_SPMC,a.Z_SPBH,a.Z_GG,a.Z_HTBH ,a.Z_ZBSL,a.Z_ZBJG,a.Z_ZBJE,a.Z_HTJSRQ,a.Z_QPZT,a.Z_HTZT) as tab  where 1=1 ";    

        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and 商品名称 like '%'+@spmc+'%' ";
        }

        if (ht_tiaojian["合同编号"].ToString() != "")
        {
            sql += " and 对应的电子购货合同编号 like '%'+@htbh+'%' ";
        }

        sql = sql + " order by 对应电子购货合同结束日期 ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

                      
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
                //删除需要隐藏的列 
                ds_data.Tables[0].Columns.Remove("投标定标单号");
                    

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 货物收发C区已下达提货单
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqYXDTHD(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " d.Number 中标定标表编号,d.Z_HTBH 电子购货合同编号,t.T_THDXDSJ 下达时间,'T'+t.Number 提货单编号,d.Z_SPBH 商品编号,d.Z_SPMC 商品名称,d.Z_GG 规格,t.T_THSL 提货数量,t.ZBDJ 定标价格,t.T_DJHKJE 提货金额,t.T_DJHKJE 冻结货款金额,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.Y_YSYDDDLYX where DDB.Number=d.Number),'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),t.F_DQZT 提货单状态, case d.Z_QPZT when '未开始清盘' then '定标' else case d.Z_HTZT when '定标合同到期' then '合同期满'+d.Z_QPZT when '定标合同终止' then '废标'+d.Z_QPZT when '定标执行完成' then '合同完成'+d.Z_QPZT else '异常'   end   end as '合同状态' "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 'T'+t.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " t.F_DQZT in ('未生成发货单','已生成发货单') ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " t.T_THDXDSJ ";  //用于排序的字段(必须设置)   


            if (!ht_tiaojian.Contains("用户邮箱"))
            {
                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += "and d.Y_YSYDDDLYX='" + ht_tiaojian["用户邮箱"].ToString() + "'";

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and d.Z_SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

            if (ht_tiaojian["合同编号"].ToString() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and d.Z_HTBH like '%" + ht_tiaojian["合同编号"].ToString() + "%' ";
            }
            if (ht_tiaojian["提货单号"].ToString() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 'T'+t.Number like '%" + ht_tiaojian["提货单号"].ToString() + "%' ";
            }
            /*-------------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
             --------------------------------------------------------------------*/
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }           
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出货物收发C区已下达提货单数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>   
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquYXDTHD(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件 
        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();        
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();
        input["@htbh"] = ht_tiaojian["合同编号"].ToString();
        input["@thdh"] = ht_tiaojian["提货单号"].ToString();

        //设置导出语句
        string sql = "select t.T_THDXDSJ 下达时间,'T'+t.Number 提货单编号,d.Z_SPBH 商品编号,d.Z_SPMC 商品名称,d.Z_GG 规格,t.T_THSL 提货数量,t.ZBDJ 定标价格,t.T_DJHKJE 提货金额,t.T_DJHKJE 冻结货款金额,'买方名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.Y_YSYDDDLYX where DDB.Number=d.Number),'卖方名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),d.Z_HTBH 对应电子购货合同编号,t.F_DQZT 提货单状态, case d.Z_QPZT when '未开始清盘' then '定标' else case d.Z_HTZT when '定标合同到期' then '合同期满'+d.Z_QPZT when '定标合同终止' then '废标'+d.Z_QPZT when '定标执行完成' then '合同完成'+d.Z_QPZT else '异常'   end   end as '合同状态' from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where  t.F_DQZT in ('未生成发货单','已生成发货单') ";

        if (!ht_tiaojian.Contains("用户邮箱"))
        {
            return null;
        }

        sql+= "and d.Y_YSYDDDLYX=@dlyx";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and d.Z_SPMC like '%'+@spmc+'%' ";
        }

        if (ht_tiaojian["合同编号"].ToString() != "")
        {
            sql += " and d.Z_HTBH like '%'+@htbh+'%' ";
        }
        if (ht_tiaojian["提货单号"].ToString() != "")
        {
            sql+= " and 'T'+t.Number like '%'+@thdh+'%' ";
        }

        sql = sql + " order by t.T_THDXDSJ ASC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];
                      
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
                //删除需要隐藏的列 
                //ds_data.Tables[0].Columns.Remove("");                   

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }


    /// <summary>
    /// 货物收发C区货物签收
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqHWQS(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " b.Number 中标定标信息表编号,F_BUYWYYSHCZSJ as 签收时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 提货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 提货金额,a.T_THSL as 无异议签收数量 ,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买家名称,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖家名称 ,a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单号,a.F_FPHM as 发票号码, b.Z_HTBH as 电子合同编号, case b.Z_QPZT when '未开始清盘' then '定标' else case b.Z_HTZT when '定标合同到期' then '合同期满'+b.Z_QPZT when '定标合同终止' then '废标'+b.Z_QPZT when '定标执行完成' then '合同完成'+b.Z_QPZT else '异常'   end   end as '合同状态' "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 'F'+a.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " F_DQZT in ('无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货') ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " F_BUYWYYSHCZSJ ";  //用于排序的字段(必须设置)   


            if (!ht_tiaojian.Contains("买家编号")||!ht_tiaojian.Contains ("卖家编号"))
            {
                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and (b.Y_YSYDDMJJSBH = '" + ht_tiaojian["买家编号"].ToString() + "' or b.T_YSTBDMJJSBH ='" + ht_tiaojian["卖家编号"].ToString() + "') ";

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.Z_SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }
          
            if (ht_tiaojian["发货单号"].ToString() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 'F'+a.Number like '%" + ht_tiaojian["发货单号"].ToString() + "%' ";
            }
            /*----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
             -----------------------------------------------------------------*/
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }           
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出货物收发C区货物签收数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>    
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquHWQS(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件 
        Hashtable input = new Hashtable();
        input["@buyer"] = ht_tiaojian["买家编号"].ToString();
        input["@saler"] = ht_tiaojian["卖家编号"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();        
        input["@fhdh"] = ht_tiaojian["发货单号"].ToString();

        //设置导出语句
        string sql = "select F_BUYWYYSHCZSJ as 签收时间,'F'+a.Number as 发货单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,a.T_THSL as 无异议签收数量 ,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买方名称,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖方名称 ,a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单编号,a.F_FPHM as 发票编号, b.Z_HTBH as 对应电子购货合同编号,case b.Z_QPZT when '未开始清盘' then '定标' else case b.Z_HTZT when '定标合同到期' then '合同期满'+b.Z_QPZT when '定标合同终止' then '废标'+b.Z_QPZT when '定标执行完成' then '合同完成'+b.Z_QPZT else '异常'   end   end as '合同状态' from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where F_DQZT in ('无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货')";

        if (!ht_tiaojian.Contains("卖家编号")||!ht_tiaojian.Contains ("买家编号"))
        {
            return null;
        }

        sql+= " and (b.Y_YSYDDMJJSBH = @buyer or b.T_YSTBDMJJSBH =@saler) ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and b.Z_SPMC like '%'+@spmc+'%' ";
        }

        if (ht_tiaojian["发货单号"].ToString() != "")
        {
            sql += " and 'F'+a.Number like '%'+@fhdh+'%' ";
        }

        sql = sql + " order by F_BUYWYYSHCZSJ DESC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

                       
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
                //删除需要隐藏的列
                //ds_data.Tables[0].Columns.Remove("");
                  

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 货物收发C区货物发出
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqHWFC(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " b.Number 中标定标信息表编号,a.F_WLXXLRSJ as  生成时间,'F'+a.Number as 发货单号,b.Z_SPMC as 商品名称,b.Z_SPBH as 商品编号,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,'T'+a.Number as 对应提货单,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买家名称,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖家名称,a.F_WLGSMC as 物流公司名称,a.F_WLGSLXR as 物流公司联系人,a.F_WLGSDH as 物流联系电话,a.F_WLDH as 物流单编号,a.F_FPHM as 发票编号,(case  a.F_FPSFSHTH when '是' then '随货同行' else '另外邮寄' end ) as 发票发送方式 , F_FPSFSHTH, b.Z_HTBH as 合同编号,a.F_FPYJXXLRSJ as 发票邮寄信息录入时间,a.F_FPYJDWMC as 发票邮寄单位名称,a.F_FPYJDBH as 发票邮寄单编号,case b.Z_QPZT when '未开始清盘' then '定标' else case b.Z_HTZT when '定标合同到期' then '合同期满'+b.Z_QPZT when '定标合同终止' then '废标'+b.Z_QPZT when '定标执行完成' then '合同完成'+b.Z_QPZT else '异常'   end   end as '合同状态' "; //检索字段(必须设置)

            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 'F'+a.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " a.F_FHDSCSJ is not null and a.F_DQZT = '已录入发货信息' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " a.F_WLXXLRSJ ";  //用于排序的字段(必须设置)   


            if (!ht_tiaojian.Contains("买家编号") || !ht_tiaojian.Contains("卖家编号"))
            {
                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += "  and (b.T_YSTBDMJJSBH ='" + ht_tiaojian["卖家编号"].ToString() + "' or b.Y_YSYDDMJJSBH = '" + ht_tiaojian["买家编号"].ToString() + "' ) ";

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.Z_SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }
            if (ht_tiaojian["合同编号"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.Z_HTBH like '%" + ht_tiaojian["合同编号"].ToString() + "%' ";
            }

            if (ht_tiaojian["发货单号"].ToString() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 'F'+a.Number like '%" + ht_tiaojian["发货单号"].ToString() + "%' ";
            }
            /*----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            -------------------------------------------------------------------- */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }           
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出货物收发C区货物发出数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>    
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquHWFC(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件 
        Hashtable input = new Hashtable();
        input["@buyer"] = ht_tiaojian["买家编号"].ToString();
        input["@saler"] = ht_tiaojian["卖家编号"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();
        input["@htbh"] = ht_tiaojian["合同编号"].ToString();
        input["@fhdh"] = ht_tiaojian["发货单号"].ToString();

        //设置导出语句
        string sql = "select a.F_WLXXLRSJ as  发货时间,'F'+a.Number as 发货单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,'T'+a.Number as 对应提货单号,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买方名称,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖方名称,a.F_WLGSMC as 物流公司名称,a.F_WLGSLXR as 物流联系人,a.F_WLGSDH as 物流联系电话,a.F_WLDH as 物流单编号,a.F_FPHM as 发票编号,(case  a.F_FPSFSHTH when '是' then '随货同行' else '另外邮寄' end ) as 发票发送方式 , b.Z_HTBH as 对应电子购货合同编号,a.F_FPYJXXLRSJ as 发票邮寄信息录入时间,a.F_FPYJDWMC as 发票邮寄单位名称,a.F_FPYJDBH as 发票邮寄单编号,case b.Z_QPZT when '未开始清盘' then '定标' else case b.Z_HTZT when '定标合同到期' then '合同期满'+b.Z_QPZT when '定标合同终止' then '废标'+b.Z_QPZT when '定标执行完成' then '合同完成'+b.Z_QPZT else '异常'   end   end as '合同状态' from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where a.F_FHDSCSJ is not null and a.F_DQZT = '已录入发货信息' ";

        if (!ht_tiaojian.Contains("买家编号") || !ht_tiaojian.Contains("卖家编号"))
        {
            return null;
        }

        sql += "  and (b.T_YSTBDMJJSBH =@saler or b.Y_YSYDDMJJSBH = @buyer) ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and b.Z_SPMC like '%'+@spmc+'%' ";
        }
        if (ht_tiaojian["合同编号"].ToString().Trim() != "")
        {
            sql += " and b.Z_HTBH like '%'+@htbh+'%' ";
        }

        if (ht_tiaojian["发货单号"].ToString() != "")
        {
            sql += " and 'F'+a.Number like '%'+@fhdh+'%' ";
        }

        sql = sql + " order by a.F_WLXXLRSJ ASC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

                      
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
               //删除需要隐藏的列 
                ds_data.Tables[0].Columns.Remove("发票邮寄信息录入时间");
                ds_data.Tables[0].Columns.Remove("发票邮寄单位名称");
                ds_data.Tables[0].Columns.Remove("发票邮寄单编号");  


                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 货物收发C区问题与处理
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqWTYCL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " a.Number, (case when ( a.F_DQZT  = '部分收货' or a.F_DQZT  = '已录入补发备注' or a.F_DQZT  = '补发货物无异议收货' ) then a.F_BUYBFSHCZSJ  when ( a.F_DQZT  = '有异议收货' or a.F_DQZT  = '有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then a.F_BUYYYYSHCZSJ  when ( a.F_DQZT  = '请重新发货' or a.F_DQZT  = '撤销' ) then a.F_BUYQZXFHCZSJ  end)  as 问题产生时间,case when (a.F_DQZT='部分收货' or a.F_DQZT='已录入补发备注' or a.F_DQZT='补发货物无异议收货' ) then '部分收货' when ( a.F_DQZT='有异议收货' or a.F_DQZT='有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then '有异议收货' when (a.F_DQZT='请重新发货' or a.F_DQZT='撤销'  ) then '请重新发货' end as 问题类别及详情 , (case when (a.F_DQZT = '补发货物无异议收货' or a.F_DQZT = '有异议收货后无异议收货' or a.F_DQZT = '卖家主动退货' or a.F_DQZT = '撤销' ) then '完成' else '未完成' end ) as 处理状态  ,'F'+a.Number as 发货单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.T_THSL as 提货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 提货金额,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买家名称,(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖家名称 ,a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单号,a.F_FPHM 发票编号,case a.F_FPSFSHTH when '是' then '发票随货同行' else '发票不随货同行'  end as '发票发送方式',b.Z_HTBH 合同编号,b.Number 中标定标表编号, case b.Z_QPZT when '未开始清盘' then '定标' else case b.Z_HTZT when '定标合同到期' then '合同期满'+b.Z_QPZT when '定标合同终止' then '废标'+b.Z_QPZT when '定标执行完成' then '合同完成'+b.Z_QPZT else '异常'   end   end as '合同状态' "; //检索字段(必须设置)

            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number  ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " F_DQZT in ('部分收货','已录入补发备注','补发货物无异议收货','有异议收货','有异议收货后无异议收货','卖家主动退货','请重新发货','撤销') ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " (case when ( a.F_DQZT  = '部分收货' or a.F_DQZT  = '已录入补发备注' or a.F_DQZT  = '补发货物无异议收货' ) then a.F_BUYBFSHCZSJ  when ( a.F_DQZT  = '有异议收货' or a.F_DQZT  = '有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then a.F_BUYYYYSHCZSJ  when ( a.F_DQZT  = '请重新发货' or a.F_DQZT  = '撤销' ) then a.F_BUYQZXFHCZSJ  end) ";  //用于排序的字段(必须设置)   


            if (!ht_tiaojian.Contains("买家编号") || !ht_tiaojian.Contains("卖家编号"))
            {
                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += "  and (b.Y_YSYDDMJJSBH = '" + ht_tiaojian["买家编号"].ToString() + "' or b.T_YSTBDMJJSBH ='" + ht_tiaojian["卖家编号"].ToString() + "') ";

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.Z_SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }           
            /*----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
           --------------------------------------------------------------------*/
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }           
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出货物收发C区问题与处理
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>    
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquWTYCL(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件 
        Hashtable input = new Hashtable();
        input["@buyer"] = ht_tiaojian["买家编号"].ToString();
        input["@saler"] = ht_tiaojian["卖家编号"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();  

        //设置导出语句
        string sql = "select a.Number,";
        sql+="(case when ( a.F_DQZT  = '部分收货' or a.F_DQZT  = '已录入补发备注' or a.F_DQZT  = '补发货物无异议收货' ) then a.F_BUYBFSHCZSJ  when ( a.F_DQZT  = '有异议收货' or a.F_DQZT  = '有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then a.F_BUYYYYSHCZSJ  when ( a.F_DQZT  = '请重新发货' or a.F_DQZT  = '撤销' ) then a.F_BUYQZXFHCZSJ  end)  as 问题产生时间,";
        sql+="case when (a.F_DQZT='部分收货' or a.F_DQZT='已录入补发备注' or a.F_DQZT='补发货物无异议收货' ) then '部分收货' when ( a.F_DQZT='有异议收货' or a.F_DQZT='有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then '有异议收货' when (a.F_DQZT='请重新发货' or a.F_DQZT='撤销'  ) then '请重新发货' end as 问题类别及详情 , ";
        sql+="(case when (a.F_DQZT = '补发货物无异议收货' or a.F_DQZT = '有异议收货后无异议收货' or a.F_DQZT = '卖家主动退货' or a.F_DQZT = '撤销' ) then '完成' else '未完成' end ) as 处理状态  ,";
        sql+="'F'+a.Number as 发货单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.T_THSL as 发货数量,b.Z_ZBJG as 定标价格,a.T_DJHKJE as 发货金额,";
        sql+="(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.Y_YSYDDDLYX) as 买方名称,";
        sql+="(select I_JYFMC from AAA_DLZHXXB where B_DLYX = b.T_YSTBDDLYX) as 卖方名称 ,";
        sql+="a.F_WLGSMC as 物流公司名称,a.F_WLDH as 物流单号,a.F_FPHM 发票编号,";
        sql+="case a.F_FPSFSHTH when '是' then '发票随货同行' else '发票不随货同行'  end as '发票发送方式',";
        sql += "b.Z_HTBH 对应电子购货合同编号,b.Number 中标定标表编号, ";
        sql+="case b.Z_QPZT when '未开始清盘' then '定标' else case b.Z_HTZT when '定标合同到期' then '合同期满'+b.Z_QPZT when '定标合同终止' then '废标'+b.Z_QPZT when '定标执行完成' then '合同完成'+b.Z_QPZT else '异常'   end   end as '合同状态'";
        sql+="from  AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number where F_DQZT in ('部分收货','已录入补发备注','补发货物无异议收货','有异议收货','有异议收货后无异议收货','卖家主动退货','请重新发货','撤销') ";

        if (!ht_tiaojian.Contains("买家编号") || !ht_tiaojian.Contains("卖家编号"))
        {
            return null;
        }

        sql += "  and (b.Y_YSYDDMJJSBH = @buyer or b.T_YSTBDMJJSBH =@saler) ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and b.Z_SPMC like '%'+@spmc+'%' ";
        }        

        sql = sql + " order by (case when ( a.F_DQZT  = '部分收货' or a.F_DQZT  = '已录入补发备注' or a.F_DQZT  = '补发货物无异议收货' ) then a.F_BUYBFSHCZSJ  when ( a.F_DQZT  = '有异议收货' or a.F_DQZT  = '有异议收货后无异议收货' or a.F_DQZT='卖家主动退货' ) then a.F_BUYYYYSHCZSJ  when ( a.F_DQZT  = '请重新发货' or a.F_DQZT  = '撤销' ) then a.F_BUYQZXFHCZSJ  end)  ASC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

                       
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
                //删除需要隐藏的列
                ds_data.Tables[0].Columns.Remove("Number");
                ds_data.Tables[0].Columns.Remove("中标定标表编号");
                ds_data.Tables[0].Columns.Remove("物流公司名称");
                  

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }
    #endregion

    /// <summary>
    /// 经纪人业务C区分页
    /// </summary>
    /// <param name="ds_page">分页参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqJJRYW(DataSet ds_page, Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "交易方交易概况":
                dsreturn = GetCqJYFJYGK(ds_page, ht_tiaojian);
                break;
            case "交易方基本资料":
                dsreturn = GetCqJYFJBZL(ds_page, ht_tiaojian);
                break;
            case "交易方违规记录":
                dsreturn = GetCqJYFWGJL(ds_page, ht_tiaojian);
                break;
            case "交易商品分析":
                dsreturn = GetCqJYSPFX(ds_page, ht_tiaojian);
                break;
            case "银行交易方基本资料":
                dsreturn = GetCqBankJYFJBZL(ds_page, ht_tiaojian);
                break;  
            default:
                break;
        }
        return dsreturn;
    }
    /// <summary>
    /// 经纪人业务C区导出
    /// </summary>
    /// <param name="ht_tiaojian">查询条件</param>    
    /// <returns></returns>
    public DataSet Export_CquJJRYW(Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "交易方交易概况":
                dsreturn = Export_CquJYFJYGK(ht_tiaojian);
                break;
            case "交易方基本资料":
                dsreturn = Export_CquJYFJBZL(ht_tiaojian);
                break;
            case "交易方违规记录":
                dsreturn = Export_CquJYFWGJL(ht_tiaojian);
                break;
            case "交易商品分析":
                dsreturn = Export_CquJYSPFX(ht_tiaojian);
                break; 
            case "银行交易方基本资料":
                dsreturn = Export_CquBankJYFJBZL(ht_tiaojian);
                break;
            default:
                break;
        }
        return dsreturn;
    }

    #region 经纪人业务管理C区获取分页数据及导出数据功能
    /// <summary>
    /// 经纪人业务管理C区交易方交易概况
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqJYFJYGK(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //string tab_ljsy = "(select (case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end) as 关联经纪人,(case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end) as 交易方账号,sum(JE) as 收益金额 from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.Number left join AAA_ZBDBXXB as c on c.Number=b.ZBDBXXBBH where (XZ='来自买方收益' or XZ='来自卖方收益') and SJLX='预' group by (case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end),(case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end))";
            string tab_ljsy = "(select  关联经纪人,交易方账号,sum(收益金额) as 收益金额 from   (( select (case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end) as 关联经纪人,(case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end) as 交易方账号,sum( isnull(JE,0.00)) as 收益金额 from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.Number left join AAA_ZBDBXXB as c on c.Number=b.ZBDBXXBBH where (XZ='来自买方收益' or XZ='来自卖方收益') and SJLX='预' group by (case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end),(case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end))    union all    ( select (case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end) as 关联经纪人,(case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end) as 交易方账号,sum( isnull(JE,0.00)) as 收益金额 from AAA_ZKLSMXB as a  left join AAA_ZBDBXXB as c on c.Number=a.LYDH where (XZ='来自买方收益' or XZ='来自卖方收益') and SJLX='预' group by (case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end),(case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end)) ) as d   group by 关联经纪人,交易方账号)";

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " a.关联经纪人账号,a.交易方账号,a.交易方名称,a.结算账户类型,a.是否当前关联,a.所属区域,a.注册类型,isnull(b.收益金额,0) as 经纪人累计收益,'' as 累计卖出金额,'' as 累计买入金额,'' as 累计竞标次数,'' as 累计中标次数,'' as 已发货待付款金额 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_View_JJRGLJYFinfo as a left join "+tab_ljsy+" as b on a.交易方账号=b.交易方账号 and a.关联经纪人账号=b.关联经纪人 ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.交易方账号 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " isnull(b.收益金额,0) DESC,a.交易方账号 ";  //用于排序的字段(必须设置)   

            if(!ht_tiaojian.Contains("用户邮箱"))
            {
                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and a.关联经纪人账号='" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["交易方名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and a.交易方名称 like '%" + ht_tiaojian["交易方名称"].ToString () + "%'  ";
            }

            if (ht_tiaojian["省份"].ToString() != "请选择省份")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属省='" +ht_tiaojian["省份"].ToString () + "' ";
            }
            if (ht_tiaojian["地市"].ToString() != "请选择城市")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属市='" +ht_tiaojian["地市"].ToString () + "' ";
            }
            if (ht_tiaojian["区县"].ToString() != "请选择区县")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属区县='" +ht_tiaojian["区县"].ToString () + "' ";
            }
            /*-----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                DataSet ds_old = (DataSet)re_ds[1];
                if (ds_old.Tables[0].TableName == "主要数据")
                {
                    anaPageRetreat apr = new anaPageRetreat();
                    ds_res = apr.JYFJYGK(ds_old);
                }
                else
                {
                    ds_res = ds_old.Copy();
                }
            }
            else
            {
                ds_res = null;
            }
            ------------------------------------------------------------------------*/
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            DataSet ds_old = pd.GetPagerDB(ds_page);
            if (ds_old.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }
            else if (ds_old.Tables[0].TableName == "主要数据")
            {//如果返回值中的表0为主要数据，则对其中的部分字段进行二次处理
                anaPageRetreat apr = new anaPageRetreat();
                ds_res = apr.JYFJYGK(ds_old);
            }
            else
            {//如果有表，表0不是主要数据，则不做处理直接返回
                ds_res = ds_old.Copy();
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出经纪人业务管理C区交易方交易概况
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>   
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquJYFJYGK(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件 
        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@jyfmc"] = ht_tiaojian["交易方名称"].ToString();
        input["@sf"] = ht_tiaojian["省份"].ToString();
        input["@ds"] = ht_tiaojian["地市"].ToString();
        input["@qx"] = ht_tiaojian["区县"].ToString();

        //设置导出语句
        string tab_ljsy = "(select (case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end) as 关联经纪人,(case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end) as 交易方账号,sum(JE) as 收益金额 from AAA_ZKLSMXB as a left join AAA_THDYFHDXXB as b on a.LYDH=b.Number left join AAA_ZBDBXXB as c on c.Number=b.ZBDBXXBBH where (XZ='来自买方收益' or XZ='来自卖方收益') and SJLX='预' group by (case XZ when '来自买方收益' then c.Y_YSYDDDLYX when '来自卖方收益' then c.T_YSTBDDLYX end),(case XZ when '来自买方收益' then c.Y_YSYDDGLJJRYX when '来自卖方收益' then c.T_YSTBDGLJJRYX end))";

        string sql = " select a.交易方账号,a.交易方名称,a.是否当前关联,a.所属区域,a.注册类型,'' as 累计卖出金额,'' as 累计买入金额,isnull(b.收益金额,0) as 经纪人累计收益,'' as 累计竞标次数,'' as 累计中标次数,'' as 已发货待付款金额,a.关联经纪人账号,a.结算账户类型 from  AAA_View_JJRGLJYFinfo as a left join " + tab_ljsy + " as b on a.交易方账号=b.交易方账号 and a.关联经纪人账号=b.关联经纪人 where 1=1 ";

        if (!ht_tiaojian.Contains("用户邮箱"))
        {
            return null;
        }

        sql += " and a.关联经纪人账号=@dlyx ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (ht_tiaojian["交易方名称"].ToString().Trim() != "")
        {
            sql += "  and a.交易方名称 like '%'+@jyfmc+'%'  ";
        }

        if (ht_tiaojian["省份"].ToString() != "请选择省份")
        {
            sql += " and  所属省=@sf ";
        }
        if (ht_tiaojian["地市"].ToString() != "请选择城市")
        {
            sql += " and  所属市=@ds ";
        }
        if (ht_tiaojian["区县"].ToString() != "请选择区县")
        {
            sql += " and  所属区县=@qx ";
        }
        sql = sql + " order by isnull(b.收益金额,0) DESC,a.交易方账号 ASC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            DataSet  ds_old = (DataSet)ht_res["return_ds"];

            if (ds_old != null && ds_old.Tables[0].Rows.Count > 0)
            {
                //对其中的空白字段进行二次处理获取数值
                anaPageRetreat apr = new anaPageRetreat();
                ds_data = apr.JYFJYGK(ds_old);

                //删除需要隐藏的列
                ds_data.Tables[0].Columns.Remove("关联经纪人账号");
                ds_data.Tables[0].Columns.Remove("结算账户类型");

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 经纪人业务管理C区交易方基本资料
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqJYFJBZL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集 
           
            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " *,'' as 账户状态 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_View_JJRGLJYFinfo ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 结算账户类型='买家卖家交易账户' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " Number ";  //用于排序的字段(必须设置)   

            if (!ht_tiaojian.Contains("用户邮箱"))
            {
                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and 关联经纪人账号='" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["交易方名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 交易方名称 like '%" + ht_tiaojian["交易方名称"].ToString() + "%'  ";
            }

            if (ht_tiaojian["省份"].ToString() != "请选择省份")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属省='" + ht_tiaojian["省份"].ToString() + "' ";
            }
            if (ht_tiaojian["地市"].ToString() != "请选择城市")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属市='" + ht_tiaojian["地市"].ToString() + "' ";
            }
            if (ht_tiaojian["区县"].ToString() != "请选择区县")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属区县='" + ht_tiaojian["区县"].ToString() + "' ";
            }

            if (ht_tiaojian["复审记录"].ToString() != "请选择")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  复审记录='" + ht_tiaojian["复审记录"].ToString () + "' ";
            }
            if (ht_tiaojian["是否当前关联"].ToString() != "请选择")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  是否当前关联='" + ht_tiaojian["是否当前关联"].ToString() + "' ";
            }           
            /*--------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                DataSet ds_old = (DataSet)re_ds[1];
                if (ds_old.Tables[0].TableName == "主要数据")
                {
                    anaPageRetreat apr = new anaPageRetreat();
                    ds_res = apr.JYFJBZL(ds_old);
                }
                else
                {
                    ds_res = ds_old.Copy();
                }
            }
            else
            {
                ds_res = null;
            }
            --------------------------------------------------------------------*/
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            DataSet ds_old = pd.GetPagerDB(ds_page);
            if (ds_old.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }
            else if (ds_old.Tables[0].TableName == "主要数据")
            {//如果返回值中的表0为主要数据，则对其中的部分字段进行二次处理
                anaPageRetreat apr = new anaPageRetreat();
                ds_res = apr.JYFJBZL(ds_old);
            }
            else
            {//如果有表，表0不是主要数据，则不做处理直接返回
                ds_res = ds_old.Copy();
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出经纪人业务管理C区交易方基本资料
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>   
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquJYFJBZL(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件 
        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@jyfmc"] = ht_tiaojian["交易方名称"].ToString();
        input["@sf"] = ht_tiaojian["省份"].ToString();
        input["@ds"] = ht_tiaojian["地市"].ToString();
        input["@qx"] = ht_tiaojian["区县"].ToString();
        input["@fsjl"] = ht_tiaojian["复审记录"].ToString();
        input["@sfdqgl"] = ht_tiaojian["是否当前关联"].ToString();

        //设置导出语句
        string sql = "select 交易方账号,交易方名称,是否当前关联,所属区域,注册类型,联系人,联系电话,初审记录,复审记录,暂停新业务时间,'' as 账户状态,是否冻结,是否休眠,是否暂停用户新业务 from AAA_View_JJRGLJYFinfo where 结算账户类型='买家卖家交易账户' ";

        if (!ht_tiaojian.Contains("用户邮箱"))
        {
            return null;
        }

        sql += " and 关联经纪人账号=@dlyx ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (ht_tiaojian["交易方名称"].ToString().Trim() != "")
        {
            sql += "  and 交易方名称 like '%'+@jyfmc+'%'  ";
        }

        if (ht_tiaojian["省份"].ToString() != "请选择省份")
        {
            sql += " and  所属省=@sf ";
        }
        if (ht_tiaojian["地市"].ToString() != "请选择城市")
        {
            sql += " and  所属市=@ds ";
        }
        if (ht_tiaojian["区县"].ToString() != "请选择区县")
        {
            sql += " and  所属区县=@qx ";
        }

        if (ht_tiaojian["复审记录"].ToString() != "请选择")
        {
            sql += " and  复审记录=@fsjl ";
        }
        if (ht_tiaojian["是否当前关联"].ToString() != "请选择")
        {
            sql += " and  是否当前关联=@sfdqgl ";
        }           
        sql = sql + " order by Number DESC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            DataSet ds_old = (DataSet)ht_res["return_ds"];
            if (ds_old != null && ds_old.Tables[0].Rows.Count > 0)
            {
                //对其中的空白字段进行二次处理获取数值
                anaPageRetreat apr = new anaPageRetreat();
                ds_data = apr.JYFJBZL(ds_old);

                //删除需要隐藏的列
                ds_data.Tables[0].Columns.Remove("是否冻结");
                ds_data.Tables[0].Columns.Remove("是否休眠");
                ds_data.Tables[0].Columns.Remove("是否暂停用户新业务");

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 经纪人业务管理C区交易方违规记录
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqJYFWGJL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集 

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " a.*,b.* "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_View_JYFWGJL as a left join AAA_View_JJRGLJYFinfo as b on a.交易方账号=b.交易方账号 and a.关联经纪人账号=b.关联经纪人账号 ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 违约时间 ";  //用于排序的字段(必须设置)   

            if (!ht_tiaojian.Contains("用户邮箱"))
            {
                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and a.关联经纪人账号='" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["交易方名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and b.交易方名称 like '%" + ht_tiaojian["交易方名称"].ToString() + "%'  ";
            }

            if (ht_tiaojian["省份"].ToString() != "请选择省份")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属省='" + ht_tiaojian["省份"].ToString() + "' ";
            }
            if (ht_tiaojian["地市"].ToString() != "请选择城市")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属市='" + ht_tiaojian["地市"].ToString() + "' ";
            }
            if (ht_tiaojian["区县"].ToString() != "请选择区县")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属区县='" + ht_tiaojian["区县"].ToString() + "' ";
            }          
            /*----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];                
            }
            else
            {
                ds_res = null;
            }
            ------------------------------------------------------------------ */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }           
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出经纪人业务管理C区交易方违规记录
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>   
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquJYFWGJL(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件 
        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@jyfmc"] = ht_tiaojian["交易方名称"].ToString();
        input["@sf"] = ht_tiaojian["省份"].ToString();
        input["@ds"] = ht_tiaojian["地市"].ToString();
        input["@qx"] = ht_tiaojian["区县"].ToString();       

        //设置导出语句
        string sql = "select a.交易方账号,b.交易方名称,b.是否当前关联,b.所属区域,b.注册类型,b.联系人,b.联系电话,a.违约事项,a.违约赔偿金金额,a.违约时间 from AAA_View_JYFWGJL as a left join AAA_View_JJRGLJYFinfo as b on a.交易方账号=b.交易方账号 and a.关联经纪人账号=b.关联经纪人账号 where 1=1 ";

        if (!ht_tiaojian.Contains("用户邮箱"))
        {
            return null;
        }

        sql += " and a.关联经纪人账号=@dlyx ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (ht_tiaojian["交易方名称"].ToString().Trim() != "")
        {
            sql += "  and b.交易方名称 like '%'+@jyfmc+'%'  ";
        }

        if (ht_tiaojian["省份"].ToString() != "请选择省份")
        {
            sql += " and  所属省=@sf ";
        }
        if (ht_tiaojian["地市"].ToString() != "请选择城市")
        {
            sql += " and  所属市=@ds ";
        }
        if (ht_tiaojian["区县"].ToString() != "请选择区县")
        {
            sql += " and  所属区县=@qx ";
        }

        sql = sql + " order by 违约时间 DESC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
             ds_data = (DataSet)ht_res["return_ds"];

            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            { 
                //删除需要隐藏的列
                //ds_data.Tables[0].Columns.Remove("");
                    
                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 经纪人业务管理C区本账户下交易商品分析
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqJYSPFX(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集 

            string tb_tab = "(select distinct spbh,GLJJRYX from AAA_TBD where ZT<>'撤销' union  select distinct spbh,GLJJRYX from AAA_YDDXXB where ZT<>'撤销' )";//经纪人关联的交易方的交易商品信息
            string tb_sale = "(select b.T_YSTBDGLJJRYX as 关联经纪人账号,b.Z_SPBH,sum(a.T_DJHKJE) as 卖出金额 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null group by b.T_YSTBDGLJJRYX,b.Z_SPBH)";//累计卖出金额
            string tb_buy = "(select b.Y_YSYDDGLJJRYX as 关联经纪人账号,b.Z_SPBH,sum(a.T_DJHKJE) as 买入金额 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null group by b.Y_YSYDDGLJJRYX,b.Z_SPBH)";//累计买入金额

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " tab.GLJJRYX as 经纪人账号,a.SPBH as 商品编号,a.SPMC as 商品名称,a.GG as 商品规格,'0' as 累计中标次数,'0' as 累计定标次数,isnull(b.卖出金额,0.00) as 累计卖出金额,'--' as 最大卖方名称,'--' as 最大卖方区域,isnull(c.买入金额,0.00) as 累计买入金额,'--' as 最大买方名称,'--' as 最大买方区域  "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_PTSPXXB as a left join " + tb_tab + " as tab on tab.SPBH=a.SPBH left join " + tb_sale + " as b on a.SPBH=b.Z_SPBH and b.关联经纪人账号=tab.GLJJRYX left join " + tb_buy + " as c on c.关联经纪人账号=tab.GLJJRYX and c.Z_SPBH=tab.SPBH  and c.关联经纪人账号=tab.GLJJRYX ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.SPBH ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " isnull(c.买入金额,0.00) desc,isnull(b.卖出金额,0.00)  ";  //用于排序的字段(必须设置)   

            if (!ht_tiaojian.Contains("用户邮箱"))
            {
                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and tab.GLJJRYX='" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and a.SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%'  ";
            }            
            /*----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                DataSet ds_old = (DataSet)re_ds[1];
                if (ds_old.Tables[0].TableName == "主要数据")
                {
                    anaPageRetreat apr = new anaPageRetreat();
                    ds_res = apr.JYFJYSPTJ(ds_old);
                }
                else
                {
                    ds_res = ds_old.Copy();
                }
            }
            else
            {
                ds_res = null;
            }
            ------------------------------------------------------------------*/
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            DataSet ds_old = pd.GetPagerDB(ds_page);
            if (ds_old.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }
            else if (ds_old.Tables[0].TableName == "主要数据")
            {//如果返回值中的表0为主要数据，则对其中的部分字段进行二次处理
                anaPageRetreat apr = new anaPageRetreat();
                ds_res = apr.JYFJYSPTJ(ds_old);
            }
            else
            {//如果有表，表0不是主要数据，则不做处理直接返回
                ds_res = ds_old.Copy();
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出经纪人业务管理C区本账户下交易商品分析
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>  
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquJYSPFX(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件 
        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();        

        //设置导出语句
        string tb_tab = "(select distinct spbh,GLJJRYX from AAA_TBD where ZT<>'撤销' union  select distinct spbh,GLJJRYX from AAA_YDDXXB where ZT<>'撤销' )";//经纪人关联的交易方的交易商品信息
        string tb_sale = "(select b.T_YSTBDGLJJRYX as 关联经纪人账号,b.Z_SPBH,sum(a.T_DJHKJE) as 卖出金额 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null group by b.T_YSTBDGLJJRYX,b.Z_SPBH)";//累计卖出金额
        string tb_buy = "(select b.Y_YSYDDGLJJRYX as 关联经纪人账号,b.Z_SPBH,sum(a.T_DJHKJE) as 买入金额 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH=b.Number where a.F_BUYWYYSHCZSJ is not null group by b.Y_YSYDDGLJJRYX,b.Z_SPBH)";//累计买入金额

        string sql = "select a.SPBH as 商品编号, a.SPMC as 商品名称,a.GG as 规格,'0' as 累计中标次数,'0' as 累计定标次数,isnull(b.卖出金额,0.00) as 累计卖出金额,'--' as 最大卖方名称,'--' as 最大卖方区域,isnull(c.买入金额,0.00) as 累计买入金额,'--' as 最大买方名称,'--' as 最大买方区域,tab.GLJJRYX as 经纪人账号 from   AAA_PTSPXXB as a left join " + tb_tab + " as tab on tab.SPBH=a.SPBH left join " + tb_sale + " as b on a.SPBH=b.Z_SPBH and b.关联经纪人账号=tab.GLJJRYX left join " + tb_buy + " as c on c.关联经纪人账号=tab.GLJJRYX and c.Z_SPBH=tab.SPBH  and c.关联经纪人账号=tab.GLJJRYX where 1=1 ";
        if (!ht_tiaojian.Contains("用户邮箱"))
        {
            return null;
        }

        sql += " and tab.GLJJRYX=@dlyx ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += " and a.SPMC like '%'+@spmc+'%' ";
        }

        sql = sql + " order by isnull(c.买入金额,0.00) desc,isnull(b.卖出金额,0.00)  DESC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            DataSet ds_old = (DataSet)ht_res["return_ds"];

            if (ds_old != null && ds_old.Tables[0].Rows.Count > 0)
            {
                //对其中的空白字段进行二次处理获取数值
                anaPageRetreat apr = new anaPageRetreat();
                ds_data = apr.JYFJYSPTJ(ds_old);

                //删除需要隐藏的列  
                ds_data.Tables[0].Columns.Remove("经纪人账号");


                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }


    /// <summary>
    /// 经纪人业务管理C区银行经纪人交易方基本资料
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqBankJYFJBZL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集 

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " tab.*,(case 是否当前关联 when '否' then '--' else isnull(d.YGLSJG,'--') end) as 分支机构,(case 是否当前关联 when '否' then '--' else isnull(c.GLJJRXSYGGH,'--') end) as 员工工号,(case 是否当前关联 when '否' then '--' else isnull(d.YGXM,'--') end) as 员工姓名,'' as 账户状态  "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_View_JJRGLJYFinfo as tab left join AAA_MJMJJYZHYJJRZHGLB_FZB c on c.Number=tab.Number left join AAA_YHRYXXB  d on c.GLJJRDLZH =d.YHDLZH and c.GLJJRXSYGGH =d.YGGH ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " tab.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 结算账户类型='买家卖家交易账户' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " tab.Number ";  //用于排序的字段(必须设置)   

            if (!ht_tiaojian.Contains("用户邮箱"))
            {
                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and 关联经纪人账号='" + ht_tiaojian["用户邮箱"].ToString() + "' ";

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["交易方名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 交易方名称 like '%" + ht_tiaojian["交易方名称"].ToString() + "%'  ";
            }
            if (ht_tiaojian["分支机构"].ToString() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and d.YGLSJG like '%" +ht_tiaojian["分支机构"].ToString ()+ "%' ";
            }
            if (ht_tiaojian["员工姓名"].ToString() != "")
            {
               ds_page.Tables[0].Rows[0]["search_str_where"] += " and d.YGXM like '%" + ht_tiaojian["员工姓名"].ToString() + "%' ";
            }
            if (ht_tiaojian["省份"].ToString() != "请选择省份")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属省='" + ht_tiaojian["省份"].ToString() + "' ";
            }
            if (ht_tiaojian["地市"].ToString() != "请选择城市")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属市='" + ht_tiaojian["地市"].ToString() + "' ";
            }
            if (ht_tiaojian["区县"].ToString() != "请选择区县")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and  所属区县='" + ht_tiaojian["区县"].ToString() + "' ";
            }

            /*----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                DataSet ds_old = (DataSet)re_ds[1];
                if (ds_old.Tables[0].TableName == "主要数据")
                {
                    anaPageRetreat apr = new anaPageRetreat();
                    ds_res = apr.JYFJBZL(ds_old);
                }
                else
                {
                    ds_res = ds_old.Copy();
                }
            }
            else
            {
                ds_res = null;
            }
            -----------------------------------------------------------------*/
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            DataSet ds_old = pd.GetPagerDB(ds_page);
            if (ds_old.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }
            else if (ds_old.Tables[0].TableName == "主要数据")
            {//如果返回值中的表0为主要数据，则对其中的部分字段进行二次处理
                anaPageRetreat apr = new anaPageRetreat();
                ds_res = apr.JYFJBZL(ds_old);
            }
            else
            {//如果有表，表0不是主要数据，则不做处理直接返回
                ds_res = ds_old.Copy();
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出经纪人业务管理C区银行经纪人交易方基本资料
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>  
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CquBankJYFJBZL(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件 
        Hashtable input = new Hashtable();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@jyfmc"] = ht_tiaojian["交易方名称"].ToString();
        input["@fzjg"] = ht_tiaojian["分支机构"].ToString();
        input["@ygxm"] = ht_tiaojian["员工姓名"].ToString();
        input["@sf"] = ht_tiaojian["省份"].ToString();
        input["@ds"] = ht_tiaojian["地市"].ToString();
        input["@qx"] = ht_tiaojian["区县"].ToString();
        //设置导出语句     

        string sql = "select tab.交易方账号,tab.交易方名称,(case 是否当前关联 when '否' then '--' else isnull(d.YGLSJG,'--') end) as 分支机构,(case 是否当前关联 when '否' then '--' else isnull(c.GLJJRXSYGGH,'--') end) as 员工工号,(case 是否当前关联 when '否' then '--' else isnull(d.YGXM,'--') end) as 员工姓名,tab.是否当前关联,tab.所属区域,tab.注册类型,tab.联系人,tab.联系电话,tab.初审记录,tab.复审记录,tab.暂停新业务时间,'' as 账户状态,tab.是否冻结,tab.是否休眠,tab.是否暂停用户新业务 from AAA_View_JJRGLJYFinfo as tab left join AAA_MJMJJYZHYJJRZHGLB_FZB c on c.Number=tab.Number left join AAA_YHRYXXB  d on c.GLJJRDLZH =d.YHDLZH and c.GLJJRXSYGGH =d.YGGH where 结算账户类型='买家卖家交易账户' ";

        if (!ht_tiaojian.Contains("用户邮箱"))
        {
            return null;
        }

        sql+= " and 关联经纪人账号=@dlyx ";

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (ht_tiaojian["交易方名称"].ToString().Trim() != "")
        {
            sql += "  and 交易方名称 like '%'+@jyfmc+'%'  ";
        }
        if (ht_tiaojian["分支机构"].ToString() != "")
        {
            sql += " and d.YGLSJG like '%'+@fzjg+'%' ";
        }
        if (ht_tiaojian["员工姓名"].ToString() != "")
        {
            sql += " and d.YGXM like '%'+@ygxm+'%' ";
        }
        if (ht_tiaojian["省份"].ToString() != "请选择省份")
        {
            sql += " and  所属省=@sf ";
        }
        if (ht_tiaojian["地市"].ToString() != "请选择城市")
        {
            sql += " and  所属市=@ds ";
        }
        if (ht_tiaojian["区县"].ToString() != "请选择区县")
        {
           sql+= " and  所属区县=@qx ";
        }

        sql = sql + " order by tab.Number DESC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            DataSet ds_old = (DataSet)ht_res["return_ds"];

            if (ds_old != null && ds_old.Tables[0].Rows.Count > 0)
            {
                //对其中的空白字段进行二次处理获取数值
                anaPageRetreat apr = new anaPageRetreat();
                ds_data = apr.JYFJBZL(ds_old);

                //删除需要隐藏的列  
                ds_data.Tables[0].Columns.Remove("是否冻结");
                ds_data.Tables[0].Columns.Remove("是否休眠");
                ds_data.Tables[0].Columns.Remove("是否暂停用户新业务");


                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }
    #endregion


    /// <summary>
    /// 经纪人业务C区分页
    /// </summary>
    /// <param name="ds_page">分页参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCqBankSYGL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "收益管理":
                dsreturn = GetCquBankSYGL(ds_page, ht_tiaojian);
                break;
            case "收益详情":
                dsreturn = GetCquBankSYXQ(ds_page, ht_tiaojian);
                break;           
            default:
                break;
        }
        return dsreturn;
    }
    /// <summary>
    /// 银行经纪人资金转账C区收益管理导出
    /// </summary>
    /// <param name="ht_tiaojian">查询条件</param>   
    /// <returns></returns>
    public DataSet Export_CquBankSYGL(Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "收益管理":
                dsreturn = Export_CqBankSYGL(ht_tiaojian);
                break;
            case "收益详情":
                dsreturn = Export_CqBankSYXQ(ht_tiaojian);
                break;
            default:
                break;
        }
        return dsreturn;
    }


    /// <summary>
    /// 获取银行经纪人资金转账C区收益管理分页数据
    /// </summary>
    /// <param name="ds_page">分页参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCquBankSYGL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("经纪人编号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null
                return null;
            }

            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " 银行人员信息表员工隶属机构,中标定标信息表辅助表员工工号,银行人员信息表员工工号,银行人员信息表员工姓名,SUM(金额) 收益累计 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " (select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额, isnull(fzb.YSTBDGLJJRXSYGGH,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSTBDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSTBDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + ht_tiaojian["经纪人编号"].ToString() + "' union all select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额, isnull(fzb.YSYDDGLJJRXSYGGH,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSYDDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSYDDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB'  and zkmx.XZ='来自买方收益' and JSBH='" + ht_tiaojian["经纪人编号"].ToString() + "'  union all  select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额,isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSTBDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSTBDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + ht_tiaojian["经纪人编号"].ToString() + "'  union all  select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额, isnull(fzb.YSYDDGLJJRXSYGGH ,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSYDDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSYDDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益' and JSBH='" + ht_tiaojian["经纪人编号"].ToString() + "') as tab1 ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 中标定标信息表辅助表员工工号 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " SUM(金额) ";  //用于排序的字段(必须设置) 
           

            if (ht_tiaojian["分支机构"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 银行人员信息表员工隶属机构 like '%" + ht_tiaojian["分支机构"].ToString() + "%' ";
            }

            if (ht_tiaojian["员工工号"].ToString ()!="")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 中标定标信息表辅助表员工工号 like '%" + ht_tiaojian["员工工号"].ToString() + "%' ";
            }
            if (ht_tiaojian["员工姓名"].ToString ()!="")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 银行人员信息表员工姓名 like '%" + ht_tiaojian["员工姓名"].ToString() + "%' ";
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " group by 中标定标信息表辅助表员工工号,银行人员信息表员工工号,银行人员信息表员工隶属机构,银行人员信息表员工工号,银行人员信息表员工姓名 ";

            /*-----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。                
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            ----------------------------------------------------------------- */

            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }           
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出银行经纪人资金转账C区收益管理中的数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>   
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CqBankSYGL(Hashtable ht_tiaojian)
    {       

        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("经纪人编号"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }

        Hashtable input = new Hashtable();
        input["@jjrbh"] = ht_tiaojian["经纪人编号"].ToString();
        input["@fzjg"] = ht_tiaojian["分支机构"].ToString();
        input["@yggh"] = ht_tiaojian["员工工号"].ToString();
        input["@ygxm"] = ht_tiaojian["员工姓名"].ToString();

        //设置导出语句
        string sql = "select 银行人员信息表员工隶属机构 as 分支机构,中标定标信息表辅助表员工工号 as 员工工号,银行人员信息表员工姓名 as 员工姓名,SUM(金额) 收益累计 from ( select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额, isnull(fzb.YSTBDGLJJRXSYGGH,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSTBDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSTBDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB'  and zkmx.XZ='来自卖方收益' and JSBH=@jjrbh union all select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额,isnull( fzb.YSYDDGLJJRXSYGGH,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSYDDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSYDDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB'  and zkmx.XZ='来自买方收益' and JSBH=@jjrbh  union all  select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额,isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSTBDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSTBDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB'  and zkmx.XZ='来自卖方收益' and JSBH=@jjrbh  union all  select isnull(yhryxxb.YGLSJG,'--') 银行人员信息表员工隶属机构,isnull(yhryxxb.YGGH,'--') 银行人员信息表员工工号,isnull(yhryxxb.YGXM,'--') 银行人员信息表员工姓名,isnull(zkmx.JE,0.00) 金额, isnull(fzb.YSYDDGLJJRXSYGGH ,'--') 中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH left join AAA_YHRYXXB yhryxxb on yhryxxb.YGGH=fzb.YSYDDGLJJRXSYGGH and yhryxxb.YHDLZH=fzb.YSYDDGLJJRDLYX  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB'  and zkmx.XZ='来自买方收益' and JSBH=@jjrbh) as tab  where 1=1 ";

        if (ht_tiaojian["分支机构"].ToString().Trim() != "")
        {
            sql += " and 银行人员信息表员工隶属机构 like '%'+@fzjg+'%' ";
        }

        if (ht_tiaojian["员工工号"].ToString() != "")
        {
            sql += " and 中标定标信息表辅助表员工工号 like '%'+@yggh+'%' ";
        }
        if (ht_tiaojian["员工姓名"].ToString() != "")
        {
            sql += " and 银行人员信息表员工姓名 like '%'+@ygxm+'%' ";
        }

        sql += " group by 中标定标信息表辅助表员工工号,银行人员信息表员工工号,银行人员信息表员工隶属机构,银行人员信息表员工工号,银行人员信息表员工姓名 ";

        sql = sql + " order by SUM(金额) DESC ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];

                       
            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
                //删除需要隐藏的列
                //ds_data.Tables[0].Columns.Remove("");                

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }


    /// <summary>
    /// 获取银行经纪人资金转账C区收益管理中收益详情分页数据
    /// </summary>
    /// <param name="ds_page">分页参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetCquBankSYXQ(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("经纪人编号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null
                return null;
            }

            DataSet ds_res = new DataSet();//待返回的结果集           

            #region tableName
            string JJRJSBH = ht_tiaojian["经纪人编号"].ToString();
            string YGGH = ht_tiaojian["员工工号"].ToString();
            string tableName = " (select *,'收益累计'=( select isnull(sum(收益金额),0.00) from ( select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH + "' and fzb.YSTBDGLJJRXSYGGH='" + YGGH + "' union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH + "' and fzb.YSYDDGLJJRXSYGGH='" + YGGH + "'  union all   select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益'and JSBH='" + JJRJSBH + "' and fzb.YSTBDGLJJRXSYGGH='" + YGGH + "' union all     select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH + "' and fzb.YSYDDGLJJRXSYGGH='" + YGGH + "' 	) as a  where a.交易方编号=tab.交易方编号) " +

            "from (select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH + "' and fzb.YSTBDGLJJRXSYGGH='" + YGGH + "' union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH + "' and fzb.YSYDDGLJJRXSYGGH='" + YGGH + "'  union all   select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益'and JSBH='" + JJRJSBH + "' and fzb.YSTBDGLJJRXSYGGH='" + YGGH + "' union all select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH + "' and fzb.YSYDDGLJJRXSYGGH='" + YGGH + "') as tab ) tab1"; 
            #endregion

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " * "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = tableName;  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 收益累计 desc, 收益产生时间 ";  //用于排序的字段(必须设置) 
          
            if (ht_tiaojian["交易方名称"].ToString() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 交易方名称 like '%" + ht_tiaojian["交易方名称"].ToString() + "%' ";
            }           
            /*----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。                
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            --------------------------------------------------------------- */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出银行经纪人资金转账C区收益管理中收益详情的数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param> 
    /// <returns>待导出的数据集</returns>
    public DataSet Export_CqBankSYXQ(Hashtable ht_tiaojian)
    {

        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("经纪人编号"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }

        Hashtable input = new Hashtable();
        input["@jjrbh"] = ht_tiaojian["经纪人编号"].ToString();        
        input["@yggh"] = ht_tiaojian["员工工号"].ToString();
        input["@jyfmc"] = ht_tiaojian["交易方名称"].ToString();

        //设置导出语句
        string sql = "select *,'收益累计'=( select isnull(sum(收益金额),0.00) from ( select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH=@jjrbh and fzb.YSTBDGLJJRXSYGGH=@yggh union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自买方收益'and JSBH=@jjrbh and fzb.YSYDDGLJJRXSYGGH=@yggh  union all   select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益'and JSBH=@jjrbh and fzb.YSTBDGLJJRXSYGGH=@yggh union all     select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益'and JSBH=@jjrbh and fzb.YSYDDGLJJRXSYGGH=@yggh) as a  where a.交易方编号=tab.交易方编号) ";

        sql += "from (select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH=@jjrbh and fzb.YSTBDGLJJRXSYGGH=@yggh union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自买方收益'and JSBH=@jjrbh and fzb.YSYDDGLJJRXSYGGH=@yggh  union all   select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益'and JSBH=@jjrbh and fzb.YSTBDGLJJRXSYGGH=@yggh union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益'and JSBH=@jjrbh and fzb.YSYDDGLJJRXSYGGH=@yggh) as tab where 1=1";  

       
        if (ht_tiaojian["交易方名称"].ToString() != "")
        {
            sql += " and 交易方名称 like '%'+@jyfmc+'%' ";
        }

        sql = sql + " order by 收益累计 desc, 收益产生时间 asc ";

        DataSet ds_data = new DataSet();//获取到导出数据集
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", input);
        if ((bool)ht_res["return_float"])
        {
            ds_data = (DataSet)ht_res["return_ds"];


            if (ds_data != null && ds_data.Tables[0].Rows.Count > 0)
            {
                //删除需要隐藏的列
                ds_data.Tables[0].Columns.Remove("Number");
                ds_data.Tables[0].Columns.Remove("交易方编号");
                ds_data.Tables[0].Columns.Remove("中标定标信息表辅助表员工工号");            

                ds_data.Tables[0].TableName = "导出数据";
                dsreturn.Tables.Add(ds_data.Tables[0].Copy());
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "暂无数据！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获得导出数据！";
        }
        return dsreturn;
    }


    /// <summary>
    /// 获取大盘高级搜索弹窗中的分页数据
    /// </summary>
    /// <param name="ds_page">分页参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetDaPanGJSS(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集 

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " * "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " ( select 商品编号+合同期限 as 主键, SortParentID,a.name as 一级分类,  SSFLBH,SortName as 二级分类, 商品编号,合同期限, 商品名称, 型号规格,计价单位,JJPL as 经济批量  from AAA_DaPanPrd_New left join AAA_PTSPXXB on SPBH=商品编号 left join  AAA_tbMenuSPFL on SortID=SSFLBH left join (select SortID as aid,SortName as name  from AAA_tbMenuSPFL ) a on a.aid= SortParentID ) as tab  ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 主键 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 商品编号 ";  //用于排序的字段(必须设置) 


            if (ht_tiaojian["所属分类"].ToString() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and  SSFLBH in (" + ht_tiaojian["所属分类"].ToString() + ") ";
            }

            if (ht_tiaojian["合同期限"].ToString() != "全部")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 合同期限 like '%" + ht_tiaojian["合同期限"].ToString() + "%' ";
            }
            if (ht_tiaojian["商品编号"].ToString() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 商品编号 like '%" + ht_tiaojian["商品编号"].ToString() + "%' ";
            }
            if (ht_tiaojian["商品名称"].ToString() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }
            if (ht_tiaojian["型号规格"].ToString() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 型号规格 like '%" + ht_tiaojian["型号规格"].ToString() + "%' ";
            }                       
            /*-----------------------------------------------------------------
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。                
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            ---------------------------------------------------------------- */
            //直接调用本中心的“分页数据获取”方法
            pagerdemo pd = new pagerdemo();
            ds_res = pd.GetPagerDB(ds_page);
            if (ds_res.Tables.Count <= 0)
            {//如果返回值中没有表，则返回空数据集
                ds_res = null;
            }           
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
}