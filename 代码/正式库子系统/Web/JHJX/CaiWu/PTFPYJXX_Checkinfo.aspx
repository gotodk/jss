<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PTFPYJXX_Checkinfo.aspx.cs"
    Inherits="Web_JHJX_PTFPYJXX_Checkinfo" %>

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
            <radTS:Tab ID="Tab1" runat="server"  Text="平台发票邮递信息审核"  ForeColor="Red" >
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
                                    发票信息 <span runat="server" id="spanNumber" visible="false"></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="text-align: right; height: 25px; width: 120px">
                                    发票单号：
                                </td>
                                <td style="text-align: left; width: 180px">
                                    <span runat="server" id="spanFPDB"></span><span runat="server" id="span2">_</span><span
                                        runat="server" id="spanFPDH"></span>
                                </td>
                                <td style="width: 100px; text-align: right; height: 25px">
                                    平台管理机构：
                                </td>
                                <td style="text-align: left; width: 300px">
                                    <span runat="server" id="spanPTGLJG"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    客户编号：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanKHBH"></span>
                                </td>
                                <td style="text-align: right; height: 25px">
                                    客户全称：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanKHQC"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    发票类型：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanFPLX"></span>
                                </td>
                                <td style="text-align: right;">
                                    发票金额：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanFPJE"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 25px">
                                    发票号码：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanFPHM"></span>
                                </td>
                                <td style="text-align: right;">
                                    生成时间：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanSHSJ"></span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="content_lx" style="width: 700px">
                    </div>
                    <table width="700px" class="Message" runat="server" id="tableZYFPXX">
                        <tr>
                            <th class="TitleTh" style="padding-left: 15px; width: 105px">
                                发票邮递信息
                            </th>
                            <th style="width: 580px; text-align: left; font-size: 9pt; color: Red">
                                <span runat="server" id="spanFPJSBZ"></span>
                            </th>
                        </tr>
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
                                    收件人姓名及电话：
                                </td>
                                <td style="text-align: left;">
                                    <span runat="server" id="spanFPJSLXR"></span>&nbsp;&nbsp; <span runat="server" id="spanFPJSLXRDH">
                                    </span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="content_lx" style="width: 700px">
                    </div>
                    <table width="700px" class="Message" id="tableJBXX" runat="server">
                        <thead>
                            <tr>
                                <th class="TitleTh" colspan="2" style="padding-left: 15px">
                                    物流信息
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="text-align: right; height: 30px; width: 120px">
                                    物流公司：
                                </td>
                                <td style="text-align: left; width: 180px">
                                    <span runat="server" id="spanWLGS"></span>
                                </td>
                                <td style="text-align: right; width: 100px">
                                    物流单号：
                                </td>
                                <td style="text-align: left; width: 300px">
                                    <span runat="server" id="spanWLDH"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 30px; width: 120px">
                                    录入人：
                                </td>
                                <td style="text-align: left; width: 180px">
                                    <span runat="server" id="spanLRR"></span>
                                </td>
                                <td style="text-align: right; width: 100px">
                                    录入时间：
                                </td>
                                <td style="text-align: left; width: 300px">
                                    <span runat="server" id="spanLRSJ"></span>
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
                                <td style="text-align: right; height: 30px; width: 120px">
                                    审核状态：
                                </td>
                                <td style="text-align: left; width: 580px">
                                    <span runat="server" id="spanSHZT"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; height: 70px; width: 120px">
                                    审核备注：
                                    <br />
                                    <font color="red">（150字以内）</font>
                                </td>
                                <td style="text-align: left; width: 580px">
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
                                                    Width="70" OnClick="btnSave_Click" OnClientClick="javascript:return confirm('您确定要审核通过吗？');" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnZuoFei" runat="server" CssClass="tj_bt_da" Text="作废" Height="30"
                                                    Width="70" OnClick="btnZuoFei_Click" OnClientClick="javascript:return confirm('您确定要将该单据作废吗？');" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnCancle" runat="server" CssClass="tj_bt_da" Text="返回列表" Height="30"
                                                    Width="70" OnClick="btnCancel_Click" />
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
