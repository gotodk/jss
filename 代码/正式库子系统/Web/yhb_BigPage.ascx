<%@ Control Language="C#" AutoEventWireup="true" CodeFile="yhb_BigPage.ascx.cs" Inherits="大量数据测试.yhb_BigPage" %>
<link href="yhb_BigPage.css" type="text/css" rel="stylesheet">
<div id="bigys" runat="server">

</div>


<asp:GridView ID="GV_show" runat="server" Width="2000" CssClass="GridViewStyle" >
        <HeaderStyle CssClass="HeaderStyle" />
        <RowStyle CssClass="RowStyle" />
        <AlternatingRowStyle CssClass="AltRowStyle" />
        </asp:GridView>
<!-- 分页 begin -->
		<table><tr><td>
			<div class="yhb_pagebox" id="fybox" runat="server">

				

			</div>
		</td></tr></table>
		<!-- 分页 end --> 
 