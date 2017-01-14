using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;

[WebService(Namespace = "http://system.ipc.com/", Description = "V1.00->系统控制中心业务类接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class system : System.Web.Services.WebService
{
    public system () {

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
    /// 用户锁定，针对一个用户进行的业务锁定，调用此锁的地方，只能同时处理此用户一次业务。只有返回1，才能继续进行业务处理，否则取消业务处理
    /// </summary>
    /// <param name="khbh">客户编号</param>
    /// <returns>只有返回1，才能继续进行业务处理，否则取消业务处理，提示系统忙。0代表发生意外。2代表强制解锁了，也必须取消业务处理。</returns>
    [WebMethod(MessageName = "用户锁定", Description = "后台操作-进行更删改查操作时用户锁定")]
    public string LockBegin_ByUser(string khbh)
    {
        string state = jhjx_Lock.LockBegin_ByUser(khbh);

        return state;
    }


    /// <summary>
    /// 解除用户锁定,与LockBegin_ByUser配对
    /// </summary>
    /// <param name="khbh">客户编号</param>
    /// <returns>这个返回值没什么用</returns>
     [WebMethod(MessageName = "用户解锁", Description = "后台操作-进行更删改查操作时用户解锁")]
    public  string LockEnd_ByUser(string khbh)
    {
        string state = jhjx_Lock.LockEnd_ByUser(khbh);

        return state;
    }


    #region  测试方法可用性代码
    //[WebMethod(Description = "用户解锁")]
     //public string  testlock()
     //{
     //     return jhjx_Lock.LockBegin_ByUser("全局锁");
     //}

     //[WebMethod(Description = "用户枷锁")]
     //public string UNLOCK()
     //{
     //    return jhjx_Lock.LockEnd_ByUser("全局锁");
    //}
    #endregion

     /// <summary>
     /// 发送提醒
     /// </summary>
     /// <param name="ds">1、必备字段：row["type"]={集合经销平台、业务平台、手机短信、邮箱提醒 [可同时包含这四种]；}、row["提醒对象登陆邮箱"]、row["提醒对象结算账户类型"]、row["提醒对象角色编号"]、row["提醒对象角色类型"]、 row["提醒内容文本"]、row["创建人"]、row["查看地址"]{可为空}、row["模提醒模块名"]；     
     /// </param>
     [WebMethod(MessageName = "发送提醒", Description = "后台操作-发送提醒")]
     public string Sendmes_XTKZ(DataSet ds)
     {
         string state = Index_XTKZ.Sendmes_XTKZ(ds);
         return state;
     }
    /// <summary>
    /// 通过模块名和开头标记字母，获得下一个编号
    /// </summary>
    /// <param name="ModuleName">模块名【表名】</param>
    /// <param name="HeadStr">开头标记字母</param>
    /// <returns></returns>
     [WebMethod(MessageName = "获取主键", Description = "后台操作-获取主键")]
     public string GetNextNumberZZ_XTKZ(string ModuleName, string HeadStr)
     {
         //通过sql数据库中的序列方式获取主键
         //string state = Index_XTKZ.GetNextNumberZZ_XTKZ(ModuleName, HeadStr);
         //通过redis方式获取主键
         string state = Index_XTKZ.GetNextNumberZZ_XTKZ_redis(ModuleName, HeadStr);
         return state;
     }

    /// <summary>
    /// 检查提醒
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <returns></returns>
     [WebMethod(MessageName = "检查提醒", Description = "大盘右下角-检查提醒")]
     public DataSet CheckFormTrayMsg_XTKZ(string dlyx)
     {
         DataSet state = Index_XTKZ.CheckFormTrayMsg_XTKZ(dlyx);
         return state;
     }

     /// <summary>
    /// 通过最新的配置动态参数信息
    /// </summary>
     /// <param name="cs">此参数无效，只是为了满足格式要求</param>
    /// <returns>带有信息的DataSet</returns>
     [WebMethod(MessageName = "平台动态参数", Description = "后台-平台动态参数")]
     public DataSet GetParameterInfo_XTKZ(string cs)
     {
         DataSet state = Index_XTKZ.GetParameterInfo_XTKZ();
         return state;
     }

    /// <summary>
    /// 是否在服务时间【同时在服务时间内(工作时)及在服务时间内(假期)】
    /// </summary>
    /// <param name="cs">此参数无效，只是为了满足格式要求</param>
     /// <returns>“是”、“否”、null</returns>
     [WebMethod(MessageName = "是否在服务时间", Description = "后台-是否在服务时间")]
     public string IsInServeTime_XTKZ(string cs)
     {
         string state = Index_XTKZ.IsInServeTime_XTKZ();
         return state;
     }
    /// <summary>
    /// 验证登陆账号是否允许登陆
    /// </summary>
    /// <param name="email">登陆邮箱</param>
    /// <returns></returns>
     [WebMethod(MessageName = "踢人", Description = "获得数据，用于判定当前软件是否允许使用，不允许的会被强制退出客户端。")]
     public string GetOpen_XTKZ(string email)
     {
         string state = XZDLYX.GetOpen(email); 
         return state;
     }

     /// <summary>
    /// 通过角色编号或证券资金账号获取账户各种频繁使用的相关信息
    /// </summary>
     /// <param name="bh">买家or卖家or经纪人角色编号or证券资金账号</param>
    /// <returns>带有信息的DataSet(包括了主要数据和完整数据集的两个DataTable)</returns>
    [WebMethod(MessageName = "获取账户的相关信息", Description = "后台-获取账户的相关信息")]
     public DataSet GetUserInfo_XTKZ(string bh)
     {
         try
         {
             return Index_XTKZ.GetUserInfo(bh);
         }
         catch(Exception)
         {
             return null;
         }
     }
}