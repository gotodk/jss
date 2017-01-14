<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JYFXYDJCX.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_XYPJ_JYFXYDJCX" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>

<%@ Register src="../UCFWJG/UCFWJGDetail.ascx" tagname="UCFWJGDetail" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>交易方信用等级查询</title>
   <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />    
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript" ></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script> 

    <script src="../../../../js/ProvinceCity.js"></script>
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
<body style="background-color: #f7f7f7;" onload="initProvince('<%= Page.IsPostBack%>','<%= Session["province"].ToString() %>','<%=Session["city"].ToString() %>' )" >
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JYFXYDJCX.aspx" ForeColor="red"
                Text="交易方信用等级查询">
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
                            <td style="text-align:left;padding-left:10px" colspan="10">                               
                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                             </td>   
                        </tr>
                        <tr>
                              <td width="70px"  style="padding-left:10px">
                                所属区域：
                            </td>
                            <td width="135px"  align="left">
                               <select id="selProvince" onChange = "getCity(this.options[this.selectedIndex].value)" runat="server" style="width:130px">
                                    <%--<option>-请选择省份-</option>--%>
                                </select>
                            </td>
                            <td  width="130px">
                                <select id="selCity" runat="server" style="width:130px;">
                                   <%-- <option>-请选择城市-</option>--%>
                                </select>                           
                            </td>   
                             <td width="80px" align="right">
                                账户类型：
                             </td>                           
                            <td width="100px" align="left">
                                <asp:DropDownList ID="ddlzllx" runat="server" CssClass="tj_input" Width="100px"  Height="23"   >
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem>买家卖家</asp:ListItem>
                                    <asp:ListItem>经纪人</asp:ListItem>
                                                               
                                </asp:DropDownList>
                            </td>                   
                            <td style="width: 80px; overflow: hidden;" align="right">
                                交易方账号：
                            </td>
                            <td style="text-align: left; width: 120px;">
                               <asp:TextBox ID="txtjyfzh" runat="server"   class="tj_input" Width="120px"  Height="23" ></asp:TextBox>
                            </td>
                            <td width="80px" align="right">
                                交易方名称：</td>
                            <td style="text-align: left; width: 120px;">
                               <asp:TextBox ID="txtjyfmc" runat="server"   class="tj_input" Width="120px"  Height="23" ></asp:TextBox>
                            </td>  
                            
                            <td  style="padding-left:10px" >
                               
                                 <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="50px" 
                                    Text="查询" onclick="btnSearch_Click" />
                                </td>
                            <td style="width:70px"></td>
                        </tr>
                       
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                信息列表</td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;table-layout:fixed;width:100%;" class="tab">
                        <tr>
                            <td>
                             <%--  <div class="content_nr_lb" style="width:1080px; ">--%>
                                <table id="theObjTable" style="width:1080px; " cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>   
                                            <th class="TheadTh" style="width: 100px; text-align:center;">
                                                账户类型
                                            </th>                                        
                                            <th class="TheadTh" style="width: 100px; text-align:center;">
                                                交易方账号
                                            </th>
                                            <th class="TheadTh" style="width: 220px; text-align:center;">
                                                交易方名称
                                            </th>
                                           
                                            <th class="TheadTh" style="width: 180px; text-align:center;">
                                               所在区域
                                            </th> 
                                            
                                            <th class="TheadTh" style="width: 100px; text-align:center;">
                                                业务管理部门
                                            </th>
                                               
                                              <th class="TheadTh" style="width: 80px; text-align:center;">
                                               信用积分
                                            </th>
                                             <th class="TheadTh" style="width: 220px; text-align:center;">
                                                 信用等级
                                            </th>
                                                                                   
                                            
                                              <th class="TheadTh" style="width: 80px; text-align:center;">
                                               操作
                                            </th>
                                           
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound" OnItemCommand="rpt_ItemCommand" >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td  style="width:100px; text-align:center;">                                                       
                                                         <%#Eval("交易账户类型").ToString().TrimEnd(new char[]{'交','易','账','户'})%> 
                                                    </td>
                                                    <td style="width:100px">
                                                        <%#Eval("交易方账号")%>
                                                    </td>
                                                    <td  title='<%#Eval("交易方名称")%>' style="width:200px; text-align:center;">                                                      
                                                        
                                                        <asp:Label ID="lbjyfmc" runat="server" Text='<%#Eval("交易方名称")%> '></asp:Label>
                                                    </td>
                                                   
                                                      <td style="width:180px; text-align:center;">
                                                        <%#Eval("所属区域")%>
                                                        
                                                    </td>
                                                  <%--  <td title='<%#Eval("规格")%>'>
                                                       <asp:Label ID="lbgg" runat="server" Text='<%#Eval("规格")%>' > </asp:Label>
                                                    </td>--%>                                                                                                    
                                                    
                                                    <td style="width:100px; text-align:center;">
                                                        <%#Eval("所属分公司")%>
                                                    </td>

                                                     <td style="width:80px; text-align:center;">
                                                        <%#Eval("信用分值")%>
                                                    </td>
                                                     <td style="width:220px; text-align:center;">
                                                      <div style="width:220px; word-wrap:break-word;"> <%#Eval("信用等级")%></div>
                                                    </td>

                                                   
                                                                                                        
                                                    <td>  <asp:LinkButton ID="ledit" runat="server" CommandName="lck" CommandArgument='<%#Eval("交易方账号")%>' style="width:80px">查看详情</asp:LinkButton></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                          <tr id="tdEmpty" runat="server" visible="false"  style="text-align: center;">
                                            <td colspan="8">
                                                您查询的数据为空！
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            <%--    </div>--%>
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
