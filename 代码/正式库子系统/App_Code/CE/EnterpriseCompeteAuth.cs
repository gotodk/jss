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
namespace FMOP.CE
{
    /// <summary>
    /// Common 的摘要说明
    /// </summary>
    public class EnterpriseCompeteAuth
    {
        private int id = 0;
        private int checkstate = 0;
        private int qybh = 0;
        private string gwbh = "";
        private string emp_no = "";
        private string createTime = "";
        private string createUser = "";
        private string canModify = "";
        private string canView = "";
        private string level = "";

        public int CheckState
        {
            get
            {
                return checkstate;
            }
            set
            {
                checkstate = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }


        public int Qybh
        {
            get
            {
                return qybh;
            }
            set
            {
                qybh = value;
            }
        }

        public string Gwbh
        {
            get
            {
                return gwbh;
            }
            set
            {
                gwbh = value;
            }
        }

        public string Emp_no
        {
            get
            {
                return emp_no;
            }
            set
            {
                if (value == "")
                {
                    emp_no = "all";
                }
                else
                {
                    emp_no = value;
                }
            }
        }

        public string CreateTime
        {
            get
            {
                return createTime;
            }
            set
            {
                createTime = value;
            }
        }

        public string CreateUser
        {
            get
            {
                return createUser;
            }
            set
            {
                createUser = value;
            }
        }

        public string CanModify
        {
            get
            {
                return canModify;
            }
            set
            {
                canModify = value;
            }
        }

        public string CanView
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

        public string Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }



	    public EnterpriseCompeteAuth()
	    {
		    //
		    // TODO: 在此处添加构造函数逻辑
		    //
	    }


        /// <summary>
        /// 查询数据并绑定到DropDownList
        /// </summary>
        /// <param name="dropList"></param>
        /// <param name="topList"></param>
        public DropDownList bindList(DropDownList dropList, DropDownList topList, string cmdText)
        {
            DataSet resultSet = new DataSet();
            dropList.Items.Clear();
            if (!topList.SelectedValue.Equals(""))
            {
                resultSet = DbHelperSQL.Query(cmdText);
                dropList = forResult(dropList,resultSet);
            }
            dropList.Items.Insert(0, new ListItem("<请选择>", ""));

            return dropList;
        }

        /// <summary>
        /// 循环DataSet 到DropDownList中
        /// </summary>
        /// <param name="dropList"></param>
        public DropDownList forResult(DropDownList dropList, DataSet resultSet)
        {
            dropList.Items.Clear();
            if (resultSet != null && resultSet.Tables[0] != null)
            {
                foreach (DataRow dr in resultSet.Tables[0].Rows)
                {
                    dropList.Items.Add(new ListItem(dr[0].ToString(), dr[1].ToString()));
                }
            }

            return dropList;
        }

        /// <summary>
        /// 判断是否重复
        /// </summary>
        /// <returns></returns>
        public bool isRepeat()
        {
            int rows = 0;
            string sqlCmd = "";

            sqlCmd = "select count(*) from SCJHB_JZQYZLBQX where QYBH='" + this.qybh + "' and GWBH='" + this.gwbh + "' and  EMP_NO='" + this.emp_no + "' and CanView='" + this.canView + "' and CanModify='" + canModify + "' and DataLevel='" + level + "'";
            rows = DbHelperSQL.QueryInt(sqlCmd);
            if(rows > 0)
            {
                return false;

            }
            return true;
        }

        public int AddAuth()
        {
            SqlCommand sqlcmd = new SqlCommand();
            int rowsAffect = 0;
            sqlcmd.CommandText = "insert into SCJHB_JZQYZLBQX(id,QYBH,GWBH,EMP_NO,CanView,CanModify,DataLevel,CreateUser,CreateTime) values(@id,@QYBH,@GWBH,@EMP_NO,@CanView,@CanModify,@DataLevel,@CreateUser,getdate())";
            id = DbHelperSQL.GetMaxID("id", "SCJHB_JZQYZLBQX");

            sqlcmd.Parameters.Add("@id", SqlDbType.Int);
            sqlcmd.Parameters["@id"].Value = id;

            sqlcmd.Parameters.Add("@QYBH", SqlDbType.Int);
            sqlcmd.Parameters["@QYBH"].Value = qybh;

            sqlcmd.Parameters.Add("@GWBH", SqlDbType.VarChar, 20);
            sqlcmd.Parameters["@GWBH"].Value = gwbh;

            sqlcmd.Parameters.Add("@EMP_NO", SqlDbType.VarChar, 20);
            sqlcmd.Parameters["@EMP_NO"].Value = emp_no;

            sqlcmd.Parameters.Add("@CanView", SqlDbType.VarChar, 20);
            sqlcmd.Parameters["@CanView"].Value = canView;

            sqlcmd.Parameters.Add("@CanModify", SqlDbType.VarChar, 20);
            sqlcmd.Parameters["@CanModify"].Value = canModify;

            sqlcmd.Parameters.Add("@DataLevel", SqlDbType.VarChar, 50);
            sqlcmd.Parameters["@DataLevel"].Value = level;

            sqlcmd.Parameters.Add("@CreateUser",SqlDbType.VarChar,50);
            sqlcmd.Parameters["@CreateUser"].Value = createUser;

            rowsAffect = DbHelperSQL.Insert(sqlcmd);
            return rowsAffect;
        }

        public int UpdateAuth()
        {
            SqlCommand sqlcmd = new SqlCommand();
            int rowsAffect = 0;
            sqlcmd.CommandText = "Update SCJHB_JZQYZLBQX set QYBH=@QYBH,GWBH=@GWBH,EMP_NO=@EMP_NO,CanView=@CanView,CanModify=@CanModify,DataLevel=@DataLevel,CreateUser=@CreateUser,CreateTime=getdate() where id=@id";

            sqlcmd.Parameters.Add("@id", SqlDbType.Int);
            sqlcmd.Parameters["@id"].Value = id;

            sqlcmd.Parameters.Add("@QYBH", SqlDbType.Int);
            sqlcmd.Parameters["@QYBH"].Value = qybh;

            sqlcmd.Parameters.Add("@GWBH", SqlDbType.VarChar, 20);
            sqlcmd.Parameters["@GWBH"].Value = gwbh;

            sqlcmd.Parameters.Add("@EMP_NO", SqlDbType.VarChar, 20);
            sqlcmd.Parameters["@EMP_NO"].Value = emp_no;

            sqlcmd.Parameters.Add("@CanView", SqlDbType.VarChar, 20);
            sqlcmd.Parameters["@CanView"].Value = canView;

            sqlcmd.Parameters.Add("@CanModify", SqlDbType.VarChar, 20);
            sqlcmd.Parameters["@CanModify"].Value = canModify;

            sqlcmd.Parameters.Add("@DataLevel", SqlDbType.VarChar, 50);
            sqlcmd.Parameters["@DataLevel"].Value = level;

            sqlcmd.Parameters.Add("@CreateUser", SqlDbType.VarChar, 50);
            sqlcmd.Parameters["@CreateUser"].Value = createUser;

            rowsAffect = DbHelperSQL.Update(sqlcmd);
            return rowsAffect;
        }

        public int DeleteAuth()
        {
            string sqlcmd = "";
            int rowsAffect = 0;
            sqlcmd = " delete from SCJHB_JZQYZLBQX where id=" + id;
            rowsAffect = DbHelperSQL.ExecuteSql(sqlcmd);
            return rowsAffect;
        }
    }
}
