<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JYFPFZJMX.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_XYPJ_JYFPFZJMX" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>交易方评分变化明细表</title>

     <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
    <style type="text/css">
        #content_zw
        {
            width: 1080px;
        }
        </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="" ForeColor="red"
                Text="交易方评分变化明细表">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                 <div class="content_bz" >
                    <%--说明文字：<br />
                     1、该模块用于交易扣罚的查看。<br /> --%>
                </div>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        <tr>
                            <td style="width: 80px; overflow: hidden; text-align: right;">
                                交易方账号：
                            </td>
                            <td style="text-align: left; width: 120px;">
                               <asp:TextBox ID="txtjyfzh" runat="server"   class="tj_input" Width="120px"  Height="23" ></asp:TextBox>
                            </td>
                            <td width="80px" align="right">
                                交易方名称：</td>
                            <td style="text-align: left; width: 100px;">
                               <asp:TextBox ID="txtjyfmc" runat="server"   class="tj_input" Width="120px"  Height="23" ></asp:TextBox>
                            </td>
                            
                            <td width="80px" align="right">
                                所属分公司：</td>
                           
                            <td width="100px" align="left">
                                <asp:DropDownList ID="ddlssfgs" runat="server" CssClass="tj_input" Width="100px"  Height="23"   >                                   
                                                               
                                </asp:DropDownList>
                            </td>
                            <td width="50px" style="width: 100px">
                            
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="80px" 
                                    Text="查询" onclick="btnSearch_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnSearch0" runat="server" CssClass="tj_bt" Width="80px" 
                                    Text="导出" onclick="btnExcel_Click" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                信息列表
                            </td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;table-layout:fixed;width:100%;" class="tab">
                        <tr>
                            <td>
                               <div class="content_nr_lb" style="width:1080px; ">
                                <table id="theObjTable" style="width:1400px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 100px; text-align:center;">
                                                编号
                                            </th>
                                            <th class="TheadTh" style="width: 100px; text-align:center;">
                                                交易方账号
                                            </th>
                                            <th class="TheadTh" style="width: 200px; text-align:center;">
                                                交易方名称
                                            </th>
                                            <th class="TheadTh" style="width: 100px; text-align:center;">
                                                交易账户类型
                                            </th>
                                            <th class="TheadTh" style="width: 80px; text-align:center;">
                                               注册类别
                                            </th>                                          
                                            <th class="TheadTh" style="width: 80px; text-align:center;">
                                               分数
                                            </th>                                                                                     
                                            <th class="TheadTh" style="width: 80px; text-align:center;">
                                                运算类型
                                            </th>
                                            <th class="TheadTh" style="width:400px; text-align:center;">
                                                原因</th>
                                            
                                            <th class="TheadTh" style="width: 100px; text-align:center;">
                                               所属分公司
                                            </th>                                            
                                             <th class="TheadTh" style="width: 160px; text-align:center;">
                                               创建时间
                                            </th>
                                           
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound" OnItemCommand="rpt_ItemCommand" >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td style="width:100px; text-align:center;">
                                                        <%#Eval("主键")%>
                                                    </td>
                                                    <td style="width:100px">
                                                        <%#Eval("交易方账号")%>
                                                    </td>
                                                    <td  title='<%#Eval("交易方名称")%>' style="width:200px; text-align:center;">                                                      
                                                        
                                                        <asp:Label ID="lbjyfmc" runat="server" Text='<%#Eval("交易方名称")%> '></asp:Label>
                                                    </td>
                                                    <td  style="width:100px; text-align:center;">                                                       
                                                         <%#Eval("交易账户类型")%> 
                                                    </td>
                                                      <td style="width:80px; text-align:center;">
                                                        <%#Eval("注册类别")%>
                                                        
                                                    </td>
                                                  <%--  <td title='<%#Eval("规格")%>'>
                                                       <asp:Label ID="lbgg" runat="server" Text='<%#Eval("规格")%>' > </asp:Label>
                                                    </td>--%>
                                                    <td style="width:80px; text-align:center;">
                                                        <%#Eval("分数")%>
                                                    </td>
                                                    <td style="width:80px">
                                                        <%#Eval("类型")%>
                                                    </td>
                                                    <td  title='<%#Eval("原因")%>' style="width:400px; text-align:center;">    
                                                        
                                                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("原因")%> '></asp:Label>
                                                    </td> 
                                                    
                                                    <td style="width:100px; text-align:center;">
                                                        <%#Eval("所属分公司")%>
                                                    </td>
                                                    
                                                    <td style="width:160px; text-align:center;">
                                                        <%#Eval("创建时间")%>
                                                    </td>
                                                                                                        
                                                    <%--<td>  <asp:LinkButton ID="ledit" runat="server" CommandName="lck" CommandArgument='<%#Eval("主键")%>' style="width:80px">查看</asp:LinkButton></td>--%>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                          <tr id="tdEmpty" runat="server" visible="false"  style="text-align: center;">
                                            <td colspan="13">
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
