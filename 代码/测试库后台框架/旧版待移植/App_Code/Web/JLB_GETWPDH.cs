using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// JLB_GETWPDH 的摘要说明
/// </summary>
public class JLB_GETWPDH
{
    private JLB_WPDH jlb_wpdh;

    string _lpbh;
    string _lpmc;
    int _dhsl;
    float _xyjf;
    float _hjjf;

    public JLB_GETWPDH(string lpbh, string lpmc, int dhsl, float xyjf ,float hjjf)
	{
        _lpbh = lpbh;
        _lpmc = lpmc;
        _dhsl = dhsl;
        _xyjf = xyjf;
        _hjjf = hjjf;
	}
    /// <summary>
    /// JLB_WPDH对象
    /// </summary>
    /// <returns></returns>
    public JLB_WPDH getlplist()
    {
        JLB_WPDH jlbw = new JLB_WPDH();
        jlbw.LPBH = _lpbh;
        jlbw.LPMC = _lpmc;
        jlbw.DHSL = _dhsl;
        jlbw.XYJF = _xyjf;
        jlbw.HJJF = _hjjf;
        return jlbw;
    }
}
