<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Web_Default" %>

<%@ Register Assembly="RadWindow.Net2" Namespace="Telerik.WebControls" TagPrefix="radW" %>

<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>

<%@ Register Assembly="RadSplitter.Net2" Namespace="Telerik.WebControls" TagPrefix="radspl" %>


<script runat="server" language="C#">

    protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        Session.RemoveAll();
    }
</script>

<html >
<head runat="server">
    <title>济商所管理系统</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta http-equiv="Page-Enter" content="blendTrans(Duration=0.5)">
    <link href="v3css.css" rel="stylesheet" type="text/css" />
<style type="text/css"> 
<!-- 
a { color: #E15A00; text-decoration: none; } 
a:hover { color: #F60; } 
#scrollWrap { 
 width:180px;
 height: 18px; 
 overflow: hidden; 
} 
#scrollMsg { 
 float: left; 
 text-align: left;
  padding:0 10px;
}
#scrollMsg ul {
  margin:0;
  padding:0;
}
#scrollMsg li { 
 line-height: 18px; 
  list-style:none;
} 
--> 
</style>
    
    <script type="text/javascript" src="Dialog.js"></script>
<script type="text/javascript">
//    function zOpenD() {
//        var diag = new Dialog("Diag1");
//        diag.Width = 900;
//        diag.Height = 400;
//        diag.Title = "弹出窗口示例";
//        diag.URL = "LeftContent.aspx";
//        diag.ShowMessageRow = true;
//        diag.MessageTitle = "弹出窗口示例";
//        diag.Message = "在这儿你可以对这个窗口的内容或功能作一些说明";
//        diag.OKEvent = function () { alert("123"); };   //点击确定后调用的方法
//        diag.show();
//    }
//    function zOpen() {
//        var diag = new Dialog("Diag2");
//        diag.Width = 900;
//        diag.Height = 400;
//        diag.Title = "弹出窗口示例";
//        diag.URL = "http://demo.zving.com/";
//        diag.OKEvent = zAlert; //点击确定后调用的方法
//        diag.show();
//    }
//    function zOpenInner() {
//        var diag = new Dialog("Diag3");
//        diag.Width = 300;
//        diag.Height = 100;
//        diag.Title = "弹出窗口示例";
//        diag.innerHTML = '<div style="text-align:center">直接输出html，使用dialog.<b>innerHTML</b>。</div>'
//        diag.OKEvent = function() { diag.close(); }; //点击确定后调用的方法
//        diag.show();
//    }
//    function zOpenEle() {
//        var diag = new Dialog("Diag4");
//        diag.Width = 300;
//        diag.Height = 100;
//        diag.Title = "弹出窗口示例";
//        diag.innerElementId = "forlogin"
//        diag.OKEvent = function() { $E.getTopLevelWindow().$("username").value || alert("用户名不能为空"); $E.getTopLevelWindow().$("userpwd").value || alert("密码不能为空") }; //点击确定后调用的方法
//        diag.show();
//    }
//    function zAlert() {
//        Dialog.alert("你点击了一个按钮");
//    }
//    function zConfirm() {
//        Dialog.confirm('警告：您确认要XXOO吗？', function() { Dialog.alert("yeah，周末到了，正是好时候"); });
//        
//    }
//    function sometext(ele, n) {
//        var strArr = ["可", "以", "清", "心", "也"];
//        var writeStr = ""
//        for (i = 0; i < n; i++) {
//            index = parseInt(Math.random() * 5);
//            for (j = 0; j < 5; j++) {
//                str = index + j > 4 ? index + j - 5 : index + j;
//                writeStr += strArr[str];
//            }
//        }
//        $(ele).innerHTML = writeStr;
//    }




//    function zConfirm_relogin() {

//        Dialog.confirm('警告！重新登录会导致腾讯通RTX通讯平台重新启动！请谨慎操作！<br><br>确定要重新登录吗？', function() { __doPostBack('LoginStatus1$ctl00', ''); });
//        initgogo();
//        
//        ///document.getElementById(Dialog._Array[0]).style.display = "none";

    //    }
    var diagOpenWelcome;
    //欢迎页面里面用到的弹窗信息
    function OpenWelcome(FunIdent) {
        diagOpenWelcome = new Dialog("Diag1");
        diagOpenWelcome.Width = 463;
        diagOpenWelcome.Height = 400;
        diagOpenWelcome.Title = "增加模块";
        diagOpenWelcome.URL = "LeftContent.aspx?FunIdent=" + FunIdent;
        diagOpenWelcome.ShowMessageRow = false;
        diagOpenWelcome.ShowButtonRow = false;
        diagOpenWelcome.show();
       
    }
    //欢迎页面执行后续操作
    function UpdateWelcome(FunIdentInfo) {
        window.frames['rightFrame'].UpdateOpenVelcome(FunIdentInfo);
        diagOpenWelcome.close();
    }
    //删除警告
    function DeleteWarning() {

        Dialog.confirm("你确定要删除此常用业务吗？<br>&nbsp删除后需重新添加才能使用。", function () {
            window.frames['rightFrame'].controllerInfo.DeleteFunctions(window.frames['rightFrame'].operateInfoText, window.frames['rightFrame'].functionInfoText);
        });
    }


    function zConfirm_relogin() {

        Dialog.confirm('警告！系统将关闭！\n请谨慎操作！<br><br>确定要退出系统吗？', function () { __doPostBack('LoginStatus1$ctl00', ''); });
        initgogo();

        ///document.getElementById(Dialog._Array[0]).style.display = "none";

    }
    function zConfirm_outlogin() {
      
        Dialog.confirm('警告！系统将关闭！\n请谨慎操作！<br><br>确定要退出系统吗？', function() { __doPostBack('LoginStatus2$ctl00', ''); });
        initgogo();

    }

    function close_mywin() {
        Dialog[0].close();
    }
       function close_yj() {
       rightFrame.history.back(-1);
       rightFrame.history.back(-1); 
    }
     function close_yjqx() {
       rightFrame.history.back(-1);
    }
