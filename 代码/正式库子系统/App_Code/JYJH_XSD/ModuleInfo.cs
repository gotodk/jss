using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Hesion.Brick.Core.WorkFlow;
/// <summary>
/// ModuleInfo 的摘要说明
/// </summary>
public class ModuleInfo
{

    /// <summary>
    /// 模块主键
    /// </summary>
    private string number = "";
    public string Number
    {
        set
        {
            number = value;
        }
        get
        {
            return number;
        }
    }

    private string nextchecker = "";
    private string checkstate = "0";

    public string NextChecker
    {
        get
        {
            return nextchecker;
        }
        set
        {
            nextchecker = value;
        }
    }

    public string CheckState
    {
        get
        {
            return checkstate;
        }
        set
        {
            checkstate = value;
        }
    }

    private int minue = 0;
    public int Minue
    {
        get
        {
            return minue;
        }
        set
        {
            minue = value;
        }
    }

    public ModuleInfo()
    {
    }
	public ModuleInfo( string module,string userName)
	{
        //取主键
        WorkFlowModule WFM = new WorkFlowModule(module);
        number = WFM.numberFormat.GetNextNumber();
        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
        nextchecker = wf.check != null ? wf.check.GetFirstCheckRole(number,userName) : "";
	    minue = wf.check != null ? wf.check.GetFirstCheckRoleLimitTime() :0;
        checkstate = !string.IsNullOrEmpty(nextchecker) ? "0" : "1";
	}

}
