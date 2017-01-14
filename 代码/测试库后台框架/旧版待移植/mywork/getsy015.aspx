<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getsy015.aspx.cs" Inherits="mywork_getsy015_" ResponseEncoding="gb2312" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=gb2312"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <jieduan>
        <asp:Repeater ID="Repeater1" runat="server" EnableTheming="False" EnableViewState="False">
            <HeaderTemplate></HeaderTemplate>

<ItemTemplate>

<tr>
                <td class="pm_talbe_first <%# ((System.Data.DataRowView)Container.DataItem)["特殊样式a"]%>" title="<%# ((System.Data.DataRowView)Container.DataItem)["xuhao"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["xuhao"]%></td>
                <td class="ziti_lan" title="<%# ((System.Data.DataRowView)Container.DataItem)["s1"]%>"><%# CutString(((System.Data.DataRowView)Container.DataItem)["s1"].ToString(),20)%></td>
                <td title="<%# ((System.Data.DataRowView)Container.DataItem)["s2"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["s2"]%></td>
                <%--<td class="ziti_juhong" title="<%# ((System.Data.DataRowView)Container.DataItem)["s3"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["s3"]%></td>--%>
              </tr>

</ItemTemplate>

<FooterTemplate></FooterTemplate>
        </asp:Repeater>
            </jieduan>
    </form>
</body>
</html>
