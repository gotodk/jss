using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using FMOP.DB;
using System.Text;

/// <summary>
///KHGL_QX 用于验证
/// </summary>
public class KHGL_QX
{
	public KHGL_QX()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 验证是否有编辑客户档案的权限(权限和客户档案同时验证)
    /// </summary>
    /// <param name="checktype">检查类型(工号验证,身份证验证)</param>
    /// <param name="checkNumberOrSFZ">要检查的员工编号或身份证编号</param>
    /// <param name="checkKHDANumber">要检查的客户档案编号</param>
    /// <param name="checkPassword">要检查的用户密码</param>
    /// <returns></returns>
    static public bool Check_QX(string checktype, string checkNumberOrSFZ, string checkKHDANumber, string checkPassword)
    {
        //如果客户档案不存在,返回否
        DataSet ds_KHDA = DbHelperSQL.Query("select * from KHGL_New where  Number='" + checkKHDANumber + "'");
        if (ds_KHDA.Tables[0].Rows.Count <= 0)
        {
            return false;
        }


        //工号验证
        if (checktype == "工号验证" || checktype == "以上皆可")
        {
            DataSet ds = DbHelperSQL.Query("select * from KHDAQXFPMK where  YGGH='" + checkNumberOrSFZ + "' and SSCS = '" + ds_KHDA.Tables[0].Rows[0]["SSXSQY"].ToString() + "' and ((KGLHY + ',') like '%" + ds_KHDA.Tables[0].Rows[0]["SSHY"].ToString() + ",%') ");
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //身份证验证
        if (checktype == "身份证验证" || checktype == "以上皆可")
        {
            DataSet ds = DbHelperSQL.Query("select * from KHDAQXFPMK where  SFZH='" + checkNumberOrSFZ + "' and SSCS = '" + ds_KHDA.Tables[0].Rows[0]["SSXSQY"].ToString() + "' and ((KGLHY + ',') like '%" + ds_KHDA.Tables[0].Rows[0]["SSHY"].ToString() + ",%') and GLMM is not null and GLMM <> '' and GLMM = '" + checkPassword + "' ");
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 验证是否有编辑客户档案的权限(只验证用户权限，不验证客户档案权限)
    /// </summary>
    /// <param name="checktype">检查类型(工号验证,身份证验证)</param>
    /// <param name="checkNumberOrSFZ">要检查的员工编号或身份证编号</param>
    /// <param name="checkPassword">要检查的用户密码</param>
    /// <returns></returns>
    static public bool Check_QX(string checktype, string checkNumberOrSFZ, string checkPassword)
    {


        //工号验证
        if (checktype == "工号验证" || checktype == "以上皆可")
        {
            DataSet ds = DbHelperSQL.Query("select * from KHDAQXFPMK where  YGGH='" + checkNumberOrSFZ + "' ");
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //身份证验证
        if (checktype == "身份证验证" || checktype == "以上皆可")
        {
            DataSet ds = DbHelperSQL.Query("select * from KHDAQXFPMK where  SFZH='" + checkNumberOrSFZ + "' and GLMM is not null and GLMM <> '' and GLMM = '" + checkPassword + "' ");
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }



}
