<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCFWJGDetail.ascx.cs" Inherits="Web_JHJX_New2013_UCFWJG_UCFWJGDetail" %>
<%--  <script src="../../../jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../../../js/standardJSFile/jquery.art_confirm.js" type="text/javascript"></script>--%>
<script type="text/javascript">
  
    $(function () {
        var Nunm = "#<%=this.ClientID.ToString()%>_txtGXTW";
      
        $(Nunm).art_confirm(FunName<%=this.ClientID.ToString()%>);
      
    });

 
    var FunName<%=this.ClientID.ToString()%> = {
        name: "diag1",
        width: 680,
        height: 510,
        title: "高校名称",
        url: "JHJX/New2013/UCFWJG/GXTWDetail.aspx",
        showmessagerow: false,
        showbuttonrow: false,
        optionName: "FunName<%=this.ClientID.ToString()%>",
        IsReady: function (object, dialog) {
            __artConfirmOperner.close();
            var Nunm = "#" + "<%=this.ClientID.ToString()%>" + "_txtGXTW";
           
            $(Nunm).val(object.GXMC);
        }
    }
 
    </script>
<style type="text/css">
    .Back
    {
          background-color:#D3E0F0;
    }
</style>
             <div>
                  <table cellpadding="0" cellspacing="0" >
<tr>
            <td> <span style="background-color:#D3E0F0;"> <asp:Label ID="lblYWGLBMFL" runat="server"><span id="spanYWGLBMFL" runat="server"  style="background-color:#D3E0F0;">业务管理部门分类：</span> </asp:Label><asp:DropDownList ID="ddlYWGLBMFL" CssClass="tj_input" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlYWGLBMFL_SelectedIndexChanged">
</asp:DropDownList></span>
             <asp:Label ID="lblYWGLBM" runat="server"><span id="spanYWGLBM" runat="server" style="background-color:#D3E0F0;">业务管理部门：</span> </asp:Label>
       
              <asp:DropDownList ID="ddlYWGLBM" CssClass="tj_input" runat="server">
</asp:DropDownList>
       
           <asp:Label ID="lblGXTW" runat="server">  <span  id="spanGXTW" runat="server" style="background-color:#D3E0F0;">高校团委：</span></asp:Label>
             <asp:TextBox ID="txtGXTW"  class="tj_input" runat="server" Text="请选择" Width="120px" CssClass="tj_input tj_input_search" onfocus="this.blur()"></asp:TextBox>
   </td> </tr>
                      </table>
                       </div>
        






