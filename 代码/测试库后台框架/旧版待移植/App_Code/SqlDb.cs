using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

/// <summary>
///Code By GuoTuo.Just For the First Temp. 2012-02-17
/// </summary>
public class SqlDb
{
    public SqlConnection sqlcon;  //申明一个SqlConnection对象
    protected string ConnectionString;
    public SqlDb()
	{
        ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
	}
    //保护方法，打开数据库连接
		private void Open()
		{
		  //判断数据库连接是否存在
			if (sqlcon == null)
			{
			  //不存在，新建并打开
				sqlcon = new SqlConnection(ConnectionString);
				sqlcon.Open();
			}
			else
			{
			  //存在，判断是否处于关闭状态
			  if (sqlcon.State.Equals(ConnectionState.Closed))
				  sqlcon.Open();    //连接处于关闭状态，重新打开
			}
		}

		//公有方法，关闭数据库连接
		public void Close() 
		{
			if (sqlcon.State.Equals(ConnectionState.Open))
			{
                sqlcon.Close();     //连接处于打开状态，关闭连接
			}
		}

        /// <summary>
		/// 析构函数，释放非托管资源
		/// </summary>
        ~SqlDb()
		{
			try
			{
				if (sqlcon != null)
					sqlcon.Close();
			}
			catch{}
			try
			{
				Dispose();
			}
			catch{}
		}

		//公有方法，释放资源
		public void Dispose()
		{
			if (sqlcon != null)		// 确保连接被关闭
			{
				sqlcon.Dispose();
				sqlcon = null;
			}
		}		
    /// <summary>
    /// 此方法返回一个DataSet类型
    /// </summary>
    /// <param name="SqlCom">要执行的SQL语句</param>
    /// <returns></returns>
        public DataSet ExceDS(string SqlCom)
        {
            try
            {
                Open();   //打开链接
                SqlCommand sqlcom = new SqlCommand(SqlCom, sqlcon);
                SqlDataAdapter sqldata = new SqlDataAdapter();
                sqldata.SelectCommand = sqlcom;
                DataSet ds = new DataSet();
                sqldata.Fill(ds);
                return ds;
            }
            finally
            {
                Close();
            }
        }
    /// <summary>
    /// 此方法实现数据绑定到GridView中
    /// </summary>
    /// <param name="dl">要绑定的控件</param>
    /// <param name="SqlCom">要执行的SQL语句</param>
    /// <returns></returns>
    public bool BindDataGridView(GridView dl, string SqlCom)
    {
        dl.DataSource = this.ExceDS(SqlCom);
        try
        {
            dl.DataBind();
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            Close();
        }
    }
}
