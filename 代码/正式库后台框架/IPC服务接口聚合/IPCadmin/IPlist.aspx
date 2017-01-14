<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IPlist.aspx.cs" Inherits="IPCadmin_IPlist" %>

<%@ Register Src="wuccaidan.ascx" TagName="wuccaidan" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css.css" type="text/css" rel="stylesheet" />
    <title>IPC管理中心--管理负载IP地址</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:wuccaidan ID="wuccaidan1" runat="server" />
            <br />
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left" valign="middle">排序:</td>
                    <td align="left" valign="middle">
                        <asp:TextBox ID="TextBox1" runat="server" Width="377px">order by IP.IP_JK_host asc,IP.IP_address asc, IP.IP_open asc</asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp; 条件:</td>
                    <td align="left" valign="middle">
                        <asp:TextBox ID="TextBox2" runat="server" Width="368px">where 1=1</asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;<asp:Button ID="btnView" runat="server" Text="查 询" OnClick="btnView_Click" />&nbsp;&nbsp;
                        <asp:Button ID="Button1" runat="server" Text="刷 新" OnClick="Button1_Click" /></td>
                    <td align="left" valign="middle">&nbsp;</td>
                    <td align="left" valign="middle">&nbsp;</td>
                </tr>
            </table>
            <br />
            <%-- <a href="javascript:alert('后续实现，先人工处理！');">新增数据</a>--%>
            <br />
            <div id="divAdd" runat="server" visible="true">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="center" style="width: 100px">
                            <strong>新增负载IP地址</strong>
                        </td>
                        <td style="height: 30px; width: 70px" align="right">接口域名：
                        </td>
                        <td align="left" style="width: 200px">
                            <asp:DropDownList ID="ddlJK" runat="server" Width="195px"></asp:DropDownList>
                        </td>
                        <td align="right" style="width: 60px">IP地址：
                        </td>
                        <td align="left" style="width: 100px">
                            <asp:TextBox ID="txtIP" runat="server"></asp:TextBox>
                        </td>
                        <td align="right" style="width: 90px">IP是否有效：</td>
                        <td align="left" style="width: 100px">
                            <asp:DropDownList ID="ddlIPYX" runat="server" Width="100px">
                                <asp:ListItem Value="">请选择</asp:ListItem>
                                <asp:ListItem Value="正常负载">正常负载</asp:ListItem>
                                <asp:ListItem Value="临时禁用">临时禁用</asp:ListItem>
                                <asp:ListItem Value="无效">无效</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right" style="width: 50px">备注：
                        </td>
                        <td align="left" style="width: 200px">
                            <asp:TextBox ID="txtBZ" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td align="center" style="width: 100px">
                            <asp:Button ID="btnAdd" runat="server" Text="添加" OnClick="btnAdd_Click" />&nbsp;&nbsp;<asp:Button ID="btnReset" runat="server" Text="重置" OnClick="btnReset_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" HorizontalAlign="Left" OnRowDataBound="GridView1_RowDataBound" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("IP_guid")%>' OnClick="btnEdit_Click" Text="编辑"></asp:LinkButton>
                            <asp:LinkButton ID="lkbSave" runat="server" CausesValidation="False" CommandArgument='<%# Eval("IP_guid") %>' OnClick="btnSave_Click" Text="保存" Visible="false"></asp:LinkButton>
                            <asp:LinkButton ID="lkbCancel" runat="server" CausesValidation="False" CommandArgument='<%# Eval("IP_guid") %>' OnClick="btnCancel_Click" Text="取消" Visible="false"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="接口域名">
                        <ItemTemplate>
                            <asp:Label ID="lbljk" runat="server" Text='<%#Eval("接口域名") %>'></asp:Label>
                            <asp:DropDownList ID="ddljk" runat="server" Visible="false"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IP地址">
                        <ItemTemplate>
                            <asp:Label ID="lblip" runat="server" Text='<%#Eval("IP地址") %>'></asp:Label>
                            <asp:TextBox ID="txtip" runat="server" Text='<%#Eval("IP地址") %>' Visible="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IP地址是否有效">
                        <ItemTemplate>
                            <asp:Label ID="lblipyx" runat="server" Text='<%#Eval("IP地址是否有效") %>'></asp:Label>
                            <asp:DropDownList ID="ddlipyx" runat="server" Visible="false">
                                <asp:ListItem Value="正常负载">正常负载</asp:ListItem>
                                <asp:ListItem Value="临时禁用">临时禁用</asp:ListItem>
                                <asp:ListItem Value="无效">无效</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="备注">
                        <ItemTemplate>
                            <asp:Label ID="lblbz" runat="server" Text='<%#Eval("备注") %>'></asp:Label>
                            <asp:TextBox ID="txtbz" runat="server" Text='<%#Eval("备注") %>' Visible="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="接口备用端口">
                        <ItemTemplate>
                            <asp:Label ID="lbljkbydk" runat="server" Text='<%#Eval("接口备用端口") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="接口有效性">
                        <ItemTemplate>
                            <asp:Label ID="lbljkzt" runat="server" Text='<%#Eval("接口有效性") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IP地址状态">
                        <ItemTemplate>
                            <asp:Label ID="lblipzt" runat="server" Text='<%#Eval("IP地址状态") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="添加时间">
                        <ItemTemplate>
                            <asp:Label ID="lblipaddtime" runat="server" Text='<%#Eval("IP添加时间") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="最后修改时间">
                        <ItemTemplate>
                            <asp:Label ID="lblipedittime" runat="server" Text='<%#Eval("IP最后修改时间") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
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
