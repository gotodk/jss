<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AuthenticationList.aspx.cs" Inherits="Web_AuthenticationList" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>模块权限列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" /><style type="text/css">
<!--
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
-->
</style></head>
<body>
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" Text="模块权限列表">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <radG:RadGrid ID="RadGrid1" runat="server" GridLines="None" AutoGenerateColumns="False" OnPageIndexChanged="RadGrid1_PageIndexChanged" Skin="Default2006">
            <ExportSettings>
                <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                    PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
            </ExportSettings>
            <PagerStyle CssClass="GridPager" Height="30px" Mode="NextPrevAndNumeric" PagerTextFormat="{4} &#160;|&#160;&#160;&#160;{0}&#160;/&#160;{1}&#160;页;&#160;&#160;(&#160;{2}&#160;-{3}&#160;)&#160;条记录&#160;&#160;,共{5}条"
              VerticalAlign="Top" />
            <MasterTableView>
                <Columns>
                    <radG:GridBoundColumn DataField="Title"　 HeaderText="模块名称"　UniqueName="Title">
                    </radG:GridBoundColumn>
                     <radG:GridBoundColumn DataField="ModuleType"　 HeaderText="模块类型"　UniqueName="ModuleType">
                    </radG:GridBoundColumn>
                      <radG:GridBoundColumn DataField="roleName"　 HeaderText="权限名称"　UniqueName="roleName">
                    </radG:GridBoundColumn>
                      <radG:GridBoundColumn DataField="roleType"　 HeaderText="权限类别"　UniqueName="roleType">
                    </radG:GridBoundColumn>
                      <radG:GridBoundColumn DataField="canAdd"　 HeaderText="添加权限"　UniqueName="canAdd">
                    </radG:GridBoundColumn>
                      <radG:GridBoundColumn DataField="canView"　 HeaderText="查看权限"　UniqueName="canView">
                    </radG:GridBoundColumn>
                      <radG:GridBoundColumn DataField="canModify"　 HeaderText="修改权限"　UniqueName="canModify">
                    </radG:GridBoundColumn>
                      <radG:GridBoundColumn DataField="viewType"　 HeaderText="查看范围"　UniqueName="viewType">
                    </radG:GridBoundColumn>
                      <radG:GridBoundColumn DataField="modifyType"　 HeaderText="修改范围"　UniqueName="modifyType">
                    </radG:GridBoundColumn>
                </Columns>
                <ExpandCollapseColumn Resizable="False" Visible="False">
                    <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
            </MasterTableView>
         
        </radG:RadGrid><asp:Button ID="Button1" runat="server" Text="导出Excel文件" CssClass="button" OnClick="Button1_Click" Width="150px" />
                <asp:Button ID="Button2" runat="server" Text="导出Word文件" CssClass="button" OnClick="Button2_Click" Width="150px" />
                <asp:Button ID="Button3" runat="server" Text="导出 CSV文件" CssClass="button" OnClick="Button3_Click" Width="150px" />
    </form>
</body>
</html>
