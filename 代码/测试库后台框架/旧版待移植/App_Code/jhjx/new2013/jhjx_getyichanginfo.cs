using FMOP.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// jhjx_getyichanginfo 的摘要说明
/// </summary>
public class jhjx_getyichanginfo
{
	public jhjx_getyichanginfo()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public DataSet Getyiwaiinfo(DataSet dsreturn, DataTable ht)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = " 没有查找任何数据！";
      
        DataRow dr = ht.Rows[0];
        string Strsql = " select " + dr["cloumn"].ToString() + ", (case  F_DQZT when '撤销'  then '同意重新发货' else replace(cast(F_DQZT as varchar(8000)),'家','方') end) as F_DQZT  from AAA_THDYFHDXXB where Number='" + dr["Number"].ToString().Trim() + "'";
        DataSet dsinfo = DbHelperSQL.Query(Strsql);
        if (dsinfo != null && dsinfo.Tables[0].Rows.Count == 1)
        {
            DataTable dt = dsinfo.Tables[0].Copy();
            dt.TableName = "info";
            dsreturn.Tables.Add(dt);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = " 找到数据！";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = " 数据查找出错！";
        }
        return dsreturn;

    }
}