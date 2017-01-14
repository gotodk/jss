using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Galaxy.ClassLib.DataBaseFactory;

namespace 大量数据测试
{
    public partial class yhb_BigPage : System.Web.UI.UserControl
    {
        //连接工厂接口
        private I_DBFactory I_DBF;
        //数据库连接接口
        private I_Dblink I_DBL;

        //哈希表,存储存储过程参数
        Hashtable news_hashTb = new Hashtable();

        private int page_index = 0;//分页索引，从0开始

        /// <summary>
        /// 分页索引，从0开始
        /// </summary>
        public int Page_index
        {
            get { return page_index; }
            set { page_index = value; }
        }

        private int page_count = 0;//分页后总页数

        /// <summary>
        /// 分页后总页数
        /// </summary>
        public int Page_count
        {
            get { return page_count; }
            set { page_count = value; }
        }
        private int record_count = 0;//返回的记录总数量

        /// <summary>
        /// 返回的记录总数量
        /// </summary>
        public int Record_count
        {
            get { return record_count; }
            set { record_count = value; }
        }
        private int page_size = 15; //单页数量

        /// <summary>
        /// 单页数量
        /// </summary>
        public int Page_size
        {
            get { return page_size; }
            set { page_size = value; }
        }
        private string serach_Row_str = " * "; //要查询的列表达式

        /// <summary>
        /// 要查询的列表达式
        /// </summary>
        public string Serach_Row_str
        {
            get { return serach_Row_str; }
            set { serach_Row_str = value; }
        }
        private string search_str_where = " "; //搜索条件

        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search_str_where
        {
            get { return search_str_where; }
            set { search_str_where = value; }
        }
        private string search_tbname = " FMEventLog "; //要查询的表名称

        /// <summary>
        /// 要查询的表名称
        /// </summary>
        public string Search_tbname
        {
            get { return search_tbname; }
            set { search_tbname = value; }
        }
        private string search_mainid = " id "; //主键

        /// <summary>
        /// 主键
        /// </summary>
        public string Search_mainid
        {
            get { return search_mainid; }
            set { search_mainid = value; }
        }
        private string search_paixu = " DESC ";  //排序方式表达式

        /// <summary>
        /// 排序方式表达式
        /// </summary>
        public string Search_paixu
        {
            get { return search_paixu; }
            set { search_paixu = value; }
        }
        private string search_paixuZD = " id "; //用来排序的字段

        /// <summary>
        /// 用来排序的字段
        /// </summary>
        public string Search_paixuZD
        {
            get { return search_paixuZD; }
            set { search_paixuZD = value; }
        }
        private int search_fyshow = 5; //显示分页数的一半

        /// <summary>
        /// 显示分页数的一半
        /// </summary>
        public int Search_fyshow
        {
            get { return search_fyshow; }
            set { search_fyshow = value; }
        }
        
        /// <summary>
        /// GridView控件
        /// </summary>
        public GridView GGridView_show
        {
            get { return GV_show; }
            set { GV_show = value; }
        }

        private string listys = "YahooGridView"; //列表样式

        /// <summary>
        /// 列表样式
        /// </summary>
        public string Listys
        {
            get { return listys; }
            set { listys = value; }
        }


        /// <summary>
        /// 用于填充DataGrid控件的数据集
        /// </summary>
        protected DataSet DataSet_Beuse = new DataSet();

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

        protected void Page_Load(object sender, EventArgs e)
        {
            bigys.InnerHtml = "<link href=\"yhb_BigPage_css/" + listys + ".css\" type=\"text/css\" rel=\"stylesheet\">";
            AppSettingsReader hh = new AppSettingsReader();//调用web.Config中的数据库配置

            //初始化数据工厂
            I_DBF = new DBFactory();
            I_DBL = I_DBF.DbLinkSqlMain(ConfigurationManager.ConnectionStrings["FMOPConn"].ToString());

            //分析传送的参数(不能为空，同时不能为负数，同时必须为数字)
            if (Request["page_index"] != null && IsNumeric(Request["page_index"].ToString()) && Convert.ToInt32(Request["page_index"].ToString()) >= 0)
            {
                page_index = Convert.ToInt32(Request["page_index"].ToString());
            }
            else
            {
                page_index = 0;
            }


            //绑定数据
            My_Binding_Pager();
        }




