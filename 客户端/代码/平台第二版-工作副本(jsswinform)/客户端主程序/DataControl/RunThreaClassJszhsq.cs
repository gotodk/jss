using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
using 客户端主程序.Support;

namespace 客户端主程序.DataControl
{

     
    class RunThreaClassJszhsq
    {
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;
       
        public RunThreaClassJszhsq(Hashtable PHT, delegateForThread DFT)
        {
            InPutHT = PHT;
            DForThread = DFT;
           
        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        public void BeginRun()
        {
            Thread.Sleep(500);
            DataTable dt = StringOP.GetDataTableFormHashtable(InPutHT);
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            string dstest = WSC.jszhzx(dt,"insert");

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["登陆结果"] = dstest;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        public void BeginRun_GetBasicInfo()
        {
            Thread.Sleep(500);
           // DataTable dt = StringOP.GetDataTableFormHashtable(InPutHT);
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            DataSet resultds = WSC.Get_BasicInfo(InPutHT ["dlyx"].ToString (), InPutHT["结算账户类型"].ToString());

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = resultds;
            DForThread(OutPutHT);

        }

       

         
    }
}
