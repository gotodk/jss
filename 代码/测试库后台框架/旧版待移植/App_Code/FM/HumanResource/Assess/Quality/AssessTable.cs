using System;
using System.Data;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;

namespace FM.HumanResource.Assess.Quality{



	/// <summary>
	/// 评分表
	/// </summary>
	public abstract class AssessTable{	



		/// <summary>
		/// * 向所有参评员工发送评议表
		/// * 
		/// * 遍历所有的员工编号，调用CriterionLibrary获取所有指标，然后调用CriterionPeople获取所有对其评议的员工，并向这些员工发送提醒，包括员工本人的自评表格
		/// </summary>
		/// <param name="year">年份</param>
		/// <param name="month">月份</param>
		/// <param name="remark">备注</param>
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
						Warning.AddWarning("收到个人素质考核评议单，被评人为" + dr["Employee_Name"].ToString(), "HR/ViewDemocraryTable.aspx", 1, number, al[i].ToString());
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