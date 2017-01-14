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
    public static class PrdTongJi
    {
        /// <summary>
        /// 依据大盘存储过程返回的数据，将商品的统计信息写入redis中
        /// </summary>
        /// <param name="dsSP"></param>
        public static void SetPrdTongJi_from_AAA_DaPanPrd_Get(DataSet dsSP)
        {
            string[] ZD = "商品编号,合同期限,当前商品产地,当前卖方信用等级,当前卖方名称,当前供货区域,当前投标轮次,上轮定标价,220均价,当前卖家最低价,当前买家最高价,升降幅,达成率/中标率,最低价标的投标拟售量,当前集合预订量,当前拟订购总量,最低价标的经济批量,最低价标的日均最高供货量,买家当前数量,买家今日新增数量,买家区域覆盖率,卖家当前数量,卖家今日新增数量,卖家区域覆盖率,最低价投标单号n,最低价投标单发票税率n".Split(',');
            DataRowCollection drc = dsSP.Tables[0].DefaultView.ToTable(false, ZD).Rows;

           
            RedisClient RC = RedisClass.GetRedisClient(null);


            Dictionary<string, string> DicTempK = new Dictionary<string, string>();
            //开始存redis
            for (int dd = 0; dd < drc.Count; dd++)
            {
                DataRow drTemp = drc[dd];
                string spbh = drTemp["商品编号"].ToString();
                string htqx = drTemp["合同期限"].ToString();


                if (htqx != "--")
                {

                    string hashId = "H:YeWuDB:" + spbh + ":" + htqx;

                    List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                    for (int i = 0; i < ZD.Count(); i++)
                    {//遍历获取的数据集，生成要写入redis的键值对列表。0和1是商品编号和合同期限。跳过去。不作为filed写入。
                        if (i > 1)
                        {
                            keyValuePairs.Add(new KeyValuePair<string, string>(ZD[i], drTemp[ZD[i]].ToString()));
                        }

                    }

                    lock (RedisClass.LockObj)
                    {
                        if (!DicTempK.ContainsKey(spbh))
                        {
                            RC.Del(new string[] { "H:YeWuDB:" + spbh + ":即时", "H:YeWuDB:" + spbh + ":三个月", "H:YeWuDB:" + spbh + ":一年" });
                            DicTempK[spbh] = "";
                        }
                        RC.SetRangeInHash(hashId, keyValuePairs);

                    }
                }
                else
                {
                    lock (RedisClass.LockObj)
                    {
                        RC.Del(new string[] { "H:YeWuDB:" + spbh + ":即时", "H:YeWuDB:" + spbh + ":三个月", "H:YeWuDB:" + spbh + ":一年" });
                    }
                }
      

                
            }
            
        }

        /// <summary>
        /// 将商品的统计信息写入redis中
        /// </summary>
        /// <param name="spbh">商品编号</param>
        /// <param name="htqx">合同期限</param>
        public static void SetPrdTongJi(string spbh, string htqx)
        {
            RedisClient RC = RedisClass.GetRedisClient(null);

            DataTable dt = GetPrdTJ(spbh, htqx);

            string hashId = "H:YeWuDB:" + spbh + ":" + htqx;

            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            if (dt == null || dt.Rows.Count <= 0)
            {//如果没有从表中获取到对应数据，直接返回
                return;
            }

            for (int i = 0; i < dt.Columns.Count; i++)
            {//遍历获取的数据集，生成要写入redis的键值对列表
                keyValuePairs.Add(new KeyValuePair<string, string>(dt.Columns[i].ColumnName.ToString(), dt.Rows[0][i].ToString()));
            }

            lock (RedisClass.LockObj)
            {
                RC.SetRangeInHash(hashId, keyValuePairs);
            }
        }

        //从大盘临时表中获取当前统计的商品最低价等相关信息
        private static DataTable GetPrdTJ(string spbh, string htqx)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@spbh", spbh);
            ht.Add("@htqx", htqx);
            string sql = "select [当前商品产地],[当前卖方信用等级],[当前卖方名称],[当前供货区域],[当前投标轮次],[上轮定标价],[220均价],[当前卖家最低价],[当前买家最高价],[升降幅],[达成率/中标率],[最低价标的投标拟售量],[当前集合预订量],[当前拟订购总量],[最低价标的经济批量],[最低价标的日均最高供货量],[买家当前数量],[买家今日新增数量],[买家区域覆盖率],[卖家当前数量],[卖家今日新增数量],[卖家区域覆盖率],[最低价投标单号n],[最低价投标单发票税率n] from dbo.AAA_DaPanPrd_New where 商品编号=@spbh and 合同期限=@htqx";
            
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable htres = I_DBL.RunParam_SQL(sql, "data", ht);
            DataTable dtres = new DataTable();
            if ((bool)htres["return_float"])
            {
                DataSet ds = (DataSet)htres["return_ds"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("data"))
                {
                    dtres = ds.Tables["data"];
                }
                else
                {
                    dtres = null;
                }
            }
            else
            {
                dtres = null;
            }
            return dtres;
        }
 
    }
}
