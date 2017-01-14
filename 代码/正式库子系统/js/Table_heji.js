function setheji() {

var table = document.all("XSGS_YDXQJH_CPXQMX");
for(var i=0;i<table.rows.length;i++)
{
	for(var j=0;j<table.rows[i].cells.length;j++)
	{
		alert(table.rows[i].cells[j].innerHTML);
	}
}

}
