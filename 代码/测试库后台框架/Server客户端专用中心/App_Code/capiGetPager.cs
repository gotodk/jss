using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;
using FMPublicClass;

[WebService(Namespace = "http://capi.ipc.com/", Description = "V1.00->客户端专用中心获取分页数据接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class capiGetPager : System.Web.Services.WebService
{
    public capiGetPager()
    {

        //如果使用设计的组件，请取消注释以下行
        //InitializeComponent(); 
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
    /// 是否开启防篡改验证
    /// </summary>
    /// <returns></returns>
    private bool IsMD5check()
    {
        return true;
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
    /// 2014-06-16 shiyan
    /// 客户端获取分页数据的统一调用接口
    /// </summary>
    /// <param name="methodname">调用方法业务名</param>
    /// <param name="page">分页参数，包括page_index,page_size</param>
    /// <param name="tiaojian">查询条件字符串数组</param>
    /// <param name="SPstring">md5验证字符串</param>
    /// <returns>查询结果数据集</returns>
    [WebMethod(MessageName = "分页统一接口", Description = "客户端涉及分页功能的统一调用接口")]
    public DataSet GetPageData(string methodname, string[] page, string[][] tiaojian, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { methodname,page,tiaojian}) && IsMD5check())
        {
            return null;
        }

        //设置分页参数
        DataSet ds_page = new DataSet(); //大分页参数集

        //通过调用web方法，获取大分页功能需要用到的参数列表，返回值为dataset类型
        //此处将page中的page_index、page_size传入，直接在初始化时赋值，不需要自行再次赋值了。
        object[] re_init = IPC.Call("定义分页参数", new object[] { page });
        if (re_init[0].ToString() == "ok")
        {
            ds_page = (DataSet)re_init[1];
        }
        else
        {//分页参数初始化失败返回null
            return null;
        }
        //开始调用实际获取分页数据的方法
        object[] re = IPC.Call(methodname, new object[] { ds_page, tiaojian });
        if (re[0].ToString() == "ok")
        {
            return (DataSet)re[1];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 返回资金账户C区查询中的项目、性质下拉框的选项内容
    /// </summary>
    /// <param name="Index">查询标签对应的序号</param>
    /// <param name="type">数据类型：实、预</param>
    /// <returns>返回标签对应的项目、性质数据集</returns>
    [WebMethod(MessageName="C区资金项目性质",Description = "获取资金查询C区各标签对应的项目、性质选项数据集")]
    public DataSet GetZJCXinfo(string index, string type,string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { index,type }) && IsMD5check())
        {
            return null;
        }
        //开始调用实际获取数据的方法
        object[] re = IPC.Call("获取C区资金项目性质", new object[] { index, type });
        if (re[0].ToString() == "ok")
        {
            return (DataSet)re[1];           
        }
        else
        {
           return  null;                     
        }
       
    }
    /// <summary>
    /// 导出到Excel
    /// </summary>   
    [WebMethod(MessageName = "导出统一接口", Description = "客户端导出功能的统一调用接口，获取需要导出的数据集")]
    public byte[] Export(string methodname, string[][] tiaojian, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] {methodname,tiaojian }) && IsMD5check())
        {
            return null;
        }
        
        //开始调用实际获取数据的方法
        object[] re = IPC.Call(methodname, new object[] { tiaojian});
        if (re[0].ToString() == "ok")
        {
            DataSet  dsreturn = (DataSet)re[1];
            if (dsreturn != null)
            {
                byte[] rebyte = Helper.DataSet2Byte(dsreturn);
                return rebyte;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }        
    }

}