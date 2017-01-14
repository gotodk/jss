using FMDBHelperClass;
using Hesion.Brick.Core.WorkFlow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Index_XTKZ 的摘要说明
/// </summary>
public class Index_XTKZ
{
    public Index_XTKZ()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 初始化返回值数据集,执行结果只有两种ok和err
    /// </summary>
    /// <returns></returns>
    public static DataSet initReturnDataSet()
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

    /// <summary>
    /// 将哈希表转化成只有一行的数据表，只能用字符串值，其他类型参数单独声明
    /// </summary>
    /// <param name="HTforParameter">要转化的哈希表</param>
    /// <returns></returns>
    public static DataTable GetDataTableFormHashtable(Hashtable HTforParameter)
    {
        DataTable dsforparameter = new DataTable();
        dsforparameter.TableName = "传递参数";
        ArrayList zhi = new ArrayList();
        foreach (System.Collections.DictionaryEntry item in HTforParameter)
        {
            dsforparameter.Columns.Add(item.Key.ToString(), typeof(string));
            zhi.Add(item.Value.ToString());
        }
        dsforparameter.Rows.Add(zhi.ToArray());
        return dsforparameter;
    }

    /// <summary>
    /// 发送提醒
    /// </summary>
    /// <param name="ds">1、必备字段：row["type"]={集合经销平台、业务平台、手机短信、邮箱提醒；}、row["提醒对象登陆邮箱"]、row["提醒对象结算账户类型"]、row["提醒对象角色编号"]、row["提醒对象角色类型"]、 row["提醒内容文本"]、row["创建人"]、row["查看地址"]{可为空}、row["模提醒模块名"]；
    /// </param>
    public static string Sendmes_XTKZ(DataSet ds)
    {
        try
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

                Hashtable ht_PTTXXXB = new Hashtable();

                DateTime dt = DateTime.Now;
                string Insert = "";
                string KeyNumber = "";
                int ct = 0;//用于记录SQL是否正确执行
                ArrayList list = new ArrayList();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow ht = ds.Tables[0].Rows[i];
                    #region//集合集合经销平台
                    if (ht["type"].ToString().Equals("集合经销平台"))
                    {
                        KeyNumber = GetNextNumberZZ_XTKZ("AAA_PTTXXXB", "");
                        //集合经销平台平台提醒  
                        Insert = " insert into  AAA_PTTXXXB ( Number, TXDXDLYX,TXDXYHM,TXDXJSZHLX,TXDXJSBH,TXDXJSLX,TXSJ,TXNRWB,SFXSG,CreateUser ) values ( @Number" + i.ToString() + ",@TXDXDLYX" + i.ToString() + ",'  ',@TXDXJSZHLX" + i.ToString() + ",@TXDXJSBH" + i.ToString() + ",@TXDXJSLX" + i.ToString() + ",@TXSJ" + i.ToString() + ",@TXNRWB" + i.ToString() + ",'否',@CreateUser" + i.ToString() + ") ";
                        list.Add(Insert);

                        ht_PTTXXXB["@Number" + i.ToString()] = KeyNumber;
                        ht_PTTXXXB["@TXDXDLYX" + i.ToString()] = ht["提醒对象登陆邮箱"].ToString();
                        ht_PTTXXXB["@TXDXJSZHLX" + i.ToString()] = ht["提醒对象结算账户类型"].ToString();
                        ht_PTTXXXB["@TXDXJSBH" + i.ToString()] = ht["提醒对象角色编号"].ToString();
                        ht_PTTXXXB["@TXDXJSLX" + i.ToString()] = ht["提醒对象角色类型"].ToString();
                        ht_PTTXXXB["@TXSJ" + i.ToString()] = dt;
                        ht_PTTXXXB["@TXNRWB" + i.ToString()] = ht["提醒内容文本"].ToString();
                        ht_PTTXXXB["@CreateUser" + i.ToString()] = ht["创建人"].ToString();
                    }
                    #endregion
                    #region//业务平台
                    if (ht["type"].ToString().Equals("业务平台"))
                    {
                        string Strurl = "commonpage.aspx";
                        if (!string.IsNullOrEmpty(ht["查看地址"].ToString().Trim()) && ht["查看地址"].ToString().Trim() != "#")
                        {
                            Strurl = ht["查看地址"].ToString().Trim();
                        }
                        Hashtable ht_touserCS = new Hashtable();
                        ht_touserCS["@txmk" + i.ToString()] = ht["模提醒模块名"].ToString();
                        Hashtable ht_touser = I_DBL.RunParam_SQL("select * from AAA_DYWCZPTDTXPZB where txmk=@txmk" + i.ToString(), "", ht_touserCS);
                        if (!(bool)ht_touser["return_float"])
                        {
                            return null;
                        }
                        DataSet ds_touser = (DataSet)ht_touser["return_ds"];

                        if (ds_touser.Tables[0].Rows.Count > 0)
                        {
                            for (int dsrowcount = 0; dsrowcount < ds_touser.Tables[0].Rows.Count; dsrowcount++)
                            {
                                //分为两种情况,如果员工编号为空，则向岗位批量发送提醒。
                                if (ds_touser.Tables[0].Rows[dsrowcount]["YGBH"].ToString() == "")
                                {
                                    string ssql = "select number,employee_name,bm,gwmc from HR_employees where  ygzt not like '%离职%' and gwmc=@gwmc and BM=@BM";

                                    Hashtable ht_touserCS2 = new Hashtable();
                                    ht_touserCS2["@gwmc" + i.ToString() + dsrowcount.ToString()] = ds_touser.Tables[0].Rows[dsrowcount]["GW"].ToString();
                                    ht_touserCS2["@BM" + i.ToString() + dsrowcount] = ds_touser.Tables[0].Rows[dsrowcount]["BM"].ToString();
                                    I_Dblink I_DBL2 = (new DBFactory()).DbLinkSqlMain("newFMOPkf");
                                    Hashtable ht_touser2 = I_DBL2.RunParam_SQL(ssql, "", ht_touserCS2);
                                    if (!(bool)ht_touser2["return_float"])
                                    {
                                        return null;
                                    }
                                    DataSet ds_touser2 = (DataSet)ht_touser2["return_ds"];

                                    if (ds_touser2 != null && ds_touser2.Tables[0].Rows.Count > 0)
                                    {

                                        for (int t = 0; t < ds_touser2.Tables[0].Rows.Count; t++)
                                        {
                                            string touser = ds_touser2.Tables[0].Rows[t]["number"].ToString();
                                            WorkFlowModule wf = new WorkFlowModule("AAA_ZBDBXXB");
                                            int count = wf.authentication.InsertWarnings(ht["提醒内容文本"].ToString(), Strurl, "1", "admin", touser);
                                            if (count > 0)
                                            {
                                                ct++;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    string touser = ds_touser.Tables[0].Rows[dsrowcount]["ygbh"].ToString();
                                    WorkFlowModule wf = new WorkFlowModule("AAA_ZBDBXXB");
                                    int count = wf.authentication.InsertWarnings(ht["提醒内容文本"].ToString(), Strurl, "1", "admin", touser);
                                    if (count > 0)
                                    {
                                        ct++;
                                    }
                                }
                            }
                        }


                    }
                    #endregion
                    #region//手机短信、邮箱提醒
                    if (ht["type"].ToString().Equals("手机短信"))
                    {
                        //手机提醒

                    }
                    if (ht["type"].ToString().Equals("邮箱提醒"))
                    {
                        //手机提醒

                    }
                    #endregion
                }
                if (!string.IsNullOrEmpty(Insert))
                {
                    DataSet ds_PTTXXXB = new DataSet();
                    Hashtable returnHT = I_DBL.RunParam_SQL(list, ht_PTTXXXB);
                    if (returnHT["return_other"] != null && Convert.ToInt32(returnHT["return_other"]) > 0)
                    {
                        ct++;
                    }
                }
                if (ct > 0)
                {
                    return "发送提醒成功";
                }
                else
                {
                    return "发送提醒失败";
                }
            }
            return "发送提醒失败";
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// 通过模块名和开头标记字母，获得下一个编号
    /// </summary>
    /// <param name="ModuleName">模块名【表名】</param>
    /// <param name="HeadStr">开头标记字母</param>
    /// <returns></returns>
    //public static string GetNextNumberZZ_XTKZ(string ModuleName, string HeadStr)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(ModuleName))
    //        {
    //            return null;
    //        }
    //        WorkFlowModule WFM = new WorkFlowModule(ModuleName);
    //        return WFM.numberFormat.GetNextNumberZZ(HeadStr);
    //    }
    //    catch(Exception)
    //    {
    //        return null;
    //    }
    //}

    public static string GetNextNumberZZ_XTKZ(string ModuleName, string HeadStr)
    {//2014-09-12 shiyan 因变更编号获取方法重写
        try
        {
            if (string.IsNullOrEmpty(ModuleName))
            {
                return null;
            }
            string KeyNumber = null;
            Hashtable input = new Hashtable();
            input["@ModuleName"] = ModuleName;

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("IPCsqlserver");
            Hashtable ht_return = I_DBL.RunProc_CMD("dbo.GetSequence", "data", input);
            if ((bool)ht_return["return_float"])
            {
                DataSet ds_key = (DataSet)ht_return["return_ds"];
                if (ds_key != null && ds_key.Tables.Count > 0 && ds_key.Tables[0].Rows.Count > 0 && ds_key.Tables[0].Rows[0]["KeyNumber"].ToString() != "")
                {
                    KeyNumber = HeadStr + ds_key.Tables[0].Rows[0]["KeyNumber"].ToString();
                }
            }
            return KeyNumber;
        }
        catch (Exception)
        {
            return null;
        }
    }
    /// <summary>
    /// 检查提醒
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <returns></returns>
    public static DataSet CheckFormTrayMsg_XTKZ(string dlyx)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Copy();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            if (string.IsNullOrEmpty(dlyx))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空";
                return dsreturn;
            }

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            //检查是否要踢人
            //可以登陆时"ok"   要踢人时"提醒内容"
            string T_restr = XZDLYX.GetOpen(dlyx.Trim());
            if (T_restr != "ok")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "要踢人";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = T_restr;
                return dsreturn;
            }

