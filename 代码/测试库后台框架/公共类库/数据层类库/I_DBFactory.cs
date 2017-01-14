namespace FMDBHelperClass
{
	/// <summary>
	/// 定义工厂调用接口，也就是所谓的抽象工厂角色
	/// </summary>
	public interface I_DBFactory
	{

        /// <summary>
        /// 获取数据库操作类的实例,用于sql
        /// </summary>
        /// <param name="ConnConfig">数据库配置key</param>
        /// <returns></returns>
        I_Dblink DbLinkSqlMain(string ConnConfig);
	}
}
