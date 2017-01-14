using FM.HumanResource.Salary;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Text;
using FMOP.DB;

namespace FM.HumanResource.Salary{



	/// <summary>
	/// 薪酬计算类
	/// </summary>
	public class SalaryCalculator{	

		/// <summary>
		/// </summary>
		private string EmployeeNumber = string.Empty;
		private int	RuleId;
		private string EmployeeName = string.Empty;
		private string EmployeeDept = string.Empty;
		private float StandardSalary = 0;
		
		

		/// <summary>
		/// 通过月度薪酬ID初始化本类
		/// </summary>
		/// <param name="RuleId"></param>
		public SalaryCalculator(int RuleId){
			this.RuleId = RuleId;	
		}

		/// <summary>
		/// 向数据库中插入一条员工薪酬记录
		/// </summary>
		/// <param name="Emp"></param>
		public void AddSalaryRecord(Employee Emp){
			EmployeeNumber = Emp.EmpNumber;
			GetEmployeePart();
           
            float Amerce = 0;
            float noAssess = 0;
            float achievement = 0;
            float quality = 0;
            float ShouldSalary = 0;
            float BeforeTaxSalary = 0;
            float VicegerentPersonalTax = 0;
			SalaryRule sr = Emp.EmpSalaryRule;
            if (Emp.FactDays != 0.0)
            {
                Amerce = (Emp.LeaveHours / Emp.FactDays) * StandardSalary;//考勤扣款
            }

			noAssess = StandardSalary * sr.NoAssessRate;//非考核部分
			achievement = StandardSalary * sr.AchivementAssessRate * Emp.AchievementScore / 100;//业务
			quality = StandardSalary * sr.QualityAssessRate * Emp.QualityScore / 100;//个人素质
			ShouldSalary = noAssess + achievement + quality + Emp.Penalty - Emp.Bounty;//应发薪酬
			BeforeTaxSalary = (ShouldSalary - sr.VicegerentSocialSecurity) - sr.VicegerentAccumulationFund - Amerce;//实发薪酬
			VicegerentPersonalTax = Hesion.Brick.Tools.PersonalTax.GetTaxMoney(BeforeTaxSalary);//个人所得税
			






			string SQLstr = "insert into Hr_SalaryPeople(EmployeeNumber,SalaryID,EmployeeName,EmployeeDept,EmployeeType,StandardSalary,NoAssess,Achievement,Quality,OvertimeSubsidy,"+
							"OtherAreaSubsidy,Hortation,Amerce,ShouldSalary,VicegerentSocialSecurity,VicegerentAccumulationFund,BeforeTaxSalary,VicegerentPersonalTax,TakeSalary,remark) "+
							"values(@EmployeeNumber,@SalaryID,@EmployeeName,@EmployeeDept,@EmployeeType,@StandardSalary,@NoAssess,@Achievement,@Quality,@OvertimeSubsidy,@OtherAreaSubsidy,"+
							"@Hortation,@Amerce,@ShouldSalary,@VicegerentSocialSecurity,@VicegerentAccumulationFund,@BeforeTaxSalary,@VicegerentPersonalTax,@TakeSalary,@remark)";
			SqlParameter[] parameters = {
				new SqlParameter("@EmployeeNumber",SqlDbType.VarChar,50),
				new SqlParameter("@SalaryID",SqlDbType.Int),
				new SqlParameter("@EmployeeName",SqlDbType.VarChar,50),
				new SqlParameter("@EmployeeDept",SqlDbType.VarChar,50),
				new SqlParameter("@EmployeeType",SqlDbType.VarChar,50),
				new SqlParameter("@StandardSalary",SqlDbType.Float),
				new SqlParameter("@NoAssess",SqlDbType.Float),
				new SqlParameter("@Achievement",SqlDbType.Float),
				new SqlParameter("@Quality",SqlDbType.Float),
				new SqlParameter("@OvertimeSubsidy",SqlDbType.Float),
				new SqlParameter("@OtherAreaSubsidy",SqlDbType.Float),
				new SqlParameter("@Hortation",SqlDbType.Float),
				new SqlParameter("@Amerce",SqlDbType.Float),
				new SqlParameter("@ShouldSalary",SqlDbType.Float),
				new SqlParameter("@VicegerentSocialSecurity",SqlDbType.Float),
				new SqlParameter("@VicegerentAccumulationFund",SqlDbType.Float),
				new SqlParameter("@BeforeTaxSalary",SqlDbType.Float),
				new SqlParameter("@VicegerentPersonalTax",SqlDbType.Float),
				new SqlParameter("@TakeSalary",SqlDbType.Float),
				new SqlParameter("@remark",SqlDbType.Text,1000)
			};
			parameters[0].Value = EmployeeNumber;
			parameters[1].Value = RuleId;
			parameters[2].Value = EmployeeName;
			parameters[3].Value = EmployeeDept;
			parameters[4].Value = Emp.JobType;
			parameters[5].Value = StandardSalary;
			parameters[6].Value = noAssess;
			parameters[7].Value = achievement;
			parameters[8].Value = quality;
			parameters[9].Value = 0;
			parameters[10].Value = Amerce;
			parameters[11].Value = Emp.Penalty;
			parameters[12].Value = Emp.Bounty;
			parameters[13].Value = ShouldSalary;
			parameters[14].Value = sr.VicegerentSocialSecurity;
			parameters[15].Value = sr.VicegerentAccumulationFund;
			parameters[16].Value = BeforeTaxSalary;
			parameters[17].Value = VicegerentPersonalTax;
			parameters[18].Value = BeforeTaxSalary - VicegerentPersonalTax;
			parameters[19].Value = string.Empty;
			DbHelperSQL.ExecuteSql(SQLstr, parameters);
		}

		private void GetEmployeePart(){
			string employee_sql = "select employee_name,bm,bzgz from hr_employees where number='" +  EmployeeNumber+ "'";
			SqlDataReader sdr = null;
			try {
				sdr = DbHelperSQL.ExecuteReader(employee_sql);
				while(sdr.Read())
				{
					EmployeeName = sdr["employee_name"].ToString();
					EmployeeDept = sdr["bm"].ToString();
					StandardSalary = float.Parse(sdr["bzgz"].ToString());
				}
				sdr.Close();
			}
			catch(Exception e)
			{
				sdr.Close();
				throw(e);
			}
		}
	
		
		
	}
}