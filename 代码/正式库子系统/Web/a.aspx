<%@ Page Language="C#" AutoEventWireup="true" CodeFile="a.aspx.cs" Inherits="Web_a" %>

<form id="form1" runat="server">
<p>
    确定同意该服务商的申请？<asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="确定" style="width: 40px" />
&nbsp;
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="取消" />
</p>
</form>
