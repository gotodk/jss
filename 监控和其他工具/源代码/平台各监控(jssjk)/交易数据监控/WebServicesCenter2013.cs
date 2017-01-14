using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using FMipcClass;

namespace 交易数据监控
{
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
                string peizhi = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\ConfigYHB.txt";
                if (File.Exists(peizhi))
                {
                    StreamReader sr = new StreamReader(peizhi, Encoding.Default);
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
                    StringOP.WriteLog("读取配置文件有问题","配置文件错误日志");
                    return "|";
                }
            }
            catch (Exception ex)
            {
                StringOP.WriteLog("读取配置文件错误：" + ex.ToString(),"配置文件错误日志");
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
        /// 验证字符串中是否存在中文字符。真 为 存在至少一个中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool haveCN(string str)
        {
            Regex reg = new Regex(@"[\u4e00-\u9fa5]");//正则表达式
            if (reg.IsMatch(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 执行webservices的基础方法。
        /// </summary>
        /// <param name="MethodName">远程方法名</param>
        /// <param name="CSarr">要传递的参数</param>
        /// <returns></returns>
        public object RunAtServicesBase(string MethodName, object[] CSarr)
        {
            try
            {
                //方法名中有中文的(都移植完这个判断就可以去掉了)
                if (haveCN(MethodName))
                {
                    //至少要有一个参数
                    if (CSarr.Length == 0)
                    {
                        return null;
                    }

 

                    //====如何调用一个远程接口=========================================================================
                    //统一的调用方式，第一个参数就是方法的中文业务名称(聚合中心配置)。参数和返回值，目前只允许string|string[]|DataSet
                    object[] re = IPC.Call(MethodName, CSarr);
                    //远程方法执行成功（这里的成功，仅仅是程序上的运行成功。只代表程序运行没有出错，不代表业务成功。业务是否成功，需要根据每个方法自己定义的返回值规则来定。按照预定应该是yewuok:xxx或者yewuerr:xxxx）
                    if (re[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                        return (re[1]);

                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                        string reFF = re[1].ToString();

                        return null;
                    }
                    //===============================================================================
                }
                else
                {
                    MethodInfo method = type.GetMethod(MethodName);
                    return method.Invoke(instance, CSarr);
                }




            }
            catch (Exception ex)
            {
                StringOP.WriteLog("调用webservices错误,方法名：[" + MethodName + "]：" + ex.ToString(), "调用WebService错误日志");
                return null;
            }
        }




        /// <summary>
        /// 执行webservices，并返回数据，参数为object数组，绝大数据情况下，统一使用DataSet类型作为返回值。参数可根据情况处理。
        /// </summary>
        /// <param name="MethodName">远程方法名</param>
        /// <param name="CSarr">要传递的参数</param>
        /// <returns></returns>
        public DataSet RunAtServices(string MethodName, object[] CSarr)
        {
            object re = RunAtServicesBase(MethodName, CSarr);
            if (re == null)
            {
                return null;
            }
            else
            {
                return (DataSet)re;
            }
        }


        /// <summary>
        /// 执行webservices，并返回数据，参数为object数组，统一使用byte[]类型作为返回值。参数可根据情况处理。
        /// </summary>
        /// <param name="MethodName">远程方法名</param>
        /// <param name="CSarr">要传递的参数</param>
        /// <returns></returns>
        public byte[] RunAtServices_YS(string MethodName, object[] CSarr)
        {
            object re = RunAtServicesBase(MethodName, CSarr);
            if (re == null)
            {
                return null;
            }
            else
            {
                return (byte[])re;
            }
        }


        /// <summary>
        /// 执行webservices，并返回数据，参数为object数组，统一使用byte[]类型作为返回值。参数可根据情况处理。
        /// </summary>
        /// <param name="MethodName">远程方法名</param>
        /// <param name="CSarr">要传递的参数</param>
        /// <returns></returns>
        public string[][] RunAtServices_DB(string MethodName, object[] CSarr)
        {
            object re = RunAtServicesBase(MethodName, CSarr);
            if (re == null)
            {
                return null;
            }
            else
            {
                return (string[][])re;
            }
        }

        /// <summary>
        /// 执行webservices，并返回数据，参数为object数组，统一使用byte[]类型作为返回值。参数可根据情况处理。
        /// </summary>
        /// <param name="MethodName">远程方法名</param>
        /// <param name="CSarr">要传递的参数</param>
        /// <returns></returns>
        public string RunAtServices_ZX(string MethodName, object[] CSarr)
        {
            object re = RunAtServicesBase(MethodName, CSarr);
            if (re == null)
            {
                return null;
            }
            else
            {
                return (string)re;
            }
        }



    }
}
