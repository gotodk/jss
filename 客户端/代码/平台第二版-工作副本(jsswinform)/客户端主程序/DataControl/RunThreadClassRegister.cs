using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;

namespace 客户端主程序.DataControl
{
    class RunThreadClassRegister
    {
        delegateForThread dft;
        Hashtable InPutHT;
        public RunThreadClassRegister(Hashtable pht, delegateForThread dfts)
        {
            dft = dfts;
            InPutHT = pht;
        }
        public void beginRun()
        {
            Thread.Sleep(500);
            DataControl.WebServicesCenter wsc = new DataControl.WebServicesCenter();
            DataSet ds = wsc.UserRegister(InPutHT["dlyx"].ToString(), InPutHT["yhm"].ToString().Trim(), InPutHT["pwd"].ToString().Trim(), InPutHT["yzm"].ToString().Trim());
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行结果"] = ds;
            dft(OutPutHT);
        }
    }
}
