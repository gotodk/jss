/* --- BoxOver ---
/* --- v 2.1 17th June 2006
By Oliver Bryant with help of Matthew Tagg
http://boxover.swazz.org */
if (typeof document.attachEvent != 'undefined') {
   window.attachEvent('onload',init);
   document.attachEvent('onmousemove',moveMouse);
   document.attachEvent('onclick',checkMove); }

else {
   window.addEventListener('load',init,false);
   document.addEventListener('mousemove',moveMouse,false);
   document.addEventListener('click',checkMove,false);
}

var oDv=document.createElement("div");
var dvHdr=document.createElement("div");
var dvBdy=document.createElement("div");
var windowlock,boxMove,fixposx,fixposy,lockX,lockY,fixx,fixy,ox,oy,boxLeft,boxRight,boxTop,boxBottom,evt,mouseX,mouseY,boxOpen,totalScrollTop,totalScrollLeft;
boxOpen=false;
ox=10;
oy=10;
lockX=0;
lockY=0;

function init() {
	oDv.appendChild(dvHdr);
	oDv.appendChild(dvBdy);
	oDv.style.position="absolute";
	oDv.style.visibility = 'hidden';

	document.body.appendChild(oDv);
}

function defHdrStyle() {
    dvHdr.innerHTML = '<img  style="vertical-align:middle"  src="info.gif">&nbsp;&nbsp;' + dvHdr.innerHTML;
    if (document.all) { //IE
        dvHdr.style.fontWeight = 'bold';
        dvHdr.style.width = '500px';
        dvHdr.style.height = '30px';
        dvHdr.style.border = '1px solid  #1468B2';
        dvHdr.style.fontFamily = "宋体";
        dvHdr.style.fontSize = '14px';
        dvHdr.style.color = '#FFFFFF';
        dvHdr.style.padding = "0px 5px 0px 5px";
        dvHdr.style.background = 'url(images/loginNew/bg20120829_03.png) repeat-x';
        dvHdr.style.filter = 'alpha(opacity=100)'; // IE
        dvHdr.style.opacity = '1'; // FF
    }
    else {  //FF
        dvHdr.style.fontWeight = 'bold';
        dvHdr.style.width = '500px';
        dvHdr.style.height = '30px';
        dvHdr.style.border = '1px solid #1468B2';
        dvHdr.style.fontFamily = "宋体";
        dvHdr.style.fontSize = '14px';
        dvHdr.style.color = '#FFFFFF';
        dvHdr.style.padding = "0px 5px 0px 5px";
        dvHdr.style.background = 'url(images/loginNew/bg20120829_03.png) repeat-x';
        dvHdr.style.filter = 'alpha(opacity=100)'; // IE
        dvHdr.style.opacity = '1'; // FF
    }
}

function defBdyStyle() {
    if (document.all) {  //IE
        dvBdy.style.borderBottom = '1px solid #1468B2';
        dvBdy.style.borderLeft = '1px solid #1468B2';
        dvBdy.style.borderRight = '1px solid #1468B2';
        dvBdy.style.width = '500px';
        dvBdy.style.fontSize = '11px';
        dvBdy.style.fontFamily = "宋体";
        dvBdy.style.padding = '8px';
        dvBdy.style.color = '#333333';
        dvBdy.style.background = '#FFFFFF';
        dvBdy.style.padding = "5px";
        dvBdy.style.filter = 'alpha(opacity=100)'; // IE
        dvBdy.style.opacity = '1'; // FF
    }
    else {  //FF
        dvBdy.style.borderBottom = '1px solid #1468B2';
        dvBdy.style.borderLeft = '1px solid #1468B2';
        dvBdy.style.borderRight = '1px solid #1468B2';
        dvBdy.style.width = '500px';
        dvBdy.style.fontSize = '12px';
        dvBdy.style.fontFamily = "宋体";
        dvBdy.style.padding = '8px';
        dvBdy.style.color = '#333333';
        dvBdy.style.background = '#FFFFFF';
        dvBdy.style.padding = "5px";
        dvBdy.style.filter = 'alpha(opacity=100)'; // IE
        dvBdy.style.opacity = '1'; // FF
    
    
    }
}

