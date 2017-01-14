using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;

/// <summary>
/// TQBuyQSClass 的摘要说明
/// </summary>
public class TQBuyQSClass
{
	public TQBuyQSClass()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    //添加提请信息
    public DataSet InsertSQL(DataSet ds, DataSet returnDS)
    {
        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "提请信息添加成功！";
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

                #region//基础验证
                Hashtable JCYZ = PublicClass2013.GetParameterInfo();
                Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
                if (JBXX["交易账户是否开通"].ToString() == "否")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                    return returnDS;
                }
    

                if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                    return returnDS;
                }
                if (JCYZ["是否在服务时间内(工作时)"].ToString().Trim() != "是")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                    return returnDS;
                }
                if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
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

                #region//验证投标资料审核表对应的投标点是否审核通过
                string strTBDZLSH = "select c.* from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH = b.Number left join AAA_TBZLSHB c on b.T_YSTBDBH = c.TBDH where a.Number='" + FHD.Trim() + "'";
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

                #region//验证合同状态
                string strSQLDB = "select d.T_YSTBDMJJSBH 原始投标单卖家角色编号,d.Z_HTZT 合同状态,t.F_DQZT 发货单状态,d.Y_YSYDDDLYX 提醒对象登陆邮箱,'提醒对象用户名'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_BUYJSBH=d.Y_YSYDDMJJSBH),'提醒对象结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB DL where DL.J_BUYJSBH=d.Y_YSYDDMJJSBH),d.Y_YSYDDMJJSBH '提醒对象角色编号','卖家用户名'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_SELJSBH=d.T_YSTBDMJJSBH),d.T_YSTBDDLYX 原始投标单登录邮箱 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where t.Number='" + FHD.Trim() + "'";
                DataSet zt = DbHelperSQL.Query(strSQLDB);
                if (zt != null && zt.Tables[0].Rows.Count>0)
                {
                    #region//对发货单状态的验证
                    #region//20131108作废
                    //if (zt.Tables[0].Rows[0]["原始投标单卖家角色编号"].ToString() != JSBH || zt.Tables[0].Rows[0]["合同状态"].ToString() != "定标" || (zt.Tables[0].Rows[0]["发货单状态"].ToString() != "已录入发货信息"&& zt.Tables[0].Rows[0]["发货单状态"].ToString() != "已录入补发备注"))
                    //{
                    //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "抱歉！该发货单无法执行此类指令。";
                    //    return returnDS;
                    //}
                    #endregion
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
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "请输入正确的发货单编号！";
                    return returnDS;
                }
                #endregion

                #region//执行SQL
                string strUpdateTHD = "update AAA_THDYFHDXXB set F_QMJQSQRCZSJ='" + time + "',F_QMJQSWLGSMC='" + WLGSMC + "',F_QMJQSWLDH='" + WLDH + "',F_QMJQSWLQSD='" + WLQSD + "' where Number='" + FHD + "'";
                int i = DbHelperSQL.ExecuteSql(strUpdateTHD);
                if (i > 0)
                {
                    List<Hashtable> list = new List<Hashtable>();
                    Hashtable htJYPT = new Hashtable();//交易平台
                    htJYPT["type"] = "集合集合经销平台";
                    htJYPT["提醒对象登陆邮箱"] = TXDXDLYX;
                    htJYPT["提醒对象用户名"] = TXDXYHM;
                    htJYPT["提醒对象结算账户类型"] = TXDXJSZHLX;
                    htJYPT["提醒对象角色编号"] = TXDXJSBH;
                    htJYPT["提醒对象角色类型"] = TXDXJSLX;
                    htJYPT["提醒内容文本"] = TXNR;
                    htJYPT["创建人"] = CreaterUser;
                    list.Add(htJYPT);
                    Hashtable htYWPT = new Hashtable();//业务操作平台
                    htYWPT["type"] = "业务平台";
                    htYWPT["模提醒模块名"] = "AAA_FWJLXXB";
                    htYWPT["提醒内容文本"] = TXNR;
                    htYWPT["查看地址"] = "abcdf";
                    list.Add(htYWPT);
                    PublicClass2013.Sendmes(list);
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
}