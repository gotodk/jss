using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pingtaiservices_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Hashtable htmes = new Hashtable();
        htmes["type"] = "业务平台";
        htmes["模提醒模块名"] = "AAA_FWJLXXB";
        htmes["提醒内容文本"] = "测试提醒";
        htmes["查看地址"] = "abcdf";
        List<Hashtable> lth = new List<Hashtable>();
        lth.Add(htmes);
        PublicClass2013.Sendmes(lth);
    }
}