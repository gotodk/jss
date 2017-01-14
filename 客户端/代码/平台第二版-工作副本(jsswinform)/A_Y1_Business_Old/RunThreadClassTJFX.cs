using System;
using System.Collections;
using System.Threading;

namespace 客户端主程序.DataControl
{

    /// <summary>
    /// 主页底部统计分析数据处理线程类
    /// </summary>
    public class RunThreadClassTJFX
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
        public RunThreadClassTJFX(Hashtable PHT, delegateForThread DFT)
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
            string[][] TJSJ_old = null;
            while (true)
            {

                Thread.Sleep(300);
 
                NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                //string[][] TJSJ = WSC2013.RunAtServices_DB("indexTJSJ", null);
                //已移植可替换(已替换)
                string[][] TJSJ = WSC2013.RunAtServices_DB("C大盘底部统计", new object[] { "" });

                if (TJSJ == null || TJSJ.Length < 1 || TJSJ[0].Length < 1)
                {
                    Thread.Sleep(5000);
                    continue;

                }
                if (TJSJ != null && TJSJ.Length == 4)
                {
                    try
                    {
                        PublicDS.PublisTimeServer = Convert.ToDateTime(TJSJ[3][1]);
                    }
                    catch (Exception exx)
                    {
                        //报错后，返回本机时间
                        PublicDS.PublisTimeServer = DateTime.Now;
                    }

                }
      

                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["统计数据1"] = TJSJ[0];
                OutPutHT["统计数据2"] = TJSJ[1];
                OutPutHT["统计数据3"] = TJSJ[2];
                if (TJSJ_old == null)
                {
                    TJSJ_old = TJSJ;
                    DForThread(OutPutHT);
                }
                else
                {
                    //判定新数据和旧数据是否一致，如果一致，不再更新界面
                    if (!String.Join(",", TJSJ[0]).Equals(String.Join(",", TJSJ_old[0])) && !String.Join(",", TJSJ[1]).Equals(String.Join(",", TJSJ_old[1])) && !String.Join(",", TJSJ[2]).Equals(String.Join(",", TJSJ_old[2])))
                    {
                        TJSJ_old = TJSJ;
                        DForThread(OutPutHT);
                    }
                }

               
                Thread.Sleep(3000);
            }

        }
    }
}
