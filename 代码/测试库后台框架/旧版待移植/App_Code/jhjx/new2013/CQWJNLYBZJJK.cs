using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FMOP.DB;

/// <summary>
/// CQWJNLYBZJJK 的摘要说明
/// </summary>
public class CQWJNLYBZJJK
{
	public CQWJNLYBZJJK()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public Hashtable ServerCQWJNLYBZJJK()
    {
        Hashtable ht_result = new Hashtable();
        ht_result["执行结果"] = "ok";
        ht_result["提示文本"] = "执行成功！";
        string jkbh = "履约保证金超期监控";

        ArrayList alLog = new ArrayList();//用户保存需要写入日志的内容
        //获取需要插入的账款流水明细资金类型基本上数据
        DataSet ds_ZKMX=DbHelperSQL .Query ("select * from AAA_moneyDZB where number in ('1304000020','1304000006','1304000035','1304000025')");

        //获取需要超期未缴纳履约保证金的数据明细信息
        Hashtable htPTVal = PublicClass2013.GetParameterInfo();//获取履约保证金缴纳超期天数
        if (htPTVal["卖家履约保证金冻结期限"] == null || htPTVal["卖家履约保证金冻结期限"].ToString() == "")
        {
            ht_result["执行结果"] = "err";
            ht_result["提示文本"] = "未获取到卖家履约保证金冻结期限";
            return ht_result;
        }

        //string sql_info = " select *, cast((Z_ZBSL/Total)*T_YSTBDDJTBBZJ as numeric(18,2)) as 买家补偿收益,0.00 as 卖家违约赔偿总额,'' as 数据类型  from (select *,(select SUM(Z_ZBSL) from AAA_ZBDBXXB as tb1 where tb1.T_YSTBDBH=AAA_ZBDBXXB.T_YSTBDBH and Z_HTZT='中标' AND Z_HTQX<>'即时' and  CONVERT (varchar(10), DATEADD(DD," + htPTVal["卖家履约保证金冻结期限"].ToString() + ",Z_ZBSJ),120)<'" + DateTime.Now.ToString("yyyy-MM-dd") + "' ) as Total from AAA_ZBDBXXB where Z_HTQX <> '即时' AND Z_HTZT='中标' and  CONVERT (varchar(10), DATEADD(DD," + htPTVal["卖家履约保证金冻结期限"].ToString() + ",Z_ZBSJ),120)<'" + DateTime.Now.ToString("yyyy-MM-dd") + "') as tab ";
        //sql_info += " union all ";
       // sql_info += " select *, cast((Z_ZBSL/Total)*T_YSTBDDJTBBZJ as numeric(18,2)) as 买家补偿收益,0.00 as 卖家违约赔偿总额,'' as 数据类型  from (select *,(select SUM(Z_ZBSL) from AAA_ZBDBXXB as tb1 where tb1.T_YSTBDBH=AAA_ZBDBXXB.T_YSTBDBH and Z_HTZT='中标' AND Z_HTQX='即时' and  CONVERT (varchar(10), DATEADD(DD," + htPTVal["即时合同定标下提货单期限"].ToString() + ",Z_ZBSJ),120)<'" + DateTime.Now.ToString("yyyy-MM-dd") + "' ) as Total from AAA_ZBDBXXB where Z_HTQX = '即时' AND Z_HTZT='中标' and  CONVERT (varchar(10), DATEADD(DD," + htPTVal["即时合同定标下提货单期限"].ToString() + ",Z_ZBSJ),120)<'" + DateTime.Now.ToString("yyyy-MM-dd") + "') as tab2 ";

        string sql_info = "select a.*,b.Total,(case Z_HTQX when '即时' then a.T_YSTBDDJTBBZJ else cast((Z_ZBSL/b.Total)*a.T_YSTBDDJTBBZJ as numeric(18,2))  end )  as 买家补偿收益,0.00 as 卖家违约赔偿总额,'' as 数据类型 from AAA_ZBDBXXB as a left join (select T_YSTBDBH, isnull(SUM(Z_ZBSL),0) as Total from AAA_ZBDBXXB where Z_HTZT='中标' group by T_YSTBDBH) as b on b.T_YSTBDBH=a.T_YSTBDBH  where Z_HTZT='中标' and CONVERT (varchar(10), DATEADD(DD,(case Z_HTQX when '即时' then " + htPTVal["即时合同定标下提货单期限"].ToString() + " else " + htPTVal["卖家履约保证金冻结期限"].ToString() + " end),Z_ZBSJ),120)<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
        DataSet ds_info = new DataSet();
        try
        {
            ds_info = DbHelperSQL.Query(sql_info);
        }
        catch (Exception ex)
        {
            ht_result["执行结果"] = "err";
            ht_result["提示文本"] = "基础数据获取过程出现错误 ！" + ex.ToString();
            alLog.Add(ht_result["提示文本"].ToString());
            WriteLog(alLog);
            return ht_result;
        }
        if (ds_info == null || ds_info.Tables[0].Rows.Count <= 0)
        {
            ht_result["执行结果"] = "ok";
            ht_result["提示文本"] = "无超期未缴纳履约保证金数据！";
            alLog.Add(ht_result["提示文本"].ToString());
        }
        else
        {
            ArrayList al = new ArrayList();//用于保存需要执行的sql语句
            jhjx_JYFXYMX jyxy=new jhjx_JYFXYMX();

            //用户执行提醒插入功能的参数列表
            List<Hashtable> listTX = new List<Hashtable>();            

            DataTable dt_Money = DbHelperSQL.Query("select '' as 登录邮箱,0.00 as 金额, '' as 运算,'' as 数据类型 from AAA_ZBDBXXB where 1!=1").Tables[0];//用于保存钱的变动明细

            #region //处理买家相关的数据，包括：投标单、账款、提醒。买家数据根据中标定标信息表处理。
            DataRow[] dr_buyjd = ds_ZKMX.Tables[0].Select("number='1304000020'");//订金 废标解冻 
            DataRow[] dr_buybc = ds_ZKMX.Tables[0].Select("number='1304000035'");//补偿收益 收到卖家未定标补偿 
            for (int i = 0; i < ds_info.Tables[0].Rows.Count; i++)
            {
                //更新《中标定标信息表》中所获取的数据中的“合同状态”为“未定标废标”；更新“废标时间”。
                string sql_ZBDB = "update AAA_ZBDBXXB set Z_HTZT='未定标废标',Z_FBSJ='" + System.DateTime.Now.ToString() + "' where number='" + ds_info.Tables[0].Rows[i]["number"].ToString() + "'";
                al.Add(sql_ZBDB);

                //更新《预订单信息表》中“原始预订单编号”对应预定单号中【拟订购数量=已中标数量】的数据的“状态”为“撤销”，并更新“撤销时间”
                string sql_YDD = "update AAA_YDDXXB set ZT='撤销',CXSJ='" + System.DateTime.Now.ToString() + "' where number='" + ds_info.Tables[0].Rows[i]["Y_YSYDDBH"].ToString() + "' and NDGSL=YZBSL";
                al.Add(sql_YDD);


                //插入《账款流水明细表》中买家订金解冻               
               string zy_buyjd = dr_buyjd[0]["zy"].ToString().Replace("[x1]", ds_info.Tables[0].Rows[i]["Y_YSYDDBH"].ToString());//摘要文字：[x1]预订单中标后卖家未定标                 
                string KeyNumber_buyjd = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                string sql_BuyJD = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buyjd + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + ds_info.Tables[0].Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buyjd[0]["YSLX"].ToString() + "'," + ds_info.Tables[0].Rows[i]["Z_DJJE"].ToString() + ",'" + dr_buyjd[0]["XM"].ToString() + "','" + dr_buyjd[0]["XZ"].ToString() + "','" + zy_buyjd + "','"+jkbh+"','','" + dr_buyjd[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                al.Add(sql_BuyJD);

                //插入《账款流水明细表》中买家补偿收益
                string zy_buybc = dr_buybc[0]["zy"].ToString().Replace("[x1]", ds_info.Tables[0].Rows[i]["Y_YSYDDBH"].ToString()).Replace("[x2]", ds_info.Tables[0].Rows[i]["Z_SPMC"].ToString());//摘要文字：[x1]预订单[x2]商品未定标              

                string KeyNumber_buybc = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                string sql_BuyBC = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buybc + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + ds_info.Tables[0].Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buybc[0]["YSLX"].ToString() + "'," + ds_info.Tables[0].Rows[i]["买家补偿收益"].ToString () + ",'" + dr_buybc[0]["XM"].ToString() + "','" + dr_buybc[0]["XZ"].ToString() + "','" + zy_buybc + "','"+jkbh+"','','" + dr_buybc[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                al.Add(sql_BuyBC);

                //将钱的变动增加到dt_Money中
                dt_Money.Rows.Add(new object[] { ds_info.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(ds_info.Tables[0].Rows[i]["Z_DJJE"]), '加', dr_buyjd[0]["SJLX"].ToString() });//买家订金解冻
                dt_Money.Rows.Add(new object[] { ds_info.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(ds_info.Tables[0].Rows[i]["买家补偿收益"]), '加', dr_buybc[0]["SJLX"].ToString() });//买家补充收益

                //向《平台提醒信息表》中插入给对应的买家的提醒
                Hashtable ht = new Hashtable();
                ht["type"] = "集合集合经销平台";
                ht["提醒对象登陆邮箱"] = ds_info.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString();
                ht["提醒对象用户名"] = "";//对象名不需要传参数，传什么都没意义
                ht["提醒对象结算账户类型"] = ds_info.Tables[0].Rows[i]["Y_YSYDDJSZHLX"];
                ht["提醒对象角色编号"] = ds_info.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString();
                ht["提醒对象角色类型"] = "买家";
                ht["提醒内容文本"] = "因卖方未履行定标程序，您所中标的" + ds_info.Tables[0].Rows[i]["Y_YSYDDBH"].ToString() + "预订单作废，对应订金已解冻，卖方向您支付的" + ds_info.Tables[0].Rows[i]["买家补偿收益"].ToString() + "元违约赔偿金已计入您的账户。";
                ht["创建人"] = "admin";
                listTX.Add(ht);
                alLog.Add("提醒："+ht["提醒对象登陆邮箱"].ToString() + "→" + ht["提醒内容文本"].ToString());
            }
#endregion

            #region //处理卖家相关的数据，包括：投标单、账款、提醒
            DataTable dt_sale = ds_info.Tables[0].DefaultView.ToTable(true, new string[] { "Z_SPMC", "T_YSTBDBH", "T_YSTBDDLYX", "T_YSTBDJSZHLX", "T_YSTBDMJJSBH", "T_YSTBDDJTBBZJ", "卖家违约赔偿总额"});
            //处理卖家的账款明细和提醒信息
                           
            DataRow[] dr_salejd = ds_ZKMX.Tables[0].Select("number='1304000006'");//投标保证金 废标解冻 
            DataRow[] dr_salewybc = ds_ZKMX.Tables[0].Select("number='1304000025'");//违约赔偿金 卖家中标后未定标扣罚 
            for (int i = 0; i < dt_sale.Rows.Count; i++)
            {
                //更新《投标单》中“原始投标单编号”对应的数据的“状态”为“撤销”，并更新“撤销时间”；
                string sql_TBD = "update AAA_TBD set ZT='撤销',CXSJ='" + DateTime.Now.ToString() + "' where number='" + dt_sale.Rows[i]["T_YSTBDBH"].ToString() + "' AND TBNSL=YZBSL";
                al.Add(sql_TBD);

               //插入《账款流水明细表》中卖家投保保证金解冻  
                string zy_salejd = dr_salejd[0]["zy"].ToString().Replace("[x1]", dt_sale.Rows[i]["T_YSTBDBH"].ToString());//摘要文字：[x1]投标单中标后未定标

                string KeyNumber_salejd = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                string sql_Salejd = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_salejd + "','" + dt_sale.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_sale.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_sale.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_TBD','" + dt_sale.Rows[i]["T_YSTBDBH"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_salejd[0]["YSLX"].ToString() + "'," + Convert .ToDouble (dt_sale.Rows[i]["T_YSTBDDJTBBZJ"]) + ",'" + dr_salejd[0]["XM"].ToString() + "','" + dr_salejd[0]["XZ"].ToString() + "','" + zy_salejd + "','"+jkbh+"','','" + dr_salejd[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                al.Add(sql_Salejd);

                //插入《账款流水明细表》中卖家违约赔偿金 
                dt_sale.Rows[i]["卖家违约赔偿总额"] = ds_info.Tables[0].Compute("sum(买家补偿收益)", "T_YSTBDMJJSBH='" + dt_sale.Rows[i]["T_YSTBDMJJSBH"].ToString() + "' and T_YSTBDBH='" + dt_sale.Rows[i]["T_YSTBDBH"].ToString() + "'").ToString();
              
                string zy_salewybc = dr_salewybc[0]["zy"].ToString().Replace("[x1]", dt_sale.Rows[i]["T_YSTBDBH"].ToString()).Replace("[x2]", dt_sale.Rows[i]["Z_SPMC"].ToString());//摘要文字：[x1]投标单[x2]商品中标后未定标

                string KeyNumber_salewypc = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                string sql_Salewybc = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_salewypc + "','" + dt_sale.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_sale.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_sale.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_TBD','" + dt_sale.Rows[i]["T_YSTBDBH"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_salewybc[0]["YSLX"].ToString() + "'," + Convert.ToDouble (dt_sale.Rows[i]["卖家违约赔偿总额"]) + ",'" + dr_salewybc[0]["XM"].ToString() + "','" + dr_salewybc[0]["XZ"].ToString() + "','" + zy_salewybc + "','"+jkbh+"','','" + dr_salewybc[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                al.Add(sql_Salewybc);

                //将卖家钱的变动增加到dt_Money中
                dt_Money.Rows.Add(new object[] { dt_sale.Rows[i]["T_YSTBDDLYX"].ToString(), Convert.ToDouble(dt_sale.Rows[i]["T_YSTBDDJTBBZJ"]), '加', dr_salejd[0]["SJLX"].ToString(), });//买家订金解冻
                dt_Money.Rows.Add(new object[] { dt_sale.Rows[i]["T_YSTBDDLYX"].ToString(), Convert.ToDouble(dt_sale.Rows[i]["卖家违约赔偿总额"].ToString()), '减', dr_salewybc[0]["SJLX"].ToString(), });//买家补充收益

                //向《平台提醒信息表》中插入给对应的卖家的提醒
                Hashtable ht = new Hashtable();
                ht["type"] = "集合集合经销平台";
                ht["提醒对象登陆邮箱"] = dt_sale.Rows[i]["T_YSTBDDLYX"].ToString();
                ht["提醒对象用户名"] = "";//用户名不用传参数，其他地方已处理
                ht["提醒对象结算账户类型"] = dt_sale.Rows[i]["T_YSTBDJSZHLX"];
                ht["提醒对象角色编号"] = dt_sale.Rows[i]["T_YSTBDMJJSBH"].ToString();
                ht["提醒对象角色类型"] = "卖家";
                ht["提醒内容文本"] = "因您未能按时冻结履约保证金，您所中标的" + dt_sale.Rows[i]["T_YSTBDBH"] + "投标单作废，对应" + dt_sale.Rows[i]["卖家违约赔偿总额"].ToString() + "元的投标保证金已全额赔付给相应买方，作为补偿。";
                ht["创建人"] = "admin";
                listTX.Add(ht);
                alLog.Add("提醒：" + ht["提醒对象登陆邮箱"].ToString() + "→" + ht["提醒内容文本"].ToString());
            }
            alLog.AddRange(al);
            #endregion

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
            
            #region 扣罚卖家信用评分 EditTime 2013-09
            DataTable dt_saleXY = ds_info.Tables[0].DefaultView.ToTable(true, new string[] { "T_YSTBDDLYX", "Z_HTBH" });
            for (int i = 0; i < dt_saleXY.Rows.Count; i++)
            {
                string[] strs = new string[2];
                strs = jyxy.ZSZHMC_ZBHWDB(dt_saleXY.Rows[i]["T_YSTBDDLYX"].ToString().Trim(), dt_saleXY.Rows[i]["Z_HTBH"].ToString().Trim(), "卖家", "admin");
                al.Add(strs[0]);
                al.Add(strs[1]);
            }
            #endregion

           // WriteLog(alLog);//将需要执行的内容写日志
            #region 执行所有需要执行的内容
            //开始执行sql语句
            try
            {
                //执行状态更新及账款流水插入的sql语句
                DbHelperSQL.ExecSqlTran(al);               
            }
            catch(Exception ex)
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
            catch(Exception ex)
            { 
                ht_result["执行结果"] = "err";
                ht_result["提示文本"] = "更新余额的过程出现错误：" + ex.ToString();
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
        }
        
        return ht_result;
    }

    /// <summary>
    /// 写入运行日志
    /// </summary>
    /// <param name="strLog"></param>
    public static void WriteLog(ArrayList al)
    {
        string sFilePath =System.Web.HttpRuntime.AppDomainAppPath+"\\logs\\" + DateTime.Now.ToString("yyyyMMdd");
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
        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "超期未缴纳履约保证金监控执行语句：");
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