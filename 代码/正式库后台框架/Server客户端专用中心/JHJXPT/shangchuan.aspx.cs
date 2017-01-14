using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class JHJXPT_shangchuan : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.Files.Count > 0)
        {
            try
            {


                HttpPostedFile file = Request.Files[0];
                string nstr = DateTime.Now.ToString("yyyyMM");
                //生成保存路径
                string sfilepath = this.MapPath("SaveDir") + "\\" + nstr;
                ////生成新文件名
                //string nfilename = Guid.NewGuid().ToString();
                ////获得原扩展名
                //string extstr = new System.IO.FileInfo(file.FileName).Extension;
                //生成保存完整路径和文件名
                string filepath = sfilepath + "\\" + file.FileName;

                if (!Directory.Exists(sfilepath))//验证路径是否存在
                {
                    Directory.CreateDirectory(sfilepath);//不存在则创建
                }

                file.SaveAs(filepath);
                Response.Write("Success*" + nstr + "\\" + file.FileName + "\r\n");
                Response.End();
            }
            catch
            {
                Response.Write("Error*\r\n");
                Response.End();
            }

        }
    }
}