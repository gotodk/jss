namespace FMDBHelperClass
{
	/// <summary>
	/// 包含工厂可以处理的所有,也就是具体工厂角色
	/// </summary>
	public class DBFactory : I_DBFactory
	{
 
        /// <summary>
        /// 获取数据库操作类的实例,用于sql数据库
        /// </summary>
        public I_Dblink DbLinkSqlMain(string ConnConfig)
        {
            return new Dblink_Sql_Main(ConnConfig); 
        }
	}
}
