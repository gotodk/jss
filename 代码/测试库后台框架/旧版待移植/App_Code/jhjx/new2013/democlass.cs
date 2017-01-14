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
/// democlass 的摘要说明
/// </summary>
public class democlass
{
	public democlass()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 测试返回值标准
    /// </summary>
    /// <returns></returns>
    public DataSet RunDemo(DataSet dsreturn,string test)
    {

        //进行处理....

        //获得结果
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的[" + test + "]单号操作成功！";

        //附加数据表
        

        return dsreturn;

    }
}