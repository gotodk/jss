<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index2.aspx.cs" Inherits="Web_tijiao_index2" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查询列表类</title>
    <link href="../../css/style.css" rel="Stylesheet" type="text/css" />
    <script src="../../js/adddate.js" type="text/javascript">        function igtbl_reOkBtn_onclick() {

        }

    </script>
    <style type="text/css">
        /*最外层div，宽100%，背景#f7f7f7*/
        #new_content
        {
            width: 100%;
            background-color: #f7f7f7;
        }
        /*内层div*/
        #new_zicontent
        {
            padding: 30px;
        }
        /*内容正文div，用于控制内容宽度，取值为500px、919px、1084px;*/
        #content_zw
        {
            width: 1084px;
        }
        /*页面备注内容*/
        .content_bz
        {
            font-size: 12px;
            line-height: 25px;
            color: Red;
        }
        /*分割线*/
        .content_lx
        {
            height: 1px;
            background-color: #a5cbe2;
            margin-top: 10px;
            margin-bottom: 10px;
        }
        .content_nr
        {
        }
        .tj_input
        {
            height: 20px;
            border-width: 1px;
            border-style: solid;
            border-color: #999999;
            line-height: 20px;
        }
        .tj_input:hover
        {
            border-color: #7e9cb6;
        }
        .tj_input_dj
        {
            height: 20px;
            border-width: 1px;
            border-style: solid;
            border-color: #AAAAAA;
            line-height: 20px;
        }
        .tj_bt
        {
            height: 22px;
            border-width: 1px;
            border-style: solid;
            border-color: #999999;
            background-image: url("huibg.jpg");
            line-height: 20px;
        }
        .tj_bt:hover
        {
            border-color: #7e9cb6;
            cursor: pointer;
            background: url("lanbg.jpg");
        }
        .tj_bt_da
        {
            height: 30px;
            border-width: 1px;
            border-style: solid;
            border-color: #999999;
            background-image: url("huibg2.jpg");
            line-height: 30px;
        }
        .tj_bt_da:hover
        {
            border-color: #7e9cb6;
            cursor: pointer;
            background: url("lanbg2.jpg");
        }
        .tj_input_search
        {
            background-image: url("search.jpg");
            background-position: right;
            background-repeat: no-repeat;
        }
        .tj_input_time
        {
            background-image: url("shijian.jpg");
            background-position: right;
            background-repeat: no-repeat;
        }
        .content_nr_cx
        {
            border: solid 1px #99BBE8;
            background-color: #E0E7F7;
        }
        .content_nr_cx tr
        {
            height: 40px;
            line-height: 40px;
        }
        .content_nr_hj
        {
            line-height: 25px;
            margin-top: 10px;
            margin-bottom: 5px;
        }
        .content_nr_hj td
        {
            font-weight: bold;
        }
        
        .tab
        {
        }
        
        .tab td
        {
            height: 26px;
            vertical-align: middle;
            line-height: 25px;
        }
        .bg
        {
            background-color: #D6E3F3;
        }
        .bt_bg
        {
            background: url(bg.gif) repeat-x 0 0;
        }
        .tou_table td
        {
            border-right: solid 1px #BEBCBF;
        }
        .nr_table td
        {
            border-bottom: solid 1px #DCDCDC;
            border-right: solid 1px #DCDCDC;
        }
        .nr_table tr:hover
        {
            background-color: #DFE8F7;
        }
        .wei_table td
        {
            border-right: solid 1px #BEBCBF;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="25px" Skin="Default2006"
        BackColor="#f7f7f7">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" Text="查询类列表">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
    <div id="new_content">
        <div id="new_zicontent">
            <div id="content_zw">
                <div class="content_bz">
                    说明文字：<br />
                    1、该模块用于记录已签约服务商以个人名义打款的打款人信息以及与服务商的对应关系。<br />
                    2、保存时系统根据规则自动生成正式客户编号，打款人编号以"6"开头，用生成的编号录入ERP。<br />
                    3、“所属服务商编号”请填写本办事处销售渠道为“服务商”或“门店服务商”的客户编号。<br /><br />
                </div>
                <div class="content_nr">
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_cx">
                        <tr>
                            <td width="50px" align="right">
                                分公司:
                            </td>
                            <td width="80px">
                                <asp:DropDownList ID="DropDownList4" runat="server" Width="80px" CssClass="tj_input"
                                    Height="22px">
                                    <asp:ListItem>办公室</asp:ListItem>
                                    <asp:ListItem>人力资源部</asp:ListItem>
                                    <asp:ListItem>信息化中心</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="70px" align="right">
                                开始时间:
                            </td>
                            <td width="120px">
                                <asp:TextBox ID="TextBox4" runat="server" class="tj_input tj_input_time" Width="158px"
                                    onfocus="setday(this,document.all.TextBox4)"></asp:TextBox>
                            </td>
                            <td width="20px" align="center">
                                至
                            </td>
                            <td width="120px">
                                <asp:TextBox ID="TextBox1" runat="server" class="tj_input tj_input_time" Width="158px"
                                    onfocus="setday(this,document.all.TextBox4)"></asp:TextBox>
                            </td>
                            <td width="80px" align="right">
                                服务商名称:
                            </td>
                            <td width="120px">
                                <asp:TextBox ID="TextBox7" runat="server" class="tj_input tj_input_search" Width="120px"></asp:TextBox>
                            </td>
                            <td width="60px" align="right">
                                订单号:
                            </td>
                            <td width="120px">
                                <asp:TextBox ID="TextBox2" runat="server" CssClass="tj_input" Width="120px" Enabled="True"
                                    TabIndex="1"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:Button ID="Button4" runat="server" CssClass="tj_bt" Text="查询" UseSubmitBehavior="False" />&nbsp;&nbsp;<asp:Button
                                    ID="Button3" CssClass="tj_bt" runat="server" Text="导出Excel" UseSubmitBehavior="False" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" class="content_nr_hj">
                        <tr>
                            <td>
                                当前合计：共24242条数据，数量合计66492支，已交数量6558885。<br />
                                其中，订单（包括普通订单、特殊支持、春雨行动）共计19744条，数据合计125844只，已提交12578.
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#99BBE8"
                        style="border-collapse: collapse;" class="tab">
                        <tr>
                            <td class="bg">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="bt_bg">
                                <table width="100%" cellspacing="0" cellpadding="0" class="tou_table">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="nr_table">
                                    <tr>
                                        <td>
                                            ddd
                                        </td>
                                        <td>
                                            dd;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="bg">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <asp:Panel ID="Panel1" runat="server" Visible="false">
            <div>
                <table>
                    <tr>
                        <td>
                            会议主题:
                        </td>
                        <td>
                            <asp:TextBox ID="tb_hyzt" runat="server" class="input_out" Width="500px"></asp:TextBox>
                        </td>
                        <td>
                            会议主办方:
                        </td>
                        <td>
                            <asp:TextBox ID="tb_hyzbf" runat="server" class="input_out" Width="178px"></asp:TextBox>
                        </td>
                        <td>
                            会议主持人:
                        </td>
                        <td>
                            <asp:TextBox ID="tb_hyzcr" runat="server" class="input_out" Width="178px"></asp:TextBox>
                        </td>
                        <td>
                            会议地点:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_hydd" runat="server" Width="178px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            会议开始时间:
                        </td>
                        <td>
                            <asp:TextBox ID="txtHYStartTime" runat="server" Width="178px" onfocus="setday(this,document.all.txtHYStartTime)"></asp:TextBox>
                        </td>
                        <td>
                            会议结束时间:
                        </td>
                        <td>
                            <asp:TextBox ID="txtHYEndTime" runat="server" Width="178px" onfocus="setday(this,document.all.txtHYEndTime)"></asp:TextBox>
                        </td>
                        <td>
                            会议申请人姓名:
                        </td>
                        <td>
                            <asp:TextBox ID="tb_hysqrxm" runat="server" class="input_out" Width="178px"></asp:TextBox>
                        </td>
                        <td>
                            是否有效:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_sfyx" runat="server" Width="178px">
                                <asp:ListItem>所有</asp:ListItem>
                                <asp:ListItem>有效</asp:ListItem>
                                <asp:ListItem>撤销</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            申请提交时间:
                        </td>
                        <td>
                            <asp:TextBox ID="tb_sqtjsj" runat="server" Width="178px" onfocus="setday(this,document.all.tb_sqtjsj)"></asp:TextBox>
                        </td>
                        <td>
                            会议当前状态:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_hydqzt" runat="server" Width="178px">
                                <asp:ListItem>所有</asp:ListItem>
                                <asp:ListItem>尚未召开</asp:ListItem>
                                <asp:ListItem>正在召开</asp:ListItem>
                                <asp:ListItem>已经结束</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="4" align="right">
                            <asp:Button ID="BtnCheck" runat="server" CssClass="button" OnClick="BtnCheck_Click"
                                Text="查询" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <asp:Panel ID="Panel3" runat="server" Width="100%" ScrollBars="Both" Visible="true">
            <radg:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" Skin="Monochrome" PageSize="10"
                Width="98%" OnPageIndexChanged="RadGrid1_PageIndexChanged">
                <HeaderStyle Height="28px"></HeaderStyle>
                <ExportSettings>
                    <Pdf PageWidth="8.5in" PageRightMargin="" PageFooterMargin="" PageLeftMargin="" PageTopMargin=""
                        PageHeight="11in" PageBottomMargin="" PageHeaderMargin=""></Pdf>
                </ExportSettings>
                <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条."
                    PrevPageText="上一页"></PagerStyle>
                <MasterTableView AutoGenerateColumns="false" DataKeyNames="ID">
                    <NoRecordsTemplate>
                        没有找到任何数据。
                    </NoRecordsTemplate>
                    <ExpandCollapseColumn Visible="False" Resizable="False">
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>
                    <Columns>
                        <radg:GridBoundColumn DataField="ID" HeaderText="编号" SortExpression="ID" UniqueName="ID"
                            Visible="false">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYZT" HeaderText="会议主题" SortExpression="HYZT" UniqueName="HYT">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYZBF" HeaderText="会议主办方" SortExpression="HYZBF"
                            UniqueName="HYZBF">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYZCR" HeaderText="会议主持人" SortExpression="HYZCR"
                            UniqueName="HYZCR">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYDD" HeaderText="会议地点" SortExpression="HYDD" UniqueName="HYDD">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYKSSJ" HeaderText="会议开始时间" SortExpression="HYKSSJ"
                            UniqueName="HYKSSJ">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYJSSJ" HeaderText="会议结束时间" SortExpression="HYJSSJ"
                            UniqueName="HYJSSJ">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYSQRGH" HeaderText="会议申请人工号" SortExpression="HYSQRGH"
                            UniqueName="HYSQRGH" Visible="false">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYSQRXM" HeaderText="会议申请人" SortExpression="HYSQRXM"
                            UniqueName="HYSQRXM">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="SFYX" HeaderText="是否有效" SortExpression="SFYX" UniqueName="SFYX">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="SQTJSJ" HeaderText="申请提交时间" SortExpression="SQTJSJ"
                            UniqueName="SQTJSJ">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYDQZT" HeaderText="会议当前状态" SortExpression="HYDQZT"
                            UniqueName="HYDQZT">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="HYDJS" HeaderText="会议倒计时(小时)" SortExpression="HYDJS"
                            UniqueName="HYDJS">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="Ischeck" HeaderText="是否审核" SortExpression="Ischeck"
                            UniqueName="Ischeck">
                        </radg:GridBoundColumn>
                        <radg:GridBoundColumn DataField="Remark" HeaderText="备注" SortExpression="Remark"
                            UniqueName="Remark">
                        </radg:GridBoundColumn>
                        <radg:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemStyle Width="0px" />
                            <HeaderTemplate>
                                查看详情
                            </HeaderTemplate>
                            <ItemTemplate>
                                <a href='HYSSQDJ.aspx?lable=view&ly=qgs&ID=<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                    style="color: Red">查看详情</a>
                            </ItemTemplate>
                        </radg:GridTemplateColumn>
                        <radg:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemStyle Width="0px" />
                            <ItemTemplate>
                                <asp:LinkButton Text="撤销会议" OnClientClick="javascript:return confirm('您确实要撤销此会议吗？');"
                                    OnClick="lbtnCX_Click" runat="server" ID="lbtnCX" ForeColor="red"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                撤销会议
                            </HeaderTemplate>
                        </radg:GridTemplateColumn>
                        <radg:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemStyle Width="0px" />
                            <ItemTemplate>
                                <a href='HYSSQDJ.aspx?lable=Edit&ly=qgs&ID=<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                    style="color: Red">会议变更</a>
                            </ItemTemplate>
                            <HeaderTemplate>
                                会议变更
                            </HeaderTemplate>
                        </radg:GridTemplateColumn>
                        <radg:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemStyle Width="0px" />
                            <ItemTemplate>
                                <asp:LinkButton Text="审核" OnClick="lbtnCHeck_Click" runat="server" ID="lbtnCHeck"
                                    ForeColor="red"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                审核
                            </HeaderTemplate>
                        </radg:GridTemplateColumn>
                        <radg:GridTemplateColumn UniqueName="TemplateColumn">
                            <ItemStyle Width="0px" />
                            <ItemTemplate>
                                <asp:LinkButton Text="备注" runat="server" ID="lbtnRemark" ForeColor="red" OnClick="lbtnRemark_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                备注
                            </HeaderTemplate>
                        </radg:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </radg:RadGrid>
        </asp:Panel>
    </div>
    <asp:Panel ID="Panel2" runat="server">
        <table border="0" cellpadding="5" cellspacing="0" class="FormView" width="100%">
            <tr>
                <td align="left">
                    <asp:Button ID="btnTJ" runat="server" Text="显示查询条件" CssClass="button" Width="102px"
                        OnClick="btnTJ_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div id="divStop" runat="server" style="border: 3px solid #C3D9FF; text-align: left;
        left: 50px; top: 348px; width: 700px; height: 150px; background-color: #F9FBFD;
        position: absolute; z-index: 999;" visible="false">
        <div id="Div2" style="cursor: move; left: 1px; top: 100px; width: 100%; height: 30px;
            background-color: #A5C2E0; text-align: center; vertical-align: center; line-height: 30px;
            font-weight: bold; color: #ffffff">
            您正在为：
            <asp:Label ID="labsmtitle" runat="server" Text="Label">   
            </asp:Label>
            添加备注。 使用部门:
            <asp:Label ID="labsmid" runat="server" Text="Label"></asp:Label></div>
        <table width="100%" style="height: 122px">
            <tr>
                <td colspan="1" valign="middle" class="style6" align="center">
                    备注：
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtStopidear" runat="server" TextMode="MultiLine" Width="540px"
                        Height="56px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnStopSave" runat="server" Text="确定" CssClass="button" OnClick="btnStopSave_Click" />
                    <asp:Button ID="Btnstopcancel" runat="server" Text="取消" CssClass="button" OnClick="Btnstopcancel_Click" />
                </td>
            </tr>
        </table>
    </div>
    <input runat="server" id="hidID" type="hidden" />
    </form>
</body>
</html>
