using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //ParamHelper p = new ParamHelper();
        //Response.Write(p.GetIp());//获取IP地址测试
        //BR();
        //Response.Write(p.GetPort());//获取端口号
        //BR();
        //Response.Write(p.GetIpAndPortStr());//获取IP:Port
        //BR();
        //Response.Write(DateTime.Now.ToString("yyyyMMdd"));
        //BR();
        //Response.Write(DateTime.Now.ToString("HHmmss"));

        Service s = new Service();
        DataSet ds = new DataSet();
        ds.ReadXml(Server.MapPath("~/XML/Us_Send_1006.xml"));
        DataSet dsre = s.Reincarnation(ds);
        GridView1.DataSource = dsre.Tables["包头"];
        GridView2.DataSource = dsre.Tables["包体"];
        GridView1.DataBind();
        GridView2.DataBind();
    }
    public void BR()
    {
        Response.Write("<br />");
    }
}