<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Import_YGFZJJRXX_LB.aspx.cs" Inherits="Web_JHJX_Import_YGFZJJRXX_LB" %>

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
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="Import_YGFZJJRXX_LB.aspx" ForeColor="red"
                Text="员工发展经纪人信息列表">
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
                            <td style="width: 70px; overflow: hidden; text-align: right;">
                                员工工号：
                            </td>
                            <td style="text-align: left; width: 130px;">
                                <asp:TextBox ID="txtYGGH" runat="server"
                                    class="tj_input" Width="130px"></asp:TextBox>
                            
                            </td>
                            <td width="70px" align="right">
                                员工姓名：
                            </td>
                            <td width="130px" align="left">
                               <asp:TextBox ID="txtYGXM" runat="server"
                                    class="tj_input" Width="130px"></asp:TextBox>
                            </td>
                            <td width="145px" align="right">
                            关联经纪人交易方名称：
                            </td>
                            <td width="130px" align="left">
                                <asp:TextBox ID="txtGLJJRJYFMC" runat="server"
                                    class="tj_input" Width="130px"></asp:TextBox>
                            </td>
                            <td width="140px" align="right">
                            关联经纪人登陆邮箱：
                            </td>
                            <td width="130px" align="left">
                                <asp:TextBox ID="txtGLJJRDLYX" runat="server"
                                    class="tj_input" Width="130px"></asp:TextBox>
                            </td>
                            <td width="20px"></td>
                            <td align="left">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="70px" 
                                    Text="查询" onclick="btnSearch_Click" />&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                               员工发展经纪人信息列表
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
                                            <th class="TheadTh" style="width: 100px;">
                                                员工工号
                                            </th>
                                            <th class="TheadTh" style="width: 160px;">
                                                员工姓名
                                            </th>
                                            <th class="TheadTh" style="width: 200px;">
                                                关联经纪人交易方名称
                                            </th>
                                               <th class="TheadTh" style="width: 130px;">
                                                关联经纪人登陆邮箱
                                            </th>
                                            <th class="TheadTh" style="width: 150px;">
                                                关联经纪人注册类别
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                是否有效
                                            </th>
                                            <th class="TheadTh" style="width: 220px;">
                                                备注
                                            </th>        
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptDLRSH" runat="server"  >
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td>
                                                        <%#Eval("YGGH")%>
                                                    </td>
                                                    <td style=" width:160px; word-wrap:break-word; line-height:18px; ">
                                                        <%#Eval("YGXM")%>
                                                    </td>
                                                    <td style="width: 200px; word-wrap:break-word; line-height:18px;">
                                                        <%#Eval("GLJJRJYFMC")%>
                                                    </td>
                                                      <td style="width: 130px; word-wrap:break-word; line-height:18px;">
                                                        <%#Eval("GLJJRDLYX")%>
                                                    </td>
                                                    <td style="width: 150px; word-wrap:break-word; line-height:18px;">
                                                        <%#Eval("GLJJRZCLB")%>
                                                    </td>
                                                    <td style="width: 100px; word-wrap:break-word; line-height:18px;">
                                                        <%#Eval("SFYX")%>
                                                    </td>
                                                    <td style="width: 220px; word-wrap:break-word; line-height:18px;">
                                                        <%#Eval("BZ")%>&nbsp;
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                          <tr id="tdEmpty" runat="server" visible="false"  style="text-align: center;">
                                            <td colspan="7">
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
