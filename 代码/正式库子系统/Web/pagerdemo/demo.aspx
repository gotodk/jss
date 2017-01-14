<%@ Page Language="C#" AutoEventWireup="true" CodeFile="demo.aspx.cs" Inherits="Web_pagerdemo_demo" %>

<%@ Register src="commonpager.ascx" tagname="commonpager" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GV_show" runat="server">
        </asp:GridView>
        <uc1:commonpager ID="commonpager1" runat="server" />
        <asp:TextBox ID="TextBox1" runat="server">1834</asp:TextBox>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="加载测试数据" />
    
    </div>
    </form>
</body>
</html>
