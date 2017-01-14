<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YDJZHGL.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_UserDongJie_YDJZHGL" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<%@ Register Src="../UCFWJG/UCFWJGDetail.ascx" TagName="UCFWJGDetail" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-已冻结交易账户管理</title>
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
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
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="YDJZHGL.aspx" ForeColor="red" Text="已冻结交易账户管理">
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
                                <td style="padding-left: 10px" align="left" colspan="7">
                                    <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td width="118px" align="right">交易方账号：
                                </td>
                                <td width="120" align="left">
                                    <asp:TextBox ID="txtJYFZH" runat="server" class="tj_input" Width="120px"></asp:TextBox>
                                </td>
                                <td width="85px" align="right">交易方名称：
                                </td>
                                <td width="120px" align="left">
                                    <asp:TextBox ID="txtJYFMC" runat="server" class="tj_input" Width="120px"></asp:TextBox>
                                </td>
                                <td width="70px" align="right">冻结功能：
                                </td>
                                <td style="text-align: left; width: 160px;">
                                    <asp:DropDownList ID="ddlDJGN" runat="server" CssClass="tj_input" Width="160px">
                                        <asp:ListItem Value="">请选择</asp:ListItem>
                                        <asp:ListItem>经纪人暂停代理新业务</asp:ListItem>
                                        <asp:ListItem>经纪人暂停用户新业务</asp:ListItem>
                                        <asp:ListItem>出金</asp:ListItem>
                                        <asp:ListItem>可投标商品申请</asp:ListItem>
                                        <asp:ListItem>投标单</asp:ListItem>
                                        <asp:ListItem>预订单</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="left" style="padding-left: 15px">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="50px" Text="查询"
                                        OnClick="btnSearch_Click" />&nbsp;&nbsp;<asp:Button ID="btnExport" runat="server"
                                            CssClass="tj_bt" Width="50px" Text="导出" OnClick="btnExport_Click" />
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td>已冻结交易账户列表
                                </td>
                            </tr>
                        </table>
                        <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse; table-layout: fixed; width: 1110px;"
                            class="tab">
                            <tr>
                                <td>
                                    <table id="theObjTable" style="width: 1110px;" cellspacing="0" cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style="width: 120px;">交易方账号
                                                </th>
                                                <th class="TheadTh" style="width: 100px;">交易账户类型
                                                </th>
                                                <th class="TheadTh" style="width: 70px;">注册类别
                                                </th>
                                                <th class="TheadTh" style="width: 100px;">交易方名称
                                                </th>
                                                <th class="TheadTh" style="width: 100px; line-height: 18px">交易方<br />
                                                    联系电话
                                                </th>
                                                <th class="TheadTh" style="width: 70px; line-height: 18px">联系人<br />
                                                    姓名
                                                </th>
                                                <th class="TheadTh" style="width: 100px; line-height: 18px">联系人<br />
                                                    手机号
                                                </th>
                                                <th class="TheadTh" style="width: 100px;">所属区域
                                                </th>
                                                <th class="TheadTh" style="width: 100px; line-height: 18px">当前关联<br />
                                                    经纪人
                                                </th>
                                                <th class="TheadTh" style="width: 90px;">平台管理机构
                                                </th>
                                                <th class="TheadTh" style="width: 100px;">冻结功能
                                                </th>
                                                <th class="TheadTh" style="width: 60px;">操作
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptKPXX" runat="server" OnItemDataBound="rptKPXX_ItemDataBound"
                                                OnItemCommand="rptKPXX_ItemCommand">
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td title='<%#Eval("交易方账号")%>'>
                                                            <asp:Label ID="lblJYFZH" runat="server" Text='<%#Eval("交易方账号")%>'> </asp:Label>
                                                        </td>
                                                        <td>
                                                            <%#Eval("交易帐户类型")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("注册类别")%>
                                                        </td>
                                                        <td title='<%#Eval("交易方名称")%>'>
                                                            <asp:Label ID="lblJYFMC" runat="server" Text='<%#Eval("交易方名称")%>'> </asp:Label>
                                                        </td>
                                                        <td title='<%#Eval("交易方联系电话") %>'>
                                                            <asp:Label ID="lblJYFLXDH" runat="server" Text='<%#Eval("交易方联系电话")%>'> </asp:Label>
                                                        </td>
                                                        <td style="word-break: break-all; word-wrap: break-word">
                                                            <%#Eval("联系人姓名")%>
                                                        </td>
                                                        <td title='<%#Eval("联系人手机号") %>'>
                                                            <asp:Label ID="lblLXRSJH" runat="server" Text='<%#Eval("联系人手机号")%>'> </asp:Label>
                                                        </td>
                                                        <td title='<%#Eval("所属区域")%>'>
                                                            <asp:Label ID="lblSSQY" runat="server" Text='<%#Eval("所属区域")%>'> </asp:Label>
                                                        </td>
                                                        <td title='<%#Eval("关联经纪人") %>'>
                                                            <asp:Label ID="lblDQGLJJR" runat="server" Text='<%#Eval("关联经纪人")%>'> </asp:Label>
                                                        </td>
                                                        <td>
                                                            <%#Eval("平台管理机构")%>
                                                        </td>
                                                        <td title='<%#Eval("冻结功能") %>'>
                                                            <asp:Label ID="lblDJGN" runat="server" Text='<%#Eval("冻结功能")%>'> </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lkbtnCheck" runat="server" CommandName="linkbj" CommandArgument='<%#Eval("交易方账号")%>'>管理</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                                <td colspan="12">暂无满足条件的数据！
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
