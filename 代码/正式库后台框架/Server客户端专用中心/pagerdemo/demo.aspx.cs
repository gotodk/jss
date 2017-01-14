using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class pagerdemo_demo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //commonpager1.OnNeedLoadData += new pagerdemo_commonpager.OnNeedDataHandler(MyWebControl_OnNeedLoadData); //这个的参数不一样
        commonpager1.OnNeedLoadData_all += new pagerdemo_commonpager.OnNeedDataHandler_all(MyWebControl_OnNeedLoadData);

        if (!IsPostBack)
        {
            bd();
        }

    }
    //设置初始默认值检索
    private Hashtable SetV()
    {
        Hashtable ht_where = new Hashtable();
        //ht_where["GetCustomersDataPage_NAME"] = "GetCustomersDataPage2";//这个可以不设置,默认是GetCustomersDataPage2,即使用普通的存储过程，2和3差不多，某写情况不一定哪个快.
        //ht_where["this_dblink"] = "不赋值或ERP";//这个可以不设置,默认是FMOP,即连接业务平台数据库。如需连接ERP，则需要设置为ERP.
        //ht_where["page_index"] = "0";//这个可以不设置,如果需要指定进入页面后显示第几页，才需要设置。必须是数字
        //ht_where["search_fyshow"] = "5";//这个也可以不设置，默认就是5,即页数可点击的按钮是11个。必须是数字，不能是0。
        //ht_where["count_float"] = "普通";  //这个可以不用设置，特殊情况设置为特殊。默认是普通
        //ht_where["count_zd"] = "0";  //这个可以不用设置，特殊情况设置为一个算出来的值，最好重设where前设置
        //ht_where["page_size"] = "3"; //可以不设置，默认10。每页的数据量。必须是数字。不能是0。
        //ht_where["returnlastpage_open"] = "demo_show.aspx"; //开启自动返回之前页数，留空或不设置时，不返回，若需要返回，需要设置详情页面网址关键字(如aaa.aspx)
        //Hashtable returnlastpage_spHT = new Hashtable();
        //ht_where["returnlastpage_spHT"] = returnlastpage_spHT; //自动返回之前页数功能额度参数，可以设置特殊返回值，用于处理页面中条件的显示等。
        ht_where["serach_Row_str"] = " id,module,OperateTime,IP "; //检索字段
        ht_where["search_tbname"] = " FMEventLog ";  //检索的表
        ht_where["search_mainid"] = " id ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " id ";  //用于排序的字段
        return ht_where;
    }
    //绑定事件(返回很多东西的,以事件注册为准来决定调用哪个)
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, Hashtable ALLconfig)
    {
        //输出调试错误
        //Response.Write( (Convert.ToInt32(ALLconfig["page_index"]) +1 ).ToString());
        //Response.End();
        GV_show.DataSource = null;
        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            //有数据
            GV_show.DataSource = NewDS.Tables[0].DefaultView;

            //用特殊参数重写条件选择区域
            //TextBox1.Text = ((Hashtable)(ALLconfig["returnlastpage_spHT"]))["TextBox1"].ToString();
        }
        GV_show.DataBind();
    }


    //绑定事件(只返回错误,以事件注册为准来决定调用哪个)
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string err)
    {
        //输出调试错误
        //Response.Write(err);
        //Response.End();
        GV_show.DataSource = null;
        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            //有数据
            GV_show.DataSource = NewDS.Tables[0].DefaultView;
        }
        GV_show.DataBind();
    }

    private void bd()
    {
        //应用实例
        string testwhere = TextBox1.Text;
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        //设置本次要变更的参数值
        HTwhere["search_str_where"] = HTwhere["search_str_where"].ToString() + " and ID > " + testwhere;
        //设置自动返回页面的特殊参数
        //Hashtable returnlastpage_spHT = new Hashtable();
        //returnlastpage_spHT["TextBox1"] = testwhere;
        //HTwhere["returnlastpage_spHT"] = returnlastpage_spHT; //自动返回之前页数功能额度参数，可以设置特殊返回值，用于处理页面中条件的显示等。

        commonpager1.HTwhere = HTwhere;
        commonpager1.GetFYdataAndRaiseEvent();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        bd();
    }

}