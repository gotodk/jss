/* 张伟 2007.11.27
/*脚本方法说明: checkControl(,,,,,)
  *  obj     :要验证的控件名称
  *  type    :验证控件的类型:1:整数,2:小数.3
  *  empty   :验证控件是否为空
  *  length  :验证控件的最大长度
  *  maxValue :验证控件的最大值
  *  minValue :验证控件的最小值
*/

var totalFile = 1;
function checkControl(checkRule)
{
    var controlList;
    var control,type,empty,length,maxValue,minValue;
    //控件列表
    controlList = checkRule.split(".");
    for(var i=0; i<controlList.length; i++)
    {   
        if(controlList[i].split(":").length > 5)
        {
            //控件名称
            control = document.getElementById(controlList[i].split(":")[0]);
            
            //控件类型
            type    = controlList[i].split(":")[1];
            
            //是否为空
            empty   = controlList[i].split(":")[2];
            if(empty.toLowerCase() == "false")
            {
                empty = false;
            }
            else
            {
                empty = true;
            }
           
            //最大值
            maxValue = controlList[i].split(":")[3];
            
            //最小值
            minValue = controlList[i].split(":")[4];
            
            //长度 
            length = controlList[i].split(":")[5];
            
            //control = document.getElementById(controlList[i]);
            flag = true;
            
            //根据类型判断验证的方式
            switch (type)
            {
                case "TextBox":  //验证textbox文本
                        if ( checkString (control,empty,length) == false )
                        {
                            setControl(control);
                            return false;
                        }
                        break;
                        
                case "IntBox":  //验证整数
                        if ( checkInt (control,empty,maxValue,minValue) == false )
                        {
                            setControl(control);
                            return false;                   
                        }
                        break;
                        
                case "FloatBox":  //验证浮点数
                        if ( checkFloat (control,empty,maxValue,minValue) == false )
                        {
                            setControl(control);        
                            return false;              
                        }
                        break;
                case "DropDownList":
                        if ( checkDropDownList (control,empty) == false)
                        {
                            setControl(control);        
                            return false;   
                        }
                case "DateSelectBox":
                        if ( checkDateSelectBox(control,empty) == false)
                        {
                            setControl(control);        
                            return false;   
                        }
                case "MutiLineTextBox":
                        if ( checkMutiLineTextBox(control,empty) == false)
                        {
                            setControl(control);        
                            return false;                           
                        }    
                        break;
                case "3":  //验证上传控件
                        if ( checkUpFile(control,empty,length) == false )
                        {
                            setControl(control);    
                            return false;                      
                        }
                        else
                        {
                            checkFileDisplay (control,0);
                        }
                        break;
            }
        }
    }
     return true;
}


//取字符串长度
function GetLength(control)
{
    var len;
    len = control.value.replace( /^\s*/, "" ).replace( /\s*$/, "" ).length
    return len;
}

//验证是否为空
function checkIsEmpty(obj)
{
    if (obj.value.replace( /^\s*/, "" ).replace( /\s*$/, "" ) == "")
    {
        return false;
    }
    return true;
}

//使一控件选中，获取焦点
function setControl(control)
{
    control.focus();
    control.select();
}

//去掉无聊无字符
function Trim(control)
{
    var result = control.replace( /^\s*/, "" ).replace( /\s*$/, "" );
    return result;
}

//验证字符串
function checkString(control,empty,length)
{
    //若可为空
    if( empty == true )
    {   
        if ( length != "")
        {
            //当字符不为空时，进行长度检验
            if(GetLength(control) > 0 && GetLength(control) > length)
            {
                alert("已经超出最大限度!");
                return false;
            }
        }
    }
    else
    {
        if (checkIsEmpty(control) == false )
        {
            alert("页面内容输入不完整!");
            setControl(control);
            return false;
        }
        if(length != "")
        {
            if(GetLength(control) > length)
            {
                return false;
            }
        }
    }
    return true;
}

