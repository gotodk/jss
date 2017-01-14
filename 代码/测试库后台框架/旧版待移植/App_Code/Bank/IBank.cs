using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;
using FMOP.DB;

/// <summary>
/// 调用银行接口方法 | 
/// 系统会自动判定用户所属银行，从而调用相应的接口 | 
/// 2013 09 22 guotuo
/// </summary>
public class IBank
{
    /// <summary>
    /// 将要发生银行业务的用户邮箱
    /// </summary>
    private string uEmail = string.Empty;

    /// <summary>
    /// 将要调用的银行接口方法名
    /// </summary>
    private string methodName = string.Empty;

    /// <summary>
    /// 初始化列表
    /// </summary>
    private BankInit BI = null;

    /// <summary>
    /// 路由帮助类
    /// </summary>
    private BankRoute BR = new BankRoute();

    /// <summary>
    /// 默认构造函数
    /// </summary>
    /// <param name="uemail">将要发生银行业务的用户登录邮箱 </param>
    /// <param name="method">将要调用的方法名 </param>
    public IBank(string uemail, string method)
    {
        uEmail = uemail.Trim();
        methodName = Helper.Convert(method.Trim()).ToLower();
        if (method.IndexOf("银行") >= 0)
            methodName = methodName.Replace("yinxing", "yinhang");//多音字啊尼玛
        BI = BR.GetUserInformation(uemail);

        #region 判断接口开关
        if (BI != null)
        {
            DataTable Cfg = BankHelper.GetConfig(BI.Bank);
            if (Cfg != null && Cfg.Rows.Count > 0)
            {
                if (Cfg.Rows[0]["OnOff"].ToString() == "关")
                {
                    methodName += "_Result";
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 调用银行接口
    /// </summary>
    /// <param name="ds">参数表</param>
    /// <param name="InvokeInfo">调用结果指针</param>
    /// <returns>DataSet调用结果</returns>
    public DataSet Invoke(DataSet ds,ref string InvokeInfo)
    {
        try
        {
            //调用时已带有银行信息，并且跟本地记录不匹配。直接报错。
            if (InvokeInfo.IndexOf("发起行*") >= 0 && BI.Bank != InvokeInfo.Replace("发起行*",""))
            {
                InvokeInfo = "发起行与本地记录行不匹配";
                return null;
            }


            //wyh 2014.04.22 add 防止用户名为Null异常
            if (BI == null)
            {
                InvokeInfo = "用户账户无效或未关联银行业务";
                return null;
            }

            string bankTypes = BI.Routes;
            if (bankTypes != null)
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                Type tp = asm.GetType(bankTypes, false);
                object instance = asm.CreateInstance(bankTypes, false);
                MethodInfo method = tp.GetMethod(methodName);
                object[] _params = { ds, BI };
                DataSet dsReturn = (DataSet)method.Invoke(instance, _params);
                if (dsReturn != null)
                {
                    InvokeInfo = "ok";
                    return dsReturn;
                }
                else
                {
                    InvokeInfo = "与银行服务器通信超时或发生错误";
                    return null;
                }
            }
            else
            {
                InvokeInfo = "用户账户无效或未关联银行业务";
                return null;
            }
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            InvokeInfo = msg;
            return null;
        }
    }

    /// <summary>
    /// 调用银行接口（无执行状态指针）
    /// </summary>
    /// <param name="ds">参数表</param>
    /// <returns>DataSet调用结果</returns>
    public DataSet Invoke(DataSet ds)
    {

        try
        {
            //wyh 2014.04.22 add 防止用户名为Null异常
            if (BI == null)
            {
                return null;
            }


            string bankTypes = BI.Routes;
            if (bankTypes != null)
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                Type tp = asm.GetType(bankTypes, false);
                object instance = asm.CreateInstance(bankTypes, false);
                MethodInfo method = tp.GetMethod(methodName);
                object[] _params = { ds, BI };
                DataSet dsReturn = (DataSet)method.Invoke(instance, _params);
                if (dsReturn != null)
                    return dsReturn;
                else
                    return null;
            }
            else
                return null;
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            return null;
        }
    }
}