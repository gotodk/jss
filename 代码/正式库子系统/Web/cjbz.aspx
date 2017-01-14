<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cjbz.aspx.cs" Inherits="Web_cjbz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link type="text/css" href="../css/style.css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>

    
    
    <table width="500" height="263" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="34" align="left" valign="middle" bgcolor="#F0FAFF" colspan="2">填写<asp:Label ID="mctxt" runat="server" Text="..........."></asp:Label>处理过程备注以及最终处理结果</td>
  </tr>
  <tr>
    <td height="108" align="left" valign="middle" colspan="2">
        <asp:TextBox ID="cljgbz" runat="server" Height="170px" Width="483px" 
            TextMode="MultiLine"></asp:TextBox>
        </td>
  </tr>
  <tr>
    <td height="35" align="left" valign="middle" bgcolor="#F0FAFF">编辑处理状态</td>
    <td height="35" align="left" valign="middle" bgcolor="#F0FAFF">编辑有效性</td>
  </tr>
  <tr>
    <td align="left" valign="middle">
        <asp:ListBox ID="LB_clzt" runat="server" Height="64px" Width="91px">
            <asp:ListItem Selected="True">处理完成</asp:ListItem>
            <asp:ListItem>正在处理</asp:ListItem>
            <asp:ListItem>未处理</asp:ListItem>
        </asp:ListBox>
        </td>
    <td align="left" valign="middle">
        <asp:ListBox ID="LB_clzt0" runat="server" Height="64px" Width="91px">
            <asp:ListItem Selected="True">有效</asp:ListItem>
            <asp:ListItem>无效</asp:ListItem>
        </asp:ListBox>
        </td>
  </tr>
  <tr>
    <td align="left" valign="middle" colspan="2">&nbsp;</td>
  </tr>
  <tr>
    <td align="center" valign="middle" colspan="2">
        <asp:Button ID="Button1" runat="server" Text="保存处理结果备注" 
            onclick="Button1_Click" />
    
    
    
      </td>
  </tr>
</table>
    
    
        <asp:Label ID="yctxt" runat="server" Text="..........." Visible="False"></asp:Label>
    
        <asp:Label ID="mmm" runat="server" Text="..........." Visible="False"></asp:Label>
    
        <asp:Label ID="iii" runat="server" Text="..........." Visible="False"></asp:Label>
    
    </div>
    </form>
</body>
</html>
