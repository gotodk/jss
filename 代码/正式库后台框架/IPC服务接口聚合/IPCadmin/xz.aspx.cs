using FMipcClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_xz : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        object re = null;
        try
        {
            string IPCurl = ConfigurationManager.ConnectionStrings["IPCurl"].ToString(); //聚合中心地址
            string GX_shibie = Request["yewuming"].ToString();//用进程池名称作为标识
            FMWScenter wsd = new FMWScenter(IPCurl + "?wsdl");
            re = wsd.ExecuteQuery("GetGX_from_GX_shibie", new object[] { GX_shibie });

            if (re == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('获取失败，得到了null！');history.go(-1);</script>");
                return;
            }
            else
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();


                ((DataSet)re).WriteXml(ms, XmlWriteMode.WriteSchema);
                Response.Clear();
                // 下载附件的名字 
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(GX_shibie,System.Text.Encoding.UTF8) + "_IPClist.config");
                // 下载附件的大小，以便让浏览器显示进度条 
                Response.AddHeader("Content-Length", ms.Length.ToString());
                // 指定浏览器为下载模式 
                Response.ContentType = "application/octet-stream";
                // 发送到客户端 
                byte[] b = ms.ToArray();
                Response.OutputStream.Write(b, 0, b.Length);
                Response.End();


            }

        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('获取失败！'" + ex.ToString() + ");history.go(-1);</script>");
            return;
        }
    }
}