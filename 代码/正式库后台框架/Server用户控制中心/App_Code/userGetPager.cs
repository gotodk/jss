using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;
using FMPublicClass;

[WebService(Namespace = "http://user.ipc.com/", Description = "V1.00->用户控制中心获取分页数据接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class userGetPager : System.Web.Services.WebService
{
    public userGetPager()
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
    /// 经纪人业务管理B区
    /// </summary> 
    /// <param name="ds_page">分页参数列表</param>
    /// <param name="tiaojian">前台查询时需要用的查询条件的值的列表，用于拼接查询条件</param>    
    /// <returns>执行结果数据集</returns>
    [WebMethod(MessageName = "经纪人业务管理B区", Description = "获取经纪人业务管理B区分页数据")]
    public DataSet BquJJRYWGL(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用        
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);
        userGetPdata ugpd = new userGetPdata();
        DataSet dsreturn = ugpd.GetBqJJRYWGL(ds_page, ht_tiaojian);
        return dsreturn;
    }

    /// <summary>
    /// 开通交易账户中的部分弹窗内的分页数据
    /// </summary> 
    /// <param name="ds_page">分页参数列表</param>
    /// <param name="tiaojian">前台查询时需要用的查询条件的值的列表，用于拼接查询条件</param>    
    /// <returns>执行结果数据集</returns>
    [WebMethod(MessageName = "开通交易账户弹窗分页", Description = "获取开通交易账户功能中部分弹窗内的分页数据")]
    public DataSet KTJYZHPage(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用        
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);
        userGetPdata ugpd = new userGetPdata();
        DataSet dsreturn = ugpd.GetKTJYZH(ds_page, ht_tiaojian);
        return dsreturn;
    }

    /// <summary>
    /// 账户维护B区相关页面的分页数据
    /// </summary> 
    /// <param name="ds_page">分页参数列表</param>
    /// <param name="tiaojian">前台查询时需要用的查询条件的值的列表，用于拼接查询条件</param>    
    /// <returns>执行结果数据集</returns>
    [WebMethod(MessageName = "账户维护B区", Description = "获取账户维护B区中相关页面的分页数据")]
    public DataSet BquZHWH(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用        
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);
        userGetPdata ugpd = new userGetPdata();
        DataSet dsreturn = ugpd.GetBqZHWH(ds_page, ht_tiaojian);
        return dsreturn;
    }
}