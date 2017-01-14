<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkFlow_View_Detail.aspx.cs" Inherits="WorkFlow_View_Detail" %>

<%@ Register Assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" TagPrefix="igxl" %>

<%@ Register Assembly="Infragistics2.WebUI.UltraWebGrid.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid" TagPrefix="igtbl" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>查看详情</title>
    <link type="text/css" rel="Stylesheet" href="../css/style.css" />
    <script type="text/javascript" src="../js/tigra_tables.js"></script>
    <script type="text/javascript" src="../js/pageTurn.js"></script>
    <script type="text/javascript" src="Dialog.js"></script>
    
    <base target ="_self" />
    <script type="text/javascript" language="javascript">
    function disp()
    {
        document.getElementById('sele').style.display = 'block';
    }
    function nodisp()
    {
        document.getElementById('sele').style.display = 'none';
    }
    function passed()
    {
        document.getElementById('IsPass').value = 'true';
        document.form1.submit();
    }
    function unpass()
    {
        var ss = document.getElementById('remark').value;
        if(ss==null||ss=="")
        {
            alert("请填写审批意见！");
            return;
        }
        document.getElementById('IsPass').value = 'false';
        
        document.form1.submit();
    }

    window.onerror = function() {
        return true;
    } 
    </script>
</head>
<body style="font-size:12px; background-color:#FBFCFE">
    <form id="form1" runat="server">
    <div style="width:100%;height:100%;border-width:0px;text-align:center">
    <div id="subtable_gaoji" runat="server" style="width:100%;height:100%;border-width:0px; text-align:center; vertical-align:middle">
        <table width="100%"><tr><td align="right"><asp:Label ID="Label_title" runat="server" Visible="false"></asp:Label></td><td align="right" style="width:50%"><asp:Button 
            ID="btnOut" runat="server" onclick="Button1_Click" Text="导出Excel" Visible="False" 
            Width="120px" /></td></tr></table> 
 
&nbsp;<igtbl:UltraWebGrid ID="UltraWebGrid1" runat="server"
            Height="500px" Width="90%" Visible="False" oninitializelayout="UltraWebGrid1_InitializeLayout" 
            >
            <Bands>
                <igtbl:UltraGridBand>
                    <AddNewRow View="NotSet" Visible="NotSet">
                    </AddNewRow>
                    <RowEditTemplate>
                        <p align="right">
                        </p>
                        <br>
                            <p align="center">
                                <input id="igtbl_reOkBtn" onclick="igtbl_gRowEditButtonClick(event);" 
                                    style="width:50px;" type="button" value="OK">
                                    &nbsp;
                                    <input id="igtbl_reCancelBtn" onclick="igtbl_gRowEditButtonClick(event);" 
                                        style="width:50px;" type="button" value="Cancel">
                                    </input>
                                </input>
                            </p>
                        </br>
                    </RowEditTemplate>
                    <RowTemplateStyle BackColor="Ivory" BorderColor="Ivory" BorderStyle="Ridge">
                        <BorderDetails WidthBottom="3px" WidthLeft="3px" WidthRight="3px" 
                            WidthTop="3px" />
                    </RowTemplateStyle>
                </igtbl:UltraGridBand>
            </Bands>
            <DisplayLayout AllowColSizingDefault="Free" AllowColumnMovingDefault="OnServer" 
                AllowSortingDefault="OnClient" BorderCollapseDefault="Separate" 
                FixedHeaderIndicatorDefault="Button" HeaderClickActionDefault="SortMulti" 
                Name="UltraWebGrid1" RowHeightDefault="20px" 
                RowSizingDefault="Free" StationaryMarginsOutlookGroupBy="True" 
                TableLayout="Fixed" UseFixedHeaders="True" Version="4.00" 
                ViewType="OutlookGroupBy" AllowUpdateDefault="Yes">
                <FrameStyle BackColor="Ivory" BorderStyle="Solid" BorderWidth="1px" 
                    Font-Names="Verdana" Font-Size="8pt" Height="500px" Width="90%">
                </FrameStyle>
                <RowAlternateStyleDefault BackColor="#FFFFC0">
                    <Padding Left="3px" />
                    <BorderDetails ColorLeft="255, 255, 192" ColorTop="255, 255, 192" />
                </RowAlternateStyleDefault>
                <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                </EditCellStyleDefault>
                <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
                </FooterStyleDefault>
                <HeaderStyleDefault BackColor="LightGray" BorderStyle="Solid">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
                </HeaderStyleDefault>
                <RowStyleDefault BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" 
                    Font-Names="Verdana" Font-Size="8pt">
                    <Padding Left="3px" />
                    <BorderDetails ColorLeft="White" ColorTop="White" />
                </RowStyleDefault>
                <AddNewBox>
                    <BoxStyle BackColor="LightGray" BorderStyle="Solid" 
                        BorderWidth="1px">
                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                            WidthTop="1px" />
                    </BoxStyle>
                </AddNewBox>
                <ActivationObject BorderColor="Black" BorderWidth="">
                </ActivationObject>
                <FilterOptionsDefault AllowRowFiltering="OnClient" FilterUIType="FilterRow">
                    <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" 
                        BorderWidth="1px" CustomRules="overflow:auto;" 
                        Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px" 
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
        </div>
        <div id="table" runat="server" style="width:100%;height:100%;border-width:0px; text-align:center">
         
            <igxl:UltraWebGridExcelExporter ID="UltraWebGridExcelExporter1" runat='server'  OnInitializeRow="UltraWebGridExcelExporter1_InitializeRow" OnBeginExport="UltraWebGridExcelExporter1_BeginExport">
            
            </igxl:UltraWebGridExcelExporter>

        </div>
        <div runat="server" id="sele" style="width: 600px;height:100%;display:none; position:relative;top:-3cm">
        </div>
        <asp:TextBox ID="txtmodule" runat="server" Width="0" style="display:none"></asp:TextBox>
        <asp:TextBox ID="txtnumber" runat="server" Width="0" style="display:none"></asp:TextBox>
        <asp:TextBox ID="IsPass" runat="server" Width="0" style="display:none"></asp:TextBox>
        <asp:TextBox ID="note" runat="server" Width="0" style="display:none"></asp:TextBox>
      </div>
    </form>
</body>
</html>
