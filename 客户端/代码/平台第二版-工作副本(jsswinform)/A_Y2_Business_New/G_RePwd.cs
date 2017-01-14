using System.Collections;
using System.Data;
using System.Threading;

namespace 客户端主程序.NewDataControl
{
    public class G_RePwd
    {
        delegateForThread dft;
        Hashtable InPutHT;
        public G_RePwd(Hashtable pht, delegateForThread dfts)
        {
            dft = dfts;
            InPutHT = pht;
        }
        /// <summary>
        /// 提交忘记密码申请，发送验证码
        /// </summary>
        public void RePwd()
        {
            Thread.Sleep(5);
            WebServicesCenter2013 wsc = new WebServicesCenter2013();
            DataSet ds = wsc.RunAtServices("C忘记密码验证码", new object[] { InPutHT["dlyx"].ToString().Trim(), InPutHT["typeToSet"].ToString().Trim(), InPutHT["RandomNumber"].ToString().Trim() });
            Hashtable outPutHT = new Hashtable();
            outPutHT["执行结果"] = ds;
            dft(outPutHT);
        }

        /// <summary>
        /// 真正的修改密码
        /// </summary>
        public void ChangePwd()
        {
            Thread.Sleep(5);
            WebServicesCenter2013 wsc = new WebServicesCenter2013();
            DataSet ds = wsc.RunAtServices("C忘记密码修改密码", new object[] { InPutHT["dlyx"].ToString().Trim(), InPutHT["types"].ToString().Trim(), InPutHT["pwd"].ToString().Trim(), InPutHT["valNum"].ToString().Trim(), InPutHT["typeToSet"].ToString().Trim() });
            Hashtable outPutHT = new Hashtable();
            outPutHT["执行结果"] = ds;
            dft(outPutHT);
        }
    }
}
