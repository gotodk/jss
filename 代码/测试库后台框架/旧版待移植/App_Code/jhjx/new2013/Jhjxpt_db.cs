using FMOP.DB;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;

/// <summary>
/// Jhjxpt_db 的摘要说明
/// </summary>
public class Jhjxpt_db
{
	public Jhjxpt_db()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 测试返回值标准
    /// </summary>
    /// <returns></returns>
    public DataSet jhjx_db(DataSet dsreturn, DataTable ht )
    {
        //进行处理....
        //解冻投标保证金
        try
        {
            if (ht != null && ht.Rows.Count > 0)
            {

                ArrayList al = new ArrayList();
                DataRow dr = ht.Rows[0];
                string action = dr["ispass"].ToString().Trim();
                Hashtable htt = PublicClass2013.GetParameterInfo();
                ClassMoney2013 cm = new ClassMoney2013();

                //工作日验证 

                Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
                if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                    return dsreturn;
                }
                if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
                    return dsreturn;
                }
                if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                    return dsreturn;
                }


                if (htt["是否在服务时间内(工作时)"].ToString().Trim() != "是" )
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                    return dsreturn;
                }
               
              
                 Hashtable hsuser = PublicClass2013.GetUserInfo(dr["卖家角色编号"].ToString());
                  //验证是开通交易账户
                 if (hsuser["交易账户是否开通"].ToString().Trim() != "是")
                 {
                     dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                     dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                     return dsreturn;
                 }

 

                //验证是否休眠

                 if (hsuser["是否休眠"].ToString().Trim() == "是")
                 {
                     dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                     dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                     return dsreturn;
                 }

               // object objhtzt = DbHelperSQL.GetSingle("")

                //验证金额是否足够 

               // string Strdjinfo = " select T_YSTBDDJTBBZJ,Z_ZBJE,Z_LYBZJJE,Y_YSYDDDLYX,Y_YSYDDJSZHLX,Y_YSYDDMJJSBH,Z_SPMC,Y_YSYDDBH,Z_HTBH,Z_HTQX from dbo.AAA_ZBDBXXB where Number = '" + dr["Number"].ToString() + "' and Z_HTZT = '中标'  ";
                 string strwhere = " and  T_YSTBDBH='" + dr["投标单号"].ToString().Trim() + "' ";
                   
                //确定合同期限
                //string StrHTQX = " select  Z_HTQX from dbo.AAA_ZBDBXXB where Number = '" + dr["Number"].ToString() + "' and Z_HTZT = '中标'  ";
                //object objhtqx = DbHelperSQL.GetSingle(StrHTQX);
                 string objhtqx = dr["合同期限"].ToString().Trim();
                 if (objhtqx.ToString().Trim() != "")
                 {
                    if (objhtqx.Equals("即时"))
                    {
                        strwhere = " and Number = '" + dr["Number"].ToString() + "'";
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请确认中标定标信息表内存在单号为：" + dr["Number"].ToString().Trim() + "的已中标的，且未冻结履约保证金的数据存在！";
                    return dsreturn;
                }
                 string Strdjinfo = "select  T_YSTBDBH  ,Z_SPBH , Z_SPMC ,Z_GG , T_YSTBDDJTBBZJ ,T_YSTBDTBNSL ,Z_ZBSL,Z_ZBJG ,Z_ZBJE,Z_ZBSJ , Z_LYBZJJE,Y_YSYDDDLYX,Y_YSYDDMJJSBH,Y_YSYDDBH ,Z_HTBH,Z_HTQX,Y_YSYDDJSZHLX,Y_YSYDDMJJSBH,Z_HTQX from AAA_ZBDBXXB where  Z_HTZT = '中标' and  Z_SFDJLYBZJ='否' "+strwhere;
                DataSet dsobj = DbHelperSQL.Query(Strdjinfo);
               // if(ds)
                double doutbbzj = 0.00;
                double doulvbzj = 0.00;
                double douchane = 0.00;
              


                if (dsobj != null && dsobj.Tables[0].Rows.Count >0)
                {  
                    doutbbzj = Convert.ToDouble(dsobj.Tables[0].Rows[0]["T_YSTBDDJTBBZJ"].ToString().Trim());
                   // HTQX = dsobj.Tables[0].Rows[0]["Z_HTQX"].ToString().Trim();
                   
                    foreach (DataRow drobj in dsobj.Tables[0].Rows)
                    {

                        doulvbzj += Convert.ToDouble(drobj["Z_LYBZJJE"].ToString().Trim());
                    }
                    Double douye = cm.GetMoneyT(dr["登陆邮箱"].ToString(), "");
                    if (douye + doutbbzj < doulvbzj)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的结算账户可用余额为" + douye.ToString() + "元，差额" + (doulvbzj - (douye + doutbbzj)).ToString("0.00") + "元，\n\r履约保证金无法足额冻结，不能定标，请您增加账户资金。";
                        return dsreturn;
                    }
                    douchane = doutbbzj - doulvbzj;

                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请确认原始投标单号为" + dr["投标单号"].ToString().Trim() + "的投标单已中标，且单据状态正常！";
                    return dsreturn;
                }

                if (action == "no")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "pass";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您将冻结" + doulvbzj.ToString("0.00") + "元的履约保证金予以定标。定标后，\n\r提货单功能自动开通，平台同时代所有买方向您开具定标金额5%的保证函，您对应的投标保证金即予解冻。";
                    return dsreturn;

                }

                if (action == "ok")
                {

                    string Strtb = " select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000005'";
                    DataSet ds = DbHelperSQL.Query(Strtb);


                    if (ds != null && ds.Tables[0].Rows.Count == 1 && dsobj != null && dsobj.Tables[0].Rows.Count >0)
                    {

                        DataRow drjeid = ds.Tables[0].Rows[0];
                        drjeid["ZY"] = drjeid["ZY"].ToString().Trim().Replace("[x1]", dr["投标单号"].ToString().Trim());

                        DateTime drnow = DateTime.Now;

                        string Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB","");
                        //定标解冻投标保证金
                        string strinserttbbbj = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + dr["登陆邮箱"].ToString() + "','" + dr["结算账户类型"].ToString() + "','" + dr["卖家角色编号"].ToString() + "','AAA_TBD','" + dr["投标单号"].ToString() + "','" + drnow + "','" + drjeid["YSLX"].ToString() + "','" + dsobj.Tables[0].Rows[0]["T_YSTBDDJTBBZJ"].ToString().Trim() + "','" + drjeid["XM"].ToString() + "','" + drjeid["XZ"].ToString() + "','" + drjeid["ZY"].ToString() + "','" + drjeid["SJLX"].ToString() + "','" + dr["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";

                        al.Add(strinserttbbbj);

                        //冻结履约保证金
                        string Strlv = " select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000047'";

                        DataSet dsdj = DbHelperSQL.Query(Strlv);
                        if (dsdj != null && dsdj.Tables[0].Rows.Count == 1)
                        {
                            DataRow drlvy = dsdj.Tables[0].Rows[0];
                            string strzy = drlvy["ZY"].ToString();
                            if (dsobj != null && dsobj.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsobj.Tables[0].Rows.Count; i++)
                                {
                                    Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    drlvy["ZY"] = strzy.ToString().Trim().Replace("[x1]", dsobj.Tables[0].Rows[i]["Z_HTBH"].ToString().Trim());
                                    string Strinsetlvbzj = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + dr["登陆邮箱"].ToString() + "','" + dr["结算账户类型"].ToString() + "','" + dr["卖家角色编号"].ToString() + "','AAA_TBD','" + dr["投标单号"].ToString() + "','" + drnow + "','" + drlvy["YSLX"].ToString() + "','" + dsobj.Tables[0].Rows[i]["Z_LYBZJJE"].ToString().Trim() + "','" + drlvy["XM"].ToString() + "','" + drlvy["XZ"].ToString() + "','" + drlvy["ZY"].ToString() + "','" + drlvy["SJLX"].ToString() + "','" + dr["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";

                                    al.Add(Strinsetlvbzj);
                                }
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "履约保证金流水信息插入有错！";
                                return dsreturn;
                            }
                            //更改账户余额

                            string Sqlzhye = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE +(" + douchane + ") where B_DLYX='" + dr["登陆邮箱"].ToString() + "'";
                            al.Add(Sqlzhye);
                            //更改中标定标表中数据
                            DateTime dtheqx = new DateTime();
                            if (dsobj.Tables[0].Rows[0]["Z_HTQX"].ToString().Trim().Equals("三个月"))
                            {
                                dtheqx = drnow.AddDays(90);
                            }
                            else if (dsobj.Tables[0].Rows[0]["Z_HTQX"].ToString().Trim().Equals("一年"))
                            {
                                dtheqx = drnow.AddDays(365);
                            }
                            else
                            {
                                dtheqx = drnow.AddDays(7);
                            }



                            string Strupdate = "  update AAA_ZBDBXXB set Z_HTZT = '定标',Z_DBSJ='" + drnow + "',Z_HTJSRQ = '" + dtheqx + "', Z_SFDJLYBZJ='是'  where T_YSTBDBH = '" + dr["投标单号"].ToString() + "' ";
                            if (objhtqx.ToString().Trim() != "")
                            {
                                if (objhtqx.Equals("即时"))
                                {
                                    Strupdate += " and Number = '" + dr["Number"].ToString() + "'";
                                }
                            }


                            al.Add(Strupdate);
                           

                           DbHelperSQL.ExecSqlTran(al);

                            //发送提醒
                            Hashtable htmes = null ;
                            string StrMC = "";
                            object obj = null;
                            for (int i = 0; i < dsobj.Tables[0].Rows.Count; i++)
                            {
                                htmes = new Hashtable();
                                htmes["type"] = "集合集合经销平台";

                                htmes["提醒对象登陆邮箱"] = dsobj.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString().Trim();
                                htmes["提醒对象用户名"] = "";
                                //获取交易方名称 
                                StrMC = "  select I_JYFMC from  AAA_DLZHXXB  where B_DLYX = '" + dsobj.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString().Trim() + "' ";

                                obj = DbHelperSQL.GetSingle(StrMC);

                                if (obj != null)
                                {
                                    htmes["提醒对象用户名"] = obj.ToString();
                                }


                                //dsobj.Tables[0].Rows[0]["Y_YSYDDDLYX"].ToString().Trim();
                                htmes["提醒对象结算账户类型"] = dsobj.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString().Trim();
                                htmes["提醒对象角色编号"] = dsobj.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString().Trim();
                                htmes["提醒对象角色类型"] = "买家";//dsobj.Tables[0].Rows[0]["Y_YSYDDDLYX"].ToString().Trim();

                                htmes["提醒内容文本"] = "您" + dsobj.Tables[0].Rows[i]["Z_SPMC"].ToString().Trim() + "商品的" + dsobj.Tables[0].Rows[i]["Y_YSYDDBH"].ToString().Trim() + "预订单已定标，提货单功能已开通，平台已经代您向卖方开具保证函，请您按照" + dsobj.Tables[0].Rows[i]["Z_HTBH"].ToString() + "《电子购货合同》约定执行。";
                                htmes["创建人"] = dr["登陆邮箱"].ToString();
                                List<Hashtable> lth = new List<Hashtable>();
                                lth.Add(htmes);
                                PublicClass2013.Sendmes(lth);
                                
                                
                            }
                                


                            #region 测试语句
                            //string sql = "";
                            //if (al != null && al.Count > 0)
                            //{
                            //    for (int i = 0; i < al.Count; i++)
                            //    {
                            //        sql += al[i].ToString() +"|| <br />";
                            //    }
                            //}
                            //dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] =sql.Trim();
                            //return dsreturn;
                            #endregion 
                           
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您" + doulvbzj.ToString("0.00") + "元履约保证金冻结成功，\n\r" + dr["投标单号"].ToString().Trim() + "投标单已定标，" + doutbbzj.ToString("0.00") + "元的投标保证金已解冻。";
                            return dsreturn;


                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "冻结履约保证金是出错！";
                            return dsreturn;
                        }

                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有任何数据被更新！";
                        return dsreturn;
                    }



                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有任何数据被更新！";
                    return dsreturn;
                }
            }
            else 
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "参数不可为空！";
                return dsreturn;
            }
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "程序出现异常，提交失败！";
            return dsreturn;
        }
        //附加数据表

    }

    public DataSet jhjx_DBYbzhinfo(DataSet dsreturn, string  Number)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有查找到任何数据！";
        string Strsql = "   select Z_HTBH as 合同编号, ( select MJSDJJPL from AAA_TBD where Number = AAA_ZBDBXXB.T_YSTBDBH  ) as 经济批量,Z_RJZGFHL as 日均最高发货量,Z_ZBSL as 定标数量,( select I_JYFMC from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.T_YSTBDDLYX ) as 卖家名称,( select I_LXRSJH from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.T_YSTBDDLYX ) as 卖家联系方式,( select I_LXRXM from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.T_YSTBDDLYX ) as 卖家联系人,( select I_JYFMC from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.Y_YSYDDDLYX ) as  买家家名称,( select I_LXRSJH from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.Y_YSYDDDLYX ) as 买家联系方式,( select I_LXRXM from AAA_DLZHXXB where B_DLYX = AAA_ZBDBXXB.Y_YSYDDDLYX ) as 买家联系人,Z_HTQX as 合同期限  from AAA_ZBDBXXB where Number = '" + Number + "'";
        DataSet ds = DbHelperSQL.Query(Strsql);
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            DataTable dt = ds.Tables[0];
            DataTable dtRt = dt.Copy();
            // dtRt.Rows[0]["NEWDJDJ"] = (Convert.ToDouble(dtRt.Rows[0]["NDGJE"].ToString().Trim()) * Convert.ToDouble(htt["买家订金比率"].ToString().Trim())).ToString("#0.00");
            dtRt.TableName = "SPXX";
            dsreturn.Tables.Add(dtRt);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "单号为：" + Number + "中标定标信息单据不存在！";
            return dsreturn;
        }



        return dsreturn;
 
    }


    public DataSet jhjx_DBZBXXinfoget(DataSet dsreturn, string Number)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有查找到任何数据！";
        string Strsql = "   SELECT  distinct  AAA_ZBDBXXB.Number AS 主键 ,Z_QPZT AS 清盘状态,Z_QPKSSJ AS 清盘开始时间,Z_QPJSSJ AS 清盘结束时间, Z_HTBH AS 电子购货合同编号,Z_HTJSRQ AS 合同结束日期,Z_SPMC AS 商品名称,Z_SPBH AS 商品编号,Z_GG AS 规格,Z_ZBSL AS 合同数量,Z_ZBJG AS 单价,Z_ZBJE AS 合同金额,Z_LYBZJJE AS 履约保证金金额,  (select  isnull(sum (T_THSL),0)   from AAA_THDYFHDXXB where ZBDBXXBBH=AAA_ZBDBXXB.Number and  F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','卖家主动退货','撤销') AND (Z_QPZT='清盘中' or Z_QPZT='清盘结束' )) AS 争议数量 ,(select  ISNULL(sum (T_DJHKJE),0.00)   from AAA_THDYFHDXXB where ZBDBXXBBH=AAA_ZBDBXXB.Number and  F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','卖家主动退货','撤销') AND (Z_QPZT='清盘中' or Z_QPZT='清盘结束' ) ) AS  争议金额,'人工清盘' AS 清盘类型,CASE WHEN Z_HTZT='未定标废标' THEN '废标' WHEN Z_HTZT='定标合同到期' THEN '《电子购货合同》期满' WHEN Z_HTZT='定标合同终止' THEN '废标' WHEN Z_HTZT='定标执行完成' THEN '合同期内买家中标量全部无异议收货' ELSE '其他' END AS 清盘原因 ,T_YSTBDDLYX AS 卖方邮箱,Y_YSYDDDLYX AS 买方邮箱,Q_ZMSCFDLYX AS 证明上传方邮箱,Q_SFYQR AS 是否已确认 ,Q_ZMWJLJ AS 证明文件路径, Q_ZFLYZH AS 转付来源账户, Q_ZFMBZH AS 转付目标账户,Q_ZFJE AS 转付金额,Q_CLYJ AS 处理依据  FROM  AAA_ZBDBXXB JOIN AAA_THDYFHDXXB ON AAA_ZBDBXXB.Number=AAA_THDYFHDXXB.ZBDBXXBBH WHERE  F_DQZT NOT IN ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','卖家主动退货') AND (Z_QPZT='清盘中' or Z_QPZT='清盘结束' ) and AAA_ZBDBXXB.Number='" + Number + "' and isnull(Z_QPKSSJ,'') != isnull(Z_QPJSSJ,'') ";
        DataSet ds = DbHelperSQL.Query(Strsql);
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            DataTable dt = ds.Tables[0];
            DataTable dtRt = dt.Copy();
            // dtRt.Rows[0]["NEWDJDJ"] = (Convert.ToDouble(dtRt.Rows[0]["NDGJE"].ToString().Trim()) * Convert.ToDouble(htt["买家订金比率"].ToString().Trim())).ToString("#0.00");
            dtRt.TableName = "HTPXX";
            dsreturn.Tables.Add(dtRt);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "单号为：" + Number + "中标定标信息单据不存在！";
            return dsreturn;
        }



        return dsreturn;

    }

}