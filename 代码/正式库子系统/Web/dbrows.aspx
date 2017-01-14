<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dbrows.aspx.cs" Inherits="Web_dbrows" %>

<%@ Register assembly="Infragistics2.WebUI.UltraWebGrid.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid" tagprefix="igtbl" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

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
	background-image:url('../Images/buttonBg.gif');
	background-position:50% top;
		background-color:white;
		width:50px;
		vertical-align:middle;
     	text-align :center;
	margin-bottom: 0px;
}


    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    

        <igtbl:UltraWebGrid ID="GV_show" runat="server" 
            EnableAppStyling="True" Height="492px" StyleSetName="LucidDream" 
            Width="1017px" EnableTheming="True">
            <Bands>
                <igtbl:UltraGridBand>
                    <AddNewRow View="NotSet" Visible="NotSet">
                    </AddNewRow>
                    <RowTemplateStyle BackColor="Window" BorderColor="Window" BorderStyle="Ridge">
                        <BorderDetails WidthBottom="3px" WidthLeft="3px" WidthRight="3px" 
                            WidthTop="3px" />
<BorderDetails WidthLeft="3px" WidthTop="3px" WidthRight="3px" WidthBottom="3px"></BorderDetails>
                    </RowTemplateStyle>
                </igtbl:UltraGridBand>
            </Bands>
            <DisplayLayout AllowColSizingDefault="Free" AllowColumnMovingDefault="OnServer" BorderCollapseDefault="Separate" 
                FixedHeaderIndicatorDefault="Button" 
                Name="UltraWebGrid1" RowHeightDefault="20px" RowSizingDefault="Free" 
                SelectTypeCellDefault="Single" SelectTypeColDefault="Single" 
                SelectTypeRowDefault="Single" StationaryMargins="Header" 
                StationaryMarginsOutlookGroupBy="True" TableLayout="Fixed" 
                UseFixedHeaders="True" Version="4.00" ViewType="OutlookGroupBy" 
                CellTitleModeDefault="Always" EnableClientSideRenumbering="True" 
                ScrollBar="Always" ColWidthDefault="180px">
                <FrameStyle BackColor="Window" BorderColor="InactiveCaption" 
                    BorderStyle="Solid" BorderWidth="1px" Font-Names="Microsoft Sans Serif" 
                    Font-Size="8.25pt" Height="492px" Width="1017px">
                </FrameStyle>
                <Pager MinimumPagesForDisplay="2">
                    <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
<BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px"></BorderDetails>
                    </PagerStyle>
                </Pager>
                <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                </EditCellStyleDefault>
                <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
<BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px"></BorderDetails>
                </FooterStyleDefault>
                <HeaderStyleDefault BackColor="LightGray" BorderStyle="Solid"  Height="30px"
                    HorizontalAlign="Left">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
                        
<BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px"></BorderDetails>
                        
                </HeaderStyleDefault>
                <RowStyleDefault  BorderStyle="Solid" 
                    BorderWidth="1px"  Font-Size="9pt">
                    <Padding Left="3px" />
                    <BorderDetails ColorLeft="Window" ColorTop="Window" />
<Padding Left="3px"></Padding>

<BorderDetails ColorLeft="Window" ColorTop="Window"></BorderDetails>
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
<BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px"></BorderDetails>
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
<Padding Left="2px"></Padding>
                    </FilterDropDownStyle>
                    <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                    </FilterHighlightRowStyle>
                    <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px" CustomRules="overflow:auto;" 
                        Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px">
                        <Padding Left="2px" />
<Padding Left="2px"></Padding>
                    </FilterOperandDropDownStyle>
                </FilterOptionsDefault>
            </DisplayLayout>
        </igtbl:UltraWebGrid>
 
    </div>
    </form>
</body>
</html>
