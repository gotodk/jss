<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getsy003.aspx.cs" Inherits="mywork_getsy003_" ResponseEncoding="gb2312" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=gb2312"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <jieduan>
             <%-- 商品排名TOP10 最近30个交易日(含当日）累计成交次数前10名的商品 --%>
        <asp:Repeater ID="Repeater1" runat="server" EnableTheming="False" EnableViewState="False">
            <HeaderTemplate></HeaderTemplate>

<ItemTemplate>

<tr>
                <td class="pm_talbe_first <%# ((System.Data.DataRowView)Container.DataItem)["特殊样式a"]%>" title="<%# ((System.Data.DataRowView)Container.DataItem)["xuhao"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["xuhao"]%></td>
                <td class="ziti_lan" title="<%# ((System.Data.DataRowView)Container.DataItem)["s1"]%>"><%# CutString(((System.Data.DataRowView)Container.DataItem)["s1"].ToString(),10)%></td>
                <td title="<%# ((System.Data.DataRowView)Container.DataItem)["s2"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["s2"]%></td>
                <td class="<%# ((System.Data.DataRowView)Container.DataItem)["特殊样式a"]%>" title="<%# ((System.Data.DataRowView)Container.DataItem)["s3"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["s3"]%></td>
              </tr>

</ItemTemplate>

<FooterTemplate></FooterTemplate>
        </asp:Repeater>
            </jieduan>
    </form>
</body>
</html>
