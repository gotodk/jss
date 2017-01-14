<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sel_zlfkCity.aspx.cs" Inherits="js_sel_zlfkCity" %>

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
    for(var i=0;i<opts.length;i++){
    if(opts[i].value == '<%=BM%>' ){
        opts[i].selected = true;        
	    break;
        }
    }

} 



//根据城市弹出终端客户选择窗口
function openSelect_ZDYH(){
    var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;
    ShowDialog('../Web/zlwtList.aspx?cs='+encodeURI(cs),'800','600');  
}
//根据产品类型弹出打印机类型选择窗口
function openSelect_DYJLX(){
    var cpxh=document.getElementById("CPXH").value;
    ShowDialog('../Web/DYJXHList.aspx?cpxh='+encodeURI(cpxh),'800','600');  
}

$(document).ready(function(){
    changeCity();   //页面载入后改变默认城市
    $("#SSBSC").change(function(){changeCity();});  //城市下拉框中发生改变时，自动恢复到默认城市
    $("#btnKHBH").attr("onclick","");  //去掉服务商选择的onclick事件
    $("#btnKHBH").click(function(){openSelect_ZDYH()});  //重新设置onclick，将其指向一个新的选择页面   
    $("#btnKHSYJX").attr("onclick","");  //去掉客户使用机型的onclick事件
    $("#btnKHSYJX").click(function(){openSelect_DYJLX()});  //重新设置onclick，将其指向一个新的选择页面 
}); 