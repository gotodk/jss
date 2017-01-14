using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

/// <summary>
/// 浦发银行接口调用
/// </summary>
public class BankPufa
{
    /// <summary>
    /// 统一调用前置机的方法
    /// </summary>
    /// <param name="dtParams">修改后的Dt</param>
    /// <returns></returns>
    private DataSet Go(DataTable dtParams)
    {
        DataTable dt = BankHelper.GetConfig("浦发银行");
        if (dt.Rows.Count > 0)
        {
            string routing = dt.Rows[0]["Imv"].ToString() == "测试库" ? dt.Rows[0]["CStr"].ToString() : dt.Rows[0]["ZStr"].ToString();
            DataSet ds = new DataSet();
            DataTable hl = dtParams.Copy();
            ds.Tables.Add(hl);
            object[] _params = { ds };
            return BankHelper.Go(routing, "Reincarnation", _params);
        }
        else return null;
    }

    /// <summary>
    /// 浦发银行出金方法（调用参数：出金）
    /// </summary>
    /// <param name="dsParams">用户前台传递过来的参数</param>
    /// <param name="BI">用户信息实体</param>
    /// <returns>执行结果</returns>
    public DataSet chujin(DataSet dsParams, BankInit BI)
    {
        //1、XML->DataTable   DataTable.ReadXML(path);
        //2、替换Dt里的字段
        //3、调用Go方法（统一调用前置机的方法）  DataSet ds = Go(dt);
        //4、银行会返回结果，前置机会把结果返回到本地，dataset
        //5、根据结果（接口里的结构）进行下一步处理。
        


        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt.Columns.Add("ColA");
        dt.Rows.Add(new string[] { "我是浦发银行出金" });
        ds.Tables.Add(dt);
        return ds;
    }
    public DataSet chujin_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 浦发入金（调用参数：入金）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet rujin(DataSet dsParams, BankInit BI)
    {
        //入金方法调用
        return (new DataSet());
    }
    public DataSet rujin_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 查询银行资金余额（调用参数：余额）
    /// (原始应为yinhangyue，但是“行”是多音字，系统会自动翻译成yinxingyue)
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yue(DataSet dsParams, BankInit BI)
    {
        //查询银行资金余额调用
        return null;
    }
    public DataSet yue_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 变更银行账户（调用参数：变更账户）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet biangengzhanghu(DataSet dsParams, BankInit BI)
    {
        //变更银行账户方法调用
        return null;
    }
    public DataSet biangengzhanghu_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 冲银行转证券(调用参数：冲银转证)
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet chongyinzhuanzheng(DataSet dsParams, BankInit BI)
    {
        //冲银转证
        return null;
    }
    public DataSet chongyinzhuanzheng_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }


    /// <summary>
    /// 冲证券转银行（调用参数：冲证转银）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet chongzhengzhuanyin(DataSet dsParams, BankInit BI)
    {
        //冲证转银
        return null;
    }
    public DataSet chongzhengzhuanyin_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 变更存管银行（调用参数：变更银行）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet biangengyinhang(DataSet dsParams, BankInit BI)
    {
        //变更存管银行
        return null;
    }
    public DataSet biangengyinhang_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 签约（调用参数：客户签约）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet qianyue(DataSet dsParams, BankInit BI)
    {
         //1、XML->DataTable   DataTable.ReadXML(path);
        //2、替换Dt里的字段
        //3、调用Go方法（统一调用前置机的方法）  DataSet ds = Go(dt);
        //4、银行会返回结果，前置机会把结果返回到本地，dataset
        //5、根据结果（接口里的结构）进行下一步处理。


        string Paths = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\App_Code\\Bank\\XMLPufa\\券商发起-客户存管签约#26701.xml";
        DataTable dt = new DataTable();
        dt.ReadXml(Paths);
        DataTable dtClone = dt.Clone();
        dtClone.Rows[0]["ExSerial"] = dsParams.Tables[0].Rows[0]["外部流水号"].ToString();//外部流水号
        dtClone.Rows[0]["sBranchId"] = dsParams.Tables[0].Rows[0]["营业部编码"].ToString();//营业部编码
        dtClone.Rows[0]["Name"] = dsParams.Tables[0].Rows[0]["客户姓名"].ToString();//客户姓名
        dtClone.Rows[0]["OccurBala"] = dsParams.Tables[0].Rows[0]["证券资金余额"].ToString();//证券资金余额
        dtClone.Rows[0]["sCustAcct"] = dsParams.Tables[0].Rows[0]["证券资金账号"].ToString();//证券资金账号
        dtClone.Rows[0]["sCustProperty"] = dsParams.Tables[0].Rows[0]["客户性质"].ToString();//客户性质
        dtClone.Rows[0]["CardType"] = dsParams.Tables[0].Rows[0]["客户证件类型"].ToString();//客户证件类型
        dtClone.Rows[0]["Card"] = dsParams.Tables[0].Rows[0]["客户证件号"].ToString();//客户证件号
        dtClone.Rows[0]["Date"] = dsParams.Tables[0].Rows[0]["交易日期"].ToString();//交易日期
        DataSet dataSet = Go(dtClone);
       DataSet dataResult=qianyue_Result(null,null);
       if (dataSet.Tables[0].Rows[0]["ErrorNo"].ToString() == "0000")
       {
           dataResult.Tables[0].Rows[0]["接口是否执行成功"] = "是";
       }
       else
       {
           dataResult.Tables[0].Rows[0]["接口是否执行成功"] = "否";
       }
        //客户存管签约
       return dataResult;
    }
    public DataSet qianyue_Result(DataSet dsParams, BankInit BI)
    {
        DataSet ds = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.Columns.Add("接口是否执行成功");
        auto2.Rows[0]["接口是否执行成功"] = "是";
        ds.Tables.Add(auto2);
        return ds;
    }

    /// <summary>
    /// 客户登记激活（调用参数：激活）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet jihuo(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet jihuo_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 结息（调用参数：结息）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet jiexi(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet jiexi_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 解约（调用参数：解约）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet jieyue(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet jieyue_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 客户预指定存管签约确认（调用参数：预指定确认）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yuzhidingqueren(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet yuzhidingqueren_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 取银行转账明细（调用参数：银行转帐明细）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yinhangzhuanzhangmingxi(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet yinhangzhuanzhangmingxi_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 修改客户资料（调用参数：修改资料）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet xiugaiziliao(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet xiugaiziliao_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 银行转证券（调用参数：银转证）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet yinzhuanzheng(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet yinzhuanzheng_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

    /// <summary>
    /// 证券转银行（调用参数：证转银）
    /// </summary>
    /// <param name="dsParams"></param>
    /// <param name="BI"></param>
    /// <returns></returns>
    public DataSet zhengzhuanyin(DataSet dsParams, BankInit BI)
    {
        return null;
    }
    public DataSet zhengzhuanyin_Result(DataSet dsParams, BankInit BI)
    {
        return new DataSet();
    }

}