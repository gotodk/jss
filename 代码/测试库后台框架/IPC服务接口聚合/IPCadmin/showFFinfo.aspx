<%@ Page Language="C#" AutoEventWireup="true" CodeFile="showFFinfo.aspx.cs" Inherits="IPCadmin_showFFinfo" %>

<%@ Register Src="wuccaidan.ascx" TagName="wuccaidan" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>IPC管理中心--方法详情</title>
    <link href="css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:wuccaidan ID="wuccaidan1" runat="server" />
            <div>
                <br />
                <strong>方法详情：<a href="editFF.aspx?FF_guid=<%=Request["FF_guid"].ToString() %>" target="_self">修改方法资料</a></strong>
                <table>
                    <tr>
                        <td></td>
                    </tr>
                </table>
                <div runat="server" id="showFF"></div>
                <br />

            </div>
        </div>
    </form>
</body>
</html>
