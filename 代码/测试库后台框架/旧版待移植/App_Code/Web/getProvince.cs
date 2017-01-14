using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.DB;

/// <summary>
/// getProvince 的摘要说明
/// </summary>
public class getProvince
{
	public getProvince()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 根据省份取得所属城市中心
    /// </summary>
    /// <param name="province">省份</param>
    /// <returns></returns>
    public static string getSSCSZX(string province)
    {
        string SSCSZX = "";
        string sql = "select name from system_city where province like '%" + province + "%' ";
        if (DbHelperSQL.Query(sql).Tables[0].Rows.Count < 1)
        {
            return "山东省";
        }
        else
        {
            SSCSZX = DbHelperSQL.Query(sql).Tables[0].Rows[0][0].ToString();
        }
        
        return SSCSZX;
    }
}
