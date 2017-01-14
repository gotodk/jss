using System.Collections;
using System.Threading;
using System.Data;
using 客户端主程序.Support;

namespace 客户端主程序.NewDataControl
{
    public class OpenThreadMyXls
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
        public OpenThreadMyXls(Hashtable PHT, delegateForThread DFT)
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

            DataSet dsreturn = new DataSet();
            //if (InPutHT.Contains("tiaojian") && InPutHT["tiaojian"]!=null)
            //{//新架构下的方法
                Hashtable ht_tiaojian = (Hashtable)InPutHT["tiaojian"];
                string methodname = InPutHT["webmethod"].ToString();
                string[][] str_params = StringOP.GetstrArryFromHashtable(ht_tiaojian);
                //string[] HideColumns = (string[])InPutHT["HideColumns"];
                //byte[] b_return = WSC2013.RunAtServices_YS("导出统一接口", new object[] { methodname, str_params, (string[])InPutHT["HideColumns"] });
                byte[] b_return = WSC2013.RunAtServices_YS("导出统一接口", new object[] { methodname, str_params});
               
                if (b_return == null)
                {
                    dsreturn = null;
                }
                else
                {
                    dsreturn = Helper.Byte2DataSet(b_return);
                }
            //}
            //else
            //{//沿用老架构的方法 
            //    //InPutHT可以选择灵活使用
            //    object[] cs = { InPutHT["sql"].ToString(), (string[])InPutHT["HideColumns"] };
            //    dsreturn = WSC2013.RunAtServices("CMyXls", cs);
            //}

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }
    }
}
