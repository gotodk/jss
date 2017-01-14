using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

/// <summary>
/// BankServicePF 的摘要说明
/// </summary>
public abstract class BankServicePF
{
    /// <summary>
    /// 全局公用接口实例
    /// </summary>
    public static BankPF.Service BankIsPF = new BankPF.Service();

    /// <summary>
    /// 获取初始化数据集。 接口编号：#24603 （券商发起，查询银行资金余额）
    /// </summary>
    /// <returns></returns>
    public static DataTable N24603_UMoneyInBank_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_UMoneyInBank();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 发送查询银行资金余额指令。 接口编号：#24603 （券商发起，查询银行资金余额）
    /// </summary>
    /// <param name="dt">获取初始化数据集并修改后的数据表</param>
    /// <returns></returns>
    public static DataSet N24603_UMoneyInBank_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_UMoneyInBank(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 接口编号：#26710 （券商发起，变更银行账户指令）
    /// </summary>
    /// <returns></returns>
    public static DataTable N26710_ModifyBankCard_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_ModifyBankCard();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 发送变更银行账户指令。 接口编号：#26710 （券商发起，变更银行账户指令）
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N26710_ModifyBankCard_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_ModifyBankCard(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 接口编号：#24703 （券商发起，冲银行转证券）
    /// </summary>
    /// <returns></returns>
    public static DataTable N24703_BankToSecurities_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_CBankToSecurities();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 发送冲银行转证券指令。 接口编号：#24703 （券商发起，冲银行转证券）
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24703_BankToSecurities_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_CBankToSecurities(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-冲证券转银行#24704
    /// </summary>
    /// <returns></returns>
    public static DataTable N24704_CSecuritiesToBank_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_CSecuritiesToBank();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-冲证券转银行#24704
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24704_CSecuritiesToBank_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_CSecuritiesToBank(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-客户变更存管银行#26713
    /// </summary>
    /// <returns></returns>
    public static DataTable N26713_ModifyBank_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_ModifyBank();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户变更存管银行#26713
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N26713_ModifyBank_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_ModifyBank(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-客户存管签约#26701
    /// </summary>
    /// <returns></returns>
    public static DataTable N26701_Deal_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_Deal();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户存管签约#26701
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N26701_Deal_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_Deal(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-客户登记激活#26611
    /// </summary>
    /// <returns></returns>
    public static DataTable N26611_Active_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_Active();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户登记激活#26611
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N26611_Active_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_Active(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-客户结息#26712
    /// </summary>
    /// <returns></returns>
    public static DataTable N26712_UserInterest_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_UserInterest();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户结息#26712
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N26712_UserInterest_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_UserInterest(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-客户解约#26703
    /// </summary>
    /// <returns></returns>
    public static DataTable N26703_DealBroked_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_DealBroked();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户解约#26703
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N26703_DealBroked_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_DealBroked(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-客户预指定存管签约确认#26702
    /// </summary>
    /// <returns></returns>
    public static DataTable N26702_DealAerify_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_DealAerify();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-客户预指定存管签约确认#26702
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N26702_DealAerify_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_DealAerify(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-取银行转帐明细#24710
    /// </summary>
    /// <returns></returns>
    public static DataTable N24710_BankMoneyDetails_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_BankMoneyDetails();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-取银行转帐明细#24710
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24710_BankMoneyDetails_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_BankMoneyDetails(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-修改客户资料#26704
    /// </summary>
    /// <returns></returns>
    public static DataTable N26704_ModifyUserInfo_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_ModifyUserInfo();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-修改客户资料#26704
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N26704_ModifyUserInfo_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_ModifyUserInfo(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-银行转证券#24701
    /// </summary>
    /// <returns></returns>
    public static DataTable N24701_BankToSecurities_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_BankToSecurities();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银行转证券#24701
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24701_ModifyUserInfo_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_BankToSecurities(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-银证转帐解约#26715
    /// </summary>
    /// <returns></returns>
    public static DataTable N26715_BankTraDealBroked_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_BankTraDealBroked();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银证转帐解约#26715
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N26715_BankTraDealBroked_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_BankTraDealBroked(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-银证转帐签约#26714
    /// </summary>
    /// <returns></returns>
    public static DataTable N26714_BankTraDeal_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_BankTraDeal();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银证转帐签约#26714
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N26714_BankTraDeal_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_BankTraDeal(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-银证转账对总账#24708
    /// </summary>
    /// <returns></returns>
    public static DataTable N24708_BankTraToMain_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_BankTraToMain();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银证转账对总账#24708
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24708_BankTraToMain_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_BankTraToMain(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。 券商发起-银证转账证券签到#24901
    /// </summary>
    /// <returns></returns>
    public static DataTable N24901_BankTraSignIn_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_BankTraSignIn();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银证转账证券签到#24901
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24901_BankTraSignIn_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_BankTraSignIn(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。券商发起-银证转账证券签退#24902
    /// </summary>
    /// <returns></returns>
    public static DataTable N24902_BankTraSignOut_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_BankTraSignOut();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-银证转账证券签退#24902
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24902_BankTraSignOut_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_BankTraSignOut(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。券商发起-证券发送文件#24903
    /// </summary>
    /// <returns></returns>
    public static DataTable N24903_SecuritiesSendFile_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_SecuritiesSendFile();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-证券发送文件#24903
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24903_SecuritiesSendFile_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_SecuritiesSendFile(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。券商发起-证券发送文件完毕通知#24909
    /// </summary>
    /// <returns></returns>
    public static DataTable N24909_SecuritiesSendFileEnd_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_SecuritiesSendFileEnd();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-证券发送文件完毕通知#24909
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24909_SecuritiesSendFileEnd_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_SecuritiesSendFileEnd(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。券商发起-证券接收文件#24904
    /// </summary>
    /// <returns></returns>
    public static DataTable N24904_SecuritiesGetFile_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_SecuritiesGetFile();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-证券接收文件#24904
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24904_SecuritiesGetFile_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_SecuritiesGetFile(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。券商发起-证券文件生成完毕通知#24908
    /// </summary>
    /// <returns></returns>
    public static DataTable N24908_SecuritiesGetFileEnd_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_SecuritiesGetFileEnd();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-证券文件生成完毕通知#24908
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24908_SecuritiesGetFileEnd_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_SecuritiesGetFileEnd(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

    /// <summary>
    /// 获取初始化数据集。券商发起-证券转银行#24702
    /// </summary>
    /// <returns></returns>
    public static DataTable N24702_SecuritiesToBank_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_SecuritiesToBank();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-券商发起-证券转银行#24702
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N24702_SecuritiesToBank_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_SecuritiesToBank(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }


    /// <summary>
    /// 获取初始化数据集。券商发起-软加密密钥交换#26700
    /// </summary>
    /// <returns></returns>
    public static DataTable N26700_SecretKey_Init()
    {
        DataTable dt = null;
        try
        {
            dt = BankIsPF.Get_SecuritiesKey();
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
            */
            dt = null;
        }
        return dt;
    }

    /// <summary>
    /// 券商发起-软加密密钥交换#26700
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DataSet N26700_SecretKey_Push(DataTable dt)
    {
        DataSet ds = null;
        try
        {
            ds = BankIsPF.Set_SecuritiesKey(dt);
        }
        catch (Exception ex)
        {
            /*
             在此处写错误日志。
             */
            ds = null;
        }
        return ds;
    }

}