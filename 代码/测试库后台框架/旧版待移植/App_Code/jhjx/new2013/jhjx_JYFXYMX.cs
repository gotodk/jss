using System;
using System.Collections;
using System.Collections.Generic;
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
using RedisReLoad.ShareDBcache;

/// <summary>
/// jhjx_JYFXYMX 的摘要说明
/// </summary>
public class jhjx_JYFXYMX
{
	public jhjx_JYFXYMX()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 获取写入交易方信用明细表的SQL语句，获取更新信用余额的语句
    /// </summary>
    /// <param name="num">对照表主键编号</param>
    /// <param name="dlyx">登陆邮箱</param>    
    /// <param name="zhxz">账户性质：卖家、卖家、经纪人</param>
    /// <param name="replacex1">替代字符串</param>
    /// <param name="replacex2">替代字符串</param>
    /// <param name="creater">创建者</param>
    /// <returns></returns>
    protected string[] SqlText(string num, string dlyx, string replacex1, string replacex2, string zhxz, string creater)
    {
        WorkFlowModule WFM = new WorkFlowModule("AAA_JYFXYMXB");
        string key = WFM.numberFormat.GetNextNumberZZ("");

        //先从redis中读取
        DataTable dtemp1 = XYDZBRedis.Redis_GetXYBDMXDZB(num);

        //如果返回的数据表为null，则从数据库中读取，并更新redis 
        if (dtemp1 == null)
        {
            dtemp1 = DbHelperSQL.Query("select * from AAA_JYFXYBDMXDZB where Number='" + num + "'").Tables[0];

            if (dtemp1 != null && dtemp1.Rows.Count > 0)
            { XYDZBRedis.Redis_SetXYBDMXDZB(num, dtemp1); }
        }

       
        string jfxm = dtemp1.Rows[0]["JFXM"].ToString().Trim();
        string qtbz = dtemp1.Rows[0]["TYDBZ"].ToString().Trim();
        string jfsx = dtemp1.Rows[0]["JFSX"].ToString().Trim().Replace("[x1]", replacex1).Replace("[x2]", replacex2);
        string fs = dtemp1.Rows[0]["JFGZ"].ToString().Trim();
        string yslx = dtemp1.Rows[0]["YSLX"].ToString().Trim();

        DataTable dtemp2 = DbHelperSQL.Query("select B_JSZHLX, J_SELJSBH,J_BUYJSBH,J_JJRJSBH from AAA_DLZHXXB  where B_DLYX='" + dlyx + "'").Tables[0];
        string jszhlx = dtemp2.Rows[0]["B_JSZHLX"].ToString().Trim();

        string jsbh = "";
        if (zhxz == "买家" && dtemp2.Rows[0]["J_BUYJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_BUYJSBH"].ToString().Trim();
        }
        else if (zhxz == "卖家" && dtemp2.Rows[0]["J_SELJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_SELJSBH"].ToString().Trim();
        }
        else if (zhxz == "经纪人" && dtemp2.Rows[0]["J_JJRJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_JJRJSBH"].ToString().Trim();
        }

        string[] strs = new string[2];
        strs[0] = "insert into AAA_JYFXYMXB ([Number],[DLYX],[JYZHLX],[JFXM],[JSBH],[JFSX],[YSLX],[FS],[FSSJ],[QTBZ],[CheckState],[CreateUser],[CreateTime])   values('"+key+"','"+dlyx+"','"+jszhlx+"','"+jfxm+"','"+jsbh+"','"+jfsx+"' ,'"+yslx+"',"+fs+",GETDATE(),'"+qtbz+"',1,'"+creater+"',GETDATE())";
        strs[1] = " update AAA_DLZHXXB set B_ZHDQXYFZ = ISNULL(B_ZHDQXYFZ,0) " + fs + " where B_DLYX='" + dlyx + "'";      

        dtemp1.Dispose();
        dtemp2.Dispose();

        return strs;

    }

