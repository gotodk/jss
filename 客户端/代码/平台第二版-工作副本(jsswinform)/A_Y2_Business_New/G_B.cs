using System.Collections;
using System.Data;
using System.Threading;
using 客户端主程序.DataControl;

namespace 客户端主程序.NewDataControl
{
    public class G_B
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
        public G_B(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }
        /// <summary>
        /// 获取关联经纪人的管理机构
        /// </summary>
        public void GetGLJJRGLJG()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            object[] cs = { InPutHT["DLYX"].ToString() };
            //C关联经纪人平台管理结构
            //已移植可替换(已替换) wyh 2014.0711
            //DataSet dsreturn = WSC2013.RunAtServices("GetGLJJRGLJG", cs);
            DataSet dsreturn = WSC2013.RunAtServices("C关联经纪人平台管理结构", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 下达预订单
        /// </summary>
        public void SetYDD()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch(); stopwatch.Start(); //  开始监视代码运行时间

            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            DataTable dt = 客户端主程序.Support.StringOP.GetDataTableFormHashtable(InPutHT);
            dt.TableName = "YDD";//命名为预订单
            DataSet ds = new DataSet("YDDds");
            ds.Tables.Add(dt);
            object[] cs = { ds };
           // DataSet dsreturn = wsc.RunAtServices("SetYDD", cs);
            //已移植可替换(已替换)
            DataSet dsreturn = wsc.RunAtServices("C下达预订单", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;

            stopwatch.Stop(); //  停止监视            
            System.TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间    
            double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数

            DForThread(OutPutHT);
        }

        /// <summary>
        /// 下达预订单草稿
        /// </summary>
        public void SetYDDCG()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            DataTable dt = 客户端主程序.Support.StringOP.GetDataTableFormHashtable(InPutHT);
            dt.TableName = "YDD";//命名为预订单
            DataSet ds = new DataSet("YDDds");
            ds.Tables.Add(dt);

            ds.Tables[0].Columns.Add("买家角色编号");
            ds.Tables[0].Rows[0]["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();

            object[] cs = { ds };
            //已移植可替换(已替换)
            DataSet dsreturn = wsc.RunAtServices("C下达预订单草稿", cs);
           //DataSet dsreturn = wsc.RunAtServices("SetYDDCG", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 预订单撤销
        /// </summary>
        public void YDD_CX()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            object[] cs = { InPutHT["Number"].ToString() };
            //  DataSet dsreturn = wsc.RunAtServices("YDD_CX", cs);
            //已移植可替换(已替换)
            DataSet dsreturn = wsc.RunAtServices("C撤销预订单", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 预订单修改
        /// </summary>
        public void YDD_XG()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            object[] cs = { InPutHT["Number"].ToString() };
            //DataSet dsreturn = wsc.RunAtServices("YDD_XG", cs);
            //已移植可替换(已替换)
            DataSet dsreturn = wsc.RunAtServices("C返回撤销的预订单原始信息", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 获取预订单草稿
        /// </summary>
        public void YDD_Caogaoinfo()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            object[] cs = { InPutHT["Number"].ToString() };
            //已移植可替换(已替换) wyh 2014.07.14 
            // DataSet dsreturn = wsc.RunAtServices("GetYDDcaogaoinfo", cs); 
            DataSet dsreturn = wsc.RunAtServices("C获取预订单草稿信息", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;
            DForThread(OutPutHT);
        }
    }
}
