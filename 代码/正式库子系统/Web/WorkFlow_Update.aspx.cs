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
using FMOP.DB;
using FMOP.Execute;
//using FMOP.WF;
public partial class WorkFlow_Update : System.Web.UI.Page
{
    string module = string.Empty;
    string number = string.Empty;
    string pageNumber = string.Empty;
    
    /// <summary>
    /// 生成页面Html代码，并输出到画面上
    /// </summary>
    /// <param name="output"></param>
    protected override void Render(HtmlTextWriter output)
    {
        string htmlCode = string.Empty;

        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter htmlOutPut = new HtmlTextWriter(sw);
        this.RadTabStrip1.TabIndex = 0;
        if (!Page.IsPostBack)
        {
            if (Request["module"] != null && Request["Number"] != null && Request["module"].ToString() != "" && Request["Number"].ToString() != "")
            {
                module = Request["module"];
                number = Request["Number"];
                if (Request["pagenumber"] != null && Request["pagenumber"].ToString() != "")
                {
                    pageNumber = Request["pagenumber"];
                }

                //设置RadTabStrip的转向页面
                setRadTabUrl();
                Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
                Hesion.Brick.Core.WorkFlow.UserModuleAuth role = new Hesion.Brick.Core.WorkFlow.UserModuleAuth();
                role = wf.authentication.GetAuthByUserNumber(User.Identity.Name);

                //WorkFlowRole role = new WorkFlowRole(module, User.Identity.Name);
                if (role.CanModify == true)
                {
                    WorkFlowModule editWorkFlow = new WorkFlowModule(module, number);
                    RenderChildren(htmlOutPut);
                    if (pageNumber != "")
                    {
                        editWorkFlow.pageNumer = pageNumber;
                    }
                    htmlCode = editWorkFlow.GetHtml("Update", sw.ToString());
                    output.Write(htmlCode);
                }
                else if (role.CanAdd == true)
                {
                    Response.Redirect("WorkFlow_Add.aspx?module=" + module);
                }
                else if (role.CanView == true)
                {
                    Response.Redirect("WorkFlow_View.aspx?module=" + module);
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
        this.RadTabStrip1.Tabs[2].ForeColor = System.Drawing.Color.Red;

        this.RadTabStrip1.Tabs[0].NavigateUrl = "WorkFlow_Add.aspx?module=" + module;
        this.RadTabStrip1.Tabs[1].NavigateUrl = "WorkFlow_View.aspx?module=" + module;
        this.RadTabStrip1.Tabs[2].NavigateUrl = "WorkFlow_Edit.aspx?module=" + module;
        //若存在审核,则添加审核
        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule(module);
        if (wf.check!=null && wf.check .GetCheckedContent ()) //FMOP.CI.CheckInfo.ifEnsChecker(module)
        {   
           Telerik.WebControls.Tab  ss = new Telerik.WebControls.Tab(TabTool.getTitle(module) + "审核");
            ss.NavigateUrl = "WorkFlow_Check.aspx?module=" + module;
            RadTabStrip1.Tabs.Add(ss);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
