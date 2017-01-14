<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FWPT_TYGKHFK.aspx.cs" Inherits="js_FWPT_TYGKHFK" %>

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



//根据城市弹出服务商选择窗口
function openSelect_FWS(){
    var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;
    ShowDialog('../Web/FWPT/TYGKHFK_TYGLQXQ.aspx?cs='+encodeURI(cs),'800','600');  
}

$(document).ready(function(){
    changeCity();   //页面载入后改变默认城市
    
    //体验鼓客户信息反馈中的服务商领取体验鼓弹窗
    $("#SSBSC").change(function(){changeCity();});  //城市下拉框中发生改变时，自动恢复到默认城市
    $("#btnFWSBH").attr("onclick","");  //去掉服务商选择的onclick事件
    $("#btnFWSBH").click(function(){openSelect_FWS()});  //重新设置onclick，将其指向一个新的选择页面
}); 