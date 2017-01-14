using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using FMOP.DB;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;

/// <summary>
///KGclassYH 的摘要说明
/// </summary>
public class KGclassYH
{
    public KGclassYH()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    #region 获取户或推广的剩余合格旧硒鼓数量
    public DataTable GetTable1(string UserNumber)
    {
        //获取所有空鼓入库的信息（k101）;
        string sql = "select ltrim(rtrim(INVTB.UDF01)) as 客户编号, ltrim(rtrim(INVTB.TB029)) as 空鼓类型, (case ltrim(rtrim(INVTB.TB029)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end) as 空鼓规格,cast(TB007 as int) as 剩余总量 from INVTB left join INVTA on TB002=TA002 and TB001=TA001 and INVTB.TSFM_bsc=INVTA.TSFM_bsc left join  CMSME on TA004=ME001 left join COPMA on INVTB.UDF01=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k101' and ME002 like '%办事处%' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "' and TA006='Y' ";

        DataSet dt1 = GetErpData.GetDataSet(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    public DataTable GetTable2(string UserNumber)
    {
        //获取k102中,来源(单头的UDF01字段)不为用户直通车的部分
        string sql = "select ltrim(rtrim(INVTB.UDF01)) as 客户编号, ltrim(rtrim(INVTB.TB029)) as 空鼓类型, (case ltrim(rtrim(INVTB.TB029)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end) as 空鼓规格,-cast(TB007 as int) as 剩余总量,LTrim(RTrim(TB004)) as 空鼓品号 from INVTB left join INVTA on TB002=TA002 and TB001=TA001 and INVTA.TSFM_bsc=INVTB.TSFM_bsc  left join  CMSME on TA004=ME001 left join COPMA on INVTB.UDF01=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k102' and ME002 like '%办事处%' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "' and TA006='Y' and INVTA.UDF01 not like '%用户直通车%'";
        DataSet ds = GetErpData.GetDataSet(sql);
        if (ds != null && ds.Tables != null)
        {
            return ds.Tables[0];
        }
        return null;
    }


    public DataTable GetTable3(string UserNumber)
    {
        //空鼓使用记录表中记录的使用的数据，现在用户只有循环提货，推广人员只有用户非循环提货时扣减空鼓增加收益的用途
        string sql = "select yhbh as 客户编号,'' as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,cplb as 订单类型,cpsl as 订单数量,cpxh as 订单型号 from FM_YHKGSYJLB where djly='用户直通车' and yhbh='" + UserNumber + "'";
        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }


    public DataTable GetTable4(string UserNumber)
    {
        //新版合格旧硒鼓交易中心数据的计算
      //  string sql = "select MRKHBH as 客户编号,CJLX as 空鼓类型,CJXH as 空鼓规格, CJSL as 剩余总量  from SuccedTrade where MRKHBH='" + UserNumber + "'";

        string sql = "select MCKHBH as 客户编号,CJLX as 空鼓类型,CJXH as 空鼓规格, -CJSL as 剩余总量 from SuccedTrade where MCKHBH='" + UserNumber + "' union all select MRKHBH as 客户编号,CJLX as 空鼓类型,CJXH as 空鼓规格, CJSL as 剩余总量  from SuccedTrade where MRKHBH='" + UserNumber + "'";

        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    public DataTable GetTable4_1(string UserNumber)
    {
        //合格旧硒鼓转让信息发布，空鼓冻结
        string sql = "select MCFKHBH as 客户编号,LX as 空鼓类型,XH as 空鼓规格, -SYKMSL as 剩余总量   from Newtransfer where MCFKHBH='" + UserNumber + "' and JYZT <>'5' and JYZT <>'6' ";
        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    public DataTable GetTable5(string UserNumber)
    {
        //获取k301中来源是富美直通车的数据（单头UDF01为用户直通车），不管审核不审核。
        string sql = "select substring(PURMA.UDF01,0,charindex(',',PURMA.UDF01)) as 客户编号, TG005 as 供应商编号, ltrim(rtrim(TH072)) as 空鼓类型, (case ltrim(rtrim(TH072)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end) as 空鼓规格,-cast(TH007 as int) as 剩余总量,LTrim(RTrim(TH004)) as 空鼓品号 from PURTG left join PURTH on TG001=TH001 and TG002=TH002 and PURTG.TSFM_bsc=PURTH.TSFM_bsc left join  CMSME on TG049=ME001 left join PURMA on TG005=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TH004)) left join KGCPDZB on KGPH=LTrim(RTrim(TH004))  where TG001='k301' and ME002 like '%办事处%' and ltrim(rtrim(substring(PURMA.UDF01,0,charindex(',',PURMA.UDF01))))='" + UserNumber + "' and PURTG.UDF01 like '%用户直通车%'";
        DataSet ds = GetErpData.GetDataSet(sql);
        if (ds != null && ds.Tables != null)
        {
            return ds.Tables[0];
        }
        return null;
    }
    public DataTable GetTable6(string UserNumber)
    {
        //终端用户空鼓调整表数据   ----2012.6.6 郭拓
        string sql_ru = "select khbh as 客户编号,kglb as 空鼓类型,kggg as 空鼓规格,tzsl as 剩余总量 from CWGL_ZDYHKGTZB where tzlx='调入' and  khbh='" + UserNumber + "' and shzt='已审核'";

        string sql_chu = "select khbh as 客户编号,kglb as 空鼓类型,kggg as 空鼓规格,-tzsl as 剩余总量 from CWGL_ZDYHKGTZB where tzlx='调出' and khbh='" + UserNumber + "' and shzt='已审核'";

        string sql = "select 客户编号,空鼓类型,空鼓规格,sum(剩余总量) as 剩余总量 from ( " + sql_ru + " union all " + sql_chu + ") as tab group by 客户编号,空鼓类型,空鼓规格";
        DataSet dt = DbHelperSQL.Query(sql);
        if (dt != null && dt.Tables != null)
        {
            return dt.Tables[0];
        }
        return null;
    }


    /// <summary>
    /// 重新处理数据集，处理类型和剩余总量
    /// </summary>
    /// <param name="dtre">要处理的数据集</param>
    /// <param name="lx_str">要处理的类型</param>
    /// <returns></returns>
    public DataTable revalue(DataTable dtre, string lx_str)
    {
        DataTable dtre_copy = dtre.Copy();//空鼓类型
        for (int y = 0; y < dtre_copy.Rows.Count; y++)
        {
            dtre_copy.Rows[y]["空鼓类型"] = lx_str;
            string jgl = dtre.Rows[y][lx_str + "旧鼓量"].ToString();
            int fujgl = 0;
            if (jgl.Trim() != "")
            {
                fujgl = Convert.ToInt32(jgl);
            }
            dtre_copy.Rows[y]["剩余总量"] = -fujgl;
        }
        return dtre_copy;
    }

    /// <summary>
    /// 合并多个数据表，根据特定规则
    /// </summary>
    /// <param name="dt1"></param>
    /// <param name="dt2"></param>
    /// <param name="dt3"></param>
    /// <param name="dt4"></param>
    /// <returns></returns>
    public DataTable MergeTable(DataTable dt1, DataTable dt2, DataTable dt3, DataTable dt4,DataTable dt4_1, DataTable dt5, DataTable dt6, DataTable dt7,DataTable dt8)
    {
        DataTable dtmt = dt1.Clone();
        dtmt.Merge(dt1, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt2, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt3, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt4, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt4_1, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt5, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt6, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt7, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt8, false, MissingSchemaAction.Ignore);
        return dtmt;
    }

    /// <summary>
    /// 获取某客户所有可以使用的空鼓数量,列： 型号，类型，型号，数量
    /// </summary>
    /// <param name="UserNumber">服务商编号</param>
    ///  <param name="dt7">产品列表已验证的部分使用的空鼓信息</param>
    /// <returns></returns>
    public DataTable GetAllKGCanUseByUserNumber(string UserNumber, DataTable dtNow)
    {
        DataTable dt1 = GetTable1(UserNumber);//空鼓入库   

        DataTable dt2 = GetTable2(UserNumber);//空鼓出库
        DataTable dt3 = GetTable3(UserNumber);//订单中循环提货的空鼓使用数据
        DataTable dt4 = GetTable4(UserNumber);//交易大厅买入
        DataTable dt4_1 = GetTable4_1(UserNumber);//交易大厅卖出冻结
        DataTable dt5 = GetTable5(UserNumber);//直接出售给富美公司
        DataTable dt6 = GetTable6(UserNumber);//空鼓调整表
        //将循环提货的，搞成跟第一个表一样的格式。       
        DataTable dt3re1 = revalue(dt3, "蓝装");
        DataTable dt3re2 = revalue(dt3, "绿装");
        DataTable dt3re3 = revalue(dt3, "原装");



        //合并几个表格
        DataTable dt_mt = MergeTable(dt1, dt2, dt3re1, dt3re2, dt3re3, dt4, dt4_1, dt5, dt6);

        //已验证产品列表的空鼓使用情况加入到空鼓使用中。
        if (dtNow != null)
        {
            DataTable dtNowre1 = revalue(dtNow, "蓝装");
            DataTable dtNowre2 = revalue(dtNow, "绿装");
            DataTable dtNowre3 = revalue(dtNow, "原装");
            dt_mt.Merge(dtNowre1, false, MissingSchemaAction.Ignore);
            dt_mt.Merge(dtNowre2, false, MissingSchemaAction.Ignore);
            dt_mt.Merge(dtNowre3, false, MissingSchemaAction.Ignore);
        }

        DataTable dtRes = GetResultKG(dt_mt);

        return dtRes;       
    }

    //对合并完成的空鼓信息进行同类型、同规格信息的数量合并
    protected DataTable GetResultKG(DataTable dt_mt)
    {
        DataTable dt_mt_js = dt_mt.Clone();
        //开始数量合并
        for (int i = 0; i < dt_mt.Rows.Count; i++)
        {
            //ERP空鼓中存在 FM-C912CC/435A(大)、FM-C912CC/435A(小)、FM-C912CC三种，但成品都是对应FM-C912CC，故都替换成FM-C912CC方便核销
            if (dt_mt.Rows[i]["空鼓规格"].ToString().IndexOf("FM-C912CC/435A") >= 0)
            {
                dt_mt.Rows[i]["空鼓规格"] = "FM-C912CC";
            }

            //是否需要更新新表
            bool needadd = true;
            for (int p = 0; p < dt_mt_js.Rows.Count; p++)
            {
                //发现匹配列，开始更新新表原有数据。
                if (dt_mt_js.Rows[p]["客户编号"].ToString() == dt_mt.Rows[i]["客户编号"].ToString() && dt_mt_js.Rows[p]["空鼓类型"].ToString() == dt_mt.Rows[i]["空鼓类型"].ToString() && dt_mt_js.Rows[p]["空鼓规格"].ToString() == dt_mt.Rows[i]["空鼓规格"].ToString() && dt_mt_js.Rows[p]["空鼓规格"].ToString() == dt_mt.Rows[i]["空鼓规格"].ToString())
                {
                    dt_mt_js.Rows[p]["剩余总量"] = (Convert.ToInt32(dt_mt_js.Rows[p]["剩余总量"]) + Convert.ToInt32(dt_mt.Rows[i]["剩余总量"])).ToString();
                    //标记为无需添加新行,此行被合并计算了
                    needadd = false;
                }
            }
            if (needadd)
            {
                DataRow aDataRow = dt_mt_js.NewRow();
                aDataRow.ItemArray = dt_mt.Rows[i].ItemArray;
                dt_mt_js.Rows.Add(aDataRow);
            }
        }
        return dt_mt_js;
    }

    /// <summary>
    /// 获取某加上当前已验证的产品列表的拆分结果后的可用空鼓数量,列： 型号，类型，型号，数量
    /// 传入空值为不限制
    /// </summary>
    /// <param name="UserNumber"></param>
    /// <param name="lx"></param>
    /// <param name="xh"></param>
    /// <returns></returns>
    public DataTable GetAllKGCanUseByWhere(string UserNumber, string lx, string xh, DataTable dtNow)
    {
        string sqltj = " ";
        if (lx != "")
        {
            sqltj = sqltj + " and 空鼓类型 like '%" + lx + "%' ";
        }
        if (xh != "")
        {
            sqltj = sqltj + " and 空鼓规格 like '%" + xh + "%' ";
        }

        DataTable dt = GetAllKGCanUseByUserNumber(UserNumber, dtNow);
        DataRow[] dr = dt.Select(" 1=1 " + sqltj, " 空鼓规格 DESC ");

        DataTable ndt = new DataTable();
        ndt = dt.Clone();
        for (int i = 0; i <= dr.Length - 1; i++)
        {
            if (dr[i]["剩余总量"].ToString() != "0" && dr[i]["剩余总量"].ToString().Trim() != "")
            {
                ndt.Rows.Add(dr[i].ItemArray);
            }

        }
        return ndt;
    }


    /// <summary>
    /// 获取某客户某些条件的可以使用的空鼓数量,列： 型号，类型，型号，数量
    /// 传入空值为不限制
    /// </summary>
    /// <param name="UserNumber"></param>
    /// <param name="lx"></param>
    /// <param name="xh"></param>
    /// <returns></returns>
    public DataTable GetAllKGCanUseByWhere(string UserNumber, string lx, string xh)
    {
        string sqltj = " ";
        if (lx != "")
        {
            sqltj = sqltj + " and 空鼓类型 like '%" + lx + "%' ";
        }
        if (xh != "")
        {
            sqltj = sqltj + " and 空鼓规格 like '%" + xh + "%' ";
        }

        DataTable dt = GetAllKGCanUseByUserNumber(UserNumber, null);
        DataRow[] dr = dt.Select(" 1=1 " + sqltj, " 空鼓规格 DESC ");

        DataTable ndt = new DataTable();
        ndt = dt.Clone();
        for (int i = 0; i <= dr.Length - 1; i++)
        {
            if (dr[i]["剩余总量"].ToString() != "0" && dr[i]["剩余总量"].ToString().Trim() != "")
            {
                ndt.Rows.Add(dr[i].ItemArray);
            }
        }
        return ndt;
    }
    #endregion

    #region 合格旧硒鼓拆分方法
    /// <summary>   
    /// 合格旧硒鼓普通型号的空鼓扣减拆分，如果空鼓足够，结果放入dtkg中，如果空鼓不足，返回msg提示字符串   
   /// </summary>
   /// <param name="fwsbh">用户编号</param>
    /// <param name="syjxglx">用户编号</param>
   /// <param name="cplb">产品类别</param>
   /// <param name="cpxh">产品型号</param>
   /// <param name="cpsl">产品数量</param>
   /// <param name="dtkg">当前已拆分完成数据</param>
   /// <param name="msg">如果数量不足，提示信息由此字段返回</param>
    public void ComputeKG(string fwsbh,string cplb, string cpxh, int cpsl, ref DataTable dtkg_hg, ref string msg_hg)
    {
        int yuanz = 0;//使用的原装空鼓量
        int lanz = 0;//使用的蓝装空鼓量
        int lvz = 0;//使用的绿装空鼓量

        //获取该型号的空鼓各种类别的当前可用量       
        DataTable dtky = GetAllKGCanUseByWhere(fwsbh, "", cpxh, dtkg_hg);       
        int lvz_ky = 0;//绿装空鼓可用量
        int yuanz_ky = 0;//原装空鼓可用量
        int lanz_ky = 0;//蓝装空鼓可用量
        for (int i = 0; i < dtky.Rows.Count; i++)
        {
            string kglx = dtky.Rows[i]["空鼓类型"].ToString();
            int SYZL = Convert.ToInt32(dtky.Rows[i]["剩余总量"].ToString());
            if (SYZL > 0)
            {
                if (kglx == "绿装")
                {
                    lvz_ky = SYZL;
                }
                if (kglx == "蓝装")
                {
                    lanz_ky = SYZL;
                }
                if (kglx == "原装")
                {
                    yuanz_ky = SYZL;
                }
            }
        }

        //根据蓝、绿装进行空鼓拆分
        if (cplb == "蓝装")//如果产品为蓝装，则拆分顺序为：蓝装空鼓、原装空鼓
        {
            if (lanz_ky >= cpsl)//如果蓝装空鼓够用，则直接扣减蓝装
            {
                lanz = cpsl;
            }
            else//如果蓝装空鼓不够用，则同时扣减蓝装、原装空鼓
            {
                if (yuanz_ky >= cpsl - lanz_ky)
                {
                    lanz = lanz_ky;
                    yuanz = cpsl - lanz_ky;
                }
                else
                {
                    msg_hg = msg_hg + cplb + cpxh + "，差" + (cpsl - lanz_ky - yuanz_ky).ToString() + "支\\n\\r";
                    lanz = lanz_ky;
                    yuanz = yuanz_ky;
                }
            }
        }
        if (cplb == "绿装")//如果产品为绿装，则拆分顺序为：绿装空鼓、原装空鼓、蓝装空鼓
        {
            if (lvz_ky >= cpsl)//如果绿装空鼓够用，则直接扣减绿装空鼓。
            {
                lvz = cpsl;
            }
            else//如果绿装空鼓不够用
            {
                if (yuanz_ky >= cpsl - lvz_ky)//判断原装空鼓，如果够用则扣减绿装、原装两种空鼓
                {
                    lvz = lvz_ky;
                    yuanz = cpsl - lvz_ky;
                }
                else//如果绿装+原装也不够用，则扣减绿装、原装、蓝装空鼓
                {
                    if (lanz_ky >= cpsl - lvz_ky - yuanz_ky)
                    {
                        lvz = lvz_ky;
                        yuanz = yuanz_ky;
                        lanz = cpsl - lvz_ky - yuanz_ky;
                    }
                    else
                    {
                        msg_hg = msg_hg + cplb + cpxh + "，差" + (cpsl - lvz_ky - yuanz_ky - lanz_ky).ToString() + "支\\n\\r";
                        lvz = lvz_ky;
                        yuanz = yuanz_ky;
                        lanz = lanz_ky;
                    }
                }
            }
        }

        //DataSet dt_KG = DbHelperSQL.Query("select fwsbh as 客户编号,''as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量 from CWGL_KGSYMXJLB where 1!=1");
        if (yuanz + lanz + lvz > 0)
        {
            dtkg_hg.Rows.Add(new object[] { fwsbh, "", cpxh, 0, yuanz, lanz, lvz, cplb, cpxh, cpsl });
        }

    }

    /// <summary>
    /// 合格旧硒鼓特殊型号的空鼓扣减拆分
    /// </summary>   
    public void ComputeTSKG(string fwsbh,string cplb, string cpxh, int cpsl, string kgxh2, ref  DataTable dtkg, ref string msg)
    {
        int yuanzA = 0;//使用的同型号原装空鼓量
        int lanzA = 0;//使用的同型号蓝装空鼓量
        int lvzA = 0;//使用的同型号绿装空鼓量

        int yuanzX = 0;//使用的另一型号原装空鼓量
        int lanzX = 0;//使用的另一型号蓝装空鼓量
        int lvzX = 0;//使用的另一型号绿装空鼓量

        //获取同型号的空鼓各种类别的当前可用量 
        DataTable dtA = GetAllKGCanUseByWhere(fwsbh, "", cpxh, dtkg);  

        int lvz_kyA = 0;//绿装空鼓可用量
        int yuanz_kyA = 0;//原装空鼓可用量
        int lanz_kyA = 0;//蓝装空鼓可用量
        for (int i = 0; i < dtA.Rows.Count; i++)
        {
            string kglx = dtA.Rows[i]["空鼓类型"].ToString();
            int A_SYZL = Convert.ToInt32(dtA.Rows[i]["剩余总量"].ToString());
            if (A_SYZL > 0)
            {
                if (kglx == "绿装")
                {
                    lvz_kyA = A_SYZL;
                }
                if (kglx == "蓝装")
                {
                    lanz_kyA = A_SYZL;
                }
                if (kglx == "原装")
                {
                    yuanz_kyA = A_SYZL;
                }
            }
        }

        int yuanz_kyX = 0;//X规格原装可用空鼓量
        int lanz_kyX = 0;//X规格蓝装可用空鼓量
        int lvz_kyX = 0;//X规格绿装可用空鼓量
        DataTable dtX = GetAllKGCanUseByWhere(fwsbh, "", kgxh2, dtkg);//获取X规格各类型的可用空鼓量
        for (int i = 0; i < dtX.Rows.Count; i++)
        {
            string kglx = dtX.Rows[i]["空鼓类型"].ToString();
            int X_SYZL = Convert.ToInt32(dtX.Rows[i]["剩余总量"].ToString());
            if (X_SYZL > 0)
            {
                if (kglx == "绿装")
                {
                    lvz_kyX = X_SYZL;
                }
                if (kglx == "蓝装")
                {
                    lanz_kyX = X_SYZL;
                }
                if (kglx == "原装")
                {
                    yuanz_kyX = X_SYZL;
                }
            }
        }

        //根据蓝、绿装进行空鼓拆分
        if (cplb == "蓝装")//如果产品为蓝装，则拆分顺序为：同规格蓝装、原装空鼓，X规格蓝装、原装空鼓
        {
            int totalA = lanz_kyA + yuanz_kyA;
            if (totalA >= cpsl)//如果同规格的蓝装、原装空鼓够用，直接扣减同规格
            {
                if (lanz_kyA >= cpsl)//如果同规格蓝装空鼓够用，则直接扣减蓝装
                {
                    lanzA = cpsl;
                }
                else //如果同规格蓝装空鼓不够用，判断同规格的原装是否够用，如果不够，则进一步扣减另一规格的空鼓
                {
                    lanzA = lanz_kyA;
                    yuanzA = cpsl - lanzA;
                }
            }
            else//如果同规格空鼓不够，则继续扣减X规格的空鼓
            {
                lanzA = lanz_kyA;//先将同规格空鼓扣减掉
                yuanzA = yuanz_kyA;
                if (lanz_kyX >= cpsl - totalA)//如果X的蓝装空鼓够用
                {
                    lanzX = cpsl - totalA;
                }
                else//如果X的蓝装不够，则继续扣减X的原装
                {
                    if (yuanz_kyX >= cpsl - totalA - lanz_kyX)
                    {
                        lanzX = lanz_kyX;
                        yuanzX = cpsl - totalA - lanzX;
                    }
                    else
                    {
                        msg = msg + cplb + cpxh + "，差" + (cpsl - totalA - lanz_kyX - yuanz_kyX).ToString() + "支\\n\\r";
                        lanzA = lanz_kyA;
                        yuanzA = yuanz_kyA;
                        lanzX = lanz_kyX;
                        yuanzX = yuanz_kyX;
                    }

                }
            }
        }
        if (cplb == "绿装")//如果产品为绿装，则拆分顺序为：绿装空鼓、原装空鼓、蓝装空鼓
        {
            int totalA = lvz_kyA + yuanz_kyA + lanz_kyA;

            if (totalA >= cpsl)//如果同型号的空鼓数量足够
            {
                if (lvz_kyA >= cpsl)//如果绿装空鼓够用，则直接扣减绿装空鼓。
                {
                    lvzA = cpsl;
                }
                else
                {
                    if (yuanz_kyA >= cpsl - lvz_kyA)//判断原装空鼓，如果够用则扣减绿装、原装两种空鼓
                    {
                        lvzA = lvz_kyA;
                        yuanzA = cpsl - lvzA;
                    }
                    else//如果绿装+原装也不够用，则扣减绿装、原装、蓝装空鼓
                    {
                        lvzA = lvz_kyA;
                        yuanzA = yuanz_kyA;
                        lanzA = cpsl - lvzA - yuanzA;
                    }
                }
            }
            else//如果同型号的空鼓不够，则开始扣减第二个型号的空鼓
            {
                lvzA = lvz_kyA;//先将同规格的空鼓扣减掉
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                if (lvz_kyX >= cpsl - totalA)//如果X的绿装够用，则扣减X的绿装
                {
                    lvzX = cpsl - totalA;
                }
                else
                {
                    if (yuanz_kyX >= cpsl - totalA - lvz_kyX)//如果X的绿装不够，继续扣减X的原装
                    {
                        lvzX = lvz_kyX;
                        yuanzX = cpsl - totalA - lvzX;
                    }
                    else
                    {
                        if (lanz_kyX >= cpsl - totalA - lvz_kyX - yuanz_kyX)//如果X的原装还不够，则继续扣减X的蓝装
                        {
                            lvzX = lvz_kyX;
                            yuanzX = yuanz_kyX;
                            lanzX = cpsl - totalA - lvzX - yuanzX;
                        }
                        else
                        {
                            msg = msg + cplb + cpxh + "，差" + (cpsl - totalA - lvz_kyX - yuanz_kyX - lanz_kyX).ToString() + "支\\n\\r";
                            lvzA = lvz_kyA;
                            yuanzA = yuanz_kyA;
                            lanzA = lanz_kyA;
                            lvzX = lvz_kyX;
                            yuanzX = yuanz_kyX;
                            lanzX = lanz_kyX;
                        }

                    }
                }
            }
        }

        //添加同型号拆分结果
        //DataSet dt_KG = DbHelperSQL.Query("select fwsbh as 客户编号,''as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量 from CWGL_KGSYMXJLB where 1!=1");

        if (yuanzA + lanzA + lvzA > 0)
        {
            dtkg.Rows.Add(new object[] { fwsbh, "", cpxh, 0, yuanzA, lanzA, lvzA, cplb, cpxh, cpsl });
        }

        //添加X型号的扣减结果
        if (yuanzX + lanzX + lvzX > 0)
        {
            dtkg.Rows.Add(new object[] { fwsbh, "", kgxh2, 0, yuanzX, lanzX, lvzX, cplb, cpxh, cpsl });
        }
    }
    #endregion

    #region 获取可用于计算服务站点空鼓收益的剩余空鼓数量   
    private DataTable GetdtRK_profit(string UserNumber, string FWZDBH)
    {
        //从ERP的k101中获取用户从服务站点入库的空鼓数据
        string sql_rk = "select ltrim(rtrim(INVTB.UDF01)) as 客户编号, ltrim(rtrim(INVTB.TB029)) as 空鼓类型, (case ltrim(rtrim(INVTB.TB029)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end) as 空鼓规格,cast(TB007 as int) as 剩余总量 from INVTB left join INVTA on TB002=TA002 and TB001=TA001 and INVTB.TSFM_bsc=INVTA.TSFM_bsc left join  CMSME on TA004=ME001 left join COPMA on INVTB.UDF01=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k101' and ME002 like '%办事处%' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "' and TA006='Y' and ltrim(rtrim(TB012))='" + FWZDBH + "' and convert(datetime,TA003)>='2012-01-01 0:00:00'";

        string sql_ck = "select ltrim(rtrim(INVTB.UDF01)) as 客户编号, ltrim(rtrim(INVTB.TB029)) as 空鼓类型, (case ltrim(rtrim(INVTB.TB029)) when '原装' then DYCPGG else ltrim(rtrim(MB003)) end)  as 空鼓规格,-cast(TB007 as int) as 剩余总量 from INVTB left join INVTA on TB002=TA002 and TB001=TA001 and INVTB.TSFM_bsc=INVTA.TSFM_bsc left join  CMSME on TA004=ME001 left join COPMA on INVTB.UDF01=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k102' and ME002 like '%办事处%' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "' and TA006='Y' and ltrim(rtrim(TB012))='" + FWZDBH + "' and INVTA.UDF01 not like '%用户直通车%' and convert(datetime,TA003)>='2012-01-01 0:00:00'";

        string sql = sql_rk + " union all " + sql_ck;

        DataSet dt1 = GetErpData.GetDataSet(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }
    private DataTable GetdtYJS_profit(string UserNumber, string FWZDBH)
    {
        //获取已经计算过收益的旧硒鼓总数
        string sql_yjs = "select khbh as 客户编号,jxglb as 空鼓类型,jxgxh as 空鼓规格,-hssyjxgsl as 剩余总量 from FM_FWZDKGSYMXB where djly='用户直通车' and khbh='" + UserNumber + "' and sfhgjxg='是' and fwzdbh='" + FWZDBH + "' and createtime>='2012-01-01 0:00:00'";
        DataSet dsSY = DbHelperSQL.Query(sql_yjs);

        if (dsSY != null && dsSY.Tables != null)
        {
            return dsSY.Tables[0];
        }
        return null;
    }

    //根据用户编号和服务站点编号获取该用户当前剩余的可用户计算该服务站点收益的旧硒鼓数量。
    public DataTable GetAllProfitKGCanUseByUserNumber(string UserNumber, string FWZDBH,DataTable dtNow)
    {
        DataTable dterp = GetdtRK_profit(UserNumber, FWZDBH);
        DataTable dtysy = GetdtYJS_profit(UserNumber, FWZDBH);
        DataTable dt_mt = dterp.Clone();
        dt_mt.Merge(dterp, false, MissingSchemaAction.Ignore);
        dt_mt.Merge(dtysy, false, MissingSchemaAction.Ignore);


        //已验证产品列表的空鼓使用情况加入到空鼓使用中。
        if (dtNow != null)
        {
            DataTable dtNowre1 = revalue(dtNow, "蓝装");
            DataTable dtNowre2 = revalue(dtNow, "绿装");
            DataTable dtNowre3 = revalue(dtNow, "原装");
            dt_mt.Merge(dtNowre1, false, MissingSchemaAction.Ignore);
            dt_mt.Merge(dtNowre2, false, MissingSchemaAction.Ignore);
            dt_mt.Merge(dtNowre3, false, MissingSchemaAction.Ignore);
        }

        DataTable dtRes = GetResultKG(dt_mt);
        DataTable dt_mt_js = new DataTable();
        dt_mt_js = dtRes.Clone();
        for (int i = 0; i <= dtRes.Rows.Count - 1; i++)
        {
            if (dtRes.Rows[i]["剩余总量"].ToString() != "0" && dtRes.Rows[i]["剩余总量"].ToString().Trim() != "")
            {
                dt_mt_js.Rows.Add(dtRes.Rows[i].ItemArray);
            }
        }
        return dt_mt_js;
    }

  
    /// <summary>
    /// 获取当前可用于计算服务站点收益的旧硒鼓数量列表，类型，型号可以为空
    /// 用于除订单之外的计算
    /// </summary> 
    /// <param name="dtKG">可用于计算收益的空鼓列表</param>
    /// <param name="lx">类型</param>
    /// <param name="xh">型号</param>
    /// <returns></returns>
    public int GetProfitKGCanUseByWhere(DataTable dtKG, string lx, string xh,int sl)
    {       
        int kgsl = 0;
        string num = dtKG.Compute("sum(剩余总量)", "空鼓类型='" + lx + "' and 空鼓规格='" + xh + "'").ToString();
        if (num.Trim() != "")
        {
            kgsl = Convert.ToInt32(num);
        }
        if (kgsl < sl)
        {
            return kgsl;
        }
        return sl;        
    }
    /// <summary>
    /// 获取当前可用于计算服务站点收益的旧硒鼓数量列表，类型，型号可以为空
    ///只用于订单的计算
    /// </summary>
    /// <param name="UserNumber"></param>
    /// <param name="lx"></param>
    /// <param name="xh"></param>
    /// <param name="dtNow"></param>
    /// <returns></returns>
    public int GetProfitKGCanUseByWhere(DataTable dtKG, string lx, string xh,int sl,DataTable dtNow)
    {        
        int kgsl = 0;
        int now = 0;
        string num = dtKG.Compute("sum(剩余总量)", "空鼓类型='" + lx + "' and 空鼓规格='" + xh + "'").ToString();
        string Now = "";
        if (dtNow != null && dtNow.Rows.Count > 0)
        {
            Now = dtNow.Compute("sum(核算收益旧硒鼓数量)", "旧硒鼓类别='" + lx + "' and 旧硒鼓型号='" + xh + "'").ToString();
        }        
        if (num.Trim() != "")
        {
            kgsl = Convert.ToInt32(num);
        }
        if (Now.Trim() != "")
        {
            now = Convert.ToInt32(Now);
        }
        if ((kgsl-now) < sl)
        {
            return kgsl-now;
        }
        return sl;      
    }

    #endregion

    #region 获取不合格旧硒鼓当前可用情况
    public DataTable GetAll_bhg_KGCanUserByWhere(string UserNumber, string kglx, string kggg, DataTable dtNow)
    {
        string sql_ru = "select khbh as 客户编号,yslx as 不合格旧硒鼓类别,ysxh as 不合格旧硒鼓型号,sum(yssl) as 剩余总量 from FM_FWZDBHGJXGLRB as a left join YSXXXX as b on a.number=b.parentnumber where a.khbh='" + UserNumber + "' and sfyx='是' group by khssbsc,khbh,yslx,ysxh ";

        string sql_yz = "select yhbh as 客户编号,'原装' as 不合格旧硒鼓类别,kgxh as 不合格旧硒鼓型号,-yz as 剩余总量 from FM_BHGJXGSYMXB where djly='用户直通车' and yhbh='" + UserNumber + "' and yz<>0 and yz is not null and kjdx='用户' ";
        string sql_lan = "select yhbh as 客户编号,'蓝装' as 不合格旧硒鼓类别,kgxh as 不合格旧硒鼓型号,-lanz as 剩余总量 from FM_BHGJXGSYMXB where djly='用户直通车' and yhbh='" + UserNumber + "' and lanz<>0 and lanz is not null and kjdx='用户'";
        string sql_lv = "select yhbh as 客户编号,'绿装' as 不合格旧硒鼓类别,kgxh as 不合格旧硒鼓型号,-lvz as 剩余总量 from FM_BHGJXGSYMXB where djly='用户直通车' and yhbh='" + UserNumber + "' and lvz<>0 and lvz is not null and kjdx='用户'";

        string sql = "select 客户编号,不合格旧硒鼓类别 as 空鼓类型,不合格旧硒鼓型号 as 空鼓规格,sum(剩余总量) as 剩余总量 from (" + sql_ru + " union all " + sql_yz + " union all " + sql_lan + " union all " + sql_lv + ") as tab where 不合格旧硒鼓类别 like '%" + kglx + "%' and 不合格旧硒鼓型号 like '%" + kggg + "%' group by 客户编号,不合格旧硒鼓类别,不合格旧硒鼓型号 having sum(剩余总量)<>0";

        //所有录入的不合格旧硒鼓-所有已用的不合格旧硒鼓
        DataSet ds = DbHelperSQL.Query(sql);

        
        DataTable dt_mt = ds.Tables[0];
       
        //已验证产品列表的空鼓使用情况加入到空鼓使用中。
        if (dtNow != null)
        {
            DataTable dtNowre1 = revalue(dtNow, "蓝装");
            DataTable dtNowre2 = revalue(dtNow, "绿装");
            DataTable dtNowre3 = revalue(dtNow, "原装");
            dt_mt.Merge(dtNowre1, false, MissingSchemaAction.Ignore);
            dt_mt.Merge(dtNowre2, false, MissingSchemaAction.Ignore);
            dt_mt.Merge(dtNowre3, false, MissingSchemaAction.Ignore);
            //获取合并完成的信息
            //DataTable dtRes = GetResultKG(dt_mt);

            DataTable dt_mt_js = new DataTable();
            dt_mt_js = dt_mt.Clone();
            for (int i = 0; i <= dt_mt.Rows.Count - 1; i++)
            {
                if (dt_mt.Rows[i]["剩余总量"].ToString() != "0" && dt_mt.Rows[i]["剩余总量"].ToString().Trim() != "")
                {
                    dt_mt_js.Rows.Add(dt_mt.Rows[i].ItemArray);
                }
            }
            return dt_mt_js;
        }
       
        return dt_mt;
    }

    #endregion
   
    #region 不合格旧硒鼓拆分方法
    /// <summary>   
    /// 合格旧硒鼓普通型号的空鼓扣减拆分，如果空鼓足够，结果放入dtkg中，如果空鼓不足，返回msg提示字符串   
    /// </summary>
    /// <param name="fwsbh">用户编号</param>
    /// <param name="syjxglx">用户编号</param>
    /// <param name="cplb">产品类别</param>
    /// <param name="cpxh">产品型号</param>
    /// <param name="cpsl">产品数量</param>
    /// <param name="dtkg">当前已拆分完成数据</param>
    /// <param name="msg">如果数量不足，提示信息由此字段返回</param>
    public void ComputeKG_BHG(string fwsbh, string cplb, string cpxh, int cpsl, ref DataTable dtkg_bhg, ref string msg_bhg)
    {
        int yuanz = 0;//使用的原装空鼓量
        int lanz = 0;//使用的蓝装空鼓量
        int lvz = 0;//使用的绿装空鼓量

        //获取该型号的空鼓各种类别的当前可用量       
        DataTable dtky = GetAll_bhg_KGCanUserByWhere(fwsbh, "", cpxh, dtkg_bhg);
        int lvz_ky = 0;//绿装空鼓可用量
        int yuanz_ky = 0;//原装空鼓可用量
        int lanz_ky = 0;//蓝装空鼓可用量
        for (int i = 0; i < dtky.Rows.Count; i++)
        {
            string kglx = dtky.Rows[i]["空鼓类型"].ToString();
            int SYZL = Convert.ToInt32(dtky.Rows[i]["剩余总量"].ToString());
            if (SYZL > 0)
            {
                if (kglx == "绿装")
                {
                    lvz_ky = SYZL;
                }
                if (kglx == "蓝装")
                {
                    lanz_ky = SYZL;
                }
                if (kglx == "原装")
                {
                    yuanz_ky = SYZL;
                }
            }
        }

        //根据蓝、绿装进行空鼓拆分
        if (cplb == "蓝装")//如果产品为蓝装，则拆分顺序为：蓝装空鼓、原装空鼓
        {
            if (lanz_ky >= cpsl)//如果蓝装空鼓够用，则直接扣减蓝装
            {
                lanz = cpsl;
            }
            else//如果蓝装空鼓不够用，则同时扣减蓝装、原装空鼓
            {
                if (yuanz_ky >= cpsl - lanz_ky)
                {
                    lanz = lanz_ky;
                    yuanz = cpsl - lanz_ky;
                }
                else
                {
                    msg_bhg = msg_bhg + cplb + cpxh + "，差" + (cpsl - lanz_ky - yuanz_ky).ToString() + "支\\n\\r";
                    lanz = lanz_ky;
                    yuanz = yuanz_ky;
                }
            }
        }
        if (cplb == "绿装")//如果产品为绿装，则拆分顺序为：绿装空鼓、原装空鼓、蓝装空鼓
        {
            if (lvz_ky >= cpsl)//如果绿装空鼓够用，则直接扣减绿装空鼓。
            {
                lvz = cpsl;
            }
            else//如果绿装空鼓不够用
            {
                if (yuanz_ky >= cpsl - lvz_ky)//判断原装空鼓，如果够用则扣减绿装、原装两种空鼓
                {
                    lvz = lvz_ky;
                    yuanz = cpsl - lvz_ky;
                }
                else//如果绿装+原装也不够用，则扣减绿装、原装、蓝装空鼓
                {
                    if (lanz_ky >= cpsl - lvz_ky - yuanz_ky)
                    {
                        lvz = lvz_ky;
                        yuanz = yuanz_ky;
                        lanz = cpsl - lvz_ky - yuanz_ky;
                    }
                    else
                    {
                        msg_bhg = msg_bhg + cplb + cpxh + "，差" + (cpsl - lvz_ky - yuanz_ky - lanz_ky).ToString() + "支\\n\\r";
                        lvz = lvz_ky;
                        yuanz = yuanz_ky;
                        lanz = lanz_ky;
                    }
                }
            }
        }

        //DataSet dt_KG = DbHelperSQL.Query("select fwsbh as 客户编号,''as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量 from CWGL_KGSYMXJLB where 1!=1");
        if (yuanz + lanz + lvz > 0)
        {
            dtkg_bhg.Rows.Add(new object[] { fwsbh, "", cpxh, 0, yuanz, lanz, lvz, cplb, cpxh, cpsl });
        }

    }

    /// <summary>
    /// 不合格旧硒鼓特殊型号的空鼓扣减拆分
    /// </summary>   
    public void ComputeTSKG_BHG(string fwsbh, string cplb, string cpxh, int cpsl, string kgxh2, ref  DataTable dtkg_bhg, ref string msg_bhg)
    {
        int yuanzA = 0;//使用的同型号原装空鼓量
        int lanzA = 0;//使用的同型号蓝装空鼓量
        int lvzA = 0;//使用的同型号绿装空鼓量

        int yuanzX = 0;//使用的另一型号原装空鼓量
        int lanzX = 0;//使用的另一型号蓝装空鼓量
        int lvzX = 0;//使用的另一型号绿装空鼓量

        //获取同型号的空鼓各种类别的当前可用量 
        DataTable dtA = GetAll_bhg_KGCanUserByWhere(fwsbh, "", cpxh, dtkg_bhg);

        int lvz_kyA = 0;//绿装空鼓可用量
        int yuanz_kyA = 0;//原装空鼓可用量
        int lanz_kyA = 0;//蓝装空鼓可用量
        for (int i = 0; i < dtA.Rows.Count; i++)
        {
            string kglx = dtA.Rows[i]["空鼓类型"].ToString();
            int A_SYZL = Convert.ToInt32(dtA.Rows[i]["剩余总量"].ToString());
            if (A_SYZL > 0)
            {
                if (kglx == "绿装")
                {
                    lvz_kyA = A_SYZL;
                }
                if (kglx == "蓝装")
                {
                    lanz_kyA = A_SYZL;
                }
                if (kglx == "原装")
                {
                    yuanz_kyA = A_SYZL;
                }
            }
        }

        int yuanz_kyX = 0;//X规格原装可用空鼓量
        int lanz_kyX = 0;//X规格蓝装可用空鼓量
        int lvz_kyX = 0;//X规格绿装可用空鼓量
        DataTable dtX = GetAll_bhg_KGCanUserByWhere(fwsbh, "", kgxh2, dtkg_bhg);//获取X规格各类型的可用空鼓量
        for (int i = 0; i < dtX.Rows.Count; i++)
        {
            string kglx = dtX.Rows[i]["空鼓类型"].ToString();
            int X_SYZL = Convert.ToInt32(dtX.Rows[i]["剩余总量"].ToString());
            if (X_SYZL > 0)
            {
                if (kglx == "绿装")
                {
                    lvz_kyX = X_SYZL;
                }
                if (kglx == "蓝装")
                {
                    lanz_kyX = X_SYZL;
                }
                if (kglx == "原装")
                {
                    yuanz_kyX = X_SYZL;
                }
            }
        }

        //根据蓝、绿装进行空鼓拆分
        if (cplb == "蓝装")//如果产品为蓝装，则拆分顺序为：同规格蓝装、原装空鼓，X规格蓝装、原装空鼓
        {
            int totalA = lanz_kyA + yuanz_kyA;
            if (totalA >= cpsl)//如果同规格的蓝装、原装空鼓够用，直接扣减同规格
            {
                if (lanz_kyA >= cpsl)//如果同规格蓝装空鼓够用，则直接扣减蓝装
                {
                    lanzA = cpsl;
                }
                else //如果同规格蓝装空鼓不够用，判断同规格的原装是否够用，如果不够，则进一步扣减另一规格的空鼓
                {
                    lanzA = lanz_kyA;
                    yuanzA = cpsl - lanzA;
                }
            }
            else//如果同规格空鼓不够，则继续扣减X规格的空鼓
            {
                lanzA = lanz_kyA;//先将同规格空鼓扣减掉
                yuanzA = yuanz_kyA;
                if (lanz_kyX >= cpsl - totalA)//如果X的蓝装空鼓够用
                {
                    lanzX = cpsl - totalA;
                }
                else//如果X的蓝装不够，则继续扣减X的原装
                {
                    if (yuanz_kyX >= cpsl - totalA - lanz_kyX)
                    {
                        lanzX = lanz_kyX;
                        yuanzX = cpsl - totalA - lanzX;
                    }
                    else
                    {
                        msg_bhg = msg_bhg + cplb + cpxh + "，差" + (cpsl - totalA - lanz_kyX - yuanz_kyX).ToString() + "支\\n\\r";
                        lanzA = lanz_kyA;
                        yuanzA = yuanz_kyA;
                        lanzX = lanz_kyX;
                        yuanzX = yuanz_kyX;
                    }

                }
            }
        }
        if (cplb == "绿装")//如果产品为绿装，则拆分顺序为：绿装空鼓、原装空鼓、蓝装空鼓
        {
            int totalA = lvz_kyA + yuanz_kyA + lanz_kyA;

            if (totalA >= cpsl)//如果同型号的空鼓数量足够
            {
                if (lvz_kyA >= cpsl)//如果绿装空鼓够用，则直接扣减绿装空鼓。
                {
                    lvzA = cpsl;
                }
                else
                {
                    if (yuanz_kyA >= cpsl - lvz_kyA)//判断原装空鼓，如果够用则扣减绿装、原装两种空鼓
                    {
                        lvzA = lvz_kyA;
                        yuanzA = cpsl - lvzA;
                    }
                    else//如果绿装+原装也不够用，则扣减绿装、原装、蓝装空鼓
                    {
                        lvzA = lvz_kyA;
                        yuanzA = yuanz_kyA;
                        lanzA = cpsl - lvzA - yuanzA;
                    }
                }
            }
            else//如果同型号的空鼓不够，则开始扣减第二个型号的空鼓
            {
                lvzA = lvz_kyA;//先将同规格的空鼓扣减掉
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                if (lvz_kyX >= cpsl - totalA)//如果X的绿装够用，则扣减X的绿装
                {
                    lvzX = cpsl - totalA;
                }
                else
                {
                    if (yuanz_kyX >= cpsl - totalA - lvz_kyX)//如果X的绿装不够，继续扣减X的原装
                    {
                        lvzX = lvz_kyX;
                        yuanzX = cpsl - totalA - lvzX;
                    }
                    else
                    {
                        if (lanz_kyX >= cpsl - totalA - lvz_kyX - yuanz_kyX)//如果X的原装还不够，则继续扣减X的蓝装
                        {
                            lvzX = lvz_kyX;
                            yuanzX = yuanz_kyX;
                            lanzX = cpsl - totalA - lvzX - yuanzX;
                        }
                        else
                        {
                            msg_bhg = msg_bhg + cplb + cpxh + "，差" + (cpsl - totalA - lvz_kyX - yuanz_kyX - lanz_kyX).ToString() + "支\\n\\r";
                            lvzA = lvz_kyA;
                            yuanzA = yuanz_kyA;
                            lanzA = lanz_kyA;
                            lvzX = lvz_kyX;
                            yuanzX = yuanz_kyX;
                            lanzX = lanz_kyX;
                        }

                    }
                }
            }
        }

        //添加同型号拆分结果
        //DataSet dt_KG = DbHelperSQL.Query("select fwsbh as 客户编号,''as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量 from CWGL_KGSYMXJLB where 1!=1");

        if (yuanzA + lanzA + lvzA > 0)
        {
            dtkg_bhg.Rows.Add(new object[] { fwsbh, "", cpxh, 0, yuanzA, lanzA, lvzA, cplb, cpxh, cpsl });
        }

        //添加X型号的扣减结果
        if (yuanzX + lanzX + lvzX > 0)
        {
            dtkg_bhg.Rows.Add(new object[] { fwsbh, "", kgxh2, 0, yuanzX, lanzX, lvzX, cplb, cpxh, cpsl });
        }
    }
    #endregion

    #region 用户服务站点收益计算的不合格旧硒鼓数量
    public int GetProfitKGCanUseByWhere_bhg(string UserNumber, string FWZDBH,string lx,string xh,int sl,DataTable dtNow)
    {
        string sql_ru = "select khbh as 客户编号,yslx as 不合格旧硒鼓类别,ysxh as 不合格旧硒鼓型号,yssl as 剩余总量 from FM_FWZDBHGJXGLRB as a left join YSXXXX as b on a.number=b.parentnumber where a.khbh='" + UserNumber+ "'  and a.fwzdbh='"+FWZDBH+"' and sfyx='是' and yslx='"+lx+"' and ysxh='"+xh+"' and createtime>='2012-01-01 0:00:00'";

        string sql_chu = "select khbh as 客户编号,jxglb as 不合格旧硒鼓类别,jxgxh as 不合格旧硒鼓型号,-hssyjxgsl as 剩余总量 from FM_FWZDKGSYMXB where djly='用户直通车' and khbh='" + UserNumber + "' and fwzdbh='"+FWZDBH +"' and khlx='用户' and sfhgjxg='否' and jxglb='"+lx+"' and jxgxh='"+xh+"' and createtime>='2012-01-01 0:00:00' ";       

        string sql = "select 客户编号,不合格旧硒鼓类别 as 空鼓类型,不合格旧硒鼓型号 as 空鼓规格,isnull(sum(剩余总量),0) as 剩余总量 from (" + sql_ru + " union all " + sql_chu +") as tab  group by 客户编号,不合格旧硒鼓类别,不合格旧硒鼓型号";

        DataSet ds = DbHelperSQL.Query(sql);

        int num = 0;//剩余的可用于计算是收益的不合格旧硒鼓总量
        int now = 0;
        if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["剩余总量"].ToString().Trim() != "")
        {
            num = Convert.ToInt32(ds.Tables[0].Rows[0]["剩余总量"]);
        }
        if (dtNow != null && dtNow.Rows.Count > 0)
        {
            string kgnow = dtNow.Compute("sum(核算收益旧硒鼓数量)", "旧硒鼓类别='" + lx + "' and 旧硒鼓型号='" + xh + "'").ToString();
            if (kgnow.Trim() != "")
            {
                now = Convert.ToInt32(kgnow);
            }
        }
        if ((num - now) < sl)
        {
            return num - now;
        }
        return sl;
    }

    #endregion

    #region 用户当前可用信用额度计算
    /// <summary>
    /// 获取用户当前可用信用额度
    /// </summary>
    /// <param name="UserNumber">服务商编号</param>
    /// <returns></returns>
    public double GetCreditMoney(string UserNumber)
    {
        //订单中未填写过销货单的总金额 
        string sql1 = "select TC004 as 客户编号,sum(cast((TD008-TD009)*TD011 as numeric(18,2))) as 金额 from COPTD left join COPTC on TC001=TD001 and TC002=TD002 and COPTC.TSFM_bsc=COPTD.TSFM_bsc where ltrim(rtrim(TC004))='" + UserNumber + "' and TC027='Y' and TD016='N' group by TC004";

        //销货单中未开过销售发票的总金额        

        string sql2 = "select TG004 as 客户编号,sum((case cast(TH008 as int) when 0 then cast((TH035+TH036) as numeric(18,2)) else cast((TH008-TH042)*TH012 as numeric(18,2)) end)) as 金额 from COPTG left join COPTH on TH001=TG001 and TH002=TG002 and COPTG.TSFM_bsc=COPTH.TSFM_bsc where ltrim(rtrim(TG004)) = '" + UserNumber + "' and TG023='Y' and TH026='N' group by TG004";

        //销退单中未开过发票的金额       
        string sql3 = "select TI004 as 客户编号,sum((case cast(TJ007 as int) when 0 then cast(TJ012 as numeric(18,2)) else cast((TJ007-TJ037)*TJ011 as numeric(18,2)) end) ) as 金额 from COPTI left join COPTJ on TI001=TJ001 and TI002=TJ002 and COPTI.TSFM_bsc=COPTJ.TSFM_bsc where ltrim(rtrim(TI004))='" + UserNumber + "' and TI019='Y' and TJ024='N' group by TI004";

        //开过的发票总额
        string sql4 = "select TA004 as 客户编号,sum(TA041+TA042-TA098) as 金额 from ACRTA where ltrim(rtrim(TA004))='" + UserNumber + "' and TA025='Y' and TA079='1' group by TA004";
        string sql4_2 = "select TA004 as 客户编号, -sum(TA041+TA042-TA098) as 金额 from ACRTA where ltrim(rtrim(TA004))='" + UserNumber + "' and TA025='Y' and TA079='2' group by TA004";


        //信用额度初始值
        string sql5 = "select MA001 as 客户编号,MA033*(1+MA034) as 金额 from COPMA where ltrim(rtrim(MA001))='" + UserNumber + "' and MA097='1'";

        //收款单中未核销的部分的总额
        string sql6 = "select TK004 as 客户编号,sum(TK033+TK035+TK036-TK038) as 金额 from ACRTK where ltrim(rtrim(TK004))= '" + UserNumber + "' and TK020='Y' and TK030!=3 group by TK004";

        //退款单中未核销部分的总额
        string sql7 = "select TK004 as 客户编号,sum(TK033+TK035+TK036-TK038) as 金额 from ACRTK where ltrim(rtrim(TK004))= '" + UserNumber + "' and TK020='Y' and TK001='6D' and TK030!=3 group by TK004";

        //业务平台中未录入销货单的订单金额
        //只用直通车的用户才有订单未录入销货单的金额，推广人员没有这一项。
        string sql8 = "select number as 订单号,gmfs as 购买方式,cplb as 产品类别,cpxh as 产品型号,isnull(sl,0) as 产品数量,0 as 已交数量,isnull(jg,0) as 价格,isnull(cast(0.00 as numeric(18,2)),0) as 剩余金额 from FM_YHDDB as a left join  FM_YHDDB_CPLB as b on a.number=b.parentnumber where yhbh= '" + UserNumber + "'  and isnull(qzjs,'')<>'是'";

        //新版合格旧硒鼓交易中心ERP中未审核的收款单，因审核后的收款单转入了“收款单中未核销部分的总额中计算了”
        string sql9 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6C1' and TK004='" + UserNumber + "' and TK020<>'Y'";
        //新版合格旧硒鼓交易中心ERP中未审核的退款单
        string sql10 = "select -isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6D1' and TK004='" + UserNumber + "' and TK020<>'Y'";
        //新版合格旧硒鼓交易中心被冻结的额度(剩余可交易量总额)
        string sql11 = "select  -isnull(sum(SYKMSL*DJ),0) as 金额  from NewPurchase where MRFKHBH='" + UserNumber + "' and JYZT <>'5' and JYZT <>'6'";
        //新版合格旧硒鼓交易中心在成功交易明细表中的收款额度(转换到ERP之前的临时数据)
        string sql12 = "select  isnull(sum(CJZJ_MJSJSD),0) as 金额  from CGJYMXB where KHBH='" + UserNumber + "' and ZHZT ='0' and MMLX ='卖出'";
        //新版合格旧硒鼓交易中心在成功交易明细表中的退款额度(转换到ERP之前的临时数据)
        string sql13 = "select  -isnull(sum(CJZJ),0) as 金额  from CGJYMXB where KHBH='" + UserNumber + "' and ZHZT ='0' and MMLX ='买入'";
        //新版合格旧硒鼓交易中心提现申请中的信用额度扣减(只扣减已生效的，其他状态，将在ERP信用额度中体现)
        string sql14 = "select  -isnull(sum(TXED),0) as 金额  from SQTX where KHBH='" + UserNumber + "' and ZT ='已生效' ";


        //中转收款单中未转入ERP的数据
        string sql15 = "select isnull(sum(case jeys when 'ADD' then JE when 'DEL' then -JE end),0) as 金额 from FM_ZZSKD where KHBH='" + UserNumber + "' and isnull(ZHERPZT,'')<>'是' and isnull(JLZT,'')<>'作废'";

        //用户的中奖信息中未转入ERP的数据，目前推广没有中奖
        string sql16 = "select isnull(sum(zjje),0) as 金额 from FM_YHZJXXB where yhbh='" + UserNumber + "' and isnull(ZHZT,'')<>'是'";


        //中转收款单中寄送发票费用转入ERP未审核的数据（审核之后的通过“收款单中未核销部分的总额”计算进去了）
        string sql15_0 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6C3' and TK004='" + UserNumber + "' and TK020<>'Y'";
        //中转收款单中扣减交易大厅服务费转入ERP未审核的数据
        string sql15_1 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6C4' and TK004='" + UserNumber + "' and TK020<>'Y'";
        //中转收款单中用户直接出售转入ERP未审核的数据
        string sql15_2 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6I1' and TK004='" + UserNumber + "' and TK020<>'Y'";
        //用户博彩中奖转入ERP未审核的数据
        string sql16_0 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6C5' and TK004='" + UserNumber + "' and TK020<>'Y'";

        string sql17 = "select isnull(sum(金额),0) as 金额 from (" + sql15_0 + " union all " + sql15_1 + " union all " + sql15_2 + " union all " + sql16_0 + ") as tab ";

        DataSet ds_1 = GetErpData.GetDataSet(sql1);
        DataSet ds_2 = GetErpData.GetDataSet(sql2);
        DataSet ds_3 = GetErpData.GetDataSet(sql3);
        DataSet ds_4 = GetErpData.GetDataSet(sql4);
        DataSet ds_4_2 = GetErpData.GetDataSet(sql4_2);
        DataSet ds_5 = GetErpData.GetDataSet(sql5);
        DataSet ds_6 = GetErpData.GetDataSet(sql6);
        DataSet ds_7 = GetErpData.GetDataSet(sql7);

        DataSet ds_9 = GetErpData.GetDataSet(sql9);
        DataSet ds_10 = GetErpData.GetDataSet(sql10);
        DataSet ds_11 = DbHelperSQL.Query(sql11);
        DataSet ds_12 = DbHelperSQL.Query(sql12);
        DataSet ds_13 = DbHelperSQL.Query(sql13);
        DataSet ds_14 = DbHelperSQL.Query(sql14);
        DataSet ds_15 = DbHelperSQL.Query(sql15);
        DataSet ds_16 = DbHelperSQL.Query(sql16);
        DataSet ds_17 = GetErpData.GetDataSet(sql17);

        DataSet ds8 = DbHelperSQL.Query(sql8);

        if (ds8 != null && ds8.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds8.Tables[0].Rows.Count; i++)
            {
                string ddh = ds8.Tables[0].Rows[i]["订单号"].ToString();
                string thfs = ds8.Tables[0].Rows[i]["购买方式"].ToString();
                string cpjg = ds8.Tables[0].Rows[i]["价格"].ToString();

                string db = "";
                if (thfs == "循环价购买" || thfs == "市场价购买")
                {
                    db = "2313";
                }

                string sqlXH = "select isnull(sum(cast(TH008 as int)),0) as 已交付数量 from COPTG left join COPTH on TG001=TH001 and TG002=TH002 and COPTG.TSFM_bsc=COPTH.TSFM_bsc where ltrim(rtrim(TG004))='" + UserNumber + "' and COPTG.UDF01='" + ddh + "' and TG001 in ('" + db + "') and (case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' end)='" + ds8.Tables[0].Rows[i]["产品类别"].ToString() + "' and ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005))))= '" + ds8.Tables[0].Rows[i]["产品型号"].ToString() + "' and TG023='Y' and cast(TH012 as numeric(18,2))=" + cpjg + "";

                DataSet dsERP = GetErpData.GetDataSet(sqlXH);
                if (dsERP != null && dsERP.Tables[0].Rows.Count > 0)
                {
                    ds8.Tables[0].Rows[i]["已交数量"] = dsERP.Tables[0].Rows[0]["已交付数量"];
                    ds8.Tables[0].Rows[i]["剩余金额"] = (Convert.ToInt32(ds8.Tables[0].Rows[i]["产品数量"]) - Convert.ToInt32(dsERP.Tables[0].Rows[0]["已交付数量"])) * Convert.ToDecimal(ds8.Tables[0].Rows[i]["价格"]);
                }
            }
        }
        double d1 = 0.00;
        double d2 = 0.00;
        double d3 = 0.00;
        double d4 = 0.00;
        double d4_2 = 0.00;
        double d5 = 0.00;
        double d6 = 0.00;
        double d7 = 0.00;
        double d8 = 0.00;
        double d9 = 0.00;
        double d10 = 0.00;
        double d11 = 0.00;
        double d12 = 0.00;
        double d13 = 0.00;
        double d14 = 0.00;
        double d15 = 0.00;
        double d16 = 0.00;
        double d17 = 0.00;
        if (ds_1 != null && ds_1.Tables[0].Rows.Count > 0)
        {
            d1 = Convert.ToDouble(ds_1.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_2 != null & ds_2.Tables[0].Rows.Count > 0)
        {
            d2 = Convert.ToDouble(ds_2.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_3 != null & ds_3.Tables[0].Rows.Count > 0)
        {
            d3 = Convert.ToDouble(ds_3.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_4 != null & ds_4.Tables[0].Rows.Count > 0)
        {
            d4 = Convert.ToDouble(ds_4.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_4_2 != null & ds_4_2.Tables[0].Rows.Count > 0)
        {
            d4_2 = Convert.ToDouble(ds_4_2.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_5 != null & ds_5.Tables[0].Rows.Count > 0)
        {
            d5 = Convert.ToDouble(ds_5.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_6 != null & ds_6.Tables[0].Rows.Count > 0)
        {
            d6 = Convert.ToDouble(ds_6.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_7 != null && ds_7.Tables[0].Rows.Count > 0)
        {
            d7 = Convert.ToDouble(ds_7.Tables[0].Rows[0]["金额"].ToString());
        }

        if (ds_9 != null && ds_9.Tables[0].Rows.Count > 0)
        {
            d9 = Convert.ToDouble(ds_9.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_10 != null && ds_10.Tables[0].Rows.Count > 0)
        {
            d10 = Convert.ToDouble(ds_10.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_11 != null && ds_11.Tables[0].Rows.Count > 0)
        {
            d11 = Convert.ToDouble(ds_11.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_12 != null && ds_12.Tables[0].Rows.Count > 0)
        {
            d12 = Convert.ToDouble(ds_12.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_13 != null && ds_13.Tables[0].Rows.Count > 0)
        {
            d13 = Convert.ToDouble(ds_13.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_14 != null && ds_14.Tables[0].Rows.Count > 0)
        {
            d14 = Convert.ToDouble(ds_14.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_15 != null && ds_15.Tables[0].Rows.Count > 0)
        {
            d15 = Convert.ToDouble(ds_15.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_16 != null && ds_16.Tables[0].Rows.Count > 0)
        {
            d16 = Convert.ToDouble(ds_16.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds_17 != null && ds_17.Tables[0].Rows.Count > 0)
        {
            d17 = Convert.ToDouble(ds_17.Tables[0].Rows[0]["金额"].ToString());
        }
        if (ds8 != null && ds8.Tables[0].Rows.Count > 0)
        {
            d8 = Convert.ToDouble(ds8.Tables[0].Compute("sum(剩余金额)", "true"));
        }

        double xyed = d5 + d6 - d7 - (d1 + (d2 - d3) + (d4 + d4_2)) - d8 + d9 + d10 + d11 + d12 + d13 + d14 + d15 + d16 + d17;
        return xyed;
    }
    #endregion


    #region 获取本次业务可以免费的回收箱箱号
    /// <summary>
    /// 获取本次业务完成后，可变为免费的回收箱信息
    /// </summary>
    /// <param name="UserNumber">用户编号</param>
    /// <param name="dtkg_hg">合格旧硒鼓列表（必须包含空鼓类别、空鼓型号、使用数量字段）</param>
    /// <param name="dtkg_bhg">不合格旧硒鼓列表（必须包含空鼓类别、空鼓型号、使用数量字段）</param>
    /// <returns>可变为免费的回收箱信息列表，包含字段：接收办事处、回收箱号、关联用户编号、关联用户名称、服务站点分配时间、是否免费</returns>
    public DataTable GetFreeBox(string UserNumber, DataTable dtKG, DataTable dtBHGKG)
    {
        //获取与用户关联且尚未“免费”的回收箱信息
        DataSet dsBox = DbHelperSQL.Query("select jsbsc as 所属办事处,xh as 回收箱号,jsyhbh as 关联用户编号,jsyhmc as 关联用户名称,fwzdfpsj as 服务站点分配时间,sfmf as 是否免费 from FM_JXGHSXB where jsyhbh='" + UserNumber + "' and xzzt='普通' and isnull(sfmf,'')<>'是' order by fwzdfpsj");


        if (dsBox != null && dsBox.Tables[0].Rows.Count > 0)
        {
            //判断本次业务使用的合格旧硒鼓可使哪些回收箱变为免费
            if (dtKG != null && dtKG.Rows.Count > 0)
            {
                for (int i = 0; i < dtKG.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dtKG.Rows[i]["使用数量"]) > 0)
                    {
                        for (int j = 0; j < dsBox.Tables[0].Rows.Count; j++)
                        {
                            //如果回收箱已经是免费状态，则不用进行任何处理，否则判断此业务产生后回收箱是否可以免费
                            if (dsBox.Tables[0].Rows[j]["是否免费"].ToString() != "是" && Convert.ToInt32(dtKG.Rows[i]["使用数量"]) > 0)
                            {
                                //获取回收箱是否有同类型、型号的合格旧硒鼓回收
                                string sql_ruku = "select cast(TB007 as int) as 回收总量 from INVTB left join INVTA on TB002=TA002 and TB001=TA001 left join  CMSME on TA004=ME001 left join COPMA on INVTB.UDF01=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k101' and ME002 like '%办事处%' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "' and ltrim(rtrim(INVTB.UDF04))='" + dsBox.Tables[0].Rows[j]["回收箱号"].ToString() + "' and TA006='Y' and convert(varchar(10),convert(datetime,TA003),120)>='" + Convert.ToDateTime(dsBox.Tables[0].Rows[j]["服务站点分配时间"]).ToString("yyyy-MM-dd") + "' and ltrim(rtrim(INVTB.TB029))='" + dtKG.Rows[i]["空鼓类别"].ToString() + "' and  (case ltrim(rtrim(INVTB.TB029)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end)='" + dtKG.Rows[i]["空鼓型号"].ToString() + "'";

                                string sql_chuku = "select -cast(TB007 as int) as 回收总量 from INVTB left join INVTA on TB002=TA002 and TB001=TA001 left join  CMSME on TA004=ME001 left join COPMA on INVTB.UDF01=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k102' and ME002 like '%办事处%' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "' and TA006='Y' and INVTA.UDF01 not like '%用户直通车%' and ltrim(rtrim(INVTB.UDF04))='" + dsBox.Tables[0].Rows[j]["回收箱号"].ToString() + "' and TA006='Y' and convert(varchar(10),convert(datetime,TA003),120)>='" + Convert.ToDateTime(dsBox.Tables[0].Rows[j]["服务站点分配时间"]).ToString("yyyy-MM-dd") + "' and ltrim(rtrim(INVTB.TB029))='" + dtKG.Rows[i]["空鼓类别"].ToString() + "' and  (case ltrim(rtrim(INVTB.TB029)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end)='" + dtKG.Rows[i]["空鼓型号"].ToString() + "'";

                                string sql = "select isnull(sum(回收总量),0) as 回收总量 from (" + sql_ruku + " union all " + sql_chuku + ") as tab";

                                DataSet dsSL = GetErpData.GetDataSet(sql);
                                if (dsSL != null && dsSL.Tables[0].Rows.Count > 0 )
                                {
                                    if (Convert.ToInt32(dsSL.Tables[0].Rows[0]["回收总量"]) > 0)
                                    {
                                        dsBox.Tables[0].Rows[j]["是否免费"] = "是";
                                        dtKG.Rows[i]["使用数量"] = (Convert.ToInt32(dtKG.Rows[i]["使用数量"]) - 1).ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //针对用户不合格旧硒鼓循环购买，判断不合格旧硒鼓可使哪些回收箱变为免费
            if (dtBHGKG != null && dtBHGKG.Rows.Count > 0)
            {
                for (int i = 0; i < dtBHGKG.Rows.Count; i++)
                {
                    for (int j = 0; j < dsBox.Tables[0].Rows.Count; j++)
                    {
                        if (dsBox.Tables[0].Rows[j]["是否免费"].ToString() != "是" && Convert.ToInt32(dtBHGKG.Rows[i]["使用数量"]) > 0)
                        {

                            string sql = "select isnull(sum(yssl),0) as 回收总量 from FM_FWZDBHGJXGLRB as a left join YSXXXX as b on a.number=b.parentnumber where a.khbh='" + UserNumber + "' and sfyx='是' and hsxbh='" + dsBox.Tables[0].Rows[j]["回收箱号"].ToString() + "' and convert(varchar(10),createtime,120) >='" + Convert.ToDateTime(dsBox.Tables[0].Rows[j]["服务站点分配时间"]).ToString("yyyy-MM-dd") + "' and yslx='" + dtBHGKG.Rows[i]["空鼓类别"].ToString() + "' and ysxh='" + dtBHGKG.Rows[i]["空鼓型号"].ToString() + "'";
                            DataSet dsBHGSL = DbHelperSQL.Query(sql);
                            if (dsBHGSL != null && dsBHGSL.Tables[0].Rows.Count > 0)
                            {
                                if (Convert.ToInt32(dsBHGSL.Tables[0].Rows[0]["回收总量"]) > 0)
                                {
                                    dsBox.Tables[0].Rows[j]["是否免费"] = "是";
                                    dtBHGKG.Rows[i]["使用数量"] = (Convert.ToInt32(dtBHGKG.Rows[i]["使用数量"]) - 1).ToString();
                                }
                            }
                        }
                    }
                }
            }

            //获取免费的回收箱信息           
            DataTable dtFreeBox = dsBox.Tables[0].Clone();

            for (int i = 0; i < dsBox.Tables[0].Rows.Count; i++)
            {
                if (dsBox.Tables[0].Rows[i]["是否免费"].ToString() == "是")
                {
                    DataRow adtr = dtFreeBox.NewRow();
                    adtr.ItemArray = dsBox.Tables[0].Rows[i].ItemArray;
                    dtFreeBox.Rows.Add(adtr);
                }
            }
            return dtFreeBox;
        }

        return null;
    }

    /// <summary>
    /// 更新回收箱分配信息，并保存回收箱免费详情
    /// </summary>
    /// <param name="UserNumber">用户编号</param>
    /// <param name="dtKG">合格旧硒鼓列表（必须包含空鼓类别、空鼓型号、使用数量字段）</param>
    /// <param name="dtBHGKG">不合格旧硒鼓列表（必须包含空鼓类别、空鼓型号、使用数量字段）</param>
    /// <param name="ywlx">当前业务类型，包括：富美直供订单、出售给富美、旧硒鼓兑换礼品、交易卖出</param>
    /// <param name="ywdh">当前业务的单号</param>
    public void SetFreeBox(string UserNumber, DataTable dtKG, DataTable dtBHGKG, string ywlx, string ywdh)
    {
        ArrayList alist = new ArrayList();
        DataTable dtFreeBox = GetFreeBox(UserNumber, dtKG, dtBHGKG);
        if (dtFreeBox != null && dtFreeBox.Rows.Count > 0)
        {
            for (int i = 0; i < dtFreeBox.Rows.Count; i++)
            {
                string sql_up = "update FM_JXGHSXB set sfmf='是' where xh='" + dtFreeBox.Rows[i]["回收箱号"].ToString() + "'";
                alist.Add(sql_up);

                WorkFlowModule WFMBox = new WorkFlowModule("FM_JXGHSXMFXXB");
                string KeyNumberBox = WFMBox.numberFormat.GetNextNumber();
                string sql_insert = "insert into FM_JXGHSXMFXXB(number,ssbsc,hsxh,glyhbh,glyhmc,fwzdfpsj,mfsj,sqmfywlx,sqmfywdh,checkstate,createuser,createtime) values ('" + KeyNumberBox + "','" + dtFreeBox.Rows[i]["所属办事处"].ToString() + "','" + dtFreeBox.Rows[i]["回收箱号"].ToString() + "','" + dtFreeBox.Rows[i]["关联用户编号"].ToString() + "','" + dtFreeBox.Rows[i]["关联用户名称"].ToString() + "','" + dtFreeBox.Rows[i]["服务站点分配时间"].ToString() + "',getdate(),'" + ywlx + "','" + ywdh + "','1','" + UserNumber + "',getdate()) ";
                alist.Add(sql_insert);
            }
        }
        if (alist.Count > 0)
        {
            try
            {
                DbHelperSQL.ExecuteSqlTran(alist);
            }
            catch
            {
                ;
            }

        }
    }
   /// <summary>
    ///获取本次业务可以免费的回收箱箱号，适用于交易中心 liujie 2012-02-21
   /// </summary>
   /// <param name="UserNumber">用户编号《终端用户》</param>
   /// <param name="kglb">空鼓类别(蓝装、绿装)</param>
   /// <param name="kgxh">空鼓型号(规格）</param>
   /// <param name="sl">数量</param>
   /// <returns></returns>
    public DataTable GetFreeOneBox(string UserNumber, string kglb, string kgxh, double sl)
    {
        //获取与用户关联且尚未“免费”的回收箱信息
        DataSet dsBox = DbHelperSQL.Query("select jsbsc as 所属办事处,xh as 回收箱号,jsyhbh as 关联用户编号,jsyhmc as 关联用户名称,fwzdfpsj as 服务站点分配时间,sfmf as 是否免费 from FM_JXGHSXB where jsyhbh='" + UserNumber + "' and xzzt='普通' and isnull(sfmf,'')<>'是' order by fwzdfpsj");


        if (dsBox != null && dsBox.Tables[0].Rows.Count > 0)
        {
            ////判断本次业务使用的合格旧硒鼓可使哪些回收箱变为免费

            if (sl > 0)
            {
                for (int j = 0; j < dsBox.Tables[0].Rows.Count; j++)
                {
                    //如果回收箱已经是免费状态，则不用进行任何处理，否则判断此业务产生后回收箱是否可以免费
                    if (dsBox.Tables[0].Rows[j]["是否免费"].ToString() != "是" && sl > 0)
                    {
                        //获取回收箱是否有同类型、型号的合格旧硒鼓回收
                        string sql_ruku = "select cast(TB007 as int) as 回收总量 from INVTB left join INVTA on TB002=TA002 and TB001=TA001 left join  CMSME on TA004=ME001 left join COPMA on INVTB.UDF01=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k101' and ME002 like '%办事处%' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "' and ltrim(rtrim(INVTB.UDF04))='" + dsBox.Tables[0].Rows[j]["回收箱号"].ToString() + "' and TA006='Y' and convert(varchar(10),convert(datetime,TA003),120)>='" + Convert.ToDateTime(dsBox.Tables[0].Rows[j]["服务站点分配时间"]).ToString("yyyy-MM-dd") + "' and ltrim(rtrim(INVTB.TB029))='" + kglb.Trim() + "' and  (case ltrim(rtrim(INVTB.TB029)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end)='" + kgxh.Trim() + "'";

                        string sql_chuku = "select -cast(TB007 as int) as 回收总量 from INVTB left join INVTA on TB002=TA002 and TB001=TA001 left join  CMSME on TA004=ME001 left join COPMA on INVTB.UDF01=MA001 left join INVMB on ltrim(rtrim(MB001))=LTrim(RTrim(TB004)) left join KGCPDZB on KGPH=LTrim(RTrim(TB004))  where TA001='k102' and ME002 like '%办事处%' and ltrim(rtrim(INVTB.UDF01))='" + UserNumber + "' and TA006='Y' and INVTA.UDF01 not like '%用户直通车%' and ltrim(rtrim(INVTB.UDF04))='" + dsBox.Tables[0].Rows[j]["回收箱号"].ToString() + "' and TA006='Y' and convert(varchar(10),convert(datetime,TA003),120)>='" + Convert.ToDateTime(dsBox.Tables[0].Rows[j]["服务站点分配时间"]).ToString("yyyy-MM-dd") + "' and ltrim(rtrim(INVTB.TB029))='" + kglb.Trim() + "' and  (case ltrim(rtrim(INVTB.TB029)) when '原装' then DYCPGG else (case  when charindex('(',MB003)>0 then ltrim(rtrim(substring(MB003,0,charindex('(',MB003)))) else ltrim(rtrim(MB003)) end) end)='" + kgxh.Trim() + "'";

                        string sql = "select isnull(sum(回收总量),0) as 回收总量 from (" + sql_ruku + " union all " + sql_chuku + ") as tab";

                        DataSet dsSL = GetErpData.GetDataSet(sql);

                       if (dsSL != null && dsSL.Tables[0].Rows.Count > 0 )
                       {
                            if (Convert.ToInt32(dsSL.Tables[0].Rows[0]["回收总量"]) > 0)
                            {
                                dsBox.Tables[0].Rows[j]["是否免费"] = "是"; //判断条件
                                sl = sl - 1; //循环变量
                            }
                        }
                    }
                }
            }

            //获取免费的回收箱信息           
            DataTable dtFreeBox = dsBox.Tables[0].Clone();

            for (int i = 0; i < dsBox.Tables[0].Rows.Count; i++)
            {
                if (dsBox.Tables[0].Rows[i]["是否免费"].ToString() == "是")
                {
                    DataRow adtr = dtFreeBox.NewRow();
                    adtr.ItemArray = dsBox.Tables[0].Rows[i].ItemArray;
                    dtFreeBox.Rows.Add(adtr);
                }
            }
            return dtFreeBox;
        }

        return null;
    }


 /// <summary>
 /// 获取本次业务可以免费的回收箱箱号，用户《交易中心》  liujie 2012-02-21
 /// <param name="UserNumber">用户编号（终端用户)</param>
 /// <param name="kglb">空鼓类别</param>
 /// <param name="kgxh">空鼓型号</param>
 /// <param name="sl">数量</param>
 /// <param name="ywlx">当前业务类型，包括：富美直供订单、出售给富美、旧硒鼓兑换礼品、交易卖出</param>
 /// <param name="ywdh">当前业务的单号</param>
 /// <returns></returns>
    public  bool GetFreeOne(string UserNumber, string kglb, string kgxh, double sl, string ywlx, string ywdh)
    {
        ArrayList alist = new ArrayList();
        DataTable dtFreeBox = GetFreeOneBox(UserNumber, kglb, kgxh, sl);


        if (dtFreeBox != null && dtFreeBox.Rows.Count > 0)
        {
            for (int i = 0; i < dtFreeBox.Rows.Count; i++)
            {
                string sql_up = "update FM_JXGHSXB set sfmf='是' where xh='" + dtFreeBox.Rows[i]["回收箱号"].ToString() + "'";
                alist.Add(sql_up);

                WorkFlowModule WFMBox = new WorkFlowModule("FM_JXGHSXMFXXB");
                string KeyNumberBox = WFMBox.numberFormat.GetNextNumber();
                string sql_insert = "insert into FM_JXGHSXMFXXB(number,ssbsc,hsxh,glyhbh,glyhmc,fwzdfpsj,mfsj,sqmfywlx,sqmfywdh,checkstate,createuser,createtime) values ('" + KeyNumberBox + "','" + dtFreeBox.Rows[i]["所属办事处"].ToString() + "','" + dtFreeBox.Rows[i]["回收箱号"].ToString() + "','" + dtFreeBox.Rows[i]["关联用户编号"].ToString() + "','" + dtFreeBox.Rows[i]["关联用户名称"].ToString() + "','" + dtFreeBox.Rows[i]["服务站点分配时间"].ToString() + "',getdate(),'" + ywlx + "','" + ywdh + "','1','" + UserNumber + "',getdate()) ";
                alist.Add(sql_insert);
            }
        }
        Boolean b = true;
        if (alist.Count > 0)
        {
            try
            {
                b = DbHelperSQL.ExecSqlTran(alist);//  加入了新的类方法
            }
            catch (Exception ex)
            {
                b = false;
            }

        }
        return b;
    }
    /// <summary>
    /// 获取本次业务可以免费的回收箱箱号，用户《交易中心》  liujie 2012-02-21
    /// <param name="UserNumber">用户编号（终端用户)</param>
    /// <param name="kglb">空鼓类别</param>
    /// <param name="kgxh">空鼓型号</param>
    /// <param name="sl">数量</param>
    /// <param name="ywlx">当前业务类型，包括：富美直供订单、出售给富美、旧硒鼓兑换礼品、交易卖出</param>
    /// <param name="ywdh">当前业务的单号</param>
    /// <returns>arraylist</returns>
    public ArrayList GetFreeOneA(string UserNumber, string kglb, string kgxh, double sl, string ywlx, string ywdh)
    {
        ArrayList alist = new ArrayList();
        DataTable dtFreeBox = GetFreeOneBox(UserNumber, kglb, kgxh, sl);
        if (dtFreeBox != null && dtFreeBox.Rows.Count > 0)
        {
            for (int i = 0; i < dtFreeBox.Rows.Count; i++)
            {
                string sql_up = "update FM_JXGHSXB set sfmf='是' where xh='" + dtFreeBox.Rows[i]["回收箱号"].ToString() + "'";
                alist.Add(sql_up);

                WorkFlowModule WFMBox = new WorkFlowModule("FM_JXGHSXMFXXB");
                string KeyNumberBox = WFMBox.numberFormat.GetNextNumber();
                string sql_insert = "insert into FM_JXGHSXMFXXB(number,ssbsc,hsxh,glyhbh,glyhmc,fwzdfpsj,mfsj,sqmfywlx,sqmfywdh,checkstate,createuser,createtime) values ('" + KeyNumberBox + "','" + dtFreeBox.Rows[i]["所属办事处"].ToString() + "','" + dtFreeBox.Rows[i]["回收箱号"].ToString() + "','" + dtFreeBox.Rows[i]["关联用户编号"].ToString() + "','" + dtFreeBox.Rows[i]["关联用户名称"].ToString() + "','" + dtFreeBox.Rows[i]["服务站点分配时间"].ToString() + "',getdate(),'" + ywlx + "','" + ywdh + "','1','" + UserNumber + "',getdate()) ";
                alist.Add(sql_insert);
            }
        }
        return alist;
    }
    /// <summary>
    /// 测试效果
    /// </summary>
    /// <param name="UserNumber"></param>
    /// <param name="kglb"></param>
    /// <param name="kgxh"></param>
    /// <param name="sl"></param>
    /// <param name="ywlx"></param>
    /// <param name="ywdh"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    public string[] GetFreeOneB(string UserNumber, string kglb, string kgxh, string sl, string ywlx, string ywdh, string pass)
    {

        ArrayList al = new ArrayList();
        al = null;
        if (String.IsNullOrEmpty(UserNumber) || String.IsNullOrEmpty(kglb) || String.IsNullOrEmpty(kgxh) || String.IsNullOrEmpty(ywdh) || String.IsNullOrEmpty(pass) || String.IsNullOrEmpty(sl))
        {
            return null;
        }
        if (pass == "fmFun")
        {

            try
            {
                KGclassYH ky = new KGclassYH();
                al = ky.GetFreeOneA(UserNumber, kglb, kgxh, Convert.ToDouble(sl), ywlx, ywdh);
                string[] a = new string[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    a[i] = al[i].ToString();
                }
                return a;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

 

   /// <summary>
   /// 获取更新回收箱状态的sql语句
   /// </summary>
   /// <param name="UserNumber">用户编号</param>
    /// <param name="dtKG">合格旧硒鼓列表（必须包含空鼓类别、空鼓型号、使用数量字段）</param>
    /// <param name="dtBHGKG">不合格旧硒鼓列表（必须包含空鼓类别、空鼓型号、使用数量字段）</param>
    /// <param name="ywlx">当前业务类型，包括：富美直供订单、出售给富美、旧硒鼓兑换礼品、交易卖出</param>
    /// <param name="ywdh">当前业务的单号</param>
   /// <returns>更新回收箱状态的sql语句</returns>
    public ArrayList GeFreeBoxSql(string UserNumber, DataTable dtKG, DataTable dtBHGKG, string ywlx, string ywdh)
    {
        ArrayList alist = new ArrayList();
        DataTable dtFreeBox = GetFreeBox(UserNumber, dtKG, dtBHGKG);
        if (dtFreeBox != null && dtFreeBox.Rows.Count > 0)
        {
            for (int i = 0; i < dtFreeBox.Rows.Count; i++)
            {
                string sql_up = "update FM_JXGHSXB set sfmf='是' where xh='" + dtFreeBox.Rows[i]["回收箱号"].ToString() + "'";
                alist.Add(sql_up);

                WorkFlowModule WFMBox = new WorkFlowModule("FM_JXGHSXMFXXB");
                string KeyNumberBox = WFMBox.numberFormat.GetNextNumber();
                string sql_insert = "insert into FM_JXGHSXMFXXB(number,ssbsc,hsxh,glyhbh,glyhmc,fwzdfpsj,mfsj,sqmfywlx,sqmfywdh,checkstate,createuser,createtime) values ('" + KeyNumberBox + "','" + dtFreeBox.Rows[i]["所属办事处"].ToString() + "','" + dtFreeBox.Rows[i]["回收箱号"].ToString() + "','" + dtFreeBox.Rows[i]["关联用户编号"].ToString() + "','" + dtFreeBox.Rows[i]["关联用户名称"].ToString() + "','" + dtFreeBox.Rows[i]["服务站点分配时间"].ToString() + "',getdate(),'" + ywlx + "','" + ywdh + "','1','" + UserNumber + "',getdate()) ";
                alist.Add(sql_insert);
            }
        }
        return alist;      
    }
    #endregion



    
}