<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JJRSYFPXXLR.aspx.cs" Inherits="Web_JHJX_New2013_JJRSYDW_JJRSYFPXXLR" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../../../css/style.css" rel="Stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 
    <script src="../../../../js/standardJSFile/fcf.js" type="text/javascript"></script>

    <script lang="ja" type="text/javascript">
        //浮点数
        function CheckInputIntFloat(oInput) {
            if ('' != oInput.value.replace(/\d{1,}\.{0,1}\d{0,2}/, '')) {
                oInput.value = oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/) == null ? '' : oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/);

               
            }
            var a = 0.00;
            var b = 0.00;
            //本次收到合格发票金额
            if (oInput.value != "") {
                b = oInput.value;
                
            }
            //缺票收益金额
            if ($("#txtQPSYJE").val() != "") {
                a = $("#txtQPSYJE").val();
                
            }
            if (parseFloat(a) >= parseFloat(b)) {
                $("#txtKZQJE").val(b);
            }
            else {
                $("#txtKZQJE").val(a);
            }
        }
        $(document).ready(function () {
            $("#txtJJRBH").art_confirm(optsConfirm);            
        });
        var optsConfirm = {
            name: "diag1",
            width: 850,
            height: 470,
            title: "经纪人",
            url: "JHJX/New2013/JJRSYDW/JJR.aspx",
            showmessagerow: false,
            showbuttonrow: false,
            optionName: "optsConfirm",
            IsReady: function (object, dialog) {
                __artConfirmOperner.close();
                
                $("#txtJJRBH").val(object.JJRBH);
                $("#txtJJRMC").val(object.JJRMC);
                $("#txtSSFGS").val(object.FGSMC);
                $("#txtQPSYJE").val(object.QPSYJE);
                $("#txtDSFCGZT").val(object.DSFCGZT);
                $("#hidSYZJE").val(object.ZSY);
                $("#hidYZQJE").val(object.YZQSY);                
                var a = 0.00;
                var b = 0.00;
                //本次收到合格发票金额
                if ($("#txtHGFPJE").val()!="")
                {
                    b = $("#txtHGFPJE").val();
                }
                //缺票收益金额
                if($("#txtQPSYJE").val()!="")
                {
                    a = $("#txtQPSYJE").val();
                }
                if (parseFloat(a) >= parseFloat(b)) {
                    $("#txtKZQJE").val(b);
                }
                else {
                    $("#txtKZQJE").val(a);
                }
                
            }
        }
        function Promot(this_an) {
            if ($("#txtJJRBH").val()=="")
            {
                window.top.Dialog.alert('经纪人编号不能为空！');
                return false;
            }
            if ($("#txtJJRMC").val() == "") {
                window.top.Dialog.alert('经纪人名称不能为空！');
                return false;
            }
            if ($("#txtSSFGS").val() == "") {
                window.top.Dialog.alert('所属分公司不能为空！');
                return false;
            }
            if ($("#txtDSFCGZT").val() == "") {
                window.top.Dialog.alert('第三方存管状态不能为空！');
                return false;
            }
            if ($("#txtQPSYJE").val() == "") {
                window.top.Dialog.alert('缺票收益金额不能为空！');
                return false;
            }
            if ($("#txtHGFPJE").val() == "") {
                window.top.Dialog.alert('本次收到合格发票金额不能为空！');
                return false;
            }
            if ($("#txtKZQJE").val() == "") {
                window.top.Dialog.alert('本次可支取收益金额不能为空！');
                return false;
            }
            if ($("#txtFPSQTime").val() == "") {
                window.top.Dialog.alert('发票收取时间不能为空！');
                return false;
            }
            return art_confirm_fcf(this_an, "您确定要提交操作吗？", 'clickyc()');
        }
    </script>

