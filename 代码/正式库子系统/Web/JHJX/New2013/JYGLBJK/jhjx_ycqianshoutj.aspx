<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjx_ycqianshoutj.aspx.cs" Inherits="Web_JHJX_New2013_JYGLBJK_jhjx_ycqianshoutj" %>

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

              
                <radTS:Tab ID="Tab1" runat="server" NavigateUrl="jhjx_ycqianshoutj.aspx"
                    Text="异常签收统计" ForeColor="Red" >
                </radTS:Tab>
            


            </Tabs>
        </radTS:RadTabStrip>
        <div id="new_content">
            <div id="new_zicontent">
                <div id="content_zw">

                    <div class="content_nr">
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx" style="border: solid 1px #ccc">
                            <tr>
                                <td width="100px" align="right">异常签收情况：
                                </td>
                                <td width="120px" align="left">
                                    <asp:DropDownList ID="ddl_ycqsqk" runat="server" CssClass="tj_input" Width="110px">
                                        <asp:ListItem>全部</asp:ListItem>
                                        <asp:ListItem>有异议收货</asp:ListItem>
                                        <asp:ListItem>部分收货</asp:ListItem>
                                        <asp:ListItem>请重新发货</asp:ListItem>
                                    </asp:DropDownList>
                                </td>

                                <td width="90px" align="right">异常签收时间：</td>
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
                                <td align="left" colspan=" 1" style=" width:120px">
                                    <asp:TextBox ID="txt_jyfmc" runat="server" class="tj_input" Width="110px"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="Btnsure" runat="server" CssClass="tj_bt" Width="40px" Text="查询"
                                        OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnexport" runat="server" CssClass="tj_bt" Width="40px" Text="导出" OnClick="btnexport_Click" />

                                </td>


                            </tr>
                            <tr>
                                <td colspan="8">
                                    <table>
                                        <tr>
                                            <td align="right" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 发货单金额大于等于：</td>
                                            <td>
                                                <asp:TextBox ID="txtjjrbh" runat="server" class="tj_input" Width="60px">0.00</asp:TextBox>
                                            </td>
                                            <td><font  color="red" >万元</font>的发货单汇总。
                                            </td>
                                            <td>
                                                <asp:Button ID="btnsave" runat="server" CssClass="tj_bt" Width="40px" Text="确定"
                                                  OnClientClick="javascript:return confirm('您确实要更改发货单查询的发货金额最小值吗？');"   OnClick="btnsave_Click" />
                                            </td>

                                        </tr>

                                    </table>
                                </td>


                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                            <tr>
                                <td>异常签收统计列表 (金额单位：元)</td>
                            </tr>
                        </table>

                        <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse; table-layout: fixed; width: 100%;"
                            class="tab">
                            <tr>
                                <td>
                                    <div style="overflow-x: scroll;">
                                        <table id="theObjTable" style="width: 2050px;" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr>
                                                    <th class="TheadTh" style="width: 110px;">合同编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">商品编号
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px;">商品名称
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">发货单号
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">发货数量
                                                    </th>
                                                    <th class="TheadTh" style="width: 150px;">发货金额
                                                    </th>
                                                    <th class="TheadTh" style="width: 80px;">异常签收类型
                                                    </th>
                                                    <th class="TheadTh" style="width: 150px;">签收时间   
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">买家账号
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px;">买家名称
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">买家联系人
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">买家联系方式
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">卖家账号
                                                    </th>
                                                    <th class="TheadTh" style="width: 120px;">卖家名称
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">卖家联系人
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">卖家联系方式
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">收货数量
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">差异数量
                                                    </th>
                                                    <th class="TheadTh" style="width: 100px;">异常操作附件
                                                    </th>


                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="Repeater1" runat="server">
                                                    <ItemTemplate>
                                                        <tr class="TbodyTr">
                                                            <td style="width: 110px;">
                                                                <%#Eval("合同编号")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("商品编号")%>
                                                            </td>
                                                            <td style="width: 120px;">
                                                               
                                                                 <asp:Label ID="Label2" runat="server" Text='<%#Eval("商品名称").ToString().Length > 8 ? Eval("商品名称").ToString().Substring(0, 8) + "..." : Eval("商品名称").ToString()%>' ToolTip='<%#Eval("商品名称")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("发货单号")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("发货数量")%>
                                                     
                                                            </td>
                                                            <td style="width: 150px;">
                                                                <%#Eval("发货金额")%>
                                                            </td>
                                                            <td style="width: 80px;">
                                                                <%#Eval("异常签收类型")%>
                                                            </td>
                                                            <td style="width: 150px;">
                                                                <%#Eval("签收时间")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("买家账号")%> 
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("买家名称").ToString().Length > 8 ? Eval("买家名称").ToString().Substring(0, 8) + "..." : Eval("买家名称").ToString()%>' ToolTip='<%#Eval("买家名称")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("买家联系人")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("买家联系方式")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("卖家账号")%> 
                                                            </td>
                                                            <td style="width: 120px;">

                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("卖家名称").ToString().Length > 10 ? Eval("卖家名称").ToString().Substring(0, 8) + "..." : Eval("卖家名称").ToString()%>' ToolTip='<%#Eval("卖家名称")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("卖家联系人")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("卖家联系方式")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("收货数量")%> 
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("差异数量")%>
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <%#Eval("异常操作附件")%>
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
