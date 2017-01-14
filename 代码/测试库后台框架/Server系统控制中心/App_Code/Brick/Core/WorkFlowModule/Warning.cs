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
using System.Collections;
using System.Text;
using System.Xml;
//using FMOP.DB;
using System.IO;
//using ShareLiu.DXS;  //���ŷ��Ϳռ� ����2010-07


namespace Hesion.Brick.Core.WorkFlow
{



    /// <summary>
    /// ������
    /// </summary>
    public class Warning
    {

        /// <summary>
        /// �������ݼ�
        /// </summary>
        private DataSet RoleWarning;

        /// <summary>
        /// ������ģ���������
        /// </summary>
        private Property property;

        private XmlNode WarningNode;
        #region
        /// <summary>
        /// ͨ��������ģ��������Ժ�Xml�ڵ㹹�챾��
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="Node"></param>
        public Warning(Property pro, XmlNode Node)
        {
            if (pro != null)
            {
                property = pro;
                DataSet ds = new DataSet();
                if (Node != null)
                {
                    string nodetext = "<Warning>" + Node.InnerXml + "</Warning>";

                    StringReader sr = new StringReader(nodetext);
                    XmlTextReader xtr = new XmlTextReader(sr);
                    ds.ReadXml(xtr);
                    RoleWarning = ds;
                    WarningNode = Node;
                }
            }
        }
        #endregion
        #region//2014.05.22--��������--�ò�������ʱע�͵�������õ����ͷŲ��޸����ݿ����

        //#region
        ///// <summary>
        ///// ������״̬�ı�ʱ������ظ�λ����ģ�������ļ��ж�Ӧ���͵�������Ϣ
        ///// </summary>
        ///// <param name="Number">����</param>
        ///// <param name="Type">�¼����� 1:���ݲ����� 2:�����޸ĺ� 3:����ɾ���� 4:�������ͨ����</param>
        ///// <param name="UserNumber">��Ϣ�����ߵ�Ա�����</param>
        ///// <returns>1:�ɹ� 0:ʧ��</returns>
        //public short CreateWarningToRole(string Number, int Type, string UserNumber)
        //{
            
        //    //���ǹ�����ģ�飬��������ø÷���
        //    if (property == null || RoleWarning == null || RoleWarning.Tables.Count <= 0 )
        //        return 0;
        //    int ModuleType = property.ModuleType;
        //    string Module_Url = null;
        //    string name = property.ModuleName;
        //    string page = "WorkFlow_View.aspx?module=";
        //    string filenName = "&number=";
        //    //ƴ�����ӵ�ַ�ַ���
        //    Module_Url = page + name + filenName + Number;
        //    return CreateWarningToRole(Number, Type, UserNumber, Module_Url);
        //}
        ///// <summary>
        ///// ������״̬�ı�ʱ������ظ�λ����ģ�������ļ��ж�Ӧ���͵�������Ϣ����Ҫ�������Զ���ģ�顣
        ///// </summary>
        ///// <param name="Number">����</param>
        ///// <param name="Type">�¼����� 1:���ݲ����� 2:�����޸ĺ� 3:����ɾ���� 4:�������ͨ����</param>
        ///// <param name="UserNumber">�����˵Ĺ��� ��Ҫָ�������ӵ��û�</param>
        ///// <param name="ViewUrl">�鿴�õ��ݵĵ�ַ</param>
        ///// <returns>1:�ɹ� 0:ʧ��</returns>
        //public short CreateWarningToRole(string Number, int Type, string UserNumber, string ViewUrl)
        //{
        //    //���ģ�鲻���ڣ���ֱ�ӷ���0
        //    if (property == null || RoleWarning == null || RoleWarning.Tables.Count <= 0)
        //        return 0;
        //    int ModuleType = property.ModuleType;
        //    string name = property.ModuleName;
        //    string title = property.Title;
        //    DataRow[] row = RoleWarning.Tables[0].Select("warningType='" + Type + "'");
        //    if (row.Length == 0)
        //        return 0;
        //    try
        //    {
        //        SendWarningsToUsers(Number, UserNumber, title, ViewUrl, row);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error(e.Message);
        //        return 0;
        //    }
        //    return 1;

        //}

        ///// <summary>
        ///// ����һ������
        ///// </summary>
        ///// <param name="Number">����</param>
        ///// <param name="UserNumber">��Ϣ�����߱��</param>
        ///// <param name="title">ģ������</param>
        ///// <param name="Module_Url">�鿴��ַ</param>
        ///// <param name="row">������Ϣ���͵ĸ�λ�б�</param>

