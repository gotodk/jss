<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="mywork_GXTW_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
    <link href="dh.css" rel="stylesheet" />
</head>

<body>
    <form id="AdminLogin" name="AdminLogin"  runat="server">
        <div style=" height:100px;"></div>
    <div style="width:350px;	margin:0 auto; background-position:0 -38px;">
        <table cellspacing="0" cellpadding="0" border="1px" bordercolor="#D1CFCF" style="border-style: solid;
                                    border-collapse: collapse; width: 100%;">
            <thead>
                <tr style="background-color: #D6E3F3; font-weight: bold;">
                    <td height="28" style=" text-align: left; font-size:16px; line-height:37px; 	font-weight:bold; text-indent:24px; border-right:0px; width:100px;">
                        用户登录
                    </td>
                    <td style="border-left:0px;"></td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td style=" border-right:0px; border-bottom:0px; border-top:0px;"></td>
                    <td style=" border-left:0px; text-align:left; border-bottom:0px; border-top:0px; height:20px; vertical-align:bottom;">
                        <asp:Label ID="lblTS" runat="server" Text="" style=" color:red; vertical-align:bottom;"></asp:Label>

                    </td>
                </tr>
                  <tr>
                      <td height="45" align="right" style="font-size:16px; font-weight:bold; border-right:0px; border-bottom:0px; border-top:0px; ">用户名：</td>
                      <td style="border-left:0px; text-align:left; border-bottom:0px; border-top:0px; ">
                          <asp:TextBox ID="txtUserName" runat="server" CssClass="tj_input" Width="178px"></asp:TextBox>
                      </td>
                  </tr>  
                <tr>
                      <td height="45" align="right" style="font-size:16px; font-weight:bold; border-right:0px; border-bottom:0px; border-top:0px;">密码：</td>
                      <td style="border-left:0px; text-align:left; border-bottom:0px; border-top:0px;">
                          <asp:TextBox ID="txtPassWord" runat="server" CssClass="tj_input" Width="178px" TextMode="Password"></asp:TextBox>
                      </td>
                  </tr> 
                <tr>
                    <td style="border-right:0px; border-bottom:0px; border-top:0px;">
                    </td>
                    <td style="border-left:0px; text-align:left; border-bottom:0px; border-top:0px; height:35px;">
                        <asp:Button ID="btnLogin" runat="server" Text="登录" CssClass="tj_bt" OnClick="btnLogin_Click" Width="80px"/>
                    </td>
                </tr>                    
            </tbody>
        </table>
    </div>

</form>
</body>
</html>
