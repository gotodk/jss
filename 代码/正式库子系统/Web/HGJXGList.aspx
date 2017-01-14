<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HGJXGList.aspx.cs" Inherits="Web_HGJXGList" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>合格旧硒鼓列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
</head>
<script src="../js/jquery.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="5" cellspacing="0" class="FormView" width="680">
        <tr>
            <td height="25" width="50%">
              回收鼓型号：<asp:TextBox ID="txtKGGG" runat="server" CssClass="input1"></asp:TextBox>
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
     <radg:radgrid id="RadGrid1" runat="server" allowpaging="True" gridlines="None" skin="Monochrome" 
        PageSize="10" OnPageIndexChanged="RadGrid1_PageIndexChanged" >
        <HeaderStyle Height="28px"></HeaderStyle>
                <ExportSettings>
                <Pdf PageWidth="8.5in" PageRightMargin="" PageFooterMargin="" PageLeftMargin="" PageTopMargin="" PageHeight="11in" PageBottomMargin="" PageHeaderMargin=""></Pdf>
                </ExportSettings>
                <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条." PrevPageText="上一页"></PagerStyle>
                <MasterTableView AutoGenerateColumns="False" >
                <NoRecordsTemplate>
                  没有找到任何数据。             
                </NoRecordsTemplate>
                <ExpandCollapseColumn Visible="False" Resizable="False">
                <HeaderStyle Width="20px"></HeaderStyle>
                </ExpandCollapseColumn>
                <RowIndicatorColumn Visible="False">
                <HeaderStyle Width="20px"></HeaderStyle>
                </RowIndicatorColumn>
                    <Columns>
                         <radg:GridTemplateColumn UniqueName="TemplateColumn" AllowFiltering="False">
                        <ItemTemplate>
                           <a href='#' onclick="setValue('<%#Eval("空鼓规格")%>');"><img  alt=""  src="../images/jpg/Update.gif" /></a>
                        </ItemTemplate>
                        <HeaderTemplate>
                            选择操作
                        </HeaderTemplate>
                             <ItemStyle CssClass="selectButton" />
                       </radg:GridTemplateColumn>
                        <radg:GridBoundColumn DataField="客户编号" HeaderText="客户编号" ReadOnly="True" SortExpression="客户编号"
                            UniqueName="客户编号">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="空鼓类型" HeaderText="空鼓类型" SortExpression="空鼓类型" UniqueName="空鼓类型">
                        </radg:GridBoundColumn>
                      <radg:GridBoundColumn DataField="空鼓规格" HeaderText="空鼓规格" SortExpression="空鼓规格" UniqueName="空鼓规格">
                        </radg:GridBoundColumn>  
                       <radg:GridBoundColumn DataField="剩余总量" HeaderText="剩余总量" SortExpression="剩余总量" UniqueName="剩余总量">
                        </radg:GridBoundColumn>
                    </Columns>
                </MasterTableView>
    </radg:radgrid>
    </form>
    <script type="text/javascript">
        function setValue(xh) {
            if (parent.opener.document.getElementById("ZRXGXH") != null) {
                parent.opener.document.getElementById("ZRXGXH").value = xh;
            }
           
            window.close();
            
        }
    </script>
</body>
</html>
