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
///FBtb 的摘要说明，处理投标信息的发布 (by 于海滨)
/// </summary>
public class THDclass
{
    public THDclass()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}


    /// <summary>
    /// 计算最迟发货日
    /// </summary>
    /// <param name="begintime">合同开始时间</param>
    /// <param name="htzq">合同周期</param>
    /// <param name="nowthdl">当前要计算的提货单的提货量</param>
    /// <param name="zhubiao">投标信息主表编号</param>
    /// <param name="zibiao">投标信息子表编号</param>
    /// <returns></returns>
    public string Getzcfhr(string begintime, string htzq, Int64 nowthdl, string zhubiao, string zibiao)
    {
        DataSet dsthl_all = DbHelperSQL.Query(" select isnull(sum(ZBSL),0) from ZZ_ZBDBXXB where SELTBXXZBH = '" + zhubiao + "' and MJTBXXChildBH = '" + zibiao + "' ");
        Int64 zthl = Convert.ToInt64(dsthl_all.Tables[0].Rows[0][0]);

        DataSet dsthl_old = DbHelperSQL.Query(" select isnull(sum(THZSL),0) from ZZ_THDXXB where SELTBXXH = '" + zhubiao + "' and SELTBXXZBH = '" + zibiao + "' ");
        Int64 oldthzl = Convert.ToInt64(dsthl_old.Tables[0].Rows[0][0]);
        DateTime DTbegintime= Convert.ToDateTime(begintime);
        DateTime DTreturntime = new DateTime();
        int alldaynum = 0;
        if (htzq == "三个月")
        {
            alldaynum = 30 * 3;
        }
        if (htzq == "一年")
        {
            alldaynum = 30 * 12;
        }
        //日均发货量
        Int64 daypingjun = Convert.ToInt64(Math.Ceiling(zthl / alldaynum * 1.05));
        //时间差
        TimeSpan ts =DateTime.Now-DTbegintime;
        //下达提货单时应发货总量
        Int64 yfhl = daypingjun * ts.Days;
        if (nowthdl + oldthzl > yfhl)
        {
            //定标日期+(当前单个提货单量+已下达提货总量) / 日平均最高发货量
            double tc = (nowthdl + oldthzl) / daypingjun;
            Int64 tcday = Convert.ToInt64(Math.Ceiling(tc));
            DTreturntime = DTbegintime.AddDays(tcday + 3);
        }
        else
        {
            //定标日期+3
            DTreturntime = DateTime.Now.AddDays(3);
        }
        return DTreturntime.Year + "-" + DTreturntime.Month + "-" + DTreturntime.Day + " 00:00:00";
    }


    /// <summary>
    /// 保存提货单
    /// </summary>
    /// <param name="dsinfo">提货单提交参数</param>
    /// <returns></returns>
    public string SaveTHD(DataSet dsinfo)
    {
        //基本验证
        string BuyJSBH = dsinfo.Tables[0].Rows[0]["买家角色编号"].ToString();
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(BuyJSBH);

        if (BuyJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
        {
            return "下达预订单失败，您尚未开通结算账户!";
        }
        if (UserInfo["是否允许登录"].ToString() != "是")
        {
            return "下达预订单失败，您已被禁止登录!";
        }
        if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
        {
            return "下达预订单失败，当前时间业务停止运行!";
        }
        if (UserInfo["是否休眠"].ToString() == "是")
        {
            return "下达预订单失败，您的账号已休眠!";
        }
        //if (UserInfo["是否冻结"].ToString() == "是")
        //{
        //    return "下达预订单失败，您的账号已被禁止业务!";
        //}
        if (dsinfo.Tables[0].Rows[0]["本次提货量"].ToString().Trim() == "" || Convert.ToInt64(dsinfo.Tables[0].Rows[0]["本次提货量"]) < 1)
        {
            return "下达预订单失败，本次提货数量不能为零!";
        }
        //获取中标信息
        DataSet dsZBinfo = DbHelperSQL.Query(" select top 1 * from ZZ_ZBDBXXB   where Number='" + dsinfo.Tables[0].Rows[0]["合同编号"].ToString() + "' and BUYJSBH='" + BuyJSBH + "' and ZBDBZT='定标执行中' order by  Number");
        //验证本次提货量是否超过可提货量
        if (Convert.ToInt64(dsinfo.Tables[0].Rows[0]["本次提货量"]) > (Convert.ToInt64(dsZBinfo.Tables[0].Rows[0]["ZBSL"]) - Convert.ToInt64(dsZBinfo.Tables[0].Rows[0]["BUYYTHSL"])))
        {
            return "下达预订单失败，可提货量已不足!";
        }



        double DBJG = Convert.ToDouble(dsZBinfo.Tables[0].Rows[0]["SELTBJG"]);
        Int64 THZSL = Convert.ToInt64(dsinfo.Tables[0].Rows[0]["本次提货量"]);
        double THZJE = DBJG * THZSL;

        //验证额度是否充足
        ClassMoney CM = new ClassMoney();
        double kyMoney = CM.GetMoneyT(UserInfo["登录邮箱"].ToString(), "模拟而已");
        if (kyMoney < THZJE)
        {
            return "下达预订单失败，可用额度不足，提货总金额" + THZJE + "元，剩余金额" + kyMoney + "元!";
        }
        

        //保存提货单并更新中标定标信息表
        string nowtimestr = DateTime.Now.ToString();


        string Number = jhjx_PublicClass.GetNextNumberZZ("ZZ_THDXXB", "");
        string ZBDBXXBH = dsinfo.Tables[0].Rows[0]["合同编号"].ToString();
        string BUYDLYX = dsZBinfo.Tables[0].Rows[0]["BUYDLYX"].ToString();
        string BUYYHM = dsZBinfo.Tables[0].Rows[0]["MJYHM"].ToString();
        string BUYJSBH = dsZBinfo.Tables[0].Rows[0]["BUYJSBH"].ToString();
        string GLDLRDLYX = dsZBinfo.Tables[0].Rows[0]["BUYJJRDLYX"].ToString();
        string GLDLRYHM = dsZBinfo.Tables[0].Rows[0]["BUYJJRYHM"].ToString();
        string GLDLRJSBH = dsZBinfo.Tables[0].Rows[0]["BUYJJRJSBH"].ToString();
        string YDDH = dsZBinfo.Tables[0].Rows[0]["BUYYDDBH"].ToString();
        string SELDLYX = dsZBinfo.Tables[0].Rows[0]["SELDLYX"].ToString();
        string SELYHM = dsZBinfo.Tables[0].Rows[0]["SELYHM"].ToString();
        string SELJSBH = dsZBinfo.Tables[0].Rows[0]["SELJSBH"].ToString();
        string SELDLRDLYX = dsZBinfo.Tables[0].Rows[0]["SELJJRDLYX"].ToString();
        string SELDLRYHM = dsZBinfo.Tables[0].Rows[0]["SELJJRYHM"].ToString();
        string SELDLRJSBH = dsZBinfo.Tables[0].Rows[0]["SELJJRJSBH"].ToString();
        string SELTBXXH = dsZBinfo.Tables[0].Rows[0]["SELTBXXZBH"].ToString();
        string SELTBXXZBH = dsZBinfo.Tables[0].Rows[0]["MJTBXXChildBH"].ToString();
        string TBLX = dsZBinfo.Tables[0].Rows[0]["HTQX"].ToString();
        string SPBH = dsZBinfo.Tables[0].Rows[0]["SPBH"].ToString();
        string SPMC = dsZBinfo.Tables[0].Rows[0]["SPMC"].ToString();
        string GGXH = dsZBinfo.Tables[0].Rows[0]["GGXH"].ToString();
        string JJDW = dsZBinfo.Tables[0].Rows[0]["JJDW"].ToString();
        Int64 JJPL = Convert.ToInt64(dsZBinfo.Tables[0].Rows[0]["JJPL"]);


        string SHGSMC = dsinfo.Tables[0].Rows[0]["SHDWMC"].ToString();
        string SZSF = dsinfo.Tables[0].Rows[0]["SZSF"].ToString();
        string SZDS = dsinfo.Tables[0].Rows[0]["SZDS"].ToString();
        string SZQX = dsinfo.Tables[0].Rows[0]["SZQX"].ToString();
        string XXDZ = dsinfo.Tables[0].Rows[0]["XXDZ"].ToString();
        string YZBM = dsinfo.Tables[0].Rows[0]["YZBM"].ToString();
        string LXDH = dsinfo.Tables[0].Rows[0]["LXDH"].ToString();
        string FPLX = dsinfo.Tables[0].Rows[0]["开票类型"].ToString();
        string KPXX = dsinfo.Tables[0].Rows[0]["开票信息"].ToString();
        string THDXDSJ = nowtimestr;
        string SELFHSJ = "null";
        string CYWLGSMC = "null";
        string WLDH = "null";
        string WLQSSJ = "null";
        string FPH = "null";
        string BUYQSSJ = "null";
        string THDZT = "待发货";
        string THDBZ = dsinfo.Tables[0].Rows[0]["备注"].ToString();

        string ZCFHR = Getzcfhr(dsZBinfo.Tables[0].Rows[0]["DBSJ"].ToString(), dsZBinfo.Tables[0].Rows[0]["HTQX"].ToString(), THZSL, dsZBinfo.Tables[0].Rows[0]["SELTBXXZBH"].ToString(), dsZBinfo.Tables[0].Rows[0]["MJTBXXChildBH"].ToString());
        string CreateTime = nowtimestr;
        ArrayList alsql = new ArrayList();
        alsql.Add(" INSERT INTO ZZ_THDXXB ([Number],[ZBDBXXBH],[BUYDLYX],[BUYYHM],[BUYJSBH],[GLDLRDLYX],[GLDLRYHM],[GLDLRJSBH],[YDDH],[SELDLYX],[SELYHM],[SELJSBH],[SELDLRDLYX],[SELDLRYHM],[SELDLRJSBH],[SELTBXXH],[SELTBXXZBH],[TBLX],[SPBH],[SPMC],[GGXH],[JJDW],[JJPL],[DBJG],[THZSL],[THZJE],[SHGSMC],[SZSF],[SZDS],[SZQX],[XXDZ],[YZBM],[LXDH],[FPLX],[KPXX],[THDXDSJ],[SELFHSJ],[CYWLGSMC],[WLDH],[WLQSSJ],[FPH],[BUYQSSJ],[THDZT],[THDBZ],[ZCFHR],[CreateTime])  VALUES ('" + Number + "','" + ZBDBXXBH + "','" + BUYDLYX + "','" + BUYYHM + "','" + BUYJSBH + "','" + GLDLRDLYX + "','" + GLDLRYHM + "','" + GLDLRJSBH + "','" + YDDH + "','" + SELDLYX + "','" + SELYHM + "','" + SELJSBH + "','" + SELDLRDLYX + "','" + SELDLRYHM + "','" + SELDLRJSBH + "','" + SELTBXXH + "','" + SELTBXXZBH + "','" + TBLX + "','" + SPBH + "','" + SPMC + "','" + GGXH + "','" + JJDW + "'," + JJPL + "," + DBJG + "," + THZSL + "," + THZJE + ",'" + SHGSMC + "','" + SZSF + "','" + SZDS + "','" + SZQX + "','" + XXDZ + "','" + YZBM + "','" + LXDH + "','" + FPLX + "','" + KPXX + "','" + THDXDSJ + "'," + SELFHSJ + "," + CYWLGSMC + "," + WLDH + "," + WLQSSJ + "," + FPH + "," + BUYQSSJ + ",'" + THDZT + "','" + THDBZ + "','" + ZCFHR + "','" + CreateTime + "')");
        alsql.Add("update ZZ_ZBDBXXB set BUYYTHSL = BUYYTHSL+" + THZSL.ToString() + "  where Number='" + dsinfo.Tables[0].Rows[0]["合同编号"].ToString() + "' and BUYJSBH='" + BuyJSBH + "' and ZBDBZT='定标执行中'");

        
        //插入资金变更明细表SQl
        Hashtable htinfo = new Hashtable();
        htinfo["来源单号"] = Number;
        htinfo["变动金额"] = THZJE;
        htinfo["变动前账户余额"] = kyMoney;
        htinfo["变动后账户余额"] = kyMoney - THZJE;
        ArrayList almoney = CM.InsertMoneyUse("下达提货单", htinfo, BuyJSBH);
        alsql.AddRange(almoney);

        bool th = DbHelperSQL.ExecSqlTran(alsql);
        if (th)
        {
            //执行成功，调用银行接口扣减额度
            CM.FreezeMoneyT(UserInfo["登录邮箱"].ToString(), THZJE.ToString(), "-");
            return "ok";
        }
        else
        {
            return "下达提货单失败，意外错误！";
        }
        
    }



    /// <summary>
    /// 获取提货单的一些基本信息，用于提交提货单
    /// </summary>
    /// <param name="Number">合同编号</param>
    /// <param name="jsbh">买家角色编号</param>
    /// <returns></returns>
    public DataSet GetTHinfoRun(string Number, string jsbh)
    {
        //获取中标信息
        DataSet dsreturn = DbHelperSQL.Query(" select top 1 '是否存在默认地址'='','默认收货单位名称'='','默认收货人姓名'='','默认省'='','默认市'='','默认区'='','默认详细地址'='','默认邮政编码'='','默认联系电话'='','开票类型'='','开票信息'='',* from ZZ_ZBDBXXB   where Number='" + Number + "' and BUYJSBH='" + jsbh + "' and ZBDBZT='定标执行中' order by  Number");
        if (dsreturn == null || dsreturn.Tables[0].Rows.Count < 1)
        {
            return null;
        }
        //获取默认收货地址
        DataSet dsreturn_dizhi = DbHelperSQL.Query(" select top 1 * from ZZ_SHDZXXB where JSDLZH = '" + dsreturn.Tables[0].Rows[0]["BUYDLYX"] + "' and JSBH='" + jsbh + "' and SFMRDZ = '是' order by  Number ");
        if (dsreturn_dizhi != null && dsreturn_dizhi.Tables[0].Rows.Count > 0)
        {
            dsreturn.Tables[0].Rows[0]["是否存在默认地址"] = "是";
            dsreturn.Tables[0].Rows[0]["默认收货单位名称"] = dsreturn_dizhi.Tables[0].Rows[0]["SHDWMC"].ToString();
            dsreturn.Tables[0].Rows[0]["默认收货人姓名"] = dsreturn_dizhi.Tables[0].Rows[0]["SHRXM"].ToString();
            dsreturn.Tables[0].Rows[0]["默认省"] = dsreturn_dizhi.Tables[0].Rows[0]["SZSF"].ToString();
            dsreturn.Tables[0].Rows[0]["默认市"] = dsreturn_dizhi.Tables[0].Rows[0]["SZDS"].ToString();
            dsreturn.Tables[0].Rows[0]["默认区"] = dsreturn_dizhi.Tables[0].Rows[0]["SZQX"].ToString();
            dsreturn.Tables[0].Rows[0]["默认详细地址"] = dsreturn_dizhi.Tables[0].Rows[0]["XXDZ"].ToString();
            dsreturn.Tables[0].Rows[0]["默认邮政编码"] = dsreturn_dizhi.Tables[0].Rows[0]["YZBM"].ToString();
            dsreturn.Tables[0].Rows[0]["默认联系电话"] = dsreturn_dizhi.Tables[0].Rows[0]["LXDH"].ToString();
        }
        else
        {
            dsreturn.Tables[0].Rows[0]["是否存在默认地址"] = "否";
        }

        //获取开票类型和开票信息
        DataSet dsreturn_userinfo = DbHelperSQL.Query(" select top 1 * from ZZ_BUYJBZLXXB where JSBH = '" + jsbh + "' order by  Number ");
        if (dsreturn_userinfo != null && dsreturn_userinfo.Tables[0].Rows.Count > 0)
        {
            dsreturn.Tables[0].Rows[0]["开票类型"] = dsreturn_userinfo.Tables[0].Rows[0]["YXKPLX"].ToString();
            dsreturn.Tables[0].Rows[0]["开票信息"] = dsreturn_userinfo.Tables[0].Rows[0]["YXKPXX"].ToString();
        }

        return dsreturn;
    }





    /// <summary>
    /// 发货
    /// </summary>
    /// <param name="dsinfo">发货提交参数</param>
    /// <returns></returns>
    public string FaHuo(DataSet dsinfo)
    {
        //基本验证
        string selJSBH = dsinfo.Tables[0].Rows[0]["卖家角色编号"].ToString();
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(selJSBH);

        if (selJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
        {
            return "录入发货信息失败，您尚未开通结算账户!";
        }
        if (UserInfo["是否允许登录"].ToString() != "是")
        {
            return "录入发货信息失败，您已被禁止登录!";
        }
        if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
        {
            return "录入发货信息失败，当前时间业务停止运行!";
        }
        if (UserInfo["是否休眠"].ToString() == "是")
        {
            return "录入发货信息失败，您的账号已休眠!";
        }
        //if (UserInfo["是否冻结"].ToString() == "是")
        //{
        //    return "录入发货信息失败，您的账号已被禁止业务!";
        //}
        
        
        //验证是否可以发货
        DataSet dsFHinfo = DbHelperSQL.Query(" select top 1 '状态'=THDZT from ZZ_THDXXB where Number = '" + dsinfo.Tables[0].Rows[0]["发货单号"].ToString() + "' and SELJSBH = '" + selJSBH + "' ");
        if (dsFHinfo.Tables[0].Rows[0][0].ToString() != "待发货")
        {
            return "录入发货信息失败，您只能对处于待发货状态的提货单进行发货!";
        }


        //更新提货单发货信息
        string nowtimestr = DateTime.Now.ToString();
        ArrayList alsql = new ArrayList();
        alsql.Add("update  ZZ_THDXXB set SELFHSJ='" + nowtimestr + "',CYWLGSMC='" + dsinfo.Tables[0].Rows[0]["物流公司"].ToString() + "',WLDH='" + dsinfo.Tables[0].Rows[0]["物流单号"].ToString() + "',FPH='" + dsinfo.Tables[0].Rows[0]["发票号码"].ToString() + "',THDZT='已发货待签收'  where Number = '" + dsinfo.Tables[0].Rows[0]["发货单号"].ToString() + "' and SELJSBH = '" + selJSBH + "' ");

      

        bool th = DbHelperSQL.ExecSqlTran(alsql);
        if (th)
        {
            return "ok";
        }
        else
        {
            return "录入发货信息失败，意外错误！";
        }

      
    }




    /// <summary>
    /// 签收
    /// </summary>
    /// <param name="Number">发货单号</param>
    /// <param name="BUYjsbh">买家角色编号</param>
    /// <returns></returns>
    public string QianShou(string Number, string BUYjsbh)
    {
        //基本验证
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(BUYjsbh);

        if (BUYjsbh.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
        {
            return "签收失败，您尚未开通结算账户!";
        }
        if (UserInfo["是否允许登录"].ToString() != "是")
        {
            return "签收失败，您已被禁止登录!";
        }
        if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
        {
            return "签收失败，当前时间业务停止运行!";
        }
        if (UserInfo["是否休眠"].ToString() == "是")
        {
            return "签收失败，您的账号已休眠!";
        }
        //if (UserInfo["是否冻结"].ToString() == "是")
        //{
        //    return "签收失败，您的账号已被禁止业务!";
        //}


    


        //更新签收信息
        string nowtimestr = DateTime.Now.ToString();
        ArrayList alsql = new ArrayList();
        alsql.Add("update  ZZ_THDXXB set BUYQSSJ='" + nowtimestr + "',THDZT='已签收'  where Number = '" + Number + "' and BUYJSBH = '" + BUYjsbh + "' ");

        DataSet dsQS = DbHelperSQL.Query("select * from ZZ_THDXXB  where Number = '" + Number + "' and BUYJSBH = '" + BUYjsbh + "' ");

        Hashtable htParameterInfo = jhjx_PublicClass.GetParameterInfo();
        ClassMoney CM = new ClassMoney();
        double kyMoney = CM.GetMoneyT(dsQS.Tables[0].Rows[0]["SELDLYX"].ToString(), "模拟而已");

       
        //插入资金变更明细表SQl
        Hashtable htinfo = new Hashtable();
        htinfo["来源单号"] = Number;
        htinfo["变动金额_货款"] = dsQS.Tables[0].Rows[0]["THZJE"].ToString();
        htinfo["变动金额_技术服务费"] = (Convert.ToDouble(htinfo["变动金额_货款"]) * Convert.ToDouble(htParameterInfo["签收技术服务费"])).ToString("#.##");
        htinfo["变动前账户余额"] = kyMoney;//这个从调用点传入

        
        ArrayList almoney = CM.InsertMoneyUse("签收", htinfo, dsQS.Tables[0].Rows[0]["SELJSBH"].ToString());
        alsql.AddRange(almoney);



        bool th = DbHelperSQL.ExecSqlTran(alsql);
        if (th)
        {
            //执行成功，调用银行接口，调用两次，一次加，一次减
            CM.FreezeMoneyT(dsQS.Tables[0].Rows[0]["SELDLYX"].ToString(), htinfo["变动金额_货款"].ToString(), "+");
            CM.FreezeMoneyT(dsQS.Tables[0].Rows[0]["SELDLYX"].ToString(), htinfo["变动金额_技术服务费"].ToString(), "-");
            return "ok";
        }
        else
        {
            return "签收失败，意外错误！";
        }


    }

}