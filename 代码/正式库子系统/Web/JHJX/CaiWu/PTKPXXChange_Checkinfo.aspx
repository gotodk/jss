<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PTKPXXChange_Checkinfo.aspx.cs"
    Inherits="Web_JHJX_PTKPXXChange_Checkinfo" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-平台开票信息审核</title>
    <link href="../../../css/style.css" rel="Stylesheet" type="text/css" />
    <%--<script src="../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>--%>
    <link href="../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="99%" BackColor="#f7f7f7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="PTKPXX_Check.aspx" Text="开票信息审核">
            </radTS:Tab>
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="PTKPXXChange_Check.aspx" ForeColor="Red"
                Text="开票信息变更申请审核">
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
                                    基本信息 <span runat="server" id="spanID" visible="false"></span><span runat="server"
                                        id="spanNumber" visible="false"></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="width: 130px; text-align: right; height: 25px">
                                    平台管理机构：
                                </td>
                                <td style="text-align: left; width: 150px">
                                    <span runat="server" id="spanPTGLJG"></span>
                                </td>
                                <td style="width: 80px; text-align: right;">
                                    客户编号：
                                </td>
                                <td style="text-align: left; width: 340px">
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
                                    原发票类型：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanFPLX"></span>
                                </td>
                                <td style="text-align: right; height: 25px">
                                    单位名称：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanDWMC"></span>
                                </td>
                            </tr>
                    </table>
                    <div class="content_lx" style="width: 700px">
                    </div>
                    <table width="700px" class="Message">
                        <thead>
                            <tr>
                                <th class="TitleTh" colspan="4" style="padding-left: 15px">
                                    开票/邮递信息变更申请
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="text-align: right; height: 30px; width: 130px">
                                    信息提交时间：
                                </td>
                                <td style="text-align: left; width: 150px">
                                    <span runat="server" id="spanXXTJSJ"></span>
                                </td>
                                <td style="text-align: right; width: 150px">
                                    处理状态：
                                </td>
                                <td style="text-align: left; width: 270px">
                                    <span runat="server" id="spanCLZT"></span>
                                </td>
                            </tr>
                            <tr id="trYBNSRZGZMSMJ" runat="server" style="text-align: right; height: 30px;">
                                <td align="right">
                                    变更信息扫描件：
                                </td>
                                <td style="text-align: left;">
                                    &nbsp;&nbsp;<a id="linkBGXXSMJ" class="link" runat="server" href="" target="_blank">查看</a>
                                </td>
                                <td align="right" runat="server" >
                                    一般纳税人资格证明：
                                </td>
                                <td style="text-align: left;" runat="server" >
                                    &nbsp;&nbsp;<a id="linkYBNSRZGZM" class="link" runat="server" href="" target="_blank">查看新文件</a>&nbsp;&nbsp;<a
                                        id="linkYBNSRZGZ" class="link" runat="server" href="" target="_blank">查看原文件</a><span
                                            id="spanXTPMC" runat="server" visible="false"></span><span id="spanTPMC" runat="server"
                                                visible="false"></span>
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
                                <td style="text-align: right; height: 70px; width: 130px">
                                    审核备注：
                                    <br />
                                    <font color="red">（150字以内）</font>
                                </td>
                                <td style="text-align: left; width: 600px">
                                    <asp:TextBox ID="txtSHXX" runat="server" Width="540px" Enabled="True" Height="100px"
                                        MaxLength="150" CssClass=" tj_input" TextMode="MultiLine"></asp:TextBox>
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
                                            <td style="width: 120px; height: 30px">
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:Button ID="btnSave" runat="server" CssClass="tj_bt_da" Text="审核通过" Height="30"
                                                    Width="70" OnClick="btnSave_Click" OnClientClick="javascript:return confirm('请修改开票信息！');" />
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
                    <table width="700px" class="Message" id="tableJBXX" runat="server" visible="false">
                        <thead>
                            <tr>
                                <th colspan="2">
                                    <div class="content_lx" style="width: 700px">
                                    </div>
                                </th>
                            </tr>
                            <tr>
                                <th class="TitleTh" colspan="2" style="padding-left: 15px">
                                    开票/邮递信息修改
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="width: 130px; text-align: right; height: 30px">
                                    发票类型：
                                </td>
                                <td style="text-align: left; width: 570px">
                                    <asp:RadioButtonList ID="rblFPLX" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                                        OnSelectedIndexChanged="rblFPLX_SelectedIndexChanged">
                                        <asp:ListItem>增值税普通发票</asp:ListItem>
                                        <asp:ListItem>增值税专用发票</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 30px">
                                    单位名称：
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtDWMC" runat="server" CssClass="tj_input" Width="300px" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table width="700px" class="Message" runat="server" id="tableZYFPXX" visible="false">
                        <tbody>
                            <tr>
                                <td style="width: 130px; text-align: right; height: 30px">
                                    纳税人识别号：
                                </td>
                                <td style="text-align: left; width: 570px">
                                    <asp:TextBox ID="txtNSRSBH" runat="server" CssClass="tj_input" Width="200px" Enabled="True"
                                        MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    单位地址：
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtDWDZ" runat="server" CssClass="tj_input" Width="300px" Enabled="True"
                                        MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    联系电话：
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtLXDH" runat="server" CssClass="tj_input" Width="100px" Enabled="True"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    开户行：
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtKHH" runat="server" CssClass="tj_input" Width="300px" Enabled="True"
                                        MaxLength="80"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    开户账号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtKHZH" runat="server" CssClass="tj_input" Width="200px" Enabled="True"
                                        MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    一般纳税人资格证明：
                                </td>
                                <td style="text-align: left;">
                                    <asp:RadioButtonList ID="rblYBNSRZGZ" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem>使用原文件</asp:ListItem>
                                        <asp:ListItem>使用新文件</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table width="700px" class="Message" id="tableYDXX" runat="server" visible="false">
                        <tbody>
                            <tr>
                                <td style="width: 130px; text-align: right; height: 30px">
                                    收件单位名称：
                                </td>
                                <td style="text-align: left; width: 570px">
                                    <asp:TextBox ID="txtSJDWMC" runat="server" CssClass="tj_input" Width="300px" Enabled="True"
                                        MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 30px">
                                    收件地址：
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtSJDZ" runat="server" CssClass="tj_input" Width="300px" Enabled="True"
                                        MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 30px">
                                    收件人：
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtSJR" runat="server" CssClass="tj_input" Width="100px" Enabled="True"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 30px">
                                    收件人电话：
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtSJRDH" runat="server" CssClass="tj_input" Width="100px" Enabled="True"
                                        MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td style="width: 130px; height: 50px">
                                </td>
                                <td style="text-align: left;">
                                    <asp:Button ID="btnQRXG" runat="server" CssClass="tj_bt_da" Text="确定修改" Height="30"
                                        Width="80" OnClick="btnQRXG_Click" OnClientClick="javascript:return confirm('您确定要提交修改的开票/邮递信息吗？');" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnQXXG" runat="server" CssClass="tj_bt_da" Text="重置" Height="30"
                                        Width="80" OnClick="btnQXXG_Click" OnClientClick="javascript:return confirm('您确定要取消修改开票/邮递信息吗？');" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnTCXG" runat="server" CssClass="tj_bt_da" Text="退出修改" Height="30"
                                        Width="80" OnClick="btnTCXG_Click" OnClientClick="javascript:return confirm('您确定要退出修改吗？');" />
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