        /// <summary>
        /// 得到从工厂返回的哈希表
        /// </summary>
        private void getHT_F()
        {
            DataSet_Beuse.Clear();
            return_ht.Clear();
            Hashtable_PutIn.Clear();
            Hashtable_PutOut.Clear();
            //给哈希表赋值
            Hashtable_PutIn.Add("@PageIndex", page_index);//页面索引
            Hashtable_PutIn.Add("@PageSize", page_size);//单页数量
            Hashtable_PutIn.Add("@strGetFields", serach_Row_str);//要查询的列
            Hashtable_PutIn.Add("@tableName", search_tbname);//表名称
            Hashtable_PutIn.Add("@ID", search_mainid); //主键
            Hashtable_PutIn.Add("@strWhere", search_str_where); //查询条件
            Hashtable_PutIn.Add("@sortName", search_paixu); //排序方式,前后空格
            Hashtable_PutIn.Add("@orderName", search_paixuZD); //父级查询排序方式,用于排序的字段

            Hashtable_PutOut.Add("@RecordCount", record_count); //返回记录总数
            Hashtable_PutOut.Add("@PageCount", page_count); //返回分页后页数
            //获取数据
            return_ht = I_DBL.RunProc_CMD("GetCustomersDataPage", DataSet_Beuse, Hashtable_PutIn, ref Hashtable_PutOut);

            page_count = Convert.ToInt32(Hashtable_PutOut["@PageCount"]);
            record_count = Convert.ToInt32(Hashtable_PutOut["@RecordCount"]);
        }


