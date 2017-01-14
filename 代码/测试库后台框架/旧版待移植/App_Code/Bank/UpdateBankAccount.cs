using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;

/// <summary>
/// UpdateBankAccount 的摘要说明
/// </summary>
public class UpdateBankAccount
{
	public UpdateBankAccount()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    //银行发起-变更银行账户25710【变更银行账户允许第三方存管账户有余额，不允许有出金入金的操作（此部分银行验证）】
    public DataSet BankRequest_UpdateBankAccount(DataSet dsreturn, DataSet dt)
    {
        try
        {
            Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
            if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }
            if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }
            if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }


            if (dt != null && dt.Tables[0].Rows.Count > 0)
            {
                string BankLSH = dt.Tables[0].Rows[0]["ExSerial"].ToString();//外部流水号
                string OldYHZH = dt.Tables[0].Rows[0]["bCustAcct"].ToString();//原银行账号
                string NewYHZH = dt.Tables[0].Rows[0]["bNewAcct"].ToString();//新银行账号
                string ZQZJZH = dt.Tables[0].Rows[0]["sCustAcct"].ToString();//证券资金账号
                
                #region//基本验证

                //查询账号
                string strDLB = "select * from AAA_DLZHXXB where I_YHZH='" + OldYHZH.Trim() + "' and I_ZQZJZH='" + ZQZJZH.Trim() + "'";
                DataSet dsDLB = DbHelperSQL.Query(strDLB);
                if (dsDLB != null)
                {
                    if (dsDLB.Tables[0].Rows.Count == 1)
                    {
                        if (dsDLB.Tables[0].Rows[0]["B_SFDJ"].ToString() == "是")//冻结的验证
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您在证券方的账号已被冻结，不能进行变更变更银行账户的操作";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                            return dsreturn;
                        }

                        string strUpDLB = "update AAA_DLZHXXB set I_YHZH='" + NewYHZH.Trim() + "' where I_YHZH='" + OldYHZH.Trim() + "' and I_ZQZJZH='" + ZQZJZH.Trim() + "'";
                        int i = DbHelperSQL.ExecuteSql(strUpDLB);
                        if (i > 0)
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行账号变更成功";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "0000";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = dsDLB.Tables[0].Rows[0]["Number"].ToString();
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券方数据库更新失败";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
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
                #endregion
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
            }
        }
        catch (Exception e)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = e.Message;
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        }
        return dsreturn;
    }
    //券商发起-变更银行账户26710--功能去掉
    public DataSet SRequest_UpdateBankAccount(DataSet dsreturn, DataTable dt)
    {
        try
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                string JSBH = dt.Rows[0]["买家角色编号"].ToString();                
                string NewYHZH = dt.Rows[0]["bNewAcct"].ToString();//新银行账号                
                string time = DateTime.Now.ToString("yyyyMMdd");
                    
                #region//基本验证

                Hashtable JCYZ = PublicClass2013.GetParameterInfo();
                Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
                DataTable dtDLB = JBXX["完整数据集"] as DataTable;
                if (dtDLB == null || dtDLB.Rows.Count <= 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前交易账户的信息，不能进行交易操作。";
                    return dsreturn;
                }
                if (JBXX["交易账户是否开通"].ToString() == "否")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                    return dsreturn;
                }
                if (JBXX["是否冻结"].ToString() == "是" || JBXX["冻结功能项"].ToString().Trim()!="")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能："+JBXX ["冻结功能项"].ToString ();
                    return dsreturn;
                }
                if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                    return dsreturn;
                }
                if (JCYZ["是否在服务时间内(假期)"].ToString() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易日期，暂停交易！";
                    return dsreturn;
                }

                
                #endregion

                if (dtDLB != null)
                {
                    if (dtDLB.Rows.Count == 1)
                    {
                        string BankLSH = dtDLB.Rows[0]["Number"].ToString();//外部流水号
                        string OldYHZH = dtDLB.Rows[0]["I_YHZH"].ToString();//原银行账号
                        string ZQZJZH = dtDLB.Rows[0]["I_ZQZJZH"].ToString();//证券资金账号

                        #region//向银行发送指令查询银行端存管管理账户余额

                        DataTable dtBankYE = BankServicePF.N26710_ModifyBankCard_Init();
                        dtBankYE.Rows[0]["ExSerial"] = BankLSH;//外部流水号
                        dtBankYE.Rows[0]["bCustAcct"] = OldYHZH;//银行账号                    
                        dtBankYE.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号
                        dtBankYE.Rows[0]["bNewAcct"] = NewYHZH;//新银行账号
                        dtBankYE.Rows[0]["Date"] = time;//交易日期
                        DataSet ds = BankServicePF.N26710_ModifyBankCard_Push(dtBankYE);

                        DataTable dt1 = ds.Tables["result"];

                        #endregion

                        if (dt1.Rows[0]["ErrorNo"].ToString() == "0000")
                        {
                            string strUpDLB = "update AAA_DLZHXXB set I_YHZH='" + NewYHZH.Trim() + "' where I_YHZH='" + OldYHZH.Trim() + "' and I_ZQZJZH='" + ZQZJZH.Trim() + "'";
                            int i = DbHelperSQL.ExecuteSql(strUpDLB);
                            if (i > 0)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行账号变更成功!";
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行账号变更失败!";
                            }
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = dt1.Rows[0]["ErrorNo"].ToString() + dt1.Rows[0]["ErrorInfo"].ToString();
                        }


                    }
                    else if (dtDLB.Rows.Count > 1)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "多帐号重复，请通知交易平台工作人员进行处理!";
                    }
                    else if (dtDLB.Rows.Count == 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未在数据库中找到有效信息!";
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您输入的银行账号不正确！";
                }

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
            }
        }
        catch (Exception e)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = e.Message;
        }
        return dsreturn;
    }
    //客户变更存管银行26713
    public DataSet SRequest_UpdateCGBank(DataSet dsreturn, DataTable dt)
    {
        try
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                #region//传过来的参数
                string JSBH = dt.Rows[0]["买家角色编号"].ToString();
                string NewBank = dt.Rows[0]["新存管银行"].ToString();
                string MiMa = dt.Rows[0]["原银行卡密码"].ToString();
                string ZQZJMM = dt.Rows[0]["证券资金密码"].ToString();
                #endregion

                #region//基础验证
                Hashtable JCYZ = PublicClass2013.GetParameterInfo();
                Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
                if (JBXX["交易账户是否开通"].ToString() == "否")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                    return dsreturn;
                }
                if (JBXX["是否冻结"].ToString() == "是" || JBXX["冻结功能项"].ToString().Trim()!="")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系!\n\r被冻结功能：" + JBXX["冻结功能项"].ToString();
                    return dsreturn;
                }

                if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                    return dsreturn;
                }
                if (JCYZ["是否在服务时间内(工作时)"].ToString().Trim() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
                    return dsreturn;
                }
                if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                    return dsreturn;
                }


                #endregion

                #region//准备传输字段
                DataTable dtDLB = JBXX["完整数据集"] as DataTable;
                if (dtDLB == null || dtDLB.Rows.Count <= 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前交易账户的信息，不能进行交易操作。";
                    return dsreturn;
                }

                #region//验证证券资金密码
                if (ZQZJMM != dtDLB.Rows[0]["B_JSZHMM"].ToString())
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码不正确，请重新输入！";
                    return dsreturn;
                }
                #endregion

                double KYYE = Convert.ToDouble(dtDLB.Rows[0]["B_ZHDQKYYE"].ToString());//本地当前账户可用余额
                double QSKYYE = Convert.ToDouble(dtDLB.Rows[0]["B_DSFCGKYYE"].ToString());//第三方存管可用余额
                string ZCLB = dtDLB.Rows[0]["I_ZCLB"].ToString();//注册类别
                string SFZH = dtDLB.Rows[0]["I_SFZH"].ToString();//身份证号
                string YYZZ = dtDLB.Rows[0]["I_YYZZZCH"].ToString(); //营业执照注册号
                string ZZJGDAZ = dtDLB.Rows[0]["I_ZZJGDMZDM"].ToString(); //组织机构代码证代码
                string WBLSH = dtDLB.Rows[0]["Number"].ToString(); //外部流水号
                string ZQZJZH = dtDLB.Rows[0]["I_ZQZJZH"].ToString(); //证券资金账号
                string JSZH = dtDLB.Rows[0]["B_JSZHLX"].ToString();//结算账户类型
                string OldYHZH = dtDLB.Rows[0]["I_YHZH"].ToString();//旧银行账号
                string time = DateTime.Now.ToString("yyyyMMdd");
                string ZJLX = "";//客户证件类型
                string KHZJH = "";//客户证件号
                
                if (ZCLB == "自然人")
                {
                    ZJLX = "1";
                    KHZJH = dtDLB.Rows[0]["I_SFZH"].ToString();//身份证号
                }
                else if (ZCLB == "单位")
                {
                    ZJLX = "8";
                    KHZJH = dtDLB.Rows[0]["I_ZZJGDMZDM"].ToString(); //组织机构代码证代码
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前账号的注册类型无效！";
                    return dsreturn;
                }
                #endregion
                                

                switch (JSZH)
                {
                    case "经纪人交易账户":
                        #region//验证是否有正在进行的交易、资金是否都已清算完、账户余额是否都已为零
                        string JJRJSBH = dtDLB.Rows[0]["J_JJRJSBH"].ToString();//经纪人角色编号
                        //预订单
                        string YDD = "select * from AAA_YDDXXB where MJJSBH='"+JSBH+"' and ZT='竞标'";
                        DataSet dsYDD = DbHelperSQL.Query(YDD);
                        //中标定标信息表
                        string ZBDB = "select * from AAA_ZBDBXXB where Z_QPZT!='清盘结束' and Y_YSYDDMJJSBH='" + JSBH + "'";
                        DataSet dsZBDB = DbHelperSQL.Query(ZBDB);
                        //账款流水明细表 所有"实"
                        string ZKLSS = "select isnull(sum(JE),0.00) '实' from AAA_ZKLSMXB where JSBH='"+JJRJSBH+"' and XM='经纪人收益' and SJLX='实'";
                        DataSet dsZKLSS = DbHelperSQL.Query(ZKLSS);
                        //账款流水明细表 所有"预"
                        string ZKLSY = "select isnull(sum(JE),0.00) '预' from AAA_ZKLSMXB where JSBH='" + JJRJSBH + "' and XM='经纪人收益' and SJLX='预'";
                        DataSet dsZKLSY = DbHelperSQL.Query(ZKLSY);
                        //账款流水明细表
                        string BDStartime = DateTime.Now.ToString("yyyy-MM-dd");
                        string BDEndtime = Convert.ToDateTime(BDStartime).AddHours(23).AddMinutes(59).AddSeconds(59.998).ToString("yyyy-MM-dd HH:mm:ss:fff");
                        string strZKLXMXB = "select * from AAA_ZKLSMXB where (JSBH='" + JSBH + "' or JSBH='" + JJRJSBH + "') and LSCSSJ between '" + BDStartime.ToString() + "' and '" + BDEndtime + "'";
                        DataSet dsZK = DbHelperSQL.Query(strZKLXMXB);
                        //冲正记录表                        
                        string strCZJLB = "select * from AAA_ZKLSMXB where (JSBH='" + JSBH + "' or JSBH='" + JJRJSBH + "') and LSCSSJ between '" + BDStartime.ToString() + "' and '" + BDEndtime + "'";
                        DataSet dsCZJLB = DbHelperSQL.Query(strCZJLB);
                        string msg = "";
                        if ((dsYDD != null && dsYDD.Tables[0].Rows.Count > 0) || (dsZBDB != null && dsZBDB.Tables[0].Rows.Count>0))
                        {
                            msg += "您当前有尚未完成的交易，不能进行变更存管银行的操作！\n\r";
                        }
                        if (dsZKLSS != null && dsZKLSY != null && Convert.ToDouble(dsZKLSS.Tables[0].Rows[0]["实"].ToString()) == Convert.ToDouble(dsZKLSY.Tables[0].Rows[0]["预"].ToString()))
                        {                            
                        }
                        else
                        {
                            msg += "您当前有尚未结算清的账款，不能进行变更存管银行的操作！\n\r";
                        }
                        if ((dsZK != null && dsZK.Tables[0].Rows.Count > 0) || (dsCZJLB != null && dsCZJLB.Tables[0].Rows.Count > 0))
                        {
                            msg += "今天已有账款变动，请第二日再进行变更存管银行的操作！\n\r";
                        }
                        if (KYYE != 0 || QSKYYE!=0)
                        {
                            msg += "本地账户余额：" + KYYE.ToString("#0.00") + "；第三方存管账户余额：" + QSKYYE.ToString("#0.00") + "；本地账户及第三方存管账户余额都为零，才能进行变更存管银行的操作！\n\r";
                        }
                        #endregion

                        if (msg != "")
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = msg;
                            return dsreturn;
                        }
                        else
                        {
                            #region//向原来旧银行卡的银行发送请求、向新存管银行做新客户签约交易
                            DataTable dtUpdatBank = BankServicePF.N26713_ModifyBank_Init();
                            dtUpdatBank.Rows[0]["ExSerial"] = WBLSH;//外部流水号
                            dtUpdatBank.Rows[0]["bCustAcct"] = OldYHZH;//银行账号                    
                            dtUpdatBank.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号	
                            dtUpdatBank.Rows[0]["CardType"] = ZJLX;//客户证件类型
                            dtUpdatBank.Rows[0]["Card"] = KHZJH;//客户证件号
                            dtUpdatBank.Rows[0]["OccurBala"] = KYYE;//证券资金余额	
                            dtUpdatBank.Rows[0]["Date"] = time;//交易日期
                            DataSet dsUpdatBank = BankServicePF.N26713_ModifyBank_Push(dtUpdatBank);

                            DataTable dtUpdatBank1 = dsUpdatBank.Tables["result"];
                            if (dtUpdatBank1.Rows[0]["ErrorNo"].ToString() == "0000")//交易成功
                            {
                                #region//向新存管银行做新客户签约交易--应答成功后，修改登录表“第三方存管状态”为“未开通”，“银行账号”清空
                                //
                                //start向新存管银行做新客户签约交易代码
                                //------
                                //------

                                string strUpdateDLB = "update AAA_DLZHXXB set I_DSFCGZT='未开通',I_YHZH='' where J_BUYJSBH='"+JSBH+"'";
                                int i = DbHelperSQL.ExecuteSql(strUpdateDLB);
                                if (i > 0)
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "变更存管银行操作成功！";
                                    return dsreturn;
                                }
                                else
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "变更存管银行操作失败！";
                                    return dsreturn;
                                }
                                #endregion
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = dtUpdatBank1.Rows[0]["ErrorNo"].ToString() + dtUpdatBank1.Rows[0]["ErrorInfo"].ToString();
                                return dsreturn;
                            }
                            #endregion
                        }                       

                    case "买家卖家交易账户":
                        
                        #region//验证是否有正在进行的交易、资金是否都已清算完、账户余额是否都已为零
                        string SelJSBH = dtDLB.Rows[0]["J_SELJSBH"].ToString();//卖家角色编号
                        //预订单
                        YDD = "select * from AAA_YDDXXB where MJJSBH='"+JSBH+"' and ZT='竞标'";
                        dsYDD = DbHelperSQL.Query(YDD);
                        string TBD = "select * from AAA_TBD where MJJSBH='" + SelJSBH + "' and ZT='竞标'";
                        DataSet dsTBD = DbHelperSQL.Query(TBD);
                        //中标定标信息表
                        ZBDB = "select * from AAA_ZBDBXXB where Z_QPZT!='清盘结束' and T_YSTBDMJJSBH='" + SelJSBH + "' or Y_YSYDDMJJSBH='" + JSBH + "'";
                        dsZBDB = DbHelperSQL.Query(ZBDB);
                        //账款流水明细表
                        BDStartime = DateTime.Now.ToString("yyyy-MM-dd");
                        BDEndtime = Convert.ToDateTime(BDStartime).AddHours(23).AddMinutes(59).AddSeconds(59.998).ToString("yyyy-MM-dd HH:mm:ss:fff");
                        strZKLXMXB = "select * from AAA_ZKLSMXB where (JSBH='" + JSBH + "' or JSBH='" + SelJSBH + "') and LSCSSJ between '" + BDStartime.ToString() + "' and '" + BDEndtime + "'";
                        dsZK = DbHelperSQL.Query(strZKLXMXB);
                        //冲正记录表                        
                        strCZJLB = "select * from AAA_ZKLSMXB where (JSBH='" + JSBH + "' or JSBH='" + SelJSBH + "') and LSCSSJ between '" + BDStartime.ToString() + "' and '" + BDEndtime + "'";
                        dsCZJLB = DbHelperSQL.Query(strCZJLB);
                        msg = "";
                        if ((dsYDD != null && dsYDD.Tables[0].Rows.Count > 0) || (dsTBD != null && dsTBD.Tables[0].Rows.Count > 0) || (dsZBDB != null && dsZBDB.Tables[0].Rows.Count > 0))
                        {
                            msg += "您当前有尚未完成的交易，不能进行变更存管银行的操作！\n\r";
                        }
                        if (KYYE != 0 || QSKYYE!=0)
                        {
                            msg += "本地账户余额：" + KYYE.ToString("#0.00") + "；第三方存管账户余额：" + QSKYYE.ToString("#0.00") + "；本地账户及第三方存管账户余额都为零，才能进行变更存管银行的操作！\n\r";
                        }
                        if ((dsZK != null && dsZK.Tables[0].Rows.Count > 0)||(dsCZJLB != null && dsCZJLB.Tables[0].Rows.Count > 0))
                        {
                            msg += "今天已有账款变动，请第二日再进行变更存管银行的操作！\n\r";
                        }
                        #endregion

                        if (msg != "")
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = msg;
                            return dsreturn;
                        }
                        else
                        {
                            #region//向原来旧银行卡的银行发送请求、向新存管银行做新客户签约交易
                            DataTable dtUpdatBank = BankServicePF.N26713_ModifyBank_Init();
                            dtUpdatBank.Rows[0]["ExSerial"] = WBLSH;//外部流水号
                            dtUpdatBank.Rows[0]["bCustAcct"] = OldYHZH;//银行账号                    
                            dtUpdatBank.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号	
                            dtUpdatBank.Rows[0]["CardType"] = ZJLX;//客户证件类型
                            dtUpdatBank.Rows[0]["Card"] = KHZJH;//客户证件号
                            dtUpdatBank.Rows[0]["OccurBala"] = KYYE;//证券资金余额	
                            dtUpdatBank.Rows[0]["Date"] = time;//交易日期
                            DataSet dsUpdatBank = BankServicePF.N26713_ModifyBank_Push(dtUpdatBank);

                            DataTable dtUpdatBank1 = dsUpdatBank.Tables["result"];
                            if (dtUpdatBank1.Rows[0]["ErrorNo"].ToString() == "0000")//交易成功
                            {
                                #region//向新存管银行做新客户签约交易--应答成功后，修改登录表“第三方存管状态”为“未开通”，“银行账号”清空
                                //
                                //start向新存管银行做新客户签约交易代码
                                //------
                                //------

                                string strUpdateDLB = "update AAA_DLZHXXB set I_DSFCGZT='未开通',I_YHZH='' where J_BUYJSBH='" + JSBH + "'";
                                int i = DbHelperSQL.ExecuteSql(strUpdateDLB);
                                if (i > 0)
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "变更存管银行操作成功！";
                                    return dsreturn;
                                }
                                else
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "变更存管银行操作失败！";
                                    return dsreturn;
                                }
                                #endregion
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = dtUpdatBank1.Rows[0]["ErrorNo"].ToString() + dtUpdatBank1.Rows[0]["ErrorInfo"].ToString();
                                return dsreturn;
                            }
                            #endregion
                        }                       


                    default:
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "结算账户类型无效！";
                        break;
                }

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
            }
        }
        catch(Exception e)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = e.Message;
        }
        return dsreturn;
    }

}