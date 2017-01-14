<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_FGSJYZHSH_JJR.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_FGSJYZHSH_JJR" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>

<%@ Register src="../../UCCityList.ascx" tagname="UCCityList" tagprefix="uc2" %>

<%@ Register src="UCFWJG/UCFWJGDetail.ascx" tagname="UCFWJGDetail" tagprefix="uc3" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>分公司审核经纪人资料</title>
    <script src="../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_FGSJYZHSH_JJR.aspx" ForeColor="red"
                Text="审核经纪人资料">
            </radTS:Tab>
               <radTS:Tab ID="Tab2" runat="server" NavigateUrl="JHJX_FGSJYZHSH_MMJ.aspx" 
                Text="审核交易方资料">
            </radTS:Tab>
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
                        
                         <td width="70px" align="right">
                                所属区域：
                            </td>
                            <td  style="display:inline;width:380px" colspan="3">
                                <uc2:UCCityList ID="UCCityList1"   runat="server" />                             
                             
                                
                            </td>
                            <td colspan="6" style="text-align:left">   <uc3:UCFWJGDetail ID="UCFWJGDetail1" runat="server" /></td>
                        
                            
                           
                        </tr>      
                        <tr>
                           
                            <td width="70px" align="right">
                                初审记录：
                            </td>
                            <td width="135px" align="left">
                                <asp:DropDownList ID="ddLCSJL" runat="server" Width="135px" CssClass="tj_input"
                                    Height="22px">
                                      <asp:ListItem Text="请选择" Value="">请选择</asp:ListItem>
                                    <asp:ListItem Text="审核中" Value="审核中" >审核中</asp:ListItem>
                                    <asp:ListItem Text="驳回" Value="驳回" >驳回</asp:ListItem>
                                </asp:DropDownList>
                           
                            </td>
                             <td width="110px" align="right">
                             驳回后是否修改：
                            </td>
                            <td width="135px" align="left">
                                <asp:DropDownList ID="ddlSFXG" runat="server" Width="135px" CssClass="tj_input"
                                    Height="22px">
                                      <asp:ListItem Text="全部" Value="">全部</asp:ListItem>
                                    <asp:ListItem Text="是" Value="是" >是</asp:ListItem>
                                    <asp:ListItem Text="否" Value="否" >否</asp:ListItem>
                                </asp:DropDownList>
                           
                            </td>
                            <td style="text-align:right;width:85px">交易方账号：</td>
                            <td style="text-align:left;width:120px">                                
                               <asp:TextBox ID="txtJYFZH" runat="server" 
                                    
                                    class="tj_input" Width="120px"></asp:TextBox>
                            </td>
                            <td style="text-align:right;width:85px"> 
                                 交易方名称：
                            </td>
                             <td width="120px" align="left">
                         
                               <asp:TextBox ID="txtJYFMC" runat="server" 
                                    
                                    class="tj_input" Width="120px"></asp:TextBox>
                         
                            </td>
                            <td  style="padding-left:10px;" >
                                  <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="50px" 
                                    Text="查询" onclick="btnSearch_Click"
                                     /></td>
                            <td style="width:220px"></td>
                        </tr>   
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                               经纪人信息列表
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                                   <div id="exprot" runat="server" style=" width:1110px;" class="content_nr_lb">
                                <table id="theObjTable" style="width: 1250px;" cellspacing="0" cellpadding="0">
                                   <thead>
                                        <tr>
                                                <th class="TheadTh" style="width: 90px;  text-align: center;">
                                                审核操作
                                            </th>
                                            <th class="TheadTh" style="width:130px;  text-align: center;">
                                                交易方账号
                                            </th>
                                            <th class="TheadTh" style="width: 120px;  text-align: center;">
                                                交易方名称
                                            </th>
                                            <th class="TheadTh" style="width: 100px;  text-align: center;">
                                                资料提交时间
                                            </th>
                                            <th class="TheadTh" style="width: 60px;  text-align: center;">
                                                注册类型
                                            </th>
                                            <th class="TheadTh" style="width: 200px;  text-align: center;">
                                                所属区域
                                            </th>                                           
                                            <th class="TheadTh" style="width: 130px;  text-align: center;">
                                                业务管理部门
                                            </th>
                                            <th class="TheadTh" style="width: 100px;  text-align: center;">
                                                联系人姓名
                                            </th>
                                            <th class="TheadTh" style="width: 100px;  text-align: center;">
                                                联系电话
                                            </th>
                                             <th class="TheadTh" style="width: 70px;  text-align: center;">
                                                初审记录
                                            </th>
                                             <th class="TheadTh" style="width: 150px;  text-align: center;">
                                                驳回后是否修改
                                            </th>
                                          
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptDLRSH" runat="server" onitemcommand="rptDLRSH_ItemCommand" 
                                          
                                           >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                      <td style="width:90px;word-wrap:break-word; ">
                                                        <div style=" width:90px; word-wrap:break-word;"> <asp:LinkButton ID="linkJXSH" runat="server" CommandName="linkJXSH" CommandArgument='<%#Eval("Number")%>'>进行审核</asp:LinkButton></div> 
                            
                                                    </td>
                                                    <td align="center" style="width:130px;word-wrap:break-word; ">
                                                    <div style=" width:130px; word-wrap:break-word; "><%#Eval("交易方账号")%></div>
                                                    </td>
                                                   <td align="center" style="width:120px;word-wrap:break-word; ">
                                                    <div style=" width:120px; word-wrap:break-word; "><%#Eval("交易方名称")%></div>
                                                    </td>
                                                   <td align="center" style="width:100px;word-wrap:break-word; ">
                                                    <div style=" width:100px; word-wrap:break-word; "><%#Eval("资料提交时间")%></div>
                                                    </td>
                                                  <td align="center" style="width:60px;word-wrap:break-word; ">
                                                    <div style=" width:60px; word-wrap:break-word; "><asp:Label ID="lblb" runat="server" Text='<%#Eval("注册类型")%>' > </asp:Label>    </div>
                                                    </td>
                                                    <td align="center" style="width:200px;word-wrap:break-word; ">
                                                    <div style=" width:200px; word-wrap:break-word; "><%#Eval("所属区域")%></div>
                                                    </td>
                                                  <td align="center" style="width:130px;word-wrap:break-word; ">
                                                    <div style=" width:130px; word-wrap:break-word;"><asp:Label ID="lbjg" runat="server" Text='<%#Eval("平台管理机构")%>' > </asp:Label>  </div>
                                                    </td>
                                                    <td align="center" style="width:100px;word-wrap:break-word; ">
                                                    <div style=" width:100px; word-wrap:break-word;"><%#Eval("联系人姓名")%></div>
                                                    </td>
                                                    <td align="center" style="width:100px;word-wrap:break-word; ">
                                                    <div style=" width:100px; word-wrap:break-word; "><%#Eval("联系电话")%></div>
                                                    </td>
                                                    <td align="center" style="width:70px;word-wrap:break-word; ">
                                                    <div style=" width:70px; word-wrap:break-word;"><%#Eval("初审记录")%></div>
                                                    </td>
                                                      <td align="center" style="width:150px;word-wrap:break-word; ">
                                                    <div style=" width:70px; word-wrap:break-word;"><%#Eval("驳回后是否修改")%></div>
                                                    </td>
                                                  
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
   
    </form>
</body>
</html>