        /// <summary>
        /// 绑定需要分页的数据
        /// </summary>
        private void My_Binding_Pager()
        {

            getHT_F();
            if ((bool)return_ht["return_float"])
            {
                DataSet_Beuse = (DataSet)return_ht["return_ds"];
                GV_show.DataSource = DataSet_Beuse.Tables[0].DefaultView;
                GV_show.DataBind();
                //GroupRows(GV_show, new int[] {1,2,3,4,5,6,7,8,9 });
                //L_pagemsg.Text = "共有:" + record_count.ToString() + "条数据,当前第" + page_index_show.ToString() + "/" + page_count.ToString() + ",每页" + page_size.ToString() + "条";

                string fy_html_str = "";

                if (page_index <= 0)
                {
                    page_index = 0;
                    fy_html_str = fy_html_str + "<span class=\"pagebox_pre_nolink\">第一页</span>";
                    fy_html_str = fy_html_str + "<span class=\"pagebox_pre_nolink\">上一页</span>";
                }
                else
                {
                    fy_html_str = fy_html_str + "<span class=\"pagebox_pre\"><a title=\"第一页\" href=\"?page_index=0\">第一页</a></span>";
                    fy_html_str = fy_html_str + "<span class=\"pagebox_pre\"><a title=\"上一页\" href=\"?page_index=" + (page_index-1) + "\">上一页</a></span>";
                }
                if (page_index >= page_count - 1)
                {
                    page_index = page_count - 1;
                }
                if ((page_index + 1) < search_fyshow)
                {
                    for (int i = 0; i < search_fyshow; i++)
                    {
                        if (page_index == i)
                        {
                            fy_html_str = fy_html_str + "<span class=\"pagebox_num_nonce\">" + (i + 1) + "</span>";
                        }
                        else
                        {
                            fy_html_str = fy_html_str + "<span class=\"pagebox_num\"><a href=\"?page_index=" + i + "\">" + (i + 1) + "</a></span>";
                        }

                    }
                }
                if ((page_index + 1) >= search_fyshow)
                {
                    if ((page_index + 1) > (page_count - search_fyshow))
                    {
                            for (int i = page_index - search_fyshow + 1; i < page_count; i++)
                            {
                                if (page_index == i)
                                {
                                    fy_html_str = fy_html_str + "<span class=\"pagebox_num_nonce\">" + (i + 1) + "</span>";
                                }
                                else
                                {
                                    fy_html_str = fy_html_str + "<span class=\"pagebox_num\"><a href=\"?page_index=" + i + "\">" + (i + 1) + "</a></span>";
                                }
                            }
                    }
                    else
                    {
                        for (int i = page_index - search_fyshow + 1; i < page_index + search_fyshow + 1; i++)
                        {
                            if (page_index == i)
                            {
                                fy_html_str = fy_html_str + "<span class=\"pagebox_num_nonce\">" + (i + 1) + "</span>";
                            }
                            else
                            {
                                fy_html_str = fy_html_str + "<span class=\"pagebox_num\"><a href=\"?page_index=" + i + "\">" + (i + 1) + "</a></span>";
                            }
                        }
                    }
                    
                }
                





                if (page_index >= page_count -1)
                {
                    page_index = page_count - 1;
                    fy_html_str = fy_html_str + "<span class=\"pagebox_next_nolink\">下一页</span>";
                    fy_html_str = fy_html_str + "<span class=\"pagebox_pre_nolink\">最后一页</span>";

                }
                else
                {
                    fy_html_str = fy_html_str + "<span class=\"pagebox_next\"><a title=\"下一页\" href=\"?page_index=" + (page_index + 1) + "\">下一页</a></span>";
                    fy_html_str = fy_html_str + "<span class=\"pagebox_next\"><a title=\"最后一页\" href=\"?page_index=" + (page_count - 1) + "\">最后一页</a></span>";
                }


                fy_html_str = fy_html_str + "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"text-align:center;vertical-align:middle;font-size:12px;color:#999999;\"  >";
                fy_html_str = fy_html_str + "<tr><td title=\"点击跳转到\" style=\"cursor:hand;\" onclick=\"location.href='?page_index='+(parseInt(document.getElementById('yhb_gotopage').value) -1);\">点击跳转到:</td><td>";
                fy_html_str = fy_html_str + "<input type=\"text\" name=\"yhb_gotopage\" id=\"yhb_gotopage\" style=\"width:40px; height:20px;border:1px #ccc solid;\" value=\"" + (page_index + 1) + "\" onkeypress=\"var keynum;var keychar;var numcheck;if(window.event) {keynum = event.keyCode;}else if(event.which) {keynum = event.which;}if(keynum == '13'){location.href='?page_index='+(parseInt(document.getElementById('yhb_gotopage').value) -1);return false;}if ( !(((window.event.keyCode >= 48) && (window.event.keyCode <= 57)) || (window.event.keyCode == 13))){return false;}\" /></td>";
                fy_html_str = fy_html_str + "<td>页,共" + page_count + "页,共" + record_count + "条数据,每页" + page_size + "条,</td></tr></table>";
                fybox.InnerHtml = fy_html_str;
            }
            else
            {
                
                //Label1.Text = return_ht["return_errmsg"].ToString();
            }
        }

        static bool IsNumeric(string str)
        {
            if (str == null || str.Length == 0)
                return false;
            foreach (char c in str)
            {
                if (!Char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }



        /// <summary>
        /// GridView合并行
        /// </summary>
        /// <param name="p_GridView">Grid控件</param>
        /// <param name="p_ColumnsIndex">列索引</param>
        public static void GroupRows(GridView p_GridView, int[] p_ColumnsIndex)
        {
            int _Count = p_GridView.Rows.Count;
            string[] _TempText = new string[p_ColumnsIndex.Length];
            int[] _RowIndex = new int[p_ColumnsIndex.Length];
            for (int i = 0; i != _Count; i++)
            {
                string _CellText = "";
                for (int z = 0; z != p_ColumnsIndex.Length; z++)
                {
                    _CellText += p_GridView.Rows[i].Cells[p_ColumnsIndex[z]].Text;
                    if (_TempText[z] == _CellText)
                    {
                        p_GridView.Rows[i].Cells[p_ColumnsIndex[z]].Visible = false;
                        p_GridView.Rows[_RowIndex[z]].Cells[p_ColumnsIndex[z]].RowSpan++;
                    }
                    else
                    {
                        _RowIndex[z] = i;
                        _TempText[z] = _CellText;
                        p_GridView.Rows[_RowIndex[z]].Cells[p_ColumnsIndex[z]].RowSpan = 1;
                    }
                }
            }
        }
        
    }
}