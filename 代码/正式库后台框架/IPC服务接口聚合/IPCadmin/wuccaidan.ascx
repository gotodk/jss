<%@ Control Language="C#" AutoEventWireup="true" CodeFile="wuccaidan.ascx.cs" Inherits="IPCadmin_wuccaidan" %>
    <script type="text/javascript" src="/IPCadmin/jquery-1.11.1.js" ></script>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button1" runat="server" Text="查看当前接口关系" OnClick="Button1_Click" Width="124px" /></td>
        <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button5" runat="server" Text="添加接口调用关系" OnClick="Button5_Click" Width="123px" /></td>
        <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button2" runat="server" Text="查看所有接口" OnClick="Button2_Click1" Width="104px" /></td>
        <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button7" runat="server" Text="查看所有方法" OnClick="Button7_Click1" Width="99px" /></td>
        <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button4" runat="server" Text="添加新接口和方法" OnClick="Button4_Click" Width="119px" /></td>
        <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button6" runat="server" Text="管理负载IP地址" OnClick="Button6_Click1" Width="110px" /></td>
        <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button3" runat="server" Text="通知接口更新和编译代理类" OnClick="Button3_Click" Width="182px" /></td>
        <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button8" runat="server" Text="接口综合状态检测" OnClick="Button8_Click" /></td>
    </tr>
    <tr>
        <td align="left" valign="middle">&nbsp;</td>
        <td align="left" valign="middle">&nbsp;</td>
        <td align="left" valign="middle">&nbsp;</td>
        <td align="left" valign="middle">&nbsp;</td>
        <td align="left" valign="middle">&nbsp;</td>
        <td align="left" valign="middle">&nbsp;</td>
        <td align="left" valign="middle">&nbsp;</td>
    </tr>
</table>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td align="left" valign="middle">web.config中配置的对应聚合中心地址：</td>
        <td align="left" valign="middle"><span id="dizhi" runat="server"></span></td>
    </tr>
    </table>