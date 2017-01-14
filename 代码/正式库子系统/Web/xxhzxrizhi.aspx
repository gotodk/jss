<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xxhzxrizhi.aspx.cs" Inherits="Web_xxhzxrizhi" %>
<%@ Register assembly="Infragistics2.WebUI.UltraWebGrid.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid" tagprefix="igtbl" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="/css/style.css" />
        <link href="/css/supfy.css" rel="Stylesheet" type="text/css" />
            <script src="/js/adddate.js" type="text/javascript"></script>
                <script type="text/javascript" src="/js/pageTurn.js"></script>
    <script type="text/javascript" src="/web/Dialog.js"></script>
        <script language="javascript" type="text/javascript">
        window.onerror = function() {
            return true;
        }
            </script>
</head>
<body>
    <form id="form1" runat="server">

         <radTS:RadTabStrip ID="RadTabStrip2" runat="server" Skin="Default2006">
        <Tabs>
            <radTS:Tab ID="Tab1" runat="server" Text="工作推进浏览" NavigateUrl="xxhzxrizhi.aspx" ForeColor="Red">
            </radTS:Tab>
        </Tabs>
    </radTS:RadTabStrip>
        <table width="1024px" border="0" cellpadding="0" cellspacing="0">
          <tr>
    <td>
      <table border="0" cellpadding="2" cellspacing="2">
  <tr>
    <td>关键词：</td>
    <td>
        <asp:TextBox ID="tb_key" runat="server"></asp:TextBox>
      </td>
  </tr>
</table>
<table border="0" cellpadding="2" cellspacing="2">
  <tr>
    <td>录入时间段从：</td>
    <td>
        <asp:TextBox ID="tm1" runat="server"  onfocus="setday(this,this)" style="readonly:expression(this.readOnly=true) "></asp:TextBox>
      </td>
    <td>到</td>
    <td>
        <asp:TextBox ID="tm2" runat="server"  onfocus="setday(this,this)" style="readonly:expression(this.readOnly=true) "></asp:TextBox>
      </td>
    <td>
        <asp:Button ID="Button1" runat="server" Text="查询" CssClass="button" 
            Height="20px" onclick="Button1_Click" />
      </td>
  </tr>
</table>
    </td>
    </tr>
  <tr>
    <td>
    <igtbl:UltraWebGrid ID="GV_show" runat="server" 
            EnableAppStyling="True" Height="304px" StyleSetName="LucidDream" 
            Width="100%" Browser="Xml" EnableTheming="True">
            <Bands>
                <igtbl:UltraGridBand>
                    <AddNewRow View="NotSet" Visible="NotSet">
                    </AddNewRow>
                    <Columns>
                        
<igtbl:TemplatedColumn AllowRowFiltering="False" AllowGroupBy="No" 
                            BaseColumnName="编号" Format="" Width="60px">
                            <CellTemplate>
                                <input id="txtmodule" type="text" value="XXHZXGZTJJLB" style=" display:none;" /><a href='#' onclick = "openview('<%# DataBinder.Eval(Container, "DataItem.编号") %>');">详情</a> 
                            </CellTemplate>
                            <Header Caption="详情">
                                <RowLayoutColumnInfo OriginX="1" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="1" />
                            </Footer>
                        </igtbl:TemplatedColumn>
                    </Columns>
                    <RowTemplateStyle BackColor="Window" BorderColor="Window" BorderStyle="Ridge">
                        <BorderDetails WidthBottom="3px" WidthLeft="3px" WidthRight="3px" 
                            WidthTop="3px" />
                    </RowTemplateStyle>
                </igtbl:UltraGridBand>
            </Bands>
            <DisplayLayout AllowColSizingDefault="Free" AllowColumnMovingDefault="OnClient" 
                AllowSortingDefault="OnClient" BorderCollapseDefault="Separate" 
                FixedHeaderIndicatorDefault="Button" HeaderClickActionDefault="NotSet" 
                Name="UltraWebGrid1" RowHeightDefault="20px" RowSizingDefault="Free" 
                SelectTypeCellDefault="Single" SelectTypeColDefault="Single" 
                SelectTypeRowDefault="Single" StationaryMargins="Header" 
                StationaryMarginsOutlookGroupBy="True" TableLayout="Fixed" 
                UseFixedHeaders="True" Version="4.00" ViewType="OutlookGroupBy" 
                CellTitleModeDefault="Always" EnableClientSideRenumbering="True" 
                ScrollBar="Always" ColWidthDefault="140px">
                <FrameStyle BackColor="Window" BorderColor="InactiveCaption" 
                    BorderStyle="Solid" BorderWidth="1px" Font-Names="Microsoft Sans Serif" 
                    Font-Size="8.25pt" Height="304px" Width="100%">
                </FrameStyle>
                <Pager MinimumPagesForDisplay="2">
                    <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
                    </PagerStyle>
                </Pager>
                <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                </EditCellStyleDefault>
                <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
                </FooterStyleDefault>
                <HeaderStyleDefault BackColor="LightGray" BorderStyle="Solid"  Height="30px"
                    HorizontalAlign="Left">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
                        
                </HeaderStyleDefault>
                <RowStyleDefault  BorderStyle="Solid" 
                    BorderWidth="1px"  Font-Size="9pt">
                    <Padding Left="3px" />
                    <BorderDetails ColorLeft="Window" ColorTop="Window" />
                </RowStyleDefault>
                <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
                </GroupByRowStyleDefault>
                <GroupByBox>
                    <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                    </BoxStyle>
                </GroupByBox>
                <AddNewBox>
                    <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" 
                        BorderWidth="1px">
                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                            WidthTop="1px" />
                    </BoxStyle>
                </AddNewBox>
                <ActivationObject BorderColor="" BorderWidth="">
                </ActivationObject>
                <FilterOptionsDefault AllowRowFiltering="OnServer" FilterUIType="FilterRow">
                    <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" 
                        BorderWidth="1px" CustomRules="overflow:auto;" 
                        Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px" Height="300px" 
                        Width="250px">
                        <Padding Left="2px" />
                    </FilterDropDownStyle>
                    <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                    </FilterHighlightRowStyle>
                    <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px" CustomRules="overflow:auto;" 
                        Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px">
                        <Padding Left="2px" />
                    </FilterOperandDropDownStyle>
                </FilterOptionsDefault>
            </DisplayLayout>
        </igtbl:UltraWebGrid>
    </td>
  </tr>
  <tr>
    <td>
    <!-- 分页 begin -->
