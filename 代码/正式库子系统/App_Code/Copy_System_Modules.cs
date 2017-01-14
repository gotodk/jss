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
using System.Text;
using FMOP.DB;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;

/// <summary>
/// Copy_System_Modules 的摘要说明
/// </summary>
public class Copy_System_Modules
{
	public Copy_System_Modules()
	{
        string delsql = "TRUNCATE TABLE system_auth_guodu ";
        DbHelperSQL.ExecuteSql(delsql);
        string sql = "SELECT name,AuthType,ModuleType FROM system_Modules ";
        DataSet moduleds = DbHelperSQL.Query(sql);
        for (int i = 0; i < moduleds.Tables[0].Rows.Count; i++)
        {
            string modulename = moduleds.Tables[0].Rows[i]["name"].ToString();
            int AuthType = int.Parse(moduleds.Tables[0].Rows[i]["AuthType"].ToString());
            int ModuleType = int.Parse(moduleds.Tables[0].Rows[i]["ModuleType"].ToString());
            if (AuthType == 0)
            {
                Add(modulename, ModuleType, "全公司", 3, 1, 1, 1, 0);
            }
            else
            {
                if (ModuleType == 1)
                {
                    WorkFlowModule wf = new WorkFlowModule(modulename);
                    if (wf.authentication != null && wf.authentication.retunauthds != null && wf.authentication.retunauthds.Tables.Count!=0)
                    {
                        DataSet ds = wf.authentication.retunauthds;
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            string roleName = ds.Tables[0].Rows[j]["roleName"].ToString();
                            int roleType = int.Parse(ds.Tables[0].Rows[j]["roleType"].ToString());
                            int canAdd = getbit(ds.Tables[0].Rows[j]["canAdd"].ToString());
                            int canView = getbit(ds.Tables[0].Rows[j]["canView"].ToString());
                            int canModify = getbit(ds.Tables[0].Rows[j]["canModify"].ToString());
                            Add(modulename, ModuleType, roleName, roleType, canAdd, canModify, canView, 0);
                        }

                    }
                    if (wf.check != null && wf.check.retunchekds != null && wf.check.retunchekds.Tables.Count != 0)
                    {
                        DataSet checkds = wf.check.retunchekds;

                        for (int k = 0; k < checkds.Tables[0].Rows.Count; k++)
                        {
                            string roleName = checkds.Tables[0].Rows[k]["roleName"].ToString();
                            int roleType = int.Parse(checkds.Tables[0].Rows[k]["roleType"].ToString());
                            Add(modulename, ModuleType, roleName, roleType, 0, 0, 0,1);
                        }
                    }
                }
                else if(ModuleType==4)
                {
                    DefinedModule wf = new DefinedModule(modulename);
                    if (wf.authentication != null && wf.authentication.retunauthds != null && wf.authentication.retunauthds.Tables.Count != 0)
                    {
                        DataSet ds = wf.authentication.retunauthds;
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            string roleName = ds.Tables[0].Rows[j]["roleName"].ToString();
                            int roleType = int.Parse(ds.Tables[0].Rows[j]["roleType"].ToString());
                            int canAdd = getbit(ds.Tables[0].Rows[j]["canAdd"].ToString());
                            int canView = getbit(ds.Tables[0].Rows[j]["canView"].ToString());
                            int canModify = getbit(ds.Tables[0].Rows[j]["canModify"].ToString());
                           
                            Add(modulename, ModuleType, roleName, roleType, canAdd, canModify, canView, 0);
                        }

                    }
                    if (wf.check != null && wf.check.retunchekds != null && wf.check.retunchekds.Tables.Count != 0)
                    {
                        DataSet checkds = wf.check.retunchekds;

                        for (int k = 0; k < checkds.Tables[0].Rows.Count; k++)
                        {
                            string roleName = checkds.Tables[0].Rows[k]["roleName"].ToString();
                            int roleType = int.Parse(checkds.Tables[0].Rows[k]["roleType"].ToString());
                            Add(modulename, ModuleType, roleName, roleType, 0, 0, 0,1);
                        }
                    }
                }

               
            }
        }
    }

    /// <summary>
    /// 增加一条数据
    /// </summary>
    public int Add(string ModuleName, int ModuleType, string Role, int Type, int CanAdd, int CanEdit, int CanView, int NeedAudit)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("insert into system_auth_guodu(");
        strSql.Append("ModuleName,ModuleType,Role,Type,CanAdd,CanEdit,CanView,NeedAudit)");
        strSql.Append(" values (");
        strSql.Append("@ModuleName,@ModuleType,@Role,@Type,@CanAdd,@CanEdit,@CanView,@NeedAudit)");
        strSql.Append(";select @@IDENTITY");
        SqlParameter[] parameters = {
					new SqlParameter("@ModuleName", SqlDbType.VarChar,50),
                    new SqlParameter("@ModuleType", SqlDbType.SmallInt,2),
					new SqlParameter("@Role", SqlDbType.VarChar,50),
					new SqlParameter("@Type", SqlDbType.SmallInt,2),
					new SqlParameter("@CanAdd", SqlDbType.Bit,1),
					new SqlParameter("@CanEdit", SqlDbType.Bit,1),
					new SqlParameter("@CanView", SqlDbType.Bit,1),
					new SqlParameter("@NeedAudit", SqlDbType.Bit,1)};
        parameters[0].Value = ModuleName;
        parameters[1].Value = ModuleType;
        parameters[2].Value = Role;
        parameters[3].Value = Type;
        parameters[4].Value = CanAdd;
        parameters[5].Value = CanEdit;
        parameters[6].Value = CanView;
        parameters[7].Value = NeedAudit;
        

        object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
        if (obj == null)
        {
            return 1;
        }
        else
        {
            return Convert.ToInt32(obj);
        }
    }

    public int getbit(string bit)
    {
        if (bit.ToLower() == "true")
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
                    

}
