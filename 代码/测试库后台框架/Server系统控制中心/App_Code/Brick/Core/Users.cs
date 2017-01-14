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
using FMDBHelperClass;
using System.Collections;
namespace Hesion.Brick.Core{



	/// <summary>
	/// 员工操作类
	/// </summary>
	public  abstract class Users{	



		/// <summary>
		 /// 通过员工编号获取员工信息
		/// </summary>
		/// <param name="UserNumber">员工编号</param>
		/// <returns></returns>
        public static User GetUserByNumber(string UserNumber)
        {
            User user = new User();
            if (UserNumber != null)
            {
                string strSQL = "SELECT HR_Employees.Number, HR_Employees.Employee_Name, HR_Employees.JobNo, HR_Jobs.JobName, HR_Dept.DeptName,HR_Dept.Superior  ";
                strSQL += "FROM HR_Employees INNER JOIN HR_Jobs ON HR_Employees.JobNo = HR_Jobs.Number INNER JOIN HR_Dept ON HR_Jobs.Dept_No = HR_Dept.Number ";
                strSQL += "where HR_Employees.Number =@number";
                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");                
                Hashtable ht = new Hashtable();
                ht["@number"] = UserNumber;
               Hashtable returnHT = I_DBL.RunParam_SQL(strSQL, "", ht);
               if (!(bool)returnHT["return_float"])
                {
                    return user;
                }
               DataSet dsReturn = (DataSet)returnHT["return_ds"];

                if (dsReturn!=null&&dsReturn.Tables.Count>0&&dsReturn.Tables[0].Rows.Count>0)
                {
                    DataRow dr_HR_Employees_info = dsReturn.Tables[0].Rows[0];

                    user.Number = dr_HR_Employees_info["Number"].ToString();
                    user.Name = dr_HR_Employees_info["Employee_Name"].ToString();
                    user.JobNumber = dr_HR_Employees_info["JobNo"].ToString();
                    user.JobName = dr_HR_Employees_info["JobName"].ToString();
                    user.DeptName = dr_HR_Employees_info["DeptName"].ToString();
                    user.LSName = dr_HR_Employees_info["Superior"].ToString();
                }
            }
            return user;
		}
	
		/// <summary>
		 /// 通过员工名称获取员工信息
		/// </summary>
        /// <param name="UserName">员工名称</param>
		/// <returns></returns>
		public static User GetUserByName(string UserName){

            User user = new User();
            if (UserName != null)
            {
                string strSQL = "SELECT HR_Employees.Number, HR_Employees.Employee_Name, HR_Jobs.Number AS jobNo, HR_Employees.JobNo, HR_Jobs.JobName, HR_Dept.DeptName ";
                strSQL += "FROM HR_Employees INNER JOIN HR_Jobs ON HR_Employees.JobNo = HR_Jobs.Number INNER JOIN HR_Dept ON HR_Jobs.Dept_No = HR_Dept.Number ";
                strSQL += "where HR_Employees.Employee_Name =@Employee_Name";
                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
                DataSet ds = new DataSet();
                Hashtable ht = new Hashtable();
                ht["@Employee_Name"] = UserName;
                Hashtable returnHT = I_DBL.RunParam_SQL(strSQL, "", ht);
                if (!(bool)returnHT["return_float"])
                {
                    return user;
                }
                DataSet dsReturn = (DataSet)returnHT["return_ds"];

                if (dsReturn != null && dsReturn.Tables[0].Rows.Count > 0)
                {
                    DataRow dr_HR_Employees_info = dsReturn.Tables[0].Rows[0];

                    user.Number = dr_HR_Employees_info["Number"].ToString();
                    user.Name = dr_HR_Employees_info["Employee_Name"].ToString();
                    user.JobNumber = dr_HR_Employees_info["jobNo"].ToString();
                    user.JobName = dr_HR_Employees_info["JobName"].ToString();
                    user.DeptName = dr_HR_Employees_info["DeptName"].ToString();
                }
                
            }
            return user;
		}
		
	}
}