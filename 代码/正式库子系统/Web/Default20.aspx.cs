using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using FMOP.WN;

public partial class Default20 : System.Web.UI.Page
{
    public string warncount;
    public string remindcount;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        this.warncount =Hesion.Brick.Core.WorkFlow.Warning.GetWarningCount(User.Identity.Name).ToString();
        this.remindcount = Hesion.Brick.Core.WorkFlow.Warning.GetRemindCount(User.Identity.Name).ToString();
    }

    public string WarnCount
    {
        get
        {
            return this.warncount;
        }
        set
        {
            this.warncount = value;
        }
    }
    public string RemindCount
    {
        get
        {
            return this.remindcount;
        }
        set
        {
            this.remindcount = value;
        }
    }
}
