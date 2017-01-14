<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LYBZJBZ.aspx.cs" Inherits="Web_JHJX_HTJK_LYBZJBZ" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-履约保证金不足卖家</title>
    <link href="../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../jquery-1.7.2.min.js" type="text/javascript"></script>    
    <script src="../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();          
        });

        function art_confirm_LinkButton_ViewItem(this_an) {
            if (!this_an.getAttribute("art_sp_s")) {
                this_an.setAttribute('art_sp_s', '0');
            }
            if (this_an.getAttribute("art_sp_s") == '0') {
                optsConfirm.renderID = this_an.id;
                optsConfirm.url = "JHJX/HTJK/LYBZJKFinfo.aspx?Number=" + this_an.getAttribute("aa");
                $("#" + this_an.id).art_confirm(optsConfirm);
                this_an.setAttribute('art_sp_s', '1');
                return false;
            }
            else {
                this_an.setAttribute('art_sp_s', '0');
                return false;
            }
        }

        var optsConfirm = {
            name: "diag1",
            width: 830,
            height: 500,
            title: "已扣罚履约保证金详情",
            url: "JHJX/HTJK/LYBZJKFinfo.aspx",
            showmessagerow: false,
            showbuttonrow: false,
            optionName: "optsConfirm",
            IsReady: function (object, dialog) {
                __artConfirmOperner.close();
            }
        }

    </script>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="LYJSTX_Buyer.aspx" Text="履约结束前买家提醒">
            </radTS:Tab>
            <radTS:Tab ID="Tab2" runat="server" NavigateUrl="LYJSTX_Saler.aspx" Text="履约结束前卖家提醒">
            </radTS:Tab>
            <radTS:Tab ID="Tab3" runat="server" NavigateUrl="LYBZJBZ.aspx" ForeColor="red" Text="履约保证金不足卖家">
            </radTS:Tab>
            <radTS:Tab ID="Tab4" runat="server" NavigateUrl="YCFHYJ.aspx" Text="延迟发货预警">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_bz">
                    监控条件：履约保证金小于等于原冻结金额&nbsp;&nbsp;<span id="spanTiaoJian" runat="server" style="font-weight: bold"></span>%&nbsp;&nbsp;的卖家予以汇总。
                </div>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                        <tr>
                            <td width="10%" align="right">
                                合同编号：
                            </td>
                            <td width="12%" align="left">
                                <asp:TextBox ID="txtHTBH" runat="server" class="tj_input" Width="120px"></asp:TextBox>
                            </td>
                            <td width="10%" align="right">
                                是否已处理：
                            </td>
                            <td width="8%" align="left">
                                <asp:DropDownList ID="ddlCLZT" runat="server" Width="82px" CssClass="tj_input" Height="22px">
                                    <asp:ListItem Value="">全部</asp:ListItem>
                                    <asp:ListItem>是</asp:ListItem>
                                    <asp:ListItem>否</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" width="2%" rowspan="2">
                                &nbsp;&nbsp;
                            </td>
                            <td align="left" width="58%">
                                <asp:Button ID="btnSearch" runat="server" CssClass="tj_bt" Width="80px" Text="查询"
                                    OnClick="btnSearch_Click" />&nbsp;&nbsp;<asp:Button ID="btnExport" runat="server"
                                        CssClass="tj_bt" Width="100px" Text="导出" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                &nbsp;&nbsp;履约保证金不足卖家信息
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td>
                                <div id="exprot" runat="server" style="width: 1110px;" class="content_nr_lb">
                                    <table id="theObjTable" style="width: 1350px;" cellspacing="0" cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style="width: 130px; text-align: center;">
                                                    操作
                                                </th>
                                                <th class="TheadTh" style="width: 60px; text-align: center; line-height: 18px">
                                                    是否<br />
                                                    已处理
                                                </th>
                                                <th class="TheadTh" style="width: 70px; text-align: center; line-height: 18px">
                                                    卖家<br />
                                                    冻结状态
                                                </th>
                                                <th class="TheadTh" style="width: 120px; text-align: center;">
                                                    卖家账号
                                                </th>
                                                <th class="TheadTh" style="width: 110px; text-align: center;">
                                                    卖家名称
                                                </th>
                                                <th class="TheadTh" style="width: 70px; text-align: center;">
                                                    联系人
                                                </th>
                                                <th class="TheadTh" style="width: 100px; text-align: center;">
                                                    联系方式
                                                </th>
                                                <th class="TheadTh" style="width: 90px; text-align: center;">
                                                    商品编号
                                                </th>
                                                <th class="TheadTh" style="width: 100px; text-align: center;">
                                                    商品名称
                                                </th>
                                                <th class="TheadTh" style="width: 100px; text-align: center;">
                                                    规格
                                                </th>
                                                <th class="TheadTh" style="width: 80px; text-align: center; line-height: 18px">
                                                    合同<br />
                                                    结束时间
                                                </th>
                                                <th class="TheadTh" style="width: 120px; text-align: center;">
                                                    合同编号
                                                </th>
                                                <th class="TheadTh" style="width: 100px; text-align: center; line-height: 18px">
                                                    履约保证金<br />
                                                    原金额
                                                </th>
                                                <th class="TheadTh" style="width: 100px; text-align: center; line-height: 18px">
                                                    已扣罚履约<br />
                                                    保证金金额
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td style="width: 130px;">
                                                            <asp:LinkButton
                                                    ID="linkView" runat="server" CommandName="linkView" aa='<%#Eval("合同编号") %>'  OnClientClick="return art_confirm_LinkButton_ViewItem(this);" CommandArgument='<%#Eval("合同编号")%>'  
                                                    >扣罚详情</asp:LinkButton>&nbsp;
                                                            <asp:LinkButton ID="btnCL" runat="server" CommandName="btnCL" CommandArgument='<%#Eval("合同编号")%>'
                                                                OnClientClick="javascript:return confirm('确定要标记该数据为已处理吗？');" Enabled='<%#Eval("是否已处理").ToString()=="否" %>'
                                                                Text="通知处理"></asp:LinkButton>
                                                        </td>
                                                        <td align="center" style="width: 60px; word-wrap: break-word;">
                                                            <div style="width: 60px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("是否已处理")%></div>
                                                        </td>
                                                        <td align="center" style="width: 70px; word-wrap: break-word;">
                                                            <div style="width: 70px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("卖家冻结状态")%></div>
                                                        </td>
                                                        <td align="center" style="width: 120px; word-wrap: break-word;">
                                                            <div style="width: 120px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("卖家账号")%></div>
                                                        </td>
                                                        <td align="center" style="width: 110px; word-wrap: break-word;">
                                                            <div style="width: 110px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("卖家名称")%></div>
                                                        </td>
                                                        <td align="center" style="width: 70px; word-wrap: break-word;">
                                                            <div style="width: 70px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("卖家联系人")%></div>
                                                        </td>
                                                        <td align="center" style="width: 100px; word-wrap: break-word;">
                                                            <div style="width: 100px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("卖家联系方式")%></div>
                                                        </td>
                                                        <td align="center" style="width: 90px; word-wrap: break-word;">
                                                            <div style="width: 90px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("商品编号")%></div>
                                                        </td>
                                                        <td align="center" style="width: 100px; word-wrap: break-word;">
                                                            <div style="width: 100px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("商品名称")%></div>
                                                        </td>
                                                        <td align="center" style="width: 100px; word-wrap: break-word;">
                                                            <div style="width: 100px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("规格")%></div>
                                                        </td>
                                                        <td align="center" style="width: 80px; word-wrap: break-word;">
                                                            <div style="width: 80px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("合同结束时间")%></div>
                                                        </td>
                                                        <td align="center" style="width: 120px; word-wrap: break-word;">
                                                            <div style="width: 120px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("合同编号")%></div>
                                                        </td>
                                                        <td align="center" style="width: 100px; word-wrap: break-word;">
                                                            <div style="width: 100px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("履约保证金原金额")%></div>
                                                        </td>
                                                        <td align="center" style="width: 100px; word-wrap: break-word;">
                                                            <div style="width: 100px; word-wrap: break-word; line-height: 18px">
                                                                <%#Eval("已扣罚履约保证金")%></div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                                <td colspan="10">
                                                    暂无满足条件的数据！
                                                </td>
                                                <td colspan="4">
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <uc1:commonpagernew ID="commonpagernew1" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
