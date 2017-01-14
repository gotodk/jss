<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sel_kgzrd.aspx.cs" Inherits="js_Sel_kgzrd" %>
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



//根据转让方弹出
function openSelect_FWS(){
    var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;
    ShowDialog('../Web/FWSListAndDKR.aspx?cs='+encodeURI(cs),'800','600');  
}
//根据求购方弹出
function openSelect_FWSQY(){
    var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;
    ShowDialog('../Web/FWSListAndDKRall.aspx?cs='+encodeURI(cs),'800','600');  
}

//根据合格旧硒鼓弹出
function openSelect_HG(){
    var csDropDown= document.getElementById("ZRLX");
    var zrfbh= document.getElementById("ZRFKHBH").value;
    var cs=csDropDown.options[csDropDown.selectedIndex].text;
    ShowDialog('../Web/HGJXGList.aspx?lx='+encodeURI(cs)+'&bh='+encodeURI(zrfbh),'800','600');  
}

$(document).ready(function(){
    changeCity();   //页面载入后改变默认城市
    
    //电子地图服务商列表中的服务商信息弹窗
    $("#SSBSC").change(function(){changeCity();});  //城市下拉框中发生改变时，自动恢复到默认城市
    $("#btnZRFKHBH").attr("onclick","");  //去掉转让方选择的onclick事件
    $("#btnZRFKHBH").click(function(){openSelect_FWS()});  //重新设置onclick，将其指向一个新的选择页面
    $("#btnQGFKHBH").attr("onclick","");  //去掉求购方选择的onclick事件
    $("#btnQGFKHBH").click(function(){openSelect_FWSQY()});  //重新设置onclick，将其指向一个新的选择页面   

    $("#btnZRXGXH").attr("onclick","");  //去掉合格旧硒鼓选择的onclick事件
    $("#btnZRXGXH").click(function(){openSelect_HG()});  //重新设置onclick，将其指向一个新的选择页面   
}); 