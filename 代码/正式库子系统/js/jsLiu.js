//数值减法得到数值
function GetJRNum(BNum,JNum,RNum)  //得到减法操作数
{
  RNum.value=BNum.value-JNum.value;
}  
//日期控件减法，得到天数
function  DateDiff(beginDate,  endDate,iDays)
{    
       var  arrbeginDate,  Date1,  Date2, arrendDate 
       arrbeginDate=  beginDate.value.split("-") 
       Date1=  new  Date(arrbeginDate[1]  +  '-'  +  arrbeginDate[2]  +  '-'  +  arrbeginDate[0])    //转换为2007-8-10格式
       arrendDate=  endDate.value.split("-")  
       Date2=  new  Date(arrendDate[1]  +  '-'  +  arrendDate[2]  +  '-'  +  arrendDate[0])  
       iDays.value= (Date1-  Date2)/  1000  /  60  /  60  /24   //转换为天数 
      //iDays  =  parseInt(Math.abs(Date1-  Date2)  /  1000  /  60  /  60  /24)    //转换为天数 
       return  false; 
 } 


