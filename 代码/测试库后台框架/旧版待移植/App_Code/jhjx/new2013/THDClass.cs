using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;

/// <summary>
/// THDClass 的摘要说明
/// </summary>
public class THDClass
{
	public THDClass()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
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
    //获取买家最近一次使用的发票信息
    public DataSet GetFPXX(string DLYX, DataSet returnDS)
    {
        //DataSet returnDS = initReturnDataSet();
        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        try
        {
            //string strSQL = "select top 1 T_SHXXDZ 收货详细地址,T_SHRXM 收货人姓名,T_SHRLXFS 收货人联系方式,T_FPLX 发票类型,T_FPTT 发票抬头,c.I_SWDJZSH 税号,T_KHYH 开户银行,T_YHZH 银行账号,T_DWDZ 单位地址 from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number left join AAA_DLZHXXB c on b.Y_YSYDDDLYX=c.B_DLYX where b.Y_YSYDDDLYX='" + DLYX.Trim() + "' order by T_THDXDSJ desc";
            string strSQL = "select I_SWDJZSH 税号,'收货详细地址'=(select top 1 T_SHXXDZ from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where b.Y_YSYDDDLYX='" + DLYX.Trim() + "' order by T_THDXDSJ desc),'收货人姓名'=(select top 1 T_SHRXM from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where b.Y_YSYDDDLYX='" + DLYX.Trim() + "' order by T_THDXDSJ desc),'收货人联系方式'=(select top 1 T_SHRLXFS from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where b.Y_YSYDDDLYX='" + DLYX.Trim() + "' order by T_THDXDSJ desc),'发票类型'=(select top 1 T_FPLX from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where b.Y_YSYDDDLYX='" + DLYX.Trim() + "' order by T_THDXDSJ desc),'发票抬头'=(select top 1 T_FPTT from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where b.Y_YSYDDDLYX='" + DLYX.Trim() + "' order by T_THDXDSJ desc),'开户银行'=(select top 1 T_KHYH from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where a.T_FPLX='增值税专用发票' and b.Y_YSYDDDLYX='" + DLYX.Trim() + "' order by T_THDXDSJ desc),'银行账号'=(select top 1 T_YHZH from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where a.T_FPLX='增值税专用发票' and b.Y_YSYDDDLYX='" + DLYX.Trim() + "' order by T_THDXDSJ desc),'单位地址'=(select top 1 T_DWDZ from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH=b.Number where a.T_FPLX='增值税专用发票' and b.Y_YSYDDDLYX='" + DLYX.Trim() + "' order by T_THDXDSJ desc) from AAA_DLZHXXB c where c.B_DLYX='" + DLYX.Trim() + "'";
            DataSet ds = DbHelperSQL.Query(strSQL);
            DataTable dt = ds.Tables[0].Copy();
            dt.TableName = "主表";
            returnDS.Tables.Add(dt);
        }
        catch(Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";   
        }
        return returnDS;
    }
    //保存提货单
    public DataSet SaveTHD(DataSet ds, DataSet returnDS)
    {        
        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        try
        {
            
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string JSBH = ds.Tables[0].Rows[0]["买家角色编号"].ToString();//买家角色编号
                string DLYX = ds.Tables[0].Rows[0]["登陆邮箱"].ToString();//登陆邮箱
                string JSZHLX = ds.Tables[0].Rows[0]["结算账户类型"].ToString();//结算账户类型
                double DJJE = Convert.ToInt32(ds.Tables[0].Rows[0]["提货数量"].ToString()) * Convert.ToDouble(ds.Tables[0].Rows[0]["中标单价"].ToString());//冻结货款金额
                string ZBDBBH = "";//中标定标信息表编号
                ZBDBBH = ds.Tables[0].Rows[0]["中标定标信息表编号"].ToString();//中标定标信息表编号


                #region//基础验证
                Hashtable JCYZ = PublicClass2013.GetParameterInfo();
                Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
                if (JBXX["交易账户是否开通"].ToString() == "否")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                    return returnDS;
                }
 

                if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                    return returnDS;
                }
                if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(工作时间)，暂停交易！";
                    return returnDS;
                }


                if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                    return returnDS;
                }


                if (JBXX["是否休眠"].ToString() == "是")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                    return returnDS;
                }
                DataTable dtall = JBXX["完整数据集"] as DataTable;
                if (dtall.Rows.Count > 0)
                {
                    if (dtall.Rows[0]["B_SFDJ"].ToString() == "是")
                    {
                        if (dtall.Rows[0]["DJGNX"].ToString().Contains("下达提货单"))
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + dtall.Rows[0]["DJGNX"].ToString();
                            return returnDS;
                        }
                    }
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return returnDS;
                }
                #endregion

                #region//验证合同状态
                string strSQLDB = "  select d.Number,d.Z_HTZT,d.Z_ZBSL,d.Z_YTHSL,d.Z_DJJE,d.Z_HTQX,d.Z_HTBH from AAA_ZBDBXXB d where d.Number='" + ZBDBBH.Trim() + "'";
                DataSet zt = DbHelperSQL.Query(strSQLDB);
                if (zt != null && zt.Tables[0].Rows.Count>0)
                {
                    switch (zt.Tables[0].Rows[0]["Z_HTZT"].ToString())
                    {
                        case "定标":
                            if (Convert.ToInt32(zt.Tables[0].Rows[0]["Z_YTHSL"].ToString()) >= Convert.ToInt32(zt.Tables[0].Rows[0]["Z_ZBSL"].ToString()))
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "合同编号为：" + zt.Tables[0].Rows[0]["Z_HTBH"].ToString() + " 的定标信息已提货完成，不能下达提货单。";
                                return returnDS;
                            }
                            break;
                        default:
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "该中标定标信息不能生成提货单！";
                            return returnDS;
                    }
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到中标定标信息，不能生成提货单！";
                    return returnDS;
                }
                #endregion

                #region//验证履约保证金是否不足60%
                //找出履约保证金不足需要废标的标的
                double lybzjbzbl = Convert.ToDouble(JCYZ["履约保证金不足比率"]);

                DataSet dsFB = DbHelperSQL.Query("select * from ( select '履约保证金金额'=Z_LYBZJJE, '两项延迟总金额'=isnull( (select sum(LS.JE) from  AAA_ZKLSMXB as LS where LS.XM='违约赔偿金' and  LS.XZ in ( '超过最迟发货日后录入发货信息','发货单生成后5日内未录入发票邮寄信息') and LS.SJLX='预' and LS.LYDH in ( select TTT.Number from AAA_THDYFHDXXB as TTT where  TTT.ZBDBXXBBH = ZZZ.Number ) ),0.00),'是否已解冻过订金'=isnull((select top 1 ZK.LYDH from AAA_ZKLSMXB as ZK where ZK.LYDH=ZZZ.Number and ZK.LYYWLX='AAA_ZBDBXXB' and ZK.XM='订金' and ZK.XZ='合同期内下达完全部提货单解冻' and ZK.SJLX='实' ),'否') , * from AAA_ZBDBXXB as ZZZ where ZZZ.Z_HTZT = '定标' ) as tab11 where convert(float,履约保证金金额-两项延迟总金额)/convert(float,履约保证金金额) < convert(float," + lybzjbzbl.ToString() + ") and Z_HTBH='" + zt.Tables[0].Rows[0]["Z_HTBH"].ToString() + "' ");
                if (dsFB != null && dsFB.Tables[0].Rows.Count>0)
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "抱歉，该《电子购货合同》因履约保证金不足已废标，您无法下达提货单！";
                    return returnDS;
                }

                #endregion


                #region//资金验证
                ClassMoney2013 money = new ClassMoney2013();
                double KYYE = money.GetMoneyT(DLYX, "");//账户当前可用余额
                if (KYYE >= DJJE)
                {
                    if (ds.Tables[0].Rows[0]["操作"].ToString() == "只验证钱")//只验证钱
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "onlymoney";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您的提货单一经提交将冻结" + DJJE.ToString("#0.00") + "元的货款。";
                        return returnDS;
                    }
                    else if (ds.Tables[0].Rows[0]["操作"].ToString() == "实际操作")//实际操作：插入提货单、写入资金流水明细、返写中标定标信息表已提货数量、返写登录表可用余额
                    {
                        #region//插入准备字段
                        string number = PublicClass2013.GetNextNumberZZ("AAA_THDYFHDXXB", "");                        
                        string ZBDJ = "";//中标单价
                        int THJJPLS = 0;//提货的经济批量数
                        int THSL = 0;//提货数量
                        string newZCFHR = "";//最迟发货日
                        string SHXXDZ = "";//收货详细地址
                        string SHRXM = "";//收货人姓名
                        string SHLXFS = "";//收货人联系方式
                        //string DJHKJE = "";//冻结货款金额
                        string FPLX = "";//发票类型
                        string FPTT = "";//发票抬头
                        string SH = "";//税号
                        string KHYH = "";//开户银行
                        string YHZH = "";//银行账号
                        string DWDZ = "";//单位地址
                        string time = DateTime.Now.ToString();
                        int ythsl = 0;//已提货数量
                        int newythsl = 0;//下达新的提货单后，已提货数量
                        int zbsl = 0;//中标数量
                        //string htqx = "";//合同期限
                        string htjsrq = "";//合同结束日期
                        string htbh = "";//合同编号                        
                        
                        THJJPLS = Convert.ToInt32(ds.Tables[0].Rows[0]["提货的经济批量数"].ToString());//提货的经济批量数
                        //THSL = Convert.ToInt32(ds.Tables[0].Rows[0]["提货数量"].ToString());//提货数量
                        int jjpl = Convert.ToInt32(ds.Tables[0].Rows[0]["经济批量"].ToString());//卖家设定的提货经济批量
                        string DBTime = "";//定标时间
                        SHXXDZ = ds.Tables[0].Rows[0]["收货详细地址"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["收货详细地址"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["收货详细地址"].ToString();//收货详细地址
                        SHRXM = ds.Tables[0].Rows[0]["收货人姓名"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["收货人姓名"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["收货人姓名"].ToString();//收货人姓名
                        SHLXFS = ds.Tables[0].Rows[0]["收货人联系方式"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["收货人联系方式"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["收货人联系方式"].ToString();//收货人联系方式
                        //DJHKJE = DJJE.ToString();//冻结货款金额
                        FPLX = ds.Tables[0].Rows[0]["发票类型"].ToString();//发票类型
                        FPTT = ds.Tables[0].Rows[0]["发票抬头"].ToString();//发票抬头
                        SH = ds.Tables[0].Rows[0]["税号"].ToString();//税号
                        KHYH = ds.Tables[0].Rows[0]["开户银行"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["开户银行"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["开户银行"].ToString();//开户银行
                        YHZH = ds.Tables[0].Rows[0]["银行账号"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["银行账号"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["银行账号"].ToString();//银行账号
                        DWDZ = ds.Tables[0].Rows[0]["单位地址"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["单位地址"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["单位地址"].ToString();//单位地址
                        htbh = ds.Tables[0].Rows[0]["合同编号"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["合同编号"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["合同编号"].ToString();//合同编号

                        double DayMaxFHL = 0.00;//最高发货量
                        #region//根据中标定标信息表编号查询中标定标信息表中的可提货数量、已提货数量、中标数量、合同期限
                        string strDB = "select convert(int,Z_ZBSL)-convert(int,Z_YTHSL) 可提货数量,Z_YTHSL 已提货数量,Z_ZBSL 中标数量,Z_HTQX 合同期限,Z_HTJSRQ 合同结束日期,Z_RJZGFHL 日均最高发货量,Z_DBSJ 定标时间,Z_ZBJG from AAA_ZBDBXXB where Z_HTZT='定标' and Number='" + ZBDBBH.Trim() + "'";
                        DataSet dsDB = DbHelperSQL.Query(strDB);
                        if (dsDB != null && dsDB.Tables[0].Rows.Count > 0)
                        {
                            ZBDJ = dsDB.Tables[0].Rows[0]["Z_ZBJG"].ToString();//中标单价
                            DBTime = dsDB.Tables[0].Rows[0]["定标时间"].ToString();
                            ythsl = Convert.ToInt32(dsDB.Tables[0].Rows[0]["已提货数量"].ToString());//已提货数量
                            zbsl = Convert.ToInt32(dsDB.Tables[0].Rows[0]["中标数量"].ToString());//中标数量                            
                            htjsrq = dsDB.Tables[0].Rows[0]["合同结束日期"].ToString();//合同结束日期
                            DayMaxFHL = Convert.ToDouble(dsDB.Tables[0].Rows[0]["日均最高发货量"].ToString());//最高发货量
                            Int32 kthsl = Convert.ToInt32(dsDB.Tables[0].Rows[0]["可提货数量"].ToString());//可提货数量
                            Int32 thbs = Convert.ToInt32(THJJPLS);//提货笔数
                            if (kthsl < jjpl) //可提货量不足一个经济批量
                            {
                                THSL = kthsl;
                            }
                            else if (kthsl < thbs * jjpl) //可提货量不足以支撑录入的提货笔数
                            {
                                THSL = kthsl;
                            }
                            else if (kthsl - thbs * jjpl < jjpl)//录入提货笔数后，剩余可提货数量小于经济批量
                            {
                                THSL = kthsl;
                            }
                            else
                            {
                                THSL = thbs * jjpl;
                            }
                            newythsl = ythsl + THSL;//下达新的提货单后，已提货数量
                                
                        }
                        else
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            //returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到单号为：" + ZBDBBH + " 的定标信息，不能下达提货单。";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到中标定标信息，不能下达提货单。";
                            return returnDS;
                        }
                        double kjje = Convert.ToDouble(ZBDJ) * THSL;//扣减金额
                        #endregion

                        #region//最迟发货日
                        int ZWZCFHRYPZL = 0;//最晚最迟发货日已排总量  
                        string ZCFHR = "";//相应标的的最晚最迟发货日
                        double newGZWFHRGTHDYPL = 0.00;//该最晚发货日该提货单已排量
                        if (dsDB.Tables[0].Rows[0]["合同期限"].ToString() == "即时")
                        {
                            newZCFHR = Convert.ToDateTime(time).AddDays(1).ToString();//最迟发货日=下单时间+1天；
                            newGZWFHRGTHDYPL = DayMaxFHL;//该最晚发货日该提货单已排量=日均最高发货量；
                            //下单时间大于等于定标时间+3天（动态参数设定表里的“即时合同定标下提货单期限”值）；否则不允许下达提货单；
                            if (Convert.ToDateTime(time) >= Convert.ToDateTime(DBTime).AddDays(Convert.ToInt32(JCYZ["即时合同定标下提货单期限"].ToString())))
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "不能下达提货单！\n\r此提货单定标时间为：" + DBTime + "；\n\r下达提货单的时间必须小于定标时间后" + JCYZ["即时合同定标下提货单期限"].ToString() + "天！";
                                return returnDS;
                            }
                        }
                        else
                        {
                            string strSelTHD = "select isnull(sum(isnull(T_GZWFHRGTHDYPL,0)),0) 最晚最迟发货日已排总量,max(T_ZCFHR) 最迟发货日 from AAA_THDYFHDXXB where T_ZCFHR =(select max(T_ZCFHR)from AAA_THDYFHDXXB  where ZBDBXXBBH='" + ZBDBBH + "') and ZBDBXXBBH='" + ZBDBBH + "'";//对应标的的最晚最迟发货日已排总量、最迟发货日
                            DataSet dsTHD = DbHelperSQL.Query(strSelTHD);
                            if (dsTHD != null && dsTHD.Tables[0].Rows.Count > 0)
                            {
                                ZWZCFHRYPZL = Convert.ToInt32(dsTHD.Tables[0].Rows[0]["最晚最迟发货日已排总量"].ToString());
                                ZCFHR = dsTHD.Tables[0].Rows[0]["最迟发货日"].ToString();//最迟发货日
                            }
                            double ZWZCFHRWPL = DayMaxFHL - ZWZCFHRYPZL;//最晚的最迟发货日未排满量                            
                            if (ZCFHR == "")//没有最晚日期
                            {
                                if (THSL * 0.8 <= DayMaxFHL)
                                {
                                    newZCFHR = Convert.ToDateTime(time).ToString();//最迟发货日
                                    if (THSL >= DayMaxFHL)
                                    {
                                        newGZWFHRGTHDYPL = DayMaxFHL;//该最晚发货日该提货单已排量
                                    }
                                    else
                                    {
                                        newGZWFHRGTHDYPL = THSL;//该最晚发货日该提货单已排量
                                    }
                                }
                                else
                                {
                                    double pxDays = Math.Round((THSL - DayMaxFHL) / DayMaxFHL, 0) + 1;//排期天数
                                    newZCFHR = Convert.ToDateTime(time).AddDays(pxDays).ToString();//最迟发货日
                                    newGZWFHRGTHDYPL = (THSL - DayMaxFHL) % DayMaxFHL;//该最晚发货日该提货单已排量
                                }
                            }
                            else//有最晚日期
                            {
                                #region//最迟发货日最晚日期晚于本次提货单提交日期
                                if (Convert.ToDateTime(ZCFHR) > Convert.ToDateTime(time))
                                {
                                    if (THSL - ZWZCFHRWPL <= 0)//若本次提货量-Z<= 0，则最迟发货日即，最晚最迟发货日，同时以G = 本次提货量更新数据库。
                                    {
                                        newZCFHR = ZCFHR;//最迟发货日
                                        newGZWFHRGTHDYPL = THSL;//该最晚发货日该提货单已排量
                                    }
                                    else
                                    {
                                        double pxDays = Math.Round((THSL - ZWZCFHRWPL) / DayMaxFHL, 0) + 1;//排期天数
                                        newZCFHR = Convert.ToDateTime(ZCFHR).AddDays(pxDays).ToString();//最迟发货日
                                        newGZWFHRGTHDYPL = (THSL - ZWZCFHRWPL) % DayMaxFHL;//该最晚发货日该提货单已排量
                                    }
                                }
                                #endregion
                                #region//若最晚日期早于或等于本次提货单的提交日期
                                else
                                {
                                    #region//最晚日期早于本次提货单的提交日期,则今日未排量为最高发货量
                                    if (Convert.ToDateTime(ZCFHR) < Convert.ToDateTime(time))
                                    {
                                        if (THSL * 0.8 <= DayMaxFHL)
                                        {
                                            newZCFHR = Convert.ToDateTime(time).ToString();//最迟发货日
                                            if (THSL >= DayMaxFHL)
                                            {
                                                newGZWFHRGTHDYPL = DayMaxFHL;//该最晚发货日该提货单已排量
                                            }
                                            else
                                            {
                                                newGZWFHRGTHDYPL = THSL;//该最晚发货日该提货单已排量
                                            }
                                        }
                                        else
                                        {
                                            double pxDays = Math.Round((THSL - DayMaxFHL) / DayMaxFHL, 0) + 1;//排期天数
                                            newZCFHR = Convert.ToDateTime(time).AddDays(pxDays).ToString();//最迟发货日
                                            newGZWFHRGTHDYPL = (THSL - DayMaxFHL) % DayMaxFHL;//该最晚发货日该提货单已排量
                                        }
                                    }
                                    #endregion
                                    #region//最晚日期等于本次提货单的提交日期
                                    else
                                    {
                                        if (THSL * 0.8 <= DayMaxFHL)
                                        {
                                            newZCFHR = Convert.ToDateTime(ZCFHR).ToString();//最迟发货日
                                            if (THSL >= ZWZCFHRWPL)
                                            {
                                                newGZWFHRGTHDYPL = ZWZCFHRWPL;//该最晚发货日该提货单已排量
                                            }
                                            else
                                            {
                                                newGZWFHRGTHDYPL = THSL;//该最晚发货日该提货单已排量
                                            }
                                        }
                                        else
                                        {
                                            double pxDays = Math.Round((THSL - ZWZCFHRWPL) / DayMaxFHL, 0) + 1;//排期天数
                                            newZCFHR = Convert.ToDateTime(ZCFHR).AddDays(pxDays).ToString();//最迟发货日
                                            newGZWFHRGTHDYPL = (THSL - ZWZCFHRWPL) % DayMaxFHL;//该最晚发货日该提货单已排量
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            //最终最迟发货日必须在合同期满前5天，否则不允许下达提货单
                            if (Convert.ToDateTime(newZCFHR) > Convert.ToDateTime(htjsrq).AddDays(-5))
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "不能下达提货单！\n\r此提货单最迟发货日为：" + newZCFHR + "；\n\r最迟发货日必须为合同结束日期前五天！";
                                return returnDS;
                            }
                        }

                        

                        #endregion

                        #endregion

                        #region//插入语句
                        #region//插入提货单
                        ArrayList list = new ArrayList();
                        string strInsertTHDSQL = "INSERT INTO [AAA_THDYFHDXXB] ([Number],[ZBDBXXBBH],[ZBDJ],[T_THDJJPLS],[T_THSL],[T_ZCFHR],T_GZWFHRGTHDYPL,[T_SHXXDZ],[T_SHRXM],[T_SHRLXFS],[T_DJHKJE] ,[T_FPLX],[T_FPTT] ,[T_SH],[T_KHYH] ,[T_YHZH],[T_DWDZ],[T_THDXDSJ] ,F_DQZT,[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + number + "','" + ZBDBBH + "'," + ZBDJ + "," + THJJPLS + "," + THSL + ",'" + newZCFHR + "'," + newGZWFHRGTHDYPL + ",'" + SHXXDZ + "','" + SHRXM + "','" + SHLXFS + "'," + kjje + ",'" + FPLX + "','" + FPTT + "','" + SH + "','" + KHYH + "','" + YHZH + "','" + DWDZ + "','" + time + "','未生成发货单',1,'" + DLYX + "','" + time + "','" + time + "')";
                        list.Add(strInsertTHDSQL);
                        #endregion

                        #region//插入资金流水明细

                        string zjnumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        //查询资金变动明细对照表
                        string strZJBDMX = "select * from AAA_moneyDZB where Number in ('1304000009','1304000023')";
                        DataSet dsZJBDMX = DbHelperSQL.Query(strZJBDMX);
                        string xm = "";//项目
                        string xz = "";//性质
                        string zy = "";//摘要
                        string sjlx = "";//数据类型
                        string yslx = "";//运算类型
                        DataRow[] row1 = dsZJBDMX.Tables[0].Select("1=1 and Number='1304000009'");
                        if (row1 != null && row1.Length > 0)
                        {
                            xm = row1[0]["XM"].ToString();//项目
                            xz = row1[0]["XZ"].ToString();//性质
                            zy = row1[0]["ZY"].ToString().Replace("[x1]", "T"+number);//摘要
                            sjlx = row1[0]["SJLX"].ToString();//数据类型
                            yslx = row1[0]["YSLX"].ToString();//运算类型
                        }
                        string strInsertZJLSSQL = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + zjnumber + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','AAA_THDYFHDXXB','" + number + "','" + time + "','" + yslx + "'," + kjje + ",'" + xm + "','" + xz + "','" + zy + "','','','" + sjlx + "',1,'" + DLYX + "','" + time + "','" + time + "')";
                        list.Add(strInsertZJLSSQL);
                        #endregion

                        #region//返写中标定标信息表已提货数量

                        string strUpdateZB = "update AAA_ZBDBXXB set Z_YTHSL=" + newythsl + " where number='" + ZBDBBH + "'";
                        list.Add(strUpdateZB);

                        #endregion

                        #region//解冻订金
                        string djje = "0";//订金金额
                        if (Convert.ToInt32(zt.Tables[0].Rows[0]["Z_ZBSL"].ToString()) == newythsl)//发现买家下达的提货单量=定标量时，则需要解冻买家全部订金（资金流水明细）,并返写登录表的可用余额
                        {
                            #region//插入资金流水明细

                            string zjnumber1 = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                            //查询资金变动明细对照表
                            DataRow[] row2 = dsZJBDMX.Tables[0].Select("1=1 and Number='1304000023'");
                            string xm1 = "";//项目
                            string xz1 = "";//性质
                            string zy1 = "";//摘要
                            string sjlx1 = "";//数据类型
                            string yslx1 = "";//运算类型
                            djje = zt.Tables[0].Rows[0]["Z_DJJE"].ToString();//订金金额
                            if (row2 != null && row2.Length > 0)
                            {
                                xm1 = row2[0]["XM"].ToString();//项目
                                xz1 = row2[0]["XZ"].ToString();//性质
                                zy1 = row2[0]["ZY"].ToString().Replace("[x1]", htbh);//摘要
                                sjlx1 = row2[0]["SJLX"].ToString();//数据类型
                                yslx1 = row2[0]["YSLX"].ToString();//运算类型
                            }
                            string strInsertZJLSSQL1 = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime],[QTBZ]) VALUES ('" + zjnumber1 + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','AAA_ZBDBXXB','" + zt.Tables[0].Rows[0]["Number"].ToString() + "','" + time + "','" + yslx1 + "'," + djje + ",'" + xm1 + "','" + xz1 + "','" + zy1 + "','','" + sjlx1 + "',1,'" + DLYX + "','" + time + "','" + time + "','" + number + "')";
                            list.Add(strInsertZJLSSQL1);
                            #endregion
                        }
                        #endregion

                        #region//返写登录表可用余额

                        //double newkyye = KYYE - Convert.ToDouble(kjje) + Convert.ToDouble(djje);
                        double newkyye = Convert.ToDouble(djje) - Convert.ToDouble(kjje);
                        string strUpdateDL = "update AAA_DLZHXXB set B_ZHDQKYYE= B_ZHDQKYYE +" + newkyye + " where B_DLYX='" + DLYX + "'";
                        list.Add(strUpdateDL);                        

                        #endregion
                                                       
                        bool bl = DbHelperSQL.ExecSqlTran(list);
                        if (bl)//提货单下达成功
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "本次提货单提交成功！";
                            return returnDS;
                            #region//作废
                            //strDB = "select isnull(convert(int,Z_ZBSL),0)-isnull(convert(int,Z_YTHSL),0)  可提货数量,Z_DJJE 订金金额 from AAA_ZBDBXXB where Z_HTZT='定标' and Number='" + ZBDBBH.Trim() + "'";
                            //DataSet dsDBCX = DbHelperSQL.Query(strDB);
                            //if (dsDBCX != null && dsDBCX.Tables[0].Rows.Count>0)
                            //{
                            //    if (Convert.ToInt32(dsDBCX.Tables[0].Rows[0]["可提货数量"].ToString()) == 0)//发现买家下达的提货单量=定标量时，则需要解冻买家全部订金（资金流水明细）,并返写登录表的可用余额
                            //    {
                            //        #region//插入资金流水明细

                            //        string zjnumber1 = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                            //        //查询资金变动明细对照表
                            //        DataRow[] row2 = dsZJBDMX.Tables[0].Select("1=1 and Number='1304000023'");
                            //        string xm1 = "";//项目
                            //        string xz1 = "";//性质
                            //        string zy1 = "";//摘要
                            //        string sjlx1 = "";//数据类型
                            //        string yslx1 = "";//运算类型
                            //        string djje = dsDBCX.Tables[0].Rows[0]["订金金额"].ToString();//订金金额
                            //        if (row2 != null && row2.Length > 0)
                            //        {
                            //            xm1 = row2[0]["XM"].ToString();//项目
                            //            xz1 = row2[0]["XZ"].ToString();//性质
                            //            zy1 = row2[0]["ZY"].ToString().Replace("[x1]", htbh);//摘要
                            //            sjlx1 = row2[0]["SJLX"].ToString();//数据类型
                            //            yslx1 = row2[0]["YSLX"].ToString();//运算类型
                            //        }
                            //        string strInsertZJLSSQL1 = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime],[QTBZ]) VALUES ('" + zjnumber1 + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','AAA_ZBDBXXB','" + zt.Tables[0].Rows[0]["Number"].ToString() + "','" + time + "','" + yslx1 + "'," + djje + ",'" + xm1 + "','" + xz1 + "','" + zy1 + "','','" + sjlx1 + "',1,'" + DLYX + "','" + time + "','" + time + "','" + number + "')";
                            //        DbHelperSQL.ExecuteSql(strInsertZJLSSQL1);
                            //        #endregion

                            //        #region//返写登录表可用余额
                            //        double newKYYE = money.GetMoneyT(DLYX, "");//账户当前可用余额
                            //        double newkyye1 = newKYYE + Convert.ToDouble(djje);
                            //        string strUpdateDL1 = "update AAA_DLZHXXB set B_ZHDQKYYE=" + newkyye1 + " where B_DLYX='" + DLYX + "'";
                            //        int fxDL1= DbHelperSQL.ExecuteSql(strUpdateDL1);
                            //        if (fxDL1 > 0)
                            //        {
                            //            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            //            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "本次提货单提交成功！";
                            //            return returnDS;
                            //        }
                            //        else
                            //        {
                            //            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            //            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "提货单提交失败！";
                            //            return returnDS;
                            //        }
                            //        #endregion
                            //    }
                            //    else
                            //    {
                            //        if (fxDL > 0)
                            //        {
                            //            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            //            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "本次提货单提交成功！";
                            //            return returnDS;
                            //        }
                            //        else
                            //        {
                            //            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            //            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "提货单提交失败！";
                            //            return returnDS;
                            //        }
                            //    }
                            //}
                            #endregion
                        }
                        else
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "提货单提交失败！";
                            return returnDS;
                        }                            

                        #endregion
                    }
                    else
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您的操作不正确！";
                        return returnDS;
                    }
                }
                else
                {
                    double ce = DJJE - KYYE;
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    if (zt.Tables[0].Rows[0]["Z_HTQX"].ToString() == "即时")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您交易账户中的可用资金余额为" + KYYE.ToString("#0.00") + "元，\n\r与该提货单的订金差额为" + ce.ToString("#0.00") + "元。\n\r您可增加账户的可用资金余额。";
                    }
                    else
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您交易账户中的可用资金余额为" + KYYE.ToString("#0.00") + "元，\n\r与该提货单的订金差额为" + ce.ToString("#0.00") + "元。\n\r您可增加账户的可用资金余额或修改提货单。";
                    }
                   
                    return returnDS;
                }
                #endregion
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "存入的数据不能为空！";
            }                        
            
        }
        catch (Exception e)
        {

            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";   
        }
        return returnDS;
    }
    //查看提货单
    public DataSet CKTHD(string THDBH, DataSet returnDS)
    {
        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        try
        {            
            //string strSQL = "select t.F_DQZT,t.Number 提货单编号,t.T_THDXDSJ 提货单下达时间,d.Z_HTBH 电子购货合同编号,'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),t.T_ZCFHR 最迟发货日,'此前累计提货次数'=(select count(*) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),t.T_THSL 本次提货数量,'此前累计提货数量'=(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),d.Z_ZBSL 定标数量,'剩余可提货数量'=(isnull(d.Z_ZBSL,0)-isnull((select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),0)),T_DJHKJE 本次提货金额,'此前累计提货金额'=(select isnull(sum(T_DJHKJE),0.00) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),isnull(isnull(d.Z_ZBSL,0)*isnull(Z_ZBJG,0),0) 定标金额,'剩余可提货金额'=(isnull(isnull(d.Z_ZBSL,0)-isnull((select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),0)*t.ZBDJ,0.00)), d.Z_SPBH 商品编号,d.Z_SPMC 商品名称,d.Z_GG 规格,d.Z_JJDW 计价单位,t.T_THSL 提货数量,t.ZBDJ 定标价格,t.T_DJHKJE 金额,t.T_FPLX 发票类型,t.T_FPTT 发票抬头,t.T_SH 税号,t.T_KHYH 开户银行,t.T_YHZH 银行账号,t.T_DWDZ 单位地址,t.T_SHRXM 收货人,t.T_SHRLXFS 收货联系方式,(d.Y_YSYDDSHQYsheng+d.Y_YSYDDSHQYshi+t.T_SHXXDZ) 收货地址 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number  where t.Number='" + THDBH.Trim() + "'";
            string strSQL = "select t.F_DQZT,t.Number 提货单编号,t.T_THDXDSJ 提货单下达时间,d.Z_HTBH 电子购货合同编号,'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),t.T_ZCFHR 最迟发货日,'此前累计提货次数'=(select count(*) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),t.T_THSL 本次提货数量,'此前累计提货数量'=(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),d.Z_ZBSL 定标数量,'剩余可提货数量'=(isnull(d.Z_ZBSL,0)-isnull((select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),0)-isnull(t.T_THSL,0)),T_DJHKJE 本次提货金额,'此前累计提货金额'=(select isnull(sum(T_DJHKJE),0.00) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),isnull(isnull(d.Z_ZBSL,0)*isnull(Z_ZBJG,0),0) 定标金额,'剩余可提货金额'=(isnull((isnull(d.Z_ZBSL,0)-isnull((select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT!='撤销' and a.T_THDXDSJ < t.T_THDXDSJ),0)-isnull(t.T_THSL,0))*t.ZBDJ,0.00)), d.Z_SPBH 商品编号,d.Z_SPMC 商品名称,d.Z_GG 规格,d.Z_JJDW 计价单位,t.T_THSL 提货数量,t.ZBDJ 定标价格,t.T_DJHKJE 金额,t.T_FPLX 发票类型,t.T_FPTT 发票抬头,t.T_SH 税号,t.T_KHYH 开户银行,t.T_YHZH 银行账号,t.T_DWDZ 单位地址,t.T_SHRXM 收货人,t.T_SHRLXFS 收货联系方式,(d.Y_YSYDDSHQYsheng+d.Y_YSYDDSHQYshi+t.T_SHXXDZ) 收货地址 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number  where t.Number='" + THDBH.Trim() + "'";
            DataSet ds = DbHelperSQL.Query(strSQL);
            DataTable dt = ds.Tables[0].Copy();
            dt.TableName = "主表";
            returnDS.Tables.Add(dt);
            return returnDS;
        }
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";    
        }
        return returnDS;
    }

}