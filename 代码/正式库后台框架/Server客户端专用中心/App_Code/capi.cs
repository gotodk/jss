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

[WebService(Namespace = "http://capi.ipc.com/", Description = "V1.00->客户端专用中心业务接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class capi : System.Web.Services.WebService
{
 
    public capi()
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
        return false;
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
    /// 本中心接口的代码标准
    /// </summary>
    /// <param name="cs1">参数1</param>
    /// <param name="cs2">参数2</param>
    /// <returns>程序出错就返回null，其他都要正常返回值</returns>
    [WebMethod(MessageName = "C例子测试", Description = "本中心接口的代码标准")]
    public DataSet demoxxx(string cs1, object cs2, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { cs1, cs2 }) && IsMD5check())
        {
            return null;
        }


        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


        //====加锁(个别必要的业务才加)
        if (IPC.Call("用户锁定", new object[] { "传入锁标记，通常是客户邮箱。或者“全局锁”" })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

 
        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);
  
        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { "传入锁标记，通常是客户邮箱。或者“全局锁”" });
        

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;

    }

    #region 大盘和辅助

    /// <summary>
    /// 可以提前获取的一些数据或参数
    /// </summary>
    /// <param name="kong">空参数，规则要求，但没用</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>使用时，客户端应转化成byte[]</returns>
    [WebMethod(MessageName = "C初始化数据", Description = "可以提前获取的一些数据或参数")]
    public byte[] GetPublisDsData_YS(string kong, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { kong }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        //正式开始处理
        DataSet PublisDsData = new DataSet();
        object[] re  = null;


        re = IPC.Call("省市区", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            DataSet reFF = (DataSet)(re[1]);
            PublisDsData.Tables.Add(reFF.Tables[0].Copy());
            PublisDsData.Tables.Add(reFF.Tables[1].Copy());
            PublisDsData.Tables.Add(reFF.Tables[2].Copy());
        }
        else
        {
            //发生错误，写入日志
            string reFF = re[1].ToString();
        }
        //=======================================


        re = IPC.Call("分公司对照表", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            DataSet reFF = (DataSet)(re[1]);
            PublisDsData.Tables.Add(reFF.Tables[0].Copy());
        }
        else
        {
            //发生错误，写入日志
            string reFF = re[1].ToString();
        }
        //=======================================



        re = IPC.Call("分公司表", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            DataSet reFF = (DataSet)(re[1]);
            PublisDsData.Tables.Add(reFF.Tables[0].Copy());
        }
        else
        {
            //发生错误，写入日志
            string reFF = re[1].ToString();
        }
        //=======================================

 
        re = IPC.Call("院系表", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            DataSet reFF = (DataSet)(re[1]);
            PublisDsData.Tables.Add(reFF.Tables[0].Copy());
        }
        else
        {
            //发生错误，写入日志
            string reFF = re[1].ToString();
        }
        //=======================================


 
        re = IPC.Call("动态参数表", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            DataSet reFF = (DataSet)(re[1]);
            PublisDsData.Tables.Add(reFF.Tables[0].Copy());
        }
        else
        {
            //发生错误，写入日志
            string reFF = re[1].ToString();
        }
        //===============================================================================


        //返回数据
        return Helper.DataSet2Byte(PublisDsData);
       
    }


    /// <summary>
    /// 获得大盘底部统计分析，直接从压缩文件中读取
    /// </summary>
    /// <param name="kong">空参数，规则要求，但没用</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>使用时，客户端应转化成string[][]</returns>
    [WebMethod(MessageName = "C大盘底部统计", Description = "获得大盘底部统计分析，直接从压缩文件中读取")]
    public string[][] indexTJSJ(string kong, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { kong }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        //正式开始处理
        object[] re = null;
        string[][] reFF = null;

        re = IPC.Call("底部统计", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            reFF = (string[][])(re[1]);
        }
        else
        {
            //发生错误，写入日志
            string err = re[1].ToString();
        }
        return reFF;
    }



    /// <summary>
    /// 获得一个数据，大盘列表
    /// </summary>
    /// <param name="sp">处理参数，参数不同作用不同</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>返回字节流</returns>
    [WebMethod(MessageName = "C获取大盘", Description = "获得一个数据，大盘列表")]
    public byte[] GetIndexDB(string sp, string SPstring)
    {
       // System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
       // stopwatch.Start();
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { sp }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        //正式开始处理
        object[] re = null;
        byte[] reFF = null;

        re = IPC.Call("列表加载", new object[] { sp });
        if (re[0].ToString() == "ok")
        {
            reFF = (byte[])(re[1]);
        }
        else
        {
            //发生错误，写入日志
            string err = re[1].ToString();
        }

       // stopwatch.Stop();
       // TimeSpan hs1 = stopwatch.Elapsed;


       // FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "获取大盘方法--执行时间:"  + hs1.TotalMilliseconds.ToString("#.#####"), null);
        return reFF;
    }


    /// <summary>
    /// 获取临时的自选商品编号表
    /// </summary>
    /// <param name="DLYX">登陆邮箱</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>返回处理结果提示文本</returns>
    [WebMethod(MessageName = "C临时自选商品表", Description = "获取临时的自选商品编号表")]
    public DataSet ZXSPtemp(string DLYX, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { DLYX }) && IsMD5check())
        {
            return null;
        }


        object[] re = null;
        re = IPC.Call("自选商品临时列表", new object[] { DLYX });
        if (re[0].ToString() == "ok")
        {
            return (DataSet)(re[1]);
        }
        else
        {
            //发生错误，写入日志
            string err = re[1].ToString();

            return null;
        }



    }

    /// <summary>
    /// 加入或删除自选商品
    /// </summary>
    /// <param name="DLYX">登陆邮箱</param>
    /// <param name="SPBH">商品编号</param>
    /// <param name="edit">操作类型</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>返回处理结果提示文本</returns>
    [WebMethod(MessageName = "C操作自选商品", Description = "加入或删除自选商品")]
    public string ZXSPedit(string DLYX, string SPBH, string edit, string SPstring)
    {

 
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { DLYX, SPBH, edit }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        if (edit == "加入")
        {
            IPC.Call("自选添加", new object[] { DLYX, SPBH });
            return "成功添加到自选商品！";
        }
        if (edit == "删除")
        {
            IPC.Call("自选删除", new object[] { DLYX, SPBH });
            return "成功从自选商品中删除！";
        }
        return null;

    }



    /// <summary>
    /// 获得成交信息数据集，首页列表显示
    /// </summary>
    /// <param name="kong">空参数，规则要求，但没用</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName="C获得成交信息",Description = "获得成交信息数据集，首页列表显示")]
    public byte[] cjxx(string kong, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { kong }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        //正式开始处理
        object[] re = null;
        byte[] reFF = null;

        re = IPC.Call("成交详情", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            reFF = (byte[])(re[1]);
        }
        else
        {
            //发生错误，写入日志
            string err = re[1].ToString();
        }
        return reFF;
    }

    /// <summary>
    /// 获得所有商品一级分类
    /// </summary>
    /// <param name="kong">空参数，规则要求，但没用</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C获得一级分类", Description = "获得所有商品一级分类")]
    public DataSet dsfl(string kong, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { kong }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        //正式开始处理
        object[] re = null;
        DataSet reFF = null;

        re = IPC.Call("一级分类", new object[] { "AAA_tbMenuSPFL" });
        if (re[0].ToString() == "ok")
        {
            reFF = (DataSet)(re[1]);
        }
        else
        {
            //发生错误，写入日志
            string err = re[1].ToString();
        }
        return reFF;
    }


    /// <summary>
    /// 获得指定一级分类下的二级分类
    /// </summary>
    /// <param name="SortParentID">父级分类ID</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C获得二级分类", Description = "获得指定一级分类下的二级分类")]
    public DataSet dsfl_erji(string SortParentID, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { SortParentID }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        //正式开始处理
        object[] re = null;
        DataSet reFF = null;

        re = IPC.Call("二级分类", new object[] { SortParentID });
        if (re[0].ToString() == "ok")
        {
            reFF = (DataSet)(re[1]);
        }
        else
        {
            //发生错误，写入日志
            string err = re[1].ToString();
        }
        return reFF;
    }



    /// <summary>
    /// 检查右下角提醒
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C检查右下角提醒", Description = "检查右下角提醒")]
    public DataSet CheckFormTrayMsg(string dlyx, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dlyx }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        //正式开始处理
        object[] re = null;
        DataSet reFF = null;

        re = IPC.Call("检查提醒", new object[] { dlyx });
        if (re[0].ToString() == "ok")
        {
            reFF = (DataSet)(re[1]);
        }
        else
        {
            //发生错误，写入日志
            string err = re[1].ToString();
        }
        return reFF;
    }


    /// <summary>
    /// 获得商品详情页面，用于详情页面
    /// </summary>
    /// <param name="spbh">商品编号</param>
    /// <param name="htqx">合同期限</param>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C获得商品详情页面", Description = "获得商品详情页面，用于详情页面")]
    public byte[] SelectSPXQ(string spbh, string htqx,string dlyx, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { spbh, htqx, dlyx }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        //正式开始处理
        object[] re = null;
        DataSet reFF = null;
        byte[] reFF_end = null;

        re = IPC.Call("商品详情首次", new object[] { spbh, htqx, dlyx });
        if (re[0].ToString() == "ok")
        {
            reFF = (DataSet)(re[1]);
            reFF_end = Helper.DataSet2Byte(reFF);
        }
        else
        {
            //发生错误，写入日志
            string err = re[1].ToString();
        }
        return reFF_end;
    }

    /// <summary>
    /// 2014-06-16 shiyan
    /// 获取C区上部冻结、余额、收益的数值
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="type">结算账户类型</param>
    /// <returns>返回获取的统计数值</returns>
    [WebMethod(MessageName="C区统计数据", Description = "返回C区上部关于钱的部分的数据值")]
    public DataSet GetTongJi(string dlyx, string type,string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dlyx,type }) && IsMD5check())
        {
            return null;
        }
        DataSet dsreturn = initReturnDataSet().Clone();
        object[] re = IPC.Call("获取C区统计数据", new object[]{dlyx,type});
        if (re[0].ToString() == "ok")
        {
            dsreturn = (DataSet)re[1];
        }
        else
        {
            //初始化返回值,先塞一行数据            
            dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err",  "数据获取失败！" + re[1].ToString() });  
        }      

        return dsreturn;
    }



    /// <summary>
    /// 2014.07.11 商品分类 wyh 2014.07.11
    /// </summary>
    /// <returns></returns>
    [WebMethod(MessageName = "C商品分类", Description = "获取商品分类列表")]
    public DataSet GetSPFL(string cs, string SPstring)
    {
        DataSet dsReturn = initReturnDataSet().Clone();
        dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { cs }) && IsMD5check())
        {
            return null;
        }

        object[] ret = IPC.Call("商品分类", new object[] { cs });
        if (ret[0].ToString() == "ok")
        {
            dsReturn = (DataSet) ret[1];

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        return dsReturn;
    }
    #endregion

    #region 账户相关

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
    [WebMethod(MessageName = "C登录账户验证", Description = "验证账号密码并记录登录IP")]
    public DataSet checkLoginIn(string user, string pass, string SPstring)
    {
        DataSet dsReturn = initReturnDataSet().Clone();//克隆初始化的数据集形式
        dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "", "", "", "", "", "", "" });//首先插入一条空数据行
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { user,pass }) && IsMD5check())
        {
            return null;
        }
        //结束检查      
       
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
        object[] rezc = IPC.Call("登录信息检查", new object[] { user,pass,ip});
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

    /// <summary>
    /// 用户注册第一步
    /// </summary>
    /// <param name="uid">账号</param>
    /// <param name="uname">用户名</param>
    /// <param name="pwd">密码</param>
    /// <param name="yzm">客户端生成的邮箱验证码</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.06.25
    [WebMethod(MessageName = "C用户注册", Description = "验证账号密码并记录登录IP")]
    public DataSet UserRegister(string uid, string uname, string pwd, string yzm, string SPstring)
    {
        DataSet dsReturn = initReturnDataSet().Clone();
        dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { uid, uname, pwd, yzm }) && IsMD5check())
        {
            return null;
        }
        //结束检查              
        
        #region 1.调用"是否邮箱重复"，检查邮箱，如果重复，不再往下执行了。不允许注册。
        object[] reyx = IPC.Call("是否邮箱重复", new object[] { uid });
        if (reyx[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            string reFF = (string)(reyx[1]);

            if (reFF == "是")
            {
                DataTable dt = dsReturn.Tables["返回值单条"];
                dt.Rows.Clear();
                dt.Rows.Add(new object[] {"FALSE", "已存在该账户",  "0", uid, uname, pwd, yzm  });
                return dsReturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。    
            //下面这句写日志只暂时用于测试
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.debug, "用户注册邮箱重复" + reyx[1].ToString(), null);
            return null;
        }
        #endregion

        #region 2.调用"是否用户名重复"，检查用户名，如果重复，不再往下执行了。不允许注册。
        object[] reyhm = IPC.Call("是否用户名重复", new object[] { uname });       
        if (reyhm[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            string reFF = (string)(reyhm[1]);

            if (reFF == "是")
            {
                DataTable dt = dsReturn.Tables["返回值单条"];
                dt.Rows.Clear();
                dt.Rows.Add(new object[] {  "FALSE", "用户名重复","0", uid, uname, pwd, yzm });
                return dsReturn;
            }
                    
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。    
            //下面这句写日志只暂时用于测试
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.debug, "用户注册用户名重复" + reyhm[1].ToString(), null);
            return null;
        }
        #endregion

        //3.获取客户端ip地址
        string ip = GetIP();

        #region 4.调用获取主键的方法，来得到一个新的主键。

        string key = "";
        object[] reynum = IPC.Call("获取主键", new object[] { "AAA_MJMJJYZHYJJRZHGLB", "" });
        if (reynum[0].ToString() == "ok")
        {            
                key = (string)(reynum[1]);   
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。    
            //下面这句写日志只暂时用于测试
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.debug, "用户注册主键" + reynum[1].ToString(), null);
            return null;
        }
        
        #endregion

        #region 5.调用“发送邮件信息”, 将验证码发送出去。

        object[] remail = IPC.Call("发送邮件信息", new object[] { uid, "中国商品批发交易平台 - 注册账号", yzm, "~/HtmlTemplate/EmailTemplate.htm" });
        ////模拟期间不必须发送
        //if (!(remail[0].ToString() == "ok"))
        //{
        //    return null;
        //}    
        #endregion

        #region 6.调用"用户注册"，进行注册。        
        object[] rezc = IPC.Call("用户注册", new object[] { key,uid,uname,pwd,ip,yzm });
        if (rezc[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(rezc[1]);
            DataTable dt=dsReturn.Tables["返回值单条"];

            if(ds!=null && ds.Tables[0].Rows.Count>0)
            {
                dt.Rows[0]["执行结果"] = ds.Tables[0].Rows[0]["RESULT"];
                dt.Rows[0]["提示文本"] = ds.Tables[0].Rows[0]["REASON"];
                dt.Rows[0]["附件信息1"]=key;
                dt.Rows[0]["附件信息2"]=uid;
                dt.Rows[0]["附件信息3"]=uname;
                dt.Rows[0]["附件信息4"]=pwd;
                dt.Rows[0]["附件信息5"]=yzm;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。    
            //下面这句写日志只暂时用于测试
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.debug, "用户注册过程" + rezc[1].ToString(), null);
            return null;
        }

        #endregion

        return dsReturn;
    }


    /// <summary>
    /// 处理注册时的更换邮箱
    /// </summary>
    /// <param name="number">主键编号</param>
    /// <param name="uid">账号</param>
    /// <param name="uname">用户名</param>
    /// <param name="pwd">密码</param>
    /// <param name="yzm">客户端生成的邮箱验证码</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.06.25
    [WebMethod(MessageName = "C更换邮箱", Description = "处理注册时的更换邮箱")]
    public DataSet UserChangeRegister(string number, string uid, string uname, string pwd, string yzm, string SPstring)
    {
        DataSet dsReturn = initReturnDataSet().Clone();
        dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { number, uid, uname, pwd, yzm }) && IsMD5check())
        {
            return null;
        }
        //结束检查              

        #region 1.调用"是否邮箱除此重复"，检查邮箱，如果重复，不再往下执行了。不允许注册。
        object[] reyx = IPC.Call("是否邮箱除此重复", new object[] { uid,number });
        if (reyx[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            string reFF = (string)(reyx[1]);

            if (reFF == "是")
            {
                DataTable dt = dsReturn.Tables["返回值单条"];
                dt.Rows.Clear();
                dt.Rows.Add(new object[] { "FALSE", "已存在该账户", "0", uid, uname, pwd, yzm });
                return dsReturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

        #region 2.调用"是否用户名除此重复"，检查用户名，如果重复，不再往下执行了。不允许注册。
        object[] reyhm = IPC.Call("是否用户名除此重复", new object[] { uname,number });
        if (reyhm[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            string reFF = (string)(reyhm[1]);

            if (reFF == "是")
            {
                DataTable dt = dsReturn.Tables["返回值单条"];
                dt.Rows.Clear();
                dt.Rows.Add(new object[] { "FALSE", "用户名重复", "0", uid, uname, pwd, yzm });
                return dsReturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

        #region 3.调用“发送邮件信息”, 将验证码发送出去。

        object[] remail = IPC.Call("发送邮件信息", new object[] { uid, "中国商品批发交易平台 - 注册账号", yzm, "~/HtmlTemplate/EmailTemplate.htm" });
        ////模拟期间不必须发送
        //if (!(remail[0].ToString() == "ok"))
        //{
        //    return null;
        //}    
        #endregion

        #region 4.调用"更换邮箱注册"，进行注册。
        object[] rezc = IPC.Call("更换邮箱注册", new object[] { number, uid, uname, pwd,  yzm });
        if (rezc[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(rezc[1]);
            DataTable dt = dsReturn.Tables["返回值单条"];

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                dt.Rows[0]["执行结果"] = ds.Tables[0].Rows[0]["RESULT"];
                dt.Rows[0]["提示文本"] = ds.Tables[0].Rows[0]["REASON"];
                dt.Rows[0]["附件信息1"] = number;
                dt.Rows[0]["附件信息2"] = uid;
                dt.Rows[0]["附件信息3"] = uname;
                dt.Rows[0]["附件信息4"] = pwd;
                dt.Rows[0]["附件信息5"] = yzm;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        return dsReturn;
    }


    /// <summary>
    /// 再次发送验证码（注册，换验证码，不换邮箱）
    /// </summary>
    /// <param name="dlyx">账号</param>
    /// <param name="valNum">新验证码</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.06.25
    [WebMethod(MessageName = "C再次发送验证码", Description = "再次发送验证码（注册，换验证码，不换邮箱）")]
    public DataSet reSendValNum(string dlyx, string valNum, string SPstring)
    {
        DataSet dsReturn = initReturnDataSet().Clone();
        dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dlyx, valNum }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        #region 1.调用“发送邮件信息”, 将验证码发送出去。

        object[] remail = IPC.Call("发送邮件信息", new object[] { dlyx, "中国商品批发交易平台 - 注册账号", valNum, "~/HtmlTemplate/EmailTemplate.htm" });
        ////模拟期间不必须发送
        //if (!(remail[0].ToString() == "ok"))
        //{
        //    return null;
        //}    
        #endregion

        #region 2. 调用"更新注册验证码"，进行更换。

        object[] reyzm = IPC.Call("更新注册验证码", new object[] { dlyx, valNum });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(reyzm[1]);
            DataTable dt = dsReturn.Tables["返回值单条"];

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                dt.Rows[0]["执行结果"] = ds.Tables[0].Rows[0]["RESULT"];
                dt.Rows[0]["提示文本"] = ds.Tables[0].Rows[0]["REASON"];
                dt.Rows[0]["附件信息1"] = dlyx;
                dt.Rows[0]["附件信息2"] = valNum;               
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion
        
        return dsReturn;
    }

    /// <summary>
    /// 激活帐户
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.06.26
    [WebMethod(MessageName = "C账户激活", Description = "激活帐户，注册时激活账户")]
    public DataSet AccountID(string dlyx, string SPstring)
    {
        DataSet dsReturn = initReturnDataSet().Clone();
        dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dlyx }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        #region 1.调用用户基本信息，返回激活的状态，如果已经激活了，即不再往下执行了。
        
        object[] reinfo = IPC.Call("用户基本信息", new object[] { dlyx,  });
        if (reinfo[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(reinfo[1]);
            DataTable dt = dsReturn.Tables["返回值单条"];

            if (ds.Tables[0]!=null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["是否验证邮箱"].ToString().Trim() == "是" && ds.Tables[0].Rows[0]["是否允许登录"].ToString().Trim() == "是")
                {
                    dt.Rows[0]["执行结果"] = "FALSE";
                    dt.Rows[0]["提示文本"] = "该账户已经处于激活状态，请勿重复激活！";
                    dt.Rows[0]["附件信息1"] = dlyx;

                    return dsReturn;
                }
               
            }

        }
        else
        {                 
            return null;
        }
        #endregion

        #region 2.调用"账户激活"，进行激活。
        object[] rejh = IPC.Call("账户激活", new object[] { dlyx,  });
        if (rejh[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(rejh[1]);
            DataTable dt = dsReturn.Tables["返回值单条"];

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                dt.Rows[0]["执行结果"] = ds.Tables[0].Rows[0]["RESULT"];
                dt.Rows[0]["提示文本"] = ds.Tables[0].Rows[0]["REASON"];
                dt.Rows[0]["附件信息1"] = dlyx;
                dt.Rows[0]["附件信息2"] = ds.Tables[0].Rows[0]["B_YHM"];
                dt.Rows[0]["附件信息3"] = ds.Tables[0].Rows[0]["B_DLMM"];               
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        return dsReturn;
    }


    /// <summary>
    /// 忘记密码验证码（验证用户信息、发送验证码）
    /// </summary>
    /// <param name="user">登录邮箱或手机号码</param>
    /// <param name="typeToSet">找回方式</param>
    /// <param name="RandomNumber">验证码</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>Dataset（保存发送信息、验证码等）</returns>
    /// edit by zzf 2014.06.26
    [WebMethod(MessageName = "C忘记密码验证码", Description = "找回密码（验证用户信息、发送验证码）")]
    public DataSet SendValNumber(string user, string typeToSet, string RandomNumber, string SPstring)
    {
        DataSet dsReturn = initReturnDataSet().Clone();
        dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { user, typeToSet, RandomNumber }) && IsMD5check())
        {
            return null;
        }
        //结束检查       

        //1.获取客户端ip地址
        string ip = GetIP();

        #region 2.调用获取主键的方法，来得到一个新的主键。

        string key = "";
        object[] reynum = IPC.Call("获取主键", new object[] { "AAA_WJMMYZMB", "" });
        if (reynum[0].ToString() == "ok")
        {
            key = (string)(reynum[1]);
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        #region 3.如果方式为邮箱调用“发送邮件信息”, 将验证码发送出去。

        if (typeToSet == "email")
        {
            object[] remail = IPC.Call("发送邮件信息", new object[] { user, "中国商品批发交易平台 - 找回密码", RandomNumber, "~/HtmlTemplate/EmailTemplate_Repwd.htm" });     
        }
               
        #endregion    
            
        #region 4. 调用"忘记密码验证码"，返回结果。

        object[] reyzm = IPC.Call("忘记密码验证码", new object[] { user ,typeToSet,key, RandomNumber,ip });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(reyzm[1]);
            DataTable dt = dsReturn.Tables["返回值单条"];

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                dt.Rows[0]["执行结果"] = ds.Tables[0].Rows[0]["RESULT"];
                //dt.Rows[0]["提示文本"] = ds.Tables[0].Rows[0]["REASON"];
                dt.Rows[0]["附件信息1"] =user ;
                dt.Rows[0]["附件信息2"] = RandomNumber;
                dt.Rows[0]["附件信息3"] = typeToSet;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion
             
        return dsReturn;
    }

    /// <summary>
    /// 忘记密码后输入验证码进行修改密码
    /// </summary>
    /// <param name="uid">用户登录Email</param>
    /// <param name="phoneoremail">找回密码的email或者手机号</param>
    /// <param name="pwd">新密码</param>
    /// <param name="valNum">验证码</param>
    /// <param name="typeToSet">找回密码方式</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C忘记密码修改密码", Description = "忘记密码后输入验证码进行修改密码")]
    public DataSet ChangePassword(string uid, string phoneoremail, string pwd, string valNum, string typeToSet, string SPstring)
    {    
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { uid, phoneoremail, pwd, valNum, typeToSet }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        #region 调用"忘记密码修改"，返回结果。

        object[] rexg = IPC.Call("忘记密码修改", new object[] {uid ,phoneoremail,pwd,valNum,typeToSet});
        if (rexg[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(rexg[1]);
            return ds;

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion
               
    }
    
    /// <summary>
    /// 用户账户休眠后自己激活账户
    /// </summary>
    /// <param name="dt">账户相关信息</param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C休眠激活", Description = "用户账户休眠后自己激活账户")]
    public DataSet AccountWakeUp(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        
        string dlyx = dt.Rows[0]["dlyx"].ToString().Trim();
        string jszhlx = dt.Rows[0]["jszhlx"].ToString().Trim();             

        #region 1.调用“是否在服务时间”。不在服务时间内，不允许激活。
        DataSet dsparsa = null;
        object[] re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow htPTVal = dsparsa.Tables[0].Rows[0];

        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停处理！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停处理！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停处理！";
            return dsreturn;
        }

        #endregion

        #region 2调用“用户基本信息”来判断是否休眠，开通交易账户状态，存管账户状态，可用余额

        object[] reinfo = IPC.Call("用户基本信息", new object[] { dlyx});
        if(reinfo[0].ToString()=="ok")
        {
            //获取字段
            DataSet reds = (DataSet)(reinfo[1]);
            string sfxm = reds.Tables[0].Rows[0]["是否休眠"].ToString().Trim();
            string zhye = reds.Tables[0].Rows[0]["账户当前可用余额"].ToString().Trim();
            string zhlx = reds.Tables[0].Rows[0]["结算账户类型"].ToString().Trim();
            string jjrsh = reds.Tables[0].Rows[0]["是否已被经纪人审核通过"].ToString().Trim();
            string fgssh = reds.Tables[0].Rows[0]["是否已被分公司审核通过"].ToString().Trim();
            string cgzt = reds.Tables[0].Rows[0]["第三方存管状态"].ToString().Trim();

            //相关判断
            //是否休眠
            if (sfxm == "否")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "cf";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "检测到帐户已在其他设备上激活，不再扣减激活费用！";
                return dsreturn;
            }
            //是否开通交易账户
            bool flag = false;
            if (zhlx == "")
            {
                flag = true;
            }
            else if (zhlx == "经纪人交易账户" && fgssh!="是")
            {
                flag = true;
            }
            else if (zhlx == "买家卖家交易账户" && !(jjrsh=="是" && fgssh=="是"))
            {
                flag = true;
            }
            if (flag)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。 ";
                return dsreturn; 
            }

            //第三方存管状态
            if (cgzt != "开通")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
               // dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前的存管帐户状态为“"+cgzt+"”，不能进行交易操作。 ";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！ ";
                return dsreturn;  
            }

            //当前了用余额
            double YE = 0.00;
            YE = double.Parse((string)(zhye));
            //查询用户余额
            if (100 > YE)
            {
                double ce = 100 - YE;
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的账户可用资金余额为" + YE.ToString("0.00") + "元，差额" + ce.ToString("0.00") + "元，休眠账户无法激活，请您增加账户资金！";
                return dsreturn;
            }
        }
        else
        {
            return null;
        }

        #endregion
                            
        #region 3.调用获取主键的方法，来得到一个新的主键。

        string key = "";
        object[] reynum = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });
        if (reynum[0].ToString() == "ok")
        {
            key = (string)(reynum[1]);
        }
        else
        {          
            return null;
        }

        #endregion

        //====加锁(个别必要的业务才加)
        if (IPC.Call("用户锁定", new object[] { "传入锁标记，通常是客户邮箱。或者“全局锁”" })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);
        
        #region  4.调用“休眠激活”，进行激活。
        DataSet dsr = null;
        object[] rejh = IPC.Call("休眠激活", new object[] { key,dlyx,jszhlx });
        if (rejh[0].ToString() == "ok")
        {
            dsr = (DataSet)(rejh[1]);
        }
        else
        {
            return null;
        }

        #endregion
        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { "传入锁标记，通常是客户邮箱。或者“全局锁”" });
        
        //====返回处理结果(一定要在解锁之后返回)        
        return dsr;
    }


    /// <summary>
    /// 检查当前账户的所属经纪人的状态情况，以便给用户友好提示，同时返回最新的用户基本资料到客户端
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C当前经纪人状态", Description = "检查当前账户的所属经纪人的状态情况，以便给用户友好提示，同时返回最新的用户基本资料到客户端")]
    public DataSet checkJJRzt(string dlyx, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dlyx }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //审核情况
        string str_jjr = "否";
        string str_fgs = "否";
        string str_jyfmc = "",jjrdlyx="";
        

        #region 调用“用户及关联信息”，获取用户信息和关联信息数据表，以及审核情况

        object[] reinfo = IPC.Call("用户及关联信息", new object[] { dlyx });
        if (reinfo[0].ToString() == "ok")
        {
            DataSet ms = (DataSet)(reinfo[1]);

            dsreturn.Tables.Add(ms.Tables["用户信息"].Copy());
            dsreturn.Tables.Add(ms.Tables["关联信息"].Copy());

            str_jjr = ms.Tables["用户信息"].Rows[0]["是否已被经纪人审核通过"].ToString().Trim();
            str_fgs = ms.Tables["用户信息"].Rows[0]["是否已被分公司审核通过"].ToString().Trim();
            str_jyfmc = ms.Tables["用户信息"].Rows[0]["交易方名称"].ToString().Trim();

            jjrdlyx = ms.Tables["关联信息"].Rows[0]["关联经纪人登陆账号"].ToString().Trim();
            
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = str_jjr;
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = str_fgs;
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"] = str_jyfmc;
        }
        else
        {
            return null;
        }

        #endregion

        bool sfxm=false,sfdj=false; 

        #region 调用“是否休眠”，判断关联的经纪人账户是否休眠
        object[] rexm = IPC.Call("是否休眠", new object[] { jjrdlyx });
        if (rexm[0].ToString() == "ok")
        {
            string ms = (string)(rexm[1]);
            if (ms == "是")
            {
                sfxm = true;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息5"] = "账户休眠";
            }
        }
        else
        {
            return null;
        }

        #endregion

        #region 调用“冻结信息”，判断关联的经纪人账户是否冻结

        object[] redj = IPC.Call("冻结信息", new object[] { jjrdlyx });
        if (redj[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(redj[1]);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["是否冻结"].ToString().Trim() == "是" || ds.Tables[0].Rows[0]["冻结功能项"].ToString().Trim()!="")
                {
                    sfdj = true;
                  
                }
            }
        }

        #endregion

        if (sfxm && sfdj)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的经纪人账户已被冻结，若您需要开展新的业务，请重新选择经纪人！\n\r您的经纪人账户已休眠，若您需要开展新的业务，请重新选择经纪人！";
            return dsreturn;
 
        }
        else if (sfdj)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的经纪人账户已被冻结，若您需要开展新的业务，请重新选择经纪人！";
            return dsreturn;
        }
        else if (sfxm)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的经纪人账户已休眠，若您需要开展新的业务，请重新选择经纪人！";

            return dsreturn;
        }

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "一切正常";
        return dsreturn;
    }


    /// <summary>
    /// 用户修改密码
    /// </summary>
    /// <param name="dt">相关资料</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    /// edit by zzf 2014.7.9
    [WebMethod(MessageName = "C用户修改密码", Description = "用户修改密码")]
    public DataSet PsdChange(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查
              
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        string dlyx = dt.Rows[0]["dlyx"].ToString().Trim();
        string yma = dt.Rows[0]["yma"].ToString().Trim();
        string xma = dt.Rows[0]["xma"].ToString().Trim();

        #region 1.调用“是否休眠”，如果账户休眠，不再继续向下进行

        object[] rexm = IPC.Call("是否休眠", new object[] { dlyx });
        if (rexm[0].ToString() == "ok")
        {            
            string ms = (string)(rexm[1]);
            if (ms == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";

                return dsreturn;
            }
        }
        else
        {                    
            return null;
        }

        #endregion
        
        #region 2.调用“用户基本信息含标识”，验证旧密码是否正确，不正确也不允许修改密码。
        object[] rema = IPC.Call("用户基本信息含标识", new object[] { dlyx,"ALL" });
        if (rema[0].ToString() == "ok")
        {
            DataSet ms = (DataSet)(rema[1]);
            if (ms.Tables[0]!=null && ms.Tables[0].Rows.Count>0)
            {
                string ymm = ms.Tables[0].Rows[0]["登录密码"].ToString().Trim();
                if (yma != ymm)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "原密码输入错误，请重新录入！";

                    return dsreturn; 
                }
                
            }
        }
        else
        {
            return null;
        }

        #endregion

        #region 3.调用“登录密码修改”，开始修改密码。

        object[] rexg = IPC.Call("登录密码修改", new object[] { dlyx,xma });
        if (rexg[0].ToString() == "ok")
        {
            DataSet dsr = (DataSet)rexg[1];
            return dsr;
        }
        else
        {
            return null;
        }
        #endregion

        
    }

    /// <summary>
    /// 用户修改证券资金密码
    /// </summary>
    /// <param name="dt">相关资料</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    /// edit by zzf 2014.7.9
    [WebMethod(MessageName = "C修改证券资金密码", Description = "用户修改证券资金密码")]
    public DataSet RunZQZJMMChange(DataTable dt, string SPstring)
    {       
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        string dlyx = dt.Rows[0]["登录邮箱"].ToString().Trim();
        string yzjma = dt.Rows[0]["原证券资金密码"].ToString().Trim();
        string xzjma = dt.Rows[0]["新证券资金密码"].ToString().Trim();

        #region 1.调用“是否休眠”，如果账户休眠，不再继续向下进行

        object[] rexm = IPC.Call("是否休眠", new object[] { dlyx });
        if (rexm[0].ToString() == "ok")
        {
            string ms = (string)(rexm[1]);
            if (ms == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";

                return dsreturn;
            }
        }
        else
        {
            return null;
        }

        #endregion

        #region 2.调用“用户基本信息含标识”，验证旧资金密码是否正确，不正确也不允许修改密码。
        object[] rema = IPC.Call("用户基本信息含标识", new object[] { dlyx, "ALL" });
        if (rema[0].ToString() == "ok")
        {
            DataSet ms = (DataSet)(rema[1]);
            if (ms.Tables[0] != null && ms.Tables[0].Rows.Count > 0)
            {
                string ymm = ms.Tables[0].Rows[0]["结算账户密码"].ToString().Trim();
                if (yzjma != ymm)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "原交易资金密码输入错误，请重新录入！";

                    return dsreturn;
                }

            }
        }
        else
        {
            return null;
        }

        #endregion

        #region 3.调用“资金密码修改”，开始修改密码。

        object[] rexg = IPC.Call("资金密码修改", new object[] { dlyx, xzjma });
        if (rexg[0].ToString() == "ok")
        {
            DataSet dsr = (DataSet)rexg[1];
            return dsr;
        }
        else
        {
            return null;
        }
        #endregion
        
    }
    /// <summary>
    /// 用于用户重新选择其他经纪人
    /// </summary>
    /// <param name="dt">参数列表</param>
    /// <param name="SPstring">防篡改字符串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C选择经纪人", Description = "用于用户重新选择其他经纪人")]
    public DataSet ChooseJJR(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        string dlyx = dt.Rows[0]["买卖家登录邮箱"].ToString().Trim();

        #region 1.调用“是否休眠”，如果账户休眠，不再继续向下进行
        
        object[] rexm = IPC.Call("是否休眠", new object[] { dlyx });
        if (rexm[0].ToString() == "ok")
        {
            string ms = (string)(rexm[1]);
            if (ms == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";

                return dsreturn;
            }
        }
        else
        {
            return null;
        }

        #endregion


        #region 2.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { dlyx });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

        #region 3.调用系统控制中心的"获取主键"方法来生成“关联经纪人信息表AAA_MJMJJYZHYJJRZHGLB”的主键
        string key = "";
        object[] reynum = IPC.Call("获取主键", new object[] { "AAA_MJMJJYZHYJJRZHGLB", "" });
        if (reynum[0].ToString() == "ok")
        {
            key = (string)(reynum[1]);
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        #region 4.调用“选择经纪人”，执行操作
        object[] rexz = IPC.Call("选择经纪人", new object[] { dt,key });
        if (rexz[0].ToString() == "ok")
        {
            DataSet dsr = (DataSet)(rexz[1]);
            return dsr;
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

    }
    /// <summary>
    /// 返回一些基础验证的状态值
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="SPstring">防篡改字符串</param>
    /// <returns>一个DataSet，包含一个名为“状态值单条”的数据表，含一系列用于基本验证的状态值</returns>
    /// Create by zzf 2014.7.30
    [WebMethod(MessageName = "C基础验证", Description = "返回一些基础验证的状态值")]
    public DataSet BasicValidation(string dlyx, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dlyx }) && IsMD5check())
        {
            return null;
        }

        DataTable dt = new DataTable();
        dt.Columns.Add("是否在服务时间内", typeof(string));
        dt.Columns.Add("时间类型", typeof(string));
        dt.Columns.Add("是否休眠", typeof(string));
        dt.Columns.Add("是否开通交易账户", typeof(string));
        dt.Columns.Add("交易账户审核状态", typeof(string));
        dt.Columns.Add("第三方存管状态", typeof(string));
        dt.Columns.Add("是否冻结", typeof(string));
        dt.Columns.Add("冻结功能项", typeof(string));
        dt.Columns.Add("当前默认经纪人是否休眠", typeof(string));
        dt.Columns.Add("当前默认经纪人是否被冻结", typeof(string));

        dt.TableName = "状态值单条";
        dt.Rows.Add(new string[] { "err", "初始化" });

        DataSet dsreturn = new DataSet();
        //dsreturn.Tables.Add(dt);      

        #region 1.是否在服务时间
        DataSet dsparsa = null;
        object[] re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow htPTVal = dsparsa.Tables[0].Rows[0];

        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            dt.Rows[0]["是否在服务时间内"] = "否";
            dt.Rows[0]["时间类型"] = "假期";
            
        }
        else if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            dt.Rows[0]["是否在服务时间内"] = "否";
            dt.Rows[0]["时间类型"] = "非工作时间";            
        }
        else if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            dt.Rows[0]["是否在服务时间内"] = "否";
            dt.Rows[0]["时间类型"] = "非交易时间";

        }
        else
        {
            dt.Rows[0]["是否在服务时间内"] = "是";
            dt.Rows[0]["时间类型"] = "交易时间"; 
        }
        #endregion

        #region 2调用“用户基本信息”来判断是否休眠，开通交易账户状态，存管账户状态，可用余额

        object[] reinfo = IPC.Call("用户基本信息", new object[] { dlyx });
        if (reinfo[0].ToString() == "ok")
        {
            //获取字段
            DataSet reds = (DataSet)(reinfo[1]);
            dt.Rows[0]["是否休眠"] = reds.Tables[0].Rows[0]["是否休眠"].ToString().Trim();
            //string zhye = reds.Tables[0].Rows[0]["账户当前可用余额"].ToString().Trim();
           
            dt.Rows[0]["第三方存管状态"] = reds.Tables[0].Rows[0]["第三方存管状态"].ToString().Trim();
            dt.Rows[0]["是否冻结"] = reds.Tables[0].Rows[0]["是否冻结"].ToString().Trim();
            dt.Rows[0]["冻结功能项"] = reds.Tables[0].Rows[0]["冻结功能项"].ToString().Trim();

            string zhlx = reds.Tables[0].Rows[0]["结算账户类型"].ToString().Trim();
            //string jjrsh = reds.Tables[0].Rows[0]["是否已被经纪人审核通过"].ToString().Trim();
            //string fgssh = reds.Tables[0].Rows[0]["是否已被分公司审核通过"].ToString().Trim();

            object[] rejjr = IPC.Call("关联经纪人信息", new object[] { dlyx });
            if (rejjr[0].ToString() == "ok")
            {
                DataSet jjrds = (DataSet)(rejjr[1]);
                bool nonnul = true;
                if (jjrds == null || jjrds.Tables.Count == 0 || jjrds.Tables[0].Rows.Count == 0)
                {
                    nonnul = false;
                }

                string jjrshzt =nonnul==true ? jjrds.Tables[0].Rows[0]["经纪人审核状态"].ToString().Trim():"";
                string fgsshzt =nonnul==true ? jjrds.Tables[0].Rows[0]["分公司审核状态"].ToString().Trim():"";

                dt.Rows[0]["当前默认经纪人是否被冻结"] =nonnul==true ? jjrds.Tables[0].Rows[0]["当前默认经纪人是否被冻结"].ToString().Trim():"";
                dt.Rows[0]["当前默认经纪人是否休眠"] =nonnul==true ? jjrds.Tables[0].Rows[0]["当前默认经纪人是否休眠"].ToString().Trim():"";

                //是否开通交易账户            
                if (zhlx == "")
                {
                    dt.Rows[0]["是否开通交易账户"] = "否";
                    dt.Rows[0]["交易账户审核状态"] = "未申请";

                }
                
                else if (zhlx == "经纪人交易账户")
                {

                    if (fgsshzt == "审核通过")
                    {
                        dt.Rows[0]["交易账户审核状态"] = "审核通过";
                        dt.Rows[0]["是否开通交易账户"] = "是";
                    }
                    else if (fgsshzt == "驳回")
                    {
                        dt.Rows[0]["交易账户审核状态"] = "未通过";
                        dt.Rows[0]["是否开通交易账户"] = "否";
                    }
                    else
                    {
                        dt.Rows[0]["交易账户审核状态"] = "审核中";
                        dt.Rows[0]["是否开通交易账户"] = "否";

                    }
                }
                else if (zhlx == "买家卖家交易账户")
                {
                    if (jjrshzt == "审核通过" && fgsshzt == "审核通过")
                    {
                        dt.Rows[0]["交易账户审核状态"] = "审核通过";
                        dt.Rows[0]["是否开通交易账户"] = "是";
                    }
                    else if (jjrshzt == "驳回" || fgsshzt == "驳回")
                    {
                        dt.Rows[0]["交易账户审核状态"] = "未通过";
                        dt.Rows[0]["是否开通交易账户"] = "否";
                    }
                    else
                    {
                        dt.Rows[0]["交易账户审核状态"] = "审核中";
                        dt.Rows[0]["是否开通交易账户"] = "否";
                    }

                }

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

        #endregion

        dsreturn.Tables.Add(dt);
        return dsreturn;
 
    }

    /// <summary>
    /// 更换经纪人时获取的经纪人交易账户数据信息，选择经纪人时应用
    /// </summary>
    /// <param name="dt">参数数据表</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.7.11
    [WebMethod(MessageName = "C获取更换经纪人信息", Description = "更换经纪人时获取的经纪人交易账户数据信息，选择经纪人时应用")]
    public DataSet GetSiftJJRData(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        string dlyx = dt.Rows[0]["买卖家登录邮箱"].ToString().Trim();

        #region 1.是否在服务时间
        DataSet dsparsa = null;
        object[] re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow htPTVal = dsparsa.Tables[0].Rows[0];

        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }
        #endregion

        #region 2调用“用户基本信息”来判断是否休眠，是否冻结

        object[] rejbxx = IPC.Call("用户基本信息", new object[] { dlyx });
        if (rejbxx[0].ToString() == "ok")
        {
            //获取字段
            DataSet reds = (DataSet)(rejbxx[1]);

            string sfxm = reds.Tables[0].Rows[0]["是否休眠"].ToString().Trim();
            //string zhye = reds.Tables[0].Rows[0]["账户当前可用余额"].ToString().Trim();

            //string cgzt = reds.Tables[0].Rows[0]["第三方存管状态"].ToString().Trim();
            string sfdj = reds.Tables[0].Rows[0]["是否冻结"].ToString().Trim();
            string djgnx = reds.Tables[0].Rows[0]["冻结功能项"].ToString().Trim();

            string zhlx = reds.Tables[0].Rows[0]["结算账户类型"].ToString().Trim();
            //string jjrsh = reds.Tables[0].Rows[0]["是否已被经纪人审核通过"].ToString().Trim();
            //string fgssh = reds.Tables[0].Rows[0]["是否已被分公司审核通过"].ToString().Trim();

            if (sfxm == "是" && (sfdj == "是" || djgnx!=""))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！\n\r您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + djgnx;
                return dsreturn;
 
            }
            else if (sfxm == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                return dsreturn;
            }
            else if (sfdj == "是" || djgnx!="")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + djgnx;
                return dsreturn;
 
            }         

        }
        else
        {
            return null;
        }

        #endregion

        #region 3.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { dlyx });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion
        
        
        #region 4.调用“获取更换经纪人信息”，得到结果数据集
         DataSet dsr = null;
        object[] reinfo = IPC.Call("获取更换经纪人信息", new object[] { dt });
        if (reinfo[0].ToString() == "ok")
        {
            dsr = (DataSet)(reinfo[1]);
          
        }
        return dsr;
        #endregion
        
    }

    
    /// <summary>
    /// 得到买卖、卖家数据的基本信息
    /// </summary>
    /// <param name="dt">相关资料</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C获取买卖家信息", Description = "得到买卖、卖家数据的基本信息。用于获得买家或卖家的资料。")]
    public DataSet GetMMJData(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        #region
        object[] rexg = IPC.Call("获取买卖家信息", new object[] { dt });
        if (rexg[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(rexg[1]);
            return ds;
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

    }


    /// <summary>
    /// 得到买卖、卖家数据的基本信息
    /// </summary>
    /// <param name="dt">相关资料</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C买卖家账户信息", Description = "得到买卖、卖家数据的基本信息")]
    public DataSet GetMMJZHData(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        string dlyx = dt.Rows[0]["DLYX"].ToString().Trim();
        string jsbh = dt.Rows[0]["JSBH"].ToString().Trim();

        DataSet dsreturn = null;

        #region 调用“买卖家账户基本信息”，获取基本信息
        object[] rexg = IPC.Call("买卖家账户基本信息", new object[] { jsbh });
        if (rexg[0].ToString() == "ok")
        {
            dsreturn = (DataSet)(rexg[1]);
            
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

        #region 2.调用“用户及关联信息”来得到用户信息和关联信息表

        object[] rex = IPC.Call("用户及关联信息", new object[] { dlyx });
        if (rex[0].ToString() == "ok")
        {
            DataSet dx = (DataSet)(rex[1]);
            if (dx != null && dx.Tables.Count > 0)
            {
                dsreturn.Tables.Add(dx.Tables["用户信息"].Copy());
                dsreturn.Tables[3].TableName = "用户信息";
                dsreturn.Tables.Add(dx.Tables["关联信息"].Copy());
                dsreturn.Tables[4].TableName = "关联信息";
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion
                
        return dsreturn;
    }


    /// <summary>
    /// 得到经纪人数据的基本信息
    /// </summary>
    /// <param name="dt">相关资料</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C经纪人账户信息", Description = "得到经纪人账户基本信息")]
    public DataSet GetJJRZHData(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        string dlyx = dt.Rows[0]["DLYX"].ToString().Trim();
        string jsbh = dt.Rows[0]["JSBH"].ToString().Trim();

        DataSet dsreturn = null;

        #region 调用“经纪人账户基本信息”，获取基本信息
        object[] rexg = IPC.Call("经纪人账户基本信息", new object[] { jsbh });
        if (rexg[0].ToString() == "ok")
        {
            dsreturn = (DataSet)(rexg[1]);

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

        #region 2.调用“用户及关联信息”来得到用户信息和关联信息表

        object[] rex = IPC.Call("用户及关联信息", new object[] { dlyx });
        if (rex[0].ToString() == "ok")
        {
            DataSet dx = (DataSet)(rex[1]);
            if (dx != null && dx.Tables.Count > 0)
            {
                dsreturn.Tables.Add(dx.Tables["用户信息"].Copy());
                dsreturn.Tables[2].TableName = "用户信息";
                dsreturn.Tables.Add(dx.Tables["关联信息"].Copy());
                dsreturn.Tables[3].TableName = "关联信息";
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        return dsreturn;
    }


    /// <summary>
    /// 得到当前交易账户的当前状态
    /// </summary>
    /// <param name="dt">相关资料</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C当前交易账户状态", Description = "得到当前交易账户的开通状态")]
    public DataSet GetJYZHDQZT(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        string DLYX = dt.Rows[0]["登录邮箱"].ToString();//登录邮箱

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        #region 调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { DLYX });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(reyzm[1]);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || ds.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okJudge";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "未开通交易账户";
                }
                else if (ds.Tables[0].Rows[0]["审核状态"].ToString().Trim() == "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okJudge";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已开通交易账户，能进行交易操作。";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "已开通交易账户";
                }
            }

        }
        else
        {
            return null;
        }
        #endregion

        

        return dsreturn;
    }

    /// <summary>
    /// 暂停、恢复新用户审核的初始审核状态，用于控制“暂停新交易方审核”页签，防止重复提交信息
    /// </summary>
    /// <param name="dt">参数数据表</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C暂停恢复新用户审核初始状态", Description = "暂停、恢复新用户审核的审核状态")]
    public DataSet ZTHFXYHSHCSZT(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        //开始执行     

        string DLYX = dt.Rows[0]["登录邮箱"].ToString();//登录邮箱

        #region 1.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { DLYX });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(reyzm[1]);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okOther";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "未开通交易账户";
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okOther";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已开通交易账户，能进行交易操作。";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "已开通交易账户";
                }
            }

        }
        else
        {
            return null;
        }
        #endregion
        #region 2.调用“经纪人暂停审核”，判断初始状态

        object[] rezt = IPC.Call("经纪人暂停审核", new object[] { DLYX });
        if (rezt[0].ToString() == "ok")
        {
            string  rs = (string )(rezt[1]);
            if (rs == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okOther";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "交易账户当前为暂停状态";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okOther";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "交易账户当前为恢复状态"; 
            }
        }
        else
        {
            return null;
        }

        #endregion

        return dsreturn;
    }

    /// <summary>
    /// 暂停、恢复新用户审核  仅用于经纪人交易账户的时候
    /// </summary>
    /// <param name="dt">相关资料</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    /// edit by zzf 2014.7.10
    [WebMethod(MessageName = "C暂停恢复新用户审核", Description = "暂停、恢复新用户审核  仅用于经纪人交易账户的时候")]
    public DataSet ZTHFXYHSH(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查
        string CZLX = dt.Rows[0]["操作类型"].ToString();//操作类型  暂停、恢复
        string jjrjsbh = dt.Rows[0]["经纪人角色编号"].ToString();

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        bool sfxm = false, sfdj = false;
        string item = "";
        #region 1.调用“是否休眠”，如果账户休眠，不再继续向下进行

        object[] rexm = IPC.Call("是否休眠", new object[] { jjrjsbh });
        if (rexm[0].ToString() == "ok")
        {
            string ms = (string)(rexm[1]);
            if (ms == "是")
            {
                sfxm = true;
                
            }
        }
        else
        {
            return null;
        }

        #endregion

        #region 2.调用“是否冻结”，如果冻结，则不再进行下面的操作

        object[] redjx = IPC.Call("冻结信息", new object[] { jjrjsbh });
        if (redjx[0].ToString() == "ok")
        {
            DataSet dm = (DataSet)(redjx[1]);
            if (dm != null && dm.Tables[0] != null && dm.Tables[0].Rows.Count > 0)
            {
                item = dm.Tables[0].Rows[0]["冻结功能项"].ToString().Trim();
                if (item.Contains("经纪人暂停代理新业务"))
                {
                    sfdj = true;
                }
            }

        }
        else
        {
            return null;
        }
               
        #endregion

        if (sfxm && sfdj)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！\n\r您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能："+item;
            return dsreturn;

        }
        else if (sfdj)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能："+item;
            return dsreturn;
        }
        else if (sfxm)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";

            return dsreturn;
        }
         
        #region 3.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { jjrjsbh });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion             

        #region 4.调用“经纪人暂停审核”，判断状态
        object[] resh = IPC.Call("经纪人暂停审核", new object[] { jjrjsbh });
        if (resh[0].ToString() == "ok")
        {
            string ms = (string)(resh[1]);
            if (ms == "是" &&  CZLX == "暂停")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前已经是暂停新用户审核审核状态，请不要再执行暂停操作！";
                return dsreturn;
            }
            else if (ms == "否" && CZLX == "恢复")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前已经是恢复新用户审核审核状态，请不要再执行恢复操作！";
                return dsreturn;
            }
        }
        else
        {
            return null;
        }

        #endregion

        #region 5.调用“暂停或恢复审核”执行相应操作

        object[] reyw = IPC.Call("暂停或恢复审核", new object[] { dt });
        DataSet dsr = null;
        if (reyw[0].ToString() == "ok")
        {
            dsr = (DataSet)(reyw[1]);

        }
        return dsr;

        #endregion
                
    }


    /// <summary>
    /// 暂停恢复用户的新业务
    /// </summary>
    /// <param name="dt">相关资料</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    /// edit by zzf 2014.7.10
    [WebMethod(MessageName = "C暂停恢复用户的新业务", Description = "暂停恢复用户的新业务")]
    public DataSet ZTHFYHXYW(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        string jjrjsbh = dt.Rows[0]["经纪人角色编号"].ToString();

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
       

        bool sfxm = false, sfdj = false;
        string item = "";
        #region 1.调用“是否休眠”，如果账户休眠，不再继续向下进行

        object[] rexm = IPC.Call("是否休眠", new object[] { jjrjsbh });
        if (rexm[0].ToString() == "ok")
        {
            string ms = (string)(rexm[1]);
            if (ms == "是")
            {
                sfxm = true;

            }
        }
        else
        {
            return null;
        }

        #endregion

        #region 2.调用“冻结信息”，如果冻结，则不再进行下面的操作

        object[] redjx = IPC.Call("冻结信息", new object[] { jjrjsbh });
        if (redjx[0].ToString() == "ok")
        {
            DataSet dm = (DataSet)(redjx[1]);
            if (dm != null && dm.Tables[0] != null && dm.Tables[0].Rows.Count > 0)
            {
                item = dm.Tables[0].Rows[0]["冻结功能项"].ToString().Trim();
                if (item.Contains("经纪人暂停用户新业务"))
                {
                    sfdj = true;
                } 
            }

        }
        else
        {
            return null;
        }
               
        #endregion

        if (sfxm && sfdj)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！\n\r您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能："+item;
            return dsreturn;

        }
        else if (sfdj)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能："+item;
            return dsreturn;
        }
        else if (sfxm)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";

            return dsreturn;
        }


        
        #region 3.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { jjrjsbh });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

       

        #region 4.调用“暂停或恢复新业务”执行相应的操作

        object[] reyw = IPC.Call("暂停或恢复新业务", new object[] { dt });
        DataSet dsr = null; 
        if (reyw[0].ToString() == "ok")
         {
             dsr = (DataSet)(reyw[1]);
              
         }
        return dsr;
        #endregion
        
    }
    #region 测试执行之间的方法，暂时保存
    /// <summary>
    /// 测试执行之间的方法
    /// </summary>
    /// <param name="sw">Stopwatch实例</param>
    /// <param name="text">提示文本</param>
    public void CalculateTime(System.Diagnostics.Stopwatch sw, string text)
    {
        sw.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = sw.Elapsed;

        // Format and display the TimeSpan value. 
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}:{3}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);

        //读: TextBox1.Text = File.ReadAllText("d:/b.txt", Encoding.Default);
        //写: File.WriteAllText("d:/a.txt", TextBox1.Text, Encoding.Default);
        //追加: File.AppendAllText("d:/a.txt", TextBox1.Text, Encoding.Default);
        //Console.WriteLine("RunTime " + elapsedTime);
        //System.IO.File.AppendAllText("g:/time.txt", "\r\n" + text + "：" + elapsedTime, System.Text.Encoding.Default);

    }
    #endregion
    

    /// <summary>
    /// 交易账户的开通
    /// </summary>
    /// <param name="dt">参数数据表</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.07.01
    [WebMethod(MessageName = "C开通交易账户", Description = "用于开通交易账户")]
    public DataSet SubmitAccount(DataTable dt, string SPstring)
    {
        //Stopwatch stl = new Stopwatch();
        //stl.Start();
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.debug, "开通个人经纪人账户参数个数不对防止篡改", null);
            return null;
        }

        DataSet dsr = new DataSet();

        string strMethodName = "";
        string dlyx = "";
        switch (dt.Rows[0]["方法类别"].ToString())
        {
            case "经纪人单位":
                strMethodName = "开通单位经纪人账户";
                dlyx = dt.Rows[0]["经纪人单位登录邮箱"].ToString().Trim();
                break;
            case "经纪人个人":
                strMethodName = "开通个人经纪人账户";
                dlyx = dt.Rows[0]["经纪人个人登录邮箱"].ToString().Trim();
                break;
            case "买卖家单位":
                strMethodName = "开通单位买卖家账户";
                dlyx = dt.Rows[0]["买卖家单位登录邮箱"].ToString().Trim();
                break;
            case "买卖家个人":
                strMethodName = "开通个人买卖家账户";
                dlyx = dt.Rows[0]["买卖家个人登录邮箱"].ToString().Trim();
                break;
        }
        //Stopwatch sw = Stopwatch.StartNew();

        Hashtable htya = new hqcs().GetZhParas(dlyx);
        if (htya == null || htya.Count != 2)
        {
            //下面这句写日志只暂时用于测试
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.debug, "开通个人经纪人账户参数个数不对", null);

            return null;
        }

        #region 1.判断交易账户是否已经开通

       //object[] reyzm = IPC.Call("交易账户开通状态", new object[] { dlyx });       
        object[] reyzm = htya["结算账户类型状态"] as object[];
        if (reyzm[0].ToString() == "ok")
        {            
            DataSet ds = (DataSet)(reyzm[1]);
            DataTable shdt = ds.Tables[0];
            
            if (shdt.Rows.Count == 0)
            {
                //下面这句写日志只暂时用于测试
                FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.debug, "开通个人经纪人账户参数个数不对：开通", null);
                return null;
            }
            else if (ds.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() != "")
            {
                DataSet dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请已经提交过，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";

                return dsreturn;

            }               

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。    

            //下面这句写日志只暂时用于测试
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.debug, "开通个人经纪人账户参数个数不对：判断交易账户" + reyzm[1].ToString(), null);

            return null;
        }
        #endregion

        //CalculateTime(sw, "判断交易帐号是否开通");        
        //sw.Restart();
              
        #region 2.获取主键。

        string key = "";
        //object[] reynum = IPC.Call("获取主键", new object[] { "AAA_MJMJJYZHYJJRZHGLB", "" });
        object[] reynum = htya["主键"] as object[];
        if (reynum[0].ToString() == "ok")
        {
            key = (string)(reynum[1]);
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。     
            //下面这句写日志只暂时用于测试
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.debug, "开通个人经纪人账户参数个数不对：获取主键" + reyzm[1].ToString(), null);
            return null;
        }

        #endregion

        //CalculateTime(sw, "获取主键");
        //sw.Restart();

        #region 3.调用开通账户的具体方法，执行相关操作

        object[] rezh = IPC.Call(strMethodName, new object[] { dt,key });
        if (rezh[0].ToString() == "ok")
        {
            dsr = (DataSet)(rezh[1]);          
            
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。     
            //下面这句写日志只暂时用于测试
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.debug, "开通个人经纪人账户参数个数不对：具体执行" + reyzm[1].ToString(), null);
            return null;
        }

        #endregion

        //CalculateTime(sw, "执行具体方法");
        //sw.Restart();

        #region 4.调用“用户及关联信息”来得到用户信息和关联信息表

        object[] rex = IPC.Call("用户及关联信息", new object[] { dlyx });
        if (rex[0].ToString() == "ok")
        {
            DataSet  dx= (DataSet)(rex[1]);
            if (dx != null && dx.Tables.Count > 0)
            {
                dsr.Tables.Add(dx.Tables["用户信息"].Copy());
                dsr.Tables[1].TableName = "用户信息";
                dsr.Tables.Add(dx.Tables["关联信息"].Copy());
                dsr.Tables[2].TableName = "关联信息";
            }
            
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。     
            //下面这句写日志只暂时用于测试
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.debug, "开通个人经纪人账户参数个数不对：关联信息" + reyzm[1].ToString(), null);
            return null;
        }

        #endregion

        //CalculateTime(sw, "获取用户信息和关联信息");

        //CalculateTime(stl,"返回数据之前总时间");
        //stl.Reset();
        return dsr;
 
    }

    /// <summary>
    /// 用于交易账户信息的修改
    /// </summary>
    /// <param name="dt">参数数据表</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns>结果数据集</returns>
    [WebMethod(MessageName = "C修改交易账户", Description = "用于修改交易账户")]
    public DataSet UpdateAccount(DataTable dt, string SPstring)
    {       

        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        DataSet dsr = new DataSet();

        string strMethodName = "", dlyx = "", chd = "", key = "";

        switch (dt.Rows[0]["方法类别"].ToString())
        {
            case "经纪人单位":
                strMethodName = "修改单位经纪人账户";
                dlyx = dt.Rows[0]["经纪人单位登录邮箱"].ToString().Trim();
                chd = dt.Rows[0]["经纪人单位是否更换平台管理机构"].ToString().Trim();
                break;
            case "经纪人个人":
                strMethodName = "修改个人经纪人账户";
                dlyx = dt.Rows[0]["经纪人个人登录邮箱"].ToString().Trim();
                chd = dt.Rows[0]["经纪人个人是否更换平台管理机构"].ToString().Trim();
                break;
            case "买卖家单位":
                strMethodName = "修改单位买卖家账户";
                dlyx = dt.Rows[0]["买卖家单位登录邮箱"].ToString().Trim();
                chd = dt.Rows[0]["买卖家单位是否更换经纪人资格证书编号"].ToString().Trim();
                break;
            case "买卖家个人":
                strMethodName = "修改个人买卖家账户";
                dlyx = dt.Rows[0]["买卖家个人登录邮箱"].ToString().Trim();
                chd = dt.Rows[0]["买卖家个人是否更换经纪人资格证书编号"].ToString().Trim(); 
                break;
        }

        #region 1.调用“是否休眠”，如果账户休眠，不再继续向下进行

        object[] rexm = IPC.Call("是否休眠", new object[] { dlyx });
        if (rexm[0].ToString() == "ok")
        {
            DataSet dsreturn = initReturnDataSet().Clone();
            dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
            string  ms = (string)(rexm[1]);
            if (ms=="是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";

                return dsreturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        #region 2.如果更换了平台管理机构或经纪人，调用获取主键的方法，来得到一个新的主键。
        if (chd == "是")
        {
            object[] reynum = IPC.Call("获取主键", new object[] { "AAA_MJMJJYZHYJJRZHGLB", "" });
            if (reynum[0].ToString() == "ok")
            {
                key = (string)(reynum[1]);
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
                return null;
            }
 
        }             

        #endregion

        #region 3.调用修改账户的具体方法，执行相关操作

        object[] rezh = IPC.Call(strMethodName, new object[] { dt, key });
        if (rezh[0].ToString() == "ok")
        {
            dsr = (DataSet)(rezh[1]);

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        #region 4.调用“用户及关联信息”来得到用户信息和关联信息表

        object[] rex = IPC.Call("用户及关联信息", new object[] { dlyx });
        if (rex[0].ToString() == "ok")
        {
            DataSet dx = (DataSet)(rex[1]);
            if (dx != null && dx.Tables.Count > 0)
            {
                dsr.Tables.Add(dx.Tables["用户信息"].Copy());
                dsr.Tables[1].TableName = "用户信息";
                dsr.Tables.Add(dx.Tables["关联信息"].Copy());
                dsr.Tables[2].TableName = "关联信息";
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        return dsr;
    }

    /// <summary>
    /// 用于经纪人驳回交易账户的申请
    /// </summary>
    /// <param name="dt">参数数据表</param>
    /// <param name="SPstring">方篡改串</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.7.7
    [WebMethod(MessageName = "C经纪人驳回买卖家", Description = "经纪人驳回了交易账户的申请")]
    public DataSet JJRRejectMMJ(DataTable dt, string SPstring)
    {

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //  开始监视代码运行时间

        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        string gljsbh = dt.Rows[0]["GLJJRBH"].ToString().Trim();

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        Hashtable htya = new hqcs().GetJJRSHBH(gljsbh);
        if (htya == null || htya.Count != 2)
        {

            return null;
        }

      
       
        #region 1.调用“是否休眠”，如果账户休眠，不再继续向下进行

        object[] rexm = htya["是否休眠"] as object[]; //IPC.Call("是否休眠", new object[] { gljsbh });
        if (rexm[0].ToString() == "ok")
        {
            
            string ms = (string)(rexm[1]);
            if (ms == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";

                return dsreturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion


        #region 2.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = htya["交易账户开通状态"] as object[]; //IPC.Call("交易账户开通状态", new object[] { gljsbh });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

        
       
        string result = "";
        stopwatch.Restart();
        #region 3.调用“经纪人驳回买卖家,如果提交信息成功，则向用户发送提醒”
        object[] rexg = IPC.Call("经纪人驳回买卖家", new object[] { dt });
        
        if (rexg[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(rexg[1]);

            result = ds.Tables["返回值单条"].Rows[0]["执行结果"].ToString().Trim();

            //如果信息提交成功，发送提醒
            if (result == "ok")
            {              

                //构建提醒参数
                DataSet dstx = new DataSet();
                DataTable dttx = new DataTable();
                dttx.Columns.Add("type",typeof(string));
                dttx.Columns.Add("提醒对象登陆邮箱", typeof(string));
                dttx.Columns.Add("提醒对象结算账户类型", typeof(string));
                dttx.Columns.Add("提醒对象角色编号", typeof(string));
                dttx.Columns.Add("提醒对象角色类型", typeof(string));
                dttx.Columns.Add("提醒内容文本", typeof(string));
                dttx.Columns.Add("创建人", typeof(string));
                dttx.Columns.Add("查看地址", typeof(string));
                dttx.Columns.Add("模提醒模块名", typeof(string));

                dstx.Tables.Add(dttx);

                string tp = "集合经销平台";
                string txdlyx = dt.Rows[0]["DLYX"].ToString();
                //string txyhm = dt.Rows[0]["JYFMC"].ToString();
                string tjszhlx = "买家卖家交易账户";
                string tjsbh = dt.Rows[0]["JSBH"].ToString();
                string tjslx = "卖家";
                string creater = dt.Rows[0]["当前经纪人名称"].ToString();
                //string text = " 您选择的经纪人" + creater + "申请审核未通过，请您进入“选择经纪人”模块重新选择！";
                string text = "您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请！";

                dttx.Rows.Add(new object[] { tp,txdlyx,tjszhlx,tjsbh,tjslx,text,creater,"",""});
                
                //IPC.Call("发送提醒", new object[] { dstx });
                new hqcs().SendTX(dstx);
                
            }

            stopwatch.Stop(); //  停止监视
            TimeSpan hs = stopwatch.Elapsed;

            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "C 经纪人驳回买卖家整体执行时间：" + hs.TotalMilliseconds.ToString("#.#####"), null);

            return ds;
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion                     

 
    }


    /// <summary>
    /// 用于经纪人通过交易账户的申请
    /// </summary>
    /// <param name="dt">参数数据表</param>
    /// <param name="SPstring">方篡改串</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.7.7
    [WebMethod(MessageName = "C经纪人通过买卖家", Description = "经纪人通过了交易账户的申请")]
    public DataSet JJRPassMMJ(DataTable dt, string SPstring)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //  开始监视代码运行时间
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        string gljsbh = dt.Rows[0]["GLJJRBH"].ToString().Trim();

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        Hashtable htya = new hqcs().GetJJRSHBH(gljsbh);
        if (htya == null || htya.Count != 2)
        {
            return null;
        }


        #region 1.调用“是否休眠”，如果账户休眠，不再继续向下进行

       // object[] rexm = IPC.Call("是否休眠", new object[] { gljsbh });
        object[] rexm = htya["是否休眠"] as object[];
        if (rexm[0].ToString() == "ok")
        {

            string ms = (string)(rexm[1]);
            if (ms == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";

                return dsreturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        

        #region 2.调用“交易账户开通状态”，判断交易账户是否已经开通

        //object[] reyzm = IPC.Call("交易账户开通状态", new object[] { gljsbh });
        object[] reyzm = htya["交易账户开通状态"] as object[];
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion
        
        string result = "";
        string sc = "";
        #region 3.调用“经纪人通过买卖家,如果提交信息成功，则向用户发送提醒”
        object[] rexg = IPC.Call("经纪人通过买卖家", new object[] { dt });
        if (rexg[0].ToString() == "ok")
        {
            DataSet ds = (DataSet)(rexg[1]);

            result = ds.Tables["返回值单条"].Rows[0]["执行结果"].ToString().Trim();
            sc = ds.Tables["返回值单条"].Rows[0]["附件信息1"].ToString().Trim();
            //如果信息提交成功，发送提醒
            if (result == "ok" && sc!="首次关联")
            {

                //构建提醒参数
                DataSet dstx = new DataSet();
                DataTable dttx = new DataTable();
                dttx.Columns.Add("type", typeof(string));
                dttx.Columns.Add("提醒对象登陆邮箱", typeof(string));
                dttx.Columns.Add("提醒对象结算账户类型", typeof(string));
                dttx.Columns.Add("提醒对象角色编号", typeof(string));
                dttx.Columns.Add("提醒对象角色类型", typeof(string));
                dttx.Columns.Add("提醒内容文本", typeof(string));
                dttx.Columns.Add("创建人", typeof(string));
                dttx.Columns.Add("查看地址", typeof(string));
                dttx.Columns.Add("模提醒模块名", typeof(string));

                dstx.Tables.Add(dttx);

                string tp = "集合经销平台";
                string txdlyx = dt.Rows[0]["DLYX"].ToString();
                string txyhm = dt.Rows[0]["JYFMC"].ToString();
                string tjszhlx = "买家卖家交易账户";
                string tjsbh = dt.Rows[0]["JSBH"].ToString();
                string tjslx = "卖家";
                string creater = dt.Rows[0]["当前经纪人名称"].ToString();
                string text = " 您选择的经纪人" + creater + "申请审核通过，请进行业务操作！";


                dttx.Rows.Add(new object[] { tp, txdlyx, tjszhlx, tjsbh, tjslx, text, creater, "", "" });
                
               // IPC.Call("发送提醒", new object[] { dstx });

            }
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "C 经纪人通过买卖家整体执行时间：" + ts.TotalMilliseconds.ToString("#.#####"), null);
            return ds;
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion                     
      
    }

    /// <summary>
    /// 加载账户维护中开票信息页面的数据信息
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="SPstring">方篡改串</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.7.10
    [WebMethod(MessageName = "C加载开票信息", Description = "加载账户维护中开票信息页面的数据信息")]
    public DataSet GetKPXX(string dlyx, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dlyx }) && IsMD5check())
        {
            return null;
        }
        //结束检查
        DataSet ds = null;
        object[] rexg = IPC.Call("获取加载开票信息", new object[] { dlyx });
        if (rexg[0].ToString() == "ok")
        {
            ds = (DataSet)(rexg[1]);
           
        }
        return ds;
 
    }

    /// <summary>
    /// 账户维护中提交开票信息
    /// </summary>
    /// <param name="dt">参数列表</param>
    /// <param name="SPstring">防篡改字符串</param>
    /// <returns>结果数据集</returns>
    /// edit by zzf 2014.7.10
    [WebMethod(MessageName = "C提交修改开票信息", Description = "账户维护中提交开票信息")]
    public DataSet CommitKPXX(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        #region 1.调用“是否休眠”，如果账户休眠，不再继续向下进行
        string dlyx = dt.Rows[0]["登陆邮箱"].ToString();
        object[] rexm = IPC.Call("是否休眠", new object[] { dlyx });
        if (rexm[0].ToString() == "ok")
        {

            string ms = (string)(rexm[1]);
            if (ms == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";

                return dsreturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        #region 2.调用“提交修改开票信息”,完成信息的提交
        DataSet ds = null;
        object[] rexg = IPC.Call("提交修改开票信息", new object[] { dt });
        if (rexg[0].ToString() == "ok")
        {
            ds = (DataSet)(rexg[1]);

        }
        return ds;
        #endregion
        
    }

    /// <summary>
    /// 变更开票信息
    /// </summary>
    /// <param name="dt">参数列表</param>
    /// <param name="SPstring">防篡改字符串</param>
    /// <returns>结果数据集</returns>
    [WebMethod(MessageName = "C变更修改开票信息", Description = "变更开票信息")]
    public DataSet ChangeKPXX(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        #region 1.调用“是否休眠”，如果账户休眠，不再继续向下进行
        string jsbh = dt.Rows[0]["买家角色编号"].ToString();
        object[] rexm = IPC.Call("是否休眠", new object[] { jsbh });
        if (rexm[0].ToString() == "ok")
        {

            string ms = (string)(rexm[1]);
            if (ms == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";

                return dsreturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }

        #endregion

        #region 2.调用“变更修改开票信息”,完成信息的提交
        DataSet ds = null;
        object[] rexg = IPC.Call("变更修改开票信息", new object[] { dt });
        if (rexg[0].ToString() == "ok")
        {
            ds = (DataSet)(rexg[1]);

        }
        return ds;
        #endregion

     }

    /// <summary>
    /// 通过资格证书编号，得到经纪人的相关信息
    /// </summary>
    /// <param name="zsbh"></param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C证书获取经纪人信息", Description = "通过资格证书编号，得到经纪人的相关信息")]
    public DataSet GetJJRInfoByZSBH(string zsbh,string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { zsbh }) && IsMD5check())
        {
            return null;
        }
        //结束检查
        DataSet ds = null;
        object[] reinfo = IPC.Call("证书获取经纪信息", new object[] { zsbh });
        if (reinfo[0].ToString() == "ok")
        {
            ds = (DataSet)(reinfo[1]);

        }
        return ds;

       
    }

    /// <summary>
    /// 判断身份证号是否已经被注册
    /// </summary>
    /// <param name="strZGZS">类型</param>
    /// <param name="strSFZH">身份证号</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    /// edit by zzf 2014.7.11
    [WebMethod(MessageName="C身份证号重复检验", Description = "判断身份证号是否已经被注册")]
    public DataSet JudgeSFZHXX(string strZHLX, string strSFZH, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { strZHLX, strSFZH }) && IsMD5check())
        {
            return null;
        }
        //结束检查
        //初始化返回值,先塞一行数据
        DataSet ds = null;
        object[] reinfo = IPC.Call("身份证号重复检查", new object[] { strZHLX, strSFZH });
        if (reinfo[0].ToString() == "ok")
        {
            ds = (DataSet)(reinfo[1]);

        }
        return ds;
    }
    /// <summary>
    /// 获取关联表中有关审核通过的买卖家的数据信息，用于“暂停交易方新业务”中判断交易方是否真实有效
    /// </summary>
    /// <param name="dtInfor">参数列表</param>
    /// <param name="SPstring">防止篡改字符串</param>
    /// <returns></returns>
    /// edit by zzf 2014.7.13
    [WebMethod(MessageName = "C已审核买卖家信息", Description = "获取关联表中有关审核通过的买卖家的数据信息")]
    public DataSet GetMMJJYZHData(DataTable dtInfor, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dtInfor }) && IsMD5check())
        {
            return null;
        }
        //结束检查


        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        UserInfo user = new UserInfo();

        DataSet ds = user.GetMMJJYZHData(dtInfor);
        if (ds != null)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功获取买卖家数据信息！";
            DataTable dt = ds.Tables["买卖家数据信息"].Copy();
            dsreturn.Tables.Add(dt);
        }
        else
        {
            return null;
        }

        return dsreturn;

    }

    /// <summary>
    /// 得到交易账户审核的数据信息，如审核意见，审核时间等等，用于账户资料页签中的“查看审核详情页面”
    /// </summary>
    /// <param name="dtInfor">参数列表</param>
    /// <param name="SPstring">防止篡改字符串</param>
    /// <returns></returns>
    /// edit by zzf 2014.7.13
    [WebMethod(MessageName = "C交易账户审核数据信息", Description = "得到交易账户审核的数据信息，如审核意见，审核时间等等")]
    public DataSet GetJYZHSHData(DataTable dtInfor, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dtInfor }) && IsMD5check())
        {
            return null;
        }
        //结束检查


        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        UserInfo user = new UserInfo();

        DataSet ds = user.GetJYZHSHData(dtInfor);
        if (ds != null)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易账户审核信息获取成功！";
            DataTable dt = ds.Tables["交易账户审核数据信息"].Copy();
            dsreturn.Tables.Add(dt);
        }
        else
        {
            return null;
        }

        return dsreturn; 
    }

    /// <summary>
    /// 生成经纪人资格证书图片,获取经纪人资格证书路径
    /// </summary>
    /// <param name="dt">相关资料</param>
    /// <param name="SPstring">防篡改串</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C获取经纪人资格证书", Description = "生成经纪人资格证书图片,获取经纪人资格证书路径")]
    public DataSet GetJJRZGZS(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        string dlyx = dt.Rows[0]["DLYX"].ToString();
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


        #region 调用“是否休眠”，如果休眠，不再往下进行
        object[] rexm = IPC.Call("是否休眠", new object[] { dlyx });
        if (rexm[0].ToString() == "ok")
        {
            string ms = (string)(rexm[1]);
            if (ms == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";

                return dsreturn;
            }
        }
        else
        {
            return null;
        }

        #endregion

        UserInfo user = new UserInfo();

        return user.GetJJRZGZS(dt);

    }


    /// <summary>
    /// 提交银行员工信息
    /// </summary>
    /// <param name="dt">参数列表</param> 
    /// <returns> </returns>
    [WebMethod(MessageName = "C提交银行员工信息", Description = "提交银行员工信息")]
    public DataSet SubmitYHUserInfor(DataTable dt, string SPstring)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        #region//基础验证


        DataSet dsuserinfo = null;
        object[] re = IPC.Call("获取账户的相关信息", new object[] { dt.Rows[0]["经纪人角色编号"].ToString()});
        if (re[0].ToString() == "ok")
        {
            dsuserinfo = (DataSet)(re[1]);
            if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow JBXX = dsuserinfo.Tables[0].Rows[0];
        

        //if (JBXX["交易账户是否开通"].ToString() == "否")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
        //    return dsreturn;
        //}

        #region 2.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { dt.Rows[0]["经纪人角色编号"].ToString() });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion


        if (JBXX["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }

        if (JBXX["是否冻结"].ToString() == "是" || JBXX["冻结功能项"].ToString().Trim()!="")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + JBXX["冻结功能项"].ToString();
            return dsreturn;
        }


        DataSet dsparsa = null;
       re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow JCYZ = dsparsa.Tables[0].Rows[0];

        if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }

        #endregion

        #region  执行业务
        //====加锁
        if (IPC.Call("用户锁定", new object[] { dt.Rows[0]["员工工号"].ToString() })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        re = IPC.Call("提交银行人员信息表", new object[] { dt });
        if (re[0].ToString().Equals("ok"))
        {
            dsreturn= (DataSet)re[1];
            //====解锁(与加锁成对出现)
            IPC.Call("用户解锁", new object[] { dt.Rows[0]["员工工号"].ToString() });
        }
        else
        {
            //====解锁(与加锁成对出现)
            IPC.Call("用户解锁", new object[] { dt.Rows[0]["员工工号"].ToString() });
            return null;
        }

        #endregion



        return dsreturn;
    }

     /// <summary>
    ///修改银行人员信息表
    /// </summary>

    /// <param name="dataTable"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C修改银行人员信息表", Description = "修改银行人员信息表")]
    public DataSet ModifyYHYHInfor(DataTable dataTable, string SPstring)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dataTable }) && IsMD5check())
        {
            return null;
        }

        #region//基础验证


        DataSet dsuserinfo = null;
        object[] re = IPC.Call("获取账户的相关信息", new object[] { dataTable.Rows[0]["经纪人角色编号"].ToString() });
        if (re[0].ToString() == "ok")
        {
            dsuserinfo = (DataSet)(re[1]);
            if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow JBXX = dsuserinfo.Tables[0].Rows[0];


        //if (JBXX["交易账户是否开通"].ToString() == "否")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
        //    return dsreturn;
        //}

        #region 2.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { dataTable.Rows[0]["经纪人角色编号"].ToString() });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion


        if (JBXX["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }

        if (JBXX["是否冻结"].ToString() == "是" || JBXX["冻结功能项"].ToString().Trim()!="")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + JBXX["冻结功能项"].ToString();
            return dsreturn;
        }


        DataSet dsparsa = null;
        re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow JCYZ = dsparsa.Tables[0].Rows[0];

        if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }

        #endregion


        //====加锁
        if (IPC.Call("用户锁定", new object[] { dataTable.Rows[0]["员工工号"].ToString() })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        re = IPC.Call("修改银行人员信息表", new object[] { dataTable });
        if (re[0].ToString().Equals("ok"))
        {
            dsreturn = (DataSet)re[1];
            //====解锁(与加锁成对出现)
            IPC.Call("用户解锁", new object[] { dataTable.Rows[0]["员工工号"].ToString() });
        }
        else
        {
            //====解锁(与加锁成对出现)
            IPC.Call("用户解锁", new object[] { dataTable.Rows[0]["员工工号"].ToString() });
            return null;
        }

        return dsreturn;
    }

     /// <summary>
     /// 删除银行员工信息 
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns> </returns>
    [WebMethod(MessageName = "C删除银行员工信息", Description = "删除银行员工信息")]
    public DataSet DeleteYHYHInfor(DataTable dt,string SPstring)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        #region//基础验证


        DataSet dsuserinfo = null;
        object[] re = IPC.Call("获取账户的相关信息", new object[] { dt.Rows[0]["经纪人角色编号"].ToString() });
        if (re[0].ToString() == "ok")
        {
            dsuserinfo = (DataSet)(re[1]);
            if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow JBXX = dsuserinfo.Tables[0].Rows[0];


        //if (JBXX["交易账户是否开通"].ToString() == "否")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
        //    return dsreturn;
        //}

        #region 2.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { dt.Rows[0]["经纪人角色编号"].ToString() });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

        if (JBXX["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }

        if (JBXX["是否冻结"].ToString() == "是" || JBXX["冻结功能项"].ToString().Trim()!="")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + JBXX["冻结功能项"].ToString();
            return dsreturn;
        }


        DataSet dsparsa = null;
        re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow JCYZ = dsparsa.Tables[0].Rows[0];

        if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }

        #endregion

        //====加锁
        if (IPC.Call("用户锁定", new object[] { dt.Rows[0]["员工工号"].ToString() })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        re = IPC.Call("删除银行员工信息", new object[] { dt });
        if (re[0].ToString().Equals("ok"))
        {
            dsreturn = (DataSet)re[1];
            //====解锁(与加锁成对出现)
            IPC.Call("用户解锁", new object[] { dt.Rows[0]["员工工号"].ToString() });
        }
        else
        {
            //====解锁(与加锁成对出现)
            IPC.Call("用户解锁", new object[] { dt.Rows[0]["员工工号"].ToString() });
            return null;
        }

        return dsreturn;


    }


    #endregion


    #region 银行相关
    /// <summary>
    /// 用户银行存款与用户账户之间的资金转账 WYH 2014.07.24 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C资金转账", Description = "C用户多银行框架--资金转账")]
    public DataSet TransferAccounts(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        //====加锁
        if (IPC.Call("用户锁定", new object[] { dt.Rows[0]["用户邮箱"].ToString() })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        object[] re = IPC.Call("多银行框架-券商发起资金转账", new object[] { dt });

        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { dt.Rows[0]["用户邮箱"].ToString() });

        if (re[0].ToString() == "ok")
        {
            return (DataSet)(re[1]);    
        }
        else
        {
            return null;
        }

    }


     /// <summary>
    /// 券商端发起-变更银行账户(26710)
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C券商端发起-变更银行账户", Description = "C适用于多银行框架--券商端发起-变更银行账户(26710)")]
    public DataSet SRequest_UpdateBankAccount(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        //====加锁
        if (IPC.Call("用户锁定", new object[] { dt.Rows[0]["用户邮箱"].ToString() })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        object[] re = IPC.Call("多银行框架-券商发起变更银行账户", new object[] { dt });

        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { dt.Rows[0]["用户邮箱"].ToString() });

        if (re[0].ToString() == "ok")
        {
            return (DataSet)(re[1]);
        }
        else
        {
            return null;
        }



    }

    [WebMethod(MessageName = "C最大可转金额", Description = "用于银转商时显示到页面")]
    public DataSet MaxZZJE(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        if (dt==null)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
            return dsreturn;
        }
        object[] re = IPC.Call("获取余额", new object[] { dt.Rows[0]["买家角色编号"].ToString() });
        if (re[0].ToString() == "ok")
        {
            double BDYE = Convert.ToDouble(((DataSet)re[1]).Tables[0].Rows[0]["账户当前可用余额"].ToString().Trim() == "" ? "0.00" : ((DataSet)re[1]).Tables[0].Rows[0]["账户当前可用余额"].ToString().Trim());//查询当前登录表可用余额 
            double DSFCGYE = Convert.ToDouble(((DataSet)re[1]).Tables[0].Rows[0]["第三方存管可用余额"].ToString().Trim() == "" ? "0.00" : ((DataSet)re[1]).Tables[0].Rows[0]["第三方存管可用余额"].ToString().Trim());//第三方存管可用余额 
            #region//可转余额
            double KZYE = 0.00;//可转余额
            //登录账号可用余额小于银行端存管管理账户余额，可转账额取登录账号可用余额
            if (BDYE <= DSFCGYE)
            {
                KZYE = BDYE;//可转余额
            }
            else
            {
                KZYE = DSFCGYE;//可转余额
            }
            #endregion
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前帐户最大可转金额" + KZYE.ToString("#0.00") + "元";
            return dsreturn;
        }
        else
        {
            return null;
        }

    }

    #endregion


    #region 收发货、指令、清盘

    #region  商品买卖相关

    /// <summary>
    /// 下达预订单 wyh　２０１４．０６．2６　add
    /// </summary>
    /// <param name="dt">各种信息</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C下达预订单", Description = "下达预定单")]
    public DataSet SetYDD(DataSet ds, string SPstring)
    {
       
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //  开始监视代码运行时间

        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        if (ds == null || ds.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递参数不可为空！";
            return dsreturn;
        }
        DataRow dr = ds.Tables[0].Rows[0];

        Hashtable htya = new hqcs().Getht(dr["HTQX"].ToString(), dr["MJJSBH"].ToString(), dr["SPBH"].ToString());
        
        #region 基础验证
        if (htya == null || htya.Count!=6)
        {
            string csAll = "";
            foreach (System.Collections.DictionaryEntry de in htya)
            {
                csAll += de.Key.ToString()+"-";//得到key
            }
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "capi获取所有参数个数不对：" + htya.Count + "-" + dr["MJJSBH"].ToString() + "-获取参数：" + csAll, null);
            return null;
        }

        DataSet dsparsa = null;
        //object[] re = IPC.Call("平台动态参数", new object[] { "" });
        object[] re = htya["平台动态参数"] as object[];
        if (re[0].ToString() == "ok")
        { 
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        { 
            return null;
        }

        DataRow htPTVal = dsparsa.Tables[0].Rows[0];

        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }

        DataSet dsuserinfo = null;
        //re = IPC.Call("获取账户的相关信息", new object[] { ds.Tables[0].Rows[0]["MJJSBH"].ToString() });
        re = htya["获取账户的相关信息"] as object[];
        if (re[0].ToString() == "ok")
        {
            dsuserinfo = (DataSet)(re[1]);
            if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow htUserVal = dsuserinfo.Tables[0].Rows[0];

        #region 2.调用“交易账户开通状态”，判断交易账户是否已经开通

       // object[] reyzm = IPC.Call("交易账户开通状态", new object[] { ds.Tables[0].Rows[0]["MJJSBH"].ToString() });
        object[] reyzm = htya["交易账户开通状态"] as object[];
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

       

        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("预订单") >= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！\n\r" + "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString().Trim();
            }
            return dsreturn;
        }

        if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("预订单") >= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString().Trim();
            return dsreturn;
        }


        if (htUserVal["是否被默认经纪人暂停新业务"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人已暂停您的新业务，请联系您的经纪人或向平台申诉。";
            return dsreturn;
        }

        if (htUserVal["第三方"].ToString() != "开通")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的第三方存管状态为：" + htUserVal["第三方"].ToString() + "，无法下达预订单。";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！";

            return dsreturn;
        }


        //判断商品是否有效
        string stryx = "";
       // re = IPC.Call("商品有效", new object[] { ds.Tables[0].Rows[0]["SPBH"].ToString() });
        re = htya["商品有效"] as object[]; //IPC.Call("商品有效", new object[] { ds.Tables[0].Rows[0]["SPBH"].ToString() })
        if (re[0].ToString() == "ok")
        {
            stryx = (string)(re[1]);
            if (stryx == null || stryx.Equals(""))
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        if (!stryx.Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该商品刚刚下线，无法买入！";
            return dsreturn;
        }
        #endregion 

        #region  数据重新提取和计算。 关键数据只包括： 商品编号、合同期限、拟买入价格、拟订购数量、收货区域、截止日期、登陆邮箱、角色编号
      
        //获取商品信息
        DataTable dtsp = htya["商品信息"] as DataTable;
        if (dtsp == null || dtsp.Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取商品信息出错！";
            return dsreturn;
        }

        DataRow drsp = dtsp.Rows[0];
        dr["SPMC"] = drsp["SPMC"].ToString();
        dr["GG"] = drsp["GG"].ToString();
        dr["JJDW"] = drsp["JJDW"].ToString();
        //验证经济批量取哪一个值，如果有卖家取卖家设定的经济批量，如果没有就去平台设定的经济批量
        string strjjpl = htya["卖家设定经济批量"] as string;
        if (strjjpl.Trim().Equals(""))
        {
            dr["PTSDZDJJPL"] = drsp["JJPL"].ToString();
        }
        else
        {
            dr["PTSDZDJJPL"] = strjjpl;
        }
        //重新计算你订购金额与冻结定金金额
        dr["YZBSL"] = 0;
        dr["MJDJBL"] = htPTVal["买家订金比率"].ToString();

        double money = 0.00;//拟买入价格
        double result = 0.00;

        if (double.TryParse(dr["NMRJG"].ToString(), out result))
        {
            money = Convert.ToDouble(dr["NMRJG"].ToString());
        }
        else
        {
            money = 0.00;
        }

        double amount = 0.00;//拟买入数量
        if (double.TryParse(dr["NDGSL"].ToString(), out result))
        {
            amount = Convert.ToDouble(dr["NDGSL"].ToString());//拟买入数量
        }
        else
        {
            amount = 0.00;
        }

        dr["NDGJE"] = (money * amount).ToString().Length > 10 ? "0.00" : (money * amount).ToString();//订购金额
        if (dr["NDGJE"].ToString().Trim() != "0.00")
        {
            dr["DJDJ"] = Math.Round((money * amount * Convert.ToDouble(htPTVal["买家订金比率"].ToString())), 2).ToString();//冻结订金的金额
        }
        else
        {
            dr["DJDJ"] = "0.00";
        }

        dr["ZT"] = "竞标";
        dr["JSZHLX"] = htUserVal["结算账户类型"].ToString();
        ds.AcceptChanges();
        #endregion

        #region  执行业务
        re = IPC.Call("预订单提交", new object[] { ds });
        if (re[0].ToString().Equals("ok"))
        {
            if (re[1].ToString().Equals("成功"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "本次预订单提交成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
            }
        
        }
        else
        { 
            return null;
        }

        #endregion

        stopwatch.Stop(); //  停止监视
        TimeSpan hs = stopwatch.Elapsed;

        FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "-C下达预订单整体运行时间：-" + ds.Tables[0].Rows[0]["MJJSBH"].ToString() + "-执行时间:" + hs.TotalMilliseconds.ToString("#.#####"), null);
        return dsreturn;
    }

    /// <summary>
    /// 下达预订单草稿 wyh　２０１４．０６．2６　add
    /// </summary>
    /// <param name="dt">各种信息</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C下达预订单草稿", Description = "下达预定单草稿")]
    public DataSet SetYDDCG(DataSet ds, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        #region 基本验证
        //获取用户账户 信息
        //DataSet dsuserinfo = null;
        //object[] re = IPC.Call("获取账户的相关信息", new object[] { ds.Tables[0].Rows[0]["买家角色编号"].ToString() });
        //if (re[0].ToString() == "ok")
        //{
        //    dsuserinfo = (DataSet)(re[1]);
        //    if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
        //    {
        //        return null;
        //    }
        //}
        //else
        //{
        //    return null;
        //}

        //DataRow htUserVal = dsuserinfo.Tables[0].Rows[0];

        //if (htUserVal["是否休眠"].ToString().Trim() == "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
        //    return dsreturn;
        //}
        #endregion

        if (IPC.Call("预订单草稿提交", new object[] { ds })[1].ToString().Equals("成功"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "预订单草稿保存成功！";
        }
        else
        {
            //写入日志
            return null;
        }

        return dsreturn;
    }

    /// <summary>
    /// 撤销预订单 wyh　２０１４．０６．2６　add
    /// </summary>
    /// <param name="dt">各种信息</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C撤销预订单", Description = "撤销预订单")]
    public DataSet Ydd_CX(string Number, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { Number }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        #region 基础验证

        DataTable dt = null;
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@Number"] = Number;
        string sql = "SELECT MJJSBH,ZT FROM AAA_YDDXXB WHERE Number=@Number";
        Hashtable return_htzkls = I_DBL.RunParam_SQL(sql, "yddinfo", input);

        if ((bool)(return_htzkls["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_htzkls["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["yddinfo"].Rows.Count > 0)
            {
                dt = dsGXlist.Tables["yddinfo"];
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有查找到单号为：" + Number + "的预订单。";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
           // dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_htzkls["return_errmsg"].ToString();
            //dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return null;
        }

        DataSet dsparsa = null;
        object[] re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow htPTVal = dsparsa.Tables[0].Rows[0];
        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }

        DataSet dsuserinfo = null;
        re = IPC.Call("获取账户的相关信息", new object[] { dt.Rows[0]["MJJSBH"].ToString() });
        if (re[0].ToString() == "ok")
        {
            dsuserinfo = (DataSet)(re[1]);
            if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow htUserVal = dsuserinfo.Tables[0].Rows[0];
        #region 2.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { dt.Rows[0]["MJJSBH"].ToString() });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

      
        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("预订单") >= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！\n\r" + "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString();
                
            }
            return dsreturn;
        }

        if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("预订单") >= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString();
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;
        }

        if (dt.Rows[0]["ZT"].ToString() == "中标")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的预订单已中标，无法撤销！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;
        }
        if (dt.Rows[0]["ZT"].ToString() == "撤销")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的预订单已撤销，无法重复撤销！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;
        }


        #endregion

        #region 执行业务逻辑

        //====加锁
        if (IPC.Call("用户锁定", new object[] { htUserVal["登录邮箱"].ToString() })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        re = IPC.Call("预订单撤销", new object[] { Number });
        IPC.Call("用户解锁", new object[] { htUserVal["登录邮箱"].ToString() });
        if (re[0].ToString().Equals("ok"))
        {
            dsreturn = (DataSet)re[1];          
           // ====解锁(与加锁成对出现)
          
        }
        else
        {
            return null;
        }
      
        #endregion

        return dsreturn;
    }


    /// <summary>
    /// 获取已撤销的预订单的信息--预订单修改时使用 wyh　２０１４．０６．2６　add
    /// </summary>
    /// <param name="dt">各种信息</param>
    /// <returns></returns>
    [WebMethod(MessageName = "C返回撤销的预订单原始信息", Description = "预订单修改时获取预订单原始信息")]
    public DataSet YDD_XG(string Number, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { Number }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        #region 验证商品是否有效

        //string spsfyx = "否";
        //DataTable dt = null;
        //I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        //Hashtable param = new Hashtable();
        //param.Add("@number", Number);

        //string sql_SPSFYX = "SELECT SFYX FROM AAA_PTSPXXB WHERE SPBH=(SELECT SPBH FROM AAA_YDDXXB WHERE NUMBER=@number)";
        //Hashtable return_ht = I_DBL.RunParam_SQL(sql_SPSFYX, "sfyx", param);
        //if ((bool)(return_ht["return_float"])) //说明执行完成
        //{
        //    dt = ((DataSet)return_ht["return_ds"]).Tables["sfyx"];
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        spsfyx = dt.Rows[0][0].ToString();
        //    }
        //}
       
      
        ////商品是否有效
        //if (spsfyx != "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该商品刚刚已下线，请重新选择其他的商品！";
        //    return dsreturn;
        //}
        #endregion

      
        object[] re = IPC.Call("返回撤销的预订单原始信息", new object[] { Number });
        if (re[0].ToString().Equals("ok"))
        {
            dsreturn = (DataSet)re[1];
        }
        else
        {
            return null;
        }
        return dsreturn;
    }

    /// <summary>
    /// 发布投标单 wyh 2014.06.30 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <param name="SPstring">加密防篡改字符串</param>
    /// <returns>返回符合dsreturn结构的Dataset，出错是返回null</returns>
    [WebMethod(MessageName="C发布投标单" ,Description = "投标单")]
    public DataSet SetTBD(DataSet ds, string SPstring)
    {
        //System.Diagnostics.Stopwatch stopwatch1 = new System.Diagnostics.Stopwatch();

        //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //stopwatch.Start();
       // stopwatch1.Start();
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {

           
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

       
        string htqx_new = ds.Tables["TBD"].Rows[0]["HTQX"].ToString();
       
        Hashtable htya =  new hqcs().GetTBDYZ(ds.Tables[0].Rows[0]["MJJSBH"].ToString(), ds.Tables[0].Rows[0]["SPBH"].ToString(), htqx_new, ds.Tables[0].Rows[0]["DLYX"].ToString());
       // stopwatch.Stop();
       //  TimeSpan hs=stopwatch.Elapsed;
        
        
        #region 基础验证
        
        if (htya == null || htya.Count != 6)
        {
            string csAll = "";
            foreach (System.Collections.DictionaryEntry de in htya)
            {
                csAll += de.Key.ToString() + "-";//得到key
            }
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBDcapi参数获取个数不对：" + csAll, null);
            return null;
        }

        DataSet dsparsa = null;
        //object[] re = IPC.Call("平台动态参数", new object[] { "" });
        object[] re = htya["平台动态参数"] as object[];
        if (re[0].ToString() == "ok")
        {
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD平台动态参数1：没有数据", null);
                return null;
            }
        }
        else
        {
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD平台动态参数：" + re[1].ToString(), null);
            return null;
        }

        DataRow htPTVal = dsparsa.Tables[0].Rows[0];

        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }


        DataSet dsuserinfo = null;
        //re = IPC.Call("获取账户的相关信息", new object[] { ds.Tables[0].Rows[0]["MJJSBH"].ToString() });//根据买家角色编号获取用户信息
         re = htya["获取账户的相关信息"] as object[];
        if (re[0].ToString() == "ok")
        {
            dsuserinfo = (DataSet)(re[1]);
            if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
            {
                FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD获取账户的相关信息1：无数据", null);
                return null;
            }
        }
        else
        {
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD获取账户的相关信息：" + re[1].ToString(), null);
            return null;
        }

        DataRow htUserVal = dsuserinfo.Tables[0].Rows[0];

       
        #region 2.调用“交易账户开通状态”，判断交易账户是否已经开通

       // object[] reyzm = IPC.Call("交易账户开通状态", new object[] { ds.Tables[0].Rows[0]["MJJSBH"].ToString() });
        object[] reyzm = htya["交易账户开通状态"] as object[];
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD交易账户开通状态：" + dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim(), null);
                    return dsreturn;

                }
            }

        }
        else
        {
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD交易账户开通状态1：" + reyzm[1].ToString(), null);
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion



        if (htUserVal["结算账户类型"].ToString() != "买家卖家交易账户")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您是经纪人交易账户，无法下达投标单！";
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD经纪人账户无法下达：" + ds.Tables[0].Rows[0]["DLYX"].ToString(), null);
            return dsreturn;

        }

       
        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("投标单") >= 0 )
            {
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！\n\r" + "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString().Trim();
            }
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD经纪人账户休眠：" + ds.Tables[0].Rows[0]["DLYX"].ToString(), null);
            return dsreturn;
        }

        if (htUserVal["冻结功能项"].ToString().Trim().Contains("投标单"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString().Trim();
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD冻结：" + ds.Tables[0].Rows[0]["DLYX"].ToString(), null);
            return dsreturn;
        }


        if (htUserVal["是否被默认经纪人暂停新业务"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人已暂停您的新业务，请联系您的经纪人或向平台申诉。";
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD暂停经纪人业务：" + ds.Tables[0].Rows[0]["DLYX"].ToString(), null);
            return dsreturn;
        }

        if (htUserVal["第三方"].ToString() != "开通")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
           
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！";
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD第三方开通：" + ds.Tables[0].Rows[0]["DLYX"].ToString(), null);
            return dsreturn;
        }


        //判断商品是否在冷静期内
        if (!htqx_new.Equals("即时")) // 2013.6.25 wyh add 即时区分
        {
           // re = IPC.Call("是否冷静期", new object[] { ds.Tables["TBD"].Rows[0]["SPBH"].ToString(), ds.Tables["TBD"].Rows[0]["HTQX"].ToString() });
            re = htya["是否冷静期"] as object[];
            if(!re[1].ToString().Equals("否"))
            {
                FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "已进冷机期1", null);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
             //   DateTime times = Convert.ToDateTime(jssj);
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您所投标的商品正处于冷静期，该投标单无法提交。";
                return dsreturn;
            }
        }


        //判断商品是否有效
        string stryx = "";
       // re = IPC.Call("商品有效", new object[] { ds.Tables["TBD"].Rows[0]["SPBH"].ToString() });
        re = htya["商品有效"] as object[];
        if (re[0].ToString() == "ok")
        {
            stryx = (string)(re[1]);
            if (stryx == null || stryx.Equals(""))
            {
                FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBDSP无效2" + re[1].ToString(), null);
                return null;
            }
        }
        else
        {
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBDSP无效1"+re[1].ToString(), null);
            return null;
        }

        if (!stryx.Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该商品刚刚下线，无法卖出！";
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBDSP无效3" + re[1].ToString(), null);
            return dsreturn;
        }


        #endregion
       
        #region 关键数据只包括： 商品编号、合同期限、卖方设定经济批量、投标拟售量、投标价格、卖方增值税、商品产地、供货区域、资质,登陆邮箱，卖家角色编号。

        if (ds == null || ds.Tables["TBD"].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递参数不可为空！";
            return dsreturn;
        }

        DataRow drtbd = ds.Tables["TBD"].Rows[0];
        
        drtbd["JSZHLX"] = htUserVal["结算账户类型"].ToString(); //更新结算账户类型

        //获取商品基本信息
        DataTable dtsp = new GetSPInfo().dtSPInfo(drtbd["SPBH"].ToString());
        if (dtsp == null || dtsp.Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取商品信息出错！";
            return dsreturn;
        }

        DataRow drsp = dtsp.Rows[0];
        drtbd["SPMC"] = drsp["SPMC"].ToString();
        drtbd["GG"] = drsp["GG"].ToString();
        drtbd["JJDW"] = drsp["JJDW"].ToString();
        drtbd["PTSDZDJJPL"] = drsp["JJPL"].ToString();

        //计算你订购金额与冻结的都表保证金
        drtbd["MJTBBZJBL"] = htPTVal["卖家投标保证金比率"].ToString();
        drtbd["MJTBBZJZXZ"] = htPTVal["卖家投标保证金最小值"].ToString();
        double money = 0.00;//拟买入金额
        double result = 0.00;
        double MJDJDJBL = Convert.ToDouble(htPTVal["卖家投标保证金比率"].ToString());
        double MJTBBZJZXZ = Convert.ToDouble(htPTVal["卖家投标保证金最小值"].ToString());

        if (double.TryParse(drtbd["TBJG"].ToString(), out result))
        {
            money = Convert.ToDouble(drtbd["TBJG"].ToString());
        }
        else
        {
            money = 0.00;
        }

        double amount = 0.00;//拟买入数量
        if (double.TryParse(drtbd["TBNSL"].ToString(), out result))
        {
            amount = Convert.ToDouble(drtbd["TBNSL"].ToString());//拟买入数量
        }
        else
        {
            amount = 0.00;
        }
        drtbd["TBJE"] = (money * amount).ToString().Length > 10 ? "0.00" : (money * amount).ToString();//订购金额
        if (drtbd["TBJE"].ToString().Trim() != "0.00")
        {
            drtbd["DJTBBZJ"] = Math.Round((money * amount * MJDJDJBL), 2) >= MJTBBZJZXZ ? Math.Round((money * amount * MJDJDJBL), 2).ToString() : MJTBBZJZXZ.ToString();//冻结订金的金额
        }
        else
        {
            drtbd["DJTBBZJ"] = "0.00";
        }


        #endregion         
        #region 开始业务处理
       
        ds.AcceptChanges();
        
        //验证余额

        double DJ = Convert.ToDouble(ds.Tables["TBD"].Rows[0]["DJTBBZJ"]);//要冻结的投标保证金
        double YE = 0.00; //AboutMoney.GetMoneyT(ds.Tables[0].Rows[0]["DLYX"].ToString(), "");//当前余额
        //re = IPC.Call("账户当前可用余额", new object[] { htUserVal["登录邮箱"].ToString() });
        re = htya["账户当前可用余额"] as object[];
        if (re[0].ToString() == "ok")
        {
            YE =  double.Parse((string)(re[1]));   
        }
        else
        {
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "账户当前可用余额1" + re[1].ToString(), null);
            return null;
        }

        //查询用户余额
        if (DJ > YE)
        {
            
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您交易账户中的可用资金余额为" + YE.ToString("0.00") + "元，与该投标单的投标保证金差额为" + (Math.Round((DJ - YE), 2)).ToString("0.00") + "元。您可增加账户的可用资金余额或修改投标单。";
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBDSP余额不足" + ds.Tables[0].Rows[0]["DLYX"].ToString(), null);
            return dsreturn;
        }
        else if (ds.Tables["TBD"].Rows[0]["FLAG"].ToString().Trim() != "提交")
        {
            
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "同时确认，该《投标单》一经提交，即依规冻结" + (Math.Round(DJ, 2)).ToString("0.00") + "元的投标保证金。";
            return dsreturn;
        }

        //执行投标单插入操作
        re = IPC.Call("发布投标单", new object[] { ds });

        if (re[0].ToString() == "ok")
        {
            if (re[1].ToString().Equals("成功"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "本次投标单提交成功！";  
            }
            else
            {
                FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "发布投标单1" + re[1].ToString(), null);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                
            }
        
            return dsreturn;
        }
        else
        {
            return null;
        }
       


        #endregion

        return dsreturn;
    }

    /// <summary>
    /// 投标单撤销 wyh 2014.06.30 
    /// </summary>
    /// <param name="Number">投标单单号</param>
    /// <returns>成功：撤销成功；返回字符串不是成功：失败原因。 ；null :程序出现异常</returns>
    [WebMethod(MessageName = "C投标单撤销", Description = "投标单撤销")]
    public DataSet TBD_CX(string Number, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { Number }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        #region 验证们

        DataTable dt = null;
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@Number"] = Number;
        string sql = "SELECT * FROM AAA_TBD WHERE Number=@Number";
        Hashtable return_htzkls = I_DBL.RunParam_SQL(sql, "tbdinfo", input);

        if ((bool)(return_htzkls["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_htzkls["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["tbdinfo"].Rows.Count > 0)
            {
                dt = dsGXlist.Tables["tbdinfo"];
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有查找到单号为：" + Number + "的预订单。";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
            // dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_htzkls["return_errmsg"].ToString();
            //dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return null;
        }


        DataSet dsparsa = null;
        object[] re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow htPTVal = dsparsa.Tables[0].Rows[0];



        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }


        DataSet dsuserinfo = null;
        re = IPC.Call("获取账户的相关信息", new object[] { dt.Rows[0]["MJJSBH"].ToString() });
        if (re[0].ToString() == "ok")
        {
            dsuserinfo = (DataSet)(re[1]);
            if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow htUserVal = dsuserinfo.Tables[0].Rows[0];

        //if (htUserVal["交易账户是否开通"].ToString().Trim() != "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
        //    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
        //    return dsreturn;
        //}

        #region 2.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { dt.Rows[0]["MJJSBH"].ToString() });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("投标单") >= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！\n\r" + "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString().Trim(); ;
            }


            return dsreturn;
        }

        if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("投标单") >= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString().Trim();
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;
        }

        //if (htUserVal["是否被默认经纪人暂停新业务"].ToString().Trim() == "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人已暂停您的新业务，请联系您的经纪人或向平台申诉。";
        //    return dsreturn;
        //}

       //冷静期验证
        re = IPC.Call("是否冷静期", new object[] { dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString() });
        if (!re[1].ToString().Equals("否"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            //   DateTime times = Convert.ToDateTime(jssj);
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您所投标的商品正处于冷静期，无法撤销。";
            return dsreturn;
        }


        //if (isInLJQ == "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该商品已进入冷静期，无法撤销投标单！";
        //    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
        //    return dsreturn;
        //}
        if (dt.Rows[0]["ZT"].ToString() == "中标")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的投标单已中标，无法撤销！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;
        }
        if (dt.Rows[0]["ZT"].ToString() == "撤销")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的投标单已撤销，无法重复撤销！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;
        }
        #endregion

        #region 开始执行业务逻辑
        //====加锁
        if (IPC.Call("用户锁定", new object[] { htUserVal["登录邮箱"].ToString() })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        re = IPC.Call("投标单撤销", new object[] { Number });

        //解锁
        IPC.Call("用户解锁", new object[] { htUserVal["登录邮箱"].ToString() });

        if (re[0].ToString() == "ok")
        {
            dsreturn = (DataSet)re[1];   
        }
        else
        {
            return null;
        }
        #endregion

        return dsreturn;

    }


    /// <summary>
    /// 投标单修改时获取投标单原始信息 wyh 2014.06.30 
    /// </summary>
    /// <param name="Number">投标单number值</param>
    /// <param name="SPstring">防篡改字符串</param>
    /// <returns>符合dsreturn结构的Dataset</returns>
    [WebMethod(MessageName = "C投标单修改", Description = "投标单修改")]
    public DataSet TBD_XG(string Number, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { Number }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        #region 基础验证

        //验证商品有效

        //string spsfyx = "否";
        //DataTable dt = null;
        //I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        //Hashtable param = new Hashtable();
        //param.Add("@number", Number);

        //string sql_SPSFYX = "SELECT SFYX FROM AAA_PTSPXXB WHERE SPBH=(SELECT SPBH FROM AAA_TBD WHERE NUMBER=@number)";
        //Hashtable return_ht = I_DBL.RunParam_SQL(sql_SPSFYX, "sfyx", param);
        //if ((bool)(return_ht["return_float"])) //说明执行完成
        //{
        //    dt = ((DataSet)return_ht["return_ds"]).Tables["sfyx"];
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        spsfyx = dt.Rows[0][0].ToString();
        //    }
        //}


        ////商品是否有效
        //if (spsfyx != "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该商品刚刚已下线，请重新选择其他的商品！";
        //    return dsreturn;
        //}

        #endregion 

       object[] re= IPC.Call("返回投原始信息", new object[] { Number });
       if (re[0].ToString().Equals("ok"))
       {
           dsreturn = (DataSet)re[1];
       }
       else
       {
           dsreturn = null;
       }


        return dsreturn;
    }

    /// <summary>
    /// 投标单草稿提交 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <param name="SPstring">防篡改字符串</param>
    /// <returns>符合dsreturn结构的Dataset</returns>
    [WebMethod(MessageName = "C下达投标单草稿", Description = "下达投标单草稿")]
    public DataSet SetTBDCG(DataSet ds, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        #region 基本验证
        //获取用户账户 信息
        //DataSet dsuserinfo = null;
        //object[] re = IPC.Call("获取账户的相关信息", new object[] { ds.Tables[0].Rows[0]["买家角色编号"].ToString() });
        //if (re[0].ToString() == "ok")
        //{
        //    dsuserinfo = (DataSet)(re[1]);
        //    if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
        //    {
        //        return null;
        //    }
        //}
        //else
        //{
        //    return null;
        //}

        //DataRow htUserVal = dsuserinfo.Tables[0].Rows[0];

        //if (htUserVal["是否休眠"].ToString().Trim() == "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
        //    return dsreturn;
        //}
        #endregion

        object[] re = IPC.Call("投标单草稿提交", new object[] { ds });
        if (re[0].ToString().Equals("ok"))
        {
            if (re[1].ToString().Equals("成功"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "投标单草稿保存成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
            }
            
        }
        else
        {
            dsreturn = null;
        }

        return dsreturn;

    }

      /// <summary>
     /// 异常投标单详情 2014.06.30 wyh add
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns> 返回符合dsreturn结构的dataset</returns>
    [WebMethod( MessageName="C异常投标单详情", Description = "异常投标单详情")]
    public DataSet CXTBDYCZZ(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        DataSet dspar = new DataSet();
        dspar.Tables.Add(dt);
        object[] re = IPC.Call("查询投标单的异常资质", new object[] { dspar });
        if (re[0].ToString().Equals("ok"))
        {
            dsreturn = (DataSet)re[1];
        }
        else
        {
            dsreturn = null;
        }
        return dsreturn;
    }

     /// <summary>
     /// 异常投标单修改 2014.06.30 
     /// </summary>
     /// <param name="dt">参数列表</param> 
     /// <returns>返回修改成功或失败</returns>
    [WebMethod(MessageName = "C异常投标单修改", Description = "异常投标单修改")]
    public DataSet UpdateTBDYCZZ(DataSet dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        #region 基础验证

        DataSet dsuserinfo = null;
        object[] re = IPC.Call("获取账户的相关信息", new object[] { dt.Tables["投标单号表"].Rows[0]["买家角色编号"].ToString() });//根据买家角色编号获取用户信息
        if (re[0].ToString() == "ok")
        {
            dsuserinfo = (DataSet)(re[1]);
            if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow htUserVal = dsuserinfo.Tables[0].Rows[0];
        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }


        #endregion

        #region 开始业务操作
        //====加锁
        if (IPC.Call("用户锁定", new object[] { htUserVal["登录邮箱"].ToString() })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙！";
            return dsreturn;
        }

        re = IPC.Call("异常投标单修改", new object[] { dt });

        //解锁
        IPC.Call("用户解锁", new object[] { htUserVal["登录邮箱"].ToString() });

        if (re[0].ToString().Equals("ok"))
        {
            if (re[1].ToString().Equals("成功"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "确认修改成功！";
              
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
            }


           
        }
        else
        {
            dsreturn = null;
        }
        #endregion


        return dsreturn;


    }

    /// <summary>
    /// 定标操作 2014.06.30  wyh 2014.06.30
    /// </summary>
    /// <param name="test">参数</param>
    /// <returns>返回固定格式数据集</returns>
    [WebMethod(MessageName = "C定标", Description = "定标操作")]
    public DataSet Jhjx_db(DataTable ht, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { ht }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        #region 基础验证

        DataSet dsparsa = null;
        object[] re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow htPTVal = dsparsa.Tables[0].Rows[0];

        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }


        DataSet dsuserinfo = null;
        re = IPC.Call("获取账户的相关信息", new object[] { ht.Rows[0]["卖家角色编号"].ToString() });
        if (re[0].ToString() == "ok")
        {
            dsuserinfo = (DataSet)(re[1]);
            if (dsuserinfo == null || dsuserinfo.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow hsuser = dsuserinfo.Tables[0].Rows[0];

        //验证是开通交易账户
        //if (hsuser["交易账户是否开通"].ToString().Trim() != "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
        //    return dsreturn;
        //}

        #region 2.调用“交易账户开通状态”，判断交易账户是否已经开通

        object[] reyzm = IPC.Call("交易账户开通状态", new object[] { ht.Rows[0]["卖家角色编号"].ToString() });
        if (reyzm[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(reyzm[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。          
            return null;
        }
        #endregion

        //验证是否休眠

        if (hsuser["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        #endregion

        #region 开始逻辑操作
        //====加锁
        if (IPC.Call("用户锁定", new object[] { hsuser["登录邮箱"].ToString() })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        re = IPC.Call("定标", new object[] { ht });

        //解锁
        IPC.Call("用户解锁", new object[] { hsuser["登录邮箱"].ToString() });
        if (re[0].ToString().Equals("ok"))
        {
            dsreturn = (DataSet)re[1];
        }
        else
        {
            dsreturn = null;
        }

        #endregion

        return dsreturn;


    }

    /// <summary>
    /// 获取用户关联经纪人的管理机构 wyh 2014.07.10 
    /// </summary>
    /// <param name="DLYX"></param>
    /// <returns></returns>
    [WebMethod(MessageName="C关联经纪人平台管理结构",Description = "获取用户关联经纪人的管理机构")]
    public DataSet GetGLJJRGLJG(string DLYX, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { DLYX }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        object[] re = IPC.Call("关联经纪人信息", new object[] { DLYX });
        if (re[0].ToString().Equals("ok"))
        {
            DataSet dsptgl = (DataSet)re[1];
            if (dsptgl != null && dsptgl.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable("JJR");
                dt.Columns.Clear();
                DataColumn cd = new DataColumn();
                cd.DataType = typeof(string);
                dt.Columns.Add(cd);
                dt.Clear();
                DataRow dr = dt.NewRow();
                dr[0] = dsptgl.Tables[0].Rows[0]["关联经纪人平台管理机构"].ToString();
                dt.Rows.Add(dr);

                dsreturn.Tables.Add(dt);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";


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


        return dsreturn;
    }

    /// <summary>
    /// 得到投标单的信息电子合同专用 wyh 2014.07.14 
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C得到投标单的信息HT", Description = "得到投标单的信息电子合同专用")]
    public DataSet Get_TBDData(DataTable dt,string SPstring )
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        object[] re = IPC.Call("得到投标单的信息HT", new object[] { dt });
        if (re[0].ToString().Equals("ok"))
        {
            dsreturn = (DataSet)re[1];
        }
        else
        {
            dsreturn = null;
        }
        return dsreturn;
    }

    /// <summary>
    /// 得到预订单的数据信息电子合同专用 wyh 2014.07.14
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod( MessageName="C得到预订单的信息HT" ,Description = "得到预订单的数据信息电子合同专用")]
    public DataSet Get_YDDData(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        object[] re = IPC.Call("得到预订单的信息HT", new object[] { dt });
        if (re[0].ToString().Equals("ok"))
        {
            dsreturn = (DataSet)re[1];
        }
        else
        {
            dsreturn = null;
        }
        return dsreturn;
    }


    [WebMethod(MessageName = "C获取预订单草稿信息", Description = "获取预订单草稿信息")]
    public DataSet GetYDDcaogaoinfo(string Number, string SPstring)
    {
      //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { Number }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        DataSet dsparsa = null;
        object[] re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            dsparsa = (DataSet)(re[1]);
            if (dsparsa == null || dsparsa.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        DataRow htPTVal = dsparsa.Tables[0].Rows[0];


        re = IPC.Call("获取预订单草稿信息", new object[] { Number });
        if (re[0].ToString().Equals("ok"))
        {
            dsreturn = (DataSet)re[1];
            DataTable dt = dsreturn.Tables["SPXX"];
            if(dt!= null && dt.Rows.Count>0)
            {
                dt.Rows[0]["NEWDJDJ"] = (Convert.ToDouble(dt.Rows[0]["NDGJE"].ToString().Trim()) * Convert.ToDouble(htPTVal["买家订金比率"].ToString().Trim())).ToString("#0.00");
            }
        }
        else
        {
            dsreturn = null;
        }
        return dsreturn;
        


    }


    [WebMethod(MessageName = "C获取投标单草稿信息", Description = "获取投标单草稿信息")]
    public DataSet TBD_GetCGinfo(string Number, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { Number }) && IsMD5check())
        {
            return null;
        }

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        object[] re = IPC.Call("获取投标单草稿信息", new object[] { Number });
        if (re[0].ToString().Equals("ok"))
        {
            dsreturn = (DataSet)re[1];

        }
        else
        {
            dsreturn = null;
        }
        return dsreturn;



    }

    /// <summary>
    /// 判断是否签订承诺书
    /// </summary>
    /// <param name="dt">参数表</param>
    /// <param name="SPstring">防止篡改字符串</param>
    /// <returns></returns>
    /// edit by zzf 2014.7.14
    [WebMethod(MessageName="C判断是否签订承诺书", Description = "判断是否签订承诺书")]
    public DataSet JudgeSFQDCNS(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        object[] re = IPC.Call("判断是否签订承诺书", new object[] { dt });
        if (re[0].ToString().Equals("ok"))
        {
            return  (DataSet)re[1];
        }
        else
        {
            return null;
        }       
 
    }

    /// <summary>
    /// 交易账户签订承诺书
    /// </summary>
    /// <param name="dt">参数表</param>
    /// <param name="SPstring">防止篡改字符串</param>
    /// <returns></returns>
    /// edit by zzf 2014.7.14
    [WebMethod(MessageName = "C交易账户签订承诺书", Description = "交易账户签订承诺书")]
    public DataSet JYZHQDCNS(DataTable dt, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }

        object[] re = IPC.Call("交易账户签订承诺书", new object[] { dt });
        if (re[0].ToString().Equals("ok"))
        {
            return (DataSet)re[1];
        }
        else
        {
            return null;
        }

    }

    #region //商品买卖--清盘 zhouli 2014.07.08 add
    /// <summary>
    /// 人工清盘点击列表单条数据清盘信息 -- zhouli  2014.07.08 add --查询方法，无需加锁
    /// </summary>
    /// <param name="dt">包含：“dlyx”：登陆邮箱；“num” :AAA_ZBDBXXB,Number值</param>
    /// <returns>返回dataset内含表明为“qpinfo”的Datatable</returns>
    [WebMethod(MessageName = "C获取清盘数据", Description = "返回dataset内含表明为“qpinfo”的Datatable，null:程序出错！")]
    public DataSet GetQBInfo(DataTable ht, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { ht }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
                
        if (ht == null || ht.Rows.Count<1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
            return dsreturn;
        }

        string DLYX = ht.Rows[0]["dlyx"].ToString();
        #region//1.验证是否休眠
        DataRow JBXX = null;
        object[] re = IPC.Call("用户基本信息", new object[] { DLYX });
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                JBXX = ((DataSet)re[1]).Tables[0].Rows[0];
                if (JBXX["是否休眠"].ToString() == "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                    return dsreturn;
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                return dsreturn;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
            return dsreturn;
        }
        #endregion
        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);
        re = IPC.Call("获取清盘数据", new object[] { ht });
        if (re[0].ToString() == "ok")
        {
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                dsreturn = (DataSet)re[1];
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取清盘数据失败！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
        }

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;

    }
    /// <summary>
    ///根据中标定标信息表编号获得投标资料审核表中的相关信息  zhouli 2014.07.08 add--查询方法，无需加锁
    /// </summary>
    /// <param name="num">中标定标信息表Number</param>
    /// <returns>Dataset</returns>
    [WebMethod(MessageName = "C投标资料审核表相关信息", Description = "返回dataset内含表明为“sh”的Datatable，null:程序出错！用于人工清盘提交时。")]
    public DataSet GetTBZLSHinfo(string number, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { number }) && IsMD5check())
        {
            return null;
        }
        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);
        object[] re = IPC.Call("投标资料审核表相关信息", new object[] { number });
        if (re[0].ToString() == "ok")
        {
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                return (DataSet)re[1];
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
    /// <summary>
    /// 获取中标定标信息表上传文件的信息，用于验证  zhouli 2014.07.08  add--查询方法，无需加锁
    /// </summary>
    /// <param name="num">中标定标信息表Number</param>
    /// <returns>Dataset</returns>
    [WebMethod(MessageName = "C获取中标定标信息表上传文件的信息", Description = "返回dataset内含表明为“docinfo”的Datatable，null:程序出错！用于人工清盘提交时。")]
    public DataSet GetSHWJinfo(string number, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { number }) && IsMD5check())
        {
            return null;
        }
        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);
        object[] re = IPC.Call("获取中标定标信息表上传文件的信息", new object[] { number });
        if (re[0].ToString() == "ok")
        {
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                return (DataSet)re[1];
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
    /// <summary>
    /// 人工清盘时用户上传证明文件写入数据表  zhouli 2014.07.08  add
    /// </summary>
    /// <param name="dt">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    [WebMethod(MessageName = "C用户上传证明文件", Description = "返回dataset内含表明为“docinfo”的Datatable，null:程序出错！人工清盘时卖家确认用户上传证明文件写入数据表。")]
    public DataSet DWControversial(DataTable dt, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        #region//1.验证传递的参数是否为空
        if (dt==null||dt.Rows.Count<1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
            return dsreturn;
        }
        #endregion

        string DLYX = dt.Rows[0]["dlyx"].ToString();
        string HTBH = dt.Rows[0]["htbh"].ToString();

        #region//2.验证是否在服务时间内
       object[]  re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                DataRow JCYZ = ((DataSet)re[1]).Tables[0].Rows[0];
                if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                    return dsreturn;
                }
                if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(工作时间)，暂停交易！";
                    return dsreturn;
                }
                if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                    return dsreturn;
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到平台动态参数信息！";
                return dsreturn;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
            return dsreturn;
        }
        #endregion
        #region//3.验证交易账户是否开通
        re = IPC.Call("交易账户开通状态", new object[] { DLYX });
        if (re[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(re[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                return dsreturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
            return dsreturn;
        }
        #endregion
        #region//4.验证是否休眠
        DataRow JBXX = null;
        re = IPC.Call("用户基本信息", new object[] { DLYX });
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                JBXX = ((DataSet)re[1]).Tables[0].Rows[0];
                if (JBXX["是否休眠"].ToString() == "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                    return dsreturn;
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                return dsreturn;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
            return dsreturn;
        }
        #endregion

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        #region//5.验证是否异常投标单
        Hashtable htCS = new Hashtable();
        htCS["@HTBH"] = HTBH;
        string strSQL = "select JYGLBSHZT,JYGLBSHZT from AAA_TBZLSHB where TBDH=(select T_YSTBDBH from AAA_ZBDBXXB where Z_HTBH=@HTBH)";
        Hashtable returnHT = I_DBL.RunParam_SQL(strSQL, "", htCS);
        if (!(bool)returnHT["return_float"])
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = (string)returnHT["return_errmsg"];
            return dsreturn;
        }
        if (returnHT["return_ds"] == null || ((DataSet)returnHT["return_ds"]).Tables.Count < 1 || ((DataSet)returnHT["return_ds"]).Tables[0].Rows.Count < 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到中标定标信息表信息！";
            return dsreturn;
        }
        DataSet dsZLSHB = (DataSet)returnHT["return_ds"]; 
        if (dsZLSHB != null && dsZLSHB.Tables[0].Rows.Count > 0)
        {
            if (dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "未审核" || dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                return dsreturn;
            }
        }
        #endregion

        string BuyJSBH = dt.Rows[0]["买家角色编号"].ToString();
        //====加锁(个别必要的业务才加)
        if (IPC.Call("用户锁定", new object[] { BuyJSBH })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);

        re =IPC.Call("用户上传证明文件", new object[] {dt });
        if (re[0].ToString() == "ok")
        {
            dsreturn = (DataSet)re[1];
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
        }

        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { BuyJSBH });


        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }
    /// <summary>
    /// 争议文件确认  zhouli 2014.07.08  add
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>返回符合dareturn结构的Dataset</returns>
    [WebMethod(MessageName = "C对方确认", Description = "返回dataset内含表明为“docinfo”的Datatable，null:程序出错！人工清盘时买卖家确认。")]
    public DataSet DWConfirm(DataTable dt, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        #region//1.验证传递的参数是否为空
        if (dt == null || dt.Rows.Count < 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
            return dsreturn;
        }
        #endregion

        string DLYX = dt.Rows[0]["dlyx"].ToString();
        string HTBH = dt.Rows[0]["htbh"].ToString();

        #region//2.验证是否在服务时间内
        object[] re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                DataRow JCYZ = ((DataSet)re[1]).Tables[0].Rows[0];
                if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                    return dsreturn;
                }
                if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(工作时间)，暂停交易！";
                    return dsreturn;
                }
                if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                    return dsreturn;
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到平台动态参数信息！";
                return dsreturn;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
            return dsreturn;
        }
        #endregion
        #region//3.验证交易账户是否开通
        re = IPC.Call("交易账户开通状态", new object[] { DLYX });
        if (re[0].ToString() == "ok")
        {
            DataSet dsjszh = (DataSet)(re[1]);
            if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
            {
                if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                    }
                    else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                    }
                    return dsreturn;

                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                return dsreturn;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
            return dsreturn;
        }
        #endregion
        #region//4.验证是否休眠
        DataRow JBXX = null;
        re = IPC.Call("用户基本信息", new object[] { DLYX });
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                JBXX = ((DataSet)re[1]).Tables[0].Rows[0];
                if (JBXX["是否休眠"].ToString() == "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                    return dsreturn;
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                return dsreturn;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
            return dsreturn;
        }
        #endregion

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        #region//5.验证是否异常投标单
        Hashtable htCS = new Hashtable();
        htCS["@HTBH"] = HTBH;
        string strSQL = "select JYGLBSHZT,JYGLBSHZT from AAA_TBZLSHB where TBDH=(select T_YSTBDBH from AAA_ZBDBXXB where Z_HTBH=@HTBH)";
        Hashtable returnHT = I_DBL.RunParam_SQL(strSQL, "", htCS);
        if (!(bool)returnHT["return_float"])
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = (string)returnHT["return_errmsg"];
            return dsreturn;
        }
        if (returnHT["return_ds"] == null || ((DataSet)returnHT["return_ds"]).Tables.Count < 1 || ((DataSet)returnHT["return_ds"]).Tables[0].Rows.Count < 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到中标定标信息表信息！";
            return dsreturn;
        }
        DataSet dsZLSHB = (DataSet)returnHT["return_ds"];
        if (dsZLSHB != null && dsZLSHB.Tables[0].Rows.Count > 0)
        {
            if (dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "未审核" || dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                return dsreturn;
            }
        }
        #endregion

        string BuyJSBH = dt.Rows[0]["买家角色编号"].ToString();
        //====加锁(个别必要的业务才加)
        if (IPC.Call("用户锁定", new object[] { BuyJSBH })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);

        re = IPC.Call("对方确认", new object[] { dt });
        if (re[0].ToString() == "ok")
        {
            dsreturn = (DataSet)re[1];
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
        }

        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { BuyJSBH });


        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }


    /// <summary>
    /// 获取用户账户的最新状态，用于验证 zhouli 2014.07.11 add--查询方法，无需加锁
    /// </summary>
    /// <param name="dt">一行数据，两列【"卖方邮箱","买方邮箱"】</param>
    /// <param name="SPstring"></param>
    /// <returns>返回DataSet【有多个DataTable:"返回值单条","卖方用户基本信息","买方用户基本信息"】</returns>
    [WebMethod(MessageName = "C用户基本信息", Description = "获取用户账户的最新状态，用于验证")]
    public DataSet GetNewUserState(DataTable dt, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { dt }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        #region//1.验证传递的参数是否为空
        if (dt == null || dt.Rows.Count < 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
            return dsreturn;
        }
        #endregion

        #region//2.查询数据
        foreach (DataColumn column in dt.Columns)
        {
            string DLYX = dt.Rows[0][column.ColumnName].ToString();
            object[] re = IPC.Call("用户基本信息", new object[] { DLYX });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    DataTable table = ((DataSet)re[1]).Tables[0].Copy();
                    table.TableName = column.ColumnName.Substring(0, 2) + "用户基本信息";
                    dsreturn.Tables.Add(table);
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
                    
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
        }
        #endregion
        return dsreturn;
    }

    /// <summary>
    /// 获取中标定标信息表详细信息 zhouli 2014.07.14 add--查询方法，无需加锁
    /// </summary>
    /// <param name="number">中标定标信息表Number</param>
    /// <returns>DataSet[两个DataTable:“返回值单条”、“HTPXX”]</returns>
    [WebMethod(MessageName = "C清盘C区获取中标定标表的信息", Description = "商品买卖C区--清盘 返回dataset，其中包含表名为'返回值单条','HTPXX'的Datatable")]
    public DataSet DBZBXXinfoget(string number, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { number }) && IsMD5check())
        {
            return null;
        }
        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);
        object[] re = IPC.Call("清盘C区获取中标定标表的信息", new object[] { number });
        if (re[0].ToString() == "ok")
        {
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                return (DataSet)re[1];
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
    /// <summary>
    /// 获取定标保证函详细信息 zhouli 2014.07.14 add--查询方法，无需加锁
    /// </summary>
    /// <param name="Number">中标定标信息表Number</param>
    /// <returns>DataSet[两个DataTable:“返回值单条”、“SPXX”]</returns>
    [WebMethod(MessageName = "C清盘C区定标保证函详细信息", Description = "商品买卖C区--清盘 返回dataset，其中包含表名为'返回值单条','SPXX'的Datatable")]
    public DataSet TBDYDDbzh_GetCGinfo(string number, string SPstring)
    {
        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { number }) && IsMD5check())
        {
            return null;
        }
        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);
        object[] re = IPC.Call("清盘C区定标保证函详细信息", new object[] { number });
        if (re[0].ToString() == "ok")
        {
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                return (DataSet)re[1];
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
    #endregion

    #endregion

    #region//货物收发--zhouli add 2014.07.03

    #region//获取买家最近一次发票信息--查询方法，无需加锁
    /// <summary>
    /// 获取买家最近一次发票信息--zhouli 2014.07.01 add
    /// </summary>
    /// <param name="DLYX">登陆邮箱</param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C获取买家最近一次发票信息", Description = "下达提货单时需获取发票信息")]
    public DataSet GetTHDFP(string DLYX, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { DLYX }) && IsMD5check())
        {
            return null;
        }

        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        
        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);
        if (string.IsNullOrEmpty(DLYX))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
            return dsreturn;
        }

        object[] re = IPC.Call("最近一次发票信息", new object[] { DLYX });
        if (re[0].ToString() == "ok")
        {
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {                
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取发票信息成功！";
                DataTable tb = ((DataSet)re[1]).Tables[0].Copy();
                tb.TableName = "主表";
                dsreturn.Tables.Add(tb);
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取发票信息失败！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
        }

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }
    #endregion

    #region//提货单查看--查询方法，无需加锁
    /// <summary>
    /// 提货单详情--zhouli 2014.07.03 add
    /// </summary>
    /// <param name="THDBH">提货单编号（去掉‘T’）</param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C提货单详情", Description = "提货单详情")]
    public DataSet CKTHD(string THDBH, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { THDBH }) && IsMD5check())
        {
            return null;
        }

        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //====调用其他类里的方法，正式进行处理
        if (string.IsNullOrEmpty(THDBH))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
            return dsreturn;
        }

        object[] re = IPC.Call("提货单信息查看", new object[] { THDBH });
        if (re[0].ToString() == "ok")
        {
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                dsreturn = (DataSet)re[1];
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询失败！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
        }

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }

    #endregion

    #region//发货单查看--查询方法，无需加锁
    /// <summary>
    /// 发货单详情--zhouli 2014.07.03 add
    /// </summary>
    /// <param name="FHDBH">发货单编号（去掉‘F’）</param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C发货单详情", Description = "发货单详情")]
    public DataSet CKFHD(string FHDBH, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { FHDBH }) && IsMD5check())
        {
            return null;
        }

        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //====调用其他类里的方法，正式进行处理
        if (string.IsNullOrEmpty(FHDBH))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
            return dsreturn;
        }

        object[] re = IPC.Call("发货单信息查看", new object[] { FHDBH });
        if (re[0].ToString() == "ok")
        {
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                dsreturn = (DataSet)re[1];
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询失败！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
        }

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }

    #endregion

    #region//下达提货单
    /// <summary>
    /// 下达提货单--zhouli 2014.07.01 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C下达提货单", Description = "下达提货单")]
    public DataSet SaveTHD(DataSet ds, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


        string DLYX = ds.Tables[0].Rows[0]["登陆邮箱"].ToString();
        //====加锁(个别必要的业务才加)
        if (IPC.Call("用户锁定", new object[] { DLYX })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);
        dsreturn = HWSF.SaveTHD(ds, dsreturn);

        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { DLYX });


        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }
    #endregion

    #region//生成发货单
    /// <summary>
    /// 生成发货单--zhouli 2014.07.02 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C生成发货单", Description = "生成发货单")]
    public DataSet SCFHD(DataSet ds, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


        string SelJSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();
        //====加锁(个别必要的业务才加)
        if (IPC.Call("用户锁定", new object[] { SelJSBH })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        //====调用其他类里的方法，正式进行处理
        //dsreturn = 类.处理方法(参数);
        dsreturn = HWSF.SCFHD(ds, dsreturn);

        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { SelJSBH });


        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }
    #endregion

    #region//录入发货信息、发票信息
    /// <summary>
    /// 录入发货信息--zhouli 2014.07.03 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C录入发货信息", Description = "录入发货信息")]
    public DataSet LRFHXX(DataSet ds, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        string SelJSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();
        //====加锁(个别必要的业务才加)
        if (IPC.Call("用户锁定", new object[] { SelJSBH })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        //====调用其他类里的方法，正式进行处理
        dsreturn = HWSF.LRFHXX(ds, dsreturn);

        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { SelJSBH });

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }
    /// <summary>
    /// 录入发票信息--zhouli 2014.07.03 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C录入发票信息", Description = "录入发票信息")]
    public DataSet LRFPXX(DataSet ds, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        string SelJSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();
        //====加锁(个别必要的业务才加)
        if (IPC.Call("用户锁定", new object[] { SelJSBH })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        //====调用其他类里的方法，正式进行处理
        dsreturn = HWSF.LRFPXX(ds, dsreturn);

        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { SelJSBH });

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }
    #endregion

    #region//提请买家签收
    /// <summary>
    /// 提请买家签收--zhouli 2014.07.03 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C提请买家签收", Description = "提请买家签收")]
    public DataSet InsertSQL(DataSet ds, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        string SelJSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();
        //====加锁(个别必要的业务才加)
        if (IPC.Call("用户锁定", new object[] { SelJSBH })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        //====调用其他类里的方法，正式进行处理
        dsreturn = HWSF.InsertSQL(ds, dsreturn);

        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { SelJSBH });

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }

    /// <summary>
    /// 提请买家签收--zhouli 2014.07.03 add
    /// </summary>
    /// <param name="number"></param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C是否已提请买家签收", Description = "货物收发--提请买家签收时，输入发货单编号后获取的信息")]
    public DataSet IsHavedQS(string number, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { number }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        if (string.IsNullOrEmpty(number))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空！";
            return dsreturn;
        }

        //====调用其他类里的方法，正式进行处理
       object[] re =  IPC.Call("是否已提请买家签收", new object[] { number });
        if (re[0].ToString() == "ok")
        {
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                dsreturn = (DataSet)re[1];
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询失败！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
        }

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }
    #endregion

    #region//货物签收
    /// <summary>
    /// 货物签收--zhouli 2014.07.03 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C货物签收", Description = "货物签收")]
    public DataSet Jhjx_hwqs(DataTable ds, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        string DLYX = ds.Rows[0]["登陆邮箱"].ToString();
        //====加锁(个别必要的业务才加)
        if (IPC.Call("用户锁定", new object[] { DLYX })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        //====调用其他类里的方法，正式进行处理
        dsreturn = HWSF.WYYQS(ds, dsreturn);

        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { DLYX });

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }

    #endregion

    #region//问题与处理
    /// <summary>
    /// 问题与处理--zhouli 2014.07.08 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="SPstring"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "C问题与处理", Description = "问题与处理")]
    public DataSet WTYCL(DataSet ds, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        string BuyJSBH = ds.Tables[0].Rows[0]["买家角色编号"].ToString();
        //====加锁(个别必要的业务才加)
        if (IPC.Call("用户锁定", new object[] { BuyJSBH })[1].ToString() != "1")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
            return dsreturn;
        }

        //====调用其他类里的方法，正式进行处理
        dsreturn = HWSF.WTYCL(ds, dsreturn);

        //====解锁(与加锁成对出现)
        IPC.Call("用户解锁", new object[] { BuyJSBH });

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }

    /// <summary>
    /// 获取异常信息
    /// </summary>
    /// <param name="ht">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset，内含有表明为：info的Datatable ;null:程序出现异常</returns>
    [WebMethod(MessageName = "C查询问题与处理", Description = "返回符合dsreturn结构Dataset,null:程序出现错误。")]
    public DataSet Getyiwaiinfo(DataTable ds, string SPstring)
    {
        //====开始防篡改串检查,除SPstring外，都放入下面的数组
        if (SPstring != StringOP.GetMD5(new object[] { ds }) && IsMD5check())
        {
            return null;
        }
        //====初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        if (ds==null||ds.Rows.Count<1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
            return dsreturn;
        }

        object[] re = IPC.Call("查询问题与处理", new object[] { ds });
        if (re[0].ToString() == "ok")
        {
            if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
            {
                dsreturn = (DataSet)re[1];
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据查找出错！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
        }

        //====返回处理结果(一定要在解锁之后返回)
        return dsreturn;
    }
    #endregion

    #endregion

    #endregion

    #region 测试demo wyh 2014.07.11
    /// <summary>
    /// 测试返回值标准
    /// </summary>
    /// <returns></returns>
    [WebMethod(MessageName = "C测试", Description = "原始版本的demotest")]
    public DataSet demotest(string test, string SPstring)
    {

        //开始防篡改串检查
        if (SPstring != StringOP.GetMD5(new object[] { test }) && IsMD5check())
        {
            return null;
        }
        //结束检查

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        //进行处理....

        //获得结果
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的[" + test + "]单号操作成功！";

        //附加数据表


        return dsreturn;

    }
    #endregion

}