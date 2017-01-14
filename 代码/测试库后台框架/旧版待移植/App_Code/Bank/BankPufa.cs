using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;


/// <summary>
/// 浦发银行接口调用
/// </summary>
public class BankPufa
{
    /// <summary>
    /// 统一调用前置机的方法
    /// </summary>
    /// <param name="dtParams">修改后的Dt</param>
    /// <returns></returns>
    /// 
    ws2013 ObjService = null; //为了初始化返回函数结构，调用函数：initReturnDataSet
    private DataSet Go(DataTable dtParams)
    {
        DataTable dt = BankHelper.GetConfig("浦发银行");
        if (dt.Rows.Count > 0)
        {
            string routing = dt.Rows[0]["Imv"].ToString() == "测试库" ? dt.Rows[0]["CStr"].ToString() : dt.Rows[0]["ZStr"].ToString();
            DataSet ds = new DataSet();
            DataTable hl = dtParams.Copy();
            ds.Tables.Add(hl);
            object[] _params = { ds };
            return BankHelper.Go(routing, "Reincarnation", _params);
        }
        else return null;

        //TestPF.Service aa = new TestPF.Service();
        
    }


    /// <summary>
    /// 浦发银行出金方法（调用参数：出金）
    /// </summary>
    /// <param name="dsParams">用户前台传递过来的参数</param>
    /// <param name="BI">用户信息实体</param>
    /// <returns>执行结果</returns>
    public DataSet chujin(DataSet dsParams, BankInit BI)
    {
        //1、XML->DataTable   DataTable.ReadXML(path);
        //2、替换Dt里的字段
        //3、调用Go方法（统一调用前置机的方法）  DataSet ds = Go(dt);
        //4、银行会返回结果，前置机会把结果返回到本地，dataset
        //5、根据结果（接口里的结构）进行下一步处理。

        #region
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化","8888" });
        
       
            DataTable dt = dsParams.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
               
                    #region//传过来的信息

                    string JSBH = dt.Rows[0]["买家角色编号"].ToString().Trim();
                    string zzlb = dt.Rows[0]["转账类别"].ToString().Trim();//转账方式
                    double je = Convert.ToDouble(dt.Rows[0]["金额"]);//转账金额
                    string ZQZJMM = dt.Rows[0]["交易资金密码"].ToString().Trim();//证券资金密码
                    #endregion

                    Hashtable JCYZ = PublicClass2013.GetParameterInfo();
                    Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
                    DataTable dtDLB = JBXX["完整数据集"] as DataTable;

                    if (dtDLB == null || dtDLB.Rows.Count <= 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前交易账户的信息，不能进行交易操作。";
                        return dsreturn;
                    }

                    #region//验证证券资金密码--模拟运行期间，暂时去掉这个验证

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
                    zjmm = dt.Rows[0]["银行取款密码"].ToString().Trim();
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
               

                if (zzlb == "商转银")
                {
                   
                    #region 新出金流程  wyh 2014.04.23 add
                    //1.首先更改证券段数据库
                    //2.如果证券段操作失败，直接返回失败信息。
                    //3.如果证券端操作成功，给银行发包。
                    //4.银行收到证券端发的包操作成功，出金流程结束。
                    //5.银行收到证券端发的包操作失败，证券段操作回滚---将流水明细表内信息移植到出入金异常表，同时改回用户余额。同时返回给用户失败信息。

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
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前帐户最大可转金额" + KZYE.ToString("#0.00") + "元！";//"您当前账户最大可转金额为" + KZYE.ToString("#0.00") + "元，本次转账金额过大，不能进行转账！";
                        return dsreturn;
                    }
                    else
                    {
                        //1.基本验证通过开始，证券段数据操作
                    
                            List<string> list = new List<string>();
                            string SelLSH = key;//自己的流水号 是否需要严格用流水号生成规则生成?
                            string BankLSH = "";//银行流水号 暂时获取不到
                            double YDje = je;//转帐金额
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
                            str = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE - " + YDje + ",B_DSFCGKYYE=B_DSFCGKYYE + " + YDje + " where B_DLYX='" + dlyx + "'";
                            list.Add(str);

                            #endregion
                            int i = 0;
                            try
                            {
                                i = DbHelperSQL.ExecuteSqlTran(list);

                            }
                            catch
                            {
                                //出错直接返回错误信息
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败，请重试！";
                                return dsreturn;
                            }


                            if (i > 0)
                            {
                                //3.证券段操作成功，给银行发包，此时出现任何错误都需要回滚证券段的操作

                                try
                                {
                                    DataTable dtSToBank;
                                    string JMZJMM = CallPinMac.EncryptString(zjmm);//加密后的交易密码

                                    #region//向银行发送指令进行商转银


                                    //读取xml的信息
                                    FileInfo FISZY = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPufa/券商发起-证券转银行#24702.xml"));
                                    DataTable dtBank = new DataTable();
                                    dtBank.ReadXml(FISZY.FullName);

                                        //DataTable dtBank = BankServicePF.N24702_SecuritiesToBank_Init();  //券商发起-证券转银行#24702.xml
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
                                          //DataSet dsSToBank = BankServicePF.N24702_SecuritiesToBank_Push(dtBank);
                                        //调用Go方法（统一调用前置机的方法）,获取返回值ds。
                                        DataSet dsSToBank = Go(dtBank);

                                       // dtSToBank = dsSToBank.Tables["result"];

                                        string strpass = "pass";
                                        if (dsSToBank.Tables["str"].Rows[0]["状态信息"].ToString().Equals("ok"))
                                        {
                                            dtSToBank = dsSToBank.Tables["result"];
                                        }
                                        else //银行端返回失败信息
                                        {
                                            dtSToBank = null;
                                            strpass = "no";
                                        }

                                        #endregion
                                  
                                    //判断银行操作是否成功
                                    if (dtSToBank.Rows[0]["ErrorNo"].ToString() == "0000" && strpass.Equals("pass"))  //如果银行操作成功
                                    {
                                        //把银行端返回的银行流水号更新到账户流水明细表里面的来源单号字段
                                        BankLSH = dtSToBank.Rows[0]["bSerial"].ToString();//银行流水号
                                        string StrSql = "update AAA_ZKLSMXB set LYDH = '" + BankLSH.Trim() + "' where Number='" + key.Trim() + "'";
                                        int a = DbHelperSQL.ExecuteSql(StrSql);

                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您成功转账" + YDje.ToString("#0.00") + "元!";
                                        return dsreturn;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            //回滚已经插入证券段数据表的数据
                                            ArrayList al = new ArrayList();
                                            WorkFlowModule Wcrjyc = new WorkFlowModule("AAA_CRJYCSJB");
                                            string Keyyc = Wcrjyc.numberFormat.GetNextNumberZZ("");
                                            //在出入金异常信息表插入数据
                                            string Strinsert = "INSERT INTO [AAA_CRJYCSJB] ([Number],[YSBH],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],BDCRJLSH,[CheckState],[CreateUser],[CreateTime],ZT) VALUES ('" + Keyyc + "','" + key + "','" + dlyx + "','" + JSZHLX + "','" + JSBH + "','券商出金','" + BankLSH + "','" + BDTime + "','" + yslx + "'," + YDje + ",'" + xm + "','" + xz + "','" + zy + "','','" + sjlx + "','" + SelLSH + "',0,'" + dlyx + "','" + BDTime + "','浦发出金异常')";
                                            al.Add(Strinsert);


                                            //从账款流水明细表删除信息
                                            string Strdele = " delete from  AAA_ZKLSMXB where Number='" + key.Trim() + "'";

                                            al.Add(Strdele);

                                            //更改登录表可用余额改回去。
                                            string strup = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE + " + YDje + ",B_DSFCGKYYE=B_DSFCGKYYE - " + YDje + " where B_DLYX='" + dlyx + "'";

                                            al.Add(strup);

                                            DbHelperSQL.ExecuteSqlTran(al);

                                            //出错直接返回错误信息
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败，请重试！";
                                            return dsreturn;

                                        }
                                        catch
                                        {
                                            //此时出错该如何处理?

                                        }

                                    }  //catch
                                }
                                catch
                                {
                                    //回滚已经插入证券段数据表的数据
                                    ArrayList al = new ArrayList();
                                    WorkFlowModule Wcrjyc = new WorkFlowModule("AAA_CRJYCSJB");
                                    string Keyyc = Wcrjyc.numberFormat.GetNextNumberZZ("");
                                    //在出入金异常信息表插入数据
                                    string Strinsert = "INSERT INTO [AAA_CRJYCSJB] ([Number],[YSBH],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM] ,[XZ],[ZY] ,[JKBH],[SJLX],BDCRJLSH,[CheckState],[CreateUser],[CreateTime],ZT) VALUES ('" + Keyyc + "','" + key + "','" + dlyx + "','" + JSZHLX + "','" + JSBH + "','券商出金','" + BankLSH + "','" + BDTime + "','" + yslx + "'," + YDje + ",'" + xm + "','" + xz + "','" + zy + "','','" + sjlx + "','" + SelLSH + "',0,'" + dlyx + "','" + BDTime + "','浦发出金异常')";
                                    al.Add(Strinsert);


                                    //从账款流水明细表删除信息
                                    string Strdele = " delete from  AAA_ZKLSMXB where Number='" + key.Trim() + "'";

                                    al.Add(Strdele);

                                    //更改登录表可用余额改回去。
                                    string strup = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE + " + YDje + ",B_DSFCGKYYE=B_DSFCGKYYE - " + YDje + " where B_DLYX='" + dlyx + "'";

                                    al.Add(strup);

                                    DbHelperSQL.ExecuteSqlTran(al);

                                    //出错直接返回错误信息
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败，请重试！！";
                                    return dsreturn;
                                }

                            }
                            else
                            {

                                //出错直接返回错误信息
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金转账失败！";
                                return dsreturn;
                            }
                      
                       
                    }

