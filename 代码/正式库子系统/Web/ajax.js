function $1(id)
{
	return document.getElementById(id);	
}
function echo(obj,html)
{
	$1(obj).innerHTML=html;
}
function createxmlhttp()
{
	var xmlhttp = false;
	try	
	{
  		xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
 	} 
	catch (e) 
	{
  		try 
  		{
   			xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
  		} 
		catch (e) 
		{
   			xmlhttp = false;
 		}
 	}
	if (!xmlhttp && typeof XMLHttpRequest!='undefined') 
	{
  		xmlhttp = new XMLHttpRequest();
		if (xmlhttp.overrideMimeType)
		{
			//设置MiME类别 
			xmlhttp.overrideMimeType('text/xml');
		}
	}
	return xmlhttp;	
}

//向服务器获取数据
function getdata1(url, obj, initJS) {
    var xmlhttp = createxmlhttp();
    xmlhttp.onreadystatechange = requestdata;
    xmlhttp.open("GET", url, false);
    xmlhttp.setRequestHeader("If-Modified-Since", "0");
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xmlhttp.send(null);
    function requestdata() {
        echo(obj, "<IMG SRC='imgx/loading.gif' /><span style=' font-size:12px; color:Black;'>加载中……</span>");
        if (xmlhttp.readyState == 4) {
            //alert(xmlhttp.responseText);
            if (xmlhttp.status == 200) {
                echo(obj, xmlhttp.responseText);

                if (initJS != null) {
                    getInitJS(initJS);
                }
            }
        }
    }
}
//向服务器获取数据
function getdata(url,obj,initJS)
{
		var xmlhttp = createxmlhttp();
		xmlhttp.onreadystatechange=requestdata;
		xmlhttp.open("GET",url,true);
		xmlhttp.setRequestHeader("If-Modified-Since","0");
		xmlhttp.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
		xmlhttp.send(null);
		function requestdata()
		{
			echo(obj,"<IMG SRC='imgx/loading.gif' /><span style=' font-size:12px; color:Black;'>加载中……</span>");
			if(xmlhttp.readyState==4)
			{
			//alert(xmlhttp.responseText);
				if(xmlhttp.status==200)
				{
					echo(obj,xmlhttp.responseText);
					
					if(initJS != null)
					{
					    getInitJS(initJS);
					}
				}
			}
		}
}
//向服务器发送数据
function postdata(url,obj,data)
{
		var xmlhttp= createxmlhttp();
		xmlhttp.onreadystatechange=requestdata;
		xmlhttp.open("POST", url, true);
		xmlhttp.setRequestHeader("If-Modified-Since","0");
		xmlhttp.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
		xmlhttp.send(data);
		function requestdata()
		{
			echo(obj,"<IMG SRC='imgx/loading.gif' /><span style=' font-size:12px; color:Black;'>加载中……</span><br>");
			if(xmlhttp.readyState==4)
			{
			
				if(xmlhttp.status==200)
				{
					if(!postInitJS(xmlhttp.responseText))
					{
					    echo(obj,xmlhttp.responseText);
					}
				}
			}
		}
}


function ShowSub1(id) {
//    if (flag == "1") {
//        eval("sub" + id).style.display = '';
//        document.images["AddImg" + id].src = 'imgx/subImg.gif';
//        return;
//    }
//    if (flag == "0") {
//        eval("sub" + id).style.display = 'none';
//        document.images["AddImg" + id].src = 'imgx/addImg.gif';
//        return;
//    }
    if (eval("sub" + id).style.display == 'none') {
        eval("sub" + id).style.display = '';
        document.images["AddImg" + id].src = 'imgx/subImg.gif';
        return;
    }
    else {
//        eval("sub" + id).style.display = 'none';
//        document.images["AddImg" + id].src = 'imgx/addImg.gif';
        return;
    }
}


