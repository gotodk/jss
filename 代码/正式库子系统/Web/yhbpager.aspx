<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yhbpager.aspx.cs" Inherits="Web_yhbpager" %>
<%@ Register Assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" TagPrefix="igxl" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="yhbpager.css" type="text/css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        改变每页数量:<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="搜索测试" />

        <asp:GridView ID="GV_show" runat="server" Width="2000">
        </asp:GridView>

        <br />
<table border="0" cellpadding="0" cellspacing="0" style=" display:none">
  <tr>
    <td>
    <div class="PageBoxYuHaiBin" id="PageBoxYuHaiBin" runat="server" >
        <asp:Button ID="PageBoxFirst" runat="server" Text="第一页"  class="pagebox_css" 
                onclick="PageBoxFirst_Click" Height="23" onmouseover="this.className='pagebox_css_onm';" onmouseout="this.className='pagebox_css';" />
        <asp:Button ID="PageBoxPre" runat="server" Text="上一页"  class="pagebox_css" 
                onclick="PageBoxPre_Click"  Height="23" onmouseover="this.className='pagebox_css_onm';" onmouseout="this.className='pagebox_css';" />
        <asp:Button ID="PageBoxPageNumber" runat="server" Text="0"  class="pagebox_css"  Visible="false"
                Width="30px" onclick="PageBoxPageNumber_Click"  Height="23" onmouseover="this.className='pagebox_css_onm';" onmouseout="this.className='pagebox_css';" />
        <asp:Button ID="PageBoxNext" runat="server" Text="下一页"  class="pagebox_css" 
                onclick="PageBoxNext_Click"  Height="23" onmouseover="this.className='pagebox_css_onm';" onmouseout="this.className='pagebox_css';" />
        <asp:Button ID="PageBoxEnd" runat="server" Text="最后一页"  class="pagebox_css" 
                onclick="PageBoxEnd_Click"  Height="23" onmouseover="this.className='pagebox_css_onm';" onmouseout="this.className='pagebox_css';" />
        <asp:Button ID="PageBoxGoto" runat="server" Text="转到"  class="pagebox_css" 
                onclick="PageBoxGoto_Click"  Height="23"  onmouseover="this.className='pagebox_css_onm';" onmouseout="this.className='pagebox_css';" />
        <asp:TextBox ID="PageBoxGotoNum" runat="server"  
            class="pagebox_css"  Height="23px" 
            onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)));" 
            style="ime-mode: Disabled;" MaxLength="10" Width="80px"></asp:TextBox>
        </div>
    </td>
    <td><div id="fyinfo" runat="server" style=" font-size:14px"></div></td>
  </tr>
</table>
        

    </div>
    </form>
</body>
</html>