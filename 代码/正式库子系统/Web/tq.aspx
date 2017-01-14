<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tq.aspx.cs" Inherits="tq" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>内容显示页</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<style type="text/css">
html,body{ 
margin-left: 0px;
	margin-top: 10px;
	line-height:150%;
	color: #333;
	font: 12px/1.5 tahoma,arial,宋体B8B\4F53;
	}

.dahongzi {
	font-size: 16px;
	font-weight: bolder;
	color: #ca0100;
}
.huisezi {
	font-size: 9pt;
	color: #999999;
}
.dalanzi {
	font-size: 14px;
	font-weight: bolder;
	color: #557aae;
}
.xiaolanzi {
	font-size: 9pt;
	font-weight: normal;
	color: #003785;
}
.huizebiankuang {
	border: 1px solid #bbbbbb;
}
.huizebiankuang2 {
	border: 1px solid #d8dfe3;
}


.xuxiakuang {
	border-top-width: 0px;
	border-right-width: 0px;
	border-bottom-width: 1px;
	border-left-width: 0px;
	border-top-style: none;
	border-right-style: none;
	border-bottom-style: dashed;
	border-left-style: none;
	border-top-color: #aad4ee;
	border-right-color: #aad4ee;
	border-bottom-color: #aad4ee;
	border-left-color: #aad4ee;
}
a { color:#444; text-decoration:none;}
a:hover { color:#f00;}
</style>

</head>
<body style="background-color:#deecef;height:100%" >
    <form id="form1" runat="server">
    <br />   <br />
<table width="860px" border="0" align="center" cellpadding="0" cellspacing="0" height="57px">
  <tr>
    <td><img src="tianqi/tianqi_logo.png"  style=" border:0px;" /></td>
  </tr>
</table>
<div style=" height:6px;"></div>
<table width="860px"  border="0" align="center" cellpadding="10" cellspacing="0" class="huizebiankuang" bgcolor="#FFFFFF" style="white-space:nowrap; ">
  <tr>
    <td  align="left" valign="middle" class="dahongzi xuxiakuang"><asp:Label 
            ID="titlemain" runat="server" Text="系统错误，暂时无法获取数据！"></asp:Label>
      </td>
    <td  align="right" valign="middle" class="huisezi xuxiakuang" width="300px">本气象信息数据来自&ldquo;中国天气网&rdquo;</td>
  </tr>
  <tr>
    <td align="left" valign="middle">
    <table border="0" cellpadding="0" cellspacing="0">
    <tr>
    <td>更换城市：&nbsp;&nbsp;
    </td>
    <td> <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
            onselectedindexchanged="DropDownList1_SelectedIndexChanged">
    </asp:DropDownList>
    </td>
    <td>&nbsp;&nbsp;<asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" 
            onselectedindexchanged="DropDownList2_SelectedIndexChanged">
    </asp:DropDownList>
    </td>
    <td>&nbsp;&nbsp;&nbsp;&nbsp;
       <asp:LinkButton 
            ID="LB_moren" runat="server" onclick="LB_moren_Click" >设为默认城市</asp:LinkButton>
    </td>
    </tr>
    </table>

   
  
       </td>
    <td align="right" valign="middle">

   
       
        
      </td>
  </tr>
  <tr>
    <td = colspan="2"  align="left"><table  border="0" cellpadding="10" cellspacing="0">
      <tr>
       
        <td  align="center" valign="middle">
            <asp:Image ID="top_tu" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  />
          </td>
        <td align="center" valign="middle" class="xiaolanzi" style=" font-size:14px; font-weight:bolder; color:#006DA2;"><asp:Label ID="top_tq" runat="server" Text=""  style="white-space:nowrap; "></asp:Label></td>
        <td  align="left" valign="middle" class="huisezi"><asp:Label ID="top_other" runat="server" Text=""  style="white-space:nowrap; "></asp:Label>
        </td>

      </tr>
    </table></td>
  </tr>
</table>
<div style=" height:6px;"></div>
<table width="860px" border="0" align="center" cellpadding="0" cellspacing="1"  bgcolor="#d8dfe3">
  <tr>
    <td height="25" align="center" valign="middle" bgcolor="#FFFFFF">
        <asp:Label 
                ID="riqi1" runat="server"></asp:Label>
          </td>
    <td align="center" valign="middle" bgcolor="#FFFFFF">
        <asp:Label 
                ID="riqi2" runat="server"></asp:Label>
          </td>
    <td align="center" valign="middle" bgcolor="#FFFFFF">
        <asp:Label 
                ID="riqi3" runat="server"></asp:Label>
          </td>
    <td align="center" valign="middle" bgcolor="#FFFFFF">
        <asp:Label 
                ID="riqi4" runat="server"></asp:Label>
          </td>
    <td align="center" valign="middle" bgcolor="#FFFFFF">
        <asp:Label 
                ID="riqi5" runat="server"></asp:Label>
          </td>
    <td align="center" valign="middle" bgcolor="#FFFFFF">
        <asp:Label 
                ID="riqi6" runat="server"></asp:Label>
          </td>
  </tr>
  <tr height="80">
    <td  bgcolor="#FFFFFF">
    <table width="100%"  border="0" cellpadding="0" cellspacing="0">
      <tr height="80" >
        <td align="center" valign="middle"><asp:Image ID="Image1" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
        <td align="center" valign="middle"><asp:Image ID="Image2" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">白天：<asp:Label 
                ID="Label1" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">夜间：<asp:Label 
                ID="yj1" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="wd1" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="fx1" runat="server" Text=""></asp:Label>
          </td>
      </tr>
    </table>
    <div style=" height:10px;"></div>
    </td>
    <td bgcolor="#FFFFFF">
        <table width="100%"   border="0" cellpadding="0" cellspacing="0">
      <tr height="80">
        <td align="center" valign="middle"><asp:Image ID="Image3" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
        <td align="center" valign="middle"><asp:Image ID="Image4" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">白天：<asp:Label 
                ID="Label2" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">夜间：<asp:Label 
                ID="yj2" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="wd2" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="fx2" runat="server" Text=""></asp:Label>
          </td>
      </tr>
    </table><div style=" height:10px;"></div>
    </td>
    <td bgcolor="#FFFFFF">
        <table width="100%"   border="0" cellpadding="0" cellspacing="0">
      <tr height="80">
        <td align="center" valign="middle"><asp:Image ID="Image5" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
        <td align="center" valign="middle"><asp:Image ID="Image6" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">白天：<asp:Label 
                ID="Label3" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">夜间：<asp:Label 
                ID="yj3" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="wd3" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="fx3" runat="server" Text=""></asp:Label>
          </td>
      </tr>
    </table><div style=" height:10px;"></div>
    </td>
    <td bgcolor="#FFFFFF">
        <table width="100%"   border="0" cellpadding="0" cellspacing="0">
      <tr height="80">
        <td align="center" valign="middle"><asp:Image ID="Image7" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
        <td align="center" valign="middle"><asp:Image ID="Image8" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">白天：<asp:Label 
                ID="Label4" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">夜间：<asp:Label 
                ID="yj4" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="wd4" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="fx4" runat="server" Text=""></asp:Label>
          </td>
      </tr>
    </table><div style=" height:10px;"></div>
    </td>
    <td bgcolor="#FFFFFF">
        <table width="100%"   border="0" cellpadding="0" cellspacing="0">
      <tr height="80">
        <td align="center" valign="middle"><asp:Image ID="Image9" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
        <td align="center" valign="middle"><asp:Image ID="Image10" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">白天：<asp:Label 
                ID="Label5" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">夜间：<asp:Label 
                ID="yj5" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="wd5" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="fx5" runat="server" Text=""></asp:Label>
          </td>
      </tr>
    </table><div style=" height:10px;"></div>
    </td>
    <td bgcolor="#FFFFFF">
        <table width="100%"   border="0" cellpadding="0" cellspacing="0">
      <tr height="80">
        <td align="center" valign="middle"><asp:Image ID="Image11" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
        <td align="center" valign="middle"><asp:Image ID="Image12" runat="server" BorderStyle="None" ImageUrl="tianqi/main/a4.gif"  /></td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">白天：<asp:Label 
                ID="Label6" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle" class="huisezi">夜间：<asp:Label 
                ID="yj6" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="wd6" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center" valign="middle">
            <asp:Label 
                ID="fx6" runat="server" Text=""></asp:Label>
          </td>
      </tr>
    </table><div style=" height:10px;"></div>
    </td>
  </tr>
</table>
    </form>
</body>
</html>