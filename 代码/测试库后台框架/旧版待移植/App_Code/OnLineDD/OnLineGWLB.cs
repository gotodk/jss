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
using System.Text;

/// <summary>
/// OnLineGWLB 的摘要说明
/// </summary>
namespace FMOP.OnLineDD
{
    public class OnLineGWLB
    {
        public OnLineGWLB()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private string _parentnumber;
        private string _cpxh;
        private decimal _sctylsj;
        private decimal _sl;
        private decimal _je;
        private string _shuxing;
        /// <summary>
        /// 
        /// </summary>
        public string parentNumber
        {
            set { _parentnumber = value; }
            get { return _parentnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CPXH
        {
            set { _cpxh = value; }
            get { return _cpxh; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal SCTYLSJ
        {
            set { _sctylsj = value; }
            get { return _sctylsj; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal SL
        {
            set { _sl = value; }
            get { return _sl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal JE
        {
            set { _je = value; }
            get { return _je; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SHUXING
        {
            set { _shuxing = value; }
            get { return _shuxing; }
        }

        public CommandInfo Add()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OnLineGWLB_bak(");
            strSql.Append("parentNumber,CPXH,SCTYLSJ,SL,JE,shuxing)");
            strSql.Append(" values (");
            strSql.Append("@parentNumber,@CPXH,@SCTYLSJ,@SL,@JE,@SHUXING)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@parentNumber", SqlDbType.VarChar,50),
					new SqlParameter("@CPXH", SqlDbType.VarChar,50),
					new SqlParameter("@SCTYLSJ", SqlDbType.Float,8),
					new SqlParameter("@SL", SqlDbType.Float,8),
					new SqlParameter("@JE", SqlDbType.Float,8),
                                        new SqlParameter("@SHUXING", SqlDbType.VarChar,50)};
            parameters[0].Value = parentNumber;
            parameters[1].Value = CPXH;
            parameters[2].Value = SCTYLSJ;
            parameters[3].Value = SL;
            parameters[4].Value = JE;
            parameters[5].Value = SHUXING;

            return MkCommandInfo(strSql.ToString(), parameters);

        }

        private CommandInfo MkCommandInfo(string sql, SqlParameter[] parameter)
        {
            CommandInfo obj = new CommandInfo();
            obj.CommandText = sql;
            obj.Parameters = parameter;
            return obj;
        }
    }
}
