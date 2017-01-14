using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;

/// <summary>
/// ErpNumber 的摘要说明
/// </summary>
public class ErpNumber
{
	public ErpNumber()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 生成“证券资金账号”规则：注册类别为单位的：8+2位年+2位月+5位顺序号，注册类别为个人的：9+2位年+2位月+5位顺序号
    /// </summary>
    /// <param name="str">单位，个人</param>
    /// <returns></returns>
    public string GetZQZJNumber(string str)
    {
        WorkFlowModule WFM;
        string re = "";
        switch (str)
        {
            case "单位":
                WFM = new WorkFlowModule("AAA_ZQZJZHJLB");
                re = WFM.numberFormat.GetNextNumberZZ_JYFBH("8");

                break;
            case "个人":
                WFM = new WorkFlowModule("AAA_ZQZJZHJLB");
                re = WFM.numberFormat.GetNextNumberZZ_JYFBH("9");
                break;
        }
        //设置这个证券资金账号已经用过
        string strSql3 = "insert AAA_ZQZJZHJLB (Number,YSYZQZJZH,NextChecker,CheckState,CreateUser,CreateTime,CheckLimitTime) values('" + re + "','" + re + "','0','1','定死的','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
        DbHelperSQL.ExecuteSql(strSql3);
        return re;
        //string KeyNumber = "";
        //// DateTime dt = DateTime.Now;
        //string StrGetNum = "";
        //switch (str)
        //{ 
        //    case "单位":
        //        StrGetNum = " select Max(isnull(YSYZQZJZH,0)) as YSYZQZJZH from  AAA_ZQZJZHJLB where YSYZQZJZH like '" + "8" + DateTime.Now.ToString("yyyyMMdd").Substring(2, 4).ToString() + "%'";
        //        break;
        //    case "个人":
        //        StrGetNum = " select Max(isnull(YSYZQZJZH,0)) as YSYZQZJZH from  AAA_ZQZJZHJLB where YSYZQZJZH like '" + "9" + DateTime.Now.ToString("yyyyMMdd").Substring(2, 4).ToString() + "%'";
        //        break;
        //}


   

        //DataSet dsNum = DbHelperSQL.Query(StrGetNum);
        //if (dsNum == null || dsNum.Tables[0].Rows.Count < 1)
        //{
        //    switch (str)
        //    {
        //        case "单位":
        //            KeyNumber ="8"+DateTime.Now.ToString("yyyyMMdd").Substring(2,4).ToString()+ "00001";
        //            break;
        //        case "个人":
        //            KeyNumber = "9" + DateTime.Now.ToString("yyyyMMdd").Substring(2, 4).ToString() + "00001";
        //            break;
        //    }
         
        //}
        //else
        //{
        //    if (dsNum.Tables[0].Rows[0]["YSYZQZJZH"].ToString().Trim() != "")
        //    {
        //        // KeyNumber = (int.Parse(dsNum.Tables[0].Rows[0]["Number"].ToString().Trim()) + 1).ToString().Trim();
        //        /*这样做的原因：直接把Number转换成int类型在加1的操作不可取，会超出Int32的长度限制
        //         * 1.对I_ZQZJZH进行分割操作取出前5位为8(9)yyMM前置号，后5位为顺序号
        //         * 2.对顺序号进行加1操作,判断顺序号加1后的值如果为100000则对日期号执行加1操作，同时顺序号重置为00001
        //         */
        //        int strDate = Convert.ToInt32(dsNum.Tables[0].Rows[0]["YSYZQZJZH"].ToString().Trim().Substring(0, 5));
        //        int strOrder = Convert.ToInt32(dsNum.Tables[0].Rows[0]["YSYZQZJZH"].ToString().Trim().Substring(5));
        //        strOrder++;
        //        if (strOrder < 100000)
        //        {
        //            KeyNumber = strDate.ToString() + strOrder.ToString().PadLeft(5, '0');
        //        }
        //        else
        //        {
        //            strDate++;
        //            KeyNumber = strDate.ToString() + "00001";
                 
        //        }
        //    }
        //    else
        //    {
        //        switch (str)
        //        {
        //            case "单位":
        //                KeyNumber = "8" + DateTime.Now.ToString("yyyyMMdd").Substring(2, 4).ToString() + "00001";
        //                break;
        //            case "个人":
        //                KeyNumber = "9" + DateTime.Now.ToString("yyyyMMdd").Substring(2, 4).ToString() + "00001";
        //                break;
        //        }
        //    }
        //}
        //return KeyNumber;
    }




}