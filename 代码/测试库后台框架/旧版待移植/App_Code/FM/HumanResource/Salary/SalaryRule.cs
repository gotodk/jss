namespace FM.HumanResource.Salary{



	/// <summary>
	/// н�������
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
		/// �ǿ��˲�����ռ����
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
		/// ҵ��������ռ����
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
		/// �������ʿ�����ռ����
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
		/// �����籣
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
		/// ���ɹ�����
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