using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pingtaiservices_moban_getpdfHT : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //通过中标定标信息表编号，找到对应pdf的话，就直接跳转到pdf页面。没有找到，就生成一个，然后跳转到pdf页面
        if (Request.Params["Number"] != null && Request.Params["Number"].ToString() != "")
        {
            string number = Request.Params["Number"].ToString();
            string lujing = "/HTpdf/HT" + number + ".pdf";
            if (File.Exists(Server.MapPath(lujing)))
            {
                //存在文件
                Response.Redirect(lujing, true);
            }
            else
            {
                //如果不存在文件，生成一个，然后转向

                //生成合同的pdf格式
                //执行wkhtmltopdf.exe 
                Process MyProcess = new Process();
                MyProcess.StartInfo.FileName = Server.MapPath("wkhtmltopdf.exe");
                string sp = " --footer-right 第[page]页,共[topage]页 ";
                string url = Request.Url.GetLeftPart(UriPartial.Path).Replace("getpdfHT.aspx", "") + "FMPTDZGHHT.aspx?Number=" + number;
                string savepathto = Server.MapPath("/HTpdf/HT") + number + ".pdf";
                MyProcess.StartInfo.Arguments = sp + " " + url + " " + savepathto;
                MyProcess.StartInfo.CreateNoWindow = true;
                MyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                MyProcess.Start();
                MyProcess.WaitForExit();

                Response.Redirect(lujing, true);
        
            }
        }

    }
}