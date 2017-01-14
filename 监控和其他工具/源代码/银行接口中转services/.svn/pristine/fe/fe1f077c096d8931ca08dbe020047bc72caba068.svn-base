using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;

/// <summary>
/// BankServiceHelper 的摘要说明
/// </summary>
public static class BankServiceHelper
{
    /// <summary>
    /// 获取最新流水号
    /// </summary>
    /// <returns>string</returns>
    public static string GetLSH()
    {
        string LSH = string.Empty;
        try
        {
            HttpWebRequest htpQu = (HttpWebRequest)WebRequest.Create("http://192.168.0.26:777/pingtaiservices/bankls.aspx");
            HttpWebResponse htpRp = (HttpWebResponse)htpQu.GetResponse();
            string Number = string.Empty;
            using (Stream s = htpRp.GetResponseStream())
            {
                if (s != null)
                {
                    using (StreamReader sr = new StreamReader(s, System.Text.Encoding.UTF8))
                        Number = LSH = sr.ReadToEnd().Trim();
                }
                else
                    LSH = null;
            }
        }
        catch (Exception ex)
        {
            LSH = null;
            LogHelper.Log_Wirte("LogMachine", "获取流水号异常：" + ex.Message, "GetLSH", false, false);
        }
        return LSH;
    }


}