<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkFlow_Update.aspx.cs" Inherits="WorkFlow_Update" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="0px" Skin="Default2006">
    <Tabs>
        <radTS:Tab ID="Tab1" runat="server" Text="新增">
        </radTS:Tab>
        <radTS:Tab ID="Tab2" runat="server" Text="修改">
        </radTS:Tab>
        <radTS:Tab ID="Tab3" runat="server" Text="查看">
        </radTS:Tab>
    </Tabs>
</radTS:RadTabStrip>