using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
/// <summary>
/// InitValidater 的摘要说明
/// </summary>
public static class InitValidater
{
    /// <summary>
    /// 验证投标单信息，将客户端提交过来的DataTable数据传到本方法中，方法将返回相同架构的DataTable作为最后验证数据和附加信息
    /// </summary>
    /// <param name="Result">算法进行评估，如果客户端的值与服务器端的值相差过大，不允许提交数据</param>
    /// <param name="Info">Result为true时，本信息没有价值。为False时，可将此信息作为错误数据返回给客户端</param>
    /// <param name="dtTBD">客户端传递过来的DataTable信息</param>
    public static DataTable ValTBD(DataTable dtTBD, ref bool Result, ref string Info)
    {
        #region 需要重新验证||计算的变量
        string MJTBBZJBL = dtTBD.Rows[0]["MJTBBZJBL"].ToString();//卖家投标保证金比例
        string TBJE = dtTBD.Rows[0]["TBJE"].ToString();//投标金额
        string TBNSL = dtTBD.Rows[0]["TBNSL"].ToString();//投标拟售量
        string MJTBBZJZXZ = dtTBD.Rows[0]["MJTBBZJZXZ"].ToString();//卖家投标保证金最小值
        string HTQX = dtTBD.Rows[0]["HTQX"].ToString();//合同期限
        string PTSDZDJJPL = dtTBD.Rows[0]["PTSDZDJJPL"].ToString();//平台设定最低经济批量
        string TBJG = dtTBD.Rows[0]["TBJG"].ToString();//投标价格
        string 其他资质 = dtTBD.Rows[0]["其他资质"].ToString();//附加资质
        string MJSDJJPL = dtTBD.Rows[0]["MJSDJJPL"].ToString();//卖家设定经济批量
        string DJTBBZJ = dtTBD.Rows[0]["DJTBBZJ"].ToString();//冻结投标保证金
        #endregion
        if (!(Convert.ToInt32(MJSDJJPL) > 0) || (Convert.ToInt32(MJSDJJPL) > Convert.ToInt32(PTSDZDJJPL)))
        {
            Result = false;
            Info = "经济批量数据错误。";
            return dtTBD;
        }

        int NSL = Convert.ToInt32(TBNSL);
        if (HTQX.Trim().Equals("即时"))
        {
            if (Convert.ToInt32(PTSDZDJJPL) > NSL)
            {
                Result = false;
                Info = "投标拟售量数据错误。";
                return dtTBD;
            }
        }
        else
        {
            if (Convert.ToInt32(PTSDZDJJPL) * 10 > NSL)
            {
                //10倍验证
                Result = false;
                Info = "投标拟售量数据错误，error code:2。";
                return dtTBD;
            }
        }

        if ((Convert.ToDouble(TBJG) * Convert.ToDouble(TBNSL)).ToString().Length > 10)
        {
            Result = false;
            Info = "金额过大。";
            return dtTBD;
        }

        double money = Convert.ToDouble(TBJG);
        double amount = Convert.ToDouble(TBNSL);
        double bl = Convert.ToDouble(MJTBBZJBL);
        double zxz = Convert.ToDouble(MJTBBZJZXZ);
        double bzj = Math.Round((money * amount * bl), 2) >= zxz ? Math.Round((money * amount * bl), 2) : zxz;
        if (bzj != Convert.ToDouble(DJTBBZJ))
        {
            if (Math.Abs(bzj - Convert.ToDouble(DJTBBZJ)) >= 1.00)
            {
                //运算异常
                Result = false;
                Info = "投标单数据运算异常，请勿使用非法软件。";
                return dtTBD;
            }
        }

        TBJE = (money * amount).ToString().Trim();//重新计算投标金额
        dtTBD.Rows[0]["TBJE"] = TBJE;
        Result = true;
        return dtTBD;
    }

    /// <summary>
    /// 验证预订单信息，将客户端提交过来的DataTable数据传到本方法中，方法将返回相同架构的DataTable作为最后验证数据和附加信息
    /// </summary>
    /// <param name="dtYDD">预订单初始信息数据</param>
    /// <param name="Result">算法进行评估，如果客户端的值与服务器端的值相差过大，不允许提交数据</param>
    /// <param name="Info">Result为true时，本信息没有价值。为False时，可将此信息作为错误数据返回给客户端</param>
    public static DataTable ValYDD(DataTable dtYDD, ref bool Result, ref string Info)
    {
        double DJDJ = Convert.ToDouble(dtYDD.Rows[0]["DJDJ"]);//冻结订金
        string HTQX = dtYDD.Rows[0]["HTQX"].ToString();//合同期限 
        double PTSDZDJJPL = Convert.ToDouble(dtYDD.Rows[0]["PTSDZDJJPL"]);//平台设定最低经济批量
        double NMRJG = Convert.ToDouble(dtYDD.Rows[0]["NMRJG"]);//拟买入价格
        double NDGSL = Convert.ToDouble(dtYDD.Rows[0]["NDGSL"]);//拟订购数量
        double MJDJBL = Convert.ToDouble(dtYDD.Rows[0]["MJDJBL"]);//冻结比例
        double NDGJE = Convert.ToDouble(dtYDD.Rows[0]["NDGJE"]);//拟定购金额

        if (NDGSL < PTSDZDJJPL)
        {
            Result = false;
            Info = "拟订购数量不能小于最低经济批量";
            return dtYDD;
        }

        if (NDGJE != NMRJG * NDGSL)
        {
            if (Math.Abs(NMRJG * NDGSL - NDGJE) >= 1.00)
            { 
                //运算错误
                Result = false;
                Info = "数据运算错误，请勿使用非法软件。";
                return dtYDD;
            }
        }

        dtYDD.Rows[0]["DJDJ"] = Math.Round((NMRJG * NDGSL * MJDJBL), 2);
        Result = true;
        return dtYDD;
    }
}