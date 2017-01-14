/***********************************************************************************************
 * 
 *创建人：wyh
 *
 *创建时间：2014.05.17
 *
 *代码功能：实现投标单各项功能
 *
 *参考文档：《业务分析》交易指令中心部分
 *
 ********************************************************************************************************/
using FMDBHelperClass;
using FMipcClass;
using System;
using System.Collections;
using System.Data;

/// <summary>
/// TBD 的摘要说明
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
    /// 发布投标单 wyh 2014.05.20
    /// </summary>
    /// <param name="ds">参数</param>
    /// <returns>成功：插入成功；返回字符串不是成功：失败原因。 ；null :程序出现异常</returns>
    public string SetTBD(DataSet ds)
    {
        #region 投标单主体

       
        string strreSult = "后台未进行任何操作！";
        if (ds == null || ds.Tables[0].Rows.Count <= 0)
        {
            strreSult = "传递参数不可为空！";
            return strreSult;
        }

        DataTable dt = ds.Tables["TBD"];
        bool valFlag = false;
        string info = string.Empty;
        dt = InitValidater.ValTBD(dt, ref valFlag, ref info);
        if (!valFlag)
        {
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD" + strreSult, null);
            strreSult = info;
            return strreSult;
        }

        //?wyh 2014.05.21 add
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();


        //验证商品是否已经投标
        input["@DLYX"] = dt.Rows[0]["DLYX"].ToString();//htUserVal["登录邮箱"].ToString(); //跟sql语句中对应的参数。
        input["@HTQX"] = dt.Rows[0]["HTQX"].ToString();
        input["@SPBH"] = dt.Rows[0]["SPBH"].ToString();
        Hashtable return_ht = I_DBL.RunParam_SQL("SELECT Number FROM AAA_TBD WHERE DLYX=@DLYX AND HTQX=@HTQX AND ZT='竞标' AND SPBH=@SPBH ", "TBD", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["TBD"].Rows.Count > 0)
            {
                strreSult = "该商品您已有投标，您可以撤销或修改原投标单！";
                FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, strreSult, null);
                return strreSult;
            }

        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
            strreSult = return_ht["return_errmsg"].ToString();
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, strreSult, null);
            return strreSult;
        } //?



        Hashtable htya =  new  SetYDD_hqcs().Get_TBDcs(dt.Rows[0]["MJJSBH"].ToString(), dt.Rows[0]["DLYX"].ToString());

        if (htya == null || htya.Count != 4)
        {
            string csAll = "";
            foreach (System.Collections.DictionaryEntry de in htya)
            {
                csAll += de.Key.ToString() + "-";//得到key
            }
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD投标单--获取所有参数个数不对：-" + csAll, null);
            return null;
        }



        //object[] re = IPC.Call("用户基本信息", new object[] { dt.Rows[0]["MJJSBH"].ToString()});
        object[] re = htya["用户基本信息"] as object[];
        DataSet dsUserVal =null;
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。

             dsUserVal = (DataSet)(re[1]);
             if (dsUserVal == null || dsUserVal.Tables[0].Rows.Count <=0)
             {
                 strreSult = "查找用户信息出错";
                 FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD投标单" + strreSult, null);
                 return strreSult;
             }
           
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            strreSult = re[1].ToString();
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD投标单" + strreSult, null);
            return strreSult;
             
        }
        //================


        
        DataRow htUserVal = dsUserVal.Tables[0].Rows[0];
        
        string htqx_new = dt.Rows[0]["HTQX"].ToString(); //2013.0.26 wyh添加即时类型

        DataSet dsjjrinfo = null;
        //re = IPC.Call("关联经纪人信息", new object[] { dt.Rows[0]["DLYX"].ToString() });//-----------
        re = htya["关联经纪人信息"] as object[];
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            dsjjrinfo = (DataSet)(re[1]);     
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            strreSult = re[1].ToString();
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD投标单" + strreSult, null);
            return strreSult;

        }
       
        if (dsjjrinfo != null && dsjjrinfo.Tables[0].Rows.Count > 0)
        {
            DataRow JJRInfo = dsjjrinfo.Tables[0].Rows[0];
            dt.Rows[0]["GLJJRYX"] = JJRInfo["关联经纪人登陆账号"];
            dt.Rows[0]["GLJJRYHM"] = JJRInfo["关联经纪人用户名"];
            dt.Rows[0]["GLJJRJSBH"] = JJRInfo["关联经纪人编号"];
            dt.Rows[0]["GLJJRPTGLJG"] = JJRInfo["关联经纪人平台管理机构"];
        }
        else
        {
            strreSult = "没有获取到相关经纪人信息！";
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD投标单" + strreSult, null);
            return strreSult;
        }

      
        #endregion
        string Time = DateTime.Now.ToString();//本单统一的时间
        #region 投标单SQL

        Hashtable ht_ZZ = new Hashtable(); //dt.Rows[0]["其他资质"] == null ? null : (Hashtable)dt.Rows[0]["其他资质"];

        I_Dblink I_DBL_all = (new DBFactory()).DbLinkSqlMain("");     
        Hashtable inputall = new Hashtable();
        ArrayList al = new ArrayList();


        string Number_TBD = "";
        //re = IPC.Call("获取主键", new object[] { "AAA_TBD", "" });//--------------------
        re = htya["AAA_TBD获取主键"] as object[];
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (re[1] != null && !re[1].ToString().Trim().Equals(""))
            {
                Number_TBD = (string)(re[1]);
            }
            else
            {
                strreSult = "获取投标单主键出错！";
                FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD投标单" + strreSult, null);
                return strreSult;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            strreSult = re[1].ToString();
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD投标单" + strreSult, null);
            return strreSult;
        }
     
        inputall["@Number"] = Number_TBD;
        inputall["@DLYX"] = dt.Rows[0]["DLYX"].ToString();
        inputall["@JSZHLX"] = dt.Rows[0]["JSZHLX"].ToString();
        inputall["@MJJSBH"] = dt.Rows[0]["MJJSBH"].ToString();
        inputall["@GLJJRYX"] = dt.Rows[0]["GLJJRYX"].ToString();
        inputall["@GLJJRYHM"] = dt.Rows[0]["GLJJRYHM"].ToString();
        inputall["@GLJJRJSBH"] = dt.Rows[0]["GLJJRJSBH"].ToString();
        inputall["@GLJJRPTGLJG"] = dt.Rows[0]["GLJJRPTGLJG"].ToString();
        inputall["@SPBH"] = dt.Rows[0]["SPBH"].ToString();
        inputall["@SPMC"] = dt.Rows[0]["SPMC"].ToString();
        inputall["@GG"] = dt.Rows[0]["GG"].ToString();
        inputall["@JJDW"] = dt.Rows[0]["JJDW"].ToString();
        inputall["@PTSDZDJJPL"] = dt.Rows[0]["PTSDZDJJPL"].ToString();
        inputall["@HTQX"] = dt.Rows[0]["HTQX"].ToString();
        inputall["@MJSDJJPL"] = dt.Rows[0]["MJSDJJPL"].ToString();
        inputall["@GHQY"] = dt.Rows[0]["GHQY"].ToString();
        inputall["@TBNSL"] = dt.Rows[0]["TBNSL"].ToString();
        inputall["@TBJG"] = dt.Rows[0]["TBJG"].ToString();
        inputall["@TBJE"] = dt.Rows[0]["TBJE"].ToString();
        inputall["@MJTBBZJBL"] = dt.Rows[0]["MJTBBZJBL"].ToString();
        inputall["@MJTBBZJZXZ"] = dt.Rows[0]["MJTBBZJZXZ"].ToString();
        inputall["@DJTBBZJ"] = dt.Rows[0]["DJTBBZJ"].ToString();
        inputall["@ZT"] = "竞标";
        inputall["@TJSJ"] = Time;
        inputall["@ZLBZYZM"] = dt.Rows[0]["ZLBZYZM"].ToString();
        inputall["@CPJCBG"] = dt.Rows[0]["CPJCBG"].ToString();
        inputall["@PGZFZRFLCNS"] = dt.Rows[0]["PGZFZRFLCNS"].ToString();
        inputall["@FDDBRCNS"] = dt.Rows[0]["FDDBRCNS"].ToString();
        inputall["@SHFWGDYCN"] = dt.Rows[0]["SHFWGDYCN"].ToString();
        inputall["@CPSJSQS"] = dt.Rows[0]["CPSJSQS"].ToString();
        inputall["@CreateUser"] = dt.Rows[0]["DLYX"].ToString();
        inputall["@CreateTime"] = Time;
        inputall["@SLZM"] = dt.Rows[0]["SLZM"].ToString();
        inputall["@ZZ01"] = dt.Rows[0]["其他资质"].ToString();
        inputall["@ZZ02"] = ht_ZZ["资质2"] == null ? "" : ht_ZZ["资质2"].ToString();
        inputall["@ZZ03"] = ht_ZZ["资质3"] == null ? "" : ht_ZZ["资质3"].ToString();
        inputall["@ZZ04"] = ht_ZZ["资质4"] == null ? "" : ht_ZZ["资质4"].ToString();
        inputall["@ZZ05"] = ht_ZZ["资质5"] == null ? "" : ht_ZZ["资质5"].ToString();
        inputall["@ZZ06"] = ht_ZZ["资质6"] == null ? "" : ht_ZZ["资质6"].ToString();
        inputall["@ZZ07"] = ht_ZZ["资质7"] == null ? "" : ht_ZZ["资质7"].ToString();
        inputall["@ZZ08"] = ht_ZZ["资质8"] == null ? "" : ht_ZZ["资质8"].ToString();
        inputall["@ZZ09"] = ht_ZZ["资质9"] == null ? "" : ht_ZZ["资质9"].ToString();
        inputall["@ZZ10"] = ht_ZZ["资质10"] == null ? "" : ht_ZZ["资质10"].ToString();
        inputall["@YZBSL"] = 0;
        inputall["@SPCD"] = dt.Rows[0]["省市区"].ToString();
     
        

        string sql_TBD = "INSERT INTO AAA_TBD(Number,DLYX,JSZHLX,MJJSBH,GLJJRYX,GLJJRYHM,GLJJRJSBH,GLJJRPTGLJG,SPBH,SPMC,GG,JJDW,PTSDZDJJPL,HTQX,MJSDJJPL,GHQY,TBNSL,TBJG,TBJE,MJTBBZJBL,MJTBBZJZXZ,DJTBBZJ,ZT,TJSJ,ZLBZYZM,CPJCBG,PGZFZRFLCNS,FDDBRCNS,SHFWGDYCN,CPSJSQS,SLZM,ZZ01,ZZ02,ZZ03,ZZ04,ZZ05,ZZ06,ZZ07,ZZ08,ZZ09,ZZ10,CreateUser,CreateTime,YZBSL,SPCD) VALUES(@Number,@DLYX,@JSZHLX,@MJJSBH,@GLJJRYX,@GLJJRYHM,@GLJJRJSBH,@GLJJRPTGLJG,@SPBH,@SPMC,@GG,@JJDW,@PTSDZDJJPL,@HTQX,@MJSDJJPL,@GHQY,@TBNSL,@TBJG,@TBJE,@MJTBBZJBL,@MJTBBZJZXZ,@DJTBBZJ,@ZT,@TJSJ,@ZLBZYZM,@CPJCBG,@PGZFZRFLCNS,@FDDBRCNS,@SHFWGDYCN,@CPSJSQS,@SLZM,@ZZ01,@ZZ02,@ZZ03,@ZZ04,@ZZ05,@ZZ06,@ZZ07,@ZZ08,@ZZ09,@ZZ10,@CreateUser,@CreateTime,0,@SPCD)";

        al.Add(sql_TBD);
       
        #endregion
       // ht_Sql.Add(sql_TBD, sp_TBD); wyh 2014.05.21 delet
        #region 账款流水明细表
        DataTable dt_LS = GetMoneydbzDS("1304000003").Tables["DZB"]; //htya["DZB"] as DataTable; //GetMoneydbzDS("1304000003").Tables["DZB"]; 
        
        double DJ = Convert.ToDouble(dt.Rows[0]["DJTBBZJ"]);//要冻结的投标保证金-------------

        string Number_LS = "99999K";//

       // object[] rezkls = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });
        object[] rezkls = htya["AAA_ZKLSMXB获取主键"] as object[];
        if (rezkls[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (rezkls[1] != null && !rezkls[1].ToString().Trim().Equals(""))
            {
                Number_LS = (string)(rezkls[1]);
            }
            else
            {
                strreSult = "获取账款流水明细表主键出错！";
                FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD投标单" + strreSult, null);
                return strreSult;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            strreSult = rezkls[1].ToString();
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD投标单" + strreSult, null);
            return strreSult;
        }

        string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,SJLX,CreateUser,CreateTime) VALUES(@Numberzkls,@DLYXzkls,@JSZHLXzkls,@JSBH,@LYYWLX,@LYDH,@LSCSSJ,@YSLX,@JE,@XM,@XZ,@ZY,@JKBH,@SJLX,@CreateUserzkls,@CreateTimezkls)";

        inputall["@Numberzkls"] = Number_LS;
        inputall["@DLYXzkls"] = htUserVal["登录邮箱"].ToString();
        inputall["@JSZHLXzkls"] = htUserVal["结算账户类型"].ToString();
        inputall["@JSBH"] = htUserVal["卖家角色编号"].ToString();
        inputall["@LYYWLX"] = "AAA_TBD";
        inputall["@LYDH"] = Number_TBD;
        inputall["@LSCSSJ"] = Time;// Number_LS;
        inputall["@YSLX"] = dt_LS.Rows[0]["YSLX"].ToString().Trim();
        inputall["@JE"] = DJ;
        inputall["@XM"] = dt_LS.Rows[0]["XM"].ToString().Trim();
        inputall["@XZ"] = dt_LS.Rows[0]["XZ"].ToString().Trim();
        inputall["@ZY"] = dt_LS.Rows[0]["ZY"].ToString().Trim().Replace("[x1]", Number_TBD);
        inputall["@JKBH"] = "接口编号";
        inputall["@SJLX"] = dt_LS.Rows[0]["SJLX"].ToString().Trim();
        inputall["@CreateUserzkls"] = htUserVal["登录邮箱"].ToString();
        inputall["@CreateTimezkls"] = Time;

        #endregion

        al.Add(sql_LS);

        #region 更新用户余额
        string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE-@JE WHERE B_DLYX=@DLYXzkls";
        al.Add(sql_YE);
        #endregion
            Hashtable return_all=  I_DBL_all.RunParam_SQL(al, inputall);
            if ((bool)(return_all["return_float"])) //说明执行完成
            {
                if ((int)return_all["return_other"] > 0)
                {
                        strreSult = "成功";
                    if (!htqx_new.Equals("即时"))
                    {
                        DataTable dtJK = new DataTable("dtJK");
                        dtJK.Columns.Add("商品编号");
                        dtJK.Columns.Add("合同期限");
                        dtJK.Rows.Add(new string[] { dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString() });

                        //需调用成交处理中心方法---------------------------------

                        //需调用成交处理中心方法---------------------------------
                        Hashtable htcs = new Hashtable();
                        htcs["@HTQX"] = dt.Rows[0]["HTQX"].ToString();
                        htcs["@SPBH"] = dt.Rows[0]["SPBH"].ToString();
                        ArrayList aljk = LJQ.SFJRLJQ(dtJK);
                        if (aljk != null && aljk.Count > 0)
                        {
                            I_DBL_all.RunParam_SQL(aljk, htcs);
                        }

                    }

                    strreSult = "成功";
                 
                    return strreSult;
                }
                else
                {
                    strreSult = "数据受影响行数为0";
                    return strreSult;
                }
            }
            else
            {
                //读取数据库过程中发生程序错误了。。。
                strreSult = return_all["return_errmsg"].ToString();
               // FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "投标单--：" + strreSult, null);
                FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "TBD投标单" + strreSult, null);
                return strreSult;
            } //?
  
      
    }

    /// <summary>
    /// 发布投标单草稿 wyh 2014.05.22 
    /// </summary>
    /// <param name="ds"></param>
    /// <returns>成功：插入成功；返回字符串不是成功：失败原因。 ；null :程序出现异常</returns>
    public string SetTBDCG(DataSet ds)
    {
         string strreSult = "后台未进行任何操作！";
         DataTable dt = ds.Tables["TBD"];
         DataTable dtZZ = ds.Tables["ZZ"];//资质表

         if (ds == null || ds.Tables.Count < 1)
         {
             strreSult ="传递的参数不可为空！";
             return strreSult;
         }

         Hashtable ht_Sql = new Hashtable();
         #region 投标单SQL

         string Time = DateTime.Now.ToString();
         Hashtable ht_ZZ = new Hashtable(); //dt.Rows[0]["其他资质"] == null ? null : (Hashtable)dt.Rows[0]["其他资质"];
         if (dtZZ != null)
         {
             for (int i = 0; i < dtZZ.Columns.Count; i++)
             {
                 ht_ZZ[dtZZ.Columns[i].ColumnName] = dtZZ.Rows[0][dtZZ.Columns[i].ColumnName].ToString();
             }
         }

         I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
         Hashtable input = new Hashtable();
        
        string Number_TBD = "9999999k"; //PublicClass2013.GetNextNumberZZ("AAA_TBDCGB", ""); 2014.0521 wyh delete 此方法移植到系统控制中心----------------
        object[] rezkls = IPC.Call("获取主键", new object[] { "AAA_TBDCGB", "" });

        if (rezkls[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (rezkls[1] != null &&!rezkls[1].ToString().Trim().Equals(""))
            {
                Number_TBD = (string)(rezkls[1]);
            }
            else
            {
                strreSult = "获取投标单草稿主键出错！";
                return strreSult;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            strreSult = rezkls[1].ToString();
            return strreSult;
        }

        input["@Number"] = Number_TBD;
        input["@DLYX"] = dt.Rows[0]["DLYX"].ToString();
        input["@SPBH"] = dt.Rows[0]["SPBH"].ToString();
        input["@HTQX"] = dt.Rows[0]["HTQX"].ToString();
        input["@MJSDJJPL"] = dt.Rows[0]["MJSDJJPL"].ToString();
        input["@GHQY"] = dt.Rows[0]["GHQY"].ToString();
        input["@TBNSL"] = dt.Rows[0]["TBNSL"].ToString();
        input["@TBJG"] = dt.Rows[0]["TBJG"].ToString();
        input["@TBJE"] = dt.Rows[0]["TBJE"].ToString();
        input["@TJSJ"] = Time;
        input["@ZLBZYZM"] = dt.Rows[0]["ZLBZYZM"] == null ? null : dt.Rows[0]["ZLBZYZM"].ToString();
        input["@CPJCBG"] = dt.Rows[0]["CPJCBG"] == null ? null : dt.Rows[0]["CPJCBG"].ToString();
        input["@PGZFZRFLCNS"] = dt.Rows[0]["PGZFZRFLCNS"] == null ? null : dt.Rows[0]["PGZFZRFLCNS"].ToString();
        input["@FDDBRCNS"] = dt.Rows[0]["FDDBRCNS"] == null ? null : dt.Rows[0]["FDDBRCNS"].ToString();
        input["@SHFWGDYCN"] = dt.Rows[0]["SHFWGDYCN"] == null ? null : dt.Rows[0]["SHFWGDYCN"].ToString();
        input["@CPSJSQS"] = dt.Rows[0]["CPSJSQS"] == null ? null : dt.Rows[0]["CPSJSQS"].ToString();
        input["@CreateUser"] = dt.Rows[0]["DLYX"].ToString();
        input["@CreateTime"] = Time;
        input["@ZZ01"] = ht_ZZ["资质1"] == null ? "" : ht_ZZ["资质1"].ToString();
        input["@ZZ02"] = ht_ZZ["资质2"] == null ? "" : ht_ZZ["资质2"].ToString();
        input["@ZZ03"] = ht_ZZ["资质3"] == null ? "" : ht_ZZ["资质3"].ToString();
        input["@ZZ04"] = ht_ZZ["资质4"] == null ? "" : ht_ZZ["资质4"].ToString();
        input["@ZZ05"] = ht_ZZ["资质5"] == null ? "" : ht_ZZ["资质5"].ToString();
        input["@ZZ06"] = ht_ZZ["资质6"] == null ? "" : ht_ZZ["资质6"].ToString();
        input["@ZZ07"] = ht_ZZ["资质7"] == null ? "" : ht_ZZ["资质7"].ToString();
        input["@ZZ08"] = ht_ZZ["资质8"] == null ? "" : ht_ZZ["资质8"].ToString();
        input["@ZZ09"] = ht_ZZ["资质9"] == null ? "" : ht_ZZ["资质9"].ToString();
        input["@ZZ10"] = ht_ZZ["资质10"] == null ? "" : ht_ZZ["资质10"].ToString();
        input["@SLZM"] = dt.Rows[0]["SLZM"] == null ? null : dt.Rows[0]["SLZM"].ToString();
        input["@SPCD"] = dt.Rows[0]["省市区"].ToString();

         string sql_TBD = "INSERT INTO AAA_TBDCGB(Number,DLYX,SPBH,HTQX,MJSDJJPL,GHQY,TBNSL,TBJG,TBJE,TJSJ,ZLBZYZM,CPJCBG,PGZFZRFLCNS,FDDBRCNS,SHFWGDYCN,CPSJSQS,ZZ01,ZZ02,ZZ03,ZZ04,ZZ05,ZZ06,ZZ07,ZZ08,ZZ09,ZZ10,CreateUser,CreateTime,SLZM,SPCD) VALUES(@Number,@DLYX,@SPBH,@HTQX,@MJSDJJPL,@GHQY,@TBNSL,@TBJG,@TBJE,@TJSJ,@ZLBZYZM,@CPJCBG,@PGZFZRFLCNS,@FDDBRCNS,@SHFWGDYCN,@CPSJSQS,@ZZ01,@ZZ02,@ZZ03,@ZZ04,@ZZ05,@ZZ06,@ZZ07,@ZZ08,@ZZ09,@ZZ10,@CreateUser,@CreateTime,@SLZM,@SPCD)";
       
         #endregion

         Hashtable return_all = I_DBL.RunParam_SQL(sql_TBD, input);
         if ((bool)(return_all["return_float"])) //说明执行完成
         {
             if ((int)return_all["return_other"] == 1)
             {
                 strreSult = "成功";
                 return strreSult;
             }
             else
             {
                 strreSult = "插入了" + return_all["return_other"].ToString()+"条数据。";
                 return strreSult;
             }
         }
         else
         {
             //连接数据库过程中发生程序错误了。。。
             strreSult = return_all["return_errmsg"].ToString();
             return strreSult;
         }
        
    }

    /// <summary>
    /// 投标单撤销 wyh 2014.05.23 
    /// </summary>
    /// <param name="Number">投标单单号</param>
    /// <returns>成功：撤销成功；返回字符串不是成功：失败原因。 ；null :程序出现异常</returns>
    public DataSet TBD_CX(string Number)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });


        //?wyh 2014.05.21 add
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@Number"] = Number;
        DataTable dt = null;
        Hashtable return_htzkls = I_DBL.RunParam_SQL("SELECT * FROM AAA_TBD WHERE Number=@Number","TBD", input);
        if ((bool)(return_htzkls["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_htzkls["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["TBD"].Rows.Count > 0)
            {
                dt = dsGXlist.Tables["TBD"];
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有查找到单号为：" + Number+"的投标单。";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_htzkls["return_errmsg"].ToString();
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            return dsreturn;
           
        } //?


        if (dt != null&&dt.Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息5"] = (Convert.ToInt32(dt.Rows[0]["TBNSL"].ToString().Trim()) - Convert.ToInt32(dt.Rows[0]["YZBSL"].ToString().Trim())).ToString().Trim(); //wyh 2014.05.23 dele

         
            
            object[] re = IPC.Call("用户基本信息", new object[] { dt.Rows[0]["MJJSBH"].ToString()}); //----------------------
            DataSet dsUserVal = null;
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。

                dsUserVal = (DataSet)(re[1]);
                if (dsUserVal == null || dsUserVal.Tables[0].Rows.Count <= 0)
                {    
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查找用户信息出错";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    return dsreturn;
           
                }

            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }

            DataRow htUserVal = dsUserVal.Tables[0].Rows[0];
            #region 验证们

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

           
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "true"; 
            ArrayList list = new ArrayList();//要执行的SQL
            DateTime time = DateTime.Now;
            I_Dblink I_DBL_all = (new DBFactory()).DbLinkSqlMain("");
            Hashtable inputall = new Hashtable();
            if (dt.Rows[0]["YZBSL"].ToString() == "0" || dt.Rows[0]["YZBSL"] == DBNull.Value)
            {

                inputall["@Number"] = Number;

                #region 更改投标单状态
                string sql_TBD = "UPDATE AAA_TBD SET ZT='撤销' WHERE Number=@Number";//更改单据状态为撤销
                #endregion
                list.Add(sql_TBD);//更新投标单状态

                #region 生成流水信息

                DataTable dt_LSDZ = GetMoneydbzDS("1304000004").Tables["DZB"];
               

                string Number_zkls = "";
                re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//--------------------

                if (re[0].ToString() == "ok")
                {
                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                    if (re[1] != null && !re[1].ToString().Trim().Equals(""))
                    {
                        Number_zkls = (string)(re[1]);
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账款流水明细表主键获取出错!";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                        return dsreturn;
                    }
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                 
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    return dsreturn;
                }

                inputall["@Numberzkls"] = Number_zkls;
                inputall["@DLYXzkls"] = dt.Rows[0]["DLYX"].ToString();
                inputall["@JSZHLXzkls"] = dt.Rows[0]["JSZHLX"].ToString();
                inputall["@MJJSBHzkls"] = dt.Rows[0]["MJJSBH"].ToString();
                inputall["@LYYWLX"] = "AAA_TBD";
                inputall["@LYDH"] = Number;
                inputall["@LSCSSJ"] = time;
                inputall["@YSLX"] = dt_LSDZ.Rows[0]["YSLX"].ToString();
                inputall["@JE"] = dt.Rows[0]["DJTBBZJ"].ToString();
                inputall["@XM"] = dt_LSDZ.Rows[0]["XM"].ToString();
                inputall["@XZ"] = dt_LSDZ.Rows[0]["XZ"].ToString();
                inputall["@ZY"] = dt_LSDZ.Rows[0]["ZY"].ToString().Replace("[x1]", Number) ;
                inputall["@JKBH"] = "接口编号";
                inputall["@QTBZ"] = Number + "投标单撤销（全单）";
                inputall["@SJLX"] = dt_LSDZ.Rows[0]["SJLX"].ToString();


                string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES(@Numberzkls,@DLYXzkls,@JSZHLXzkls,@MJJSBHzkls,@LYYWLX,@LYDH,@LSCSSJ,@YSLX,@JE,@XM,@XZ,@ZY,@JKBH,@QTBZ,@SJLX)";//插入流水表

                #endregion
                list.Add(sql_LS);//插入流水表

                #region 更新用户余额

                inputall["@genggai"] = dt.Rows[0]["DJTBBZJ"].ToString().Trim();
                inputall["@dlyx_UP"] = dt.Rows[0]["DLYX"].ToString();
                string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+@genggai  WHERE B_DLYX=@dlyx_UP";
                #endregion
                list.Add(sql_YE);//插入余额表


                I_Dblink I_DBL_a = (new DBFactory()).DbLinkSqlMain("");
                DataSet dsaa = new DataSet();
                Hashtable inputa = new Hashtable();
                inputa["@Number"] = "Number";
                DataTable dtaa = null;
                Hashtable return_htaa = I_DBL.RunParam_SQL("SELECT * FROM AAA_TBD WHERE Number=@Number AND ZT='撤销' ","cx", inputa);
                if ((bool)(return_htaa["return_float"])) //说明执行完成
                {
                    //这里就可以用了。
                    dsaa = (DataSet)(return_htaa["return_ds"]);
                    if (dsaa != null && dsaa.Tables["cx"].Rows.Count > 0)
                    {
                        dtaa = dsaa.Tables["cx"];
                    }

                }
                else
                {
                    //读取数据库过程中发生程序错误了。。。
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_htaa["return_errmsg"].ToString();
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    return dsreturn;
                }
                

                if (dtaa==null||dtaa.Rows.Count == 0)
                {
                    Hashtable return_htall= I_DBL_all.RunParam_SQL(list,inputall);
                    if ((bool)(return_htall["return_float"])) //说明执行完成
                    {
                        if ((int)return_htall["return_other"] > 0)
                        {
                            try
                            {
                                DataTable dtJK = new DataTable("dtJK");
                                dtJK.Columns.Add("商品编号");
                                dtJK.Columns.Add("合同期限");
                                dtJK.Rows.Add(new string[] { dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString() });

                                //需调用成交处理中心方法---------------------------------
                                Hashtable htcs = new Hashtable();
                                htcs["@HTQX"] = dt.Rows[0]["HTQX"].ToString();
                                htcs["@SPBH"] = dt.Rows[0]["SPBH"].ToString();
                                ArrayList aljk = LJQ.SFJRLJQ(dtJK);
                                if (aljk != null && aljk.Count > 0)
                                {
                                    I_DBL_all.RunParam_SQL(aljk, htcs);
                                }


                               
                            }
                            catch
                            {

                            }
                            finally
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "投标单撤销成功！";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = Number;       
                            }

                            return dsreturn;
        
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试！";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                            return dsreturn;
                        }
                    }
                    else
                    {
                        //读取数据库过程中发生程序错误了。。。
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_htall["return_errmsg"].ToString();
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                        return dsreturn;

                    } //?
      
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "投标单已被撤销！";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "true";
                }
            }
            else
            {
                #region 更改投标单状态

                inputall["@Number"] = Number;
                string sql_TBD = "UPDATE AAA_TBD SET ZT='中标',TBNSL=YZBSL WHERE Number=@Number";//更改单据状态为撤销
                #endregion
                list.Add(sql_TBD);//更新投标单状态

                #region 生成流水信息

                DataTable dt_LSDZ = GetMoneydbzDS("1304000004").Tables["DZB"];

                string Number_zkls = "";
                 re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });

                if (re[0].ToString() == "ok")
                {
                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                    if (re[1] != null && !re[1].ToString().Trim().Equals(""))
                    {
                        Number_zkls = (string)(re[1]);
                    }
                    else
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账款流水明细表出错!";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                        return dsreturn;
                    }
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。

                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    return dsreturn;
                }

                inputall["@Numberzkls"] = Number_zkls;
                inputall["@DLYXzkls"] = dt.Rows[0]["DLYX"].ToString();
                inputall["@JSZHLXzkls"] = dt.Rows[0]["JSZHLX"].ToString();
                inputall["@MJJSBHzkls"] = dt.Rows[0]["MJJSBH"].ToString();
                inputall["@LYYWLX"] = "AAA_TBD";
                inputall["@LYDH"] = Number;
                inputall["@LSCSSJ"] = time;
                inputall["@YSLX"] = dt_LSDZ.Rows[0]["YSLX"].ToString();
                inputall["@JE"] = dt.Rows[0]["DJTBBZJ"].ToString();
                inputall["@XM"] = dt_LSDZ.Rows[0]["XM"].ToString();
                inputall["@XZ"] = dt_LSDZ.Rows[0]["XZ"].ToString();
                inputall["@ZY"] = dt_LSDZ.Rows[0]["ZY"].ToString().Replace("[x1]", Number);
                inputall["@JKBH"] = "接口编号";
                inputall["@QTBZ"] = Number + "投标单撤销（拆单）";
                inputall["@SJLX"] = dt_LSDZ.Rows[0]["SJLX"].ToString();


                string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES(@Numberzkls,@DLYXzkls,@JSZHLXzkls,@MJJSBHzkls,@LYYWLX,@LYDH,@LSCSSJ,@YSLX,@JE,@XM,@XZ,@ZY,@JKBH,@QTBZ,@SJLX)";//插入流水表


                #endregion
                list.Add(sql_LS);//插入流水表

                #region 更新用户余额
                inputall["@genggai"] = dt.Rows[0]["DJTBBZJ"].ToString().Trim();
                inputall["@dlyx_UP"] = dt.Rows[0]["DLYX"].ToString();
                string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+@genggai  WHERE B_DLYX=@dlyx_UP";

                #endregion
                list.Add(sql_YE);//插入余额表

                I_Dblink I_DBL_a = (new DBFactory()).DbLinkSqlMain("");
                DataSet dsaa = new DataSet();
                Hashtable inputa = new Hashtable();
                inputa["@Number"] = "Number";
                DataTable dtaa = null;
                Hashtable return_htaa = I_DBL.RunParam_SQL("SELECT * FROM AAA_TBD WHERE Number=@Number AND ZT='中标' ","zb", inputa);
                if ((bool)(return_htaa["return_float"])) //说明执行完成
                {
                    //这里就可以用了。
                    dsaa = (DataSet)(return_htaa["return_ds"]);
                    if (dsaa != null && dsaa.Tables["zb"].Rows.Count > 0)
                    {
                        dtaa = dsaa.Tables["zb"];
                    }

                }
                else
                {
                    //读取数据库过程中发生程序错误了。。。
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_htaa["return_errmsg"].ToString();
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    return dsreturn;
                }


                if (dtaa==null||dsaa.Tables[0].Rows.Count== 0)
                {
                    Hashtable return_htall = I_DBL_all.RunParam_SQL(list, inputall);
                    if ((bool)(return_htall["return_float"])) //说明执行完成
                    {
                        if ((int)return_htall["return_other"] > 0)
                        {
                            try
                            {
                                DataTable dtJK = new DataTable("dtJK");
                                dtJK.Columns.Add("商品编号");
                                dtJK.Columns.Add("合同期限");
                                dtJK.Rows.Add(new string[] { dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString() });

                                //需调用成交处理中心方法---------------------------------

                                re=  IPC.Call("成交处理", new object[] { dtJK,"提交" });
                                if (re[0].ToString().Trim().Equals("ok"))
                                {

                                    DataSet dsjk = (DataSet)re[1];
                                    ArrayList aljk = new ArrayList();
                                    if (dsjk != null && dsjk.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dsjk.Tables[0].Rows)
                                        {
                                            aljk.Add(dr["sql"].ToString());
                                        }
                                    }

                                    if (aljk != null && aljk.Count > 0)
                                    {
                                        I_DBL_all.RunParam_SQL(aljk);
                                    }
                                }

                                
                            }
                            catch
                            {

                            }
                            finally
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "投标单撤销成功！";
                                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = Number;
                            }

                            return dsreturn;

                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试！";
                            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                            return dsreturn;
                        }
                    }
                    else
                    {
                        //读取数据库过程中发生程序错误了。。。
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_htall["return_errmsg"].ToString();
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                        return dsreturn;

                    } //?
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
    /// 投标单异常修改 wyh  2014.05.26 add
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>成功：操作成功；非成功：失败原因。</returns>
    public string  Update(DataSet ds)
    {
        string strlable = "";
        try
        {
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable tblGDZZ = ds.Tables["固定资质表"];
                DataTable tblTSZZ = ds.Tables["特殊资质表"];
                DataTable tblTBDH = ds.Tables["投标单号表"];
                if (tblTBDH != null && tblTBDH.Rows.Count > 0)
                {
                    string TBDH = tblTBDH.Rows[0]["TBDH"].ToString();
                    #region//验证交易管理部审核状态

                    string  shzt = TBDSHZT(TBDH);
                    if (shzt != null && shzt.ToString() != "")
                    {
                        if (shzt.ToString() == "审核通过")
                        {
                            
                           strlable="该异常投标单已审核通过，不能进行修改操作！";
                           return strlable;
                        }
                    }
                    else
                    {
                      strlable = "未查询到该异常投标单的信息！";
                      return strlable;
                    }
                    #endregion

                    I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
                    DataSet dsGXlist = new DataSet();
                    Hashtable input = new Hashtable();

                    ArrayList list = new ArrayList();
                    string strTBD = "update AAA_TBD set ";
                    #region//固定资质
                    if (tblGDZZ.Rows.Count > 0)
                    {
                        foreach (DataRow row in tblGDZZ.Rows)
                        {
                            strTBD += row["Name"].ToString() + "=@" + row["Name"].ToString() + ",";
                            input["@"+row["Name"].ToString()] = row["Value"].ToString();
                        }
                    }
                    #endregion
                    #region//特殊资质
                    if (tblTSZZ.Rows.Count > 0)
                    {
                        string tszz = "|";
                        foreach (DataRow row in tblTSZZ.Rows)
                        {
                            tszz += row["Name"].ToString() + "*" + row["Value"].ToString() + "|";
                        }
                        strTBD += " ZZ01 =@ZZ01,";
                        input["@ZZ01"] = tszz;
                    }
                    strTBD = strTBD.TrimEnd(',');
                    input["@Number"] = TBDH.Trim();
                    strTBD += " where Number=@Number ";
                    if (tblGDZZ.Rows.Count > 0 || tblTSZZ.Rows.Count > 0)
                    {
                        list.Add(strTBD);
                    }
                    #endregion
                    #region//更新投标资料审核表
                    input["@TBDH"] = TBDH.Trim();
                    string str = "update AAA_TBZLSHB set FWZXSHYCHSFXG='是',JYGLBSHWTGHSFXG='是',MJZXXGSJ=getdate() where TBDH=@TBDH";
                    list.Add(str);
                    #endregion

                    Hashtable return_all = I_DBL.RunParam_SQL(list, input);
                       if ((bool)(return_all["return_float"])) //说明执行完成
                       {
                           if ((int)return_all["return_other"] > 0)
                           {
                               strlable = "成功";
                               return strlable;
                           }
                           else
                           {
                               strlable = "数据受影响行数为0";
                               return strlable;
                           }
                       }
                       else
                       {
                           //读取数据库过程中发生程序错误了。。。
                           strlable = return_all["return_errmsg"].ToString();
                           return strlable;
                       } //?
                }
                else
                {
                    strlable = "传递的参数不完整！";
                    return strlable;
                }
            }
            else
            {

                strlable = "传递的参数不完整！";
                return strlable;
            }
        }
        catch (Exception e)
        {  
           strlable = e.Message;
           return strlable;
        }
        
    }

    public string TBDSHZT(string tbdh)
    {
        string strreSult = "查询失败";
        //?wyh 2014.05.21 add
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();

        //验证商品是否已经投标
        input["@TBDH"] = tbdh;
        Hashtable return_ht = I_DBL.RunParam_SQL("select JYGLBSHZT from AAA_TBZLSHB where TBDH=@TBDH","shzt", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["shzt"].Rows.Count > 0)
            {
                strreSult = dsGXlist.Tables["shzt"].Rows[0][0].ToString();
                return strreSult;
            }
        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
            strreSult = return_ht["return_errmsg"].ToString();
            return strreSult;
        } //?
        return strreSult;
    }

    /// <summary>
    /// 主要用于投标担修改时查询投标单原始数据
    /// </summary>
    /// <param name="Number">投标单号</param>
    /// <returns>返回符合dsretrun结构的dataset,同时含有表明为：SPXX的Datatable</returns>
    public DataSet Tbd_Change(string Number)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", ""});
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();

        try
        {
            input["@Number"] = Number;
            string sql = "SELECT AAA_TBD.Number, AAA_TBD.DLYX, AAA_TBD.JSZHLX,AAA_TBD.SPCD, AAA_TBD.MJJSBH, AAA_TBD.GLJJRYX, AAA_TBD.GLJJRYHM, AAA_TBD.GLJJRJSBH, AAA_TBD.GLJJRPTGLJG, AAA_TBD.SPBH, AAA_TBD.HTQX, AAA_TBD.MJSDJJPL, AAA_TBD.GHQY, (AAA_TBD.TBNSL-AAA_TBD.YZBSL) as TBNSL, AAA_TBD.TBJG, AAA_TBD.TBJE, AAA_TBD.MJTBBZJBL, AAA_TBD.MJTBBZJZXZ, AAA_TBD.DJTBBZJ, AAA_TBD.ZT, AAA_TBD.TJSJ, AAA_TBD.CXSJ, AAA_TBD.ZLBZYZM, AAA_TBD.CPJCBG, AAA_TBD.PGZFZRFLCNS, AAA_TBD.FDDBRCNS, AAA_TBD.SHFWGDYCN, AAA_TBD.SLZM, AAA_TBD.CPSJSQS, AAA_TBD.ZZ01, AAA_TBD.ZZ02, AAA_TBD.ZZ03, AAA_TBD.ZZ04, AAA_TBD.ZZ05, AAA_TBD.ZZ06, AAA_TBD.ZZ07, AAA_TBD.ZZ08, AAA_TBD.ZZ09, AAA_TBD.ZZ10, AAA_PTSPXXB.SPMC, AAA_PTSPXXB.GG, AAA_PTSPXXB.SSFLBH, AAA_PTSPXXB.JJDW, AAA_PTSPXXB.JJPL, AAA_PTSPXXB.SPMS, AAA_PTSPXXB.SCZZYQ, AAA_PTSPXXB.FBRQ, AAA_PTSPXXB.SFYX, AAA_PTSPXXB.ZFRQ, ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标'  AND (SELECT HTQX FROM AAA_TBD WHERE Number=@Number)=AAA_TBD.HTQX  ORDER BY TBJG ASC),0) AS ZDJG, ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标'  AND (SELECT HTQX FROM AAA_TBD WHERE Number=@Number)=AAA_TBD.HTQX  ORDER BY MJSDJJPL DESC) ,0) AS MJJJPL FROM  AAA_TBD INNER JOIN AAA_PTSPXXB ON AAA_TBD.SPBH = AAA_PTSPXXB.SPBH WHERE  AAA_TBD.Number=@Number";
    
            Hashtable return_ht = I_DBL.RunParam_SQL(sql, "SPInfo", input);
            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                //这里就可以用了。
                dsGXlist = (DataSet)(return_ht["return_ds"]);
                if (dsGXlist != null && dsGXlist.Tables["SPInfo"].Rows.Count > 0)
                {
                    DataTable dt = dsGXlist.Tables["SPInfo"].Copy();
                    dt.TableName = "SPXX";
                    dsreturn.Tables.Add(dt);
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";

                }
                else
                {

                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                }

            }
            else
            {
                //读取数据库过程中发生程序错误了。。。
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_ht["return_errmsg"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                
            } //?
           
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
        }


        return dsreturn;
    }


    //查询投标单的异常资质
    /// <summary>
    /// 查询投标单的异常资质 wyh 2014.05.30 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsreturn的Dataset包含标明为”主表“的Datatable</returns>
    public DataSet CXTBDYCZZ(DataSet ds)
    {
        DataSet returnDS = initReturnDataSet().Clone();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });

        try
        {
            DataTable table = ds.Tables[0];
            if (table != null && table.Rows.Count > 0)
            {

                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
                DataSet dsGXlist = new DataSet();
                Hashtable input = new Hashtable();

                input["@DLYX"] = table.Rows[0]["DLYX"].ToString();
                input["@TBDH"] = table.Rows[0]["TBDH"].ToString();
               // string TBDH = table.Rows[0]["TBDH"].ToString();
                string str = "select * from AAA_TBZLSHB a left join AAA_TBD b on a.TBDH=b.Number where DLYX=@DLYX and a.TBDH=@TBDH";
                Hashtable return_ht = I_DBL.RunParam_SQL(str, "aa", input);
                if ((bool)(return_ht["return_float"])) //说明执行完成
                {
                    //这里就可以用了。
                    dsGXlist = (DataSet)(return_ht["return_ds"]);
                    if (dsGXlist != null && dsGXlist.Tables["aa"].Rows.Count > 0)
                    {
                        DataTable dt = dsGXlist.Tables["aa"].Copy();
                        dt.TableName = "主表";
                        returnDS.Tables.Add(dt);
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";

                    }
                    else
                    {

                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试";
                        returnDS.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    }

                }
                else
                {
                    //读取数据库过程中发生程序错误了。。。
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = return_ht["return_errmsg"].ToString();
                    returnDS.Tables["返回值单条"].Rows[0]["附件信息1"] = "";

                } //?
           
                
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


    /// <summary>
    /// 初始化返回值数据集,执行结果只有两种ok和err
    /// </summary>
    /// <returns></returns>
    public DataSet initReturnDataSet()
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

    public DataSet  GetMoneydbzDS(string Number)
    {
        #region//zhouli作废 -- 替换成redis--2014.11.05
        DataSet ds = null;

        I_Dblink I_DBL_ZKLS = (new DBFactory()).DbLinkSqlMain("");
        DataSet dszkls = new DataSet();
        Hashtable inputzkls = new Hashtable();

        inputzkls["@NUMBER"] = Number;

        Hashtable return_htzkls = I_DBL_ZKLS.RunParam_SQL("SELECT * FROM AAA_moneyDZB WHERE NUMBER=@NUMBER", "DZB", inputzkls);
        if ((bool)(return_htzkls["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dszkls = (DataSet)(return_htzkls["return_ds"]);
            if (dszkls != null && dszkls.Tables["DZB"].Rows.Count > 0)
            {
                ds = dszkls;
            }
        }

        return ds;

        #endregion

       // return IsMoneyDZB.ReadRedisMoneyDZB(new string[] { Number }, null, "DZB");

    }

    /// <summary>
    ///得到投标单的信息 察看合同时使用 wyh 2014.07.14 
    /// </summary>
    /// <param name="dsreturn">返回值</param>
    /// <param name="dataTable">传递的参数</param>
    /// <returns></returns>
    public DataSet Get_TBDData( DataTable dataTable)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });
        //?wyh 2014.05.21 add
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();

        input.Add("@Number", dataTable.Rows[0]["中标定标_Number"].ToString());
        DataTable dataTableTBD = null;
        string strSql = "select TT.Number '投标单号',TT.TJSJ '发布时间',DD.I_JYFMC '卖方名称',TT.SPMC '拟卖出商品名称',TT.GG '规格',TT.JJDW '计价单位',TT.TBNSL '投标拟售量',TT.TBJG '投标价格',TT.TBJE '投标金额',TT.DJTBBZJ '冻结的投标保证金',TT.PTSDZDJJPL '平台设定最大经济批量',TT.MJSDJJPL '卖方设定的经济批量',TT.HTQX '合同期限',TT.GHQY '可供货区域' from AAA_ZBDBXXB as ZZ left join AAA_TBD as TT on ZZ.T_YSTBDBH=TT.Number left join AAA_DLZHXXB as DD on TT.MJJSBH=DD.J_SELJSBH where ZZ.Number=@Number";
        Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "o", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["o"].Rows.Count > 0)
            {
                dataTableTBD = dsGXlist.Tables["o"].Copy();
                dataTableTBD.TableName = "投标单";
                dsreturn.Tables.Add(dataTableTBD);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";

            }
            else
            {

                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有查询到任何数据";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
            }

        }
        else
        {
            //读取数据库过程中发生程序错误了。。。
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_ht["return_errmsg"].ToString();
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";

        } //?
       
       
        return dsreturn;

    }


    /// <summary>
    /// 获得投标单草稿信息获得 wyh 2014.07.14 
    /// </summary>
    /// <param name="Number">投标单草稿单号</param>
    /// <returns></returns>
    public DataSet TBD_GetCGinfo( string Number)
    {

    

        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });
        //?wyh 2014.05.21 add
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable input = new Hashtable();

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到对应的预订单信息！";
        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";

     

        try
        {
            input.Add("@Number", Number);
            string sql = " SELECT AAA_TBDCGB.Number, AAA_TBDCGB.DLYX, AAA_TBDCGB.JSZHLX,AAA_TBDCGB.SPCD, AAA_TBDCGB.MJJSBH, AAA_TBDCGB.GLJJRYX, AAA_TBDCGB.GLJJRYHM, AAA_TBDCGB.GLJJRJSBH, AAA_TBDCGB.GLJJRPTGLJG, AAA_TBDCGB.SPBH, AAA_TBDCGB.HTQX, AAA_TBDCGB.MJSDJJPL, AAA_TBDCGB.GHQY, AAA_TBDCGB.TBNSL, AAA_TBDCGB.TBJG, AAA_TBDCGB.TBJE, AAA_TBDCGB.MJTBBZJBL, AAA_TBDCGB.MJTBBZJZXZ, AAA_TBDCGB.DJTBBZJ, AAA_TBDCGB.ZT, AAA_TBDCGB.TJSJ, AAA_TBDCGB.CXSJ, AAA_TBDCGB.ZLBZYZM, AAA_TBDCGB.CPJCBG, AAA_TBDCGB.PGZFZRFLCNS, AAA_TBDCGB.FDDBRCNS, AAA_TBDCGB.SHFWGDYCN, AAA_TBDCGB.CPSJSQS, AAA_TBDCGB.ZZ01, AAA_TBDCGB.ZZ02, AAA_TBDCGB.ZZ03, AAA_TBDCGB.ZZ04, AAA_TBDCGB.ZZ05, AAA_TBDCGB.ZZ06, AAA_TBDCGB.ZZ07, AAA_TBDCGB.ZZ08, AAA_TBDCGB.ZZ09, AAA_TBDCGB.ZZ10, AAA_TBDCGB.SLZM, AAA_PTSPXXB.SPMC, AAA_PTSPXXB.GG, AAA_PTSPXXB.SSFLBH, AAA_PTSPXXB.JJDW, AAA_PTSPXXB.JJPL, AAA_PTSPXXB.SPMS, AAA_PTSPXXB.SCZZYQ, AAA_PTSPXXB.FBRQ, AAA_PTSPXXB.SFYX, AAA_PTSPXXB.ZFRQ,(ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_TBDCGB.SPBH and AAA_TBD.HTQX=AAA_TBDCGB.HTQX  AND ZT='竞标' ORDER BY TBJG ASC),0)) as ZDJG,( select JJPL from AAA_PTSPXXB where SPBH=AAA_TBDCGB.SPBH )as JJPL,(SELECT TOP 1 SCZZYQ FROM AAA_PTSPXXB WHERE AAA_PTSPXXB.SPBH=AAA_TBDCGB.SPBH ) as SCZZYQ,(ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_TBDCGB.SPBH AND ZT='竞标' and AAA_TBD.HTQX = AAA_TBDCGB.HTQX ORDER BY MJSDJJPL DESC) ,0)) as MJJJPL  FROM AAA_TBDCGB  left join AAA_PTSPXXB  on AAA_TBDCGB.SPBH=AAA_PTSPXXB.SPBH  WHERE AAA_TBDCGB.Number =@Number";
            DataSet ds = null;//DbHelperSQL.Query(sql);
            Hashtable return_ht = I_DBL.RunParam_SQL(sql, "CG", input);

            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                //这里就可以用了。
                ds = (DataSet)(return_ht["return_ds"]);

            }


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



   
}