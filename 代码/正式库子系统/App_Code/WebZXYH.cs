using System;
using System.Collections.Generic;
using System.Web;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using FMOP.DB;
using System.Data.SqlClient;
using System.Data;


/// <summary>
///WebZXYH 的摘要说明
/// </summary>
public class WebZXYH
{
	public WebZXYH()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public string GetClientNumber(string title)
    {
        string ClientNumber = null;

        //string NowYear = System.DateTime.Now.ToString("yyyy");
        //string TwobitYear = System.DateTime.Now.ToString("yy");
        string NowYear = System.DateTime.Now.ToString("yy");
        string NowMonth = System.DateTime.Now.ToString("MM");


        //获取年月和顺序号的编码 
        string nextNumber = RtClientNumber(NowYear, NowMonth,title);

        string t = title;
        //获得最后的客户编号
        ClientNumber = t + NowYear + NowMonth + nextNumber;
        return ClientNumber;
    }

    string RtClientNumber(string NowYear, string NowMonth,string Title)
    {
        int sq = 1;
        string sql = "select * from fm_sequence where NF='" + NowYear + "' and YF='" + NowMonth + "' and ktzfc='"+Title+"'";
        string Operate = "";
        SqlDataReader reader = DbHelperSQL.ExecuteReader(sql);
        if (reader.Read())
        {
            sq = Convert.ToInt32(reader["SXH"]) + 1;
            Operate = "update fm_sequence set SXH= " + sq + " where number='" + reader["number"] + "'";
        }
        else
        {
            WorkFlowModule WFM = new WorkFlowModule("FM_Sequence");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            Operate = "INSERT INTO [FM_Sequence]([Number],[NF],[YF],[SXH],[KTZFC]) VALUES " +
                " ('" + KeyNumber + "','" + NowYear + "','" + NowMonth + "',1,'" + Title + "')";
        }
        reader.Close();
        string Sequence = sq.ToString();
        int length = 5;
        for (int i = Sequence.Length; i < length; i++)
        {
            Sequence = "0" + Sequence;
        }
        int c = DbHelperSQL.ExecuteSql(Operate);
        return Sequence;
    }

    string RtClientNumber(string NowYear, string NowMonth, string Title,int Seq)
    {
        int sq = 1;
        string sql = "select * from fm_sequence where NF='" + NowYear + "' and YF='" + NowMonth + "' and ktzfc='" + Title + "'";
        string Operate = "";
        SqlDataReader reader = DbHelperSQL.ExecuteReader(sql);
        if (reader.Read())
        {
            sq = Convert.ToInt32(reader["SXH"]) + 1;
            Operate = "update fm_sequence set SXH= " + sq + " where number='" + reader["number"] + "'";
        }
        else
        {
            WorkFlowModule WFM = new WorkFlowModule("FM_Sequence");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            Operate = "INSERT INTO [FM_Sequence]([Number],[NF],[YF],[SXH],[KTZFC]) VALUES " +
                " ('" + KeyNumber + "','" + NowYear + "','" + NowMonth + "',1,'" + Title + "')";
        }
        reader.Close();
        string Sequence = sq.ToString();
        int length = Seq;
        for (int i = Sequence.Length; i < length; i++)
        {
            Sequence = "0" + Sequence;
        }
        int c = DbHelperSQL.ExecuteSql(Operate);
        return Sequence;
    }
    //获得服务站点编号
    public string getFWZDNumber(string Title)
    {
        int sq = 1;
        string sql = "select * from fm_sequence where KTZFC='F'";
        string Operate = "";
        SqlDataReader reader = DbHelperSQL.ExecuteReader(sql);
        if (reader.Read())
        {
            sq = Convert.ToInt32(reader["SXH"]) + 1;
            Operate = "update fm_sequence set SXH= " + sq + " where number='" + reader["number"] + "'";
        }
        else
        {
            WorkFlowModule WFM = new WorkFlowModule("FM_Sequence");
            string KeyNumber = WFM.numberFormat.GetNextNumber();
            Operate = "INSERT INTO [FM_Sequence]([Number],[NF],[YF],[SXH],[KTZFC]) VALUES " +
                " ('" + KeyNumber + "','','',1,'F')";
        }
        reader.Close();
        string Sequence = sq.ToString();
        int length = 5;
        for (int i = Sequence.Length; i < length; i++)
        {
            Sequence = "0" + Sequence;
        }
        int c = DbHelperSQL.ExecuteSql(Operate);
        Sequence = Title + Sequence;
        return Sequence;
    }
    


   
    //生成服务机构的编号
    /// <summary>
    ///title 编号的开头字母
    ///Seq 顺序号的位数
    /// </summary>
    public string getJGNumber(string title, int Seq)
    {
        string ClientNumber = null;

        //string NowYear = System.DateTime.Now.ToString("yyyy");
        //string TwobitYear = System.DateTime.Now.ToString("yy");
        string NowYear = System.DateTime.Now.ToString("yy");
        string NowMonth = System.DateTime.Now.ToString("MM");


        //获取年月和顺序号的编码 
        string nextNumber = RtClientNumber(NowYear, NowMonth, title,Seq);

        string t = title;
        //获得最后的客户编号
        ClientNumber = t + NowYear + NowMonth + nextNumber;
        return ClientNumber;
    }


}