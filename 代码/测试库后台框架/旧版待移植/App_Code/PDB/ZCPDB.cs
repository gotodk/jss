using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using FMOP.DB;
using System.Data.SqlClient;

/// <summary>
/// ZCPDB 的摘要说明
/// </summary>

namespace FMOP.PDB
{
    public class zcpdb
    {
        public zcpdb()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private string currentDate;
        private string number;
        private string zcbm;
        private string zcmc;
        private string xh;
        private string gg;
        private string dw;
        private string dj;
        private string zmsl;

        public string userName;


        public string CurrentDate
        {
            get
            {
                return currentDate;
            }
            set
            {
                currentDate = value;
            }
        }
        public string Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

        public string ZCBM
        {
            get
            {
                return zcbm;
            }
            set
            {
                zcbm = value;
            }
        }

        public string ZCMC
        {
            get
            {
                return zcmc;
            }
            set
            {
                zcmc = value;
            }
        }

        public string XH
        {
            get
            {
                return xh;
            }
            set
            {
                xh = value;
            }
        }

        public string GG
        {
            get
            {
                return gg;
            }
            set
            {
                gg = value;
            }
        }

        public string DW
        {
            get
            {
                return dw;
            }
            set
            {
                dw = value;
            }
        }

        public string DJ
        {
            get
            {
                return dj;
            }
            set
            {
                dj = value;
            }
        }

        public string ZMSL
        {
            get
            {
                return zmsl;
            }
            set
            {
                zmsl = value;
            }
        }

        public int Insert(DataSet pdlbset)
        {
            List<CommandInfo> saveToDb = new List<CommandInfo>();
            //添加主表
            saveToDb = insertZCPDB(saveToDb);

            //添加子表
            saveToDb = insertZCPDLB(pdlbset, saveToDb);
            return  DbHelperSQL.ExecuteSqlTranWithIndentity(saveToDb);
        }

        private List<CommandInfo> insertZCPDB(List<CommandInfo> saveToDb)
        {
            CommandInfo zcpdbObj = new CommandInfo();
            string cmdText = "insert into CWGL_ZCPDB(Number,RQ,NextChecker,CheckState,CreateUser,CreateTime) values(@Number,@RQ,@NextChecker,1,@CreateUser,getdate())";
            SqlParameter[] ParamArray = new SqlParameter[4];
            ParamArray[0] = new SqlParameter("@number", SqlDbType.VarChar, 50);
            ParamArray[0].Value = number;

            ParamArray[1] = new SqlParameter("@RQ", SqlDbType.DateTime);
            ParamArray[1].Value = Convert.ToDateTime(CurrentDate);

            ParamArray[2] = new SqlParameter("@NextChecker", SqlDbType.VarChar, 50);
            ParamArray[2].Value = "";

            ParamArray[3] = new SqlParameter("@CreateUser", SqlDbType.VarChar, 50);
            ParamArray[3].Value = userName;

            zcpdbObj = MakeObj(cmdText, ParamArray, CommandType.Text);
            saveToDb.Add(zcpdbObj);
            return saveToDb;

        }

        private List<CommandInfo> insertZCPDLB(DataSet resultSet, List<CommandInfo> saveToDb)
        {
            string cmdText = "";
            foreach (DataRow dr in resultSet.Tables[0].Rows)
            {
                CommandInfo obj = new CommandInfo();
                zcbm = dr["ZCBH"].ToString();
                zcmc = dr["ZCMC"].ToString();
                xh = dr["XH"].ToString();
                gg = dr["GG"].ToString();
                dw = dr["DW"].ToString();
                dj = dr["DJ"].ToString();
                zmsl = dr["SL"].ToString();

                cmdText = " insert into ZCPDLB(parentNumber,ZCBM,ZCMC,XH,GG,DW,DJ,ZMSL)values(@parentNumber,@ZCBM,@ZCMC,@XH,@GG,@DW,@DJ,@ZMSL)";
                SqlParameter[] ParamArray = new SqlParameter[8];
                ParamArray[0] = new SqlParameter("@parentNumber", SqlDbType.VarChar, 50);
                ParamArray[0].Value = number;

                ParamArray[1] = new SqlParameter("@ZCBM", SqlDbType.VarChar, 50);
                ParamArray[1].Value = zcbm;

                ParamArray[2] = new SqlParameter("@ZCMC", SqlDbType.VarChar, 50);
                ParamArray[2].Value = zcmc;

                ParamArray[3] = new SqlParameter("@XH", SqlDbType.VarChar, 50);
                ParamArray[3].Value = xh;

                ParamArray[4] = new SqlParameter("@GG", SqlDbType.VarChar, 50);
                ParamArray[4].Value = gg;

                ParamArray[5] = new SqlParameter("@DW", SqlDbType.VarChar, 50);
                ParamArray[5].Value = dw;

                ParamArray[6] = new SqlParameter("@DJ", SqlDbType.Float);
                ParamArray[6].Value = dj;

                ParamArray[7] = new SqlParameter("@ZMSL", SqlDbType.Float);
                ParamArray[7].Value = zmsl;
                obj = MakeObj(cmdText, ParamArray, CommandType.Text);
                saveToDb.Add(obj);
            }

            return saveToDb;
        }


        /// <summary>
        /// 创建泛型对象
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterArray"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        private static CommandInfo MakeObj(string strSql, SqlParameter[] parameterArray, CommandType cmdType)
        {
            CommandInfo commInfo = new CommandInfo();
            commInfo.CommandText = strSql;
            commInfo.Parameters = parameterArray;
            commInfo.cmdType = cmdType;
            return commInfo;
        }
    }
}