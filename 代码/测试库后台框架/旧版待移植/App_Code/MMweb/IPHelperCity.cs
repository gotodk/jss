using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

/// <summary>
///（IPHelperCity ）根据IP查找城市
/// </summary>
public class IPHelperCity
{


	public IPHelperCity()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    //数据库连接字符串(web.config来配置)，
    private static string connectionString = ConfigurationManager.ConnectionStrings["FMOPConn"].ToString(); //如果要换数据库，你可以参考SysManage      
   

    /// <summary>
    /// 通过客户端的IP地址查询客户所在的地区（省|市）
    /// </summary>
    /// <param name="clientIP">客户端的IP</param>
    /// <returns>客户所在的省市格式“省|市”</returns>
    public static string GetAddressByClientIP(string clientIP)
    {
        //根据真实IP得到转换后的数字IP
        int numerIP = IPConvert(clientIP);
        DataSet ds = null;
        DataTable dt = null;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
                try
                {
                    ds = new DataSet();
                    dt = new DataTable();
                    connection.Open();
                    string strSqlString = "select IPProvince,IPCity from dbo.MM_FMQGCSIPSJK where " + numerIP + " between IPStart and IPEnd";
                    SqlDataAdapter command = new SqlDataAdapter(strSqlString, connection);
                   command.Fill(ds,"ds");
                   dt = ds.Tables[0];
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    connection.Close();
                    throw new Exception(E.Message);
                }
          
        }
        if (dt != null && dt.Rows.Count > 0)
        {
            return dt.Rows[0]["IPProvince"] + "|" + dt.Rows[0]["IPCity"];
        }
        else
        {
            return "";
        }
    
    }

    /// <summary>
    /// 客户端IP转换位数字IP
    /// </summary>
    /// <param name="cilentIP">客户端IP</param>
    /// <returns>转换后的数字IP地址</returns>
    private static int IPConvert(string clientIP)
    {
        //localhost 进行访问的,那么服务器端获得的也就是127.0.0.1，因为localhost是一个环路地址
        if (clientIP == "127.0.0.1")
        {
            //此时把IP地址转换为公司外网地址（联通地址"60.216.251.3"）公司外网电信地址"58.56.49.153"
            clientIP = "60.216.251.3";
        }
        //此时说明是公司内部局域网地址  网络中私有地址有三种 10.0.0.0--10.255.255.255,172.16.0.0--172.31.255.255,192.168.0.0--192.168.255.255
        //公司使用192.168的私有地址作为内部局域网地址 有一下几段 6,5,8,16
        else if (clientIP.IndexOf("192.168") == 0)
        {
            //此时把IP地址转换为公司外网地址（联通地址"60.216.251.3"）公司外网电信地址"58.56.49.153"
            clientIP = "60.216.251.3";
        }
        return IPConvertToInt(clientIP);
       
    }
    /// <summary>
    /// 把客户端IP根据算法转化为具体数字已进行地区查找
    /// </summary>
    /// <param name="clientIP">客户端IP</param>
    /// <returns>转换后的IP数字</returns>
    private static int IPConvertToInt(string clientIP)
    {
      string [] strClientIP=clientIP.Split('.');
        //这是具体算法将真实IP转换为数字IP  参考文章http://www.cnblogs.com/hanpu0725/archive/2009/08/29/1556207.html
      return Convert.ToInt32(strClientIP[0]) * 256 * 256 * 256 + Convert.ToInt32(strClientIP[1]) * 256 * 256 + Convert.ToInt32(strClientIP[2]) * 256 + Convert.ToInt32(strClientIP[3]);
    }
}