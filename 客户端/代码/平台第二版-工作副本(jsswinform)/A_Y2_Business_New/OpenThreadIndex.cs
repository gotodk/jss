using System.Collections;
using System.Threading;
using System.Data;
using 客户端主程序.DataControl;
namespace 客户端主程序.NewDataControl
{
    public class OpenThreadIndex
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
        public OpenThreadIndex(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

        /// <summary>
        /// 开始执行线程(检查对应经纪人是否休眠或冻结)
        /// </summary>
        public void BeginRun_JJR()
        {
            //防卡死，停5秒再跑
            Thread.Sleep(1);

            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            object[] cs = { PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("checkJJRzt", cs);
            DataSet dsreturn = WSC2013.RunAtServices("C当前经纪人状态", cs);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 开始执行线程(提醒)
        /// </summary>
        public void BeginRun()
        {
            //防卡死，停5秒再跑
            Thread.Sleep(5000);

            while (true)
            {
                //访问远程服务
                NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
                //InPutHT可以选择灵活使用
                object[] cs = { PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["登录邮箱"].ToString() };
                //DataSet dsreturn = WSC2013.RunAtServices("CheckFormTrayMsg", cs);
                //已移植可替换
                DataSet dsreturn = WSC2013.RunAtServices("C检查右下角提醒", cs);

                if (dsreturn != null && dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "要踢人")
                {
                    //填充传入参数哈希表
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["踢人消息"] = dsreturn.Tables["返回值单条"].Rows[0]["提示文本"].ToString();
                    DForThread(OutPutHT);
                }

                if (dsreturn != null && dsreturn.Tables["返回值单条"].Rows[0]["执行结果"].ToString() == "ok" && dsreturn.Tables.Contains("提醒数据") && dsreturn.Tables["提醒数据"].Rows.Count > 0)
                {
                    //填充传入参数哈希表
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["提醒内容"] = dsreturn;
                    DForThread(OutPutHT);
                }

                //每分钟检查一次
                Thread.Sleep(60000);
            }



        }
    }
}
