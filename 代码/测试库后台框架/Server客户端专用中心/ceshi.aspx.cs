using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using FMDBHelperClass;
using RedisReLoad.ShareDBcache;

public partial class ceshi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        byte[] Buffer = HomePageRedis.GetHomePageByteSSS("str:HomePageByte");
        byte[] Buffer2 = Helper.Decompress(Buffer);

        Response.ContentType = "application/octet-stream";
 
        Response.BinaryWrite(Buffer2);
    }
}