<%@ Page Language="C#" AutoEventWireup="true" CodeFile="XGMM.aspx.cs" Inherits="mywork_GXTW_XGMM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link href="dh.css" rel="stylesheet" />
<link href="Css/fwsite.css" rel="stylesheet" />
    <script src="js/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="newstop">
            <tr>
                <td width="50" height="29" align="right">
                    &nbsp;&nbsp;
                </td>
                <td align="left">
                    您现在的位置：  修改密码 
                </td>
            </tr>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" >
        <tr>
            <td width="50" height="29" align="right">
                &nbsp;&nbsp;
            </td>
            <td align="left">                
            </td>
        </tr>
    </table>
    <div style=" margin-left:50px; text-align:left;">
        <table cellpadding="0" cellspacing="0" border="0">
<%--            <tr style=" height:35px;">
                <td>
                    原始密码：
                </td>
                <td>
                    <asp:TextBox ID="txtOldPassWord" runat="server" aa="aa" CssClass="tj_input" Width="178px"
                                    Enabled="True" TabIndex="1"></asp:TextBox>
                </td>
            </tr>--%>
            <tr style=" height:35px;">
                <td>
                    新密码：
                </td>
                <td>
                    <asp:TextBox ID="txtNewPassWord" runat="server" aa="aa" CssClass="tj_input" Width="178px"
                                    Enabled="True" TabIndex="1" TextMode="Password"></asp:TextBox>
                    
                </td>
                <td>
                    &nbsp;&nbsp;<asp:Label ID="lblTS" runat="server" Text="" style="color:red;"></asp:Label>
                </td>
            </tr>
             <tr style=" height:35px;">
                <td>
                </td>
                <td>
                                                    <asp:Button ID="btnSeave" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="保存" Width="70px" style=" margin-left:10px;" OnClick="btnSeave_Click" />
                                
                </td>
                 <td></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
