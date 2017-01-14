
/************************************************
 * 
 * 创建人：zhaoxh
 * 
 * 创建时间：2014.5.22
 * 
 * 代码功能：成交处理中心功能。
 * 
 * 参考文档：无
 * 
 */
using FMDBHelperClass;
using FMipcClass;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

/// <summary>
/// ChengjiaoService 的摘要说明
/// </summary>
public class ChengjiaoService
{
	public ChengjiaoService()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    
    static string str = "";
    static DataSet dstx = new DataSet();
    //static DataTable dttx = null;


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

    private static DataTable InitDttx()
    {


        DataTable DTTX = new DataTable("dttx");
        DataColumn Dc = new DataColumn("投标单号+电子购货合同编号"); //设为主键  
        DTTX.Columns.Add(Dc);

        DataColumn Dc1 = new DataColumn("预订单+电子购货合同编号");
        DTTX.Columns.Add(Dc1);

        DataColumn dc2 = new DataColumn("商品编号+合同期限");
        DTTX.Columns.Add(dc2);

        DataColumn dc3 = new DataColumn("投标单提醒对象登陆邮箱");
        DTTX.Columns.Add(dc3);

        DataColumn dc8 = new DataColumn("预订单提醒对象登陆邮箱");
        DTTX.Columns.Add(dc8);

        DataColumn dc4 = new DataColumn("投标单提醒对象结算账户类型");
        DTTX.Columns.Add(dc4);

        DataColumn dc9 = new DataColumn("预订单提醒对象结算账户类型");
        DTTX.Columns.Add(dc9);

        DataColumn dc5 = new DataColumn("投标单提醒对象角色编号");
        DTTX.Columns.Add(dc5);

        DataColumn dc10 = new DataColumn("预订单提醒对象角色编号");
        DTTX.Columns.Add(dc10);


        DataColumn dc11 = new DataColumn("YDD是否全中");
        DTTX.Columns.Add(dc11);
       // htmess["YDD是否全中"] = "否";
        DTTX.Clear();
        DTTX.PrimaryKey = new DataColumn[] { DTTX.Columns["投标单号+电子购货合同编号"] };

        return DTTX;

       
    }

   
    /// <summary>
    /// 定标 zhaoxh 2014.5.22
    /// </summary>
    /// <param name="DataTable">dt: 登陆邮箱/结算账户类型/卖家角色编号/投标单号/Number/合同期限</param>
    /// <returns>成功/失败信息</returns>
    public static DataSet Jhjx_DB(DataTable dt)
    {
        //定义变量
        ArrayList al = new ArrayList();
        DataRow dr = dt.Rows[0];
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable param = new Hashtable();
        DataSet rtds = ReturnDt();
        rtds.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        //验证金额是否足够 
        string strwhere = " and  T_YSTBDBH='" + dr["投标单号"].ToString().Trim() + "' ";
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
            rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "请确认中标定标信息表内存在单号为：" + dr["Number"].ToString().Trim() + "的已中标的，且未冻结履约保证金的数据存在！";
            return rtds;
        }
        string Strdjinfo = "select  T_YSTBDBH  ,Z_SPBH , Z_SPMC ,Z_GG , T_YSTBDDJTBBZJ ,T_YSTBDTBNSL ,Z_ZBSL,Z_ZBJG ,Z_ZBJE,Z_ZBSJ , Z_LYBZJJE,Y_YSYDDDLYX,Y_YSYDDMJJSBH,Y_YSYDDBH ,Z_HTBH,Z_HTQX,Y_YSYDDJSZHLX,Y_YSYDDMJJSBH,Z_HTQX from AAA_ZBDBXXB where  Z_HTZT = '中标' and  Z_SFDJLYBZJ='否' " + strwhere;        
        Hashtable dsobjht = I_DBL.RunProc(Strdjinfo, "");
        DataSet dsobj = null;
        if ((bool)dsobjht["return_float"])
        {
            dsobj = (DataSet)dsobjht["return_ds"];
        }
        else
        {
            rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "数据查询报错！";
            return rtds;
        }
        // if(ds)
        double doutbbzj = 0.00;
        double doulvbzj = 0.00;
        double douchane = 0.00;
        //执行完成
        if ((bool)(dsobjht["return_float"]))
        {
            if (dsobj != null && dsobj.Tables[0].Rows.Count > 0)
            {
                doutbbzj = Convert.ToDouble(dsobj.Tables[0].Rows[0]["T_YSTBDDJTBBZJ"].ToString().Trim());
                // HTQX = dsobj.Tables[0].Rows[0]["Z_HTQX"].ToString().Trim();

                foreach (DataRow drobj in dsobj.Tables[0].Rows)
                {

                    doulvbzj += Convert.ToDouble(drobj["Z_LYBZJJE"].ToString().Trim());
                }
                Double douye = 0;
                /******************************************************************
                 *获取账户可用余额
                 *Double douye = cm.GetMoneyT(dr["登陆邮箱"].ToString(), "");
                 *****************************************************************/
                /****************************************************************/
                //原调用方法:Keynumber =PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "")
                object[] re = IPC.Call("账户当前可用余额", new object[] { dr["登陆邮箱"].ToString() });
                if (re[0].ToString() == "ok")
                {
                    if (re[1] == null)
                    {
                        rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "获取表（AAA_ZKLSMXB）主键失败";
                        return rtds;
                    }
                    string reFF = (string)(re[1]);
                    douye = double.Parse(reFF);
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                    string reFF = re[1].ToString();
                    rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "IPC调用\"用户控制中心=>账户当前可用余额\"方法时报错";
                    return rtds;
                }
                /****************************************************************/
                if (douye + doutbbzj < doulvbzj)
                {
                    rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "您的结算账户可用余额为" + douye.ToString() + "元，差额" + (doulvbzj - (douye + doutbbzj)).ToString("0.00") + "元，\n\r履约保证金无法足额冻结，不能定标，请您增加账户资金。";
                    return rtds;
                }
                douchane = doutbbzj - doulvbzj;

            }
            else
            {
                rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "请确认原始投标单号为" + dr["投标单号"].ToString().Trim() + "的投标单已中标，且单据状态正常！";
                return rtds;
            }
        }
        else
        {
            rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "数据查询报错！";
            return rtds;
        }


        //首次定标时的扣款提示
        string action = dr["ispass"].ToString().Trim();
        if (action == "no")
        {
            rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "pass";
            rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "您将冻结" + doulvbzj.ToString("0.00") + "元的履约保证金予以定标。定标后，\n\r提货单功能自动开通，平台同时代所有买方向您开具定标金额5%的保证函，您对应的投标保证金即予解冻。";
            return rtds;
        }
        //定标+
        if (action == "ok")
        {

            string Strtb = " select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from AAA_moneyDZB where Number = '1304000005'";
            
            Hashtable dsht = I_DBL.RunProc(Strtb, "");
            DataSet ds = null;
            if ((bool)dsht["return_float"])
            {
                ds = (DataSet)dsht["return_ds"];
            }
            else
            {
                rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "数据查询报错！";
                return rtds;
            }
            //执行完成
            if ((bool)(dsht["return_float"]))
            {
                if (ds != null && ds.Tables[0].Rows.Count == 1 && dsobj != null && dsobj.Tables[0].Rows.Count > 0)
                {

                    DataRow drjeid = ds.Tables[0].Rows[0];
                    drjeid["ZY"] = drjeid["ZY"].ToString().Trim().Replace("[x1]", dr["投标单号"].ToString().Trim());

                    DateTime drnow = DateTime.Now;

                    string Keynumber = "";
                    /****************************************************************/
                    //原调用方法:Keynumber =PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "")
                    object[] re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });
                    if (re[0].ToString() == "ok")
                    {
                        if (re[1] == null)
                        {
                            rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "获取表（AAA_ZKLSMXB）主键失败";
                            return rtds;
                        }
                        string reFF = (string)(re[1]);
                        Keynumber = reFF;
                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                        string reFF = re[1].ToString();
                        rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "IPC调用\"获取主键\"方法时报错";
                        return rtds;
                    }
                    /****************************************************************/
                    //定标解冻投标保证金
                    string strinserttbbbj = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + dr["登陆邮箱"].ToString() + "','" + dr["结算账户类型"].ToString() + "','" + dr["卖家角色编号"].ToString() + "','AAA_TBD','" + dr["投标单号"].ToString() + "','" + drnow + "','" + drjeid["YSLX"].ToString() + "','" + dsobj.Tables[0].Rows[0]["T_YSTBDDJTBBZJ"].ToString().Trim() + "','" + drjeid["XM"].ToString() + "','" + drjeid["XZ"].ToString() + "','" + drjeid["ZY"].ToString() + "','" + drjeid["SJLX"].ToString() + "','" + dr["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";

                    al.Add(strinserttbbbj);

                    //冻结履约保证金
                    string Strlv = " select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000047'";
                                        
                    Hashtable dsdjht = I_DBL.RunProc(Strlv, "");
                    DataSet dsdj = null;
                    if ((bool)dsdjht["return_float"])
                    {
                        dsdj = (DataSet)dsdjht["return_ds"];
                    }
                    else
                    {
                        rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "数据查询报错！";
                        return rtds;
                    }
                    //执行完成
                    if ((bool)(dsdjht["return_float"]))
                    {
                        if (dsdj != null && dsdj.Tables[0].Rows.Count == 1)
                        {
                            DataRow drlvy = dsdj.Tables[0].Rows[0];
                            string strzy = drlvy["ZY"].ToString();
                            if (dsobj != null && dsobj.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsobj.Tables[0].Rows.Count; i++)
                                {
                                    /****************************************************************/
                                    //原调用方法:Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    object[] re2 = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });
                                    if (re2[0].ToString() == "ok")
                                    {
                                        if (re2[1] == null)
                                        {
                                            rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "获取表（AAA_ZKLSMXB）主键失败";
                                            return rtds;
                                        }
                                        string reFF = (string)(re2[1]);
                                        Keynumber = reFF;
                                    }
                                    else
                                    {
                                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                                        string reFF = re2[1].ToString();
                                        rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "IPC调用\"获取主键\"方法时报错";
                                        return rtds;
                                    }
                                    /****************************************************************/
                                    drlvy["ZY"] = strzy.ToString().Trim().Replace("[x1]", dsobj.Tables[0].Rows[i]["Z_HTBH"].ToString().Trim());
                                    string Strinsetlvbzj = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + dr["登陆邮箱"].ToString() + "','" + dr["结算账户类型"].ToString() + "','" + dr["卖家角色编号"].ToString() + "','AAA_TBD','" + dr["投标单号"].ToString() + "','" + drnow + "','" + drlvy["YSLX"].ToString() + "','" + dsobj.Tables[0].Rows[i]["Z_LYBZJJE"].ToString().Trim() + "','" + drlvy["XM"].ToString() + "','" + drlvy["XZ"].ToString() + "','" + drlvy["ZY"].ToString() + "','" + drlvy["SJLX"].ToString() + "','" + dr["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";

                                    al.Add(Strinsetlvbzj);
                                }
                            }
                            else
                            {
                                rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "履约保证金流水信息插入有错！";
                                return rtds;
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
                            //插入日志：定标关键语句
                            FMipcClass.Log.WorkLog("定标", Log.type.other, FMipcClass.Log.ArrayListSQL2string(al), null);

                            Hashtable dsdjht2 = I_DBL.RunParam_SQL(al);
                           
                            //执行完成 err
                            if (!(bool)(dsdjht2["return_float"]))
                            {
                                rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "数据查询报错！";
                                //插入日志：定标语句执行失败
                                return rtds;
                            }
                            
                            //发送提醒
                            string StrMC = "";
                            //object obj = null;
                            for (int i = 0; i < dsobj.Tables[0].Rows.Count; i++)
                            {
                                DataSet dsmes = new DataSet();
                                DataTable dtmes = new DataTable();
                                dtmes.Columns.Add(new DataColumn("type", typeof(string)));
                                dtmes.Columns.Add(new DataColumn("提醒对象登陆邮箱", typeof(string)));
                                dtmes.Columns.Add(new DataColumn("提醒对象结算账户类型", typeof(string)));
                                dtmes.Columns.Add(new DataColumn("提醒对象角色编号", typeof(string)));
                                dtmes.Columns.Add(new DataColumn("提醒对象角色类型", typeof(string)));
                                //dtmes.Columns.Add(new DataColumn("提醒对象用户名", typeof(string)));
                                dtmes.Columns.Add(new DataColumn("提醒内容文本", typeof(string)));
                                dtmes.Columns.Add(new DataColumn("创建人", typeof(string)));
                                dtmes.Columns.Add(new DataColumn("查看地址", typeof(string)));
                                dtmes.Rows.Add(dtmes.NewRow());
                                dsmes.Tables.Add(dtmes);


                                dtmes.Rows[0]["type"] = "集合经销平台";

                                dtmes.Rows[0]["提醒对象登陆邮箱"] = dsobj.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString().Trim();
                                //dtmes.Rows[0]["提醒对象用户名"] = "";
                                //获取交易方名称 
                                StrMC = "  select I_JYFMC from  AAA_DLZHXXB  where B_DLYX = '" + dsobj.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString().Trim() + "' ";

                                Hashtable dsobj3ht = I_DBL.RunProc(StrMC, "");
                                DataSet dsobj3 = (DataSet)dsobj3ht["return_ds"];
                                
                                //执行完成 err
                                if ((bool)(dsobj3ht["return_float"]))
                                {
                                    //if (dsobj3 != null && dsobj3.Tables.Count > 0 && dsobj3.Tables[0].Rows.Count>0)
                                    //{
                                    //    obj = dsobj3.Tables[0].Rows[0][0];
                                    //    //dtmes.Rows[0]["提醒对象用户名"] = obj.ToString();
                                    //}
                                    //dsobj.Tables[0].Rows[0]["Y_YSYDDDLYX"].ToString().Trim();
                                    dtmes.Rows[0]["提醒对象结算账户类型"] = dsobj.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString().Trim();
                                    dtmes.Rows[0]["提醒对象角色编号"] = dsobj.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString().Trim();
                                    dtmes.Rows[0]["提醒对象角色类型"] = "买家";//dsobj.Tables[0].Rows[0]["Y_YSYDDDLYX"].ToString().Trim();

                                    dtmes.Rows[0]["提醒内容文本"] = "您" + dsobj.Tables[0].Rows[i]["Z_SPMC"].ToString().Trim() + "商品的" + dsobj.Tables[0].Rows[i]["Y_YSYDDBH"].ToString().Trim() + "预订单已定标，提货单功能已开通，平台已经代您向卖方开具保证函，请您按照" + dsobj.Tables[0].Rows[i]["Z_HTBH"].ToString() + "《电子购货合同》约定执行。";
                                    dtmes.Rows[0]["创建人"] = dr["登陆邮箱"].ToString();
                                    

                                    /****************************************************************/
                                    //原调用方法:PublicClass2013.Sendmes(lth);
                                    object[] re3 = IPC.Call("发送提醒", new object[] { dsmes });
                                    if (re3[0].ToString() == "ok")
                                    {
                                        if (re3[1] == null)
                                        {
                                            rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "发送提醒失败";
                                            return rtds;
                                        }
                                        string reFF = (string)(re3[1]);
                                        StrMC = reFF;
                                    }
                                    else
                                    {
                                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                                        string reFF = re[1].ToString();
                                        rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "IPC调用\"发送提醒\"方法时报错";
                                        return rtds;
                                    }
                                    /****************************************************************/
                                }
                                else
                                {
                                    rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "数据查询报错！";
                                    return rtds;
                                }

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

                            rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "您" + doulvbzj.ToString("0.00") + "元履约保证金冻结成功，\n\r" + dr["投标单号"].ToString().Trim() + "投标单已定标，" + doutbbzj.ToString("0.00") + "元的投标保证金已解冻。";
                            //插入日志：定标成功
                            return rtds;


                        }
                        else
                        {
                            rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "冻结履约保证金是出错！";
                            return rtds;
                        }
                    }
                    else
                    {
                        rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "数据查询报错！";
                        return rtds;
                    }

                }
                else
                {
                    rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "没有任何数据被更新！";
                    return rtds;
                }
            }
            else
            {
                rtds.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                rtds.Tables["返回值单条"].Rows[0]["提示文本"] = "数据查询报错！";
                return rtds;
            }
        }
        return rtds;
    }

    /// <summary>
    /// 处理成交规则业务逻辑 zhaoxh 2014.5.27
    /// 查看是否在冷静期，查看是否能够凑齐，根据调用点进行不同的处理
    /// 传入的数据表，必须包含至少两列“商品编号”“合同期限”，且一定不能有重复数据
    /// 调用点包括： 提交、删除、修改、中标监控
    /// </summary>
    /// <param name="dtRun">包含至少两列“商品编号”“合同期限”的DataTable</param>
    /// <param name="sp">调用点： 提交、删除、修改、中标监控</param>
    /// <returns>数据结果集</returns>
    public static DataSet RunCJGZ(DataTable dtRun, string sp)
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
        string sql_dsTB = "";
        for (int i = 0; i < dtRun.Rows.Count; i++)
        {
            param["@SPBH" + i.ToString()] = dtRun.Rows[i]["商品编号"].ToString();
            param["@HTQX" + i.ToString()] = dtRun.Rows[i]["合同期限"].ToString();
            string f = "";
            if (i != 0)
            {
                f = " union all ";
            }

            //按照价格从小到大，时间从晚到早找出top1
            sql_dsTB = sql_dsTB + f + " select * from ( select top 1 ZZZ.Number,ZZZ.DLYX,ZZZ.JSZHLX,ZZZ.MJJSBH,ZZZ.GLJJRYX,ZZZ.GLJJRYHM,ZZZ.GLJJRJSBH,ZZZ.GLJJRPTGLJG,ZZZ.SPBH,ZZZ.SPMC,ZZZ.GG,ZZZ.JJDW,ZZZ.PTSDZDJJPL,ZZZ.HTQX,ZZZ.MJSDJJPL,ZZZ.GHQY,ZZZ.TBNSL,ZZZ.TBJG,ZZZ.TBJE,ZZZ.MJTBBZJBL,ZZZ.MJTBBZJZXZ,ZZZ.DJTBBZJ,ZZZ.ZT,ZZZ.TJSJ,ZZZ.CXSJ,ZZZ.ZLBZYZM,ZZZ.CPJCBG,ZZZ.PGZFZRFLCNS,ZZZ.FDDBRCNS,ZZZ.SHFWGDYCN,ZZZ.CPSJSQS,ZZZ.CreateTime,LLL.LJQKSSJ,LLL.LJQJSSJ,LLL.SFJRLJQ,LLL.LJQYZBS,'是否凑齐'='否','成交量'=0,'是否在冷静期内'=isnull(LLL.SFJRLJQ,'否') from AAA_TBD as ZZZ left join AAA_LJQDQZTXXB as LLL on LLL.SPBH=ZZZ.SPBH and LLL.HTQX=ZZZ.HTQX where ZZZ.SPBH = @SPBH" + i.ToString() + " and  ZZZ.HTQX = @HTQX" + i.ToString() + " and  ZZZ.ZT='竞标' AND ZZZ.HTQX <> '即时'  order  by ZZZ.TBJG asc,ZZZ.TJSJ asc ) as tab" + i.ToString() + "   ";//AND ZZZ.HTQX <> '即时'
        }
        

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


        #region//动态参数

        DataRow htParameterInfo = null;
        object[] re = IPC.Call("平台动态参数", new object[] { "" });//-----------
       
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (((DataSet)(re[1])) != null && ((DataSet)(re[1])).Tables[0].Rows.Count > 0)
            {
                htParameterInfo = ((DataSet)(re[1])).Tables[0].Rows[0];
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
        #endregion

       
        Hashtable htinput = null;
       

        #region//遍历要处理的投标信息
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



            #region//若是中标监控，先给这个商品加锁，避免再发布投标信息和预订单。(这个其实也不能写到这里，要写到中标监控获取待处理商品的地方。)

            if (sp == "中标监控")
            {
                htinput = new Hashtable();
                htinput["@SPBH"] = SPBH;
                htinput["@HTQX"] = HTQX;
                // DbHelperSQL.ExecuteSql("update AAA_LJQDQZTXXB set LJQYZBS = '已锁' where SPBH='" + SPBH + "' and HTQX='" + HTQX + "'");
                return_ht = I_DBL.RunParam_SQL("update AAA_LJQDQZTXXB set LJQYZBS = '已锁' where SPBH=@SPBH and HTQX=@HTQX", htinput);
                if ((bool)(return_ht["return_float"])) //说明执行完成
                {
                    //这里就可以用了。
                    int a = (int)(return_ht["return_other"]);
                    if (a != 1)
                    {
                        return null;
                    }
                }
                else
                {
                    throw ((Exception)return_ht["return_errmsg"]);
                    //return null;
                } //?

            }
            #endregion

            #region//临时记录的语句，如果最后为凑齐，则这些语句都无效
            ArrayList alSQLtemp = new ArrayList();

            Int64 CQsl = 0; //凑齐数量
            //看某个投标信息能不能凑齐
            htinput = new Hashtable();
            htinput["@TBJG"] = TBJG;
            htinput["@GHQY"] = GHQY;
            htinput["@HTQX"] = HTQX;
            htinput["@SPBH"] = SPBH;
            return_ht = I_DBL.RunParam_SQL(" select *,'本次成交'=0,'多余量'=0  from AAA_YDDXXB where HTQX=@HTQX and ZT='竞标' and SPBH  =@SPBH  and NMRJG >= @TBJG and charindex('|'+SHQYsheng+'|',@GHQY)>0   order  by CreateTime asc  ","", htinput);
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
                Hashtable htall = null;
                lh = new List<Hashtable>();
                Hashtable htmess = null;
                #region//遍历预订单信息表
                for (int y = 0; y < dsYDD.Tables[0].Rows.Count; y++)
                {
                    CQsl = CQsl + (Convert.ToInt64(dsYDD.Tables[0].Rows[y]["NDGSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]));

                    #region//上限和下限
                    Int64 min = Convert.ToInt64(Math.Round(TBsl * 0.9));
                    Int64 max = Convert.ToInt64(Math.Round(TBsl * 1.1));
                    #endregion

                    #region//凑到了大于预售量
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
                            dsYDD.Tables[0].Rows[y]["多余量"] = 0;
                            dsYDD.Tables[0].Rows[y]["本次成交"] = Convert.ToInt64(dsYDD.Tables[0].Rows[y]["NDGSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]);
                        }

                        #region //监控专用，写入中标定标信息表，并反写预订单
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

                            string Number = "";//jhjx_PublicClass.GetNextNumberZZ("AAA_ZBDBXXB", LMstr);
                            re = IPC.Call("获取主键", new object[] { "AAA_ZBDBXXB", LMstr });//--------------------

                            if (re[0].ToString() == "ok")
                            {
                                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                if (re[1] != null && !re[1].ToString().Trim().Equals(""))
                                {
                                    Number = (string)(re[1]);
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
                            //提醒：  放入临时变量。
                            //本次插入数据中涉及的投标单号+电子购货合同编号，预订单+电子购货合同编号。分别记录。。 A
                            //本次插入数据中涉及的商品编号+合同期限， 记录。   B

                           


                            //DataRow drtx = DTTX.NewRow();
                            htmess = new Hashtable();
                            htmess["投标单号+电子购货合同编号"] = T_YSTBDBH.Trim() + "*" + Z_HTBH.Trim();
                            htmess["预订单+电子购货合同编号"] = Y_YSYDDBH.Trim() + "*" + Z_HTBH.Trim();
                            htmess["商品编号+合同期限"] = Z_SPBH.Trim() + "*" + Z_HTQX.Trim() + "*" + Z_SPMC.Trim();
                            htmess["投标单提醒对象登陆邮箱"] = T_YSTBDDLYX.Trim();
                            htmess["预订单提醒对象登陆邮箱"] = Y_YSYDDDLYX.Trim();
                            htmess["投标单提醒对象结算账户类型"] = T_YSTBDJSZHLX.Trim();
                            htmess["预订单提醒对象结算账户类型"] = Y_YSYDDJSZHLX.Trim();
                            htmess["投标单提醒对象角色编号"] = T_YSTBDMJJSBH;
                            htmess["预订单提醒对象角色编号"] = Y_YSYDDMJJSBH;
                            htmess["YDD是否全中"] = "否";
                           

                          

                            //反写预订单//不一定都写中标，未完全成交的，要写竞标。(TS)
                            string djdj_sqlstr = " ";  //订金特殊处理的sql字符串
                            if (ZTstr == "竞标")
                            {
                                //重新计算拟订购金额
                                double new_NDGJE = Math.Round((Convert.ToInt64(dsYDD.Tables[0].Rows[y]["NDGSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]) - Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"])) * Convert.ToDouble(dsYDD.Tables[0].Rows[y]["NMRJG"]), 2);
                                //“反写预订单订金金额”=原预订单原订金金额-写入中标定标信息表的订金金额
                                djdj_sqlstr = " ,DJDJ=DJDJ- " + Z_DJJE + " , NDGJE = " + new_NDGJE + " ";
                                htmess["YDD是否全中"] = "否";
                            }
                            if (ZTstr == "中标")
                            {
                                djdj_sqlstr = " ";
                                htmess["YDD是否全中"] = "是";
                            }

                            lh.Add(htmess);

                            //处理是否拆单标记
                            string cd_sqlstr = "  "; //拆单更新处理sql
                            //若是此预订单第一次中标 并且 部分中标
                            if (Convert.ToInt64(dsYDD.Tables[0].Rows[y]["多余量"]) != 0 && Convert.ToInt64(dsYDD.Tables[0].Rows[y]["YZBSL"]) == 0)
                            {
                                cd_sqlstr = " ,SFCD='是' ";
                            }

                            alSQLtemp.Add("update AAA_YDDXXB set ZT = '" + ZTstr + "',YZBSL=YZBSL+ " + Convert.ToInt64(dsYDD.Tables[0].Rows[y]["本次成交"]) + djdj_sqlstr + cd_sqlstr + "  where Number = '" + dsYDD.Tables[0].Rows[y]["Number"].ToString() + "' ");

                        }
                        #endregion
                        #region//若凑齐量超过了最大上限，强制退出这个for循环，只退出一层
                        if (CQsl > max)
                        {
                            y = dsYDD.Tables[0].Rows.Count + 10;
                        }
                        #endregion
                    }
                    #endregion
                    #region//未凑到预售量，继续凑，但如果是中标监控，先把语句生成好。
                    else
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
                            string Number = "";//jhjx_PublicClass.GetNextNumberZZ("AAA_ZBDBXXB", LMstr);

                            re = IPC.Call("获取主键", new object[] { "AAA_ZBDBXXB", LMstr });//--------------------

                            if (re[0].ToString() == "ok")
                            {
                                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                if (re[1] != null && !re[1].ToString().Trim().Equals(""))
                                {
                                    Number = (string)(re[1]);
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
                            //提醒：  放入临时变量。
                            //本次插入数据中涉及的投标单号+电子购货合同编号，预订单+电子购货合同编号。分别记录。。 A
                            //本次插入数据中涉及的商品编号+合同期限， 记录。   B
                            //DataRow drtx1 = DTTX.NewRow();
                            htmess = new Hashtable();
                            htmess["投标单号+电子购货合同编号"] = T_YSTBDBH.Trim() + "*" + Z_HTBH.Trim();
                            htmess["预订单+电子购货合同编号"] = Y_YSYDDBH.Trim() + "*" + Z_HTBH.Trim();
                            htmess["商品编号+合同期限"] = Z_SPBH.Trim() + "*" + Z_HTQX.Trim() + "*" + Z_SPMC.Trim();
                            htmess["投标单提醒对象登陆邮箱"] = T_YSTBDDLYX.Trim();
                            htmess["预订单提醒对象登陆邮箱"] = Y_YSYDDDLYX.Trim();
                            htmess["投标单提醒对象结算账户类型"] = T_YSTBDJSZHLX.Trim();
                            htmess["预订单提醒对象结算账户类型"] = Y_YSYDDJSZHLX.Trim();
                            htmess["投标单提醒对象角色编号"] = T_YSTBDMJJSBH;
                            htmess["预订单提醒对象角色编号"] = Y_YSYDDMJJSBH;
                            htmess["YDD是否全中"] = "是";
                            lh.Add(htmess);
                            //------------------直接全中

                          


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
                    #endregion
                } //预订单遍历结束
#endregion

                #region//凑够了
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
                        DataSet ds_jyfsl = null;
                        Hashtable htaa = new Hashtable();
                        htaa["@SPBH"] = SPBH;
                        htaa["@HTQX"] = HTQX;
                        return_ht = I_DBL.RunParam_SQL("select (select count(distinct MJJSBH) from AAA_TBD where ZT = '竞标' and SPBH =@SPBH and HTQX=@HTQX) as 卖方数量 , (select count(distinct MJJSBH) from AAA_YDDXXB where ZT = '竞标' and SPBH =@SPBH and HTQX=@HTQX) as 买方数量", "", htaa);

                        if ((bool)(return_ht["return_float"])) //说明执行完成
                        {
                            //这里就可以用了。
                            ds_jyfsl = (DataSet)(return_ht["return_ds"]);

                        }
                        else
                        {
                            //return null;
                            throw ((Exception)return_ht["return_errmsg"]);
                        }
                        // DataSet ds_jyfsl = DbHelperSQL.Query("select (select count(distinct MJJSBH) from AAA_TBD where ZT = '竞标' and SPBH ='" + SPBH + "' and HTQX='" + HTQX + "') as 卖方数量 , (select count(distinct MJJSBH) from AAA_YDDXXB where ZT = '竞标' and SPBH ='" + SPBH + "' and HTQX='" + HTQX + "') as 买方数量");
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


                        //提醒：  将A,B的内容，写入txt。
                        if(lh!=null && lh.Count>0)
                        {
                            DataRow drmes = null;//DTTX.NewRow();
                            foreach (Hashtable ht in lh)
                            {
                                drmes = dstx.Tables["dttx"].NewRow();
                                drmes["投标单号+电子购货合同编号"] = ht["投标单号+电子购货合同编号"].ToString();
                                drmes["预订单+电子购货合同编号"] = ht["预订单+电子购货合同编号"].ToString();
                                drmes["商品编号+合同期限"] = ht["商品编号+合同期限"].ToString();
                                drmes["投标单提醒对象登陆邮箱"] = ht["投标单提醒对象登陆邮箱"].ToString();
                                drmes["预订单提醒对象登陆邮箱"] = ht["预订单提醒对象登陆邮箱"].ToString();
                                drmes["投标单提醒对象结算账户类型"] = ht["投标单提醒对象结算账户类型"].ToString();
                                drmes["预订单提醒对象结算账户类型"] = ht["预订单提醒对象结算账户类型"].ToString();
                                drmes["投标单提醒对象角色编号"] = ht["投标单提醒对象角色编号"].ToString();
                                drmes["预订单提醒对象角色编号"] = ht["预订单提醒对象角色编号"].ToString();
                                drmes["YDD是否全中"] = ht["YDD是否全中"].ToString();
                                dstx.Tables["dttx"].Rows.Add(drmes);
   //dstx.Tables["1"].Rows.Add(drmes);
                            }

                           // DTTX.AcceptChanges();
                          
                        }
                        

                    }
                }
                #endregion
            }
            #endregion

            //无论找没找到能匹配的预订单，都进行一下冷静期的处理
            //看这个投标信息的商品是否进入了冷静期，对冷静期控制进行处理
            #region//在冷静期
            if (SFJRLJQ == "是") //在冷静期
            {
                //处理中标监控
                if (sp == "中标监控")
                {
                    //更新冷静期状态为否，时间置空
                    ALsql.Add(" update AAA_LJQDQZTXXB set SFJRLJQ = '否',LJQKSSJ = null,LJQJSSJ=null where  HTQX = '" + HTQX + "' and SPBH  = '" + SPBH + "' ");
                    //记录历史记录



                    string newnumber = ""; //jhjx_PublicClass.GetNextNumberZZ("AAA_LJQBGLSJLB", "");

                    re = IPC.Call("获取主键", new object[] { "AAA_LJQBGLSJLB", "" });//--------------------

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
                        throw((Exception)re[1]);
                    }
                    ALsql.Add(" INSERT INTO AAA_LJQBGLSJLB (Number,SPBH,HTQX,LJQKSSJBGW,LJQJSSJBGW,SFZLJQZTBGW) VALUES ('" + newnumber + "','" + SPBH + "','" + HTQX + "',null,null,'否') ");

                    ALsql[0] = ALsql[0].ToString() + "  select '有商品离开冷静期'   ";

                   
                    
                   
                  //  dstx.Tables.Add(DTTX);

                }
                else  //不是中标监控，不动冷静期
                {
                    ;
                }
            }
            #endregion
            #region//不在冷静期
            else //不在冷静期
            {
                if (dsTB.Tables[0].Rows[i]["是否凑齐"].ToString() == "是") //可以凑齐
                {
                    //更新冷静期状态为是，时间更新为当前时间。 冷静期进入时间，需要与中标时间一致。
                    string t1 = ZBSJ;
                    DateTime time1begin = DateTime.Parse(t1);

                    DateTime time2begin = time1begin.AddDays(1);

                    //计算冷静期结束时间
                    DataSet dsJQ = null; //DbHelperSQL.Query(" select * from AAA_PTJRSDB where SFYX = '是' order by RQ asc ");
                    return_ht = I_DBL.RunProc(" select * from AAA_PTJRSDB where SFYX = '是' order by RQ asc ","");
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
                    string t2 = time2begin.ToString();

                    ALsql.Add(" update AAA_LJQDQZTXXB set SFJRLJQ = '是',LJQKSSJ = '" + t1 + "',LJQJSSJ = '" + t2 + "' where    HTQX = '" + HTQX + "' and SPBH  = '" + SPBH + "' ");
                    //记录历史记录
                    string newnumber = ""; //jhjx_PublicClass.GetNextNumberZZ("AAA_LJQBGLSJLB", "");
                    re = IPC.Call("获取主键", new object[] { "AAA_LJQBGLSJLB", "" });//--------------------

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
                    ALsql.Add(" INSERT INTO AAA_LJQBGLSJLB (Number,SPBH,HTQX,LJQKSSJBGW,LJQJSSJBGW,SFZLJQZTBGW) VALUES ('" + newnumber + "','" + SPBH + "','" + HTQX + "','" + t1 + "','" + t2 + "','是') ");

                    //写入投标资料审核表   2013-08-22 添加
                    DataSet dsCount = null;
                    return_ht = I_DBL.RunProc("select COUNT(*) from AAA_TBZLSHB where  TBDH='" + dsTB.Tables[0].Rows[i]["Number"].ToString() + "'","");
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

                        re = IPC.Call("获取主键", new object[] { "AAA_TBZLSHB", "" });//--------------------

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
                            throw((Exception)re[1]);
                        }

                        ALsql.Add("  insert AAA_TBZLSHB(Number,TBDH,FWZXSHZT,JYGLBSHZT,FWZXSHYCHSFXG,JYGLBSHWTGHSFXG,CreateTime,CheckLimitTime) values('" + newnumberTBZLSH + "','" + dsTB.Tables[0].Rows[i]["Number"].ToString() + "','未审核','初始值','初始值','初始值','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "') ");
                    }


                    ALsql[0] = ALsql[0].ToString() + "  select '有商品进入冷静期'   ";

                }
                else //不能凑齐
                {
                    //冷静期状态不动，时间不动
                }
            }
            #endregion


            #region//给这个商品解锁，允许再发布投标信息和预订单。(先临时写到这里，其实不能写到这里，应该写到中标监控中的语句真正执行后。但发布投标和预订单后执行本方法返回语句时，不需要执行这个解锁)
            if (sp == "中标监控")
            {
                Hashtable htcc = new Hashtable();
                htcc["@SPBH"] = SPBH;
                htcc["@HTQX"] = HTQX;
                return_ht = I_DBL.RunParam_SQL("update AAA_LJQDQZTXXB set LJQYZBS = '未锁' where SPBH=@SPBH and HTQX=@HTQX", "", htcc);
                // DbHelperSQL.ExecuteSql("update AAA_LJQDQZTXXB set LJQYZBS = '未锁' where SPBH='" + SPBH + "' and HTQX='" + HTQX + "'");

            }
            #endregion

        }//投标单遍历结束
        #endregion

        if (ALsql != null && ALsql.Count > 0)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("sql");
            dc.DataType = typeof(string);
            dt.Columns.Add(dc);
            dt.Clear();
            ds.Tables.Add(dt);
            DataRow dr = null;
            for (int i = 0; i < ALsql.Count; i++)
            {
                dr = dt.NewRow();
                dr["sql"] = ALsql[i].ToString();
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            return ds;
        }
        else
        {
            return null;
        }


    }

