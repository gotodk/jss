<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PTFPYJXX_LR.aspx.cs" Inherits="Web_JHJX_PTFPYJXX_LR" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-平台发票邮递信息管理</title>
    <script src="../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
    <style type="text/css">
        #content_zw
        {
            width: 900px;
        }
    </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="PTFPYJXX_LR.aspx" Text="平台发票邮递信息录入"
                ForeColor="Red">
            </radTS:Tab>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="PTFPYJXX_Check.aspx" Text="平台发票邮递信息修改审核">
            </radTS:Tab>
            <radTS:Tab ID="Tab3" runat="server" NavigateUrl="PTFPYDXX_View.aspx" Text="平台发票邮递信息查询">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <%--  <div class="content_bz">
                    说明文字：<br />
                    该模块用于交易方提交的开票信息的审核，审核通过后开票信息生效。
                </div>--%>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        <tr>
                            <td style="width: 100px; overflow: hidden; text-align: right;">
                                平台管理机构：
                            </td>
                            <td style="text-align: left; width: 130px;">
                                <asp:DropDownList ID="ddlPTGLJG" runat="server" CssClass="tj_input" Width="130px"
                                    Height="23">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 90px; overflow: hidden; text-align: right;">
                                发票类别：
                            </td>
                            <td style="text-align: left; width: 120px;">
                                <asp:DropDownList ID="ddlFPLX" runat="server" CssClass="tj_input" Height="23">
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem Value="A">增值税专用发票</asp:ListItem>
                                    <asp:ListItem Value="B">增值税普通发票</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <%--<td width="60px" align="right">
                               状态：
                            </td>
                            <td style="text-align: left; width: 70px;">
                               <asp:DropDownList ID="ddlZT" runat="server" CssClass="tj_input" Width="70px" Height="23">
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem Selected="True">待审核</asp:ListItem>
                                    <asp:ListItem>审核通过</asp:ListItem>
                                    <asp:ListItem>驳回</asp:ListItem>
                                </asp:DropDownList>
                            </td>--%>
                            <td width="80px" align="right">
                                客户编号：
                            </td>
                            <td width="100px" align="left">
                                <asp:TextBox ID="txtKHBH" runat="server" class="tj_input" Width="100px" Height="23"></asp:TextBox>
                            </td>
                            <td width="80px" align="right">
                                单位名称：
                            </td>
                            <td width="100px" align="left">
                                <asp:TextBox ID="txtDWMC" runat="server" class="tj_input" Width="100px" Height="23"></asp:TextBox>
                            </td>
                            <td align="center" width="70px">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="50px" Text="查询"
                                    OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                未录入邮递信息发票列表（金额单位：元）
                            </td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse;
                        table-layout: fixed; width: 900px;" class="tab">
                        <tr>
                            <td>
                                <table id="theObjTable" style="width: 900px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 110px;">
                                                发票单号
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                平台管理机构
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                客户编号
                                            </th>
                                            <th class="TheadTh" style="width: 140px;">
                                                单位名称
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                发票类型
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                发票金额
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                发票号码
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                发票生成日期
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                操作
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptKPXX" runat="server" OnItemDataBound="rptKPXX_ItemDataBound"
                                            OnItemCommand="rptKPXX_ItemCommand">
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td>
                                                        <%#Eval("编号")%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPTGLJG" runat="server" Text='<%#Eval("平台管理机构")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <%#Eval("客户编号")%>
                                                    </td>
                                                    <td title='<%#Eval("客户全称")%>'>
                                                        <asp:Label ID="lblDWMC" runat="server" Text='<%#Eval("客户全称")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <%#Eval("发票种类")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("发票金额")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("发票号码")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("审核日期")%>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lkbtnCheck" runat="server" CommandName="linkbj" CommandArgument='<%#Eval("编号")%>'>录入邮递信息</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                            <td colspan="9">
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
    <input runat="server" id="hidID" type="hidden" />
    </form>
</body>
</html>