    /// <summary>
    /// 重载，获取写入交易方信用明细表的SQL语句，获取更新信用余额的语句
    /// </summary>
    /// <param name="num">对照表主键编号</param>
    /// <param name="dlyx">登陆邮箱</param>    
    /// <param name="zhxz">账户性质：卖家、卖家、经纪人</param>
    /// <param name="replacex1">替代字符串</param>
    /// <param name="replacex2">替代字符串</param>
    /// <param name="creater">创建者</param>
    /// <param name="bz">备注</param>
    /// <returns></returns>
    protected string[] SqlText(string num, string dlyx, string replacex1, string replacex2, string zhxz, string creater, string bz)
    {
        WorkFlowModule WFM = new WorkFlowModule("AAA_JYFXYMXB");
        string key = WFM.numberFormat.GetNextNumberZZ("");

        //先从redis中读取
        DataTable dtemp1 = XYDZBRedis.Redis_GetXYBDMXDZB(num);

        //如果返回的数据表为null，则从数据库中读取，并更新redis 
        if (dtemp1 == null)
        {
            dtemp1 = DbHelperSQL.Query("select * from AAA_JYFXYBDMXDZB where Number='" + num + "'").Tables[0];

            if (dtemp1 != null && dtemp1.Rows.Count > 0)
            { XYDZBRedis.Redis_SetXYBDMXDZB(num, dtemp1); }
        }       
        string jfxm = dtemp1.Rows[0]["JFXM"].ToString().Trim();
        string qtbz = dtemp1.Rows[0]["TYDBZ"].ToString().Trim() + "。" + bz;
        string jfsx = dtemp1.Rows[0]["JFSX"].ToString().Trim().Replace("[x1]", replacex1).Replace("[x2]", replacex2);
        string fs = dtemp1.Rows[0]["JFGZ"].ToString().Trim();
        string yslx = dtemp1.Rows[0]["YSLX"].ToString().Trim();

        DataTable dtemp2 = DbHelperSQL.Query("select B_JSZHLX, J_SELJSBH,J_BUYJSBH,J_JJRJSBH from AAA_DLZHXXB  where B_DLYX='" + dlyx + "'").Tables[0];
        string jszhlx = dtemp2.Rows[0]["B_JSZHLX"].ToString().Trim();

        string jsbh = "";
        if (zhxz == "买家" && dtemp2.Rows[0]["J_BUYJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_BUYJSBH"].ToString().Trim();
        }
        else if (zhxz == "卖家" && dtemp2.Rows[0]["J_SELJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_SELJSBH"].ToString().Trim();
        }
        else if (zhxz == "经纪人" && dtemp2.Rows[0]["J_JJRJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_JJRJSBH"].ToString().Trim();
        }

        string[] strs = new string[2];
        strs[0] = "insert into AAA_JYFXYMXB ([Number],[DLYX],[JYZHLX],[JFXM],[JSBH],[JFSX],[YSLX],[FS],[FSSJ],[QTBZ],[CheckState],[CreateUser],[CreateTime])   values('" + key + "','" + dlyx + "','" + jszhlx + "','" + jfxm + "','" + jsbh + "','" + jfsx + "' ,'" + yslx + "'," + fs + ",GETDATE(),'" + qtbz + "',1,'" + creater + "',GETDATE())";
        strs[1] = " update AAA_DLZHXXB set B_ZHDQXYFZ = ISNULL(B_ZHDQXYFZ,0) " + fs + " where B_DLYX='" + dlyx + "'";

        dtemp1.Dispose();
        dtemp2.Dispose();

        return strs;

    }


