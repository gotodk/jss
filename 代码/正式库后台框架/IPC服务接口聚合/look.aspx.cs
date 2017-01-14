using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
 
using System.Text.RegularExpressions;
 

public partial class look : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
         Response.Write("来访者IP："+GetIP() + "<br/>");
         Response.Write("负载机器IP："+ GetInterIDList("look.txt") + "<br/>");
         Response.Write("Headers头部：" + Request.Headers.ToString() + "<br/>");
            
    }

  private string GetInterIDList(string strfile)
        {
            string strout;
            strout = "";
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(strfile)))
            {
            }
            else
            {
                StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(strfile), System.Text.Encoding.Default);
                String input = sr.ReadToEnd();
                sr.Close();
                strout = input;
            }
            return strout;
        }


    
        private  string GetIP() { string result = String.Empty; result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]; if (null == result || result == String.Empty) { result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; } if (null == result || result == String.Empty) { result = HttpContext.Current.Request.UserHostAddress; } if (null == result || result == String.Empty || !IsIP(result)) { return "0.0.0.0"; } return result; }


        private bool IsIP(string ip) { return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"); }
}