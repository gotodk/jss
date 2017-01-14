using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using FMOP.DB;
namespace FM.HumanResource.Assess.Quality{



	/// <summary>
	/// ָ�����
	/// </summary>
	public abstract class CriterionLibrary
	{	

		/// <summary>
		/// ��ȡĳԱ�������п���ָ��
		/// </summary>
		/// <param name="AssessID">��ʷ���˱�ID</param>
		/// <param name="number">Ա�����</param>		
		/// <returns>ָ���б�DataSet</returns>
		public static DataSet GetCriterionByType(int AssessID,string number){
			string type = GetType(number);
			string sql = "SELECT  id, AssessId, Title, Type, Explain, MaxScosre, MinScore  FROM  Hr_Quality_History_Criterions  WHERE  (AssessId = " + AssessID.ToString() + ") AND (Type = '" + type + "')";
			DataSet ds  = FMOP.DB.DbHelperSQL.Query(sql);
			return ds;
		}

		/// <summary>
		/// ����Ա����ţ���ѯ����Ա���
		/// </summary>
		/// <param name="number">Ա�����</param>
		/// <returns>������Ա��������Ա</returns>
		public static string GetType(string number){
			string sql = "select case when rylb='����' then '������Ա' else '������Ա' end as rylb from hr_employees where number='" + number + "'";
			object ob = DbHelperSQL.GetSingle(sql);
			if (ob != null)
			{
				return ob.ToString();
			}
			return string.Empty;
		}

		/// <summary>
		/// ��hr_quality_assess����ʷ���˱�����һ����¼
		/// </summary>
		/// <param name="year">���</param>
		/// <param name="month">�·�</param>
		/// <param name="remark">��ע</param>
		/// <returns>��ʷ���˱�ID</returns>
		public static int AddAssess(int year,int month,string remark)
		{
			//���Ӽ�¼
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
			//��ѯID
			string sql = "select IDENT_CURRENT('hr_quality_assess')";
			object ob = DbHelperSQL.GetSingle(sql);
			if (ob != null)
			{
				return Convert.ToInt32(ob);
			}
			return 0;
		}

		/// <summary>
		/// ����ָ���
		/// </summary>
		/// <param name="assessID">��ʷ���˱�ID</param>
		/// <returns>��Ӱ�������</returns>
		public static int AddCriterions(int assessID)
		{
			string sql = "insert into hr_quality_history_criterions select " + assessID.ToString() + ",[name],[type],explain,maxscore,minscore from Hr_Quality_Criterion_Library where [enable]='����'";
			return DbHelperSQL.ExecuteSql(sql);
		}
        /// <summary>
        /// ����ָ���(���ò���)by galaxy
        /// </summary>
        /// <param name="assessID">��ʷ���˱�ID</param>
        /// <returns>��Ӱ�������</returns>
        public static int AddCriterions(int assessID,string Job_Dept)
        {
            string sql = "insert into hr_quality_history_criterions select " + assessID.ToString() + ",[name],[type],explain,maxscore,minscore from Hr_Quality_Criterion_Library where [enable]='����' and [SYBM]='" + Job_Dept + "' ";
            return DbHelperSQL.ExecuteSql(sql);
        }
		/// <summary>
		/// ��ȡԱ���б�
		/// </summary>
		/// <returns>DataSetԱ���б�</returns>
		public static DataSet GetEmployees()
		{
			string sql = "select Number,Employee_Name from hr_employees where ygzt<> '��ְ'";
			return DbHelperSQL.Query(sql);
		}
        /// <summary>
        /// ��ȡԱ���б�,�޶�����(���أ�by galaxy)
        /// </summary>
        /// <param name="Job_Dept"></param>
        /// <returns></returns>
        public static DataSet GetEmployees(string Job_Dept)
        {
            string sql = "select Number,Employee_Name from hr_employees where ygzt<> '��ְ' and BM = '" + Job_Dept + "'";
            return DbHelperSQL.Query(sql);
        }
		/// <summary>
		/// ��Hr_Quality_People��������Ա��ϵ������һ����¼
		/// </summary>
		/// <param name="assessID">��ʷ���˱�ID</param>
		/// <param name="employees">������</param>
		/// <param name="people">������</param>
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