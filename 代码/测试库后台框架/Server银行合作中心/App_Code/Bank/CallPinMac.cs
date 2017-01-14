using FMipcClass;
using System;

/// <summary>
/// PinMac类的封装。
/// </summary>
public static class CallPinMac
{

   // public static BankPF.Service BankIsPF = new BankPF.Service();

    /// <summary>
    /// 加密字符串
    /// </summary>
    /// <param name="word">待加密串</param>
    /// <returns>结果</returns>
    public static string EncryptString(string word)
    {
        //DataTable dt = DbHelperSQL.Query("SELECT [ID],[ExSerial],[PinKey],[MacKey] FROM [AAA_PinMac]").Tables[0];
        string Keys = "";//BankIsPF.GetKey();

        object[] ob = IPC.Call("浦发获得加密串", new object[] { "" });
        if (ob[0].ToString().Equals("ok"))
        {
            Keys = ob[1].ToString();
        }
        else
        {
            return null;
        }

        string[] strArr = Keys.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        string key = "";

        if (strArr[0] != "NULL")
            key = PinMac.InitPassword("handsome", strArr[1]);
        else
            return null;

        return  PinMac.NewPassword(key,word);
    }
    /// <summary>
    /// 解密字符串
    /// </summary>
    /// <param name="word">带解密串</param>
    /// <returns>结果</returns>
    public static string DecryptString(string word)
    {
        //DataTable dt = DbHelperSQL.Query("SELECT [ID],[ExSerial],[PinKey],[MacKey] FROM [AAA_PinMac]").Tables[0];
        string Keys = "";//BankIsPF.GetKey();

        object[] ob = IPC.Call("浦发获得加密串", new object[] { "" });
        if (ob[0].ToString().Equals("ok"))
        {
            Keys = ob[1].ToString();
        }
        else
        {
            return null;
        }

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
        string Keys = "";//BankIsPF.GetKey();
        object[] ob = IPC.Call("浦发获得加密串", new object[] { "" });
        if (ob[0].ToString().Equals("ok"))
        {
            Keys = ob[1].ToString();
        }
        else
        {
            return null;
        }
        string[] strArr = Keys.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        string key = "";
        if (strArr[0] != "NULL")
            key = PinMac.InitPassword("handsome", strArr[2]);
        else
            return null;
        return PinMac.NewMac (word, key);
    }
}