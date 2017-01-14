<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sel_CSXSDQX.aspx.cs" Inherits="js_Sel_CSZXJHNXQJHB" %>
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
 
//将城市下拉框选择为登录者所在的城市 
function changeCity(){
    var cs= document.getElementById("CS") ;
    var opts =cs.options;
    for(var i=0;i<opts.length;i++){
    if(opts[i].value == '<%=BM %>' ){
        opts[i].selected = true;

	    break;
        }
    }
} 

//根据行业和城市弹出选择窗口
function openSelectWindow(){
    var cityname='<%=BM %>'    
    ShowDialog('../Web/XSLC/CXorder.aspx?cs='+cityname,'800','600');  
}

$(document).ready(function(){
    $("#btnDDDH").attr("onclick","");  //去掉客户选择的onclick事件
    $("#btnDDDH").click(function(){openSelectWindow()});  //重新设置onclick，将其指向一个新的选择页面
  
}); 

 