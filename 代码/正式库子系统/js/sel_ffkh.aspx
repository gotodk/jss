<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sel_ffkh.aspx.cs" Inherits="js_sel_ffkh" %>

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
 


//根据城市弹出客户选择窗口
function openSelectWindow_khbh(){
    var cityname='<%=BM %>'    
    ShowDialog('../Web/FFKH_List.aspx?cs='+cityname,'800','600');  
}


$(document).ready(function(){
    
    $("#btnKHBH").attr("onclick","");  //去掉客户编号选择的onclick事件
    $("#btnKHBH").click(function(){openSelectWindow_khbh()});  //重新设置onclick，将其指向一个新的选择页面
   
}); 

