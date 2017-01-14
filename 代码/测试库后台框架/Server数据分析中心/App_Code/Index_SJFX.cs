using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


/// <summary>
/// Index_SJFX 的摘要说明
/// </summary>
public class Index_SJFX
{
	public Index_SJFX()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    private static DataSet ReturnDt()
    {
        DataSet dsreturn = new DataSet();
        DataTable rtdt = new DataTable("返回值单条");
        rtdt.Columns.Add(new DataColumn("执行结果", typeof(string)));
        rtdt.Columns.Add(new DataColumn("提示文本", typeof(string)));
        //rtdt.Rows.Add(rtdt.NewRow());
        dsreturn.Tables.Add(rtdt);
        return dsreturn;
    }
    /// <summary>
    /// 获得成交信息数据集，首页列表显示
    /// </summary>
    /// <returns></returns>
    public static DataSet jhjx_cjxx_XTKZ()
    {
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
           // string Strsql = "select  Z_SPBH as '商品编号' ,Z_SPMC as '商品名称' ,Z_JJDW as '计价单位',Z_DBSJ as '定标时间',Z_ZBSL as '定标数量',Z_ZBJG as '定标价格',Z_ZBJE as '定标金额',Z_HTQX as '合同期限',(select  isnull(sum(isnull(T_THSL,0)),0) from   AAA_THDYFHDXXB where ZBDBXXBBH = AAA_ZBDBXXB.Number and F_DQZT <>'撤销') as '已提货数量', (select  isnull(sum(ISNULL(T_THSL,0)),0)  as aa  from   AAA_THDYFHDXXB where ZBDBXXBBH =AAA_ZBDBXXB.Number  and (F_DQZT <>'撤销' and F_DQZT <>'未生成发货单' and F_DQZT <>'已生成发货单')) as '已发货数量', case  isnull (Z_QPZT,'') when '' then '---' when '未开始清盘' then '---' else  ISNULL(CONVERT(varchar(100), Z_QPKSSJ, 20),'---') end as 清盘时间,case when  isnull (Z_QPZT,'') =  '' then '---' when isnull (Z_QPZT,'') =  '未开始清盘' then '未开始清盘' when CONVERT(varchar(100), Z_QPKSSJ, 20)=CONVERT(varchar(100), Z_QPJSSJ, 20) then '自动清盘'  else '人工清盘' end  as '清盘类型' from  AAA_ZBDBXXB where (Z_HTZT = '定标' or Z_HTZT = '定标合同终止' or Z_HTZT = '定标合同到期'  or Z_HTZT = '定标执行完成') and datediff(YEAR,Z_DBSJ,getdate())=0 order by Z_DBSJ desc";
            string Strsql = "select  Z_SPBH as '商品编号' ,Z_SPMC as '商品名称' ,Z_JJDW as '计价单位',Z_DBSJ as '定标时间',Z_ZBSL as '定标数量',Z_ZBJG as '定标价格',Z_ZBJE as '定标金额',Z_HTQX as '合同期限',(select  isnull(sum(isnull(T_THSL,0)),0) from   AAA_THDYFHDXXB where ZBDBXXBBH = AAA_ZBDBXXB.Number and F_DQZT <>'撤销') as '已提货数量', (select  isnull(sum(ISNULL(T_THSL,0)),0)  as aa  from   AAA_THDYFHDXXB where ZBDBXXBBH =AAA_ZBDBXXB.Number  and (F_DQZT <>'撤销' and F_DQZT <>'未生成发货单' and F_DQZT <>'已生成发货单')) as '已发货数量', case  isnull (Z_QPZT,'') when '' then '---' when '未开始清盘' then '---' else  ISNULL(CONVERT(varchar(100), Z_QPKSSJ, 20),'---') end as 清盘时间,case when  isnull (Z_QPZT,'') =  '' then '---' when isnull (Z_QPZT,'') =  '未开始清盘' then '未开始清盘' when CONVERT(varchar(100), Z_QPKSSJ, 20)=CONVERT(varchar(100), Z_QPJSSJ, 20) then '自动清盘'  else '人工清盘' end  as '清盘类型' from  AAA_ZBDBXXB where (Z_HTZT = '定标' or Z_HTZT = '定标合同终止' or Z_HTZT = '定标合同到期'  or Z_HTZT = '定标执行完成') and (Z_DBSJ  between dateadd(mm,-12,convert(varchar(10),getdate(),120)) and convert(varchar(10),getdate(),120)) order by Z_DBSJ desc";
            
            Hashtable returnht = I_DBL.RunProc(Strsql, "");
            if (!(bool)returnht["return_float"])
            {
                return null;
            }
            DataSet dsreturnnew = (DataSet)returnht["return_ds"];
            return dsreturnnew;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// 得到所有商品分类，有卖家、买家区分
    /// </summary>
    /// <returns></returns>
    public static DataSet GetSPFL_XTKZ(string sTable)
    {
        try
        {
             
            //Hashtable ht = I_DBL.RunProc("select SortID,SortName,SortParentID from AAA_tbMenuSPFL order by SortOrder", "");
         
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            DataSet DataSet_this = new DataSet();
            Hashtable return_ht = new Hashtable();
            Hashtable putin_ht = new Hashtable();
            putin_ht["sField"] = "SortID,SortName,SortParentID,SortParentPath,SortOrder,GoToUrl,GoToUrlParameter,ShowWho,TargetGo,ToolTipText";
            putin_ht["sTable"] = sTable;
            putin_ht["iSortID"] = 0;
            putin_ht["iCond"] = 1;
            return_ht = I_DBL.RunProc_CMD("sp_Util_Sort_SELECT", "分类表", putin_ht);
            if (Convert.ToBoolean(return_ht["return_float"]))
            {
                DataSet_this = (DataSet)return_ht["return_ds"];
                if (DataSet_this != null)
                {
                    return DataSet_this;

                }
                else
                {
                    return null;
                }
            }
            else
            {
                //显示错误
                //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;

            }


        }
        catch(Exception)
        {
            return null;
        }
    }
    /// <summary>
    /// 根据表明获取商品一级分类
    /// </summary>
    /// <param name="sTable">”AAA_tbMenuSPFL“：平台商品分类表</param>
    /// <returns></returns>
    public static DataSet test_dsfl(string sTable)
    {
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            DataSet DataSet_this = new DataSet();
            Hashtable return_ht = new Hashtable();
            Hashtable putin_ht = new Hashtable();
            putin_ht["sField"] = "SortID,SortName,SortParentID,SortParentPath,SortOrder,GoToUrl,GoToUrlParameter,ShowWho,TargetGo,ToolTipText";
            putin_ht["sTable"] = sTable;
            putin_ht["iSortID"] = 0;
            putin_ht["iCond"] = 4;
            return_ht = I_DBL.RunProc_CMD("sp_Util_Sort_SELECT", "", putin_ht);
            if (Convert.ToBoolean(return_ht["return_float"]))
            {
                DataSet_this = (DataSet)return_ht["return_ds"];
                if (DataSet_this != null)
                {
                    return DataSet_this;

                }
                else
                {
                    return null;
                }
            }
            else
            {
                //显示错误
                //MessageBox.Show(return_ht["return_errmsg"].ToString(), "数据库操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;

            }
        }
        catch(Exception)
        {
            return null;
        }

    }


