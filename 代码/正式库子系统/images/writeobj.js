////////////////////////////////////////////////////////////////////////////////////////////////
//
//	codeBase的意义：
//
//	codeBase="http://127.0.0.1/soaaspdemo/SOAOffice.ocx#version=5,0,1,0" 
//
//	前面是控件的URL路径：
//		
//	http://127.0.0.1/soaaspdemo/SOAOffice.ocx  ，浏览器会根据此路径自动下载安装控件； 
//
//	后面是控件的版本号：
//
//	#version=5,0,1,0 ，这里的版本号如果比客户端电脑已安装的控件版本新，那么客户端会自动下载更新控件。
//
////////////////////////////////////////////////////////////////////////////////////////////////

//  以下是输出控件的代码，这里的codebase采用了相对于当前页面的相对路径。

document.write('<OBJECT id="SOAOfficeCtrl" codeBase="SOAOffice.ocx#version=5,0,1,0" height="100%" width="100%" classid="clsid:FABFB7B0-B15E-413C-94BC-96D21EC78712" data="">');
document.write('</OBJECT>');
											
											
											
											
											
											
											
											
												
											