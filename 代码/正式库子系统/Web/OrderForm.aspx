<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderForm.aspx.cs" Inherits="OrderForm" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body style="margin:0,0">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="0px" Skin="Default2006">
    <Tabs>
        <radTS:Tab runat="server" Text="新增">
        </radTS:Tab>
    </Tabs>
</radTS:RadTabStrip>
    <div id="gridDiv" runat ="server" visible=true> 
            <radG:RadGrid ID="saleGrid" runat="server" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" GridLines="None" GroupingSettings-CollapseTooltip="折叠"
                GroupingSettings-ExpandTooltip="展开" GroupingSettings-GroupContinuedFormatString="接上页"
                GroupingSettings-GroupContinuesFormatString="接下页..." GroupingSettings-GroupSplitDisplayFormat="共有{1}条记录，本页显示{0}条."
                GroupPanel-Text="可拖动此项至表格上方的区域进行分组" HierarchySettings-CollapseTooltip="折叠" HierarchySettings-ExpandTooltip="展开"
                MasterTableView-NoMasterRecordsText="没有可显示的记录" PagerStyle-CssClass="GridPager"
                PagerStyle-Height="20px" PagerStyle-Mode="NextPrev" PagerStyle-PagerTextFormat="{4} &nbsp;|&nbsp;&nbsp;&nbsp;{0}&nbsp;/&nbsp;{1}&nbsp;页;&nbsp;&nbsp;(&nbsp;{2}&nbsp;-{3}&nbsp;)&nbsp;条记录&nbsp;&nbsp;,共{5}条"
                PagerStyle-VerticalAlign="Top" Skin="Default" SortingSettings-SortedAscToolTip="升序"
                SortingSettings-SortedDescToolTip="降序" SortingSettings-SortToolTip="按此项排序" Width="800px" DataSourceID="SqlDataSource1" OnItemCommand="saleGrid_ItemCommand">
                <ExportSettings>
                    <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                        PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
                </ExportSettings>
                <PagerStyle CssClass="GridPager" Height="30px" Mode="NextPrevAndNumeric" PagerTextFormat="{4} &#160;|&#160;&#160;&#160;{0}&#160;/&#160;{1}&#160;页;&#160;&#160;(&#160;{2}&#160;-{3}&#160;)&#160;条记录&#160;&#160;,共{5}条"
                    VerticalAlign="Top" />
                <MasterTableView DataKeyNames="Number" DataSourceID="SqlDataSource1" NoMasterRecordsText="没有可显示的记录">
                    <ExpandCollapseColumn Resizable="False" Visible="False">
                        <HeaderStyle Width="20px" />
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    <Columns>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="Number" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblNumber" runat="server" Text='<%# Eval("Number") %>'></asp:Label>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                        <radG:GridBoundColumn DataField="CUST_ID" HeaderText="客户编号" SortExpression="CUST_ID"
                            UniqueName="CUST_ID">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn DataField="CUST_NAME" HeaderText="客户名称" SortExpression="CUST_NAME"
                            UniqueName="CUST_NAME">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn DataField="LINKMAN_NAME" HeaderText="联系人姓名" SortExpression="LINKMAN_NAME"
                            UniqueName="LINKMAN_NAME">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn DataField="LINKMAN_TEL" HeaderText="联系人电话" SortExpression="LINKMAN_TEL"
                            UniqueName="LINKMAN_TEL">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn DataField="RECEIVE_NAME" HeaderText="收货人姓名" SortExpression="RECEIVE_NAME"
                            UniqueName="RECEIVE_NAME">
                        </radG:GridBoundColumn>
                        <radG:GridBoundColumn DataField="RECEIVE_TEL" HeaderText="收货人电话" SortExpression="RECEIVE_TEL"
                            UniqueName="RECEIVE_TEL">
                        </radG:GridBoundColumn>
                        <radG:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemTemplate>
                                <asp:Button ID="btnStart" runat="server" Text="生成销售单" CommandName="create"/>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings AllowColumnsReorder="True">
                    <Selecting AllowRowSelect="True" />
                </ClientSettings>
                <HierarchySettings CollapseTooltip="折叠" ExpandTooltip="展开" />
                <GroupPanel Text="可拖动此项至表格上方的区域进行分组" Visible="True">
                </GroupPanel>
                <SortingSettings SortedAscToolTip="升序" SortedDescToolTip="降序" SortToolTip="按此项排序" />
                <GroupingSettings CollapseTooltip="折叠" ExpandTooltip="展开" GroupContinuedFormatString="接上页"
                    GroupContinuesFormatString="接下页..." GroupSplitDisplayFormat="共有{1}条记录，本页显示{0}条." />
            </radG:RadGrid>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FMOPConn %>"
            SelectCommand="SELECT Number,CUST_ID,CUST_NAME,LINKMAN_NAME,LINKMAN_TEL,RECEIVE_NAME,RECEIVE_TEL FROM GNKH_Order WHERE 1<>1">
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
