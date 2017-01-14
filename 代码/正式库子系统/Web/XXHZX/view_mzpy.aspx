<%@ Page Language="C#" AutoEventWireup="true" CodeFile="view_mzpy.aspx.cs" Inherits="Web_XXHZX_view_mzpy" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
     <link type="text/css" rel="Stylesheet" href="/css/style.css" />
    <link href="/web/yhb_BigPage_css/YahooGridView.css" type="text/css" rel="stylesheet" />  
</head>
<body>
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="0px" Skin="Default2006">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" Text="查看工作能力评议表">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>  
    <br />
  <br />
    <table width="90%"><tr><td>
        <asp:Button ID="Button2" runat="server" Text="清空得分" OnClick ="Bttton_qingkong" />
        &nbsp;<asp:Button ID="Button1" runat="server" Text="导出到Excel" OnClick ="Button1_Click"/>
        </td></tr></table>
    
        <asp:GridView ID="GridView1" runat="server" CssClass="GridViewStyle">
        <HeaderStyle CssClass="HeaderStyle" />
        <RowStyle CssClass="RowStyle" />        
        <AlternatingRowStyle CssClass="AltRowStyle" />
        </asp:GridView>
    </form>
</body>
</html>
