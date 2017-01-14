<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddError.aspx.cs" Inherits="Web_AddError" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
        .style1
        {
            width: 50%;
        }
        .style2
        {
            width: 564px;
        }
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
       <div style="">
      <table width="80%">
         <tr style="width:100%">
           <td>
               <light>模块代码</light>
           </td>
           <td class="style1">
               <asp:Label ID="Labmod" runat="server" Text=""></asp:Label>
           </td>
           
         </tr>
         <tr>
           <td>Number:
           </td>
           <td class="style2" >
               <asp:Label ID="LabNumber" runat="server" Text=""></asp:Label>
           </td>
         </tr>
         <tr style="width:100%">
           <td class="style1" colspan="2">
               <asp:Label ID="Worning" runat="server" Text="提示:"></asp:Label>
           </td>
           
         </tr>
      </table>
     </div>
     
     <div align="center">
                    <!--闪动  begin -->
                    <span id=flashFont></span>
                    <script>
 var colorkey = 1;
 var c = new Array();
 c[1] = '#FF0000';
 c[2] = '#333366';
 function chg_Font_Cont(){ 
  if ( colorkey<c.length-1 ) {
   colorkey += 1;
  } else {
   colorkey = 1;
  }
  colorcode = c[colorkey];
  eval("document.getElementById('flashFont').innerHTML = '<a href=down/per_smk.zip class=a06 target=_blank><font color="+colorcode+">"+"短信助手客户端软件下载"+"</font></a><br>'");
  setTimeout("chg_Font_Cont()",600); 
 }
 chg_Font_Cont()
</script>
<!--闪动  end --></div>
    </form>
</body>
</html>
