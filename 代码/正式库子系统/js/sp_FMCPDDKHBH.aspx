<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sp_FMCPDDKHBH.aspx.cs" Inherits="js_sp_FMCPDDKHBH" %>
<%
    //获取帐号信息
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
//过滤城市(刘杰2009-06-01)
function openSelectWindow1(){
    var cityname='<%=BM %>'    
    ShowDialog('../Web/XSLC/FMCPDDKHBH.aspx?cs='+cityname,'800','600');  
}

$(document).ready(function(){
    $("#btnCUST_ID").attr("onclick","");  //去掉客户选择的onclick事件
    $("#btnCUST_ID").click(function(){openSelectWindow1()});  //重新设置onclick，将其指向一个新的选择页面
  
}); 

