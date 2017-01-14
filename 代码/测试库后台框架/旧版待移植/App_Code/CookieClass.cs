using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;


    /// <summary> 
    /// Cookie操作类 
    /// </summary> 
    public class CookieClass
    {
        /// <summary> 
        /// 保存一个Cookie 
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        /// <param name="CookieValue">Cookie值</param> 
        /// <param name="CookieTime">Cookie过期时间(小时),0为关闭页面失效</param> 
        public static void SaveCookie(string CookieName, string CookieValue, double CookieTime)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            //myCookie.Domain = "fm8844.com";
            DateTime now = DateTime.Now;
            myCookie.Value = CookieValue;

            if (CookieTime != 0)
            {
                //有两种方法，第一方法设置Cookie时间的话，关闭浏览器不会自动清除Cookie 
                //第二方法不设置Cookie时间的话，关闭浏览器会自动清除Cookie ,但是有效期 
                //多久还未得到证实。                   ----有效期是在完全关闭创建Cookie的浏览器后失效
                myCookie.Expires = now.AddDays(CookieTime);
                if (HttpContext.Current.Response.Cookies[CookieName] != null)
                    HttpContext.Current.Response.Cookies.Remove(CookieName);

                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            else
            {
                if (HttpContext.Current.Response.Cookies[CookieName] != null)
                    HttpContext.Current.Response.Cookies.Remove(CookieName);

                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
        /// <summary> 
        /// 取得CookieValue 
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        /// <returns>Cookie的值</returns> 
        public static string GetCookie(string CookieName)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            //myCookie.Domain = "fm8844.com";
            myCookie = HttpContext.Current.Request.Cookies[CookieName];

            if (myCookie != null)
                return myCookie.Value;
            else
                return null;
        }
        /// <summary> 
        /// 清除CookieValue 
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        public static void ClearCookie(string CookieName)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            //myCookie.Domain = "fm8844.com";
            DateTime now = DateTime.Now;

            myCookie.Expires = now.AddYears(-2);

            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

}

