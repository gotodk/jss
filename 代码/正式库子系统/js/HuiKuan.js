

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
    ShowDialog('../Web/HKGL/HKD_XSD.aspx?cs='+cs,'800','600');  
}

$(document).ready(function(){
    $("#btnXSDH").attr("onclick","");  //去掉客户选择的onclick事件
    $("#btnXSDH").click(function(){openSelectWindow()});  //重新设置onclick，将其指向一个新的选择页面
}); 
