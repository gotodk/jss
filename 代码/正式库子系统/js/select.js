// JavaScript 文件

///上层select控件的值,下层控件ID,下层控件的默认值,值所保存的数组
function ChangeSelect(ParentValue, NextId, NextSelectedValue, ArrObj)
{
    return false;
}


///根据上层select控件的值，确定childIDStr是否显示
//function childDisplay(ParentID,strValue,childIDStr)
//{
//    Array controlArray = childStr.split(',');
//    if(document.getElementById(ParentID) != null)
//    {
//        if(document.getElementById(ParentID).value == strValue)
//        {
//            for(var i=0;i<controlArray.length;i++)
//            {
//                if(document.getElementById(controlArray[i]) != null)
//                {
//                    document.getElementById(controlArray[i]).style.display = 'inline';
//                }
//            }
//        }
//        else
//        {
//            for(var i=0;i<controlArray.length;i++)
//            {
//                if(document.getElementById(controlArray[i]) != null)
//                {
//                    document.getElementById(controlArray[i]).style.display = 'none';
//                }
//            }
//        }
//    }
//}