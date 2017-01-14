namespace FM.HumanResource.Salary{



	/// <summary>
	/// 薪酬规则类
	/// </summary>
	public class SalaryRule{	

		private float noAssessRate;

		private float achivementAssessRate;

		private float qualityAssessRate;

		private float vicegerentSocialSecurity;

		private float vicegerentAccumulationFund;

		/// <summary>
		/// </summary>
		private SalaryRuleManager salaryRuleManager;


		/// <summary>
		/// 非考核部门所占比重
		/// </summary>
		public float NoAssessRate{	
			get{
				return this.noAssessRate;
			}	
			set{
				this.noAssessRate = value;
			}	
		}
	
		/// <summary>
		/// 业绩考核所占比重
		/// </summary>
		public float AchivementAssessRate{	
			get{
				return this.achivementAssessRate;
			}	
			set{
				this.achivementAssessRate = value;
			}	
		}
	
		/// <summary>
		/// 个人素质考核所占比重
		/// </summary>
		public float QualityAssessRate{	
			get{
				return this.qualityAssessRate;
			}	
			set{
				this.qualityAssessRate = value;
			}	
		}

		/// <summary>
		/// 代缴社保
		/// </summary>
		public float VicegerentSocialSecurity {
			get {
				return this.vicegerentSocialSecurity;
			}
			set {
				this.vicegerentSocialSecurity = value;
			}
		}

		/// <summary>
		/// 代缴公积金
		/// </summary>
		public float VicegerentAccumulationFund {
			get {
				return this.vicegerentAccumulationFund;
			}
			set {
				this.vicegerentAccumulationFund = value;
			}
		}
	
	}
}