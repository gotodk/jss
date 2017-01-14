//如需其他页面，增加一个js,起个名字，把<script src="js/新名字.js" type="text/javascript"></script>放到</html>后面
//使用说明：把按钮区域改成 <div id="djycqymain"><div id="djycqy_show">原按钮区域的代码</div></div>
function clickyc(wz) {
    document.getElementById("djycqy_show").style.display = "none";
    var divload = document.createElement("div");
    divload.style.width = "100%";
    divload.style.height = "100px";
    divload.style.backgroundColor = "#f7f7f7";
    if (wz != undefined) {
        divload.style.textAlign = wz;
    }
    divload.innerHTML = "<br><br>正在提交……<br><img src='/Web/images/standardImageFile/loding.gif' />";
    document.getElementById("djycqymain").appendChild(divload);

}

function clickyc2(wz) {
    document.getElementById("djycqy_show2").style.display = "none";
    var divload = document.createElement("div");
    divload.style.width = "100%";
    divload.style.height = "100px";
    divload.style.backgroundColor = "#f7f7f7";
    if (wz != undefined) {
        divload.style.textAlign = wz;
    }
    divload.innerHTML = "<br><br>正在提交……<br><img src='/Web/images/standardImageFile/loding.gif' />";
    document.getElementById("djycqymain2").appendChild(divload);

}

function clickyc3(wz) {
    document.getElementById("djycqy_show3").style.display = "none";
    var divload = document.createElement("div");
    divload.style.width = "100%";
    divload.style.height = "100px";
    divload.style.backgroundColor = "#f7f7f7";
    if (wz != undefined) {
        divload.style.textAlign = wz;
    }
    divload.innerHTML = "<br><br>正在提交……<br><img src='/Web/images/standardImageFile/loding.gif' />";
    document.getElementById("djycqymain3").appendChild(divload);

}
//4\5\6\7专门用于富美网登录页面
function clickyc4(wz) {
    document.getElementById("djycqy_show1").style.display = "none";
    var divload = document.createElement("div");
    divload.style.width = "100%";
    divload.style.height = "22px";
    divload.style.backgroundColor = "#f7f7f7";
    if (wz != undefined) {
        divload.style.textAlign = wz;
    }
    divload.innerHTML = "正在提交……";
    document.getElementById("djycqymain1").appendChild(divload);

}
function clickyc5(wz) {
    document.getElementById("djycqy_show2").style.display = "none";
    var divload = document.createElement("div");
    divload.style.width = "100%";
    divload.style.height = "22px";
    divload.style.backgroundColor = "#f7f7f7";
    if (wz != undefined) {
        divload.style.textAlign = wz;
    }
    divload.innerHTML = "正在提交……";
    document.getElementById("djycqymain2").appendChild(divload);

}
function clickyc6(wz) {
    document.getElementById("djycqy_show3").style.display = "none";
    var divload = document.createElement("div");
    divload.style.width = "100%";
    divload.style.height = "22px";
    divload.style.backgroundColor = "#f7f7f7";
    if (wz != undefined) {
        divload.style.textAlign = wz;
    }
    divload.innerHTML = "正在提交……";
    document.getElementById("djycqymain3").appendChild(divload);

}
function clickyc7(wz) {
    document.getElementById("djycqy_show4").style.display = "none";
    var divload = document.createElement("div");
    divload.style.width = "100%";
    divload.style.height = "22px";
    divload.style.backgroundColor = "#f7f7f7";
    if (wz != undefined) {
        divload.style.textAlign = wz;
    }
    divload.innerHTML = "正在提交……";
    document.getElementById("djycqymain4").appendChild(divload);

}

//使用说明： 需要哪些按钮放重复提交，增加上就行，setAttribute后面的参数用动。
/////document.getElementById("btnAdd").setAttribute('onmouseup', document.all ? eval(function () { clickyc() }) : 'javascript:clickyc()');
/////document.getElementById("btnSave").setAttribute('onclick', document.all ? eval(function () { clickyc() }) : 'javascript:clickyc()'); 