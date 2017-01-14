<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FWPT_TYGLQ.aspx.cs" Inherits="js_FWPT_TYGLQ" %>

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
//体验鼓领取表中服务商信息领取人信息弹窗

//根据城市弹出服务商信息选择窗口（送货单信息填写页面）
function openSelect_FWSXX()
{
     var csDropDown= document.getElementById("SSBSC");
    var cs=csDropDown.options[csDropDown.selectedIndex].text;   
    ShowDialog('../Web/FWPT/TYGLQ_FWSXX.aspx?cs='+encodeURI(cs),'800','600');  
}


$(document).ready(function(){
    changeCity();   //页面载入后改变默认城市
    $("#SSBSC").change(function(){changeCity();});  //城市下拉框中发生改变时，自动恢复到默认城市   
    
    $("#btnFWSBH").attr("onclick","");  //去掉订单选择的onclick事件
    $("#btnFWSBH").click(function(){openSelect_FWSXX()});  //重新设置onclick，将其指向一个新的选择页面     
   
})