using FMipcClass;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using 客户端主程序.Support;

namespace 客户端主程序.NewDataControl
{
    /// <summary>
    /// 远程调用对应类库，新版
    /// </summary>
    public class WebServicesCenter2013
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

        public WebServicesCenter2013()
        {
            //初始化动态配置
            sjk = DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[1];
            asm = Assembly.GetExecutingAssembly();
            type = asm.GetType(sjk);
 
            instance = asm.CreateInstance(sjk);

   

            //带参数列子return (DataSet)method.Invoke(instance, new object[] { "hello word!" } );
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
                if (ValStr.haveCN(MethodName))
                {
                    //至少要有一个参数
                    if (CSarr.Length == 0)
                    {
                        return null;
                    }

                    //自动增加防篡改参数
                    string m = StringOP.GetMD5(CSarr);
                    object[] CSarrNew = new object[CSarr.Length + 1];
                    CSarr.CopyTo(CSarrNew, 0);
                    CSarrNew[CSarrNew.Length - 1] = m;
                    CSarr = CSarrNew;

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
                        Support.StringOP.WriteLog("调用webservices错误,方法名：[" + MethodName + "]：" + reFF);
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
                Support.StringOP.WriteLog("调用webservices错误,方法名：[" + MethodName + "]：" + ex.ToString());
                return null;
            }
        }



        /// <summary>
        /// 获取分页数据专用 
        /// </summary>
        /// <param name="HTwhere">条件参数哈希表</param>
        /// <returns></returns>
        //public DataSet GetPagerDB(Hashtable HTwhere)
        //{
        //    try
        //    {
            
        //        //处理默认值
        //        string GetCustomersDataPage_NAME = "";
        //        if (HTwhere.ContainsKey("GetCustomersDataPage_NAME"))
        //        {
        //            GetCustomersDataPage_NAME = HTwhere["GetCustomersDataPage_NAME"].ToString();
        //        }
        //        string this_dblink = "";
        //        if (HTwhere.ContainsKey("this_dblink"))
        //        {
        //            this_dblink = HTwhere["this_dblink"].ToString();
        //        }
        //        string page_index = "";
        //        if (HTwhere.ContainsKey("page_index"))
        //        {
        //            page_index = HTwhere["page_index"].ToString();
        //        }
        //        string page_size = "";
        //        if (HTwhere.ContainsKey("page_size"))
        //        {
        //            page_size = HTwhere["page_size"].ToString();
        //        }
        //        string serach_Row_str = "";
        //        if (HTwhere.ContainsKey("serach_Row_str"))
        //        {
        //            serach_Row_str = HTwhere["serach_Row_str"].ToString();
        //        }
        //        string search_tbname = "";
        //        if (HTwhere.ContainsKey("search_tbname"))
        //        {
        //            search_tbname = HTwhere["search_tbname"].ToString();
        //        }
        //        string search_mainid = "";
        //        if (HTwhere.ContainsKey("search_mainid"))
        //        {
        //            search_mainid = HTwhere["search_mainid"].ToString();
        //        }
        //        string search_str_where = "";
        //        if (HTwhere.ContainsKey("search_str_where"))
        //        {
        //            search_str_where = HTwhere["search_str_where"].ToString();
        //        }
        //        string search_paixu = "";
        //        if (HTwhere.ContainsKey("search_paixu"))
        //        {
        //            search_paixu = HTwhere["search_paixu"].ToString();
        //        }
        //        string search_paixuZD = "";
        //        if (HTwhere.ContainsKey("search_paixuZD"))
        //        {
        //            search_paixuZD = HTwhere["search_paixuZD"].ToString();
        //        }
        //        string count_float = "";
        //        if (HTwhere.ContainsKey("count_float"))
        //        {
        //            count_float = HTwhere["count_float"].ToString();
        //        }
        //        string count_zd = "";
        //        if (HTwhere.ContainsKey("count_zd"))
        //        {
        //            count_zd = HTwhere["count_zd"].ToString();
        //        }
        //        string cmd_descript = "";
        //        if (HTwhere.ContainsKey("cmd_descript"))
        //        {
        //            cmd_descript = HTwhere["cmd_descript"].ToString();
        //        }

        //        string method_retreatment = "";
        //        if (HTwhere.ContainsKey("method_retreatment"))
        //        {
        //            method_retreatment = HTwhere["method_retreatment"].ToString();
        //        }


        //        object re = RunAtServicesBase("GetPagerDB", new object[] { GetCustomersDataPage_NAME, this_dblink, page_index, page_size, serach_Row_str, search_tbname, search_mainid, search_str_where, search_paixu, search_paixuZD, count_float, count_zd, cmd_descript, method_retreatment });
        //        if (re == null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return (DataSet)re;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
        //        return null;
        //    }
        //}

        /// <summary>
        /// 新的获取分页数据方法
        /// </summary>
        /// <param name="HTwhere">条件参数哈希表</param>
        /// <returns></returns>
        public DataSet NewGetPagerDB(Hashtable HTwhere)
        {
            try
            {                
                //处理默认值               
                string page_index = "";
                if (HTwhere.ContainsKey("page_index"))
                {
                    page_index = HTwhere["page_index"].ToString();
                }
                string page_size = "";
                if (HTwhere.ContainsKey("page_size"))
                {
                    page_size = HTwhere["page_size"].ToString();
                }
                string webmethod = "";
                if (HTwhere.ContainsKey("webmethod"))
                {
                    webmethod = HTwhere["webmethod"].ToString();
                }                
                string[][] tiaojian = null;
                if (HTwhere.ContainsKey("tiaojian"))
                {
                    Hashtable ht_tiaojian = (Hashtable)HTwhere["tiaojian"];
                    tiaojian = StringOP.GetstrArryFromHashtable(ht_tiaojian);
                }

                string[] page = new string[] { page_index, page_size };
                DataSet ds_result = RunAtServices("分页统一接口", new object[] { webmethod, page, tiaojian });
                return ds_result;               
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("调用webservices错误：" + ex.ToString());
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
