using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;
using FMPublicClass;
/// <summary>
/// lvyueGetPdata 的摘要说明
/// </summary>
public class lvyueGetPdata
{
	public lvyueGetPdata()
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
    /// 通知记录B区个标签分页数据获取
    /// </summary>
    /// <param name="ds_page">分页参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqHWSF(DataSet ds_page, Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "下达提货单":
                dsreturn = GetBquXDTHD(ds_page, ht_tiaojian);
                break;
            case "货物签收":
                dsreturn = GetBquHWQS(ds_page, ht_tiaojian);
                break;
            case "生成发货单":
                dsreturn = GetBquSCFHD(ds_page, ht_tiaojian);
                break;
            case "录入发货信息":
                dsreturn = GetBquLRFHXX(ds_page, ht_tiaojian);
                break;
            case "问题与处理":
                dsreturn = GetBquWTYCL(ds_page, ht_tiaojian);
                break;
            default:
                break;
        }
        return dsreturn;
    }

    /// <summary>
    /// 货物收发B区下达提货单
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBquXDTHD(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " a.Number,a.Z_HTBH 电子购货合同编号,a.Z_SPMC 商品名称,a.Z_SPBH 商品编号,a.Z_GG 商品规格,b.I_JYFMC 卖家名称,'买家名称'=(select I_JYFMC from AAA_DLZHXXB where AAA_DLZHXXB.B_DLYX=a.Y_YSYDDDLYX),isnull(Z_ZBJG,0.00) 定标价格,isnull((convert(int,Z_ZBSL)-convert(int,Z_YTHSL)),0) 可提货数量,isnull(a.T_YSTBDMJSDJJPL,0) 经济批量,isnull(a.Z_PTSDZDJJPL,0) 平台设定最大经济批量,a.YSYDDTJSJ 原始预订单提交时间,a.Y_YSYDDSHQY 原始预订单收货区域,Z_LYBZJJE 履约保证金金额,T_YSTBDDLYX 原始投标单登陆邮箱,Z_HTBH 合同编号,a.Z_HTQX 合同期限 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_ZBDBXXB a left Join AAA_DLZHXXB b on a.T_YSTBDDLYX=b.B_DLYX ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " isnull((convert(int,Z_ZBSL)-convert(int,Z_YTHSL)),0)>0 and Z_HTZT='定标' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " a.YSYDDTJSJ ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }
            ds_page.Tables[0].Rows[0]["search_str_where"] += " and a.Y_YSYDDDLYX='" + ht_tiaojian["用户邮箱"].ToString() + "' ";
            
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and a.Z_SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }
            if (ht_tiaojian["合同编号"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and a.Z_HTBH like '%" + ht_tiaojian["合同编号"].ToString() + "%' ";
            }

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
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 货物收发B区货物签收
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBquHWQS(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " a.Number,('T'+a.Number) as  提货单号,('F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖方名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买方角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.Z_HTZT as 合同状态 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " a.F_DQZT='已录入发货信息' and b.Z_HTZT='定标'  ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " a.F_FHDSCSJ ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("买方编号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }
            ds_page.Tables[0].Rows[0]["search_str_where"] += " and b.Y_YSYDDMJJSBH='" + ht_tiaojian["买方编号"].ToString () + "' ";

            if (ht_tiaojian["发货单号"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and ('F'+a.Number) like '%" + ht_tiaojian["发货单号"].ToString() + "%' ";
            }
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and b.Z_SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

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
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 导出货物收发B区中货物签收的数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>    
    /// <returns>待导出的数据集</returns>
    public DataSet Export_BquHWQS(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //设置导出语句
        string sql = "select b.Z_HTBH as 对应电子购货合同编号,( 'F'+a.Number ) as 发货单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.T_THSL as 提货数量,a.ZBDJ as 定标价格,T_DJHKJE as 发货金额,('T'+a.Number) as  对应提货单号,c.I_JYFMC as 卖方名称,a.Number,a.ZBDBXXBBH as 中标定标单号 from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX where 1=1 and a.F_DQZT='已录入发货信息' and b.Z_HTZT='定标' ";
        
        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("买方编号"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }
        sql += " and b.Y_YSYDDMJJSBH='" + ht_tiaojian["买方编号"].ToString() + "' ";

        if (ht_tiaojian["发货单号"].ToString().Trim() != "")
        {
            sql += "  and ('F'+a.Number) like '%" + ht_tiaojian["发货单号"].ToString() + "%' ";
        }
        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += "  and b.Z_SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
        }

        Hashtable input = new Hashtable();
        input["@mfbh"] = ht_tiaojian["买方编号"].ToString();
        input["@fhdh"] = ht_tiaojian["发货单号"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();


        sql = sql + " order by a.F_FHDSCSJ ASC ";

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
                ds_data.Tables [0].Columns.Remove ("中标定标单号");               
               
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
    /// 货物收发B区生成发货单
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBquSCFHD(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " d.Z_SPMC 商品名称,d.Z_GG 规格,'T'+t.Number 提货单编号,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL  where DL.B_DLYX=d.Y_YSYDDDLYX),d.Y_YSYDDSHQY 买家收货区域,d.Z_ZBJG 定标价格,t.T_THSL 提货数量,DATEDIFF(dd,getdate(),t.T_ZCFHR) 距最迟发货日天数,t.ZBDBXXBBH 中标定标信息表编号,t.F_FPYJXXLRSJ 发票邮寄信息录入时间,t.F_FPHM 发票号码,t.F_FPSFSHTH 发票是否随货同行,t.T_THDXDSJ 提货单下达时间,t.F_SELTYZXFHCZSJ 卖家同意重新发货操作时间  "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " d.Z_HTZT='定标' and t.F_DQZT='未生成发货单' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            switch (ht_tiaojian["排序"].ToString ())
            {
                case "按收货区域排序":
                    ds_page.Tables[0].Rows[0]["search_paixuZD"] = " d.Y_YSYDDSHQY asc, DATEDIFF(dd,getdate(),t.T_ZCFHR) ";  //用于排序的字段(必须设置)
                    break;
                case "按距最迟发货日天数排序":
                    ds_page.Tables[0].Rows[0]["search_paixuZD"] = " DATEDIFF(dd,getdate(),t.T_ZCFHR) asc,t.T_THDXDSJ ";  //用于排序的字段(必须设置)
                    break;
                default:
                    
                    break;
            }          

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("卖方编号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }
            ds_page.Tables[0].Rows[0]["search_str_where"] += " and d.T_YSTBDMJJSBH='" + ht_tiaojian["卖方编号"].ToString () + "' ";

            if (ht_tiaojian["提货单号"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 'T'+t.Number like '%" + ht_tiaojian["提货单号"].ToString() + "%' ";
            }
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and d.Z_SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

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
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 货物收发B区录入发货信息
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBquLRFHXX(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " d.Z_SPMC 商品名称,d.Z_GG 规格,'F'+t.Number 发货单编号,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.Y_YSYDDDLYX where DDB.Number=d.Number),d.Z_ZBJG 定标价格,t.T_THSL 提货数量,t.F_FPSFSHTH 发票是否随货同行,DATEDIFF(dd,getdate(),t.T_ZCFHR) 距最迟发货日天数,t.ZBDBXXBBH 中标定标信息表编号,t.F_DQZT 发货单状态,t.F_FPYJXXLRSJ 发票录入时间,d.Y_YSYDDSHQY 买家收货区域 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 'F'+t.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " d.Z_HTZT='定标' and (t.F_DQZT='已生成发货单' or (t.F_DQZT not in ('未生成发货单','卖家主动退货','撤销') and t.F_FPYJXXLRSJ is null)) ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            switch (ht_tiaojian["排序"].ToString())
            {
                case "按收货区域排序":
                    ds_page.Tables[0].Rows[0]["search_paixuZD"] = " d.Y_YSYDDSHQY asc, DATEDIFF(dd,getdate(),t.T_ZCFHR) ";  //用于排序的字段(必须设置)
                    break;
                case "按距最迟发货日天数排序":
                    ds_page.Tables[0].Rows[0]["search_paixuZD"] = " DATEDIFF(dd,getdate(),t.T_ZCFHR) asc,t.T_THDXDSJ ";  //用于排序的字段(必须设置)
                    break;
                default:                   
                    break;
            }

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("卖方编号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }
            ds_page.Tables[0].Rows[0]["search_str_where"] += " and d.T_YSTBDMJJSBH='" + ht_tiaojian["卖方编号"].ToString() + "' ";

            if (ht_tiaojian["提货单号"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 'F'+t.Number like '%" + ht_tiaojian["提货单号"].ToString() + "%' ";
            }
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and d.Z_SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

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
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 货物收发B区问题与处理
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBquWTYCL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            string sql = "select '类别'='卖家',t.Number 提货单编号,d.Number 中标定标信息表编号,d.Z_SPMC 商品名称,d.Z_GG 规格,d.Z_SPBH 商品编号,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL where DL.J_BUYJSBH=d.Y_YSYDDMJJSBH),'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL where DL.J_SELJSBH=d.T_YSTBDMJJSBH),t.ZBDJ 定标价格,t.T_THSL 发货单数量,d.Z_HTBH 电子购货合同编号,('F'+t.Number) 发货单编号,t.F_DQZT 问题类别,(case when t.F_DQZT in ('部分收货','已录入补发备注') then '部分收货' when t.F_DQZT in ('有异议收货') then '有异议收货' when t.F_DQZT in ('请重新发货') then '请重新发货' end) 问题类别及详情,(case t.F_DQZT when '部分收货' then t.F_BUYBFSHSJQSSL when '已录入补发备注' then t.F_BUYBFSHSJQSSL  else t.T_THSL end) 已签收数量,(case t.F_DQZT when '部分收货' then t.F_BUYBFSHCZSJ when '有异议收货' then t.F_BUYYYYSHCZSJ when '请重新发货' then t.F_BUYQZXFHCZSJ when '已录入补发备注' then t.F_BUYBFSHCZSJ  end) 签收时间,d.T_YSTBDMJJSBH 原始投标单卖家角色编号,d.Y_YSYDDMJJSBH 原始预订单买家角色编号,d.Y_YSYDDDLYX 原始预订单登录邮箱,d.Y_YSYDDJSZHLX 原始预订单结算账户类型,d.T_YSTBDDLYX 原始投标单登录邮箱,d.T_YSTBDJSZHLX 原始投标单结算账户类型,t.F_FPHM 发票编号,case t.F_FPSFSHTH when '是' then '发票随货同行' else '发票单独邮寄' end as '发票发送方式' from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where d.Z_HTZT='定标' and d.Z_QPZT='未开始清盘' and t.F_DQZT in ('部分收货','已录入补发备注','有异议收货','请重新发货') and d.T_YSTBDMJJSBH='" + ht_tiaojian["卖方编号"].ToString() + "'";
            sql += " union all ";
            sql += " select '类别'='买家',t.Number 提货单编号,d.Number 中标定标信息表编号,d.Z_SPMC 商品名称,d.Z_GG 规格,d.Z_SPBH 商品编号,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL where DL.J_BUYJSBH=d.Y_YSYDDMJJSBH),'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL where DL.J_SELJSBH=d.T_YSTBDMJJSBH),t.ZBDJ 定标价格,t.T_THSL 发货单数量,d.Z_HTBH 电子购货合同编号,('F'+t.Number) 发货单编号,t.F_DQZT 问题类别,(case when t.F_DQZT in ('部分收货','已录入补发备注') then '部分收货' when t.F_DQZT in ('有异议收货') then '有异议收货' when t.F_DQZT in ('请重新发货') then '请重新发货' end) 问题类别及详情,(case t.F_DQZT when '部分收货' then t.F_BUYBFSHSJQSSL when '已录入补发备注' then t.F_BUYBFSHSJQSSL  else t.T_THSL end) 已签收数量,(case t.F_DQZT when '部分收货' then t.F_BUYBFSHCZSJ when '有异议收货' then t.F_BUYYYYSHCZSJ when '请重新发货' then t.F_BUYQZXFHCZSJ when '已录入补发备注' then t.F_BUYBFSHCZSJ  end) 签收时间,d.T_YSTBDMJJSBH 原始投标单卖家角色编号,d.Y_YSYDDMJJSBH 原始预订单买家角色编号,d.Y_YSYDDDLYX 原始预订单登录邮箱,d.Y_YSYDDJSZHLX 原始预订单结算账户类型,d.T_YSTBDDLYX 原始投标单登录邮箱,d.T_YSTBDJSZHLX 原始投标单结算账户类型,t.F_FPHM 发票编号,case t.F_FPSFSHTH when '是' then '发票随货同行' else '发票单独邮寄' end as '发票发送方式' from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where d.Z_HTZT='定标' and d.Z_QPZT='未开始清盘' and t.F_DQZT in ('已录入补发备注','有异议收货') and d.Y_YSYDDMJJSBH='" + ht_tiaojian["买方编号"].ToString() + "'  ";

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " * "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " ("+sql+") as tab ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 提货单编号 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 签收时间 ";  //用于排序的字段(必须设置)
                   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("卖方编号")||!ht_tiaojian.Contains("买方编号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }            

            if (ht_tiaojian["发货单号"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 发货单编号 like '%" + ht_tiaojian["发货单号"].ToString() + "%' ";
            }
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

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
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

   
    /// <summary>
    /// 商品买卖B区定标
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqSPMMDB(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " *,case 合同期限 when '即时' then datediff(day,getdate(),(dateadd(day,( select JSHTDBXTHDQX from AAA_PTDTCSSDB ),中标时间))) else (( select MJLYBZJDJQX from AAA_PTDTCSSDB ) - datediff(day,中标时间,getdate()) ) end as 距定标最后时间天数 "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " ((select  T_YSTBDBH as 投标单编号 ,Z_SPBH as 商品编号, Z_SPMC as 商品名称,Z_GG as 规格, T_YSTBDDJTBBZJ as 投标保证金冻结金额,T_YSTBDTBNSL as 投标拟售量,Sum(  isnull(Z_ZBSL,0.00)) as 中标数量,Z_ZBJG as 中标价格,Sum ( isnull(Z_ZBJE,0.00)) as 中标金额,Z_ZBSJ as 中标时间, sum (isnull (Z_LYBZJJE,0.00)) as 应冻结履约保证金金额 ,Z_HTQX as 合同期限,'' as Number,'供货区域'=(select GHQY from AAA_TBD where AAA_TBD.Number=T_YSTBDBH)  from AAA_ZBDBXXB where  Z_HTZT = '中标' and  Z_SFDJLYBZJ='否' and T_YSTBDMJJSBH ='" + ht_tiaojian["卖方编号"].ToString () + "' and T_YSTBDDLYX='" + ht_tiaojian["用户邮箱"].ToString () + "'  and Z_HTQX <>'即时'   group by T_YSTBDBH,Z_SPBH,Z_SPMC,Z_GG,Z_ZBSJ,T_YSTBDTBNSL,T_YSTBDTBNSL,Z_ZBJG,Z_ZBSJ,T_YSTBDDJTBBZJ,Z_HTQX ) union (select  T_YSTBDBH as 投标单编号 ,Z_SPBH as 商品编号, Z_SPMC as 商品名称,Z_GG as 规格, T_YSTBDDJTBBZJ as 投标保证金冻结金额,T_YSTBDTBNSL as 投标拟售量,isnull(Z_ZBSL,0.00) as 中标数量,Z_ZBJG as 中标价格, isnull(Z_ZBJE,0.00) as 中标金额,Z_ZBSJ as 中标时间, isnull (Z_LYBZJJE,0.00) as 应冻结履约保证金金额 ,Z_HTQX as 合同期限,  Number,'供货区域'=(select GHQY from AAA_TBD where AAA_TBD.Number=T_YSTBDBH)  from AAA_ZBDBXXB where  Z_HTZT = '中标' and  Z_SFDJLYBZJ='否' and T_YSTBDMJJSBH ='" + ht_tiaojian["卖方编号"].ToString () + "' and T_YSTBDDLYX='" + ht_tiaojian["用户邮箱"].ToString () + "'  and Z_HTQX ='即时' ) ) as tab ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 投标单编号 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 中标时间 ";  //用于排序的字段(必须设置)


            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱")||!ht_tiaojian.Contains("卖方编号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

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
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 导出商品买卖B区中定标数据
    /// </summary>
    /// <param name="ht_tiaojian">sql中的查询条件</param>    
    /// <returns>待导出的数据集</returns>
    public DataSet Export_BquSPMMDB(Hashtable ht_tiaojian)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //使用ht_tiaojian 传入的参数拼出需要增加的条件
        if (!ht_tiaojian.Contains("卖方编号") || !ht_tiaojian.Contains("用户邮箱"))
        {//如果传入的参数不包括这几项，说明有问题，直接返回null
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "导出失败！";
            return dsreturn;
        }       
        Hashtable input = new Hashtable();
        input["@mfbh"] = ht_tiaojian["卖方编号"].ToString();
        input["@dlyx"] = ht_tiaojian["用户邮箱"].ToString();
        input["@spmc"] = ht_tiaojian["商品名称"].ToString();

        //设置导出语句
        string sql = "select 投标单编号,商品编号,商品名称,规格,合同期限,投标保证金冻结金额,投标拟售量,中标数量,中标价格,中标金额,中标时间,case 合同期限 when '即时' then datediff(day,getdate(),(dateadd(day,( select JSHTDBXTHDQX from AAA_PTDTCSSDB ),中标时间))) else ((select MJLYBZJDJQX from AAA_PTDTCSSDB)-datediff(day,中标时间,getdate()) ) end as 距定标最后时间天数,应冻结履约保证金金额,供货区域 from ((select  T_YSTBDBH as 投标单编号 ,Z_SPBH as 商品编号, Z_SPMC as 商品名称,Z_GG as 规格, T_YSTBDDJTBBZJ as 投标保证金冻结金额,T_YSTBDTBNSL as 投标拟售量,Sum(  isnull(Z_ZBSL,0.00)) as 中标数量,Z_ZBJG as 中标价格,Sum ( isnull(Z_ZBJE,0.00)) as 中标金额,Z_ZBSJ as 中标时间, sum (isnull (Z_LYBZJJE,0.00)) as 应冻结履约保证金金额 ,Z_HTQX as 合同期限,'' as Number,'供货区域'=(select GHQY from AAA_TBD where AAA_TBD.Number=T_YSTBDBH)  from AAA_ZBDBXXB where  Z_HTZT = '中标' and  Z_SFDJLYBZJ='否' and T_YSTBDMJJSBH =@mfbh and T_YSTBDDLYX=@dlyx  and Z_HTQX <>'即时' group by T_YSTBDBH,Z_SPBH,Z_SPMC,Z_GG,Z_ZBSJ,T_YSTBDTBNSL,T_YSTBDTBNSL,Z_ZBJG,Z_ZBSJ, T_YSTBDDJTBBZJ,Z_HTQX) union (select  T_YSTBDBH as 投标单编号 ,Z_SPBH as 商品编号, Z_SPMC as 商品名称,Z_GG as 规格, T_YSTBDDJTBBZJ as 投标保证金冻结金额,T_YSTBDTBNSL as 投标拟售量,isnull(Z_ZBSL,0.00) as 中标数量,Z_ZBJG as 中标价格, isnull(Z_ZBJE,0.00) as 中标金额,Z_ZBSJ as 中标时间, isnull (Z_LYBZJJE,0.00) as 应冻结履约保证金金额 ,Z_HTQX as 合同期限,Number,'供货区域'=(select GHQY from AAA_TBD where AAA_TBD.Number=T_YSTBDBH)  from AAA_ZBDBXXB where  Z_HTZT = '中标' and  Z_SFDJLYBZJ='否' and T_YSTBDMJJSBH =@mfbh and T_YSTBDDLYX=@dlyx and Z_HTQX ='即时' ) ) as tab where 1=1 ";
      
        if (ht_tiaojian["商品名称"].ToString().Trim() != "")
        {
            sql += "  and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
        }

        sql = sql + " order by 中标时间 ASC ";

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
    /// 商品买卖B区清盘
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqSPMMQP(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " * "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " (SELECT  AAA_ZBDBXXB.Number AS 主键 ,Z_QPZT AS 清盘状态,Z_QPKSSJ AS 清盘开始时间,Z_QPJSSJ AS 清盘结束时间, Z_HTBH AS 电子购货合同编号,Z_HTJSRQ AS 合同结束日期,Z_SPMC AS 商品名称,Z_SPBH AS 商品编号,Z_GG AS 规格,AAA_ZBDBXXB.Z_HTQX 合同期限,Z_ZBSL AS 合同数量,Z_ZBJG AS 单价,Z_ZBJE AS 合同金额,Z_LYBZJJE AS 履约保证金金额,  CAST( SUM( T_DJHKJE/ZBDJ) AS numeric(18,2)) AS 争议数量 ,SUM(T_DJHKJE) AS  争议金额,'人工清盘' AS 清盘类型,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '合同期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同完成' ELSE '其他' END AS 清盘原因 ,T_YSTBDDLYX AS 卖方邮箱,Y_YSYDDDLYX AS 买方邮箱,Q_ZMSCFDLYX AS 证明上传方邮箱,Q_SFYQR AS 是否已确认 ,Q_ZMWJLJ AS 证明文件路径, Q_ZFLYZH AS 转付来源账户, Q_ZFMBZH AS 转付目标账户,Q_ZFJE AS 转付金额,Q_CLYJ AS 处理依据  FROM AAA_THDYFHDXXB JOIN AAA_ZBDBXXB ON AAA_ZBDBXXB.Number=AAA_THDYFHDXXB.ZBDBXXBBH WHERE (RTRIM( LTRIM( Q_SFYQR)) !='是' or  Q_SFYQR is null) and (T_YSTBDDLYX='" + ht_tiaojian["用户邮箱"].ToString() + "' OR Y_YSYDDDLYX='" + ht_tiaojian["用户邮箱"].ToString() + "') AND Z_QPZT='清盘中' AND F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','撤销','卖家主动退货') GROUP BY  Z_HTBH,Z_HTJSRQ,Z_SPMC,Z_SPBH,Z_GG,AAA_ZBDBXXB.Z_HTQX,Z_ZBSL,Z_ZBJG,Z_ZBJE,AAA_ZBDBXXB.Z_HTZT,AAA_ZBDBXXB.Number,Z_LYBZJJE,Z_QPZT,T_YSTBDDLYX ,Y_YSYDDDLYX ,Z_QPKSSJ ,Z_QPJSSJ ,Q_ZMSCFDLYX,Q_SFYQR ,Q_ZMWJLJ, Q_ZFLYZH , Q_ZFMBZH ,Q_ZFJE ,Q_CLYJ  ) as tab ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 主键 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 清盘开始时间 ";  //用于排序的字段(必须设置)


            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += "  and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

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
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

}