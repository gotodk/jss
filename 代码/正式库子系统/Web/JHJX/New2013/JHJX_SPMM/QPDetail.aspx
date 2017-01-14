<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QPDetail.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_SPMM_QPDetail" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 

</head>
<body>
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="950px">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="" Text="清盘详情"
                ForeColor="Red">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="export" runat="server" style="width: 800px; margin-left: 30px; margin-right: auto;
        margin-top: 20px">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="word-break: break-all;">
            <tr>
                <th colspan="4" align="center" valign="middle" style="font-size: 16px; font-family: 宋体;
                    font-weight: bold; height: 30px">
                    清盘详情
                </th>
            </tr>
            <tr style="height: 16px">
                <td align="right" width="10%" style="font-size: 9pt; font-family: 宋体;">
                    &nbsp;</td>
                <td align="left" width="30%" style="font-size: 9pt; font-family: 宋体;">
                    &nbsp;</td>
                <td align="right" style="font-size: 9pt; font-family: 宋体;">
                    客户确认日期:
                </td>
                <td align="left" width="20%" style="font-size: 9pt; font-family: 宋体;">
                    <div runat="server" id="divQRRQ" style="width: 100%" />
                </td>
            </tr>
        </table>
        <hr class="content_lx" style="margin-top: 0px; border: 0px; border-top: solid 1px #a5cbe2;
            height: 0px;" />
        <div style="font-size: 14px; font-family: 宋体; font-weight: bold; height: 25px; vertical-align: middle;">
            <b><span>电子购货合同（金额单位：元）</span></b></div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="font-size: 12px;
            font-family: 宋体; word-break: break-all; border-collapse: collapse;" bordercolor="#D4D4D4">
            <tr>
                <td style="width: 14%; text-align: right; vertical-align: middle; height: 27px">
                    合同编号：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divHTBH" />
                </td>
                <td style="width: 16%; text-align: right; vertical-align: middle; height: 27px">
                    合同结束日期：</td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divHTJSRQ" />
                </td>
            </tr>
            <tr>
                <td style="width: 14%; text-align: right; vertical-align: middle; height: 27px">
                    商品名称：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divSPMC" />
                </td>
                <td style="width: 16%; text-align: right; vertical-align: middle; height: 27px">
                    商品编号：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divSPBH" />
                </td>
            </tr>
            <tr>
                <td style="width: 14%; text-align: right; vertical-align: middle; height: 27px">
                    商品规格：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divSPGG" />
                </td>
                <td style="width: 16%; text-align: right; vertical-align: middle; height: 27px">
                    清盘状态：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divQPZT" />
                </td>
            </tr>
            <tr>
                <td style="width: 14%; text-align: right; vertical-align: middle; height: 27px">
                    合同数量：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divHTSL" />
                </td>
                <td style="width: 16%; text-align: right; vertical-align: middle; height: 27px">
                    单价：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divDJ" />
                </td>
            </tr>
            <tr>
                <td style="width: 14%; text-align: right; vertical-align: middle; height: 27px">
                    合同金额：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divHTJE" />
                </td>
                <td style="width: 16%; text-align: right; vertical-align: middle; height: 27px">
                    货款冻结金额：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divHKDJJE" />
                </td>
            </tr>
            <tr>
                <td style="width: 14%; text-align: right; vertical-align: middle; height: 27px">
                    履约保证金：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divLYBZJ" />
                </td>
                <td style="width: 16%; text-align: right; vertical-align: middle; height: 27px">
                    争议数量：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divZYSL" />
                </td>
            </tr>
            <tr>
                <td style="width: 14%; text-align: right; vertical-align: middle; height: 27px">
                    争议金额：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divZYJE" />
                </td>
                <td style="width: 16%; text-align: right; vertical-align: middle; height: 27px">
                    清盘原因：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divQPYY" />
                </td>
            </tr>
            <tr>
                <td style="width: 14%; text-align: right; vertical-align: middle; height: 27px">
                    清盘开始时间：
                </td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divQPKSSJ" />
                </td>
                <td style="width: 16%; text-align: right; vertical-align: middle; height: 27px">
                    清盘结束时间：</td>
                <td style="width: 35%; text-align: left; vertical-align: middle; height: 27px">
                    <div runat="server" id="divQPJSSJ" />
                </td>
            </tr>          
           
        </table>
        <div style="font-size: 14px; font-family: 宋体; font-weight: bold; height: 25px; vertical-align: middle;
            margin-top: 15px;">
            <b><span>争议双方协商结果</span></b></div>
         <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="font-size: 12px;
            font-family: 宋体; word-break: break-all; border-collapse: collapse;" bordercolor="#D4D4D4">
            <tr>
                <td style="width: 14%; text-align: right; vertical-align: middle; height:30px">
                    处理依据：
                </td>
                <td>

                    <div runat="server" id="divCLYJ" />
                
                </td>
               
            </tr>
             <tr >
                <td style="width: 14%; text-align: right; vertical-align: middle; height: 73px">
                    处理说明：
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtCLSM" runat="server" Width="77%" Height="70px" CssClass="tj_input"
                        TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>  
            
            <tr >
                <td align="right" style="width: 14%; font-size: 12px; height: 30px">
                    证明文件：
                </td>
                <td colspan="3" align="left" valign="middle">
                    <a id="lwjck"  runat="server" href="" target="_blank">查看</a></td>
            </tr>           
            
           
        </table>     
        <div style="font-size: 14px; font-family: 宋体; font-weight: bold; height: 25px; vertical-align: middle;
            margin-top: 15px;">
            <b><span>争议双方结果确认</span></b></div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="font-size: 12px;
            font-family: 宋体; word-break: break-all; border-collapse: collapse;" bordercolor="#D4D4D4">
            
                        
            <tr >
                <td style="width: 14%; text-align: right; vertical-align: middle; height: 73px">
                    结果确认：
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtJGQR" runat="server" Width="77%" Height="70px" CssClass="tj_input"
                        TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>           
           
            <tr>
                <td colspan="2" style="display: none;">
                    &nbsp;</td>
            </tr>
        </table>
        <div style="font-size: 14px; font-family: 宋体; font-weight: bold; height: 25px; vertical-align: middle;
            margin-top: 15px;">
            平台处理意见</div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="font-size: 12px;
            font-family: 宋体; word-break: break-all; border-collapse: collapse;" bordercolor="#D4D4D4">
            
                        
            <tr >
                <td style="text-align: right; vertical-align: middle; width: 14%;" >
                    处理意见：
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtPTYJ" runat="server" Width="77%" Height="70px" CssClass="tj_input"
                        TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr >
                <td style="text-align: right; vertical-align: middle; " >
                </td>
                <td colspan="3" class="auto-style2">
                    </td>
            </tr>
            <tr>
                <td align="center" colspan="4" style="height: 100px">                                        
                        <asp:Button ID="btnGoBack" runat="server" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da" Text="返回列表" OnClick="btnGoBack_Click" Height="30px" Width="80px" />
                            &nbsp;&nbsp;                        
                  
                </td>
            </tr>
            <tr>
                <td colspan="2" style="display: none;">
                    &nbsp;</td>
            </tr>
        </table>
    </div>   
    </form>
</body>

</html>
