using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using 客户端主程序.DataControl;
using 客户端主程序.Support;

namespace 客户端主程序.NewDataControl
{
    public class DPSPXQ
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
        public DPSPXQ(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

        /*
        /// <summary>
        /// 开始执行线程(旧版)
        /// </summary>
        public void BeginRun()
        {
            while (Thread.CurrentThread.Priority != ThreadPriority.Lowest)
            {
                //防卡死，停100毫秒
                Thread.Sleep(500);
                //Support.StringOP.WriteLog("临时调试：" + Thread.CurrentThread.Name);
                //访问远程服务
                NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                //InPutHT可以选择灵活使用
                object[] cs = new object[] { InPutHT };
                Hashtable ht = new Hashtable();
                ht.Add("时间", "System.DateTime");
                ht.Add("即时交易", "System.Double");
                ht.Add("三个月合同", "System.Double");
                ht.Add("一年合同", "System.Double");
                ht.Add("账户当前信用分值", "System.Double"); 
                //DataSet dsreturn = Helper.Byte2DataSet(WSC2013.RunAtServices_YS("SelectSPXQ", cs),ht);
                //已移植可替换(已替换)
                DataSet dsreturn = Helper.Byte2DataSet(WSC2013.RunAtServices_YS("C获得商品详情页面", cs), ht);
                //填充传入参数哈希表
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["返回值"] = dsreturn;
                if (Thread.CurrentThread.Priority == ThreadPriority.Lowest) //用低级别线程特殊标记该停止了
                { return; }
                DForThread(OutPutHT);
                Thread.Sleep(5000);


                return;
            }            

        }
        */


        /// <summary>
        /// 新版，获得商品详情页面的数据
        /// </summary>
        public void BeginRun2014()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);

                
                //访问远程服务
                NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                string DLYX = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString().Trim();
                object[] cs = new object[] { InPutHT["商品编号"].ToString(), InPutHT["合同期限"].ToString(), DLYX };
                byte[] b = WSC2013.RunAtServices_YS("C获得商品详情页面", cs);
                if (b == null)
                {
                    return;
                }
                DataSet dsreturn = Helper.Byte2DataSet(b);

                if (dsreturn == null)
                {
                    return;
                }

              
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["返回值"] = dsreturn;
                DForThread(OutPutHT);

         

            


        }
    }
}
