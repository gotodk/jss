<%@ Page Language="C#" AutoEventWireup="true" CodeFile="View_Detail.aspx.cs" Inherits="View_Detail" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看详情</title>
    <link type="text/css" rel="Stylesheet" href="../../css/style.css" />

    <script type="text/javascript" src="../../js/tigra_tables.js"></script>

    <script type="text/javascript" src="../../js/pageTurn.js"></script>

    <script type="text/javascript" src="../../Dialog.js"></script>

    <base target="_self" />

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
<body style="font-size: 12px; background-color: #FBFCFE">
    <form id="form1" runat="server">
    <table width="100%"><tr><td align=center>
        <asp:Label ID="llbPostTitle" runat="server" Font-Bold="True" 
            Font-Size="Larger" ></asp:Label>
        <br />
        </td></tr></table>
    <div style="width: 100%; height: 100%; border-width: 0px; text-align: center">
        <div id="subtable_gaoji" runat="server" style="width: 100%; height: 100%; border-width: 0px;
            text-align: center; vertical-align: middle">
            <table width="100%">
                <tr>
                    <td align="right">
                        &nbsp;
                    </td>
                    <td align="right" style="width: 50%">
                        &nbsp;
                    </td>
                </tr>
            </table>
            &nbsp;</div>
        <div id="table" runat="server" style="width: 100%; height: 100%; border-width: 0px;
            text-align: center">
        </div>
        <div runat="server" id="sele" style="width: 600px; height: 100%; display: none; position: relative;
            top: -3cm">
        </div>
        <asp:TextBox ID="txtmodule" runat="server" Width="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="txtnumber" runat="server" Width="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="IsPass" runat="server" Width="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="note" runat="server" Width="0" Style="display: none"></asp:TextBox>
    </div>
    </form>
</body>
</html>