        //private  static void SendWarningsToUsers(string Number, string UserNumber, string title, string Module_Url, DataRow[] row)
        //{
        //    foreach (DataRow dc in row)
        //    {
        //        //ȡ�ø�λҪ���ѵĸ�λ����
        //        string jobname = dc["roleName"].ToString();
        //        //ȡ��roleType 
        //        string roleType = dc["roleType"].ToString();
        //        //ȡ��������Ϣ������
        //        int Grade = int.Parse(dc["grade"].ToString());
        //        //��ȡwarningContext
        //        string Context = title + ":" + dc["warningContext"].ToString() + "������Ϊ��" + Number;
        //        //����һ������Ǹ�λ���ƣ��������Ƶı���
        //        string condition = null;
        //        //���ŷ��ͽڵ����  2010-07-12 ����
        //        string mobileE = "";
        //        try
        //        {
        //             mobileE = dc["sMobileEnable"].ToString();
        //        }
        //        catch (Exception e1)
        //        {
        //            mobileE = "��";

        //        }

        //        //����roleType�ж��Ǹ�λ���ƻ��ǲ�������
        //        if (roleType == "1")
        //        {
        //            condition = "HR_Jobs.JobName";
        //        }
        //        else if (roleType == "2")
        //        {
        //            condition = "HR_Employees.BM";
        //        }
        //        else if (roleType == "3")
        //        {
        //            condition = "HR_Employees.Number";
        //        }
        //        //��ȡ���ϸø�λ��Ա�����
        //        StringBuilder str_sql = new StringBuilder();
        //        str_sql.Append("SELECT HR_Employees.Number ");
        //        str_sql.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
        //        str_sql.Append("WHERE  ");
        //        str_sql.Append(condition);
        //        str_sql.Append("='" + jobname + "'");
        //        SqlDataReader dr_employees = DbHelperSQL.ExecuteReader(str_sql.ToString());
        //        while (dr_employees.Read())
        //        {
        //            string ToUser = dr_employees[0].ToString();
        //            //����������Ϣ
        //            AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
        //            //����������� 2010-07-12 ����
        //            if (mobileE == "1")
        //            {
                      
        //                SendDX sd = new SendDX();
        //                sd.SendToP(ToUser, Context);
        //            }
        //        }
        //        dr_employees.Close();
  





        //        //��ȡ���ϸø�λ��Ա�����
        //        StringBuilder str_sql2 = new StringBuilder();
        //        str_sql2.Append("SELECT HR_Employees.Number,(select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) AS SQWT  ");
        //        str_sql2.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
        //        str_sql2.Append("WHERE (select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) is not null  AND  ");
        //        str_sql2.Append(condition);
        //        str_sql2.Append("='" + jobname + "'");
        //        SqlDataReader dr_employees2 = DbHelperSQL.ExecuteReader(str_sql2.ToString());
        //        while (dr_employees2.Read())
        //        {
        //            string ToUser = dr_employees2[1].ToString();
        //            //����������Ϣ
        //            AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
        //        }
        //        dr_employees2.Close();
             

        //    }
        //}
        //#endregion

        //#region
        //  /// <summary>
        ///// ����һ������
        ///// </summary>
        ///// <param name="Number">����</param>
        ///// <param name="UserNumber">��Ϣ�����߱��</param>
        ///// <param name="title">ģ������</param>
        ///// <param name="Module_Url">�鿴��ַ</param>
        ///// <param name="row">������Ϣ���͵ĸ�λ�б�</param>

