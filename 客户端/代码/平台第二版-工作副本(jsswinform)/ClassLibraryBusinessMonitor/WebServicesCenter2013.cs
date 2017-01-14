using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.IO;

namespace ClassLibraryBusinessMonitor
{

    /// <summary>
    /// 用于线程的委托。显示回调。用于显示线程的处理结果，传入线程类中并在线程类中调用
    /// </summary>
    /// <param name="OutPutHT">需要返回的数据集合</param>
    public delegate void delegateForThread(Hashtable OutPutHT);
    /// <summary>
    /// 用于线程的委托。显示回调。用于显示线程的处理结果，在主线程中配合Invoke使用
    /// </summary>
    /// <param name="OutPutHT">需要返回的数据集合</param>
    public delegate void delegateForThreadShow(Hashtable temp);


    /// <summary>
    /// 远程调用对应类库，新版
    /// </summary>
    class WebServicesCenter2013
    {

                /// <summary>
        /// 配置文件webservice动态类名称
        /// </summary>
        string sjk;
        /// <summary>
        /// 程序集
        /// </summary>
        Assembly asm;
        /// <summary>
        /// 类型
        /// </summary>
        Type type;
        /// <summary>
        /// 激活器
        /// </summary>
        object instance;


        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns></returns>
        public string ReadConfig()
        {
            try
            {
                if (File.Exists("c:\\ConfigYHB.txt"))
                {
                    StreamReader sr = new StreamReader("c:\\ConfigYHB.txt", Encoding.Default);
                    string s;
                    string configStr = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        configStr = configStr + s + "|";
                    }
                    return configStr;
                }
                else
                {
                    return "|";
                }
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("读取配置文件c:\\ConfigYHB.txt错误：" + ex.ToString());
                return "|";
            }

        }


        public WebServicesCenter2013()
        {
            //初始化动态配置
            sjk = ReadConfig().Split('|')[0];
            asm = Assembly.GetExecutingAssembly();
            type = asm.GetType(sjk);
            instance = asm.CreateInstance(sjk);
        }



        /// <summary>
        /// 执行webservices，并返回数据，参数为object数组，绝大数据情况下，统一使用DataSet类型作为返回值。参数可根据情况处理。
        /// </summary>
        /// <param name="MethodName">远程方法名</param>
        /// <param name="CSarr">要传递的参数</param>
        /// <returns></returns>
        public DataSet RunAtServices(string MethodName ,object[] CSarr)
        {
            try
            {
                MethodInfo method = type.GetMethod(MethodName);
                return (DataSet)method.Invoke(instance, CSarr);

            }
            catch (Exception ex)
            {
                StringOP.WriteLog("调用webservices错误,方法名：[" + MethodName + "]：" + ex.ToString());
                return null;
            }
        }


    }
}
