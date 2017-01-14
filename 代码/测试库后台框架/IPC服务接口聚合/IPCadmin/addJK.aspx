<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addJK.aspx.cs" Inherits="IPCadmin_addJK" %>

<%@ Register Src="wuccaidan.ascx" TagName="wuccaidan" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>IPC管理中心--添加接口和方法</title>
    <link href="css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:wuccaidan ID="wuccaidan1" runat="server" />
            <br />
            <strong>添加接口资料：</strong>
            <br />
            <table border='0' cellpadding='5' cellspacing='1' bgcolor='#333333'>
                <tr>
                    <td bgcolor='#FFFFFF'><strong>接口域名：</strong></td>
                    <td bgcolor='#FFFFFF'>
                        <%--<asp:TextBox ID="TextBox1" runat="server" Width="500px"></asp:TextBox>--%>
                        <div style="position: relative;" id="JK_host">
                            <span style="margin-left: 100px; width: 18px; overflow: hidden;">
                                <asp:DropDownList runat="server" Style="width: 194px; margin-left: -100px" onchange="this.parentNode.nextSibling.value=this.value" ID="JK_host_selt" name="JK_host_selt" OnSelectedIndexChanged="JK_host_selt_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </span>
                            <asp:TextBox ID="TextBox1" runat="server" Style="width: 170px; position: absolute; left: 0px;"></asp:TextBox>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td bgcolor='#FFFFFF'><strong>接口地址：</strong></td>
                    <td bgcolor='#FFFFFF'>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <div style="position: relative;" id="JK_path">
                                        <span style="margin-left: 100px; width: 18px; overflow: hidden;">
                                            <asp:DropDownList runat="server" Style="width: 194px; margin-left: -100px" onchange="this.parentNode.nextSibling.value=this.value" ID="JK_path_selt" name="JK_path_selt"></asp:DropDownList>
                                        </span>
                                        <asp:TextBox ID="TextBox2" runat="server" Style="width: 170px; position: absolute; left: 0px;"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    &nbsp;&nbsp;<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="检查" />
                                </td>
                            </tr>

                        </table>
                        <%--<asp:TextBox ID="TextBox2" runat="server" Width="500px"></asp:TextBox>--%>
                       
                       
                    </td>
                </tr>
                <tr>
                    <td bgcolor='#FFFFFF'><strong>接口说明：</strong></td>
                    <td bgcolor='#FFFFFF'>
                        <asp:TextBox ID="TextBox3" runat="server" Width="500px" Rows="5" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
                <tr>
                    <td bgcolor='#FFFFFF'><strong>接口版本号：</strong></td>
                    <td bgcolor='#FFFFFF'>
                        <asp:TextBox ID="TextBox4" runat="server" Width="500px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td bgcolor='#FFFFFF'><strong>接口是否有效：</strong></td>
                    <td bgcolor='#FFFFFF'>
                        <asp:ListBox ID="ListBox1" runat="server" Rows="1">
                            <asp:ListItem Selected="True" Value="1">有效</asp:ListItem>
                            <asp:ListItem Value="0">禁用</asp:ListItem>
                        </asp:ListBox></td>
                </tr>
                <tr>
                    <td bgcolor='#FFFFFF'><strong>备用端口：</strong></td>
                    <td bgcolor='#FFFFFF'>
                        <asp:TextBox ID="TextBox5" runat="server" Width="500px"></asp:TextBox></td>
                </tr>
            </table>
            <br />
            <asp:Button ID="Button2" runat="server" Text="确认添加(接口和被选中的内部方法同时添加)" Height="30px" OnClick="Button2_Click" Width="660px" />
            <br />
            <br />
             <asp:Label ID="lblerrmsg" runat="server" Text="" Visible="false"></asp:Label>
            <br />
            <asp:Label ID="lblJKguid" runat="server" Text="" Visible="false"></asp:Label>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <strong>接口内提供的尚未添加的方法资料：</strong>
                    </td>
                    <td>
                        <asp:Button ID="btnAddM" runat="server" Text="确认添加选中的方法" Height="30px" OnClick="btnAddM_Click" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblFF" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                    </td>

                </tr>
            </table>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" HorizontalAlign="Left" OnRowDataBound="GridView1_RowDataBound" Style="margin-top: 0px">
                <Columns>
                    <asp:TemplateField HeaderText="选择">
                        <HeaderTemplate>
                            <asp:CheckBox ID="FF_chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="FF_chkAll_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="FF_chkItem" runat="server" />

                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="业务名称">

                        <ItemTemplate>
                            <asp:TextBox ID="FF_yewuname" runat="server" Text='<%# Eval("业务名称") %>'></asp:TextBox>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="方法名">

                        <ItemTemplate>
                            <asp:TextBox ID="FF_name" runat="server" Text='<%# Eval("方法名") %>'></asp:TextBox>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="返回值类型">
                        <ItemTemplate>
                            <asp:TextBox ID="FF_retype" runat="server" Text='<%# Eval("返回值类型") %>' Width="80px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="参数类型">
                        <ItemTemplate>
                            <asp:TextBox ID="FF_canshu" runat="server" Text='<%# Eval("参数类型") %>' Width="250px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="方法注释">
                        <ItemTemplate>
                            <asp:TextBox ID="FF_shuoming" runat="server" Text='<%# Eval("方法注释") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="方法是否有效">
                        <ItemTemplate>
                            <asp:ListBox ID="FF_open" runat="server" Rows="1">
                                <asp:ListItem Selected="True" Value="1">有效</asp:ListItem>
                                <asp:ListItem Value="0">禁用</asp:ListItem>
                            </asp:ListBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作特点">
                        <ItemTemplate>
                            <asp:ListBox ID="FF_CorE" runat="server" Rows="1">
                                <asp:ListItem Selected="True" Value="0">仅查询操作</asp:ListItem>
                                <asp:ListItem Value="1">有插入或更新操作</asp:ListItem>
                            </asp:ListBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="日志设置">
                        <ItemTemplate>
                            <asp:CheckBoxList ID="FF_Log" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem>程序</asp:ListItem>
                                <asp:ListItem>业务</asp:ListItem>
                                <asp:ListItem>其他</asp:ListItem>
                            </asp:CheckBoxList>
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
