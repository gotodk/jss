// JScript 文件


//城市销售公司登录
var selectname = "CSXSGS";//下拉框名称
window.onload  = function () {
$.ajax({
    url: '../web/owncitylist.aspx',
    type: 'GET',
    dataType: 'html',
    timeout: 5000,
    error: function(){
        alert('Error loading');
    },
    success: function(html){
    if(html != "nothing"){
    document.getElementById(selectname).options.length = 0;   
    jsAddItemToSelect(document.getElementById(selectname),html,html);
        }
    }
});
}




function jsAddItemToSelect(objSelect, objItemText, objItemValue) {        
      
        var varItem = new Option(objItemText, objItemValue);      
        objSelect.options.add(varItem);     
 
       
}  

//根据行业和城市弹出选择窗口
function openSelectWindow(){
    var csDropDown= document.getElementById("CSXSGS") ;
    var cs=encodeURI(csDropDown.options[csDropDown.selectedIndex].text);
    ShowDialog('../Web/HKJH/XSDList.aspx?cs='+cs,'800','600');  
}

$(document).ready(function(){
    $("#btnXSGS_YDXSHKJH_MXBXSDH").attr("onclick","");  //去掉客户选择的onclick事件
    $("#btnXSGS_YDXSHKJH_MXBXSDH").click(function(){openSelectWindow()});  //重新设置onclick，将其指向一个新的选择页面
    $("#btnSubTable").click(function(){
         $("#btnXSGS_YDXSHKJH_MXBXSDH").unbind( "click" ) ;
         $("#btnXSGS_YDXSHKJH_MXBXSDH").click(function(){openSelectWindow()});
    });
  
}); 


var ff_sptime = "DHSJ";
var ff_sptime_b = "yes";
var ff_sptime_b_1 = "XSGS_YDXSHKJH_MXBBQYHKSJ";
var ff_sptime_b_2 = "5555555";
var ff_sptime_b_3 = "55555555";
var ff_sptime_b_4 = "55555";