        //public static void SendSeeWarningsToUsers(string Number, string UserNumber, string title, string Module_Url, string[] JobName, int grade)
        //{
        //    if (JobName == null) return;
        //    foreach (string jbs in JobName)
        //    {
        //        //ȡ�÷ָ��ַ����ĸ�λ
        //        string[] jb = FMOP.SS.SplitString.GetArralist(jbs, ":");
        //        //ȡ�ø�λҪ���ѵĸ�λ����
        //        string jobname =jb[1].ToString ();
        //        //ȡ��roleType 
        //        string roleType = jb[0].ToString();
        //        //ȡ��������Ϣ������
        //        int Grade = grade;
        //        //��ȡwarningContext
        //        string Context = title + ":" + "�����󲵻أ�" + "����Ϊ��" + Number;
        //        //����һ������Ǹ�λ���ƣ��������Ƶı���
        //        string condition = null;
        //        //����roleType�ж��Ǹ�λ���ƻ��ǲ�������
        //        if (roleType == "1")
        //        {
        //            condition = "HR_Jobs.JobName";
        //        }
        //        else if (roleType == "2")
        //        {
        //            condition = "HR_Employees.BM";
        //        }
        //        else if (roleType == "3")
        //        {
        //            condition = "HR_Employees.Number";
        //        }
        //        //��ȡ���ϸø�λ��Ա�����
        //        StringBuilder str_sql = new StringBuilder();
        //        str_sql.Append("SELECT HR_Employees.Number ");
        //        str_sql.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
        //        str_sql.Append("WHERE  ");
        //        str_sql.Append(condition);
        //        str_sql.Append("='" + jobname + "'");
        //        SqlDataReader dr_employees = DbHelperSQL.ExecuteReader(str_sql.ToString());
        //        while (dr_employees.Read())
        //        {
        //            string ToUser = dr_employees[0].ToString();
        //            //����������Ϣ
        //            AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
        //        }
        //        dr_employees.Close();
        //    }
        //}
        //#endregion


        //#region
        ///// <summary>
        ///// �Ը�λ����������Ϣ
        ///// </summary>
        ///// <param name="Number">����</param>
        ///// <param name="Context">����</param>
        ///// <param name="Module">ģ����</param>
        ///// <param name="UserNumber">Ա�����</param>
        ///// <returns></returns>
        //public short CreateWarningToJobName(string Number, string Context, string Module, string UserNumber, string JobName, int Grade)
        //{
        //    if (Number != "" && Context != "" && Module != "" && UserNumber != "" && JobName != "")
        //    {
        //        string page = "WorkFlow_Check.aspx?module=";
        //        string filenName = "&number=";
        //        string Module_Url = page + Module + filenName + Number;
        //        string jobname = JobName;
        //        string[] str = jobname.Split(':');
        //        string roleType = str[0].ToString();
        //        string NextChecker = str[1].ToString();
        //        //����һ������Ǹ�λ���ƣ��������Ƶı���
        //        string condition = null;
        //        //����roleType�ж��Ǹ�λ���ƻ��ǲ�������
        //        if (roleType == "1")
        //        {
        //            condition = "HR_Jobs.JobName";
        //        }
        //        else if (roleType == "2")
        //        {
        //            condition = "HR_Employees.BM";
        //        }
        //        else if (roleType == "3")
        //        {
        //            condition = "HR_Employees.Number";
        //        }
        //        StringBuilder str_sql = new StringBuilder();
        //        str_sql.Append("SELECT HR_Employees.Number ");
        //        str_sql.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
        //        str_sql.Append("WHERE  ");
        //        str_sql.Append(condition);
        //        str_sql.Append("='" + NextChecker + "'");
        //        SqlDataReader dr_employees = DbHelperSQL.ExecuteReader(str_sql.ToString());
        //        while (dr_employees.Read())
        //        {
        //            string ToUser = dr_employees[0].ToString();
        //            //����������Ϣ
        //            AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
        //            Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new WorkFlowModule(Module);
        //            int su=wf.check.UpdateNextChecker(Number, jobname);
        //        }
        //        dr_employees.Close();
        //        //����ί������
        //        StringBuilder str_sql2 = new StringBuilder();
        //        str_sql2.Append("SELECT HR_Employees.Number,(select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) AS SQWT  ");
        //        str_sql2.Append(" FROM  HR_Jobs INNER JOIN  HR_Employees ON HR_Jobs.Number = HR_Employees.JobNo ");
        //        str_sql2.Append("WHERE (select BSQRZH FROM dbo.AuthTransfer WHERE SQRZH =HR_Employees.Number AND STATE=1) is not null  AND  ");
        //        str_sql2.Append(condition);
        //        str_sql2.Append("='" + NextChecker + "'");
        //        SqlDataReader dr_employees2 = DbHelperSQL.ExecuteReader(str_sql2.ToString());
        //        while (dr_employees2.Read())
        //        {
        //            string ToUser = dr_employees2[0].ToString();
        //            //����������Ϣ
        //            AddWarning(Context, Module_Url, Grade, UserNumber, ToUser);
        //            Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new WorkFlowModule(Module);
        //            int su = wf.check.UpdateNextChecker(Number, jobname);
        //        }
        //        dr_employees2.Close();



        //        return 1;
        //    }
        //    return 0;
        //}

        //#endregion

