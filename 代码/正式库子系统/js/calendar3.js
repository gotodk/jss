// if two digit year input dates after this year considered 20 century.
var NUM_CENTYEAR = 30;
// is time input control required by default
var BUL_TIMECOMPONENT = false;
// are year scrolling buttons required by default
var BUL_YEARSCROLL = true;

var calendars = [];
var RE_NUM = /^\-?\d+$/;

function calendar3(obj_target) {

	// assigning methods
	this.gen_date = cal_gen_date3;
	this.gen_time = cal_gen_time3;
	this.gen_tsmp = cal_gen_tsmp3;
	this.prs_date = cal_prs_date3;
	this.prs_time = cal_prs_time3;
	this.prs_tsmp = cal_prs_tsmp3;
	this.popup    = cal_popup3;

	// validate input parameters
	if (!obj_target)
		return cal_error("调用日期控件出错:找不到指定的控件！");
	if (obj_target.value == null)
		return cal_error("调用日期控件出错: 参数不正确！");
	this.target = obj_target;
	this.time_comp = BUL_TIMECOMPONENT;
	this.year_scroll = BUL_YEARSCROLL;
	
	// register in global collections
	this.id = calendars.length;
	calendars[this.id] = this;
}

function cal_popup3 (str_datetime) {
	if (str_datetime)
		this.dt_current = this.prs_tsmp(str_datetime);
	else 
		this.dt_selected = this.dt_current = this.prs_tsmp(this.target.value);

	if (!this.dt_current) return;
	var iTop = (window.screen.height - 200) / 2;
	var iLeft = (window.screen.width - (this.time_comp ? 225 : 200)) / 2;
	var obj_calwindow = window.open(
		'calendar.html?id=' + this.id + '&s=' + this.dt_selected.valueOf() + '&c=' + this.dt_current.valueOf(),
		'Calendar', 'width=200,height=' + (this.time_comp ? 225 : 200) +
		',status=no,resizable=no,top=' + iTop+',left=' + iLeft + ',dependent=yes,alwaysRaised=yes'
	);
	obj_calwindow.opener = window;
	obj_calwindow.focus();
}

// timestamp generating function
function cal_gen_tsmp3 (dt_datetime) {
	return(this.gen_date(dt_datetime) + ' ' + this.gen_time(dt_datetime));
}

// date generating function
function cal_gen_date3 (dt_datetime) {
	return (
		dt_datetime.getFullYear() + "-"
		+ (dt_datetime.getMonth() < 9 ? '0' : '') + (dt_datetime.getMonth() + 1) + "-"
		+ (dt_datetime.getDate() < 10 ? '0' : '') + dt_datetime.getDate()
	);
}
// time generating function
function cal_gen_time3 (dt_datetime) {
	return (
		(dt_datetime.getHours() < 10 ? '0' : '') + dt_datetime.getHours() + ":"
		+ (dt_datetime.getMinutes() < 10 ? '0' : '') + (dt_datetime.getMinutes()) + ":"
		+ (dt_datetime.getSeconds() < 10 ? '0' : '') + (dt_datetime.getSeconds())
	);
}

// timestamp parsing function
function cal_prs_tsmp3 (str_datetime) {
	// if no parameter specified return current timestamp
	if (!str_datetime)
		return (new Date());

	// if positive integer treat as milliseconds from epoch
	if (RE_NUM.exec(str_datetime))
		return new Date(str_datetime);
		
	// else treat as date in string format
	var arr_datetime = str_datetime.split(' ');
	return this.prs_time(arr_datetime[1], this.prs_date(arr_datetime[0]));
}

// date parsing function
function cal_prs_date3 (str_date) {

	var arr_date = str_date.split('-');

	if (arr_date.length != 3) return alert ("日期格式错误: '" + str_date + "'.\n日期格式为：年-月-日.");

	if (!arr_date[0]) return alert ("日期格式错误: '" + str_date + "'.\n找不到年份.");
	if (!RE_NUM.exec(arr_date[0])) return alert ("年份值错误: '" + arr_date[0] + "'.\n只能为正整数.");

	if (!arr_date[1]) return alert ("日期格式错误: '" + str_date + "'.\n找不到月份.");
	if (!RE_NUM.exec(arr_date[1])) return alert ("月份值错误: '" + arr_date[1] + "'.\n只能为正整数.");

	if (!arr_date[2]) return alert ("日期格式错误: '" + str_date + "'.\n找不到日期.");
	if (!RE_NUM.exec(arr_date[2])) return alert ("日格式错误: '" + arr_date[2] + "'.\n只能为正整数.");



	var dt_date = new Date();
	dt_date.setDate(1);

	if (arr_date[0] < 100) arr_date[0] = Number(arr_date[0]) + (arr_date[0] < NUM_CENTYEAR ? 2000 : 1900);
	dt_date.setFullYear(arr_date[0]);


	if (arr_date[1] < 1 || arr_date[1] > 12) return alert ("月份格式错误: '" + arr_date[1] + "'.\n只能为01-12.");
	dt_date.setMonth(arr_date[1] - 1);
	 

	var dt_numdays = new Date(arr_date[0], arr_date[1], 0);
	dt_date.setDate(arr_date[2]);
	if (dt_date.getMonth() != (arr_date[1]-1)) return alert ("日期格式错误: '" + arr_date[2] + "'.\n只能为 01-"+dt_numdays.getDate()+".");

	return (dt_date)
}

// time parsing function
function cal_prs_time3 (str_time, dt_date) {

	if (!dt_date) return null;
	var arr_time = String(str_time ? str_time : '').split(':');

	if (!arr_time[0]) dt_date.setHours(0);
	else if (RE_NUM.exec(arr_time[0])) 
		if (arr_time[0] < 24) dt_date.setHours(arr_time[0]);
		else return cal_error ("错误的小时数: '" + arr_time[0] + "'.\n只能为 00-23.");
	else return cal_error ("错误的小时数: '" + arr_time[0] + "'.\n只能为正整数.");
	
	if (!arr_time[1]) dt_date.setMinutes(0);
	else if (RE_NUM.exec(arr_time[1]))
		if (arr_time[1] < 60) dt_date.setMinutes(arr_time[1]);
		else return cal_error ("错误的分钟数: '" + arr_time[1] + "'.\n只能为00-59.");
	else return cal_error ("错误的分钟数: '" + arr_time[1] + "'.\n只能为正整数.");

	if (!arr_time[2]) dt_date.setSeconds(0);
	else if (RE_NUM.exec(arr_time[2]))
		if (arr_time[2] < 60) dt_date.setSeconds(arr_time[2]);
		else return cal_error ("错误的秒数: '" + arr_time[2] + "'.\n只能为00-59.");
	else return cal_error ("错误的秒数: '" + arr_time[2] + "'.\n只能为正整数.");

	dt_date.setMilliseconds(0);
	return dt_date;
}

function cal_error (str_message) {
	alert (str_message);
	return null;
}
