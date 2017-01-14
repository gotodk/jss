<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dr_employee.aspx.cs" Inherits="Web_dr_employee" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>

<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>导入员工档案</title>
    <link type="text/css" href="../css/style.css" rel="Stylesheet" />
     
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <table  id="tb_sc" runat="server" width="400" border="0" align="center" cellpadding="1" cellspacing="1" bgcolor="#CCCCCC"  style="color:#000; font-size:12px">
    <tr>
    <td height="35" align="center" valign="middle" bgcolor="#669999" style="color:#FFF; font-size:14px">员工档案生成</td>
    </tr>
    <tr>
    <td height="54" bgcolor="#FFFFFF"  style="color:#F00; font-size:12px">警告：一旦点击&quot;生成档案&quot;后，会将生成的工号分配给该员工，并同时产生正式的员工档案。此操作不可撤销,请谨慎使用此功能!</td>
  </tr>
    <tr><td height="35" bgcolor="#FFFFFF">点击此按钮生成员工号: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btn_sc" runat="server" Text="生成工号" OnClick="btn_sc_Click1" /></td></tr>
    <tr><td height="35" align="center" valign="middle" bgcolor="#FFFFFF"><asp:TextBox ID="ygbh" runat="server" ReadOnly="True"></asp:TextBox></td></tr>
    <tr><td align="center"  style="color:#F00; font-size:12px">确认:<asp:TextBox 
            ID="ygbh_ok" runat="server" 
            Width="48px"></asp:TextBox>&nbsp;&nbsp; <asp:Button ID="btn_dr" runat="server" Text="生成档案" OnClick="btn_dr_Click" />
        <br />
        (请在输入框中输入&quot;确定&quot;两个汉字后，点击生成档案)</td></tr>
    <tr><td height="40" align="center" valign="middle" bgcolor="#FFFFFF"><asp:Label ID="module_txt" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="number_txt" runat="server" Visible="False"></asp:Label></td></tr>
    </table>
    </div>
    </form>
</body>
</html>
