using System.Collections;
using System.Threading;
using System.Data;
using 客户端主程序.Support;
namespace 客户端主程序.NewDataControl
{
    /// <summary>
    /// 用于激活休眠的账户
    /// </summary>
    public class OpenTheadAccontActivate
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
        public OpenTheadAccontActivate(Hashtable PHT, delegateForThread DFT)
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
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            DataTable dt = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] cs = { dt };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("AccountActivate", cs);
            DataSet dsreturn = WSC2013.RunAtServices("C休眠激活", cs);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }
    }
}
