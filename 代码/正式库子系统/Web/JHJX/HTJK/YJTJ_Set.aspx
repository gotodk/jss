<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YJTJ_Set.aspx.cs" Inherits="Web_JHJX_HTJK_YJTJ_Set" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>业务操作平台-预警条件维护</title>
    <link href="../../../css/style.css" rel="Stylesheet" type="text/css" />
    <script src="../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <link href="../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/standardJSFile/fcf.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function IPEditFinished(obj) {
            //IP输入框只能输入数字            
            var len = obj.value.length;
            var IPValue = obj.value.substring(len - 1, len);
            if (IPValue.charCodeAt() == 46 || IPValue.charCodeAt() == 32) {
                obj.value = obj.value.replace(/[^0-9]/g, '');
            }
            //数字
            else if (IPValue.charCodeAt() <= 47 || IPValue.charCodeAt() >= 58) {
                obj.value = obj.value.replace(/[^0-9]/g, '');
            }
        }
    </script>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server" style="margin: 0 0 0 0;">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="99%" BackColor="#f7f7f7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="YJTJ_Set.aspx" Text="预警条件维护" ForeColor="Red"
                Font-Size="12px">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_bz">
                    说明：<br />
                    1、该模块用于设置【预警信息查询】功能中各业务的预警条件，提交后即可生效。<br />
                    2、如需修改，请在此页面中进行修改，修改完成后提交，覆盖原来的设置。<br />
                    3、所有条件只允许输入大于0的正整数。
                </div>
                <div class="content_nr">
                    <table width="800px" class="Message">
                        <tr>
                            <th class="TitleTh" style="padding-left: 15px; width: 100px">
                                预警条件维护
                            </th>
                            <th colspan="2" style="text-align: left; font-size: 9pt; color: Red">
                            <span runat="server" id="spanNum" visible ="false" ></span>
                            </th>
                        </tr>
                        <tbody>
                            <tr>
                                <td colspan="3" style="text-align: left; height: 30px; padding-left: 10px">
                                    1、 履约结束前买家提醒：
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px; text-align: right; height: 30px">
                                    距离合同结束期
                                </td>
                                <td style="width: 70px; text-align: center;">
                                    <asp:TextBox ID="txtLVJSTX_Buyer" runat="server" class="tj_input" Width="80%" onkeyup="IPEditFinished(this)"
                                        Style="text-align: center" MaxLength="3"></asp:TextBox>
                                </td>
                                <td style="text-align: left; width: 600">
                                    天内，尚未完全下达提货单的买家予以汇总（不包括“即时”类合同）。
                                </td>
                            </tr>
                        </tbody>
                    </table>                  
                    <table width="800px" class="Message">
                        <tbody>
                            <tr>
                                <td colspan="3" style="text-align: left; height: 30px; padding-left: 10px">
                                    2、 履约结束前卖家提醒：
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px; text-align: right; height: 30px">
                                    距离合同结束期
                                </td>
                                <td style="width: 70px; text-align: center;">
                                    <asp:TextBox ID="txtLVJSTX_Saler" runat="server" class="tj_input" Width="80%" onkeyup="IPEditFinished(this)"
                                        Style="text-align: center" MaxLength="3"></asp:TextBox>
                                </td>
                                <td style="text-align: left; width: 600">
                                    天内，有尚未录入物流发货信息或发票信息的提货单的卖家予以汇总（不包括“即时”类合同）。
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table width="800px" class="Message">
                        <tbody>
                            <tr>
                                <td colspan="3" style="text-align: left; height: 30px; padding-left: 10px">
                                    3、延迟发货预警：
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px; text-align: right; height: 30px">
                                    距离最迟发货日
                                </td>
                                <td style="width: 70px; text-align: center;">
                                    <asp:TextBox ID="txtZCFHR" runat="server" class="tj_input" Width="80%" onkeyup="IPEditFinished(this)"
                                        Style="text-align: center" MaxLength="3"></asp:TextBox>
                                </td>
                                <td style="text-align: left; width: 600">
                                    天内，尚未录入物流发货信息的卖家予以汇总（不包括“即时”类合同）。
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table width="800px" class="Message">
                        <tbody>
                            <tr>
                                <td colspan="3" style="text-align: left; height: 30px; padding-left: 10px">
                                    4、履约保证金不足卖家：
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 230px; text-align: right; height: 30px">
                                    履约保证金小于等于原保证金金额
                                </td>
                                <td style="width: 70px; text-align: center;">
                                    <asp:TextBox ID="txtLYBZJ" runat="server" class="tj_input" Width="80%" onkeyup="IPEditFinished(this)"
                                        Style="text-align: center" MaxLength="3"></asp:TextBox>
                                </td>
                                <td style="text-align: left; width: 500">
                                    %的卖家予以汇总。
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table width="800px" class="Message" id="tableZYFPXX">
                        <tfoot>
                            <tr>
                                <td style="width: 100px; height: 50px">
                                </td>
                                <td style="text-align: left;" colspan="3" valign ="bottom" >
                                    <asp:Button ID="btnCommit" runat="server" CssClass="tj_bt_da" Text="确认提交" Height="30"
                                        Width="80" OnClick="btnCommit_Click" OnClientClick="javascript:return confirm('您确定要提交信息吗？');" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnReSet" runat="server" CssClass="tj_bt_da" Text="重置" Height="30"
                                        Width="80" OnClick="btnReSet_Click" OnClientClick="javascript:return confirm('您确定要重置页面信息吗？');" />
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
