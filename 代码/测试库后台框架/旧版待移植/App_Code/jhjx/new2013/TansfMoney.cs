using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using Key;
using Hesion.Brick.Core;
using System.Data.SqlClient;
using Hesion.Brick.Core.WorkFlow;
using Galaxy.ClassLib.DataBaseFactory;
using System.Web.Services;
using System.Threading;
using System.IO;
/// <summary>
/// TansfMoney 用于用户在银行与交易账户之间的资金转账
/// </summary>
public class TansfMoney
{
	public TansfMoney()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 获取写入账款流水明细表的SQL语句
    /// </summary>
    /// <param name="num">对照表主键编号</param>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="mname">模块名</param>
    /// <param name="je">金额</param>
    /// <param name="lydh">来源单号</param>
    /// <param name="replacex1">替代字符串</param>
    /// <param name="replacex2">替代字符串</param>
    /// <returns></returns>
    protected string SqlInsertText(string num, string dlyx, string mname, string je, string lydh, string replacex1, string replacex2)
    {
        WorkFlowModule WFM = new WorkFlowModule("AAA_ZKLSMXB");
        string key = WFM.numberFormat.GetNextNumberZZ("");

        DataTable dtemp1 = DbHelperSQL.Query("select * from AAA_moneyDZB where Number='" + num + "'").Tables[0];
        string xm = dtemp1.Rows[0]["XM"].ToString().Trim();
        string xz = dtemp1.Rows[0]["XZ"].ToString().Trim();
        string zy = dtemp1.Rows[0]["ZY"].ToString().Trim().Replace("[x1]", replacex1).Replace("[x2]", replacex2);
        string sjlx = dtemp1.Rows[0]["SJLX"].ToString().Trim();
        string yslx = dtemp1.Rows[0]["YSLX"].ToString().Trim();

        DataTable dtemp2 = DbHelperSQL.Query("select  B_JSZHLX,J_BUYJSBH from AAA_DLZHXXB where B_DLYX='" + dlyx + "'").Tables[0];
        string jszhlx = dtemp2.Rows[0]["B_JSZHLX"].ToString().Trim();
        string jsbh = dtemp2.Rows[0]["J_BUYJSBH"].ToString().Trim();

        string str = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],[CheckState],[CreateUser],[CreateTime]) VALUES ('" + key + "','" + dlyx + "','" + jszhlx + "','" + jsbh + "','" + mname + "','" + lydh + "',getdate(),'" + yslx + "'," + je + ",'" + xm + "','" + xz + "','" + zy + "','接口编号','" + sjlx + "',0,'" + dlyx + "',GETDATE())";

        dtemp1.Dispose();
        dtemp2.Dispose();

        return str;

    }

    //用于资金转账的方法
    public DataSet RunTranfer(DataSet dsreturn, DataTable dt)
    {
        try
        {

            if (dt != null && dt.Rows.Count > 0)
            {
                #region//传过来的信息
                string JSBH = dt.Rows[0]["买家角色编号"].ToString().Trim();
                string zzlb = dt.Rows[0]["zzlb"].ToString().Trim();//转账方式
                double je = Convert.ToDouble(dt.Rows[0]["je"]);//转账金额
                string ZQZJMM = dt.Rows[0]["zqzjmm"].ToString().Trim();//证券资金密码
                #endregion

                Hashtable JCYZ = PublicClass2013.GetParameterInfo();
                Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
                DataTable dtDLB = JBXX["完整数据集"] as DataTable;

                if (dtDLB==null || dtDLB.Rows.Count <= 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前交易账户的信息，不能进行交易操作。";
                    return dsreturn;
                }

                #region//验证证券资金密码--模拟运行期间，暂时去掉这个验证
                //if (ZQZJMM != dtDLB.Rows[0]["B_JSZHMM"].ToString().Trim())
                //{
                //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码不正确，请重新输入！";
                //    return dsreturn;
                //}
                #endregion

                string dlyx = dtDLB.Rows[0]["B_DLYX"].ToString().Trim();
                string yhzh = dtDLB.Rows[0]["I_YHZH"].ToString().Trim();//银行账号
                string khyh = dtDLB.Rows[0]["I_KHYH"].ToString().Trim();//开户银行
                string JSZHLX = dtDLB.Rows[0]["B_JSZHLX"].ToString().Trim();//结算账户类型
                string ZQZJZH = dtDLB.Rows[0]["I_ZQZJZH"].ToString().Trim();//证券资金账号
                double DQZHKYYE = Convert.ToDouble(dtDLB.Rows[0]["B_ZHDQKYYE"].ToString().Trim());//当前账户可用余额
                double DSFCGKYYE = Convert.ToDouble(dtDLB.Rows[0]["B_DSFCGKYYE"].ToString().Trim());//第三方存管可用余额

                string zjmm = "";
                //if (zzlb == "银转商")
                //{
                //    zjmm = dt.Rows[0]["zjmm"].ToString().Trim();
                //}
                zjmm = dt.Rows[0]["zjmm"].ToString().Trim();              

                string time = DateTime.Now.ToString("yyyyMMdd");

                #region//获取关联经纪人的信息

                string GLJJR = "";//关联经纪人
                string PTGLJG = "";//平台管理机构
                if (JSZHLX.Contains("买家卖家交易账户"))
                {
                    string strJJRXX = "select b.I_JYFMC 交易方名称,b.I_PTGLJG 平台管理机构 from AAA_MJMJJYZHYJJRZHGLB a left join AAA_DLZHXXB b on a.GLJJRDLZH=b.B_DLYX where a.SFDQMRJJR='是' and a.DLYX='" + dlyx.Trim() + "'";
                    DataSet dsJJRXX = DbHelperSQL.Query(strJJRXX);
                    GLJJR = dsJJRXX.Tables[0].Rows[0]["交易方名称"].ToString().Trim();//关联经纪人
                    PTGLJG = dsJJRXX.Tables[0].Rows[0]["平台管理机构"].ToString().Trim();//平台管理机构
                }
                else
                {
                    GLJJR = dtDLB.Rows[0]["I_JYFMC"].ToString().Trim();//关联经纪人
                    PTGLJG = dtDLB.Rows[0]["I_PTGLJG"].ToString().Trim();//平台管理机构
                }
                
                #endregion


                //资金变动明细对照表
                DataSet dsDZB = DbHelperSQL.Query("select * from AAA_moneyDZB where Number in ('1304000001','1304000002')");

                //生成帐款流水明细表的Number
                WorkFlowModule WFM = new WorkFlowModule("AAA_ZKLSMXB");
                string key = WFM.numberFormat.GetNextNumberZZ("");
    
                if (zzlb == "银转商")
                {
                    
                    DataTable dt1;
                    if (ConfigurationManager.ConnectionStrings["onlineJHJX_BankInterface"].ToString().Contains("关闭出金入金"))
                    {
                        
                        dt1 = new DataTable();
                        DataColumn colum = new DataColumn("ErrorNo", System.Type.GetType("System.String"));
                        dt1.Columns.Add(colum);
                        colum = new DataColumn("bSerial", System.Type.GetType("System.String"));
                        dt1.Columns.Add(colum);
                        colum = new DataColumn("ExSerial", System.Type.GetType("System.String"));
                        dt1.Columns.Add(colum);
                        colum = new DataColumn("OccurBala", System.Type.GetType("System.String"));
                        dt1.Columns.Add(colum);
                        DataRow row = dt1.NewRow();
                        row["ErrorNo"] = "0000";
                        row["bSerial"] = "123123";
                        row["ExSerial"] = "456456";
                        row["OccurBala"] = je;//转帐金额
                        dt1.Rows.Add(row);  
                    }
                    else if (ConfigurationManager.ConnectionStrings["onlineJHJX_BankInterface"].ToString().Contains("开启出金入金"))
                    {
                        string JMZJMM = CallPinMac.EncryptString(zjmm);//加密后的交易密码
                        #region//向银行发送指令进行银转商
                        DataTable dtBank = BankServicePF.N24701_BankToSecurities_Init();
                        dtBank.Rows[0]["ExSerial"] = key;//外部流水号
                        dtBank.Rows[0]["bCustAcct"] = yhzh;//银行账号                    
                        dtBank.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号	
                        dtBank.Rows[0]["bCustPwd"] = JMZJMM;//银行密码
                        dtBank.Rows[0]["OccurBala"] = je;//转帐金额
                        dtBank.Rows[0]["Date"] = time;//交易日期
                        DataSet ds = BankServicePF.N24701_ModifyUserInfo_Push(dtBank);

                        dt1 = ds.Tables["result"];
                        #endregion
                    }
                    else
                    {
                        dt1 = null;
                    }

                    

                    #region//接收到银行的反馈后进行的操作
                    if (dt1.Rows[0]["ErrorNo"].ToString() == "0000")//交易成功
                    {
                        try
                        {
                            List<string> list = new List<string>();
                            string BankLSH = dt1.Rows[0]["bSerial"].ToString();//银行流水号
                            string BDLSH = dt1.Rows[0]["ExSerial"].ToString();//本地流水号，自己的
                            double YDje = Convert.ToDouble(dt1.Rows[0]["OccurBala"].ToString());//转帐金额
                            string BDTime = DateTime.Now.ToString();

                            #region//写入资金流水明细、更改登录表可用余额及第三方存管可用余额
                            //写入资金流水明细
                            DataRow[] rows = dsDZB.Tables[0].Select("Number='1304000001'");
                            string xm = rows[0]["XM"].ToString().Trim();
                            string xz = rows[0]["XZ"].ToString().Trim();
                            string zy = rows[0]["ZY"].ToString().Trim().Replace("[x1]", BankLSH);
                            string sjlx = rows[0]["SJLX"].ToString().Trim();
                            string yslx = rows[0]["YSLX"].ToString().Trim();
                            string str = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],BDCJRJLSH,[CheckState],[CreateUser],[CreateTime]) VALUES ('" + key + "','" + dlyx + "','" + JSZHLX + "','" + JSBH + "','券商入金','" + BankLSH + "','" + BDTime + "','" + yslx + "'," + YDje + ",'" + xm + "','" + xz + "','" + zy + "','','" + sjlx + "','" + BDLSH + "',0,'" + dlyx + "','" + BDTime + "')";
                            list.Add(str);
                            //更改登录表可用余额
                            str = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE +" + YDje + ",B_DSFCGKYYE=B_DSFCGKYYE+" + YDje + " where B_DLYX='" + dlyx + "'";
                            list.Add(str);
                            #endregion

                            int i = DbHelperSQL.ExecuteSqlTran(list);
                            if (i > 0)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您成功转账" + YDje.ToString("#0.00") + "元!";
                                return dsreturn;
                            }
                            else
                            {
                                #region//发起冲银行转证券操作（24703）
                                #region//向银行发送冲银行转证券操作（24703）的指令
                                DataTable dtCZQToBank = BankServicePF.N24703_BankToSecurities_Init();
                                dtCZQToBank.Rows[0]["ExSerial"] = key;//外部流水号
                                dtCZQToBank.Rows[0]["CorrectSerial"] = dt1.Rows[0]["ExSerial"].ToString();//被冲正的流水号：自己的流水号
                                dtCZQToBank.Rows[0]["bCustAcct"] = yhzh;//银行账号      
                                dtCZQToBank.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号	
                                dtCZQToBank.Rows[0]["Date"] = time;//交易日期
                                DataSet dsCZQToBank = BankServicePF.N24703_BankToSecurities_Push(dtCZQToBank);

                                DataTable dtCSToBank = dsCZQToBank.Tables["result"];
                                #endregion
                                if (dtCSToBank.Rows[0]["ErrorNo"].ToString() == "0000")
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败!";
                                    return dsreturn;
                                }
                                else
                                {
                                    #region//冻结当前账号、向用户冻结状态变更记录表中插数据
                                    ArrayList arylist = new ArrayList();
                                    string strUpdateDLB = "update AAA_DLZHXXB set B_SFDJ='是' where J_BUYJSBH='" + JSBH.Trim() + "'";
                                    arylist.Add(strUpdateDLB);

                                    //生成帐款流水明细表的Number
                                    WorkFlowModule JHJXUserDJ = new WorkFlowModule("AAA_JhjxUserdongjieremark");
                                    string DJNumber = JHJXUserDJ.numberFormat.GetNextNumberZZ("");

                                    string strUserDJBGJLB = "INSERT INTO [AAA_JhjxUserdongjieremark] ([Number],[DLYX],[JSZHLX],[DJSGLJJR],[PTGLJG],[DJGN],[DJYY],[DJPZ],[YYSM],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + DJNumber + "','" + dlyx + "','" + JSZHLX + "','" + GLJJR + "','" + PTGLJG + "','其他','券商发起-银转商',<DJPZ, varchar(350),>,'本地插入流水明细、更改可用余额SQL执行失败,执行冲银行转证券指令，银行返回错误码执行冻结操作，转出金额：" + Convert.ToDouble(dt1.Rows[0]["OccurBala"].ToString()).ToString("#0.00") + "',银行流水号：" + dt1.Rows[0]["bSerial"].ToString() + "',1,'系统管理员',getdate(),getdate())";
                                    arylist.Add(strUserDJBGJLB);
                                    bool DJ = DbHelperSQL.ExecSqlTran(arylist);
                                    if (DJ)
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败,当前账号被冻结!";
                                        return dsreturn;
                                    }
                                    else
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败,当前账号冻结失败!";
                                        return dsreturn;
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        catch(Exception)
                        {
                            #region//发起冲银行转证券操作（24703）
                            #region//向银行发送冲银行转证券操作（24703）的指令
                            DataTable dtCZQToBank = BankServicePF.N24703_BankToSecurities_Init();
                            dtCZQToBank.Rows[0]["ExSerial"] = key;//外部流水号
                            dtCZQToBank.Rows[0]["CorrectSerial"] = dt1.Rows[0]["ExSerial"].ToString();//被冲正的流水号：自己的流水号
                            dtCZQToBank.Rows[0]["bCustAcct"] = yhzh;//银行账号      
                            dtCZQToBank.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号	
                            dtCZQToBank.Rows[0]["Date"] = time;//交易日期
                            DataSet dsCZQToBank = BankServicePF.N24703_BankToSecurities_Push(dtCZQToBank);

                            DataTable dtCSToBank = dsCZQToBank.Tables["result"];
                            #endregion
                            if (dtCSToBank.Rows[0]["ErrorNo"].ToString() == "0000")
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败!";
                                return dsreturn;
                            }
                            else
                            {
                                #region//冻结当前账号、向用户冻结状态变更记录表中插数据
                                ArrayList arylist = new ArrayList();
                                string strUpdateDLB = "update AAA_DLZHXXB set B_SFDJ='是' where J_BUYJSBH='" + JSBH.Trim() + "'";
                                arylist.Add(strUpdateDLB);

                                //生成帐款流水明细表的Number
                                WorkFlowModule JHJXUserDJ = new WorkFlowModule("AAA_JhjxUserdongjieremark");
                                string DJNumber = JHJXUserDJ.numberFormat.GetNextNumberZZ("");

                                string strUserDJBGJLB = "INSERT INTO [AAA_JhjxUserdongjieremark] ([Number],[DLYX],[JSZHLX],[DJSGLJJR],[PTGLJG],[DJGN],[DJYY],[DJPZ],[YYSM],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + DJNumber + "','" + dlyx + "','" + JSZHLX + "','" + GLJJR + "','" + PTGLJG + "','其他','券商发起-银转商',<DJPZ, varchar(350),>,'本地插入流水明细、更改可用余额SQL执行失败,执行冲银行转证券指令，银行返回错误码执行冻结操作，转出金额：" + Convert.ToDouble(dt1.Rows[0]["OccurBala"].ToString()).ToString("#0.00") + "',银行流水号：" + dt1.Rows[0]["bSerial"].ToString() + "',1,'系统管理员',getdate(),getdate())";
                                arylist.Add(strUserDJBGJLB);
                                bool DJ = DbHelperSQL.ExecSqlTran(arylist);
                                if (DJ)
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败,当前账号被冻结!";
                                    return dsreturn;
                                }
                                else
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败,当前账号冻结失败!";
                                    return dsreturn;
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = dt1.Rows[0]["ErrorNo"].ToString()+dt1.Rows[0]["ErrorInfo"].ToString();
                        return dsreturn;
                    }
                    #endregion

                }
                else if (zzlb == "商转银")
                {

 



                    #region//基础验证
                    
                    if (JBXX["交易账户是否开通"].ToString() == "否")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                        return dsreturn;
                    }



                    if (JBXX["冻结功能项"].ToString().Trim().IndexOf("出金") >= 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + JCYZ["冻结功能项"].ToString().Trim();
                        return dsreturn;
                    }
                    if (JBXX["是否休眠"].ToString().Trim() == "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
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
                    DataTable JBXXAll = JBXX["完整数据集"] as DataTable;
                    #region//测试期间，暂时去掉此验证
                    //if (JBXXAll.Rows[0]["I_DSFCGZT"].ToString() != "开通")
                    //{
                    //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！";
                    //    return dsreturn;
                    //}
                    #endregion
                    #endregion



                    #region//可转余额
                    double KZYE = 0.00;//可转余额
                    //登录账号可用余额小于银行端存管管理账户余额，可转账额取登录账号可用余额
                    if (DQZHKYYE <= DSFCGKYYE)
                    {
                        KZYE = DQZHKYYE;//可转余额=当前账户可用余额
                    }
                    else
                    {
                        KZYE = DSFCGKYYE;//可转余额=第三方存管可用余额
                    }
                    #endregion

                    if (je > KZYE)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前账户最大可转金额为" + KZYE.ToString("#0.00") + "元，本次转账金额过大，不能进行转账！";
                        return dsreturn;
                    }
                    else
                    {
                        DataTable dtSToBank;
                        if (ConfigurationManager.ConnectionStrings["onlineJHJX_BankInterface"].ToString().Contains("关闭出金入金"))
                        {
                            dtSToBank = new DataTable();

                            DataColumn colum = new DataColumn("ErrorNo", System.Type.GetType("System.String"));
                            dtSToBank.Columns.Add(colum);
                            colum = new DataColumn("bSerial", System.Type.GetType("System.String"));
                            dtSToBank.Columns.Add(colum);
                            colum = new DataColumn("ExSerial", System.Type.GetType("System.String"));
                            dtSToBank.Columns.Add(colum);
                            colum = new DataColumn("OccurBala", System.Type.GetType("System.String"));
                            dtSToBank.Columns.Add(colum);
                            DataRow row = dtSToBank.NewRow();
                            row["ErrorNo"] = "0000";
                            row["bSerial"] = "123123";
                            row["ExSerial"] = "456456";
                            row["OccurBala"] = je;//转帐金额
                            dtSToBank.Rows.Add(row);
                        }
                        else if (ConfigurationManager.ConnectionStrings["onlineJHJX_BankInterface"].ToString().Contains("开启出金入金"))
                        {
                            string JMZJMM = CallPinMac.EncryptString(zjmm);//加密后的交易密码
                            #region//向银行发送指令进行商转银
                            DataTable dtBank = BankServicePF.N24702_SecuritiesToBank_Init();
                            dtBank.Rows[0]["ExSerial"] = key;//外部流水号
                            dtBank.Rows[0]["bCustAcct"] = yhzh;//银行账号      
                            dtBank.Rows[0]["bCustPwd"] = JMZJMM;//银行卡交易密码
                            dtBank.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号	
                            dtBank.Rows[0]["OccurBala"] = je;//转帐金额
                            dtBank.Rows[0]["Date"] = time;//交易日期
                            if (JBXXAll.Rows[0]["I_ZCLB"].ToString() == "单位")
                            {
                                dtBank.Rows[0]["CardType"] = "8";//客户证件类型
                                dtBank.Rows[0]["Card"] = JBXXAll.Rows[0]["I_ZZJGDMZDM"].ToString();//客户证件号--组织机构代码证
                            }
                            DataSet dsSToBank = BankServicePF.N24702_SecuritiesToBank_Push(dtBank);

                            dtSToBank = dsSToBank.Tables["result"];
                            #endregion
                        }
                        else
                        {
                            dtSToBank = null;
                        }
                        

                        #region//接收到银行的反馈后进行的操作
                        if (dtSToBank.Rows[0]["ErrorNo"].ToString() == "0000")//交易成功
                        {
                            try
                            {
                                List<string> list = new List<string>();
                                string SelLSH = dtSToBank.Rows[0]["ExSerial"].ToString();//自己的流水号
                                string BankLSH = dtSToBank.Rows[0]["bSerial"].ToString();//银行流水号
                                double YDje = Convert.ToDouble(dtSToBank.Rows[0]["OccurBala"].ToString());//转帐金额
                                string BDTime = DateTime.Now.ToString();

                                #region//写入资金流水明细、更改登录表可用余额
                                //写入资金流水明细
                                DataRow[] rows = dsDZB.Tables[0].Select("Number='1304000002'");
                                string xm = rows[0]["XM"].ToString().Trim();
                                string xz = rows[0]["XZ"].ToString().Trim();
                                string zy = rows[0]["ZY"].ToString().Trim().Replace("[x1]", BankLSH);
                                string sjlx = rows[0]["SJLX"].ToString().Trim();
                                string yslx = rows[0]["YSLX"].ToString().Trim();
                                string str = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],BDCJRJLSH,[CheckState],[CreateUser],[CreateTime]) VALUES ('" + key + "','" + dlyx + "','" + JSZHLX + "','" + JSBH + "','券商出金','" + BankLSH + "','" + BDTime + "','" + yslx + "'," + YDje + ",'" + xm + "','" + xz + "','" + zy + "','','" + sjlx + "','" + SelLSH + "',0,'" + dlyx + "','" + BDTime + "')";
                                list.Add(str);
                                //更改登录表可用余额
                                str = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE -" + YDje + ",B_DSFCGKYYE=B_DSFCGKYYE+" + YDje + " where B_DLYX='" + dlyx + "'";
                                list.Add(str);
                                #endregion

                                int i = DbHelperSQL.ExecuteSqlTran(list);
                                if (i > 0)
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您成功转账" + YDje.ToString("#0.00") + "元!";
                                    return dsreturn;
                                }
                                else
                                {                                    
                                    #region//发起冲证券转银行的操作
                                    #region//向银行发送冲证券转银行（24704）的指令
                                    DataTable dtCZQToBank = BankServicePF.N24704_CSecuritiesToBank_Init();
                                    dtCZQToBank.Rows[0]["ExSerial"] = key;//外部流水号
                                    dtCZQToBank.Rows[0]["CorrectSerial"] = SelLSH;//被冲正外部流水号
                                    dtCZQToBank.Rows[0]["bCustAcct"] = yhzh;//银行账号      
                                    dtCZQToBank.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号	
                                    dtCZQToBank.Rows[0]["Date"] = time;//交易日期
                                    DataSet dsCZQToBank = BankServicePF.N24704_CSecuritiesToBank_Push(dtCZQToBank);

                                    DataTable dtCSToBank = dsCZQToBank.Tables["result"];
                                    #endregion
                                    if (dtCSToBank.Rows[0]["ErrorNo"].ToString() == "0000")
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败!";
                                        return dsreturn;
                                    }
                                    else
                                    {                                        
                                        #region//冻结当前账号、向用户冻结状态变更记录表中插数据
                                        ArrayList arylist = new ArrayList();
                                        string strUpdateDLB = "update AAA_DLZHXXB set B_SFDJ='是' where J_BUYJSBH='" + JSBH.Trim() + "'";
                                        arylist.Add(strUpdateDLB);

                                        //生成帐款流水明细表的Number
                                        WorkFlowModule JHJXUserDJ = new WorkFlowModule("AAA_JhjxUserdongjieremark");
                                        string DJNumber = JHJXUserDJ.numberFormat.GetNextNumberZZ("");

                                        string strUserDJBGJLB = "INSERT INTO [AAA_JhjxUserdongjieremark] ([Number],[DLYX],[JSZHLX],[DJSGLJJR],[PTGLJG],[DJGN],[DJYY],[DJPZ],[YYSM],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + DJNumber + "','" + dlyx + "','" + JSZHLX + "','" + GLJJR + "','" + PTGLJG + "','其他','券商发起-商转银',<DJPZ, varchar(350),>,'本地插入流水明细、更改可用余额SQL执行失败,执行冲证券转银行指令，银行返回错误码执行冻结操作，转出金额：" + YDje.ToString("#0.00") + ",银行流水号：" + BankLSH + "',1,'系统管理员',getdate(),getdate())";
                                        arylist.Add(strUserDJBGJLB);
                                        bool DJ = DbHelperSQL.ExecSqlTran(arylist);
                                        if (DJ)
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败,当前账号被冻结!";
                                            return dsreturn;
                                        }
                                        else
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败,当前账号冻结失败!";
                                            return dsreturn;
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            catch(Exception)
                            {
                                #region//发起冲证券转银行的操作
                                #region//向银行发送冲证券转银行（24704）的指令
                                DataTable dtCZQToBank = BankServicePF.N24704_CSecuritiesToBank_Init();
                                dtCZQToBank.Rows[0]["ExSerial"] = key;//外部流水号
                                dtCZQToBank.Rows[0]["CorrectSerial"] = dtSToBank.Rows[0]["ExSerial"].ToString();//被冲正外部流水号，自己的流水号
                                dtCZQToBank.Rows[0]["bCustAcct"] = yhzh;//银行账号      
                                dtCZQToBank.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号	
                                dtCZQToBank.Rows[0]["Date"] = time;//交易日期
                                DataSet dsCZQToBank = BankServicePF.N24704_CSecuritiesToBank_Push(dtCZQToBank);

                                DataTable dtCSToBank = dsCZQToBank.Tables["result"];
                                #endregion
                                if (dtCSToBank.Rows[0]["ErrorNo"].ToString() == "0000")
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败!";
                                    return dsreturn;
                                }
                                else
                                {
                                    #region//冻结当前账号、向用户冻结状态变更记录表中插数据
                                    ArrayList arylist = new ArrayList();
                                    string strUpdateDLB = "update AAA_DLZHXXB set B_SFDJ='是' where J_BUYJSBH='" + JSBH.Trim() + "'";
                                    arylist.Add(strUpdateDLB);

                                    //生成帐款流水明细表的Number
                                    WorkFlowModule JHJXUserDJ = new WorkFlowModule("AAA_JhjxUserdongjieremark");
                                    string DJNumber = JHJXUserDJ.numberFormat.GetNextNumberZZ("");

                                    string strUserDJBGJLB = "INSERT INTO [AAA_JhjxUserdongjieremark] ([Number],[DLYX],[JSZHLX],[DJSGLJJR],[PTGLJG],[DJGN],[DJYY],[DJPZ],[YYSM],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + DJNumber + "','" + dlyx + "','" + JSZHLX + "','" + GLJJR + "','" + PTGLJG + "','其他','券商发起-商转银',<DJPZ, varchar(350),>,'本地插入流水明细、更改可用余额SQL执行失败,执行冲证券转银行指令，银行返回错误码执行冻结操作，转出金额：" + Convert.ToDouble(dtSToBank.Rows[0]["OccurBala"].ToString()).ToString("#0.00") + ",银行流水号：" + dtSToBank.Rows[0]["bSerial"].ToString() + "',1,'系统管理员',getdate(),getdate())";
                                    arylist.Add(strUserDJBGJLB);
                                    bool DJ = DbHelperSQL.ExecSqlTran(arylist);
                                    if (DJ)
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败,当前账号被冻结!";
                                        return dsreturn;
                                    }
                                    else
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败,当前账号冻结失败!";
                                        return dsreturn;
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = dtSToBank.Rows[0]["ErrorNo"].ToString() + dtSToBank.Rows[0]["ErrorInfo"].ToString();
                            return dsreturn;
                        }
                        #endregion
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请选择正确的转账类型！";
                    return dsreturn;
                }

            }
            return dsreturn;
        }
        catch(Exception e)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统错误！";
            return dsreturn;
        }
    }


    public DataSet MaxZZJE(DataSet dsreturn, DataTable dt)
    {
        try
        {
            if (dt != null)
            {
                string JSBH = dt.Rows[0]["买家角色编号"].ToString();
                Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
                DataTable dtDLB = JBXX["完整数据集"] as DataTable;
                if (dtDLB == null || dtDLB.Rows.Count <= 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前交易账户的信息，不能进行交易操作。";
                    return dsreturn;
                }

                //string time = DateTime.Now.ToString("yyyyMMdd");
                //string dlyx = dtDLB.Rows[0]["B_DLYX"].ToString().Trim();//登录邮箱
                //string yhzh = dtDLB.Rows[0]["I_YHZH"].ToString().Trim();//银行账号
                //string ZQZJZH = dtDLB.Rows[0]["I_ZQZJZH"].ToString().Trim();//证券资金账号
                double BDYE = Convert.ToDouble(dtDLB.Rows[0]["B_ZHDQKYYE"].ToString().Trim() == "" ? "0.00" : dtDLB.Rows[0]["B_ZHDQKYYE"].ToString().Trim());//查询当前登录表可用余额 
                double DSFCGYE = Convert.ToDouble(dtDLB.Rows[0]["B_DSFCGKYYE"].ToString().Trim() == "" ? "0.00" : dtDLB.Rows[0]["B_DSFCGKYYE"].ToString().Trim());//第三方存管可用余额 

                #region//可转余额
                double KZYE = 0.00;//可转余额
                //登录账号可用余额小于银行端存管管理账户余额，可转账额取登录账号可用余额
                if (BDYE <= DSFCGYE)
                {
                    KZYE = BDYE;//可转余额
                }
                else
                {
                    KZYE = DSFCGYE;//可转余额
                }
                #endregion
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前帐户最大可转金额" + KZYE.ToString("#0.00") + "元";
                return dsreturn;
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
                return dsreturn;
            }
            
        }
        catch(Exception e)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = e.Message;
            return dsreturn;
        }
    }
       
}