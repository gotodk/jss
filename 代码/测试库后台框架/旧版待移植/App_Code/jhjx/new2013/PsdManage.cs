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

/// <summary>
/// PsdManage 的摘要说明
/// </summary>
public class PsdManage
{
	public PsdManage()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
	public DataSet RunPsdChange(DataSet dsreturn, DataTable dt)
	{
		//进行处理....
		if (dt != null && dt.Rows.Count > 0)
		{
            Hashtable htUserVal = PublicClass2013.GetUserInfo(dt.Rows[0]["买家角色编号"].ToString());//通过买家角色编号获取信息

            if (htUserVal["是否休眠"].ToString().Trim() == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                return dsreturn;
            }

			string str = "UPDATE AAA_DLZHXXB SET B_DLMM='"+dt.Rows[0]["xma"].ToString().Trim()+"' WHERE B_DLYX='"+dt.Rows[0]["dlyx"].ToString().Trim()+"'";
			List<string> list = new List<string>();
			list.Add(str);

			if (DbHelperSQL.ExecuteSqlTran(list) > 0)
			{
				//获得结果
				dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
				dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的密码修改成功！";

			}
			else
			{
				dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
				dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "密码修改失败，请稍后重试！";

			}
		}
		else 
		{

			//获得结果
			dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
			dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "密码修改失败，请稍后重新尝试！";

			//附加数据表
		}
		return dsreturn;

	}



    public DataSet RunZQZJMMChange(DataSet dsreturn, DataTable dt)
    {
        //进行处理....
        if (dt != null && dt.Rows.Count > 0)
        {
            Hashtable htUserVal = PublicClass2013.GetUserInfo(dt.Rows[0]["买家角色编号"].ToString());//通过买家角色编号获取信息

            if (htUserVal["是否休眠"].ToString().Trim() == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
                return dsreturn;
            }

            string str = "UPDATE AAA_DLZHXXB SET B_JSZHMM='" + dt.Rows[0]["新证券资金密码"].ToString().Trim() + "' WHERE B_DLYX='" + dt.Rows[0]["登录邮箱"].ToString().Trim() + "'";
            List<string> list = new List<string>();
            list.Add(str);

            if (DbHelperSQL.ExecuteSqlTran(list) > 0)
            {
                //获得结果
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易资金密码修改成功！";

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易资金密码修改失败，请稍后重试！";

            }
        }
        else
        {

            //获得结果
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易资金密码修改失败，请稍后重新尝试！";

            //附加数据表
        }
        return dsreturn;

    }
}