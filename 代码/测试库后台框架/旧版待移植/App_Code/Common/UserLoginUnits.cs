using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///UserLoginUnits 的摘要说明
/// </summary>
public class UserLoginUnits
{
	public UserLoginUnits()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    #region //Cookie信息
    #region 用户1个小时之内自动登录登录
    /// <summary>
    /// 保存用户Cookie信息。
    /// </summary>
    /// <param name="cookie_value">要存入Cookie的值,要存入Cookie的密码</param>
    public static void UserAutoLoginCookie(string userName, string userPwd)
    {
        HttpCookie userCookie = new HttpCookie("UserLogin_FWPTZS");
        if (HttpContext.Current.Request.Cookies["UserLogin"] != null)
        {
            userCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(userCookie);
        }
        userCookie["UserAccount_FWPTZS"] = userName; //用户名
        userCookie["PwdKey_FWPTZS"] = userPwd;//密码
        userCookie["UserPwd_FWPTZS"] = Entry.EntryPassword(userPwd, 0, 20);//保存加密后的密码
        HttpContext.Current.Response.Cookies.Add(userCookie);
    }
    #endregion

    #region 清空用户自动登录Cookie信息。
    /// <summary>
    /// 用户两周内自动登录，清空Cookie保存的信息。
    /// </summary>
    public static void UserAutoLoginClearCookie()
    {
        HttpCookie userCookie = new HttpCookie("UserLogin");
        if (HttpContext.Current.Request.Cookies["UserLogin"] != null)
        {
            userCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(userCookie);
        }

    }
    #endregion

    #region 判断用户是否自动登录
    /// <summary>
    /// 判断用户是否自动登录
    /// </summary>
    public static bool IsUserAutoLogin(System.Web.UI.Page page)
    {
        //成功
        if (HttpContext.Current.Request.Cookies["UserLogin_FWPTZS"] != null && HttpContext.Current.Request.Cookies["UserLogin_FWPTZS"].Values["UserPwd_FWPTZS"] == Entry.EntryPassword(HttpContext.Current.Request.Cookies["UserLogin_FWPTZS"].Values["PwdKey_FWPTZS"], 0, 20))
        {
            return true;
        }
        else
        {
            HttpContext.Current.Response.Redirect("WebLogin.aspx");
            return false;
        }

    }
    #endregion
    #endregion

    #region //Session信息
    #endregion
}