                    #endregion
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请选择正确的转账类型！";
                    return dsreturn;
                }

            }
            return dsreturn;
        
        #endregion
     
    }
    public DataSet chujin_Result(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();

        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "8888" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "出金浦发银行接口被关闭！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        return dsreturn;
    }



    /// <summary>
    /// 浦发银行出金方法（调用参数：银行端出金）(银行端发起)
    /// </summary>
    /// <returns></returns>
    public DataSet yinhangduanchujin(DataSet ds, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "初始化！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
   
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
                //测试暂时不用
                string ZQZJMM = CallPinMac.DecryptString(dt.Rows[0]["sCustPwd2"].ToString());
                //string ZQZJMM = dt.Rows[0]["sCustPwd2"].ToString();//证券资金密码
                string BankLSH = dt.Rows[0]["ExSerial"].ToString();//外部流水号
                double YDje = Convert.ToDouble(dt.Rows[0]["OccurBala"].ToString());//转帐金额


                DataSet dspp = null;
                DataSet dsmm = null;
                string sql = "";
                string strMM = "";
                if (CardType.ToString().Trim() == "1")
                {
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_SFZH='" + Card + "'";
                     sql = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_SFZH='" + Card + "'";//不验证密码

                     #region//验证证券资金密码,与账号与身份证是否匹陪。


                     //验证账号与身份证是否匹配,由于在ws2013已经验证账号是否有效，此处不再验证
                     dspp = DbHelperSQL.Query(sql);
                     if (dspp == null || dspp.Tables[0].Rows.Count <= 0) //不匹配
                     {
                         dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                         dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "身份证号码不符";
                         dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2009";
                         return dsreturn;
                     }

                     if (dspp.Tables[0].Rows.Count > 1)//出现多条匹配信息
                     {
                         dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                         dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号重复";
                         dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                         return dsreturn;
                     }

                     dsmm = DbHelperSQL.Query(strMM);
                     if (dsmm == null || dsmm.Tables[0].Rows.Count < 1) //身份证号与账号匹配，但是密码不匹配
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
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_YYZZZCH='" + Card + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_YYZZZCH='" + Card + "'";
                    #region//验证证券资金密码,与账号与身份证是否匹陪。


                    //验证账号与身份证是否匹配,由于在ws2013已经验证账号是否有效，此处不再验证
                    dspp = DbHelperSQL.Query(sql);
                    if (dspp == null || dspp.Tables[0].Rows.Count <= 0) //不匹配
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "身份证号码不符";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2009";
                        return dsreturn;
                    }

                    if (dspp.Tables[0].Rows.Count > 1)//出现多条匹配信息
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号重复";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                        return dsreturn;
                    }

                    dsmm = DbHelperSQL.Query(strMM);
                    if (dsmm == null || dsmm.Tables[0].Rows.Count < 1) //身份证号与账号匹配，但是密码不匹配
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
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_ZZJGDMZDM='" + Card + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_ZZJGDMZDM='" + Card + "'";
                    #region//验证证券资金密码,与账号与身份证是否匹陪。


                    //验证账号与身份证是否匹配,由于在ws2013已经验证账号是否有效，此处不再验证
                    dspp = DbHelperSQL.Query(sql);
                    if (dspp == null || dspp.Tables[0].Rows.Count <= 0) //不匹配
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "身份证号码不符";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2009";
                        return dsreturn;
                    }

                    if (dspp.Tables[0].Rows.Count > 1)//出现多条匹配信息
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号重复";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                        return dsreturn;
                    }

                    dsmm = DbHelperSQL.Query(strMM);
                    if (dsmm == null || dsmm.Tables[0].Rows.Count < 1) //身份证号与账号匹配，但是密码不匹配
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



                string dlyx = dsmm.Tables[0].Rows[0]["B_DLYX"].ToString();//登录邮箱
                string JSZHLX = dsmm.Tables[0].Rows[0]["B_JSZHLX"].ToString();//结算账户类型
                string JSBH = dsmm.Tables[0].Rows[0]["J_BUYJSBH"].ToString();//买家角色编号
                double SYJE = Convert.ToDouble(dsmm.Tables[0].Rows[0]["B_ZHDQKYYE"].ToString());//本地剩余可用金额
                double DSFCGKYYE = Convert.ToDouble(dsmm.Tables[0].Rows[0]["B_DSFCGKYYE"].ToString());//第三方存管可用余额
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
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2150";
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

    public DataSet yinhangduanchujin_Result(DataSet ds, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "8888" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "浦发银行接口被关闭！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        return dsreturn;
          
    }


    /// <summary>
    /// 浦发入金（调用参数：入金）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet rujin(DataSet dsParams, BankInit BI)
    {
        //入金方法调用
        #region
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "8888" });

       
            DataTable dt = dsParams.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {

                #region//传过来的信息
                string JSBH = dt.Rows[0]["买家角色编号"].ToString().Trim();
                string zzlb = dt.Rows[0]["转账类别"].ToString().Trim();//转账方式
                double je = Convert.ToDouble(dt.Rows[0]["金额"]);//转账金额
                string ZQZJMM = dt.Rows[0]["交易资金密码"].ToString().Trim();//证券资金密码
                #endregion

                Hashtable JCYZ = PublicClass2013.GetParameterInfo();
                Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
                DataTable dtDLB = JBXX["完整数据集"] as DataTable;

                if (dtDLB == null || dtDLB.Rows.Count <= 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前交易账户的信息，不能进行交易操作。";
                    return dsreturn;
                }


                string dlyx = dtDLB.Rows[0]["B_DLYX"].ToString().Trim();
                string yhzh = dtDLB.Rows[0]["I_YHZH"].ToString().Trim();//银行账号
                string khyh = dtDLB.Rows[0]["I_KHYH"].ToString().Trim();//开户银行
                string JSZHLX = dtDLB.Rows[0]["B_JSZHLX"].ToString().Trim();//结算账户类型
                string ZQZJZH = dtDLB.Rows[0]["I_ZQZJZH"].ToString().Trim();//证券资金账号
                double DQZHKYYE = Convert.ToDouble(dtDLB.Rows[0]["B_ZHDQKYYE"].ToString().Trim());//当前账户可用余额
                double DSFCGKYYE = Convert.ToDouble(dtDLB.Rows[0]["B_DSFCGKYYE"].ToString().Trim());//第三方存管可用余额

                string zjmm = "";
               
                zjmm = dt.Rows[0]["银行取款密码"].ToString().Trim(); //发给银行，银行来验证
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

                if (zzlb == "银转商")   //入金（先向银行发送包，银行处理完成后，再处理证券段数据）
                {
                    #region
                    DataTable dt1;
                    string JMZJMM = CallPinMac.EncryptString(zjmm);//加密后的交易密码
                   
                    try
                    { 
                        #region//向银行发送指令进行银转商
                        //读取xml的信息
                        FileInfo FIYZS = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPufa/券商发起-银行转证券#24701.xml"));
                        DataTable dtBank = new DataTable();
                        dtBank.ReadXml(FIYZS.FullName);

                        dtBank.Rows[0]["ExSerial"] = key;//外部流水号
                        dtBank.Rows[0]["bCustAcct"] = yhzh;//银行账号                    
                        dtBank.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号	
                        dtBank.Rows[0]["bCustPwd"] = JMZJMM;//银行密码
                        dtBank.Rows[0]["OccurBala"] = je;//转帐金额
                        dtBank.Rows[0]["Date"] = time;//交易日期

                        DataSet ds = Go(dtBank);

                        if (ds == null || ds.Tables.Count < 1)
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行返回的数据集为空。";
                            return dsreturn;
                        }

                        if (ds.Tables["str"].Rows[0]["状态信息"].ToString().Equals("ok"))
                        {
                            dt1 = ds.Tables["result"];
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ds.Tables["str"].Rows[0]["状态信息"].ToString();
                            return dsreturn;
                        }
                  
                        //wyh add 2014.04.21 
                        if (dt1 == null || dt1.Rows.Count < 0)
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行前置机返回的result数据集为空。";
                            return dsreturn;
                        }

                        #endregion
                    }
                    catch( Exception e)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = e.ToString();
                        return dsreturn;
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

                            #region//
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

                                //读取xml的信息
                                FileInfo FICYZS = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPufa/券商发起-冲银行转证券#24703.xml"));
                                DataTable dtCZQToBank = new DataTable();
                                dtCZQToBank.ReadXml(FICYZS.FullName);
                                dtCZQToBank.Rows[0]["ExSerial"] = key;//外部流水号
                                dtCZQToBank.Rows[0]["CorrectSerial"] = dt1.Rows[0]["ExSerial"].ToString();//被冲正的流水号：自己的流水号
                                dtCZQToBank.Rows[0]["bCustAcct"] = yhzh;//银行账号      
                                dtCZQToBank.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号	
                                dtCZQToBank.Rows[0]["Date"] = time;//交易日期

                                DataSet dsCZQToBank = Go(dtCZQToBank);

                                DataTable dtCSToBank = null;
                                if (dsCZQToBank.Tables["str"].Rows[0]["状态信息"].ToString().Equals("ok"))
                                {
                                    dtCSToBank = dsCZQToBank.Tables["result"];
                                }
                                else //银行冲失败怎么办？
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

                                if (dt == null || dt.Rows.Count < 1) //冲入金失败  冻结否？
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
                        catch (Exception)
                        {
                            #region//发起冲银行转证券操作（24703）
                            #region//向银行发送冲银行转证券操作（24703）的指令

                            //读取xml的信息
                            FileInfo FICYZS = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPufa/券商发起-冲银行转证券#24703.xml"));
                            DataTable dtCZQToBank = new DataTable();
                            dtCZQToBank.ReadXml(FICYZS.FullName);
                            dtCZQToBank.Rows[0]["ExSerial"] = key;//外部流水号
                            dtCZQToBank.Rows[0]["CorrectSerial"] = dt1.Rows[0]["ExSerial"].ToString();//被冲正的流水号：自己的流水号
                            dtCZQToBank.Rows[0]["bCustAcct"] = yhzh;//银行账号      
                            dtCZQToBank.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号	
                            dtCZQToBank.Rows[0]["Date"] = time;//交易日期

                            DataSet dsCZQToBank = Go(dtCZQToBank);

                            DataTable dtCSToBank = null;
                            if (dsCZQToBank.Tables["str"].Rows[0]["状态信息"].ToString().Equals("ok"))
                            {
                                dtCSToBank = dsCZQToBank.Tables["result"];
                            }
                            else //银行冲失败怎么办？
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

                            if (dt == null || dt.Rows.Count < 1) //冲入金失败  冻结否？
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
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = dt1.Rows[0]["ErrorNo"].ToString() + dt1.Rows[0]["ErrorInfo"].ToString();
                        return dsreturn;
                    }
                    #endregion
                    #endregion
                }  
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请选择正确的转账类型！";
                    return dsreturn;
                }

            }
            return dsreturn;
       
        #endregion

    }
    public DataSet rujin_Result(DataSet dsParams, BankInit BI)
    {

        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "入金时浦发银行接口被关闭！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        return dsreturn;
    }

    /// <summary>
    /// 浦发入金（调用参数：银行端入金）（银行端发起）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yinhangduanrujin(DataSet ds, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "初始化！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";

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
                string ZQZJMM = CallPinMac.DecryptString(dt.Rows[0]["sCustPwd2"].ToString());//证券资金密码 (模拟运行期间暂时去掉)
                string BankLSH = dt.Rows[0]["ExSerial"].ToString();//外部流水号
                double YDje = Convert.ToDouble(dt.Rows[0]["OccurBala"].ToString());//转帐金额

                string sql = "";
                string strMM = "";
                DataSet dspp = null;
                DataSet dsmm = null;
                //(模拟运行期间暂时去掉)
                if (CardType.ToString().Trim() == "1")
                {
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_SFZH='" + Card + "'";  
                    //(模拟运行期间暂时去掉)
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_SFZH='" + Card + "'";

                    #region//验证证券资金密码,与账号与身份证是否匹陪。


                    //验证账号与身份证是否匹配,由于在ws2013已经验证账号是否有效，此处不再验证
                     dspp = DbHelperSQL.Query(sql);
                    if (dspp == null || dspp.Tables[0].Rows.Count <= 0) //不匹配
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "身份证号码不符";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2009";
                        return dsreturn;
                    }

                    if (dspp.Tables[0].Rows.Count > 1)//出现多条匹配信息
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号重复";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                        return dsreturn;
                    }

                     dsmm = DbHelperSQL.Query(strMM);
                    if (dsmm == null || dsmm.Tables[0].Rows.Count < 1) //身份证号与账号匹配，但是密码不匹配
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
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_YYZZZCH='" + Card + "'"; 
                    // (模拟运行期间暂时去掉)
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_YYZZZCH='" + Card + "'";
                    #region//验证证券资金密码,与账号与身份证是否匹陪。


                    //验证账号与身份证是否匹配,由于在ws2013已经验证账号是否有效，此处不再验证
                     dspp = DbHelperSQL.Query(sql);
                    if (dspp == null || dspp.Tables[0].Rows.Count <= 0) //不匹配
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "身份证号码不符";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2009";
                        return dsreturn;
                    }

                    if (dspp.Tables[0].Rows.Count > 1)//出现多条匹配信息
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号重复";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                        return dsreturn;
                    }

                     dsmm = DbHelperSQL.Query(strMM);
                    if (dsmm == null || dsmm.Tables[0].Rows.Count < 1) //身份证号与账号匹配，但是密码不匹配
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
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_ZZJGDMZDM='" + Card + "'";  
                    //(模拟运行期间暂时去掉)
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_YHZH='" + YHZH + "' and I_ZQZJZH='" + ZQZH + "' AND I_ZZJGDMZDM='" + Card + "'";
                    #region//验证证券资金密码,与账号与身份证是否匹陪。


                    //验证账号与身份证是否匹配,由于在ws2013已经验证账号是否有效，此处不再验证
                     dspp = DbHelperSQL.Query(sql);
                    if (dspp == null || dspp.Tables[0].Rows.Count <= 0) //不匹配
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "身份证号码不符";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2009";
                        return dsreturn;
                    }

                    if (dspp.Tables[0].Rows.Count > 1)//出现多条匹配信息
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号重复";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                        return dsreturn;
                    }

                     dsmm = DbHelperSQL.Query(strMM);
                    if (dsmm == null || dsmm.Tables[0].Rows.Count < 1) //身份证号与账号匹配，但是密码不匹配
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


                string dlyx = dsmm.Tables[0].Rows[0]["B_DLYX"].ToString();//登录邮箱
                string JSZHLX = dsmm.Tables[0].Rows[0]["B_JSZHLX"].ToString();//结算账户类型
                string JSBH = dsmm.Tables[0].Rows[0]["J_BUYJSBH"].ToString();//买家角色编号
                double SYJE = Convert.ToDouble(dsmm.Tables[0].Rows[0]["B_ZHDQKYYE"].ToString());//剩余可用金额
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
                        if (BDLSH == null)
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
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "连接证券失败";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2405";
            }
        }
        catch (Exception e)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券平台错误";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        }
        return dsreturn;
    }
    public DataSet yinhangduanrujin_Result(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "浦发银行端口已关闭！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        return dsreturn;
    }


    /// <summary>
    /// 券商查询银行资金余额（调用参数：余额）
    /// (原始应为yinhangyue，但是“行”是多音字，系统会自动翻译成yinxingyue)
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yue(DataSet dsParams, BankInit BI)
    {
        //查询银行资金余额调用
        return null;
    }
    public DataSet yue_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 银行查询券商资金余额（银行发起）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet quanshangyuechaxun(DataSet ds, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "初始化！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
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
                //string ZQZJMM = dt.Rows[0]["sCustPwd2"].ToString();//证券资金密码
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
    public DataSet quanshangyuechaxun_Result(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "浦发银行接口关闭！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        return dsreturn;
    }


    /// <summary>
    /// 变更银行账户（调用参数：变更账户） 券商发起
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet biangengzhanghu(DataSet dsParams, BankInit BI)
    {
        //wyh 2014.04.21 add
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "浦发数据未为被填充！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";

        try
        {
            DataTable dt = dsParams.Tables[0];
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
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + JBXX["冻结功能项"].ToString();
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

                        #region//向银行发送指令
                        //读取xml的信息
                        FileInfo FIBGYH = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPufa/券商发起-变更银行账户#26710.xml"));
                        DataTable dtBank = new DataTable();
                        dtBank.ReadXml(FIBGYH.FullName);

                        DataTable dtBankYE = BankServicePF.N26710_ModifyBankCard_Init();
                        dtBankYE.Rows[0]["ExSerial"] = BankLSH;//外部流水号
                        dtBankYE.Rows[0]["bCustAcct"] = OldYHZH;//银行账号                    
                        dtBankYE.Rows[0]["sCustAcct"] = ZQZJZH;//证券资金账号
                        dtBankYE.Rows[0]["bNewAcct"] = NewYHZH;//新银行账号
                        dtBankYE.Rows[0]["Date"] = time;//交易日期
                        DataSet ds = Go(dtBankYE);//BankServicePF.N26710_ModifyBankCard_Push(dtBankYE);
                        DataTable dt1 = null;
                        if (ds.Tables["str"].Rows[0]["状态信息"].ToString().Equals("ok"))
                        {
                             dt1 = ds.Tables["result"];
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = dt1.Rows[0]["ErrorNo"].ToString() + dt1.Rows[0]["ErrorInfo"].ToString();
                            return dsreturn;
                        }
                        #endregion


                        if (dt1 == null || dt1.Rows.Count < 0)  // wyh 2014.04.21 add
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行反馈信息出错!";
                            return dsreturn;
                        }


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
    public DataSet biangengzhanghu_Result(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "浦发接口被关闭测试！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        return dsreturn;
    }



    /// <summary>
    /// 变更银行账户（调用参数：变更账户） 银行端发起  wyh  2014.04.17 add
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yinhangduanhuanka(DataSet dt, BankInit BI)
    {

        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "数据未被填充！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";

        try
        {
            Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
            if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2032";
                return dsreturn;
            }

            if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2140";
                return dsreturn;
            }

            if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2140";
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
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "原银行账号错误";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2301";
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
    public DataSet yinhangduanhuanka_Result(DataSet dt, BankInit BI)//冲银行端出金
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "浦发银行接口关闭！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        return dsreturn;
    }

    /// <summary>
    /// 冲银行转证券(调用参数：冲银转证)
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet chongyinzhuanzheng(DataSet dsParams, BankInit BI)
    {
        //冲银转证
        return null;
    }
    public DataSet chongyinzhuanzheng_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }


    /// <summary>
    /// 冲银行转证券(调用参数：冲银行端入金) 银行发起 wyh 2014.04.17 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet chongyinhangduanrujin(DataSet ds, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到相关数据！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
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

    public DataSet chongyinhangduanrujin_Result(DataSet ds, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到相关数据！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        return dsreturn;
    }



    /// <summary>
    /// 冲证券转银行（调用参数：冲证转银）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet chongzhengzhuanyin(DataSet dsParams, BankInit BI)
    {
        //冲证转银
        return null;
    }
    public DataSet chongzhengzhuanyin_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    ///  wyh 2014.04.18 冲证券转银行（调用参数：冲银行端出金）---银行端发起
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet chongyinhangduanchujin(DataSet ds, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获取到数据！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
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
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "冲正金额不符";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2008";
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
    public DataSet chongyinhangduanchujin_Result(DataSet ds, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "浦发银行接口关闭！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        return dsreturn;
    }


    /// <summary>
    /// 变更存管银行（调用参数：变更银行）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet biangengyinhang(DataSet dsParams, BankInit BI)
    {
        //变更存管银行
        return null;
    }
    public DataSet biangengyinhang_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 签约（调用参数：客户签约）（券商发起）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet qianyueyuzhiding(DataSet dsParams, BankInit BI)
    {
        //客户存管签约预指定  wyh 2014.04.14 add
        ws2013 ws = new ws2013();
        DataSet dsReturn = ws.initReturnDataSet();
        dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //读取xml的信息
        FileInfo FIQYYZD = new FileInfo(HttpContext.Current.Server.MapPath("BankXML/XMLPufa/券商发起-客户存管签约#26701.xml"));
        DataTable dt = new DataTable();
        dt.ReadXml(FIQYYZD.FullName);

        dt.Rows[0]["ExSerial"] = dsParams.Tables[0].Rows[0]["业务流水号"].ToString();//外部流水号
        dt.Rows[0]["sBranchId"] = "0000";//营业部编码
        dt.Rows[0]["Name"] = dsParams.Tables[0].Rows[0]["客户姓名"].ToString();
        dt.Rows[0]["OccurBala"] = "0";//客户姓名
        string strzclb = dsParams.Tables[0].Rows[0]["注册类别"].ToString();
        string  strCustProperty = "";
        string  strCardType = "";//身份证
        switch (strzclb)
        {
            case "radZRR": //自然人
                strCustProperty = "0";
                strCardType = "1";//身份证
                break;
            case "radDW": //单位
                strCustProperty = "1";
                strCardType = "8";//组织机构代码
                break;
            default:
                
                break;
        }

        dt.Rows[0]["sCustAcct"] = dsParams.Tables[0].Rows[0]["证券资金账号"].ToString();
        dt.Rows[0]["sCustProperty"] = strCustProperty;
        dt.Rows[0]["CardType"] = strCardType;
        dt.Rows[0]["Card"] = dsParams.Tables[0].Rows[0]["客户证件号"].ToString(); //7.	客户证件号
        dt.Rows[0]["Date"] = DateTime.Now.ToString("yyyyMMdd");

         DataSet dsResult = Go(dt);

         if (dsResult != null)
            {
                if (dsResult.Tables["str"].Rows[0]["状态信息"] == "ok")
                {
                    dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "签约预指定成功！";
                }
                else
                {
                    dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = dsReturn.Tables["str"].Rows[0]["状态信息"].ToString();
                }
            }
            else
            {
                dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "前置机执行出错！";
            }

           return dsReturn;
         
    }
    public DataSet qianyueyuzhiding_Result(DataSet dsParams, BankInit BI)
    {
        //客户存管签约
        ws2013 ws = new ws2013();
        DataSet dsReturn = ws.initReturnDataSet();
        dsReturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        dsReturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsReturn.Tables["返回值单条"].Rows[0]["提示文本"] = "浦发银行接口已关闭！";
        return dsReturn;
    }
    
    /// <summary>
    /// 客户签约确认（银行发起） wyh 2014.04.016 add
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet kehuqianyuequeren(DataSet ds, BankInit BI)
    {
       
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "初始化！";
        DataTable dtUserInfo = ds.Tables["UINFO"];
        try
        {
            if (dtUserInfo.Rows.Count > 0)
            {
                string CardType = dtUserInfo.Rows[0]["CardType"].ToString().Trim();//证件类型
                string ZQZJMM = CallPinMac.DecryptString(dtUserInfo.Rows[0]["sCustPwd2"].ToString().Trim());//证件资金密码
                string sql = "";
                string strMM = "";

                if (CardType == "1")
                {
                   
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' and I_ZQZJZH='" + dtUserInfo.Rows[0]["sCustAcct"].ToString() + "' AND I_SFZH='" + dtUserInfo.Rows[0]["Card"].ToString() + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_ZQZJZH='" + dtUserInfo.Rows[0]["sCustAcct"].ToString() + "' AND I_SFZH='" + dtUserInfo.Rows[0]["Card"].ToString() + "'";
    
                    #region//验证证券资金密码,与账号与身份证是否匹陪。

                   
                    //验证账号与身份证是否匹配,由于在ws2013已经验证账号是否有效，此处不再验证
                    DataSet dspp = DbHelperSQL.Query(sql);
                    if (dspp == null || dspp.Tables[0].Rows.Count <= 0) //不匹配
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "身份证号码不符";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2009";
                        return dsreturn;
                    }

                    if (dspp.Tables[0].Rows.Count > 1)//出现多条匹配信息
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号重复";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                        return dsreturn;
                    }

                    DataSet dsmm = DbHelperSQL.Query(strMM);
                    if (dsmm == null || dsmm.Tables[0].Rows.Count < 1) //身份证号与账号匹配，但是密码不匹配
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

                    #region//验证证券资金密码,与账号与身份证是否匹陪。


                    //验证账号与身份证是否匹配,由于在ws2013已经验证账号是否有效，此处不再验证
                    DataSet dspp = DbHelperSQL.Query(sql);
                    if (dspp == null || dspp.Tables[0].Rows.Count <= 0) //不匹配
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "组织机构代码号码不符";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2009";
                        return dsreturn;
                    }

                    if (dspp.Tables[0].Rows.Count > 1)//出现多条匹配信息
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号重复";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                        return dsreturn;
                    }

                    DataSet dsmm = DbHelperSQL.Query(strMM);
                    if (dsmm == null || dsmm.Tables[0].Rows.Count < 1) //身份证号与账号匹配，但是密码不匹配
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }


                    #endregion

                    #region
                    /*
                    //验证证券资金密码
                    if (DbHelperSQL.Query(strMM).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }

                    
                     * */
                    #endregion

                }
                else if (CardType == "9")
                {
                    strMM = "SELECT * FROM AAA_DLZHXXB WHERE B_JSZHMM='" + ZQZJMM + "' I_ZQZJZH='" + dtUserInfo.Rows[0]["sCustAcct"].ToString() + "' AND I_YYZZZCH='" + dtUserInfo.Rows[0]["Card"].ToString() + "'";
                    sql = "SELECT * FROM AAA_DLZHXXB WHERE I_ZQZJZH='" + dtUserInfo.Rows[0]["sCustAcct"].ToString() + "' AND I_YYZZZCH='" + dtUserInfo.Rows[0]["Card"].ToString() + "'";

                    #region//验证证券资金密码,与账号与身份证是否匹陪。


                    //验证账号与身份证是否匹配,由于在ws2013已经验证账号是否有效，此处不再验证
                    DataSet dspp = DbHelperSQL.Query(sql);
                    if (dspp == null || dspp.Tables[0].Rows.Count <= 0) //不匹配
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "营业执照号码不符";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2009";
                        return dsreturn;
                    }

                    if (dspp.Tables[0].Rows.Count > 1)//出现多条匹配信息
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账号重复";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                        return dsreturn;
                    }

                    DataSet dsmm = DbHelperSQL.Query(strMM);
                    if (dsmm == null || dsmm.Tables[0].Rows.Count < 1) //身份证号与账号匹配，但是密码不匹配
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }


                    #endregion


                    #region//验证证券资金密码
                    /*
                    if (DbHelperSQL.Query(strMM).Tables[0].Rows.Count == 0 && DbHelperSQL.Query(sql).Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证券资金密码错误";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2001";
                        return dsreturn;
                    }
                    */
                    #endregion
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "证件类型无效";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                    return dsreturn;
                }

               

                //验证各机构审核是否通过
                DataTable dtUInPT = DbHelperSQL.Query(sql).Tables[0];
                if (dtUInPT.Rows.Count == 1)
                {
                    if (dtUInPT.Rows[0]["S_SFYBJJRSHTG"].ToString() == "否" || dtUInPT.Rows[0]["S_SFYBFGSSHTG"].ToString() == "否")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方在交易所审核未通过！";
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
                            sw.WriteLine("查询语句：" + sql + "   |   更新语句：" + sqlUpdate + "   |   异常信息：" + ex.Message);
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
    public DataSet kehuqianyuequeren_Result(DataSet dsParams, BankInit BI)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行接口关闭！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
        return dsreturn;
    }

    /// <summary>
    /// 客户登记激活（调用参数：激活）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet jihuo(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet jihuo_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 结息（调用参数：结息）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet jiexi(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet jiexi_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 解约（调用参数：解约）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet jieyue(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet jieyue_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 客户预指定存管签约确认（调用参数：预指定确认）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yuzhidingqueren(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet yuzhidingqueren_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 取银行转账明细（调用参数：银行转帐明细）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yinhangzhuanzhangmingxi(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet yinhangzhuanzhangmingxi_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 修改客户资料（调用参数：修改资料）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet xiugaiziliao(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet xiugaiziliao_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 银行转证券（调用参数：银转证）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yinzhuanzheng(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet yinzhuanzheng_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 证券转银行（调用参数：证转银）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet zhengzhuanyin(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet zhengzhuanyin_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }


    /// <summary>
    /// 检测指定文件是否存在,如果存在则返回true。
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>        
    public bool IsExistFile(string filePath)
    {
        return File.Exists(filePath);
    }
    /// <summary>
    /// 检测指定目录是否存在
    /// </summary>
    /// <param name="directoryPath">目录的绝对路径</param>
    /// <returns></returns>
    public bool IsExistDirectory(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }


    /// <summary>
    /// 初始化返回值数据集,执行结果只有两种ok和err
    /// </summary>
    /// <returns></returns>
    public DataSet initReturnDataSet()
    {
        DataSet ds = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.TableName = "返回值单条";
        auto2.Columns.Add("执行结果");
        auto2.Columns.Add("提示文本");
        auto2.Columns.Add("附件信息1");
        auto2.Columns.Add("附件信息2");
        auto2.Columns.Add("附件信息3");
        auto2.Columns.Add("附件信息4");
        auto2.Columns.Add("附件信息5");
        ds.Tables.Add(auto2);
        return ds;
    }


}