    /// <summary>
    /// 重载，获取写入交易方信用明细表的SQL语句，获取更新信用余额的语句
    /// </summary>
    /// <param name="dr">对照表中的一个具体行</param>
    /// <param name="dlyx">登陆邮箱</param>   
    /// <param name="jsbh">角色编号</param>    
    /// <param name="jszhlx">买家卖家交易账户、经纪人交易账户</param>
    /// <param name="replacex1">替代字符串</param>
    /// <param name="replacex2">替代字符串</param>
    /// <param name="creater">创建者</param>
    /// <param name="bz">备注</param>
    /// <returns></returns>
    public  string[] SqlText(DataRow dr , string dlyx, string replacex1, string replacex2, string jsbh,string jszhlx , string creater, string bz)
    {
        WorkFlowModule WFM = new WorkFlowModule("AAA_JYFXYMXB");
        string key = WFM.numberFormat.GetNextNumberZZ("");
       
        string jfxm = dr["JFXM"].ToString().Trim();
        string qtbz = dr["TYDBZ"].ToString().Trim() + "。" + bz;
        string jfsx = dr["JFSX"].ToString().Trim().Replace("[x1]", replacex1).Replace("[x2]", replacex2);
        string fs = dr["JFGZ"].ToString().Trim();
        string yslx = dr["YSLX"].ToString().Trim();             


        string[] strs = new string[2];
        strs[0] = "insert into AAA_JYFXYMXB ([Number],[DLYX],[JYZHLX],[JFXM],[JSBH],[JFSX],[YSLX],[FS],[FSSJ],[QTBZ],[CheckState],[CreateUser],[CreateTime])   values('" + key + "','" + dlyx + "','" + jszhlx + "','" + jfxm + "','" + jsbh + "','" + jfsx + "' ,'" + yslx + "'," + fs + ",GETDATE(),'" + qtbz + "',1,'" + creater + "',GETDATE())";
        strs[1] = " update AAA_DLZHXXB set B_ZHDQXYFZ = ISNULL(B_ZHDQXYFZ,0) " + fs + " where B_DLYX='" + dlyx + "'";

    

        return strs;

    }



