using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Data;

namespace 客户端主程序.DataControl
{
    class RunThreadClassDataGet
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
        public RunThreadClassDataGet(Hashtable PHT, delegateForThread DFT)
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
            DataControl.WebServicesCenter WSC = new DataControl.WebServicesCenter();
            string where = InPutHT["where"].ToString().Trim();
            try
            {
                DataSet dstest = WSC.GetDataSet(where);

                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["返回数据"] = dstest;//测试参数
                DForThread(OutPutHT);

            }
            catch
            {
                ArrayList AlmsgMR = new ArrayList();
                AlmsgMR.Add("");
                AlmsgMR.Add("请检查网络，稍后再试！");
                FormAlertMessage FRSEMR = new FormAlertMessage("仅确定", "叹号", "提醒", AlmsgMR);
                FRSEMR.ShowDialog();
            }          

            
        }
    }
}
