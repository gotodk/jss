﻿
var handle;
function start()
{
var obj = document.getElementById("tip");
if (parseInt(obj.style.height)==0)
{ obj.style.display="block";
handle = setInterval("changeH('up')",2);
}else
{
     handle = setInterval("changeH('down')",2) 
} 
}
function changeH(str)
{
var obj = document.getElementById("tip");
if(str=="up")
{
if (parseInt(obj.style.height)>100)
   clearInterval(handle);
else
   obj.style.height=(parseInt(obj.style.height)+8).toString()+"px";
}
if(str=="down")
{
if (parseInt(obj.style.height)<8)
{ clearInterval(handle);
   obj.style.display="none";
}
else 
   obj.style.height=(parseInt(obj.style.height)-8).toString()+"px"; 
}
}

function recover()
{
document.getElementsByTagName("html")[0].style.overflow = "auto";
document.getElementById("shadow").style.display="none";
document.getElementById("detail").style.display="none"; 
}
