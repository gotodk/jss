using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;

/// <summary>
/// BankRequest 的摘要说明
/// </summary>
public class BankRequest
{
	public BankRequest()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    //银转商23701
    public DataSet BankToS(DataSet ds, DataSet dsreturn)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        DataTable dt = ds.Tables["UINFO"];
        try
        {
            if (dt.Rows.Count > 0)
            {
                string YHLX = dt.Rows[0]["bOrganId"].ToString();//银行类型
                string YHZH = dt.Rows[0]["bCustAcct"].ToString();//银行账号
                string Card = dt.Rows[0]["Card"].ToString();//客户证件号
                string CardType = dt.Rows[0]["CardType"].ToString();//客户证件类型
                string ZQZH = dt.Rows[0]["sCustAcct"].ToString();//证券资金账号
                //string ZQZJMM = CallPinMac.DecryptString(dt.Rows[0]["sCustPwd2"].ToString());//证券资金密码
                string BankLSH = dt.Rows[0]["ExSerial"].ToString();//外部流水号
                double YDje = Convert.ToDouble(dt.Rows[0]["OccurBala"].ToString());//转帐金额

                string sql = "";
                //string strMM = "";
                if (CardType.ToString().Trim() == "1")
                {
                    //strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_SFZH='" + Card + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_SFZH='" + Card + "'";
                    #region//验证证券资金密码
                    //if (DbHelperSQL.Query(strMM).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count > 0)
                    //{
                    //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                    //    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                    //    return dsreturn;
                    //}

                    #endregion
                }
                else if (CardType.ToString().Trim() == "9")
                {
                    //strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_YYZZZCH='" + Card + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_YYZZZCH='" + Card + "'";
                    #region//验证证券资金密码
                    //if (DbHelperSQL.Query(strMM).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count > 0)
                    //{
                    //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                    //    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                    //    return dsreturn;
                    //}

                    #endregion
                }
                else if (CardType.ToString().Trim() == "8")
                {
                    //strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_ZZJGDMZDM='" + Card + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_ZZJGDMZDM='" + Card + "'";
                    #region//验证证券资金密码
                    //if (DbHelperSQL.Query(strMM).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count > 0)
                    //{
                    //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                    //    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                    //    return dsreturn;
                    //}

                    #endregion
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证件类型无效";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                    return dsreturn;
                }
                DataSet dsDLB = DbHelperSQL.Query(sql);

                if (dsDLB != null)
                {
                    if (dsDLB.Tables[0].Rows.Count == 1)
                    {
                        string dlyx = dsDLB.Tables[0].Rows[0]["B_DLYX"].ToString();//登录邮箱
                        string JSZHLX = dsDLB.Tables[0].Rows[0]["B_JSZHLX"].ToString();//结算账户类型
                        string JSBH = dsDLB.Tables[0].Rows[0]["J_BUYJSBH"].ToString();//买家角色编号
                        double SYJE = Convert.ToDouble(dsDLB.Tables[0].Rows[0]["B_ZHDQKYYE"].ToString());//剩余可用金额
                        string time = DateTime.Now.ToString();//


                        //资金变动明细对照表
                        DataSet dsDZB = DbHelperSQL.Query("select * from AAA_moneyDZB where Number='1304000001'");
                        //生成帐款流水明细表的Number
                        WorkFlowModule WFM = new WorkFlowModule("AAA_ZKLSMXB");
                        string key = WFM.numberFormat.GetNextNumberZZ("");

                        List<string> list = new List<string>();

                        #region//写入资金流水明细、更改登录表可用余额
                        //写入资金流水明细
                        DataRow[] rows = dsDZB.Tables[0].Select("Number='1304000001'");
                        string xm = rows[0]["XM"].ToString().Trim();
                        string xz = rows[0]["XZ"].ToString().Trim();
                        string zy = rows[0]["ZY"].ToString().Trim().Replace("[x1]", BankLSH);
                        string sjlx = rows[0]["SJLX"].ToString().Trim();
                        string yslx = rows[0]["YSLX"].ToString().Trim();
                        string BDLSH = BankHelper.GetLSH();//生成本地流水号
                        if (BDLSH==null)
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券放服务器繁忙";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                            return dsreturn;
                        }
                        string str = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],BDCJRJLSH,[CheckState],[CreateUser],[CreateTime]) VALUES ('" + key + "','" + dlyx + "','" + JSZHLX + "','" + JSBH + "','银行入金','" + BankLSH + "','" + time + "','" + yslx + "'," + YDje + ",'" + xm + "','" + xz + "','" + zy + "','','" + sjlx + "','" + BDLSH + "',0,'银行入金监控','" + time + "')";
                        list.Add(str);
                        //更改登录表可用余额
                        str = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE +" + YDje + ",B_DSFCGKYYE=B_DSFCGKYYE+" + YDje + " where B_DLYX='" + dlyx + "'";
                        list.Add(str);
                        #endregion

                        int i = DbHelperSQL.ExecuteSqlTran(list);
                        if (i > 0)
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您成功转账" + YDje.ToString("#0.00") + "元!";//返回说明
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "0000";//返回码
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = key;//证券流水号
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"] = (SYJE + YDje).ToString("#0.00");  //证券可取金额
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败!";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                        }
                    }
                    else if (dsDLB.Tables[0].Rows.Count > 1)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "多帐号重复，请通知联系证券方进行处理";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2105";
                    }
                    else if (dsDLB.Tables[0].Rows.Count ==0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未在证券方数据库中找到有效信息";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                    }
                    

                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未在证券方数据库中找到有效信息";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                }


            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "连接证券失败";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2405";
            }
        }
        catch(Exception e)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券平台错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        }
        return dsreturn;
    }
    //商转银23702
    public DataSet SToBank(DataSet ds, DataSet dsreturn)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        DataTable dt = ds.Tables["UINFO"];
        try
        {
            if (dt.Rows.Count > 0)
            {
                string YHLX = dt.Rows[0]["bOrganId"].ToString();//银行类型
                string YHZH = dt.Rows[0]["bCustAcct"].ToString();//银行账号
                string Card = dt.Rows[0]["Card"].ToString();//客户证件号
                string CardType = dt.Rows[0]["CardType"].ToString();//客户证件类型
                string ZQZH = dt.Rows[0]["sCustAcct"].ToString();//证券资金账号
                string ZQZJMM = CallPinMac.DecryptString(dt.Rows[0]["sCustPwd2"].ToString());
                //string ZQZJMM = dt.Rows[0]["sCustPwd2"].ToString();//证券资金密码
                string BankLSH = dt.Rows[0]["ExSerial"].ToString();//外部流水号
                double YDje = Convert.ToDouble(dt.Rows[0]["OccurBala"].ToString());//转帐金额

                string sql = "";
                if (CardType.ToString().Trim() == "1")
                {
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_SFZH='" + Card + "'";
                    string PwdAction = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_SFZH='" + Card + "'";//不验证密码
                    if (DbHelperSQL.Query(PwdAction).Tables[0].Rows.Count > 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count == 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }

                }
                else if (CardType.ToString().Trim() == "9")
                {
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_YYZZZCH='" + Card + "'";
                    string PwdAction = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_YYZZZCH='" + Card + "'";
                    if (DbHelperSQL.Query(PwdAction).Tables[0].Rows.Count > 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count == 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }

                }
                else if (CardType.ToString().Trim() == "8")
                {
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_ZZJGDMZDM='" + Card + "'";
                    string PwdAction = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_ZZJGDMZDM='" + Card + "'";
                    if (DbHelperSQL.Query(PwdAction).Tables[0].Rows.Count > 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count == 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证件类型无效";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                    return dsreturn;
                }
                DataSet dsDLB = DbHelperSQL.Query(sql);

                if (dsDLB != null)
                {
                    if (dsDLB.Tables[0].Rows.Count == 1)
                    {
                        if (dsDLB.Tables[0].Rows[0]["B_SFDJ"].ToString() == "是")
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您在证券方的账号已被冻结";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                            return dsreturn;
                        }
                                
                        string dlyx = dsDLB.Tables[0].Rows[0]["B_DLYX"].ToString();//登录邮箱
                        string JSZHLX = dsDLB.Tables[0].Rows[0]["B_JSZHLX"].ToString();//结算账户类型
                        string JSBH = dsDLB.Tables[0].Rows[0]["J_BUYJSBH"].ToString();//买家角色编号
                        double SYJE = Convert.ToDouble(dsDLB.Tables[0].Rows[0]["B_ZHDQKYYE"].ToString());//本地剩余可用金额
                        double DSFCGKYYE = Convert.ToDouble(dsDLB.Tables[0].Rows[0]["B_DSFCGKYYE"].ToString());//第三方存管可用余额
                        string time = DateTime.Now.ToString("yyyyMMdd");//用于请求
                        string BDTime = DateTime.Now.ToString();//用于向本地数据库写SQL时

                        #region//取当前账户可用余额 和 第三方存管可用余额 中最小的一个为可转余额
                        double KZYE = 0.00;//可转余额
                        if (SYJE <= DSFCGKYYE)
                        {
                            KZYE = SYJE;
                        }
                        else
                        {
                            KZYE = DSFCGKYYE;
                        }
                        #endregion

                        //资金变动明细对照表
                        DataSet dsDZB = DbHelperSQL.Query("select * from AAA_moneyDZB where Number='1304000002'");
                        //生成帐款流水明细表的Number
                        WorkFlowModule WFM = new WorkFlowModule("AAA_ZKLSMXB");
                        string key = WFM.numberFormat.GetNextNumberZZ("");

                        if (KZYE >= YDje)
                        {
                            List<string> list = new List<string>();

                            #region//写入资金流水明细、更改登录表可用余额
                            //写入资金流水明细
                            DataRow[] rows = dsDZB.Tables[0].Select("Number='1304000002'");
                            string xm = rows[0]["XM"].ToString().Trim();
                            string xz = rows[0]["XZ"].ToString().Trim();
                            string zy = rows[0]["ZY"].ToString().Trim().Replace("[x1]", BankLSH);
                            string sjlx = rows[0]["SJLX"].ToString().Trim();
                            string yslx = rows[0]["YSLX"].ToString().Trim();
                            string BDLSH = BankHelper.GetLSH();//生成本地流水号
                            if (BDLSH == null)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券放服务器繁忙";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                                return dsreturn;
                            }
                            string str = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],BDCJRJLSH,[CheckState],[CreateUser],[CreateTime]) VALUES ('" + key + "','" + dlyx + "','" + JSZHLX + "','" + JSBH + "','银行出金','" + BankLSH + "','" + BDTime + "','" + yslx + "'," + YDje + ",'" + xm + "','" + xz + "','" + zy + "','','" + sjlx + "','" + BDLSH + "',0,'银行出金监控','" + BDTime + "')";
                            list.Add(str);
                            //更改登录表可用余额
                            str = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE -" + YDje + ",B_DSFCGKYYE=B_DSFCGKYYE+" + YDje + " where B_DLYX='" + dlyx + "'";
                            list.Add(str);
                            #endregion

                            int i = DbHelperSQL.ExecuteSqlTran(list);
                            if (i > 0)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您成功转账" + YDje.ToString("#0.00") + "元!";//返回说明
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "0000";//返回码
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = key;//证券流水号
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息3"] = (SYJE - YDje).ToString("#0.00");  //本地证券可取金额
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败!";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                            }
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "余额不足,不能进行转账";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2002";
                        }

                        

                    }
                    else if (dsDLB.Tables[0].Rows.Count > 1)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "多帐号重复，请通知联系证券方进行处理";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2105";
                    }
                    else if (dsDLB.Tables[0].Rows.Count == 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未在证券方数据库中找到有效信息";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未在证券方数据库中找到有效信息";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "2405";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "连接证券失败";
            }
        }
        catch (Exception e)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "8888";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "证券平台错误";
        }
        return dsreturn;
    }
    //冲证券转银行23704
    public DataSet CSToBank(DataSet ds, DataSet dsreturn)
    { 
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        DataTable dt = ds.Tables["UINFO"];
        try
        {
            if (dt.Rows.Count > 0)
            {
                string LYDH = dt.Rows[0]["CorrectSerial"].ToString();//被冲正外部流水号=资金流水明细表来源单号
                string YHLX = dt.Rows[0]["bOrganId"].ToString();//银行类型
                string YHZH = dt.Rows[0]["bCustAcct"].ToString();//银行账号
                string Card = dt.Rows[0]["Card"].ToString();//客户证件号
                string CardType = dt.Rows[0]["CardType"].ToString();//客户证件类型
                string ZQZH = dt.Rows[0]["sCustAcct"].ToString();//证券资金账号
                string ZZJE = dt.Rows[0]["OccurBala"].ToString();//转帐金额

                string str = "select *,a.number '资金流水Number' from AAA_ZKLSMXB a left join AAA_DLZHXXB b on a.DLYX=b.B_DLYX where a.LYDH='" + LYDH + "'";
                DataSet dsZJLSMX = DbHelperSQL.Query(str);
                if (dsZJLSMX != null && dsZJLSMX.Tables[0].Rows.Count > 0)
                {
                    if (dsZJLSMX.Tables[0].Rows.Count == 1)
                    {
                        string ZJLSNumber = dsZJLSMX.Tables[0].Rows[0]["资金流水Number"].ToString();
                        double ZJLSJE=Convert.ToDouble(dsZJLSMX.Tables[0].Rows[0]["JE"].ToString());//对应资金流水明细中的金额
                        string SHZH = dsZJLSMX.Tables[0].Rows[0]["I_SFZH"].ToString();//身份证号
                        string ZZJGDAZ = dsZJLSMX.Tables[0].Rows[0]["I_ZZJGDMZDM"].ToString();//组织机构代码证代码
                        string YYZZ = dsZJLSMX.Tables[0].Rows[0]["I_YYZZZCH"].ToString();//营业执照注册号
                        string BDYHZH = dsZJLSMX.Tables[0].Rows[0]["I_YHZH"].ToString();//银行账号
                        double BDKYYE = Convert.ToDouble(dsZJLSMX.Tables[0].Rows[0]["B_ZHDQKYYE"].ToString());//账户当前可用余额
                        double BDDSFCUKYYE = Convert.ToDouble(dsZJLSMX.Tables[0].Rows[0]["B_DSFCGKYYE"].ToString());//第三方存管可用余额
                        if (Convert.ToDouble(ZZJE) == ZJLSJE)
                        {
                            #region//证件验证
                            if (YHZH!=BDYHZH)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金帐户信息不一致";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                                return dsreturn;
                            }
                            if (CardType == "1")//身份证
                            {
                                if (Card != SHZH)
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金帐户信息不一致";
                                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                                    return dsreturn;
                                }
                            }
                            else if (CardType == "8")//组织机构代码
                            {
                                if (Card != ZZJGDAZ)
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金帐户信息不一致";
                                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                                    return dsreturn;
                                }
                            }
                            else if (CardType == "9")//营业执照
                            {
                                if (Card != YYZZ)
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金帐户信息不一致";
                                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                                    return dsreturn;
                                }
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证件类型无效";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                                return dsreturn;
                            }
                            //验证钱，此地方是加钱，不需验证
                            #endregion
                            
                            #region//执行本地的操作
                            
                            ArrayList list = new ArrayList();
                            //1、将此条信息的数据插入“冲正记录表”中
                            string strINsert = " insert into AAA_CZJLB select * from AAA_ZKLSMXB where LYDH='" + LYDH + "'";
                            list.Add(strINsert);
                            //2、删除这条信息；
                            string strDel = "delete AAA_ZKLSMXB where LYDH='" + LYDH + "'";
                            list.Add(strDel);
                            //3、更新用户余额。
                            //更改登录表可用余额
                            string strUpdate = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE +" + ZZJE + ",B_DSFCGKYYE=B_DSFCGKYYE+" + ZZJE + " where B_DLYX='" + dsZJLSMX.Tables[0].Rows[0]["DLYX"].ToString().Trim() + "'";
                            list.Add(strUpdate);
                            bool bl = DbHelperSQL.ExecSqlTran(list);
                            if (bl)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行成功";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "0000";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = ZJLSNumber;
                                return dsreturn;
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券服务器系统错误";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2041";
                                return dsreturn;
                            }
                            #endregion
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "与原流水信息不符";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2007";
                            return dsreturn;
                        }
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行流水号重复";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2004";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行成功";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "0000";
                    return dsreturn;
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "2405";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "连接证券失败";
            }
        }
        catch (Exception)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "8888";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "证券平台错误";
        }
        return dsreturn;
    }
    //冲银行转证券23703
    public DataSet CBankToS(DataSet ds, DataSet dsreturn)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        DataTable dt = ds.Tables["UINFO"];
        try
        {
            if (dt.Rows.Count > 0)
            {
                string LYDH = dt.Rows[0]["CorrectSerial"].ToString();//被冲正外部流水号=资金流水明细表来源单号
                string YHLX = dt.Rows[0]["bOrganId"].ToString();//银行类型
                string YHZH = dt.Rows[0]["bCustAcct"].ToString();//银行账号
                string Card = dt.Rows[0]["Card"].ToString();//客户证件号
                string CardType = dt.Rows[0]["CardType"].ToString();//客户证件类型
                string ZQZH = dt.Rows[0]["sCustAcct"].ToString();//证券资金账号
                string ZZJE = dt.Rows[0]["OccurBala"].ToString();//转帐金额

                string str = "select *,a.number '资金流水Number' from AAA_ZKLSMXB a left join AAA_DLZHXXB b on a.DLYX=b.B_DLYX where a.LYDH='" + LYDH + "'";
                DataSet dsZJLSMX = DbHelperSQL.Query(str);
                if (dsZJLSMX != null && dsZJLSMX.Tables[0].Rows.Count > 0)
                {
                    if (dsZJLSMX.Tables[0].Rows.Count == 1)
                    {
                        string ZJLSNumber = dsZJLSMX.Tables[0].Rows[0]["资金流水Number"].ToString();
                        double ZJLSJE = Convert.ToDouble(dsZJLSMX.Tables[0].Rows[0]["JE"].ToString());//对应资金流水明细中的金额
                        string SHZH = dsZJLSMX.Tables[0].Rows[0]["I_SFZH"].ToString();//身份证号
                        string ZZJGDAZ = dsZJLSMX.Tables[0].Rows[0]["I_ZZJGDMZDM"].ToString();//组织机构代码证代码
                        string YYZZ = dsZJLSMX.Tables[0].Rows[0]["I_YYZZZCH"].ToString();//营业执照注册号
                        string BDYHZH = dsZJLSMX.Tables[0].Rows[0]["I_YHZH"].ToString();//银行账号
                        double BDKYYE = Convert.ToDouble(dsZJLSMX.Tables[0].Rows[0]["B_ZHDQKYYE"].ToString());//账户当前可用余额
                        double BDDSFCUKYYE = Convert.ToDouble(dsZJLSMX.Tables[0].Rows[0]["B_DSFCGKYYE"].ToString());//第三方存管可用余额
                        if (Convert.ToDouble(ZZJE) == ZJLSJE)
                        {
                            #region//证件验证
                            if (YHZH != BDYHZH)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金帐户信息不一致";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                                return dsreturn;
                            }
                            if (CardType == "1")//身份证
                            {
                                if (Card != SHZH)
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金帐户信息不一致";
                                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                                    return dsreturn;
                                }
                            }
                            else if (CardType == "8")//组织机构代码
                            {
                                if (Card != ZZJGDAZ)
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金帐户信息不一致";
                                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                                    return dsreturn;
                                }
                            }
                            else if (CardType == "9")//营业执照
                            {
                                if (Card != YYZZ)
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金帐户信息不一致";
                                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                                    return dsreturn;
                                }
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证件类型无效";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                                return dsreturn;
                            }
                            //验证钱
                            if (BDKYYE - Convert.ToDouble(ZZJE) < 0 || BDDSFCUKYYE - Convert.ToDouble(ZZJE) < 0)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金不足";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2150";
                                return dsreturn;
                            }
                            #endregion

                            #region//执行本地的操作
                            
                            ArrayList list = new ArrayList();
                            //1、将此条信息的数据插入“冲正记录表”中
                            string strINsert = " insert into AAA_CZJLB select * from AAA_ZKLSMXB where LYDH='" + LYDH + "'";
                            list.Add(strINsert);
                            //2、删除这条信息；
                            string strDel = "delete AAA_ZKLSMXB where LYDH='" + LYDH + "'";
                            list.Add(strDel);
                            //3、更新用户余额。
                            //更改登录表可用余额
                            string strUpdate = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE -" + ZZJE + ",B_DSFCGKYYE=B_DSFCGKYYE-" + ZZJE + " where B_DLYX='" + dsZJLSMX.Tables[0].Rows[0]["DLYX"].ToString().Trim() + "'";
                            list.Add(strUpdate);
                            bool bl = DbHelperSQL.ExecSqlTran(list);
                            if (bl)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行成功";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "0000";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = ZJLSNumber;
                                return dsreturn;
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券服务器系统错误";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2041";
                                return dsreturn;
                            }
                            #endregion
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "与原流水信息不符";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2007";
                            return dsreturn;
                        }
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行流水号重复";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2004";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行成功";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "0000";
                    return dsreturn;
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "2405";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "连接证券失败";
            }
        }
        catch (Exception)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "8888";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "证券平台错误";
        }
        return dsreturn;
    }
}