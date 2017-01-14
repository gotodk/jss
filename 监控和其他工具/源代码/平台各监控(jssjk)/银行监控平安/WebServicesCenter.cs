using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Configuration;

namespace 银行监控平安
{
    /// <summary>
    /// 远程调用对应类库，新版
    /// </summary>
    /// 
    class WebServicesCenter
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
        

        public WebServicesCenter()
        {
            //初始化动态配置
            sjk = ConfigurationManager.AppSettings["WebAdd"].ToString();
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
        public DataSet RunAtServices(string MethodName, object[] CSarr)
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
