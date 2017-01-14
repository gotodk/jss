<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_HWFC.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_HWSF_JHJX_HWFC" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<%@ Register Src="../UCFWJG/UCFWJGDetail.ascx" TagName="UCFWJGDetail" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        //$(document).ready(function () {
        //    $("#txtJJRBH").art_confirm(optsConfirm);
        //});
        function aa(obj) {
            //alert("aa");
            $(this).art_confirm(optsConfirm);

        }
        var optsConfirm = {
            name: "diag1",
            width: 850,
            height: 470,
            title: "经纪人",
            url: "JHJX/New2013/JJRSYDW/JJR.aspx?id=1",
            showmessagerow: false,
            showbuttonrow: false,
            optionName: "optsConfirm",
            IsReady: function (object, dialog) {
                __artConfirmOperner.close();
                alert("aa");
            }
        }
    </script>
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
                    Font-Size="12px">
                </radTS:Tab>
                <radTS:Tab ID="Tab3" runat="server" NavigateUrl="JHJX_HWQS.aspx" Text="货物签收"
                    Font-Size="12px">
                </radTS:Tab>
                <radTS:Tab ID="Tab4" runat="server" NavigateUrl="JHJX_HWFC.aspx" Text="货物发出" ForeColor="Red"
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
                                    <uc2:UCFWJGDetail ID="UCFWJGDetail3" runat="server" />
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
                                    <asp:TextBox ID="txtSellerMC" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td style="padding-left:10px;" colspan="5">
                                    <uc2:UCFWJGDetail ID="UCFWJGDetail4" runat="server" />
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
                                <td  ><asp:TextBox ID="txtDZGHHTBH" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>                               
                                <td  style="text-align:right;width:142px" >发货单号：</td>
                                <td  style="text-align:left">
                                    <asp:TextBox ID="txtFHDBH" runat="server" Width="100px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td  style="padding-left: 5px;width:70px">
                                    <asp:Label ID="Label1" runat="server" Text="请稍后..."></asp:Label>
                                    <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                        Text="查询" Width="50px" OnClientClick="Click();" OnClick="BtnCheck_Click" />
                                </td>
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
                                        <table id="theObjTable" style="width: 2020px;" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">生成时间
                                                    </th>
                                                    <th class="TheadTh" style="width: 110px;">发货单号</th>
                                                    <th class="TheadTh" style="width: 120px;">对应电子购货合同编号</th>
                                                    <th class="TheadTh" style="width: 120px;">商品名称</th>
                                                    <th class="TheadTh" style="width: 70px;">商品编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">规格 
                                                    </th>
                                                    <th class="TheadTh" style="width: 60px; line-height: 18px;">发货数量</th>
                                                    <th class="TheadTh" style="width: 60px; line-height: 18px;">定标价格
                                                    </th>
                                                    <th class="TheadTh" style="width: 70px; line-height: 18px;">发货金额</th>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">对应提货单</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">买方账号</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">买方名称</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">买方管理部门</th>
                                                    <th class="TheadTh" style="width: 120px;">卖方账号</th>
                                                    <th class="TheadTh" style="width: 120px;">卖方名称</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">卖方管理部门</th>
                                                    <th class="TheadTh" style="width: 120px; line-height: 18px;">物流公司名称</th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">物流公司联系人</th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">物流联系电话</th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">物流单编号</th>
                                                    <th class="TheadTh" style="width: 90px; line-height: 18px;">发票编号</th>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">发票发送方式</th>

                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr class="TbodyTr">
                                                            <td style="width: 120px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 120px;"><%#Eval("生成时间")%> </div>
                                                            </td>
                                                            <td style="width: 110px;">
                                                                <asp:LinkButton ID="btnFHD" runat="server" CommandName='<%#Eval("发货单号") %>' Width="110px" CommandArgument="FHD"><%#Eval("发货单号")%></asp:LinkButton>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <asp:LinkButton ID="btnDYDZGHHT" runat="server" CommandName='<%#Eval("中标定标信息表编号") %>' CommandArgument="DYDZGHHT"><%#Eval("对应电子购货合同编号")%></asp:LinkButton>
                                                            </td>
                                                            <td style="word-wrap: break-word; line-height: 18px; width: 120px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 120px;"><%#Eval("商品名称") %></div>
                                                            </td>
                                                            <td style="width: 70px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 70px;"><%#Eval("商品编号")%></div>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 100px;"><%#Eval("规格")%></div>
                                                            </td>
                                                            <td style="width: 60px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 60px;"><%#Eval("发货数量")%></div>
                                                            </td>
                                                            <td style="width: 60px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 60px;"><%#Eval("定标价格")%></div>
                                                            </td>
                                                            <td style="width: 70px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 70px;"><%#Eval("发货金额")%></div>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 100px;"><%#Eval("对应提货单")%></div>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 120px;"><%#Eval("买方账号") %></div>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 120px;"><%#Eval("买方名称")%>&nbsp;</div>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 120px;"><%#Eval("买方所属分公司")%>&nbsp;</div>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 120px;"><%#Eval("卖方账号")%>&nbsp;</div>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 120px;"><%#Eval("卖方名称")%>&nbsp;</div>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 120px;"><%#Eval("卖方所属分公司")%>&nbsp;</div>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 120px;"><%#Eval("物流公司名称")%>&nbsp;</div>
                                                            </td>
                                                            <td style="width: 90px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 90px;"><%#Eval("物流公司联系人")%>&nbsp;</div>
                                                            </td>
                                                            <td style="width: 90px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 90px;"><%#Eval("物流联系电话")%>&nbsp;</div>
                                                            </td>
                                                            <td style="width: 90px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 90px;"><%#Eval("物流单编号")%>&nbsp;</div>
                                                            </td>
                                                            <td style="width: 90px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 90px;"><%#Eval("发票编号")%>&nbsp;</div>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <div style="word-wrap: break-word; line-height: 18px; width: 100px;"><%#Eval("发票发送方式")%>&nbsp;</div>
                                                            </td>



                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                            <tfoot>
                                                <tr id="ts" runat="server" class="TfootTr">
                                                    <td colspan="22" align="center">当前数据为空！
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
