<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Employee_List_LZ.aspx.cs" Inherits="Web_HR_Employee_List_LZ" %>

<%@ Register assembly="RadTabStrip.Net2" namespace="Telerik.WebControls" tagprefix="radTS" %>

<%@ Register assembly="RadGrid.Net2" namespace="Telerik.WebControls" tagprefix="radG" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>离职人员信息查询</title>
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
            height: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <radTS:RadTabStrip ID="RadTabStrip2" runat="server" Skin="Default2006">
     <Tabs>
        <radTS:Tab ID="Tab1" runat="server" Text="离职人员信息查询">
            </radTS:Tab>
     </Tabs>
     </radTS:RadTabStrip>
      <table border="0" cellpadding="0" cellspacing="0" style="width: 90%">
            <tr>            
                <td style="width: 100%" align="right">
                  <a href ="../HR/YGXX_Export_LZ.aspx">进入离职人员信息导出页面</a>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 排序：<asp:DropDownList ID="DropDownList2" runat="server">
                        <asp:ListItem Selected="True" Value="Number">员工编号</asp:ListItem>
                        <asp:ListItem Value="Employee_Name">姓名</asp:ListItem>
                        <asp:ListItem Value="Employee_Sex">性别</asp:ListItem>
                        <asp:ListItem Value="LS">隶属</asp:ListItem>
                        <asp:ListItem Value="BM">部门</asp:ListItem>
                        <asp:ListItem Value="GWMC">岗位名称</asp:ListItem>                    
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    姓名：<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 部门：<asp:DropDownList
                        ID="DropDownList1" runat="server">
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                   
                    <asp:ImageButton ID="ImageButton1" runat="server" Height="18px" ImageUrl="~/Web/images/20070927165819838.gif"
                        Width="59px" OnClick="ImageButton1_Click" /></td>
            </tr>
            <tr>
                <td align="left" class="style1">
                    <radG:RadGrid ID="RadGrid1" runat="server" GridLines="None" 
                        DataSourceID="SqlDataSource1">
                    <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条." PrevPageText="上一页"></PagerStyle>
<ExportSettings>
 
<Pdf PageWidth="8.5in" PageHeight="11in" PageTopMargin="" PageBottomMargin="" PageLeftMargin="" PageRightMargin="" PageHeaderMargin="" PageFooterMargin=""></Pdf>
 
</ExportSettings>

<MasterTableView AutoGenerateColumns="False" DataKeyNames="Number" DataSourceID="SqlDataSource1" OnPageIndexChanged="RadGrid1_PageIndexChanged" >
<RowIndicatorColumn Visible="False">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="False" Resizable="False">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>
 <NoRecordsTemplate>
                     没有找到任何数据
 </NoRecordsTemplate>
    <Columns>
        <radG:GridBoundColumn DataField="Number" HeaderText="员工编号" UniqueName="Number">
         </radG:GridBoundColumn>
         <radG:GridBoundColumn DataField="Employee_Name" HeaderText="姓名" UniqueName="Employee_Name">
         </radG:GridBoundColumn>
        <radG:GridBoundColumn DataField="Employee_Sex" HeaderText="性别" UniqueName="Employee_Sex">
         </radG:GridBoundColumn>
        <radG:GridBoundColumn DataField="LS" HeaderText="隶属" UniqueName="LS">
        </radG:GridBoundColumn>
        <radG:GridBoundColumn DataField="BM" HeaderText="部门" UniqueName="BM">
        </radG:GridBoundColumn>
        <radG:GridBoundColumn DataField="GWMC" HeaderText="岗位名称"  UniqueName="GWMC">
        </radG:GridBoundColumn>
         <radg:GridTemplateColumn UniqueName="TemplateColumn" AllowFiltering="False">
                        <ItemTemplate>
                           <a href='Employees_List_Rusult_LZ.aspx?Number=<%# DataBinder.Eval(Container, "DataItem.Number") %>' >  <img  src="../../images/a.gif"/></a>
                        </ItemTemplate>
                        <HeaderTemplate>
                            查看详情
                        </HeaderTemplate>
         </radg:GridTemplateColumn>
    </Columns>
</MasterTableView>
                    </radG:RadGrid>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 100%">
                    </td>
            </tr>
            </table>
     <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
         ConnectionString="<%$ ConnectionStrings:FMOPConn %>" SelectCommand="SELECT Number , Employee_Name, Employee_Sex , LS , BM, GWMC  FROM HR_Employees 
WHERE 1!=1"></asp:SqlDataSource>
    </form>
</body>
</html>
