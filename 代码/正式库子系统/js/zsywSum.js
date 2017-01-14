var MoneySum=0;
var ProductSum=0;

function GetSum(priceObj,countObj,totalCount,totalMoney)
{
    //统计数量
    if(document.getElementById(countObj) !=null)
    {
        ProductSum += Number(document.getElementById(countObj).value);
        if(document.getElementById(totalCount) != null )
        {
            document.getElementById(totalCount).value = ProductSum;
        }
    }
    
    //统计金额
    if(document.getElementById(priceObj)!= null)
    {
        MoneySum = Number(document.getElementById(priceObj).value) * ProductSum ;
        
        if(document.getElementById(totalMoney) != null )
        {
            document.getElementById(totalMoney).value = MoneySum;
        }
    }   
    return false;
}

var Total ;
function SetSum(count,price,sumMoney)
{
    if(document.getElementById(count) != null && document.getElementById(price) !=null && document.getElementById(sumMoney) != null)
    {
        if(document.getElementById(count).value != "" && document.getElementById(price).value != "")
        {
            Total = document.getElementById(count).value;
            document.getElementById(sumMoney).value = fixTo((Number(Total)) * (Number(document.getElementById(price).value)),2);
        }
        else
        {
            document.getElementById(sumMoney).value = "";
        }
    }
    return false;
}


