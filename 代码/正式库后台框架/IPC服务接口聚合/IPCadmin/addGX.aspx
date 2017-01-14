<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addGX.aspx.cs" Inherits="IPCadmin_addGX" %>

<%@ Register Src="wuccaidan.ascx" TagName="wuccaidan" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>IPC管理中心--添加接口调用关系</title>
    <link href="css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:wuccaidan ID="wuccaidan1" runat="server" />
            <br />
            <strong>添加接口调用关系：</strong>
            <br />
            <table border='0' cellpadding='5' cellspacing='1' bgcolor='#333333'>
                <tr>
                    <td bgcolor='#FFFFFF'>调用方识别标记:
                    </td>

                    <td bgcolor='#FFFFFF'>
                        <%--<asp:TextBox ID="GX_shibie" runat="server" Width="461px"></asp:TextBox>--%>
                        <asp:ListBox ID="GX_shibiel" runat="server" Rows="1">
                            <asp:ListItem Value="">请选择</asp:ListItem>
                            <asp:ListItem>测试版客户端</asp:ListItem>
                            <asp:ListItem>模拟版客户端</asp:ListItem>
                            <asp:ListItem>Server客户端专用中心</asp:ListItem>
                            <asp:ListItem>Server成交处理中心</asp:ListItem>
                            <asp:ListItem>Server成交履约中心</asp:ListItem>
                            <asp:ListItem>Server辅助业务中心</asp:ListItem>
                            <asp:ListItem>Server交易指令中心</asp:ListItem>
                            <asp:ListItem>Server数据分析中心</asp:ListItem>
                            <asp:ListItem>Server用户控制中心</asp:ListItem>
                            <asp:ListItem>Server系统控制中心</asp:ListItem>
                            <asp:ListItem>Server开放平台中心</asp:ListItem>
                            <asp:ListItem>Server银行合作中心</asp:ListItem>
                            <asp:ListItem>交易数据监控</asp:ListItem>
                            <asp:ListItem>各银行监控统一标记</asp:ListItem>
                            <asp:ListItem>旧版待移植</asp:ListItem>
                        </asp:ListBox>
                    </td>

                </tr>
                <tr>
                    <td bgcolor='#FFFFFF'>是否开启日志:
                    </td>
                    <td bgcolor='#FFFFFF'>
                        <asp:ListBox ID="GX_savelog" runat="server" Rows="1">
                            <asp:ListItem Value="0" Selected="True">禁用</asp:ListItem>
                            <asp:ListItem Value="1">开启</asp:ListItem>
                        </asp:ListBox>
                    </td>

                </tr>
                <tr>
                    <td bgcolor='#FFFFFF'>被调用接口唯一标示:
                    </td>
                    <td bgcolor='#FFFFFF'>
                        <asp:ListBox ID="GX_JK_guid" runat="server" Rows="1" AutoPostBack="True" OnSelectedIndexChanged="GX_JK_guid_SelectedIndexChanged"></asp:ListBox>
                    </td>

                </tr>
                <tr>
                    <td bgcolor='#FFFFFF'>被调用方法唯一标示:
                    </td>
                    <td bgcolor='#FFFFFF'>
                        <asp:ListBox ID="GX_FF_guid" runat="server" Rows="1"></asp:ListBox>
                    </td>

                </tr>
                <tr>
                    <td bgcolor='#FFFFFF'>调用方式:
                    </td>
                    <td bgcolor='#FFFFFF'>
                        <asp:ListBox ID="GX_type" runat="server" Rows="1">
                            <asp:ListItem Selected="True" Value="1">同步执行</asp:ListItem>
                            <asp:ListItem Value="0">异步执行</asp:ListItem>
                        </asp:ListBox>
                    </td>

                </tr>
                <tr>
                    <td bgcolor='#FFFFFF'>关系是否有效:
                    </td>
                    <td bgcolor='#FFFFFF'>
                        <asp:ListBox ID="GX_open" runat="server" Rows="1">
                            <asp:ListItem Selected="True" Value="1">有效</asp:ListItem>
                            <asp:ListItem Value="0">禁用</asp:ListItem>
                        </asp:ListBox>
                    </td>

                </tr>
            </table>
            <br />
            <asp:Button ID="Button1" runat="server" Text="保存关系" Width="381px" Height="28px" OnClick="Button1_Click" />
        </div>
    </form>
</body>

</html>
