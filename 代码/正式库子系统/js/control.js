//(标准版)

//隐藏或显示修订痕迹
function jsShowTrack(value) 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").ShowRevisions = value;
} 

//接受所有修订，清除痕迹
function jsAcceptAllRevisions() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").AcceptAllRevisions();
}
//获取并显示所有痕迹
function jsGetAllRevisions()
{
  var i;
  var str="";
  for (i = 1;i <=document.all("SOAOfficeCtrl").Document.Revisions.Count;i++)
  {
    str=str + document.all("SOAOfficeCtrl").Document.Revisions.Item(i).Author;
    if (document.all("SOAOfficeCtrl").Document.Revisions.Item(i).Type=="1")
	{ 
    	str=str + ' 插入：'+document.all("SOAOfficeCtrl").Document.Revisions.Item(i).Range.Text+"\r\n";
    }
	else
	{
    	str=str + ' 删除：'+document.all("SOAOfficeCtrl").Document.Revisions.Item(i).Range.Text+"\r\n";
    }
  }
  alert("当前文档的所有修改痕迹如下：\r\n"+str);
}
//插入本地印章
function jsInsertLocalSeal() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").InsertSealFromLocal();
}
//插入手写签名
function jsInsertSignature() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
	{
		//document.all("SOAOfficeCtrl").InsertSealFromURL( "images/sign02.esf");//不通过选择，直接插入指定签名
		var mDialogUrl = "../../images/selectSignature.htm";
		var mObject = new Object();
		mObject.SelectValue = "";
		window.showModalDialog(mDialogUrl, mObject, "dialogHeight:180px; dialogWidth:340px;center:yes;scroll:no;status:no;"); 
		//判断用户是否选择签名
		if (mObject.SelectValue!="")
		{
			document.all("SOAOfficeCtrl").InsertSealFromURL("../../images/" + mObject.SelectValue);
		}
	}
}
//全文手写批注
function jsStartHandDraw() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").StartHandDraw();
} 
//分层显示全文手写批注
function jsShowHandDrawDispBar() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").ShowHandDrawDispBar();
} 
//给文档添加数字签名
function jsAddDigitalSignature() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		//document.all("SOAOfficeCtrl").AddDigitalSignature();
		alert("标准版无数字签名功能。");
} 

//验证数字签名
function jsValidateDigitalSignatures()
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else		
		//document.all("SOAOfficeCtrl").VerifyDigitalSignatures();
		alert("标准版无数字签名功能。");
		
} 