        ///// <summary>
        ///// �Զ�������������
        ///// </summary>
        ///// <param name="Number">����</param>
        ///// <param name="Type">�������</param>
        ///// <param name="UserNumber">��ǰԱ�����</param>
        ///// <param name="ToUserNumber">����������Ա�����</param>
        ///// <returns></returns>
        //public  short CreateWarningToUser(string Number, int Type, string UserNumber, string ToUserNumber)
        //{
        //    if (property != null && RoleWarning != null && RoleWarning.Tables.Count > 0)
        //    {
        //        //ȡ��ģ����
        //        string name = property.ModuleName;
        //        string title = property.Title;
        //        //ȡ��ģ������
        //        int ModuleType = property.ModuleType;
        //        if (ModuleType == 1 || ModuleType == 4)
        //        {
        //            string Module_Url = null;
        //            if (ModuleType == 1)
        //            {
        //                string page = "WorkFlow_Check.aspx?module=";
        //                string filenName = "&number=";
        //                //ƴ�����ӵ�ַ�ַ���
        //                Module_Url = page + name + filenName + Number;
        //                string msg = "����Ϊ:" + Number + "��" + title + "��������!";
        //                AddWarning(msg, Module_Url, 1, UserNumber, ToUserNumber);
        //            }
        //            //else if (ModuleType == 4)
        //            //{
        //            //    //string UrlXml = dr["Url"].ToString();
        //            //    //XmlNode UrlNode = UrlDcmt.SelectSingleNode("//AddOnModule");
        //            //    //Module_Url = UrlNode["Url"].InnerXml;
        //            //}
        //            DataRow[] row = RoleWarning.Tables[0].Select("warningType='" + Type + "'");
        //            if (row.Length == 1)
        //            {
        //                foreach (DataRow dc in row)
        //                {
        //                    //ȡ�ø�λҪ���ѵĸ�λ����
        //                    string jobname = dc["roleName"].ToString();
        //                    //ȡ��������Ϣ������
        //                    int Grade = int.Parse(dc["grade"].ToString());
        //                    //��ȡwarningContext
        //                    string Context = title + ":" + dc["warningContext"].ToString() + "������Ϊ��" + Number;
        //                    //����������Ϣ
        //                    AddWarning(Context, Module_Url, Grade, UserNumber, ToUserNumber);
        //                    return 1;
        //                }
        //            }
        //        }
        //    }
        //    return 0;
        //}

        ///// <summary>
        ///// ��������Ϣд�����ݿ�
        ///// </summary>
        ///// <param name="Context">��������</param>
        ///// <param name="Module_Url">�鿴��ַ</param>
        ///// <param name="Grade">�������1������2������</param>
        ///// <param name="FromUser">��Ϣ�����˵Ĺ���</param>
        ///// <param name="ToUser">��Ϣ�����˵Ĺ���</param>
        //public static void AddWarning(string Context, string Module_Url, int Grade, string FromUser, string ToUser)
        //{
        //    try
        //    {
        //        StringBuilder strSql = new StringBuilder();
        //        strSql.Append("insert into User_Warnings(");
        //        strSql.Append("Context,Module_Url,Grade,FromUser,ToUser)");
        //        strSql.Append(" values (");
        //        strSql.Append("@Context,@Module_Url,@Grade,@FromUser,@ToUser)");
        //        SqlParameter[] parameters = {
        //            new SqlParameter("@Context", SqlDbType.VarChar,4000),
        //            new SqlParameter("@Module_Url", SqlDbType.VarChar,500),
        //            new SqlParameter("@Grade", SqlDbType.SmallInt,2),
        //            new SqlParameter("@FromUser", SqlDbType.VarChar,50),
        //            new SqlParameter("@ToUser", SqlDbType.VarChar,50)};
        //        parameters[0].Value = Context;
        //        parameters[1].Value = Module_Url;
        //        parameters[2].Value = Grade;
        //        parameters[3].Value = FromUser;
        //        parameters[4].Value = ToUser;
        //        DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

        //    }
        //    catch (Exception EX)
        //    {
        //        throw (EX);
        //    }

        //}

        ///// <summary>
        ///// ��ȡ������Ϣ�ļ�¼��
        ///// </summary>
        ///// <param name="EmployeeNo"></param>
        ///// <returns></returns>
        //public static int GetWarningCount(string EmployeeNo)
        //{
        //    if (EmployeeNo != null)
        //    {
        //        StringBuilder sql = new StringBuilder();
        //        #region ԭʼ�汾  
        //        //sql.Append("SELECT count(HR_Employees.Employee_Name) ");
        //        //sql.Append("FROM  User_Warnings INNER JOIN HR_Employees ON User_Warnings.ToUser = HR_Employees.Number ");
        //        //sql.Append("where isRead=0 and Grade=2 and touser ='" + EmployeeNo + "' ");
        //        #endregion 
              
