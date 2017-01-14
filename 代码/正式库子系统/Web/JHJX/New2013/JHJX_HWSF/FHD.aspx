<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FHD.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_HWSF_FHD" %>

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
                    <div style="float:left;  width:100%; height:30px;">
                        <span style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; ">富美集团中国商品批发交易平台</span>
                        <span style=" float:right; padding-right:5px; line-height:30px; vertical-align:middle;" id="spanDH" runat="server">单号：****</span>
                    </div>
                    <div style="float:left;  width:100%; text-align:center;">
                        <div style=" margin:auto auto; position:relative; line-height:30px; vertical-align:middle; font-size:14px; font-weight:bold; width:100px;">发货单</div>
                        <div style=" float:right; position:relative;  padding-right:5px; line-height:30px; vertical-align:middle; width:120px;" id="spanFHDTime" runat="server">****年**月**日</div>
                    </div>
                    <hr  style="width:100%;" />
                    <div style=" width:100%; vertical-align:middle;">
                        <div style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; width:100%; text-align:left; " id="spanBuyMC" runat="server">(买家)：</div>
                        <div style=" width:100%; vertical-align:middle;">
                        <span style=" float:left; position:relative; padding-left:25px; line-height:30px; vertical-align:middle;  width:auto; " id="spanDZGHHT" runat="server">根据***《电子购货合同》，及贵方***提货单，我方已按要求将货物发出。贵方验收时以此发货单为准。</span>
                            </div>
                    </div>
                    <br />
                    <div style=" width:100%; vertical-align:middle;">
                        <table style=" width:100%;" cellpadding="0" cellspacing="0">
                            <tr style=" height:30px;">
                                <td style=" width:75px; padding-left:5px;">发货单编号：</td>
                                <td style=" width:115px;" id="tdFHDH" runat="server">****</td>
                                <td style=" width:110px;">此前累计提货次数：</td>
                                <td style=" width:70px;" id="tdCQLJTHCS" runat="server">****</td>
                                <td style=" width:85px;">本次发货数量：</td>
                                <td style=" width:60px;" id="tdBCFHSL" runat="server">&nbsp;</td>
                                <td style=" width:110px;">此前累计发货数量：</td>
                                <td style=" width:75px;" id="tdCQLJFHSL" runat="server">&nbsp;</td>
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
                                                    发货数量</th>
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
                                                <td>
                                                   <div style=" word-wrap:break-word; line-height:18px;  width:150px;"><%=ViewState["商品名称"].ToString() %> 
                                                       
                                                       </div>
                                                       <%--<span id="spanSPMC" runat="server"></span>--%>
                                                </td>
                                                <td>
                                                   <div style=" word-wrap:break-word; line-height:18px;  width:150px;"><%=ViewState["规格"].ToString()%> 
                                                       
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
                                <td style=" width:90px; padding-left:5px;">收货人姓名：</td>
                                <td style=" width:280px; word-wrap:break-word; line-height:18px;" id="tdSHRXM" runat="server">****</td>
                                <td style=" width:110px;">收货人联系电话：</td>
                                <td style=" width:220px; word-wrap:break-word; line-height:18px;" id="tdSHRLXDH" runat="server">****</td>
                            </tr>
                            <tr style=" height:30px;" id="KHYH" runat="server">
                                <td style=" width:90px; padding-left:5px;">收货地址：</td>
                                <td style=" width:600px; word-wrap:break-word; line-height:18px;" colspan="3" id="tdSHDZ" runat="server">****</td>
                            </tr>
                            <tr style=" height:30px;" id="DWDZ" runat="server">
                                <td style=" width:90px; padding-left:5px;">发货人姓名：</td>
                                <td style=" width:280px; word-wrap:break-word; line-height:18px;" id="tdFHRXM" runat="server">****</td>
                                <td style=" width:110px;">发货人联系电话：</td>
                                <td style=" width:220px; word-wrap:break-word; line-height:18px;" id="tdFHRLXDH" runat="server">****</td>
                            </tr>
                            <tr style=" height:30px;">
                                <td style=" width:90px; padding-left:5px;">发票编号：</td>
                                <td style=" width:280px; word-wrap:break-word; line-height:18px;" id="tdFPBH" runat="server">****</td>
                                <td style=" width:110px;">发票是否随货同行：</td>
                                <td style=" width:220px; word-wrap:break-word; line-height:18px;" id="tdFPSFSHTX" runat="server">****</td>
                            </tr>
                            <tr style=" height:30px;">
                                <td style=" width:90px; padding-left:5px;">运输特殊要求：</td>
                                <td style=" width:600px; word-wrap:break-word; line-height:18px;" colspan="3" id="tdYSTSYQ" runat="server">****</td>
                            </tr>
                            <tr style=" height:30px;">
                                <td style=" width:90px; padding-left:5px;">补发备注：</td>
                                <td style=" width:600px; word-wrap:break-word; line-height:18px;" colspan="3" id="tdBFBZ" runat="server">****</td>
                            </tr>
                        </table>
                    </div> 
                    <hr  style="width:100%;"/>
                    <div style=" width:100%; vertical-align:middle; height:225px;">
                        <div style=" float:left; border:1px solid black; height:220px; width:200px; margin-left:5px;">
                            <div style="width:90%; margin-left:10px; margin-top:10px;">发货栏</div>
                            <div style="width:90%; margin-left:10px; margin-top:40px;">发货人签字：</div>
                            <div style="width:90%; margin-left:10px; margin-top:30px;">承运人签字：</div>
                            <div style="width:90%; margin-left:10px; margin-top:30px;">提货车牌号：</div>
                        </div>
                        <div style=" float:right; border:1px solid black; height:220px; width:380px; margin-right:5px;">
                            <div style="width:90%; margin-left:10px; margin-top:10px;">买家签收栏</div>
                            <div style="width:90%; margin-left:10px; margin-top:80px;">
                                <span style="margin-left:5px;">收货人签字：</span>
                                <span style="margin-left:100px;">身份证号：</span>
                            </div>
                            <div style="width:100%; margin-right:5px; margin-top:30px; text-align:right;">
                                <span style=" margin-right:50px; width:auto;">年</span>
                                <span style=" margin-right:50px; width:auto;">月</span>
                                <span style=" margin-right:30px; width:auto;">日</span>
                            </div>
                            <div style="width:100%; margin-right:5px; margin-top:20px; text-align:right;">
                                <span style=" margin-right:110px;">单位签章：</span>

                            </div>
                        </div>

                    </div>

                    <div style=" width:100%; vertical-align:middle;">
                        <span style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; width:100%;">备注：</span>
                        <span style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; width:100%;" id="spanBZ1" runat="server">1、本发货单作为***《电子购货合同》的附件，与***《电子购货合同》具有同等法律效力。</span>
                        <span style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; width:100%;" id="spanBZ2" runat="server">2、本次发货由***自主安排物流并承担相关费用及责任，运费已包含运险费。</span>
                        <span style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; width:100%;">3、此单商品物流方须按收货地址送货上门，并不再向收货方收取任何费用。</span>
                        <span style=" float:left; padding-left:5px; line-height:30px; vertical-align:middle; width:100%;">4、本次发货未夹带任何违法、禁运物品。</span>
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
