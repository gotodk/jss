<%@ Page Language="C#" AutoEventWireup="true" CodeFile="allFFlist.aspx.cs" Inherits="IPCadmin_allFFlist" %>

<%@ Register Src="wuccaidan.ascx" TagName="wuccaidan" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css.css" type="text/css" rel="stylesheet" />
    <title>IPC管理中心--方法列表</title>
    <script type="text/javascript">
        function changecolor(obj) {
            var p = obj.parentNode;
            for (i = 0; i < p.rows.length; i++) {
                p.rows[i].style.backgroundColor = "white";
            }
            obj.style.backgroundColor = "red";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <uc1:wuccaidan ID="wuccaidan1" runat="server" />

            <br />

            <table border="0" cellpadding="0" cellspacing="0" width="90%">
                <tr>
                    <td align="left" valign="middle">排序:</td>
                    <td align="left" valign="middle">
                        <asp:TextBox ID="TextBox1" runat="server" Width="350px">order by JK.JK_host asc,JK.JK_path asc,FF.FF_open asc, FF.FF_addtime desc</asp:TextBox></td>
                    <td>&nbsp;&nbsp;条件</td>
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server" Width="100px">where 1=1</asp:TextBox>
                    </td>
                    <td>&nbsp;&nbsp;接口地址：</td>
                    <td>
                        <asp:TextBox ID="txt_jkdz" runat="server" Width="120px"></asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;业务名称：</td>
                    <td>
                        <asp:TextBox ID="txt_ywmc" runat="server" Width="120px"></asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;方法名：</td>
                    <td>
                        <asp:TextBox ID="txt_ffm" runat="server" Width="120px"></asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;
                        <asp:Button ID="btnView" runat="server" Text="查 询" OnClick="btnView_Click" />&nbsp;&nbsp;
                        <asp:Button ID="Button1" runat="server" Text="刷 新" OnClick="Button1_Click" /></td>
                </tr>
            </table>

            <br />
            <strong>所有方法列表：</strong>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" HorizontalAlign="Left" OnRowDataBound="GridView1_RowDataBound" Width="2000px" Style="word-break: break-all; word-wrap: break-word">
                <Columns>
                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="70px">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "showFFinfo.aspx?FF_guid="+Eval("方法唯一标示")+"&temp=" %>' Text='查看详情' Target="_blank"></asp:HyperLink>
                        </ItemTemplate>

                        <ItemStyle Width="70px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="接口地址" HeaderText="接口地址" ItemStyle-Width="200px">
                        <ItemStyle Width="200px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="业务名称" HeaderText="业务名称" ItemStyle-Width="200px">
                        <ItemStyle Width="200px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="方法名" HeaderText="方法名" ItemStyle-Width="250px">
                        <ItemStyle Width="250px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="返回值类型" HeaderText="返回值类型" ItemStyle-Width="150px">
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="参数类型" HeaderText="参数类型" ItemStyle-Width="300px">
                        <ItemStyle Width="300px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="方法是否有效" HeaderText="方法是否有效" ItemStyle-Width="150px">
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="操作特点" HeaderText="操作特点" ItemStyle-Width="130px">
                        <ItemStyle Width="130px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="日志设置" HeaderText="日志设置" ItemStyle-Width="100px">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="添加时间" HeaderText="添加时间" ItemStyle-Width="150px">
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="最后一次修改时间" HeaderText="最后修改时间" ItemStyle-Width="150px">
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="接口域名" HeaderText="接口域名" ItemStyle-Width="150px">
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                <RowStyle BackColor="White" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#9966FF" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="Gray" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