    /// <summary>
    /// 商品详情_首次(2104新版)
    /// </summary>
    /// <param name="spbh">商品编号</param>
    /// <param name="htqx">合同期限</param>
    /// <param name="dlyx">登录邮箱</param>
    /// <returns></returns>
    public static DataSet SelectSPXQ_sc(string spbh, string htqx,string dlyx)
    {
        
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable htCS = new Hashtable();
        htCS["@SPBH"] = spbh;
        htCS["@DLYX"] = dlyx;
        DataSet dsreturn = new DataSet();

        //获取走势图
        string sql_zst = "select 中标时间 as X轴数据,中标价格 as Y轴数据 , '' as 标签附加文字, '' as 其他 from  (select  top 30 Z_ZBSJ as 中标时间,Z_ZBJG 中标价格 from AAA_ZBDBXXB where Z_SPBH=@SPBH and Z_HTQX = @HTQX ) as tab1  order by X轴数据 desc";

        htCS["@HTQX"] = "即时";
        Hashtable returnHT = I_DBL.RunParam_SQL(sql_zst, "数据点_即时", htCS);
        DataSet tb_zst = (DataSet)returnHT["return_ds"];
        dsreturn.Tables.Add(tb_zst.Tables["数据点_即时"].Copy());

        htCS["@HTQX"] = "三个月";
        Hashtable returnHT0 = I_DBL.RunParam_SQL(sql_zst, "数据点_三个月", htCS);
        DataSet tb_zst0 = (DataSet)returnHT0["return_ds"];
        dsreturn.Tables.Add(tb_zst0.Tables["数据点_三个月"].Copy());

        htCS["@HTQX"] = "一年";
        Hashtable returnHT1 = I_DBL.RunParam_SQL(sql_zst, "数据点_一年", htCS);
        DataSet tb_zst1 = (DataSet)returnHT1["return_ds"];
        dsreturn.Tables.Add(tb_zst1.Tables["数据点_一年"].Copy());





        //商品描述
        string sql_spinfo = "select SPBH as 商品编号,SPMC as 商品名称,GG as 规格标准,SPMS as 商品描述,'' as 商品性能,'' as 商品图片  from AAA_PTSPXXB where SPBH = @SPBH";

        Hashtable returnHT2 = I_DBL.RunParam_SQL(sql_spinfo, "商品信息", htCS);
        DataSet tb_spinfo = (DataSet)returnHT2["return_ds"];
        dsreturn.Tables.Add(tb_spinfo.Tables["商品信息"].Copy());

        //竟标信息
        if(htqx != "")
        {
            
            string sql_jbxx = "select  DP.商品编号  as 商品编号, DP.当前买家最高价 as 最高买入价, DP.当前卖家最低价 as 最低卖出价, DP.最低价标的经济批量 as 经济批量, DP.最低价标的投标拟售量  as 拟售量, DP.当前集合预订量 as 集合预订量, DP.当前拟订购总量 as 拟订购总量, DP.[达成率/中标率] as  达成率, DP.最低价标的日均最高供货量 as 日均最高供货量, DP.合同期限 as 合同期限,  DP.最低价投标单号n as 投标单号,DP.最低价投标单发票税率n as 发票税率, DP.当前卖方名称 as 卖方名称, DP.当前卖方信用等级 as 卖方信用,DP.当前商品产地 as 商品产地, TBD.GHQY as 供货区域, DP.当前投标轮次 as 竟标轮次, DP.竞标状态 as 状态,DP.上轮定标价 as 上轮定标价, DP.升降幅 as 升降幅, DP.买家当前数量 as 买方数量,DP.卖家当前数量 as 卖方数量, DP.买家今日新增数量 as 买方新增, DP.卖家今日新增数量 as 卖方新增,DP.买家区域覆盖率 as 买方区域覆盖,DP.卖家区域覆盖率 as 卖方区域覆盖 from  AAA_DaPanPrd_New as DP  left join AAA_TBD as TBD on DP.最低价投标单号n = TBD.Number   where DP.商品编号 = @SPBH  and DP.合同期限 = @HTQX";
            htCS["@HTQX"] = htqx;
            Hashtable returnHT3 = I_DBL.RunParam_SQL(sql_jbxx, "竟标信息", htCS);
            DataSet tb_jbxx = (DataSet)returnHT3["return_ds"];
            dsreturn.Tables.Add(tb_jbxx.Tables["竟标信息"].Copy());
        }


        //右侧各种
        string sql_y = " select *, ";
        sql_y = sql_y + " (select isnull(sum(TBNSL),0) as 拟售量 from AAA_TBD where TJSJ >= DATEADD(MM,-6,GETDATE()) and SPBH = @SPBH and   DLYX = @DLYX) as 拟售量, ";
        sql_y = sql_y + "  (select isnull(sum(NDGSL),0) as 拟订购总量 from AAA_YDDXXB where TJSJ >= DATEADD(MM,-6,GETDATE()) and SPBH = @SPBH and DLYX = @DLYX) as 拟订购总量, ";
        sql_y = sql_y + " (select count(Number) as 拟售次数 from AAA_TBD where DLYX = @DLYX and SPBH = @SPBH  ) as 拟售次数, ";
        sql_y = sql_y + " (select count(Number) as 拟订购次数 from AAA_YDDXXB where DLYX = @DLYX and SPBH = @SPBH  ) as 拟订购次数 ";
        sql_y = sql_y + "  from  ";
        sql_y = sql_y + " ( ";
        sql_y = sql_y + "    select  isnull(max(Z_ZB_JE),0) as  最大值, isnull(min(Z_ZB_JE),0) as  最小值 from (select distinct top 10   Z_LC_number,Z_ZB_JE,Z_DBSJ   from AAA_ZBDBXXB  where Z_HTZT in('定标','定标合同到期','定标合同终止','定标执行完成') and Z_SPBH =@SPBH and (Y_YSYDDDLYX = @DLYX or T_YSTBDDLYX = @DLYX) order by Z_DBSJ desc) as tab1  ";
        sql_y = sql_y + "  ) as tab0  ";

        sql_y = sql_y + "  join    ";
        sql_y = sql_y + "  ( ";
        sql_y = sql_y + "     select count(Number) as 买入次数,isnull(sum(Z_ZBJE),0) as 买入金额 from AAA_ZBDBXXB where Z_HTZT in('定标','定标合同到期','定标合同终止','定标执行完成') and  Y_YSYDDDLYX = @DLYX and Z_SPBH = @SPBH ";
        sql_y = sql_y + "  ) as tab1  ";
        sql_y = sql_y + "  on 1=1 ";
        sql_y = sql_y + "    join    ";
        sql_y = sql_y + "   ( ";
        sql_y = sql_y + " select count(Number) as 卖出次数,isnull(sum(Z_ZBJE),0) as 卖出金额 from AAA_ZBDBXXB where Z_HTZT in('定标','定标合同到期','定标合同终止','定标执行完成') and  T_YSTBDDLYX = @DLYX  and Z_SPBH = @SPBH ";
        sql_y = sql_y + "  ) as tab2 ";
        sql_y = sql_y + "  on 1=1  ";
        Hashtable returnHT4 = I_DBL.RunParam_SQL(sql_y, "右侧各种", htCS);
        DataSet tb_jbxx4 = (DataSet)returnHT4["return_ds"];
        dsreturn.Tables.Add(tb_jbxx4.Tables["右侧各种"].Copy());



        return dsreturn;
    }



