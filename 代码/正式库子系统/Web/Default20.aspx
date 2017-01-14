<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default20.aspx.cs" Inherits="Default20" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
<!--
body
{
    background-position: left top; left: 0px; background-image: url(../images/jpg/back.GIF); width: 167px; background-repeat: repeat-x; position: absolute; top:-16px; height: 23px;
}
.fonts{ font-weight:bold;font-family:Verdana, Arial, Helvetica, sans-serif;color:Yellow}/*数量文字*/
.STYLE2 {font-size: 12px}
-->
</style>
    <script type="text/javascript" language="javascript" >
    function realload()
    {
        setTimeout("",5000);
        window.location.reload();
        //window.parent.frames('leftFrame').location.reload();
            //document.getElementById('txtflag').value = '2';
            //document.form1.submit();
    } 
    </script>
</head>
<body onload="setTimeout('realload()',5000); ">
    <form id="form1" runat="server">
    <div style="border: 0px;  color: #ff0000;  background-color: transparent; left: 0px; width: 167px; position: absolute; top: 0px; height: 23px;">
        <a  href="select_warnings.aspx" target="rightFrame" style="TEXT-DECORATION: none"><span class="STYLE2" style="color:White">提醒</span><span class="fonts"><%=RemindCount%></span></a>
        &nbsp;&nbsp; 
        <a  href="select_warnings.aspx" target="rightFrame" style="TEXT-DECORATION: none"><span class="STYLE2" style="color:White">报警</span><span class="fonts"><%=WarnCount%></span></a></div>
    </form>
</body>
</html>
