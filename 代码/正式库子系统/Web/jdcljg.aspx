<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jdcljg.aspx.cs" Inherits="Web_jdcljg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
        #i0 {
            height: 300px;
        }
    </style>
    <base target="_self">
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="module_txt" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="number_txt" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="JDDX_temp_txt" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="CreateUser_temp_txt" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="nowuser_temp_txt" runat="server" Visible="False"></asp:Label>
    <div id="modPD1_showall"  runat="server"></div><HR />
    <div id="tijiaorenxinxin"  runat="server" style="font-size:14px; color:Red"></div><HR />
    <div id="fankuiinfo"  runat="server" style="font-size:12px; color:#000000"></div><HR />
    <div id="Dtijiaofk"  runat="server" style="font-size:14px;">
    提交反馈信息:
            <br />
    
        <asp:TextBox ID="tb_fankui" runat="server" Height="85px" Width="404px" 
            TextMode="MultiLine"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="提交反馈" onclick="Button1_Click" />
    </div>
    </div>
    
    </form>
</body>
</html>
