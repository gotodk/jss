<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_CKXQ.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_CSSPZLSH_JHJX_CKXQ" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../../css/style.css" rel="stylesheet" />
    <link href="../../../../css/standardStyle.css" rel="stylesheet" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/fcf.js"></script>
    <script src="../../../dialog.js" type="text/javascript"></script>
    <style type="text/css">
        table th
        {
            text-align: left;
        }

        #content_zw
        {
            width: 700px;
        }
    </style>
    <script lang="ja" type="text/javascript">
        function Promot(this_an) {
            return art_confirm_fcf(this_an, "您确定要审核通过吗？此操作执行的结果将不可恢复！", 'clickyc()');
        }
        function PromotBohui(this_an) {

            return art_confirm_fcf(this_an, "您确定要进行驳回操作吗？此操作执行的结果将不可恢复！", 'clickyc()');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="new_content">
            <div id="new_zicontent">
                <div id="content_zw">
                    <%-- <div class="content_bz">
                    1、该模块用于添加新的商品条目。<br />
                </div>--%>
                    <div class="content_nr">
                        <table width="700px" class="Message" cellspacing="0" cellpadding="0">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="4" style="padding-left: 15px">商品基本信息

                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px">商品编号：
                                    </td>
                                    <td style="text-align: left; width: 150px">
                                        <span runat="server" id="spanSPBH"></span>
                                    </td>
                                    <td style="width: 150px; text-align: right;">商品名称：
                                    </td>
                                    <td style="text-align: left; width: 250px">
                                        <span runat="server" id="spanSPMC" style="line-height: 18px;"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">执行标准：
                                    </td>
                                    <td style="text-align: left;" colspan="3">
                                        <span runat="server" id="spanZXBZ" style="line-height: 18px;"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">计价单位：
                                    </td>
                                    <td style="text-align: left;">
                                        <span runat="server" id="spanJJDW"></span>
                                    </td>
                                    <td style="text-align: right;">平台设定最大经济批量：
                                    </td>
                                    <td style="text-align: left;">
                                        <span runat="server" id="spanPTSDZDJJPL"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">商品描述：
                                    </td>
                                    <td style="text-align: left;" colspan="3">
                                        <span runat="server" id="spanSPMS" style="word-wrap: break-word; line-height: 18px; width: 620px;"></span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="content_lx" style="width: 700px">
                        </div>
                        <table width="700px" class="Message" cellspacing="0" cellpadding="0">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="4" style="padding-left: 15px">交易方基本信息
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px">交易方账号：
                                    </td>
                                    <td style="text-align: left; width: 150px; line-height: 18px;">
                                        <span runat="server" id="spanJYFZH" style="line-height: 18px;"></span>
                                    </td>
                                    <td style="width: 150px; text-align: right;">交易方名称：
                                    </td>
                                    <td style="text-align: left; width: 250px; line-height: 18px;">
                                        <span runat="server" id="spanJYFMC" style="line-height: 18px;"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">联系人姓名：
                                    </td>
                                    <td style="text-align: left;">
                                        <span runat="server" id="spanLXRXM"></span>
                                    </td>
                                    <td style="text-align: right;">联系人手机号：
                                    </td>
                                    <td style="text-align: left;">
                                        <span runat="server" id="spanLXRSJH"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">交易方联系电话：
                                    </td>
                                    <td style="text-align: left;" colspan="3">
                                        <span runat="server" id="spanJYFLXDH"></span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="content_lx" style="width: 700px">
                        </div>
                        <table width="700px" class="Message" cellspacing="0" cellpadding="0">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="2" style="padding-left: 15px">投标资质
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <%--<tr>
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px">一般人纳税人资格证明：
                                    </td>
                                    <td style="text-align: left; width: 550px">&nbsp;&nbsp;<a id="AYBNSR" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px">质量标准与证明：
                                    </td>
                                    <td style="text-align: left; width: 550px">&nbsp;&nbsp;<a id="AZLBZ" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">产品检测报告：
                                    </td>
                                    <td style="text-align: left;">&nbsp;&nbsp;<a id="ACPJCBG" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">品管总负责人法律承诺书：
                                    </td>
                                    <td style="text-align: left;">&nbsp;&nbsp;<a id="APGZFZRFLCNS" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">法定代表人承诺书：
                                    </td>
                                    <td style="text-align: left;">&nbsp;&nbsp;<a id="AFDDBRCRS" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">售后服务规定与承诺：
                                    </td>
                                    <td style="text-align: left;">&nbsp;&nbsp;<a id="ASHFWGD" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">产品送检授权书：
                                    </td>
                                    <td style="text-align: left;">&nbsp;&nbsp;<a id="ACPSJSQS" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                </tr>

                            </tbody>
                        </table>
                        <div runat="server" id="divTSZZ"></div>
                        <%-- <table width="700px" class="Message" cellspacing="0"  cellpadding="0" id="tabTSZZ" runat="server" visible="false">
                        <tr><td></td></tr>
                    </table>--%>
                        <div class="content_lx" style="width: 700px">
                        </div>
                        <table width="700px" class="Message" cellspacing="0" cellpadding="0">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="2" style="padding-left: 15px">服务中心审核

                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="text-align: right; height: 70px; width: 120px">审核意见：
                                    </td>
                                    <td style="text-align: left; width: 580px">
                                        <asp:TextBox ID="txtSHYJ" runat="server" CssClass="tj_input" Width="400px" Enabled="True"
                                            Height="80px" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" height="20px"></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 180px; height: 30px"></td>
                                                <td style="text-align: left;">
                                                    <div id="djycqymain" style="margin-top: 10px;">
                                                        <div id="djycqy_show">
                                                            <asp:Button ID="btnSave" runat="server" CssClass="tj_bt_da" Text="审核通过" Height="30"
                                                                Width="70" OnClientClick="return Promot(this);" OnClick="btnSave_Click" UseSubmitBehavior="False" />
                                                            &nbsp;&nbsp;
                                    
                                                <asp:Button ID="btnBoHui" runat="server" CssClass="tj_bt_da" Text="驳回" Height="30"
                                                    Width="70" OnClientClick="return PromotBohui(this);" OnClick="btnBoHui_Click" />&nbsp;&nbsp;
                                                <asp:Button ID="btnCancle" runat="server" CssClass="tj_bt_da" Text="返回列表" Height="30"
                                                    Width="70" OnClick="btnCancle_Click" />
                                                        </div>
                                                    </div>
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
