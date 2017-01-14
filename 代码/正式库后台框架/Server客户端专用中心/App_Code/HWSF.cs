/*******************************************************************
 * 
 * 创建人：zhouli
 *
 *创建时间：2014.07.01
 *
 *代码功能：实现货物收发各项功能
 *
 * 
 * ******************************************************************/
using FMDBHelperClass;
using FMipcClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// HWSF 的摘要说明 
/// </summary>
public class HWSF
{
	public HWSF()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 下达提货单--zhouli 2014.07.01 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public static DataSet SaveTHD(DataSet ds, DataSet dsreturn)
    {
        try
        { 
            //====验证传递的参数是否为空
            if (ds==null||ds.Tables.Count<1||ds.Tables[0].Rows.Count<1)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
                return dsreturn;
            }
            string DLYX = ds.Tables[0].Rows[0]["登陆邮箱"].ToString();

            #region//1.验证交易账户是否开通
            object[] re = IPC.Call("交易账户开通状态", new object[] { DLYX });
            if (re[0].ToString() == "ok")
            {
                DataSet dsjszh = (DataSet)(re[1]);
                if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
                {
                    if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                        }
                        else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                        }
                        return dsreturn;

                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }

            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//2.验证是否在服务时间内
            re = IPC.Call("平台动态参数", new object[] { "" });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    DataRow JCYZ = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(工作时间)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到平台动态参数信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//3.验证是否休眠
            DataRow JBXX = null;
            re = IPC.Call("用户基本信息", new object[] { DLYX });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    JBXX = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JBXX["是否休眠"].ToString() == "是")
                    {
                        //if (JBXX["是否冻结"].ToString() == "是" || JBXX["冻结功能项"].ToString().Trim()!="")//zhouili 作废 2014.08.18 需求变更
                        //{
                            if (JBXX["冻结功能项"].ToString().Contains("下达提货单"))
                            {
                                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！\n\r您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + JBXX["冻结功能项"].ToString();
                                return dsreturn;
                            }
                        //}

                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//4.验证交易账户是否冻结
            //if (JBXX["是否冻结"].ToString() == "是" || JBXX["冻结功能项"].ToString() != "")//zhouili 作废 2014.08.18 需求变更
            //{
                if (JBXX["冻结功能项"].ToString().Contains("下达提货单"))
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + JBXX["冻结功能项"].ToString();
                    return dsreturn;
                }
            //}
            #endregion
            #region//准备参数

            #region//为参数ds添加新列
            ds.Tables[0].Columns.Add("买家角色编号");
            ds.Tables[0].Columns.Add("结算账户类型");
            ds.Tables[0].Columns.Add("发票抬头");
            ds.Tables[0].Columns.Add("合同编号");
            ds.Tables[0].Columns.Add("经济批量");
            ds.Tables[0].Columns.Add("中标单价");
            ds.Tables[0].Columns.Add("原始投标单单号");
            #endregion
            #region//为新列"买家角色编号"、"结算账户类型"、"发票抬头"赋值
            ds.Tables[0].Rows[0]["买家角色编号"] = JBXX["买家角色编号"].ToString();
            ds.Tables[0].Rows[0]["结算账户类型"] = JBXX["结算账户类型"].ToString();
            ds.Tables[0].Rows[0]["发票抬头"] = JBXX["交易方名称"].ToString();
            #endregion
            #region//为新列"合同编号"、"经济批量"、"中标单价"、“原始投标单单号”赋值
            Hashtable htCS = new Hashtable();
            htCS["@Number"] = ds.Tables[0].Rows[0]["中标定标信息表编号"].ToString();
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable ReturnDataSet = I_DBL.RunParam_SQL("select  Z_HTBH '合同编号',T_YSTBDMJSDJJPL '经济批量',Z_ZBJG '中标单价', T_YSTBDBH '原始投标单单号'  from AAA_ZBDBXXB where Number=@Number ", "", htCS);
            if (!(bool)ReturnDataSet["return_float"])
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = (string)ReturnDataSet["return_errmsg"];
                return dsreturn;
            }
            if (ReturnDataSet["return_ds"] == null || ((DataSet)ReturnDataSet["return_ds"]).Tables.Count < 1 || ((DataSet)ReturnDataSet["return_ds"]).Tables[0].Rows.Count < 1)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到中标定标信息表信息！";
                return dsreturn;
            }

            ds.Tables[0].Rows[0]["合同编号"] = ((DataSet)ReturnDataSet["return_ds"]).Tables[0].Rows[0]["合同编号"].ToString();
            ds.Tables[0].Rows[0]["经济批量"] = ((DataSet)ReturnDataSet["return_ds"]).Tables[0].Rows[0]["经济批量"].ToString();
            ds.Tables[0].Rows[0]["中标单价"] = ((DataSet)ReturnDataSet["return_ds"]).Tables[0].Rows[0]["中标单价"].ToString();
            ds.Tables[0].Rows[0]["原始投标单单号"] = ((DataSet)ReturnDataSet["return_ds"]).Tables[0].Rows[0]["原始投标单单号"].ToString();
            #endregion

            #endregion
            #region//调用成交履约中心的“保存提货单”
            re = IPC.Call("保存提货单", new object[] { ds });
            if (re[0].ToString() == "ok")
            {
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    dsreturn = (DataSet)re[1];
                    return dsreturn;
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }

            #endregion

        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }
    }
    /// <summary>
    /// 生成发货单--zhouli 2014.07.02 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public static DataSet SCFHD(DataSet ds, DataSet dsreturn)
    {
        try
        {
            //====验证传递的参数是否为空
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
                return dsreturn;
            }

            string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();//卖家角色编号

            #region//1.验证是否在服务时间内
            object[] re = IPC.Call("平台动态参数", new object[] { "" });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    DataRow JCYZ = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(工作时间)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到平台动态参数信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//2.验证交易账户是否开通

            re = IPC.Call("交易账户开通状态", new object[] { JSBH });
            if (re[0].ToString() == "ok")
            {
                DataSet dsjszh = (DataSet)(re[1]);
                if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
                {
                    if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                        }
                        else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                        }
                        return dsreturn;

                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }

            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion            
            #region//3.验证是否休眠
            DataRow JBXX = null;
            re = IPC.Call("用户基本信息", new object[] { JSBH });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    JBXX = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JBXX["是否休眠"].ToString() == "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion

            re = IPC.Call("生成发货单", new object[] {ds });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    dsreturn = (DataSet)re[1];
                    return dsreturn;
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }
    }
    /// <summary>
    /// 录入发货信息--zhouli 2014.07.03 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public static DataSet LRFHXX(DataSet ds, DataSet dsreturn)
    {
        try
        {
            //====验证传递的参数是否为空
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
                return dsreturn;
            }
            string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();//卖家角色编号

            #region//1.验证交易账户是否开通

            object[] re = IPC.Call("交易账户开通状态", new object[] { JSBH });
            if (re[0].ToString() == "ok")
            {
                DataSet dsjszh = (DataSet)(re[1]);
                if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
                {
                    if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                        }
                        else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                        }
                        return dsreturn;

                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }

            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//2.验证是否在服务时间内
            re = IPC.Call("平台动态参数", new object[] { "" });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    DataRow JCYZ = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(工作时间)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到平台动态参数信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//3.验证是否休眠
            DataRow JBXX = null;
            re = IPC.Call("用户基本信息", new object[] { JSBH });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    JBXX = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JBXX["是否休眠"].ToString() == "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion

            re = IPC.Call("录入发货信息", new object[] { ds });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    dsreturn = (DataSet)re[1];
                    return dsreturn;
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "发货信息录入失败！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }
    }

    /// <summary>
    /// 录入发票信息--zhouli 2014.07.03 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public static DataSet LRFPXX(DataSet ds, DataSet dsreturn)
    {
        try
        {
            //====验证传递的参数是否为空
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
                return dsreturn;
            }
            string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();//卖家角色编号

            #region//1.验证交易账户是否开通
            object[] re = IPC.Call("交易账户开通状态", new object[] { JSBH });
            if (re[0].ToString() == "ok")
            {
                DataSet dsjszh = (DataSet)(re[1]);
                if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
                {
                    if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                        }
                        else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                        }
                        return dsreturn;

                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }

            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//2.验证是否在服务时间内
            re = IPC.Call("平台动态参数", new object[] { "" });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    DataRow JCYZ = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(工作时间)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到平台动态参数信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//3.验证是否休眠
            DataRow JBXX = null;
            re = IPC.Call("用户基本信息", new object[] { JSBH });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    JBXX = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JBXX["是否休眠"].ToString() == "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion

            re = IPC.Call("录入发票信息", new object[] { ds });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    dsreturn = (DataSet)re[1];
                    return dsreturn;
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "发票信息录入失败！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }
    }

    /// <summary>
    /// 提请买家签收--zhouli 2014.07.03 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public static DataSet InsertSQL(DataSet ds, DataSet dsreturn)
    {
        try
        {
            //====验证传递的参数是否为空
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
                return dsreturn;
            }
            string JSBH = ds.Tables[0].Rows[0]["卖家角色编号"].ToString();

            #region//1.验证交易账户是否开通
            object[] re = IPC.Call("交易账户开通状态", new object[] { JSBH });
            if (re[0].ToString() == "ok")
            {
                DataSet dsjszh = (DataSet)(re[1]);
                if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
                {
                    if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                        }
                        else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                        }
                        return dsreturn;

                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }

            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//2.验证是否在服务时间内
            re = IPC.Call("平台动态参数", new object[] { "" });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    DataRow JCYZ = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(工作时间)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到平台动态参数信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//3.验证是否休眠
            DataRow JBXX = null;
            re = IPC.Call("用户基本信息", new object[] { JSBH });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    JBXX = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JBXX["是否休眠"].ToString() == "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion

            re = IPC.Call("提请买家签收", new object[] { ds });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    dsreturn = (DataSet)re[1];
                    return dsreturn;
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "发票信息录入失败！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }

        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }
    }

    /// <summary>
    /// 货物签收--zhouli 2014.07.03 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public static DataSet WYYQS(DataTable ds, DataSet dsreturn)
    {
        try
        {
            //====验证传递的参数是否为空
            if (ds == null || ds.Rows.Count < 1)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
                return dsreturn;
            }
            string DLYX = ds.Rows[0]["登陆邮箱"].ToString();
            
            #region//1.验证是否在服务时间内
            object[] re = IPC.Call("平台动态参数", new object[] { "" });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    DataRow JCYZ = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(工作时间)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到平台动态参数信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//2.验证交易账户是否开通
            re = IPC.Call("交易账户开通状态", new object[] { DLYX });
            if (re[0].ToString() == "ok")
            {
                DataSet dsjszh = (DataSet)(re[1]);
                if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
                {
                    if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                        }
                        else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                        }
                        return dsreturn;

                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }

            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//3.验证是否休眠
            DataRow JBXX = null;
            re = IPC.Call("用户基本信息", new object[] { DLYX });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    JBXX = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JBXX["是否休眠"].ToString() == "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//4.准备参数
            ds.Columns.Add("买家角色编号");
            ds.Columns.Add("结算账户类型");

            ds.Rows[0]["买家角色编号"] = JBXX["买家角色编号"].ToString();
            ds.Rows[0]["结算账户类型"] = JBXX["结算账户类型"].ToString();
            #endregion
            re = IPC.Call("货物签收",new object[]{ds});
           if (re[0].ToString() == "ok")
           {
               //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
               if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
               {
                   dsreturn = (DataSet)re[1];
                   return dsreturn;
               }
               else
               {
                   dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                   dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] ="无异议收货操作失败！";
                   return dsreturn;
               }
           }
           else
           {
               //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
               dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
               dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
               return dsreturn;
           }

        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }
    }
    /// <summary>
    /// 问题与处理--zhouli 2014.07.08 add
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="dsreturn"></param>
    /// <returns></returns>
    public static DataSet WTYCL(DataSet ds, DataSet dsreturn)
    {
        try
        {
            //====验证传递的参数是否为空
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空！";
                return dsreturn;
            }
            string JSBH = ds.Tables[0].Rows[0]["买家角色编号"].ToString();

            #region//1.验证是否在服务时间内
            object[] re = IPC.Call("平台动态参数", new object[] { "" });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    DataRow JCYZ = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JCYZ["是否在服务时间内(假期)"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(假期)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间(工作时间)，暂停交易！";
                        return dsreturn;
                    }
                    if (JCYZ["是否在服务时间内"].ToString().Trim() != "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到平台动态参数信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//2.验证交易账户是否开通
            re = IPC.Call("交易账户开通状态", new object[] { JSBH });
            if (re[0].ToString() == "ok")
            {
                DataSet dsjszh = (DataSet)(re[1]);
                if (dsjszh != null && dsjszh.Tables[0].Rows.Count > 0)
                {
                    if (dsjszh.Tables[0].Rows[0]["结算账户类型"].ToString().Trim() == "" || dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim() != "已通过")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("审核中"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请正在审核中，请耐心等待！ ";
                        }
                        else if (dsjszh.Tables[0].Rows[0]["审核状态"].ToString().Trim().Equals("驳回"))
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请进入“账户维护”界面\n\r查询详情并重新提交申请！ ";
                        }
                        else
                        {
                            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未提交开通交易账户申请，请及时提交！ ";
                        }
                        return dsreturn;

                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }

            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//3.验证是否休眠
            DataRow JBXX = null;
            re = IPC.Call("用户基本信息", new object[] { JSBH });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    JBXX = ((DataSet)re[1]).Tables[0].Rows[0];
                    if (JBXX["是否休眠"].ToString() == "是")
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                        return dsreturn;
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前账户的信息！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }
            #endregion
            #region//4.准备参数
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            string CZ = ds.Tables[0].Rows[0]["操作"].ToString();
            switch (CZ)
            {
                #region//请买方“无异议收货”
                case "请买方“无异议收货”":
                    ds.Tables[0].Columns.Add("卖家角色编号");
                    ds.Tables[0].Columns.Add("结算账户类型");
                    ds.Tables[0].Columns.Add("发货单号");
                    ds.Tables[0].Rows[0]["卖家角色编号"] = JBXX["卖家角色编号"].ToString();
                    ds.Tables[0].Rows[0]["结算账户类型"] = JBXX["结算账户类型"].ToString();
                    ds.Tables[0].Rows[0]["发货单号"] = ds.Tables[0].Rows[0]["提货单编号"].ToString();
                    break;
                #endregion
                #region//卖方主动退货
                case "卖方主动退货":
                    ds.Tables[0].Columns.Add("登录邮箱");
                    ds.Tables[0].Columns.Add("结算账户类型");
                    ds.Tables[0].Columns.Add("卖家角色编号");
                    ds.Tables[0].Columns.Add("卖家登录邮箱");
                    ds.Tables[0].Columns.Add("买家登录邮箱");
                    ds.Tables[0].Columns.Add("提醒对象用户名");
                    ds.Tables[0].Columns.Add("提醒对象结算账户类型");
                    ds.Tables[0].Columns.Add("提醒对象角色编号");
                    ds.Tables[0].Columns.Add("创建人");
                    ds.Tables[0].Rows[0]["登录邮箱"] = JBXX["登录邮箱"].ToString();
                    ds.Tables[0].Rows[0]["结算账户类型"] = JBXX["结算账户类型"].ToString();
                    ds.Tables[0].Rows[0]["卖家角色编号"] = JBXX["卖家角色编号"].ToString();
                    ds.Tables[0].Rows[0]["卖家登录邮箱"] = JBXX["登录邮箱"].ToString();

                    Hashtable htcs = new Hashtable();
                    htcs["@YDDBH"] = ds.Tables[0].Rows[0]["提货单编号"].ToString().TrimStart('T');
                    string strSQL = "select Y_YSYDDDLYX '原始预订单登录邮箱',( select I_JYFMC from AAA_DLZHXXB where B_DLYX= Y_YSYDDDLYX) '买家名称',Y_YSYDDJSZHLX '原始预订单结算账户类型',Y_YSYDDMJJSBH '原始预订单买家角色编号',T_YSTBDDLYX '原始投标单登陆邮箱' from AAA_ZBDBXXB where  Number=(select ZBDBXXBBH from AAA_THDYFHDXXB where Number=@YDDBH)";
                   Hashtable returnHT =  I_DBL.RunParam_SQL(strSQL, "", htcs);
                   if (!(bool)returnHT["return_float"])
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取中标定标信息失败！";
                        return dsreturn;
                    }
                   if (returnHT["return_ds"] == null || ((DataSet)returnHT["return_ds"]).Tables.Count < 1 || ((DataSet)returnHT["return_ds"]).Tables[0].Rows.Count < 1)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取中标定标信息失败！";
                        return dsreturn;
                    }
                   DataRow ZBDBMsg = ((DataSet)returnHT["return_ds"]).Tables[0].Rows[0];
                   ds.Tables[0].Rows[0]["买家登录邮箱"] = ZBDBMsg["原始预订单登录邮箱"].ToString();
                   ds.Tables[0].Rows[0]["提醒对象用户名"] = ZBDBMsg["买家名称"].ToString();
                   ds.Tables[0].Rows[0]["提醒对象结算账户类型"] = ZBDBMsg["原始预订单结算账户类型"].ToString();
                   ds.Tables[0].Rows[0]["提醒对象角色编号"] = ZBDBMsg["原始预订单买家角色编号"].ToString();
                   ds.Tables[0].Rows[0]["创建人"] = ZBDBMsg["原始投标单登陆邮箱"].ToString();
                    break;
                #endregion
                #region//同意重新发货
                case "同意重新发货":
                    ds.Tables[0].Columns.Add("Number");
                    ds.Tables[0].Columns.Add("登录邮箱");
                    ds.Tables[0].Columns.Add("结算账户类型");
                    ds.Tables[0].Columns.Add("卖家角色编号");
                    ds.Tables[0].Columns.Add("提醒对象登陆邮箱");
                    ds.Tables[0].Columns.Add("提醒对象用户名");
                    ds.Tables[0].Columns.Add("提醒对象结算账户类型");
                    ds.Tables[0].Columns.Add("提醒对象角色编号");
                    ds.Tables[0].Columns.Add("创建人");
                    ds.Tables[0].Columns.Add("原始投标单单号");
                    ds.Tables[0].Rows[0]["Number"] = ds.Tables[0].Rows[0]["提货单编号"].ToString();
                    ds.Tables[0].Rows[0]["登录邮箱"] = JBXX["登录邮箱"].ToString();
                    ds.Tables[0].Rows[0]["结算账户类型"] = JBXX["结算账户类型"].ToString();
                    ds.Tables[0].Rows[0]["卖家角色编号"] = JBXX["卖家角色编号"].ToString();

                    htcs = new Hashtable();
                    htcs["@YDDBH"] = ds.Tables[0].Rows[0]["提货单编号"].ToString().TrimStart('T');
                    strSQL = "select T_YSTBDBH  '原始投标单单号',Y_YSYDDDLYX '原始预订单登录邮箱',( select I_JYFMC from AAA_DLZHXXB where B_DLYX= Y_YSYDDDLYX) '买家名称',Y_YSYDDJSZHLX '原始预订单结算账户类型',Y_YSYDDMJJSBH '原始预订单买家角色编号',T_YSTBDDLYX '原始投标单登陆邮箱' from AAA_ZBDBXXB where Number=(select ZBDBXXBBH from AAA_THDYFHDXXB where Number=@YDDBH)";
                   returnHT =  I_DBL.RunParam_SQL(strSQL, "", htcs);
                   if (!(bool)returnHT["return_float"])
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取中标定标信息失败！";
                        return dsreturn;
                    }
                   if (returnHT["return_ds"] == null || ((DataSet)returnHT["return_ds"]).Tables.Count < 1 || ((DataSet)returnHT["return_ds"]).Tables[0].Rows.Count < 1)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取中标定标信息失败！";
                        return dsreturn;
                    }
                   ZBDBMsg = ((DataSet)returnHT["return_ds"]).Tables[0].Rows[0];
                   ds.Tables[0].Rows[0]["提醒对象登陆邮箱"] = ZBDBMsg["原始预订单登录邮箱"].ToString();
                   ds.Tables[0].Rows[0]["提醒对象用户名"] = ZBDBMsg["买家名称"].ToString();
                   ds.Tables[0].Rows[0]["提醒对象结算账户类型"] = ZBDBMsg["原始预订单结算账户类型"].ToString();
                   ds.Tables[0].Rows[0]["提醒对象角色编号"] = ZBDBMsg["原始预订单买家角色编号"].ToString();
                   ds.Tables[0].Rows[0]["创建人"] = ZBDBMsg["原始投标单登陆邮箱"].ToString();
                   ds.Tables[0].Rows[0]["原始投标单单号"] = ZBDBMsg["原始投标单单号"].ToString();
                    break;
                #endregion
                #region//无异议收货
                case "无异议收货":
                    ds.Tables[0].Columns.Add("买家名称");
                    ds.Tables[0].Columns.Add("Number");
                    ds.Tables[0].Columns.Add("登陆邮箱");
                    ds.Tables[0].Columns.Add("结算账户类型");
                    ds.Tables[0].Columns.Add("提醒对象登陆邮箱");
                    ds.Tables[0].Columns.Add("提醒对象用户名");
                    ds.Tables[0].Columns.Add("提醒对象结算账户类型");
                    ds.Tables[0].Columns.Add("提醒对象角色编号");
                    ds.Tables[0].Columns.Add("创建人");
                    ds.Tables[0].Rows[0]["买家名称"] = JBXX["交易方名称"].ToString();
                    ds.Tables[0].Rows[0]["Number"] = ds.Tables[0].Rows[0]["提货单编号"].ToString();
                    ds.Tables[0].Rows[0]["登陆邮箱"] = JBXX["登录邮箱"].ToString();
                    ds.Tables[0].Rows[0]["结算账户类型"] = JBXX["结算账户类型"].ToString();                    

                    htcs = new Hashtable();
                    htcs["@YDDBH"] = ds.Tables[0].Rows[0]["提货单编号"].ToString().TrimStart('T');
                    strSQL = "select Y_YSYDDDLYX '原始预订单登录邮箱',( select I_JYFMC from AAA_DLZHXXB where B_DLYX= Y_YSYDDDLYX) '买家名称',Y_YSYDDJSZHLX '原始预订单结算账户类型',Y_YSYDDMJJSBH '原始预订单买家角色编号',T_YSTBDDLYX '原始投标单登陆邮箱' from AAA_ZBDBXXB where Number=(select ZBDBXXBBH from AAA_THDYFHDXXB where Number=@YDDBH)";
                   returnHT =  I_DBL.RunParam_SQL(strSQL, "", htcs);
                   if (!(bool)returnHT["return_float"])
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取中标定标信息失败！";
                        return dsreturn;
                    }
                   if (returnHT["return_ds"] == null || ((DataSet)returnHT["return_ds"]).Tables.Count < 1 || ((DataSet)returnHT["return_ds"]).Tables[0].Rows.Count < 1)
                    {
                        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取中标定标信息失败！";
                        return dsreturn;
                    }
                   ZBDBMsg = ((DataSet)returnHT["return_ds"]).Tables[0].Rows[0];
                   ds.Tables[0].Rows[0]["提醒对象登陆邮箱"] = ZBDBMsg["原始预订单登录邮箱"].ToString();
                   ds.Tables[0].Rows[0]["提醒对象用户名"] = ZBDBMsg["买家名称"].ToString();
                   ds.Tables[0].Rows[0]["提醒对象结算账户类型"] = ZBDBMsg["原始预订单结算账户类型"].ToString();
                   ds.Tables[0].Rows[0]["提醒对象角色编号"] = ZBDBMsg["原始预订单买家角色编号"].ToString();
                   ds.Tables[0].Rows[0]["创建人"] = ZBDBMsg["原始投标单登陆邮箱"].ToString();
                    break;
                #endregion
                default:
                    break;
            }
            #endregion

            re = IPC.Call("问题与处理", new object[] { ds });
            if (re[0].ToString() == "ok")
            {
                //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
                if (re[1] != null && ((DataSet)re[1]).Tables.Count > 0 && ((DataSet)re[1]).Tables[0].Rows.Count > 0)
                {
                    dsreturn = (DataSet)re[1];
                    return dsreturn;
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败！";
                    return dsreturn;
                }
            }
            else
            {
                //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。            
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = re[1].ToString();
                return dsreturn;
            }


        }
        catch(Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = ex.Message;
            return dsreturn;
        }
    }

}