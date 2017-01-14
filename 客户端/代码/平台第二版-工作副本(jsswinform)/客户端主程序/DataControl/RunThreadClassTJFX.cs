using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;

namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 主页底部统计分析数据处理线程类
    /// </summary>
    class RunThreadClassTJFX
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
        public RunThreadClassTJFX(Hashtable PHT, delegateForThread DFT)
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
                //用于测试
                DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
                string[][] TJSJ = WSC.indexTJSJ();


                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["统计数据1"] = TJSJ[0];
                OutPutHT["统计数据2"] = TJSJ[1];
                OutPutHT["统计数据3"] = TJSJ[2];
                DForThread(OutPutHT);
              
        }
    }
}
