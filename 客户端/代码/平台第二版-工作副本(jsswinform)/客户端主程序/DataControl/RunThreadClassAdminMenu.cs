using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;

namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 后台菜单处理线程类
    /// </summary>
    class RunThreadClassAdminMenu
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
        public RunThreadClassAdminMenu(Hashtable PHT, delegateForThread DFT)
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
            Thread.Sleep(500);
            WebServicesCenter WSC = new WebServicesCenter();
            DataSet dsmenu = WSC.test_dsfl("ZZ_tbMenuAdmin");

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["后台菜单"] = dsmenu;//测试参数
            DForThread(OutPutHT);
        }


       


    }
}
