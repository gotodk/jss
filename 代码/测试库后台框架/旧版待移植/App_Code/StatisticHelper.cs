using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
/// <summary>
/// For Statistic Helper Class.Des by Gao Chunyu.Code by Guo Tuo.
/// </summary>
public class StatisticHelper
{
	public StatisticHelper()
	{
        //HttpContext.Current.Response.Write("StatisticHelper");
	}
    /// <summary>
    /// 插入终端统计表（+1重载）
    /// </summary>
    /// <param name="ModelName">模块名（XXXX.aspx）</param>
    /// <param name="terminal">终端名（PC/PDA/PAD）</param>
    /// <param name="GH">操作人工号</param>
    /// <param name="Do">具体操作（页面回发/提交）</param>
    /// <returns></returns>
    public bool InsertStatistic(string ModelName, string terminal,string GH,string Do) 
    { 
        WorkFlowModule PK = new WorkFlowModule("FWPT_ZDCZTJ");
        string Number = PK.numberFormat.GetNextNumber();
        string CZSJ=DateTime.Now.ToString();
        string XM = DbHelperSQL.GetSingle("SELECT Employee_Name FROM HR_Employees WHERE NUMBER ='" + GH + "'").ToString();
        string InsertSql = "INSERT INTO FWPT_ZDCZTJ(Number,CZMK,CZZGH,CZZXM,CZZD,CZSJ,BYZD1,CreateTime,CreateUser) VALUES('" + Number + "','" + ModelName + "','" + GH + "','" + XM + "','" + terminal + "','" + CZSJ + "','" + Do + "','" + CZSJ + "','" + GH + "')";
        int i = DbHelperSQL.ExecuteSql(InsertSql);
        if (i == 1)
            return true;
        else
            return false;
    }
    public bool InsertStatistic(string ModelName, string terminal, string GH)
    {
        WorkFlowModule PK = new WorkFlowModule("FWPT_ZDCZTJ");
        string Number = PK.numberFormat.GetNextNumber();
        string CZSJ = DateTime.Now.ToString();
        string XM = DbHelperSQL.GetSingle("SELECT Employee_Name FROM HR_Employees WHERE NUMBER ='" + GH + "'").ToString();
        string InsertSql = "INSERT INTO FWPT_ZDCZTJ(Number,CZMK,CZZGH,CZZXM,CZZD,CZSJ,BYZD1,CreateTime,CreateUser) VALUES('" + Number + "','" + ModelName + "','" + GH + "','" + XM + "','" + terminal + "','" + CZSJ + "','" + "页面回发或提交" + "','" + CZSJ + "','" + GH + "')";
        int i = DbHelperSQL.ExecuteSql(InsertSql);
        if (i == 1)
            return true;
        else
            return false;
    }
}