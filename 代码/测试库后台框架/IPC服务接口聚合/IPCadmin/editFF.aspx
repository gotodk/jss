<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editFF.aspx.cs" Inherits="IPCadmin_editFF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>IPC管理中心--修改方法资料</title>
    <link href="css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td style="height: 45px; font-weight: bold; width: 630px" valign="bottom" align="left">修改方法信息：</td>
                </tr>
                <tr>
                    <td>
                        <table border='0' cellpadding='5' cellspacing='1' bgcolor='#333333'>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>接口域名：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px; line-height: 20px'>
                                    <asp:Label runat="server" ID="lblJK_host"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>接口地址：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px; line-height: 20px'>
                                    <asp:Label runat="server" ID="lblJK_path"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>方法唯一标示：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px; line-height: 20px'>
                                    <asp:Label runat="server" ID="lblFF_guid"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>业务名称：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px; line-height: 20px'>
                                    <asp:TextBox ID="FF_yewuname" runat="server" Width="400px"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblFF_yewuname" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>方法名：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px; line-height: 20px'>
                                    <asp:TextBox ID="FF_name" runat="server" Width="400px"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblFF_name" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>返回值类型：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px; line-height: 20px'>
                                    <asp:TextBox ID="FF_retype" runat="server" Width="400px"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblFF_retype" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>参数类型：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px; line-height: 20px'>
                                    <asp:TextBox ID="FF_canshu" runat="server" Width="400px"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblFF_canshu" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>方法注释：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px; line-height: 20px'>
                                    <asp:TextBox ID="FF_shuoming" runat="server" Width="450px" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblFF_shuoming" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>方法是否有效：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px; line-height: 20px'>
                                    <asp:ListBox ID="FF_open" runat="server" Rows="1">
                                        <asp:ListItem Value="1">有效</asp:ListItem>
                                        <asp:ListItem Value="0">禁用</asp:ListItem>
                                    </asp:ListBox>
                                    <asp:Label runat="server" ID="lblFF_open" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>操作特点：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px;'>
                                    <asp:ListBox ID="FF_CorE" runat="server" Rows="1">
                                        <asp:ListItem Value="0">仅查询操作</asp:ListItem>
                                        <asp:ListItem Value="1">有插入或更新操作</asp:ListItem>
                                    </asp:ListBox>
                                    <asp:Label runat="server" ID="lblFF_CorE" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>日志设置：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px; line-height: 20px'>
                                    <asp:CheckBoxList ID="FF_Log" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem>程序</asp:ListItem>
                                        <asp:ListItem>业务</asp:ListItem>
                                        <asp:ListItem>其他</asp:ListItem>
                                    </asp:CheckBoxList>
                                    <asp:Label runat="server" ID="lblFF_log" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='#FFFFFF' style='width: 130px'><strong>方法目前状态：</strong></td>
                                <td bgcolor='#FFFFFF' style='width: 500px;'>
                                    <asp:Label runat="server" ID="lblFF_zt"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>

                </tr>
                <tr>
                    <td style="height: 80px" valign="middle" align="center">
                        <asp:Button ID="btn_EditJK" runat="server" Text="确认修改方法信息" Height="30px" Width="150px" OnClick="btn_EditJK_Click" />&nbsp;&nbsp;
             <asp:Button ID="btn_Back" runat="server" Text="返回方法详情" Height="30px" Width="150px" OnClick="btn_Back_Click" />
                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
