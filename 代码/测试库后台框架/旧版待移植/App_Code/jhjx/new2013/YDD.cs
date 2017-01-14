using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FMOP.DB;
/// <summary>
/// 预订单小帮助类
/// </summary>
public class YDD
{
	public YDD()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 撤销预订单
    /// </summary>
    /// <param name="dsreturn">要返回的DataSet</param>
    /// <param name="Number">预订单号</param>
    /// <returns>dsreturn</returns>
    public DataSet Ydd_CX(DataSet dsreturn, string Number)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到对应的预订单信息！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
        string sql = "SELECT * FROM AAA_YDDXXB WHERE Number='" + Number + "'";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        if (dt.Rows.Count > 0)
        {

            ClassMoney2013 AboutMoney = new ClassMoney2013();//与钱相关的类
            Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
            Hashtable htUserVal = PublicClass2013.GetUserInfo(dt.Rows[0]["MJJSBH"].ToString());//通过买家角色编号获取信息
            #region 验证们
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


            if (htUserVal["交易账户是否开通"].ToString().Trim() != "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
 
            if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("预订单") >= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
            if (htUserVal["是否休眠"].ToString().Trim() == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
            if (htUserVal["是否被默认经纪人暂停新业务"].ToString().Trim() == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人已暂停您的新业务，请联系您的经纪人或向平台申诉。";
                return dsreturn;
            }
            if (dt.Rows[0]["ZT"].ToString() == "中标")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的预订单已中标，无法撤销！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
            if (dt.Rows[0]["ZT"].ToString() == "撤销")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的预订单已撤销，无法重复撤销！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
            #endregion















            string YZBSL = dt.Rows[0]["YZBSL"].ToString().Trim();//已中标数量
            //如果没有出现拆单的情况
            if (YZBSL == "0")
            {
                List<string> list = new List<string>();//要执行的SQL语句
                #region 更改预订单信息
                string sql_NGZB = "UPDATE AAA_YDDXXB SET ZT='撤销' ,NDGJE=((NDGSL-YZBSL)*NMRJG) WHERE Number='" + Number + "'";//把拟订购数量更新为与中标数量相同的量，把状态改为中标
                #endregion
                list.Add(sql_NGZB);

                #region 生成流水信息
                string sql_LSDZ = "SELECT * FROM AAA_moneyDZB WHERE Number='1304000019'";//资金对照表中的撤销预订单

                DataTable dt_LSGZ = DbHelperSQL.Query(sql_LSDZ).Tables[0];
                string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES('" + PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "") + "','" + dt.Rows[0]["DLYX"].ToString() + "','" + dt.Rows[0]["JSZHLX"].ToString() + "','" + dt.Rows[0]["MJJSBH"].ToString() + "','" + "AAA_YDDXXB" + "','" + Number + "','" + DateTime.Now.ToString() + "','" + dt_LSGZ.Rows[0]["YSLX"].ToString() + "','" + dt.Rows[0]["DJDJ"].ToString() + "','" + dt_LSGZ.Rows[0]["XM"].ToString() + "','" + dt_LSGZ.Rows[0]["XZ"].ToString() + "','" + dt_LSGZ.Rows[0]["ZY"].ToString().Replace("[x1]", Number) + "','" + "接口编号" + "','" + Number + "预订单撤销（全单）" + "','" + dt_LSGZ.Rows[0]["SJLX"].ToString() + "')";//插入流水的表
                #endregion
                list.Add(sql_LS);

                #region 更新用户余额
                string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+" + dt.Rows[0]["DJDJ"].ToString().Trim() + " WHERE B_DLYX='" + dt.Rows[0]["DLYX"].ToString() + "'";
                #endregion
                list.Add(sql_YE);
                string sqlState = "SELECT * FROM AAA_YDDXXB WHERE Number='" + Number + "' AND ZT='撤销' ";
                if (DbHelperSQL.Query(sqlState).Tables[0].Rows.Count == 0)
                {
                    int i = DbHelperSQL.ExecuteSqlTran(list);
                    if (i > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "预订单撤销成功！";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = Number;
                        try
                        {
                            if (dt.Rows[0]["HTQX"].ToString() == "即时")
                            {

                            }
                            else
                            {
                                DataTable dtJK = new DataTable();
                                dtJK.Columns.Add("商品编号");
                                dtJK.Columns.Add("合同期限");
                                dtJK.Rows.Add(new string[] { dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString() });
                                CJGZ2013 jk = new CJGZ2013();
                                ArrayList al = jk.RunCJGZ(dtJK, "提交");
                                DbHelperSQL.ExecSqlTran(al);
                            }

                        }
                        catch (Exception exJK)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试！";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到该预订单的未撤销信息。";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                }
            }
            else//出现了拆单的情况
            {
                List<string> list = new List<string>();//要执行的SQL语句
                #region 更改预订单信息
                string sql_NGZB = "UPDATE AAA_YDDXXB SET NDGSL=YZBSL,ZT='中标' WHERE Number='" + Number + "'";//把拟订购数量更新为与中标数量相同的量，把状态改为中标
                #endregion
                list.Add(sql_NGZB);

                #region 生成流水信息
                string sql_LSDZ = "SELECT * FROM AAA_moneyDZB WHERE Number='1304000019'";//资金对照表中的撤销预订单

                DataTable dt_LSGZ = DbHelperSQL.Query(sql_LSDZ).Tables[0];
                string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES('" + PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "") + "','" + dt.Rows[0]["DLYX"].ToString() + "','" + dt.Rows[0]["JSZHLX"].ToString() + "','" + dt.Rows[0]["MJJSBH"].ToString() + "','" + "AAA_YDDXXB" + "','" + Number + "','" + DateTime.Now.ToString() + "','" + dt_LSGZ.Rows[0]["YSLX"].ToString() + "','" + dt.Rows[0]["DJDJ"].ToString() + "','" + dt_LSGZ.Rows[0]["XM"].ToString() + "','" + dt_LSGZ.Rows[0]["XZ"].ToString() + "','" + dt_LSGZ.Rows[0]["ZY"].ToString().Replace("[x1]", Number) + "','" + "接口编号" + "','" + Number + "预订单撤销，原拟购量：" + dt.Rows[0]["NDGSL"].ToString() + "','" + dt_LSGZ.Rows[0]["SJLX"].ToString() + "')";//插入流水的表
                #endregion
                list.Add(sql_LS);

                #region 更新用户余额
                string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+" + dt.Rows[0]["DJDJ"].ToString().Trim() + " WHERE B_DLYX='" + dt.Rows[0]["DLYX"].ToString() + "'";
                #endregion
                list.Add(sql_YE);
                string sqlState = "SELECT * FROM AAA_YDDXXB WHERE Number='" + Number + "' AND ZT='中标' ";
                if (DbHelperSQL.Query(sqlState).Tables[0].Rows.Count == 0)
                {
                    int i = DbHelperSQL.ExecuteSqlTran(list);
                    if (i > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "预订单撤销成功！";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = Number;
                        try
                        {
                            if (dt.Rows[0]["HTQX"].ToString() == "即时")
                            {

                            }
                            else
                            {

                                DataTable dtJK = new DataTable();
                                dtJK.Columns.Add("商品编号");
                                dtJK.Columns.Add("合同期限");
                                dtJK.Rows.Add(new string[] { dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString() });
                                CJGZ2013 jk = new CJGZ2013();
                                ArrayList al = jk.RunCJGZ(dtJK, "提交");
                                DbHelperSQL.ExecSqlTran(al);
                            }
                        }
                        catch (Exception exJK)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试！";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到该预订单的未撤销信息。";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                }
            }

        }


        return dsreturn;
    }

    /// <summary>
    /// 修改预订单（本方法主要用于返回撤销后的订单信息，以及已撤销的订单的商品的最新信息）
    /// </summary>
    /// <param name="dsreturn">要返回的DataSet</param>
    /// <param name="Number">已经撤销的预订单号</param>
    /// <returns></returns>
    public DataSet Ydd_Change(DataSet dsreturn, string Number)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到对应的预订单信息！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";

        string sql_SPSFYX = "SELECT SFYX FROM AAA_PTSPXXB WHERE SPBH=(SELECT SPBH FROM AAA_YDDXXB WHERE NUMBER='" + Number + "')";
        string spsfyx = DbHelperSQL.GetSingle(sql_SPSFYX).ToString();//商品是否有效
        if (spsfyx != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该预订单的商品已失效，无法再下达此商品的预订单！";
            return dsreturn;
        }

        try
        {
            string sql = "SELECT *,(SELECT MJDJBL FROM AAA_PTDTCSSDB) as MJDJBL,CAST((NMRJG*(NDGSL)) AS NUMERIC(18,2)) AS NEWNDGJE,CAST((NMRJG*(NDGSL)*(SELECT MJDJBL FROM AAA_PTDTCSSDB)) AS NUMERIC(18,2)) AS NEWDJDJ FROM AAA_YDDXXB LEFT JOIN (SELECT AAA_PTSPXXB.Number, AAA_PTSPXXB.SPBH, AAA_PTSPXXB.SPMC, AAA_PTSPXXB.GG, AAA_PTSPXXB.JJDW, AAA_PTSPXXB.JJPL, ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND (SELECT HTQX FROM AAA_YDDXXB WHERE Number='" + Number + "')=AAA_TBD.HTQX ORDER BY TBJG ASC),0) AS ZDJG, ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND (SELECT HTQX FROM AAA_YDDXXB WHERE Number='" + Number + "')=AAA_TBD.HTQX ORDER BY MJSDJJPL DESC) ,0) AS MJJJPL,AAA_PTSPXXB.SCZZYQ  FROM AAA_PTSPXXB  WHERE SFYX='是' ) AS AAA_SP ON AAA_YDDXXB.SPBH=AAA_SP.SPBH WHERE AAA_YDDXXB.Number='" + Number + "'";

            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            DataTable dtRt = dt.Copy();
            dtRt.TableName = "SPXX";
            dsreturn.Tables.Add(dtRt);
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
        }
        return dsreturn;
    }

    /// <summary>
    /// 获得预订单草稿信息获得
    /// </summary>
    /// <param name="dsreturn">要返回的DataSet</param>
    /// <param name="Number">已经预订单草稿单号</param>
    /// <returns></returns>
    public DataSet YDD_Getcaogao(DataSet dsreturn, string Number)
    {
        Hashtable htt = PublicClass2013.GetParameterInfo();
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到对应的预订单信息！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";

        string sql_SPSFYX = "SELECT SFYX FROM AAA_PTSPXXB WHERE SPBH=(SELECT SPBH FROM AAA_YDDCGB WHERE NUMBER='" + Number + "')";
        object obj = DbHelperSQL.GetSingle(sql_SPSFYX);
        if (obj != null)
        {
            string spsfyx = DbHelperSQL.GetSingle(sql_SPSFYX).ToString();//商品是否有效
            if (spsfyx != "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该预订单草稿的商品已失效，无法再下达此商品的预订单！";
                return dsreturn;
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无法查到该草稿商品的信息，无法再下达此商品的预订单！";
            return dsreturn;
        }

        try
        {
            string sql = "SELECT a.Number,DLYX,a.SPBH,HTQX,SHQY,SHQYsheng,SHQYshi,NMRJG,NDGSL,NDGJE,b.SPMC,b.GG,'0.00' as NEWDJDJ,b.JJDW,(ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=a.SPBH and AAA_TBD.HTQX=a.HTQX  AND ZT='竞标' ORDER BY TBJG ASC),0)) as ZDJG,( select JJPL from AAA_PTSPXXB where SPBH=a.SPBH )as JJPL,(SELECT TOP 1 SCZZYQ FROM AAA_TBD WHERE AAA_TBD.SPBH=a.SPBH ) as SCZZYQ,(ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=a.SPBH AND ZT='竞标' and AAA_TBD.HTQX = a.HTQX ORDER BY MJSDJJPL DESC) ,0)) as MJJJPL,YXJZRQ   FROM AAA_YDDCGB as a left join AAA_PTSPXXB as b on a.SPBH=b.SPBH  WHERE a.Number ='" + Number + "'";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds != null && ds.Tables[0].Rows.Count ==1)
            {
                DataTable dt = ds.Tables[0];
                DataTable dtRt = dt.Copy();
                dtRt.Rows[0]["NEWDJDJ"] = (Convert.ToDouble(dtRt.Rows[0]["NDGJE"].ToString().Trim()) * Convert.ToDouble(htt["买家订金比率"].ToString().Trim())).ToString("#0.00");
                dtRt.TableName = "SPXX";
                dsreturn.Tables.Add(dtRt);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "单号为：" + Number + "的预订单草稿信息查找错误！";
                return dsreturn;
            }
            
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
        }
        return dsreturn;
    }



}