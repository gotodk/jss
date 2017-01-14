﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_MMJCKLB.aspx.cs" Inherits="Web_JHJX_JHJX_MMJCKLBt" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台审核代理人功能</title>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
    <style type="text/css">
        #content_zw
        {
            width: 1110px;
        }
        </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="JHJX_SHDLR.aspx" ForeColor="red"
                Text="分公司审核买卖家功能">
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
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        <tr>
                            <td style="width: 90px; overflow: hidden; text-align: right;">
                                所属分公司：
                            </td>
                            <td style="text-align: left; width: 180px;">
                                <asp:DropDownList ID="ddrFGSMC" runat="server" CssClass="tj_input" Width="175px">
                                </asp:DropDownList>
                            </td>
                            <td width="70px" align="right">
                                角色分类：
                            </td>
                            <td style="text-align: left; width: 100px;">
                                <asp:DropDownList ID="drpJSFL" runat="server" CssClass="tj_input" Width="100px">
                                    <asp:ListItem>所有分类</asp:ListItem>
                                    <asp:ListItem>买家</asp:ListItem>
                                    <asp:ListItem>卖家</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="60px" align="right">
                                用户名：
                            </td>
                            <td width="130px" align="left">
                               <asp:TextBox ID="txtYHM" runat="server" onkeyup="value=value.replace(/[^\w\d]/g,'')"
                                    onpaste="value=value.replace(/[^\w\d]/g,'')" oncontextmenu="value=value.replace(/[^\w\d]/g,'')"
                                    class="tj_input" Width="120px"></asp:TextBox>
                            </td>
                            <td width="60px" align="right">
                                注册时间：
                            </td>
                            <td width="80px" align="left">
                                 <asp:TextBox ID="txtTimeStart" runat="server" Width="100px" class="tj_input Wdate"
                                                onClick="WdatePicker({readOnly:true,minDate:'2012-12-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"></asp:TextBox>
                           
                            </td>
                            <td width="10px" align="right">
                             至
                            </td>
                            <td width="80px" align="left">
                                  <asp:TextBox ID="txtTimeEnd" runat="server" Width="100px" class="tj_input Wdate"
                                                
                                      onClick="WdatePicker({readOnly:true,minDate:'2012-12-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"></asp:TextBox>
                            </td>
                            <td width="50px">
                            
                            </td>
                            <td align="left">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="80px" 
                                    Text="查询" onclick="btnSearch_Click" />&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                               买家、卖家信息列表
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                                <table id="theObjTable" style="width: 1110px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 50px;">
                                                序号
                                            </th>
                                            <th class="TheadTh" style="width: 110px;">
                                                所属分公司
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                用户名
                                            </th>
                                               <th class="TheadTh" style="width: 50px;">
                                                角色
                                            </th>
                                            <th class="TheadTh" style="width: 150px;">
                                                邮箱
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                联系人姓名
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                手机号
                                            </th>                                           
                                            <th class="TheadTh" style="width: 130px;">
                                                注册时间
                                            </th>
                                            <th class="TheadTh" style="width: 130px;">
                                                经纪人审核时间
                                            </th>
                                              <th class="TheadTh" style="width: 130px;">
                                                分公司审核时间
                                            </th>
                                            <th class="TheadTh" style="width: 60px;">
                                                操作
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptDLRSH" runat="server"  >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td>
                                                        <%#Eval("ROWID")%>
                                                    </td>
                                                    <td style=" width:120px; word-wrap:break-word; ">
                                                        <%#Eval("SSFGS")%>
                                                    </td>
                                                    <td style="width: 100px; word-wrap:break-word;">
                                                        <%#Eval("YHM")%>
                                                    </td>
                                                      <td>
                                                        <%#Eval("JSLX")%>
                                                        <asp:HiddenField ID="hidJSLX" runat="server" Value='<%#Eval("JSLX")%>' />
                                                    </td>
                                                    <td style="width: 150px; word-wrap:break-word;">
                                                        <%#Eval("DLYX")%>
                                                    </td>
                                                    <td style="width: 100px; word-wrap:break-word;">
                                                        <%#Eval("LXRXM")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("SJH")%>
                                                    </td>
                                                    <td style="width: 150px; word-wrap:break-word;">
                                                        <%#Eval("ZCSJ")%>&nbsp;
                                                    </td>
                                                      <td>
                                                        <%#Eval("KTSHTGSJ")%>&nbsp;
                                                    </td>
                                                    <td>
                                                        <%#Eval("FGSKTSHTGSJ")%>&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="linkSee" runat="server" CommandName="linkSee" CommandArgument='<%#Eval("JSBH")%>'>审核资料</asp:LinkButton>
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
