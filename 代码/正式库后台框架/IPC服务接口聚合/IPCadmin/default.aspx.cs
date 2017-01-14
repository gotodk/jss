using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Button)wuccaidan1.FindControl("Button1")).ForeColor = System.Drawing.Color.Red;
            beginshow();
        }
    }
    //读取数据库，显示接口关系。
    private void beginshow()
    {


        string sql = " select GX.GX_guid as 关系唯一标示,GX.GX_shibie as 调用方识别标记,(CASE GX.GX_savelog WHEN '1' THEN '开启'  WHEN '0' THEN '关闭' ELSE '未知' END) as 是否开启日志, ";
        sql = sql + " GX.GX_JK_guid as 被调用接口唯一标示, GX.GX_FF_guid as 被调用方法唯一标示, ";
        sql = sql + " JK.JK_host as 接口域名,JK.JK_path as 接口地址,JK.JK_shuoming as 接口说明,JK.JK_banben as 接口版本号,(CASE JK.JK_open WHEN '1' THEN '有效'  WHEN '0' THEN '禁用'  ELSE '未知'  END) as 接口是否有效,(CASE JK.JK_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as 接口目前状态, JK.JK_addtime as 接口添加时间, JK.JK_edittime as 接口最后一次修改时间, ";
        sql = sql + " FF.FF_yewuname as 方法业务名称,FF.FF_name as 方法名,FF.FF_retype as 方法返回值类型,FF.FF_canshu as 方法参数类型,FF.FF_shuoming  as 方法注释,(CASE FF.FF_open WHEN '1' THEN '有效'  WHEN '0' THEN '禁用'  ELSE '未知'  END) as 方法是否有效,(CASE FF.FF_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as 方法目前状态,(CASE FF.FF_CorE WHEN '1' THEN '有插入或更新操作'  WHEN '0' THEN '仅查询操作'  ELSE '未知'  END) as 方法操作特点, ";
        sql = sql + " (CASE GX.GX_type WHEN '1' THEN '同步执行'  WHEN '0' THEN '异步执行'  ELSE '未知'  END) as 关系调用方式,(CASE GX.GX_open WHEN '1' THEN '有效'  WHEN '0' THEN '禁用'  ELSE '未知'  END) as 关系是否有效, GX.GX_addtime as 关系添加时间,GX.GX_edittime as 关系最后修改时间  ";
        sql = sql + " from AAA_ipcGX as GX left join AAA_ipcJK as JK on GX.GX_JK_guid=JK.JK_guid  left join AAA_ipcFF as FF on GX.GX_FF_guid = FF.FF_guid and FF.FF_JK_guid = JK.JK_guid ";
        sql = sql + " " + TextBox2.Text + " ";
        if (txt_dyfbs.Text.Trim() != "")
        {
            sql = sql + " and GX.GX_shibie like '%" + txt_dyfbs.Text.Trim() + "%'";
        }
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
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "关系列表", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            GridView1.DataSource = dsGXlist.Tables[0].DefaultView;
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
        Response.Redirect("default.aspx");
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