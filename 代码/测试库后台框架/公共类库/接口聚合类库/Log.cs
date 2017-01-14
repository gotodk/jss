using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace FMipcClass
{
    static public class Log
    {
        /// <summary>
        /// 日志类型的枚举
        /// </summary>
        public enum type
        {
            debug, yewu, other
        }
      /// <summary>      
      ///处理业务运行日志 
      /// </summary>
      /// <param name="FF_yewuname">业务方法名</param>
      /// <param name="ELogType">日志类型</param>
      /// <param name="LogText">日志内容</param>
      /// <param name="dsAttrs">其他信息</param>
      /// <returns></returns>
        static public string WorkLog(string FF_yewuname,Enum ELogType,string LogText,DataSet dsAttrs)
        {
            string LogType = "";
            switch (ELogType.ToString())
            { 
                case "debug":
                    LogType = "程序";
                    break;
                case "yewu":
                    LogType = "业务";
                    break;
                case "other":
                    LogType = "其他";
                    break;
                default:
                    break;
            }
        
            //开启一个线程，执行日志处理 
            ThreadStart savelog = delegate { StartSaveLog(FF_yewuname, LogType, LogText, dsAttrs); };           
            Thread thread = new Thread(savelog);
            thread.Name = "IPCWorkLog";
            thread.IsBackground = true;
            //Thread.Sleep(20);
            thread.Start();
            return "ok";
        }
        static private void StartSaveLog(string FF_yewuname, string LogType, string LogText, DataSet dsAttrs)
        {
            try
            {
                //调用业务接口
                object Dre = null;
                //开始真正调用
                string IPCurl = ConfigurationManager.ConnectionStrings["IPCurl"].ToString(); //聚合中心地址
                FMWScenter wsd = new FMWScenter(IPCurl + "?wsdl");
                //获取程序所在服务器主机名  
                System.Net.IPAddress[] addressList = Dns.GetHostAddresses(Dns.GetHostName());
                string allIP = "";
                for(int p = 0; p < addressList.Count();p++)
                {
                    allIP = allIP +  addressList[p].ToString() + "★";
                }
                IPAddress localaddr = addressList[0];
                string[] logparams = new string[] { FF_yewuname, LogType, LogText, allIP };
                Dre = wsd.ExecuteQuery("SaveWorkLog", new object[] {logparams, dsAttrs });
            }
            catch (Exception exx)
            {
                string trytype = "调用写运行日志方法失败：" + exx.ToString();
            }
        }

        /// <summary>
        /// 将要执行的多条sql语句，转为带换行的string，便于记录日志。
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        static public string ArrayListSQL2string(ArrayList al)
        {
            return string.Join(Environment.NewLine + "    ", (string[])al.ToArray(typeof(string)));
        }
    }
}
