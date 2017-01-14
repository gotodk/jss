<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangPass.aspx.cs" Inherits="Web_ChangPass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>修改密码</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
<style type="text/css">
<!--
    .input1 {
	BACKGROUND-COLOR: transparent; BORDER-BOTTOM: #B3B3B3 1px solid; BORDER-LEFT: transparent 0px solid; BORDER-RIGHT: transparent 0px solid; BORDER-TOP: #D8D8D8 0px solid; COLOR: #666666;
}
-->
</style>
</head>

<body style="margin: 5px;" oncontextmenu="return false" onselectstart="return false" ondragstart="return false">
    <form id="form1" runat="server">
<table width="520" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
<td align="center" valign="middle" bgcolor="#ffffff" style="color:#000000; font-size:14pt"><strong>密码修改</strong></td>
  </tr>
</table>
<table width="520" border="0" align="center" cellpadding="0" cellspacing="5" style="color:#F00">
  <tr>
    <td><p align="left">·员工应妥善保管工号和密码，不得将账户信息转交他人使用，不得使用他人账户信息登录。 <br />
        <br />
·首次登录后应立即修改默认密码，新设置密码至少7位，并区分大小写。<br />
<br />
·必须使用较为复杂的密码，不易被别人冒用，避免因密码保管不善造成信息泄露。<br />
<br />
·由于密码保管不善导致的信息泄露，责任自负。<br />
<br />
·忘记密码，总部员工携带本人工卡到信息化中心登记备案；办事处员工联系对口行政商务，由对口行政商务到信息化中心进行登记备案；信息化中心安排专人负责恢复默认密码。</p></td>
  </tr>
</table>
<table width="500" border="0" align="center" cellpadding="0" cellspacing="5" style="color:#F00">
  <tr>
    <td><hr align="center" width="100%" size="1" noshade="noshade" />      <p align="center">
<asp:ChangePassword ID="ChangePassword1" runat="server" CssClass="txt1" Width="100%" OnChangedPassword="ChangePassword1_ChangedPassword">
            <CancelButtonStyle CssClass="BtnAction" />
            <ChangePasswordButtonStyle CssClass="BtnAction" />
            <ChangePasswordTemplate>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 325px" align="center">
            <tr>
                <td align="center" colspan="2" style="height: 23px; width: 305px;">
                    <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">　　　密码:</asp:Label>
                    <asp:TextBox ID="CurrentPassword" runat="server" CssClass="input1"
                        TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                        ErrorMessage="必须填写“密码”。" ToolTip="必须填写“密码”。" ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="width: 305px">
                    <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">　　新密码:</asp:Label>
                    <asp:TextBox ID="NewPassword" runat="server" CssClass="input1"
                        TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                        ErrorMessage="必须填写“新密码”。" ToolTip="必须填写“新密码”。" ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="width: 305px">
                    <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">确认新密码:</asp:Label>
                    <asp:TextBox ID="ConfirmNewPassword" runat="server" CssClass="input1"
                        TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                        ErrorMessage="必须填写“确认新密码”。" ToolTip="必须填写“确认新密码”。" ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                    </td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="width: 305px">
<br/><br/>
                    <asp:Button ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword"
                        CssClass="BtnAction" Text="更改密码" ValidationGroup="ChangePassword1" /></td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="width: 305px">
                    <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                        ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="“确认新密码”与“新密码”项必须匹配。"
                        ValidationGroup="ChangePassword1"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="color: red; width: 305px;">
                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                </td>
            </tr>
      </table>
      
            </ChangePasswordTemplate>
            <ContinueButtonStyle CssClass="BtnAction" />
            <SuccessTemplate>
            
<br /><br />
                            <table border="0" cellspacing="0" cellpadding="0"  width="100%"/>
                                    <td align="center" colspan="2" height="50">
                                        您的密码已成功更改!</td>
                                </tr>
                               
                            </table>

            </SuccessTemplate>
        </asp:ChangePassword>

</p></td>
  </tr>
</table>
<p>&nbsp;</p>

        

    </form>
</body>
</html>
