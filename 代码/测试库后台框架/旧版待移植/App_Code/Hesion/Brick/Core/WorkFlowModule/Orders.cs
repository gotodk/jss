using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using FMOP.DB;

namespace Hesion.Brick.Core.WorkFlow{
	/// <summary>
	/// </summary>
	public abstract class Orders{	
		/// <summary>
		 /// ����������Ա�����
		/// </summary>
		/// <param name="ModuleName"></param>
		/// <param name="OrderNumber"></param>
		public static string  GetCreatUserNumber(string ModuleName, string OrderNumber){
            string CreateUser = null;
            if (ModuleName != null && OrderNumber != null)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select CreateUser ");
                strSql.Append("from ");
                strSql.Append(ModuleName);
                strSql.Append(" where Number=");
                strSql.Append("'" + OrderNumber + "'");
                SqlDataReader dr = DbHelperSQL.ExecuteReader(strSql.ToString());
                if (dr.Read())
                {
                    CreateUser = dr["CreateUser"].ToString();
                }
                dr.Close();
            }
            return CreateUser;	
		}
        /// <summary>
        /// �ж�ĳ�ŵ����Ƿ��Ѿ��������
        /// </summary>
        /// <param name="OrderNumber">����</param>
        /// <returns>true:����δͨ�� false������ͨ��</returns>
        public static bool IsFinish(string ModuleName, string OrderNumber)
        {
            bool finish = false;
            if (ModuleName != null&&OrderNumber !=null )
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select CheckState ");
                strSql.Append("from ");
                strSql.Append(ModuleName);
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
	}
}