</script>
    <script language="javascript" type="text/javascript">
 
      function StopAjax() {

          parent.document.getElementById("rightFrame_hide").style.display = "none";
          parent.document.getElementById("rightFrame").style.display = "";
          
          window.top.frames.f1.location.href = "http://192.168.0.4:8080/left_fm.asp";
          if(window.top.frames.rightFrame.location.href !=null && window.top.frames.rightFrame.location.href !='undefined' )
          {
             var url = window.top.frames.rightFrame.location.href ;
           
           
                 if(url.indexOf("CreateDocument.aspx")>0)
                 {
            
                          window.clearTimeout(window["RadAjaxTimer1"].TimerTimeouts["RadAjaxTimer1"]);
                          window.clearInterval(window["RadAjaxTimer1"].TimerTimeouts["RadAjaxTimer1"]);
               
                  }
           }
     }

    </script>
    <script language="javascript" type="text/javascript">


        function ShowRemindForm() {

            var oManager = GetRadWindowManager();
            var oWnd = oManager.GetWindowByName("WarningWindow");
            oWnd.SetUrl('select_warnings.aspx?type=1');
            oWnd.Show();
            oWnd.SetActive();
            oWnd.MoveTo("10", document.body.clientHeight - 320);
            return true;
            
        }
        function ShowWarnForm() {

            var oManager = GetRadWindowManager();
            var oWnd = oManager.GetWindowByName("WarningWindow");
            oWnd.SetUrl('select_warnings.aspx?type=2');
            oWnd.Show();
            oWnd.SetActive();
            oWnd.MoveTo("10", document.body.clientHeight - 320);
            return false;
        }
        function ShowForm_Minimize() {

            var oManager = GetRadWindowManager();
            var oWnd = oManager.GetWindowByName("WarningWindow");
            oWnd.Show();
            oWnd.SetActive();
            oWnd.MoveTo("490", "35");
            oWnd.Minimize();
            oWnd.SetActive();
            oWnd.AutoResize();
            return false;
        }
        function window.onbeforeunload() {
            //改变clientX，clientY的判断，也可以检测到用户通过任务栏右键关闭，自己摸索一下具体参数 
            if (event.clientX > document.body.clientWidth && event.clientY < 0 || event.altKey)
            //点击了取消，会留在页面，这个时候让用户自己保存数据
                window.event.returnValue = "警告！确定要退出系统吗?\n请谨慎操作！";
            //或者这里用AJAX将该保存的数据直接送到后台保存。只不过那句提示是摆设了 
        } 

         
    </script>
    <script Language="JavaScript">
        //***********默认设置定义.*********************
        tPopWait = 50; //停留tWait豪秒后显示提示。
        tPopShow = 5000; //显示tShow豪秒后关闭提示
        showPopStep = 20;
        popOpacity = 99;

        //***************内部变量定义*****************
        sPop = null;
        curShow = null;
        tFadeOut = null;
        tFadeIn = null;
        tFadeWaiting = null;
        document.write("<style type='text/css'id='defaultPopStyle'>");
        document.write(".cPopText { background-color: #FFFFFF;color:#000000; border: 1px #000000 solid;font-color: font-size: 10px; padding-right: 4px; padding-left: 4px; height: 20px; padding-top: 2px; padding-bottom: 2px; filter: Alpha(Opacity=0)}");
        document.write("</style>");
        document.write("<div id='dypopLayer' style='position:absolute;z-index:1000;' class='cPopText'></div>");
        function showPopupText() {
            var o = event.srcElement;
            MouseX = event.x;
            MouseY = event.y;
            if (o.alt != null && o.alt != "") { o.dypop = o.alt; o.alt = "" };
            if (o.title != null && o.title != "") { o.dypop = o.title; o.title = "" };
            if (o.dypop != sPop) {
                sPop = o.dypop;
                clearTimeout(curShow);
                clearTimeout(tFadeOut);
                clearTimeout(tFadeIn);
                clearTimeout(tFadeWaiting);
                if (sPop == null || sPop == "") {
                    dypopLayer.innerHTML = "";
                    dypopLayer.style.filter = "Alpha()";
                    dypopLayer.filters.Alpha.opacity = 0;
                }
                else {
                    if (o.dyclass != null) popStyle = o.dyclass
                    else popStyle = "cPopText";
                    curShow = setTimeout("showIt()", tPopWait);
                }

            }
        }
        function showIt() {
            dypopLayer.className = popStyle;
            dypopLayer.innerHTML = sPop;
            popWidth = dypopLayer.clientWidth;
            popHeight = dypopLayer.clientHeight;
            if (MouseX + 12 + popWidth > document.body.clientWidth) popLeftAdjust = -popWidth - 24
            else popLeftAdjust = 0;
            if (MouseY + 12 + popHeight > document.body.clientHeight) popTopAdjust = -popHeight - 24
            else popTopAdjust = 0;
            dypopLayer.style.left = MouseX + 12 + document.body.scrollLeft + popLeftAdjust;
            dypopLayer.style.top = MouseY + 12 + document.body.scrollTop + popTopAdjust;
            dypopLayer.style.filter = "Alpha(Opacity=0)";
            fadeOut();
        }
        function fadeOut() {
            if (dypopLayer.filters.Alpha.opacity < popOpacity) {
                dypopLayer.filters.Alpha.opacity += showPopStep;
                tFadeOut = setTimeout("fadeOut()", 1);
            }
            else {
                dypopLayer.filters.Alpha.opacity = popOpacity;
                tFadeWaiting = setTimeout("fadeIn()", tPopShow);
            }
        }
        function fadeIn() {
            if (dypopLayer.filters.Alpha.opacity > 0) {
                dypopLayer.filters.Alpha.opacity -= 1;
                tFadeIn = setTimeout("fadeIn()", 1);
            }
        }
        document.onmouseover = showPopupText;
