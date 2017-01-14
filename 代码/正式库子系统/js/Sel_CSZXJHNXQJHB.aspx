<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sel_CSZXJHNXQJHB.aspx.cs" Inherits="js_Sel_CSZXJHNXQJHB" %>
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
    var csDropDown= document.getElementById("CS") ;
    var cs=csDropDown.options[csDropDown.selectedIndex].text;
    var hyDropDown= document.getElementById("HY") ;
    var hy=hyDropDown.options[hyDropDown.selectedIndex].text;
    ShowDialog('../Web/KHList.aspx?cs='+cs+'&hy='+hy,'800','600');  
}

$(document).ready(function(){
    changeCity();   //页面载入后改变默认城市
    $("#CS").change(function(){changeCity();});  //城市下拉框中发生改变时，自动恢复到默认城市
    $("#btnCSZX_CPYXQJHB_JHLBKHBH").attr("onclick","");  //去掉客户选择的onclick事件
    $("#btnCSZX_CPYXQJHB_JHLBKHBH").click(function(){openSelectWindow()});  //重新设置onclick，将其指向一个新的选择页面
    $("#btnSubTable").click(function(){
         $("#btnCSZX_CPYXQJHB_JHLBKHBH").unbind( "click" ) ;
         $("#btnCSZX_CPYXQJHB_JHLBKHBH").click(function(){openSelectWindow()});
    });
  
}); 


 