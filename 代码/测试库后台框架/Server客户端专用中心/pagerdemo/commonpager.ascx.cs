using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMDBHelperClass;


public partial class pagerdemo_commonpager : System.Web.UI.UserControl
{

    //哈希表,存储存储过程参数
    Hashtable news_hashTb = new Hashtable();

    /// <summary>
    /// 用于填充DataGrid控件的数据集
    /// </summary>
    protected DataSet DataSet_Beuse = new DataSet();

    /// <summary>
    /// 设置属性的时候获取数据
    /// </summary>
    public DataSet GetFYds
    {
        get {
            return getDataSet_FY(""); 
        }
    }

    Hashtable ht_where = new Hashtable();

    public Hashtable HTwhere
    {
        set {
            ViewState["GetCustomersDataPage_NAME"] = value["GetCustomersDataPage_NAME"];
            ViewState["this_dblink"] = value["this_dblink"];
            ViewState["page_index"] = value["page_index"];
            ViewState["page_size"] = value["page_size"];
            ViewState["serach_Row_str"] = value["serach_Row_str"];
            ViewState["search_tbname"] = value["search_tbname"];
            ViewState["search_mainid"] = value["search_mainid"];
            ViewState["search_str_where"] = value["search_str_where"];
            ViewState["search_paixu"] = value["search_paixu"];
            ViewState["search_paixuZD"] = value["search_paixuZD"];
            ViewState["count_float"] = value["count_float"];
            ViewState["count_zd"] = value["count_zd"];
            ViewState["cmd_descript"] = value["cmd_descript"];
            ViewState["returnlastpage_open"] = value["returnlastpage_open"];
            ViewState["returnlastpage_spHT"] = value["returnlastpage_spHT"];
            ViewState["search_fyshow"] = value["search_fyshow"];
        }
    }

    /// <summary>
    /// 用于传递给工厂类的哈希表,包含存储过程需要传入的参数
    /// </summary>
    protected Hashtable Hashtable_PutIn = new Hashtable();

    /// <summary>
    /// 用于获取工厂类返回结果的哈希表,包含存储过程需要传出的参数
    /// </summary>
    protected Hashtable Hashtable_PutOut = new Hashtable();

    /// <summary>
    /// 用于获取工厂类返回结果的哈希表,包含工厂类执行完成返回的结果集合
    /// </summary>
    protected Hashtable return_ht = new Hashtable();

    /// <summary>
    /// 用来初始化给存储过程需要的参数
    /// </summary>
    protected Hashtable init_ht = new Hashtable();

    /// <summary>
    /// 设置一个委托，用于当需要的时候，获取数据
    /// </summary>
    /// <param name="NewDS">返回的数据集</param>
    /// <param name="ERRinfo">返回的错误</param>
    public delegate void OnNeedDataHandler(DataSet NewDS,string ERRinfo);


    /// <summary>
    /// 设置委托函数的handler key
    /// </summary>
    private static object _on_need_load_data_handler_key = new object();

    /// <summary>
    /// 编写事件,用于对事件处理函数的赋值,其中“OnNeedLoadData "是事件名称，“OnNeedDataHandler“是处理此事件的委托的类型
    /// </summary>
    public event OnNeedDataHandler OnNeedLoadData        
    {
        add { Events.AddHandler(_on_need_load_data_handler_key, value); } //为web控件添加事件处理函数
        remove { Events.RemoveHandler(_on_need_load_data_handler_key, value); }//移出此web控件的事件处理函数
    }

    /// <summary>
    /// 用于激活事件的函数，此函数原形需要和该事件对应的委托一致。
    /// </summary>
    /// <param name="NewDS">返回的数据集</param>
    /// <param name="ERRinfo">返回的错误</param>
    private void RaiseEvent_OnNeedLoadData(DataSet NewDS,string ERRinfo)
    {
        OnNeedDataHandler handler = (OnNeedDataHandler)Events[_on_need_load_data_handler_key];//在事件处理列表根据该事件的关键字找出此事件处理函数
        if (handler != null) handler(NewDS, ERRinfo);//如果有，则执行此事件处理函数
    }

