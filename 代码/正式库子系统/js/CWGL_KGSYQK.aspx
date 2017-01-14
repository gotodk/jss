<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CWGL_KGSYQK.aspx.cs" Inherits="js_CWGL_KGSYQK" %>

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
    var cityname='<%=BM%>';
    var cs= document.getElementById("SSBSC") ;
    var opts =cs.options;
    if(cityname.indexOf("办事处")>=0)
    {
         cs.length=0;
         cs.options[cs.length]=new Option(cityname,cityname);
    }

} 



//根据城市弹出该城市的订单选择窗口
function openSelect_ERPDD(){
    var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;
     var djlxDropDown= document.getElementById("DJLX");
    var djlx=djlxDropDown.options[djlxDropDown.selectedIndex].text;
    if(djlx=="订单")
    {
        ShowDialog('../Web/CWGL/sel_ERPDD.aspx?cs='+encodeURI(cs),'800','600');  
    }
    if(djlx=="销货单")
    {
        ShowDialog('../Web/CWGL/sel_ERPXHD.aspx?cs='+encodeURI(cs),'800','600');
    }
      
}

//根据城市弹出回收的空鼓选择窗口
function openSelect_ERPKGHS(){
     var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;   
    ShowDialog('../Web/CWGL/sel_ERPKGHS.aspx?cs='+encodeURI(cs),'800','600');  
}

function repeat(){
     $("#btnCWGL_KHKGSYQKJLB_SYKGXXKGPH").attr("onclick","");  //去掉空鼓回收选择的onclick事件    
     document.getElementById("btnCWGL_KHKGSYQKJLB_SYKGXXKGPH").onclick =openSelect_ERPKGHS; 
   <%-- $("#btnCWGL_KHKGSYQKJLB_SYKGXXHSLB").click(function(){openSelect_ERPKGHS()});  //重新设置onclick，将其指向一个新的选择页面 --%>
    setTimeout("repeat()","1000");   
}

$(document).ready(function(){
    changeCity();   //页面载入后改变默认城市
    $("#SSBSC").change(function(){changeCity();});  //城市下拉框中发生改变时，自动恢复到默认城市
    $("#btnKHBH").attr("onclick","");  //去掉订单选择的onclick事件
    $("#btnKHBH").click(function(){openSelect_ERPDD()});  //重新设置onclick，将其指向一个新的选择页面     
    repeat();   
      
}); 