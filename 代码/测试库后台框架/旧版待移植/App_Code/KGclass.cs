using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using FMOP.DB;
using System.Collections;
using Hesion.Brick.Core.WorkFlow;
/// <summary>
///可用剩余旧硒鼓统计分析类
/// </summary>
public class KGclass
{
    //2013年3月新财年，旧硒鼓新建了期初，新财年之后的数据行计算。
    #region 获取服务商当前可用空鼓数量  
    /// <summary>
    /// 获取所有空鼓入库的信息（期初+交付合格）
    /// </summary>
    /// <param name="UserNumber"></param>
    /// <returns></returns>
    public DataTable GetTable_Ru(string UserNumber,string qcsj)
    {
        //获取期初;
        string sqlQC = "select a.KHBH as 客户编号,b.KGLB as 空鼓类型,KGGG as 空鼓规格,QCJYL as 剩余总量 from CWGL_KHKGSYQKQCSJ as a left join CWGL_KHKGSYQKQCSJ_KGQCXX as b on a.Number=b.parentNumber where a.KHBH='" + UserNumber + "' and a.QCSJ='" + qcsj + "'";

        //获取所有交付的旧硒鼓中的合格信息
        string sqlJFHG = "select fwsbh as 客户编号,jxglb as 空鼓类型,dycpxh as 空鼓规格,hgsl as 剩余总量 from FWPT_FWSJFJXGMXB as a left join FWPT_FWSJFJXGMXB_JXGLB as b on a.number=b.parentnumber where a.fwsbh='"+UserNumber +"' and  a.CheckZT='已审核' and djzt='有效' and convert(varchar(10),CheckSJ,120)>='"+qcsj+"'";

        string sql = "select 客户编号,空鼓类型,空鼓规格,sum(剩余总量) as 剩余总量 from (" + sqlQC + " union all " + sqlJFHG + " ) as tab group by 客户编号,空鼓类型,空鼓规格";

        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }   

    /// <summary>
    /// 服务商空鼓使用明细表中的2013-03-01之后的数据，包括循环提货、返还押金、返还授信
    /// </summary>
    /// <param name="UserNumber"></param>
    /// <returns></returns>
    public DataTable GetTable_Used(string UserNumber,string qcsj)
    {
        string sql = "select fwsbh as 客户编号,(case 空鼓类别 when 'YZ' then '原装' when 'LANZ' then '蓝装' when 'LVZ' then '绿装' when 'HZ' then '红装' end) as 空鼓类型,kgxh as 空鼓规格,-sum(空鼓数量) as 剩余总量   from (select * from CWGL_KGSYMXJLB where FWSBH ='" + UserNumber + "' and convert(varchar(10), sysj,120)>='"+qcsj+"') as t unpivot (空鼓数量 for 空鼓类别 in ([YZ],[LANZ],[LVZ],[HZ])) as r  group by FWSBH,KGXH,空鼓类别 having SUM(空鼓数量)>0 ";
        DataSet dt1 = DbHelperSQL.Query(sql);
       
        if (dt1 != null && dt1.Tables!=null)
        {
            return dt1.Tables[0];
        }
        return null;
    }
   
 
   //获取交易大厅中的旧硒鼓使用情况2013-03-01之后的数据
    public DataTable GetTable_ECTC(string UserNumber,string qcsj)
    {
        //合格旧硒鼓卖出信息发布，空鼓冻结
        string sql_dj = "select MCFKHBH as 客户编号,LX as 空鼓类型,XH as 空鼓规格, -SYKMSL as 剩余总量   from Newtransfer where MCFKHBH='" + UserNumber + "' and JYZT <>'5' and JYZT <>'6'  and convert(varchar(10),CreateTime,120)>='"+qcsj+"' ";

        //新版合格旧硒鼓交易中心数据的计算
        string sql_sale = "select MCKHBH as 客户编号,CJLX as 空鼓类型,CJXH as 空鼓规格, -CJSL as 剩余总量 from SuccedTrade where MCKHBH='" + UserNumber + "' and convert(varchar(10),CreateTime,120)>='"+qcsj+"'";
        string sql_buy="select MRKHBH as 客户编号,CJLX as 空鼓类型,CJXH as 空鼓规格, CJSL as 剩余总量  from SuccedTrade where MRKHBH='" + UserNumber + "' and convert(varchar(10),CreateTime,120)>='"+qcsj+"' ";

        string sql = "select 客户编号,空鼓类型,空鼓规格,sum(剩余总量) as 剩余总量 from ( " + sql_dj + " union all " + sql_sale + " union all " + sql_buy + ") as tab group by 客户编号,空鼓类型,空鼓规格 ";
        DataSet dt1 = DbHelperSQL.Query(sql);
        if (dt1 != null && dt1.Tables != null)
        {
            return dt1.Tables[0];
        }
        return null;
    }

    //获取服务商空鼓调整表中的数据2013-03-01之后的数据
    public DataTable GetTable1_TiaoZheng(string UserNumber,string qcsj)
    {
        //服务商空鼓调整表数据
        string sql_ru = "select fwsbh as 客户编号,kglb as 空鼓类型,kggg as 空鼓规格,tzsl as 剩余总量 from CWGL_FWSKGTZB where tzlx='调入' and  fwsbh='" + UserNumber + "' and shzt='已审核'  and convert(varchar(10),CreateTime,120)>='"+qcsj+"'  ";
        string sql_chu = "select fwsbh as 客户编号,kglb as 空鼓类型,kggg as 空鼓规格,-tzsl as 剩余总量 from CWGL_FWSKGTZB where tzlx='调出' and fwsbh='" + UserNumber + "' and shzt='已审核'  and convert(varchar(10),CreateTime,120)>='"+qcsj+"' ";

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
    public DataTable MergeTable(DataTable dt1, DataTable dt2, DataTable dt3, DataTable dt4)
    {
        DataTable dtmt = dt1.Clone();
        dtmt.Merge(dt1, false, MissingSchemaAction.Ignore);      
        dtmt.Merge(dt2, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt3, false, MissingSchemaAction.Ignore);
        dtmt.Merge(dt4, false, MissingSchemaAction.Ignore);     
        return dtmt;
    }

    /// <summary>
    /// 获取某客户所有可以使用的空鼓数量,列： 型号，类型，型号，数量
    /// </summary>
    /// <param name="UserNumber">服务商编号</param>
    ///  <param name="dt7">产品列表已验证的部分使用的空鼓信息</param>
    /// <returns></returns>
    public DataTable GetAllKGCanUseByUserNumber(string UserNumber,DataTable dtNow)
    {
        string qcsj = "2013-03-01";
        DataTable dt1 = GetTable_Ru(UserNumber,qcsj);//空鼓入库和期初      
        DataTable dt2 = GetTable_Used(UserNumber,qcsj);//空鼓使用明细表中的数据   
        DataTable dt3 = GetTable_ECTC(UserNumber,qcsj);//交易大厅中的数据
        DataTable dt4 = GetTable1_TiaoZheng(UserNumber,qcsj);//服务商空鼓调整表的数据   
      
        //合并几个表格
        DataTable dt_mt = MergeTable(dt1,dt2,dt3, dt4);
        
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
        
        DataTable dt_mt_js = dt_mt.Clone();
        //开始数量合并
        for (int i = 0; i < dt_mt.Rows.Count; i++)
        {           

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
    public DataTable GetAllKGCanUseByWhere(string UserNumber, string lx, string xh,DataTable dt7)
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

        DataTable dt = GetAllKGCanUseByUserNumber(UserNumber,dt7);
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

    #region 服务商自有空鼓扣减拆分功，空鼓不足返回提醒字符串
   /// <summary>   
   /// 普通型号的空鼓扣减拆分
   /// </summary>
   /// <param name="fwsbh">服务商编号</param>
   /// <param name="cplb">产品类别</param>
   /// <param name="cpxh">产品型号</param>
   /// <param name="cpsl">产品数量</param>
   /// <param name="dtkg">用于存放空鼓拆分结果的数据表</param>
   /// <param name="msg">数量不足时返回的提醒字符串</param>
    public void ComputeKG(string fwsbh, string cplb, string cpxh, int cpsl, ref DataTable dtkg, ref string msg)
    {
        int yuanz = 0;//使用的原装空鼓量
        int lanz = 0;//使用的蓝装空鼓量
        int lvz = 0;//使用的绿装空鼓量
        int hongz = 0;//使用的红装空鼓数量

        //获取该型号的空鼓各种类别的当前可用量       
        DataTable dtky = GetAllKGCanUseByWhere(fwsbh, "", cpxh, dtkg);
        int lvz_ky = 0;//绿装空鼓可用量
        int yuanz_ky = 0;//原装空鼓可用量
        int lanz_ky = 0;//蓝装空鼓可用量
        int hongz_ky = 0;//红装空鼓可用量
        for (int i = 0; i < dtky.Rows.Count; i++)
        {
            string kglx = dtky.Rows[i]["空鼓类型"].ToString();
            int SYZL = Convert.ToInt32(dtky.Rows[i]["剩余总量"].ToString());
            if (SYZL > 0)
            {
                switch (kglx)
                {
                    case "绿装":
                        lvz_ky = SYZL;
                        break;
                    case "蓝装":
                        lanz_ky = SYZL;
                        break;
                    case "原装":
                        yuanz_ky = SYZL;
                        break;
                    case "红装":
                        hongz_ky = SYZL;
                        break;
                    default:
                        break;
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
            else if (yuanz_ky >= cpsl - lanz_ky)
            {
                lanz = lanz_ky;
                yuanz = cpsl - lanz_ky;
            }
            else
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - lanz_ky - yuanz_ky).ToString() + "支\\n\\r";
                lanz = lanz_ky;
                yuanz = yuanz_ky;
            }
        }
        if (cplb == "绿装")//如果产品为绿装，则拆分顺序为：绿装空鼓、原装空鼓、蓝装空鼓
        {
            if (lvz_ky >= cpsl)//如果绿装空鼓够用，则直接扣减绿装空鼓。
            {
                lvz = cpsl;
            }
            else if (yuanz_ky >= cpsl - lvz_ky)//如果绿装空鼓不够用
            {
                //判断原装空鼓，如果够用则扣减绿装、原装两种空鼓

                lvz = lvz_ky;
                yuanz = cpsl - lvz_ky;
            }
            else if (lanz_ky >= cpsl - lvz_ky - yuanz_ky)//如果绿装+原装也不够用，则扣减绿装、原装、蓝装空鼓
            {
                lvz = lvz_ky;
                yuanz = yuanz_ky;
                lanz = cpsl - lvz_ky - yuanz_ky;
            }
            else
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - lvz_ky - yuanz_ky - lanz_ky).ToString() + "支\\n\\r";
                lvz = lvz_ky;
                yuanz = yuanz_ky;
                lanz = lanz_ky;
            }
        }
        if (cplb == "红装")//如果产品为红装，则只能使用红装空鼓
        {
            if (hongz_ky >= cpsl)
            {
                hongz = cpsl;
            }
            else
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - hongz_ky).ToString() + "支\\n\\r";
                hongz = hongz_ky;
            }
        }

