using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace AutoUpdate
{
    /// <summary>
    /// 配置文件下载类
    /// </summary>
    class ThreadDownLoad
    {
        string a_strUrl_TT = "";
        int timeout_TT = 0;
        string filepath_TT = "";
        bool keepname_TT = true;
        bool timepath_TT = true;
        CookieContainer myCookieContainer_TT;

        public ThreadDownLoad(string a_strUrl, int timeout, string filepath, bool keepname, bool timepath, CookieContainer myCookieContainer)
        {
            a_strUrl_TT = a_strUrl;
            timeout_TT = timeout;
            filepath_TT = filepath;
            keepname_TT = keepname;
            timepath_TT = timepath;
            myCookieContainer_TT = myCookieContainer;
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        public string BeginDown()
        {
            客户端主程序.Support.GetHttpClass GHC = new 客户端主程序.Support.GetHttpClass();
            CookieContainer CC = new CookieContainer();
           string s= GHC.Get_Img(a_strUrl_TT, timeout_TT, filepath_TT, keepname_TT, timepath_TT, myCookieContainer_TT);
           return s;
        }
    }
}
