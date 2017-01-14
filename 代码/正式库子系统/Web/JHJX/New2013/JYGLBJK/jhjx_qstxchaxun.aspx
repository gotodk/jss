<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjx_qstxchaxun.aspx.cs" Inherits="Web_JHJX_New2013_JYGLBJK_jhjx_qstxchaxun" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" />
    <script src="../../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
    <style type="text/css">
        #content_zw {
            width: 1110px;
        }
    </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="99%">
            <Tabs>


                <radTS:Tab ID="Tab3" runat="server" NavigateUrl="jhjx_qstxchaxun.aspx"
                    Text="卖家提醒买家签收" ForeColor="Red">
                </radTS:Tab>


            </Tabs>
        </radTS:RadTabStrip>
        <div id="new_content">
            <div id="new_zicontent">
                <div id="content_zw">

                    <div class="content_nr">
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                            <tr>
                                <td width="80px" align="right">发货单号：
                                </td>
                                <td width="120px" align="left">
                                    <asp:TextBox ID="txt_fhdh" runat="server" class="tj_input" Width="110px"></asp:TextBox>
                                </td>

                                <td width="90px" align="right">提醒时间：</td>
                                <td style="text-align: left; width: 180px;">
                                    <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                        ID="txtBeginTime" runat="server" Width="178px"></asp:TextBox>
                                </td>
                                <td style="text-align: right; width: 20px;" align="center">至：</td>
                                <td width="180px" align="left">
                                    <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                        ID="txtEndTime" runat="server" Width="178px"></asp:TextBox>
                                </td>
                                <td width="80px" align="right">交易方名称：</td>
                                <td align="left" colspan=" 1" style="width: 120px">
                                    <asp:TextBox ID="txt_jyfmc" runat="server" class="tj_input" Width="110px"></asp:TextBox>
                                </td>
                                <td colspan="2">&nbsp;</td>


                            </tr>
                            <tr>
                                <td colspan="8">
                                    <table>
                                        <tr>
                                            <td align="right" valign="middle" style="width: 80px">签收状态：</td>
                                            <td style="width: 120px">
                                                <asp:DropDownList ID="ddl_qszt" runat="server" CssClass="tj_input" Width="110px">
                                                    <asp:ListItem>全部</asp:ListItem>
                                                    <asp:ListItem>未签收</asp:ListItem>
                                                    <asp:ListItem>正常签收</asp:ListItem>
                                                    <asp:ListItem>异常签收</asp:ListItem>
                                                    <asp:ListItem>默认签收</asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td width="90px" align="right">是否已处理：</td>
                                            <td>
                                                <asp:DropDownList ID="ddl_sfycl" runat="server" CssClass="tj_input" Width="60px">
                                                    <asp:ListItem>全部</asp:ListItem>
                                                    <asp:ListItem>是</asp:ListItem>
                                                    <asp:ListItem>否</asp:ListItem>

                                                </asp:DropDownList></td>
                                            <td>

                                                <asp:Button ID="Btnsure" runat="server" CssClass="tj_bt" Width="40px" Text="查询"
                                                    OnClick="btnSearch_Click" />

                                                <asp:Button ID="btnexport" runat="server" CssClass="tj_bt" Width="40px" Text="导出" OnClick="btnexport_Click" />

                                            </td>

                                        </tr>

                                    </table>
                                </td>


                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td>卖家提醒买家签收列表(金额单位：元)</td>
                            </tr>
                        </table>

                        <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse; table-layout: fixed; width: 100%;"
                            class="tab">
                            <tr>
                                <td>
                                    <div style="overflow-x: scroll;width:1110px">
                                        <table id="theObjTable" style="width: 2040px;" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr>
                                                    <th class="TheadTh" style="width: 120px;">合同编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">发货单号
                                                    </th>
                                                    <th class="TheadTh" style="width: 150px; line-height: 18px;">提醒签收时间   
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">买家账号
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px;">买家名称
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px;">买家联系人
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">买家联系方式
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">卖家账号
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px;">卖家名称
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px;">卖家联系人
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">卖家联系方式
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">商品编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">商品名称
                                                    </th>

                                                    <th class="TheadTh" style="width: 100px;">发货数量
                                                    </th>
                                                    <th class="TheadTh" style="width: 150px;">发货金额
                                                    </th>
                                                    <th class="TheadTh" style="width: 80px;">签收状态
                                                    </th>
                                                    <th class="TheadTh" style="width: 80px;">是否已处理
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">卖家反馈签收单
                                                    </th>
                                                    <th class="TheadTh" style="width: 80px;">处理
                                                    </th>




                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr class="TbodyTr">
                                                            <td style="width: 120px;">
                                                                <%#Eval("合同编号")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%--   <%#Eval("发货单号")%>--%>
                                                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("发货单号")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 150px;">
                                                                <%#Eval("提醒签收时间")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("买家账号")%> 
                                                            </td>
                                                            <td style="width: 120px;">

                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("买家名称").ToString().Length > 8 ? Eval("买家名称").ToString().Substring(0, 8) + "..." : Eval("买家名称").ToString()%>' ToolTip='<%#Eval("买家名称")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("买家联系人").ToString().Length > 8 ? Eval("买家联系人").ToString().Substring(0, 8) + "..." : Eval("买家联系人").ToString()%>' ToolTip='<%#Eval("买家联系人")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("买家联系方式")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("卖家账号")%> 
                                                            </td>
                                                            <td style="width: 120px;">

                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("卖家名称").ToString().Length > 8 ? Eval("卖家名称").ToString().Substring(0, 8) + "..." : Eval("卖家名称").ToString()%>' ToolTip='<%#Eval("卖家名称")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("卖家联系人").ToString().Length > 8 ? Eval("卖家联系人").ToString().Substring(0, 8) + "..." : Eval("卖家联系人").ToString()%>' ToolTip='<%#Eval("卖家联系人")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("卖家联系方式")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("商品编号")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("商品名称")%>
                                                            </td>

                                                            <td style="width: 100px;">
                                                                <%#Eval("发货数量")%>
                                                     
                                                            </td>
                                                            <td style="width: 150px;">
                                                                <%#Eval("发货金额")%>
                                                            </td>
                                                            <td style="width: 80px;">
                                                                <%#Eval("签收状态")%>
                                                            </td>
                                                            <td style="width: 80px;">
                                                                <%#Eval("是否已处理")%> 
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("卖家反馈签收单")%>
                                                            </td>
                                                            <td style="width: 80px;">
                                                                <asp:LinkButton ID="lkbtnCheck" runat="server" CommandName="linkbj" CommandArgument='<%#Eval("发货单号")%>' Text="通知处理" Enabled=' <%#Eval("是否已处理").ToString().Equals("否")%> '></asp:LinkButton>
                                                            </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                                    <td colspan="19">您查询的数据为空！
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <uc1:commonpagernew ID="commonpagernew1" runat="server" />
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
