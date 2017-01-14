using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///Jscript 的摘要说明
/// </summary>
public class Jscript
{
    public static void Show(System.Web.UI.Page page, string message)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script type='text/javascript'> alert('" + message + "')</script>");
    }
}