function checkElemBO(txt) {
if (!txt || typeof(txt) != 'string') return false;
if ((txt.indexOf('header')>-1)&&(txt.indexOf('body')>-1)&&(txt.indexOf('[')>-1)&&(txt.indexOf('[')>-1)) 
   return true;
else
   return false;
}

function scanBO(curNode) {
    curNode.tamade = curNode.tamade ? curNode.tamade : curNode.getAttribute("tamade");
    if (checkElemBO(curNode.tamade)) {
         curNode.boHDR=getParam('header',curNode.tamade);
         curNode.boBDY=getParam('body',curNode.tamade);
			curNode.boCSSBDY=getParam('cssbody',curNode.tamade);			
			curNode.boCSSHDR=getParam('cssheader',curNode.tamade);
			curNode.IEbugfix=(getParam('hideselects',curNode.tamade)=='on')?true:false;
			curNode.fixX=parseInt(getParam('fixedrelx',curNode.tamade));
			curNode.fixY=parseInt(getParam('fixedrely',curNode.tamade));
			curNode.absX=parseInt(getParam('fixedabsx',curNode.tamade));
			curNode.absY=parseInt(getParam('fixedabsy',curNode.tamade));
			curNode.offY=(getParam('offsety',curNode.tamade)!='')?parseInt(getParam('offsety',curNode.tamade)):10;
			curNode.offX=(getParam('offsetx',curNode.tamade)!='')?parseInt(getParam('offsetx',curNode.tamade)):10;
			curNode.fade=(getParam('fade',curNode.tamade)=='on')?true:false;
			curNode.fadespeed=(getParam('fadespeed',curNode.tamade)!='')?getParam('fadespeed',curNode.tamade):0.04;
			curNode.delay=(getParam('delay',curNode.tamade)!='')?parseInt(getParam('delay',curNode.tamade)):0;
			if (getParam('requireclick',curNode.tamade)=='on') {
				curNode.requireclick=true;
				document.all?curNode.attachEvent('onclick',showHideBox):curNode.addEventListener('click',showHideBox,false);
				document.all?curNode.attachEvent('onmouseover',hideBox):curNode.addEventListener('mouseover',hideBox,false);
			}
			else {// Note : if requireclick is on the stop clicks are ignored   			
   			if (getParam('doubleclickstop',curNode.tamade)!='off') {
   				document.all?curNode.attachEvent('ondblclick',pauseBox):curNode.addEventListener('dblclick',pauseBox,false);
   			}	
   			if (getParam('singleclickstop',curNode.tamade)=='on') {
   				document.all?curNode.attachEvent('onclick',pauseBox):curNode.addEventListener('click',pauseBox,false);
   			}
   		}
			curNode.windowLock=getParam('windowlock',curNode.tamade).toLowerCase()=='off'?false:true;
			curNode.tamade='';
			curNode.hasbox=1;
	   }
	   else
	      curNode.hasbox=2;   
}


function getParam(param,list) {
	var reg = new RegExp('([^a-zA-Z]' + param + '|^' + param + ')\\s*=\\s*\\[\\s*(((\\[\\[)|(\\]\\])|([^\\]\\[]))*)\\s*\\]');
	var res = reg.exec(list);
	var returnvar;
	if(res)
		return res[2].replace('[[','[').replace(']]',']');
	else
		return '';
}

function Left(elem) {
	var x=0;
	if (elem.calcLeft)
		return elem.calcLeft;
	var oElem=elem;
	while(elem){
		 if ((elem.currentStyle)&& (!isNaN(parseInt(elem.currentStyle.borderLeftWidth)))&&(x!=0))
		 	x+=parseInt(elem.currentStyle.borderLeftWidth);
		 x+=elem.offsetLeft;
		 elem=elem.offsetParent;
	  } 
	oElem.calcLeft=x;
	return x;
	}

