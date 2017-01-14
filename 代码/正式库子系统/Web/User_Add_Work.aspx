<%@ Page Language="C#" AutoEventWireup="true" CodeFile="User_Add_Work.aspx.cs" Inherits="User_Add_Work" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>员工添加工作页</title>
     <style type="text/css">
    .tabHead{
    background-image: url(images/headerbg.jpg);
    }
    </style>
    <script language="javascript" src="../js/tigra_tables.js"></script>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" /></head>
<body>
    <form id="form1" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="1">
            <tr>
                <td align="center" style="width: 1627px">
                                 
                                <asp:DataGrid ID="dgShow" runat="server" AutoGenerateColumns="False" Width="100%" OnItemDataBound="dgShow_ItemDataBound" PageSize="15" OnSelectedIndexChanged="Page_Load">
                                    <Columns>
                                        <asp:BoundColumn DataField="name" HeaderText="模块名" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="title" HeaderText="模块名"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="isSelect" HeaderText="isselect" Visible="False"></asp:BoundColumn>
                                        <asp:TemplateColumn>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="cbAll" runat="server" AutoPostBack="True" Text="全选" 　OnCheckedChanged="CheckAll" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSelect" runat="server" Text="选择"  />&nbsp;
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="ModuleType" HeaderText="模块类型" Visible="False"></asp:BoundColumn>
                                    </Columns>
                                    <HeaderStyle BackColor="#5398B5" CssClass="tabHead" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" ForeColor="Black" HorizontalAlign="Center" Height="28px" />
                                    <AlternatingItemStyle BackColor="#EEEEEE" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                    <PagerStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Right" Mode="NumericPages" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:DataGrid>
                                <script language="JavaScript" type="text/javascript">
			                    <!--
				                    tigra_tables('dgShow', 1, 0, '#F3F3F3', '#ffffff', '#F1F6F9', '#E0EFF2');
			                    // -->
			                    </script>
              </td>
            </tr>
      </table>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="right" style="height: 24px">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="确认"/>&nbsp;</td>
                        </tr>
      </table>
    </form>
</body>
</html>
