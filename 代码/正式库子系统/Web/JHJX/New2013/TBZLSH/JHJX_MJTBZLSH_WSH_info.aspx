<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_MJTBZLSH_WSH_info.aspx.cs" Inherits="Web_JHJX_New2013_TBZLSH_JHJX_MJTBZLSH_WSH_info" %>

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
            var textYJ = $("#txtSHYJ").val();
            textYJ = $.trim(textYJ);
            if (textYJ.toString() == "")
            {
                window.top.Dialog.WarningConfirm('审核意见不能为空，请填写审核意见！');
                return false;
            }
            else if (textYJ.toString().length>1000)
            {
                window.top.Dialog.WarningConfirm('您输入的审核意见字符过长，请限制在1000个字符以内！');
                return false;
            }

            return art_confirm_fcf(this_an, "您确认审核该投标单为“正常吗”？", 'clickyc()');
        }
        function PromotBohui(this_an) {
            var textYJ = $("#txtSHYJ").val();
            textYJ = $.trim(textYJ);
            if ($("input[type=checkbox][name=typeZZ]:checked").length <= 0) {
                window.top.Dialog.WarningConfirm('请选择异常投标资质！');
                return false;
            }
            else if (textYJ.toString() == "") {
                window.top.Dialog.WarningConfirm('审核意见不能为空，请填写审核意见！');
                return false;
            }
            else if (textYJ.toString().length > 1000) {
                window.top.Dialog.WarningConfirm('您输入的审核意见字符过长，请限制在1000个字符以内！');
                return false;
            }
            

            return art_confirm_fcf(this_an, "您确认审核该投标单为“异常”吗？", 'clickyc()');
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
                                    <th class="TitleTh" colspan="4" style="padding-left: 15px">交易方资质
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr id="trYYZZ" runat="server">
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px">营业执照：
                                    </td>
                                    <td style="text-align: left; width: 150px; line-height: 18px;">
                                        <span runat="server" id="spanYYZZZCH" style="line-height: 18px;"></span>
                                    </td>
                                    <td style="width: 400px; text-align: left;">
                                        &nbsp;&nbsp;<a id="linkYYZZ" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                  
                                </tr>
                                 <tr id="trZZJGDMZ" runat="server">
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px">组织机构代码：
                                    </td>
                                    <td style="text-align: left; width: 150px; line-height: 18px;">
                                        <span runat="server" id="spanZZJGDMZDM" style="line-height: 18px;"></span>
                                    </td>
                                    <td style="width: 400px; text-align: left;">
                                        &nbsp;&nbsp;<a id="linkZZJGDMZ" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                   
                                </tr>
                                 <tr id="trSWDJZ" runat="server">
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px">税务登记证：
                                    </td>
                                    <td style="text-align: left; width: 150px; line-height: 18px;">
                                        <span runat="server" id="spanSWDJZSH" style="line-height: 18px;"></span>
                                    </td>
                                    <td style="width: 400px; text-align: left;">
                                        &nbsp;&nbsp;<a id="linkSWDJZ" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                   
                                </tr>
                                 <tr id="trFDDBRSFZ" runat="server">
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px">法定代表人身份证：
                                    </td>
                                    <td style="text-align: left; width: 150px; line-height: 18px;">
                                        <span runat="server" id="spanFDDBRSFZ" style="line-height: 18px;"></span>
                                    </td>
                                    <td style="width: 400px; text-align: left;">
                                        &nbsp;&nbsp;<a id="linkFDDBRSFZSMJ" class="link" runat="server" href="" target="_blank">正面查看</a>  &nbsp;&nbsp;<a id="linkFDDBRSFZSMJ_FM" class="link" runat="server" href="" target="_blank">反面查看</a>
                                    </td>
                                   
                                </tr>
                                  <tr id="trYLYJK" runat="server">
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px">预留印鉴卡：
                                    </td>
                                    <td style="text-align: left; width: 150px; line-height: 18px;">
                                      &nbsp;&nbsp;<a id="linkYLYJKSMJ" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                    <td style="width: 400px; text-align: left;">
                                     
                                    </td>
                                   
                                </tr>
                                <tr id="trSFZ" runat="server">
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px">身份证：
                                    </td>
                                    <td style="text-align: left; width: 150px; line-height: 18px;">
                                        <span runat="server" id="spanSFZH" style="line-height: 18px;"></span>
                                    </td>
                                    <td style="width: 400px; text-align: left;">
                                        &nbsp;&nbsp;<a id="linkSFZ" class="link" runat="server" href="" target="_blank">正面查看</a>  &nbsp;&nbsp;<a id="linkSFZ_FM" class="link" runat="server" href="" target="_blank">反面查看</a>
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
                                    <td style="text-align: left; width: 40px">&nbsp;&nbsp;<a id="AZLBZ" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                    <td style="text-align: left; width: 510px"> <input type="checkbox" name="typeZZ" value="质量标准与证明"/>   </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">产品检测报告：
                                    </td>
                                    <td style="text-align: left;">&nbsp;&nbsp;<a id="ACPJCBG" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                    <td><input type="checkbox" name="typeZZ" value="产品检测报告"/></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">品管总负责人法律承诺书：
                                    </td>
                                    <td style="text-align: left;">&nbsp;&nbsp;<a id="APGZFZRFLCNS" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                      <td><input type="checkbox" name="typeZZ" value="品管总负责人法律承诺书"/></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">法定代表人承诺书：
                                    </td>
                                    <td style="text-align: left;">&nbsp;&nbsp;<a id="AFDDBRCRS" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                              <td><input type="checkbox" name="typeZZ" value="法定代表人承诺书"/></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">售后服务规定与承诺：
                                    </td>
                                    <td style="text-align: left;">&nbsp;&nbsp;<a id="ASHFWGD" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                           <td><input type="checkbox" name="typeZZ" value="售后服务规定与承诺"/></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 25px">产品送检授权书：
                                    </td>
                                    <td style="text-align: left;">&nbsp;&nbsp;<a id="ACPSJSQS" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                       <td><input type="checkbox" name="typeZZ" value="产品送检授权书"/></td>
                                </tr>
                               <%--  <tr>
                                    <td style="text-align: right; height: 25px">税率证明：
                                    </td>
                                    <td style="text-align: left;">&nbsp;&nbsp;<a id="ASLZM" class="link" runat="server" href="" target="_blank">查看</a>
                                    </td>
                                        <td><input type="checkbox" name="typeZZ" value="税率证明"/></td>
                                </tr>--%>
                             
                            </tbody>
                        </table>
                           <table   width="700px" class="Message" cellspacing="0" cellpadding="0">
                               <tbody>
                                   <asp:Repeater ID="rptTSZZ" runat="server">
                                       <ItemTemplate>
                                        <tr>
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px"><%#Eval("特殊资质名称")%>：
                                    </td>
                                    <td style="text-align: left; width:40px">&nbsp;&nbsp;<a  class="link"  href='<%#Eval("特殊资质路径")%>' target="_blank">查看</a>
                                    </td>
                                      <td style="text-align: left; width:510px"><input type="checkbox"  name="typeZZ" value='<%#Eval("特殊资质名称")%>'/>
                                    </td>
                                </tr>
                                           </ItemTemplate>
                                   </asp:Repeater>
                               </tbody>
                         </table>
                        <div runat="server" id="divTSZZ"></div>
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
                                                            <asp:Button ID="btnZhengChang" runat="server" CssClass="tj_bt_da" Text="正常" Height="30"
                                                                Width="70" OnClientClick="return Promot(this);"  UseSubmitBehavior="False" OnClick="btnZhengChang_Click" />
                                                            &nbsp;&nbsp;
                                    
                                                <asp:Button ID="btnYiChang" runat="server" CssClass="tj_bt_da" Text="异常" Height="30"
                                                    Width="70" OnClientClick="return PromotBohui(this);" OnClick="btnYiChang_Click" />&nbsp;&nbsp;
                                                <asp:Button ID="btnBack" runat="server" CssClass="tj_bt_da" Text="返回列表" Height="30"
                                                    Width="70" OnClick="btnBack_Click"  />
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