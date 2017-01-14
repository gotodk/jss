using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using System.Data.SqlClient;
using Hesion.Brick.Core;
using Key;
using System.Collections.Generic;
using Hesion.Brick.Core.WorkFlow;

/// <summary>
///fwzd_fwsClass 的摘要说明
/// </summary>
public class fwzd_fwsClass
{
    string khbh = "";
    string fwbh = "";
	public fwzd_fwsClass()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
/// <summary>
/// 返回用户所关联服务商信息
/// </summary>
/// <param name="khbh">用户或推广人员编号</param>
/// <returns></returns>
    public Hashtable GetfwzdInfo(string khbh)
    {
        string fwzdmc = "";
        string fwzdbh="";
        string sql = "select FWZDBH,fwzdmc from  FM_FWZDYTGYHGLB where  yxx='是' and GLKHBH='" + khbh + "' ";
       SqlDataReader reader1 = DbHelperSQL.ExecuteReader(sql);
       Hashtable ht = new Hashtable();
       if (reader1.Read())
       {
           fwzdbh = reader1["fwzdbh"].ToString();
           fwzdmc = reader1["fwzdmc"].ToString();
       
       }

        ht.Add("服务站点编号", fwzdbh);    //用户所关联服务站点编号
        ht.Add("服务站点名称", fwzdmc);
        reader1.Close();
        return ht;


    }

    /// <summary>
    /// 返回服务商关联服务站点其中一方的信息
    /// </summary>
    /// <param name="khbh">服务商或服务站点编号，调用时输入一方编号便可，另一方为空</param>
    /// <returns></returns>
    public Hashtable GetfwzdfwsInfo(string fwsbhcd, string fwzdbhcd)
    {
        string fwsmc = "";
        string fwsbh = "";
        string fwzdmc = "";
        string fwzdbh = "";
        string sql = "select fwsbh,fwsmc,fwzdmc,fwzdbh  from FM_FWSFWZDGL where sfyx='有效' and fwsbh like '%" + fwsbhcd + "%' and fwzdbh like '" + fwzdbhcd + "'";
        SqlDataReader reader1 = DbHelperSQL.ExecuteReader(sql);
        if (reader1.Read())
        {
            fwsbh = reader1["fwsbh"].ToString();
            fwsmc = reader1["fwsmc"].ToString();
            fwzdbh=reader1["fwzdbh"].ToString();
            fwzdmc = reader1["fwzdmc"].ToString();

        }

        Hashtable ht = new Hashtable();
        ht.Add("服务商编号", fwsbh);
        ht.Add("服务商名称", fwsmc);
        ht.Add("服务站点编号", fwzdbh);
        ht.Add("服务站点名称", fwzdmc);  
        reader1.Close();
        return ht;
    
    }
   
}