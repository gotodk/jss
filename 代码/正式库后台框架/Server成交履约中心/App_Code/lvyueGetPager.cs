using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;
using FMPublicClass;

[WebService(Namespace = "http://lvyue.ipc.com/", Description = "V1.00->成交履约中心获取分页数据接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class lvyueGetPager : System.Web.Services.WebService
{
    public lvyueGetPager()
    {

        //如果使用设计的组件，请取消注释以下行
        //InitializeComponent(); 
    }


    /// <summary>
    /// 测试该接口是否还活着(每个接口必备)
    /// </summary>
    /// <param name="temp">随便传</param>
    /// <returns>返回ok就是接口正常</returns>
    [WebMethod(Description = "测试该接口是否还活着(每个接口必备)")]
    public string onlinetest(string temp)
    {
        //根据不同的传入值，后续可以检查不同的东西，比如这个接口所连接的数据库，比如进程池，服务器空间等等。。。
        return "ok";
    }

    /// <summary>
    /// 货物收发B区中通用方法
    /// </summary> 
    /// <param name="ds_page">分页参数列表</param>
    /// <param name="tiaojian">前台查询时需要用的查询条件的值的列表，用于拼接查询条件</param>    
    /// <returns>执行结果数据集</returns>
    [WebMethod(MessageName = "货物收发B区", Description = "获取货物收发B区各标签分页数据")]
    public DataSet BquHWSF(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用        
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);
        lvyueGetPdata lgpd = new lvyueGetPdata();
        DataSet dsreturn = lgpd.GetBqHWSF(ds_page, ht_tiaojian);
        return dsreturn;
    }

    [WebMethod(MessageName = "货物收发B区导出", Description = "获取货物收发B区的导出数据")]
    public DataSet CMyXls_BquHWSF(string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);

        lvyueGetPdata lgpd = new lvyueGetPdata(); ;
        DataSet dsreturn = lgpd.Export_BquHWQS(ht_tiaojian);
        return dsreturn;
    }

    /// <summary>
    /// 商品买卖B区定标
    /// </summary> 
    /// <param name="ds_page">分页参数列表</param>
    /// <param name="tiaojian">前台查询时需要用的查询条件的值的列表，用于拼接查询条件</param>    
    /// <returns>执行结果数据集</returns>
    [WebMethod(MessageName = "商品买卖B区定标", Description = "获取商品买卖B区定标签分页数据")]
    public DataSet BquSPMMDB(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用        
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);
        lvyueGetPdata lgpd = new lvyueGetPdata();
        DataSet dsreturn = lgpd.GetBqSPMMDB(ds_page, ht_tiaojian);
        return dsreturn;
    }
   

    [WebMethod(MessageName = "商品买卖B区定标导出", Description = "获取商品买卖B区定标的导出数据")]
    public DataSet CMyXls_BquSPMMDB(string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);

        lvyueGetPdata lgpd = new lvyueGetPdata(); ;
        DataSet dsreturn = lgpd.Export_BquSPMMDB(ht_tiaojian);
        return dsreturn;
    }
    /// <summary>
    /// 商品买卖B区清盘
    /// </summary> 
    /// <param name="ds_page">分页参数列表</param>
    /// <param name="tiaojian">前台查询时需要用的查询条件的值的列表，用于拼接查询条件</param>    
    /// <returns>执行结果数据集</returns>
    [WebMethod(MessageName = "商品买卖B区清盘", Description = "获取商品买卖B区清盘标签分页数据")]
    public DataSet BquSPMMQP(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用        
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);
        lvyueGetPdata lgpd = new lvyueGetPdata();
        DataSet dsreturn = lgpd.GetBqSPMMQP(ds_page, ht_tiaojian);
        return dsreturn;
    }

}