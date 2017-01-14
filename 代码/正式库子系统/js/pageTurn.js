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


    function openview_free(url) {
        var dtitle = document.getElementById('ov_ttf').value;
        var WinSettings = "center:yes;resizable:no;dialogHeight:600px;dialogWidth:800px;"
        var diag = new Dialog("Diag1");
        dig_public = diag;
        try {
            var zbk_yhb = 0;
            var zbk_yhb1 = 0;
            zbk_yhb = parent.document.getElementById("theObjTable").style.width;
            diag.Width = parent.document.body.scrollWidth - 200;
            zbk_yhb1 = parent.getHeight() - 200;
            diag.Height = zbk_yhb1;
        }
        catch (err) { diag.Width = document.body.scrollWidth - 100; diag.Height = 500; }
        diag.Title = dtitle;
        diag.URL = url;
        diag.ShowMessageRow = false;
        diag.MessageTitle = "";
        diag.Message = "";
        //diag.OKEvent = zAlert; //点击确定后调用的方法
        diag.show();
        try {
            parent.initgogo();
        } catch (err) { }
    }

    
function openview_spcial(number,url)
    {
        var rnd = Math.floor(Math.random() * 100000000);
        var module = document.getElementById('txtmodule').value;
        var WinSettings = "center:yes;resizable:no;dialogHeight:600px;dialogWidth:800px;"
        //window.open("WorkFlow_View_Detail.aspx?module="+module+"&number="+number,'child','width=400,height=300,left=200,top=200');
        //var MyArgs = window.showModalDialog(url + "?module=" + module + "&number=" + number + "&from=view&rnd=" + rnd, MyArgs, WinSettings);


        var diag = new Dialog("Diag1");
        dig_public = diag;
        try {
            var zbk_yhb = 0;
            var zbk_yhb1 = 0;
            zbk_yhb = parent.document.getElementById("theObjTable").style.width;
            diag.Width = parent.document.body.scrollWidth - 200;
            zbk_yhb1 = parent.getHeight() - 200;
            diag.Height = zbk_yhb1;
        }
        catch (err) { diag.Width = document.body.scrollWidth - 100; diag.Height = 500; }
        diag.Title = "特殊操作-单号: " + number + "";
        diag.URL = url + "?module=" + module + "&number=" + number + "&from=view&rnd=" + rnd;
        diag.ShowMessageRow = false;
        diag.MessageTitle = "关于表单查看";
        diag.Message = "在这里查看表单详情。";
        //diag.OKEvent = zAlert; //点击确定后调用的方法
        diag.show();
        try {
            parent.initgogo();
        } catch (err) { }
    }


    function openview_spcial_cj(number, url) {
        //var rnd = Math.floor(Math.random() * 100000000);
       // var module = document.getElementById('txtmodule').value;
        var WinSettings = "center:yes;resizable:no;dialogHeight:600px;dialogWidth:800px;"
        //window.open("WorkFlow_View_Detail.aspx?module="+module+"&number="+number,'child','width=400,height=300,left=200,top=200');
        //var MyArgs = window.showModalDialog(url + "?module=" + module + "&number=" + number + "&from=view&rnd=" + rnd, MyArgs, WinSettings);


        var diag = new Dialog("Diag1");
        dig_public = diag;
        try {
            var zbk_yhb = 0;
            var zbk_yhb1 = 0;
            zbk_yhb = parent.document.getElementById("theObjTable").style.width;
            diag.Width = parent.document.body.scrollWidth - 200;
            zbk_yhb1 = parent.getHeight() - 200;
            diag.Height = zbk_yhb1;
        }
        catch (err) { diag.Width = document.body.scrollWidth - 100; diag.Height = 500; }
        diag.Title = "拜访记录表抽检与回访-单号: " + number + "";
        diag.URL = url + "?number=" + number;
        diag.ShowMessageRow = false;
        diag.MessageTitle = "关于表单查看";
        diag.Message = "在这里查看表单详情。";
        //diag.OKEvent = my_closediag(diag); //点击确定后调用的方法
        diag.show();
        try {
            parent.initgogo();
        } catch (err) { }
    }

    var dig_public;
    function my_closediag() {
        dig_public.close();
    }
    
