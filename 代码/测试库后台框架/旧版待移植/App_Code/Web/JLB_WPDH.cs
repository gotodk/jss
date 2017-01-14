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
/// JLB_WPDH 的摘要说明
/// </summary>
public class JLB_WPDH
{
	public JLB_WPDH()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    private string _lpbh;
    private string _lpmc;
    private int _dhsl;
    private float _xyjf;
    private float _hjjf;

    /// <summary>
    /// 礼品编号
    /// </summary>
    public string LPBH
    {
        set { _lpbh = value; }
        get { return _lpbh; }
    }
    /// <summary>
    /// 礼品名称
    /// </summary>
    public string LPMC
    {
        set { _lpmc = value; }
        get { return _lpmc; }
    }
    /// <summary>
    /// 兑换数量
    /// </summary>
    public int DHSL
    {
        set { _dhsl = value; }
        get { return _dhsl; }
    }
    /// <summary>
    /// 需要积分
    /// </summary>
    public float XYJF
    {
        set { _xyjf = value; }
        get { return _xyjf; }
    }
    /// <summary>
    /// 合计积分
    /// </summary>
    public float HJJF
    {
        set { _hjjf = value; }
        get { return _hjjf; }
    }
}
