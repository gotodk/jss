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
public class WebYHZTC
{
    public WebYHZTC()
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

        //获得最后的客户编号
        ClientNumber = title + NowYear + NowMonth + nextNumber;
        return ClientNumber;
    }

    string RtClientNumber(string NowYear, string NowMonth,string Title)
    {
        int sq = 1;
        string sql = "select * from fm_sequence where NF='" + NowYear + "' and YF='" + NowMonth + "' and KTZFC='"+Title+"'";
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
                " ('" + KeyNumber + "','" + NowYear + "','" + NowMonth + "',1,'"+Title+"')";
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dtHT">合同日期</param>
    /// <param name="JSZQ">结算周期</param>
    /// <param name="JSR">每个月的结算日</param>
    /// <param name="dtJZ">截止日期</param>
    /// <returns>获取截止日期之后的结算日</returns>
    public DateTime getNextJSR(DateTime dtHT,int JSZQ,int JSR,DateTime dtJZ)
    {
        dtHT = Convert.ToDateTime(dtHT.ToShortDateString());
        dtJZ = Convert.ToDateTime(dtJZ.ToShortDateString());
        DateTime nextJSR = dtHT;
        DateTime dtStart = dtHT;
        TimeSpan span = dtJZ - dtStart;
        int day = span.Days;
        ///获取截止要计算的结算日是第几个结算日
        if (JSZQ==0)
        {
            JSZQ = 1;
        }
        int m = day / JSZQ;
        if (m==0)
        {
            DateTime d1 = dtHT.AddDays(JSZQ);
            DateTime d2 = d1.AddDays(-d1.Day +JSR);
            if (d1 > d2)
            {
                while (d1>d2)
                {
                    d2 = d2.AddMonths(1);
                }
                
            }
            nextJSR = d2;
        }
        int c = day%JSZQ;
        if (m==1)
        {
            if (c==0)
            {
                DateTime d1 = dtHT.AddDays(JSZQ);
                DateTime d2 = d1.AddDays(-d1.Day + JSR);
                if (d2<dtJZ)
                {
                    while (d2<dtJZ)
                    {
                        d2 = d2.AddMonths(1);
                    }
                }
                nextJSR = d2;
            }
            else
            {
                 DateTime d1 = dtHT.AddDays(JSZQ);//4.17
                 DateTime d2 = d1.AddDays(-d1.Day + JSR);//4.20
                 if (d1 > d2)
                 {
                     while (d1 > d2)
                     {
                         d2 = d2.AddMonths(1);
                     }
                 }
                 if (dtJZ <= d1)
                 {
                     nextJSR = d2;
                 }
                 if (dtJZ>d1&&dtJZ<=d2)
                 {
                      nextJSR = d2;
                 }
                 if (dtJZ>d2)
                 {
                     d2 = d2.AddDays(JSZQ);
                     DateTime d3 = d2.AddDays(JSR - d2.Day);
                     if (d3<d2)
                     {
                         while (d3 < d2)
                         {
                             d3 = d3.AddMonths(1);
                         }
                        
                     }
                     nextJSR = d3;
                 }
            }
        }
        if (m>1)
        {
            int s = m;
            //if (c!=0)
            //{
            //    s = s + 1;
            //}
            int days = JSZQ * s;
            DateTime d1 = dtHT.AddDays(days);//周期日
            DateTime d2 = d1.AddDays(-d1.Day + JSR);//结算日
            if (d1>d2)
            {
                while (d1>d2)
                {
                    d2 = d2.AddMonths(1);
                }
            }
            if (dtJZ<=d1)
            {
                nextJSR = d2;
            }
            if (dtJZ>d1&&dtJZ<=d2)
            {
                nextJSR = d2;
            }
            if (dtJZ > d2)
            {
                d2 = d2.AddDays(JSZQ);
                DateTime d3 = d2.AddDays(JSR - d2.Day);
                if (d3 < d2)
                {
                    while (d3 < d2)
                    {
                        d3 = d3.AddMonths(1);
                    }

                }
                nextJSR = d3;
            }
        }
        return nextJSR;
    }

}