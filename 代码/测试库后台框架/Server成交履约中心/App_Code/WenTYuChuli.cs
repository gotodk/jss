using FMDBHelperClass;
using FMipcClass;
/*******************************************************************
 * 
 * 创建人：wyh
 *
 *创建时间：2014.06.05
 *
 *代码功能：实现问题预处理各项功能
 *
 *参考文档：《业务分析》成交履约中心部分
 * 
 * 
 * ******************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// WenTYuChuli 的摘要说明
/// </summary>
public class WenTYuChuli
{
	public WenTYuChuli()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 问题与处理 wyh 2014.06.10
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsreturn结构的dataset</returns>
    public DataSet WTYCL(DataSet ds)
    {
       
        DataSet returnDS = new TBD().initReturnDataSet().Copy();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        if (ds == null || ds.Tables[0].Rows.Count <= 0)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空。";
            return returnDS;
        }


        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input = new Hashtable();
        Hashtable htreturn = new Hashtable();

        try
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                TBD tbd = new TBD();
                string JSBH = ds.Tables[0].Rows[0]["买家角色编号"].ToString();//买家角色编号
                string THDBH = ds.Tables[0].Rows[0]["提货单编号"].ToString();//提货单编号
                string time = DateTime.Now.ToString();
                #region//基础验证
                //Hashtable JCYZ = PublicClass2013.GetParameterInfo();
                DataRow JCYZ = null;
                object[] re = IPC.Call("平台动态参数 ", new object[] { "" }); //----------------------
                DataSet dsUserVal = null;
                if (re[0].ToString() == "ok")
                {
                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。

                    dsUserVal = (DataSet)(re[1]);
                    if (dsUserVal == null || dsUserVal.Tables[0].Rows.Count <= 0)
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查找用户信息出错";
                        returnDS.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                        return returnDS;

                    }
                    else
                    {
                        JCYZ = dsUserVal.Tables[0].Rows[0];
                    }

                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                    returnDS.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    return returnDS;
                }
                #endregion
                #region//验证合同状态
                //----------------------wyh拆分支履约控制中心
                DataSet zt = new lvyueclass().GetTHDXXAndHTZT(THDBH.Trim());
                if (zt != null && zt.Tables[0].Rows.Count > 0)
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
                       
                       //----已拆分到成交履约中心 wyh 2014.06.10
                         DataSet dsZLSHB = new lvyueclass().GTTBZLSHinfo(THDBH.Trim()); //DbHelperSQL.Query(strTBDZLSH);
                        if (dsZLSHB != null && dsZLSHB.Tables[0].Rows.Count > 0)
                        {
                            if (dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "未审核" || dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                                return returnDS;
                            }
                        }
                        #endregion

                        string bfbz = ds.Tables[0].Rows[0]["补发备注"].ToString();
                        input["@F_BFBZ"] = bfbz;
                        input["@F_BFBZZHGXSJ"] = time;
                        input["@Number"] = THDBH;
                        string strSQLBF = "update AAA_THDYFHDXXB set F_BFBZ=@F_BFBZ,F_BFBZZHGXSJ=@F_BFBZZHGXSJ,F_DQZT='已录入补发备注',F_QMJQSQRCZSJ=null,F_QMJQSWLGSMC='',F_QMJQSWLDH='',F_QMJQSWLQSD='' where Number=@Number ";
                        int i = 0;
                        htreturn= I_DBL.RunParam_SQL(strSQLBF,input);
                        if ((bool)(htreturn["return_float"]))
                        {
                            i = (int)(htreturn["return_other"]);
                        }
                        else
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "提交请买方无异议收货时出错！";
                            return returnDS;
                        }

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

                        break;

                    case "请买方“无异议收货”"://提请买家签收                        
                        if (zt.Tables[0].Rows[0]["F_BFBZZHGXSJ"].ToString() != "")
                        { 
                            return InsertSQL(ds);
                        }
                        else
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "请先录入补发备注！";
                            return returnDS;
                        }
                        break;

                    case "卖方主动退货":
                        if (zt.Tables[0].Rows[0]["F_DQZT"].ToString() != "有异议收货")
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "OpIsInvalid";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作无效！";
                            return returnDS;
                        }
                        return SelTH(ds);
                    case "同意重新发货":
                        if (zt.Tables[0].Rows[0]["F_DQZT"].ToString() != "请重新发货")
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "OpIsInvalid";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作无效！";
                            return returnDS;
                        }
                        return CXFH(ds, JCYZ);
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

                        HWQSInfo hw = new HWQSInfo();
                        return hw.dsjhjx_hwqs(table);
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
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";
        }
        return returnDS;
    }
    //重新发货
    private DataSet CXFH(DataSet ds, DataRow JCYZ)
    {
        DataSet returnDS = new TBD().initReturnDataSet().Copy();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsparr = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable htreturn = new Hashtable();

        TBD tbd = new TBD();
        string DLYX = ds.Tables[0].Rows[0]["登录邮箱"].ToString();
        string JSZHLX = ds.Tables[0].Rows[0]["结算账户类型"].ToString();
        string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();
        string BuyDLYX = ds.Tables[0].Rows[0]["提醒对象登陆邮箱"].ToString();//买家登录邮箱
        string BuyJSZHLX = ds.Tables[0].Rows[0]["提醒对象结算账户类型"].ToString();//买家结算账户类型
        string BuyJSBH = ds.Tables[0].Rows[0]["提醒对象角色编号"].ToString();//买家角色编号
        string YSTBDDH = ds.Tables[0].Rows[0]["原始投标单单号"].ToString();//原始投标单单号
        ArrayList list = new ArrayList();
        try
        {
            #region//验证投标资料审核表对应的投标点是否审核通过
            //----已拆分到成交履约中心 zhouli 2014.07.07 add
            DataSet dsZLSHB = new lvyueclass().GTTBZLSHinfo(ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim()); //DbHelperSQL.Query(strTBDZLSH);
            if (dsZLSHB != null && dsZLSHB.Tables[0].Rows.Count > 0)
            {
                if (dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "未审核" || dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                    return returnDS;
                }
            }
            #endregion
            input["@THDBH"] = ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim();
            string strSelect = "select * from AAA_THDYFHDXXB where Number=@THDBH ";
            DataSet dsSelect = null;// DbHelperSQL.Query(strSelect);
            htreturn = I_DBL.RunParam_SQL(strSelect,"select",input);
            if ((bool)(htreturn["return_float"])) //说明执行完成
            {
                dsSelect = (DataSet)(htreturn["return_ds"]);
            }
            

            if (dsSelect != null && dsSelect.Tables["select"].Rows.Count > 0)
            {
                Hashtable htall = new Hashtable();
                htall.Add("@THDBH", ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim());
                string strCX = "update AAA_THDYFHDXXB set F_DQZT='撤销' where Number=@THDBH ";
                list.Add(strCX);

                #region//插入准备字段
                string number = tbd.GetKey("AAA_THDYFHDXXB","");//PublicClass2013.GetNextNumberZZ("AAA_THDYFHDXXB", "");
                if (number.Equals(""))
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "主键获取错误1";
                    return returnDS;
                }
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
                Hashtable htht = new Hashtable();
                htht["@Number"] = ZBDBBH.Trim();
                string strDB = "select convert(int,Z_ZBSL)-convert(int,Z_YTHSL) 可提货数量,Z_YTHSL 已提货数量,Z_ZBSL 中标数量,Z_HTQX 合同期限,Z_HTJSRQ 合同结束日期,Z_RJZGFHL 日均最高发货量,Z_DBSJ 定标时间,Z_HTBH  from AAA_ZBDBXXB where Z_HTZT='定标' and Number=@Number ";
                DataSet dsDB = null;//DbHelperSQL.Query(strDB);
                htreturn = I_DBL.RunParam_SQL(strDB,"TBD",htht);
                if ((bool)(htreturn["return_float"])) //说明执行完成
                {
                    dsDB = (DataSet)(htreturn["return_ds"]);
                }


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
                    Hashtable htthd = new Hashtable();
                    htthd["@YSTBDDH"] = YSTBDDH;
                    
                    #region//zhouli 2014.07.23 作废 需求变更
                    //htthd["@ZBDBXXBBH"] = ZBDBBH;
                    //string strSelTHD = "select isnull(sum(isnull(T_GZWFHRGTHDYPL,0)),0) 最晚最迟发货日已排总量,max(T_ZCFHR) 最迟发货日 from AAA_THDYFHDXXB where T_ZCFHR =(select max(T_ZCFHR)from AAA_THDYFHDXXB  where ZBDBXXBBH=@ZBDBXXBBH) and ZBDBXXBBH=@ZBDBXXBBH ";//对应标的的最晚最迟发货日已排总量、最迟发货日
                    #endregion
                    string strSelTHD = "select isnull(sum(isnull(T_GZWFHRGTHDYPL,0)),0) 最晚最迟发货日已排总量,max(T_ZCFHR) 最迟发货日 from AAA_THDYFHDXXB where T_ZCFHR =(select max(T_ZCFHR)from AAA_THDYFHDXXB  where ZBDBXXBBH in (select Number from AAA_ZBDBXXB where T_YSTBDBH=@YSTBDDH)) and ZBDBXXBBH in (select Number from AAA_ZBDBXXB where T_YSTBDBH=@YSTBDDH) ";//对应标的的最晚最迟发货日已排总量、最迟发货日

                    DataSet dsTHD = null;//DbHelperSQL.Query(strSelTHD);
                    htreturn = I_DBL.RunParam_SQL(strSelTHD, "selthd", htthd);
                    if ((bool)(htreturn["return_float"])) //说明执行完成
                    {
                        dsTHD = (DataSet)(htreturn["return_ds"]);
                    }
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

                htall["@number"] = number;
                htall["@ZBDBBH"] = ZBDBBH;
                htall["@ZBDJ"] = ZBDJ;
                htall["@THJJPLS"] = THJJPLS;
                htall["@THSL"] = THSL;
                htall["@newZCFHR"] = newZCFHR;
                htall["@newGZWFHRGTHDYPL"] = newGZWFHRGTHDYPL;
                htall["@SHXXDZ"] = SHXXDZ;
                htall["@SHRXM"] = SHRXM;
                htall["@SHLXFS"] = SHLXFS;
                htall["@DJHKJE"] = DJHKJE;
                htall["@FPLX"] = FPLX;
                htall["@FPTT"] = FPTT;
                htall["@SH"] = SH;
                htall["@KHYH"] = KHYH;
                htall["@YHZH"] = YHZH;
                htall["@DWDZ"] = DWDZ;
                htall["@time"] = time;
                htall["@fpsfshtx"] = fpsfshtx;
                
                htall["@FPHM"] = FPHM;
                htall["@FPYJDWMC"] = FPYJDWMC;
                htall["@FPYJDBH"] = FPYJDBH;
                htall["@DLYX"] = DLYX;
                

                if (FPYJXXLRSJ == "")
                {
                    htall["@FPYJXXLRSJ"] = null;
                    
                }
                else
                {
                    htall["@FPYJXXLRSJ"] = FPYJXXLRSJ;//
                }

                string strInsertTHDSQL = "INSERT INTO [AAA_THDYFHDXXB] ([Number],[ZBDBXXBBH],[ZBDJ],[T_THDJJPLS],[T_THSL],[T_ZCFHR],T_GZWFHRGTHDYPL,[T_SHXXDZ],[T_SHRXM],[T_SHRLXFS],[T_DJHKJE] ,[T_FPLX],[T_FPTT] ,[T_SH],[T_KHYH] ,[T_YHZH],[T_DWDZ],[T_THDXDSJ] ,F_FPSFSHTH,F_FPYJXXLRSJ,F_FPHM,F_FPYJDWMC,F_FPYJDBH,F_DQZT,F_SELTYZXFHCZSJ,[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES (@number,@ZBDBBH,@ZBDJ,@THJJPLS,@THSL,@newZCFHR,@newGZWFHRGTHDYPL,@SHXXDZ ,@SHRXM ,@SHLXFS, @DJHKJE ,@FPLX,@FPTT,@SH ,@KHYH ,@YHZH ,@DWDZ,@time,@fpsfshtx ,@FPYJXXLRSJ ,@FPHM ,@FPYJDWMC,@FPYJDBH,'未生成发货单',@time ,1,@DLYX, @time,@time)";
                list.Add(strInsertTHDSQL);

                #endregion

                #region//插入资金流水明细

                string zjnumber = tbd.GetKey("AAA_ZKLSMXB", ""); //PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                if (zjnumber.Equals(""))
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "账款流水明细表主键获取出错1！";
                    return returnDS;
                }
                //查询资金变动明细对照表
                string strZJBDMX = "select * from AAA_moneyDZB where Number in ('1304000017','1304000009')";
                DataSet dsZJBDMX = tbd.GetMoneydbzDS("1304000017");//DbHelperSQL.Query(strZJBDMX);
                string xm = "";//项目
                string xz = "";//性质
                string zy = "";//摘要
                string sjlx = "";//数据类型
                string yslx = "";//运算类型
                DataRow row1 = dsZJBDMX.Tables[0].Rows[0];//先解冻买家货款

                if (row1 != null)
                {
                    xm = row1["XM"].ToString();//项目
                    xz = row1["XZ"].ToString();//性质
                    zy = row1["ZY"].ToString().Replace("[x1]", "T" + ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim());//摘要
                    sjlx = row1["SJLX"].ToString();//数据类型
                    yslx = row1["YSLX"].ToString();//运算类型
                }

                htall["@zjnumber1"] = zjnumber;
                htall["@BuyDLYX"] = BuyDLYX;
                htall["@BuyJSZHLX"] = BuyJSZHLX;
                htall["@BuyJSBH"] = BuyJSBH;
                htall["@THDBH"] = ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim();
                htall["@time"] = time;
                htall["@yslx1"] = yslx;
                htall["@je1"] = Convert.ToDouble(DJHKJE);
                htall["@xm1"] = xm;
                htall["@xz1"] = xz;
                htall["@zy1"] = zy;
                htall["@sjlx1"] = sjlx;



                string strInsertZJLSSQL = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES (@zjnumber1,@BuyDLYX,@BuyJSZHLX,@BuyJSBH,'AAA_THDYFHDXXB',@THDBH,@time,@yslx1,@je1,@xm1,@xz1,@zy1,'','',@sjlx1,1,@DLYX ,@time,@time)";
                list.Add(strInsertZJLSSQL);

                zjnumber = tbd.GetKey("AAA_ZKLSMXB", "");//PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                if (zjnumber.Equals(""))
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "账款流水明细表主键获取出错2！";
                    return returnDS;
                }
                row1 = null;
                DataSet dsZJBDMXb = tbd.GetMoneydbzDS("1304000009");
                row1 = dsZJBDMXb.Tables[0].Rows[0];
                if (row1 != null )
                {
                    xm = row1["XM"].ToString();//项目
                    xz = row1["XZ"].ToString();//性质
                    zy = row1["ZY"].ToString().Replace("[x1]", "T" + number);//摘要
                    sjlx = row1["SJLX"].ToString();//数据类型
                    yslx = row1["YSLX"].ToString();//运算类型
                }

                htall["@zjnumber2"] = zjnumber;
                htall["@lydh"] = number;           
                htall["@yslx2"] = yslx;
                htall["@je2"] = Convert.ToDouble(DJHKJE);
                htall["@xm2"] = xm;
                htall["@xz2"] = xz;
                htall["@zy2"] = zy;
                htall["@sjlx2"] = sjlx;

                strInsertZJLSSQL = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES (@zjnumber2,@BuyDLYX,@BuyJSZHLX,@BuyJSBH,'AAA_THDYFHDXXB',@lydh,@time,@yslx2,@je2,@xm2,@xz2,@zy2,'','',@sjlx2,1,@DLYX,@time,@time)";
                list.Add(strInsertZJLSSQL);
                #endregion
                
                htreturn = I_DBL.RunParam_SQL(list, htall);
                if ((bool)(htreturn["return_float"])) //说明执行完成
                {
                   
                    //这里就可以用了。
                    if ((int)(htreturn["return_other"]) <= 0)
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "执行了0条语句！";
                        
                        return returnDS;
                    }

                }
                else
                {        
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "提交失败";
                    return returnDS;
                }

                #region 发送提醒
                DataTable dtmess = new TBD().dtmeww().Clone();
                DataRow htJYPT = dtmess.NewRow();

                string YHM = new TBD().GetYHM(DLYX);
                    htJYPT["type"] = "集合经销平台";
                    htJYPT["提醒对象登陆邮箱"] = ds.Tables[0].Rows[0]["提醒对象登陆邮箱"].ToString();
                  
                    htJYPT["提醒对象结算账户类型"] = ds.Tables[0].Rows[0]["提醒对象结算账户类型"].ToString();
                    htJYPT["提醒对象角色编号"] = ds.Tables[0].Rows[0]["提醒对象角色编号"].ToString();
                    htJYPT["提醒对象角色类型"] = "买家";
                    htJYPT["提醒内容文本"] = YHM + "卖方根据您的要求已同意重新发货，对应的T" + ds.Tables[0].Rows[0]["Number"].ToString().Trim() + "提货单与F" + ds.Tables[0].Rows[0]["Number"].ToString().Trim() + "发货单作废，卖方将根据系统自动新生成的T" + number + "提货单重新发货。";
                    htJYPT["创建人"] = ds.Tables[0].Rows[0]["创建人"].ToString();
                    dtmess.Rows.Add(htJYPT);


                    DataRow htYWPT = dtmess.NewRow();//业务操作平台
                    htYWPT["type"] = "业务平台";
                    htYWPT["模提醒模块名"] = "卖家重新发货";
                    htYWPT["提醒内容文本"] = YHM + "卖方根据您的要求已同意重新发货，对应的T" + ds.Tables[0].Rows[0]["Number"].ToString().Trim() + "提货单与F" + ds.Tables[0].Rows[0]["Number"].ToString().Trim() + "发货单作废，卖方将根据系统自动新生成的T" + number + "提货单重新发货。";
                    htYWPT["查看地址"] = "";
                    dtmess.Rows.Add(htYWPT);

                    DataSet dsmeww = new DataSet();
                    dsmeww.Tables.Add(dtmess);

                    //调用系统控制中心发送提醒---------------------- wyh 2014.06.09 以后可能会做成一步，不必判断对错。

                    object[] re = IPC.Call("发送提醒", new object[] { dsmeww });
                    
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "同意重新发货操作成功！";
                    return returnDS;
                 #endregion
               
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到F" + ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim() + "发货单的信息。";
                return returnDS;
            }
        }
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            //returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = e.Message;
            return returnDS;
        }

    }
    //卖家主动退货
    private DataSet SelTH(DataSet ds)
    {
        TBD tbd = new TBD();
        DataSet returnDS = tbd.initReturnDataSet().Copy();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        DataTable dtmess = tbd.dtmeww().Clone();

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsparr = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable htreturn = new Hashtable();
        DataSet dsmess = new DataSet();

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
            //----已拆分到成交履约中心 zhouli 2014.07.07 add
            DataSet dsZLSHB = new lvyueclass().GTTBZLSHinfo(ds.Tables[0].Rows[0]["提货单编号"].ToString().Trim()); //DbHelperSQL.Query(strTBDZLSH);
            if (dsZLSHB != null && dsZLSHB.Tables[0].Rows.Count > 0)
            {
                if (dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "未审核" || dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                    return returnDS;
                }
            }
            #endregion

            #region//查询当前提货单的状态
            string strSQLTHD = "select F_DQZT from AAA_THDYFHDXXB where Number='" + THDBH + "'";
            htreturn= I_DBL.RunProc(strSQLTHD,"YHD");
            string zt = "";
            if ((bool)(htreturn["return_float"])) //说明执行完成
            {
                //这里就可以用了。
                if (htreturn["return_ds"] != null && ((DataSet)(htreturn["return_ds"])).Tables["YHD"].Rows.Count>0)
                {
                    zt = ((DataSet)(htreturn["return_ds"])).Tables["YHD"].Rows[0][0].ToString();
                }
                
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "该发货单买方已“改为无异议收货”！";
                return returnDS;
            }

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
           // ClassMoney2013 money = new ClassMoney2013();
            //buyyue = money.GetMoneyT(BuyDLYX, "");//账户当前可用余额
            //sellyue = money.GetMoneyT(DLYX, "");//账户当前可用余额
            #endregion


            #region 满足买家无异议签收数量=定标数量是单据处理
            input["@Number"] = THDBH;
            input["@Y_YSYDDDLYX"] = BuyDLYX;
            string strTHDAndDB = "select a.Number,('T'+a.Number) as  提货单号,('F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖家名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买家角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.T_YSTBDMJJSBH as 卖家角色编号,b.T_YSTBDDLYX as 卖家登陆邮箱,b.T_YSTBDJSZHLX as 卖家角色账户类型,b.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,b.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,b.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,b.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,b.Z_ZBSL as 定标数量,b.Z_LYBZJJE as 履约保证金金额,b.Z_HTZT as 合同状态  from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX  where a.Number=@Number and b.Y_YSYDDDLYX=@Y_YSYDDDLYX";

            DataSet dsinfo = null;//DbHelperSQL.Query(strTHDAndDB);
            htreturn = I_DBL.RunParam_SQL(strTHDAndDB,"qssl",input);
            if ((bool)(htreturn["return_float"])) //说明执行完成
            {
                if (htreturn["return_ds"] != null && ((DataSet)(htreturn["return_ds"])).Tables["qssl"].Rows.Count > 0)
                {
                    dsinfo = (DataSet)(htreturn["return_ds"]);
                }

            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "该发货单买方已“改为无异议收货”！";
                return returnDS;
            }

            if (dsinfo != null && dsinfo.Tables["qssl"].Rows.Count > 0)
            {
                DataRow drinfo = dsinfo.Tables["qssl"].Rows[0];
                Hashtable htall = new Hashtable();
                #region//解冻买家货款，写入资金流水明细表
                #region//zhouli作废 -- 替换成redis--2014.11.05
                //string ZJBD = "select  Number,'' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number in ('1304000016','1304000048','1304000028','1304000037','1304000026','1304000036')";
                //DataSet dsZJBDMX = null;//DbHelperSQL.Query(ZJBD);
                //htreturn = I_DBL.RunProc(ZJBD, "MDXB");
                //if ((bool)(htreturn["return_float"])) //说明执行完成
                //{
                //    if (htreturn["return_ds"] != null && ((DataSet)(htreturn["return_ds"])).Tables["MDXB"].Rows.Count > 0)
                //    {
                //        dsZJBDMX = (DataSet)(htreturn["return_ds"]);
                //    }
                //    else
                //    {
                //        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "获取资金对照表数据是出错！";
                //        return returnDS;
                //    }

                //}
                //else
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "该发货单买方已“改为无异议收货”！";
                //    return returnDS;
                //}
                #endregion
                DataSet dsZJBDMX = IsMoneyDZB.ReadRedisMoneyDZB(new string[] { "1304000016", "1304000048", "1304000028", "1304000037", "1304000026", "1304000036" }, null, "MDXB");

                string xm = "";//项目
                string xz = "";//性质
                string zy = "";//摘要
                string sjlx = "";//数据类型
                string yslx = "";//运算类型
                DataRow[] row1 = dsZJBDMX.Tables["MDXB"].Select("1=1 and Number='1304000016'");
                if (row1 != null && row1.Length > 0)
                {
                    xm = row1[0]["XM"].ToString();//项目
                    xz = row1[0]["XZ"].ToString();//性质
                    zy = row1[0]["ZY"].ToString().Replace("[x1]", "F" + THDBH);//摘要
                    sjlx = row1[0]["SJLX"].ToString();//数据类型
                    yslx = row1[0]["YSLX"].ToString();//运算类型
                }
                string zjnumber = tbd.GetKey("AAA_ZKLSMXB", "");

                if (zjnumber.Equals(""))
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "主键获取错误1！";
                    return returnDS;
                }

                htall["@Number1"] = zjnumber;
                htall["@BuyDLYX"] = BuyDLYX;
                htall["BuyJSZHLX"] = BuyJSZHLX;
                htall["@BuyJSBH"] = BuyJSBH;
                htall["@LYDH"] = THDBH;
                htall["@time"] = time;
                htall["@YSLX1"] = yslx;
                htall["@je1"] = dsinfo.Tables[0].Rows[0]["发货金额"].ToString();
                htall["@xm1"] = xm;
                htall["@xz1"] = xz;
                htall["@zy1"] = zy;
                htall["@sjlx1"] = sjlx;
                htall["@DLYX"] = DLYX;

                string strInsertZJLSSQL = "INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES (@Number1,@BuyDLYX,@BuyJSZHLX,@BuyJSBH,'AAA_THDYFHDXXB',@LYDH,@time,@YSLX1,@je1,@xm1,@xz1,@zy1,'','',@sjlx1,1,@DLYX,@time,@time)";
                list.Add(strInsertZJLSSQL);
                buyyue += Convert.ToDouble(dsinfo.Tables[0].Rows[0]["发货金额"].ToString());
                #endregion

                int zbNum = Convert.ToInt32(drinfo["定标数量"].ToString().Trim());
                int thbc = Convert.ToInt32(drinfo["提货数量"].ToString().Trim());
                int ythNum = 0;

                Hashtable htaa = new Hashtable();
               
                string StrgetyithNum = "select  isnull(SUM (ISNULL(T_THSL,0)),0) as  已提货数量,ZBDBXXBBH     from AAA_THDYFHDXXB where ZBDBXXBBH = '" + drinfo["中标定标单号"].ToString() + "' and F_DQZT in ( '无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货','卖家主动退货' ) group by ZBDBXXBBH";

                DataSet dsyth = null; //DbHelperSQL.Query(StrgetyithNum);
                htreturn = I_DBL.RunProc(StrgetyithNum, "THSL");
                if ((bool)(htreturn["return_float"]))
                {
                    dsyth = (DataSet)htreturn["return_ds"];
                }
                
                if (dsyth != null && dsyth.Tables[0].Rows.Count > 0)
                {
                    ythNum = Convert.ToInt32(dsyth.Tables[0].Rows[0]["已提货数量"].ToString());
                }
            
                if (zbNum == thbc + ythNum)
                 {
                    #region 解冻卖家履约保证金
                    //解冻卖家履约保证金
                    double lvbzj = Convert.ToDouble(drinfo["履约保证金金额"].ToString().Trim());
                    //无异议提货解冻卖家履约保证金
                    DataRow[] drhkjd = dsZJBDMX.Tables["MDXB"].Select("1=1 and  Number = '1304000048'");
                    if (drhkjd != null && drhkjd.Length > 0)
                    {
                        drhkjd[0]["ZY"] = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                        string Keynumber = tbd.GetKey("AAA_ZKLSMXB", ""); //PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string jszhlx = tbd.Getjszhlx(drinfo["卖家角色编号"].ToString()); //PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                        if (jszhlx.Equals("") || Keynumber.Equals(""))
                        {
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "获取基础数据出错2！";
                            return returnDS;
                        }

                        htall["@Keynumber2"] = Keynumber;
                        htall["@selldlyx"] = drinfo["卖家登陆邮箱"].ToString();
                        htall["@jszhlx2"] = jszhlx;
                        htall["@selljsbh"] = drinfo["卖家角色编号"].ToString();
                        htall["@zbdbNum"] = drinfo["中标定标单号"].ToString();
                        htall["@YSLX2"] = drhkjd[0]["YSLX"].ToString();
                        htall["@je2"] = drinfo["履约保证金金额"].ToString();
                        htall["@xm2"] = drhkjd[0]["XM"].ToString();
                        htall["@XZ2"] = drhkjd[0]["XZ"].ToString();
                        htall["@ZY2"] = drhkjd[0]["ZY"].ToString();
                        htall["@SJLX2"] = drhkjd[0]["SJLX"].ToString();


                        //无异议提货解冻卖家履约保证金sql
                        string strinsehwqsjdmjlybzj = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber2,@selldlyx,@jszhlx2,@selljsbh,'AAA_ZBDBXXB',@zbdbNum,@time,@YSLX2,@je2,@xm2,@XZ2,@ZY2,@SJLX2,@DLYX,@time,'监控编号' ) ";
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
                    htreturn = I_DBL.RunProc(GetTHdNumber,"THD");
                    DataSet dsfpyq = null;
                    if ((bool)(htreturn["return_float"]))
                    {
                        dsfpyq = (DataSet)htreturn["return_ds"];
                    }

                    if (dsfpyq != null && dsfpyq.Tables["THD"].Rows.Count > 0)
                    {
                        string Strwhere = "";
                        foreach (DataRow dr in dsfpyq.Tables["THD"].Rows)
                        {
                            Strwhere += "'" + dr["Number"].ToString().Trim() + "',";
                        }

                        Strwhere = Strwhere.Trim().Remove(Strwhere.Length - 1);

                        # region 发票延期的扣罚与补赏
                        //获取次投标定标预订单的发票延迟预扣款流水明细
                        double je = 0.00;
                        string Strfpykmc = "select Number, JE from AAA_ZKLSMXB where XM = '违约赔偿金' and XZ = '发货单生成后5日内未录入发票邮寄信息' and LYDH in (" + Strwhere + ") and SJLX='预'";
                        DataSet dsfpykmc = null; //= DbHelperSQL.Query(Strfpykmc);
                        htreturn = I_DBL.RunProc(Strfpykmc,"kf");
                        if ((bool)(htreturn["return_float"]))
                        {
                            dsfpykmc = (DataSet)htreturn["return_ds"];
                        }

                        if (dsfpykmc != null && dsfpykmc.Tables["kf"].Rows.Count > 0)
                        {
                            foreach (DataRow drfpykk in dsfpykmc.Tables["kf"].Rows)
                            {
                                je += Convert.ToDouble(drfpykk["JE"].ToString().Trim());
                            }

                            #region //卖家扣罚(发货单生成后5日内未录入发票邮寄信息)

                            drhkjd = dsZJBDMX.Tables["MDXB"].Select("1=1 and  Number = '1304000028'");
                            if (drhkjd != null && drhkjd.Length > 0)
                            {
                                drhkjd[0]["ZY"] = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                string Keynumber = tbd.GetKey("AAA_ZKLSMXB", "");
                                //string jszhlx = tbd.Getjszhlx(drinfo["卖家角色编号"].ToString());

                                if (Keynumber.Equals(""))
                                {
                                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "获取基础数据出错3！";
                                    return returnDS;
                                }
                                //发货单生成后5日内未录入发票邮寄信息sql

                                htall["@Keynumber3"] = Keynumber;
                                htall["@lydh3"] = drinfo["中标定标单号"].ToString();//drinfo["Number"].ToString();
                                htall["@YSLX3"] = drhkjd[0]["YSLX"].ToString();
                                htall["@je3"] = je.ToString();
                                htall["@XM3"] = drhkjd[0]["XM"].ToString();
                                htall["@XZ3"] = drhkjd[0]["XZ"].ToString();
                                htall["@ZY3"] = drhkjd[0]["ZY"].ToString();
                                htall["@SJLX3"] = drhkjd[0]["SJLX"].ToString();

                                string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber3,@selldlyx,@jszhlx2,@selljsbh,'AAA_ZBDBXXB',@lydh3,@time,@YSLX3,@je3,@XM3,@XZ3,@ZY3,@SJLX3,@DLYX,@time,'监控编号' ) ";
                                list.Add(strinsehwqsfpyqkf);
                                sellyue -= je;
                            }
                            else
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "日内未录入发票邮寄信息未找到！";
                                return returnDS;
                            }
                            #endregion

                            #region//买家补赏(发货单生成后5日内未录入发货信息)

                            drhkjd = dsZJBDMX.Tables["MDXB"].Select("1=1 and  Number = '1304000037'");
                            if (drhkjd != null && drhkjd.Length > 0)
                            {
                                drhkjd[0]["ZY"] = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                string Keynumber = tbd.GetKey("AAA_ZKLSMXB", "");
                                string jszhlx = tbd.Getjszhlx(drinfo["买家角色编号"].ToString());

                                if (Keynumber.Equals("") || jszhlx.Equals(""))
                                {
                                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "获取基础数据出错4！";
                                    return returnDS;
                                }

                                htall["@Keynumber4"] = Keynumber;
                                htall["@DLYX4"] = BuyDLYX;//drinfo["登陆邮箱"].ToString();
                                htall["@jszhlx4"] = jszhlx;
                                htall["@buyjsbh4"] = drinfo["买家角色编号"].ToString();
                                //htall["@lydh3"] = drinfo["Number"].ToString();
                                htall["@YSLX4"] = drhkjd[0]["YSLX"].ToString();
                                htall["@je4"] = je.ToString();
                                htall["@XM4"] = drhkjd[0]["XM"].ToString();
                                htall["@XZ4"] = drhkjd[0]["XZ"].ToString();
                                htall["@ZY4"] = drhkjd[0]["ZY"].ToString();
                                htall["@SJLX4"] = drhkjd[0]["SJLX"].ToString();
                                
                                //发货单生成后5日内未录入发票邮寄信息sql
                                string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber4,@DLYX4,@jszhlx4,@buyjsbh4,'AAA_ZBDBXXB',@lydh3,@time,@YSLX4,@je4,@XM4,@XZ4,@ZY4,@SJLX4,@DLYX,@time,'监控编号' ) ";
                                list.Add(strinsehwqsfpyqkf);
                                buyyue += je;
                            }
                            else
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                return returnDS;
                            }
                            #endregion


                        }
                        #endregion

                        #region  卖家发货延期的扣罚与补偿
                        //获取次投标定标预订单的发票延迟预扣款流水明细
                        double hwje = 0.00;
                        string Strhwykmc = "select Number, JE from AAA_ZKLSMXB where XM = '违约赔偿金' and XZ = '超过最迟发货日后录入发货信息' and LYDH in (" + Strwhere + ") and SJLX='预'";
                        DataSet dshwykmc = null; //DbHelperSQL.Query(Strhwykmc);
                        htreturn = I_DBL.RunProc(Strhwykmc,"je");
                        if ((bool)(htreturn["return_float"]))
                        {
                            dshwykmc = (DataSet)htreturn["return_ds"];
                        }

                        if (dshwykmc != null && dshwykmc.Tables["je"].Rows.Count > 0)
                        {
                            foreach (DataRow drfpykk in dshwykmc.Tables["je"].Rows)
                            {
                                hwje += Convert.ToDouble(drfpykk["JE"].ToString().Trim());
                            }

                            #region//卖家扣罚(发货单生成后5日内未录入发票邮寄信息)

                            drhkjd = dsZJBDMX.Tables["MDXB"].Select("1=1 and  Number = '1304000026'");
                            if (drhkjd != null && drhkjd.Length > 0)
                            {
                                drhkjd[0]["ZY"] = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]",  drinfo["电子购货合同"].ToString().Trim());
                                string Keynumber = tbd.GetKey("AAA_ZKLSMXB", ""); //PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                //string jszhlx2 = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                                if (Keynumber.Equals(""))
                                {
                                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "获取基础数据出错5！";
                                    return returnDS;
                                }
                                htall["@Keynumber5"] = Keynumber;

                                //htall["@lydh3"] = drinfo["Number"].ToString();
                                htall["@YSLX5"] = drhkjd[0]["YSLX"].ToString();
                                htall["@je5"] = hwje.ToString();
                                htall["@XM5"] = drhkjd[0]["XM"].ToString();
                                htall["@XZ5"] = drhkjd[0]["XZ"].ToString();
                                htall["@ZY5"] = drhkjd[0]["ZY"].ToString();
                                htall["@SJLX5"] = drhkjd[0]["SJLX"].ToString();
                                //发货单生成后5日内未录入发票邮寄信息sql
                                string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber5,@selldlyx,@jszhlx2,@selljsbh,'AAA_ZBDBXXB',@lydh3,@time,@YSLX5,@je5,@XM5,@XZ5,@ZY5,@SJLX5,@DLYX ,@time,'监控编号' ) ";
                                list.Add(strinsehwqsfpyqkf);
                                sellyue -= hwje;
                            }
                            else
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                return returnDS;
                            }
                            #endregion
                            #region//买家补赏(发货单生成后5日内未录入发票邮寄信息)

                            drhkjd = dsZJBDMX.Tables["MDXB"].Select("1=1 and  Number = '1304000036'");
                            if (drhkjd != null && drhkjd.Length > 0)
                            {
                                drhkjd[0]["ZY"] = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                string Keynumber = tbd.GetKey("AAA_ZKLSMXB", "");//PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                if (Keynumber.Equals(""))
                                {
                                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "获取基础数据出错6！";
                                    return returnDS;
                                }
                                htall["@Keynumber6"] = Keynumber;
                                //string jszhlx4 = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString())["结算账户类型"].ToString();
                                htall["@YSLX6"] = drhkjd[0]["YSLX"].ToString();
                                htall["@je6"] = hwje.ToString();
                                htall["@XM6"] = drhkjd[0]["XM"].ToString();
                                htall["@XZ6"] = drhkjd[0]["XZ"].ToString();
                                htall["@ZY6"] = drhkjd[0]["ZY"].ToString();
                                htall["@SJLX6"] = drhkjd[0]["SJLX"].ToString();
                                htall["@jsbh6"] = JSBH;
                                 htall["@DLYX4"] = BuyDLYX;
                                 string jszhlx = tbd.Getjszhlx(drinfo["买家角色编号"].ToString());
                                 htall["@jszhlx4"] = jszhlx;
                                //发货单生成后5日内未录入发票邮寄信息sql
                                 string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber6,@DLYX4,@jszhlx4,@jsbh6,'AAA_ZBDBXXB',@lydh3,@time,@YSLX6,@je6,@XM6,@XZ6,@ZY6,@SJLX6,@DLYX ,@time,'监控编号' ) ";
                                list.Add(strinsehwqsfpyqkf);
                                buyyue += hwje;
                            }
                            else
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                return returnDS;
                            }
                            #endregion


                        }
                        #endregion
                        #region 反写中标定标信息表
                        htall["@dzghht"] = drinfo["电子购货合同"].ToString();
                        string Strupdate = " update AAA_ZBDBXXB set Z_HTZT = '定标执行完成' , Z_QPKSSJ=@time,Z_QPJSSJ=@time ,Z_QPZT='清盘结束' where Z_HTBH = @dzghht ";
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

     
                    DataRow htJYPT = dtmess.NewRow();

                    //Hashtable htJYPT = new Hashtable();//交易平台
                    htJYPT["type"] = "集合经销平台";
                    htJYPT["提醒对象登陆邮箱"] = DLYX;
                  
                    htJYPT["提醒对象结算账户类型"] = JSZHLX;
                    htJYPT["提醒对象角色编号"] = JSBH;
                    htJYPT["提醒对象角色类型"] = "卖家";
                    htJYPT["提醒内容文本"] = "尊敬的" + drinfo["卖家名称"].ToString() + "，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                    htJYPT["创建人"] = DLYX;
                    dtmess.Rows.Add(htJYPT);

                    htJYPT = dtmess.NewRow();//交易平台
                    htJYPT["type"] = "集合经销平台";
                    htJYPT["提醒对象登陆邮箱"] = BuyDLYX;
                    //htJYPT["提醒对象用户名"] = ds.Tables[0].Rows[0]["提醒对象用户名"].ToString();
                    htJYPT["提醒对象结算账户类型"] = ds.Tables[0].Rows[0]["提醒对象结算账户类型"].ToString();
                    htJYPT["提醒对象角色编号"] = ds.Tables[0].Rows[0]["提醒对象角色编号"].ToString();
                    htJYPT["提醒对象角色类型"] = "买家";
                    htJYPT["提醒内容文本"] = "尊敬的" + ds.Tables[0].Rows[0]["提醒对象用户名"].ToString() + "，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                    htJYPT["创建人"] = ds.Tables[0].Rows[0]["创建人"].ToString();
                    dtmess.Rows.Add(htJYPT);

                    dtmess.AcceptChanges();

                   

                    
                    #endregion

                   
                }
                else if (zbNum < thbc + ythNum)
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "所有无异议收货的数量大于了总定标数量！";
                    return returnDS;
                }
                //更改账户余额
                htall["@buyyue"] = buyyue;
                string StrUpbuy = " update   AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE + @buyyue  where B_DLYX = @BuyDLYX  ";
                list.Add(StrUpbuy);
                htall["@sellyue"] = sellyue;
                string Strupsell = " update   AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE + @sellyue where B_DLYX = @DLYX ";
                list.Add(Strupsell);
                //更改单据状态
                htall["@THDBH"] = THDBH;
                string Strupda = " update AAA_THDYFHDXXB set F_DQZT='卖家主动退货' , F_SELZDTHCZSJ=@time  where Number=@THDBH ";
                list.Add(Strupda);
                htreturn = I_DBL.RunParam_SQL(list, htall);

                if ((bool)htreturn["return_float"])
                {
                    if ((int)htreturn["return_other"] <= 0)
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_other"].ToString() + "条数据被更新。";
                        return returnDS;
                    }
                }
                else
                {
                    
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_errmsg"].ToString();
                    return returnDS;
                }


                    //**发货单卖家已同意退货，您相应货款已解冻。
                    //List<Hashtable> listHT = new List<Hashtable>();

                    DataRow mes = dtmess.NewRow();
                    mes["type"] = "集合经销平台";
                    mes["提醒对象登陆邮箱"] = BuyDLYX;
                    //mes["提醒对象用户名"] = ds.Tables[0].Rows[0]["提醒对象用户名"].ToString();
                    mes["提醒对象结算账户类型"] = ds.Tables[0].Rows[0]["提醒对象结算账户类型"].ToString();
                    mes["提醒对象角色编号"] = ds.Tables[0].Rows[0]["提醒对象角色编号"].ToString();
                    mes["提醒对象角色类型"] = "买家";
                    mes["提醒内容文本"] = "F" + THDBH + "发货单卖方已同意退货，您相应货款已解冻。";
                    mes["创建人"] = ds.Tables[0].Rows[0]["创建人"].ToString();
                    dtmess.Rows.Add(mes);
                    dtmess.AcceptChanges();
                   
                    //调用系统控制中心发送提醒---------------------- wyh 2014.06.09 以后可能会做成一步，不必判断对错。
                    dsmess.Tables.Add(dtmess);
                    object[] re = IPC.Call("发送提醒", new object[] { dsmess });
                    #region 交易方信用变化情况 edittime 2013-09 准备移植到用户控制中心。
                    //---------------------wyh 2014.07.28 add


                    DataTable dt = new DataTable("jf");
                    DataColumn dc = new DataColumn("合同编号", typeof(string));
                    dt.Columns.Add(dc);
                    dt.Clear();
                    DataRow drht = dt.NewRow();
                    drht["合同编号"] = drinfo["电子购货合同"].ToString().Trim();
                    dt.Rows.Add(drht);
                    IPC.Call("信用等级积分处理", new object[] { "卖家主动退货", dt });


                    #endregion    


                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "卖方主动退货操作成功！";
                    return returnDS;
               
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
    //请买方无异议收获
    public DataSet InsertSQL(DataSet ds)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsparr = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable htreturn = new Hashtable();

        DataSet returnDS = new TBD().initReturnDataSet().Copy();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        
        try
        {

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();
                string JSZHLX = ds.Tables[0].Rows[0]["结算账户类型"].ToString();
                string FHD = ds.Tables[0].Rows[0]["发货单号"].ToString();
                string WLGSMC = ds.Tables[0].Rows[0]["物流公司名称"].ToString();
                string WLDH = ds.Tables[0].Rows[0]["物流单号"].ToString();
                string WLQSD = ds.Tables[0].Rows[0]["物流签收单"].ToString();
                string TXDXDLYX = "";//提醒对象登陆邮箱
                string TXDXYHM = "";//提醒对象用户名
                string TXDXJSZHLX = "";//提醒对象结算账户类型
                string TXDXJSBH = "";//提醒对象角色编号
                string TXDXJSLX = "买家";//提醒对象角色类型
                string TXNR = "";//提醒内容文本
                string CreaterUser = "";//创建人
                string time = DateTime.Now.ToString();

                if (JSZHLX == "经纪人交易账户")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "抱歉！只有卖方才能执行此类指令。";
                    return returnDS;
                }

                #region//验证合同状态
                //-----------已拆分至成交履约中心 wyh 2014.06.10
                DataSet zt = new lvyueclass().GetTHDXXAndHTZT(FHD.Trim()); //DbHelperSQL.Query(strSQLDB);
                if (zt != null && zt.Tables[0].Rows.Count > 0)
                {
                    #region//对发货单状态的验证
                  
                    if (zt.Tables[0].Rows[0]["原始投标单卖家角色编号"].ToString() != JSBH)
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "抱歉，只有卖方才能执行此操作！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["发货单状态"].ToString() == "未生成发货单" || zt.Tables[0].Rows[0]["发货单状态"].ToString() == "已生成发货单")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "抱歉，录入发货信息后才能执行此操作！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["发货单状态"].ToString() == "无异议收货" || zt.Tables[0].Rows[0]["发货单状态"].ToString() == "默认无异议收货" || zt.Tables[0].Rows[0]["发货单状态"].ToString() == "有异议收货后无异议收货")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单已签收，不能执行此操作！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["合同状态"].ToString() == "定标合同到期")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单对应的合同已到期！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["合同状态"].ToString() == "定标合同终止")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单对应的合同已终止！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["合同状态"].ToString() == "定标执行完成")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单对应的合同已完成！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["合同状态"].ToString() == "中标")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单对应的合同尚未定标，不能执行此操作！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["合同状态"].ToString() == "未定标废标")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单对应的合同已废标，不能执行此操作！";
                        return returnDS;
                    }
                    #endregion
                    TXDXDLYX = zt.Tables[0].Rows[0]["提醒对象登陆邮箱"].ToString();//提醒对象登陆邮箱
                    TXDXYHM = zt.Tables[0].Rows[0]["提醒对象用户名"].ToString();//提醒对象用户名
                    TXDXJSZHLX = zt.Tables[0].Rows[0]["提醒对象结算账户类型"].ToString();//提醒对象结算账户类型
                    TXDXJSBH = zt.Tables[0].Rows[0]["提醒对象角色编号"].ToString();//提醒对象角色编号
                    TXNR = zt.Tables[0].Rows[0]["卖家用户名"].ToString() + "卖方就 F" + FHD + " 发货单向您发出“提请买方签收”的通知，自通知发出之日起三日后系统将默认您“无异议收货”，全部货款即自动支付。您如有异议，请即致电平台服务人员！";//提醒内容文本
                    CreaterUser = zt.Tables[0].Rows[0]["原始投标单登录邮箱"].ToString();//创建人
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单状态获取失败！";
                    return returnDS;
                }
                #endregion

                #region//执行SQL
                input["@F_QMJQSQRCZSJ"] = time;
                input["@F_QMJQSWLGSMC"] = WLGSMC;
                input["@F_QMJQSWLDH"] = WLDH;
                input["@F_QMJQSWLQSD"] = WLQSD;
                input["@Number"] = FHD;
                string strUpdateTHD = "update AAA_THDYFHDXXB set F_QMJQSQRCZSJ=@F_QMJQSQRCZSJ,F_QMJQSWLGSMC=@F_QMJQSWLGSMC,F_QMJQSWLDH=@F_QMJQSWLDH,F_QMJQSWLQSD=@F_QMJQSWLQSD where Number=@Number ";
                htreturn = I_DBL.RunParam_SQL(strUpdateTHD, input);
                int i = 0;
                if ((bool)(htreturn["return_float"]))
                {
                    i = (int)(htreturn["return_other"]);
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "提交请买方无异议收货时出错！";
                    return returnDS;
                }
                
                if (i > 0)
                {
                    DataTable dtmess = new TBD().dtmeww().Clone();
                    DataRow htJYPT = dtmess.NewRow();
                    htJYPT["type"] = "集合经销平台";
                    htJYPT["提醒对象登陆邮箱"] = TXDXDLYX;
                    
                    htJYPT["提醒对象结算账户类型"] = TXDXJSZHLX;
                    htJYPT["提醒对象角色编号"] = TXDXJSBH;
                    htJYPT["提醒对象角色类型"] = TXDXJSLX;
                    htJYPT["提醒内容文本"] = TXNR;
                    htJYPT["创建人"] = CreaterUser;
                    dtmess.Rows.Add(htJYPT);

                    DataRow htYWPT = dtmess.NewRow(); ;//业务操作平台
                    htYWPT["type"] = "业务平台";
                    htYWPT["模提醒模块名"] = "请买方无异议收获";
                    htYWPT["提醒内容文本"] = TXNR;
                    htYWPT["查看地址"] = "abcdf";
                    dtmess.Rows.Add(htYWPT);
                    dtmess.AcceptChanges();

                    DataSet dsmess = new DataSet();
                    dsmess.Tables.Add(dtmess);
                    //调用系统控制中心发送提醒---------------------- wyh 2014.06.09 以后可能会做成一步，不必判断对错。

                    object[] re = IPC.Call("发送提醒", new object[] { dsmess });

                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "提请信息添加成功！";
                    return returnDS;
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "提请信息添加失败！";
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

    /// <summary>
    /// 获取异常信息 wyh 2014.06.11 
    /// </summary>
    /// <param name="ht">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset，内含有表明为：info的Datatable</returns>
    public DataSet Getyiwaiinfo(DataTable ht)
    {
        DataSet dsreturn = new TBD().initReturnDataSet().Copy();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input = new Hashtable();
        Hashtable htreturn = new Hashtable();

        DataRow dr = ht.Rows[0];
        string Strsql = " select " + dr["cloumn"].ToString() + ", (case  F_DQZT when '撤销'  then '同意重新发货' else replace(cast(F_DQZT as varchar(8000)),'家','方') end) as F_DQZT  from AAA_THDYFHDXXB where Number='" + dr["Number"].ToString().Trim() + "'";
        DataSet dsinfo = null;//DbHelperSQL.Query(Strsql);
        htreturn = I_DBL.RunProc(Strsql,"yicinfo");
        if ((bool)(htreturn["return_float"]))
        {
            dsinfo = (DataSet)(htreturn["return_ds"]);
        }
        if (dsinfo != null && dsinfo.Tables[0].Rows.Count == 1)
        {
            DataTable dt = dsinfo.Tables[0].Copy();
            dt.TableName = "info";
            dsreturn.Tables.Add(dt);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = " 找到数据！";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = " 数据查找出错！";
        }
        return dsreturn;

    }

   
}