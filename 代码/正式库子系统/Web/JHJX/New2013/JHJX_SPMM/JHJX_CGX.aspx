<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_CGX.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_SPMM_JHJX_CGX" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<%@ Register src="../UCFWJG/UCFWJGDetail.ascx" tagname="UCFWJGDetail" tagprefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(document).ready(function () {
             $("#theObjTable").tablechangecolor();
         });
    </script>
</head>
<body onload="ShowUpdating()" style=" background-color:#f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
        Skin="Default2006" Width="99%">
        <Tabs>
             <radTS:Tab ID="Tab2" runat="server" NavigateUrl="jhjx_spmmgk.aspx" 
                Text=" 商品买卖概况">
            </radTS:Tab>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="jhjx_spmmjb.aspx" 
                Text="竞标中">
            </radTS:Tab>
            <radTS:Tab ID="Tab3" runat="server" NavigateUrl="jhjx_spmmljq.aspx" 
                Text="冷静期">
            </radTS:Tab>
            <radTS:Tab ID="Tab4" runat="server" NavigateUrl="jhjx_spmmzb.aspx" 
                Text="中标">
            </radTS:Tab>
            <radTS:Tab ID="Tab6" runat="server" NavigateUrl="jhjx_spmmdbybzh.aspx" 
                Text="定标与保证函">
            </radTS:Tab>
            <radTS:Tab ID="Tab5" runat="server" NavigateUrl="JHJX_FB.aspx"  Text="废标"
                 Font-Size="12px">
            </radTS:Tab>
            <radTS:Tab ID="Tab7" runat="server" NavigateUrl="JHJX_QP.aspx"  Text="清盘"  
                 Font-Size="12px">
            </radTS:Tab>
             <radTS:Tab ID="Tab8" runat="server" NavigateUrl="JHJX_CGX.aspx"  Text="草稿箱" ForeColor="Red"
                 Font-Size="12px">
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
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        <tr>
                            <td width="110px" align="right">
                                单据类别：&nbsp;
                            </td>
                            <td width="150px">
                                <asp:DropDownList ID="ddldjlb" runat="server" Height="25px" Width="150px" CssClass="tj_input">
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem Value="预订单">预订单</asp:ListItem>
                                    <asp:ListItem Value="投标单">投标单</asp:ListItem>
                                </asp:DropDownList>                           
                            </td>                            
                            <td align="right" width="90px">
                                交易方名称：                                
                            </td>
                            <td width="150px">
                            <asp:TextBox ID="txtjyfmc" runat="server" Width="150px" CssClass="tj_input" ></asp:TextBox>  
                            </td>
                            <td align="right" width="80px">
                                交易方账户：
                            </td>
                            <td  width="150px">
                            <asp:TextBox ID="txtjyfzh" runat="server" Width="150px" CssClass="tj_input"></asp:TextBox>                        
                            </td> 
                             <td  style="width:100px;text-align:center">
                                 <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="查询" Width="70px" OnClientClick="Click();" OnClick="BtnCheck_Click" />  
                                 
                             </td>  
                            <td  align="left">              
                          
                                <asp:Button ID="btnToExcel" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="导出" Width="70px" OnClick="btnToExcel_Click"/>
                           
                             <asp:Label ID="Label1" runat="server" Text="请稍后..." Width="50px" style="height: 15px"></asp:Label>
                               
                            </td>
                          
                           
                          
                        </tr>
                        <tr>
                            <td  align="right" width="110px">
                                商品名称：
                            </td>
                            <td width="150px">
                            <asp:TextBox ID="txtSPMC" runat="server" Width="150px" CssClass="tj_input"></asp:TextBox>  
                            </td>
                          
                            <td align="left" colspan="7">
                                <span style="width:10px"></span>
                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                            </td>
                            
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                <%--说明文字--%>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>   
                                <div class="content_nr_lb" style="width:1110px; ">
                                     <table id="theObjTable"  style="width:1390px; " cellspacing="0"  cellpadding="0">
                                        <thead>
                                            <tr>
                                                   <th class="TheadTh" style=" width:200px; line-height:18px;">
                                                    交易方名称</th>
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    所属管理部门</th>                                               
                                                 <th class="TheadTh" style=" width:100px;">
                                                    交易方账号</th>   
                                               
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                  单据类型
                                                </th>
                                                <th class="TheadTh" style=" width:100px;">
                                                  商品编号</th>                                                 
                                                <th class="TheadTh" style=" width:200px;">
                                                    商品名称</th>                                              
                                                
                                                <th class="TheadTh" style=" width:200px; line-height:18px;">
                                                    规格 
                                                </th>
                                                <th class="TheadTh" style=" width:90px; line-height:18px;">
                                                    数量</th>                                               
                                                <th class="TheadTh" style=" width:90px; line-height:18px;">
                                                    价格
                                                </th>
                                                <th class="TheadTh" style=" width:90px; line-height:18px;">
                                                    金额</th>                                             
                                               
                                                <th class="TheadTh" style=" width:120px; line-height:18px;">创建时间</th>
                                                                    
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" 
                                                >
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                         <td  style="width:200px; text-align:center">
                                                            <div style=" word-wrap:break-word; line-height:18px;  "> <%#Eval("交易方名称")%>&nbsp;</div>
                                                        </td>
                                                        <td   style="width:100px; text-align:center">
                                                            <div style=" word-wrap:break-word; line-height:18px; "> <%#Eval("所属管理部门")%> &nbsp;</div>
                                                        </td>
                                                        
                                                         <td  >
                                                            <div style=" word-wrap:break-word; line-height:18px; "> <%#Eval("交易方账号")%>&nbsp;</div>
                                                        </td>
                                                        
                                                        <td >
                                                   <div style=" word-wrap:break-word; line-height:18px; ">   <%#Eval("单据类型")%> </div>                                              
                                                        </td>    
                                                        <td >
                                                   <div style=" word-wrap:break-word; line-height:18px; ">   <%#Eval("商品编号")%> </div>                                              
                                                        </td> 
                                                        <td  style="width:200px; text-align:center">
                                                   <div style=" word-wrap:break-word; line-height:18px; ">   <%#Eval("商品名称")%> </div>                                              
                                                        </td> 
                                                        <td style="width:200px; text-align:center">
                                                   <div style=" word-wrap:break-word; line-height:18px; ">   <%#Eval("规格")%> </div>                                              
                                                        </td> 
                                                       
                                                        <td >
                                                            <div style=" word-wrap:break-word; line-height:18px; "> <%#Eval("数量")%></div>
                                                        </td>
                                                        <td >
                                                           <div style=" word-wrap:break-word; line-height:18px; "> <%#Eval("价格")%></div>
                                                        </td>
                                                        <td >
                                                             <div style=" word-wrap:break-word; line-height:18px; "> <%#Eval("金额")%></div>
                                                        </td>                                                        
                                                        <td>
                                                            <div style=" word-wrap:break-word; line-height:18px; "> <%#Eval("创建时间")%></div>

                                                        </td>
                                                                                         
                                                      
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr id="ts" runat="server" class="TfootTr">
                                                <td colspan="11" align="center">
                                                当前数据为空！
                                                </td>
                                            </tr> 
                                        </tfoot>
                                    </table>
                                </div>                            
                                   
                            
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
<script lang="ja" type="text/javascript">
    function ShowUpdating() {
        document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'block';
        document.getElementById("<%=btnToExcel.ClientID %>").style.display = 'block';
        document.getElementById("Label1").style.display = 'none';
    }
    function Click() {
        document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'none';
        document.getElementById("<%=btnToExcel.ClientID %>").style.display = 'none';
        document.getElementById("Label1").style.display = 'block';
    }
</script>