    /// <summary>
    /// 判断合同编号是否存在违规扣罚,注：应用此方法必须具备前提条件：中标定标信息表中的清盘状态为清盘结束。
    /// </summary>
    /// <param name="htbh">合同编号</param>
    /// <returns></returns>
    public  bool CanDoing(string htbh)
    {
        string str = " select top 1 * from  AAA_View_JYFXYMXDic where 合同编号='" + htbh + "' and 清盘状态='清盘结束'";
        DataTable dt = DbHelperSQL.Query(str).Tables[0];

        if (dt != null && dt.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    /// <summary>
    /// 重载。判断合同编号是否存在违规扣罚 注：应用此方法必须具备前提条件：中标定标信息表中的清盘状态为清盘结束。
    /// </summary>
    /// <param name="htbh">合同编号</param>
    /// <param name="dlyx">卖方或者买方登陆邮箱</param>
    /// <param name="zhxz">账户性质</param>
    /// <returns></returns>
    public bool CanDoing(string htbh, string dlyx, string zhxz)
    {

        DataTable dtemp2 = DbHelperSQL.Query("select B_JSZHLX, J_SELJSBH,J_BUYJSBH,J_JJRJSBH from AAA_DLZHXXB  where B_DLYX='" + dlyx + "'").Tables[0];
        //string jszhlx = dtemp2.Rows[0]["B_JSZHLX"].ToString().Trim();

        string jsbh = "";
        if (zhxz == "买家" && dtemp2.Rows[0]["J_BUYJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_BUYJSBH"].ToString().Trim();
        }
        else if (zhxz == "卖家" && dtemp2.Rows[0]["J_SELJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_SELJSBH"].ToString().Trim();
        }
        else if (zhxz == "经纪人" && dtemp2.Rows[0]["J_JJRJSBH"] != null)
        {
            jsbh = dtemp2.Rows[0]["J_JJRJSBH"].ToString().Trim();
        }

        string str = " select top 1 * from  AAA_View_JYFXYMXDic where 合同编号='" + htbh + "' and 角色编号='" + jsbh + "' and 清盘状态='清盘结束'";
        DataTable dt = DbHelperSQL.Query(str).Tables[0];

        if (dt != null && dt.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    /// <summary>
    /// 判断合同编号是否存在违规扣罚 注：应用此方法必须具备前提条件：中标定标信息表中的清盘状态为清盘结束，或者是即将改写状态为清盘结束但尚未。此方法为针对合同期满监控对CanDing方法所做的修改
    /// </summary>
    /// <param name="htbh">合同编号</param>   
    /// <param name="jsbh">角色编号</param>
    /// <returns></returns>
    public bool IfCan(string htbh,  string jsbh)
    {      

        string str = " select top 1 * from  AAA_View_JYFXYMXDic where 合同编号='" + htbh + "' and 角色编号='" + jsbh + "' ";
        DataTable dt = DbHelperSQL.Query(str).Tables[0];

        if (dt != null && dt.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    #region//自身账户（卖出）

    /// <summary>
    ///自身账户（卖出）是否完全履约  自身账户买入【合同[x1]完全履约	+10	增项	清盘结束】【合同[x1]未完全履约	-1	减项	清盘结束】
    ///注：应用此方法必须具备前提条件：中标定标信息表中的清盘状态为清盘结束。
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="htbh">合同编号</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] ZSZHMC_SFWQLV(string dlyx, string htbh, string zhxz, string creater)
    {
        bool canDoing = CanDoing(htbh,dlyx,zhxz);//判断是否完全履约
        if (canDoing == true)//完全履约
        {
            return SqlText("1309000009", dlyx, htbh, "", zhxz, creater);

        }
        else//为完全履约
        {
            return SqlText("1309000010", dlyx, htbh, "", zhxz, creater);
        }
    }


    /// <summary>
    ///自身账户（卖出）合同[x1]未定标	-1	减项	定标监控
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="htbh">合同编号</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] ZSZHMC_ZBHWDB(string dlyx, string htbh, string zhxz, string creater)
    {

        return SqlText("1309000011", dlyx, htbh, "", zhxz, creater);

    }


    /// <summary>
    ///自身账户（卖出）[x1]投标单服务中心审核异常	-1	减项	服务中心审核时
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="tbdbh">投标单编号</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] ZSZHMC_TBDFWZXSHYC(string dlyx, string tbdbh, string zhxz, string creater)
    {

        return SqlText("1309000012", dlyx, tbdbh, "", zhxz, creater);

    }


    /// <summary>
    ///自身账户（卖出）[x1]异常投标单交易管理部审核未通过	-1	减项	交易管理部审核时
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="tbdbh">投标单编号</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] ZSZHMC_YCTBDJYGLBSHWTG(string dlyx, string tbdbh, string zhxz, string creater)
    {

        return SqlText("1309000013", dlyx, tbdbh, "", zhxz, creater);

    }


    /// <summary>
    ///自身账户（卖出）  合同[x1]因争议进行法律诉讼，被判定为责任方的。	-2	减项	业务平台扣减
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="htbh">合同编号</param>
    /// <param name="zhxz">账户性质：卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] ZSZHMC_YZYJXFLSS(string dlyx, string htbh, string zhxz, string creater, string bz)
    {

        return SqlText("1309000014", dlyx, htbh, "", zhxz, creater,bz);
    }


    #endregion



    #region//自身账户（买入）

    /// <summary>
    ///自身账户（买入）是否完全履约  自身账户买入【合同[x1]完全履约	+10	增项	清盘结束】【合同[x1]未完全履约	-1	减项	清盘结束】
    ///注：应用此方法必须具备前提条件：中标定标信息表中的清盘状态为清盘结束。
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="htbh">合同编号</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] ZSZHMR_SFWQLV(string dlyx, string htbh, string zhxz, string creater)
    {
        bool canDoing = CanDoing(htbh,dlyx,zhxz);//判断是否完全履约
        if (canDoing == true)//完全履约
        {
            return SqlText("1309000006", dlyx, htbh, "", zhxz, creater);

        }
        else//为完全履约
        {
            return SqlText("1309000007", dlyx, htbh, "", zhxz, creater);
        }
    }


    /// <summary>
    ///自身账户（买入）  合同[x1]因争议进行法律诉讼，被判定为责任方的。
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="htbh">合同编号</param>
    /// <param name="zhxz">账户性质：买家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] ZSZHMR_YZYJXFLSS(string dlyx, string htbh, string zhxz, string creater,string bz)
    {

        return SqlText("1309000008", dlyx, htbh, "", zhxz, creater,bz);
    }
    #endregion



    #region//自身开户申请
    /// <summary>
    ///自身开户申请 [x1]被经纪人审核时驳回	-0.5	减项	经纪人审核时
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] ZSKHSQ_BJJRSHSBH(string dlyx, string zhxz, string creater)
    {
        return SqlText("1309000015", dlyx, dlyx, "", zhxz, creater);
    }

