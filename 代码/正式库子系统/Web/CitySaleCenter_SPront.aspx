<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CitySaleCenter_SPront.aspx.cs" Inherits="Web_CitySaleCenter_SPront" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>要货申请单 
列表</title>
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" Text="要货申请单打印列表">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
    <table border="0" cellpadding="5" cellspacing="0" class="FormView" width="680">
        <tr>
            <td height="25" width="50%">
                要货申请单号：<asp:TextBox ID="TXTYHSQDH" runat="server" CssClass="input1"></asp:TextBox>
            </td>            <td height="25" width="50%">
                城市销售公司：<asp:DropDownList ID="DDlSaleCity" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
              <tr>
                  <td align="right" colspan="2">
                      <asp:Button ID="EditButton" runat="server" CssClass="Button"
                          Text="搜索" OnClick="EditButton_Click" Width="40px" />
                      </td>
              </tr>
          </table>
     <radg:radgrid id="RadGrid1" runat="server" allowpaging="True" gridlines="None" skin="Monochrome" 
        PageSize="15"  OnPageIndexChanged="RadGrid1_PageIndexChanged">
        <HeaderStyle Height="28px"></HeaderStyle>
                <ExportSettings>
                <Pdf PageWidth="8.5in" PageRightMargin="" PageFooterMargin="" PageLeftMargin="" PageTopMargin="" PageHeight="11in" PageBottomMargin="" PageHeaderMargin=""></Pdf>
                </ExportSettings>
                <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条." PrevPageText="上一页"></PagerStyle>
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="Number" 
           >
                <NoRecordsTemplate>
                  没有找到任何数据。             
                </NoRecordsTemplate>
                <Columns>
                <radG:GridBoundColumn DataField="Number" UniqueName="Number" 
                        SortExpression="Number" HeaderText="编号" ReadOnly="True"></radG:GridBoundColumn>
                <radG:GridBoundColumn DataField="YHSQDH" UniqueName="Number" 
                        SortExpression="Number" HeaderText="要货申请单号" ReadOnly="True"></radG:GridBoundColumn>
                <radG:GridBoundColumn DataField="CSXSGS" UniqueName="KHBH" SortExpression="KHBH" 
                        HeaderText="城市销售公司名称"></radG:GridBoundColumn>
               <radG:GridBoundColumn DataField="CreateUser" UniqueName="KHBH" SortExpression="KHBH" 
                        HeaderText="创建人"></radG:GridBoundColumn>
                <radG:GridBoundColumn DataField="SHDZ" UniqueName="KHMC" HeaderText="收货地址" 
                        SortExpression="KHMC"></radG:GridBoundColumn>
                <radG:GridBoundColumn DataField="SHR" UniqueName="LXR" SortExpression="LXR" 
                        HeaderText="收货人"></radG:GridBoundColumn>
                <radG:GridBoundColumn DataField="LXDH" UniqueName="LXDH" SortExpression="LXDH" 
                        HeaderText="联系电话"></radG:GridBoundColumn>
                <radG:GridBoundColumn DataField="YQDHRQ" UniqueName="CSZX" SortExpression="CSZX" 
                        HeaderText="要求到货日期"></radG:GridBoundColumn>
                <radg:GridTemplateColumn UniqueName="TemplateColumn" AllowFiltering="False">
                        <ItemTemplate>
                           <a href='../web/MySJBB/YHSQDrEPORT.aspx?number=<%# DataBinder.Eval(Container, "DataItem.Number") %>'><img  alt=""  src="../images/jpg/Update.gif" /></a>
                        </ItemTemplate>
                        <HeaderTemplate>
                            打印
                        </HeaderTemplate>
                    </radg:GridTemplateColumn>
                </Columns>
                <ExpandCollapseColumn Visible="False" Resizable="False">
                <HeaderStyle Width="20px"></HeaderStyle>
                </ExpandCollapseColumn>
                <RowIndicatorColumn Visible="False">
                <HeaderStyle Width="20px"></HeaderStyle>
                </RowIndicatorColumn>
                </MasterTableView>
    </radg:radgrid>
    
    </form>
</body>
</html>
