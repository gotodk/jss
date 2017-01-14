using FMDBHelperClass;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RedisReLoad.ShareDBcache
{
   public  class RDisPtdtcs
   {
       /// <summary>
       /// 获取平台动态参数表
       /// </summary>
       /// <param name="paras">Null为获取所有值，否则返回设定的字段值paras为以“,”隔开的数据库字段名称字符串</param>
       /// <returns></returns>
       public  DataTable Read_PTDTCS(string paras)
       {
           DataTable dt = null; //new DataTable();
          
           RedisClient RC = RedisClass.GetRedisClient(null);
           Dictionary<string, string> dic = null ;
           lock (RedisClass.LockObj)
           {
               //Dictionary<string ,string > dicc=GetDBB();
               if (paras == null || paras.Equals(""))
               {       
                   dic = RC.GetAllEntriesFromHash("H:PTDTCSSDB");              
               }
               else
               {
                   dic = new Dictionary<string, string>();
                   string[] strs = paras.Split(','); 
                   foreach (string str in strs)
                   {
                       dic.Add(str, RC.GetValueFromHash("H:PTDTCSSDB", str.Trim()));
                   }   
               }

               dt = Initdatatable(dic);
               dt.Clear();
               DataRow dr = dt.NewRow(); 

               foreach (var item in dic)
               {
                 dr[item.Key.ToString().Trim()] = item.Value.ToString();
               }

               dt.Rows.Add(dr);  
           }
           dt.AcceptChanges();

           return dt;
       }

       private  DataTable Initdatatable(Dictionary<string ,string > dic)
       {
           if (dic == null || dic.Count <= 0)
           {
               return null;
           }

          
           DataTable dt = new DataTable();
           DataColumn dc = null;
           foreach (var item in dic)
           {
               dc = new DataColumn(item.Key.ToString().Trim(), typeof(string));
               dt.Columns.Add(dc);
           }

           return dt;
          
       }
       private  Dictionary<string ,string > GetDBB()
       {
           Dictionary<string, string> dics = new Dictionary<string, string>();
           dics.Add("MJTBBZJBL", "卖家投标保证金比率");
           dics.Add("MJTBBZJZXZ", "卖家投标保证金最小值");
           dics.Add("MJLYBZJBL", "卖家履约保证金比率");
           dics.Add("MJLYBZJDJQX", "卖家履约保证金冻结期限");
           dics.Add("LYBZJBZBL", "履约保证金不足比率");
           dics.Add("MJDJBL", "买家订金比率");
           dics.Add("BZHBL", "保证函比率");
           dics.Add("QSJSFWFBL", "签收技术服务费比率");
           dics.Add("BZHWJSFWFBL", "保证函外技术服务费比率");
           dics.Add("JJRSYMJBLsel", "经纪人收益卖家比率");
           dics.Add("JJRSYMJBLbuy", "经纪人收益买家比率");
           dics.Add("XMQX", "休眠期限");
           dics.Add("PTMRKSFWSJ", "平台每日开始服务时间");
           dics.Add("PTMRJSFWSJ", "平台每日结束服务时间");
           dics.Add("JSHTDBXTHDQX", "即时合同定标下提货单期限");
           //dics.Add("MJTBBZJBL", "卖家投标保证金比率");
          
           return dics;

       }

       /// <summary>
       /// 更新动态参数表
       /// </summary>
       /// <param name="Strparas">Null是全部更新，否则传入以逗号分隔的数据库字段字符串，更新对应字段</param>
       /// <returns></returns>
      public bool  Updte_PTDTCSB(string Strparas)
      {
          string strwhere = "MJTBBZJBL, MJTBBZJZXZ, MJLYBZJBL, MJLYBZJDJQX, LYBZJBZBL, MJDJBL, BZHBL,  QSJSFWFBL, BZHWJSFWFBL, JJRSYMJBLsel, JJRSYMJBLbuy, XMQX, PTMRKSFWSJ, PTMRJSFWSJ,JSHTDBXTHDQX";
          if (Strparas != null && !Strparas.Equals(""))
          {
              strwhere = Strparas;
          }
          bool ispass = false;
          I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
          Hashtable ht = I_DBL.RunProc("SELECT   " + strwhere + " from AAA_PTDTCSSDB order by CreateTime DESC", "");
          if ((bool)ht["return_float"])
          {
              DataSet ds_p = (DataSet)ht["return_ds"];
              if (ds_p != null && ds_p.Tables[0].Rows.Count > 0)
              { 
                  RedisClient newRC = RedisClass.GetRedisClient(null);
                  List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                  KeyValuePair<string, string> kvp = new KeyValuePair<string,string>();
                  DataTable dt = ds_p.Tables[0];
                  for (int i = 0; i < dt.Columns.Count; i++)
                  {

                      kvp = new KeyValuePair<string, string>(dt.Columns[i].ColumnName.ToString().Trim(), dt.Rows[0][i].ToString());
                      keyValuePairs.Add(kvp);
                     
                  }
                  newRC.SetRangeInHash("H:PTDTCSSDB", keyValuePairs);
                  ispass = true;
              }
          }
         
          return ispass;

      }

   }
}
