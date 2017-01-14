using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Collections;
using FMOP.DB;
using System.IO;
using System.Text;

/// <summary>
/// SocketListener 的摘要说明
/// </summary>
public class SocketListener
{
	public SocketListener()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 比较用户信息，并执行操作
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public DataSet CompareUserInfo(DataSet ds,DataSet dsreturn)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        DataTable dtUserInfo = ds.Tables["UINFO"];
        try
        {
            if (dtUserInfo.Rows.Count > 0)
            {
                string CardType = dtUserInfo.Rows[0]["CardType"].ToString().Trim();//证件类型
                string ZQZJMM=CallPinMac.DecryptString(dtUserInfo.Rows[0]["sCustPwd2"].ToString().Trim());//证件资金密码
                string sql = "";
                string strMM="";
                if (CardType == "1")
                {
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_ZQZJZH='" + dtUserInfo.Rows[0]["sCustAcct"].ToString() + "' AND I_SFZH='" + dtUserInfo.Rows[0]["Card"].ToString() + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_ZQZJZH='" + dtUserInfo.Rows[0]["sCustAcct"].ToString() + "' AND I_SFZH='" + dtUserInfo.Rows[0]["Card"].ToString() + "'";
                    #region//验证证券资金密码
                    if (DbHelperSQL.Query(strMM).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }

                    #endregion
                }
                else if (CardType == "8")
                {
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_ZQZJZH='" + dtUserInfo.Rows[0]["sCustAcct"].ToString() + "' AND I_ZZJGDMZDM='" + dtUserInfo.Rows[0]["Card"].ToString() + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_ZQZJZH='" + dtUserInfo.Rows[0]["sCustAcct"].ToString() + "' AND I_ZZJGDMZDM='" + dtUserInfo.Rows[0]["Card"].ToString() + "'";
                    #region//验证证券资金密码
                    if (DbHelperSQL.Query(strMM).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }

                    #endregion
                }
                else if (CardType == "9")
                {
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' I_ZQZJZH='" + dtUserInfo.Rows[0]["sCustAcct"].ToString() + "' AND I_YYZZZCH='" + dtUserInfo.Rows[0]["Card"].ToString() + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_ZQZJZH='" + dtUserInfo.Rows[0]["sCustAcct"].ToString() + "' AND I_YYZZZCH='" + dtUserInfo.Rows[0]["Card"].ToString() + "'";
                    #region//验证证券资金密码
                    if (DbHelperSQL.Query(strMM).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }

                    #endregion
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证件类型无效";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                    return dsreturn;
                }

                

                DataTable dtUInPT = DbHelperSQL.Query(sql).Tables[0];
                if (dtUInPT.Rows.Count == 1)
                {
                    if (dtUInPT.Rows[0]["S_SFYBJJRSHTG"].ToString() == "否" || dtUInPT.Rows[0]["S_SFYBFGSSHTG"].ToString() == "否")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该帐户未在证券方审核通过";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                        return dsreturn;
                    }

                    string sqlUpdate = "UPDATE AAA_DLZHXXB SET I_YHZH='" + dtUserInfo.Rows[0]["bCustAcct"].ToString() + "' , I_DSFCGZT='开通', I_JYFMC='" + dtUserInfo.Rows[0]["Name"].ToString() + "' WHERE Number='" + dtUInPT.Rows[0]["Number"].ToString() + "'";
                    #region 撰写日志
                    try
                    {
                        string serverDir = HttpContext.Current.Server.MapPath("~/Log_BankService_GT/" + DateTime.Now.ToString("yyyy-MM-dd") + "/25702/");
                        if (!IsExistDirectory(serverDir))
                            Directory.CreateDirectory(serverDir);
                        string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                        if (!IsExistFile(serverFile))
                        {
                            FileStream f = File.Create(serverFile);
                            f.Close();
                            f.Dispose();
                        }
                        using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                        {
                            sw.WriteLine("查询语句：" + sql + "   |   更新语句：" + sqlUpdate);
                        }
                    }
                    catch(Exception ex)
                    {
                        string serverDir = HttpContext.Current.Server.MapPath("~/Log_BankService_GT/" + DateTime.Now.ToString("yyyy-MM-dd") + "/");
                        if (!IsExistDirectory(serverDir))
                            Directory.CreateDirectory(serverDir);
                        string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                        if (!IsExistFile(serverFile))
                        {
                            FileStream f = File.Create(serverFile);
                            f.Close();
                            f.Dispose();
                        }
                        using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                        {
                            sw.WriteLine("查询语句：" + sql + "   |   更新语句：" + sqlUpdate+"   |   异常信息："+ex.Message);
                        }
                    }
                    #endregion
                    if (DbHelperSQL.ExecuteSql(sqlUpdate) > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "验证通过";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "0000";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = dtUInPT.Rows[0]["Number"].ToString();
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券平台发生错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                    }
                }
                else if (dtUInPT.Rows.Count > 1)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "多帐号重复，请通知联系证券方进行处理";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2105";
                }
                else if (dtUInPT.Rows.Count == 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未在证券方数据库中找到有效信息";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "验证信息丢失";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2405";
            }
        }
        catch (Exception ex)
        {
            /*
             书写异常日志
             */
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券平台错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        }
        return dsreturn;
    }

    public DataSet GetUserMoney(DataSet ds, DataSet dsreturn)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        DataTable dt = ds.Tables["UINFO"];
        try
        {
            if (dt.Rows.Count > 0)
            {
                string ZQZJZH = dt.Rows[0]["sCustAcct"].ToString();//证券资金帐号
                string CardType = dt.Rows[0]["CardType"].ToString();//证件类型
                string Card = dt.Rows[0]["Card"].ToString();//证件号
                string FunctionId = dt.Rows[0]["FunctionId"].ToString();
                string ExSerial = dt.Rows[0]["ExSerial"].ToString();
                string sCustAcct = dt.Rows[0]["sCustAcct"].ToString();
                string MoneyType = dt.Rows[0]["MoneyType"].ToString();
                string ZQZJMM = CallPinMac.DecryptString(dt.Rows[0]["sCustPwd2"].ToString());//证券资金密码
                string sql = "";
                string strMM = "";
                if (CardType.ToString().Trim() == "1")
                {
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_ZQZJZH='" + ZQZJZH + "' AND I_SFZH='" + Card + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_ZQZJZH='" + ZQZJZH + "' AND I_SFZH='" + Card + "'";
                    #region//验证证券资金密码
                    if (DbHelperSQL.Query(strMM).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }

                    #endregion
                }
                else if (CardType.ToString().Trim() == "9")
                {
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_ZQZJZH='" + ZQZJZH + "' AND I_YYZZZCH='" + Card + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_ZQZJZH='" + ZQZJZH + "' AND I_YYZZZCH='" + Card + "'";
                    #region//验证证券资金密码
                    if (DbHelperSQL.Query(strMM).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }

                    #endregion
                }
                else if (CardType.ToString().Trim() == "8")
                {
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_ZQZJZH='" + ZQZJZH + "' AND I_ZZJGDMZDM='" + Card + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_ZQZJZH='" + ZQZJZH + "' AND I_ZZJGDMZDM='" + Card + "'";
                    #region//验证证券资金密码
                    if (DbHelperSQL.Query(strMM).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }

                    #endregion
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证件类型无效";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                    return dsreturn;
                }
                DataTable dtUInfo = DbHelperSQL.Query(sql).Tables[0];

                if (dtUInfo.Rows.Count == 1)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "0000";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = dtUInfo.Rows[0]["Number"].ToString();
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"] = dtUInfo.Rows[0]["B_ZHDQKYYE"].ToString();
                }
                else if (dtUInfo.Rows.Count > 1)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "多帐号重复，请通知联系证券方进行处理";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2105";
                }
                else if (dtUInfo.Rows.Count == 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未在证券方数据库中找到有效信息";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                }


                #region 撰写日志
                try
                {
                    string serverDir = HttpContext.Current.Server.MapPath("~/Log_BankService_GT/" + DateTime.Now.ToString("yyyy-MM-dd") + "/23603/");
                    if (!IsExistDirectory(serverDir))
                        Directory.CreateDirectory(serverDir);
                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                    if (!IsExistFile(serverFile))
                    {
                        FileStream f = File.Create(serverFile);
                        f.Close();
                        f.Dispose();
                    }
                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                    {
                        sw.WriteLine("查询语句：" + sql);
                    }
                }
                catch (Exception ex)
                {
                    string serverDir = HttpContext.Current.Server.MapPath("~/Log_BankService_GT/" + DateTime.Now.ToString("yyyy-MM-dd") + "/");
                    if (!IsExistDirectory(serverDir))
                        Directory.CreateDirectory(serverDir);
                    string serverFile = serverDir + DateTime.Now.ToString("HH：mm：ss-fff") + ".txt";
                    if (!IsExistFile(serverFile))
                    {
                        FileStream f = File.Create(serverFile);
                        f.Close();
                        f.Dispose();
                    }
                    using (StreamWriter sw = new StreamWriter(serverFile, true, System.Text.Encoding.UTF8))
                    {
                        sw.WriteLine("查询语句：" + sql + "   |    异常信息：" + ex.Message);
                    }
                }
                #endregion

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "验证信息丢失";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2405";
            }

        }
        catch(Exception ex)
        {
            /*
            书写异常日志
            */
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券平台错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        }
        return dsreturn;
    }

    /// <summary>
    /// 检测指定文件是否存在,如果存在则返回true。
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>        
    public  bool IsExistFile(string filePath)
    {
        return File.Exists(filePath);
    }
    /// <summary>
    /// 检测指定目录是否存在
    /// </summary>
    /// <param name="directoryPath">目录的绝对路径</param>
    /// <returns></returns>
    public  bool IsExistDirectory(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }

}