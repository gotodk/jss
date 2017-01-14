using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;
using System.Collections;

/// <summary>
///SaveYDD 的摘要说明
/// </summary>
public class SaveYDD
{
    public SaveYDD()
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

        //DataTable auto3 = new DataTable();
        //auto3.TableName = "返回值多条";
        //auto3.Columns.Add("多条错误");


        //ds.Tables.Add(auto3);
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
        string SellerJSBH = dsinput.Tables["主表"].Rows[0]["买家角色编号"].ToString();
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(SellerJSBH);


        if (SellerJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您尚未开通结算账户，将不能下达预订单信息！";
            return dsreturn;
        }
        if (UserInfo["是否允许登录"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您已被禁止登录，将不能下达预订单信息！";
            return dsreturn;
        }
        if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "当前时间业务停止运行，将不能下达预订单信息！";
            return dsreturn;
        }
        if (UserInfo["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的账号已休眠，将不能下达预订单信息！";
            return dsreturn;
        }
        //if (UserInfo["是否冻结"].ToString() == "是")
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的账号已被禁止业务，将不能下达预订单信息！";
        //    return dsreturn;
        //}
        if (UserInfo["当前默认关联经纪人角色编号"].ToString() == "")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "您的经纪人被禁止或暂停业务，将不能下达预订单信息！";
            return dsreturn;
        }
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "检查通过！";

