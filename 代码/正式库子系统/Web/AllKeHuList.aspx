<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllKeHuList.aspx.cs" Inherits="Web_AllKeHuList" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>分公司客户信息列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="5" cellspacing="0" class="FormView" width="680">
        <tr>
            <td height="25" width="50%">
                客户编号：<asp:TextBox ID="fwsbh" runat="server" CssClass="input1"></asp:TextBox>
            </td>
            <td height="25" width="50%">
                客户名称：<asp:TextBox ID="fwsmc" runat="server" MaxLength="10" CssClass="input1" Height="18px"
                    Width="124px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2">
                <asp:Button ID="EditButton" runat="server" CssClass="Button" Text="搜索" OnClick="EditButton_Click"
                    Width="40px" />
            </td>
        </tr>
    </table>
    <radg:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GridLines="None" Skin="Monochrome"
        PageSize="10" OnPageIndexChanged="RadGrid1_PageIndexChanged">
        <HeaderStyle Height="28px"></HeaderStyle>
        <ExportSettings>
            <Pdf PageWidth="8.5in" PageRightMargin="" PageFooterMargin="" PageLeftMargin="" PageTopMargin=""
                PageHeight="11in" PageBottomMargin="" PageHeaderMargin=""></Pdf>
        </ExportSettings>
        <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条."
            PrevPageText="上一页"></PagerStyle>
        <MasterTableView AutoGenerateColumns="False" DataKeyNames="Number">
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
                        <a href='#' onclick="setValue('<%#Eval("number")%>','<%#Eval("KHMC")%>','<%#Eval("DZ")%>','<%#Eval("LXDH")%>');">
                            <img alt="" src="../images/jpg/Update.gif" /></a>
                    </ItemTemplate>
                    <HeaderTemplate>
                        选择操作
                    </HeaderTemplate>
                    <ItemStyle CssClass="selectButton" />
                </radg:GridTemplateColumn>
                <radg:GridBoundColumn DataField="Number" HeaderText="客户编号" ReadOnly="True" SortExpression="Number"
                    UniqueName="Number">
                </radg:GridBoundColumn>
                <radg:GridBoundColumn DataField="KHMC" HeaderText="客户名称" SortExpression="KHMC" UniqueName="KHMC">
                </radg:GridBoundColumn>
                <radg:GridBoundColumn DataField="DZ" HeaderText="地址" SortExpression="DZ" UniqueName="DZ">
                </radg:GridBoundColumn>
                <radg:GridBoundColumn DataField="LXDH" HeaderText="联系电话" SortExpression="LXDH" UniqueName="LXDH">
                </radg:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </radg:RadGrid>
    </form>
    <script type="text/javascript">
        function setValue(number, khmc, dz, lxdh) {
            if (parent.opener.document.getElementById("KHBH") != null) {
                parent.opener.document.getElementById("KHBH").value = number;
            }
            if (parent.opener.document.getElementById("KHMC") != null) {
                parent.opener.document.getElementById("KHMC").value = khmc;
            }
            window.close();

        }
    </script>
</body>
</html>
