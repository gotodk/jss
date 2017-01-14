<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CSSPZLSH_YSH.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_CSSPZLSH_CSSPZLSH_YSH" %>

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
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
</head>
<body onload="ShowUpdating()" style=" background-color:#f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="99%">
        <Tabs>
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="CSSPZLSH_WSH.aspx" Text="未审核"
                 Font-Size="12px">
            </radTS:Tab>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="CSSPZLSH_YSH.aspx" Text="已审核"
                Font-Size="12px" ForeColor="Red">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_bz" >
                    <%--说明文字--%>
                </div>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">                        
                        <tr>
                            <td width="70px" align="right">
                                <span>申请时间</span>：
                            </td>
                            <td width="178px">
                            <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="txtBeginTime" runat="server" Width="178px"></asp:TextBox>                            
                            </td>
                            <td width="20px" align="center">
                                至
                            </td>
                            <td width="178px">
                            <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="txtEndTime" runat="server" Width="178px"></asp:TextBox>                            
                            </td>  
                            <td  align="right" width="80px">
                                交易方账号：
                            </td>
                            <td width="130px">
                                <asp:TextBox ID="txtJYFZH" runat="server" Width="130px" CssClass="tj_input"></asp:TextBox>                       
                            </td>
                            <td width="80px" align="center">
                                交易方名称：
                            </td>
                            <td width="130px">
                            <asp:TextBox ID="txtJYFMC" runat="server" Width="130px" CssClass="tj_input"></asp:TextBox>                        
                            </td>   
                             
                            <td  align="center" style=" padding-left:5px;">
                                
                            </td>
                        </tr>
                        <tr>
                            <td width="70px" align="right">
                                <span>审核时间</span>：
                            </td>
                            <td width="178px">
                            <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="SHTimeStart" runat="server" Width="178px"></asp:TextBox>                            
                            </td>
                            <td width="20px" align="center">
                                至
                            </td>
                            <td width="178px">
                            <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="SHTimeEnd" runat="server" Width="178px"></asp:TextBox>                            
                            </td> 
                            <td  align="right">
                                审核人：
                            </td>
                            <td width="130px">
                                <asp:TextBox ID="txtSHR" runat="server" Width="130px" CssClass="tj_input"></asp:TextBox>                       
                            </td>  
                            <td align="center">                                
                            </td> 
                            <td  align="left" style="width:120px;">
                                <asp:Label ID="Label1" runat="server" Text="正在查询中，请稍后..." Width="120px"></asp:Label>
                                <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="查询" Width="70px" OnClientClick="Click();" OnClick="BtnCheck_Click" />
                            </td>
                            <td  align="left">
                            <asp:Button ID="btnToExcel" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="导出" Width="70px" OnClick="btnToExcel_Click"/>
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
                                                <th class="TheadTh"  style=" width:100px; line-height:18px;">
                                                    交易方账号</th>
                                                <th class="TheadTh" style=" width:140px;">
                                                    交易方名称</th>
                                                  <th class="TheadTh" style=" width:120px;">
                                                    申请时间</th> 
                                                <th class="TheadTh" style=" width:75px;">
                                                    商品编号
                                                </th>  
                                                <th class="TheadTh" style=" width:150px; line-height:18px;">
                                                    商品名称
                                                </th>   
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    是否变更<br />资质或标准</th>    
                                                <th class="TheadTh" style=" width:75px;">
                                                    是否有效</th>     
                                                <th class="TheadTh" style=" width:75px;">
                                                    审核状态</th>                                             
                                                <th class="TheadTh" style=" width:120px;">
                                                    审核时间</th>
                                                <th class="TheadTh" style=" width:70px;">
                                                    审核人</th>   
                                                <th class="TheadTh">操作</th>                                               
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" 
                                                >
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td style=" width:100px;" title='<%#Eval("交易方账号") %>'>
                                                            <div style=" word-wrap:break-word; line-height:18px;  width:100px;">  <%#Eval("交易方账号").ToString().Length > 10 ? Eval("交易方账号").ToString().Substring(0, 10) + "..." : Eval("交易方账号").ToString()%></div>                                             
                                                        </td>
                                                        <td style=" width:140px;" title='<%#Eval("交易方名称") %>'>
                                                          <div style=" word-wrap:break-word; line-height:18px;  width:140px;">  <%#Eval("交易方名称").ToString().Length > 10 ? Eval("交易方名称").ToString().Substring(0, 10) + "..." : Eval("交易方名称").ToString()%></div>   
                                                        </td>
                                                          <td>
                                                               <%#Eval("申请时间") %>
                                                        </td>
                                                        <td>
                                                           <%#Eval("商品编号")%>
                                                        </td>
                                                        <td style=" width:150px;" title='<%#Eval("商品名称") %>'>
                                                            <div style=" word-wrap:break-word; line-height:18px;  width:150px;"><%#Eval("商品名称").ToString().Length > 10 ? Eval("商品名称").ToString().Substring(0, 10) + "..." : Eval("商品名称").ToString()%> </div>
                                                        </td>
                                                        <td>   
                                                        <%#Eval("是否变更资质或标准") %>
                                                        </td> 
                                                        <td>   
                                                        <%#Eval("是否有效") %>
                                                        </td> 
                                                        <td>   
                                                        <%#Eval("审核状态") %>
                                                        </td>                                                        
                                                        <td>
                                                            <%#Eval("审核时间")%>
                                                        </td>
                                                        <td title='<%#Eval("审核人") %>'>   
                                                            <div style=" word-wrap:break-word; line-height:18px;  width:70px;"><%#Eval("审核人").ToString().Length > 10 ? Eval("审核人").ToString().Substring(0, 10) + "..." : Eval("审核人").ToString()%> </div>
                                                        
                                                        </td> 
                                                        <td>
                                                           <asp:LinkButton ID="btnCK" runat="server" CommandName='<%#Eval("Number") %>' CommandArgument="ck">查看详情</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr id="ts" runat="server" class="TfootTr">
                                                <td colspan="11" align="center">
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
                            font-family: 宋体; border-collapse:collapse; text-align:center;" border="1" bordercolor="#D4D4D4">                            
                            <thead>
                                <tr align="center" style="font-size: 16px; font-family: 宋体; word-break: break-all; height:30px;">
                                    <th colspan="10">出售商品资料已审核列表</th>
                                </tr>
                                <tr>
                                    <th style=" width:150px; line-height:18px;">
                                        交易方账号</th>
                                    <th style=" width:170px;">
                                        交易方名称</th>
                                        <th style=" width:120px;">
                                        申请时间</th> 
                                    <th style=" width:80px;">
                                        商品编号
                                    </th>  
                                    <th style=" width:150px; line-height:18px;">
                                        商品名称
                                    </th>  
                                    <th style=" width:100px; line-height:18px;">
                                        是否变更<br />资质或标准</th>    
                                    <th style=" width:75px;">
                                        是否有效</th>          
                                    <th style=" width:90px;">
                                        审核状态</th>                                             
                                    <th style=" width:120px;">
                                        审核时间</th>
                                    <th style=" width:120px;">
                                        审核人</th>                                                            
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater2" runat="server" 
                                     >
                                    <ItemTemplate>
                                        <tr class="TbodyTr">
                                            <td>
                                                <%#Eval("交易方账号")%>
                                            </td>
                                            <td style=" word-wrap:break-word; line-height:18px; text-align:center; width:170px;">
                                               <%#Eval("交易方名称")%> 
                                            </td>
                                            <td>
                                                <%#Eval("申请时间")%>
                                            </td>                                            
                                            <td >
                                                <%#Eval("商品编号")%>
                                            </td>
                                            <td style=" word-wrap:break-word; line-height:18px; text-align:center;  width:150px;">
                                                <%#Eval("商品名称")%>
                                            </td>
                                           <td>   
                                            <%#Eval("是否变更资质或标准") %>
                                            </td> 
                                            <td>   
                                            <%#Eval("是否有效") %>
                                            </td> 
                                            <td >
                                                <%#Eval("审核状态")%>
                                            </td>
                                            <td>
                                                <%#Eval("审核时间")%>
                                            </td>   
                                            <td>
                                                <%#Eval("审核人")%>
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
</body>
</html>
<script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script lang="ja" type="text/javascript">
    function ShowUpdating() {
        document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'block';
        document.getElementById("<%=btnToExcel.ClientID %>").style.display = 'block';
        document.getElementById("Label1").style.display = 'none';
    }
    function Click() {
        document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'none';
        document.getElementById("<%=btnToExcel.ClientID %>").style.display = 'none';
        document.getElementById("Label1").style.display = 'block';
    }
</script>
