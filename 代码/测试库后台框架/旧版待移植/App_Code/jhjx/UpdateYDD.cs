using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Collections;
using FMOP.DB;

/// <summary>
///UpdateYDD 的摘要说明
/// </summary>
public class UpdateYDD
{
	public UpdateYDD()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 预订单信息表,修改、撤销预订单
    /// </summary>
    /// <param name="dt">修改的数据集，必须有的字段"修改的数量"、"修改的价格"、"预订单号"、"商品编号"、"合同期限"</param>
    /// <param name="cz">修改、撤销</param>
    /// <returns>返回修改或撤销的结果</returns>
    public string BegionUpdateYDD(DataSet ds)
    {
        try
        {
            string strSql = "";
            DataTable dt = ds.Tables["预订单"];
            if (dt != null && dt.Rows.Count > 0)
            {
              

                ArrayList list = new ArrayList();
                switch (dt.Rows[0]["操作"].ToString().Trim())
                {
                    case "修改":
                        #region//修改
                        foreach (DataRow row in dt.Rows)
                        {
                            int updateSL = Convert.ToInt32(row["修改的数量"].ToString().Trim());
                            string nmrjg = row["修改的价格"].ToString();
                            string Number = row["预订单号"].ToString();
                            DataSet dsYDD = jhjx_PublicClass.GetYDDXX(Number, "竞标");
                            if (dsYDD != null && dsYDD.Tables[0].Rows.Count > 0)
                            {
                                #region//获取未修改前对应预订单拟买入价格、预定总数量、已中标量、修改次数、未中标量
                                string MJDLYX = dsYDD.Tables[0].Rows[0]["MJDLYX"].ToString();//登陆账号
                                string MJJSBH = dsYDD.Tables[0].Rows[0]["MJJSBH"].ToString();//买家角色编号

                                Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(MJJSBH);
                                if (MJJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
                                {
                                    return "修改失败，您尚未开通结算账户！";
                                }
                                if (UserInfo["是否允许登录"].ToString() != "是")
                                {
                                    return "修改失败，您已被禁止登录！";
                                }
                                if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
                                {
                                    return "修改失败，当前时间业务停止运行！";
                                }
                                if (UserInfo["是否休眠"].ToString() == "是")
                                {
                                    return "修改失败，您的账号已休眠！";
                                }
                                //if (UserInfo["是否冻结"].ToString() == "是")
                                //{
                                //    return "修改失败，您的账号已被禁止业务！";
                                //}

                            


                                double jg = Convert.ToDouble(dsYDD.Tables[0].Rows[0]["NMRJG"].ToString());//拟买入价格
                                double old_ZFDJJE = Convert.ToDouble(dsYDD.Tables[0].Rows[0]["ZFDJJE"].ToString());//订金金额
                                int ydzsl = Convert.ToInt32(dsYDD.Tables[0].Rows[0]["YDZSL"].ToString() == "" ? "0" : dsYDD.Tables[0].Rows[0]["YDZSL"].ToString());//预定总数量
                                int yzbl = Convert.ToInt32(dsYDD.Tables[0].Rows[0]["YZBL"].ToString() == "" ? "0" : dsYDD.Tables[0].Rows[0]["YZBL"].ToString());//已中标量
                                int xgcs = Convert.ToInt32(dsYDD.Tables[0].Rows[0]["XGCS"].ToString() == "" ? "0" : dsYDD.Tables[0].Rows[0]["XGCS"].ToString());//修改次数

                                


                                int wzbl = ydzsl - yzbl;//未中标量
                                #endregion

                                if (xgcs > 0 && 1==2)
                                {
                                    return "您已修改过，不能再进行修改！";
                                }
                                else
                                {
                                    if (updateSL <= 0)
                                    {
                                        return "修改后的数量必须大于零！";
                                    }
                                    #region//达到修改的条件进行的操作
                                    else
                                    {
                                        int zsl = yzbl + updateSL;
                                        xgcs += 1;
                                        if (zsl == yzbl)
                                        {
                                            strSql = "update ZZ_YDDXXB set NMRJG='" + nmrjg.Trim() + "',YDZSL=" + zsl + ",ZT='中标',XGCS=" + xgcs + " where Number='" + Number.Trim() + "'";
                                            list.Add(strSql);
                                        }
                                        else
                                        {
                                            strSql = "update ZZ_YDDXXB set NMRJG='" + nmrjg.Trim() + "',YDZSL=" + zsl + ",XGCS=" + xgcs + " where Number='" + Number.Trim() + "'";
                                            list.Add(strSql);
                                        }
                                      
                                   

                                        //重新计算订金
                                        Hashtable htParameterInfo = jhjx_PublicClass.GetParameterInfo();
                                        ClassMoney CM = new ClassMoney();
                                        double kyMoney = CM.GetMoneyT(MJDLYX, "模拟而已");

                                        double new_ZFDJJE = 0;
                                        //若不存在已中标数量，则更新新的订金比率，按新的订金比率计算新订金
                                        if (yzbl == 0)
                                        {
                                            strSql = "update ZZ_YDDXXB set ZFDJBL =" + htParameterInfo["订金"].ToString() + " where Number='" + Number.Trim() + "'";
                                            list.Add(strSql);
                                            new_ZFDJJE = Math.Round(zsl * Convert.ToDouble(nmrjg) * Convert.ToDouble(htParameterInfo["订金"]), 2); //新的订金金额
                                        }
                                        else //存在已中标量，则不更新订金比率，按原有订金比率计算新订金
                                        {
                                            new_ZFDJJE = Math.Round(zsl * Convert.ToDouble(nmrjg) * Convert.ToDouble(dsYDD.Tables[0].Rows[0]["ZFDJBL"].ToString()), 2); //新的订金金额
                                        }

                                        
                                        double bujiao = new_ZFDJJE - old_ZFDJJE;
                                        if (bujiao > 0 && kyMoney < bujiao)//需要补交
                                        {
                                            return "提交失败，账户余额不足，需补充缴纳订金" + bujiao + "元，您的账户余额为" + kyMoney + "元！";
                                        }
                                        //资金充足了，解冻原来的订金，并重新冻结新的订金
                                        list.Add("update ZZ_YDDXXB set ZFDJJE=" + new_ZFDJJE + ",DJSYKYJE=" + new_ZFDJJE + "  where Number = '" + Number + "' ");

                                        //插入资金变更明细表SQl(增加)
                                        Hashtable htinfo = new Hashtable();
                                        htinfo["来源单号"] = Number;
                                        htinfo["变动金额"] = old_ZFDJJE.ToString();
                                        htinfo["变动前账户余额"] = kyMoney;
                                        htinfo["变动后账户余额"] = kyMoney + old_ZFDJJE;
                                        htinfo["金额运算类型"] = "返还";
                                        htinfo["变更说明"] = "修改预订单返还原订金";
                                        ArrayList almoney = CM.InsertMoneyUse("修改预订单", htinfo, MJJSBH);
                                        list.AddRange(almoney);
                                        //插入资金变更明细表SQl(减少)
                                        Hashtable htinfo2 = new Hashtable();
                                        htinfo2["来源单号"] = Number;
                                        htinfo2["变动金额"] = new_ZFDJJE;
                                        htinfo2["变动前账户余额"] = kyMoney + old_ZFDJJE;
                                        htinfo2["变动后账户余额"] = kyMoney + old_ZFDJJE - new_ZFDJJE;
                                        htinfo2["金额运算类型"] = "冻结";
                                        htinfo2["变更说明"] = "修改预订单重新冻结订金";
                                        ArrayList almoney2 = CM.InsertMoneyUse("修改预订单", htinfo2, MJJSBH);
                                        list.AddRange(almoney2);


                                        bool bl = DbHelperSQL.ExecSqlTran(list);

                                        //跑跑冷静期
                                        CJGZ cj = new CJGZ();
                                        ArrayList listLJQ = new ArrayList();
                                        listLJQ = cj.RunCJGZ(dt, "提交");
                                        DbHelperSQL.ExecSqlTran(listLJQ);

                                        string re_str = "";
                                        if (bl)
                                        {
                                            //执行成功，调用银行接口，调用两次，一次加，一次减
                                            CM.FreezeMoneyT(MJDLYX, old_ZFDJJE.ToString(), "+");
                                            CM.FreezeMoneyT(MJDLYX, new_ZFDJJE.ToString(), "-");
                                            re_str = "修改成功！";
                                        }
                                        else
                                        {
                                            re_str = "修改失败！";
                                        }



                                        return re_str;
                                    }
                                    #endregion
                                }

                            }
                            else
                            {
                                return "未找到该类商品的预订单！";
                            }
                        }
                        #endregion
                        return "数据集不能为空！";
                    case "撤销":
                        #region//撤销
                        foreach (DataRow row in dt.Rows)
                        {
                            int updateSL = Convert.ToInt32(row["修改的数量"].ToString().Trim());
                            double nmrjg = Convert.ToDouble(row["修改的价格"].ToString());
                            string Number = row["预订单号"].ToString();
                            DataSet dsYDD = jhjx_PublicClass.GetYDDXX(Number, "竞标");
                            if (dsYDD != null && dsYDD.Tables[0].Rows.Count > 0)
                            {
                                #region//获取未修改前对应预订单拟买入价格、预定总数量、已中标量、修改次数、未中标量
                                string MJDLYX = dsYDD.Tables[0].Rows[0]["MJDLYX"].ToString();//登陆账号
                                string MJJSBH = dsYDD.Tables[0].Rows[0]["MJJSBH"].ToString();//买家角色编号
                             

                                Hashtable UserInfo = jhjx_PublicClass.GetUserInfo(MJJSBH);
                                if (MJJSBH.Trim() == "" || UserInfo["是否审核通过"].ToString() != "审核通过")
                                {
                                    return "撤销失败，您尚未开通结算账户！";
                                }
                                if (UserInfo["是否允许登录"].ToString() != "是")
                                {
                                    return "撤销失败，您已被禁止登录！";
                                }
                                if (!jhjx_PublicClass.IsOpenDay() || !jhjx_PublicClass.IsOpenTime())
                                {
                                    return "撤销失败，当前时间业务停止运行！";
                                }
                                if (UserInfo["是否休眠"].ToString() == "是")
                                {
                                    return "撤销失败，您的账号已休眠！";
                                }
                                //if (UserInfo["是否冻结"].ToString() == "是")
                                //{
                                //    return "撤销失败，您的账号已被禁止业务！";
                                //}


                                int ydzsl = Convert.ToInt32(dsYDD.Tables[0].Rows[0]["YDZSL"].ToString() == "" ? "0" : dsYDD.Tables[0].Rows[0]["YDZSL"].ToString());//预定总数量
                                int yzbl = Convert.ToInt32(dsYDD.Tables[0].Rows[0]["YZBL"].ToString() == "" ? "0" : dsYDD.Tables[0].Rows[0]["YZBL"].ToString());//已中标量

                                double jg = Convert.ToDouble(dsYDD.Tables[0].Rows[0]["NMRJG"].ToString());//拟买入价格
                                double old_ZFDJJE = Math.Round((ydzsl-yzbl) * Convert.ToDouble(nmrjg) * Convert.ToDouble(dsYDD.Tables[0].Rows[0]["ZFDJBL"].ToString()), 2); //需要撤销的订金金额
                                
                                int xgcs = Convert.ToInt32(dsYDD.Tables[0].Rows[0]["XGCS"].ToString() == "" ? "0" : dsYDD.Tables[0].Rows[0]["YZBL"].ToString());//修改次数
                                int wzbl = ydzsl - yzbl;//未中标量
                                #endregion


                                if (yzbl == 0)
                                {
                                    strSql = "update ZZ_YDDXXB set ZT='撤销' where number='" + Number.Trim() + "'";
                                    list.Add(strSql);
                                }
                                else
                                {
                                    strSql = "update ZZ_YDDXXB set YDZSL=YZBL,ZT='中标' where Number='" + Number.Trim() + "'";
                                    list.Add(strSql);
                                }



                                //返还订金
                                DataSet ds_old_jia = DbHelperSQL.Query("select isnull(TBBZJSYKYJE,0) from ZZ_TBXXB where Number = '" + Number + "' ");//获得原来的投标保证金金额
                                ClassMoney CM = new ClassMoney();
                                double kyMoney = CM.GetMoneyT(UserInfo["登录邮箱"].ToString(), "模拟而已");
                                //插入资金变更明细表SQl(增加)
                                Hashtable htinfo = new Hashtable();
                                htinfo["来源单号"] = Number;
                                htinfo["变动金额"] = old_ZFDJJE.ToString();
                                htinfo["变动前账户余额"] = kyMoney;
                                htinfo["变动后账户余额"] = kyMoney + old_ZFDJJE;
                                ArrayList almoney = CM.InsertMoneyUse("撤销预订单", htinfo, MJJSBH);
                                list.AddRange(almoney);

                                //跑一遍，看是否进入冷静期
                                CJGZ cjgz = new CJGZ();
                                ArrayList alsql2 = new ArrayList();
                                alsql2 = cjgz.RunCJGZ(dt, "撤销");


                                bool bl = DbHelperSQL.ExecSqlTran(list);
                                if (bl)
                                {
                                    //执行成功，调用银行接口，
                                    CM.FreezeMoneyT(UserInfo["登录邮箱"].ToString(), old_ZFDJJE.ToString(), "+");

                                    return "撤销成功！";
                                }
                                else
                                {
                                    return "撤销失败！";
                                }





                            }
                            else
                            {
                                return "未找到该类商品的预订单！";
                            }
                        }
                        #endregion
                        return "数据集不能为空！";
                    default:
                        return "操作不正确！";
                }


            }
            else
            {
                return "数据集不能为空！";
            }
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }

 
    }
}