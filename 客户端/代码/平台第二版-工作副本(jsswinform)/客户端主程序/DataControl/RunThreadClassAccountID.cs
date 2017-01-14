using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;

namespace 客户端主程序.DataControl
{
    /// <summary>
    /// 激活帐户
    /// </summary>
    class RunThreadClassAccountID
    {
        private delegateForThread threading;
        private Hashtable inPutHT;
        public RunThreadClassAccountID(Hashtable ht, delegateForThread dft)
        {
            threading = dft;
            inPutHT = ht;
        }
        public void runThreading()
        {
            Thread.Sleep(500);
            DataControl.WebServicesCenter wsc = new WebServicesCenter();
            DataSet ds = wsc.AccountID(inPutHT["dlyx"].ToString(), inPutHT["valNum"].ToString());
            Hashtable outPutHT = new Hashtable();
            outPutHT["执行结果"] = ds;
            threading(outPutHT);

        }

    }
}
