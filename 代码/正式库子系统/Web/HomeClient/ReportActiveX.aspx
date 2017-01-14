<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportActiveX.aspx.cs" Inherits="Web_HomeClient_ReportActiveX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    请在弹出的安全警告窗口中点击安装按钮，安装完成后，即可关闭本页面
    <div>
        <object id="PrintControl" classid="clsid:B83FC273-3522-4CC6-92EC-75CC86678DA4" height="0" width="0" codebase="PrintControl.cab"></object></div>
    </form>
</body>
</html>