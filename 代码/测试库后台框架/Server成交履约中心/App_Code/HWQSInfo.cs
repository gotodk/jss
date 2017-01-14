/************************************************************************************
 * 
 * 创建人：wyh
 *
 *创建时间：2014.06.09
 *
 *代码功能：实现货物签收各项功能
 *
 *参考文档：《业务分析》成交履约中心部分
 * 
 * 
 * 
 * ************************************************************************************/
using FMDBHelperClass;
using FMipcClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// HWQSInfo 的摘要说明
/// </summary>
public class HWQSInfo
{
    DataRow htt = null;
	public HWQSInfo()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 货物签收 wyh 2014.06.09 
    /// </summary>
    /// <param name="ht">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    public DataSet dsjhjx_hwqs( DataTable ht)
    {
        DataSet dsreturn = new TBD().initReturnDataSet().Copy();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        if (ht == null || ht.Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空!";
        }
        DataRow dr = ht.Rows[0];
       // htt = PublicClass2013.GetParameterInfo();------------------已经移植到系统控制中心
        object[] re = IPC.Call("平台动态参数", new object[] { "" });
        if (re[0].ToString() == "ok")
        {
            DataSet dspar = (DataSet)(re[1]);
            if (dspar != null && dspar.Tables[0].Rows.Count > 0)
            {
                htt = dspar.Tables[0].Rows[0];
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取平台动态参数表出错!";
                return dsreturn;
            }
            
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取平台动态参数表出错!";
            return dsreturn;
        }
        
        //cm = new ClassMoney2013();
        Hashtable htresul = null;


        string Type = dr["type"].ToString();
        switch (Type)
        {
            case "无异议收货":
            case "补发货物无异议收货":
            case "有异议收货后无异议收货":
                dsreturn = wyysh_strs(ht);
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "wyy";
                break;
            case "请重新发货":
                dsreturn = qcxsh_strs(ht);
               
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
                break;
            case "部分收货":
                dsreturn = bufensh_strs(ht);
               
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
                break;
            case "有异议收货":
                dsreturn = wyyth_strs(ht);
               
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
                break;

            default:
                break;
        }



        return dsreturn;

    }

