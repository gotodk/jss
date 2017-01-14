<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jsERPXHD.aspx.cs" Inherits="js_jsERPXHD" %>
function openSelectWindow(){   
       ShowDialog('../Web/ERPXhdShow.aspx','800','600');   
}

$(document).ready(function(){
    $("#btnYDXZ").attr("onclick","");  //去掉客户选择的onclick事件
    $("#btnYDXZ").click(function(){openSelectWindow()});  //重新设置onclick，将其指向一个新的选择页面 
  
});  
