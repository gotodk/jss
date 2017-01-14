<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="mywork_GXTW_main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="dh.css" rel="stylesheet" />
    <link href="Css/fwsite.css" rel="stylesheet" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <style type="text/css">
        tr
        {
            white-space: nowrap;
        }
        td
        {
            white-space: nowrap;
        }
        body
        {
            font-size: 63%;
            color: #000;
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
        }
        
    </style>

</head>
<body>
    <form runat="server" id="form1">
        <div id="shenfen_yz" runat="server">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td style="color: #FF0000">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td height="10">
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0"
                        >
                        <tr>
                            <td style=" text-align:left; width:600px; height:56px;">
                                <img src="image/toptitle.jpg" style=" margin-left:25px;" />
                            </td>
                            <td style=" vertical-align:bottom; text-align:right; ">
                                <asp:Label ID="lblDLZH" runat="server" Text="Label" style=" margin-right:50px;"></asp:Label>

                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="linkb" OnClientClick="return window.confirm('您确定要退出系统吗？')" OnClick="LinkButton1_Click" style="margin-right:50px; text-decoration:none; "><img src="image/LoginOut.png" style="width:20px; height:20px; border:0px; vertical-align:bottom;" />&nbsp;&nbsp;退出系统</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table width="100%" border="1" align="center" cellpadding="0" cellspacing="0" style="border: 1px solid #CCC;
            height: 570px; background-color: #EDF7FA; border-top-color:#1C8EDC; border-top:3px;">
            <tr>
                <td align="left" valign="top" width="13%">
                    <iframe src="left.aspx" name="leftFrame" id="Iframe1" frameborder="0" scrolling="auto"
                        width="100%" height="570px" style="vertical-align: top;"></iframe>
                </td>
                <td width="88%" valign="top">
                    <iframe name="rightFrame" src="TZ.aspx" id="rightFrame" frameborder="0"
                        height="570px" scrolling="no" width="100%" style="vertical-align: top;" >
                    </iframe>
                </td>
            </tr>
        </table>
    </div>
        </form>
</body>
</html>
