using System;
using System.Collections;
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

/// <summary>
/// 资金处理类，处理所有的资金变动与银行接口
/// </summary>
public class ClassMoney2013
{
	public ClassMoney2013()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 模拟获取用户托管账户中可用余额(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="bankinfo">银行接口的相关信息</param>
    /// <returns></returns>
    public double GetMoneyT(string dlyx,string bankinfo)
    {
        //接口未开发前，用登录表模拟银行托管账户余额
        DataSet dsmoney = DbHelperSQL.Query("select  isnull(B_ZHDQKYYE,0.00) from AAA_DLZHXXB where B_DLYX='" + dlyx + "'");
        if (dsmoney != null && dsmoney.Tables[0].Rows.Count == 1)
        {
            return Convert.ToDouble(dsmoney.Tables[0].Rows[0][0]);
        }
        else
        {  
            return 0.00;
        }
        
    }

    /// <summary>
    /// 模拟冻结或解冻或增加或减少托管账户中的用户资金(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="dlyx">登陆邮箱</param>
    /// <param name="money">金额</param>
    /// <param name="IsF">+、-</param>
    /// <returns></returns>
    public bool FreezeMoneyT(string dlyx,string money, string IsF)
    {
        DbHelperSQL.ExecuteSql(" update AAA_DLZHXXB set B_ZHDQKYYE = B_ZHDQKYYE " + IsF + " " + money + " where B_DLYX='" + dlyx + "'");
        return true;
    }


    /// <summary>
    /// 模拟入金，将用户绑定卡中的资金，转入托管账户中(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    public bool UserToFMT(string temp)
    {
        return true;
    }

    /// <summary>
    /// 模拟出金，将用户托管账户中的资金，转到用户绑定卡中(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    public bool FMTToUser(string temp)
    {
        return true;
    }

    /// <summary>
    /// 模拟从托管账户，转账至平台主账户(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    public bool FMTToFMZ(string temp)
    {
        return true;
    }
    /// <summary>
    /// 模拟从平台主账户，转账至托管账户(若执行错误需立刻短信报警)
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    public bool FMZToFMT(string temp)
    {
        return true;
    }

  

}