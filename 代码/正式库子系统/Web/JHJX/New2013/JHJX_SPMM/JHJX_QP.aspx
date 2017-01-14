<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_QP.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_SPMM_JHJX_QP" %>

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
     <script lang="javascript" type="text/javascript">
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
            <radTS:Tab ID="Tab7" runat="server" NavigateUrl="JHJX_QP.aspx"  Text="清盘"  ForeColor="Red"
                 Font-Size="12px">
            </radTS:Tab>
             <radTS:Tab ID="Tab8" runat="server" NavigateUrl="JHJX_CGX.aspx"  Text="草稿箱" 
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
                                买家名称：</td>
                            <td width="150px">
                            <asp:TextBox ID="txtBuyMC" runat="server" Width="150px" CssClass="tj_input"></asp:TextBox>  
                            </td>                            
                            <td align="right" width="70px">
                                &nbsp;卖家名称：</td>
                            <td width="150px">
                            <asp:TextBox ID="txtSebMC" runat="server" Width="150px" CssClass="tj_input"></asp:TextBox>                        
                            </td>
                            <td align="right" width="120px">
                                &nbsp;电子购货合同编号：</td>
                            <td  width="150px">
                                 <asp:TextBox ID="txtDZGHHTBH" runat="server" Width="150px" CssClass="tj_input"></asp:TextBox>                           
                            </td>   
                            <td width="120px" align="right">
                                &nbsp;</td>
                            <td width="150px">
                                 &nbsp;</td>
                           
                        </tr>
                        <tr>
                            <td  align="right" width="110px">
                                &nbsp;清盘类型：</td>
                            <td width="150px">
                                <asp:DropDownList ID="ddlqplx" runat="server" Height="25px" Width="150px" CssClass="tj_input">
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem Value="人工清盘">人工清盘</asp:ListItem>
                                    <asp:ListItem Value="自动清盘">自动清盘</asp:ListItem>
                                </asp:DropDownList>                           
                            </td>
                            <td width="70px" align="right">
                                &nbsp;清盘状态：</td>
                            <td width="150px">
                                <asp:DropDownList ID="ddlqpzt" runat="server" Height="25px" Width="150px" CssClass="tj_input" >
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem Value="清盘中">清盘中</asp:ListItem>
                                    <asp:ListItem Value="清盘结束">清盘结束</asp:ListItem>
                                </asp:DropDownList>                           
                            </td>   
                            <td  align="left" colspan="4">
                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                            </td>
                            
                        </tr>
                        <tr>
                            <td  style="padding-left:20px" colspan="6" >
                                <uc2:UCFWJGDetail ID="UCFWJGDetail2" runat="server" />
                            </td>
                            <td>
                                <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="查询" Width="70px" OnClientClick="Click();" OnClick="BtnCheck_Click" />
                                <asp:Label ID="Label1" runat="server" Text="查询中，请稍后..." Width="120px"></asp:Label>
                                </td>
                            <td>
                            <asp:Button ID="btnToExcel" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                    Text="导出" Width="70px" OnClick="btnToExcel_Click"/>
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
                                    <table id="theObjTable"  style="width:1840px; " cellspacing="0"  cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    清盘开始时间
                                                </th>
                                                <th class="TheadTh" style=" width:200px; text-align:center">
                                                    清盘原因</th>
                                                  <th class="TheadTh" style=" width:80px;">
                                                    清盘类型</th>         
                                                <th class="TheadTh" style=" width:80px;">
                                                    清盘状态</th>                                              
                                                <th class="TheadTh" style=" width:100px;">
                                                    清盘结束时间 
                                                </th>  
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    电子购货</br>合同编号
                                                </th>
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    订金解</br>冻金额</th>                                               
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    履约保证金</br>解冻金额
                                                </th>                                               
                                                <th class="TheadTh" style=" width:200px; line-height:18px;">
                                                    买家名称</th>
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    买家账号</th>
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    买家所属</br>管理部门</th>                                               
                                                 <th class="TheadTh" style=" width:200px;">
                                                    卖家名称</th> 
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    卖家账号</th>  
                                                <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    卖家所属</br>管理部门</th>
                                                  <th class="TheadTh"  style=" width:80px; line-height:18px;">
                                                    清盘确认</th>                                                  
                                                 <th class="TheadTh" style=" width:100px; line-height:18px;">
                                                    清盘详情</th>
                                                                                               
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" 
                                                OnItemDataBound="Repeater1_ItemDataBound" >
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td >
                                                   <div style=" word-wrap:break-word; line-height:18px; ">   <%#Eval("清盘开始时间")%> </div>                                              
                                                        </td>
                                                        <td  title='<%#Eval("清盘原因")%>' style=" width:200px;">
                                                            <div style=" word-wrap:break-word; line-height:18px;  "> <%#Eval("清盘原因").ToString().Length > 100 ? Eval("清盘原因").ToString().Substring(0, 100) + "..." : Eval("清盘原因").ToString()%></div>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbqplx" runat="server" Text='<%#Eval("清盘类型")%>'></asp:Label>
                                                          
                                                        </td>
                                                        <td >
                                                            <div style=" word-wrap:break-word; line-height:18px;"  > <%#Eval("清盘状态")%></div>
                                                        </td>   
                                                         <td >
                                                            <div style=" word-wrap:break-word; line-height:18px; " > <%#Eval("清盘结束时间")%></div>
                                                        </td>  
                                                        <td >
                                                               <asp:LinkButton ID="btnDYDZGHHT" runat="server" CommandName='<%#Eval("Number") %>' CommandArgument="DZGHHT"><%#Eval("电子购货合同编号")%></asp:LinkButton>
                                                        </td>
                                                        <td >
                                                           <div style=" word-wrap:break-word; line-height:18px; " > <%#Eval("订金解冻金额")%></div>
                                                        </td>
                                                        <td >
                                                             <div style=" word-wrap:break-word; line-height:18px;"  > <%#Eval("履约保证金解冻金额")%></div>
                                                        </td>                                                       
                                                      
                                                        <td style=" width:200px;">
                                                            <div style=" word-wrap:break-word; line-height:18px; " > <%#Eval("买家名称")%>&nbsp;</div>
                                                        </td>
                                                        <td  >
                                                            <div style=" word-wrap:break-word; line-height:18px;"  > <%#Eval("买家账号")%>&nbsp;</div>
                                                        </td>
                                                        <td  >
                                                            <div style=" word-wrap:break-word; line-height:18px;"  > <%#Eval("买家所属分公司")%>&nbsp;</div>
                                                        </td>                                                        
                                                         <td  style=" width:200px;">
                                                            <div style=" word-wrap:break-word; line-height:18px; " > <%#Eval("卖家名称")%>&nbsp;</div>
                                                        </td>
                                                        <td  >
                                                            <div style=" word-wrap:break-word; line-height:18px; " > <%#Eval("卖家账号")%>&nbsp;</div>
                                                        </td>
                                                        <td   >
                                                            <div style=" word-wrap:break-word; line-height:18px; " > <%#Eval("卖家所属分公司")%>&nbsp;</div>
                                                        </td>   
                                                        <td    >
                                                             <asp:Label ID="lbsfqr" runat="server" Text='<%#Eval("是否确认")%>' ></asp:Label>
                                                        </td>   
                                                           
                                                       <td >
                                                            <asp:LinkButton ID="linlxq" runat="server" CommandName='<%#Eval("Number") %>' CommandArgument="XQ" >查看详情</asp:LinkButton> 
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr id="ts" runat="server" class="TfootTr">
                                                <td colspan="22" align="center">
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
