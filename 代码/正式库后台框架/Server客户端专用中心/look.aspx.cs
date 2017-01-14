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
using System.Threading;
 

public partial class look : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string rehtml = "";
        rehtml = rehtml + "来访者IP：" + GetIP() + "<br/>";
        rehtml = rehtml + "负载机器IP：" + GetInterIDList("look.txt") + "<br/>";
        rehtml = rehtml + "Headers头部：" + Request.Headers.ToString() + "<br/>";

    
        MakeFile(Server.MapPath("log.txt"), rehtml);
         Thread.Sleep(1);

         Response.Write(rehtml);
            
    }

    private void MakeFile(string FileName, string Content) { System.IO.StreamWriter sw = new System.IO.StreamWriter(FileName, false, System.Text.Encoding.GetEncoding("gb2312")); try { sw.Write(Content); sw.Flush(); } finally { if (sw != null)  sw.Close(); } }// MakeFile

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