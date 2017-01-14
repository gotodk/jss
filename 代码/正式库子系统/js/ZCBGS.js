// JScript 文件

//doCheckBlur('SFWD','SYQX')

function doCheckBlur(sfwd,syqx)
{
    var objSfwd;
    var objSyqx;
    
    objSfwd = document.getElementsByName(sfwd)[0];
    objSyqx = document.getElementsByName(syqx)[0];
    
    if(objSfwd.value == "是" )
    {
        objSyqx.style.display='inline';
        document.getElementById('sprytextfieldSYQX').style.display='inline';
        objSyqx.focus();
        return false;
    }
    else
    {
       objSyqx.style.display='none';
       document.getElementById('sprytextfieldSYQX').style.display='none';
       return true;
    }
    return true;
}

function doBlur(sfwd,syqx)
{
    var objSfwd;
    var objSyqx;
    
    objSfwd = document.getElementsByName(sfwd)[0];
    objSyqx = document.getElementsByName(syqx)[0];
    
    if(objSfwd.value == "是" && objSyqx.value == "" )
    {
        objSyqx.focus();
        alert("使用期限必须填写!");
        return false;
    }
    return true;
}