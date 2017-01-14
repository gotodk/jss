using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using FMOP.DB;
namespace FM.HumanResource.Assess.Quality{



	/// <summary>
	/// 指标库类
	/// </summary>
	public abstract class CriterionLibrary
	{	

		/// <summary>
		/// 获取某员工的所有考核指标
		/// </summary>
		/// <param name="AssessID">历史考核表ID</param>
		/// <param name="number">员工编号</param>		
		/// <returns>指标列表DataSet</returns>
		public static DataSet GetCriterionByType(int AssessID,string number){
			string type = GetType(number);
			string sql = "SELECT  id, AssessId, Title, Type, Explain, MaxScosre, MinScore  FROM  Hr_Quality_History_Criterions  WHERE  (AssessId = " + AssessID.ToString() + ") AND (Type = '" + type + "')";
			DataSet ds  = FMOP.DB.DbHelperSQL.Query(sql);
			return ds;
		}

		/// <summary>
		/// 根据员工编号，查询其人员类别
		/// </summary>
		/// <param name="number">员工编号</param>
		/// <returns>管理人员或其他人员</returns>
		public static string GetType(string number){
			string sql = "select case when rylb='管理' then '管理人员' else '其他人员' end as rylb from hr_employees where number='" + number + "'";
			object ob = DbHelperSQL.GetSingle(sql);
			if (ob != null)
			{
				return ob.ToString();
			}
			return string.Empty;
		}

		/// <summary>
		/// 向hr_quality_assess（历史考核表）增加一条记录
		/// </summary>
		/// <param name="year">年份</param>
		/// <param name="month">月份</param>
		/// <param name="remark">备注</param>
		/// <returns>历史考核表ID</returns>
		public static int AddAssess(int year,int month,string remark)
		{
			//增加记录
			StringBuilder sb = new StringBuilder();
			sb.Append("insert into HR_Quality_Assess(");
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
			string sql = "select IDENT_CURRENT('hr_quality_assess')";
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
			string sql = "insert into hr_quality_history_criterions select " + assessID.ToString() + ",[name],[type],explain,maxscore,minscore from Hr_Quality_Criterion_Library where [enable]='可用'";
			return DbHelperSQL.ExecuteSql(sql);
		}
        /// <summary>
        /// 复制指标库(适用部门)by galaxy
        /// </summary>
        /// <param name="assessID">历史考核表ID</param>
        /// <returns>所影响的行数</returns>
        public static int AddCriterions(int assessID,string Job_Dept)
        {
            string sql = "insert into hr_quality_history_criterions select " + assessID.ToString() + ",[name],[type],explain,maxscore,minscore from Hr_Quality_Criterion_Library where [enable]='可用' and [SYBM]='" + Job_Dept + "' ";
            return DbHelperSQL.ExecuteSql(sql);
        }
		/// <summary>
		/// 获取员工列表
		/// </summary>
		/// <returns>DataSet员工列表</returns>
		public static DataSet GetEmployees()
		{
			string sql = "select Number,Employee_Name from hr_employees where ygzt<> '离职'";
			return DbHelperSQL.Query(sql);
		}
        /// <summary>
        /// 获取员工列表,限定部门(重载，by galaxy)
        /// </summary>
        /// <param name="Job_Dept"></param>
        /// <returns></returns>
        public static DataSet GetEmployees(string Job_Dept)
        {
            string sql = "select Number,Employee_Name from hr_employees where ygzt<> '离职' and BM = '" + Job_Dept + "'";
            return DbHelperSQL.Query(sql);
        }
		/// <summary>
		/// 向Hr_Quality_People（评定人员关系表）增加一条记录
		/// </summary>
		/// <param name="assessID">历史考核表ID</param>
		/// <param name="employees">被评人</param>
		/// <param name="people">评价人</param>
		public static int AddPeople(int assessID,string employee,string people)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("insert into Hr_Quality_People(");
			sb.Append("AssessId, Employee, Appraiser, Score, State");
			sb.Append(")values(");
			sb.Append("@AssessId,@Employee,@Appraiser,0,0");
			sb.Append(")");
			SqlParameter[] parameters = {
				new SqlParameter("@AssessId",SqlDbType.Int),
				new SqlParameter("@Employee",SqlDbType.VarChar,50),
				new SqlParameter("@Appraiser",SqlDbType.VarChar,50)
			};
			parameters[0].Value = assessID;
			parameters[1].Value = employee;
			parameters[2].Value = people;
			return DbHelperSQL.ExecuteSql(sb.ToString(), parameters);
		}
		
	}
}