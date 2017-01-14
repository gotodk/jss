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
/// 资金处理类，处理所有的资金变动与银行接口
/// </summary>
public class ClassMoney
{
	public ClassMoney()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 模拟获取用户托管账户中可用余额(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="bankinfo">银行接口的相关信息</param>
    /// <returns></returns>
    public double GetMoneyT(string dlyx,string bankinfo)
    {
        //接口未开发前，用登录表模拟银行托管账户余额
        DataSet dsmoney = DbHelperSQL.Query("select isnull(ZHDQKYYE,0.00) from ZZ_UserLogin where DLYX='" + dlyx + "'");
        if (dsmoney != null && dsmoney.Tables[0].Rows.Count == 1)
        {
            return Convert.ToDouble(dsmoney.Tables[0].Rows[0][0]);
        }
        else
        {  
            return 0.00;
        }
        
    }

    /// <summary>
    /// 模拟入金，将用户绑定卡中的资金，转入托管账户中(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    public bool UserToFMT(string temp)
    {
        return true;
    }

    /// <summary>
    /// 模拟出金，将用户托管账户中的资金，转到用户绑定卡中(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    public bool FMTToUser(string temp)
    {
        return true;
    }

    /// <summary>
    /// 模拟冻结或解冻托管账户中的用户资金(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="dlyx"></param>
    /// <param name="money"></param>
    /// <param name="IsF">+、-</param>
    /// <returns></returns>
    public bool FreezeMoneyT(string dlyx,string money, string IsF)
    {
        DbHelperSQL.ExecuteSql(" update ZZ_UserLogin set ZHDQKYYE = ZHDQKYYE " + IsF + " " + money + " where DLYX='" + dlyx + "'");
        return true;
    }

    /// <summary>
    /// 模拟从托管账户，转账至平台主账户(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    public bool FMTToFMZ(string temp)
    {
        return true;
    }
    /// <summary>
    /// 模拟从平台主账户，转账至托管账户(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    public bool FMZToFMT(string temp)
    {
        return true;
    }

    /// <summary>
    /// 根据参数，返回一条流水明细插入语句
    /// </summary>
    /// <param name="htinfo"></param>
    /// <returns></returns>
    private string ReturnSQLstr_MX(Hashtable htinfo)
    {
        return "INSERT INTO ZZ_ZKLSMXB ([Number],[DLYX],[YHM],[JSZHLX],[JSBH],[JSLX],[LYYWLX],[LYDH],[ZJLX],[SYLX],[BDJE],[JEYSLX],[CSSJ],[BDQZHYE],[BDHZHYE],[BGSM],[YHZHXTSJBH],[BZ],[CreateTime]) VALUES ('" + htinfo["唯一编号"].ToString() + "','" + htinfo["登录邮箱"].ToString() + "','" + htinfo["用户名"].ToString() + "','" + htinfo["结算账户类型"].ToString() + "','" + htinfo["角色编号"].ToString() + "','" + htinfo["角色类型"].ToString() + "','" + htinfo["来源业务类型"].ToString() + "','" + htinfo["来源单号"].ToString() + "','" + htinfo["资金类型"].ToString() + "','" + htinfo["使用类型"].ToString() + "'," + htinfo["变动金额"].ToString() + ",'" + htinfo["金额运算类型"].ToString() + "','" + htinfo["产生时间"].ToString() + "'," + htinfo["变动前账户余额"].ToString() + "," + htinfo["变动后账户余额"].ToString() + ",'" + htinfo["变更说明"].ToString() + "','" + htinfo["银行账户系统数据编号"].ToString() + "','" + htinfo["备注"].ToString() + "','" + htinfo["产生时间"].ToString() + "') ";
    }

    /// <summary>
    /// 根据款项触发点，生成相应插入语句
    /// </summary>
    /// <param name="usetype">款项触发点</param>
    /// <param name="htinfo">触发点需要的参数</param>
    /// <returns></returns>
    public ArrayList InsertMoneyUse(string usetype,Hashtable htinfo,string jsbh)
    {
        Hashtable htuserinfo = jhjx_PublicClass.GetUserInfo(jsbh);
        ArrayList alreturn = new ArrayList(); //普通返回值
        DataTable alreturnSPSPDT = new DataTable("SPSP"); //用户特殊返回，不仅要返回语句，还有其他的东西
        alreturnSPSPDT.Columns.Add("邮箱");
        alreturnSPSPDT.Columns.Add("增加金额");
        switch (usetype)
        {
            case "解除休眠":
                #region 解除休眠

                //缴纳账户管理费
                htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["产生时间"] = DateTime.Now.ToString();

                htinfo["登录邮箱"] = htuserinfo["登录邮箱"];
                htinfo["用户名"] = htuserinfo["用户名"];
                htinfo["结算账户类型"] = htuserinfo["结算账户类型"];
                htinfo["角色编号"] = jsbh;
                htinfo["角色类型"] = htuserinfo["角色类型"];

                htinfo["来源业务类型"] = "登录";
                htinfo["来源单号"] = "无";
                htinfo["资金类型"] = "账户管理费";
                htinfo["使用类型"] = "休眠";
                //htinfo["变动金额"] = "";//这个从调用点传入
                htinfo["金额运算类型"] = "扣减";
                //htinfo["变动前账户余额"] = "";//这个从调用点传入
                //htinfo["变动后账户余额"] = "";//这个从调用点传入
                htinfo["变更说明"] = "连续长时间未登录时缴纳账户管理费以解除休眠";
                htinfo["银行账户系统数据编号"] = "无";
                htinfo["备注"] = "无";

                alreturn.Add(ReturnSQLstr_MX(htinfo));
                #endregion
                break;
            case "提交投标信息":
                #region 提交投标信息
                //缴纳投标保证金
                htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["产生时间"] = DateTime.Now.ToString();

                htinfo["登录邮箱"] = htuserinfo["登录邮箱"];
                htinfo["用户名"] = htuserinfo["用户名"];
                htinfo["结算账户类型"] = htuserinfo["结算账户类型"];
                htinfo["角色编号"] = jsbh;
                htinfo["角色类型"] = htuserinfo["角色类型"];

                htinfo["来源业务类型"] = "ZZ_TBXXB";
                //htinfo["来源单号"] = ""; //这个从调用点传入
                htinfo["资金类型"] = "投标保证金";
                htinfo["使用类型"] = "投标";
                //htinfo["变动金额"] = "";//这个从调用点传入
                htinfo["金额运算类型"] = "冻结";
                //htinfo["变动前账户余额"] = "";//这个从调用点传入
                //htinfo["变动后账户余额"] = "";//这个从调用点传入
                htinfo["变更说明"] = "";
                htinfo["银行账户系统数据编号"] = "无";
                htinfo["备注"] = "无";

                alreturn.Add(ReturnSQLstr_MX(htinfo));

                #endregion

                break;
            case "提交预订单":
                #region  提交预订单
                //缴纳订金
                 htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["产生时间"] = DateTime.Now.ToString();

                htinfo["登录邮箱"] = htuserinfo["登录邮箱"];
                htinfo["用户名"] = htuserinfo["用户名"];
                htinfo["结算账户类型"] = htuserinfo["结算账户类型"];
                htinfo["角色编号"] = jsbh;
                htinfo["角色类型"] = htuserinfo["角色类型"];

                htinfo["来源业务类型"] = "ZZ_YDDXXB";
                //htinfo["来源单号"] = ""; //这个从调用点传入
                htinfo["资金类型"] = "订金";
                htinfo["使用类型"] = "预订";
                //htinfo["变动金额"] = "";//这个从调用点传入
                htinfo["金额运算类型"] = "冻结";
                //htinfo["变动前账户余额"] = "";//这个从调用点传入
                //htinfo["变动后账户余额"] = "";//这个从调用点传入
                htinfo["变更说明"] = "";
                htinfo["银行账户系统数据编号"] = "无";
                htinfo["备注"] = "无";

                alreturn.Add(ReturnSQLstr_MX(htinfo));
                #endregion
                break;
            case "修改投标信息":
                #region 修改投标信息
                //重算投标保证金
                htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["产生时间"] = DateTime.Now.ToString();

                htinfo["登录邮箱"] = htuserinfo["登录邮箱"];
                htinfo["用户名"] = htuserinfo["用户名"];
                htinfo["结算账户类型"] = htuserinfo["结算账户类型"];
                htinfo["角色编号"] = jsbh;
                htinfo["角色类型"] = htuserinfo["角色类型"];

                htinfo["来源业务类型"] = "ZZ_TBXXB";
                //htinfo["来源单号"] = ""; //这个从调用点传入
                htinfo["资金类型"] = "投标保证金";
                htinfo["使用类型"] = "投标修改";
                //htinfo["变动金额"] = "";//这个从调用点传入
                //htinfo["金额运算类型"] = "冻结";
                //htinfo["变动前账户余额"] = "";//这个从调用点传入
                //htinfo["变动后账户余额"] = "";//这个从调用点传入
                //htinfo["变更说明"] = "";
                htinfo["银行账户系统数据编号"] = "无";
                htinfo["备注"] = "无";
                alreturn.Add(ReturnSQLstr_MX(htinfo));

                #endregion


                break;
            case "修改预订单":

                #region  修改预订单
                //重算订金
                htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["产生时间"] = DateTime.Now.ToString();

                htinfo["登录邮箱"] = htuserinfo["登录邮箱"];
                htinfo["用户名"] = htuserinfo["用户名"];
                htinfo["结算账户类型"] = htuserinfo["结算账户类型"];
                htinfo["角色编号"] = jsbh;
                htinfo["角色类型"] = htuserinfo["角色类型"];

                htinfo["来源业务类型"] = "ZZ_YDDXXB";
                //htinfo["来源单号"] = ""; //这个从调用点传入
                htinfo["资金类型"] = "订金";
                htinfo["使用类型"] = "预订单修改";
                //htinfo["变动金额"] = "";//这个从调用点传入
                //htinfo["金额运算类型"] = "冻结";
                //htinfo["变动前账户余额"] = "";//这个从调用点传入
                //htinfo["变动后账户余额"] = "";//这个从调用点传入
                //htinfo["变更说明"] = "";
                htinfo["银行账户系统数据编号"] = "无";
                htinfo["备注"] = "无";
                alreturn.Add(ReturnSQLstr_MX(htinfo));
                #endregion
                break;
            case "撤销投标信息":

                #region 撤销投标信息
                //返还投标保证金
                htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["产生时间"] = DateTime.Now.ToString();

                htinfo["登录邮箱"] = htuserinfo["登录邮箱"];
                htinfo["用户名"] = htuserinfo["用户名"];
                htinfo["结算账户类型"] = htuserinfo["结算账户类型"];
                htinfo["角色编号"] = jsbh;
                htinfo["角色类型"] = htuserinfo["角色类型"];

                htinfo["来源业务类型"] = "ZZ_TBXXB";
                //htinfo["来源单号"] = ""; //这个从调用点传入
                htinfo["资金类型"] = "投标保证金";
                htinfo["使用类型"] = "投标撤销";
                //htinfo["变动金额"] = "";//这个从调用点传入
                htinfo["金额运算类型"] = "返还";
                //htinfo["变动前账户余额"] = "";//这个从调用点传入
                //htinfo["变动后账户余额"] = "";//这个从调用点传入
                htinfo["变更说明"] = "撤销投标信息返还保证金";
                htinfo["银行账户系统数据编号"] = "无";
                htinfo["备注"] = "无";
                alreturn.Add(ReturnSQLstr_MX(htinfo));
                #endregion
                break;
            case "撤销预订单":
                #region  撤销预订单
                //返还订金
                htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["产生时间"] = DateTime.Now.ToString();

                htinfo["登录邮箱"] = htuserinfo["登录邮箱"];
                htinfo["用户名"] = htuserinfo["用户名"];
                htinfo["结算账户类型"] = htuserinfo["结算账户类型"];
                htinfo["角色编号"] = jsbh;
                htinfo["角色类型"] = htuserinfo["角色类型"];

                htinfo["来源业务类型"] = "ZZ_YDDXXB";
                //htinfo["来源单号"] = ""; //这个从调用点传入
                htinfo["资金类型"] = "订金";
                htinfo["使用类型"] = "预订单撤销";
                //htinfo["变动金额"] = "";//这个从调用点传入
                htinfo["金额运算类型"] = "返还";
                //htinfo["变动前账户余额"] = "";//这个从调用点传入
                //htinfo["变动后账户余额"] = "";//这个从调用点传入
                htinfo["变更说明"] = "撤销预订单返还订金";
                htinfo["银行账户系统数据编号"] = "无";
                htinfo["备注"] = "无";
                alreturn.Add(ReturnSQLstr_MX(htinfo));
                #endregion
                break;
            case "定标确认":

                #region 定标确认
                //返还投标保证金
                htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["产生时间"] = DateTime.Now.ToString();

                htinfo["登录邮箱"] = htuserinfo["登录邮箱"];
                htinfo["用户名"] = htuserinfo["用户名"];
                htinfo["结算账户类型"] = htuserinfo["结算账户类型"];
                htinfo["角色编号"] = jsbh;
                htinfo["角色类型"] = htuserinfo["角色类型"];

                htinfo["来源业务类型"] = "ZZ_TBXXB|ZZ_TBXXBZB";
                //htinfo["来源单号"] = ""; //这个从调用点传入
                htinfo["资金类型"] = "投标保证金";
                htinfo["使用类型"] = "定标";
                htinfo["变动金额"] = htinfo["变动金额_投标保证金"];//这个从调用点传入
                htinfo["金额运算类型"] = "返还";
                //htinfo["变动前账户余额"] = "";//这个从调用点传入
                htinfo["变动后账户余额"] = (Convert.ToDouble(htinfo["变动前账户余额"]) + Convert.ToDouble(htinfo["变动金额"])).ToString();//这个从调用点传入
                htinfo["变更说明"] = "";
                htinfo["银行账户系统数据编号"] = "无";
                htinfo["备注"] = "无";
                //如果投标保证金变动金额是0，代表不释放投标保证金，不生成语句
                if (Convert.ToDouble(htinfo["变动金额"]) != 0)
                {
                    alreturn.Add(ReturnSQLstr_MX(htinfo));
                }
                //缴纳履约保证金
                htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["资金类型"] = "履约保证金";
                htinfo["变动金额"] = htinfo["变动金额_履约保证金"];//这个从调用点传入
                htinfo["金额运算类型"] = "冻结";
                htinfo["变动前账户余额"] = htinfo["变动后账户余额"];//这个从调用点传入
                htinfo["变动后账户余额"] = (Convert.ToDouble(htinfo["变动前账户余额"]) - Convert.ToDouble(htinfo["变动金额"])).ToString();//这个从调用点传入
                htinfo["变更说明"] = "";
                htinfo["银行账户系统数据编号"] = "无";
                htinfo["备注"] = "无";
                alreturn.Add(ReturnSQLstr_MX(htinfo));

                #endregion
                break;
            case "废标":
                #region 废标
                //返还并扣减投标保证金,返还订金,计算补偿
                DataSet DSFeiBiaoInfo = (DataSet)htinfo["废标数据"];
                Hashtable yueDLYX = new Hashtable();//这东西能确保金额循环计算正确
                Hashtable FCFfanhuan = new Hashtable();//这东西可以防止重复返还投标保证金，即一个投标信息主表号只返还一次

                //生成返还和扣减数据
                for (int p = 0; p < DSFeiBiaoInfo.Tables["fbgroup"].Rows.Count; p++)
                {
                    //若已对某个投标信息主表编号尚未返还过，才能进行返还
                    if (!FCFfanhuan.ContainsKey(DSFeiBiaoInfo.Tables["fbgroup"].Rows[p]["投标信息主表编号"].ToString()))
                    {
                        string thisjsbh = DSFeiBiaoInfo.Tables["fbgroup"].Rows[p]["卖家角色编号"].ToString();
                        string thisDLYX = DSFeiBiaoInfo.Tables["fbgroup"].Rows[p]["卖家登陆邮箱"].ToString();
                        Hashtable htuserinfo99 = jhjx_PublicClass.GetUserInfo(thisjsbh);

                        if (!yueDLYX.ContainsKey(thisDLYX))
                        {
                            ClassMoney CM = new ClassMoney();
                            double kyMoney = CM.GetMoneyT(thisDLYX, "模拟而已");
                            yueDLYX[thisDLYX] = kyMoney;
                        }

                        //返还投标保证金
                        htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                        htinfo["产生时间"] = DateTime.Now.ToString();

                        htinfo["登录邮箱"] = htuserinfo99["登录邮箱"];
                        htinfo["用户名"] = htuserinfo99["用户名"];
                        htinfo["结算账户类型"] = htuserinfo99["结算账户类型"];
                        htinfo["角色编号"] = DSFeiBiaoInfo.Tables["fbgroup"].Rows[p]["卖家角色编号"].ToString();
                        htinfo["角色类型"] = htuserinfo99["角色类型"];

                        htinfo["来源业务类型"] = "ZZ_TBXXB|ZZ_TBXXBZB";
                        htinfo["来源单号"] = DSFeiBiaoInfo.Tables["fbgroup"].Rows[p]["投标信息主表编号"].ToString() + "|" + DSFeiBiaoInfo.Tables["fbgroup"].Rows[p]["投标信息子表编号"].ToString();
                        htinfo["资金类型"] = "投标保证金";
                        htinfo["使用类型"] = "废标";
                        htinfo["变动金额"] = DSFeiBiaoInfo.Tables["fbgroup"].Rows[p]["当前可用投标保证金金额"].ToString();
                        htinfo["金额运算类型"] = "返还";
                        htinfo["变动前账户余额"] = yueDLYX[thisDLYX].ToString();
                        htinfo["变动后账户余额"] = (Convert.ToDouble(htinfo["变动前账户余额"]) + Convert.ToDouble(htinfo["变动金额"])).ToString();
                        yueDLYX[thisDLYX] = htinfo["变动后账户余额"];
                        htinfo["变更说明"] = "未缴纳履约保证金导致废标";
                        htinfo["银行账户系统数据编号"] = "无";
                        htinfo["备注"] = "无";
                        alreturn.Add(ReturnSQLstr_MX(htinfo));


                        //扣减投标保证金
                        htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                        htinfo["产生时间"] = DateTime.Now.ToString();

                        htinfo["来源业务类型"] = "ZZ_WGKFMXB";
                        htinfo["来源单号"] = "";
                        htinfo["资金类型"] = "违规赔付";
                        htinfo["使用类型"] = "违规";

                        htinfo["金额运算类型"] = "扣减";
                        htinfo["变动前账户余额"] = htinfo["变动后账户余额"];
                        htinfo["变动后账户余额"] = (Convert.ToDouble(htinfo["变动前账户余额"]) - Convert.ToDouble(htinfo["变动金额"])).ToString();
                        yueDLYX[thisDLYX] = htinfo["变动后账户余额"];
                        htinfo["变更说明"] = "投标信息号：“" + DSFeiBiaoInfo.Tables["fbgroup"].Rows[p]["投标信息主表编号"].ToString() + "_" + DSFeiBiaoInfo.Tables["fbgroup"].Rows[p]["投标信息子表编号"].ToString() + "”。处罚类型：“未缴纳履约保证金导致废标”。罚金来源：“投标保证金”。";

                        alreturn.Add(ReturnSQLstr_MX(htinfo));
                        FCFfanhuan[DSFeiBiaoInfo.Tables["fbgroup"].Rows[p]["投标信息主表编号"].ToString()] = "投标保证金返过了,标记一下";

                    }

                }

                //返还订金和补偿
                Hashtable htParameterInfo = jhjx_PublicClass.GetParameterInfo();
                Hashtable FCFpeichang = new Hashtable();//这东西能确保防止重复赔偿
                Hashtable FCFdingjin = new Hashtable();//这东西能确保防止重复返还订金

                for (int p = 0; p < DSFeiBiaoInfo.Tables["ds"].Rows.Count; p++)
                {
                    string thisjsbh = DSFeiBiaoInfo.Tables["ds"].Rows[p]["BUYJSBH"].ToString();
                    string thisDLYX = DSFeiBiaoInfo.Tables["ds"].Rows[p]["BUYDLYX"].ToString();
                    if (!yueDLYX.ContainsKey(thisDLYX))
                    {
                        ClassMoney CM = new ClassMoney();
                        double kyMoney = CM.GetMoneyT(thisDLYX, "模拟而已");
                        yueDLYX[thisDLYX] = kyMoney;
                    }
          

                    //对应投标信息子表中，是状态竞标的，强制撤销那些数据
                    alreturn.Add("update ZZ_TBXXBZB set ZT='撤销',SPBZ='因废标而导致的强制撤销' where parentNumber = '" + DSFeiBiaoInfo.Tables["ds"].Rows[p]["SELTBXXZBH"].ToString() + "' and ZT='竞标'");
                    //中标定标信息表中，主表号对应的，状态是“中标”的，不管是否到期，全部强制废标。
                    string fbtime = DateTime.Now.ToString();
                    alreturn.Add("update ZZ_ZBDBXXB set ZBDBZT='未定标废标',FBSJ='" + fbtime + "',SPBZ='因废标而导致的强制废标' where SELTBXXZBH = '" + DSFeiBiaoInfo.Tables["ds"].Rows[p]["SELTBXXZBH"].ToString() + "' and ZBDBZT='中标'");

                    //要返还的订金,废标的投标信息主表号名下所有中标均需要返还
                    //主表编号没处理过的，才进行处理
                    if (!FCFdingjin.ContainsKey(DSFeiBiaoInfo.Tables["ds"].Rows[p]["SELTBXXZBH"].ToString()))
                    {
                        DataSet ds_buchang = DbHelperSQL.Query(" select AAAA.Number,AAAA.SELJSBH,AAAA.BUYJSBH,'需返还订金金额'=(select ROUND(convert(float,YYY.NMRJG)*convert(float,YYY.ZFDJBL)*convert(float,AAAA.ZBSL),2) from ZZ_YDDXXB as YYY where YYY.Number = AAAA.BUYYDDBH ) from  ZZ_ZBDBXXB AS AAAA where  AAAA.SELTBXXZBH = '" + DSFeiBiaoInfo.Tables["ds"].Rows[p]["SELTBXXZBH"].ToString() + "' and AAAA.ZBDBZT='中标' ");

                        for (int h = 0; h < ds_buchang.Tables[0].Rows.Count; h++)
                        {
                            Hashtable htuserinfo777 = jhjx_PublicClass.GetUserInfo(ds_buchang.Tables[0].Rows[h]["BUYJSBH"].ToString());

                            string thisDLYX_n = htuserinfo777["登录邮箱"].ToString();
                            if (!yueDLYX.ContainsKey(thisDLYX_n))
                            {
                                ClassMoney CM = new ClassMoney();
                                double kyMoney = CM.GetMoneyT(thisDLYX_n, "模拟而已");
                                yueDLYX[thisDLYX_n] = kyMoney;
                            }

                            htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                            htinfo["产生时间"] = DateTime.Now.ToString();

                            htinfo["登录邮箱"] = htuserinfo777["登录邮箱"];
                            htinfo["用户名"] = htuserinfo777["用户名"];
                            htinfo["结算账户类型"] = htuserinfo777["结算账户类型"];
                            htinfo["角色编号"] = ds_buchang.Tables[0].Rows[h]["BUYJSBH"].ToString();
                            htinfo["角色类型"] = htuserinfo777["角色类型"];

                            htinfo["来源业务类型"] = "ZZ_ZBDBXXB";
                            htinfo["来源单号"] = ds_buchang.Tables[0].Rows[h]["Number"].ToString();
                            htinfo["资金类型"] = "订金";
                            htinfo["使用类型"] = "废标";
                            htinfo["变动金额"] = Math.Round(Convert.ToDouble(ds_buchang.Tables[0].Rows[h]["需返还订金金额"]),2).ToString();
                            alreturnSPSPDT.Rows.Add(new string[] { htuserinfo777["登录邮箱"].ToString(), htinfo["变动金额"].ToString() });
                            htinfo["金额运算类型"] = "返还";
                            htinfo["变动前账户余额"] = yueDLYX[thisDLYX_n].ToString();
                            htinfo["变动后账户余额"] = (Convert.ToDouble(htinfo["变动前账户余额"]) + Convert.ToDouble(htinfo["变动金额"])).ToString();
                            yueDLYX[thisDLYX_n] = htinfo["变动后账户余额"];
                            htinfo["变更说明"] = "未缴纳履约保证金导致废标";
                            htinfo["银行账户系统数据编号"] = "无";
                            htinfo["备注"] = "无";
                            alreturn.Add(ReturnSQLstr_MX(htinfo));
                        }

                        FCFdingjin[DSFeiBiaoInfo.Tables["ds"].Rows[p]["SELTBXXZBH"].ToString()] = "订金返还过了,标记一下";
                    }



                    //主表编号没处理过的，才进行处理
                    if (!FCFpeichang.ContainsKey(DSFeiBiaoInfo.Tables["ds"].Rows[p]["SELTBXXZBH"].ToString()))
                    {
                        //要补偿的金额(买家往流水表写被补偿记录，卖家往处罚表里面写处罚记录)
                        //要补偿的列表
                        DataSet ds_buchang = DbHelperSQL.Query(" select AAAA.Number,AAAA.SELJSBH,AAAA.BUYJSBH,'投标保证金金额'=isnull((select isnull(PPP.TBBZJJE,0) from  ZZ_TBXXB as PPP  where PPP.Number = AAAA.SELTBXXZBH),0),'补偿百分比'=ROUND(convert(float,AAAA.ZBZJE)/convert(float,(select isnull(sum(Z.ZBZJE),0) from ZZ_ZBDBXXB as Z where Z.SELTBXXZBH = AAAA.SELTBXXZBH and ZBDBZT='中标' )),5) from  ZZ_ZBDBXXB AS AAAA where  AAAA.SELTBXXZBH = '" + DSFeiBiaoInfo.Tables["ds"].Rows[p]["SELTBXXZBH"].ToString() + "' and AAAA.ZBDBZT='中标' ");

                        for (int r = 0; r < ds_buchang.Tables[0].Rows.Count; r++)
                        {
                            

                            //买家信息
                            Hashtable htuserinfo_BUY = jhjx_PublicClass.GetUserInfo(ds_buchang.Tables[0].Rows[r]["BUYJSBH"].ToString());
                            //卖家信息
                            Hashtable htuserinfo_SEL = jhjx_PublicClass.GetUserInfo(ds_buchang.Tables[0].Rows[r]["SELJSBH"].ToString());

                            string thisDLYX_n = htuserinfo_BUY["登录邮箱"].ToString();
                            if (!yueDLYX.ContainsKey(thisDLYX_n))
                            {
                                ClassMoney CM = new ClassMoney();
                                double kyMoney = CM.GetMoneyT(thisDLYX_n, "模拟而已");
                                yueDLYX[thisDLYX_n] = kyMoney;
                            }

                            //买家往流水表写被补偿记录
                            htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                            htinfo["产生时间"] = DateTime.Now.ToString();

                            htinfo["登录邮箱"] = htuserinfo_BUY["登录邮箱"];
                            htinfo["用户名"] = htuserinfo_BUY["用户名"];
                            htinfo["结算账户类型"] = htuserinfo_BUY["结算账户类型"];
                            htinfo["角色编号"] = ds_buchang.Tables[0].Rows[r]["BUYJSBH"].ToString();
                            htinfo["角色类型"] = htuserinfo_BUY["角色类型"];

                            htinfo["来源业务类型"] = "ZZ_ZBDBXXB";
                            htinfo["来源单号"] = ds_buchang.Tables[0].Rows[r]["Number"].ToString();
                            htinfo["资金类型"] = "违规赔付";
                            htinfo["使用类型"] = "被赔付";
                            double thisPFmoney = Math.Round(Convert.ToDouble(ds_buchang.Tables[0].Rows[r]["投标保证金金额"]) * Convert.ToDouble(ds_buchang.Tables[0].Rows[r]["补偿百分比"]),2);
                            htinfo["变动金额"] = thisPFmoney;
                            alreturnSPSPDT.Rows.Add(new string[] { htuserinfo_BUY["登录邮箱"].ToString(), htinfo["变动金额"].ToString() });
                            htinfo["金额运算类型"] = "增加";
                            htinfo["变动前账户余额"] = yueDLYX[thisDLYX_n].ToString();
                            htinfo["变动后账户余额"] = (Convert.ToDouble(htinfo["变动前账户余额"]) + Convert.ToDouble(htinfo["变动金额"])).ToString();
                            yueDLYX[thisDLYX_n] = htinfo["变动后账户余额"];
                            htinfo["变更说明"] = "处罚类型：“未缴纳履约保证金导致废标”。罚金来源：“投标保证金”。";
                            htinfo["银行账户系统数据编号"] = "无";
                            htinfo["备注"] = "无";
                            alreturn.Add(ReturnSQLstr_MX(htinfo));



                            string WGnumber = jhjx_PublicClass.GetNextNumberZZ("ZZ_WGKFMXB", "");
                            alreturn.Add("INSERT INTO ZZ_WGKFMXB ([Number],[SFFDLYX],[SFFYHM],[SFFJSZHLX],[SFFJSBH],[SFFJSLX],[CFJE],[CFSJ],[FJLY],[CFLX],[SJLY],[PFDXDLYX],[PFDXYHM],[PFDXJSZHLX],[PFDXJSBH],[PFDXJSLX],[CFLYYWLX],[CFLYYWDH],[CFYYSM],[CFLYBSH],[SFYYXYHZHKYED],[CreateTime]) VALUES ('" + WGnumber + "','" + htuserinfo_SEL["登录邮箱"].ToString() + "','" + htuserinfo_SEL["用户名"].ToString() + "','" + htuserinfo_SEL["结算账户类型"].ToString() + "','" + ds_buchang.Tables[0].Rows[r]["SELJSBH"].ToString() + "','" + htuserinfo_SEL["角色类型"].ToString() + "'," + thisPFmoney + ",'" + htinfo["产生时间"].ToString() + "','投标保证金','未缴纳履约保证金导致废标','系统计算','" + htuserinfo_BUY["登录邮箱"].ToString() + "','" + htuserinfo_BUY["用户名"].ToString() + "','" + htuserinfo_BUY["结算账户类型"].ToString() + "','" + ds_buchang.Tables[0].Rows[r]["BUYJSBH"].ToString() + "','" + htuserinfo_BUY["角色类型"].ToString() + "','ZZ_ZBDBXXB','" + ds_buchang.Tables[0].Rows[r]["Number"].ToString() + "','','','是','" + htinfo["产生时间"].ToString() + "')");

                        }


                        FCFpeichang[DSFeiBiaoInfo.Tables["ds"].Rows[p]["SELTBXXZBH"].ToString()] = "补偿过了,标记一下";
                    }


                   
                   

                }
                alreturn.Add(alreturnSPSPDT);
                #endregion
                break; 
            case "下达提货单":
           
                #region  下达提货单
                //扣除货款
                htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["产生时间"] = DateTime.Now.ToString();

                htinfo["登录邮箱"] = htuserinfo["登录邮箱"];
                htinfo["用户名"] = htuserinfo["用户名"];
                htinfo["结算账户类型"] = htuserinfo["结算账户类型"];
                htinfo["角色编号"] = jsbh;
                htinfo["角色类型"] = htuserinfo["角色类型"];

                htinfo["来源业务类型"] = "ZZ_THDXXB";
                //htinfo["来源单号"] = ""; //这个从调用点传入
                htinfo["资金类型"] = "货款";
                htinfo["使用类型"] = "提货";
                //htinfo["变动金额"] = "";//这个从调用点传入
                htinfo["金额运算类型"] = "扣减";
                //htinfo["变动前账户余额"] = "";//这个从调用点传入
                //htinfo["变动后账户余额"] = "";//这个从调用点传入
                htinfo["变更说明"] = "";
                htinfo["银行账户系统数据编号"] = "无";
                htinfo["备注"] = "无";

                alreturn.Add(ReturnSQLstr_MX(htinfo));
                #endregion
                break;
            case "签收":
                //支付货款，技术服务费,延期发货惩罚,计算补偿
                #region  签收
                //支付货款
                htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["产生时间"] = DateTime.Now.ToString();

                htinfo["登录邮箱"] = htuserinfo["登录邮箱"];
                htinfo["用户名"] = htuserinfo["用户名"];
                htinfo["结算账户类型"] = htuserinfo["结算账户类型"];
                htinfo["角色编号"] = jsbh;
                htinfo["角色类型"] = htuserinfo["角色类型"];

                htinfo["来源业务类型"] = "ZZ_THDXXB";
                //htinfo["来源单号"] = ""; //这个从调用点传入
                htinfo["资金类型"] = "货款";
                htinfo["使用类型"] = "签收";
                htinfo["变动金额"] = htinfo["变动金额_货款"];//这个从调用点传入
                htinfo["金额运算类型"] = "增加";
                //htinfo["变动前账户余额"] = "";//这个从调用点传入
                htinfo["变动后账户余额"] = Convert.ToDouble(htinfo["变动前账户余额"]) + Convert.ToDouble(htinfo["变动金额"]);//这个从调用点传入
                htinfo["变更说明"] = "";
                htinfo["银行账户系统数据编号"] = "无";
                htinfo["备注"] = "无";

                alreturn.Add(ReturnSQLstr_MX(htinfo));
                //扣除技术服务费
                htinfo["唯一编号"] = jhjx_PublicClass.GetNextNumberZZ("ZZ_ZKLSMXB", "");
                htinfo["产生时间"] = DateTime.Now.ToString();

                htinfo["资金类型"] = "技术服务费";
                htinfo["使用类型"] = "签收";
                htinfo["变动金额"] = htinfo["变动金额_技术服务费"];//这个从调用点传入
                htinfo["金额运算类型"] = "扣减";
                htinfo["变动前账户余额"] = htinfo["变动后账户余额"];//这个从调用点传入
                htinfo["变动后账户余额"] = (Convert.ToDouble(htinfo["变动前账户余额"])-Convert.ToDouble(htinfo["变动金额"])).ToString();//这个从调用点传入
                alreturn.Add(ReturnSQLstr_MX(htinfo));
                #endregion

                break;
            case "履约保证金不足":
                //合同终止清算
                break;
            case "订金不足":
                //合同终止清算
            default:
                break;
        }

        return alreturn;
    }

}