<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_SPJYXXView.aspx.cs"
    Inherits="Web_JHJX_JHJX_SPJYXXView" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-商品建议信息查询</title>
    <link href="../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
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
            <radTS:Tab ID="Tab2" runat="server" ForeColor="Red" NavigateUrl="JHJX_SPJYXXView.aspx"
                Text="商品建议信息查询">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw" style="width: 954px">
                <%--  <div class="content_bz">
                    说明文字：<br />
                    该模块用于交易方提交的开票信息的审核，审核通过后开票信息生效。
                </div>--%>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        <tr>
                            <td style="width: 100px; overflow: hidden; text-align: right;">
                                建议事项：
                            </td>
                            <td style="text-align: left; width: 130px;">
                                <asp:DropDownList ID="ddlJYSX" runat="server" CssClass="tj_input" Width="130px">
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem>商品俗称</asp:ListItem>
                                    <asp:ListItem>商品描述</asp:ListItem>
                                    <asp:ListItem>验收标准</asp:ListItem>
                                    <asp:ListItem>质量标准</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 90px; overflow: hidden; text-align: right;">
                                商品编号：
                            </td>
                            <td style="text-align: left; width: 120px;">
                                <asp:TextBox ID="txtSPBH" runat="server" class="tj_input" Width="90px"></asp:TextBox>
                            </td>
                            <td width="80px" align="right">
                                商品名称：
                            </td>
                            <td width="120px" align="left">
                                <asp:TextBox ID="txtSPMC" runat="server" class="tj_input" Width="90px"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="50px" Text="查询"
                                    OnClick="btnSearch_Click" />&nbsp;&nbsp;<asp:Button ID="btnExport" runat="server"
                                        CssClass="tj_bt" Width="100px" Text="导出到excel" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                商品建议信息列表
                            </td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse;
                        table-layout: fixed; width: 950px;" class="tab">
                        <tr>
                            <td>
                                <table id="theObjTable" style="width: 950px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 40px;">
                                                序号
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                商品编号
                                            </th>
                                            <th class="TheadTh" style="width: 110px;">
                                                商品名称
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                型号规格
                                            </th>
                                            <th class="TheadTh" style="width: 130px;">
                                                商品描述
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                建议事项
                                            </th>
                                            <th class="TheadTh" style="width: 300px;">
                                                建议内容
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                支持率
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptKPXX" runat="server" OnItemDataBound="rptKPXX_ItemDataBound">
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td>
                                                        <asp:Label ID="lblXH" runat="server" Text='<%#Eval("rownum")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSPBH" runat="server" Text='<%#Eval("SPBH")%>'> </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("SPMC")%>'>
                                                        <asp:Label ID="lblSPMC" runat="server" Text='<%#Eval("SPMC")%>'> </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("GG")%>'>
                                                        <asp:Label ID="lblGG" runat="server" Text='<%#Eval("GG")%>'> </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("SPMS")%>'>
                                                        <asp:Label ID="lblSPMS" runat="server" Text='<%#Eval("SPMS")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblJYSX" runat="server" Text='<%#Eval("JYSX")%>'> </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("JYNR") %>'>
                                                        <asp:Label ID="lblJYNR" runat="server" Text='<%#Eval("JYNR")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblZCL" runat="server" Text='<%#Eval("ZCL")%>'> </asp:Label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                            <td colspan="7">
                                                暂无满足条件的数据！
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
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
