using System.Threading;
using System.Data;
using System.Collections;

namespace 客户端主程序.NewDataControl
{
   public class SCFHD
    {
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private DataSet InPutHT;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PHT">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public SCFHD(DataSet PHT, delegateForThread DFT)
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
            //访问远程服务
            WebServicesCenter2013 WSC2013 = new WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            object[] cs = { InPutHT };
            //DataSet dsreturn = WSC2013.RunAtServices("SCFHD", cs);
            //已移植可替换(已替换)
            DataSet dsreturn = WSC2013.RunAtServices("C生成发货单", cs);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }
    }
}