function ShowSub(id,flag) {
    if(flag=="1")
    {
        eval("sub" + id).style.display='';
        document.images["AddImg" + id].src='imgx/subImg.gif';
        return;
    }
    if(flag=="0")
    {
        eval("sub" + id).style.display='none';
        document.images["AddImg" + id].src='imgx/addImg.gif';
        return;
    }
    if(eval("sub" + id).style.display=='none')
    {
        eval("sub" + id).style.display='';
        document.images["AddImg" + id].src='imgx/subImg.gif';
        return;
    }
    else
    {
        eval("sub" + id).style.display='none';
        document.images["AddImg" + id].src='imgx/addImg.gif';
        return;
    }    
}

function showMenu(id,fatherID)
{ 
    contextmenu.style.posLeft = document.body.scrollLeft + event.x + 10 ;
    contextmenu.style.posTop = document.body.scrollTop + event.y + 10;
    var menuHtml = "";
    menuHtml = menuHtml + "<span style='cursor:hand;font-size:12px; color:Black;' onmousemove=this.style.backgroundColor='#00cccc'; onmouseout=this.style.backgroundColor=''; onclick=getdata('tree.aspx?mode=getAdd&fatherID=" + fatherID + "','add" + fatherID + "','addFocus" + fatherID + "');closeMenu(); >添加同级</span><br />";
    menuHtml = menuHtml + "<span style='cursor:hand;font-size:12px; color:Black;' onmousemove=this.style.backgroundColor='#00cccc'; onmouseout=this.style.backgroundColor=''; onclick=getdata('tree.aspx?mode=addSub&id=" + id + "','addSub" + id + "','getTree" + id + "');closeMenu(); >添加子级</span><br />";
    menuHtml = menuHtml + "<span style='cursor:hand;font-size:12px; color:Black;' onmousemove=this.style.backgroundColor='#00cccc'; onmouseout=this.style.backgroundColor=''; onclick=getdata('tree.aspx?mode=getEdit&id=" + id + "','edit" + id + "','editFocus" + id + "');closeMenu(); >修　　改</span><br />";
    menuHtml = menuHtml + "<span style='cursor:hand;font-size:12px; color:Black;' onmousemove=this.style.backgroundColor='#00cccc'; onmouseout=this.style.backgroundColor=''; onclick=closeMenu();if(confirm('你确定要删除此菜单吗？')){postdata('tree.aspx?mode=del&id=" + id + "&fatherID=" + fatherID + "','edit" + id + "');} >删　　除</span>";
    document.getElementById('contextmenu').innerHTML = menuHtml ;
    contextmenu.style.display = "" ;
}

function closeMenu()
{
    contextmenu.style.display="none";
}



