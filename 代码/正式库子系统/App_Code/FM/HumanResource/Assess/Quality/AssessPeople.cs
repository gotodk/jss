using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Collections;
using FMOP.DB;

namespace FM.HumanResource.Assess.Quality{



	/// <summary>
	/// ������Ա
	/// </summary>
	public class AssessPeople{ 
        //����
        private string BM;
        //��һ����λ���
        private string SuperioNo;
        //ͬ�²��㣳�����˵������
        private int margin = 0;
        ////�漴��ȡ���������Ƿ��㹻������������������δ��
        //private int allmargin = 1;
		/// <summary>
		 /// ��ȡ��ĳ��Ա���������Ա�б�
		 /// 
		 /// ֱ���ϼ�һ�ˣ�ͬ��3-20�ˣ�������λ1-20�ˣ������ȡ
		/// </summary>
		/// <param name="EmployeeNo"></param>
		/// <returns></returns>
        public   ArrayList GetAssessPeople(string EmployeeNo)
        {

            margin = 0;
            SuperioNo = null;
            ArrayList AssessNo = new ArrayList();
            if (EmployeeNo !=null )
            {
                string SuperioNumber = GetSuperioEmployeeNo(EmployeeNo);//����ϼ���λ����
                ArrayList DeptNo = GetDeptEmployees(EmployeeNo);
                ArrayList InterrelatedNo = GetInterrelatedEmployees(EmployeeNo);
                AssessNo.AddRange(DeptNo);
                AssessNo.AddRange(InterrelatedNo);
                if (SuperioNumber != null)
                {
                    int count = AssessNo.Count;
                    AssessNo.Insert(count, SuperioNumber);
                }
            }

			return AssessNo;	
		}
		/// <summary>
		 /// ͨ��ĳԱ���Ĺ��ţ���ȡ��ֱ���ϼ��Ĺ���
		/// </summary>
		/// <param name="EmployeeNo"></param>
		/// <returns></returns>
        private  string GetSuperioEmployeeNo(string EmployeeNo)
        {
            string SuperioEmployeeNo = null;
            if (EmployeeNo != null)
            {
                StringBuilder str_sql = new StringBuilder();
                str_sql.Append("SELECT HR_Employees.Number,HR_Employees.BM ");
                str_sql.Append("FROM HR_Employees INNER JOIN ");
                str_sql.Append("HR_Jobs ON HR_Employees.JobNo = HR_Jobs.Number where HR_Jobs.Number=(SELECT top 1 HR_Jobs.SuperioJobNo FROM HR_Employees INNER JOIN HR_Jobs ON HR_Employees.JobNo = HR_Jobs.Number");
                str_sql.Append(" WHERE YGZT<>'��ְ' and HR_Employees.Number=");
                str_sql.Append("'" + EmployeeNo + "'");
                str_sql.Append(")");
                SqlDataReader dr = DbHelperSQL.ExecuteReader(str_sql.ToString ());
                if (dr.Read())
                {
                    SuperioEmployeeNo = dr["Number"].ToString();
                    SuperioNo = SuperioEmployeeNo; 
                    //BM = dr["BM"].ToString();
                }
                dr.Close();
            }
            return SuperioEmployeeNo;
		}
	
		/// <summary>
		 /// ͨ��ĳһԱ���Ĺ��ţ���ȡ��ͬ�µĹ����б��ų���Ա�������Լ������ϼ���
		/// </summary>
		/// <param name="EmployeeNo"></param>
		/// <returns></returns>
        private  ArrayList GetDeptEmployees(string EmployeeNo)
        {
            ArrayList DeptEmployees = new ArrayList();
            BM = getbm(EmployeeNo);
            //arr DeptEmployees = null;
			if(EmployeeNo !=null && BM !=null && SuperioNo !=null  )
            {
                //���ȡ���������˻�10��
                //int[] str = new int[2];
                //str[0] = 3;
                //str[1] = 4;
                Random ran = new Random();
                int str2 = ran.Next(100, 110);
                //��ѯ�ַ���
                StringBuilder str_sql = new StringBuilder();
                str_sql.Append("SELECT top(");
                str_sql.Append(str2);
                str_sql.Append(")HR_Employees.Number ");
                str_sql.Append("FROM HR_Employees  ");
                str_sql.Append("WHERE ");
                str_sql.Append("(BM='"+BM+"') ");
                str_sql.Append(" AND YGZT<>'��ְ' AND ");
                str_sql.Append("Number<>'"+EmployeeNo+"' ");
                str_sql.Append(" AND ");
                str_sql.Append("Number<>'" + SuperioNo + "' ");
                str_sql.Append(" ORDER BY NEWID()");
                DataSet ds = DbHelperSQL.Query(str_sql.ToString());
                int i = ds.Tables[0].Rows.Count;
                //�ж�ͬ�������Ƿ�С�ڣ�
                if (i<3)
                {
                    margin = 3 - i;//ȴ�ټ��˵�3��
                }
                if (i != 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        DeptEmployees.Add(ds.Tables[0].Rows[j]["Number"].ToString());//������3�˵�ͬ�±����ӵ�����
                    }
                }
            }
            return DeptEmployees;
		}
	