//插入电子印章
function jsInsertSeal() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
	{
		//document.all("SOAOfficeCtrl").InsertSealFromURL("images/seal02.esf");//不通过选择，直接插入指定印章
		var mDialogUrl = "../../images/selectSeal.htm";
		var mObject = new Object();
		mObject.SelectValue = "";
		window.showModalDialog(mDialogUrl, mObject, "dialogHeight:180px; dialogWidth:340px;center:yes;scroll:no;status:no;"); 
		//判断用户是否选择印章
		if (mObject.SelectValue!="")
		{
			document.all("SOAOfficeCtrl").InsertSealFromURL( "../images/" + mObject.SelectValue);
		}
	}
} 
//验证电子印章/签名的有效性
function jsValidateSeal() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").ValidateSeal();
} 
//允许或禁止 复制/拷贝
function jsCanCopy(value) 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").CanCopy = value;	
} 
//插入Web图片
function jsInsertWebImage() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").InsertWebImage( "../../images/SOA_05.gif");
}
//页面设置
function jsDocPageSetup() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").ShowDialog(5);
}
//切换标题栏
function jsToggleTitlebar() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").Titlebar = !document.all("SOAOfficeCtrl").Titlebar;
}
//切换菜单栏
function jsToggleMenubar() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").Menubar = !document.all("SOAOfficeCtrl").Menubar;
}
//切换工具栏
function jsToggleToolbars() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").Toolbars = !document.all("SOAOfficeCtrl").Toolbars;
}
//禁止/允许 打印文档菜单及按钮
function jsEnablePrint(value) 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
	{
		document.all("SOAOfficeCtrl").EnableFileCommand(5) = value;
		// 刷新一下工具条
		if(document.all("SOAOfficeCtrl").Toolbars)
		{
			document.all("SOAOfficeCtrl").Toolbars = false;
			document.all("SOAOfficeCtrl").Toolbars = true;
		}
	}
		
}
//禁止/允许 保存文档菜单及按钮
function jsEnableSave(value) 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
	{
		document.all("SOAOfficeCtrl").EnableFileCommand(3) = value;
		// 刷新一下工具条
		if(document.all("SOAOfficeCtrl").Toolbars)
		{
			document.all("SOAOfficeCtrl").Toolbars = false;
			document.all("SOAOfficeCtrl").Toolbars = true;
		}
	}	

}
//禁止/允许 另存文档菜单及按钮
function jsEnableSaveAs(value) 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").EnableFileCommand(4) = value;
}
//保存文档到web服务器
function jsSave() 
{ 
	try {
		if (!bDocOpen)
			alert("当前没有已打开的文档。");
		else
			document.all("SOAOfficeCtrl").WebSave();//保存当前文档到web服务器，保存（覆盖）到原打开文档的地址处
		//document.all("SOAOfficeCtrl").WebSave("aa.doc");表示把当前文档另存到web服务器为aa.doc
	} 
	catch (e) 
	{ 
		alert("文档保存失败!\n错误信息:" + e.message); 
	} 
}
//保存文档到web服务器，使用页面提交技术，在提交文档的同时提交其他用户定义的页面字段或域
function jsSubmitSave() 
{ 
	try {
		if (!bDocOpen)
			alert("当前没有已打开的文档。");
		else
		{
			document.all("SOAOfficeCtrl").WebSave();
			formData.submit();
		}
	} 
	catch (e) 
	{ 
		alert("文档保存失败!\n错误信息:" + e.message); 
	} 
}
//打开插入本地图片的对话框
function jsOpenImageDialog() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").OpenImageDialog();
} 
//获取文档Txt正文
function jsWordToText()
{
    alert(document.all("SOAOfficeCtrl").DocText);
}
//VBA套红，套用VBA编程模板
function jsInsertVBATemplate()
{
	var DocObject=document.all("SOAOfficeCtrl").Document;
	var myl=DocObject.Shapes.AddLine(91,60,285,60)
	myl.Line.ForeColor=255;
	myl.Line.Weight=2;
	var myl1=DocObject.Shapes.AddLine(308,60,502,60)
	myl1.Line.ForeColor=255;
	myl1.Line.Weight=2;

   	var myRange=DocObject.Range(0,0);
	myRange.Select();

	var mtext="★";
	DocObject.Application.Selection.Range.InsertAfter (mtext+"\n");
   	var myRange=DocObject.Paragraphs(1).Range;
   	myRange.ParagraphFormat.LineSpacingRule =1.5;
   	myRange.font.ColorIndex=6;
   	myRange.ParagraphFormat.Alignment=1;
   	myRange=DocObject.Range(0,0);
	myRange.Select();
	mtext="市政发[2005]0168号";
	DocObject.Application.Selection.Range.InsertAfter (mtext+"\n");
	myRange=DocObject.Paragraphs(1).Range;
	myRange.ParagraphFormat.LineSpacingRule =1.5;
	myRange.ParagraphFormat.Alignment=1;
	myRange.font.ColorIndex=1;
	
	mtext="某市政府红头文件";
	DocObject.Application.Selection.Range.InsertAfter (mtext+"\n");
	myRange=DocObject.Paragraphs(1).Range;
	myRange.ParagraphFormat.LineSpacingRule =1.5;
	
	myRange.Font.ColorIndex=6;
	myRange.Font.Name="仿宋_GB2312";
	myRange.font.Bold=true;
	myRange.Font.Size=30;
	myRange.ParagraphFormat.Alignment=1;

}
//使用指定的模板套红
function jsApplyFileTemplate() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
	{
		var mDialogUrl = "../../images/selectTemplate.htm";
		var mObject = new Object();
		mObject.SelectValue = "";
		window.showModalDialog(mDialogUrl, mObject, "dialogHeight:180px; dialogWidth:340px;center:yes;scroll:no;status:no;"); 
		//判断用户是否选择模板
		if (mObject.SelectValue!="")
		{
			document.all("SOAOfficeCtrl").ApplyTemplateFromURL("myRedheaddoc/" + mObject.SelectValue);
		}
	}
} 
//禁止/允许WORD鼠标拖曳功能
function jsDisableDragAndDrop() 
{ 
	if (!bDocOpen)
		alert("当前没有已打开的文档。");
	else
		document.all("SOAOfficeCtrl").Document.Application.Options.AllowDragAndDrop = !document.all("SOAOfficeCtrl").Document.Application.Options.AllowDragAndDrop;
} 
//显示保存失败信息
function jsDisplayError() 
{
	var  strErro;
	strErro = document.all("SOAOfficeCtrl").LastErro;
	if  (strErro != ""){
		document.write(strErro);
	}		
	else
	{
		alert("没有发现错误。");
	}
}
// 切换到PDF模式
function jsPDF() 
{ 
	if (!bDocOpen){
		alert("当前没有已打开的文档。");
	}
	else{
		document.all("SOAOfficeCtrl").Document.ActiveWindow.Thumbnails = true;
	}		

} 
// 切换到Page模式
function jsPage() 
{ 
	if (!bDocOpen){
		alert("当前没有已打开的文档。");
	}
	else{
		document.all("SOAOfficeCtrl").Document.ActiveWindow.Thumbnails = false;
	}		

} 

// 颜色
function jsOleColor(red, green, blue)
{
    var decColor = red + 256 * green + 65536 * blue;
    return decColor;
}

//将html form的域值拷贝到Word文档的标签中
function jsCopyTextToBookMark(inputname,BookMarkName)
{
	try
	{	
		var inputValue="";
		var j,elObj,optionItem;
		var elObj = document.forms[0].elements(inputname);		 
		if (!elObj)
		{
			alert("HTML的FORM中没有此输入域："+ inputname);
			return;
		}
		switch(elObj.type)
		{
				case "select-one":
					inputValue = elObj.options[elObj.selectedIndex].text;
					break;
				case "select-multiple":
					var isFirst = true;
					for(j=0;j<elObj.options.length;j++)
					{
						optionItem = elObj.options[j];					
						if (optionItem.selected)
						{
							if(isFirst)
							{
								inputValue = optionItem.text;
								isFirst = false;
							}
							else
							{
								inputValue += "  " + optionItem.text;
							}
						}
					}
					
					break;
				default: // text,Areatext,selecte-one,password,submit,etc.
					inputValue = elObj.value;
					break;
		}
		var bkmkObj = document.all("SOAOfficeCtrl").Document.BookMarks(BookMarkName);	
		if(!bkmkObj)
		{
			alert("Word 模板中不存在名称为：\""+BookMarkName+"\"的书签！");
		}
		var saverange = bkmkObj.Range
		saverange.Text = inputValue;
		document.all("SOAOfficeCtrl").Document.Bookmarks.Add(BookMarkName,saverange);
	}
	catch(err){
		
	}
	finally{
	}		
}