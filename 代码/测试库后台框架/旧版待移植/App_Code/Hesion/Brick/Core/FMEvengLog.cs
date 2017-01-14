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
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
/// <summary>
/// FMEvengLog 的摘要说明
/// </summary>
namespace Hesion.Brick.Core
{
    public class FMEvengLog
    {
        static List<CommandInfo> saveDB = new List<CommandInfo>();
        static string type = string.Empty;
        public FMEvengLog()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 保存到日志数据表中 -- 针对于SQL语句类型
        /// </summary>
        /// <param name="user">当前登录的员工</param>
        /// <param name="module">当前所在的模块名</param>
        /// <param name="type">执行操作的类型</param>
        /// <param name="table">操作的表名</param>
        public static void SaveToLog(string user, string module, string IP, string pageName, string keyNumber, string sqlCmd,string dailiren)
        {
            saveDB = new List<CommandInfo>();
            if (keyNumber != "" && sqlCmd != "")
            {
                type = GetOperateType(user, sqlCmd, module, pageName, keyNumber);
                saveDB.Add(ExecuteLog(user, module, IP, pageName, keyNumber, type, dailiren));
                DbHelperSQL.ExecuteSqlTranWithIndentity(saveDB);
            }
            else
            {
                if (pageName == "登录系统")
                {
                    type = "用户:" + user + "已登录系统 .";
                    saveDB.Add(ExecuteLog(user, module, IP, pageName, keyNumber, type, dailiren));
                    DbHelperSQL.ExecuteSqlTranWithIndentity(saveDB);
                }

                if (pageName == "退出系统")
                {
                    type = "用户:" + user + "已退出系统 .";
                    saveDB.Add(ExecuteLog(user, module, IP, pageName, keyNumber, type, dailiren));
                    DbHelperSQL.ExecuteSqlTranWithIndentity(saveDB);
                }
            }
        }

        ///// <summary>
        ///// 记录事件执行的操作 -- 针对于执行事务的对象
        ///// </summary>
        ///// <param name="user">当前登录的员工</param>
        ///// <param name="module">当前所操作的模块名</param>
        ///// <param name="sqlList">产生的SQL集合</param>
        ///// <returns></returns>
        public static void SaveToLog(string user, string module, string IP, string pageName, string keyNumber, List<CommandInfo> sqlList, string dailiren)
        {
            type = string.Empty;  //SQL 语句类型 insert,Update,delete
            saveDB = new List<CommandInfo>();

            

            //循环 List<CommandInfo>,查找表名和类型
            foreach (CommandInfo myDE in sqlList)
            {
                string cmdText = myDE.CommandText;
                //取执行操作的类型
                if (myDE.cmdType != CommandType.StoredProcedure)
                {
                    type = GetOperateType(user,cmdText, module, pageName, keyNumber);
                }
                else
                {
                    type = OperText(user, module, pageName, keyNumber, "存储过程", cmdText);
                }
                saveDB.Add(ExecuteLog(user, module, IP, pageName, keyNumber, type, dailiren));
            }

            //执行新增
            DbHelperSQL.ExecuteSqlTranWithIndentity(saveDB);
        }

        ///// <summary>
        ///// 记录事件执行的操作 -- 针对于执行事务的对象
        ///// </summary>
        ///// <param name="user">当前登录的员工</param>
        ///// <param name="module">当前所操作的模块名</param>
        ///// <param name="sqlList">产生的SQL集合</param>
        ///// <returns></returns>
        public static void SaveToLog(string user, string module, string IP, string pageName, string keyNumber, CommandInfo sqlList, string dailiren)
        {
            type = string.Empty;  //SQL 语句类型 insert,Update,delete
            saveDB = new List<CommandInfo>();
            string cmdText = sqlList.CommandText;
            if (sqlList.cmdType == CommandType.StoredProcedure)
            {
                type = OperText(user, module, pageName, keyNumber, "存储过程", cmdText);                
            }
            else
            {
                type = GetOperateType(user, cmdText, module, pageName, keyNumber);
            }

            saveDB.Add(ExecuteLog(user, module, IP, pageName, keyNumber, type, dailiren));
            //执行新增
            DbHelperSQL.ExecuteSqlTranWithIndentity(saveDB);
        }

        /// <summary>
        /// 记录事件执行的操作--针对sqlCommand 对象
        /// </summary>
        /// <param name="user"></param>
        /// <param name="module"></param>
        /// <param name="comm"></param>
        public static void SaveToLog(string user, string module, string IP, string pageName, string keyNumber, SqlCommand comm, string dailiren)
        {
            string cmdText = string.Empty;
            type = string.Empty;
            saveDB = new List<CommandInfo>();
            if (comm != null)
            {
                cmdText = comm.CommandText;
                if (comm.CommandType == CommandType.Text)
                {
                    type = GetOperateType(user, cmdText, module, pageName, keyNumber);
                }
                else
                {
                    type = OperText(user,module,pageName,keyNumber,"存储过程",cmdText);
                }
                saveDB.Add(ExecuteLog(user, module, IP, pageName, keyNumber, type, dailiren));
                DbHelperSQL.ExecuteSqlTranWithIndentity(saveDB);
            }
        }

