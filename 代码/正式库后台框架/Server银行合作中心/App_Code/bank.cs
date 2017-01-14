using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;

[WebService(Namespace = "http://bank.ipc.com/", Description = "V1.00->银行合作中心业务类接口")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class bank : System.Web.Services.WebService
{
    public bank()
    {

        //如果使用设计的组件，请取消注释以下行
        //InitializeComponent(); 
    }

    /// <summary>
    /// 测试该接口是否还活着(每个接口必备)
    /// </summary>
    /// <param name="temp">随便传</param>
    /// <returns>返回ok就是接口正常</returns>
    [WebMethod(Description = "测试该接口是否还活着(每个接口必备)")]
    public string onlinetest(string temp)
    {
        //根据不同的传入值，后续可以检查不同的东西，比如这个接口所连接的数据库，比如进程池，服务器空间等等。。。
        return "ok";
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

    [WebMethod(MessageName = "多银行框架-客户预制定签约", Description = "客户预制定签约")]
    public DataSet DsSendQianYue(DataSet ds)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        DataRow Drow = ds.Tables[0].Rows[0];


        Hashtable ht = new Hashtable();
        ht["DLYX"] = Drow["用户邮箱"].ToString();
        Hashtable htre = JHJX_YanZhengClass.SFTJKTSQ(ht);
        if (htre["info"].ToString().Equals("ok"))
        {
            if (!htre["第三方存管状态是否开通"].ToString().Equals("否"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "已绑定第三方存管银行！";
                return dsreturn;
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htre["info"].ToString();
            return dsreturn;
        }

        IBank bank = new IBank(Drow["用户邮箱"].ToString(), "签约预指定");
        string info = string.Empty;
        DataSet dsResult = bank.Invoke(ds, ref info);
        if (info.Equals("ok"))
        {
            dsreturn = dsResult;
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = info;
        }

        return dsreturn;
    }

    /// <summary>
    /// 用户银行存款与用户账户之间的资金转账
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName="多银行框架-券商发起资金转账",Description = "用户多银行框架--资金转账")]
    public DataSet TransferAccounts(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "操作失败！" });

        if (dt == null || dt.Rows.Count < 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递参数不可为空！";
            return dsreturn;
        }

        //条件验证
        Hashtable ht = new Hashtable();
        ht["DLYX"] = dt.Rows[0]["用户邮箱"].ToString();
        ht["JYZJMM"] = dt.Rows[0]["交易资金密码"].ToString();
        Hashtable htreul = JHJX_YanZhengClass.SFTJKTSQ(ht);
        if (!htreul["info"].ToString().Equals("ok"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreul["info"].ToString();
            return dsreturn;
        }


        if (!htreul["是否提交开通申请"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账申请，请及时提交！";
            return dsreturn;
        }

        if (htreul["是否提交开通申请"].ToString().Equals("审核中"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！";
            return dsreturn;
        }

        if (htreul["是否提交开通申请"].ToString().Equals("驳回"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请！";
            return dsreturn;
        }


        #region  2014.09.02 wyh 模拟运行期间暂时去掉
        //if (!htreul["交易资金密码正确"].ToString().Equals("是"))
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请输入正确交易资金密码！";
        //    return dsreturn;
        //}
        #endregion
        if (!htreul["第三方存管状态是否开通"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！";
            return dsreturn;
        }

        if (!htreul["是否交易时间内"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }
        if (!htreul["是否交易日期内"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易日期，暂停交易！";
            return dsreturn;
        }


        IBank IB = null;

        if (dt.Rows[0]["转账类别"].ToString().Equals("银转商"))
        {
            IB = new IBank(dt.Rows[0]["用户邮箱"].ToString(), "入金");

        }
        else if (dt.Rows[0]["转账类别"].ToString().Equals("商转银"))
        {
            if (!htreul["是否休眠"].ToString().Equals("否"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                return dsreturn;
            }
            if (!htreul["当前账户是否冻结出金"].ToString().Equals("否"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！被冻结功能：" + htreul["被冻结项"].ToString();
                return dsreturn;
            }

            if (!htreul["是否存在两次出金失败未处理记录"].ToString().Equals("否"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "银行转账系统正在抢修，请耐心等待！";
                return dsreturn;
            }

            IB = new IBank(dt.Rows[0]["用户邮箱"].ToString(), "出金");

        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "转账类别不正确";
            return dsreturn;
        }

        //if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
        //    return dsreturn;
        //}
        DataSet dspara = new DataSet();
        dspara.Tables.Add(dt);
        DataSet dsresult = IB.Invoke(dspara);
        if (dsresult != null && dsresult.Tables[0].Rows.Count > 0)
        {
            dsreturn = dsresult;
        }

        //解锁
        //jhjx_Lock.LockEnd_ByUser("全局锁");

        return dsreturn;
    }



    [WebMethod(MessageName="多银行框架-银行发起客户签约", Description = "适用于多银行框架--银行发起的客户签约处理")]
    public DataSet CompareUInfo(DataSet ds, string khbh, string ssbank)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input = new Hashtable();
        input["@khbh"] = khbh;
        //根据传过来的客户编号，获取客户邮箱
        string sql_userinfo = "select b_dlyx from AAA_DLZHXXB where I_ZQZJZH=@khbh";
        Hashtable ht_userinfo = I_DBL.RunParam_SQL(sql_userinfo, "data", input);
        DataSet ds_userinfo = new DataSet();
        if ((bool)ht_userinfo["return_float"])
        {
            ds_userinfo = (DataSet)ht_userinfo["return_ds"];
        }
        else
        {
            ds_userinfo = null;
        }
        if (ds_userinfo == null || ds_userinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号不存在！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
        }
        else if (ds_userinfo.Tables[0].Rows.Count != 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = " 交易方编号重复！";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
        }
        else
        {
            Hashtable ht = new Hashtable();
            ht["DLYX"] = ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
            Hashtable htre = JHJX_YanZhengClass.SFTJKTSQ(ht);

            if (htre["第三方存管状态是否开通"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "资金账户与银行方账户已建立对应关系";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2014";

                return dsreturn;
            }

            //if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
            //{
            //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
            //    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
            //    return dsreturn;
            //}

            IBank IB = new IBank(ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "客户签约确认");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = strinfo;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }

            //解锁
            //jhjx_Lock.LockEnd_ByUser("全局锁");
        }
        return dsreturn;
    }

    /// <summary>
    /// wyh add 2014.04.16
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="khbh"></param>
    /// <returns></returns>
    [WebMethod(MessageName="多银行框架-银行查客户余额", Description = "适用于多银行框架--查询客户余额（银行发起）")]
    public DataSet GetUMoneyByBANK(DataSet ds, string khbh, string ssbank)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input = new Hashtable();
        input["@khbh"] = khbh;

        //根据传过来的客户编号，获取客户邮箱
        string sql_userinfo = "select b_dlyx from AAA_DLZHXXB where I_ZQZJZH=@khbh";
        Hashtable ht_userinfo = I_DBL.RunParam_SQL(sql_userinfo, "data", input);
        DataSet ds_userinfo = new DataSet();
        if ((bool)ht_userinfo["return_float"])
        {
            ds_userinfo = (DataSet)ht_userinfo["return_ds"];
        }
        else
        {
            ds_userinfo = null;
        }

        if (ds_userinfo == null || ds_userinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该客户不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";

        }
        else if (ds_userinfo.Tables[0].Rows.Count > 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号重复";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
        }
        else
        {
            IBank IB = new IBank(ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "券商余额查询");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = strinfo;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }

        }
        return dsreturn;
    }

    /// <summary>
    /// wyh 2014.04.16 add
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="khbh"></param>
    /// <returns></returns>
    [WebMethod(MessageName="多银行框架-银行发起银转商", Description = "适用于多银行框架--银行端发起-银转商")]
    public DataSet BankToS(DataSet ds, string khbh, string ssbank)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input = new Hashtable();
        input["@khbh"] = khbh;

        //根据传过来的客户编号，获取客户邮箱
        string sql_userinfo = "select b_dlyx from AAA_DLZHXXB where I_ZQZJZH=@khbh";
        Hashtable ht_userinfo = I_DBL.RunParam_SQL(sql_userinfo, "data", input);
        DataSet ds_userinfo = new DataSet();
        if ((bool)ht_userinfo["return_float"])
        {
            ds_userinfo = (DataSet)ht_userinfo["return_ds"];
        }
        else
        {
            ds_userinfo = null;
        }
        if (ds_userinfo == null || ds_userinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
            return dsreturn;
        }
        else if (ds_userinfo.Tables[0].Rows.Count > 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号重复";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
            return dsreturn;
        }
        else
        {

            Hashtable ht = new Hashtable();
            ht["DLYX"] = ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
            Hashtable htreul = JHJX_YanZhengClass.SFTJKTSQ(ht);
            if (!htreul["info"].ToString().Equals("ok"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreul["info"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }
            if (!htreul["是否提交开通申请"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账申请，请及时提交！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (htreul["是否提交开通申请"].ToString().Equals("审核中"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (htreul["是否提交开通申请"].ToString().Equals("驳回"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (!htreul["第三方存管状态是否开通"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2013";
                return dsreturn;
            }

            if (!htreul["是否交易时间内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2140";
                return dsreturn;
            }
            if (!htreul["是否交易日期内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易日期，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2032";
                return dsreturn;
            }


            //====加锁(个别必要的业务才加)
            if (IPC.Call("用户锁定", new object[] { ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString()})[1].ToString() != "1")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
                return dsreturn;
            }

            IBank IB = new IBank(ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "银行端入金");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = strinfo;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }

            //====解锁(与加锁成对出现)
            IPC.Call("用户解锁", new object[] { ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString() });
        }
        return dsreturn;
    }

    /// <summary>
    /// wyh add 2014.04.17 适用于多银行框架--银行端发起-商转银
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="khbh"></param>
    /// <returns></returns>
    [WebMethod(MessageName ="多银行框架-银行发起商转银", Description = "适用于多银行框架--银行端发起-商转银")]
    public DataSet SToBank(DataSet ds, string khbh, string ssbank)
    {

        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input = new Hashtable();
        input["@khbh"] = khbh;

        //根据传过来的客户编号，获取客户邮箱
        string sql_userinfo = "select b_dlyx from AAA_DLZHXXB where I_ZQZJZH=@khbh";
        Hashtable ht_userinfo = I_DBL.RunParam_SQL(sql_userinfo, "data", input);
        DataSet ds_userinfo = new DataSet();
        if ((bool)ht_userinfo["return_float"])
        {
            ds_userinfo = (DataSet)ht_userinfo["return_ds"];
        }
        else
        {
            ds_userinfo = null;
        }
        if (ds_userinfo == null || ds_userinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
            return dsreturn;
        }
        else if (ds_userinfo.Tables[0].Rows.Count > 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号重复";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
            return dsreturn;
        }
        else
        {

            Hashtable ht = new Hashtable();
            ht["DLYX"] = ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
            Hashtable htreul = JHJX_YanZhengClass.SFTJKTSQ(ht);
            if (!htreul["info"].ToString().Equals("ok"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreul["info"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }
            if (!htreul["是否提交开通申请"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账申请，请及时提交！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (htreul["是否提交开通申请"].ToString().Equals("审核中"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (htreul["是否提交开通申请"].ToString().Equals("驳回"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2011";
                return dsreturn;
            }

            if (!htreul["第三方存管状态是否开通"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2013";
                return dsreturn;
            }

            if (!htreul["是否交易时间内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2140";
                return dsreturn;
            }
            if (!htreul["是否交易日期内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易日期，暂停交易！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2032";
                return dsreturn;
            }
            if (!htreul["当前账户是否冻结出金"].ToString().Equals("否"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方已冻结出金";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }

            //====加锁(个别必要的业务才加)
            if (IPC.Call("用户锁定", new object[] { ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString() })[1].ToString() != "1")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
                return dsreturn;
            }

            IBank IB = new IBank(ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "银行端出金");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", strinfo });
                return dsreturn;
            }

            //====解锁(与加锁成对出现)
            IPC.Call("用户解锁", new object[] { ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString() });            
        }
        return dsreturn;
    }

    /// <summary>
    /// 银行端发起-变更银行账户biangengyinhangzhannghu
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "多银行框架-银行发起变更银行账户", Description = "适用于多银行框架--银行端发起-变更银行账户")]
    public DataSet BankRequest_UpdateBankAccount(DataSet ds, string khbh, string ssbank)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input = new Hashtable();
        input["@khbh"] = khbh;

        //根据传过来的客户编号，获取客户邮箱
        string sql_userinfo = "select b_dlyx from AAA_DLZHXXB where I_ZQZJZH=@khbh";
        Hashtable ht_userinfo = I_DBL.RunParam_SQL(sql_userinfo, "data", input);
        DataSet ds_userinfo = new DataSet();
        if ((bool)ht_userinfo["return_float"])
        {
            ds_userinfo = (DataSet)ht_userinfo["return_ds"];
        }
        else
        {
            ds_userinfo = null;
        }
        if (ds_userinfo == null || ds_userinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
            return dsreturn;
        }
        else if (ds_userinfo.Tables[0].Rows.Count > 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号重复";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
            return dsreturn;
        }
        else
        {
            //验证是否已开通第三方结算账户
            Hashtable ht = new Hashtable();
            ht["DLYX"] = ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
            Hashtable htre = JHJX_YanZhengClass.SFTJKTSQ(ht);

            if (!htre["第三方存管状态是否开通"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未绑定第三方存管银行！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2013";
                return dsreturn;
            }

            //if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
            //{
            //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
            //    dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
            //    return dsreturn;
            //}

            IBank IB = new IBank(ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "银行端换卡");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", strinfo, "8888" });
                return dsreturn;
            }

            //解锁
            //jhjx_Lock.LockEnd_ByUser("全局锁");
        }
        return dsreturn;
    }

    /// <summary>
    /// 券商端发起-变更银行账户
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName="多银行框架-券商发起变更银行账户", Description = "适用于多银行框架--券商端发起-变更银行账户")]
    public DataSet SRequest_UpdateBankAccount(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //条件验证
        Hashtable ht = new Hashtable();
        ht["DLYX"] = dt.Rows[0]["用户邮箱"].ToString();

        Hashtable htreul = JHJX_YanZhengClass.SFTJKTSQ(ht);
        if (!htreul["info"].ToString().Equals("ok"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htreul["info"].ToString();
            return dsreturn;
        }
        if (!htreul["是否提交开通申请"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账申请，请及时提交！";
            return dsreturn;
        }

        if (htreul["是否提交开通申请"].ToString().Equals("审核中"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！";
            return dsreturn;
        }

        if (htreul["是否提交开通申请"].ToString().Equals("驳回"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请！";
            return dsreturn;
        }

        if (!htreul["第三方存管状态是否开通"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前未绑定第三方存管银行，请到相关银行开通！";
            return dsreturn;
        }

        if (!htreul["是否交易时间内"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }
        if (!htreul["是否交易日期内"].ToString().Equals("是"))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易日期，暂停交易！";
            return dsreturn;
        }

        //if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
        //    return dsreturn;
        //}

        IBank IB = new IBank(dt.Rows[0]["用户邮箱"].ToString(), "变更帐户");
        //if (jhjx_Lock.LockBegin_ByUser("全局锁") != "1") //加锁
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙（锁）";
        //    return dsreturn;
        //}

        DataSet dspars = new DataSet();
        dspars.Tables.Add(dt);
        DataSet dsresult = IB.Invoke(dspars);
        if (dsresult != null && dsresult.Tables[0].Rows.Count > 0)
        {
            dsreturn = dsresult;
        }

        //解锁
        //jhjx_Lock.LockEnd_ByUser("全局锁");

        return dsreturn;
    }

    /// <summary>
    /// 监控，冲证券转银行23704,wyh add
    /// </summary>
    /// <param name="dt">参数列表</param> 
    /// <returns>监控，冲证券转银行23704</returns>

    [WebMethod(MessageName="多银行框架-监控冲商转银", Description = "适用于多银行框架--冲证券转银行23704")]
    public DataSet CSToBank(DataSet ds, string khbh, string ssbank)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input = new Hashtable();
        input["@khbh"] = khbh;

        //根据传过来的客户编号，获取客户邮箱
        string sql_userinfo = "select b_dlyx from AAA_DLZHXXB where I_ZQZJZH=@khbh";
        Hashtable ht_userinfo = I_DBL.RunParam_SQL(sql_userinfo, "data", input);
        DataSet ds_userinfo = new DataSet();
        if ((bool)ht_userinfo["return_float"])
        {
            ds_userinfo = (DataSet)ht_userinfo["return_ds"];
        }
        else
        {
            ds_userinfo = null;
        }
        if (ds_userinfo == null || ds_userinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该客户不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
            return dsreturn;

        }
        else if (ds_userinfo.Tables[0].Rows.Count > 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "客户编号重复";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
            return dsreturn;
        }
        else
        {
            //验证是否已开通第三方结算账户
            Hashtable ht = new Hashtable();
            ht["DLYX"] = ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
            Hashtable htre = JHJX_YanZhengClass.SFTJKTSQ(ht);

            if (!htre["info"].ToString().Equals("ok"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htre["info"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }

            if (!htre["第三方存管状态是否开通"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未绑定第三方存管银行！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2013";
                return dsreturn;
            }

            if (!htre["是否交易时间内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2140";
                return dsreturn;
            }

            if (!htre["是否交易日期内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统交易日期不符";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
                return dsreturn;
            }

            //====加锁(个别必要的业务才加)
            if (IPC.Call("用户锁定", new object[] { ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString() })[1].ToString() != "1")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
                return dsreturn;
            }

            IBank IB = new IBank(ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "冲银行端出金");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", strinfo });
                return dsreturn;
            }

            //====解锁(与加锁成对出现)
            IPC.Call("用户解锁", new object[] { ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString()});
        }

        return dsreturn;

    }

    /// <summary>
    /// 监控，冲银行转证券23703  银行端发起
    /// </summary>
    /// <param name="dt">参数列表</param> 
    /// <returns> </returns>
    [WebMethod(MessageName="多银行框架-监控冲银转商", Description = "适用于多银行框架--冲银行转证券23703")]
    public DataSet CBankToS(DataSet ds, string khbh, string ssbank)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input = new Hashtable();
        input["@khbh"] = khbh;

        //根据传过来的客户编号，获取客户邮箱
        string sql_userinfo = "select b_dlyx from AAA_DLZHXXB where I_ZQZJZH=@khbh";
        Hashtable ht_userinfo = I_DBL.RunParam_SQL(sql_userinfo, "data", input);
        DataSet ds_userinfo = new DataSet();
        if ((bool)ht_userinfo["return_float"])
        {
            ds_userinfo = (DataSet)ht_userinfo["return_ds"];
        }
        else
        {
            ds_userinfo = null;
        }
        if (ds_userinfo == null || ds_userinfo.Tables[0].Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号不存在";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
        }
        else if (ds_userinfo.Tables[0].Rows.Count > 1)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易方编号重复";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
        }
        else
        {

            //验证是否已开通第三方结算账户
            Hashtable ht = new Hashtable();
            ht["DLYX"] = ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString();
            Hashtable htre = JHJX_YanZhengClass.SFTJKTSQ(ht);

            if (!htre["info"].ToString().Equals("ok"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = htre["info"].ToString();
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }

            if (!htre["第三方存管状态是否开通"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未绑定第三方存管银行！";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2013";
                return dsreturn;
            }

            if (!htre["是否交易时间内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2140";
                return dsreturn;
            }

            if (!htre["是否交易日期内"].ToString().Equals("是"))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统交易日期不符";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "2302";
                return dsreturn;
            }

            //====加锁(个别必要的业务才加)
            if (IPC.Call("用户锁定", new object[] {ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString()})[1].ToString() != "1")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙";
                return dsreturn;
            }
            IBank IB = new IBank(ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString(), "冲银行端入金");
            string strinfo = ssbank;
            dsreturn = IB.Invoke(ds, ref strinfo);
            if (!strinfo.Equals("ok"))
            {
                dsreturn = initReturnDataSet().Clone();
                dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = strinfo;
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "8888";
                return dsreturn;
            }
            //====解锁(与加锁成对出现)
            IPC.Call("用户解锁", new object[] { ds_userinfo.Tables[0].Rows[0]["b_dlyx"].ToString()});
        }
        return dsreturn;
    }

    /// <summary>
    /// 判断银行闭式时间外是否存在数据
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName="浦发银行-闭市时间外是否存在数据",Description = "判断银行闭式时间外是否存在数据")]
    public DataSet dsishav(DataTable ht)
    {
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        I_Dblink I_DBL=(new DBFactory()).DbLinkSqlMain("");

        string Strbakdnam = ht.Rows[0]["开户银行"].ToString(); ;
        string Strjssj = "";
        string Strsql = "select PTMRJSFWSJ from AAA_PTDTCSSDB";
        Hashtable htres_bs = I_DBL.RunProc(Strsql, "data");
        if ((bool)htres_bs["return_float"])
        {
            DataSet ds_bs = (DataSet)htres_bs["return_ds"];
            if (ds_bs != null && ds_bs.Tables[0].Rows.Count > 0)
            {
                if (ds_bs.Tables[0].Rows[0]["PTMRJSFWSJ"].ToString() != "")
                {
                    Strjssj = ds_bs.Tables[0].Rows[0]["PTMRJSFWSJ"].ToString() + ":00.000";
                }
            }
        }       
        //object objbs = DbHelperSQL.GetSingle(Strsql);
        //if (objbs != null && !objbs.ToString().Trim().Equals(""))
        //{
        //    Strjssj = objbs.ToString().Trim() + ":00.000";
        //}
        //string Sqlhav = "select COUNT(a.Number)  from AAA_ZKLSMXB  as a left join AAA_DLZHXXB as b on ( a.JSBH = b.J_BUYJSBH or a.JSBH = b.J_JJRJSBH or a.JSBH = b.J_SELJSBH )  where (a.LSCSSJ between '" + ht.Rows[0]["结算时间"].ToString().Trim() + " " + Strjssj.Trim() + "'  and  '" + ht.Rows[0]["结算时间"].ToString().Trim() + " 23:59:59.999' ) and b.I_KHYH = '" + Strbakdnam + "'";
        //object obj = DbHelperSQL.GetSingle(Sqlhav);
        //if (obj != null && !obj.ToString().Trim().Equals("0") && !obj.ToString().Trim().Equals(""))
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "have";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账户流水明细表中存在 时间段：" + ht.Rows[0]["结算时间"].ToString().Trim() + " " + Strjssj.Trim() + " 至 \n\r" + ht.Rows[0]["结算时间"].ToString().Trim() + " 23:59:59.999'" + "的流水数据" + obj + "条，是否继续统计！";
        //    return dsreturn;
        //}
        //else
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "pass";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不存在不合法数据，你可以正常统计！";
        //    return dsreturn;
        //}

        Hashtable input = new Hashtable();
        input["@StartTime"] = ht.Rows[0]["结算时间"].ToString().Trim() + Strjssj.Trim();
        input["@EndTime"] = ht.Rows[0]["结算时间"].ToString().Trim() + " 23:59:59.999";
        input["@khyh"] = Strbakdnam;

        string Sqlhav = "select COUNT(a.Number) as 总数量  from AAA_ZKLSMXB  as a left join AAA_DLZHXXB as b on ( a.JSBH = b.J_BUYJSBH or a.JSBH = b.J_JJRJSBH or a.JSBH = b.J_SELJSBH )  where (a.LSCSSJ between @StartTime  and  @EndTime ) and b.I_KHYH = @khyh";
        Hashtable ht_hav = I_DBL.RunParam_SQL(Sqlhav, "data", input);
        DataSet ds_hav = new DataSet();
        if ((bool)ht_hav["return_float"])
        {
            ds_hav = (DataSet)ht_hav["return_ds"];
        }
        else
        {
            ds_hav = null;
        }
        if (ds_hav != null && ds_hav.Tables[0].Rows.Count > 0)
        {
            string total = ds_hav.Tables[0].Rows[0]["总数量"].ToString();
            if (total != "" && total != "0")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "have";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "账户流水明细表中存在 时间段：" + ht.Rows[0]["结算时间"].ToString().Trim() + " " + Strjssj.Trim() + " 至 \n\r" + ht.Rows[0]["结算时间"].ToString().Trim() + " 23:59:59.999'" + "的流水数据" + total + "条，是否继续统计！";                
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "pass";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不存在不合法数据，你可以正常统计！";          
        }
        return dsreturn;
    }

    /// <summary>
    /// 存管客户交易资金净额清算文件 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "浦发银行-获取交易资金净额清算数据", Description = "获取存管客户交易资金净额清算文件数据")]
    public DataSet Getcgkhjyzijefile(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });


        //开始执行     TBD_GetCGinfo
        if (1 == 1) //添加工作日条件限制
        {
            jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
            dsreturn = bankmethed.Getcgkhjyzijefile(dsreturn, dt);

        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不在文件上传时间段内无法上传文件！";

        }
        return dsreturn;
    }
   
    /// <summary>
    /// 存管客户批量利息入帐文件 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName="浦发银行-获取批量利息入帐数据", Description = "存管客户批量利息入帐文件")]
    public DataSet Getcgkhpllx(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });



        //开始执行     TBD_GetCGinfo
        if (1 == 1) //添加工作日条件限制
        {
            jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
            dsreturn = bankmethed.Getcgkhpllx(dsreturn, dt);
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不在文件上传时间段内无法上传文件！";
        }

        return dsreturn;
    }
    
    /// <summary>
    /// 存管客户资金余额明细文件 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName="浦发银行-获取资金余额明细", Description = "存管客户资金余额明细文件")]
    public DataSet Getcgkhzjmc(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //开始执行     TBD_GetCGinfo
        if (1 == 1) //添加工作日条件限制
        {
            jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
            dsreturn = bankmethed.Getcgkhzjmc(dsreturn, dt);
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不在文件上传时间段内无法上传文件！";
        }
        return dsreturn;
    }
    
    /// <summary>
    /// 存管汇总账户资金交收汇总文件格式 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "浦发银行-存管汇总账户资金交收汇总文件格式", Description = "存管汇总账户资金交收汇总文件格式")]
    public DataSet Getcghzzhzjjs(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //开始执行     TBD_GetCGinfo
        if (1 == 1) //添加工作日条件限制
        {
            jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
            dsreturn = bankmethed.Getcghzzhzjjs(dsreturn, dt);
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不在文件上传时间段内无法上传文件！";
        }
        return dsreturn;
    }

    [WebMethod(MessageName = "浦发银行-获取结算文件需发送数据", Description = "获取此次结算文件需发送数据信息")]
    public DataSet GetStrwhere(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //开始执行     TBD_GetCGinfo
        if (1 == 1) //添加工作日条件限制
        {
            jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
            dsreturn = bankmethed.GetSqlwhere(dsreturn, dt);
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "不在文件上传时间段内无法上传文件！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 获取银证转账对账数据
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName="浦发银行-获取银证转账对账数据", Description = "获取银证转账对账数据")]
    public DataSet GetYHZZXX(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
        dsreturn = bankmethed.GetYZZZXX(dsreturn, dt);
        return dsreturn;
    }
 
    /// <summary>
    /// 执行每日清算
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    [WebMethod(MessageName="浦发银行-日终闭市清算", Description = "对每天银行闭市后的数据进行清算")]
    public DataSet WCQS(DataTable dt)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        jhjx_greatbankfile bankmethed = new jhjx_greatbankfile();
        dsreturn = bankmethed.WCQS(dsreturn, dt);
        return dsreturn;
    }

    [WebMethod(MessageName = "平安银行-签到签退", Description = "平安银行--签到签退")]
    public DataSet PingAn_Sign(string type)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        BankPingan bkp = new BankPingan();
        try
        {
            dsreturn = bkp.Sign(type, dsreturn);
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用sign方法失败！" + ex.ToString();
        }
        return dsreturn;
    }

    /// <summary>
    /// 平安银行-通过子账户账号获取交易网会员代码
    /// </summary>
    /// <param name="CustAcctId">子账户账号</param>
    /// <returns></returns>
    [WebMethod(MessageName = "平安银行-获取客户编号", Description = "平安银行-通过子账户账号获取交易网会员代码（即客户编号）")]
    public DataSet PingAn_GetCustId(string CustAcctId)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new object[] { "err", "初始化" });

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable input = new Hashtable();
        input["@CustAcctId"] = CustAcctId;
        string sql = "select * from AAA_PingAnBank where CustAcctId=@CustAcctId";
        Hashtable ht_res=I_DBL.RunParam_SQL(sql,"data",input );
        DataSet dsinfo = new DataSet();
        if ((bool)ht_res["return_float"])
        {
            dsinfo = (DataSet)ht_res["return_ds"];
        }
        else
        {
            dsinfo = null;
        }
        if (dsinfo != null && dsinfo.Tables[0].Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = dsinfo.Tables[0].Rows[0]["ThirdCustId"].ToString();
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "子账户不存在";
        }
        return dsreturn;
    }

    [WebMethod(MessageName="平安银行-开销户流水匹配", Description = "平安银行--开销户流水匹配")]
    public DataSet PingAn_AcctAndMnyMatch(DataTable dtParams)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        BankPingan bkp = new BankPingan();
        try
        {
            dsreturn = bkp.AcctAndMnyMatch(dtParams, dsreturn);
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用AccountMatch方法失败！" + ex.ToString();
        }
        return dsreturn;
    }

    [WebMethod(MessageName = "平安银行-对账数据处理", Description = "平安银行--对账数据处理")]
    public DataSet PingAn_DataMatch(DataTable dtParams, string FuncId, string filename)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        BankPingan bkp = new BankPingan();
        try
        {
            dsreturn = bkp.DataProcess(dtParams, FuncId, filename, dsreturn);
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用DataMatch方法失败！" + ex.ToString();
        }
        return dsreturn;
    }
    /// <summary>
    /// 可以提前获取的一些数据或参数
    /// </summary>
    /// <returns>返回数据集</returns>
    [WebMethod(MessageName = "平安银行-获取清算数据", Description = "平安银行--获取清算数据")]
    public DataSet GetPingAnQSDsData(string date)
    {
        BankPingan BankPinganQS = new BankPingan();
        return BankPinganQS.GetQSDataSource(date);
    }

    /// <summary>
    /// 发起清算请求
    /// </summary>
    /// <returns>返回数据集</returns>
    [WebMethod(MessageName = "平安银行-发起清算请求", Description = "平安银行--发起清算请求")]
    public DataSet PingAn_RunQingSuan(DataTable dsParams)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        BankPingan bkp = new BankPingan();
        try
        {
            //1004接口查询银行处理进度
            dsreturn = bkp.RunQingSuan(dsParams, dsreturn);
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用RunQingSuan方法失败！" + ex.ToString();
        }
        return dsreturn;
    }

    /// <summary>
    /// 平安银行-查询银行清算进度
    /// </summary>
    /// <param name="type">查询业务类型</param>
    /// <param name="date">日期</param>
    /// <returns>查询结果数据集</returns>
    [WebMethod(MessageName = "平安银行-查询银行清算进度", Description = "平安银行--查询银行清算进度")]
    public DataSet PingAn_BankStateQuery(string type, string date)
    {
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        BankPingan bkp = new BankPingan();
        try
        {
            //1004接口查询银行处理进度
            dsreturn = bkp.BankStateQuery(type, date, dsreturn);
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "调用QueryBankState方法失败！" + ex.ToString();
        }
        return dsreturn;
    }
}