    //无疑义提货操作
    /// <summary>
    ///无疑义提货操作 wyh 2014.06.11 add
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    public DataSet wyysh_strs(DataTable ds)
    {
        DataSet dsreturn = new TBD().initReturnDataSet().Copy();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        try
        {       
            if (ds == null || ds.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递参数不可为空!";
                return dsreturn;
            }

            object[] re = IPC.Call("平台动态参数", new object[] { "" });
            if (re[0].ToString() == "ok")
            {
                DataSet dspar = (DataSet)(re[1]);
                if (dspar != null && dspar.Tables[0].Rows.Count > 0)
                {
                    htt = dspar.Tables[0].Rows[0];
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取平台动态参数表出错!";
                    return dsreturn;
                }

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取平台动态参数表出错!";
                return dsreturn;
            }


            DataRow drs = ds.Rows[0];
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            DataSet dsparr = new DataSet();
            Hashtable input = new Hashtable();
            Hashtable htreturn = new Hashtable();
         
            string action = drs["ispass"].ToString().Trim();
            ArrayList al = new ArrayList();
            input["@Number"] = drs["Number"].ToString().Trim();
            input["@Y_YSYDDMJJSBH"] = drs["买家角色编号"].ToString();
            DataSet dsgetinfo = null;
            //获得当前提货单据详细信息
            string Strgetinfo = "select a.Number,('T'+a.Number) as  提货单号,('F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖家名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买家角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.T_YSTBDMJJSBH as 卖家角色编号,b.T_YSTBDDLYX as 卖家登陆邮箱,b.T_YSTBDJSZHLX as 卖家角色账户类型,b.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,b.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,b.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,b.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,b.Z_ZBSL as 定标数量,b.Z_LYBZJJE as 履约保证金金额,b.Z_HTZT as 合同状态,'买家名称'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_BUYJSBH=b.Y_YSYDDMJJSBH),b.Y_YSYDDJSZHLX  买家结算账户类型,'买家关联经纪人名称'=(select I_JYFMC from AAA_DLZHXXB a where a.J_JJRJSBH=b.Y_YSYDDGLJJRJSBH),'买家关联经纪人结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB a where a.J_JJRJSBH=b.Y_YSYDDGLJJRJSBH),'卖家关联经纪人名称'=(select I_JYFMC from AAA_DLZHXXB a where a.J_JJRJSBH=b.T_YSTBDGLJJRJSBH),'卖家关联经纪人结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB a where a.J_JJRJSBH=b.T_YSTBDGLJJRJSBH) from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX  where a.Number=@Number and  (a.F_DQZT ='已录入发货信息' or a.F_DQZT ='已录入补发备注' or a.F_DQZT ='有异议收货') and b.Y_YSYDDMJJSBH=@Y_YSYDDMJJSBH";
            htreturn = I_DBL.RunParam_SQL(Strgetinfo,"info",input);
            if ((bool)(htreturn["return_float"])) //说明执行完成
            {
                //这里就可以用了。
                dsgetinfo = (DataSet)(htreturn["return_ds"]);
                
            }


            if (dsgetinfo != null && dsgetinfo.Tables["info"].Rows.Count == 1)
            {

                DataRow drinfo = dsgetinfo.Tables["info"].Rows[0];
                if (!drinfo["合同状态"].ToString().Trim().Equals("定标"))
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "提货单合同状态不是定标!";
                    return dsreturn;
                    
                }
                if (action.Equals("ok"))
                {
                    TBD tbd = new TBD();
                    Hashtable htall = new Hashtable();
                    #region 获取卖家卖家账户当前余额
                    //获得当前卖家卖家余额
                   // string Strbuyyue = " select B_ZHDQKYYE  from AAA_DLZHXXB where B_DLYX = '" + drs["登陆邮箱"].ToString().Trim() + "'  ";
                    //object objbuy = DbHelperSQL.GetSingle(Strbuyyue);
                    double buyyue = 0.00;
                    double sellyue = 0.00;
                    #region//zhouli 作废 2014.07.24 逻辑错误
                    // re = IPC.Call("账户当前可用余额", new object[] { drs["登陆邮箱"].ToString().Trim() });
                    //if (re[0].ToString() == "ok")
                    //{
                    //    string  dspar = (string)(re[1]);
                    //    if (dspar != null && !dspar.Equals(""))
                    //    {
                    //        buyyue = Convert.ToDouble(dspar);
                    //    }
                    //    else
                    //    {
                    //        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    //        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从登陆信息表中查找到任何关于" + drs["登陆邮箱"].ToString().Trim() + "的信息！";
                    //        return dsreturn;
                           
                    //    }

                    //}
                    //else
                    //{
                    //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从登陆信息表中查找到任何关于" + drs["登陆邮箱"].ToString().Trim() + "的信息！";
                    //    return dsreturn;
                        
                    //}



                    //re = IPC.Call("账户当前可用余额", new object[] { drinfo["卖家登陆邮箱"].ToString().Trim() });
                    //if (re[0].ToString() == "ok")
                    //{
                    //    string dspar = (string)(re[1]);
                    //    if (dspar != null && !dspar.Equals(""))
                    //    {
                    //        sellyue = Convert.ToDouble(dspar);
                    //    }
                    //    else
                    //    {
                    //        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    //        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从登陆信息表中查找到任何关于" + drinfo["卖家登陆邮箱"].ToString().Trim() + "的信息！";
                    //        return dsreturn;
                            
                    //    }

                    //}
                    //else
                    //{
                    //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从登陆信息表中查找到任何关于" + drinfo["卖家登陆邮箱"].ToString().Trim() + "的信息！";
                    //    return dsreturn;

                    //}
                    #endregion

                    #endregion
                    DateTime drnow = DateTime.Now;
                    DataRow drhkjd = null;
                    string Keynumber = "";
                    #region 货物签收解冻买家货款
                    //货物签收解冻买家货款
                    DataSet dshkjd = tbd.GetMoneydbzDS("1304000010");

                    if (dshkjd != null && dshkjd.Tables["DZB"].Rows.Count == 1)
                    {
                        re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });
                        if (re[0].ToString() == "ok")
                        {
                            string reFF = (string)(re[1]);
                            if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错1！";
                                return dsreturn;
                                
                            }
                            Keynumber = reFF;
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错1！";
                            return dsreturn;
                            
                        }

                        drhkjd = dshkjd.Tables["DZB"].Rows[0];
                        htall["@Keynumber"] = Keynumber;
                        htall["@DLYX"] = drs["登陆邮箱"].ToString();
                        htall["@ZY1"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());
                        htall["@YSLX1"] = drhkjd["YSLX"].ToString();
                        htall["@time"] = drnow;
                        htall["@JSZHLX"] = drs["结算账户类型"].ToString();
                        htall["@JSBH1"] = drs["买家角色编号"].ToString();
                        htall["@LYYWLX"] = "AAA_THDYFHDXXB";
                        htall["@LYDH1"] = drinfo["Number"].ToString();
                     
                        htall["@JE1"] = drinfo["发货金额"].ToString().Trim();
                        htall["@XM1"] = drhkjd["XM"].ToString();
                        htall["@XZ1"] = drhkjd["XZ"].ToString();
                       
                        htall["@SJLX1"] = drhkjd["SJLX"].ToString();
                        htall["@JHBH"] = "监控编号2";

                       
                        //货物签收解冻买家货款sql
                        string strinsehwqshkjd = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber,@DLYX,@JSZHLX,@JSBH1,@LYYWLX,@LYDH1,@time,@YSLX1,@JE1,@XM1,@XZ1,@ZY1,@SJLX1,@DLYX,@time,@JHBH ) ";
                        al.Add(strinsehwqshkjd);
                        buyyue += Convert.ToDouble(drinfo["发货金额"].ToString().Trim());
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "为从资金表动明细表中查找到任何关于货物签收无疑义解冻货款的信息！";
                        return dsreturn;
                       
                    }
                    #endregion

                    #region 签收后扣减货款
                    //签收后扣减货款
                    DataSet dskjhk = tbd.GetMoneydbzDS("1304000012");

                    if (dskjhk != null && dskjhk.Tables["DZB"].Rows.Count == 1)
                    {
                        drhkjd = dskjhk.Tables["DZB"].Rows[0];                   
                        htall["@ZY2"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());

                        re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//----------------
                        if (re[0].ToString() == "ok")
                        {
                            string reFF = (string)(re[1]);
                            if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错2！";
                                return dsreturn;
                            }

                            Keynumber = reFF;
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错2！";
                            return dsreturn;
                        }

                        htall["@Keynumber1"] = Keynumber;
                        htall["@XM2"] = drhkjd["XM"].ToString();
                        htall["@XZ2"] = drhkjd["XZ"].ToString();
                      //  htall["@ZY2"] = drhkjd["ZY"].ToString();
                        htall["@SJLX2"] = drhkjd["SJLX"].ToString();
                        htall["@YSLX2"] = drhkjd["YSLX"].ToString();
                        //签收后扣减货款sql
                        string strinsehwqshkjd = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber1,@DLYX,@JSZHLX,@JSBH1,@LYYWLX,@LYDH1,@time,@YSLX2,@JE1,@XM2,@XZ2,@ZY2,@SJLX2,@DLYX,@time,@JHBH ) ";

                        al.Add(strinsehwqshkjd);
                        buyyue -= Convert.ToDouble(drinfo["发货金额"].ToString().Trim());
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "为从资金表动明细表中查找到任何关于货物签收无疑义签收扣减货款的信息！";
                        return dsreturn;
        
                    }
                    #endregion

                    #region 向卖家支付货款
                    //向卖家支付货款
                    DataSet dszfhk = tbd.GetMoneydbzDS("1304000014");


                    if (dszfhk != null && dszfhk.Tables["DZB"].Rows.Count == 1)
                    {
                        re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//-----------------
                        if (re[0].ToString() == "ok")
                        {
                            string reFF = (string)(re[1]);
                            if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错3！";
                                return dsreturn;
                            }
                            Keynumber = reFF;
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错3！";
                            return dsreturn;
                        }

                        drhkjd = dszfhk.Tables["DZB"].Rows[0];
                        
                        string jszhlx = "";// PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();
                        re = IPC.Call("用户基本信息", new object[] { drinfo["卖家角色编号"].ToString() });//----------------
                        if (re[0].ToString() == "ok")
                        {
                            DataSet reFF = (DataSet)(re[1]);
                            if (reFF == null || reFF.Tables.Count <= 0 || reFF.Tables[0].Rows.Count<=0)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错4！";
                                return dsreturn;
                            }
                            jszhlx = reFF.Tables[0].Rows[0]["结算账户类型"].ToString();
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错4！";
                            return dsreturn;
                        }

                        htall["@ZY3"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());
                        htall["@Keynumber3"] = Keynumber;
                        htall["@MaiJdlyx"] = drinfo["卖家登陆邮箱"].ToString();
                        htall["@jszhlx3"] = jszhlx;
                        htall["@JSBH3"] = drinfo["卖家角色编号"].ToString();
                        htall["@YSLX3"] = drhkjd["YSLX"].ToString();
                        htall["@XM3"] = drhkjd["XM"].ToString();
                        htall["@XZ3"] = drhkjd["XZ"].ToString();
                       // htall["@ZY3"] = drhkjd["ZY"].ToString();
                        htall["@SJLX3"] = drhkjd["SJLX"].ToString();

                        //向卖家支付货款sql
                        string strinsehwqszfhk = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber3,@MaiJdlyx,@jszhlx3,@JSBH3,@LYYWLX,@LYDH1,@time,@YSLX3,@JE1,@XM3,@XZ3,@ZY3,@SJLX3,@DLYX,@time,@JHBH  ) ";
                        al.Add(strinsehwqszfhk);
                        sellyue += Convert.ToDouble(drinfo["发货金额"].ToString().Trim());

                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从资金表动明细表中查找到任何关于货物签收无疑义签收向卖方支付货款的信息！";
                        return dsreturn;
                        
                    }

                    #endregion

                    #region 扣减卖家技术服务费
                    //扣减卖家技术服务费 

                    double jsfwufei = Convert.ToDouble((Convert.ToDouble(drinfo["发货金额"].ToString().Trim()) * Convert.ToDouble(htt["签收技术服务费比率"].ToString().Trim())).ToString("#0.00"));
                    DataSet dsjsfws = tbd.GetMoneydbzDS("1304000007");

                    if (dsjsfws != null && dsjsfws.Tables["DZB"].Rows.Count > 0)
                    {
                        re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//-----------------
                        if (re[0].ToString() == "ok")
                        {
                            string reFF = (string)(re[1]);
                            if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错5！";
                                return dsreturn;

                            }
                            Keynumber = reFF;
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错5！";
                            return dsreturn;
                        }

                        drhkjd = dsjsfws.Tables["DZB"].Rows[0];
                        htall["@Keynumber4"] = Keynumber;
                        htall["@ZY4"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());
                        
                       
                        htall["@YSLX4"] = drhkjd["YSLX"].ToString();
                        htall["@jsfwufei"] = jsfwufei.ToString();
                        htall["@XM4"] = drhkjd["XM"].ToString();
                        htall["@XZ4"] = drhkjd["XZ"].ToString();
                       // htall["@ZY4"] = drhkjd["ZY"].ToString();
                        htall["@SJLX4"] = drhkjd["SJLX"].ToString();

                        //扣减卖家技术服务费sql
                        string strinsehwqszfhk = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber4,@MaiJdlyx,@jszhlx3,@JSBH3,@LYYWLX,@LYDH1,@time,@YSLX4,@jsfwufei,@XM4,@XZ4,@ZY4,@SJLX4,@DLYX,@time,'监控编号' ) ";
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
                 
                    DataSet dsjjrsybuy = tbd.GetMoneydbzDS("1304000043"); //= DbHelperSQL.Query(Strjjrsyfbuy);
                    if (dsjjrsybuy != null && dsjjrsybuy.Tables["DZB"].Rows.Count == 1)
                    {
                        re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//-----------------
                        if (re[0].ToString() == "ok")
                        {
                            string reFF = (string)(re[1]);
                            if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                            {

                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错6！";
                                return dsreturn;

                               
                            }
                            Keynumber = reFF;
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错6！";
                            return dsreturn;
                        }

                        drhkjd = dsjjrsybuy.Tables["DZB"].Rows[0];
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x2]", drinfo["发货单号"].ToString().Trim());
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drs["登陆邮箱"].ToString().Trim());

                        string jszhlx = "";//PublicClass2013.GetUserInfo(drinfo["买家关联经纪人角色编号"].ToString())["结算账户类型"].ToString();
                        re = IPC.Call("用户基本信息", new object[] { drinfo["买家关联经纪人角色编号"].ToString()});//----------------
                        if (re[0].ToString() == "ok")
                        {
                            DataSet reFF = (DataSet)(re[1]);
                            if (reFF == null || reFF.Tables.Count <= 0 || reFF.Tables[0].Rows.Count <= 0)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取用户基本信息出错5！";
                                return dsreturn;
                                
                            }
                            jszhlx = reFF.Tables[0].Rows[0]["结算账户类型"].ToString();
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取用户基本信息出错5！";
                            return dsreturn;
                        }

                        buyjjrsy = (jsfwufei * Convert.ToDouble(htt["经纪人收益买家比率"].ToString().Trim())).ToString("#0.00");

                        htall["@Keynumber5"] = Keynumber;
                        htall["@MaiJgljjryx"] = drinfo["买家关联经纪人邮箱"].ToString();
                        htall["@jszhlx5"] = jszhlx;
                        htall["@JSBH5"] = drinfo["买家关联经纪人角色编号"].ToString();
                        htall["@YSLX5"] = drhkjd["YSLX"].ToString();
                        htall["@buyjjrsy5"] = buyjjrsy.ToString();
                        htall["@XM5"] = drhkjd["XM"].ToString();
                        htall["@XZ5"] = drhkjd["XZ"].ToString();
                        htall["@ZY5"] = drhkjd["ZY"].ToString();
                        htall["@SJLX5"] = drhkjd["SJLX"].ToString();


                        //来自买家的经纪人收益sql
                        string strinsehwqsbuyjjrsy = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber5,@MaiJgljjryx,@jszhlx5,@JSBH5,@LYYWLX,@LYDH1,@time,@YSLX5,@buyjjrsy5,@XM5,@XZ5,@ZY5,@SJLX5,@DLYX,@time,'监控编号' ) ";
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
                    
                    DataSet dsjjrsysell = tbd.GetMoneydbzDS("1304000042");//DbHelperSQL.Query(Strjjrsyfsell);
                    if (dsjjrsysell != null && dsjjrsysell.Tables["DZB"].Rows.Count == 1)
                    {
                        drhkjd = dsjjrsysell.Tables["DZB"].Rows[0];
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x2]", drinfo["发货单号"].ToString().Trim());
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["卖家登陆邮箱"].ToString().Trim());
                       // Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//-----------------
                        if (re[0].ToString() == "ok")
                        {
                            string reFF = (string)(re[1]);
                            if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错7！";
                                return dsreturn;
                            }
                            Keynumber = reFF;
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错7！";
                            return dsreturn;
                        }


                        string jszhlx = "";//PublicClass2013.GetUserInfo(drinfo["卖家关联经纪人角色编号"].ToString())["结算账户类型"].ToString();
                        re = IPC.Call("用户基本信息", new object[] { drinfo["卖家关联经纪人角色编号"].ToString() });//----------------
                        if (re[0].ToString() == "ok")
                        {
                            DataSet reFF = (DataSet)(re[1]);
                            if (reFF == null || reFF.Tables.Count <= 0 || reFF.Tables[0].Rows.Count <= 0)
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取用户基本信息出错6！";
                                return dsreturn;
                                
                            }
                            jszhlx = reFF.Tables[0].Rows[0]["结算账户类型"].ToString();
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取用户基本信息出错6！";
                            return dsreturn;
                        }

                        buyjjrsell = (jsfwufei * Convert.ToDouble(htt["经纪人收益卖家比率"].ToString().Trim())).ToString("#0.00");
                        htall["@Keynumber6"] = Keynumber;
                        htall["@Buygljjryx"] = drinfo["卖家关联经纪人邮箱"].ToString();
                        htall["@jszhlx6"] = jszhlx;
                        htall["@JSBH6"] = drinfo["卖家关联经纪人角色编号"].ToString();
                        htall["@YSLX6"] = drhkjd["YSLX"].ToString();
                        htall["@JE6"] = buyjjrsell.ToString();
                        htall["@XM6"] = drhkjd["XM"].ToString();
                        htall["@XZ6"] = drhkjd["XZ"].ToString();
                        htall["@ZY6"] = drhkjd["ZY"].ToString();
                        htall["@SJLX6"] = drhkjd["SJLX"].ToString();

                        //来自卖家的经纪人收益sql
                        string strinsehwqsselljjrsy = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber6,@Buygljjryx,@jszhlx6,@JSBH6,@LYYWLX,@LYDH1,@time,@YSLX6,@JE6,@XM6,@XZ6,@ZY6,@SJLX6,@DLYX,@time,'监控编号' ) ";
                        al.Add(strinsehwqsselljjrsy);

                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未从资金表动明细表中查找到任何关于来自买方的经纪人收益的基本信息！";
                        return dsreturn;
                    }

                    #endregion

                    #region 满足买家无异议签收数量=定标数量是单据处理
                    int zbNum = Convert.ToInt32(drinfo["定标数量"].ToString().Trim());
                    int thbc = Convert.ToInt32(drinfo["提货数量"].ToString().Trim());
                    int ythNum = 0;
                    Hashtable htsl = new Hashtable();
                    htsl["@zbdbsl"] = drinfo["中标定标单号"].ToString();
                    string StrgetyithNum = "select  isnull(SUM (ISNULL(T_THSL,0)),0) as  已提货数量,ZBDBXXBBH     from AAA_THDYFHDXXB where ZBDBXXBBH = @zbdbsl and F_DQZT in ( '无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货','卖家主动退货' ) group by ZBDBXXBBH";
                    DataSet dsyth = null;//DbHelperSQL.Query(StrgetyithNum);
                    htreturn = I_DBL.RunParam_SQL(StrgetyithNum,"thinfo", htsl);
                    if ((bool)(htreturn["return_float"])) //说明执行完成
                    {
                        //这里就可以用了。
                        dsyth = (DataSet)(htreturn["return_ds"]);
                       
                    }


                    if (dsyth != null && dsyth.Tables["thinfo"].Rows.Count > 0)
                    {
                        ythNum = Convert.ToInt32(dsyth.Tables["thinfo"].Rows[0]["已提货数量"].ToString());
                    }

                    if (zbNum == thbc + ythNum)
                    {
                        #region 解冻卖家履约保证金
                        //解冻卖家履约保证金
                        double lvbzj = Convert.ToDouble(drinfo["履约保证金金额"].ToString().Trim());
                        //无异议提货解冻卖家履约保证金

                        DataSet dswyyjelybzj = tbd.GetMoneydbzDS("1304000048");//DbHelperSQL.Query(Strwyythjdlybzj);
                        if (dswyyjelybzj != null && dswyyjelybzj.Tables["DZB"].Rows.Count > 0)
                        {
                            drhkjd = dswyyjelybzj.Tables["DZB"].Rows[0];

                            drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());

                            Keynumber = ""; //PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                            re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//-----------------
                            if (re[0].ToString() == "ok")
                            {
                                string reFF = (string)(re[1]);
                                if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                                {
                                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错7！";
                                    return dsreturn;
                                    
                                }
                                Keynumber = reFF;
                            }
                            else
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错7！";
                                return dsreturn;
                            }

                            
                            htall["@Keynumber7"] = Keynumber;
                            htall["@zbdbsl"] = drinfo["中标定标单号"].ToString();
                            htall["@YSLX7"] = drhkjd["YSLX"].ToString();
                            htall["@JE7"] = drinfo["履约保证金金额"].ToString();
                            htall["@XM7"] = drhkjd["XM"].ToString();
                            htall["@XZ7"] = drhkjd["XZ"].ToString();
                            htall["@ZY7"] = drhkjd["ZY"].ToString();
                            htall["@SJLX7"] = drhkjd["SJLX"].ToString();


                            //无异议提货解冻卖家履约保证金sql
                            string strinsehwqsjdmjlybzj = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber7,@MaiJdlyx,@jszhlx3,@JSBH3,'AAA_ZBDBXXB',@zbdbsl,@time,@YSLX7,@JE7,@XM7,@XZ7,@ZY7,@SJLX7,@DLYX,@time,'监控编号' ) ";
                            al.Add(strinsehwqsjdmjlybzj);
                            sellyue += Convert.ToDouble(drinfo["履约保证金金额"].ToString());
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无异议提货解冻卖方履约保证金信息未找到！";
                            return dsreturn;
                           
                        }
                        #endregion

                        #region 因发票延期或者发货延期的罚单与补偿
                        //获得所有相同中标定标单号的提货单编号的发货单号

                        string GetTHdNumber = " select Number from  AAA_THDYFHDXXB where ZBDBXXBBH = '" + drinfo["中标定标单号"].ToString() + "' ";

                        DataSet dsfpyq = (DataSet)I_DBL.RunProc(GetTHdNumber,"thdNUm")["return_ds"];

                        if (dsfpyq != null && dsfpyq.Tables["thdNUm"].Rows.Count > 0)
                        {
                            string Strwhere = "";
                            foreach (DataRow dr in dsfpyq.Tables["thdNUm"].Rows)
                            {
                                Strwhere += "'" + dr["Number"].ToString().Trim() + "',";
                            }

                            Strwhere = Strwhere.Trim().Remove(Strwhere.Length - 1);

                            # region 发票延期的扣罚与补赏
                            //获取次投标定标预订单的发票延迟预扣款流水明细
                            double je = 0.00;
                            string Strfpykmc = "select Number, JE from AAA_ZKLSMXB where XM = '违约赔偿金' and XZ = '发货单生成后5日内未录入发票邮寄信息' and LYDH in (" + Strwhere + ") and SJLX='预'";
                            DataSet dsfpykmc = (DataSet)I_DBL.RunProc(Strfpykmc,"zklsinfo")["return_ds"];//DbHelperSQL.Query(Strfpykmc);
                            if (dsfpykmc != null && dsfpykmc.Tables["zklsinfo"].Rows.Count > 0)
                            {
                                foreach (DataRow drfpykk in dsfpykmc.Tables["zklsinfo"].Rows)
                                {
                                    je += Convert.ToDouble(drfpykk["JE"].ToString().Trim());
                                }

                                //卖家扣罚(发货单生成后5日内未录入发票邮寄信息)

                            
                                DataSet dsStrfpyqmjkf = tbd.GetMoneydbzDS("1304000028");//DbHelperSQL.Query(Strfpyqmjkf);
                                if (dsStrfpyqmjkf != null && dsStrfpyqmjkf.Tables["DZB"].Rows.Count > 0)
                                {
                                    drhkjd = dsStrfpyqmjkf.Tables["DZB"].Rows[0];

                                    drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    Keynumber = "";//PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//-----------------
                                    if (re[0].ToString() == "ok")
                                    {
                                        string reFF = (string)(re[1]);
                                        if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                                        {

                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错8！";
                                            return dsreturn;

                                        }
                                        Keynumber = reFF;
                                    }
                                    else
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错8！";
                                        return dsreturn;
                                    }
                                    htall["@Keynumber8"] = Keynumber;
                                    htall["@YSLX8"] = drhkjd["YSLX"].ToString();
                                    htall["@je8"] = je.ToString();
                                    htall["@XM8"] = drhkjd["XM"].ToString();
                                    htall["@XZ8"] = drhkjd["XZ"].ToString();
                                    htall["@ZY8"] = drhkjd["ZY"].ToString();
                                    htall["@SJLX8"] = drhkjd["SJLX"].ToString();
                                    
                                   // string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber8,@MaiJdlyx,@jszhlx3,@JSBH3,'AAA_ZBDBXXB',@zbdbsl,@time,@YSLX8,@je8,@XM8,@XZ8,@ZY8,@SJLX8,@DLYX,@time,'监控编号' ) ";
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

                               
                                DataSet dsStrfpyqmjbs = tbd.GetMoneydbzDS("1304000037");//DbHelperSQL.Query(Strfpyqmjbs);
                                if (dsStrfpyqmjbs != null && dsStrfpyqmjbs.Tables["DZB"].Rows.Count > 0)
                                {
                                    drhkjd = dsStrfpyqmjbs.Tables["DZB"].Rows[0];

                                    drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    Keynumber = "";// PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//-----------------
                                    if (re[0].ToString() == "ok")
                                    {
                                        string reFF = (string)(re[1]);
                                        if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错9！";
                                            return dsreturn;
                                        }
                                        Keynumber = reFF;
                                    }
                                    else
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错9！";
                                        return dsreturn;
                                    }
                                     //string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString())["结算账户类型"].ToString();
                                    htall["@Keynumber9"] = Keynumber;
                                    htall["@YSLX9"] = drhkjd["YSLX"].ToString();
                                    htall["@XM9"] = drhkjd["XM"].ToString();
                                   
                                    htall["@XZ9"] = drhkjd["XZ"].ToString();
                                    htall["@ZY9"] = drhkjd["ZY"].ToString();
                                    htall["@SJLX9"] = drhkjd["SJLX"].ToString();
                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber9,@DLYX,@JSZHLX,@JSBH1,'AAA_ZBDBXXB',@zbdbsl,@time,@YSLX9,@je8,@XM9,@XZ9,@ZY9,@SJLX9,@DLYX,@time,'监控编号' ) ";
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
                            //获取次投标定标预订单的发票延迟预扣款流水明细
                            double hwje = 0.00;
                            string Strhwykmc = "select Number, JE from AAA_ZKLSMXB where XM = '违约赔偿金' and XZ = '超过最迟发货日后录入发货信息' and LYDH in (" + Strwhere + ") and SJLX='预'";
                            DataSet dshwykmc = (DataSet)I_DBL.RunProc(Strhwykmc,"info")["return_ds"];//DbHelperSQL.Query(Strhwykmc);
                            if (dshwykmc != null && dshwykmc.Tables["info"].Rows.Count > 0)
                            {
                                foreach (DataRow drfpykk in dshwykmc.Tables["info"].Rows)
                                {
                                    hwje += Convert.ToDouble(drfpykk["JE"].ToString().Trim());
                                }

                                //卖家扣罚(发货单生成后5日内未录入发票邮寄信息)

                               // string Strfhyqmjkf = "select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000026'";
                                DataSet dsStrfhyqmjkf = tbd.GetMoneydbzDS("1304000026");//DbHelperSQL.Query(Strfhyqmjkf);
                                if (dsStrfhyqmjkf != null && dsStrfhyqmjkf.Tables["DZB"].Rows.Count > 0)
                                {
                                    drhkjd = dsStrfhyqmjkf.Tables["DZB"].Rows[0];

                                    drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    Keynumber = "";// PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");

                                    re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//-----------------
                                    if (re[0].ToString() == "ok")
                                    {
                                        string reFF = (string)(re[1]);
                                        if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错10！";
                                            return dsreturn;
                                            
                                        }
                                        Keynumber = reFF;
                                    }
                                    else
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错10！";
                                        return dsreturn;
                                    }
                                    htall["@Keynumber10"] = Keynumber;
                                    htall["@YSLX10"] = drhkjd["YSLX"].ToString();
                                    htall["@XM10"] = drhkjd["XM"].ToString();
                                    htall["@je10"] = hwje.ToString();
                                    htall["@XZ10"] = drhkjd["XZ"].ToString();
                                    htall["@ZY10"] = drhkjd["ZY"].ToString();
                                    htall["@SJLX10"] = drhkjd["SJLX"].ToString();

                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber10,@MaiJdlyx,@jszhlx3,@JSBH3,'AAA_ZBDBXXB',@zbdbsl,@time,@YSLX10,@je10,@XM10,@XZ10,@ZY10,@SJLX10,@DLYX,@time,'监控编号' ) ";
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

                               // string Strfhyqmjbs = "select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000036'";
                                DataSet dsStrfhyqmjbs = tbd.GetMoneydbzDS("1304000036");//DbHelperSQL.Query(Strfhyqmjbs);
                                if (dsStrfhyqmjbs != null && dsStrfhyqmjbs.Tables["DZB"].Rows.Count > 0)
                                {
                                    drhkjd = dsStrfhyqmjbs.Tables["DZB"].Rows[0];

                                    drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    Keynumber = "";// PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");

                                    re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//-----------------
                                    if (re[0].ToString() == "ok")
                                    {
                                        string reFF = (string)(re[1]);
                                        if ((string)(re[1]) == null || ((string)(re[1])).Equals(""))
                                        {
                                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错11！";
                                            return dsreturn;
                                        }
                                        Keynumber = reFF;
                                    }
                                    else
                                    {
                                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取主键出错11！";
                                        return dsreturn;
                                    }
                                    //string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString())["结算账户类型"].ToString();
                                    htall["@Keynumber11"] = Keynumber;
                                    htall["@YSLX11"] = drhkjd["YSLX"].ToString();
                                    htall["@XM11"] = drhkjd["XM"].ToString();
                                    htall["@je11"] = hwje.ToString();
                                    htall["@XZ11"] = drhkjd["XZ"].ToString();
                                    htall["@ZY11"] = drhkjd["ZY"].ToString();
                                    htall["@SJLX11"] = drhkjd["SJLX"].ToString();
                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values (@Keynumber11,@DLYX,@JSZHLX,@JSBH1,'AAA_ZBDBXXB',@zbdbsl,@time,@YSLX11,@je11,@XM11,@XZ11,@ZY11,@SJLX11,@DLYX,@time,'监控编号' ) ";
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
                            htall["@Z_HTBH"] = drinfo["电子购货合同"].ToString();
                            string Strupdate = " update AAA_ZBDBXXB set Z_HTZT = '定标执行完成' , Z_QPKSSJ=@time,Z_QPJSSJ=@time ,Z_QPZT='清盘结束' where Z_HTBH = @Z_HTBH ";
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
                    htall["@buyyue"] = buyyue.ToString("#0.00");
                    string StrUpbuy = " update   AAA_DLZHXXB set B_ZHDQKYYE =B_ZHDQKYYE+@buyyue  where B_DLYX =@DLYX   ";
                    al.Add(StrUpbuy);
                    htall["@sellyue"] = sellyue.ToString("#0.00");
                    string Strupsell = " update   AAA_DLZHXXB set B_ZHDQKYYE =B_ZHDQKYYE+@sellyue where B_DLYX = @MaiJdlyx ";
                    al.Add(Strupsell);
                    //更改单据状态
                    htall["@type"] = drs["type"].ToString();
                    htall["@Numbera"] = drs["Number"].ToString();
                    string Strupda = " update AAA_THDYFHDXXB set F_DQZT=@type , F_BUYWYYSHCZSJ=@time where Number=@Numbera ";
                    al.Add(Strupda);

                    htreturn = I_DBL.RunParam_SQL(al, htall);
                    if ((bool)(htreturn["return_float"])) //说明执行完成
                    {
                        //这里就可以用了。
                        if ((int)(htreturn["return_other"]) <= 0)   
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行了0条语句！";
                            return dsreturn;
                            
                        }

                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_errmsg"].ToString();
                        return dsreturn;
                           
                    }


                    #region 交易方信用变化情况 edittime 2013-09 准备移植到用户控制中心。
                    //---------------------wyh 2014.07.28 add
                   
            
                        DataTable dt = new DataTable("jf");
                        DataColumn dc = new DataColumn("合同编号",typeof(string));
                        dt.Columns.Add(dc);
                        dt.Clear();
                        DataRow drht = dt.NewRow();
                        drht["合同编号"] = drinfo["电子购货合同"].ToString().Trim();
                        dt.Rows.Add(drht);
                        IPC.Call("信用等级积分处理", new object[] { "无异议收货",dt });

                
                    #endregion

                    #region//提醒信息
                    string Type = drs["type"].ToString();
                    DataSet dsmess = null ;
                    switch (Type)
                    {
                        case "无异议收货":
                           // 您***（编号）发货单，买家已无异议收货，***元的货款已转付至您的交易账户， **元的技术服务费已同时扣收，敬请知悉。

                            dsmess = TX(Type, drinfo, drs, sellyue, jsfwufei, drnow, buyjjrsy, buyjjrsell, zbNum, thbc, ythNum);
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无异议收货操作成功！";
                            break;
                        case "补发货物无异议收货":
                            dsmess = TX(Type, drinfo, drs, sellyue, jsfwufei, drnow, buyjjrsy, buyjjrsell, zbNum, thbc, ythNum);
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "补发货物无异议收货操作成功！";
                          
                            break;
                        case "有异议收货后无异议收货":
                            dsmess = TX(Type, drinfo, drs, sellyue, jsfwufei, drnow, buyjjrsy, buyjjrsell, zbNum, thbc, ythNum);
                             dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                             dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "有异议收货后无异议收货操作成功！"; 
                            break;
                        default: 
                            break;
                    }

                    //调用系统控制中心发送提醒---------------------- wyh 2014.06.09 以后可能会做成一步，不必判断对错。

                    re = IPC.Call("发送提醒", new object[] { dsmess });
                    return dsreturn;

                    #endregion

                }
                else if (action.Equals("no"))
                {
                    string Type = drs["type"].ToString();
                    switch (Type)
                    {
                        case "无异议收货":
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "pass";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "根据您的指令，" + drinfo["发货单号"].ToString() + "发货单的" + drinfo["发货金额"].ToString() + "货款将全额转付至" + drinfo["卖家名称"].ToString() + "账户。";
                            return dsreturn;
                           
                        case "补发货物无异议收货": 
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "pass";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "根据您的“补发货物无异议收货”指令，\n\r" + drinfo["发货单号"].ToString() + "发货单的" + drinfo["商品名称"].ToString() + "商品您已全部无异议收货(含所有补货)，\n\r" + drinfo["发货金额"].ToString() + "元的货款将全额转付至" + drinfo["卖家名称"].ToString() + "账户。是否确认？";  
                            return dsreturn;
                        case "有异议收货后无异议收货":
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "pass";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "根据您的“改为无异议收货”指令，\n\r" + drinfo["发货单号"].ToString() + "发货单的" + drinfo["商品名称"].ToString() + "商品您已全部无异议收货，\n\r" + drinfo["发货金额"].ToString() + "元的货款将全额转付至" + drinfo["卖家名称"].ToString() + "账户。是否确认？";
                            return dsreturn;
                        default:
                            break;
                    }

                }

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有获得任何T" + drs["Number"].ToString() + "提货单的信息！";
            }
            return dsreturn;
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无异议收货时程序遇到异常！";
            return dsreturn;
        }

    }
    //无意义收货时发送的提醒
    private DataSet TX(string type,DataRow drinfo, DataRow drs, double sellyue, double jsfwufei, DateTime drnow, string buyjjrsy, string buyjjrsell, int zbNum, int thbc, int ythNum)
    {

        DataSet ds = new DataSet();

        DataTable dtmess =new TBD().dtmeww().Clone();
        
        DataRow htJYPT = dtmess.NewRow();
        htJYPT["type"] = "集合经销平台";
        htJYPT["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString().Trim();     
        htJYPT["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
        htJYPT["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
        htJYPT["提醒对象角色类型"] = "卖家";
        switch (type)
        {
            case "无异议收货":
                htJYPT["提醒内容文本"] = "您F" + drs["Number"].ToString().Trim() + "发货单，买方已无异议收货，" + drinfo["发货金额"].ToString() + "元的货款已转付至您的交易账户，" + jsfwufei.ToString("#0.00") + "元的技术服务费已同时扣收，敬请知悉。";
                break;
            case "补发货物无异议收货":
                htJYPT["提醒内容文本"] = "您F" + drs["Number"].ToString().Trim() + "发货单，买方已全部无异议收货（含所有补货），" + drinfo["发货金额"].ToString() + "元的货款已转付至您的交易账户， " + jsfwufei.ToString("#0.00") + "元的技术服务费已同时扣收，敬请知悉。";
                break;
            case "有异议收货后无异议收货":
                htJYPT["提醒内容文本"] = "您F" + drs["Number"].ToString().Trim() + "发货单，买方已改为全部无异议收货，" + drinfo["发货金额"].ToString() + "元的货款已转付至您的交易账户，" + jsfwufei.ToString("#0.00") + "元的技术服务费已同时扣收，敬请知悉。 ";
                break;
            default:
                break;
        }
        
        htJYPT["创建人"] = drs["登陆邮箱"].ToString().Trim();
        dtmess.Rows.Add(htJYPT);


        DataRow htjhjx = dtmess.NewRow();
        htjhjx["type"] = "集合经销平台";
        htjhjx["提醒对象登陆邮箱"] = drinfo["买家关联经纪人邮箱"].ToString();
        htjhjx["提醒对象结算账户类型"] = drinfo["买家关联经纪人结算账户类型"].ToString();
        htjhjx["提醒对象角色编号"] = drinfo["买家关联经纪人角色编号"].ToString();
        htjhjx["提醒对象角色类型"] = "经纪人";
        htjhjx["提醒内容文本"] = "尊敬的" + drinfo["买家关联经纪人名称"].ToString() + "，您的交易账户于" + drnow.Year.ToString() + "年" + drnow.Month.ToString() + "月" + drnow.Day.ToString() + "日" + drnow.Hour.ToString() + "时" + drnow.Minute.ToString() + "分新增经纪人收益" + buyjjrsy + "元，请继续努力！";
        htjhjx["创建人"] = drinfo["买家关联经纪人邮箱"].ToString();
        dtmess.Rows.Add(htjhjx);


        htjhjx = dtmess.NewRow(); 
        htjhjx["type"] = "集合经销平台";
        htjhjx["提醒对象登陆邮箱"] = drinfo["卖家关联经纪人邮箱"].ToString();
        htjhjx["提醒对象结算账户类型"] = drinfo["卖家关联经纪人结算账户类型"].ToString();
        htjhjx["提醒对象角色编号"] = drinfo["卖家关联经纪人角色编号"].ToString();
        htjhjx["提醒对象角色类型"] = "经纪人";
        htjhjx["提醒内容文本"] = "尊敬的" + drinfo["卖家关联经纪人名称"].ToString() + "，您的交易账户于" + drnow.Year.ToString() + "年" + drnow.Month.ToString() + "月" + drnow.Day.ToString() + "日" + drnow.Hour.ToString() + "时" + drnow.Minute.ToString() + "分新增经纪人收益" + buyjjrsell + "元，请继续努力！";
        htjhjx["创建人"] = drinfo["买家关联经纪人邮箱"].ToString();
        dtmess.Rows.Add(htjhjx);

        if (zbNum == thbc + ythNum)
        {
            #region //自动清盘向卖家、买家发送提醒
            //集合经销平台                    
            htjhjx = dtmess.NewRow(); 
            htjhjx["type"] = "集合经销平台";
            htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
            htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
            htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
            htjhjx["提醒对象角色类型"] = "卖家";
            htjhjx["提醒内容文本"] = "尊敬的" + drinfo["卖家名称"].ToString() + "，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
            htjhjx["创建人"] = drs["登陆邮箱"].ToString().Trim();
            dtmess.Rows.Add(htjhjx);

            htjhjx = dtmess.NewRow(); 
            htjhjx["type"] = "集合经销平台";
            htjhjx["提醒对象登陆邮箱"] = drs["登陆邮箱"].ToString().Trim();
            htjhjx["提醒对象结算账户类型"] = drinfo["买家结算账户类型"].ToString();
            htjhjx["提醒对象角色编号"] = drinfo["买家角色编号"].ToString();
            htjhjx["提醒对象角色类型"] = "买家";
            htjhjx["提醒内容文本"] = "尊敬的" + drinfo["买家名称"].ToString() + "，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
            htjhjx["创建人"] = drs["登陆邮箱"].ToString().Trim();
            dtmess.Rows.Add(htjhjx);

            #endregion
        }

        dtmess.AcceptChanges();

        ds.Tables.Add(dtmess);

        return ds;




    }

    private DataTable dtmeww()
    {
        DataTable dtmess = new DataTable();
        DataColumn dc = null; //new DataColumn("提醒对象登陆邮箱");

        dc = new DataColumn("type");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("提醒对象登陆邮箱");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);


        dc = new DataColumn("提醒对象结算账户类型");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("提醒对象角色编号");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("提醒对象角色类型");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("提醒内容文本");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("创建人");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("模提醒模块名");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);

        dc = new DataColumn("查看地址");
        dc.DataType = typeof(string);
        dtmess.Columns.Add(dc);
        return dtmess;
    }

    /// <summary>
    /// 请重新发货 wyh 2014.06.11 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsretun结构的Dataset</returns>
    public DataSet qcxsh_strs(DataTable ds)
    {

        DataSet dsreturn = new TBD().initReturnDataSet().Copy();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        try
        {
            if (ds == null || ds.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递参数不可为空!";
                return dsreturn;
            }


            DataRow drs = ds.Rows[0];
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            DataSet dsparr = new DataSet();
            Hashtable input = new Hashtable();
            Hashtable htreturn = new Hashtable();
            
            string action = drs["ispass"].ToString().Trim();
            
            DateTime dt = DateTime.Now;

            //获得当前提货单据详细信息
            input["@Number"] = drs["Number"].ToString().Trim();
            input["@jsbh"] = drs["买家角色编号"].ToString();
            string Strgetinfo = "select a.Number,('T'+a.Number) as  提货单号,('F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖家名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买家角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.T_YSTBDMJJSBH as 卖家角色编号,b.T_YSTBDDLYX as 卖家登陆邮箱,b.T_YSTBDJSZHLX as 卖家角色账户类型,b.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,b.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,b.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,b.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,b.Z_ZBSL as 定标数量,b.Z_LYBZJJE as 履约保证金金额,b.Z_HTZT as 合同状态  from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX  where a.Number=@Number and  a.F_DQZT ='已录入发货信息' and b.Y_YSYDDMJJSBH=@jsbh and b.Z_HTZT='定标'";

            DataSet dsgetinfo = (DataSet)(I_DBL.RunParam_SQL(Strgetinfo,"info", input)["return_ds"]);//DbHelperSQL.Query(Strgetinfo);
            if (dsgetinfo != null && dsgetinfo.Tables["info"].Rows.Count == 1)
            {
                DataRow drinfo = dsgetinfo.Tables["info"].Rows[0];
                if (drs["验收照片路径"].ToString().Trim().Equals(""))
                {   
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您还没有上传验收时照片！";
                    return dsreturn;
                }
                if (drs["情况说明"].ToString().Trim().Equals(""))
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请录入重新发货的情况说明！";
                    return dsreturn;
                }

                if (action.Equals("ok"))
                {
                    Hashtable htall = new Hashtable();
                    htall["@F_BUYQZXFHYSZP"] = drs["验收照片路径"].ToString().Trim();
                    htall["@F_BUYQZXFHQKSM"] = drs["情况说明"].ToString().Trim();
                    htall["@F_BUYQZXFHCZSJ"] = dt;
                    htall["@Number"] = drs["Number"].ToString().Trim();
                    //请重新发货操作
                    string StrSql = "update AAA_THDYFHDXXB set F_DQZT='请重新发货',F_BUYQZXFHYSZP=@F_BUYQZXFHYSZP,F_BUYQZXFHQKSM=@F_BUYQZXFHQKSM,F_BUYQZXFHCZSJ=@F_BUYQZXFHCZSJ   where Number = @Number";

                    htreturn = I_DBL.RunParam_SQL(StrSql,htall);
                    if ((bool)htreturn["return_float"])
                    {
                        if ((int)htreturn["return_other"] <= 0)
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_other"].ToString() + "条数据被更新。";
                            return dsreturn;
                        }
                    }
                    else
                    {    
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_errmsg"].ToString();
                        return dsreturn;

                    }

                        #region 向卖家跟平台管理人员发送提醒
                        //集合经销平台
                       DataTable dtmess = new TBD().dtmeww().Clone();
                       DataRow htjhjx = dtmess.NewRow();
                        htjhjx["type"] = "集合经销平台";
                        htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
                        
                        htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
                        htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
                        htjhjx["提醒对象角色类型"] = "卖家";
                        htjhjx["提醒内容文本"] = "由于买方对" + drinfo["发货单号"].ToString() + "发货单要求重新发货，请尽快处理!";
                        htjhjx["创建人"] = drs["登陆邮箱"].ToString();
                        dtmess.Rows.Add(htjhjx);

                        //操作平台 
                        DataRow htyept = dtmess.NewRow();
                        htyept["type"] = "业务平台";
                        htyept["模提醒模块名"] = "货物签收重新发货";
                        htyept["查看地址"] = "";
                        htyept["提醒内容文本"] = "由于买方对" + drinfo["发货单号"].ToString() + "发货单要求重新发货，请尽快处理!";
                        dtmess.Rows.Add(htyept);
                        dtmess.AcceptChanges();
                        DataSet dsmess = new DataSet();
                        dsmess.Tables.Add(dtmess);

                        //调用系统控制中心发送提醒---------------------- wyh 2014.06.09 以后可能会做成一步，不必判断对错。

                       object[] re = IPC.Call("发送提醒", new object[] { dsmess });
                        #endregion

                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请重新发货操作成功！ ";
                        return dsreturn;
                   
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "pass";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您确认“请卖方重新发货”后，系统\n\r将会通知卖方就该申请进行确认。";
                    return dsreturn;
                }


            }
            else
            {  
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有查找到任何单号为" + drs["Number"].ToString() + "的，状态\n\r为“已录入发货信息”的，对应中标定标单状态为“定标”的提货单！";
                return dsreturn;

            }


            //return Htwyy;
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请重新发货时程序出现异常。";
            return dsreturn;
        }
    }

    /// <summary>
    /// 部分收货 wyh 2014.06.11 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsreturn结构的dataset</returns>
    public DataSet bufensh_strs(DataTable ds)
    {
        DataSet dsreturn = new TBD().initReturnDataSet().Copy();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        try
        {
            if (ds == null || ds.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递参数不可为空!";
                return dsreturn;
            }

            DataRow drs = ds.Rows[0];
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            DataSet dsparr = new DataSet();
            Hashtable input = new Hashtable();
            Hashtable htreturn = new Hashtable();


            string action = drs["ispass"].ToString().Trim();
           
            DateTime dt = DateTime.Now;
          
            input["@Number"] = drs["Number"].ToString().Trim();
            input["@Y_YSYDDMJJSBH"] = drs["买家角色编号"].ToString();

            //获得当前提货单据详细信息
            string Strgetinfo = "select a.Number,('T'+a.Number) as  提货单号,('F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖家名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买家角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.T_YSTBDMJJSBH as 卖家角色编号,b.T_YSTBDDLYX as 卖家登陆邮箱,b.T_YSTBDJSZHLX as 卖家角色账户类型,b.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,b.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,b.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,b.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,b.Z_ZBSL as 定标数量,b.Z_LYBZJJE as 履约保证金金额,b.Z_HTZT as 合同状态  from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX  where a.Number=@Number and  a.F_DQZT ='已录入发货信息' and b.Y_YSYDDMJJSBH=@Y_YSYDDMJJSBH and b.Z_HTZT='定标'";
           DataSet dsgetinfo = null;
           htreturn=  I_DBL.RunParam_SQL(Strgetinfo, "FHDinfo", input);//
           if ((bool)(htreturn["return_float"])) //说明执行完成
           {
               //这里就可以用了。
               dsgetinfo = (DataSet)(htreturn["return_ds"]);   
           }
           else
           {
               dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
               dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_errmsg"].ToString(); 
               return dsreturn;
           }

          //DbHelperSQL.Query(Strgetinfo);

           if (dsgetinfo != null && dsgetinfo.Tables["FHDinfo"].Rows.Count == 1)
            {
                DataRow drinfo = dsgetinfo.Tables["FHDinfo"].Rows[0];
                if (drs["验收照片路径"].ToString().Trim().Equals(""))
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您还没有上传验收时照片！";
                    return dsreturn;
                }

                if (drs["发货单（物流单）影印件"].ToString().Trim().Equals(""))
                {
                  
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您还没有上传签收纸质发货单（物流单）影印件！";
                    return dsreturn;
                }
                if (Convert.ToInt64(drs["部分签收数量"].ToString().Trim()) >= Convert.ToInt64(drinfo["提货数量"].ToString().Trim()))
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err1";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "部分收货数量不可大于或者等于提货数量！";
                    return dsreturn;
                }

                if (action.Equals("ok"))
                {
                    //请重新发货操作
                    Hashtable htall = new Hashtable();
                    htall["@F_BUYBFSHYSZP"] = drs["验收照片路径"].ToString().Trim();
                    htall["@F_BUYBFSHFHDWLDYYJ"] = drs["发货单（物流单）影印件"].ToString().Trim();
                    htall["@F_BUYBFSHCZSJ"] = dt;
                    htall["@F_BUYBFSHSJQSSL"] = drs["部分签收数量"].ToString();
                    htall["@Number"] = drs["Number"].ToString().Trim();
                    string StrSql = "update AAA_THDYFHDXXB set F_DQZT='部分收货',F_BUYBFSHYSZP=@F_BUYBFSHYSZP,F_BUYBFSHFHDWLDYYJ=@F_BUYBFSHFHDWLDYYJ,F_BUYBFSHCZSJ= @F_BUYBFSHCZSJ, F_BUYBFSHSJQSSL=@F_BUYBFSHSJQSSL   where Number = @Number ";

                    htreturn = I_DBL.RunParam_SQL(StrSql, htall);//
                    if ((bool)(htreturn["return_float"])) //说明执行完成
                    {
                        //这里就可以用了。
                        if ((int)htreturn["return_other"] <= 0)
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_other"].ToString() + "数据被修改！";
                            return dsreturn;
                        }
                        
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_errmsg"].ToString();
                        return dsreturn;

                    }
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "部分收货操作成功！ ";
                   

                    #region 向卖家跟平台管理人员发送提醒
                        //集合经销平台
                        DataTable dtmess = new TBD().dtmeww().Clone();
                        DataRow htjhjx = dtmess.NewRow();
                        htjhjx["type"] = "集合经销平台";
                        htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
                      
                        htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
                        htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
                        htjhjx["提醒对象角色类型"] = "卖家";
                        htjhjx["提醒内容文本"] = drinfo["发货单号"].ToString() + "发货单买方已做“部分收货”处理，请您尽快安排补发货。";
                        htjhjx["创建人"] = drs["登陆邮箱"].ToString();
                        dtmess.Rows.Add(htjhjx);

                        //操作平台 
                        DataRow htyept = dtmess.NewRow();
                        htyept["type"] = "业务平台";
                        htyept["模提醒模块名"] = "部分收货";
                        htyept["查看地址"] = "";
                        htyept["提醒内容文本"] = drinfo["发货单号"].ToString() + "发货单买方已做“部分收货”处理，请您尽快安排补发货。";
                        dtmess.Rows.Add(htyept);
                        dtmess.AcceptChanges();
                        DataSet dsmess = new DataSet();
                        dsmess.Tables.Add(dtmess);

                    //调用系统控制中心发送提醒---------------------- wyh 2014.06.09 以后可能会做成一步，不必判断对错。

                   object[]  re = IPC.Call("发送提醒", new object[] { dsmess });
                   return dsreturn;
                        #endregion  
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "pass";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您确认“部分收货”后，系统将会通知卖方就收货差额部分给予补发。";
                    return dsreturn;
                }


            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有查找到任何单号为" + drs["Number"].ToString() + "的，状态\n\r为“已录入发货信息”的，对应中标定标单状态为“定标”的提货单！";
                return dsreturn;
            }

           return dsreturn;
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "部分发货时程序出现异常！";
            return dsreturn;
        }
    }

    /// <summary>
    /// 有异议收货  wyh 2014.06.11
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsretrn结构的Dataset</returns>
    public DataSet wyyth_strs(DataTable ds)
    {
        DataSet dsreturn = new TBD().initReturnDataSet().Copy();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable input = new Hashtable();
            Hashtable htreturn = new Hashtable();

            DataRow drs = ds.Rows[0];

            if (ds == null || ds.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递参数不可为空!";
                return dsreturn;
            }

            string action = drs["ispass"].ToString().Trim();
         
            DateTime dt = DateTime.Now;
           
            input["@Number"] = drs["Number"].ToString().Trim();
            input["@Y_YSYDDMJJSBH"] = drs["买家角色编号"].ToString();
            //获得当前提货单据详细信息
            string Strgetinfo = "select a.Number,('T'+a.Number) as  提货单号,('F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖家名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买家角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.T_YSTBDMJJSBH as 卖家角色编号,b.T_YSTBDDLYX as 卖家登陆邮箱,b.T_YSTBDJSZHLX as 卖家角色账户类型,b.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,b.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,b.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,b.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,b.Z_ZBSL as 定标数量,b.Z_LYBZJJE as 履约保证金金额,b.Z_HTZT as 合同状态  from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX  where a.Number=@Number and  a.F_DQZT ='已录入发货信息' and b.Y_YSYDDMJJSBH=@Y_YSYDDMJJSBH and b.Z_HTZT='定标'";

            DataSet dsgetinfo = null;
            htreturn = I_DBL.RunParam_SQL(Strgetinfo,"info",input);
            if ((bool)(htreturn["return_float"])) //说明执行完成
            {
                //这里就可以用了。
                dsgetinfo = (DataSet)(htreturn["return_ds"]);   

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_errmsg"].ToString();
                return dsreturn;
            }
 
            if (dsgetinfo != null && dsgetinfo.Tables["info"].Rows.Count == 1)
            {
                DataRow drinfo = dsgetinfo.Tables["info"].Rows[0];
                if (drs["验收照片路径"].ToString().Trim().Equals(""))
                { 
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您还没有上传验收时照片！";
                    return dsreturn;
                }

                if (drs["发货单（物流单）影印件"].ToString().Trim().Equals(""))
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您还没有上传签收纸质发货单（物流单）影印件！";
                    return dsreturn;
                }

                if (drs["情况说明"].ToString().Trim().Equals(""))
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "有异议收货情况说明不能为空！";
                    return dsreturn;
                }

                if (action.Equals("ok"))
                {
                    //请重新发货操作
                    Hashtable htall = new Hashtable();
                    htall["@F_BUYYYYYSZP"] = drs["验收照片路径"].ToString().Trim();
                    htall["@F_BUYYYYSHFHDWLDYYJ"] = drs["发货单（物流单）影印件"].ToString().Trim();
                    htall["@F_BUYYYYSHCZSJ"] = dt;
                    htall["@F_BUYYYYQKSM"] = drs["情况说明"].ToString();
                    htall["@Number"] = drs["Number"].ToString().Trim();

                    string StrSql = "update AAA_THDYFHDXXB set F_DQZT='有异议收货',F_BUYYYYYSZP=@F_BUYYYYYSZP,F_BUYYYYSHFHDWLDYYJ=@F_BUYYYYSHFHDWLDYYJ,F_BUYYYYSHCZSJ= @F_BUYYYYSHCZSJ, F_BUYYYYQKSM=@F_BUYYYYQKSM   where Number = @Number ";

                    htreturn = I_DBL.RunParam_SQL(StrSql, htall);//
                    if ((bool)(htreturn["return_float"])) //说明执行完成
                    {
                        //这里就可以用了。
                        if ((int)htreturn["return_other"] <= 0)
                        {                   
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_other"].ToString() + "数据被修改！";
                            return dsreturn;
                        }

                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_errmsg"].ToString();
                        return dsreturn;
                    }
                  
                        #region 向卖家跟平台管理人员发送提醒
                        //集合经销平台
                        DataTable dtmess = new TBD().dtmeww().Clone();
                        DataRow htjhjx = dtmess.NewRow();
                        htjhjx["type"] = "集合经销平台";
                        htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
                       
                        htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
                        htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
                        htjhjx["提醒对象角色类型"] = "卖家";
                        htjhjx["提醒内容文本"] = "您的" + drinfo["发货单号"].ToString() + "发货单，买方已做“有异议收货”处理，已进入争议处理程序，请尽快与相应买方联系!";
                        htjhjx["创建人"] = drs["登陆邮箱"].ToString();
                        dtmess.Rows.Add(htjhjx);

                        //操作平台 
                        DataRow htyept = dtmess.NewRow();
                        htyept["type"] = "业务平台";
                        htyept["模提醒模块名"] = "有异议收货";
                        htyept["查看地址"] = "";
                        htyept["提醒内容文本"] = "您的" + drinfo["发货单号"].ToString() + "发货单，买方已做“有异议收货”处理，已进入争议处理程序，请尽快与相应买方联系!";
                        dtmess.Rows.Add(htyept);
                        dtmess.AcceptChanges();
                        DataSet dsmess = new DataSet();
                        dsmess.Tables.Add(dtmess);

                        //调用系统控制中心发送提醒---------------------- wyh 2014.06.09 以后可能会做成一步，不必判断对错。

                       object[] re = IPC.Call("发送提醒", new object[] { dsmess });
                        #endregion

                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = " 有异议收货操作成功！ ";
                        return dsreturn;
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "pass";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "根据您的指令，" + drinfo["发货单号"].ToString() + "发货单将进入争议处理程序。";
                    return dsreturn;
                }


            }
            else
            {         
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有查找到任何单号为" + drs["Number"].ToString() + "的，状态为“已录入发货信息”的\n\r，对应中标定标单状态为“定标”的提货单！";
                return dsreturn;
            }

        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "有异议收货时时程序出现异常！";
            return dsreturn;
        }
    }

    //提请买家签收
    /// <summary>
    /// 提请买家签收 wyh 2014.06.11 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsreturn结构的Dataset</returns>
    public DataSet InsertSQL(DataSet ds)
    {
        DataSet returnDS = new TBD().initReturnDataSet().Copy();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        if (ds == null || ds.Tables[0].Rows.Count <= 0)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空!";
            return returnDS;
        }

        try
        {
                string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();
                string JSZHLX = ds.Tables[0].Rows[0]["结算账户类型"].ToString();
                string FHD = ds.Tables[0].Rows[0]["发货单号"].ToString();
                string WLGSMC = ds.Tables[0].Rows[0]["物流公司名称"].ToString();
                string WLDH = ds.Tables[0].Rows[0]["物流单号"].ToString();
                string WLQSD = ds.Tables[0].Rows[0]["物流签收单"].ToString();
                string TXDXDLYX = "";//提醒对象登陆邮箱
                string TXDXYHM = "";//提醒对象用户名
                string TXDXJSZHLX = "";//提醒对象结算账户类型
                string TXDXJSBH = "";//提醒对象角色编号
                string TXDXJSLX = "买家";//提醒对象角色类型
                string TXNR = "";//提醒内容文本
                string CreaterUser = "";//创建人
                string time = DateTime.Now.ToString();

                if (JSZHLX == "经纪人交易账户")
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "抱歉！只有卖方才能执行此类指令。";
                    return returnDS;
                }

                #region//基础验证
               
                //------拆分到外层已经
                #endregion

                #region//验证投标资料审核表对应的投标点是否审核通过
                //string strTBDZLSH = "select c.* from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH = b.Number left join AAA_TBZLSHB c on b.T_YSTBDBH = c.TBDH where a.Number='" + FHD.Trim() + "'";
                DataSet dsZLSHB = new lvyueclass().GTTBZLSHinfo(FHD.Trim());//new DataSet(); //DbHelperSQL.Query(strTBDZLSH);-----------------已经拆分
                if (dsZLSHB != null && dsZLSHB.Tables[0].Rows.Count > 0)
                {
                    if (dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "未审核" || dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                        return returnDS;
                    }
                }
               
                #endregion

                #region//验证合同状态
                //string strSQLDB = "select d.T_YSTBDMJJSBH 原始投标单卖家角色编号,d.Z_HTZT 合同状态,t.F_DQZT 发货单状态,d.Y_YSYDDDLYX 提醒对象登陆邮箱,'提醒对象用户名'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_BUYJSBH=d.Y_YSYDDMJJSBH),'提醒对象结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB DL where DL.J_BUYJSBH=d.Y_YSYDDMJJSBH),d.Y_YSYDDMJJSBH '提醒对象角色编号','卖家用户名'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_SELJSBH=d.T_YSTBDMJJSBH),d.T_YSTBDDLYX 原始投标单登录邮箱 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where t.Number='" + FHD.Trim() + "'";
                DataSet zt = new lvyueclass().GetTHDXXAndHTZT(FHD.Trim());//new DataSet();//DbHelperSQL.Query(strSQLDB);已经拆分----------------------
                if (zt != null && zt.Tables[0].Rows.Count > 0)
                {
                    #region//对发货单状态的验证
             
                    if (zt.Tables[0].Rows[0]["原始投标单卖家角色编号"].ToString() != JSBH)
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "抱歉，只有卖方才能执行此操作！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["发货单状态"].ToString() == "未生成发货单" || zt.Tables[0].Rows[0]["发货单状态"].ToString() == "已生成发货单")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "抱歉，录入发货信息后才能执行此操作！";
                        return returnDS;
                    }

                    if (zt.Tables[0].Rows[0]["发货单状态"].ToString() == "部分收货" )
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "买方已部分收货，请先录入补发备注！";
                        return returnDS;
                    }

                    if (zt.Tables[0].Rows[0]["发货单状态"].ToString() == "无异议收货" || zt.Tables[0].Rows[0]["发货单状态"].ToString() == "默认无异议收货" || zt.Tables[0].Rows[0]["发货单状态"].ToString() == "有异议收货后无异议收货" || zt.Tables[0].Rows[0]["发货单状态"].ToString() == "补发货物无异议收货" || zt.Tables[0].Rows[0]["发货单状态"].ToString() == "有异议收货" || zt.Tables[0].Rows[0]["发货单状态"].ToString() == "卖家主动退货" || zt.Tables[0].Rows[0]["发货单状态"].ToString() == "请重新发货" || zt.Tables[0].Rows[0]["发货单状态"].ToString() == "撤销")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单已签收，不能执行此操作！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["合同状态"].ToString() == "定标合同到期")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单对应的合同已到期！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["合同状态"].ToString() == "定标合同终止")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单对应的合同已终止！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["合同状态"].ToString() == "定标执行完成")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单对应的合同已完成！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["合同状态"].ToString() == "中标")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单对应的合同尚未定标，不能执行此操作！";
                        return returnDS;
                    }
                    if (zt.Tables[0].Rows[0]["合同状态"].ToString() == "未定标废标")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "此发货单对应的合同已终止！";
                        return returnDS;
                    }
                    #endregion
                    TXDXDLYX = zt.Tables[0].Rows[0]["提醒对象登陆邮箱"].ToString();//提醒对象登陆邮箱
                    TXDXYHM = zt.Tables[0].Rows[0]["提醒对象用户名"].ToString();//提醒对象用户名
                    TXDXJSZHLX = zt.Tables[0].Rows[0]["提醒对象结算账户类型"].ToString();//提醒对象结算账户类型
                    TXDXJSBH = zt.Tables[0].Rows[0]["提醒对象角色编号"].ToString();//提醒对象角色编号
                    TXNR = zt.Tables[0].Rows[0]["卖家用户名"].ToString() + "卖方就 F" + FHD + " 发货单向您发出“提请买方签收”的通知，自通知发出之日起三日后系统将默认您“无异议收货”，全部货款即自动支付。您如有异议，请即致电平台服务人员！";//提醒内容文本
                    CreaterUser = zt.Tables[0].Rows[0]["原始投标单登录邮箱"].ToString();//创建人
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "请输入正确的发货单编号！";
                    return returnDS;
                }
                #endregion

                #region//执行SQL

                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");  
                Hashtable input = new Hashtable();
                Hashtable htreturn = new Hashtable();

                input["@F_QMJQSQRCZSJ"] = time;
                input["@F_QMJQSWLGSMC"] = WLGSMC;
                input["@F_QMJQSWLDH"] = WLDH;
                input["@F_QMJQSWLQSD"] = WLQSD;
                input["@Number"] = FHD;

                string strUpdateTHD = "update AAA_THDYFHDXXB set F_QMJQSQRCZSJ=@F_QMJQSQRCZSJ,F_QMJQSWLGSMC=@F_QMJQSWLGSMC,F_QMJQSWLDH=@F_QMJQSWLDH,F_QMJQSWLQSD=@F_QMJQSWLQSD where Number=@Number ";
              
                int i = 0;//DbHelperSQL.ExecuteSql(strUpdateTHD);
                htreturn = I_DBL.RunParam_SQL(strUpdateTHD, input);
                if ((bool)(htreturn["return_float"])) //说明执行完成
                {
                    //这里就可以用了。
                    if ((int)(htreturn["return_other"]) <= 0)
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "提请信息添加失败！";
                        return returnDS;
                    }
                    i = (int)(htreturn["return_other"]);

                }

                #region  发送提醒
                    DataTable dtmess = new TBD().dtmeww().Clone();
                    DataRow htJYPT = dtmess.NewRow();
                    htJYPT["type"] = "集合经销平台";
                    htJYPT["提醒对象登陆邮箱"] = TXDXDLYX;
                  
                    htJYPT["提醒对象结算账户类型"] = TXDXJSZHLX;
                    htJYPT["提醒对象角色编号"] = TXDXJSBH;
                    htJYPT["提醒对象角色类型"] = TXDXJSLX;
                    htJYPT["提醒内容文本"] = TXNR;
                    htJYPT["创建人"] = CreaterUser;
                    dtmess.Rows.Add(htJYPT);

                    DataRow htYWPT = dtmess.NewRow();//业务操作平台
                    htYWPT["type"] = "业务平台";
                    htYWPT["模提醒模块名"] = "提请买家签收";
                    htYWPT["提醒内容文本"] = TXNR;
                    htYWPT["查看地址"] = "abcdf";
                    dtmess.Rows.Add(htYWPT);
                    dtmess.AcceptChanges();

                    DataSet dsmess = new DataSet();
                    dsmess.Tables.Add(dtmess);
                    //调用系统控制中心发送提醒---------------------- wyh 2014.06.09 以后可能会做成一步，不必判断对错。

                    object[] re = IPC.Call("发送提醒", new object[] { dsmess });

                #endregion
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "提请信息添加成功！";
                    return returnDS;
                
                #endregion

           
        }
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";
        }
        return returnDS;
    }

    /// <summary>
    /// 提请买家签收时，输入发货单编号后获取的信息--zhouli add 2019.09.04
    /// </summary>
    /// <param name="number">提货单与发货单表的Number</param>
    /// <returns></returns>
    public DataSet IsHavedQS(string number)
    {
        DataSet returnDS = new TBD().initReturnDataSet().Copy();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });

        if (string.IsNullOrEmpty(number))
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不可为空!";
            return returnDS;
        }
        try {
            Hashtable ht = new Hashtable();
            ht["@number"] = number;
            string strSQL = "select F_QMJQSQRCZSJ '请买家签收确认操作时间',F_WLGSMC '物流公司名称',F_WLDH '物流单号',F_QMJQSWLQSD '物流签收单' from AAA_THDYFHDXXB where Number=@number";
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable returnHT =  I_DBL.RunParam_SQL(strSQL, "", ht);
            if ((bool)(returnHT["return_float"])) //说明执行完成
            {
                if (returnHT["return_ds"] != null && ((DataSet)(returnHT["return_ds"])).Tables.Count > 0 && ((DataSet)(returnHT["return_ds"])).Tables[0].Rows.Count>0)
                {
                    DataTable tab = ((DataSet)(returnHT["return_ds"])).Tables[0].Copy();
                    tab.TableName = "主表";
                    returnDS.Tables.Add(tab);
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
                }
                  
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = returnHT["return_errmsg"].ToString();
                return returnDS;
            }
        }
        catch(Exception ex)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
        }
        return returnDS;
    }
}