//验证整数
function checkInt(control,empty,maxValue,minValue)
{
    Flag = true;
    
    if ( empty == true )
    {
        //允许为空，若输入内容为空，则不再进行验证
        if (checkIsEmpty(control) == false )
        {
            return true;
        }
        
        //有输入内容，继续进行验证
        //验证整数格式
        if (checkIntFormat(control)==false)
        {
            return false;
        }
        
        //验证上限和下限
        if(checkMax_Min(control,maxValue,minValue)==false)
        {
            return false;
        }
        return true;
        
    }
    else
    {
        //判断是否为空,必须输入...
        if(checkIsEmpty(control)==false)
        {
            alert("必输不能为空!");
            setControl(control);
            return false;
        }
        
        //验证整数格式
        if (checkIntFormat(control)==false)
        {
            return false;
        }
        
        //验证上限和下限
        if(checkMax_Min(control,maxValue,minValue)==false)
        {
            return false;
        }
        return true;
    }
}

//验证DropDownList
function checkDropDownList(control,empty)
{
    if(empty == false)
    {
        if(checkIsEmpty(control) == false)
        {
            alert("请选择下拉列表中的内容!");
            setControl(control);
            return false;
        }
    }
    return true;
}

//验证日期
function checkDateSelectBox(control,empty)
{
    if(empty == false)
    {
        if(checkIsEmpty(control) == false)
        {
            alert("请选择日期!");
            setControl(control);
            return false;
        }
    }
    
    //var a=/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})/ ;
    return true;
}

//验证TextArea
function checkMutiLineTextBox(control,empty)
{
    if(empty == false)
    {
        if(checkIsEmpty(control) == false)
        {
            alert("多行文本框不能为空，请输入内容!");
            setControl(control);
            return false;
        }
    }
    return true;
}

//验证小数
function checkFloat(control,empty,maxValue,minValue)
{
    //若允许为空
    if ( empty == true )
    {
        //在允许为空的情况下，若为空，则返回true
        if( checkIsEmpty(control) == false )
        {
            return true;
        }
        
        //检查格式是否正确
        if( checkFloatFormat(control) == false )
        {
            return false;
        }
        
        //判断是否超过上限与下限
        if(!checkMax_Min(control,maxValue,minValue))
        {
            return false;
        }
        return true;
    }
     //不允许为空
    else
    {
        //判断是否为空
        if ( checkIsEmpty(control) == false )
        {
            alert("必输不能为空!");
            setControl(control);
            return false;
        }
        else
        {
            //不为空，则验证格式
            if ( checkFloatFormat(control) == false )
            {
                return false;
            }
            
            //判断是否超过上限与下限            
            if(!checkMax_Min(control,maxValue,minValue))
            {
                return false;
            }
            
            return true;
        }
    }
}

//验证小数的模式
function checkFloatFormat(control)
{
    var partter = /^(-?\d+)(\.\d+)?$/;
    
    if ( partter.test(control.value) == false)
    {
        alert("浮点数格式不正确!");
        return false;
    }
    return true;
}

//验证为整数
function checkIntFormat(control)
{
    var partter =/^-?\d+$/;
    if(control.value != "0")
    {
        if( partter.test(control.value ) == false || ( control.value.substring(0,1) =="0" ) )
        {
            alert ("整数格式不正确!");
            return false;
        }
    }
    return true;
}

//验证整数的最大上限和最小下限数..
function checkMax_Min(control,maxV,minV)
{
    var maxValue ;
    var minValue ;
    
    if (maxV.toString() != "")
    {
        maxValue = Number(maxV);
        if(Number(control.value) > maxValue)
        {
            alert("超过最大上限值"+maxValue);
            return false;
        }
    }
    
    //先判断是否有最小上限
    if ( minV.toString() != "")
    {
        minValue = Number(minV);
        //比较输入值与最小上限值
        if(Number(control.value) < minValue)
        {
            alert("超过最小上限值:"+minValue);
            return false;
        }
    }
    return true;
}

//验证字符串是否为正确的路径格式，和文件名是否为正确格式..
function checkPath(control)
{
    var fpath = control.value;
    myFile = new ActiveXObject("Scripting.FileSystemObject");
    if(!myFile.FileExists(fpath))
    {
           return false;
    }
}

//验证文件扩展名
function chk_ext(f_path)
{
    f_path = f_path.replace(/s\g/,"");
    ext = f_path.substring(f_path.lastIndexOf(".") + 1, f_path.length);
    //根据需求定制
    var accept_ext = new Array("doc", "pdf", "bmp", "jpeg", "jpg", "gif", "ppt", "xls", "rar", "zip", "txt", "xml", "DOC", "PDF", "BMP", "JPEG", "JPG", "GIF", "PPT", "XLS", "RAR", "ZIP", "TXT", "XML");
    var flag = false;

    if(ext != '')
    {
       for(var i=0; i<accept_ext.length; i++)
       {
            if( ext == accept_ext[i] )
            {
                flag = true;
            }
       }
    }
    else
    {
        flag = false;
    }
    return flag;
}

