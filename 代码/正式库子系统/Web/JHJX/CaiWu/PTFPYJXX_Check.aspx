<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PTFPYJXX_Check.aspx.cs" Inherits="Web_JHJX_PTFPYJXX_Check" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>
<%@ Register src="../New2013/UCFWJG/UCFWJGDetail.ascx" tagname="UCFWJGDetail" tagprefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-平台发票邮递信息管理</title>
    <script src="../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
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
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="PTFPYJXX_LR.aspx" Text="平台发票邮递信息录入">
            </radTS:Tab>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="PTFPYJXX_Check.aspx" Text="平台发票邮递信息修改审核"
                ForeColor="Red">
            </radTS:Tab>
            <radTS:Tab ID="Tab3" runat="server" NavigateUrl="PTFPYDXX_View.aspx" Text="平台发票邮递信息查询">
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
                            <td colspan ="9" style="text-align: left; padding-left:10px;height :30px">
                               
                                <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                               
                            </td>                                       
                        </tr>
                        <tr>
                             <td style="width: 80px; overflow: hidden; text-align: right;">
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
                            <td width="80px" align="left">
                                <asp:TextBox ID="txtKHBH" runat="server" class="tj_input" Width="80px"></asp:TextBox>
                            </td>
                            <td width="80px" align="right">
                                单位名称：
                            </td>
                            <td width="90px" align="left">
                                <asp:TextBox ID="txtDWMC" runat="server" class="tj_input" Width="90px"></asp:TextBox>
                            </td>
                              <td width="80px" align="right">
                                审核状态：
                            </td>
                            <td style="text-align: left; width: 80px;">
                                <asp:DropDownList ID="ddlZT" runat="server" CssClass="tj_input" Width="80px">
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem Selected="True">待审核</asp:ListItem>
                                    <asp:ListItem>审核通过</asp:ListItem>
                                    <asp:ListItem>作废</asp:ListItem>
                                </asp:DropDownList>
                            </td>              
                             <td align="left" width="270px" style ="padding-left:15px">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="50px" Text="查询"
                                    OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                已录入邮递信息发票列表（金额单位：元）
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
                                                发票单号
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                平台管理机构
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                客户编号
                                            </th>
                                            <th class="TheadTh" style="width: 140px;">
                                                单位名称
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                发票类型
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                发票金额
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                发票号码
                                            </th>
                                            <th class="TheadTh" style="width: 90px; line-height: 18px">
                                                邮递信息<br />
                                                录入日期
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                审核状态
                                            </th>
                                            <th class="TheadTh" style="width: 120px;">
                                                操作
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptKPXX" runat="server" OnItemDataBound="rptKPXX_ItemDataBound"
                                            OnItemCommand="rptKPXX_ItemCommand">
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td>
                                                        <%#Eval("发票单号")%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPTGLJG" runat="server" Text='<%#Eval("PTGLJG")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <%#Eval("KHBH")%>
                                                    </td>
                                                    <td title='<%#Eval("KHMC")%>'>
                                                        <asp:Label ID="lblDWMC" runat="server" Text='<%#Eval("KHMC")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <%#Eval("FPLX")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("FPJE")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("FPH")%>
                                                    </td>
                                                    <td>
                                                        <%#Convert.ToDateTime (Eval("Createtime")).ToString ("yyyy-MM-dd")%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSHZT" runat="server" Text='<%#Eval("SHZT")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lkbtnEdit" runat="server" CommandName="linkEdit" CommandArgument='<%#Eval("Number")%>'
                                                            Enabled='<%#Eval("SHZT").ToString()=="待审核"%>'>修改</asp:LinkButton>&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lkbtnCheck" runat="server" CommandName="linkbj" CommandArgument='<%#Eval("Number")%>'>审核处理</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                            <td colspan="10">
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
