<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editJK.aspx.cs" Inherits="IPCadmin_editJK" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>IPC管理中心--修改接口资料</title>
    <link href="css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height: 45px; font-weight: bold; width: 630px" valign="bottom" align="left">修改接口信息：</td>
                </tr>
                <tr>
                    <td>
                        <table border='0' cellpadding='5' cellspacing='1' bgcolor='#333333'>
                            <tr>
                                <td bgcolor='#FFFFFF' style="width: 120px"><strong>接口guid：</strong></td>
                                <td bgcolor='#FFFFFF' style="width: 500px">
                                    <asp:Label ID="lblJKgudi" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'><strong>负载IP数量：</strong></td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:Label ID="lblFZIPSL" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'><strong>接口当前状态：</strong></td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:Label ID="lblJKZT" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'><strong>接口域名：</strong></td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:TextBox ID="txtJKYM" runat="server" Width="500px"></asp:TextBox></td>
                                <asp:Label ID="lblJKYM" runat="server" Visible="false" Text=""></asp:Label>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'><strong>接口地址：</strong></td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:TextBox ID="txtJKDZ" runat="server" Width="500px"></asp:TextBox>
                                    <asp:Label ID="lblJKDZ" runat="server" Visible="false" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'><strong>接口说明：</strong></td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:TextBox ID="txtJKSM" runat="server" Width="500px" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                    <asp:Label ID="lblJKSM" runat="server" Visible="false" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'><strong>接口版本号：</strong></td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:TextBox ID="txtJKBB" runat="server" Width="500px"></asp:TextBox>
                                    <asp:Label ID="lblJKBB" runat="server" Visible="false" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'><strong>接口是否有效：</strong></td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:ListBox ID="lbJKYX" runat="server" Rows="1">
                                        <asp:ListItem Selected="True" Value="1">有效</asp:ListItem>
                                        <asp:ListItem Value="0">禁用</asp:ListItem>
                                    </asp:ListBox></td>
                                <asp:Label ID="lblJKYX" runat="server" Visible="false" Text=""></asp:Label>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'><strong>备用端口：</strong></td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:TextBox ID="txtBYDK" runat="server" Width="500px"></asp:TextBox>
                                    <asp:Label ID="lblBYDK" runat="server" Visible="false" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 80px" valign="middle" align="center">
                        <asp:Button ID="btn_EditJK" runat="server" Text="确认修改接口信息" Height="30px" Width="150px" OnClick="btn_EditJK_Click" />&nbsp;&nbsp;
             <asp:Button ID="btn_Back" runat="server" Text="返回接口详情" Height="30px" Width="150px" OnClick="btn_Back_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

