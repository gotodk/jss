<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhoneMess.aspx.cs" Inherits="Web_PhoneMess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
.aaa {
	font-size: 9px;
	text-decoration: none;
	background-image: url(images/msmbg1.jpg);
	padding-top: -2px;
	padding-right: 0px;
	padding-bottom: 0px;
	padding-left: 0px;
	height: 50px;
	background-repeat:no-repeat;
	background-position: 0px -9px;
}
body {
	margin-left: 0px;
	margin-top: 0px;
}
</style>
</head>
<body>
    <form id="form1" runat="server" >
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="aaa">
      <tr>
        <td  align="left" valign="top"><asp:CheckBox ID="CbPhone" runat="server" Text="短信提醒" AutoPostBack="true"  Font-Size="Small" ForeColor="White" 
                oncheckedchanged="CbPhone_CheckedChanged" Checked="false"></asp:CheckBox></td>
      </tr>
</table>
    </form>
</body>
</html>
