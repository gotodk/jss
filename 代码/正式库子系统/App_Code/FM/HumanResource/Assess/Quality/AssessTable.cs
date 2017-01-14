using System;
using System.Data;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;
using FMOP.DB;
using System.Data.SqlClient;

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

			DataSet ds = CriterionLibrary.GetEmployees();
			if (ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					string number = dr["Number"].ToString();
					AssessPeople ap = new AssessPeople();
					ArrayList al = new ArrayList();
					al = ap.GetAssessPeople(number);
					al.Add(number);
					for (int i = 0; i < al.Count; i++)
					{
						string sql = "select count(*) from HR_Quality_People where assessid=" + assessID.ToString() + " and Employee='" + number + "' and appraiser ='" + al[i].ToString() + "'";
						object ob = DbHelperSQL.GetSingle(sql);
						if (ob!=null)
						{
							int clo = Convert.ToInt32(ob);
							if(clo>0)
								continue;
						}
						CriterionLibrary.AddPeople(assessID,number,al[i].ToString());
						Warning.AddWarning("�յ��������ʿ������鵥��������Ϊ" + dr["Employee_Name"].ToString(), "HR/ViewDemocraryTable.aspx", 1, number, al[i].ToString());
					}	
				}
			}	
		}



        /// <summary>
        /// * �����в���Ա�����������
        /// * by galaxy ����
        /// * �������е�Ա����ţ�����CriterionLibrary��ȡ����ָ�꣬Ȼ�����CriterionPeople��ȡ���ж��������Ա����������ЩԱ���������ѣ�����Ա�����˵��������
        /// </summary>
        /// <param name="year">���</param>
        /// <param name="month">�·�</param>
        /// <param name="remark">��ע</param>
        /// <param name="Job_Dept">����</param>
        public static void SendAllTables(int year, int month, string remark, string Job_Dept)
        {
            int assessID = CriterionLibrary.AddAssess(year, month, remark);
            CriterionLibrary.AddCriterions(assessID, Job_Dept);

            DataSet ds = CriterionLibrary.GetEmployees(Job_Dept);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string number = dr["Number"].ToString();
                    AssessPeople ap = new AssessPeople();
                    ArrayList al = new ArrayList();
                    al = ap.GetAssessPeople(number);
                    al.Add(number);
                    for (int i = 0; i < al.Count; i++)
                    {
                        string sql = "select count(*) from HR_Quality_People where assessid=" + assessID.ToString() + " and Employee='" + number + "' and appraiser ='" + al[i].ToString() + "'";
                        object ob = DbHelperSQL.GetSingle(sql);
                        if (ob != null)
                        {
                            int clo = Convert.ToInt32(ob);
                            if (clo > 0)
                                continue;
                        }
                        CriterionLibrary.AddPeople(assessID, number, al[i].ToString());
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