<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YGSYDNDQXX.aspx.cs" Inherits="Web_YGSYDNDQXX" MaintainScrollPositionOnPostback="false"%>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="Infragistics2.WebUI.UltraWebGrid.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid" TagPrefix="igtbl" %>
<%@ Register src="pagerdemo/commonpager.ascx" tagname="commonpager" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" margin:0px;">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="970px">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" Text="员工使用电脑到期信息" Font-Size="12px" ForeColor="red" NavigateUrl="YGSYDNDQXX.aspx">
                </radTS:Tab>
            </Tabs>
    </radTS:RadTabStrip>
    <div style=" width:970px;">
        <table border="0" cellpadding="0" cellspacing="0" class="FormView" width="100%">
            <tr>
                <td width="100%" align="left" height="0px">
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" class="FormView" width="100%">
            <tr style=" height:30px;">
                <td align="right">
                    序号：
                </td>
                <td width="120px" align="center">
                    <asp:TextBox runat="server" Width="100px" ID="txtXH"></asp:TextBox>
                </td>
                <td width="60px" align="right">
                    使用人：
                </td>
                <td width="130px" align="center">
                    <asp:TextBox runat="server" Width="100px" ID="txtSYR"></asp:TextBox>
                    
                </td>                
                <td  align="center" width="60px">
                    <asp:Button ID="BtnCheck" runat="server" CssClass="button" Height="25px"
                        Text="查询" onclick="BtnCheck_Click"/>
                </td>
                <td  align="center" width="90px">
                    <asp:Button ID="btnCloseALL" runat="server" CssClass="button" Width="80px" Height="25px"
                        Text="全部关闭" onclick="btnCloseALL_Click" />
                </td>
                <td  align="center" width="110px">
                    <asp:Button ID="btnBack" runat="server" CssClass="button" Width="100px" Height="25px"
                        Text="返回查看页面" onclick="btnBack_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div id="export" runat="server">        
        <radG:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" Skin="Monochrome" Width="970px"
        AutoGenerateColumns="False" PageSize="15" 
            onitemcommand="RadGrid1_ItemCommand" >
        <HeaderStyle CssClass="HeaderStyle" />        
        
        <ExportSettings>
            <Pdf PageWidth="8.5in" PageRightMargin="" PageFooterMargin="" PageLeftMargin="" PageTopMargin=""
                PageHeight="11in" PageBottomMargin="" PageHeaderMargin=""></Pdf>
        </ExportSettings>
        <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条."
            PrevPageText="上一页"></PagerStyle>
        <MasterTableView DataKeyNames="Number" ItemStyle-VerticalAlign="Middle">
            <NoRecordsTemplate>
                没有找到任何数据。
            </NoRecordsTemplate>
            <Columns>   
                <radG:GridBoundColumn DataField="Number" HeaderText="序号" ItemStyle-Width="140"  ItemStyle-VerticalAlign="Middle">
                </radG:GridBoundColumn>         
                <radG:GridBoundColumn DataField="SYR" HeaderText="使用人" ItemStyle-Width="150" ItemStyle-VerticalAlign="Middle">
                </radG:GridBoundColumn>
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="110">
                    <HeaderTemplate>
                        内网
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="blbNW" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="20px" CommandName='<%#Eval("Number") %>' CommandArgument="nw" ><%#Eval("NW")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn> 
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="110">
                    <HeaderTemplate>
                        外网
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="blbWW" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="20px" CommandName='<%#Eval("Number") %>' CommandArgument="ww" ><%#Eval("WW")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn> 
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="110">
                    <HeaderTemplate>
                        USB
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="blbUSB" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="20px"  CommandName='<%#Eval("Number") %>' CommandArgument="usb"  ><%#Eval("USB")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn> 
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="110">
                    <HeaderTemplate>
                        光驱
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="blbGQ" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="20px"  CommandName='<%#Eval("Number") %>' CommandArgument="gq"  ><%#Eval("GQ")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn> 
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="110">
                    <HeaderTemplate>
                        管理员
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="blbDNGLY" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="20px"  CommandName='<%#Eval("Number") %>' CommandArgument="gly"  ><%#Eval("DNGLY")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn> 
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="130">
                    <HeaderTemplate>
                        操作
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="130px">
                            <tr style=" height:30px;">
                                <td style=" width:43px; border:0px;">
                                <asp:Button ID="btnGB" runat="server" CssClass="button" Text="关闭" CommandName='<%#Eval("Number") %>' CommandArgument="gb" />
                                </td>
                                <td style=" width:43px; border:0px;">
                                <asp:Button ID="btnDel" runat="server" CssClass="button" Text="删除" CommandName='<%#Eval("Number") %>' CommandArgument="del"/>
                                </td>
                                </tr>
                        </table>
                    </ItemTemplate>                    
                </radG:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </radG:RadGrid>
    <uc1:commonpager ID="commonpager1" runat="server" />
    </div>
    <div id="divUpdate" runat="server" visible="false"  onmousedown="mousedown(this)" style="display: block;
    border: 0px; left: 20px; top: 190px; width: 500px; background-color: #BFE3FC;
    position: absolute; z-index: 999; height:150px;" align="center">
        <table width="98%">
            <tr>
                <td style=" height:25px; text-align:left; font-size:14px;" colspan="2">
                    <b>序号：<label id="lblXH" runat="server"></label></b> 
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td align="right"><label id="lblDQTime" runat="server"></label>当前使用期限：</td>
                <td align="left">
                    <asp:TextBox ID="txtDQTime" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" width:130px;" align="right">状态：</td>
                <td align="left" >
                    <asp:DropDownList ID="drpZT" runat="server">
                        <asp:ListItem Value="开通">开通</asp:ListItem>
                        <asp:ListItem Value="关闭">关闭</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trTime" runat="server" style="background-color:white; height:30px;">
                <td align="right">使用时间：</td>
                <td align="left">
                    <div style="float: left; width:70px;">
                        <asp:DropDownList ID="drpNWTime" runat="server" AutoPostBack="true" 
                            onselectedindexchanged="drpNWTime_SelectedIndexChanged">
                            <asp:ListItem Value="请选择">请选择</asp:ListItem>
                            <asp:ListItem Value="长期">长期</asp:ListItem>
                            <asp:ListItem Value="短期">短期</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div style="width: 300px; padding-left: 5px; display: block; z-index:100;" valign="top" id="divNW"
                                                runat="server" visible="false">
                        <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                            ID="txtNWStart" runat="server" Width="100px"></asp:TextBox>
                        至
                        <asp:TextBox class="Wdate" onFocus="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                            ID="txtNWEnd" runat="server" Width="100px"></asp:TextBox>
                    </div>
                  
                </td>
            </tr>
            <tr>
                <td colspan="2" style=" height:40px;text-align:center;">
                <asp:Button ID="btnOK" runat="server" Width="60px" Height="25px" CssClass="Button"
                        Text="确定" onclick="btnOK_Click"/>&nbsp;&nbsp;
                    <asp:Button ID="btnQX" runat="server" Width="60px" Height="25px" CssClass="Button"
                        Text="取消" onclick="btnQX_Click"  />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script language="javascript" type="text/javascript">
        var x, y;
        function mousedown(obj) {
            obj.onmousemove = mousemove;
            obj.onmouseup = mouseup;

            oEvent = window.event ? window.event : event;
            x = oEvent.clientX;
            y = oEvent.clientY;
        }
        function mousemove() {
            oEvent = window.event ? window.event : event;
            var _top = oEvent.clientY - y + parseInt(this.style.top) + "px"; // oEvent.clientY - y  为div 上 移动的位置                                                                         //parseInt(this.style.top) 
            var _left = oEvent.clientX - x + parseInt(this.style.left) + "px";
            this.style.top = _top;
            this.style.left = _left;
            x = oEvent.clientX;
            y = oEvent.clientY
        }
        function mouseup() {
            this.onmousemove = null;
            this.onmouseup = null;
        }
    </script>
</body>
</html>
