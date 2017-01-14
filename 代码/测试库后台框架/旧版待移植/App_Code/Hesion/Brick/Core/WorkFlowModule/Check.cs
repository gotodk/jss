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
using FMOP.SP_OnChecks;
using Hesion.Brick.Core.WorkFlow;
using ShareLiu.DXS;  //短信发送空间 刘杰 2010-07-14
namespace Hesion.Brick.Core.WorkFlow
{



    /// <summary>
    /// 审批类
    /// </summary>
    public class Check
    {

        /// <summary>
        /// 审批人数据集
        /// </summary>
        private DataSet Check_Role;

        /// <summary>
        /// 工作流模块基本属性
        /// </summary>
        private Property property;

        /// <summary>
        /// 提醒类(用于创建提醒信息)
        /// </summary>
        private Warning warning;

        /// <summary>
        /// Check节点
        /// </summary>


        /// <summary>
        /// 通过工作流模块基本属性,Xml节点,提醒类构造本类
        /// </summary>
        /// <param name="pro">工作流模块基本属性</param>
        /// <param name="Node">Check节点</param>
        /// <param name="warning">提醒数据集</param>
        public Check(Property pro, XmlNode Node, Warning warning)
        {
            if (pro != null && Node != null)
            {
                //获得pro
                //string ModuleName = pro.ModuleName;
                property = pro;
                //把节点填充到DATASET
                DataSet ds = new DataSet();
                string nodetext = "<Check>" + Node.InnerXml + "</Check>";
                StringReader sr = new StringReader(nodetext);
                XmlTextReader xtr = new XmlTextReader(sr);
                ds.ReadXml(xtr);
                Check_Role = ds;
                //warning传值
                this.warning = warning;
            }
        }

        public DataSet retunchekds
        {
            get
            {
                return this.Check_Role;
            }
        }

        #region Check节点内容是否有为空
        /// <summary>
        /// Check节点内容是否有为空
        /// </summary>
        /// <returns>true:不为空 false:内容为空</returns>
        public bool GetCheckedContent()
        {
            bool IsNull = false;
            if ((Check_Role != null) && (Check_Role.Tables.Count > 0))
            {
                if (Check_Role.Tables[0].Rows.Count == 0)
                {
                    IsNull = false;
                }
                else
                {
                    IsNull = true;
                }
            }
            return IsNull;
        }
        #endregion

        #region 判断模块中是否有此审批岗位
        /// <summary>
        /// 判断模块中是否有此审批岗位
        /// </summary>
        /// <param name="UserNumber">员工编号</param>
        /// <returns></returns>
        public bool isCheckName(string UserNumber)
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
                    DataRow[] rowUserNumber = Check_Role.Tables[0].Select("roleName='" + UserNumber + "'");
                    DataRow[] rowJobName = Check_Role.Tables[0].Select("roleName='" + JobName + "'");
                    DataRow[] rowDeptName = Check_Role.Tables[0].Select("roleName='" + DeptName + "'");
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
        #endregion

        #region 查询此单号的最终审批状态
        /// <summary>
        /// 查询此单号的最终审批状态
        /// </summary>
        /// <param name="Number">单号</param>
        /// <returns></returns>
        public string GetCheckState(string Number)
        {
            string CheckState = null;
            if (property != null && Number != null)
            {
                string module = property.ModuleName;
                StringBuilder sql = new StringBuilder();
                //sql.Append("SELECT case when CheckState = 1 then '已审' else '审批中' end as CheckState ");
                sql.Append("SELECT  CheckState ");
                sql.Append("from ");
                sql.Append(module);
                sql.Append(" where (Number='");
                sql.Append(Number);
                sql.Append("') ");
                SqlDataReader drCheckState = DbHelperSQL.ExecuteReader(sql.ToString());
                if (drCheckState.Read())
                {
                    string type = drCheckState["CheckState"].ToString();
                    switch (type)
                    {
                        case "0":
                            CheckState = "审批中！";
                            break;
                        case "1":
                            CheckState = "审批完成！";
                            break;
                        case "2":
                            CheckState = "审批驳回！";
                            break;
                    }
                }
                drCheckState.Close();
            }
            return CheckState;
        }
        #endregion

        #region 获取已经对某张单据审批的所有员工名称,员工名称之间以逗号(,)分割
        /// <summary>
        /// 获取已经对某张单据审批的所有员工名称,员工名称之间以逗号(,)分割
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        public string GetCheckedUserName(string OrderNumber)
        {
            string getallcheck = null;
            if (property != null && OrderNumber != null)
            {
                string ModuleName = property.ModuleName.ToString();
                //读取check
                SqlDataReader drCheckerGather = DbHelperSQL.ExecuteReader("select Checker from dbo.system_CheckInfo where ((ModuleName='" + ModuleName + "') AND (Number='" + OrderNumber + "')) ");
                while (drCheckerGather.Read())
                {
                    string Checker = drCheckerGather[0].ToString();
                    //用逗号进行间隔
                    getallcheck = getallcheck + Checker + ',';
                }
                drCheckerGather.Close();

                //取得字符串的长度
                if (getallcheck != null)
                {
                    int _AllCheckJobslength = getallcheck.Length - 1;
                    //移除最后一位（也就是去掉最后的，操作）
                    getallcheck = getallcheck.Remove(_AllCheckJobslength);
                }
            }
            return getallcheck;
        }
        #endregion

