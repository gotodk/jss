<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jhjx_FGSYWDataTongji.aspx.cs" Inherits="Web_JHJX_New2013_JHJX_FGSYWCX_jhjx_FGSYWDataTongji" %>

<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../../js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/fcf.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
                BackColor="#F7F7F7">
                <Tabs>
                    <radTS:Tab ID="Tab2" runat="server" NavigateUrl="jhjx_FGSYWDataTongji.aspx" ForeColor="Red"
                        Text=" 分公司业务数据统计">
                    </radTS:Tab>
                    <radTS:Tab ID="Tab1" runat="server" NavigateUrl="jhjx_jyzhywchax.aspx"
                        Text="分公司所属的交易账户业务查看">
                    </radTS:Tab>

                </Tabs>
            </radTS:RadTabStrip>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btntongji" runat="server" Text="开始统计" CssClass="tj_bt_da" OnClick="btntongji_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btndaochu" runat="server" Text="导出" CssClass="tj_bt_da" OnClick="btndaochu_Click" />

                    </td>
                </tr>
            </table>

            <table width="1600px" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                style="border-collapse: collapse;" class="tab" id="Table1">
                <thead>
                    <tr>
                        <td colspan="30" align="center">
                            <span style="text-align: center; font-size: large; font-weight: 800">分公司业务数据统计</span>
                        </td>
                    </tr>

                    <tr>
                        <td rowspan="2" align="center" class="FHTheadThAlign" style="width: 30px; font-weight: bold;">序号
                        </td>
                        <td rowspan="2" align="center" class="FHTheadThAlign" style="width: 120px; font-weight: bold;">分公司
                        </td>


                        <td colspan="4" align="center" class="FHTheadThAlign" style="width: 200px; font-weight: bold; height: 15px">经纪人数量(家)</td>
                        <td colspan="4" align="center" class="FHTheadThAlign" style="width: 200px; font-weight: bold; height: 15px">有效经纪人数量（家）</td>
                        <td colspan="4" align="center" class="FHTheadThAlign" style="width: 200px; font-weight: bold; height: 15px">有效卖家数量（家）</td>
                        <td colspan="4" align="center" class="FHTheadThAlign" style="width: 200px; font-weight: bold; height: 15px">有效买家数量（家）</td>
                        <td colspan="4" align="center" class="FHTheadThAlign" style="width: 200px; font-weight: bold; height: 15px">定标商品品类数量（家）</td>
                        <td colspan="4" align="center" class="FHTheadThAlign" style="width: 420px; font-weight: bold; height: 15px">定标数量</td>
                        <td colspan="4" align="center" class="FHTheadThAlign" style="width: 420px; font-weight: bold; height: 15px">定标金额（元）</td>

                    </tr>
                    <tr>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本日<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本周<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本月<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本年<br />
                            新增
                        </td>
                        <td class="FHTheadThAlign" align="center" style="width: 50px; font-weight: bold;">本日<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本周<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本月<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本年<br />
                            新增
                        </td>
                        <td class="FHTheadThAlign" align="center" style="width: 50px; font-weight: bold;">本日<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本周<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本月<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本年<br />
                            新增
                        </td>
                        <td class="FHTheadThAlign" align="center" style="width: 50px; font-weight: bold;">本日<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本周<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本月<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本年<br />
                            新增
                        </td>
                        <td class="FHTheadThAlign" align="center" style="width: 50px; font-weight: bold;">本日<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本周<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本月<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本年<br />
                            新增
                        </td>
                        <td class="FHTheadThAlign" align="center" style="width: 50px; font-weight: bold;">本日<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本周<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本月<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 50px; font-weight: bold;">本年<br />
                            新增
                        </td>
                        <td class="FHTheadThAlign" align="center" style="width: 80px; font-weight: bold;">本日<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 80px; font-weight: bold;">本周<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 80px; font-weight: bold;">本月<br />
                            新增
                        </td>
                        <td align="center" class="FHTheadThAlign" style="width: 80px; font-weight: bold;">本年<br />
                            新增
                        </td>


                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptFWS" runat="server">
                        <ItemTemplate>
                            <tr class="TbodyTr">
                                <td align="center" style="width: 30px; word-wrap: break-word;"><%#Eval("序号")%></td>
                                <td align="center" style="width: 120px; word-wrap: break-word;"><%#Eval("所属分公司")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("经纪人本日新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("经纪人本周新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("经纪人本月新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("经纪人本年新增")%>  
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效经纪人本日新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效经纪人本周新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效经纪人本月新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效经纪人本年新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效卖家本日新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效卖家本周新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效卖家本月新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效卖家本年新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效买家本日新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效买家本周新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效买家本月新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("有效买家本年新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("定标商品品类数量本日新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("定标商品品类数量本周新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("定标商品品类数量本月新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("定标商品品类数量本年新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("定标数量本日新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("定标数量本周新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("定标数量本月新增")%>
                                </td>
                                <td align="center" style="width: 50px; word-wrap: break-word;">
                                    <%#Eval("定标数量本年新增")%>
                                </td>
                                <td align="center" style="width: 80px; word-wrap: break-word;">
                                    <%#Eval("定标金额本日新增")%>
                                </td>
                                <td align="center" style="width: 80px; word-wrap: break-word;">
                                    <%#Eval("定标金额本周新增")%>
                                </td>
                                <td align="center" style="width: 80px; word-wrap: break-word;">
                                    <%#Eval("定标金额本月新增")%>
                                </td>
                                <td align="center" style="width: 80px; word-wrap: break-word;">
                                    <%#Eval("定标金额本年新增")%>
                                </td>

                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td class="FHTheadThAlign" colspan="2" align="center" style="font-weight: bold;">合计</td>

                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label1" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label2" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label3" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label4" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label5" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label6" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label7" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label8" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label9" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label10" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label11" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label12" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label13" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label14" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label15" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label16" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label17" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label18" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label19" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label20" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label21" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label22" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label23" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label24" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label25" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label26" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label27" runat="server" Text="0"></asp:Label></td>
                        <td class="FHTheadThAlign" align="center" style="font-weight: bold;">
                            <asp:Label ID="Label28" runat="server" Text="0"></asp:Label></td>


                    </tr>
                </tfoot>
            </table>
            <div id="export" runat="server" style="width: 1510px; display: none;">
                <table width="1600px" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                    id="Table2">
                    <thead>
                        <tr>
                            <td colspan="30" align="center">
                                <span>分公司业务数据统计</span>
                            </td>
                        </tr>

                        <tr>
                            <td rowspan="2" align="center">序号
                            </td>
                            <td rowspan="2" align="center">分公司
                            </td>


                            <td colspan="4">经纪人数量(家)</td>
                            <td colspan="4">有效经纪人数量（家）</td>
                            <td colspan="4">有效卖家数量（家）</td>
                            <td colspan="4">有效买家数量（家）</td>
                            <td colspan="4">定标商品品类数量（家）</td>
                            <td colspan="4">定标数量</td>
                            <td colspan="4">定标金额（元）</td>

                        </tr>
                        <tr>
                            <td align="center">本日<br />
                                新增
                            </td>
                            <td align="center">本周<br />
                                新增
                            </td>
                            <td align="center">本月<br />
                                新增
                            </td>
                            <td align="center">本年<br />
                                新增
                            </td>
                            <td align="center">本日<br />
                                新增
                            </td>
                            <td align="center">本周<br />
                                新增
                            </td>
                            <td align="center">本月<br />
                                新增
                            </td>
                            <td align="center">本年<br />
                                新增
                            </td>
                            <td align="center">本日<br />
                                新增
                            </td>
                            <td align="center">本周<br />
                                新增
                            </td>
                            <td align="center">本月<br />
                                新增
                            </td>
                            <td align="center">本年<br />
                                新增
                            </td>
                            <td align="center">本日<br />
                                新增
                            </td>
                            <td align="center">本周<br />
                                新增
                            </td>
                            <td align="center">本月<br />
                                新增
                            </td>
                            <td align="center">本年<br />
                                新增
                            </td>
                            <td align="center">本日<br />
                                新增
                            </td>
                            <td align="center">本周<br />
                                新增
                            </td>
                            <td align="center">本月<br />
                                新增
                            </td>
                            <td align="center">本年<br />
                                新增
                            </td>
                            <td align="center">本日<br />
                                新增
                            </td>
                            <td align="center">本周<br />
                                新增
                            </td>
                            <td align="center">本月<br />
                                新增
                            </td>
                            <td align="center">本年<br />
                                新增
                            </td>
                            <td align="center">本日<br />
                                新增
                            </td>
                            <td align="center">本周<br />
                                新增
                            </td>
                            <td align="center">本月<br />
                                新增
                            </td>
                            <td align="center">本年<br />
                                新增
                            </td>


                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <tr class="TbodyTr">
                                    <td align="center"><%#Eval("序号")%></td>
                                    <td align="center"><%#Eval("所属分公司")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("经纪人本日新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("经纪人本周新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("经纪人本月新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("经纪人本年新增")%>  
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效经纪人本日新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效经纪人本周新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效经纪人本月新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效经纪人本年新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效卖家本日新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效卖家本周新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效卖家本月新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效卖家本年新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效买家本日新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效买家本周新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效买家本月新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("有效买家本年新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标商品品类数量本日新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标商品品类数量本周新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标商品品类数量本月新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标商品品类数量本年新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标数量本日新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标数量本周新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标数量本月新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标数量本年新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标金额本日新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标金额本周新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标金额本月新增")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("定标金额本年新增")%>
                                    </td>

                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="2" align="center">合计</td>

                            <td align="center">
                                <asp:Label ID="Label29" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label30" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label31" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label32" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label33" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label34" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label35" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label36" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label37" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label38" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label39" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label40" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label41" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label42" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label43" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label44" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label45" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label46" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label47" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label48" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label49" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label50" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label51" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label52" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label53" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label54" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label55" runat="server" Text="0"></asp:Label></td>
                            <td align="center">
                                <asp:Label ID="Label56" runat="server" Text="0"></asp:Label></td>


                        </tr>
                    </tfoot>
                    <%--</table>--%>
            </div>
        </div>
    </form>
</body>
</html>
