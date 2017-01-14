using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;

/// <summary>
/// 平安银行参数获取以及常用帮助类
/// </summary>
public class ParamHelper
{
	public ParamHelper()
	{

	}
    /// <summary>
    /// 获取AppSetting配置节点中的值
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public string GetConfigValue(string key)
    {
        return ConfigurationManager.AppSettings[key].ToString().Trim();
    }

    /// <summary>
    /// 获取配置IP地址
    /// </summary>
    /// <returns></returns>
    public string GetIp()
    {
        try
        {
            return ConfigurationManager.AppSettings["IP"].ToString().Trim();
        }
        catch (Exception ex)
        {
            //记录日志并抛出异常
            LogHelper.Log_Wirte("Logs", ex.Message, "ParamHelper.GetIp", false, true);
            throw ex;
        }
    }

    /// <summary>
    /// 获取配置IP地址
    /// </summary>
    /// <param name="AppSetStr">显式指定Key(AppSetting.Key)</param>
    /// <returns></returns>
    public string GetIp(string AppSetStr)
    {
        try
        {
            return ConfigurationManager.AppSettings[AppSetStr].ToString().Trim();
        }
        catch (Exception ex)
        {
            //记录日志并抛出异常
            LogHelper.Log_Wirte("Logs", ex.Message, "ParamHelper.GetIp", false, true);
            throw ex;
        }
    }

    /// <summary>
    /// 获取配置端口号
    /// </summary>
    /// <returns></returns>
    public string GetPort()
    {
        try
        {
            return ConfigurationManager.AppSettings["Port"].ToString().Trim();
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("Logs", ex.Message, "ParamHelper.GetPort", false, true);
            throw ex;
        }
    }

    /// <summary>
    /// 获取配置端口号
    /// </summary>
    /// <param name="AppSetStr">显式指定Key(AppSetting.Key)</param>
    /// <returns></returns>
    public string GetPort(string AppSetStr)
    {
        try
        {
            return ConfigurationManager.AppSettings[AppSetStr].ToString().Trim();
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("Logs", ex.Message, "ParamHelper.GetPort", false, true);
            throw ex;
        }
    }

    /// <summary>
    /// 获取IP地址+端口号字符串（格式为："192.168.1.1:80"）
    /// </summary>
    /// <returns></returns>
    public string GetIpAndPortStr()
    {
        try
        {
            return ConfigurationManager.AppSettings["IP"].ToString().Trim() + ":" + ConfigurationManager.AppSettings["Port"].ToString().Trim();
        }
        catch(Exception ex)
        {
            LogHelper.Log_Wirte("Logs", ex.Message, "ParamHelper.GetPort", false, true);
            throw ex;
        }
    }

    /// <summary>
    /// 获取IP地址+端口号字符串（格式为："192.168.1.1:80"）
    /// </summary>
    /// <param name="IPKey">显式指定ip address key</param>
    /// <param name="PortKey">显式指定port key</param>
    /// <returns></returns>
    public string GetIpAndPortStr(string IPKey,string PortKey)
    {
        try
        {
            return ConfigurationManager.AppSettings["IP"].ToString().Trim() + ":" + ConfigurationManager.AppSettings["Port"].ToString().Trim();
        }
        catch (Exception ex)
        {
            LogHelper.Log_Wirte("Logs", ex.Message, "ParamHelper.GetPort", false, true);
            throw ex;
        }
    }

    /// <summary>
    /// 判断字符串是否为Null，如果为Null，则返回""，否则返回字符串本身（不进行任何处理）
    /// </summary>
    /// <param name="str">要处理的字符串</param>
    /// <returns></returns>
    public string IsNullOrEmpty(string str)
    {
        return str == null ? "" : str;
    }

    /// <summary>
    /// 获取最新流水号
    /// </summary>
    /// <returns>string</returns>
    public string GetLSH()
    {
        return "12345";
    }

}