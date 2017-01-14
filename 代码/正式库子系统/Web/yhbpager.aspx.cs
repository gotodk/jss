using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Galaxy.ClassLib.DataBaseFactory;

public partial class Web_yhbpager : System.Web.UI.Page
{

    public struct RegularExp
{
    public const string Chinese = @"^[\u4E00-\u9FA5\uF900-\uFA2D]+$";
    public const string Color = "^#[a-fA-F0-9]{6}";
    public const string Date = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
    public const string DateTime = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
    public const string Email = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
    public const string Float = @"^(-?\d+)(\.\d+)?$";
    public const string ImageFormat = @"\.(?i:jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$";
    public const string Integer = @"^-?\d+$";
    public const string IP = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
    public const string Letter = "^[A-Za-z]+$";
    public const string LowerLetter = "^[a-z]+$";
    public const string MinusFloat = @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$";
    public const string MinusInteger = "^-[0-9]*[1-9][0-9]*$";
    public const string Mobile = "^0{0,1}13[0-9]{9}$";
    public const string NumbericOrLetterOrChinese = @"^[A-Za-z0-9\u4E00-\u9FA5\uF900-\uFA2D]+$";
    public const string Numeric = "^[0-9]+$";
    public const string NumericOrLetter = "^[A-Za-z0-9]+$";
    public const string NumericOrLetterOrUnderline = @"^\w+$";
    public const string PlusFloat = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
    public const string PlusInteger = "^[0-9]*[1-9][0-9]*$";
    public const string Telephone = @"(\d+-)?(\d{4}-?\d{7}|\d{3}-?\d{8}|^\d{7,8})(-\d+)?";
    public const string UnMinusFloat = @"^\d+(\.\d+)?$";
    public const string UnMinusInteger = @"\d+$";
    public const string UnPlusFloat = @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$";
    public const string UnPlusInteger = @"^((-\d+)|(0+))$";
    public const string UpperLetter = "^[A-Z]+$";
    public const string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
}


