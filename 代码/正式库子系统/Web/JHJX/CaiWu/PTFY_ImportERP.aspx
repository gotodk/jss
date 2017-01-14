<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PTFY_ImportERP.aspx.cs" Inherits="Web_JHJX_PTFY_ImportERP" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务操作平台-平台开票信息查询</title>   
    <script src="../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../../js/standardJSFile/art_confirm.js" type="text/javascript"></script>
    <link href="../../../css/standardStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/standardJSFile/jquery.tableresizer.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>
    <script src="../../../js/standardJSFile/jquery.tablechangecolor.js" type="text/javascript"></script>
    <script src="../../../js/pingbi.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#theObjTable").tablechangecolor();
        });
    </script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#btnConvert").click(function () {
                $("#lb1").show();
                $(".btnDisplay").hide();
            })
            var hiddenVal = $("#IsConvert").val();
            if (hiddenVal == "0") {
                $(".btnDisplay").hide();
                $("#lb1").show();
                $("#lb1").val("没有需要处理的数据").text("没有需要处理的数据");
            }
            if (hiddenVal == "3") {
                $(".btnDisplay").hide();
                $("#lb1").show();
                $("#lb1").val("数据中有为空的字段，无法执行转入ERP操作").text("数据中有为空的字段，无法执行转入ERP操作");
            }
            $("#btnValidate").click(function () {
                $("#lb2").val("正在验证ERP中的客户信息，请稍后……").tex("正在验证ERP中的客户信息，请稍后……");
                $("#lb2").show();
                $(".btnDisplay").hide();
            })
        })
    </script>
    <style type="text/css">
        #content_zw
        {
            width: 950px;
        }
    </style>
