<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JYPT_WSH.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_PTZHZLSH_JYPT_WSH" %>

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
            <radTS:Tab ID="Tab1" runat="server" ForeColor="Red" NavigateUrl="JYPT_WSH.aspx" 
                Text="未审核">
            </radTS:Tab>
                 <radTS:Tab ID="Tab2" runat="server" NavigateUrl="JYPT_SHTG.aspx"
                Text="已审核通过"> </radTS:Tab>
                         <radTS:Tab ID="Tab3" runat="server" NavigateUrl="JHJX_JYDJ.aspx" 
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
                      <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc;height:66px;">
                         <tr>
                           <td width="10px" align="right">
                              &nbsp;
                            </td>
                            <td colspan="10" align="left" >
                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                          
                            </td>                          
                        
                        </tr>
                          <tr>
                           <td width="10px" align="right">
                              &nbsp;
                            </td>
                            <td width="65px" align="left">
                                账户类型：
                            </td>
                            <td style="width: 125px">
                                  <asp:DropDownList ID="ddZHLB" runat="server" Height="25px" Width="125px" CssClass="tj_input" >
                                      <asp:ListItem Text="请选择" Value="">请选择</asp:ListItem>
                                      <asp:ListItem Text="经纪人交易账户" Value="经纪人交易账户">经纪人交易账户</asp:ListItem>
                                        <asp:ListItem Text="买家卖家交易账户" Value="买家卖家交易账户">买家卖家交易账户</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                             <td align="right" style="width:70px">
                                注册类别：
                            </td>
                              <td Width="125px">
                                 <asp:DropDownList ID="ddZCLB" runat="server" Height="25px" Width="125px" CssClass="tj_input" >
                                     <asp:ListItem Text="请选择" Value="">请选择</asp:ListItem>
                                      <asp:ListItem Text="自然人" Value="自然人">自然人</asp:ListItem>
                                      <asp:ListItem Text="单位" Value="单位">单位</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                             <td style="width:85px;text-align:right;">
                                交易方账号：
                            </td>
                            <td width="120px">
                                <asp:TextBox  ID="txtJYFZH" runat="server" CssClass="tj_input"  Width="120px"></asp:TextBox>
                                    
                            </td>
                            <td style="width:85px;text-align:right;">
                                交易方名称：
                            </td>
                            <td width="120px">
                                <asp:TextBox  ID="txtJYFMC" runat="server" CssClass="tj_input"  Width="120px"></asp:TextBox>
                                    
                            </td>                           
                      
                            <td width="120px">
                               <table  width="100%" cellpadding="0" cellspacing="0">
                                    <tr><td align="right" style="padding-left:10px"> <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" 
                                    Text="查询"  Width="50px" OnClick="btnSearch_Click" /></td><td align="left" style="padding-left:10px;"><asp:Button ID="btnToExcel" runat="server" CssClass="tj_bt" 
                                    Text="导出"  Width="50px" OnClick="btnToExcel_Click"  /></td>
                                </table>
                            </td>                        
                             
                            <td width="210px">
                            
                            </td>
                        </tr>
                       
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                              未审核资料
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
                                             <th class="TheadTh" style=" width:80px;word-wrap:break-word; ">
                                                操作
                                            </th>
                                            <th class="TheadTh"  style=" width:180px;word-wrap:break-word; ">
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
                                            <th class="TheadTh" style=" width:130px;word-wrap:break-word; ">
                                                业务管理部门
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
                                                       <td  style=" width:80px;word-wrap:break-word; ">
                                                       <asp:LinkButton ID="linkCKXQ" runat="server" Width="90px" CommandName="linkCKXQ" CommandArgument='<%#Eval("Number")%>'>查看详情</asp:LinkButton>
                                                       
                                                    </td>
                                                    <td style=" width:180px;word-wrap:break-word; word-break:break-all; line-height:18px">
                                                      <div style=" width:180px;word-wrap:break-word; word-break:break-all ; line-height:18px">  <%#Eval("交易方账号")%></div>
                                                    </td>
                                                    <td style=" width:100px;word-wrap:break-word; ">
                                                      
                                                           <div style=" width:100px;word-wrap:break-word;">  <%#Eval("交易方编号")%></div>
                                                    </td>
                                                    <td style=" width:120px;word-wrap:break-word; ">
                                                 
                                                          <div style=" width:120px;word-wrap:break-word;">  <%#Eval("账户类型")%></div>
                                                    </td>
                                                    <td style=" width:240px;word-wrap:break-word; word-break:break-all; line-height:18px ">
                                                  
                                                          <div style=" width:240px;word-wrap:break-word; word-break:break-all; line-height:18px">  <%#Eval("交易方名称")%></div>
                                                    </td>
                                                    <td style=" width:70px;word-wrap:break-word; ">
                                                          <div style=" width:70px;word-wrap:break-word; line-height:18px;">  <%#Eval("注册类别")%></div>
                                                    </td>
                                                    <td style=" width:120px;word-wrap:break-word; ">
                                                
                                                            <div style=" width:120px;word-wrap:break-word;">  <%#Eval("分公司审核时间")%></div>
                                                    </td>
                                                    <td  style=" width:130px;word-wrap:break-word; word-break:break-all ;line-height:18px ">
                                                   
                                                           <div style=" width:130px;word-wrap:break-word; word-break :break-all ;line-height:18px">  <%#Eval("平台管理机构")%></div>
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
                                            <td colspan="10">
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
