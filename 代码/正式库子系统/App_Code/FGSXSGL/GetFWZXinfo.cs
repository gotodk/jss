using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using FMOP.DB;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;

/// <summary>
/// 获得服务中心设定的一些数据，主要用于分公司阅读需求单的验证和报表拉取数据
/// 财务中心所需天、周、自然月、自然财年的时间区间帮助方法在最下方。
/// 郭拓 2012-06-18
/// </summary>
public class GetFWZXinfo
{
    /// <summary>
    /// 当前日期是否可以提交月度需求
    /// </summary>
    /// <returns></returns>
    public bool IsSubRQ(string year,string month)
    {
        //int year = DateTime.Now.Year; //当前填写的年份
        //int month = DateTime.Now.Month; //当前填写的月份
        int day = DateTime.Now.Day;  //当前填写时间是那一天

        //string now = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        bool Is = false;

        string SelectSql = "SELECT * FROM FGS_XQTJQRSJSD WHERE SSNF = '" + year + "' AND SSYF='" + month + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            //string Days = (Convert.ToDateTime(dt.Rows[0]["ZWTJSJ"].ToString())).Day.ToString();
            //string Years = (Convert.ToDateTime(dt.Rows[0]["ZWTJSJ"].ToString())).Year.ToString();
            //string Months = (Convert.ToDateTime(dt.Rows[0]["ZWTJSJ"].ToString())).Month.ToString();

            //DateTime SD = Convert.ToDateTime(Years + "-" + Months + "-" + Days);
            string setTime = (Convert.ToDateTime(dt.Rows[0]["ZWTJSJ"].ToString())).ToString("yyyy-MM-dd");//服务中心设定的最晚提交时间
            string setTimeMilt = (Convert.ToDateTime(dt.Rows[0]["ZWTJSJ"].ToString())).AddDays(-1).ToString("yyyy-MM-dd");
            string now = DateTime.Now.ToString("yyyy-MM-dd"); //现在的时间（将与服务中心设定的最晚提交时间和最晚-1的时间相比较）
            //如果今天是15号，最晚时间是16号，那么，16-1，或者16号都能提交
            if (now == setTime || now == setTimeMilt)
            {
                Is = true;
            }
            else
            {
                Is = false;
            }
        }
        else
        {
            //如果没有设定，那么每个月的2、3号都可以提交
            if (day == 2 || day == 3)
            {
                Is = true;
            }
            else
            {
                Is = false;
            }
        }
        return Is;
    }

    /// <summary>
    /// 获取当月的最晚提交时间的天（string）
    /// </summary>
    /// <param name="year">传入的年份</param>
    /// <param name="month">传入的月份</param>
    public string GetSubmitDay(string year, string month)
    {
        //int days = GetDayOfYearAndMonth(year, month);//获取当月一共有多少天
        //string TimeMin = year + "-" + month + "-01";
        //string TimeMain = year + "-" + month + "-" + days.ToString();
        //string SelectSql = "SELECT ZWTJSJ FROM FGS_XQTJQRSJSD WEHRE ZWTJSJ BETWEEN '" + TimeMin + "' AND '" + TimeMain + "'";
        string SelectSql = "SELECT ZWTJSJ FROM FGS_XQTJQRSJSD WEHRE SSNF = '" + year + "' AND SSYF='" + month + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        string Days = "";
        if (dt.Rows.Count > 0)
        {
            Days = Convert.ToDateTime(dt.Rows[0]["ZWTJSJ"].ToString()).Day.ToString();
        }
        else
        {
            Days = "3";
        }
        return Days;
    }
    /// <summary>
    /// 获取当月的最晚提交时间（yyyy-MM-dd）
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    public DateTime GetSubmitShotTime(string year, string month)
    {
        string SelectSql = "SELECT ZWTJSJ FROM FGS_XQTJQRSJSD WHERE SSNF = '" + year + "' AND SSYF='" + month + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        DateTime times;
        if (dt.Rows.Count > 0)
        {
            times = Convert.ToDateTime(Convert.ToDateTime(dt.Rows[0]["ZWTJSJ"].ToString()).ToString("yyyy-MM-dd"));
        }
        else
        {
            times = Convert.ToDateTime(Convert.ToDateTime(year + "-" + month + "-3").ToString("yyyy-MM-dd"));
        }
        return times;
    }

    /// <summary>
    /// 返回当前时间是否可以录入月度需求
    /// </summary>
    /// <returns></returns>
    public bool IsInputRQ(string year,string month)
    {
        int day = DateTime.Now.Day;  //当前填写时间是那一天
        string now = DateTime.Now.ToString("yyyy-MM-dd");
        bool Is = false;

        string SelectSql = "SELECT * FROM FGS_XQTJQRSJSD WHERE SSNF = '" + year + "' AND SSYF='" + month + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        //如果有设定日期
        if (dt.Rows.Count > 0)
        {
            string SetTime = (Convert.ToDateTime(dt.Rows[0]["ZWTJSJ".ToString()])).AddDays(-1).ToString("yyyy-MM-dd");
            if (now == SetTime)
            {
                Is = true;
            }
            else
            {
                Is = false;
            }
        }
        else
        {
            if (day == 2)
            {
                Is = true;
            }
            else
            {
                Is = false;
            }
        }
        return Is;
    }

    /// <summary>
    /// 获得最新经济批量信息
    /// </summary>
    /// <returns>Int[] ,0--发货经济批量，1--彩鼓生产批量，2--非彩鼓生产批量</returns>
    public int[] GetPLSetNumver()
    {
        string SelectSCPL = "SELECT TOP 1 * FROM FGS_JJPLSDB ORDER BY CreateTime DESC"; //获得最新的发货经济批量、非彩鼓、彩鼓的生产批量
        DataSet ds = DbHelperSQL.Query(SelectSCPL);
        DataTable dtPL = ds.Tables[0];
        int FCG = Convert.ToInt32(dtPL.Rows[0]["FCGSCJJPL"].ToString()); //非彩鼓的生产经济批量
        int CG = Convert.ToInt32(dtPL.Rows[0]["CGSCJJPL"].ToString());//彩鼓的生产经济批量
        int FH = Convert.ToInt32(dtPL.Rows[0]["FHJJPL"].ToString());//发货经济批量
        int[] Number = { FH, CG, FCG };

        return Number;

        //if (dt.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        string XGLB = dt.Rows[i]["XGLB"].ToString();//获得当前硒鼓类别
        //        string XGXH = dt.Rows[i]["XGXH"].ToString();//获得当前硒鼓型号
        //        int SXSL = Convert.ToInt32(dt.Rows[i]["SXSL"].ToString().Trim());//该型号的需求数量

        //        string SelectSql = "SELECT YS FROM FGS_CPXHJGB WHERE CPLB='" + XGLB + "' AND CPXH ='" + XGXH + "'";
        //        string YS = DbHelperSQL.GetSingle(SelectSql).ToString(); //获得改硒鼓型号对应的颜色，黑色代表非彩鼓，其他颜色代表彩鼓
        //        if (YS == "黑色")
        //        {
                    
        //        }
        //    }
        //}
        ////该批次没有需求
        //else
        //{
 
        //}
    }

    /// <summary>
    /// 获取指定类别型号的硒鼓的需求总量（包含已录入、已确认）
    /// </summary>
    /// <param name="year">需求的年份</param>
    /// <param name="month">需求的月份</param>
    /// <param name="XGLB">硒鼓类别（蓝装、绿装、红装）</param>
    /// <param name="XGXH">硒鼓型号</param>
    public int GetPLRealNumber(int year, int month ,string XGLB,string XGXH)
    {
        int sum = 0;

        string XQtime = year.ToString() + "-" + month.ToString();//获得当前需求的时间
        string SelectMain ="SELECT * FROM ("+ "SELECT * FROM FGS_YDXQ_Child WHERE parentNumber in (" + "SELECT Number FROM FGS_YDXQ WHERE XQNF = '" + year + "' AND XQYF = '" + month + "')) AS t WHERE XGLB='"+XGLB+"' AND XGXH ='"+XGXH+"'";
        DataSet ds = DbHelperSQL.Query(SelectMain);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sum = sum + Convert.ToInt32(dt.Rows[i]["SXSL"].ToString().Trim());
            }
            return sum;
        }
        else
        {
            return sum;
        }
        
    }
    /// <summary>
    /// 获取当前批次的全国需求总量
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="XGLB"></param>
    /// <param name="XGXH"></param>
    /// <param name="PC"></param>
    /// <returns></returns>
    public int GetPLRealNumberByPC(int year, int month, string XGLB, string XGXH, string PC)
    {
        int sum = 0;

        string XQtime = year.ToString() + "-" + month.ToString();//获得当前需求的时间
        string SelectMain = "SELECT * FROM (" + "SELECT * FROM FGS_YDXQ_Child WHERE parentNumber in (" + "SELECT Number FROM FGS_YDXQ WHERE XQNF = '" + year + "' AND XQYF = '" + month + "')) AS t WHERE XGLB='" + XGLB + "' AND XGXH ='" + XGXH + "' AND SSPC='" + PC + "'";
        DataSet ds = DbHelperSQL.Query(SelectMain);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sum = sum + Convert.ToInt32(dt.Rows[i]["SXSL"].ToString().Trim());
            }
            return sum;
        }
        else
        {
            return sum;
        }
    }

    /// <summary>
    /// 根据硒鼓类别和型号获得硒鼓颜色
    /// </summary>
    /// <param name="XGLB">硒鼓类别（蓝装、绿装、红装）</param>
    /// <param name="XGXH">硒鼓型号（FM-XXXXXXXXX）</param>
    /// <returns>颜色（除“黑色”外，其他颜色都是彩鼓）</returns>
    public string GetXGColor(string XGLB, string XGXH)
    {
        string SelectSql = "SELECT DXG FROM FGS_CPXHJGB WHERE CPLB = '" + XGLB + "' AND CPXH='" + XGXH + "'";
        string YS = DbHelperSQL.GetSingle(SelectSql).ToString().Trim();
        if (YS=="彩鼓")
        {
            YS = "彩色";
        }
        else
        {
            YS = "黑色";
        }
        return YS;
    }

    /// <summary>
    /// 根据产品类别和硒鼓型号判断是否需要整箱验证
    /// </summary>
    /// <param name="CPLB">产品类别（蓝装、绿装、红装）</param>
    /// <param name="CPXH">产品型号（FM-XXXXXXX）</param>
    /// <returns>0：不需要整箱验证；其他：满箱数量。</returns>
    public int IsNeedValidateFullBox(string CPLB, string CPXH)
    {
        string SelectSql = "SELECT MXSL,SFYZXYZ FROM FGS_CPXHJGB WHERE CPLB='" + CPLB + "' AND CPXH = '" + CPXH + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        DataTable dt = ds.Tables[0];
        string IsNeed = dt.Rows[0]["SFYZXYZ"].ToString();
        string FullBoxNum = "0";
        if (IsNeed.Trim() == "是")
        {
            FullBoxNum = dt.Rows[0]["MXSL"].ToString();
        }
        int FullBox = Convert.ToInt32(FullBoxNum);
        return FullBox;
    }

    /// <summary>
    /// 返回指定类别型号硒鼓已经提交的数量总量（不包含录入）
    /// </summary>
    /// <param name="year">需求年份</param>
    /// <param name="month">需求月份</param>
    /// <param name="XGLB">硒鼓类别</param>
    /// <param name="XGXH">硒鼓型号</param>
    /// <returns></returns>
    public int GetSubedXGNums(string year, string month, string XGLB, string XGXH)
    {
        int sum = 0;

        string SelectMain = "SELECT * FROM (" + "SELECT * FROM FGS_YDXQ_Child WHERE parentNumber in (" + "SELECT Number FROM FGS_YDXQ WHERE XQNF = '" + year + "' AND SFYSX='是' AND XQYF = '" + month + "')) AS t WHERE XGLB='" + XGLB + "' AND XGXH ='" + XGXH + "'";

        DataSet ds = DbHelperSQL.Query(SelectMain);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sum = sum + Convert.ToInt32(dt.Rows[i]["SXSL"].ToString().Trim());
            }
            return sum;
        }
        else
        {
            return sum;
        }
    }
    /// <summary>
    /// 返回指定类别型号硒鼓已经提交的数量某批次（不包含录入）
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="XGLB"></param>
    /// <param name="XGXH"></param>
    /// <returns></returns>
    public int GetSubedXGNumsByPC(string year, string month, string XGLB, string XGXH,string PC)
    {
        int sum = 0;

        string SelectMain = "SELECT * FROM (" + "SELECT * FROM FGS_YDXQ_Child WHERE parentNumber in (" + "SELECT Number FROM FGS_YDXQ WHERE XQNF = '" + year + "' AND SFYSX='是' AND XQYF = '" + month + "')) AS t WHERE XGLB='" + XGLB + "' AND XGXH ='" + XGXH + "' AND SSPC='"+PC+"'";

        DataSet ds = DbHelperSQL.Query(SelectMain);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sum = sum + Convert.ToInt32(dt.Rows[i]["SXSL"].ToString().Trim());
            }
            return sum;
        }
        else
        {
            return sum;
        }
    }
    /// <summary>
    /// 返回是否当前硒鼓型号是否满足经济生产批量
    /// </summary>
    /// <param name="year">需求年份</param>
    /// <param name="month">需求月份</param>
    /// <param name="XGLB">硒鼓类别</param>
    /// <param name="XGXH">硒鼓型号</param>
    /// <returns>Bool类型</returns>
    public bool IsPassMakeNums(string year, string month, string XGLB, string XGXH,string PC)
    {
        int[] PLNums = GetPLSetNumver();//1--彩鼓生产批量，2--非彩鼓生产批量
        string YS = GetXGColor(XGLB, XGXH);//得到颜色
        int Nums = 0;
        if (YS == "黑色")
            Nums = PLNums[2];
        else
            Nums = PLNums[1];

        int RealNums = GetPLRealNumberByPC(Convert.ToInt32(year), Convert.ToInt32(month), XGLB, XGXH,PC);//获取这只硒鼓当月的需求总量（已提交的）

        if (RealNums < Nums)
        {
            return false;
        }
        else
            return true;
    }

    /// <summary>
    /// 获取需求月服务中心设定的批次到货时间
    /// </summary>
    /// <returns>string[] ，0-7分别为1-4批次的最早最晚到货时间</returns>
    public string[] GetFourPCTime(string year,string month)
    {
        string SelectSql = "SELECT Number FROM FGS_YDXQPCSD WHERE SSNF = '" + year + "' AND SSYF = '" + month + "'";
        DataSet ds = DbHelperSQL.Query(SelectSql);
        string SelectSql_Child1 = "SELECT * FROM FGS_YDXQPCSD_Child WHERE parentNumber = '" + ds.Tables[0].Rows[0]["Number"].ToString() + "' AND SSPC='第一批'";
        string SelectSql_Child2 = "SELECT * FROM FGS_YDXQPCSD_Child WHERE parentNumber = '" + ds.Tables[0].Rows[0]["Number"].ToString() + "' AND SSPC='第二批'";
        string SelectSql_Child3 = "SELECT * FROM FGS_YDXQPCSD_Child WHERE parentNumber = '" + ds.Tables[0].Rows[0]["Number"].ToString() + "' AND SSPC='第三批'";
        string SelectSql_Child4 = "SELECT * FROM FGS_YDXQPCSD_Child WHERE parentNumber = '" + ds.Tables[0].Rows[0]["Number"].ToString() + "' AND SSPC='第四批'";
        DataTable ds1 = DbHelperSQL.Query(SelectSql_Child1).Tables[0];
        DataTable ds2 = DbHelperSQL.Query(SelectSql_Child2).Tables[0];
        DataTable ds3 = DbHelperSQL.Query(SelectSql_Child3).Tables[0];
        DataTable ds4 = DbHelperSQL.Query(SelectSql_Child4).Tables[0];
        string[] pc1 = {null,null };
        if (ds1.Rows.Count > 0)
        {
            pc1[0] =ds1.Rows[0]["ZZDHRQ"].ToString();
            pc1[1] = ds1.Rows[0]["ZWDHRQ"].ToString();
        }
        string[] pc2 = { null, null };
        if (ds2.Rows.Count > 0)
        {
            pc2[0] = ds2.Rows[0]["ZZDHRQ"].ToString();
            pc2[1] = ds2.Rows[0]["ZWDHRQ"].ToString();
        }
        string[] pc3 = { null, null };
        if (ds3.Rows.Count > 0)
        {
            pc3[0] = ds3.Rows[0]["ZZDHRQ"].ToString();
            pc3[1] = ds3.Rows[0]["ZWDHRQ"].ToString();
        }
        string[] pc4 = { null, null };
        if (ds4.Rows.Count > 0)
        {
            pc4[0] = ds4.Rows[0]["ZZDHRQ"].ToString();
            pc4[1] = ds4.Rows[0]["ZWDHRQ"].ToString();
        }
        string[] Arrays = {
                              pc1[0],
                              pc1[1],
                              pc2[0],
                              pc2[1],
                              pc3[0],
                              pc3[1],
                              pc4[0],
                              pc4[1]
                          };
        return Arrays;
    }

    /// <summary>
    /// 获取某年某月有多少天
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    public int GetDayOfYearAndMonth(string year, string month)
    {
        int days = 0;
        if (DateTime.IsLeapYear(Convert.ToInt32(year)))//如果是闰年
        {
            switch (month)
            {
                case "1":
                    days = 31;
                    break;
                case "2":
                    days = 29;
                    break;
                case "3":
                    days = 31;
                    break;
                case "4":
                    days = 30;
                    break;
                case "5":
                    days = 31;
                    break;
                case "6":
                    days = 30;
                    break;
                case "7":
                    days = 31;
                    break;
                case "8":
                    days = 31;
                    break;
                case "9":
                    days = 30;
                    break;
                case "10":
                    days = 31;
                    break;
                case "11":
                    days = 30;
                    break;
                case "12":
                    days = 31;
                    break;
            }
        }
        else
        {
            switch (month)
            {
                case "1":
                    days = 31;
                    break;
                case "2":
                    days = 28;
                    break;
                case "3":
                    days = 31;
                    break;
                case "4":
                    days = 30;
                    break;
                case "5":
                    days = 31;
                    break;
                case "6":
                    days = 30;
                    break;
                case "7":
                    days = 31;
                    break;
                case "8":
                    days = 31;
                    break;
                case "9":
                    days = 30;
                    break;
                case "10":
                    days = 31;
                    break;
                case "11":
                    days = 30;
                    break;
                case "12":
                    days = 31;
                    break;
            }
        }
        return days;
    }

    /// <summary>
    /// 超级验证方法。用于验证某年某月某批次的某支硒鼓，修改后的数量是否满足该批次的生产经济批量。
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="XGLB">硒鼓类型</param>
    /// <param name="XGXH">硒鼓型号</param>
    /// <param name="PC">所属批次</param>
    /// <param name="OldNums">修改前的数量</param>
    /// <param name="NewNums">修改后的数量</param>
    public bool SuperValidate(string year, string month, string XGLB, string XGXH, string PC,int OldNums,int NewNums)
    {
        //思路：根据年份月份、硒鼓型号和批次获得该硒鼓当前的全国需求总量
        //取差，总量中减去差量再判断
        //用修改后的数量减去修改前的数量，如果大于0，说明数量被改大了。
        //如果小于0，说明硒鼓数量被改小了。
        int cha = NewNums - OldNums;//差
        int Change = GetPLRealNumberByPC(Convert.ToInt32(year), Convert.ToInt32(month), XGLB, XGXH, PC);//更改前的总量
        Change = Change + cha;//改后这只硒鼓的全国需求总量

        int[] PLNums = GetPLSetNumver();//1--彩鼓生产批量，2--非彩鼓生产批量
        string YS = GetXGColor(XGLB, XGXH);//得到颜色
        int Nums = 0;
        if (YS == "黑色")
            Nums = PLNums[2];
        else
            Nums = PLNums[1];

        if (Change < Nums)
            return false;
        else
            return true;

    }
    /// <summary>
    /// 验证某年某月某主键的某批次是否符合发货经济批量
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="KeyNumber">主键</param>
    /// <param name="PC">批次</param>
    public bool IsSendXG(string year,string month,string KeyNumber,string PC)
    {
        string SelectSql = "SELECT * FROM FGS_YDXQ_Child where parentNumber='" + KeyNumber + "'";
        DataTable dt = DbHelperSQL.Query(SelectSql).Tables[0];
        int sums = 0;
        if (dt.Rows.Count > 0)
        {
            int[] PLNums = GetPLSetNumver();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sums = sums + Convert.ToInt32(dt.Rows[i]["SXSL"].ToString());
            }
            if (sums < PLNums[0])
            {
                return false;
            }
            else
                return true;
        }
        else
        {
            return true;
        }
    }
    public bool IsSendXG(string year, string month, string KeyNumber, string PC,int OldNums,int NewNums)
    {
        string SelectSql = "SELECT * FROM FGS_YDXQ_Child where parentNumber='" + KeyNumber + "'";
        DataTable dt = DbHelperSQL.Query(SelectSql).Tables[0];
        int sums = 0;
        if (dt.Rows.Count > 0)
        {
            int[] PLNums = GetPLSetNumver();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sums = sums + Convert.ToInt32(dt.Rows[i]["SXSL"].ToString());
            }
            sums = sums + (NewNums - OldNums);
            if (sums < PLNums[0])
            {
                return false;
            }
            else
                return true;
        }
        else
        {
            int[] PLNums = GetPLSetNumver();
            if (NewNums < PLNums[0])
            {
                return false;
            }
            else
                return true;
        }
    }

    //-----------------------------------------------------华丽的分割线-----------------------------------------------------------------

    //服务中心报表所需数据

    /// <summary>
    /// 根据需求年月确定当前年月所有分公司的订单单号，可根据订单单号去ERP查询关联的销货单
    /// </summary>
    /// <param name="Year">string，需求年份</param>
    /// <param name="Month">string，需求月份</param>
    /// <returns>订单编号1,2,3,4...</returns>
    public string GetMonthDealList(string Year, string Month)
    {
        string MainERP = DbHelperSQL.GetSingle("SELECT DYERP FROM SYSTEM_CITY_0 WHERE NAME='公司总部'").ToString();
        string SelectSql = "SELECT FGS_YDXQ.Number FROM FGS_YDXQ INNER JOIN SYSTEM_CITY_0 ON FGSMC = DYFGSMC WHERE XQNF='" + Year.Trim() + "' AND XQYF='" + Month.Trim() + "' AND SFYSX='是' AND DYERP <> '" + MainERP + "'";
        DataTable dt = DbHelperSQL.Query(SelectSql).Tables[0];
        string numbers = "";
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                numbers = numbers + "'" + dt.Rows[i]["Number"].ToString() + "',";
            }
            numbers = numbers.Substring(0, numbers.Length - 1);
        }
        return numbers;
    }
    /// <summary>
    /// 根据需求年月（和/或）所属分公司确定一组订单单号，可根据订单单号去ERP查询关联的销货单
    /// </summary>
    /// <param name="Year">string，需求年份</param>
    /// <param name="Month">string，需求月份</param>
    /// <param name="FGSBH">string，分公司编号</param>
    /// <returns>订单编号1,2,3,4...</returns>
    public string GetMonthDealList(string Year, string Month, string FGSBH)
    {
        string SelectSql = "SELECT Number FROM FGS_YDXQ WHERE XQNF='" + Year.Trim() + "' AND XQYF='" + Month.Trim() + "' AND KHBH='" + FGSBH.Trim() + "' AND SFYSX='是'";
        DataTable dt = DbHelperSQL.Query(SelectSql).Tables[0];
        string numbers = "";
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                numbers = numbers + ",'" + dt.Rows[i]["Number"].ToString() + "'";
            }
            numbers = numbers.Substring(1, numbers.Length-1);
        }
        return numbers;
    }
    /// <summary>
    /// 为了省事，返回String类型数组，0是最迟入库时间，1是数量，2是欠货天书
    /// </summary>
    /// <param name="DealNumber">订单单号</param>
    /// <param name="SSPC">所属批次</param>
    /// <param name="XGLB">硒鼓类别</param>
    /// <param name="XGXH">硒鼓型号</param>
    /// <param name="FGSBH">分公司编号</param>
    /// <param name="XHRQ">销货日期</param>
    /// <returns></returns>
    public string[] GetLastRQAndNumsAndQHDays(string DealNumber,string SSPC,string XGLB,string XGXH,string FGSBH,string XHRQ)
    {
        string[] Arrays = { null, null, null };
        //最迟生产入库时间
        string LastTime = "SELECT ZWDHSJ,SXSL FROM FGS_YDXQ_Child WHERE parentNumber='" + DealNumber + "' AND SSPC='" + SSPC + "' AND XGLB='" + XGLB + "' AND XGXH='" + XGXH + "'";
        DataTable dt = DbHelperSQL.Query(LastTime).Tables[0];
        if (dt.Rows.Count > 0)
        {
            Arrays[1] = dt.Rows[0]["SXSL"].ToString();//需求数量
            DateTime zuiwanTime = Convert.ToDateTime(dt.Rows[0]["ZWDHSJ"].ToString());//最晚到货时间
            string SSFGS = DbHelperSQL.GetSingle("SELECT FGSMC FROM FGS_YDXQ WHERE KHBH='" + FGSBH + "' AND Number='" + DealNumber + "'").ToString();
            string WLZQ = DbHelperSQL.GetSingle("SELECT YSZQT FROM FGS_ZBFGSDHZQB WHERE FGSMC='" + SSFGS + "'").ToString();//对应分公司的物流周期
            //最迟入库 = 最晚到货时间-物流周期-2
            DateTime rukuTime = zuiwanTime.AddDays(-Convert.ToInt32(WLZQ)).AddDays(-2);//最迟入库时间
            Arrays[0] = rukuTime.ToString("yyyy-MM-dd");//最迟生产入库时间
            //销货时欠货天书 = 销货日期-最迟生产入库时间-2
            DateTime XHTime = DateTime.ParseExact(XHRQ, "yyyyMMdd",null);//销货日期
            TimeSpan tmsp = XHTime.Subtract(rukuTime);//销货日期和最迟入库时间相差的天数
            //DateTime QHDays = XHTime.AddDays(-2).AddDays(tmsp.Days);
            if (tmsp.Days-2 >0)
                Arrays[2] = (tmsp.Days-2).ToString();
            else
                Arrays[2] = "0";
        }
        return Arrays;
    }
    //-----------------------------------------------------华丽的分割线-----------------------------------------------------------------


    //--------------以下内容属于财务中心统计日销售、周销售、月销售报表用--------------------

    /// <summary>
    /// 根据当前时间获取本日统计销售额的时间段
    /// </summary>
    /// <returns>string[]，日初时间：0，日末时间：1</returns>
    public string[] GetDaysInDay()
    {
        DateTime Now = DateTime.Now;//获取当前时间
        DateTime DayFirstDay = Now.AddDays(-1);//统计时用从前一天的下午4：30开始统计
        DateTime DayLastDay = Now;  //到今天的下午4：30结束

        string First = (DayFirstDay.ToString("yyyyMMdd"));
        string Last = (DayLastDay.ToString("yyyyMMdd"));

        string[] Times = { First, Last };
        return Times;
    }
    /// <summary>
    /// 根据当前日期返回所在周需要统计销售额的时间段
    /// </summary>
    /// <returns>string[]，周一：0，周末：1。</returns>
    public string[] GetDaysInWeek()
    {
        //周统计的时间比较变态，是从上周五的下午4：30到下周五的下午4：30之前。
        DateTime Now = DateTime.Now;//获取当前时间
        DateTime WeekFirstDay = Now.AddDays(1 - Convert.ToInt32(Now.DayOfWeek.ToString("d")));//当前时间所在周的周一的日期
        DateTime WeekLastDay = WeekFirstDay.AddDays(4);//当前时间所在周的周五的日期
        //那么，本周的统计区间就应该是WeekFirstDay的前一天的下午4点半以后   --到--  WeekLastDay的当天下午4点半之前。
        string First = WeekFirstDay.AddDays(-3).ToString("yyyyMMdd");//上周五
        //First = First + "163000";//16：30分之后

        string Last = WeekLastDay.ToString("yyyyMMdd");//周五
        //Last = Last + "163000";//16：30分之前
        string[] Times = { First, Last };
        return Times;
    }

    /// <summary>
    /// 根据当前日期返货所在月需要统计销售额的时间段
    /// </summary>
    /// <returns>string[],月初：0，月末：1</returns>
    public string[] GetDaysInMonth()
    {
        DateTime Now = DateTime.Now;//获取当前时间
        DateTime MonthFirstDay = Now.AddDays(1 - Now.Day);//本月月初
        DateTime MonthLastDay = MonthFirstDay.AddMonths(1);//本月月末

        //string First = MonthFirstDay.AddDays(-1).ToString("yyyyMMdd");//本月月初的前一天
        string First = MonthFirstDay.ToString("yyyyMMdd");//上面这行的数据已经要求更改，取的是自然月和自然财年的时间间隔
        //First = First + "1630000";//月初前一天的下午4：30之后

        string Last = MonthLastDay.ToString("yyyyMMdd");//月末
        //Last = Last + "163000";//月末当天下午的4：30之前
        string[] Times = { First, Last };
        return Times;
    }

    /// <summary>
    /// 根据当前时间返回所在财年需要统计销售额的时间段
    /// </summary>
    /// <returns>string[]，年初：0，年末：1</returns>
    public string[] GetDaysInYear()
    {
        //公司财年为每年的三月一号开始。故统计的区间为本年的三月一号至次年的二月末。
        //如果当前月份为三月份之前，则统计时间区间应该是去年的三月初至今年的二月末
        if (DateTime.Now.Month < 3)
        {
            string FirstYear = DateTime.Now.AddYears(-1).Year.ToString() + "0301";//开始年份的年份
            string LastYear = (DateTime.ParseExact(DateTime.Now.Year.ToString() + "0301", "yyyyMMdd", null)).AddDays(-1).ToString("yyyyMMdd");
            string[] Times = { FirstYear.Trim(), LastYear.Trim() };
            return Times;
        }
        //月份大于或者等于三，则统计时间区间应该是今年的三月初至次年的二月末
        else
        {
            string FirstYear = DateTime.Now.Year.ToString() + "0301";//开始年份
            string LastYear = (DateTime.ParseExact(DateTime.Now.AddYears(1).Year.ToString() + "0301", "yyyyMMdd", null)).AddDays(-1).ToString("yyyyMMdd");
            string[] Times = { FirstYear, LastYear };
            return Times;
        }
    }
    //------------------------------服务中心销售报表统计方法完毕------------------------------
}