        #region 获取应该对该单据审批，但尚未审批的所有员工岗位
        /// <summary>
        /// 获取应该对该单据审批，但尚未审批的所有员工岗位
        /// </summary>
        /// <param name="OrderNumber">单号</param>
        /// <param name="types">1格式为(admin)/2格式为(3:admin)</param>
        /// <returns></returns>
        public string GetUnCheckedJobName(string OrderNumber, int types)
        {
            string allJobName = null;
            if (property != null && OrderNumber != null)
            {
                //取得模块名
                string ModuleName = property.ModuleName;
                StringBuilder sql = new StringBuilder();
                sql.Append("select NextChecker, CheckState from ");
                sql.Append(ModuleName);
                sql.Append(" where Number='" + OrderNumber + "'");
                SqlDataReader DrNextJobName = DbHelperSQL.ExecuteReader(sql.ToString());
                if (DrNextJobName.Read())
                {
                    //
                    string strNextChecker = DrNextJobName["NextChecker"].ToString();
                    if (DrNextJobName["CheckState"].ToString() == "1" || DrNextJobName["CheckState"].ToString() == "2")
                    {
                        return allJobName;
                    }

                    if (strNextChecker.IndexOf(":") <= 0)
                    {
                        DrNextJobName.Close();
                        return allJobName;
                    }
                    string[] str = strNextChecker.Split(':');
                    if (str.Length != 2) return allJobName;
                    string roleType = str[0].ToString();
                    string NextChecker = str[1].ToString();
                    int rowCount = Check_Role.Tables[0].Rows.Count;
                    for (int i = 0; i < rowCount; i++)
                    {
                        string jobname = Check_Role.Tables[0].Rows[i]["roleName"].ToString();
                        int chekType = int.Parse(Check_Role.Tables[0].Rows[i]["checkType"].ToString());
                        if (chekType == 2)
                        {
                            //string EMNO = GetCreateUser(OrderNumber);
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            SqlDataReader drr = DbHelperSQL.ExecuteReader("SELECT JobName FROM dbo.HR_Jobs WHERE Number =(select SuperioJobNo from dbo.HR_Jobs where Number=(select JobNo from dbo.HR_Employees where Number ='" + EMNO + "')) ");
                            if (drr.Read())
                            {
                                jobname = drr["JobName"].ToString();
                            }
                            drr.Close();
                        }
                        //获取特殊审批上级
                        if (chekType == 3)
                        {
                            //string EMNO = GetCreateUser(OrderNumber);
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            SqlDataReader drr = DbHelperSQL.ExecuteReader("SELECT JobName FROM dbo.HR_Jobs WHERE Number =(SELECT SPGWXZ FROM dbo.TSSPSZ where MBGWXZ =(select JobNo from dbo.HR_Employees where Number ='" + EMNO + "')) ");
                            if (drr.Read())
                            {
                                jobname = drr["JobName"].ToString();
                            }
                            drr.Close();
                        }


                        if (jobname == NextChecker)
                        {
                            if (i > (rowCount - 1))
                            {
                                allJobName = " ,";
                                //int iswaring = warning.CreateWarningToRole(OrderNumber, 4, UserNumber, CreateUserNumber);
                                break;
                            }
                            for (int j = i; j < rowCount; j++)
                            {
                                string cktype = Check_Role.Tables[0].Rows[j]["checkType"].ToString();
                                string rtype = Check_Role.Tables[0].Rows[j]["roleType"].ToString();
                                if (types == 1)
                                {
                                    //cktype=2为直接上级,提取的时候直接提取直接上级的岗位名称
                                    if (cktype != "2" && cktype != "3")
                                    {
                                        jobname = Check_Role.Tables[0].Rows[j]["roleName"].ToString();
                                    }
                                    //如果rtype为3表示为个人审核项,显示的时候按员工名称显示不按员工编号显示
                                    if (rtype == "3")
                                    {
                                        string userNumber = Check_Role.Tables[0].Rows[j]["roleName"].ToString();
                                        Hesion.Brick.Core.User us = Hesion.Brick.Core.Users.GetUserByNumber(userNumber);
                                        jobname = us.Name;
                                    }
                                    allJobName = allJobName + jobname + ",";
                                }
                                else if (types == 2)
                                {

                                    if (cktype != "2" && cktype != "3")
                                    {
                                        jobname = rtype + ":" + Check_Role.Tables[0].Rows[j]["roleName"].ToString();
                                    }
                                    else
                                    {
                                        jobname = rtype + ":" + jobname;
                                    }

                                    allJobName = allJobName + jobname + ",";
                                }
                            }
                            break;
                        }
                    }

                }
                DrNextJobName.Close();
                if (allJobName != null)
                {
                    int lg = allJobName.Length - 1;
                    return allJobName.Remove(lg);
                }
            }
            return allJobName;
        }
        #endregion