        //DataSet dt_KG = DbHelperSQL.Query("select fwsbh as 客户编号,''as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,hz as 红装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量 from CWGL_KGSYMXJLB where 1!=1");
        if (yuanz + lanz + lvz + hongz > 0)
        {
            dtkg.Rows.Add(new object[] { fwsbh, "", cpxh, 0, yuanz, lanz, lvz, hongz, cplb, cpxh, cpsl });
        }
    }

   /// <summary>
   /// 特殊型号空鼓拆分
   /// </summary>
    /// <param name="fwsbh">服务商编号</param>
    /// <param name="cplb">产品类别</param>
    /// <param name="cpxh">产品型号</param>
    /// <param name="cpsl">产品数量</param>
    /// <param name="kgxh2">可扣减的另一种空鼓型号（388X、436X）</param>
    /// <param name="dtkg">用于存放空鼓拆分结果的数据表</param>
    /// <param name="msg">数量不足时返回的提醒字符串</param>
    public void ComputeTSKG(string fwsbh, string cplb, string cpxh, int cpsl, string kgxh2, ref  DataTable dtkg, ref string msg)
    {
        int yuanzA = 0;//使用的同型号原装空鼓量
        int lanzA = 0;//使用的同型号蓝装空鼓量
        int lvzA = 0;//使用的同型号绿装空鼓量
        int hongzA = 0;//使用的同型号的红装空鼓量；

        int yuanzX = 0;//使用的另一型号原装空鼓量
        int lanzX = 0;//使用的另一型号蓝装空鼓量
        int lvzX = 0;//使用的另一型号绿装空鼓量
        int hongzX = 0;//使用的另一型号红装空鼓量

        //获取同型号的空鼓各种类别的当前可用量    
        string kgxh = cpxh;
        //针对特殊的型号，进行特别的处理
        if (cpxh == "FM-CC388AH(医院专供)")
        {
            kgxh = "FM-CC388AH";
        }
        else if (cpxh == "FM-CB436AH(医院专供)")
        {
            kgxh = "FM-CB436AH";
        }

        DataTable dtA = GetAllKGCanUseByWhere(fwsbh, "", kgxh, dtkg);
        int lvz_kyA = 0;//绿装空鼓可用量
        int yuanz_kyA = 0;//原装空鼓可用量
        int lanz_kyA = 0;//蓝装空鼓可用量
        int hongz_kyA = 0;//红装空鼓可用量
        for (int i = 0; i < dtA.Rows.Count; i++)
        {
            string kglx = dtA.Rows[i]["空鼓类型"].ToString();
            int A_SYZL = Convert.ToInt32(dtA.Rows[i]["剩余总量"].ToString());
            if (A_SYZL > 0)
            {
                switch (kglx)
                {
                    case "绿装":
                        lvz_kyA = A_SYZL;
                        break;
                    case "蓝装":
                        lanz_kyA = A_SYZL;
                        break;
                    case "原装":
                        yuanz_kyA = A_SYZL;
                        break;
                    case "红装":
                        hongz_kyA = A_SYZL;
                        break;
                    default:
                        break;
                }
            }
        }

        int yuanz_kyX = 0;//X规格原装可用空鼓量
        int lanz_kyX = 0;//X规格蓝装可用空鼓量
        int lvz_kyX = 0;//X规格绿装可用空鼓量
        int hongz_kyX = 0;//X规格的红装可用空鼓量
        DataTable dtX = GetAllKGCanUseByWhere(fwsbh, "", kgxh2, dtkg);//获取X规格各类型的可用空鼓量
        for (int i = 0; i < dtX.Rows.Count; i++)
        {
            string kglx = dtX.Rows[i]["空鼓类型"].ToString();
            int X_SYZL = Convert.ToInt32(dtX.Rows[i]["剩余总量"].ToString());
            if (X_SYZL > 0)
            {
                switch (kglx)
                {
                    case "绿装":
                        lvz_kyX = X_SYZL;
                        break;
                    case "蓝装":
                        lanz_kyX = X_SYZL;
                        break;
                    case "原装":
                        yuanz_kyX = X_SYZL;
                        break;
                    case "红装":
                        hongz_kyX = X_SYZL;
                        break;
                    default:
                        break;
                }
            }
        }

        //根据蓝、绿装进行空鼓拆分
        if (cplb == "蓝装")//如果产品为蓝装，则拆分顺序为：同规格蓝装、原装空鼓，X规格蓝装、原装空鼓
        {
            if (lanz_kyA >= cpsl)
            {
                lanzA = cpsl;
            }
            else if (yuanz_kyA >= cpsl - lanz_kyA)
            {
                lanzA = lanz_kyA;
                yuanzA = cpsl - lanz_kyA;
            }
            else if (lanz_kyX >= cpsl - lanz_kyA - yuanz_kyA)
            {
                lanzA = lanz_kyA;
                yuanzA = yuanz_kyA;
                lanzX = cpsl - lanz_kyA - yuanz_kyA;
            }
            else if (yuanz_kyX >= cpsl - lanz_kyA - yuanz_kyA - lanz_kyX)
            {
                lanzA = lanz_kyA;
                yuanzA = yuanz_kyA;
                lanzX = lanz_kyX;
                yuanzX = cpsl = lanz_kyA - yuanz_kyA - lanz_kyX;
            }
            else
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - lanz_kyA - yuanz_kyA - lanz_kyX - yuanz_kyX).ToString() + "支\\n\\r";
                lanzA = lanz_kyA;
                yuanzA = yuanz_kyA;
                lanzX = lanz_kyX;
                yuanzX = yuanz_kyX;
            }
        }
        else if (cplb == "绿装")//如果产品为绿装，则拆分顺序为：绿装空鼓、原装空鼓、蓝装空鼓
        {
            if (lvz_kyA >= cpsl)
            {
                lvzA = cpsl;
            }
            else if (yuanz_kyA >= cpsl - lvz_kyA)
            {
                lvzA = lvz_kyA;
                yuanzA = cpsl - lvz_kyA;
            }
            else if (lanz_kyA >= cpsl - lvz_kyA - yuanz_kyA)
            {
                lvzA = lvz_kyA;
                yuanzA = yuanz_kyA;
                lanzA = cpsl - lvz_kyA - yuanz_kyA;
            }
            else if (lvz_kyX >= cpsl - lvz_kyA - yuanz_kyA - lanz_kyA)
            {
                lvzA = lvz_kyA;
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                lvzX = cpsl - lvz_kyA - yuanz_kyA - lanz_kyA;

            }
            else if (yuanz_kyX >= cpsl - lvz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX)
            {
                lvzA = lvz_kyA;
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                lvzX = lvz_kyX;
                yuanzX = cpsl - lvz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX;
            }
            else if (lanz_kyX >= cpsl - lvz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX - yuanz_kyX)
            {
                lvzA = lvz_kyA;
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                lvzX = lvz_kyX;
                yuanzX = yuanz_kyX;
                lanzX = cpsl - lvz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX - yuanz_kyX;
            }
            else
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - lvz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX - yuanz_kyX - lanz_kyX).ToString() + "支\\n\\r";
                lvzA = lvz_kyA;
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                lvzX = lvz_kyX;
                yuanzX = yuanz_kyX;
                lanzX = lanz_kyX;
            }
        }
        else if (cplb == "红装")
        {
            if (hongz_kyA >= cpsl)//如果同规格蓝装空鼓够用，则直接扣减蓝装
            {
                hongzA = cpsl;
            }
            else if (hongz_kyX >= cpsl - hongz_kyA)
            {
                hongzA = hongz_kyA;
                hongzX = cpsl - hongz_kyA;
            }
            else
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - hongz_kyA - hongz_kyX).ToString() + "支\\n\\r";
                hongzA = hongz_kyA;
                hongzX = hongz_kyX;
            }
        }

        //添加同型号拆分结果
        //DataSet dt_KG = DbHelperSQL.Query("select fwsbh as 客户编号,''as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,hz as 红装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量 from CWGL_KGSYMXJLB where 1!=1");

        if (yuanzA + lanzA + lvzA + hongzA > 0)
        {
            dtkg.Rows.Add(new object[] { fwsbh, "", kgxh, 0, yuanzA, lanzA, lvzA, hongzA, cplb, cpxh, cpsl });
        }

        //添加X型号的扣减结果
        if (yuanzX + lanzX + lvzX + hongzX > 0)
        {
            dtkg.Rows.Add(new object[] { fwsbh, "", kgxh2, 0, yuanzX, lanzX, lvzX, hongzX, cplb, cpxh, cpsl });
        }
    }
    #endregion

    #region 带授信空鼓的空鼓扣减拆分功能，空鼓不足返回提醒字符串
   /// <summary>
   /// 普通型号产品的空鼓扣减
   /// </summary>
   /// <param name="fwsbh">服务商编号</param>
   /// <param name="cplb">产品类别</param>
   /// <param name="cpxh">产品型号</param>
   /// <param name="cpsl">产品数量</param>
   /// <param name="dqshouxinsl">当前可用授信空鼓数量</param>
   /// <param name="dtkg">用来保存空鼓扣减结果的数据表</param>
   /// <param name="msg">空鼓不足时返回的提示信息</param>
    public void ComputeKG(string fwsbh, string cplb, string cpxh, int cpsl, int dqshouxinsl, ref DataTable dtkg, ref string msg)
    {
        int yuanz = 0;//使用的原装空鼓量
        int lanz = 0;//使用的蓝装空鼓量
        int lvz = 0;//使用的绿装空鼓量
        int hongz = 0;//使用的红装空鼓数量
        int shouxin = 0;//使用的授信空鼓数量


        int lvz_ky = 0;//绿装空鼓可用量
        int yuanz_ky = 0;//原装空鼓可用量
        int lanz_ky = 0;//蓝装空鼓可用量
        int hongz_ky = 0;//红装空鼓可用量
        int shouxin_ky = 0;//授信空鼓的可用量

        //获取该型号的空鼓各种类别的当前可用量       
        DataTable dtky = GetAllKGCanUseByWhere(fwsbh, "", cpxh, dtkg);
        for (int i = 0; i < dtky.Rows.Count; i++)
        {
            string kglx = dtky.Rows[i]["空鼓类型"].ToString();
            int SYZL = Convert.ToInt32(dtky.Rows[i]["剩余总量"].ToString());
            if (SYZL > 0)
            {
                switch (kglx)
                {
                    case "绿装":
                        lvz_ky = SYZL;
                        break;
                    case "蓝装":
                        lanz_ky = SYZL;
                        break;
                    case "原装":
                        yuanz_ky = SYZL;
                        break;
                    case "红装":
                        hongz_ky = SYZL;
                        break;
                    default:
                        break;

                }
            }
        }

        //获取授信空鼓当前实际可用数量
        shouxin_ky = dqshouxinsl - Convert.ToInt32(dtkg.Compute("sum(使用授信数量)", "true").ToString() == "" ? "0" : dtkg.Compute("sum(使用授信数量)", "true").ToString());

        //根据蓝、绿装、红装进行空鼓拆分
        if (cplb == "蓝装")//如果产品为蓝装，则拆分顺序为：蓝装空鼓、原装空鼓
        {
            if (lanz_ky >= cpsl)//如果蓝装空鼓够用，则直接扣减蓝装
            {
                lanz = cpsl;
            }
            else if (yuanz_ky >= cpsl - lanz_ky)//如果蓝装空鼓不够用，加上原装够用，则同时扣减蓝装、原装空鼓
            {
                lanz = lanz_ky;
                yuanz = cpsl - lanz_ky;
            }
            else if (shouxin_ky >= cpsl - lanz_ky - yuanz_ky)//如果蓝装、原装不够用，加上授信够用，则扣减蓝装、原装、授信
            {
                lanz = lanz_ky;
                yuanz = yuanz_ky;
                shouxin = cpsl - lanz_ky - yuanz_ky;
            }
            else//如果都不够了，给出提示。
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - lanz_ky - yuanz_ky - shouxin_ky).ToString() + "支\\n\\r";
                lanz = lanz_ky;
                yuanz = yuanz_ky;
                shouxin = shouxin_ky;
            }
        }
        else if (cplb == "绿装")//如果产品为绿装，则拆分顺序为：绿装空鼓、原装空鼓、蓝装空鼓
        {
            if (lvz_ky >= cpsl)//如果绿装空鼓够用，则直接扣减绿装空鼓。
            {
                lvz = cpsl;
            }
            else if (yuanz_ky >= cpsl - lvz_ky)//如果绿装空鼓不够用，加上原装够用，则扣减绿装、原装
            {
                lvz = lvz_ky;
                yuanz = cpsl - lvz_ky;
            }
            else if (lanz_ky >= cpsl - lvz_ky - yuanz_ky)//如果绿装、原装不沟通，加上蓝装沟通，则扣减绿装、原装、蓝装
            {
                lvz = lvz_ky;
                yuanz = yuanz_ky;
                lanz = cpsl - lvz_ky - yuanz_ky;
            }
            else if (shouxin_ky >= cpsl - lvz_ky - yuanz_ky - lanz_ky)//如果都不够用，加上授信够用，则扣减绿装、原装、蓝装、授信
            {
                lvz = lvz_ky;
                yuanz = yuanz_ky;
                lanz = lanz_ky;
                shouxin = cpsl - lvz_ky - yuanz_ky - lanz_ky;
            }
            else//如果加上授信也不够用，则给出提示
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - lvz_ky - yuanz_ky - lanz_ky - shouxin_ky).ToString() + "支\\n\\r";
                lvz = lvz_ky;
                yuanz = yuanz_ky;
                lanz = lanz_ky;
            }
        }
        else if (cplb == "红装")//如果产品为红装，则只能使用红装空鼓
        {
            if (hongz_ky >= cpsl)
            {
                hongz = cpsl;
            }
            else if (shouxin_ky >= cpsl - hongz_ky)
            {
                hongz = hongz_ky;
                shouxin = cpsl - hongz_ky;
            }
            else
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - hongz_ky - shouxin_ky).ToString() + "支\\n\\r";
                hongz = hongz_ky;
            }
        }

        //DataSet dt_KG = DbHelperSQL.Query("select fwsbh as 客户编号,''as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,hz as 红装旧鼓量,0 as 使用授信数量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量 from CWGL_KGSYMXJLB where 1!=1");
        if (yuanz + lanz + lvz + hongz + shouxin > 0)
        {
            dtkg.Rows.Add(new object[] { fwsbh, "", cpxh, 0, yuanz, lanz, lvz, hongz, shouxin, cplb, cpxh, cpsl });
        }
    }

    /// <summary>
    /// 特殊型号(388A、436A)的空鼓扣减
    /// </summary>
    /// <param name="fwsbh">服务商编号</param>
    /// <param name="cplb">产品类别</param>
    /// <param name="cpxh">产品型号</param>
    /// <param name="cpsl">产品数量</param>
    /// <param name="kgxh2">可扣减的另一种空鼓型号（388X、436X）</param>
    /// <param name="dqshouxinsl">当前可用授信空鼓数量</param>
    /// <param name="dtkg">用来保存空鼓扣减结果的数据表</param>
    /// <param name="msg">空鼓不足时返回的提示信息</param>
    public void ComputeTSKG(string fwsbh, string cplb, string cpxh, int cpsl, string kgxh2, int dqshouxinsl, ref  DataTable dtkg, ref string msg)
    {
        int yuanzA = 0;//使用的同型号原装空鼓量
        int lanzA = 0;//使用的同型号蓝装空鼓量
        int lvzA = 0;//使用的同型号绿装空鼓量
        int hongzA = 0;//使用的同型号的红装空鼓量；       

        int yuanzX = 0;//使用的另一型号原装空鼓量
        int lanzX = 0;//使用的另一型号蓝装空鼓量
        int lvzX = 0;//使用的另一型号绿装空鼓量
        int hongzX = 0;//使用的另一型号红装空鼓量

        int shouxin = 0;//使用的授信空鼓数量

        string kgxh = cpxh;
        //针对特殊的型号，进行特别的处理
        switch (cpxh)
        {
            case "FM-CC388AH(医院专供)":
                kgxh = "FM-CC388AH";
                break;
            case "FM-CB436AH(医院专供)":
                kgxh = "FM-CB436AH";
                break;
            default:
                break;
        }

        //获取同型号的空鼓各种类别的当前可用量
        DataTable dtA = GetAllKGCanUseByWhere(fwsbh, "", kgxh, dtkg);
        int lvz_kyA = 0;//绿装空鼓可用量
        int yuanz_kyA = 0;//原装空鼓可用量
        int lanz_kyA = 0;//蓝装空鼓可用量
        int hongz_kyA = 0;//红装空鼓可用量
        if (dtA.Rows.Count == 0)
        {
            dtA.Rows.Add(fwsbh, cplb, kgxh, "0");
        }
        for (int i = 0; i < dtA.Rows.Count; i++)
        {
            string kglx = dtA.Rows[i]["空鼓类型"].ToString();
            int A_SYZL = Convert.ToInt32(dtA.Rows[i]["剩余总量"].ToString());
            if (A_SYZL > 0)
            {
                switch (kglx)
                {
                    case "绿装":
                        lvz_kyA = A_SYZL;
                        break;
                    case "蓝装":
                        lanz_kyA = A_SYZL;
                        break;
                    case "原装":
                        yuanz_kyA = A_SYZL;
                        break;
                    case "红装":
                        hongz_kyA = A_SYZL;
                        break;
                    default:
                        break;
                }
            }
        }

        int yuanz_kyX = 0;//X规格原装可用空鼓量
        int lanz_kyX = 0;//X规格蓝装可用空鼓量
        int lvz_kyX = 0;//X规格绿装可用空鼓量
        int hongz_kyX = 0;//X规格的红装可用空鼓量
        DataTable dtX = GetAllKGCanUseByWhere(fwsbh, "", kgxh2, dtkg);//获取X规格各类型的可用空鼓量
        for (int i = 0; i < dtX.Rows.Count; i++)
        {
            string kglx = dtX.Rows[i]["空鼓类型"].ToString();
            int X_SYZL = Convert.ToInt32(dtX.Rows[i]["剩余总量"].ToString());
            if (X_SYZL > 0)
            {
                switch (kglx)
                {
                    case "绿装":
                        lvz_kyX = X_SYZL;
                        break;
                    case "蓝装":
                        lanz_kyX = X_SYZL;
                        break;
                    case "原装":
                        yuanz_kyX = X_SYZL;
                        break;
                    case "红装":
                        hongz_kyX = X_SYZL;
                        break;
                    default:

                        break;
                }
            }
        }

        //获取授信空鼓当前实际可用量  
        int shouxin_ky = 0;//授信空鼓可用量
        shouxin_ky = dqshouxinsl - Convert.ToInt32(dtkg.Compute("sum(使用授信数量)", "true").ToString() == "" ? "0" : dtkg.Compute("sum(使用授信数量)", "true").ToString());


        //根据蓝、绿装进行空鼓拆分
        if (cplb == "蓝装")//蓝装产品，拆分顺序：同规格蓝装、原装；X规格蓝装、原装
        {
            if (lanz_kyA >= cpsl)//如果蓝装A型号够用，直接扣减
            {
                lanzA = cpsl;
            }
            else if (yuanz_kyA >= cpsl - lanz_kyA)//如果不够，加上原装
            {
                lanzA = lanz_kyA;
                yuanzA = cpsl - lanz_kyA;
            }
            else if (lanz_kyX >= cpsl - lanz_kyA - yuanz_kyA)//如果不够，加上蓝装X
            {
                lanzA = lanz_kyA;
                yuanzA = yuanz_kyA;
                lanzX = cpsl - lanz_kyA - yuanz_kyA;
            }
            else if (yuanz_kyX >= cpsl - lanz_kyA - yuanz_kyA - lanz_kyX)//如果不够，加上原装X
            {
                lanzA = lanz_kyA;
                yuanzA = yuanz_kyA;
                lanzX = lanz_kyX;
                yuanzX = cpsl - lanz_kyA - yuanz_kyA - lanz_kyX;
            }
            else if (shouxin_ky >= cpsl - lanz_kyA - yuanz_kyA - lanz_kyX - yuanz_kyX)//如果不够，继续加授信空鼓
            {
                lanzA = lanz_kyA;
                yuanzA = yuanz_kyA;
                lanzX = lanz_kyX;
                yuanzX = yuanz_kyX;
                shouxin = cpsl - lanz_kyA - yuanz_kyA - lanz_kyX - yuanz_kyX;
            }
            else
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - lanz_kyA - yuanz_kyA - lanz_kyX - yuanz_kyX - shouxin_ky).ToString() + "支\\n\\r";
                lanzA = lanz_kyA;
                yuanzA = yuanz_kyA;
                lanzX = lanz_kyX;
                yuanzX = yuanz_kyX;
                shouxin = shouxin_ky;
            }
        }
        else if (cplb == "绿装")//如果产品为绿装，则拆分顺序为：绿装空鼓、原装空鼓、蓝装空鼓
        {
            if (lvz_kyA >= cpsl)//如果同型号的空鼓数量足够
            {
                lvzA = cpsl;
            }
            else if (yuanz_kyA >= cpsl - lvz_kyA)//判断原装空鼓，如果够用则扣减绿装、原装两种空鼓
            {
                lvzA = lvz_kyA;
                yuanzA = cpsl - lvzA;
            }
            else if (lanz_kyA > cpsl - lvz_kyA - yuanz_kyA)//如果绿装+原装也不够用，则扣减绿装、原装、蓝装空鼓
            {
                lvzA = lvz_kyA;
                yuanzA = yuanz_kyA;
                lanzA = cpsl - lvzA - yuanzA;
            }
            else if (lvz_kyX >= cpsl - lanz_kyA - yuanz_kyA - lanz_kyA)
            {
                lvzA = lvz_kyA;//先将同规格的空鼓扣减掉
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                lvzX = cpsl - lanz_kyA - yuanz_kyA - lanz_kyA;
            }
            else if (yuanz_kyX >= cpsl - lanz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX)
            {
                lvzA = lvz_kyA;//先将同规格的空鼓扣减掉
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                lvzX = lvz_kyX;
                yuanzX = cpsl - lanz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX;
            }
            else if (lanz_kyX >= cpsl - lanz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX - yuanz_kyX)
            {
                lvzA = lvz_kyA;//先将同规格的空鼓扣减掉
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                lvzX = lvz_kyX;
                yuanzX = yuanz_kyX;
                lanzX = cpsl - lanz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX - yuanz_kyX;
            }
            else if (shouxin_ky >= cpsl - lvz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX - yuanz_kyX - lanz_kyX)
            {
                lvzA = lvz_kyA;//先将同规格的空鼓扣减掉
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                lvzX = lvz_kyX;
                yuanzX = yuanz_kyX;
                lanzX = lanz_kyX;
                shouxin = cpsl - lvz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX - yuanz_kyX - lanz_kyX;
            }
            else
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - lvz_kyA - yuanz_kyA - lanz_kyA - lvz_kyX - yuanz_kyX - lanz_kyX - shouxin_ky).ToString() + "支\\n\\r";
                lvzA = lvz_kyA;
                yuanzA = yuanz_kyA;
                lanzA = lanz_kyA;
                lvzX = lvz_kyX;
                yuanzX = yuanz_kyX;
                lanzX = lanz_kyX;
                shouxin = shouxin_ky;
            }
        }
        else if (cplb == "红装")//红装只能扣减红装空鼓,目前红装没有388X的型号
        {
            if (hongz_kyA >= cpsl)//如果同规格红装空鼓够用
            {
                hongzA = cpsl;
            }
            else if (hongz_kyX > cpsl - hongz_kyA)
            {
                hongzA = hongz_kyA;
                hongzX = cpsl - hongz_kyA;
            }
            else if (shouxin_ky >= cpsl - hongz_kyA - hongz_kyX) //如果同规格蓝装空鼓不够用，判断同规格的原装是否够用，如果不够，则进一步扣减另一规格的空鼓
            {
                hongzA = hongz_kyA;
                hongzX = hongz_kyX;
                shouxin = cpsl - hongz_kyA - hongz_kyX;
            }
            else
            {
                msg = msg + cplb + cpxh + "，差" + (cpsl - hongz_kyA - hongz_kyX - shouxin_ky).ToString() + "支\\n\\r";
                hongzA = hongz_kyA;
                hongzX = hongz_kyX;
                shouxin = shouxin_ky;
            }
        }


        //添加同型号拆分结果，授信的扣减添加到同型号的空鼓扣减结果中
        //DataSet dt_KG = DbHelperSQL.Query("select fwsbh as 客户编号,''as 空鼓类型,kgxh as 空鼓规格,0 as 剩余总量,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,hz as 红装旧鼓量,0 as 使用授信数量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量 from CWGL_KGSYMXJLB where 1!=1");

        if (yuanzA + lanzA + lvzA + hongzA + shouxin > 0)
        {
            dtkg.Rows.Add(new object[] { fwsbh, "", kgxh, 0, yuanzA, lanzA, lvzA, hongzA, shouxin, cplb, cpxh, cpsl });
        }

        //添加X型号的扣减结果，授信记为0
        if (yuanzX + lanzX + lvzX + hongzX > 0)
        {
            dtkg.Rows.Add(new object[] { fwsbh, "", kgxh2, 0, yuanzX, lanzX, lvzX, hongzX, 0, cplb, cpxh, cpsl });
        }
    }
   
