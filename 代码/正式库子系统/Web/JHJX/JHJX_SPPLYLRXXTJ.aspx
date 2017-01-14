<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_SPPLYLRXXTJ.aspx.cs"
    Inherits="Web_JHJX_JHJX_SPPLYLRXXTJ" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-商品品类预录入信息统计</title>
    <link href="../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_SPPLYLRXXTJ.aspx" ForeColor="red"
                Text="商品品类预录入信息统计">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw" style="width: 700px">
                <%--   <div class="content_bz">
                    &nbsp;&nbsp;
                </div>--%>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        <tr>
                            <td width="15%" align="right">
                                预录入时间：从&nbsp;&nbsp;
                            </td>
                            <td width="15%" align="center">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtStart" runat="server" Width="95%"></asp:TextBox>
                            </td>
                            <td width="5%" align="center">
                                到
                            </td>
                            <td width="15%" align="center">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtEnd" runat="server" Width="95%"></asp:TextBox>
                            </td>
                            <td align="center" width="40%">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="80px" Text="查询"
                                    OnClick="btnSearch_Click" />&nbsp;&nbsp;<asp:Button ID="btnExport" runat="server"
                                        CssClass="tj_bt" Width="100px" Text="导出到excel" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="export" runat="server">
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td colspan="5" align="center" style="font-size: 14px; font-family: 宋体; font-weight: bold;
                                    height: 30px">
                                    <span id ="spanTime" runat ="server"></span>商品品类预录入信息统计
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                            style="border-collapse: collapse;" class="tab">
                            <tr>
                                <td>
                                    <table id="theObjTable" style="width: 700px;" cellspacing="0" cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style="width: 100px; text-align: center;">
                                                    预录入操作人
                                                </th>
                                                <th class="TheadTh" style="width: 150px; text-align: center;">
                                                    添加商品品类数量
                                                </th>
                                                <th class="TheadTh" style="width: 150px; text-align: center;">
                                                    复核未通过品类数量
                                                </th>
                                                <th class="TheadTh" style="width: 150px; text-align: center;">
                                                    确定上线品类数量
                                                </th>
                                                <th class="TheadTh" style="width: 150px; text-align: center;">
                                                    商品上线率
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rpt" runat="server">
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td align="center" style="width: 100px; word-wrap: break-word;">
                                                            <div style="width: 100px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("预录入操作人")%></div>
                                                        </td>
                                                        <td align="center" style="width: 150px; word-wrap: break-word;">
                                                            <div style="width: 150px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("添加商品品类数量")%></div>
                                                        </td>
                                                        <td align="center" style="width: 150px; word-wrap: break-word;">
                                                            <div style="width: 150px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("复核未通过品类数量")%></div>
                                                        </td>
                                                        <td align="center" style="width: 150px; word-wrap: break-word;">
                                                            <div style="width: 150px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("确定上线品类数量")%></div>
                                                        </td>
                                                        <td align="center" style="width: 150px; word-wrap: break-word;">
                                                            <div style="width: 150px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("商品上线率")%>%</div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <tr id="tdEmpty" runat="server" visible="true" style="text-align: center;">
                                                <td colspan="5">
                                                    暂无任何统计数据！
                                                </td>
                                            </tr>
                                        </tbody>
                                        <tfoot id="foot" runat="server" visible="false">
                                            <tr class="TbodyTr">
                                                <td align="center">
                                                    合计：
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblLRSL" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblWTGSL" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblYSXSL" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblSXBL" runat="server" Text="0"></asp:Label>
                                                </td>
                                            </tr>
                                        </tfoot>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    </form>
</body>
</html>
