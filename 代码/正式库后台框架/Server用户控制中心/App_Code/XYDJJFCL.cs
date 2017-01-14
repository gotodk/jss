using FMDBHelperClass;
using FMipcClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// XYDJJFCL 的摘要说明
/// </summary>
public class XYDJJFCL
{
    public XYDJJFCL()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 根据业务点分别调用不同方法处理信用积分
    /// </summary>
    /// <param name="dtInput">参数集，至少包含“合同编号”字段</param>
    /// <param name="ywName">业务类型名</param>
    /// <returns></returns>
    public bool DealXYJF(DataTable dtInput,string ywName)
    {
        bool Res = false;
        switch (ywName)
        {
            case "无异议收货":
                Res = RunHTQPJS(dtInput);
                break;
            case "卖家主动退货":
                 Res = RunHTQPJS(dtInput);
                break;
            case "提醒后自动签收":
                Res = RunHTQPJS(dtInput);
                break;
            case "合同期满监控":
                Res = RunHTQPJS(dtInput);
                break;
            case "超期未缴纳履约保证金监控":
                Res = RunCQWJNLYBZJ(dtInput);
                break;
            case "大盘经验交流":
                Res = RunDaPanJYJL(dtInput);
                break;
            case "经纪人驳回买卖家":
                Res = RunJJRBoHui(dtInput);
                break;
            default:
                break;
        }
        return Res;    
    }

