<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_HWSFGK.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_HWSF_JHJX_HWSFGK" %>

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
                <radTS:Tab ID="Tab2" runat="server" NavigateUrl="JHJX_HWSFGK.aspx" Text="货物收发概况"
                    ForeColor="Red" Font-Size="12px">
                </radTS:Tab>
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_YXDTHD.aspx" Text="已下达提货单"
                    Font-Size="12px">
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
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                            <tr>
                                <td style="text-align:right;width:70px" >卖方账号：</td>
                                <td>
                                    <asp:TextBox ID="txtSelzh" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td style="text-align:right;width:70px">卖方名称：</td>
                                <td style="width:100px">
                                    <asp:TextBox ID="txtSebMC" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td colspan="4">
                                    <table>
                                        <tr>
                                              <td style="text-align:right;width:70px">&nbsp;合同编号：</td>
                                <td>
                                    <asp:TextBox ID="txtDZGHHTBH" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td  style="padding-left:10px" colspan="2">
                                    <uc2:UCFWJGDetail ID="UCFWJGDetail2" runat="server" />
                                </td>
                                        </tr>
                                    </table>
                                </td>

                              
                             
                            </tr>                            
                           
                            <tr>
                                <td width="70px" align="right">买方账号：</td>
                                <td width="100px">
                                    <asp:TextBox ID="txtBuyzh" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td align="right" width="70px">买方名称：                                
                                </td>
                                <td width="100px">
                                    <asp:TextBox ID="txtBuyMC" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td style="padding-left:10px;width:520px" >
                                    <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                                </td>
                             
                                   <td width="60px" style="text-align:right;">
                                   <asp:Label ID="Label1" runat="server" Text="请稍后..."></asp:Label> <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                        Text="查询" Width="50px" OnClientClick="Click();" OnClick="BtnCheck_Click" />
                                    
                                </td>
                                <td  style="padding-left:10px" >
                                    <asp:Button ID="btnDC" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                        Text="导出" Width="50px" OnClick="btnDC_Click" />
                                </td>
                                <td  style="width:120px" ></td>
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
                                        <table id="theObjTable" style="width: 1590px;" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">商品名称 
                                                    </th>
                                                    <th class="TheadTh" style="width: 90px;">商品编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">规格</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">对应的合同编号
                                                    </th>

                                                    <th class="TheadTh" style="width: 70px; line-height: 18px;">定标数量
                                                    </th>
                                                    <th class="TheadTh" style="width: 70px; line-height: 18px;">定标价格</th>
                                                    <th class="TheadTh" style="width: 70px; line-height: 18px;">定标金额</th>
                                                    <th class="TheadTh" style="width: 80px; line-height: 18px;">无异议收货数量</th>
                                                    <th class="TheadTh" style="width: 80px; line-height: 18px;">有异议收货数量</th>
                                                     <th class="TheadTh" style="width: 120px; line-height: 18px;">买方账号</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">买方名称</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">买方管理部门</th>
                                                     <th class="TheadTh" style="width: 120px; line-height: 18px;">卖方账号</th>
                                                    <th class="TheadTh" style="width: 120px;">卖方名称</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">卖方管理部门</th>
                                                    <th class="TheadTh" style="width: 100px; text-align: center">合同结束日期</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr class="TbodyTr">
                                                            <td title='<%#Eval("商品名称")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px;"><%#Eval("商品名称").ToString().Length > 10 ? Eval("商品名称").ToString().Substring(0, 8) + "..." : Eval("商品名称").ToString()%></div>
                                                            </td>
                                                            <td>
                                                                <%#Eval("商品编号")%>
                                                            </td>
                                                            <td title='<%#Eval("规格")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px;"><%#Eval("规格").ToString().Length > 10 ? Eval("规格").ToString().Substring(0, 6) + "..." : Eval("规格").ToString()%></div>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="btnCK" runat="server" CommandName='<%#Eval("投标定标单号") %>' CommandArgument="DZGHHT"><%#Eval("对应的电子购货合同编号")%></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <%#Eval("定标数量")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("定标价格")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("定标金额")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("无异议收货数量") %>
                                                            </td>
                                                            <td>
                                                                <%#Eval("有异议收货数量") %>
                                                            </td>
                                                            <td> <%#Eval("买方账号") %></td>
                                                            <td title='<%#Eval("买家名称")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px;"><%#Eval("买家名称").ToString().Length > 10 ? Eval("买家名称").ToString().Substring(0,8) + "..." : Eval("买家名称").ToString()%>&nbsp;</div>
                                                            </td>
                                                            <td title='<%#Eval("买家所属分公司")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px;"><%#Eval("买家所属分公司").ToString().Length > 10 ? Eval("买家所属分公司").ToString().Substring(0,8) + "..." : Eval("买家所属分公司").ToString()%>&nbsp;</div>
                                                            </td>
                                                             <td> <%#Eval("卖方账号") %></td>
                                                            <td title='<%#Eval("卖家名称")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px;"><%#Eval("卖家名称").ToString().Length > 10 ? Eval("卖家名称").ToString().Substring(0,8) + "..." : Eval("卖家名称").ToString()%>&nbsp;</div>
                                                            </td>
                                                            <td title='<%#Eval("卖家所属分公司")%>'>
                                                                <div style="word-wrap: break-word; line-height: 18px;"><%#Eval("卖家所属分公司").ToString().Length > 10 ? Eval("卖家所属分公司").ToString().Substring(0,8) + "..." : Eval("卖家所属分公司").ToString()%>&nbsp;</div>
                                                            </td>
                                                            <td>
                                                                <%#Eval("对应电子购货合同结束日期")%>&nbsp;
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                            <tfoot>
                                                <tr id="ts" runat="server" class="TfootTr">
                                                    <td colspan="14" align="center">当前数据为空！
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

    </form>
</body>
</html>
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
