<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_JYDJ.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_PTZHZLSH_JHJX_JYDJ" %>
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
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JYPT_WSH.aspx" 
                Text="未审核">
            </radTS:Tab>
                 <radTS:Tab ID="Tab2" runat="server"    NavigateUrl="JYPT_SHTG.aspx"
                Text="已审核通过"> </radTS:Tab>
                         <radTS:Tab ID="Tab3" runat="server" ForeColor="Red" NavigateUrl="JHJX_JYDJ.aspx" 
                Text="建议冻结"> </radTS:Tab>
                               
          
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
                           <td style=" width:10px;">&nbsp;</td>
                           
                             <td align="right" style="width: 110px">
                               分公司审核时间：
                            </td>
                            <td  style="width:90px;">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtFGSBeginTime" runat="server" Width="90px"></asp:TextBox>
                            </td>
                            <td align="center"  style="width:20px;">
                                至
                            </td>
                            <td style="width:90px;">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtFGSEndTime" runat="server" Width="90px"></asp:TextBox>
                            </td>
                                <td style="width:8px;">&nbsp;</td>
                             <td style=" width:65px;text-align:right">

                                      审核状态：</td>
                                     <td Width="120px" >

                                <asp:DropDownList ID="ddshzt" runat="server" Height="25px" Width="120px" CssClass="tj_input">
                                    <asp:ListItem Text="请选择" Value="">请选择</asp:ListItem>
                                      <asp:ListItem Text="尚未处理" Value="尚未处理">尚未处理</asp:ListItem>
                                        <asp:ListItem Text="同意冻结" Value="同意冻结">同意冻结</asp:ListItem>
                                     <asp:ListItem Text="不予冻结" Value="不予冻结">不予冻结</asp:ListItem>
                                </asp:DropDownList>

                                     </td>
                             <td colspan="6"  style="padding-left:10px">
                              
                                 
                              
                              <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                              
                                 
                              
                            </td>
                          
                         
                        </tr>
                        <tr>
                           <td style=" width:10px;">&nbsp;</td>
                           
                             <td width="110px" align="right">
                                服务中心审核时间：
                            </td>
                            <td  style="width:90px;">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtFWZXBeginTime" runat="server" Width="90px"></asp:TextBox>
                            </td>
                            <td align="center">
                                至
                            </td>
                            <td style="width:90px;">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtFWZXEndTime" runat="server" Width="90px"></asp:TextBox>
                            </td>
                            <td style="width:8px;">&nbsp;</td>
                             <td width="65px"  style="text-align:right">
                                账户类型：
                            </td>
                            <td style="width: 120px">
                                <asp:DropDownList ID="ddZHLB" runat="server" Height="25px" Width="120px" CssClass="tj_input">
                                    <asp:ListItem Text="请选择" Value="">请选择</asp:ListItem>
                                      <asp:ListItem Text="经纪人交易账户" Value="经纪人交易账户">经纪人交易账户</asp:ListItem>
                                        <asp:ListItem Text="买家卖家交易账户" Value="买家卖家交易账户">买家卖家交易账户</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                             <td align="right" Width="70px" >
                                注册类别：
                            </td>
                            <td align="left" Width="70px" >
                                <asp:DropDownList ID="ddZCLB" runat="server" Height="25px" Width="70px" CssClass="tj_input">
                                  <asp:ListItem Text="请选择" Value="">请选择</asp:ListItem>
                                      <asp:ListItem Text="单位" Value="单位">单位</asp:ListItem>
                                        <asp:ListItem Text="自然人" Value="自然人">自然人</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                               <td align="right" Width="80px" >
                                交易方账号：
                            </td>
                            <td align="left"  Width="90px">
                                <asp:TextBox ID="txtJYFZH" runat="server" Width="90px" CssClass="tj_input"></asp:TextBox>
                            </td>
                               <td align="right" Width="80px" >
                                交易方名称：
                            </td>
                            <td align="left"  Width="90px">
                                <asp:TextBox ID="txtJYFMC" runat="server" Width="90px" CssClass="tj_input"></asp:TextBox>
                            </td>
                               <td  valign="middle" style="text-align:right;padding-left:5px" >
                               <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" 
                                    Text="查询"  Width="50px" OnClick="btnSearch_Click" /> </td>
                              <td  valign="middle" style="padding-left:5px">
                             <asp:Button ID="btnToExcel" runat="server" CssClass="tj_bt" 
                                    Text="导出"  Width="50px" OnClick="btnToExcel_Click"/> </td>
                        </tr>
                          
                          <%--<tr>
                             <td width="10px">&nbsp;</td>
                            <td width="70px" align="right">
                                账户类型：
                            </td>
                            <td style="width: 130px">
                                <asp:DropDownList ID="ddZHLB" runat="server" Height="25px" Width="120px" CssClass="tj_input">
                                    <asp:ListItem Text="请选择" Value="">请选择</asp:ListItem>
                                      <asp:ListItem Text="经纪人交易账户" Value="经纪人交易账户">经纪人交易账户</asp:ListItem>
                                        <asp:ListItem Text="买家卖家交易账户" Value="买家卖家交易账户">买家卖家交易账户</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                             <td align="right" style="width: 120px">
                               分公司审核时间：
                            </td>
                            <td  style="width:138px;">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtFGSBeginTime" runat="server" Width="138px"></asp:TextBox>
                            </td>
                            <td align="center"  style="width:20px;">
                                至
                            </td>
                            <td style="width:138px;">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtFGSEndTime" runat="server" Width="138px"></asp:TextBox>
                            </td>
                            <td width="460px" align="left" colspan="4">
                             <table style="width:100%" >
                                 <tr>
                                     <td style=" width:120px">

                                         交易管理部审核状态：</td>
                                     <td>

                                <asp:DropDownList ID="ddshzt" runat="server" Height="25px" Width="120px" CssClass="tj_input">
                                    <asp:ListItem Text="请选择" Value="">请选择</asp:ListItem>
                                      <asp:ListItem Text="尚未处理" Value="尚未处理">尚未处理</asp:ListItem>
                                        <asp:ListItem Text="同意冻结" Value="同意冻结">同意冻结</asp:ListItem>
                                     <asp:ListItem Text="不予冻结" Value="不予冻结">不予冻结</asp:ListItem>
                                </asp:DropDownList>

                                     </td>
                                 </tr>
                             </table>
                            </td>
                            
                        </tr>
                        <tr>
                            <td width="10px">&nbsp;</td>
                            <td align="right">
                                注册类别：
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddZCLB" runat="server" Height="25px" Width="120px" CssClass="tj_input">
                                  <asp:ListItem Text="请选择" Value="">请选择</asp:ListItem>
                                      <asp:ListItem Text="单位" Value="单位">单位</asp:ListItem>
                                        <asp:ListItem Text="自然人" Value="自然人">自然人</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                             <td width="120px" align="right">
                                服务中心审核时间：
                            </td>
                            <td  style="width:138px;">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtFWZXBeginTime" runat="server" Width="138px"></asp:TextBox>
                            </td>
                            <td align="center">
                                至
                            </td>
                            <td style="width:138px;">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtFWZXEndTime" runat="server" Width="138px"></asp:TextBox>
                            </td>
                              <td align="right">
                                交易方名称：
                            </td>
                            <td align="left" style=" width:170px;">
                                <asp:TextBox ID="txtJYFMC" runat="server" Width="160px" CssClass="tj_input"></asp:TextBox>
                            </td>
                              <td valign="middle">
                               <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" 
                                    Text="查询"  Width="80px" OnClick="btnSearch_Click" /> </td>
                              <td  valign="middle" >
                             <asp:Button ID="btnToExcel" runat="server" CssClass="tj_bt" 
                                    Text="导出"  Width="80px" OnClick="btnToExcel_Click"/> </td>
                        </tr>
                            <tr>
                            <td width="10px">&nbsp;</td>
                            <td colspan="6" align="left">
                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                                </td>
                         
                              <td align="right">
                                &nbsp;</td>
                            <td align="left" style=" width:170px;">
                                &nbsp;</td>
                              <td valign="middle">
                                  &nbsp;</td>
                              <td  valign="middle" >
                                  &nbsp;</td>
                        </tr>--%>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                             建议冻结交易账户资料
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
                                            <th class="TheadTh"  style=" width:160px;word-wrap:break-word; ">
                                                交易方账号
                                            </th>
                                            <th class="TheadTh" style=" width:100px;word-wrap:break-word; ">
                                                交易方编号
                                            </th>
                                            <th class="TheadTh"style=" width:120px;word-wrap:break-word; ">
                                                账户类型
                                            </th>
                                            <th class="TheadTh" style=" width:240px;word-wrap:break-word; ">
                                                交易方名称
                                            </th>
                                            <th class="TheadTh" style=" width:70px;word-wrap:break-word; ">
                                                注册类别
                                            </th>
                                            <th class="TheadTh" style=" width:120px;word-wrap:break-word; ">
                                                分公司审核时间
                                            </th>
                                             <th class="TheadTh" style=" width:120px;word-wrap:break-word; ">
                                                服务中心审核时间
                                            </th>
                                              <th class="TheadTh" style=" width:120px;word-wrap:break-word; ">
                                                服务中心审核人
                                            </th>
                                              <th class="TheadTh" style=" width:120px;word-wrap:break-word; ">
                                                服务中心<br />审核意见
                                            </th>
                                              <th class="TheadTh" style=" width:120px;word-wrap:break-word; ">
                                                服务中心<br />新审核意见
                                            </th>
                                              <th class="TheadTh" style=" width:140px;word-wrap:break-word; ">
                                                交易管理部审核状态
                                            </th>
                                            <%--   <th class="TheadTh" style=" width:140px;word-wrap:break-word; ">
                                                交易管理部审核意见
                                            </th>--%>
                                              <th class="TheadTh" style=" width:120px;word-wrap:break-word; ">
                                                交易管理部审核人
                                            </th>
                                            <th class="TheadTh" style=" width:130px;word-wrap:break-word; ">
                                                平台管理机构
                                            </th>
                                              <th class="TheadTh" style=" width:90px;word-wrap:break-word; ">
                                                联系人姓名
                                            </th>
                                              <th class="TheadTh" style=" width:90px;word-wrap:break-word; ">
                                                联系人手机号
                                            </th>
                                           
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptZJYEBDMX" runat="server" OnItemCommand="rptZJYEBDMX_ItemCommand" >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                      <td  style=" width:90px;word-wrap:break-word; ">
                                                       <asp:LinkButton ID="linkCKXQ" runat="server" Width="90px" CommandName="linkCKXQ" CommandArgument='<%#Eval("Number")%>'>查看详情</asp:LinkButton>
                                                       
                                                    </td>
                                                    <td style=" width:160px;word-wrap:break-word; ">
                                                      <div style=" width:160px;word-wrap:break-word;">  <%#Eval("交易方账号")%></div>
                                                    </td>
                                                    <td style=" width:100px;word-wrap:break-word; ">
                                                      
                                                           <div style=" width:100px;word-wrap:break-word;">  <%#Eval("交易方编号")%></div>
                                                    </td>
                                                    <td style=" width:120px;word-wrap:break-word; ">
                                                 
                                                          <div style=" width:120px;word-wrap:break-word;">  <%#Eval("账户类型")%></div>
                                                    </td>
                                                    <td style=" width:240px;word-wrap:break-word; ">
                                                  
                                                          <div style=" width:240px;word-wrap:break-word;">  <%#Eval("交易方名称")%></div>
                                                    </td>
                                                    <td style=" width:70px;word-wrap:break-word; ">
                                                          <div style=" width:70px;word-wrap:break-word; line-height:18px;">  <%#Eval("注册类别")%></div>
                                                    </td>
                                                    <td style=" width:120px;word-wrap:break-word; ">
                                                
                                                            <div style=" width:120px;word-wrap:break-word;">  <%#Eval("分公司审核时间")%></div>
                                                    </td>
                                                      <td style=" width:120px;word-wrap:break-word; ">
                                                
                                                            <div style=" width:120px;word-wrap:break-word;">  <%#Eval("服务中心审核时间")%></div>
                                                    </td>
                                                      <td style=" width:120px;word-wrap:break-word; ">
                                                
                                                            <div style=" width:120px;word-wrap:break-word;">  <%#Eval("服务中心审核人")%></div>
                                                    </td>
                                                       <td style=" width:120px;word-wrap:break-word; ">
                                                
                                                            <div style=" width:120px;word-wrap:break-word;">  <%#Eval("服务中心审核意见")%></div>
                                                    </td>
                                                       <td style=" width:120px;word-wrap:break-word; ">
                                                
                                                            <div style=" width:120px;word-wrap:break-word;">  <%#Eval("服务中心新审核意见")%></div>
                                                    </td>
                                                     <td style=" width:140px;word-wrap:break-word; ">
                                                
                                                            <div style=" width:140px;word-wrap:break-word;">  <%#Eval("交易管理部审核状态")%></div>
                                                    </td>
                                                 <%--     <td style=" width:140px;word-wrap:break-word; ">
                                                
                                                            <div style=" width:140px;word-wrap:break-word;">  <%#Eval("交易管理部审核意见")%></div>
                                                    </td>--%>
                                                       <td style=" width:120px;word-wrap:break-word; ">
                                                
                                                            <div style=" width:120px;word-wrap:break-word;">  <%#Eval("交易管理部审核人")%></div>
                                                    </td>
                                                    <td  style=" width:130px;word-wrap:break-word; ">
                                                   
                                                           <div style=" width:130px;word-wrap:break-word;">  <%#Eval("平台管理机构")%></div>
                                                    </td>
                                                     <td  style=" width:90px;word-wrap:break-word; ">
                                                   
                                                           <div style=" width:90px;word-wrap:break-word;">  <%#Eval("联系人姓名")%></div>
                                                    </td>
                                                        <td  style=" width:90px;word-wrap:break-word; ">
                                                   
                                                           <div style=" width:90px;word-wrap:break-word;">  <%#Eval("联系人手机号")%></div>
                                                    </td>
                                                     
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                            <td colspan="14">
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

