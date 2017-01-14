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
///CJGZ 的摘要说明，新版，处理成交规则业务逻辑(by 于海滨)
/// </summary>
public class CJGZ2013
{
    public CJGZ2013()
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
        ArrayList ALsql_saveLJQ_TBD = new ArrayList();
        ArrayList ALsql = new ArrayList();


        ALsql.Add("   ");
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
            sql_dsTB = sql_dsTB + f + " select * from ( select top 1 ZZZ.Number,ZZZ.DLYX,ZZZ.JSZHLX,ZZZ.MJJSBH,ZZZ.GLJJRYX,ZZZ.GLJJRYHM,ZZZ.GLJJRJSBH,ZZZ.GLJJRPTGLJG,ZZZ.SPBH,ZZZ.SPMC,ZZZ.GG,ZZZ.JJDW,ZZZ.PTSDZDJJPL,ZZZ.HTQX,ZZZ.MJSDJJPL,ZZZ.GHQY,ZZZ.TBNSL,ZZZ.TBJG,ZZZ.TBJE,ZZZ.MJTBBZJBL,ZZZ.MJTBBZJZXZ,ZZZ.DJTBBZJ,ZZZ.ZT,ZZZ.TJSJ,ZZZ.CXSJ,ZZZ.ZLBZYZM,ZZZ.CPJCBG,ZZZ.PGZFZRFLCNS,ZZZ.FDDBRCNS,ZZZ.SHFWGDYCN,ZZZ.CPSJSQS,ZZZ.CreateTime,LLL.LJQKSSJ,LLL.LJQJSSJ,LLL.SFJRLJQ,LLL.LJQYZBS,'是否凑齐'='否','成交量'=0,'是否在冷静期内'=isnull(LLL.SFJRLJQ,'否') from AAA_TBD as ZZZ left join AAA_LJQDQZTXXB as LLL on LLL.SPBH=ZZZ.SPBH and LLL.HTQX=ZZZ.HTQX where ZZZ.SPBH = '" + dtRun.Rows[i]["商品编号"].ToString() + "' and  ZZZ.HTQX = '" + dtRun.Rows[i]["合同期限"].ToString() + "' and  ZZZ.ZT='竞标' AND ZZZ.HTQX <> '即时'  order  by ZZZ.TBJG asc,ZZZ.TJSJ asc ) as tab" + i.ToString() + "   ";//AND ZZZ.HTQX <> '即时'
        }

        DataSet dsTB = DbHelperSQL.Query(sql_dsTB);
        //没找到符合条件的投标单
        if (dsTB == null || dsTB.Tables[0].Rows.Count < 1)
        {
            return null;
        }

        //动态参数
        Hashtable htParameterInfo = PublicClass2013.GetParameterInfo();

        //遍历要处理的投标信息
        for (int i = 0; i < dsTB.Tables[0].Rows.Count; i++)
        {

            //参与本轮并中标的卖方数量(不用算，一直就是1) 
            Int64 sum_Z_Cynum_ZB_Sel = 1;
            //参与本轮并中标的买方数量(累加，但要过滤重复的买方)
            Hashtable htsum_Z_Cynum_ZB_Buy = new Hashtable();
            //参与本轮中标总金额(累加)
            double sum_Z_ZB_JE = 0;
            //参与本轮中标总数量(累加)
            Int64 sum_Z_ZB_SL = 0;

            string ZBSJ = DateTime.Now.ToString(); //中标时间，本次产生的数据保持一致，所以需要放到这里

            Int64 TBsl = Convert.ToInt64(dsTB.Tables[0].Rows[i]["TBNSL"]); //当前投标拟售量
            double TBJG = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBJG"]); //投标价格
            string HTQX = dsTB.Tables[0].Rows[i]["HTQX"].ToString(); //合同期限
            string SPBH = dsTB.Tables[0].Rows[i]["SPBH"].ToString(); //商品编号
            string SFJRLJQ = dsTB.Tables[0].Rows[i]["是否在冷静期内"].ToString(); //是否进入冷静期
            string GHQY = dsTB.Tables[0].Rows[i]["GHQY"].ToString(); //供货区域

            //若是中标监控，先给这个商品加锁，避免再发布投标信息和预订单。(这个其实也不能写到这里，要写到中标监控获取待处理商品的地方。)
            if (sp == "中标监控")
            {
                DbHelperSQL.ExecuteSql("update AAA_LJQDQZTXXB set LJQYZBS = '已锁' where SPBH='" + SPBH + "' and HTQX='" + HTQX + "'");
            }

            //临时记录的语句，如果最后为凑齐，则这些语句都无效
            ArrayList alSQLtemp = new ArrayList();
            Int64 CQsl = 0; //凑齐数量
            //看某个投标信息能不能凑齐
            string sql_dsYDD = " select *,'本次成交'=0,'多余量'=0  from AAA_YDDXXB where HTQX='" + HTQX + "' and ZT='竞标' and SPBH  = '" + SPBH + "' and NMRJG >= " + TBJG + " and charindex('|'+SHQYsheng+'|','" + GHQY + "')>0   order  by CreateTime asc  ";
            DataSet dsYDD = DbHelperSQL.Query(sql_dsYDD);
            if (dsYDD != null && dsYDD.Tables[0].Rows.Count > 0)
            {
                for (int y = 0; y < dsYDD.Tables[0].Rows.Count; y++)
                {
                    CQsl = CQsl + (Convert.ToInt64(dsYDD.Tables[0].Rows[y]["NDGSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]));
                    
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
                            dsYDD.Tables[0].Rows[y]["本次成交"] = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["NDGSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]) - (CQsl - max);
                            dsYDD.Tables[0].Rows[y]["多余量"] = CQsl - max;
                        }
                        else
                        {
                            //未超浮动上限了，成交全部
                            dsTB.Tables[0].Rows[i]["成交量"] = CQsl;
                            dsYDD.Tables[0].Rows[y]["本次成交"] = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["NDGSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]);
                            dsYDD.Tables[0].Rows[y]["多余量"] = 0;
                        }

                        //监控专用，写入中标定标信息表，并反写预订单
                        if (sp == "中标监控")
                        {


                            /*
                             * 特殊处理(TS)：由于预订单存在拆单，为保证订金冻结与历次解冻相符，若中标后，预订单仍然存在未成交量，则“写入中标定标信息表的订金金额”=预订单本次成交量*预订单拟买入价格*预订单中存储的订金比率。然后“反写预订单订金金额”=原预订单原订金金额-写入中标定标信息表的订金金额。同时还要将预订单的已成交量反写。
相对应的，这样当预订单中标后不再有未成交量了，才能直接用预订单中记录的订金金额直接写入《中标定标信息表》，这种情况不需要反写预订单订金金额了。
投标单的投标保证金不存在拆单，因此可以直接使用投标单中的值写入《中标定标信息表》，不需要反写
                             */

                            //确定预订单是否完全中标，不完全中标的，仍然要保留竞标状态
                            string ZTstr = "";
                            if (Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) + Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]) == Convert.ToInt64(dsYDD.Tables[0].Rows[y]["NDGSL"]))
                            {
                                ZTstr = "中标";
                            }
                            else
                            {
                                ZTstr = "竞标";
                            }


                            //生成一条插入中标定标信息表数据的语句
                            //..........................
                            string LMstr = ""; //前缀单号
                            if (HTQX == "三个月")
                            { LMstr = "L"; }
                            if (HTQX == "一年")
                            { LMstr = "M"; }
                            string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZBDBXXB", LMstr);
                            string Z_HTBH = "HT" + Number;  //临时用number生成*****
                            string Z_HTQX = HTQX;
                            string Z_HTZT = "中标";
                            string Z_QPZT = "未开始清盘";
                            string Z_SPBH = SPBH;
                            string Z_SPMC = dsTB.Tables[0].Rows[i]["SPMC"].ToString();
                            string Z_GG = dsTB.Tables[0].Rows[i]["GG"].ToString();
                            string Z_JJDW = dsTB.Tables[0].Rows[i]["JJDW"].ToString();
                            string Z_PTSDZDJJPL = dsTB.Tables[0].Rows[i]["PTSDZDJJPL"].ToString(); 
                            string Z_ZBSJ = ZBSJ;  //中标时间
                            Int64 Z_ZBSL = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]);
                            Int64 Z_YTHSL = 0;    //已提货数量，固定值
                            Int64 Z_RJZGFHL = 0;  //这个后面批量算******
                            double Z_ZBJG = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBJG"]); //中标价格
                            double Z_ZBJE = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) * Z_ZBJG; //中标金额
                            string Z_DBSJ = "null";   //定标时间
                            string Z_HTJSRQ = "null";  //合同结束日期
                            string Z_SFDJLYBZJ = "否";   //是否冻结履约保证金
                            double Z_LYBZJBL = Convert.ToDouble(htParameterInfo["卖家履约保证金比率"]);  //履约保证金比率，从动态参数设定表取值。
                            double Z_LYBZJJE = Math.Round(Z_ZBJE * Z_LYBZJBL, 2);  //履约保证金金额
                            if (Z_LYBZJJE < 0.01)
                            {
                                Z_LYBZJJE = 0.01;
                            }
                            //订金金额，这个地方需要特殊处理(TS)
                            double Z_DJJE = 0;
                            if (ZTstr == "竞标")
                            {
                                //“写入中标定标信息表的订金金额”=预订单本次成交量*预订单拟买入价格*预订单中存储的订金比率
                                Z_DJJE = Math.Round(Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) * Convert.ToDouble(dsYDD.Tables[0].Rows[y]["NMRJG"]) * Convert.ToDouble(dsYDD.Tables[0].Rows[y]["MJDJBL"]), 2);
                            }
                            if (ZTstr == "中标")
                            {
                                Z_DJJE = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["DJDJ"]);
                            }
                            double Z_BZHBL = Convert.ToDouble(htParameterInfo["保证函比率"]); //保证函比率
                            double Z_BZHJE = Math.Round(Z_ZBJE * Convert.ToDouble(htParameterInfo["保证函比率"]),2); //保证函金额


                            string T_YSTBDBH = dsTB.Tables[0].Rows[i]["Number"].ToString();
                            string T_YSTBDDLYX = dsTB.Tables[0].Rows[i]["DLYX"].ToString();
                            string T_YSTBDJSZHLX = dsTB.Tables[0].Rows[i]["JSZHLX"].ToString();
                            string T_YSTBDMJJSBH = dsTB.Tables[0].Rows[i]["MJJSBH"].ToString();
                            string T_YSTBDGLJJRYX = dsTB.Tables[0].Rows[i]["GLJJRYX"].ToString();
                            string T_YSTBDGLJJRYHM = dsTB.Tables[0].Rows[i]["GLJJRYHM"].ToString();
                            string T_YSTBDGLJJRJSBH = dsTB.Tables[0].Rows[i]["GLJJRJSBH"].ToString();
                            string YSTBDGLJJRPTGLJG = dsTB.Tables[0].Rows[i]["GLJJRPTGLJG"].ToString();
                            Int64 T_YSTBDMJSDJJPL = Convert.ToInt64(dsTB.Tables[0].Rows[i]["MJSDJJPL"]);
                            string T_YSTBDGHQY = dsTB.Tables[0].Rows[i]["GHQY"].ToString();
                            double T_YSTBDTBNSL = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBNSL"]);
                            double T_YSTBDTBJG = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBJG"]);
                            double T_YSTBDTBJE = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBJE"]);
                            double T_YSTBDMJTBBZJBL = Convert.ToDouble(dsTB.Tables[0].Rows[i]["MJTBBZJBL"]);
                            double T_YSTBDMJTBBZJZXZ = Convert.ToDouble(dsTB.Tables[0].Rows[i]["MJTBBZJZXZ"]);
                            double T_YSTBDDJTBBZJ = Convert.ToDouble(dsTB.Tables[0].Rows[i]["DJTBBZJ"]);
                            string YSTBDTJSJ = dsTB.Tables[0].Rows[i]["TJSJ"].ToString();


                            string Y_YSYDDBH = dsYDD.Tables[0].Rows[y]["Number"].ToString();
                            string Y_YSYDDDLYX = dsYDD.Tables[0].Rows[y]["DLYX"].ToString();
                            string Y_YSYDDJSZHLX = dsYDD.Tables[0].Rows[y]["JSZHLX"].ToString();
                            string Y_YSYDDMJJSBH = dsYDD.Tables[0].Rows[y]["MJJSBH"].ToString();
                            string Y_YSYDDGLJJRYX = dsYDD.Tables[0].Rows[y]["GLJJRYX"].ToString();
                            string Y_YSYDDGLJJRYHM = dsYDD.Tables[0].Rows[y]["GLJJRYHM"].ToString();
                            string Y_YSYDDGLJJRJSBH = dsYDD.Tables[0].Rows[y]["GLJJRJSBH"].ToString();
                            string YSYDDGLJJRPTGLJG = dsYDD.Tables[0].Rows[y]["GLJJRPTGLJG"].ToString();
                            string Y_YSYDDSHQY = dsYDD.Tables[0].Rows[y]["SHQY"].ToString();
                            string Y_YSYDDSHQYsheng = dsYDD.Tables[0].Rows[y]["SHQYsheng"].ToString();
                            string Y_YSYDDSHQYshi = dsYDD.Tables[0].Rows[y]["SHQYshi"].ToString();
                            double Y_YSYDDNMRJG = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["NMRJG"]);
                            double Y_YSYDDNDGSL = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["NDGSL"]);
                            double Y_YSYDDYZBSL = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["YZBSL"]);
                            double Y_YSYDDNDGJE = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["NDGJE"]);
                            double Y_YSYDDMJDJBL = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["MJDJBL"]);
                            double Y_YSYDDDJDJ = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["DJDJ"]);
                            string YSYDDTJSJ = dsYDD.Tables[0].Rows[y]["TJSJ"].ToString();


                            string Q_QPZMSCSJ = "null";
                            string Q_ZMSCFDLYX = "null";
                            string Q_ZMSCFJSZHLX = "null";
                            string Q_ZMSCFJSBH = "null";
                            string Q_ZMWJLJ = "null";
                            string Q_ZFLYZH = "null";
                            string Q_ZFMBZH = "null";
                            string Q_ZFJE = "null";
                            string Q_SFYQR = "null";
                            string Q_QRSJ = "null";
                            string NextChecker = "null";
                            int CheckState = 1;
                            string CreateUser = "admin";
                            string CreateTime = ZBSJ;
                            string CheckLimitTime = CreateTime;

                            //***********************
                            //参与本轮并中标的卖方数量(不用算，一直就是1) 
                            //参与本轮并中标的买方数量(累加，但要过滤重复的买方)
                            htsum_Z_Cynum_ZB_Buy[Y_YSYDDMJJSBH] = "x";
                            //参与本轮中标总金额(累加)
                            sum_Z_ZB_JE = sum_Z_ZB_JE + Z_ZBJE;
                            //参与本轮中标总数量(累加)
                            sum_Z_ZB_SL = sum_Z_ZB_SL + Z_ZBSL;
                            //**********************

                            alSQLtemp.Add(" INSERT INTO AAA_ZBDBXXB ( [Number],[Z_HTBH],[Z_HTQX],[Z_HTZT],[Z_QPZT],[Z_SPBH],[Z_SPMC],[Z_GG],[Z_JJDW],[Z_PTSDZDJJPL],[Z_ZBSJ],[Z_ZBSL],[Z_YTHSL],[Z_RJZGFHL],[Z_ZBJG],[Z_ZBJE],[Z_DBSJ],[Z_HTJSRQ],[Z_SFDJLYBZJ],[Z_LYBZJBL],[Z_LYBZJJE],[Z_DJJE],[Z_BZHBL],[Z_BZHJE],[T_YSTBDBH],[T_YSTBDDLYX],[T_YSTBDJSZHLX],[T_YSTBDMJJSBH],[T_YSTBDGLJJRYX],[T_YSTBDGLJJRYHM],[T_YSTBDGLJJRJSBH],[YSTBDGLJJRPTGLJG],[T_YSTBDMJSDJJPL],[T_YSTBDGHQY],[T_YSTBDTBNSL],[T_YSTBDTBJG],[T_YSTBDTBJE],[T_YSTBDMJTBBZJBL],[T_YSTBDMJTBBZJZXZ],[T_YSTBDDJTBBZJ],[YSTBDTJSJ],[Y_YSYDDBH],[Y_YSYDDDLYX],[Y_YSYDDJSZHLX],[Y_YSYDDMJJSBH],[Y_YSYDDGLJJRYX],[Y_YSYDDGLJJRYHM],[Y_YSYDDGLJJRJSBH],[YSYDDGLJJRPTGLJG],[Y_YSYDDSHQY],[Y_YSYDDSHQYsheng],[Y_YSYDDSHQYshi],[Y_YSYDDNMRJG],[Y_YSYDDNDGSL],[Y_YSYDDYZBSL],[Y_YSYDDNDGJE],[Y_YSYDDMJDJBL],[Y_YSYDDDJDJ],[YSYDDTJSJ],[Q_QPZMSCSJ],[Q_ZMSCFDLYX],[Q_ZMSCFJSZHLX],[Q_ZMSCFJSBH],[Q_ZMWJLJ],[Q_ZFLYZH],[Q_ZFMBZH],[Q_ZFJE],[Q_SFYQR],[Q_QRSJ],[NextChecker],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES('" + Number + "','" + Z_HTBH + "','" + Z_HTQX + "','" + Z_HTZT + "','" + Z_QPZT + "','" + Z_SPBH + "','" + Z_SPMC + "','" + Z_GG + "','" + Z_JJDW + "','" + Z_PTSDZDJJPL + "','" + Z_ZBSJ + "','" + Z_ZBSL + "','" + Z_YTHSL + "','" + Z_RJZGFHL + "','" + Z_ZBJG + "','" + Z_ZBJE + "'," + Z_DBSJ + "," + Z_HTJSRQ + ",'" + Z_SFDJLYBZJ + "','" + Z_LYBZJBL + "','" + Z_LYBZJJE + "','" + Z_DJJE + "','" + Z_BZHBL + "','" + Z_BZHJE + "','" + T_YSTBDBH + "','" + T_YSTBDDLYX + "','" + T_YSTBDJSZHLX + "','" + T_YSTBDMJJSBH + "','" + T_YSTBDGLJJRYX + "','" + T_YSTBDGLJJRYHM + "','" + T_YSTBDGLJJRJSBH + "','" + YSTBDGLJJRPTGLJG + "','" + T_YSTBDMJSDJJPL + "','" + T_YSTBDGHQY + "','" + T_YSTBDTBNSL + "','" + T_YSTBDTBJG + "','" + T_YSTBDTBJE + "','" + T_YSTBDMJTBBZJBL + "','" + T_YSTBDMJTBBZJZXZ + "','" + T_YSTBDDJTBBZJ + "','" + YSTBDTJSJ + "','" + Y_YSYDDBH + "','" + Y_YSYDDDLYX + "','" + Y_YSYDDJSZHLX + "','" + Y_YSYDDMJJSBH + "','" + Y_YSYDDGLJJRYX + "','" + Y_YSYDDGLJJRYHM + "','" + Y_YSYDDGLJJRJSBH + "','" + YSYDDGLJJRPTGLJG + "','" + Y_YSYDDSHQY + "','" + Y_YSYDDSHQYsheng + "','" + Y_YSYDDSHQYshi + "','" + Y_YSYDDNMRJG + "','" + Y_YSYDDNDGSL + "','" + Y_YSYDDYZBSL + "','" + Y_YSYDDNDGJE + "','" + Y_YSYDDMJDJBL + "','" + Y_YSYDDDJDJ + "','" + YSYDDTJSJ + "'," + Q_QPZMSCSJ + "," + Q_ZMSCFDLYX + "," + Q_ZMSCFJSZHLX + "," + Q_ZMSCFJSBH + "," + Q_ZMWJLJ + "," + Q_ZFLYZH + "," + Q_ZFMBZH + "," + Q_ZFJE + "," + Q_SFYQR + "," + Q_QRSJ + "," + NextChecker + ",'" + CheckState + "','" + CreateUser + "','" + CreateTime + "','" + CheckLimitTime + "')");
                        

                            //反写预订单//不一定都写中标，未完全成交的，要写竞标。(TS)
                            string djdj_sqlstr = " ";  //订金特殊处理的sql字符串
                            if (ZTstr == "竞标")
                            {
                                //重新计算拟订购金额
                                double new_NDGJE = Math.Round( (Convert.ToInt64(dsYDD.Tables[0].Rows[y]["NDGSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"])) * Convert.ToDouble(dsYDD.Tables[0].Rows[y]["NMRJG"]) , 2);
                                //“反写预订单订金金额”=原预订单原订金金额-写入中标定标信息表的订金金额
                                djdj_sqlstr = " ,DJDJ=DJDJ- " + Z_DJJE + " , NDGJE = " + new_NDGJE + " ";
                            }
                            if (ZTstr == "中标")
                            {
                                djdj_sqlstr = " ";
                            }

                            //处理是否拆单标记
                            string cd_sqlstr = "  "; //拆单更新处理sql
                            //若是此预订单第一次中标 并且 部分中标
                            if (Convert.ToInt64(dsYDD.Tables[0].Rows[y]["多余量"]) != 0 &&  Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]) == 0)
                            {
                                cd_sqlstr = " ,SFCD='是' ";
                            }

                            alSQLtemp.Add("update AAA_YDDXXB set ZT = '" + ZTstr + "',YZBSL=YZBSL+ " + Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) + djdj_sqlstr + cd_sqlstr + "  where Number = '" + dsYDD.Tables[0].Rows[y]["Number"].ToString() + "' ");



                        }
                        //若凑齐量超过了最大上限，强制退出这个for循环，只退出一层 
                        if (CQsl > max)
                        {
                            y = dsYDD.Tables[0].Rows.Count + 10;
                        }
                         
                    }
                    else//未凑到预售量，继续凑，但如果是中标监控，先把语句生成好。
                    {
                        //监控专用，反写预订单
                        if (sp == "中标监控")
                        {

                            //虽然没有凑齐，但是应该需要先生成sql语句
                            dsYDD.Tables[0].Rows[y]["本次成交"] = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["NDGSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]);
                            //生成一条插入中标定标信息表数据的语句
                            //..........................
                            string LMstr = ""; //前缀单号
                            if (HTQX == "三个月")
                            { LMstr = "L"; }
                            if (HTQX == "一年")
                            { LMstr = "M"; }
                            string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZBDBXXB", LMstr);
                            string Z_HTBH = "HT" + Number;  //临时用number生成*****
                            string Z_HTQX = HTQX;
                            string Z_HTZT = "中标";
                            string Z_QPZT = "未开始清盘";
                            string Z_SPBH = SPBH;
                            string Z_SPMC = dsTB.Tables[0].Rows[i]["SPMC"].ToString();
                            string Z_GG = dsTB.Tables[0].Rows[i]["GG"].ToString();
                            string Z_JJDW = dsTB.Tables[0].Rows[i]["JJDW"].ToString();
                            string Z_PTSDZDJJPL = dsTB.Tables[0].Rows[i]["PTSDZDJJPL"].ToString();

                            string Z_ZBSJ = ZBSJ;  //中标时间

                            Int64 Z_ZBSL = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]);
                            Int64 Z_YTHSL = 0;    //已提货数量，固定值
                            Int64 Z_RJZGFHL = 0;  //这个后面批量算******
                            double Z_ZBJG = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBJG"]); //中标价格
                            double Z_ZBJE = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) * Z_ZBJG; //中标金额
                            string Z_DBSJ = "null";   //定标时间
                            string Z_HTJSRQ = "null";  //合同结束日期
                            string Z_SFDJLYBZJ = "否";   //是否冻结履约保证金
                            double Z_LYBZJBL = Convert.ToDouble(htParameterInfo["卖家履约保证金比率"]);  //履约保证金比率，从动态参数设定表取值。
                            double Z_LYBZJJE = Math.Round(Z_ZBJE * Z_LYBZJBL, 2);  //履约保证金金额
                            if (Z_LYBZJJE < 0.01)
                            {
                                Z_LYBZJJE = 0.01;
                            }
                            //订金金额，这个地方需要特殊处理(TS) ,这里是未凑齐的，不需要特殊处理了。
                            double Z_DJJE = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["DJDJ"]);

                            double Z_BZHBL = Convert.ToDouble(htParameterInfo["保证函比率"]); //保证函比率
                            double Z_BZHJE = Math.Round(Z_ZBJE * Convert.ToDouble(htParameterInfo["保证函比率"]), 2); //保证函金额


                            string T_YSTBDBH = dsTB.Tables[0].Rows[i]["Number"].ToString();
                            string T_YSTBDDLYX = dsTB.Tables[0].Rows[i]["DLYX"].ToString();
                            string T_YSTBDJSZHLX = dsTB.Tables[0].Rows[i]["JSZHLX"].ToString();
                            string T_YSTBDMJJSBH = dsTB.Tables[0].Rows[i]["MJJSBH"].ToString();
                            string T_YSTBDGLJJRYX = dsTB.Tables[0].Rows[i]["GLJJRYX"].ToString();
                            string T_YSTBDGLJJRYHM = dsTB.Tables[0].Rows[i]["GLJJRYHM"].ToString();
                            string T_YSTBDGLJJRJSBH = dsTB.Tables[0].Rows[i]["GLJJRJSBH"].ToString();
                            string YSTBDGLJJRPTGLJG = dsTB.Tables[0].Rows[i]["GLJJRPTGLJG"].ToString();
                            Int64 T_YSTBDMJSDJJPL = Convert.ToInt64(dsTB.Tables[0].Rows[i]["MJSDJJPL"]);
                            string T_YSTBDGHQY = dsTB.Tables[0].Rows[i]["GHQY"].ToString();
                            double T_YSTBDTBNSL = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBNSL"]);
                            double T_YSTBDTBJG = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBJG"]);
                            double T_YSTBDTBJE = Convert.ToDouble(dsTB.Tables[0].Rows[i]["TBJE"]);
                            double T_YSTBDMJTBBZJBL = Convert.ToDouble(dsTB.Tables[0].Rows[i]["MJTBBZJBL"]);
                            double T_YSTBDMJTBBZJZXZ = Convert.ToDouble(dsTB.Tables[0].Rows[i]["MJTBBZJZXZ"]);
                            double T_YSTBDDJTBBZJ = Convert.ToDouble(dsTB.Tables[0].Rows[i]["DJTBBZJ"]);
                            string YSTBDTJSJ = dsTB.Tables[0].Rows[i]["TJSJ"].ToString();


                            string Y_YSYDDBH = dsYDD.Tables[0].Rows[y]["Number"].ToString();
                            string Y_YSYDDDLYX = dsYDD.Tables[0].Rows[y]["DLYX"].ToString();
                            string Y_YSYDDJSZHLX = dsYDD.Tables[0].Rows[y]["JSZHLX"].ToString();
                            string Y_YSYDDMJJSBH = dsYDD.Tables[0].Rows[y]["MJJSBH"].ToString();
                            string Y_YSYDDGLJJRYX = dsYDD.Tables[0].Rows[y]["GLJJRYX"].ToString();
                            string Y_YSYDDGLJJRYHM = dsYDD.Tables[0].Rows[y]["GLJJRYHM"].ToString();
                            string Y_YSYDDGLJJRJSBH = dsYDD.Tables[0].Rows[y]["GLJJRJSBH"].ToString();
                            string YSYDDGLJJRPTGLJG = dsYDD.Tables[0].Rows[y]["GLJJRPTGLJG"].ToString();
                            string Y_YSYDDSHQY = dsYDD.Tables[0].Rows[y]["SHQY"].ToString();
                            string Y_YSYDDSHQYsheng = dsYDD.Tables[0].Rows[y]["SHQYsheng"].ToString();
                            string Y_YSYDDSHQYshi = dsYDD.Tables[0].Rows[y]["SHQYshi"].ToString();
                            double Y_YSYDDNMRJG = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["NMRJG"]);
                            double Y_YSYDDNDGSL = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["NDGSL"]);
                            double Y_YSYDDYZBSL = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["YZBSL"]);
                            double Y_YSYDDNDGJE = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["NDGJE"]);
                            double Y_YSYDDMJDJBL = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["MJDJBL"]);
                            double Y_YSYDDDJDJ = Convert.ToDouble(dsYDD.Tables[0].Rows[y]["DJDJ"]);
                            string YSYDDTJSJ = dsYDD.Tables[0].Rows[y]["TJSJ"].ToString();


                            string Q_QPZMSCSJ = "null";
                            string Q_ZMSCFDLYX = "null";
                            string Q_ZMSCFJSZHLX = "null";
                            string Q_ZMSCFJSBH = "null";
                            string Q_ZMWJLJ = "null";
                            string Q_ZFLYZH = "null";
                            string Q_ZFMBZH = "null";
                            string Q_ZFJE = "null";
                            string Q_SFYQR = "null";
                            string Q_QRSJ = "null";
                            string NextChecker = "null";
                            int CheckState = 1;
                            string CreateUser = "admin";
                            string CreateTime = ZBSJ;
                            string CheckLimitTime = CreateTime;

                            //***********************
                            //参与本轮并中标的卖方数量(不用算，一直就是1) 
                            //参与本轮并中标的买方数量(累加，但要过滤重复的买方)
                            htsum_Z_Cynum_ZB_Buy[Y_YSYDDMJJSBH] = "x";
                            //参与本轮中标总金额(累加)
                            sum_Z_ZB_JE = sum_Z_ZB_JE + Z_ZBJE;
                            //参与本轮中标总数量(累加)
                            sum_Z_ZB_SL = sum_Z_ZB_SL + Z_ZBSL;
                            //**********************

                            alSQLtemp.Add(" INSERT INTO AAA_ZBDBXXB ( [Number],[Z_HTBH],[Z_HTQX],[Z_HTZT],[Z_QPZT],[Z_SPBH],[Z_SPMC],[Z_GG],[Z_JJDW],[Z_PTSDZDJJPL],[Z_ZBSJ],[Z_ZBSL],[Z_YTHSL],[Z_RJZGFHL],[Z_ZBJG],[Z_ZBJE],[Z_DBSJ],[Z_HTJSRQ],[Z_SFDJLYBZJ],[Z_LYBZJBL],[Z_LYBZJJE],[Z_DJJE],[Z_BZHBL],[Z_BZHJE],[T_YSTBDBH],[T_YSTBDDLYX],[T_YSTBDJSZHLX],[T_YSTBDMJJSBH],[T_YSTBDGLJJRYX],[T_YSTBDGLJJRYHM],[T_YSTBDGLJJRJSBH],[YSTBDGLJJRPTGLJG],[T_YSTBDMJSDJJPL],[T_YSTBDGHQY],[T_YSTBDTBNSL],[T_YSTBDTBJG],[T_YSTBDTBJE],[T_YSTBDMJTBBZJBL],[T_YSTBDMJTBBZJZXZ],[T_YSTBDDJTBBZJ],[YSTBDTJSJ],[Y_YSYDDBH],[Y_YSYDDDLYX],[Y_YSYDDJSZHLX],[Y_YSYDDMJJSBH],[Y_YSYDDGLJJRYX],[Y_YSYDDGLJJRYHM],[Y_YSYDDGLJJRJSBH],[YSYDDGLJJRPTGLJG],[Y_YSYDDSHQY],[Y_YSYDDSHQYsheng],[Y_YSYDDSHQYshi],[Y_YSYDDNMRJG],[Y_YSYDDNDGSL],[Y_YSYDDYZBSL],[Y_YSYDDNDGJE],[Y_YSYDDMJDJBL],[Y_YSYDDDJDJ],[YSYDDTJSJ],[Q_QPZMSCSJ],[Q_ZMSCFDLYX],[Q_ZMSCFJSZHLX],[Q_ZMSCFJSBH],[Q_ZMWJLJ],[Q_ZFLYZH],[Q_ZFMBZH],[Q_ZFJE],[Q_SFYQR],[Q_QRSJ],[NextChecker],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES('" + Number + "','" + Z_HTBH + "','" + Z_HTQX + "','" + Z_HTZT + "','" + Z_QPZT + "','" + Z_SPBH + "','" + Z_SPMC + "','" + Z_GG + "','" + Z_JJDW + "','" + Z_PTSDZDJJPL + "','" + Z_ZBSJ + "','" + Z_ZBSL + "','" + Z_YTHSL + "','" + Z_RJZGFHL + "','" + Z_ZBJG + "','" + Z_ZBJE + "'," + Z_DBSJ + "," + Z_HTJSRQ + ",'" + Z_SFDJLYBZJ + "','" + Z_LYBZJBL + "','" + Z_LYBZJJE + "','" + Z_DJJE + "','" + Z_BZHBL + "','" + Z_BZHJE + "','" + T_YSTBDBH + "','" + T_YSTBDDLYX + "','" + T_YSTBDJSZHLX + "','" + T_YSTBDMJJSBH + "','" + T_YSTBDGLJJRYX + "','" + T_YSTBDGLJJRYHM + "','" + T_YSTBDGLJJRJSBH + "','" + YSTBDGLJJRPTGLJG + "','" + T_YSTBDMJSDJJPL + "','" + T_YSTBDGHQY + "','" + T_YSTBDTBNSL + "','" + T_YSTBDTBJG + "','" + T_YSTBDTBJE + "','" + T_YSTBDMJTBBZJBL + "','" + T_YSTBDMJTBBZJZXZ + "','" + T_YSTBDDJTBBZJ + "','" + YSTBDTJSJ + "','" + Y_YSYDDBH + "','" + Y_YSYDDDLYX + "','" + Y_YSYDDJSZHLX + "','" + Y_YSYDDMJJSBH + "','" + Y_YSYDDGLJJRYX + "','" + Y_YSYDDGLJJRYHM + "','" + Y_YSYDDGLJJRJSBH + "','" + YSYDDGLJJRPTGLJG + "','" + Y_YSYDDSHQY + "','" + Y_YSYDDSHQYsheng + "','" + Y_YSYDDSHQYshi + "','" + Y_YSYDDNMRJG + "','" + Y_YSYDDNDGSL + "','" + Y_YSYDDYZBSL + "','" + Y_YSYDDNDGJE + "','" + Y_YSYDDMJDJBL + "','" + Y_YSYDDDJDJ + "','" + YSYDDTJSJ + "'," + Q_QPZMSCSJ + "," + Q_ZMSCFDLYX + "," + Q_ZMSCFJSZHLX + "," + Q_ZMSCFJSBH + "," + Q_ZMWJLJ + "," + Q_ZFLYZH + "," + Q_ZFMBZH + "," + Q_ZFJE + "," + Q_SFYQR + "," + Q_QRSJ + "," + NextChecker + ",'" + CheckState + "','" + CreateUser + "','" + CreateTime + "','" + CheckLimitTime + "')");
                         

                            //反写预订单
                            //不一定都写中标，未完全成交的，要写竞标。(TS) 这里不需要特殊处理了。

                            //处理是否拆单标记
                            string cd_sqlstr = "  "; //拆单更新处理sql
                            //若是此预订单第一次中标 并且 部分中标
                            if (Convert.ToInt64(dsYDD.Tables[0].Rows[y]["多余量"]) != 0 && Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]) == 0)
                            {
                                cd_sqlstr = " ,SFCD='是' ";
                            }

                            alSQLtemp.Add("update AAA_YDDXXB set ZT = '中标',YZBSL=YZBSL+ " + Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) + cd_sqlstr + "  where Number = '" + dsYDD.Tables[0].Rows[y]["Number"].ToString() + "' ");



                        }
                    }
                    
                }

                //凑够了
                if (dsTB.Tables[0].Rows[i]["是否凑齐"].ToString() == "是")
                {
                    //监控专用，反写投标信息
                    if (sp == "中标监控")
                    {
                        //把临时语句增加进去
                        ALsql.AddRange(alSQLtemp);
                        //反写投标信息
                        ALsql.Add("update AAA_TBD set ZT = '中标'  where Number = '" + dsTB.Tables[0].Rows[i]["Number"].ToString() + "' ");

                        //找参与竞标的买家和卖家数量，更新到中标定标信息表
                        DataSet ds_jyfsl = DbHelperSQL.Query("select (select count(distinct MJJSBH) from AAA_TBD where ZT = '竞标' and SPBH ='" + SPBH + "' and HTQX='" + HTQX + "') as 卖方数量 , (select count(distinct MJJSBH) from AAA_YDDXXB where ZT = '竞标' and SPBH ='" + SPBH + "' and HTQX='" + HTQX + "') as 买方数量");
                        string Z_LC_number = Guid.NewGuid().ToString(); //轮次识别号




                        //重新处理定标中标信息表中的日均最高发货量。 定标的集合订购量/(90或365)*120%
                        Int64 RE_rjzgfhl = 0;
                        Int64 tian = 0; 
                        if (HTQX == "三个月")
                        { tian = 90; }
                        if (HTQX == "一年")
                        { tian = 365; }
                        RE_rjzgfhl = Convert.ToInt64(Math.Round((Convert.ToDouble(dsTB.Tables[0].Rows[i]["成交量"]) / tian) * 1.2, 0)) + 1;
                        ALsql.Add("update AAA_ZBDBXXB set Z_RJZGFHL = " + RE_rjzgfhl + ",Z_Cynum_Sel = " + ds_jyfsl.Tables[0].Rows[0]["卖方数量"].ToString() + " , Z_Cynum_Buy = " + ds_jyfsl.Tables[0].Rows[0]["买方数量"].ToString() + ", Z_LC_number = '" + Z_LC_number + "', Z_Cynum_ZB_Sel=" + sum_Z_Cynum_ZB_Sel + ", Z_Cynum_ZB_Buy=" + htsum_Z_Cynum_ZB_Buy.Count.ToString() + ",Z_ZB_JE=" + sum_Z_ZB_JE.ToString() + ",Z_ZB_SL=" + sum_Z_ZB_SL.ToString() + "  where T_YSTBDBH = '" + dsTB.Tables[0].Rows[i]["Number"].ToString() + "' ");



                        ALsql[0] = ALsql[0].ToString() + "  select '有中标数据产生'   ";

                    }
                }
            }


            //无论找没找到能匹配的预订单，都进行一下冷静期的处理
            //看这个投标信息的商品是否进入了冷静期，对冷静期控制进行处理
            if (SFJRLJQ == "是") //在冷静期
            {
                //处理中标监控
                if (sp == "中标监控")
                {
                    //更新冷静期状态为否，时间置空
                    ALsql.Add(" update AAA_LJQDQZTXXB set SFJRLJQ = '否',LJQKSSJ = null,LJQJSSJ=null where  HTQX = '" + HTQX + "' and SPBH  = '" + SPBH + "' ");
                    //记录历史记录
                    string newnumber = jhjx_PublicClass.GetNextNumberZZ("AAA_LJQBGLSJLB", "");
                    ALsql.Add(" INSERT INTO AAA_LJQBGLSJLB (Number,SPBH,HTQX,LJQKSSJBGW,LJQJSSJBGW,SFZLJQZTBGW) VALUES ('" + newnumber + "','" + SPBH + "','" + HTQX + "',null,null,'否') ");

                    ALsql[0] = ALsql[0].ToString() + "  select '有商品离开冷静期'   ";

                }
                else  //不是中标监控，不动冷静期
                {
                    ;
                }
            }
            else //不在冷静期
            {
                if (dsTB.Tables[0].Rows[i]["是否凑齐"].ToString() == "是") //可以凑齐
                {
                    //更新冷静期状态为是，时间更新为当前时间。 冷静期进入时间，需要与中标时间一致。
                    string t1 = ZBSJ;
                    DateTime time1begin = DateTime.Parse(t1);

                    DateTime time2begin = time1begin.AddDays(1);
                    
                    //计算冷静期结束时间
                    DataSet dsJQ = DbHelperSQL.Query(" select * from AAA_PTJRSDB where SFYX = '是' order by RQ asc ");
                    if (dsJQ != null && dsJQ.Tables[0].Rows.Count > 0)
                    {
                        for (int p = 0; p < dsJQ.Tables[0].Rows.Count; p++)
                        {
                            //若日期存在，则说明不能出冷静期
                            DateTime thisrowtime = DateTime.Parse(dsJQ.Tables[0].Rows[p]["RQ"].ToString());
                            //若日期在数据库中存在，则累加一天
                            if (time2begin.Date.CompareTo(thisrowtime.Date) ==0) 
                            {
                                time2begin = time2begin.AddDays(1);
                            }
                        }
                    }
                    string t2 = time2begin.ToString();

                    ALsql.Add(" update AAA_LJQDQZTXXB set SFJRLJQ = '是',LJQKSSJ = '" + t1 + "',LJQJSSJ = '" + t2 + "' where    HTQX = '" + HTQX + "' and SPBH  = '" + SPBH + "' ");
                    //记录历史记录
                    string newnumber = jhjx_PublicClass.GetNextNumberZZ("AAA_LJQBGLSJLB", "");
                    ALsql.Add(" INSERT INTO AAA_LJQBGLSJLB (Number,SPBH,HTQX,LJQKSSJBGW,LJQJSSJBGW,SFZLJQZTBGW) VALUES ('" + newnumber + "','" + SPBH + "','" + HTQX + "','" + t1 + "','" + t2 + "','是') ");

                    //写入投标资料审核表   2013-08-22 添加
                  
                    if (Convert.ToInt32(DbHelperSQL.GetSingle("select COUNT(*) from AAA_TBZLSHB where  TBDH='" + dsTB.Tables[0].Rows[i]["Number"].ToString() + "'"))==0)//不存在这条投标单单号数据
                    {
                        string newnumberTBZLSH = jhjx_PublicClass.GetNextNumberZZ("AAA_TBZLSHB", "");
                        ALsql.Add("  insert AAA_TBZLSHB(Number,TBDH,FWZXSHZT,JYGLBSHZT,FWZXSHYCHSFXG,JYGLBSHWTGHSFXG,CreateTime,CheckLimitTime) values('" + newnumberTBZLSH + "','" + dsTB.Tables[0].Rows[i]["Number"].ToString() + "','未审核','初始值','初始值','初始值','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "') ");
                    }
                 

                    ALsql[0] = ALsql[0].ToString() + "  select '有商品进入冷静期'   ";

                }
                else //不能凑齐
                {
                    //冷静期状态不动，时间不动
                }
            }



            //给这个商品解锁，允许再发布投标信息和预订单。(先临时写到这里，其实不能写到这里，应该写到中标监控中的语句真正执行后。但发布投标和预订单后执行本方法返回语句时，不需要执行这个解锁)
            if (sp == "中标监控")
            {
                DbHelperSQL.ExecuteSql("update AAA_LJQDQZTXXB set LJQYZBS = '未锁' where SPBH='" + SPBH + "' and HTQX='" + HTQX + "'");
            }

        }




        return ALsql;
    }



    /// <summary>
    /// 即时交易监控
    /// </summary>
    /// <returns>返回要执行的SQL语句组</returns>
    public ArrayList RunCJGZ_JSJY()
    {
        //声明要返回的SQL数组
        ArrayList al = new ArrayList();//要返回的SQL
        al.Add("  ");
        int Num_zbspsl = 0; //中标商品数量
        int Num_runspsl = 0; //参与计算的商品总数量
        Hashtable htParameterInfo = PublicClass2013.GetParameterInfo();//平台公用参数
        //-----------------------------------------------
        /*
         逻辑说明
 
         1、遍历商品编号，找出每种商品在投标单中价格最低的那一条。
         2、遍历找出后的投标单信息，匹配预订单中价格大于等于此投标单的、时间最早的一条数据，进行匹配。
         3、(投标单拆单、预订单拆单)成交。
         */
        //-----------------------------------------------
 
 
            
            //-----------------------------------------------
            #region 2、遍历商品编号，找出每种商品在投标单中价格最低的那一条。
            string sql_TBDs = "";
            sql_TBDs = sql_TBDs +  "    update AAA_DaPanTJTime set LastJSZBtime_YC = getdate()  where LastYXBGtime>=LastJSZBtime  ";
            sql_TBDs = " select * from ( select row_number() over(partition by spbh order by tbjg,createtime) as 序号, * from AAA_TBD where ZT='竞标' and HTQX='即时' ) as tab left join AAA_DaPanTJTime as T on tab.SPBH = T.SPBH  where tab.序号=1 and  T.LastYXBGtime >= T.LastJSZBtime order by  tab.SPBH desc   ";
            DataTable dt_TBDs = DbHelperSQL.Query(sql_TBDs).Tables[0];
            #endregion

            //dt_TBDs

            #region 3、遍历找出后的投标单信息，匹配预订单中价格大于等于此投标单的、时间最早的一条数据，进行匹配。
            for (int i = 0; i < dt_TBDs.Rows.Count;i++ )
            {
                Num_runspsl++;
                string TBD_Number = dt_TBDs.Rows[i]["Number"].ToString();//投标单号
                Int64 TBSL = Convert.ToInt64(dt_TBDs.Rows[i]["TBNSL"]) - Convert.ToInt64(dt_TBDs.Rows[i]["YZBSL"]);//投标数量
                double TBJG = Convert.ToDouble(dt_TBDs.Rows[i]["TBJG"]);//投标价格
                string SPBH = dt_TBDs.Rows[i]["SPBH"].ToString();//商品编号
                string GHQY = dt_TBDs.Rows[i]["GHQY"].ToString();//供货区域
                string sql_YDD = "SELECT TOP 1 * FROM AAA_YDDXXB WHERE ZT='竞标' AND HTQX='即时' AND SPBH='" + SPBH + "' AND NMRJG >= " + TBJG + " AND CHARINDEX('|'+SHQYsheng+'|','" + GHQY + "') > 0 ORDER BY CreateTime ASC";
                DataTable dt_YDD = DbHelperSQL.Query(sql_YDD).Tables[0];//预订单信息
                if (dt_YDD.Rows.Count == 1)
                {
                    //参与本轮并中标的卖方数量(不用算，一直就是1) 
                    Int64 sum_Z_Cynum_ZB_Sel = 1;
                    //参与本轮并中标的买方数量(不用算，一直就是1) 
                    Int64 sum_Z_Cynum_ZB_Buy = 1;
                    //参与本轮中标总金额(累加)
                    double sum_Z_ZB_JE = 0;
                    //参与本轮中标总数量(累加)
                    Int64 sum_Z_ZB_SL = 0;


                    Num_zbspsl++;
                    //-----------------------------------------------
                    #region 4、(投标单拆单、预订单拆单)成交。
                    string YDD_Number = dt_YDD.Rows[0]["Number"].ToString();//预订单号
                    Int64 YDSL = Convert.ToInt64(dt_YDD.Rows[0]["NDGSL"]) - Convert.ToInt64(dt_YDD.Rows[0]["YZBSL"]);//预定数量

                    string ZBDB_Number = ""; //中标定标信息化新编号

                    #region 预定数量>投标数量时，中标数量=投标数量。投标单被标记为中标，预订单要反写
                    if (YDSL > TBSL)
                    {
                        string TimeNow = DateTime.Now.ToString();//本次业务产生的时间都用这个
                        //更新投标单信息表
                        string UPDATE_TBD = "UPDATE AAA_TBD SET ZT='中标', YZBSL=(CASE (SELECT ZT FROM AAA_TBD WHERE Number='" + TBD_Number + "') WHEN '竞标' THEN TBNSL WHEN '撤销' THEN 'aaa' WHEN '中标' THEN 'aaa' END) WHERE Number='" + TBD_Number + "'";//如果单子的状态不是竞标，这句SQL会出错，从而使得事务回滚。
                        al.Add(UPDATE_TBD);

                        //插入中标信息表
                        ZBDB_Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZBDBXXB", "N");//中标定标主键
                        string Z_HTBH = "HT" + ZBDB_Number; //合同编号
                        string Z_HTQX = "即时";
                        string Z_HTZT = "中标";
                        string Z_QPZT = "未开始清盘";
                        string Z_SPBH = SPBH;
                        string Z_SPMC = dt_TBDs.Rows[i]["SPMC"].ToString();//商品名称
                        string Z_GG = dt_TBDs.Rows[i]["GG"].ToString();
                        string Z_JJDW = dt_TBDs.Rows[i]["JJDW"].ToString();
                        string Z_PTSDZDJJPL = dt_TBDs.Rows[i]["PTSDZDJJPL"].ToString(); //平台设定最大经济批量
                        string Z_ZBSJ = TimeNow;  //中标时间
                        Int64 Z_ZBSL = TBSL;//投标||预定数量
                        int Z_YTHSL = 0;//已提货数量
                        Int64 Z_RJZGFHL = Z_ZBSL;//日均最高发货量，即时的写0
                        double Z_ZBJG = Convert.ToDouble(dt_TBDs.Rows[i]["TBJG"]);//中标价格已投标价为准
                        double Z_ZBJE = Convert.ToInt64(Z_ZBSL * Z_ZBJG);//中标金额
                        string Z_DBSJ = "null";   //定标时间
                        string Z_HTJSRQ = "null";  //合同结束日期
                        string Z_SFDJLYBZJ = "否";   //是否冻结履约保证金
                        double Z_LYBZJBL = Convert.ToDouble(htParameterInfo["卖家履约保证金比率"]);  //履约保证金比率，从动态参数设定表取值。
                        double Z_LYBZJJE = Math.Round(Z_ZBJE * Z_LYBZJBL, 2) < 0.01 ? 0.01 : Math.Round(Z_ZBJE * Z_LYBZJBL, 2);  //履约保证金金额
                        double Z_DJJE = Math.Round(TBSL * Convert.ToDouble(dt_YDD.Rows[0]["NMRJG"]) * Convert.ToDouble(dt_YDD.Rows[0]["MJDJBL"]), 2);//冻结订金
                        double Z_BZHBL = Convert.ToDouble(htParameterInfo["保证函比率"]); //保证函比率
                        double Z_BZHJE = Math.Round(Z_ZBJE * Convert.ToDouble(htParameterInfo["保证函比率"]), 2); //保证函金额

                        string T_YSTBDBH = dt_TBDs.Rows[i]["Number"].ToString();
                        string T_YSTBDDLYX = dt_TBDs.Rows[i]["DLYX"].ToString();
                        string T_YSTBDJSZHLX = dt_TBDs.Rows[i]["JSZHLX"].ToString();
                        string T_YSTBDMJJSBH = dt_TBDs.Rows[i]["MJJSBH"].ToString();
                        string T_YSTBDGLJJRYX = dt_TBDs.Rows[i]["GLJJRYX"].ToString();
                        string T_YSTBDGLJJRYHM = dt_TBDs.Rows[i]["GLJJRYHM"].ToString();
                        string T_YSTBDGLJJRJSBH = dt_TBDs.Rows[i]["GLJJRJSBH"].ToString();
                        string YSTBDGLJJRPTGLJG = dt_TBDs.Rows[i]["GLJJRPTGLJG"].ToString();
                        Int64 T_YSTBDMJSDJJPL = Convert.ToInt64(dt_TBDs.Rows[i]["MJSDJJPL"]);
                        string T_YSTBDGHQY = dt_TBDs.Rows[i]["GHQY"].ToString();
                        double T_YSTBDTBNSL = Convert.ToDouble(dt_TBDs.Rows[i]["TBNSL"]);
                        double T_YSTBDTBJG = Convert.ToDouble(dt_TBDs.Rows[i]["TBJG"]);
                        double T_YSTBDTBJE = Convert.ToDouble(dt_TBDs.Rows[i]["TBJE"]);
                        double T_YSTBDMJTBBZJBL = Convert.ToDouble(dt_TBDs.Rows[i]["MJTBBZJBL"]);
                        double T_YSTBDMJTBBZJZXZ = Convert.ToDouble(dt_TBDs.Rows[i]["MJTBBZJZXZ"]);
                        double T_YSTBDDJTBBZJ = Convert.ToDouble(dt_TBDs.Rows[i]["DJTBBZJ"]);//这个分支中，是直接取余量的所有值
                        string YSTBDTJSJ = dt_TBDs.Rows[i]["TJSJ"].ToString();


                        string Y_YSYDDBH = dt_YDD.Rows[0]["Number"].ToString();
                        string Y_YSYDDDLYX = dt_YDD.Rows[0]["DLYX"].ToString();
                        string Y_YSYDDJSZHLX = dt_YDD.Rows[0]["JSZHLX"].ToString();
                        string Y_YSYDDMJJSBH = dt_YDD.Rows[0]["MJJSBH"].ToString();
                        string Y_YSYDDGLJJRYX = dt_YDD.Rows[0]["GLJJRYX"].ToString();
                        string Y_YSYDDGLJJRYHM = dt_YDD.Rows[0]["GLJJRYHM"].ToString();
                        string Y_YSYDDGLJJRJSBH = dt_YDD.Rows[0]["GLJJRJSBH"].ToString();
                        string YSYDDGLJJRPTGLJG = dt_YDD.Rows[0]["GLJJRPTGLJG"].ToString();
                        string Y_YSYDDSHQY = dt_YDD.Rows[0]["SHQY"].ToString();
                        string Y_YSYDDSHQYsheng = dt_YDD.Rows[0]["SHQYsheng"].ToString();
                        string Y_YSYDDSHQYshi = dt_YDD.Rows[0]["SHQYshi"].ToString();
                        double Y_YSYDDNMRJG = Convert.ToDouble(dt_YDD.Rows[0]["NMRJG"]);
                        double Y_YSYDDNDGSL = Convert.ToDouble(dt_YDD.Rows[0]["NDGSL"]);
                        double Y_YSYDDYZBSL = Convert.ToDouble(dt_YDD.Rows[0]["YZBSL"]);
                        double Y_YSYDDNDGJE = Convert.ToDouble(dt_YDD.Rows[0]["NDGJE"]);
                        double Y_YSYDDMJDJBL = Convert.ToDouble(dt_YDD.Rows[0]["MJDJBL"]);
                        double Y_YSYDDDJDJ = Convert.ToDouble(dt_YDD.Rows[0]["DJDJ"]);
                        string YSYDDTJSJ = dt_YDD.Rows[0]["TJSJ"].ToString();


                        string Q_QPZMSCSJ = "null";
                        string Q_ZMSCFDLYX = "null";
                        string Q_ZMSCFJSZHLX = "null";
                        string Q_ZMSCFJSBH = "null";
                        string Q_ZMWJLJ = "null";
                        string Q_ZFLYZH = "null";
                        string Q_ZFMBZH = "null";
                        string Q_ZFJE = "null";
                        string Q_SFYQR = "null";
                        string Q_QRSJ = "null";
                        string CreateUser = "admin";
                        string CreateTime = TimeNow;


                        //***********************
                        //参与本轮并中标的卖方数量(不用算，一直就是1) 
                        //参与本轮并中标的买方数量(不用算，一直就是1)
                        //参与本轮中标总金额(累加)
                        sum_Z_ZB_JE = sum_Z_ZB_JE + Z_ZBJE;
                        //参与本轮中标总数量(累加)
                        sum_Z_ZB_SL = sum_Z_ZB_SL + Z_ZBSL;
                        //**********************


                        string INSERT_ZBDBXXB = "INSERT INTO AAA_ZBDBXXB(Number, Z_HTBH, Z_HTQX, Z_HTZT, Z_QPZT, Z_SPBH, Z_SPMC, Z_GG, Z_JJDW, Z_PTSDZDJJPL, Z_ZBSJ, Z_ZBSL, Z_YTHSL, Z_RJZGFHL, Z_ZBJG, Z_ZBJE, Z_DBSJ, Z_HTJSRQ, Z_SFDJLYBZJ, Z_LYBZJBL, Z_LYBZJJE, Z_DJJE, Z_BZHBL, Z_BZHJE, T_YSTBDBH, T_YSTBDDLYX, T_YSTBDJSZHLX, T_YSTBDMJJSBH, T_YSTBDGLJJRYX, T_YSTBDGLJJRYHM, T_YSTBDGLJJRJSBH, YSTBDGLJJRPTGLJG, T_YSTBDMJSDJJPL, T_YSTBDGHQY, T_YSTBDTBNSL, T_YSTBDTBJG, T_YSTBDTBJE, T_YSTBDMJTBBZJBL, T_YSTBDMJTBBZJZXZ, T_YSTBDDJTBBZJ, YSTBDTJSJ, Y_YSYDDBH, Y_YSYDDDLYX, Y_YSYDDJSZHLX, Y_YSYDDMJJSBH, Y_YSYDDGLJJRYX, Y_YSYDDGLJJRYHM, Y_YSYDDGLJJRJSBH, YSYDDGLJJRPTGLJG, Y_YSYDDSHQY, Y_YSYDDSHQYsheng, Y_YSYDDSHQYshi, Y_YSYDDNMRJG, Y_YSYDDNDGSL, Y_YSYDDYZBSL, Y_YSYDDNDGJE, Y_YSYDDMJDJBL, Y_YSYDDDJDJ, YSYDDTJSJ, Q_QPZMSCSJ, Q_ZMSCFDLYX, Q_ZMSCFJSZHLX, Q_ZMSCFJSBH, Q_ZMWJLJ, Q_ZFLYZH, Q_ZFMBZH, Q_ZFJE, Q_SFYQR, Q_QRSJ, CreateUser, CreateTime) VALUES('" + ZBDB_Number + "','" + Z_HTBH + "','" + Z_HTQX + "','" + Z_HTZT + "','" + Z_QPZT + "','" + Z_SPBH + "','" + Z_SPMC + "','" + Z_GG + "','" + Z_JJDW + "','" + Z_PTSDZDJJPL + "','" + Z_ZBSJ + "','" + Z_ZBSL + "'," + Z_YTHSL + ",'" + Z_RJZGFHL + "','" + Z_ZBJG + "','" + Z_ZBJE + "'," + Z_DBSJ + "," + Z_HTJSRQ + ",'" + Z_SFDJLYBZJ + "','" + Z_LYBZJBL + "','" + Z_LYBZJJE + "','" + Z_DJJE + "','" + Z_BZHBL + "','" + Z_BZHJE + "','" + T_YSTBDBH + "','" + T_YSTBDDLYX + "','" + T_YSTBDJSZHLX + "','" + T_YSTBDMJJSBH + "','" + T_YSTBDGLJJRYX + "','" + T_YSTBDGLJJRYHM + "','" + T_YSTBDGLJJRJSBH + "','" + YSTBDGLJJRPTGLJG + "','" + T_YSTBDMJSDJJPL + "','" + T_YSTBDGHQY + "','" + T_YSTBDTBNSL + "','" + T_YSTBDTBJG + "','" + T_YSTBDTBJE + "','" + T_YSTBDMJTBBZJBL + "','" + T_YSTBDMJTBBZJZXZ + "','" + T_YSTBDDJTBBZJ + "','" + YSTBDTJSJ + "','" + Y_YSYDDBH + "','" + Y_YSYDDDLYX + "','" + Y_YSYDDJSZHLX + "','" + Y_YSYDDMJJSBH + "','" + Y_YSYDDGLJJRYX + "','" + Y_YSYDDGLJJRYHM + "','" + Y_YSYDDGLJJRJSBH + "','" + YSYDDGLJJRPTGLJG + "','" + Y_YSYDDSHQY + "','" + Y_YSYDDSHQYsheng + "','" + Y_YSYDDSHQYshi + "','" + Y_YSYDDNMRJG + "','" + Y_YSYDDNDGSL + "','" + Y_YSYDDYZBSL + "','" + Y_YSYDDNDGJE + "','" + Y_YSYDDMJDJBL + "','" + Y_YSYDDDJDJ + "','" + YSYDDTJSJ + "'," + Q_QPZMSCSJ + "," + Q_ZMSCFDLYX + "," + Q_ZMSCFJSZHLX + "," + Q_ZMSCFJSBH + "," + Q_ZMWJLJ + "," + Q_ZFLYZH + "," + Q_ZFMBZH + "," + Q_ZFJE + "," + Q_SFYQR + "," + Q_QRSJ + ",'" + CreateUser + "','" + CreateTime + "')";
                        al.Add(INSERT_ZBDBXXB);
                        //更新预订单信息表

                        double new_NDGJE = Math.Round((Convert.ToInt64(dt_YDD.Rows[0]["NDGSL"])-Convert.ToInt64(dt_YDD.Rows[0]["YZBSL"])-TBSL)*Convert.ToDouble(dt_YDD.Rows[0]["NMRJG"]),2);

                        //这种情况，预订单被拆单了
                        string cd_sqlstr = "  "; //拆单更新处理sql
                        //若是此预订单第一次中标 并且 部分中标
                        if (Convert.ToInt64(dt_YDD.Rows[0]["YZBSL"]) == 0)
                        {
                            cd_sqlstr = " ,SFCD='是' ";
                        }

                        string UPDATE_YDD = "UPDATE AAA_YDDXXB SET YZBSL=(CASE (SELECT ZT FROM AAA_YDDXXB WHERE Number='" + YDD_Number + "') WHEN '竞标' THEN (YZBSL+" + TBSL + ") WHEN '撤销' THEN 'aaa' WHEN '中标' THEN 'aaa' END),DJDJ=DJDJ-" + Z_DJJE + ",NDGJE=" + new_NDGJE + cd_sqlstr + " WHERE Number='" + YDD_Number + "'";//如果预订单状态不是竞标，这句SQL会出错，从而使得事务回滚。反写：已中标数量、冻结投标保证金、投标金额
                        al.Add(UPDATE_YDD);
                    }
                    #endregion

                    #region 预定数量<投标数量时，中标数量=预定数量。预订单为中标，投标单仍然为竞标
                    if (YDSL < TBSL)
                    {
                        string TimeNow = DateTime.Now.ToString();//本次业务产生的时间都用这个
                        //更新预订单信息表
                        string UPDATE_YDD = "UPDATE AAA_YDDXXB SET ZT='中标', YZBSL=(CASE (SELECT ZT FROM AAA_YDDXXB WHERE Number='" + YDD_Number + "') WHEN '竞标' THEN NDGSL WHEN '撤销' THEN 'aaa' WHEN '中标' THEN 'aaa' END) WHERE Number='" + YDD_Number + "'";//如果预订单状态不是竞标，这句SQL会出错，从而使得事务回滚。
                        al.Add(UPDATE_YDD);
                        #region 插入中标定标信息表
                        //插入中标信息表
                        ZBDB_Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZBDBXXB", "N");//中标定标主键
                        string Z_HTBH = "HT" + ZBDB_Number; //合同编号
                        string Z_HTQX = "即时";
                        string Z_HTZT = "中标";
                        string Z_QPZT = "未开始清盘";
                        string Z_SPBH = SPBH;
                        string Z_SPMC = dt_TBDs.Rows[i]["SPMC"].ToString();//商品名称
                        string Z_GG = dt_TBDs.Rows[i]["GG"].ToString();
                        string Z_JJDW = dt_TBDs.Rows[i]["JJDW"].ToString();
                        string Z_PTSDZDJJPL = dt_TBDs.Rows[i]["PTSDZDJJPL"].ToString(); //平台设定最大经济批量
                        string Z_ZBSJ = TimeNow;  //中标时间
                        Int64 Z_ZBSL = YDSL;//预定数量
                        int Z_YTHSL = 0;//已提货数量
                        Int64 Z_RJZGFHL = Z_ZBSL;//日均最高发货量，即时的写0
                        double Z_ZBJG = Convert.ToDouble(dt_TBDs.Rows[i]["TBJG"]);//中标价格已投标价为准
                        double Z_ZBJE = Convert.ToInt64(Z_ZBSL * Z_ZBJG);//中标金额
                        string Z_DBSJ = "null";   //定标时间
                        string Z_HTJSRQ = "null";  //合同结束日期
                        string Z_SFDJLYBZJ = "否";   //是否冻结履约保证金
                        double Z_LYBZJBL = Convert.ToDouble(htParameterInfo["卖家履约保证金比率"]);  //履约保证金比率，从动态参数设定表取值。
                        double Z_LYBZJJE = Math.Round(Z_ZBJE * Z_LYBZJBL, 2) < 0.01 ? 0.01 : Math.Round(Z_ZBJE * Z_LYBZJBL, 2);  //履约保证金金额
                        double Z_DJJE = Convert.ToDouble(dt_YDD.Rows[0]["DJDJ"]);//冻结订金
                        double Z_BZHBL = Convert.ToDouble(htParameterInfo["保证函比率"]); //保证函比率
                        double Z_BZHJE = Math.Round(Z_ZBJE * Convert.ToDouble(htParameterInfo["保证函比率"]), 2); //保证函金额

                        string T_YSTBDBH = dt_TBDs.Rows[i]["Number"].ToString();
                        string T_YSTBDDLYX = dt_TBDs.Rows[i]["DLYX"].ToString();
                        string T_YSTBDJSZHLX = dt_TBDs.Rows[i]["JSZHLX"].ToString();
                        string T_YSTBDMJJSBH = dt_TBDs.Rows[i]["MJJSBH"].ToString();
                        string T_YSTBDGLJJRYX = dt_TBDs.Rows[i]["GLJJRYX"].ToString();
                        string T_YSTBDGLJJRYHM = dt_TBDs.Rows[i]["GLJJRYHM"].ToString();
                        string T_YSTBDGLJJRJSBH = dt_TBDs.Rows[i]["GLJJRJSBH"].ToString();
                        string YSTBDGLJJRPTGLJG = dt_TBDs.Rows[i]["GLJJRPTGLJG"].ToString();
                        Int64 T_YSTBDMJSDJJPL = Convert.ToInt64(dt_TBDs.Rows[i]["MJSDJJPL"]);
                        string T_YSTBDGHQY = dt_TBDs.Rows[i]["GHQY"].ToString();
                        double T_YSTBDTBNSL = Convert.ToDouble(dt_TBDs.Rows[i]["TBNSL"]);
                        double T_YSTBDTBJG = Convert.ToDouble(dt_TBDs.Rows[i]["TBJG"]);
                        double T_YSTBDTBJE = Convert.ToDouble(dt_TBDs.Rows[i]["TBJE"]);
                        double T_YSTBDMJTBBZJBL = Convert.ToDouble(dt_TBDs.Rows[i]["MJTBBZJBL"]);
                        double T_YSTBDMJTBBZJZXZ = Convert.ToDouble(dt_TBDs.Rows[i]["MJTBBZJZXZ"]);
                        double T_YSTBDDJTBBZJ = Math.Round(Convert.ToDouble(Convert.ToDouble(dt_TBDs.Rows[i]["DJTBBZJ"]) * Convert.ToDouble(Convert.ToDouble(Z_ZBSL) / Convert.ToDouble(TBSL))), 2);//这个分支中，是按比例计算一个值
                        string YSTBDTJSJ = dt_TBDs.Rows[i]["TJSJ"].ToString();


                        string Y_YSYDDBH = dt_YDD.Rows[0]["Number"].ToString();
                        string Y_YSYDDDLYX = dt_YDD.Rows[0]["DLYX"].ToString();
                        string Y_YSYDDJSZHLX = dt_YDD.Rows[0]["JSZHLX"].ToString();
                        string Y_YSYDDMJJSBH = dt_YDD.Rows[0]["MJJSBH"].ToString();
                        string Y_YSYDDGLJJRYX = dt_YDD.Rows[0]["GLJJRYX"].ToString();
                        string Y_YSYDDGLJJRYHM = dt_YDD.Rows[0]["GLJJRYHM"].ToString();
                        string Y_YSYDDGLJJRJSBH = dt_YDD.Rows[0]["GLJJRJSBH"].ToString();
                        string YSYDDGLJJRPTGLJG = dt_YDD.Rows[0]["GLJJRPTGLJG"].ToString();
                        string Y_YSYDDSHQY = dt_YDD.Rows[0]["SHQY"].ToString();
                        string Y_YSYDDSHQYsheng = dt_YDD.Rows[0]["SHQYsheng"].ToString();
                        string Y_YSYDDSHQYshi = dt_YDD.Rows[0]["SHQYshi"].ToString();
                        double Y_YSYDDNMRJG = Convert.ToDouble(dt_YDD.Rows[0]["NMRJG"]);
                        double Y_YSYDDNDGSL = Convert.ToDouble(dt_YDD.Rows[0]["NDGSL"]);
                        double Y_YSYDDYZBSL = Convert.ToDouble(dt_YDD.Rows[0]["YZBSL"]);
                        double Y_YSYDDNDGJE = Convert.ToDouble(dt_YDD.Rows[0]["NDGJE"]);
                        double Y_YSYDDMJDJBL = Convert.ToDouble(dt_YDD.Rows[0]["MJDJBL"]);
                        double Y_YSYDDDJDJ = Convert.ToDouble(dt_YDD.Rows[0]["DJDJ"]);
                        string YSYDDTJSJ = dt_YDD.Rows[0]["TJSJ"].ToString();


                        string Q_QPZMSCSJ = "null";
                        string Q_ZMSCFDLYX = "null";
                        string Q_ZMSCFJSZHLX = "null";
                        string Q_ZMSCFJSBH = "null";
                        string Q_ZMWJLJ = "null";
                        string Q_ZFLYZH = "null";
                        string Q_ZFMBZH = "null";
                        string Q_ZFJE = "null";
                        string Q_SFYQR = "null";
                        string Q_QRSJ = "null";
                        string CreateUser = "admin";
                        string CreateTime = TimeNow;

                        //***********************
                        //参与本轮并中标的卖方数量(不用算，一直就是1) 
                        //参与本轮并中标的买方数量(不用算，一直就是1)
                        //参与本轮中标总金额(累加)
                        sum_Z_ZB_JE = sum_Z_ZB_JE + Z_ZBJE;
                        //参与本轮中标总数量(累加)
                        sum_Z_ZB_SL = sum_Z_ZB_SL + Z_ZBSL;
                        //**********************

                        string INSERT_ZBDBXXB = "INSERT INTO AAA_ZBDBXXB(Number, Z_HTBH, Z_HTQX, Z_HTZT, Z_QPZT, Z_SPBH, Z_SPMC, Z_GG, Z_JJDW, Z_PTSDZDJJPL, Z_ZBSJ, Z_ZBSL, Z_YTHSL, Z_RJZGFHL, Z_ZBJG, Z_ZBJE, Z_DBSJ, Z_HTJSRQ, Z_SFDJLYBZJ, Z_LYBZJBL, Z_LYBZJJE, Z_DJJE, Z_BZHBL, Z_BZHJE, T_YSTBDBH, T_YSTBDDLYX, T_YSTBDJSZHLX, T_YSTBDMJJSBH, T_YSTBDGLJJRYX, T_YSTBDGLJJRYHM, T_YSTBDGLJJRJSBH, YSTBDGLJJRPTGLJG, T_YSTBDMJSDJJPL, T_YSTBDGHQY, T_YSTBDTBNSL, T_YSTBDTBJG, T_YSTBDTBJE, T_YSTBDMJTBBZJBL, T_YSTBDMJTBBZJZXZ, T_YSTBDDJTBBZJ, YSTBDTJSJ, Y_YSYDDBH, Y_YSYDDDLYX, Y_YSYDDJSZHLX, Y_YSYDDMJJSBH, Y_YSYDDGLJJRYX, Y_YSYDDGLJJRYHM, Y_YSYDDGLJJRJSBH, YSYDDGLJJRPTGLJG, Y_YSYDDSHQY, Y_YSYDDSHQYsheng, Y_YSYDDSHQYshi, Y_YSYDDNMRJG, Y_YSYDDNDGSL, Y_YSYDDYZBSL, Y_YSYDDNDGJE, Y_YSYDDMJDJBL, Y_YSYDDDJDJ, YSYDDTJSJ, Q_QPZMSCSJ, Q_ZMSCFDLYX, Q_ZMSCFJSZHLX, Q_ZMSCFJSBH, Q_ZMWJLJ, Q_ZFLYZH, Q_ZFMBZH, Q_ZFJE, Q_SFYQR, Q_QRSJ, CreateUser, CreateTime) VALUES('" + ZBDB_Number + "','" + Z_HTBH + "','" + Z_HTQX + "','" + Z_HTZT + "','" + Z_QPZT + "','" + Z_SPBH + "','" + Z_SPMC + "','" + Z_GG + "','" + Z_JJDW + "','" + Z_PTSDZDJJPL + "','" + Z_ZBSJ + "','" + Z_ZBSL + "'," + Z_YTHSL + ",'" + Z_RJZGFHL + "','" + Z_ZBJG + "','" + Z_ZBJE + "'," + Z_DBSJ + "," + Z_HTJSRQ + ",'" + Z_SFDJLYBZJ + "','" + Z_LYBZJBL + "','" + Z_LYBZJJE + "','" + Z_DJJE + "','" + Z_BZHBL + "','" + Z_BZHJE + "','" + T_YSTBDBH + "','" + T_YSTBDDLYX + "','" + T_YSTBDJSZHLX + "','" + T_YSTBDMJJSBH + "','" + T_YSTBDGLJJRYX + "','" + T_YSTBDGLJJRYHM + "','" + T_YSTBDGLJJRJSBH + "','" + YSTBDGLJJRPTGLJG + "','" + T_YSTBDMJSDJJPL + "','" + T_YSTBDGHQY + "','" + T_YSTBDTBNSL + "','" + T_YSTBDTBJG + "','" + T_YSTBDTBJE + "','" + T_YSTBDMJTBBZJBL + "','" + T_YSTBDMJTBBZJZXZ + "','" + T_YSTBDDJTBBZJ + "','" + YSTBDTJSJ + "','" + Y_YSYDDBH + "','" + Y_YSYDDDLYX + "','" + Y_YSYDDJSZHLX + "','" + Y_YSYDDMJJSBH + "','" + Y_YSYDDGLJJRYX + "','" + Y_YSYDDGLJJRYHM + "','" + Y_YSYDDGLJJRJSBH + "','" + YSYDDGLJJRPTGLJG + "','" + Y_YSYDDSHQY + "','" + Y_YSYDDSHQYsheng + "','" + Y_YSYDDSHQYshi + "','" + Y_YSYDDNMRJG + "','" + Y_YSYDDNDGSL + "','" + Y_YSYDDYZBSL + "','" + Y_YSYDDNDGJE + "','" + Y_YSYDDMJDJBL + "','" + Y_YSYDDDJDJ + "','" + YSYDDTJSJ + "'," + Q_QPZMSCSJ + "," + Q_ZMSCFDLYX + "," + Q_ZMSCFJSZHLX + "," + Q_ZMSCFJSBH + "," + Q_ZMWJLJ + "," + Q_ZFLYZH + "," + Q_ZFMBZH + "," + Q_ZFJE + "," + Q_SFYQR + "," + Q_QRSJ + ",'" + CreateUser + "','" + CreateTime + "')";
                        al.Add(INSERT_ZBDBXXB);
                        #endregion
                        //更新投标单信息
                        Double new_TBJE = Math.Round((Convert.ToInt64(dt_TBDs.Rows[i]["TBNSL"]) - Convert.ToInt64(dt_TBDs.Rows[i]["YZBSL"]) - YDSL) * Convert.ToDouble(dt_TBDs.Rows[i]["TBJG"]), 2);

                        //这种情况，预订单被拆单了
                        string cd_sqlstr = "  "; //拆单更新处理sql
                        //若是此预订单第一次中标 并且 部分中标
                        if (Convert.ToInt64(dt_TBDs.Rows[i]["YZBSL"]) == 0)
                        {
                            cd_sqlstr = " ,SFCD='是' ";
                        }

                        string UPDATE_TBD = "UPDATE AAA_TBD SET YZBSL=(CASE (SELECT ZT FROM AAA_TBD WHERE Number='" + TBD_Number + "') WHEN '竞标' THEN (" + YDSL + " + YZBSL) WHEN '撤销' THEN 'aaa' WHEN '中标' THEN 'aaa' END),DJTBBZJ = DJTBBZJ- " + T_YSTBDDJTBBZJ + ", TBJE=" + new_TBJE + cd_sqlstr + " WHERE Number='" + TBD_Number + "'";//如果单子的状态不是竞标，这句SQL会出错，从而使得事务回滚。  反写：已中标数量、冻结投标保证金、投标金额
                        al.Add(UPDATE_TBD);
                        //Int64 TBSL = Convert.ToInt64(dt_TBDs.Rows[i]["TBNSL"]) - Convert.ToInt64(dt_TBDs.Rows[i]["YZBSL"]);//投标数量
                        //如果投标单余量 < 卖家设定的经济批量，撤销该订单。
                        Int64 MJSDJJPL = Convert.ToInt64(dt_TBDs.Rows[i]["MJSDJJPL"]);//卖家设定的经济批量
                        if (((TBSL - YDSL) < MJSDJJPL) && (TBSL - YDSL) != 0)
                        {
                            string CX_TBD = "UPDATE AAA_TBD SET ZT='中标',TBNSL=YZBSL WHERE Number='" + TBD_Number + "'";
                            al.Add(CX_TBD);
                            string sql_LSDZ = "SELECT * FROM AAA_moneyDZB WHERE Number='1304000004'";//撤销投标单解冻对照
                            DataTable dt_LSDZ = DbHelperSQL.Query(sql_LSDZ).Tables[0];
                            string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES('" + PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "") + "','" + dt_TBDs.Rows[i]["DLYX"].ToString() + "','" + dt_TBDs.Rows[i]["JSZHLX"].ToString() + "','" + dt_TBDs.Rows[i]["MJJSBH"].ToString() + "','" + "AAA_TBD" + "','" + TBD_Number + "','" + DateTime.Now.ToString() + "','" + dt_LSDZ.Rows[0]["YSLX"].ToString() + "','" + (Convert.ToDouble(dt_TBDs.Rows[i]["DJTBBZJ"].ToString()) - T_YSTBDDJTBBZJ) + "','" + dt_LSDZ.Rows[0]["XM"].ToString() + "','" + dt_LSDZ.Rows[0]["XZ"].ToString() + "','" + dt_LSDZ.Rows[0]["ZY"].ToString().Replace("[x1]", TBD_Number) + "','" + "接口编号" + "','" + TBD_Number + "投标单撤销" + "','" + dt_LSDZ.Rows[0]["SJLX"].ToString() + "')";//插入流水表
                            al.Add(sql_LS);

                            string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+" + (Convert.ToDouble(dt_TBDs.Rows[i]["DJTBBZJ"].ToString())  -T_YSTBDDJTBBZJ) + " WHERE B_DLYX='" + dt_TBDs.Rows[i]["DLYX"].ToString() + "'"; //更新余额
                            al.Add(sql_YE);
                    
                        }


                    }
                    #endregion

                    #region 预定数量=投标数量，最理想情况。
                    if (YDSL == TBSL)
                    {
                        string TimeNow = DateTime.Now.ToString();//本次业务产生的时间都用这个
                        //更新投标单信息表
                        string UPDATE_TBD = "UPDATE AAA_TBD SET ZT='中标', YZBSL=(CASE (SELECT ZT FROM AAA_TBD WHERE Number='" + TBD_Number + "') WHEN '竞标' THEN TBNSL WHEN '撤销' THEN 'aaa' WHEN '中标' THEN 'aaa' END) WHERE Number='" + TBD_Number + "'";//如果单子的状态不是竞标，这句SQL会出错，从而使得事务回滚。
                        al.Add(UPDATE_TBD);
                        //更新预订单信息表
                        string UPDATE_YDD = "UPDATE AAA_YDDXXB SET ZT='中标', YZBSL=(CASE (SELECT ZT FROM AAA_YDDXXB WHERE Number='" + YDD_Number + "') WHEN '竞标' THEN NDGSL WHEN '撤销' THEN 'aaa' WHEN '中标' THEN 'aaa' END) WHERE Number='" + YDD_Number + "'";//如果预订单状态不是竞标，这句SQL会出错，从而使得事务回滚。
                        al.Add(UPDATE_YDD);
                        //插入中标信息表
                        ZBDB_Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZBDBXXB", "N");//中标定标主键
                        string Z_HTBH = "HT" + ZBDB_Number; //合同编号
                        string Z_HTQX = "即时";
                        string Z_HTZT = "中标";
                        string Z_QPZT = "未开始清盘";
                        string Z_SPBH = SPBH;
                        string Z_SPMC = dt_TBDs.Rows[i]["SPMC"].ToString();//商品名称
                        string Z_GG = dt_TBDs.Rows[i]["GG"].ToString();
                        string Z_JJDW = dt_TBDs.Rows[i]["JJDW"].ToString();
                        string Z_PTSDZDJJPL = dt_TBDs.Rows[i]["PTSDZDJJPL"].ToString(); //平台设定最大经济批量
                        string Z_ZBSJ = TimeNow;  //中标时间
                        Int64 Z_ZBSL = TBSL;//投标||预定数量
                        int Z_YTHSL = 0;//已提货数量
                        Int64 Z_RJZGFHL = Z_ZBSL;//日均最高发货量，即时的写0
                        double Z_ZBJG = Convert.ToDouble(dt_TBDs.Rows[i]["TBJG"]);//中标价格已投标价为准
                        double Z_ZBJE = Convert.ToInt64(Z_ZBSL * Z_ZBJG);//中标金额
                        string Z_DBSJ = "null";   //定标时间
                        string Z_HTJSRQ = "null";  //合同结束日期
                        string Z_SFDJLYBZJ = "否";   //是否冻结履约保证金
                        double Z_LYBZJBL = Convert.ToDouble(htParameterInfo["卖家履约保证金比率"]);  //履约保证金比率，从动态参数设定表取值。
                        double Z_LYBZJJE = Math.Round(Z_ZBJE * Z_LYBZJBL, 2) < 0.01 ? 0.01 : Math.Round(Z_ZBJE * Z_LYBZJBL, 2);  //履约保证金金额
                        double Z_DJJE = Convert.ToDouble(dt_YDD.Rows[0]["DJDJ"]);//冻结订金
                        double Z_BZHBL = Convert.ToDouble(htParameterInfo["保证函比率"]); //保证函比率
                        double Z_BZHJE = Math.Round(Z_ZBJE * Convert.ToDouble(htParameterInfo["保证函比率"]), 2); //保证函金额

                        string T_YSTBDBH = dt_TBDs.Rows[i]["Number"].ToString();
                        string T_YSTBDDLYX = dt_TBDs.Rows[i]["DLYX"].ToString();
                        string T_YSTBDJSZHLX = dt_TBDs.Rows[i]["JSZHLX"].ToString();
                        string T_YSTBDMJJSBH = dt_TBDs.Rows[i]["MJJSBH"].ToString();
                        string T_YSTBDGLJJRYX = dt_TBDs.Rows[i]["GLJJRYX"].ToString();
                        string T_YSTBDGLJJRYHM = dt_TBDs.Rows[i]["GLJJRYHM"].ToString();
                        string T_YSTBDGLJJRJSBH = dt_TBDs.Rows[i]["GLJJRJSBH"].ToString();
                        string YSTBDGLJJRPTGLJG = dt_TBDs.Rows[i]["GLJJRPTGLJG"].ToString();
                        Int64 T_YSTBDMJSDJJPL = Convert.ToInt64(dt_TBDs.Rows[i]["MJSDJJPL"]);
                        string T_YSTBDGHQY = dt_TBDs.Rows[i]["GHQY"].ToString();
                        double T_YSTBDTBNSL = Convert.ToDouble(dt_TBDs.Rows[i]["TBNSL"]);
                        double T_YSTBDTBJG = Convert.ToDouble(dt_TBDs.Rows[i]["TBJG"]);
                        double T_YSTBDTBJE = Convert.ToDouble(dt_TBDs.Rows[i]["TBJE"]);
                        double T_YSTBDMJTBBZJBL = Convert.ToDouble(dt_TBDs.Rows[i]["MJTBBZJBL"]);
                        double T_YSTBDMJTBBZJZXZ = Convert.ToDouble(dt_TBDs.Rows[i]["MJTBBZJZXZ"]);
                        double T_YSTBDDJTBBZJ = Convert.ToDouble(dt_TBDs.Rows[i]["DJTBBZJ"]);//这个分支中，是直接取余量的所有值
                        string YSTBDTJSJ = dt_TBDs.Rows[i]["TJSJ"].ToString();


                        string Y_YSYDDBH = dt_YDD.Rows[0]["Number"].ToString();
                        string Y_YSYDDDLYX = dt_YDD.Rows[0]["DLYX"].ToString();
                        string Y_YSYDDJSZHLX = dt_YDD.Rows[0]["JSZHLX"].ToString();
                        string Y_YSYDDMJJSBH = dt_YDD.Rows[0]["MJJSBH"].ToString();
                        string Y_YSYDDGLJJRYX = dt_YDD.Rows[0]["GLJJRYX"].ToString();
                        string Y_YSYDDGLJJRYHM = dt_YDD.Rows[0]["GLJJRYHM"].ToString();
                        string Y_YSYDDGLJJRJSBH = dt_YDD.Rows[0]["GLJJRJSBH"].ToString();
                        string YSYDDGLJJRPTGLJG = dt_YDD.Rows[0]["GLJJRPTGLJG"].ToString();
                        string Y_YSYDDSHQY = dt_YDD.Rows[0]["SHQY"].ToString();
                        string Y_YSYDDSHQYsheng = dt_YDD.Rows[0]["SHQYsheng"].ToString();
                        string Y_YSYDDSHQYshi = dt_YDD.Rows[0]["SHQYshi"].ToString();
                        double Y_YSYDDNMRJG = Convert.ToDouble(dt_YDD.Rows[0]["NMRJG"]);
                        double Y_YSYDDNDGSL = Convert.ToDouble(dt_YDD.Rows[0]["NDGSL"]);
                        double Y_YSYDDYZBSL = Convert.ToDouble(dt_YDD.Rows[0]["YZBSL"]);
                        double Y_YSYDDNDGJE = Convert.ToDouble(dt_YDD.Rows[0]["NDGJE"]);
                        double Y_YSYDDMJDJBL = Convert.ToDouble(dt_YDD.Rows[0]["MJDJBL"]);
                        double Y_YSYDDDJDJ = Convert.ToDouble(dt_YDD.Rows[0]["DJDJ"]);
                        string YSYDDTJSJ = dt_YDD.Rows[0]["TJSJ"].ToString();


                        string Q_QPZMSCSJ = "null";
                        string Q_ZMSCFDLYX = "null";
                        string Q_ZMSCFJSZHLX = "null";
                        string Q_ZMSCFJSBH = "null";
                        string Q_ZMWJLJ = "null";
                        string Q_ZFLYZH = "null";
                        string Q_ZFMBZH = "null";
                        string Q_ZFJE = "null";
                        string Q_SFYQR = "null";
                        string Q_QRSJ = "null";
                        string CreateUser = "admin";
                        string CreateTime = TimeNow;

                        //***********************
                        //参与本轮并中标的卖方数量(不用算，一直就是1) 
                        //参与本轮并中标的买方数量(不用算，一直就是1)
                        //参与本轮中标总金额(累加)
                        sum_Z_ZB_JE = sum_Z_ZB_JE + Z_ZBJE;
                        //参与本轮中标总数量(累加)
                        sum_Z_ZB_SL = sum_Z_ZB_SL + Z_ZBSL;
                        //**********************

                        string INSERT_ZBDBXXB = "INSERT INTO AAA_ZBDBXXB(Number, Z_HTBH, Z_HTQX, Z_HTZT, Z_QPZT, Z_SPBH, Z_SPMC, Z_GG, Z_JJDW, Z_PTSDZDJJPL, Z_ZBSJ, Z_ZBSL, Z_YTHSL, Z_RJZGFHL, Z_ZBJG, Z_ZBJE, Z_DBSJ, Z_HTJSRQ, Z_SFDJLYBZJ, Z_LYBZJBL, Z_LYBZJJE, Z_DJJE, Z_BZHBL, Z_BZHJE, T_YSTBDBH, T_YSTBDDLYX, T_YSTBDJSZHLX, T_YSTBDMJJSBH, T_YSTBDGLJJRYX, T_YSTBDGLJJRYHM, T_YSTBDGLJJRJSBH, YSTBDGLJJRPTGLJG, T_YSTBDMJSDJJPL, T_YSTBDGHQY, T_YSTBDTBNSL, T_YSTBDTBJG, T_YSTBDTBJE, T_YSTBDMJTBBZJBL, T_YSTBDMJTBBZJZXZ, T_YSTBDDJTBBZJ, YSTBDTJSJ, Y_YSYDDBH, Y_YSYDDDLYX, Y_YSYDDJSZHLX, Y_YSYDDMJJSBH, Y_YSYDDGLJJRYX, Y_YSYDDGLJJRYHM, Y_YSYDDGLJJRJSBH, YSYDDGLJJRPTGLJG, Y_YSYDDSHQY, Y_YSYDDSHQYsheng, Y_YSYDDSHQYshi, Y_YSYDDNMRJG, Y_YSYDDNDGSL, Y_YSYDDYZBSL, Y_YSYDDNDGJE, Y_YSYDDMJDJBL, Y_YSYDDDJDJ, YSYDDTJSJ, Q_QPZMSCSJ, Q_ZMSCFDLYX, Q_ZMSCFJSZHLX, Q_ZMSCFJSBH, Q_ZMWJLJ, Q_ZFLYZH, Q_ZFMBZH, Q_ZFJE, Q_SFYQR, Q_QRSJ, CreateUser, CreateTime) VALUES('" + ZBDB_Number + "','" + Z_HTBH + "','" + Z_HTQX + "','" + Z_HTZT + "','" + Z_QPZT + "','" + Z_SPBH + "','" + Z_SPMC + "','" + Z_GG + "','" + Z_JJDW + "','" + Z_PTSDZDJJPL + "','" + Z_ZBSJ + "','" + Z_ZBSL + "'," + Z_YTHSL + ",'" + Z_RJZGFHL + "','" + Z_ZBJG + "','" + Z_ZBJE + "'," + Z_DBSJ + "," + Z_HTJSRQ + ",'" + Z_SFDJLYBZJ + "','" + Z_LYBZJBL + "','" + Z_LYBZJJE + "','" + Z_DJJE + "','" + Z_BZHBL + "','" + Z_BZHJE + "','" + T_YSTBDBH + "','" + T_YSTBDDLYX + "','" + T_YSTBDJSZHLX + "','" + T_YSTBDMJJSBH + "','" + T_YSTBDGLJJRYX + "','" + T_YSTBDGLJJRYHM + "','" + T_YSTBDGLJJRJSBH + "','" + YSTBDGLJJRPTGLJG + "','" + T_YSTBDMJSDJJPL + "','" + T_YSTBDGHQY + "','" + T_YSTBDTBNSL + "','" + T_YSTBDTBJG + "','" + T_YSTBDTBJE + "','" + T_YSTBDMJTBBZJBL + "','" + T_YSTBDMJTBBZJZXZ + "','" + T_YSTBDDJTBBZJ + "','" + YSTBDTJSJ + "','" + Y_YSYDDBH + "','" + Y_YSYDDDLYX + "','" + Y_YSYDDJSZHLX + "','" + Y_YSYDDMJJSBH + "','" + Y_YSYDDGLJJRYX + "','" + Y_YSYDDGLJJRYHM + "','" + Y_YSYDDGLJJRJSBH + "','" + YSYDDGLJJRPTGLJG + "','" + Y_YSYDDSHQY + "','" + Y_YSYDDSHQYsheng + "','" + Y_YSYDDSHQYshi + "','" + Y_YSYDDNMRJG + "','" + Y_YSYDDNDGSL + "','" + Y_YSYDDYZBSL + "','" + Y_YSYDDNDGJE + "','" + Y_YSYDDMJDJBL + "','" + Y_YSYDDDJDJ + "','" + YSYDDTJSJ + "'," + Q_QPZMSCSJ + "," + Q_ZMSCFDLYX + "," + Q_ZMSCFJSZHLX + "," + Q_ZMSCFJSBH + "," + Q_ZMWJLJ + "," + Q_ZFLYZH + "," + Q_ZFMBZH + "," + Q_ZFJE + "," + Q_SFYQR + "," + Q_QRSJ + ",'" + CreateUser + "','" + CreateTime + "')";
                        al.Add(INSERT_ZBDBXXB);
                    }
                    #endregion


                    //找参与竞标的买家和卖家数量，更新到中标定标信息表
                    DataSet ds_jyfsl = DbHelperSQL.Query("select (select count(distinct MJJSBH) from AAA_TBD where ZT = '竞标' and SPBH ='" + SPBH + "' and HTQX='即时') as 卖方数量 , (select count(distinct MJJSBH) from AAA_YDDXXB where ZT = '竞标' and SPBH ='" + SPBH + "' and HTQX='即时') as 买方数量");
                    string Z_LC_number = Guid.NewGuid().ToString();

                    al.Add("update AAA_ZBDBXXB set Z_Cynum_Sel = " + ds_jyfsl.Tables[0].Rows[0]["卖方数量"].ToString() + " , Z_Cynum_Buy = " + ds_jyfsl.Tables[0].Rows[0]["买方数量"].ToString() + " , Z_LC_number = '" + Z_LC_number + "', Z_Cynum_ZB_Sel=" + sum_Z_Cynum_ZB_Sel + ", Z_Cynum_ZB_Buy=" + sum_Z_Cynum_ZB_Buy.ToString() + ",Z_ZB_JE=" + sum_Z_ZB_JE.ToString() + ",Z_ZB_SL=" + sum_Z_ZB_SL.ToString() + "  where Number = '" + ZBDB_Number + "' ");


                    if (Convert.ToInt32(DbHelperSQL.GetSingle("select COUNT(*) from AAA_TBZLSHB where  TBDH='" + dt_TBDs.Rows[i]["Number"].ToString() + "'")) == 0)//不存在这条投标单单号数据
                    {
                        string newnumberTBZLSH = jhjx_PublicClass.GetNextNumberZZ("AAA_TBZLSHB", "");
                        al.Add("  insert AAA_TBZLSHB(Number,TBDH,FWZXSHZT,JYGLBSHZT,FWZXSHYCHSFXG,JYGLBSHWTGHSFXG,CreateTime,CheckLimitTime) values('" + newnumberTBZLSH + "','" + dt_TBDs.Rows[i]["Number"].ToString() + "','未审核','初始值','初始值','初始值','"+DateTime.Now.ToString()+"','"+DateTime.Now.ToString()+"') ");
                    }

                    #endregion
                }


            }
            #endregion
            //无论是否中标，更新“大盘数据变化时间对照表”中最后一次执行即时中标监控时间 为 预存时间 by 于海滨
            al.Add(" update AAA_DaPanTJTime set LastJSZBtime = LastJSZBtime_YC where LastYXBGtime>=LastJSZBtime ");
            al[0] = " ----即时交易成功中标商品数量" + Num_zbspsl + "个， 共" + Num_runspsl + "个商品参与计算。  " + Environment.NewLine ;
            //-----------------------------------------------
            return al;
 
    }


}