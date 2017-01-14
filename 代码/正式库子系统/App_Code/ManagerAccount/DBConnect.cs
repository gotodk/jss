using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// AccountDBConnect 的摘要说明
/// </summary>
namespace FM.ManagerAccount
{
public class DBConnect
{
    private String connectiong = @"Data Source=192.168.0.64\SQL2005;Initial Catalog=110;Persist Security Info=True;User ID=sa;Password=100zzcom;Max Pool Size=1000;Connection LifeTime=120;";
	private SqlConnection conn = null;
    private int rowsAffect = 0;
    public DBConnect()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        conn = new SqlConnection(connectiong);
	}

    /// <summary>
    /// 执行新增操作
    /// </summary>
    /// <param name="sqlcmd"></param>
    public int Insert(SqlCommand sqlcmd)
    {
        return Execute(sqlcmd);
    }

    /// <summary>
    /// 执行修改操作
    /// </summary>
    /// <param name="sqlcmd"></param>
    /// <returns></returns>
    public int Update(SqlCommand sqlcmd)
    {
        return Execute(sqlcmd);
    }

    /// <summary>
    /// 执行删除操作
    /// </summary>
    /// <param name="sqlcmd"></param>
    /// <returns></returns>
    public int Delete(SqlCommand sqlcmd)
    {
        return Execute(sqlcmd);
    }

    /// <summary>
    /// 操作操作的具体方法
    /// </summary>
    /// <param name="sqlcmd"></param>
    /// <returns></returns>
    private int Execute(SqlCommand sqlcmd)
    {
        rowsAffect = 0;
        if(sqlcmd != null)
        {
               sqlcmd.Connection = conn;
               conn.Open();
               rowsAffect = sqlcmd.ExecuteNonQuery();
               conn.Close();
        }
        return rowsAffect;
    }

    /// <summary>
    /// 根据命令和属性生成sqlCommand对象
    /// </summary>
    /// <param name="sqltext"></param>
    /// <param name="objProperty"></param>
    /// <returns></returns>
    public int Insert(String sqltext,DBproperty objProperty)
    {
        rowsAffect = 0;
        SqlCommand sqlcmd = new SqlCommand(sqltext);
        if (objProperty != null)
        {
            sqlcmd.Parameters.Add("@UserNumber", SqlDbType.NVarChar, 50);
            sqlcmd.Parameters["@UserNumber"].Value = objProperty.userNo;

            sqlcmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50);
            sqlcmd.Parameters["@UserName"].Value = objProperty.userName;

            sqlcmd.Parameters.Add("@UserPassword", SqlDbType.NVarChar, 50);
            sqlcmd.Parameters["@UserPassword"].Value = objProperty.userPassword;

            sqlcmd.Parameters.Add("@PassQuestion", SqlDbType.NVarChar, 50);
            sqlcmd.Parameters["@PassQuestion"].Value = objProperty.passQuestion;

            sqlcmd.Parameters.Add("@PassAnswer", SqlDbType.NVarChar, 50);
            sqlcmd.Parameters["@PassAnswer"].Value = objProperty.passAnswer;

            sqlcmd.Parameters.Add("@safeCode", SqlDbType.NVarChar, 20);
            sqlcmd.Parameters["@safeCode"].Value = objProperty.SafeCode;

            sqlcmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            sqlcmd.Parameters["@Email"].Value = objProperty.EMAIL;

            sqlcmd.Parameters.Add("@IsCorporation", SqlDbType.SmallInt, 5);
            sqlcmd.Parameters["@IsCorporation"].Value = objProperty.isCorporation;

            sqlcmd.Parameters.Add("@Integral", SqlDbType.Int, 6);
            sqlcmd.Parameters["@Integral"].Value = objProperty.integral;

            sqlcmd.Parameters.Add("@FS_Money", SqlDbType.Money, 10);
            sqlcmd.Parameters["@FS_Money"].Value = objProperty.fS_Money;

            sqlcmd.Parameters.Add("@RegTime", SqlDbType.DateTime);
            sqlcmd.Parameters["@RegTime"].Value = Convert.ToDateTime(objProperty.regTime);

            sqlcmd.Parameters.Add("@GroupID", SqlDbType.SmallInt, 6);
            sqlcmd.Parameters["@GroupID"].Value = objProperty.groupID;

            sqlcmd.Parameters.Add("@isLock", SqlDbType.SmallInt, 6);
            sqlcmd.Parameters["@isLock"].Value = objProperty.IsLock;

            rowsAffect = Execute(sqlcmd);
        }
        return rowsAffect;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="sqltext"></param>
    /// <param name="objProperty"></param>
    /// <returns></returns>
    public int Update(string sqltext, DBproperty objProperty)
    {
        rowsAffect = 0;
        SqlCommand sqlcmd = new SqlCommand(sqltext);
        if (objProperty != null)
        {
            sqlcmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50);
            sqlcmd.Parameters["@UserName"].Value = objProperty.userName;

            sqlcmd.Parameters.Add("@UserPassword", SqlDbType.NVarChar, 50);
            sqlcmd.Parameters["@UserPassword"].Value = objProperty.userPassword;

            rowsAffect = Execute(sqlcmd);
        }
        return rowsAffect;
    }
}

}
