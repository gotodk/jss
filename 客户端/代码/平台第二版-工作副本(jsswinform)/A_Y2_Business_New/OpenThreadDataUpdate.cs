using System.Collections;
using System.Threading;
using System.Data;
using 客户端主程序.Support;
using 客户端主程序.DataControl;


namespace 客户端主程序.NewDataControl
{
    public class OpenThreadDataUpdate
    {
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;

        private string methodName;

        public OpenThreadDataUpdate(Hashtable PHT, delegateForThread DFT, string name)
        {
            DForThread = DFT;
            InPutHT = PHT;
            methodName = name;
        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        /// <param name="merhodname">方法名</param>
        public void BeginRun()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            DataTable dt = StringOP.GetDataTableFormHashtable(InPutHT);

            if (!dt.Columns.Contains("买家角色编号"))
            {
                dt.Columns.Add("买家角色编号");
                dt.Rows[0]["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            }

            object[] cs = { dt };
            DataSet dsreturn = WSC2013.RunAtServices(methodName, cs);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }
    }
}
