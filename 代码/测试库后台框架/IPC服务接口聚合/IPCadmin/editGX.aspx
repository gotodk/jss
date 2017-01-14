<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editGX.aspx.cs" Inherits="IPCadmin_editGX" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>IPC管理中心--修改接口关系资料</title>
    <link href="css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <br />

            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td style="height: 40px" valign="bottom">
                        <strong>修改接口关系资料：</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border='0' cellpadding='5' cellspacing='1' bgcolor='#333333'>

                            <tr>
                                <td bgcolor='#FFFFFF'>关系唯一标示:
                                </td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:Label ID="lblGX_guid" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'>调用方识别标记:
                                </td>

                                <td bgcolor='#FFFFFF'>
                                    <asp:TextBox ID="GX_shibie" runat="server" Width="461px"></asp:TextBox>
                                    <asp:Label ID="lblGX_shibie" runat="server" Visible="false"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'>是否开启日志:
                                </td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:ListBox ID="GX_savelog" runat="server" Rows="1">
                                        <asp:ListItem Value="1">开启</asp:ListItem>
                                        <asp:ListItem Value="0">禁用</asp:ListItem>
                                    </asp:ListBox>
                                    <asp:Label ID="lblGX_savelog" runat="server" Visible="false"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'>被调用接口唯一标示:
                                </td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:ListBox ID="GX_JK_guid" runat="server" Rows="1" AutoPostBack="True" OnSelectedIndexChanged="GX_JK_guid_SelectedIndexChanged"></asp:ListBox>
                                    <asp:Label ID="lblGX_JK_guid" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'>被调用方法唯一标示:
                                </td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:ListBox ID="GX_FF_guid" runat="server" Rows="1"></asp:ListBox>
                                    <asp:Label ID="lblGX_FF_guid" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'>调用方式:
                                </td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:ListBox ID="GX_type" runat="server" Rows="1">
                                        <asp:ListItem Value="1">同步执行</asp:ListItem>
                                        <asp:ListItem Value="0">异步执行</asp:ListItem>
                                    </asp:ListBox>
                                    <asp:Label ID="lblGX_type" runat="server" Visible="false"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF'>关系是否有效:
                                </td>
                                <td bgcolor='#FFFFFF'>
                                    <asp:ListBox ID="GX_open" runat="server" Rows="1">
                                        <asp:ListItem Value="1">有效</asp:ListItem>
                                        <asp:ListItem Value="0">禁用</asp:ListItem>
                                    </asp:ListBox>
                                    <asp:Label ID="lblGX_open" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="height: 50px" valign="bottom">
                        <asp:Button ID="btnSave" runat="server" Text="保存修改" Width="150px" Height="28px" OnClick="btnSave_Click" />&nbsp;&nbsp;
            <asp:Button ID="btnBack" runat="server" Text="返回接口关系详情" Width="150px" Height="28px" OnClick="btnBack_Click" />
                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>

</html>
