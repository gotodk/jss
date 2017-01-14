using FMOP.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Jhjx_hwqs 的摘要说明
/// </summary>
public class Jhjx_hwqs
{
    ClassMoney2013 cm = null;
    Hashtable htt = null;
    
	public Jhjx_hwqs()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public DataSet dsjhjx_hwqs(DataSet dsreturn, DataTable ht)
    {
        DataRow dr = ht.Rows[0];
      
        htt = PublicClass2013.GetParameterInfo();
        Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
        if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
            if (dr["islegal"] != null && dr["islegal"].ToString().Trim().Equals("yes"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
            }
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
            if (dr["islegal"] != null && dr["islegal"].ToString().Trim().Equals("yes"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
            }
            return dsreturn;
        }
        if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            if (dr["islegal"] != null && dr["islegal"].ToString().Trim().Equals("yes"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
            }
            return dsreturn;
        }
         //cm = new ClassMoney2013();
         Hashtable htresul = null;


        Hashtable hsuser = PublicClass2013.GetUserInfo(dr["买家角色编号"].ToString());
        //验证是开通交易账户
        if (hsuser["交易账户是否开通"].ToString().Trim() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            if (dr["ispass"] != null && dr["ispass"].ToString().Trim().Equals("yes"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
            }
            return dsreturn;
        }

 

        //验证是否休眠

        if (hsuser["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
 

            return dsreturn;
        }

        string Type = dr["type"].ToString();
        switch (Type)
        {
            case "无异议收货":
            case "补发货物无异议收货":
            case "有异议收货后无异议收货":
                htresul = wyysh_strs(dr);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htresul["执行结果"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htresul["提示文本"].ToString();

                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] ="wyy";
                break;
            case "请重新发货":
                htresul = qcxsh_strs(dr);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htresul["执行结果"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htresul["提示文本"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
                break;
            case "部分收货":
                htresul = bufensh_strs(dr);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htresul["执行结果"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htresul["提示文本"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
                break;
            case "有异议收货":
                htresul = wyyth_strs(dr);
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = htresul["执行结果"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htresul["提示文本"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "nowyy";
                break;
          
            default:
                break;
        }
        


        return dsreturn;
       
    }

    //无疑义提货操作
    private Hashtable wyysh_strs(DataRow drs)
    {
        try
        {
            Hashtable Htwyy = new Hashtable();
            Htwyy["执行结果"] = "err";
            Htwyy["提示文本"] = "无异议收货没有执行任何操作";
           
            string action = drs["ispass"].ToString().Trim();
            ArrayList al = new ArrayList();

            //获得当前提货单据详细信息
            string Strgetinfo = "select a.Number,('T'+a.Number) as  提货单号,('F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖家名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买家角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.T_YSTBDMJJSBH as 卖家角色编号,b.T_YSTBDDLYX as 卖家登陆邮箱,b.T_YSTBDJSZHLX as 卖家角色账户类型,b.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,b.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,b.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,b.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,b.Z_ZBSL as 定标数量,b.Z_LYBZJJE as 履约保证金金额,b.Z_HTZT as 合同状态,'买家名称'=(select I_JYFMC from AAA_DLZHXXB DL where DL.J_BUYJSBH=b.Y_YSYDDMJJSBH),b.Y_YSYDDJSZHLX  买家结算账户类型,'买家关联经纪人名称'=(select I_JYFMC from AAA_DLZHXXB a where a.J_JJRJSBH=b.Y_YSYDDGLJJRJSBH),'买家关联经纪人结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB a where a.J_JJRJSBH=b.Y_YSYDDGLJJRJSBH),'卖家关联经纪人名称'=(select I_JYFMC from AAA_DLZHXXB a where a.J_JJRJSBH=b.T_YSTBDGLJJRJSBH),'卖家关联经纪人结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB a where a.J_JJRJSBH=b.T_YSTBDGLJJRJSBH) from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX  where a.Number='" + drs["Number"].ToString().Trim() + "' and  (a.F_DQZT ='已录入发货信息' or a.F_DQZT ='已录入补发备注' or a.F_DQZT ='有异议收货') and b.Y_YSYDDMJJSBH='" + drs["买家角色编号"].ToString() + "' ";
            DataSet dsgetinfo = DbHelperSQL.Query(Strgetinfo);
            if (dsgetinfo != null && dsgetinfo.Tables[0].Rows.Count == 1)
            {

                DataRow drinfo = dsgetinfo.Tables[0].Rows[0];
                if (!drinfo["合同状态"].ToString().Trim().Equals("定标"))
                {
                    Htwyy["执行结果"] = "err";
                    Htwyy["提示文本"] = "未从登陆信息表中查找到任何关于" + drs["登陆邮箱"].ToString().Trim() + "的信息！";
                    return Htwyy;
                }
                if (action.Equals("ok"))
                {
                    

                    #region 获取卖家卖家账户当前余额
                    //获得当前卖家卖家余额
                    string Strbuyyue = " select B_ZHDQKYYE  from AAA_DLZHXXB where B_DLYX = '" + drs["登陆邮箱"].ToString().Trim() + "'  ";
                    object objbuy = DbHelperSQL.GetSingle(Strbuyyue);
                    double buyyue = 0.00;
                    double sellyue = 0.00;
                    if (objbuy != null && objbuy.ToString().Trim() != "")
                    {
                        buyyue = Convert.ToDouble(objbuy.ToString().Trim());
                    }
                    else
                    {
                        Htwyy["执行结果"] = "err";
                        Htwyy["提示文本"] = "未从登陆信息表中查找到任何关于" + drs["登陆邮箱"].ToString().Trim() + "的信息！";
                        return Htwyy;
                    }
                    string Strsellyue = " select B_ZHDQKYYE  from AAA_DLZHXXB where B_DLYX = '" + drinfo["卖家登陆邮箱"].ToString().Trim() + "'  ";
                    object objsell = DbHelperSQL.GetSingle(Strsellyue);
                    if (objsell != null && objsell.ToString().Trim() != "")
                    {
                        sellyue = Convert.ToDouble(objsell.ToString().Trim());
                    }
                    else
                    {
                        Htwyy["执行结果"] = "err";
                        Htwyy["提示文本"] = "未从登陆信息表中查找到任何关于" + drinfo["卖家登陆邮箱"].ToString().Trim() + "的信息！";
                        return Htwyy;
                    }
                    #endregion
                    DateTime drnow = DateTime.Now;
                    DataRow drhkjd = null;
                    string Keynumber = "";
                    #region 货物签收解冻买家货款
                    //货物签收解冻买家货款
                    string Strtb = " select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000010'";
                    DataSet dshkjd = DbHelperSQL.Query(Strtb);
                    if (dshkjd != null && dshkjd.Tables[0].Rows.Count == 1)
                    {
                        drhkjd = dshkjd.Tables[0].Rows[0];
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());

                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        //货物签收解冻买家货款sql
                        string strinsehwqshkjd = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drs["登陆邮箱"].ToString() + "','" + drs["结算账户类型"].ToString() + "','" + drs["买家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd["YSLX"].ToString() + "','" + drinfo["发货金额"].ToString().Trim() + "','" + drhkjd["XM"].ToString() + "','" + drhkjd["XZ"].ToString() + "','" + drhkjd["ZY"].ToString() + "','" + drhkjd["SJLX"].ToString() + "','" + drs["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";

                        al.Add(strinsehwqshkjd);
                        buyyue += Convert.ToDouble(drinfo["发货金额"].ToString().Trim());
                    }
                    else
                    {
                        Htwyy["执行结果"] = "err";
                        Htwyy["提示文本"] = "为从资金表动明细表中查找到任何关于货物签收无疑义解冻货款的信息！";
                        return Htwyy;
                    }
                    #endregion


                    #region 签收后扣减货款
                    //签收后扣减货款
                    string Strtbkjhk = " select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000012'";
                    DataSet dskjhk = DbHelperSQL.Query(Strtbkjhk);
                    if (dskjhk != null && dskjhk.Tables[0].Rows.Count == 1)
                    {

                        drhkjd = dskjhk.Tables[0].Rows[0];
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());

                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        //签收后扣减货款sql
                        string strinsehwqshkjd = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drs["登陆邮箱"].ToString() + "','" + drs["结算账户类型"].ToString() + "','" + drs["买家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd["YSLX"].ToString() + "','" + drinfo["发货金额"].ToString().Trim() + "','" + drhkjd["XM"].ToString() + "','" + drhkjd["XZ"].ToString() + "','" + drhkjd["ZY"].ToString() + "','" + drhkjd["SJLX"].ToString() + "','" + drs["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";

                        al.Add(strinsehwqshkjd);
                        buyyue -= Convert.ToDouble(drinfo["发货金额"].ToString().Trim());
                    }
                    else
                    {
                        Htwyy["执行结果"] = "err";
                        Htwyy["提示文本"] = "为从资金表动明细表中查找到任何关于货物签收无疑义签收扣减货款的信息！";
                        return Htwyy;

                    }
                    #endregion

                    #region 向卖家支付货款
                    //向卖家支付货款
                    string Strzfhk = " select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000014'";
                    DataSet dszfhk = DbHelperSQL.Query(Strzfhk);
                    if (dszfhk != null && dszfhk.Tables[0].Rows.Count == 1)
                    {
                        drhkjd = dszfhk.Tables[0].Rows[0];
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());
                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();
                        //向卖家支付货款sql
                        string strinsehwqszfhk = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd["YSLX"].ToString() + "','" + drinfo["发货金额"].ToString().Trim() + "','" + drhkjd["XM"].ToString() + "','" + drhkjd["XZ"].ToString() + "','" + drhkjd["ZY"].ToString() + "','" + drhkjd["SJLX"].ToString() + "','" + drs["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                        al.Add(strinsehwqszfhk);
                        sellyue += Convert.ToDouble(drinfo["发货金额"].ToString().Trim());

                    }
                    else
                    {
                        Htwyy["执行结果"] = "err";
                        Htwyy["提示文本"] = "未从资金表动明细表中查找到任何关于货物签收无疑义签收向卖方支付货款的信息！";
                        return Htwyy;
                    }

                    #endregion

                    #region 扣减卖家技术服务费
                    //扣减卖家技术服务费 

                    double jsfwufei = Convert.ToDouble((Convert.ToDouble(drinfo["发货金额"].ToString().Trim()) * Convert.ToDouble(htt["签收技术服务费比率"].ToString().Trim())).ToString("#0.00"));

                    string Strjsfwf = " select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000007'";
                    DataSet dsjsfws = DbHelperSQL.Query(Strjsfwf);
                    if (dsjsfws != null && dsjsfws.Tables[0].Rows.Count > 0)
                    {

                        drhkjd = dsjsfws.Tables[0].Rows[0];
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());
                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();
                        //扣减卖家技术服务费sql
                        string strinsehwqszfhk = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd["YSLX"].ToString() + "','" + jsfwufei.ToString() + "','" + drhkjd["XM"].ToString() + "','" + drhkjd["XZ"].ToString() + "','" + drhkjd["ZY"].ToString() + "','" + drhkjd["SJLX"].ToString() + "','" + drs["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                        al.Add(strinsehwqszfhk);
                        sellyue -= jsfwufei;

                    }
                    else
                    {
                        Htwyy["执行结果"] = "err";
                        Htwyy["提示文本"] = "未从资金表动明细表中查找到任何关于货物签收无疑义签收扣减技术服务费的基本信息！";
                        return Htwyy;
                    }
                    #endregion

                    #region  计算经纪人收益
                    //来自买家的经纪人收益
                    string buyjjrsy = "";
                    string Strjjrsyfbuy = "select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000043'";
                    DataSet dsjjrsybuy = DbHelperSQL.Query(Strjjrsyfbuy);
                    if (dsjjrsybuy != null && dsjjrsybuy.Tables[0].Rows.Count == 1)
                    {
                        drhkjd = dsjjrsybuy.Tables[0].Rows[0];
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x2]", drinfo["发货单号"].ToString().Trim());
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drs["登陆邮箱"].ToString().Trim());
                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家关联经纪人角色编号"].ToString())["结算账户类型"].ToString();

                        buyjjrsy = (jsfwufei * Convert.ToDouble(htt["经纪人收益买家比率"].ToString().Trim())).ToString("#0.00");


                        //来自买家的经纪人收益sql
                        string strinsehwqsbuyjjrsy = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["买家关联经纪人邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["买家关联经纪人角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd["YSLX"].ToString() + "','" + buyjjrsy.ToString() + "','" + drhkjd["XM"].ToString() + "','" + drhkjd["XZ"].ToString() + "','" + drhkjd["ZY"].ToString() + "','" + drhkjd["SJLX"].ToString() + "','" + drs["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                        al.Add(strinsehwqsbuyjjrsy);

                    }
                    else
                    {
                        Htwyy["执行结果"] = "err";
                        Htwyy["提示文本"] = "未从资金表动明细表中查找到任何关于来自买方的经纪人收益的基本信息！";
                        return Htwyy;
                    }


                    //来自卖家的经纪人收益
                    string buyjjrsell = "";
                    string Strjjrsyfsell = "select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000042'";
                    DataSet dsjjrsysell = DbHelperSQL.Query(Strjjrsyfsell);
                    if (dsjjrsysell != null && dsjjrsysell.Tables[0].Rows.Count == 1)
                    {
                        drhkjd = dsjjrsysell.Tables[0].Rows[0];
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x2]", drinfo["发货单号"].ToString().Trim());
                        drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["卖家登陆邮箱"].ToString().Trim());
                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家关联经纪人角色编号"].ToString())["结算账户类型"].ToString();

                        buyjjrsell = (jsfwufei * Convert.ToDouble(htt["经纪人收益卖家比率"].ToString().Trim())).ToString("#0.00");


                        //来自卖家的经纪人收益sql
                        string strinsehwqsselljjrsy = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家关联经纪人邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家关联经纪人角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd["YSLX"].ToString() + "','" + buyjjrsell.ToString() + "','" + drhkjd["XM"].ToString() + "','" + drhkjd["XZ"].ToString() + "','" + drhkjd["ZY"].ToString() + "','" + drhkjd["SJLX"].ToString() + "','" + drs["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                        al.Add(strinsehwqsselljjrsy);

                    }
                    else
                    {
                        Htwyy["执行结果"] = "err";
                        Htwyy["提示文本"] = "未从资金表动明细表中查找到任何关于来自卖方的经纪人收益的基本信息！";
                        return Htwyy;
                    }

                    #endregion

                    #region 满足买家无异议签收数量=定标数量是单据处理
                    int zbNum = Convert.ToInt32(drinfo["定标数量"].ToString().Trim());
                    int thbc = Convert.ToInt32(drinfo["提货数量"].ToString().Trim());
                    int ythNum = 0;
                    string StrgetyithNum = "select  isnull(SUM (ISNULL(T_THSL,0)),0) as  已提货数量,ZBDBXXBBH     from AAA_THDYFHDXXB where ZBDBXXBBH = '" + drinfo["中标定标单号"].ToString() + "' and F_DQZT in ( '无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货','卖家主动退货' ) group by ZBDBXXBBH";
                    DataSet dsyth = DbHelperSQL.Query(StrgetyithNum);
                    if (dsyth != null && dsyth.Tables[0].Rows.Count > 0)
                    {
                        ythNum = Convert.ToInt32(dsyth.Tables[0].Rows[0]["已提货数量"].ToString());
                    }
                                      
                    if (zbNum == thbc + ythNum)
                    {
                        #region 解冻卖家履约保证金
                        //解冻卖家履约保证金
                        double lvbzj = Convert.ToDouble(drinfo["履约保证金金额"].ToString().Trim());
                        //无异议提货解冻卖家履约保证金
                        string Strwyythjdlybzj = "select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000048'";
                        DataSet dswyyjelybzj = DbHelperSQL.Query(Strwyythjdlybzj);
                        if (dswyyjelybzj != null && dswyyjelybzj.Tables[0].Rows.Count > 0)
                        {
                            drhkjd = dswyyjelybzj.Tables[0].Rows[0];

                            drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                            Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                            string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                            //无异议提货解冻卖家履约保证金sql
                            string strinsehwqsjdmjlybzj = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_ZBDBXXB','" + drinfo["中标定标单号"].ToString() + "','" + drnow + "','" + drhkjd["YSLX"].ToString() + "','" + drinfo["履约保证金金额"].ToString() + "','" + drhkjd["XM"].ToString() + "','" + drhkjd["XZ"].ToString() + "','" + drhkjd["ZY"].ToString() + "','" + drhkjd["SJLX"].ToString() + "','" + drs["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                            al.Add(strinsehwqsjdmjlybzj);
                            sellyue += Convert.ToDouble(drinfo["履约保证金金额"].ToString());
                        }
                        else
                        {
                            Htwyy["执行结果"] = "err";
                            Htwyy["提示文本"] = "无异议提货解冻卖方履约保证金信息未找到！";
                            return Htwyy;
                        }
                        #endregion

                        #region 因发票延期或者发货延期的罚单与补偿
                        //获得所有相同中标定标单号的提货单编号的发货单号

                        string GetTHdNumber = " select Number from  AAA_THDYFHDXXB where ZBDBXXBBH = '" + drinfo["中标定标单号"].ToString() + "' ";
                        DataSet dsfpyq = DbHelperSQL.Query(GetTHdNumber);
                        if (dsfpyq != null && dsfpyq.Tables[0].Rows.Count > 0)
                        {
                            string Strwhere = "";
                            foreach (DataRow dr in dsfpyq.Tables[0].Rows)
                            {
                                Strwhere += "'" + dr["Number"].ToString().Trim() + "',";
                            }

                            Strwhere = Strwhere.Trim().Remove(Strwhere.Length - 1);

                            # region 发票延期的扣罚与补赏
                            //获取次投标定标预订单的发票延迟预扣款流水明细
                            double je = 0.00;
                            string Strfpykmc = "select Number, JE from AAA_ZKLSMXB where XM = '违约赔偿金' and XZ = '发货单生成后5日内未录入发票邮寄信息' and LYDH in (" + Strwhere + ") and SJLX='预'";
                            DataSet dsfpykmc = DbHelperSQL.Query(Strfpykmc);
                            if (dsfpykmc != null && dsfpykmc.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow drfpykk in dsfpykmc.Tables[0].Rows)
                                {
                                    je += Convert.ToDouble(drfpykk["JE"].ToString().Trim());
                                }

                                //卖家扣罚(发货单生成后5日内未录入发票邮寄信息)

                                string Strfpyqmjkf = "select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000028'";
                                DataSet dsStrfpyqmjkf = DbHelperSQL.Query(Strfpyqmjkf);
                                if (dsStrfpyqmjkf != null && dsStrfpyqmjkf.Tables[0].Rows.Count > 0)
                                {
                                    drhkjd = dsStrfpyqmjkf.Tables[0].Rows[0];

                                    drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_ZBDBXXB','" + drinfo["中标定标单号"].ToString() + "','" + drnow + "','" + drhkjd["YSLX"].ToString() + "','" + je.ToString() + "','" + drhkjd["XM"].ToString() + "','" + drhkjd["XZ"].ToString() + "','" + drhkjd["ZY"].ToString() + "','" + drhkjd["SJLX"].ToString() + "','" + drs["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                                    al.Add(strinsehwqsfpyqkf);
                                    sellyue -= je;
                                }
                                else
                                {
                                    Htwyy["执行结果"] = "err";
                                    Htwyy["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                    return Htwyy;
                                }

                                //买家补赏(发货单生成后5日内未录入发票邮寄信息)

                                string Strfpyqmjbs = "select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000037'";
                                DataSet dsStrfpyqmjbs = DbHelperSQL.Query(Strfpyqmjbs);
                                if (dsStrfpyqmjbs != null && dsStrfpyqmjbs.Tables[0].Rows.Count > 0)
                                {
                                    drhkjd = dsStrfpyqmjbs.Tables[0].Rows[0];

                                    drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString())["结算账户类型"].ToString();

                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drs["登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["买家角色编号"].ToString() + "','AAA_ZBDBXXB','" + drinfo["中标定标单号"].ToString() + "','" + drnow + "','" + drhkjd["YSLX"].ToString() + "','" + je.ToString() + "','" + drhkjd["XM"].ToString() + "','" + drhkjd["XZ"].ToString() + "','" + drhkjd["ZY"].ToString() + "','" + drhkjd["SJLX"].ToString() + "','" + drs["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                                    al.Add(strinsehwqsfpyqkf);
                                    buyyue += je;
                                }
                                else
                                {
                                    Htwyy["执行结果"] = "err";
                                    Htwyy["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                    return Htwyy;
                                }



                            }
                            #endregion

                            #region  卖家发货延期的扣罚与补偿
                            //获取次投标定标预订单的发货延迟预扣款流水明细
                            double hwje = 0.00;
                            string Strhwykmc = "select Number, JE from AAA_ZKLSMXB where XM = '违约赔偿金' and XZ = '超过最迟发货日后录入发货信息' and LYDH in (" + Strwhere + ") and SJLX='预'";
                            DataSet dshwykmc = DbHelperSQL.Query(Strhwykmc);
                            if (dshwykmc != null && dshwykmc.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow drfpykk in dshwykmc.Tables[0].Rows)
                                {
                                    hwje += Convert.ToDouble(drfpykk["JE"].ToString().Trim());
                                }

                                //卖家扣罚(发货单生成后5日内未录入发货信息)

                                string Strfhyqmjkf = "select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000026'";
                                DataSet dsStrfhyqmjkf = DbHelperSQL.Query(Strfhyqmjkf);
                                if (dsStrfhyqmjkf != null && dsStrfhyqmjkf.Tables[0].Rows.Count > 0)
                                {
                                    drhkjd = dsStrfhyqmjkf.Tables[0].Rows[0];

                                    drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                                    //发货单生成后5日内未录入发货信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_ZBDBXXB','" + drinfo["中标定标单号"].ToString() + "','" + drnow + "','" + drhkjd["YSLX"].ToString() + "','" + hwje.ToString() + "','" + drhkjd["XM"].ToString() + "','" + drhkjd["XZ"].ToString() + "','" + drhkjd["ZY"].ToString() + "','" + drhkjd["SJLX"].ToString() + "','" + drs["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                                    al.Add(strinsehwqsfpyqkf);
                                    sellyue -= hwje;
                                }
                                else
                                {
                                    Htwyy["执行结果"] = "err";
                                    Htwyy["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                    return Htwyy;
                                }

                                //买家补赏(发货单生成后5日内未录入发货信息)收到卖方发货延迟补偿

                                string Strfhyqmjbs = "select  '' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number = '1304000036'";
                                DataSet dsStrfhyqmjbs = DbHelperSQL.Query(Strfhyqmjbs);
                                if (dsStrfhyqmjbs != null && dsStrfhyqmjbs.Tables[0].Rows.Count > 0)
                                {
                                    drhkjd = dsStrfhyqmjbs.Tables[0].Rows[0];

                                    drhkjd["ZY"] = drhkjd["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                                    Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString())["结算账户类型"].ToString();

                                    //发货单生成后5日内未录入发货信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drs["登陆邮箱"].ToString() + "','" + jszhlx + "','" + drs["买家角色编号"].ToString() + "','AAA_ZBDBXXB','" + drinfo["中标定标单号"].ToString() + "','" + drnow + "','" + drhkjd["YSLX"].ToString() + "','" + hwje.ToString() + "','" + drhkjd["XM"].ToString() + "','" + drhkjd["XZ"].ToString() + "','" + drhkjd["ZY"].ToString() + "','" + drhkjd["SJLX"].ToString() + "','" + drs["登陆邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
                                    al.Add(strinsehwqsfpyqkf);
                                    buyyue += hwje;
                                }
                                else
                                {
                                    Htwyy["执行结果"] = "err";
                                    Htwyy["提示文本"] = "发货单生成后5日内未录入发票邮寄信息未找到！";
                                    return Htwyy;
                                }



                            }
                            #endregion
                            #region 反写中标定标信息表
                            string Strupdate = " update AAA_ZBDBXXB set Z_HTZT = '定标执行完成' , Z_QPKSSJ='" + drnow + "',Z_QPJSSJ='" + drnow + "' ,Z_QPZT='清盘结束' where Z_HTBH = '" + drinfo["电子购货合同"].ToString() + "' ";
                            al.Add(Strupdate);
                            #endregion

                        }
                        else
                        {
                            Htwyy["执行结果"] = "err";
                            Htwyy["提示文本"] = "未找到中标定标编号为：" + drinfo["中标定标单号"].ToString() + "的发货单！";
                            return Htwyy;

                        }


                        #endregion
                                                
                    }
                    else if (zbNum < thbc + ythNum)
                    {
                        Htwyy["执行结果"] = "err";
                        Htwyy["提示文本"] = "所有无异议收货的数量大于了总定标数量！";
                        return Htwyy;
                    }



                    #endregion

                    //更改账户余额
                    string StrUpbuy = " update   AAA_DLZHXXB set B_ZHDQKYYE ='" + buyyue.ToString("#0.00") + "' where B_DLYX = '" + drs["登陆邮箱"].ToString().Trim() + "'  ";
                    al.Add(StrUpbuy);
                    string Strupsell = " update   AAA_DLZHXXB set B_ZHDQKYYE ='" + sellyue.ToString("#0.00") + "' where B_DLYX = '" + drinfo["卖家登陆邮箱"].ToString().Trim() + "'  ";
                    al.Add(Strupsell);
                    //更改单据状态
                    string Strupda = " update AAA_THDYFHDXXB set F_DQZT='"+drs["type"].ToString()+"' , F_BUYWYYSHCZSJ='" + drnow + "' where Number='" + drs["Number"].ToString() + "' ";
                    al.Add(Strupda);

                    DbHelperSQL.ExecuteSqlTran(al);

                    #region 交易方信用变化情况 edittime 2013-09

                    jhjx_JYFXYMX xymc = new jhjx_JYFXYMX();

                    List<string> jfl = new List<string>();

                    string sqlstr = "select Z_QPZT  from  AAA_ZBDBXXB where  Z_HTBH='" + drinfo["电子购货合同"].ToString().Trim() +"'";
                    object bszt = DbHelperSQL.GetSingle(sqlstr);
                    if (bszt != null && bszt.ToString().Trim() == "清盘结束")
                    {
                        string buyeremail = drs["登陆邮箱"].ToString().Trim();
                        string selleremail = drinfo["卖家登陆邮箱"].ToString().Trim();
                        string buyjjr = drinfo["买家关联经纪人邮箱"].ToString().Trim();
                        string selljjr = drinfo["卖家关联经纪人邮箱"].ToString().Trim();
                        string htnum = drinfo["电子购货合同"].ToString().Trim();

                        string[] buystrs = xymc.ZSZHMR_SFWQLV(buyeremail, htnum, "买家", drs["登陆邮箱"].ToString().Trim());
                        string[] sellstrs = xymc.ZSZHMC_SFWQLV(selleremail, htnum, "卖家", drs["登陆邮箱"].ToString().Trim());
                        string[] bjjrstrs = new string[2];
                        string[] sjjrstrs = new string[2];

                        if (xymc.CanDoing(htnum, buyeremail, "买家"))
                        {
                            bjjrstrs = xymc.GLJYF_WQLY(buyjjr, buyeremail, htnum, "经纪人", drs["登陆邮箱"].ToString().Trim());

                        }
                        else
                        {
                            bjjrstrs = xymc.GLJYF_WWQLY(buyjjr, buyeremail, htnum, "经纪人", drs["登陆邮箱"].ToString().Trim());
 
                        }

                        if (xymc.CanDoing(htnum,selleremail,"卖家"))
                        {
                            sjjrstrs = xymc.GLJYF_WQLY(selljjr, selleremail, htnum, "经纪人", drs["登陆邮箱"].ToString().Trim());
                           
                        }
                        else
                        {
                            sjjrstrs = xymc.GLJYF_WWQLY(selljjr, selleremail, htnum, "经纪人", drs["登陆邮箱"].ToString().Trim());
                           

                        }

                        jfl.Add(buystrs[0]);
                        jfl.Add(buystrs[1]);
                        jfl.Add(sellstrs[0]);
                        jfl.Add(sellstrs[1]);
                        jfl.Add(sjjrstrs[0]);
                        jfl.Add(sjjrstrs[1]);
                        jfl.Add(bjjrstrs[0]);
                        jfl.Add(bjjrstrs[1]);

                    }

                    if (jfl.Count > 0)
                    {
                        DbHelperSQL.ExecuteSqlTran(jfl);
                    }
                    
              

                    #endregion

                    #region//提醒信息
                    string Type = drs["type"].ToString();
                    switch (Type)
                    {
                        case "无异议收货":
                            //您***（编号）发货单，买家已无异议收货，***元的货款已转付至您的交易账户， **元的技术服务费已同时扣收，敬请知悉。
                            List<Hashtable> list = new List<Hashtable>();
                            list = TX(Type,drinfo, drs, sellyue, jsfwufei, drnow, buyjjrsy, buyjjrsell, zbNum, thbc, ythNum);
                            PublicClass2013.Sendmes(list);
                            Htwyy["执行结果"] = "ok";
                            Htwyy["提示文本"] = "无异议收货操作成功！";
                            return Htwyy;
                        case "补发货物无异议收货":
                            list = new List<Hashtable>();
                            list = TX(Type, drinfo, drs, sellyue, jsfwufei, drnow, buyjjrsy, buyjjrsell, zbNum, thbc, ythNum);
                            PublicClass2013.Sendmes(list);
                            Htwyy["执行结果"] = "ok";
                            Htwyy["提示文本"] = "补发货物无异议收货操作成功！";
                            return Htwyy;
                        case "有异议收货后无异议收货":
                            list = new List<Hashtable>();
                            list = TX(Type, drinfo, drs, sellyue, jsfwufei, drnow, buyjjrsy, buyjjrsell, zbNum, thbc, ythNum);
                            PublicClass2013.Sendmes(list);
                            Htwyy["执行结果"] = "ok";
                            Htwyy["提示文本"] = "有异议收货后无异议收货操作成功！";
                            return Htwyy;
                        default:
                            break;
                    }
                    #endregion
                                       
                }
                else if (action.Equals("no"))
                {
                    string Type = drs["type"].ToString();
                    switch (Type)
                    {
                        case "无异议收货":
                            Htwyy["执行结果"] = "pass";
                            Htwyy["提示文本"] = "根据您的指令，" + drinfo["发货单号"].ToString() + "发货单的" + drinfo["发货金额"].ToString() + "货款将全额转付至"  + drinfo["卖家名称"].ToString() + "账户。"; //+ drinfo["卖家登陆邮箱"].ToString() + "）"
                            return Htwyy;
                           // break;
                        case "补发货物无异议收货":
                            Htwyy["执行结果"] = "pass";
                            Htwyy["提示文本"] = "根据您的“补发货物无异议收货”指令，\n\r" + drinfo["发货单号"].ToString() + "发货单的" + drinfo["商品名称"].ToString() + "商品您已全部无异议收货，\n\r" + drinfo["发货金额"].ToString() + "元的货款将全额转付至" + drinfo["卖家名称"].ToString() + "账户。";
                            return Htwyy;
                        case "有异议收货后无异议收货":
                            Htwyy["执行结果"] = "pass";
                            Htwyy["提示文本"] = "根据您的“改为无异议收货”指令，\n\r" + drinfo["发货单号"].ToString() + "发货单的" + drinfo["商品名称"].ToString() + "商品您已全部无异议收货，\n\r" + drinfo["发货金额"].ToString() + "元的货款将全额转付至" + drinfo["卖家名称"].ToString() + "账户。";
                            return Htwyy;
                        default:
                            break;
                    }
                    
                }



            }
            else
            {
                Htwyy["执行结果"] = "err";
                Htwyy["提示文本"] = "没有获得任何T" + drs["Number"].ToString() + "提货单的信息！";
                return Htwyy;
            }
            return Htwyy;
        }
        catch
        {
            Hashtable Htwyy = new Hashtable();
            Htwyy["执行结果"] = "err";
            Htwyy["提示文本"] = "无异议收货时程序遇到异常！";
            return Htwyy;
        }
 
    }
    //无意义收货时发送的提醒
    private List<Hashtable> TX(string type, DataRow drinfo, DataRow drs, double sellyue, double jsfwufei, DateTime drnow, string buyjjrsy, string buyjjrsell, int zbNum, int thbc, int ythNum)
    {
        List<Hashtable> list = new List<Hashtable>();
        Hashtable htJYPT = new Hashtable();//交易平台
        htJYPT["type"] = "集合集合经销平台";
        htJYPT["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString().Trim();
        htJYPT["提醒对象用户名"] = drinfo["卖家名称"].ToString();
        htJYPT["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
        htJYPT["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
        htJYPT["提醒对象角色类型"] = "卖家";
        switch (type)
        {
            case "无异议收货":
                htJYPT["提醒内容文本"] = "您F" + drs["Number"].ToString().Trim() + "发货单，买方已无异议收货，" + drinfo["发货金额"].ToString() + "元的货款已转付至您的交易账户，" + jsfwufei.ToString("#0.00") + "元的技术服务费已同时扣收，敬请知悉。";
                break;
            case "补发货物无异议收货":
                htJYPT["提醒内容文本"] = "您F" + drs["Number"].ToString().Trim() + "发货单，买家已全部无异议收货（含所有补货），" + drinfo["发货金额"].ToString() + "元的货款已转付至您的交易账户， " + jsfwufei.ToString("#0.00") + "元的技术服务费已同时扣收，敬请知悉。";
                break;
            case "有异议收货后无异议收货":
                htJYPT["提醒内容文本"] = "您F" + drs["Number"].ToString().Trim() + "发货单，买方已改为全部无异议收货，" + drinfo["发货金额"].ToString() + "元的货款已转付至您的交易账户，" + jsfwufei.ToString("#0.00") + "元的技术服务费已同时扣收，敬请知悉。 ";
                break;
            default:
                break;
        }
        htJYPT["创建人"] = drs["登陆邮箱"].ToString().Trim();
        list.Add(htJYPT);

        Hashtable htjhjx = new Hashtable();
        htjhjx["type"] = "集合集合经销平台";
        htjhjx["提醒对象登陆邮箱"] = drinfo["买家关联经纪人邮箱"].ToString();
        htjhjx["提醒对象用户名"] = drinfo["买家关联经纪人名称"].ToString();
        htjhjx["提醒对象结算账户类型"] = drinfo["买家关联经纪人结算账户类型"].ToString();
        htjhjx["提醒对象角色编号"] = drinfo["买家关联经纪人角色编号"].ToString();
        htjhjx["提醒对象角色类型"] = "经纪人";
        htjhjx["提醒内容文本"] = "尊敬的" + drinfo["买家关联经纪人名称"].ToString() + "，您的交易账户于" + drnow.Year.ToString() + "年" + drnow.Month.ToString() + "月" + drnow.Day.ToString() + "日" + drnow.Hour.ToString() + "时" + drnow.Minute.ToString() + "分新增经纪人收益" + buyjjrsy + "元，请继续努力！";
        htjhjx["创建人"] = drinfo["买家关联经纪人邮箱"].ToString();
        list.Add(htjhjx);

        htjhjx = new Hashtable();
        htjhjx["type"] = "集合集合经销平台";
        htjhjx["提醒对象登陆邮箱"] = drinfo["卖家关联经纪人邮箱"].ToString();
        htjhjx["提醒对象用户名"] = drinfo["卖家关联经纪人名称"].ToString();
        htjhjx["提醒对象结算账户类型"] = drinfo["卖家关联经纪人结算账户类型"].ToString();
        htjhjx["提醒对象角色编号"] = drinfo["卖家关联经纪人角色编号"].ToString();
        htjhjx["提醒对象角色类型"] = "经纪人";
        htjhjx["提醒内容文本"] = "尊敬的" + drinfo["卖家关联经纪人名称"].ToString() + "，您的交易账户于" + drnow.Year.ToString() + "年" + drnow.Month.ToString() + "月" + drnow.Day.ToString() + "日" + drnow.Hour.ToString() + "时" + drnow.Minute.ToString() + "分新增经纪人收益" + buyjjrsell + "元，请继续努力！";
        htjhjx["创建人"] = drinfo["买家关联经纪人邮箱"].ToString();
        list.Add(htjhjx);

        if (zbNum == thbc + ythNum)
        {
            #region //自动清盘向卖家、买家发送提醒
            //集合经销平台                    
            htjhjx = new Hashtable();
            htjhjx["type"] = "集合集合经销平台";
            htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
            htjhjx["提醒对象用户名"] = drinfo["卖家名称"].ToString();
            htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
            htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
            htjhjx["提醒对象角色类型"] = "卖家";
            htjhjx["提醒内容文本"] = "尊敬的" + drinfo["卖家名称"].ToString() + "，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
            htjhjx["创建人"] = drs["登陆邮箱"].ToString().Trim();
            list.Add(htjhjx);
            htjhjx = new Hashtable();
            htjhjx["type"] = "集合集合经销平台";
            htjhjx["提醒对象登陆邮箱"] = drs["登陆邮箱"].ToString().Trim();
            htjhjx["提醒对象用户名"] = drinfo["买家名称"].ToString();//
            htjhjx["提醒对象结算账户类型"] = drinfo["买家结算账户类型"].ToString();
            htjhjx["提醒对象角色编号"] = drinfo["买家角色编号"].ToString();
            htjhjx["提醒对象角色类型"] = "买家";
            htjhjx["提醒内容文本"] = "尊敬的" + drinfo["买家名称"].ToString() + "，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
            htjhjx["创建人"] = drs["登陆邮箱"].ToString().Trim();
            list.Add(htjhjx);
            #endregion
        }

        return list;

    }

    //请重新发货
    private Hashtable qcxsh_strs(DataRow drs)
    {
        try
        {
            string action = drs["ispass"].ToString().Trim();
            Hashtable Htwyy = new Hashtable();
            // Htwyy["iserr"] = "buquan";
            DateTime dt = DateTime.Now;
            Htwyy["执行结果"] = "err";
            Htwyy["提示文本"] = "重新发货没有执行任何操作";
            //获得当前提货单据详细信息
            string Strgetinfo = "select a.Number,('T'+a.Number) as  提货单号,('F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖家名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买家角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.T_YSTBDMJJSBH as 卖家角色编号,b.T_YSTBDDLYX as 卖家登陆邮箱,b.T_YSTBDJSZHLX as 卖家角色账户类型,b.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,b.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,b.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,b.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,b.Z_ZBSL as 定标数量,b.Z_LYBZJJE as 履约保证金金额,b.Z_HTZT as 合同状态  from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX  where a.Number='" + drs["Number"].ToString().Trim() + "' and  a.F_DQZT ='已录入发货信息' and b.Y_YSYDDMJJSBH='" + drs["买家角色编号"].ToString() + "' and b.Z_HTZT='定标'";
            DataSet dsgetinfo = DbHelperSQL.Query(Strgetinfo);
            if (dsgetinfo != null && dsgetinfo.Tables[0].Rows.Count == 1)
            {
                DataRow drinfo = dsgetinfo.Tables[0].Rows[0];
                if (drs["验收照片路径"].ToString().Trim().Equals(""))
                {
                    Htwyy["执行结果"] = "err";
                    Htwyy["提示文本"] = "您还没有上传验收时照片！";
                    return Htwyy;
                }
                if (drs["情况说明"].ToString().Trim().Equals(""))
                {
                    Htwyy["执行结果"] = "err";
                    Htwyy["提示文本"] = "请录入重新发货的情况说明！";
                    return Htwyy;
                }

                if (action.Equals("ok"))
                {
                    //请重新发货操作
                    string StrSql = "update AAA_THDYFHDXXB set F_DQZT='请重新发货',F_BUYQZXFHYSZP='" + drs["验收照片路径"].ToString().Trim() + "',F_BUYQZXFHQKSM='" + drs["情况说明"].ToString().Trim() + "',F_BUYQZXFHCZSJ= '" + dt + "'   where Number = '" + drs["Number"].ToString().Trim() + "'";



                    if (DbHelperSQL.ExecuteSql(StrSql) > 0)
                    {
                        

                        #region 向卖家跟平台管理人员发送提醒
                        //集合经销平台
                        List<Hashtable> lht = new List<Hashtable>();
                        Hashtable htjhjx = new Hashtable();
                        htjhjx["type"] = "集合集合经销平台";
                        htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
                        htjhjx["提醒对象用户名"] = drinfo["卖家名称"].ToString();
                        htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
                        htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
                        htjhjx["提醒对象角色类型"] = "卖家";
                        htjhjx["提醒内容文本"] = "由于买方对" + drinfo["发货单号"].ToString() + "发货单要求重新发货，请尽快处理!";
                        htjhjx["创建人"] = drs["登陆邮箱"].ToString();
                        lht.Add(htjhjx);

                        //操作平台 
                        Hashtable htyept = new Hashtable();
                        htyept["type"] = "业务平台";
                        htyept["模提醒模块名"] = "货物签收重新发货";
                        htyept["查看地址"] = "";
                        htyept["提醒内容文本"] = "由于买方对" + drinfo["发货单号"].ToString() + "发货单要求重新发货，请尽快处理!";
                        lht.Add(htyept);
                        PublicClass2013.Sendmes(lht);
                        #endregion

                        Htwyy["执行结果"] = "ok";
                        Htwyy["提示文本"] = "请重新发货操作成功！ ";
                        return Htwyy;
                    }
                    else
                    {
                        Htwyy["执行结果"] = "ok";
                        Htwyy["提示文本"] = StrSql;
                        //Htwyy["iserr"] = "quan";
                        return Htwyy;
                    }

                }
                else
                {
                    Htwyy["执行结果"] = "pass";
                    Htwyy["提示文本"] = "您确认“请卖方重新发货”后，系统\n\r将会通知卖方就该申请进行确认。";
                    return Htwyy;
                }


            }
            else
            {
                Htwyy["执行结果"] = "err";
                Htwyy["提示文本"] = "没有查找到任何单号为" + drs["Number"].ToString() + "的，状态\n\r为“已录入发货信息”的，对应中标定标单状态为“定标”的提货单！";
                return Htwyy;
            }


            //return Htwyy;
        }
        catch
        {
            Hashtable Htwyy = new Hashtable();
            Htwyy["执行结果"] = "err";
            Htwyy["提示文本"] = "请重新发货时程序出现异常。";
            return Htwyy;
        }
    }

    //部分收货
    private Hashtable bufensh_strs(DataRow drs)
    {
        try
        {
            string action = drs["ispass"].ToString().Trim();
            Hashtable Htwyy = new Hashtable();
            DateTime dt = DateTime.Now;
            Htwyy["执行结果"] = "err";
            Htwyy["提示文本"] = "部分收货没有执行任何操作";

            //获得当前提货单据详细信息
            string Strgetinfo = "select a.Number,('T'+a.Number) as  提货单号,('F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖家名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买家角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.T_YSTBDMJJSBH as 卖家角色编号,b.T_YSTBDDLYX as 卖家登陆邮箱,b.T_YSTBDJSZHLX as 卖家角色账户类型,b.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,b.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,b.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,b.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,b.Z_ZBSL as 定标数量,b.Z_LYBZJJE as 履约保证金金额,b.Z_HTZT as 合同状态  from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX  where a.Number='" + drs["Number"].ToString().Trim() + "' and  a.F_DQZT ='已录入发货信息' and b.Y_YSYDDMJJSBH='" + drs["买家角色编号"].ToString() + "' and b.Z_HTZT='定标'"; 
            DataSet dsgetinfo = DbHelperSQL.Query(Strgetinfo);
            if (dsgetinfo != null && dsgetinfo.Tables[0].Rows.Count == 1)
            {
                DataRow drinfo = dsgetinfo.Tables[0].Rows[0];
                if (drs["验收照片路径"].ToString().Trim().Equals(""))
                {
                    Htwyy["执行结果"] = "err";
                    Htwyy["提示文本"] = "您还没有上传验收时照片！";
                    return Htwyy;
                }

                if (drs["发货单（物流单）影印件"].ToString().Trim().Equals(""))
                {
                    Htwyy["执行结果"] = "err";
                    Htwyy["提示文本"] = "您还没有上传签收纸质发货单（物流单）影印件！";
                    return Htwyy;
                }
                if (Convert.ToInt64(drs["部分签收数量"].ToString().Trim()) >= Convert.ToInt64(drinfo["提货数量"].ToString().Trim()))
                {
                    Htwyy["执行结果"] = "err1";
                    Htwyy["提示文本"] = "部分收货数量不可大于或者等于提货数量！";
                    return Htwyy;
                }

                if (action.Equals("ok"))
                {
                    //请重新发货操作
                    string StrSql = "update AAA_THDYFHDXXB set F_DQZT='部分收货',F_BUYBFSHYSZP='" + drs["验收照片路径"].ToString().Trim() + "',F_BUYBFSHFHDWLDYYJ='" + drs["发货单（物流单）影印件"].ToString().Trim() + "',F_BUYBFSHCZSJ= '" + dt + "', F_BUYBFSHSJQSSL='" + drs["部分签收数量"].ToString() + "'   where Number = '" + drs["Number"].ToString().Trim() + "'";

                 

                    if (DbHelperSQL.ExecuteSql(StrSql) > 0)
                    {
                        #region 向卖家跟平台管理人员发送提醒
                        //集合经销平台
                        List<Hashtable> lht = new List<Hashtable>();
                        Hashtable htjhjx = new Hashtable();
                        htjhjx["type"] = "集合集合经销平台";
                        htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
                        htjhjx["提醒对象用户名"] = drinfo["卖家名称"].ToString();
                        htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
                        htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
                        htjhjx["提醒对象角色类型"] = "卖家";
                        htjhjx["提醒内容文本"] =  drinfo["发货单号"].ToString() + "发货单买方已做“部分收货”处理，请您尽快安排补发货。";
                        htjhjx["创建人"] = drs["登陆邮箱"].ToString();
                        lht.Add(htjhjx);

                        //操作平台 
                        Hashtable htyept = new Hashtable();
                        htyept["type"] = "业务平台";
                        htyept["模提醒模块名"] = "部分收货";
                        htyept["查看地址"] = "";
                        htyept["提醒内容文本"] = drinfo["发货单号"].ToString() + "发货单买方已做“部分收货”处理，请您尽快安排补发货。";
                        lht.Add(htyept);
                        PublicClass2013.Sendmes(lht);
                        #endregion
                        Htwyy["执行结果"] = "ok";
                        Htwyy["提示文本"] = "部分收货操作成功！ ";
                        return Htwyy;
                    }
                    else
                    {
                          Htwyy["执行结果"] = "ok";
                          Htwyy["提示文本"] = StrSql;
                          return Htwyy;
                    }

                }
                else
                {
                    Htwyy["执行结果"] = "pass";
                    Htwyy["提示文本"] = "您确认“部分收货”后，系统将会通知卖方就收货差额部分给予补发。";
                    return Htwyy;
                }


            }
            else
            {
                Htwyy["执行结果"] = "err";
                Htwyy["提示文本"] = "没有查找到任何单号为" + drs["Number"].ToString() + "的，状态\n\r为“已录入发货信息”的，对应中标定标单状态为“定标”的提货单！";
                //return Htwyy；
                      return Htwyy;
            }

            return Htwyy;
        }
        catch
        {
            Hashtable Htwyy = new Hashtable();
            Htwyy["执行结果"] = "err";
            Htwyy["提示文本"] = "部分发货时程序出现异常！";
            //return Htwyy；
            return Htwyy;
        }
    }

    //有异议收货
    private Hashtable wyyth_strs(DataRow drs)
    {
        try
        {
            string action = drs["ispass"].ToString().Trim();
            Hashtable Htwyy = new Hashtable();
            DateTime dt = DateTime.Now;
            Htwyy["执行结果"] = "err";
            Htwyy["提示文本"] = "有意义签收没有执行任何操作";

            //获得当前提货单据详细信息
            string Strgetinfo = "select a.Number,('T'+a.Number) as  提货单号,('F'+a.Number ) as 发货单号,a.ZBDBXXBBH as 中标定标单号,b.Z_SPBH as 商品编号,b.Z_SPMC as 商品名称,b.Z_GG as 规格,a.ZBDJ as 定标价格,a.T_THSL as 提货数量,T_DJHKJE as 发货金额,c.I_JYFMC as 卖家名称,b.Z_HTBH as 电子购货合同,b.Y_YSYDDMJJSBH as 买家角色编号 ,a.F_DQZT as 当前状态,a.F_FHDSCSJ as 发货单生成时间,b.T_YSTBDMJJSBH as 卖家角色编号,b.T_YSTBDDLYX as 卖家登陆邮箱,b.T_YSTBDJSZHLX as 卖家角色账户类型,b.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,b.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,b.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,b.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,b.Z_ZBSL as 定标数量,b.Z_LYBZJJE as 履约保证金金额,b.Z_HTZT as 合同状态  from AAA_THDYFHDXXB as a left join AAA_ZBDBXXB as b on a.ZBDBXXBBH = b.Number left join AAA_DLZHXXB as c on b.T_YSTBDDLYX=c.B_DLYX  where a.Number='" + drs["Number"].ToString().Trim() + "' and  a.F_DQZT ='已录入发货信息' and b.Y_YSYDDMJJSBH='" + drs["买家角色编号"].ToString() + "' and b.Z_HTZT='定标'";
            DataSet dsgetinfo = DbHelperSQL.Query(Strgetinfo);
            if (dsgetinfo != null && dsgetinfo.Tables[0].Rows.Count == 1)
            {
                DataRow drinfo = dsgetinfo.Tables[0].Rows[0];
                if (drs["验收照片路径"].ToString().Trim().Equals(""))
                {
                    Htwyy["执行结果"] = "err";
                    Htwyy["提示文本"] = "您还没有上传验收时照片！";
                    return Htwyy;
                }

                if (drs["发货单（物流单）影印件"].ToString().Trim().Equals(""))
                {
                    Htwyy["执行结果"] = "err";
                    Htwyy["提示文本"] = "您还没有上传签收纸质发货单（物流单）影印件！";
                    return Htwyy;
                }

                if (drs["情况说明"].ToString().Trim().Equals(""))
                {
                    Htwyy["执行结果"] = "err";
                    Htwyy["提示文本"] = "有异议收货情况说明不能为空！";
                    return Htwyy;
                }

                if (action.Equals("ok"))
                {
                    //请重新发货操作
                    string StrSql = "update AAA_THDYFHDXXB set F_DQZT='有异议收货',F_BUYYYYYSZP='" + drs["验收照片路径"].ToString().Trim() + "',F_BUYYYYSHFHDWLDYYJ='" + drs["发货单（物流单）影印件"].ToString().Trim() + "',F_BUYYYYSHCZSJ= '" + dt + "', F_BUYYYYQKSM='" + drs["情况说明"].ToString() + "'   where Number = '" + drs["Number"].ToString().Trim() + "'";


                    if (DbHelperSQL.ExecuteSql(StrSql) > 0)
                    {
                      

                        #region 向卖家跟平台管理人员发送提醒
                        //集合经销平台
                        List<Hashtable> lht = new List<Hashtable>();
                        Hashtable htjhjx = new Hashtable();
                        htjhjx["type"] = "集合集合经销平台";
                        htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
                        htjhjx["提醒对象用户名"] = drinfo["卖家名称"].ToString();
                        htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
                        htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
                        htjhjx["提醒对象角色类型"] = "卖家";
                        htjhjx["提醒内容文本"] = "您的" + drinfo["发货单号"].ToString() + "发货单，买方已做“有异议收货”处理，已进入争议处理程序，请尽快与相应买方联系!";
                        htjhjx["创建人"] = drs["登陆邮箱"].ToString();
                        lht.Add(htjhjx);

                        //操作平台 
                        Hashtable htyept = new Hashtable();
                        htyept["type"] = "业务平台";
                        htyept["模提醒模块名"] = "有异议收货";
                        htyept["查看地址"] = "";
                        htyept["提醒内容文本"] = "您的" + drinfo["发货单号"].ToString() + "发货单，买方已做“有异议收货”处理，已进入争议处理程序，请尽快与相应买方联系!"; 
                        lht.Add(htyept);
                        PublicClass2013.Sendmes(lht);
                        #endregion

                        Htwyy["执行结果"] = "ok";
                        Htwyy["提示文本"] = " 有异议收货操作成功！ ";
                        return Htwyy;
                    }

                }
                else
                {
                    Htwyy["执行结果"] = "pass";
                    Htwyy["提示文本"] = "根据您的指令，" + drinfo["发货单号"].ToString() + "发货单将进入争议处理程序。";
                    return Htwyy;
                }


            }
            else
            {
                Htwyy["执行结果"] = "err";
                Htwyy["提示文本"] = "没有查找到任何单号为" + drs["Number"].ToString() + "的，状态为“已录入发货信息”的\n\r，对应中标定标单状态为“定标”的提货单！";
            }


            return Htwyy;
        }
        catch
        {
            Hashtable Htwyy = new Hashtable();
            Htwyy["执行结果"] = "err";
            Htwyy["提示文本"] = "有异议收货时时程序出现异常！";
            //return Htwyy；
            return Htwyy;
        }
    }
}