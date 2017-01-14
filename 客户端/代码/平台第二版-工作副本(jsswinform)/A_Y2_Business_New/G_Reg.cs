using System.Collections;
using System.Threading;
using System.Data;
namespace 客户端主程序.NewDataControl
{
    public class G_Reg
    {
        delegateForThread dft;
        Hashtable InPutHT;
        public G_Reg(Hashtable pht, delegateForThread dfts)
        {
            dft = dfts;
            InPutHT = pht;
        }
        /// <summary>
        /// 首次注册
        /// </summary>
        public void FirstReg()
        {
            Thread.Sleep(1);
            WebServicesCenter2013 wsc = new WebServicesCenter2013();
            DataSet ds = wsc.RunAtServices("C用户注册", new object[] { InPutHT["dlyx"].ToString(), InPutHT["yhm"].ToString().Trim(), InPutHT["pwd"].ToString().Trim(), InPutHT["yzm"].ToString().Trim() });
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行结果"] = ds;
            dft(OutPutHT);
        }
        /// <summary>
        /// 后退或其他非首次提交的注册
        /// </summary>
        public void OtherReg()
        {
            Thread.Sleep(1);
            //DataControl.WebServicesCenter wsc = new DataControl.WebServicesCenter();
            WebServicesCenter2013 wsc = new WebServicesCenter2013();
            DataSet ds = wsc.RunAtServices("C更换邮箱", new object[] { InPutHT["pk"].ToString(), InPutHT["dlyx"].ToString(), InPutHT["yhm"].ToString().Trim(), InPutHT["pwd"].ToString().Trim(), InPutHT["yzm"].ToString().Trim() });
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行结果"] = ds;
            dft(OutPutHT);
        }
        /// <summary>
        /// 再次发送验证码
        /// </summary>
        public void SendAgain()
        {
            Thread.Sleep(1);
            WebServicesCenter2013 wsc = new WebServicesCenter2013();
            DataSet ds = wsc.RunAtServices("C再次发送验证码", new object[] { InPutHT["dlyx"].ToString(), InPutHT["valNum"].ToString() });
            Hashtable outPutHT = new Hashtable();
            outPutHT["执行结果"] = ds;
            dft(outPutHT);
        }
        /// <summary>
        /// 激活帐户
        /// </summary>
        public void AccountID()
        {
            Thread.Sleep(1);
            WebServicesCenter2013 wsc = new WebServicesCenter2013();
            DataSet ds = wsc.RunAtServices("C账户激活", new object[] { InPutHT["dlyx"].ToString() });
            Hashtable outPutHT = new Hashtable();
            outPutHT["执行结果"] = ds;
            dft(outPutHT);
        }

        //移植时想办法废掉，改正常分页方案
        public void GetQuery()
        {
            Thread.Sleep(1);
            WebServicesCenter2013 wsc = new WebServicesCenter2013();
            DataSet ds = wsc.RunAtServices("QueryFor", new object[] { InPutHT["SQL"].ToString() });
            Hashtable outPutHT = new Hashtable();
            outPutHT["执行结果"] = ds;
            dft(outPutHT);
        }
    }
}