        return dsreturn;

    }
    //public DataSet YZ(string User_email)
    //{
    //    DataSet ds = new DataSet();
    //    DataTable tb = new DataTable();        
    //    tb.Columns.Add("执行结果");
    //    tb.Columns.Add("错误提示");
    //    ds.Tables.Add(tb);
    //    tb.TableName = "预订单执行结果";
    //    string strSQLUserLogion = "select * from ZZ_UserLogin where DLYX='" + User_email.Trim()  + "'";
    //    DataSet dsUserLogion = DbHelperSQL.Query(strSQLUserLogion);
    //    if (dsUserLogion != null && dsUserLogion.Tables[0].Rows.Count > 0)
    //    {
    //        #region


    //        #endregion
    //    }
    //    else
    //    {
    //        tb.Rows.Add(new string[] { "未找到该账号的信息", "" });
    //        return ds;
    //    }
    //    if (jhjx_PublicClass.IsOpenDay())
    //    {
    //        tb.Rows.Add(new string[] { "不在营业日", "" });
    //        return ds;
    //    }
    //    else
    //    {
    //        if (jhjx_PublicClass.IsOpenTime())
    //        {

    //        }
    //        else
    //        {
    //            tb.Rows.Add(new string[] { "不在营业时间", "" });
    //            return ds;
    //        }
    //    }
    //}
    public DataSet BegionSaveYDD(DataSet dsinput)
    {
        //首次进入的检查
        if (dsinput.Tables["主表"]!=null &&dsinput.Tables["主表"].Rows.Count>0)
        {
            if (dsinput.Tables["主表"].Rows[0]["特殊标记"].ToString() == "仅检查基础")
            {
                return OnlyJC(dsinput);
            }            
        }

        //正常提交的检查
        //初始化返回值结构
        DataSet dsreturn = initReturnDataSet();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "未知错误" });       

        string MJDLYX = "";//买家登录邮箱
        string MJYHM = "";//买家姓名
        string MJJSBH = "";//买家角色编号
        string GLDLRDLYX = "";//经纪人登录邮箱
        string GLDLRYHM = "";//经纪人姓名
        string GLDLRJSBH = "";//经纪人角色编号
        string TBLX = "";//投标类型
        string HTZQ = "";//合同周期
        string SPBH = "";//商品编号
        string SPMC = "";//商品名称
        string GGXH = "";//规格型号
        string JJDW = "";//计价单位
        int JJPL = 0;//经济批量
        int YDZSL = 0;//预定总数量
        int YZBL = 0;//已中标量
        double NMRJG = 0;//拟买入价格
        string DQWCTBXXH = "";//当前位次投标信息号
        double DQWCTBJG = 0;//当前位次投标价格
        double ZFDJBL = 0;//支付定金比率
        double ZFDJJE = 0;//支付定金金额
        double DJSYKYJE = 0;//订金剩余可用金额
        string XDYDDSJ = DateTime.Now.ToString(); ;//下达预订单时间
        string ZT = "竞标";//状态
        string JRLJQSJ = "null";//进入冷静期时间
        string SFJRLJQ = "否";//是否进入冷静期
        int XGCS = 0;//修改次数
        string SHQY = "";//收货区域
        string CreateTime = DateTime.Now.ToString();
        string CheckLimitTime = DateTime.Now.ToString();



        Hashtable htParameterInfo = jhjx_PublicClass.GetParameterInfo();
        if (dsinput != null && dsinput.Tables["预订单"].Rows.Count > 0)
        {
            
            MJDLYX = dsinput.Tables["预订单"].Rows[0]["买家登录邮箱"].ToString();
            TBLX = dsinput.Tables["预订单"].Rows[0]["投标类型"].ToString();
            HTZQ = dsinput.Tables["预订单"].Rows[0]["合同周期"].ToString();//合同周期
            SPBH = dsinput.Tables["预订单"].Rows[0]["商品编号"].ToString();//商品编号
            SPMC = dsinput.Tables["预订单"].Rows[0]["商品名称"].ToString();//商品名称
            GGXH = dsinput.Tables["预订单"].Rows[0]["规格型号"].ToString();//规格型号
            JJDW = dsinput.Tables["预订单"].Rows[0]["计价单位"].ToString();//计价单位
            JJPL = Convert.ToInt32(dsinput.Tables["预订单"].Rows[0]["经济批量"].ToString().Trim());//经济批量
            YDZSL = Convert.ToInt32(dsinput.Tables["预订单"].Rows[0]["预定总数量"].ToString().Trim());//预定总数量
            NMRJG = Convert.ToDouble(dsinput.Tables["预订单"].Rows[0]["拟买入价格"].ToString().Trim());//拟买入价格
            string strSQLTBXXH = "select top 1 min(TBJG) TBJG,TBXXH from ZZ_TBXXB left join ZZ_TBXXBZB on ZZ_TBXXBZB.parentNumber=ZZ_TBXXB.Number where ZT='竞标' and SFJRLJQ='否' and SPBH='" + SPBH + "'group by TBXXH order by TBJG asc";
            
    
                DataSet  dsTBXXH = DbHelperSQL.Query(strSQLTBXXH);

            if (dsTBXXH!=null && dsTBXXH.Tables[0].Rows.Count>0)
            {
                DQWCTBXXH = dsTBXXH.Tables[0].Rows[0]["TBXXH"].ToString().Trim();//当前位次投标信息号
            }           
            
            DQWCTBJG = Convert.ToDouble(dsinput.Tables["预订单"].Rows[0]["当前位次投标价格"].ToString().Trim());//当前位次投标价格   
            ZFDJBL = Convert.ToDouble(htParameterInfo["订金"]);
            ZFDJJE = Math.Round(NMRJG * YDZSL * ZFDJBL,2);//支付定金金额
            DJSYKYJE = ZFDJJE;//订金剩余可用金额
            SHQY = dsinput.Tables["预订单"].Rows[0]["收货区域"].ToString().Trim();//收货区域
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "请先填写预订单的信息！";
            return dsreturn;
        }
        
        //检查账户余额
        ClassMoney CM = new ClassMoney();
        double kyMoney = CM.GetMoneyT(MJDLYX, "模拟而已");
        if (kyMoney < ZFDJJE)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "提交失败，账户余额不足，预订单总额" + NMRJG * YDZSL + "元，需缴纳订金" + ZFDJJE + "元，您的账户余额为" + kyMoney + "元!";
            return dsreturn;
        }




        string strSQLGLJJR = "select * from ZZ_MMJYDLRZHGLB where JSDLZH='" + MJDLYX + "' and SFMRDLR='是' and DLRSHZT='已审核' and GLZT='有效' and JSLX='买家' order by CreateTime desc";
        DataSet dsGLJJR = DbHelperSQL.Query(strSQLGLJJR);

        if (dsGLJJR != null && dsGLJJR.Tables[0].Rows.Count > 0)
        {
            MJYHM = dsGLJJR.Tables[0].Rows[0]["JSYHM"].ToString();
            MJJSBH = dsGLJJR.Tables[0].Rows[0]["JSBH"].ToString();
            GLDLRDLYX = dsGLJJR.Tables[0].Rows[0]["DLRDLZH"].ToString();
            GLDLRYHM = dsGLJJR.Tables[0].Rows[0]["DLRYHM"].ToString();
            GLDLRJSBH = dsGLJJR.Tables[0].Rows[0]["DLRBH"].ToString();
        }

        //数据检查        
        Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(MJJSBH);


        if (MJJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
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

        WorkFlowModule WFPKNumber = new WorkFlowModule("ZZ_YDDXXB");
        string Number = WFPKNumber.numberFormat.GetNextNumber();//生成ZZ_YDDXXB表的主键
        string strSQLInsertYDD = "INSERT INTO [dbo].[ZZ_YDDXXB] ([Number] ,[MJDLYX] ,[MJYHM] ,[MJJSBH] ,[GLDLRDLYX] ,[GLDLRYHM] ,[GLDLRJSBH] ,[TBLX] ,[HTZQ] ,[SPBH] ,[SPMC] ,[GGXH] ,[JJDW] ,[JJPL] ,[YDZSL] ,[YZBL] ,[NMRJG] ,[DQWCTBXXH],[DQWCTBJG],[ZFDJBL] ,[ZFDJJE] ,[DJSYKYJE],[XDYDDSJ] ,[ZT],[JRLJQSJ],[SFJRLJQ],[XGCS] ,[SHQY],[CheckState] ,[CreateUser] ,[CreateTime] ,[CheckLimitTime]) VALUES ('" + Number + "','" + MJDLYX + "','" + MJYHM + "','" + MJJSBH + "' ,'" + GLDLRDLYX + "','" + GLDLRYHM + "','" + GLDLRJSBH + "','" + TBLX + "','" + HTZQ + "','" + SPBH + "' ,'" + SPMC + "' ,'" + GGXH + "','" + JJDW + "'," + JJPL + "," + YDZSL + "," + YZBL + "," + NMRJG + ",'" + DQWCTBXXH + "'," + DQWCTBJG + "," + ZFDJBL + "," + ZFDJJE + "," + DJSYKYJE + ",'" + XDYDDSJ + "','" + ZT + "'," + JRLJQSJ + ",'" + SFJRLJQ + "'," + XGCS + " ,'" + SHQY + "' ,1,'" + MJYHM + "','" + CreateTime + "' ,'" + CheckLimitTime + "')";
        ArrayList list = new ArrayList();
        list.Add(strSQLInsertYDD);

        //插入资金变更明细表SQl
        Hashtable htinfo = new Hashtable();
        htinfo["来源单号"] = Number;
        htinfo["变动金额"] = ZFDJJE;
        htinfo["变动前账户余额"] = kyMoney;
        htinfo["变动后账户余额"] = Convert.ToDouble(kyMoney - ZFDJJE);
        ArrayList almoney = CM.InsertMoneyUse("提交预订单", htinfo, MJJSBH);
        list.AddRange(almoney);


        bool bl = DbHelperSQL.ExecSqlTran(list);


        #region//是否进入冷静期的验证
        CJGZ cj = new CJGZ();
        DataTable dt = new DataTable();
        dt.Columns.Add("商品编号");
        dt.Columns.Add("合同期限");
        dt.Rows.Add(new string[] { SPBH, HTZQ });
        ArrayList listLJQ = new ArrayList();
        listLJQ = cj.RunCJGZ(dt, "提交");
        DbHelperSQL.ExecSqlTran(listLJQ);

        #endregion

  

        
        if (bl)
        {
            //执行成功，调用银行接口扣减额度
            CM.FreezeMoneyT(MJDLYX, ZFDJJE.ToString(), "-");

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "success";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "本次下达预订单成功！";
            return dsreturn;
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["错误提示"] = "本次下达预订单失败！";
            return dsreturn;
        }
    }
}