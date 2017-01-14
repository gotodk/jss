using System;
using System.Collections.Generic;
using System.Web;
using Hesion.Brick.Core.WorkFlow;
using FMOP.DB;
using System.Data.SqlClient;

/// <summary>
///YHDengLu 的摘要说明
/// </summary>
public class YHDengLu
{
	public YHDengLu()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 写入用户登录提醒表
    /// </summary>
    /// <param name="yhbh">用户编号</param>
    /// <param name="yhmc">用户名称</param>
    /// <param name="ssbsc">所属办事处</param>
    /// <param name="IP">登录IP</param>
    /// <returns>成功返回1，否则返回0</returns>
    public int DL(string yhbh,string yhmc,string ssbsc,string IP)
    {
        WorkFlowModule WFM = new WorkFlowModule("FM_YHDLTXB");
        string KeyNumber = WFM.numberFormat.GetNextNumber();//主键
        string sql = "INSERT INTO [FM_YHDLTXB]([Number],[YHBH],[YHMC],[SSBSC],[DLIP]) "+        
            " VALUES('"+KeyNumber+"','"+yhbh+"','"+yhmc+"' ,'"+ssbsc+"','"+IP+"')";
        int c = DbHelperSQL.ExecuteSql(sql);
        return c;
    }
    /// <summary>
    /// 统计登录次数
    /// </summary>
    /// <param name="yhbh">用户编号</param>
    /// <param name="dtstart">统计开始时间，不做限制的时候写空字符串</param>
    /// <param name="dtend">统计结束时间，不做限制的时候写空字符串</param>
    /// <returns></returns>
    public int DLStatist(string yhbh,string dtstart,string dtend)
    {
        string sql = "select Count(*)c from [FM_YHDLTXB] where yhbh='"+yhbh+"'";
        if (dtstart!="")
        {
            sql += " and CreateTime>='"+dtstart+"'";
        }
        if (dtend!="")
        {
            sql += " and CreateTime<'"+dtend+"'";
        }
        int dlcs = 0;
        SqlDataReader reader = DbHelperSQL.ExecuteReader(sql);
        if (reader.Read())
        {
            dlcs = Convert.ToInt32(reader["c"]);
        }
        reader.Close();
        return dlcs;
    }
}