using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.DB;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

namespace FMOP.CE
{
    /// <summary>
    /// EnterpriseCompeteFile 的摘要说明
    /// </summary>
    public class EnterpriseCompeteFile
    {
        string cmdText = "";
        public EnterpriseCompeteFile()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 根据员工编号，判断当前人员是否为系统管理员
        /// </summary>
        /// <param name="userNo"></param>
        /// <returns></returns>
        public bool isAdministrators(string userNo)
        {
            int count;
            cmdText = "select count(*) from HR_Employees where GWMC='系统管理员' and Number='" + userNo+ "'";
            count = DbHelperSQL.QueryInt(cmdText);
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 取员工岗位编号
        /// </summary>
        /// <param name="userNo"></param>
        /// <returns></returns>
        public string getJobNo(string userNo)
        {
           DataSet ds = new DataSet();
           string jobNo = "";
           cmdText = "select JobNo from HR_Employees where userNo='" + userNo +"'";
           ds = DbHelperSQL.Query(cmdText);
            if(ds== null || ds.Tables[0] == null || ds.Tables[0].Rows.Count ==0 || ds.Tables[0].Rows[0][0].ToString() =="")
            {
                jobNo = "";
            }
            else
            {
                jobNo = ds.Tables[0].Rows[0][0].ToString();
            }

            return jobNo;
        }

        /// <summary>
        /// 获取基本概况
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public DataSet getJBGK(string Number)
        {
            SqlCommand sqlcmd = new SqlCommand();
            cmdText = @"SELECT 
	                        DWMC,FDDBR,CWFZR,DWDZ,YZBM,LXDH,DZXX,ZGGSJ,ZCZE,
	                        ZGDSJ,JZCE,DWCLRQ,SHSR,DWJYD,JYFW,JJXZ,ZZJG,ZBJG,
	                        CPGK,HYDW,QYRY,QYLN,MBSC,DLSMC,SCZYL1 
                        FROM 
	                        SCJHB_JZQYZLB
                        WHERE
	                        Number=@Number";
            sqlcmd.CommandText = cmdText;
            sqlcmd.Parameters.Add("@Number", SqlDbType.VarChar, 20);
            sqlcmd.Parameters[0].Value = Number;

            return DbHelperSQL.Query(sqlcmd);

        }

        /// <summary>
        /// 根据编号删除
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public int DeleteEnterpriseCompeterFile(string Number)
        {
            //删除企业竞争资料表－主表
            List<string> deleteSqlList = new List<string>();
            deleteSqlList.Add("DELETE FROM SCJHB_JZQYZLB WHERE Number='" + Number + "'");
            deleteSqlList.Add("DELETE FROM SCJHB_JZQYZLB_GLCXX WHERE parentNumber='" + Number + "'");
            deleteSqlList.Add("DELETE FROM SCJHB_JZQYZLB_JZQYRLZYZK WHERE parentNumber='" + Number + "'");
            deleteSqlList.Add("DELETE FROM SCJHB_JZQYZLB_ZYJSRYJS WHERE parentNumber='" + Number + "'");
            deleteSqlList.Add("DELETE FROM SCJHB_JZQYZLB_YCL WHERE parentNumber='" + Number + "'");
            deleteSqlList.Add("DELETE FROM SCJHB_JZQYZLB_GYSDQK WHERE parentNumber='" + Number + "'");
            deleteSqlList.Add("DELETE FROM SCJHB_jzqyzlbPL WHERE parentNumber='" + Number + "'");

            return DbHelperSQL.ExecuteSqlTran(deleteSqlList);
        }
    }
}
