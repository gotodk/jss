<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjx_spmmgk.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_SPMM_jhjx_spmmgk" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>
<%@ Register Src="../UCFWJG/UCFWJGDetail.ascx" TagName="UCFWJGDetail" TagPrefix="uc2" %>
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
            width: 1130px;
        }
    </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
            BackColor="#F7F7F7">
            <Tabs>
                <radTS:Tab ID="Tab2" runat="server" NavigateUrl="jhjx_spmmgk.aspx" ForeColor="red"
                    Text=" 商品买卖概况">
                </radTS:Tab>
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="jhjx_spmmjb.aspx"
                    Text="竞标中">
                </radTS:Tab>
                <radTS:Tab ID="Tab3" runat="server" NavigateUrl="jhjx_spmmljq.aspx"
                    Text="冷静期">
                </radTS:Tab>
                <radTS:Tab ID="Tab4" runat="server" NavigateUrl="jhjx_spmmzb.aspx"
                    Text="中标">
                </radTS:Tab>
                <radTS:Tab ID="Tab5" runat="server" NavigateUrl="jhjx_spmmdbybzh.aspx"
                    Text="定标与保证函">
                </radTS:Tab>
                <radTS:Tab ID="Tab6" runat="server" NavigateUrl="JHJX_FB.aspx" Text="废标"
                    Font-Size="12px">
                </radTS:Tab>
                <radTS:Tab ID="Tab7" runat="server" NavigateUrl="JHJX_QP.aspx" Text="清盘"
                    Font-Size="12px">
                </radTS:Tab>
                <radTS:Tab ID="Tab8" runat="server" NavigateUrl="JHJX_CGX.aspx" Text="草稿箱"
                    Font-Size="12px">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <div id="new_content">
            <div id="new_zicontent">
                <div id="content_zw">

                    <div class="content_nr">
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                            <tr>
                                <td width="90px" align="right">商品名称：
                                </td>
                                <td width="120px" align="left">
                                    <asp:TextBox ID="txt_spmc" runat="server" class="tj_input" Width="110px"></asp:TextBox>
                                </td>

                                <td width="75px" align="right">单据类别：</td>
                                <td style="text-align: left; width: 120px;">
                                    <asp:DropDownList ID="ddl_djlb" runat="server" CssClass="tj_input" Width="110px">
                                        <asp:ListItem>选择单据类别</asp:ListItem>
                                        <asp:ListItem>预订单</asp:ListItem>
                                        <asp:ListItem>投标单</asp:ListItem>

                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right; width: 90px;" align="right">交易方名称：</td>
                                <td width="120px" align="left">
                                    <asp:TextBox ID="txtjjrbh" runat="server" class="tj_input" Width="110px"></asp:TextBox>
                                </td>

                                <td width="550px" align="left">
                                    <uc2:UCFWJGDetail ID="UCFWJGDetail1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <table width="100%">
                                        <tr>
                                            <td width="85px" align="right">&nbsp;交易方账号：</td>
                                            <td width="120px" align="left">

                                                <asp:TextBox ID="txtjjrxm" runat="server" class="tj_input" Width="110px" Height="23"></asp:TextBox>

                                            </td>

                                            <td width="85px" align="left">
                                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="40px" Text="查询"
                                                    OnClick="btnSearch_Click" />
                                                <asp:Button ID="Button1" runat="server" CssClass="tj_bt" Width="40px" Text="导出"
                                                    OnClick="Button1_Click" />
                                            </td>
                                            <td style="text-align: left; width: 120px;">&nbsp;</td>
                                            <td style="text-align: right; width: 90px;" align="right">&nbsp;</td>
                                            <td width="70px" align="left">&nbsp;</td>
                                            <td width="100px" align="right">&nbsp;</td>
                                            <td align="left" colspan="1" width="115px">&nbsp;</td>
                                            <td width="100px" align="right">&nbsp;</td>
                                            <td width="115px" align="left">&nbsp;</td>
                                            <td width="120px" align="left">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>


                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td>商品买卖概况列表（金额单位：元）</td>
                            </tr>
                        </table>

                        <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse; table-layout: fixed; width: 100%;"
                            class="tab">
                            <tr>
                                <td>
                                    <div style="overflow-x: scroll;">
                                        <table id="theObjTable" style="width: 1640px;" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr>
                                                    <th class="TheadTh" style="width: 100px;">业务管理部门
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">交易方账号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">交易方名称
                                                    </th>
                                                    <th class="TheadTh" style="width: 150px;">下单时间
                                                    </th>
                                                    <th class="TheadTh" style="width: 80px;">商品编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">商品名称
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">规格
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">合同期限
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">数量
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">价格
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">金额
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">当前卖家<br />
                                                        最低价
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">最低价标的<br />
                                                        经济批量
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px; line-height: 18px;">最低价标的<br />
                                                        达成率/中标率%
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">单据类型
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">单据编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">当前状态
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">是否拆单
                                                    </th>

                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptSPXX" runat="server">
                                                    <ItemTemplate>
                                                        <tr class="TbodyTr">
                                                            <td style="width: 100px;">
                                                                <%#Eval("业务管理部门")%>
                                                            </td>
                                                            <td title='<%#Eval("交易方账号")%>' style="width: 100px;">
                                                                <asp:Label ID="lbbuyer" runat="server" Text='<%#Eval("交易方账号")%>'> </asp:Label>
                                                            </td>
                                                            <td title='<%#Eval("交易方名称")%>' style="width: 100px;">
                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("交易方名称").ToString().Length > 8 ? Eval("交易方名称").ToString().Substring(0, 8) + "..." : Eval("交易方名称").ToString()%>' ToolTip='<%#Eval("交易方名称")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 150px;">
                                                                <%#Eval("下单时间")%>
                                                            </td>
                                                            <td style="width: 80px;">
                                                                <%#Eval("商品编号")%>
                                                            </td>
                                                            <td title='<%#Eval("商品名称")%>' style="width: 100px;">
                                                                <%-- <asp:Label ID="lbspmc" runat="server" Text='<%#Eval("商品名称")%>'> </asp:Label>--%>

                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("商品名称").ToString().Length > 8 ? Eval("商品名称").ToString().Substring(0, 8) + "..." : Eval("商品名称").ToString()%>' ToolTip='<%#Eval("商品名称")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                 <asp:Label ID="lblGG" runat="server" Text='<%#Eval("规格").ToString().Length > 8 ? Eval("规格").ToString().Substring(0, 8) + "..." : Eval("规格").ToString()%>' ToolTip='<%#Eval("规格")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("合同期限")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("数量")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("价格")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("金额")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("当前卖家最低价")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("最低价标的经济批量")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("最低价标的达成率")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("单据类型")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("单据编号")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("当前状态")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("是否拆单")%>
                                                            </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                                    <td colspan="7">您查询的数据为空！
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
