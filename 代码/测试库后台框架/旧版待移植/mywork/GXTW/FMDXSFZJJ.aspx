<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FMDXSFZJJ.aspx.cs" Inherits="mywork_GXTW_FMDXSFZJJ" %>

<%@ Register Src="../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link href="dh.css" rel="stylesheet" />
<link href="Css/fwsite.css" rel="stylesheet" />
</head>
<body style="text-align:left;">
    <form id="form1" runat="server">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="newstop">
            <tr>
                <td width="50" height="29" align="right">
                    &nbsp;&nbsp;
                </td>
                <td align="left">
                    您现在的位置：  富美大学生发展基金 
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
        <div class="content_nr" id="divLB" runat="server" style=" width:800px; margin-left:50px; text-align:left;">
            <table align="left" cellpadding="0" cellspacing="0" class="table_nr" style=" width:100%; margin-left:0px;">                       
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" border="1px" bordercolor="#D1CFCF" style="border-style: solid;
                                    border-collapse: collapse; text-align: center; width: 100%;">
                                    <thead>
                                        <tr style="background-color: #D6E3F3; font-weight: bold;">
                                            <td height="28" width="300px">
                                                高校名称
                                            </td>
                                            <td height="28" width="500px">
                                                富美大学生发展基金金额（单位：万）
                                            </td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr style="height: 28px;">
                                            <td width="300px" title='<%#Eval("建议内容") %>'>
                                                <asp:Label ID="lbljynr" runat="server" Text='系统自动从登陆账户带出'> </asp:Label>
                                            </td>
                                            <td width="500px" title='<%#Eval("建议者")%>'>
                                                <asp:Label ID="lblJYZ" runat="server" Text='贵单位经纪人累计收益尚不足30万，暂无该项基金，继续加油！'> </asp:Label>
                                            </td>                                                    
                                        </tr>
                                        <%--<asp:Repeater ID="rpt" runat="server">
                                            <ItemTemplate>
                                                <tr style="height: 28px;">
                                                    <td width="300px" title='<%#Eval("建议内容") %>'>
                                                        <asp:Label ID="lbljynr" runat="server" Text='<%#Eval("建议内容")%>'> </asp:Label>
                                                    </td>
                                                    <td width="500px" title='<%#Eval("建议者")%>'>
                                                        <asp:Label ID="lblJYZ" runat="server" Text='<%#Eval("建议者")%>'> </asp:Label>
                                                    </td>                                                    
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>--%>
                                    </tbody>
                                    <tfoot id="tempty" runat="server" style=" display:none;">
                                        <tr>
                                            <td colspan="5" class="auto-style1">
                                                <asp:Label ID="Label2" runat="server" Text="您所查询的数据为空！"></asp:Label>
                                            </td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc2:commonpager ID="commonpager1" runat="server" />
                            </td>
                        </tr>
                    </table>
        </div>
    </form>
</body>
</html>
