using System.Collections;
using System.Data;
using System.Threading;

namespace 客户端主程序.NewDataControl
{
    public class G_Login
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
        public G_Login(Hashtable PHT, delegateForThread DFT)
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

            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            object[] cs = { InPutHT["账号"].ToString(), InPutHT["密码"].ToString() };
            DataSet dsreturn = WSC2013.RunAtServices("C登录账户验证", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["登录结果"] = dsreturn;
            DForThread(OutPutHT);
        }

  
    }
}
