<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_ZHPLZC.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_ZHPLZC" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 24px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <table width="700px" class="content_tab">
            <tr id="tr1" runat="server">
                            <td align="right" width="200px;">
                                生成账户数目：
                            </td>
                            <td>
                                 <asp:TextBox ID="txtNum" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr  id="trJYZHLX" runat="server">
                            <td align="right" width="200px;">
                                交易账户类型：
                            </td>
                            <td>
                                    <asp:RadioButton ID="radJJR" runat="server"  GroupName="ZHLX" Text="经纪人交易账户" Enabled="true" AutoPostBack="true" />&nbsp;&nbsp;<asp:RadioButton
                                    ID="radMMJ" runat="server" Text="交易方交易账户" Enabled="true" AutoPostBack="true" GroupName="ZHLX" />&nbsp;&nbsp;</td>
                        </tr>
         <tr id="tr5" runat="server">
                            <td align="right" width="200px;">
                                注册类别：
                            </td>
                            <td colspan="3">
                                 <asp:RadioButton ID="radDW" GroupName="ZCLB" Enabled="true" AutoPostBack="true"  runat="server" Text="单位" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton
                                    ID="radZRR" runat="server" GroupName="ZCLB" Enabled="true" AutoPostBack="true"  Text="自然人" />&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr id="trZCLB" runat="server">
                            <td align="right" width="200px;">
                                注册类别关联经纪人登陆邮箱：
                            </td>
                            <td>
                                 <asp:TextBox ID="txtGLJJRDLYX" runat="server"></asp:TextBox>
                                 &nbsp;
                            </td>
                        </tr>
             <tr id="tr2" runat="server">
                            <td align="right" width="200px;">
                                关联经纪人角色编号： </td>
                            <td>
                                  <asp:TextBox ID="txtGLJJRJSBH" runat="server"></asp:TextBox>
                            </td>
                        </tr>
             <tr id="tr3" runat="server">
                            <td align="right" width="200px;">
                                关联经纪人用户名：
                            </td>
                            <td>
                                  <asp:TextBox ID="txtGLJJRYHM" runat="server"></asp:TextBox>
                            </td>
                        </tr>
             <tr id="tr4" runat="server">
                            <td align="right" width="200px;" class="auto-style1">
                                可用余额：</td>
                            <td class="auto-style1">
                               <asp:TextBox ID="txtKHYE" runat="server"></asp:TextBox>
                                
                            </td>
                        </tr>
             
             
                        <tr>
                            <td align="center" width="200px;" colspan="2" >
                                &nbsp;<asp:Button ID="Button1" runat="server" Text="确定" OnClick="Button1_Click" />
                            </td>
                           
                        </tr>               
                    </table>
    </form>
</body>
</html>