</head>
<body style="background-color: #f7f7f7;">
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#F7F7F7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" NavigateUrl="PTFY_ImportERP.aspx" Text="平台费用转入ERP"
                ForeColor="Red">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_bz">
                    说明：<br />
                    1、该模块用于将交易平台产生的技术服务费、账户管理费、违规罚款费用导入到ERP收款单对应单别中。<br />
                    2、由于ERP单号限制，每天最多可转入ERP的数据为999条，超出的数据请第二天处理。导入后请进入ERP对单据进行审核。<br />
                    3、为方便执行，每次最多显示和导入50条数据。如果待处理的数据多余此数量，请分多次操作。
                </div>
                <br />
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td width="25%">
                                发票/邮递信息列表（金额单位：元）
                            </td>
                            <td width="75%" align="left" style=" color :Red">
                               今日已处理数据量为：<span id ="spanYCLSJL" runat ="server">0</span>；还可处理数据量为：<span id ="spanKCLSJL" runat ="server">0</span>；当前待处理数据量为：<span id ="spanDCLSJL" runat ="server">0</span>。
                            </td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8" style="border-collapse: collapse;
                        table-layout: fixed; width: 950px;" class="tab">
                        <tr>
                            <td>
                                <table id="theObjTable" style="width: 950px;" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th class="TheadTh" style="width: 30px;">
                                                序号
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                流水号
                                            </th>
                                            <th class="TheadTh" style="width: 100px;">
                                                平台管理机构
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                客户编号
                                            </th>
                                            <th class="TheadTh" style="width: 150px;">
                                                单位名称
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                款项类型
                                            </th>
                                            <th class="TheadTh" style="width: 90px;">
                                                款项金额
                                            </th>
                                            <th class="TheadTh" style="width: 80px;">
                                                结算科目
                                            </th>
                                            <th class="TheadTh" style="width: 120px;">
                                                产生时间
                                            </th>                                            
                                            <th class="TheadTh" style="width:60px;" title='平台管理机构、客户编号、单位名称、结算科目不为空为成功'>
                                                数据验证
                                            </th>
                                            <th class="TheadTh" style="width: 60px;" title='客户信息在ERP中存在显示成功'>
                                                客户验证
                                            </th>

                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
                                            <ItemTemplate>
                                                <tr class="TbodyTr">
                                                    <td>
                                                        <asp:Label ID="lblXH" runat="server" Text='  <%#Eval("序号") %>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLSH" runat="server" Text='  <%#Eval("Number") %>'> </asp:Label>
                                                    </td>
                                                    <td title ='<%#Eval("ptgljg")%>'>
                                                        <asp:Label ID="lblPTGLJG" runat="server" Text='  <%#Eval("ptgljg")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblKHBH" runat="server" Text='  <%#Eval("I_ZQZJZH")%>'> </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("I_JYFMC")%>'>
                                                        <asp:Label ID="lblDWMC" runat="server" Text='  <%#Eval("I_JYFMC")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblKXLX" runat="server" Text='  <%#Eval("XM")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblKXJE" runat="server" Text=' <%#Eval("JE")%>'> </asp:Label>
                                                    </td>
                                                    <td title='<%#Eval("dyjskmbh")%>'>
                                                        <asp:Label ID="lblJSKM" runat="server" Text='<%#Eval("dyjskmbh")%>'> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCSSJ" runat="server" Text='<%#Eval("createtime")%>'> </asp:Label>
                                                    </td>
                                                     <td>
                                                     <asp:Label ID ="lblDui" runat ="server" Visible ='<%#Eval("sjyz").ToString()=="成功" %>' style="font-weight:bold ; font-size:11pt; color :Green" >√</asp:Label>
                                                      <asp:Label ID ="lblCha" runat ="server" Visible ='<%#Eval("sjyz").ToString()=="失败" %>' style="font-weight:bold ; font-size:11pt; color :Red" >×</asp:Label>
                                                       <%-- <asp:Label ID="lblSJYZ" runat="server" Text='<%#Eval("sjyz")%>'> </asp:Label>--%>
                                                    </td>
                                                    <td>
                                                   <asp:Label ID ="Label1" runat ="server" Visible ='<%#Eval("erpkhbh").ToString()!="" %>' style="font-weight:bold ; font-size:12pt; color :Green" >√</asp:Label>
                                                      <asp:Label ID ="Label2" runat ="server" Visible ='<%#Eval("erpkhbh").ToString()=="" %>' style="font-weight:bold ; font-size:12pt; color :Red" >×</asp:Label>
                                                        <%--<asp:Label ID="lblKHXXYZ" runat="server" Text='<%#Eval("erpkhbh").ToString ().Trim ()==""?"失败":"成功"%>'> </asp:Label>--%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr id="tdEmpty" runat="server" visible="false" style="text-align: center;">
                                            <td colspan="11">
                                              <label runat ="server" id ="lblk">暂无需要处理的数据！</label>  
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div style="width: 900px; text-align: center; vertical-align: middle;">
                    <asp:Label runat="server" ID="lb1" Style="display: none;">正在转入ERP收款单，请稍后……</asp:Label>
                    <asp:Label runat="server" ID="lb2" Style="display: none;">正在向ERP中导入客户信息，请稍后……</asp:Label>
                    <div class="btnDisplay">
                        <asp:Button ID="btnValidate" runat="server" Text="验证客户信息" CssClass="tj_bt_da" Width="120px"
                            OnClick="btnValidate_Click" Height="30px" />&nbsp;
                        <asp:Button ID="btnConvert" runat="server" Text="转入ERP收款单" CssClass="tj_bt_da" Width="120px"
                            OnClick="btnConvert_Click" Height="30px" />&nbsp;
                        <asp:Button ID="btnShuaXin" runat="server" Text="刷新数据" CssClass="tj_bt_da" Width="120px"
                            OnClick="btnShuaXin_Click" Height="30px" />&nbsp;
                        <asp:Button ID="btnToExcel" runat="server" Text="导出到Excel" CssClass="tj_bt_da" Width="120px"
                            OnClick="btnToExcel_Click" Height="30px" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divDisplay" style="display: none" runat="server">
        <asp:Repeater ID="rptPrint" runat="server">
            <HeaderTemplate>
                <table border="1px" bordercolor="#D1CFCF" style="border-style: solid; border-collapse: collapse;
                    text-align: center; line-height: 25px;" rules="all" class="GridViewStyle" id="rpt1"
                    width="650px">
                    <tr style="font-weight: bold; height: 35px;">
                        <td>
                            流水号
                        </td>
                        <td>
                            所属分公司
                        </td>
                        <td>
                            客户编号
                        </td>
                        <td>
                            单位名称
                        </td>
                        <td>
                            款项类型
                        </td>
                        <td>
                            款项金额（元）
                        </td>
                        <td>
                            结算科目
                        </td>
                        <td>
                            产生时间
                        </td>
                        <td>
                            数据验证
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Eval("Number").ToString () %>&nbsp;
                    </td>
                    <td>
                        <%#Eval("ptgljg")%>
                    </td>
                    <td>
                        <%#Eval("I_ZQZJZH")%>
                    </td>
                    <td>
                        <%#Eval("I_JYFMC")%>
                    </td>
                    <td>
                        <%#Eval("XM")%>
                    </td>
                    <td>
                        <%#Eval("JE")%>
                    </td>
                    <td>
                        <%#Eval("dyjskmbh")%>
                    </td>
                    <td>
                        <%#Eval("createtime")%>
                    </td>
                    <td>
                        <%#Eval("sjyz")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
     <asp:HiddenField ID="IsConvert" runat="server" />
    </form>
</body>
</html>
