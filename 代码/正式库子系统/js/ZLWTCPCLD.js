




function jsAddItemToSelect(objSelect, objItemText, objItemValue) {        
      
        var varItem = new Option(objItemText, objItemValue);      
        objSelect.options.add(varItem);     
 
       
}  

 //根据质量问题产品处理单号弹出选择窗口
function openSelectWindow(){
    ShowDialog('../Web/THH/ZLWTCPCLDList.aspx','800','600');  
}

$(document).ready(function(){
    $("#btnZLWTCPCLDH").attr("onclick","");  //去掉客户选择的onclick事件
    $("#btnZLWTCPCLDH").click(function(){openSelectWindow()});  //重新设置onclick，将其指向一个新的选择页面
}); 
