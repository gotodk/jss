var FormUtil = new Object;

FormUtil.focusOnFirst = function () {
    if (document.forms.length > 0) {
        for (var i=0; i < document.forms[0].elements.length; i++) {
            var oField = document.forms[0].elements[i];
            if (oField.type != "hidden") {
                oField.focus();
                return;
            }
        }
    }
};

FormUtil.setTextboxes = function() {
    var colInputs = document.getElementsByTagName("input");
    var colTextAreas = document.getElementsByTagName("textarea");
        
    for (var i=0; i < colInputs.length; i++){
        if (colInputs[i].type == "text" || colInputs [i].type == "password") {
            colInputs[i].onfocus = function () { this.select(); };
            
        }
    }
        
    for (i=0; i < colTextAreas.length; i++){
        colTextAreas[i].onfocus = function () { this.select(); };
    }
};

  function   Go(oForm,   e)   {   
  if   (!e)   e   =   window.event;   
  var   code   =   e.which   ?   e.which   :   e.keyCode;   
  var   o   =   e.target   ?   e.target   :   e.srcElement;   
  if   (code   ==   13)   {   
  var   oInput   =   oForm.getElementsByTagName("input");   
  for   (var   i=0;   i<oInput.length;   i++)   
  if   (oInput[i]   ==   o)   break;   
  while   (i   !=   oInput.length-1)   {   
  if   (oInput[i   +   1].type   ==   "text")   {   
  oInput[i   +   1].focus();break;   
  }   
  i++;   
  }   
  }   
  }; 

FormUtil.tabForward = function(oTextbox) {

    var oForm = oTextbox.form;

    //make sure the textbox is not the last field in the form
    if (event.keyCode==13) {
        //event.keyCode=null;   
        for (var i=0; i < oForm.elements.length; i++) {
            if (oForm.elements[i] == oTextbox) {
                 for(var j=i+1; j < oForm.elements.length; j++) {
                     if (oForm.elements[j].type != "hidden" && oForm.elements[j].type != "file" ) {
                         oForm.elements[j].focus();
                         return;
                     }
                 }
                 return ;
            }
        }
    }
};




