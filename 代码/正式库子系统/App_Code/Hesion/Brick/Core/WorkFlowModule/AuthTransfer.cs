using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using FMOP.DB;

/// <summary>
/// AuthTransfer 的摘要说明
/// </summary>
public class AuthTransfer
{
	public AuthTransfer()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}


	/// <summary>
	///  根据被委托人查询委托人
	/// </summary>
	/// <param name="userNumber">被委托人</param>
	/// <returns>委托人</returns>
	public static string getTransfer(string userNumber)
	{
		string sql = "SELECT a.SQRZH FROM AuthTransfer a WHERE BSQRZH='" + userNumber + "' AND State=1";
		object ob = DbHelperSQL.GetSingle(sql);
		if (ob == null)
			return "";
		return ob.ToString();
	}


	/// <summary>
	/// 返回委托编号(不能有两个委托，被委托的人不能有其它委托对象.)
	/// </summary>
	/// <param name="userNumber">委托人编号</param>
	/// <returns></returns>
	public static string getTransferUser(string userNumber)
	{
		string sql = "SELECT a.BSQRZH, (select count(*) from AuthTransfer where SQRZH=a.BSQRZH AND State!=2) as count  FROM AuthTransfer a WHERE SQRZH='" + userNumber + "' AND State=1";
		DataSet ds = DbHelperSQL.Query(sql);
		if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows.Count > 1)
			return "";
		else
		{
			if (ds.Tables[0].Rows[0]["count"].ToString() != "0")
				return "";
			return ds.Tables[0].Rows[0]["BSQRZH"].ToString();
		}
	}
	/// <summary>
	/// 查询该员工是否已委托他人，并且委托没关闭
	/// </summary>
	/// <param name="userNumber">授权人员工编号</param>
	/// <returns>false:没有委托它人true：已经委托他人</returns>
	public static bool isTransferUser(string userNumber)
	{

		string sql = "SELECT SQRZH FROM AuthTransfer WHERE SQRZH ='" + userNumber + "' AND State=1 ";
		DataSet ds = DbHelperSQL.Query(sql);
		if (ds.Tables[0].Rows.Count > 0)
			return true;
		else
		{
			return false;
		}
	}

	/// <summary>
	/// 查询被委托人是否正在委托他人,在他人没有不同意和已经接受授权的情况下不能接受委托
	/// </summary>
	/// <param name="userNumber">被授权人员工编号</param>
	/// <returns>false:没有委托他人 true：已经委托他人</returns>
	public static bool isJSWT(string userNumber)
	{
		string sql = "SELECT BSQRZH FROM AuthTransfer WHERE SQRZH ='" + userNumber + "' AND  (State=0 or State =1) ";
		DataSet ds = DbHelperSQL.Query(sql);
		if (ds.Tables[0].Rows.Count > 0)
			return true;
		else
		{
			return false;
		}
	}

	/// <summary>
	/// 查询委托人是否正在委托他人,在他人没有不同意的情况下不能接受委托
	/// </summary>
	/// <param name="userNumber">授权人员工编号</param>
	/// <returns>false:没有委托他人 true：已经委托他人</returns>
	public static bool isYSQ(string userNumber)
	{
		string sql = "SELECT BSQRZH FROM AuthTransfer WHERE SQRZH ='" + userNumber + "' AND  (State=0 or State=1 ) ";
		DataSet ds = DbHelperSQL.Query(sql);
		if (ds.Tables[0].Rows.Count > 0)
			return true;
		else
		{
			return false;
		}
	}

}
