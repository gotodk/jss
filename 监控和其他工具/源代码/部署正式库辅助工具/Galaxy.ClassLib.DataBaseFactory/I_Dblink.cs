using System.Collections;
using System.Data;

namespace Galaxy.ClassLib.DataBaseFactory
{
	/// <summary>
	/// 定义数据库操作方法的接口，也就是所谓的抽象产品角色
	/// </summary>
	public interface I_Dblink
	{

        /// <summary>
        /// 从XML文件更新数据库
        /// </summary>
        /// <param name="oldtempcmd">用于获取数据结构的原始临时sql语句</param>
        /// <param name="XMLpath">文件路径</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功</returns>
        Hashtable UpdateMore(string oldtempcmd, string XMLpath);


		/// <summary>
		/// 运行SQL语句
		/// </summary>
		/// <param name="SQL">需要执行的sql语句</param>
		/// <returns>返回结果集合对应的哈希表,包括执行是否成功</returns>
		Hashtable RunProc(string SQL);

		/// <summary>
		/// 运行SQL语句
		/// </summary>
		/// <param name="SQL">SQL语句</param>
		/// <param name="Ds">DataSet对象</param>
		/// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
		Hashtable RunProc(string SQL ,DataSet Ds);

		/// <summary>
		/// 运行SQL语句
		/// </summary>
		/// <param name="SQL">SQL语句</param>
		/// <param name="Ds">DataSet对象</param>
		/// <param name="tablename">表名</param>
		/// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
		Hashtable RunProc(string SQL ,DataSet Ds,string tablename);


		/// <summary>
		/// 运行存储过程
		/// </summary>
		/// <param name="P_cmd">存储过程名称</param>
		/// <param name="Ds">DataSet对象</param>
		/// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
		Hashtable RunProc_CMD(string P_cmd,DataSet Ds);

		/// <summary>
		/// 运行存储过程
		/// </summary>
		/// <param name="P_cmd">存储过程名称</param>
		/// <param name="Ds">DataSet对象</param>
		/// <param name="P_ht_in">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
		Hashtable RunProc_CMD(string P_cmd,DataSet Ds,Hashtable P_ht_in);

		/// <summary>
		/// 运行存储过程
		/// </summary>
		/// <param name="P_cmd">存储过程名称</param>
		/// <param name="Ds">DataSet对象</param>
		/// <param name="P_ht_in">哈希表，对应存储过程传入参数</param>
		/// <param name="P_ht_out">哈希表，对应存储过程传出参数,传址</param>
		Hashtable RunProc_CMD(string P_cmd,DataSet Ds,Hashtable P_ht_in,ref Hashtable P_ht_out);
		
	}
}