    //连接工厂接口
    private I_DBFactory I_DBF;
    //数据库连接接口
    private I_Dblink I_DBL;
    //哈希表,存储存储过程参数
    Hashtable news_hashTb = new Hashtable();
    private int page_index = 0;//分页索引，从0开始
    private int page_count = 0;//分页后总页数
    private int record_count = 0;//返回的记录总数量
    private int page_size = 10; //单页数量
    private string serach_Row_str = " * "; //要查询的列表达式
    private string search_str_where = " "; //搜索条件
    private string search_tbname = " FMEventLog "; //要查询的表名称
    private string search_mainid = " id "; //主键
    private string search_paixu = " DESC ";  //排序方式表达式
    private string search_paixuZD = " id "; //用来排序的字段
    private int search_fyshow = 5; //显示分页数的一半
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
        if(!Page.IsPostBack)
        {
            //初始化默认参数
            ViewState["_page_index"] = page_index;//分页索引，从0开始
            ViewState["_page_count"] = page_count;//分页后总页数
            ViewState["_record_count"] = record_count;//返回的记录总数量
            ViewState["_page_size"] = page_size; //单页数量
            ViewState["_serach_Row_str"] = serach_Row_str; //要查询的列表达式
            ViewState["_search_str_where"] = search_str_where; //搜索条件
            ViewState["_search_tbname"] = search_tbname; //要查询的表名称
            ViewState["_search_mainid"] = search_mainid; //主键
            ViewState["_search_paixu"] = search_paixu;  //排序方式表达式
            ViewState["_search_paixuZD"] = search_paixuZD; //用来排序的字段


            Begin_b();
        }
        
    }


    protected void Begin_b()
    {
        AppSettingsReader hh = new AppSettingsReader();//调用web.Config中的数据库配置

        //初始化数据工厂
        I_DBF = new DBFactory();
        I_DBL = I_DBF.DbLinkSqlMain(ConfigurationManager.ConnectionStrings["FMOPConn"].ToString());


        //绑定数据
        My_Binding_Pager();

        //重画界面其他信息
        PageBoxGotoNum.Text = (Convert.ToInt32(ViewState["_page_index"]) + 1).ToString();
        fyinfo.InnerHtml = "当前第<>" + PageBoxGotoNum.Text + "页,共" + ViewState["_page_count"] + "页,共" + ViewState["_record_count"] + "条数据,每页" + ViewState["_page_size"] + "条";
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
        Hashtable_PutIn.Add("@PageIndex", ViewState["_page_index"]);//页面索引
        Hashtable_PutIn.Add("@PageSize", ViewState["_page_size"]);//单页数量
        Hashtable_PutIn.Add("@strGetFields",ViewState["_serach_Row_str"]);//要查询的列
        Hashtable_PutIn.Add("@tableName", ViewState["_search_tbname"]);//表名称
        Hashtable_PutIn.Add("@ID", ViewState["_search_mainid"]); //主键
        Hashtable_PutIn.Add("@strWhere", ViewState["_search_str_where"]); //查询条件
        Hashtable_PutIn.Add("@sortName", ViewState["_search_paixu"]); //排序方式,前后空格
        Hashtable_PutIn.Add("@orderName", ViewState["_search_paixuZD"]); //父级查询排序方式,用于排序的字段

        Hashtable_PutOut.Add("@RecordCount", ViewState["_record_count"]); //返回记录总数
        Hashtable_PutOut.Add("@PageCount", ViewState["_page_count"]); //返回分页后页数
        //获取数据
        return_ht = I_DBL.RunProc_CMD("GetCustomersDataPage", DataSet_Beuse, Hashtable_PutIn, ref Hashtable_PutOut);

        ViewState["_page_count"] = Convert.ToInt32(Hashtable_PutOut["@PageCount"]);
        ViewState["_record_count"] = Convert.ToInt32(Hashtable_PutOut["@RecordCount"]);
    }


    /// <summary>
    /// 绑定需要分页的数据
    /// </summary>
    private void My_Binding_Pager()
    {

        getHT_F();
        if ((bool)return_ht["return_float"])
        {
            DataSet_Beuse = (DataSet) return_ht["return_ds"];
            GV_show.DataSource = DataSet_Beuse.Tables[0].DefaultView;
            GV_show.DataBind();
        }
    }

    /// <summary>
        /// 判断字符串是否与指定正则表达式匹配
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <param name="regularExp">正则表达式</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsMatch(string input, string regularExp)
        {
            return Regex.IsMatch(input, regularExp);
        }

        /// <summary>
        /// 验证非负整数（正整数 + 0）
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsUnMinusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnMinusInteger);
        }

        /// <summary>
        /// 验证正整数
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsPlusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.PlusInteger);
        }

        /// <summary>
        /// 验证非正整数（负整数 + 0） 
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsUnPlusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnPlusInteger);
        }

        /// <summary>
        /// 验证负整数
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsMinusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.MinusInteger);
        }

        /// <summary>
        /// 验证整数
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.Integer);
        }

        /// <summary>
        /// 验证非负浮点数（正浮点数 + 0）
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsUnMinusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnMinusFloat);
        }

        /// <summary>
        /// 验证正浮点数
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsPlusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.PlusFloat);
        }

        /// <summary>
        /// 验证非正浮点数（负浮点数 + 0）
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsUnPlusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnPlusFloat);
        }

        /// <summary>
        /// 验证负浮点数
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsMinusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.MinusFloat);
        }

        /// <summary>
        /// 验证浮点数
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.Float);
        }

        /// <summary>
        /// 验证由26个英文字母组成的字符串
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.Letter);
        }

        /// <summary>
        /// 验证由中文组成的字符串
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsChinese(string input)
        {
            return Regex.IsMatch(input, RegularExp.Chinese);
        }

        /// <summary>
        /// 验证由26个英文字母的大写组成的字符串
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsUpperLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.UpperLetter);
        }

        /// <summary>
        /// 验证由26个英文字母的小写组成的字符串
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsLowerLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.LowerLetter);
        }

        /// <summary>
        /// 验证由数字和26个英文字母组成的字符串
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsNumericOrLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.NumericOrLetter);
        }

        /// <summary>
        /// 验证由数字组成的字符串
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsNumeric(string input)
        {
            return Regex.IsMatch(input, RegularExp.Numeric);
        }
        /// <summary>
        /// 验证由数字和26个英文字母或中文组成的字符串
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsNumericOrLetterOrChinese(string input)
        {
            return Regex.IsMatch(input, RegularExp.NumbericOrLetterOrChinese);
        }

        /// <summary>
        /// 验证由数字、26个英文字母或者下划线组成的字符串
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsNumericOrLetterOrUnderline(string input)
        {
            return Regex.IsMatch(input, RegularExp.NumericOrLetterOrUnderline);
        }

        /// <summary>
        /// 验证email地址
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsEmail(string input)
        {
            return Regex.IsMatch(input, RegularExp.Email);
        }

        /// <summary>
        /// 验证URL
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsUrl(string input)
        {
            return Regex.IsMatch(input, RegularExp.Url);
        }

        /// <summary>
        /// 验证电话号码
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsTelephone(string input)
        {
            return Regex.IsMatch(input, RegularExp.Telephone);
        }

        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsMobile(string input)
        {
            return Regex.IsMatch(input, RegularExp.Mobile);
        }

        /// <summary>
        /// 通过文件扩展名验证图像格式
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsImageFormat(string input)
        {
            return Regex.IsMatch(input, RegularExp.ImageFormat);
        }

        /// <summary>
        /// 验证IP
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsIP(string input)
        {
            return Regex.IsMatch(input, RegularExp.IP);
        }

        /// <summary>
        /// 验证日期（YYYY-MM-DD）
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsDate(string input)
        {
            return Regex.IsMatch(input, RegularExp.Date);
        }

        /// <summary>
        /// 验证日期和时间（YYYY-MM-DD HH:MM:SS）
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsDateTime(string input)
        {
            return Regex.IsMatch(input, RegularExp.DateTime);
        }

        /// <summary>
        /// 验证颜色（#ff0000）
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns>验证通过返回true</returns>
        public static bool IsColor(string input)
        {
            return Regex.IsMatch(input, RegularExp.Color);
        }



    protected void Button1_Click(object sender, EventArgs e)
    {
        //修改参数
        page_size =  Convert.ToInt32(TextBox1.Text);

        Begin_b();


    }
    //第一页
    protected void PageBoxFirst_Click(object sender, EventArgs e)
    {
        ViewState["_page_index"] = 0;
        Begin_b();
    }
    //上一页
    protected void PageBoxPre_Click(object sender, EventArgs e)
    {
        ViewState["_page_index"] =  Convert.ToInt32(ViewState["_page_index"]) - 1;
        if (Convert.ToInt32(ViewState["_page_index"]) < 1)
        {
            ViewState["_page_index"] = 0;
        }
        Begin_b();
    }
    //页码
    protected void PageBoxPageNumber_Click(object sender, EventArgs e)
    {

    }
    //下一页
    protected void PageBoxNext_Click(object sender, EventArgs e)
    {
        ViewState["_page_index"] = Convert.ToInt32(ViewState["_page_index"]) + 1;
        if (Convert.ToInt32(ViewState["_page_index"]) > (Convert.ToInt32(ViewState["_page_count"]) - 1))
        {
            ViewState["_page_index"] = Convert.ToInt32(ViewState["_page_count"]) - 1;
        }
        Begin_b();
    }
    //最后一页
    protected void PageBoxEnd_Click(object sender, EventArgs e)
    {
        ViewState["_page_index"] = Convert.ToInt32(ViewState["_page_count"]) - 1;
        
        Begin_b();
    }
    //转到指定页
    protected void PageBoxGoto_Click(object sender, EventArgs e)
    {
        if (IsPlusInt(PageBoxGotoNum.Text))
        {
            ViewState["_page_index"] = Convert.ToInt32(PageBoxGotoNum.Text) - 1;
        }
        else
        {
            ViewState["_page_index"] = 0;
        }
        
        if (Convert.ToInt32(ViewState["_page_index"]) < 1)
        {
            ViewState["_page_index"] = 0;
        }
        if (Convert.ToInt32(ViewState["_page_index"]) > (Convert.ToInt32(ViewState["_page_count"]) - 1))
        {
            ViewState["_page_index"] = Convert.ToInt32(ViewState["_page_count"]) - 1;
        }

        Begin_b();
    }
}
