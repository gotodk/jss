<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sel_CSZXJHWXQJHB.aspx.cs" Inherits="js_Sel_CSZXJHWXQJHB" %>
<%
    string userName=User.Identity.Name;
    
    
    System.Data.SqlClient.SqlDataReader dr=FMOP.DB.DbHelperSQL.ExecuteReader("select bm from HR_Employees where number='"+userName+"'");
    
    if (!dr.HasRows)
    {
        dr.Close();
        Response.End();
    }
    dr.Read();
    string BM = dr[0].ToString();
    dr.Close();
 %>
$(document).ready(function(){
    var cs= document.getElementById("CSZX") ;
    var opts =cs.options;
    for(var i=0;i<opts.length;i++){
    if(opts[i].value == '<%=BM %>' ){
        opts[i].selected = true;
	    break;
        }
    };
    
    $("#CSZX").change( function() {  var cs= document.getElementById("CSZX") ;
    var opts =cs.options;
    for(var i=0;i<opts.length;i++){
    if(opts[i].value == '<%=BM %>' ){
        opts[i].selected = true;
	    break;
        }
    };
});
    
});


