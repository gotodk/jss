using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_allFFlist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Button)wuccaidan1.FindControl("Button7")).ForeColor = System.Drawing.Color.Red;
            beginshow();
        }
    }
    //读取数据库，显示接口。
    private void beginshow()
    {
        string sql = "  select   ";
        sql = sql + " FF.FF_guid as 方法唯一标示,FF.FF_JK_guid as接口唯一标示,JK_host as 接口域名,JK.JK_path as 接口地址,  ";
        sql = sql + " FF.FF_yewuname as 业务名称,FF.FF_name as 方法名,FF.FF_retype as 返回值类型,FF.FF_canshu as 参数类型,   ";
        sql = sql + "  FF.FF_shuoming as 方法注释, (CASE FF.FF_open WHEN '1' THEN '有效'  WHEN '0' THEN '禁用'  ELSE '未知'  END) as 方法是否有效,   ";
        sql = sql + "   (CASE FF.FF_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as 方法目前状态, ";
        sql = sql + "  (CASE FF.FF_CorE WHEN '1' THEN '有插入或更新操作'  WHEN '0' THEN '仅查询操作'  ELSE '未知'  END) as 操作特点, FF.FF_log  as 日志设置,FF.FF_addtime as 添加时间,FF.FF_edittime as 最后一次修改时间 ";      
        sql = sql + " from   AAA_ipcFF as FF left join AAA_ipcJK as JK on JK.JK_guid=FF.FF_JK_guid";
        
        sql = sql + " " + TextBox2.Text.Trim() + " ";

        if (txt_jkdz.Text.Trim() != "")
        {
            sql = sql + " and JK.JK_path like '%" + txt_jkdz.Text.Trim() + "%'";
        }
        if (txt_ywmc.Text.Trim() != "")
        {
            sql = sql + " and FF.FF_yewuname like '%" + txt_ywmc.Text.Trim() + "%'";
        }
        if (txt_ffm.Text.Trim() != "")
        {
            sql = sql + " and FF.FF_name like '%" + txt_ffm.Text.Trim() + "%'";
        }

        sql = sql + " " + TextBox1.Text + " ";       

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsJKlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "方法列表", input);

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
        Response.Redirect("allFFlist.aspx");
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
    protected void btnView_Click(object sender, EventArgs e)
    {
        beginshow();
    }   
}