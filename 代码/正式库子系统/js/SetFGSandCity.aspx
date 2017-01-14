<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetFGSandCity.aspx.cs" Inherits="js_SetFGSandCity" %>


<%
    //获取帐号信息
    string userName=User.Identity.Name;
    System.Data.SqlClient.SqlDataReader dr=FMOP.DB.DbHelperSQL.ExecuteReader("select bm,dyfgsmc  from HR_Employees as a left join system_city as b on a.bm=b.name where a.number='"+userName+"'");
    if (!dr.HasRows)
    {
        dr.Close();
        Response.End();
    }
    dr.Read();
    string BM = dr[0].ToString();
    string FGS = dr[1].ToString();
    dr.Close();
    
    
 %>
 
//将办事处下拉框选择为登录者所在的办事处
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

//将分公司下拉框选择为登录者所在的分公司
function changeFGS(){
    var fgsname='<%=FGS%>';
    var fgs= document.getElementById("SSFGS") ;
    var opts =fgs.options;
    for(var i=0;i<opts.length;i++){
    if(opts[i].value == '<%=FGS%>' ){
        opts[i].selected = true;        
	    break;
        }
    }

} 


$(document).ready(function(){
    changeCity();   //页面载入后改变默认城市    
    //电子地图服务商列表中的服务商信息弹窗
    $("#SSBSC").change(function(){changeCity();});  //城市下拉框中发生改变时，自动恢复到默认城市   
    
     changeFGS();   //页面载入后改变默认城市    
    //电子地图服务商列表中的服务商信息弹窗
    $("#SSFGS").change(function(){changeFGS();});  //城市下拉框中发生改变时，自动恢复到默认城市   
}); 