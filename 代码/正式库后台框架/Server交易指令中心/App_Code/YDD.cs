/***********************************************************************
 * 
 *创建人：wyh
 *
 *创建时间：2014.05.28
 *
 *代码功能：实现预订单各项功能
 *
 *参考文档：《业务分析》交易指令中心部分
 *
 * 
 * ***********************************************************************/

using FMDBHelperClass;
using FMipcClass;
using System;
using System.Collections;
using System.Data;

/// <summary>
/// YDD 的摘要说明
/// </summary>
public class YDD
{
	public YDD()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}


    /// <summary>
    /// 验证预订单信息，将客户端提交过来的DataTable数据传到本方法中，方法将返回相同架构的DataTable作为最后验证数据和附加信息 wyh 2014.05.28 add
    /// </summary>
    /// <param name="dtYDD">预订单初始信息数据</param>
    /// <param name="Result">算法进行评估，如果客户端的值与服务器端的值相差过大，不允许提交数据</param>
    /// <param name="Info">Result为true时，本信息没有价值。为False时，可将此信息作为错误数据返回给客户端</param>
    public static DataTable ValYDD(DataTable dtYDD, ref bool Result, ref string Info)
    {
        double DJDJ = Convert.ToDouble(dtYDD.Rows[0]["DJDJ"]);//冻结订金
        string HTQX = dtYDD.Rows[0]["HTQX"].ToString();//合同期限 
        double PTSDZDJJPL = Convert.ToDouble(dtYDD.Rows[0]["PTSDZDJJPL"]);//平台设定最低经济批量
        double NMRJG = Convert.ToDouble(dtYDD.Rows[0]["NMRJG"]);//拟买入价格
        double NDGSL = Convert.ToDouble(dtYDD.Rows[0]["NDGSL"]);//拟订购数量
        double MJDJBL = Convert.ToDouble(dtYDD.Rows[0]["MJDJBL"]);//冻结比例
        double NDGJE = Convert.ToDouble(dtYDD.Rows[0]["NDGJE"]);//拟定购金额
      
        if (NDGSL < PTSDZDJJPL)
        {
            Result = false;
            Info = "拟订购数量不能小于最低经济批量";
            return dtYDD;
        }

        if (NDGJE != NMRJG * NDGSL)
        {
            if (Math.Abs(NMRJG * NDGSL - NDGJE) >= 1.00)
            {
                //运算错误
                Result = false;
                Info = "数据运算错误，请勿使用非法软件。";
                return dtYDD;
            }
        }

        dtYDD.Rows[0]["DJDJ"] = Math.Round((NMRJG * NDGSL * MJDJBL), 2);
        Result = true;
        return dtYDD;
    }



    /// <summary>
    /// 下达预订单 wyh 2014.05.27 add
    /// </summary>
    /// <param name="dt">各种信息</param>
    /// <returns>成功：预订单提交成功；非成功字符：预订单提交失败，返回失败原因。null：程序出现Bug</returns>
    public string  SetYDD(DataSet ds)
    {
        string strretrun = "没有执行任何操作";
        DataTable dt = ds.Tables["YDD"];
        bool valFlag = false;
        string info = string.Empty;
        dt = InitValidater.ValYDD(dt, ref valFlag, ref info);       

        if (!valFlag)
        {
            strretrun = info;
            return strretrun;
        }

        Hashtable HTCSAll = (new SetYDD_hqcs()).Get_hqcs("1304000018", dt.Rows[0]["MJJSBH"].ToString(), dt.Rows[0]["DLYX"].ToString());        

        if (HTCSAll == null || HTCSAll.Count!=6)
        {
            string csAll="";
            foreach (System.Collections.DictionaryEntry de in HTCSAll)
            {
                csAll += de.Key.ToString()+"-";//得到key
            }
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "YDD获取所有参数个数不对：" + HTCSAll.Count + "-" + dt.Rows[0]["MJJSBH"].ToString() + "-获取参数：" + csAll, null);
            return null;

        }

        object[] re =HTCSAll["用户基本信息"] as object[]; //-------------------------------------
        DataSet dsUserVal = null;
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。

            dsUserVal = (DataSet)(re[1]);
            if (dsUserVal == null || dsUserVal.Tables[0].Rows.Count <= 0)
            {
                strretrun = "查找用户信息出错";
                return strretrun;
            }

        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            strretrun = re[1].ToString();
            return strretrun;

        }
        DataRow htUserVal = dsUserVal.Tables[0].Rows[0];

        #region 验证们

        DataRow JJRInfo = null;
        DataSet dsjjrinfo = null;
        re =HTCSAll["关联经纪人信息"] as object[] ;//-----------
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            dsjjrinfo = (DataSet)(re[1]);
            
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            strretrun ="获取关联经纪人错误："+ re[1].ToString();
            return strretrun;

        }

        if (dsjjrinfo != null && dsjjrinfo.Tables[0].Rows.Count > 0)
        {
             JJRInfo = dsjjrinfo.Tables[0].Rows[0];
            dt.Rows[0]["GLJJRYX"] = JJRInfo["关联经纪人登陆账号"];
            dt.Rows[0]["GLJJRYHM"] = JJRInfo["关联经纪人用户名"];
            dt.Rows[0]["GLJJRJSBH"] = JJRInfo["关联经纪人编号"];
            dt.Rows[0]["GLJJRPTGLJG"] = JJRInfo["关联经纪人平台管理机构"];
           
        }
        else
        {
            strretrun = "没有获取到相关经纪人信息！";
            return strretrun;
        }
       
        #endregion

        string Time = DateTime.Now.ToString();//本单统一的时间
        #region 预订单SQL

        I_Dblink I_DBL_all = (new DBFactory()).DbLinkSqlMain("");
        Hashtable inputall = new Hashtable();
        ArrayList al = new ArrayList();


        string Number_YDD = "9999999999";//获取主键已经移植到 系统控制中心-----------------

        re =HTCSAll["AAA_YDDXXB获取主键"] as object[] ;//--------------------
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (re[1] != null && !re[1].ToString().Trim().Equals(""))
            {
                Number_YDD = (string)(re[1]);
            }
            else
            {
                strretrun = "获取预订单主键出错！";
                return strretrun;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            strretrun = "获取预订单主键错误：" + re[1].ToString();
            return strretrun;
        }

        string sql_YDD = "INSERT INTO AAA_YDDXXB(Number,DLYX,JSZHLX,MJJSBH,GLJJRYX,GLJJRYHM,GLJJRJSBH,GLJJRPTGLJG,SPBH,SPMC,GG,JJDW,PTSDZDJJPL,HTQX,SHQY,SHQYsheng,SHQYshi,NMRJG,NDGSL,YZBSL,NDGJE,MJDJBL,DJDJ,ZT,TJSJ,CreateTime,YXJZRQ) VALUES(@Number,@DLYX,@JSZHLX,@MJJSBH,@GLJJRYX,@GLJJRYHM,@GLJJRJSBH,@GLJJRPTGLJG,@SPBH,@SPMC,@GG,@JJDW,@PTSDZDJJPL,@HTQX,@SHQY,@SHQYsheng,@SHQYshi,@NMRJG,@NDGSL,@YZBSL,@NDGJE,@MJDJBL,@DJDJ,@ZT,@TJSJ,@CreateTime,@YXJZRQ)";

            inputall["@Number"] = Number_YDD;
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
            inputall["@SHQY"] = dt.Rows[0]["SHQY"].ToString();
            inputall["@SHQYsheng"] = dt.Rows[0]["SHQYsheng"].ToString();
            inputall["@SHQYshi"] = dt.Rows[0]["SHQYshi"].ToString();
            inputall["@NMRJG"] = dt.Rows[0]["NMRJG"].ToString();
            inputall["@NDGSL"] = dt.Rows[0]["NDGSL"].ToString();
            inputall["@YZBSL"] = dt.Rows[0]["YZBSL"].ToString();
            inputall["@NDGJE"] = dt.Rows[0]["NDGJE"].ToString();
            inputall["@MJDJBL"] = dt.Rows[0]["MJDJBL"].ToString();
            inputall["@DJDJ"] = dt.Rows[0]["DJDJ"].ToString();
            inputall["@ZT"] = "竞标";
            inputall["@TJSJ"] = Time;
            inputall["@CreateTime"] = Time;
          
            if (dt.Rows[0]["YXJZRQ"].ToString() == "")
            {     
                inputall["@YXJZRQ"] = null;          
            }
            else
            {                            
               inputall["@YXJZRQ"] = dt.Rows[0]["YXJZRQ"].ToString();                       
            }

            al.Add(sql_YDD);

        #endregion
        #region 账款流水明细表
        double DJ = Convert.ToDouble(dt.Rows[0]["DJDJ"]);//要冻结的订金
        TBD tbd = new TBD();
        DataTable dt_LS = HTCSAll["DZB"] as DataTable; //FMOP.DB.DbHelperSQL.Query("SELECT * FROM AAA_moneyDZB WHERE NUMBER='1304000018'").Tables[0];

        string Number_LS = "999999";// wyh 2014.05.27  获取主键方法拆分到系统控制中心-------------   PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
        re = HTCSAll["AAA_ZKLSMXB获取主键"] as object[]; //--------------------
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (re[1] != null && !re[1].ToString().Trim().Equals(""))
            {
                Number_LS = (string)(re[1]);
            }
            else
            {
                strretrun = "获取账款流水主键出错！";
                return strretrun;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            strretrun = "获取账款流水主键错误：" + re[1].ToString();
            return strretrun;
        }


        string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,SJLX,CreateUser,CreateTime) VALUES(@Number_zkls,@DLYX_zkls,@JSZHLX_zkls,@JSBH_zkls,@LYYWLX_zkls,@LYDH_zkls,@LSCSSJ_zkls,@YSLX_zkls,@JE_zkls,@XM,@XZ,@ZY,@JKBH,@SJLX_zkls,@CreateUser_zkls,@CreateTime_zkls)";
        al.Add(sql_LS);
        inputall["@Number_zkls"] = Number_LS;
        inputall["@DLYX_zkls"] = htUserVal["登录邮箱"].ToString();
        inputall["@JSZHLX_zkls"] = htUserVal["结算账户类型"].ToString();
        inputall["@JSBH_zkls"] = htUserVal["买家角色编号"].ToString();
        inputall["@LYYWLX_zkls"] = "AAA_YDDXXB";
        inputall["@LYDH_zkls"] = Number_YDD;
        inputall["@LSCSSJ_zkls"] = Time;
        inputall["@YSLX_zkls"] = dt_LS.Rows[0]["YSLX"].ToString().Trim();
        inputall["@JE_zkls"] = DJ;
        inputall["@XM"] = dt_LS.Rows[0]["XM"].ToString().Trim();
        inputall["@XZ"] = dt_LS.Rows[0]["XZ"].ToString().Trim();
        inputall["@ZY"] = dt_LS.Rows[0]["ZY"].ToString().Trim().Replace("[x1]", Number_YDD);
        inputall["@JKBH"] = "交易指令中心";
        inputall["@SJLX_zkls"] = dt_LS.Rows[0]["SJLX"].ToString().Trim();
        inputall["@CreateUser_zkls"] = htUserVal["登录邮箱"].ToString();
        inputall["@CreateTime_zkls"] = Time;

       
        #endregion
        #region 更新用户余额
        string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE-@JE_zkls WHERE B_DLYX=@CreateUser_zkls";
        al.Add(sql_YE);
        #endregion
        try
        {
            #region 再次验证余额，然后执行SQL
            //验证余额
            //验证余额
            double DJ_Again = Convert.ToDouble(dt.Rows[0]["DJDJ"]);//要冻结的订金
            double YE_Again = 0;//  wyh 2014.05.27 获取用户可用余额--------------------已移植到用户控制中心 AboutMoney.GetMoneyT(dt.Rows[0]["DLYX"].ToString(), "");//当前余额 账户当前可用余额
            re = HTCSAll["账户当前可用余额"] as object[];//--------------------
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && !re[1].ToString().Trim().Equals(""))
                {
                    YE_Again = double.Parse((re[1]).ToString());
                }
                else
                {
                    strretrun = "账户当前可用余额错误！";
                    return strretrun;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                strretrun = "账户当前可用余额错误：" + re[1].ToString();
                return strretrun;
            }


            if (DJ_Again > YE_Again)
            {
                strretrun = "您交易账户中的可用资金余额为" + YE_Again.ToString("#0.00") + "元，与该预订单的订金差额为" + (Math.Round((DJ - YE_Again), 2)).ToString("#0.00") + "元。您可增加账户的可用资金余额或修改预订单。";
                return strretrun;
            }

           if (dt.Rows[0]["FLAG"].ToString().Trim() == "预执行")
           {
               strretrun = "您的预订单一经提交将冻结" + DJ.ToString("#0.00") + "元的订金。";
               return strretrun;
           }
            #endregion

           Hashtable return_all = I_DBL_all.RunParam_SQL(al, inputall);
           
           if ((bool)(return_all["return_float"])) //说明执行完成
           {
               if ((int)return_all["return_other"] > 0)
               { 
                   strretrun = "成功";

                   if (dt.Rows[0]["HTQX"].ToString() == "即时")
                   {
                       //

                   }
                   else
                   {
                       DataTable dtJK = new DataTable("dtjk");
                       dtJK.Columns.Add("商品编号");
                       dtJK.Columns.Add("合同期限");
                       dtJK.Rows.Add(new string[] { dt.Rows[0]["SPBH"].ToString(), dt.Rows[0]["HTQX"].ToString() });


                       //需调用成交处理中心方法---------------------------------
                       Hashtable htcs= new Hashtable();
                       htcs["@HTQX"]=dt.Rows[0]["HTQX"].ToString();
                       htcs["@SPBH"]=dt.Rows[0]["SPBH"].ToString();

                       ArrayList aljk = LJQ.SFJRLJQ(dtJK);                       
                       if (aljk != null && aljk.Count > 0)
                       {
                          I_DBL_all.RunParam_SQL(aljk, htcs);
                       }
                   }
                   return strretrun;
               }
               else
               {
                   strretrun = "数据受影响行数为0";
                   FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "SQL 执行状态：-没报错，但是插入0调数据", null);
                   return strretrun;
               }
           }
           else
           {
               //读取数据库过程中发生程序错误了。。。
               strretrun = return_all["return_errmsg"].ToString();
               FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "SQL 执行状态：-" + strretrun, null);
               return strretrun;
           } //?
 
        }
        catch(Exception ex)
        {
            strretrun = "系统繁忙，请稍后再试！";
            FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "SQL 执行状态：-" + ex.ToString(), null);
            return strretrun;
        }
    }


    /// <summary>
    /// 下达预订单草稿 wyh 2014.05.27 add
    /// </summary>
    /// <param name="dt">各种信息</param>
    /// <returns>返回字符串，成功：提交成功；非成功字符串：未成功原因；null:程序出现bug</returns>
    public string  SetYDDCG(DataSet ds)
    {
        string strreturn = "函数没有执行操作";
        DataTable dt = ds.Tables["YDD"];
       
        I_Dblink I_DBL_all = (new DBFactory()).DbLinkSqlMain("");
        Hashtable inputall = new Hashtable();
       

        string Time = DateTime.Now.ToString();//本单统一的时间
        #region 预订单SQL
        string Number_YDD = "999999999";//wyh 2014.05.27  获取主键方法拆分到系统控制中心-------------//PublicClass2013.GetNextNumberZZ("AAA_YDDCGB", "");
        object[] re = IPC.Call("获取主键", new object[] { "AAA_YDDCGB", "" });//--------------------
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            if (re[1] != null && !re[1].ToString().Trim().Equals(""))
            {
                Number_YDD = (string)(re[1]);
            }
            else
            {
                strreturn = "获取预订单草稿主键出错！";
                return strreturn;
            }
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            strreturn = "获取预订单草稿主键错误：" + re[1].ToString();
            return strreturn;
        }

        string sql_YDD = "INSERT INTO AAA_YDDCGB(Number,DLYX,SPBH,HTQX,SHQY,SHQYsheng,SHQYshi,NMRJG,NDGSL,NDGJE,TJSJ,CreateTime,YXJZRQ) VALUES(@Number,@DLYX,@SPBH,@HTQX,@SHQY,@SHQYsheng,@SHQYshi,@NMRJG,@NDGSL,@NDGJE,@TJSJ,@CreateTime,@YXJZRQ)";
        
        inputall["@Number"] = Number_YDD;
        inputall["@DLYX"] = dt.Rows[0]["DLYX"].ToString();
        inputall["@SPBH"] = dt.Rows[0]["SPBH"].ToString();
        inputall["@HTQX"] = dt.Rows[0]["HTQX"].ToString();
        inputall["@SHQY"] = dt.Rows[0]["SHQY"].ToString();
        inputall["@SHQYsheng"] = dt.Rows[0]["SHQYsheng"].ToString();
        inputall["@SHQYshi"] = dt.Rows[0]["SHQYshi"].ToString();
        inputall["@NMRJG"] = dt.Rows[0]["NMRJG"].ToString();
        inputall["@NDGSL"] = dt.Rows[0]["NDGSL"].ToString();
        inputall["@NDGJE"] = dt.Rows[0]["NDGJE"].ToString();
        inputall["@TJSJ"] = Time;
        inputall["@CreateTime"] = Time;
        

        if (dt.Rows[0]["YXJZRQ"].ToString() == "")
        {
            inputall["@YXJZRQ"] = null;
        }
        else
        {
            inputall["@YXJZRQ"] = dt.Rows[0]["YXJZRQ"].ToString();
        }

        #endregion
        Hashtable htreturn= I_DBL_all.RunParam_SQL(sql_YDD, inputall);
        if ((bool)htreturn["return_float"])
        {
            if ((int)htreturn["return_other"] == 1)
            {
                strreturn = "成功";
            }
            else
            {
                strreturn = htreturn["return_other"].ToString()+"条语句被执行";
            }
            return strreturn;
        }
        else
        {
            strreturn = htreturn["return_errmsg"].ToString();
            return strreturn;
        }

    }


    /// <summary>
    /// 修改预订单（本方法主要用于返回撤销后的订单信息，以及已撤销的订单的商品的最新信息） wyh 2014.05.27 
    /// </summary>
    /// <param name="Number">已经撤销的预订单号</param>
    /// <returns>返回符合dsreturn结构的dataset,包含有表明为SPXX的datatable</returns>
    public DataSet Ydd_Change( string Number)
    {
        TBD tbd = new TBD();
        DataSet dsreturn = tbd.initReturnDataSet();

        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = null;//new DataSet();
        Hashtable input = new Hashtable();
        input["@Number"] = Number;
        try
        {
            string sql = "SELECT *,(SELECT MJDJBL FROM AAA_PTDTCSSDB) as MJDJBL,CAST((NMRJG*(NDGSL)) AS NUMERIC(18,2)) AS NEWNDGJE,CAST((NMRJG*(NDGSL)*(SELECT MJDJBL FROM AAA_PTDTCSSDB)) AS NUMERIC(18,2)) AS NEWDJDJ FROM AAA_YDDXXB LEFT JOIN (SELECT AAA_PTSPXXB.Number, AAA_PTSPXXB.SPBH, AAA_PTSPXXB.SPMC, AAA_PTSPXXB.GG, AAA_PTSPXXB.JJDW, AAA_PTSPXXB.JJPL, ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND (SELECT HTQX FROM AAA_YDDXXB WHERE Number=@Number)=AAA_TBD.HTQX ORDER BY TBJG ASC),0) AS ZDJG, ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=AAA_PTSPXXB.SPBH AND ZT='竞标' AND (SELECT HTQX FROM AAA_YDDXXB WHERE Number=@Number)=AAA_TBD.HTQX ORDER BY MJSDJJPL DESC) ,0) AS MJJJPL,AAA_PTSPXXB.SCZZYQ  FROM AAA_PTSPXXB  ) AS AAA_SP ON AAA_YDDXXB.SPBH=AAA_SP.SPBH WHERE AAA_YDDXXB.Number=@Number";

             Hashtable return_ht=  I_DBL.RunParam_SQL(sql, "yddinfo", input);
             if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                //这里就可以用了。
                DataTable dt = null;
                dsGXlist = (DataSet)(return_ht["return_ds"]);
                if (dsGXlist != null && dsGXlist.Tables["yddinfo"].Rows.Count > 0)
                {
                    dt = dsGXlist.Tables["yddinfo"].Copy();
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

    /// <summary>
    /// 撤销预订单
    /// </summary>
    /// <param name="dsreturn">要返回的DataSet</param>
    /// <param name="Number">预订单号</param>
    /// <returns>dsreturn</returns>
    public DataSet Ydd_CX( string Number)
    {
        TBD tdb = new TBD();

        DataSet dsreturn = tdb.initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });

        DataTable dt = null;
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = null;//new DataSet();
        Hashtable input = new Hashtable();
        input["@Number"] = Number;
        string sql = "SELECT * FROM AAA_YDDXXB WHERE Number=@Number";
        Hashtable return_htzkls = I_DBL.RunParam_SQL(sql, "yddinfo", input);

        if ((bool)(return_htzkls["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_htzkls["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["yddinfo"].Rows.Count > 0)
            {
                dt = dsGXlist.Tables["yddinfo"];
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有查找到单号为：" + Number + "的预订单。";
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

        if (dt.Rows.Count > 0)
        {
            #region 验证们
            if (dt.Rows[0]["ZT"].ToString() == "中标")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的预订单已中标，无法撤销！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
            if (dt.Rows[0]["ZT"].ToString() == "撤销")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的预订单已撤销，无法重复撤销！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                return dsreturn;
            }
            #endregion

            string YZBSL = dt.Rows[0]["YZBSL"].ToString().Trim();//已中标数量
            Hashtable htall = null;
            ArrayList list = null;//要执行的SQL语句
            DateTime Time = DateTime.Now;
            //如果没有出现拆单的情况
            if (YZBSL == "0")
            {
                list = new ArrayList();
                htall = new Hashtable();
                #region 更改预订单信息
                htall["@Number"] = Number;
                string sql_NGZB = "UPDATE AAA_YDDXXB SET ZT='撤销' ,NDGJE=((NDGSL-YZBSL)*NMRJG) WHERE Number=@Number";//把拟订购数量更新为与中标数量相同的量，把状态改为中标
                #endregion
                list.Add(sql_NGZB);

                #region 生成流水信息
                DataTable dt_LSGZ = tdb.GetMoneydbzDS("1304000019").Tables["DZB"];

                htall["@Number_zkls"] = "999999999"; // wyh 2014.05.27 -------------获取主键已经移植到用户控制中心

                object[] re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//--------------------
                if (re[0].ToString() == "ok")
                {
                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                    if (re[1] != null && !re[1].ToString().Trim().Equals(""))
                    {
                        htall["@Number_zkls"] = (string)(re[1]);
                    }
                    else
                    {
                        
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取账款流水主键出错";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                        return dsreturn;
                    }
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
                  
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取账款流水主键错误：" + re[1].ToString();
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    return dsreturn;
                }

                htall["@DLYX"] = dt.Rows[0]["DLYX"].ToString();
                htall["@JSZHLX"] = dt.Rows[0]["JSZHLX"].ToString();
                htall["@JSBH"] = dt.Rows[0]["MJJSBH"].ToString();
                htall["@LYYWLX"] = "AAA_YDDXXB";
                htall["@LYDH"] = Number;
                htall["@LSCSSJ"] = Time;
                htall["@YSLX"] = dt_LSGZ.Rows[0]["YSLX"].ToString();
                htall["@JE"] = dt.Rows[0]["DJDJ"].ToString();
                htall["@XM"] = dt_LSGZ.Rows[0]["XM"].ToString();
                htall["@XZ"] = dt_LSGZ.Rows[0]["XZ"].ToString();
                htall["@ZY"] = dt_LSGZ.Rows[0]["ZY"].ToString().Replace("[x1]", Number);
                htall["@JKBH"] = "交易指令中心预订单撤销";
                htall["@QTBZ"] = Number + "预订单撤销（全单）";
                htall["@SJLX"] = dt_LSGZ.Rows[0]["SJLX"].ToString();
                string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES(@Number_zkls,@DLYX,@JSZHLX,@JSBH,@LYYWLX,@LYDH,@LSCSSJ,@YSLX,@JE,@XM,@XZ,@ZY,@JKBH,@QTBZ,@SJLX)";//插入流水的表
                #endregion
                list.Add(sql_LS);

                #region 更新用户余额
                string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+@JE WHERE B_DLYX=@DLYX ";
                #endregion
                list.Add(sql_YE);


                string sqlState = "SELECT Number FROM AAA_YDDXXB WHERE Number=@Number AND ZT='撤销' ";
                return_htzkls = I_DBL.RunParam_SQL(sqlState, "yddinfo", input);
                dsGXlist = null;
                if ((bool)(return_htzkls["return_float"])) //说明执行完成
                {
                    //这里就可以用了。
                    dsGXlist = (DataSet)(return_htzkls["return_ds"]);
                    if (dsGXlist != null && dsGXlist.Tables["yddinfo"].Rows.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "预订单已被撤销，请不要多次操作！";
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
            }
            else//出现了拆单的情况
            {
                list = new ArrayList();
                htall = new Hashtable();
                #region 更改预订单信息
                htall["@Number"] = Number;
                string sql_NGZB = "UPDATE AAA_YDDXXB SET NDGSL=YZBSL,ZT='中标' WHERE Number=@Number";//把拟订购数量更新为与中标数量相同的量，把状态改为中标
                #endregion
                list.Add(sql_NGZB);

                #region 生成流水信息

                DataTable dt_LSGZ = tdb.GetMoneydbzDS("1304000019").Tables["DZB"];
                htall["@Number_zkls"] = "99999999";//wyh 2014.05.27--------------------获取主键已经移植到系统控制中心

                object[] re = IPC.Call("获取主键", new object[] { "AAA_ZKLSMXB", "" });//--------------------
                if (re[0].ToString() == "ok")
                {
                    //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                    if (re[1] != null && !re[1].ToString().Trim().Equals(""))
                    {
                        htall["@Number_zkls"] = (string)(re[1]);
                    }
                    else
                    {

                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取账款流水主键出错";
                        dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                        return dsreturn;
                    }
                }
                else
                {
                    //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。

                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取账款流水主键错误：" + re[1].ToString();
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                    return dsreturn;
                }


                htall["@DLYX"] = dt.Rows[0]["DLYX"].ToString();
                htall["@JSZHLX"] = dt.Rows[0]["JSZHLX"].ToString();
                htall["@JSBH"] = dt.Rows[0]["MJJSBH"].ToString();
                htall["@LYYWLX"] = "AAA_YDDXXB";
                htall["@LYDH"] = Number;
                htall["@LSCSSJ"] = Time;
                htall["@YSLX"] = dt_LSGZ.Rows[0]["YSLX"].ToString();
                htall["@JE"] = dt.Rows[0]["DJDJ"].ToString();
                htall["@XM"] = dt_LSGZ.Rows[0]["XM"].ToString();
                htall["@XZ"] = dt_LSGZ.Rows[0]["XZ"].ToString();
                htall["@ZY"] = dt_LSGZ.Rows[0]["ZY"].ToString().Replace("[x1]", Number);
                htall["@JKBH"] = "交易指令中心预订单撤销";
                htall["@QTBZ"] = Number + "预订单撤销，原拟购量：" + dt.Rows[0]["NDGSL"].ToString();
                htall["@SJLX"] = dt_LSGZ.Rows[0]["SJLX"].ToString();
                string sql_LS = "INSERT INTO AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) VALUES(@Number_zkls,@DLYX,@JSZHLX,@JSBH,@LYYWLX,@LYDH,@LSCSSJ,@YSLX,@JE,@XM,@XZ,@ZY,@JKBH,@QTBZ,@SJLX)";

                #endregion
                list.Add(sql_LS);

                #region 更新用户余额
                string sql_YE = "UPDATE AAA_DLZHXXB SET B_ZHDQKYYE=B_ZHDQKYYE+@JE WHERE B_DLYX=@DLYX";
                #endregion
                list.Add(sql_YE);

                string sqlState = "SELECT * FROM AAA_YDDXXB WHERE Number=@Number AND ZT='中标' ";
                return_htzkls = I_DBL.RunParam_SQL(sqlState, "yddzb", input);
                dsGXlist = null;
                if ((bool)(return_htzkls["return_float"])) //说明执行完成
                {
                    //这里就可以用了。
                    dsGXlist = (DataSet)(return_htzkls["return_ds"]);
                    if (dsGXlist != null && dsGXlist.Tables["yddzb"].Rows.Count > 0)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "预订单已中标，无法撤销！";
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

            }

            return_htzkls = I_DBL.RunParam_SQL(list, htall);
            if ((bool)(return_htzkls["return_float"])) //说明执行完成
            {
                if ((int)return_htzkls["return_other"] > 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "预订单撤销成功！";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = Number;
                    if (dt.Rows[0]["HTQX"].ToString() == "即时")
                    {
                        //

                    }
                    else
                    {

                        DataTable dtJK = new DataTable("dtjk");
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
                            I_DBL.RunParam_SQL(aljk, htcs);
                        }
                    }


                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "预订单撤销失败！";
                    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
                }
            }
            else
            {
                //读取数据库过程中发生程序错误了。。。
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = return_htzkls["return_errmsg"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";

            } //?


        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到需要撤销的预订单";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "";
        }

       
        return dsreturn;
    }

    /// <summary>
    /// 得到预订单的信息电子购货合同使用  wyh 2014.07.14 
    /// </summary>
    /// <param name="dsreturn">返回值</param>
    /// <param name="dataTable">传递的参数</param>
    /// <returns></returns>
    public DataSet Get_YDDData( DataTable dataTable)
    {
        TBD tdb = new TBD();

        DataSet dsreturn = tdb.initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "" });
        //?wyh 2014.05.21 add
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        input.Add("@Number", dataTable.Rows[0]["中标定标_Number"].ToString());
        string strSql = "select YY.Number '预订单号',YY.TJSJ '下单时间',DD.I_JYFMC '买方名称',YY.SPMC '拟买入商品名称',YY.GG '规格',YY.JJDW '计价单位',YY.NDGSL '拟订购数量',YY.NMRJG '拟买入价格',YY.NDGJE '拟订购金额',YY.DJDJ '冻结的订金',YY.HTQX '合同期限',YY.SHQY '收货区域' from AAA_ZBDBXXB as ZZ left join AAA_YDDXXB as YY on ZZ.Y_YSYDDBH=YY.Number left join AAA_DLZHXXB as DD on YY.MJJSBH=DD.J_BUYJSBH where ZZ.Number=@Number";
        //string strNumber = dataTable.Rows[0]["中标定标_Number"].ToString();//中标定标信息表
        DataTable dataTableYDD = null; //DbHelperSQL.Query("select YY.Number '预订单号',YY.TJSJ '下单时间',DD.I_JYFMC '买方名称',YY.SPMC '拟买入商品名称',YY.GG '规格',YY.JJDW '计价单位',YY.NDGSL '拟订购数量',YY.NMRJG '拟买入价格',YY.NDGJE '拟订购金额',YY.DJDJ '冻结的订金',YY.HTQX '合同期限',YY.SHQY '收货区域' from AAA_ZBDBXXB as ZZ left join AAA_YDDXXB as YY on ZZ.Y_YSYDDBH=YY.Number left join AAA_DLZHXXB as DD on YY.MJJSBH=DD.J_BUYJSBH where ZZ.Number='" + strNumber + "'").Tables[0];
        Hashtable return_ht = I_DBL.RunParam_SQL(strSql, "o", input);
        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            if (dsGXlist != null && dsGXlist.Tables["o"].Rows.Count > 0)
            {
                dataTableYDD = dsGXlist.Tables["o"].Copy();
                dataTableYDD.TableName = "预订单";
                dsreturn.Tables.Add(dataTableYDD);
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
    /// 获得预订单草稿信息获得 wyh 2014.07.14 
    /// </summary>
    /// <param name="Number">已经预订单草稿单号</param>
    /// <returns></returns>
    public DataSet YDD_Getcaogao( string Number)
    {
        TBD tdb = new TBD();

        DataSet dsreturn = tdb.initReturnDataSet().Clone();
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
            string sql = "SELECT a.Number,DLYX,a.SPBH,HTQX,SHQY,SHQYsheng,SHQYshi,NMRJG,NDGSL,NDGJE,b.SPMC,b.GG,'0.00' as NEWDJDJ,b.JJDW,(ISNULL((SELECT TOP 1 TBJG FROM AAA_TBD WHERE AAA_TBD.SPBH=a.SPBH and AAA_TBD.HTQX=a.HTQX  AND ZT='竞标' ORDER BY TBJG ASC),0)) as ZDJG,( select JJPL from AAA_PTSPXXB where SPBH=a.SPBH )as JJPL,(SELECT TOP 1 SCZZYQ FROM AAA_TBD WHERE AAA_TBD.SPBH=a.SPBH ) as SCZZYQ,(ISNULL((SELECT TOP 1 MJSDJJPL FROM AAA_TBD WHERE AAA_TBD.SPBH=a.SPBH AND ZT='竞标' and AAA_TBD.HTQX = a.HTQX ORDER BY MJSDJJPL DESC) ,0)) as MJJJPL,YXJZRQ   FROM AAA_YDDCGB as a left join AAA_PTSPXXB as b on a.SPBH=b.SPBH  WHERE a.Number =@Number";
            DataSet ds = null;//DbHelperSQL.Query(sql);
            Hashtable  return_ht = I_DBL.RunParam_SQL(sql, "CG", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            //这里就可以用了。
            ds = (DataSet)(return_ht["return_ds"]);
                  
        }
     
            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {

                ////判断商品是否有效
                //string stryx = "";
                //object[] re = IPC.Call("商品有效", new object[] { ds.Tables[0].Rows[0]["SPBH"].ToString() });

                //if (re[0].ToString() == "ok")
                //{
                //    stryx = (string)(re[1]);
                //    if (stryx == null || stryx.Equals(""))
                //    {
                //        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无法查到该草稿商品的信息，无法再下达此商品的预订单！";
                //        return dsreturn;
                //    }
                //}
                //else
                //{
                //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "无法查到该草稿商品的信息，无法再下达此商品的预订单！";
                //    return dsreturn;
                //}

                //if (!stryx.Equals("是"))
                //{
                //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您拟买入商品刚刚已下线，该申请单无法提交。请选择其他可拟买入商品。";
                //    return dsreturn;
                //}



                DataTable dt = ds.Tables[0];
                DataTable dtRt = dt.Copy();
                dtRt.TableName = "SPXX";
                dsreturn.Tables.Add(dtRt);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "单号为：" + Number + "的预订单草稿信息查找错误！";
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