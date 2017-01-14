using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;

/// <summary>
///setpagekey 的摘要说明
/// </summary>
public class setpagekey
{
	public setpagekey()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public Hashtable setkey(string pntemp)
    {
        Hashtable ht = new Hashtable();
        string pn = pntemp;
        string sql = "select * from BTGJZ where SSYM='" + pn + "'";
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        if (dt.Rows.Count > 0)
        {
            ht["title"] = dt.Rows[0]["YMBT"].ToString();
            ht["KeyStr"] = dt.Rows[0]["GJZ"].ToString();
            ht["DescStr"] = dt.Rows[0]["MS"].ToString();
        }
        else
        {
            ht["title"] = "富美硒鼓品质高、富美硒鼓价格低-富美科技有限公司";
            ht["KeyStr"] = "硒鼓,富美,富美硒鼓,打印机硒鼓,硒鼓价格,环保硒鼓,激光硒鼓,环保激光硒鼓";
            ht["DescStr"] = "富美科技有限公司电子商务平台服务于您的硒鼓订购,硒鼓价格低,品质高;服务于您的硒鼓回收,推动中国乃至世界的环保进步。";
        }
        return ht;
    }
}
