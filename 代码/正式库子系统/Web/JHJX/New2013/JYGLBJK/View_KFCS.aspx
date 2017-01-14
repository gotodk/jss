<%@ Page Language="C#" AutoEventWireup="true" CodeFile="View_KFCS.aspx.cs" Inherits="Web_JHJX_New2013_JYGLBJK_View_KFCS" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 
  
</head>
<body style=" background-color:#f7f7f7;">
    <form id="form1" runat="server">    
    
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_bz" >
                    <%--说明文字--%>
                </div>
                <div class="BT" style="width:980px;text-align:center; vertical-align:middle;   font-size: 16px;  font-weight: bold;  height: 30px">扣罚次数详情</div>
                <div  style="  border:solid 1px #a5cbe2; height:0px;width:980px;"></div>
                
                <div class="content_nr">
                
                    <table width="980px" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                明细列表：</td>
                            <td style="text-align:right">
                               
                                <input type="button" onclick="window.history.go(-1);" value="返回上一页"/>
                            </td>
                        </tr>
                    </table>
                    <table width="980px" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                                <div id="exprot" runat="server" class="content_nr_lb" style="width:980px; ">
                                    <table id="theObjTable" style="width: 1420px;"   cellspacing="0"  cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style=" width:40px;">
                                                    序号
                                                </th>
                                                <th class="TheadTh" style=" width:120px;">
                                                    变动时间
                                                </th>
                                                <th class="TheadTh" style=" width:170px;">
                                                    交易方名称
                                                </th>
                                                <th class="TheadTh" style=" width:120px;">
                                                    交易方账号
                                                </th>
                                                <th class="TheadTh" style=" width:100px;">
                                                    分公司名称
                                                </th>                                              
                                                <th class="TheadTh" style=" width:500px;">
                                                    摘要
                                                </th>  
                                                <th class="TheadTh" style=" width:100px;">
                                                    项目
                                                </th>
                                                <th class="TheadTh" style=" width:190px;">
                                                    性质
                                                </th>                                               
                                                <th class="TheadTh" style=" width:80px;">
                                                    金额
                                                </th>                                                                                           
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server" 
                                                 >
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                         <td  >
                                                            <%#Eval("序号")%>
                                                        </td>
                                                        <td  style=" width:120px;">
                                                            <%#Eval("时间")%>
                                                        </td>
                                                        <td style=" width:170px;">
                                                            <%#Eval("交易方名称")%>
                                                        </td>
                                                        <td  style=" width:120px;">
                                                           <%#Eval("交易方账号")%>
                                                        </td>
                                                        <td   style=" width:100px;">
                                                           <%#Eval("分公司名称")%>
                                                        </td>
                                                        <td style=" width:500px;">
                                                            <%#Eval("摘要")%>
                                                        </td>
                                                        <td style=" width:100px;">
                                                            <%#Eval("项目")%>
                                                        </td>
                                                        <td style=" width:190px;">
                                                           <%#Eval("性质")%>
                                                        </td>                                                        
                                                        <td>
                                                            <%#Eval("金额")%>
                                                        </td>                                                      
                                                      
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr id="ts" runat="server" class="TfootTr">
                                                <td colspan="8" align="center">
                                                当前数据为空！
                                                </td>
                                            </tr> 
                                        </tfoot>
                                    </table>
                                </div>
                               <%-- <uc1:commonpagernew ID="commonpagernew1" runat="server" />--%>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    </form>
</body>
</html>
<script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>