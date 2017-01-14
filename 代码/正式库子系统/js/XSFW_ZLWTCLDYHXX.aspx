<%@ Page Language="C#" AutoEventWireup="true" CodeFile="XSFW_ZLWTCLDYHXX.aspx.cs" Inherits="js_XSFW_ZLWTCLDYHXX" %>


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


//根据城市弹出该城市的终端用户信息选择窗口（质量问题处理单信息填写页面）
function openSelect_YHXX()
{
    var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;
    ShowDialog('../Web/XSFW/ZLWTCLDYHXX.aspx?cs='+encodeURI(cs),'800','600'); 
}

//根据城市弹出服务商信息选择窗口（质量问题处理单信息填写页面）
function openSelect_FWSXX()
{
     var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;   
    ShowDialog('../Web/XSFW/ZLWTCLDFWSXX.aspx?cs='+encodeURI(cs),'800','600');  
}

$(document).ready(function(){
    changeCity();   //页面载入后改变默认城市
    $("#SSBSC").change(function(){changeCity();});  //城市下拉框中发生改变时，自动恢复到默认城市
    
    $("#btnYHBH").attr("onclick","");  //去掉用户编号选择的onclick事件
    $("#btnYHBH").click(function(){openSelect_YHXX()});  //重新设置onclick，将其指向一个新的选择页面 
    
    $("#btnFWSBH").attr("onclick","");  //去掉服务商编号选择的onclick事件
    $("#btnFWSBH").click(function(){openSelect_FWSXX()});  //重新设置onclick，将其指向一个新的选择页面  

var hxxx= document.getElementById("sprytextfieldCLDH"); 
 var tr3=document .createElement ("tr");
 tr3.setAttribute ("height","25px");
 var td3=document .createElement ("td");
 td3.setAttribute ("align","right"); 
 td3.setAttribute ("className","SubTitle"); 
td3.appendChild (document .createTextNode ("客户反馈质量问题信息"));
tr3.appendChild (td3);
 hxxx.parentNode .parentNode.insertBefore(tr3,null);
 
 
    var wtg= document.getElementById("sprytextareaGZMS"); 
 var tr=document .createElement ("tr");
 tr.setAttribute ("height","25px");
 
<%-- var td=document .createElement ("td");
 td.setAttribute ("align","right"); 
 td.setAttribute ("className","SubTitle"); 
td.appendChild (document .createTextNode ("预出货与核销信息"));
tr.appendChild (td);
 wtg.parentNode .parentNode.insertBefore(tr,null);--%>
 
  
 
 
 var bz=document .getElementById ("sprytextareaBZ");
 var tr2=document .createElement ("tr");
 tr2.setAttribute ("height","30px");
 var td2=document .createElement ("td");
 td2.setAttribute ("align","right");
 td2.setAttribute ("className","SubTitle");
 td2.appendChild (document .createTextNode ("质量问题处理反馈"));
 tr2.appendChild (td2);
 bz.parentNode .parentNode .insertBefore (tr2); 
 
 
}); 



