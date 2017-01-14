<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YGXX_Print.aspx.cs" Inherits="Web_HR_YGXX_Print" %>
<%@ Register assembly="RadTabStrip.Net2" namespace="Telerik.WebControls" tagprefix="radTS" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
     <radTS:RadTabStrip ID="RadTabStrip2" runat="server" Skin="Default2006">
     <Tabs>
        <radTS:Tab ID="Tab1" runat="server" Text="在职员工档案查询" NavigateUrl="../HR/Employees_List.aspx"></radTS:Tab>        
     </Tabs>
     </radTS:RadTabStrip>        
    <div>            
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
             DisplayGroupTree="False" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" Height="1120px" Width="100%"/>
        &nbsp;

    </div>
    </form>
</body>
</html>
