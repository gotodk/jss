using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.Collections;

namespace 客户端主程序.DataControl
{
    class RunThreadClassUpdateJJR
    {
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private DataSet InPutHT;
        private string Mt;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PHT">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public RunThreadClassUpdateJJR(DataSet PHT, delegateForThread DFT,string Method)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
            Mt = Method;
        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        public void BeginRun()
        {
            Thread.Sleep(500);
            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();         

            //调用服务
            DataSet returnstr = WSC.UpdateJJR(InPutHT,Mt);          

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = returnstr;//测试参数
            DForThread(OutPutHT);

        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        public void BeginRunResubmit()
        {
            Thread.Sleep(500);
            //尽量将全套数据处理放到服务器端进行，不要再这里处理业务逻辑。个别情况例外。
            //执行处理前，如果遇到了处理一些东西可能导致网页超时的情况，可能还要直接在这里调用webservices进行运算
            //这种情况可以直接将一些运算逻辑放到这里，而不需要再开新线程。 用于减少服务器端的压力。
            //但由于这里没有事务，所以这里只能进行数据读取和分析操作，绝不能更新数据库

            //更新数据库
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();

            //调用服务
            DataSet returnstr = WSC.UpdateJJR(InPutHT, Mt);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行状态"] = returnstr;//测试参数
            DForThread(OutPutHT);

        }
    }
}
