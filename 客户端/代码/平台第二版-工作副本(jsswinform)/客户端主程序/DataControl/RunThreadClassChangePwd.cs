using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;
namespace 客户端主程序.DataControl
{
    /// <summary>
    /// 忘记密码（修改密码）
    /// </summary>
    class RunThreadClassChangePwd
    {
        private delegateForThread threading;
        private Hashtable inPutHT;
        public RunThreadClassChangePwd(Hashtable ht, delegateForThread dft)
        {
            threading = dft;
            inPutHT = ht;
        }

        public void runThreading()
        {
            Thread.Sleep(500);
            DataControl.WebServicesCenter wsc = new WebServicesCenter();
            DataSet ds = wsc.ChangePassword(inPutHT["dlyx"].ToString().Trim(), inPutHT["types"].ToString().Trim(), inPutHT["pwd"].ToString().Trim(), inPutHT["valNum"].ToString().Trim(), inPutHT["typeToSet"].ToString().Trim());
            Hashtable outPutHT = new Hashtable();
            outPutHT["执行结果"] = ds;
            threading(outPutHT);
        }

    }
}
