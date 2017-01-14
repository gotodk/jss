<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjx_yichangHTcx.aspx.cs" Inherits="Web_JHJX_New2013_JYGLBJK_jhjx_yichangHTcx" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Src="../../../pagerdemo/commonpager.ascx" TagName="commonpager" TagPrefix="uc1" %>
<%@ Register Src="../../../pagerdemo/commonpagernew.ascx" TagName="commonpagernew"
    TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
</head>
<body onload="ShowUpdating()" style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
        <radTS:RadTabStrip ID="TabStrip1" runat="server" CausesValidation="False" ReorderTabRows="True"
            Skin="Default2006" Width="99%">
            <Tabs>
                <radTS:Tab ID="Tab2" runat="server" NavigateUrl="jhjx_yichangHTcx.aspx" Text="异常合同查询 "
                    Font-Size="12px" ForeColor="Red">
                </radTS:Tab>
               
            </Tabs>
        </radTS:RadTabStrip>
        <div id="new_content">
            <div id="new_zicontent">
                <div id="content_zw">
                    <div class="content_bz">
                        <%--说明文字--%>
                    </div>
                    <div class="content_nr">
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                            <tr>
                                <td align="right" width="100px">
                                    合同异常原因：
                                </td>
                                <td width="130px">
                                    <asp:DropDownList ID="ddlyy" runat="server" CssClass="tj_input" Width="100%">
                                        <asp:ListItem>全部</asp:ListItem>
                                        <asp:ListItem>仅买家违约</asp:ListItem>
                                        <asp:ListItem>仅卖家违约</asp:ListItem>
                                        <asp:ListItem>买家卖家均有违约</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td width="80px" align="center">
                                    <span>交易方名称</span>：
                                </td>
                                <td width="130px">
                                    <asp:TextBox ID="txtJYFMC" runat="server" Width="130px" CssClass="tj_input"></asp:TextBox>
                                </td>
                                <td width="90px" align="right">合同<span>结束时间</span>：
                                </td>
                                <td width="178px">
                                    <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                        ID="txtBeginTime" runat="server" Width="178px"></asp:TextBox>
                                </td>
                                <td width="20px" align="center">至
                                </td>
                                <td width="178px">
                                    <asp:TextBox class="tj_input Wdate" onclick="WdatePicker({readOnly:true,minDate:'2012-01-01',maxDate:'2022-01-01',dateFmt:'yyyy-MM-dd'});"
                                        ID="txtEndTime" runat="server" Width="178px"></asp:TextBox>
                                </td>
                                <td align="center" style="padding-left: 5px; width: 120px;">
                                    <asp:Label ID="Label1" runat="server" Text="正在查询中，请稍后..." Width="120px"></asp:Label>
                                    <asp:Button ID="BtnCheck" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                        Text="查询" Width="70px" OnClientClick="Click();" OnClick="BtnCheck_Click" />
                                </td>
                                <td align="left" colspan="4" style="padding-left: 10px;">
                                    <asp:Button ID="btnToExcel" runat="server" CssClass="tj_bt" UseSubmitBehavior="False"
                                        Text="导出" Width="70px" OnClick="btnToExcel_Click" />
                                </td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td>
                                    异常合同查询列表</td>
                            </tr>
                        </table>
                        <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                            style="border-collapse: collapse;" class="tab">
                            <tr>
                                <td>
                                    <div  style="overflow-x: scroll;width:1110px" >
                                    <table id="theObjTable" style="width: 1695px;" cellspacing="0" cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th class="TheadTh" style="width: 110px; line-height: 18px;">合同编号</th>
                                                <th class="TheadTh" style="width:90px; line-height: 18px;" >买家是<br />否违约</th>
                                                <th class="TheadTh" style="width: 90px; line-height: 18px;">卖家是<br />否违约</th>
                                                <th class="TheadTh" style="width: 90px;">买家账号</th>
                                                <th class="TheadTh" style="width: 90px;">买家名称
                                                </th>
                                                <th class="TheadTh" style="width: 90px; line-height: 18px;">卖家账号
                                                </th>
                                                <th class="TheadTh" style="width: 90px; line-height: 18px;">卖家名称</th>
                                                <th class="TheadTh" style="width: 75px;">商品编码</th>
                                                <th class="TheadTh" style="width: 80px;">商品名称</th>

                                                <th class="TheadTh" style="width: 120px;">合同开始时间</th>
                                                <th class="TheadTh" style="width: 120px; line-height: 18px;">合同结束时间</th>
                                                <th class="TheadTh" style="width: 80px;">合同数量
                                                    </th>
                                                <th class="TheadTh" style="width: 80px;">提货数量
                                                   </th>
                                                <th class="TheadTh" style="width: 80px; line-height: 18px;">未提货<br />
                                                    数量</th>
                                                <th class="TheadTh" style="width: 80px;">发货数量
                                                </th>
                                                <th class="TheadTh" style="width: 80px; line-height: 18px;">未发货数量
                                                </th>
                                                <th class="TheadTh" style="width: 80px; line-height: 18px;">买家<br />履约率</th>
                                                <th class="TheadTh" style="width: 80px;line-height: 18px;">卖家<br />履约率</th>
                                                <th class="TheadTh" style="width: 80px;line-height: 18px;">清盘<br />是否完成</th>
                                                <th class="TheadTh" style="width: 80px;line-height: 18px;">实际<br />收货数量</th>
                                                <th class="TheadTh" style="width: 80px;line-height: 18px;">有效<br />
                                                    履约率</th>

                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server">
                                                <ItemTemplate>
                                                    <tr class="TbodyTr">
                                                        <td style="width: 110px;">
                                                            <%#Eval("合同编号") %>                                             
                                                        </td>
                                                        <td style="width: 90px;">
                                                            <%#Eval("买家是否违约") %>      
                                                        </td>
                                                        <td>
                                                            <%#Eval("卖家是否违约") %>
                                                        </td>
                                                        <td>
                                                            <%#Eval("买家账号") %>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label4" runat="server" Text='<%#Eval("买家名称").ToString().Length > 10 ? Eval("买家名称").ToString().Substring(0, 8) + "..." : Eval("买家名称").ToString()%>' ToolTip='<%#Eval("买家名称")%>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <%#Eval("卖家账号")%>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" Text='<%#Eval("卖家名称").ToString().Length > 10 ? Eval("卖家名称").ToString().Substring(0, 8) + "..." : Eval("卖家名称").ToString()%>' ToolTip='<%#Eval("卖家名称")%>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <%#Eval("商品编码")%>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" Text='<%#Eval("商品名称").ToString().Length > 10 ? Eval("商品名称").ToString().Substring(0, 8) + "..." : Eval("商品名称").ToString()%>' ToolTip='<%#Eval("商品名称")%>'></asp:Label>

                                                        </td>

                                                        <td>
                                                            <%#Eval("合同开始时间")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("合同结束时间")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("合同数量")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("提货数量")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("未提货数量")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("发货数量")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("未发货数量")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("买家履约率")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("卖家履约率")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("清盘是否完成")%> 
                                                        </td>
                                                        <td>
                                                            <%#Eval("实际收货数量")%> 
                                                        </td>
                                                        <td>
                                                            <%#Eval("有效履约率")%> 
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                        <tfoot>
                                            <tr id="ts" runat="server" class="TfootTr">
                                                <td colspan="21" align="center">当前数据为空！
                                                </td>
                                            </tr>
                                        </tfoot>
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
        <input runat="server" id="hidID" type="hidden" />
        <input runat="server" id="hidwhere" type="hidden" />
        <input runat="server" id="hidwhereis" type="hidden" />
    </form>
</body>
</html>
<script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script lang="ja" type="text/javascript">
    function ShowUpdating() {
        document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'block';
        document.getElementById("<%=btnToExcel.ClientID %>").style.display = 'block';
        document.getElementById("Label1").style.display = 'none';
    }
    function Click() {
        document.getElementById("<%=BtnCheck.ClientID %>").style.display = 'none';
        document.getElementById("<%=btnToExcel.ClientID %>").style.display = 'none';
        document.getElementById("Label1").style.display = 'block';
    }
</script>
