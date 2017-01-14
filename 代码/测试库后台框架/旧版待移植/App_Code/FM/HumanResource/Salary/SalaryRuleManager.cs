using System.Data.SqlClient;
using System.Text;
using System;
using FMOP.DB;
namespace FM.HumanResource.Salary{



	/// <summary>
	/// 薪酬计算规则管理类
	/// </summary>
	public abstract class SalaryRuleManager{	




		/// <summary>
		/// 通过月度薪酬规则ID和员工类型，返回一个薪酬规则类
		/// </summary>
		/// <param name="RuleId">薪酬规则表id</param>
		/// <param name="JobType">人员类型 （管理人员，业务人员，专业人员，行政后勤人员，生产操作人员）</param>
		/// <returns></returns>
		public static SalaryRule GetRule(int RuleId, string JobType){
			SalaryRule salaryRule = new SalaryRule();
			string columns = string.Empty;
			switch (JobType)
			{
				case "管理":
					columns = " ManagerNoAssess,ManagerAchievement,ManagerQuality ";
					break;
				case "专业":
					columns = " ProfessionalNoAssess,ProfessionalAchievement,ProfessionalQuality ";
					break;
				case "业务":
					columns = " SellerNoAssess,SellerAchievement,SellerQuality ";
					break;
				case "行政后勤":
					columns = " LogisticsNoAssess,LogisticsAchievement,LogisticsQuality ";
					break;
				case "非正式人员":
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