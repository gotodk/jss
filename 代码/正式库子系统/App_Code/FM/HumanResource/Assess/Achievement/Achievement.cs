using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Hesion.Brick.Core.WorkFlow;
using System.Text;
using System.Data.SqlClient;
using FMOP.DB;


namespace FM.HumanResource.Assess.Achievement
{
	/// <summary>
	/// Achievement 的摘要说明
	/// </summary>
	public class Achievement
	{
		public Achievement()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// * 向所有参评员工发送评议表
		/// * 
		/// * 遍历所有的员工编号，调用CriterionLibrary获取所有指标，然后调用CriterionPeople获取所有对其评议的员工，并向这些员工发送提醒，包括员工本人的自评表格
		/// </summary>
		/// <param name="year">年份</param>
		/// <param name="month">月份</param>
		/// <param name="remark">备注</param>
		public static void SendAllTables(int year, int month, string remark,string username)
		{
			int assessID = AddAssess(year, month, remark);
			AddCriterions(assessID);
			DataSet ds = GetEmployees(assessID);
			if (ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					Warning.AddWarning("收到业务考核统计表，请尽快统计提交。", "HR/ViewAchievement_Score.aspx", 1, username, dr["StatisticianNo"].ToString());
				}
			}
		}


		/// <summary>
		/// 向hr_quality_assess（历史考核表）增加一条记录
		/// </summary>
		/// <param name="year">年份</param>
		/// <param name="month">月份</param>
		/// <param name="remark">备注</param>
		/// <returns>历史考核表ID</returns>
		public static int AddAssess(int year, int month, string remark)
		{
			//增加记录
			StringBuilder sb = new StringBuilder();
			sb.Append("insert into HR_Achievement_Assess(");
			sb.Append("year, month, remark");
			sb.Append(")values(");
			sb.Append("@year, @month, @remark");
			sb.Append(")");
			SqlParameter[] parameters = {
				new SqlParameter("@year",SqlDbType.Int),
				new SqlParameter("@month",SqlDbType.Int),
				new SqlParameter("@remark",SqlDbType.Text,500)
			};
			parameters[0].Value = year;
			parameters[1].Value = month;
			parameters[2].Value = remark;
			DbHelperSQL.ExecuteSql(sb.ToString(), parameters);
			//查询ID
			string sql = "select IDENT_CURRENT('HR_Achievement_Assess')";
			object ob = DbHelperSQL.GetSingle(sql);
			if (ob != null)
			{
				return Convert.ToInt32(ob);
			}
			return 0;
		}

		/// <summary>
		/// 复制指标库
		/// </summary>
		/// <param name="assessID">历史考核表ID</param>
		/// <returns>所影响的行数</returns>
		public static int AddCriterions(int assessID)
		{
			string sql = "insert into Hr_Achivement_History_Criterions select " + assessID.ToString() + ",[name],[dept],Target,Arithmetic,Score,StatisticianNo,Score,0,number from HR_Achievement_Criterion_Library where [enable]='可用'";
			return DbHelperSQL.ExecuteSql(sql);
		}

		/// <summary>
		/// 获取发送员工列表
		/// </summary>
		/// <returns>DataSet员工列表</returns>
		public static DataSet GetEmployees(int assessid)
		{
			string sql = "select distinct StatisticianNo from Hr_Achivement_History_Criterions where assessid=" + assessid.ToString();
			return DbHelperSQL.Query(sql);
		}
	}
}
