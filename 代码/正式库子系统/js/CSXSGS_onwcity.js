var selectname = "CSXSGS";//ÏÂÀ­¿òÃû³Æ
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