    /// <summary>
    /// 设置一个委托，用于当需要的时候，获取数据
    /// </summary>
    /// <param name="NewDS">返回的数据集</param>
    /// <param name="ERRinfo">返回的错误</param>
    public delegate void OnNeedDataHandler_all(DataSet NewDS, Hashtable ALLconfig);

    /// <summary>
    /// 设置委托函数的handler key
    /// </summary>
    private static object _on_need_load_data_handler_key_all = new object();

    /// <summary>
    /// 编写事件,用于对事件处理函数的赋值,其中“OnNeedLoadData_all "是事件名称，“OnNeedDataHandler_all“是处理此事件的委托的类型
    /// </summary>
    public event OnNeedDataHandler_all OnNeedLoadData_all
    {
        add { Events.AddHandler(_on_need_load_data_handler_key_all, value); } //为web控件添加事件处理函数
        remove { Events.RemoveHandler(_on_need_load_data_handler_key_all, value); }//移出此web控件的事件处理函数
    }

    /// <summary>
    /// 用于激活事件的函数，此函数原形需要和该事件对应的委托一致。
    /// </summary>
    /// <param name="NewDS">返回的数据集</param>
    /// <param name="ERRinfo">返回的错误</param>
    private void RaiseEvent_OnNeedLoadData(DataSet NewDS, Hashtable ALLconfig)
    {
        OnNeedDataHandler_all handler_all = (OnNeedDataHandler_all)Events[_on_need_load_data_handler_key_all];//在事件处理列表根据该事件的关键字找出此事件处理函数
        if (handler_all != null) handler_all(NewDS, ALLconfig);//如果有，则执行此事件处理函数
    }

    protected void Page_Load(object sender, EventArgs e)
    {
          resetBFY();
    }

    /// <summary>
    /// 重设半分页功能的显示
    /// </summary>
    private void resetBFY()
    {
        //设置默认半分页显示数
        if (ViewState["search_fyshow"] == null || ViewState["search_fyshow"].ToString() == "")
        {
            ViewState["search_fyshow"] = "5";
        }

        Cpageshow.Controls.Clear();

        //动态生成页数可点击控件
        if ((Convert.ToInt32(ViewState["page_index"]) + 1) < Convert.ToInt32(ViewState["search_fyshow"]))
        {
            for (int i = 0; i < Convert.ToInt32(ViewState["search_fyshow"]); i++)
            {
                if (i < Convert.ToInt32(ViewState["page_count"]))
                {
                    if (Convert.ToInt32(ViewState["page_index"]) == i)
                    {
                        //正好在当前页的按钮
                        string pageshownumber = (i + 1).ToString();
                        addButton("BBBBB" + pageshownumber, pageshownumber, pageshownumber, "pagebox_num_nonce");
                    }
                    else
                    {
                        //普通按钮
                        string pageshownumber = (i + 1).ToString();
                        addButton("BBBBB" + pageshownumber, pageshownumber, pageshownumber, "pagebox_num");
                    }
                }
            }
        }

        if ((Convert.ToInt32(ViewState["page_index"]) + 1) >= Convert.ToInt32(ViewState["search_fyshow"]))
        {
            if ((Convert.ToInt32(ViewState["page_index"]) + 1) > (Convert.ToInt32(ViewState["page_count"]) - (Convert.ToInt32(ViewState["search_fyshow"]))))
            {
                for (int i = Convert.ToInt32(ViewState["page_index"]) - Convert.ToInt32(ViewState["search_fyshow"]) + 1; i < Convert.ToInt32(ViewState["page_count"]); i++)
                {
                    if (Convert.ToInt32(ViewState["page_index"]) == i)
                    {
                        //正好在当前页的按钮
                        string pageshownumber = (i + 1).ToString();
                        addButton("BBBBB" + pageshownumber, pageshownumber, pageshownumber, "pagebox_num_nonce");
                    }
                    else
                    {
                        //普通按钮
                        string pageshownumber = (i + 1).ToString();
                        addButton("BBBBB" + pageshownumber, pageshownumber, pageshownumber, "pagebox_num");
                    }
                }
            }
            else
            {
                for (int i = Convert.ToInt32(ViewState["page_index"]) - Convert.ToInt32(ViewState["search_fyshow"]) + 1; i < Convert.ToInt32(ViewState["page_index"]) + Convert.ToInt32(ViewState["search_fyshow"]) + 1; i++)
                {
                    if (Convert.ToInt32(ViewState["page_index"]) == i)
                    {
                        //正好在当前页的按钮
                        string pageshownumber = (i + 1).ToString();
                        addButton("BBBBB" + pageshownumber, pageshownumber, pageshownumber, "pagebox_num_nonce");
                    }
                    else
                    {
                        //普通按钮
                        string pageshownumber = (i + 1).ToString();
                        addButton("BBBBB" + pageshownumber, pageshownumber, pageshownumber, "pagebox_num");
                    }
                }
            }
        }
    }

