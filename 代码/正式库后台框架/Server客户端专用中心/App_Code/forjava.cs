using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;
using FMPublicClass;
using System.Text.RegularExpressions;

[WebService(Namespace = "http://capi.ipc.com/", Description = "临时用于java，手机端调用")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]


public class forjava : System.Web.Services.WebService
{

    public forjava()
    {

        //仅用于手机版临时处理

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
    /// 获取用户IP地址,本方法无法放到PulicClass下，需要用到HTTP的东西
    /// </summary>
    /// <returns></returns>
    private string GetIP()
    {
        string result = String.Empty;
        result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (null == result || result == String.Empty)
        { result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }
        if (null == result || result == String.Empty) { result = HttpContext.Current.Request.UserHostAddress; }
        if (null == result || result == String.Empty || !IsIP(result)) { return "0.0.0.0"; } return result;
    }
    private bool IsIP(string ip) { return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"); }




    /// <summary>
    /// 验证账号密码并记录登录IP
    /// </summary>
    /// <param name="user">邮箱</param>
    /// <param name="user">密码</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod]
    public DataSet checkLoginIn(string user, string pass)
    {
        DataSet dsReturn = initReturnDataSet().Clone();//克隆初始化的数据集形式
        dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "", "", "", "", "", "", "" });//首先插入一条空数据行
        ////开始防篡改串检查
        //if (SPstring != StringOP.GetMD5(new object[] { user, pass }) && IsMD5check())
        //{
        //    return null;
        //}
        ////结束检查      

        #region 1.调用方法 "踢人"， 若不允许(不是"ok"的就踢)，返回特定数据集

        object[] ret = IPC.Call("踢人", new object[] { user });
        if (ret[0].ToString() == "ok")
        {
            string key = (string)(ret[1]);
            if (key != "ok")
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "踢下去";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = key;
                return dsReturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        #region 2.调用方法“是否邮箱重复”。如果发现重复，才正确，继续登陆，不重复，代表用户不存在。返回特定数据集。

        object[] reyx = IPC.Call("是否邮箱重复", new object[] { user });
        if (reyx[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            string reFF = (string)(reyx[1]);

            if (reFF != "是")
            {
                DataTable dt = dsReturn.Tables["返回值单条"];
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不存在该用户！";
                return dsReturn;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        //3.获取客户端ip地址
        string ip = GetIP();

        #region 4.再调用方法“登录信息检查”。将得到的数据返回。
        object[] rezc = IPC.Call("登录信息检查", new object[] { user, pass, ip });
        if (rezc[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(rezc[1]);
            return ds;
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

    }


    [WebMethod]
    public string GetPageData2(string methodname, string[] page, string[][] tiaojian)
    {
        return methodname + "★" + page[0] + "," + page[1] + "★" + tiaojian[0][0] + "|" + tiaojian.Length.ToString();
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
    [WebMethod]
    public DataSet GetPageData(string methodname,string page0,string page1,   string  tiaojian)
    {
        //标签名△竞标中★用户邮箱△gccyu@163.com
        string[] page = new string[] { page0, page1 };
        string[] t1 = tiaojian.Split('★');
        int t1_sl = t1.Length;
        string[][] tiaojian0 = new string[t1_sl][];
        for (int i = 0; i < t1_sl; i++)
        {
            tiaojian0[i] = t1[i].Split('△');
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
        object[] re = IPC.Call(methodname, new object[] { ds_page, tiaojian0 });
        if (re[0].ToString() == "ok")
        {
            return (DataSet)re[1];
        }
        else
        {
            return null;
        }
    }
}
