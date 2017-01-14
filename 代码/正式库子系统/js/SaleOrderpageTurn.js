// JScript 文件
function pagesubmit(btn)
{
        //alert('sdsds');
        var currpage = document.getElementById("txtpagenumber").value;
        var total_page = document.getElementById("txtpagecount").value;
        //alert('324234');
        switch(btn)
        {
                case "begin":{
                                if(currpage*1==1 || currpage==0)
                                {
                                    //alert("已到首页！");
                                    return false;
                                }
                                currpage=1;
                                break;
                             }
                case "previous":{
                                    currpage=currpage*1-1;
                                    if(currpage*1<1)
                                    {
                                        //alert("已到首页！");
                                        return false;
                                    }
                                    break;
                                }
                case "next":{
                                currpage=currpage*1+1;
                                if(currpage*1>total_page*1)
                                {
                                    //alert("已到尾页");
                                    return false;
                                }
                                break;
                            }
                case "end":{
                                if(total_page==0)
                                {
                                   currpage=0;
                                }
                                if(currpage*1==total_page*1)
                                {
                                    //alert("已到尾页");
                                    return false;
                                }
                                currpage=total_page;
                                break;
                            }
                case "jump":{
                                currpage=document.getElementById("jumppage").value;
                                if(currpage*1<1||currpage*1>total_page*1)
                                {
                                    //alert("输入不正确！");
                                    return false;
                                }
                                break;
                            }

            }
            document.getElementById('txtflag').value = '2';
            document.getElementById('txtpagenumber').value = currpage;
            document.getElementById('jumppage').value = currpage;
            document.form1.submit();
}

function pageDisable()
{
        var currpage = document.getElementById("txtpagenumber").value;
        var total_page = document.getElementById("txtpagecount").value;
        var begin = document.getElementById('begin');
        var previous = document.getElementById('previous');
        var next = document.getElementById('next');
        var end = document.getElementById('end');
        if(currpage*1 <= 1)
        {
            begin.disabled = 'disabled';
            previous.disabled = 'disabled';
        }
        if(total_page*1 <= 1 || total_page*1 == currpage*1)
        {
            next.disabled = 'disabled';
            end.disabled = 'disabled';
        }
}

function openview(number)
    {
        var rnd = Math.floor(Math.random() * 100000000);
        var module = document.getElementById('txtmodule').value;
        var WinSettings = "center:yes;resizable:no;dialogHeight:600px;dialogWidth:800px;"
	    var MyArgs = window.showModalDialog("SaleOrder_View_Detail.aspx?module="+module+"&number="+number+"&from=view&rnd="+rnd,MyArgs,WinSettings);
    }
    function openedit(number)
    {
        var module = document.getElementById('txtmodule').value;
        var pagenumber = document.getElementById('txtpagenumber').value;
	    window.location = "SaleOrder_Update.aspx?module="+module+"&number="+number+"&pagenumber="+pagenumber;
    }
    function opendelete(number)
    {
        if (!confirm("您确实要删除这条记录吗?"))
        {        
              return;
        }
        var module = document.getElementById('txtmodule').value;
        var WinSettings = "center:yes;resizable:no;dialogHeight:0px;dialogWidth:0px;"
	    var MyArgs = window.showModalDialog("SaleOrder_Delete.aspx?module="+module+"&number="+number,MyArgs,WinSettings);
        document.getElementById('txtflag').value = '2';
        document.form1.submit();
    }
    function opencheck(number)
    {
        var rnd = Math.floor(Math.random() * 100000000);
        var module = document.getElementById('txtmodule').value;
        var WinSettings = "center:yes;resizable:no;dialogHeight:600px;dialogWidth:800px;"
	    var MyArgs = window.showModalDialog("SaleOrder_View_Detail.aspx?module="+module+"&number="+number+"&from=check&rnd="+rnd,MyArgs,WinSettings);
        window.location.reload();
    }

function went()
{
    var aa = newopen.closed;
    if(aa)
    {
        window.location.reload();
    }
}


function onlyNumber()//只能输入数字0~9，小数点
{
	if ( !(((window.event.keyCode >= 48) && (window.event.keyCode <= 57)) || (window.event.keyCode == 13) || (window.event.keyCode == 46)))
	{
		window.event.keyCode = 0 ;
	}
}

