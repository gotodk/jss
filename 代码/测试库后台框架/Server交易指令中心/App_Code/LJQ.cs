using FMDBHelperClass;
using FMipcClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RedisReLoad.ShareDBcache;
/// <summary>
/// LJQ 的摘要说明
/// </summary>
public class LJQ
{

    public static ArrayList SFJRLJQ(DataTable dtRun)
    {
        if (dtRun == null || dtRun.Rows.Count < 1)
        {
            return null;
        }


        //提醒：删除txt，定义并初始化清空临时变量。A
        //dstx = null;

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsTB = new DataSet();
        Hashtable param = new Hashtable();

        ArrayList ALsql = new ArrayList();


        ALsql.Add("   ");
        #region//根据参数，获取需要处理的投标信息
        
        param["@SPBH" ] = dtRun.Rows[0]["商品编号"].ToString();
        param["@HTQX"] = dtRun.Rows[0]["合同期限"].ToString();
        param["@ZT"] = "竞标";
        param["@HTQXJS"] = "即时";

        //按照价格从小到大，时间从晚到早找出top1
        //string sql_dsTB =  " select top 1 ZZZ.Number,ZZZ.DLYX,ZZZ.JSZHLX,ZZZ.MJJSBH,ZZZ.GLJJRYX,ZZZ.GLJJRYHM,ZZZ.GLJJRJSBH,ZZZ.GLJJRPTGLJG,ZZZ.SPBH,ZZZ.SPMC,ZZZ.GG,ZZZ.JJDW,ZZZ.PTSDZDJJPL,ZZZ.HTQX,ZZZ.MJSDJJPL,ZZZ.GHQY,ZZZ.TBNSL,ZZZ.TBJG,ZZZ.TBJE,ZZZ.MJTBBZJBL,ZZZ.MJTBBZJZXZ,ZZZ.DJTBBZJ,ZZZ.ZT,ZZZ.TJSJ,ZZZ.CXSJ,ZZZ.ZLBZYZM,ZZZ.CPJCBG,ZZZ.PGZFZRFLCNS,ZZZ.FDDBRCNS,ZZZ.SHFWGDYCN,ZZZ.CPSJSQS,ZZZ.CreateTime,LLL.LJQKSSJ,LLL.LJQJSSJ,LLL.SFJRLJQ,LLL.LJQYZBS,'是否凑齐'='否','成交量'=0,'是否在冷静期内'=isnull(LLL.SFJRLJQ,'否') from AAA_TBD as ZZZ left join AAA_LJQDQZTXXB as LLL on LLL.SPBH=ZZZ.SPBH and LLL.HTQX=ZZZ.HTQX where ZZZ.SPBH = @SPBH and  ZZZ.HTQX = @HTQX and  ZZZ.ZT='竞标' AND ZZZ.HTQX <> '即时'  order  by ZZZ.TBJG asc,ZZZ.TJSJ asc   ";//AND ZZZ.HTQX <> '即时'

        string sql_dsTB = " select top 1 ZZZ.Number,TBNSL,TBJG,ZZZ.HTQX,ZZZ.SPBH,'是否凑齐'='否','成交量'=0,'是否在冷静期内'=isnull(LLL.SFJRLJQ,'否'),ZZZ.GHQY from AAA_TBD as ZZZ left join AAA_LJQDQZTXXB as LLL on LLL.SPBH=ZZZ.SPBH and LLL.HTQX=ZZZ.HTQX where ZZZ.SPBH = @SPBH and  ZZZ.HTQX = @HTQX and  ZZZ.ZT=@ZT AND ZZZ.HTQX <>@HTQXJS  order  by ZZZ.TBJG asc,ZZZ.TJSJ asc   ";//AND ZZZ.HTQX <> '即时'

        Hashtable return_ht = I_DBL.RunParam_SQL(sql_dsTB, "", param);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsTB = (DataSet)(return_ht["return_ds"]);
            if (dsTB == null || dsTB.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
        }
        else
        {
            throw ((Exception)return_ht["return_errmsg"]);
            //return null;
        } //?
        #endregion

        Hashtable HTCSAll = new  SetYDD_hqcs().Get_LJQcs();
        Hashtable htinput = null;

        #region//遍历要处理的投标信息
        
        //参与本轮并中标的买方数量(累加，但要过滤重复的买方)
        Hashtable htsum_Z_Cynum_ZB_Buy = new Hashtable();

        string ZBSJ = DateTime.Now.ToString(); //中标时间，本次产生的数据保持一致，所以需要放到这里

        Int64 TBsl = Convert.ToInt64(dsTB.Tables[0].Rows[0]["TBNSL"]); //当前投标拟售量
        double TBJG = Convert.ToDouble(dsTB.Tables[0].Rows[0]["TBJG"]); //投标价格
        string HTQX = dsTB.Tables[0].Rows[0]["HTQX"].ToString(); //合同期限
        string SPBH = dsTB.Tables[0].Rows[0]["SPBH"].ToString(); //商品编号
        string SFJRLJQ = dsTB.Tables[0].Rows[0]["是否在冷静期内"].ToString(); //是否进入冷静期
        string GHQY = dsTB.Tables[0].Rows[0]["GHQY"].ToString(); //供货区域

        #region//临时记录的语句，如果最后为凑齐，则这些语句都无效
        ArrayList alSQLtemp = new ArrayList();

        Int64 CQsl = 0; //凑齐数量
        //看某个投标信息能不能凑齐
        htinput = new Hashtable();
        htinput["@TBJG"] = TBJG;
        htinput["@GHQY"] = GHQY;
        htinput["@HTQX"] = HTQX;
        htinput["@SPBH"] = SPBH;
        htinput["@ZT"] = "竞标";
        return_ht = I_DBL.RunParam_SQL(" select NDGSL,  YZBSL from AAA_YDDXXB where HTQX=@HTQX and ZT=@ZT and SPBH  =@SPBH  and NMRJG >= @TBJG and charindex('|'+SHQYsheng+'|',@GHQY)>0   order  by CreateTime asc  ", "", htinput);
        DataSet dsYDD = null;//DbHelperSQL.Query(sql_dsYDD);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsYDD = (DataSet)(return_ht["return_ds"]);

        }
        else
        {
            //return null;
            throw ((Exception)return_ht["return_errmsg"]);
        }

