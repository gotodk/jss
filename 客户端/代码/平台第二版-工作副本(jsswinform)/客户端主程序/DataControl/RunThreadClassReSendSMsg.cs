using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

namespace 客户端主程序.DataControl
{
    class RunThreadClassReSendSMsg
    {
        private delegateForThread threading;
        private Hashtable inPutHT;
        public RunThreadClassReSendSMsg(Hashtable ht, delegateForThread dft)
        {
            threading = dft;
            inPutHT = ht;
        }

        public void runThreading()
        {
            Thread.Sleep(500);
            DataControl.WebServicesCenter wsc = new WebServicesCenter();
            DataSet ds = wsc.SendValNumber(inPutHT["dlyx"].ToString().Trim(), inPutHT["typeToSet"].ToString().Trim(), inPutHT["RandomNumber"].ToString().Trim());
            Hashtable outPutHT = new Hashtable();
            outPutHT["执行结果"] = ds;
            threading(outPutHT);
        }
    }
}
