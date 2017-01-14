<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_YXDTHD.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_HWSF_JHJX_YXDTHD" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<%@ Register Src="../UCFWJG/UCFWJGDetail.ascx" TagName="UCFWJGDetail" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>

</head>
<body onload="ShowUpdating()" style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="99%">
            <Tabs>
                <radTS:Tab ID="Tab2" runat="server" NavigateUrl="JHJX_HWSFGK.aspx" Text="货物收发概况"
                    Font-Size="12px">
                </radTS:Tab>
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_YXDTHD.aspx" Text="已下达提货单"
                    ForeColor="Red" Font-Size="12px">
                </radTS:Tab>
                <radTS:Tab ID="Tab3" runat="server" NavigateUrl="JHJX_HWQS.aspx" Text="货物签收"
                    Font-Size="12px">
                </radTS:Tab>
                <radTS:Tab ID="Tab4" runat="server" NavigateUrl="JHJX_HWFC.aspx" Text="货物发出"
                    Font-Size="12px">
                </radTS:Tab>
                <radTS:Tab ID="Tab5" runat="server" NavigateUrl="JHJX_WTYCL.aspx" Text="问题与处理"
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
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc;height:100px">
                            
                            <tr>
                                <td style="text-align:right;width:70px" >买方账号：</td>
                                <td>
                                    <asp:TextBox ID="txtBuyzh" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td style="text-align:right;width:70px">买方名称：</td>
                                <td style="width:100px">
                                    <asp:TextBox ID="txtBuyMC" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td style="padding-left:10px " colspan="5">
                                    <uc2:UCFWJGDetail ID="UCFWJGDetailBuy" runat="server" />
                                </td>
                            </tr>                            
                           
                            <tr>
                                <td width="70px" align="right">卖方账号：</td>
                                <td width="100px">
                                    <asp:TextBox ID="txtSelzh" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td align="right" width="70px">卖方名称：                                
                                </td>
                                <td width="100px">
                                    <asp:TextBox ID="txtSelMC" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td style="padding-left:10px;" colspan="5">
                                    <uc2:UCFWJGDetail ID="UCFWJGDetailSel" runat="server" />

                                </td>
                            </tr>                          
                            <tr>                               
                                <td style="text-align:right;width:70px;" >
                                    商品名称：
                                </td>
                                <td  width="100px">
                                    <asp:TextBox ID="txtSPMC" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td  style="text-align:right;width:70px;">
                                    合同编号：</td>
                                <td  >
                                    <asp:TextBox ID="txtDZGHHTBH" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>                               
                                <td  style="text-align:right;width:142px" >提货单号：</td>
                                <td  style="text-align:left">
                                    <asp:TextBox ID="txtTHDBH" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td  style="padding-left: 5px;width:70px">
                                    <asp:Label ID="Label1" runat="server" Text="请稍后..."></asp:Label>

                                    <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                        Text="查询" Width="50px" OnClientClick="Click();" OnClick="BtnCheck_Click" /></td>
                                <td  style="text-align:left; width:60px">
                                    <asp:Button ID="btnDC" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                        Text="导出" Width="50px" OnClick="btnDC_Click" />
                                </td>
                                <td style="width:240px"></td>
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
                                        <table id="theObjTable" style="width: 1510px;" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">对应电子购<br />
                                                        货合同编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px;">时间</th>
                                                    <th class="TheadTh" style="width: 120px;">提货单编号</th>
                                                    <th class="TheadTh" style="width: 90px;">商品编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">商品名称 
                                                    </th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">规格</th>
                                                    <th class="TheadTh" style="width: 70px; line-height: 18px;">提货数量
                                                    </th>
                                                    <th class="TheadTh" style="width: 70px; line-height: 18px;">定标价格</th>
                                                    <th class="TheadTh" style="width: 70px; line-height: 18px;">提货金额</th>
                                                    <th class="TheadTh" style="width: 80px; line-height: 18px;">冻结货<br />
                                                        款金额</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">买方账号</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">买方名称</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">买方管理部门</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">卖方账号</th>
                                                    <th class="TheadTh" style="width: 120px;">卖方名称</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">卖方管理部门</th>
                                                    <th class="TheadTh" style="width: 100px;">状态</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr class="TbodyTr">
                                                            <td>
                                                                <asp:LinkButton ID="btnCK" runat="server" CommandName='<%#Eval("中标定标表编号") %>' CommandArgument="DZGHHT"><%#Eval("电子购货合同编号")%></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <%#Eval("时间")%>
                                                            </td>
                                                            <td style="word-wrap: break-word; line-height: 18px;">
                                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName='<%#Eval("提货单编号") %>' CommandArgument="THD" aa='<%#Eval("提货单编号") %>'><%#Eval("提货单编号")%></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <%#Eval("商品编号")%>
                                                            </td>
                                                            <td title='<%#Eval("商品名称")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 90px;"><%#Eval("商品名称").ToString().Length > 10 ? Eval("商品名称").ToString().Substring(0, 8) + "..." : Eval("商品名称").ToString()%></div>
                                                            </td>
                                                            <td title='<%#Eval("规格")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 80px;"><%#Eval("规格").ToString().Length > 10 ? Eval("规格").ToString().Substring(0, 6) + "..." : Eval("规格").ToString()%></div>
                                                            </td>
                                                            <td>
                                                                <%#Eval("提货数量")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("定标价格")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("提货金额")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("冻结货款金额") %>
                                                            </td>
                                                              <td>
                                                                <%#Eval("买方账号") %>
                                                            </td>
                                                            <td title='<%#Eval("买家名称")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 110px;"><%#Eval("买家名称").ToString().Length > 10 ? Eval("买家名称").ToString().Substring(0,8) + "..." : Eval("买家名称").ToString()%>&nbsp;</div>
                                                            </td>
                                                            <td title='<%#Eval("买家所属分公司")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 110px;"><%#Eval("买家所属分公司").ToString().Length > 10 ? Eval("买家所属分公司").ToString().Substring(0,8) + "..." : Eval("买家所属分公司").ToString()%>&nbsp;</div>
                                                            </td>
                                                              <td>
                                                                <%#Eval("卖方账号") %>
                                                            </td>
                                                            <td title='<%#Eval("卖家名称")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 110px;"><%#Eval("卖家名称").ToString().Length > 10 ? Eval("卖家名称").ToString().Substring(0,8) + "..." : Eval("卖家名称").ToString()%>&nbsp;</div>
                                                            </td>
                                                            <td title='<%#Eval("卖家所属分公司")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 110px;"><%#Eval("卖家所属分公司").ToString().Length > 10 ? Eval("卖家所属分公司").ToString().Substring(0,8) + "..." : Eval("卖家所属分公司").ToString()%>&nbsp;</div>
                                                            </td>
                                                            <td>
                                                                <%#Eval("状态")%>&nbsp;
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                            <tfoot>
                                                <tr id="ts" runat="server" class="TfootTr">
                                                    <td colspan="15" align="center">当前数据为空！
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

        <%--<div id="export" runat="server" style="width: 1510px; display: none;">
            <table width="100%" align="center" cellpadding="0" cellspacing="0" style="font-size: 13px; font-family: 宋体; border-collapse: collapse;"
                border="1" bordercolor="#D4D4D4">
                <thead>
                    <tr align="center" style="font-size: 16px; font-family: 宋体; word-break: break-all; height: 30px;">
                        <th colspan="15">已下达提货单查询列表</th>
                    </tr>
                    <tr>
                        <th style="width: 120px;">对应电子购<br />
                            货合同编号
                        </th>
                        <th style="width: 130px;">时间
                        </th>
                        <th style="width: 120px;">提货单编号
                        </th>
                        <th style="width: 80px;">商品编号
                        </th>
                        <th style="width: 100px; line-height: 18px;">商品名称 
                        </th>
                        <th style="width: 90px; line-height: 18px;">规格
                        </th>
                        <th style="width: 70px; line-height: 18px;">提货数量
                        </th>
                        <th style="width: 70px; line-height: 18px;">定标价格
                        </th>
                        <th style="width: 75px; line-height: 18px;">提货金额
                        </th>
                        <th style="width: 80px; line-height: 18px;">冻结货<br />
                            款金额
                        </th>
                        <th style="width: 120px; line-height: 18px;">买家名称
                        </th>
                        <th style="width: 120px; line-height: 18px;">买家所属分公司
                        </th>
                        <th style="width: 120px;">卖家名称</th>
                        <th style="width: 120px; line-height: 18px;">卖家所属分公司
                        </th>
                        <th style="width: 100px;">状态</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater2" runat="server">
                        <ItemTemplate>
                            <tr class="TbodyTr">
                                <td>
                                    <%#Eval("电子购货合同编号")%>
                                </td>
                                <td>
                                    <%#Eval("时间")%> 
                                </td>
                                <td>
                                    <%#Eval("提货单编号")%>
                                </td>
                                <td style="word-wrap: break-word; line-height: 18px; text-align: center; width: 80px;">
                                    <%#Eval("商品编号")%>
                                </td>
                                <td style="word-wrap: break-word; line-height: 18px; text-align: center; width: 100px;">
                                    <%#Eval("商品名称")%>
                                </td>
                                <td style="word-wrap: break-word; line-height: 18px; text-align: center; width: 90px;">
                                    <%#Eval("规格")%>
                                </td>
                                <td>
                                    <%#Eval("提货数量")%>
                                </td>
                                <td>
                                    <%#Eval("定标价格")%>
                                </td>
                                <td>
                                    <%#Eval("提货金额")%>
                                </td>
                                <td>
                                    <%#Eval("冻结货款金额")%>
                                </td>
                                <td style="word-wrap: break-word; line-height: 18px; width: 110px;">
                                    <%#Eval("买家名称") %>
                                </td>
                                <td style="word-wrap: break-word; line-height: 18px; width: 110px;">
                                    <%#Eval("买家所属分公司")%>
                                </td>
                                <td style="word-wrap: break-word; line-height: 18px; width: 110px;">
                                    <%#Eval("卖家名称")%>
                                </td>
                                <td style="word-wrap: break-word; line-height: 18px; width: 110px;">
                                    <%#Eval("卖家所属分公司")%>
                                </td>
                                <td>
                                    <%#Eval("状态")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr id="exprestTD" runat="server">
                        <td colspan="15" style="text-align: center;">当前数据为空！</td>
                    </tr>
                </tfoot>
            </table>
        </div>--%>
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
