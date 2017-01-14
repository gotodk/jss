 function selectdate()
        {
            var dateid=document.getElementById("HDSJ");
	        var datetext= new Date(dateid.value.replace(/-/g,"/"));
	        var date=new Date();
	        var getyer=date.getFullYear();
	        var getmonth=date.getMonth()+1;
            var getdate=date.getDate();
	        var strdate=getyer+"/"+getmonth+"/"+getdate;
            var newdate=new Date(strdate);
	        var days=datetext.getTime()-newdate.getTime();
	        var datycount=parseInt(days/(1000 * 60 * 60 * 24));
	        if(datycount>30)
		    {
			    return;
		    }
	         else
		    {
			    document.getElementsByName("HDSJ")[0].value=""; 
		    }
        }   