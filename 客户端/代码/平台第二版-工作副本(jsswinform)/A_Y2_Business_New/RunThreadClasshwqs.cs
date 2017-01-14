using System.Collections;
using System.Data;
using System.Threading;
using 客户端主程序.Support;

namespace 客户端主程序.NewDataControl
{
    public class RunThreadClasshwqs
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
        public RunThreadClasshwqs(Hashtable PHT, delegateForThread DFT)
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
            //DataSet dsreturn = WSC2013.RunAtServices("Jhjx_hwqs", cs);
            //已移植可替换(已替换)
            DataSet dsreturn = WSC2013.RunAtServices("C货物签收", cs);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }

        /// <summary>
        /// 获取定标保证函详细信息
        /// </summary>
        public void TBDYDDbzh_GetCGinfo()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            object[] cs = { InPutHT["Number"].ToString() };
            //DataSet dsreturn = wsc.RunAtServices("TBDYDDbzh_GetCGinfo", cs);
            //已移植可替换(已替换)
            DataSet dsreturn = wsc.RunAtServices("C清盘C区定标保证函详细信息", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;
            DForThread(OutPutHT);
        }


        public void DBZBXXinfoget()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            object[] cs = { InPutHT["Number"].ToString() };
            //DataSet dsreturn = wsc.RunAtServices("DBZBXXinfoget", cs);
            //已移植可替换(已替换)
            DataSet dsreturn = wsc.RunAtServices("C清盘C区获取中标定标表的信息", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }


    }
}
