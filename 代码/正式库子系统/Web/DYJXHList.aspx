<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DYJXHList.aspx.cs" Inherits="Web_DYJXHList" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>打印机类型列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="5" cellspacing="0" class="FormView" width="680">
        <tr>
            <td height="25" width="50%">
                打印机品牌：<asp:TextBox ID="dyjpp" runat="server" CssClass="input1"></asp:TextBox>
            </td>            <td height="25" width="50%">
                打印机型号：<asp:TextBox ID="dyjxh" runat="server" MaxLength="10" 
                    CssClass="input1" Height="18px" Width="124px"></asp:TextBox>
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
        PageSize="10" OnPageIndexChanged="RadGrid1_PageIndexChanged" DataSourceID="SqlDataSource1">
        <HeaderStyle Height="28px"></HeaderStyle>
                <ExportSettings>
                <Pdf PageWidth="8.5in" PageRightMargin="" PageFooterMargin="" PageLeftMargin="" PageTopMargin="" PageHeight="11in" PageBottomMargin="" PageHeaderMargin=""></Pdf>
                </ExportSettings>
                <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条." PrevPageText="上一页"></PagerStyle>
                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1">
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
                           <a href='#' onclick="setValue('<%#Eval("DYJXH")%>');"><img  alt=""  src="../images/jpg/Update.gif" /></a>
                        </ItemTemplate>
                        <HeaderTemplate>
                            选择操作
                        </HeaderTemplate>
                             <ItemStyle CssClass="selectButton" />
                       </radg:GridTemplateColumn>
                        <radg:GridBoundColumn DataField="DYJPP" HeaderText="打印机品牌" ReadOnly="True" SortExpression="DYJPP" UniqueName="DYJPP">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="DYJXH" HeaderText="打印机型号" SortExpression="DYJXH" UniqueName="DYJXH">
                        </radg:GridBoundColumn>
                    </Columns>
                </MasterTableView>
    </radg:radgrid><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FMOPConn %>"
            SelectCommand="select id, dyjpp,dyjxh from web_products_SYDYJX "></asp:SqlDataSource>
    </form>
    <script type="text/javascript">
        function setValue(dyjxh) {
          
            if (parent.opener.document.getElementById("KHSYJX") != null) {
                parent.opener.document.getElementById("KHSYJX").value = dyjxh;
            }
            window.close();
            
        }
    </script>
</body>
</html>
