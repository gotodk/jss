
function ShowDialog(url, iWidth, iHeight) {
	var iTop = (window.screen.height - iHeight) / 2;
	var iLeft = (window.screen.width - iWidth) / 2;
	var wnd = window.open(url, "Detail", "Scrollbars=no,Toolbar=no,Location=no,Direction=no,Resizeable=no,Width=" + iWidth + " ,Height=" + iHeight + ",top=" + iTop + ",left=" + iLeft);
	return wnd;
}
function ShowModel(url, iWidth, iHeight) {
	var iTop = (window.screen.height - iHeight) / 2;
	var iLeft = (window.screen.width - iWidth) / 2;
	var rnd = Math.floor(Math.random() * 100000000);
	if (url.indexOf("?") >= 0) {
		url = url + "&rnd=" + rnd;
	} else {
		url = url + "?rnd=" + rnd;
	}
	var wnd = window.showModalDialog(url, window, "dialogHeight: " + iHeight + "px; dialogWidth: " + iWidth + "px;dialogTop: " + iTop + "; dialogLeft: " + iLeft + "; resizable: no; status: 1;scroll:no");
	return wnd;
}

