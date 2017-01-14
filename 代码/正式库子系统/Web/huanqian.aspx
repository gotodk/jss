<%@ Page Language="C#" AutoEventWireup="true" CodeFile="huanqian.aspx.cs" Inherits="Web_huanqian" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:DropDownList ID="DropDownList1" runat="server">
            <asp:ListItem Selected="True">无操作</asp:ListItem>
            <asp:ListItem>更新换签编号</asp:ListItem>
            <asp:ListItem>其他操作</asp:ListItem>
        </asp:DropDownList>
&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="执行选定操作" 
            Width="115px" />
            <div style="display:none"> <asp:Label ID="Label1" runat="server" Text="Label" ></asp:Label></div>
       
    
    </div>
    </form>
</body>
</html>
