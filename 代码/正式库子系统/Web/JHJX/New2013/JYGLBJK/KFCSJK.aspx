<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KFCSJK.aspx.cs" Inherits="Web_JHJX_New2013_JYGLBJK_KFCSJK" %>

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
    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
       <script type="text/javascript" language="javascript">
           //浮点数
           function CheckInputIntFloat(oInput) {
               if ('' != oInput.value.replace(/\d{1,}\.{0,1}\d{0,2}/, '')) {
                   oInput.value = oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/) == null ? '' : oInput.value.match(/\d{1,}\.{0,1}\d{0,2}/);
               }
           }
    </script>
</head>
<body style=" background-color:#f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="99%">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="KFCSJK.aspx" Text="扣罚次数检测表"
                ForeColor="Red" Font-Size="12px">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_bz" >
                    <%--说明文字--%>
                </div>
                <div class="content_nr">
                    <table width="980px" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        
                        <tr>
                            <td align="left">
                                &nbsp;&nbsp;&nbsp;
                                由
                            <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="txtBeginTime" runat="server" Width="178px"></asp:TextBox>                            
                                至
                            <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="txtEndTime" runat="server" Width="178px"></asp:TextBox>                            
                                期间实际扣罚履约保证金或定金次数超过<asp:TextBox ID="txtcs" runat="server" Width="90px" CssClass="tj_input" Style="text-align: center; ime-mode: Disabled;"
                           onKeypress="return (/[\d]/.test(String.fromCharCode(event.keyCode)))"
                            onkeyup="javascript:CheckInputIntFloat(this);" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]\.?/g,''))"></asp:TextBox>  
                                次的账户：
                                <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="查询" onclick="BtnCheck_Click" Width="80px" /> &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnDC" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="导出" Width="80px" OnClick="btnDC_Click" />
                            </td>
                        </tr>
                        </table>
                    <table width="980px" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                <%--说明文字--%>
                            </td>
                        </tr>
                    </table>
                    <table width="980px" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                               <%-- <div id="exprot" runat="server" class="content_nr_lb">--%>
                                    <table id="theObjTable" style="width: 100%;"   cellspacing="0"  cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style=" width:170px;">
                                                    账户名称
                                                </th>
                                                <th class="TheadTh" style=" width:80px;">
                                                    账户类型
                                                </th>
                                                <th class="TheadTh" style=" width:80px;">
                                                    注册类别
                                                </th>                                              
                                                <th class="TheadTh" style=" width:170px;">
                                                    关联经纪人
                                                </th>  
                                                <th class="TheadTh" style=" width:80px;">
                                                    联系人
                                                </th>
                                                <th class="TheadTh" style=" width:80px;">
                                                    联系方式
                                                </th>                                               
                                                <th class="TheadTh" style=" width:80px;">
                                                    所属分公司
                                                </th>
                                                <th class="TheadTh" style=" width:80px; ">
                                                    罚款次数
                                                </th>
                                                <th class="TheadTh" style=" width:80px;">
                                                    罚款金额
                                                </th>                                                
                                                <th class="TheadTh" style=" width:80px;">
                                                    操作
                                                </th>                                                          
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server" 
                                                onitemcommand="Repeater1_ItemCommand"  >
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td  style=" width:150px;">
                                                            <%#Eval("账户名称")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("账户类型")%>
                                                        </td>
                                                        <td >
                                                           <%#Eval("注册类别")%>
                                                        </td>
                                                        <td   style=" width:150px;">
                                                           <%#Eval("关联经纪人")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("联系人")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("联系方式")%>
                                                        </td>
                                                        <td>
                                                           <%#Eval("所属分公司")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("罚款次数")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("金额")%>
                                                        </td>                                                      
                                                        <td>
                                                        
                                                           <asp:LinkButton ID="btnCK" runat="server" CommandArgument ='<%#Eval("邮箱") %>' CommandName="CK">查看详情</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr id="ts" runat="server" class="TfootTr">
                                                <td colspan="10" align="center">
                                                当前数据为空！
                                                </td>
                                            </tr> 
                                        </tfoot>
                                    </table>
                           <%--     </div>--%>
                                <uc1:commonpagernew ID="commonpagernew1" runat="server" />
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