//验证上传文件的格式,是否为空,文件名等
function checkUpFile(control,empty)
{
    if(empty == 'true')
    {
        empty = true;
    }
    else
    {
        empty = false;
    }

    //判断是否为空
    if (empty != true)
    {
        if ( checkIsEmpty(document.getElementById(control)) == false )
        {
            checkFileDisplay(control,1);
            return false;
        }
        
        if ( checkPathAndExt(control) == false )
        {
            return false;
        }
    }
    else
    {
        //判断是否为空
        if ( checkIsEmpty(document.getElementById(control)) == false )
        {
            return true;
        }
        
        if ( checkPathAndExt(control) == false )
        {       
            return false;
        }
    }
    //验证通过
    checkFileDisplay(control,0)
    return true;
}

//判断文件是否存在
function checkPathAndExt(control)
{
        //验证文件的扩展名是否正确
        if ( chk_ext(document.getElementById(control).value) == false )
        {
           checkFileDisplay(control,2);
           return false;
        } 
        return true;
}

//根据上传文件控件错误类型，显示指定胡错误信息:
//1.为空时，显示为空的错误信息
//2.文件不存在时，显示借误信息
function checkFileDisplay(control,type)
{
    if( type == 1 )
    {
      //  document.getElementById(control + "nullMsg").style.display ='inline';
      //  document.getElementById(control + "exteMsg").style.display ='none';
        
        setMsgDisplay(control + "nullMsg",'inline'); 
        setMsgDisplay(control + "exteMsg",'none');
        
        if(document.getElementById(control) != null)
        {
            document.getElementById(control).focus();
            document.getElementById(control).select();  
        }
    }
    else if(type == 2)
    {
        //document.getElementById(control + "nullMsg").style.display ='none';
        //document.getElementById(control + "exteMsg").style.display ='inline'; 
        
        setMsgDisplay(control + "nullMsg",'none'); 
        setMsgDisplay(control + "exteMsg",'inline');
        
        if(document.getElementById(control) != null)
        {
            document.getElementById(control).focus();
            document.getElementById(control).select();
        }
    }
    else
    {
       // document.getElementById(control + "nullMsg").style.display ='none';
       // document.getElementById(control + "exteMsg").style.display ='none';
       
        setMsgDisplay(control + "nullMsg",'none'); 
        setMsgDisplay(control + "exteMsg",'none');
    }
}

function setMsgDisplay(control,strStyle) {
    if (document.getElementById(control) != null) {
        document.getElementById(control).style.display = strStyle;
    }
}


//弹出窗口
function ShowDialog(url, iWidth, iHeight) 
{
    var iTop = (window.screen.height - iHeight) / 2;
    var iLeft = (window.screen.width - iWidth) / 2;
    var wnd = window.open(url, "Detail", "Scrollbars=no,Toolbar=no,Location=no,Direction=no,Resizeable=no,Width=" + iWidth + " ,Height=" + iHeight + ",top=" + iTop + ",left=" + iLeft);
    return wnd;
}

//提交前的验证
function Validate()
{
    return true;
}

//验证表单内所有上传文件的扩展名
function ValidateFile()
{
    var index;
    
    if(document.getElementsByTagName("input").length > 0)
    {
        var objElement = document.getElementsByTagName("input");
        
        for (index=0; index<objElement.length; index++)   
        {   
          if (objElement[index].type == "file" && objElement[index].value != "")
          {
            if(chk_ext(objElement[index].value) == false)
            {
                checkFileDisplay(objElement[index].id,2);
                alert("允许上传文件类型为:\n *.doc\t *.pdf\n *.bmp\t *.jpeg\n *.jpg\t *.gif\n *.ppt\t *.xls\n *.rar\t *.zip\n *.txt\t *.xml\n您所上传文件格式发生错误.\n请重新选择上传文件!");
                document.getElementById(objElement[index].id).focus();
                document.getElementById(objElement[index].id).select();            
                return false;
            }
          }   
        } 
    }
    return true;
}

//删除当前行
function removeRow(r,parameterName)
{
    var root = r.parentNode;
    var allRows = root.getElementsByTagName('tr')
    if(allRows.length>2)
      {
        //删除所保存该行的值
        removeHidValue(parameterName,r);
        
        //删除该行
        root.removeChild(r);
      }
    else
        alert("已经移除最后一行了");
}

