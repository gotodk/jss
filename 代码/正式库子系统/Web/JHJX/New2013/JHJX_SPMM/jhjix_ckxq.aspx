<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjix_ckxq.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_SPMM_jhjix_ckxq" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>商品买卖定标与保证函详情查看</title>
    <script type="text/javascript">
        function printsetup() {
            // 打印页面设置 
            wb.execwb(8, 1);
        }
        function printpreview() {
            // 打印页面预览 
            wb.execwb(7, 1);
        }
    </script>
    <!--用本样式在打印时隐藏非打印项目-->
    <style type="text/css" media="print">
        .Noprint
        {
            display: none;
        }
    </style>
    <!-- 两个样式表定义块必须分开，因为style标签的属性不同 -->
    <style type="text/css">
        .PageNext
        {
            page-break-after: always;
        }
         <!
        --控制分页-- > .style8
        {
            font-size: medium;
            color: #000066;
        }
        .style1
        {
            height: 19px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table width="800px" border="0" align="center" cellpadding="0" cellspacing="0" style="height: 30px;
        font-size: 14px; font-family: 宋体; word-break: break-all;" class="Noprint">
        <tr>
            <td align="left" valign="middle">
                <label for="textfield">
                </label>
                <input type="button" id="button_yl" value="打印预览" onclick="javascript: printpreview();" />
                <%-- <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="导出到Word" />--%>
            </td>
        </tr>
    </table>
    <div id="export" runat="server" style="width: 800px; margin-left: auto; margin-right: auto;">
        <!--startprint-->
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="word-break: break-all;">
            <tr>
                <td align="center" valign="middle" style="font-size: 14px; font-family: 宋体; font-weight: bold;
                    height: 30px">
                    商品买卖定标与保证函详情查看</td>
            </tr>
            <tr>
                <td align="right" valign="middle">
                    <table width="100%">
                        <tr>
                            <td align="right" width="14%" style="font-size: 9pt; font-family: 宋体;">
                                合同编号:
                            </td>
                            <td align="left" width="30%" style="font-size: 9pt; font-family: 宋体;">
                                <div runat="server" id="divDH" style="width: 100%" />
                            </td>
                            <td align="right" width="40%" style="font-size: 9pt; font-family: 宋体;">
                              
                            </td>
                            <td align="left" width="20%" style="font-size: 9pt; font-family: 宋体;">
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%" border="1" align="center" cellpadding="0" cellspacing="0" style="font-size: 12px;
            font-family: 宋体; word-break: break-all;">
            <tr>
                <td width="14%" align="right" valign="middle" bgcolor="#FFFFFF" style="height: 25px;">
                    经济批量：
                </td>
                <td width="35%" align="left" valign="middle" bgcolor="#FFFFFF">
                    <div runat="server" id="divKHMC" />
                </td>
                <td width="16%" align="right" valign="middle" bgcolor="#FFFFFF">
                    日均最高发货量：
                </td>
                <td width="35%" align="left" valign="middle" bgcolor="#FFFFFF">
                    <div runat="server" id="divKHBH" />
                </td>
            </tr>
            <tr>
                <td width="14%" align="right" valign="middle" bgcolor="#FFFFFF" style="height: 25px">
                    定标数量：
                </td>
                <td width="35%" align="left" valign="middle" bgcolor="#FFFFFF">
                    <div runat="server" id="divLXDH" />
                </td>
                <td width="16%" align="right" valign="middle" bgcolor="#FFFFFF">
                    卖家名称：
                </td>
                <td width="35%" align="left" valign="middle" bgcolor="#FFFFFF">
                    <div runat="server" id="divCZHM" />
                </td>
            </tr>
            <tr>
                <td width="14%" align="right" valign="middle" bgcolor="#FFFFFF">
                    联系方式：
                </td>
                <td width="35%" align="left" valign="middle" bgcolor="#FFFFFF">
                    <div runat="server" id="divSHDZ" />
                </td>
                <td width="16%" align="right" valign="middle" bgcolor="#FFFFFF" style="height: 25px">
                    联系人：
                </td>
                <td width="35%" align="left" valign="middle" bgcolor="#FFFFFF">
                    <div runat="server" id="divSHRXM" />
                </td>
            </tr>
            <tr>
                <td width="14%" align="right" valign="middle" bgcolor="#FFFFFF">
                    买家名称：
                </td>
                <td width="35%" align="left" valign="middle" bgcolor="#FFFFFF">
                    <div runat="server" id="divSHRSJ" />
                </td>
                <td width="16%" align="right" valign="middle" bgcolor="#FFFFFF" style="height: 25px">
                    联系方式：
                </td>
                <td width="35%" align="left" valign="middle" bgcolor="#FFFFFF">
                    <div runat="server" id="divDZYX" />
                </td>
            </tr>
             <tr>
                <td width="14%" align="right" valign="middle" bgcolor="#FFFFFF" style ="height:25px" >
                    联系人：
                </td>
                <td align="left" valign="middle" bgcolor="#FFFFFF" colspan="3">
                    <div runat="server" id="divWLFS" />
                </td>  
                            
            </tr>
            



       
        </table>
        <!--endprint-->
    </div>
    </form>
    <object classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2" height="0" id="wb" name="wb"
        width="0">
    </object>
</body>
</html>