        List<Hashtable> lh = null;//new List<Hashtable>();
        if (dsYDD != null && dsYDD.Tables[0].Rows.Count > 0)
        {
            lh = new List<Hashtable>();
            #region//遍历预订单信息表
            for (int y = 0; y < dsYDD.Tables[0].Rows.Count; y++)
            {
                CQsl = CQsl + (Convert.ToInt64(dsYDD.Tables[0].Rows[y]["NDGSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]));

                #region//下限
                Int64 min = Convert.ToInt64(Math.Round(TBsl * 0.9));
                #endregion

                #region//凑到了大于预售量
                if (CQsl >= min && TBsl > 0)
                {
                    dsTB.Tables[0].Rows[0]["是否凑齐"] = "是";
                    break;
                }
                #endregion
                    
            } //预订单遍历结束
            #endregion
                                
        }
        #endregion

        //无论找没找到能匹配的预订单，都进行一下冷静期的处理
        //看这个投标信息的商品是否进入了冷静期，对冷静期控制进行处理
        
        #region//不在冷静期
        if (SFJRLJQ != "是")  //不在冷静期
        {
            if (dsTB.Tables[0].Rows[0]["是否凑齐"].ToString() == "是") //可以凑齐
            {
                //更新冷静期状态为是，时间更新为当前时间。 冷静期进入时间，需要与中标时间一致。
                string t1 = ZBSJ;
                DateTime time1begin = DateTime.Parse(t1);

                DateTime time2begin = time1begin.AddDays(1);

                //计算冷静期结束时间
                /*-----------shiyan 2014-11-04 注释 改为通过redis获取数据推算---------
                DataSet dsJQ = null; //DbHelperSQL.Query(" select * from AAA_PTJRSDB where SFYX = '是' order by RQ asc ");
                return_ht = I_DBL.RunProc(" select RQ from AAA_PTJRSDB where SFYX = '是' order by RQ asc ", "");
                if ((bool)(return_ht["return_float"])) //说明执行完成
                {
                    //这里就可以用了。
                    dsJQ = (DataSet)(return_ht["return_ds"]);

                }
                else
                {
                    //return null;
                    throw ((Exception)return_ht["return_errmsg"]);
                }

                if (dsJQ != null && dsJQ.Tables[0].Rows.Count > 0)
                {
                    for (int p = 0; p < dsJQ.Tables[0].Rows.Count; p++)
                    {
                        //若日期存在，则说明不能出冷静期
                        DateTime thisrowtime = DateTime.Parse(dsJQ.Tables[0].Rows[p]["RQ"].ToString());
                        //若日期在数据库中存在，则累加一天
                        if (time2begin.Date.CompareTo(thisrowtime.Date) == 0)
                        {
                            time2begin = time2begin.AddDays(1);
                        }
                    }
                }
               ------------------------------------------------------------------------*/
                //通过redis获取数据的方式推算冷静期结束时间
                while (IsHoliday.ReadHoliday(time2begin.ToString("yyyy-MM-dd")) == "是")
                {                    
                    time2begin = time2begin.AddDays(1);
                }

                string t2 = time2begin.ToString();

                ALsql.Add(" update AAA_LJQDQZTXXB set SFJRLJQ = '是',LJQKSSJ = '" + t1 + "',LJQJSSJ = '" + t2 + "' where    HTQX = @HTQX and SPBH  = @SPBH ");
                //记录历史记录
                string newnumber = ""; //jhjx_PublicClass.GetNextNumberZZ("AAA_LJQBGLSJLB", "");
                object[] re = HTCSAll["AAA_LJQBGLSJLB获取主键"] as object[]; //--------------------

                if (re[0].ToString() == "ok")
                {
                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                    if (re[1] != null && !re[1].ToString().Trim().Equals(""))
                    {
                        newnumber = (string)(re[1]);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                    //return null;
                    throw ((Exception)re[1]);
                }
                ALsql.Add(" INSERT INTO AAA_LJQBGLSJLB (Number,SPBH,HTQX,LJQKSSJBGW,LJQJSSJBGW,SFZLJQZTBGW) VALUES ('" + newnumber + "',@SPBH,@HTQX,'" + t1 + "','" + t2 + "','是') ");

                //写入投标资料审核表   2013-08-22 添加
                DataSet dsCount = null;
                return_ht = I_DBL.RunProc("select COUNT(*) from AAA_TBZLSHB where  TBDH='" + dsTB.Tables[0].Rows[0]["Number"].ToString() + "'", "");
                if ((bool)(return_ht["return_float"])) //说明执行完成
                {
                    //这里就可以用了。
                    dsCount = (DataSet)(return_ht["return_ds"]);

                }
                else
                {
                    //return null;
                    throw ((Exception)return_ht["return_errmsg"]);
                }

                if (int.Parse(dsCount.Tables[0].Rows[0][0].ToString()) == 0)//不存在这条投标单单号数据
                {
                    string newnumberTBZLSH = "";//jhjx_PublicClass.GetNextNumberZZ("AAA_TBZLSHB", "");

                    re = HTCSAll["AAA_TBZLSHB获取主键"] as object[]; //--------------------

                    if (re[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                        if (re[1] != null && !re[1].ToString().Trim().Equals(""))
                        {
                            newnumberTBZLSH = (string)(re[1]);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                        //return null;
                        throw ((Exception)re[1]);
                    }

                    ALsql.Add("  insert AAA_TBZLSHB(Number,TBDH,FWZXSHZT,JYGLBSHZT,FWZXSHYCHSFXG,JYGLBSHWTGHSFXG,CreateTime,CheckLimitTime) values('" + newnumberTBZLSH + "','" + dsTB.Tables[0].Rows[0]["Number"].ToString() + "','未审核','初始值','初始值','初始值','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "') ");
                }

                ALsql[0] = ALsql[0].ToString() + "  select '有商品进入冷静期'   ";

            }
            else //不能凑齐
            {
                //冷静期状态不动，时间不动
            }
        }
        #endregion
        
        #endregion

        if (ALsql != null && ALsql.Count > 0)
        {
            return ALsql;
        }
        else
        {
            return null;
        }


    }

}