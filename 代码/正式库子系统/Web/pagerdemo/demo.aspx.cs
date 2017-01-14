using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class Web_pagerdemo_demo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        commonpager1.OnNeedLoadData += new Web_pagerdemo_commonpager.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
        //commonpager1.OnNeedLoadData_all += new Web_pagerdemo_commonpager.OnNeedDataHandler(MyWebControl_OnNeedLoadData);
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
        //ht_where["page_size"] = "10"; //可以不设置，默认10。每页的数据量。必须是数字。不能是0。
        ht_where["serach_Row_str"] = " id,module,OperateTime,IP "; //检索字段
        ht_where["search_tbname"] = " FMEventLog ";  //检索的表
        ht_where["search_mainid"] = " id ";  //所检索表的主键
        ht_where["search_str_where"] = " 1=1 ";  //检索条件
        ht_where["search_paixu"] = " DESC ";  //排序方式
        ht_where["search_paixuZD"] = " id ";  //用于排序的字段
        return ht_where;
    }
    //绑定事件
    private void MyWebControl_OnNeedLoadData(DataSet NewDS, string ERRinfo)
    {
        //输出调试错误
        //Response.Write(ERRinfo);
        //Response.End();
        GV_show.DataSource = null;
        if (NewDS != null && NewDS.Tables.Count > 0)
        {
            //有数据
            GV_show.DataSource = NewDS.Tables[0].DefaultView;
        }
        GV_show.DataBind();
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        //应用实例
        string testwhere = TextBox1.Text;
        //开始调用自定义控件
        Hashtable HTwhere = SetV();
        //设置本次要变更的参数值
        HTwhere["search_str_where"] = HTwhere["search_str_where"].ToString() + " and ID > " + testwhere;
        //如果是特殊，可以进行特殊处理
        commonpager1.HTwhere = HTwhere;
        commonpager1.GetFYdataAndRaiseEvent();
    }
}