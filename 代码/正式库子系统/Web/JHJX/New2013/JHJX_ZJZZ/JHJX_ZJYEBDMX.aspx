<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JHJX_ZJYEBDMX.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_JHJX_ZJZZ_JHJX_ZJYEBDMX" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>

<%@ Register Src="../UCFWJG/UCFWJGDetail.ascx" TagName="UCFWJGDetail" TagPrefix="uc2" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台审核代理人功能</title>
    <script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
    <%-- <style type="text/css">
        #content_zw
        {
            width: 954px;
        }
    </style>--%>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
            BackColor="#F7F7F7">
            <Tabs>
                <radTS:Tab ID="Tab1" runat="server" ForeColor="Red" NavigateUrl="JHJX_ZJYEBDMX.aspx"
                    Text="资金余额变动明细">
                </radTS:Tab>
                <radTS:Tab ID="Tab2" runat="server" NavigateUrl="JHJX_WYPCFSJL.aspx"
                    Text="违约赔偿发生记录">
                </radTS:Tab>
                <radTS:Tab ID="Tab3" runat="server" NavigateUrl="JHJX_BCSYFSJL.aspx"
                    Text="补偿收益发生记录">
                </radTS:Tab>
                <radTS:Tab ID="Tab4" runat="server" NavigateUrl="JHJX_JJRSYFSJL.aspx"
                    Text="经纪人收益发生记录">
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
                            <td align="right" width="80px">
                                变动时间：
                            </td>
                            <td align="left">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtBeginTime" runat="server" Width="178px"></asp:TextBox>
                            </td>
                            <td align="right" width="50px">
                                至：
                            </td>
                            <td align="left">
                                <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                    ID="txtEndTime" runat="server" Width="178px"></asp:TextBox>
                            </td>
                             <td  colspan="6"  style="padding-left:10px">
                             
                                 <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                             
                            </td>
                           
                        </tr>
                       
                           <tr>
                            <td  align="right">
                                项目：
                            </td>
                            <td width="160px">
                                <asp:DropDownList ID="ddXM" runat="server" Height="25px" Width="178px" CssClass="tj_input" AutoPostBack="true" OnSelectedIndexChanged="ddXM_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td width="50px" align="right">
                              性质：</td>
                            <td style="width: 178px">
                                <asp:DropDownList ID="ddXZ" runat="server" Height="25px" Width="178px" CssClass="tj_input" AutoPostBack="true" OnSelectedIndexChanged="ddXZ_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                             <td  style="padding-left:10px;width:85px">
                               交易方账号：</td>
                            <td width="170px">
                                 <asp:TextBox ID="txtJYFZH" runat="server" Width="170px" CssClass="tj_input"></asp:TextBox>
                            </td>
                            <td style="padding-left:10px;width:85px">
                                交易方名称：</td>
                            <td width="170px">
                                <asp:TextBox ID="txtJYFMC" runat="server" Width="170px" CssClass="tj_input"></asp:TextBox>
                            </td>
                          
                            <td  valign="middle" style="width:130px;" >
                                 <table  width="100%" cellpadding="0" cellspacing="0">
                                    <tr><td align="right"> <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" 
                                    Text="查询"  Width="50px" OnClick="btnSearch_Click" /></td>
                                    <td  style="padding-left:10px"><asp:Button ID="btnToExcel" runat="server" CssClass="tj_bt" 
                                    Text="导出"  Width="50px" OnClick="btnToExcel_Click"  /></td>
                                </table></td>
                        </tr>                          
                            
                          
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td>资金余额变动明细
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                            style="border-collapse: collapse;" class="tab">
                            <tr>
                                <td>
                                    <table id="theObjTable" style="width: 100%;" cellspacing="0" cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style="width: 120px; word-wrap: break-word;">变动时间
                                                </th>
                                                <th class="TheadTh" style="width: 100px; word-wrap: break-word;">交易方名称
                                                </th>
                                                <th class="TheadTh" style="width: 120px; word-wrap: break-word;">交易方账号
                                                </th>
                                                <th class="TheadTh" style="width: 100px; word-wrap: break-word;">分公司名称
                                                </th>
                                                <th class="TheadTh" style="width: 280px; word-wrap: break-word;">摘要
                                                </th>
                                                <th class="TheadTh" style="width: 120px; word-wrap: break-word;">项目
                                                </th>
                                                <th class="TheadTh" style="width: 150px; word-wrap: break-word;">性质
                                                </th>
                                                <th class="TheadTh" style="width: 90px; word-wrap: break-word;">金额
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptZJYEBDMX" runat="server">
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td style="width: 120px; word-wrap: break-word; line-height: 18px">
                                                            <div style="width: 120px; word-wrap: break-word; line-height: 18px"><%#Eval("变动时间")%></div>
                                                        </td>
                                                        <td style="width: 100px; word-wrap: break-word; line-height: 18px">

                                                            <div style="width: 100px; word-wrap: break-word; line-height: 18px"><%#Eval("交易方名称")%></div>
                                                        </td>
                                                        <td style="width: 120px; word-wrap: break-word; line-height: 18px">

                                                            <div style="width: 120px; word-wrap: break-word; line-height: 18px"><%#Eval("交易方账号")%></div>
                                                        </td>
                                                        <td style="width: 100px; word-wrap: break-word; line-height: 18px">

                                                            <div style="width: 100px; word-wrap: break-word; line-height: 18px"><%#Eval("分公司名称")%></div>
                                                        </td>
                                                        <td style="width: 280px; word-wrap: break-word; line-height: 18px">
                                                            <div style="width: 280px; word-wrap: break-word; line-height: 18px;"><%#Eval("摘要")%></div>
                                                        </td>
                                                        <td style="width: 120px; word-wrap: break-word; line-height: 18px">

                                                            <div style="width: 120px; word-wrap: break-word; line-height: 18px"><%#Eval("项目")%></div>
                                                        </td>
                                                        <td style="width: 150px; word-wrap: break-word; line-height: 18px">

                                                            <div style="width: 150px; word-wrap: break-word; line-height: 18px"><%#Eval("性质")%></div>
                                                        </td>
                                                        <td style="width: 90px; word-wrap: break-word; line-height: 18px">

                                                            <div style="width: 90px; word-wrap: break-word; line-height: 18px"><%#Eval("金额")%></div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                                <td colspan="8">您查询的数据为空！
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