        #region 获取该单据下一级审批岗位
        /// <summary>
        /// 获取该单据下一级审批岗位
        /// </summary>
        /// <param name="OrderNumber">单号</param>
        /// <returns></returns>
        public string GetNextCheckRole(string OrderNumber)
        {
            string NextJobName = null;
            if (property != null && OrderNumber != null)
            {
                //取得模块名
                string ModuleName = property.ModuleName;
                StringBuilder sql = new StringBuilder();
                sql.Append("select NextChecker  from ");
                sql.Append(ModuleName);
                sql.Append(" where Number='" + OrderNumber + "'");
                SqlDataReader DrNextJobName = DbHelperSQL.ExecuteReader(sql.ToString());
                if (DrNextJobName.Read())
                {
                    //取得定单下一级要审批的信息
                    string strNextChecker = DrNextJobName["NextChecker"].ToString();
                    if (strNextChecker == null)
                    {
                        DrNextJobName.Close();
                        return NextJobName;
                    }
                    //获得审批岗位分割字符串
                    string[] splitstring = FMOP.SS.SplitString.GetArralist(strNextChecker, ":");
                    string roleType = splitstring[0].ToString();
                    //取得要审核岗位的内容
                    string NextChecker = splitstring[1].ToString();
                    //string[] job = 
                    int rowCount = Check_Role.Tables[0].Rows.Count;
                    for (int i = 0; i < rowCount; i++)
                    {
                        string bfcheckType = Check_Role.Tables[0].Rows[i]["checkType"].ToString();
                        string jobname = Check_Role.Tables[0].Rows[i]["roleName"].ToString();
                        if (bfcheckType == "2")
                        {
                            //获得创建订单人员工编号
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = superioJobName(EMNO);
                        }
                        if (bfcheckType == "3")
                        {
                            //获得创建订单人员工编号
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = sp_JobName(EMNO);
                        }
                        if (jobname == NextChecker)
                        {
                            int nexti = i + 1;
                            //下一岗位的dataset行是否大于总行数如果大于岗位为空
                            if (nexti < rowCount)
                            {
                                //获取roleType
                                string roletype = null;
                                try
                                {
                                    roletype = Check_Role.Tables[0].Rows[nexti]["roleType"].ToString();
                                }
                                catch
                                {
                                    roletype = "1";
                                }
                                //获的checkType
                                int chekType = int.Parse(Check_Role.Tables[0].Rows[nexti]["checkType"].ToString());
                                if (chekType == 1)
                                {
                                    //if (roletype == "3" && AuthTransfer.getTransferUser(Check_Role.Tables[0].Rows[0]["roleName"].ToString()) != "")
                                    //    NextJobName = roletype + ":" + AuthTransfer.getTransferUser(Check_Role.Tables[0].Rows[0]["roleName"].ToString());
                                    //else
                                    NextJobName = roletype + ":" + Check_Role.Tables[0].Rows[nexti]["roleName"].ToString();
                                }
                                else if (chekType == 2)//为直接上级的审批岗位
                                {
                                    NextJobName = "1:" + superioJobName(OrderNumber);
                                }
                                //获取特殊审批上级岗位
                                else if (chekType == 3)
                                {
                                    NextJobName = "1:" + sp_JobName(OrderNumber);
                                }
                            }
                            else
                            {
                                return NextJobName;
                            }
                            break;
                        }
                    }

                }
                DrNextJobName.Close();
            }
            return NextJobName;
        }
        #endregion
        #region 获取该单据下一级审批岗位 算法2 刘杰 2010-07-13 加入启用短信判断
        /// <summary>
        /// 获取该单据下一级审批岗位
        /// </summary>
        /// <param name="OrderNumber">单号</param>
        /// <returns></returns>
        public string[] GetNextCheckRole2(string OrderNumber)
        {
            string[] aa = new string[2];
            string NextJobName = null;
            string mobileE="0";
            if (property != null && OrderNumber != null)
            {
                
                //取得模块名

                string ModuleName = property.ModuleName;
                StringBuilder sql = new StringBuilder();
                sql.Append("select NextChecker  from ");
                sql.Append(ModuleName);
                sql.Append(" where Number='" + OrderNumber + "'");
                SqlDataReader DrNextJobName = DbHelperSQL.ExecuteReader(sql.ToString());
                if (DrNextJobName.Read())
                {
                    //取得定单下一级要审批的信息
                    string strNextChecker = DrNextJobName["NextChecker"].ToString();
                    if (strNextChecker == null)
                    {
                        DrNextJobName.Close();
                   
                        aa[0] = NextJobName;
                        aa[1] = "0";
                        return aa;
                    }
                    //获得审批岗位分割字符串
                    string[] splitstring = FMOP.SS.SplitString.GetArralist(strNextChecker, ":");
                    string roleType = splitstring[0].ToString();
                    //取得要审核岗位的内容
                    string NextChecker = splitstring[1].ToString();
                    //string[] job = 
                    int rowCount = Check_Role.Tables[0].Rows.Count;
                    for (int i = 0; i < rowCount; i++)
                    {
                        string bfcheckType = Check_Role.Tables[0].Rows[i]["checkType"].ToString();
                        string jobname = Check_Role.Tables[0].Rows[i]["roleName"].ToString();
                        if (bfcheckType == "2")
                        {
                            //获得创建订单人员工编号
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = superioJobName(EMNO);
                        }
                        if (bfcheckType == "3")
                        {
                            //获得创建订单人员工编号
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = sp_JobName(EMNO);
                        }
                        if (jobname == NextChecker)
                        {
                            int nexti = i + 1;
                            //下一岗位的dataset行是否大于总行数如果大于岗位为空
                            if (nexti < rowCount)
                            {
                                //获取roleType
                                string roletype = null;
                                try
                                {
                                    roletype = Check_Role.Tables[0].Rows[nexti]["roleType"].ToString();
                                }
                                catch
                                {
                                    roletype = "1";
                                }
                                //获的checkType
                                int chekType = int.Parse(Check_Role.Tables[0].Rows[nexti]["checkType"].ToString());
                                try
                                {
                                    mobileE = Check_Role.Tables[0].Rows[nexti]["cMobileEnable"].ToString();
                                }
                                catch (Exception e2)
                                {
                                    mobileE = "0";
                                }
                                if (chekType == 1)
                                {

                                    NextJobName = roletype + ":" + Check_Role.Tables[0].Rows[nexti]["roleName"].ToString();

                                }
                                else if (chekType == 2)//为直接上级的审批岗位
                                {
                                    NextJobName = "1:" + superioJobName(OrderNumber);

                                }
                                //获取特殊审批上级岗位
                                else if (chekType == 3)
                                {
                                    NextJobName = "1:" + sp_JobName(OrderNumber);
                                }
                                aa[0] = NextJobName;
                                aa[1] = mobileE;

                            }
                            else
                            {
                                aa[0] = NextJobName;
                                aa[1] ="0";
                                return aa;
                            }
                            break;
                        }
                    }

                }
                DrNextJobName.Close();
            }
            return aa;
        }
        #endregion

