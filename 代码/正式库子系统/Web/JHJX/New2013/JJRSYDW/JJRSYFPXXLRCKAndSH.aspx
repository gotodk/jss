<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JJRSYFPXXLRCKAndSH.aspx.cs" Inherits="Web_JHJX_New2013_JJRSYDW_JJRSYFPXXLRCKAndSH" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <%--<link href="../../../../css/style.css" rel="Stylesheet" type="text/css" />--%>
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 
    <script src="../../../../js/standardJSFile/fcf.js" type="text/javascript"></script>

    <script lang="ja" type="text/javascript">
        
        function printsetup() {
            // 打印页面设置 
            wb.execwb(8, 1);
        }
        function printpreview() {
            // 打印页面预览            
            wb.execwb(7, 1);
        }

        function Promot(this_an) {
            if ($("#txtSHR").val() == "")
            {
                window.top.Dialog.alert('审核人不能为空！');
                return false;
            }
            if ($("#txtSHTime").val() == "") {
                window.top.Dialog.alert('审核时间不能为空！');
                return false;
            }            
            return art_confirm_fcf(this_an, "您确定要审核吗？审核后将不可更改！", 'clickyc()');
        }

    </script>
    <!--用本样式在打印时隐藏非打印项目-->
    <style type="text/css" media="print">
        .Noprint
        {
            display: none;
        }
        
    </style>
</head>
<body style=" background-color:#f7f7f7;">
        <form id="form2" runat="server">
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw" runat="server" style="margin: auto auto; width:1000px;">
                <div class="content_bz">
                    <%--1、该模块用于记录已签约服务商以个人名义打款的打款人信息以及与服务商的对应关系。<br />
                    2、保存时系统根据规则自动生成正式客户编号，打款人编号以"6"开头，用生成的编号录入ERP。<br />
                    3、“所属服务商编号”请填写本办事处销售渠道为“服务商”或“门店服务商”的客户编号。--%>
                </div>
                <div style="width: 100%;">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="3" class="BT">
                                <span>发票信息</span></td>
                        </tr>
                        <tr>
                            <td align="left"style=" width:35%; height: 25px; vertical-align: bottom; padding-left: 10px;">
                                单号：<span runat="server" id="spanDH"></span>
                            </td>
                            <td align="center" style=" width:30%; vertical-align: bottom; padding-right: 20px;">
                                审核状态：<span runat="server" id="spanZT"></span>
                            </td>
                            <td align="right" style=" width:35%; height: 25px; vertical-align: bottom; padding-right:10px;">
                                发票信息录入时间：<span runat="server" id="spanFPLRTime"></span>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr class="content_lx" style="margin-top: 0px; border: 0px; border-top: solid 1px #a5cbe2;
                    height: 0px;" />
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" bordercolor="#D4D4D4">
                        <tr>
                            <td class="LBBT" style="padding-left: 10px">
                                基本信息
                            </td>
                        </tr>
                    </table>
                    <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0" bordercolor="#D4D4D4">
                        <tr>
                            <td  align="left" valign="middle" style="width:35%; height: 25px; padding-left: 10px">
                                经纪人编号：<span runat="server" id="spanJJRBH"></span>
                            </td>
                            <td align="left" style="width:30%;">
                                经纪人名称：<span runat="server" id="spanJJRMC" style=" width:150px; word-wrap:break-word; line-height:18px;">
                                    
                                      </span>
                            </td>
                            <td align="left" style="width:35%;">
                                所属分公司：<span runat="server" id="spanSSFGS"></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="middle" style="width:35%; height: 25px; padding-left: 10px">
                                第三方存管状态：<span runat="server" id="spanDSF"></span>
                            </td>
                            <td align="left" style="width:30%;">
                                缺票收益金额：<span runat="server" id="spanQPSY"></span>
                            </td>
                            <td align="left" style="width:35%;">
                                本次收到合格发票金额：<span runat="server" id="spanBCHGFPJE"></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="middle" style="width:35%; height: 25px; padding-left: 10px">
                                本次可支取收益金额：<span runat="server" id="spanBCKZQSYJE"></span>
                            </td>
                            <td align="left" style="width:30%;">
                                发票收取时间：<span runat="server" id="spanFPSQTime"></span>
                            </td>
                            <td align="left" style="width:35%;">
                                发票是否已入账：<span runat="server" id="spanFPSFYRZ"></span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left" valign="middle" style="height: 25px; padding-left: 10px">
                                备注：<span runat="server" id="spanBZ" style=" width:600px; word-wrap:break-word; line-height:18px;"></span>
                            </td>                            
                        </tr>
                        <tr>
                            <td align="left" valign="middle" style="width:30%; height: 25px; padding-left: 10px">
                                <table>
                                    <tr>
                                        <td>审核人：</td>
                                        <td><span runat="server" id="spanSHR"></span>
                                            <asp:TextBox ID="txtSHR" runat="server" Width="178px" CssClass="tj_input"></asp:TextBox> </td>
                                    </tr>
                                </table>                                
                            </td>
                            <td align="left" style="width:30%; vertical-align:middle;">
                                <table>
                                    <tr>
                                        <td id="tdSHTime" runat="server">审核时间：</td>
                                        <td><span runat="server" id="spanSHTime"></span>
                                <%--<asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd HH:mm:ss'});"
                                ID="txtSHTime" runat="server" Width="178px"></asp:TextBox>--%>

                                        </td>
                                    </tr>
                                </table>  
                                
                            </td>
                            <td align="left" style="width:35%;">
                                                
                            </td>
                        </tr>
                    </table>                
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="Noprint" >
                        <tr>
                            <td style="height: 20px;">
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                 <div id="djycqymain" style=" margin-top:10px;">
                                <div id="djycqy_show">
                                        <asp:Button id="button_yl" runat="server" Text="打印预览" OnClientClick="javascript:printpreview();"
                                            CssClass="tj_bt_da" usesubmitbehavior="False" />
                                        &nbsp;&nbsp;
                                        <%--<asp:Button ID="btnSCQJB" runat="server" Text="导出" OnClick="btnSCQJB_Click" CssClass="tj_bt_da"
                                            UseSubmitBehavior="False" />--%>
                                        <asp:Button ID="btnSH" runat="server" Text="审核" CssClass="tj_bt_da"
                                            UseSubmitBehavior="False" OnClientClick="return Promot(this);" OnClick="btnUpdate_Click" />
                                    &nbsp;&nbsp;
                                                <asp:Button ID="btnGoBack" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da" Text="返回列表" OnClick="btnGoBack_Click" Visible="false" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
    <object classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2" height="0" id="wb" name="wb"
        width="0">
    </object>

</body>
</html>