        /// <summary>
        /// 产生日志SQL 命令与参数
        /// </summary>
        private static CommandInfo ExecuteLog(string user, string module, string IP, string pageName, string keyNumber, string type,string dailiren)
        {
            //模块中文名
            string Name = string.Empty;

            if (module != "")
            {
                Name = FMOP.Module.TabTool.getTitle(module);
            }
            else
            {
                Name = "";
            }

            //2009.06.05 王永辉修改 添加字段daliren的插入
            string sqlCmd = "insert into FMEventLog(module,Name,[user],OperateType,OperateTime,IP,pageName,Number,daliren)Values(@module,@Name,@user,@type,@OperateTime,@IP,@pageName,@Number,@daliren)";
            SqlParameter[] parameterArray = new SqlParameter[9];
            
            parameterArray[0] = new SqlParameter("@module", SqlDbType.VarChar, 50);
            if (module != "")
            {
                parameterArray[0].Value = module;
            }
            else
            {
                parameterArray[0].Value = "无";
            }
            parameterArray[1] = new SqlParameter("@Name", SqlDbType.VarChar, 50);
            if (Name != "")
            {
                parameterArray[1].Value = Name;
            }
            else
            {
                parameterArray[1].Value = "无";
            }
            parameterArray[2] = new SqlParameter("@user", SqlDbType.VarChar, 50);
            if (user != "")
            {
                parameterArray[2].Value = user;
            }
            else
            {
                parameterArray[2].Value = "无";
            }

            parameterArray[3] = new SqlParameter("@type", SqlDbType.VarChar,4000);
            if (type != "")
            {
                parameterArray[3].Value = type;
            }
            else
            {
                parameterArray[3].Value = "无";
            }

            parameterArray[4] = new SqlParameter("@IP", SqlDbType.VarChar, 16);
            parameterArray[4].Value = IP;

            parameterArray[5] = new SqlParameter("@pageName", SqlDbType.VarChar, 100);
            if (pageName != "")
            {
                parameterArray[5].Value = pageName;
            }
            else
            {
                parameterArray[5].Value = "无";
            }

            parameterArray[6] = new SqlParameter("@Number", SqlDbType.VarChar, 50);
            if (keyNumber != "")
            {
                parameterArray[6].Value = keyNumber;
            }
            else
            {
                parameterArray[6].Value = "无";
            }

            parameterArray[7] = new SqlParameter("@OperateTime", SqlDbType.DateTime);
            parameterArray[7].Value = System.DateTime.Now.ToLongTimeString();

            //2009.06.05 添加 王永辉 
            parameterArray[8] = new SqlParameter("@daliren", SqlDbType.VarChar, 50);
            if (dailiren != "")
            {
                parameterArray[8].Value = dailiren;
            }
            else
            {
                parameterArray[8].Value = "无";
            }
            return MakeObj(sqlCmd, parameterArray, CommandType.Text);
        }

        /// <summary>
        /// 返回操作类型
        /// </summary>
        /// <param name="sqlCmd">要执行操作的SQL语句</param>
        private static string GetOperateType(string user,string cmdText,string module,string pageName,string KeyNumber)
        {
            string operStr = string.Empty;

            cmdText = cmdText.ToLower();
            if (cmdText.Contains("insert"))
            {
                operStr = OperText(user, module, pageName, KeyNumber, "insert");
            }
            else if (cmdText.Contains("update"))
            {
                operStr = OperText(user, module, pageName, KeyNumber, "update");
            }

            else if (cmdText.Contains("delete"))
            {
                operStr = OperText(user, module, pageName, KeyNumber, "delete");
            }

            // 2009.06.08 王永辉 添加审核日志记录
            else if (cmdText.Contains("docheck"))
            {
                operStr = OperText(user, module, pageName, KeyNumber, "Check");
            }
            else
            {
                operStr = OperText(user, module, pageName, KeyNumber, "select");
            }
            return operStr;
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

        /// <summary>
        /// 返回操作内容
        /// </summary>
        /// <returns></returns>
        private static string OperText(string user,string model,string pageName,string KeyNumber,string type)
        {
            string operText = "用户:" + user;
            if (pageName != "")
            {
                operText += "在" + pageName + "页面\r\n";
            }

            switch (type)
            {
                case "insert":
                    operText += "执行:新增操作\r\n";
                    break;
                case "update":
                    operText += "执行:修改操作\r\n";
                    break;
                case "delete":
                    operText += "执行:删除操作\r\n";
                    break;
                case "select":
                    operText += "执行查询操作\r\n";
                    break;
                // 2009.06.08 王永辉添加 审核日志记录
                case "Check":
                    operText += "执行审核操作\r\n";
                    break;
                default :
                    break;

            }

            if (model != "")
            {
                operText += ",模块名称:" + model;
            }

            if (KeyNumber != "")
            {
                operText += ",编号:" + KeyNumber;
            }
            return operText;
        }

        /// <summary>
        /// 返回操作内容
        /// </summary>
        /// <returns></returns>
        private static string OperText(string user, string model, string pageName, string KeyNumber, string type,string cmdText)
        {
            string operText = "用户:" + user;
            if (pageName != "")
            {
                operText += "在" + pageName + "页面\r\n";
            }

            if (type == "存储过程")
            {
                operText += "执行存储过程[" + cmdText + "]的操作\r\n";
            }

            if (model != "")
            {
                operText += "模块名称:" + model;
            }

            if (KeyNumber != "")
            {
                operText += ",单号:" + KeyNumber;
            }
            return operText;
        }
    }
}
