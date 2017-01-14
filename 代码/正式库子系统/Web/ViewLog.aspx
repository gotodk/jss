<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewLog.aspx.cs" Inherits="Web_ViewLog" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>


<%@ Register src="yhb_BigPage.ascx" tagname="yhb_BigPage" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body style ="margin:0px">
    <form id="form1" runat="server">
    <div>
  <uc1:yhb_BigPage ID="yhb_BigPage1" runat="server" Page_size="20" />
    </div>
    </form>
</body>
</html>