function Top(elem){
	 var x=0;
	 if (elem.calcTop)
	 	return elem.calcTop;
	 var oElem=elem;
	 while(elem){		
	 	 if ((elem.currentStyle)&& (!isNaN(parseInt(elem.currentStyle.borderTopWidth)))&&(x!=0))
		 	x+=parseInt(elem.currentStyle.borderTopWidth); 
		 x+=elem.offsetTop;
	         elem=elem.offsetParent;
 	 } 
 	 oElem.calcTop=x;
 	 return x;
 	 
}

var ah,ab;
function applyStyles() {
	if(ab)
		oDv.removeChild(dvBdy);
	if (ah)
		oDv.removeChild(dvHdr);
	dvHdr=document.createElement("div");
	dvBdy=document.createElement("div");
	CBE.boCSSBDY?dvBdy.className=CBE.boCSSBDY:defBdyStyle();
	CBE.boCSSHDR?dvHdr.className=CBE.boCSSHDR:defHdrStyle();
	dvHdr.innerHTML=CBE.boHDR;
	dvBdy.innerHTML=CBE.boBDY;
	ah=false;
	ab=false;
	if (CBE.boHDR!='') {		
		oDv.appendChild(dvHdr);
		ah=true;
	}	
	if (CBE.boBDY!=''){
		oDv.appendChild(dvBdy);
		ab=true;
	}	
}

var CSE,iterElem,LSE,CBE,LBE, totalScrollLeft, totalScrollTop, width, height ;
var ini=false;

// Customised function for inner window dimension
function SHW() {
   if (document.body && (document.body.clientWidth !=0)) {
      width=document.body.clientWidth;
      height=document.body.clientHeight;
   }
   if (document.documentElement && (document.documentElement.clientWidth!=0) && (document.body.clientWidth + 20 >= document.documentElement.clientWidth)) {
      width=document.documentElement.clientWidth;   
      height=document.documentElement.clientHeight;   
   }   
   return [width,height];
}


var ID = null;