        #region 获取对该模块进行审批的第一级岗位
        /// <summary>
        /// 获取对该模块进行审批的第一级岗位
        /// </summary>
        /// <returns></returns>
        public string GetFirstCheckRole(string OrderNumber, string user)
        {
            string firstJobName = "";
            if (Check_Role != null && (Check_Role.Tables.Count > 0))
            {
                string roletype = null;
                string module = property.ModuleName;
                try
                {
                    roletype = Check_Role.Tables[0].Rows[0]["roleType"].ToString();
                }
                catch
                {
                    roletype = "1";
                }

                int chekType = int.Parse(Check_Role.Tables[0].Rows[0]["checkType"].ToString());
                if (chekType == 1)
                {
                    string jname=AuthTransfer.getTransferUser(Check_Role.Tables[0].Rows[0]["roleName"].ToString());
                    if (roletype == "3" && jname!="")
                        firstJobName = roletype + ":" + jname;
                    else
                    firstJobName = roletype + ":" + Check_Role.Tables[0].Rows[0]["roleName"].ToString();

                }
                else if (chekType == 2)
                {
                    //string jname = Check_Role.Tables[0].Rows[0]["roleName"].ToString();
                    //获得创建定单人员工号
                    string EMNO = user;
                    //string EMNO = Orders.GetCreatUserNumber(module, OrderNumber);
                    SqlDataReader drr = DbHelperSQL.ExecuteReader("SELECT JobName FROM dbo.HR_Jobs WHERE Number =(select SuperioJobNo from dbo.HR_Jobs where Number=(select JobNo from dbo.HR_Employees where Number ='" + EMNO + "')) ");
                    if (drr.Read())
                    {
                        firstJobName = "1:" + drr["JobName"].ToString();
                    }
                    drr.Close();
                }
                //获取特殊审批上级岗位
                else if (chekType == 3)
                {
                    //string jname = Check_Role.Tables[0].Rows[0]["roleName"].ToString();
                    //获得创建定单人员工号
                    string EMNO = user;
                    //string EMNO = Orders.GetCreatUserNumber(module, OrderNumber);
                    SqlDataReader drr = DbHelperSQL.ExecuteReader("SELECT JobName FROM dbo.HR_Jobs WHERE Number =(SELECT SPGWXZ FROM dbo.TSSPSZ where MBGWXZ =(select JobNo from dbo.HR_Employees where Number ='" + EMNO + "')) ");
                    if (drr.Read())
                    {
                        firstJobName = "1:" + drr["JobName"].ToString();
                    }
                    drr.Close();
                }
            }
            return firstJobName;
        }
        #endregion
        #region 获取对该模块进行审批的第一级岗位算法2 刘杰 2010-07-12 加入
        /// <summary>
        /// 获取对该模块进行审批的第一级岗位
        /// </summary>
        /// <returns></returns>
        public string GetFirstCheckRole2(string OrderNumber, string user)
        {
            string firstJobName = "";
            if (Check_Role != null && (Check_Role.Tables.Count > 0))
            {
                string roletype = null;
                string module = property.ModuleName;
                try
                {
                    roletype = Check_Role.Tables[0].Rows[0]["roleType"].ToString();
                }
                catch
                {
                    roletype = "1";
                }

                int chekType = int.Parse(Check_Role.Tables[0].Rows[0]["checkType"].ToString());
                string mobileE = Check_Role.Tables[0].Rows[0]["cMobileEnable"].ToString();
                if (chekType == 1)
                {
                    
                    string jname = AuthTransfer.getTransferUser(Check_Role.Tables[0].Rows[0]["roleName"].ToString());
                    if (roletype == "3" && jname != "")
                        firstJobName = roletype + ":" + jname+":"+mobileE;
                    else
                        firstJobName = roletype + ":" + Check_Role.Tables[0].Rows[0]["roleName"].ToString() + ":" + mobileE;

                }
                else if (chekType == 2)
                {
                    //string jname = Check_Role.Tables[0].Rows[0]["roleName"].ToString();
                    //获得创建定单人员工号
                    string EMNO = user;
                    //string EMNO = Orders.GetCreatUserNumber(module, OrderNumber);
                    SqlDataReader drr = DbHelperSQL.ExecuteReader("SELECT JobName FROM dbo.HR_Jobs WHERE Number =(select SuperioJobNo from dbo.HR_Jobs where Number=(select JobNo from dbo.HR_Employees where Number ='" + EMNO + "')) ");
                    if (drr.Read())
                    {
                        firstJobName = "1:" + drr["JobName"].ToString() + ":" + mobileE;
                    }
                    drr.Close();
                }
                //获取特殊审批上级岗位
                else if (chekType == 3)
                {
                    //string jname = Check_Role.Tables[0].Rows[0]["roleName"].ToString();
                    //获得创建定单人员工号
                    string EMNO = user;
                    //string EMNO = Orders.GetCreatUserNumber(module, OrderNumber);
                    SqlDataReader drr = DbHelperSQL.ExecuteReader("SELECT JobName FROM dbo.HR_Jobs WHERE Number =(SELECT SPGWXZ FROM dbo.TSSPSZ where MBGWXZ =(select JobNo from dbo.HR_Employees where Number ='" + EMNO + "')) ");
                    if (drr.Read())
                    {
                        firstJobName = "1:" + drr["JobName"].ToString() + ":" + mobileE;
                    }
                    drr.Close();
                }
            }
            return firstJobName;
        }
        #endregion

