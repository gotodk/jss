<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewXYJFJJ.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_XYPJ_ViewXYJFJJ" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>交易方评分调整</title>
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/standardJSFile/fcf.js" type="text/javascript"></script>  
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="99%" BackColor="#f7f7f7">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_JYFWGKFTJ.aspx" ForeColor="red"
                    Text="交易方评分调整">
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
                                    <th class="TitleTh" colspan="4" style="padding-left: 15px">交易方基本信息
                                    </th>
                                </tr>
                            </thead>
                        </table>
                        <table width="700px" class="Message" id="tabf" runat="server">

                            <tr>
                                <td style="text-align: right; width: 120px; height: 25px">交易方账号：
                                </td>
                                <td style="text-align: left; width: 150px">
                                    <asp:Label ID="lbjyfzh" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">交易方名称： </td>
                                <td style="text-align: left; width: 300px">
                                    <asp:Label ID="lbjyfmc" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">账户类型：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbjyzhlx" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">注册类别：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbzclb" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">联系人姓名：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblxrxm" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">联系人电话：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblxrsjh" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">所属区域：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbssqy" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">经纪人账号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbgljjrzh" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">所属分公司：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbssfgs" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">当前评分：</td>
                                <td style="text-align: left;">

                                    <asp:Label ID="lbdqpf" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">合同编号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbhtbh" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;"></td>
                                <td style="text-align: left;">
                                    <input type="hidden" runat="server" id="iptNum" />
                                </td>

                            </tr>

                        </table>
                        <div class="content_lx" style="width: 700px">
                        </div>
                        <table width="700px" class="Message" id="tabn" runat="server">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="2" style="padding-left: 15px">评分变动依据</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="text-align: right; height: 25px; width: 120px">&nbsp;扣减分值：</td>
                                    <td style="text-align: left; width: 150px">

                                        <asp:TextBox ID="txtfs" runat="server" Width="150px" CssClass="tj_input" ReadOnly="true">-2</asp:TextBox>


                                    </td>
                                    <td style="text-align: right;">&nbsp;</td>
                                    <td style="text-align: left; width: 300px">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px; width: 120px">事项依据： </td>
                                    <td style="text-align: left;" colspan="3">

                                        <asp:DropDownList ID="ddlwgsx" runat="server" CssClass="tj_input" Width="427px">
                                            <asp:ListItem>履约中被判定为违约责任方</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table width="700px" class="Message" runat="server" id="tab2">
                            <tbody>
                                <tr>
                                    <td style="width: 120px; text-align: right; height: 100px">&nbsp;情况简述：
                                    </td>
                                    <td style="text-align: left;">

                                        <asp:TextBox ID="txtqkjs" runat="server" CssClass="tj_input" Width="525px" Enabled="True"
                                            Height="100px" MaxLength="150" TextMode="MultiLine"></asp:TextBox>

                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table width="700px" class="Message" id="tab3" runat="server">

                            <tbody>
                                <tr>
                                    <td>
                                        <div class="content_lx" style="width: 700px">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 200px; height: 30px"></td>
                                                <td style="text-align: left;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnSave" runat="server" CssClass="tj_bt_da" Text="提交" Height="30"
                                                    Width="70" OnClick="btnSave_Click" OnClientClick="javascript:return confirm('您确定要进行提交吗？');" />
                                                    &nbsp;&nbsp;
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnCancle" runat="server" CssClass="tj_bt_da" Text="取消" Height="30"
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
