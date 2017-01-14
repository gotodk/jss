using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

namespace ClassLibraryBusinessMonitor
{
 public   class RunThreadClassWCQS
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
        public RunThreadClassWCQS(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

        /// <summary>
        /// 得到用户转账的信息
        /// </summary>
        public void WCQS()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
           WebServicesCenter2013 WSC2013 = new WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            DataTable dt = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] cs = { dt };
            DataSet dsreturn = WSC2013.RunAtServices("WCQS", cs);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }


    }
}
