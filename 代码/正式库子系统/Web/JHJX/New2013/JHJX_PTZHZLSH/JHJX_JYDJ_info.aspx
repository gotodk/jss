<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_JYDJ_info.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_PTZHZLSH_JHJX_JYDJ_info" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../../../css/style.css" rel="Stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/fcf.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        //浮点数
        function CheckInputIntFloat(oInput) {
            if ('' != oInput.value.replace(/\d{1,}\.{0,1}\d{0,2}/, '')) {
                oInput.value = oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/) == null ? '' : oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/);
            }
        }
    </script>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="99%" BackColor="#f7f7f7">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server"  ForeColor="red"
                    Text="交易方详情">
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
                        <table width="850px" class="Message">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="4" style="padding-left: 15px">交易方基本信息
                                    </th>
                                </tr>
                            </thead>

                        </table>
                        <table width="850px" class="Message" id="tabf" runat="server">

                            <tr id="trJYFZH_MC" runat="server">
                                <td style="text-align: right; width: 200px; height: 25px">交易方账号：
                                </td>
                                <td style="text-align: left; width: 225px">
                                    <asp:Label ID="lbl_JYFZH" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;width:180px;">交易方名称： </td>
                                <td style="text-align: left; width: 245px">
                                    <asp:Label ID="lbl_JYFMC" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trJYZHLX_LB" runat="server">
                                <td style="text-align: right; height: 25px">交易账户类型：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_JYZHLX" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">注册类别：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_ZCLB" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trJYFSFZ" runat="server">
                                <td style="text-align: right; height: 25px">身份证号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_SFZH" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">身份证扫描件：
                                </td>
                                <td style="text-align: left;">
                                    <a id="linkSFZSMJ" class="link" runat="server" href="" target="_blank">正面查看</a>&nbsp;&nbsp; <a id="linkSFZSMJ_FM" class="link" runat="server" href="" target="_blank">反面查看</a>
                                </td>
                            </tr>
                            <tr id="trJYFYYZZ" runat="server">
                                <td style="text-align: right; height: 25px">营业执照注册号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_YYZZZCH" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">营业执照扫描件：
                                </td>
                                <td style="text-align: left;">
                                    <a id="linkYYZZSMJ" class="link" runat="server" href="" target="_blank">查看</a>
                                </td>
                            </tr>
                            <tr id="trZZJGDMZ" runat="server">
                                <td style="text-align: right; height: 25px">组织机构代码证代码：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_ZZJGDMZDM" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">组织机构代码证扫描件：
                                </td>
                                <td style="text-align: left;">
                                    <a id="linkZZJGDMZSMJ" class="link" runat="server" href="" target="_blank">查看</a>
                                </td>
                            </tr>
                            <tr id="trSWDJZ" runat="server">
                                <td style="text-align: right; height: 25px">税务登记证税号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_SWDJZSH" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">税务登记证扫描件：
                                </td>
                                <td style="text-align: left;">
                                    <a id="linkSWDJZSMJ" class="link" runat="server" href="" target="_blank">查看</a>
                                </td>
                            </tr>
                            <tr id="trYBNSRZGZ" runat="server">
                                <td style="text-align: right; height: 25px" runat="server" visible="false">一般纳税人资格证明扫描件：
                                </td>
                                <td style="text-align: left;" runat="server" visible="false">
                                    <a id="linkYBNSRZGZMSMJ" class="link" runat="server" href="" target="_blank">查看</a>
                                </td>
                                <td style="text-align: right;">法定代表人授权书扫描件：
                                </td>
                                <td style="text-align: left;">
                                    <a id="linkFDDBRSQSSMJ" class="link" runat="server" href="" target="_blank">查看</a>
                                </td>
                            </tr>
                            <tr id="trKHXKZ" runat="server">
                                <td style="text-align: right; height: 25px">开户许可证号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_KHXKZH" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">开户许可证扫描件：
                                </td>
                                <td style="text-align: left;">
                                    <a id="linkKHXKZSMJ" class="link" runat="server" href="" target="_blank">查看</a>
                                </td>
                            </tr>
                            <tr id="trYLYJK" runat="server">
                                <td style="text-align: right; height: 25px">预留印鉴卡：
                                </td>
                                <td style="text-align: left;">
                                    <a id="linkYLYJK" class="link" runat="server" href="" target="_blank">查看</a>
                                </td>
                                <td style="text-align: right;">法定代表人姓名：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_FDDBRXM" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trFDDBRSFZ" runat="server">
                                <td style="text-align: right; height: 25px">法定代表人身份证号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_FDDBRSHZH" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">法定代表人身份证扫描件：
                                </td>
                                <td style="text-align: left;">

                                    <a id="linkFDDBRSFZSMJ" class="link" runat="server" href="" target="_blank">正面查看</a>&nbsp;&nbsp;     <a id="linkFDDBRSFZSMJ_FM" class="link" runat="server" href="" target="_blank">反面查看</a>
                                </td>
                            </tr>
                            <tr id="trJYFLXDH" runat="server">
                                <td style="text-align: right; height: 25px">交易方联系电话：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_JYFLXDH" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">所属区域：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_SSQY" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trXXDZ" runat="server">
                                <td style="text-align: right; height: 25px">详细地址：
                                </td>
                                <td style="text-align: left;" colspan="3">
                                  
                                    <div style=" width:600px;word-wrap:break-word;">      <asp:Label ID="lblXXDZ" runat="server"></asp:Label></div>
                                </td>
                            </tr>
                            <tr id="trLXRXM" runat="server">
                                <td style="text-align: right; height: 25px">联系人姓名：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_LXRXM" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">联系人手机号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_LXRSJH" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trKHYH" runat="server">
                                <td style="text-align: right; height: 25px">开户银行：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_KHYH" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">银行账号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_YHZH" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trPTGLJG" runat="server">
                                <td style="text-align: right; height: 25px">平台管理机构：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_PTGLJG" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">&nbsp;</td>
                                <td style="text-align: left;">&nbsp;
                                </td>
                            </tr>
                            <tr id="trGLJJR" runat="server">
                                <td style="text-align: right; height: 25px">关联经纪人资格证书编号：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_GLJJRZGZSBH" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">关联经纪人名称：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_GLJJRMC" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trGLJJRDH" runat="server">
                                <td style="text-align: right; height: 25px">关联经纪人联系电话：
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lbl_GLJJRLXDH" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;">&nbsp;</td>
                                <td style="text-align: left;">&nbsp;</td>
                            </tr>

                        </table>
                        <div class="content_lx" style="width: 850px">
                        </div>
                        <table width="850px" class="Message" id="tabJJRSH" runat="server">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="2" style="padding-left: 15px">经纪人审核
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="text-align: right; width: 190px; height: 25px;">审核人：
                                                    </td>
                                                    <td style="text-align: left; width: 150px">

                                                        <asp:Label ID="lbl_JJR_SHR" runat="server"></asp:Label>

                                                    </td>
                                                    <td style="text-align: right;">审核时间：</td>
                                                    <td style="text-align: left; width: 300px">

                                                        <asp:Label ID="lbl_JJR_SHSJ" runat="server"></asp:Label>


                                                    </td>
  </tr>
                                              
                                                <tr>
                                                    <td style="text-align: right; height: 25px; width: 190px">审核意见：
                                                    </td>
                                                    <td style="text-align: left;" colspan="3">

                                                        <asp:TextBox ID="txt_JJR_SHYJ" runat="server" CssClass="tj_input" Width="541px" Enabled="false"
                                                            Height="100px" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>

                              

                            </tbody>
                        </table>

                        <table width="850px" class="Message" id="tabFGSSH" runat="server">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="2" style="padding-left: 15px">分公司审核
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="text-align: right; height: 25px; width: 190px">审核人：
                                    </td>
                                    <td style="text-align: left; width: 150px">

                                        <asp:Label ID="lbl_FGS_SHR" runat="server"></asp:Label>

                                    </td>
                                    <td style="text-align: right;">审核时间：</td>
                                    <td style="text-align: left; width: 300px">

                                        <asp:Label ID="lbl_FGS_SHSJ" runat="server"></asp:Label>


                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px; width: 190px">审核意见：
                                    </td>
                                    <td style="text-align: left;" colspan="3">

                                        <asp:TextBox ID="txt_FGS_SHYJ" runat="server" CssClass="tj_input" Width="541px" Enabled="false"
                                            Height="100px" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table width="850px" class="Message" id="tabFWZXSH" runat="server">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="2" style="padding-left: 15px">服务中心审核
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="text-align: right; height: 25px; width: 190px">审核人：
                                    </td>
                                    <td style="text-align: left; width: 150px">

                                        <asp:Label ID="lab_FWZXSH_SHR" runat="server"></asp:Label>

                                    </td>
                                    <td style="text-align: right;">审核时间：</td>
                                    <td style="text-align: left; width: 300px">

                                        <asp:Label ID="lab_FWZXSH_SHSJ" runat="server"></asp:Label>


                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px; width: 190px">审核意见：
                                    </td>
                                    <td style="text-align: left;" colspan="3">

                                        <asp:TextBox ID="txtFWZXSH_SHYJ" runat="server" CssClass="tj_input" Width="541px" Enabled="false"
                                            Height="100px" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table width="850px" class="Message" id="tabFWZXXSH" runat="server">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="2" style="padding-left: 15px">服务中心新审核
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="text-align: right; height: 25px; width: 190px">审核人：
                                    </td>
                                    <td style="text-align: left; width: 150px">

                                        <asp:Label ID="lab_FWZXXSH_SHR" runat="server"></asp:Label>

                                    </td>
                                    <td style="text-align: right;">审核时间：</td>
                                    <td style="text-align: left; width: 300px">

                                        <asp:Label ID="lab_FWZXXSH_SHSJ" runat="server"></asp:Label>


                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px; width: 190px">审核意见：
                                    </td>
                                    <td style="text-align: left;" colspan="3">

                                        <asp:TextBox ID="txt_FWZXXSH_SHYJ" runat="server" CssClass="tj_input" Width="541px" Enabled="false"
                                            Height="100px" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table width="850px" class="Message" id="tabJYGLBSH" runat="server">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="2" style="padding-left: 15px">交易管理部审核
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="text-align: right; height: 25px; width: 190px">审核人：
                                    </td>
                                    <td style="text-align: left; width: 150px">

                                        <asp:Label ID="lab_JYGLBSH_SHR" runat="server"></asp:Label>

                                    </td>
                                    <td style="text-align: right;">审核时间：</td>
                                    <td style="text-align: left; width: 300px">

                                        <asp:Label ID="lab_JYGLBSH_SHSJ" runat="server"></asp:Label>


                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px; width: 190px">审核意见：
                                    </td>
                                    <td style="text-align: left;" colspan="3">

                                        <asp:TextBox ID="txt_JYGLBSH_SHYJ" runat="server" CssClass="tj_input" Width="541px" Enabled="false"
                                            Height="100px" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table width="800px" class="Message" id="tab3" runat="server">

                            <tbody>
                                <tr>
                                    <td>
                                        <div class="content_lx" style="width: 850px">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 200px; height: 30px"></td>
                                                <td style="text-align: left;">
                                                    &nbsp;&nbsp;
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnBack" runat="server" CssClass="tj_bt_da" Text="返回列表" Height="30"
                                                    Width="80" OnClick="btnBack_Click" />
                                                   
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
