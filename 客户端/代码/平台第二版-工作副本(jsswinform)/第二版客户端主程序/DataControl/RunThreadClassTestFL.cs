using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;

namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 商品分类数据处理线程类
    /// </summary>
    class RunThreadClassTestFL
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
        public RunThreadClassTestFL(Hashtable PHT, delegateForThread DFT)
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
                Thread.Sleep(300);
 
                NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                //DataSet dstest = WSC2013.RunAtServices("test_dsfl", new object[] { "AAA_tbMenuSPFL" });
                //已移植可替换(已替换)
                DataSet dstest = WSC2013.RunAtServices("C获得一级分类", new object[] { "" });

                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["测试分类列表"] = dstest;//测试参数
                DForThread(OutPutHT);
              
        }


        /// <summary>
        /// 开始执行线程，获取二级分类进行显示
        /// </summary>
        public void BeginRun_FLerji()
        {
            Thread.Sleep(500);
 

            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //DataSet dstest = WSC2013.RunAtServices("GetSPFLerji", new object[] { InPutHT["父分类"].ToString() });
            //已移植可替换(已替换)
            DataSet dstest = WSC2013.RunAtServices("C获得二级分类", new object[] { InPutHT["父分类"].ToString() });

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["二级分类列表"] = dstest;//测试参数
            DForThread(OutPutHT);

        }

    }
}