            //执行语句                 
            Hashtable htCS = new Hashtable();
            htCS["@TXDXDLYX"] = dlyx;
            Hashtable HTSelect = I_DBL.RunParam_SQL("select * FROM AAA_PTTXXXB WHERE TXDXDLYX=@TXDXDLYX and SFXSG = '否'", "", htCS);
            if (!(bool)HTSelect["return_float"])
            {
                return null;
            }
            DataSet dstx = (DataSet)HTSelect["return_ds"];
            if (dstx != null && dstx.Tables[0].Rows.Count > 0)
            {
                Hashtable ht = new Hashtable();
                ht["@TXDXDLYX"] = dlyx;
                Hashtable returnHT = I_DBL.RunParam_SQL("update AAA_PTTXXXB set SFXSG = '是',XSSJ=getdate() WHERE TXDXDLYX=@TXDXDLYX", ht);
                if (!(bool)returnHT["return_float"])
                {
                    return null;
                }
                if (returnHT["return_other"] != null && Convert.ToInt32(returnHT["return_other"].ToString()) > 0)
                {
                    dstx.Tables[0].TableName = "提醒数据";
                    dsreturn.Tables.Add(dstx.Tables[0].Copy());
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "找到了提醒";
                    return dsreturn;
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到提醒";
                    return dsreturn;
                }

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到提醒";
                return dsreturn;
            }

        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// 通过最新的配置动态参数信息
    /// </summary>
    /// <returns>带有信息的DataSet</returns>
    public static DataSet GetParameterInfo_XTKZ()
    {
        try
        {

            //FMipcClass.Log.WorkLog("C下达预订单", FMipcClass.Log.type.yewu, "预订单提交-平台动态参数-执行时间090:", null);
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            //待返回的设备
            Hashtable HTParameterInfo = new Hashtable();
            //单个数据
            HTParameterInfo["卖家投标保证金比率"] = null;
            HTParameterInfo["卖家投标保证金最小值"] = null;
            HTParameterInfo["卖家履约保证金比率"] = null;
            HTParameterInfo["卖家履约保证金冻结期限"] = null;
            HTParameterInfo["履约保证金不足比率"] = null;
            HTParameterInfo["买家订金比率"] = null;
            HTParameterInfo["保证函比率"] = null;
            HTParameterInfo["签收技术服务费比率"] = null;
            HTParameterInfo["保证函外技术服务费比率"] = null;
            HTParameterInfo["经纪人收益卖家比率"] = null;
            HTParameterInfo["经纪人收益买家比率"] = null;
            HTParameterInfo["休眠期限"] = null;
            HTParameterInfo["平台每日开始服务时间"] = null;
            HTParameterInfo["平台每日结束服务时间"] = null;
            HTParameterInfo["即时合同定标下提货单期限"] = null;
            HTParameterInfo["是否在服务时间内"] = null;
            HTParameterInfo["是否在服务时间内(工作时)"] = null;
            HTParameterInfo["是否在服务时间内(假期)"] = null;
            //获取单个数据，取出最后一条更新的数据            
            Hashtable ht = I_DBL.RunProc("select top 1 * from AAA_PTDTCSSDB order by CreateTime DESC", "");
            if (!(bool)ht["return_float"])
            {
                return null;
            }
            DataSet ds_p = (DataSet)ht["return_ds"];
            if (ds_p != null && ds_p.Tables[0].Rows.Count > 0)
            {
                HTParameterInfo["卖家投标保证金比率"] = (decimal)ds_p.Tables[0].Rows[0]["MJTBBZJBL"];
                HTParameterInfo["卖家投标保证金最小值"] = (decimal)ds_p.Tables[0].Rows[0]["MJTBBZJZXZ"];
                HTParameterInfo["卖家履约保证金比率"] = (decimal)ds_p.Tables[0].Rows[0]["MJLYBZJBL"];
                HTParameterInfo["卖家履约保证金冻结期限"] = (Int32)ds_p.Tables[0].Rows[0]["MJLYBZJDJQX"];
                HTParameterInfo["履约保证金不足比率"] = (decimal)ds_p.Tables[0].Rows[0]["LYBZJBZBL"];
                HTParameterInfo["买家订金比率"] = (decimal)ds_p.Tables[0].Rows[0]["MJDJBL"];
                HTParameterInfo["保证函比率"] = (decimal)ds_p.Tables[0].Rows[0]["BZHBL"];
                HTParameterInfo["签收技术服务费比率"] = (decimal)ds_p.Tables[0].Rows[0]["QSJSFWFBL"];
                HTParameterInfo["保证函外技术服务费比率"] = (decimal)ds_p.Tables[0].Rows[0]["BZHWJSFWFBL"];
                HTParameterInfo["经纪人收益卖家比率"] = (decimal)ds_p.Tables[0].Rows[0]["JJRSYMJBLsel"];
                HTParameterInfo["经纪人收益买家比率"] = (decimal)ds_p.Tables[0].Rows[0]["JJRSYMJBLbuy"];
                HTParameterInfo["休眠期限"] = (Int32)ds_p.Tables[0].Rows[0]["XMQX"];
                HTParameterInfo["平台每日开始服务时间"] = ds_p.Tables[0].Rows[0]["PTMRKSFWSJ"].ToString();
                HTParameterInfo["平台每日结束服务时间"] = ds_p.Tables[0].Rows[0]["PTMRJSFWSJ"].ToString();
                HTParameterInfo["即时合同定标下提货单期限"] = ds_p.Tables[0].Rows[0]["JSHTDBXTHDQX"].ToString();

                string Strtime1begin = HTParameterInfo["平台每日开始服务时间"].ToString();
                string Strtime1end = HTParameterInfo["平台每日结束服务时间"].ToString();
                DateTime time1begin = DateTime.Parse(Strtime1begin);
                DateTime time1end = DateTime.Parse(Strtime1end);
                if ((DateTime.Now > time1begin) && (DateTime.Now < time1end))
                {
                    HTParameterInfo["是否在服务时间内(工作时)"] = "是";
                }
                else
                {
                    HTParameterInfo["是否在服务时间内(工作时)"] = "否";
                }               

                Hashtable ht1 = I_DBL.RunProc("select top 1 Number from AAA_PTJRSDB where SFYX='是' and DATEDIFF(day,RQ,GETDATE())=0   order by CreateTime DESC", "");                

                if (!(bool)ht1["return_float"])
                {
                    return null;
                }
                DataSet ds_pjq = (DataSet)ht1["return_ds"];
                if (ds_pjq != null && ds_pjq.Tables[0].Rows.Count > 0)
                {
                    HTParameterInfo["是否在服务时间内(假期)"] = "否";
                }
                else
                {
                    HTParameterInfo["是否在服务时间内(假期)"] = "是";
                }

                if (HTParameterInfo["是否在服务时间内(工作时)"].ToString() == "是" && HTParameterInfo["是否在服务时间内(假期)"].ToString() == "是")
                {
                    HTParameterInfo["是否在服务时间内"] = "是";
                }
                else
                {
                    HTParameterInfo["是否在服务时间内"] = "否";
                }

            }
            DataTable dt = GetDataTableFormHashtable(HTParameterInfo);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }
        catch (Exception)
        {
            return null;
        }
    }
    /// <summary>
    /// 是否在服务时间【同时在服务时间内(工作时)及在服务时间内(假期)】
    /// </summary>
    /// <returns>“是”、“否”、null</returns>
    public static string IsInServeTime_XTKZ()
    {
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

            //待返回的设备
            Hashtable HTParameterInfo = new Hashtable();
            //单个数据                     
            HTParameterInfo["是否在服务时间内"] = null;
            HTParameterInfo["是否在服务时间内(工作时)"] = null;
            HTParameterInfo["是否在服务时间内(假期)"] = null;
            //获取单个数据，取出最后一条更新的数据

            Hashtable ht = I_DBL.RunProc("select top 1 * from AAA_PTDTCSSDB order by CreateTime DESC", "");
            if (!(bool)ht["return_float"])
            {
                return null;
            }
            DataSet ds_p = (DataSet)ht["return_ds"];
            #region//获取服务时间
            if (ds_p != null && ds_p.Tables[0].Rows.Count > 0)
            {
                string Strtime1begin = ds_p.Tables[0].Rows[0]["PTMRKSFWSJ"].ToString();//平台每日开始服务时间
                string Strtime1end = ds_p.Tables[0].Rows[0]["PTMRJSFWSJ"].ToString();//平台每日结束服务时间
                DateTime time1begin = DateTime.Parse(Strtime1begin);
                DateTime time1end = DateTime.Parse(Strtime1end);
                if ((DateTime.Now > time1begin) && (DateTime.Now < time1end))
                {
                    HTParameterInfo["是否在服务时间内(工作时)"] = "是";
                }
                else
                {
                    HTParameterInfo["是否在服务时间内(工作时)"] = "否";
                }
                Hashtable ht1 = I_DBL.RunProc("select top 1 * from AAA_PTJRSDB where SFYX='是' and DATEDIFF(day,RQ,GETDATE())=0   order by CreateTime DESC", "");
                if (!(bool)ht1["return_float"])
                {
                    return null;
                }
                DataSet ds_pjq = (DataSet)ht1["return_ds"];
                if (ds_pjq != null && ds_pjq.Tables[0].Rows.Count > 0)
                {
                    HTParameterInfo["是否在服务时间内(假期)"] = "否";
                }
                else
                {
                    HTParameterInfo["是否在服务时间内(假期)"] = "是";
                }

            }
            #endregion

            if (HTParameterInfo["是否在服务时间内(工作时)"] != null && HTParameterInfo["是否在服务时间内(假期)"] != null && HTParameterInfo["是否在服务时间内(工作时)"].ToString() == "是" && HTParameterInfo["是否在服务时间内(假期)"].ToString() == "是")
            {
                return "是";
            }
            else
            {
                return "否";
            }

        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// 通过角色编号或证券资金账号获取账户各种频繁使用的相关信息
    /// </summary>
    /// <param name="bh">买家or卖家or经纪人角色编号or证券资金账号</param>
    /// <returns>带有信息的DataSet(包括了主要数据和完整数据集的两个DataTable)</returns>
    public static DataSet GetUserInfo(string bh)
    {
        //待返回的数据
        DataSet ds = new DataSet();
        DataTable UserInfoData = UserInfoDataTable();
        DataRow HTUserInfo = UserInfoData.NewRow();
        //Hashtable HTUserInfo = new Hashtable();
        HTUserInfo["登录邮箱"] = "";
        HTUserInfo["结算账户类型"] = "";
        HTUserInfo["买家角色编号"] = "";
        HTUserInfo["卖家角色编号"] = "";
        HTUserInfo["经纪人角色编号"] = "";
        HTUserInfo["是否冻结"] = "";
        HTUserInfo["冻结功能项"] = "";
        HTUserInfo["是否休眠"] = "";
        //***********************
        HTUserInfo["当前默认经纪人编号"] = "";
        HTUserInfo["是否被默认经纪人暂停新业务"] = "否";
        HTUserInfo["当前默认经纪人是否被冻结"] = "否";
        //***********************
        HTUserInfo["交易账户是否开通"] = "否";
        //***********************
        HTUserInfo["完整数据集"] = null;
        //***********************
        HTUserInfo["第三方"] = "";//I_DSFCGZT
        HTUserInfo["经纪人分类"] = "";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable htCS = new Hashtable();
        htCS["@BH"] = bh;
        htCS["@SFDQMRJJR"] = "是";
        //获取单个数据
        StringBuilder strBuilder = new StringBuilder(" select top 1 *,'当前默认经纪人是否被冻结'=isnull((select top 1 SubL.B_SFDJ from AAA_DLZHXXB as SubL where SubL.B_DLYX=G.GLJJRDLZH ),'否') from AAA_DLZHXXB as L left join AAA_MJMJJYZHYJJRZHGLB as G on L.B_DLYX = G.DLYX  where ");
        if (bh.StartsWith("B"))//卖家
        {
            strBuilder.Append(" L.J_SELJSBH=@BH");
        }
        else if (bh.StartsWith("C"))//买家
        {
            strBuilder.Append(" L.J_BUYJSBH=@BH");
        }
        else if (bh.StartsWith("J"))//经纪人
        {
            strBuilder.Append(" L.J_JJRJSBH = @BH");
        }
        else
        {
            strBuilder.Append(" L.I_ZQZJZH=@BH");
        }
        strBuilder.Append(" and ''<>@BH and G.SFDQMRJJR = @SFDQMRJJR");
        Hashtable returnHT = I_DBL.RunParam_SQL(strBuilder.ToString(), "", htCS);

        //Hashtable returnHT = I_DBL.RunParam_SQL("select top 1 *,'当前默认经纪人是否被冻结'=isnull((select top 1 SubL.B_SFDJ from AAA_DLZHXXB as SubL where SubL.B_DLYX=G.GLJJRDLZH ),'否') from AAA_DLZHXXB as L left join AAA_MJMJJYZHYJJRZHGLB as G on L.B_DLYX = G.DLYX where (L.J_BUYJSBH=@BH or L.J_SELJSBH=@BH or L.J_JJRJSBH = @BH or L.I_ZQZJZH=@BH) and ''<>@BH and G.SFDQMRJJR = '是'", "", htCS);

        if (!(bool)returnHT["return_float"])
        {
            return null;
        }
        DataSet ds_p1 = (DataSet)returnHT["return_ds"];
        if (ds_p1 != null && ds_p1.Tables.Count > 0 && ds_p1.Tables[0].Rows.Count > 0)
        {
            HTUserInfo["登录邮箱"] = ds_p1.Tables[0].Rows[0]["B_DLYX"].ToString();
            HTUserInfo["结算账户类型"] = ds_p1.Tables[0].Rows[0]["B_JSZHLX"].ToString();
            HTUserInfo["买家角色编号"] = ds_p1.Tables[0].Rows[0]["J_BUYJSBH"].ToString();
            HTUserInfo["卖家角色编号"] = ds_p1.Tables[0].Rows[0]["J_SELJSBH"].ToString();
            HTUserInfo["经纪人角色编号"] = ds_p1.Tables[0].Rows[0]["J_JJRJSBH"].ToString();
            HTUserInfo["是否冻结"] = ds_p1.Tables[0].Rows[0]["B_SFDJ"].ToString();
            HTUserInfo["冻结功能项"] = ds_p1.Tables[0].Rows[0]["DJGNX"].ToString();
            HTUserInfo["是否休眠"] = ds_p1.Tables[0].Rows[0]["B_SFXM"].ToString();
            HTUserInfo["经纪人分类"] = ds_p1.Tables[0].Rows[0]["I_JJRFL"].ToString();
            //***********************
            HTUserInfo["当前默认经纪人编号"] = ds_p1.Tables[0].Rows[0]["GLJJRBH"].ToString();
            HTUserInfo["第三方"] = ds_p1.Tables[0].Rows[0]["I_DSFCGZT"].ToString();//I_DSFCGZT
            if (ds_p1.Tables[0].Rows[0]["SFZTYHXYW"].ToString() == "是")
            {
                HTUserInfo["是否被默认经纪人暂停新业务"] = "是";
            }
            else
            {
                HTUserInfo["是否被默认经纪人暂停新业务"] = "否";
            }

            if (ds_p1.Tables[0].Rows[0]["当前默认经纪人是否被冻结"].ToString() == "是")
            {
                HTUserInfo["当前默认经纪人是否被冻结"] = "是";
            }
            else
            {
                HTUserInfo["当前默认经纪人是否被冻结"] = "否";
            }

            //***********************
            if (ds_p1.Tables[0].Rows[0]["B_JSZHLX"].ToString() == "经纪人交易账户")
            {
                if (ds_p1.Tables[0].Rows[0]["S_SFYBFGSSHTG"].ToString() == "是")
                {
                    HTUserInfo["交易账户是否开通"] = "是";
                }
                else
                {
                    HTUserInfo["交易账户是否开通"] = "否";
                }
            }
            if (ds_p1.Tables[0].Rows[0]["B_JSZHLX"].ToString() == "买家卖家交易账户")
            {
                if (ds_p1.Tables[0].Rows[0]["S_SFYBFGSSHTG"].ToString() == "是" && ds_p1.Tables[0].Rows[0]["S_SFYBJJRSHTG"].ToString() == "是")
                {
                    HTUserInfo["交易账户是否开通"] = "是";
                }
                else
                {
                    HTUserInfo["交易账户是否开通"] = "否";
                }
            }
            //***********************
            UserInfoData.Rows.Add(HTUserInfo);

            ds.Tables.Add(UserInfoData);
            DataTable table = ds_p1.Tables[0].Copy();
            table.TableName = "完整数据集";
            ds.Tables.Add(table);
        }

        return ds;
    }

    public static DataTable UserInfoDataTable()
    {
        DataTable UserInfoData = new DataTable();
        UserInfoData.TableName = "主要数据";
        UserInfoData.Columns.Add("登录邮箱");
        UserInfoData.Columns.Add("结算账户类型");
        UserInfoData.Columns.Add("买家角色编号");
        UserInfoData.Columns.Add("卖家角色编号");
        UserInfoData.Columns.Add("经纪人角色编号");
        UserInfoData.Columns.Add("是否冻结");
        UserInfoData.Columns.Add("冻结功能项");
        UserInfoData.Columns.Add("是否休眠");
        UserInfoData.Columns.Add("当前默认经纪人编号");
        UserInfoData.Columns.Add("是否被默认经纪人暂停新业务");
        UserInfoData.Columns.Add("当前默认经纪人是否被冻结");
        UserInfoData.Columns.Add("交易账户是否开通");
        UserInfoData.Columns.Add("完整数据集");
        UserInfoData.Columns.Add("第三方");
        UserInfoData.Columns.Add("经纪人分类");
        return UserInfoData;
    }
}