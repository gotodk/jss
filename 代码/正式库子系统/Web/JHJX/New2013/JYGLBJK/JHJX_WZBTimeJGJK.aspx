<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_WZBTimeJGJK.aspx.cs" Inherits="Web_JHJX_New2013_JYGLBJK_JHJX_WZBTimeJGJK" %>

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
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="JHJX_WZBTimeJGJK.aspx" Text="未中标时间间隔监控"
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
                <div class="content_nr" id="divLB" runat="server">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">                        
                        <tr>  
                            <td width="70px" align="right">
                                <span>发布时间</span>：
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
                            <td width="100px" align="right">
                                <span>截止未中标时间</span>：
                            </td>
                            <td width="178px">
                            <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="TextBox1" runat="server" Width="178px"></asp:TextBox>                            
                            </td>  
                            <td  align="center" style=" padding-left:5px; width:120px;">
                                <asp:Label ID="Label1" runat="server" Text="正在查询中，请稍后..." Width="120px"></asp:Label>
                                <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="查询" Width="70px" OnClientClick="Click();" OnClick="BtnCheck_Click" />
                            </td>
                            <td  align="left" colspan="4" style=" padding-left:10px; ">
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
                                                <th class="TheadTh"  style=" width:80px; line-height:18px;">
                                                    商品编号</th>
                                                <th class="TheadTh" style=" width:150px;">
                                                    商品名称</th>
                                                  <th class="TheadTh" style=" width:70px;">
                                                    计价单位</th>         
                                                <th class="TheadTh" style=" width:120px;">
                                                    最近一次中标日期</th>                                              
                                                <th class="TheadTh" style=" width:100px;">
                                                    参与买家数量
                                                </th>  
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    参与卖家数量
                                                </th>
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    最高拟<br />订购价格</th>    
                                                <th class="TheadTh" style=" width:75px; line-height:18px;">
                                                    最低拟<br />出售价格</th>
                                                <th class="TheadTh" style=" width:100px;">
                                                    拟订购总量</th> 
                                                <th class="TheadTh" style=" width:100px;">
                                                    拟出售总量</th> 
                                                <th class="TheadTh">操作</th>                                               
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" 
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
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName='<%#Eval("商品编码")+"&"+Eval("商品名称") %>' CommandArgument="ck">查看</asp:LinkButton> 
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
                <div id="divXQ" runat="server" visible="false">
                    <div class="content_bz">
                    <%--1、该模块用于记录已签约服务商以个人名义打款的打款人信息以及与服务商的对应关系。<br />
                    2、保存时系统根据规则自动生成正式客户编号，打款人编号以"6"开头，用生成的编号录入ERP。<br />
                    3、“所属服务商编号”请填写本办事处销售渠道为“服务商”或“门店服务商”的客户编号。--%>
                </div>
                <div style=" width:100%;">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2" class="BT">
                                未中标时间间隔监控详情
                            </td>
                        </tr>
                        <tr>
                            <td class="BTLeftDD">                                
                            </td>
                            <td class="BTRightTJTime">                            
                            </td>
                        </tr>
                    </table>
                </div>
                <hr class="content_lx" style=" margin-top:0px; border:0px; border-top:solid 1px #a5cbe2; height:0px;"/>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" bordercolor="#D4D4D4">
                    <tr>
                        <td style=" height:20px;"></td>
                    </tr>
                    <tr>
                        <td class="LBBT" style=" height:30px; ">
                           <span id="spanSP" runat="server"> 商品编号：（自动带出）商品名称：（自动带出）</span>

                        </td>
                    </tr>    
                </table>
                <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                               <%--<div class="content_nr_lb" style="width:1110px; ">--%>
                                    <table id="Table1"  style="width:100%; " cellspacing="0"  cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh"  style=" width:100px; line-height:18px;">
                                                    交易方编号</th>
                                                <th class="TheadTh" style=" width:100px;">
                                                    所在区域</th>
                                                  <th class="TheadTh" style=" width:70px;">
                                                    单据类别</th>         
                                                <th class="TheadTh" style=" width:70px;">
                                                    合同期限</th>                                              
                                                <th class="TheadTh" style=" width:120px;">
                                                    下达/发布时间
                                                </th>  
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    价格
                                                </th>
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    数量</th>    
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    平台设定的<br />经济批量</th>
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    卖家设定的<br />经济批量</th> 
                                                <th class="TheadTh" style=" width:250px;">
                                                    收货/发货区域</th>                                               
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater3" runat="server" 
                                                >
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td style=" width:100px;" title='<%#Eval("交易方编号") %>'>
                                                            <div style=" word-wrap:break-word; line-height:18px;  width:100px;">  <%#Eval("交易方编号") %></div>                                             
                                                        </td>
                                                        <td >
                                                          <%#Eval("所在区域") %>
                                                        </td>
                                                          <td>
                                                               <%#Eval("单据类别") %>
                                                        </td>
                                                        <td>   
                                                        <%#Eval("合同期限") %>
                                                        </td>
                                                        <td>
                                                           <%#Eval("发布时间")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("价格")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("数量")%>
                                                        </td>
                                                         <td>
                                                            <%#Eval("平台设定的经济批量")%>
                                                        </td>
                                                         <td>
                                                            <%#Eval("卖家设定的经济批量")%>
                                                        </td>
                                                         <td style=" width:250px;">
                                                           <div style=" word-wrap:break-word; line-height:18px;  width:250px;"> <%#Eval("收货/发货区域")%>
                                                               </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr id="Tr1" runat="server" class="TfootTr">
                                                <td colspan="10" align="center">
                                                当前数据为空！
                                                </td>
                                            </tr> 
                                        </tfoot>
                                    </table>
                                <%--</div>--%>
                                <%--<uc1:commonpagernew ID="commonpagernew1" runat="server" />--%>
                            </td>
                        </tr>
                    </table>
                    <div style=" width:100%; text-align:center;">
                        <table class="Noprint" style="width:100%;">
                            <tr>
                                <td style=" height:20px;"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnBack" runat="server" Text="返回列表" CssClass="tj_bt_da"  Width="100px"  UseSubmitBehavior="False" Height="30px" OnClick="btnBack_Click" />
                                </td>
                            </tr>
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
        if ($("#divLB").length > 0) {
            document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'block';
            document.getElementById("<%=btnToExcel.ClientID %>").style.display = 'block';
            document.getElementById("Label1").style.display = 'none';
        }
    }
    function Click() {
        document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'none';
        document.getElementById("<%=btnToExcel.ClientID %>").style.display = 'none';
        document.getElementById("Label1").style.display = 'block';
    }
</script>
