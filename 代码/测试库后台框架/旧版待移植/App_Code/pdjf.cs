using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using FMOP.DB;

/// <summary>
/// pdjf 的摘要说明
/// </summary>
public class pdjf
{
    private bool jf;
    public pdjf(ArrayList lp, string JLBUserID)
	{
        jf = true;
        ArrayList lplist = new ArrayList();
        lplist = lp;
        float czjf;
        if (DbHelperSQL.Query("SELECT M.HYBH,(ISNULL((SELECT SUM(FS) FROM JLB_JLBHYJFMXB WHERE FS>0 AND HYBH=M.HYBH),0)+ISNULL((SELECT SUM(FS) FROM JLB_JLBHYJFMXB WHERE FS<0 AND HYBH=M.HYBH),0)) AS JF FROM JLB_JLBHYJFMXB M WHERE  HYBH='" + JLBUserID + "'").Tables[0].Rows.Count == 0)
            czjf = 0;
        else
        {
            czjf = float.Parse(DbHelperSQL.Query("SELECT M.HYBH,(ISNULL((SELECT SUM(FS) FROM JLB_JLBHYJFMXB WHERE FS>0 AND HYBH=M.HYBH),0)+ISNULL((SELECT SUM(FS) FROM JLB_JLBHYJFMXB WHERE FS<0 AND HYBH=M.HYBH),0)) AS JF FROM JLB_JLBHYJFMXB M WHERE  HYBH='" + JLBUserID + "'").Tables[0].Rows[0]["JF"].ToString());
        }
        float sumfs = 0;
        JLB_WPDH jlb = new JLB_WPDH();
        for (int i = 0; i < lplist.Count; i++)
        {
            jlb = (JLB_WPDH)lplist[i];
            sumfs = float.Parse(jlb.HJJF.ToString()) + sumfs;
            if (sumfs > czjf)
            {
                jf = false;
            }
        }
	}
    public bool isjf
    {
        get
        {
            return jf;
        }
    }
}
