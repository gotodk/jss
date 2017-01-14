<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JJRSYZQMXB.aspx.cs" Inherits="Web_JHJX_New2013_JJRSYDW_JJRSYZQMXB" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<%@ Register src="../UCFWJG/UCFWJGDetail.ascx" tagname="UCFWJGDetail" tagprefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> <script src="../../../../js/standardJSFile/jquery.art_confirm.js"></script>
    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
</head>
<body style=" background-color:#f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="99%">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JJRSYZQMXB.aspx" Text="经纪人（单位）收益支取明细管理"
                ForeColor="Red" Font-Size="12px">
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
                            <td  align="right" width="80px">
                                审核时间：
                            </td>
                            <td colspan="8">
                                <table>
                                 <tr>
                                <td width="138px">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="txtBeginTime" runat="server" Width="138px"></asp:TextBox>                            
                               </td>
                                <td style="width:35px;text-align:right">至：</td>
                               <td width="138px" align="left">                           
                               <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="txtEndTime" runat="server" Width="138px"></asp:TextBox>                            
                            </td>
                                 <td colspan="4" style="padding-left:10px">

                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />

                            </td>
                                 </tr>
                                </table>
                            </td>    
                        </tr>
                         <tr>
                                                 
                            <td align="right" width="80px">
                                审核状态：                                
                            </td>
                            <td width="138px">
                            <asp:DropDownList ID="drpSFSH" runat="server" CssClass="tj_input" Width="138px">
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem Value="待审核">待审核</asp:ListItem>
                                    <asp:ListItem Value="已审核">已审核</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td  align="right" width="80px">
                                经纪人账号：
                            </td>
                            <td width="138px">
                                <asp:TextBox ID="txtJJRBH" runat="server" Width="138px" CssClass="tj_input"></asp:TextBox>  
                                                     
                            </td>
                            <td  style="width:80px;padding-left:10px">
                                经纪人名称：</td>
                            <td width="138px">
                                 <asp:TextBox ID="txtJJRMC" runat="server" Width="138px" CssClass="tj_input"></asp:TextBox>                           
                             </td>
                           
                              <td  align="left" style=" padding-left:10px; ">
                                <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False" Width="50px"
                                    Text="查询" onclick="BtnCheck_Click" /> &nbsp;&nbsp;
                                <asp:Button ID="btnDC" runat="server" CssClass="tj_bt" UseSubmitBehavior="False" Width="50px"
                                    Text="导出"  OnClick="btnDC_Click" />
                            </td>
                              <td style="width:138px"></td>
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
                               <%-- <div id="exprot" runat="server" class="content_nr_lb">--%>
                                    <table id="theObjTable" style="width: 100%;"   cellspacing="0"  cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style=" width:90px;">
                                                    单号
                                                </th>
                                                <th class="TheadTh" style=" width:90px;">
                                                    分公司名称
                                                </th>
                                                <th class="TheadTh" style=" width:100px;">
                                                    经纪人账号
                                                </th>                                              
                                                <th class="TheadTh" style=" width:120px;">
                                                    经纪人名称
                                                </th>  
                                                <th class="TheadTh" style=" width:60px; line-height:18px;">
                                                   第三方存<br />管状态 
                                                </th>
                                                <th class="TheadTh" style=" width:70px; line-height:18px;">
                                                    缺票收<br />益金额
                                                </th>                                               
                                                <th class="TheadTh" style=" width:70px; line-height:18px;">
                                                    本次已收合<br />格发票金额
                                                </th>
                                                <th class="TheadTh" style=" width:70px; line-height:18px;">
                                                    本次可支<br />取金额
                                                </th>
                                                <th class="TheadTh" style=" width:115px; line-height:18px;">
                                                   收取发<br />票时间
                                                </th>
                                                <th class="TheadTh" style=" width:80px; line-height:18px;">
                                                    审核状态
                                                </th>
                                                <th class="TheadTh" style=" width:115px; line-height:18px;">
                                                    审核时间
                                                </th>
                                                <th class="TheadTh" style=" width:120px;">
                                                    操作
                                                </th>                                                          
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server" 
                                                onitemcommand="Repeater1_ItemCommand"  >
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td>
                                                            <%#Eval("Number")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("平台管理机构")%>
                                                        </td>
                                                        <td style=" word-wrap:break-word; line-height:18px;">
                                                           <%#Eval("JJRBH")%>
                                                        </td>
                                                        <td  title='<%#Eval("JJRMC")%>'>
                                                            <div style=" word-wrap:break-word; line-height:18px;  width:120px;"> <%#Eval("JJRMC").ToString().Length > 10 ? Eval("JJRMC").ToString().Substring(0, 10) + "..." : Eval("JJRMC").ToString()%></div>
                                                        </td>
                                                        <td>
                                                            <%#Eval("I_DSFCGZT")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("缺票收益金额")%>
                                                        </td>
                                                        <td>
                                                           <%#Eval("BCYSHGFPJE")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("BCKZQJE")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("发票收取时间")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("SHZT") %>
                                                        </td>
                                                        <td>
                                                            <%#Eval("SHSJ")%>&nbsp;
                                                        </td>
                                                        <td>

                                                           <asp:HiddenField ID="hidNumber" runat="server" Value='<%#Eval("Number") %>' />
                                                           <asp:LinkButton ID="btnCK" runat="server" CommandName='<%#Eval("Number") %>' CommandArgument="CK">查看详情</asp:LinkButton>
                                                           &nbsp;
                            <asp:LinkButton ID="btnZXTD" runat="server" CommandName='<%#Eval("Number") %>' CommandArgument="XG" Enabled='<%#Eval("SHZT").ToString()=="待审核"?true:false %>' >修改</asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="btnSH" runat="server" CommandName='<%#Eval("Number") %>' CommandArgument="SH" Enabled='<%#Eval("SHZT").ToString()=="待审核"?true:false %>'>审核</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr id="ts" runat="server" class="TfootTr">
                                                <td colspan="12" align="center">
                                                当前数据为空！
                                                </td>
                                            </tr> 
                                        </tfoot>
                                    </table>
                           <%--     </div>--%>
                                <uc1:commonpagernew ID="commonpagernew1" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div id="export" runat="server" style="width:100%; display:none;">
                        <table width="100%"  align="center" cellpadding="0" cellspacing="0" style="font-size: 13px;
                            font-family: 宋体; border-collapse:collapse;" border="1" bordercolor="#D4D4D4">                            
                            <thead>
                                <tr align="center" style="font-size: 16px; font-family: 宋体; word-break: break-all; height:30px;">
                                    <th colspan="11">经纪人（单位）收益支取明细列表</th>
                                </tr>
                                <tr>
                                    <th style=" width:90px;">
                                        单号
                                    </th>
                                    <th style=" width:90px;">
                                        分公司名称
                                    </th>
                                    <th style=" width:130px;">
                                        经纪人编号
                                    </th>                                              
                                    <th  style=" width:150px;">
                                        经纪人名称
                                    </th>  
                                    <th style=" width:90px; line-height:18px;">
                                        第三方存<br />管状态 
                                    </th>
                                    <th style=" width:80px; line-height:18px;">
                                        缺票收<br />益金额
                                    </th>                                               
                                    <th style=" width:80px; line-height:18px;">
                                        本次已收合<br />格发票金额
                                    </th>
                                    <th style=" width:80px; line-height:18px;">
                                        本次可支<br />取金额
                                    </th>
                                    <th style=" width:115px; line-height:18px;">
                                        收取发<br />票时间
                                    </th>
                                    <th style=" width:80px; line-height:18px;">
                                        审核状态
                                    </th>
                                    <th style=" width:115px; line-height:18px;">
                                        审核时间
                                    </th>                                                         
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="Repeater2" runat="server" 
                                     >
                                    <ItemTemplate>
                                        <tr class="TbodyTr">
                                            <td>
                                                <%#Eval("Number")%>
                                            </td>
                                            <td>
                                                <%#Eval("平台管理机构")%>
                                            </td>
                                            <td>
                                                <%#Eval("JJRBH")%>
                                            </td>
                                            <td >
                                                <%#Eval("JJRMC")%>
                                            </td>
                                            <td>
                                                <%#Eval("I_DSFCGZT")%>
                                            </td>
                                            <td>
                                                <%#Eval("缺票收益金额")%>
                                            </td>
                                            <td>
                                                <%#Eval("BCYSHGFPJE")%>
                                            </td>
                                            <td>
                                                <%#Eval("BCKZQJE")%>
                                            </td>
                                            <td>
                                                <%#Eval("SQFPRQ")%>
                                            </td>
                                            <td>
                                                <%#Eval("SHZT") %>
                                            </td>
                                            <td>
                                                <%#Eval("SHSJ")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                            <tfoot>
                                <tr id="exprestTD" runat="server"><td colspan="11" style=" text-align:center;">当前数据为空！</td>
                                </tr>
                            </tfoot>
                        </table>
                    </div> 
    </form>
</body>
</html>
<script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>