#endregion

    #region 返还授信空鼓
    /// <summary>
    /// 交易成功或调整空鼓时返还授信旧硒鼓
    /// </summary>
    /// <param name="fwsbh">服务商编号</param>
    /// <param name="kglb">空鼓类别</param>
    /// <param name="kgxh">空鼓型号</param>
    /// <param name="kgsl">空鼓数量</param>
    /// <param name="ywlx">业务类型：交易买入、空鼓调整</param>
    /// <param name="ywdh">业务单号</param>
    /// <returns>需要执行的sql语句数组</returns>
    public ArrayList GetFHSXsql(string fwsbh, string kglb, string kgxh, int kgsl,string ywlx,string ywdh)
    {
        ArrayList al = new ArrayList();//用于存放需要返回的sql语句
        DataTable dtKG = DbHelperSQL.Query("select kgxh as 空鼓型号,'' as 空鼓类别,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,hz as 红装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量,gldh as 关联单号,sysj as 使用时间,ssbsc as 所属办事处,fwsmc as 服务商名称 from CWGL_KGSYMXJLB where 1!=1").Tables[0];
        

        //根据传入的空鼓规格，查找是否存在对应规格的待返还授信信息
        string cpxh2 = "";
        string cpxh3 = "";
        switch (kgxh)
        {
            case "FM-CC388XH":
                cpxh2 = "FM-CC388AH";
                cpxh3 = "FM-CC388AH(医院专供)";
                break;
            case "FM-CB436XH":
                cpxh2 = "FM-CB436AH";
                cpxh3 = "FM-CB436AH(医院专供)";
                break;
            case "FM-CC388AH":
                cpxh2 = "FM-CC388AH(医院专供)";
                break;
            case "FM-CB436AH":
                cpxh2 = "FM-CB436AH(医院专供)";
                break;
            default:
                break;
        }
      
        //获取该型号空鼓可以返还的授信空鼓使用信息
        string sql = "select  Number,SSBSC as 所属办事处,FWSBH as 服务商编号,FWSMC as 服务商名称,SYSJ as 使用时间,CPLB as 产品类别,CPXH as 产品型号,CPSL as 产品数量,SYSXSL as 使用授信数量,YFHSL as 已返还数量,SYSXSL-YFHSL as 未返还数量,createtime from FWPT_FWSSXKGSYMXB where fwsbh='" + fwsbh + "' and (cpxh='" + kgxh + "' or cpxh='" + cpxh2 + "' or cpxh='" + cpxh3 + "' )  and YFHSL<SYSXSL  and YXZT='有效' ";
        switch (kglb)
        {
            case "原装":
                sql = sql + " and (cplb='绿装' or cplb='蓝装')";             
                break;
            case "蓝装":
                sql = sql + " and (cplb='绿装' or cplb='蓝装')";               
                break;
            case "绿装":
                sql = sql + " and cplb='绿装' ";               
                break;
            case "红装":
                sql = sql + " and cplb='红装'";                
                break;          
            default:
                sql = sql + " 1!=1";              
                break;
        }
        sql = sql + " order by SYSJ";     
       
        //根据服务商编号获取服务商当前需要返还的授信空鼓信息    
        DataSet dsSX = DbHelperSQL.Query(sql);      

        //获取本次的返还明细信息
        int rest= kgsl;//剩余可用空鼓数量    
        for (int i = 0; i < dsSX.Tables[0].Rows.Count; i++)
        {
            string cplb = dsSX.Tables[0].Rows[i]["产品类别"].ToString();
            string cpxh = dsSX.Tables[0].Rows[i]["产品型号"].ToString();
            int wfhsl = Convert.ToInt32(dsSX.Tables[0].Rows[i]["未返还数量"].ToString());

            int kjsl = 0;//记录本次可返还的数量
            if (rest > 0)
            {
                if (rest >= wfhsl)
                {
                    kjsl = wfhsl;
                }
                else
                {
                    kjsl = rest;
                }              
                rest = rest - kjsl;

               // DataTable dtKG = DbHelperSQL.Query("select kgxh as 空鼓规格,'' as 空鼓类别,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,hz as 红装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量,gldh as 关联单号,sysj as 使用时间,ssbsc as 所属办事处,fwsmc as 服务商名称 from CWGL_KGSYMXJLB where 1!=1").Tables[0];
                switch (kglb)
                {
                    case "原装":
                        dtKG.Rows.Add(new object[] {  kgxh, kglb, kjsl, 0, 0, 0, cplb, cpxh, dsSX.Tables[0].Rows[i]["使用授信数量"].ToString(), dsSX.Tables[0].Rows[i]["number"].ToString(), dsSX.Tables[0].Rows[i]["createtime"].ToString(), dsSX.Tables[0].Rows[i]["所属办事处"].ToString(), dsSX.Tables[0].Rows[i]["服务商名称"].ToString() });
                        break;
                    case "蓝装":
                        dtKG.Rows.Add(new object[] {  kgxh, kglb, 0, kjsl, 0, 0, cplb, cpxh, dsSX.Tables[0].Rows[i]["使用授信数量"].ToString(), dsSX.Tables[0].Rows[i]["number"].ToString(), dsSX.Tables[0].Rows[i]["createtime"].ToString(), dsSX.Tables[0].Rows[0]["所属办事处"].ToString(), dsSX.Tables[0].Rows[i]["服务商名称"].ToString() });
                        break;
                    case "绿装":
                        dtKG.Rows.Add(new object[] {  kgxh, kglb, 0, 0, kjsl, 0, cplb, cpxh, dsSX.Tables[0].Rows[i]["使用授信数量"].ToString(), dsSX.Tables[0].Rows[i]["number"].ToString(), dsSX.Tables[0].Rows[i]["createtime"].ToString(), dsSX.Tables[0].Rows[i]["所属办事处"].ToString(), dsSX.Tables[0].Rows[i]["服务商名称"].ToString() });
                        break;
                    case "红装":
                        dtKG.Rows.Add(new object[] {  kgxh, kglb, 0, 0, 0, kjsl, cplb, cpxh, dsSX.Tables[0].Rows[i]["使用授信数量"].ToString(), dsSX.Tables[0].Rows[i]["number"].ToString(), dsSX.Tables[0].Rows[i]["createtime"].ToString(), dsSX.Tables[0].Rows[i]["所属办事处"].ToString(), dsSX.Tables[0].Rows[i]["服务商名称"].ToString() });
                        break;
                    default:
                        break;
                }
            }           
        }
       
        string bz = ywlx + "，单号：" + ywdh;
        //生成需要执行的sql语句
        al = CreateSql(dtKG, fwsbh, bz);
        return al;
    }

    /// <summary>
    /// 交付旧硒鼓审核通过后返还授信
    /// </summary>
    /// <param name="fwsbh">服务商编号</param>
    /// <param name="dtRu">本次交付的旧硒鼓数据表，必须包含字段：空鼓类别、空鼓型号、空鼓数量（即合格数量）</param>
    /// <param name="ywlx">业务类型，固定为“交付旧硒鼓”</param>
    /// <param name="ywdh">业务单号</param>
    /// <returns></returns>
    public ArrayList GetFHSXsql(string fwsbh, DataTable dtRu, string ywlx, string ywdh)
    {
        ArrayList al = new ArrayList();//返还的sql语句数组
        DataTable  dtKG = DbHelperSQL.Query("select kgxh as 空鼓型号,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,hz as 红装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量,gldh as 关联单号,sysj as 使用时间,ssbsc as 所属办事处,fwsmc as 服务商名称 from CWGL_KGSYMXJLB where 1!=1").Tables[0];

        //获取待返还的授信空鼓数据
        string sql = "select  Number,SSBSC as 所属办事处,FWSBH as 服务商编号,FWSMC as 服务商名称,SYSJ as 使用时间,CPLB as 产品类别,CPXH as 产品型号,CPSL as 产品数量,SYSXSL as 使用授信数量,YFHSL as 已返还数量,SYSXSL-YFHSL as 未返还数量,createtime from FWPT_FWSSXKGSYMXB where fwsbh='" + fwsbh + "'and YFHSL<SYSXSL  and YXZT='有效' order by SYSJ";
        DataSet dtcp = DbHelperSQL.Query(sql);      
        
        if (dtcp != null && dtcp.Tables[0].Rows.Count > 0 && dtRu != null && dtRu.Rows.Count > 0)
        {
            for (int i = 0; i < dtcp.Tables[0].Rows.Count; i++)
            {
                string cplb = dtcp.Tables[0].Rows[i]["产品类别"].ToString();
                string cpxh = dtcp.Tables[0].Rows[i]["产品型号"].ToString();
                int wfhsl = Convert.ToInt32(dtcp.Tables[0].Rows[i]["未返还数量"].ToString());

                string kgxh = cpxh;
                string kgxh2 = "";
                if (cpxh.IndexOf("FM-CB436AH") >= 0)
                {
                    kgxh = "FM-CB436AH";
                    kgxh2 = "FM-CB436XH";
                }
                else if (cpxh.IndexOf("FM-CC388AH") >= 0)
                {
                    kgxh = "FM-CC388AH";
                    kgxh2 = "FM-CC388XH";
                }

                Hashtable ht = GetDQKY(cplb, kgxh,kgxh2, dtRu, dtKG);
                int lvz_ky = Convert.ToInt32(ht["lvz_ky"]);//当前绿装可用数量
                int yuanz_ky = Convert.ToInt32(ht["yuanz_ky"]);//当前原装可用数量
                int lanz_ky = Convert.ToInt32(ht["lanz_ky"]);//当前可用蓝装数量         
                int hongz_ky = Convert.ToInt32(ht["hongz_ky"]);//当前红装可用数量       

                int lvz_kyX = Convert.ToInt32(ht["lvzX_ky"]);//当前可用绿装X型号数量
                int yuanz_kyX = Convert.ToInt32(ht["yuanzX_ky"]);//当前可用原装X型号数量
                int lanz_kyX = Convert.ToInt32(ht["lanzX_ky"]);//当前可用蓝装X型号数量

                if (lvz_ky + lanz_ky + yuanz_ky + lvz_kyX + lanz_kyX + yuanz_kyX > 0)
                {
                    int yuanz = 0; int lanz = 0; int lvz = 0; int hongz = 0;//使用的原装、蓝装、绿装、红装同型号的空鼓
                    int yuanzX = 0; int lanzX = 0; int lvzX = 0;//388A、436A使用的X型号的原装、蓝装、绿装空鼓

                    //针对产品类别不同，分别进行不同的处理
                    if (cplb == "红装")
                    {
                        //红装产品只能用红装硒鼓，且不存在特殊型号 
                        if (hongz_ky >= wfhsl)
                        {
                            hongz = wfhsl;
                        }
                        else
                        {
                            hongz = hongz_ky;
                        }
                    }
                    else if (cplb == "绿装")
                    {
                        if (lvz_ky >= wfhsl)
                        {
                            lvz = wfhsl;
                        }
                        else if (yuanz_ky > wfhsl - lvz_ky)
                        {
                            lvz = lvz_ky;
                            yuanz = wfhsl - lvz_ky;
                        }
                        else if (lanz_ky > wfhsl - lvz_ky - yuanz_ky)
                        {
                            lvz = lvz_ky;
                            yuanz = yuanz_ky;
                            lanz = wfhsl - lvz_ky - yuanz_ky;
                        }
                        else
                        {
                            lvz = lvz_ky;
                            yuanz = yuanz_ky;
                            lanz = lanz_ky;
                            if (lvz_kyX + lanz_kyX + yuanz_kyX > 0)//388A、436A的产品需要检查是否有X型号的空鼓
                            {
                                if (lvz_kyX >= wfhsl - lvz_ky - yuanz_ky - lanz_ky)
                                {
                                    lvzX = wfhsl - lvz_ky - yuanz_ky - lanz_ky;
                                }
                                else if (yuanz_kyX >= wfhsl - lvz_ky - yuanz_ky - lanz_ky - lvz_kyX)
                                {
                                    lvzX = lvz_kyX;
                                    yuanzX = wfhsl - lvz_ky - yuanz_ky - lanz_ky - lvz_kyX;
                                }
                                else if (lanz_kyX >= wfhsl - lvz_ky - yuanz_ky - lanz_ky - lvz_kyX - yuanz_kyX)
                                {
                                    lvzX = lvz_kyX;
                                    yuanzX = yuanz_kyX;
                                    lanzX = wfhsl - lvz_ky - yuanz_ky - lanz_ky - lvz_kyX - yuanz_kyX;
                                }
                                else
                                {
                                    lvzX = lvz_kyX;
                                    yuanzX = yuanz_kyX;
                                    lanzX = lanz_kyX;
                                }
                            }
                        }
                    }
                    else if (cplb == "蓝装")
                    {
                        if (lanz_ky > wfhsl)
                        {
                            lanz = wfhsl;
                        }
                        else if (yuanz_ky > wfhsl - lanz_ky)
                        {
                            lanz = lanz_ky;
                            yuanz = wfhsl - lanz_ky;
                        }
                        else
                        {
                            lanz = lanz_ky;
                            yuanz = yuanz_ky;
                            if (lvz_kyX + lanz_kyX + yuanz_kyX > 0)//388A、436A的产品需要检查是否有X型号的空鼓
                            {

                                if (lanz_kyX >= wfhsl - lanz_ky - yuanz_ky)
                                {
                                    lanzX = wfhsl - lanz_ky - yuanz_ky;
                                }
                                else if (yuanz_kyX >= wfhsl - lanz_ky - yuanz_ky - lanz_kyX)
                                {
                                    lanzX = lanz_kyX;
                                    yuanzX = wfhsl - lanz_ky - yuanz_ky - lanz_kyX;
                                }
                                else
                                {
                                    lanzX = lanz_kyX;
                                    yuanzX = yuanz_kyX;
                                }
                            }
                        }
                    }

                    // DataTable  dtKG = DbHelperSQL.Query("select kgxh as 空鼓规格,yz as 原装旧鼓量,lanz as 蓝装旧鼓量,lvz as 绿装旧鼓量,hz as 红装旧鼓量,cplb as 产品类别,cpxh as 产品型号,cpsl as 产品数量,gldh as 关联单号,sysj as 使用时间,ssbsc as 所属办事处,fwsmc as 服务商名称 from CWGL_KGSYMXJLB where 1!=1").Tables[0];
                    if (lvz + lanz + yuanz + hongz > 0)
                    {
                        dtKG.Rows.Add(new object[] { kgxh, yuanz, lanz, lvz, hongz, cplb, cpxh, dtcp.Tables[0].Rows[i]["使用授信数量"].ToString(), dtcp.Tables[0].Rows[i]["number"].ToString(), dtcp.Tables[0].Rows[i]["createtime"].ToString(), dtcp.Tables[0].Rows[i]["所属办事处"].ToString(), dtcp.Tables[0].Rows[i]["服务商名称"].ToString() });
                    }
                    if (lvzX + lanzX + yuanzX > 0)
                    {
                        dtKG.Rows.Add(new object[] { kgxh2, yuanzX, lanzX, lvzX, 0, cplb, cpxh, dtcp.Tables[0].Rows[i]["使用授信数量"].ToString(), dtcp.Tables[0].Rows[i]["number"].ToString(), dtcp.Tables[0].Rows[i]["createtime"].ToString(), dtcp.Tables[0].Rows[i]["所属办事处"].ToString(), dtcp.Tables[0].Rows[i]["服务商名称"].ToString() });
                    }

                } 
            }
        }

        string bz = ywlx + "，单号：" + ywdh;
      
        //生成需要执行的sql语句
        al = CreateSql(dtKG,fwsbh, bz);       

        return al;
    }

    //获取当前可用的空鼓数量
    private Hashtable GetDQKY(string cplb, string kgxh,string kgxh2, DataTable dtRu, DataTable dtKG)
    {
        Hashtable ht = new Hashtable();    

        //获取该型号总共可用的空鼓数据
        int yuanz_zl = 0; int lanz_zl = 0; int lvz_zl = 0; int hongz_zl = 0;//同型号可使用的空鼓
        if (dtRu != null && dtRu.Select("空鼓型号='" + kgxh + "'").Length > 0)
        {
            lvz_zl = Convert.ToInt32(dtRu.Compute("sum(空鼓数量)", "空鼓类别='绿装' and 空鼓型号='" + kgxh + "'").ToString() == "" ? "0" : dtRu.Compute("sum(空鼓数量)", "空鼓类别='绿装' and 空鼓型号='" + kgxh + "'").ToString());
            lanz_zl = Convert.ToInt32(dtRu.Compute("sum(空鼓数量)", "空鼓类别='蓝装' and 空鼓型号='" + kgxh + "'").ToString() == "" ? "0" : dtRu.Compute("sum(空鼓数量)", "空鼓类别='蓝装' and 空鼓型号='" + kgxh + "'").ToString());
            yuanz_zl = Convert.ToInt32(dtRu.Compute("sum(空鼓数量)", "空鼓类别='原装' and 空鼓型号='" + kgxh + "'").ToString() == "" ? "0" : dtRu.Compute("sum(空鼓数量)", "空鼓类别='原装' and 空鼓型号='" + kgxh + "'").ToString());
            hongz_zl = Convert.ToInt32(dtRu.Compute("sum(空鼓数量)", "空鼓类别='红装' and 空鼓型号='" + kgxh + "'").ToString() == "" ? "0" : dtRu.Compute("sum(空鼓数量)", "空鼓类别='红装' and 空鼓型号='" + kgxh + "'").ToString());
        }

        int yuanzX_zl = 0; int lanzX_zl = 0; int lvzX_zl = 0;//388A、436A使用的X型号的空鼓总共可用
        if (kgxh2 != "" && dtRu != null && dtRu.Select("空鼓型号='" + kgxh2 + "'").Length > 0)
        {
            lvzX_zl = Convert.ToInt32(dtRu.Compute("sum(空鼓数量)", "空鼓类别='绿装' and 空鼓型号='" + kgxh2 + "'").ToString() == "" ? "0" : dtRu.Compute("sum(空鼓数量)", "空鼓类别='绿装' and 空鼓型号='" + kgxh2 + "'").ToString());
            lanzX_zl = Convert.ToInt32(dtRu.Compute("sum(空鼓数量)", "空鼓类别='蓝装' and 空鼓型号='" + kgxh2 + "'").ToString() == "" ? "0" : dtRu.Compute("sum(空鼓数量)", "空鼓类别='蓝装' and 空鼓型号='" + kgxh2 + "'").ToString());
            yuanzX_zl = Convert.ToInt32(dtRu.Compute("sum(空鼓数量)", "空鼓类别='原装' and 空鼓型号='" + kgxh2 + "'").ToString() == "" ? "0" : dtRu.Compute("sum(空鼓数量)", "空鼓类别='原装' and 空鼓型号='" + kgxh2 + "'").ToString());
        }

        //获取该型号本次已经使用过的空鼓数量
        int yuanz_yy = 0; int lanz_yy = 0; int lvz_yy = 0; int hongz_yy = 0;//本次已经使用过的同型号空鼓       
        if (dtKG != null && dtKG.Select("空鼓型号='" + kgxh + "'").Length > 0)
        {
            yuanz_yy = Convert.ToInt32(dtKG.Compute("sum(原装旧鼓量)", "空鼓型号='" + kgxh + "'").ToString() == "" ? "0" : dtKG.Compute("sum(原装旧鼓量)", "空鼓型号='" + kgxh + "'").ToString());
            lanz_yy = Convert.ToInt32(dtKG.Compute("sum(蓝装旧鼓量)", "空鼓型号='" + kgxh + "'").ToString() == "" ? "0" : dtKG.Compute("sum(蓝装旧鼓量)", "空鼓型号='" + kgxh + "'").ToString());
            lvz_yy = Convert.ToInt32(dtKG.Compute("sum(绿装旧鼓量)", "空鼓型号='" + kgxh + "'").ToString() == "" ? "0" : dtKG.Compute("sum(绿装旧鼓量)", "空鼓型号='" + kgxh + "'").ToString());
            hongz_yy = Convert.ToInt32(dtKG.Compute("sum(红装旧鼓量)", "空鼓型号='" + kgxh + "'").ToString() == "" ? "0" : dtKG.Compute("sum(红装旧鼓量)", "空鼓型号='" + kgxh + "'").ToString());
        }

        int yuanzX_yy = 0; int lanzX_yy = 0; int lvzX_yy = 0;//本次已经使用过的X型号数量
        if (kgxh2 != "" && dtKG != null && dtKG.Select("空鼓型号='" + kgxh2 + "'").Length > 0)
        {
            lvzX_yy = Convert.ToInt32(dtKG.Compute("sum(绿装旧鼓量)", "空鼓型号='" + kgxh2 + "'").ToString() == "" ? "0" : dtKG.Compute("sum(绿装旧鼓量)", "空鼓型号='" + kgxh2 + "'").ToString());
            lanzX_yy = Convert.ToInt32(dtKG.Compute("sum(蓝装旧鼓量)", "空鼓型号='" + kgxh2 + "'").ToString() == "" ? "0" : dtKG.Compute("sum(蓝装旧鼓量)", "空鼓型号='" + kgxh2 + "'").ToString());
            yuanzX_yy = Convert.ToInt32(dtKG.Compute("sum(原装旧鼓量)", "空鼓型号='" + kgxh2 + "'").ToString() == "" ? "0" : dtKG.Compute("sum(原装旧鼓量)", "空鼓型号='" + kgxh2 + "'").ToString());
        }

        ht["lvz_ky"] = lvz_zl - lvz_yy;
        ht["lanz_ky"] = lanz_zl - lanz_yy;
        ht["yuanz_ky"] = yuanz_zl - yuanz_yy;
        ht["hongz_ky"] = hongz_zl - hongz_yy;
        ht["lvzX_ky"] = lvzX_zl - lvzX_yy;
        ht["lanzX_ky"] = lanzX_zl - lanzX_yy;
        ht["hongzX_ky"] = hongz_zl - hongz_yy;

        return ht;
    }    

    private ArrayList CreateSql(DataTable dtKG,string fwsbh, string bz)
    {
        ArrayList al = new ArrayList();

        if (dtKG !=null&&dtKG.Rows.Count > 0)
        {
            for (int i = 0; i < dtKG.Rows.Count; i++)
            {
                //获取空鼓使用明细表新插入数据的number
                WorkFlowModule WFMkg = new WorkFlowModule("CWGL_KGSYMXJLB");
                string KeyNumberKG = WFMkg.numberFormat.GetNextNumber();
                string sql_insert = "insert into CWGL_KGSYMXJLB (number,ssbsc,fwsbh,fwsmc,djlb,djly,gldh,syfs,sysj,cplb,cpxh,cpsl,kgxh,yz,lanz,lvz,hz,bz) values ('" + KeyNumberKG + "','" + dtKG.Rows[i]["所属办事处"].ToString() + "','" + fwsbh + "','" + dtKG.Rows[i]["服务商名称"].ToString() + "','授信使用','富美直通车','" + dtKG.Rows[i]["关联单号"].ToString() + "','返还授信',getdate(),'" + dtKG.Rows[i]["产品类别"].ToString() + "','" + dtKG.Rows[i]["产品型号"].ToString() + "','" + dtKG.Rows[i]["产品数量"].ToString() + "','" + dtKG.Rows[i]["空鼓型号"].ToString() + "','" + dtKG.Rows[i]["原装旧鼓量"].ToString() + "','" + dtKG.Rows[i]["蓝装旧鼓量"].ToString() + "','" + dtKG.Rows[i]["绿装旧鼓量"].ToString() + "','" + dtKG.Rows[i]["红装旧鼓量"].ToString() + "','" + bz + "')";
                al.Add(sql_insert);

                int yfhsl = Convert.ToInt32(dtKG.Rows[i]["原装旧鼓量"].ToString()) + Convert.ToInt32(dtKG.Rows[i]["蓝装旧鼓量"].ToString()) + Convert.ToInt32(dtKG.Rows[i]["绿装旧鼓量"].ToString()) + Convert.ToInt32(dtKG.Rows[i]["红装旧鼓量"].ToString());
                //生成更新授信空鼓使用明细表的数据
                string sql_update = "update FWPT_FWSSXKGSYMXB set YFHSL=YFHSL+" + yfhsl + " where number='" + dtKG.Rows[i]["关联单号"].ToString() + "'";
                al.Add(sql_update);
            }
            //生成更新授信空鼓记录表的sql语句
            int zsl = Convert.ToInt32(dtKG.Compute("sum([原装旧鼓量])", "true").ToString()) + Convert.ToInt32(dtKG.Compute("sum([蓝装旧鼓量])", "true").ToString()) + Convert.ToInt32(dtKG.Compute("sum([绿装旧鼓量])", "true").ToString()) + Convert.ToInt32(dtKG.Compute("sum([红装旧鼓量])", "true").ToString());
            string sql_upd = "update FWPT_FWSKGSXJLB set DQKYSL=DQKYSL+" + zsl + " where fwsbh='" + fwsbh + "'";
            al.Add(sql_upd);
        }  
        return al;
    }

    #endregion
    
    //2013财年服务商业务全部转回总部，从新从总部获取相关数据
    #region 获取服务商当前可用信用余额
    /// <summary>
    /// 获取服务商当前可用信用额度
    /// </summary>
    /// <param name="UserNumber">服务商编号</param>
    /// <returns></returns>
    public double GetCreditMoney(string UserNumber)
    {
        #region ERP信用额度计算部分
        //订单中未填写过销货单的总金额 
        string sql1 = "select TC004 as 客户编号,sum(cast((TD008-TD009)*TD011 as numeric(18,2))) as 金额 from COPTD left join COPTC on TC001=TD001 and TC002=TD002 and COPTC.TSFM_bsc=COPTD.TSFM_bsc  where ltrim(rtrim(TC004))='" + UserNumber + "' and TC027='Y' and TD016='N' group by TC004 ";

        //销货单中未开过销售发票的总金额 
        string sql2 = "select TG004 as 客户编号,sum((case cast(TH008 as int) when 0 then cast((TH035+TH036) as numeric(18,2)) else cast((TH008-TH042)*TH012 as numeric(18,2)) end)) as 金额 from COPTG left join COPTH on TH001=TG001 and TH002=TG002 and COPTG.TSFM_bsc=COPTH.TSFM_bsc  where ltrim(rtrim(TG004)) = '" + UserNumber + "' and TG023='Y' and TH026='N' group by TG004";

        //销退单中未开过发票的金额       
        string sql3 = "select TI004 as 客户编号,sum((case cast(TJ007 as int) when 0 then cast(TJ012 as numeric(18,2)) else cast((TJ007-TJ037)*TJ011 as numeric(18,2)) end) ) as 金额 from COPTI left join COPTJ on TI001=TJ001 and TI002=TJ002 and COPTJ.TSFM_bsc=COPTI.TSFM_bsc  where ltrim(rtrim(TI004))='" + UserNumber + "' and TI019='Y' and TJ024='N' group by TI004";

        //开过的发票总额
        string sql4 = "select TA004 as 客户编号,sum(TA041+TA042-TA098) as 金额 from ACRTA where ltrim(rtrim(TA004))='" + UserNumber + "' and TA025='Y' and TA079='1'  group by TA004";

        string sql4_2 = "select TA004 as 客户编号, -sum(TA041+TA042-TA098) as 金额 from ACRTA where ltrim(rtrim(TA004))='" + UserNumber + "' and TA025='Y' and TA079='2'  group by TA004";

        //信用额度初始值
        string sql5 = "select MA001 as 客户编号,MA033*(1+MA034) as 金额 from COPMA where ltrim(rtrim(MA001))='" + UserNumber + "' and MA097='1'";
        //收款单中未核销的部分的总额
        string sql6 = "select TK004 as 客户编号,sum(TK033+TK035+TK036-TK038) as 金额 from ACRTK where ltrim(rtrim(TK004))= '" + UserNumber + "' and TK020='Y' and TK030!=3 group by TK004";

        //退款单中未核销部分的总额
        string sql7 = "select TK004 as 客户编号,sum(TK033+TK035+TK036-TK038) as 金额 from ACRTK where ltrim(rtrim(TK004))= '" + UserNumber + "' and TK020='Y' and TK001='6D' and TK030!=3  group by TK004";

        //string sql_ERP = "select 客户编号,sum(金额) as 金额 from (" + sql1 + " union all " + sql2 + " union all " + sql3 + " union all " + sql4 + " union all " + sql4_2 + " union all " + sql5 + " union all " + sql6 + " union all " + sql7 + ") as tab group by 客户编号";
        #endregion

        //业务平台中未录入销货单的订单金额
        string sql8 = "select number as 订单号,thfs as 提货方式,cppl as 产品品类,cpxh as 产品型号,isnull(cpsl,0) as 产品数量,0 as 已交数量,isnull(thj,0) as 提货价,isnull(yj,0) as 押金,isnull(cast((thj+yj)*cpsl as numeric(18,2)),0) as 剩余金额 from FWPT_FWSDHD as a left join  FWPT_FWSDHD_CPLB as b on a.number=b.parentnumber where fwsbh= '" + UserNumber + "'  and isnull(qzjs,'')<>'是'";

        //新版合格旧硒鼓交易中心ERP中未审核的收款单，因审核后的收款单转入了“收款单中未核销部分的总额中计算了”
        string sql9 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6C1' and TK004='" + UserNumber + "' and TK020<>'Y'";
        //新版合格旧硒鼓交易中心ERP中未审核的退款单
        string sql10 = "select -isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6D1' and TK004='" + UserNumber + "' and TK020<>'Y'";

        //新版合格旧硒鼓交易中心被冻结的额度(剩余可交易量总额)
        string sql11 = "select  -isnull(sum(SYKMSL*DJ),0) as 金额  from NewPurchase where MRFKHBH='" + UserNumber + "' and JYZT <>'5' and JYZT <>'6' ";
        //新版合格旧硒鼓交易中心在成功交易明细表中的收款额度(转换到ERP之前的临时数据)
        string sql12 = "select  isnull(sum(CJZJ),0) as 金额  from CGJYMXB where KHBH='" + UserNumber + "' and ZHZT ='0' and MMLX ='卖出'";
        //新版合格旧硒鼓交易中心在成功交易明细表中的退款额度(转换到ERP之前的临时数据)
        string sql13 = "select  -isnull(sum(CJZJ),0) as 金额  from CGJYMXB where KHBH='" + UserNumber + "' and ZHZT ='0' and MMLX ='买入'";
        //新版合格旧硒鼓交易中心提现申请中的信用额度扣减(只扣减已生效的，其他状态，将在ERP信用额度中体现)
        string sql14 = "select  -isnull(sum(TXED),0) as 金额  from SQTX where KHBH='" + UserNumber + "' and ZT ='已生效'";

        //中转收款单中的数据
        string sql15 = "select isnull(sum(case jeys when 'ADD' then JE when 'DEL' then -JE end),0) as 金额 from FM_ZZSKD where KHBH='" + UserNumber + "' and isnull(ZHERPZT,'')<>'是' and isnull(JLZT,'')<>'作废'";

        //中转收款单中扣减交易大厅服务费转入ERP未审核的数据
        string sql15_1 = "select isnull(sum(TK033),0) as 金额 from ACRTK where TK001 = '6C4' and TK004='" + UserNumber + "' and TK020<>'Y'";


        DataSet ds_1 = GetErpData.GetDataSet(sql1,"ERP大视图");
        DataSet ds_2 = GetErpData.GetDataSet(sql2,"ERP大视图");
        DataSet ds_3 = GetErpData.GetDataSet(sql3,"ERP大视图");
        DataSet ds_4 = GetErpData.GetDataSet(sql4,"ERP大视图");
        DataSet ds_4_2 = GetErpData.GetDataSet(sql4_2, "ERP大视图");
        DataSet ds_5 = GetErpData.GetDataSet(sql5, "ERP大视图");
        DataSet ds_6 = GetErpData.GetDataSet(sql6, "ERP大视图");
        DataSet ds_7 = GetErpData.GetDataSet(sql7, "ERP大视图");

        DataSet ds_9 = GetErpData.GetDataSet(sql9, "ERP大视图");
        DataSet ds_10 = GetErpData.GetDataSet(sql10, "ERP大视图");
        DataSet ds_11 = DbHelperSQL.Query(sql11);
        DataSet ds_12 = DbHelperSQL.Query(sql12);
        DataSet ds_13 = DbHelperSQL.Query(sql13);
        DataSet ds_14 = DbHelperSQL.Query(sql14);
        DataSet ds_15 = DbHelperSQL.Query(sql15);
        DataSet ds_15_1 = GetErpData.GetDataSet(sql15_1, "ERP大视图");

        //计算订单未转销货单金额
        DataSet ds8 = DbHelperSQL.Query(sql8);  
        if (ds8 != null && ds8.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds8.Tables[0].Rows.Count; i++)
            {
                string ddh = ds8.Tables[0].Rows[i]["订单号"].ToString();
                string thfs = ds8.Tables[0].Rows[i]["提货方式"].ToString();
                string yjje = ds8.Tables[0].Rows[i]["押金"].ToString();
                string cpjg = ds8.Tables[0].Rows[i]["提货价"].ToString();
                string cppl = ds8.Tables[0].Rows[i]["产品品类"].ToString();
                string cpxh = ds8.Tables[0].Rows[i]["产品型号"].ToString();

                string db = "";
                if (thfs == "循环提货")
                {
                    db = "2313";
                }
                else if (thfs == "押金提货" )
                {
                    if (yjje == "0.00")
                    {
                        db = "2302";
                    }
                    else
                    {
                        db = "2312";
                    }
                }
                else if (thfs == "无押金提货")
                {
                    db = "2301";
                }
                string sqlXH = "";
                if (thfs == "循环提货" || thfs == "无押金提货" || (thfs == "押金提货" && yjje == "0.00"))
                {
                    sqlXH = "select isnull(sum(cast(TH008 as int)),0) as 已交付数量,isnull(sum(cast(TH013 as numeric(18,2))),0.00) as 已交金额 from COPTG left join COPTH on TG001=TH001 and TG002=TH002 and COPTG.TSFM_bsc=COPTH.TSFM_bsc where TG004='" + UserNumber + "' and COPTG.UDF01='" + ddh + "' and TG001 in ('" + db + "') and (case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' when '153' then '红装' end)='" + cppl + "' and ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005))))='" + cpxh + "' and TG023='Y' and cast(TH012 as numeric(18,2))=" + cpjg + "";
                }
                else if (thfs == "押金提货" && yjje != "0.00")
                {
                    sqlXH = "select isnull(sum(cast(已交付数量 as int)),0) as 已交付数量,isnull(sum(cast(提货金合计+押金合计 as numeric(18,2))),0.00) as 已交金额 from (select TH004,TH008 as 已交付数量,TH013 as 提货金合计 from COPTG left join COPTH on TG001=TH001 and TG002=TH002 and COPTG.TSFM_bsc=COPTH.TSFM_bsc where TG004='" + UserNumber + "' and COPTG.UDF01='" + ddh + "' and TG001 in ('" + db + "') and (case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' when '153' then '红装' end)='" + cppl + "' and substring(TH005,charindex(' ',TH005),len(TH005)) like '%" + cpxh + "%' and TG023='Y' and  cast(TH012 as numeric(18,2))='" + cpjg + "') as a left join (select distinct TH004 ,isnull(cast(TH013/TH012 as int),0) as 数量,isnull(cast(TH013 as numeric(18,2)),0.00) as 押金合计 from COPTG left join COPTH on TG001=TH001 and TG002=TH002 and COPTG.TSFM_bsc=COPTH.TSFM_bsc where TG004='" + UserNumber + "' and COPTG.UDF01='" + ddh + "' and TG001 in ('" + db + "') and (case substring(ltrim(rtrim(TH004)),1,3) when '151' then '绿装'  when '150' then '蓝装' when '153' then '红装' end)='" + cppl + "' and ltrim(rtrim(substring(TH005,charindex(' ',TH005),len(TH005))))='" + cpxh + "' and TG023='Y' and  cast(TH012 as numeric(18,2))='" + yjje + "') as b on a.TH004=b.TH004 and a.已交付数量=b.数量";
                }

                DataSet dsERP_XH = GetErpData.GetDataSet(sqlXH,"ERP大视图");
                if (dsERP_XH != null && dsERP_XH.Tables[0].Rows.Count > 0)
                {
                    ds8.Tables[0].Rows[i]["已交数量"] = dsERP_XH.Tables[0].Rows[0]["已交付数量"];
                    ds8.Tables[0].Rows[i]["剩余金额"] = (Convert.ToInt32(ds8.Tables[0].Rows[i]["产品数量"]) - Convert.ToInt32(dsERP_XH.Tables[0].Rows[0]["已交付数量"])) * (Convert.ToDecimal(ds8.Tables[0].Rows[i]["提货价"]) + Convert.ToDecimal(ds8.Tables[0].Rows[i]["押金"]));
                    if (Convert.ToDouble(ds8.Tables[0].Rows[i]["剩余金额"]) < 0)
                    {
                        ds8.Tables[0].Rows[i]["剩余金额"] = 0.00;
                    }
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
        double d15_1 = 0.00;

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
        if (ds_15_1 != null && ds_15_1.Tables[0].Rows.Count > 0)
        {
            d15_1 = Convert.ToDouble(ds_15_1.Tables[0].Rows[0]["金额"].ToString());
        }

        if (ds8 != null && ds8.Tables[0].Rows.Count > 0)
        {
            d8 = Convert.ToDouble(ds8.Tables[0].Compute("sum(剩余金额)", "true"));
        }

        double xyed = d5 + d6 - d7 - (d1 + (d2 - d3) + (d4 + d4_2)) - d8 + d9 + d10 + d11 + d12 + d13 + d14 + d15 + d15_1;
        return xyed;
    }
    #endregion



}
