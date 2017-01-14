<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FGSMJMJTJ_type.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_FGSYWCX_FGSMJMJTJ_type" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-平台开票信息查询</title>
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
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
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="FGSMJMJTJ_type.aspx" Text="分公司买家卖家统计（按类型）"
                ForeColor="red">
            </radTS:Tab>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="FGSMJMJTJ_city.aspx" Text="分公司买家卖家统计（按区域）">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw" style="width: 960px">
                  <div class="content_bz">
                    说明：<br />
                    1、本统计基础数据为交易平台中的有效的买家卖家交易账户。<br />
                    2、注册类别为“自然人”的，归类于“买家、个人”类别；生效时间以分公司审核通过交易账户开通申请的时间为准；<br />
                    3、注册类别为“单位”的，若存在审核通过的出售商品信息，则归类为“卖家、企业”类别，生效时间以第一条出售商品审核通过时间为准；<br />&nbsp;&nbsp;&nbsp;&nbsp;否则归类为“买家、企业”类别，生效时时间以分公司审核通过交易账户开通申请的时间为准。
                    
                </div>
                <div class="content_nr">
                    <div id="export" runat="server">
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td colspan="13" align="center" style="font-size: 14px; font-family: 宋体; font-weight: bold;
                                    height: 30px">
                                    分公司买家卖家统计（按类型）
                                </td>
                            </tr>
                        </table>
                        <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse;
                            table-layout: fixed; width: 960px;" class="tab">
                            <tr>
                                <td>
                                    <table id="theObjTable" style="width: 960px;" cellspacing="0" cellpadding="0">
                                        <thead>
                                            <tr>
                                                <td width="120px" rowspan="3" align="center" class="FHTheadThAlign" style="font-weight: bold;">
                                                    分公司
                                                </td>
                                                <td colspan="8" align="center" class="FHTheadThAlign" style="font-weight: bold; height: 18px">
                                                    买家
                                                </td>
                                                <td colspan="4" align="center" class="FHTheadThAlign" style="font-weight: bold;">
                                                    卖家
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center" class="FHTheadThAlign" style="font-weight: bold; height: 18px">
                                                    企业
                                                </td>
                                                <td colspan="4" align="center" class="FHTheadThAlign" style="font-weight: bold;">
                                                    个人
                                                </td>
                                                <td colspan="4" align="center" class="FHTheadThAlign" style="font-weight: bold;">
                                                    企业
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="FHTheadThAlign" style="width: 70px; font-weight: bold;
                                                    height: 18px">
                                                    本日数量
                                                </td>
                                                <td align="center" class="FHTheadThAlign" style="width: 70px; font-weight: bold;">
                                                    本周数量
                                                </td>
                                                <td class="FHTheadThAlign" align="center" style="width: 70px; font-weight: bold;
                                                    height: 15px">
                                                    本月数量
                                                </td>
                                                <td align="center" class="FHTheadThAlign" style="width: 70px; font-weight: bold;">
                                                    财年数量
                                                </td>
                                                <td align="center" class="FHTheadThAlign" style="width: 70px; font-weight: bold;
                                                    height: 15px">
                                                    本日数量
                                                </td>
                                                <td align="center" class="FHTheadThAlign" style="width: 70px; font-weight: bold;">
                                                    本周数量
                                                </td>
                                                <td class="FHTheadThAlign" align="center" style="width: 70px; font-weight: bold;
                                                    height: 15px">
                                                    本月数量
                                                </td>
                                                <td align="center" class="FHTheadThAlign" style="width: 70px; font-weight: bold;">
                                                    财年数量
                                                </td>
                                                <td align="center" class="FHTheadThAlign" style="width: 70px; font-weight: bold;
                                                    height: 15px">
                                                    本日数量
                                                </td>
                                                <td align="center" class="FHTheadThAlign" style="width: 70px; font-weight: bold;">
                                                    本周数量
                                                </td>
                                                <td class="FHTheadThAlign" align="center" style="width: 70px; font-weight: bold;
                                                    height: 15px">
                                                    本月数量
                                                </td>
                                                <td align="center" class="FHTheadThAlign" style="width: 70px; font-weight: bold;">
                                                    财年数量
                                                </td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rpt" runat="server">
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td align="center">
                                                            <%#Eval("分公司")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("买家单位本日")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("买家单位本周")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("买家单位本月")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("买家单位财年")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("买家个人本日")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("买家个人本周")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("买家个人本月")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("买家个人财年")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("卖家本日")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("卖家本周")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("卖家本月")%>
                                                        </td>
                                                        <td align="center">
                                                            <%#Eval("卖家财年")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr class="TbodyTr">
                                                <td align="center">
                                                    合计：
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblBuyerDW_day" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblBuyerDW_week" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblBuyerDW_month" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblBuyerDW_year" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblBuyerGR_day" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblBuyerGR_week" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblBuyerGR_month" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblBuyerGR_year" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblSaler_day" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblSaler_week" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblSaler_month" runat="server" Text="0"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblSaler_year" runat="server" Text="0"></asp:Label>
                                                </td>
                                            </tr>
                                        </tfoot>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table width="960px" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="50px" align="center" colspan="2">
                                <asp:Button ID="btnExport" runat="server" CssClass="tj_bt_da" Text="导出" OnClick="btnExport_Click"
                                    Width="100px" Height="30px" />
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
