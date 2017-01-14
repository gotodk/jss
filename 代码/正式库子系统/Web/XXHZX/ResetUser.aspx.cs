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
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;

public partial class Web_XXHZX_ResetUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DefinedModule module = new DefinedModule("YGZHDJ");
        Authentication auth = module.authentication;
        if (auth == null)
        {
            MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
        }
        if (!auth.GetAuthByUserNumber(User.Identity.Name).CanView)
        {
            MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
        }

        System.Globalization.CultureInfo ci = null;
        ci = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");
        System.Threading.Thread.CurrentThread.CurrentCulture = ci;
    }
    protected void RadGrid1_PageIndexChanged(object source, Telerik.WebControls.GridPageChangedEventArgs e)
    {
        RadGrid1.CurrentPageIndex = e.NewPageIndex;
        string sqldj = "";
        if (ListBox1.SelectedValue == "不限")
        {
            sqldj = "";
        }
        if (ListBox1.SelectedValue == "已冻结")
        {
            sqldj = " and IsApproved = 0 ";
        }
        if (ListBox1.SelectedValue == "未冻结")
        {
            sqldj = " and IsApproved = 1 ";
        }
        string sql = "";
        sql = "SELECT e.Number, e.BM,  e.GWMC,e.Employee_Name,e.YGZT, (CASE IsApproved WHEN 1 THEN '未冻结' WHEN 0 THEN '已冻结' ELSE '未知' END) as 冻结状态  ,'冻结人工号'=(select top 1 PRL.DJ_hg FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '冻结' order by PRL.DJ_time DESC),'冻结人姓名'=(select top 1 PRL.DJ_name FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '冻结' order by PRL.DJ_time DESC),  '冻结时间'=( select top 1 PRL.DJ_time FROM PassReLog PRL   where PRL.OP_gh = e.Number and PRL.OP_type = '冻结'  order by PRL.DJ_time DESC), '解冻人工号'=(select top 1 PRL.UDJ_hg FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '解冻'  order by PRL.UDJ_time DESC ),  '解冻人姓名'=( select top 1 PRL.UDJ_name FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '解冻'  order by PRL.UDJ_time DESC ),  '解冻操时间'=( select top 1 PRL.UDJ_time FROM PassReLog PRL   where PRL.OP_gh = e.Number and PRL.OP_type = '解冻'    order by PRL.UDJ_time DESC )  FROM HR_Employees AS e INNER JOIN aspnet_Users AS u ON e.Number = u.UserName INNER JOIN aspnet_Membership AS m ON u.UserId = m.UserId where(e.ygzt<>'离职')  and  e.Number LIKE '%" + yggh.Text.Trim() + "%' AND e.Employee_Name LIKE '%" + ygxm.Text.Trim() + "%' " + sqldj + " ";
        SqlDataSource1.SelectCommand = sql;
        SqlDataSource1.DataBind();
    }
    protected void EditButton_Click(object sender, EventArgs e)
    {
        string sqldj = "";
        if (ListBox1.SelectedValue == "不限")
        {
            sqldj = "";
        }
        if (ListBox1.SelectedValue == "已冻结")
        {
            sqldj = " and IsApproved = 0 ";
        }
        if (ListBox1.SelectedValue == "未冻结")
        {
            sqldj = " and IsApproved = 1 ";
        }
        string sql = "";
        sql = "SELECT e.Number, e.BM,  e.GWMC,e.Employee_Name,e.YGZT, (CASE IsApproved WHEN 1 THEN '未冻结' WHEN 0 THEN '已冻结' ELSE '未知' END) as 冻结状态  ,'冻结人工号'=(select top 1 PRL.DJ_hg FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '冻结' order by PRL.DJ_time DESC),'冻结人姓名'=(select top 1 PRL.DJ_name FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '冻结' order by PRL.DJ_time DESC),  '冻结时间'=( select top 1 PRL.DJ_time FROM PassReLog PRL   where PRL.OP_gh = e.Number and PRL.OP_type = '冻结'  order by PRL.DJ_time DESC), '解冻人工号'=(select top 1 PRL.UDJ_hg FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '解冻'  order by PRL.UDJ_time DESC ),  '解冻人姓名'=( select top 1 PRL.UDJ_name FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '解冻'  order by PRL.UDJ_time DESC ),  '解冻操时间'=( select top 1 PRL.UDJ_time FROM PassReLog PRL   where PRL.OP_gh = e.Number and PRL.OP_type = '解冻'    order by PRL.UDJ_time DESC )  FROM HR_Employees AS e INNER JOIN aspnet_Users AS u ON e.Number = u.UserName INNER JOIN aspnet_Membership AS m ON u.UserId = m.UserId where(e.ygzt<>'离职')  and  e.Number LIKE '%" + yggh.Text.Trim() + "%' AND e.Employee_Name LIKE '%" + ygxm.Text.Trim() + "%' " + sqldj + " ";
        SqlDataSource1.SelectCommand = sql;
        SqlDataSource1.DataBind();
        RadGrid1.DataBind();
    }
}
