<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_YCTBZLSH_JYGLBSH_info.aspx.cs" Inherits="Web_JHJX_New2013_TBZLSH_JHJX_YCTBZLSH_JYGLBSH_info" %>

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

        #CBXShow
        {
            display: none;
        }
    </style>
    <script lang="ja" type="text/javascript">
        function Promot(this_an) {
            var textYJ = $("#txtJYGLB").val();
            textYJ = $.trim(textYJ);
            if (textYJ.toString() == "") {
                window.top.Dialog.WarningConfirm('审核意见不能为空，请填写审核意见！');
                return false;
            }
            else if (textYJ.toString().length > 1000) {
                window.top.Dialog.WarningConfirm('您输入的审核意见字符过长，请限制在1000个字符以内！');
                return false;
            }
            return art_confirm_fcf(this_an, "您确认审核该投标单为“审核通过”吗？", 'clickyc()');
        }
        function PromotBohui(this_an) {


      
            var duigou = false;
            $('.zizhicss00').each(function () {
                
                if ($(this).attr('checked') == "checked")
                {
                    duigou = true;
                }
            });
            if (!duigou)
            {
                window.top.Dialog.WarningConfirm('请选择异常投标资质！');
                return false;
            }

            var textYJ = $("#txtJYGLB").val();
            textYJ = $.trim(textYJ);
            if (textYJ.toString() == "") {
                window.top.Dialog.WarningConfirm('审核意见不能为空，请填写审核意见！');
                return false;
            }
            else if (textYJ.toString().length > 1000) {
                window.top.Dialog.WarningConfirm('您输入的审核意见字符过长，请限制在1000个字符以内！');
                return false;
            }

            return art_confirm_fcf(this_an, "您确认审核该投标单为“审核未通过”吗？", 'clickyc()');
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
                        </table>
                        <%
                            System.Data.DataTable thisTBZZ = (System.Data.DataTable)ViewState["TBZZ"];
                        %>
                        <table width="700px" class="Message" cellspacing="0" cellpadding="0">
                            <tbody>
                                <% 

                                    for (int p = 0; p < thisTBZZ.Rows.Count; p++)
                                    {
                                        if (thisTBZZ.Rows[p]["是否异常资质"].ToString() == "是")
                                        {
                                %>

                                <tr>
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px"><%=thisTBZZ.Rows[p]["资质名称"].ToString()%>：
                                    </td>
                                    <td style="text-align: left; width: 40px">&nbsp;&nbsp;<a class="link" href='<%=thisTBZZ.Rows[p]["资质路径"].ToString()%>' target="_blank">查看</a>
                                    </td>


                                    <td style="text-align: left; width: 510px">
                                        <input type='checkbox' name='typeZZ' class="zizhicss00" value='<%=thisTBZZ.Rows[p]["资质名称"].ToString()%>' />

                                    </td>
                                </tr>
                                <%
                }
                else
                {
                                %>

  <tr>
                                    <td style="width: 150px; text-align: right; height: 25px; padding-left: 15px"><%=thisTBZZ.Rows[p]["资质名称"].ToString()%>：
                                    </td>
                                    <td style="text-align: left; width: 40px">&nbsp;&nbsp;<a class="link" href='<%=thisTBZZ.Rows[p]["资质路径"].ToString()%>' target="_blank">查看</a>
                                    </td>


                                    <td style="text-align: left; width: 510px">
                                      &nbsp;

                                    </td>
                                </tr>




                                <%
             }

            }
                                %>
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
                                    <td style="text-align: right; height: 25px; width: 150px">审核人：
                                    </td>
                                    <td style="text-align: left; width: 150px">

                                        <asp:Label ID="lbl_FWZX_SHR" runat="server"></asp:Label>

                                    </td>
                                    <td style="text-align: right;">审核时间：</td>
                                    <td style="text-align: left; width: 200px">

                                        <asp:Label ID="lbl_FWZX_SHSJ" runat="server"></asp:Label>


                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; height: 70px; width: 150px">审核意见：
                                    </td>
                                    <td style="text-align: left; width: 550px" colspan="3">
                                        <asp:TextBox ID="txtSHYJ" runat="server" CssClass="tj_input" Width="400px" Enabled="false"
                                            Height="80px" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="content_lx" style="width: 700px">
                        </div>
                        <table width="700px" class="Message" cellspacing="0" cellpadding="0">
                            <thead>
                                <tr>
                                    <th class="TitleTh" colspan="2" style="padding-left: 15px">交易管理部审核

                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="text-align: right; height: 70px; width: 150px">审核意见：
                                    </td>
                                    <td style="text-align: left; width: 550px" colspan="3">
                                        <asp:TextBox ID="txtJYGLB" runat="server" CssClass="tj_input" Width="400px" Enabled="true"
                                            Height="80px" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 150px; height: 30px"></td>
                                                <td style="text-align: left;">
                                                    <div id="djycqymain" style="margin-top: 10px;">
                                                        <div id="djycqy_show">
                                                            <asp:Button ID="btnPass" runat="server" CssClass="tj_bt_da" Text="审核通过" Height="30"
                                                                Width="70"  OnClientClick="return Promot(this);" OnClick="btnPass_Click" UseSubmitBehavior="False"  />&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnReject" runat="server" CssClass="tj_bt_da" Text="审核未通过" Height="30"
                                                                Width="90" OnClick="btnReject_Click" OnClientClick="return PromotBohui(this);" UseSubmitBehavior="False"  />&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnBack" runat="server" CssClass="tj_bt_da" Text="返回列表" Height="30"
                                                                Width="70" OnClick="btnBack_Click1" UseSubmitBehavior="False" />
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
