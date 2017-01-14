<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getsy012.aspx.cs" Inherits="mywork_getsy012_" ResponseEncoding="gb2312" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=gb2312"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <jieduan>
            <%-- 最大买入额TOP10  一种商品在一个轮次的买方最大中标额前10名。 --%>
        <asp:Repeater ID="Repeater1" runat="server" EnableTheming="False" EnableViewState="False">
            <HeaderTemplate></HeaderTemplate>

<ItemTemplate>

<tr>
                <td class="pm_talbe_first <%# ((System.Data.DataRowView)Container.DataItem)["特殊样式a"]%>" title="<%# ((System.Data.DataRowView)Container.DataItem)["xuhao"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["xuhao"]%></td>
     <td class="<%# ((System.Data.DataRowView)Container.DataItem)["特殊样式a"]%>" title="<%# ((System.Data.DataRowView)Container.DataItem)["s1"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["s1"]%></td>
                <td class="ziti_lan" title="<%# ((System.Data.DataRowView)Container.DataItem)["s2"]%>"><%# CutString(((System.Data.DataRowView)Container.DataItem)["s2"].ToString(),8)%></td>
                <td title="<%# ((System.Data.DataRowView)Container.DataItem)["s3"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["s3"]%></td>
               
              </tr>

</ItemTemplate>

<FooterTemplate></FooterTemplate>
        </asp:Repeater>
            </jieduan>
    </form>
</body>
</html>
