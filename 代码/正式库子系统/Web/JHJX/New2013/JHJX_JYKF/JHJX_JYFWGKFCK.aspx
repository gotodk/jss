<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_JYFWGKFCK.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_JYKF_JHJX_JYFWGKFCK" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<%@ Register Src="~/Web/JHJX/New2013/UCFWJG/UCFWJGDetail.ascx" TagPrefix="uc1" TagName="UCFWJGDetail" %>





<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>交易方违规扣罚</title>

    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript" ></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 
     <script type="text/javascript">
         $(document).ready(function () {
             $("#theObjTable").tablechangecolor();
         });
    </script>
   
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_JYFWGKFCK.aspx" ForeColor="red"
                Text="违规扣罚一览表">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                 <div class="content_bz" >
                  
                 </div>
                <div class="content_nr">
                    <table width="100%"  cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc;height:66px">
                          <tr>
                            <td colspan="9" style="padding-left:10px" >                                  
                                  <uc1:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                            </td>
                        </tr>
                          
                        <tr>
                            <td style="overflow: hidden; text-align: right;width:118px">
                              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 交易方账号：</td>
                            <td style="text-align: left; width: 120px;">
                               <asp:TextBox ID="txtjyfzh" runat="server"   class="tj_input" Width="120px"  Height="23" ></asp:TextBox>
                            </td>
                            <td width="80px" align="right">
                                交易方名称：

                            </td>
                            <td style="text-align: left; width: 100px;">
                               <asp:TextBox ID="txtjyfmc" runat="server"   class="tj_input" Width="120px"  Height="23" ></asp:TextBox>
                            </td>
                          <%--  <td width="80px" align="right">
                                经纪人账号：

                            </td>
                            <td width="130px" align="left">
                                <asp:TextBox ID="txtjjrzh" runat="server"  class="tj_input" Width="120px"  Height="23" ></asp:TextBox>
                            
                            </td>            --%>                
                          
                             <td width="80px" align="right">
                                违规事项：

                             </td>
                            <td width="120px">                            
                                <asp:DropDownList ID="ddlwgsx" runat="server" CssClass="tj_input" Width="120px"  Height="23"   >  
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="50px" 
                                    Text="查询" onclick="btnSearch_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnSearch0" runat="server" CssClass="tj_bt" Width="50px" 
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
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                               <div class="content_nr_lb" style="width:1110px; ">
                                <table id="theObjTable"  style="width:1710px; " cellspacing="0"  cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 100px;">
                                                编号
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                交易方账号
                                            </th>
                                            <th class="TheadTh" style="width: 150px;">
                                                交易方名称
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                交易账户类型
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                               注册类别
                                            </th>                                          
                                            <th class="TheadTh" style="width: 100px;">
                                               交易方扣罚
                                            </th>                                                                                     
                                            <th class="TheadTh" style="width: 100px;">
                                                经纪人扣罚
                                            </th>
                                            <th class="TheadTh" style="width:100px;">
                                                扣罚原因</th>
                                              <th class="TheadTh" style="width: 100px;">
                                                联系人
                                            </th>
                                             <th class="TheadTh" style="width: 100px;">
                                                联系方式
                                            </th>
                                            <th class="TheadTh" style="width: 150px;">
                                               所属区域
                                            </th>
                                             <th class="TheadTh" style="width: 100px;">
                                               经纪人账号
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                业务管理部门
                                            </th>
                                             <th class="TheadTh" style="width: 80px;">
                                               操作人
                                             </th>
                                             <th class="TheadTh" style="width: 150px;">
                                               创建时间
                                            </th>
                                             <th class="TheadTh" style="width: 80px;">
                                               查看详情
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound" OnItemCommand="rpt_ItemCommand" >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td style="width:100px">
                                                        <%#Eval("主键")%>
                                                    </td>
                                                    <td style="width:100px">
                                                        <%#Eval("交易方账号")%>
                                                    </td>
                                                    <td  title='<%#Eval("交易方名称")%>' style="width:150px">   
                                                        
                                                        <asp:Label ID="lbjyfmc" runat="server" Text='<%#Eval("交易方名称")%> '></asp:Label>
                                                    </td>
                                                    <td  style="width:100px">                                                       
                                                         <%#Eval("交易账户类型")%> 
                                                    </td>
                                                      <td style="width:100px">
                                                        <%#Eval("注册类别")%>
                                                        
                                                    </td>
                                               
                                                    <td style="width:100px">
                                                        <%#Eval("交易方扣罚")%>
                                                    </td>
                                                    <td style="width:100px">                                                    
                                                         <asp:Label ID="lbjjrkf" runat="server" Text='<%#Eval("经纪人扣罚")%>' > </asp:Label>
                                                    </td>
                                                    <td style="width:100px">                                                       
                                                        <%#Eval("扣罚原因")%>  
                                                    </td>
                                                      <td style="width:100px">                                                       
                                                        <%#Eval("联系人")%> 
                                                    </td>
                                                    <td style="width:100px">
                                                        <%#Eval("联系方式")%>
                                                    </td>
                                                    <td style="width:150px">
                                                        <%#Eval("所属区域")%>
                                                    </td>
                                                    <td style="width:100px">
                                                        <%#Eval("经纪人账号")%>
                                                    </td>
                                                    <td style="width:100px">
                                                        <%#Eval("业务管理部门")%>
                                                    </td>
                                                    <td style="width:80px">
                                                        <%#Eval("操作人")%>
                                                    </td>
                                                    <td style="width:150px">
                                                        <%#Eval("创建时间")%>
                                                    </td>
                                                                                                        
                                                    <td>  <asp:LinkButton ID="ledit" runat="server" CommandName="lck" CommandArgument='<%#Eval("主键")%>' style="width:80px">查看</asp:LinkButton></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>                                         
                                    </tbody>
                                    <tfoot>
                                         <tr id="tdEmpty" runat="server" visible="false"  style="text-align: center;">
                                            <td colspan="16">
                                                您查询的数据为空！
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
    <input runat="server" id="hidID" type="hidden" />
    </form>
</body>
</html>
