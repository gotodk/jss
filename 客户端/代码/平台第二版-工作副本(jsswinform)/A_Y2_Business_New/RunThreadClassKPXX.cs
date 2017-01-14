using System.Collections;
using System.Data;
using System.Threading;
using 客户端主程序.DataControl;
using 客户端主程序.Support;

namespace 客户端主程序.NewDataControl
{
    public class RunThreadClassKPXX
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
        public RunThreadClassKPXX(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }
        /// <summary>
        /// 开始执行获取开票信息线程
        /// </summary>
        public void BeginRunGetInfo()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);

            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            object[] cs = { InPutHT["dlyx"].ToString() };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("GetKPXX", cs);
            DataSet dsreturn = WSC2013.RunAtServices("C加载开票信息", cs);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 开始执行提交开票信息线程
        /// </summary>
        public void BeginRunCommit()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);

            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);

            dataTable.Columns.Add("买家角色编号");
            dataTable.Rows[0]["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();

            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("CommitKPXX", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C提交修改开票信息", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }
        /// <summary>
        /// 开始执行提交开票信息变更线程
        /// </summary>
        public void BeginRunCommitKPXXChange()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);

            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            dataTable.Columns.Add("买家角色编号");
            dataTable.Rows[0]["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();

            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("CommitKPXXChange", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C变更修改开票信息", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }
    }
}
