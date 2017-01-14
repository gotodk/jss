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

public partial class Web_CustormModule : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (Request["module"] != null)
		{
			string module = Request["module"].ToString();
			string url = FMOP.XParse.xmlParse.XmlGetNodeInnerXml(module, "//AddOnModule/Url");
			bool[] can = new bool[3];
            Hesion.Brick.Core.WorkFlow.DefinedModule dm = new Hesion.Brick.Core.WorkFlow.DefinedModule(module);
			Hesion.Brick.Core.WorkFlow.Authentication aut = dm.authentication;
			if (aut != null)
			{
				can[0] = aut.GetAuthByUserNumber(User.Identity.Name).CanAdd;
				can[1] = aut.GetAuthByUserNumber(User.Identity.Name).CanModify;
				//can[2] = aut.GetAuthByUserNumber(User.Identity.Name).CanModifyAll;
				can[2] = aut.GetAuthByUserNumber(User.Identity.Name).CanView;
				//can[4] = aut.GetAuthByUserNumber(User.Identity.Name).CanViewAll;
			}
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				if (can[i])
					flag = true;
			}
			if (url != null && url != string.Empty)
			{
				if (flag)
				{
					Response.Clear();
					Response.Redirect(url + "?module=" + module);
					Response.End();
				}
				else
				{
					Response.Clear();
					Response.Write("<script language=javascript>window.alert('没有权限！');history.back();</script>");
					Response.End();
					return;
				}
			}
			else
			{
				Response.Clear();
				Response.Write("<script language=javascript>window.alert('配置文件错误！');history.back();</script>");
				Response.End();
				return;
			}
		}
		else
		{
			Response.Clear();
			Response.Write("<script language=javascript>window.alert('参数错误！');history.back();</script>");
			Response.End();
			return;
		}
    }
}
