using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///PageUtils 的摘要说明
/// </summary>
public class PageUtils
{
	public PageUtils()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    //接受全选的参数
    public static string Post(string name)
    {
        string text = HttpContext.Current.Request.Form[name];
        if (text != null)
        {
            return text.Trim();
        }
        return string.Empty;
    }
}