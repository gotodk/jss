<%@ Page Language="C#" AutoEventWireup="true" CodeFile="THD.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_HWSF_THD" %>

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

    <style type="text/css">
        #content_zw
        {
            width:700px;
        
        }
    </style>
</head>
<body style=" background-color:#f7f7f7;">
    <form id="form1" runat="server">
     <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_bz" >
                    <%--说明文字--%>
                </div>
                <div class="content_nr">
                    <div style=" float:left; width:100%; height:30px;">
                        <span style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; ">富美集团中国商品批发交易平台</span>
                        <span style=" float:right; padding-right:5px; line-height:30px; vertical-align:middle;" id="spanDH" runat="server">单号：****</span>
                    </div>
                    <div style=" float:left;  width:100%; text-align:center;">
                        <div style=" margin:auto auto; position:relative; line-height:30px; vertical-align:middle; font-size:14px; font-weight:bold; width:100px;">提货单</div>
                        <div style=" float:right; position:relative; padding-right:5px; line-height:30px; vertical-align:middle; width:120px;" id="spanTHDTime" runat="server">****年**月**日</div>
                    </div>
                    <hr style="width:100%;" />
                    <div style=" width:100%; vertical-align:middle;">
                        <div style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; width:100%; text-align:left; " id="spanSelMC" runat="server">(卖家)：</div>
                        <div style=" width:100%; vertical-align:middle;">
                        <span style=" float:left; position:relative; padding-left:25px; line-height:30px; vertical-align:middle;  width:auto; " id="spanDZGHHT" runat="server">根据****《电子购货合同》，我方现下达提货单，请贵方于</span>
                        <span style=" float:left; position:relative; color:red; line-height:30px; vertical-align:top;  width:auto; height:30px; " id="spanZCFHR" runat="server">****年**月**日前（最迟发货日）</span>
                        <span style=" float:left; position:relative;  line-height:30px; vertical-align:middle;  width:auto; height:30px;">将货物发出。</span>
                            </div>
                    </div>
                    <br />
                    <div style=" width:100%; vertical-align:middle;">
                        <table style=" width:100%;" cellpadding="0" cellspacing="0">
                            <tr style=" height:30px;">
                                <td style=" width:85px; padding-left:5px;">提货单编号：</td>
                                <td style=" width:120px;" id="tdTHDH" runat="server">****</td>
                                <td style=" width:110px;">此前累计提货次数：</td>
                                <td style=" width:70px;" id="tdCQLJTHCS" runat="server">****</td>
                                <td style=" width:70px;">&nbsp;</td>
                                <td style=" width:70px;">&nbsp;</td>
                                <td style=" width:100px;">&nbsp;</td>
                                <td style=" width:65px;">&nbsp;</td>
                            </tr>
                            <tr style=" height:30px;">
                                <td style=" width:85px; padding-left:5px;">本次提货数量：</td>
                                <td style=" width:120px;" id="tdBCTHSL" runat="server">****</td>
                                <td style=" width:110px;">此前累计提货数量：</td>
                                <td style=" width:70px;" id="tdCQLJTHSL" runat="server">****</td>
                                <td style=" width:70px;">定标数量：</td>
                                <td style=" width:70px;" id="tdDBSL" runat="server">****</td>
                                <td style=" width:100px;">剩余可提货数量：</td>
                                <td style=" width:65px;" id="tdSYKTHSL" runat="server">****</td>
                            </tr>
                            <tr style=" height:30px;">
                                <td style=" width:85px; padding-left:5px;">本次提货金额：</td>
                                <td style=" width:120px;" id="tdBCTHJE" runat="server">****</td>
                                <td style=" width:110px;" >此前累计提货金额：</td>
                                <td style=" width:70px;" id="tdCQLJTHJE" runat="server">****</td>
                                <td style=" width:70px;" >定标金额：</td>
                                <td style=" width:70px;" id="tdDBJE" runat="server">****</td>
                                <td style=" width:100px;">剩余可提货金额：</td>
                                <td style=" width:65px;" id="tdSYKTHJE" runat="server">****</td>
                            </tr>
                        </table>
                    </div>                    
                    <br />
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                               <%--<div class="content_nr_lb" style="width:700px; ">--%>
                                    <table id="theObjTable"  style="min-width:700px; " cellspacing="0"  cellpadding="0">
                                        <thead>
                                            <tr>                                                                                         
                                                <th class="TheadTh" style=" width:80px;">
                                                    商品编号
                                                </th>  
                                                <th class="TheadTh" style=" width:150px;">
                                                    商品名称 
                                                </th>
                                                <th class="TheadTh" style=" width:150px;">
                                                    规格</th> 
                                                <th class="TheadTh" style=" width:60px;">
                                                    计价单位
                                                </th>
                                                <th class="TheadTh" style=" width:100px;">
                                                    提货数量</th>
                                                <th class="TheadTh" style=" width:70px;">
                                                    定标价格</th>                                                   
                                                <th class="TheadTh" style=" width:90px;">
                                                    金额
                                                </th>                                                                                                       
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr class="TbodyTr" id="tbodyTR" runat="server">
                                                <td>
                                                    <span id="spanSPBH" runat="server"></span>
                                                </td>
                                                <td title='<%=ViewState["商品名称"].ToString() %>'>
                                                   <div style=" word-wrap:break-word; line-height:18px;  width:120px;"><%=ViewState["商品名称"].ToString().Length > 10 ? ViewState["商品名称"].ToString().Substring(0, 8) + "..." : ViewState["商品名称"].ToString() %> 
                                                       
                                                       </div>
                                                       <%--<span id="spanSPMC" runat="server"></span>--%>
                                                </td>
                                                <td title='<%=ViewState["规格"].ToString() %>'>
                                                   <div style=" word-wrap:break-word; line-height:18px;  width:130px;"><%=ViewState["规格"].ToString().Length > 10 ? ViewState["规格"].ToString().Substring(0, 8) + "..." : ViewState["规格"].ToString() %> 
                                                       
                                                       </div>
                                                    <%--<span id="spanGG" runat="server"></span>--%>
                                                </td>
                                                <td>
                                                    <span id="spanJJDW" runat="server"></span>
                                                </td>
                                                <td>
                                                    <span id="spanTHSL" runat="server"></span>
                                                </td>
                                                <td>
                                                     <span id="spanDBJG" runat="server"></span>
                                                </td>
                                                <td>
                                                    <span id="spanJE" runat="server"></span>
                                                </td>
                                            </tr>
                                        </tbody>
                                        <tfoot>
                                            <tr id="ts" runat="server" class="TfootTr">
                                                <td colspan="7" align="center">
                                                当前数据为空！
                                                </td>
                                            </tr> 
                                        </tfoot>
                                    </table>
                                <%--</div>  --%>                              
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div style=" width:100%; vertical-align:middle;">
                        <table style=" width:100%;" cellpadding="0" cellspacing="0">
                            <tr style=" height:30px;">
                                <td style=" width:70px; padding-left:5px;">发票类型：</td>
                                <td style=" width:300px; word-wrap:break-word; line-height:18px;" id="tdFPLX" runat="server">****</td>
                                <td style=" width:85px;">发票抬头：</td>
                                <td style=" width:245px; word-wrap:break-word; line-height:18px;" id="tdFPTT" runat="server">****</td>
                            </tr>
                            <tr style=" height:30px;" id="SHAndYHZH" runat="server">
                                <td style=" width:70px; padding-left:5px;">发票税号：</td>
                                <td style=" width:300px; word-wrap:break-word; line-height:18px;" id="tdFPSH" runat="server">****</td>
                                <td style=" width:85px;">银行账号：</td>
                                <td style=" width:230px; word-wrap:break-word; line-height:18px;" id="tdYHZH" runat="server">****</td>
                            </tr>
                            <tr style=" height:30px;" id="KHYH" runat="server">
                                <td style=" width:70px; padding-left:5px;">开户银行：</td>
                                <td style=" width:300px; word-wrap:break-word; line-height:18px;" colspan="3" id="tdKHYH" runat="server">****</td>
                            </tr>
                            <tr style=" height:30px;" id="DWDZ" runat="server">
                                <td style=" width:70px; padding-left:5px;">单位地址：</td>
                                <td style=" width:620px; word-wrap:break-word; line-height:18px;" colspan="3" id="tdDWDZ" runat="server">****</td>
                            </tr>
                            <tr style=" height:30px;">
                                <td style=" width:70px; padding-left:5px;">收货人：</td>
                                <td style=" width:300px; word-wrap:break-word; line-height:18px;" id="tdSHR" runat="server">****</td>
                                <td style=" width:85px;">收货联系方式：</td>
                                <td style=" width:230px; word-wrap:break-word; line-height:18px;" id="tdSHLXFS" runat="server">****</td>
                            </tr>
                            <tr style=" height:30px;">
                                <td style=" width:70px; padding-left:5px;">收货地址：</td>
                                <td style=" width:620px; word-wrap:break-word; line-height:18px;" colspan="3" id="tdSHDZ" runat="server">****</td>
                            </tr>
                        </table>
                    </div> 
                    <hr style=" width:100%;" />
                    <div style=" width:100%; vertical-align:middle;">
                        <span style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; width:100%;">备注：</span>
                        <span style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; width:100%;" id="spanBZ" runat="server">1、本提货单作为***《电子购货合同》的附件，与***《电子购货合同》具有同等法律效力。</span>
                        <span style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; width:100%;" id="span3" runat="server">2、卖家承担物流费用，包含运险费。</span>
                    </div>
                    <div style=" width:100%; vertical-align:middle; text-align:center;">
                        <asp:Button ID="btnGoBack" runat="server" UseSubmitBehavior="False" runat="server" CssClass="tj_bt_da" Text="返回列表" OnClick="btnGoBack_Click" Height="30px" Width="80px" />
                    </div>
                    
                </div>
            </div>
        </div>
    </div>
        
    </form>
</body>
</html>
