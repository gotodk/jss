<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjx_wltj.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_SPMM_jhjx_wltj" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>

<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
</head>
<body onload="ShowUpdating()" style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="99%">
            <Tabs>
                <radTS:Tab ID="Tab2" runat="server" NavigateUrl="jhjx_wltj.aspx" Text="物流信息统计" ForeColor="Red"
                    Font-Size="12px">
                </radTS:Tab>

            </Tabs>
        </radTS:RadTabStrip>
        <div id="new_content">
            <div id="new_zicontent">
                <div id="content_zw">
                    <div class="content_bz">
                        <%--说明文字--%>
                    </div>
                    <div class="content_nr">
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">

                            <tr>
                                <td width="70px" align="center">时间：由
                                </td>
                                <td width="150px">
                                    <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                        ID="txtBeginTime" runat="server" Width="178px"></asp:TextBox>
                                </td>
                                <td width="30px" align="center">至
                                </td>
                                <td width="150px">
                                    <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                        ID="TextBox1" runat="server" Width="178px"></asp:TextBox>
                                </td>
                                <td align="center" style="padding-left: 5px; width: 120px;">
                                    <asp:Label ID="Label1" runat="server" Text="请稍后..."></asp:Label>
                                    <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                        Text="查询" Width="70px" OnClick="BtnCheck_Click" />
                                </td>
                                <td align="left" colspan="4" style="padding-left: 10px;">
                                    <asp:Button ID="btnDC" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                        Text="导出" Width="70px" OnClick="btnDC_Click" />
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
                                    <div class="content_nr_lb" style="width: 1110px;">
                                        <table id="theObjTable" style="width: 1110px;" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr>
                                                   
                                                    <th class="TheadTh" style="width: 60px;">发货单号</th>
                                                    <th class="TheadTh" style="width: 90px;">卖家账号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">卖家名称 
                                                    </th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">卖家联系人</th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">卖家联系方式
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">买家名称</th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">买家联系人</th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">买家联系方式<br />
                                                     </th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">物流公司名称</th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">物流单号</th>
                                                    <th class="TheadTh" style="width: 90px;">物流联系人</th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">物流联系电话</th>
                                               
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="Repeater1" runat="server" >
                                                    <ItemTemplate>
                                                        <tr class="TbodyTr">
                                                            <td style="width: 90px;">
                                                                <%#Eval("发货单号")%>                                                           
                                                            </td>
                                                            <td style="width: 90px;">
                                                               <%#Eval("卖家账号")%>   

                                                            </td>
                                                            <td style="width: 100px; line-height: 18px;">    
                                                                   <asp:Label ID="Label4" runat="server" Text='<%#Eval("卖家名称").ToString().Length > 10 ? Eval("卖家名称").ToString().Substring(0, 8) + "..." : Eval("卖家名称").ToString()%>' ToolTip='<%#Eval("卖家名称")%>' ></asp:Label>
                                                            </td>
                                                            <td title='<%#Eval("卖家联系人")%>' style="width: 90px; line-height: 18px;">
                                                            
                                                                <asp:Label ID="labliaxire" runat="server" Text='<%#Eval("卖家联系人").ToString().Length > 10 ? Eval("卖家联系人").ToString().Substring(0, 8) + "..." : Eval("卖家联系人").ToString()%>' ToolTip='<%#Eval("卖家联系人")%>' ></asp:Label>
                                                            </td>
                                                            <td title='<%#Eval("卖家联系方式")%>' style="width: 90px; line-height: 18px;">
                                                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("卖家联系方式").ToString().Length > 13 ? Eval("卖家联系方式").ToString().Substring(0, 8) + "..." : Eval("卖家联系方式").ToString()%>' ToolTip='<%#Eval("卖家联系方式")%>' ></asp:Label>
                                                            </td>
                                                            <td style="width: 100px; line-height: 18px;">
                                                                 <asp:Label ID="Label6" runat="server" Text='<%#Eval("买家名称").ToString().Length > 10 ? Eval("买家名称").ToString().Substring(0, 8) + "..." : Eval("买家名称").ToString()%>' ToolTip='<%#Eval("买家名称")%>' ></asp:Label>
                                                               
                                                            </td>
                                                            <td style="width: 90px; line-height: 18px;">
                                                                <%#Eval("买家联系人")%>
                                                            </td>
                                                            <td style="width: 90px; line-height: 18px;">
                                                                
                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("买家联系方式").ToString().Length > 13 ? Eval("买家联系方式").ToString().Substring(0, 8) + "..." : Eval("买家联系方式").ToString()%>' ToolTip='<%#Eval("买家联系方式")%>' ></asp:Label> 
                                                            </td>
                                                            <td style="width: 90px; line-height: 18px;">
                                                                 <asp:Label ID="Label3" runat="server" Text='<%#Eval("物流公司名称").ToString().Length > 10 ? Eval("物流公司名称").ToString().Substring(0, 8) + "..." : Eval("物流公司名称").ToString()%>' ToolTip='<%#Eval("物流公司名称")%>' ></asp:Label> 
                                                            </td>
                                                            <td title='<%#Eval("物流单号")%>' style="width: 90px; line-height: 18px;">
                                                                <%#Eval("物流单号")%>
                                                            </td>
                                                            <td title='<%#Eval("物流联系人")%>' style="width: 90px;">
                                                               <%#Eval("物流联系人")%>
                                                            </td>
                                                            <td title='<%#Eval("物流联系电话")%>' style="width: 90px; line-height: 18px;">
                                                              <%#Eval("物流联系电话")%>
                                                            </td>
                                                            
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                            <tfoot>
                                                <tr id="ts" runat="server" class="TfootTr">
                                                    <td colspan="12" align="center">当前数据为空！
                                                    </td>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                    <uc1:commonpagernew ID="commonpagernew1" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>

      <%--  <div id="export" runat="server" style="width: 1770px; display: none;">
            <table width="100%" align="center" cellpadding="0" cellspacing="0" style="font-size: 13px; font-family: 宋体; border-collapse: collapse;"
                border="1" bordercolor="#D4D4D4">
                <thead>
                    <tr align="center" style="font-size: 16px; font-family: 宋体; word-break: break-all; height: 30px;">
                        <th colspan="17">货物签收列表</th>
                    </tr>
                    <tr>
                        <th style="width: 120px; line-height: 18px;">货物签收时间
                        </th>
                        <th style="width: 120px;">发货单号</th>
                        <th style="width: 90px;">商品编号
                        </th>
                        <th style="width: 100px; line-height: 18px;">商品名称 
                        </th>
                        <th style="width: 90px; line-height: 18px;">规格</th>
                        <th style="width: 70px; line-height: 18px;">发货数量
                        </th>
                        <th style="width: 70px; line-height: 18px;">定标价格</th>
                        <th style="width: 70px; line-height: 18px;">发货金额</th>
                        <th style="width: 80px; line-height: 18px;">无异议<br />
                            签收数量</th>
                        <th style="width: 120px; line-height: 18px;">买家名称</th>
                        <th style="width: 120px; line-height: 18px;">买家所属分公司</th>
                        <th style="width: 120px;">卖家名称</th>
                        <th style="width: 120px; line-height: 18px;">卖家所属分公司</th>
                        <th style="width: 120px;">物流公司名称</th>
                        <th style="width: 120px;">物流单号</th>
                        <th style="width: 120px;">发票编号</th>
                        <th style="width: 120px; line-height: 18px;">对应电子购<br />
                            货合同编号</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater2" runat="server">
                        <ItemTemplate>
                            <tr class="TbodyTr">
                                <td>
                                    <%#Eval("签收时间")%>                                                           
                                </td>
                                <td>
                                    <%#Eval("发货单号")%>                                                            
                                </td>
                                <td>
                                    <%#Eval("商品编号")%>
                                </td>
                                <td title='<%#Eval("商品名称")%>'>
                                    <div style="word-wrap: break-word; line-height: 18px; width: 90px;"><%#Eval("商品名称")%></div>
                                </td>
                                <td title='<%#Eval("规格")%>'>
                                    <div style="word-wrap: break-word; line-height: 18px; width: 80px;"><%#Eval("规格")%></div>
                                </td>
                                <td style="word-wrap: break-word; line-height: 18px;">
                                    <%#Eval("提货数量")%>
                                </td>
                                <td>
                                    <%#Eval("定标价格")%>
                                </td>
                                <td>
                                    <%#Eval("提货金额")%>
                                </td>
                                <td>
                                    <%#Eval("无异议签收数量")%>
                                </td>
                                <td title='<%#Eval("买家名称")%>'>
                                    <div style="word-wrap: break-word; line-height: 18px; width: 110px;"><%#Eval("买家名称")%>&nbsp;</div>
                                </td>
                                <td title='<%#Eval("买家所属分公司")%>'>
                                    <div style="word-wrap: break-word; line-height: 18px; width: 110px;"><%#Eval("买家所属分公司")%></div>
                                </td>
                                <td title='<%#Eval("卖家名称")%>'>
                                    <div style="word-wrap: break-word; line-height: 18px; width: 110px;"><%#Eval("卖家名称")%>&nbsp;</div>
                                </td>
                                <td title='<%#Eval("卖家所属分公司")%>'>
                                    <div style="word-wrap: break-word; line-height: 18px; width: 110px;"><%#Eval("卖家所属分公司")%></div>
                                </td>
                                <td title='<%#Eval("物流公司名称")%>'>
                                    <div style="word-wrap: break-word; line-height: 18px; width: 110px;"><%#Eval("物流公司名称")%></div>
                                </td>
                                <td title='<%#Eval("物流单号")%>'>
                                    <div style="word-wrap: break-word; line-height: 18px; width: 110px;"><%#Eval("物流单号")%></div>
                                </td>
                                <td title='<%#Eval("发票号码")%>'>
                                    <div style="word-wrap: break-word; line-height: 18px; width: 110px;"><%#Eval("发票号码")%></div>
                                </td>
                                <td>
                                    <%#Eval("电子合同编号") %>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr id="exprestTD" runat="server">
                        <td colspan="17" style="text-align: center;">当前数据为空！</td>
                    </tr>
                </tfoot>
            </table>            
        </div>--%>
         <input runat="server" id="hidwhere" type="hidden" />  
         <input runat="server" id="hidwhereis" type="hidden" />  
    </form>
</body>
</html>
<script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script lang="ja" type="text/javascript">
    function ShowUpdating() {
        document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'block';
        document.getElementById("<%=btnDC.ClientID %>").style.display = 'block';
        document.getElementById("Label1").style.display = 'none';
    }
    function Click() {
        document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'none';
        document.getElementById("<%=btnDC.ClientID %>").style.display = 'none';
        document.getElementById("Label1").style.display = 'block';
    }
</script>

