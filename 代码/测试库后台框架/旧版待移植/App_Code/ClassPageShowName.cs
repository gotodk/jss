using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using Hesion.Brick.Core.WorkFlow;

/// <summary>
///ClassPageShowName 的摘要说明
/// </summary>
public class ClassPageShowName
{
	public ClassPageShowName()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public void SetThisPageTitle(Page page,ContentPlaceHolder CPH)
    {
        string bz = page.Request.Url.ToString();
        DataSet ds_touser = DbHelperSQL.Query("select * from FM_YMLMMCSZ where  charindex(UPPER(BZZFC) , UPPER('" + bz + "')) > 0 ");
        if (ds_touser.Tables[0].Rows.Count > 0)
        {
            CPH.Page.Title = ds_touser.Tables[0].Rows[0]["BTMC"].ToString();
        }
        else
        {
            CPH.Page.Title = "富美网(测试版)";
        }
    }
}