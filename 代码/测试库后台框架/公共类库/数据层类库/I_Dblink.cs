using System.Collections;
using System.Data;

namespace FMDBHelperClass
{
	/// <summary>
	/// 定义数据库操作方法的接口，也就是所谓的抽象产品角色
	/// </summary>
	public interface I_Dblink
	{




        /// <summary>
        /// 单语句执行，不支持事务。
        /// 执行静态语句。 不返回数据集， 会返回受影响行数。
        /// 常用于执行静态语句进行更新、插入操作，不能用于查询操作。
        /// </summary>
        /// <param name="SQL">需要执行的sql语句</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功,受影响的行数(return_ds、return_float、return_errmsg、return_other)</returns>
		Hashtable RunProc(string SQL);

        /// <summary>
        /// 单语句执行，不支持事务。
        /// 执行静态语句。 会返回数据集， 不返回受影响行数。
        /// 常用于执行静态语句进行查询操作，不能用于插入、更新操作。
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集(return_ds、return_float、return_errmsg、return_other)</returns>
		Hashtable RunProc(string SQL ,string DTname);


        /// <summary>
        /// 单语句执行，不支持事务。(优先读取缓存)
        /// 执行静态语句。 会返回数据集， 不返回受影响行数。
        /// 常用于执行静态语句进行查询操作，不能用于插入、更新操作。
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="RedisKey">键</param>
        /// <param name="RedisPZ">配置字符串</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集(return_ds、return_float、return_errmsg、return_other)</returns>
        Hashtable RunProc(string SQL, string DTname, string RedisKey, string RedisPZ);












        /// <summary>
        /// 单语句执行，不支持事务。
        /// 执行动态语句。 会返回数据集， 不返回受影响行数。
        /// 常用于执行动态语句进行查询操作，不能用于插入、更新操作。
        /// </summary>
        /// <param name="P_cmd">带参数的语句</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="P_ht_in">参数哈希表</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集(return_ds、return_float、return_errmsg、return_other)</returns>
        Hashtable RunParam_SQL(string P_cmd, string DTname, Hashtable P_ht_in);


        /// <summary>
        /// 单语句执行，不支持事务。
        /// 执行动态语句。 会返回数据集， 不返回受影响行数。
        /// 常用于执行动态语句进行查询操作，不能用于插入、更新操作。
        /// </summary>
        /// <param name="P_cmd">带参数的语句</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="P_ht_in">参数哈希表</param>
        /// <param name="RedisKey">键</param>
        /// <param name="RedisPZ">配置字符串</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集(return_ds、return_float、return_errmsg、return_other)</returns>
        Hashtable RunParam_SQL(string P_cmd, string DTname, Hashtable P_ht_in, string RedisKey, string RedisPZ);




        /// <summary>
        /// 语句执行，不支持事务。
        /// 执行动态语句。 不返回数据集， 会返回受影响行数。
        /// 常用于执行动态语句进行插入、更新操作，不能用于查询操作。
        /// </summary>
        /// <param name="P_cmd">带参数的语句</param>
        /// <param name="P_ht_in">参数哈希表</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集(return_ds、return_float、return_errmsg、return_other)</returns>
        Hashtable RunParam_SQL(string P_cmd, Hashtable P_ht_in);
        /// <summary>
        /// 多语句执行，支持事务。
        /// 执行动态语句。 不返回数据集， 会返回受影响行数。
        /// 常用于执行动态语句进行插入、更新操作，不能用于查询操作。
        /// </summary>
        /// <param name="SQLStringList">多个语句数组</param>
        /// <param name="P_ht_in">参数哈希表</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，不会返回结果集</returns>
        Hashtable RunParam_SQL(ArrayList SQLStringList, Hashtable P_ht_in);

        /// <summary>
        /// 多语句执行，支持事务。
        /// 执行静态语句。 不返回数据集， 会返回受影响行数。
        /// 常用于执行静态语句进行插入、更新操作，不能用于查询操作。
        /// </summary>
        /// <param name="SQLStringList">多个语句数组</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，不会返回结果集</returns>
        Hashtable RunParam_SQL(ArrayList SQLStringList);


		/// <summary>
        /// 无参数存储过程执行，事务在存储过程内自行实现。
        /// 会返回数据集， 不返回受影响行数。
		/// </summary>
		/// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">DataSet对象</param>
		/// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable RunProc_CMD(string P_cmd, string DTname);

        /// <summary>
        /// 无参数存储过程执行，事务在存储过程内自行实现。
        /// 会返回数据集， 不返回受影响行数。
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="RedisKey">键</param>
        /// <param name="RedisPZ">配置字符串</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable RunProc_CMD(string P_cmd, string DTname, string RedisKey, string RedisPZ);

		/// <summary>
        /// 带参数存储过程执行，事务在存储过程内自行实现。
        /// 带输入参数，无输出参数，会返回数据集， 不返回受影响行数。
		/// </summary>
		/// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">DataSet对象</param>
		/// <param name="P_ht_in">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable RunProc_CMD(string P_cmd, string DTname, Hashtable P_ht_in);


        /// <summary>
        /// 带参数存储过程执行，事务在存储过程内自行实现。
        /// 带输入参数，无输出参数，会返回数据集， 不返回受影响行数。
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="P_ht_in">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
        /// <param name="RedisKey">键</param>
        /// <param name="RedisPZ">配置字符串</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable RunProc_CMD(string P_cmd, string DTname, Hashtable P_ht_in, string RedisKey, string RedisPZ);

		/// <summary>
        /// 带参数存储过程执行，事务在存储过程内自行实现。
        /// 带输入参数，带输出参数，会返回数据集， 不返回受影响行数。
		/// </summary>
		/// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">DataSet对象</param>
		/// <param name="P_ht_in">哈希表，对应存储过程传入参数</param>
		/// <param name="P_ht_out">哈希表，对应存储过程传出参数,传址</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable RunProc_CMD(string P_cmd, string DTname, Hashtable P_ht_in, ref Hashtable P_ht_out);


        /// <summary>
        /// 带参数存储过程执行，事务在存储过程内自行实现。
        /// 带输入参数，带输出参数，会返回数据集， 不返回受影响行数。
        /// </summary>
        /// <param name="P_cmd">存储过程名称</param>
        /// <param name="DTname">返回的数据集DataTable名</param>
        /// <param name="P_ht_in">哈希表，对应存储过程传入参数</param>
        /// <param name="P_ht_out">哈希表，对应存储过程传出参数,传址</param>
        /// <param name="RedisKey">键</param>
        /// <param name="RedisPZ">配置字符串</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Hashtable RunProc_CMD(string P_cmd, string DTname, Hashtable P_ht_in, ref Hashtable P_ht_out, string RedisKey, string RedisPZ);

		
	}
}
