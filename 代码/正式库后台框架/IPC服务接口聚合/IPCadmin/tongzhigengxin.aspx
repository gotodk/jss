<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tongzhigengxin.aspx.cs" Inherits="IPCadmin_tongzhigengxin" %>

<%@ Register Src="wuccaidan.ascx" TagName="wuccaidan" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>IPC管理中心--通知接口更新和编译代理类</title>
    <link href="css.css" type="text/css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:wuccaidan ID="wuccaidan1" runat="server" />
            <br />
            <strong>通过IP与端口执行,批量通知被选中的接口域名下的正常负载的所有IP地址，更新代理类：</strong><br />

            <br />
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left" valign="middle">排序:</td>
                    <td align="left" valign="middle">
                        <asp:TextBox ID="TextBox1" runat="server" Width="377px">order by IP.IP_JK_host asc, IP.IP_address asc </asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp; 条件:</td>
                    <td align="left" valign="middle">
                        <asp:TextBox ID="TextBox2" runat="server" Width="368px">where IP.IP_open='正常负载' and IP.IP_zt='1'</asp:TextBox></td>
                    <td align="left" valign="middle">&nbsp;&nbsp;<asp:Button ID="btnView" runat="server" Text="查 询" OnClick="btnView_Click" />&nbsp;&nbsp;
                        <asp:Button ID="Button2" runat="server" Text="刷 新" OnClick="Button2_Click" /></td>
                    <td align="left" valign="middle">&nbsp;</td>
                    <td align="left" valign="middle">&nbsp;</td>
                </tr>
            </table>
            <br />
            <br />
            <div id="begintongzhi">
                 <asp:Button ID="Button1" runat="server" Text="开始批量通知" Height="24px" Width="400px" OnClick="Button1_Click" OnClientClick="$('#begintongzhi').hide(500);$('#bload').show(500);"/>
 
 
            </div>
            <div id="bload" style=" display:none; color:red;">正在进行通知，挺慢，等待……</div>
           
            <br />
                        <div runat="server" id="xiazaiguanxi" style="line-height:25px">

                    
                                </div>
            <br />
            <div runat="server" id="showmsg"></div>
            <br />
            <strong>有效负载IP地址列表：</strong>
              <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" HorizontalAlign="Left" OnRowDataBound="GridView1_RowDataBound" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="选择" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="IP_chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="IP_chkAll_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="IP_chkItem" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="接口域名" HeaderText="接口域名" />
                    <asp:BoundField DataField="IP地址" HeaderText="IP地址" />                    
                    <asp:BoundField DataField="接口备用端口" HeaderText="接口备用端口" />
                    <asp:BoundField DataField="IP地址是否有效" HeaderText="IP地址是否有效" />
                    <asp:BoundField DataField="IP地址状态" HeaderText="IP地址状态" />
                    <asp:BoundField DataField="备注" HeaderText="备注" />
                    <asp:BoundField DataField="IP添加时间" HeaderText="添加时间" />
                    <asp:BoundField DataField="IP最后修改时间" HeaderText="最后修改时间" />
                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                <RowStyle BackColor="White" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="Gray" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>  
             
        </div>
        
    </form>
</body>
</html>

