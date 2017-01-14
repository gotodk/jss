<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JYPT_SHJYJL_SPMS.aspx.cs"
    Inherits="mywork_JYPT_JYPT_SHJYJL_SPMS" %>

<%@ Register Src="../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>中国商品批发交易平台-验收货经验交流-商品俗称</title>
    <link href="dh.css" rel="stylesheet" type="text/css" />
    <script type="text/javaScript">
        function yz() {
            if (document.getElementById("txtSPXMC").value == "") {
                alert("请输入您的建议，然后提交！");
                return false;
            }
        }
        function textCounter(field, counter, maxlimit, linecounter) {
            var fieldWidth = parseInt(field.offsetWidth);
            var charcnt = field.value.length;
            if (charcnt > maxlimit) {
                field.value = field.value.substring(0, maxlimit);
            }
            else {
                document.getElementById(counter).innerHTML = "已输入：<span style='color:#F63'>" + charcnt + "</span>字&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;上限：<span style='color:#F63'>" + maxlimit + "</span>字&nbsp;&nbsp;&nbsp;&nbsp;"
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 814px">
            <table width="814px" align="center" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="left">
                        <div class="tab1">
                            <ul id="test1_li_now">
                                <li><a id="aSPSC" runat="server">商品俗称</a></li>
                                <li class="now"><a id="aSPMS" runat="server">商品描述</a></li>
                                <li><a id="aJYBZ" runat="server">验收方法</a></li>
                                <li><a id="aZLBZ" runat="server">质量标准</a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <table align="center" cellpadding="0" cellspacing="0" class="table_nr">
                <tr>
                    <td align="left" height="25px">
                        <span class="title ">平台现用商品描述：</span> <span id="spanXYNR" runat="server" style="line-height: 20px"></span>
                    </td>
                </tr>
                <%--  <tr>
                <td align="left" style="padding-left: 10px">
                   
                </td>
            </tr>--%>
                <tr>
                    <td align="left" style="height: 25px" valign="bottom" class="title">用户建议商品描述
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="0" border="1px" bordercolor="#D1CFCF" style="border-style: solid;
                        border-collapse: collapse; text-align: center; width: 786px;">
                            <thead>
                                <tr style="background-color: #D6E3F3; font-weight: bold;">
                                    <td height="28" width="330px">建议商品描述
                                    </td>
                                    <td height="28" width="170px">提供者
                                    </td>
                                    <td height="28" width="100px">支持率
                                    </td>
                                    <td height="28" width="70px">操作
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound" OnItemCommand="rpt_ItemCommand">
                                    <ItemTemplate>
                                        <tr style="height: 28px;">
                                            <td width="330px" title='<%#Eval("建议内容") %>'>
                                                <asp:Label ID="lbljynr" runat="server" Text='<%#Eval("建议内容")%>'> </asp:Label>
                                            </td>
                                            <td width="170px" title='<%#Eval("建议者")%>'>
                                                <asp:Label ID="lblJYZ" runat="server" Text='<%#Eval("建议者")%>'> </asp:Label>
                                            </td>
                                            <td width="100x" title='<%#Eval("支持率")%>'>
                                                <asp:Label ID="lblZCL" runat="server" Text='<%#Eval("支持率")%>'> </asp:Label>
                                            </td>
                                            <td width="70px">
                                                <asp:LinkButton ID="btnZhiChi" runat="server" CommandName="ZhiChi" CommandArgument='<%#Eval("Number")+","+Eval("建议者邮箱") %>'
                                                    Text="支持" CssClass="linkb"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                            <tfoot id="tempty" runat="server">
                                <tr>
                                    <td height="28" colspan="4">
                                        <asp:Label ID="Label1" runat="server" Text="暂时没有此商品的建议商品描述，您可以提供您的建议。"></asp:Label>
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc2:commonpager ID="commonpager1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td height="10px"></td>
                </tr>
                <tr>
                    <td align="left" style="height: 22px" class="title ">我来提供新的商品描述
                    </td>
                </tr>
                <tr runat="server" id="trTJ">
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td align="left" valign="bottom">
                                    <asp:TextBox ID="txtSPXMC" runat="server" Width="696px" Height="100px" TextMode="MultiLine"
                                        CssClass="tj_input_M" onKeyUp="textCounter(this,'divBZ',300)"></asp:TextBox>
                                </td>
                                <td align="right" valign="bottom" width="90px">
                                    <asp:Button ID="btnTJ" runat="server" CssClass="tj_bt_da" Text="提交建议" OnClick="btnTJ_Click"
                                        OnClientClick="return yz();" Height="25px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <div id="divBZ">
                                    </div>
                                    <script type="text/javascript">                                    textCounter(document.getElementById("txtSPXMC"), "divBZ", 300)</script>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trCK">
                    <td style="padding-left: 30px; word-break:break-all ;word-wrap:break-word" align="left" height="30px" >
                        <asp:Label ID="lblCK" runat="server" Text="" Style="line-height: 20px; word-break:break-all; word-wrap:break-word"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <input type="hidden" runat="server" id="hidSPBH" />
        <input type="hidden" runat="server" id="hidYHM" />
        <input type="hidden" runat="server" id="hidDLYX" />
        <input type="hidden" runat="server" id="hidCan" />
    </form>
</body>
</html>
