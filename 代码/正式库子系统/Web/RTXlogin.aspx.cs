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
using RTXServerApi;
using RTXClient;
using RTXCAPILib;

public partial class Web_RTXlogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request["rtx"] != null && Request["rtx"].ToString() != "")
            {
                RTXSevera obiRtxSever = new RTXSevera();

                /*这里需要一个业务操作平台账号跟RTX账号的对应暂时没有*/
                if (1 == 1)
                {
                    string UserID = DES.DESDecrypt(Request["rtx"].ToString(), "TMDkey22", "TMDiv222");
                    string severIP = "192.168.0.11";
                    if (!Page.IsStartupScriptRegistered("RTXClientLogin"))
                    {
                        Page.RegisterStartupScript("RTXClientLogin", obiRtxSever.GenerateClientLoginScript(UserID, severIP));
                    }
                }
                else
                {
                    //FMOP.Common.MessageBox.Show(Page, "您的RTX未能登陆，可能是您还未分配RTX账号！");
                }
            }
        }
        catch (Exception ex)
        {
            //FMOP.Common.MessageBox.Show(Page, ex + "可能是您还未分配RTX账号！");
        }
    }


}