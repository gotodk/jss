//如需其他页面，增加一个js,起个名字，把<script src="js/新名字.js" type="text/javascript"></script>放到</html>后面
//使用说明：把按钮区域改成 <div id="djycqymain"><div id="djycqy_show">原按钮区域的代码</div></div>
function clickyc() {
    document.getElementById("djycqy_show").style.display = "none";
    var divload = document.createElement("div");
    divload.style.width = "100%";
    divload.style.height = "100px";
    divload.style.backgroundColor = "#FFFFFF";
    divload.style.textAlign = "center";
    divload.innerHTML = "<br><br>正在提交……<br><img src='images/loding.gif' />";
    document.getElementById("djycqymain").appendChild(divload);

}
//使用说明： 需要哪些按钮放重复提交，增加上就行，setAttribute后面的参数用动。
/////document.getElementById("btnAdd").setAttribute('onmouseup', document.all ? eval(function () { clickyc() }) : 'javascript:clickyc()');
/////document.getElementById("btnSave").setAttribute('onclick', document.all ? eval(function () { clickyc() }) : 'javascript:clickyc()'); 