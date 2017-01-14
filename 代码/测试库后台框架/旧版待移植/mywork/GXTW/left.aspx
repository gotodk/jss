<%@ Page Language="C#" AutoEventWireup="true" CodeFile="left.aspx.cs" Inherits="mywork_GXTW_left" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style=" background-color:#EDF7FA;">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="dh.css" rel="stylesheet" />
    <style>
        a:link {color:#464646;line-height:150%;text-decoration: none;	}
a:visited {color:#464646;text-decoration: none;	}
a:hover {color:#3399FF;	text-decoration:none;}
    </style>
</head>
<body style=" background-color:#EDF7FA;">
    <form id="form1" runat="server">
    <table width="130px" height="100%" border="0" cellpadding="0" cellspacing="0" style=" margin-top:10px; text-align:left; font-size:14px; font-weight:bolder;">
        <tr>
            <td>
                <asp:TreeView runat="server">
                    <Nodes>      
                        <asp:TreeNode Value="交易平台通知" Target="rightFrame" NavigateUrl="~/mywork/GXTW/TZ.aspx" Selected="true">                        
                            </asp:TreeNode>
                            <asp:TreeNode Value="经纪人查询" Target="rightFrame" NavigateUrl="~/mywork/GXTW/JJRCX.aspx"></asp:TreeNode>
                            <asp:TreeNode Value="富美大学生发展基金" Target="rightFrame" NavigateUrl="~/mywork/GXTW/FMDXSFZJJ.aspx"></asp:TreeNode>
                            <%--<asp:TreeNode Value="院系维护" Target="rightFrame" NavigateUrl="~/mywork/GXTW/YXWH.aspx"></asp:TreeNode>--%>
                            <asp:TreeNode Value="修改密码" Target="rightFrame" NavigateUrl="~/mywork/GXTW/XGMM.aspx"></asp:TreeNode>
                    </Nodes>
                    <NodeStyle Height="30px" ImageUrl="~/mywork/GXTW/image/left.png" />
                </asp:TreeView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
