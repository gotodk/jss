using System;
using System.Collections.Generic;
using System.Web;
using FMOP.DB;

/// <summary>
///ISbsc 的摘要说明
/// </summary>
public class ISbsc
{
	public ISbsc()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

   public  static string isbsc( string StrID)
    {
        string istur = "";

        string sql = "select bm from hr_employees where number='" + StrID + "'";
        istur = DbHelperSQL.GetSingle(sql).ToString();
         
        return istur;
     
         
    }
}