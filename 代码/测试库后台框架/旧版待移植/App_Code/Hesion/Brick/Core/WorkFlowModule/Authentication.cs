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
using System.IO;
using Hesion.Brick.Core;
using FMOP.DB;
namespace Hesion.Brick.Core.WorkFlow{



	/// <summary>
	/// 工作流权限验证类
	/// </summary>
	public class Authentication{	

		/// <summary>
		/// 权限数据集
		/// </summary>
		private DataSet RoleAuth;

		/// <summary>
		/// 工作流模块基本属性
		/// </summary>
		private Property property;


        #region
        public Authentication()
        {

        }
        /// <summary>
		/// 通过工作流模块基本属性和xml节点构造本类
		/// </summary>
		/// <param name="pro"></param>
		/// <param name="Node"></param>
		public Authentication(Property pro, XmlNode Node){
            if (pro.AuthType != 0 || Node != null)
            {
                property = pro;
                //把Node节点填充到DATASET
                DataSet ds = new DataSet();
                if (Node != null)
                {
                    string nodetext = "<Authentication>" + Node.InnerXml + "</Authentication>";
                    StringReader sr = new StringReader(nodetext);
                    XmlTextReader xr = new XmlTextReader(sr);
                    ds.ReadXml(xr);
                    RoleAuth = ds;
                }
            }
            else if (pro.AuthType == 0)
            {
                property = pro;
            }

        }
        #endregion

        /// <summary>
        /// 是否有此岗位的权限信息
        /// </summary>
        /// <param name="EmployeeNo"></param>
        /// <returns></returns>
        public bool IsRoportRole( string UserNumber)
        {
            
            bool isroport = false;
            if (UserNumber != null)
            {
                User us = new User();
                us = Users.GetUserByNumber(UserNumber);
                string JobName = us.JobName;
                string DeptName = us.DeptName;
                try
                {
                    DataRow[] rowUserNumber = RoleAuth.Tables[0].Select("roleName='" + UserNumber + "'");
                    DataRow[] rowJobName = RoleAuth.Tables[0].Select("roleName='" + JobName + "'");
                    DataRow[] rowDeptName = RoleAuth.Tables[0].Select("roleName='" + DeptName + "'");
                    //DataRow[] row = RoleAuth.Tables[0].Select("roleName='" + JobName + "'");
                    //if (row.Length == 1)
                    //{
                    //    isroport = true;
                    //}
                    if (rowUserNumber.Length == 1 || rowJobName.Length == 1 || rowDeptName.Length == 1)
                    {
                        isroport = true;
                    }
                }
                catch
                {
                    isroport = false;
                }
            }
            return isroport;
            
        }
        #region

