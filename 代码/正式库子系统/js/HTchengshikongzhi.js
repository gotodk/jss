





var selectname = "SSBM";//ÏÂÀ­¿òÃû³Æ
var pageUrl = window.location;
 var a = pageUrl.toString().split("/") 
   // alert(a[(a.length-1)]) ;


if(a[(a.length-1)].toLowerCase() == "WorkFlow_Add.aspx?module=HTCDJL".toLowerCase() || a[(a.length-1)].indexOf("WorkFlow_Update.aspx?module=HTCDJL".toLowerCase()) >= 0)
{


 
window.onload = function() {
document.getElementById("btnYHTBH").style.display = "none";
document.getElementById("HTBHLX").onchange=new Function("checklx1()");
    $.ajax({
        url: '../web/HTowncitylist.aspx',
        type: 'GET',
        dataType: 'html',
        timeout: 5000,
        error: function() {
            alert('Error loading');
        },
        success: function(html) {
           
               if (html != "nothing") {
               
                if(html.indexOf("1")>=0)
                {
                   
                }
                else
                {
                 
                 document.getElementById(selectname).options.length = 0;
                 jsAddItemToSelect1(document.getElementById(selectname), html, html);
                  
                }
              }
           


        }
    });

}
}

function jsAddItemToSelect1(objSelect, objItemText, objItemValue) {        
      
        var varItem = new Option(objItemText, objItemValue);      
        objSelect.options.add(varItem);     
 
       
}

function checklx1()
{
var ddllll = document.getElementById("HTBHLX");
var sz = ddllll.options[ddllll.selectedIndex].value;

if(escape(sz) == "%u5408%u540C%u7EED%u7B7E"  || escape(sz) == "%u8865%u5145%u534F%u8BAE" || escape(sz)== "%u5408%u540C%u6362%u7B7E" || escape(sz)== "%u5408%u540C%u89E3%u7EA6")
{
document.getElementById("btnYHTBH").style.display = "";
document.getElementById("YHTBH").value = "";
}
else
{
document.getElementById("btnYHTBH").style.display = "none";
document.getElementById("YHTBH").value = "0";
}



}