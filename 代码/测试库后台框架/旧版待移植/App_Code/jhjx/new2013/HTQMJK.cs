using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using FMOP.DB;

/// <summary>
/// HTQMJK 的摘要说明
/// </summary>
public class HTQMJK
{
	public HTQMJK()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public Hashtable ServerHTQMJK()
    {
        Hashtable ht_result = new Hashtable();
        ht_result["执行结果"] = "初始化";
        ht_result["提示文本"] = "未执行！";
        ArrayList alLog = new ArrayList();//用户保存需要写入日志的内容
        string jkbh="合同到期监控";
        //获取需要插入的账款流水明细资金类型基本上数据       
        DataSet ds_ZKMX = DbHelperSQL.Query("select * from AAA_moneyDZB where XM in ('订金','技术服务费','违约赔偿金','补偿收益','履约保证金')");

        DataRow[] dr_saleycfh_y = ds_ZKMX.Tables[0].Select("number='1304000027'");//卖家发货延迟扣罚--预
        DataRow[] dr_saleycfp_y = ds_ZKMX.Tables[0].Select("number='1304000029'");//卖家发票延迟扣罚--预
        DataRow[] dr_buyycfh_y = ds_ZKMX.Tables[0].Select("number='1304000051'");//买家发货延迟补偿--预
        DataRow[] dr_buyycfp_y = ds_ZKMX.Tables[0].Select("number='1304000050'");//买家发票延迟补偿--预
      
        //获取信用评分对照表的基本数据
         DataTable dt_xypf = DbHelperSQL.Query("select * from AAA_JYFXYBDMXDZB" ).Tables[0];
        //用于存储更新评分的语句
        List<string> jfl = new List<string>();

        //获取合同到期需要处理的所有基础数据信息
        //string sql_info = "select top 5 *,cast((Z_ZBSL-Z_YTHSL)/Z_ZBSL*Z_BZHJE as numeric(18,2)) as 买家违约赔偿,cast((Z_ZBSL-Z_YTHSL)/Z_ZBSL*(Z_DJJE-Z_BZHJE) as numeric(18,2)) as 买家技术服务费,";
        string sql_info = "select *,cast((Z_ZBSL-Z_YTHSL)/Z_ZBSL*Z_BZHJE as numeric(18,2)) as 买家违约赔偿,cast(Z_ZBJE*0.02 as numeric(18,2)) as 买家技术服务费,";
        sql_info += "(select  isnull(SUM(je),0.00) from AAA_ZKLSMXB as c where c.DLYX=tab.T_YSTBDDLYX and c.LYYWLX='AAA_ZBDBXXB' and c.LYDH=tab.Number and c.XM ='" + dr_saleycfh_y[0]["xm"].ToString() + "' and c.XZ='" + dr_saleycfh_y[0]["xz"].ToString() + "' and c.SJLX ='" + dr_saleycfh_y[0]["sjlx"].ToString() + "' ) as 卖家发货扣罚,";
        sql_info += "(select isnull(SUM(je),0.00) from AAA_ZKLSMXB as d where d.DLYX=tab.T_YSTBDDLYX and d.LYYWLX='AAA_ZBDBXXB' and d.LYDH=tab.Number and d.XM ='" + dr_saleycfp_y[0]["xm"].ToString() + "' and d.XZ='" + dr_saleycfp_y[0]["xz"].ToString() + "' and d.SJLX ='" + dr_saleycfp_y[0]["sjlx"].ToString() + "' ) as 卖家发票扣罚,";
        sql_info += "(select isnull(SUM(je),0.00) from AAA_ZKLSMXB as e where e.DLYX=tab.Y_YSYDDDLYX and e.LYYWLX='AAA_ZBDBXXB' and e.LYDH=tab.Number and e.XM ='" + dr_buyycfh_y[0]["xm"].ToString() + "' and e.XZ='" + dr_buyycfh_y[0]["xz"].ToString() + "' and e.SJLX ='" + dr_buyycfh_y[0]["sjlx"].ToString() + "' ) as 买家发货补偿收益,";
        sql_info += "(select isnull(SUM(je),0.00) from AAA_ZKLSMXB as f where f.DLYX=tab.Y_YSYDDDLYX and f.LYYWLX='AAA_ZBDBXXB' and f.LYDH=tab.Number and f.XM ='" + dr_buyycfp_y[0]["xm"].ToString() + "' and f.XZ='" + dr_buyycfp_y[0]["xz"].ToString() + "' and f.SJLX ='" + dr_buyycfp_y[0]["sjlx"].ToString() + "' ) as 买家发票补偿收益 ";       
        sql_info+=" from ( select a.*,(select COUNT(*) from AAA_THDYFHDXXB as b where b.ZBDBXXBBH=a.number and b.F_DQZT not in ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','卖家主动退货','撤销'))  as 有异议收货 from AAA_ZBDBXXB as a where Z_HTZT='定标' and Z_QPZT='未开始清盘' and  CONVERT (varchar(10), Z_HTJSRQ,120)<'" + DateTime.Now.ToString("yyyy-MM-dd") + "') as tab where (Z_ZBSL=Z_YTHSL and 有异议收货>0) or (Z_ZBSL>Z_YTHSL)";
        DataTable dt_info = new DataTable();
        try
        {
            dt_info = DbHelperSQL.Query(sql_info).Tables[0];
        }
        catch (Exception ex)
        {
            ht_result["执行结果"] = "err";
            ht_result["提示文本"] = "基础数据获取过程出现错误 ！" + ex.ToString();
            alLog.Add(ht_result["提示文本"].ToString());
            WriteLog(alLog);
            return ht_result;
        }

        if (dt_info == null || dt_info.Rows.Count == 0)
        {
            ht_result["执行结果"] = "ok";
            ht_result["提示文本"] = "无到期合同需要处理！";
            alLog.Add("无到期合同需要处理！");
            WriteLog(alLog);
            return ht_result;
        }
        else
        {
            ArrayList al = new ArrayList();//用于保存需要执行的sql语句       
            List<Hashtable> listTX = new List<Hashtable>(); //用户执行提醒插入功能的参数列表
            DataTable dt_Money = DbHelperSQL.Query("select '' as 登录邮箱,0.00 as 金额, '' as 运算,'' as 数据类型 from AAA_ZBDBXXB where 1!=1").Tables[0];//用于保存钱的变动明细

            DataRow[] dr_buyjd = ds_ZKMX.Tables[0].Select("number='1304000022'");//订金 合同期满解冻 
            DataRow[] dr_buywypc = ds_ZKMX.Tables[0].Select("number='1304000024'");//违约赔偿金 买家违约赔偿给卖家 
            DataRow[] dr_salebc = ds_ZKMX.Tables[0].Select("number='1304000034'");//补偿收益 收到买家违约补偿 
            DataRow[] dr_buyjsfwf = ds_ZKMX.Tables[0].Select("number='1304000008'");//技术服务费 买家违约赔偿 
            DataRow[] dr_salelybzj = ds_ZKMX.Tables[0].Select("number='1304000048'");// 履约保证金 履约保证金解冻 
            DataRow[] dr_saleycfh_s = ds_ZKMX.Tables[0].Select("number='1304000026'");//违约赔偿金 超过最迟发货日后录入发货信息 
            DataRow[] dr_buyycfh_s = ds_ZKMX.Tables[0].Select("number='1304000036'");//补偿收益 收到卖家发货延迟补偿 
            DataRow[] dr_saleycfp_s = ds_ZKMX.Tables[0].Select("number='1304000028'");//违约赔偿金 发货单生成后5日内未录入发票邮寄信息 
            DataRow[] dr_buyycfp_s = ds_ZKMX.Tables[0].Select("number='1304000037'");//补偿收益 收到卖家发票延迟补偿 

            
            #region 循环

            for (int i = 0; i < dt_info.Rows.Count; i++)
            {
                //如果已提货数量=中标数量，没有完成无异议收货的
                if (dt_info.Rows[i]["Z_YTHSL"].ToString() == dt_info.Rows[i]["Z_ZBSL"].ToString() && dt_info.Rows[i]["有异议收货"].ToString() != "0")
                {
                    string sql_update = "update AAA_ZBDBXXB set Z_HTZT='定标合同到期',Z_QPZT='清盘中',Z_QPKSSJ='" + DateTime.Now.ToString() + "' where Number='" + dt_info.Rows[i]["number"].ToString() + "'";
                    al.Add(sql_update);
                }

                //已提货数量<中标数量  
                if (Convert.ToInt64(dt_info.Rows[i]["Z_YTHSL"]) < Convert.ToInt64(dt_info.Rows[i]["Z_ZBSL"]))
                {
                    //插入《账款流水明细表》中买家订金解冻              
                    string zy_buyjd = dr_buyjd[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》期满                 
                    string KeyNumber_buyjd = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string sql_BuyJD = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buyjd + "','" + dt_info.Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buyjd[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["Z_DJJE"].ToString() + ",'" + dr_buyjd[0]["XM"].ToString() + "','" + dr_buyjd[0]["XZ"].ToString() + "','" + zy_buyjd + "','" + jkbh + "','','" + dr_buyjd[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                    al.Add(sql_BuyJD);

                    //插入《账款流水明细表》中买家未完成提货的违约赔偿金                
                    string zy_buywypc = dr_buywypc[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》期满买家没有下完提货单                

                    string KeyNumber_buywypc = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string sql_BuyWYPC = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buywypc + "','" + dt_info.Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buywypc[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["买家违约赔偿"].ToString() + ",'" + dr_buywypc[0]["XM"].ToString() + "','" + dr_buywypc[0]["XZ"].ToString() + "','" + zy_buywypc + "','" + jkbh + "','','" + dr_buywypc[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                    al.Add(sql_BuyWYPC);

                    //插入《账款流水明细表》卖家的补偿收益               
                    string zy_salebc = dr_salebc[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要内容：[x1]《电子购货合同》期满买家未完成提货

                    string KeyNumber_salebc = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string sql_Salebc = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_salebc + "','" + dt_info.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_salebc[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["买家违约赔偿"].ToString() + ",'" + dr_salebc[0]["XM"].ToString() + "','" + dr_salebc[0]["XZ"].ToString() + "','" + zy_salebc + "','" + jkbh + "','','" + dr_salebc[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                    al.Add(sql_Salebc);


                    //插入《账款流水明细表》扣减买家的技术服务费               
                    string zy_buyjsfwf = dr_buyjsfwf[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要内容：[x1]《电子购货合同》期满买家未完成提货

                    string KeyNumber_buyjsfwf = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string sql_Buyjsfwf = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buyjsfwf + "','" + dt_info.Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buyjsfwf[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["买家技术服务费"].ToString() + ",'" + dr_buyjsfwf[0]["XM"].ToString() + "','" + dr_buyjsfwf[0]["XZ"].ToString() + "','" + zy_buyjsfwf + "','" + jkbh + "','','" + dr_buyjsfwf[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                    al.Add(sql_Buyjsfwf);

                    //将钱的变动增加到dt_Money中
                    dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["Z_DJJE"]), '加', dr_buyjd[0]["SJLX"].ToString(), });//买家订金解冻
                    dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["买家违约赔偿"]), '减', dr_buywypc[0]["SJLX"].ToString(), });//买家未完成提货的违约赔偿
                    dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["T_YSTBDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["买家违约赔偿"]), '加', dr_salebc[0]["SJLX"].ToString(), });//卖家补偿收益
                    dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["买家技术服务费"]), '减', dr_buyjsfwf[0]["SJLX"].ToString(), });//买家未完成提货的技术服务费

                    //没有完成无异议收货
                    if (Convert.ToInt64(dt_info.Rows[i]["有异议收货"]) > 0)
                    {
                        string sql_update = "update AAA_ZBDBXXB set Z_HTZT='定标合同到期',Z_QPZT='清盘中',Z_QPKSSJ='" + DateTime.Now.ToString() + "' where number='" + dt_info.Rows[i]["number"].ToString() + "'";
                        al.Add(sql_update);
                    }
                    else//完成无异议收货的
                    {
                        //写入《账款流水明细表》中卖家解冻履约保证金
                        string zy_salelybzj = dr_salelybzj[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》终止，所有问题已全部处理完毕，即清盘结束时。包括但不限于“请重新发货”、“部分收货”、“有异议收货”等情况已有最终处理结果。
                        string KeyNumber_salelybzj = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string sql_salelybzj = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_salelybzj + "','" + dt_info.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_salelybzj[0]["YSLX"].ToString() + "'," + Convert.ToDouble(dt_info.Rows[i]["Z_LYBZJJE"]) + ",'" + dr_salelybzj[0]["XM"].ToString() + "','" + dr_salelybzj[0]["XZ"].ToString() + "','" + zy_salelybzj + "','" + jkbh + "','','" + dr_salelybzj[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                        al.Add(sql_salelybzj);

                        //将钱的变动增加到dt_Money中
                        dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["T_YSTBDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["Z_LYBZJJE"]), '加', dr_salelybzj[0]["SJLX"].ToString() });//卖家履约保证金解冻

                        //扣罚卖家的延迟录入发货信息违约赔偿金,给买家发货延迟补偿收益
                        if (Convert.ToDouble(dt_info.Rows[i]["卖家发货扣罚"]) > 0 && Convert.ToDouble(dt_info.Rows[i]["卖家发货扣罚"]).ToString("#0.00") != "0.00")
                        {
                            //写入《账款流水明细表》中卖家延迟发货违约赔偿金                        
                            string zy_saleycfh_s = dr_saleycfh_s[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》结束

                            string KeyNumber_saleycfh = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                            string sql_saleycfh = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_saleycfh + "','" + dt_info.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_saleycfh_s[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["卖家发货扣罚"].ToString() + ",'" + dr_saleycfh_s[0]["XM"].ToString() + "','" + dr_saleycfh_s[0]["XZ"].ToString() + "','" + zy_saleycfh_s + "','" + jkbh + "','','" + dr_saleycfh_s[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                            al.Add(sql_saleycfh);

                            //插入《账款流水明细表》中买家延迟发货补偿收益
                            string zy_buyycfh_s = dr_buyycfh_s[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());
                            string KeyNumber_buyycfh = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                            string sql_buyycfh = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buyycfh + "','" + dt_info.Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buyycfh_s[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["买家发货补偿收益"].ToString() + ",'" + dr_buyycfh_s[0]["XM"].ToString() + "','" + dr_buyycfh_s[0]["XZ"].ToString() + "','" + zy_buyycfh_s + "','" + jkbh + "','','" + dr_buyycfh_s[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                            al.Add(sql_buyycfh);

                            //将钱的变动增加到dt_Money中
                            dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["T_YSTBDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["卖家发货扣罚"]), '减', dr_saleycfh_s[0]["SJLX"].ToString() });//卖家延迟发货违约赔偿
                            dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["买家发货补偿收益"]), '加', dr_buyycfh_s[0]["SJLX"].ToString() });//买家延迟发货补偿收益
                        }

                        //卖家延迟发票扣罚，买家延迟发票补偿
                        if (Convert.ToDouble(dt_info.Rows[i]["卖家发票扣罚"]) > 0 && Convert.ToDouble(dt_info.Rows[i]["卖家发货扣罚"]).ToString("#0.00") != "0.00")
                        {
                            //写入《账款流水明细表》中卖家延迟发票违约赔偿金                       
                            string zy_saleycfp_s = dr_saleycfp_s[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》结束
                            string KeyNumber_saleycfp = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                            string sql_saleycfp = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_saleycfp + "','" + dt_info.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_saleycfp_s[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["卖家发票扣罚"].ToString() + ",'" + dr_saleycfp_s[0]["XM"].ToString() + "','" + dr_saleycfp_s[0]["XZ"].ToString() + "','" + zy_saleycfp_s + "','" + jkbh + "','','" + dr_saleycfp_s[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                            al.Add(sql_saleycfp);

                            //插入《账款流水明细表》中买家延迟发票补偿收益                        
                            string zy_buyycfp_s = dr_buyycfp_s[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》结束
                            string KeyNumber_buyycfp = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                            string sql_buyycfp = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buyycfp + "','" + dt_info.Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buyycfp_s[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["买家发票补偿收益"].ToString() + ",'" + dr_buyycfp_s[0]["XM"].ToString() + "','" + dr_buyycfp_s[0]["XZ"].ToString() + "','" + zy_buyycfp_s + "','" + jkbh + "','','" + dr_buyycfp_s[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                            al.Add(sql_buyycfp);

                            //将钱的变动增加到dt_Money中
                            dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["T_YSTBDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["卖家发票扣罚"]), '减', dr_saleycfp_s[0]["SJLX"].ToString() });//卖家延迟发票违约赔偿
                            dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["买家发票补偿收益"]), '加', dr_buyycfp_s[0]["SJLX"].ToString() });//买家延迟发票补偿
                        }

                        //更新中标定标信息表清盘状态和清盘开始结束时间
                        string datetime = DateTime.Now.ToString();
                        string sql_update = "update AAA_ZBDBXXB set Z_HTZT='定标合同到期',Z_QPZT='清盘结束',Z_QPKSSJ='" + datetime + "',Z_QPJSSJ='" + datetime + "' where number='" + dt_info.Rows[i]["number"].ToString() + "'";
                        al.Add(sql_update);

                        #region 用户评级更新，EditTime 2013-12
                        try
                        {
                            jhjx_JYFXYMX xymc = new jhjx_JYFXYMX();

                            //买方邮箱
                            string buyeremail = dt_info.Rows[i]["Y_YSYDDDLYX"].ToString().Trim();
                            //卖方邮箱
                            string selleremail = dt_info.Rows[i]["T_YSTBDDLYX"].ToString().Trim();
                            //买方关联经纪人邮箱
                            string buyjjr = dt_info.Rows[i]["Y_YSYDDGLJJRYX"].ToString().Trim();
                            //卖方关联经纪人邮箱
                            string selljjr = dt_info.Rows[i]["T_YSTBDGLJJRYX"].ToString().Trim();
                            //合同编号
                            string htnum = dt_info.Rows[i]["Z_HTBH"].ToString().Trim();
                            //买方角色编号
                            string buyjsbh = dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString().Trim();
                            //卖方角色编号
                            string selljsbh = dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString().Trim();
                            //买方关联经纪人角色编号
                            string bjjrjsbh = dt_info.Rows[i]["Y_YSYDDGLJJRJSBH"].ToString().Trim();
                            //卖方关联经纪人角色编号
                            string sjjrjsbh = dt_info.Rows[i]["T_YSTBDGLJJRJSBH"].ToString().Trim();

                            //买方判断
                            bool B_Can = xymc.IfCan(htnum, buyjsbh);
                            //卖方判断
                            bool S_Can = xymc.IfCan(htnum, selljsbh);
                            //买方评分
                            string[] buystrs = new string[2];

                            if (B_Can == true)//完全履约
                            {

                                DataRow[] drs = dt_xypf.Select("Number='1309000006'");
                                buystrs = xymc.SqlText(drs[0], buyeremail, htnum, "", buyjsbh, "买家卖家交易账户", "admin", "");

                            }
                            else//未完全履约
                            {
                                DataRow[] drs = dt_xypf.Select("Number='1309000007'");
                                buystrs = xymc.SqlText(drs[0], buyeremail, htnum, "", buyjsbh, "买家卖家交易账户", "admin", "");
                            }

                            //卖方评分

                            string[] sellstrs = new string[2];

                            if (S_Can == true)//完全履约
                            {
                                DataRow[] drs = dt_xypf.Select("Number='1309000009'");
                                sellstrs = xymc.SqlText(drs[0], selleremail, htnum, "", buyjsbh, "买家卖家交易账户", "admin", "");
                            }
                            else//未完全履约
                            {
                                DataRow[] drs = dt_xypf.Select("Number='1309000010'");
                                sellstrs = xymc.SqlText(drs[0], selleremail, htnum, "", buyjsbh, "买家卖家交易账户", "admin", "");
                            }

                            //买方关联经纪人评分   
                            string[] bjjrstrs = new string[2];


                            if (B_Can == true)
                            {

                                DataRow[] drs = dt_xypf.Select("Number='1309000002'");
                                bjjrstrs = xymc.SqlText(drs[0], buyjjr, buyeremail, htnum, bjjrjsbh, "经纪人交易账户", "admin", "");
                            }
                            else
                            {
                                DataRow[] drs = dt_xypf.Select("Number='1309000003'");
                                bjjrstrs = xymc.SqlText(drs[0], buyjjr, buyeremail, htnum, bjjrjsbh, "经纪人交易账户", "admin", "");

                            }
                            //卖方关联经纪人评分 
                            string[] sjjrstrs = new string[2];
                            if (S_Can == true)
                            {
                                DataRow[] drs = dt_xypf.Select("Number='1309000002'");
                                sjjrstrs = xymc.SqlText(drs[0], selljjr, selleremail, htnum, sjjrjsbh, "经纪人交易账户", "admin", "");
                            }
                            else
                            {
                                DataRow[] drs = dt_xypf.Select("Number='1309000003'");
                                sjjrstrs = xymc.SqlText(drs[0], selljjr, selleremail, htnum, sjjrjsbh, "经纪人交易账户", "admin", "");


                            }

                            jfl.Add(buystrs[0]);
                            jfl.Add(buystrs[1]);
                            jfl.Add(sellstrs[0]);
                            jfl.Add(sellstrs[1]);
                            jfl.Add(sjjrstrs[0]);
                            jfl.Add(sjjrstrs[1]);
                            jfl.Add(bjjrstrs[0]);
                            jfl.Add(bjjrstrs[1]);


                        }
                        catch (Exception ex)
                        {
                            ht_result["执行结果"] = "err";
                            ht_result["提示文本"] = "更新用户评分的过程出现错误：" + ex.ToString();
                            return ht_result;
                        }

                        #endregion



                        //此种情况的数据为自动清盘，需要发送提醒
                        Hashtable ht_saleinfo = PublicClass2013.GetUserInfo(dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString());
                        string saler = "不存在";
                        if (((DataTable)ht_saleinfo["完整数据集"]).Rows.Count > 0)
                        {
                            saler = ((DataTable)ht_saleinfo["完整数据集"]).Rows[0]["I_JYFMC"].ToString();
                        }
                        Hashtable ht_sale = new Hashtable();
                        ht_sale["type"] = "集合集合经销平台";
                        ht_sale["提醒对象登陆邮箱"] = dt_info.Rows[i]["T_YSTBDDLYX"].ToString();
                        ht_sale["提醒对象用户名"] = "";//用户名不用传参数，其他地方已处理
                        ht_sale["提醒对象结算账户类型"] = dt_info.Rows[i]["T_YSTBDJSZHLX"];
                        ht_sale["提醒对象角色编号"] = dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString();
                        ht_sale["提醒对象角色类型"] = "卖家";
                        ht_sale["提醒内容文本"] = "尊敬的" + saler + "，您" + dt_info.Rows[i]["Z_HTBH"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                        ht_sale["创建人"] = "admin";
                        listTX.Add(ht_sale);
                        alLog.Add("提醒：" + ht_sale["提醒对象登陆邮箱"].ToString() + "→" + ht_sale["提醒内容文本"].ToString());

                        Hashtable ht_buyinfo = PublicClass2013.GetUserInfo(dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString());
                        string buyer = "不存在";
                        if (((DataTable)ht_buyinfo["完整数据集"]) != null && ((DataTable)ht_buyinfo["完整数据集"]).Rows.Count > 0)
                        {
                            buyer = ((DataTable)ht_buyinfo["完整数据集"]).Rows[0]["I_JYFMC"].ToString();
                        }
                        Hashtable ht_buy = new Hashtable();
                        ht_buy["type"] = "集合集合经销平台";
                        ht_buy["提醒对象登陆邮箱"] = dt_info.Rows[i]["Y_YSYDDDLYX"].ToString();
                        ht_buy["提醒对象用户名"] = "";//用户名不用传参数，其他地方已处理
                        ht_buy["提醒对象结算账户类型"] = dt_info.Rows[i]["Y_YSYDDJSZHLX"];
                        ht_buy["提醒对象角色编号"] = dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString();
                        ht_buy["提醒对象角色类型"] = "买家";
                        ht_buy["提醒内容文本"] = "尊敬的" + buyer + "，您" + dt_info.Rows[i]["Z_HTBH"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                        ht_buy["创建人"] = "admin";
                        listTX.Add(ht_buy);
                        alLog.Add("提醒：" + ht_buy["提醒对象登陆邮箱"].ToString() + "→" + ht_buy["提醒内容文本"].ToString());
                    }
                }
            }
            #endregion
            alLog.AddRange(al);

            #region //处理钱的数据，获取每个人应该变更的余额数据
            DataTable dt_Money_end = new DataTable();
            dt_Money_end.Columns.Add("登录邮箱");
            dt_Money_end.Columns.Add("金额");

            if (dt_Money != null && dt_Money.Rows.Count > 0)
            {
                //开始数量合并
                for (int i = 0; i < dt_Money.Rows.Count; i++)
                {
                    //是否需要更新新表
                    bool needadd = true;
                    for (int p = 0; p < dt_Money_end.Rows.Count; p++)
                    {
                        //发现匹配列，开始更新新表原有数据。
                        if (dt_Money_end.Rows[p]["登录邮箱"].ToString() == dt_Money.Rows[i]["登录邮箱"].ToString() && dt_Money.Rows[i]["数据类型"].ToString() == "实")
                        {
                            if (dt_Money.Rows[i]["运算"].ToString() == "加")
                            {
                                dt_Money_end.Rows[p]["金额"] = (Convert.ToDouble(dt_Money_end.Rows[p]["金额"]) + Convert.ToDouble(dt_Money.Rows[i]["金额"])).ToString("#0.00");
                            }
                            else if (dt_Money.Rows[i]["运算"].ToString() == "减")
                            {
                                dt_Money_end.Rows[p]["金额"] = (Convert.ToDouble(dt_Money_end.Rows[p]["金额"]) - Convert.ToDouble(dt_Money.Rows[i]["金额"])).ToString("#0.00");
                            }
                            //标记为无需添加新行,此行被合并计算了
                            needadd = false;
                        }
                    }
                    if (needadd)
                    {
                        dt_Money_end.Rows.Add(new object[] { dt_Money.Rows[i]["登录邮箱"].ToString(), dt_Money.Rows[i]["金额"].ToString() });
                    }
                }
            }
            #endregion

            WriteLog(alLog);//将需要执行的内容写日志
            #region 执行所有需要执行的内容
            //开始执行sql语句
            try
            {
                //执行状态更新及账款流水插入的sql语句
                DbHelperSQL.ExecSqlTran(al);
            }
            catch (Exception ex)
            {
                ht_result["执行结果"] = "err";
                ht_result["提示文本"] = "状态更新及账款流水写入过程出现错误：" + ex.ToString();
                return ht_result;
            }
            try
            {
                //执行余额更新程序
                for (int i = 0; i < dt_Money_end.Rows.Count; i++)
                {
                    ClassMoney2013 CM2013 = new ClassMoney2013();
                    CM2013.FreezeMoneyT(dt_Money_end.Rows[i]["登录邮箱"].ToString(), dt_Money_end.Rows[i]["金额"].ToString(), "+");
                    alLog.Add("注册表余额更新：" + dt_Money_end.Rows[i]["登录邮箱"].ToString() + "→" + dt_Money_end.Rows[i]["金额"].ToString());
                }
            }
            catch (Exception ex)
            {
                ht_result["执行结果"] = "err";
                ht_result["提示文本"] = "更新余额的过程出现错误：" + ex.ToString();
                return ht_result;
            }
                                
            //更新评分
            try
            {
                if (jfl.Count > 0)
                {
                    DbHelperSQL.ExecuteSqlTran(jfl);
                }
 
            }
            catch (Exception ex)
            {
                ht_result["执行结果"] = "err";
                ht_result["提示文本"] = "更新用户评分的过程出现错误：" + ex.ToString();
                return ht_result;
            }


            try
            {
                //发送提醒
                PublicClass2013.Sendmes(listTX);
            }
            catch (Exception ex)
            {
                ht_result["执行结果"] = "err";
                ht_result["提示文本"] = "发送提醒过程出现错误：" + ex.ToString();
                return ht_result;
            }         
            #endregion

            ht_result["执行结果"] = "ok";
            ht_result["提示文本"] = "执行成功!";
        }   
        return ht_result;
    }

    /// <summary>
    /// 写入运行日志
    /// </summary>
    /// <param name="strLog"></param>
    public static void WriteLog(ArrayList al)
    {
        string sFilePath = System.Web.HttpRuntime.AppDomainAppPath + "\\logs\\" + DateTime.Now.ToString("yyyyMMdd");
        string sFileName = DateTime.Now.ToString("dd") + ".log";
        sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径
        if (!Directory.Exists(sFilePath))//验证路径是否存在
        {
            Directory.CreateDirectory(sFilePath);//不存在则创建
        }
        FileStream fs;
        StreamWriter sw;
        if (File.Exists(sFileName))//验证文件是否存在，有则追加，无则创建
        {
            fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
        }
        else
        {
            fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
        }
        sw = new StreamWriter(fs);
        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "合同到期监控执行语句：");
        if (al.Count > 0)
        {
            for (int i = 0; i < al.Count; i++)
            {
                sw.WriteLine("---" + al[i].ToString());
                sw.WriteLine("");
            }
        }
        else
        {
            sw.WriteLine("没有需要执行的sql语句！");
        }
        sw.WriteLine("");
        sw.Close();
        fs.Close();
    }
}