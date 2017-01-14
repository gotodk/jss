using System;
using System.Data;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;

namespace FM.HumanResource.Assess.Quality{



	/// <summary>
	/// ���ֱ�
	/// </summary>
	public abstract class AssessTable{	



		/// <summary>
		/// * �����в���Ա�����������
		/// * 
		/// * �������е�Ա����ţ�����CriterionLibrary��ȡ����ָ�꣬Ȼ�����CriterionPeople��ȡ���ж��������Ա����������ЩԱ���������ѣ�����Ա�����˵��������
		/// </summary>
		/// <param name="year">���</param>
		/// <param name="month">�·�</param>
		/// <param name="remark">��ע</param>
		public static void SendAllTables(int year, int month,string remark){
			int assessID = CriterionLibrary.AddAssess(year, month, remark);
			CriterionLibrary.AddCriterions(assessID);

			AssessPeople ap = new AssessPeople();
			DataSet ds = CriterionLibrary.GetEmployees();
			if (ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					string number = dr["Number"].ToString();
					ArrayList al = ap.GetAssessPeople(number);
					al.Add(number);
					for (int i = 0; i < al.Count; i++)
					{ 
						CriterionLibrary.AddPeople(assessID,number,al[i].ToString());
						Warning.AddWarning("�յ��������ʿ������鵥��������Ϊ" + dr["Employee_Name"].ToString(), "HR/ViewDemocraryTable.aspx", 1, number, al[i].ToString());
					}
				}
			}	
		}

	
		/// <summary>
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="fromEmpNo"></param>
		/// <param name="toEmpNo"></param>
		/// <param name="empType"></param>
		private static void SendTableToEmployee(int year, int month, string fromEmpNo, string toEmpNo, string empType){
			// TODO put your implementation here.	
		}
	
		/// <summary>
		/// </summary>
		public static void Comput(){
			// TODO put your implementation here.	
			
		}

		
		
	}
}