<table border="0" runat="server" id="fy_tb_main" style=" font-size:9pt;" cellpadding="1" cellspacing="1">
  <tr>
    <td  nowrap="nowrap">
        <asp:Button ID="B_sy" runat="server" Text="首页" onclick="B_sy_Click" CssClass="supfy"  />
      </td>
    <td nowrap="nowrap">
        <asp:Button ID="B_syy" runat="server" Text="上一页" onclick="B_syy_Click"  CssClass="supfy" />
      </td>
    <td  runat="server" id="fynumlist" nowrap="nowrap">
        <asp:Button ID="tb_ym1" runat="server" Text="1" OnCommand="B_fynumlist_one_Click" CommandArgument="1" CommandName="Click"  CssClass="supfy" />
        <asp:Button ID="tb_ym2" runat="server" Text="2" OnCommand="B_fynumlist_one_Click" CommandArgument="2" CommandName="Click"  CssClass="supfy" />
        <asp:Button ID="tb_ym3" runat="server" Text="3" OnCommand="B_fynumlist_one_Click" CommandArgument="3" CommandName="Click"  CssClass="supfy" />
        <asp:Button ID="tb_ym4" runat="server" Text="4" OnCommand="B_fynumlist_one_Click" CommandArgument="4" CommandName="Click"  CssClass="supfy" />
        <asp:Button ID="tb_ym5" runat="server" Text="5" OnCommand="B_fynumlist_one_Click" CommandArgument="5" CommandName="Click"  CssClass="supfy" />
        <asp:Button ID="tb_ym6" runat="server" Text="6" OnCommand="B_fynumlist_one_Click" CommandArgument="6" CommandName="Click"  CssClass="supfy" />
        <asp:Button ID="tb_ym7" runat="server" Text="7" OnCommand="B_fynumlist_one_Click" CommandArgument="7" CommandName="Click"  CssClass="supfy" />
        <asp:Button ID="tb_ym8" runat="server" Text="8" OnCommand="B_fynumlist_one_Click" CommandArgument="8" CommandName="Click"  CssClass="supfy" />
        <asp:Button ID="tb_ym9" runat="server" Text="9" OnCommand="B_fynumlist_one_Click" CommandArgument="9" CommandName="Click"  CssClass="supfy" />
        <asp:Button ID="tb_ym10" runat="server" Text="10" OnCommand="B_fynumlist_one_Click" CommandArgument="10" CommandName="Click"  CssClass="supfy" />
      </td>
    <td nowrap="nowrap">
        <asp:Button ID="B_xyy" runat="server" Text="下一页" onclick="B_xyy_Click"  CssClass="supfy" />
      </td>
    <td nowrap="nowrap">
        <asp:Button ID="B_sy0" runat="server" Text="尾页" onclick="B_sy0_Click"  CssClass="supfy" />
      </td>
    <td nowrap="nowrap">跳转到：</td>
    <td nowrap="nowrap">
        <asp:TextBox ID="tb_tz" runat="server" Width="44px" 
            onkeyup="value=value.replace(/[^\d]/g,'');"
            onkeypress=" if ( !((window.event.keyCode >= 48) && (window.event.keyCode <= 57))){window.event.keyCode = 0 ;}" 
            onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))" 
            style="ime-mode:disabled" onkeydown="if(event.keyCode==13)event.keyCode=9" 
            MaxLength="8"></asp:TextBox>
      </td>
    <td nowrap="nowrap">页</td>
    <td nowrap="nowrap">
        <asp:Button ID="tb_gotz" runat="server" Text="跳转" onclick="tb_gotz_Click"   CssClass="supfy"  /></td>
    <td id="fymsg" runat="server" nowrap="nowrap">共1页,共2条数据,每页10条。</td>
  </tr>
</table>


<!-- 分页 end --> 
    </td>
  </tr>
</table>

    </form>
</body>
</html>
