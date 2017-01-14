using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FMDBHelperClass;
using FMipcClass;
using FMPublicClass;
using System.Data;
using System.Collections;

/// <summary>
/// orderGetPdata 的摘要说明
/// </summary>
public class orderGetPdata
{
	public orderGetPdata()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 初始化返回值数据集,执行结果只有两种ok和err(大多数情况是这个标准)
    /// </summary>
    /// <returns></returns>
    private DataSet initReturnDataSet()
    {
        DataSet ds = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.TableName = "返回值单条";
        auto2.Columns.Add("执行结果");
        auto2.Columns.Add("提示文本");
        auto2.Columns.Add("附件信息1");
        auto2.Columns.Add("附件信息2");
        auto2.Columns.Add("附件信息3");
        auto2.Columns.Add("附件信息4");
        auto2.Columns.Add("附件信息5");
        ds.Tables.Add(auto2);
        return ds;
    }
    /// <summary>
    /// 商品买卖B区获取分页数据
    /// </summary>
    /// <param name="ds_page">分页使用参数</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqSPMM(DataSet ds_page, Hashtable ht_tiaojian)
    {
        DataSet dsreturn = new DataSet();
        switch (ht_tiaojian["标签名"].ToString())
        {
            case "预订单管理":
                dsreturn = GetBqYDDGL(ds_page, ht_tiaojian);
                break;
            case "投标单管理":
                dsreturn = GetBqTBDGL(ds_page, ht_tiaojian);
                break;
            case "异常投标单":
                dsreturn = GetBqYCTBD(ds_page, ht_tiaojian);
                break;           
            default:
                dsreturn = null;
                break;
        }
        return dsreturn;
    }

    /// <summary>
    /// 商品买卖B区预订单管理
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqYDDGL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集 
           
