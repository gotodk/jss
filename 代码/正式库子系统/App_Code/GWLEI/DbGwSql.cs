using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Data.SqlClient;
/// <summary>
///DbGwSql 的摘要说明
/// </summary>
namespace GWLEI
{
    public class DbGwSql
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["FMOPConn"].ToString();
        public DbGwSql()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        //}
        //读出二进制文件
        //public static byte[] ExecutegetSQLimg(string SQLString)
        //{
        //    byte[] FileArray = null;
        //     using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(SQLString, connection))
        //        {
                 
              
        //            try
        //            {
        //                connection.Open();
        //                SqlDataReader sdr = cmd.ExecuteReader();
        //                if (!sdr.HasRows)
        //                {

        //                  return null;
        //                }
        //                 sdr.Read();
        //                 FileArray=(byte[])sdr[0];
        //                 return FileArray;

        //            }
        //            catch (System.Data.SqlClient.SqlException E)
        //            {
        //                connection.Close();
        //                throw new Exception(E.Message);
        //            }


        //        }
        //    }

           
        //}

        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@fs", SqlDbType.Image);
                if (fs.Length > 0)
                {
                    myParameter.Value = fs;
                }
                else
                {
                    myParameter.Value = DBNull.Value;
                }
                    cmd.Parameters.Add(myParameter);
                
                 try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }




    }
   



}
