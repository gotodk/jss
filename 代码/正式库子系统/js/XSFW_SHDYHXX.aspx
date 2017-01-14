<%@ Page Language="C#" AutoEventWireup="true" CodeFile="XSFW_SHDYHXX.aspx.cs" Inherits="js_XSFW_SHDYHXX" %>

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


//根据城市弹出该城市的终端用户信息选择窗口（送货单信息填写页面）
function openSelect_YHXX()
{
    var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;
    ShowDialog('../Web/XSFW/SHDYHXX.aspx?cs='+encodeURI(cs),'800','600'); 
}

//根据城市弹出服务商信息选择窗口（送货单信息填写页面）
function openSelect_FWSXX()
{
     var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;   
    ShowDialog('../Web/XSFW/SHDFWSXX.aspx?cs='+encodeURI(cs),'800','600');  
}

//根据城市弹出终端客户选择窗口（客户收货信息中的用户编号弹窗）
function openSelect_XSFW_SHRXX()
{
    var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;
    ShowDialog('../Web/XSFW/KHSHRYHXX.aspx?cs='+encodeURI(cs),'800','600');  
}

$(document).ready(function(){
    changeCity();   //页面载入后改变默认城市
    $("#SSBSC").change(function(){changeCity();});  //城市下拉框中发生改变时，自动恢复到默认城市
    $("#btnYHBH").attr("onclick","");  //去掉订单选择的onclick事件
    $("#btnYHBH").click(function(){openSelect_YHXX()});  //重新设置onclick，将其指向一个新的选择页面 
    
    $("#btnFWSBH").attr("onclick","");  //去掉订单选择的onclick事件
    $("#btnFWSBH").click(function(){openSelect_FWSXX()});  //重新设置onclick，将其指向一个新的选择页面 
    
    $("#btnKHBH").attr("onclick","");  //去掉服务商选择的onclick事件
    $("#btnKHBH").click(function(){openSelect_XSFW_SHRXX()});  //重新设置onclick，将其指向一个新的选择页面  
}); 