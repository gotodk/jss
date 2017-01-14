<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_QPCL.aspx.cs" Inherits="Web_JHJX_JHJX_QPCL" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>人工清盘</title>

    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
    <style type="text/css">
        #content_zw {
            width: 970px;
        }
    </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
            BackColor="#F7F7F7">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_QPCL.aspx" ForeColor="red"
                    Text="人工清盘受理">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <div id="new_content">
            <div id="new_zicontent">
                <div id="content_zw">
                    <div class="content_bz">
                        说明文字：<br />
                        1、该模块用于电子购物合同的人工清盘。<br />
                    </div>
                    <div class="content_nr">
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                            <tr>
                                <td style="width: 90px; overflow: hidden; text-align: right;">合同编号：
                                </td>
                                <td style="text-align: left; width: 120px;">
                                    <asp:TextBox ID="txthtbh" runat="server" class="tj_input" Width="120px" Height="23"></asp:TextBox>
                                </td>
                                <td width="70px" align="right">商品名称：</td>
                                <td style="text-align: left; width: 100px;">
                                    <asp:TextBox ID="txtspmc" runat="server" class="tj_input" Width="120px" Height="23"></asp:TextBox>
                                </td>
                                <td width="70px" align="right">商品规格：</td>
                                <td width="130px" align="left">
                                    <asp:TextBox ID="txtspgg" runat="server" class="tj_input" Width="120px" Height="23"></asp:TextBox>

                                </td>

                                <td width="80px" align="right">清盘状态：</td>

                                <td width="80px" align="left">
                                    <asp:DropDownList ID="ddlqpzt" runat="server" CssClass="tj_input" Width="80px" Height="23">
                                        <asp:ListItem>清盘中</asp:ListItem>
                                        <asp:ListItem>清盘结束</asp:ListItem>
                                        <asp:ListItem Value="">全部</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="50px">&nbsp;</td>
                                <td align="left">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="80px"
                                        Text="查询" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td>商品信息列表
                                </td>
                            </tr>
                        </table>
                        <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                            style="border-collapse: collapse; table-layout: fixed; width: 970px;" class="tab">
                            <tr>
                                <td>
                                    <table id="theObjTable" style="width: 970px;" cellspacing="0" cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style="width: 100px;">购物合同编号
                                                </th>
                                                <th class="TheadTh" style="width: 100px;">合同结束日期
                                                </th>
                                                <th class="TheadTh" style="width: 100px;">商品名称
                                                </th>
                                                <th class="TheadTh" style="width: 80px;">商品编号
                                                </th>
                                                <th class="TheadTh" style="width: 100px;">规格
                                                </th>
                                                <th class="TheadTh" style="width: 70px;">合同数量
                                                </th>
                                                <th class="TheadTh" style="width: 70px;">单价
                                                </th>
                                                <th class="TheadTh" style="width: 70px;">合同金额
                                                </th>
                                                <th class="TheadTh" style="width: 70px;">争议数量</th>
                                                <th class="TheadTh" style="width: 70px;">争议金额
                                                </th>
                                                <th class="TheadTh" style="width: 80px;">清盘类型
                                                </th>
                                                <th class="TheadTh" style="width: 80px;">清盘原因
                                                </th>
                                                <th class="TheadTh" style="width: 80px;">清盘状态
                                                </th>
                                                <th class="TheadTh" style="width: 80px;">详情及操作
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptSPXX" runat="server" OnItemDataBound="rptSPXX_ItemDataBound" OnItemCommand="rptSPXX_ItemCommand">
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td>
                                                            <%#Eval("电子购货合同编号")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("合同结束日期")%> 
                                                        </td>
                                                        <td title='<%#Eval("商品名称")%>'>
                                                            <asp:Label ID="lbspmc" runat="server" Text='<%#Eval("商品名称")%>'> </asp:Label>
                                                        </td>
                                                        <td>
                                                            <%#Eval("商品编号")%>
                                                        
                                                        </td>
                                                        <td title='<%#Eval("规格")%>'>
                                                            <asp:Label ID="lbgg" runat="server" Text='<%#Eval("规格")%>'> </asp:Label>
                                                        </td>
                                                        <td>
                                                            <%#Eval("合同数量")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("单价")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("合同金额")%>  
                                                        </td>
                                                        <td>
                                                            <%#Eval("争议数量")%> 
                                                        </td>
                                                        <td>
                                                            <%#Eval("争议金额")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("清盘类型")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("清盘原因")%>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbqpzt" runat="server" Text='<%#Eval("清盘状态")%>'> </asp:Label>

                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="ledit" runat="server" CommandName="lqp" CommandArgument='<%#Eval("主键")%>'>清盘处理</asp:LinkButton></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr id="tdEmpty" runat="server" class="TfootTr">
                                                <td colspan="14" align="center">您查询的数据为空！
                                                </td>
                                            </tr>
                                        </tfoot>
                                    </table>
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
