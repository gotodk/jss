﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PTKPXX_View.aspx.cs" Inherits="Web_JHJX_PTKPXX_View" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>
<%@ Register src="../New2013/UCFWJG/UCFWJGDetail.ascx" tagname="UCFWJGDetail" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-平台开票信息查询</title>
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
    <style type="text/css">
        #content_zw
        {
            width: 1000px;
        }
    </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="PTKPXX_Check.aspx" Text="开票信息审核">
            </radTS:Tab>
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="PTKPXXChange_Check.aspx" Text="开票信息变更申请审核">
            </radTS:Tab>
            <radTS:Tab ID="Tab3" runat="server" NavigateUrl="PTKPXX_View.aspx" Text="有效开票信息查询"
                ForeColor="red">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <%--  <div class="content_bz">
                    说明文字：<br />
                    该模块用于交易方提交的开票信息的审核，审核通过后开票信息生效。
                </div>--%>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        <tr>
                            <td colspan ="7" style ="padding-left:10px" align ="left" >
                                
                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                                
                            </td>
                        </tr>
                        <tr>                            
                            <td style="width: 90px; overflow: hidden; text-align: right;">
                                发票类别：
                            </td>
                            <td style="text-align: left; width: 120px;">
                                <asp:DropDownList ID="ddlFPLX" runat="server" CssClass="tj_input">
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem>增值税专用发票</asp:ListItem>
                                    <asp:ListItem>增值税普通发票</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="80px" align="right">
                                客户编号：
                            </td>
                            <td width="90px" align="left">
                                <asp:TextBox ID="txtKHBH" runat="server" class="tj_input" Width="90px"></asp:TextBox>
                            </td>
                            <td width="80px" align="right">
                                单位名称：
                            </td>
                            <td width="90px" align="left">
                                <asp:TextBox ID="txtDWMC" runat="server" class="tj_input" Width="90px"></asp:TextBox>
                            </td>
                            <td align="left" style ="padding-left:15px" width="450px">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="50px" Text="查询"
                                    OnClick="btnSearch_Click" />&nbsp;&nbsp;<asp:Button ID="btnExport" runat="server"
                                        CssClass="tj_bt" Width="100px" Text="导出到excel" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                发票/邮递信息列表
                            </td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse;
                        table-layout: fixed; width: 1000px;" class="tab">
                        <tr>
                            <td>
                                <table id="theObjTable" style="width: 1000px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 110px;">
                                                平台管理机构
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                客户编号
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                交易账户类型
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                注册类别
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                发票类型
                                            </th>
                                            <th class="TheadTh" style="width: 150px;">
                                                单位名称
                                            </th>
                                            <th class="TheadTh" style="width: 120px;">
                                                专票信息
                                            </th>
                                            <th class="TheadTh" style="width: 120px;">
                                                邮递信息
                                            </th>
                                            <th class="TheadTh" style="width: 120px;">
                                                最后更新时间
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptKPXX" runat="server" OnItemDataBound="rptKPXX_ItemDataBound">
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td title='<%#Eval("ptgljg")%>'>
                                                        <asp:Label ID="lblDLYX" runat="server" Text='<%#Eval("ptgljg")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <%#Eval("khbh").ToString().Trim ()==""?"无":Eval("khbh")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("jyzhlx")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("zclb")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("fplx")%>
                                                    </td>
                                                    <td title='<%#Eval("dwmc")%>'>
                                                        <asp:Label ID="lblDWMC" runat="server" Text='<%#Eval("dwmc")%>'> </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("zpxx")%>'>
                                                        <asp:Label ID="lblZPXX" runat="server" Text='<%#Eval("zpxx")%>'> </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("ydxx")%>'>
                                                        <asp:Label ID="lblydxx" runat="server" Text='<%#Eval("ydxx")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <%#Eval("zhgxsj")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                            <td colspan="9">
                                                暂无满足条件的数据！
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
