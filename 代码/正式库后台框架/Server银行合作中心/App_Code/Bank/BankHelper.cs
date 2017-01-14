using FMipcClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI;
using FMDBHelperClass;
using System.Collections;

/// <summary>
/// BankHelper 的摘要说明
/// </summary>
public class BankHelper
{
    /// <summary>
    /// 获取流水号
    /// </summary>
    /// <returns></returns>
    public static string GetLSH()
    {
        string LSH = null;

        #region 2014-07-14 shiyan 从调用网页方式改为直接获取流水号
        //try
        //{

        //HttpWebRequest htpQu = (HttpWebRequest)WebRequest.Create("http://192.168.0.26:777/pingtaiservices/bankls.aspx");
        //HttpWebResponse htpRp = (HttpWebResponse)htpQu.GetResponse();
        //string Number = string.Empty;
        //using (Stream s = htpRp.GetResponseStream())
        //{
        //    if (s != null)
        //    {
        //        using (StreamReader sr = new StreamReader(s, System.Text.Encoding.UTF8))
        //            Number = LSH = sr.ReadToEnd().Trim();
        //    }
        //    else
        //        LSH = null;
        //} 
        //}
        //catch (Exception ex)
        //{
        //    LSH = null;
        //    //LogHelper.Log_Wirte("LogMachine", "获取流水号异常：" + ex.Message, "GetLSH", false, false);
        //} 
        #endregion

        string sql = "update AAA_bankLS set LWSXH = CONVERT(varchar(50), convert(decimal(6,0),isnull(LWSXH,'0'))+1, 0)         select top 1 right ('000000'+LWSXH,6) as newnumber  from AAA_bankLS";
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht_res = I_DBL.RunProc(sql, "data");
        if ((bool)ht_res["return_float"])
        {
            DataSet ds_res = (DataSet)ht_res["return_ds"];
            if (ds_res != null && ds_res.Tables[0].Rows.Count > 0)
            {
                string number = ds_res.Tables[0].Rows[0]["newnumber"].ToString();
                LSH = DateTime.Now.ToString("yyyyMMdd") + number;
            }
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

    public static DataSet Go(string Routing, string Method, object[] bParams)
    {
        try
        {
            if (Routing != null)
            {
                /*
                Assembly asm = Assembly.GetExecutingAssembly();
                Type tp = asm.GetType(Routing, false);
                object instance = asm.CreateInstance(Routing, false);
                MethodInfo method = tp.GetMethod(Method);
                object[] _params = bParams;
                DataSet dsReturn = (DataSet)method.Invoke(instance, _params);
                return dsReturn;
                */
                //改为直接通过ipc调用

                //Routing就是“调用方识别标记”， 这个方法中的"Method"参数无效了。随便传
                object[] re = IPC.Call(Routing, bParams);
                if (re[0].ToString() == "ok")
                {
                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                    DataSet dsReturn = (DataSet)(re[1]);
                    return dsReturn;
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                    string msgerr = re[1].ToString();
                    return null;
                }
            }
            else
                return null;
        }
        catch (Exception ex)
        {
            string msgerr = ex.Message;
            return null;
        }
    }
}