    //
    /// <summary>
    /// 执行合同状态变更为“清盘结束”后的相关信用积分处理
    /// </summary>
    /// <param name="dtInput"></param>
    /// <returns></returns>
    private bool RunHTQPJS(DataTable dtInput)
    {
        ArrayList alist = new ArrayList();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        if (dtInput == null || dtInput.Rows.Count <= 0)
        {//没有需要处理的数据，直接返回成功
            return true;
        }

        //循环数据表处理符合条件的数据
        foreach (DataRow drinfo in dtInput.Rows)
        {
            string HTBH = drinfo["合同编号"].ToString().Trim();//电子购货合同编号
            //根据合同编号获取合同基本信息
            string sql_HTinfo = "select Z_QPZT as 清盘状态,T_YSTBDDLYX as 卖家邮箱,T_YSTBDMJJSBH as 卖家角色编号,T_YSTBDJSZHLX as 卖家结算账户类型,T_YSTBDGLJJRYX as 卖家经纪人邮箱,T_YSTBDGLJJRJSBH as 卖家经纪人角色编号,'经纪人交易账户' as 卖家经纪人结算账户类型,Y_YSYDDDLYX as 买家邮箱,Y_YSYDDMJJSBH as 买家角色编号,Y_YSYDDJSZHLX as 买家结算账户类型,Y_YSYDDGLJJRYX as 买家经纪人邮箱,Y_YSYDDGLJJRJSBH as 买家经纪人角色编号,'经纪人交易账户' as 买家经纪人结算账户类型 from AAA_ZBDBXXB where Z_HTBH='" + HTBH + "'";
            DataSet ds_HTinfo = new DataSet();
            Hashtable ht_HTinfo = I_DBL.RunProc(sql_HTinfo, "data");
            if ((bool)ht_HTinfo["return_float"])
            {
                ds_HTinfo = (DataSet)ht_HTinfo["return_ds"];
            }
            else
            {
                ds_HTinfo = null;
            }
            if (ds_HTinfo == null || ds_HTinfo.Tables[0].Rows.Count <= 0)
            {//找不到对应合同数据，直接返回失败
                return false;
            }

            string bszt = ds_HTinfo.Tables[0].Rows[0]["清盘状态"].ToString();

            if (bszt.ToString().Trim() == "清盘结束")
            {//只处理清盘状态变为“清盘结束”的数据

                #region 根据买家履约情况确定买家及其买家经纪人的信用增减
                //买家参数
                Hashtable ht_params_buy = new Hashtable();
                ht_params_buy["登录邮箱"] = ds_HTinfo.Tables[0].Rows[0]["买家邮箱"].ToString().Trim();
                ht_params_buy["角色编号"] = ds_HTinfo.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
                ht_params_buy["结算账户类型"] = ds_HTinfo.Tables[0].Rows[0]["买家结算账户类型"].ToString().Trim();
                ht_params_buy["备注"] = "";
                ht_params_buy["x1"] = HTBH;
                ht_params_buy["x2"] = "";

                //买家经纪人参数
                Hashtable ht_params_buyjjr = new Hashtable();
                ht_params_buyjjr["登录邮箱"] = ds_HTinfo.Tables[0].Rows[0]["买家经纪人邮箱"].ToString().Trim();
                ht_params_buyjjr["角色编号"] = ds_HTinfo.Tables[0].Rows[0]["买家经纪人角色编号"].ToString().Trim();
                ht_params_buyjjr["结算账户类型"] = ds_HTinfo.Tables[0].Rows[0]["买家经纪人结算账户类型"].ToString().Trim();
                ht_params_buyjjr["备注"] = "";
                ht_params_buyjjr["x1"] = ds_HTinfo.Tables[0].Rows[0]["买家邮箱"].ToString().Trim();
                ht_params_buyjjr["x2"] =HTBH;

                //判断买家是否在合同履行期间存在违规扣罚
                string Buy_ExistKouFa = ExistKouFa(HTBH, ds_HTinfo.Tables[0].Rows[0]["买家角色编号"].ToString().Trim());

                if (Buy_ExistKouFa.IndexOf("no") >= 0)
                {//不存在买家履约扣罚数据，即完全履约

                    //买家信用积分增加
                    ht_params_buy["对照表编号"] = "1309000006";//自身账户（买入） 合同[x1]完全履约	+10	增项	清盘结束
                    ArrayList al_buy = GetSqlText(ht_params_buy);
                    alist.AddRange(al_buy);

                    //买家经纪人信用积分增加
                    ht_params_buyjjr["对照表编号"] = "1309000002";//关联交易方 关联交易方[x1]，合同[x2]完全履约 +1 增项 清盘结束
                    ArrayList al_buyjjr = GetSqlText(ht_params_buyjjr);
                    alist.AddRange(al_buyjjr);

                }
                else if (Buy_ExistKouFa.IndexOf("yes") >= 0)
                {//存在买家履约扣罚数据，即未完全履约                  

                    //买家信用积分扣减
                    ht_params_buy["对照表编号"] = "1309000007";//自身账户（买入） 合同[x1]未完全履约	-1	减项	清盘结束
                    ArrayList al_buy = GetSqlText(ht_params_buy);
                    alist.AddRange(al_buy);

                    //买家经纪人信用积分扣减
                    ht_params_buyjjr["对照表编号"] = "1309000003";//关联交易方 关联交易方[x1]，合同[x2]未完全履约 -1 减项 清盘结束
                    ArrayList al_buyjjr = GetSqlText(ht_params_buyjjr);
                    alist.AddRange(al_buyjjr);
                }
                else
                {//获取买家违约扣罚数据失败，直接返回失败
                    return false;
                }
                #endregion

                #region 根据卖家的履约情况确定卖家及卖家经纪人的信用增减
                //卖家参数
                Hashtable ht_params_sale = new Hashtable();
                ht_params_sale["登录邮箱"] = ds_HTinfo.Tables[0].Rows[0]["卖家邮箱"].ToString().Trim();
                ht_params_sale["角色编号"] = ds_HTinfo.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim();
                ht_params_sale["结算账户类型"] = ds_HTinfo.Tables[0].Rows[0]["卖家结算账户类型"].ToString().Trim();
                ht_params_sale["备注"] = "";
                ht_params_sale["x1"] = HTBH;
                ht_params_sale["x2"] = "";

                //卖家家经纪人参数
                Hashtable ht_params_salejjr = new Hashtable();
                ht_params_salejjr["登录邮箱"] = ds_HTinfo.Tables[0].Rows[0]["卖家经纪人邮箱"].ToString().Trim();
                ht_params_salejjr["角色编号"] = ds_HTinfo.Tables[0].Rows[0]["卖家经纪人角色编号"].ToString().Trim();
                ht_params_salejjr["结算账户类型"] = ds_HTinfo.Tables[0].Rows[0]["卖家经纪人结算账户类型"].ToString().Trim();
                ht_params_salejjr["备注"] = "";
                ht_params_salejjr["x1"] = ds_HTinfo.Tables[0].Rows[0]["卖家邮箱"].ToString().Trim();
                ht_params_salejjr["x2"] =HTBH ;

                //判断卖家是否在合同履行期间存在违规扣罚
                string Sale_ExistKouFa = ExistKouFa(HTBH, ds_HTinfo.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim());

                if (Sale_ExistKouFa.IndexOf("no") >= 0)
                {//不存在卖家履约扣罚数据，即完全履约

                    //卖家信用积分增加
                    ht_params_sale["对照表编号"] = "1309000009";//自身账户（卖出） 合同[x1]完全履约 +10 增项 清盘结束
                    ArrayList al_sale = GetSqlText(ht_params_sale);
                    alist.AddRange(al_sale);

                    //卖家经纪人信用积分增加
                    ht_params_salejjr["对照表编号"] = "1309000002";//关联交易方 关联交易方[x1]，合同[x2]完全履约 +1 增项 清盘结束
                    ArrayList al_salejjr = GetSqlText(ht_params_salejjr);
                    alist.AddRange(al_salejjr);

                }
                else if (Sale_ExistKouFa.IndexOf("yes") >= 0)
                {//存在卖家履约扣罚数据，即未完全履约                  

                    //卖家信用积分扣减
                    ht_params_sale["对照表编号"] = "1309000010";//自身账户（卖出） 合同[x1]未完全履约 -1 减项 清盘结束
                    ArrayList al_sale = GetSqlText(ht_params_sale);
                    alist.AddRange(al_sale);

                    //卖家经纪人信用积分扣减
                    ht_params_salejjr["对照表编号"] = "1309000003";////关联交易方 关联交易方[x1]，合同[x2]未完全履约 -1 减项 清盘结束
                    ArrayList al_salejjr = GetSqlText(ht_params_salejjr);
                    alist.AddRange(al_salejjr);
                }
                else
                {//获取卖家违约扣罚数据失败，直接返回失败
                    return false;
                }
                #endregion
            }
        }
        //执行生成的sql语句，增减积分
        if (alist.Count > 0)
        {
            Hashtable ht_res = I_DBL.RunParam_SQL(alist);
            if ((bool)ht_res["return_float"])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {//没有语句需要执行 
            return true;
        }
    }

    /// <summary>
    /// 执行超期未缴纳履约保证金监控中的积分处理
    /// </summary>
    /// <param name="dtInput">合同编号数据表</param>
    /// <returns>执行成功与否 true or false</returns>
    private bool RunCQWJNLYBZJ(DataTable dtInput)
    {//超期未缴纳履约保证金导致的废标，仅扣除卖家积分
        
        ArrayList alist = new ArrayList();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        if (dtInput == null || dtInput.Rows.Count <= 0)
        {//没有需要处理的数据，直接返回成功
            return true;
        }
        foreach (DataRow drinfo in dtInput.Rows)
        {
            string HTBH = drinfo["合同编号"].ToString().Trim();//电子购货合同编号
            
            //根据合同编号获取合同基本信息
            string sql_HTinfo = "select T_YSTBDDLYX as 卖家邮箱,T_YSTBDMJJSBH as 卖家角色编号,T_YSTBDJSZHLX as 卖家结算账户类型 from AAA_ZBDBXXB where Z_HTBH='" + HTBH + "'";
            DataSet ds_HTinfo = new DataSet();
            Hashtable ht_HTinfo = I_DBL.RunProc(sql_HTinfo, "data");
            if ((bool)ht_HTinfo["return_float"])
            {
                ds_HTinfo = (DataSet)ht_HTinfo["return_ds"];
            }
            else
            {
                ds_HTinfo = null;
            }
            if (ds_HTinfo == null || ds_HTinfo.Tables[0].Rows.Count <= 0)
            {//找不到对应合同数据，直接返回失败
                return false;
            }

            Hashtable ht_params = new Hashtable();
            ht_params["对照表编号"] = "1309000011";//自身账户（卖出） 合同[x1]未定标 -1 减项 定标监控
            ht_params["登录邮箱"] = ds_HTinfo.Tables[0].Rows[0]["卖家邮箱"].ToString().Trim();
            ht_params["角色编号"] = ds_HTinfo.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim();
            ht_params["结算账户类型"] = ds_HTinfo.Tables[0].Rows[0]["卖家结算账户类型"].ToString().Trim();
            ht_params["备注"] = "";
            ht_params["x1"] = HTBH;
            ht_params["x2"] = "";

            ArrayList al = GetSqlText(ht_params);
            alist.AddRange(al);
        }

        //执行生成的sql语句，增减积分
        if (alist.Count > 0)
        {
            Hashtable ht_res = I_DBL.RunParam_SQL(alist);
            if ((bool)ht_res["return_float"])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {//没有语句需要执行
            return true;        
        }
    }

    /// <summary>
    /// 经纪人驳回开户申请
    /// </summary>
    /// <param name="dtInput">需加分的的登录邮箱列表</param>
    /// <returns></returns>
    private bool RunJJRBoHui(DataTable dtInput)
    {//交易方首次关联经纪人被驳回的，作为卖家身份扣信用等级积分

        ArrayList alist = new ArrayList();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        if (dtInput == null || dtInput.Rows.Count <= 0)
        {//没有需要处理的数据，直接返回成功
            return true;
        }
        foreach (DataRow drinfo in dtInput.Rows)
        {
            string DLYX = drinfo["登录邮箱"].ToString().Trim();//登录邮箱
            //根据登录邮箱获取基本信息
            string sql_info = "select B_JSZHLX as 结算账户类型,J_SELJSBH as 卖家角色编号 from AAA_DLZHXXB where B_DLYX='" + DLYX + "'";
            DataSet ds_info = new DataSet();
            Hashtable ht_info = I_DBL.RunProc(sql_info, "data");
            if ((bool)ht_info["return_float"])
            {
                ds_info = (DataSet)ht_info["return_ds"];
            }
            else
            {
                ds_info = null;
            }
            if (ds_info == null || ds_info.Tables[0].Rows.Count <= 0)
            {//找不到对应数据，直接返回失败
                return false;
            }           
                Hashtable ht_params = new Hashtable();
                ht_params["对照表编号"] = "1309000015";//自身开户申请 [x1]被经纪人审核时驳回 -0.5 减项 经纪人审核时
                ht_params["登录邮箱"] = DLYX;
                ht_params["角色编号"] = ds_info.Tables[0].Rows[0]["卖家角色编号"].ToString().Trim();
                ht_params["结算账户类型"] = ds_info.Tables[0].Rows[0]["结算账户类型"].ToString().Trim();
                ht_params["备注"] = "经纪人账号：" + drinfo["经纪人邮箱"].ToString();
                ht_params["x1"] = DLYX;
                ht_params["x2"] = "";               

                ArrayList al = GetSqlText(ht_params);
                alist.AddRange(al);
            
        }
        //执行生成的sql语句，增减积分
        if (alist.Count > 0)
        {
            Hashtable ht_res = I_DBL.RunParam_SQL(alist);
            if ((bool)ht_res["return_float"])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {//没有语句需要执行
            return true;
        }
    }

    /// <summary>
    /// 大盘经验交流信用积分增加
    /// </summary>
    /// <param name="dtInput">参数列表，包括：登录邮箱、备注</param>
    /// <returns></returns>
    private bool RunDaPanJYJL(DataTable dtInput)
    {//被第五个人支持后，发起人是“交易方交易账户”的给买家身份加分

         ArrayList alist = new ArrayList();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        if (dtInput == null || dtInput.Rows.Count <= 0)
        {//没有需要处理的数据，直接返回成功
            return true;
        }
        foreach (DataRow drinfo in dtInput.Rows)
        {
            string DLYX = drinfo["登录邮箱"].ToString().Trim();//登录邮箱
            //根据登录邮箱获取基本信息
            string sql_HTinfo = "select B_JSZHLX as 结算账户类型,J_BUYJSBH as 买家角色编号 from AAA_DLZHXXB where B_DLYX='" + DLYX + "'";
            DataSet ds_HTinfo = new DataSet();
            Hashtable ht_HTinfo = I_DBL.RunProc(sql_HTinfo, "data");
            if ((bool)ht_HTinfo["return_float"])
            {
                ds_HTinfo = (DataSet)ht_HTinfo["return_ds"];
            }
            else
            {
                ds_HTinfo = null;
            }
            if (ds_HTinfo == null || ds_HTinfo.Tables[0].Rows.Count <= 0)
            {//找不到对应数据，直接返回失败
                return false;
            }
            if (ds_HTinfo.Tables[0].Rows[0]["结算账户类型"].ToString() == "买家卖家交易账户")
            {
                Hashtable ht_params = new Hashtable();
                ht_params["对照表编号"] = "1309000018";//经验交流发布建议 [x1]经验交流区发布建议累计支持人数大于等于5个 +5 增项 经验交流支持 
                ht_params["登录邮箱"] = DLYX;
                ht_params["角色编号"] = ds_HTinfo.Tables[0].Rows[0]["买家角色编号"].ToString().Trim();
                ht_params["结算账户类型"] = ds_HTinfo.Tables[0].Rows[0]["结算账户类型"].ToString().Trim();
                ht_params["备注"] = drinfo["备注"].ToString();
                ht_params["x1"] = DLYX;
                ht_params["x2"] = "";

                ArrayList al = GetSqlText(ht_params);
                alist.AddRange(al);
            }
        }
        //执行生成的sql语句，增减积分
        if (alist.Count > 0)
        {
            Hashtable ht_res = I_DBL.RunParam_SQL(alist);
            if ((bool)ht_res["return_float"])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {//没有语句需要执行
            return true;
        }
    }

    /// <summary>
    /// 判断合同履行期间是否存在违规扣罚 注：中标定标信息表中的“清盘状态”为清盘结束。
    /// </summary>
    /// <param name="htbh">合同编号</param>
    /// <param name="dlyx">角色编号</param>    
    /// <returns>字符串：yes,存在扣罚数据;no,不存在扣罚数据;err,获取扣罚数据错误</returns>
    public string ExistKouFa(string htbh, string jsbh)
    {
        string res = "err,初始化";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");       

        string str = " select top 1 * from  AAA_View_JYFXYMXDic where 合同编号='" + htbh + "' and 角色编号='" + jsbh + "' and 清盘状态='清盘结束'";
        Hashtable ht_res = I_DBL.RunProc(str, "data");
        DataSet ds_res = new DataSet();
        if ((bool)ht_res["return_float"])
        {
            ds_res = (DataSet)ht_res["return_ds"];
            if (ds_res != null && ds_res.Tables[0].Rows.Count > 0)
            {
                res = "yes,存在扣罚数据";
            }
            else
            {
                res = "no,不存在扣罚数据";
            }
        }
        else
        {
            res = "err,获取扣罚数据失败";
        }
        return res;
    }

    /// <summary>
    /// 获取写入交易方信用明细表的SQL语句及获取更新信用余额的sql语句
    /// </summary>
    /// <param name="htInput">参数哈希表，包括：对照表编号、登录邮箱、角色编号、结算账户类型、备注、x1，x2</param>    
    /// <returns></returns>
    protected ArrayList GetSqlText(Hashtable htInput)
    {
        ArrayList alist = new ArrayList();//生成的sql语句
        //获取交易方信用明细表主键
        object[] re = IPC.Call("获取主键", new string[] { "AAA_JYFXYMXB", "" });
        string key = "";
        if (re[0].ToString() == "ok")
        {
            //这个就是得到远程方法真正的返回值，不同类型的，自行进行强制转换即可。
            key = (string)(re[1]);
        }
        else
        {
            //远程方法执行程序出错了。这里得到的是具体的程序错误信息。一般不用反馈给用户。这时，类型肯定是string。
            string reFF = re[1].ToString();
            return null;
        }

        //根据对照表编号获取对照表数据信息
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable returnHT1 = I_DBL.RunProc("select * from AAA_JYFXYBDMXDZB where Number='" + htInput["对照表编号"].ToString ()  + "'", "");
        if (!(bool)returnHT1["return_float"])
        {
            return null;
        }

        DataTable dt_DZB = ((DataSet)returnHT1["return_ds"]).Tables[0];
        string jfxm = dt_DZB.Rows[0]["JFXM"].ToString().Trim();//积分项目        
        string fs = dt_DZB.Rows[0]["JFGZ"].ToString().Trim();//积分规则，即分值
        string yslx = dt_DZB.Rows[0]["YSLX"].ToString().Trim();//运算类型
        string jfsx = dt_DZB.Rows[0]["JFSX"].ToString().Trim().Replace("[x1]", htInput["x1"].ToString()).Replace("[x2]", htInput["x2"].ToString());//积分事项，替换其中的内容。
        string qtbz = dt_DZB.Rows[0]["TYDBZ"].ToString().Trim();//调用点备注
        if (htInput["备注"].ToString() != "")
        {//如果存在备注信息，则增加到调用点备注中
            qtbz +="。"+ htInput["备注"].ToString();
        }

        //插入【交易方信用明细表】
        string sql_insert = "insert into AAA_JYFXYMXB ([Number],[DLYX],[JYZHLX],[JFXM],[JSBH],[JFSX],[YSLX],[FS],[FSSJ],[QTBZ],[CheckState],[CreateUser],[CreateTime])   values('" + key + "','" + htInput["登录邮箱"].ToString() + "','" + htInput["结算账户类型"].ToString() + "','" + jfxm + "','" + htInput["角色编号"].ToString() + "','" + jfsx + "' ,'" + yslx + "'," + fs + ",GETDATE(),'" + qtbz + "',1,'admin',GETDATE())";
        alist.Add(sql_insert);
        //更新登陆账号信息表中的“账户当前信用分值”字段
        string sql_update = " update AAA_DLZHXXB set B_ZHDQXYFZ = ISNULL(B_ZHDQXYFZ,0) " + fs + " where B_DLYX='" + htInput["登录邮箱"].ToString() + "'";
        alist.Add(sql_update);
        return alist;
    }

}