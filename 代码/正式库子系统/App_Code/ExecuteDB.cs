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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FMOP.SD;
using FMOP.DB;
using FMOP.XParse;
/// <summary>
/// SubmitDB 的摘要说明
/// </summary>
namespace FMOP.Execute
{
    public class ExecuteDB
    {
        public static string KeyNumber;
        public static ArrayList IDlist = new ArrayList();
        public static Hashtable saveFile = new Hashtable();
        public static string  userName=string.Empty;    //创建者
        public static string checkRoleName = string.Empty;//审核者
        public static string modeName = string.Empty;
        public static string NumberType = string.Empty;
        public static int minueInt = 0;
        public ExecuteDB()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 新增主表数据的操作
        /// </summary>
        /// <param name="paramArray">要新增的节点列表与值</param>
        public static CommandInfo InsertMaster(SaveDepositary[] paramArray)
        {
            int flag = 2;
            string ParamName = string.Empty;
            string strSql = string.Empty;
            StringBuilder sqlField = new StringBuilder("INSERT INTO " + modeName + "(number,CreateUser,CreateTime,");
            StringBuilder sqlValue = new StringBuilder("Values(@number,@CreateUser,getdate(),");
            SqlParameter[] masterParamArray = null;
            if (checkRoleName != "")
            {
                 masterParamArray = new SqlParameter[paramArray.Length + 3];
            }
            else
            {
                 masterParamArray = new SqlParameter[paramArray.Length + 2];
            }

            //添加参数@number
            masterParamArray[0] = new SqlParameter("@number", SqlDbType.VarChar, 50);
            masterParamArray[0].Value = KeyNumber;

            masterParamArray[1] = new SqlParameter("@CreateUser", SqlDbType.VarChar, 20);
            masterParamArray[1].Value = userName;

            if (checkRoleName != "")
            {
                sqlField.Append("NextChecker,");
                sqlValue.Append("@NextChecker,");
                masterParamArray[flag] = new SqlParameter("@NextChecker", SqlDbType.VarChar, 50);
                masterParamArray[flag].Value = checkRoleName;
                sqlField.Append("CheckState,");
                sqlValue.Append("0,");
                flag = flag + 1;

                //加上一CheckLimitTime
                sqlField.Append("CheckLimitTime,");
                sqlValue.Append("dbo.addMinute(getdate()," + minueInt + "),");

            }
            else
            {
                sqlField.Append("CheckState,");
                sqlValue.Append("1,");
            }

            //循环所有的参数数组,生成SQL 命令
            for (int i = 0; i < paramArray.Length; i++)
            {
                 ParamName = "@" + paramArray[i].Name;


                //生成SQL 语句 --添加要新增的数据库字段名
                if (i != paramArray.Length - 1)
                {
                    sqlField.Append(paramArray[i].Name + ",");
                    sqlValue.Append(ParamName + ",");
                }
                else
                {
                    sqlField.Append(paramArray[i].Name + ")");
                    sqlValue.Append(ParamName + ")");
                }

                //添加参数
                masterParamArray[i + flag] = ChooseFields(paramArray[i]);
            }

            try
            {
                strSql = sqlField.ToString() + sqlValue.ToString();
                return MakeObj(strSql, masterParamArray, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 若接受到空的资料，则新增空记录
        /// </summary>
        /// <returns></returns>
        public static CommandInfo InsertNullMaster()
        {
            string strSql = "INSERT INTO " + modeName + "(number,CreateUser,CreateTime,NextChecker,CheckState)Values(@number,@CreateUser,getdate(),@NextChecker,@CheckState)";
            string strSql2 = "INSERT INTO " + modeName + "(number,CreateUser,CreateTime,NextChecker,CheckState,CheckLimitTime)Values(@number,@CreateUser,getdate(),@NextChecker,@CheckState,dbo.addMinute(getdate()," + minueInt + "))";
            SqlParameter[] masterParamArray = new SqlParameter[4];

            //添加参数@number
            masterParamArray[0] = new SqlParameter("@number", SqlDbType.VarChar, 50);
            masterParamArray[0].Value = KeyNumber;

            masterParamArray[1] = new SqlParameter("@CreateUser", SqlDbType.VarChar, 20);
            masterParamArray[1].Value = userName;

            masterParamArray[2] = new SqlParameter("@NextChecker", SqlDbType.VarChar, 50);
            masterParamArray[2].Value = checkRoleName;

            masterParamArray[3] = new SqlParameter("@CheckState", SqlDbType.SmallInt, 1);

            if (checkRoleName != "")
            {

                masterParamArray[3].Value = 0;
                return MakeObj(strSql, masterParamArray, CommandType.Text);
            }
            else
            {
                masterParamArray[3].Value =1;
                return MakeObj(strSql2, masterParamArray, CommandType.Text);
            }
        }

        /// <summary>
        /// 新增子表数据的操作
        /// </summary>
        /// <param name="paramArray">子表参数对象</param>
        /// <param name="trans">事务</param>
        /// <param name="sqlConn">数据库连接对象</param>
        /// <returns></returns>
        public static CommandInfo[] InsertSubTable(SaveDepositary[] paramArray)
        {
            //要存库的子表名
            string subName = paramArray[0].SubTable;
            StringBuilder sqlCmdField = new StringBuilder("");
            StringBuilder sqlCmdValue = new StringBuilder("");
            string strSql = string.Empty;

            //保存控件的属性
            SaveDepositary paramers = new SaveDepositary();
            //统计子表控件值的个数，比对各个控件要新增的次数
            int Total = 0;

            CommandInfo[] subCommandInfo = null;
            try
            {
                //判断所获取的各个子表控件的列表个数是否相同
                for (int i = 0; i < paramArray.Length; i++)
                {
                    Total = paramArray[0].Text.Split(',').Length;

                    //若个数一样，则向下执行
                    if (Total == paramArray[i].Text.Split(',').Length)
                    {
                        Total = paramArray[i].Text.Split(',').Length;
                    }
                    else
                    {
                        throw new Exception("新增操作,子表个数不同");
                    }
                }

                //创建CommandInfo 对象数组
                subCommandInfo = new CommandInfo[paramArray[0].Text.Split(',').Length];

                //循环所有控件并根据控件产生SQL语句
                for (int rowIndex = 0; rowIndex < paramArray[0].Text.Split(',').Length; rowIndex++)
                {
                    sqlCmdField = new StringBuilder("insert into " + subName + "(ParentNumber,");
                    sqlCmdValue = new StringBuilder("values(@ParentNumber,");

                    SqlParameter[] subParamArray = new SqlParameter[paramArray.Length+1];

                    subParamArray[0] = new SqlParameter("@ParentNumber", SqlDbType.VarChar, 20);
                    subParamArray[0].Value = KeyNumber;

                    //根据控件个数产生SQL语句
                    for (int i = 0; i < paramArray.Length; i++)
                    {
                        //产生一个存放参数的对象
                        paramers = new SaveDepositary();
                        //存放名称
                        paramers.Name = paramArray[i].Name;

                        //存放长度 
                        paramers.Length = paramArray[i].Length;

                        //存放内容
                        paramers.Text = paramArray[i].Text.Split(',')[rowIndex].ToString();

                        //存放类型
                        paramers.Type = paramArray[i].Type;

                        if (i != paramArray.Length - 1)
                        {
                            sqlCmdField.Append(paramArray[i].Name + ",");
                            sqlCmdValue.Append("@" + paramArray[i].Name + ",");
                        }
                        else
                        {
                            sqlCmdField.Append(paramArray[i].Name + ")");
                            sqlCmdValue.Append("@" + paramArray[i].Name + ")");
                        }

                        //添加参数
                        subParamArray[i+1] = ChooseFields(paramers);
                    }

                    //组成SQL语句
                    strSql = sqlCmdField.ToString() + sqlCmdValue.ToString();

                    //返回List<CommandInfo>对象
                    subCommandInfo[rowIndex] = MakeObj(strSql, subParamArray, CommandType.Text);
                }
                return subCommandInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存上传控件入库的操作
        /// </summary>
        /// <param name="paramArray"></param>
        /// <param name="trans"></param>
        /// <param name="sqlConn"></param>
        /// <param name="KeyNumber"></param>
        /// <param name="files"></param>
        public static CommandInfo[] InsertFileUp(SaveDepositary[] paramArray, HttpFileCollection files)
        {
            int ID = 0;
            CommandInfo[] fileInfo = null;
            string strSql = "INSERT INTO FileUp(Number,SourceName,LocalName,ModuleName)Values(@Number,@SourceName,@LocalName,@ModuleName)";
            try
            {
                //产生的ID 
                ID = GetLocalName();
                fileInfo = new CommandInfo[files.Count];
                saveFile.Clear();
                for (int index = 0; index < paramArray.Length; index++)
                {
                    //如果控件类型为上传控件 ，则进行上传
                    for (int filesIndex = 0; filesIndex < files.Count; filesIndex++)
                    {
                        SqlParameter[] FileParamter = new SqlParameter[4]; 
                        HttpPostedFile postedFile = files[filesIndex];

                        if (postedFile.FileName != "")
                        {
                            FileParamter[0] = new SqlParameter("@Number", SqlDbType.NVarChar, 20);
                            FileParamter[0].Value = KeyNumber;

                            FileParamter[1] = new SqlParameter("@SourceName", SqlDbType.NVarChar, 100);
                            FileParamter[1].Value = System.IO.Path.GetFileName(postedFile.FileName);

                            FileParamter[2] = new SqlParameter("@LocalName", SqlDbType.NVarChar, 100);
                            FileParamter[2].Value = ID.ToString() + '_'+KeyNumber.ToString ()+ System.IO.Path.GetExtension(postedFile.FileName);

                            FileParamter[3] = new SqlParameter("@ModuleName", SqlDbType.NVarChar, 20);
                            FileParamter[3].Value = modeName;

                            //执行存库
                            fileInfo[filesIndex] = MakeObj(strSql, FileParamter, CommandType.Text);
                            IDlist.Add(ID.ToString() + System.IO.Path.GetExtension(postedFile.FileName));
                            saveFile.Add(ID.ToString() + '_' + KeyNumber.ToString() + System.IO.Path.GetExtension(postedFile.FileName), postedFile);
                            //根据存库后的ID 再加1
                            ID = ID + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileInfo;
        }

        /// <summary>
        /// 修改主表数据的操作
        /// </summary>
        /// <param name="paramArray">要新增的节点列表与值</param>
        public static CommandInfo UpdateMaster(SaveDepositary[] paramArray)
        {
            int flag = 2;
            string ParamName = string.Empty;
            try
            {
                SqlParameter[] updateMasterParameter = null;
                StringBuilder sqlCmd = null;
                //galaxy修改，禁用修改创建人
                if (!modeName.Equals("BB_HTXQTBB") && 1==2)
                {
                    updateMasterParameter = new SqlParameter[paramArray.Length + 3];

                    sqlCmd = new StringBuilder("UPDATE " + modeName + " SET CreateUser=@CreateUser,CreateTime=CreateTime,");
                    updateMasterParameter[0] = new SqlParameter("@number", SqlDbType.VarChar, 20);
                    updateMasterParameter[0].Value = KeyNumber;
                    updateMasterParameter[1] = new SqlParameter("@CreateUser", SqlDbType.VarChar, 20);
                    updateMasterParameter[1].Value = userName;


                    if (checkRoleName != "")
                    {
                        flag = 2;
                        sqlCmd.Append("NextChecker=@NextChecker,CheckState=0,CheckLimitTime=dbo.addMinute(getdate()," +
                                      minueInt + "),");
                        updateMasterParameter[2] = new SqlParameter("@NextChecker", SqlDbType.VarChar, 50);
                        updateMasterParameter[2].Value = checkRoleName;
                        flag = flag + 1;
                    }
                    else
                    {
                        flag = 2;
                        sqlCmd.Append("NextChecker=@NextChecker,CheckState=1,");
                        updateMasterParameter[2] = new SqlParameter("@NextChecker", SqlDbType.VarChar, 50);
                        updateMasterParameter[2].Value = "";
                        flag = flag + 1;
                    }
                }
                else
                {
                    updateMasterParameter = new SqlParameter[paramArray.Length + 2];
                    sqlCmd = new StringBuilder("UPDATE " + modeName + " SET CreateTime=CreateTime,");
                    updateMasterParameter[0] = new SqlParameter("@number", SqlDbType.VarChar, 20);
                    updateMasterParameter[0].Value = KeyNumber;
                    if (checkRoleName != "")
                    {
                        flag = 1;
                        sqlCmd.Append("NextChecker=@NextChecker,CheckState=0,CheckLimitTime=dbo.addMinute(getdate()," +
                                      minueInt + "),");
                        updateMasterParameter[1] = new SqlParameter("@NextChecker", SqlDbType.VarChar, 50);
                        updateMasterParameter[1].Value = checkRoleName;
                        flag = flag + 1;
                    }
                    else
                    {
                        flag = 1;
                        sqlCmd.Append("NextChecker=@NextChecker,CheckState=1,");
                        updateMasterParameter[1] = new SqlParameter("@NextChecker", SqlDbType.VarChar, 50);
                        updateMasterParameter[1].Value = "";
                        flag = flag + 1;
                    }
                }

                //循环所有的参数数组,生成SQL 命令
                for (int i = 0; i < paramArray.Length; i++)
                {
                    ParamName = "@" + paramArray[i].Name;

                    //生成SQL 语句 --添加要新增的数据库字段名
                    if (i != paramArray.Length - 1)
                    {
                        sqlCmd.Append(paramArray[i].Name + "= @" + paramArray[i].Name + ",");
                    }
                    else
                    {
                        sqlCmd.Append(paramArray[i].Name + "= @" + paramArray[i].Name + " WHERE Number=@number");
                    }
                    //添加参数
                    updateMasterParameter[i + flag] = ChooseFields(paramArray[i]);
                } 
                 return  MakeObj(sqlCmd.ToString(), updateMasterParameter,CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 删除子表数据的操作
        /// </summary>
        /// <param name="paramArray">子表参数对象</param>
        /// <param name="trans">事务</param>
        /// <param name="sqlConn">数据库连接对象</param>
        /// <returns></returns>
        public static CommandInfo DeleteSubTable(SaveDepositary[] paramArray)
        {
            //要存库的子表名
            string subName = paramArray[0].SubTable;
            string strSql = string.Empty;
            SqlParameter[]  deleteParam = new SqlParameter[1];

            deleteParam[0] = new SqlParameter("@parentNumber", SqlDbType.VarChar, 20);
            deleteParam[0].Value = KeyNumber;
            strSql = "Delete FROM " + subName + " WHERE parentNumber=@parentNumber";

            //执行存库操作
            try
            {
                return MakeObj(strSql, deleteParam, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="sp_Type">执行存储过程的类型</param>
        /// <param name="trans">事务</param>
        /// <param name="sqlConn">数据库连接对象</param>
        /// 0:增加一条信息后执行的存储过程
        /// 1:删除一条信息后执行的存储过程
        /// 2:修改一条信息后执行的存储过程
        /// <returns></returns>
        public static CommandInfo SP_OnExcute(int sp_Type)
        {
            //存储过程名称
            string sp_Name = string.Empty;
            string sqlCmd = string.Empty;
            string strXml = string.Empty;
            XmlNode resultNode = null;
            CommandInfo spInfo = null;
            try
            {   sqlCmd = "SELECT Configuration.query('/WorkFlowModule') from system_Modules where name ='"+ modeName +"'";
                strXml = DbHelperSQL.GetSingle(sqlCmd).ToString();

                switch (sp_Type)
                {
                    case 0:
                        resultNode = xmlParse.XmlFirstNode(strXml, "//SP_OnAdd");
                        if (resultNode != null)
                        {
                            sp_Name = xmlParse.XmlFirstNode(strXml, "//SP_OnAdd").InnerXml;
                        }
                        break;
                    case 1:
                        resultNode = xmlParse.XmlFirstNode(strXml, "//SP_OnDelete");
                        if (resultNode != null)
                        {
                            sp_Name = xmlParse.XmlFirstNode(strXml, "//SP_OnDelete").InnerXml;
                        }
                        break;
                    case 2:
                        resultNode = xmlParse.XmlFirstNode(strXml, "//SP_OnModify");
                        if (resultNode != null)
                        {
                            sp_Name = xmlParse.XmlFirstNode(strXml, "//SP_OnModify").InnerXml;
                        }
                        break;
                }
                //若找到存储过程，则执行
                if (sp_Name != "")
                {
                    SqlParameter[] spParameter = new SqlParameter[1];
                    spParameter[0] = new SqlParameter("@Number", SqlDbType.VarChar, 20, "Number");
                    spParameter[0].Value = KeyNumber;
                    spParameter[0].Direction = ParameterDirection.Input;
                    spInfo = MakeObj(sp_Name, spParameter,CommandType.StoredProcedure);
                }
                return spInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据对象类型，添加相应类型的参数 
        /// </summary>
        /// <param name="obj">添加的参数对象</param>
        /// <returns></returns>
        private static SqlParameter ChooseFields(SaveDepositary obj)
        {
            SqlParameter parameter = null;
            string ParamName = "@" + obj.Name;
            int Length = obj.Length;
            string ParamValue = obj.Text;
            switch (obj.Type)
            {
                case "TextBox":
                case "DropDownList":
                    parameter = new SqlParameter(ParamName, SqlDbType.VarChar, Length);
                  //  parameter = SetValue(parameter,ParamValue);
                    parameter.Value = ParamValue;
                    break;
                case "MutiLineTextBox":
                    parameter = new SqlParameter(ParamName, SqlDbType.Text, 4000);
                    parameter.Value = ParamValue;
                    break;
                case "DateSelectBox":
                    parameter = new SqlParameter(ParamName, SqlDbType.DateTime);
                    parameter = SetValue(parameter,ParamValue);
                    break;
                case "FloatBox":
                    parameter = new SqlParameter(ParamName, SqlDbType.Float);
                    parameter = SetValue(parameter,ParamValue);
                    break;
                case "IntBox":
                    parameter = new SqlParameter(ParamName, SqlDbType.Int);
                    parameter = SetValue(parameter,ParamValue);
                    break;
                    /*2010 1.06 王永辉添加 --框架添加复选框控件*/
                case "CheckBoxs":
                    parameter = new SqlParameter(ParamName, SqlDbType.VarChar, Length);
                    parameter = SetValue(parameter, ParamValue);
                    break;
                default :
                    break;
            }
            return parameter;
        }
        
        /// <summary>
        /// 给SqlParameter 参数赋值
        /// </summary>
        /// <param name="parameter">Sql参数</param>
        /// <param name="Value">参数值,若为空则为DBNull.value</param>
        private static SqlParameter SetValue(SqlParameter parameter, string Value)
        {
            if(Value == "")
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = Value;
            }
            return parameter ;
        }

        /// <summary>
        /// 取主表存库后主鍵
        /// </summary>
        /// <param name="KeyNumber">单号</param>
        /// <returns></returns>
        public static int GetLocalName()
        {
            DataSet result = new DataSet();
            int ID = 1;
            string SqlCmd = "";

            SqlCmd = "select max(ID) as ID from FileUp";
            try
            {
                result = DbHelperSQL.Query(SqlCmd);
                if (result != null && result.Tables[0].Rows.Count > 0)
                {
                    if (result.Tables[0].Rows[0]["ID"].ToString() != "")
                    {
                        ID = Convert.ToInt32(result.Tables[0].Rows[0]["ID"].ToString()) + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ID;
        }

        /// <summary>
        /// 查询上传文件
        /// </summary>
        /// <returns></returns>
        public static DataSet GetUpFiles()
        {
            DataSet result = null;
            string SqlCmd = " SELECT  * FROM FileUp  WHERE Number='" + KeyNumber + "'";
            result = DbHelperSQL.Query(SqlCmd);
            return result;
        }

        /// <summary>
        /// 根据单号查询主表记录
        /// </summary>
        /// <returns></returns>
        public static DataSet GetSaleOrdersRecordset()
        {
            string sqlCmd = string.Empty;
            DataSet result = null;

            sqlCmd ="SELECT * FROM " +modeName +" WHERE Number='" + KeyNumber +"'";
            result = DbHelperSQL.Query(sqlCmd);
            return result;
        }

        /// <summary>
        /// 返回子表的记录
        /// </summary>
        /// <param name="subTable">子表名称</param>
        /// <returns></returns>
        public static DataSet GetSubRecordset(string subTable)
        {
            string sqlCmd = string.Empty;
            DataSet result = null;
            sqlCmd = "select * from " + subTable + " where parentNumber='" + KeyNumber + "'";
            result = DbHelperSQL.Query(sqlCmd);
            return result;
        }

        /// <summary>
        /// 查询模块的中文名称
        /// </summary>
        /// <param name="module">模块的英文名称</param>
        /// <returns></returns>
        public static string GetModuleName(string module)
        {
            string Name = "";
            SqlCommand comm = new SqlCommand("SELECT Title FROM system_Modules where name=@name");
            comm.Parameters.Add("@name",SqlDbType.VarChar,100).Value = module;
            Name = DbHelperSQL.QueryName(comm);
            return Name;

        }

        /// <summary>
        /// 创建泛型对象
        /// </summary>
        /// <param name="strSql">属性:SQL 命令</param>
        /// <param name="parameterArray">属性: SQL参数</param>
        /// <returns></returns>
        private static CommandInfo MakeObj(string strSql, SqlParameter[] parameterArray)
        {
            CommandInfo commInfo = new CommandInfo();
            commInfo.CommandText = strSql;
            commInfo.Parameters = parameterArray;
            return commInfo;
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
