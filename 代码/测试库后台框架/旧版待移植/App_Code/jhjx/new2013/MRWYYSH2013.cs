using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FMOP.DB;

/// <summary>
/// MRWYYSH2013 的摘要说明
/// </summary>
public class MRWYYSH2013
{
	public MRWYYSH2013()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public DataSet MRWYYQS(DataSet dsreturn)
    {
        try
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "默认无异议收货未执行任何操作！";

            
            int num = 0;
            int errnum = 0;
            string fhd = "";
            //string strSQL = "select DATEDIFF(dd,t.F_QMJQSQRCZSJ,getdate()) 提请签收后天数,t.Number,('T'+t.Number) 提货单编号,('F'+t.Number) 发货单号,t.ZBDBXXBBH as 中标定标单号,d.Z_SPBH as 商品编号,d.Z_SPMC as 商品名称,d.Z_GG as 规格,t.ZBDJ as 定标价格,t.T_THSL as 提货数量,T_DJHKJE as 发货金额,DL.I_JYFMC as 卖家名称,d.Z_HTBH as 电子购货合同,d.Y_YSYDDMJJSBH as 买家角色编号 ,d.Y_YSYDDDLYX 买家登录邮箱,'买家结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB a where a.J_BUYJSBH=d.Y_YSYDDMJJSBH),'买家名称'=(select I_JYFMC from AAA_DLZHXXB a where a.J_BUYJSBH=d.Y_YSYDDMJJSBH),t.F_DQZT as 当前状态,t.F_FHDSCSJ as 发货单生成时间,d.T_YSTBDMJJSBH as 卖家角色编号,d.T_YSTBDDLYX as 卖家登陆邮箱,d.T_YSTBDJSZHLX as 卖家角色账户类型,d.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,d.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,d.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,d.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,d.Z_ZBSL as 定标数量,d.Z_LYBZJJE as 履约保证金金额,d.Z_HTZT as 合同状态 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number left join AAA_DLZHXXB DL on DL.J_SELJSBH=d.T_YSTBDMJJSBH where d.Z_HTZT='定标' and d.Z_QPZT='未开始清盘' and (t.F_DQZT='已录入发货信息' or t.F_DQZT='已录入补发备注') and t.F_QMJQSQRCZSJ is not null and DATEDIFF(dd,t.F_QMJQSQRCZSJ,getdate())>=4";
            string strSQL = "select DATEDIFF(dd,t.F_QMJQSQRCZSJ,getdate()) 提请签收后天数,t.Number,('T'+t.Number) 提货单编号,('F'+t.Number) 发货单号,t.ZBDBXXBBH as 中标定标单号,d.Z_SPBH as 商品编号,d.Z_SPMC as 商品名称,d.Z_GG as 规格,t.ZBDJ as 定标价格,t.T_THSL as 提货数量,T_DJHKJE as 发货金额,DL.I_JYFMC as 卖家名称,d.Z_HTBH as 电子购货合同,d.Y_YSYDDMJJSBH as 买家角色编号 ,d.Y_YSYDDDLYX 买家登录邮箱,'买家结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB a where a.J_BUYJSBH=d.Y_YSYDDMJJSBH),'买家名称'=(select I_JYFMC from AAA_DLZHXXB a where a.J_BUYJSBH=d.Y_YSYDDMJJSBH),t.F_DQZT as 当前状态,t.F_FHDSCSJ as 发货单生成时间,d.T_YSTBDMJJSBH as 卖家角色编号,d.T_YSTBDDLYX as 卖家登陆邮箱,d.T_YSTBDJSZHLX as 卖家角色账户类型,d.T_YSTBDGLJJRYX as 卖家关联经纪人邮箱,d.T_YSTBDGLJJRJSBH as 卖家关联经纪人角色编号,'卖家关联经纪人名称'=(select I_JYFMC from AAA_DLZHXXB a where a.J_JJRJSBH=d.T_YSTBDGLJJRJSBH),'卖家关联经纪人结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB a where a.J_JJRJSBH=d.T_YSTBDGLJJRJSBH),d.Y_YSYDDGLJJRYX as 买家关联经纪人邮箱,d.Y_YSYDDGLJJRJSBH as 买家关联经纪人角色编号,'买家关联经纪人名称'=(select I_JYFMC from AAA_DLZHXXB a where a.J_JJRJSBH=d.Y_YSYDDGLJJRJSBH),'买家关联经纪人结算账户类型'=(select B_JSZHLX from AAA_DLZHXXB a where a.J_JJRJSBH=d.Y_YSYDDGLJJRJSBH),d.Z_ZBSL as 定标数量,d.Z_LYBZJJE as 履约保证金金额,d.Z_HTZT as 合同状态 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number left join AAA_DLZHXXB DL on DL.J_SELJSBH=d.T_YSTBDMJJSBH where d.Z_HTZT='定标' and d.Z_QPZT='未开始清盘' and (t.F_DQZT='已录入发货信息' or t.F_DQZT='已录入补发备注') and t.F_QMJQSQRCZSJ is not null and DATEDIFF(dd,t.F_QMJQSQRCZSJ,getdate())>=4";
            DataSet dsgetinfo = DbHelperSQL.Query(strSQL);
            if (dsgetinfo != null && dsgetinfo.Tables[0].Rows.Count >0)
            {
                
                #region//提前获取资金变动明细表中的信息、平台动态参数设定表
                string Strtb = " select  Number,'' as 登录邮箱,'' as 结算账户类型,'' as 角色编号,'' as 来源单号,'' as 流水产生时间,'' as 运算类型,'' as 金额, 'AAA_ZBDBXXB' as 来源业务来行,XM,XZ,ZY,SJLX,YSLX from   AAA_moneyDZB where Number in ('1304000010','1304000012','1304000014','1304000007','1304000043','1304000042','1304000048','1304000028','1304000037','1304000026','1304000036')";
                DataSet dshkjd = DbHelperSQL.Query(Strtb);
                Hashtable htt = PublicClass2013.GetParameterInfo();
                #endregion

               
                

                foreach (DataRow drinfo in dsgetinfo.Tables[0].Rows)
                {
                    ArrayList al = new ArrayList();
                    Hashtable htjhjx;
                    List<Hashtable> lht = new List<Hashtable>();

                    #region//验证买家账户状态
                    Hashtable hsuser = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString());
                    //验证是开通交易账户
                    if (hsuser["交易账户是否开通"].ToString().Trim() == "否")
                    {
                        continue;
                    }

                    //验证是否冻结

                    //if (hsuser["是否冻结"].ToString().Trim() == "是")
                    //{
                    //    continue;
                    //}

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

                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
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

                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
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
                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();
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
                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();
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
                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家关联经纪人角色编号"].ToString())["结算账户类型"].ToString();

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
                        Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                        string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家关联经纪人角色编号"].ToString())["结算账户类型"].ToString();

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
                        drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000048'");
                        if (drhkjd != null && drhkjd.Length > 0)
                        {
                            string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["电子购货合同"].ToString().Trim());
                            Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                            string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

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
                        DataSet dsfpyq = DbHelperSQL.Query(GetTHdNumber);
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
                            DataSet dsfpykmc = DbHelperSQL.Query(Strfpykmc);
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
                                    Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + je.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
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
                                    Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString())["结算账户类型"].ToString();

                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["买家登录邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["买家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + je.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
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
                            DataSet dshwykmc = DbHelperSQL.Query(Strhwykmc);
                            if (dshwykmc != null && dshwykmc.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow drfpykk in dshwykmc.Tables[0].Rows)
                                {
                                    hwje += Convert.ToDouble(drfpykk["JE"].ToString().Trim());
                                }

                                //卖家扣罚(发货单生成后5日内未录入发货信息)

                                drhkjd = dshkjd.Tables[0].Select("1=1 and Number='1304000026'");
                                if (drhkjd != null && drhkjd.Length > 0)
                                {
                                    string zy = drhkjd[0]["ZY"].ToString().Trim().Replace("[x1]", drinfo["发货单号"].ToString().Trim());
                                    Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    string jszhlx = PublicClass2013.GetUserInfo(drinfo["卖家角色编号"].ToString())["结算账户类型"].ToString();

                                    //发货单生成后5日内未录入发货信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["卖家登陆邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["卖家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + hwje.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
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
                                    Keynumber = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                    string jszhlx = PublicClass2013.GetUserInfo(drinfo["买家角色编号"].ToString())["结算账户类型"].ToString();

                                    //发货单生成后5日内未录入发票邮寄信息sql
                                    string strinsehwqsfpyqkf = "insert into AAA_ZKLSMXB (Number, DLYX,JSZHLX,JSBH,LYYWLX,LYDH, LSCSSJ,YSLX,JE,XM,XZ,ZY,SJLX,CreateUser,CreateTime,JKBH  ) values ('" + Keynumber + "','" + drinfo["买家登录邮箱"].ToString() + "','" + jszhlx + "','" + drinfo["买家角色编号"].ToString() + "','AAA_THDYFHDXXB','" + drinfo["Number"].ToString() + "','" + drnow + "','" + drhkjd[0]["YSLX"].ToString() + "','" + hwje.ToString() + "','" + drhkjd[0]["XM"].ToString() + "','" + drhkjd[0]["XZ"].ToString() + "','" + zy + "','" + drhkjd[0]["SJLX"].ToString() + "','" + drinfo["买家登录邮箱"].ToString() + "','" + drnow + "','监控编号' ) ";
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
                    htjhjx = new Hashtable();
                    htjhjx["type"] = "集合集合经销平台";
                    htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
                    htjhjx["提醒对象用户名"] = drinfo["卖家名称"].ToString();
                    htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
                    htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
                    htjhjx["提醒对象角色类型"] = "卖家";
                    htjhjx["提醒内容文本"] = "您" + drinfo["发货单号"].ToString().Trim() + "发货单，系统自动签收，" + drinfo["发货金额"].ToString() + "元的货款已转付至您的交易账户， " + jsfwufei.ToString("#0.00") + "元的技术服务费已同时扣收，敬请知悉。";
                    htjhjx["创建人"] = drinfo["买家登录邮箱"].ToString();
                    lht.Add(htjhjx);
                
                    htjhjx = new Hashtable();
                    htjhjx["type"] = "集合集合经销平台";
                    htjhjx["提醒对象登陆邮箱"] = drinfo["买家登录邮箱"].ToString();
                    htjhjx["提醒对象用户名"] = drinfo["买家名称"].ToString();
                    htjhjx["提醒对象结算账户类型"] = drinfo["买家结算账户类型"].ToString();
                    htjhjx["提醒对象角色编号"] = drinfo["买家角色编号"].ToString();
                    htjhjx["提醒对象角色类型"] = "买家";
                    htjhjx["提醒内容文本"] = "您" + drinfo["发货单号"].ToString().Trim() + "发货单，系统自动签收，" + drinfo["发货金额"].ToString() + "元的货款已转付至相应卖方账户，敬请知悉。";
                    htjhjx["创建人"] = drinfo["买家登录邮箱"].ToString();
                    lht.Add(htjhjx);

                    htjhjx = new Hashtable();
                    htjhjx["type"] = "集合集合经销平台";
                    htjhjx["提醒对象登陆邮箱"] = drinfo["买家关联经纪人邮箱"].ToString();
                    htjhjx["提醒对象用户名"] = drinfo["买家关联经纪人名称"].ToString();
                    htjhjx["提醒对象结算账户类型"] = drinfo["买家关联经纪人结算账户类型"].ToString();
                    htjhjx["提醒对象角色编号"] = drinfo["买家关联经纪人角色编号"].ToString();
                    htjhjx["提醒对象角色类型"] = "经纪人";
                    htjhjx["提醒内容文本"] = "尊敬的" + drinfo["买家关联经纪人名称"].ToString() + "，您的交易账户于" + drnow.Year.ToString() + "年" + drnow.Month.ToString() + "月" + drnow.Day.ToString() + "日" + drnow.Hour.ToString() + "时" + drnow.Minute.ToString() + "分新增经纪人收益" + buyjjrsy + "元，请继续努力！";
                    htjhjx["创建人"] = drinfo["买家关联经纪人邮箱"].ToString();
                    lht.Add(htjhjx);

                    htjhjx = new Hashtable();
                    htjhjx["type"] = "集合集合经销平台";
                    htjhjx["提醒对象登陆邮箱"] = drinfo["卖家关联经纪人邮箱"].ToString();
                    htjhjx["提醒对象用户名"] = drinfo["卖家关联经纪人名称"].ToString();
                    htjhjx["提醒对象结算账户类型"] = drinfo["卖家关联经纪人结算账户类型"].ToString();
                    htjhjx["提醒对象角色编号"] = drinfo["卖家关联经纪人角色编号"].ToString();
                    htjhjx["提醒对象角色类型"] = "经纪人";
                    htjhjx["提醒内容文本"] = "尊敬的" + drinfo["卖家关联经纪人名称"].ToString() + "，您的交易账户于" + drnow.Year.ToString() + "年" + drnow.Month.ToString() + "月" + drnow.Day.ToString() + "日" + drnow.Hour.ToString() + "时" + drnow.Minute.ToString() + "分新增经纪人收益" + buyjjrsell + "元，请继续努力！";
                    htjhjx["创建人"] = drinfo["买家关联经纪人邮箱"].ToString();
                    lht.Add(htjhjx);
                    #endregion
                    if (zbNum == thbc + ythNum)
                    {
                        #region //清盘提醒 向卖家、买家发送提醒
                        //集合经销平台                    
                        htjhjx = new Hashtable();
                        htjhjx["type"] = "集合集合经销平台";
                        htjhjx["提醒对象登陆邮箱"] = drinfo["卖家登陆邮箱"].ToString();
                        htjhjx["提醒对象用户名"] = drinfo["卖家名称"].ToString();
                        htjhjx["提醒对象结算账户类型"] = drinfo["卖家角色账户类型"].ToString();
                        htjhjx["提醒对象角色编号"] = drinfo["卖家角色编号"].ToString();
                        htjhjx["提醒对象角色类型"] = "卖家";
                        htjhjx["提醒内容文本"] = "尊敬的" + drinfo["卖家名称"].ToString() + "，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                        htjhjx["创建人"] = drinfo["买家登录邮箱"].ToString();
                        lht.Add(htjhjx);
                        htjhjx = new Hashtable();
                        htjhjx["type"] = "集合集合经销平台";
                        htjhjx["提醒对象登陆邮箱"] = drinfo["买家登录邮箱"].ToString();
                        htjhjx["提醒对象用户名"] = drinfo["买家名称"].ToString();
                        htjhjx["提醒对象结算账户类型"] = drinfo["买家结算账户类型"].ToString();
                        htjhjx["提醒对象角色编号"] = drinfo["买家角色编号"].ToString();
                        htjhjx["提醒对象角色类型"] = "买家";
                        htjhjx["提醒内容文本"] = "尊敬的" + drinfo["买家名称"].ToString() + "，您" + drinfo["电子购货合同"].ToString() + "《电子购货合同》已履行完毕，系统已给予自动清盘，您可在“交易账户”查看有关情况。";
                        htjhjx["创建人"] = drinfo["买家登录邮箱"].ToString();
                        lht.Add(htjhjx);
                        #endregion
                    }
                

                    if (al != null && al.Count > 0)
                    {
                        try
                        {
                            DbHelperSQL.ExecuteSqlTran(al);
                            PublicClass2013.Sendmes(lht);
                            #region 交易方信用变化情况 edittime 2013-09

                            jhjx_JYFXYMX xymc = new jhjx_JYFXYMX();

                            List<string> jfl = new List<string>();

                            //foreach (DataRow drinfo in dsgetinfo.Tables[0].Rows)
                            //{
                                string sqlstr = "select Z_QPZT  from  AAA_ZBDBXXB where  Z_HTBH='" + drinfo["电子购货合同"].ToString().Trim() + "'";
                                object bszt = DbHelperSQL.GetSingle(sqlstr);
                                if (bszt != null && bszt.ToString().Trim() == "清盘结束")
                                {
                                    string buyeremail = drinfo["买家登录邮箱"].ToString().Trim();
                                    string selleremail = drinfo["卖家登陆邮箱"].ToString().Trim();
                                    string buyjjr = drinfo["买家关联经纪人邮箱"].ToString().Trim();
                                    string selljjr = drinfo["卖家关联经纪人邮箱"].ToString().Trim();
                                    string htnum = drinfo["电子购货合同"].ToString().Trim();

                                    string[] buystrs = xymc.ZSZHMR_SFWQLV(buyeremail, htnum, "买家", "admin");
                                    string[] sellstrs = xymc.ZSZHMC_SFWQLV(selleremail, htnum, "卖家", "admin");
                                    string[] bjjrstrs = new string[2];
                                    string[] sjjrstrs = new string[2];

                                    if (xymc.CanDoing(htnum, buyeremail, "买家"))
                                    {
                                        bjjrstrs = xymc.GLJYF_WQLY(buyjjr, buyeremail, htnum, "经纪人", "admin");

                                    }
                                    else
                                    {
                                        bjjrstrs = xymc.GLJYF_WWQLY(buyjjr, buyeremail, htnum, "经纪人", "admin");
                                    }

                                    if (xymc.CanDoing(htnum, selleremail, "卖家"))
                                    {
                                        sjjrstrs = xymc.GLJYF_WQLY(selljjr, selleremail, htnum, "经纪人", "admin");

                                    }
                                    else
                                    {
                                        sjjrstrs = xymc.GLJYF_WWQLY(selljjr, selleremail, htnum, "经纪人", "admin");


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



                            //}
                            if (jfl.Count > 0)
                            {
                                DbHelperSQL.ExecuteSqlTran(jfl);

                            }


                            #endregion

                            num++;
                        }
                        catch(Exception)
                        {
                            errnum++;
                            fhd += "F" + drinfo["Number"].ToString()+"、";
                        }                    
                    }

                }

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
}