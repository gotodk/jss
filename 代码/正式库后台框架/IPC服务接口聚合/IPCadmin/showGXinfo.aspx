<%@ Page Language="C#" AutoEventWireup="true" CodeFile="showGXinfo.aspx.cs" Inherits="IPCadmin_showGXinfo" %>

<%@ Register Src="wuccaidan.ascx" TagName="wuccaidan" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>IPC管理中心--接口关系详情</title>
    <link href="css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:wuccaidan ID="wuccaidan1" runat="server" />
            <br />
            <br />
            <strong>接口关系详情：<a href="editGX.aspx?GX_guid=<%=Request["GX_guid"].ToString() %>" target="_self">修改接口关系数据</a></strong>
            <%--  <strong>接口关系详情：<a href="javascript:alert('后续实现，先人工处理！');">修改接口关系数据</a></strong>--%>
            <br />
            <br />
            <div runat="server" id="gxinfo"></div>
            <br />

        </div>
    </form>
</body>
</html>
