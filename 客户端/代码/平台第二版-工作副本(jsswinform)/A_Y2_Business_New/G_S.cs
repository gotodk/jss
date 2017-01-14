using System.Collections;
using System.Data;
using System.Threading;
using 客户端主程序.DataControl;

namespace 客户端主程序.NewDataControl
{
   public class G_S
    {
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;
        public G_S(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }
        /// <summary>
        /// 下达投标单
        /// </summary>
        public void SetTBD()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            //Hashtable ht_QTZZ = (Hashtable)InPutHT["其他资质"];
            DataTable dt = GetDataTableFormHashtable(InPutHT);
            dt.TableName = "TBD";//命名为预订单
            //DataTable dt_zz = new DataTable();
            //if (ht_QTZZ == null)
            //{

            //}
            //else
            //{
            //    dt_zz = GetDataTableFormHashtable(ht_QTZZ);
            //}
            //dt_zz.TableName = "ZZ";
            DataSet ds = new DataSet("TBDds");
            ds.Tables.Add(dt);
            //ds.Tables.Add(dt_zz);
            object[] cs = { ds };
            //DataSet dsreturn = wsc.RunAtServices("SetTBD", cs);
            //已移植可替换(已替换) wyh 2014.07.03
            DataSet dsreturn = wsc.RunAtServices("C发布投标单", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;
            DForThread(OutPutHT);
        }

        public static DataTable GetDataTableFormHashtable(Hashtable HTforParameter)
        {
            DataTable dsforparameter = new DataTable();
            dsforparameter.TableName = "传递参数";
            ArrayList zhi = new ArrayList();
            foreach (System.Collections.DictionaryEntry item in HTforParameter)
            {
                dsforparameter.Columns.Add(item.Key.ToString(), typeof(string));
                zhi.Add(item.Value);
            }
            dsforparameter.Rows.Add(zhi.ToArray());
            return dsforparameter;
        }

        /// <summary>
        /// 下达投标单草稿
        /// </summary>
        public void SetTBDCG()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            Hashtable ht_QTZZ = (Hashtable)InPutHT["其他资质"];
            DataTable dt = GetDataTableFormHashtable(InPutHT);
            dt.TableName = "TBD";//命名为预订单
            DataTable dt_zz = new DataTable();
            if (ht_QTZZ != null)
            {
                dt_zz = GetDataTableFormHashtable(ht_QTZZ);
            }
            dt_zz.TableName = "ZZ";
            DataSet ds = new DataSet("TBDds");
            ds.Tables.Add(dt);
            ds.Tables[0].Columns.Add("买家角色编号");
            ds.Tables[0].Rows[0]["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            ds.Tables.Add(dt_zz);
            object[] cs = { ds };
            //已移植可替换(已替换)
           // DataSet dsreturn = wsc.RunAtServices("SetTBDCG", cs);
            DataSet dsreturn = wsc.RunAtServices("C下达投标单草稿", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 投标单撤销
        /// </summary>
        public void TBD_CX()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            object[] cs = { InPutHT["Number"].ToString() };
            //已移植可替换(已替换)
          //  DataSet dsreturn = wsc.RunAtServices("TBD_CX", cs);
            DataSet dsreturn = wsc.RunAtServices("C投标单撤销", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 投标单修改
        /// </summary>
        public void TBD_XG()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            object[] cs = { InPutHT["Number"].ToString() };
            //已移植可替换(已替换)
            //DataSet dsreturn = wsc.RunAtServices("TBD_XG", cs);
            DataSet dsreturn = wsc.RunAtServices("C投标单修改", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 获得投标草稿
        /// </summary>
        public void TBD_GetCGinfo()
        {
            Thread.Sleep(1);
            NewDataControl.WebServicesCenter2013 wsc = new WebServicesCenter2013();
            object[] cs = { InPutHT["Number"].ToString() };
            //已移植可替换(已替换) wyh  2014.07.14 
            // DataSet dsreturn = wsc.RunAtServices("TBD_GetCGinfo", cs);
            DataSet dsreturn = wsc.RunAtServices("C获取投标单草稿信息", cs);
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["结果"] = dsreturn;
            DForThread(OutPutHT);
        }

  
        
    }
}
