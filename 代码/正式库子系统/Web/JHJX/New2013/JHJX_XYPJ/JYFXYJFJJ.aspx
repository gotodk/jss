<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JYFXYJFJJ.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_XYPJ_JYFXYJFJJ" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<%@ Register src="../UCFWJG/UCFWJGDetail.ascx" tagname="UCFWJGDetail" tagprefix="uc2" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>交易方信用积分调整</title>
     <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
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
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JYFXYJFJJ.aspx" ForeColor="red"
                Text="交易方信用评分调整">
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
                            <td style="text-align:left;padding-left:10px" colspan="8">                           
                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                            </td>
                           
                        </tr>
                        <tr>
                            <td style="width: 118px; overflow: hidden; text-align: right;">
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
                           
                            <td width="70px" style="text-align:right">
                              合同编号：
                                </td>
                             <td style="text-align: left; width: 80px;">
                               <asp:TextBox ID="txthtbh" runat="server"   class="tj_input" Width="120px"  Height="23" ></asp:TextBox>
                            </td>
                           
                            <td  style="padding-left:10px">
                             
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="50px" 
                                    Text="查询" onclick="btnSearch_Click" />&nbsp;&nbsp;
                                </td>
                              <td >                            
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
                                <table id="theObjTable" style="width:1140px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>   
                                            <th class="TheadTh" style="width: 80px; text-align:center;">
                                                角色类型
                                            </th>                                        
                                            <th class="TheadTh" style="width: 100px; text-align:center;">
                                                交易方账号
                                            </th>
                                            <th class="TheadTh" style="width: 200px; text-align:center;">
                                                交易方名称
                                            </th>
                                            <th class="TheadTh" style="width: 80px; text-align:center;">
                                                账户类型
                                            </th>
                                            <th class="TheadTh" style="width: 80px; text-align:center;">
                                               联系人
                                            </th>  
                                               <th class="TheadTh" style="width: 80px; text-align:center;">
                                               联系电话
                                            </th> 
                                            
                                            <th class="TheadTh" style="width: 100px; text-align:center;">
                                                业务管理部门
                                            </th>   
                                              <th class="TheadTh" style="width: 80px; text-align:center;">
                                               当前积分
                                            </th>
                                             <th class="TheadTh" style="width: 100px; text-align:center;">
                                               合同编号
                                            </th>
                                                                                   
                                             <th class="TheadTh" style="width: 130px; text-align:center;">
                                               定标时间
                                            </th>
                                              <th class="TheadTh" style="width: 70px; text-align:center;">
                                               操作
                                            </th>
                                           
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound" OnItemCommand="rpt_ItemCommand" >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td  style="width:80px; text-align:center;">                                                       
                                                         <%#Eval("交易账户类型")%> 
                                                    </td>
                                                    <td style="width:100px">
                                                        <%#Eval("交易方账号")%>
                                                    </td>
                                                    <td  title='<%#Eval("交易方名称")%>' style="width:200px; text-align:center;">                                                      
                                                        
                                                        <asp:Label ID="lbjyfmc" runat="server" Text='<%#Eval("交易方名称")%> '></asp:Label>
                                                    </td>
                                                     <td style="width:80px; text-align:center;">
                                                        <%#Eval("角色类型").ToString().TrimEnd(new char[]{'交','易','账','户'})%>
                                                        
                                                    </td>
                                                      <td style="width:80px; text-align:center;">
                                                        <%#Eval("联系人")%>
                                                        
                                                    </td>
                                                  <%--  <td title='<%#Eval("规格")%>'>
                                                       <asp:Label ID="lbgg" runat="server" Text='<%#Eval("规格")%>' > </asp:Label>
                                                    </td>--%>
                                                    <td style="width:80px; text-align:center;">
                                                        <%#Eval("联系方式")%>
                                                    </td>                                                   
                                                    
                                                    <td style="width:100px; text-align:center;">
                                                        <%#Eval("所属分公司")%>
                                                    </td>
                                                     <td style="width:80px; text-align:center;">
                                                        <%#Eval("信用分值")%>
                                                    </td>
                                                     <td style="width:100px; text-align:center;">
                                                        <%#Eval("合同编号")%>
                                                    </td>

                                                    <td style="width:130px; text-align:center;">
                                                        <%#Eval("定标时间")%>
                                                    </td>
                                                                                                        
                                                    <td>  <asp:LinkButton ID="ledit" runat="server" CommandName="lck" CommandArgument='<%#Eval("信息表编号")%>' style="width:70px">加减积分</asp:LinkButton></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                          <tr id="tdEmpty" runat="server" visible="false"  style="text-align: center;">
                                            <td colspan="11">
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
