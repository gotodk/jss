<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetUser.aspx.cs" Inherits="Web_XXHZX_ResetUser" %>
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
                    NavigateUrl="ResetUser.aspx" Text="员工帐号冻结">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
         <table border="0" cellpadding="5" cellspacing="0" class="FormView" width="680">
        <tr>
            <td height="25" width="50%">
                员工工号：<asp:TextBox ID="yggh" runat="server" CssClass="input1"></asp:TextBox>
            </td>            <td height="25" width="50%">
                员工姓名：<asp:TextBox ID="ygxm" runat="server" MaxLength="10" 
                    CssClass="input1" Height="18px" Width="124px"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td height="25" width="50%">
                帐号状态：<asp:ListBox ID="ListBox1" runat="server" Rows="1">
                    <asp:ListItem Selected="True">不限</asp:ListItem>
                    <asp:ListItem>已冻结</asp:ListItem>
                    <asp:ListItem>未冻结</asp:ListItem>
                </asp:ListBox>
            </td>            <td height="25" width="50%">
               
            </td>
        </tr>
              <tr>
                  <td align="right" colspan="2">
                      <asp:Button ID="EditButton" runat="server" CssClass="Button"
                          Text="搜索" OnClick="EditButton_Click" Width="40px" />
                      </td>
              </tr>
          </table>
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
                    <radg:GridBoundColumn DataField="BM" HeaderText="部门" SortExpression="BM"
                        UniqueName="BM">
                    </radg:GridBoundColumn>
                    <radg:GridBoundColumn DataField="GWMC" HeaderText="岗位名称" SortExpression="GWMC"
                        UniqueName="GWMC">
                    </radg:GridBoundColumn>
                     <radg:GridBoundColumn DataField="YGZT" HeaderText="员工状态" SortExpression="YGZT"
                        UniqueName="YGZT">
                    </radg:GridBoundColumn>
                    <radg:GridBoundColumn DataField="冻结状态" HeaderText="冻结状态" SortExpression="冻结状态"
                        UniqueName="冻结状态">
                    </radg:GridBoundColumn>    
                    <radg:GridTemplateColumn UniqueName="TemplateColumn">
                        <ItemTemplate>
                        <a href='resetpwd.aspx?number=<%# DataBinder.Eval(Container, "DataItem.Number") %>&dj=yes'><img alt=""  src="../images/Update.gif"/></a>
                        </ItemTemplate>
                        <HeaderTemplate>
                            冻结帐号
                        </HeaderTemplate>
                    </radg:GridTemplateColumn>   
                    <radg:GridTemplateColumn UniqueName="TemplateColumn">
                        <ItemTemplate>
                        <a href='resetpwd.aspx?number=<%# DataBinder.Eval(Container, "DataItem.Number") %>&dj=no'><img alt=""  src="../images/Update.gif"/></a>
                        </ItemTemplate>
                        <HeaderTemplate>
                            解除冻结
                        </HeaderTemplate>
                    </radg:GridTemplateColumn>      
                    <radg:GridBoundColumn DataField="冻结人工号" HeaderText="冻结人工号" SortExpression="冻结人工号"
                        UniqueName="冻结人工号">
                    </radg:GridBoundColumn>    
                    
                    <radg:GridBoundColumn DataField="冻结人姓名" HeaderText="冻结人姓名" SortExpression="冻结人姓名"
                        UniqueName="冻结人姓名">
                    </radg:GridBoundColumn>  
                    <radg:GridBoundColumn DataField="冻结时间" HeaderText="冻结时间" SortExpression="冻结时间"
                        UniqueName="冻结时间">
                    </radg:GridBoundColumn>  
                    <radg:GridBoundColumn DataField="解冻人工号" HeaderText="解冻人工号" SortExpression="解冻人工号"
                        UniqueName="最后解冻人工号">
                    </radg:GridBoundColumn>  
                    <radg:GridBoundColumn DataField="解冻人姓名" HeaderText="解冻人姓名" SortExpression="解冻人姓名"
                        UniqueName="解冻人姓名">
                    </radg:GridBoundColumn>  
                    <radg:GridBoundColumn DataField="解冻操时间" HeaderText="解冻操时间" SortExpression="解冻操时间"
                        UniqueName="解冻操时间">
                    </radg:GridBoundColumn>  
                   
                </Columns>
            </MasterTableView>
            <ExportSettings>
                <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                    PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
            </ExportSettings>
        </radg:RadGrid>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FMOPConn %>"
            SelectCommand="SELECT  e.Number, e.BM,  e.GWMC,e.Employee_Name,e.YGZT, (CASE IsApproved WHEN 1 THEN '未冻结' WHEN 0 THEN '已冻结' ELSE  '未知' END) as 冻结状态,'冻结人工号'=(select top 1 PRL.DJ_hg FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '冻结' order by PRL.DJ_time DESC),'冻结人姓名'=(select top 1 PRL.DJ_name FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '冻结' order by PRL.DJ_time DESC),  '冻结时间'=( select top 1 PRL.DJ_time FROM PassReLog PRL   where PRL.OP_gh = e.Number and PRL.OP_type = '冻结'  order by PRL.DJ_time DESC), '解冻人工号'=(select top 1 PRL.UDJ_hg FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '解冻'  order by PRL.UDJ_time DESC ),  '解冻人姓名'=( select top 1 PRL.UDJ_name FROM PassReLog PRL  where PRL.OP_gh = e.Number and PRL.OP_type = '解冻'  order by PRL.UDJ_time DESC ),  '解冻操时间'=( select top 1 PRL.UDJ_time FROM PassReLog PRL   where PRL.OP_gh = e.Number and PRL.OP_type = '解冻'    order by PRL.UDJ_time DESC )  FROM HR_Employees AS e INNER JOIN aspnet_Users AS u ON e.Number = u.UserName INNER JOIN aspnet_Membership AS m ON  u.UserId = m.UserId where(e.ygzt<>'离职') and IsApproved = 0 order by e.Number">
        </asp:SqlDataSource>
    </form>
</body>
</html>
