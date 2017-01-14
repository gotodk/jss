using System;
using System.Collections;
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

/// <summary>
///FHyanchi2013 的摘要说明(by 于海滨)
/// </summary>
public class FHyanchi2013
{
    public FHyanchi2013()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 执行发货延迟监控
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public DataSet RunFHYC(DataSet dsreturn)
    {
        //--所有的延迟发货，处理时， 需要补处理的天数(B) = 共延迟天数-已完成预扣天数-1 ，即为 。 从今天开始倒着来，分别插入B天中，每天的预扣和预补偿数据就行了。 减1是因为监控是在凌晨零几分跑的，所以跑监控这天要去掉。
        DataSet dsYC = DbHelperSQL.Query("select '履约保证金剩余金额'=ZZZ.Z_LYBZJJE - isnull((select sum(LS.JE) from   AAA_ZKLSMXB as LS left join AAA_THDYFHDXXB as TTTn on LS.LYDH = TTTn.Number  where  LS.XM='违约赔偿金' and ( LS.XZ = '超过最迟发货日后录入发货信息' or LS.XZ = '发货单生成后5日内未录入发票邮寄信息' ) and LS.SJLX='预' and TTTn.ZBDBXXBBH=TTT.ZBDBXXBBH ),0),'是否有延迟'='否','物流信息录入时间'=TTT.F_WLXXLRSJ,'最迟发货日'=TTT.T_ZCFHR,'共延迟天数'=DATEDIFF(day,TTT.T_ZCFHR,isnull(TTT.F_WLXXLRSJ,GETDATE())), '已完成预扣天数'=(select count(*) from  AAA_ZKLSMXB as LS where  LS.XM='违约赔偿金' and  LS.XZ = '超过最迟发货日后录入发货信息' and LS.SJLX='预' and LS.LYDH=TTT.Number), * from AAA_THDYFHDXXB as TTT left join AAA_ZBDBXXB as ZZZ on TTT.ZBDBXXBBH = ZZZ.Number where DATEDIFF(day,TTT.T_ZCFHR,isnull(TTT.F_WLXXLRSJ,GETDATE())) >0 and  ZZZ.Z_QPZT = '未开始清盘' and  ZZZ.Z_HTZT = '定标' and   DATEDIFF(day, ZZZ.Z_HTJSRQ, GETDATE()) <=0  and ZZZ.Z_HTQX <> '即时' and TTT.F_DQZT not in ('撤销','卖家主动退货') ");


        if (dsYC != null && dsYC.Tables[0].Rows.Count > 0)
        {
            //获得履约保证金剩余金额哈希表，防出现负值
            Hashtable HTlvhzj_SY = new Hashtable();
            for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
            {
                if (!HTlvhzj_SY.ContainsKey(dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()))
                {
                    HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = dsYC.Tables[0].Rows[i]["履约保证金剩余金额"].ToString();
                }
            }
            //要执行的sql语句
            ArrayList alsql = new ArrayList();

            //对照表
            DataSet dsDZB = DbHelperSQL.Query("select * from AAA_moneyDZB where Number in         ('1304000027','1304000051')");

            //本次几个提货单被处理了
            int Daynum = 0;//被有效处理的延迟天数
            for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
            {
                //共延迟天数
                int allycday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["共延迟天数"]);
                //已完成预扣天数
                int yykday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["已完成预扣天数"]);
                //计算扣罚金额***********
                double f_money = 5;
                //货款金额*0.002
                double old_money = Math.Round(Convert.ToDouble(dsYC.Tables[0].Rows[i]["T_DJHKJE"]) * 0.002,2);
                
  
                if (old_money < 5)
                {
                    //不足5元的，按5元算
                    f_money = 5;
                }
                else
                {
                    f_money = old_money;
                }
       
                /*
                #region wyh 2013.6.24 add增加即时扣款规则
                if (dsYC.Tables[0].Rows[i]["Z_HTQX"].ToString().Trim().Equals("即时"))
                {
                    if (old_money < 100)
                    {
                        //不足5元的，按5元算
                        f_money = 100;
                    }
                    else
                    {
                        f_money = old_money;
                    }
                }
                else
                {
                    if (old_money < 5)
                    {
                        //不足5元的，按5元算
                        f_money = 5;
                    }
                    else
                    {
                        f_money = old_money;
                    }
                }
                #endregion
                */
                //倒着跑，从最后一天往前插入
                for (int b = (allycday - 1); b > yykday; b--)
                {
     
                    //插入罚款数据，预*********************************
                    DataRow dr = dsDZB.Tables[0].Select("Number = '1304000027'")[0];

                    string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string DLYX = dsYC.Tables[0].Rows[i]["T_YSTBDDLYX"].ToString();  //登陆邮箱
                    string JSZHLX = dsYC.Tables[0].Rows[i]["T_YSTBDJSZHLX"].ToString();  //交易账户类型
                    string JSBH = dsYC.Tables[0].Rows[i]["T_YSTBDMJJSBH"].ToString(); //角色编号
                    string LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                    string LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                    string LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                    string YSLX = dr["YSLX"].ToString();   //运算类型
                    double JE = f_money;   //金额
                    //对金额进行二次处理，防止被扣成负值，若这条扣的金额大于履约保证金剩余金额，则按照履约保证金剩余金额处理
                    if (JE > Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()))
                    {
                        JE = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString());
                    }
                    HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()) - JE;

                    string XM = dr["XM"].ToString();  //项目
                    string XZ = dr["XZ"].ToString();   //性质
                    string ZY = dr["ZY"].ToString().Replace("[x1]", "F"+dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                    string JKBH = "暂无";
                    string QTBZ = "暂无";
                    string SJLX = dr["SJLX"].ToString();  //数据类型
                    alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");
                    //插入被赔付的数据，预*********************************
                    dr = dsDZB.Tables[0].Select("Number = '1304000051'")[0];
                    Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    DLYX = dsYC.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString();  //登陆邮箱
                    JSZHLX = dsYC.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString();  //交易账户类型
                    JSBH = dsYC.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString(); //角色编号
                    LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                    LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                    LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                    YSLX = dr["YSLX"].ToString();   //运算类型
                    //JE = f_money;   //金额,这个不用算了，用上面罚款的金额
                    XM = dr["XM"].ToString();  //项目
                    XZ = dr["XZ"].ToString();   //性质
                    ZY = dr["ZY"].ToString().Replace("[x1]", "F" + dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                    JKBH = "暂无";
                    QTBZ = "暂无";
                    SJLX = dr["SJLX"].ToString();  //数据类型
                    alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");

                    //计算延迟天数和有效条数
                    Daynum++;
                    dsYC.Tables[0].Rows[i]["是否有延迟"] = "是";
                    
                }
            }

            bool rr = DbHelperSQL.ExecSqlTran(alsql);
            //bool rr = true;
            if (rr)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理了" + dsYC.Tables[0].Select("是否有延迟='是'").Length.ToString() + "条延迟发货提货单，共计" + Daynum.ToString() + "天延迟!";
                return dsreturn;

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "延迟发货数据生成的SQL语句执行未成功!";
                return dsreturn;
            }

            
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有要处理的延迟发货数据!";
            return dsreturn;
        }


    }

    /// <summary>
    /// 执行发票延迟监控
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public DataSet RunFPYC(DataSet dsreturn)
    {
        //--所有的延迟发票寄送，处理时， 需要补处理的天数(B) = 共延迟天数-已完成预扣天数-1 ，即为 。 从今天开始倒着来，分别插入B天中，每天的预扣和预补偿数据就行了。 减1是因为监控是在凌晨零几分跑的，所以跑监控这天要去掉。
        DataSet dsYC = DbHelperSQL.Query("select '履约保证金剩余金额'=ZZZ.Z_LYBZJJE - isnull((select sum(LS.JE) from  AAA_ZKLSMXB as LS left join AAA_THDYFHDXXB as TTTn on LS.LYDH = TTTn.Number  where  LS.XM='违约赔偿金' and  ( LS.XZ = '超过最迟发货日后录入发货信息' or LS.XZ = '发货单生成后5日内未录入发票邮寄信息' ) and LS.SJLX='预' and TTTn.ZBDBXXBBH=TTT.ZBDBXXBBH ),0),'是否有延迟'='否', '发货单生成时间'=TTT.F_FHDSCSJ,'发票邮寄信息录入时间'=TTT.F_FPYJXXLRSJ,'发票最迟寄送日期'=DATEADD(day, 5, TTT.F_FHDSCSJ) ,'共延迟天数'=DATEDIFF(day,DATEADD(day, 5, TTT.F_FHDSCSJ),isnull(TTT.F_FPYJXXLRSJ,GETDATE())), '已完成预扣天数'=(select count(*) from  AAA_ZKLSMXB as LS where  LS.XM='违约赔偿金' and  LS.XZ = '发货单生成后5日内未录入发票邮寄信息' and LS.SJLX='预' and LS.LYDH=TTT.Number), * from AAA_THDYFHDXXB as TTT left join AAA_ZBDBXXB as ZZZ on TTT.ZBDBXXBBH = ZZZ.Number where DATEDIFF(day,DATEADD(day, 5, TTT.F_FHDSCSJ),isnull(TTT.F_FPYJXXLRSJ,GETDATE())) >0 and  ZZZ.Z_QPZT = '未开始清盘' and ZZZ.Z_HTQX <> '即时' and  ZZZ.Z_HTZT = '定标' and   DATEDIFF(day, ZZZ.Z_HTJSRQ, GETDATE()) <=0  and TTT.F_FHDSCSJ is not null and TTT.F_DQZT not in ('撤销','卖家主动退货')");
        if (dsYC != null && dsYC.Tables[0].Rows.Count > 0)
        {
            //获得履约保证金剩余金额哈希表，防出现负值
            Hashtable HTlvhzj_SY = new Hashtable();
            for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
            {
                if (!HTlvhzj_SY.ContainsKey(dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()))
                {
                    HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = dsYC.Tables[0].Rows[i]["履约保证金剩余金额"].ToString();
                }
            }


            ArrayList alsql = new ArrayList();

            //对照表
            DataSet dsDZB = DbHelperSQL.Query("select * from AAA_moneyDZB where Number in         ('1304000029','1304000050')");

            int Daynum = 0;//被有效处理的延迟天数
            for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
            {
                //共延迟天数
                int allycday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["共延迟天数"]);
                //已完成预扣天数
                int yykday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["已完成预扣天数"]);
                //计算扣罚金额，一天5元***********
                double f_money = 5;
                //倒着跑，从最后一天往前插入
                for (int b = (allycday - 1); b > yykday; b--)
                {
                    //插入罚款数据，预*********************************
                    DataRow dr = dsDZB.Tables[0].Select("Number = '1304000029'")[0];
                    string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string DLYX = dsYC.Tables[0].Rows[i]["T_YSTBDDLYX"].ToString();  //登陆邮箱
                    string JSZHLX = dsYC.Tables[0].Rows[i]["T_YSTBDJSZHLX"].ToString();  //交易账户类型
                    string JSBH = dsYC.Tables[0].Rows[i]["T_YSTBDMJJSBH"].ToString(); //角色编号
                    string LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                    string LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                    string LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                    string YSLX = dr["YSLX"].ToString();   //运算类型
                    double JE = f_money;   //金额
                    //对金额进行二次处理，防止被扣成负值，若这条扣的金额大于履约保证金剩余金额，则按照履约保证金剩余金额处理
                    if (JE > Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()))
                    {
                        JE = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString());
                    }
                    HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()) - JE;

                    string XM = dr["XM"].ToString();  //项目
                    string XZ = dr["XZ"].ToString();   //性质
                    string ZY = dr["ZY"].ToString().Replace("[x1]", "F" + dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                    string JKBH = "暂无";
                    string QTBZ = "暂无";
                    string SJLX = dr["SJLX"].ToString();  //数据类型
                    alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");
                    //插入被赔付的数据，预*********************************
                    dr = dsDZB.Tables[0].Select("Number = '1304000050'")[0];
                    Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    DLYX = dsYC.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString();  //登陆邮箱
                    JSZHLX = dsYC.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString();  //交易账户类型
                    JSBH = dsYC.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString(); //角色编号
                    LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                    LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                    LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                    YSLX = dr["YSLX"].ToString();   //运算类型
                    //JE = f_money;   //金额,这个不用算了，用上面罚款的金额
                    XM = dr["XM"].ToString();  //项目
                    XZ = dr["XZ"].ToString();   //性质
                    ZY = dr["ZY"].ToString().Replace("[x1]", "F" + dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                    JKBH = "暂无";
                    QTBZ = "暂无";
                    SJLX = dr["SJLX"].ToString();  //数据类型
                    alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");

                    //计算延迟天数和有效条数
                    Daynum++;
                    dsYC.Tables[0].Rows[i]["是否有延迟"] = "是";

                }
            }
            //bool rr = true;
            bool rr = DbHelperSQL.ExecSqlTran(alsql);
            if (rr)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理了" + dsYC.Tables[0].Select("是否有延迟='是'").Length.ToString() + "条延迟发票寄送提货单，共计" + Daynum.ToString() + "天延迟!";
                return dsreturn;
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "延迟发票寄送数据生成的SQL执行失败!";
                return dsreturn;
            }


        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有要处理的延迟发票寄送数据!";
            return dsreturn;
        }

   
    }




    /// <summary>
    /// 执行发货延迟监控(仅用于即时的)
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public DataSet RunFHYC_onlyJSJY(DataSet dsreturn)
    {
        //--所有的延迟发货，处理时， 需要补处理的天数(B) = 共延迟天数-已完成预扣天数-1 ，即为 。 从今天开始倒着来，分别插入B天中，每天的预扣和预补偿数据就行了。 减1是因为监控是在凌晨零几分跑的，所以跑监控这天要去掉。
        DataSet dsYC = DbHelperSQL.Query("select '履约保证金剩余金额'=ZZZ.Z_LYBZJJE - isnull((select sum(LS.JE) from   AAA_ZKLSMXB as LS left join AAA_THDYFHDXXB as TTTn on LS.LYDH = TTTn.Number where  LS.XM='违约赔偿金' and ( LS.XZ = '超过最迟发货日后录入发货信息' or LS.XZ = '发货单生成后5日内未录入发票邮寄信息' ) and LS.SJLX='预' and TTTn.ZBDBXXBBH=TTT.ZBDBXXBBH ),0),'是否有延迟'='否','物流信息录入时间'=TTT.F_WLXXLRSJ,'最迟发货日'=TTT.T_ZCFHR,'共延迟天数'=DATEDIFF(day,TTT.T_ZCFHR,CASE WHEN isnull(TTT.F_WLXXLRSJ,GETDATE()) >= isnull(TTT.F_FPYJXXLRSJ,GETDATE()) THEN isnull(TTT.F_WLXXLRSJ,GETDATE())   ELSE isnull(TTT.F_FPYJXXLRSJ,GETDATE())   END ), '已完成预扣天数'=(select count(*) from  AAA_ZKLSMXB as LS where  LS.XM='违约赔偿金' and  LS.XZ = '超过最迟发货日后录入发货信息' and LS.SJLX='预' and LS.LYDH=TTT.Number), * from AAA_THDYFHDXXB as TTT left join AAA_ZBDBXXB as ZZZ on TTT.ZBDBXXBBH = ZZZ.Number where DATEDIFF(day,TTT.T_ZCFHR,CASE WHEN isnull(TTT.F_WLXXLRSJ,GETDATE()) >= isnull(TTT.F_FPYJXXLRSJ,GETDATE()) THEN isnull(TTT.F_WLXXLRSJ,GETDATE())   ELSE isnull(TTT.F_FPYJXXLRSJ,GETDATE())   END ) >0 and  ZZZ.Z_QPZT = '未开始清盘' and  ZZZ.Z_HTZT = '定标' and   DATEDIFF(day, ZZZ.Z_HTJSRQ, GETDATE()) <=0  and ZZZ.Z_HTQX = '即时' and TTT.F_DQZT not in ('撤销','卖家主动退货') ");


        if (dsYC != null && dsYC.Tables[0].Rows.Count > 0)
        {
            //获得履约保证金剩余金额哈希表，防出现负值
            Hashtable HTlvhzj_SY = new Hashtable();
            for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
            {
                if (!HTlvhzj_SY.ContainsKey(dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()))
                {
                    HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = dsYC.Tables[0].Rows[i]["履约保证金剩余金额"].ToString();
                }
            }
            //要执行的sql语句
            ArrayList alsql = new ArrayList();

            //对照表
            DataSet dsDZB = DbHelperSQL.Query("select * from AAA_moneyDZB where Number in         ('1304000027','1304000051')");

            //本次几个提货单被处理了
            int Daynum = 0;//被有效处理的延迟天数
            for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
            {
                //共延迟天数
                int allycday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["共延迟天数"]);
                //已完成预扣天数
                int yykday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["已完成预扣天数"]);
                //计算扣罚金额***********
                double f_money = 0;
                //货款金额 *0.0005
                double old_money = Math.Round(Convert.ToDouble(dsYC.Tables[0].Rows[i]["T_DJHKJE"]) * 0.0005, 2);


                if (old_money < 100)
                {
                    //不足5元的，按5元算
                    f_money = 100;
                }
                else
                {
                    f_money = old_money;
                }

                //倒着跑，从最后一天往前插入
                for (int b = (allycday - 1); b > yykday; b--)
                {

                    //插入罚款数据，预*********************************
                    DataRow dr = dsDZB.Tables[0].Select("Number = '1304000027'")[0];

                    string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string DLYX = dsYC.Tables[0].Rows[i]["T_YSTBDDLYX"].ToString();  //登陆邮箱
                    string JSZHLX = dsYC.Tables[0].Rows[i]["T_YSTBDJSZHLX"].ToString();  //交易账户类型
                    string JSBH = dsYC.Tables[0].Rows[i]["T_YSTBDMJJSBH"].ToString(); //角色编号
                    string LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                    string LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                    string LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                    string YSLX = dr["YSLX"].ToString();   //运算类型
                    double JE = f_money;   //金额
                    //对金额进行二次处理，防止被扣成负值，若这条扣的金额大于履约保证金剩余金额，则按照履约保证金剩余金额处理
                    if (JE > Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()))
                    {
                        JE = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString());
                    }
                    HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()) - JE;

                    string XM = dr["XM"].ToString();  //项目
                    string XZ = dr["XZ"].ToString();   //性质
                    string ZY = dr["ZY"].ToString().Replace("[x1]", "F" + dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                    string JKBH = "暂无";
                    string QTBZ = "暂无";
                    string SJLX = dr["SJLX"].ToString();  //数据类型
                    alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");
                    //插入被赔付的数据，预*********************************
                    dr = dsDZB.Tables[0].Select("Number = '1304000051'")[0];
                    Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    DLYX = dsYC.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString();  //登陆邮箱
                    JSZHLX = dsYC.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString();  //交易账户类型
                    JSBH = dsYC.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString(); //角色编号
                    LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                    LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                    LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                    YSLX = dr["YSLX"].ToString();   //运算类型
                    //JE = f_money;   //金额,这个不用算了，用上面罚款的金额
                    XM = dr["XM"].ToString();  //项目
                    XZ = dr["XZ"].ToString();   //性质
                    ZY = dr["ZY"].ToString().Replace("[x1]", "F" + dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                    JKBH = "暂无";
                    QTBZ = "暂无";
                    SJLX = dr["SJLX"].ToString();  //数据类型
                    alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");

                    //计算延迟天数和有效条数
                    Daynum++;
                    dsYC.Tables[0].Rows[i]["是否有延迟"] = "是";

                }
            }

            bool rr = DbHelperSQL.ExecSqlTran(alsql);
            //bool rr = true;
            if (rr)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理了" + dsYC.Tables[0].Select("是否有延迟='是'").Length.ToString() + "条延迟发货提货单，共计" + Daynum.ToString() + "天延迟!";
                return dsreturn;

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "延迟发货数据生成的SQL语句执行未成功!";
                return dsreturn;
            }


        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有要处理的延迟发货数据!";
            return dsreturn;
        }


    }


    /// <summary>
    /// 计算履约保证金不足的合同
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public DataSet RunLYBZJbuzu(DataSet dsreturn)
    {
        //动态参数表
        Hashtable htParameterInfo = PublicClass2013.GetParameterInfo();
        //找出履约保证金不足需要废标的标的
        double lybzjbzbl = Convert.ToDouble(htParameterInfo["履约保证金不足比率"]);
        if(lybzjbzbl == 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "履约保证金不足废标参数获取错误!";
            return dsreturn;
        }
        DataSet dsFB = DbHelperSQL.Query("select * from ( select '履约保证金金额'=Z_LYBZJJE,     '两项延迟总金额'=(select isnull(sum(LS.JE),0.00) from AAA_ZKLSMXB as LS  left join AAA_THDYFHDXXB as TTT on LS.LYDH = TTT.Number  where LS.XM='违约赔偿金' and  LS.XZ in ( '超过最迟发货日后录入发货信息','发货单生成后5日内未录入发票邮寄信息') and LS.SJLX='预' and TTT.ZBDBXXBBH = ZZZ.Number),     '是否已解冻过订金'=(select count(*)  from AAA_ZKLSMXB as ZK where ZK.LYDH=ZZZ.Number and ZK.LYYWLX='AAA_ZBDBXXB'   and ZK.XM='订金' and ZK.XZ='合同期内下达完全部提货单解冻' and ZK.SJLX='实') ,          ZZZ.Y_YSYDDDLYX,ZZZ.Y_YSYDDJSZHLX,ZZZ.Y_YSYDDMJJSBH,ZZZ.Number,ZZZ.Z_DJJE,ZZZ.Z_HTBH  from AAA_ZBDBXXB as ZZZ where ZZZ.Z_HTZT = '定标'  ) as tab11       where convert(float,履约保证金金额-两项延迟总金额)/convert(float,履约保证金金额) < convert(float," + lybzjbzbl.ToString() + ")  ");
        //string aaa = Convert.ToDouble(htParameterInfo["履约保证金不足比率"]).ToString();
        if (dsFB != null && dsFB.Tables[0].Rows.Count > 0)
        {
            //防重复处理，记录应变更的余额
            Hashtable HTyue = new Hashtable();
            //要执行的sql语句
            ArrayList alsql = new ArrayList();
            //对照表
            DataSet dsDZB = DbHelperSQL.Query("select * from AAA_moneyDZB where Number in ('1304000021')");
            for (int i = 0; i < dsFB.Tables[0].Rows.Count; i++)
            {

                    //解冻订金
                    DataRow dr = dsDZB.Tables[0].Select("Number = '1304000021'")[0];
                    string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string DLYX = dsFB.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString();  //登陆邮箱
                    string JSZHLX = dsFB.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString();  //交易账户类型
                    string JSBH = dsFB.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString(); //角色编号
                    string LYYWLX = "AAA_ZBDBXXB";   //中标定标表名
                    string LYDH = dsFB.Tables[0].Rows[i]["Number"].ToString();  //中标定标编号
                    string LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                    string YSLX = dr["YSLX"].ToString();   //运算类型
                    double JE = Convert.ToDouble(dsFB.Tables[0].Rows[i]["Z_DJJE"]);   //订金金额
                    string XM = dr["XM"].ToString();  //项目
                    string XZ = dr["XZ"].ToString();   //性质
                    string ZY = dr["ZY"].ToString().Replace("[x1]", dsFB.Tables[0].Rows[i]["Z_HTBH"].ToString());   //摘要，这个要替换
                    string JKBH = "暂无";
                    string QTBZ = "暂无";
                    string SJLX = dr["SJLX"].ToString();  //数据类型

                    if (dsFB.Tables[0].Rows[i]["是否已解冻过订金"].ToString().Trim() == "0")
                    {
                        alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");

                        //更新余额
                        if (!HTyue.ContainsKey(DLYX))
                        {
                            HTyue[DLYX] = JE;
                        }
                        else
                        {
                            HTyue[DLYX] = Convert.ToDouble(HTyue[DLYX]) + JE;
                        }
                    }

                //更新中标定标信息表状态
                alsql.Add("update AAA_ZBDBXXB set Z_HTZT = '定标合同终止',Z_QPZT='清盘中',Z_QPKSSJ='" + LSCSSJ + "' where  Number = '" + LYDH + "' ");
                
                
            }
            //执行sql语句和处理可用余额
            //bool rr = true;
            bool rr = DbHelperSQL.ExecSqlTran(alsql);
            if (rr)
            {
                ClassMoney2013 CM2013 = new ClassMoney2013();
                //遍历处理余额
                foreach (DictionaryEntry de in HTyue) 
                {
                    CM2013.FreezeMoneyT(de.Key.ToString(), de.Value.ToString(), "+");
                }
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "履约保证金不足废标" + dsFB.Tables[0].Rows.Count.ToString() + "条被处理成功!";
                return dsreturn;
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "履约保证金不足废标SQL语句执行失败!";
                return dsreturn;
            }

        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有要处理的履约保证金不足废标数据!";
            return dsreturn;
        }
    
    }

}