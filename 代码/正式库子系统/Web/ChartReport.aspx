<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChartReport.aspx.cs" Inherits="ChartReport" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ChartReport</title>
         <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script  language="javascript" type="text/javascript">
    function disp()
    {
      var selDisp=document.getElementById('sele').style;
      selDisp.display = (selDisp.display == 'none') ? 'block' : 'none';
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
    function realload()
    {
        setTimeout("",5000); 
        
        var realed = document.getElementById('realed').checked;
        if(realed)
        {
            //window.location.reload();
            document.getElementById('txtflag').value = '2';
            document.form1.submit();
        }
    }
    
    </script>
</head>
<body onload="setInterval('realload()',5000);">
    <form id="form1" runat="server">
    <div style="width:100%;height:100%;border-width:0px;text-align:center; background-color:White">
        <div style="width:100%; height:100%; text-align:left; font-size:12px;">
            <radTS:RadTabStrip ID="RadTabStrip1" runat="server" Height="0px" Skin="Default2006">
            </radTS:RadTabStrip>   
        </div>
        <table id="one" border="0" cellpadding="0" cellspacing="0" class="FormView" style="width: 100%;text-align:right">
            <tr>
               <td>
                   &nbsp;
                   <asp:CheckBox ID="realed" text="即时刷新" style ="color:#336699"  runat="server"/>
                 &nbsp;&nbsp;| &nbsp;&nbsp; <a href="#" onclick="disp()" style="font-family: 宋体; text-decoration: none;"><font color="#336699">条件查询</font></a> &nbsp; &nbsp; 
                </td>
            </tr>
        </table>
        <table class="FormView" border="0" cellpadding="0" cellspacing="0" width="100%">
          <tr>
            <td>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="450"></rsweb:ReportViewer>
            </td>
          </tr>
        </table>
        
        <div runat="server" id="sele" style="width: 600px;top:-7cm; height: 100%; position: relative;display:none">
        </div>
        <asp:TextBox ID="txtmodule" runat="server" Visible="false"></asp:TextBox><asp:TextBox ID="txtSql" runat="server" Visible="false"></asp:TextBox><asp:TextBox ID="txtflag" runat="server" Width="0" style="display:none"></asp:TextBox>
    </div>
    </form>
</body>
</html>
