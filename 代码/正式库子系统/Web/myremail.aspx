<%@ Page Language="C#" AutoEventWireup="true" CodeFile="myremail.aspx.cs" Inherits="myremail" Async="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" href="../css/style.css" rel="Stylesheet" />
    <style type="text/css">
        #modPD1_showall
        {
            height: 300px;
        }
    </style>
    <base target="_self">
</head>
<body>
    <form id="form1" runat="server">
    
 <div>
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="font-size:10pt; color:#333333">
  <tr>
    <td  align="center" valign="top">
      <table width="95%" border="0" cellpadding="0" cellspacing="0" style="font-size:10pt; color:#333333">
        <tr>
          <td align="left" valign="middle" height="25px"><asp:Label ID="bt" runat="server" style="font-size:10pt;color:#0066ff"></asp:Label></td>
        </tr>
      </table>
      <table width="95%" border="0" cellpadding="0" cellspacing="0"  style="font-size:10pt; color:#333333;">
        <tr>
          <td align="left" valign="middle" height="20px">提交时间： <asp:Label ID="tjsj" runat="server"></asp:Label></td>
        </tr>
        <tr><td align="left" valign="middle" height="20px">提交内容：</td></tr>
      </table>
      <table width="95%" border="0" cellpadding="0" cellspacing="0" style="font-size:10pt; color:#333333">
        <tr>
          <td align="center" valign="middle" height="300px"><div id="modPD1_showall"  runat="server"></div> </td>
        </tr>
      </table><br/>
      <table width="95%" height="20" border="0" cellpadding="0" cellspacing="0" style="font-size:10pt; color:#333333">
        <tr>
          <td align="left" valign="middle"><span style="font-size:10pt;color:#0066ff">【回复】</span></td>
        </tr>
      </table>
      <table width="95%" border="0" cellpadding="0" cellspacing="0" style="font-size:10pt; color:#333333">
        <tr>
          <td align="left" valign="middle">收件人邮箱：<asp:TextBox ID="yjdz" runat="server" Width="300px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="yjdz" ErrorMessage="收件人邮箱不能为空！"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="yjdz" ErrorMessage="请输入正确的Email！" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
          </td>
        </tr>
         <tr> 
          <td align="left" valign="middle">邮件标题：&nbsp; &nbsp; <asp:TextBox ID="yjbt" runat="server" Width="300px"></asp:TextBox></td>
        </tr>
        <tr>
          <td align="left" valign="middle">发件人姓名：<asp:TextBox ID="fjrxm" runat="server" Width="300px"></asp:TextBox></td>
        </tr>
        <tr>
          <td align="left" valign="middle">发件人邮箱：<asp:TextBox ID="fjryx" runat="server" Width="300px"></asp:TextBox> 
           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="fjryx" ErrorMessage="发件人邮箱不能为空！"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="fjryx" ErrorMessage="请输入正确的Email！" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
          </td>
        </tr>
        <tr>
          <td align="left" valign="middle">回复内容：&nbsp;&nbsp;  &nbsp;<asp:TextBox ID="hfnr" runat="server" Height="100px" TextMode="MultiLine" Width="655px" Text="这里填写回复的信息"></asp:TextBox></td>
        </tr>
        <tr><td align="center" valign="middle"><asp:Button ID="btn_fs" runat="server" 
                Text="发送邮件"  CssClass="buttonlong" Height="20px" onclick="btn_fs_Click" />
            <asp:Button ID="Button1" runat="server" Text="取消"  CssClass="buttonlong" Height="20px"  OnClientClick="window.top.close_yjqx();return false;" />
            </td></tr>
      </table>
      
    </td>
  </tr>
</table>
</div>
    </form>
</body>
</html>