function openview(number) {
        var rnd = Math.floor(Math.random() * 100000000);
        var module = document.getElementById('txtmodule').value;
        var WinSettings = "center:yes;resizable:no;dialogHeight:600px;dialogWidth:800px;"
        //window.open("WorkFlow_View_Detail.aspx?module="+module+"&number="+number,'child','width=400,height=300,left=200,top=200');
        //var MyArgs = window.showModalDialog("WorkFlow_View_Detail.aspx?module=" + module + "&number=" + number + "&from=view&rnd=" + rnd, MyArgs, WinSettings);


        var diag = new Dialog("Diag1");
        dig_public = diag;
        try {
            var zbk_yhb = 0;
            var zbk_yhb1 = 0;
            zbk_yhb = parent.document.getElementById("theObjTable").style.width;
            diag.Width = parent.document.body.scrollWidth - 200;
            zbk_yhb1 = parent.getHeight() - 300;
            diag.Height = zbk_yhb1;
        }
        catch (err) { diag.Width = document.body.scrollWidth - 100; diag.Height = 500; }
        diag.Title = "查看表单详细内容-单号: " + number + "";
        diag.URL = "WorkFlow_View_Detail.aspx?module=" + module + "&number=" + number + "&from=view&rnd=" + rnd;
        diag.ShowMessageRow = true;
        diag.MessageTitle = "关联模块查看：";
        if (module == "FM_XHDPJYQS" || module == "FM_GZBX" || module == "FM_YHJFJXG" || module == "Web_GZBX_FHY" || module == "Web_WLZZFW_ZXJY" || module == "Web_WLZZFW_ZXTSFHY" || module == "Web_WLZZFW_ZXZXFHY" || module == "OnLineOrder" || module == "Web_HBHS_FHY" || module == "FWPT_YJYJJFK" || module == "FM_YJYJY" || module == "FWPT_XHDYHFK") {
            diag.Message = "&nbsp;&nbsp; <a href='#' onclick=\"parent.goUrl();document.getElementById('_DialogDiv_Diag1').style.display = 'none'; document.getElementById('_DialogBGDiv').style.display = 'none'; window.parent.rightFrame.location = '/Web/myremail.aspx?module=" + module + "&number=" + number + "'; \">用邮件答复客户</a>, &nbsp;&nbsp; <a href='#' onclick=\"parent.goUrl();document.getElementById('_DialogDiv_Diag1').style.display = 'none'; document.getElementById('_DialogBGDiv').style.display = 'none'; window.parent.rightFrame.location = '/Web/DXTZ.aspx?module=" + module + "&number=" + number + "'; \">通知指定办事处</a>, &nbsp;&nbsp; <a href='#' onclick=\"parent.goUrl();document.getElementById('_DialogDiv_Diag1').style.display = 'none'; document.getElementById('_DialogBGDiv').style.display = 'none'; window.parent.rightFrame.location = '/Web/cjbz.aspx?module=" + module + "&number=" + number + "'; \">处理结果备注</a>";
        }
        if(module=="SJYHXXFKB")
{
diag.Message = "&nbsp;&nbsp;<a href='#' onclick=\"parent.goUrl();document.getElementById('_DialogDiv_Diag1').style.display = 'none'; document.getElementById('_DialogBGDiv').style.display = 'none'; window.parent.rightFrame.location = '/Web/FWPT/DXHF.aspx?module=" + module + "&number=" + number + "'; \">短信回复用户</a>";
}
        //diag.Message = "&nbsp;&nbsp; <a href='#' onclick=\"parent.goUrl();document.getElementById('_DialogDiv_Diag1').style.display = 'none'; document.getElementById('_DialogBGDiv').style.display = 'none'; window.parent.rightFrame.location = '#';\">无关联模块</a>, &nbsp;&nbsp; <a href='#'>无关联模块</a>,&nbsp;&nbsp; <a href='#'>无关联模块</a>, &nbsp;&nbsp; <a href='#'>··无关联模块</a>, &nbsp;&nbsp; <a href='#'>无关联模块</a>, &nbsp;&nbsp; <a href='#'>无关联模块</a>";
        //diag.OKEvent = zAlert; //点击确定后调用的方法
        diag.show();
        try {
        parent.initgogo();
    } catch (err) { }


}
    
    
    
    
    function openview_gaoji(number,i_str)
    {
        var rnd = Math.floor(Math.random() * 100000000);
        var module = document.getElementById('txtmodule').value;
        var WinSettings = "center:yes;resizable:no;dialogHeight:600px;dialogWidth:800px;"
        //window.open("WorkFlow_View_Detail.aspx?module="+module+"&number="+number,'child','width=400,height=300,left=200,top=200');
        var urltemp = "WorkFlow_View_Detail.aspx?module="+module+"&number="+number+"&from=view&rnd="+rnd+"&gaoji="+i_str;
        
        urltemp = urltemp.replace("?","[wh]").replace("&","[he]").replace("&","[he]").replace("&","[he]").replace("&","[he]").replace("=","[dh]");
        //alert(urltemp);
        var MyArgs = window.showModalDialog("showmodaldialog.aspx?url=" + urltemp, MyArgs, WinSettings);


	    
    }
    function openedit(number)
    {
        var module = document.getElementById('txtmodule').value;
        var pagenumber = document.getElementById('txtpagenumber').value;

        parent.document.getElementById("rightFrame").style.display = "none";
        parent.document.getElementById("rightFrame_hide").style.display = "";

       window.top.frames.rightFrame_hide.location.href = "WorkFlow_Update.aspx?module=" + module + "&number=" + number + "&pagenumber=" + pagenumber;
    }
    function opendelete(number)
    {
        Dialog.confirm('警告：记录删除后不可恢复！<br><br>您确实要删除这条记录吗？', function() { opendelete_gogo(number); });

    }

    function opendelete_gogo(number) {
        var module = document.getElementById('txtmodule').value;
        var WinSettings = "center:yes;resizable:no;dialogHeight:0px;dialogWidth:0px;"
        var MyArgs = window.showModalDialog("WorkFlow_Delete.aspx?module=" + module + "&number=" + number, MyArgs, WinSettings);
        document.getElementById('txtflag').value = '2';
        document.form1.submit();
        //window.location.reload();
        //newopen = window.open("WorkFlow_Delete.aspx?module="+module+"&number="+number,'child','width=400,height=300,left=200,top=200');
        //setInterval('went()',1000);
    }
    
    function opencheck(number)
    {
        var rnd = Math.floor(Math.random() * 100000000);
        var module = document.getElementById('txtmodule').value;
        var WinSettings = "center:yes;resizable:no;dialogHeight:600px;dialogWidth:800px;"
	    //var MyArgs = window.showModalDialog("WorkFlow_View_Detail.aspx?module="+module+"&number="+number+"&from=check&rnd="+rnd,MyArgs,WinSettings);
	   // window.location.reload();

        var diag = new Dialog("Diag1");
        dig_public = diag;
	    try {
	        var zbk_yhb = 0;
	        var zbk_yhb1 = 0;
	        zbk_yhb = parent.document.getElementById("theObjTable").style.width;
	        diag.Width = parent.document.body.scrollWidth - 200;
	        zbk_yhb1 = parent.getHeight() - 200;
	        diag.Height = zbk_yhb1;
	    }
	    catch (err) { diag.Width = document.body.scrollWidth - 100; diag.Height = 500; }
	    diag.Title = "审批表单-单号: " + number + "";
	    diag.URL = "WorkFlow_View_Detail.aspx?module=" + module + "&number=" + number + "&from=check&rnd=" + rnd;
	    diag.ShowMessageRow = true;
	    diag.MessageTitle = "关于表单审批的说明";
	    diag.Message = "审批分为几种情况。。。。";
	    //diag.OKEvent = ; //点击确定后调用的方法
	    diag.show();
	    
	    try {
	        parent.initgogo();
	    } catch (err) { }
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

