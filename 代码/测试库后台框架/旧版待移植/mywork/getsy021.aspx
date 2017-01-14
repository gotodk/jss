<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getsy021.aspx.cs" Inherits="mywork_getsy021" ResponseEncoding="gb2312" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
           <%-- <td class="pm_talbe_first <%# ((System.Data.DataRowView)Container.DataItem)["特殊样式a"]%>" title="<%# ((System.Data.DataRowView)Container.DataItem)["xuhao"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["xuhao"]%></td>--%>
                <td style="width: 31%" class="companynews_talbe_first" title="<%# ((System.Data.DataRowView)Container.DataItem)["s1"]%>"><%# CutString(((System.Data.DataRowView)Container.DataItem)["s1"].ToString(),5)%></td>
                <td style="width: 18%" class="ziti_juhong" title="<%# ((System.Data.DataRowView)Container.DataItem)["s2"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["s2"]%></td>
                <td style="width: 32%" class="ziti_juhong" title="<%# ((System.Data.DataRowView)Container.DataItem)["s3"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["s3"]%></td>
                <td style="width: 18%" title="<%# ((System.Data.DataRowView)Container.DataItem)["s4"]%>"><%# ((System.Data.DataRowView)Container.DataItem)["s4"]%></td>
              
 </tr>

</ItemTemplate>

<FooterTemplate></FooterTemplate>
        </asp:Repeater>
            </jieduan>
    </form>
</body>
</html>