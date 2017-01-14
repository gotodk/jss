/*******************************************************************
 * 
 * 创建人：wyh
 *
 *创建时间：2014.06.05
 *
 *代码功能：实现发货单各项功能
 *
 *参考文档：《业务分析》成交履约中心部分
 * 
 * 
 * ******************************************************************/
using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// FHDInfo 的摘要说明
/// </summary>
public class FHDInfo
{
	public FHDInfo()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

   
    /// <summary>
    /// 查询发货单详情
    /// </summary>
    /// <param name="FHDBH">发货单号</param>
    /// <returns>返回符合dsreturn结构Dataset，并且包含名为主表的datatable</returns>
    public DataSet CXFHD(string FHDBH)
    {
        DataSet returnDS = new TBD().initReturnDataSet().Copy();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            DataSet dsparr = new DataSet();
            Hashtable input = new Hashtable();
            Hashtable htreturn = new Hashtable();
            input["@FHDBH"] = FHDBH.Trim();
            #region//zhouli 作废 2017.07.17 需求变更
            //string strSQL = " select t.Number 发货单编号,t.F_FHDSCSJ 发货单下达时间,d.Z_HTBH 电子购货合同编号,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.Y_YSYDDDLYX where DDB.Number=d.Number),'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),'此前累计发货次数'=(select count(*) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT !='撤销'  and a.F_FHDSCSJ < t.F_FHDSCSJ),t.T_THSL 本次发货数量,'此前累计发货数量'=(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT in ('已录入发货信息','无异议收货','默认无异议收货','部分收货','已录入补发备注','补发货物无异议收货','有异议收货','有异议收货后无异议收货','卖家主动退货','请重新发货') and a.F_FHDSCSJ < t.F_FHDSCSJ), d.Z_SPBH 商品编号,d.Z_SPMC 商品名称,d.Z_GG 规格,d.Z_JJDW 计价单位,t.T_THSL 发货数量,t.ZBDJ 定标价格,t.T_DJHKJE 金额,t.T_SHRXM 收货人姓名,t.T_SHRLXFS 收货人联系电话,(d.Y_YSYDDSHQYsheng+d.Y_YSYDDSHQYshi+t.T_SHXXDZ) 收货地址,'发货人姓名'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),'发货人联系电话'=(select DL.I_LXRSJH from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),t.F_FPHM 发票编号,t.F_FPSFSHTH 发票是否随货同行,t.F_SFYWLTSYQ 特殊要求,t.F_WLTSYQSM 特殊要求说明,t.F_BFBZ 补发备注 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where t.Number=@FHDBH";
            #endregion
            string strSQL = " select t.Number 发货单编号,t.F_FHDSCSJ 发货单下达时间,d.Z_HTBH 电子购货合同编号,'买家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.Y_YSYDDDLYX where DDB.Number=d.Number),'卖家名称'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),'此前累计发货次数'=(select count(*) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT not in ('撤销','未生成发货单','已生成发货单')   and a.F_FHDSCSJ < t.F_FHDSCSJ),t.T_THSL 本次发货数量,'此前累计发货数量'=(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB a where a.ZBDBXXBBH=t.ZBDBXXBBH and a.F_DQZT not in ('撤销','未生成发货单','已生成发货单')  and a.F_FHDSCSJ < t.F_FHDSCSJ), d.Z_SPBH 商品编号,d.Z_SPMC 商品名称,d.Z_GG 规格,d.Z_JJDW 计价单位,t.T_THSL 发货数量,t.ZBDJ 定标价格,t.T_DJHKJE 金额,t.T_SHRXM 收货人姓名,t.T_SHRLXFS 收货人联系电话,(d.Y_YSYDDSHQYsheng+d.Y_YSYDDSHQYshi+t.T_SHXXDZ) 收货地址,'发货人姓名'=(select DL.I_JYFMC from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),'发货人联系电话'=(select DL.I_LXRSJH from AAA_DLZHXXB DL left join AAA_ZBDBXXB DDB on DL.B_DLYX=DDB.T_YSTBDDLYX where DDB.Number=d.Number),t.F_FPHM 发票编号,t.F_FPSFSHTH 发票是否随货同行,t.F_SFYWLTSYQ 特殊要求,t.F_WLTSYQSM 特殊要求说明,t.F_BFBZ 补发备注 from AAA_THDYFHDXXB t left join AAA_ZBDBXXB d on t.ZBDBXXBBH=d.Number where t.Number=@FHDBH";

            htreturn = I_DBL.RunParam_SQL(strSQL, "FHDINFo", input);
            if ((bool)(htreturn["return_float"])) //说明执行完成
            {
                //这里就可以用了。
                dsparr = (DataSet)(htreturn["return_ds"]);
                if (dsparr != null && dsparr.Tables["FHDINFo"].Rows.Count > 0)
                {
                    DataTable dt = dsparr.Tables["FHDINFo"].Copy();
                    dt.TableName = "主表";
                    returnDS.Tables.Add(dt);
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询失败！";
                }

            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_errmsg"].ToString();
            }

            return returnDS;
 
        }
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = e.ToString();
        }
        return returnDS;
    }

    /// <summary>
    /// 生成发货单 wyh 2014.06.11 
    /// </summary>
    /// <param name="ds">参数集合</param>
    /// <returns>返回符合dsretrn的Dataset</returns>
    public DataSet SCFHD(DataSet ds)
    {
        TBD tbd = new TBD();
        DataSet returnDS = tbd.initReturnDataSet().Copy();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功！";
        try
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string CZ = ds.Tables[0].Rows[0]["操作"].ToString();
                string ZT = ds.Tables[0].Rows[0]["状态"].ToString();
                string THDBH = ds.Tables[0].Rows[0]["提货单编号"].ToString();//提货单编号
                string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();//买家角色编号
                string ZBDBNumber = ds.Tables[0].Rows[0]["中标定标信息表编号"].ToString();//中标定标信息表编号
                string TSYQ = ds.Tables[0].Rows[0]["是否有物流特殊要求"].ToString();//是否有物流特殊要求
                string TSYQSM = ds.Tables[0].Rows[0]["物流特殊要求说明"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["物流特殊要求说明"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["物流特殊要求说明"].ToString();//物流特殊要求说明
                string FPHM = ds.Tables[0].Rows[0]["发票号码"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["发票号码"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["发票号码"].ToString();//发票号码
                string FPSHTX = ds.Tables[0].Rows[0]["发票是否随货同行"].ToString();//发票是否随货同行
                string time = DateTime.Now.ToString();//发货单生成时间
                #region//基础验证
                //拆分到外层
                #endregion

                #region//验证投标资料审核表对应的投标点是否审核通过
                //------------已经拆分到本中心
                DataSet dsZLSHB = new lvyueclass().GTTBZLSHinfo(THDBH.Trim());
                if (dsZLSHB != null && dsZLSHB.Tables[0].Rows.Count > 0)
                {
                    if (dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "未审核" || dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                        return returnDS;
                    }
                }
                #endregion

                #region//验证合同状态
               // string strSQLDB = "  select  d.Z_HTZT 合同状态,t.*  from AAA_THDYFHDXXB t,AAA_ZBDBXXB d where t.ZBDBXXBBH=d.Number and t.Number='" + THDBH.Trim() + "'";
                DataSet zt = new lvyueclass().GetTHDXXAndHTZT(THDBH.Trim());//DbHelperSQL.Query(strSQLDB); -----------------wyh 已拆分本中心
                if (zt != null && zt.Tables[0].Rows.Count > 0)
                {
                    switch (zt.Tables[0].Rows[0]["合同状态"].ToString())
                    {
                        case "定标":
                            if (zt.Tables[0].Rows[0]["F_DQZT"].ToString() != "未生成发货单")
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "OpIsInvalid";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作无效！";
                                return returnDS;
                            }
                            break;
                        default:
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "该中标定标信息不能生成发货单！";
                            return returnDS;
                    }
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到中标定标信息，不能生成发货单！";
                    return returnDS;
                }
                #endregion


                if (ZT == "重新发货")//重新发货后，新的提货单生成发货单
                {
                    switch (TSYQ)//是否有物流特殊要求
                    {
                        case "是":
                            #region//发票是否随货同行
                            switch (FPSHTX)
                            {
                                case "是":
                                    string strUpdateSQL = " update AAA_THDYFHDXXB set F_FHDSCSJ='" + time + "',F_SFYWLTSYQ='" + TSYQ + "',F_WLTSYQSM='" + TSYQSM + "',F_DQZT='已生成发货单',F_QMJQSQRCZSJ=null where Number='" + THDBH + "'";
                                    return YZAndCZ(CZ, "您已确认此次发货商品“有运输特殊要求”，发票随货同行。是否生成发货单？", strUpdateSQL, FPSHTX, returnDS);
                                case "否":
                                    strUpdateSQL = " update AAA_THDYFHDXXB set F_FHDSCSJ='" + time + "',F_SFYWLTSYQ='" + TSYQ + "',F_WLTSYQSM='" + TSYQSM + "',F_DQZT='已生成发货单',F_QMJQSQRCZSJ=null where Number='" + THDBH + "'";
                                    return YZAndCZ(CZ, "您已确认此次发货商品“有运输特殊要求”，发票不随货同行。是否生成发货单？", strUpdateSQL, FPSHTX, returnDS);
                                default:
                                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "意外错误！";
                                    return returnDS;
                            }
                            #endregion
                        case "否":
                            #region//发票是否随货同行
                            switch (FPSHTX)
                            {
                                case "是":
                                    string strUpdateSQL = " update AAA_THDYFHDXXB set F_FHDSCSJ='" + time + "',F_SFYWLTSYQ='" + TSYQ + "',F_DQZT='已生成发货单',F_QMJQSQRCZSJ=null where Number='" + THDBH + "'";
                                    return YZAndCZ(CZ, "您已确认此次发货商品“无运输特殊要求”，发票随货同行。是否生成发货单？", strUpdateSQL, FPSHTX, returnDS);
                                case "否":
                                    strUpdateSQL = " update AAA_THDYFHDXXB set F_FHDSCSJ='" + time + "',F_SFYWLTSYQ='" + TSYQ + "',F_DQZT='已生成发货单',F_QMJQSQRCZSJ=null where Number='" + THDBH + "'";
                                    return YZAndCZ(CZ, "您已确认此次发货商品“无运输特殊要求”，发票不随货同行。是否生成发货单？", strUpdateSQL, FPSHTX, returnDS);
                                default:
                                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "意外错误！";
                                    return returnDS;
                            }
                            #endregion
                        default:
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "意外错误！";
                            return returnDS;
                    }
                }
                else//第一次生成发货单
                {
                    switch (TSYQ)//是否有物流特殊要求
                    {
                        case "是":
                            #region//发票是否随货同行
                            switch (FPSHTX)
                            {
                                case "是":
                                    string strUpdateSQL = " update AAA_THDYFHDXXB set F_FHDSCSJ='" + time + "',F_SFYWLTSYQ='" + TSYQ + "',F_WLTSYQSM='" + TSYQSM + "',F_FPSFSHTH='" + FPSHTX + "',F_FPYJXXLRSJ='" + time + "',F_FPHM='" + FPHM + "',F_DQZT='已生成发货单',F_QMJQSQRCZSJ=null where Number='" + THDBH + "'";
                                    return YZAndCZ(CZ, "您已确认此次发货商品“有运输特殊要求”，发票随货同行。是否生成发货单？", strUpdateSQL, FPSHTX, returnDS);
                                case "否":
                                    strUpdateSQL = " update AAA_THDYFHDXXB set F_FHDSCSJ='" + time + "',F_SFYWLTSYQ='" + TSYQ + "',F_WLTSYQSM='" + TSYQSM + "',F_FPSFSHTH='" + FPSHTX + "',F_FPHM='" + FPHM + "',F_DQZT='已生成发货单',F_QMJQSQRCZSJ=null where Number='" + THDBH + "'";
                                    return YZAndCZ(CZ, "您已确认此次发货商品“有运输特殊要求”，发票不随货同行。是否生成发货单？", strUpdateSQL, FPSHTX, returnDS);
                                default:
                                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "意外错误！";
                                    return returnDS;
                            }
                            #endregion
                        case "否":
                            #region//发票是否随货同行
                            switch (FPSHTX)
                            {
                                case "是":
                                    string strUpdateSQL = " update AAA_THDYFHDXXB set F_FHDSCSJ='" + time + "',F_SFYWLTSYQ='" + TSYQ + "',F_FPSFSHTH='" + FPSHTX + "',F_FPYJXXLRSJ='" + time + "',F_FPHM='" + FPHM + "',F_DQZT='已生成发货单',F_QMJQSQRCZSJ=null where Number='" + THDBH + "'";
                                    return YZAndCZ(CZ, "您已确认此次发货商品“无运输特殊要求”，发票随货同行。是否生成发货单？", strUpdateSQL, FPSHTX, returnDS);
                                case "否":
                                    strUpdateSQL = " update AAA_THDYFHDXXB set F_FHDSCSJ='" + time + "',F_SFYWLTSYQ='" + TSYQ + "',F_FPSFSHTH='" + FPSHTX + "',F_FPHM='" + FPHM + "',F_DQZT='已生成发货单',F_QMJQSQRCZSJ=null where Number='" + THDBH + "'";
                                    return YZAndCZ(CZ, "您已确认此次发货商品“无运输特殊要求”，发票不随货同行。是否生成发货单？", strUpdateSQL, FPSHTX, returnDS);
                                default:
                                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "意外错误！";
                                    return returnDS;
                            }
                            #endregion
                        default:
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "意外错误！";
                            return returnDS;
                    }
                }


            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "存入的数据不能为空！";
            }
        }
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";
        }
        return returnDS;
    }

    /// <summary>
    /// 验证和实际操作
    /// </summary>
    /// <param name="TSMessage">提示文字</param>
    /// <param name="returnDS">返回操作结果</param>
    /// <param name="UpdateTHDSQL">执行SQL</param>
    /// <param name="FPSFSHTX">发票是否随货同行["是"、"否"]</param>
    protected DataSet YZAndCZ(string CZ, string TSMessage, string UpdateTHDSQL, string FPSFSHTX, DataSet returnDS)
    {
        if (CZ == "只验证")
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "OnlyYZ";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = TSMessage;
            return returnDS;
        }
        else if (CZ == "实际操作")
        {
            if (UpdateTHDSQL != "")
            {

                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
                DataSet dsparr = new DataSet();
                Hashtable input = new Hashtable();
                Hashtable htreturn = new Hashtable();
                Hashtable Htwyy = new Hashtable();
                htreturn = I_DBL.RunProc(UpdateTHDSQL);
                int i = 0;//DbHelperSQL.ExecuteSql(UpdateTHDSQL);
             
                if ((bool)(htreturn["return_float"])) //说明执行完成
                {
                    //这里就可以用了。
                    if ((int)(htreturn["return_other"]) <= 0)
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "执行了0条语句！";
                        return returnDS;
                    }
                    i = (int)(htreturn["return_other"]);

                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = htreturn["return_errmsg"].ToString(); ;
                    return returnDS;
                }

                if (i > 0)
                {
                    if (FPSFSHTX == "是")
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单已生成！请您尽快录入物流发货信息，否则系统将默认为您未发货。";
                        return returnDS;
                    }
                    else
                    {
                        returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                        returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单已生成！请您尽快录入物流发货信息及发票邮寄信息，否则系统将默认为您未发货。";
                        return returnDS;
                    }

                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货单生成失败！";
                    return returnDS;
                }
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "执行的SQL语句不能为空！";
                return returnDS;
            }
        }
        else
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "意外错误！";
            return returnDS;
        }
    }

    #region//录入发货、发票信息--zhouli 2017.07.02 add
    //录入发货信息
    public DataSet LRFHXX(DataSet ds)
    {
        TBD tbd = new TBD();
        DataSet returnDS = tbd.initReturnDataSet().Copy();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        try
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();//卖家角色编号
                string FHDBH = ds.Tables[0].Rows[0]["发货单编号"].ToString();//发货单编号
                string GSMC = ds.Tables[0].Rows[0]["物流公司名称"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["物流公司名称"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["物流公司名称"].ToString();//物流公司名称
                string WLDH = ds.Tables[0].Rows[0]["物流单号"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["物流单号"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["物流单号"].ToString();//物流单号
                string LXR = ds.Tables[0].Rows[0]["物流公司联系人"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["物流公司联系人"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["物流公司联系人"].ToString();//物流公司联系人
                string DH = ds.Tables[0].Rows[0]["物流公司电话"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["物流公司电话"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["物流公司电话"].ToString();//物流公司电话
                string time = DateTime.Now.ToString();//物流信息录入时间
                #region//基础验证
                //Hashtable JCYZ = PublicClass2013.GetParameterInfo();
                //Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
                //if (JBXX["交易账户是否开通"].ToString() == "否")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                //    return returnDS;
                //}

                //Hashtable htPTVal = PublicClass2013.GetParameterInfo();//平台信息
                //if (htPTVal["是否在服务时间内(假期)"].ToString().Trim() != "是")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                //    return returnDS;
                //}
                //if (htPTVal["是否在服务时间内(工作时)"].ToString().Trim() != "是")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间（工作时间），暂停交易！";
                //    return returnDS;
                //}
                //if (htPTVal["是否在服务时间内"].ToString().Trim() != "是")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                //    return returnDS;
                //}


                //if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                //    return returnDS;
                //}
                //if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                //    return returnDS;
                //}
                //if (JCYZ["是否在服务时间内(工作时)"].ToString().Trim() != "是")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                //    return returnDS;
                //}
                //if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                //    return returnDS;
                //}
                //if (JBXX["是否休眠"].ToString() == "是")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                //    return returnDS;
                //}
                #endregion

                #region//验证投标资料审核表对应的发货单是否审核通过
                if (!THDIsSHTG(FHDBH.Trim()))
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                    return returnDS;
                }
                #endregion

                #region//验证合同状态
                DataSet zt = HTMsg(FHDBH.Trim());

                if (zt != null && zt.Tables .Count>0&& zt.Tables[0].Rows.Count > 0)
                {
                    switch (zt.Tables[0].Rows[0]["Z_HTZT"].ToString())
                    {
                        case "定标":
                            if (zt.Tables[0].Rows[0]["F_DQZT"].ToString() != "已生成发货单")
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "OpIsInvalid";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作无效！";
                                return returnDS;
                            }
                            break;
                        default:
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "该中标定标信息不能录入发货信息！";
                            return returnDS;
                    }
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到中标定标信息，不能录入发货信息！";
                    return returnDS;
                }
                #endregion

                Hashtable htcs = new Hashtable();
                htcs["@time"] = time;
                htcs["@GSMC"] = GSMC;
                htcs["@WLDH"] = WLDH;
                htcs["@LXR"] = LXR;
                htcs["@DH"] = DH;
                htcs["@FHDBH"] = FHDBH;
                string strUpdateSQL = "update AAA_THDYFHDXXB set F_WLXXLRSJ=@time,F_WLGSMC=@GSMC,F_WLDH=@WLDH,F_WLGSLXR=@LXR,F_WLGSDH=@DH,F_DQZT='已录入发货信息' where Number=@FHDBH ";
                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
                Hashtable returnHT = I_DBL.RunParam_SQL(strUpdateSQL, htcs);
                if (!(bool)returnHT["return_float"])
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货信息录入失败！";
                    return returnDS;
                }
                int i = (int)returnHT["return_other"];
                if (i > 0)
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货信息录入成功！";
                    return returnDS;

                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发货信息录入失败！";
                    return returnDS;
                }
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "存入的数据不能为空！";
            }
        }
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";
        }
        return returnDS;
    }

    /// <summary>
    /// 验证投标资料审核表对应的发货单是否审核通过
    /// </summary>
    /// <param name="number">发货单Number</param>
    /// <returns>false：未审核通过；true：审核通过；</returns>
    public bool THDIsSHTG(string FHDBH)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht = new Hashtable();
        ht["@number"] = FHDBH;
        string strTBDZLSH = "select c.* from AAA_THDYFHDXXB a left join AAA_ZBDBXXB b on a.ZBDBXXBBH = b.Number left join AAA_TBZLSHB c on b.T_YSTBDBH = c.TBDH where a.Number=@number";
        Hashtable returnHT = I_DBL.RunParam_SQL(strTBDZLSH,"",ht);
        if ((bool)returnHT["return_float"])
        {
            if (returnHT["return_ds"] != null && ((DataSet)returnHT["return_ds"]).Tables.Count > 0 && ((DataSet)returnHT["return_ds"]).Tables[0].Rows.Count > 0)
            {
                DataSet dsZLSHB = (DataSet)returnHT["return_ds"];
                if (dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "未审核" || dsZLSHB.Tables[0].Rows[0]["JYGLBSHZT"].ToString() == "审核未通过")
                {
                    return false;
                }
            }
        }
        return true;
    }
    /// <summary>
    /// 获取合同信息
    /// </summary>
    /// <param name="FHDBH"></param>
    /// <returns>ds：Z_HTZT、F_DQZT、F_FPYJXXLRSJ</returns>
    public DataSet HTMsg(string FHDBH)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable ht = new Hashtable();
        ht["@number"] = FHDBH;
        string strSQLDB = "  select d.Z_HTZT,t.F_DQZT,t.F_FPYJXXLRSJ from AAA_THDYFHDXXB t,AAA_ZBDBXXB d where t.ZBDBXXBBH=d.Number and t.Number=@number";
        Hashtable returnht = I_DBL.RunParam_SQL(strSQLDB,"",ht);
        if ((bool)returnht["return_float"])
        {
            return (DataSet)returnht["return_ds"];
        }
        return null;
    }
    //录入发票信息
    public DataSet LRFPXX(DataSet ds)
    {
        TBD tbd = new TBD();
        DataSet returnDS = tbd.initReturnDataSet().Copy();
        returnDS.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化", "", "true" });
        try
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();//卖家角色编号
                string FHDBH = ds.Tables[0].Rows[0]["发货单编号"].ToString();//发货单编号
                string DWMC = ds.Tables[0].Rows[0]["发票邮寄单位名称"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["发票邮寄单位名称"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["发票邮寄单位名称"].ToString();//发票邮寄单位名称
                string FPBH = ds.Tables[0].Rows[0]["发票邮寄单编号"].ToString().Contains("'") == true ? ds.Tables[0].Rows[0]["发票邮寄单编号"].ToString().Replace("'", "‘") : ds.Tables[0].Rows[0]["发票邮寄单编号"].ToString();//发票邮寄单编号
                string time = DateTime.Now.ToString();//F_FPYJXXLRSJ
                #region//基础验证
                //Hashtable JCYZ = PublicClass2013.GetParameterInfo();
                //Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
                //if (JBXX["交易账户是否开通"].ToString() == "否")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
                //    return returnDS;
                //}

                //if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                //    return returnDS;
                //}
                //if (JBXX["是否休眠"].ToString() == "是")
                //{
                //    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                //    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                //    return returnDS;
                //}
                #endregion

                #region//验证投标资料审核表对应的投标点是否审核通过
                if(!THDIsSHTG(FHDBH))
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前投标单异常，请与平台服务人员联系！";
                    return returnDS;
                }
                
                #endregion

                #region//验证合同状态
                DataSet zt = HTMsg(FHDBH.Trim());
                if (zt != null && zt.Tables.Count > 0 && zt.Tables[0].Rows.Count > 0)
                {
                    switch (zt.Tables[0].Rows[0]["Z_HTZT"].ToString())
                    {
                        case "定标":
                            if (zt.Tables[0].Rows[0]["F_FPYJXXLRSJ"].ToString() != "")
                            {
                                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "OpIsInvalid";
                                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作无效！";
                                return returnDS;
                            }
                            break;
                        default:
                            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "该中标定标信息不能录入发票信息！";
                            return returnDS;
                    }
                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到中标定标信息，不能录入发票信息！";
                    return returnDS;
                }
                #endregion
                Hashtable htcs = new Hashtable();
                htcs["@time"] = time;
                htcs["@DWMC"] = DWMC;
                htcs["@FPBH"] = FPBH;
                htcs["@FHDBH"] = FHDBH;
                string strUpdateSQL = "update AAA_THDYFHDXXB set F_FPYJXXLRSJ=@time,F_FPYJDWMC=@DWMC,F_FPYJDBH=@FPBH where Number=@FHDBH ";
                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
                Hashtable returnHT = I_DBL.RunParam_SQL(strUpdateSQL, htcs);
                if (!(bool)returnHT["return_float"])
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发票信息录入失败！";
                    return returnDS;
                }
                int i = (int)returnHT["return_other"];
                if (i > 0)
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发票信息录入成功！";
                    return returnDS;

                }
                else
                {
                    returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "发票信息录入失败！";
                    return returnDS;
                }
            }
            else
            {
                returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "存入的数据不能为空！";
            }
        }
        catch (Exception e)
        {
            returnDS.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            returnDS.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";
        }
        return returnDS;
    }

    #endregion
}