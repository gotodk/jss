using System.Data;
using System.Data.SqlClient;
using FMOP.DB;


namespace FM.check
{
	/// <summary>
	/// cust_check 的摘要说明
	/// </summary>
	public class cust_check
	{
		/// <summary>
		/// 改nextchecker为第一个审批者，并发送提醒信息； checkstate = 0
		/// </summary>
		/// <param name="module"></param>
		/// <param name="number"></param>
		/// <returns></returns>
		public static int set_cust_check(string module,string number)
		{
			SqlParameter[] spParameter = new SqlParameter[2];
			spParameter[0] = new SqlParameter("@Module", SqlDbType.VarChar, 100);
			spParameter[0].Value = module;
			spParameter[1] = new SqlParameter("@Number", SqlDbType.VarChar, 50);
			spParameter[1].Value = number;
			DbHelperSQL.RunProcedure("sp_customer_nextcheck", spParameter); 
			return 1;
		}
	}
}