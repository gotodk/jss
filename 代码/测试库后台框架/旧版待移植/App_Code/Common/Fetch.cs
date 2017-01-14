using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///Fetch 的摘要说明
/// </summary>
public class Fetch
{
	public Fetch()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    #region 获取页面地址的参数值，相当于 Request.QueryString public static string Get(string name)
    /// <summary>
    /// 获取页面地址的参数值，相当于 Request.QueryString
    /// </summary>
    public static string Get(string name)
    {
        string value = HttpContext.Current.Request.QueryString[name];
        return value == null ? string.Empty : value.Trim();
    }
    #endregion
}