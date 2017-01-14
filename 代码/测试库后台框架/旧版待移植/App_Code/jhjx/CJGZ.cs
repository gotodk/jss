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
///CJGZ 的摘要说明，处理成交规则业务逻辑(by 于海滨)
/// </summary>
public class CJGZ
{
	public CJGZ()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 处理成交规则的逻辑，查看是否在冷静期，查看是否能够凑齐，根据调用点进行不同的处理
    /// 传入的数据表，必须包含至少两列“商品编号”“合同期限”，且一定不能有重复数据
    /// 调用点包括： 提交、删除、修改、中标监控
    /// </summary>
    /// <param name="dtRun">必须包含至少两列“商品编号”“合同期限”</param>
    /// <param name="sp">调用点</param>
    /// <returns>返回要执行的SQL语句组</returns>
    public ArrayList RunCJGZ(DataTable dtRun, string sp)
    {

        if (dtRun == null || dtRun.Rows.Count < 1)
        {
            return null;
        }

        ArrayList ALsql = new ArrayList();

        //根据参数，获取需要处理的投标信息
        string sql_dsTB = "";
        for (int i = 0; i < dtRun.Rows.Count; i++)
        {
            string f = "";
            if (i != 0)
            {
                f = " union all ";
            }
            //按照价格从小到大，时间从晚到早找出top1
            sql_dsTB = sql_dsTB + f + " select * from ( select top 1 *,'是否凑齐'='否','成交量'=0 from ZZ_TBXXB join ZZ_TBXXBZB on ZZ_TBXXB.Number = ZZ_TBXXBZB.parentNumber where ZZ_TBXXBZB.SPBH = '" + dtRun.Rows[i]["商品编号"].ToString() + "' and  ZZ_TBXXBZB.HTQX = '" + dtRun.Rows[i]["合同期限"].ToString() + "' and  ZZ_TBXXBZB.ZT='竞标'  order  by ZZ_TBXXBZB.TBJG asc,ZZ_TBXXB.CreateTime asc ) as tab"+i.ToString()+"   ";
        }

        DataSet dsTB = DbHelperSQL.Query(sql_dsTB);
        if (dsTB == null || dsTB.Tables[0].Rows.Count < 1)
        {
            return null;
        }

        //动态参数
        Hashtable htParameterInfo = jhjx_PublicClass.GetParameterInfo();

        //遍历要处理的投标信息
        for (int i = 0; i < dsTB.Tables[0].Rows.Count; i++)
        {
            Int64 TBsl = Convert.ToInt64(dsTB.Tables[0].Rows[i]["TBYSL"]); //当前投标拟售量
            double TBJG = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBJG"]); //投标价格
            string HTZQ = dsTB.Tables[0].Rows[i]["HTQX"].ToString(); //合同期限
            string SPBH = dsTB.Tables[0].Rows[i]["SPBH"].ToString(); //商品编号
            string SFJRLJQ = dsTB.Tables[0].Rows[i]["SFJRLJQ"].ToString(); //是否进入冷静期
            string BGHQY = dsTB.Tables[0].Rows[i]["BGHQY"].ToString(); //不发货区域
            //string[] arr_BGHQY = BGHQY.Substring(1, BGHQY.Length-2).Split('|');
            //string BGHQY_re = "'|大补|'";
            //foreach (string qy in arr_BGHQY)
            //{
            //    BGHQY_re = BGHQY_re + ",'|" + qy + "|'";
            //}

            //临时记录的语句，如果最后为凑齐，则这些语句都无效
            ArrayList alSQLtemp = new ArrayList();
            Int64 CQsl = 0; //凑齐数量
            //看某个投标信息能不能凑齐
            string sql_dsYDD = " select *,'本次成交'=0,'多余量'=0  from ZZ_YDDXXB where HTZQ='" + HTZQ + "' and ZT='竞标' and SPBH  = '" + SPBH + "' and NMRJG >= " + TBJG + " and charindex(SHQY,'" + BGHQY + "')<1   order  by CreateTime asc ";
            DataSet dsYDD = DbHelperSQL.Query(sql_dsYDD);
            if (dsYDD != null && dsYDD.Tables[0].Rows.Count > 0)
            {
                for (int y = 0; y < dsYDD.Tables[0].Rows.Count; y++)
                {
                    CQsl = CQsl + (Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YDZSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBL"]));
                    
                    //上限和下限
                    Int64 min = Convert.ToInt64(Math.Round(TBsl * 0.9));
                    Int64 max = Convert.ToInt64(Math.Round(TBsl * 1.1));

                    //凑到了大于预售量
                    if (CQsl >= min && TBsl > 0)
                    {
                        dsTB.Tables[0].Rows[i]["是否凑齐"] = "是";

                        if (CQsl > max)
                        {
                            //超浮动上限了，成交一部分
                            dsTB.Tables[0].Rows[i]["成交量"] = max;
                            try
                            {
                                Int64 aaa = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YDZSL"]);
                                Int64 bbb = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBL"]);
                            }
                            catch (Exception ex)
                            {
                                string aaaa = ex.ToString();
                            }
                            dsYDD.Tables[0].Rows[y]["本次成交"] = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YDZSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBL"]) - (CQsl - max);
                            dsYDD.Tables[0].Rows[y]["多余量"] = CQsl - max;
                        }
                        else
                        {
                            //未超浮动上限了，成交全部
                            dsTB.Tables[0].Rows[i]["成交量"] = CQsl;
                            dsYDD.Tables[0].Rows[y]["本次成交"] = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YDZSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBL"]);
                            dsYDD.Tables[0].Rows[y]["多余量"] = 0;
                        }

                        //监控专用，反写预订单
                        if (sp == "中标监控")
                        {
                            //反写预订单//不一定都写中标，未完全成交的，要写竞标。
                            string ZTstr = "";
                            if (Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) + Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBL"]) == Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YDZSL"]))
                            {
                                ZTstr = "中标";
                            }
                            else
                            {
                                ZTstr = "竞标";
                            }
                            
                            alSQLtemp.Add("update ZZ_YDDXXB set ZT = '" + ZTstr + "',YZBL=YZBL+ " + Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) + " where Number = '" + dsYDD.Tables[0].Rows[y]["Number"].ToString() + "' ");
                            //插入中标定标信息表一条数据
                            string LMstr = ""; //前缀单号
                            if (HTZQ == "半年")
                            { LMstr = "L"; }
                            if (HTZQ == "半年")
                            { LMstr = "M"; }
                            string Number = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZBDBXXB", LMstr);
                            string HTBH = Number;
                            string HTQX = HTZQ;
                            string SPBHSPBH = SPBH;
                            string SPMC = dsTB.Tables[0].Rows[i]["SPMC"].ToString();
                            string GGXH = dsTB.Tables[0].Rows[i]["GGXH"].ToString();
                            string JJDW = dsTB.Tables[0].Rows[i]["JJDW"].ToString();
                            string JJPL = dsTB.Tables[0].Rows[i]["JJPL"].ToString();
                            string SELTBXXZBH = dsTB.Tables[0].Rows[i]["Number"].ToString();
                            string MJTBXXChildBH = dsTB.Tables[0].Rows[i]["id"].ToString();
                            string SELDLYX = dsTB.Tables[0].Rows[i]["SELDLYX"].ToString();
                            string SELYHM = dsTB.Tables[0].Rows[i]["SELYHM"].ToString();
                            string SELJSBH = dsTB.Tables[0].Rows[i]["SELJSBH"].ToString();
                            string SELJJRDLYX = dsTB.Tables[0].Rows[i]["GLJJRDLYX"].ToString();
                            string SELJJRYHM = dsTB.Tables[0].Rows[i]["GLJJRYHM"].ToString();
                            string SELJJRJSBH = dsTB.Tables[0].Rows[i]["GLJJRJSBH"].ToString();
                            Int64 SELTBYSL = Convert.ToInt64(dsTB.Tables[0].Rows[i]["TBYSL"]);
                            double SELTBJG = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBJG"]);
                            string SELBGHQY = dsTB.Tables[0].Rows[i]["BGHQY"].ToString();
                            double XDTBXXSYSTBBZJJE = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBBZJJE"]);
                            double DQTBBZJJE = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBBZJJE"]);
                            double TBBZJSYKYJE = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBBZJSYKYJE"]);
                            string SELCYJBSJ = dsTB.Tables[0].Rows[i]["CreateTime"].ToString();
                            string BUYYDDBH = dsYDD.Tables[0].Rows[y]["Number"].ToString();
                            string BUYDLYX = dsYDD.Tables[0].Rows[y]["MJDLYX"].ToString();
                            string MJYHM = dsYDD.Tables[0].Rows[y]["MJYHM"].ToString();
                            string BUYJSBH = dsYDD.Tables[0].Rows[y]["MJJSBH"].ToString();
                            string BUYJJRDLYX = dsYDD.Tables[0].Rows[y]["GLDLRDLYX"].ToString();
                            string BUYJJRYHM = dsYDD.Tables[0].Rows[y]["GLDLRYHM"].ToString();
                            string BUYJJRJSBH = dsYDD.Tables[0].Rows[y]["GLDLRJSBH"].ToString();
                            double BUYMRJG = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["NMRJG"]);
                            Int64 BUYYDZSL = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YDZSL"]);
                            Int64 BUYYZBL = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBL"]);
                            Int64 BUYYTHSL = 0;
                            double BUYYDDYSDJJE = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["ZFDJJE"]);
                            double BUYDQDJJE = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["ZFDJJE"]);
                            double BUYDJSYKYJE = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["ZFDJJE"]);
                            string BUYXDYDDSJ = dsYDD.Tables[0].Rows[y]["CreateTime"].ToString();
                            string MJSHQY = dsYDD.Tables[0].Rows[y]["SHQY"].ToString();
                            string ZBDBZT = "中标";
                            string ZBSJ = DateTime.Now.ToString();
                            Int64 ZBSL = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]);
                            double ZBZJE = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) * SELTBJG;
                            string SFJNLYBZJ = "否";
                            double DBSYSLYBZJJE = 0;//现在不写，后边代码统一反写
                            double DQLYBZJJE = 0;//现在不写，后边代码统一反写
                            double LYBZJSYKYJE = 0;//现在不写，后边代码统一反写
                            string DBSJ = "null";
                            string HTJSYYFL = "";
                            string CreateTime = ZBSJ;


                            alSQLtemp.Add("INSERT INTO ZZ_ZBDBXXB (Number,HTBH,HTQX,SPBH,SPMC,GGXH,JJDW,JJPL,SELTBXXZBH,MJTBXXChildBH,SELDLYX,SELYHM,SELJSBH,SELJJRDLYX,SELJJRYHM,SELJJRJSBH,SELTBYSL,SELTBJG,SELBGHQY,XDTBXXSYSTBBZJJE,DQTBBZJJE,TBBZJSYKYJE,SELCYJBSJ,BUYYDDBH,BUYDLYX,MJYHM,BUYJSBH,BUYJJRDLYX,BUYJJRYHM,BUYJJRJSBH,BUYMRJG,BUYYDZSL,BUYYZBL,BUYYTHSL,BUYYDDYSDJJE,BUYDQDJJE,BUYDJSYKYJE,BUYXDYDDSJ,MJSHQY,ZBDBZT,ZBSJ,ZBSL,ZBZJE,SFJNLYBZJ,DBSYSLYBZJJE,DQLYBZJJE,LYBZJSYKYJE,DBSJ,HTJSYYFL,CreateTime) VALUES('" + Number + "','" + HTBH + "','" + HTQX + "','" + SPBHSPBH + "','" + SPMC + "','" + GGXH + "','" + JJDW + "','" + JJPL + "','" + SELTBXXZBH + "','" + MJTBXXChildBH + "','" + SELDLYX + "','" + SELYHM + "','" + SELJSBH + "','" + SELJJRDLYX + "','" + SELJJRYHM + "','" + SELJJRJSBH + "'," + SELTBYSL + "," + SELTBJG + ",'" + SELBGHQY + "'," + XDTBXXSYSTBBZJJE + "," + DQTBBZJJE + "," + TBBZJSYKYJE + ",'" + SELCYJBSJ + "','" + BUYYDDBH + "','" + BUYDLYX + "','" + MJYHM + "','" + BUYJSBH + "','" + BUYJJRDLYX + "','" + BUYJJRYHM + "','" + BUYJJRJSBH + "'," + BUYMRJG + "," + BUYYDZSL + "," + BUYYZBL + "," + BUYYTHSL + "," + BUYYDDYSDJJE + "," + BUYDQDJJE + "," + BUYDJSYKYJE + ",'" + BUYXDYDDSJ + "','" + MJSHQY + "','" + ZBDBZT + "','" + ZBSJ + "'," + ZBSL + "," + ZBZJE + ",'" + SFJNLYBZJ + "'," + DBSYSLYBZJJE + "," + DQLYBZJJE + "," + LYBZJSYKYJE + "," + DBSJ + ",'" + HTJSYYFL + "','" + CreateTime + "') ");
                        }


                        y = dsYDD.Tables[0].Rows.Count + 10;//强制退出这个for循环，只退出一层  
                    }
                    else//未凑到预售量，继续凑，但如果是中标监控，先把语句生成好。
                    {
                        //监控专用，反写预订单
                        if (sp == "中标监控")
                        {
                            dsYDD.Tables[0].Rows[y]["本次成交"] = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YDZSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBL"]);
                            //虽然未凑齐，但中标数据先记下来
                            alSQLtemp.Add("update ZZ_YDDXXB set ZT = '中标',YZBL=YZBL+ " + Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) + " where Number = '" + dsYDD.Tables[0].Rows[y]["Number"].ToString() + "' ");
                            //插入中标定标信息表一条数据
                            string LMstr = ""; //前缀单号
                            if (HTZQ == "半年")
                            { LMstr = "L"; }
                            if (HTZQ == "半年")
                            { LMstr = "M"; }
                            string Number = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZBDBXXB", LMstr);
                            string HTBH = Number;
                            string HTQX = HTZQ;
                            string SPBHSPBH = SPBH;
                            string SPMC = dsTB.Tables[0].Rows[i]["SPMC"].ToString();
                            string GGXH = dsTB.Tables[0].Rows[i]["GGXH"].ToString();
                            string JJDW = dsTB.Tables[0].Rows[i]["JJDW"].ToString();
                            string JJPL = dsTB.Tables[0].Rows[i]["JJPL"].ToString();
                            string SELTBXXZBH = dsTB.Tables[0].Rows[i]["Number"].ToString();
                            string MJTBXXChildBH = dsTB.Tables[0].Rows[i]["id"].ToString();
                            string SELDLYX = dsTB.Tables[0].Rows[i]["SELDLYX"].ToString();
                            string SELYHM = dsTB.Tables[0].Rows[i]["SELYHM"].ToString();
                            string SELJSBH = dsTB.Tables[0].Rows[i]["SELJSBH"].ToString();
                            string SELJJRDLYX = dsTB.Tables[0].Rows[i]["GLJJRDLYX"].ToString();
                            string SELJJRYHM = dsTB.Tables[0].Rows[i]["GLJJRYHM"].ToString();
                            string SELJJRJSBH = dsTB.Tables[0].Rows[i]["GLJJRJSBH"].ToString();
                            Int64 SELTBYSL = Convert.ToInt64(dsTB.Tables[0].Rows[i]["TBYSL"]);
                            double SELTBJG = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBJG"]);
                            string SELBGHQY = dsTB.Tables[0].Rows[i]["BGHQY"].ToString();
                            double XDTBXXSYSTBBZJJE = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBBZJJE"]);
                            double DQTBBZJJE = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBBZJJE"]);
                            double TBBZJSYKYJE = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBBZJSYKYJE"]);
                            string SELCYJBSJ = dsTB.Tables[0].Rows[i]["CreateTime"].ToString();
                            string BUYYDDBH = dsYDD.Tables[0].Rows[y]["Number"].ToString();
                            string BUYDLYX = dsYDD.Tables[0].Rows[y]["MJDLYX"].ToString();
                            string MJYHM = dsYDD.Tables[0].Rows[y]["MJYHM"].ToString();
                            string BUYJSBH = dsYDD.Tables[0].Rows[y]["MJJSBH"].ToString();
                            string BUYJJRDLYX = dsYDD.Tables[0].Rows[y]["GLDLRDLYX"].ToString();
                            string BUYJJRYHM = dsYDD.Tables[0].Rows[y]["GLDLRYHM"].ToString();
                            string BUYJJRJSBH = dsYDD.Tables[0].Rows[y]["GLDLRJSBH"].ToString();
                            double BUYMRJG = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["NMRJG"]);
                            Int64 BUYYDZSL = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YDZSL"]);
                            Int64 BUYYZBL = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBL"]);
                            Int64 BUYYTHSL = 0;
                            double BUYYDDYSDJJE = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["ZFDJJE"]);
                            double BUYDQDJJE = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["ZFDJJE"]);
                            double BUYDJSYKYJE = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["ZFDJJE"]);
                            string BUYXDYDDSJ = dsYDD.Tables[0].Rows[y]["CreateTime"].ToString();
                            string MJSHQY = dsYDD.Tables[0].Rows[y]["SHQY"].ToString();
                            string ZBDBZT = "中标";
                            string ZBSJ = DateTime.Now.ToString();
                            Int64 ZBSL = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]);
                            double ZBZJE = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) * SELTBJG;
                            string SFJNLYBZJ = "否";
                            double DBSYSLYBZJJE = 0; //现在不写，后边代码统一反写
                            double DQLYBZJJE = 0;//现在不写，后边代码统一反写
                            double LYBZJSYKYJE = 0;//现在不写，后边代码统一反写
                            string DBSJ = "null";
                            string HTJSYYFL = "";
                            string CreateTime = ZBSJ;


                            alSQLtemp.Add("INSERT INTO ZZ_ZBDBXXB (Number,HTBH,HTQX,SPBH,SPMC,GGXH,JJDW,JJPL,SELTBXXZBH,MJTBXXChildBH,SELDLYX,SELYHM,SELJSBH,SELJJRDLYX,SELJJRYHM,SELJJRJSBH,SELTBYSL,SELTBJG,SELBGHQY,XDTBXXSYSTBBZJJE,DQTBBZJJE,TBBZJSYKYJE,SELCYJBSJ,BUYYDDBH,BUYDLYX,MJYHM,BUYJSBH,BUYJJRDLYX,BUYJJRYHM,BUYJJRJSBH,BUYMRJG,BUYYDZSL,BUYYZBL,BUYYTHSL,BUYYDDYSDJJE,BUYDQDJJE,BUYDJSYKYJE,BUYXDYDDSJ,MJSHQY,ZBDBZT,ZBSJ,ZBSL,ZBZJE,SFJNLYBZJ,DBSYSLYBZJJE,DQLYBZJJE,LYBZJSYKYJE,DBSJ,HTJSYYFL,CreateTime) VALUES('" + Number + "','" + HTBH + "','" + HTQX + "','" + SPBHSPBH + "','" + SPMC + "','" + GGXH + "','" + JJDW + "','" + JJPL + "','" + SELTBXXZBH + "','" + MJTBXXChildBH + "','" + SELDLYX + "','" + SELYHM + "','" + SELJSBH + "','" + SELJJRDLYX + "','" + SELJJRYHM + "','" + SELJJRJSBH + "'," + SELTBYSL + "," + SELTBJG + ",'" + SELBGHQY + "'," + XDTBXXSYSTBBZJJE + "," + DQTBBZJJE + "," + TBBZJSYKYJE + ",'" + SELCYJBSJ + "','" + BUYYDDBH + "','" + BUYDLYX + "','" + MJYHM + "','" + BUYJSBH + "','" + BUYJJRDLYX + "','" + BUYJJRYHM + "','" + BUYJJRJSBH + "'," + BUYMRJG + "," + BUYYDZSL + "," + BUYYZBL + "," + BUYYTHSL + "," + BUYYDDYSDJJE + "," + BUYDQDJJE + "," + BUYDJSYKYJE + ",'" + BUYXDYDDSJ + "','" + MJSHQY + "','" + ZBDBZT + "','" + ZBSJ + "'," + ZBSL + "," + ZBZJE + ",'" + SFJNLYBZJ + "'," + DBSYSLYBZJJE + "," + DQLYBZJJE + "," + LYBZJSYKYJE + "," + DBSJ + ",'" + HTJSYYFL + "','" + CreateTime + "') ");
                        }
                    }
                    
                }

                //凑够了
                if (dsTB.Tables[0].Rows[i]["是否凑齐"].ToString() == "是")
                {
                    //监控专用，反写投标信息，反写履约保证金
                    if (sp == "中标监控")
                    {
                        //把临时语句增加进去
                        ALsql.AddRange(alSQLtemp);
                        //反写投标信息
                        ALsql.Add("update ZZ_TBXXBZB set ZT = '中标'  where id = '" + dsTB.Tables[0].Rows[i]["id"].ToString() + "' and parentNumber = '" + dsTB.Tables[0].Rows[i]["Number"].ToString() + "' ");
                        double lybzj = Math.Round(Convert.ToDouble(dsTB.Tables[0].Rows[i]["成交量"]) * Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBJG"]) * Convert.ToDouble(htParameterInfo["履约保证金比率"]), 2);
                        ALsql.Add("update ZZ_ZBDBXXB set DBSYSLYBZJJE=" + lybzj + ",DQLYBZJJE=" + lybzj + ",LYBZJSYKYJE=" + lybzj + "  where MJTBXXChildBH = '" + dsTB.Tables[0].Rows[i]["id"].ToString() + "' and SELTBXXZBH = '" + dsTB.Tables[0].Rows[i]["Number"].ToString() + "' ");
                    }
                }
            }

            //看这个投标信息的商品是否进入了冷静期，对冷静期控制进行处理
            if (SFJRLJQ == "是") //在冷静期
            {
                if (dsTB.Tables[0].Rows[i]["是否凑齐"].ToString() == "是") //可以凑齐
                {
                    //冷静期状态不动，时间不动

                    //处理中标监控
                    if (sp == "中标监控")
                    {
                        //更新冷静期状态为否，时间置空
                        ALsql.Add(" update ZZ_TBXXBZB set SFJRLJQ = '否',JRLJQSJ = null where  HTQX = '" + HTZQ + "' and SPBH  = '" + SPBH + "' ");
                        ALsql.Add(" update ZZ_YDDXXB set SFJRLJQ = '否',JRLJQSJ = null where  HTZQ = '" + HTZQ + "' and SPBH  = '" + SPBH + "' ");
                        //记录历史记录
                        string newnumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_LJQLSJL", "");
                        ALsql.Add(" INSERT INTO ZZ_LJQLSJL (Number,SPBH,HTZQ,LJQZTBGXW,LJQSJBGXW) VALUES ('" + newnumber + "','" + SPBH + "','" + HTZQ + "','否',null) ");
                    }
                }
                else //不能凑齐
                {
                    //处理中标监控，只有通过中标监控，才会取消冷静期，中标监控只会跑到冷静期应该结束的数据
                    if (sp == "中标监控")
                    {
                        //更新冷静期状态为否，时间置空
                        ALsql.Add(" update ZZ_TBXXBZB set SFJRLJQ = '否',JRLJQSJ = null where   HTQX = '" + HTZQ + "' and SPBH  = '" + SPBH + "' ");
                        ALsql.Add(" update ZZ_YDDXXB set SFJRLJQ = '否',JRLJQSJ = null where   HTZQ = '" + HTZQ + "' and SPBH  = '" + SPBH + "' ");
                        string newnumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_LJQLSJL", "");
                        ALsql.Add(" INSERT INTO ZZ_LJQLSJL (Number,SPBH,HTZQ,LJQZTBGXW,LJQSJBGXW) VALUES ('" + newnumber + "','" + SPBH + "','" + HTZQ + "','否',null) ");
                    }
                }
            }
            else //不在冷静期
            {
                if (dsTB.Tables[0].Rows[i]["是否凑齐"].ToString() == "是") //可以凑齐
                {
                    //更新冷静期状态为是，时间更新为当前时间。
                    string t = DateTime.Now.ToString();
                    ALsql.Add(" update ZZ_TBXXBZB set SFJRLJQ = '是',JRLJQSJ = '" + t + "' where ZT='竞标' and  HTQX = '" + HTZQ + "' and SPBH  = '" + SPBH + "' ");
                    ALsql.Add(" update ZZ_YDDXXB set SFJRLJQ = '是',JRLJQSJ =  '" + t + "' where ZT='竞标' and  HTZQ = '" + HTZQ + "' and SPBH  = '" + SPBH + "' ");
                    //记录历史记录
                    string newnumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_LJQLSJL", "");
                    ALsql.Add(" INSERT INTO ZZ_LJQLSJL (Number,SPBH,HTZQ,LJQZTBGXW,LJQSJBGXW) VALUES ('" + newnumber + "','" + SPBH + "','" + HTZQ + "','是','" + t + "') ");
                }
                else //不能凑齐
                {
                    //冷静期状态不动，时间不动
                }
            }


        }




        return ALsql;
    }


}