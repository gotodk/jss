<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JYPT_SHJYJL_YSBZ.aspx.cs"
    Inherits="mywork_JYPT_JYPT_SHJYJL_YSBZ" %>

<%@ Register Src="../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>中国商品批发交易平台-验收货经验交流-验收方法</title>
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
    <div style="width: 700px">
        <table width="700px" align="center" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left">
                    <div class="tab1">
                        <ul id="test1_li_now">
                            <li><a id="aSPSC" runat="server">商品俗称</a></li>
                            <li><a id="aSPMS" runat="server">商品描述</a></li>
                            <li class="now"><a id="aJYBZ" runat="server">验收方法</a></li>
                            <li><a id="aZLBZ" runat="server">质量标准</a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
        <table align="center" cellpadding="0" cellspacing="0" class="table_nr">
            <%-- <tr>
                <td align="left" height="30px" class="title ">
                    平台现用商品描述：
                </td>
            </tr>
            <tr>
                <td align="left" style="padding-left: 10px">
                    <span id="spanXYNR" runat="server" style="line-height: 20px"></span>
                </td>
            </tr>--%>
            <tr>
                <td align="left" style="height: 25px" valign="bottom" class="title">
                    用户建议验收方法
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" border="1px" bordercolor="#D1CFCF" style="border-style: solid;
                        border-collapse: collapse; text-align: center; width: 670px;">
                        <thead>
                            <tr style="background-color: #D6E3F3; font-weight: bold;">
                                <td height="28" width="330px">
                                    建议验收方法
                                </td>
                                <td height="28" width="170px">
                                    提供者
                                </td>
                                <td height="28" width="100px">
                                    支持率
                                </td>
                                <td height="28" width="70px">
                                    操作
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound" OnItemCommand="rpt_ItemCommand">
                                <ItemTemplate>
                                    <tr style="height: 28px;">
                                        <td width="330px" title='<%#Eval("建议内容") %>'>
                                        <asp:LinkButton ID="linkbCKNR" runat="server" CommandName="ViewXQ" CommandArgument='<%#Eval("建议者")+"|"+Eval("建议内容") %>'
                                                 CssClass="linkb"><asp:Label ID="lbljynr" runat="server" Text='<%#Eval("建议内容")%>'> </asp:Label></asp:LinkButton>     
                                        </td>
                                        <td width="170px" title='<%#Eval("建议者")%>'>
                                            <asp:Label ID="lblJYZ" runat="server" Text='<%#Eval("建议者")%>'> </asp:Label>
                                        </td>
                                        <td width="100x" title='<%#Eval("支持率")%>'>
                                            <asp:Label ID="lblZCL" runat="server" Text='<%#Eval("支持率")%>'> </asp:Label>
                                        </td>
                                        <td width="70px">
                                            <asp:LinkButton ID="btnZhiChi" runat="server" CommandName="ZhiChi" CommandArgument='<%#Eval("Number") %>'
                                                Text="支持" CssClass="linkb"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                        <tfoot id="tempty" runat="server">
                            <tr>
                                <td height="28" colspan="4">
                                    <asp:Label ID="Label1" runat="server" Text="暂时没有此商品的建议验收方法，您可以提供您的建议。"></asp:Label>
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
                <td height="10px">
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 22px" class="title ">
                    我来提供新的验收方法
                </td>
            </tr>
            <tr runat="server" id="trTJ">
                <td>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="left" valign="bottom">
                                <asp:TextBox ID="txtSPXMC" runat="server" Width="580px" Height="130px" TextMode="MultiLine"
                                    CssClass="tj_input_M" onKeyUp="textCounter(this,'divBZ',3000)"></asp:TextBox>
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
                                <script type="text/javascript">                                    textCounter(document.getElementById("txtSPXMC"), "divBZ", 3000)</script>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="trCK">
                <td style="padding-left: 30px" align="left" height="30px">
                    <div runat="server" id="divCK" style="width: 100%; height: 100px; overflow-y: auto;">
                        <span id="spanCK" runat="server" style="line-height: 20px;"></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" runat="server" id="hidSPBH" />
    <input type="hidden" runat="server" id="hidYHM" />
    <input type="hidden" runat="server" id="hidDLYX" />
    <input type="hidden" runat="server" id="hidCan" />
    <div id="divCKXQ" style="display: block; left: 80px; top: 50px; width: 500px; background-color: #ffffff;
        position: absolute; z-index: 999; border: solid 5px #D6E3F3;" visible="false"
        runat="server">
        <table width="100%" cellpadding="0" cellspacing="0" id="tdhandle">
            <tr style="font-weight: bold; background-color: #D6E3F3; height: 25px">
                <td align="left" style="padding-left: 10px">
                    建议验收方法详情
                </td>
                <td align="right">
                    <asp:ImageButton ID="ImageButton1" runat="server" Height="15px" ImageUrl="chahao.png"
                        OnClick="ImageButton1_Click" Width="15px" />
                </td>
            </tr>           
        </table>
        <table width="100%" align="left" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left" height="30px" style="padding: 10px">
                    <span class="title">提供者：</span><span id="spanTGR" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td align="left" height="30px" style="padding-left: 10px">
                    <div id="divJYXQ" runat="server" style="width: 480px; height: 250px; overflow-y: auto;
                        line-height: 20px;">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
