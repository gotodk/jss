using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_allJKlist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Button)wuccaidan1.FindControl("Button2")).ForeColor = System.Drawing.Color.Red;
            beginshow();
        }
    }
    //读取数据库，显示接口。
    private void beginshow()
    {


        string sql = " select JK.JK_guid as 接口唯一标示,JK.JK_host as 接口域名,JK.JK_path as 接口地址,JK.JK_shuoming as 接口说明,  (select count(*) from AAA_ipcIP where IP_JK_host = JK.JK_host  and IP_open = '正常负载' ) as 负载IP数量, ";
        sql = sql + " JK.JK_banben as 接口版本号,(CASE JK.JK_open WHEN '1' THEN '有效'  WHEN '0' THEN '禁用'  ELSE '未知'  END) as 接口是否有效, (CASE JK.JK_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as 接口目前状态, ";

        sql = sql + " JK.JK_addtime as 添加时间,JK.JK_edittime as 最后一次修改时间, JK.JK_port as 接口备用端口, ";
        sql = sql + " '含有效方法数量' = isnull((select count(FF.FF_guid) from AAA_ipcFF as FF where FF.FF_JK_guid=JK.JK_guid and FF.FF_open = 1),0), ";
        sql = sql + " '含禁用方法数量'=isnull((select count(FF.FF_guid) from AAA_ipcFF as FF where FF.FF_JK_guid=JK.JK_guid and FF.FF_open = 0),0)  from AAA_ipcJK as JK  ";
        sql = sql + " " + TextBox2.Text + " ";

        if (txt_jkym.Text.Trim() != "")
        {
            sql = sql + " and JK.JK_host like '%" + txt_jkym.Text.Trim() + "%'";
        }
        if (txt_jkdz.Text.Trim() != "")
        {
            sql = sql + " and JK.JK_path like '%" + txt_jkdz.Text.Trim() + "%'";
        }
        sql = sql + " " + TextBox1.Text + " ";
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsJKlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "接口列表", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsJKlist = (DataSet)(return_ht["return_ds"]);
            GridView1.DataSource = dsJKlist.Tables[0].DefaultView;
            GridView1.DataBind();
            //Response.Write(dsGXlist.Tables[0].Rows.Count.ToString());
        }
        else
        {
            //读取数据库出错。。。
            GridView1.DataSource = null;
            GridView1.DataBind();

            Response.Write(return_ht["return_errmsg"].ToString());
        }

    }
    //刷新
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("allJKlist.aspx");
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //如果当前行尾数据行
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //添加鼠标在当前行时的background-color属性
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#fbed90'){this.style.backgroundColor='#aadef6';}");
            //鼠标离开当前行后
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#fbed90'){this.style.backgroundColor='#ffffff';}");
            e.Row.Attributes.Add("onclick", "if(window.oldtr!=null){window.oldtr.runtimeStyle.cssText= ' ';}this.runtimeStyle.cssText= 'background-color:#e6c5fc ';window.oldtr=this");
            e.Row.Attributes.Add("ondblclick", "if(this.style.backgroundColor=='#aadef6' || this.style.backgroundColor=='#ffffff'){this.style.backgroundColor='#fbed90';}else{this.style.backgroundColor='#ffffff';}");
        }
    }
    //查询
    protected void btnView_Click(object sender, EventArgs e)
    {
        beginshow();
    }
}