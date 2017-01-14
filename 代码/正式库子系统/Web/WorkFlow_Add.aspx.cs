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
using FMOP.XParse;
using FMOP.Module;
using FMOP.Execute;
//using FMOP.WF;
//using Hesion.Brick.Core.WorkFlow;
public partial class WorkFlow_Add : System.Web.UI.Page
{
    string module = string.Empty;

    /// <summary>
    /// 输入修改画面的Html代码
    /// </summary>
    /// <param name="output"></param>
    protected override void Render(HtmlTextWriter output)
    {
        string Html = string.Empty;
        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter htmlOutPut = new HtmlTextWriter(sw);
        this.RadTabStrip1.TabIndex = 0;
        if (!Page.IsPostBack)
        {
            if (Request["module"] != null && Request["module"].ToString() != "")
            {
                module = Request["module"];

                //设定不同操作的转向页面
                setRadTabUrl();
                //WorkFlowModule实例化
                Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
                Hesion.Brick.Core.WorkFlow.UserModuleAuth um = new Hesion.Brick.Core.WorkFlow.UserModuleAuth();
                Hesion.Brick.Core.WorkFlow.absolutenessModule abModule = new Hesion.Brick.Core.WorkFlow.absolutenessModule(module);
                //获取岗位权限对象
                um= wf.authentication.GetAuthByUserNumber(User.Identity.Name);
                //WorkFlowRole role = new WorkFlowRole(module, User.Identity.Name);

                if (um.CanAdd == true || abModule.CanAdd == true)
                {
                    FMOP.Module.WorkFlowModule addWorkFlow = new WorkFlowModule(module);
                    RenderChildren(htmlOutPut);
                    Html = addWorkFlow.GetHtml("Add", sw.ToString());
                    output.Write(Html);

                }
                else if (um.CanView == true)
                {
                    Response.Redirect("WorkFlow_View.aspx?module=" + module);
                }
                else if (um.CanModify == true)
                {
                    Response.Redirect("WorkFlow_Edit.aspx?module=" + module);
                }
                else
                {
                    Response.Clear();
                    Response.Write("<script language=javascript>window.alert(\"权限错误,请执行解决操作:\\n\\t[*]: 重新登录系统\\n错误原因:\\n\\t当前用户无执行操作的权限!\");history.back();</script>");
                }
            }
            else
            {
                Response.Clear();
                Response.Write("<script language=javascript>window.alert(\"系统错误:无模块可进行操作！\\n执行确定后页面将返回·空白页面·!\");history.back();</script>");
            }
        }
    }

    /// <summary>
    /// 设定RadTabStrip的转向页面
    /// </summary>
    private void setRadTabUrl()
    {
        string ModeName = ExecuteDB.GetModuleName(module);
        this.RadTabStrip1.Tabs[0].Text = ModeName + "增加";
        this.RadTabStrip1.Tabs[1].Text = ModeName + "查看";
        this.RadTabStrip1.Tabs[2].Text = ModeName + "修改、删除";
        this.RadTabStrip1.Tabs[0].ForeColor = System.Drawing.Color.Red;
        this.RadTabStrip1.Tabs[0].NavigateUrl = "WorkFlow_Add.aspx?module=" + module;
        this.RadTabStrip1.Tabs[1].NavigateUrl = "WorkFlow_View.aspx?module=" + module;
        this.RadTabStrip1.Tabs[2].NavigateUrl = "WorkFlow_Edit.aspx?module=" + module;
       
        //若存在审核,则添加审核
        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
        if ((wf.check != null )&&(wf.check .GetCheckedContent ()))//FMOP.CI.CheckInfo.ifEnsChecker(module)
        {
            Telerik.WebControls.Tab ss = new Telerik.WebControls.Tab(TabTool.getTitle(module) + "审核");
            ss.NavigateUrl = "WorkFlow_Check.aspx?module=" + module;
            RadTabStrip1.Tabs.Add(ss);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
