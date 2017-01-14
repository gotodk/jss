using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// gets 的摘要说明
/// </summary>
public class gets
{
	public gets()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public static string getsf(string sf)
    {
        if (sf.Contains("市"))
            return "";
        else
            return sf;
    }
}
