using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;
using FMPublicClass;

[WebService(Namespace = "http://order.ipc.com/", Description = "V1.00->交易指令中心获取分页数据接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class orderGetPager : System.Web.Services.WebService
{
    public orderGetPager()
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
    /// 商品买卖B区
    /// </summary> 
    /// <param name="ds_page">分页参数列表</param>
    /// <param name="tiaojian">前台查询时需要用的查询条件的值的列表，用于拼接查询条件</param>    
    /// <returns>执行结果数据集</returns>
    [WebMethod(MessageName = "商品买卖B区", Description = "获取商品买卖B区定标签分页数据")]
    public DataSet BquSPMMDB(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用        
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);
        orderGetPdata lgpd = new orderGetPdata();
        DataSet dsreturn = lgpd.GetBqSPMM(ds_page, ht_tiaojian);
        return dsreturn;
    }

    /// <summary>
    /// 商品买卖B区
    /// </summary> 
    /// <param name="ds_page">分页参数列表</param>
    /// <param name="tiaojian">前台查询时需要用的查询条件的值的列表，用于拼接查询条件</param>    
    /// <returns>执行结果数据集</returns>
    [WebMethod(MessageName = "B区选择商品", Description = "获取B区商品买入卖出选择商品页面分页数据")]
    public DataSet BquXZSP(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用        
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);
        orderGetPdata lgpd = new orderGetPdata();
        DataSet dsreturn = lgpd.GetBqXZSP(ds_page, ht_tiaojian);
        return dsreturn;
    }
}