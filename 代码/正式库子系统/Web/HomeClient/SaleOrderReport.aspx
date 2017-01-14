<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleOrderReport.aspx.cs" Inherits="Web_HomeClient_SalerOrderReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>


<html>
<head runat="server">
    <title>无标题页</title>
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="../../css/style.css" type="text/css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="0px" Skin="Default2006"
            Width="397px">
            <Tabs>
                <radTS:Tab runat="server" Text="打印销售单">
                </radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        <table class="FormView" width="100%"><tr><td style="height: 51px">
        &nbsp;<strong>注意：</strong>此模块仅供条码打印测试。如果销售单右上角不能显示条码，请<a href="BarCode.exe" target="_blank">点此</a>安装条码显示支持程序。<br />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;如果销售单无法正常打印，请<a href="ReportActiveX.aspx"  target="_blank">点此</a>安装报表打印程序
        </td></tr></table>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" HasCrystalLogo="False" HasDrillUpButton="False" HasToggleGroupTreeButton="False" HasViewList="False" HasZoomFactorList="False" PrintMode="ActiveX" ReportSourceID="CrystalReportSource1" DisplayGroupTree="False" Height="1106px" Width="876px" />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FMOPConn %>"
            SelectCommand="SELECT * FROM [CSZX_XSD_HWLB] WHERE ([parentNumber] = @parentNumber)">
            <SelectParameters>
                <asp:QueryStringParameter Name="parentNumber" QueryStringField="Number" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="SaleOrderReport.rpt">
                <DataSources>
                    <CR:DataSourceRef DataSourceID="SqlDataSource1" TableName="CSZX_XSD_HWLB" />
                </DataSources>
                <Parameters>
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="Number" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="DDH" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="KHJL" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="FKFS" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="KHBH" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="KHMC" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="DZ" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="LXR" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="LXDH" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="JEZJ" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="createUser" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="FHR" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="THSHR" PropertyName="" ReportName="" />
                    <CR:ControlParameter ControlID="" ConvertEmptyStringToNull="False" DefaultValue=""
                        Name="RMBJE" PropertyName="" ReportName="" />
                </Parameters>
            </Report>
        </CR:CrystalReportSource>
    
    </div>
    </form>
</body>
</html>
