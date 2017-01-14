using System.Collections;
using System.Threading;
using System.Data;
using 客户端主程序.Support;
using System.IO;
using System;

namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 测试首页数据表加载线程类
    /// </summary>
    public class RunThreadClassCityList
    {
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PHT">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public RunThreadClassCityList(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        public void BeginRun()
        {

            //防卡死，停100毫秒
            Thread.Sleep(100);
            DataSet dsreturn = null;
            byte[] initb = null;
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();

            try 
            {
                initb = File.ReadAllBytes(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\initfile.dll");
                dsreturn = Helper.Byte2DataSet(initb);
            }
            catch(Exception ex)
            {
                try
                {
                    StringOP.WriteLog(ex.ToString());
                    //initb = WSC2013.RunAtServices_YS("GetPublisDsData_YS", null);
                    //已移植可替换(已替换)
                    initb = WSC2013.RunAtServices_YS("C初始化数据", new object[] { "" });

                    dsreturn = Helper.Byte2DataSet(initb);
                    File.WriteAllBytes(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\initfile.dll", initb);
                }
                catch { }
            }


            //获取失败要弄成空的
            if (initb == null || initb.Length == 0)
            {
                dsreturn = null;
            }


            //测试接口存活情况
            //string livenow = WSC2013.RunAtServices_ZX("C测试接口存活", new object[] { "" });
            //if (livenow != "ok")
            //{
            //    dsreturn = null;
            //}

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["前置数据"] = dsreturn;//测试参数
            DForThread(OutPutHT);
        }
    }
}
