using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

namespace 客户端主程序.DataControl
{
    class RunThreadClassUserChangeRegister
    {
        delegateForThread dft;
        Hashtable InPutHT;
        public RunThreadClassUserChangeRegister(Hashtable pht, delegateForThread dfts)
        {
            dft = dfts;
            InPutHT = pht;
        }
        public void beginRun()
        {
            Thread.Sleep(500);
            DataControl.WebServicesCenter wsc = new DataControl.WebServicesCenter();
            DataSet ds = wsc.UserChangeRegister(InPutHT["pk"].ToString(), InPutHT["dlyx"].ToString(), InPutHT["yhm"].ToString().Trim(), InPutHT["pwd"].ToString().Trim(), InPutHT["yzm"].ToString().Trim());
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行结果"] = ds;
            dft(OutPutHT);
        }
    }
}