    #region//中标监控(三个月一年)
    /// <summary>
    /// 执行中标监控
    /// </summary>
    /// <returns></returns>
    public static DataSet serverZBJK()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt();        
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            //找出超过出冷静期的数据
            Hashtable returnHT = I_DBL.RunProc("select distinct '商品编号'=SPBH,'合同期限'=HTQX  from  AAA_LJQDQZTXXB where SFJRLJQ='是' and LJQJSSJ < getdate()  order  by SPBH desc",""); //C
            if (!(bool)returnHT["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询超出冷静期数据的操作失败!";
                return dsreturn;
            }

            DataSet DStbxx = (DataSet)returnHT["return_ds"];
            if (DStbxx == null || DStbxx.Tables.Count < 1 || DStbxx.Tables[0].Rows.Count < 1)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有过冷静期的数据!";
                return dsreturn;
            } 
            
             //将查询到的进入冷静期的商品与合同期限写入Datset wyh 2014.08.01



            dstx = null;
            dstx = new DataSet();
            DataTable dtspxxx = DStbxx.Tables[0].Copy();
            dtspxxx.TableName = "SPXX";
            dstx.Tables.Add(dtspxxx);

            //初始化
            DataTable DTTX = InitDttx();
            DTTX.Clear();
            dstx.Tables.Add(DTTX);

            //执行语句                 
            DataSet alsql = new DataSet();

            alsql = RunCJGZ(DStbxx.Tables[0], "中标监控");
            bool ff = false;
            ArrayList list = new ArrayList();
            if (alsql != null && alsql.Tables[0].Rows.Count>0)
            {
               foreach(DataRow row in alsql.Tables[0].Rows)
               {
                   list.Add(row[0].ToString());
               }
            }
            if (list != null && list.Count > 1)
            {
               Hashtable  returnHTlist = I_DBL.RunParam_SQL(list);
               ff = (bool)returnHTlist["return_float"];
            }