        /// <summary>
        /// 返回权限列表
        /// </summary>
        public DataSet retunauthds
        {
            get {
                return this.RoleAuth; 
                }
        }
        /// <summary>
		/// 通过员工编号获取该员工对该模块的访问权限
		/// </summary>
		/// <param name="UserNumber"></param>
		/// <returns></returns>
        public UserModuleAuth GetAuthByUserNumber(string UserNumber)
        {
            //UserModuleAuth实例化
            UserModuleAuth uma = new UserModuleAuth();
            //string Autype = property.AuthType;
            if (property == null) return uma;
                if (property.AuthType == 0)
                {
                    uma.CanAdd = uma.CanModify = uma.CanView  = true;
                    uma.ViewType = 3;
                    uma.ModifyType = 3;
                    return uma;
                }
                else
                {
                    User us = new User();
                    us = Users.GetUserByNumber(UserNumber);
                    string JobName = us.JobName;
                    string Dept = us.DeptName;
                    if (RoleAuth == null||RoleAuth.Tables.Count <= 0 || RoleAuth.Tables[0].Rows.Count <= 0) return uma;
                    //DataRow[] ROW = RoleAuth.Tables[0].Rows;
                    //try
                    //{
                        DataRow[] rowjobName = RoleAuth.Tables[0].Select("roleName='" + JobName + "'");
                        DataRow[] rowDeptName = RoleAuth.Tables[0].Select("roleName='" + Dept + "'");
                        DataRow[] rowUserNumber = RoleAuth.Tables[0].Select("roleName='" + UserNumber + "'");

                        if (rowUserNumber.Length == 1)
                        {
                            foreach (DataRow dr in rowUserNumber)
                            {
                                uma.CanAdd = bool.Parse(dr["canAdd"].ToString());
                                if (uma.CanAdd == true)
                                {
                                    uma.CanView = true;
                                }
                                else
                                {
                                    uma.CanView = bool.Parse(dr["canView"].ToString());
                                }
                                uma.CanModify = bool.Parse(dr["canModify"].ToString());
                                try
                                {
                                    uma.ViewType = int.Parse(dr["viewType"].ToString());
                                }
                                catch
                                {
                                    uma.ViewType = 1;
                                }
                                try
                                {
                                    uma.ModifyType = int.Parse(dr["modifyType"].ToString());
                                }
                                catch
                                {
                                    uma.ModifyType = 1;
                                }
                                try
                                {
                                    uma.RoleType = int.Parse(dr["roleType"].ToString());
                                }
                                catch
                                {
                                    uma.RoleType = 1;
                                }
                            }
                            return uma;

                        }
                        else if (rowjobName.Length == 1)
                        {
                            foreach (DataRow dr in rowjobName)
                            {
                                uma.CanAdd = bool.Parse(dr["canAdd"].ToString());
                                if (uma.CanAdd == true)
                                {
                                    uma.CanView = true;
                                }
                                else
                                {
                                    uma.CanView = bool.Parse(dr["canView"].ToString());
                                }
                                uma.CanModify = bool.Parse(dr["canModify"].ToString());
                                try
                                {
                                    uma.ViewType = int.Parse(dr["viewType"].ToString());
                                }
                                catch
                                {
                                    uma.ViewType = 1;
                                }
                                try
                                {
                                    uma.ModifyType = int.Parse(dr["modifyType"].ToString());
                                }
                                catch
                                {
                                    uma.ModifyType = 1;
                                }
                                try
                                {
                                    uma.RoleType = int.Parse(dr["roleType"].ToString());
                                }
                                catch
                                {
                                    uma.RoleType = 1;
                                }
                            }
                            return uma;
                        }
                        else if (rowDeptName.Length == 1)
                        {

                            foreach (DataRow dr in rowDeptName)
                            {
                                uma.CanAdd = bool.Parse(dr["canAdd"].ToString());
                                if (uma.CanAdd == true)
                                {
                                    uma.CanView = true;
                                }
                                else
                                {
                                    uma.CanView = bool.Parse(dr["canView"].ToString());
                                }
                                uma.CanModify = bool.Parse(dr["canModify"].ToString());
                                try
                                {
                                    uma.ViewType = int.Parse(dr["viewType"].ToString());
                                }
                                catch
                                {
                                    uma.ViewType = 1;
                                }
                                try
                                {
                                    uma.ModifyType = int.Parse(dr["modifyType"].ToString());
                                }
                                catch
                                {
                                    uma.ModifyType = 1;
                                }
                                try
                                {
                                    uma.RoleType = int.Parse(dr["roleType"].ToString());
                                }
                                catch
                                {
                                    uma.RoleType = 1;
                                }
                            }
                            return uma;
                        }


                    //}
                    //catch
                    //{
                    //    uma.CanAdd = uma.CanModify  = uma.CanView = false;
                    //}

                }
            return uma;
        }
        #endregion
        /// <summary>
        /// 遍历分配权限列表，划分为人自动写入报警表 开发：刘杰2009-09-01
        /// </summary>
        /// <param name="context">报警内容</param>
        /// <param name="url">"链接路径"</param>
        /// <param name="grade">类型</param>
        /// <param name="fromuser">来源人</param>
        public void AddWarn(string context, string url, string grade, string fromuser)
        {
            DataTable dt = RoleAuth.Tables[0];
            DataTable dt1 = null;
            if (dt.Rows.Count == 0)
            {
                return;
            }
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (dt.Rows[i][1].ToString() == "3")  //个人
                {

                    dt1 = null;
                    dt1 = DbHelperSQL.Query("select Number from HR_Employees where Number='" + dt.Rows[i][0].ToString().Trim() + "'and YGZT<>'离职'").Tables[0];

                }
                if (dt.Rows[i][1].ToString() == "1") //职位
                {
                    dt1 = null;
                    dt1 = DbHelperSQL.Query("select Number from HR_Employees where GWMC='" + dt.Rows[i][0].ToString().Trim() + "'and YGZT<>'离职'").Tables[0];


                }
                if (dt.Rows[i][1].ToString() == "2") //部门
                {
                    dt1 = null;
                    dt1 = DbHelperSQL.Query("select Number from HR_Employees where BM='" + dt.Rows[i][0].ToString().Trim() + "'and YGZT<>'离职'").Tables[0];

                }

                if (dt1.Rows.Count > 0)
                {
                    for (int j = 0; j <= dt1.Rows.Count - 1; j++)
                    {
                        //Response.Write(dt1.Rows[j][0].ToString() + " ");
                        int rcount1 = InsertWarnings(context.Trim(), url.Trim(), grade.Trim(), fromuser.Trim(), dt1.Rows[j][0].ToString());
                    }

                }
            }
        }
        /// <summary>
        /// 插入报警表，自定义使用 开发：刘杰2009-09-01
        /// </summary>
        /// <param name="context"></param>
        /// <param name="url"></param>
        /// <param name="grade"></param>
        /// <param name="fromuser"></param>
        /// <param name="touser"></param>
        /// <returns></returns>
        public int InsertWarnings(string context, string url, string grade, string fromuser, string touser)
        {
            string sql = "insert into User_Warnings(Context,Module_Url,Grade,FromUser,Touser)";
            sql = sql + " values('" + context.Trim() + "','" + url.Trim() + "','" + grade.Trim() + "','" + fromuser.Trim() + "','" + touser.Trim() + "')";
            int Rcount = DbHelperSQL.ExecuteSql(sql);
            return Rcount;
        }
    }
}