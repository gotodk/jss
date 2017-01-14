<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zp_show.aspx.cs" Inherits="Web_HR_zp_show" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>员工照片</title>
</head>
<body topmargin="0" leftmargin="0" ondragstart="return false" onselectstart="return false" style ="background-color:#f7f7f7">
    <form id="form1" runat="server">
    <div id="divFile" runat ="server">
    <table>
    
    <%
          if (result.Tables[0].Rows.Count > 0)
                          {
                              for (int i = 0; i < result.Tables[0].Rows.Count; i++)
                              {
                                  string localName =result.Tables [0].Rows[i]["LocalName"].ToString();                                  
                                  picture = "<img src=../upload/" + localName.ToString() + " />";
                  %>
    <tr align ="center">
      <td align="center" ><%=picture%></td>      
   </tr>
   <tr><td style ="height:50px "></td></tr>
       <%
                          }
                          }       
                       %>
    
    </tabel>
     </div>
    </form>
</body>
</html>