    /// <summary>
    ///自身开户申请 [x1]被分公司审核时驳回	-0.5	减项	分公司审核时
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] ZSKHSQ_BFGSSHSBH(string dlyx, string zhxz, string creater)
    {
        return SqlText("1309000016", dlyx, dlyx, "", zhxz, creater);
    }

    /// <summary>
    ///自身开户申请 [x1]被平台终审交易管理部同意冻结	-0.5	减项	平台终审时
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] ZSKHSQ_BPTJYGLBTYDJ(string dlyx, string zhxz, string creater)
    {
        return SqlText("1309000017", dlyx, dlyx, "", zhxz, creater);
    }

    #endregion


    #region//关联交易方
    /// <summary>
    ///关联交易方     关联交易方[x1]，合同[x2]完全履约	+1	增项	清盘结束
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="mjmjDLYX">买家卖家登录邮箱</param>
    /// <param name="htbh">合同编号</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] GLJYF_WQLY(string dlyx, string mjmjDLYX, string htbh, string zhxz, string creater)
    {
        return SqlText("1309000002", dlyx, mjmjDLYX, htbh, zhxz, creater);
    }


    /// <summary>
    ///关联交易方    关联交易方[x1]，合同[x2]未完全履约	 -1	减项	清盘结束
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="mjmjDLYX">买家卖家登录邮箱</param>
    /// <param name="htbh">合同编号</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] GLJYF_WWQLY(string dlyx, string mjmjDLYX, string htbh, string zhxz, string creater)
    {
        return SqlText("1309000003", dlyx, mjmjDLYX, htbh, zhxz, creater);
    }


    /// <summary>
    ///关联交易方   关联交易方[x1]，被分公司审核时驳回	-1	减项	分公司审核时
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="mjmjDLYX">买家卖家登录邮箱</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] GLJYF_BFGSSHSBH(string dlyx, string mjmjDLYX,  string zhxz, string creater)
    {
        return SqlText("1309000004", dlyx, mjmjDLYX, "", zhxz, creater);
    }


    /// <summary>
    ///关联交易方   关联交易方[x1]，被平台终审交易管理部同意冻结	-1	减项	平台终审时
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <param name="mjmjDLYX">买家卖家登录邮箱</param>
    /// <param name="zhxz">账户性质：买家、卖家、经纪人</param>
    /// <param name="creater">创建者</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] GLJYF_BPTJYGLBTYDJ(string dlyx, string mjmjDLYX, string zhxz, string creater)
    {
        return SqlText("1309000005", dlyx, mjmjDLYX, "", zhxz, creater);
    }



    #endregion


    #region//经验交流
    /// <summary>
    ///经验交流   加分sql
    /// </summary>
    /// <param name="dlyx">登录邮箱</param>
    /// <returns>要返回的执行Sql语句</returns>
    public string[] JYJL(string dlyx)
    {
        return SqlText("1309000018", dlyx, dlyx, "", "买家", "admin");
    }
    #endregion









}