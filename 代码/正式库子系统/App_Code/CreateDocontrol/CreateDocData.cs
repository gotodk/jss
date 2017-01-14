using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using Hesion.Brick.Core;

/// <summary>
///CreateDocData 的摘要说明
/// </summary>
public class CreateDocData
{

    public static string connectionString = ConfigurationManager.ConnectionStrings["FMOPConn"].ToString(); //如果要换数据库，你可以参考SysManage  
	public CreateDocData()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}


    /// <summary>
    /// 插入函数
    /// </summary>
    /// <param name="Number"></param>
    /// <param name="DocType"></param>
    /// <param name="DocTitle"></param>
    /// <param name="DocFullName"></param>
    /// <param name="KeyWords"></param>
    /// <param name="DocStatus"></param>
    /// <param name="DocDate"></param>
    /// <param name="Createtime"></param>
    /// <param name="CreateUser"></param>
    /// <param name="DocMentAttach"></param>
    /// <param name="AttachedName"></param>
    /// <param name="AttachedExName"></param>
    /// <param name="DocExpendName"></param>
    /// <returns></returns>
    public static bool InsertData(string Number, int DocType, string DocTitle, string DocFullName, string KeyWords, int DocStatus, DateTime DocDate, DateTime Createtime, string CreateUser, byte[] DocMentAttach, string AttachedName, string AttachedExName, string DocExpendName, string remark)
    {

        try
        {

            string insertstr = "Insert into RegularDocMag (Number,DocType,DocTitle,DocFullName,KeyWords,DocStatus,DocDate,Createtime,CreateUser,EditeUser,Editetime ,DocExpendName,remark1 ";
            if (DocMentAttach != null)
            {
                insertstr += ",DocMentAttach,AttachedName,AttachedExName";
            }

            insertstr += ") values (@Number,@DocType,@DocTitle,@DocFullName,@KeyWords,@DocStatus,@DocDate,@Createtime,@CreateUser,@EditeUser,@Editetime,@DocExpendName,@remark1 ";
            if (DocMentAttach != null)
            {
                insertstr += ",@DocMentAttach,@AttachedName,@AttachedExName";
            }

            insertstr += ")";
            //插入数据库语句
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand(insertstr, conn);
        
            comm.Parameters.Add(new SqlParameter("@Number", SqlDbType.VarChar, 50));
            comm.Parameters["@Number"].Value = Number;
            comm.Parameters.Add(new SqlParameter("@DocType", SqlDbType.Int, 4));
            comm.Parameters["@DocType"].Value = DocType;
            comm.Parameters.Add(new SqlParameter("@DocTitle", SqlDbType.VarChar, 200));
            comm.Parameters["@DocTitle"].Value = DocTitle;
            //comm.Parameters.add(new sqlparameter("@DocTitle", SqlDbType.VarChar, 200));
            //comm.Parameters["@DocTitle"].value = DocTitle;
            comm.Parameters.Add(new SqlParameter("@DocFullName", SqlDbType.VarChar, 300));
            comm.Parameters["@DocFullName"].Value = DocFullName;
            comm.Parameters.Add(new SqlParameter("@KeyWords", SqlDbType.VarChar, 50));
            comm.Parameters["@KeyWords"].Value = KeyWords;
            comm.Parameters.Add(new SqlParameter("@DocStatus", SqlDbType.Int, 4));
            comm.Parameters["@DocStatus"].Value = DocStatus;
            comm.Parameters.Add(new SqlParameter("@DocDate", SqlDbType.DateTime));
            comm.Parameters["@DocDate"].Value = DocDate;
            comm.Parameters.Add(new SqlParameter("@Createtime", SqlDbType.DateTime));
            comm.Parameters["@Createtime"].Value = Createtime;
            comm.Parameters.Add(new SqlParameter("@CreateUser", SqlDbType.VarChar, 50));
            comm.Parameters["@CreateUser"].Value = CreateUser;

            comm.Parameters.Add(new SqlParameter("@Editetime", SqlDbType.DateTime));
            comm.Parameters["@Editetime"].Value = DateTime.Now;
            comm.Parameters.Add(new SqlParameter("@EditeUser", SqlDbType.VarChar, 50));
            comm.Parameters["@EditeUser"].Value = CreateUser;

            comm.Parameters.Add(new SqlParameter("@remark1", SqlDbType.VarChar, 50));
            comm.Parameters["@remark1"].Value = remark;
            if (DocMentAttach != null)
            {
                comm.Parameters.Add(new SqlParameter("@DocMentAttach", SqlDbType.Image));
                comm.Parameters["@DocMentAttach"].Value = DocMentAttach;
                //comm.Parameters.Add(new SqlParameter("@DocMentAttach", SqlDbType.VarChar, 50));
                //comm.Parameters["@CreateUser"].Value = DocMentAttach;
                comm.Parameters.Add(new SqlParameter("@AttachedName", SqlDbType.VarChar, 200));
                comm.Parameters["@AttachedName"].Value = AttachedName;
                comm.Parameters.Add(new SqlParameter("@AttachedExName", SqlDbType.VarChar, 200));
                comm.Parameters["@AttachedExName"].Value = AttachedExName;
            }
            comm.Parameters.Add(new SqlParameter("@DocExpendName", SqlDbType.VarChar, 200));
            comm.Parameters["@DocExpendName"].Value = DocExpendName;
            conn.Open();//打开数据库连接
            comm.ExecuteNonQuery();//添加数据
            conn.Close();//关闭数据库
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            return false;

        }

    }

    /// <summary>
    /// 修改函数
    /// </summary>
    /// <param name="Number"></param>
    /// <param name="DocType"></param>
    /// <param name="DocTitle"></param>
    /// <param name="DocFullName"></param>
    /// <param name="KeyWords"></param>
    /// <param name="DocStatus"></param>
    /// <param name="DocDate"></param>
    /// <param name="Createtime"></param>
    /// <param name="CreateUser"></param>
    /// <param name="DocMentAttach"></param>
    /// <param name="AttachedName"></param>
    /// <param name="AttachedExName"></param>
    /// <param name="DocExpendName"></param>
    /// <returns></returns>
    public static bool UpdateData( int ID , string DocTitle,  string KeyWords, int DocStatus,  string EditUser,DateTime Editetime ,byte[] DocMentAttach, string AttachedName,string AttachedExName, string Remark)
    {
     
        try
        {
            string insertstr = "update RegularDocMag  set  DocTitle=@DocTitle,KeyWords=@KeyWords,DocStatus=@DocStatus,EditeUser=@EditeUser,Editetime = @Editetime,remark1=@remark1";
        if(DocMentAttach != null)
        {
          insertstr += ",DocMentAttach=@DocMentAttach,AttachedName=@AttachedName,AttachedExName=@AttachedExName" ;
        }

        insertstr += " where ID = @ID" ;
            //插入数据库语句
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand comm = new SqlCommand(insertstr, conn);


            comm.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
            comm.Parameters["@ID"].Value = ID;
            //comm.Parameters.Add(new sqlparameter("@Number", SqlDbType.varchar, 50));
            //comm.Parameters["@Number"].value = Number;
            //comm.Parameters.Add(new sqlparameter("@DocType", SqlDbType.Int, 4));
            //comm.Parameters["@DocType"].value = DocType;
            comm.Parameters.Add(new SqlParameter("@DocTitle", SqlDbType.VarChar, 200));
            comm.Parameters["@DocTitle"].Value = DocTitle;
            
            //comm.Parameters.add(new sqlparameter("@DocFullName", SqlDbType.VarChar, 300));
            //comm.Parameters["@DocFullName"].value = DocFullName;
            comm.Parameters.Add(new SqlParameter("@KeyWords", SqlDbType.VarChar, 50));
            comm.Parameters["@KeyWords"].Value = KeyWords;
            comm.Parameters.Add(new SqlParameter("@DocStatus", SqlDbType.Int, 4));
            comm.Parameters["@DocStatus"].Value = DocStatus;
            //comm.Parameters.add(new sqlparameter("@DocDate", SqlDbType.DateTime));
            //comm.Parameters["@DocDate"].value = DocDate;
            comm.Parameters.Add(new SqlParameter("@Editetime", SqlDbType.DateTime));
            comm.Parameters["@Editetime"].Value = Editetime;

            comm.Parameters.Add(new SqlParameter("@EditeUser", SqlDbType.VarChar, 50));
            comm.Parameters["@EditeUser"].Value = EditUser;

            comm.Parameters.Add(new SqlParameter("@remark1", SqlDbType.VarChar, 50));
            comm.Parameters["@remark1"].Value = Remark;
            if(DocMentAttach != null)
            {
                comm.Parameters.Add(new SqlParameter("@AttachedName", SqlDbType.VarChar, 200));
                comm.Parameters["@AttachedName"].Value = AttachedName;
                comm.Parameters.Add(new SqlParameter("@AttachedExName", SqlDbType.VarChar, 200));
                comm.Parameters["@AttachedExName"].Value = AttachedExName;
                comm.Parameters.Add(new SqlParameter("@DocMentAttach", SqlDbType.Binary, 2000));
                comm.Parameters["@DocMentAttach"].Value = DocMentAttach;
            }
            //comm.Parameters.add(new sqlparameter("@DocExpendName", SqlDbType.VarChar, 200));
            //comm.Parameters["@DocTitle"].value = DocExpendName;
            conn.Open();//打开数据库连接
            comm.ExecuteNonQuery();//添加数据
            conn.Close();//关闭数据库
            return true;
        }
    
        catch (Exception ex)
        {
            throw ex;
            return false;

        }

    }
         		

}
