<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GWJG.aspx.cs" Inherits="Web_HR_GWJG" %>

<%@ Register Assembly="RadTreeView.Net2" Namespace="Telerik.WebControls" TagPrefix="radT" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>


<html>
<head id="Head1" runat="server">
<title>富美业务操作平台</title>
<style type="text/css">
input1 {
	width: 65px;
	BACKGROUND-COLOR: transparent; 
	BORDER-BOTTOM: #B3B3B3 1px solid;; 
	BORDER-LEFT: transparent 0px solid; 
	BORDER-RIGHT: transparent 0px solid; 
	BORDER-TOP: #D8D8D8 0px solid; COLOR: #666666;
   margin-bottom:4px;  
}
</style>
    <link href="../../css/style.css"
        rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" Height="16px"
            ReorderTabRows="True" Skin="Default2006" Width="580px">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" ImageUrl="~/RadControls/Grid/Skins/AddRecord.gif"
                    NavigateUrl="GWJG.aspx" Text="岗位结构">
                </radTS:Tab>
               
            </Tabs>
        </radTS:RadTabStrip>
        <table border="0" cellpadding="5" cellspacing="0" width="680">
            <tr>
                <td colspan="2" height="25" style="width: 150px" valign="top">
        <radT:RadTreeView ID="RadTreeView1" runat="server">
            <Nodes>
                <radT:RadTreeNode runat="server" Text="New Item">
                    <Nodes>
                        <radT:RadTreeNode runat="server" Text="New Item">
                        </radT:RadTreeNode>
                    </Nodes>
                </radT:RadTreeNode>
                <radT:RadTreeNode runat="server" Text="New Item">
                </radT:RadTreeNode>
            </Nodes>
        </radT:RadTreeView>
                </td>
                <td colspan="1" height="25" width="530px" valign="top">
                    <span class="Title">
                        </span>
                    <radg:RadGrid ID="RadGrid1" runat="server" AllowPaging="True"
                        AllowSorting="True" DataMember="DefaultView" DataSourceID="HR_EmployeesDS"
                        GroupingSettings-CollapseTooltip="折叠" GroupingSettings-ExpandTooltip="展开" GroupingSettings-GroupContinuedFormatString="接上页"
                        GroupingSettings-GroupContinuesFormatString="接下页..." GroupingSettings-GroupSplitDisplayFormat="共有{1}条记录，本页显示{0}条."
                        GroupPanel-Text="可拖动此项至表格上方的区域进行分组" HeaderStyle-Font-Size="12px" HierarchySettings-CollapseTooltip="折叠"
                        HierarchySettings-ExpandTooltip="展开" HorizontalAlign="Center" MasterTableView-NoMasterRecordsText="没有可显示的记录"
                        PagerStyle-CssClass="GridPager" PagerStyle-Mode="NextPrev"
                        PagerStyle-PagerTextFormat="{4} &nbsp;|&nbsp;&nbsp;&nbsp;{0}&nbsp;/&nbsp;{1}&nbsp;页;&nbsp;&nbsp;(&nbsp;{2}&nbsp;-{3}&nbsp;)&nbsp;条记录&nbsp;&nbsp;,共{5}条"
                        PagerStyle-VerticalAlign="Top" Skin="Telerik" SortingSettings-SortedAscToolTip="升序"
                        SortingSettings-SortedDescToolTip="降序" SortingSettings-SortToolTip="按此项排序" Width="530px" GridLines="None">
                        <GroupPanel Text="&amp;nbsp;拖拽一个项目到这里，可按项目内容自动分组" ToolTip="拖拽一个项目到这里，可按项目内容自动分组"
                            Visible="True">
                        </GroupPanel>
                        <PagerStyle CssClass="GridPager" Height="30px" Mode="NextPrevAndNumeric" PagerTextFormat="{4} &#160;|&#160;&#160;&#160;{0}&#160;/&#160;{1}&#160;页;&#160;&#160;(&#160;{2}&#160;-{3}&#160;)&#160;条记录&#160;&#160;,共{5}条"
                            VerticalAlign="Top" />
                        <GroupingSettings CollapseTooltip="折叠" ExpandTooltip="展开" GroupContinuedFormatString="... 接上页. "
                            GroupContinuesFormatString="接下页..." GroupSplitDisplayFormat="共{1}条，本页显示{0}条。" />
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="Number" DataMember="DefaultView"
                            DataSourceID="HR_EmployeesDS" NoMasterRecordsText="没有可显示的记录">
                            <NoRecordsTemplate>
                                没有找到任何数据。
                            </NoRecordsTemplate>
                            <Columns>
                                <radg:GridBoundColumn DataField="Number" HeaderText="员工编号" SortExpression="Number"
                                    UniqueName="Employee_No">
                                </radg:GridBoundColumn>
                                <radg:GridBoundColumn DataField="Employee_Name" HeaderText="姓名" SortExpression="Employee_Name"
                                    UniqueName="Employee_Name">
                                </radg:GridBoundColumn>
                                <radg:GridBoundColumn DataField="GWMC" HeaderText="岗位名称" SortExpression="GWMC" UniqueName="GWMC">
                                </radg:GridBoundColumn>
                                <radg:GridHyperLinkColumn AllowFiltering="False" DataNavigateUrlField="Number"
                                    DataNavigateUrlFormatString="../WorkFlow_View_Detail.aspx?Module=HR_Employees&amp;Number={0}" HeaderText="员工信息" Text="查看"
                                    UniqueName="column">
                                    <HeaderStyle Width="60px" />
                                </radg:GridHyperLinkColumn>
                                <radg:GridHyperLinkColumn  AllowFiltering="False" DataNavigateUrlField="GWSMSBH" DataNavigateUrlFormatString="../WorkFlow_View_Detail.aspx?Module=HR_GWSMS&amp;Number={0}"
                                    HeaderText="岗位说明书" Text="查看" UniqueName="column">
                                    <HeaderStyle Width="80px" />
                                </radg:GridHyperLinkColumn>
                            </Columns>
                            <ExpandCollapseColumn Visible="False" Resizable="False">
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <RowIndicatorColumn Visible="False">
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                        </MasterTableView>
                        <FilterItemStyle CssClass="GridHeader_Default" Font-Size="12px" HorizontalAlign="Center" />
                        <SortingSettings SortedAscToolTip="升序" SortedDescToolTip="降序" SortToolTip="点这里按此项排序" />
                        <HierarchySettings CollapseTooltip="折叠" ExpandTooltip="展开" />
                        <ExportSettings>
                            <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                                PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
                        </ExportSettings>
                    </radg:RadGrid></td>
            </tr>
        </table>
        </div>
        <asp:SqlDataSource ID="HR_EmployeesDS" runat="server" ConnectionString="<%$ ConnectionStrings:FMOPConn %>"
            SelectCommand="select Number,Employee_Name,GWMC,GWSMSBH from HR_Employees where JobNo=@JobNo and ygzt<>'离职'">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="" Name="JobNO" QueryStringField="JobNo" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>