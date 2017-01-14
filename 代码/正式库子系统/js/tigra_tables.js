function tigra_tables (

		str_tableid, // table id (req.)
		num_header_offset, // how many rows to skip before applying effects at the begining (opt.)
		num_footer_offset, // how many rows to skip at the bottom of the table (opt.)
		str_odd_color, // background color for odd rows (opt.)
		str_even_color, // background color for even rows (opt.)
		str_mover_color, // background color for rows with mouse over (opt.)
		str_onclick_color )
	{
	

	// validate required parameters
	if (!str_tableid) {return alert("No table(s) ID specified in parameters");}
	var obj_tables = (document.all ? document.all[str_tableid] : document.getElementById(str_tableid));
	//if (!obj_tables) {return alert("Can't find table(s) with specified ID (" + str_tableid + ")");}
if (!obj_tables) {return}

	// set defaults for optional parameters
	var col_config = [];
	col_config.header_offset = (num_header_offset ? num_header_offset : 0);
	col_config.footer_offset = (num_footer_offset ? num_footer_offset : 0);
	col_config.odd_color = (str_odd_color ? str_odd_color : '#ffffff');
	col_config.even_color = (str_even_color ? str_even_color : '#dbeaf5');
	col_config.mover_color = (str_mover_color ? str_mover_color : '#6699cc');
	col_config.onclick_color = (str_onclick_color ? str_onclick_color : '#4C7DAB');
	
	// init multiple tables with same ID
	if (obj_tables.length)
	{
		for (var i = 0; i < obj_tables.length; i=i+1)
		{
			tt_init_table(obj_tables[i], col_config);
		}
	}
	// init single table
	else
	{
		tt_init_table(obj_tables, col_config);
	}
}

function tt_init_table (obj_table, col_config) {
	var col_lconfig = [],
		col_trs = obj_table.rows;
	if (!col_trs) 
	{
	  return;
	 }
	for (var i = col_config.header_offset; i < col_trs.length - col_config.footer_offset; i=i+1) {
		col_trs[i].config = col_config;
		col_trs[i].lconfig = col_lconfig;
		col_trs[i].set_color = tt_set_color;
		col_trs[i].onmouseover = tt_mover; 
		col_trs[i].onmouseout = tt_mout;
		col_trs[i].onmousedown = tt_onclick;
		col_trs[i].order = (i - col_config.header_offset) % 2;
		col_trs[i].onmouseout();
	}
}
function tt_set_color(str_color) {
	this.style.backgroundColor = str_color;
}

// event handlers
function tt_mover () {
	if (this.lconfig.clicked != this)
	{
		this.set_color(this.config.mover_color);
	}
}
function tt_mout () {
	if (this.lconfig.clicked != this)
	{
		this.set_color(this.order ? this.config.odd_color : this.config.even_color);
	}
}
function tt_onclick () {
	if (this.lconfig.clicked == this) {
		this.lconfig.clicked = null;
		this.onmouseover();
	}
	else 
	{
		var last_clicked = this.lconfig.clicked;
		this.lconfig.clicked = this;
		if (last_clicked) 
		{
		  last_clicked.onmouseout();
		}
		this.set_color(this.config.onclick_color);
	}
}



