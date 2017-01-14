using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;

namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 测试提醒检查线程类
    /// </summary>
    class RunThreadClassTrayMsg
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
        public RunThreadClassTrayMsg(Hashtable PHT, delegateForThread DFT)
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
            while (true)
            {
                Thread.Sleep(500);
                //检查一次有没有新的提醒
                WebServicesCenter WSC = new WebServicesCenter();
                string msg = WSC.GetTrayMsg(PublicDS.PublisDsUser.Tables[0].Rows[0]["DLYX"].ToString());
                //若有提醒，弹出提示
                if (msg != "")
                {
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["提醒内容"] = msg;//测试参数
                    DForThread(OutPutHT);
                    
                }
                Thread.Sleep(10000);
            }
        }
    }
}
