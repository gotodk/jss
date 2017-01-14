<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OpenUser.aspx.cs" Inherits="Web_XXHZX_OpenUser" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" Height="25px"
            ReorderTabRows="True" Skin="Default2006" Width="580px">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" ImageUrl="~/RadControls/Grid/Skins/AddRecord.gif"
                    NavigateUrl="OpenUser.aspx" Text="用户解锁">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
          <radg:RadGrid ID="RadGrid1"  runat="server" DataSourceID="SqlDataSource1" 
         GridLines="None" Skin="Monochrome" AllowPaging="True" AllowSorting="True" OnPageIndexChanged="RadGrid1_PageIndexChanged" PageSize="15">
           <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条." PrevPageText="上一页"></PagerStyle>
            
            <MasterTableView AutoGenerateColumns="False" DataKeyNames="Number" DataSourceID="SqlDataSource1">
             <NoRecordsTemplate>
                     没有找到任何数据
                </NoRecordsTemplate>
                <ExpandCollapseColumn Resizable="False" Visible="False">
                    <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
                <Columns>
                    <radg:GridBoundColumn DataField="Number" HeaderText="员工编号" ReadOnly="True" SortExpression="Number"
                        UniqueName="Number">
                    </radg:GridBoundColumn>
                    <radg:GridBoundColumn DataField="Employee_Name" HeaderText="员工姓名" SortExpression="Employee_Name" UniqueName="Employee_Name">
                    </radg:GridBoundColumn>
                    <radg:GridBoundColumn DataField="GWMC" HeaderText="岗位名称" SortExpression="GWMC"
                        UniqueName="GWMC">
                    </radg:GridBoundColumn>
                     <radg:GridBoundColumn DataField="BM" HeaderText="部门" SortExpression="BM"
                        UniqueName="BM">
                    </radg:GridBoundColumn>
                    <radg:GridTemplateColumn UniqueName="TemplateColumn">
                        <ItemTemplate>
                        <a href='open.aspx?number=<%# DataBinder.Eval(Container, "DataItem.Number") %>'><img alt=""  src="../images/Update.gif"/></a>
                        </ItemTemplate>
                        <HeaderTemplate>
                             解锁
                        </HeaderTemplate>
                    </radg:GridTemplateColumn>                   
                </Columns>
            </MasterTableView>
            <ExportSettings>
                <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                    PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
            </ExportSettings>
        </radg:RadGrid>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FMOPConn %>"
            SelectCommand="SELECT e.Number, e.Employee_Name, e.GWMC, e.BM, m.IsLockedOut FROM HR_Employees AS e INNER JOIN aspnet_Users AS u ON e.Number = u.UserName INNER JOIN aspnet_Membership AS m ON u.UserId = m.UserId WHERE (m.IsLockedOut = 1)">
        </asp:SqlDataSource>
    </form>
</body>
</html>
