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
using System.Text.RegularExpressions;
using Telerik.WebControls;
using Hesion.Brick.Core;
using FMOP.XParse;
using FMOP.Module;
using FMOP.DB;
using FMOP.Tools;

public partial class OrderForm : System.Web.UI.Page
{
    string module = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bool CanAdd = false;
            if (Request["module"] != null && Request["module"].ToString() == "DDZXSD")
            {
                // module = Request["module"].ToString();
                module = "GNKH_Order";
                Hesion.Brick.Core.WorkFlow.DefinedModule um = new Hesion.Brick.Core.WorkFlow.DefinedModule(module);

                //获取岗位权限对象
                CanAdd = um.authentication.GetAuthByUserNumber(User.Identity.Name).CanAdd;
                if (CanAdd == true)
                {
                    Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule("GNKH_Order");
                    if (wf.authentication != null)
                    {
                        string cmdText = "";
                        int ViewType = wf.authentication.GetAuthByUserNumber(User.Identity.Name).ViewType;
                        switch (ViewType)
                        {
                            case 1:
                                cmdText = @"SELECT Number,CUST_ID,CUST_NAME,LINKMAN_NAME,LINKMAN_TEL,RECEIVE_NAME,RECEIVE_TEL FROM GNKH_Order 
                                    WHERE Number NOT IN (select DDH from cszx_XSD) AND CheckState =1 AND GNKH_Order.CreateUser='" + User.Identity.Name + "'  ORDER BY Number DESC";
                                break;
                            case 2:
                                cmdText = @"SELECT Number,CUST_ID,CUST_NAME,LINKMAN_NAME,LINKMAN_TEL,RECEIVE_NAME,RECEIVE_TEL 
                                    FROM GNKH_Order WHERE Number
                                    NOT IN (select DDH from cszx_XSD) AND CheckState =1 AND GNKH_Order.CreateUser  
                                    IN (select Number from HR_Employees where BM IN (select BM from HR_Employees where Number='" + User.Identity.Name + "')) ORDER BY Number DESC";
                                break;
                            case 3:
                                cmdText = "SELECT Number,CUST_ID,CUST_NAME,LINKMAN_NAME,LINKMAN_TEL,RECEIVE_NAME,RECEIVE_TEL FROM GNKH_Order WHERE Number NOT IN (select DDH from cszx_XSD) AND CheckState =1 ORDER BY Number DESC";
                                break;
                        }
                        this.SqlDataSource1.SelectCommand = cmdText;
                        this.SqlDataSource1.DataBind();
                    }

                }
                else
                {
                    Response.Clear();
                    Response.Write("<script language=javascript>window.alert(\"权限错误,请执行解决操作:\\n\\t[*]: 重新登录系统\\n错误原因:\\n\\t当前用户无执行操作的权限!\");history.back();</script>");
                }
            }
        }
        else
        {
            Response.Clear();
            Response.Write("<script language=javascript>window.alert(\"系统错误:无模块可进行操作！\\n执行确定后页面将返回·空白页面·!\");history.back();</script>");
        }

        if (module == "GNKH_Order")
        {
            this.RadTabStrip1.Tabs[0].Text = "生成销售单";
            this.RadTabStrip1.Tabs[0].ForeColor = System.Drawing.Color.Red;
            this.RadTabStrip1.Tabs[0].NavigateUrl = @"OrderForm.aspx?module=DDZXSD";
        }
    }

    /// <summary>
    /// 设置并生成销售单
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void saleGrid_ItemCommand(object source, Telerik.WebControls.GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("create"))
        {
            //订单的主键编号
            Label lblNumber = (Label)e.Item.FindControl("lblNumber");
            string Keys = lblNumber.Text.ToString();
            Response.Redirect("actionOrder.aspx?module=GNKH_Order&Number=" + Keys);
        }
    }
}
