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
/// otherGetPdata 的摘要说明
/// </summary>
public class otherGetPdata
{
	public otherGetPdata()
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
    public DataSet GetBqTZJL(DataSet ds_page, Hashtable ht_tiaojian )
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "通知记录":
                dsreturn = GetBquTZJL(ds_page, ht_tiaojian);
                break;
            case "信用评级":
                dsreturn = GetBquXYPJ(ds_page, ht_tiaojian);
                break;
            default:
                break;
        }
        return dsreturn;     
    }

    /// <summary>
    /// 通知记录B区通知记录
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBquTZJL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " Number, TXDXDLYX, TXSJ,LTRIM(TXNRWB) AS TXNRWB "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_PTTXXXB ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " TXSJ ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and TXDXDLYX='" + ht_tiaojian["用户邮箱"].ToString() + "'   ";

            if (ht_tiaojian["通知内容"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and TXNRWB  like '%" + ht_tiaojian["通知内容"].ToString() + "%' ";
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
    /// 通知记录B区信用评级记录
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBquXYPJ(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集           

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " a.Number as num ,a.JFSX as ys,convert(numeric(18,2), FS) as fs, (select  isnull(sum(convert(numeric(18,2), FS)),0.00) FROM AAA_JYFXYMXB as b where DLYX='" +ht_tiaojian["用户邮箱"].ToString () + "' and b.Number <=a.Number ) as xyjf,a.CreateTime as cjsj "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_JYFXYMXB as a ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " a.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " a.Number ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            ds_page.Tables[0].Rows[0]["search_str_where"] += " and a.DLYX='" + ht_tiaojian["用户邮箱"].ToString() + "'   ";

            if (ht_tiaojian.Contains("开始时间")&&ht_tiaojian.Contains("结束时间"))
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and convert(varchar(10), a.CreateTime ,120)>='" + ht_tiaojian["开始时间"].ToString() + "' and convert(varchar(10), a.CreateTime ,120)<='" + ht_tiaojian["结束时间"].ToString() + "' ";
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
