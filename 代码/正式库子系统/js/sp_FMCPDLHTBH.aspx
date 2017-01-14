<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sp_FMCPDLHTBH.aspx.cs" Inherits="js_sp_FMCPDLHTBH" %>
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

//过滤城市(刘杰2009-06-01)
function openSelectWindow(){
    var cityname='<%=BM %>'    
    ShowDialog('../Web/XSLC/FMCPDDHTBH.aspx?cs='+cityname,'800','600');  
}

$(document).ready(function(){
    $("#btnSSHTBH").attr("onclick","");  //去掉客户选择的onclick事件
    $("#btnSSHTBH").click(function(){openSelectWindow()});  //重新设置onclick，将其指向一个新的选择页面
  
}); 
