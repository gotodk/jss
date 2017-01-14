<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.IO;
public class Handler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Charset = "utf-8";
        HttpPostedFile file = context.Request.Files["Filedata"];//得到数据

        string uploadPath = HttpContext.Current.Server.MapPath(@context.Request["folder"]) + "\\";//得到路径 还需要创建日期文件夹。
        // string  upext = "txt,rar,zip,jpg,jpeg,gif,png,swf,wmv,avi,wma,mp3,mid,doc,xls,ppt"; // 上传扩展名
        // string extension=GetFileExt(file.FileName);
        //if (("," + upext + ",").IndexOf("," + extension + ",") < 0)
        //{
        //    err = "上传文件扩展名必需为：" + upext;
        //}
        //else
        //{
        if (file != null)
        {

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            string result = uploadPath + file.FileName;
            file.SaveAs(result);

            //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失    
            //必须要有输出 作为 AJAX 的回调函数的参数。否则不响应 成功后的操作。
            //一般用于传递 JSON数据，传递需要的当前文件的URL路径。用于数据库保存。
            context.Response.Write(result);

            //}
            //else
            //{
            //    context.Response.Write("0");
            //}
        }



    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }


    ///// <summary>
    ///// 获取文件扩展名
    ///// </summary>
    ///// <param name="FullPath">完整路径</param>
    ///// <returns></returns>
    //string GetFileExt(string FullPath)
    //{
    //    if (FullPath != "")
    //    {
    //        return FullPath.Substring(FullPath.LastIndexOf('.') + 1).ToLower();
    //    }
    //    else
    //    {
    //        return "";
    //    }
    //}

}