		/// <summary>
		 /// ͨ��ĳԱ���Ĺ��ţ���ȡ������λ��Ա���б�
		/// </summary>
		/// <param name="EmployeeNo"></param>
		/// <returns></returns>
        private  ArrayList GetInterrelatedEmployees(string EmployeeNo)
        {
            ArrayList InterrelatedEmployees = new ArrayList();
            if (EmployeeNo != null)
            {
                //�����ȡ������λ����
                //int[] str = new int[2];
                //str[0] = 1;
                //str[1] = 2;
                Random ran = new Random();
                int str2 = ran.Next(1, 21);
                int allNo=str2;
                if (SuperioNo != null)
                {
                    allNo = str2 + margin;//����ϼ���λ��Ϊ��ʱ!���ͬ�²���3��,�������������λ�г�ȡ
                }
                else
                {
                    allNo = str2 + margin + 1;//���û���ϼ���λ,Ҳ�ӹ�����λ�ж��ȡһ��
                }
                //ȡ�ø�Ա���ĸ�λ���
                Hesion.Brick.Core.User us = new Hesion.Brick.Core.User();
                us = Hesion.Brick.Core.Users.GetUserByNumber(EmployeeNo);
                string jobNo = us.JobNumber;
                StringBuilder str_sql = new StringBuilder();
                str_sql.Append("SELECT top(");
                str_sql.Append(allNo);
                str_sql.Append(")HR_Employees.Number ");
                str_sql.Append("FROM HR_Employees  ");
                str_sql.Append("WHERE YGZT<>'��ְ' AND ");
                str_sql.Append("HR_Employees.Number <>'" + EmployeeNo + "' AND HR_Employees.Number<>'" + SuperioNo + "' and ");
                str_sql.Append("HR_Employees.JobNo ");
                str_sql.Append(" IN ");
                str_sql.Append("(SELECT Job_No ");
                str_sql.Append("FROM HR_Job_Interrelated ");
                str_sql.Append("WHERE parentNumber = ");
                str_sql.Append("'" + jobNo + "'");
                str_sql.Append(") ORDER BY NEWID()");
                DataSet ds = DbHelperSQL.Query(str_sql.ToString());
                int i = ds.Tables[0].Rows.Count;
                //�ж�ͬ�������Ƿ�С��Ҫ��ȡ����
                if (i != 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        InterrelatedEmployees.Add(ds.Tables[0].Rows[j]["Number"].ToString());
                    }
                }
            }
		
           
            return InterrelatedEmployees;
		}

        /// <summary>
        /// ������λ,ȡ�����������ĸ�λ����
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetAccordJobName()
        {
            ArrayList AllEmployees = new ArrayList();
            StringBuilder str_sql = new StringBuilder();
            str_sql.Append("select j.Number,j.JobName from HR_Jobs J ");
            str_sql.Append("inner join HR_Dept d on j.Job_Dept = d.DeptName  ");
            str_sql.Append("where (");
            str_sql.Append("select count(*) from Hr_Employees where BM=d.deptname) + ");
            str_sql.Append("(select count(*) from Hr_employees where JobNo in (select Job_No from  HR_Job_Interrelated where parentNumber=j.Number)");
            str_sql.Append(")<6 ");
            SqlDataReader dr = DbHelperSQL.ExecuteReader(str_sql.ToString ());
            while(dr.Read ())
            {
                AllEmployees.Add(dr["JobName"].ToString ());
            }
            dr.Close();
            return AllEmployees;
        }

        /// <summary>
        /// ������λ,ȡ�����������ĸ�λ����(���أ��޶�����) by galaxy
        /// </summary>
        /// <param name="Job_Dept">��������</param>
        /// <returns></returns>
        public static ArrayList GetAccordJobName(string Job_Dept)
        {
            ArrayList AllEmployees = new ArrayList();
            StringBuilder str_sql = new StringBuilder();
            str_sql.Append("select j.Number,j.JobName from HR_Jobs J ");
            str_sql.Append("inner join HR_Dept d on j.Job_Dept = d.DeptName  ");
            str_sql.Append("where (");
            str_sql.Append("select count(*) from Hr_Employees where BM=d.deptname) + ");
            str_sql.Append("(select count(*) from Hr_employees where JobNo in (select Job_No from  HR_Job_Interrelated where parentNumber=j.Number)");
            //str_sql.Append(")<6 ");
            str_sql.Append(")<6 and j.Job_Dept = '" + Job_Dept + "'");
            SqlDataReader dr = DbHelperSQL.ExecuteReader(str_sql.ToString());
            while (dr.Read())
            {
                AllEmployees.Add(dr["JobName"].ToString());
            }
            dr.Close();
            return AllEmployees;
        }
        /// <summary>
        /// ��ò���
        /// </summary>
        /// <param name="Employees"></param>
        /// <returns></returns>
        public static string getbm(string Employees)
        {
            string em = null;
            DataSet dss = DbHelperSQL.Query("select BM from hr_Employees where Number ='" + Employees + "'");
            em = dss.Tables[0].Rows[0]["BM"].ToString();
            return em;
        }
	}
}