        //        #region  ������ 2009.06.15
        //        sql.Append("select count(*) from ( SELECT User_Warnings.* ");
        //        sql.Append("FROM  User_Warnings INNER JOIN HR_Employees ON User_Warnings.ToUser = HR_Employees.Number ");
        //        sql.Append("where touser ='" + EmployeeNo + "' ");
        //        sql.Append(SqlServiceMemcountdtr(true,EmployeeNo));
        //        sql.Append(") as c where isRead=0 and Grade=2");

        //        #endregion
        //        object ob = DbHelperSQL.GetSingle(sql.ToString());
        //        if (ob != null)
        //        {
        //            return int.Parse(ob.ToString());
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    return 0;
        //}
        ///// <summary>
        ///// ��ȡ������Ϣ�ļ�¼��
        ///// </summary>
        ///// <param name="EmployeeNo"></param>
        ///// <returns></returns>
        //public static int GetRemindCount(string EmployeeNo)
        //{
        //    if (EmployeeNo != null)
        //    {
        //        StringBuilder sql = new StringBuilder();

        //        #region ԭʼ�汾
        //        //sql.Append("SELECT count(HR_Employees.Employee_Name) ");
        //        //sql.Append("FROM  User_Warnings INNER JOIN HR_Employees ON User_Warnings.ToUser = HR_Employees.Number ");
        //        //sql.Append("where isRead=0 and Grade=1 and touser ='" + EmployeeNo + "' ");
        //        #endregion

        //        #region ������ 2009.06.15 �޸�
        //        sql.Append("select count(*) from ( SELECT User_Warnings.* ");
        //        sql.Append("FROM  User_Warnings INNER JOIN HR_Employees ON User_Warnings.ToUser = HR_Employees.Number ");
        //        sql.Append("where touser ='" + EmployeeNo + "' ");
        //        sql.Append(SqlServiceMemcountdtr(true, EmployeeNo));
        //        sql.Append(") as c where isRead=0 and Grade=1");
        //        #endregion
        //        object ob = DbHelperSQL.GetSingle(sql.ToString());
        //        if (ob != null)
        //        {
        //            return int.Parse(ob.ToString());
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    return 0;
        //}

        ///// <summary>
        ///// �ж��û��Ƿ��Ƿ���������Ա 2009.06.10 ������
        ///// </summary>
        ///// <param name="UserID">��ǰ�û�ID</param>
        ///// <returns></returns>
        //private static bool SqlServiceMemcount(string UserID)
        //{
        //    bool lable = false ;
        //    string strSql = "select bm ,number from dbo.HR_Employees where Number = '" + UserID+"'";
        //    DataSet ds = DbHelperSQL.Query(strSql);
        //    if (ds != null)
        //    {
        //        if (ds.Tables[0] != null)
        //        {
        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                if (dr[0].ToString().Trim() == "��������")
        //                {
        //                   lable = true ;
        //                    break ;
        //                }
        //            }
        //        }
        //    }

        //    return lable ;
            
        //}


        ///// <summary>
        ///// ����������Ա���Ѻ;������⴦�� 2009.06.10
        ///// </summary>
        ///// <param name="lable"></param>
        ///// <param name="UserID"></param>
        ///// <returns></returns>
        //public static string  SqlServiceMemcountdtr(bool lable,string UserID )
        //{
        //    StringBuilder strb = new StringBuilder(" ");
        //    if (lable)
        //    {
        //        string StrSql = "select * from dbo.RightFenpei as a left join QXSZXXXX as b on a.Number = b.ParentNumber where a.YGBH = '" + UserID+"'";
        //        DataSet ds = DbHelperSQL.Query(StrSql);
        //        if (ds != null)
        //        {
        //            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //            {
        //                strb.Append(" or ");
        //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //                {
        //                    if (i == ds.Tables[0].Rows.Count-1)
        //                    {
        //                        strb.Append(" touser = '" + ds.Tables[0].Rows[i]["SLRBH"].ToString() + "'");
        //                    }
        //                    else
        //                    {
        //                        strb.Append(" touser = '" + ds.Tables[0].Rows[i]["SLRBH"].ToString() + "' or");
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return strb.ToString();
        //}

        #endregion
    }
}