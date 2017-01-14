<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_MJTBZLSH_WSH.aspx.cs" Inherits="Web_JHJX_New2013_TBZLSH_JHJX_MJTBZLSH_WSH" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>

<%@ Register src="../UCFWJG/UCFWJGDetail.ascx" tagname="UCFWJGDetail" tagprefix="uc2" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台审核代理人功能</title>
    <script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
    <%-- <style type="text/css">
        #content_zw
        {
            width: 954px;
        }
    </style>--%>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" ForeColor="Red" NavigateUrl="JHJX_MJTBZLSH_WSH.aspx" 
                Text="未审核">
            </radTS:Tab>
                 <radTS:Tab ID="Tab2" runat="server" NavigateUrl="JHJX_MJTBZLSH_YSH.aspx"
                Text="已审核"> </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <%-- <div class="content_bz" >
                    说明文字：<br />
                    1、该模块用于记录分公司和总部之间的补货信息。<br />
                    2、销货单号以FZ开头的是分公司质量退换补货单，以FT开头的是分公司与总部调换货业务补货单。<br />
                </div>--%>
                <div class="content_nr">
                      <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                         <tr>
                              <td align="right" width="80px; ">
                                投标时间：
                             <td colspan="9">
                                 <table  style="border:0">
                                     <tr>
                                          
                            </td>
                            <td  style="width:130px">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtTBStart" runat="server" Width="138px"></asp:TextBox>
                            </td>
                            <td align="right" width="40px">
                                至：
                            </td>
                            <td  style="width: 130px">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtTBEnd" runat="server" Width="130px"></asp:TextBox>
                            </td>
                             <td  colspan="6"  style="padding-left:10px">
                             
                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                             
                            </td>
                                     </tr>

                                 </table>

                             </td>
                          
                           
                        </tr>
                       
                           <tr>
                            <td  align="right" width="80px">
                               合同期限：
                            </td>
                            <td  style="width:80px">
                              <asp:DropDownList ID="ddHTQX" runat="server" Height="25px" Width="138px" CssClass="tj_input">
                                         <asp:ListItem Text="全部" Value="">全部</asp:ListItem>
                                      <asp:ListItem Text="三个月" Value="三个月">三个月</asp:ListItem>
                                        <asp:ListItem Text="一年" Value="一年">一年</asp:ListItem>
                                          <asp:ListItem Text="即时" Value="即时">即时</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="70px" align="right">
                                卖方账号：</td>
                            <td style="width: 130px">
                                <asp:TextBox CssClass="tj_input"
                                    ID="txtSelzh" runat="server" Width="130px"></asp:TextBox>
                               </td>
                             <td  style="padding-left:10px;width:65px">
                                卖方名称：</td>
                            <td  style="text-align:left;width:130px">
                                <asp:TextBox CssClass="tj_input"
                                    ID="txtBuyerMC" runat="server" Width="130px"></asp:TextBox>
                            </td>
                            <td style=" text-align:right ;width:80px">
                                投标单号：</td>
                            <td  style="text-align:left;width:130px">
                                 <asp:TextBox CssClass="tj_input"
                                    ID="txtTBDBH" runat="server" Width="130px"></asp:TextBox>
                            </td>
                          
                            <td  valign="middle" style="width:130px;" >
                                 <table  width="100%" cellpadding="0" cellspacing="0">
                                    <tr><td align="right"> 
                               <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" 
                                    Text="查询"  Width="50px" OnClick="btnSearch_Click" /> </td>
                                    <td  style="padding-left:10px">
                             <asp:Button ID="btnToExcel" runat="server" CssClass="tj_bt" 
                                    Text="导出"  Width="50px" OnClick="btnToExcel_Click"/> </td>
                                </table></td>
                               <td></td>
                        </tr>                        
                       
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                              未审核投标单资料
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                                    <div id="exprot" runat="server" style=" width:1110px;" class="content_nr_lb">
                                <table id="theObjTable" style="width: 100%;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                             <th class="TheadTh" style=" width:100px;word-wrap:break-word; ">
                                                操作
                                            </th>
                                            <th class="TheadTh"  style=" width:110px;word-wrap:break-word; ">
                                               投标单号
                                            </th>
                                            <th class="TheadTh" style=" width:140px;word-wrap:break-word; ">
                                                卖方账号
                                            </th>
                                            <th class="TheadTh"style=" width:160px;word-wrap:break-word; ">
                                                卖方名称
                                            </th>
                                            <th class="TheadTh" style=" width:140px;word-wrap:break-word; ">
                                                投标时间
                                            </th>
                                            <th class="TheadTh" style=" width:100px;word-wrap:break-word; ">
                                                投标单状态
                                            </th>
                                            <th class="TheadTh" style=" width:120px;word-wrap:break-word; ">
                                                商品名称
                                            </th>
                                            <th class="TheadTh" style=" width:80px;word-wrap:break-word; ">
                                                合同期限
                                            </th>
                                              <th class="TheadTh" style=" width:90px;word-wrap:break-word; ">
                                                规格
                                            </th>
                                              <th class="TheadTh" style=" width:90px;word-wrap:break-word; ">
                                                投标拟售量
                                            </th>
                                              <th class="TheadTh" style=" width:70px;word-wrap:break-word; ">
                                                投标价格
                                            </th>
                                               <th class="TheadTh" style=" width:130px;word-wrap:break-word; ">
                                                平台管理机构
                                            </th>
                                             
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptZJYEBDMX" runat="server" OnItemCommand="rptZJYEBDMX_ItemCommand" >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                       <td  style=" width:90px;word-wrap:break-word; ">
                                                       <asp:LinkButton ID="linkCKXQ" runat="server" Width="90px" CommandName="linkCKXQ" CommandArgument='<%#Eval("Number")%>'>查看资料详情</asp:LinkButton>
                                                       
                                                    </td>
                                                    <td style=" width:110px;word-wrap:break-word; ">
                                                      <div style=" width:110px;word-wrap:break-word;">  <%#Eval("投标单编号")%></div>
                                                    </td>
                                                    <td style=" width:140px;word-wrap:break-word; ">
                                                      
                                                           <div style=" width:140px;word-wrap:break-word;">  <%#Eval("卖方账号")%></div>
                                                    </td>
                                                    <td style=" width:160px;word-wrap:break-word; ">
                                                 
                                                          <div style=" width:160px;word-wrap:break-word;">  <%#Eval("卖方名称")%></div>
                                                    </td>
                                                    <td style=" width:140px;word-wrap:break-word; ">
                                                  
                                                          <div style=" width:140px;word-wrap:break-word;">  <%#Eval("投标时间")%></div>
                                                    </td>
                                                    <td style=" width:100px;word-wrap:break-word; ">
                                                          <div style=" width:100px;word-wrap:break-word; line-height:18px;">  <%#Eval("投标单状态")%></div>
                                                    </td>
                                                    <td style=" width:120px;word-wrap:break-word; ">
                                                
                                                            <div style=" width:120px;word-wrap:break-word;">  <%#Eval("商品名称")%></div>
                                                    </td>
                                                    <td  style=" width:80px;word-wrap:break-word; ">
                                                   
                                                           <div style=" width:80px;word-wrap:break-word;">  <%#Eval("合同期限")%></div>
                                                    </td>
                                                     <td  style=" width:90px;word-wrap:break-word; ">
                                                   
                                                           <div style=" width:90px;word-wrap:break-word;">  <%#Eval("规格")%></div>
                                                    </td>
                                                        <td  style=" width:90px;word-wrap:break-word; ">
                                                   
                                                           <div style=" width:90px;word-wrap:break-word;">  <%#Eval("投标拟售量")%></div>
                                                    </td>
                                                       <td  style=" width:70px;word-wrap:break-word; ">
                                                   
                                                           <div style=" width:70px;word-wrap:break-word;">  <%#Eval("投标价格")%></div>
                                                    </td>
                                                       <td  style=" width:130px;word-wrap:break-word; ">
                                                   
                                                           <div style=" width:130px;word-wrap:break-word;">  <%#Eval("平台管理机构")%></div>
                                                    </td>
                                                    
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                            <td colspan="12">
                                                您查询的数据为空！
                                            </td>
                                        </tr>
                                    </tbody>
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
    <input runat="server" id="hidID" type="hidden" />
    </form>
</body>
</html>
