<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ERPXhdShow.aspx.cs" Inherits="Web_ERPXhdShow" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售单列表</title>
    <link href="../../css/style.css" rel="Stylesheet" type="text/css" />

    <script src="../../js/SetDate.js" type="text/javascript"> </script>

    <script type="text/javascript">
        function setValue() 
        {
           var tab=parent.opener.getElementById('YDXHD'); 
           //var count=0;
            // for(var i=2;i <=tab.rows.length-2;i++) 
          //  { 
              
             //count=count+1;
                //parent.opener.document.getElementById("SL").value =tab.rows[i].cells[1].innerText;
                          
             parent.opener.document.getElementById("SL").value=tab.rows[3].cells[1].innerText;  
         //   }
                  

       }

    </script>

</head>

<script src="../js/jquery.js" type="text/javascript"></script>

<body>
    <form id="form1" runat="server">
    <asp:Panel ID="Panal1" runat="server" Width="100%" ScrollBars="Both" Height="100%">
        <table border="0" cellpadding="5" cellspacing="0" class="FormView" width="790px"
            style="height: 95%">
            <tr>
                <td height="25" width="25%">
                    单别：<asp:DropDownList ID="ddldanbie" runat="server" Visible="False">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtdanbie" runat="server" CssClass="input1"></asp:TextBox>
                </td>
                <td width="25%">
                    单号：<asp:DropDownList ID="ddldanhao" runat="server" Visible="False">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtdanhao" runat="server" CssClass="input1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td height="25" width="25%">
                    收货地址：<asp:TextBox ID="txtshouhuoAdress" runat="server" MaxLength="10" CssClass="input1"
                        Height="18px" Width="124px" AutoPostBack="True"></asp:TextBox>
                </td>
                <td height="25" width="25%">
                    所属区域：<asp:TextBox ID="txtArea" runat="server" MaxLength="10" CssClass="input1" Height="18px"
                        Width="124px" AutoPostBack="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td height="25" width="25%">
                    销货开始时间：<asp:TextBox ID="txtStartTime" runat="server" MaxLength="10" CssClass="input1"
                        Height="18px" Width="124px" AutoPostBack="True" onfocus="setday(this,document.all.txtStartTime)"></asp:TextBox>
                </td>
                <td height="25" width="25%">
                    销货结束时间<asp:TextBox ID="txtEndTime" runat="server" MaxLength="10" CssClass="input1"
                        Height="18px" Width="124px" AutoPostBack="True" onfocus="setday(this,document.all.txtEndTime)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:Button ID="EditButton" runat="server" CssClass="Button" Text="搜索" OnClick="EditButton_Click"
                        Width="40px" />
                </td>
            </tr>
        </table>
        <radg:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" GridLines="None" OnPageIndexChanged="RadGrid1_PageIndexChanged"
            PageSize="8" Skin="Monochrome" Width="1500px">
            <HeaderStyle Height="28px" />
            <ExportSettings>
                <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                    PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
            </ExportSettings>
            <PagerStyle HorizontalAlign="Right" NextPageText="下一页" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条."
                PrevPageText="上一页" />
            <MasterTableView AutoGenerateColumns="False" DataKeyNames="TG001">
                <NoRecordsTemplate>
                    没有找到任何数据。
                </NoRecordsTemplate>
                <ExpandCollapseColumn Resizable="False" Visible="False">
                    <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
                <Columns>
                    <radg:GridTemplateColumn AllowFiltering="False" UniqueName="TemplateColumndelete">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbAll" runat="server" AutoPostBack="True" OnCheckedChanged="cbAll_CheckedChanged"
                                Text="全选" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="checkbox" runat="server" />
                        </ItemTemplate>
                    </radg:GridTemplateColumn>
                    <radg:GridTemplateColumn AllowFiltering="False" UniqueName="TemplateColumn">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnChildTable" runat="server" ImageUrl="../images/jpg/Update.gif"
                                OnClick="btnChildTable_Click" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            明细显示
                        </HeaderTemplate>
                        <ItemStyle CssClass="selectButton" />
                    </radg:GridTemplateColumn>
                    <radg:GridBoundColumn DataField="TG001" HeaderText="单别" ReadOnly="True" SortExpression="TG001"
                        UniqueName="TG001">
                    </radg:GridBoundColumn>
                    <radg:GridBoundColumn DataField="TG002" HeaderText="单号" SortExpression="KHMC" UniqueName="TG002">
                    </radg:GridBoundColumn>
                    <radg:GridBoundColumn DataField="TG003" HeaderText="销货日期" SortExpression="TG003"
                        UniqueName="TG002">
                    </radg:GridBoundColumn>
                    <radg:GridBoundColumn DataField="MA003" HeaderText="客户全称" SortExpression="SZDWXZ"
                        UniqueName="TGADDRESS">
                    </radg:GridBoundColumn>
                    <radg:GridBoundColumn DataField="TG008" HeaderText="送货地址1">
                    </radg:GridBoundColumn>
                    <radg:GridBoundColumn DataField="TG009" HeaderText="送货地址2">
                    </radg:GridBoundColumn>
                    <radg:GridBoundColumn DataField="TG020" HeaderText="备注">
                    </radg:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </radg:RadGrid>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Width="100%" ScrollBars="Both" Height="100%">
        <div id="divChildTable" visible="true" runat="server" style="width: 100%">
            <table width="100%">
                <tr>
                    <td>
                        <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" Text="确定" />
                        <asp:Button ID="Btnhide" runat="server" Text="隐藏主表" OnClick="Btnhide_Click" />
                        <input id="btnClose" type="button" onclick="window.close()" value="关闭" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input runat="server" id="hiddanbie" type="hidden" /><input id="hiddanhao" type="hidden"
                            runat="server" /><input id="hidshadress" runat="server" type="hidden" />
                        <asp:Panel ID="Panel1" runat="server" Width="800px" ScrollBars="Both" Height="250px">
                            <radg:RadGrid ID="RadGrid2" runat="server" GridLines="None" Skin="Monochrome" PageSize="200"
                                OnPageIndexChanged="RadGrid1_PageIndexChanged" Width="1500px">
                                <HeaderStyle Height="28px"></HeaderStyle>
                                <ExportSettings>
                                    <Pdf PageWidth="8.5in" PageRightMargin="" PageFooterMargin="" PageLeftMargin="" PageTopMargin=""
                                        PageHeight="11in" PageBottomMargin="" PageHeaderMargin=""></Pdf>
                                </ExportSettings>
                                <PagerStyle NextPageText="下一页" HorizontalAlign="Right" PagerTextFormat=" {4} &amp;nbsp;|&amp;nbsp; 当前页 {0} / {1}, 当前记录{2} 到 {3} 共 {5}条."
                                    PrevPageText="上一页"></PagerStyle>
                                <MasterTableView AutoGenerateColumns="False">
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
                                        <radg:GridBoundColumn DataField="TH017" HeaderText="批次">
                                        </radg:GridBoundColumn>
                                        <radg:GridBoundColumn DataField="XIAOHUODANHAO" HeaderText="销货单号" ReadOnly="True"
                                            UniqueName="TG001">
                                        </radg:GridBoundColumn>
                                        <radg:GridBoundColumn DataField="TH004" HeaderText="品号">
                                        </radg:GridBoundColumn>
                                        <radg:GridBoundColumn DataField="TH005" HeaderText="产品名称">
                                        </radg:GridBoundColumn>
                                        <radg:GridBoundColumn DataField="TH006" HeaderText="规格">
                                        </radg:GridBoundColumn>
                                        <radg:GridBoundColumn DataField="TH009" HeaderText="单位">
                                        </radg:GridBoundColumn>
                                        <radg:GridBoundColumn DataField="XHSL" HeaderText="数量">
                                        </radg:GridBoundColumn>
                                        <radg:GridBoundColumn DataField="TH018" HeaderText="备注">
                                        </radg:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </radg:RadGrid>
                        </asp:Panel>
                        <input runat="server" id="Hidden1" type="hidden" /><input id="Hidden2" type="hidden"
                            runat="server" /><input id="Hidden3" runat="server" type="hidden" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    </form>

    <script type="text/javascript">
        
        function SetParentValue()
        {
                    
              window.close;
                           
        } 
        
        
    </script>

</body>
</html>