//清除控件内容
function clearControl(controlList)
{
    for(forIndex =0; forIndex<controlList.length;forIndex++)
    {
        document.getElementsByName(controlList[forIndex])[0].value = "";
    }
}

//移除隐藏控件的最后一项值
function removeHidValue(parameterName,r)
{
    if (parameterName != "")
    {
        //产生控件名称集合
        var controlList = parameterName.split(',');
        
        //外层循环变量
        var forIndex =0;
        //内层循环变量
        var innerIndex = 0;
        
        var hidObj;
        
        var hidArray;
        
        for (forIndex =0; forIndex<controlList.length;forIndex++)
        {
            var hidObj = document.getElementsByName(controlList[forIndex]+"hid")[0];
           
            //去掉hidArray 中最后一个多余的分隔符
            hidArray=hidObj.value.substring(0,hidObj.value.length-1);  
             
            //分隔字符串
            hidArray = hidArray.split(',');
           
            //移出数组最后一个元素
            hidArray.pop();
            
            //清空之间的值
            hidObj.value = "";
            //重新赋值
            for (innerIndex=0 ;innerIndex<hidArray.length ;innerIndex++)
            {
                hidObj.value = hidObj.value + hidArray[innerIndex] +","
            }
        }
    }
}

//取指定子表的表头<Table>
function GetTableTag(subName)
{
    var tableTag = document.getElementById(subName+"_div").innerHTML;
    if(tableTag.split('<TBODY>').length > 0)
    {
        tableTag= tableTag.split('<TBODY>')[0];
    }
    else
    {
        tableTag = "<TABLE id=\""+subName+"\" width=\"100%\">";
    }
    return tableTag;
}

//新增上传控件
function addFile(offsetName,objName,empty)
{
  if(empty == 'true')
    {
    empty = true;
    }
    else
    {
    empty = false;
    }
    
   var obj = document.getElementById(offsetName); 
   var createName = objName + totalFile;
   var str;
   str = "<span id="+createName+"top><br/>";
   str += "<INPUT type=\"file\" size=\"100%\" NAME=\""+ createName+ "\" id=\""+ createName +"\" onchange=\"checkUpFile('"+createName+"','"+ empty.toString() +"');\" onkeydown=\"return false;\">";
   str += "<input type=\"image\" id=\""+createName+"\" name =\""+ createName +"\" onclick=\"return delFileControl('"+ offsetName+"','"+ createName +"')\" src=\"images/deleteFile.gif\"/>"
   if(empty == false)
   {
        str += "<span id=\"" + createName + "nullMsg\"  style=\"display:none\">&nbsp;上传文件不能为空。</span>";
   }
   str += "<span id=\"" + createName + "exteMsg\"  style=\"display:none\">&nbsp;上传文件的扩展名不符合。</span>";
   str +="</span>";
   obj.insertAdjacentHTML("beforeEnd",str);
   totalFile = totalFile+1;
}

//删除<span>中的上传控件
function delFileControl(offsetName,objName)
{
    var obj = document.getElementById(offsetName);    
    
   // 移除控件
   if(document.getElementById(objName)!=null)
   {
        obj.removeChild(document.getElementById(objName+"top"));
   }
    return false;
}

//判断是否为日期时间类型
function  CheckDateTime(str)
{                            
       var  reg = /^(\d+)-(\d{1,2})-(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;    
       var  r = str.match(reg);    
       if(r==null) return  str;    
       r[2]=r[2]-1;    
       var  d=new  Date(r[1],  r[2],r[3],  r[4],r[5],  r[6]);    
       if((d.getFullYear()==r[1])&& (d.getMonth()==r[2])&& (d.getDate()==r[3]) &&(d.getHours()==r[4]) &&(d.getMinutes()==r[5]) &&(d.getSeconds()==r[6])) 
       return (d.getFullYear()+ '-'+ (d.getMonth()+1)+ '-' + d.getDate()); 
       else return str; 
}  

///设置主键单号为只读
function setReadOnlyNumber()
{
    if(document.getElementById("Number") != null)
    {
        document.getElementById("Number").disabled=true;
    }
}

//显示条件层
function display()
{
    document.getElementById('query').style.display = 'block';
}

//隐藏条件层
function nodisp()
{
    document.getElementById('query').style.display = 'none';
}
