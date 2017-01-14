<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ZZRY_Report.aspx.cs" Inherits="Web_HR_RYYD_ZZRY_Report" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>人员异动信息查询</title>
    <link type="text/css" rel="Stylesheet" href="/css/style.css" />
    <script src="/js/setday.js" type="text/javaScript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Default2006" Height="16px"
            Width="980px">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" Text="入职人员信息查询" NavigateUrl="RZRY_Report.aspx">
                </radTS:Tab>
                <radTS:Tab ID="Tab2" runat="server" Text="转正人员信息查询" NavigateUrl="ZZRY_Report.aspx"
                    ForeColor="Red">
                </radTS:Tab>
                <radTS:Tab ID="Tab3" runat="server" Text="调岗人员信息查询" NavigateUrl="TGRY_Report.aspx">
                </radTS:Tab>
                <radTS:Tab ID="Tab4" runat="server" Text="离职人员信息查询" NavigateUrl="LZRY_Report.aspx">
                </radTS:Tab>
                <radTS:Tab ID="Tab5" runat="server" Text="改签劳动合同信息查询" NavigateUrl="GQLDHT_Report.aspx">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <br />
        <br />
        <table border="0" cellpadding="5" cellspacing="0" class="FormView">
            <tr>
                <td width="60" align="right">
                    转正时间:
                </td>
                <td width="10" align="center">
                    从
                </td>
                <td width="70" align="center">
                    <input runat="server" type="text" id="kssj" name="time1" style="width: 70px" onfocus="setday(this)"
                        class="input1" onblur=" return closeDateWindow();" />
                </td>
                <td width="10" align="center">
                    到
                </td>
                <td width="70" align="center">
                    <input runat="server" type="text" id="jssj" name="time2" style="width: 70px" onfocus="setday(this)"
                        class="input1" onblur=" return closeDateWindow();" />
                </td>
                <td width="60" align="right">
                    隶属:
                </td>
                <td width="120" align="left">
                    <asp:DropDownList ID="ddlLS" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlLS_SelectedIndexChanged">
                       <asp:ListItem Value ="">全部</asp:ListItem>
                        <asp:ListItem Value="公司总部">公司总部</asp:ListItem>
                        <asp:ListItem Value="驻外机构">办事处</asp:ListItem>
                        <asp:ListItem Value="百仕加">百仕加</asp:ListItem>
                        <asp:ListItem Value="董事会/总裁">董事会/总裁</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="50" align="right">
                    部门:
                </td>
                <td width="150" align="left">
                    <asp:DropDownList ID="ddlBM" runat="server" Width="150px">
                    </asp:DropDownList>
                </td>
                <td width="80" align="right" rowspan="2" valign="middle">
                    <asp:Button ID="Button2" runat="server" Text="查看结果" Width="80px" OnClick="Button1_Click1" />
                </td>
                <td width="80" align="left" rowspan="2" valign="middle">
                    <asp:Button ID="Button4" runat="server" OnClick="Button3_Click" Text="导出到Excel" Width="80px" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                 <div id="divDaoChu" runat="server">
                        <table id="tbtitle" runat ="server" visible ="false" width="100%" cellpadding="0" cellspacing="0" border="1">
                            <tr>
                                <td colspan="9" align="center" valign="middle" style="font-size: 10pt; font-weight: bold;
                                    height: 30px">
                                    <asp:Label ID="lbltitle" runat="server" Width="100%" Style="text-align: center"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                        EnableModelValidation="True">
                        <HeaderStyle Font-Size="10pt" Height="25px" BorderStyle="Solid" BorderColor="Black"
                            BackColor="LightGray" />
                        <RowStyle Font-Size="9pt" Height="25px" HorizontalAlign="Center" VerticalAlign="Middle"
                            BorderStyle="Solid" BorderColor="Black" />
                        <Columns>
                         <asp:BoundField DataField="序号" HeaderText="序号" ItemStyle-Width="50px"></asp:BoundField>
                            <asp:BoundField DataField="员工工号" HeaderText="员工工号" ItemStyle-Width="100px"></asp:BoundField>
                            <asp:BoundField DataField="员工姓名" HeaderText="员工姓名" ItemStyle-Width="100px"></asp:BoundField>
                            <asp:BoundField DataField="部门" HeaderText="部门" ItemStyle-Width="100px"></asp:BoundField>
                            <asp:BoundField DataField="岗位" HeaderText="岗位" ItemStyle-Width="150px"></asp:BoundField>
                            <asp:BoundField DataField="转正形式" HeaderText="转正形式" ItemStyle-Width="100px"></asp:BoundField>
                            <asp:BoundField DataField="转正月份" HeaderText="转正月份" ItemStyle-Width="100px"></asp:BoundField>
                            <asp:BoundField DataField="转正时间" HeaderText="转正时间" ItemStyle-Width="100px"></asp:BoundField>
                            <asp:BoundField DataField="备注" HeaderText="备注" ItemStyle-Width="130px"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    </div> 
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