</head>
<body style=" background-color:#f7f7f7;">
    
    <form id="form1" runat="server" style=" margin:0 0 0 0;">
        <span id="span"></span>
        <asp:HiddenField ID="hidSYZJE" runat="server" />
    <asp:HiddenField ID="hidYZQJE" runat="server" />
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="99%" BackColor="#f7f7f7">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JJRSYFPXXLR.aspx" Text="发票信息录入"
                    ForeColor="Red" Font-Size="12px">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <div id="new_content">
        <div id="new_zicontent">            
            <div id="content_zw">                
                <div class="content_bz">
                    <%--1、该模块用于记录已签约服务商以个人名义打款的打款人信息以及与服务商的对应关系。<br />
                    2、保存时系统根据规则自动生成正式客户编号，打款人编号以"6"开头，用生成的编号录入ERP。<br />
                    3、“所属服务商编号”请填写本办事处销售渠道为“服务商”或“门店服务商”的客户编号。--%>
                </div>
                <%--<div class="content_lx">
                </div>--%>
                <div class="content_nr">
                    <table width="100%" class="Message">
                        <thead>
                            <tr>
                                <th class="TitleTh">
                                    <span>经纪人</span>信息
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                           <tr>
                                <td style=" width:35%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:140px;overflow:hidden; text-align:right;">
                                                <span>经纪人编号</span>：
                                            </td>
                                            <td  style="text-align:left; ">
                                                <asp:TextBox ID="txtJJRBH" runat="server" onfocus="this.blur()" 
                                                class="tj_input_search" Width="200px"></asp:TextBox>
                                                <asp:TextBox ID="txtJJRBH1" runat="server" onfocus="this.blur()" 
                                                class="tj_input" Width="200px"></asp:TextBox>                            
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style=" width:65%;">
                            
                                </td>                            
                           </tr>
                           <tr>
                                <td style=" width:35%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:140px;overflow:hidden; text-align:right;">
                                                <span>经纪人名称</span>：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtJJRMC" runat="server" CssClass="tj_input" Width="200px" Enabled="True"
                                    TabIndex="1" onfocus="this.blur()"></asp:TextBox>
                                            
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style=" width:65%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:120px;overflow:hidden; text-align:right;">
                                                <span>所属分公司</span>：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtSSFGS" runat="server" CssClass="tj_input" Width="150px" Enabled="True"
                                    TabIndex="1" onfocus="this.blur()"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>                            
                           </tr>
                           <tr>
                                <td style=" width:35%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:140px;overflow:hidden; text-align:right;">
                                                第三方存管状态：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtDSFCGZT" runat="server" CssClass="tj_input" Width="200px" 
                                    TabIndex="1" onfocus="this.blur()"></asp:TextBox>
                                            
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style=" width:65%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:120px;overflow:hidden; text-align:right;">
                                                缺票收益金额：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtQPSYJE" runat="server" CssClass="tj_input" Width="150px" Enabled="True"
                                    TabIndex="1" onfocus="this.blur()"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>                            
                           </tr>
                           <tr>
                                <td style=" width:35%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:140px;overflow:hidden; text-align:right;">
                                                本次收到合格发票金额：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtHGFPJE" runat="server" CssClass="tj_input" Width="200px" Enabled="True"
                                    TabIndex="1" onkeyup="javascript:CheckInputIntFloat(this);"  onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]\.?/g,''))"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style=" width:65%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:120px;overflow:hidden; text-align:right;">
                                                本次可支取收益金额：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtKZQJE" runat="server" CssClass="tj_input" Width="150px" Enabled="True"
                                    TabIndex="1" onfocus="this.blur()"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>                            
                           </tr>
                            <tr>
                                <td style=" width:35%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <span class="span">*</span>
                                            </td>
                                            <td style=" width:140px;overflow:hidden; text-align:right;">
                                                发票收取时间：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:TextBox ID="txtFPSQTime" runat="server"  Width="200px"
                                   class="tj_input Wdate" onfocus="WdatePicker({readOnly:true,minDate:'2013-03-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style=" width:65%;">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left;">
                                                <%--<span class="span">*</span>--%>
                                            </td>
                                            <td style=" width:120px;overflow:hidden; text-align:right;">
                                                发票是否已入账：
                                            </td>
                                            <td  style="text-align:left;">
                                                <asp:DropDownList ID="dropFPSFYRZ" runat="server" Width="152px" CssClass="tj_input">
                                                    <asp:ListItem Value="是">是</asp:ListItem>
                                                    <asp:ListItem Value="否">否</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>                            
                           </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left; vertical-align:top;">
                                                
                                            </td>
                                            <td style=" width:140px;overflow:hidden; text-align:right; vertical-align:top;">
                                                备注：
                                            </td>
                                            <td  style="text-align:left; vertical-align:top;">
                                                <asp:TextBox ID="txtBZ" runat="server" class="tj_input" Width="518px" Height="80px"
                                    TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>                            
                           </tr>
                        </tbody>
                    </table>        
                    
                    <br />
                    <table width="100%" class="Message">
                        <tbody>
                           <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td style=" width:10px; text-align:left; vertical-align:top;">
                                                
                                            </td>
                                            <td style=" width:140px;overflow:hidden; text-align:right; vertical-align:top;">
                                                
                                            </td>
                                            <td  style="text-align:left; vertical-align:top;">
                                                <div id="djycqymain" style=" margin-top:10px;">
                                <div id="djycqy_show">
                                    <asp:Button ID="btnUpdate" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da"  Text="保存"  OnClientClick="return Promot(this);" OnClick="btnUpdate_Click"  />
                                                <asp:Button ID="btnSave" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da"  Text="保存"  OnClientClick="return Promot(this);" OnClick="btnSave_Click"  />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnCancel" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da" Text="取消"  OnClick="btnCancel_Click" />
                                     &nbsp;&nbsp;
                                                <asp:Button ID="btnGoBack" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da" Text="返回列表" OnClick="btnGoBack_Click" Visible="false" />
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
