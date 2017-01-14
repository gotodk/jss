using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mywork_getsymain : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string p = Request["p"].ToString();
        try
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Headers.Set("User-Agent", "Microsoft Internet Explorer");//增加的代码

            Stream resStream = wc.OpenRead(Request.Url.AbsoluteUri.Replace("getsymain.aspx", p + ".aspx"));
            
            StreamReader sr = new StreamReader(resStream, System.Text.Encoding.Default);
            string htmlstr = sr.ReadToEnd();
            resStream.Close();
            
            HTMLAnalyzeClass HAC = new HTMLAnalyzeClass();
            ArrayList al = new ArrayList();
            al = HAC.My_Cut_Str(htmlstr, "<jieduan>", "</jieduan>", 1, true);
            Response.Clear();
            Response.Write("document.write(\"" + al[0].ToString().Replace("\"", "'").Replace("\n", "").Replace("\r", "") + "\")");
        }
        catch(Exception ex){
            Response.Clear();
            Response.Write("");
    
        }
    }
}