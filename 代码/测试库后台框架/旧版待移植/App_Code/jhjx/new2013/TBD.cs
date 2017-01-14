using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Collections;
using FMOP.DB;

/// <summary>
/// 投标单
/// </summary>
public class TBD
{
	public TBD()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 申请出售商品
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public DataSet TBD_SQCSSP(DataSet ds, DataSet dsreturn)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "初始化错误信息";

        Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
        if (ds == null)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递过来的数据集为空";
            return dsreturn;
        }


        Hashtable htUserVal = PublicClass2013.GetUserInfo(ds.Tables[0].Rows[0]["mjjsbh"].ToString());//通过卖家角色编号获取信息
        if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("投标单") >= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString().Trim();
            return dsreturn;
        }
        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }

        try
        {
            DataTable dt = ds.Tables[0];
            string dlyx = dt.Rows[0]["dlyx"].ToString();
            string jszhlx = dt.Rows[0]["jszhlx"].ToString();
            string mjjsbh = dt.Rows[0]["mjjsbh"].ToString();
            string spbh = dt.Rows[0]["spbh"].ToString();
            string SFBGZZHBZ = "否";
            DateTime SQSJ = DateTime.Now;
            string SHZT = "未审核";
            //string YBNSRZGZM = dt.Rows[0]["YBNSRZGZM"].ToString();//一般纳税人资格证
            string ZLBZYZM = dt.Rows[0]["ZLBZYZM"].ToString();
            string CPJCBG = dt.Rows[0]["CPJCBG"].ToString();
            string PGZFZRFLCNS = dt.Rows[0]["PGZFZRFLCNS"].ToString();
            string FDDBRCNS = dt.Rows[0]["FDDBRCNS"].ToString();
            string SHFWGDYCN = dt.Rows[0]["SHFWGDYCN"].ToString();
            string CPSJSQS = dt.Rows[0]["CPSJSQS"].ToString();
            string zz01 = dt.Rows[0]["zz01"].ToString();
            string zz02 = dt.Rows[0]["zz02"].ToString();
            string zz03 = dt.Rows[0]["zz03"].ToString();
            string zz04 = dt.Rows[0]["zz04"].ToString();
            string zz05 = dt.Rows[0]["zz05"].ToString();
            string zz06 = dt.Rows[0]["zz06"].ToString();
            string zz07 = dt.Rows[0]["zz07"].ToString();
            string zz08 = dt.Rows[0]["zz08"].ToString();
            string zz09 = dt.Rows[0]["zz09"].ToString();
            string zz10 = dt.Rows[0]["zz10"].ToString();
            string Number = PublicClass2013.GetNextNumberZZ("AAA_CSSPB", "");
            string sqlVals = "SELECT TOP 1 * FROM AAA_CSSPB WHERE SPBH ='" + spbh + "' AND DLYX='" + dlyx + "' ORDER BY CREATETIME DESC";
            DataTable dtVal = DbHelperSQL.Query(sqlVals).Tables[0];
            if (dtVal.Rows.Count > 0)
            {
                if (dtVal.Rows[0]["SFBGZZHBZ"].ToString().Trim() == "否")
                {
                    if (dtVal.Rows[0]["SHZT"].ToString().Trim() == "未审核")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已提交本商品审核，当前状态：审核中，不允许重复提交。";
                        return dsreturn;
                    }
                    if (dtVal.Rows[0]["SHZT"].ToString().Trim() == "审核通过")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已经可以出售此商品，并且资质要求未发生变动，请勿重复提交申请。";
                        return dsreturn;
                    }
                }
            }

            if (!PublicClass2013.isYX(spbh))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您拟出售商品刚刚已下线，该申请单无法提交。请选择其他可拟出售商品。";
                return dsreturn;
            }
            string sql = "INSERT INTO AAA_CSSPB(Number, DLYX, JSZHLX, MJJSBH, SPBH, SFBGZZHBZ, SQSJ, SHZT, SHR, SHYJ, ZLBZYZM, CPJCBG, PGZFZRFLCNS, FDDBRCNS, SHFWGDYCN, CPSJSQS, ZZ1, ZZ2, ZZ3, ZZ4, ZZ5, ZZ6, ZZ7, ZZ8, ZZ9, ZZ10) VALUES('" + Number + "','" + dlyx + "','" + jszhlx + "','" + mjjsbh + "','" + spbh + "','" + SFBGZZHBZ + "','" + SQSJ + "','" + SHZT + "','" + " " + "','" + " " + "','" + ZLBZYZM + "','" + CPJCBG + "','" + PGZFZRFLCNS + "','" + FDDBRCNS + "','" + SHFWGDYCN + "','" + CPSJSQS + "','" + zz01 + "','" + zz02 + "','" + zz03 + "','" + zz04 + "','" + zz05 + "','" + zz06 + "','" + zz07 + "','" + zz08 + "','" + zz09 + "','" + zz10 + "')";
            //string UpdateNSR = "UPDATE AAA_DLZHXXB SET I_YBNSRZGZSMJ='" + YBNSRZGZM + "' WHERE B_DLYX='" + dlyx + "'";
            List<string> list = new List<string>();
            list.Add(sql);
            //list.Add(UpdateNSR);

            int i = DbHelperSQL.ExecuteSqlTran(list);
            if (i > 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "提交申请成功！";
                //dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = YBNSRZGZM;//一般纳税人资格证
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "";
                return dsreturn;
            }
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }
        return dsreturn;
    }

    public DataSet TBD_SQCSSP_XG(string Number, DataSet dsreturn)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "初始化错误信息";
        //string SQL = ""
        return dsreturn;
    }

    /// <summary>
    /// 当前用户是否有可出售商品
    /// </summary>
    /// <param name="dlyx"></param>
    /// <returns></returns>
    public bool TBD_IsHasSthToSell(string dlyx)
    {
        bool b = true;
        string sql = "SELECT * FROM AAA_CSSPB WHERE DLYX='" + dlyx.Trim() + "' AND SFBGZZHBZ='否' AND SHZT='审核通过'";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        if (!(dt.Rows.Count > 0))
            b = false;
        return b;
    }

	/// <summary>
	/// 发布投标单
	/// </summary>
	/// <param name="ds"></param>
	/// <param name="dsreturn"></param>
	/// <returns></returns>
	public DataSet SetTBD(DataSet ds, DataSet dsreturn)
    {
        #region 投标单主体
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";

        DataTable dt = ds.Tables["TBD"];
        bool valFlag = false;
        string info = string.Empty;
        dt = InitValidater.ValTBD(dt, ref valFlag, ref info);

        //DataTable dtZZ = ds.Tables["ZZ"];//资质表
        string htqx_new = dt.Rows[0]["HTQX"].ToString(); //2013.0.26 wyh添加即时类型
        ClassMoney2013 AboutMoney = new ClassMoney2013();//与钱相关的类
        Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
        Hashtable htUserVal = PublicClass2013.GetUserInfo(dt.Rows[0]["MJJSBH"].ToString());//通过卖家角色编号获取信息
        Hashtable ht_Sql = new Hashtable();
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
            return dsreturn;
        }

 
        if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("投标单") >= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString().Trim();
            return dsreturn;
        }
        if (htUserVal["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        if (htUserVal["是否被默认经纪人暂停新业务"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人已暂停您的新业务，请联系您的经纪人或向平台申诉。";
            return dsreturn;
        }
 

        //判断商品是否在冷静期内
        string jssj = "";
        if (!htqx_new.Equals("即时")) // 2013.6.25 wyh add 即时区分
        {
            if (PublicClass2013.isLJQ(dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString(), ref jssj))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                DateTime times = Convert.ToDateTime(jssj);
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您所投标的商品正处于冷静期，该投标单无法提交。请于" + times.Year + "年" + times.Month + "月" + times.Day + "日后重新提交。";
                return dsreturn;
            }
        }

        string sql_isFB = "SELECT * FROM AAA_TBD WHERE DLYX='" + htUserVal["登录邮箱"].ToString() + "' AND HTQX='" + dt.Rows[0]["HTQX"].ToString() + "' AND ZT='竞标' AND SPBH='" + dt.Rows[0]["SPBH"].ToString() + "'";//同一商品、同一卖家、同一合同期限、且在竞标中的验证
        DataTable isHad = DbHelperSQL.Query(sql_isFB).Tables[0];
        if (isHad.Rows.Count > 0 && PublicClass2013.isLJQ(dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString(), ref jssj))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该商品您已有投标，此投标已进入冷静期！";
            return dsreturn;
        }
        if (isHad.Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该商品您已有投标，您可以撤销或修改原投标单！";
            return dsreturn;
        }

        //判断商品是否有效
        if (!PublicClass2013.isYX(dt.Rows[0]["SPBH"].ToString()))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您拟出售商品刚刚已下线，该申请单无法提交。请选择其他可拟出售商品。";
            return dsreturn;
        }
        //if (!isUserCanSell(dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["DLYX"].ToString()))
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您所投标的商品所需资质已发生变更，该投标单无法提交。请重新申请该商品的出售资格或选择其他可出售商品。";
        //    return dsreturn;
        //}

        if (!valFlag)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = info;
            return dsreturn;
        }


        //验证余额
        double DJ = Convert.ToDouble(dt.Rows[0]["DJTBBZJ"]);//要冻结的投标保证金
        double YE = AboutMoney.GetMoneyT(dt.Rows[0]["DLYX"].ToString(), "");//当前余额
        if (DJ > YE)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您交易账户中的可用资金余额为" + YE.ToString("0.00") + "元，与该投标单的投标保证金差额为" + (Math.Round((DJ - YE), 2)).ToString("0.00") + "元。您可增加账户的可用资金余额或修改投标单。";
            return dsreturn;
        }
        else if (dt.Rows[0]["FLAG"].ToString().Trim() == "预执行")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "同时确认，该《投标单》一经提交，即依规冻结" + (Math.Round(DJ, 2)).ToString("0.00") + "元的投标保证金。";
            return dsreturn;
        }
        Hashtable JJRInfo = GetUserJJRInfo(dt.Rows[0]["DLYX"].ToString());
        if (JJRInfo != null)
        {
            if (JJRInfo["info"].ToString() == "error")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "关联经纪人信息异常。";
                return dsreturn;
            }
            if (JJRInfo["info"].ToString() == "ok")
            {
                //ht[info](ok/error) | ht[content](错误/正确描述) | ht[关联经纪人邮箱] | ht[关联经纪人用户名] | ht[关联经纪人角色编号] | ht[关联经纪人平台管理机构]
                dt.Rows[0]["GLJJRYX"] = JJRInfo["关联经纪人邮箱"];
                dt.Rows[0]["GLJJRYHM"] = JJRInfo["关联经纪人用户名"];
                dt.Rows[0]["GLJJRJSBH"] = JJRInfo["关联经纪人角色编号"];
                dt.Rows[0]["GLJJRPTGLJG"] = JJRInfo["关联经纪人平台管理机构"];
            }
        }
        #endregion
        string Time = DateTime.Now.ToString();//本单统一的时间
        #region 投标单SQL

        Hashtable ht_ZZ = new Hashtable(); //dt.Rows[0]["其他资质"] == null ? null : (Hashtable)dt.Rows[0]["其他资质"];
        //if (dtZZ != null)
        //{
        //    for (int i = 0; i < dtZZ.Columns.Count; i++)
        //    {
        //        ht_ZZ[dtZZ.Columns[i].ColumnName] = dtZZ.Rows[0][dtZZ.Columns[i].ColumnName].ToString();
        //    }
        //}
        string Number_TBD = PublicClass2013.GetNextNumberZZ("AAA_TBD", "");
        string sql_TBD = "INSERT INTO AAA_TBD(Number,DLYX,JSZHLX,MJJSBH,GLJJRYX,GLJJRYHM,GLJJRJSBH,GLJJRPTGLJG,SPBH,SPMC,GG,JJDW,PTSDZDJJPL,HTQX,MJSDJJPL,GHQY,TBNSL,TBJG,TBJE,MJTBBZJBL,MJTBBZJZXZ,DJTBBZJ,ZT,TJSJ,ZLBZYZM,CPJCBG,PGZFZRFLCNS,FDDBRCNS,SHFWGDYCN,CPSJSQS,SLZM,ZZ01,ZZ02,ZZ03,ZZ04,ZZ05,ZZ06,ZZ07,ZZ08,ZZ09,ZZ10,CreateUser,CreateTime,YZBSL,SPCD) VALUES(@Number,@DLYX,@JSZHLX,@MJJSBH,@GLJJRYX,@GLJJRYHM,@GLJJRJSBH,@GLJJRPTGLJG,@SPBH,@SPMC,@GG,@JJDW,@PTSDZDJJPL,@HTQX,@MJSDJJPL,@GHQY,@TBNSL,@TBJG,@TBJE,@MJTBBZJBL,@MJTBBZJZXZ,@DJTBBZJ,@ZT,@TJSJ,@ZLBZYZM,@CPJCBG,@PGZFZRFLCNS,@FDDBRCNS,@SHFWGDYCN,@CPSJSQS,@SLZM,@ZZ01,@ZZ02,@ZZ03,@ZZ04,@ZZ05,@ZZ06,@ZZ07,@ZZ08,@ZZ09,@ZZ10,@CreateUser,@CreateTime,0,@SPCD)";
        
        SqlParameter[] sp_TBD = { 
									new SqlParameter("@Number",Number_TBD),
									new SqlParameter("@DLYX",dt.Rows[0]["DLYX"].ToString()),
									new SqlParameter("@JSZHLX",dt.Rows[0]["JSZHLX"].ToString()),
									new SqlParameter("@MJJSBH",dt.Rows[0]["MJJSBH"].ToString()),
									new SqlParameter("@GLJJRYX",dt.Rows[0]["GLJJRYX"].ToString()),
									new SqlParameter("@GLJJRYHM",dt.Rows[0]["GLJJRYHM"].ToString()),
									new SqlParameter("@GLJJRJSBH",dt.Rows[0]["GLJJRJSBH"].ToString()),
									new SqlParameter("@GLJJRPTGLJG",dt.Rows[0]["GLJJRPTGLJG"].ToString()),
									new SqlParameter("@SPBH",dt.Rows[0]["SPBH"].ToString()),
									new SqlParameter("@SPMC",dt.Rows[0]["SPMC"].ToString()),
									new SqlParameter("@GG",dt.Rows[0]["GG"].ToString()),
									new SqlParameter("@JJDW",dt.Rows[0]["JJDW"].ToString()),
									new SqlParameter("@PTSDZDJJPL",dt.Rows[0]["PTSDZDJJPL"].ToString()),
									new SqlParameter("@HTQX",dt.Rows[0]["HTQX"].ToString()),
									new SqlParameter("@MJSDJJPL",dt.Rows[0]["MJSDJJPL"].ToString()),
									new SqlParameter("@GHQY",dt.Rows[0]["GHQY"].ToString()),
									new SqlParameter("@TBNSL",dt.Rows[0]["TBNSL"].ToString()),
									new SqlParameter("@TBJG",dt.Rows[0]["TBJG"].ToString()),
									new SqlParameter("@TBJE",dt.Rows[0]["TBJE"].ToString()),
									new SqlParameter("@MJTBBZJBL",dt.Rows[0]["MJTBBZJBL"].ToString()),
									new SqlParameter("@MJTBBZJZXZ",dt.Rows[0]["MJTBBZJZXZ"].ToString()),
									new SqlParameter("@DJTBBZJ",dt.Rows[0]["DJTBBZJ"].ToString()),
									new SqlParameter("@ZT","竞标"),
									new SqlParameter("@TJSJ",Time),
									new SqlParameter("@ZLBZYZM",dt.Rows[0]["ZLBZYZM"].ToString()),
									new SqlParameter("@CPJCBG",dt.Rows[0]["CPJCBG"].ToString()),
									new SqlParameter("@PGZFZRFLCNS",dt.Rows[0]["PGZFZRFLCNS"].ToString()),
									new SqlParameter("@FDDBRCNS",dt.Rows[0]["FDDBRCNS"].ToString()),
									new SqlParameter("@SHFWGDYCN",dt.Rows[0]["SHFWGDYCN"].ToString()),
									new SqlParameter("@CPSJSQS",dt.Rows[0]["CPSJSQS"].ToString()),
									new SqlParameter("@CreateUser",dt.Rows[0]["DLYX"].ToString()),
									new SqlParameter("@CreateTime",Time),
                                    new SqlParameter("@SLZM",dt.Rows[0]["SLZM"].ToString()),
                                    new SqlParameter("@ZZ01",dt.Rows[0]["其他资质"].ToString()),
                                    new SqlParameter("@ZZ02",ht_ZZ["资质2"] == null?"":ht_ZZ["资质2"].ToString()),
                                    new SqlParameter("@ZZ03",ht_ZZ["资质3"] == null?"":ht_ZZ["资质3"].ToString()),
                                    new SqlParameter("@ZZ04",ht_ZZ["资质4"] == null?"":ht_ZZ["资质4"].ToString()),
                                    new SqlParameter("@ZZ05",ht_ZZ["资质5"] == null?"":ht_ZZ["资质5"].ToString()),
                                    new SqlParameter("@ZZ06",ht_ZZ["资质6"] == null?"":ht_ZZ["资质6"].ToString()),
                                    new SqlParameter("@ZZ07",ht_ZZ["资质7"] == null?"":ht_ZZ["资质7"].ToString()),
                                    new SqlParameter("@ZZ08",ht_ZZ["资质8"] == null?"":ht_ZZ["资质8"].ToString()),
                                    new SqlParameter("@ZZ09",ht_ZZ["资质9"] == null?"":ht_ZZ["资质9"].ToString()),
                                    new SqlParameter("@ZZ10",ht_ZZ["资质10"] == null?"":ht_ZZ["资质10"].ToString()),
                                    new SqlParameter("@YZBSL",0),
                                    new SqlParameter("@SPCD",dt.Rows[0]["省市区"].ToString())
								};
        #endregion
        ht_Sql.Add(sql_TBD, sp_TBD);
        #region 账款流水明细表
        DataTable dt_LS = FMOP.DB.DbHelperSQL.Query("SELECT * FROM AAA_moneyDZB WHERE NUMBER='1304000003'").Tables[0];

        string Number_LS = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
        string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,SJLX,CreateUser,CreateTime) VALUES(@Number,@DLYX,@JSZHLX,@JSBH,@LYYWLX,@LYDH,@LSCSSJ,@YSLX,@JE,@XM,@XZ,@ZY,@JKBH,@SJLX,@CreateUser,@CreateTime)";
        SqlParameter[] sp_LS = {
                                 new SqlParameter("@Number",Number_LS),
                                 new SqlParameter("@DLYX",htUserVal["登录邮箱"].ToString()),
                                 new SqlParameter("@JSZHLX",htUserVal["结算账户类型"].ToString()),
                                 new SqlParameter("@JSBH",htUserVal["卖家角色编号"].ToString()),
                                 new SqlParameter("@LYYWLX","AAA_TBD"),
                                 new SqlParameter("@LYDH",Number_TBD),
                                 new SqlParameter("@LSCSSJ",DateTime.Now),
                                 new SqlParameter("@YSLX",dt_LS.Rows[0]["YSLX"].ToString().Trim()),
                                 new SqlParameter("@JE",DJ),
                                 new SqlParameter("@XM",dt_LS.Rows[0]["XM"].ToString().Trim()),
                                 new SqlParameter("@XZ",dt_LS.Rows[0]["XZ"].ToString().Trim()),
                                 new SqlParameter("@ZY",dt_LS.Rows[0]["ZY"].ToString().Trim().Replace("[x1]",Number_TBD)),
                                 new SqlParameter("@JKBH","接口编号"),
                                 new SqlParameter("@SJLX",dt_LS.Rows[0]["SJLX"].ToString().Trim()),
                                 new SqlParameter("@CreateUser",htUserVal["登录邮箱"].ToString()),
                                 new SqlParameter("@CreateTime",Time)
                              };
        #endregion
        ht_Sql.Add(sql_LS, sp_LS);
        #region 更新用户余额
        string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE-" + DJ + " WHERE B_DLYX=@B_DLYX";
        SqlParameter[] sp_YE = {
                                   new SqlParameter("@B_DLYX",htUserVal["登录邮箱"].ToString())
                               };
        #endregion
        ht_Sql.Add(sql_YE, sp_YE);

        try
        {
            #region 再次验证余额，然后执行SQL
            //验证余额
            double DJ_Again = Convert.ToDouble(dt.Rows[0]["DJTBBZJ"]);//要冻结的订金
            double YE_Again = AboutMoney.GetMoneyT(dt.Rows[0]["DLYX"].ToString(), "");//当前余额
            if (DJ_Again > YE_Again)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您交易账户中的可用资金余额为" + YE + "元，与该投标单的订金差额为" + Math.Round((DJ - YE), 2) + "元。您可增加账户的可用资金余额或修改投标单。";
                return dsreturn;
            }
            else if (dt.Rows[0]["FLAG"].ToString().Trim() == "预执行")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的投标单一经提交将冻结" + DJ + "元的订金。";
                return dsreturn;
            }
            #endregion
            if (FMOP.DB.DbHelperSQL.ExecuteSqlTran(ht_Sql))
            {
                try
                {
                    if (!htqx_new.Equals("即时")) // 2013.6.25 wyh add 即时区分
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
                    //监控异常信息开始
                    string jkMsg = exJK.Message;
                    //监控异常信息结束
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "本次投标单提交成功！";
                    return dsreturn;
                }
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "本次投标单提交成功！";
                return dsreturn;
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试！";
                return dsreturn;
            }
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试！";
            return dsreturn;
        }
        #endregion
        

	}

    /// <summary>
    /// 撤销投标单
    /// </summary>
    /// <param name="Number"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public DataSet TBD_CX(string Number, DataSet dsreturn)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到对应的投标单信息！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "true";
        string sql = "SELECT * FROM AAA_TBD WHERE Number='" + Number + "'";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        if (dt.Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息5"] = (Convert.ToInt32(dt.Rows[0]["TBNSL"].ToString().Trim()) - Convert.ToInt32(dt.Rows[0]["YZBSL"].ToString().Trim())).ToString().Trim();
            string isInLJQ = DbHelperSQL.GetSingle("SELECT isnull(SFJRLJQ,'') FROM AAA_LJQDQZTXXB WHERE SPBH='" + dt.Rows[0]["SPBH"].ToString() + "' AND HTQX='" + dt.Rows[0]["HTQX"].ToString() + "'") == null ? "" : DbHelperSQL.GetSingle("SELECT isnull(SFJRLJQ,'') FROM AAA_LJQDQZTXXB WHERE SPBH='" + dt.Rows[0]["SPBH"].ToString() + "' AND HTQX='" + dt.Rows[0]["HTQX"].ToString() + "'").ToString();//是否在冷静期
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
 
            if (htUserVal["冻结功能项"].ToString().Trim().IndexOf("投标单") >= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + htUserVal["冻结功能项"].ToString().Trim();
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
            if (isInLJQ == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该商品已进入冷静期，无法撤销投标单！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
            if (dt.Rows[0]["ZT"].ToString() == "中标")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的投标单已中标，无法撤销！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
            if (dt.Rows[0]["ZT"].ToString() == "撤销")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的投标单已撤销，无法重复撤销！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
            #endregion

            //if (!isUserCanSell(dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["DLYX"].ToString()))
            //    dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "false";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "true";
            List<string> list = new List<string>();//要执行的SQL
            if (dt.Rows[0]["YZBSL"].ToString() == "0" || dt.Rows[0]["YZBSL"] == DBNull.Value)
            {
                #region 更改投标单状态
                string sql_TBD = "UPDATE AAA_TBD SET ZT='撤销' WHERE Number='" + Number + "'";//更改单据状态为撤销
                #endregion
                list.Add(sql_TBD);//更新投标单状态

                #region 生成流水信息
                string sql_LSDZ = "SELECT * FROM AAA_moneyDZB WHERE Number='1304000004'";//撤销投标单解冻对照
                DataTable dt_LSDZ = DbHelperSQL.Query(sql_LSDZ).Tables[0];

                string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES('" + PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "") + "','" + dt.Rows[0]["DLYX"].ToString() + "','" + dt.Rows[0]["JSZHLX"].ToString() + "','" + dt.Rows[0]["MJJSBH"].ToString() + "','" + "AAA_TBD" + "','" + Number + "','" + DateTime.Now.ToString() + "','" + dt_LSDZ.Rows[0]["YSLX"].ToString() + "','" + dt.Rows[0]["DJTBBZJ"].ToString() + "','" + dt_LSDZ.Rows[0]["XM"].ToString() + "','" + dt_LSDZ.Rows[0]["XZ"].ToString() + "','" + dt_LSDZ.Rows[0]["ZY"].ToString().Replace("[x1]", Number) + "','" + "接口编号" + "','" + Number + "投标单撤销（全单）" + "','" + dt_LSDZ.Rows[0]["SJLX"].ToString() + "')";//插入流水表

                #endregion
                list.Add(sql_LS);//插入流水表

                #region 更新用户余额
                string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+" + dt.Rows[0]["DJTBBZJ"].ToString().Trim() + " WHERE B_DLYX='" + dt.Rows[0]["DLYX"].ToString() + "'";
                #endregion
                list.Add(sql_YE);//插入余额表
                string sqlState = "SELECT * FROM AAA_TBD WHERE Number='" + Number + "' AND ZT='撤销' ";
                if (DbHelperSQL.Query(sqlState).Tables[0].Rows.Count == 0)
                {
                    int i = DbHelperSQL.ExecuteSqlTran(list);
                    if (i > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "投标单撤销成功！";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = Number;
                        try
                        {
                            DataTable dtJK = new DataTable();
                            dtJK.Columns.Add("商品编号");
                            dtJK.Columns.Add("合同期限");
                            dtJK.Rows.Add(new string[] { dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString() });
                            CJGZ2013 jk = new CJGZ2013();
                            ArrayList al = jk.RunCJGZ(dtJK, "提交");
                            DbHelperSQL.ExecSqlTran(al);
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
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到对应的投标单信息！";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "true";
                }
            }
            else
            {
                #region 更改投标单状态
                string sql_TBD = "UPDATE AAA_TBD SET ZT='中标',TBNSL=YZBSL WHERE Number='" + Number + "'";//更改单据状态为撤销
                #endregion
                list.Add(sql_TBD);//更新投标单状态

                #region 生成流水信息
                string sql_LSDZ = "SELECT * FROM AAA_moneyDZB WHERE Number='1304000004'";//撤销投标单解冻对照
                DataTable dt_LSDZ = DbHelperSQL.Query(sql_LSDZ).Tables[0];

                string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES('" + PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "") + "','" + dt.Rows[0]["DLYX"].ToString() + "','" + dt.Rows[0]["JSZHLX"].ToString() + "','" + dt.Rows[0]["MJJSBH"].ToString() + "','" + "AAA_TBD" + "','" + Number + "','" + DateTime.Now.ToString() + "','" + dt_LSDZ.Rows[0]["YSLX"].ToString() + "','" + dt.Rows[0]["DJTBBZJ"].ToString() + "','" + dt_LSDZ.Rows[0]["XM"].ToString() + "','" + dt_LSDZ.Rows[0]["XZ"].ToString() + "','" + dt_LSDZ.Rows[0]["ZY"].ToString().Replace("[x1]", Number) + "','" + "接口编号" + "','" + Number + "投标单撤销（拆单）" + "','" + dt_LSDZ.Rows[0]["SJLX"].ToString() + "')";//插入流水表

                #endregion
                list.Add(sql_LS);//插入流水表

                #region 更新用户余额
                string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+" + dt.Rows[0]["DJTBBZJ"].ToString().Trim() + " WHERE B_DLYX='" + dt.Rows[0]["DLYX"].ToString() + "'";
                #endregion
                list.Add(sql_YE);//插入余额表
                 string sqlState = "SELECT * FROM AAA_TBD WHERE Number='" + Number + "' AND ZT='中标' ";
                 if (DbHelperSQL.Query(sqlState).Tables[0].Rows.Count == 0)
                 {
                     int i = DbHelperSQL.ExecuteSqlTran(list);
                     if (i > 0)
                     {
                         dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                         dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "投标单撤销成功！";
                         dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = Number;
                         try
                         {
                             DataTable dtJK = new DataTable();
                             dtJK.Columns.Add("商品编号");
                             dtJK.Columns.Add("合同期限");
                             dtJK.Rows.Add(new string[] { dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString() });
                             CJGZ2013 jk = new CJGZ2013();
                             ArrayList al = jk.RunCJGZ(dtJK, "提交");
                             DbHelperSQL.ExecSqlTran(al);
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
                     dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到对应的投标单信息！";
                     dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                     dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "true";
                 }
            }
        }



        return dsreturn;
    }

    /// <summary>
    /// 投标单修改
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="Number"></param>
    /// <returns></returns>
    public DataSet Tbd_Change(DataSet dsreturn, string Number)
    {
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到对应的投标单信息！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";

        string sql_SPSFYX = "SELECT SFYX FROM AAA_PTSPXXB WHERE SPBH=(SELECT SPBH FROM AAA_TBD WHERE NUMBER='" + Number + "')";
        string spsfyx = DbHelperSQL.GetSingle(sql_SPSFYX).ToString();//商品是否有效
        if (spsfyx != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该投标单的商品已失效，无法再下达此商品的投标单！";
            return dsreturn;
        }


        try
        {
            string sql = "SELECT AAA_TBD.Number, AAA_TBD.DLYX, AAA_TBD.JSZHLX,AAA_TBD.SPCD, AAA_TBD.MJJSBH, AAA_TBD.GLJJRYX, AAA_TBD.GLJJRYHM, AAA_TBD.GLJJRJSBH, AAA_TBD.GLJJRPTGLJG, AAA_TBD.SPBH, AAA_TBD.HTQX, AAA_TBD.MJSDJJPL, AAA_TBD.GHQY, (AAA_TBD.TBNSL-AAA_TBD.YZBSL) as TBNSL, AAA_TBD.TBJG, AAA_TBD.TBJE, AAA_TBD.MJTBBZJBL, AAA_TBD.MJTBBZJZXZ, AAA_TBD.DJTBBZJ, AAA_TBD.ZT, AAA_TBD.TJSJ, AAA_TBD.CXSJ, AAA_TBD.ZLBZYZM, AAA_TBD.CPJCBG, AAA_TBD.PGZFZRFLCNS, AAA_TBD.FDDBRCNS, AAA_TBD.SHFWGDYCN, AAA_TBD.SLZM, AAA_TBD.CPSJSQS, AAA_TBD.ZZ01, AAA_TBD.ZZ02, AAA_TBD.ZZ03, AAA_TBD.ZZ04, AAA_TBD.ZZ05, AAA_TBD.ZZ06, AAA_TBD.ZZ07, AAA_TBD.ZZ08, AAA_TBD.ZZ09, AAA_TBD.ZZ10, AAA_PTSPXXB.SPMC, AAA_PTSPXXB.GG, AAA_PTSPXXB.SSFLBH, AAA_PTSPXXB.JJDW, AAA_PTSPXXB.JJPL, AAA_PTSPXXB.SPMS, AAA_PTSPXXB.SCZZYQ, AAA_PTSPXXB.FBRQ, AAA_PTSPXXB.SFYX, AAA_PTSPXXB.ZFRQ, ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标'  AND (SELECT HTQX FROM AAA_TBD WHERE Number='" + Number + "')=AAA_TBD.HTQX  ORDER BY TBJG ASC),0) AS ZDJG, ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标'  AND (SELECT HTQX FROM AAA_TBD WHERE Number='" + Number + "')=AAA_TBD.HTQX  ORDER BY MJSDJJPL DESC) ,0) AS MJJJPL FROM  AAA_TBD INNER JOIN AAA_PTSPXXB ON AAA_TBD.SPBH = AAA_PTSPXXB.SPBH WHERE AAA_PTSPXXB.SFYX='是' AND AAA_TBD.Number='" + Number + "'";


            /*
SELECT   AAA_TBD.Number, AAA_TBD.DLYX, AAA_TBD.JSZHLX, AAA_TBD.MJJSBH, AAA_TBD.GLJJRYX, AAA_TBD.GLJJRYHM, 
                AAA_TBD.GLJJRJSBH, AAA_TBD.GLJJRPTGLJG, AAA_TBD.SPBH, AAA_TBD.HTQX, AAA_TBD.MJSDJJPL, 
                AAA_TBD.GHQY, AAA_TBD.TBNSL, AAA_TBD.TBJG, AAA_TBD.TBJE, AAA_TBD.MJTBBZJBL, AAA_TBD.MJTBBZJZXZ, 
                AAA_TBD.DJTBBZJ, AAA_TBD.ZT, AAA_TBD.TJSJ, AAA_TBD.CXSJ, AAA_TBD.ZLBZYZM, AAA_TBD.CPJCBG, 
                AAA_TBD.PGZFZRFLCNS, AAA_TBD.FDDBRCNS, AAA_TBD.SHFWGDYCN, AAA_TBD.CPSJSQS, AAA_TBD.ZZ01, 
                AAA_TBD.ZZ02, AAA_TBD.ZZ03, AAA_TBD.ZZ04, AAA_TBD.ZZ05, AAA_TBD.ZZ06, AAA_TBD.ZZ07, AAA_TBD.ZZ08, 
                AAA_TBD.ZZ09, AAA_TBD.ZZ10, AAA_PTSPXXB.SPMC, AAA_PTSPXXB.GG, AAA_PTSPXXB.SSFLBH, 
                AAA_PTSPXXB.JJDW, AAA_PTSPXXB.JJPL, AAA_PTSPXXB.SPMS, AAA_PTSPXXB.SCZZYQ, AAA_PTSPXXB.FBRQ, 
                AAA_PTSPXXB.SFYX, AAA_PTSPXXB.ZFRQ
FROM      AAA_TBD INNER JOIN
                AAA_PTSPXXB ON AAA_TBD.SPBH = AAA_PTSPXXB.SPBH   
 */


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
    /// 获得投标单草稿信息获得
    /// </summary>
    /// <param name="dsreturn">要返回的DataSet</param>
    /// <param name="Number">投标单草稿单号</param>
    /// <returns></returns>
    public DataSet TBD_GetCGinfo(DataSet dsreturn, string Number)
    {
        Hashtable htt = PublicClass2013.GetParameterInfo();
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到对应的投标单信息！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";

        string sql_SPSFYX = "SELECT SFYX FROM AAA_PTSPXXB WHERE SPBH=(SELECT SPBH FROM AAA_TBDCGB WHERE NUMBER='" + Number + "')";
        object obj = DbHelperSQL.GetSingle(sql_SPSFYX);
        if (obj != null)
        {
            string spsfyx = DbHelperSQL.GetSingle(sql_SPSFYX).ToString();//商品是否有效
            if (spsfyx != "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该投标单草稿的商品已失效，无法再下达此商品的投标单！";
                return dsreturn;
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无法查到该草稿商品的信息，无法再下达此商品的投标单！";
            return dsreturn;
        }

        try
        {
            string sql = " SELECT AAA_TBDCGB.Number, AAA_TBDCGB.DLYX, AAA_TBDCGB.JSZHLX,AAA_TBDCGB.SPCD, AAA_TBDCGB.MJJSBH, AAA_TBDCGB.GLJJRYX, AAA_TBDCGB.GLJJRYHM, AAA_TBDCGB.GLJJRJSBH, AAA_TBDCGB.GLJJRPTGLJG, AAA_TBDCGB.SPBH, AAA_TBDCGB.HTQX, AAA_TBDCGB.MJSDJJPL, AAA_TBDCGB.GHQY, AAA_TBDCGB.TBNSL, AAA_TBDCGB.TBJG, AAA_TBDCGB.TBJE, AAA_TBDCGB.MJTBBZJBL, AAA_TBDCGB.MJTBBZJZXZ, AAA_TBDCGB.DJTBBZJ, AAA_TBDCGB.ZT, AAA_TBDCGB.TJSJ, AAA_TBDCGB.CXSJ, AAA_TBDCGB.ZLBZYZM, AAA_TBDCGB.CPJCBG, AAA_TBDCGB.PGZFZRFLCNS, AAA_TBDCGB.FDDBRCNS, AAA_TBDCGB.SHFWGDYCN, AAA_TBDCGB.CPSJSQS, AAA_TBDCGB.ZZ01, AAA_TBDCGB.ZZ02, AAA_TBDCGB.ZZ03, AAA_TBDCGB.ZZ04, AAA_TBDCGB.ZZ05, AAA_TBDCGB.ZZ06, AAA_TBDCGB.ZZ07, AAA_TBDCGB.ZZ08, AAA_TBDCGB.ZZ09, AAA_TBDCGB.ZZ10, AAA_TBDCGB.SLZM, AAA_PTSPXXB.SPMC, AAA_PTSPXXB.GG, AAA_PTSPXXB.SSFLBH, AAA_PTSPXXB.JJDW, AAA_PTSPXXB.JJPL, AAA_PTSPXXB.SPMS, AAA_PTSPXXB.SCZZYQ, AAA_PTSPXXB.FBRQ, AAA_PTSPXXB.SFYX, AAA_PTSPXXB.ZFRQ,(ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_TBDCGB.SPBH and AAA_TBD.HTQX=AAA_TBDCGB.HTQX  AND ZT='竞标' ORDER BY TBJG ASC),0)) as ZDJG,( select JJPL from AAA_PTSPXXB where SPBH=AAA_TBDCGB.SPBH )as JJPL,(SELECT TOP 1 SCZZYQ FROM AAA_PTSPXXB WHERE AAA_PTSPXXB.SPBH=AAA_TBDCGB.SPBH ) as SCZZYQ,(ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_TBDCGB.SPBH AND ZT='竞标' and AAA_TBD.HTQX = AAA_TBDCGB.HTQX ORDER BY MJSDJJPL DESC) ,0)) as MJJJPL  FROM AAA_TBDCGB  left join AAA_PTSPXXB  on AAA_TBDCGB.SPBH=AAA_PTSPXXB.SPBH  WHERE AAA_TBDCGB.Number ='" + Number + "'";
            DataSet ds = DbHelperSQL.Query(sql);
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
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "单号为：" + Number + "的投标单草稿信息查找错误！";
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

    /// <summary>
    /// 用户是否可以出售这个商品
    /// </summary>
    /// <param name="spbh"></param>
    /// <param name="dlyx"></param>
    /// <returns></returns>
    public bool isUserCanSell(string spbh, string dlyx)
    {
        bool b = false;
        string sql = "SELECT TOP 1 * FROM AAA_CSSPB WHERE DLYX='" + dlyx + "' AND SPBH='" + spbh + "' ORDER BY CreateTime DESC";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["SFBGZZHBZ"].ToString() == "否" && dt.Rows[0]["SHZT"].ToString() == "审核通过")
            {
                b = true;
            }
        }
        return b;
    }

    /// <summary>
    /// 获取关联经纪人信息
    /// </summary>
    /// <param name="dlyx">本用户的邮箱</param>
    /// <returns>ht[info](ok/error) | ht[content](错误/正确描述) | ht[关联经纪人邮箱] | ht[关联经纪人用户名] | ht[关联经纪人角色编号] | ht[关联经纪人平台管理机构]</returns>
    public Hashtable GetUserJJRInfo(string dlyx)
    {
        Hashtable ht = new Hashtable();
        try
        {
            string sql = "SELECT * FROM AAA_DLZHXXB LEFT JOIN AAA_MJMJJYZHYJJRZHGLB ON AAA_DLZHXXB.B_DLYX=AAA_MJMJJYZHYJJRZHGLB.DLYX WHERE AAA_DLZHXXB.B_DLYX='" + dlyx + "' AND S_SFYBJJRSHTG='是' AND S_SFYBFGSSHTG='是' AND AAA_MJMJJYZHYJJRZHGLB.SFDQMRJJR='是'";
            DataTable dtUser = DbHelperSQL.Query(sql).Tables[0];
            if (dtUser.Rows.Count > 0)
            {
                string sqlJJR = "SELECT * FROM AAA_DLZHXXB WHERE B_DLYX='" + dtUser.Rows[0]["GLJJRDLZH"].ToString() + "'";
                DataTable dtJJR = DbHelperSQL.Query(sqlJJR).Tables[0];
                ht["info"] = "ok";
                ht["content"] = "succeed";
                ht["关联经纪人邮箱"] = dtUser.Rows[0]["GLJJRDLZH"].ToString();
                ht["关联经纪人用户名"] = dtJJR.Rows[0]["B_YHM"].ToString();
                ht["关联经纪人角色编号"] = dtJJR.Rows[0]["J_JJRJSBH"].ToString();
                ht["关联经纪人平台管理机构"] = dtJJR.Rows[0]["I_PTGLJG"].ToString();
            }
            else
            {
                ht["info"] = "error";
                ht["content"] = "未开通结算帐户";
            }
        }
        catch (Exception ex)
        {
            ht["info"] = "error0";
            ht["content"] = "程式错误";
        }
        return ht;
    }
}