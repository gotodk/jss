using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI;

/// <summary>
/// BankHelper 的摘要说明
/// </summary>
public static class BankHelper
{
    /// <summary>
    /// 获取流水号
    /// </summary>
    /// <returns></returns>
    public static string GetLSH()
    {
        string LSH = null;
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
            //LogHelper.Log_Wirte("LogMachine", "获取流水号异常：" + ex.Message, "GetLSH", false, false);
        }
        return LSH;
    }

    /// <summary>
    /// 全局银行接口配置表
    /// </summary>
    /// <param name="p">this</param>
    /// <returns></returns>
    public static DataTable GetConfig(string Bank)
    {
        try
        {
            string Paths = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "BankConfig.xml";
            DataTable dt = new DataTable();
            dt.ReadXml(Paths);
            DataTable result = dt.Clone();
            result.Rows.Add(dt.Select("Name='" + Bank.Trim() + "'")[0].ItemArray);
            return result;
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            return null;
        }
    }

    public static DataSet Go(string Routing,string Method,object[] bParams)
    {
        try
        {
            if (Routing != null)
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                Type tp = asm.GetType(Routing, false);
                object instance = asm.CreateInstance(Routing, false);
                MethodInfo method = tp.GetMethod(Method);
                object[] _params = bParams;
                DataSet dsReturn = (DataSet)method.Invoke(instance, _params);
                return dsReturn;
            }
            else
                return null;
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            return null;
        }
    }
}