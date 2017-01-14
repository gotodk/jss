<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YHSEDYULAN.aspx.cs" Inherits="Web_YHSEDYULAN" %>


<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form2" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
            ReportSourceID="CrystalReportSource1" DisplayGroupTree="False" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" Height="1120px" Width="765px" PrintMode="ActiveX" />
        &nbsp;
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="CPXSD.rpt">
                <DataSources>
                    <CR:DataSourceRef DataSourceID="SqlDataSource1" TableName="YXSYB_CPXSD" />
                    <CR:DataSourceRef DataSourceID="SqlDataSource2" TableName="YXSYB_CPXSD_DHYD" />
                </DataSources>
            </Report>
        </CR:CrystalReportSource>
    
    </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FMOPConn %>"
            SelectCommand="select o.*,(select count(Number) from YXSYB_CPXSD where KHBH  =o.KHBH and CreateTime<o.CreateTime )+1 as cNumber from YXSYB_CPXSD o where number=@number">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="" Name="number" QueryStringField="number" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:FMOPConn %>"
            SelectCommand="SELECT c.id, c.parentNumber, c.CPBM, c.CPPL, c.CPXH, c.XHFL, c.DYJPP, c.DYJXH, c.HCXH, c.GG, c.YS, c.DJ, c.SL, c.DW, c.JE, c.BPFHSL, c.BPFHJE, p.ZCXZ FROM YXSYB_CPXSD_DHYD AS c INNER JOIN system_Products AS p ON c.CPBM = p.Number where c.parentNumber=@parentNumber" ProviderName="<%$ ConnectionStrings:FMOPConn.ProviderName %>">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="" Name="parentNumber" QueryStringField="number" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>




<!--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:100%; text-align:center; margin-top:30px">
      <table style="width:95%; border-width:1px; border-color:Black " cellpadding="3px" cellspacing="0" border="1">
      <tr>
        <td align="center" colspan="2">  <font size = "5"><strong>山东富美科技有限公司---城市销售公司要货申请单</strong></font>
           
        </td>
      </tr>
         <tr>
           <td style=" width:20%">要货申请单号：</td>
           <td align="left"><asp:Label runat="server" ID="lblyhsqcode" Text="要货申请单号"></asp:Label></td>
         </tr>
        <tr>
           <td style=" width:30%">申请单位：</td>
           <td align="left"> <asp:Label runat="server" ID="lblSQDW"></asp:Label></td>
         </tr>
         <tr>
           <td style=" width:30%">收获地址：</td>
           <td align="left"><asp:Label runat="server" ID="lblSHadress"></asp:Label></td>
         </tr>
          <tr>
           <td style=" width:30%">收货人：</td>
           <td align="left"><asp:Label runat="server" ID="lblSHR"></asp:Label></td>
         </tr>
         
        <tr>
           <td style=" width:30%">联系电话：</td>
           <td align="left"><asp:Label runat="server" ID="lblPhoneNum"></asp:Label></td>
         
          </tr>
        <tr>
           <td style=" width:30%">要求到货时间：</td>
           <td align="left"><asp:Label runat="server" ID="lblyqdhtime"></asp:Label></td>
         </tr>
         <tr>
          <td colspan="2">
             <table style="width:100%; border-width:1px; border-color:Black " cellpadding="1px" cellspacing="0" border="1">
               <tr> 
                    <td rowspan="2">
                        序号 
                    </td>
                     <td rowspan="2">
                       产品性质
                    </td>
                     <td rowspan="2">
                       销售单号
                    </td>
                     <td rowspan="2">
                       产品名称
                    </td>
                     <td rowspan="2">
                       产品编码
                    </td>
                     <td rowspan="2">
                       产品型号
                    </td>
                     <td rowspan="2">
                       单价（元）
                    </td>
                     <td colspan="2">
                      本次申请
                    </td>
                     <td rowspan="2">
                       备注
                    </td>
               </tr>
                <tr> 
                    <td>
                      
                       数量</td>
                     <td>
                     
                       金额
                                         
                    </td>
                  
               </tr>
             </table>
          </td>
         </tr>
      </table>-->
    </div>
    </form>
</body>
</html>