document.onclick = function () {
    if (document.activeElement != contextmenu) {
        closeMenu();
    }
}
function getInitJS(JS)
{
	if(JS.substr(0,9)=='getnewmsg')
    {
       getMsg();
    }
	
    if(JS.substr(0,6)=='addSub')
    {
        id=JS.substr(6,JS.length);
        getdata('tree.aspx?mode=getAdd&fatherID=' + id,'add' + id,'addFocus' + id);
    }
    if(JS.substr(0,8)=='addFocus')
    {
        id=JS.substr(8,JS.length);
        $1('addName'+id).select();
    }
    if(JS.substr(0,9)=='editFocus')
    {
        id=JS.substr(9,JS.length);
        $1('editName'+id).select();
    }
    if(JS.substr(0,7)=='getTree')
    {
        id=JS.substr(7,JS.length);
        getdata('tree.aspx?mode=getTree&fatherID=' + id,'sub' + id,'addSub' + id);
        ShowSub(id,'1');
    }
    if(JS.substr(0,10)=='getSubTree')
    {
        id=JS.substr(10,JS.length);
        getdata('tree.aspx?mode=getTree&fatherID=' + id,'sub' + id);
        ShowSub(id,'1');
    }
}
function postInitJS(JS)
{
    if(JS.substr(0,5)=='added')
    {
        id=JS.substr(5,JS.length);
        if(id=="0")
        {
            getdata("tree.aspx?mode=getTree&fatherID=0","divAjax");
        }
        else
        {
            getdata("tree.aspx?mode=getTree&fatherID=" + id,"sub" + id );
        }
        return true;
    }
    if(JS.substr(0,6)=='edited')
    {
        id=JS.substr(6,JS.length);
        getdata('tree.aspx?mode=returnEdit&id=' + id,'edit' + id );
        return true;
    }
    if(JS.substr(0,10)=='addNameRep')
    {
        id=JS.substr(10,JS.length);
        alert('此菜单名已经存在');
        getdata('tree.aspx?mode=getAdd&fatherID=' + id ,'add' + id ,'addFocus' + id);
        return true;
    }
    if(JS.substr(0,11)=='editNameRep')
    {
        id=JS.substr(11,JS.length);
        alert('此菜单名已经存在');
        getdata('tree.aspx?mode=getEdit&id=' + id ,'edit' + id ,'editFocus' + id);
        return true;
    }
    if(JS.substr(0,10)=='deletError')
    {
        id=JS.substr(10,JS.length);
        alert('请先删除此菜单的子级项');
        if(id=="0")
        {
            getdata("tree.aspx?mode=getTree&fatherID=0","divAjax");
        }
        else
        {
            getdata("tree.aspx?mode=getTree&fatherID=" + id,"sub" + id );
        }
        return true;
    }
    if(JS.substr(0,7)=='deleted')
    {
        id=JS.substr(7,JS.length);
        if(id=="0")
        {
            getdata("tree.aspx?mode=getTree&fatherID=0","divAjax");
        }
        else
        {
            getdata("tree.aspx?mode=getTree&fatherID=" + id,"sub" + id );
        }
        return true;
    }
    if(JS.substr(0,5)=='moved')
    {
        id=JS.substr(5,(JS.indexOf('i')-5));
        idFNO=JS.substr(JS.indexOf('i')+1,(JS.indexOf('f')-JS.indexOf('i')-1));
        subID=JS.substr((JS.indexOf('f')+1),JS.length);
        if(idFNO=="0")
        {
            getdata("tree.aspx?mode=getTree&fatherID=0","divAjax");
        }
        else if(document.getElementById("sub" + idFNO)!=null)
        {
            getdata("tree.aspx?mode=getTree&fatherID=" + idFNO,"sub" + idFNO);
        }
        if(id=="0")
        {
            getdata("tree.aspx?mode=getTree&fatherID=0","divAjax",'getSubTree'+subID);
        }
        else if(document.getElementById("sub" + id)!=null)
        {
            getdata("tree.aspx?mode=getTree&fatherID=" + id,"sub" + id,'getSubTree'+subID);
        }
        return true;
    }
    return false;
}

function trim(str) 
{
    return str.replace(/\s+$|^\s+/g,"");
}


function postAdd(id)
{
    if(trim($1('addName'+id).value)=='')
    {
        alert('菜单名字不能为空！');
        $1('addName'+id).focus();
        return false;
    }
    data="Name="+($1('addName'+id).value);
    postdata('tree.aspx?mode=saveAdd&fatherID=' + id ,'add' + id,data);
}

function postEdit(id)
{
    if(trim($1('editName'+id).value)=='')
    {
        alert('菜单名字不能为空！');
        $1('editName'+id).focus();
        return false;
    }
    data="Name="+($1('editName'+id).value);
    postdata('tree.aspx?mode=saveEdit&id=' + id ,'edit' + id,data);
}

function postMove(id,fatherID)
{
    if(id==''||id==null)
    {
        alert('移动失败！');
        return false;
    }
    if(fatherID==''||fatherID==null)
    {
        alert('移动失败！');
        return false;
    }
    data="id="+id+"&fatherID="+fatherID;
	//open this can be moved
    postdata('tree.aspx?mode=move','sub' + fatherID,data);
}
var g_CatchDiv = false;
var g_objNO;
var g_objName;
function catchBDiv(obj,name)
{
    window.setTimeout('g_CatchDiv = true',200)
    g_objNO = obj.substr(4,obj.length);
    g_objName = name;
    var BMDiv = document.getElementById("bDiv");
    var MoveObj = document.getElementById(obj);
    BMDiv.innerHTML=MoveObj.innerHTML;
    BMDiv.style.cursor='';
    BMDiv.style.filter='alpha(opacity=40)';
    BMDiv.style.left = event.x-25;
    BMDiv.style.top = event.y+10;
    BMDiv.setCapture();
    document.onmouseup = function(){releaseDiv();};
    document.onmousemove = function(){moveBDiv();};
}