    /// <summary>
    /// 商品详情
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public static DataSet SelectSPXQ(DataTable ds)
    {
        //初始化返回值,先塞一行数据
        DataSet returnDS = ReturnDt();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        try
        {
            if (ds != null && ds.Rows.Count > 0)
            {
                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

                Hashtable htCS = new Hashtable();
                htCS["@SPBH"] = ds.Rows[0]["商品编号"].ToString();
                htCS["@HTQX"] = ds.Rows[0]["合同期限"].ToString();

                #region//商品的历史价格走势
                string strSQL = "select top 100 isnull(isnull(即时竞标时间,三个月竞标时间),一年竞标时间) 时间,即时交易,三个月合同,一年合同 from (select Z_ZBSJ as 即时竞标时间,Z_ZBJG 即时交易 from AAA_ZBDBXXB where Z_SPBH=@SPBH and Z_HTQX = '即时' group by Z_HTQX,Z_SPBH,Z_SPMC,Z_ZBJG,Z_ZBSJ) a full join (select Z_ZBSJ 三个月竞标时间,Z_ZBJG 三个月合同 from AAA_ZBDBXXB where Z_SPBH=@SPBH and Z_HTQX = '三个月' group by Z_HTQX,Z_SPBH,Z_SPMC,Z_ZBJG,Z_ZBSJ) b on a.即时竞标时间=b.三个月竞标时间 full join (select Z_ZBSJ  一年竞标时间,Z_ZBJG 一年合同 from AAA_ZBDBXXB where Z_SPBH=@SPBH and Z_HTQX = '一年' group by Z_HTQX,Z_SPBH,Z_SPMC,Z_ZBJG,Z_ZBSJ) c on a.即时竞标时间=c.一年竞标时间 order by 时间;";
                #endregion

                //商品详情
                string strSPXQ = "select * from AAA_DaPanPrd_New where 商品编号=@SPBH and 合同周期n=@HTQX ;";

                //卖家信息
                string strSelXX = "select top 1 GHQY as 供货区域,DLYX,SLZM as '税率证明','卖出方名称'=(select I_JYFMC from AAA_DLZHXXB b where b.B_DLYX =a.DLYX),'账户当前信用分值'=(select isnull(B_ZHDQXYFZ,0) from AAA_DLZHXXB b where b.B_DLYX =a.DLYX) from AAA_TBD a where ZT='竞标' and SPBH=@SPBH and HTQX=@HTQX order by TBJG,TJSJ;";

                //商品描述
                string strSPMS = "select SPMS 商品描述 from AAA_PTSPXXB where SPBH=@SPBH;";

                string str = strSQL + strSPXQ + strSelXX + strSPMS;
                Hashtable returnHT =I_DBL.RunParam_SQL( str,"",htCS);
                if (!(bool)returnHT["return_float"])
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT["return_errmsg"].ToString();
                    return returnDS;
                }
                DataSet tble = (DataSet)returnHT["return_ds"];

                DataTable dtJGZS = tble.Tables[0].Copy();
                dtJGZS.TableName = "价格走势";
                returnDS.Tables.Add(dtJGZS);

                DataTable dtSPXQ = tble.Tables[1].Copy();
                dtSPXQ.TableName = "商品详情";
                returnDS.Tables.Add(dtSPXQ);

                DataTable dtSelXX = tble.Tables[2].Copy();
                dtSelXX.TableName = "卖家信息";
                returnDS.Tables.Add(dtSelXX);

                DataTable dsSPMS = tble.Tables[3].Copy();
                dsSPMS.TableName = "商品描述";
                returnDS.Tables.Add(dsSPMS);

                //不供货区域
                string strSQLBGHQY = "";
                if (dtSelXX != null && dtSelXX.Rows.Count > 0)
                {
                    string strGHQY = dtSelXX.Rows[0]["供货区域"].ToString().Replace("|", "','");
                    strGHQY = strGHQY.Substring(2, strGHQY.Length - 4);
                    strSQLBGHQY = "select p_namestr from AAA_CityList_Promary where  p_namestr not in (" + strGHQY + ");";
                }
                else
                {
                    strSQLBGHQY = "select p_namestr from AAA_CityList_Promary;";
                }
                Hashtable returnHT1 = I_DBL.RunProc(strSQLBGHQY, "");
                if (!(bool)returnHT1["return_float"])
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT["return_errmsg"].ToString();
                    return returnDS;
                }
                DataTable dsBGHQY = ((DataSet)returnHT1["return_ds"]).Tables[0].Copy();
                dsBGHQY.TableName = "不供货区域";
                returnDS.Tables.Add(dsBGHQY);
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] ="查询成功";
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";

            }
        }
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = e.Message;

        }


        return returnDS;
    }
}