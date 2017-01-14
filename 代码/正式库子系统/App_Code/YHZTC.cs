using System;
using System.Collections.Generic;
using System.Web;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using FMOP.DB;
using System.Data.SqlClient;
using System.Data;

/// <summary>
///YHZTC 的摘要说明
/// </summary>
public class YHZTC
{
	public YHZTC()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    public string GetClientNumber()
    {
        string ClientNumber = null;

        //string NowYear = System.DateTime.Now.ToString("yyyy");
        //string TwobitYear = System.DateTime.Now.ToString("yy");
        string NowYear = System.DateTime.Now.ToString("yy");
        string NowMonth = System.DateTime.Now.ToString("MM");


        //获取年月和顺序号的编码 
        string nextNumber = RtClientNumber(NowYear, NowMonth);

        string title = "Z";
        //获得最后的客户编号
        ClientNumber = title + NowYear + NowMonth + nextNumber;
        return ClientNumber;
    }

    string RtClientNumber(string NowYear, string NowMonth)
    {
        int sq= 1;
        string sql = "select * from fm_sequence where NF='"+NowYear+"' and YF='"+NowMonth+"'";
        string Operate = "";
        SqlDataReader reader = DbHelperSQL.ExecuteReader(sql);
        if (reader.Read())
        {
            sq = Convert.ToInt32(reader["SXH"])+1;
            Operate = "update fm_sequence set SXH= "+sq+" where number='"+reader["number"]+"'";
        }
        else
        {
            WorkFlowModule WFM = new WorkFlowModule("FM_Sequence");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            Operate = "INSERT INTO [FM_Sequence]([Number],[NF],[YF],[SXH],[KTZFC]) VALUES "+
                " ('"+KeyNumber+"','"+NowYear+"','"+NowYear+"',1,'Z')";
        }
        reader.Close();
        string Sequence=sq.ToString();
        int length = 5;
        for (int i = Sequence.Length; i < length; i++)
        {
            Sequence = "0" + Sequence;
        }
        int c = DbHelperSQL.ExecuteSql(Operate);
        return Sequence;
    }


}