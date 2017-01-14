<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sel_CSYG.aspx.cs" Inherits="js_sel_CSYG" %>


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
 


//根据城市弹出工号选择窗口
function openSelectWindow_gh(){
    var cityname='<%=BM %>'    
    ShowDialog('../Web/CSYGList.aspx?cs='+cityname,'800','600');  
}

//根据城市弹出身份证号选择窗口
function openSelectWindow_sfzh(){
    var cityname='<%=BM %>'    
    ShowDialog('../Web/CSSFZList.aspx?cs='+cityname,'800','600');  
}

$(document).ready(function(){
    
    $("#btnYGGH").attr("onclick","");  //去掉工号选择的onclick事件
    $("#btnYGGH").click(function(){openSelectWindow_gh()});  //重新设置onclick，将其指向一个新的选择页面
    $("#btnSFZH").attr("onclick","");  //去掉身份证号选择的onclick事件
    $("#btnSFZH").click(function(){openSelectWindow_sfzh()});  //重新设置onclick，将其指向一个新的选择页面
}); 

