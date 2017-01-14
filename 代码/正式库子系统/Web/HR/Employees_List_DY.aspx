<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Employees_List_DY.aspx.cs" Inherits="Web_HR_Employees_List_DY" %>

<%@ Register assembly="RadTabStrip.Net2" namespace="Telerik.WebControls" tagprefix="radTS" %>

<%@ Register assembly="Infragistics2.WebUI.UltraWebGrid.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid" tagprefix="igtbl" %>
<%@ Register assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" tagprefix="igxl" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

table {
	font-size: 9pt;
}


TD {FONT-SIZE: 12px;}

TH {FONT-SIZE: 12px;}
.button, button 
{
	border-left: 1px solid #D5D5D5;
	border-top: 1px solid #D5D5D5;
	border-bottom: 1px solid #C2C2C2;
	border-right: 1px solid #C2C2C2;
	font-family: 宋体;
		font-size:12px;
		font-weight:bold;
		color:#666666;
	background-image:url('../../../../../../../../../Images/buttonBg.gif');
	background-position:50% top;
		background-color:white;
		width:50px;
		vertical-align:middle;
     	text-align :center;
	}


        button
        {
            border-left: 1px solid #D5D5D5;
            border-top: 1px solid #D5D5D5;
            border-bottom: 1px solid #C2C2C2;
            border-right: 1px solid #C2C2C2;
            font-family: 宋体;
            font-size: 12px;
            font-weight: bold;
            color: #666666;
            background-image: url('../images/buttonBg.gif');
            background-position: 50% top;
            background-color: white;
            width: 80px;
            vertical-align: middle;
            text-align: center;
        }
        </style>
            <link href="../../css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
            height: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <radTS:RadTabStrip ID="RadTabStrip2" runat="server" Skin="Default2006">
     <Tabs>
        <radTS:Tab ID="Tab1" runat="server" Text="在职人员信息查询(党员信息)">
            </radTS:Tab>
     </Tabs>
     </radTS:RadTabStrip>
     
     
        <igtbl:UltraWebGrid ID="UltraWebGrid1" runat="server" 
            EnableAppStyling="True" Height="80%" StyleSetName="LucidDream" 
            Width="100%" Browser="Xml" EnableTheming="True">
            <Bands>
                <igtbl:UltraGridBand>
                    <AddNewRow View="NotSet" Visible="NotSet">
                    </AddNewRow>
                    <RowTemplateStyle BackColor="Window" BorderColor="Window" BorderStyle="Ridge">
                        <BorderDetails WidthBottom="3px" WidthLeft="3px" WidthRight="3px" 
                            WidthTop="3px" />
                    </RowTemplateStyle>
                </igtbl:UltraGridBand>
            </Bands>
            <DisplayLayout AllowColSizingDefault="Free" AllowColumnMovingDefault="OnClient" 
                AllowSortingDefault="OnClient" BorderCollapseDefault="Separate" 
                FixedHeaderIndicatorDefault="Button" HeaderClickActionDefault="NotSet" 
                Name="UltraWebGrid1" RowHeightDefault="25px" RowSizingDefault="Free" 
                SelectTypeCellDefault="Single" SelectTypeColDefault="Single" 
                SelectTypeRowDefault="Single" StationaryMargins="Header" 
                StationaryMarginsOutlookGroupBy="True" TableLayout="Fixed" 
                UseFixedHeaders="True" Version="4.00" ViewType="OutlookGroupBy" 
                CellTitleModeDefault="Always" EnableClientSideRenumbering="True" ScrollBar="Always">
                <FrameStyle BackColor="Window" BorderColor="InactiveCaption" 
                    BorderStyle="Solid" BorderWidth="1px" Font-Names="Microsoft Sans Serif" 
                    Font-Size="8.25pt" Height="80%" Width="100%">
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
                        Width="200px">
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
 
 
 
        
  <igxl:UltraWebGridExcelExporter ID="UltraWebGridExcelExporter1" runat='server'>
        </igxl:UltraWebGridExcelExporter>

 
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
         Text="导出党员信息" Width="181px" />
     
     
    </form>
</body>
</html>