function moveMouse(e) {
//    boxMove=true;
//    alert(e.target.tamade);
  e ? evt = e : evt = event;
    CSE = e.target ? e.target : e.srcElement;
	if (!CSE.hasbox) {
	   // Note we need to scan up DOM here, some elements like TR don't get triggered as srcElement
	    iElem = CSE;
	  
	   while ((iElem.parentNode) && (!iElem.hasbox)) {
	      scanBO(iElem);
	      iElem = iElem.parentNode;
	     
	   }	   
	}
	
	if ((CSE!=LSE)&&(!isChild(CSE,dvHdr))&&(!isChild(CSE,dvBdy))){		
	   if (!CSE.boxItem) {
			iterElem=CSE;
			while ((iterElem.hasbox==2)&&(iterElem.parentNode))
					iterElem=iterElem.parentNode;
			CSE.boxItem = iterElem;
			}
		iterElem=CSE.boxItem;
		if (CSE.boxItem&&(CSE.boxItem.hasbox==1))  {
			LBE=CBE;
			CBE=iterElem;
			if (CBE!=LBE) {
				applyStyles();
				if (!CBE.requireclick)
					if (CBE.fade) {
						if (ID!=null)
							clearTimeout(ID);
						ID=setTimeout("fadeIn("+CBE.fadespeed+")",CBE.delay);
					}
					else {
						if (ID!=null)
							clearTimeout(ID);
						COL=1;
						ID=setTimeout("oDv.style.visibility='visible';ID=null;",CBE.delay);						
					}
				if (CBE.IEbugfix) {hideSelects();} 
				fixposx=!isNaN(CBE.fixX)?Left(CBE)+CBE.fixX:CBE.absX;
				fixposy=!isNaN(CBE.fixY)?Top(CBE)+CBE.fixY:CBE.absY;			
				lockX=0;
				lockY=0;
				boxMove=true;
				ox=CBE.offX?CBE.offX:10;
				oy=CBE.offY?CBE.offY:10;
			}
		}
		else if (!isChild(CSE,dvHdr) && !isChild(CSE,dvBdy) && (boxMove))	{
			// The conditional here fixes flickering between tables cells.
			if ((!isChild(CBE,CSE)) || (CSE.tagName!='TABLE')) {   			
   			CBE=null;
   			if (ID!=null)
  					clearTimeout(ID);
   			fadeOut();
   			showSelects();
			}
		}
		LSE=CSE;
	}
	else if (((isChild(CSE,dvHdr) || isChild(CSE,dvBdy))&&(boxMove))) {
		totalScrollLeft=0;
		totalScrollTop=0;
		
		iterElem=CSE;
		while(iterElem) {
			if(!isNaN(parseInt(iterElem.scrollTop)))
				totalScrollTop+=parseInt(iterElem.scrollTop);
			if(!isNaN(parseInt(iterElem.scrollLeft)))
				totalScrollLeft+=parseInt(iterElem.scrollLeft);
			iterElem=iterElem.parentNode;			
		}
		if (CBE!=null) {
			boxLeft=Left(CBE)-totalScrollLeft;
			boxRight=parseInt(Left(CBE)+CBE.offsetWidth)-totalScrollLeft;
			boxTop=Top(CBE)-totalScrollTop;
			boxBottom=parseInt(Top(CBE)+CBE.offsetHeight)-totalScrollTop;
			doCheck();
		}
	}
	
	if (boxMove&&CBE) {
		// This added to alleviate bug in IE6 w.r.t DOCTYPE
		bodyScrollTop=document.documentElement&&document.documentElement.scrollTop?document.documentElement.scrollTop:document.body.scrollTop;
		bodyScrollLet=document.documentElement&&document.documentElement.scrollLeft?document.documentElement.scrollLeft:document.body.scrollLeft;
		mouseX=evt.pageX?evt.pageX-bodyScrollLet:evt.clientX-document.body.clientLeft;
		mouseY=evt.pageY?evt.pageY-bodyScrollTop:evt.clientY-document.body.clientTop;
		if ((CBE)&&(CBE.windowLock)) {
			mouseY < -oy?lockY=-mouseY-oy:lockY=0;
			mouseX < -ox?lockX=-mouseX-ox:lockX=0;
			mouseY > (SHW()[1]-oDv.offsetHeight-oy)?lockY=-mouseY+SHW()[1]-oDv.offsetHeight-oy:lockY=lockY;
			mouseX > (SHW()[0] - dvBdy.offsetWidth - ox) ? lockX = -mouseX - ox + SHW()[0] - dvBdy.offsetWidth : lockX = lockX;
          }

//   var spacing = 10;
////calls the method ,get the name and version of the browser
//   browserinfo();
//   if (navigator.Actual_Name == "Microsoft Internet Explorer" && navigator.Actual_Version == "7.0") {
//       spacing = 110;
//   }
//   else if (navigator.Actual_Name == "Microsoft Internet Explorer(Maxthon)" && navigator.Actual_Version == "8.0") {
//       spacing = 130;
//   }
//   else {
//       spacing = 10;
//   }
//   alert(navigator.Actual_Name + "   " + navigator.Actual_Version);
         oDv.style.left = ((fixposx) || (fixposx == 0)) ? fixposx : bodyScrollLet + mouseX + ox + lockX - 10 + "px";
		oDv.style.top = ((fixposy) || (fixposy == 0)) ? fixposy : bodyScrollTop + mouseY + oy + lockY + "px";
}
}

function doCheck() {	
	if (   (mouseX < boxLeft)    ||     (mouseX >boxRight)     || (mouseY < boxTop) || (mouseY > boxBottom)) {
		if (!CBE.requireclick)
			fadeOut();
		if (CBE.IEbugfix) {showSelects();}
		CBE=null;
	}
}

function pauseBox(e) {
   e?evt=e:evt=event;
	boxMove=false;
	evt.cancelBubble=true;
}

function showHideBox(e) {
	oDv.style.visibility=(oDv.style.visibility!='visible')?'visible':'hidden';
}

function hideBox(e) {
	oDv.style.visibility='hidden';
}

