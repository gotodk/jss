<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YWGLBMCK.aspx.cs" Inherits="Web_JHJX_New2013_GXTW_YWGLBMCK" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" />
    <script src="../../../../js/jquery-1.7.2.min.js"></script>     
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js"></script>
</head>
<body onload="ShowUpdating()" style=" background-color:#f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="99%">
        <Tabs>
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="YWGLBMAdd.aspx" Text="业务管理部门添加"
                     Font-Size="12px">
                </radTS:Tab>                
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="YWGLBMCK.aspx" Text="业务管理部门查看"
                 Font-Size="12px" ForeColor="Red"></radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_bz" >
                    <%--说明文字--%>
                </div>
                <div class="content_nr" id="divLB" runat="server">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">                        
                        <tr>  
                            <td width="100px" align="right">
                                管理部门名称：
                            </td>
                            <td width="160px">
                            <asp:TextBox ID="txtGLBMMC" runat="server" CssClass="tj_input" Width="160px" Enabled="True"
                                    TabIndex="1" MaxLength="50"></asp:TextBox>                      
                            </td>
                            <td width="100px" align="center">
                                管理部门帐号：
                            </td>
                            <td width="160px">
                            <asp:TextBox ID="txtGLBMZH" runat="server" CssClass="tj_input" Width="160px" Enabled="True"
                                    TabIndex="1" MaxLength="50"></asp:TextBox>                        
                            </td> 
                            <td width="100px" align="right">
                                管理部门分类：
                            </td>
                            <td width="162px">
                                <asp:DropDownList ID="drpGLBMFL" runat="server" Width="162px" CssClass="tj_input">
                                    <asp:ListItem>全部</asp:ListItem>
                                    <asp:ListItem>高校团委</asp:ListItem>
                                </asp:DropDownList>                           
                            </td>  
                            <td  align="center" style=" padding-left:5px; width:140px;">
                                <asp:Label ID="Label1" runat="server" Text="正在查询中，请稍后..." Width="140px" style=" display:none;"></asp:Label>
                                <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="查询" Width="70px" OnClientClick="Click();" OnClick="BtnCheck_Click" />
                            </td>
                            <td  align="left" colspan="4" style=" padding-left:10px; ">
                            <%--<asp:Button ID="btnToExcel" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="导出" Width="70px" OnClick="btnToExcel_Click"/>--%>
                                </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                <%--说明文字--%>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                               <%--<div class="content_nr_lb" style="width:1110px; ">--%>
                                    <table id="theObjTable"  style="width:100%; " cellspacing="0"  cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style=" width:150px;">
                                                    管理部门分类
                                                </th>  
                                                <th class="TheadTh"  style=" width:200px; line-height:18px;">
                                                    管理部门名称</th>
                                                <th class="TheadTh" style=" width:200px;">
                                                    管理部门帐号</th>
                                                  <th class="TheadTh" style=" width:150px;">
                                                    管理部门密码</th>         
                                                <th class="TheadTh" style=" width:120px;">
                                                    是否有效</th>          
                                                <th class="TheadTh" style=" width:150px; line-height:18px;">
                                                    注册时间
                                                </th>
                                                <th class="TheadTh">操作</th>                                               
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" 
                                                >
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td style=" width:150px;" title='<%#Eval("GLBMFLMC") %>'>
                                                            <div style=" word-wrap:break-word; line-height:18px;  width:150px;">  <%#Eval("GLBMFLMC").ToString().Length > 20 ? Eval("GLBMFLMC").ToString().Substring(0, 20) + "..." : Eval("GLBMFLMC").ToString()%></div>                                             
                                                        </td>
                                                        <td style=" width:200px;" title='<%#Eval("GLBMMC") %>'>
                                                          <div style=" word-wrap:break-word; line-height:18px;  width:200px;">  <%#Eval("GLBMMC").ToString().Length > 14 ? Eval("GLBMMC").ToString().Substring(0, 25) + "..." : Eval("GLBMMC").ToString()%></div>   
                                                        </td>
                                                          <td>
                                                               <%#Eval("GLBMZH") %>
                                                        </td>
                                                        <td>   
                                                        <%#Eval("GLBMMM") %>
                                                        </td>
                                                        <td>
                                                           <%#Eval("SFYX")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("CreateTIme")%>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName='<%#Eval("Number") %>' CommandArgument="XG">修改</asp:LinkButton> 
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr id="ts" runat="server" class="TfootTr">
                                                <td colspan="7" align="center">
                                                当前数据为空！
                                                </td>
                                            </tr> 
                                        </tfoot>
                                    </table>
                                <%--</div>--%>
                                <uc1:commonpagernew ID="commonpagernew1" runat="server" />
                            </td>
                        </tr>
                    </table>

                    <div id="export" runat="server" style="width:100%; display:none;">
                        <table width="100%"  align="center" cellpadding="0" cellspacing="0" style="font-size: 13px;
                            font-family: 宋体; border-collapse:collapse;" border="1" bordercolor="#D4D4D4">                            
                            <thead>
                                <tr align="center" style="font-size: 16px; font-family: 宋体; word-break: break-all; height:30px;">
                                    <th colspan="10">未中标时间间隔监控列表</th>
                                </tr>
                                <tr>
                                    <th style=" width:80px; line-height:18px;">
                                        商品编号</th>
                                    <th style=" width:150px;">
                                        商品名称</th>
                                        <th style=" width:70px;">
                                        计价单位</th>         
                                    <th style=" width:120px;">
                                        最近一次中标日期</th>                                              
                                    <th style=" width:100px;">
                                        参与买家数量
                                    </th>  
                                    <th style=" width:100px; line-height:18px;">
                                        参与卖家数量
                                    </th>
                                    <th style=" width:100px; line-height:18px;">
                                        最高拟<br />订购价格</th>    
                                    <th style=" width:75px; line-height:18px;">
                                        最低拟<br />出售价格</th>
                                    <th style=" width:100px;">
                                        拟订购总量</th> 
                                    <th style=" width:100px;">
                                        拟出售总量</th>                                                         
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater2" runat="server" 
                                     >
                                    <ItemTemplate>
                                        <tr class="TbodyTr">
                                           <td style=" width:80px;" title='<%#Eval("商品编码") %>'>
                                                <div style=" word-wrap:break-word; line-height:18px;  width:80px;">  <%#Eval("商品编码").ToString().Length > 20 ? Eval("商品编码").ToString().Substring(0, 20) + "..." : Eval("商品编码").ToString()%></div>                                             
                                            </td>
                                            <td style=" width:150px;" title='<%#Eval("商品名称") %>'>
                                                <div style=" word-wrap:break-word; line-height:18px;  width:150px;">  <%#Eval("商品名称").ToString().Length > 14 ? Eval("商品名称").ToString().Substring(0, 14) + "..." : Eval("商品名称").ToString()%></div>   
                                            </td>
                                                <td>
                                                    <%#Eval("计价单位") %>
                                            </td>
                                            <td>   
                                            <%#Eval("最近一次中标日期") %>&nbsp;
                                            </td>
                                            <td>
                                                <%#Eval("参与买家数量")%>
                                            </td>
                                            <td>
                                                <%#Eval("参与卖家数量")%>
                                            </td>
                                            <td>
                                                <%#Eval("最高拟订购价格")%>
                                            </td>
                                                <td>
                                                <%#Eval("最低拟出售价格")%>
                                            </td>
                                                <td>
                                                <%#Eval("拟订购总量")%>
                                            </td>
                                                <td>
                                                <%#Eval("拟出售总量")%>
                                            </td>                                        
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                            <tfoot>
                                <tr id="exprestTD" runat="server"><td colspan="10" style=" text-align:center;">当前数据为空！</td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>

                </div>
                
                
                    
                </div>
            </div>
        
    </div>
    </form>
    <script lang="ja" type="text/javascript">
        function ShowUpdating() {
            if ($("#divLB").length > 0) {
                document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'block';            
            document.getElementById("Label1").style.display = 'none';
        }
    }
    function Click() {
        document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'none';
        document.getElementById("Label1").style.display = 'block';
    }
</script>
</body>

</html>
