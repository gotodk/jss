	   function onColor(td)
		{
		td.style.backgroundColor='#DCDCDC';
		}
		function offColor(td)
		{
		td.style.backgroundColor='';
		}
		function SetLinkUrl(svrpage, fileid)
		{
			//location.href = svrpage+'?ID='+fileid;
			window.open(svrpage+'?ID='+fileid);
		}
		function SetLinkUrl2(svrpage, fileid)
		{
			location.href = svrpage+'?ID='+fileid;
			//window.open(svrpage+'?ID='+fileid);
		}
		function openHtml(filename)
		{
			window.open("doc/"+filename);
		}
		function openDataList(svrpage, fileid)
		{
			window.open(svrpage + '?ID='+fileid,"","fullscreen=0,toolbar=0,location=1,directories=0,status=0,menubar=0,scrollbars=1,resizable=0,width=" + 500 + ",height=" + 280 + ",top=200,left=100",true);	
		}
		function openWordReadOnly(fileid)
		{
			var aw = window.screen.availWidth - 10; 
			var ah = window.screen.availHeight - 130; 
			window.open('SubmitDataOfDoc.aspx?ID='+fileid,"","fullscreen=0,toolbar=1,location=1,directories=1,status=0,menubar=1,scrollbars=1,resizable=1,width=" + aw + ",height=" + ah + ",top=0,left=0",true);	
		}

		