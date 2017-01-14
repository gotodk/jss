<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjx_jjrsyhzbgr.aspx.cs" 
    Inherits="Web_JHJX_JJRSYGR_jhjx_jjrsyhzbgr" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>
<%@ Register src="../New2013/UCFWJG/UCFWJGDetail.ascx" tagname="UCFWJGDetail" tagprefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>经纪人（自然人）收益汇总表</title>
    <script src="../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
    <style type="text/css">
        #content_zw
        {
            width: 1024px;
        }
        </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="jhjx_jjrsyhzbgr.aspx" ForeColor="red"
                Text="经纪人（自然人）收益汇总表">
            </radTS:Tab>
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="jhjx_jjrsycsmxb.aspx" 
                Text="经纪人收益产生明细表">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc;height:66px">
                        <tr>
                            
                            
                            <td  style="width:680px;padding-left:10px" >
                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                            </td>
                        </tr>
                       
                        <tr>
                            <td >
                                 <table width="100%" >
           <tr>
                        <td  style="padding-left:10px ;width:40px">
                                年份：
                            </td>
                            <td width="75px" align="left">
                                <asp:DropDownList ID="ddlnf" runat="server" CssClass="tj_input" Width="75px" Height="23">
                                  <asp:ListItem >无</asp:ListItem>
                                     <asp:ListItem>2013</asp:ListItem>
                                    <asp:ListItem>2014</asp:ListItem>
                                    <asp:ListItem>2015</asp:ListItem>
                                    <asp:ListItem>2016</asp:ListItem>
                                    <asp:ListItem>2017</asp:ListItem>
                                    <asp:ListItem>2018</asp:ListItem>
                                    <asp:ListItem>2019</asp:ListItem>
                                   
                                </asp:DropDownList>
                            </td>
                            <td width="40px" align="right">
                                月份：
                            </td>
                            <td width="55px"> 
                              <asp:DropDownList ID="ddlyf" runat="server" CssClass="tj_input" Width="40px" Height="23">
                               <asp:ListItem>无</asp:ListItem>
                                    <asp:ListItem>01</asp:ListItem>
                                    <asp:ListItem>02</asp:ListItem>
                                    <asp:ListItem>03</asp:ListItem>
                                    <asp:ListItem>04</asp:ListItem>
                                    <asp:ListItem>05</asp:ListItem>
                                    <asp:ListItem>06</asp:ListItem>
                                    <asp:ListItem>07</asp:ListItem>
                                    <asp:ListItem>08</asp:ListItem>
                                    <asp:ListItem>09</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>11</asp:ListItem>
                                    <asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList>
                            </td>
                            <td style="width: 80px; overflow: hidden; text-align: right;">
                                经纪人编号：</td>
                            <td style="text-align: left; width: 120px;">
                                <asp:TextBox ID="txtjjrbh" runat="server" class="tj_input" Width="110px" Height="23"></asp:TextBox>
                            </td>    
                        <td width="80px" align="left">
                               &nbsp;
                               经纪人名称：</td>
                            <td width="75px" align="left">
                                <asp:TextBox ID="txtjjrxm" runat="server" class="tj_input" Width="110px" Height="23"></asp:TextBox>
                            </td>
                            
                               
                            <td style="width:60px; overflow: hidden; text-align: right;">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="50px" Text="查询"
                                    OnClick="btnSearch_Click" /></td>
                            <td style="text-align: left; padding-left:10px; width: 60px;">
                                <asp:Button ID="Button1" runat="server" CssClass="tj_bt" Width="50px" Text="导出"
                                    OnClick="Button1_Click" /></td>
                           
                            <td align="left">
                                &nbsp;&nbsp;
                            </td>
            </tr>
     </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                <span>经纪人（自然人）收益汇总表</span>
                                （金额单位：元）</td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse;
                        table-layout: fixed; width: 100%;" class="tab">
                        <tr>
                            <td>
                                <table id="theObjTable" style="width: 100%;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 100px; text-align: center;">
                                                收益时间(年月)
                                            </th>
                                            <th class="TheadTh" style="width: 100px;text-align: center;">
                                                业务管理部门
                                            </th>
                                            <th class="TheadTh" style="width: 100px;text-align: center;">
                                                经纪人编号
                                            </th>
                                            <th class="TheadTh" style="width: 150px;text-align: center;">
                                                经纪人名称
                                            </th>
                                            <th class="TheadTh" style="width: 80px;text-align: center;">
                                                总收益金额
                                            </th>
                                            <th class="TheadTh" style="width: 100px;text-align: center;">
                                                扣税金额
                                            </th>
                                            <th class="TheadTh" style="width: 100px;text-align: center;">
                                                可支取收益
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptSPXX" runat="server">
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td style="width: 100px;">
                                                        <%#Eval("收益时间")%>
                                                    </td>
                                                    <td title='<%#Eval("业务管理部门")%>' style="width: 100px;">
                                                        <asp:Label ID="lbbuyer" runat="server" Text='<%#Eval("业务管理部门")%>'> </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("经纪人编号")%>' style="width: 100px;">
                                                        <asp:Label ID="lbseller" runat="server" Text='<%#Eval("经纪人编号")%>'> </asp:Label>
                                                    </td>
                                                    <td style="width: 150px;">
                                                        <%#Eval("经纪人名称")%>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <%#Eval("总收益金额")%>
                                                    </td>
                                                    <td title='<%#Eval("扣税金额")%>' style="width: 100px;">
                                                        <asp:Label ID="lbspmc" runat="server" Text='<%#Eval("扣税金额")%>'> </asp:Label>
                                                    </td>
                                                    <td style="width: 100px;">
                                                        <%#Eval("可支取收益")%>
                                                    </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                            <td colspan="7">
                                                您查询的数据为空！
                                             
                                              
                                               
                                             
                                              
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table> <uc1:commonpagernew ID="commonpagernew1" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <input runat="server" id="hidID" type="hidden" />
        <input runat="server" id="hidwhere" type="hidden" />  
         <input runat="server" id="hidwhereis" type="hidden" />  
    </form>
</body>
</html>
