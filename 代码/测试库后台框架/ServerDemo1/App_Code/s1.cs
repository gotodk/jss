using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;

[WebService(Namespace = "http://ServerDemo1.ipc.com/", Description = "V1.99->这是这个接口的描述")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class s1 : System.Web.Services.WebService
{
    public s1 () {

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

 
    [WebMethod(Description = "这是说明")]
    public string test(string temp) {


        //====如何调用一个远程接口=========================================================================
        //统一的调用方式，第一个参数就是方法的中文业务名称(聚合中心配置)。参数和返回值，目前只允许string|string[]|DataSet
        object[] re = IPC.Call("测试33", new object[] { "参数1", "参数2" });
        //远程方法执行成功（这里的成功，仅仅是程序上的运行成功。只代表程序运行没有出错，不代表业务成功。业务是否成功，需要根据每个方法自己定义的返回值规则来定。按照预定应该是yewuok:xxx或者yewuerr:xxxx）
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            string reFF = (string)(re[1]);
        }
        else  
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            string reFF = re[1].ToString();
        }
        //===============================================================================



        //====如何连接数据库执行语句=========================================================================
        //先初始化，传空参数就是默认的web.config数据库连接配置。 传别的，就是自定义的其他name对应的连接。
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        //实例化一个数据集，需要传入方法，也用于保存执行结果数据集
        DataSet dsGXlist = new DataSet();
        //定义一个sql语句中的参数。即使没有参数也要new一个哈希表传入。不需要定义参数而已。
        Hashtable input = new Hashtable();
        input["@TBNSL"] = "10"; //跟sql语句中对应的参数。
        //开始执行。返回的哈希表键值包括：return_ds、return_float、return_errmsg、return_other
        //分别对应执行结果，是否运行成功，错误调试信息，其他东西(比如受影响的行啥的，很少用到)
        //除执行结果为dataset外，其他都是string
        //绝大多数情况下，调用RunParam_SQL就足够了，尽量使用参数化，不要直接拼接sql语句。另外的几种不常见的情况是：
        //I_DBL.RunProc是直接运行拼接好的语句，不支持sql语句内参数。如非特别必要，不要使用这种方式。有特殊字符隐患。
        //I_DBL.RunParam_SQL另一个重载，用于基于事务回滚运行多个sql语句，这些语句也都可以带参数。
        //I_DBL.RunProc_CMD是运行存储过程。多个不同作用的重载，可以传入参数，传出参数。
        Hashtable return_ht = I_DBL.RunParam_SQL("select top 100 * from AAA_TBD where TBNSL > @TBNSL", "测试表", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
 
        }
        //===============================================================================



        //这句没用，是为了显示出来而已。
        return re[0].ToString() + "|||" + re[1].ToString();
    }


    [WebMethod(Description = "测试第二个方法哦")]
    public string ceshi22222222(int aaa, DataSet dd)
    {
        DateTime dt1 = DateTime.Now;


        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@TBNSL"] = "10"; //跟sql语句中对应的参数。
        Hashtable return_ht = I_DBL.RunParam_SQL("select top 50 * from AAA_TBD where TBNSL > @TBNSL", "测试表", input);

        DateTime dt2 = DateTime.Now;



        return "ddd";
    }

    /// <summary>
    /// 前台分页调用的方法示例
    /// </summary>
    /// <param name="page">前台设定page_index,page_size值组成的字符串数组</param>
    /// <param name="tiaojian">前台查询时需要用的查询条件的值的列表，用于拼接查询条件</param>
    /// <returns>执行结果数据集</returns>
    [WebMethod(Description = "大分页调用测试")]   
    public DataSet pagedemoTest(string[] page, string[] tiaojian)
    {
        DataSet ds_res = new DataSet();//待返回的结果集
        DataSet ds_page = new DataSet(); //大分页参数集,因新框架不能用哈希表作为参数，所以为了方便使用期间，改用dataset作为分页的参数集。

        //返回值类型为dataset时，要求必须带的一个表，表名和字段名是固定的，不能修改。
        //jieguo字段的值也是固定的，msg的值可自行确定。
        DataTable dt_must = new DataTable();
        dt_must.TableName = "REyewu";
        dt_must.Columns.Add("jieguo", typeof(string));
        dt_must.Columns.Add("msg", typeof(string));        
        ds_res.Tables.Add(dt_must);

        //通过调用web方法，获取大分页功能需要用到的参数列表，返回值为dataset类型
        //此处将page中的page_index、page_size传入，直接在初始化时赋值，不需要自行再次赋值了。
        object[] re_init = IPC.Call("定义分页参数", new object[] {page});        
        if (re_init[0].ToString() != "ok")
        { 
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            dt_must.Rows.Add(new object[] { "yewuerr", "分页参数列表获取失败。"});
        }
        else
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            ds_page = (DataSet)re_init[1];

            //设置大分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
           // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " * "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_PTGLJGB ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " DESC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " Number ";  //用于排序的字段(必须设置)
           // ds_page.Tables[0].Rows[0]["method_retreatment"] = "类名|方法名"; //用于二次处理时传入类名和方法名

            //使用string[] tiaojian 传入的参数拼出需要增加的条件
            if (tiaojian.Length > 0)
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] = ds_page.Tables[0].Rows[0]["search_str_where"] + " and Number>'" + tiaojian[0].ToString() + "' ";
            }


            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
                dt_must.Rows.Add(new object[] { "yewuok", "" });                
            }
            else
            {
                //远程方法执行程序出错了。
                dt_must.Rows.Add(new object[] { "yewuerr", "分页数据获取失败!" }); 
            }
        }
        return ds_res;
    }

    [WebMethod(Description = "大分页调用测试2")]
    public DataSet  pageTest(string page_index,string page_size,string number)
    {
        string msg = "";
        DataSet ds = new DataSet();
        string[] page = new string[] { page_index, page_size };
        string[] tiaojian = new string[] { number };
        try
        {
           ds = pagedemoTest(page, tiaojian);           
        }
        catch(Exception ex)
        {
            ds = null;   
        }
        return ds;

    }


    /// <summary>
    /// 测试IDC调用
    /// </summary>
    /// <param name="khbh">客户编号</param>
    /// <returns>只有返回1，才能继续进行业务处理，否则取消业务处理，提示系统忙。0代表发生意外。2代表强制解锁了，也必须取消业务处理。</returns>
      [WebMethod(Description = "用户枷锁控制")]
    public string TestLock(string khbh)
    {
        string reFF = "";
        object[] re = IPC.Call("用户解锁", new object[] { khbh });
        //远程方法执行成功（这里的成功，仅仅是程序上的运行成功。只代表程序运行没有出错，不代表业务成功。业务是否成功，需要根据每个方法自己定义的返回值规则来定。按照预定应该是yewuok:xxx或者yewuerr:xxxx）
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            reFF = (string)(re[1]);
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            reFF = re[1].ToString();
        }
        return reFF;
    }

}