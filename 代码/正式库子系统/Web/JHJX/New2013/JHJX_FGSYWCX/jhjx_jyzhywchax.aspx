<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjx_jyzhywchax.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_FGSYWCX_jhjx_jyzhywchax" %>

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
            width: 1100px;
        }
    </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
            BackColor="#F7F7F7">
            <Tabs>
                <radTS:Tab ID="Tab2" runat="server" NavigateUrl="jhjx_FGSYWDataTongji.aspx"
                    Text=" 分公司业务数据统计">
                </radTS:Tab>
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="jhjx_jyzhywchax.aspx" ForeColor="Red"
                    Text="分公司所属的交易账户业务查看">
                </radTS:Tab>


            </Tabs>
        </radTS:RadTabStrip>
        <div id="new_content">
            <div id="new_zicontent">
                <div id="content_zw">

                    <div class="content_nr">
                        <table width="1100px" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                            <tr>
                                <td width="100px" align="right">商品名称：
                                </td>
                                <td width="120px" align="left">
                                    <asp:TextBox ID="txt_spmc" runat="server" class="tj_input" Width="110px"></asp:TextBox>
                                </td>

                                <td width="75px" align="right">交易类型：</td>
                                <td style="text-align: left; width: 120px;">
                                    <asp:DropDownList ID="ddl_djlb" runat="server" CssClass="tj_input" Width="110px">
                                        <asp:ListItem>选择交易类型</asp:ListItem>
                                        <asp:ListItem>买入</asp:ListItem>
                                        <asp:ListItem>卖出</asp:ListItem>

                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right; width: 90px;" align="right">交易状态：</td>
                                <td width="120px" align="left">
                                    <asp:DropDownList ID="ddl_jyzt" runat="server" CssClass="tj_input" Width="110px">
                                        <asp:ListItem>选择交易状态</asp:ListItem>
                                        <asp:ListItem>竞标中</asp:ListItem>
                                        <asp:ListItem>冷静期</asp:ListItem>
                                        <asp:ListItem>中标</asp:ListItem>
                                        <asp:ListItem>定标</asp:ListItem>
                                        <asp:ListItem>废标</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="80px" align="right">所属分公司：</td>
                                <td align="left" colspan=" 3">

                                    <asp:DropDownList ID="ddlssfgs" runat="server" CssClass="tj_input" Width="120px">
                                    </asp:DropDownList>

                                </td>



                            </tr>
                            <tr>

                                <td width="100px" align="right">账户名称：</td>
                                <td width="120px" align="left">
                                    <asp:TextBox ID="txtjjrbh" runat="server" class="tj_input" Width="110px"></asp:TextBox>
                                </td>


                                <td style="text-align: right; width: 75px;">登陆邮箱：</td>
                                <td style="text-align: left; width: 120px;">
                                    <asp:TextBox ID="txtjjrxm" runat="server" class="tj_input" Width="110px" Height="23"></asp:TextBox>
                                </td>
                                <td style="text-align: right; width: 90px;" align="right">

                                    <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="40px" Text="查询"
                                        OnClick="btnSearch_Click" />
                                    <asp:Button ID="Button1" runat="server" CssClass="tj_bt" Width="40px" Text="导出"
                                        OnClick="Button1_Click" />

                                </td>
                                <td width="120px" align="left">&nbsp;</td>
                                <td style="text-align: left; ">&nbsp;&nbsp;
                                                
                                </td>
                                <td></td>



                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td>分公司所属交易账户业务查看列表（金额：元）</td>
                            </tr>
                        </table>

                        <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse; table-layout: fixed; width: 100%;"
                            class="tab">
                            <tr>
                                <td>
                                    <div style="overflow-x: scroll; width:1100px">
                                        <table id="theObjTable" style="width: 1680px;" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr>
                                                    <th class="TheadTh" style="width: 100px;">所属分公司
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">交易方账号
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px;">交易方名称
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">账户类型
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px;">商品名称
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">规格
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">合同期限
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">交易类型
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">交易状态
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">数量
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">金额
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">是否拆单
                                                    </th>
                                                    <th class="TheadTh" style="width: 150px;">发生时间
                                                    </th>
                                                    <th class="TheadTh" style="width: 150px;">经纪人收益
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptSPXX" runat="server">
                                                    <ItemTemplate>
                                                        <tr class="TbodyTr">
                                                            <td style="width: 100px;">
                                                                <%#Eval("所属分公司")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <asp:Label ID="lbbuyer" runat="server" Text='<%#Eval("交易方账号")%>'> </asp:Label>
                                                            </td>
                                                            <td style="width: 120px;">
                                                               <%-- <asp:Label ID="lbseller" runat="server" Text='<%#Eval("交易方名称")%>'> </asp:Label>--%>
                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("交易方名称").ToString().Length > 8 ? Eval("交易方名称").ToString().Substring(0, 8) + "..." : Eval("交易方名称").ToString()%>' ToolTip='<%#Eval("交易方名称")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("账户类型")%>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <%#Eval("商品名称")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("规格")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("合同期限")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("交易类型")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("交易状态")%>
                                                            </td>
                                                            <td title='<%#Eval("数量")%>' style="width: 100px;">
                                                                <asp:Label ID="lbspmc" runat="server" Text='<%#Eval("数量")%>'> </asp:Label>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("金额")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("是否拆单")%>
                                                            </td>
                                                            <td style="width: 150px;">
                                                                <%#Eval("发生时间")%>
                                                            </td>
                                                            <td style="width: 150px;">
                                                                <%#Eval("经纪人收益")%>
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

