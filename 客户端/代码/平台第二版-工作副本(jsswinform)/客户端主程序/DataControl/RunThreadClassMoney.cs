using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;

namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 测试首页数据表加载线程类
    /// </summary>
    class RunThreadClassMoney
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
        public RunThreadClassMoney(Hashtable PHT, delegateForThread DFT)
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
            //用于测试
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            string  mymoney = WSC.GetMyMoney(InPutHT["邮箱"].ToString(),"");
            string remoney = "";
            string jieguo = "";
            if (mymoney.IndexOf("|ok|") >= 0)
            {
                jieguo = "ok";
                remoney = mymoney.Replace("|ok|", "");
            }
            else
            {
                jieguo = mymoney;
            }
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行结果"] = jieguo;//测试参数
            OutPutHT["余额"] = remoney;
            DForThread(OutPutHT);
        }
    }
}
