/********************************************************************
 * 
 * 创建人：wyh
 *
 *创建时间：2014.06.27
 *
 *代码功能：获取商品基本信息
 *
 * 
 ********************************************************************/
using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// GetSPInfo 的摘要说明
/// </summary>
public class GetSPInfo
{
	public GetSPInfo()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    ///根据商品编号获取商品基本信息：商品编号，商品名称，规格，计价单位，最大经济批量
    /// </summary>
    /// <param name="SPnum">商品编号</param>
    /// <returns></returns>
    public DataTable dtSPInfo(string SPnum)
    {
        try
        {
            DataTable dt = null;
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable param = new Hashtable();
            param.Add("@SPBH", SPnum);

            string strsql = " select  SPBH,SPMC,GG,JJDW,JJPL from AAA_PTSPXXB where SPBH = @SPBH  ";
            Hashtable return_ht = I_DBL.RunParam_SQL(strsql, "spinfo", param);
            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                dt = ((DataSet)return_ht["return_ds"]).Tables["spinfo"];
            }
            else
                dt= null;
            return dt;
        }
        catch
        {
            return null;
        }

        
    }


    /// <summary>
    /// 获取经济批量值
    /// </summary>
    /// <param name="htqx"></param>
    /// <returns></returns>
    public string Getjjpl(string htqx,string spbh)
    {
        string jjpl ="";

        DataTable dt = null;
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();
        param.Add("@htqx", htqx);
        param.Add("@spbh", spbh);
        param.Add("@ZT", "竞标");

        string strjjpl = "SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE SPBH=@spbh AND ZT=@ZT AND HTQX=@htqx ORDER BY MJSDJJPL DESC";

      Hashtable return_ht = I_DBL.RunParam_SQL(strjjpl, "jjpl", param);
      if ((bool)(return_ht["return_float"])) //说明执行完成
      {
          dt = ((DataSet)return_ht["return_ds"]).Tables["jjpl"];
          if (dt != null && dt.Rows.Count > 0)
          {
              jjpl = dt.Rows[0]["MJSDJJPL"].ToString();
          }
      }
      

        return jjpl;
    }
}