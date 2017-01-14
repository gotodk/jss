using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
 
using FMDBHelperClass;
using RedisReLoad.ShareDBcache;

public partial class rediskeygogo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            string key = ( Request["key"] == null ||  Request["key"].ToString().Trim() == "") ? "" :  Request["key"].ToString().Trim();
            if (key == "")
            {
                return;
            }


            byte[] Buffer = HomePageRedis.GetHomePageByteSSS(key);
            if (Buffer == null)
            {

                 return;

            }
            else
            {
                Response.ContentType = "application/octet-stream";
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                 Response.BinaryWrite(Buffer);
            }



        }
        catch (Exception ex)
        {
             Response.ContentType = "text/html";
             Response.Clear();
             Response.ClearContent();
             Response.ClearHeaders();
             Response.Write(ex.ToString());
        }
    }
}