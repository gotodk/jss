using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Key;
using Hesion.Brick.Core;
public partial class Web_picView_picView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url=Request["path"].ToString();

        //jiami用来表示传递的参数是否经过加密，如果是y表示是加密过的,否则全部认为没有加密
        if (Request["jiami"] != null && Request["jiami"].ToString() == "y")
        {
            url = keyEncryption.uncMe(Request["path"].ToString(), "mimamima");           
        }        
        //判断文件是否存在，不存在给出提示。
        if (url.ToLower().IndexOf("http://") == 0)
        {//这是判断网络文件的
            if (RemoteFileExists(url) == false)
            {
                MessageBox.ShowAlertAndBack(this, "文件不存在！"); 
            }
        }
        else
        {
            //这个地方是判断业务平台上传的文件的。
            string lj = Server.MapPath(url);
            if (!System.IO.File.Exists(lj))
            {
                MessageBox.ShowAlertAndBack(this, "文件不存在！"); 
            }
        }
        string extname = Path.GetExtension(url);
        if (extname != ".png" && extname != ".jpg" && extname != ".jpeg" && extname != ".gif" && extname != ".bmp")
        {
            Response.Redirect(url);            
        }
        idSrc.Value = url;
    }

    private bool RemoteFileExists(string fileUrl)
    {
        bool result = false;//判断结果

        System.Net.WebResponse response = null;
        try
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(fileUrl);
            response = req.GetResponse();
            result = response == null ? false : true;
        }
        catch (Exception ex)
        {
            result = false;
        }
        finally
        {
            if (response != null)
            {
                response.Close();
            }
        }
        return result;
    }

}