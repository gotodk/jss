using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// JYFXYMX 交易方信用明细
/// </summary>
public static class JYFXYMX
{
	
    /// <summary>
    /// 用户信用等级图片
    /// </summary>
    /// <param name="userScore"></param>
    /// <returns></returns>
    public static string[] GetXYImages(double userScore)
    {
        if (userScore >= 1 && userScore < 4)//1-3分  “一心”
        {
            return new string[] { UserYXImage.XinXing };
        }
        else if (userScore >= 4 && userScore < 7)//4-6分  “二心”
        {
            return new string[] { UserYXImage.XinXing, UserYXImage.XinXing };
        }
        else if (userScore >= 7 && userScore < 10)//7-9分  “三心”
        {
            return new string[] { UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing };
        }
        else if (userScore >= 10 && userScore < 13)//10-12分  “四心”
        {
            return new string[] { UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing };
        }
        else if (userScore >= 13 && userScore < 16)//13-15分  “五心”
        {
            return new string[] { UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing };
        }
        else if (userScore >= 16 && userScore < 21)//13-15分  “一钻”
        {
            return new string[] { UserYXImage.ZuanXing };
        }
        else if (userScore >= 21 && userScore < 26)//21-25分  “二钻”
        {
            return new string[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing };
        }
        else if (userScore >= 26 && userScore < 31)//26-30分  “三钻”
        {
            return new string[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing };
        }
        else if (userScore >= 31 && userScore < 36)//31-25分  “四钻”
        {
            return new string[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing };
        }
        else if (userScore >= 36 && userScore < 41)//36-40分  “五钻”
        {
            return new string[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing };
        }
        else if (userScore >= 41 && userScore < 46)//41-45分  “一皇冠”
        {
            return new string[] { UserYXImage.HuanGuanXing };
        }
        else if (userScore >= 46 && userScore < 51)//46-50分  “二皇冠”
        {
            return new string[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
        }
        else if (userScore >= 51 && userScore < 56)//46-50分  “三皇冠”
        {
            return new string[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
        }
        else if (userScore >= 56 && userScore < 61)//56-60分  “四皇冠”
        {
            return new string[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
        }
        else if (userScore >= 61 && userScore < 100)//61-99分  “五皇冠”
        {
            return new string[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
        }
        else if (userScore >= 100) //100分以上 “VIP”
        {
            return new string[] { UserYXImage.VipXing };

        }
        return null;
    }




}