<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PTKPXX_Checkinfo.aspx.cs"
    Inherits="Web_JHJX_PTKPXX_Checkinfo" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-平台开票信息审核</title>
    <link href="../../../css/style.css" rel="Stylesheet" type="text/css" />
    <%-- <script src="../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>--%>
    <link href="../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #theObjTable
        {
            width: 666px;
        }
    </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="99%" BackColor="#f7f7f7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="PTKPXX_Check.aspx" ForeColor="red"
                Text="开票信息审核">
            </radTS:Tab>
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="PTKPXXChange_Check.aspx" Text="开票信息变更申请审核">
            </radTS:Tab>
            <radTS:Tab ID="Tab3" runat="server" NavigateUrl="PTKPXX_View.aspx" Text="有效开票信息查询">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <%-- <div class="content_bz">
                    1、该模块用于添加新的商品条目。<br />
                </div>--%>
                <div class="content_nr">
                    <table width="700px" class="Message">
                        <thead>
                            <tr>
                                <th class="TitleTh" colspan="4" style="padding-left: 15px">
                                    基本信息 <span runat="server" id="spanNumber" visible="false"></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="width: 120px; text-align: right; height: 25px">
                                    平台管理机构：
                                </td>
                                <td style="text-align: left; width: 150px">
                                    <span runat="server" id="spanPTGLJG"></span>
                                </td>
                                <td style="width: 80px; text-align: right;">
                                    客户编号：
                                </td>
                                <td style="text-align: left; width: 350px">
                                    <span runat="server" id="spanKHBH"></span>（邮箱：<span runat="server" id="spanDLYX"></span>）
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    交易方名称：
                                </td>
                                <td style="text-align: left;" colspan="3">
                                    <span runat="server" id="spanJYFMC"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    交易账户类型：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanJYZHLX"></span>
                                </td>
                                <td style="text-align: right;">
                                    注册类别：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanZCLB"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    信息提交时间：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanXXTJSJ"></span>
                                </td>
                                <td style="text-align: right;">
                                    当前状态：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanZT"></span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="content_lx" style="width: 700px">
                    </div>
                    <table width="700px" class="Message">
                        <thead>
                            <tr>
                                <th class="TitleTh" colspan="2" style="padding-left: 15px">
                                    开票/邮递信息
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="width: 120px; text-align: right; height: 25px">
                                    发票类型：
                                </td>
                                <td style="text-align: left; width: 580px">
                                    <span runat="server" id="spanFPLX"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    单位名称：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanDWMC"></span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table width="700px" class="Message" runat="server" id="tableZYFPXX" visible="false">
                        <tbody>
                            <tr>
                                <td style="width: 120px; text-align: right; height: 25px">
                                    纳税人识别号：
                                </td>
                                <td style="text-align: left; width: 580px">
                                    <span runat="server" id="spanNSRSBH"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    单位地址及电话：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanDWDZ"></span>&nbsp;&nbsp;<span runat="server" id="spanLXDH"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    开户行及账号：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanKHH"></span>&nbsp;&nbsp;<span runat="server" id="spanKHZH"></span>
                                </td>
                            </tr>
                            <tr runat="server" >
                                <td style="text-align: right; height: 25px">
                                    一般纳税人资格证：
                                </td>
                                <td style="text-align: left;">
                                    &nbsp;&nbsp;<a id="linkYBNSRZGZ" class="link" runat="server" href="" target="_blank">查看</a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table width="700px" class="Message">
                        <tbody>
                            <tr>
                                <td style="width: 120px; text-align: right; height: 25px">
                                    收件单位名称：
                                </td>
                                <td style="text-align: left; width: 580px">
                                    <span runat="server" id="spanFPJSDWMC"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    收件地址：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanFPJSDWDZ"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    收件人及电话：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanFPJSLXR"></span>&nbsp;&nbsp;<span runat="server" id="spanFPJSLXRDH"></span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="content_lx" style="width: 700px">
                    </div>
                    <table width="700px" class="Message">
                        <thead>
                            <tr>
                                <th class="TitleTh" colspan="2" style="padding-left: 15px">
                                    审核
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="text-align: right; height: 60px; width: 120px">
                                    上次审核情况：
                                </td>
                                <td style="text-align: left; width: 580px">
                                    <span runat="server" id="spanSHJG"></span>&nbsp;&nbsp;<span runat="server" id="spanSHR"></span>&nbsp;&nbsp<span
                                        runat="server" id="spanSHSJ"></span><br />
                                    <span runat="server" id="spanSHXX"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 70px; width: 120px">
                                    本次审核备注：<br />
                                    <font color="red">（150字以内）</font>
                                </td>
                                <td style="text-align: left; width: 580px">
                                    <asp:TextBox ID="txtSHXX" runat="server" CssClass="tj_input" Width="540px" Enabled="True"
                                        Height="100px" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" height="20px">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 180px; height: 30px">
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:Button ID="btnSave" runat="server" CssClass="tj_bt_da" Text="审核通过" Height="30"
                                                    Width="70" OnClick="btnSave_Click" OnClientClick="javascript:return confirm('您确定要审核通过吗？');" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnBoHui" runat="server" CssClass="tj_bt_da" Text="驳回" Height="30"
                                                    Width="70" OnClick="btnBoHui_Click" OnClientClick="javascript:return confirm('您确定要驳回吗？');" />&nbsp;&nbsp;
                                                <asp:Button ID="btnCancle" runat="server" CssClass="tj_bt_da" Text="返回列表" Height="30"
                                                    Width="70" OnClick="btnCancle_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
