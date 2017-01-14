<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkFlow_View.aspx.cs" Inherits="WorkFlow_View" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../css/style.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/tigra_tables.js"></script>
    <script type="text/javascript" src="../js/pageTurn.js"></script>
    <script type="text/javascript" src="Dialog.js"></script>
    <script type="text/javascript"  language="javascript">
    function disp()
    {
        document.getElementById('sele').style.display = 'block';
        //document.getElementById('sele').style.top = 
    }
    function nodisp()
    {
        document.getElementById('sele').style.display = 'none';
    }
    function formsubmit()
    {
        document.getElementById('txtflag').value = '1';
        document.form1.submit();
    }

    
    </script>
    <style type="text/css">
    #sele{
        position:absolute;
	left:expression((document.body.clientWidth-sele.offsetWidth)/2);
	top:72px;
	width:600px;
	height:auto;
	z-index:999;
	display:none;
    }



    </style>
</head>
<body  style=" background-color:#FBFCFE">
    <form id="form1" runat="server">
    <div style="width:100%;height:100%;border-width:0px;text-align:center">
        <div style="width:100%; height:100%; text-align:left; font-size:12px;">
            <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="100%" Skin="Default2006">
            </radTS:RadTabStrip>   
        </div>
        <div runat="server" id="sele">
        <!-- 查询条件 -->
        </div>
        <div id ="table" runat="server" style="width:100%; height:100%; border:solid 0px #ffffff; text-align:center" >
        <!--数据列表 -->
        </div>
        
        <div style="width:100%; height:100%; text-align:right; font-size:12px;">
                <a id="begin" href="#" onclick="pagesubmit('begin')" style="text-decoration: none;">首页</a>&nbsp;&nbsp;
                <a id="previous" href="#" onclick="pagesubmit('previous')" style="text-decoration: none;">上一页</a>&nbsp;&nbsp;
                <a id="next" href="#" onclick="pagesubmit('next')" style="text-decoration: none;">下一页</a>&nbsp;&nbsp;
                <a id="end" href="#" onclick="pagesubmit('end')" style="text-decoration: none;">尾页</a>&nbsp;&nbsp;
                <a href="#" onclick="pagesubmit('jump')" style="text-decoration: none;">跳转</a>至
                <asp:TextBox ID="jumppage" runat="server" Width="35px"></asp:TextBox>/
                <asp:TextBox ID="txtpagecount" runat="server" Width="35px" ReadOnly="true">"></asp:TextBox>页
        </div>
        
    <asp:TextBox ID="txtmodule" runat="server" BackColor="Aqua" Text="0" style="display:none"></asp:TextBox>
    <asp:TextBox ID="txtflag" runat="server" BackColor="Aqua" Text="0" style="display:none"></asp:TextBox>
    <asp:TextBox ID="txtpagenumber" runat="server" Width="0px" style ="display:none ">"></asp:TextBox>
    </div>
    <script language="javascript" type ="text/javascript">
    pageDisable();
    </script>
    </form>
</body>
</html>