        #region 对某张单据进行审批
        /// <summary>
        /// 对某张单据进行审批
        /// </summary>
        /// <param name="OrderNumber">单号</param>
        /// <param name="UserNumber">员工编号</param>
        /// <param name="Ispass">是否通过</param>
        /// <param name="Reamrk">备注</param>
        /// <returns>0:审批不成功１：审批成功</returns>
        public short DoCheck(string OrderNumber, string UserNumber, bool Ispass, string Reamrk ,string Dailiren)
        {
            if (OrderNumber != null && UserNumber != null && Reamrk != null)
            {
                //写入审批信息
                int iswrite = WriteCheckInfo(OrderNumber, UserNumber, Ispass, Reamrk, Dailiren);
                //取得下一级审核岗位
                string NextChecJobname = GetNextCheckRole(OrderNumber);
                string module = property.ModuleName;
                string Title = property.Title;
                //如果审批不通过发送给报警给创建者
                if (Ispass == false)
                {
                    int upState = UpdateState(OrderNumber, 2);
                    UpdateNextChecker(OrderNumber, NextChecJobname);
                    string CreateUserNumber = Orders.GetCreatUserNumber(property.ModuleName, OrderNumber);
                    //int iswaring = warning.CreateWarningToUser(OrderNumber, 4, UserNumber, CreateUserNumber);
                    string Context = "单号为:" + OrderNumber + "的" + Title + "审批驳回,请修改!";
                    string Module_Url = "WorkFlow_View.aspx?module=" + module;
                    //int iswaring = warning.CreateWarningToUser(OrderNumber, 4, UserNumber, CreateUserNumber);
                    Warning.AddWarning(Context, Module_Url, 2, UserNumber, CreateUserNumber);
                    string UnCheckedJobName = GetUnCheckedJobName(OrderNumber, 2);
                    if (UnCheckedJobName == null)
                    {
                        //Deletecheck(OrderNumber);
                        return 1;
                    }
                    string[] nxjbname = FMOP.SS.SplitString.GetArralist(UnCheckedJobName, ",");
                    Warning.SendSeeWarningsToUsers(OrderNumber, UserNumber, Title, Module_Url, nxjbname, 1);
                    //Deletecheck(OrderNumber);
                    return 1;
                }
                if (iswrite == 1)
                {
                    if (NextChecJobname == null)
                    {
                        int upState = UpdateState(OrderNumber, 1);
                        if (warning != null)
                            warning.CreateWarningToRole(OrderNumber, 4, UserNumber);
                        string CreateUserNumber = Orders.GetCreatUserNumber(property.ModuleName, OrderNumber);//取得创建人
                        string Context = "单号为:" + OrderNumber + "的" + Title + "审批完成!";
                        string Module_Url = "WorkFlow_View.aspx?module=" + module;
                        CustomWarning.AddWarning(Context, Module_Url, 1, UserNumber, CreateUserNumber);
                        //审批完成后执行审批后存储过程
                        SP_OnCheck.do_oncheck(module, OrderNumber);
                        return 1;
                    }
                    else
                    {
                        string[] str = NextChecJobname.Split(':');
                        string roleType = str[0].ToString();
                        string NextChecker = str[1].ToString();
                        if (roleType == "3")
                        {
                            UpdateNextChecker(OrderNumber, NextChecJobname);
                            string msg = "单号为:" + OrderNumber + "的" + Title + "请求审批!";
                            CustomWarning.CreateWarningToJobName(OrderNumber, msg, module, UserNumber, NextChecJobname, 1, GetNextCheckLimitTime(OrderNumber));
                        }
                        else if (roleType == "1" || roleType == "2")
                        {
                            UpdateNextChecker(OrderNumber, NextChecJobname);
                            //配置信息中的审批信息
                            //if (warning != null)
                            //    warning.CreateWarningToUser(OrderNumber, 4, UserNumber, NextChecker);
                            //如果没有提醒信息节点，自定提醒内容
                            string msg = "单号为:" + OrderNumber + "的" + Title + "请求审批!";
                            CustomWarning.CreateWarningToJobName(OrderNumber, msg, module, UserNumber, NextChecJobname, 1, GetNextCheckLimitTime(OrderNumber));
                        }
                        return 1;
                    }
                }
            }
            return 0;
        }
        #endregion
        #region 对某张单据进行审批 算法2，开启短信功能 刘杰 2010-07-13
        /// <summary>
        /// 对某张单据进行审批
        /// </summary>
        /// <param name="OrderNumber">单号</param>
        /// <param name="UserNumber">员工编号</param>
        /// <param name="Ispass">是否通过</param>
        /// <param name="Reamrk">备注</param>
        /// <returns>0:审批不成功１：审批成功</returns>
        public short DoCheck2(string OrderNumber, string UserNumber, bool Ispass, string Reamrk, string Dailiren)
        {
            string[] nextDX;
            if (OrderNumber != null && UserNumber != null && Reamrk != null)
            {
                //写入审批信息
                int iswrite = WriteCheckInfo(OrderNumber, UserNumber, Ispass, Reamrk, Dailiren);
                //取得下一级审核岗位
              //  string NextChecJobname = GetNextCheckRole(OrderNumber);
                nextDX = GetNextCheckRole2(OrderNumber);
                string NextChecJobname = nextDX[0];
                string mobileE = nextDX[1].ToString();
                string module = property.ModuleName;
                string Title = property.Title;
                //如果审批不通过发送给报警给创建者
                if (Ispass == false)
                {
                    int upState = UpdateState(OrderNumber, 2);
                    UpdateNextChecker(OrderNumber, NextChecJobname);
                    string CreateUserNumber = Orders.GetCreatUserNumber(property.ModuleName, OrderNumber);
                    //int iswaring = warning.CreateWarningToUser(OrderNumber, 4, UserNumber, CreateUserNumber);
                    string Context = "单号为:" + OrderNumber + "的" + Title + "审批驳回,请修改!";
                    string Module_Url = "WorkFlow_View.aspx?module=" + module;
                    //int iswaring = warning.CreateWarningToUser(OrderNumber, 4, UserNumber, CreateUserNumber);
                    Warning.AddWarning(Context, Module_Url, 2, UserNumber, CreateUserNumber);
                    string UnCheckedJobName = GetUnCheckedJobName(OrderNumber, 2);
                    if (UnCheckedJobName == null)
                    {
                        //Deletecheck(OrderNumber);
                        return 1;
                    }
                    string[] nxjbname = FMOP.SS.SplitString.GetArralist(UnCheckedJobName, ",");
                    Warning.SendSeeWarningsToUsers(OrderNumber, UserNumber, Title, Module_Url, nxjbname, 1);
                    //Deletecheck(OrderNumber);
                    return 1;
                }
                if (iswrite == 1)
                {
                    if (NextChecJobname == null)
                    {
                        int upState = UpdateState(OrderNumber, 1);
                        if (warning != null)
                            warning.CreateWarningToRole(OrderNumber, 4, UserNumber);
                        string CreateUserNumber = Orders.GetCreatUserNumber(property.ModuleName, OrderNumber);//取得创建人
                        string Context = "单号为:" + OrderNumber + "的" + Title + "审批完成!";
                        string Module_Url = "WorkFlow_View.aspx?module=" + module;
                        CustomWarning.AddWarning(Context, Module_Url, 1, UserNumber, CreateUserNumber);
                        //审批完成后执行审批后存储过程
                        SP_OnCheck.do_oncheck(module, OrderNumber);
                        return 1;
                    }
                    else
                    {
                        string[] str = NextChecJobname.Split(':');
                        string roleType = str[0].ToString();
                        string NextChecker = str[1].ToString();
                        if (roleType == "3")
                        {
                            UpdateNextChecker(OrderNumber, NextChecJobname);
                            string msg = "单号为:" + OrderNumber + "的" + Title + "请求审批!";
                            CustomWarning.CreateWarningToJobName(OrderNumber, msg, module, UserNumber, NextChecJobname, 1, GetNextCheckLimitTime(OrderNumber));
                            //短信发送 刘杰 2010-07-12
                            if (mobileE == "1")
                            {
                                SendDX sd = new SendDX();
                                sd.SendToP(NextChecker, msg);
                                
                            } 

                        }
                        else if (roleType == "1" || roleType == "2")
                        {
                            UpdateNextChecker(OrderNumber, NextChecJobname);
                            //配置信息中的审批信息
                            //if (warning != null)
                            //    warning.CreateWarningToUser(OrderNumber, 4, UserNumber, NextChecker);
                            //如果没有提醒信息节点，自定提醒内容
                            string msg = "单号为:" + OrderNumber + "的" + Title + "请求审批!";
                            //加入短信发送
                            CustomWarning.CreateWarningToJobName(OrderNumber, msg, module, UserNumber, NextChecJobname, 1, GetNextCheckLimitTime(OrderNumber),mobileE); 
                    

                        
                        }
              
                        return 1;
                    }
                }
            }
            return 0;
        }
        #endregion

