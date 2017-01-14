<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YGSYDNXXCK.aspx.cs" Inherits="Web_YGSYDNXXCK" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"  %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style=" margin:0px;">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="970px">
            <Tabs>
                <radTS:Tab ID="Tab3" runat="server" Text="员工使用电脑信息新增" Font-Size="12px"
                    NavigateUrl="YGSYDNXXXZ.aspx">
                </radTS:Tab>
                <radTS:Tab ID="Tab1" runat="server" Text="员工使用电脑信息查看及修改" Font-Size="12px" ForeColor="red" NavigateUrl="YGSYDNXXCK.aspx">
                </radTS:Tab>
            </Tabs>
    </radTS:RadTabStrip>
    </div>
    <div style=" width:970px;">
        <table border="0" cellpadding="0" cellspacing="0" class="FormView" width="100%">
            <tr>
                <td width="100%" align="left" height="20px">
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" class="FormView" width="100%">
            <tr style=" height:30px;">
                <td width="50px" align="right">
                    部门：
                </td>
                <td width="70px" align="left">
                    <asp:DropDownList ID="drpSSBM" runat="server" Height="25px">
                    </asp:DropDownList>
                </td>
                <td width="60px" align="right">
                    使用人：
                </td>
                <td width="70px" align="center">
                    <asp:TextBox runat="server" Width="70px" ID="txtSYR"></asp:TextBox>
                    
                </td>
                <td width="30px" align="right">
                   IP：
                </td>
                <td width="220px" align="left">
                    <asp:TextBox ID="txtIP1" runat="server" Width="40px" onkeyup="value=value.replace(/[^0-9]/g,'')" 
                                        MaxLength="4"></asp:TextBox>
                    <span style=" font-size:14px; font-weight:bold;">.</span>
                    <asp:TextBox ID="txtIP2" runat="server" Width="40px" onkeyup="value=value.replace(/[^0-9]/g,'')" 
                                        MaxLength="4"></asp:TextBox>
                    <span style=" font-size:14px; font-weight:bold;">.</span>
                    <asp:TextBox ID="txtIP3" runat="server" Width="40px" onkeyup="value=value.replace(/[^0-9]/g,'')" 
                                        MaxLength="4"></asp:TextBox>
                    <span style=" font-size:14px; font-weight:bold;">.</span>
                    <asp:TextBox ID="txtIP4" runat="server" Width="40px" onkeyup="value=value.replace(/[^0-9]/g,'')" 
                                        MaxLength="4"></asp:TextBox>
                </td>
                <td width="40px" align="right">
                    内网：
                </td>
                <td width="70px" align="center">
                    <asp:DropDownList ID="drpNW" runat="server">
                        <asp:ListItem Value="">请选择</asp:ListItem>
                        <asp:ListItem Value="是">使用</asp:ListItem>
                        <asp:ListItem Value="否">未使用</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="40px" align="right">
                    外网：
                </td>
                <td width="70px" align="center">
                    <asp:DropDownList ID="drpWW" runat="server">
                        <asp:ListItem Value="">请选择</asp:ListItem>
                        <asp:ListItem Value="是">使用</asp:ListItem>
                        <asp:ListItem Value="否">未使用</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td  align="center">
                    <asp:Button ID="BtnCheck" runat="server" CssClass="button" Height="20px"
                        Text="查询" onclick="BtnCheck_Click"/>
                    <asp:Button ID="btnDCZD" runat="server" Text="选择导出字段"  CssClass="button" Height="20px" Width="100px" onclick="btnDCZD_Click"/>
                </td>
            </tr>
        </table>
    </div>
    <div id="export" runat="server">
        <div id="Div1" style="display: none"   runat="server">
        <asp:GridView ID="GV_DC" runat="server" Width="970px" AutoGenerateColumns="False"
            AllowPaging="false" CssClass="GridViewStyle" EnableModelValidation="True" >
            <EmptyDataTemplate>
                暂无已保存的有效地址
            </EmptyDataTemplate>
            <HeaderStyle CssClass="HeaderStyle" />
            <RowStyle CssClass="RowStyle" HorizontalAlign="center" />
            <AlternatingRowStyle CssClass="AltRowStyle" />
            <Columns>
                <asp:BoundField DataField="CreateTime" HeaderText="填写时间" />
                <asp:BoundField DataField="Number" HeaderText="序号" />
                <asp:BoundField DataField="SSBM" HeaderText="部门" />
                <asp:BoundField DataField="SYR" HeaderText="使用人" />
                <asp:BoundField DataField="IPDZ" HeaderText="IP" />
                <asp:BoundField DataField="MACDZ" HeaderText="MAC" />
                <asp:BoundField DataField="PZXX" HeaderText="配置" />
                <asp:BoundField DataField="NW" HeaderText="内网" />
                <asp:BoundField DataField="NWSYQX" HeaderText="内网使用期限" />
                <asp:BoundField DataField="WW" HeaderText="外网" />
                <asp:BoundField DataField="WWSYQX" HeaderText="外网使用期限" />
                <asp:BoundField DataField="USB" HeaderText="USB" />
                <asp:BoundField DataField="USBSYQX" HeaderText="USB使用期限" />
                <asp:BoundField DataField="GQ" HeaderText="光驱" />
                <asp:BoundField DataField="GQSYQX" HeaderText="光驱使用期限" />
                <asp:BoundField DataField="DNGLY" HeaderText="管理员" />
                <asp:BoundField DataField="DNGLYSYQX" HeaderText="管理员使用期限" />
                <asp:BoundField DataField="ZHYCGXR" HeaderText="最后一次更新人" />
                <asp:BoundField DataField="ZHYCGXSJ" HeaderText="最后一次更新时间" />
                <asp:BoundField DataField="ZHYCZCZYSJ" HeaderText="最后一次资产转移时间" /> 
                <asp:BoundField DataField="CreateUser" HeaderText="创建人" />  
                <asp:BoundField DataField="BZ" HeaderText="备注" />
            </Columns>
        </asp:GridView>
        </div>
        <radG:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" Skin="Monochrome" Width="970px"
        AutoGenerateColumns="False" PageSize="15" 
        onitemcommand="RadGrid1_ItemCommand">
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
                <radG:GridBoundColumn DataField="CreateTime" HeaderText="填写时间" ItemStyle-Width="120" ItemStyle-VerticalAlign="Middle" >
                </radG:GridBoundColumn>
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="90">
                    <HeaderTemplate>
                        序号
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName='<%#Eval("Number") %>' CommandArgument="ck" ForeColor="Blue"><%#Eval("Number")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn>
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="100">
                    <HeaderTemplate>
                        部门
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName='<%#Eval("Number") %>' CommandArgument="zczy" ForeColor="Blue"><%#Eval("SSBM")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn>              
                <radG:GridBoundColumn DataField="SYR" HeaderText="使用人" ItemStyle-Width="70" ItemStyle-VerticalAlign="Middle">
                </radG:GridBoundColumn>
                <radG:GridBoundColumn DataField="IPDZ" HeaderText="IP" ItemStyle-Width="50" ItemStyle-VerticalAlign="Middle">
                </radG:GridBoundColumn>
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="50">
                    <HeaderTemplate>
                        内网
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="blbNW" runat="server" Font-Size="20px" Font-Bold="true" CommandName='<%#Eval("Number") %>' CommandArgument="nw" ForeColor='<%#Eval("NW").ToString()=="√"?System.Drawing.Color.Green:System.Drawing.Color.Red%>'><%#Eval("NW")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn> 
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="50">
                    <HeaderTemplate>
                        外网
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="blbWW" runat="server" Font-Size="20px" Font-Bold="true"  ForeColor='<%#Eval("WW").ToString()=="√"?System.Drawing.Color.Green:System.Drawing.Color.Red%>' CommandName='<%#Eval("Number") %>' CommandArgument="ww"><%#Eval("WW")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn> 
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="50">
                    <HeaderTemplate>
                        USB
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="blbUSB" runat="server" Font-Size="20px" Font-Bold="true"  ForeColor='<%#Eval("USB").ToString()=="√"?System.Drawing.Color.Green:System.Drawing.Color.Red%>' CommandName='<%#Eval("Number") %>' CommandArgument="usb" ><%#Eval("USB")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn> 
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="50">
                    <HeaderTemplate>
                        光驱
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="blbGQ" runat="server" Font-Size="20px" Font-Bold="true"  ForeColor='<%#Eval("GQ").ToString()=="√"?System.Drawing.Color.Green:System.Drawing.Color.Red%>' CommandName='<%#Eval("Number") %>' CommandArgument="gq" ><%#Eval("GQ")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn> 
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="50">
                    <HeaderTemplate>
                        管理员
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="blbDNGLY" runat="server" Font-Size="20px" Font-Bold="true"  ForeColor='<%#Eval("DNGLY").ToString()=="√"?System.Drawing.Color.Green:System.Drawing.Color.Red%>' CommandName='<%#Eval("Number") %>' CommandArgument="gly" ><%#Eval("DNGLY")%></asp:LinkButton>
                    </ItemTemplate>
                </radG:GridTemplateColumn> 
                <radG:GridTemplateColumn UniqueName="TemplateColumn" ItemStyle-Width="170">
                    <HeaderTemplate>
                        操作
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="170px">
                            <tr style=" height:30px;">
                                <td style=" width:43px; border:0px;">
                                <asp:Button ID="btnXG" runat="server" CssClass="button" Text="修改" Width="40px" CommandName='<%#Eval("Number") %>' CommandArgument="xg" />
                                </td>
                                <td style=" width:43px; border:0px;">
                                <asp:Button ID="btnDel" runat="server" CssClass="button" Text="删除" Width="40px" CommandName='<%#Eval("Number") %>' CommandArgument="del"/>
                                </td>
                                <td style=" width:43px; border:0px;">
                                <asp:Button ID="btnCKXQ" runat="server" CssClass="button" Text="查看详情" Width="80px" CommandName='<%#Eval("Number") %>' CommandArgument="ckxq"/>
                                </td>
                                </tr>
                        </table>
                    </ItemTemplate>
                </radG:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </radG:RadGrid>
    </div>
    <div id="divZCZY" runat="server" visible="false" onmousedown="mousedown(this)" style="display: block;
        border: 0px; left: 20px; top: 190px; width: 350px; background-color: #BFE3FC;
        position: absolute; z-index: 999; height:150px;" align="center">
        <table width="100%">
            <tr>
                <td colspan="2" style=" height:20px; text-align:left; font-size:14px;"><b>
                    资产转移：</b>
                    <asp:Label ID="lblZCZY" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td>
                    转移前所在部门：
                </td>
                <td>
                    <asp:TextBox ID="txtZYQ" runat="server" Enabled="false" Width="150px"></asp:TextBox>                    
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td>
                    转移后所在部门：
                </td>
                <td>
                    <asp:DropDownList ID="drpZYH" runat="server" Width="160px" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" style=" height:50px;">
                    <asp:Button ID="OKButton" runat="server" Width="60px" Height="25px" CssClass="Button"
                        Text="确定" onclick="OKButton_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Width="60px" Height="25px" CssClass="Button"
                        Text="取消" onclick="btnCancel_Click"  />
                </td>
            </tr>
        </table>

     </div>
     <div id="divZCZYCK" runat="server" visible="false" onmousedown="mousedown(this)" style="display: block;
        border: 0px; left: 20px; top: 190px; width: 600px; background-color: #BFE3FC;
        position: absolute; z-index: 999; height:auto;" align="center">
        <table cellpadding="0px" cellspacing="0px" border="0">
            <tr>
                <td style=" height:30px; text-align:left; font-size:14px;">
                    <b>资产转移历史记录：</b><asp:Label ID="lblFXH" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <radG:RadGrid ID="RadGrid2" runat="server" AllowPaging="True" GridLines="None" Skin="Monochrome"
                        PageSize="10" Width="580px" onpageindexchanged="RadGrid2_PageIndexChanged">
                        <PagerStyle NextPageText="下一页" HorizontalAlign="right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条."
                            PrevPageText="上一页"></PagerStyle>      
                          
                            <ItemStyle HorizontalAlign="Left"  />    
                            <AlternatingItemStyle HorizontalAlign="Left"  />       
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id">
                            <NoRecordsTemplate>
                                没有找到任何数据。
                            </NoRecordsTemplate>
                            <Columns>
                                <radG:GridBoundColumn DataField="ZCZYSJ" HeaderText="资产转移时间" SortExpression="ZCZYSJ"
                                    UniqueName="ZCZYSJ" ItemStyle-Width="250px">                                
                                </radG:GridBoundColumn>
                                <radG:GridBoundColumn DataField="ZCZYQSSBM" HeaderText="资产转移前所属部门" SortExpression="ZCZYQSSBM"
                                    UniqueName="ZCZYQSSBM" ItemStyle-Width="200px">                                 
                                </radG:GridBoundColumn>
                                <radG:GridBoundColumn DataField="ZCZYHSSBM" HeaderText="资产转移后所属部门" SortExpression="ZCZYHSSBM"
                                    UniqueName="ZCZYHSSBM" ItemStyle-Width="200px">                                 
                                </radG:GridBoundColumn>
                                <radG:GridBoundColumn DataField="CZR" HeaderText="操作人" SortExpression="CZR"
                                    UniqueName="CZR" ItemStyle-Width="140px">                                 
                                </radG:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </radG:RadGrid>
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 40px;">
                    <asp:Button ID="btnGB" runat="server" Width="100px" Height="25px" CssClass="Button"
                        Text="取消" onclick="btnGB_Click1"/>
                </td>
            </tr>
        </table>
     </div>
    <div id="divSYQX" runat="server" visible="false" onmousedown="mousedown(this)" style="display: block;
    border: 0px; left: 20px; top: 190px; width: 350px; background-color: #BFE3FC;
    position: absolute; z-index: 999; height:auto;" align="center">
        <table width="100%">
            <tr>
                <td colspan="2" style=" height:20px; text-align:left; font-size:14px;">
                    <b><asp:Label ID="lblXH" runat="server" Text="Label"></asp:Label></b>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    <asp:Label ID="lblSYQX" runat="server" Text="Label"></asp:Label>使用期限：                 
                </td>
                <td>
                    <asp:TextBox ID="txtSYQX" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style=" height:50px;"colspan="2">
                    <asp:Button ID="btnSYQX" runat="server" Width="100px" Height="25px" CssClass="Button"
                        Text="取消" onclick="btnSYQX_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divCKXQ" runat="server" visible="false"  onmousedown="mousedown(this)" style="display: block;
    border: 0px; left: 20px; top: 190px; width: 600px; background-color: #BFE3FC;
    position: absolute; z-index: 999; height:auto;" align="center">
        <table width="100%">
            <tr>
                <td style=" height:30px; text-align:left; font-size:14px;" colspan="4">
                    <b>电脑使用详情：</b><asp:Label ID="lblSYXQ" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right; width:150px;">
                    填写时间：
                </td>
                <td style=" width:165px;">
                    <asp:Label ID="lbltxsj" runat="server" Text="Label"></asp:Label>
                </td>
                <td style=" text-align:right; width:120px;">
                    序号：
                </td>
                <td style=" width:165px;">
                    <asp:Label ID="lblXQxh" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    所属部门：
                </td>
                <td>
                    <asp:Label ID="lblssbm" runat="server" Text="Label"></asp:Label>
                </td>
                <td style=" text-align:right;">
                    使用人：
                </td>
                <td>
                    <asp:Label ID="lblXQsyr" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    IP地址：
                </td>
                <td>
                    <asp:Label ID="lblIP" runat="server" Text="Label"></asp:Label>
                </td>
                <td style=" text-align:right;">
                    MAC地址：
                </td>
                <td>
                    <asp:Label ID="lblMAC" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    配置信息：
                </td>
                <td colspan="3" align="left">
                    <asp:Label ID="lblPZ" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    内网：
                </td>
                <td>
                    <asp:Label ID="lblNW" runat="server" Text="Label"></asp:Label>
                </td>
                <td style=" text-align:right;">
                    内网使用期限：
                </td>
                <td>
                    <asp:Label ID="lblNWTime" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    外网：
                </td>
                <td>
                    <asp:Label ID="lblWW" runat="server" Text="Label"></asp:Label>
                </td>
                <td style=" text-align:right;">
                    外网使用期限：
                </td>
                <td>
                    <asp:Label ID="lblWWTime" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    USB：
                </td>
                <td>
                    <asp:Label ID="lblUSB" runat="server" Text="Label"></asp:Label>
                </td>
                <td style=" text-align:right;">
                    USB使用期限：
                </td>
                <td>
                    <asp:Label ID="lblUSBTime" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    光驱：
                </td>
                <td>
                    <asp:Label ID="lblGQ" runat="server" Text="Label"></asp:Label>
                </td>
                <td style=" text-align:right;">
                    光驱使用期限：
                </td>
                <td>
                    <asp:Label ID="lblGQTime" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    管理员：
                </td>
                <td>
                    <asp:Label ID="lblGLY" runat="server" Text="Label"></asp:Label>
                </td>
                <td style=" text-align:right;">
                    管理员使用期限：
                </td>
                <td>
                    <asp:Label ID="lblGLYTime" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    创建人：
                </td>
                <td>
                    <asp:Label ID="lblCreateUser" runat="server" Text="Label"></asp:Label>
                </td>
                <td style=" text-align:right;">
                    创建时间：
                </td>
                <td>
                    <asp:Label ID="lblCreateTime" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    最后一次更新人：
                </td>
                <td>
                    <asp:Label ID="lblLastUpdateUser" runat="server" Text="Label"></asp:Label>
                </td>
                <td style=" text-align:right; width:110px;">
                    最后一次更新时间：
                </td>
                <td>
                    <asp:Label ID="lblLastUpdateTime" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right; width:140px;">
                    最后一次资产转移时间：
                </td>
                <td colspan="3">
                    <asp:Label ID="lblLastZYTime" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    备注：
                </td>
                <td colspan="3" align="left">
                    <asp:Label ID="lblBZ" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style=" height:50px;"colspan="4">
                    <asp:Button ID="btnXQQX" runat="server" Width="100px" Height="25px" CssClass="Button"
                        Text="取消" onclick="btnXQQX_Click"  />
                </td>
            </tr>
         </table>
    </div>
    <div id="divDCZD" runat="server" visible="false"  onmousedown="mousedown(this)" style="display: block;
    border: 0px; left: 20px; top: 190px; width: 400px; background-color: #BFE3FC;
    position: absolute; z-index: 999; height:auto;" align="center">
        <table width="98%">
            <tr>
                <td style=" height:25px; text-align:left; font-size:14px;" colspan="4">
                    <b>选择导出字段：</b> 
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right; width:150px;">
                    填写时间：
                </td>
                <td style=" width:65px;">
                    <input id="chkCreateTime" type="checkbox" runat="server" value="0" />
                </td>
                <td style=" text-align:right; width:120px;">
                    序号：
                </td>
                <td style=" width:65px;">
                    <input id="chkNumber" type="checkbox" runat="server" value="1" />
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    所属部门：
                </td>
                <td>
                    <input id="chkSSBM" type="checkbox" runat="server" value="2" />
                </td>
                <td style=" text-align:right;">
                    使用人：
                </td>
                <td>
                    <input id="chkSYR" type="checkbox" runat="server" value="3" />
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    IP地址：
                </td>
                <td>
                    <input id="chkIP" type="checkbox" runat="server" value="4" />
                </td>
                <td style=" text-align:right;">
                    MAC地址：
                </td>
                <td>
                    <input id="chkMAC" type="checkbox" runat="server" value="5" />
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    配置信息：
                </td>
                <td >
                    <input id="chkPZ" type="checkbox" runat="server" value="6" />
                </td>
                <td style=" text-align:right;">
                    创建人：
                </td>
                <td>
                    <input id="chkCreateUser" type="checkbox" runat="server" value="20" />
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    内网：
                </td>
                <td>
                    <input id="chkNW" type="checkbox" runat="server" value="7" />
                </td>
                <td style=" text-align:right;">
                    内网使用期限：
                </td>
                <td>
                    <input id="chkNWTime" type="checkbox" runat="server" value="8" />
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    外网：
                </td>
                <td>
                    <input id="chkWW" type="checkbox" runat="server" value="9" />
                </td>
                <td style=" text-align:right;">
                    外网使用期限：
                </td>
                <td>
                    <input id="chkWWTime" type="checkbox" runat="server" value="10" />
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    USB：
                </td>
                <td>
                    <input id="chkUSB" type="checkbox" runat="server" value="11" />
                </td>
                <td style=" text-align:right;">
                    USB使用期限：
                </td>
                <td>
                    <input id="chkUSBTime" type="checkbox" runat="server" value="12" />
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    光驱：
                </td>
                <td>
                    <input id="chkGQ" type="checkbox" runat="server" value="13" />
                </td>
                <td style=" text-align:right;">
                    光驱使用期限：
                </td>
                <td>
                    <input id="chkGQTime" type="checkbox" runat="server" value="14" />
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    管理员：
                </td>
                <td>
                    <input id="chkGLY" type="checkbox" runat="server" value="15" />
                </td>
                <td style=" text-align:right;">
                    管理员使用期限：
                </td>
                <td>
                    <input id="chkGLYTime" type="checkbox" runat="server" value="16" />
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right;">
                    最后一次更新人：
                </td>
                <td>
                    <input id="chkLastUpateUser" type="checkbox" runat="server" value="17" />
                </td>
                <td style=" text-align:right; width:110px;">
                    最后一次更新时间：
                </td>
                <td>
                    <input id="chkLastUpateTime" type="checkbox" runat="server" value="18" />
                </td>
            </tr>
            <tr style=" background-color:white; height:25px;">
                <td style=" text-align:right; width:140px;">
                    最后一次资产转移时间：
                </td>
                <td >
                    <input id="chkLastZCZYTime" type="checkbox" runat="server" value="19" />
                </td>
                <td>
                    备注：
                </td>
                <td >
                    <input id="chkBZ" type="checkbox" runat="server" value="21" />
                </td>
            </tr>            
            <tr>
                <td style="text-align:left;"colspan="2">
                    <input id="chkALL" type="checkbox" runat="server" onchange="" />全选
                </td>
                <td style=" height:40px; text-align:right;">
                        <asp:Button ID="btnExport" runat="server" Width="60px" Height="25px" CssClass="Button"
                        Text="导出" onclick="btnExport_Click"/>
                </td>
                <td style="text-align:left;">
                    <asp:Button ID="btnQX" runat="server" Width="60px" Height="25px" CssClass="Button"
                        Text="取消" onclick="btnQX_Click"  />
                </td>
            </tr>
         </table>
    </div>
    <div id="divDQ" runat="server" visible="false" onmousedown="mousedown(this)" style="display: block;
        border: 1px solid blue; left: 20px; top: 190px; width: 390px; background-color: #BFE3FC;
        position: absolute; z-index: 999; height:100px;" align="center">
        <table width="99%" style=" height:99%;">            
            <tr>
                <td>
                    <a href="YGSYDNDQXX.aspx" style=" font-size:large; color:Red; cursor:pointer; font-weight:bold;">有员工使用电脑信息即将到期，请进行处理。</a>
                    <br />
                    <br />
                    <asp:Button ID="btnDQCL" runat="server" Width="60px" Height="25px" CssClass="Button"
                        Text="处理" onclick="btnDQCL_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnDQCancle" runat="server" Width="60px" Height="25px" CssClass="Button"
                        Text="取消" onclick="btnDQCancle_Click"  />
                </td>
            </tr>
        </table>
    </div>
    <uc1:commonpager ID="commonpager1" runat="server" />
    </form>
    <script src="../js/jquery-1.7.2.min.js" type="text/javascript"></script>
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
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            /*全选与非全选
            例：<input id="checkboxAll"  type="checkbox" value="全选" />
            只需将input控件id更改为checkboxAll      */
            $("#chkALL").click(function () {
                if ($(this).attr("checked") == "checked") {
                    $('input:checkbox').each(function () {
                        $(this).attr("checked", true);
                    });
                }
                else {
                    $('input:checkbox').each(function () {
                        $(this).attr("checked", false);
                    });
                }
            });
        });
    </script>
</body>
</html>
