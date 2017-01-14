<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_FB.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_SPMM_JHJX_FB" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<%@ Register Src="../UCFWJG/UCFWJGDetail.ascx" TagName="UCFWJGDetail" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 57px;
        }
    </style>
</head>
<body onload="ShowUpdating()" style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="99%">
            <Tabs>
                <radTS:Tab ID="Tab2" runat="server" NavigateUrl="jhjx_spmmgk.aspx"
                    Text=" 商品买卖概况">
                </radTS:Tab>
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="jhjx_spmmjb.aspx"
                    Text="竞标中">
                </radTS:Tab>
                <radTS:Tab ID="Tab3" runat="server" NavigateUrl="jhjx_spmmljq.aspx"
                    Text="冷静期">
                </radTS:Tab>
                <radTS:Tab ID="Tab4" runat="server" NavigateUrl="jhjx_spmmzb.aspx"
                    Text="中标">
                </radTS:Tab>
                <radTS:Tab ID="Tab6" runat="server" NavigateUrl="jhjx_spmmdbybzh.aspx"
                    Text="定标与保证函">
                </radTS:Tab>
                <radTS:Tab ID="Tab5" runat="server" NavigateUrl="JHJX_FB.aspx" Text="废标" ForeColor="Red"
                    Font-Size="12px">
                </radTS:Tab>
                <radTS:Tab ID="Tab7" runat="server" NavigateUrl="JHJX_QP.aspx" Text="清盘"
                    Font-Size="12px">
                </radTS:Tab>
                <radTS:Tab ID="Tab8" runat="server" NavigateUrl="JHJX_CGX.aspx" Text="草稿箱"
                    Font-Size="12px">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <div id="new_content">
            <div id="new_zicontent">
                <div id="content_zw">
                    <div class="content_bz">
                        <%--说明文字：<br />
                     1、该模块用于查询。<br /> --%>
                    </div>
                    <div class="content_nr">
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                            <tr>
                                <td width="110px" align="right">单据类别：&nbsp;
                                </td>
                                <td width="150px">
                                    <asp:DropDownList ID="ddldjxz" runat="server" CssClass="tj_input" Width="150px">
                                        <asp:ListItem Value="">全部</asp:ListItem>
                                        <asp:ListItem Value="预订单">预订单</asp:ListItem>
                                        <asp:ListItem Value="投标单">投标单</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="right" width="90px">交易方名称：                                
                                </td>
                                <td width="150px">
                                    <asp:TextBox ID="txtjyfmc" runat="server" Width="150px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td align="right" width="80px">&nbsp;交易方账户：</td>
                                <td width="150px">
                                    <asp:TextBox ID="txtjyfzh" runat="server" Width="150px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td align="right" class="auto-style1">

                                    <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                        Text="查询" OnClientClick="Click();" OnClick="BtnCheck_Click" Width="70px" />

                                </td>

                                <td width="120px" align="right" style="width: 60px">

                                    <asp:Button ID="btnDC" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                        Text="导出" Width="70px" OnClick="btnDC_Click" />

                                    <asp:Label ID="Label1" runat="server" Text="请稍后..." Style="height: 15px"></asp:Label>

                                </td>

                            </tr>
                            <tr>
                                <td align="right" width="110px">商品名称：</td>
                                <td width="150px">
                                    <asp:TextBox class="tj_input" ID="txtspmc" runat="server" Width="150px"></asp:TextBox>
                                </td>
                                <td align="left" colspan="6">
                                    <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                                </td>

                            </tr>
                        </table>

                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td>废标信息列表
                                </td>
                            </tr>
                        </table>
                        <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                            style="border-collapse: collapse; table-layout: fixed; width: 100%;" class="tab">
                            <tr>
                                <td>
                                    <div class="content_nr_lb" style="width: 1110px;">
                                        <table id="theObjTable" style="width: 1980px;" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr>

                                                    <th class="TheadTh" style="width: 100px;">业务管理部门
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">交易方账号
                                                    </th>
                                                    <th class="TheadTh" style="width: 200px;">交易方名称
                                                    </th>

                                                    <th class="TheadTh" style="width: 200px; line-height: 18px">电子购货合同编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">废标时间
                                                    </th>
                                                    <th class="TheadTh" style="width: 150px;">废标原因</th>

                                                    <th class="TheadTh" style="width: 100px;">下单时间
                                                    </th>
                                                    <th class="TheadTh" style="width: 80px;">商品编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">商品名称
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">规格
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">合同期限
                                                    </th>
                                                    <th class="TheadTh" style="width: 70px;">数量
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">价格
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">金额
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">单据类型
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">单据编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">订金解冻金额
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">投标保证金</br>扣罚金额
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">履约保证金</br>扣罚金额
                                                    </th>
                                                    <th class="TheadTh" style="width: 80px;">是否拆单
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptfb" runat="server" OnItemDataBound="rptfb_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr class="TbodyTr">
                                                            <td>
                                                                <%#Eval("所属分公司")%>
                                                            </td>

                                                            <td>
                                                                <%#Eval("交易方账号")%>
                                                            </td>
                                                            <td title='<%#Eval("交易方名称")%>' style="width: 200px;">

                                                                <asp:Label ID="lbjyfmc" runat="server" Text='<%#Eval("交易方名称")%>'> </asp:Label>
                                                            </td>

                                                            <td style="width: 200px;">

                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("合同编号")%>'> </asp:Label>
                                                            </td>

                                                            <td>
                                                                <%#Eval("废标时间")%>
                                                            </td>
                                                            <td title='<%#Eval("废标原因")%>'>
                                                                <asp:Label ID="lbfbyy" runat="server" Text='<%#Eval("废标原因")%>'> </asp:Label>
                                                            </td>
                                                            <td>
                                                                <%#Eval("下单时间")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("商品编号")%>
                                                            </td>
                                                            <td title='<%#Eval("商品名称")%>'>
                                                                <asp:Label ID="lbspmc" runat="server" Text='<%#Eval("商品名称")%>'> </asp:Label>
                                                            </td>
                                                            <td>
                                                                <%#Eval("规格")%>
                                                            </td>
                                                             <td>
                                                                <%#Eval("合同期限")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("数量")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("价格")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("金额")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("单据类型")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("单据编号")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("订金解冻金额")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("投标保证金扣罚金额")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("履约保证金扣罚金额")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("是否拆单")%>
                                                            </td>
                                                            <%--  <td>  <asp:LinkButton ID="ledit" runat="server" CommandName="lck" CommandArgument='<%#Eval("保证函编号")%>' >查看详情</asp:LinkButton>          
                                                                                              
                                                    </td>--%>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                                    <td colspan="9">您查询的数据为空！
                                                    </td>
                                                </tr>
                                            </tbody>
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
        <input runat="server" id="hidID" type="hidden" />
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

