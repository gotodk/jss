<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DECJJK.aspx.cs" Inherits="Web_JHJX_New2013_JYGLBJK_DECJJK" %>

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
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="DECJJK.aspx" Text="大额出金检测表"
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
                    <table width="900px" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        
                        <%--<tr>
                           
                            <td  align="right" width="70px">
                                时间：
                            </td>
                            <td width="178px">
                            <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="txtBeginTime" runat="server" Width="178px"></asp:TextBox>                            
                            </td>
                            <td width="20px" align="center">
                                至
                            </td>
                            <td width="178px">
                            <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                ID="txtEndTime" runat="server" Width="178px"></asp:TextBox>                            
                            </td>
                            <td></td>
                        </tr>--%>
                        <tr>
                            <td  colspan="3" >
                                &nbsp;&nbsp;&nbsp;
                                交易账户<asp:DropDownList ID="ddllb" runat="server" CssClass="tj_input" Width="122px">                                   
                                    <asp:ListItem Value="单次">单次</asp:ListItem>
                                    <asp:ListItem Value="一天">一天</asp:ListItem>
                                </asp:DropDownList>
                                累积出金超过<asp:TextBox ID="txtje" runat="server" Width="120px" CssClass="tj_input" Style="text-align: center; ime-mode: Disabled;"
                           onKeypress="return (/[\d.]/.test(String.fromCharCode(event.keyCode)))"
                            onkeyup="javascript:CheckInputIntFloat(this);" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d.]\.?/g,''))"></asp:TextBox>  
                                万元的数据如下：</td>
                            <td  align="left" colspan="6" style=" padding-left:20px; ">
                                <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="查询" onclick="BtnCheck_Click" Width="80px" /> &nbsp;&nbsp;
                                <asp:Button ID="btnDC" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="导出" Width="80px" OnClick="btnDC_Click" />
                            </td>
                            
                        </tr>
                    </table>
                    <table width="900px" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                <%--说明文字--%>
                            </td>
                        </tr>
                    </table>
                    <table width="900px" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                               <%-- <div id="exprot" runat="server" class="content_nr_lb">--%>
                                    <table id="theObjTable" style="width: 900px;"   cellspacing="0"  cellpadding="0">
                                        <thead>
                                            <tr>                                               
                                                <th class="TheadTh" style=" width:190px;text-align:center">
                                                    账户名称
                                                </th>
                                                <th class="TheadTh" style=" width:80px;text-align:center">
                                                    账户类型
                                                </th>                                              
                                                <th class="TheadTh" style=" width:80px;text-align:center">
                                                    注册类别
                                                </th>  
                                                <th class="TheadTh" style=" width:90px;text-align:center">
                                                   联系人
                                                </th>
                                                <th class="TheadTh" style=" width:90px;text-align:center">
                                                    联系方式
                                                </th>  
                                                 <th class="TheadTh" style=" width:100px;text-align:center">
                                                    所属分公司
                                                </th>                                               
                                                <th class="TheadTh" style=" width:110px;text-align:center">
                                                    时间
                                                </th>  
                                                 <th class="TheadTh" style=" width:70px;text-align:center">
                                                    出金次数
                                                </th>  
                                                 <th class="TheadTh" style=" width:90px;text-align:center">
                                                    出金金额
                                                </th>  
                                                                                     
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server" >
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">                                                      
                                                        <td  title='<%#Eval("账户名称")%>'>
                                                            <div style=" word-wrap:break-word; line-height:18px;  "> <%#Eval("账户名称").ToString().Length > 25 ? Eval("账户名称").ToString().Substring(0, 25) + "..." : Eval("账户名称").ToString()%></div>
                                                        </td>
                                                        <td>
                                                            <%#Eval("账户类型").ToString().TrimEnd(new char[]{ '交','易','账','户'})%>
                                                        </td>
                                                        <td>                                                   
                                                            <%#Eval("注册类别")%>
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
                                                            <%#Eval("时间") %>
                                                        </td>
                                                        <td>
                                                            <%#Eval("出金次数")%>
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
                                                <td colspan="9" align="center">
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
