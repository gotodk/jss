<%@ Page Language="C#" AutoEventWireup="true" CodeFile="select_CheckInfo.aspx.cs" Inherits="select_CheckInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>显示审批信息</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .tabHead{
    background-image: url(images/headerbg.jpg);
    }
    </style>
    <script language="javascript" type="text/javascript" src="../js/tigra_tables.js"></script>
</head>
<body style="margin:3px">
   <form id="Form1" runat="server">

        <asp:datagrid id="MyDataGrid" runat="server" AutoGenerateColumns="False" name="MyDataGrid"
	        HorizontalAlign="Center" AlternatingItemStyle-BackColor="#eeeeee"
	        CellPadding="3" BorderWidth="1px"
	        BorderColor="#A2B5C6" OnPageIndexChanged="MyDataGrid_Page" PagerStyle-HorizontalAlign="Right"
	        PagerStyle-Mode="NumericPages" PageSize="15" AllowPaging="True" OnItemCommand="MyDataGrid_ItemCommand" OnItemDataBound="MyDataGrid_ItemDataBound" Width="100%" Font-Names="Verdana">
          <AlternatingItemStyle BackColor="#EEEEEE"></AlternatingItemStyle>
          <HeaderStyle Font-Bold="False" HorizontalAlign="Center" ForeColor="Black" CssClass="tabHead" Height="28px"></HeaderStyle>
          <PagerStyle HorizontalAlign="Right" Mode="NumericPages" Visible="False"></PagerStyle>
          <Columns>
          <asp:BoundColumn HeaderText="ID" DataField="id" Visible="False">
          </asp:BoundColumn>
              <asp:BoundColumn DataField="Number" HeaderText="单号"></asp:BoundColumn>
              <asp:BoundColumn DataField="Checker" HeaderText="审核人名称"></asp:BoundColumn>
              <asp:BoundColumn DataField="IsPass" HeaderText="审核状态" Visible="False">
              </asp:BoundColumn>
              <asp:BoundColumn DataField="Remark" HeaderText="审批备注"></asp:BoundColumn>
          <asp:BoundColumn HeaderText="审批日期" DataField="CheckTime" DataFormatString="{0:yyyy-MM-dd hh:mm:ss}">
          </asp:BoundColumn>
          </Columns>
            <ItemStyle HorizontalAlign="Center" />
        </asp:datagrid>
        <script language="JavaScript" type="text/javascript">
			<!--
				tigra_tables('MyDataGrid', 1, 0, '#F3F3F3', '#ffffff', '#F1F6F9', '#E0EFF2');
			// -->
			</script>
		<table width="100%" align="center">
		<tr>
		<td align="right" style="height: 17px">
		  <asp:label id="lblPageCount" runat="server"></asp:label>&nbsp;
          <asp:label id="lblCurrentIndex" runat="server"></asp:label>
          <asp:linkbutton id="btnFirst" onclick="PagerButtonClick" runat="server" 
  	        Font-size="8pt" ForeColor="navy" CommandArgument="0"></asp:linkbutton>&nbsp;
          <asp:linkbutton id="btnPrev" onclick="PagerButtonClick" runat="server" 
  	        Font-size="8pt" ForeColor="navy" CommandArgument="prev"></asp:linkbutton>&nbsp;
          <asp:linkbutton id="btnNext" onclick="PagerButtonClick" runat="server"
  	        Font-size="8pt" ForeColor="navy" CommandArgument="next"></asp:linkbutton>&nbsp;
          <asp:linkbutton id="btnLast" onclick="PagerButtonClick" runat="server" 
  	        Font-size="8pt" ForeColor="navy" CommandArgument="last"></asp:linkbutton>
		</td>
		</tr>
		</table>
        <p style="FONT-SIZE:9pt" align="center">
            &nbsp;</p>
</form>

</body>
</html>