</script>
    
<%--<script language="javascript" type="text/javascript">
     var colorkey = 1;
     var c = new Array();
     c[1] = '#FF0000';
     c[2] = '#333366';
     function chg_Font_Cont(){ 
      if ( colorkey<c.length-1 ) {
       colorkey += 1;
      } else {
       colorkey = 1;
      }
      colorcode = c[colorkey];
      eval("document.getElementById('warning').innerHTML = '<a href=down/per_smk.zip class=a06 target=_blank><font color="+colorcode+">"+"teste"+"</font></a><br>'");
      setTimeout("chg_Font_Cont()",600); 
     }
     chg_Font_Cont()
</script>--%>
<%--<style type="text/css"> 
#winpop { width:200px; height:0px; position:absolute; right:0; bottom:0; border:1px solid #999999; margin:0; padding:1px; overflow:hidden;display:none; background:#FFFFFF} 
#winpop .title { width:100%; height:20px; line-height:20px; background:#FFCC00; font-weight:bold; text-align:center; font-size:10px;} 
#winpop .con { width:100%; height:80px; line-height:80px; font-weight:bold; font-size:12px; color:#FF0000; text-decoration:blink; text-align:center;} 
#silu { font-size:10px; color:#999999; position:absolute; right:0;bottom:0px; text-align:right; text-decoration:blink; line-height:20px;} 
.close { position:absolute; right:4px; top:-1px; color:#FFFFFF; cursor:pointer} 
</style> 
<script type="text/javascript">
    function show_pop() {//显示窗口 
        document.getElementById("yhb_neirong").innerHTML = document.getElementById("yhb_neirong_new").innerHTML;
        if (document.getElementById("yhb_neirong_new").innerHTML != "") {
             
            document.getElementById("winpop").style.display = "block";
            timer = setInterval("changeH(4)", 2); //调用changeH(4),每0.002秒向上移动一次 
            
        }
        
    }
    function hid_pop() {//隐藏窗口 
        timer = setInterval("changeH(-4)", 2); //调用changeH(-4),每0.002秒向下移动一次 
    }
    function changeH(addH) {
        document.getElementById("yhb_neirong_new").innerHTML = "";
        var MsgPop = document.getElementById("winpop");
        var popH = parseInt(MsgPop.style.height || MsgPop.currentStyle.height); //用parseInt将对象的高度转化为数字,以方便下面比较（JS读<style>中的height要用"currentStyle.height"） 
        if (popH <= 100 && addH > 0 || popH >= 4 && addH < 0) {//如果高度小于等于100(str>0)或高度大于等于4(str<0) 
            MsgPop.style.height = (popH + addH).toString() + "px"; //高度增加或减少4个象素 
        }
        else {//否则 
            clearInterval(timer); //取消调用,意思就是如果高度超过100象素了,就不再增长了，或高度等于0象素了，就不再减少了 
            MsgPop.style.display = addH > 0 ? "block" : "none"//向上移动时窗口显示,向下移动时窗口隐藏（因为窗口有边框,所以还是可以看见1~2象素没缩进去,这时候就把DIV隐藏掉） 
        }
    }
    var timer_showshow = setInterval('show_pop()', 10000);
</script> --%>
<script type="text/javascript">

    var txt_tixing = "";
    var txt_baojing = "";
    txt_tixing = "0";
    txt_baojing = "0";
    var timer_showshow;
    function showtimer() {
        timer_showshow = setInterval('getandshowkk()', 1000);
    }
    var bjss;
    function colorfulLink(obj) {
        bjss = setInterval(function () {
            switch (obj.style.color) {
                case 'white': obj.style.color = 'Black'; obj.style.filter = 'glow(color:#FFFFFF,strength=1)'; break;
                case 'Black': obj.style.color = 'white'; obj.style.filter = 'glow(color:#FF9999,strength=3)'; break;
                default: obj.style.color = 'white'; obj.style.filter = 'glow(color:#FF9999,strength=3)'; break;
            }
        }, 300);
    };
    function getandshowkk() {
        if (parseInt(txt_tixing) < parseInt(document.getElementById("tixing").innerHTML)) {
            ShowRemindForm();
            Select_m(document.getElementById("bgshengyin").innerHTML);
        }
//        if (parseInt(txt_baojing) < parseInt(document.getElementById("baojing").innerHTML) ) {
//            ShowWarnForm();
//            Select_m(document.getElementById("bgshengyin").innerHTML);
//        }
        clearInterval(bjss);
        if (document.getElementById("tixing").innerHTML != "0") {
            colorfulLink(document.getElementById("tixing").parentNode);
        }
//        if (document.getElementById("baojing").innerHTML != "0") {
//            colorfulLink(document.getElementById("baojing").parentNode);
//        }
        txt_tixing = document.getElementById("tixing").innerHTML;
        //txt_baojing = document.getElementById("baojing").innerHTML;

    }

    var tag = "default";
    function ShowAssignOrDefault() {
        if (tag == "default") {
            tag = "assign";
            document.getElementById("lingOnload").innerHTML = "显示有效栏目";
            window.frames['leftFrame'].onload_tree_1("显示所有栏目");
        }
        else if (tag == "assign") {
            tag = "default";
            document.getElementById("lingOnload").innerHTML = "显示所有栏目";
            window.frames['leftFrame'].onload_tree_1("显示有效栏目");
        }
    }
</script>
</head>
<body  onResize="initgogo()">
    <form id="Form1" runat="server" style="height:100%;margin:0px" >
<%--    <div id="winpop"> 
    <div class="title">您有新的消息<span class="close" onclick="hid_pop()">X</span></div> 
<div class="con" id="yhb_neirong">替补</div> 
</div>--%>
<span id="bgshengyin" runat="server" style=" display:none"></span>
<bgsound loop=false autostart=false id="bgss"> 
<script language="javascript">
    function Select_m(url) {
        try {
            document.all.bgss.src = url;
            bgss.play();
        }
        catch (e) {

        }
    } 

</script>

<%--<table width="100%" id="top_top" border="0" align="center" cellpadding="0" cellspacing="0" style="background-image: url(sytp/logo1.jpg);background-repeat:no-repeat;background-position: left top; height:71px;">--%>
<table width="100%" height="75px" id="top_top" border="0" align="center" cellpadding="0" cellspacing="0" >
  <tr>
    <td style=" height:75px;">
   <%-- <div style="background-image: url(sytp/new/bg_mainNew_left.png);background-repeat: repeat-x;background-position: right top; z-index:888; width:587px; height:75px; float:left; position:absolute; top:0px; left:0px; ">--%>
        <div runat="server"  id="divtitle" style="   background-repeat: repeat-x;background-position: right top; z-index:888; width:587px; height:75px; float:right ; position:absolute; top:20px; left:30px; ">
  
    </div>
    
    </td>
    <td width="10%"  style="background-image: url(sytp/new/bg_main_center.jpg);background-repeat: repeat-x;background-position: right top; z-index:1;  height:75px;"></td>
    <td   class="top_bg_youbian">
      <table width="680px" border="0" align="right" cellpadding="0" cellspacing="0" height="57" style="color:#FFF; position:relative; float:right;top:0px;left:0px;z-index:999; ">
      <tr>
        <td height="30" align="right" valign="middle" align="right"><table border="0" cellpadding="0" cellspacing="0"  style="color:#fff; font-size:9pt;">
        <tr>
            <%--<td colspan="12">--%><%--<span>2010-09-03&nbsp;13:59&nbsp;&nbsp;星期三</span><span style=" color:White; font-weight:bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span style=" padding:2px;"><img src="sytp/new/weather.png" width="14" height="14" border="0"  /></img></span><span style=" cursor:hand;" onclick="goUrl();window.top.frames.rightFrame.location.href='tq.aspx'">&nbsp;&nbsp;&nbsp;天气预报&nbsp;&nbsp;&nbsp;</span><span style=" color:White; font-weight:bold;">&nbsp;&nbsp;|&nbsp;&nbsp;</span><span style=" cursor:hand;" onClick="window.open('http://192.168.0.64','_blank');">公司内网</span><span style=" color:White; font-weight:bold;">&nbsp;&nbsp;|&nbsp;&nbsp;</span><span style=" cursor:hand;" onClick="window.open('http://192.168.0.7','_blank');">官方网站</span><span style=" color:White; font-weight:bold;">&nbsp;&nbsp;|&nbsp;&nbsp;</span><span style=" cursor:hand;" onclick="window.top.frames.rtx_temp.location.href='RTXintegration:'">登录RTX</span></td>--%>
              <td align="left" valign="middle" width="140px"><asp:Label ID="lblServerTime" runat="server"></asp:Label></td> 
            <td align="left" valign="middle" style=" padding-top:4px;"><img src="sytp/new/weather.png" width="14" height="14" border="0"  />&nbsp;</td>
            <td align="left" valign="middle"   >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
      <%-- <td align="left" valign="middle"><span style=" color:White; font-weight:bold;">&nbsp;&nbsp;|&nbsp;&nbsp;</span></td>
            <td align="left" valign="middle"  style=" cursor:hand;" onClick="window.open('http://192.168.0.64','_blank');">公司内网&nbsp;&nbsp;&nbsp;</td>
            <td align="left" valign="middle"><span style=" color:White; font-weight:bold;">&nbsp;&nbsp;|&nbsp;&nbsp;</span></td>
            <td align="left" valign="middle"  style=" cursor:hand;" onClick="window.open('http://192.168.0.7','_blank');">官方网站&nbsp;&nbsp;&nbsp;</td>
           <td align="left" valign="middle"><span style=" color:White; font-weight:bold;">&nbsp;&nbsp;|&nbsp;&nbsp;</span></td>
            <td align="left" valign="middle" style=" cursor:hand;" onclick="window.top.frames.rtx_temp.location.href='RTXintegration:'">登录RTX&nbsp;&nbsp;&nbsp;</td>
             <td align="left" valign="middle">&nbsp;&nbsp;</td>--%>
          </tr>
       <%--   <tr>
           <td align="left" valign="middle" width="140px">&nbsp;</td>
            <td align="left" valign="middle"><img src="sytp/5.gif" width="14" height="12" border="0"  />&nbsp;</td>
            <td align="left" valign="middle"  style=" cursor:hand;" onclick ="rightFrame.history.back();">后退&nbsp;&nbsp;&nbsp;</td>
            <td align="left" valign="middle"><img src="sytp/4.gif" alt="" width="15" height="13" border="0"  />&nbsp;</td>
            <td align="left" valign="middle"  style=" cursor:hand;" onclick ="rightFrame.history.go(1);">前进&nbsp;&nbsp;&nbsp;</td>
            <td align="left" valign="middle"><img src="sytp/1.gif" alt="" width="15" height="14" border="0"  />&nbsp;</td>
            <td align="left" valign="middle"  style=" cursor:hand;" onclick="goUrl();window.top.frames.rightFrame.location.href='ChangPass.aspx'">修改密码&nbsp;&nbsp;&nbsp;</td>
            <td align="left" valign="middle"><img src="sytp/2.gif" alt="" width="15" height="15" border="0"  />&nbsp;</td>
            <td align="left" valign="middle" style=" cursor:hand;" onclick="zConfirm_relogin()">重新登录&nbsp;&nbsp;&nbsp;</td>
            <td align="left" valign="middle"><img src="sytp/3.gif" alt="" width="12" height="11" border="0"  />&nbsp;</td>
            
            <td align="left" valign="middle" style=" cursor:hand;"  onclick="zConfirm_outlogin()">退出&nbsp;&nbsp;&nbsp;</td>
            <td align="left" valign="middle">&nbsp;&nbsp;</td>
          </tr>--%>
          </table></td>
        </tr>
      <tr>
        <td height="27" align="right" valign="bottom" style="color:#255583; font-size:9pt;"><%--<span style=" cursor:hand;" onClick="window.open('http://192.168.0.64','_blank');">公司内网</span>&nbsp;&nbsp;|&nbsp;&nbsp;<span style=" cursor:hand;" onClick="window.open('http://192.168.0.64:7777','_blank');">富美家园</span>&nbsp;&nbsp;|&nbsp;&nbsp;<span style=" cursor:hand;" onClick="window.open('http://192.168.0.7','_blank');">官方网站</span>&nbsp;&nbsp;|&nbsp;&nbsp;<span style=" cursor:hand;" onClick="window.open('http://192.168.100.10:8080/forever/','_blank');">呼叫中心</span>&nbsp;&nbsp;|&nbsp;&nbsp;<span style=" cursor:hand;" onClick="window.open('http://192.168.0.64:9999/','_blank');">在线考试</span>&nbsp;&nbsp;|&nbsp;&nbsp;<span style=" cursor:hand;" onclick="goUrl();window.top.frames.rightFrame.location.href='tq.aspx'"><font id="blink" color="blue">天气预报</font></span>--%>
        <table border="0" cellpadding="0" cellspacing="0"  style="color:#fff; font-size:9pt;">
          <tr>
            <td align="left" valign="middle" style=" padding-top:5px;"><img src="sytp/d3.gif" alt="" width="15" height="15" border="0"  />&nbsp;</td>
             <td align="left" valign="middle" style="cursor:hand;"  onclick="return ShowRemindForm();">
              <radA:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="0px" EnableOutsideScripts="True"    HorizontalAlign="NotSet" LoadingPanelID="" ScrollBars="None">
             &nbsp;提醒（<span  id="tixing"><%=ViewState["remindcount"].ToString()%></span>）
             
                 <radA:RadAjaxTimer ID="RadAjaxTimer1" runat="server" Interval="60000"  OnTick="RadAjaxTimer1_Tick" />
        </radA:RadAjaxPanel>
             </td>
            <td align="left" valign="middle" >&nbsp;&nbsp;&nbsp;</td>
             <td align="left" valign="middle" style=" color:Black; padding-top:1px; width:1px;" ></td>
            <td align="left" valign="middle"  ><asp:Label ID="userRole" runat="server" ForeColor="#ffffff" ></asp:Label>&nbsp;&nbsp;&nbsp;</td>
          
            <td align="left" valign="middle" style=" color:Black;width:1px;"></td>
             <td align="left" valign="middle" style=" padding-top:3px;"><asp:Label ID="userName" runat="server" ForeColor="#ffffff"></asp:Label></td>
              <td align="left" valign="middle" >&nbsp;&nbsp;&nbsp;</td>
               <td align="left" valign="middle" ></td>
              <td align="left" valign="middle" style=" padding-top:5px;"><img src="sytp/1.gif" alt="" width="15" height="14" border="0"  />&nbsp;</td>
            <td align="left" valign="middle"  style=" cursor:hand;" onclick="goUrl();window.top.frames.rightFrame.location.href='ChangePassNew.aspx'">修改密码&nbsp;&nbsp;&nbsp;</td>
         <%--   <td align="left" valign="middle"><img src="sytp/2.gif" alt="" width="15" height="15" border="0"  />&nbsp;</td>
            <td align="left" valign="middle" style=" cursor:hand;" onclick="zConfirm_relogin()">重新登录&nbsp;&nbsp;&nbsp;</td>--%>
            <td align="left" valign="middle" style=" padding-top:2px;"><img src="sytp/3.gif" alt="" width="12" height="11" border="0"  />&nbsp;</td>
            <td align="left" valign="middle" style=" cursor:hand;"  onclick="zConfirm_relogin()">退出&nbsp;&nbsp;&nbsp;</td>
            <td align="left" valign="middle">&nbsp;&nbsp;</td>
          </tr>
          </table>
        
        </td>
        </tr>
    </table></td>
  </tr>
</table>
<table width="100%" id="lianjie_top" border="0" align="center" cellpadding="0" cellspacing="0" class="top_lianjie_bg" style=" height:7px; margin:0px; padding:0px;">
  <tr height="7px">
    <td align="left" valign="middle" style=" height:7px;"><table border="0" cellpadding="0" cellspacing="0" style="font-size:9pt; height:7px;">
      <tr>
     <%-- <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
    <%--  <td align="left" valign="middle"><img src="sytp/d1.gif" alt="" width="16" height="21" border="0"  />&nbsp;&nbsp;</td>--%>
      <%--  <td align="left" valign="middle" style="color:#FFF">当前用户：</td>--%>
      <%--  <td align="left" valign="middle" style="color:#46f808"><asp:Label ID="userName" runat="server" ForeColor="#46f808"></asp:Label></td>--%>
  <%--<td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
    <%--    <td align="left" valign="middle" style="color:#FFF">岗位：</td>--%>
     <%--   <td align="left" valign="middle" style="color:#46f808"><asp:Label ID="userRole" runat="server" ForeColor="#46f808" ></asp:Label></td>--%>
   <%-- <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
   <td align="left" valign="middle"   runat="server" id="TDshouliren0" visible="false" style="color:#FFF">身份：</td>
    <td align="left" valign="middle"  runat="server" id="TDshouliren" visible="false" style="color:#46f808"><asp:Label runat="server" ID="lblshouliren" ForeColor="#46f808"></asp:Label></td>
       <%-- <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
       
    <%--  <td align="left" valign="middle">--%>
       
       
<%-- <radA:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="0px" EnableOutsideScripts="True"    HorizontalAlign="NotSet" LoadingPanelID="" ScrollBars="None">--%>
    <%--    <table border="0" cellpadding="0" cellspacing="0" style=" font-size:9pt;"><tr>--%>
      <%--  <td align="left" valign="middle"><img src="sytp/d2.gif" alt="" width="19" height="18" border="0"  /></td>--%>
      <%--  <td align="left" valign="middle" style="color:#FFF;cursor:hand;"  onclick="return ShowRemindForm();">&nbsp;当前提醒：</td>--%>
     <%--   <td align="left" valign="middle" style="color:#46f808;cursor:hand;"  onclick="return ShowRemindForm();"><span  id="tixing"><%=ViewState["remindcount"].ToString()%></span>条</td>--%>
      <%--  <td align="left" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
     <%--   <td align="left" valign="middle"><img src="sytp/d3.gif" alt="" width="15" height="15" border="0"  /></td>--%>
<%--        <td align="left" valign="middle" style="color:#FFF;cursor:hand;"  onclick="return ShowWarnForm();">&nbsp;报警：</td>--%>
       <%-- <td align="left" valign="middle" style="color:#46f808;cursor:hand;"  onclick="return ShowWarnForm();"><span id="baojing"><%=ViewState["warncount"]%></span>条</td> --%>   
       <%--  </tr>
         </table>--%>
         <%--<div id="Div1" style="display:none"><%=ViewState["yhb_neirong_new"]%></div>--%>
<%--     <radA:RadAjaxTimer ID="RadAjaxTimer1" runat="server" Interval="10000"  OnTick="RadAjaxTimer1_Tick" />
        </radA:RadAjaxPanel>--%>
         
         
  <%--       </td>--%>
          
      </tr>
    </table></td>
    <td align="right" valign="middle" style=" height:7px;"><table border="0" cellpadding="0" cellspacing="0" style="font-size:9pt; color:#FFF; height:7px;">
      <tr>
        <td  id="TDUserchange0" runat="server" visible="false"></td>
        <td  id="TDUserchange" runat="server" visible="false"  style=" cursor:hand;" onclick="goUrl();window.top.frames.rightFrame.location.href='ServiceCenter/Bussnessdaili.aspx?module=Bussesdaili'">&nbsp;切换数据源</td>
      <%--  <td >&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
      <%--  <td align="left" valign="middle"  runat="server" id="tdReturn0"><img src="sytp/d5.gif" alt="" width="19" height="21" border="0" /></td>--%>
       <%-- <td align="left" valign="middle"  runat="server" id="tdReturn"   style=" cursor:hand;" onclick="goUrl();__doPostBack('btnReturn','')">&nbsp;返回代理用户</td>--%>
      <%--  <td> <iframe src="PhoneMess.aspx" name="f25" id = "Iframe26" frameborder="0" height="20" scrolling="no" width="104" marginwidth=0   marginheight=0 ></iframe> </td>--%>
       <%-- <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
      <%--  <td align="left" valign="middle"><img src="sytp/mail.gif" alt="" width="20" height="20" border="0" /></td>--%>
      <%--  <td align="left" valign="middle"   style=" cursor:hand;" onclick="window.top.frames['leftFrame'].open_mail();">&nbsp;进入富美邮箱</td>--%>
      <%--  <td >&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
     <%--   <td align="left" valign="middle"><img src="sytp/d6.gif" alt="" width="20" height="20" border="0" /></td>--%>
      <%--  <td align="left" valign="middle"   style=" cursor:hand;" onclick="window.top.frames.rtx_temp.location.href='RTXintegration:'">&nbsp;重新登录RTX</td>--%>
  <%--      <td >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
        </tr>
    </table></td>
  </tr>
</table>

<table width="100%"  border="0" align="center" cellpadding="0" cellspacing="0" id="zhugaodu">
  <tr>
    <td width="1%" bgcolor="#DEECEF">
    
    
    
    <table border="0" cellpadding="0" cellspacing="0" height="100%" id="theObjTable"  style="table-layout:fixed">
          <tr>
            <td width="5" id="k1"  valign="top" class="caidan_zzz_bg"><img src="sytp/5px.gif" width="5" height="1" /></td>
            <td align="left" valign="top" class="top_aidan_cd3" width="100%" >
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
  <tr>
    <td  align="left" valign="middle"><img src="sytp/cd_2.jpg" width="98" height="40" border="0" /></td>
   
    <td  align="right" valign="bottom" style="padding-top: 0px;padding-right: 0px;padding-bottom: 8px;padding-left: 0px; color:#DD5800; font-size:9pt; padding-right:10px;">
    <div style="  text-align:right; padding-top:15px; white-space:nowrap;  height:30px; text-overflow:clip; overflow:hidden;" ><a href="#" id="lingOnload" onclick="ShowAssignOrDefault()">显示所有栏目</a></div>

  </td>
  </tr>
</table>


</td>
            <td width="11"  id="k2" align="Left" valign="top" class="caidan_yous_bg"><img src="sytp/11px.gif" width="11" height="1" /></td>
          </tr>
          <tr>
            <td width="5" align="left" valign="top" class="caidan_zuo_bg"  id="k11">&nbsp;</td>
            <td align="left" valign="top">
            <iframe src="left.aspx" name="leftFrame" id="leftFrame" frameborder="0" scrolling="auto" width="100%" style="vertical-align: top;" ></iframe>
            </td>
            <td align="right" valign="middle" width="11" class="caidan_you_bg" style="cursor:e-resize"   id="k22" onMouseDown="MouseDownToResize(this);" onMouseMove="MouseMoveToResize(this);" onMouseUp="MouseUpToResize(this);"><img id="qiehuantu" src="sytp/cd_jt2.jpg" width="11" height="31" style="cursor:hand" onClick="hidezuo()" /></td>
          </tr>
        </table>
    
    
    
    </td>
    <td width="99%" bgcolor="#deecef">
              <iframe onload="StopAjax();" src=<%=GetRighUrl()%> name="rightFrame" id = "rightFrame" frameborder="0" height="100%" scrolling="auto" width="100%" style="vertical-align: top" ></iframe>
              
              <iframe src="about:blank" name="rightFrame_hide" id = "rightFrame_hide" frameborder="0" height="100%" scrolling="auto" width="100%" style="vertical-align: top" ></iframe>
              
                <iframe  src="SendmsgonT.aspx" name="Sendtixing" id = "ifTIxing" frameborder="0" height="0" scrolling="no" width="0" style="vertical-align: top" ></iframe>
                        <iframe src="about:blank" name="f2" id = "f2" frameborder="0" height="0" scrolling="no" width="0"></iframe>
                        <iframe src="about:blank" name="f1" id = "f1" frameborder="0" height="0" scrolling="no" width="0"></iframe>
       <iframe src="about:blank" name="f3" id = "f3" frameborder="0" height="0" scrolling="no" width="0"></iframe>
                        <iframe src="about:blank" name="rtx_temp" id = "rtx_temp" frameborder="0" height="0" scrolling="no" width="0"></iframe>
                      <iframe src="about:blank" name="f8" id = "f8" frameborder="0" height="0" scrolling="no" width="0"></iframe>
                     
                       <%-- "commonpage.aspx--%>
    
    </td>
      
  </tr>
</table>


<!-- 隐藏起来的重要东西-->
          <div style=" display:none;">
          
          <asp:Button  runat="server" ID="btnReturn" onclick="btnReturn_Click"/>
          <asp:LoginStatus ID="LoginStatus1" runat="server" ForeColor="#000000" LogoutText="重新登录" LoginText="重新登录" OnLoggingOut="LoginStatus1_LoggingOut" />
      <asp:LoginStatus ID="LoginStatus2" runat="server" ForeColor="#000000" LogoutText="退出系统" LoginText="退出系统" OnLoggedOut="LoginStatus2_LoggedOut" OnLoggingOut="LoginStatus1_LoggingOut" />
      <input runat="server" type="hidden" id="hidUserID"/>
    <div id="AF1Child" class="child" name="child"></div>
    
        <radW:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Web20" 
          Height="360px" Language="zh-CN" VisibleStatusbar="False" Width="500px" 
          Left="" NavigateUrl="" SkinsPath="~/RadControls/Window/Skins" Title="" Top="">
        <Windows>
            <radW:RadWindow ID="WarningWindow" runat="server" Height="300px" 
                NavigateUrl="" ReloadOnShow="True" Skin="Web20" Modal="false" 
                SkinsPath="~/RadControls/Window/Skins" Title="提醒" 
                Width="650px" VisibleStatusbar="False" OnClientClose="ShowForm_Minimize();" MinimizeMode="SameLocation" />

        </Windows>
    </radW:RadWindowManager>
    
    
      </div>


<div id="loaddiv" align="center" style=" z-index:999999">

</div>





    </form>

</body>
</html>

<script language="javascript" type="text/javascript">

function MouseDownToResize(obj){ 
setTableLayoutToFixed(); 
obj.mouseDownX=event.clientX; 
obj.pareneTdW=obj.parentElement.offsetWidth; 
obj.pareneTableW=theObjTable.offsetWidth; 
obj.setCapture(); 
} 
function MouseMoveToResize(obj){ 
if(!obj.mouseDownX) return false; 
var newWidth=obj.pareneTdW*1+event.clientX*1-obj.mouseDownX; 
if(newWidth>0) 
{ 
obj.parentElement.style.width = newWidth; 
theObjTable.style.width=obj.pareneTableW*1+event.clientX*1-obj.mouseDownX; 
} 
} 
function MouseUpToResize(obj){ 
obj.releaseCapture(); 
obj.mouseDownX=0; 
} 
function setTableLayoutToFixed() 
{ 
 if(theObjTable.style.tableLayout=='fixed') return; 
var headerTr=theObjTable.rows[0]; 
for(var i=0;i<headerTr.cells.length;i++) 
{ 
headerTr.cells[i].styleOffsetWidth=headerTr.cells[i].offsetWidth; 
} 
 
for(var i=0;i<headerTr.cells.length;i++) 
{ 
headerTr.cells[i].style.width=headerTr.cells[i].styleOffsetWidth; 
} 
theObjTable.style.tableLayout='fixed'; 
} 


function getHeight(){

    var yScroll;

    if (window.innerHeight && window.scrollMaxY) {
        yScroll = window.innerHeight + window.scrollMaxY;
    } else if (document.body.scrollHeight > document.body.offsetHeight){ // all but Explorer Mac
        yScroll = document.body.scrollHeight;
    } else { // Explorer Mac...would also work in Explorer 6 Strict, Mozilla and Safari
        yScroll = document.body.offsetHeight;
    }
   
    var windowHeight;
    if (self.innerHeight) { // all except Explorer
        windowHeight = self.innerHeight;
    } else if (document.documentElement && document.documentElement.clientHeight) {
    // Explorer 6 Strict Mode
       windowHeight = document.documentElement.clientHeight;
    } else if (document.body) { // other Explorers
       windowHeight = document.body.clientHeight;
    }

    // for small pages with total height less then height of the viewport
    if(yScroll < windowHeight){
        pageHeight = windowHeight;
    } else {
        pageHeight = yScroll;
    }
      return pageHeight;
}
          
function getWidth(){

        var xScroll

        if (window.innerHeight && window.scrollMaxY) {
        xScroll = document.body.scrollWidth;
        } else if (document.body.scrollHeight > document.body.offsetHeight){ // all but Explorer Mac
        xScroll = document.body.scrollWidth;
        } else {
        xScroll = document.body.offsetWidth;
        }

        var windowWidth
        if (self.innerHeight) { // all except Explorer
        windowWidth = self.innerWidth;
        } else if (document.documentElement && document.documentElement.clientHeight) { // Explorer 6 Strict Mode
        windowWidth = document.documentElement.clientWidth;
        } else if (document.body) { // other Explorers
        windowWidth = document.body.clientWidth;
        }

        if(xScroll < windowWidth){
        pageWidth = windowWidth;
        } else {
        pageWidth = xScroll;
        }


      return pageWidth;
}

</script> 
<script language="javascript" type="text/javascript">
var zuobiankuangdu = 280;
function initgogo()
{
	document.getElementById("lianjie_top").style.width = document.body.scrollWidth;
document.getElementById("top_top").style.width = document.body.scrollWidth;
document.getElementById("zhugaodu").style.height = getHeight() - 90;
document.getElementById("leftFrame").style.height = getHeight() - 90 - 40;
document.getElementById("theObjTable").style.width = zuobiankuangdu;

	}
initgogo();
var yjyc = 1;
function hidezuo()
{
if(yjyc == 1)
{
document.getElementById("theObjTable").style.width = "16";
document.getElementById("qiehuantu").src = "sytp/cd_jt.jpg";
yjyc = 2;
}
else
{
document.getElementById("theObjTable").style.width = zuobiankuangdu;
document.getElementById("qiehuantu").src = "sytp/cd_jt2.jpg";
yjyc = 1;
	}
	}
</script>




<script language="javascript" type="text/javascript">
        var _LoadingName = "loaddiv";
        var _IFrameName = "rightFrame";
        var _LoadingText = "正在加载，请稍后……<br /><br /><img src='sytp/a.gif'  border='0' />";
        var intervalFrame;
        //var isIE = (browser.indexOf("Microsoft") != -1 && !window.opera) ? true : false;
        function $$$(id) { return document.getElementById(id); }
        
        function goUrl() {
            var iframe = $$$(_IFrameName);
            var loading = $$$(_LoadingName);
            iframe.style.display = "none";
            loading.style.display = "";
            loading.innerHTML = _LoadingText;
            //iframe.src = url;
            
            intervalFrame = window.setInterval(showFrame2, 100);


        }
        function goUrl2() {
            alert('s');


        }
        goUrl();

        function showFrame2() {
            var iframe = $$$(_IFrameName);
            var loading = $$$(_LoadingName);
            if (iframe.readyState == "complete") {
                setTimeout(showrtx, 2000); 
                
                
                loading.style.display = "none";
                iframe.style.display = "";
                window.clearInterval(intervalFrame);
                
            }
        }

        function showrtx() {
            if (window.top.frames.f8.location.href != document.getElementById("Hidden_rtxurl").value) {
                window.top.frames.f8.location.href = document.getElementById("Hidden_rtxurl").value;
            }


        }
        showtimer();
        window.onerror = function() {
            return true;
        }
//        var stopallFrame;
//        function stopallfr() {
//            stopallFrame = window.setInterval(stopallfr_run, 20000);
//        }
//        function stopallfr_run() {
//            window.top.frames.leftFrame.location.stop();
//            window.top.frames.rightFrame.location.stop();
//            window.top.frames.rtx_temp.location.stop();
//            window.top.frames.f1.location.stop();
//            window.top.frames.f2.location.stop();
//            window.top.frames.f8.location.stop();
//        window.clearInterval(stopallFrame);
        //        }
        //playSound("001.wav");
        setInterval(function () { blink.color = blink.color == '#0000ff' ? 'red' : 'blue' }, 1000)
</script>
<script type="text/javascript" language="javascript">



    //消除某些事件
    window.onload = function () {
     setInterval(unm, 500);
    };


//    $(document).ready(function () {
//        setInterval(unm, 500);
    //    });
    //通过class属性获取元素
    function getElementsByClassName(n) {
        var classElements = [], allElements = document.getElementsByTagName('*');
        for (var i = 0; i < allElements.length; i++) {
            if (allElements[i].className == n) {
                classElements[classElements.length] = allElements[i]; //某类集合
            }
        }
        return classElements;
    }
    function unm() {
        var RawButton = getElementsByClassName("RadWButton");
        for (var i = 0; i < RawButton.length; i++) {
            RawButton[i].removeAttribute("title");
            RawButton[i].removeAttribute("dypop");
        }
        var RadWWrapperHeaderCenter = getElementsByClassName("RadWWrapperHeaderCenter");

        for (var i = 0; i < RadWWrapperHeaderCenter.length; i++) {
            RadWWrapperHeaderCenter[i].removeAttribute("title");
            RadWWrapperHeaderCenter[i].removeAttribute("dypop");
        }

//        $(".RadWButton").each(function () {
//            $(this).removeAttr("title");
//            $(this).removeAttr("dypop");
//        });
//        $(".RadWWrapperHeaderCenter").each(function () {
//            $(this).removeAttr("title");
//            $(this).removeAttr("dypop");
//        });
        
    }


</script>