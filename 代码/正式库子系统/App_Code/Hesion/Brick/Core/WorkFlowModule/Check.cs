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
using ShareLiu.DXS;  //���ŷ��Ϳռ� ���� 2010-07-14
namespace Hesion.Brick.Core.WorkFlow
{



    /// <summary>
    /// ������
    /// </summary>
    public class Check
    {

        /// <summary>
        /// ���������ݼ�
        /// </summary>
        private DataSet Check_Role;

        /// <summary>
        /// ������ģ���������
        /// </summary>
        private Property property;

        /// <summary>
        /// ������(���ڴ���������Ϣ)
        /// </summary>
        private Warning warning;

        /// <summary>
        /// Check�ڵ�
        /// </summary>


        /// <summary>
        /// ͨ��������ģ���������,Xml�ڵ�,�����๹�챾��
        /// </summary>
        /// <param name="pro">������ģ���������</param>
        /// <param name="Node">Check�ڵ�</param>
        /// <param name="warning">�������ݼ�</param>
        public Check(Property pro, XmlNode Node, Warning warning)
        {
            if (pro != null && Node != null)
            {
                //���pro
                //string ModuleName = pro.ModuleName;
                property = pro;
                //�ѽڵ���䵽DATASET
                DataSet ds = new DataSet();
                string nodetext = "<Check>" + Node.InnerXml + "</Check>";
                StringReader sr = new StringReader(nodetext);
                XmlTextReader xtr = new XmlTextReader(sr);
                ds.ReadXml(xtr);
                Check_Role = ds;
                //warning��ֵ
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

        #region Check�ڵ������Ƿ���Ϊ��
        /// <summary>
        /// Check�ڵ������Ƿ���Ϊ��
        /// </summary>
        /// <returns>true:��Ϊ�� false:����Ϊ��</returns>
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

        #region �ж�ģ�����Ƿ��д�������λ
        /// <summary>
        /// �ж�ģ�����Ƿ��д�������λ
        /// </summary>
        /// <param name="UserNumber">Ա�����</param>
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

        #region ��ѯ�˵��ŵ���������״̬
        /// <summary>
        /// ��ѯ�˵��ŵ���������״̬
        /// </summary>
        /// <param name="Number">����</param>
        /// <returns></returns>
        public string GetCheckState(string Number)
        {
            string CheckState = null;
            if (property != null && Number != null)
            {
                string module = property.ModuleName;
                StringBuilder sql = new StringBuilder();
                //sql.Append("SELECT case when CheckState = 1 then '����' else '������' end as CheckState ");
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
                            CheckState = "�����У�";
                            break;
                        case "1":
                            CheckState = "������ɣ�";
                            break;
                        case "2":
                            CheckState = "�������أ�";
                            break;
                    }
                }
                drCheckState.Close();
            }
            return CheckState;
        }
        #endregion

        #region ��ȡ�Ѿ���ĳ�ŵ�������������Ա������,Ա������֮���Զ���(,)�ָ�
        /// <summary>
        /// ��ȡ�Ѿ���ĳ�ŵ�������������Ա������,Ա������֮���Զ���(,)�ָ�
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        public string GetCheckedUserName(string OrderNumber)
        {
            string getallcheck = null;
            if (property != null && OrderNumber != null)
            {
                string ModuleName = property.ModuleName.ToString();
                //��ȡcheck
                SqlDataReader drCheckerGather = DbHelperSQL.ExecuteReader("select Checker from dbo.system_CheckInfo where ((ModuleName='" + ModuleName + "') AND (Number='" + OrderNumber + "')) ");
                while (drCheckerGather.Read())
                {
                    string Checker = drCheckerGather[0].ToString();
                    //�ö��Ž��м��
                    getallcheck = getallcheck + Checker + ',';
                }
                drCheckerGather.Close();

                //ȡ���ַ����ĳ���
                if (getallcheck != null)
                {
                    int _AllCheckJobslength = getallcheck.Length - 1;
                    //�Ƴ����һλ��Ҳ����ȥ�����ģ�������
                    getallcheck = getallcheck.Remove(_AllCheckJobslength);
                }
            }
            return getallcheck;
        }
        #endregion

