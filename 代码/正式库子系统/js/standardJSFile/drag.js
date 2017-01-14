var isIE = navigator.userAgent.toLowerCase().indexOf("msie") != -1;
var isIE6 = navigator.userAgent.toLowerCase().indexOf("msie 6.0") != -1;
var isGecko = navigator.userAgent.toLowerCase().indexOf("gecko") != -1;
var isQuirks = document.compatMode == "BackCompat";
var Drag = {
    "obj": null,
    "init": function (handle, dragBody, e) {
        if (e == null) {
            handle.onmousedown = Drag.start;
        }
        handle.root = dragBody;

        if (isNaN(parseInt(handle.root.style.left))) handle.root.style.left = "0px";
        if (isNaN(parseInt(handle.root.style.top))) handle.root.style.top = "0px";
        handle.root.onDragStart = new Function();
        handle.root.onDragEnd = new Function();
        handle.root.onDrag = new Function();
        if (e != null) {
            var handle = Drag.obj = handle;
            e = Drag.fixe(e);
            var top = parseInt(handle.root.style.top);
            var left = parseInt(handle.root.style.left);
            handle.root.onDragStart(left, top, e.pageX, e.pageY);
            handle.lastMouseX = e.pageX;
            handle.lastMouseY = e.pageY;
            document.onmousemove = Drag.drag;
            document.onmouseup = Drag.end;
        }
    },
    "start": function (e) {
        var handle = Drag.obj = this;
        e = Drag.fixEvent(e);
        var top = parseInt(handle.root.style.top);
        var left = parseInt(handle.root.style.left);
        //alert(left)
        handle.root.onDragStart(left, top, e.pageX, e.pageY);
        handle.lastMouseX = e.pageX;
        handle.lastMouseY = e.pageY;
        document.onmousemove = Drag.drag;
        document.onmouseup = Drag.end;
        return false;
    },
    "drag": function (e) {
        e = Drag.fixEvent(e);

        var handle = Drag.obj;
        var mouseY = e.pageY;
        var mouseX = e.pageX;
        var top = parseInt(handle.root.style.top);
        var left = parseInt(handle.root.style.left);

        if (isIE) { Drag.obj.setCapture(); } else { e.preventDefault(); }; //作用是将所有鼠标事件捕获到handle对象，对于firefox，以用preventDefault来取消事件的默认动作：

        var currentLeft, currentTop;
        currentLeft = left + mouseX - handle.lastMouseX;
        currentTop = top + (mouseY - handle.lastMouseY);
        //handle.root.style.left = currentLeft + "px";
        handle.root.style.top = currentTop + "px";
        //开始-----------------------------
        //网页可见全文宽
        var screenWith = document.body.clientWidth;
        //网页可见全文高
        var screenHeight = document.body.clientHeight;
        //alert(left)
        if (currentLeft >= 0 && (currentLeft) <= (screenWith - handle.offsetWidth)) {
            handle.root.style.left = currentLeft + "px";
        }
        else if ((currentLeft) < 0) { //超出左边界
            handle.root.style.left = "0px";
        }
        else if ((currentLeft) > (screenWith - handle.offsetWidth)) {
            handle.root.style.left = (screenWith - handle.offsetWidth) + "px";
        }
        //判断是否得到上边界
        if (currentTop <= 0) {
            handle.root.style.top = "0px";
        }
        else if (currentTop >= 0 && (currentTop) <= (screenHeight - handle.root.offsetHeight)) {
            //alert("1");

            handle.root.style.top = currentTop + "px";
        }
        else if ((currentTop) > (screenHeight- handle.offsetHeight)) {
            //alert("3");
            //alert("currentTop：" + currentTop + "screenHeight - handle.offsetHeight：" + (screenHeight - handle.offsetHeight));
            handle.root.style.top = (screenHeight - handle.offsetHeight) + "px";
        }
        //结束------------------------------------

        handle.lastMouseX = mouseX;
        handle.lastMouseY = mouseY;
        handle.root.onDrag(currentLeft, currentTop, e.pageX, e.pageY);
        return false;
    },
    "end": function () {
        if (isIE) { Drag.obj.releaseCapture(); }; //取消所有鼠标事件捕获到handle对象
        document.onmousemove = null;
        document.onmouseup = null;
        Drag.obj.root.onDragEnd(parseInt(Drag.obj.root.style.left), parseInt(Drag.obj.root.style.top));
        Drag.obj = null;
    },
    "fixEvent": function (e) {//格式化事件参数对象
        var sl = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
        var st = Math.max(document.documentElement.scrollTop, document.body.scrollTop);
        if (typeof e == "undefined") e = window.event;
        if (typeof e.layerX == "undefined") e.layerX = e.offsetX;
        if (typeof e.layerY == "undefined") e.layerY = e.offsetY;
        if (typeof e.pageX == "undefined") e.pageX = e.clientX + sl - document.body.clientLeft;
        if (typeof e.pageY == "undefined") e.pageY = e.clientY + st - document.body.clientTop;

        return e;
    }
};



//注释的东西
/*
        $(document).ready(function () {
            //JS话语描述
            //用户在可拖动区域onmousedown，并在onmousedown的情况下触发onmousemove事件，当onmouseup的时候移除onmousemove事件。
            var posX;
            var posY;
            //网页可见全文宽
            var screenWith = document.body.clientWidth;
            //网页可见全文高
            var screenHeight = document.body.clientHeight;
            //得到id为divProductMove的DIV的宽度
            var divWidth;
            //得到id为divProductMove的DIV的高度
            var divHeight;
            var divProductMove = $("#divProductMove")[0];
            $("#divMove").mousedown(function (e) {
                if (!e) {
                    e = window.event;
                }
                posX = e.clientX - parseInt(divProductMove.offsetLeft);
                posY = e.clientY - parseInt(divProductMove.offsetTop);
                document.onmousemove = divMouseMove;
            });
            document.onmouseup = function (e) {
                document.onmousemove = null;
            }
            function divMouseMove(event) {
                if (event == null) {
                    event = window.event;
                }
                divProductMove.style.top = (event.clientY - posY) + "px";
                //判断是否超出左右边界
                $("#divShow").html("event.clientX:" + event.clientX.toString() + " posX:" + posX.toString() + " screenWith:" + screenWith + " - divWidth:" + divWidth);
                if ((event.clientX - posX) >= 0 && (event.clientX - posX) <= (screenWith - divWidth)) {
                    divProductMove.style.left = (event.clientX - posX) + "px";
                }
                else if ((event.clientX - posX) < 0) { //超出左边界
                    divProductMove.style.left = "0px";
                }
                else if ((event.clientX - posX) > (screenWith - divWidth)) {
                    divProductMove.style.left = (screenWith - divWidth) + "px";
                }
                //判断是否得到上边界
                if ((event.clientY - posY) <= 0) {
                    divProductMove.style.top = "0px";
                }
            }
*/
