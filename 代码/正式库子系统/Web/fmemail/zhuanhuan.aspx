<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zhuanhuan.aspx.cs" Inherits="Web_fmemail_zhuanhuan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView1" runat="server" Height="133px" Width="912px">
        </asp:GridView>
    
    </div>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" 
        Width="108px" />
    </form>
</body>
</html>