        #region ��ȡӦ�öԸõ�������������δ����������Ա����λ
        /// <summary>
        /// ��ȡӦ�öԸõ�������������δ����������Ա����λ
        /// </summary>
        /// <param name="OrderNumber">����</param>
        /// <param name="types">1��ʽΪ(admin)/2��ʽΪ(3:admin)</param>
        /// <returns></returns>
        public string GetUnCheckedJobName(string OrderNumber, int types)
        {
            string allJobName = null;
            if (property != null && OrderNumber != null)
            {
                //ȡ��ģ����
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
                        //��ȡ���������ϼ�
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
                                    //cktype=2Ϊֱ���ϼ�,��ȡ��ʱ��ֱ����ȡֱ���ϼ��ĸ�λ����
                                    if (cktype != "2" && cktype != "3")
                                    {
                                        jobname = Check_Role.Tables[0].Rows[j]["roleName"].ToString();
                                    }
                                    //���rtypeΪ3��ʾΪ���������,��ʾ��ʱ��Ա��������ʾ����Ա�������ʾ
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

        #region ��ȡ�õ�����һ��������λ
        /// <summary>
        /// ��ȡ�õ�����һ��������λ
        /// </summary>
        /// <param name="OrderNumber">����</param>
        /// <returns></returns>
        public string GetNextCheckRole(string OrderNumber)
        {
            string NextJobName = null;
            if (property != null && OrderNumber != null)
            {
                //ȡ��ģ����
                string ModuleName = property.ModuleName;
                StringBuilder sql = new StringBuilder();
                sql.Append("select NextChecker  from ");
                sql.Append(ModuleName);
                sql.Append(" where Number='" + OrderNumber + "'");
                SqlDataReader DrNextJobName = DbHelperSQL.ExecuteReader(sql.ToString());
                if (DrNextJobName.Read())
                {
                    //ȡ�ö�����һ��Ҫ��������Ϣ
                    string strNextChecker = DrNextJobName["NextChecker"].ToString();
                    if (strNextChecker == null)
                    {
                        DrNextJobName.Close();
                        return NextJobName;
                    }
                    //���������λ�ָ��ַ���
                    string[] splitstring = FMOP.SS.SplitString.GetArralist(strNextChecker, ":");
                    string roleType = splitstring[0].ToString();
                    //ȡ��Ҫ��˸�λ������
                    string NextChecker = splitstring[1].ToString();
                    //string[] job = 
                    int rowCount = Check_Role.Tables[0].Rows.Count;
                    for (int i = 0; i < rowCount; i++)
                    {
                        string bfcheckType = Check_Role.Tables[0].Rows[i]["checkType"].ToString();
                        string jobname = Check_Role.Tables[0].Rows[i]["roleName"].ToString();
                        if (bfcheckType == "2")
                        {
                            //��ô���������Ա�����
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = superioJobName(EMNO);
                        }
                        if (bfcheckType == "3")
                        {
                            //��ô���������Ա�����
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = sp_JobName(EMNO);
                        }
                        if (jobname == NextChecker)
                        {
                            int nexti = i + 1;
                            //��һ��λ��dataset���Ƿ����������������ڸ�λΪ��
                            if (nexti < rowCount)
                            {
                                //��ȡroleType
                                string roletype = null;
                                try
                                {
                                    roletype = Check_Role.Tables[0].Rows[nexti]["roleType"].ToString();
                                }
                                catch
                                {
                                    roletype = "1";
                                }
                                //���checkType
                                int chekType = int.Parse(Check_Role.Tables[0].Rows[nexti]["checkType"].ToString());
                                if (chekType == 1)
                                {
                                    //if (roletype == "3" && AuthTransfer.getTransferUser(Check_Role.Tables[0].Rows[0]["roleName"].ToString()) != "")
                                    //    NextJobName = roletype + ":" + AuthTransfer.getTransferUser(Check_Role.Tables[0].Rows[0]["roleName"].ToString());
                                    //else
                                    NextJobName = roletype + ":" + Check_Role.Tables[0].Rows[nexti]["roleName"].ToString();
                                }
                                else if (chekType == 2)//Ϊֱ���ϼ���������λ
                                {
                                    NextJobName = "1:" + superioJobName(OrderNumber);
                                }
                                //��ȡ���������ϼ���λ
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
        #region ��ȡ�õ�����һ��������λ �㷨2 ���� 2010-07-13 �������ö����ж�
        /// <summary>
        /// ��ȡ�õ�����һ��������λ
        /// </summary>
        /// <param name="OrderNumber">����</param>
        /// <returns></returns>
        public string[] GetNextCheckRole2(string OrderNumber)
        {
            string[] aa = new string[2];
            string NextJobName = null;
            string mobileE="0";
            if (property != null && OrderNumber != null)
            {
                
                //ȡ��ģ����

                string ModuleName = property.ModuleName;
                StringBuilder sql = new StringBuilder();
                sql.Append("select NextChecker  from ");
                sql.Append(ModuleName);
                sql.Append(" where Number='" + OrderNumber + "'");
                SqlDataReader DrNextJobName = DbHelperSQL.ExecuteReader(sql.ToString());
                if (DrNextJobName.Read())
                {
                    //ȡ�ö�����һ��Ҫ��������Ϣ
                    string strNextChecker = DrNextJobName["NextChecker"].ToString();
                    if (strNextChecker == null)
                    {
                        DrNextJobName.Close();
                   
                        aa[0] = NextJobName;
                        aa[1] = "0";
                        return aa;
                    }
                    //���������λ�ָ��ַ���
                    string[] splitstring = FMOP.SS.SplitString.GetArralist(strNextChecker, ":");
                    string roleType = splitstring[0].ToString();
                    //ȡ��Ҫ��˸�λ������
                    string NextChecker = splitstring[1].ToString();
                    //string[] job = 
                    int rowCount = Check_Role.Tables[0].Rows.Count;
                    for (int i = 0; i < rowCount; i++)
                    {
                        string bfcheckType = Check_Role.Tables[0].Rows[i]["checkType"].ToString();
                        string jobname = Check_Role.Tables[0].Rows[i]["roleName"].ToString();
                        if (bfcheckType == "2")
                        {
                            //��ô���������Ա�����
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = superioJobName(EMNO);
                        }
                        if (bfcheckType == "3")
                        {
                            //��ô���������Ա�����
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = sp_JobName(EMNO);
                        }
                        if (jobname == NextChecker)
                        {
                            int nexti = i + 1;
                            //��һ��λ��dataset���Ƿ����������������ڸ�λΪ��
                            if (nexti < rowCount)
                            {
                                //��ȡroleType
                                string roletype = null;
                                try
                                {
                                    roletype = Check_Role.Tables[0].Rows[nexti]["roleType"].ToString();
                                }
                                catch
                                {
                                    roletype = "1";
                                }
                                //���checkType
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
                                else if (chekType == 2)//Ϊֱ���ϼ���������λ
                                {
                                    NextJobName = "1:" + superioJobName(OrderNumber);

                                }
                                //��ȡ���������ϼ���λ
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

        #region ��ȡ�Ը�ģ����������ĵ�һ����λ
        /// <summary>
        /// ��ȡ�Ը�ģ����������ĵ�һ����λ
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
                    //��ô���������Ա����
                    string EMNO = user;
                    //string EMNO = Orders.GetCreatUserNumber(module, OrderNumber);
                    SqlDataReader drr = DbHelperSQL.ExecuteReader("SELECT JobName FROM dbo.HR_Jobs WHERE Number =(select SuperioJobNo from dbo.HR_Jobs where Number=(select JobNo from dbo.HR_Employees where Number ='" + EMNO + "')) ");
                    if (drr.Read())
                    {
                        firstJobName = "1:" + drr["JobName"].ToString();
                    }
                    drr.Close();
                }
                //��ȡ���������ϼ���λ
                else if (chekType == 3)
                {
                    //string jname = Check_Role.Tables[0].Rows[0]["roleName"].ToString();
                    //��ô���������Ա����
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
        #region ��ȡ�Ը�ģ����������ĵ�һ����λ�㷨2 ���� 2010-07-12 ����
        /// <summary>
        /// ��ȡ�Ը�ģ����������ĵ�һ����λ
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
                    //��ô���������Ա����
                    string EMNO = user;
                    //string EMNO = Orders.GetCreatUserNumber(module, OrderNumber);
                    SqlDataReader drr = DbHelperSQL.ExecuteReader("SELECT JobName FROM dbo.HR_Jobs WHERE Number =(select SuperioJobNo from dbo.HR_Jobs where Number=(select JobNo from dbo.HR_Employees where Number ='" + EMNO + "')) ");
                    if (drr.Read())
                    {
                        firstJobName = "1:" + drr["JobName"].ToString() + ":" + mobileE;
                    }
                    drr.Close();
                }
                //��ȡ���������ϼ���λ
                else if (chekType == 3)
                {
                    //string jname = Check_Role.Tables[0].Rows[0]["roleName"].ToString();
                    //��ô���������Ա����
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

        #region ��ĳ�ŵ��ݽ�������
        /// <summary>
        /// ��ĳ�ŵ��ݽ�������
        /// </summary>
        /// <param name="OrderNumber">����</param>
        /// <param name="UserNumber">Ա�����</param>
        /// <param name="Ispass">�Ƿ�ͨ��</param>
        /// <param name="Reamrk">��ע</param>
        /// <returns>0:�������ɹ����������ɹ�</returns>
        public short DoCheck(string OrderNumber, string UserNumber, bool Ispass, string Reamrk ,string Dailiren)
        {
            if (OrderNumber != null && UserNumber != null && Reamrk != null)
            {
                //д��������Ϣ
                int iswrite = WriteCheckInfo(OrderNumber, UserNumber, Ispass, Reamrk, Dailiren);
                //ȡ����һ����˸�λ
                string NextChecJobname = GetNextCheckRole(OrderNumber);
                string module = property.ModuleName;
                string Title = property.Title;
                //���������ͨ�����͸�������������
                if (Ispass == false)
                {
                    int upState = UpdateState(OrderNumber, 2);
                    UpdateNextChecker(OrderNumber, NextChecJobname);
                    string CreateUserNumber = Orders.GetCreatUserNumber(property.ModuleName, OrderNumber);
                    //int iswaring = warning.CreateWarningToUser(OrderNumber, 4, UserNumber, CreateUserNumber);
                    string Context = "����Ϊ:" + OrderNumber + "��" + Title + "��������,���޸�!";
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
                        string CreateUserNumber = Orders.GetCreatUserNumber(property.ModuleName, OrderNumber);//ȡ�ô�����
                        string Context = "����Ϊ:" + OrderNumber + "��" + Title + "�������!";
                        string Module_Url = "WorkFlow_View.aspx?module=" + module;
                        CustomWarning.AddWarning(Context, Module_Url, 1, UserNumber, CreateUserNumber);
                        //������ɺ�ִ��������洢����
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
                            string msg = "����Ϊ:" + OrderNumber + "��" + Title + "��������!";
                            CustomWarning.CreateWarningToJobName(OrderNumber, msg, module, UserNumber, NextChecJobname, 1, GetNextCheckLimitTime(OrderNumber));
                        }
                        else if (roleType == "1" || roleType == "2")
                        {
                            UpdateNextChecker(OrderNumber, NextChecJobname);
                            //������Ϣ�е�������Ϣ
                            //if (warning != null)
                            //    warning.CreateWarningToUser(OrderNumber, 4, UserNumber, NextChecker);
                            //���û��������Ϣ�ڵ㣬�Զ���������
                            string msg = "����Ϊ:" + OrderNumber + "��" + Title + "��������!";
                            CustomWarning.CreateWarningToJobName(OrderNumber, msg, module, UserNumber, NextChecJobname, 1, GetNextCheckLimitTime(OrderNumber));
                        }
                        return 1;
                    }
                }
            }
            return 0;
        }
        #endregion
        #region ��ĳ�ŵ��ݽ������� �㷨2���������Ź��� ���� 2010-07-13
        /// <summary>
        /// ��ĳ�ŵ��ݽ�������
        /// </summary>
        /// <param name="OrderNumber">����</param>
        /// <param name="UserNumber">Ա�����</param>
        /// <param name="Ispass">�Ƿ�ͨ��</param>
        /// <param name="Reamrk">��ע</param>
        /// <returns>0:�������ɹ����������ɹ�</returns>
        public short DoCheck2(string OrderNumber, string UserNumber, bool Ispass, string Reamrk, string Dailiren)
        {
            string[] nextDX;
            if (OrderNumber != null && UserNumber != null && Reamrk != null)
            {
                //д��������Ϣ
                int iswrite = WriteCheckInfo(OrderNumber, UserNumber, Ispass, Reamrk, Dailiren);
                //ȡ����һ����˸�λ
              //  string NextChecJobname = GetNextCheckRole(OrderNumber);
                nextDX = GetNextCheckRole2(OrderNumber);
                string NextChecJobname = nextDX[0];
                string mobileE = nextDX[1].ToString();
                string module = property.ModuleName;
                string Title = property.Title;
                //���������ͨ�����͸�������������
                if (Ispass == false)
                {
                    int upState = UpdateState(OrderNumber, 2);
                    UpdateNextChecker(OrderNumber, NextChecJobname);
                    string CreateUserNumber = Orders.GetCreatUserNumber(property.ModuleName, OrderNumber);
                    //int iswaring = warning.CreateWarningToUser(OrderNumber, 4, UserNumber, CreateUserNumber);
                    string Context = "����Ϊ:" + OrderNumber + "��" + Title + "��������,���޸�!";
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
                        string CreateUserNumber = Orders.GetCreatUserNumber(property.ModuleName, OrderNumber);//ȡ�ô�����
                        string Context = "����Ϊ:" + OrderNumber + "��" + Title + "�������!";
                        string Module_Url = "WorkFlow_View.aspx?module=" + module;
                        CustomWarning.AddWarning(Context, Module_Url, 1, UserNumber, CreateUserNumber);
                        //������ɺ�ִ��������洢����
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
                            string msg = "����Ϊ:" + OrderNumber + "��" + Title + "��������!";
                            CustomWarning.CreateWarningToJobName(OrderNumber, msg, module, UserNumber, NextChecJobname, 1, GetNextCheckLimitTime(OrderNumber));
                            //���ŷ��� ���� 2010-07-12
                            if (mobileE == "1")
                            {
                                SendDX sd = new SendDX();
                                sd.SendToP(NextChecker, msg);
                                
                            } 

                        }
                        else if (roleType == "1" || roleType == "2")
                        {
                            UpdateNextChecker(OrderNumber, NextChecJobname);
                            //������Ϣ�е�������Ϣ
                            //if (warning != null)
                            //    warning.CreateWarningToUser(OrderNumber, 4, UserNumber, NextChecker);
                            //���û��������Ϣ�ڵ㣬�Զ���������
                            string msg = "����Ϊ:" + OrderNumber + "��" + Title + "��������!";
                            //������ŷ���
                            CustomWarning.CreateWarningToJobName(OrderNumber, msg, module, UserNumber, NextChecJobname, 1, GetNextCheckLimitTime(OrderNumber),mobileE); 
                    

                        
                        }
              
                        return 1;
                    }
                }
            }
            return 0;
        }
        #endregion

        #region ��������
        /// <summary>
        /// ����ĳ�ŵ��ݵ�����״̬(0:������;1:�������)
        /// </summary>
        /// <param name="OrderNumber">����</param>
        /// <param name="State">�Ƿ��������</param>
        /// <returns>0:���²��ɹ��������³ɹ�</returns>
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
        /// �����ݿ��в���������Ϣ
        /// </summary>
        /// <param name="OrderNumber">����</param>
        /// <param name="UserNumber">Ա�����</param>
        /// <param name="Ispass">�Ƿ�ͨ��</param>
        /// <param name="Reamrk">��ע��Ϣ</param>
        /// <returns>0:���벻�ɹ���������ɹ�</returns>
        
        private short WriteCheckInfo(string OrderNumber, string UserNumber, bool Ispass, string Reamrk ,string Dailiren)
        {   //2009.06.05 ��������Ӳ��� Dailiren
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

                   
                    
                    //ִ�в���
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
        /// �ж�ĳ�ŵ����Ƿ��Ѿ��������
        /// </summary>
        /// <param name="OrderNumber">����</param>
        /// <returns>true:����δͨ�� false������ͨ��</returns>
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
        /// ����ĳ�ŵ�����һ��������λ����Ϣ
        /// </summary>
        /// <param name="OrderNumber">����</param>
        /// <param name="Ispass">�Ƿ�ͨ��</param>
        /// <param name="UserNumber">Ա�����</param>
        /// <returns>0:���²��ɹ��������³ɹ�</returns>
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


        #region ��ȡ�õ�����һ��������λ���޶�����ʱ��
        /// <summary>
        /// ��ȡ�õ�����һ��������λ���޶�����ʱ��
        /// </summary>
        /// <param name="OrderNumber">����</param>
        /// <returns>������</returns>
        public int GetNextCheckLimitTime(string OrderNumber)
        {
            int LimitTime = 60;
            if (property != null && OrderNumber != null)
            {
                //ȡ��ģ����
                string ModuleName = property.ModuleName;
                StringBuilder sql = new StringBuilder();
                sql.Append("select NextChecker  from ");
                sql.Append(ModuleName);
                sql.Append(" where Number='" + OrderNumber + "'");
                SqlDataReader DrNextJobName = DbHelperSQL.ExecuteReader(sql.ToString());
                if (DrNextJobName.Read())
                {
                    //ȡ�ö�����һ��Ҫ��������Ϣ
                    string strNextChecker = DrNextJobName["NextChecker"].ToString();
                    if (strNextChecker == null)
                    {
                        DrNextJobName.Close();
                        return LimitTime;    //����Ҳ����˽� ����60
                    }
                    //���������λ�ָ��ַ���
                    string[] splitstring = FMOP.SS.SplitString.GetArralist(strNextChecker, ":");
                    string roleType = splitstring[0].ToString();
                    //ȡ��Ҫ��˸�λ������
                    string NextChecker = splitstring[1].ToString();
                    //string[] job = 
                    int rowCount = Check_Role.Tables[0].Rows.Count;
                    for (int i = 0; i < rowCount; i++)
                    {
                        string bfcheckType = Check_Role.Tables[0].Rows[i]["checkType"].ToString();
                        string jobname = Check_Role.Tables[0].Rows[i]["roleName"].ToString();
                        if (bfcheckType == "2")
                        {
                            //��ô���������Ա�����
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = superioJobName(EMNO);
                        }
                        if (bfcheckType == "3")
                        {
                            //��ô���������Ա�����
                            string EMNO = Orders.GetCreatUserNumber(ModuleName, OrderNumber);
                            jobname = sp_JobName(EMNO);
                        }
                        if (jobname == NextChecker)
                        {
                            int nexti = i + 1;
                            //��һ��λ��dataset���Ƿ����������������ڸ�λΪ��
                            if (nexti < rowCount)
                            {
                                //��ȡroleType
                                string roletype = null;
                                try
                                {
                                    roletype = Check_Role.Tables[0].Rows[nexti]["roleType"].ToString();
                                }
                                catch
                                {
                                    roletype = "1";
                                }
                                //���checkType
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



        #region ��ȡ�Ը�ģ����������ĵ�һ����λ���޶����ʱ��
        /// <summary>
        /// ��ȡ�Ը�ģ����������ĵ�һ����λ
        /// </summary>
        /// <returns>���ٷ�������������</returns>
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
                    //��ô���������Ա����
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