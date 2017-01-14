<%@ Page Language="C#" AutoEventWireup="true" CodeFile="allJKlist.aspx.cs" Inherits="IPCadmin_allJKlist" %>

<%@ Register Src="wuccaidan.ascx" TagName="wuccaidan" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css.css" type="text/css" rel="stylesheet" />
    <title>IPC管理中心--接口列表</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <uc1:wuccaidan ID="wuccaidan1" runat="server" />

            <br />

            <table border="0" cellpadding="0" cellspacing="0" width ="85%">
                <tr>
                    <td align="left" valign="middle">排序:</td>
                    <td align="left" valign="middle">
                        <asp:TextBox ID="TextBox1" runat="server" Width="350px">order by JK.JK_host asc,JK.JK_path asc,JK.JK_open asc, JK.JK_addtime desc</asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp; 条件:</td>
                    <td align="left" valign="middle">
                        <asp:TextBox ID="TextBox2" runat="server" Width="150px">where 1=1</asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;接口域名：</td>
                    <td>
                        <asp:TextBox ID="txt_jkym" runat="server" Width="120px"></asp:TextBox></td>   
                     <td>&nbsp;&nbsp;接口地址：</td>
                    <td>
                        <asp:TextBox ID="txt_jkdz" runat="server" Width="120px"></asp:TextBox></td>   
                    <td align="left" valign="middle">&nbsp;&nbsp;<asp:Button ID="btnView" runat="server" Text="查 询" OnClick="btnView_Click" />&nbsp;&nbsp;
                        <asp:Button ID="Button1" runat="server" Text="刷 新" OnClick="Button1_Click" /></td>   
                </tr>
            </table>

            <br />
            <strong>所有接口列表：</strong>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" HorizontalAlign="Left" OnRowDataBound="GridView1_RowDataBound" Width="2000px">
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>

                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "showJKinfo.aspx?JK_guid="+Eval("接口唯一标示")+"&temp=" %>' Text='查看详情' Target="_blank"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="接口唯一标示" HeaderText="接口唯一标示" Visible="False" />
                    <asp:BoundField DataField="接口域名" HeaderText="接口域名" />
                    <asp:BoundField DataField="负载IP数量" HeaderText="负载IP数量" />
                    <asp:BoundField DataField="接口地址" HeaderText="接口地址">
                        <ItemStyle Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="接口说明" HeaderText="接口说明" />
                    <asp:BoundField DataField="接口版本号" HeaderText="接口版本号" />
                    <asp:BoundField DataField="接口是否有效" HeaderText="接口是否有效" />
                    <asp:BoundField DataField="接口目前状态" HeaderText="接口目前状态" Visible="False" />
                    <asp:BoundField DataField="添加时间" HeaderText="添加时间" />
                    <asp:BoundField DataField="最后一次修改时间" HeaderText="最后一次修改时间" />
                    <asp:BoundField DataField="接口备用端口" HeaderText="接口备用端口" />
                    <asp:BoundField DataField="含有效方法数量" HeaderText="含有效方法数量" />
                    <asp:BoundField DataField="含禁用方法数量" HeaderText="含禁用方法数量" />
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
