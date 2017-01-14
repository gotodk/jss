using FMDBHelperClass;
using RedisReLoad.ShareDBcache;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlRedisForFMDBHelper_AAA_YDDXXB
{
    public class GO
    {
        /// <summary>
        /// 开始处理，尝试更新缓存
        /// </summary>
        /// <param name="typeS">操作类型，包括“更新”、“插入”</param>
        /// <param name="P_cmd">原始带参数的sql语句</param>
        /// <param name="P_ht_in">原始传入的参数</param>
        /// <param name="tablename">被操作的表名</param>
        /// <param name="filed">被操作的字段和对应值</param>
        /// <param name="where">操作条件字段和对应值</param>
        /// <returns></returns>
        public string TryUpdateRedis(string typeS, string P_cmd, Hashtable P_ht_in, string tablename, Dictionary<string, string> filed, Dictionary<string, string> where)
        {
            //获得数据后，根据需要进行判断和处理。 注意。 字段名称一般不会有错误，但是对应值，不一定是期待的@dlyx这样的，也可能是“dj+1”这样的东西。 具体情况具体处理。
            if (typeS == "插入")
            {//通过参数获取dlyx信息
                Insert_Cqu(P_ht_in, filed);
            }
            else if (typeS == "更新")
            {
                update_Cqu(P_ht_in, where);
            }
            return "ok";
        }

        //处理插入语句影响的商品买卖Cqu的对应标签功能
        private void Insert_Cqu(Hashtable htinput, Dictionary<string, string> field)
        {
            RedisClient RC = RedisClass.GetRedisClient(null);
            if (!field.ContainsKey("DLYX"))
            {
                return;
            }
            //包含dlyx字段才进行处理，正常插入一定会带这个字段
            string dlyx = field["DLYX"].ToString();

            if (dlyx.Trim().StartsWith("@"))
            { //如果dlyx字段对应的值是参数，则通过传入参数替换为正常值
                dlyx = htinput[dlyx].ToString();
            }
            //插入AAA_TBD的时候，会影响商品买卖C区中的商品买卖概况、竞标中两个标签的数据。冷静期暂不处理
            string keyCqu = "H:Cqu:" + dlyx;
            lock (RedisClass.LockObj)
            {
                RC.HDel(keyCqu, Encoding.UTF8.GetBytes("商品买卖概况"));//删除商品买卖概况对应的内容
                RC.HDel(keyCqu, Encoding.UTF8.GetBytes("竞标中"));//删除竞标中对应的内容
            }
        }

        //处理更新语句影响的功能
        private void update_Cqu(Hashtable htInput, Dictionary<string, string> where)
        {
            RedisClient RC = RedisClass.GetRedisClient(null);

            //将传入的where中的key全部转换为小写字母，方便后面的查找和使用
            Dictionary<string, string> where_lower = new Dictionary<string, string>();
            foreach (var item in where)
            {
                where_lower.Add(item.Key.ToLower(), item.Value.ToString());
            }

            //更新目前都是使用number作为条件进行更新的，获取number的值            
            string number = "";
            if (!where_lower.ContainsKey("number"))
            {
                return;
            }
            number = where_lower["number"].ToString();
            if (number.Trim().StartsWith("@"))
            { //如果number是参数，则进行替换                
                number = htInput[number].ToString();
            }

            //通过number获取用户的邮箱
            string dlyx = "";          
            Hashtable ht = new Hashtable();
            ht.Add("@number", number);
            string sql = "select DLYX,SPBH,HTQX from AAA_YDDXXB where Number=@number";
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable ht_res = I_DBL.RunParam_SQL(sql, "data", ht);
            if ((bool)ht_res["return_float"])
            {
                DataSet ds = (DataSet)ht_res["return_ds"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("data") && ds.Tables["data"].Rows.Count > 0)
                {
                    dlyx = ds.Tables["data"].Rows[0]["DLYX"].ToString();
                }
            }
            if (dlyx != "")
            {//更新AAA_TBD的时候，会影响商品买卖C区中的商品买卖概况、竞标中、中标三个标签的数据。冷静期暂不处理。，不管更新任何字段，都将对应的标签更新一遍。
                string keyCqu = "H:Cqu:" + dlyx;
                lock (RedisClass.LockObj)
                {
                    RC.HDel(keyCqu, Encoding.UTF8.GetBytes("商品买卖概况"));//删除商品买卖概况对应的内容
                    RC.HDel(keyCqu, Encoding.UTF8.GetBytes("竞标中"));//删除竞标中对应的内容
                    RC.HDel(keyCqu, Encoding.UTF8.GetBytes("中标"));//删除竞标中对应的内容
                }
            }
        }

    }
}

