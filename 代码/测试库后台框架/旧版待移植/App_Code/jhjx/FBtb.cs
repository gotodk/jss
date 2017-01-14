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
public class FBtb
{
	public FBtb()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 初始化返回值数据集
    /// </summary>
    /// <returns></returns>
    public DataSet initReturnDataSet()
    {
        DataSet ds = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.TableName = "返回值单条";
        auto2.Columns.Add("执行结果");
        auto2.Columns.Add("错误提示");

        DataTable auto3 = new DataTable();
        auto3.TableName = "返回值多条";
        auto3.Columns.Add("多条错误");
 

        ds.Tables.Add(auto3);
        ds.Tables.Add(auto2);
        return ds;
    }
    /// <summary>
    /// 仅处理基础检查
    /// </summary>
    /// <param name="dsinput"></param>
    /// <returns></returns>
    private DataSet OnlyJC(DataSet dsinput)
    {

        //初始化返回值结构
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "未知错误" });

        //数据检查
        string SellerJSBH = dsinput.Tables["主表"].Rows[0]["卖家角色编号"].ToString();
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(SellerJSBH);


        if (SellerJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您尚未开通结算账户，将不能下达投标信息！";
            return dsreturn;
        }
        if (UserInfo["是否允许登录"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您已被禁止登录，将不能下达投标信息！";
            return dsreturn;
        }
        if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "当前时间业务停止运行，将不能下达投标信息！";
            return dsreturn;
        }
        if (UserInfo["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的账号已休眠，将不能下达投标信息！";
            return dsreturn;
        }
        //if (UserInfo["是否冻结"].ToString() == "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的账号已被禁止业务，将不能下达投标信息！";
        //    return dsreturn;
        //}
        if (UserInfo["当前默认关联经纪人角色编号"].ToString() == "")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的经纪人被禁止或暂停业务，将不能下达投标信息！";
            return dsreturn;
        }
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "检查通过！";

        return dsreturn;
        
    }

    /// <summary>
    /// 发布投标信息
    /// </summary>
    /// <param name="dsinput"></param>
    /// <returns></returns>
    public DataSet BeginFBtb(DataSet dsinput)
    {
        //首次进入的检查
        if (dsinput.Tables["主表"].Rows[0]["特殊标记"].ToString() == "仅检查基础")
        {
            return OnlyJC(dsinput);
        }

        //正常提交的检查
        //初始化返回值结构
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "未知错误" });

        //数据检查
        string SellerJSBH = dsinput.Tables["主表"].Rows[0]["卖家角色编号"].ToString();
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(SellerJSBH);
   
        
        if (SellerJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，您尚未开通结算账户！";
            return dsreturn;
        }
        if (UserInfo["是否允许登录"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，您已被禁止登录！";
            return dsreturn;
        }
        if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，当前时间业务停止运行！";
            return dsreturn;
        }
        if (UserInfo["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，您的账号已休眠！";
            return dsreturn;
        }
        //if (UserInfo["是否冻结"].ToString() == "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，您的账号已被禁止业务！";
        //    return dsreturn;
        //}
        if (UserInfo["当前默认关联经纪人角色编号"].ToString() == "")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，您的经纪人被禁止或暂停业务！";
            return dsreturn;
        }
        //检查是已存在“竞标”状态的投标信息,以及是否处于冷静期
        string spbhstr = " 1=2  "; //拼接是否已竞标条件
        string lengjingqistr = " 1=2 "; //拼接冷静期条件
        for (int i = 0; i < dsinput.Tables["输入列表"].Rows.Count; i++)
        {
   
            spbhstr = spbhstr + " or (ZZ_TBXXBZB.SPBH='" + dsinput.Tables["输入列表"].Rows[i]["商品编号"].ToString() + "' and ZZ_TBXXBZB.ZT='竞标'  and ZZ_TBXXBZB.HTQX='" + dsinput.Tables["输入列表"].Rows[i]["合同期限"].ToString() + "') ";
            lengjingqistr = lengjingqistr + " or (ZZ_TBXXBZB.SPBH='" + dsinput.Tables["输入列表"].Rows[i]["商品编号"].ToString() + "' and ZZ_TBXXBZB.SFJRLJQ='是' and ZZ_TBXXBZB.HTQX='" + dsinput.Tables["输入列表"].Rows[i]["合同期限"].ToString() + "') ";
        }
        //竞标状态
        DataSet ds_pchongfu = DbHelperSQL.Query("select distinct ZZ_TBXXB.SELJSBH,ZZ_TBXXBZB.SPBH,ZZ_TBXXBZB.SPMC,ZZ_TBXXBZB.HTQX,ZZ_TBXXB.CreateTime  from ZZ_TBXXB join ZZ_TBXXBZB on ZZ_TBXXB.Number = ZZ_TBXXBZB.parentNumber where (" + spbhstr + ")  and SELJSBH='" + SellerJSBH + "'  order  by ZZ_TBXXB.CreateTime desc");
        if (ds_pchongfu != null && ds_pchongfu.Tables[0].Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err多条提示";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，您正在参与以下商品的竞标，详情如下：";
            for (int p = 0; p < ds_pchongfu.Tables[0].Rows.Count; p++)
            {
                dsreturn.Tables["返回值多条"].Rows.Add(new string[] { "商品编号：" + ds_pchongfu.Tables[0].Rows[p]["SPBH"].ToString() + ",商品名称：" + ds_pchongfu.Tables[0].Rows[p]["SPMC"].ToString() + ",合同期限：" + ds_pchongfu.Tables[0].Rows[p]["HTQX"].ToString() + ",参与竞标时间：" + ds_pchongfu.Tables[0].Rows[p]["CreateTime"].ToString() + "" });
            }
            return dsreturn;
        }
        //冷静期
        DataSet ds_plengjignqi = DbHelperSQL.Query("select distinct ZZ_TBXXBZB.SPBH,ZZ_TBXXBZB.SPMC,ZZ_TBXXBZB.HTQX  from ZZ_TBXXB join ZZ_TBXXBZB on ZZ_TBXXB.Number = ZZ_TBXXBZB.parentNumber where " + lengjingqistr + "   order  by ZZ_TBXXBZB.SPBH desc");
        if (ds_plengjignqi != null && ds_plengjignqi.Tables[0].Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err多条提示";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，以下商品已在处于冷静期，详情如下：";
            for (int p = 0; p < ds_plengjignqi.Tables[0].Rows.Count; p++)
            {
                dsreturn.Tables["返回值多条"].Rows.Add(new string[] { "商品编号：" + ds_plengjignqi.Tables[0].Rows[p]["SPBH"].ToString() + ",商品名称：" + ds_plengjignqi.Tables[0].Rows[p]["SPMC"].ToString() + ",合同期限：" + ds_plengjignqi.Tables[0].Rows[p]["HTQX"].ToString() + "" });
            }
            return dsreturn;
        }


        //检查账户余额是否足够
        Hashtable htParameterInfo = jhjx_PublicClass.GetParameterInfo();

        ClassMoney CM = new ClassMoney();
        double kyMoney = CM.GetMoneyT(UserInfo["登录邮箱"].ToString(), "模拟而已");
        double tbhzj_money = 0.00;
        double tbhzj_money_end = 0.00;
        for (int i = 0; i < dsinput.Tables["输入列表"].Rows.Count; i++)
        {
            double TBJG = Convert.ToDouble(dsinput.Tables["输入列表"].Rows[i]["投标价格"]);
            Int64 TBYSL = Convert.ToInt64(dsinput.Tables["输入列表"].Rows[i]["投标拟售量"]);
            tbhzj_money = tbhzj_money + TBJG*TBYSL;
        }
        double showzje = tbhzj_money;
        tbhzj_money = tbhzj_money * Convert.ToDouble(htParameterInfo["投标保证金比率"]);
        tbhzj_money = Math.Round(tbhzj_money, 2);
        if (tbhzj_money < Convert.ToDouble(htParameterInfo["投标保证金"]))
        {
            tbhzj_money_end = Convert.ToDouble(htParameterInfo["投标保证金"]);
        }
        else
        {
            tbhzj_money_end = tbhzj_money;
        }
        if (kyMoney < tbhzj_money_end)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，账户余额不足，投标总额" + showzje + "元，需缴纳投标保证金" + tbhzj_money_end + "元，您的账户余额为" + kyMoney + "元！";
            return dsreturn;
        }



        //一切就绪，开始保存这条投标信息
        ArrayList alsqlsave = new ArrayList();
        //插入主表SQL
        string Number = jhjx_PublicClass.GetNextNumberZZ("ZZ_TBXXB", "");
        string TBXXH = Number;
        string SELDLYX = UserInfo["登录邮箱"].ToString();
        string SELYHM = UserInfo["用户名"].ToString();
        string SELJSBH = SellerJSBH;
        string GLJJRDLYX = UserInfo["当前默认关联经纪人登陆邮箱"].ToString();
        string GLJJRYHM = UserInfo["当前默认关联经纪人用户名"].ToString();
        string GLJJRJSBH = UserInfo["当前默认关联经纪人角色编号"].ToString();
        //计算投标保证金
        double TBBZJJE = tbhzj_money_end;
        double TBBZJSYKYJE = TBBZJJE;
        string SFCX = "否";
        string CreateTime = DateTime.Now.ToString();
        alsqlsave.Add("INSERT INTO ZZ_TBXXB (Number,TBXXH,SELDLYX,SELYHM,SELJSBH,GLJJRDLYX,GLJJRYHM,GLJJRJSBH,TBBZJJE,TBBZJSYKYJE,SFCX,CreateTime) VALUES ('" + Number + "','" + TBXXH + "','" + SELDLYX + "','" + SELYHM + "','" + SELJSBH + "','" + GLJJRDLYX + "','" + GLJJRYHM + "','" + GLJJRJSBH + "'," + TBBZJJE + "," + TBBZJSYKYJE + ",'" + SFCX + "','" + CreateTime + "') ");
        //插入子表SQL
        for (int i = 0; i < dsinput.Tables["输入列表"].Rows.Count; i++)
        {
            string parentNumber = Number;
            string SPBH = dsinput.Tables["输入列表"].Rows[i]["商品编号"].ToString();
            string SPMC = dsinput.Tables["输入列表"].Rows[i]["商品名称"].ToString();
            string GGXH = dsinput.Tables["输入列表"].Rows[i]["规格型号"].ToString();
            Hashtable HTSPInfo = jhjx_PublicClass.GetSPInfo(SPBH);
            string JJDW = HTSPInfo["计价单位"].ToString();
            int JJPL = Convert.ToInt32(dsinput.Tables["输入列表"].Rows[i]["提货经济批量"].ToString());
            double TBJG =  Convert.ToDouble(dsinput.Tables["输入列表"].Rows[i]["投标价格"]);
            Int64 TBYSL =  Convert.ToInt64(dsinput.Tables["输入列表"].Rows[i]["投标拟售量"]);
            string ZT = "竞标";
            string JRLJQSJ = "null";
            string SFJRLJQ = "否"; //由于投标信息不能在冷静期发布新的，因此不需要判断冷静期了

            int XGCS = 0;

            string BGHQY = dsinput.Tables["输入列表"].Rows[i]["不发货区域"].ToString();
            string HTQX = dsinput.Tables["输入列表"].Rows[i]["合同期限"].ToString();

            string ZLBZFJ = dsinput.Tables["输入列表"].Rows[i]["质量标准附件"].ToString();
            string PGFZRCNS = dsinput.Tables["输入列表"].Rows[i]["品管总负责人法律承诺书"].ToString();
            string ZLJYBG = dsinput.Tables["输入列表"].Rows[i]["产品技术与安全鉴定报告"].ToString();
            string ZZSFPSLSMZM = dsinput.Tables["输入列表"].Rows[i]["增值税发票税率书面证明"].ToString();
            string SBYZLZS = dsinput.Tables["输入列表"].Rows[i]["商标与专利证书"].ToString();
            string WHBGJCN = dsinput.Tables["输入列表"].Rows[i]["危害报告及承诺"].ToString();
            string FDDBRCNS = dsinput.Tables["输入列表"].Rows[i]["法定代表人承诺书"].ToString();

            alsqlsave.Add("INSERT INTO ZZ_TBXXBZB (parentNumber,SPBH,SPMC,GGXH,JJDW,JJPL,TBJG,TBYSL,ZT,JRLJQSJ,SFJRLJQ,XGCS,BGHQY,HTQX,ZLBZFJ,PGFZRCNS,ZLJYBG,ZZSFPSLSMZM,SBYZLZS,WHBGJCN,FDDBRCNS) VALUES ('" + parentNumber + "','" + SPBH + "','" + SPMC + "','" + GGXH + "','" + JJDW + "'," + JJPL + "," + TBJG + "," + TBYSL + ",'" + ZT + "'," + JRLJQSJ + ",'" + SFJRLJQ + "'," + XGCS + ",'" + BGHQY + "','" + HTQX + "','" + ZLBZFJ + "','" + PGFZRCNS + "','" + ZLJYBG + "','" + ZZSFPSLSMZM + "','" + SBYZLZS + "','" + WHBGJCN + "','" + FDDBRCNS + "')");
        }
        //插入资金变更明细表SQl
        Hashtable htinfo = new Hashtable();
        htinfo["来源单号"] = Number;
        htinfo["变动金额"] = tbhzj_money_end;
        htinfo["变动前账户余额"] = kyMoney;
        htinfo["变动后账户余额"] = kyMoney - tbhzj_money_end;
        ArrayList almoney = CM.InsertMoneyUse("提交投标信息", htinfo, SELJSBH);


        //保存投标信息和流水明细，执行SQL
        alsqlsave.AddRange(almoney);
        bool bb = DbHelperSQL.ExecSqlTran(alsqlsave);


        //处理成交规则，看是否需要进入冷静期
        CJGZ cjgz = new CJGZ();
        ArrayList alsql = new ArrayList();
        alsql = cjgz.RunCJGZ(dsinput.Tables["输入列表"], "提交");
        if (alsql != null && alsql.Count > 0)
        {
            DbHelperSQL.ExecSqlTran(alsql);
        }

        if (bb)
        {
            //执行成功，调用银行接口扣减额度
            CM.FreezeMoneyT(SELDLYX, tbhzj_money_end.ToString(), "-");

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "本次投标信息提交成功！";
        }
        else 
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，发生意外错误！";
        }

        return dsreturn;
    }










    /// <summary>
    /// 撤销或修改投标信息
    /// </summary>
    /// <param name="Number"></param>
    /// <param name="id"></param>
    /// <param name="sp"></param>
    /// <param name="sl"></param>
    /// <param name="jg"></param>
    /// <returns></returns>
    public string delOredit(string Number, string id, string sp, string sl, string jg, string SellerJSBH)
    {
        //数据检查
   
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(SellerJSBH);

        if (SellerJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
        {
            return sp + "失败，您尚未开通结算账户！";
        }
        if (UserInfo["是否允许登录"].ToString() != "是")
        {
            return sp + "失败，您已被禁止登录！";
        }
        if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
        {
            return sp + "提交失败，当前时间业务停止运行！";
        }
        if (UserInfo["是否休眠"].ToString() == "是")
        {
            return sp + "失败，您的账号已休眠！";
        }
        //if (UserInfo["是否冻结"].ToString() == "是")
        //{
        //    return sp + "失败，您的账号已被禁止业务！";
        //}


        string sfjrljq = "";
        string spbh = "商品编号";
        string htzq = "合同期限";
        //验证是否处于冷静期;
        DataSet ds_pchongfu = DbHelperSQL.Query(" select top 1 * from ZZ_TBXXBZB as Z where Z.id ='" + id + "' and  Z.parentNumber = '" + Number + "' and Z.ZT = '竞标' ");
        if (ds_pchongfu != null && ds_pchongfu.Tables[0].Rows.Count > 0)
        {
            sfjrljq = ds_pchongfu.Tables[0].Rows[0]["SFJRLJQ"].ToString();
            spbh = ds_pchongfu.Tables[0].Rows[0]["SPBH"].ToString();
            htzq = ds_pchongfu.Tables[0].Rows[0]["HTQX"].ToString();
        }
        else
        {
            return "意外错误";
        }

        if (sfjrljq == "是")
        {
            return "此投标信息已进入冷静期，无法修改或撤销！";
        }

        if (sp == "撤销")
        {
          


            ArrayList alsql = new ArrayList();
            //更新状态
            alsql.Add("update ZZ_TBXXBZB set ZT='撤销'  where id ='" + id + "' and  parentNumber = '" + Number + "' and ZT = '竞标' and SFJRLJQ ='否' ");

            //确定是否需要返还投保保证金
            DataSet ds_shifoufanhuan = DbHelperSQL.Query("select '导致投标保证金不能返还的数据量'=COUNT(ID)  from ZZ_TBXXBZB where  parentNumber='" + Number + "' and (ZT='竞标' or ZT='中标')  and id<>" + id + " ");
            //要返还
            ClassMoney CM = new ClassMoney();
            DataSet ds_old_jia = null;
            if (Convert.ToInt32(ds_shifoufanhuan.Tables[0].Rows[0]["导致投标保证金不能返还的数据量"]) == 0)
            {

                //返还投标保证金
                ds_old_jia = DbHelperSQL.Query("select isnull(TBBZJSYKYJE,0) from ZZ_TBXXB where Number = '" + Number + "' ");//获得原来的投标保证金金额
                
                double kyMoney = CM.GetMoneyT(UserInfo["登录邮箱"].ToString(), "模拟而已");
                //插入资金变更明细表SQl(增加)
                Hashtable htinfo = new Hashtable();
                htinfo["来源单号"] = Number;
                htinfo["变动金额"] = ds_old_jia.Tables[0].Rows[0][0].ToString();
                htinfo["变动前账户余额"] = kyMoney;
                htinfo["变动后账户余额"] = kyMoney + Convert.ToDouble(ds_old_jia.Tables[0].Rows[0][0]);
                ArrayList almoney = CM.InsertMoneyUse("撤销投标信息", htinfo, SellerJSBH);
                alsql.AddRange(almoney);
            }


            bool bb = DbHelperSQL.ExecSqlTran(alsql);
           

            //跑一遍，看是否进入冷静期
            CJGZ cjgz = new CJGZ();
            DataTable dsinput = new DataTable();
            dsinput.TableName = "输入列表";
            dsinput.Columns.Add("商品编号");
            dsinput.Columns.Add("合同期限");
            dsinput.Rows.Add(new string[] { spbh, htzq });
            ArrayList alsql2 = new ArrayList();
            alsql2 = cjgz.RunCJGZ(dsinput, "撤销");
            if (alsql2 != null && alsql2.Count > 0)
            {
                DbHelperSQL.ExecSqlTran(alsql2);
            }
            if (bb)
            {
                if (Convert.ToInt32(ds_shifoufanhuan.Tables[0].Rows[0]["导致投标保证金不能返还的数据量"]) == 0)
                {
                    //执行成功，调用银行接口，
                    CM.FreezeMoneyT(UserInfo["登录邮箱"].ToString(), ds_old_jia.Tables[0].Rows[0][0].ToString(), "+");
                }
                return "ok";
            }
            else
            {
                return "撤销失败，意外错误！";
            }
        }

        if (sp == "修改")
        {
       

            //更新
            ArrayList alsql = new ArrayList();
            alsql.Add("update ZZ_TBXXBZB set TBJG=" + jg + ",TBYSL=" + sl + "  where id ='" + id + "' and  parentNumber = '" + Number + "' and ZT = '竞标' and SFJRLJQ ='否' ");

            //重新计算投标保证金=======================================
            Hashtable htParameterInfo = jhjx_PublicClass.GetParameterInfo();
            ClassMoney CM = new ClassMoney();
            double kyMoney = CM.GetMoneyT(UserInfo["登录邮箱"].ToString(), "模拟而已");
            DataSet dsold = DbHelperSQL.Query("select isnull(sum(TBJG*TBYSL),0) from ZZ_TBXXBZB where parentNumber = '" + Number + "' and id <> " + id + "");
            DataSet ds_old_jia = DbHelperSQL.Query("select isnull(TBBZJSYKYJE,0) from ZZ_TBXXB where Number = '" + Number + "' ");//获得原来的投标保证金金额


            double tbhzj_money = Convert.ToDouble(dsold.Tables[0].Rows[0][0]) + Convert.ToDouble(jg) * Convert.ToDouble(sl);
            double tbhzj_money_end = 0.00; //修改后新的投标保证金金额
            double showzje = tbhzj_money;
            tbhzj_money = tbhzj_money * Convert.ToDouble(htParameterInfo["投标保证金比率"]);
            tbhzj_money = Math.Round(tbhzj_money, 2);
            if (tbhzj_money < Convert.ToDouble(htParameterInfo["投标保证金"]))
            {
                tbhzj_money_end = Convert.ToDouble(htParameterInfo["投标保证金"]);
            }
            else
            {
                tbhzj_money_end = tbhzj_money;
            }
            double bujiao = tbhzj_money_end - Convert.ToDouble(ds_old_jia.Tables[0].Rows[0][0]);
            if (bujiao > 0 && kyMoney < bujiao)//需要补交
            {
                return "提交失败，账户余额不足，投标总额" + showzje + "元，需补充缴纳投标保证金" + bujiao + "元，您的账户余额为" + kyMoney + "元！";
            }
          

            //资金充足了，解冻原来的保证金，并重新冻结新的保证金
            alsql.Add("update ZZ_TBXXB set TBBZJJE=" + tbhzj_money_end + ",TBBZJSYKYJE=" + tbhzj_money_end + "  where Number = '" + Number + "' ");

            
            //插入资金变更明细表SQl(增加)
            Hashtable htinfo = new Hashtable();
            htinfo["来源单号"] = Number;
            htinfo["变动金额"] = ds_old_jia.Tables[0].Rows[0][0].ToString();
            htinfo["变动前账户余额"] = kyMoney;
            htinfo["变动后账户余额"] = kyMoney + Convert.ToDouble(ds_old_jia.Tables[0].Rows[0][0]);
            htinfo["金额运算类型"] = "返还";
            htinfo["变更说明"] = "修改投标信息返还原保证金";
            ArrayList almoney = CM.InsertMoneyUse("修改投标信息", htinfo, SellerJSBH);
            alsql.AddRange(almoney);
            //插入资金变更明细表SQl(减少)
            Hashtable htinfo2 = new Hashtable();
            htinfo2["来源单号"] = Number;
            htinfo2["变动金额"] = tbhzj_money_end;
            htinfo2["变动前账户余额"] = kyMoney + Convert.ToDouble(ds_old_jia.Tables[0].Rows[0][0]);
            htinfo2["变动后账户余额"] = kyMoney + Convert.ToDouble(ds_old_jia.Tables[0].Rows[0][0]) - tbhzj_money_end;
            htinfo2["金额运算类型"] = "冻结";
            htinfo2["变更说明"] = "修改投标信息重新冻结保证金";
            ArrayList almoney2 = CM.InsertMoneyUse("修改投标信息", htinfo2, SellerJSBH);
            alsql.AddRange(almoney2);



            bool bb = DbHelperSQL.ExecSqlTran(alsql);
 

            //跑一遍，看是否进入冷静期
            CJGZ cjgz = new CJGZ();
            DataTable dsinput = new DataTable();
            dsinput.TableName = "输入列表";
            dsinput.Columns.Add("商品编号");
            dsinput.Columns.Add("合同期限");
            dsinput.Rows.Add(new string[] { spbh, htzq });
            ArrayList alsql2 = new ArrayList();
            alsql2 = cjgz.RunCJGZ(dsinput, "修改");
            if (alsql2 != null && alsql2.Count > 0)
            {
                DbHelperSQL.ExecSqlTran(alsql2);
            }
            if (bb)
            {
                //执行成功，调用银行接口，调用两次，一次加，一次减
                CM.FreezeMoneyT(UserInfo["登录邮箱"].ToString(), ds_old_jia.Tables[0].Rows[0][0].ToString(), "+");
                CM.FreezeMoneyT(UserInfo["登录邮箱"].ToString(), tbhzj_money_end.ToString(), "-");
                return "ok";
            }
            else
            {
                return "撤销失败，意外错误！";
            }
        }
        return "意外错误";
    }



    /// <summary>
    /// 缴纳履约保证金
    /// </summary>
    /// <param name="zhubiao">投标信息主表编号</param>
    /// <param name="zibiao">投标信息子表编号</param>
    /// <param name="zibiao">角色编号</param>
    /// <returns>返回结果</returns>
    public string PayLYHZJ_Run(string zhubiao, string zibiao, string jsbh)
    {
        //数据检查

        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(jsbh);

        if (jsbh.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
        {
            return "缴纳履约保证金失败，您尚未开通结算账户！";
        }
        if (UserInfo["是否允许登录"].ToString() != "是")
        {
            return "缴纳履约保证金失败，您已被禁止登录！";
        }
        if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
        {
            return "缴纳履约保证金提交失败，当前时间业务停止运行！";
        }
        if (UserInfo["是否休眠"].ToString() == "是")
        {
            return "缴纳履约保证金失败，您的账号已休眠！";
        }
        //if (UserInfo["是否冻结"].ToString() == "是")
        //{
        //    return "缴纳履约保证金失败，您的账号已被禁止业务！";
        //}
        string t = DateTime.Now.ToString();

        //检查可用余额是否充足
        Hashtable htParameterInfo = jhjx_PublicClass.GetParameterInfo();

        //确定是否需要释放投保保证金
        bool keyishifang;
        DataSet ds_shifoufanhuan = DbHelperSQL.Query("select '导致投标保证金不能返还的数据量'=COUNT(AAA.ID)  from ZZ_TBXXBZB AS AAA where AAA.parentNumber='" + zhubiao + "'  and  ( AAA.ZT='竞标' or ( AAA.ZT='中标' and (select BBB.ZBDBZT from ZZ_ZBDBXXB as BBB where BBB.SELTBXXZBH=AAA.parentNumber and BBB.MJTBXXChildBH=AAA.id)='中标'  ) )  and AAA.id<>" + zibiao + " ");
        if (Convert.ToInt32(ds_shifoufanhuan.Tables[0].Rows[0]["导致投标保证金不能返还的数据量"]) == 0)
        {
            keyishifang = true;
        }
        else
        {
            keyishifang = false;
        }

        //需要释放才执行
        double tbhzj = 0;
        if (keyishifang)
        {
            //待释放的投标保证金金额
            DataSet ds_tbhzj = DbHelperSQL.Query("   select TBBZJJE from  ZZ_TBXXB  where Number = '" + zhubiao + "'  ");
            tbhzj = Convert.ToDouble(ds_tbhzj.Tables[0].Rows[0]["TBBZJJE"]);
        }

        //待缴纳的履约保证金金额
        double lvbzj = 0.00;
        DataSet ds_lvbzj = DbHelperSQL.Query("select '投标信息主表编号'=B.SELTBXXZBH,'投标信息子表编号'=B.MJTBXXChildBH,'标的号'=B.SELTBXXZBH+'_'+B.MJTBXXChildBH,'商品编号'=B.SPBH,'商品名称'=B.SPMC,'型号规格'=B.GGXH,'计价单位'=B.JJDW,'供货周期'=B.HTQX, '中标集合预订量'=sum(B.ZBSL),'中标价格'=B.SELTBJG,'中标金额'=sum(B.ZBZJE),'中标买家数量'=count(B.BUYJSBH),'中标时间'=B.ZBSJ,'履约保证金'=B.DQLYBZJJE from ZZ_ZBDBXXB as B where B.ZBDBZT='中标'  and B.SELTBXXZBH='" + zhubiao + "' and B.MJTBXXChildBH='" + zibiao + "'  and  B.SELJSBH='" + jsbh + "'  group by B.SELTBXXZBH,B.MJTBXXChildBH,B.SPBH,B.SPMC,B.GGXH,B.JJDW,B.HTQX,B.SELTBJG,B.ZBSJ,B.DQLYBZJJE");
        lvbzj = Convert.ToDouble(ds_lvbzj.Tables[0].Rows[0]["履约保证金"]);

        ClassMoney CM = new ClassMoney();
        double kyMoney = CM.GetMoneyT(UserInfo["登录邮箱"].ToString(), "模拟而已");
        //需要释放才执行
        if (keyishifang)
        {
            double chazhi = lvbzj - tbhzj;
            if (kyMoney < chazhi)
            {
                return "可用余额不足，当前账户余额" + kyMoney + "元， 投标保证金将返还" + tbhzj + "元，履约保证金需缴纳" + lvbzj + "元，合计需缴纳" + chazhi + "元！";
            }
        }
        else
        {
            if (kyMoney < lvbzj)
            {
                return "可用余额不足，当前账户余额" + kyMoney + "元，履约保证金需缴纳" + lvbzj + "元！";
            }
        }

        

        

        //更新中标状态
        ArrayList alsql = new ArrayList();
        alsql.Add("update ZZ_ZBDBXXB set ZBDBZT='定标执行中',DBSJ='" + t + "'  where ZBDBZT='中标' and SELTBXXZBH='" + zhubiao + "' and MJTBXXChildBH='" + zibiao + "'  and  SELJSBH='" + jsbh + "' ");

        //释放投标保证金并冻结履约保证金
        //插入资金变更明细表SQl
        Hashtable htinfo = new Hashtable();
        htinfo["来源单号"] = zhubiao + "|" + zibiao;
        htinfo["变动金额_投标保证金"] = tbhzj.ToString();
        htinfo["变动金额_履约保证金"] = lvbzj.ToString();
        htinfo["变动前账户余额"] = kyMoney;
        ArrayList almoney = CM.InsertMoneyUse("定标确认", htinfo, jsbh);
        alsql.AddRange(almoney);

        bool bb = DbHelperSQL.ExecSqlTran(alsql);
        if (bb)
        {
            //执行成功，调用银行接口，
            //需要释放才执行
            if (keyishifang)
            {
                CM.FreezeMoneyT(UserInfo["登录邮箱"].ToString(), tbhzj.ToString(), "+");
            }
            CM.FreezeMoneyT(UserInfo["登录邮箱"].ToString(), lvbzj.ToString(), "-");
            return "ok";
        }
        else
        {
            return "缴纳履约保证金失败，意外错误！";
        }
    }

}