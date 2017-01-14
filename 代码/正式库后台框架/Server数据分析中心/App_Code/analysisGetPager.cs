using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;
using FMPublicClass;

[WebService(Namespace = "http://analysis.ipc.com/", Description = "V1.00->数据分析中心获取分页数据接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class analysisGetPager : System.Web.Services.WebService
{
    public analysisGetPager()
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
    /// 大盘高级搜索弹窗分页
    /// </summary> 
    /// <param name="ds_page">分页参数列表</param>
    /// <param name="tiaojian">前台查询时需要用的查询条件的值的列表，用于拼接查询条件</param>    
    /// <returns>执行结果数据集</returns>
    [WebMethod(MessageName = "大盘高级搜索分页", Description = "获取大盘高级搜索分页数据")]
    public DataSet DaPanGJSS(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);
        anaGetPdata apg = new anaGetPdata();
        DataSet dsreturn = apg.GetDaPanGJSS(ds_page, ht_tiaojian);
        return dsreturn;
    }

    /// <summary>
    /// 资金转账C区中的各个页面通用方法
    /// </summary> 
    /// <param name="ds_page">分页参数列表</param>
    /// <param name="tiaojian">前台查询时需要用的查询条件的值的列表，用于拼接查询条件</param>    
    /// <returns>执行结果数据集</returns>
    [WebMethod(MessageName="资金转账C区", Description = "获取资金转账C区各标签分页数据")]
    public DataSet CquZJZZ(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);
        anaGetPdata apg = new anaGetPdata();
        DataSet dsreturn = apg.GetCqZJZZ(ds_page, ht_tiaojian);
        return dsreturn;       
    }
    [WebMethod(MessageName = "资金转账C区导出", Description = "获取资金转账C区各标签导出数据")]
    public DataSet CMyXls_CquZJZZ(string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);

        anaGetPdata agp = new anaGetPdata();
        DataSet dsreturn = agp.Export_CquZJZZ(ht_tiaojian);        
        return dsreturn;
    }

    [WebMethod(MessageName = "商品买卖C区", Description = "获取商品买卖C区分页数据")]
    public DataSet CquSPMM(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);

        //调用获取数据的方法
        anaGetPdata apg = new anaGetPdata();
        DataSet dsreturn = apg.GetCqSPMM(ds_page, ht_tiaojian);

        return dsreturn;
    }
    [WebMethod(MessageName = "商品买卖C区导出", Description = "获取商品买卖C区的导出数据")]
    public DataSet CMyXls_CquSPMM(string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);

        anaGetPdata agp = new anaGetPdata();
        DataSet dsreturn = agp.Export_CquSPMM(ht_tiaojian);
        return dsreturn;
    }

    [WebMethod(MessageName = "货物收发C区", Description = "获取货物收发C区分页数据")]
    public DataSet CquHWSF(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);

        //调用获取数据的方法
        anaGetPdata apg = new anaGetPdata();
        DataSet dsreturn = apg.GetCqHWSF(ds_page, ht_tiaojian);

        return dsreturn;
    }
    [WebMethod(MessageName = "货物收发C区导出", Description = "获取货物收发C区的导出数据")]
    public DataSet CMyXls_CquHWSF(string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);

        anaGetPdata agp = new anaGetPdata();
        DataSet dsreturn = agp.Export_CquHWSF(ht_tiaojian);
        return dsreturn;
    }

    [WebMethod(MessageName = "经纪人业务C区", Description = "获取经纪人业务C区分页数据")]
    public DataSet CquJJRYW(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);

        //调用获取数据的方法
        anaGetPdata apg = new anaGetPdata();
        DataSet dsreturn = apg.GetCqJJRYW(ds_page, ht_tiaojian);

        return dsreturn;
    }
    [WebMethod(MessageName = "经纪人业务C区导出", Description = "获取经纪人业务C区的导出数据")]
    public DataSet CMyXls_CquJJRYW(string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);

        anaGetPdata agp = new anaGetPdata();
        DataSet dsreturn = agp.Export_CquJJRYW(ht_tiaojian);
        return dsreturn;
    }

    [WebMethod(MessageName = "银行经纪人C区收益管理", Description = "获取银行经纪人资金转账C区收益管理分页数据")]
    public DataSet CquBankSYGL(DataSet ds_page, string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);

        //调用获取数据的方法
        anaGetPdata apg = new anaGetPdata();
        DataSet dsreturn = apg.GetCqBankSYGL(ds_page, ht_tiaojian);

        return dsreturn;
    }
    [WebMethod(MessageName = "银行经纪人C区收益管理导出", Description = "获取银行经纪人资金转账C区的导出数据")]
    public DataSet CMyXls_CquBankSYGL(string[][] tiaojian)
    {
        //将出入的二维数组参数转换成哈希表，方便使用
        Hashtable ht_tiaojian = StringOP.GetHashtableFromstrArry(tiaojian);

        anaGetPdata agp = new anaGetPdata();
        DataSet dsreturn = agp.Export_CquBankSYGL(ht_tiaojian);
        return dsreturn;
    }
}