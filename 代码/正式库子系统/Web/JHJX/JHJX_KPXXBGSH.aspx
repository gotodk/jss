<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_KPXXBGSH.aspx.cs" Inherits="Web_JHJX_JHJX_KPXXBGSH" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/style.css" rel="stylesheet" />
    <link href="../yhb_BigPage_css/WhiteChromeGridView.css" rel="Stylesheet" type="text/css" />
    
    <style type="text/css">
        .neikuang
        {
            border: 1px solid #818181;
        }
        .neikuangbg
        {
            background: url(../images/back.jpg);
            background-repeat: repeat-x;
            font-family: "Verdana" , "Tamoda" , "宋体";
            font-size: 12px;
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%; height: 100%; border-width: 0px; text-align: center">
        <table width="700" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="4" height="20px">
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center" style="font-size: 12pt; font-weight: bold">
                    开票信息变更申请
                </td>
            </tr>
            <tr>
                <td width="100px" align="right" height="30px">
                    用户名
                </td>
                <td width="200px" align="left">
                    <asp:TextBox ID="txtYHM" runat="server" Enabled="false" Width="100" CssClass="input1"></asp:TextBox>
                </td>
                <td width="100px" align="right">
                    登录邮箱 </td>
                <td width="200px" align="left">
                    <asp:TextBox ID="txtDLYX" runat="server" Enabled="false" Width="100" CssClass="input1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="100px" align="right" height="30px">
                    角色账户类型
                </td>
                <td align="left">
                    <asp:TextBox ID="txtZHLX" runat="server" Enabled="false" Width="200" CssClass="input1"></asp:TextBox>
                </td>
                <td width="100px" align="right">
                    申请时间
                </td>
                <td width="200px" align="left">
                    <asp:TextBox ID="txtSQSJ" runat="server" Enabled="false" Width="200" CssClass="input1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="100px" align="right" height="30px">
                    当前发票类型
                </td>
                <td colspan="3" align="left">
                    <asp:TextBox ID="txtDQFPLX" runat="server" Enabled="false" Width="150" CssClass="input1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="100px" align="right">
                    当前开票信息
                </td>
                <td colspan="3" align="left">
                    <asp:TextBox ID="txtDQKPXX" runat="server" Enabled="false" Width="400" TextMode="MultiLine"
                        Height="80px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="100px" align="right" height="30px">
                    新发票类型
                </td>
                <td colspan="3" align="left">
                    <asp:TextBox ID="txtXFPLX" runat="server" Enabled="false" Width="150" CssClass="input1" OnTextChanged="txtXFPLX_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="100px" align="right" height="100px">
                    新开票信息
                </td>
                <td colspan="3" align="left">
                    <asp:TextBox ID="txtXKPXX" runat="server" Enabled="false" Width="400" TextMode="MultiLine"
                        Height="80px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="100px" align="right" height="30px" runat="server" id="nsrzz">
                    纳税人资质
                </td>
                <td colspan="3" align="left">
                    <asp:Button ID="btnChakan" runat="server" Text="查看" OnClick="btnChakan_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center" style="font-size: 12pt; font-weight: bold; height: 40px">
                    处理变更申请
                </td>
            </tr>
            <tr>
                <td width="100px" align="right" style="height: 40px; font-weight: bold">
                    审核状态
                </td>
                <td align="left" colspan="3">
                    <asp:TextBox ID="txtSHZT" runat="server" Enabled="false" Width="100" CssClass="input1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="100px" align="right" style="height: 40px; font-weight: bold">
                    处理结果
                </td>
                <td align="left" colspan="3">
                    <asp:RadioButtonList ID="rblCLJG" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblCLJG_SelectedIndexChanged">
                        <asp:ListItem>新发票信息已更新完毕，请查看核对。</asp:ListItem>
                        <asp:ListItem>新发票类型及新发票信息已更新完毕，请查看核对。</asp:ListItem>
                        <asp:ListItem>您提交的变更信息证件不全，开票信息无法更新，请提交完整的变更信息证件后重新提交申请。</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td width="100px" align="right" style="height: 80px">
                    处理备注
                </td>
                <td align="left" colspan="3">
                    <asp:TextBox ID="txtCLBZ" runat="server" Width="400" TextMode="MultiLine" Height="70px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="100px" align="right">
                </td>
                <td colspan="3" align="left" style="height: 40px" valign="bottom">
                    <asp:Button ID="btnPass" runat="server" Text="完成处理" OnClick="btnPass_Click" />&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="4" align="left" style="height: 50px; color: Red">
                    <span style="font-weight: bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 说明：</span>“处理备注”用于填写处理过程中出现的问题或者需要记录的内容，这部分内容不会给用户显示。
                </td>
            </tr>
        </table>
        <input id="iptnumber" runat="server" type="hidden" />
        <input id="iptSHYJ" runat="server" type="hidden" />
    </div>
    </form>
</body>
</html>