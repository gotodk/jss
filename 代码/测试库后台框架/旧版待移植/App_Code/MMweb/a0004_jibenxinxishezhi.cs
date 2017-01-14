using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using FMOP.DB;
using System.Web.UI;

/// <summary>
///a0004_jibenxinxishezhi 的摘要说明
/// </summary>
public class a0004_jibenxinxishezhi
{
    public a0004_jibenxinxishezhi()
	{
       
	}

    static public void CheckJBXX(Page p)
    {
        string a = CookieClass.GetCookie("FMCookie_DLZH");
        string URL = HttpContext.Current.Request.Url.ToString(); //当前登录的url，用来判断是否是买家超市的后台
        if (URL.IndexOf("MMweb") > 0)
        {
            if (URL.IndexOf("a0004chaoshi") > 0)
            {
                if (URL.IndexOf("chaoshihoutai") > 0)
                {
                    string strSQL = "select * from MM_BuyCSJBXXB where YHDLZH='" + a + "'";
                    DataSet dsmaijia = DbHelperSQL.Query(strSQL);

                    string MJCSMC = "";//超市名称
                    string LXRXM = "";//联系人姓名
                    string LXRDH = "";//联系人电话
                    string SSDQ = "";//所属地区
                    string XXDZ = "";//详细地址
                    if (dsmaijia.Tables[0].Rows.Count > 0)
                    {
                        MJCSMC = dsmaijia.Tables[0].Rows[0]["MJCSMC"].ToString();
                        LXRXM = dsmaijia.Tables[0].Rows[0]["LXRXM"].ToString();
                        LXRDH = dsmaijia.Tables[0].Rows[0]["LXRDH"].ToString();
                        SSDQ = dsmaijia.Tables[0].Rows[0]["SZSF"].ToString() + dsmaijia.Tables[0].Rows[0]["SZDS"].ToString() + dsmaijia.Tables[0].Rows[0]["SZQX"].ToString();
                        XXDZ = dsmaijia.Tables[0].Rows[0]["XXDZ"].ToString();
                    }

                    if (MJCSMC == "" || LXRXM == "" || LXRDH == "" || SSDQ == "" || XXDZ == "")
                    {
                        string myScript = "art.dialog({content: '您尚未完善该权限页面的资料，将前往完善资料页面',icon: 'warning', lock: true,button: [{name: '确定',callback: function () {window.location.href='/services/MMweb/a0004chaoshi/chaoshihoutai/majiahoutai_shezhi.aspx';},focus: true}]});";
                        p.ClientScript.RegisterStartupScript(p.GetType(), "MyScript", myScript, true);
                    }
                }
            }
        }

    }
}