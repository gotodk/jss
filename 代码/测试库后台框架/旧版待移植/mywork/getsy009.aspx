<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getsy009.aspx.cs" Inherits="mywork_getsy009_" ResponseEncoding="gb2312" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=gb2312"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <jieduan>
            <%-- 最有购买力地区TOP10  历史累计买方中标金额最高的前10个区县 --%>
        <asp:Repeater ID="Repeater1" runat="server" EnableTheming="False" EnableViewState="False">
            <HeaderTemplate></HeaderTemplate>

<ItemTemplate>

<tr>
                <td class="pm_talbe_first <%# ((System.Data.DataRowView)Container.DataItem)["特殊样式a"]%>" title="<%# ((System.Data.DataRowView)Container.DataItem)["xuhao"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["xuhao"]%></td>
                <td class="ziti_lan" title="<%# ((System.Data.DataRowView)Container.DataItem)["s1"]%>"><%# CutString(((System.Data.DataRowView)Container.DataItem)["s1"].ToString(),12)%></td>                
                <td class=" <%# ((System.Data.DataRowView)Container.DataItem)["特殊样式a"]%>" title="<%# ((System.Data.DataRowView)Container.DataItem)["s2"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["s2"]%></td>
              </tr>

</ItemTemplate>

<FooterTemplate></FooterTemplate>
        </asp:Repeater>
            </jieduan>
    </form>
</body>
</html>