            if (ff)
            {

                //提醒：  把txt读出来，传入接口“发送中标提醒”。（txt,C）
                // 若txt不存在，不发任何提醒。
                //若txt存在， A有内容。 代表有中标。
                        //通过A中的合同编号，结合B中的商品编号和合同期限，向中标的合同中买卖双方发送提醒。
                        //通过C中的商品编号和合同期限，结合剔除A中的投标单和预订单，向没有中标的，竞标中的，投标单和预订单发送提醒。
                //若txt存在，A没有内容。 代表没有中标信息。
                         //通过C中的商品编号和合同期限，向没有中标的，竞标中的，投标单和预订单发送提醒。
                Sendmes(dstx);
            }
            if (list != null && list.Count > 1 && ff)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = alsql.Tables[0].Rows[0][0].ToString().Replace("'", "").Replace("select", "").Trim();
                return dsreturn;
            }
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无中标或冷静期操作执行！";
 
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }
    #endregion

    #region 中标监控（三个月一年发送提醒）
    private static void Sendmes(DataSet dsxx)
    {
        //提醒：  把txt读出来，传入接口“发送中标提醒”。（txt,C）
        // 若txt不存在，不发任何提醒。
        //若txt存在， A有内容。 代表有中标。
        //通过A中的合同编号，结合B中的商品编号和合同期限，向中标的合同中买卖双方发送提醒。
        //通过C中的商品编号和合同期限，结合剔除A中的投标单和预订单，向没有中标的，竞标中的，投标单和预订单发送提醒。
        //若txt存在，A没有内容。 代表没有中标信息。
        //通过C中的商品编号和合同期限，向没有中标的，竞标中的，投标单和预订单发送提醒。

        //已中标信息获取 A 
        if (dsxx == null || dsxx.Tables.Count <= 0)
        {
            return;
        }

        DataTable dtxx = dsxx.Tables["dttx"];
        DataTable drspxx = dsxx.Tables["SPXX"];
        string StrYDD = "";
        string StrTBD = "";
        DataSet dsmes= GetTXDataSet();
        dsmes.Tables[0].Clear();
        DataRow dradd = null;
        string conteng = "";
        DateTime dt = DateTime.Now.AddDays(5); // 定标最迟时间
        if (dtxx != null && dtxx.Rows.Count > 0)  //A有内容
        {
             foreach (DataRow drs in dtxx.Rows)
             {
                  //给卖家发提醒
                 conteng = "您" + drs["商品编号+合同期限"].ToString().Split('*')[2].ToString() + "商品的" + drs["投标单号+电子购货合同编号"].ToString().Split('*')[0].ToString() + "投标单已全部中标，对应" + drs["投标单号+电子购货合同编号"].ToString().Split('*')[1].ToString() + "《电子购货合同》已生效。若您在" + dt.Year.ToString()+ "年"+dt.Month.ToString()+"月"+dt.Day.ToString()+"日"+dt.Hour.ToString()+"时"+dt.Minute.ToString()+"分前未能定标，该投标单即废标，对应的投标保证金将全额赔付给对应买方。";
                 dsmes.Tables[0].Rows.Add(new string[] { "集合经销平台", drs["投标单提醒对象登陆邮箱"].ToString(), drs["投标单提醒对象结算账户类型"].ToString(), drs["投标单提醒对象角色编号"].ToString(), "卖家", conteng, "中标监控（三个月，一年）", "", "" });

                 //给买家发提醒
                 if (drs["YDD是否全中"].ToString().Equals("是"))
                 {
                     conteng = "您" + drs["商品编号+合同期限"].ToString().Split('*')[2].ToString() + "商品的" + drs["预订单+电子购货合同编号"].ToString().Split('*')[0].ToString() + "预订单已全部中标，对应" + drs["预订单+电子购货合同编号"].ToString().Split('*')[1].ToString() + "《电子购货合同》已生效。";
                     
                 }
                 else
                 {
                     conteng = "您" + drs["商品编号+合同期限"].ToString().Split('*')[2].ToString() + "商品的" + drs["预订单+电子购货合同编号"].ToString().Split('*')[0].ToString() + "预订单已部分中标，对应" + drs["预订单+电子购货合同编号"].ToString().Split('*')[1].ToString() + "《电子购货合同》已生效。";
                 }
                 dsmes.Tables[0].Rows.Add(new string[] { "集合经销平台", drs["预订单提醒对象登陆邮箱"].ToString(), drs["预订单提醒对象结算账户类型"].ToString(), drs["预订单提醒对象角色编号"].ToString(), "买家", conteng, "中标监控（三个月，一年）", "", "" });
                 StrYDD += "'" + drs["预订单+电子购货合同编号"].ToString().Split('*')[0].ToString() + "',";
                 StrTBD += "'" + drs["投标单号+电子购货合同编号"].ToString().Split('*')[0].ToString() + "',";
             }

        }


        //发送未中标提醒
        //查询未中标的预订单  
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        string strspxx = "";
        foreach (DataRow drpxx in drspxx.Rows)
        {
            strspxx += "'" + drpxx["商品编号"].ToString().Trim() + drpxx["合同期限"].ToString().Trim() + "',";
        }

        string strwhereYDD = "";
        if (StrYDD.Trim().Equals(""))
        {
            strwhereYDD = "''";
        }
        else
        {
            strwhereYDD = StrYDD.Trim().Remove(StrYDD.Length - 1, 1);
        }
        string SQLydd = " select Number,DLYX,JSZHLX,MJJSBH,SPMC from AAA_YDDXXB where SPBH+HTQX in(" + strspxx.Remove(strspxx.Length - 1, 1).Trim() + ") and Number not in (" + StrYDD.Trim().Remove(StrYDD.Length - 1, 1) + ") and ZT='竞标'";


        //找出在冷静期但是没中标的预订单
        Hashtable returnHT = I_DBL.RunProc(SQLydd, ""); //C
        if ((bool)returnHT["return_float"])
        {
            DataSet DSydd = (DataSet)returnHT["return_ds"];
            if (DSydd != null && DSydd.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drydd in DSydd.Tables[0].Rows)
                {
                    //给买家发提醒 您**商品的**投标单未中标，已自动进入下一轮竞标。
                    //您**商品的**预订单未中标，已自动进入下一轮竞标。

                    conteng = "您" + drydd["SPMC"].ToString() + "商品的" + drydd["Number"].ToString() + "预订单未中标，已自动进入下一轮竞标。";
                    dsmes.Tables[0].Rows.Add(new string[] { "集合经销平台", drydd["DLYX"].ToString(), drydd["JSZHLX"].ToString(), drydd["MJJSBH"].ToString(), "买家", conteng, "中标监控（三个月，一年）", "", "" });
                }
            }
        }


        //找出冷近期中未中标的投标单

        string strwheretdb = "";
        if (StrTBD.Trim().Equals(""))
        {
            strwheretdb = "''";
        }
        else
        {
            strwheretdb = StrTBD.Trim().Remove(StrTBD.Length - 1, 1);
        }
        string Strtbd = " select Number,DLYX,JSZHLX,MJJSBH,SPMC from AAA_TBD where SPBH+HTQX in(" + strspxx.Remove(strspxx.Length - 1, 1).Trim() + ") and Number not in (" + strwheretdb + ") and  ZT='竞标'";
        returnHT = I_DBL.RunProc(Strtbd, ""); //C
        if ((bool)returnHT["return_float"])
        {
            DataSet DSTbd = (DataSet)returnHT["return_ds"];
            if (DSTbd != null && DSTbd.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drydd in DSTbd.Tables[0].Rows)
                {
                    //给买家发提醒 您**商品的**投标单未中标，已自动进入下一轮竞标。
                    //您**商品的**预订单未中标，已自动进入下一轮竞标。

                    conteng = "您" + drydd["SPMC"].ToString() + "商品的" + drydd["Number"].ToString() + "投标单未中标，已自动进入下一轮竞标。";
                    dsmes.Tables[0].Rows.Add(new string[] { "集合经销平台", drydd["DLYX"].ToString(), drydd["JSZHLX"].ToString(), drydd["MJJSBH"].ToString(), "卖家", conteng, "中标监控（三个月，一年）", "", "" });
                }
            }
        }

        dsmes.AcceptChanges();
        IPC.Call("发送提醒", new object[] { dsmes });

        
         


    }
    #endregion

    #region//即时交易监控
    /// <summary>
    /// 即时交易监控
    /// </summary>
    /// <returns>返回要执行的SQL语句组</returns>
    public static ArrayList RunCJGZ_JSJY()
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        //声明要返回的SQL数组
        ArrayList al = new ArrayList();//要返回的SQL
        al.Add("  ");
        int Num_zbspsl = 0; //中标商品数量
        int Num_runspsl = 0; //参与计算的商品总数量
        //Hashtable htParameterInfo = PublicClass2013.GetParameterInfo();//平台公用参数
        object[] re = IPC.Call("平台动态参数", new object[]{""});
        DataSet reParameterInfo = new DataSet();
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (re[1] != null)
            {
                reParameterInfo = (DataSet)(re[1]);
            }
            else
            {
                return null;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
            //throw (Exception)re[1];
            return null;
        }
        DataRow htParameterInfo = null;
        if (reParameterInfo != null && reParameterInfo.Tables[0].Rows.Count>0)
        {
            htParameterInfo = reParameterInfo.Tables[0].Rows[0];
        }

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
        sql_TBDs = sql_TBDs + "    update AAA_DaPanTJTime set LastJSZBtime_YC = getdate()  where LastYXBGtime>=LastJSZBtime  ";
        sql_TBDs = " select * from ( select row_number() over(partition by spbh order by tbjg,createtime) as 序号, * from AAA_TBD where ZT='竞标' and HTQX='即时' ) as tab left join AAA_DaPanTJTime as T on tab.SPBH = T.SPBH  where tab.序号=1 and  T.LastYXBGtime >= T.LastJSZBtime order by  tab.SPBH desc   ";
        //DataTable dt_TBDs = DbHelperSQL.Query(sql_TBDs).Tables[0];
        
        Hashtable returnHT = I_DBL.RunProc(sql_TBDs, "");
        if (!(bool)returnHT["return_float"])
        {
            return null;
        }
        DataTable dt_TBDs = ((DataSet)returnHT["return_ds"]).Tables[0];
        #endregion

        //dt_TBDs

        #region 3、遍历找出后的投标单信息，匹配预订单中价格大于等于此投标单的、时间最早的一条数据，进行匹配。
        DataSet dsmes = GetTXDataSet();
        for (int i = 0; i < dt_TBDs.Rows.Count; i++)
        {
            Num_runspsl++;
            string TBD_Number = dt_TBDs.Rows[i]["Number"].ToString();//投标单号
            Int64 TBSL = Convert.ToInt64(dt_TBDs.Rows[i]["TBNSL"]) - Convert.ToInt64(dt_TBDs.Rows[i]["YZBSL"]);//投标数量
            double TBJG = Convert.ToDouble(dt_TBDs.Rows[i]["TBJG"]);//投标价格
            string SPBH = dt_TBDs.Rows[i]["SPBH"].ToString();//商品编号
            string GHQY = dt_TBDs.Rows[i]["GHQY"].ToString();//供货区域
            string sql_YDD = "SELECT TOP 1 * FROM AAA_YDDXXB WHERE ZT='竞标' AND HTQX='即时' AND SPBH='" + SPBH + "' AND NMRJG >= " + TBJG + " AND CHARINDEX('|'+SHQYsheng+'|','" + GHQY + "') > 0 ORDER BY CreateTime ASC";
            //DataTable dt_YDD = DbHelperSQL.Query(sql_YDD).Tables[0];//预订单信息
            
            Hashtable returnHT1 = I_DBL.RunProc(sql_YDD, "");
            if (!(bool)returnHT1["return_float"])
            {
                return null;
            }
            DataTable dt_YDD = ((DataSet)returnHT1["return_ds"]).Tables[0];
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
                    DateTime time = DateTime.Now;
                    string TimeNow = time.ToString();//本次业务产生的时间都用这个
                    //更新投标单信息表
                    string UPDATE_TBD = "UPDATE AAA_TBD SET ZT='中标', YZBSL=(CASE (SELECT ZT FROM AAA_TBD WHERE Number='" + TBD_Number + "') WHEN '竞标' THEN TBNSL WHEN '撤销' THEN 'aaa' WHEN '中标' THEN 'aaa' END) WHERE Number='" + TBD_Number + "'";//如果单子的状态不是竞标，这句SQL会出错，从而使得事务回滚。
                    al.Add(UPDATE_TBD);

                    //插入中标信息表
                    //ZBDB_Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZBDBXXB", "N");//中标定标主键
                    object[] re1= IPC.Call("获取主键", new object[] { "AAA_ZBDBXXB", "N" });
                    
                    if (re1[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                        
                        if (re1[1] != null)
                        {
                            ZBDB_Number = (string)(re1[1]);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                        string reFF = re1[1].ToString();
                        return null;
                    }
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

                    double new_NDGJE = Math.Round((Convert.ToInt64(dt_YDD.Rows[0]["NDGSL"]) - Convert.ToInt64(dt_YDD.Rows[0]["YZBSL"]) - TBSL) * Convert.ToDouble(dt_YDD.Rows[0]["NMRJG"]), 2);

                    //这种情况，预订单被拆单了
                    string cd_sqlstr = "  "; //拆单更新处理sql
                    //若是此预订单第一次中标 并且 部分中标
                    if (Convert.ToInt64(dt_YDD.Rows[0]["YZBSL"]) == 0)
                    {
                        cd_sqlstr = " ,SFCD='是' ";
                    }

                    string UPDATE_YDD = "UPDATE AAA_YDDXXB SET YZBSL=(CASE (SELECT ZT FROM AAA_YDDXXB WHERE Number='" + YDD_Number + "') WHEN '竞标' THEN (YZBSL+" + TBSL + ") WHEN '撤销' THEN 'aaa' WHEN '中标' THEN 'aaa' END),DJDJ=DJDJ-" + Z_DJJE + ",NDGJE=" + new_NDGJE + cd_sqlstr + " WHERE Number='" + YDD_Number + "'";//如果预订单状态不是竞标，这句SQL会出错，从而使得事务回滚。反写：已中标数量、冻结投标保证金、投标金额
                    al.Add(UPDATE_YDD);


                    #region  发送提醒
                    //预定数量>投标数量时 ,
                    string conteng = "";
                    //买家发送提醒
                    conteng = "您" + Z_SPMC + "商品的" + Y_YSYDDBH + "预订单已部分中标，对应" + Z_HTBH + "《电子购货合同》已生效。";

                    dsmes.Tables[0].Rows.Add(new string[] { "集合经销平台", Y_YSYDDDLYX, Y_YSYDDJSZHLX, Y_YSYDDMJJSBH, "买家", conteng, "即时监控", "", "" });
                    //卖家发送提醒
                    DateTime dtimes = time.AddDays(3);
                    conteng = "您" + Z_SPMC + "商品的" + T_YSTBDBH + "投标单已全部中标，对应" + Z_HTBH + "《电子购货合同》已生效。若您在" + dtimes.Year.ToString() + "年" + dtimes.Month.ToString() + "月" + dtimes.Day.ToString() + "日" + dtimes.Hour.ToString() + "时" + dtimes.Minute.ToString() + "分前未能定标，该投标单即废标，对应的投标保证金将全额赔付给对应买方。";
                    dsmes.Tables[0].Rows.Add(new string[] { "集合经销平台", T_YSTBDDLYX, T_YSTBDJSZHLX, T_YSTBDMJJSBH, "卖家", conteng, "即时监控", "", "" });
                    #endregion


                }
                #endregion

                #region 预定数量<投标数量时，中标数量=预定数量。预订单为中标，投标单仍然为竞标
                if (YDSL < TBSL)
                {
                    DateTime time = DateTime.Now;
                    string TimeNow = time.ToString();//本次业务产生的时间都用这个
                    //更新预订单信息表
                    string UPDATE_YDD = "UPDATE AAA_YDDXXB SET ZT='中标', YZBSL=(CASE (SELECT ZT FROM AAA_YDDXXB WHERE Number='" + YDD_Number + "') WHEN '竞标' THEN NDGSL WHEN '撤销' THEN 'aaa' WHEN '中标' THEN 'aaa' END) WHERE Number='" + YDD_Number + "'";//如果预订单状态不是竞标，这句SQL会出错，从而使得事务回滚。
                    al.Add(UPDATE_YDD);
                    #region 插入中标定标信息表
                    //插入中标信息表
                    //ZBDB_Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZBDBXXB", "N");//中标定标主键
                    object[] re2 = IPC.Call("获取主键", new object[] { "AAA_ZBDBXXB", "N" });

                    if (re2[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。

                        if (re2[1] != null)
                        {
                            ZBDB_Number = (string)(re2[1]);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                        string reFF = re2[1].ToString();
                        return null;
                    }
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
                        
                        Hashtable returnHT2 = I_DBL.RunProc(sql_LSDZ, "");
                        if (!(bool)returnHT2["return_float"])
                        {
                            return null;
                        }
                        DataTable dt_LSDZ = ((DataSet)returnHT2["return_ds"]).Tables[0];
                        //DataTable dt_LSDZ = DbHelperSQL.Query(sql_LSDZ).Tables[0];

                        object[] re3 = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });
                        string ZKLSMXNumber="";
                        if (re3[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (re3[1] != null)
                            {
                                ZKLSMXNumber = (string)(re3[1]);
                            }
                            else
                            {
                                return null;
                            }
                            
                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                            string reFF = re3[1].ToString();
                            return null;
                        }

                        string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES('" + ZKLSMXNumber + "','" + dt_TBDs.Rows[i]["DLYX"].ToString() + "','" + dt_TBDs.Rows[i]["JSZHLX"].ToString() + "','" + dt_TBDs.Rows[i]["MJJSBH"].ToString() + "','" + "AAA_TBD" + "','" + TBD_Number + "','" + DateTime.Now.ToString() + "','" + dt_LSDZ.Rows[0]["YSLX"].ToString() + "','" + (Convert.ToDouble(dt_TBDs.Rows[i]["DJTBBZJ"].ToString()) - T_YSTBDDJTBBZJ) + "','" + dt_LSDZ.Rows[0]["XM"].ToString() + "','" + dt_LSDZ.Rows[0]["XZ"].ToString() + "','" + dt_LSDZ.Rows[0]["ZY"].ToString().Replace("[x1]", TBD_Number) + "','" + "接口编号" + "','" + TBD_Number + "投标单撤销" + "','" + dt_LSDZ.Rows[0]["SJLX"].ToString() + "')";//插入流水表
                        al.Add(sql_LS);

                        string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+" + (Convert.ToDouble(dt_TBDs.Rows[i]["DJTBBZJ"].ToString()) - T_YSTBDDJTBBZJ) + " WHERE B_DLYX='" + dt_TBDs.Rows[i]["DLYX"].ToString() + "'"; //更新余额
                        al.Add(sql_YE);

                     
                    }

                    #region  发送提醒
                    //预定数量<投标数量时,预订单全中，投标单部分中
                    string conteng = "";
                    //买家发送提醒
                    conteng = "您" + Z_SPMC + "商品的" + Y_YSYDDBH + "预订单已全部中标，对应" + Z_HTBH + "《电子购货合同》已生效。";

                    dsmes.Tables[0].Rows.Add(new string[] { "集合经销平台", Y_YSYDDDLYX, Y_YSYDDJSZHLX, Y_YSYDDMJJSBH, "买家", conteng, "即时监控", "", "" });
                    //卖家发送提醒
                    DateTime dtimes = time.AddDays(3);
                    conteng = "您" + Z_SPMC + "商品的" + T_YSTBDBH + "投标单已部分中标，对应" + Z_HTBH + "《电子购货合同》已生效。若您在" + dtimes.Year.ToString() + "年" + dtimes.Month.ToString() + "月" + dtimes.Day.ToString() + "日" + dtimes.Hour.ToString() + "时" + dtimes.Minute.ToString() + "分前未能定标，该投标单即废标，对应的投标保证金将全额赔付给对应买方。";
                    dsmes.Tables[0].Rows.Add(new string[] { "集合经销平台", T_YSTBDDLYX, T_YSTBDJSZHLX, T_YSTBDMJJSBH, "卖家", conteng, "即时监控", "", "" });
                    #endregion


                }
                #endregion

                #region 预定数量=投标数量，最理想情况。
                if (YDSL == TBSL)
                {
                    DateTime time = DateTime.Now;
                    string TimeNow = time.ToString();//本次业务产生的时间都用这个
                    //更新投标单信息表
                    string UPDATE_TBD = "UPDATE AAA_TBD SET ZT='中标', YZBSL=(CASE (SELECT ZT FROM AAA_TBD WHERE Number='" + TBD_Number + "') WHEN '竞标' THEN TBNSL WHEN '撤销' THEN 'aaa' WHEN '中标' THEN 'aaa' END) WHERE Number='" + TBD_Number + "'";//如果单子的状态不是竞标，这句SQL会出错，从而使得事务回滚。
                    al.Add(UPDATE_TBD);
                    //更新预订单信息表
                    string UPDATE_YDD = "UPDATE AAA_YDDXXB SET ZT='中标', YZBSL=(CASE (SELECT ZT FROM AAA_YDDXXB WHERE Number='" + YDD_Number + "') WHEN '竞标' THEN NDGSL WHEN '撤销' THEN 'aaa' WHEN '中标' THEN 'aaa' END) WHERE Number='" + YDD_Number + "'";//如果预订单状态不是竞标，这句SQL会出错，从而使得事务回滚。
                    al.Add(UPDATE_YDD);
                    //插入中标信息表
                    //ZBDB_Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZBDBXXB", "N");//中标定标主键

                    object[] re4 = IPC.Call("获取主键", new object[] { "AAA_ZBDBXXB", "N" });

                    if (re4[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                        if (re4[1] != null)
                        {
                            ZBDB_Number = (string)(re4[1]);
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                        string reFF = re4[1].ToString();
                        return null;
                    }

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

                    #region  发送提醒
                    //预定数量<投标数量时,预订单全中，投标单部分中
                    string conteng = "";
                    //买家发送提醒
                    conteng = "您" + Z_SPMC + "商品的" + Y_YSYDDBH + "预订单已全部中标，对应" + Z_HTBH + "《电子购货合同》已生效。";

                    dsmes.Tables[0].Rows.Add(new string[] { "集合经销平台", Y_YSYDDDLYX, Y_YSYDDJSZHLX, Y_YSYDDMJJSBH, "买家", conteng, "即时监控", "", "" });
                    //卖家发送提醒
                    DateTime dtimes = time.AddDays(3);
                    conteng = "您" + Z_SPMC + "商品的" + T_YSTBDBH + "投标单已全部中标，对应" + Z_HTBH + "《电子购货合同》已生效。若您在" + dtimes.Year.ToString() + "年" + dtimes.Month.ToString() + "月" + dtimes.Day.ToString() + "日" + dtimes.Hour.ToString() + "时" + dtimes.Minute.ToString() + "分前未能定标，该投标单即废标，对应的投标保证金将全额赔付给对应买方。";
                    dsmes.Tables[0].Rows.Add(new string[] { "集合经销平台", T_YSTBDDLYX, T_YSTBDJSZHLX, T_YSTBDMJJSBH, "卖家", conteng, "即时监控", "", "" });
                    #endregion
                    

                }
                #endregion


                //找参与竞标的买家和卖家数量，更新到中标定标信息表
                //DataSet ds_jyfsl = DbHelperSQL.Query("select (select count(distinct MJJSBH) from AAA_TBD where ZT = '竞标' and SPBH ='" + SPBH + "' and HTQX='即时') as 卖方数量 , (select count(distinct MJJSBH) from AAA_YDDXXB where ZT = '竞标' and SPBH ='" + SPBH + "' and HTQX='即时') as 买方数量");
                
                Hashtable returnHT3 = I_DBL.RunProc("select (select count(distinct MJJSBH) from AAA_TBD where ZT = '竞标' and SPBH ='" + SPBH + "' and HTQX='即时') as 卖方数量 , (select count(distinct MJJSBH) from AAA_YDDXXB where ZT = '竞标' and SPBH ='" + SPBH + "' and HTQX='即时') as 买方数量", "");
                if (!(bool)returnHT3["return_float"])
                {
                    return null;
                }
                DataSet ds_jyfsl = (DataSet)returnHT3["return_ds"];
                string Z_LC_number = Guid.NewGuid().ToString();

                al.Add("update AAA_ZBDBXXB set Z_Cynum_Sel = " + ds_jyfsl.Tables[0].Rows[0]["卖方数量"].ToString() + " , Z_Cynum_Buy = " + ds_jyfsl.Tables[0].Rows[0]["买方数量"].ToString() + " , Z_LC_number = '" + Z_LC_number + "', Z_Cynum_ZB_Sel=" + sum_Z_Cynum_ZB_Sel + ", Z_Cynum_ZB_Buy=" + sum_Z_Cynum_ZB_Buy.ToString() + ",Z_ZB_JE=" + sum_Z_ZB_JE.ToString() + ",Z_ZB_SL=" + sum_Z_ZB_SL.ToString() + "  where Number = '" + ZBDB_Number + "' ");

                Hashtable returnHT4 = I_DBL.RunProc("select COUNT(*) from AAA_TBZLSHB where  TBDH='" + dt_TBDs.Rows[i]["Number"].ToString() + "'","");
                if (!(bool)returnHT4["return_float"])
                {
                    return null;
                }
                if (returnHT4["return_ds"] == null || ((DataSet)returnHT4["return_ds"]).Tables.Count < 1 || ((DataSet)returnHT4["return_ds"]).Tables[0].Rows.Count < 1 )
                {
                    return null;
                }
                if ( Convert.ToInt32(((DataSet)returnHT4["return_ds"]).Tables[0].Rows[0][0].ToString()) == 0)//不存在这条投标单单号数据
                {
                    object[] re5 = IPC.Call("获取主键", new object[] { "AAA_TBZLSHB", "N" });
                    string newnumberTBZLSH = "";
                    if (re5[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                        if (re5[1] != null)
                        {
                            newnumberTBZLSH = (string)(re5[1]);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                        return null;
                    }
                    //string newnumberTBZLSH = jhjx_PublicClass.GetNextNumberZZ("AAA_TBZLSHB", "");
                    al.Add("  insert AAA_TBZLSHB(Number,TBDH,FWZXSHZT,JYGLBSHZT,FWZXSHYCHSFXG,JYGLBSHWTGHSFXG,CreateTime,CheckLimitTime) values('" + newnumberTBZLSH + "','" + dt_TBDs.Rows[i]["Number"].ToString() + "','未审核','初始值','初始值','初始值','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "') ");
                }

                #endregion
            }


        }
        #endregion
        //无论是否中标，更新“大盘数据变化时间对照表”中最后一次执行即时中标监控时间 为 预存时间 by 于海滨
        al.Add(" update AAA_DaPanTJTime set LastJSZBtime = LastJSZBtime_YC where LastYXBGtime>=LastJSZBtime ");
        al[0] = " ----即时交易成功中标商品数量" + Num_zbspsl + "个， 共" + Num_runspsl + "个商品参与计算。  " + Environment.NewLine;
        //-----------------------------------------------
        IPC.Call("发送提醒", new object[] { dsmes });
        return al;

        

    }
    /// <summary>
    /// 执行即时交易监控
    /// </summary>
    /// <returns></returns>
    public static DataSet serverZBJK_JSJY()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        string errSQL = "";
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            //执行语句                  
            ArrayList alsql = new ArrayList();

            alsql = RunCJGZ_JSJY();


            for (int i = 0; i < alsql.Count; i++)
            {
                errSQL = errSQL + alsql[i].ToString() + Environment.NewLine;
            }


            bool ff = false;
            if (alsql != null)
            {
                //ff = DbHelperSQL.ExecSqlTran(alsql);
                Hashtable returnHT = I_DBL.RunParam_SQL(alsql);
                ff = (bool)returnHT["return_float"];
            }

            if (ff)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = alsql[0].ToString().Replace("----", "").Replace(Environment.NewLine, "").Trim();
                return dsreturn;
            }
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行出错！";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString() + "SQL:" + errSQL;
            return dsreturn;
        }
    }
    #endregion

    #region//重新生成大盘监控
    private static Hashtable htThreadRe = new Hashtable();
    /// <summary>
    /// 开始执行一组的计算 by 于海滨
    /// </summary>
    /// <param name="SPBH">有业务变动的商品编号字符串，用竖杠分割</param>
    private static void BeginDapanOneGroup(object SPBH)
    {
        //生成大盘数据，放到临时表
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            
            Hashtable htCS=new Hashtable();
            htCS["@strspbh"] = SPBH;
            Hashtable returnHT = I_DBL.RunProc_CMD("AAA_DaPanPrd_Get", "", htCS);
            //DbHelperSQL.ExecuteSql(" exec  AAA_DaPanPrd_Get '" + SPBH + "' ");

            if ((bool)returnHT["return_float"])
            {
                htThreadRe["组执行结果_" + SPBH + "_" + Guid.NewGuid().ToString()] = "ok_" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString();
            }
            else
            {
                htThreadRe["组执行结果_" + SPBH + "_" + Guid.NewGuid().ToString()] = "err_" + SPBH + "_存储过程执行失败" ;
            }
           
        }
        catch (Exception ex)
        {
            htThreadRe["组执行结果_" + SPBH + "_" + Guid.NewGuid().ToString()] = "err_" + SPBH + "_" + ex.ToString();
        }

    }
    /// <summary>
    /// 为每组数据处理大盘统计 by 于海滨
    /// </summary>
    /// <param name="dsreturn">返回值</param>
    /// <param name="SQLdapan">大盘sql读取路径</param>
    /// <param name="SQLdibu">底部统计sql读取路径</param>
    /// <returns></returns>
    public static DataSet CreatNewDapanList_Group_Run( string SQLdapan_lujing, string SQLdibu_lujing)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            string showtime = "";
            showtime = showtime + DateTime.Now + "_0_\n\r";
            //从“大盘数据变化时间对照表”中，获取“最后一次有效数据变更时间”晚于等于“最后一次执行统计时间”的数据
            //最先删除临时表和时间对照表中的已撤销的商品，再处理商品表中有，但时间对照表中没有的，插入。   再更新符合条件的预存时间。  最后找符合条件的。 
            string SQL_find = "";
            SQL_find = SQL_find + "  delete AAA_DaPanTJTime where SPBH in (  select SPBH from   AAA_PTSPXXB where SFYX = '否' )  ; ";
            SQL_find = SQL_find + "  delete AAA_DaPanPrd_New where 商品编号 in (  select SPBH from   AAA_PTSPXXB where SFYX = '否' )  ; ";
            SQL_find = SQL_find + "  insert into AAA_DaPanTJTime (SPBH) (select S.SPBH from   AAA_PTSPXXB as S left join AAA_DaPanTJTime as D on  S.SPBH = D.SPBH where D.LastTJtime is null and  S.SFYX = '是' )   ;   ";
            SQL_find = SQL_find + "    update AAA_DaPanTJTime set LastTJtime_YC = getdate()  where LastYXBGtime>=LastTJtime  ; ";
            SQL_find = SQL_find + "  select SPBH,LastTJtime,LastYXBGtime from AAA_DaPanTJTime where LastYXBGtime>=LastTJtime  order by LastYXBGtime,SPBH desc ;  ";

            DataSet dsBG = new DataSet();
            Hashtable htCS = I_DBL.RunProc(SQL_find, "");
            if (!(bool)htCS["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htCS["return_errmsg"].ToString();
                return dsreturn;
            }
            dsBG = (DataSet)htCS["return_ds"];

            //为分组好的数据，开启独立线程进行运算
            int groupNum = 10; //每组N个商品
            int groupMaxThreads = 100; //最大并发线程。
            int groupnow = 1; //当前第几组
            int spbhNum = dsBG.Tables[0].Rows.Count; //商品编号一共几个
            string GroupStr = ""; //某组的字符串
            ThreadPool.SetMaxThreads(groupMaxThreads, groupMaxThreads);//设置线程池
            htThreadRe = new Hashtable(); //滞空线程标志
            for (int p = 0; p < spbhNum; p++)
            {

                //分组

                GroupStr = GroupStr + dsBG.Tables[0].Rows[p]["SPBH"].ToString() + "|";
                if (spbhNum == p + 1 || p == groupNum * groupnow - 1)
                {
                    GroupStr = GroupStr.Substring(0, GroupStr.Length - 1);
                    ThreadPool.QueueUserWorkItem(BeginDapanOneGroup, GroupStr);
                    GroupStr = "";
                    //更新组数量
                    groupnow++;
                }



            }


            //确定所有线程都跑完后，执行底部统计和压缩
            while (spbhNum != 0 && htThreadRe.Count != (groupnow - 1))
            {
                Thread.Sleep(50);
            }


            /*
            //临时调试
            string jieguo  = "";
            foreach (DictionaryEntry de in htThreadRe)
            {
                jieguo = jieguo + de.Key + "<br />";
            }
             
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = jieguo;
            return dsreturn;
            */

            //检查是否有执行错误的线程(只记录最后一组线程的错误)
            string TreadErr = "";
            foreach (DictionaryEntry de in htThreadRe)
            {
                if (de.Value.ToString().IndexOf("err_") >= 0)
                {
                    TreadErr = de.Value.ToString();
                }
            }




            //生成新的大盘二进制压缩文件(包括底部统计分析)
            //得到大盘二进制数据
            //HomePage HP = new HomePage();           
            //DataSet dslist = HP.test_ds_Run("默认", SQLdapan_lujing);
            object[] re = IPC.Call("列表加载返回DataSet", new object[] { "默认", SQLdapan_lujing });
            DataSet dslist = null;
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。

                if (re[1] != null)
                {
                    dslist = (DataSet)(re[1]);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            byte[] b = Helper.DataSet2Byte(dslist);
            ////生成大盘二进制文件
            //FileStream fs = new FileStream(SQLdapan_lujing.Replace("HomePageSQLlist.txt", "HomePageByte.txt"), FileMode.OpenOrCreate);
            //fs.Write(b, 0, b.Length);
            //fs.Flush();
            //fs.Close();
            

            //写入Redis缓存
            RedisClient RC = RedisClass.GetRedisClient(null);
            lock (RedisClass.LockObj)
            {
                RC.Set("str:HomePageByte", b);
            }


            showtime = showtime + DateTime.Now + "_1_\n\r";

            //得到商品基本数据，用于键盘精灵 
            //DataSet shangpinS = HP.test_ds_Run("完整列表", SQLdapan_lujing);

            object[] re1 = IPC.Call("列表加载返回DataSet", new object[] { "完整列表", SQLdapan_lujing });
            DataSet shangpinS = null;
            if (re1[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。

                if (re1[1] != null)
                {
                    shangpinS = (DataSet)(re1[1]);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }

            DataSet shangpinSnew = new DataSet();
            shangpinSnew.Tables.Clear();
            shangpinSnew.Tables.Add(shangpinS.Tables[0].DefaultView.ToTable(false, new string[] { "商品编号", "商品名称", "型号规格", "合同周期n" }));
            //加载拼音
            shangpinSnew.Tables[0].Columns.Add("简拼");
            shangpinSnew.Tables[0].Columns.Add("全拼");
            for (int p = 0; p < shangpinSnew.Tables[0].Rows.Count; p++)
            {
                shangpinSnew.Tables[0].Rows[p]["全拼"] = PTHelper.Convert(shangpinSnew.Tables[0].Rows[p]["商品名称"].ToString()).ToLower();
                shangpinSnew.Tables[0].Rows[p]["简拼"] = PTHelper.GetHeadLetters(shangpinSnew.Tables[0].Rows[p]["商品名称"].ToString()).ToLower();
            }



            byte[] b_shangpinS = Helper.DataSet2Byte(shangpinSnew);
            ////生成简化商品二进制文件
            //fs = new FileStream(SQLdapan_lujing.Replace("HomePageSQLlist.txt", "HomePageByte_jhxp.txt"), FileMode.OpenOrCreate);
            //fs.Write(b_shangpinS, 0, b_shangpinS.Length);
            //fs.Flush();
            //fs.Close();

            //写入Redis缓存
            lock (RedisClass.LockObj)
            {
                RC.Set("str:JHXP", b_shangpinS);
            }


            //得到底部统计分析二进制数据
            string SQL = File.ReadAllText(SQLdibu_lujing);
            //DataSet ds = DbHelperSQL.Query(SQL);

            DataSet ds = new DataSet();
            Hashtable htCS1 = I_DBL.RunProc(SQL, "");
            if (!(bool)htCS1["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htCS1["return_errmsg"].ToString();
                return dsreturn;
            }
            ds = (DataSet)htCS1["return_ds"];

            byte[] b_tongji = Helper.DataSet2Byte(ds);
            ////生成底部统计分析二进制文件
            //fs = new FileStream(SQLdapan_lujing.Replace("HomePageSQLlist.txt", "HomePageByte_tongji.txt"), FileMode.OpenOrCreate);
            //fs.Write(b_tongji, 0, b_tongji.Length);
            //fs.Flush();
            //fs.Close();


            //写入Redis缓存
            lock (RedisClass.LockObj)
            {
                RC.Set("str:HomePageByte_tongji", b_tongji);
            }

            showtime = showtime + DateTime.Now + "_h_";
            if (TreadErr == "")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "大盘共生成" + spbhNum + "个商品。" + groupNum + "个一组，共" + (groupnow - 1).ToString() + "组，最大按组并发" + groupMaxThreads + "个线程。";
                //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "大盘共生成" + spbhNum + "个商品。" + groupNum + "个一组，共" + (groupnow - 1).ToString() + "组，最大按组并发" + groupMaxThreads + "个线程。\n\r" + showtime;
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "大盘生成完成但有至少一个错误：" + TreadErr;
            }
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }



    }

    #endregion

    #region//预订单过期监控
    /// <summary>
    /// 预订单过期监控
    /// </summary>
    /// <returns></returns>
    public static DataSet serverGQJK()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        try
        {           

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            //执行语句      
            //CJGZ2013 cjgz = new CJGZ2013();        
            ArrayList alsql = new ArrayList();
            Hashtable HT = RunGQ();
            alsql = (ArrayList)HT["SQL"];
            bool ff = false;
            if (alsql != null && alsql.Count > 0)
            {
                Hashtable returnHT = I_DBL.RunParam_SQL(alsql);
                ff = (bool)returnHT["return_float"];
            }

            if (alsql != null && alsql.Count > 1 && ff)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "发现需要撤销的数据，成功执行撤销操作。";
                try
                {
                    DataSet list = (DataSet)HT["TX"];//List<List<Hashtable>>               
                    object[] re = IPC.Call("发送提醒", new object[] { list });
                    if (re[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                        string reFF = (string)(re[1]);
                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                        
                        throw ((Exception)re[1]);
                    }
                }
                catch (Exception exMsg)
                {
                    throw;
                }
                return dsreturn;
            }
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无过期预订单！";
            return dsreturn;
        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }
    }
    /// <summary>
    /// 检测是否有超期的预订单，有则返回撤销应执行的SQL，没有返回null
    /// </summary>
    /// <returns></returns>
    public static Hashtable RunGQ()
    {
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable ht = new Hashtable();
            ArrayList al = new ArrayList();//要执行的SQL
            //List<List<Hashtable>> list = new List<List<Hashtable>>();//要发送的提醒
            DataSet dsSedmsg = GetTXDataSet();//要发送的提醒

            /*
             思路描述
             1、本监控每天凌晨执行，如果一张预定单设定到1月23号，那么1月23号凌晨运行监控时这张订单不会被撤销。
             2、1月24号凌晨时运行监控，这张单子就作废了。
             3、所以获取服务器时间为现在，在数据库中查询出小于这个时间的所有预订单
             4、遍历，跑撤销流程。
             */
            string DateOnServer = DateTime.Now.ToString("yyyy-MM-dd");//服务器当前时间
            string sql = "SELECT *,AAA_DLZHXXB.B_YHM AS YHM FROM AAA_YDDXXB LEFT JOIN AAA_DLZHXXB ON AAA_YDDXXB.DLYX=AAA_DLZHXXB.B_DLYX WHERE YXJZRQ< '" + DateOnServer + "' AND ZT='竞标'";
            Hashtable returnHT = I_DBL.RunProc(sql, "");
            if (!(bool)returnHT["return_float"])
            {
                throw ((Exception)returnHT["return_errmsg"]);
            }
            DataTable dt = ((DataSet)returnHT["return_ds"]).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string YZBSL = dt.Rows[i]["YZBSL"].ToString().Trim();//已中标数量

                if (YZBSL == "0")
                {
                    #region 更改预订单信息
                    string sql_NGZB = "UPDATE AAA_YDDXXB SET ZT='撤销' ,NDGJE=((NDGSL-YZBSL)*NMRJG) WHERE Number='" + dt.Rows[i]["Number"].ToString() + "'";
                    #endregion
                    al.Add(sql_NGZB);

                    #region 生成流水信息
                    string sql_LSDZ = "SELECT * FROM AAA_moneyDZB WHERE Number='1304000019'";//资金对照表中的撤销预订单

                    //DataTable dt_LSGZ = DbHelperSQL.Query(sql_LSDZ).Tables[0];
                    Hashtable returnHT1 = I_DBL.RunProc(sql_LSDZ, "");
                    if (!(bool)returnHT1["return_float"])
                    {
                        throw ((Exception)returnHT["return_errmsg"]);
                    }
                    DataTable dt_LSGZ = ((DataSet)returnHT1["return_ds"]).Tables[0];

                    string number = "";
                    object[] re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                    if (re[0].ToString() == "ok")
                    {
                        if (re[1] != null)
                        {
                            number = re[1].ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        throw ((Exception)re[1]);
                    }

                    string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES('" + number + "','" + dt.Rows[i]["DLYX"].ToString() + "','" + dt.Rows[i]["JSZHLX"].ToString() + "','" + dt.Rows[i]["MJJSBH"].ToString() + "','" + "AAA_YDDXXB" + "','" + dt.Rows[i]["Number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dt_LSGZ.Rows[0]["YSLX"].ToString() + "','" + dt.Rows[i]["DJDJ"].ToString() + "','" + dt_LSGZ.Rows[0]["XM"].ToString() + "','" + dt_LSGZ.Rows[0]["XZ"].ToString() + "','" + dt_LSGZ.Rows[0]["ZY"].ToString().Replace("[x1]", dt.Rows[i]["Number"].ToString()) + "','" + "接口编号" + "','" + dt.Rows[i]["Number"].ToString() + "预订单撤销，原拟购量：" + dt.Rows[i]["NDGSL"].ToString() + "','" + dt_LSGZ.Rows[0]["SJLX"].ToString() + "')";//插入流水的表
                    #endregion
                    al.Add(sql_LS);

                    #region 更新用户余额
                    string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+" + dt.Rows[i]["DJDJ"].ToString().Trim() + " WHERE B_DLYX='" + dt.Rows[i]["DLYX"].ToString() + "'";
                    #endregion
                    al.Add(sql_YE);
                    string RQ = Convert.ToDateTime(dt.Rows[i]["YXJZRQ"]).ToString("yyyy年MM月dd日");
                    //您**商品的**预订单有效期截止**年**月**日，在有效期内该预订单未能中标，现已自动撤销。

                    //string content = "您" + dt.Rows[i]["SPMC"].ToString() + "商品的" + dt.Rows[i]["Number"].ToString() + "预订单有效期截至" + RQ + "，在有效期内该预订单未能中标，现已自动撤销。"; wyh dele 

                    #region//2014.09.12 zhouli 作废 需求变更
                    //string content = "您" + dt.Rows[i]["Number"].ToString() + "编号的《预订单》已于" + RQ + "自动全部撤销，相应订金已解冻。";//wyh add 
                    #endregion
                    string content = "您" + dt.Rows[i]["Number"].ToString() + "预订单未中标部分单已于" + RQ + "自动撤销，相应订金已解冻。";//zhouli add 2014.09.12

                    //list.Add(GetTXListArray(dt.Rows[i]["DLYX"].ToString(), dt.Rows[i]["YHM"].ToString(), dt.Rows[i]["JSZHLX"].ToString(), dt.Rows[i]["MJJSBH"].ToString(), content));
                    dsSedmsg.Tables[0].Rows.Add(new string[] { "集合经销平台", dt.Rows[i]["DLYX"].ToString(), dt.Rows[i]["JSZHLX"].ToString(), dt.Rows[i]["MJJSBH"].ToString(), "买家", content, "Admin","","" });
                }
                else
                {



                    string sql_NGZB = "UPDATE AAA_YDDXXB SET NDGSL=YZBSL,ZT='中标' WHERE Number='" + dt.Rows[i]["Number"].ToString() + "'";//把拟订购数量更新为与中标数量相同的量，把状态改为中标


                    string sql_LSDZ = "SELECT * FROM AAA_moneyDZB WHERE Number='1304000019'";//资金对照表中的撤销预订单

                    //DataTable dt_LSGZ = DbHelperSQL.Query(sql_LSDZ).Tables[0];

                    Hashtable returnHT1 = I_DBL.RunProc(sql_LSDZ, "");
                    if (!(bool)returnHT1["return_float"])
                    {
                        return null;
                    }
                    DataTable dt_LSGZ = ((DataSet)returnHT1["return_ds"]).Tables[0];

                    string number = "";
                    object[] re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                    if (re[0].ToString() == "ok")
                    {
                        if (re[1] != null)
                        {
                            number = re[1].ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        throw ((Exception)re[1]);
                    }

                    string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES('" + number + "','" + dt.Rows[i]["DLYX"].ToString() + "','" + dt.Rows[i]["JSZHLX"].ToString() + "','" + dt.Rows[i]["MJJSBH"].ToString() + "','" + "AAA_YDDXXB" + "','" + dt.Rows[i]["Number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dt_LSGZ.Rows[0]["YSLX"].ToString() + "','" + dt.Rows[i]["DJDJ"].ToString() + "','" + dt_LSGZ.Rows[0]["XM"].ToString() + "','" + dt_LSGZ.Rows[0]["XZ"].ToString() + "','" + dt_LSGZ.Rows[0]["ZY"].ToString().Replace("[x1]", dt.Rows[i]["Number"].ToString()) + "','" + "接口编号" + "','" + dt.Rows[i]["Number"].ToString() + "预订单撤销（全单）" + "','" + dt_LSGZ.Rows[0]["SJLX"].ToString() + "')";//插入流水的表

                    string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+" + dt.Rows[i]["DJDJ"].ToString().Trim() + " WHERE B_DLYX='" + dt.Rows[i]["DLYX"].ToString() + "'";
                    al.Add(sql_NGZB);
                    al.Add(sql_LS);
                    al.Add(sql_YE);
                    string RQ = Convert.ToDateTime(dt.Rows[i]["YXJZRQ"]).ToString("yyyy年MM月dd日");
                    //string content = "您" + dt.Rows[i]["SPMC"].ToString() + "商品的" + dt.Rows[i]["Number"].ToString() + "预订单有效期截至" + RQ + "，在有效期内该预订单已中标" + dt.Rows[i]["YZBSL"].ToString() + dt.Rows[i]["JJDW"].ToString() + "，剩余未中标部分现在已自动撤销。"; wyh dele 
                    #region//2014.09.12 zhouli 作废 需求变更
                    //string content = "您" + dt.Rows[i]["Number"].ToString() + "编号的部分《预订单》已于" + RQ + "自动撤销，相应订金已解冻。"; //wyh add
                    #endregion
                    string content = "您" + dt.Rows[i]["Number"].ToString() + "预订单未中标部分单已于" + RQ + "自动撤销，相应订金已解冻。";//zhouli add 2014.09.12
                    //您**商品的**预订单有效期截止**年**月**日，在有效期内该预订单已中标**，剩余未中标部分现已自动撤销。

                    //list.Add(GetTXListArray(dt.Rows[i]["DLYX"].ToString(), dt.Rows[i]["YHM"].ToString(), dt.Rows[i]["JSZHLX"].ToString(), dt.Rows[i]["MJJSBH"].ToString(), content));
                    dsSedmsg.Tables[0].Rows.Add(new string[] { "集合经销平台", dt.Rows[i]["DLYX"].ToString(),  dt.Rows[i]["JSZHLX"].ToString(), dt.Rows[i]["MJJSBH"].ToString(), "买家", content, "Admin" ,"",""});
                }
            }
            ht["SQL"] = al;
            ht["TX"] = dsSedmsg;
            return ht;
        }
        catch(Exception ex)
        {
            throw (ex);
        }
    }
    
    /// <summary>
    /// 生成提醒数据集
    /// </summary>
    /// <returns></returns>
    private static DataSet GetTXDataSet()
    {
        DataSet dsreturn = new DataSet();
        DataTable rtdt = new DataTable();
        rtdt.Columns.Add(new DataColumn("type", typeof(string)));
        rtdt.Columns.Add(new DataColumn("提醒对象登陆邮箱", typeof(string)));
        //rtdt.Columns.Add(new DataColumn("提醒对象用户名", typeof(string)));
        rtdt.Columns.Add(new DataColumn("提醒对象结算账户类型", typeof(string)));
        rtdt.Columns.Add(new DataColumn("提醒对象角色编号", typeof(string)));
        rtdt.Columns.Add(new DataColumn("提醒对象角色类型", typeof(string)));
        rtdt.Columns.Add(new DataColumn("提醒内容文本", typeof(string)));
        rtdt.Columns.Add(new DataColumn("创建人", typeof(string)));
        rtdt.Columns.Add(new DataColumn("查看地址", typeof(string)));
        rtdt.Columns.Add("模提醒模块名", typeof(string));
        dsreturn.Tables.Add(rtdt);
        return dsreturn;
    }
    #endregion

    #region//延迟发货监控
    /// <summary>
    /// 执行发货延迟监控
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public static DataSet RunFHYC()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            //--所有的延迟发货，处理时， 需要补处理的天数(B) = 共延迟天数-已完成预扣天数-1 ，即为 。 从今天开始倒着来，分别插入B天中，每天的预扣和预补偿数据就行了。 减1是因为监控是在凌晨零几分跑的，所以跑监控这天要去掉。
            Hashtable htCS = I_DBL.RunProc("select '履约保证金剩余金额'=ZZZ.Z_LYBZJJE - isnull((select sum(LS.JE) from  AAA_ZKLSMXB as LS left join AAA_THDYFHDXXB as TTTn on LS.LYDH = TTTn.Number  where  LS.XM='违约赔偿金' and ( LS.XZ = '超过最迟发货日后录入发货信息' or LS.XZ = '发货单生成后5日内未录入发票邮寄信息' ) and LS.SJLX='预' and TTTn.ZBDBXXBBH=TTT.ZBDBXXBBH),0),'是否有延迟'='否','物流信息录入时间'=TTT.F_WLXXLRSJ,'最迟发货日'=TTT.T_ZCFHR,'共延迟天数'=DATEDIFF(day,TTT.T_ZCFHR,isnull(TTT.F_WLXXLRSJ,GETDATE())), '已完成预扣天数'=(select count(*) from  AAA_ZKLSMXB as LS where  LS.XM='违约赔偿金' and  LS.XZ = '超过最迟发货日后录入发货信息' and LS.SJLX='预' and LS.LYDH=TTT.Number), * from AAA_THDYFHDXXB as TTT left join AAA_ZBDBXXB as ZZZ on TTT.ZBDBXXBBH = ZZZ.Number where DATEDIFF(day,TTT.T_ZCFHR,isnull(TTT.F_WLXXLRSJ,GETDATE())) >0 and  ZZZ.Z_QPZT = '未开始清盘' and  ZZZ.Z_HTZT = '定标' and   DATEDIFF(day, ZZZ.Z_HTJSRQ, GETDATE()) <=0  and ZZZ.Z_HTQX <> '即时' and TTT.F_DQZT not in ('撤销','卖家主动退货') ", "");

            if (!(bool)htCS["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htCS["return_errmsg"].ToString();
                return dsreturn;
            }

            DataSet dsYC = (DataSet)htCS["return_ds"];
            if (dsYC != null && dsYC.Tables[0].Rows.Count > 0)
            {
                //获得履约保证金剩余金额哈希表，防出现负值
                Hashtable HTlvhzj_SY = new Hashtable();
                for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
                {
                    if (!HTlvhzj_SY.ContainsKey(dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()))
                    {
                        HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = dsYC.Tables[0].Rows[i]["履约保证金剩余金额"].ToString();
                    }
                }
                //要执行的sql语句
                ArrayList alsql = new ArrayList();

                //对照表
                Hashtable htCS1 = I_DBL.RunProc("select * from AAA_moneyDZB where Number in         ('1304000027','1304000051')","");
                if (!(bool)htCS1["return_float"])
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htCS1["return_errmsg"].ToString();
                    return dsreturn;
                }

                DataSet dsDZB = (DataSet)htCS1["return_ds"];

                //本次几个提货单被处理了
                int Daynum = 0;//被有效处理的延迟天数
                for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
                {
                    //共延迟天数
                    int allycday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["共延迟天数"]);
                    //已完成预扣天数
                    int yykday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["已完成预扣天数"]);
                    //计算扣罚金额***********
                    double f_money = 5;
                    //货款金额*0.002
                    double old_money = Math.Round(Convert.ToDouble(dsYC.Tables[0].Rows[i]["T_DJHKJE"]) * 0.002, 2);


                    if (old_money < 5)
                    {
                        //不足5元的，按5元算
                        f_money = 5;
                    }
                    else
                    {
                        f_money = old_money;
                    }

                    /*
                    #region wyh 2013.6.24 add增加即时扣款规则
                    if (dsYC.Tables[0].Rows[i]["Z_HTQX"].ToString().Trim().Equals("即时"))
                    {
                        if (old_money < 100)
                        {
                            //不足5元的，按5元算
                            f_money = 100;
                        }
                        else
                        {
                            f_money = old_money;
                        }
                    }
                    else
                    {
                        if (old_money < 5)
                        {
                            //不足5元的，按5元算
                            f_money = 5;
                        }
                        else
                        {
                            f_money = old_money;
                        }
                    }
                    #endregion
                    */
                    //倒着跑，从最后一天往前插入
                    for (int b = (allycday - 1); b > yykday; b--)
                    {

                        //插入罚款数据，预*********************************
                        DataRow dr = dsDZB.Tables[0].Select("Number = '1304000027'")[0];

                        string Number = "";
                        object[] re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (re[0].ToString() == "ok")
                        {
                            if (re[1] != null)
                            {
                                Number = re[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                            return dsreturn;
                        }

                        string DLYX = dsYC.Tables[0].Rows[i]["T_YSTBDDLYX"].ToString();  //登陆邮箱
                        string JSZHLX = dsYC.Tables[0].Rows[i]["T_YSTBDJSZHLX"].ToString();  //交易账户类型
                        string JSBH = dsYC.Tables[0].Rows[i]["T_YSTBDMJJSBH"].ToString(); //角色编号
                        string LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                        string LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                        string LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                        string YSLX = dr["YSLX"].ToString();   //运算类型
                        double JE = f_money;   //金额
                        //对金额进行二次处理，防止被扣成负值，若这条扣的金额大于履约保证金剩余金额，则按照履约保证金剩余金额处理
                        if (JE > Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()))
                        {
                            JE = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString());
                        }
                        HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()) - JE;

                        string XM = dr["XM"].ToString();  //项目
                        string XZ = dr["XZ"].ToString();   //性质
                        string ZY = dr["ZY"].ToString().Replace("[x1]", "F" + dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                        string JKBH = "暂无";
                        string QTBZ = "暂无";
                        string SJLX = dr["SJLX"].ToString();  //数据类型
                        if (JE == 0.00)
                        {
                            ZY = ZY + "（履约保证金不足）";
                        }
                        alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");
                        //插入被赔付的数据，预*********************************
                        dr = dsDZB.Tables[0].Select("Number = '1304000051'")[0];
                        //Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (re[0].ToString() == "ok")
                        {
                            if (re[1] != null)
                            {
                                Number = re[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                            return dsreturn;
                        }
                        DLYX = dsYC.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString();  //登陆邮箱
                        JSZHLX = dsYC.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString();  //交易账户类型
                        JSBH = dsYC.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString(); //角色编号
                        LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                        LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                        LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                        YSLX = dr["YSLX"].ToString();   //运算类型
                        //JE = f_money;   //金额,这个不用算了，用上面罚款的金额
                        XM = dr["XM"].ToString();  //项目
                        XZ = dr["XZ"].ToString();   //性质
                        ZY = dr["ZY"].ToString().Replace("[x1]", "F" + dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                        JKBH = "暂无";
                        QTBZ = "暂无";
                        SJLX = dr["SJLX"].ToString();  //数据类型
                        if (JE == 0.00)
                        {
                            ZY = ZY + "（履约保证金不足）";
                        }
                        alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");

                        //计算延迟天数和有效条数
                        Daynum++;
                        dsYC.Tables[0].Rows[i]["是否有延迟"] = "是";

                    }
                }

                Hashtable htCS2= I_DBL.RunParam_SQL(alsql);
                bool rr = (bool)htCS2["return_float"];
                //bool rr = true;

                if (rr && alsql.Count > 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理了" + dsYC.Tables[0].Select("是否有延迟='是'").Length.ToString() + "条延迟发货提货单，共计" + Daynum.ToString() + "天延迟!";
                    return dsreturn;

                }
                else
                {
                    if (alsql.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "延迟发货数据生成的SQL语句执行未成功!";
                        return dsreturn;
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理了" + dsYC.Tables[0].Select("是否有延迟='是'").Length.ToString() + "条延迟发货提货单，共计" + Daynum.ToString() + "天延迟!";
                        return dsreturn;
                    }
                }


            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有要处理的延迟发货数据!";
                return dsreturn;
            }
        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }

    }
    #endregion

    #region//延迟发票监控
    /// <summary>
    /// 执行发票延迟监控
    /// </summary>
    /// <returns></returns>
    public static DataSet RunFPYC()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            //--所有的延迟发票寄送，处理时， 需要补处理的天数(B) = 共延迟天数-已完成预扣天数-1 ，即为 。 从今天开始倒着来，分别插入B天中，每天的预扣和预补偿数据就行了。 减1是因为监控是在凌晨零几分跑的，所以跑监控这天要去掉。
            Hashtable returnHT = I_DBL.RunProc("select '履约保证金剩余金额'=ZZZ.Z_LYBZJJE - isnull((select sum(LS.JE) from  AAA_ZKLSMXB as LS left join AAA_THDYFHDXXB as TTTn on LS.LYDH = TTTn.Number  where  LS.XM='违约赔偿金' and  ( LS.XZ = '超过最迟发货日后录入发货信息' or LS.XZ = '发货单生成后5日内未录入发票邮寄信息' ) and LS.SJLX='预' and TTTn.ZBDBXXBBH=TTT.ZBDBXXBBH ),0),'是否有延迟'='否', '发货单生成时间'=TTT.F_FHDSCSJ,'发票邮寄信息录入时间'=TTT.F_FPYJXXLRSJ,'发票最迟寄送日期'=DATEADD(day, 5, TTT.F_FHDSCSJ) ,'共延迟天数'=DATEDIFF(day,DATEADD(day, 5, TTT.F_FHDSCSJ),isnull(TTT.F_FPYJXXLRSJ,GETDATE())), '已完成预扣天数'=(select count(*) from  AAA_ZKLSMXB as LS where  LS.XM='违约赔偿金' and  LS.XZ = '发货单生成后5日内未录入发票邮寄信息' and LS.SJLX='预' and LS.LYDH=TTT.Number), * from AAA_THDYFHDXXB as TTT left join AAA_ZBDBXXB as ZZZ on TTT.ZBDBXXBBH = ZZZ.Number where DATEDIFF(day,DATEADD(day, 5, TTT.F_FHDSCSJ),isnull(TTT.F_FPYJXXLRSJ,GETDATE())) >0 and  ZZZ.Z_QPZT = '未开始清盘' and ZZZ.Z_HTQX <> '即时' and  ZZZ.Z_HTZT = '定标' and   DATEDIFF(day, ZZZ.Z_HTJSRQ, GETDATE()) <=0  and TTT.F_FHDSCSJ is not null and TTT.F_DQZT not in ('撤销','卖家主动退货')", "");
            if (!(bool)returnHT["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT["return_errmsg"].ToString();
                return dsreturn;
            }
            DataSet dsYC = (DataSet)returnHT["return_ds"];
            if (dsYC != null && dsYC.Tables[0].Rows.Count > 0)
            {
                //获得履约保证金剩余金额哈希表，防出现负值
                Hashtable HTlvhzj_SY = new Hashtable();
                for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
                {
                    if (!HTlvhzj_SY.ContainsKey(dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()))
                    {
                        HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = dsYC.Tables[0].Rows[i]["履约保证金剩余金额"].ToString();
                    }
                }


                ArrayList alsql = new ArrayList();

                //对照表
                Hashtable htCS1 =I_DBL.RunProc("select * from AAA_moneyDZB where Number in         ('1304000029','1304000050')","");
                if (!(bool)htCS1["return_float"])
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htCS1["return_errmsg"].ToString();
                    return dsreturn;
                }
                DataSet dsDZB = (DataSet)htCS1["return_ds"];
                int Daynum = 0;//被有效处理的延迟天数
                for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
                {
                    //共延迟天数
                    int allycday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["共延迟天数"]);
                    //已完成预扣天数
                    int yykday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["已完成预扣天数"]);
                    //计算扣罚金额，一天5元***********
                    double f_money = 5;
                    //倒着跑，从最后一天往前插入
                    for (int b = (allycday - 1); b > yykday; b--)
                    {
                        //插入罚款数据，预*********************************
                        DataRow dr = dsDZB.Tables[0].Select("Number = '1304000029'")[0];
                        //string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string Number = "";
                        object[] re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (re[0].ToString() == "ok")
                        {
                            if (re[1] != null)
                            {
                                Number = re[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                            return dsreturn;
                        }
                        string DLYX = dsYC.Tables[0].Rows[i]["T_YSTBDDLYX"].ToString();  //登陆邮箱
                        string JSZHLX = dsYC.Tables[0].Rows[i]["T_YSTBDJSZHLX"].ToString();  //交易账户类型
                        string JSBH = dsYC.Tables[0].Rows[i]["T_YSTBDMJJSBH"].ToString(); //角色编号
                        string LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                        string LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                        string LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                        string YSLX = dr["YSLX"].ToString();   //运算类型
                        double JE = f_money;   //金额
                        //对金额进行二次处理，防止被扣成负值，若这条扣的金额大于履约保证金剩余金额，则按照履约保证金剩余金额处理
                        if (JE > Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()))
                        {
                            JE = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString());
                        }
                        HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()) - JE;

                        string XM = dr["XM"].ToString();  //项目
                        string XZ = dr["XZ"].ToString();   //性质
                        string ZY = dr["ZY"].ToString().Replace("[x1]", "F" + dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                        string JKBH = "暂无";
                        string QTBZ = "暂无";
                        string SJLX = dr["SJLX"].ToString();  //数据类型
                        if (JE == 0.00)
                        {
                            ZY = ZY + "（履约保证金不足）";
                        }
                        alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");
                        //插入被赔付的数据，预*********************************
                        dr = dsDZB.Tables[0].Select("Number = '1304000050'")[0];
                        //Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (re[0].ToString() == "ok")
                        {
                            if (re[1] != null)
                            {
                                Number = re[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                            return dsreturn;
                        }
                        DLYX = dsYC.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString();  //登陆邮箱
                        JSZHLX = dsYC.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString();  //交易账户类型
                        JSBH = dsYC.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString(); //角色编号
                        LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                        LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                        LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                        YSLX = dr["YSLX"].ToString();   //运算类型
                        //JE = f_money;   //金额,这个不用算了，用上面罚款的金额
                        XM = dr["XM"].ToString();  //项目
                        XZ = dr["XZ"].ToString();   //性质
                        ZY = dr["ZY"].ToString().Replace("[x1]", "F" + dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                        JKBH = "暂无";
                        QTBZ = "暂无";
                        SJLX = dr["SJLX"].ToString();  //数据类型
                        if (JE == 0.00)
                        {
                            ZY = ZY + "（履约保证金不足）";
                        }
                        alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");

                        //计算延迟天数和有效条数
                        Daynum++;
                        dsYC.Tables[0].Rows[i]["是否有延迟"] = "是";

                    }
                }
                //bool rr = true;
                //bool rr = DbHelperSQL.ExecSqlTran(alsql);
                Hashtable htCS2 = I_DBL.RunParam_SQL(alsql);
                bool rr = (bool)htCS2["return_float"];
                if (rr && alsql.Count > 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理了" + dsYC.Tables[0].Select("是否有延迟='是'").Length.ToString() + "条延迟发票寄送提货单，共计" + Daynum.ToString() + "天延迟!";
                    return dsreturn;
                }
                else
                {
                    if (alsql.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "延迟发票寄送数据生成的SQL执行失败!";
                        return dsreturn;
                    }

                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理了" + dsYC.Tables[0].Select("是否有延迟='是'").Length.ToString() + "条延迟发票寄送提货单，共计" + Daynum.ToString() + "天延迟!";
                        return dsreturn;
                    }
                }


            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有要处理的延迟发票寄送数据!";
                return dsreturn;
            }
        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }

    }
    #endregion

    #region//即时延迟发货
    /// <summary>
    /// 执行发货延迟监控(仅用于即时的)
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public static DataSet RunFHYC_onlyJSJY()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            //--所有的延迟发货，处理时， 需要补处理的天数(B) = 共延迟天数-已完成预扣天数-1 ，即为 。 从今天开始倒着来，分别插入B天中，每天的预扣和预补偿数据就行了。 减1是因为监控是在凌晨零几分跑的，所以跑监控这天要去掉。
            Hashtable returnHT = I_DBL.RunProc("select '履约保证金剩余金额'=ZZZ.Z_LYBZJJE - isnull((select sum(LS.JE) from  AAA_ZKLSMXB as LS left join AAA_THDYFHDXXB as TTTn on LS.LYDH = TTTn.Number  where  LS.XM='违约赔偿金' and ( LS.XZ = '超过最迟发货日后录入发货信息' or LS.XZ = '发货单生成后5日内未录入发票邮寄信息' ) and LS.SJLX='预' and TTTn.ZBDBXXBBH=TTT.ZBDBXXBBH),0),'是否有延迟'='否','物流信息录入时间'=TTT.F_WLXXLRSJ,'最迟发货日'=TTT.T_ZCFHR,'共延迟天数'=DATEDIFF(day,TTT.T_ZCFHR,CASE WHEN isnull(TTT.F_WLXXLRSJ,GETDATE()) >= isnull(TTT.F_FPYJXXLRSJ,GETDATE()) THEN isnull(TTT.F_WLXXLRSJ,GETDATE())   ELSE isnull(TTT.F_FPYJXXLRSJ,GETDATE())   END ), '已完成预扣天数'=(select count(*) from  AAA_ZKLSMXB as LS where  LS.XM='违约赔偿金' and  LS.XZ = '超过最迟发货日后录入发货信息' and LS.SJLX='预' and LS.LYDH=TTT.Number), * from AAA_THDYFHDXXB as TTT left join AAA_ZBDBXXB as ZZZ on TTT.ZBDBXXBBH = ZZZ.Number where DATEDIFF(day,TTT.T_ZCFHR,CASE WHEN isnull(TTT.F_WLXXLRSJ,GETDATE()) >= isnull(TTT.F_FPYJXXLRSJ,GETDATE()) THEN isnull(TTT.F_WLXXLRSJ,GETDATE())   ELSE isnull(TTT.F_FPYJXXLRSJ,GETDATE())   END ) >0 and  ZZZ.Z_QPZT = '未开始清盘' and  ZZZ.Z_HTZT = '定标' and   DATEDIFF(day, ZZZ.Z_HTJSRQ, GETDATE()) <=0  and ZZZ.Z_HTQX = '即时' and TTT.F_DQZT not in ('撤销','卖家主动退货') ", "");
             if (!(bool)returnHT["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT["return_errmsg"].ToString();
                return dsreturn;
            }
            DataSet dsYC = (DataSet)returnHT["return_ds"];

            if (dsYC != null && dsYC.Tables[0].Rows.Count > 0)
            {
                //获得履约保证金剩余金额哈希表，防出现负值
                Hashtable HTlvhzj_SY = new Hashtable();
                for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
                {
                    if (!HTlvhzj_SY.ContainsKey(dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()))
                    {
                        HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = dsYC.Tables[0].Rows[i]["履约保证金剩余金额"].ToString();
                    }
                }
                //要执行的sql语句
                ArrayList alsql = new ArrayList();

                //对照表
                Hashtable htCS1 = I_DBL.RunProc("select * from AAA_moneyDZB where Number in         ('1304000027','1304000051')","");
                if (!(bool)htCS1["return_float"])
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htCS1["return_errmsg"].ToString();
                    return dsreturn;
                }
                DataSet dsDZB = (DataSet)htCS1["return_ds"];
                //本次几个提货单被处理了
                int Daynum = 0;//被有效处理的延迟天数
                for (int i = 0; i < dsYC.Tables[0].Rows.Count; i++)
                {
                    //共延迟天数
                    int allycday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["共延迟天数"]);
                    //已完成预扣天数
                    int yykday = Convert.ToInt32(dsYC.Tables[0].Rows[i]["已完成预扣天数"]);
                    //计算扣罚金额***********
                    double f_money = 0;
                    //货款金额 *0.0005
                    double old_money = Math.Round(Convert.ToDouble(dsYC.Tables[0].Rows[i]["T_DJHKJE"]) * 0.0005, 2);


                    if (old_money < 100)
                    {
                        //不足5元的，按5元算
                        f_money = 100;
                    }
                    else
                    {
                        f_money = old_money;
                    }

                    //倒着跑，从最后一天往前插入
                    for (int b = (allycday - 1); b > yykday; b--)
                    {

                        //插入罚款数据，预*********************************
                        DataRow dr = dsDZB.Tables[0].Select("Number = '1304000027'")[0];

                        //string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string Number = "";
                        object[] re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (re[0].ToString() == "ok")
                        {
                            if (re[1] != null)
                            {
                                Number = re[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                            return dsreturn;
                        }
                        string DLYX = dsYC.Tables[0].Rows[i]["T_YSTBDDLYX"].ToString();  //登陆邮箱
                        string JSZHLX = dsYC.Tables[0].Rows[i]["T_YSTBDJSZHLX"].ToString();  //交易账户类型
                        string JSBH = dsYC.Tables[0].Rows[i]["T_YSTBDMJJSBH"].ToString(); //角色编号
                        string LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                        string LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                        string LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                        string YSLX = dr["YSLX"].ToString();   //运算类型
                        double JE = f_money;   //金额
                        //对金额进行二次处理，防止被扣成负值，若这条扣的金额大于履约保证金剩余金额，则按照履约保证金剩余金额处理
                        if (JE > Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()))
                        {
                            JE = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString());
                        }
                        HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()] = Convert.ToDouble(HTlvhzj_SY[dsYC.Tables[0].Rows[i]["ZBDBXXBBH"].ToString()].ToString()) - JE;

                        string XM = dr["XM"].ToString();  //项目
                        string XZ = dr["XZ"].ToString();   //性质
                        string ZY = dr["ZY"].ToString().Replace("[x1]", "F" + dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                        string JKBH = "暂无";
                        string QTBZ = "暂无";
                        string SJLX = dr["SJLX"].ToString();  //数据类型
                        if (JE == 0.00)
                        {
                            ZY += "（履约保证金不足）";
                        }
                        alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");
                        //插入被赔付的数据，预*********************************
                        dr = dsDZB.Tables[0].Select("Number = '1304000051'")[0];
                        //Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                       
                        re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (re[0].ToString() == "ok")
                        {
                            if (re[1] != null)
                            {
                                Number = re[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                            return dsreturn;
                        }
                        DLYX = dsYC.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString();  //登陆邮箱
                        JSZHLX = dsYC.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString();  //交易账户类型
                        JSBH = dsYC.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString(); //角色编号
                        LYYWLX = "AAA_THDYFHDXXB";   //提货单表名
                        LYDH = dsYC.Tables[0].Rows[i]["Number"].ToString();  //提货单编号
                        LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                        YSLX = dr["YSLX"].ToString();   //运算类型
                        //JE = f_money;   //金额,这个不用算了，用上面罚款的金额
                        XM = dr["XM"].ToString();  //项目
                        XZ = dr["XZ"].ToString();   //性质
                        ZY = dr["ZY"].ToString().Replace("[x1]", "F" + dsYC.Tables[0].Rows[i]["Number"].ToString()).Replace("[x2]", b.ToString());   //摘要，这个要替换
                        JKBH = "暂无";
                        QTBZ = "暂无";
                        SJLX = dr["SJLX"].ToString();  //数据类型
                        if (JE == 0.00)
                        {
                            ZY += "（履约保证金不足）";
                        }
                        alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");

                        //计算延迟天数和有效条数
                        Daynum++;
                        dsYC.Tables[0].Rows[i]["是否有延迟"] = "是";

                    }
                }

                //bool rr = DbHelperSQL.ExecSqlTran(alsql);
                if (alsql == null || alsql.Count <=0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有要处理的延迟发货数据!";
                    return dsreturn;
                }
                Hashtable htCS2 = I_DBL.RunParam_SQL(alsql);
                bool rr = (bool)htCS2["return_float"];
                //bool rr = true;
                if (rr)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "处理了" + dsYC.Tables[0].Select("是否有延迟='是'").Length.ToString() + "条延迟发货提货单，共计" + Daynum.ToString() + "天延迟!";
                    return dsreturn;

                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "延迟发货数据生成的SQL语句执行未成功!";
                    return dsreturn;
                }


            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有要处理的延迟发货数据!";
                return dsreturn;
            }
        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }

    }
    #endregion

    #region//计算履约保证金不足的合同
    /// <summary>
    /// 计算履约保证金不足的合同
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public static DataSet RunLYBZJbuzu()
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            #region//动态参数表
            double lybzjbzbl = 0.00;
           object[] re = IPC.Call("平台动态参数", new string[] { ""});
           if (re[0].ToString() == "ok")
           {
               //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
               
               if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
               {
                   //找出履约保证金不足需要废标的标的
                   lybzjbzbl = Convert.ToDouble(((DataSet)re[1]).Tables[0].Rows[0]["履约保证金不足比率"].ToString());
                   if (lybzjbzbl == 0)
                   {
                       dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                       dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "履约保证金不足废标参数获取错误!";
                       return dsreturn;
                   }
               }
               else
               {
                   dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                   dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "履约保证金不足废标参数获取错误!";
                   return dsreturn;
               }
           }
           else
           {
               //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
               dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
               dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
               return dsreturn;
           }
            #endregion

           Hashtable returnHT = I_DBL.RunProc("select * from ( select '履约保证金金额'=Z_LYBZJJE,     '两项延迟总金额'=(select isnull(sum(LS.JE),0.00) from AAA_ZKLSMXB as LS  left join AAA_THDYFHDXXB as TTT on LS.LYDH = TTT.Number  where LS.XM='违约赔偿金' and  LS.XZ in ( '超过最迟发货日后录入发货信息','发货单生成后5日内未录入发票邮寄信息') and LS.SJLX='预' and TTT.ZBDBXXBBH = ZZZ.Number),     '是否已解冻过订金'=(select count(*)  from AAA_ZKLSMXB as ZK where ZK.LYDH=ZZZ.Number and ZK.LYYWLX='AAA_ZBDBXXB'   and ZK.XM='订金' and ZK.XZ='合同期内下达完全部提货单解冻' and ZK.SJLX='实') ,          ZZZ.Y_YSYDDDLYX,ZZZ.Y_YSYDDJSZHLX,ZZZ.Y_YSYDDMJJSBH,ZZZ.Number,ZZZ.Z_DJJE,ZZZ.Z_HTBH  from AAA_ZBDBXXB as ZZZ  where   ZZZ.Z_HTZT = '定标'  ) as tab11       where convert(float,履约保证金金额-两项延迟总金额)/convert(float,履约保证金金额) < convert(float," + lybzjbzbl.ToString() + ")  ","");
           if (!(bool)returnHT["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT["return_errmsg"].ToString();
                return dsreturn;
            }
            DataSet dsFB = (DataSet)returnHT["return_ds"];
            //string aaa = Convert.ToDouble(htParameterInfo["履约保证金不足比率"]).ToString();
            if (dsFB != null && dsFB.Tables[0].Rows.Count > 0)
            {
                //防重复处理，记录应变更的余额
                Hashtable HTyue = new Hashtable();
                //要执行的sql语句
                ArrayList alsql = new ArrayList();
                //对照表
                Hashtable returnHT1 =I_DBL.RunProc("select * from AAA_moneyDZB where Number in ('1304000021')","");
                if (!(bool)returnHT1["return_float"])
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT1["return_errmsg"].ToString();
                    return dsreturn;
                }
                DataSet dsDZB = (DataSet)returnHT1["return_ds"];
                for (int i = 0; i < dsFB.Tables[0].Rows.Count; i++)
                {

                    //解冻订金
                    DataRow dr = dsDZB.Tables[0].Select("Number = '1304000021'")[0];
                    //string Number = jhjx_PublicClass.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string Number = "";
                    re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                    if (re[0].ToString() == "ok")
                    {
                        if (re[1] != null)
                        {
                            Number = re[1].ToString();
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                            return dsreturn;
                        }
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                        return dsreturn;
                    }
                    string DLYX = dsFB.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString();  //登陆邮箱
                    string JSZHLX = dsFB.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString();  //交易账户类型
                    string JSBH = dsFB.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString(); //角色编号
                    string LYYWLX = "AAA_ZBDBXXB";   //中标定标表名
                    string LYDH = dsFB.Tables[0].Rows[i]["Number"].ToString();  //中标定标编号
                    string LSCSSJ = DateTime.Now.ToString();  //流水产生时间
                    string YSLX = dr["YSLX"].ToString();   //运算类型
                    double JE = Convert.ToDouble(dsFB.Tables[0].Rows[i]["Z_DJJE"]);   //订金金额
                    string XM = dr["XM"].ToString();  //项目
                    string XZ = dr["XZ"].ToString();   //性质
                    string ZY = dr["ZY"].ToString().Replace("[x1]", dsFB.Tables[0].Rows[i]["Z_HTBH"].ToString());   //摘要，这个要替换
                    string JKBH = "暂无";
                    string QTBZ = "暂无";
                    string SJLX = dr["SJLX"].ToString();  //数据类型

                    if (dsFB.Tables[0].Rows[i]["是否已解冻过订金"].ToString().Trim() == "0")
                    {
                        alsql.Add("INSERT INTO [AAA_ZKLSMXB] ([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX])  VALUES ('" + Number + "','" + DLYX + "','" + JSZHLX + "','" + JSBH + "','" + LYYWLX + "','" + LYDH + "','" + LSCSSJ + "','" + YSLX + "','" + JE + "','" + XM + "','" + XZ + "','" + ZY + "','" + JKBH + "','" + QTBZ + "','" + SJLX + "')");

                        //更新余额
                        if (!HTyue.ContainsKey(DLYX))
                        {
                            HTyue[DLYX] = JE;
                        }
                        else
                        {
                            HTyue[DLYX] = Convert.ToDouble(HTyue[DLYX]) + JE;
                        }
                    }

                    //更新中标定标信息表状态
                    alsql.Add("update AAA_ZBDBXXB set Z_HTZT = '定标合同终止',Z_QPZT='清盘中',Z_QPKSSJ='" + LSCSSJ + "' where  Number = '" + LYDH + "' ");


                }
                //执行sql语句和处理可用余额
                //bool rr = true;
                //bool rr = DbHelperSQL.ExecSqlTran(alsql);
                Hashtable returnHT2 = I_DBL.RunParam_SQL(alsql);
                bool rr = (bool)returnHT2["return_float"];
                if (rr)
                {                    
                    //遍历处理余额
                    foreach (DictionaryEntry de in HTyue)
                    {
                        FreezeMoneyT(de.Key.ToString(), de.Value.ToString(), "+");
                    }
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "履约保证金不足废标" + dsFB.Tables[0].Rows.Count.ToString() + "条被处理成功!";
                    return dsreturn;
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "履约保证金不足废标SQL语句执行失败!";
                    return dsreturn;
                }

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有要处理的履约保证金不足废标数据!";
                return dsreturn;
            }
        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }
    }

    /// <summary>
    /// 模拟冻结或解冻或增加或减少托管账户中的用户资金(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="money">金额</param>
    /// <param name="IsF">+、-</param>
    /// <returns></returns>
    public static bool FreezeMoneyT(string dlyx, string money, string IsF)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable returnHT = I_DBL.RunProc(" update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE " + IsF + " " + money + " where B_DLYX='" + dlyx + "'");
        return (bool)returnHT["return_float"];
    }
    #endregion

    #region//提醒后自动签收
    public static DataSet MRWYYQS()
    {  
        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "默认无异议收货未执行任何操作！" });

        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            int num = 0;
            int errnum = 0;
            string fhd = "";
            string strSQL = "select DATEDIFF(dd,t.F_QMJQSQRCZSJ,getdate()) 提请签收后天数,t.Number,('T'+t.Number) 提货单编号,('F'+t.Number) 发货单号,t.ZBDBXXBBH as 中标定标单号,d.Z_SPBH as 商品编号,d.Z_SPMC as 商品名称,d.Z_GG as 规格,t.ZBDJ as 定标价格,t.T_THSL as 提货数量,T_DJHKJE as 发货金额,DL.I_JYFMC as 卖家名称,d.Z_HTBH as 电子购货合同,d.Y_YSYDDMJJSBH as 买家角色编号 ,d.Y_YSYDDDLYX 买家登录邮箱,'买家结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB a where a.J_BUYJSBH=d.Y_YSYDDMJJSBH),'买家名称'=(select I_JYFMC from AAA_DLZHXXB a where a.J_BUYJSBH=d.Y_YSYDDMJJSBH),t.F_DQZT as 当前状态,t.F_FHDSCSJ as 发货单生成时间,d.T_YSTBDMJJSBH as 卖家角色编号,d.T_YSTBDDLYX as 卖家登陆邮箱,d.T_YSTBDJSZHLX as 卖家角色账户类型,d.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,d.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,'卖家关联经纪人名称'=(select I_JYFMC from AAA_DLZHXXB a where a.J_JJRJSBH=d.T_YSTBDGLJJRJSBH),'卖家关联经纪人结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB a where a.J_JJRJSBH=d.T_YSTBDGLJJRJSBH),d.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,d.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,'买家关联经纪人名称'=(select I_JYFMC from AAA_DLZHXXB a where a.J_JJRJSBH=d.Y_YSYDDGLJJRJSBH),'买家关联经纪人结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB a where a.J_JJRJSBH=d.Y_YSYDDGLJJRJSBH),d.Z_ZBSL as 定标数量,d.Z_LYBZJJE as 履约保证金金额,d.Z_HTZT as 合同状态 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number left join AAA_DLZHXXB DL on DL.J_SELJSBH=d.T_YSTBDMJJSBH where d.Z_HTZT='定标' and d.Z_QPZT='未开始清盘' and (t.F_DQZT='已录入发货信息' or t.F_DQZT='已录入补发备注') and t.F_QMJQSQRCZSJ is not null and DATEDIFF(dd,t.F_QMJQSQRCZSJ,getdate())>=4";
            //DataSet dsgetinfo = DbHelperSQL.Query(strSQL);

            Hashtable returnHT = I_DBL.RunProc(strSQL, "");
            if (!(bool)returnHT["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT["return_errmsg"].ToString();
                return dsreturn;
            }
            DataSet dsgetinfo = (DataSet)returnHT["return_ds"];
            if (dsgetinfo != null && dsgetinfo.Tables[0].Rows.Count > 0)
            {

                #region//提前获取资金变动明细表中的信息、平台动态参数设定表
                string Strtb = " select  Number,'' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number in ('1304000010','1304000012','1304000014','1304000007','1304000043','1304000042','1304000048','1304000028','1304000037','1304000026','1304000036')";
                Hashtable returnHT1 = I_DBL.RunProc(Strtb, "");
                if (!(bool)returnHT1["return_float"])
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT1["return_errmsg"].ToString();
                    return dsreturn;
                }
                DataSet dshkjd = (DataSet)returnHT1["return_ds"];

                //Hashtable htt = new Hashtable();
                DataRow htt = null;
                object[] re = IPC.Call("平台动态参数", new object[] { ""});
                if (re[0].ToString() == "ok")
                {
                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                    if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                    {
                        htt = ((DataSet)re[1]).Tables[0].Rows[0];
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "平台动态参数获取失败";
                        return dsreturn;
                    }
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                    return dsreturn;
                }
                #endregion
                #region 积分操作准备

                DataTable dtjf = new DataTable("jf");
                DataColumn dc = new DataColumn("合同编号", typeof(string));
                dtjf.Columns.Add(dc);
                dtjf.Clear();
                DataRow drht = null;//dt.NewRow();
                #endregion
                foreach (DataRow drinfo in dsgetinfo.Tables[0].Rows)
                {
                    ArrayList al = new ArrayList();
                    //Hashtable htjhjx;
                    DataTable SenMessData =  SenMessForDataTable();
                    DataRow htjhjx;
                    DataSet lht = new DataSet();
                    //List<Hashtable> lht = new List<Hashtable>();

                    #region//验证买家账户状态
                    //Hashtable hsuser = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString());
                    DataRow hsuser;
                    object[] reUserInfo = IPC.Call("获取账户的相关信息", new string[] { drinfo["买家角色编号"].ToString() });
                    if (reUserInfo[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                        if (reUserInfo[1] != null && ((DataSet)reUserInfo[1]).Tables.Count > 0 && ((DataSet)reUserInfo[1]).Tables[0].Rows.Count>0)
                        {
                            hsuser = ((DataSet)reUserInfo[1]).Tables[0].Rows[0];
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] ="获取买家账户信息失败";
                        return dsreturn;
                        }
                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reUserInfo[1].ToString();
                        return dsreturn;
                    }
                    //验证是开通交易账户
                    if (hsuser["交易账户是否开通"].ToString().Trim() == "否")
                    {
                        continue;
                    }
                    //验证是否休眠
                    if (hsuser["是否休眠"].ToString().Trim() == "是")
                    {
                        continue;
                    }
                    #endregion

                    #region 获取买家卖家账户当前余额
                    //获得当前卖家卖家余额
                    //string Strbuyyue = " select B_ZHDQKYYE  from AAA_DLZHXXB where J_BUYJSBH = '" + drinfo["买家角色编号"].ToString().Trim() + "'  ";
                    //object objbuy = DbHelperSQL.GetSingle(Strbuyyue);
                    double buyyue = 0.00;
                    double sellyue = 0.00;
                    //if (objbuy != null && objbuy.ToString().Trim() != "")
                    //{
                    //    buyyue = Convert.ToDouble(objbuy.ToString().Trim());
                    //}
                    //else
                    //{
                    //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从登陆信息表中查找到任何关于" + drinfo["买家登录邮箱"].ToString().Trim() + "的信息！";
                    //    return dsreturn;
                    //}
                    //string Strsellyue = " select B_ZHDQKYYE  from AAA_DLZHXXB where J_SELJSBH = '" + drinfo["卖家角色编号"].ToString().Trim() + "'  ";
                    //object objsell = DbHelperSQL.GetSingle(Strsellyue);
                    //if (objsell != null && objsell.ToString().Trim() != "")
                    //{
                    //    sellyue = Convert.ToDouble(objsell.ToString().Trim());
                    //}
                    //else
                    //{
                    //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从登陆信息表中查找到任何关于" + drinfo["卖家登陆邮箱"].ToString().Trim() + "的信息！";
                    //    return dsreturn;
                    //}
                    #endregion
                    DateTime drnow = DateTime.Now;
                    string Keynumber = "";

                    #region 货物签收解冻买家货款
                    //货物签收解冻买家货款
                    DataRow[] drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000010'");
                    if (drhkjd != null && drhkjd.Length == 1)
                    {
                        string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());

                        //Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");

                        object[] reNumber = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (reNumber[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (!String.IsNullOrEmpty(reNumber[1].ToString()))
                            {
                                Keynumber = reNumber[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reNumber[1].ToString();
                            return dsreturn;
                        }

                        //货物签收解冻买家货款sql
                        string strinsehwqshkjd = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["买家登录邮箱"].ToString().Trim() + "','" + drinfo["买家结算账户类型"].ToString().Trim() + "','" + drinfo["买家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + drinfo["发货金额"].ToString().Trim() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString().Trim() + "','" + drnow + "','监控编号' ) ";

                        al.Add(strinsehwqshkjd);
                        buyyue += Convert.ToDouble(drinfo["发货金额"].ToString().Trim());
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从资金表动明细表中查找到任何关于货物签收无疑义解冻货款的信息！";
                        return dsreturn;
                    }
                    #endregion

                    #region 签收后扣减货款
                    //签收后扣减货款
                    drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000012'");
                    if (drhkjd != null && drhkjd.Length == 1)
                    {
                        string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());

                        //Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        object[] reNumber = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (reNumber[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (!String.IsNullOrEmpty(reNumber[1].ToString()))
                            {
                                Keynumber = reNumber[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reNumber[1].ToString();
                            return dsreturn;
                        }
                        //签收后扣减货款sql
                        string strinsehwqshkjd = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["买家登录邮箱"].ToString().Trim() + "','" + drinfo["买家结算账户类型"].ToString().Trim() + "','" + drinfo["买家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + drinfo["发货金额"].ToString().Trim() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString().Trim() + "','" + drnow + "','监控编号' ) ";

                        al.Add(strinsehwqshkjd);
                        buyyue -= Convert.ToDouble(drinfo["发货金额"].ToString().Trim());
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从资金表动明细表中查找到任何关于货物签收无疑义签收扣减货款的信息！";
                        return dsreturn;
                    }
                    #endregion

                    #region 向卖家支付货款
                    //向卖家支付货款
                    drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000014'");
                    if (drhkjd != null && drhkjd.Length == 1)
                    {
                        string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());
                        //Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        object[] reNumber = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (reNumber[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (!String.IsNullOrEmpty(reNumber[1].ToString()))
                            {
                                Keynumber = reNumber[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reNumber[1].ToString();
                            return dsreturn;
                        }
                        //string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();
                        string jszhlx = "";
                        reUserInfo = IPC.Call("获取账户的相关信息", new string[] { drinfo["卖家角色编号"].ToString() });
                        if (reUserInfo[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (reUserInfo[1] != null && ((DataSet)reUserInfo[1]).Tables.Count > 0 && ((DataSet)reUserInfo[1]).Tables[0].Rows.Count > 0)
                            {
                                jszhlx = ((DataSet)reUserInfo[1]).Tables[0].Rows[0]["结算账户类型"].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取卖家账户信息失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reUserInfo[1].ToString();
                            return dsreturn;
                        }
                        //向卖家支付货款sql
                        string strinsehwqszfhk = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + drinfo["发货金额"].ToString().Trim() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString().Trim() + "','" + drnow + "','监控编号' ) ";
                        al.Add(strinsehwqszfhk);
                        sellyue += Convert.ToDouble(drinfo["发货金额"].ToString().Trim());

                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从资金表动明细表中查找到任何关于货物签收无疑义签收向卖家支付货款的信息！";
                        return dsreturn;
                    }

                    #endregion

                    #region 扣减卖家技术服务费
                    //扣减卖家技术服务费 

                    double jsfwufei = Convert.ToDouble((Convert.ToDouble(drinfo["发货金额"].ToString().Trim()) * Convert.ToDouble(htt["签收技术服务费比率"].ToString().Trim())).ToString("#0.00"));

                    drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000007'");
                    if (drhkjd != null && drhkjd.Length > 0)
                    {
                        string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());
                        //Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        //string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                        object[] reNumber = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (reNumber[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (!String.IsNullOrEmpty(reNumber[1].ToString()))
                            {
                                Keynumber = reNumber[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reNumber[1].ToString();
                            return dsreturn;
                        }
                       
                        string jszhlx = "";
                        reUserInfo = IPC.Call("获取账户的相关信息", new string[] { drinfo["卖家角色编号"].ToString() });
                        if (reUserInfo[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (reUserInfo[1] != null && ((DataSet)reUserInfo[1]).Tables.Count > 0 && ((DataSet)reUserInfo[1]).Tables[0].Rows.Count > 0)
                            {
                                jszhlx = ((DataSet)reUserInfo[1]).Tables[0].Rows[0]["结算账户类型"].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取卖家账户信息失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reUserInfo[1].ToString();
                            return dsreturn;
                        }

                        //扣减卖家技术服务费sql
                        string strinsehwqszfhk = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + jsfwufei.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                        al.Add(strinsehwqszfhk);
                        sellyue -= jsfwufei;

                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从资金表动明细表中查找到任何关于货物签收无疑义签收扣减技术服务费的基本信息！";
                        return dsreturn;
                    }
                    #endregion

                    #region  计算经纪人收益
                    //来自买家的经纪人收益
                    string buyjjrsy = "";
                    drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000043'");
                    if (drhkjd != null && drhkjd.Length == 1)
                    {
                        string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x2]", drinfo["发货单号"].ToString().Trim());
                        zy = zy.ToString().Trim().Replace("[x1]", drinfo["买家登录邮箱"].ToString());
                        //Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        //string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家关联经纪人角色编号"].ToString())["结算账户类型"].ToString();

                        object[] reNumber = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (reNumber[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (!String.IsNullOrEmpty(reNumber[1].ToString()))
                            {
                                Keynumber = reNumber[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reNumber[1].ToString();
                            return dsreturn;
                        }

                        string jszhlx = "";
                        reUserInfo = IPC.Call("获取账户的相关信息", new string[] { drinfo["买家关联经纪人角色编号"].ToString() });
                        if (reUserInfo[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (reUserInfo[1] != null && ((DataSet)reUserInfo[1]).Tables.Count > 0 && ((DataSet)reUserInfo[1]).Tables[0].Rows.Count > 0)
                            {
                                jszhlx = ((DataSet)reUserInfo[1]).Tables[0].Rows[0]["结算账户类型"].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取买家关联经纪人信息失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reUserInfo[1].ToString();
                            return dsreturn;
                        }

                        buyjjrsy = (jsfwufei * Convert.ToDouble(htt["经纪人收益买家比率"].ToString().Trim())).ToString("#0.00");


                        //来自买家的经纪人收益sql
                        string strinsehwqsbuyjjrsy = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["买家关联经纪人邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["买家关联经纪人角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + buyjjrsy.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                        al.Add(strinsehwqsbuyjjrsy);

                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从资金表动明细表中查找到任何关于来自买方的经纪人收益的基本信息！";
                        return dsreturn;
                    }


                    //来自卖家的经纪人收益
                    string buyjjrsell = "";
                    drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000042'");
                    if (drhkjd != null && drhkjd.Length == 1)
                    {
                        string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x2]", drinfo["发货单号"].ToString().Trim());
                        zy = zy.ToString().Trim().Replace("[x1]", drinfo["卖家登陆邮箱"].ToString().Trim());
                        //Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        //string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家关联经纪人角色编号"].ToString())["结算账户类型"].ToString();

                        object[] reNumber = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (reNumber[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (!String.IsNullOrEmpty(reNumber[1].ToString()))
                            {
                                Keynumber = reNumber[1].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reNumber[1].ToString();
                            return dsreturn;
                        }

                        string jszhlx = "";
                        reUserInfo = IPC.Call("获取账户的相关信息", new string[] { drinfo["卖家关联经纪人角色编号"].ToString() });
                        if (reUserInfo[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (reUserInfo[1] != null && ((DataSet)reUserInfo[1]).Tables.Count > 0 && ((DataSet)reUserInfo[1]).Tables[0].Rows.Count > 0)
                            {
                                jszhlx = ((DataSet)reUserInfo[1]).Tables[0].Rows[0]["结算账户类型"].ToString();
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取卖家关联经纪人信息失败";
                                return dsreturn;
                            }
                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reUserInfo[1].ToString();
                            return dsreturn;
                        }



                        buyjjrsell = (jsfwufei * Convert.ToDouble(htt["经纪人收益卖家比率"].ToString().Trim())).ToString("#0.00");


                        //来自卖家的经纪人收益sql
                        string strinsehwqsselljjrsy = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家关联经纪人邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家关联经纪人角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + buyjjrsell.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                        al.Add(strinsehwqsselljjrsy);

                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从资金表动明细表中查找到任何关于来自卖方的经纪人收益的基本信息！";
                        return dsreturn;
                    }

                    #endregion


                    #region 满足买家无异议签收数量=定标数量是单据处理
                    int zbNum = Convert.ToInt32(drinfo["定标数量"].ToString().Trim());
                    int thbc = Convert.ToInt32(drinfo["提货数量"].ToString().Trim());
                    int ythNum = 0;
                    string StrgetyithNum = "select  isnull(SUM (ISNULL(T_THSL,0)),0) as  已提货数量,ZBDBXXBBH     from AAA_THDYFHDXXB where ZBDBXXBBH = '" + drinfo["中标定标单号"].ToString() + "' and F_DQZT in ( '无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货','卖家主动退货' ) group by ZBDBXXBBH";
                    //DataSet dsyth = DbHelperSQL.Query(StrgetyithNum);
                    Hashtable reyth = I_DBL.RunProc(StrgetyithNum,"");
                    if (!(bool)reyth["return_float"])
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reyth["return_errmsg"].ToString();
                        return dsreturn;
                    }
                    DataSet dsyth = (DataSet)reyth["return_ds"];
                    if (dsyth != null && dsyth .Tables.Count>0 && dsyth.Tables[0].Rows.Count > 0)
                    {
                        ythNum = Convert.ToInt32(dsyth.Tables[0].Rows[0]["已提货数量"].ToString());
                    }

                    if (zbNum == thbc + ythNum)
                    {
                        #region 解冻卖家履约保证金
                        //解冻卖家履约保证金
                        double lvbzj = Convert.ToDouble(drinfo["履约保证金金额"].ToString().Trim());
                        //无异议提货解冻卖家履约保证金
                        drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000048'");
                        if (drhkjd != null && drhkjd.Length > 0)
                        {
                            string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                            //Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                            //string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                            object[] reNumber = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                            if (reNumber[0].ToString() == "ok")
                            {
                                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                if (!String.IsNullOrEmpty(reNumber[1].ToString()))
                                {
                                    Keynumber = reNumber[1].ToString();
                                }
                                else
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                    return dsreturn;
                                }
                            }
                            else
                            {
                                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reNumber[1].ToString();
                                return dsreturn;
                            }

                            string jszhlx = "";
                            reUserInfo = IPC.Call("获取账户的相关信息", new string[] { drinfo["卖家角色编号"].ToString() });
                            if (reUserInfo[0].ToString() == "ok")
                            {
                                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                if (reUserInfo[1] != null && ((DataSet)reUserInfo[1]).Tables.Count > 0 && ((DataSet)reUserInfo[1]).Tables[0].Rows.Count > 0)
                                {
                                    jszhlx = ((DataSet)reUserInfo[1]).Tables[0].Rows[0]["结算账户类型"].ToString();
                                }
                                else
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取卖家信息失败";
                                    return dsreturn;
                                }
                            }
                            else
                            {
                                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reUserInfo[1].ToString();
                                return dsreturn;
                            }


                            //无异议提货解冻卖家履约保证金sql
                            string strinsehwqsjdmjlybzj = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_ZBDBXXB','" + drinfo["中标定标单号"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + drinfo["履约保证金金额"].ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                            al.Add(strinsehwqsjdmjlybzj);
                            sellyue += Convert.ToDouble(drinfo["履约保证金金额"].ToString());
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无异议提货解冻卖家履约保证金信息未找到！";
                            return dsreturn;
                        }
                        #endregion

                        #region 因发票延期或者发货延期的罚单与补偿
                        //获得所有相同中标定标单号的提货单编号的发货单号

                        string GetTHdNumber = " select Number from  AAA_THDYFHDXXB where ZBDBXXBBH = '" + drinfo["中标定标单号"].ToString() + "' ";
                        //DataSet dsfpyq = DbHelperSQL.Query(GetTHdNumber);

                        Hashtable htfpyq= I_DBL.RunProc(GetTHdNumber,"");
                        if (!(bool)htfpyq["return_float"])
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htfpyq["return_errmsg"].ToString();
                            return dsreturn;
                        }
                        DataSet dsfpyq = (DataSet)htfpyq["return_ds"];
                        if (dsfpyq != null && dsfpyq.Tables[0].Rows.Count > 0)
                        {
                            string Strwhere = "";
                            foreach (DataRow dr in dsfpyq.Tables[0].Rows)
                            {
                                Strwhere += "'" + dr["Number"].ToString().Trim() + "',";
                            }

                            // Strwhere = Strwhere.Trim().Remove(Strwhere.Length - 2);
                            Strwhere += " '十全大补丸'";
                            # region 发票延期的扣罚与补赏
                            //获取次投标定标预订单的发票延迟预扣款流水明细
                            double je = 0.00;
                            string Strfpykmc = "select Number, JE from AAA_ZKLSMXB where XM = '违约赔偿金' and XZ = '发货单生成后5日内未录入发票邮寄信息' and LYDH in (" + Strwhere + ") and SJLX='预'";
                            //DataSet dsfpykmc = DbHelperSQL.Query(Strfpykmc);

                            Hashtable htfpykmc = I_DBL.RunProc(Strfpykmc, "");
                            if (!(bool)htfpykmc["return_float"])
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htfpykmc["return_errmsg"].ToString();
                                return dsreturn;
                            }
                            DataSet dsfpykmc = (DataSet)htfpykmc["return_ds"];

                            if (dsfpykmc != null && dsfpykmc.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow drfpykk in dsfpykmc.Tables[0].Rows)
                                {
                                    je += Convert.ToDouble(drfpykk["JE"].ToString().Trim());
                                }

                                //卖家扣罚(发货单生成后5日内未录入发票邮寄信息)

                                drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000028'");
                                if (drhkjd != null && drhkjd.Length > 0)
                                {
                                    string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    //Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    //string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                                    object[] reNumber = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                    if (reNumber[0].ToString() == "ok")
                                    {
                                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                        if (!String.IsNullOrEmpty(reNumber[1].ToString()))
                                        {
                                            Keynumber = reNumber[1].ToString();
                                        }
                                        else
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                            return dsreturn;
                                        }
                                    }
                                    else
                                    {
                                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reNumber[1].ToString();
                                        return dsreturn;
                                    }

                                    string jszhlx = "";
                                    reUserInfo = IPC.Call("获取账户的相关信息", new string[] { drinfo["卖家角色编号"].ToString() });
                                    if (reUserInfo[0].ToString() == "ok")
                                    {
                                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                        if (reUserInfo[1] != null && ((DataSet)reUserInfo[1]).Tables.Count > 0 && ((DataSet)reUserInfo[1]).Tables[0].Rows.Count > 0)
                                        {
                                            jszhlx = ((DataSet)reUserInfo[1]).Tables[0].Rows[0]["结算账户类型"].ToString();
                                        }
                                        else
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取卖家信息失败";
                                            return dsreturn;
                                        }
                                    }
                                    else
                                    {
                                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reUserInfo[1].ToString();
                                        return dsreturn;
                                    }

                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_ZBDBXXB','" + drinfo["中标定标单号"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + je.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                                    al.Add(strinsehwqsfpyqkf);
                                    sellyue -= je;
                                }
                                else
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                    return dsreturn;
                                }

                                //买家补赏(发货单生成后5日内未录入发票邮寄信息)

                                drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000037'");
                                if (drhkjd != null && drhkjd.Length > 0)
                                {
                                    string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    //Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    //string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString())["结算账户类型"].ToString();

                                    object[] reNumber = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                    if (reNumber[0].ToString() == "ok")
                                    {
                                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                        if (!String.IsNullOrEmpty(reNumber[1].ToString()))
                                        {
                                            Keynumber = reNumber[1].ToString();
                                        }
                                        else
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                            return dsreturn;
                                        }
                                    }
                                    else
                                    {
                                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reNumber[1].ToString();
                                        return dsreturn;
                                    }

                                    string jszhlx = "";
                                    reUserInfo = IPC.Call("获取账户的相关信息", new string[] { drinfo["买家角色编号"].ToString() });
                                    if (reUserInfo[0].ToString() == "ok")
                                    {
                                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                        if (reUserInfo[1] != null && ((DataSet)reUserInfo[1]).Tables.Count > 0 && ((DataSet)reUserInfo[1]).Tables[0].Rows.Count > 0)
                                        {
                                            jszhlx = ((DataSet)reUserInfo[1]).Tables[0].Rows[0]["结算账户类型"].ToString();
                                        }
                                        else
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取买家信息失败";
                                            return dsreturn;
                                        }
                                    }
                                    else
                                    {
                                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reUserInfo[1].ToString();
                                        return dsreturn;
                                    }

                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["买家登录邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["买家角色编号"].ToString() + "','AAA_ZBDBXXB','" + drinfo["中标定标单号"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + je.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                                    al.Add(strinsehwqsfpyqkf);
                                    buyyue += je;
                                }
                                else
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                    return dsreturn;
                                }



                            }
                            #endregion

                            #region  卖家发货延期的扣罚与补偿
                            //获取次投标定标预订单的发发货延迟预扣款流水明细
                            double hwje = 0.00;
                            string Strhwykmc = "select Number, JE from AAA_ZKLSMXB where XM = '违约赔偿金' and XZ = '超过最迟发货日后录入发货信息' and LYDH in (" + Strwhere + ") and SJLX='预'";
                            //DataSet dshwykmc = DbHelperSQL.Query(Strhwykmc);

                            Hashtable hthwykmc = I_DBL.RunProc(Strhwykmc, "");
                            if (!(bool)hthwykmc["return_float"])
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = hthwykmc["return_errmsg"].ToString();
                                return dsreturn;
                            }
                            DataSet dshwykmc = (DataSet)hthwykmc["return_ds"];

                            if (dshwykmc != null && dshwykmc.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow drfpykk in dshwykmc.Tables[0].Rows)
                                {
                                    hwje += Convert.ToDouble(drfpykk["JE"].ToString().Trim());
                                }

                                //卖家扣罚(发货单生成后5日内未录入发票邮寄信息)

                                drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000026'");
                                if (drhkjd != null && drhkjd.Length > 0)
                                {
                                    string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    //Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    //string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                                    object[] reNumber = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                    if (reNumber[0].ToString() == "ok")
                                    {
                                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                        if (!String.IsNullOrEmpty(reNumber[1].ToString()))
                                        {
                                            Keynumber = reNumber[1].ToString();
                                        }
                                        else
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                            return dsreturn;
                                        }
                                    }
                                    else
                                    {
                                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reNumber[1].ToString();
                                        return dsreturn;
                                    }

                                    string jszhlx = "";
                                    reUserInfo = IPC.Call("获取账户的相关信息", new string[] { drinfo["卖家角色编号"].ToString() });
                                    if (reUserInfo[0].ToString() == "ok")
                                    {
                                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                        if (reUserInfo[1] != null && ((DataSet)reUserInfo[1]).Tables.Count > 0 && ((DataSet)reUserInfo[1]).Tables[0].Rows.Count > 0)
                                        {
                                            jszhlx = ((DataSet)reUserInfo[1]).Tables[0].Rows[0]["结算账户类型"].ToString();
                                        }
                                        else
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取卖家信息失败";
                                            return dsreturn;
                                        }
                                    }
                                    else
                                    {
                                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reUserInfo[1].ToString();
                                        return dsreturn;
                                    }

                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_ZBDBXXB','" + drinfo["中标定标单号"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + hwje.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                                    al.Add(strinsehwqsfpyqkf);
                                    sellyue -= hwje;
                                }
                                else
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                    return dsreturn;
                                }

                                //买家补赏(发货单生成后5日内未录入发票邮寄信息)

                                drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000036'");
                                if (drhkjd != null && drhkjd.Length > 0)
                                {
                                    string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    //Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    //string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString())["结算账户类型"].ToString();

                                    object[] reNumber = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                    if (reNumber[0].ToString() == "ok")
                                    {
                                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                        if (!String.IsNullOrEmpty(reNumber[1].ToString()))
                                        {
                                            Keynumber = reNumber[1].ToString();
                                        }
                                        else
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败";
                                            return dsreturn;
                                        }
                                    }
                                    else
                                    {
                                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reNumber[1].ToString();
                                        return dsreturn;
                                    }

                                    string jszhlx = "";
                                    reUserInfo = IPC.Call("获取账户的相关信息", new string[] { drinfo["买家角色编号"].ToString() });
                                    if (reUserInfo[0].ToString() == "ok")
                                    {
                                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                        if (reUserInfo[1] != null && ((DataSet)reUserInfo[1]).Tables.Count > 0 && ((DataSet)reUserInfo[1]).Tables[0].Rows.Count > 0)
                                        {
                                            jszhlx = ((DataSet)reUserInfo[1]).Tables[0].Rows[0]["结算账户类型"].ToString();
                                        }
                                        else
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取买家信息失败";
                                            return dsreturn;
                                        }
                                    }
                                    else
                                    {
                                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                        
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = reUserInfo[1].ToString();
                                        return dsreturn;
                                    }

                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["买家登录邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["买家角色编号"].ToString() + "','AAA_ZBDBXXB','" + drinfo["中标定标单号"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + hwje.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                                    al.Add(strinsehwqsfpyqkf);
                                    buyyue += hwje;
                                }
                                else
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                    return dsreturn;
                                }



                            }
                            #endregion
                            #region 反写中标定标信息表
                            string Strupdate = " update AAA_ZBDBXXB set Z_HTZT = '定标执行完成' ,  Z_QPZT='清盘结束',Z_QPKSSJ='" + drnow + "',Z_QPJSSJ='" + drnow + "' where Z_HTBH = '" + drinfo["电子购货合同"].ToString() + "' ";
                            al.Add(Strupdate);

                            #endregion

                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到中标定标编号为：" + drinfo["中标定标单号"].ToString() + "的发货单！";
                            return dsreturn;
                        }


                        #endregion


                    }
                    else if (zbNum < thbc + ythNum)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "所有无异议收货的数量大于了总定标数量！";
                        return dsreturn;
                    }



                    #endregion

                    //更改账户余额
                    string StrUpbuy = " update  AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE + " + buyyue + " where B_DLYX = '" + drinfo["买家登录邮箱"].ToString().Trim() + "'  ";
                    al.Add(StrUpbuy);
                    string Strupsell = " update  AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE + " + sellyue + " where B_DLYX = '" + drinfo["卖家登陆邮箱"].ToString().Trim() + "'  ";
                    al.Add(Strupsell);
                    //更改单据状态
                    string Strupda = " update AAA_THDYFHDXXB set F_DQZT='默认无异议收货', F_BUYWYYSHCZSJ='" + drnow + "' where Number='" + drinfo["Number"].ToString() + "' ";
                    al.Add(Strupda);

                    #region 向卖家、买家、经纪人发送提醒
                    //集合经销平台
                    //List<Hashtable> lht = new List<Hashtable>();
                    //htjhjx = new Hashtable();       
                    htjhjx = SenMessData.NewRow();
                    htjhjx["type"] = "集合经销平台";
                    htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
                    //htjhjx["提醒对象用户名"] = drinfo["卖家名称"].ToString();
                    htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
                    htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
                    htjhjx["提醒对象角色类型"] = "卖家";
                    htjhjx["提醒内容文本"] = "您" + drinfo["发货单号"].ToString().Trim() + "发货单，系统自动签收，" + drinfo["发货金额"].ToString() + "元的货款已转付至您的交易账户， " + jsfwufei.ToString("#0.00") + "元的技术服务费已同时扣收，敬请知悉。";
                    htjhjx["创建人"] = drinfo["买家登录邮箱"].ToString();
                    htjhjx["查看地址"] = "";
                    SenMessData.Rows.Add(htjhjx);

                    htjhjx = SenMessData.NewRow();
                    htjhjx["type"] = "集合经销平台";
                    htjhjx["提醒对象登陆邮箱"] = drinfo["买家登录邮箱"].ToString();
                    //htjhjx["提醒对象用户名"] = drinfo["买家名称"].ToString();
                    htjhjx["提醒对象结算账户类型"] = drinfo["买家结算账户类型"].ToString();
                    htjhjx["提醒对象角色编号"] = drinfo["买家角色编号"].ToString();
                    htjhjx["提醒对象角色类型"] = "买家";
                    htjhjx["提醒内容文本"] = "您" + drinfo["发货单号"].ToString().Trim() + "发货单，系统自动签收，" + drinfo["发货金额"].ToString() + "元的货款已转付至相应卖方账户，敬请知悉。";
                    htjhjx["创建人"] = drinfo["买家登录邮箱"].ToString();
                    htjhjx["查看地址"] = "";
                    SenMessData.Rows.Add(htjhjx);

                    htjhjx = SenMessData.NewRow();
                    htjhjx["type"] = "集合经销平台";
                    htjhjx["提醒对象登陆邮箱"] = drinfo["买家关联经纪人邮箱"].ToString();
                    //htjhjx["提醒对象用户名"] = drinfo["买家关联经纪人名称"].ToString();
                    htjhjx["提醒对象结算账户类型"] = drinfo["买家关联经纪人结算账户类型"].ToString();
                    htjhjx["提醒对象角色编号"] = drinfo["买家关联经纪人角色编号"].ToString();
                    htjhjx["提醒对象角色类型"] = "经纪人";
                    htjhjx["提醒内容文本"] = "尊敬的" + drinfo["买家关联经纪人名称"].ToString() + "，您的交易账户于" + drnow.Year.ToString() + "年" + drnow.Month.ToString() + "月" + drnow.Day.ToString() + "日" + drnow.Hour.ToString() + "时" + drnow.Minute.ToString() + "分新增经纪人收益" + buyjjrsy + "元，请继续努力！";
                    htjhjx["创建人"] = drinfo["买家关联经纪人邮箱"].ToString();
                    htjhjx["查看地址"] = "";
                    SenMessData.Rows.Add(htjhjx);

                    htjhjx = SenMessData.NewRow();
                    htjhjx["type"] = "集合经销平台";
                    htjhjx["提醒对象登陆邮箱"] = drinfo["卖家关联经纪人邮箱"].ToString();
                    //htjhjx["提醒对象用户名"] = drinfo["卖家关联经纪人名称"].ToString();
                    htjhjx["提醒对象结算账户类型"] = drinfo["卖家关联经纪人结算账户类型"].ToString();
                    htjhjx["提醒对象角色编号"] = drinfo["卖家关联经纪人角色编号"].ToString();
                    htjhjx["提醒对象角色类型"] = "经纪人";
                    htjhjx["提醒内容文本"] = "尊敬的" + drinfo["卖家关联经纪人名称"].ToString() + "，您的交易账户于" + drnow.Year.ToString() + "年" + drnow.Month.ToString() + "月" + drnow.Day.ToString() + "日" + drnow.Hour.ToString() + "时" + drnow.Minute.ToString() + "分新增经纪人收益" + buyjjrsell + "元，请继续努力！";
                    htjhjx["创建人"] = drinfo["买家关联经纪人邮箱"].ToString();
                    htjhjx["查看地址"] = "";
                    SenMessData.Rows.Add(htjhjx);

                    #endregion
                    if (zbNum == thbc + ythNum)
                    {
                        #region //清盘提醒 向卖家、买家发送提醒
                        //集合经销平台                    
                        htjhjx = SenMessData.NewRow();
                        htjhjx["type"] = "集合经销平台";
                        htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
                        //htjhjx["提醒对象用户名"] = drinfo["卖家名称"].ToString();
                        htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
                        htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
                        htjhjx["提醒对象角色类型"] = "卖家";
                        htjhjx["提醒内容文本"] = "尊敬的" + drinfo["卖家名称"].ToString() + "，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                        htjhjx["创建人"] = drinfo["买家登录邮箱"].ToString();
                        htjhjx["查看地址"] = "";
                        SenMessData.Rows.Add(htjhjx);
                        htjhjx = SenMessData.NewRow();
                        htjhjx["type"] = "集合经销平台";
                        htjhjx["提醒对象登陆邮箱"] = drinfo["买家登录邮箱"].ToString();
                        //htjhjx["提醒对象用户名"] = drinfo["买家名称"].ToString();
                        htjhjx["提醒对象结算账户类型"] = drinfo["买家结算账户类型"].ToString();
                        htjhjx["提醒对象角色编号"] = drinfo["买家角色编号"].ToString();
                        htjhjx["提醒对象角色类型"] = "买家";
                        htjhjx["提醒内容文本"] = "尊敬的" + drinfo["买家名称"].ToString() + "，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                        htjhjx["创建人"] = drinfo["买家登录邮箱"].ToString();
                        htjhjx["查看地址"] = "";
                        SenMessData.Rows.Add(htjhjx);
                        #endregion
                    }
                    lht.Tables.Add(SenMessData);
                    if (al != null && al.Count > 0)
                    {
                        try
                        {
                            Hashtable htAllSQL = I_DBL.RunParam_SQL(al);
                            if ((bool)htAllSQL["return_float"])
                            {
                                num++;
                                #region//发送提醒信息
                                object[] reAllSenMess = IPC.Call("发送提醒", new object[] { lht });
                                if (reAllSenMess[0].ToString() == "ok")
                                {
                                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。                                    
                                    if (!string.IsNullOrEmpty(re[1].ToString()) && re[1].ToString() == "发送提醒成功")
                                    {
                                        
                                    }
                                }
                                else
                                {
                                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                                    string reFF = reAllSenMess[1].ToString();
                                }
                                #endregion

                                #region 交易方信用变化情况 edittime 2013-09// wyh２０１４．０７．２８　ａｄｄ

                              
                                //string sqlstr = "select Z_QPZT  from  AAA_ZBDBXXB where  Z_HTBH='" + drinfo["电子购货合同"].ToString().Trim() + "'";
                                //Hashtable HTbszt = I_DBL.RunProc(sqlstr, "");
                                //if ((bool)HTbszt["return_float"])
                                //{
                                //    string bszt = ((DataSet)HTbszt["return_ds"]).Tables[0].Rows[0]["Z_QPZT"].ToString();
                                //    if (bszt != null && bszt.ToString().Trim() == "清盘结束")
                                //    {
                                        drht = dtjf.NewRow();
                                        drht["合同编号"] = drinfo["电子购货合同"].ToString().Trim();
                                        dtjf.Rows.Add(drht);
                                //    }

                                //}

                                #endregion
                            }
                            
                        }
                        catch (Exception)
                        {
                            errnum++;
                            fhd += "F" + drinfo["Number"].ToString() + "、";
                        }
                    }

                }

                #region 积分计算  wyh 2014.07.28 
                dtjf.AcceptChanges();
                IPC.Call("信用等级积分处理", new object[] { "提醒后自动签收", dtjf });
                #endregion

                if (errnum > 0)
                {
                    fhd = fhd.TrimEnd('、');
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "共正确执行了" + num.ToString() + "条默认无异议收货的信息；发货单号为：" + fhd + "，共" + errnum + "条未正确执行！";
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "共正确执行了" + num.ToString() + "条默认无异议收货的信息；";
                }

                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";



                return dsreturn;
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未获取到需要默认无异议收货的信息！";
                return dsreturn;
            }

        }
        catch (Exception e)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";
            return dsreturn;
        }

    }

    public static DataTable SenMessForDataTable()
    {
        DataTable SenMessData = new DataTable();
        SenMessData.Columns.Add("type");
        SenMessData.Columns.Add("提醒对象登陆邮箱");
        SenMessData.Columns.Add("提醒对象结算账户类型");
        SenMessData.Columns.Add("提醒对象角色编号");
        SenMessData.Columns.Add("提醒对象角色类型");
        SenMessData.Columns.Add("提醒内容文本");
        SenMessData.Columns.Add("创建人");
        SenMessData.Columns.Add("查看地址");
        return SenMessData;
    }
    #endregion

    #region//计算经纪人收益扣税

    /// <summary>
    /// 执行每月经纪人扣税后台监控
    /// </summary>
    /// <param name="dsReturn"></param>
    /// <returns></returns>
    public static DataSet ServerMYJJRKS()
    {
        ////1、匹配每月2号凌晨
        //if (!DateTime.Now.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-02")))
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人扣税后台监控只有每月2号执行，当前时间为" + DateTime.Now.ToString("yyyy年MM月dd日") + "不能执行该监控！";
        //    return dsreturn;
        //}       

        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "默认无异议收货未执行任何操作！" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        
        string sqlCount = "select COUNT(*) from AAA_JJRSYKSMXB";
        Hashtable returnHT = I_DBL.RunProc(sqlCount,"");
        if (!(bool)returnHT["return_float"])
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ 详情：" + returnHT["return_errmsg"].ToString();
            return dsreturn;
        }
        //int countNumber = Convert.ToInt32(obj);

        //扣税公式
        string strKSGS = "1、您的经纪人收益属于劳务报酬所得，劳务报酬所得，属于同一项目连续性收入的，以一个月内取得的收入为一次。2、自然月度未满的经纪人收益，由于无法确定应纳税额和税后所得，暂不计入您的经纪人收益余额。因此，你的经纪人交易账户目前可支取的收益余额未包含本月的经纪人收益。3、.劳务所得税率计算标准及收益计算公式A、0<收入<=800，应纳税额=0，税后所得=收入，B、800<收入<=4000，应纳税额=（收入-800）*0.2，税后所得=收入-应纳税额C、4000<收入<=25000，应纳税额=收入*0.8*0.2，税后所得=收入-应纳税额D、25000<收入<=62500，应纳税额=收入*0.8*0.3-2000，税后所得=收入-应纳税额E、收入>62500 ，应纳税额=收入*0.8*0.4-7000，税后所得=收入-应纳税额";

        //DataSet ds_ZKMX = DbHelperSQL.Query("select * from AAA_moneyDZB where XZ in ('自然人月收益','所得税扣缴')");
        Hashtable returnHT_ZKMX = I_DBL.RunProc("select * from AAA_moneyDZB where XZ in ('自然人月收益','所得税扣缴')", "");
        if (!(bool)returnHT_ZKMX["return_float"])
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ 详情：" + returnHT_ZKMX["return_errmsg"].ToString();
            return dsreturn;
        }
        DataSet ds_ZKMX = (DataSet)returnHT_ZKMX["return_ds"];
        DataRow[] dr_ZRRYSY = ds_ZKMX.Tables[0].Select("number='1304000045'");//自然人月收益--实
        DataRow[] dr_SDSKJ = ds_ZKMX.Tables[0].Select("number='1304000044'");//所得税扣缴--实




        #region //月度经纪人收益调整方法更改
        try
        {
            DateTime dateTimeStart = Convert.ToDateTime("2012-05-01");//确定初始时间
            ArrayList arrayList = new ArrayList();
            //for (DateTime dateEnd = DateTime.Now.AddMonths(-1); dateEnd.CompareTo(dateTimeStart) >= 0; dateEnd = dateEnd.AddMonths(-1))
            //{
            for (DateTime dateEnd = DateTime.Now; dateEnd.CompareTo(dateTimeStart) >= 0; dateEnd = dateEnd.AddMonths(-1))
            {
                Hashtable hashTable = GetMonthStartEnd(dateEnd);
                bool isExists = GetJJRSYKSMXB(Convert.ToDateTime(hashTable["月初时间"]).Year.ToString(), Convert.ToDateTime(hashTable["月初时间"]).Month.ToString());

                if (isExists)//此月份收益数据没有计算
                {
                    DataSet dataSet = ServerMYJJRKS_HQSY(hashTable["月初时间"].ToString(), hashTable["月末时间"].ToString());
                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                        {
                            if (dataSet.Tables[0].Rows[i]["第三方存管状态"].ToString() == "开通")//第三方存管状态为“已开通”
                            {
                                Hashtable hashTableJE = ServerMYJJRKS_JXKS(Convert.ToDouble(dataSet.Tables[0].Rows[i]["金额"]));
                                //获取经纪人收益扣税明细表的Number值
                                //string strNumber = PublicClass2013.GetNextNumberZZ("AAA_JJRSYKSMXB", "");
                                string strNumber = "";
                                object[] re = IPC.Call("获取主键", new string[] { "AAA_JJRSYKSMXB", "" });
                               if (re[0].ToString() == "ok")
                               {
                                   //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                   if (re[1] != null)
                                   {
                                       strNumber = (string)(re[1]);
                                   }
                                   else
                                   {
                                       dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                       dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ " ;
                                       return dsreturn;
                                   }
                                  
                               }
                               else
                               {
                                   //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                                   
                                   throw ((Exception)re[1]);
                               }


                                //向经纪人收益扣税明细表中插入数据
                                string strSqlAddJJRSYKSMXB = "insert AAA_JJRSYKSMXB(Number,DLYX,JSZHLX,JJRJSBH,SYKSZHRQ,SYKSNF,SYKSYF,KSSYJE,SKJE,SHJE,KSBZ,SFCGZRZH,JKLSH,SFYJJSWC) values('" + strNumber + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','" + DateTime.Now.ToString() + "','" + Convert.ToDateTime(hashTable["月初时间"]).Year.ToString() + "','" + Convert.ToDateTime(hashTable["月初时间"]).Month.ToString() + "','" + Convert.ToDouble(hashTableJE["收益"]).ToString("#0.00") + "','" + Convert.ToDouble(hashTableJE["税额"]).ToString("#0.00") + "','" + Convert.ToDouble(hashTableJE["税后金额"]).ToString("#0.00") + "','" + strKSGS + "','确定','" + strNumber + "','已计算')";
                                arrayList.Add(strSqlAddJJRSYKSMXB);
                                // 自然人月收益
                                string str_dr_ZRRYSY = dr_ZRRYSY[0]["ZY"].ToString().Replace("[x1]", Convert.ToDateTime(hashTable["月初时间"]).Year.ToString()).Replace("[x2]", Convert.ToDateTime(hashTable["月初时间"]).Month.ToString()).ToString();
                                //获取账款流水明细表（的Number值   
                                //string strNumberZRRYSY = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string strNumberZRRYSY = "";
                                re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                if (re[0].ToString() == "ok")
                                {
                                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                    if (re[1] != null)
                                    {
                                        strNumberZRRYSY = (string)(re[1]);
                                    }
                                    else
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ ";
                                        return dsreturn;
                                    }

                                }
                                else
                                {
                                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                                   
                                    throw ((Exception)re[1]);
                                }
                                string strSqlZRRYSY = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberZRRYSY + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_ZRRYSY[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["收益"]).ToString("#0.00") + "','" + dr_ZRRYSY[0]["XM"].ToString() + "','" + dr_ZRRYSY[0]["XZ"].ToString() + "','" + str_dr_ZRRYSY + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
                                arrayList.Add(strSqlZRRYSY);
                                //所得税扣缴
                                string str_SDSKJ = dr_SDSKJ[0]["ZY"].ToString().Replace("[x1]", Convert.ToDateTime(hashTable["月初时间"]).Year.ToString()).Replace("[x2]", Convert.ToDateTime(hashTable["月初时间"]).Month.ToString()).ToString();
                                //string strNumberSDSKJ = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string strNumberSDSKJ = "";
                                re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                if (re[0].ToString() == "ok")
                                {
                                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                    if (re[1] != null)
                                    {
                                        strNumberSDSKJ = (string)(re[1]);
                                    }
                                    else
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ ";
                                        return dsreturn;
                                    }

                                }
                                else
                                {
                                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                                   
                                    throw ((Exception)re[1]);
                                }
                                string strSqlSDSKJ = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberSDSKJ + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_SDSKJ[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["税额"]).ToString("#0.00") + "','" + dr_SDSKJ[0]["XM"].ToString() + "','" + dr_SDSKJ[0]["XZ"].ToString() + "','" + str_SDSKJ + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
                                arrayList.Add(strSqlSDSKJ);
                                //更新登陆账号信息表 账户资金余额
                                string strZHZJYE = "update AAA_DLZHXXB set B_ZHDQKYYE=B_ZHDQKYYE+'" + Convert.ToDouble(hashTableJE["税后金额"]).ToString("#0.00") + "' where B_DLYX='" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "'";
                                arrayList.Add(strZHZJYE);


                            }
                            else
                            {
                                //获取经纪人收益扣税明细表的Number值
                                //string strNumber = PublicClass2013.GetNextNumberZZ("AAA_JJRSYKSMXB", "");
                                string strNumber = "";
                                object[] re = IPC.Call("获取主键", new string[] { "AAA_JJRSYKSMXB", "" });
                                if (re[0].ToString() == "ok")
                                {
                                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                    if (re[1] != null)
                                    {
                                        strNumber = (string)(re[1]);
                                    }
                                    else
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ ";
                                        return dsreturn;
                                    }

                                }
                                else
                                {
                                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                                   
                                    throw ((Exception)re[1]);
                                }
                                //向经纪人收益扣税明细表中插入数据
                                string strSqlAddJJRSYKSMXB = "insert AAA_JJRSYKSMXB(Number,DLYX,JSZHLX,JJRJSBH,SYKSZHRQ,SYKSNF,SYKSYF,KSSYJE,SKJE,SHJE,KSBZ,SFCGZRZH,JKLSH,SFYJJSWC) values('" + strNumber + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','" + DateTime.Now.ToString() + "','" + Convert.ToDateTime(hashTable["月初时间"]).Year.ToString() + "','" + Convert.ToDateTime(hashTable["月初时间"]).Month.ToString() + "','0','0','0','" + strKSGS + "','不确定','" + strNumber + "','未计算')";
                                arrayList.Add(strSqlAddJJRSYKSMXB);

                            }
                        }
                    }
                }
                else//此月份收益数据已经计算过
                {
                    DataSet dataSet = ServerMYJJRKS_HQSYMX(hashTable["月初时间"].ToString(), hashTable["月末时间"].ToString(), Convert.ToDateTime(hashTable["月初时间"]).Year.ToString(), Convert.ToDateTime(hashTable["月初时间"]).Month.ToString());
                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                        {
                            if (dataSet.Tables[0].Rows[i]["第三方存管状态"].ToString() == "开通")
                            {
                                Hashtable hashTableJE = ServerMYJJRKS_JXKS(Convert.ToDouble(dataSet.Tables[0].Rows[i]["金额"]));
                                string strNumber = dataSet.Tables[0].Rows[i]["Number"].ToString();
                                //向经纪人收益扣税明细表中插入数据
                                string strSqlAddJJRSYKSMXB = "update AAA_JJRSYKSMXB set KSSYJE='" + hashTableJE["收益"].ToString() + "',SKJE='" + hashTableJE["税额"].ToString() + "',SHJE='" + hashTableJE["税后金额"].ToString() + "',SFYJJSWC='已计算',JKLSH='" + strNumber + "',SFCGZRZH='确定' where Number='" + strNumber + "'";
                                arrayList.Add(strSqlAddJJRSYKSMXB);
                                // 自然人月收益
                                string str_dr_ZRRYSY = dr_ZRRYSY[0]["ZY"].ToString().Replace("[x1]", Convert.ToDateTime(hashTable["月初时间"]).Year.ToString()).Replace("[x2]", Convert.ToDateTime(hashTable["月初时间"]).Month.ToString()).ToString();
                                //获取账款流水明细表（的Number值   
                                //string strNumberZRRYSY = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string strNumberZRRYSY = "";
                                object[] re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                if (re[0].ToString() == "ok")
                                {
                                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                    if (re[1] != null)
                                    {
                                        strNumberZRRYSY = (string)(re[1]);
                                    }
                                    else
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ ";
                                        return dsreturn;
                                    }

                                }
                                else
                                {
                                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                                   
                                    throw ((Exception)re[1]);
                                }
                                string strSqlZRRYSY = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberZRRYSY + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_ZRRYSY[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["收益"]).ToString("#0.00") + "','" + dr_ZRRYSY[0]["XM"].ToString() + "','" + dr_ZRRYSY[0]["XZ"].ToString() + "','" + str_dr_ZRRYSY + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
                                arrayList.Add(strSqlZRRYSY);
                                //所得税扣缴
                                string str_SDSKJ = dr_SDSKJ[0]["ZY"].ToString().Replace("[x1]", Convert.ToDateTime(hashTable["月初时间"]).Year.ToString()).Replace("[x2]", Convert.ToDateTime(hashTable["月初时间"]).Month.ToString()).ToString();
                                //string strNumberSDSKJ = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string strNumberSDSKJ = "";
                                 re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                if (re[0].ToString() == "ok")
                                {
                                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                    if (re[1] != null)
                                    {
                                        strNumberSDSKJ = (string)(re[1]);
                                    }
                                    else
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ ";
                                        return dsreturn;
                                    }

                                }
                                else
                                {
                                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                                   
                                    throw ((Exception)re[1]);
                                }
                                string strSqlSDSKJ = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberSDSKJ + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_SDSKJ[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["税额"]).ToString("#0.00") + "','" + dr_SDSKJ[0]["XM"].ToString() + "','" + dr_SDSKJ[0]["XZ"].ToString() + "','" + str_SDSKJ + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
                                arrayList.Add(strSqlSDSKJ);
                                //更新登陆账号信息表 账户资金余额
                                string strZHZJYE = "update AAA_DLZHXXB set B_ZHDQKYYE=B_ZHDQKYYE+'" + Convert.ToDouble(hashTableJE["税后金额"]).ToString("#0.00") + "' where B_DLYX='" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "'";
                                arrayList.Add(strZHZJYE);
                            }
                        }
                    }


                }
            }
            //DbHelperSQL.ExecSqlTran(arrayList);

            Hashtable returnAllSQL= I_DBL.RunParam_SQL(arrayList);
            if ((bool)returnAllSQL["return_float"])
            {
                if (Convert.ToInt32(returnAllSQL["return_other"])>0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控成功，时间" + DateTime.Now.ToString() + "！";
                    return dsreturn;
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ ";
                    return dsreturn;
                }
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ 详情：" + returnAllSQL["return_errmsg"].ToString();
                return dsreturn;
            }
            
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ 详情：" + ex.ToString();
            return dsreturn;

        }
        #endregion


    }
    /// <summary>
    ///获取相对于传入时间的月初时间和月末时间
    /// </summary>
    /// <param name="dataTime">传入的时间</param>
    /// <returns></returns>
    public static Hashtable GetMonthStartEnd(DateTime dataTime)
    {
        Hashtable hash = new Hashtable();
        //获取相对于传入时间的上月月初
        DateTime dataTimeLastMonthStart = DateTime.Parse(dataTime.AddMonths(-1).ToString("yyyy-MM-01"));
        //获取相对于传入时间的上月月末
        DateTime dataTimeLastMonthEnd = dataTimeLastMonthStart.AddMonths(1).AddDays(-1);
        hash["月初时间"] = dataTimeLastMonthStart.ToString("yyyy-MM-dd");
        hash["月末时间"] = dataTimeLastMonthEnd.ToString("yyyy-MM-dd");
        return hash;
    }
    /// <summary>
    /// 根据年份和月份获取经纪人收益扣税明细表的总数据，判断次年份和月份的数据是否存在，不存在为true存在为false
    /// </summary>
    /// <param name="strYear"></param>
    /// <param name="strMonth"></param>
    /// <returns></returns>
    public static bool GetJJRSYKSMXB(string strYear, string strMonth)
    {
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            string strSql = "select COUNT(*) from AAA_JJRSYKSMXB where SYKSNF='" + strYear + "' and SYKSYF='" + strMonth + "'";
            Hashtable returnHT = I_DBL.RunProc(strSql, "");
            if (!(bool)returnHT["return_float"])
            {
                throw ((Exception)returnHT["return_errmsg"]);
            }
            string objNum = ((DataSet)returnHT["return_ds"]).Tables[0].Rows[0][0].ToString();
            if (Convert.ToInt32(objNum) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// 根据开始时间和结束时间查找经纪人的收益数据信息
    /// </summary>
    /// <param name="timeStart">月度开始时间</param>
    /// <param name="timeEnd">月度结束时间</param>
    /// <returns></returns>
    public static DataSet ServerMYJJRKS_HQSY(string timeMonthStart, string timeMonthEnd)
    {
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            string strSql = "select tab.*,isnull(DD.I_DSFCGZT,'') '第三方存管状态' from (select a.DLYX '登录邮箱',a.JSZHLX '结算账户类型',a.JSBH '角色编号',sum(a.JE) '金额' from AAA_ZKLSMXB as a inner join AAA_moneyDZB as b on a.XZ=b.XZ inner join AAA_DLZHXXB as c on a.DLYX=c.B_DLYX where CONVERT(varchar(10),LSCSSJ,120)>='" + timeMonthStart + "' and CONVERT(varchar(10),LSCSSJ,120)<='" + timeMonthEnd + "' and a.SJLX='预' and c.I_ZCLB='自然人' and  b.xz in('来自卖方收益','来自买方收益')  and a.JSZHLX='经纪人交易账户' group by a.DLYX,a.JSZHLX,a.JSBH) as tab inner join AAA_DLZHXXB as DD on tab.登录邮箱=DD.B_DLYX";
            //这个sql滤掉AAA_JJRSYKSMXB表里，成功的
            //DataSet dataSet = DbHelperSQL.Query(strSql);
            Hashtable returnHT = I_DBL.RunProc(strSql, "");
            if (!(bool)returnHT["return_float"])
            {
                throw ((Exception)returnHT["return_errmsg"]);
            }
            if (returnHT["return_ds"] == null || ((DataSet)returnHT["return_ds"]).Tables.Count <= 0 || ((DataSet)returnHT["return_ds"]).Tables[0].Rows.Count <= 0)
            {
                return null;
            }
            return (DataSet)returnHT["return_ds"];
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// 对经纪人的收益执行扣税的操作
    /// </summary>
    /// <param name="douSK">收益金额</param>
    /// <returns></returns>
    public static Hashtable ServerMYJJRKS_JXKS(double douSK)
    {
        Hashtable hashSK = new Hashtable();
        hashSK["收益"] = douSK;
        #region//作废
        /*
         A、0<收入<=800，应纳税额=0，税后所得=收入，
B、800<收入<=4000，应纳税额=（收入-800）*0.2，税后所得=收入-应纳税额
C、4000<收入<=20000，应纳税额=收入*0.8*0.2，税后所得=收入-应纳税额
D、20000<收入<=50000，应纳税额=收入*0.8*0.3-2000，税后所得=收入-应纳税额
E、收入>50000 ，应纳税额=收入*0.8*0.4-7000，税后所得=收入-应纳税额
         */
        //double SJ = 0.00;
        //if (douSK >= 0 && douSK <= 800)
        //{
        //    SJ = 0;
        //}
        //else if (douSK >800 && douSK <= 4000)
        //{
        //    SJ = Math.Round(((douSK - 800) * 0.20),2);
        //}
        //else if (douSK > 4000 && douSK<=20000)
        //{
        //    SJ = Math.Round((douSK*0.80* 0.20), 2);
        //}
        //else if (douSK > 20000 && douSK <= 50000)
        //{
        //    SJ = Math.Round((douSK * 0.80 * 0.30 - 2000), 2);
        //}
        //else
        //{
        //    SJ = Math.Round((douSK * 0.80 * 0.40 - 7000), 2);
        //}
        //hashSK["税额"] = SJ;//此处暂时这样做

        //hashSK["税后金额"] = Math.Round((douSK - SJ),2);
        #endregion

        /*
         .劳务所得税率计算标准及收益计算公式
A、0<收入<=800，应纳税额=0，税后所得=收入，
B、800<收入<=4000，应纳税额=（收入-800）*0.2，税后所得=收入-应纳税额
C、4000<收入<=25000，应纳税额=收入*0.8*0.2，税后所得=收入-应纳税额
D、25000<收入<=62500，应纳税额=收入*0.8*0.3-2000，税后所得=收入-应纳税额
E、收入>62500 ，应纳税额=收入*0.8*0.4-7000，税后所得=收入-应纳税额
         */
        double SJ = 0.00;
        if (douSK >= 0 && douSK <= 800)
        {
            SJ = 0;
        }
        else if (douSK > 800 && douSK <= 4000)
        {
            SJ = Math.Round(((douSK - 800) * 0.20), 2);
        }
        else if (douSK > 4000 && douSK <= 25000)
        {
            SJ = Math.Round((douSK * 0.80 * 0.20), 2);
        }
        else if (douSK > 25000 && douSK <= 62500)
        {
            SJ = Math.Round((douSK * 0.80 * 0.30 - 2000), 2);
        }
        else
        {
            SJ = Math.Round((douSK * 0.80 * 0.40 - 7000), 2);
        }
        hashSK["税额"] = SJ;//此处暂时这样做

        hashSK["税后金额"] = Math.Round((douSK - SJ), 2);
        return hashSK;
    }
    /// <summary>
    /// 获取经纪人收益扣税明细表中“未计算”的数据和应获取的收益
    /// </summary>
    /// <param name="strDLYX">登陆邮箱</param>
    /// <param name="timeMonthStart">月初时间</param>
    /// <param name="timeMonthEnd">月末时间</param>
    /// <param name="strYear">年份</param>
    /// <param name="strMonth">月份</param>
    /// <returns>返回的数据</returns>
    public static DataSet ServerMYJJRKS_HQSYMX(string timeMonthStart, string timeMonthEnd, string strYear, string strMonth)
    {
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            string strSql = "select  MX.Number,MX.DLYX '登录邮箱',MX.JSZHLX '结算账户类型',MX.JJRJSBH '角色编号',isnull(MX.SFYJJSWC,'') '是否计算',isnull(DD.I_DSFCGZT,'') '第三方存管状态',(select sum(a.JE)  from AAA_ZKLSMXB as a inner join AAA_moneyDZB as b on a.XZ=b.XZ  where  a.DLYX=DD.B_DLYX and CONVERT(varchar(10),LSCSSJ,120)>='" + timeMonthStart + "' and CONVERT(varchar(10),LSCSSJ,120)<='" + timeMonthEnd + "' and a.SJLX='预' and DD.I_ZCLB='自然人' and  b.xz in('来自卖方收益','来自买方收益')  and a.JSZHLX='经纪人交易账户') '金额' from AAA_JJRSYKSMXB as MX inner join AAA_DLZHXXB as DD on MX.DLYX=DD.B_DLYX where MX.SYKSNF='" + strYear + "' and MX.SYKSYF='" + strMonth + "' and MX.JSZHLX='经纪人交易账户' and MX.SFYJJSWC='未计算'";
            //DataSet dataSet = DbHelperSQL.Query(strSql);
            Hashtable returnHT = I_DBL.RunProc(strSql,"");
            if (!(bool)returnHT["return_float"])
            {
                throw (Exception)returnHT["return_errmsg"];
            }
            return (DataSet)returnHT["return_ds"];
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region//合同期满处理

    public static DataSet ServerHTQMJK()
    {
        //初始化返回值,先塞一行数据
        DataSet ht_result = ReturnDt();
            ht_result.Tables["返回值单条"].Rows.Add(new string[] { "初始化", "未执行！" });
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            ArrayList alLog = new ArrayList();//用户保存需要写入日志的内容
            string jkbh = "合同到期监控";
            #region//获取需要插入的账款流水明细资金类型基本上数据
            //DataSet ds_ZKMX = DbHelperSQL.Query("select * from AAA_moneyDZB where XM in ('订金','技术服务费','违约赔偿金','补偿收益','履约保证金')");
            Hashtable returnHT_ZKMX= I_DBL.RunProc("select * from AAA_moneyDZB where XM in ('订金','技术服务费','违约赔偿金','补偿收益','履约保证金')","");
            if (!(bool)returnHT_ZKMX["return_float"])
            {
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT_ZKMX["return_errmsg"].ToString();
                return ht_result;
            }
            if (returnHT_ZKMX["return_ds"] == null || ((DataSet)returnHT_ZKMX["return_ds"]).Tables.Count < 1 || ((DataSet)returnHT_ZKMX["return_ds"]).Tables[0].Rows.Count<1)
            {
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取需要插入的账款流水明细资金类型基本数据失败 ";
                return ht_result;
            }
            DataSet ds_ZKMX = (DataSet)returnHT_ZKMX["return_ds"];
            DataRow[] dr_saleycfh_y = ds_ZKMX.Tables[0].Select("number='1304000027'");//卖家发货延迟扣罚--预
            DataRow[] dr_saleycfp_y = ds_ZKMX.Tables[0].Select("number='1304000029'");//卖家发票延迟扣罚--预
            DataRow[] dr_buyycfh_y = ds_ZKMX.Tables[0].Select("number='1304000051'");//买家发货延迟补偿--预
            DataRow[] dr_buyycfp_y = ds_ZKMX.Tables[0].Select("number='1304000050'");//买家发票延迟补偿--预
            #endregion

            #region//获取信用评分对照表的基本数据
            //DataTable dt_xypf = DbHelperSQL.Query("select * from AAA_JYFXYBDMXDZB").Tables[0];
            /*Hashtable returnHT_xypf = I_DBL.RunProc("select * from AAA_JYFXYBDMXDZB", "");
            if (!(bool)returnHT_xypf["return_float"])
            {
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT_xypf["return_errmsg"].ToString();
                return ht_result;
            }
            if (returnHT_xypf["return_ds"] == null || ((DataSet)returnHT_xypf["return_ds"]).Tables.Count < 1 || ((DataSet)returnHT_xypf["return_ds"]).Tables[0].Rows.Count < 1)
            {
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取信用评分对照表的基本数据失败 ";
                return ht_result;
            }
            DataTable dt_xypf = ((DataSet)returnHT_xypf["return_ds"]).Tables[0];  */

            DataTable dtjf = new DataTable("jf");
            DataColumn dc = new DataColumn("合同编号", typeof(string));
            dtjf.Columns.Add(dc);
            dtjf.Clear();
            DataRow drht = null;//dt.NewRow();

            #endregion

            //用于存储更新评分的语句
            List<string> jfl = new List<string>();



            #region//获取合同到期需要处理的所有基础数据信息
            //string sql_info = "select top 5 *,cast((Z_ZBSL-Z_YTHSL)/Z_ZBSL*Z_BZHJE as numeric(18,2)) as 买家违约赔偿,cast((Z_ZBSL-Z_YTHSL)/Z_ZBSL*(Z_DJJE-Z_BZHJE) as numeric(18,2)) as 买家技术服务费,";
            string sql_info = "select *,cast((Z_ZBSL-Z_YTHSL)/Z_ZBSL*Z_BZHJE as numeric(18,2)) as 买家违约赔偿,cast(Z_ZBJE*0.02 as numeric(18,2)) as 买家技术服务费,";
            sql_info += "(select  isnull(SUM(je),0.00) from AAA_ZKLSMXB as c where c.DLYX=tab.T_YSTBDDLYX and c.LYYWLX='AAA_THDYFHDXXB' and c.LYDH  in ( select Number from  AAA_THDYFHDXXB where ZBDBXXBBH= tab.Number) and c.XM ='" + dr_saleycfh_y[0]["xm"].ToString() + "' and c.XZ='" + dr_saleycfh_y[0]["xz"].ToString() + "' and c.SJLX ='" + dr_saleycfh_y[0]["sjlx"].ToString() + "' ) as 卖家发货扣罚,";//---LYYYLX 改为 “AAA_THDYFHDXXB” wyh 2014.07024
            sql_info += "(select isnull(SUM(je),0.00) from AAA_ZKLSMXB as d where d.DLYX=tab.T_YSTBDDLYX and d.LYYWLX='AAA_THDYFHDXXB' and d.LYDH in ( select Number from  AAA_THDYFHDXXB where ZBDBXXBBH= tab.Number) and d.XM ='" + dr_saleycfp_y[0]["xm"].ToString() + "' and d.XZ='" + dr_saleycfp_y[0]["xz"].ToString() + "' and d.SJLX ='" + dr_saleycfp_y[0]["sjlx"].ToString() + "' ) as 卖家发票扣罚,";
            sql_info += "(select isnull(SUM(je),0.00) from AAA_ZKLSMXB as e where e.DLYX=tab.Y_YSYDDDLYX and e.LYYWLX='AAA_THDYFHDXXB' and e.LYDH in ( select Number from  AAA_THDYFHDXXB where ZBDBXXBBH= tab.Number) and e.XM ='" + dr_buyycfh_y[0]["xm"].ToString() + "' and e.XZ='" + dr_buyycfh_y[0]["xz"].ToString() + "' and e.SJLX ='" + dr_buyycfh_y[0]["sjlx"].ToString() + "' ) as 买家发货补偿收益,";
            sql_info += "(select isnull(SUM(je),0.00) from AAA_ZKLSMXB as f where f.DLYX=tab.Y_YSYDDDLYX and f.LYYWLX='AAA_THDYFHDXXB' and f.LYDH in ( select Number from  AAA_THDYFHDXXB where ZBDBXXBBH= tab.Number) and f.XM ='" + dr_buyycfp_y[0]["xm"].ToString() + "' and f.XZ='" + dr_buyycfp_y[0]["xz"].ToString() + "' and f.SJLX ='" + dr_buyycfp_y[0]["sjlx"].ToString() + "' ) as 买家发票补偿收益 ";
            sql_info += " from ( select a.*,(select COUNT(*) from AAA_THDYFHDXXB as b where b.ZBDBXXBBH=a.number and b.F_DQZT not in ('无异议收货','默认无异议收货','有异议收货后无异议收货','补发货物无异议收货','卖家主动退货','撤销'))  as 有异议收货 from AAA_ZBDBXXB as a where Z_HTZT='定标' and Z_QPZT='未开始清盘' and  CONVERT (varchar(10), Z_HTJSRQ,120)<'" + DateTime.Now.ToString("yyyy-MM-dd") + "') as tab where (Z_ZBSL=Z_YTHSL and 有异议收货>0) or (Z_ZBSL>Z_YTHSL)";            

            Hashtable returnHT_info = I_DBL.RunProc(sql_info, "");
            if (!(bool)returnHT_info["return_float"])
            {
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "基础数据获取过程出现错误 ！" + returnHT_info["return_errmsg"].ToString();
                alLog.Add(ht_result.Tables["返回值单条"].Rows[0]["提示文本"].ToString());                
                return ht_result;
            }
            if (returnHT_info["return_ds"] == null || ((DataSet)returnHT_info["return_ds"]).Tables.Count < 1 )
            {
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "基础数据获取数据失败 ";
                return ht_result;
            }
            DataTable dt_info = ((DataSet)returnHT_info["return_ds"]).Tables[0];

            #endregion

            if (dt_info == null || dt_info.Rows.Count == 0)
            {
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "无到期合同需要处理！";
                alLog.Add("无到期合同需要处理！");
                return ht_result;
            }
            else
            {
                ArrayList al = new ArrayList();//用于保存需要执行的sql语句       
                //List<Hashtable> listTX = new List<Hashtable>(); //用户执行提醒插入功能的参数列表
                DataTable SenMessData = SenMessForDataTable();//用户执行提醒插入功能的参数列表

                #region//用于保存钱的变动明细
                Hashtable returnHT_Money = I_DBL.RunProc("select '' as 登录邮箱,0.00 as 金额, '' as 运算,'' as 数据类型 from AAA_ZBDBXXB where 1!=1", "");//用于保存钱的变动明细
                if (!(bool)returnHT_Money["return_float"])
                {
                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT_Money["return_errmsg"].ToString();
                    return ht_result;
                }
                if (returnHT_Money["return_ds"] == null || ((DataSet)returnHT_Money["return_ds"]).Tables.Count < 1)
                {
                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取中标定标信息表表结构失败 ";
                    return ht_result;
                }
                DataTable dt_Money =((DataSet)returnHT_Money["return_ds"]).Tables[0];
                #endregion

                DataRow[] dr_buyjd = ds_ZKMX.Tables[0].Select("number='1304000022'");//订金 合同期满解冻 
                DataRow[] dr_buywypc = ds_ZKMX.Tables[0].Select("number='1304000024'");//违约赔偿金 买家违约赔偿给卖家 
                DataRow[] dr_salebc = ds_ZKMX.Tables[0].Select("number='1304000034'");//补偿收益 收到买家违约补偿 
                DataRow[] dr_buyjsfwf = ds_ZKMX.Tables[0].Select("number='1304000008'");//技术服务费 买家违约赔偿 
                DataRow[] dr_salelybzj = ds_ZKMX.Tables[0].Select("number='1304000048'");// 履约保证金 履约保证金解冻 
                DataRow[] dr_saleycfh_s = ds_ZKMX.Tables[0].Select("number='1304000026'");//违约赔偿金 超过最迟发货日后录入发货信息 
                DataRow[] dr_buyycfh_s = ds_ZKMX.Tables[0].Select("number='1304000036'");//补偿收益 收到卖家发货延迟补偿 
                DataRow[] dr_saleycfp_s = ds_ZKMX.Tables[0].Select("number='1304000028'");//违约赔偿金 发货单生成后5日内未录入发票邮寄信息 
                DataRow[] dr_buyycfp_s = ds_ZKMX.Tables[0].Select("number='1304000037'");//补偿收益 收到卖家发票延迟补偿 
                
                #region 循环

                for (int i = 0; i < dt_info.Rows.Count; i++)
                {
                    #region//如果已提货数量=中标数量，没有完成无异议收货的
                    if (dt_info.Rows[i]["Z_YTHSL"].ToString() == dt_info.Rows[i]["Z_ZBSL"].ToString() && dt_info.Rows[i]["有异议收货"].ToString() != "0")
                    {
                        string sql_update = "update AAA_ZBDBXXB set Z_HTZT='定标合同到期',Z_QPZT='清盘中',Z_QPKSSJ='" + DateTime.Now.ToString() + "' where Number='" + dt_info.Rows[i]["number"].ToString() + "'";
                        al.Add(sql_update);
                    }
                    #endregion

                    #region//已提货数量<中标数量  
                    if (Convert.ToInt64(dt_info.Rows[i]["Z_YTHSL"]) < Convert.ToInt64(dt_info.Rows[i]["Z_ZBSL"]))
                    {
                        #region//插入《账款流水明细表》中买家订金解冻
                        string zy_buyjd = dr_buyjd[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》期满                 
                        string KeyNumber_buyjd = "";
                       object[] re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (re[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (re[1] != null)
                            {
                                KeyNumber_buyjd = (string)(re[1]);
                            }
                            else
                            {
                                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                                return ht_result;
                            }

                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                            return ht_result;
                        }
                        string sql_BuyJD = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buyjd + "','" + dt_info.Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buyjd[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["Z_DJJE"].ToString() + ",'" + dr_buyjd[0]["XM"].ToString() + "','" + dr_buyjd[0]["XZ"].ToString() + "','" + zy_buyjd + "','" + jkbh + "','','" + dr_buyjd[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                        al.Add(sql_BuyJD);
                        #endregion

                        #region//插入《账款流水明细表》中买家未完成提货的违约赔偿金
                        string zy_buywypc = dr_buywypc[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》期满买家没有下完提货单                

                        //string KeyNumber_buywypc = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string KeyNumber_buywypc = "";
                        re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (re[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (re[1] != null)
                            {
                                KeyNumber_buywypc = (string)(re[1]);
                            }
                            else
                            {
                                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                                return ht_result;
                            }

                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                            return ht_result;
                        }
                        string sql_BuyWYPC = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buywypc + "','" + dt_info.Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buywypc[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["买家违约赔偿"].ToString() + ",'" + dr_buywypc[0]["XM"].ToString() + "','" + dr_buywypc[0]["XZ"].ToString() + "','" + zy_buywypc + "','" + jkbh + "','','" + dr_buywypc[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                        al.Add(sql_BuyWYPC);
                        #endregion

                        #region //插入《账款流水明细表》卖家的补偿收益               
                        string zy_salebc = dr_salebc[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要内容：[x1]《电子购货合同》期满买家未完成提货

                        //string KeyNumber_salebc = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string KeyNumber_salebc = "";
                        re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (re[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (re[1] != null)
                            {
                                KeyNumber_salebc = (string)(re[1]);
                            }
                            else
                            {
                                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                                return ht_result;
                            }

                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                            return ht_result;
                        }
                        string sql_Salebc = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_salebc + "','" + dt_info.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_salebc[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["买家违约赔偿"].ToString() + ",'" + dr_salebc[0]["XM"].ToString() + "','" + dr_salebc[0]["XZ"].ToString() + "','" + zy_salebc + "','" + jkbh + "','','" + dr_salebc[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                        al.Add(sql_Salebc);
                        #endregion

                        #region //插入《账款流水明细表》扣减买家的技术服务费               
                        string zy_buyjsfwf = dr_buyjsfwf[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要内容：[x1]《电子购货合同》期满买家未完成提货

                        //string KeyNumber_buyjsfwf = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string KeyNumber_buyjsfwf = "";
                        re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                        if (re[0].ToString() == "ok")
                        {
                            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                            if (re[1] != null)
                            {
                                KeyNumber_buyjsfwf = (string)(re[1]);
                            }
                            else
                            {
                                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                                return ht_result;
                            }

                        }
                        else
                        {
                            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                            return ht_result;
                        }
                        string sql_Buyjsfwf = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buyjsfwf + "','" + dt_info.Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buyjsfwf[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["买家技术服务费"].ToString() + ",'" + dr_buyjsfwf[0]["XM"].ToString() + "','" + dr_buyjsfwf[0]["XZ"].ToString() + "','" + zy_buyjsfwf + "','" + jkbh + "','','" + dr_buyjsfwf[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                        al.Add(sql_Buyjsfwf);
                        #endregion

                        #region//将钱的变动增加到dt_Money中
                        dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["Z_DJJE"]), '加', dr_buyjd[0]["SJLX"].ToString(), });//买家订金解冻
                        dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["买家违约赔偿"]), '减', dr_buywypc[0]["SJLX"].ToString(), });//买家未完成提货的违约赔偿
                        dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["T_YSTBDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["买家违约赔偿"]), '加', dr_salebc[0]["SJLX"].ToString(), });//卖家补偿收益
                        dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["买家技术服务费"]), '减', dr_buyjsfwf[0]["SJLX"].ToString(), });//买家未完成提货的技术服务费
                        #endregion

                        #region//没有完成无异议收货
                        if (Convert.ToInt64(dt_info.Rows[i]["有异议收货"]) > 0)
                        {
                            string sql_update = "update AAA_ZBDBXXB set Z_HTZT='定标合同到期',Z_QPZT='清盘中',Z_QPKSSJ='" + DateTime.Now.ToString() + "' where number='" + dt_info.Rows[i]["number"].ToString() + "'";
                            al.Add(sql_update);
                        }
                        #endregion

                        #region//完成无异议收货的
                        else//完成无异议收货的
                        {
                            #region//写入《账款流水明细表》中卖家解冻履约保证金
                            string zy_salelybzj = dr_salelybzj[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》终止，所有问题已全部处理完毕，即清盘结束时。包括但不限于“请重新发货”、“部分收货”、“有异议收货”等情况已有最终处理结果。
                            //string KeyNumber_salelybzj = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                            string KeyNumber_salelybzj = "";
                            re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                            if (re[0].ToString() == "ok")
                            {
                                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                if (re[1] != null)
                                {
                                    KeyNumber_salelybzj = (string)(re[1]);
                                }
                                else
                                {
                                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                                    return ht_result;
                                }

                            }
                            else
                            {
                                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                                return ht_result;
                            }
                            string sql_salelybzj = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_salelybzj + "','" + dt_info.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_salelybzj[0]["YSLX"].ToString() + "'," + Convert.ToDouble(dt_info.Rows[i]["Z_LYBZJJE"]) + ",'" + dr_salelybzj[0]["XM"].ToString() + "','" + dr_salelybzj[0]["XZ"].ToString() + "','" + zy_salelybzj + "','" + jkbh + "','','" + dr_salelybzj[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                            al.Add(sql_salelybzj);
                            #endregion

                            #region//将钱的变动增加到dt_Money中
                            dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["T_YSTBDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["Z_LYBZJJE"]), '加', dr_salelybzj[0]["SJLX"].ToString() });//卖家履约保证金解冻
                            #endregion

                            #region //扣罚卖家的延迟录入发货信息违约赔偿金,给买家发货延迟补偿收益
                            if (Convert.ToDouble(dt_info.Rows[i]["卖家发货扣罚"]) > 0 && Convert.ToDouble(dt_info.Rows[i]["卖家发货扣罚"]).ToString("#0.00") != "0.00")
                            {
                                #region//写入《账款流水明细表》中卖家延迟发货违约赔偿金
                                string zy_saleycfh_s = dr_saleycfh_s[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》结束

                                //string KeyNumber_saleycfh = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string KeyNumber_saleycfh = "";
                                re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                if (re[0].ToString() == "ok")
                                {
                                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                    if (re[1] != null)
                                    {
                                        KeyNumber_saleycfh = (string)(re[1]);
                                    }
                                    else
                                    {
                                        ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                                        return ht_result;
                                    }

                                }
                                else
                                {
                                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                                    return ht_result;
                                }
                                string sql_saleycfh = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_saleycfh + "','" + dt_info.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_saleycfh_s[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["卖家发货扣罚"].ToString() + ",'" + dr_saleycfh_s[0]["XM"].ToString() + "','" + dr_saleycfh_s[0]["XZ"].ToString() + "','" + zy_saleycfh_s + "','" + jkbh + "','','" + dr_saleycfh_s[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                                al.Add(sql_saleycfh);
                                #endregion
                                #region//插入《账款流水明细表》中买家延迟发货补偿收益
                                string zy_buyycfh_s = dr_buyycfh_s[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());
                                //string KeyNumber_buyycfh = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string KeyNumber_buyycfh = "";
                                re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                if (re[0].ToString() == "ok")
                                {
                                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                    if (re[1] != null)
                                    {
                                        KeyNumber_buyycfh = (string)(re[1]);
                                    }
                                    else
                                    {
                                        ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                                        return ht_result;
                                    }

                                }
                                else
                                {
                                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                                    return ht_result;
                                }

                                string sql_buyycfh = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buyycfh + "','" + dt_info.Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buyycfh_s[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["买家发货补偿收益"].ToString() + ",'" + dr_buyycfh_s[0]["XM"].ToString() + "','" + dr_buyycfh_s[0]["XZ"].ToString() + "','" + zy_buyycfh_s + "','" + jkbh + "','','" + dr_buyycfh_s[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                                al.Add(sql_buyycfh);
                                #endregion
                                #region//将钱的变动增加到dt_Money中
                                dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["T_YSTBDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["卖家发货扣罚"]), '减', dr_saleycfh_s[0]["SJLX"].ToString() });//卖家延迟发货违约赔偿
                                dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["买家发货补偿收益"]), '加', dr_buyycfh_s[0]["SJLX"].ToString() });//买家延迟发货补偿收益
                                #endregion
                            }
                            #endregion

                            #region //卖家延迟发票扣罚，买家延迟发票补偿
                            if (Convert.ToDouble(dt_info.Rows[i]["卖家发票扣罚"]) > 0 && Convert.ToDouble(dt_info.Rows[i]["卖家发货扣罚"]).ToString("#0.00") != "0.00")
                            {
                                #region //写入《账款流水明细表》中卖家延迟发票违约赔偿金
                                string zy_saleycfp_s = dr_saleycfp_s[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》结束
                                //string KeyNumber_saleycfp = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string KeyNumber_saleycfp = "";
                                re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                if (re[0].ToString() == "ok")
                                {
                                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                    if (re[1] != null)
                                    {
                                        KeyNumber_saleycfp = (string)(re[1]);
                                    }
                                    else
                                    {
                                        ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                                        return ht_result;
                                    }

                                }
                                else
                                {
                                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                                    return ht_result;
                                }

                                string sql_saleycfp = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_saleycfp + "','" + dt_info.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_saleycfp_s[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["卖家发票扣罚"].ToString() + ",'" + dr_saleycfp_s[0]["XM"].ToString() + "','" + dr_saleycfp_s[0]["XZ"].ToString() + "','" + zy_saleycfp_s + "','" + jkbh + "','','" + dr_saleycfp_s[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                                al.Add(sql_saleycfp);
                                #endregion
                                #region//插入《账款流水明细表》中买家延迟发票补偿收益
                                string zy_buyycfp_s = dr_buyycfp_s[0]["zy"].ToString().Replace("[x1]", dt_info.Rows[i]["Z_HTBH"].ToString());//摘要文字：[x1]《电子购货合同》结束
                                //string KeyNumber_buyycfp = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string KeyNumber_buyycfp = "";
                                re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                                if (re[0].ToString() == "ok")
                                {
                                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                    if (re[1] != null)
                                    {
                                        KeyNumber_buyycfp = (string)(re[1]);
                                    }
                                    else
                                    {
                                        ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                                        return ht_result;
                                    }

                                }
                                else
                                {
                                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                                    return ht_result;
                                }

                                string sql_buyycfp = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buyycfp + "','" + dt_info.Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + dt_info.Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buyycfp_s[0]["YSLX"].ToString() + "'," + dt_info.Rows[i]["买家发票补偿收益"].ToString() + ",'" + dr_buyycfp_s[0]["XM"].ToString() + "','" + dr_buyycfp_s[0]["XZ"].ToString() + "','" + zy_buyycfp_s + "','" + jkbh + "','','" + dr_buyycfp_s[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                                al.Add(sql_buyycfp);
                                #endregion
                                #region//将钱的变动增加到dt_Money中
                                dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["T_YSTBDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["卖家发票扣罚"]), '减', dr_saleycfp_s[0]["SJLX"].ToString() });//卖家延迟发票违约赔偿
                                dt_Money.Rows.Add(new object[] { dt_info.Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(dt_info.Rows[i]["买家发票补偿收益"]), '加', dr_buyycfp_s[0]["SJLX"].ToString() });//买家延迟发票补偿
                                #endregion
                            }
                            #endregion

                            #region//更新中标定标信息表清盘状态和清盘开始结束时间
                            string datetime = DateTime.Now.ToString();
                            string sql_update = "update AAA_ZBDBXXB set Z_HTZT='定标合同到期',Z_QPZT='清盘结束',Z_QPKSSJ='" + datetime + "',Z_QPJSSJ='" + datetime + "' where number='" + dt_info.Rows[i]["number"].ToString() + "'";
                            al.Add(sql_update);
                            #endregion

                            #region 用户评级更新，EditTime 2013-12// wyh 2014.07.28 add
                            drht = dtjf.NewRow();
                            drht["合同编号"] = dt_info.Rows[i]["Z_HTBH"].ToString();
                            dtjf.Rows.Add(drht);

                            #endregion

                            #region//此种情况的数据为自动清盘，需要发送提醒
                            //Hashtable ht_saleinfo = PublicClass2013.GetUserInfo(dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString());
                            DataSet ht_saleinfo;
                            object[] re_UserInfo = IPC.Call("获取账户的相关信息", new object[] { dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString() });
                            if (re_UserInfo[0].ToString() == "ok")
                            {
                                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                if (re_UserInfo[1] != null && ((DataSet)re_UserInfo[1]).Tables.Count > 0 && ((DataSet)re_UserInfo[1]).Tables[0].Rows.Count > 0)
                                {
                                    ht_saleinfo = (DataSet)re_UserInfo[1];
                                }
                                else
                                {
                                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取账户相关信息失败";
                                    return ht_result;
                                }
                            }
                            else
                            {
                                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                                
                                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re_UserInfo[1].ToString();
                                return ht_result;
                            }

                            string saler = "不存在";
                            if (ht_saleinfo.Tables["完整数据集"].Rows.Count > 0)
                            {
                                saler = ht_saleinfo.Tables["完整数据集"].Rows[0]["I_JYFMC"].ToString();
                            }
                            
                            DataRow ht_sale = SenMessData.NewRow();
                            //Hashtable ht_sale = new Hashtable();
                            ht_sale["type"] = "集合经销平台";
                            ht_sale["提醒对象登陆邮箱"] = dt_info.Rows[i]["T_YSTBDDLYX"].ToString();
                            //ht_sale["提醒对象用户名"] = "";//用户名不用传参数，其他地方已处理
                            ht_sale["提醒对象结算账户类型"] = dt_info.Rows[i]["T_YSTBDJSZHLX"];
                            ht_sale["提醒对象角色编号"] = dt_info.Rows[i]["T_YSTBDMJJSBH"].ToString();
                            ht_sale["提醒对象角色类型"] = "卖家";
                            ht_sale["提醒内容文本"] = "尊敬的" + saler + "，您" + dt_info.Rows[i]["Z_HTBH"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                            ht_sale["创建人"] = "admin";
                            ht_sale["查看地址"] = "";
                            SenMessData.Rows.Add(ht_sale);
                            alLog.Add("提醒：" + ht_sale["提醒对象登陆邮箱"].ToString() + "→" + ht_sale["提醒内容文本"].ToString());

                            //Hashtable ht_buyinfo = PublicClass2013.GetUserInfo(dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString());                            
                            DataSet ht_buyinfo;
                            re_UserInfo = IPC.Call("获取账户的相关信息", new object[] { dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString() });
                            if (re_UserInfo[0].ToString() == "ok")
                            {
                                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                                if (re_UserInfo[1] != null && ((DataSet)re_UserInfo[1]).Tables.Count > 0 && ((DataSet)re_UserInfo[1]).Tables[0].Rows.Count > 0)
                                {
                                    ht_buyinfo = (DataSet)re_UserInfo[1];
                                }
                                else
                                {
                                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取账户相关信息失败";
                                    return ht_result;
                                }
                            }
                            else
                            {
                                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。                                
                                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re_UserInfo[1].ToString();
                                return ht_result;
                            }
                            string buyer = "不存在";
                            if (ht_buyinfo.Tables["完整数据集"] != null && ht_buyinfo.Tables["完整数据集"].Rows.Count > 0)
                            {
                                buyer = ht_buyinfo.Tables["完整数据集"].Rows[0]["I_JYFMC"].ToString();
                            }
                            //Hashtable ht_buy = new Hashtable();
                            DataRow ht_buy = SenMessData.NewRow();
                            ht_buy["type"] = "集合经销平台";
                            ht_buy["提醒对象登陆邮箱"] = dt_info.Rows[i]["Y_YSYDDDLYX"].ToString();
                            //ht_buy["提醒对象用户名"] = "";//用户名不用传参数，其他地方已处理
                            ht_buy["提醒对象结算账户类型"] = dt_info.Rows[i]["Y_YSYDDJSZHLX"];
                            ht_buy["提醒对象角色编号"] = dt_info.Rows[i]["Y_YSYDDMJJSBH"].ToString();
                            ht_buy["提醒对象角色类型"] = "买家";
                            ht_buy["提醒内容文本"] = "尊敬的" + buyer + "，您" + dt_info.Rows[i]["Z_HTBH"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                            ht_buy["创建人"] = "admin";
                            ht_buy["查看地址"] = "";
                            SenMessData.Rows.Add(ht_buy);
                            alLog.Add("提醒：" + ht_buy["提醒对象登陆邮箱"].ToString() + "→" + ht_buy["提醒内容文本"].ToString());
                            #endregion

                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
                alLog.AddRange(al);

                #region //处理钱的数据，获取每个人应该变更的余额数据
                DataTable dt_Money_end = new DataTable();
                dt_Money_end.Columns.Add("登录邮箱");
                dt_Money_end.Columns.Add("金额");

                if (dt_Money != null && dt_Money.Rows.Count > 0)
                {
                    //开始数量合并
                    for (int i = 0; i < dt_Money.Rows.Count; i++)
                    {
                        //是否需要更新新表
                        bool needadd = true;
                        for (int p = 0; p < dt_Money_end.Rows.Count; p++)
                        {
                            //发现匹配列，开始更新新表原有数据。
                            if (dt_Money_end.Rows[p]["登录邮箱"].ToString() == dt_Money.Rows[i]["登录邮箱"].ToString() && dt_Money.Rows[i]["数据类型"].ToString() == "实")
                            {
                                if (dt_Money.Rows[i]["运算"].ToString() == "加")
                                {
                                    dt_Money_end.Rows[p]["金额"] = (Convert.ToDouble(dt_Money_end.Rows[p]["金额"]) + Convert.ToDouble(dt_Money.Rows[i]["金额"])).ToString("#0.00");
                                }
                                else if (dt_Money.Rows[i]["运算"].ToString() == "减")
                                {
                                    dt_Money_end.Rows[p]["金额"] = (Convert.ToDouble(dt_Money_end.Rows[p]["金额"]) - Convert.ToDouble(dt_Money.Rows[i]["金额"])).ToString("#0.00");
                                }
                                //标记为无需添加新行,此行被合并计算了
                                needadd = false;
                            }
                        }
                        if (needadd)
                        {
                            dt_Money_end.Rows.Add(new object[] { dt_Money.Rows[i]["登录邮箱"].ToString(), dt_Money.Rows[i]["金额"].ToString() });
                        }
                    }
                }
                #endregion

                #region 执行所有需要执行的内容
                //开始执行sql语句
                try
                {
                    //执行状态更新及账款流水插入的sql语句
                    //DbHelperSQL.ExecSqlTran(al);
                    Hashtable HTAllSQL = I_DBL.RunParam_SQL(al);
                    if (!(bool)HTAllSQL["return_float"])
                    {
                        ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "状态更新及账款流水写入过程出现错误：" + HTAllSQL["return_errmsg"].ToString();
                        return ht_result;
                    }
                }
                catch (Exception ex)
                {
                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "状态更新及账款流水写入过程出现错误：" + ex.ToString();
                    return ht_result;
                }
                try
                {
                    //执行余额更新程序
                    for (int i = 0; i < dt_Money_end.Rows.Count; i++)
                    {
                        //ClassMoney2013 CM2013 = new ClassMoney2013();
                        //CM2013.FreezeMoneyT(dt_Money_end.Rows[i]["登录邮箱"].ToString(), dt_Money_end.Rows[i]["金额"].ToString(), "+");
                        string sqlUpdateMoney = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE + " + dt_Money_end.Rows[i]["金额"].ToString() + " where B_DLYX='" + dt_Money_end.Rows[i]["登录邮箱"].ToString() + "'";
                        Hashtable returnUpdateMoney= I_DBL.RunProc(sqlUpdateMoney);
                        if ((bool)returnUpdateMoney["return_float"])
                        {
                            alLog.Add("注册表余额更新：" + dt_Money_end.Rows[i]["登录邮箱"].ToString() + "→" + dt_Money_end.Rows[i]["金额"].ToString());
                        }
                        else
                        {
                            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "更新余额的过程出现错误：" + returnUpdateMoney["return_errmsg"].ToString();
                            return ht_result;
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "更新余额的过程出现错误：" + ex.ToString();
                    return ht_result;
                }

                #region//更新评分  wyh 2014.07.28 add
                IPC.Call("信用等级积分处理", new object[] { "合同期满监控", dtjf });
                #endregion
                //发送提醒
                DataSet listTX = new DataSet();
                listTX.Tables.Add(SenMessData);                
                object[] reSendMess = IPC.Call("发送提醒", new object[] { listTX });
                if (reSendMess[0].ToString() != "ok")
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "发送提醒过程出现错误：" + reSendMess[1].ToString();
                    return ht_result;
                }
                #endregion

                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "执行成功!";
            }
            return ht_result;
        }
        catch(Exception ex)
        {
            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return ht_result;
        }
    }

    #endregion

    #region//超期未缴纳履约保证金监控
    public static DataSet ServerCQWJNLYBZJJK()
    {
        //初始化返回值,先塞一行数据
        DataSet ht_result = ReturnDt();
        ht_result.Tables["返回值单条"].Rows.Add(new string[] { "ok", "执行成功！" });
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            string jkbh = "履约保证金超期监控";

            ArrayList alLog = new ArrayList();//用户保存需要写入日志的内容
            //获取需要插入的账款流水明细资金类型基本上数据
            //DataSet ds_ZKMX = DbHelperSQL.Query("select * from AAA_moneyDZB where number in ('1304000020','1304000006','1304000035','1304000025')");
            Hashtable returnht= I_DBL.RunProc("select * from AAA_moneyDZB where number in ('1304000020','1304000006','1304000035','1304000025')","");
            if (!(bool)returnht["return_float"])
            {
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = returnht["return_errmsg"].ToString();
                return ht_result;
            }
            if (returnht["return_ds"] == null || ((DataSet)returnht["return_ds"]).Tables.Count < 1 || ((DataSet)returnht["return_ds"]).Tables[0].Rows.Count <1)
            {
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取需要插入的账款流水明细资金类型基本数据失败";
                return ht_result;
            }
            DataSet ds_ZKMX = (DataSet)returnht["return_ds"];

            //获取需要超期未缴纳履约保证金的数据明细信息
            DataRow htPTVal;
            object[] re = IPC.Call("平台动态参数", new object[] { "" });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] == null || ((DataSet)re[1]).Tables.Count < 1 || ((DataSet)re[1]).Tables[0].Rows.Count < 1 || string.IsNullOrEmpty(((DataSet)re[1]).Tables[0].Rows[0]["卖家履约保证金冻结期限"].ToString()))
                {
                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "未获取到卖家履约保证金冻结期限";
                    return ht_result;
                }
                else
                {
                    htPTVal = ((DataSet)re[1]).Tables[0].Rows[0];
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return ht_result;
            }

            string sql_info = "select a.*,(case a.Z_HTQX when '即时' then 0 else a.T_YSTBDDJTBBZJ end) as 特殊投标保证金,b.Total,(case Z_HTQX when '即时' then a.T_YSTBDDJTBBZJ else cast((Z_ZBSL/b.Total)*a.T_YSTBDDJTBBZJ as numeric(18,2))  end )  as 买家补偿收益,0.00 as 卖家违约赔偿总额,'' as 数据类型 from AAA_ZBDBXXB as a left join (select T_YSTBDBH, isnull(SUM(Z_ZBSL),0) as Total from AAA_ZBDBXXB where Z_HTZT='中标' group by T_YSTBDBH) as b on b.T_YSTBDBH=a.T_YSTBDBH  where Z_HTZT='中标' and CONVERT (varchar(10), DATEADD(DD,(case Z_HTQX when '即时' then " + htPTVal["即时合同定标下提货单期限"].ToString() + " else " + htPTVal["卖家履约保证金冻结期限"].ToString() + " end),Z_ZBSJ),120)<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            //DataSet ds_info = new DataSet();
            //ds_info = DbHelperSQL.Query(sql_info);
            Hashtable return_info = I_DBL.RunProc(sql_info, "");
            if (!(bool)return_info["return_float"])
            {
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "基础数据获取过程出现错误 ！" + return_info["return_errmsg"].ToString();
                alLog.Add(ht_result.Tables["返回值单条"].Rows[0]["提示文本"]);                
                return ht_result;
            }
            DataSet ds_info = (DataSet)return_info["return_ds"];

            if (ds_info == null || ds_info.Tables.Count<1 || ds_info.Tables[0].Rows.Count <= 0)
            {
                ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "无超期未缴纳履约保证金数据！";
                alLog.Add(ht_result.Tables["返回值单条"].Rows[0]["提示文本"]);
            }
            else
            {
                ArrayList al = new ArrayList();//用于保存需要执行的sql语句
                //jhjx_JYFXYMX jyxy = new jhjx_JYFXYMX();

                //用户执行提醒插入功能的参数列表
                DataTable listTX=SenMessForDataTable();

                Hashtable returnHT_Money = I_DBL.RunProc("select '' as 登录邮箱,0.00 as 金额, '' as 运算,'' as 数据类型 from AAA_ZBDBXXB where 1!=1", "");//用于保存钱的变动明细
                if (!(bool)returnHT_Money["return_float"])
                {
                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT_Money["return_errmsg"].ToString();
                    return ht_result;
                }
                if (returnHT_Money["return_ds"]==null||((DataSet)returnHT_Money["return_ds"]).Tables.Count<1)
                {
                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取中标定标表结构失败";
                    return ht_result;
                }
                DataTable dt_Money=((DataSet)returnHT_Money["return_ds"]).Tables[0];

                #region //处理买家相关的数据，包括：投标单、账款、提醒。买家数据根据中标定标信息表处理。
                DataRow[] dr_buyjd = ds_ZKMX.Tables[0].Select("number='1304000020'");//订金 废标解冻 
                DataRow[] dr_buybc = ds_ZKMX.Tables[0].Select("number='1304000035'");//补偿收益 收到卖家未定标补偿 
                for (int i = 0; i < ds_info.Tables[0].Rows.Count; i++)
                {
                    //更新《中标定标信息表》中所获取的数据中的“合同状态”为“未定标废标”；更新“废标时间”。
                    string sql_ZBDB = "update AAA_ZBDBXXB set Z_HTZT='未定标废标',Z_FBSJ='" + System.DateTime.Now.ToString() + "' where number='" + ds_info.Tables[0].Rows[i]["number"].ToString() + "'";
                    al.Add(sql_ZBDB);

                    //更新《预订单信息表》中“原始预订单编号”对应预定单号中【拟订购数量=已中标数量】的数据的“状态”为“撤销”，并更新“撤销时间”
                    string sql_YDD = "update AAA_YDDXXB set ZT='撤销',CXSJ='" + System.DateTime.Now.ToString() + "' where number='" + ds_info.Tables[0].Rows[i]["Y_YSYDDBH"].ToString() + "' and NDGSL=YZBSL";
                    al.Add(sql_YDD);


                    //插入《账款流水明细表》中买家订金解冻               
                    string zy_buyjd = dr_buyjd[0]["zy"].ToString().Replace("[x1]", ds_info.Tables[0].Rows[i]["Y_YSYDDBH"].ToString());//摘要文字：[x1]预订单中标后卖家未定标                 
                    //string KeyNumber_buyjd = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string KeyNumber_buyjd = "";
                    re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                    if (re[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                        if (re[1] != null)
                        {
                            KeyNumber_buyjd = (string)(re[1]);
                        }
                        else
                        {
                            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                            return ht_result;
                        }

                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                        ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                        return ht_result;
                    }

                    string sql_BuyJD = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buyjd + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + ds_info.Tables[0].Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buyjd[0]["YSLX"].ToString() + "'," + ds_info.Tables[0].Rows[i]["Z_DJJE"].ToString() + ",'" + dr_buyjd[0]["XM"].ToString() + "','" + dr_buyjd[0]["XZ"].ToString() + "','" + zy_buyjd + "','" + jkbh + "','','" + dr_buyjd[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                    al.Add(sql_BuyJD);

                    //插入《账款流水明细表》中买家补偿收益
                    string zy_buybc = dr_buybc[0]["zy"].ToString().Replace("[x1]", ds_info.Tables[0].Rows[i]["Y_YSYDDBH"].ToString()).Replace("[x2]", ds_info.Tables[0].Rows[i]["Z_SPMC"].ToString());//摘要文字：[x1]预订单[x2]商品未定标              

                    //string KeyNumber_buybc = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string KeyNumber_buybc = "";
                    re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                    if (re[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                        if (re[1] != null)
                        {
                            KeyNumber_buybc = (string)(re[1]);
                        }
                        else
                        {
                            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                            return ht_result;
                        }

                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                        ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                        return ht_result;
                    }
                    string sql_BuyBC = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_buybc + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString() + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDJSZHLX"].ToString() + "','" + ds_info.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString() + "','AAA_ZBDBXXB','" + ds_info.Tables[0].Rows[i]["number"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_buybc[0]["YSLX"].ToString() + "'," + ds_info.Tables[0].Rows[i]["买家补偿收益"].ToString() + ",'" + dr_buybc[0]["XM"].ToString() + "','" + dr_buybc[0]["XZ"].ToString() + "','" + zy_buybc + "','" + jkbh + "','','" + dr_buybc[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                    al.Add(sql_BuyBC);

                    //将钱的变动增加到dt_Money中
                    dt_Money.Rows.Add(new object[] { ds_info.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(ds_info.Tables[0].Rows[i]["Z_DJJE"]), '加', dr_buyjd[0]["SJLX"].ToString() });//买家订金解冻
                    dt_Money.Rows.Add(new object[] { ds_info.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString(), Convert.ToDouble(ds_info.Tables[0].Rows[i]["买家补偿收益"]), '加', dr_buybc[0]["SJLX"].ToString() });//买家补充收益

                    //向《平台提醒信息表》中插入给对应的买家的提醒
                    //Hashtable ht = new Hashtable();
                    DataRow ht = listTX.NewRow();
                    ht["type"] = "集合经销平台";
                    ht["提醒对象登陆邮箱"] = ds_info.Tables[0].Rows[i]["Y_YSYDDDLYX"].ToString();
                    //ht["提醒对象用户名"] = "";//对象名不需要传参数，传什么都没意义
                    ht["提醒对象结算账户类型"] = ds_info.Tables[0].Rows[i]["Y_YSYDDJSZHLX"];
                    ht["提醒对象角色编号"] = ds_info.Tables[0].Rows[i]["Y_YSYDDMJJSBH"].ToString();
                    ht["提醒对象角色类型"] = "买家";
                    ht["提醒内容文本"] = "因卖方未履行定标程序，您所中标的" + ds_info.Tables[0].Rows[i]["Y_YSYDDBH"].ToString() + "预订单作废，对应订金已解冻，卖方向您支付的" + ds_info.Tables[0].Rows[i]["买家补偿收益"].ToString() + "元违约赔偿金已计入您的账户。";
                    ht["创建人"] = "admin";
                    ht["查看地址"] = "";
                    listTX.Rows.Add(ht);
                    alLog.Add("提醒：" + ht["提醒对象登陆邮箱"].ToString() + "→" + ht["提醒内容文本"].ToString());
                }
                #endregion

                #region //处理卖家相关的数据，包括：投标单、账款、提醒
                DataTable dt_sale = ds_info.Tables[0].DefaultView.ToTable(true, new string[] { "Z_SPMC", "Z_HTQX", "T_YSTBDBH", "T_YSTBDDLYX", "T_YSTBDJSZHLX", "T_YSTBDMJJSBH", "特殊投标保证金", "卖家违约赔偿总额" });
                //处理卖家的账款明细和提醒信息

                DataRow[] dr_salejd = ds_ZKMX.Tables[0].Select("number='1304000006'");//投标保证金 废标解冻 
                DataRow[] dr_salewybc = ds_ZKMX.Tables[0].Select("number='1304000025'");//违约赔偿金 卖家中标后未定标扣罚 
                for (int i = 0; i < dt_sale.Rows.Count; i++)
                {
                    //更新《投标单》中“原始投标单编号”对应的数据的“状态”为“撤销”，并更新“撤销时间”；
                    string sql_TBD = "update AAA_TBD set ZT='撤销',CXSJ='" + DateTime.Now.ToString() + "' where number='" + dt_sale.Rows[i]["T_YSTBDBH"].ToString() + "' AND TBNSL=YZBSL";
                    al.Add(sql_TBD);

                    //插入《账款流水明细表》中卖家投保保证金解冻  
                    string zy_salejd = dr_salejd[0]["zy"].ToString().Replace("[x1]", dt_sale.Rows[i]["T_YSTBDBH"].ToString());//摘要文字：[x1]投标单中标后未定标

                    //string KeyNumber_salejd = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string KeyNumber_salejd = "";
                    re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                    if (re[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                        if (re[1] != null)
                        {
                            KeyNumber_salejd = (string)(re[1]);
                        }
                        else
                        {
                            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                            return ht_result;
                        }

                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                        ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                        return ht_result;
                    }

                    dt_sale.Rows[i]["卖家违约赔偿总额"] = ds_info.Tables[0].Compute("sum(买家补偿收益)", "T_YSTBDMJJSBH='" + dt_sale.Rows[i]["T_YSTBDMJJSBH"].ToString() + "' and T_YSTBDBH='" + dt_sale.Rows[i]["T_YSTBDBH"].ToString() + "'").ToString();
                    double jiedongjine = 0;
                    if (dt_sale.Rows[i]["Z_HTQX"].ToString() == "即时")
                    {
                        jiedongjine = Convert.ToDouble(dt_sale.Rows[i]["卖家违约赔偿总额"]);
                    }
                    else
                    {
                        jiedongjine = Convert.ToDouble(dt_sale.Rows[i]["特殊投标保证金"]);
                    }
                    string sql_Salejd = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_salejd + "','" + dt_sale.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_sale.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_sale.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_TBD','" + dt_sale.Rows[i]["T_YSTBDBH"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_salejd[0]["YSLX"].ToString() + "'," + jiedongjine + ",'" + dr_salejd[0]["XM"].ToString() + "','" + dr_salejd[0]["XZ"].ToString() + "','" + zy_salejd + "','" + jkbh + "','','" + dr_salejd[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                    al.Add(sql_Salejd);

                    //插入《账款流水明细表》中卖家违约赔偿金       
                    string zy_salewybc = dr_salewybc[0]["zy"].ToString().Replace("[x1]", dt_sale.Rows[i]["T_YSTBDBH"].ToString()).Replace("[x2]", dt_sale.Rows[i]["Z_SPMC"].ToString());//摘要文字：[x1]投标单[x2]商品中标后未定标

                    //string KeyNumber_salewypc = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                    string KeyNumber_salewypc = "";
                    re = IPC.Call("获取主键", new string[] { "AAA_ZKLSMXB", "" });
                    if (re[0].ToString() == "ok")
                    {
                        //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                        if (re[1] != null)
                        {
                            KeyNumber_salewypc = (string)(re[1]);
                        }
                        else
                        {
                            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键失败！ ";
                            return ht_result;
                        }

                    }
                    else
                    {
                        //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。      
                        ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                        return ht_result;
                    }
                    string sql_Salewybc = "INSERT INTO [AAA_ZKLSMXB]([Number],[DLYX],[JSZHLX],[JSBH],[LYYWLX],[LYDH],[LSCSSJ],[YSLX],[JE],[XM],[XZ],[ZY],[JKBH],[QTBZ],[SJLX],[CheckState],[CreateUser],[CreateTime],[CheckLimitTime]) VALUES ('" + KeyNumber_salewypc + "','" + dt_sale.Rows[i]["T_YSTBDDLYX"].ToString() + "','" + dt_sale.Rows[i]["T_YSTBDJSZHLX"].ToString() + "','" + dt_sale.Rows[i]["T_YSTBDMJJSBH"].ToString() + "','AAA_TBD','" + dt_sale.Rows[i]["T_YSTBDBH"].ToString() + "','" + DateTime.Now.ToString() + "','" + dr_salewybc[0]["YSLX"].ToString() + "'," + Convert.ToDouble(dt_sale.Rows[i]["卖家违约赔偿总额"]) + ",'" + dr_salewybc[0]["XM"].ToString() + "','" + dr_salewybc[0]["XZ"].ToString() + "','" + zy_salewybc + "','" + jkbh + "','','" + dr_salewybc[0]["SJLX"].ToString() + "',1,'admin','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";
                    al.Add(sql_Salewybc);

                    //将卖家钱的变动增加到dt_Money中
                    dt_Money.Rows.Add(new object[] { dt_sale.Rows[i]["T_YSTBDDLYX"].ToString(), jiedongjine, '加', dr_salejd[0]["SJLX"].ToString(), });//卖家投标保证金解冻
                    dt_Money.Rows.Add(new object[] { dt_sale.Rows[i]["T_YSTBDDLYX"].ToString(), Convert.ToDouble(dt_sale.Rows[i]["卖家违约赔偿总额"].ToString()), '减', dr_salewybc[0]["SJLX"].ToString(), });//卖家违约赔偿

                    //向《平台提醒信息表》中插入给对应的卖家的提醒
                    //Hashtable ht = new Hashtable();
                    DataRow ht = listTX.NewRow();
                    ht["type"] = "集合经销平台";
                    ht["提醒对象登陆邮箱"] = dt_sale.Rows[i]["T_YSTBDDLYX"].ToString();
                    //ht["提醒对象用户名"] = "";//用户名不用传参数，其他地方已处理
                    ht["提醒对象结算账户类型"] = dt_sale.Rows[i]["T_YSTBDJSZHLX"];
                    ht["提醒对象角色编号"] = dt_sale.Rows[i]["T_YSTBDMJJSBH"].ToString();
                    ht["提醒对象角色类型"] = "卖家";
                    ht["提醒内容文本"] = "因您未能按时冻结履约保证金，您所中标的" + dt_sale.Rows[i]["T_YSTBDBH"] + "投标单作废，对应" + dt_sale.Rows[i]["卖家违约赔偿总额"].ToString() + "元的投标保证金已全额赔付给相应买方，作为补偿。";
                    ht["创建人"] = "admin";
                    ht["查看地址"] = "";
                    listTX.Rows.Add(ht);
                    alLog.Add("提醒：" + ht["提醒对象登陆邮箱"].ToString() + "→" + ht["提醒内容文本"].ToString());
                }
                alLog.AddRange(al);
                #endregion

                #region //处理钱的数据，获取每个人应该变更的余额数据
                DataTable dt_Money_end = new DataTable();
                dt_Money_end.Columns.Add("登录邮箱");
                dt_Money_end.Columns.Add("金额");
                if (dt_Money != null && dt_Money.Rows.Count > 0)
                {
                    //开始数量合并
                    for (int i = 0; i < dt_Money.Rows.Count; i++)
                    {
                        //是否需要更新新表
                        bool needadd = true;
                        for (int p = 0; p < dt_Money_end.Rows.Count; p++)
                        {
                            //发现匹配列，开始更新新表原有数据。
                            if (dt_Money_end.Rows[p]["登录邮箱"].ToString() == dt_Money.Rows[i]["登录邮箱"].ToString() && dt_Money.Rows[i]["数据类型"].ToString() == "实")
                            {
                                if (dt_Money.Rows[i]["运算"].ToString() == "加")
                                {
                                    dt_Money_end.Rows[p]["金额"] = (Convert.ToDouble(dt_Money_end.Rows[p]["金额"]) + Convert.ToDouble(dt_Money.Rows[i]["金额"])).ToString("#0.00");
                                }
                                else if (dt_Money.Rows[i]["运算"].ToString() == "减")
                                {
                                    dt_Money_end.Rows[p]["金额"] = (Convert.ToDouble(dt_Money_end.Rows[p]["金额"]) - Convert.ToDouble(dt_Money.Rows[i]["金额"])).ToString("#0.00");
                                }
                                //标记为无需添加新行,此行被合并计算了
                                needadd = false;
                            }
                        }
                        if (needadd)
                        {
                            dt_Money_end.Rows.Add(new object[] { dt_Money.Rows[i]["登录邮箱"].ToString(), dt_Money.Rows[i]["金额"].ToString() });
                        }
                    }
                }
                #endregion

                #region 扣罚卖家信用评分 EditTime 2013-09 // wyh 2014.07.28 add
                DataTable dt_saleXY = ds_info.Tables[0].DefaultView.ToTable(true, new string[] { "T_YSTBDDLYX", "Z_HTBH" });
                DataTable dtjf = new DataTable("jf");
                DataColumn dc = new DataColumn("合同编号", typeof(string));
                dtjf.Columns.Add(dc);
                dtjf.Clear();
                DataRow drht = null;//dt.NewRow();
                for (int i = 0; i < dt_saleXY.Rows.Count; i++)
                {
                    drht = dtjf.NewRow();
                    drht["合同编号"] = dt_saleXY.Rows[i]["Z_HTBH"].ToString();
                    dtjf.Rows.Add(drht);
                }

                dtjf.AcceptChanges();
                IPC.Call("信用等级积分处理", new object[] { "超期未缴纳履约保证金监控", dtjf });


                #endregion

                //执行余额更新程序
                for (int i = 0; i < dt_Money_end.Rows.Count; i++)
                {
                    string sqlUpdateMoney = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE + " + dt_Money_end.Rows[i]["金额"].ToString() + " where B_DLYX='" + dt_Money_end.Rows[i]["登录邮箱"].ToString() + "'";
                    al.Add(sqlUpdateMoney);
                    alLog.Add("注册表余额更新：" + dt_Money_end.Rows[i]["登录邮箱"].ToString() + "→" + dt_Money_end.Rows[i]["金额"].ToString());
                }

                #region 执行所有需要执行的内容
                //开始执行sql语句
                try
                {
                    //执行状态更新及账款流水插入的sql语句
                    //DbHelperSQL.ExecSqlTran(al);
                    Hashtable HTAllSQL = I_DBL.RunParam_SQL(al);
                    if (!(bool)HTAllSQL["return_float"])
                    {
                        ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "状态更新及账款流水写入过程出现错误：" + HTAllSQL["return_errmsg"].ToString();
                        return ht_result;
                    }
                }
                catch (Exception ex)
                {
                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "状态更新及账款流水写入过程出现错误：" + ex.ToString();
                    return ht_result;
                }
                #region//作废
                //try
                //{
                //    //执行余额更新程序
                //    for (int i = 0; i < dt_Money_end.Rows.Count; i++)
                //    {
                //        //ClassMoney2013 CM2013 = new ClassMoney2013();
                //        //CM2013.FreezeMoneyT(dt_Money_end.Rows[i]["登录邮箱"].ToString(), dt_Money_end.Rows[i]["金额"].ToString(), "+");
                //        //alLog.Add("注册表余额更新：" + dt_Money_end.Rows[i]["登录邮箱"].ToString() + "→" + dt_Money_end.Rows[i]["金额"].ToString());

                //        string sqlUpdateMoney = " update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE + " + dt_Money_end.Rows[i]["金额"].ToString() + " where B_DLYX='" + dt_Money_end.Rows[i]["登录邮箱"].ToString() + "'";
                //        Hashtable returnUpdateMoney = I_DBL.RunProc(sqlUpdateMoney);
                //        if ((bool)returnUpdateMoney["return_float"])
                //        {
                //            alLog.Add("注册表余额更新：" + dt_Money_end.Rows[i]["登录邮箱"].ToString() + "→" + dt_Money_end.Rows[i]["金额"].ToString());
                //        }
                //        else
                //        {
                //            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "更新余额的过程出现错误：" + returnUpdateMoney["return_errmsg"].ToString();
                //            return ht_result;
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "更新余额的过程出现错误：" + ex.ToString();
                //    return ht_result;
                //}
                #endregion
                //try
                //{
                //    //发送提醒
                //    PublicClass2013.Sendmes(listTX);
                //}
                //catch (Exception ex)
                //{
                //    ht_result["执行结果"] = "err";
                //    ht_result["提示文本"] = "发送提醒过程出现错误：" + ex.ToString();
                //    return ht_result;
                //}
                //发送提醒
                DataSet SenMessData = new DataSet();
                SenMessData.Tables.Add(listTX);
                object[] reSendMess = IPC.Call("发送提醒", new object[] { SenMessData });
                if (reSendMess[0].ToString() != "ok")
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                    ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = "发送提醒过程出现错误：" + reSendMess[1].ToString();
                    return ht_result;
                }

                #endregion
            }

            return ht_result;
        }
        catch (Exception ex)
        {
            ht_result.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            ht_result.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return ht_result;
        }
    }
    #endregion

    /// <summary>
    /// 监控正常 wyh 2014.07.05
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public static DataSet SupervisoryControlNormal(DataTable dataTable)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        string strNumner = "1310000001";
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable htpar = new Hashtable();
            htpar["@Number"] = strNumner;
            //执行语句      
            DataSet dstx = null;//DbHelperSQL.Query("select YCFSSJ,RJYHSJ,YCXCBH  from AAA_JKXXSJB where Number='" + strNumner + "'");
            Hashtable ht_return = I_DBL.RunParam_SQL("select YCFSSJ,RJYHSJ,YCXCBH  from AAA_JKXXSJB where Number=@Number", "ds", htpar);
            if ((bool)ht_return["return_float"])
            {
                dstx = (DataSet)ht_return["return_ds"];
            }
            if (dstx != null && dstx.Tables[0].Rows.Count > 0)
            {
                string[] strArray = dstx.Tables[0].Rows[0]["YCXCBH"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> listArray = new List<string>();
                listArray.AddRange(strArray);
                if (listArray.Count <= 0)//没有异常监控线程编号
                {
                    ht_return = I_DBL.RunParam_SQL("update AAA_JKXXSJB set YCFSSJ=null,RJYHSJ=GETDATE() where Number=@Number", htpar);
                   // DbHelperSQL.ExecuteSql("update AAA_JKXXSJB set YCFSSJ=null,RJYHSJ=GETDATE() where Number='" + strNumner + "'");
                    if ((bool)ht_return["return_float"])
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "监控记录信息更新完毕";
                        return dsreturn;
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "监控记录信息失败：" + ht_return["return_errmsg"].ToString();
                        return dsreturn;
                    }
                }
                else
                {
                    if (listArray.Contains(dataTable.Rows[0]["线程编号"].ToString()))
                    {
                        listArray.Remove(dataTable.Rows[0]["线程编号"].ToString());
                    }
                    //DbHelperSQL.ExecuteSql("update AAA_JKXXSJB set YCXCBH='" + String.Join("|", listArray.ToArray()) + "' where Number='" + strNumner + "'");
                    string strsql = "update AAA_JKXXSJB set YCXCBH='" + String.Join("|", listArray.ToArray()) + "' where Number='" + strNumner + "'";
                    I_DBL.RunProc(strsql);
                    //重新读取数据信息
                    DataSet dstxAgin = null;//DbHelperSQL.Query("select YCFSSJ,RJYHSJ,YCXCBH  from AAA_JKXXSJB where Number='" + strNumner + "'");
                    strsql = "select YCFSSJ,RJYHSJ,YCXCBH  from AAA_JKXXSJB where Number=@Number";
                    ht_return = I_DBL.RunParam_SQL(strsql, "tx", htpar);
                    if ((bool)ht_return["return_float"])
                    {
                        dstxAgin = (DataSet)ht_return["return_ds"];
                    }


                    string[] strArrayAgin = dstxAgin.Tables[0].Rows[0]["YCXCBH"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> listArrayAgin = new List<string>();
                    listArray.AddRange(strArrayAgin);
                    if (listArrayAgin.Count <= 0)
                    {
                        I_DBL.RunProc("update AAA_JKXXSJB set YCFSSJ=null,RJYHSJ=GETDATE() where Number='" + strNumner + "'");
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "监控记录信息更新完毕";
                        return dsreturn;
                    }
                    else
                    {
                        I_DBL.RunProc("update AAA_JKXXSJB set RJYHSJ=GETDATE() where Number='" + strNumner + "'");
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "监控记录信息更新完毕";
                        return dsreturn;
                    }
                }

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到监控记录信息";
                return dsreturn;
            }
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }

    /// <summary>
    /// 监控异常 wyh 2014.07.05
    /// </summary>
    /// <returns></returns>
    public static DataSet SupervisoryControlAbnormal(DataTable dataTable)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = ReturnDt(); 
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        string strNumber = "1310000001";
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable htpar = new Hashtable();
            htpar["@Number"] = strNumber;
            //执行语句      
            DataSet dstx = null;//DbHelperSQL.Query("select YCFSSJ,RJYHSJ,YCXCBH  from AAA_JKXXSJB where Number='" + strNumber + "'");
            Hashtable ht_return = I_DBL.RunParam_SQL("select YCFSSJ,RJYHSJ,YCXCBH  from AAA_JKXXSJB where Number=@Number", "ds", htpar);
            if ((bool)ht_return["return_float"])
            {
                dstx = (DataSet)ht_return["return_ds"];
            }
            if (dstx != null && dstx.Tables[0].Rows.Count > 0)
            {
                string strSql = "";
                if (dstx.Tables[0].Rows[0]["YCFSSJ"].ToString() == "")//异常发生时间为Null值
                {
                    string[] strArray = dstx.Tables[0].Rows[0]["YCXCBH"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> listArray = new List<string>();
                    listArray.AddRange(strArray);
                    string strYCXCBH = "";
                    if (listArray.Contains(dataTable.Rows[0]["线程编号"].ToString()))
                    {
                        strYCXCBH = String.Join("|", listArray.ToArray());
                    }
                    else
                    {
                        listArray.Add(dataTable.Rows[0]["线程编号"].ToString());
                        strYCXCBH = String.Join("|", listArray.ToArray());
                    }
                    strSql = "update AAA_JKXXSJB set YCFSSJ=GETDATE(),RJYHSJ=GETDATE(),YCXCBH='" + strYCXCBH + "' where Number='" + strNumber + "'";
                }
                else
                {
                    string[] strArray = dstx.Tables[0].Rows[0]["YCXCBH"].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> listArray = new List<string>();
                    listArray.AddRange(strArray);
                    string strYCXCBH = "";
                    if (listArray.Contains(dataTable.Rows[0]["线程编号"].ToString()))
                    {
                        strYCXCBH = String.Join("|", listArray.ToArray());
                    }
                    else
                    {
                        listArray.Add(dataTable.Rows[0]["线程编号"].ToString());
                        strYCXCBH = String.Join("|", listArray.ToArray());
                    }
                    strSql = "update AAA_JKXXSJB set RJYHSJ=GETDATE(),YCXCBH='" + strYCXCBH + "' where Number='" + strNumber + "'";

                }

                I_DBL.RunProc(strSql);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "监控记录信息更新完毕";
                return dsreturn;
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到监控记录信息";
                return dsreturn;
            }

        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.ToString();
            return dsreturn;
        }
    }
}