    //添加按钮
    private void addButton(string ID,string text,string CA,string yscss)
    {
        LinkButton bt = new LinkButton();
        bt.ID = ID;
        bt.Text = text;
        bt.CommandArgument = CA;
        //bt.CssClass = "pagebox_num_nonce";
        bt.Click += new EventHandler(BBB_Click);
        if (yscss == "pagebox_num_nonce")
        {
            bt.Enabled = false;
        }

        HtmlGenericControl span;
        span = new HtmlGenericControl();
        span.ID = "SP" + ID;
        span.Attributes["class"] = yscss;


        Cpageshow.Controls.Add(span);
        span.Controls.Add(bt);

    }

    /// <summary>
    /// 获取数据并激活调用页面的数据处理事件
    /// </summary>
    public void GetFYdataAndRaiseEvent()
    {
        //获取数据
        DataSet dsnew = getDataSet_FY("");
        if (dsnew != null && dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
        {
            zspagetable.Visible = true;
            resetBFY();
        }
        else
        {
            zspagetable.Visible = false;
        }
        //更新文字描述信息
        tbpagerinfo.InnerHtml = "当前<b style='color:#F00'>" + (Convert.ToInt32(ViewState["page_index"]) + 1) + "</b>页,共<b style='color:#F00'>" + ViewState["page_count"].ToString() + "</b>页,共<b style='color:#F00'>" + ViewState["record_count"].ToString() + "</b>条数据,每页<b style='color:#F00'>" + ViewState["page_size"].ToString() + "</b>条";
        //更新转到的显示
        tbgopage.Text = (Convert.ToInt32(ViewState["page_index"]) + 1).ToString();


        //开始加载事件(挂接了才能加载)
        RaiseEvent_OnNeedLoadData(dsnew, ViewState["return_errmsg"].ToString());
        //开始加载事件(挂接了才能加载)
        Hashtable ht_allconfig = new Hashtable();

        ht_allconfig["GetCustomersDataPage_NAME"] = ViewState["GetCustomersDataPage_NAME"];
        ht_allconfig["this_dblink"] = ViewState["this_dblink"];
        ht_allconfig["page_index"] = ViewState["page_index"];
        ht_allconfig["page_size"] = ViewState["page_size"];
        ht_allconfig["serach_Row_str"] = ViewState["serach_Row_str"];
        ht_allconfig["search_tbname"] = ViewState["search_tbname"];
        ht_allconfig["search_mainid"] = ViewState["search_mainid"];
        ht_allconfig["search_str_where"] = ViewState["search_str_where"];
        ht_allconfig["search_paixu"] = ViewState["search_paixu"];
        ht_allconfig["search_paixuZD"] = ViewState["search_paixuZD"];
        ht_allconfig["count_float"] = ViewState["count_float"];
        ht_allconfig["count_zd"] = ViewState["count_zd"];
        ht_allconfig["cmd_descript"] = ViewState["cmd_descript"];
        ht_allconfig["return_errmsg"] = ViewState["return_errmsg"];
        ht_allconfig["page_count"] = ViewState["page_count"];
        ht_allconfig["record_count"] = ViewState["record_count"];
        ht_allconfig["returnlastpage_open"] = ViewState["returnlastpage_open"];
        ht_allconfig["returnlastpage_spHT"] = ViewState["returnlastpage_spHT"];
        
  
        RaiseEvent_OnNeedLoadData(dsnew, ht_allconfig);
    }


    /// <summary>
    /// 获取分页后的数据
    /// </summary>
    /// <param name="dg_once">只允许递归一次的标记,用于自动返回之前页面</param>
    /// <returns></returns>
    public DataSet getDataSet_FY(string dg_once)
    {
        DataSet_Beuse = new DataSet();
        
        //初始化数据工厂  
        if (ViewState["GetCustomersDataPage_NAME"] == null || ViewState["GetCustomersDataPage_NAME"].ToString() == "")
        {
            ViewState["GetCustomersDataPage_NAME"] = "GetCustomersDataPage2";
        }
        if (ViewState["this_dblink"] == null || ViewState["this_dblink"].ToString() == "")
        {
            ViewState["this_dblink"] = "mainsqlserver";
        }
        if (ViewState["page_index"] == null || ViewState["page_index"].ToString() == "")
        {
            ViewState["page_index"] = "0";
        }       
        if (ViewState["page_size"] == null || ViewState["page_size"].ToString() == "")
        {
            ViewState["page_size"] = "10";
        }
        if (ViewState["cmd_descript"] == null || ViewState["cmd_descript"].ToString() == "")
        {
            ViewState["cmd_descript"] = "";
        }
        if (ViewState["record_count"] == null || ViewState["record_count"].ToString() == "")
        {
            ViewState["record_count"] = 0;
        }
        if (ViewState["page_count"] == null || ViewState["page_count"].ToString() == "")
        {
            ViewState["page_count"] = 0;
        }
        if (ViewState["count_float"] == null || ViewState["count_float"].ToString() == "")
        {
            ViewState["count_float"] = "普通";
        }       
        if (ViewState["count_float"] == "特殊")
        {
            if (ViewState["count_zd"] == null || ViewState["count_zd"].ToString() == "")
            {
                ViewState["count_zd"] = "0";
            }
        }

        return_ht.Clear();
        Hashtable_PutIn.Clear();
        Hashtable_PutOut.Clear();

        //给哈希表赋值       
        if (ViewState["page_index"] == null || ViewState["page_index"].ToString() == "")
        {
            ViewState["page_index"] = "0";
        }

        //获取自动返回前一页的各项关键信息
        object ob_returnlastpage_open = ViewState["returnlastpage_open"];
        string str_returnlastpage_open = "";
        if (ob_returnlastpage_open != null)
        {
            str_returnlastpage_open = ob_returnlastpage_open.ToString();
        }

        object ob_UrlReferrer = HttpContext.Current.Request.UrlReferrer;
        string str_UrlReferrer = "";
        if (ob_UrlReferrer != null)
        {
            str_UrlReferrer = ob_UrlReferrer.ToString();
        }

        //如果打开了自动返回，且不是自动返回页进入，记录相关数据
        if (str_returnlastpage_open != "" && str_UrlReferrer.IndexOf(str_returnlastpage_open) < 0)
        {
            Hashtable ht_lastpage = new Hashtable();
            ht_lastpage["page_index"] = ViewState["page_index"];
            ht_lastpage["page_size"] = ViewState["page_size"];
            ht_lastpage["serach_Row_str"] = ViewState["serach_Row_str"];
            ht_lastpage["search_tbname"] = ViewState["search_tbname"];
            ht_lastpage["search_mainid"] = ViewState["search_mainid"];
            ht_lastpage["search_str_where"] = ViewState["search_str_where"];
            ht_lastpage["search_paixu"] = ViewState["search_paixu"];
            ht_lastpage["search_paixuZD"] = ViewState["search_paixuZD"];
            ht_lastpage["count_float"] = ViewState["count_float"];
            ht_lastpage["count_zd"] = ViewState["count_zd"];
            ht_lastpage["returnlastpage_spHT"] = ViewState["returnlastpage_spHT"];
            
            //将当前页信息记录到session
            HttpContext.Current.Session[ViewState["returnlastpage_open"].ToString() + "_lastpage"] = ht_lastpage;
        }

        if (str_returnlastpage_open != "" && str_UrlReferrer.IndexOf(str_returnlastpage_open) > -1 && dg_once != "p" && HttpContext.Current.Session[ViewState["returnlastpage_open"].ToString() + "_lastpage"] != null)
        {
            //开启返回当前页，并设置索引为最后一次索引
            Hashtable ht_lastpage = new Hashtable();
            ht_lastpage = (Hashtable)(HttpContext.Current.Session[ViewState["returnlastpage_open"].ToString() + "_lastpage"]);
            ViewState["page_index"] = ht_lastpage["page_index"];
            ViewState["page_size"] = ht_lastpage["page_size"];
            ViewState["serach_Row_str"] = ht_lastpage["serach_Row_str"];
            ViewState["search_tbname"] = ht_lastpage["search_tbname"];
            ViewState["search_mainid"] = ht_lastpage["search_mainid"];
            ViewState["search_str_where"] = ht_lastpage["search_str_where"];
            ViewState["search_paixu"] = ht_lastpage["search_paixu"];
            ViewState["search_paixuZD"] = ht_lastpage["search_paixuZD"];
            ViewState["count_float"] = ht_lastpage["count_float"];
            ViewState["count_zd"] = ht_lastpage["count_zd"];
            ViewState["returnlastpage_spHT"] = ht_lastpage["returnlastpage_spHT"];
        }
        else
        {
            //不开启自动返回当前页
        }

        Hashtable_PutIn.Add("@PageIndex", Convert.ToInt32(ViewState["page_index"]));//页面索引
        Hashtable_PutIn.Add("@PageSize", Convert.ToInt32(ViewState["page_size"]));//单页数量
        Hashtable_PutIn.Add("@strGetFields", ViewState["serach_Row_str"]);//要查询的列
        Hashtable_PutIn.Add("@tableName", ViewState["search_tbname"]);//表名称
        Hashtable_PutIn.Add("@ID", ViewState["search_mainid"]); //主键
        Hashtable_PutIn.Add("@strWhere", ViewState["search_str_where"]); //查询条件
        Hashtable_PutIn.Add("@sortName", ViewState["search_paixu"]); //排序方式,前后空格
        Hashtable_PutIn.Add("@orderName", ViewState["search_paixuZD"]); //父级查询排序方式,用于排序的字段
        Hashtable_PutIn.Add("@countfloat", ViewState["count_float"]); //普通/特殊，两种方式，一种默认，一种
        Hashtable_PutIn.Add("@countzd", ViewState["count_zd"]); //特殊方式获取数据总量的值


        Hashtable_PutOut.Add("@RecordCount", Convert.ToInt32(ViewState["record_count"])); //返回记录总数
        Hashtable_PutOut.Add("@PageCount", Convert.ToInt32(ViewState["page_count"])); //返回分页后页数
        Hashtable_PutOut.Add("@Descript", ViewState["cmd_descript"].ToString()); //返回错误信息


        //获取数据
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain(ViewState["this_dblink"].ToString());

        return_ht = I_DBL.RunProc_CMD(ViewState["GetCustomersDataPage_NAME"].ToString(), "主要数据", Hashtable_PutIn, ref Hashtable_PutOut);
     
        int page_count = 0;
        if (Hashtable_PutOut["@PageCount"] == null || Hashtable_PutOut["@PageCount"].ToString() == "")
        {
            Hashtable_PutOut["@PageCount"] = "0";
        }
        if (Hashtable_PutOut["@RecordCount"] == null || Hashtable_PutOut["@RecordCount"].ToString() == "")
        {
            Hashtable_PutOut["@RecordCount"] = "0";
        }

        page_count = Convert.ToInt32(Hashtable_PutOut["@PageCount"]);
        ViewState["page_count"] = page_count;
        int record_count = 0;
        record_count = Convert.ToInt32(Hashtable_PutOut["@RecordCount"]);
        ViewState["record_count"] = record_count;
        string cmd_descript = "";
        cmd_descript = Hashtable_PutOut["@Descript"].ToString();
        ViewState["cmd_descript"] = cmd_descript;

        if ((bool)return_ht["return_float"])
        {
            DataSet_Beuse = (DataSet)return_ht["return_ds"];
            ViewState["return_errmsg"] = "";
        }
        else
        {

            ViewState["return_errmsg"] = return_ht["return_errmsg"];
            DataSet_Beuse = null;
        }

        //自动返回当前页后，可能页数或数据已消失，这里需要判断一下
        if (Convert.ToInt32(ViewState["page_index"]) + 1 > Convert.ToInt32(ViewState["page_count"]) && Convert.ToInt32(ViewState["page_count"]) > 0 && dg_once != "p")
        {
            ViewState["page_index"] = (Convert.ToInt32(ViewState["page_count"]) - 1).ToString();
            DataSet_Beuse = getDataSet_FY("p");
            return DataSet_Beuse;
        }

        return DataSet_Beuse;

    }



    //下一页
    protected void Bxyy_Click(object sender, EventArgs e)
    {
        //更新分页索引
        ViewState["page_index"] = (Convert.ToInt32(ViewState["page_index"]) + 1).ToString();
        if (Convert.ToInt32(ViewState["page_index"]) + 1 > Convert.ToInt32(ViewState["page_count"]) - 1)
        {
            ViewState["page_index"] = (Convert.ToInt32(ViewState["page_count"]) - 1).ToString();
        }
        //激活主页面事件
        GetFYdataAndRaiseEvent();
    }
    //尾页
    protected void Bwy_Click(object sender, EventArgs e)
    {
        //更新分页索引
        ViewState["page_index"] = (Convert.ToInt32(ViewState["page_count"]) - 1).ToString();
        //激活主页面事件
        GetFYdataAndRaiseEvent();
    }
    //首页
    protected void Bsy_Click(object sender, EventArgs e)
    {
        //更新分页索引
        ViewState["page_index"] = "0";
        //激活主页面事件
        GetFYdataAndRaiseEvent();
    }
    //上一页
    protected void Bsyy_Click(object sender, EventArgs e)
    {
        //更新分页索引
        ViewState["page_index"] = (Convert.ToInt32(ViewState["page_index"]) - 1).ToString();
        if (Convert.ToInt32(ViewState["page_index"]) - 1 < 0)
        {
            ViewState["page_index"] = "0";
        }
        //激活主页面事件
        GetFYdataAndRaiseEvent();
    }
    //转到指定页
    protected void Bgo_Click(object sender, EventArgs e)
    {
        if (IsNumeric(tbgopage.Text))
        {
            //更新分页索引
            ViewState["page_index"] = Convert.ToInt32(tbgopage.Text) - 1;
            if (Convert.ToInt32(ViewState["page_index"]) - 1 < 0)
            {
                ViewState["page_index"] = "0";
            }
            if (Convert.ToInt32(ViewState["page_index"]) + 1 > Convert.ToInt32(ViewState["page_count"]) - 1)
            {
                ViewState["page_index"] = (Convert.ToInt32(ViewState["page_count"]) - 1).ToString();
            }
            //激活主页面事件
            GetFYdataAndRaiseEvent();
        }
        else {
            tbgopage.Text = (Convert.ToInt32(ViewState["page_index"]) + 1).ToString();
        }
    }

    protected void BBB_Click(object sender, EventArgs e)
    {
        string C_page = ((LinkButton)sender).CommandArgument;
        //更新分页索引
        ViewState["page_index"] = Convert.ToInt32(C_page) - 1;
        if (Convert.ToInt32(ViewState["page_index"]) - 1 < 0)
        {
            ViewState["page_index"] = "0";
        }
        if (Convert.ToInt32(ViewState["page_index"]) + 1 > Convert.ToInt32(ViewState["page_count"]) - 1)
        {
            ViewState["page_index"] = (Convert.ToInt32(ViewState["page_count"]) - 1).ToString();
        }
        //激活主页面事件
        GetFYdataAndRaiseEvent();
    }

    ///<summary>
    ///验证输入的数据是不是正整数
    ///</summary>
    ///<param name="str">传入字符串</param>
    ///<returns>返回true或者false</returns>
    static bool IsNumeric(string str)
    {
        System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
        return reg1.IsMatch(str);
    }
}