            if (!ht_tiaojian.Contains("买方编号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " * "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " (SELECT '修改' AS 修改,'撤销' AS 撤销,  Number,SPBH,SPMC,GG,NDGSL,YZBSL,NMRJG,NDGJE,ISNULL(CAST((SELECT TOP 1 TBJG FROM AAA_TBD WHERE ZT='竞标' AND AAA_TBD.SPBH=AAA_YDDXXB.SPBH and AAA_TBD.HTQX=AAA_YDDXXB.HTQX ORDER BY TBJG ASC) AS varchar(50)),'--') AS DQMJZDJ,DJDJ,(CASE WHEN YZBSL= NDGSL THEN '否' WHEN YZBSL=0 THEN '否' ELSE '是' END) AS SFCD,(CASE WHEN(SELECT SFJRLJQ FROM AAA_LJQDQZTXXB WHERE AAA_LJQDQZTXXB.HTQX = AAA_YDDXXB.HTQX AND AAA_LJQDQZTXXB.SPBH=AAA_YDDXXB.SPBH)='是' THEN '竞标中（冷静期）' ELSE '竞标中' END) AS DQZT,isnull(replace(convert(varchar(20),YXJZRQ,111),'/','-'),'--') YXJZRQ,HTQX,SHQY FROM AAA_YDDXXB WHERE AAA_YDDXXB.ZT='竞标' AND AAA_YDDXXB.MJJSBH='" + ht_tiaojian["买方编号"].ToString () + "') AS TABLE1  ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " Number ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

            if (ht_tiaojian["当前状态"].ToString() != "全部")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and DQZT='" + ht_tiaojian["当前状态"].ToString() + "' ";
            }

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                 ds_res = (DataSet)re_ds[1];                
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 商品买卖B区投标单管理
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqTBDGL(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集 

            if (!ht_tiaojian.Contains("卖方编号"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " '撤销' AS 撤销,'修改' AS 修改,AAA_TBD.Number,SPMC,GG,TBNSL,YZBSL,TBJG,TBJE,DJTBBZJ,AAA_TBD.SPBH,AAA_TBD.HTQX,GHQY,ZT,MJJSBH,''AS XTDCL,'' AS ZDTBJG,'0.00'  AS WDDCL "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " AAA_TBD LEFT JOIN AAA_LJQDQZTXXB ON AAA_LJQDQZTXXB.HTQX=AAA_TBD.HTQX AND AAA_LJQDQZTXXB.SPBH=AAA_TBD.SPBH  ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " AAA_TBD.Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " isnull(AAA_LJQDQZTXXB.SFJRLJQ,'否') ='否' and ZT='竞标' AND AAA_TBD.MJJSBH='" + ht_tiaojian["卖方编号"].ToString ()+ "' ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " AAA_TBD.Number ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
               DataSet ds_old = (DataSet)re_ds[1];
               if (ds_old != null && ds_old.Tables.Contains("主要数据"))
               {
                   ds_res = TBDGL_Bqu(ds_old);
               }
               else
               {
                   ds_res = ds_old.Copy();
               }
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 商品买卖b区中的投标单管理
    /// </summary>
    /// <param name="dsold">待处理的数据集</param>
    /// <returns>处理完成后的数据集</returns>
    private DataSet TBDGL_Bqu(DataSet dsold)
    {
        DataSet dsreturn = dsold;
        Hashtable input = new Hashtable();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        try
        {
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            {
                #region 获取当前最低价、达成率
                //获取当前商品的卖家最低价及相关数据
                input["@spbh"] = dsreturn.Tables[0].Rows[i]["SPBH"].ToString();
                input["@htqx"] = dsreturn.Tables[0].Rows[i]["HTQX"].ToString();
                string sql_lowprice = "select top 1 number as 投标单号, TBJG as 投标价格,TBNSL as 最低价投标拟售量,YZBSL as 已中标数量,GHQY as 供货区域  from AAA_TBD where ZT='竞标' and SPBH=@spbh and HTQX=@htqx  order by TBJG,TJSJ";
                Hashtable ht_lowprice = I_DBL.RunParam_SQL(sql_lowprice, "数据", input);
                if ((bool)ht_lowprice["return_float"])
                {
                    DataSet ds_lowprice = (DataSet)ht_lowprice["return_ds"];
                    if (ds_lowprice != null && ds_lowprice.Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables[0].Rows[i]["ZDTBJG"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                        //即时合同的达成率和三个月/一年的计算公式不同
                        if (dsreturn.Tables[0].Rows[i]["HTQX"].ToString() == "即时")
                        {
                            dsreturn.Tables[0].Rows[i]["XTDCL"] = (Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["已中标数量"].ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                        }
                        else
                        {
                            input["@tbjg"] = ds_lowprice.Tables[0].Rows[0]["投标价格"].ToString();
                            input["@ghqy"] = ds_lowprice.Tables[0].Rows[0]["供货区域"].ToString();
                            //获取最低价标的拟购买数量和
                            string sql = "select isnull(sum(NDGSL-YZBSL),0) as 拟订购数量 from AAA_YDDXXB where SPBH=@spbh and  HTQX=@htqx and ZT = '竞标' and NMRJG >=convert(decimal(18,2),@tbjg) and 0<CHARINDEX ('|'+SHQYsheng+'|',isnull(@ghqy,''))";
                            Hashtable ht_ndgsl = I_DBL.RunParam_SQL(sql, "数据", input);
                            if ((bool)ht_ndgsl["return_float"])
                            {
                                DataSet ds_ndgsl = (DataSet)ht_ndgsl["return_ds"];
                                if (ds_ndgsl != null && ds_ndgsl.Tables[0].Rows.Count > 0)
                                {
                                    dsreturn.Tables[0].Rows[i]["XTDCL"] = (Convert.ToDouble(ds_ndgsl.Tables[0].Rows[0]["拟订购数量"].ToString()) / Convert.ToDouble(ds_lowprice.Tables[0].Rows[0]["最低价投标拟售量"].ToString()) * 100.00).ToString("#0.00");
                                }
                            }
                        }
                        //即时/三个月/一年的合同，只有当前投标单为作为计算基础的最低价投标单时，才有我的标的达成率，其他的都为0.
                        if (dsreturn.Tables[0].Rows[i]["Number"].ToString() == ds_lowprice.Tables[0].Rows[0]["投标单号"].ToString())
                        {
                            dsreturn.Tables[0].Rows[i]["WDDCL"] = dsreturn.Tables[0].Rows[i]["XTDCL"].ToString();
                        }
                    }

                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }
        return dsreturn;
    }

    /// <summary>
    /// 商品买卖B区预订单管理
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqYCTBD(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            DataSet ds_res = new DataSet();//待返回的结果集 

            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " * "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " (select a.FWZXSHSJ,b.Number 投标单号,b.SPBH 商品编号,b.SPMC 商品名称,b.GG 规格,b.ZT 投标单状态,b.HTQX 合同期限,a.JYGLBSHWTGHSFXG 是否修改,isnull(convert(varchar(20),a.MJZXXGSJ,120),'--') 最后修改时间 from AAA_TBZLSHB a left join AAA_TBD b on a.TBDH=b.Number where b.DLYX='" + ht_tiaojian["用户邮箱"].ToString () + "' and a.FWZXSHZT='审核异常' and a.JYGLBSHZT<>'审核通过') AS Table1  ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " 投标单号 ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " FWZXSHSJ ";  //用于排序的字段(必须设置)   

            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and 商品名称 like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }

            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                ds_res = (DataSet)re_ds[1];
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 商品买卖B区选择商品页面获取分页数据
    /// </summary>
    /// <param name="ds_page">大分页参数集</param>
    /// <param name="ht_tiaojian">查询条件</param>
    /// <returns></returns>
    public DataSet GetBqXZSP(DataSet ds_page, Hashtable ht_tiaojian)
    {
        try
        {
            if (!ht_tiaojian.Contains("用户邮箱"))
            {//如果传入的参数不包括这几项，说明有问题，直接返回null

                return null;
            }

            DataSet ds_res = new DataSet();//待返回的结果集   
            string tableName = "";
            switch (ht_tiaojian["商品类型"].ToString())
            {
                case "自选商品":
                    tableName = " SELECT a.Number,a.SPBH,b.SPMC,b.GG,b.JJDW,b.JJPL,b.SCZZYQ,(CAST(RIGHT(a.SPBH,4) AS int)) AS 右边编号, CAST(REPLACE(LEFT(a.SPBH,2),'L','') AS int) AS 左边编号 FROM AAA_ZXSPJLB as a INNER JOIN AAA_PTSPXXB as b ON b.SPBH=a.SPBH WHERE a.DLYX='" + ht_tiaojian["用户邮箱"].ToString() + "' AND b.SFYX='是' ";//自选商品
                    break;
                case "所有商品":
                    tableName = " SELECT Number, SPBH,SPMC,GG,JJDW,JJPL,SCZZYQ,(CAST(RIGHT(SPBH,4) AS int)) AS 右边编号, CAST(REPLACE(LEFT(SPBH,2),'L','') AS int) AS 左边编号 FROM AAA_PTSPXXB  WHERE SFYX='是'  ";
                    break;
                default:
                    break;
            }

            //设置分页功能需要的参数
            //dt_page.Rows[0]["this_dblink"] = "";//一般不用填，默认webconfig中的mainsqlserver。有特殊情况需连接别的数据库时使用，填写webconfig中添加的数据库链接的“name”值。
            // ds_page.Tables[0].Rows[0]["GetCustomersDataPage_NAME"] = " GetCustomersDataPage2 ";  //使用的存储过程名称,不设置的时候默认为GetCustomersDataPage。
            ds_page.Tables[0].Rows[0]["serach_Row_str"] = " Number,SPBH,SPMC,GG,JJDW,JJPL,SCZZYQ,右边编号,左边编号,0.00 as ZDJGJS,0.00 as ZDJGSGY,0.00 as ZDJGYN,0.00 as MJJJPLJS,0.00 as MJJJPLSGY,0.00 as MJJJPLYN,'' as ZLBZYZM,'' as CPJCBG,'' as FDDBRCNS,'' as SHFWGDYCN,'' as CPSJSQS,'' as PGZFZRFLCNS,'' as SLZM "; //检索字段(必须设置)
            ds_page.Tables[0].Rows[0]["search_tbname"] = " (" + tableName + ") as TABLE1 ";  //检索的表(必须设置)
            ds_page.Tables[0].Rows[0]["search_mainid"] = " Number ";  //所检索表的主键(必须设置)
            ds_page.Tables[0].Rows[0]["search_str_where"] = " 1=1 ";  //检索条件(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixu"] = " ASC ";  //排序方式(必须设置)
            ds_page.Tables[0].Rows[0]["search_paixuZD"] = " 左边编号 ASC,右边编号 ";  //用于排序的字段(必须设置)  

           
            //使用ht_tiaojian 传入的参数拼出需要增加的条件
            if (ht_tiaojian["商品名称"].ToString().Trim() != "")
            {
                ds_page.Tables[0].Rows[0]["search_str_where"] += " and SPMC like '%" + ht_tiaojian["商品名称"].ToString() + "%' ";
            }
          
            //调用执行方法获取数据
            object[] re_ds = IPC.Call("分页数据获取", new object[] { ds_page });
            if (re_ds[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
               DataSet ds_old = (DataSet)re_ds[1];
               if (ds_old != null && ds_old.Tables.Contains("主要数据"))
               {//二次处理获取其他字段
                   ds_res = Bq_XZSP(ds_old, ht_tiaojian["用户邮箱"].ToString());
               }
               else
               {
                   ds_res = ds_old.Copy();
               }
            }
            else
            {
                ds_res = null;
            }
            return ds_res;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 选择商品页面数据二次处理
    /// </summary>
    /// <param name="ds_old">分页获取的原始数据集</param>
    /// <returns></returns>
    private DataSet Bq_XZSP(DataSet dsold,string dlyx)
    {
        DataSet dsreturn = dsold;
        Hashtable input = new Hashtable();
        input["@dlyx"]=dlyx;
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        try
        {
            for (int i = 0; i < dsreturn.Tables[0].Rows.Count; i++)
            { 

               input["@spbh"] = dsreturn.Tables[0].Rows[i]["SPBH"].ToString();
               
                //获取当前商品的“即时”、“三个月”、“一年”的最低价和最大经济批量
                string sql_tbd = "select htqx as 合同期限,min(tbjg) as 最低价,max(MJSDJJPL) as 最大经济批量 from AAA_TBD where spbh=@spbh and zt='竞标' group by htqx";
                Hashtable ht_tbd = I_DBL.RunParam_SQL(sql_tbd, "数据", input);
                if ((bool)ht_tbd["return_float"])
                {
                    DataSet ds_tbd = (DataSet)ht_tbd["return_ds"];
                    if (ds_tbd != null && ds_tbd.Tables[0].Rows.Count > 0)
                    { 
                        //即时
                        DataRow[] dr_js = ds_tbd.Tables[0].Select("合同期限='即时'");
                        if (dr_js.Length > 0)
                        {
                            dsreturn.Tables[0].Rows[i]["ZDJGJS"] = dr_js[0]["最低价"].ToString();
                            dsreturn.Tables[0].Rows[i]["MJJJPLJS"] = dr_js[0]["最大经济批量"].ToString();
                        }
                        //三个月
                        DataRow[] dr_sgy = ds_tbd.Tables[0].Select("合同期限='三个月'");
                        if (dr_sgy.Length > 0)
                        {
                            dsreturn.Tables[0].Rows[i]["ZDJGSGY"] = dr_sgy[0]["最低价"].ToString();
                            dsreturn.Tables[0].Rows[i]["MJJJPLSGY"] = dr_sgy[0]["最大经济批量"].ToString();
                        }
                        //一年
                        DataRow[] dr_yn = ds_tbd.Tables[0].Select("合同期限='一年'");
                        if (dr_yn.Length > 0)
                        {
                            dsreturn.Tables[0].Rows[i]["ZDJGYN"] = dr_yn[0]["最低价"].ToString();
                            dsreturn.Tables[0].Rows[i]["MJJJPLYN"] = dr_yn[0]["最大经济批量"].ToString();
                        }
                    }
                }

                //获取当前交易方最后一次该商品投标单的资质文件
                string sql_zizhi = "select top 1 ZLBZYZM, CPJCBG, FDDBRCNS, SHFWGDYCN, CPSJSQS, SLZM, createtime, PGZFZRFLCNS from AAA_TBD where SPBH=@spbh and DLYX=@dlyx order by createtime desc ";
                Hashtable ht_zizhi = I_DBL.RunParam_SQL(sql_zizhi, "data", input);
                if ((bool)ht_zizhi["return_float"])
                {
                    DataSet ds_zizhi = (DataSet)ht_zizhi["return_ds"];
                    if (ds_zizhi != null && ds_zizhi.Tables[0].Rows.Count > 0)
                    {
                        dsreturn.Tables[0].Rows[i]["ZLBZYZM"] = ds_zizhi.Tables[0].Rows[0]["ZLBZYZM"].ToString();
                        dsreturn.Tables[0].Rows[i]["CPJCBG"] = ds_zizhi.Tables[0].Rows[0]["CPJCBG"].ToString();
                        dsreturn.Tables[0].Rows[i]["FDDBRCNS"] = ds_zizhi.Tables[0].Rows[0]["FDDBRCNS"].ToString();
                        dsreturn.Tables[0].Rows[i]["SHFWGDYCN"] = ds_zizhi.Tables[0].Rows[0]["SHFWGDYCN"].ToString();
                        dsreturn.Tables[0].Rows[i]["CPSJSQS"] = ds_zizhi.Tables[0].Rows[0]["CPSJSQS"].ToString();
                        dsreturn.Tables[0].Rows[i]["PGZFZRFLCNS"] = ds_zizhi.Tables[0].Rows[0]["PGZFZRFLCNS"].ToString();
                        dsreturn.Tables[0].Rows[i]["SLZM"] = ds_zizhi.Tables[0].Rows[0]["SLZM"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //这里发生了错误，把错误输出
            DataTable objTable = new DataTable("二次处理错误");
            objTable.Columns.Add("执行错误", typeof(string));
            dsreturn.Tables.Add(objTable);
            dsreturn.Tables["二次处理错误"].Rows.Add(new object[] { ex.ToString() });
        }
        return dsreturn;
    }

}