using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;


namespace 客户端主程序.NewDataControl
{
    public  class RunTheadGetSPFL
    {
         //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;

        /// <summary>
        /// 构造函数
        /// </summary>
    
        /// <param name="DFT">线程委托</param>
        public RunTheadGetSPFL( delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
           
        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        public void BeginRun()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            //已移植可替换(已替换) wyh 2014.0711
           // DataSet dsreturn = WSC2013.RunAtServices("GetSPFL", null);
            DataSet dsreturn = WSC2013.RunAtServices("C商品分类", new object[] { "AAA_tbMenuSPFL" });
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }
    }
}
