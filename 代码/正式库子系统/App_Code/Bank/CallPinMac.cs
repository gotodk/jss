using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using Key;
using Hesion.Brick.Core;
using System.Data.SqlClient;
using Hesion.Brick.Core.WorkFlow;
using Galaxy.ClassLib.DataBaseFactory;
using System.Web.Services;
using System.Threading;

/// <summary>
/// PinMac类的封装。
/// </summary>
public static class CallPinMac
{

    public static BankPF.Service BankIsPF = new BankPF.Service();

    /// <summary>
    /// 加密字符串
    /// </summary>
    /// <param name="word">待加密串</param>
    /// <returns>结果</returns>
    public static string EncryptString(string word)
    {
        //DataTable dt = DbHelperSQL.Query("SELECT [ID],[ExSerial],[PinKey],[MacKey] FROM [AAA_PinMac]").Tables[0];
        string Keys = BankIsPF.GetKey();
        string[] strArr = Keys.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        string key = "";

        if (strArr[0] != "NULL")
            key = PinMac.InitPassword("handsome", strArr[1]);
        else
            return null;

        return PinMac.NewPassword(key, word);
    }
    /// <summary>
    /// 解密字符串
    /// </summary>
    /// <param name="word">带解密串</param>
    /// <returns>结果</returns>
    public static string DecryptString(string word)
    {
        //DataTable dt = DbHelperSQL.Query("SELECT [ID],[ExSerial],[PinKey],[MacKey] FROM [AAA_PinMac]").Tables[0];
        string Keys = BankIsPF.GetKey();
        string[] strArr = Keys.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        string key = "";
        if (strArr[0] != "NULL")
            key = PinMac.InitPassword("handsome", strArr[1]);
        else
            return null;
        return PinMac.InitPassword(key, word);

    }

    public static byte[] GetMac(string word)
    {
        //DataTable dt = DbHelperSQL.Query("SELECT [ID],[ExSerial],[PinKey],[MacKey] FROM [AAA_PinMac]").Tables[0];
        string Keys = BankIsPF.GetKey();
        string[] strArr = Keys.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        string key = "";
        if (strArr[0] != "NULL")
            key = PinMac.InitPassword("handsome", strArr[2]);
        else
            return null;
        return PinMac.NewMac(word, key);
    }
}