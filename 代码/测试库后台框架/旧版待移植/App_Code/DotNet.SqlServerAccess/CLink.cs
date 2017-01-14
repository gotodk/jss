using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;

/// <summary>
/// 用于数据库字符串的链接和返回数据库类型
/// </summary>
public class CLink
{
	public CLink()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 获取数据库连接字符串
    /// </summary>
    //public static readonly string Source = Application.StartupPath.ToString() + ConfigurationManager.AppSettings["DBLink"];
    //public static readonly string Source = ConfigurationManager.AppSettings["DBLink"].ToString();
    //public static readonly string Source = ConfigurationManager.ConnectionStrings["DbLink"].ToString();
    public static readonly string Source = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\信息化中心工作记录\\最新文档打包\\ipHP\\ip.mdb;";

    /// <summary>
    /// 默认构造函数
    /// </summary>
    /// <returns></returns>
    public static short Type()
    {
        return Type(0);
    }
    /// <summary>
    /// 判断当前使用的数据库
    /// </summary>
    /// <param name="ConnIndex">0表示Access数据库，1表示SQL Server数据库。</param>
    /// <returns>返回0或1</returns>
    public static short Type(int ConnIndex)
    {
        string source = Source;
        if (source.IndexOf("|") > -1)
        {
            source = source.Split(new char[] { '|' })[ConnIndex];
        }
        if (source.Replace(" ", "").ToUpper().IndexOf(";SERVER=") > -1)
        {
            return 1;
        }
        return 0;
    }
}