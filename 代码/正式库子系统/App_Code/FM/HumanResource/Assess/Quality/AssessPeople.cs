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
	/// 评议人员
	/// </summary>
	public class AssessPeople{ 
        //部门
        private string BM;
        //上一级岗位编号
        private string SuperioNo;
        //同事不足３－４人的相差数
        private int margin = 0;
        ////随即抽取的总人数是否足够评议人数１：够０：未够
        //private int allmargin = 1;
		/// <summary>
		 /// 获取对某个员工评议的人员列表
		 /// 
		 /// 直接上级一人，同事3-20人，关联岗位1-20人，随机抽取
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
                string SuperioNumber = GetSuperioEmployeeNo(EmployeeNo);//获得上级岗位工号
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
		 /// 通过某员工的工号，获取其直接上级的工号
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
                str_sql.Append(" WHERE YGZT<>'离职' and HR_Employees.Number=");
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
		 /// 通过某一员工的工号，获取其同事的工号列表（排除该员工自身，以及他的上级）
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
                //随机取得人数３人或10人
                //int[] str = new int[2];
                //str[0] = 3;
                //str[1] = 4;
                Random ran = new Random();
                int str2 = ran.Next(100, 110);
                //查询字符窜
                StringBuilder str_sql = new StringBuilder();
                str_sql.Append("SELECT top(");
                str_sql.Append(str2);
                str_sql.Append(")HR_Employees.Number ");
                str_sql.Append("FROM HR_Employees  ");
                str_sql.Append("WHERE ");
                str_sql.Append("(BM='"+BM+"') ");
                str_sql.Append(" AND YGZT<>'离职' AND ");
                str_sql.Append("Number<>'"+EmployeeNo+"' ");
                str_sql.Append(" AND ");
                str_sql.Append("Number<>'" + SuperioNo + "' ");
                str_sql.Append(" ORDER BY NEWID()");
                DataSet ds = DbHelperSQL.Query(str_sql.ToString());
                int i = ds.Tables[0].Rows.Count;
                //判断同事人数是否小于３
                if (i<3)
                {
                    margin = 3 - i;//却少几人到3个
                }
                if (i != 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        DeptEmployees.Add(ds.Tables[0].Rows[j]["Number"].ToString());//把少于3人的同事编号添加到数组
                    }
                }
            }
            return DeptEmployees;
		}
	
		/// <summary>
		 /// 通过某员工的工号，获取关联岗位的员工列表
		/// </summary>
		/// <param name="EmployeeNo"></param>
		/// <returns></returns>
        private  ArrayList GetInterrelatedEmployees(string EmployeeNo)
        {
            ArrayList InterrelatedEmployees = new ArrayList();
            if (EmployeeNo != null)
            {
                //随机抽取关联岗位人数
                //int[] str = new int[2];
                //str[0] = 1;
                //str[1] = 2;
                Random ran = new Random();
                int str2 = ran.Next(1, 21);
                int allNo=str2;
                if (SuperioNo != null)
                {
                    allNo = str2 + margin;//如果上级岗位不为空时!如果同事不足3人,相差数到关联岗位中抽取
                }
                else
                {
                    allNo = str2 + margin + 1;//如果没有上级岗位,也从关联岗位中多抽取一人
                }
                //取得该员工的岗位编号
                Hesion.Brick.Core.User us = new Hesion.Brick.Core.User();
                us = Hesion.Brick.Core.Users.GetUserByNumber(EmployeeNo);
                string jobNo = us.JobNumber;
                StringBuilder str_sql = new StringBuilder();
                str_sql.Append("SELECT top(");
                str_sql.Append(allNo);
                str_sql.Append(")HR_Employees.Number ");
                str_sql.Append("FROM HR_Employees  ");
                str_sql.Append("WHERE YGZT<>'离职' AND ");
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
                //判断同事人数是否小于要抽取的数
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
        /// 遍历岗位,取得人数不够的岗位名称
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
        /// 遍历岗位,取得人数不够的岗位名称(重载，限定部门) by galaxy
        /// </summary>
        /// <param name="Job_Dept">部门名称</param>
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
        /// 获得部门
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