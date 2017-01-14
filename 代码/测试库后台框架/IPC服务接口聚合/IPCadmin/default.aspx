<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="IPCadmin_default" %>

<%@ Register Src="wuccaidan.ascx" TagName="wuccaidan" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css.css" type="text/css" rel="stylesheet" />
    <title>IPC管理中心--接口关系列表</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <uc1:wuccaidan ID="wuccaidan1" runat="server" />

            <br />

            <table border="0" cellpadding="0" cellspacing="0" width="97%">
                <tr>
                    <td align="left" valign="middle">排序:</td>
                    <td align="left" valign="middle">
                        <asp:TextBox ID="TextBox1" runat="server" Width="250px">order by GX.GX_shibie asc,JK.JK_host asc,JK.JK_path asc</asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp; 条件:</td>
                    <td align="left" valign="middle">
                        <asp:TextBox ID="TextBox2" runat="server" Width="80px">where 1=1</asp:TextBox></td>
                    <td>&nbsp;&nbsp;调用方标识：</td>
                    <td>
                        <asp:TextBox ID="txt_dyfbs" runat="server" Width="120px"></asp:TextBox></td>
                    <td>&nbsp;&nbsp;接口地址：</td>
                    <td>
                        <asp:TextBox ID="txt_jkdz" runat="server" Width="120px"></asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;方法业务名：</td>
                    <td>
                        <asp:TextBox ID="txt_ywmc" runat="server" Width="120px"></asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;方法名：</td>
                    <td>
                        <asp:TextBox ID="txt_ffm" runat="server" Width="120px"></asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;<asp:Button ID="btnView" runat="server" Text="查 询" OnClick="btnView_Click" />&nbsp;&nbsp;
                        <asp:Button ID="Button1" runat="server" Text="刷 新" OnClick="Button1_Click" /></td>
                    <td align="left" valign="middle">&nbsp;</td>
                    <td align="left" valign="middle">&nbsp;</td>
                </tr>
            </table>

            <br />
            <strong>所有接口关系列表：</strong>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" HorizontalAlign="Left" OnRowDataBound="GridView1_RowDataBound" Width="2000px" Style="word-break: break-all; word-wrap: break-word">
                <Columns>
                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="70px">
                        <ItemTemplate>

                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "showGXinfo.aspx?GX_guid="+Eval("关系唯一标示")+"&temp="+Eval("调用方识别标记") %>' Text='查看详情' Target="_blank"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="关系唯一标示" HeaderText="关系唯一标示" Visible="False" />
                    <asp:BoundField DataField="调用方识别标记" HeaderText="调用方识别标记" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="是否开启日志" HeaderText="是否开启日志" ItemStyle-Width="90px" />
                    <asp:BoundField DataField="接口域名" HeaderText="接口域名" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="接口地址" HeaderText="接口地址" ItemStyle-Width="220px" />
                    <asp:BoundField DataField="接口是否有效" HeaderText="接口是否有效" ItemStyle-Width="90px" />
                    <asp:BoundField DataField="接口目前状态" HeaderText="接口目前状态" Visible="False" />
                    <asp:BoundField DataField="方法业务名称" HeaderText="方法业务名称" ItemStyle-Width="220px" />
                    <asp:BoundField DataField="方法名" HeaderText="方法名" ItemStyle-Width="250px" />
                    <asp:BoundField DataField="方法返回值类型" HeaderText="方法返回值类型" ItemStyle-Width="130px" />
                    <asp:BoundField DataField="方法参数类型" HeaderText="方法参数类型" ItemStyle-Width="250px" />
                    <asp:BoundField DataField="方法是否有效" HeaderText="方法是否有效" ItemStyle-Width="90px" />
                    <asp:BoundField DataField="方法目前状态" HeaderText="方法目前状态" Visible="False" />
                    <asp:BoundField DataField="方法操作特点" HeaderText="方法操作特点" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="关系调用方式" HeaderText="关系调用方式" ItemStyle-Width="90px" />
                    <asp:BoundField DataField="关系是否有效" HeaderText="关系是否有效" ItemStyle-Width="90px" />
                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                <RowStyle BackColor="White" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="Gray" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