        #region 审批过程
        /// <summary>
        /// 更新某张单据的审批状态(0:审批中;1:审批完成)
        /// </summary>
        /// <param name="OrderNumber">单号</param>
        /// <param name="State">是否审批完成</param>
        /// <returns>0:更新不成功１：更新成功</returns>
        private short UpdateState(string OrderNumber, int State)
        {
            if (property != null)
            {
                string ModuleName = property.ModuleName;
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ");
                    strSql.Append(ModuleName);
                    strSql.Append(" set ");
                    strSql.Append("CheckState=@CheckState ");
                    strSql.Append(" where Number=@Number ");
                    SqlParameter[] parameters = {
					new SqlParameter("@CheckState", SqlDbType.SmallInt),
                    new SqlParameter ("@Number",SqlDbType.VarChar,50)};
                    parameters[0].Value = State;
                    parameters[1].Value = OrderNumber;
                    DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 向数据库中插入审批信息
        /// </summary>
        /// <param name="OrderNumber">单号</param>
        /// <param name="UserNumber">员工编号</param>
        /// <param name="Ispass">是否通过</param>
        /// <param name="Reamrk">备注信息</param>
        /// <returns>0:插入不成功１：插入成功</returns>
        
        private short WriteCheckInfo(string OrderNumber, string UserNumber, bool Ispass, string Reamrk ,string Dailiren)
        {   //2009.06.05 王永辉添加参数 Dailiren
            if (property != null)
            {
                User us = new User();
                us = Users.GetUserByNumber(UserNumber);
                String EmployeeName = us.Name;
                String ModuleName = property.ModuleName;
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into system_CheckInfo(");
                 
                        strSql.Append("ModuleName,Number,Checker,IsPass,Remark,CheckDailiren)");
                        strSql.Append(" values (");
                        strSql.Append("@ModuleName,@Number,@Checker,@IsPass,@Remark,@CheckDailiren)");
                        SqlParameter[] parameters = {
					        new SqlParameter("@ModuleName", SqlDbType.VarChar,100),
					        new SqlParameter("@Number", SqlDbType.VarChar,100),
					        new SqlParameter("@Checker", SqlDbType.VarChar,50),
					        new SqlParameter("@IsPass", SqlDbType.Bit,1),
					        new SqlParameter("@Remark", SqlDbType.Text),
                            new SqlParameter("@CheckDailiren", SqlDbType.VarChar,50)};

                        parameters[0].Value = ModuleName;
                        parameters[1].Value = OrderNumber;
                        parameters[2].Value = EmployeeName;
                        parameters[3].Value = Ispass;
                        parameters[4].Value = Reamrk;
                        parameters[5].Value = Dailiren;

                   
                    
                    //执行插入
                    DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// 判断某张单据是否已经审批完成
        /// </summary>
        /// <param name="OrderNumber">单号</param>
        /// <returns>true:审批未通过 false：审批通过</returns>
        private bool IsFinish(string OrderNumber)
        {
            bool finish = false;
            if (property != null)
            {
                string moduleName = property.ModuleName;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select CheckState ");
                strSql.Append("from ");
                strSql.Append(moduleName);
                strSql.Append(" where Number=");
                strSql.Append("'" + OrderNumber + "'");
                SqlDataReader dr = DbHelperSQL.ExecuteReader(strSql.ToString());
                if (dr.Read())
                {
                    finish = bool.Parse(dr["CheckState"].ToString());
                }
                dr.Close();
            }
            return finish;
        }

        /// <summary>
        /// 更新某张单据下一级审批岗位的信息
        /// </summary>
        /// <param name="OrderNumber">单号</param>
        /// <param name="Ispass">是否通过</param>
        /// <param name="UserNumber">员工编号</param>
        /// <returns>0:更新不成功１：更新成功</returns>
        public short UpdateNextChecker(string OrderNumber, bool Ispass, string CheckerJobName)
        {
            if (property != null)
            {
                string ModuleName = property.ModuleName;
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ");
                    strSql.Append(ModuleName);
                    strSql.Append(" set ");
                    strSql.Append("NextChecker=@NextChecker,");
                    strSql.Append("CheckState=@CheckState ");
                    strSql.Append(" where Number=@Number ");
                    SqlParameter[] parameters = {
                    new SqlParameter("@NextChecker", SqlDbType.VarChar,50),
					new SqlParameter("@CheckState", SqlDbType.Bit),
                    new SqlParameter ("@Number",SqlDbType.VarChar,50)};
                    parameters[0].Value = CheckerJobName;
                    parameters[1].Value = Ispass;
                    parameters[2].Value = OrderNumber;
                    DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        #endregion

        public short UpdateNextChecker(string OrderNumber, string CheckerJobName)
        {
            if (property != null)
            {
                string ModuleName = property.ModuleName;
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update ");
                    strSql.Append(ModuleName);
                    strSql.Append(" set ");
                    strSql.Append("NextChecker=@NextChecker,CheckLimitTime=dbo.addMinute(getdate(),@mins)");
                    strSql.Append(" where Number=@Number ");
                    SqlParameter[] parameters = {
                    new SqlParameter("@NextChecker", SqlDbType.VarChar,50),
                    new SqlParameter ("@Number",SqlDbType.VarChar,50),
                    new SqlParameter ("@mins",SqlDbType.Int)};
                    parameters[0].Value = CheckerJobName;
                    parameters[1].Value = OrderNumber;
                    parameters[2].Value = GetNextCheckLimitTime(OrderNumber);
                    DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private string superioJobName(string EmplyloyyeNo)
        {
            string superioJobName = null;
            if (EmplyloyyeNo == null) return superioJobName;
            StringBuilder str_sql = new StringBuilder();
            str_sql.Append("SELECT JobName FROM dbo.HR_Jobs WHERE Number = ");
            str_sql.Append("(select SuperioJobNo from dbo.HR_Jobs where Number=(select JobNo from dbo.HR_Employees where Number ='" + EmplyloyyeNo + "')) ");
            SqlDataReader drr = DbHelperSQL.ExecuteReader(str_sql.ToString());
            if (drr.Read())
            {
                superioJobName = drr["JobName"].ToString();
            }
            drr.Close();
            return superioJobName;
        }

        private string sp_JobName(string EmplyloyyeNo)
        {
            string sp_JobName = null;
            if (EmplyloyyeNo == null) return sp_JobName;
            StringBuilder str_sql = new StringBuilder();
            str_sql.Append("SELECT JobName FROM dbo.HR_Jobs WHERE Number =(SELECT SPGWXZ FROM dbo.TSSPSZ where MBGWXZ =(select JobNo from dbo.HR_Employees where Number ='" + EmplyloyyeNo + "')) ");
           
            SqlDataReader drr = DbHelperSQL.ExecuteReader(str_sql.ToString());
            if (drr.Read())
            {
                sp_JobName = drr["JobName"].ToString();
            }
            drr.Close();
            return sp_JobName;
        }

        public void Deletecheck(string Number)
        {
            string sql = "delete system_CheckInfo where Number='" + Number + "'";
            DbHelperSQL.ExecuteSql(sql);
        }


        #region 获取该单据下一级审批岗位的限定审批时间
        /// <summary>
        /// 获取该单据下一级审批岗位的限定审批时间
        /// </summary>
        /// <param name="OrderNumber">单号</param>
        /// <returns>分钟数</returns>
        public int GetNextCheckLimitTime(string OrderNumber)
        {
            int LimitTime = 60;
            if (property != null && OrderNumber != null)
            {
                //取得模块名
                string ModuleName = property.ModuleName;
                StringBuilder sql = new StringBuilder();
                sql.Append("select NextChecker  from ");
                sql.Append(ModuleName);
                sql.Append(" where Number='" + OrderNumber + "'");
                SqlDataReader DrNextJobName = DbHelperSQL.ExecuteReader(sql.ToString());
                if (DrNextJobName.Read())
                {
                    //取得定单下一级要审批的信息
                    string strNextChecker = DrNextJobName["NextChecker"].ToString();
                    if (strNextChecker == null)
                    {
                        DrNextJobName.Close();
                        return LimitTime;    //如果找不到此节 返回60
                    }
                    //获得审批岗位分割字符串
                    string[] splitstring = FMOP.SS.SplitString.GetArralist(strNextChecker, ":");
                    string roleType = splitstring[0].ToString();
                    //取得要审核岗位的内容
                    string NextChecker = splitstring[1].ToString();
                    //string[] job = 
                    int rowCount = Check_Role.Tables[0].Rows.Count;
                    for (int i = 0; i < rowCount; i++)
                    {
                        string bfcheckType = Check_Role.Tables[0].Rows[i]["checkType"].ToString();
                        string jobname = Check_Role.Tables[0].Rows[i]["roleName"].ToString();
                        if (bfcheckType == "2")
                        {
                            //获得创建订单人员工编号
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = superioJobName(EMNO);
                        }
                        if (bfcheckType == "3")
                        {
                            //获得创建订单人员工编号
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = sp_JobName(EMNO);
                        }
                        if (jobname == NextChecker)
                        {
                            int nexti = i + 1;
                            //下一岗位的dataset行是否大于总行数如果大于岗位为空
                            if (nexti < rowCount)
                            {
                                //获取roleType
                                string roletype = null;
                                try
                                {
                                    roletype = Check_Role.Tables[0].Rows[nexti]["roleType"].ToString();
                                }
                                catch
                                {
                                    roletype = "1";
                                }
                                //获的checkType
                                if (Check_Role.Tables[0].Rows[nexti]["checkType"] == null)
                                    return LimitTime;
                                else
                                {
                                    try
                                    {
                                        LimitTime = int.Parse(Check_Role.Tables[0].Rows[nexti]["limitTime"].ToString());
                                    }
                                    catch
                                    {
                                        return 60;
                                    }
                                    return LimitTime;

                                }

                            }
                            else
                            {
                                return LimitTime;
                            }
                            break;
                        }
                    }

                }
                DrNextJobName.Close();
            }
            return LimitTime;
        }
        #endregion



        #region 获取对该模块进行审批的第一级岗位的限定审核时间
        /// <summary>
        /// 获取对该模块进行审批的第一级岗位
        /// </summary>
        /// <returns>多少分钟内需审核完成</returns>
        public int GetFirstCheckRoleLimitTime()
        {
            // if (Check_Role == null || Check_Role.Tables.Count <= 0)
            //     return 60;

            int time;
            try
            {
                time = int.Parse(Check_Role.Tables[0].Rows[0]["limitTime"].ToString());
            }
            catch
            {
                return 60;
            }
            return time;
            /*
            string firstJobName = "";
            if (Check_Role != null && (Check_Role.Tables.Count > 0))
            {
                string roletype = null;
                try
                {
                    roletype = Check_Role.Tables[0].Rows[0]["roleType"].ToString();
                }
                catch
                {
                    roletype = "1";
                }
                int chekType = int.Parse(Check_Role.Tables[0].Rows[0]["checkType"].ToString());
                if (chekType == 1)
                {
                    firstJobName = roletype + ":" + Check_Role.Tables[0].Rows[0]["roleName"].ToString();
                }
                else if (chekType == 2)
                {
                    //string jname = Check_Role.Tables[0].Rows[0]["roleName"].ToString();
                    //获得创建定单人员工号
                    string EMNO = user;
                    //string EMNO = Orders.GetCreatUserNumber(module, OrderNumber);
                    SqlDataReader drr = DbHelperSQL.ExecuteReader("SELECT JobName FROM dbo.HR_Jobs WHERE Number =(select SuperioJobNo from dbo.HR_Jobs where Number=(select JobNo from dbo.HR_Employees where Number ='" + EMNO + "')) ");
                    if (drr.Read())
                    {
                        firstJobName = "1:" + drr["JobName"].ToString();
                    }
                    drr.Close();
                }
            }*/
        }
        #endregion
    }

}