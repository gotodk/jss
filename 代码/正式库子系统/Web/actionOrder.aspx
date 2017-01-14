<%@ Page Language="C#" AutoEventWireup="true" CodeFile="actionOrder.aspx.cs" Inherits="actionOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
        <style type="text/css">
        .TDCSS {FONT-SIZE: 12px;}
        .FormView
        {
	        background: #f7f7f7;
	        border: solid 1px #e5e5e5;
	        border-right: solid 2px #e5e5e5;
	        border-top: 1px;
	        font: normal 12px 宋体;
        }
        .Title
        {
            color:#FF6600; 
            font-size:14px;
        }
        .input1 
        {
	    BACKGROUND-COLOR: transparent; 
	    BORDER-BOTTOM: #B3B3B3 1px solid; 
	    BORDER-LEFT: transparent 0px solid; 
	    BORDER-RIGHT: transparent 0px solid; 
	    BORDER-TOP: #D8D8D8 0px solid; COLOR: #666666;
	    margin-bottom:0px;  
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class ="FormView">
        <tr><td colspan ="4">生成销售单的前置工作：</td></tr>
            <tr>
                <td style="height: 24px">
                    *客户经理</td><td style="height: 24px"><asp:TextBox ID="KHJL" runat="server" CssClass ="input1" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="KHJL"
                        ErrorMessage="不能为空"></asp:RequiredFieldValidator></td>
                <td style="height: 24px">
                    *付款方式</td><td style="height: 24px"><asp:DropDownList ID="FKFS" runat="server" CssClass ="input1">
                    <asp:ListItem>预付全款</asp:ListItem>
                    <asp:ListItem>货到付款</asp:ListItem>
                    <asp:ListItem>定期付款</asp:ListItem>
                </asp:DropDownList></td>    
            </tr>
            <tr>
                <td>
                    *发货人</td>
                <td><asp:TextBox ID="FHR" runat="server" CssClass ="input1" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FHR"
                        ErrorMessage="不能为空"></asp:RequiredFieldValidator></td>
                <td>
                    *提货/送货人</td>
                <td><asp:TextBox ID="THSHR" runat="server" CssClass ="input1" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="THSHR"
                        ErrorMessage="不能为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td colspan ="4" align=center>
                    <asp:Button ID="btnOK" runat="server" Text="提交" CssClass ="input1" OnClick="btnOK_Click"/></td>
            </tr>
            </table>
        </div>
    </form>
</body>
</html>
