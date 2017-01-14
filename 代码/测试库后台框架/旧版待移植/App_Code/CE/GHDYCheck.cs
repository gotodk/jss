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
using FMOP.DB;
/// <summary>
/// GHDYCheck 的摘要说明
/// </summary>
public class GHDYCheck
{
    private string Number;
    private int divId;
    private string userNo;
    private string jobNo;
    private bool canView;
    private bool canUpdate;
    private string level;

    public bool CanView
    {
        get
        {
            return canView;
        }
        set
        {
            canView = value;
        }
    }

    public bool CanUpdate
    {
        get
        {
            return canUpdate;
        }
        set
        {

            canUpdate = value;
        }
    }

	public GHDYCheck()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public GHDYCheck(int id, string user,string _Number)
    {
        divId = id;
        userNo = user;
        Number = _Number;

        getJobNo(userNo);
        if (jobNo.Equals("")||jobNo== null)
        {
            canUpdate = false;
            CanView = false;
        }
        else
        {
            getLevel(Number);
            getAuth();
        }
    }

    private void getJobNo(string userNo)
    {
        string cmdText;
        cmdText = "select JobNo from HR_Employees where Number='" + userNo + "'";
        SqlDataReader dr = DbHelperSQL.ExecuteReader(cmdText);
        if (dr.HasRows)
        {
            dr.Read();
            jobNo = dr["JobNo"].ToString();
        }
        else
        {
            jobNo = "";
        }
        dr.Close();
    }

    private void getLevel(string Number)
    {
        string cmdText;
        cmdText = "select DataLevel from SCJHB_JZQYZLB where Number='" + Number + "'";
        SqlDataReader dr = DbHelperSQL.ExecuteReader(cmdText);
        if (dr.HasRows)
        {
            dr.Read();
            level = dr["DataLevel"].ToString();
        }
        else
        {
            level = "";
        }
        dr.Close();
    }

    /// <summary>
    /// 判断是否存在修改权限
    /// </summary>
    /// <param name="userNo"></param>
    /// <returns></returns>
    public void getAuth()
    {
        string modifyStr = "";
        string viewStr = "";
        string cmdText = "";
        cmdText = "select CanModify,CanView,DataLevel from SCJHB_JZQYZLBQX where ((GWBH='" + jobNo + "' and Emp_NO ='all')";
        if (userNo != "")
        {
            cmdText += "or (GWBH='" + jobNo + "' and Emp_NO ='" + userNo + "'))";
        }
        else
        {
            cmdText += ")";
        }

        cmdText += " and QYBH=" + divId + " and DataLevel='" + level + "'";

        SqlDataReader dr = DbHelperSQL.ExecuteReader(cmdText);
        if (dr.HasRows)
        {
            dr.Read();
            modifyStr = dr["CanModify"].ToString();
            viewStr = dr["CanView"].ToString();
        }
        dr.Close();

        if (modifyStr != "Y")
        {
            this.canUpdate = false;
        }
        else
        {
            this.canUpdate = true;
        }

        if (viewStr != "Y")
        {
            this.canView = false;
        }
        else
        {
            this.canView = true;
        }
    }
}