var COL=0;
var stopfade=false;
function fadeIn(fs) {
		ID=null;
		COL=0;
		oDv.style.visibility='visible';
		fadeIn2(fs);
}

function fadeIn2(fs) {
		COL=COL+fs;
		COL=(COL>1)?1:COL;
		oDv.style.filter='alpha(opacity='+parseInt(100*COL)+')';
		oDv.style.opacity=COL;
		if (COL<1)
		 setTimeout("fadeIn2("+fs+")",20);		
}


function fadeOut() {
	oDv.style.visibility='hidden';
	
}

function isChild(s,d) {
	while(s) {
		if (s==d) 
			return true;
		s=s.parentNode;
	}
	return false;
}

var cSrc;
function checkMove(e) {
	e?evt=e:evt=event;
	cSrc=evt.target?evt.target:evt.srcElement;
	if ((!boxMove)&&(!isChild(cSrc,oDv))) {
		fadeOut();
		if (CBE&&CBE.IEbugfix) {showSelects();}
		boxMove=true;
		CBE=null;
	}
}

function showSelects(){
   var elements = document.getElementsByTagName("select");
   for (i=0;i< elements.length;i++){
      elements[i].style.visibility='visible';
   }
}

function hideSelects(){
   var elements = document.getElementsByTagName("select");
   for (i=0;i< elements.length;i++){
   elements[i].style.visibility='hidden';
   }
}

// check the name and version of browser
function browserinfo() {
    var Browser_Name = navigator.appName;
    var Browser_Version = parseFloat(navigator.appVersion);
    var Browser_Agent = navigator.userAgent;

    var Actual_Version, Actual_Name;

    var is_IE = (Browser_Name == "Microsoft Internet Explorer");
    var is_NN = (Browser_Name == "Netscape");

    if (is_NN) {
        //upper 5.0 need to be process,lower 5.0 return directly
        if (Browser_Version >= 5.0) {
            var Split_Sign = Browser_Agent.lastIndexOf("/");
            var Version = Browser_Agent.indexOf(" ", Split_Sign);
            var Bname = Browser_Agent.lastIndexOf(" ", Split_Sign);

            Actual_Version = Browser_Agent.substring(Split_Sign + 1, Version);
            Actual_Name = Browser_Agent.substring(Bname + 1, Split_Sign);
        }
        else {
            Actual_Version = Browser_Version;
            Actual_Name = Browser_Name;
        }
    }
    else if (is_IE) {
        var Version_Start = Browser_Agent.indexOf("MSIE");
        var Version_End = Browser_Agent.indexOf(";", Version_Start);
        Actual_Version = Browser_Agent.substring(Version_Start + 5, Version_End)
        Actual_Name = Browser_Name;

        if (Browser_Agent.indexOf("Maxthon") != -1) {
            Actual_Name += "(Maxthon)";
        }
        else if (Browser_Agent.indexOf("Opera") != -1) {
            Actual_Name = "Opera";
            var tempstart = Browser_Agent.indexOf("Opera");
            var tempend = Browser_Agent.length;
            Actual_Version = Browser_Agent.substring(tempstart + 6, tempend)
        }
    }
    else {
        Actual_Name = "Unknown Navigator"
        Actual_Version = "Unknown Version"
    }
    /*------------------------------------------------------------------------------
    --Your Can Create new properties of navigator(Acutal_Name and Actual_Version) --
    --Userage:                                                                     --
    --1,Call This Function.                                                        --
    --2,use the property Like This:navigator.Actual_Name/navigator.Actual_Version;--
    ------------------------------------------------------------------------------*/
    navigator.Actual_Name = Actual_Name;
    navigator.Actual_Version = Actual_Version;

    /*---------------------------------------------------------------------------
    --Or Made this a Class.                                                     --
    --Userage:                                                                  --
    --1,Create a instance of this object like this:var browser=new browserinfo;--
    --2,user this instance:browser.Version/browser.Name;                        --
    ---------------------------------------------------------------------------*/
    this.Name = Actual_Name;
    this.Version = Actual_Version;
}