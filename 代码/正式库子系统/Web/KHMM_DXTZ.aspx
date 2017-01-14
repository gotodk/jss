<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KHMM_DXTZ.aspx.cs" Inherits="Web_KHMM_DXTZ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../css/style.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table align="center">
     <tr>
            <td height="30" align="center" valign="middle">客户手机号码：</td>
            <td colspan="4"><asp:TextBox ID="sjhm" runat="server" Width="100px"  ></asp:TextBox></td>
         </tr> 
     <tr>
            <td height="30" align="center" valign="middle">短信内容：</td>
            <td colspan="4"><asp:TextBox ID="txtDXNR" runat="server" Width="560px" TextMode="MultiLine" Rows="5"  onKeyUp="textCounter(this,'baifenbi123',60)"></asp:TextBox></td>
         </tr>  
         <tr>
           <td height="30" align="left" valign="middle">&nbsp;</td>
           <td align="left" valign="middle" >
              <asp:Button ID="Btnsave" runat="server" Text="发送" OnClick="Btnsave_Click" 
                   CssClass="button"   OnClientClick="javascript:return confirm('确定发送短信提醒！');" 
                   Width="64px" />                 <asp:Button ID="BtnQX" runat="server" Text="取消"  CssClass="button" Height="20px"  Width="64px" OnClick="BtnQX_Click" />    
           </td>
           <td align="left" valign="middle" colspan="3"><div id="baifenbi123"></div><script> textCounter(document.getElementById("txtDXNR"), "baifenbi123", 60)</script>
           </td>

         </tr>
    </table>
    </div>
    </form>
</body>
</html>
