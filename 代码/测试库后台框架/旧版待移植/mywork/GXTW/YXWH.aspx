<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YXWH.aspx.cs" Inherits="mywork_GXTW_YXWH" %>

<%@ Register Src="../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="dh.css" rel="stylesheet" />
<link href="Css/fwsite.css" rel="stylesheet" />
    <script src="js/My97DatePicker/WdatePicker.js"></script>
</head>
<body style=" text-align:left;" >
    <form id="form1" runat="server">
        
     <table width="100%" border="0" cellspacing="0" cellpadding="0" class="newstop">
        <tr>
            <td width="50" height="29" align="right">
                &nbsp;&nbsp;
            </td>
            <td align="left">
                您现在的位置：  院系维护 
            </td>
        </tr>
    </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" >
        <tr>
            <td width="50" height="29" align="right">
                &nbsp;&nbsp;
            </td>
            <td align="left">                
            </td>
        </tr>
    </table>
    <div class="content_nr" id="divLB" runat="server" style=" width:800px; margin-left:50px; text-align:left;">

                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc" >
                        <tr>
                            <td style=" width:80px; text-align:right;">
                                院系名称：
                            </td>
                            <td style=" width:178px;">
                                <asp:TextBox ID="txtYXMCAdd" runat="server" aa="aa" CssClass="tj_input" Width="178px"
                                    Enabled="True" TabIndex="1"></asp:TextBox>  
                            </td>
                            <td >
                                <asp:Button ID="btnAdd" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="添加" Width="70px" style=" margin-left:10px;" OnClick="btnAdd_Click" />
                                <asp:Button ID="btnSeave" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="保存" Width="70px" style=" margin-left:10px;" Visible="false" OnClick="btnSeave_Click" />
                                <asp:Label ID="lblupdate" runat="server" Text="Label" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>

                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj" >
                        <tr>
                            <td>
                                <%--说明文字--%>
                            </td>
                        </tr>
                    </table>

                    <table align="left" cellpadding="0" cellspacing="0" class="table_nr" style=" width:100%; margin-left:0px;">
                        <tr>
                            <td align="left" style="height: 25px" valign="bottom" class="title">
                                院系列表
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" border="1px" bordercolor="#D1CFCF" style="border-style: solid;
                                    border-collapse: collapse; text-align: center; width: 100%;">
                                    <thead>
                                        <tr style="background-color: #D6E3F3; font-weight: bold;">
                                            <td height="28" width="400px">
                                                院系名称
                                            </td>
                                            <td height="28" width="200px">
                                                院系添加时间
                                            </td>
                                            <td height="28" width="200px">
                                                操作
                                            </td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                            <ItemTemplate>
                                                <tr style="height: 28px;">
                                                    <td width="400px" title='<%#Eval("YXMC") %>'>
                                                        <asp:Label ID="lbljynr" runat="server" Text='<%#Eval("YXMC")%>'> </asp:Label>
                                                    </td>
                                                    <td width="200px" title='<%#Eval("CreateTime")%>'>
                                                        <asp:Label ID="lblJYZ" runat="server" Text='<%#Eval("CreateTime")%>'> </asp:Label>
                                                    </td>
                                                    <td width="200px">
                                                        <asp:LinkButton ID="btnZhiChi" runat="server" CommandName="Up" CommandArgument='<%#Eval("Number")+"&"+Eval("YXMC") %>'
                                                            Text="修改" CssClass="linkb"></asp:LinkButton>&nbsp;&nbsp;
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Del" CommandArgument='<%#Eval("Number") %>'
                                                            Text="删除" CssClass="linkb" OnClientClick="return confirm('您确定要删除此条数据吗？')"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                    <tfoot id="tempty" runat="server">
                                        <tr>
                                            <td colspan="3" class="auto-style1">
                                                <asp:Label ID="Label2" runat="server" Text="您所查询的数据为空！"></asp:Label>
                                            </td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc2:commonpager ID="commonpager1" runat="server" />
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
    </form>
</body>
    <%--<script lang="ja" type="text/javascript">
        function ShowUpdating() {
            if ($("#divLB").length > 0) {
                document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'block';
            document.getElementById("<%=btnAdd.ClientID %>").style.display = 'block';
            document.getElementById("Label1").style.display = 'none';
        }
    }
    function Click() {
        document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'none';
        document.getElementById("<%=btnAdd.ClientID %>").style.display = 'none';
        document.getElementById("Label1").style.display = 'block';
    }
</script>--%>
</html>