function request_html(paras){ 
var url = location.href;  
var paraString = url.substring(url.indexOf("?")+1,url.length).split("&");  
var paraObj = {}  
for (i=0; j=paraString[i]; i++){  
paraObj[j.substring(0,j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=")+1,j.length);  
}  
var returnValue = paraObj[paras.toLowerCase()];  
if(typeof(returnValue)=="undefined"){  
return "";  
}else{  
return returnValue;  
}  
}

if (request_html("module") == "GreenCoverSalesBill") {
    set_SetTC = window.setInterval(SetTC, 1000);
    //alert('a');LZCPLXDJY
}
function SetTC() {
    if (document.getElementById("LZCPLXGMFS").value == "直接购买") {
        document.getElementById("LZCPLXDJY").value = document.getElementById("LZCPLXCPSCLSJ").value;
    }
    if (document.getElementById("LZCPLXGMFS").value == "以旧换新") {
        document.getElementById("LZCPLXDJY").value = document.getElementById("LZCPLXCPYJHXJ").value;
    }
    SetSum('LZCPLXSLZ', 'LZCPLXDJY', 'LZCPLXJEY');
//如果是临时员工(临时人员信息表):
//如果是普通销售(临时订单) ：
//正常提成金额 = （用户首次购买的市场零售价－用户的以旧换新价）×90%×实际数量
//如果是以旧换新(临时订单)：
//以旧换新提成金额  = （用户首次购买的市场零售价－用户的以旧换新价）×90%×实际数量×10%


//如果是正式员工(包括正式和试用期)
//如果是普通销售(临时订单) ：
//正常提成金额 = （用户首次购买的市场零售价－用户的以旧换新价）×40%×实际数量
//如果是以旧换新(临时订单)：
//以旧换新提成金额  = （用户首次购买的市场零售价－用户的以旧换新价）×40%×实际数量×10%

    //LZCPLXYGTC   YGSX  GMFS
    if (document.getElementById("XSRSFZH") != null && document.getElementById("LZCPLXJEY") != null && document.getElementById("LZCPLXCPSCLSJ") != null && document.getElementById("LZCPLXCPYJHXJ") != null && document.getElementById("YGSX") != null && document.getElementById("LZCPLXGMFS") != null && document.getElementById("LZCPLXSLZ") != null) {
        var YGSX = document.getElementById("YGSX").value; //员工属性
        var XSRSFZH = document.getElementById("XSRSFZH").value; //销售人身份证号
        var LZCPLXJEY = document.getElementById("LZCPLXJEY").value; //金额
        var LZCPLXCPSCLSJ = document.getElementById("LZCPLXCPSCLSJ").value; //产品市场零售价
        var LZCPLXCPYJHXJ = document.getElementById("LZCPLXCPYJHXJ").value; //产品以旧换新价
        var GMFS = document.getElementById("LZCPLXGMFS").value; //购买方式
        var LZCPLXSLZ = document.getElementById("LZCPLXSLZ").value; //购买数量
        var LZCPLXYGTC = 0;


        if (YGSX == "机动兼职销售推广专员") {
            if (GMFS == "直接购买") {
                LZCPLXYGTC = ((Number(LZCPLXCPSCLSJ) - Number(LZCPLXCPYJHXJ))) * ((0.9)) * Number(LZCPLXSLZ);

            }
            if (GMFS == "以旧换新") {
                LZCPLXYGTC = ((Number(LZCPLXCPSCLSJ) - Number(LZCPLXCPYJHXJ))) * ((0.9)) * Number(LZCPLXSLZ) * ((0.15));
            }
        }
        if (YGSX == "销售推广主管" || YGSX == "正式销售推广专员") {
            if (GMFS == "直接购买") {
                LZCPLXYGTC = ((Number(LZCPLXCPSCLSJ) - Number(LZCPLXCPYJHXJ))) * ((0.4)) * Number(LZCPLXSLZ);
            }
            if (GMFS == "以旧换新") {
                LZCPLXYGTC = ((Number(LZCPLXCPSCLSJ) - Number(LZCPLXCPYJHXJ))) * ((0.4)) * Number(LZCPLXSLZ) * ((0.1));
            }
        }
        if (YGSX == "固定兼职销售推广专员") {
            if (GMFS == "直接购买") {
                LZCPLXYGTC = ((Number(LZCPLXCPSCLSJ) - Number(LZCPLXCPYJHXJ))) * ((0.8)) * Number(LZCPLXSLZ);
            }
            if (GMFS == "以旧换新") {
                LZCPLXYGTC = ((Number(LZCPLXCPSCLSJ) - Number(LZCPLXCPYJHXJ))) * ((0.8)) * Number(LZCPLXSLZ) * ((0.1));
            }
        }
        if(fixTo(LZCPLXYGTC, 2) > 0)
        {
                document.getElementById("LZCPLXYGTC").value = fixTo(LZCPLXYGTC, 2);
        }
        else
        {
                document.getElementById("LZCPLXYGTC").value = "0";
        }
        

    }
    else
    { document.getElementById("LZCPLXYGTC").value = ""; }

}

function fixTo(s, i) {
    if (s == null || s == "" || isNaN(s) || Math.round(s) == 0) return 0;
    i = Math.round(i);
    if (i == 0) return Math.round(s);
    if (i == null || isNaN(i) || i < 0) i = 2;
    var v = Math.round(s * Math.pow(10, i)).toString();
    if (/e/i.test(v)) return s;
    return v.substr(0, v.length - i) + "." + v.substr(v.length - i);
}   


function setZCJE(ZCJE,JE,ZJ)
{
    var JEObj,ZJObj;
    if(document.getElementById(ZCJE) != null && document.getElementById(JE) !=null && document.getElementById(ZJ) != null)
    {
        if(document.getElementById(JE).value != "" && document.getElementById(ZJ).value != "")
        {
            JEObj = document.getElementById(JE);
            ZJObj = document.getElementById(ZJ);
            document.getElementById(ZCJE).value = Number(JEObj.value) * Number(ZJObj.value);
        }
        else
        {
            document.getElementById(ZCJE).value = "";
        }
    }
    return false;
}

function setJSY(JSY,CZSR,ZCJE,CZFY)
{
    var ZCJEObj,CZFYObj;
    var JSYValue="";
    if(document.getElementById(JSY) != null && document.getElementById(CZSR) !=null && document.getElementById(ZCJE) != null && document.getElementById(CZFY) != null)
    {
        if(document.getElementById(CZSR).value != "")
        {
            
            JSYValue = document.getElementById(CZSR).value;
            if(document.getElementById(ZCJE).value != "")
            {
                JSYValue = JSYValue - Number(document.getElementById(ZCJE).value);
            }
            
            if(document.getElementById(CZFY).value != "")
            {
                JSYValue = JSYValue - Number(document.getElementById(CZFY).value);
            }
            
            document.getElementById(JSY).value = JSYValue;
        }
        else
        {
            document.getElementById(JSY).value = "";
        }
        
    }
    return false;
}

function setSLYK(SLYK,ZMSL,PDSL)
{
        var ZMSLObj,PDSLObj;
    if(document.getElementById(PDSL) != null && document.getElementById(ZMSL) !=null && document.getElementById(SLYK) != null)
    {
        if(document.getElementById(PDSL).value != "" && document.getElementById(ZMSL).value != "")
        {
            PDSLObj = document.getElementById(PDSL);
            ZMSLObj = document.getElementById(ZMSL);
            document.getElementById(SLYK).value = Number(PDSLObj.value) - Number(ZMSLObj.value);
        }
        else
        {
            document.getElementById(SLYK).value = "";
        }
    }
    return false;
}

function setYKJE(YKJE,DJ,SLYK)
{
    var DJObj,SLYKObj;
    if(document.getElementById(YKJE) != null && document.getElementById(DJ) !=null && document.getElementById(SLYK) != null)
    {
        if(document.getElementById(DJ).value != "" && document.getElementById(SLYK).value != "")
        {
            DJObj = document.getElementById(DJ);
            SLYKObj = document.getElementById(SLYK);
            document.getElementById(YKJE).value = Number(DJObj.value) * Number(SLYKObj.value);
        }
        else
        {
            document.getElementById(YKJE).value = "";
        }
    }
    return false;
}

//检验身份证号是不是满18位
function checkCard(filedName,length,errorMessage)
{
    if(document.getElementById(filedName)!=null && document.getElementById(filedName).value != "" )
    {
        if(document.getElementById(filedName).value.length <18 && document.getElementById(filedName+"label") != null)
        {
            document.getElementById(filedName+"label").innerHTML = errorMessage;
        }
        else
        {
            document.getElementById(filedName+"label").innerHTML ="";
        }
    }
}

//城市销售公司登录
var selectname = "CSXSGS";//下拉框名称
window.onload = function() {
    $.ajax({
        url: '../web/owncitylist.aspx',
        type: 'GET',
        dataType: 'html',
        timeout: 5000,
        error: function() {
            alert('Error loading');
        },
        success: function(html) {
            if (html != "nothing") {
                document.getElementById(selectname).options.length = 0;
                jsAddItemToSelect(document.getElementById(selectname), html, html);
            }

            //特殊模块的特殊处理
//            if (request_html("module") == "temp_employee") {
//                alert(document.forms[0].CS.options[document.forms[0].CS.selectedIndex].text);


//                var opt = document.createElement("OPTION");
//                opt.text = '无';
//                opt.value = '无';
//                //document.getElementById("CS").add(opt); //
//                document.forms[0].CS.options.add(opt, 0); //
//                document.forms[0].CS.options[0].selected = true;

//            }
        }
    });

}


//办事处登录
var selectname = "SSBSC";//下拉框名称
window.onload = function() {
    $.ajax({
        url: '../web/owncitylist.aspx',
        type: 'GET',
        dataType: 'html',
        timeout: 5000,
        error: function() {
            alert('Error loading');
        },
        success: function(html) {
            if (html != "nothing") {
                document.getElementById(selectname).options.length = 0;
                jsAddItemToSelect(document.getElementById(selectname), html, html);
            }
   
        }
    });

}




function jsAddItemToSelect(objSelect, objItemText, objItemValue) {        
      
        var varItem = new Option(objItemText, objItemValue);      
        objSelect.options.add(varItem);     
 
       
}


var ff_sptime = "DHSJ";



