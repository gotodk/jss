
var gaidongstr = "";
window.onload = onloadpagedo;

//开始对比,用法 return duibigaidong();
function duibigaidong() {
    get_gaidong();

    if (gaidongstr != document.getElementById("tbcheckstrtb").value) {
        //发现改动
        //alert("有改动"); return duibigaidong();

        return true;
    }
    else {
        //没有改动
       // alert(gaidongstr + "\n" + document.getElementById("tbcheckstrtb").value);
        return false;
    }
}

//加载后获取原始值
function onloadpagedo() {
    CreateInput_one();
    get_gaidong();
    gaidongstr = document.getElementById("tbcheckstrtb").value;
    //alert(gaidongstr);

}

//获取对比值
function get_gaidong() {

    //var Elements = document.all;
    var Elements = document.getElementsByTagName("*");
    var tbcheckstr = document.getElementById("tbcheckstrtb");
    tbcheckstr.value = "";
    var msgs;
    var i;
    for (i in Elements) {
        //alert(Elements[i].type);
        if (Elements[i].type == "text") {
            tbcheckstr.value = tbcheckstr.value + Elements[i].value + "|";
            //alert(Elements[i].value);
        }
        if (Elements[i].type == "select-one") {

            tbcheckstr.value = tbcheckstr.value + Elements[i].value + "|";
            //alert(Elements[i].value);
        }
        if (Elements[i].type == "checkbox") {

            if (Elements[i].checked) {
                //alert(Elements[i].name);
                tbcheckstr.value = tbcheckstr.value + Elements[i].name + "|";
            }
        }

    }


    var objRadio = document.getElementsByTagName("input");
    for (var i = 0; i < objRadio.length; i++) {
        if (objRadio(i).type == "radio") {
            if (objRadio(i).checked) {
                //alert(objRadio(i).name);
                tbcheckstr.value = tbcheckstr.value + objRadio(i).name + "|";
                tbcheckstr.value = tbcheckstr.value + objRadio(i).value + "|";
            }
        }
    }

    //document.getElementById("TextBox1").value = checkoldstr;
    //return checkoldstr;

}



//创建 input 文本框
function CreateInput_one() {
    var input = document.createElement("input");
    input.type = "text";
    input.id = "tbcheckstrtb";
    input.value = "";
    input.style.display = "none";
    document.body.appendChild(input);
    //如果使用CSS定义input样式 可以用 className 指定样式名，如用JS创建，放开下面的注释即可
    //input.className = "TextStyle";
    //使用以下定义，需注释 //input.className = "TextStyle";
    /*input.style.width = "300px";
    input.style.height = "16px";
    input.style.lineHeight = "16px";
    input.style.border = "1px solid #006699";
    input.style.font = "12px 'Microsoft Sans Serif'";
    input.style.padding = "2px";
    input.style.color = "#006699";*/
    //document.getElementById("showText").appendChild(input);
}