function releaseDiv()
{
    window.setTimeout('g_CatchDiv = false',200)
    var BMDiv = document.getElementById("bDiv");
    BMDiv.style.display='none';
    BMDiv.releaseCapture();
    document.onmousemove = null;
}

function moveBDiv()
{
    if(g_CatchDiv)
    {
        if(document.images["AddImg" + g_objNO] !=null)
        {
            ShowSub(g_objNO,"0");
        }
        var BMDiv = document.getElementById("BDiv");
        BMDiv.style.left = event.x-25;
        BMDiv.style.top = event.y;
        BMDiv.style.display='';
    }
}

function menuClick(value)
{
    //alert(value);
	window.parent.yhb_mainFrame.location.href="fmtest.aspx?ft_str="+escape(value);
}
function menuClick_yhb(value, urlstr) {

    if (value == "fmemail_FYJ" || value == "fmemail_SJX" || value == "fmemail_CGX" || value == "fmemail_FJX" || value == "fmemail_LJX" || value == "fmemail_CZYJ" || value == "fmemail_DZB" || value == "fmemail_SZ") {
        
        window.parent.goUrl();
        window.parent.f3.location.href = urlstr + "?module=" + value;
    }
    else {
        window.parent.goUrl();
        window.parent.rightFrame.location.href = urlstr + "?module=" + value;
    }
    
}

function open_mail() {
    getdata('tree.aspx?mode=getTree&fatherID=99', 'sub99');
    ShowSub(99);
    window.parent.goUrl();
    window.parent.f3.location.href = "/web/fmemail/fmemail_FYJ.aspx?module=fmemail_FYJ";
    window.scrollTo(0, 1000);
}
function open_onesub(i) {

        if (typeof document.all["sub" + i] != "undefined") {
            getdata1('tree.aspx?mode=getTree&fatherID=' + i, 'sub' + i);
            ShowSub1(i);
    }
    }

    /*
    标记菜单
    */
    function SignSub(name) {
    for (var i = 0; i < document.getElementsByTagName('td').length; i++) {
        if (document.getElementsByTagName('td')[i].getAttribute('name') == 'dddddddd') {
            if (document.getElementsByTagName('td')[i].childNodes[0].id == "addSub" + name) {
                document.getElementsByTagName('td')[i].style.border = '1px solid red';
                var absoluteTop = getAbsoluteTop(document.getElementsByTagName('td')[i])-10;
                //                alert("页面滚动条X：" + document.body.scrollTop + " 页面滚动条Y：" + document.body.scrollLeft + " 本身的绝对高度:" + document.getElementsByTagName('td')[i].offsetHeight + "  上绝对位置:" + absoluteTop);
                window.scrollTo(0, absoluteTop); 

            }
            else {
                document.getElementsByTagName('td')[i].style.border = '0px solid #B0FFB0';
            }
        }
    }
}
/*
获取元素的上绝对位置
*/
function getAbsoluteTop(object) {
    o = object;
    oTop = o.offsetTop;
    while (o.offsetParent != null) {
        oParent = o.offsetParent
        oTop += oParent.offsetTop  // Add parent top position
        o = oParent
    }
    return oTop
}
/*
获取元素的左绝对位置
*/
function getAbsoluteLeft(object) {
    o = object;
    oLeft = o.offsetLeft;
    while (o.offsetParent != null) {
        oParent = o.offsetParent
        oLeft += oParent.offsetLeft
        o = oParent
    }
    return oLeft
}



function open_all() {

for(i = 0; i < 100; i++)
{
if (typeof document.all["sub"+i] != "undefined") {
        //alert("存在");
        getdata('tree.aspx?mode=getTree&fatherID='+i, 'sub'+i);
    ShowSub(i);
        
    }
    else {
        //alert("不存在");
   
    }
}
    
}