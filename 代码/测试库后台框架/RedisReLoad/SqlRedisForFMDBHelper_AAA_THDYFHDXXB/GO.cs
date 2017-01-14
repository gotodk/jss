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

namespace SqlRedisForFMDBHelper_AAA_THDYFHDXXB
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
            if (!field.ContainsKey("ZBDBXXBBH"))
            {
                return;
            }
            //包含dlyx字段才进行处理，正常插入一定会带这个字段
            string ZBDBnum = field["ZBDBXXBBH"].ToString();

            if (ZBDBnum.Trim().StartsWith("@"))
            { //如果dlyx字段对应的值是参数，则通过传入参数替换为正常值
                ZBDBnum = htinput[ZBDBnum].ToString();
            }

            //根据中标定标信息表编号获取对应的买家登陆邮箱
            string dlyx_buy = "";
            Hashtable ht_zbdb = new Hashtable();
            ht_zbdb.Add("@number", ZBDBnum);
            string sql_zbdb = "select  T_YSTBDDLYX as 卖家邮箱,Y_YSYDDDLYX as 买家邮箱 from AAA_ZBDBXXB where Number=@number";
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable ht_zbdb_res = I_DBL.RunParam_SQL(sql_zbdb, "data", ht_zbdb);
            if ((bool)ht_zbdb_res["return_float"])
            {
                DataSet ds = (DataSet)ht_zbdb_res["return_ds"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("data") && ds.Tables["data"].Rows.Count > 0)
                {
                    dlyx_buy = ds.Tables["data"].Rows[0]["买家邮箱"].ToString();
                }
            }

            if (dlyx_buy == "")
            {
                return;
            }
            //插入AAA_THDYFHDXXB的时候，只会影响货物收发C区中的已下达提货单标签的数据。
            string keyCqu_buy = "H:Cqu:" + dlyx_buy;
            lock (RedisClass.LockObj)
            {
                RC.HDel(keyCqu_buy, Encoding.UTF8.GetBytes("已下达提货单"));//删除已下达提货单对应的内容
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
            string dlyx_buy = "";
            string dlyx_sale = "";
            Hashtable ht_zbdb = new Hashtable();
            ht_zbdb.Add("@number", number);
            string sql_zbdb = "select  T_YSTBDDLYX as 卖家邮箱,Y_YSYDDDLYX as 买家邮箱 from AAA_ZBDBXXB as a left join AAA_THDYFHDXXB as b on a.Number=b.ZBDBXXBBH where b.Number=@number";
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable ht_zbdb_res = I_DBL.RunParam_SQL(sql_zbdb, "data", ht_zbdb);
            if ((bool)ht_zbdb_res["return_float"])
            {
                DataSet ds = (DataSet)ht_zbdb_res["return_ds"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("data") && ds.Tables["data"].Rows.Count > 0)
                {
                    dlyx_buy = ds.Tables["data"].Rows[0]["买家邮箱"].ToString();
                    dlyx_sale = ds.Tables["data"].Rows[0]["卖家邮箱"].ToString();
                }
            }

            if (dlyx_buy != "")
            {//更新买家的缓存信息
                string keyCqu_buy = "H:Cqu:" + dlyx_buy;
                lock (RedisClass.LockObj)
                {//更新AAA_THDYFHDXXB的时候，不管更新任何字段，都将对应的标签更新一遍。
                    RC.HDel(keyCqu_buy, Encoding.UTF8.GetBytes("货物收发概况"));
                    RC.HDel(keyCqu_buy, Encoding.UTF8.GetBytes("已下达提货单"));
                    RC.HDel(keyCqu_buy, Encoding.UTF8.GetBytes("货物签收"));
                    RC.HDel(keyCqu_buy, Encoding.UTF8.GetBytes("货物发出"));
                    RC.HDel(keyCqu_buy, Encoding.UTF8.GetBytes("问题与处理"));
                }
            }
            if (dlyx_sale != "")
            { //更新卖家的缓存信息               
                string keyCqu_sale = "H:Cqu:" + dlyx_sale;
                lock (RedisClass.LockObj)
                { //更新AAA_THDYFHDXXB的时候，不管更新任何字段，都将对应的标签更新一遍。
                    RC.HDel(keyCqu_sale, Encoding.UTF8.GetBytes("货物收发概况"));
                    RC.HDel(keyCqu_sale, Encoding.UTF8.GetBytes("已下达提货单"));
                    RC.HDel(keyCqu_sale, Encoding.UTF8.GetBytes("货物签收"));
                    RC.HDel(keyCqu_sale, Encoding.UTF8.GetBytes("货物发出"));
                    RC.HDel(keyCqu_sale, Encoding.UTF8.GetBytes("问题与处理"));
                }
            }
        }
    }
}
