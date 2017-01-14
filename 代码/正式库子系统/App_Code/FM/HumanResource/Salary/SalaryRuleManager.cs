using System.Data.SqlClient;
using System.Text;
using System;
using FMOP.DB;
namespace FM.HumanResource.Salary{



	/// <summary>
	/// н�������������
	/// </summary>
	public abstract class SalaryRuleManager{	




		/// <summary>
		/// ͨ���¶�н�����ID��Ա�����ͣ�����һ��н�������
		/// </summary>
		/// <param name="RuleId">н������id</param>
		/// <param name="JobType">��Ա���� ��������Ա��ҵ����Ա��רҵ��Ա������������Ա������������Ա��</param>
		/// <returns></returns>
		public static SalaryRule GetRule(int RuleId, string JobType){
			SalaryRule salaryRule = new SalaryRule();
			string columns = string.Empty;
			switch (JobType)
			{
				case "����":
					columns = " ManagerNoAssess,ManagerAchievement,ManagerQuality ";
					break;
				case "רҵ":
					columns = " ProfessionalNoAssess,ProfessionalAchievement,ProfessionalQuality ";
					break;
				case "ҵ��":
					columns = " SellerNoAssess,SellerAchievement,SellerQuality ";
					break;
				case "��������":
					columns = " LogisticsNoAssess,LogisticsAchievement,LogisticsQuality ";
					break;
				case "����ʽ��Ա":
					columns = " OperatorNoAssess,OperatorAchievement,OperatorQuality ";
					break;
			}
			string SQLstr = "select " + columns + ",VicegerentSocialSecurity,VicegerentAccumulationFund from Hr_SalaryRule where id = " + RuleId.ToString();
			SqlDataReader sdr = null;
			try
			{
				sdr = DbHelperSQL.ExecuteReader(SQLstr);

				while (sdr.Read())
				{
					salaryRule.NoAssessRate = float.Parse(sdr[0].ToString());
					salaryRule.AchivementAssessRate = float.Parse(sdr[1].ToString());
					salaryRule.QualityAssessRate = float.Parse(sdr[2].ToString());
					salaryRule.VicegerentSocialSecurity = float.Parse(sdr[3].ToString());
					salaryRule.VicegerentAccumulationFund = float.Parse(sdr[4].ToString());
				}
				sdr.Close();
			}
			catch (Exception e)
			{
				sdr.Close();
				throw(e);
			}
			return salaryRule;
		}
		
	}
}