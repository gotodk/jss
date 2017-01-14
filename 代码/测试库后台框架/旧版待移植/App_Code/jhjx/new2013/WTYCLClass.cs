using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;

/// <summary>
/// WTYCLClass 的摘要说明
/// </summary>
public class WTYCLClass
{
	public WTYCLClass()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public DataSet WTYCL(DataSet ds, DataSet returnDS)
    {
        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作成功！";
        try
        {

            Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
            if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                return returnDS;
            }
            if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
                return returnDS;
            }
            if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                return returnDS;
            }
            if (ds!=null && ds.Tables[0].Rows.Count>0)
            {
                string JSBH = ds.Tables[0].Rows[0]["买家角色编号"].ToString();//买家角色编号
                string THDBH = ds.Tables[0].Rows[0]["提货单编号"].ToString();//提货单编号
                string time = DateTime.Now.ToString();
                #region//基础验证
                Hashtable JCYZ = PublicClass2013.GetParameterInfo();
                Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
                if (JBXX["交易账户是否开通"].ToString() == "否")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                    return returnDS;
                }
 
                if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
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
                #endregion
                #region//验证合同状态
                string strSQLDB = "  select  d.Z_HTZT 合同状态,t.*  from AAA_THDYFHDXXB t,AAA_ZBDBXXB d where t.ZBDBXXBBH=d.Number and t.Number='" + THDBH.Trim() + "'";
                DataSet zt = DbHelperSQL.Query(strSQLDB);
                if (zt != null && zt.Tables[0].Rows.Count>0)
                {
                    switch (zt.Tables[0].Rows[0]["合同状态"].ToString())
                    {
                        case "定标":
                            break;
                        default:
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "抱歉,该发货单无法执行此类指令！";
                            return returnDS;
                    }
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到发货单的信息！";
                    return returnDS;
                }
                #endregion
                string cz = ds.Tables[0].Rows[0]["操作"].ToString();
                switch (cz.Trim())
                {
                    case "录入补发备注":
                        if (zt.Tables[0].Rows[0]["F_DQZT"].ToString() != "部分收货" && zt.Tables[0].Rows[0]["F_DQZT"].ToString() != "已录入补发备注")
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "OpIsInvalid";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作无效！";
                            return returnDS;
                        }

                        #region//验证投标资料审核表对应的投标点是否审核通过
                        string strTBDZLSH = "select c.* from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH = b.Number left join AAA_TBZLSHB c on b.T_YSTBDBH = c.TBDH where a.Number='" + THDBH.Trim() + "'";
                        DataSet dsZLSHB = DbHelperSQL.Query(strTBDZLSH);
                        if (dsZLSHB != null && dsZLSHB.Tables[0].Rows.Count > 0)
                        {
                            if (dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "未审核" || dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                                return returnDS;
                            }
                        }
                        //else
                        //{
                        //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询当前投标单投标资料的审核信息！";
                        //    return returnDS;
                        //}
                        #endregion

                        string bfbz = ds.Tables[0].Rows[0]["补发备注"].ToString();

                        string strSQLBF = "update AAA_THDYFHDXXB set F_BFBZ='" + bfbz + "',F_BFBZZHGXSJ='" + time + "',F_DQZT='已录入补发备注',F_QMJQSQRCZSJ=null,F_QMJQSWLGSMC='',F_QMJQSWLDH='',F_QMJQSWLQSD='' where Number='" + THDBH + "'";
                        int i = DbHelperSQL.ExecuteSql(strSQLBF);
                        if (i > 0)
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "补发备注录入成功！";
                            return returnDS;
                        }
                        else
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "补发备注录入失败！";
                            return returnDS;
                        }
                    case "请买方“无异议收货”"://提请买家签收                        
                        if (zt.Tables[0].Rows[0]["F_BFBZZHGXSJ"].ToString() != "")
                        {
                            TQBuyQSClass qs = new TQBuyQSClass();
                            return qs.InsertSQL(ds, returnDS);
                        }
                        else
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "请先录入补发备注！";
                            return returnDS;
                        }

                    case "卖方主动退货":
                        if (zt.Tables[0].Rows[0]["F_DQZT"].ToString() != "有异议收货")
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "OpIsInvalid";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作无效！";
                            return returnDS;
                        }
                        return SelTH(ds, returnDS);
                    case "同意重新发货":
                        if (zt.Tables[0].Rows[0]["F_DQZT"].ToString() != "请重新发货")
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "OpIsInvalid";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作无效！";
                            return returnDS;
                        }
                        return CXFH(ds,JCYZ, returnDS);
                    case "无异议收货":
                        if (ds.Tables[0].Rows[0]["type"].ToString() == "有异议收货后无异议收货")
                        {
                            if (zt.Tables[0].Rows[0]["F_DQZT"].ToString() != "有异议收货")
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "OpIsInvalid";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作无效！";
                                return returnDS;
                            }
                        }
                        else if (ds.Tables[0].Rows[0]["type"].ToString() == "补发货物无异议收货")
                        {
                            if (zt.Tables[0].Rows[0]["F_DQZT"].ToString() != "已录入补发备注")
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "OpIsInvalid";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作无效！";
                                return returnDS;
                            }
                        }
                        DataTable table = ds.Tables[0].Copy();
                        Jhjx_hwqs hh = new Jhjx_hwqs();
                        return hh.dsjhjx_hwqs(returnDS,table);
                    default:
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非法操作！";
                        return returnDS;
                }

            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "存入的数据不能为空！";
            }
        }
        catch(Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";   
        }
        return returnDS;
    }
    //重新发货
    private DataSet CXFH(DataSet ds,Hashtable JCYZ, DataSet returnDS)
    {
        Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
            return returnDS;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
            return returnDS;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return returnDS;
        }


        string DLYX= ds.Tables[0].Rows[0]["登录邮箱"].ToString();
        string JSZHLX = ds.Tables[0].Rows[0]["结算账户类型"].ToString();
        string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();
        string BuyDLYX = ds.Tables[0].Rows[0]["提醒对象登陆邮箱"].ToString();//买家登录邮箱
        string BuyJSZHLX = ds.Tables[0].Rows[0]["提醒对象结算账户类型"].ToString();//买家结算账户类型
        string BuyJSBH = ds.Tables[0].Rows[0]["提醒对象角色编号"].ToString();//买家角色编号
        ArrayList list = new ArrayList();
        try
        {
            #region//验证投标资料审核表对应的投标点是否审核通过
            string strTBDZLSH = "select c.* from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH = b.Number left join AAA_TBZLSHB c on b.T_YSTBDBH = c.TBDH where a.Number='" + ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim() + "'";
            DataSet dsZLSHB = DbHelperSQL.Query(strTBDZLSH);
            if (dsZLSHB != null && dsZLSHB.Tables[0].Rows.Count > 0)
            {
                if (dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "未审核" || dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                    return returnDS;
                }
            }
            //else
            //{
            //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询当前投标单投标资料的审核信息！";
            //    return returnDS;
            //}
            #endregion

            string strSelect = "select * from AAA_THDYFHDXXB where Number='" + ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim() + "'";
            DataSet dsSelect = DbHelperSQL.Query(strSelect);
            if (dsSelect != null && dsSelect.Tables[0].Rows.Count > 0)
            {
                string strCX = "update AAA_THDYFHDXXB set F_DQZT='撤销' where Number='" + ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim() + "'";
                list.Add(strCX);

                #region//插入准备字段
                string number = PublicClass2013.GetNextNumberZZ("AAA_THDYFHDXXB", "");
                string ZBDBBH = "";//中标定标表编号
                string ZBDJ = "";//中标单价
                int THJJPLS = 0;//提货的经济批量数
                int THSL = 0;//提货数量
                string newZCFHR = "";//最迟发货日
                string SHXXDZ = "";//收货详细地址
                string SHRXM = "";//收货人姓名
                string SHLXFS = "";//收货人联系方式
                string DJHKJE = "";//冻结货款金额
                string FPLX = "";//发票类型
                string FPTT = "";//发票抬头
                string SH = "";//税号
                string KHYH = "";//开户银行
                string YHZH = "";//银行账号
                string DWDZ = "";//单位地址
                string time = DateTime.Now.ToString();
                int zbsl = 0;//中标数量
                string htqx = "";//合同期限
                string htjsrq = "";//合同结束日期
                string fpsfshtx = "";//发票是否随货同行
                ZBDBBH = dsSelect.Tables[0].Rows[0]["ZBDBXXBBH"].ToString();//中标定标表编号               
                ZBDJ = dsSelect.Tables[0].Rows[0]["ZBDJ"].ToString();//中标单价
                THJJPLS = Convert.ToInt32(dsSelect.Tables[0].Rows[0]["T_THDJJPLS"].ToString());//提货的经济批量数
                THSL = Convert.ToInt32(dsSelect.Tables[0].Rows[0]["T_THSL"].ToString());//提货数量
                SHXXDZ = dsSelect.Tables[0].Rows[0]["T_SHXXDZ"].ToString();//收货详细地址
                SHRXM = dsSelect.Tables[0].Rows[0]["T_SHRXM"].ToString();//收货人姓名
                SHLXFS = dsSelect.Tables[0].Rows[0]["T_SHRLXFS"].ToString();//收货人联系方式
                DJHKJE = dsSelect.Tables[0].Rows[0]["T_DJHKJE"].ToString();//冻结货款金额
                FPLX = dsSelect.Tables[0].Rows[0]["T_FPLX"].ToString();//发票类型
                FPTT = dsSelect.Tables[0].Rows[0]["T_FPTT"].ToString();//发票抬头
                SH = dsSelect.Tables[0].Rows[0]["T_SH"].ToString();//税号
                KHYH = dsSelect.Tables[0].Rows[0]["T_KHYH"].ToString();//开户银行
                YHZH = dsSelect.Tables[0].Rows[0]["T_YHZH"].ToString();//银行账号
                DWDZ = dsSelect.Tables[0].Rows[0]["T_DWDZ"].ToString();//单位地址
                fpsfshtx = dsSelect.Tables[0].Rows[0]["F_FPSFSHTH"].ToString();//发票是否随货同行
                string FPYJXXLRSJ = "";//发票邮寄信息录入时间
                string FPHM = "";//发票号码
                string FPYJDWMC = "";//发票邮寄单位名称
                string FPYJDBH = "";//发票邮寄单编号
                FPYJXXLRSJ = dsSelect.Tables[0].Rows[0]["F_FPYJXXLRSJ"].ToString();//发票邮寄信息录入时间
                FPHM = dsSelect.Tables[0].Rows[0]["F_FPHM"].ToString();//发票号码
                FPYJDWMC = dsSelect.Tables[0].Rows[0]["F_FPYJDWMC"].ToString();//发票邮寄单位名称
                FPYJDBH = dsSelect.Tables[0].Rows[0]["F_FPYJDBH"].ToString();//发票邮寄单编号
                string DBTime = "";//定标时间

                #region//根据中标定标信息表编号查询中标定标信息表中的可提货数量、已提货数量、中标数量、合同期限
                double DayMaxFHL = 0.00;//最高发货量
                string strDB = "select convert(int,Z_ZBSL)-convert(int,Z_YTHSL) 可提货数量,Z_YTHSL 已提货数量,Z_ZBSL 中标数量,Z_HTQX 合同期限,Z_HTJSRQ 合同结束日期,Z_RJZGFHL 日均最高发货量,Z_DBSJ 定标时间,Z_HTBH  from AAA_ZBDBXXB where Z_HTZT='定标' and Number='" + ZBDBBH.Trim() + "'";
                DataSet dsDB = DbHelperSQL.Query(strDB);
                if (dsDB != null && dsDB.Tables[0].Rows.Count > 0)
                {
                    DBTime = dsDB.Tables[0].Rows[0]["定标时间"].ToString();
                    DayMaxFHL = Convert.ToDouble(dsDB.Tables[0].Rows[0]["日均最高发货量"].ToString());//最高发货量
                    zbsl = Convert.ToInt32(dsDB.Tables[0].Rows[0]["中标数量"].ToString());//中标数量
                    htqx = dsDB.Tables[0].Rows[0]["合同期限"].ToString();//合同期限
                    htjsrq = dsDB.Tables[0].Rows[0]["合同结束日期"].ToString();//合同结束日期            
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "合同编号为：" + dsDB.Tables[0].Rows[0]["Z_HTBH"].ToString() + "  的定标信息，不能进行同意重新发货的操作。";
                    return returnDS;
                }
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
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "不能进行同意重新发货的操作！\n\r此提货单定标时间为：" + DBTime + "；\n\r同意重新发货的操作时间必须小于定标时间后" + JCYZ["即时合同定标下提货单期限"].ToString() + "天！"; 
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
                    int days = 0;
                    switch (htqx)
                    {
                        case "三个月":
                            days = 90;
                            break;
                        case "一年":
                            days = 365;
                            break;
                        default:
                            days = 90;
                            break;
                    }
                    //double DayMaxFHL = Math.Round(zbsl / days * 1.2, 0) + 1;//最高发货量
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
                    //最终最迟发货日若在合同期满前5天，刚不允许下达提货单
                    if (Convert.ToDateTime(newZCFHR) > Convert.ToDateTime(htjsrq).AddDays(-5))
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "不能进行同意重新发货的操作！\n\r此提货单最迟发货日为：" + newZCFHR + "；\n\r最迟发货日必须为合同结束日期前五天！"; 
                        return returnDS;
                    }
                }

                #endregion

                #endregion

                #region//插入提货单
                if (FPYJXXLRSJ == "")
                {
                    string strInsertTHDSQL = "INSERT INTO [AAA_THDYFHDXXB] ([Number],[ZBDBXXBBH],[ZBDJ],[T_THDJJPLS],[T_THSL],[T_ZCFHR],T_GZWFHRGTHDYPL,[T_SHXXDZ],[T_SHRXM],[T_SHRLXFS],[T_DJHKJE] ,[T_FPLX],[T_FPTT] ,[T_SH],[T_KHYH] ,[T_YHZH],[T_DWDZ],[T_THDXDSJ] ,F_FPSFSHTH,F_FPYJXXLRSJ,F_FPHM,F_FPYJDWMC,F_FPYJDBH,F_DQZT,F_SELTYZXFHCZSJ,[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + number + "','" + ZBDBBH + "'," + ZBDJ + "," + THJJPLS + "," + THSL + ",'" + newZCFHR + "'," + newGZWFHRGTHDYPL + ",'" + SHXXDZ + "','" + SHRXM + "','" + SHLXFS + "'," + DJHKJE + ",'" + FPLX + "','" + FPTT + "','" + SH + "','" + KHYH + "','" + YHZH + "','" + DWDZ + "','" + time + "','" + fpsfshtx + "',null,'" + FPHM + "','" + FPYJDWMC + "','" + FPYJDBH + "','未生成发货单','" + time + "',1,'" + DLYX + "','" + time + "','" + time + "')";
                    list.Add(strInsertTHDSQL);
                }
                else
                {
                    string strInsertTHDSQL = "INSERT INTO [AAA_THDYFHDXXB] ([Number],[ZBDBXXBBH],[ZBDJ],[T_THDJJPLS],[T_THSL],[T_ZCFHR],T_GZWFHRGTHDYPL,[T_SHXXDZ],[T_SHRXM],[T_SHRLXFS],[T_DJHKJE] ,[T_FPLX],[T_FPTT] ,[T_SH],[T_KHYH] ,[T_YHZH],[T_DWDZ],[T_THDXDSJ] ,F_FPSFSHTH,F_FPYJXXLRSJ,F_FPHM,F_FPYJDWMC,F_FPYJDBH,F_DQZT,F_SELTYZXFHCZSJ,[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + number + "','" + ZBDBBH + "'," + ZBDJ + "," + THJJPLS + "," + THSL + ",'" + newZCFHR + "'," + newGZWFHRGTHDYPL + ",'" + SHXXDZ + "','" + SHRXM + "','" + SHLXFS + "'," + DJHKJE + ",'" + FPLX + "','" + FPTT + "','" + SH + "','" + KHYH + "','" + YHZH + "','" + DWDZ + "','" + time + "','" + fpsfshtx + "','" + FPYJXXLRSJ + "','" + FPHM + "','" + FPYJDWMC + "','" + FPYJDBH + "','未生成发货单','" + time + "',1,'" + DLYX + "','" + time + "','" + time + "')";
                    list.Add(strInsertTHDSQL);
                }
                
                #endregion

                #region//插入资金流水明细

                string zjnumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                //查询资金变动明细对照表
                string strZJBDMX = "select * from AAA_moneyDZB where Number in ('1304000017','1304000009')";
                DataSet dsZJBDMX = DbHelperSQL.Query(strZJBDMX);
                string xm = "";//项目
                string xz = "";//性质
                string zy = "";//摘要
                string sjlx = "";//数据类型
                string yslx = "";//运算类型
                DataRow[] row1 = dsZJBDMX.Tables[0].Select("1=1 and Number='1304000017'");//先解冻买家货款
                if (row1 != null && row1.Length > 0)
                {
                    xm = row1[0]["XM"].ToString();//项目
                    xz = row1[0]["XZ"].ToString();//性质
                    zy = row1[0]["ZY"].ToString().Replace("[x1]", "T"+ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim());//摘要
                    sjlx = row1[0]["SJLX"].ToString();//数据类型
                    yslx = row1[0]["YSLX"].ToString();//运算类型
                }
                string strInsertZJLSSQL = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + zjnumber + "','" + BuyDLYX + "','" + BuyJSZHLX + "','" + BuyJSBH + "','AAA_THDYFHDXXB','" + ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim() + "','" + time + "','" + yslx + "'," + Convert.ToDouble(DJHKJE) + ",'" + xm + "','" + xz + "','" + zy + "','','','" + sjlx + "',1,'" + DLYX + "','" + time + "','" + time + "')";
                list.Add(strInsertZJLSSQL);
                zjnumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                row1 = dsZJBDMX.Tables[0].Select("1=1 and Number='1304000009'");
                if (row1 != null && row1.Length > 0)
                {
                    xm = row1[0]["XM"].ToString();//项目
                    xz = row1[0]["XZ"].ToString();//性质
                    zy = row1[0]["ZY"].ToString().Replace("[x1]", "T"+number);//摘要
                    sjlx = row1[0]["SJLX"].ToString();//数据类型
                    yslx = row1[0]["YSLX"].ToString();//运算类型
                }
                strInsertZJLSSQL = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + zjnumber + "','" + BuyDLYX + "','" + BuyJSZHLX + "','" + BuyJSBH + "','AAA_THDYFHDXXB','" + number + "','" + time + "','" + yslx + "'," + Convert.ToDouble(DJHKJE) + ",'" + xm + "','" + xz + "','" + zy + "','','','" + sjlx + "',1,'" + DLYX + "','" + time + "','" + time + "')";
                list.Add(strInsertZJLSSQL);
                #endregion

                bool bl = DbHelperSQL.ExecSqlTran(list);
                if (bl)
                {
                    List<Hashtable> listHT = new List<Hashtable>();
                    Hashtable htJYPT = new Hashtable();//交易平台
                    htJYPT["type"] = "集合集合经销平台";
                    htJYPT["提醒对象登陆邮箱"] = ds.Tables[0].Rows[0]["提醒对象登陆邮箱"].ToString();
                    htJYPT["提醒对象用户名"] = ds.Tables[0].Rows[0]["提醒对象用户名"].ToString();
                    htJYPT["提醒对象结算账户类型"] = ds.Tables[0].Rows[0]["提醒对象结算账户类型"].ToString();
                    htJYPT["提醒对象角色编号"] = ds.Tables[0].Rows[0]["提醒对象角色编号"].ToString();
                    htJYPT["提醒对象角色类型"] = "买家";
                    htJYPT["提醒内容文本"] = DLYX + "卖方根据您的要求已同意重新发货，对应的T" + ds.Tables[0].Rows[0]["Number"].ToString().Trim() + "提货单与F" + ds.Tables[0].Rows[0]["Number"].ToString().Trim() + "发货单作废，卖方将根据系统自动新生成的T" + number + "提货单重新发货。";
                    htJYPT["创建人"] = ds.Tables[0].Rows[0]["创建人"].ToString();
                    list.Add(htJYPT);
                    Hashtable htYWPT = new Hashtable();//业务操作平台
                    htYWPT["type"] = "业务平台";
                    htYWPT["模提醒模块名"] = "卖家重新发货";
                    htYWPT["提醒内容文本"] = DLYX + "卖方根据您的要求已同意重新发货，对应的T" + ds.Tables[0].Rows[0]["Number"].ToString().Trim() + "提货单与F" + ds.Tables[0].Rows[0]["Number"].ToString().Trim() + "发货单作废，卖方将根据系统自动新生成的T" + number + "提货单重新发货。";
                    htYWPT["查看地址"] = "";
                    list.Add(htYWPT);
                    PublicClass2013.Sendmes(listHT);
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "同意重新发货操作成功！";
                    return returnDS;
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "同意重新发货操作失败！";
                    return returnDS;
                }
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到F" + ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim() + "发货单的信息。";
                return returnDS;
            }
        }
        catch(Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";   
            return returnDS;
        }
        
    }
    //卖家主动退货
    private DataSet SelTH(DataSet ds, DataSet returnDS)
    {

        Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
            return returnDS;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
            return returnDS;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return returnDS;
        }

        try
        {
            ArrayList list = new ArrayList();
            string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();//卖家角色编号
            string THDBH = ds.Tables[0].Rows[0]["提货单编号"].ToString();//提货单编号
            string DLYX = ds.Tables[0].Rows[0]["卖家登录邮箱"].ToString();//卖家登录邮箱
            string JSZHLX = ds.Tables[0].Rows[0]["结算账户类型"].ToString();//结算账户类型
            string BuyDLYX = ds.Tables[0].Rows[0]["买家登录邮箱"].ToString();//卖家登录邮箱
            string BuyJSZHLX = ds.Tables[0].Rows[0]["提醒对象结算账户类型"].ToString();//买家结算账户类型
            string BuyJSBH = ds.Tables[0].Rows[0]["提醒对象角色编号"].ToString();//买家角色编号
            string time = DateTime.Now.ToString();

            #region//验证投标资料审核表对应的投标点是否审核通过
            string strTBDZLSH = "select c.* from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH = b.Number left join AAA_TBZLSHB c on b.T_YSTBDBH = c.TBDH where a.Number='" + THDBH.Trim() + "'";
            DataSet dsZLSHB = DbHelperSQL.Query(strTBDZLSH);
            if (dsZLSHB != null && dsZLSHB.Tables[0].Rows.Count > 0)
            {
                if (dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "未审核" || dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                    return returnDS;
                }
            }
            //else
            //{
            //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询当前投标单投标资料的审核信息！";
            //    return returnDS;
            //}
            #endregion

            #region//查询当前提货单的状态
            string strSQLTHD = "select F_DQZT from AAA_THDYFHDXXB where Number='" + THDBH + "'";
            object zt = DbHelperSQL.GetSingle(strSQLTHD);
            if (zt == "有异议收货后无异议收货")
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "该发货单买方已“改为无异议收货”！";
                return returnDS;
            }
            #endregion

            #region//查询买家账户当前可用余额
            double buyyue = 0.00;
            double sellyue = 0.00;
            ClassMoney2013 money = new ClassMoney2013();
            //buyyue = money.GetMoneyT(BuyDLYX, "");//账户当前可用余额
            //sellyue = money.GetMoneyT(DLYX, "");//账户当前可用余额
            #endregion


            #region 满足买家无异议签收数量=定标数量是单据处理
            string strTHDAndDB = "select a.Number,('T'+a.Number) as  提货单号,('F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖家名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买家角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.T_YSTBDMJJSBH as 卖家角色编号,b.T_YSTBDDLYX as 卖家登陆邮箱,b.T_YSTBDJSZHLX as 卖家角色账户类型,b.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,b.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,b.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,b.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,b.Z_ZBSL as 定标数量,b.Z_LYBZJJE as 履约保证金金额,b.Z_HTZT as 合同状态  from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX  where a.Number='" + THDBH + "' and b.Y_YSYDDDLYX='" + BuyDLYX + "'";
            DataSet dsinfo = DbHelperSQL.Query(strTHDAndDB);
            if (dsinfo != null && dsinfo.Tables[0].Rows.Count > 0)
            {
                DataRow drinfo = dsinfo.Tables[0].Rows[0];

                #region//解冻买家货款，写入资金流水明细表
                string ZJBD = "select  Number,'' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number in ('1304000016','1304000048','1304000028','1304000037','1304000026','1304000036')";
                DataSet dsZJBDMX = DbHelperSQL.Query(ZJBD);
                string xm = "";//项目
                string xz = "";//性质
                string zy = "";//摘要
                string sjlx = "";//数据类型
                string yslx = "";//运算类型
                DataRow[] row1 = dsZJBDMX.Tables[0].Select("1=1 and Number='1304000016'");
                if (row1 != null && row1.Length > 0)
                {
                    xm = row1[0]["XM"].ToString();//项目
                    xz = row1[0]["XZ"].ToString();//性质
                    zy = row1[0]["ZY"].ToString().Replace("[x1]", "F"+THDBH);//摘要
                    sjlx = row1[0]["SJLX"].ToString();//数据类型
                    yslx = row1[0]["YSLX"].ToString();//运算类型
                }
                string zjnumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                string strInsertZJLSSQL = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + zjnumber + "','" + BuyDLYX + "','" + BuyJSZHLX + "','" + BuyJSBH + "','AAA_THDYFHDXXB','" + THDBH + "','" + time + "','" + yslx + "'," + dsinfo.Tables[0].Rows[0]["发货金额"].ToString() + ",'" + xm + "','" + xz + "','" + zy + "','','','" + sjlx + "',1,'" + DLYX + "','" + time + "','" + time + "')";
                list.Add(strInsertZJLSSQL);
                buyyue += Convert.ToDouble(dsinfo.Tables[0].Rows[0]["发货金额"].ToString());
                #endregion

                int zbNum = Convert.ToInt32(drinfo["定标数量"].ToString().Trim());
                int thbc = Convert.ToInt32(drinfo["提货数量"].ToString().Trim());
                int ythNum = 0;
                string StrgetyithNum = "select  isnull(SUM (ISNULL(T_THSL,0)),0) as  已提货数量,ZBDBXXBBH     from AAA_THDYFHDXXB where ZBDBXXBBH = '" + drinfo["中标定标单号"].ToString() + "' and F_DQZT in ( '无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货','卖家主动退货' ) group by ZBDBXXBBH";
                DataSet dsyth = DbHelperSQL.Query(StrgetyithNum);
                if (dsyth != null && dsyth.Tables[0].Rows.Count > 0)
                {
                    ythNum = Convert.ToInt32(dsyth.Tables[0].Rows[0]["已提货数量"].ToString());
                }
                List<Hashtable> listHT = new List<Hashtable>();
                if (zbNum == thbc + ythNum)
                {
                    #region 解冻卖家履约保证金
                    //解冻卖家履约保证金
                    double lvbzj = Convert.ToDouble(drinfo["履约保证金金额"].ToString().Trim());
                    //无异议提货解冻卖家履约保证金
                    DataRow[] drhkjd = dsZJBDMX.Tables[0].Select("1=1 and  Number = '1304000048'");
                    if (drhkjd != null && drhkjd.Length > 0)
                    {
                        drhkjd[0]["ZY"] = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                        string Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                        //无异议提货解冻卖家履约保证金sql
                        string strinsehwqsjdmjlybzj = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_ZBDBXXB','" + drinfo["中标定标单号"].ToString() + "','" + time + "','" + drhkjd[0]["YSLX"].ToString() + "','" + drinfo["履约保证金金额"].ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + drhkjd[0]["ZY"].ToString() + "','" + drhkjd[0]["SJLX"].ToString() + "','" + DLYX + "','" + time + "','监控编号' ) ";
                        list.Add(strinsehwqsjdmjlybzj);
                        sellyue += Convert.ToDouble(drinfo["履约保证金金额"].ToString());
                    }
                    else
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "无异议提货解冻卖方履约保证金信息未找到！";
                        return returnDS;
                    }
                    #endregion

                    #region 因发票延期或者发货延期的罚单与补偿
                    //获得所有相同中标定标单号的提货单编号的发货单号

                    string GetTHdNumber = " select Number from  AAA_THDYFHDXXB where ZBDBXXBBH = '" + drinfo["中标定标单号"].ToString() + "' ";
                    DataSet dsfpyq = DbHelperSQL.Query(GetTHdNumber);
                    if (dsfpyq != null && dsfpyq.Tables[0].Rows.Count > 0)
                    {
                        string Strwhere = "";
                        foreach (DataRow dr in dsfpyq.Tables[0].Rows)
                        {
                            Strwhere += "'" + dr["Number"].ToString().Trim() + "',";
                        }

                        Strwhere = Strwhere.Trim().Remove(Strwhere.Length - 1);

                        # region 发票延期的扣罚与补赏
                        //获取次投标定标预订单的发票延迟预扣款流水明细
                        double je = 0.00;
                        string Strfpykmc = "select Number, JE from AAA_ZKLSMXB where XM = '违约赔偿金' and XZ = '发货单生成后5日内未录入发票邮寄信息' and LYDH in (" + Strwhere + ") and SJLX='预'";
                        DataSet dsfpykmc = DbHelperSQL.Query(Strfpykmc);
                        if (dsfpykmc != null && dsfpykmc.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow drfpykk in dsfpykmc.Tables[0].Rows)
                            {
                                je += Convert.ToDouble(drfpykk["JE"].ToString().Trim());
                            }

                            //卖家扣罚(发货单生成后5日内未录入发票邮寄信息)

                            drhkjd = dsZJBDMX.Tables[0].Select("1=1 and  Number = '1304000028'");
                            if (drhkjd != null && drhkjd.Length > 0)
                            {
                                drhkjd[0]["ZY"] = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                string Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                                //发货单生成后5日内未录入发票邮寄信息sql
                                string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + time + "','" + drhkjd[0]["YSLX"].ToString() + "','" + je.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + drhkjd[0]["ZY"].ToString() + "','" + drhkjd[0]["SJLX"].ToString() + "','" + DLYX + "','" + time + "','监控编号' ) ";
                                list.Add(strinsehwqsfpyqkf);
                                sellyue -= je;
                            }
                            else
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "日内未录入发票邮寄信息未找到！";
                                return returnDS;
                            }

                            //买家补赏(发货单生成后5日内未录入发票邮寄信息)

                            drhkjd = dsZJBDMX.Tables[0].Select("1=1 and  Number = '1304000037'");
                            if (drhkjd != null && drhkjd.Length > 0)
                            {
                                drhkjd[0]["ZY"] = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                string Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString())["结算账户类型"].ToString();

                                //发货单生成后5日内未录入发票邮寄信息sql
                                string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + BuyDLYX + "','" + jszhlx + "','" + drinfo["买家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + time + "','" + drhkjd[0]["YSLX"].ToString() + "','" + je.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + drhkjd[0]["ZY"].ToString() + "','" + drhkjd[0]["SJLX"].ToString() + "','" + DLYX + "','" + time + "','监控编号' ) ";
                                list.Add(strinsehwqsfpyqkf);
                                buyyue += je;
                            }
                            else
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                return returnDS;
                            }



                        }
                        #endregion

                        #region  卖家发货延期的扣罚与补偿
                        //获取次投标定标预订单的发票延迟预扣款流水明细
                        double hwje = 0.00;
                        string Strhwykmc = "select Number, JE from AAA_ZKLSMXB where XM = '违约赔偿金' and XZ = '超过最迟发货日后录入发货信息' and LYDH in (" + Strwhere + ") and SJLX='预'";
                        DataSet dshwykmc = DbHelperSQL.Query(Strhwykmc);
                        if (dshwykmc != null && dshwykmc.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow drfpykk in dshwykmc.Tables[0].Rows)
                            {
                                hwje += Convert.ToDouble(drfpykk["JE"].ToString().Trim());
                            }

                            //卖家扣罚(发货单生成后5日内未录入发票邮寄信息)

                            drhkjd = dsZJBDMX.Tables[0].Select("1=1 and  Number = '1304000026'");
                            if (drhkjd != null && drhkjd.Length > 0)
                            {
                                drhkjd[0]["ZY"] = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());
                                string Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                                //发货单生成后5日内未录入发票邮寄信息sql
                                string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + time + "','" + drhkjd[0]["YSLX"].ToString() + "','" + hwje.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + drhkjd[0]["ZY"].ToString() + "','" + drhkjd[0]["SJLX"].ToString() + "','" + DLYX + "','" + time + "','监控编号' ) ";
                                list.Add(strinsehwqsfpyqkf);
                                sellyue -= hwje;
                            }
                            else
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                return returnDS;
                            }

                            //买家补赏(发货单生成后5日内未录入发票邮寄信息)

                            drhkjd = dsZJBDMX.Tables[0].Select("1=1 and  Number = '1304000036'");
                            if (drhkjd != null && drhkjd.Length > 0)
                            {
                                drhkjd[0]["ZY"] = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                string Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString())["结算账户类型"].ToString();

                                //发货单生成后5日内未录入发票邮寄信息sql
                                string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + BuyDLYX + "','" + jszhlx + "','" + JSBH + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + time + "','" + drhkjd[0]["YSLX"].ToString() + "','" + hwje.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + drhkjd[0]["ZY"].ToString() + "','" + drhkjd[0]["SJLX"].ToString() + "','" + DLYX + "','" + time + "','监控编号' ) ";
                                list.Add(strinsehwqsfpyqkf);
                                buyyue += hwje;
                            }
                            else
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                return returnDS;
                            }



                        }
                        #endregion
                        #region 反写中标定标信息表
                        string Strupdate = " update AAA_ZBDBXXB set Z_HTZT = '定标执行完成' , Z_QPKSSJ='" + time + "',Z_QPJSSJ='" + time + "' ,Z_QPZT='清盘结束' where Z_HTBH = '" + drinfo["电子购货合同"].ToString() + "' ";
                        list.Add(Strupdate);
                        #endregion


                    }
                    else
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到中标定标编号为：" + drinfo["中标定标单号"].ToString() + "的发货单！";
                        return returnDS;
                    }


                    #endregion

                    #region//向卖家、买家发出提醒
                    Hashtable htJYPT = new Hashtable();//交易平台
                    htJYPT["type"] = "集合集合经销平台";
                    htJYPT["提醒对象登陆邮箱"] = DLYX;
                    htJYPT["提醒对象用户名"] = drinfo["卖家名称"].ToString();
                    htJYPT["提醒对象结算账户类型"] = JSZHLX;
                    htJYPT["提醒对象角色编号"] = JSBH;
                    htJYPT["提醒对象角色类型"] = "卖家";
                    htJYPT["提醒内容文本"] = "尊敬的" + drinfo["卖家名称"].ToString() + "，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                    htJYPT["创建人"] = DLYX;
                    listHT.Add(htJYPT);

                    htJYPT = new Hashtable();//交易平台
                    htJYPT["type"] = "集合集合经销平台";
                    htJYPT["提醒对象登陆邮箱"] = BuyDLYX;
                    htJYPT["提醒对象用户名"] = ds.Tables[0].Rows[0]["提醒对象用户名"].ToString();
                    htJYPT["提醒对象结算账户类型"] = ds.Tables[0].Rows[0]["提醒对象结算账户类型"].ToString();
                    htJYPT["提醒对象角色编号"] = ds.Tables[0].Rows[0]["提醒对象角色编号"].ToString();
                    htJYPT["提醒对象角色类型"] = "买家";
                    htJYPT["提醒内容文本"] = "尊敬的" + ds.Tables[0].Rows[0]["提醒对象用户名"].ToString() +"，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                    htJYPT["创建人"] = ds.Tables[0].Rows[0]["创建人"].ToString();
                    listHT.Add(htJYPT);
                    #endregion
                }
                else if (zbNum < thbc + ythNum)
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "所有无异议收货的数量大于了总定标数量！";
                    return returnDS;
                }
                //更改账户余额
                string StrUpbuy = " update   AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE + " + buyyue.ToString("#0.00") + " where B_DLYX = '" + BuyDLYX + "'  ";
                list.Add(StrUpbuy);
                string Strupsell = " update   AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE + " + sellyue.ToString("#0.00") + " where B_DLYX = '" + DLYX + "'  ";
                list.Add(Strupsell);
                //更改单据状态
                string Strupda = " update AAA_THDYFHDXXB set F_DQZT='卖家主动退货' , F_BUYWYYSHCZSJ='" + time + "' where Number='" + THDBH + "' ";
                list.Add(Strupda);
                bool bl = DbHelperSQL.ExecSqlTran(list);
                if (bl)
                {
                    //**发货单卖家已同意退货，您相应货款已解冻。
                    //List<Hashtable> listHT = new List<Hashtable>();
                    Hashtable htJYPT = new Hashtable();//交易平台
                    htJYPT["type"] = "集合集合经销平台";
                    htJYPT["提醒对象登陆邮箱"] = BuyDLYX;
                    htJYPT["提醒对象用户名"] = ds.Tables[0].Rows[0]["提醒对象用户名"].ToString();
                    htJYPT["提醒对象结算账户类型"] = ds.Tables[0].Rows[0]["提醒对象结算账户类型"].ToString();
                    htJYPT["提醒对象角色编号"] = ds.Tables[0].Rows[0]["提醒对象角色编号"].ToString();
                    htJYPT["提醒对象角色类型"] = "买家";
                    htJYPT["提醒内容文本"] = "F" + THDBH + "发货单卖方已同意退货，您相应货款已解冻。";
                    htJYPT["创建人"] = ds.Tables[0].Rows[0]["创建人"].ToString();
                    listHT.Add(htJYPT);
                    PublicClass2013.Sendmes(listHT);


                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "卖方主动退货操作成功！";
                    return returnDS;
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "卖方主动退货操作失败！";
                    return returnDS;
                }
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到该提货单的信息！";
                return returnDS;
            }




            #endregion

        }
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";   
            return returnDS;
        }
    }


}