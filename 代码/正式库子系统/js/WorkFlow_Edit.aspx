﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkFlow_Edit.aspx.cs" Inherits="WorkFlow_Edit" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../css/style.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/tigra_tables.js"></script>
    <script type="text/javascript" src="../js/pageTurn.js"></script>
    <script type="text/javascript" src="Dialog.js"></script>
    <script  language="javascript" type ="text/javascript">
    function disp()
    {
        document.getElementById('sele').style.display = 'block';
    }
    function nodisp()
    {
        document.getElementById('sele').style.display = 'none';
    }
    function formsubmit()
    {
        document.getElementById('txtflag').value = '1';
        //getqqq();

        
        document.form1.submit();
    }


    
    </script>
    <style type="text/css">
    #sele{
        position:absolute;
	left:expression((document.body.clientWidth-sele.offsetWidth)/2);
	top:72px;
	width:600px;
	height:100%;
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

        function forhide_f_re() {
            pagesubmit('jump');
        }
        
        <%
    if(Request["zzz"] != null && Request["zzz"].ToString() == "yes")
    {
        %>
        parent.document.getElementById("rightFrame_hide").style.display = "none";
        parent.document.getElementById("rightFrame").style.display = "";
        window.top.frames.rightFrame.forhide_f_re();
        <%
    }
%>
    </script>
    </form>
</body>
</html>



