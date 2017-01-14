<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JGBDJK.aspx.cs" Inherits="Web_JHJX_New2013_JYGLBJK_JGBDJK" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 
    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
     <script type="text/javascript" language="javascript">
         //浮点数
         function CheckInputIntFloat(oInput) {
             if ('' != oInput.value.replace(/\d{1,}\.{0,1}\d{0,2}/, '')) {
                 oInput.value = oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/) == null ? '' : oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/);
             }
         }
    </script>
</head>
<body style=" background-color:#f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="99%">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JGBDJK.aspx" Text="价格波动检测表"
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
                    <table width="1000px" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        
                        <%--<tr>
                           
                            <td  align="right" width="70px">
                                时间：
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
                            <td></td>
                        </tr>--%>
                        <tr>
                            <td  colspan="3" >
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                新中标价格相对于上轮中标价格波动幅度超过<asp:TextBox ID="txtfd" runat="server" Width="120px" CssClass="tj_input" Style="text-align: center; ime-mode: Disabled;"
                           onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)))"
                            onkeyup="javascript:CheckInputIntFloat(this);" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d.]\.?/g,''))"></asp:TextBox>  
                                %的商品如下：</td>
                            <td  align="left" colspan="6" style=" padding-left:20px; ">
                                <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="查询" onclick="BtnCheck_Click" Width="80px" /> &nbsp;&nbsp;
                                <asp:Button ID="btnDC" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="导出" Width="80px" OnClick="btnDC_Click" />
                            </td>
                            
                        </tr>
                    </table>
                    <table width="1000px" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                <%--说明文字--%>
                            </td>
                        </tr>
                    </table>
                    <table width="1000px" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                               <%-- <div id="exprot" runat="server" class="content_nr_lb">--%>
                                    <table id="theObjTable" style="width: 1000px;"   cellspacing="0"  cellpadding="0">
                                        <thead>
                                            <tr>                                               
                                                <th class="TheadTh" style=" width:100px;text-align:center">
                                                    中标日期
                                                </th>
                                                <th class="TheadTh" style=" width:80px;text-align:center">
                                                    商品编码
                                                </th>                                              
                                                <th class="TheadTh" style=" width:140px;text-align:center">
                                                    商品名称
                                                </th>  
                                                <th class="TheadTh" style=" width:140px;text-align:center">
                                                   商品规格
                                                </th>
                                                <th class="TheadTh" style=" width:80px;text-align:center; line-height:18px;">
                                                    本轮次</br>中标价
                                                </th>  
                                                 <th class="TheadTh" style=" width:80px;text-align:center; line-height:18px;">
                                                    上轮次</br>定标价
                                                </th>                                               
                                                <th class="TheadTh" style=" width:80px;text-align:center">
                                                    波动差值
                                                </th>  
                                                 <th class="TheadTh" style=" width:80px;text-align:center">
                                                    波动幅度(%)
                                                </th>  
                                                 <th  class="TheadTh" style=" width:140px;text-align:center; line-height:18px;">
                                                        本轮中标</br>卖家名称
                                                </th>
                                                 <th class="TheadTh" style=" width:80px;text-align:center">
                                                    本轮中标</br>买家数量
                                                </th>    
                                                                                     
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server" >
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">  
                                                        <td>
                                                            <%#Eval("中标日期")%>
                                                        </td> 
                                                         <td>                                                   
                                                            <%#Eval("商品编码")%>
                                                        </td>                                                   
                                                        <td  title='<%#Eval("商品名称")%>'>
                                                            <div style=" word-wrap:break-word; line-height:18px;  "> <%#Eval("商品名称").ToString().Length > 15 ? Eval("商品名称").ToString().Substring(0, 15) + "..." : Eval("商品名称").ToString()%></div>
                                                        </td>
                                                        <td  title='<%#Eval("商品规格")%>'>
                                                            <div style=" word-wrap:break-word; line-height:18px;  "> <%#Eval("商品规格").ToString().Length > 15 ? Eval("商品规格").ToString().Substring(0, 15) + "..." : Eval("商品规格").ToString()%></div>
                                                        </td>                                                       
                                                        <td>                                                   
                                                            <%#Eval("本轮次中标价")%>
                                                        </td>
                                                        <td>
                                                           <%#Eval("上轮次定标价")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("波动差值")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("波动幅度")%>
                                                        </td>
                                                         <td  title='<%#Eval("本轮中标卖家名称")%>' style="width:140px;">
                                                            <div > <%#Eval("本轮中标卖家名称").ToString().Length > 12 ? Eval("本轮中标卖家名称").ToString().Substring(0, 12) + "..." : Eval("本轮中标卖家名称").ToString()%></div>
                                                        <td>
                                                            <%#Eval("本轮中标买家数量")%>
                                                        </td>    
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr id="ts" runat="server" class="TfootTr">
                                                <td colspan="10" align="center">
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
 
    </form>
</body>
</html>
<script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
