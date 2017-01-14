<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login11.aspx.cs" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>登陆</title>
        <style type="text/css">
<!--
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	font-size:9pt;
}
A { TEXT-DECORATION: none; color: #000000;}
A:hover {TEXT-DECORATION: none;color: #000000;}
a:link { text-decoration: none ; color: #000000;}
a:visited { text-decoration: none ; color: #000000;}

-->
</style>
</head>
<body bgcolor="#ffffff" onload="document.form1.Login1$UserName.focus();">
    <form id="form1" runat="server">
    <div>
        <br />
        <br /><asp:Login ID="Login1" runat="server" DestinationPageUrl="Web/Default.aspx" Height="87px" Width="370px" OnLoggingIn="Login1_LoggingIn">
            <LayoutTemplate>

<table width="1001" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
   <td><img name="login_r1_c1" src="images/login_r1_c1.gif" width="1001" height="117" border="0" id="login_r1_c1" alt="" /></td>
  </tr>
  <tr>
   <td><img name="login_r2_c1" src="images/login_r2_c1.gif" width="1001" height="100" border="0" id="login_r2_c1" alt="" /></td>
  </tr>
  <tr>
   <td><img name="login_r3_c1" src="images/login_r3_c1.gif" width="1001" height="100" border="0" id="login_r3_c1" alt="" /></td>
  </tr>
  <tr>
   <td><img name="login_r4_c1" src="images/login_r4_c1.gif" width="1001" height="122" border="0" id="login_r4_c1" alt="" /></td>
  </tr>
  <tr>
   <td><table align="left" border="0" cellpadding="0" cellspacing="0" width="1001">
	  <tr>
	   <td style="height: 133px"><table align="left" border="0" cellpadding="0" cellspacing="0" width="803">
		  <tr>
		   <td style="height: 117px"><table align="left" border="0" cellpadding="0" cellspacing="0" width="803">
			  <tr>
			   <td><img name="login_r5_c1" src="images/login_r5_c1.gif" width="204" height="110" border="0" id="login_r5_c1" alt="" /></td>
			   <td><table align="left" border="0" cellpadding="0" cellspacing="0" width="599">
			     <!--DWLayoutTable-->
				  <tr>
				   <td width="599" style="height: 76px"><table align="left" border="0" cellpadding="0" cellspacing="0" width="599">
					  <tr>
					   <td><table align="left" border="0" cellpadding="0" cellspacing="0" width="486">
						  <tr>
						   <td style="height: 19px"><img name="login_r5_c2" src="images/login_r5_c2.gif" width="486" height="19" border="0" id="login_r5_c2" alt="" /></td>
						  </tr>
						  <tr>
						   <td><table align="left" border="0" cellpadding="0" cellspacing="0" width="486">
						     <!--DWLayoutTable-->
							  <tr>
							   <td style="height: 26px"><img name="login_r7_c2" src="images/login_r7_c2.gif" width="105" height="26" border="0" id="login_r7_c2" alt="" /></td>
							   <td width="168" valign="middle" style="height: 26px"><!--DWLayoutEmptyCell-->&nbsp;<asp:TextBox ID="UserName" runat="server" CssClass="input1" Width="140px"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                       ErrorMessage="必须填写“用户名”。" ToolTip="必须填写“用户名”。" ValidationGroup="Login1">*</asp:RequiredFieldValidator></td>
							   <td style="height: 26px"><img name="login_r7_c4" src="images/login_r7_c4.gif" width="51" height="26" border="0" id="login_r7_c4" alt="" /></td>
							   <td width="162" valign="middle" style="height: 26px"><!--DWLayoutEmptyCell-->&nbsp;<asp:TextBox ID="Password" runat="server" CssClass="input1" TextMode="Password"
                                       Width="140px"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                       ErrorMessage="必须填写“密码”。" ToolTip="必须填写“密码”。" ValidationGroup="Login1">*</asp:RequiredFieldValidator></td>
							  </tr>
							</table></td>
						  </tr>
						  <tr>
						   <td align="center" style="color: red">
                               &nbsp;<asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></td>
						  </tr>
						</table></td>
					   <td><img name="login_r5_c6" src="images/login_r5_c6.gif" width="12" height="62" border="0" id="login_r5_c6" alt="" /></td>
					   <td><table align="left" border="0" cellpadding="0" cellspacing="0" width="50">
						  <tr>
						   <td><img name="login_r5_c7" src="images/login_r5_c7.gif" width="50" height="7" border="0" id="login_r5_c7" alt="" /></td>
						  </tr>
						  <tr>
						   <td>
                               &nbsp;<asp:ImageButton ID="ImageButton1" runat="server" CommandName="Login" ImageUrl="images/login_r6_c7.gif"
                                   ValidationGroup="Login1" />
                           </td>
						  </tr>
						  <tr>
						   <td><img name="login_r9_c7" src="images/login_r9_c7.gif" width="50" height="5" border="0" id="login_r9_c7" alt="" /></td>
						  </tr>
						</table></td>
					   <td style="width: 52px"></td>
					  </tr>
					</table></td>
				  </tr>
				  <tr>
				   <td>
                       &nbsp;</td>
				  </tr>
				  <tr>
				   <td height="19" valign="top" align="right"><!--DWLayoutEmptyCell--><span style="color: #002D6D; font-size:12px"> &nbsp; &nbsp;&nbsp; </span></td>
				  </tr>
				  <tr>
				   <td><img name="login_r12_c2" src="images/login_r12_c2.gif" width="599" height="7" border="0" id="login_r12_c2" alt="" /></td>
				  </tr>
				</table></td>
			  </tr>
			</table></td>
		  </tr>
		  <tr>
		   <td><img name="login_r13_c1" src="images/login_r13_c1.gif" width="803" height="5" border="0" id="login_r13_c1" alt="" /></td>
		  </tr>
		</table></td>
	   <td style="height: 133px"><img name="login_r5_c9" src="images/login_r5_c9.gif" width="198" height="115" border="0" id="login_r5_c9" alt="" /></td>
	  </tr>
	</table></td>
  </tr>
  <tr>
   <td style="height: 57px"><img name="login_r14_c1" src="images/login_r14_c1.gif" width="1001" height="55" border="0" id="login_r14_c1" alt="" /></td>
  </tr>
</table>
                </LayoutTemplate>
        </asp:Login>
    </div>
    </form>
</body>
</html>
