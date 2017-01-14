using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_showJKinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Button)wuccaidan1.FindControl("Button2")).ForeColor = System.Drawing.Color.Red;
            beginshow0();
            beginshow();
        }
    }

    //读取数据库，显示接口信息。
    private void beginshow0()
    {
        string sql = "  select JK.JK_guid as 接口唯一标示,JK.JK_host as 接口域名,(select count(*) from AAA_ipcIP where IP_JK_host = JK.JK_host and IP_open = '正常负载') as 负载IP数量,JK.JK_path as 接口地址,JK.JK_shuoming as 接口说明,  ";
        sql = sql + "  JK.JK_banben as 接口版本号,(CASE JK.JK_open WHEN '1' THEN '有效'  WHEN '0' THEN '禁用'  ELSE '未知'  END) as 接口是否有效,   ";
        sql = sql + "  (CASE JK.JK_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as 接口目前状态,  ";
        sql = sql + " JK.JK_addtime as 添加时间,JK.JK_edittime as 最后一次修改时间 ,JK.JK_port as 接口备用端口 ";
        sql = sql + " from AAA_ipcJK as JK   ";
        sql = sql + " where JK.JK_guid = @JK_guid ";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@JK_guid"] = Request["JK_guid"].ToString();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "接口", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            showjk.InnerHtml = "<table border='0' cellpadding='5' cellspacing='1' bgcolor='#333333'>";
            for (int i = 0; i < dsGXlist.Tables["接口"].Columns.Count; i++)
            {

                showjk.InnerHtml = showjk.InnerHtml + "<tr><td bgcolor='#FFFFFF'><strong>" + dsGXlist.Tables["接口"].Columns[i].ColumnName + "：</strong></td><td bgcolor='#FFFFFF'>" + dsGXlist.Tables["接口"].Rows[0][i].ToString() + "</td></tr>";

            }
            showjk.InnerHtml = showjk.InnerHtml + "</table> ";
        }
        else
        {
            //读取数据库出错。。。
            dsGXlist = null;

            Response.Write(return_ht["return_errmsg"].ToString());
        }

    }

    //读取数据库，显示接口中的方法信息。
    private void beginshow()
    {
        string sql = "  select   ";
        sql = sql + " FF.FF_guid as 方法唯一标示,FF.FF_yewuname as 业务名称,FF.FF_name as 方法名,FF.FF_retype as 返回值类型,FF.FF_canshu as 参数类型,   ";
        sql = sql + "  FF.FF_shuoming as 方法注释, (CASE FF.FF_open WHEN '1' THEN '有效'  WHEN '0' THEN '禁用'  ELSE '未知'  END) as 方法是否有效,   ";
        sql = sql + "   (CASE FF.FF_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as 方法目前状态, ";
        sql = sql + "  (CASE FF.FF_CorE WHEN '1' THEN '有插入或更新操作'  WHEN '0' THEN '仅查询操作'  ELSE '未知'  END) as 操作特点,  ";
        sql = sql + " FF.FF_log  as 日志设置 ";
        sql = sql + " from   AAA_ipcFF as FF     ";
        sql = sql + " where FF.FF_JK_guid = @JK_guid ";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsJKlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@JK_guid"] = Request["JK_guid"].ToString();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "方法", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsJKlist = (DataSet)(return_ht["return_ds"]);
            GridView1.DataSource = dsJKlist.Tables["方法"].DefaultView;
            GridView1.DataBind();
        }
        else
        {
            //读取数据库出错。。。
            //读取数据库出错。。。
            GridView1.DataSource = null;
            GridView1.DataBind();

            Response.Write(return_ht["return_errmsg"].ToString());
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //如果当前行尾数据行
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //添加鼠标在当前行时的background-color属性
            e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#fcdead';");
            //鼠标离开当前行后
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
            e.Row.Attributes.Add("onclick", "if(window.oldtr!=null){window.oldtr.runtimeStyle.cssText= ' ';}this.runtimeStyle.cssText= 'background-color:#e6c5fc ';window.oldtr=this");
        }
    }
   //编辑方法信息
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        beginshow();
        //判断这个方法是不是在接口关系中存在，如果已经存在，提示谨慎修改
        Hashtable ht = new Hashtable();
        ht["@GX_FF_guid"] = ((LinkButton)sender).CommandArgument.ToString();
        string sql = "select * from AAA_ipcGX where GX_FF_guid=@GX_FF_guid";
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dslist = new DataSet();       
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "关系", ht);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dslist = (DataSet)(return_ht["return_ds"]);
            if (dslist.Tables["关系"].Rows.Count > 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('该方法已在接口关系中存在，请谨慎修改！');</script>");
            }
        }
        else
        {
            Response.Write(return_ht["return_errmsg"].ToString());
        }

        LinkButton lkbEdit = (LinkButton)sender;
        GridViewRow gv = (GridViewRow)lkbEdit.Parent.Parent;
        int index = gv.RowIndex;

        ((LinkButton)GridView1.Rows[index].FindControl("lkbEdit")).Visible = false;
        ((LinkButton)GridView1.Rows[index].FindControl("lkbSave")).Visible = true;
        ((LinkButton)GridView1.Rows[index].FindControl("lkbCancel")).Visible = true;

        TextBox FF_yewuname = (TextBox)GridView1.Rows[index].FindControl("FF_yewuname");
        FF_yewuname.Visible = true;        
        ((Label)GridView1.Rows[index].FindControl("lblFF_yewuname")).Visible = false;

        TextBox FF_name = (TextBox)GridView1.Rows[index].FindControl("FF_name");
        FF_name.Visible = true;
        ((Label)GridView1.Rows[index].FindControl("lblFF_name")).Visible = false;

        TextBox FF_retype = (TextBox)GridView1.Rows[index].FindControl("FF_retype");
        FF_retype.Visible = true;
        ((Label)GridView1.Rows[index].FindControl("lblFF_retype")).Visible = false;

        TextBox FF_canshu = (TextBox)GridView1.Rows[index].FindControl("FF_canshu");
        FF_canshu.Visible = true;
        ((Label)GridView1.Rows[index].FindControl("lblFF_canshu")).Visible = false;

        TextBox FF_shuoming = (TextBox)GridView1.Rows[index].FindControl("FF_shuoming");
        FF_shuoming.Visible = true;
        ((Label)GridView1.Rows[index].FindControl("lblFF_shuoming")).Visible = false;

        ListBox FF_open = (ListBox)GridView1.Rows[index].FindControl("FF_open");
        FF_open.Visible = true;
        Label lblFF_open = ((Label)GridView1.Rows[index].FindControl("lblFF_open"));
        FF_open.SelectedValue = lblFF_open.Text == "有效" ? "1" : "0";
        lblFF_open.Visible = false;

        ListBox FF_CorE = (ListBox)GridView1.Rows[index].FindControl("FF_CorE");
        FF_CorE.Visible = true;
        Label lblFF_CorE = (Label)GridView1.Rows[index].FindControl("lblFF_CorE");
        FF_CorE.SelectedValue = lblFF_CorE.Text == "仅查询操作" ? "0" : "1";
        lblFF_CorE.Visible = false;

        CheckBoxList FF_Log = (CheckBoxList)GridView1.Rows[index].FindControl("FF_Log");
        FF_Log.Visible = true;
        Label lblFF_log = (Label)GridView1.Rows[index].FindControl("lblFF_Log");
        string choose = lblFF_log.Text;
        foreach (ListItem lt in FF_Log.Items)
        {
            if (choose.IndexOf(lt.Text) >= 0)
            {
                lt.Selected = true;
            }
        }        
        lblFF_log.Visible = false;
        
    }
    //保存方法的修改
    protected void btnSave_Click(object sender, EventArgs e)
    {
        LinkButton lkbSave = (LinkButton)sender;
        GridViewRow gv = (GridViewRow)lkbSave.Parent.Parent;
        int index = gv.RowIndex;

        Hashtable ht = new Hashtable();
        ht["@FF_guid"] = lkbSave.CommandArgument.ToString();
        ht["@FF_edittime"] = DateTime.Now.ToString();
        ht["@FF_yewuname"] = ((TextBox)GridView1.Rows[index].FindControl("FF_yewuname")).Text;
        string FF_yewuname = ((Label)GridView1.Rows[index].FindControl("lblFF_yewuname")).Text;

        ht["@FF_name"] = ((TextBox)GridView1.Rows[index].FindControl("FF_name")).Text;
        string FF_name = ((Label)GridView1.Rows[index].FindControl("lblFF_name")).Text;

        ht["@FF_retype"] = ((TextBox)GridView1.Rows[index].FindControl("FF_retype")).Text;
        string FF_retype = ((Label)GridView1.Rows[index].FindControl("lblFF_retype")).Text;

        ht["@FF_canshu"] = ((TextBox)GridView1.Rows[index].FindControl("FF_canshu")).Text;
        string FF_canshu = ((Label)GridView1.Rows[index].FindControl("lblFF_canshu")).Text;

        ht["@FF_shuoming"] = ((TextBox)GridView1.Rows[index].FindControl("FF_shuoming")).Text.Trim();
        string FF_shuoming = ((Label)GridView1.Rows[index].FindControl("lblFF_shuoming")).Text;

        ListBox lbFF_open= (ListBox)GridView1.Rows[index].FindControl("FF_open");
        ht["@FF_open"] = lbFF_open.SelectedValue;           
        string FF_open = ((Label)GridView1.Rows[index].FindControl("lblFF_open")).Text == "有效" ? "1" : "0";

        ListBox lbFF_CorE=(ListBox)GridView1.Rows[index].FindControl("FF_CorE");
        ht["@FF_CorE"] = lbFF_CorE.SelectedValue;
        string FF_CorE = ((Label)GridView1.Rows[index].FindControl("lblFF_CorE")).Text == "仅查询操作" ? "0" : "1";

        CheckBoxList ckbFF_Log = (CheckBoxList)GridView1.Rows[index].FindControl("FF_Log");
        string strFF_log = "";
        foreach (ListItem item in ckbFF_Log.Items)
        {
            if (item.Selected == true)
                strFF_log += item.Text + "|";
        }     
        if (strFF_log == "")
        {
            strFF_log = "关闭";
        }
        ht["@FF_log"] = strFF_log;
        string FF_log = ((Label)GridView1.Rows[index].FindControl("lblFF_log")).Text;

        string update="";
        if (ht["@FF_yewuname"].ToString() != FF_yewuname)
        {
            update += " FF_yewuname=@FF_yewuname";
        }
        if (ht["@FF_name"].ToString() != FF_name)
        {
            update += update == "" ? " FF_name=@FF_name" : ",FF_name=@FF_name";
        }
        if (ht["@FF_retype"].ToString() != FF_retype)
        {           
            update += update == "" ? "FF_retype=@FF_retype" : ",FF_retype=@FF_retype";
        }
        if (ht["@FF_canshu"].ToString() != FF_canshu)
        {
            update += update == "" ? "FF_canshu=@FF_canshu" : ",FF_canshu=@FF_canshu";
        }
        if (ht["@FF_shuoming"].ToString() != FF_shuoming)
        {
            update += update == "" ? "FF_shuoming=@FF_shuoming" : ",FF_shuoming=@FF_shuoming";
        }
        if (ht["@FF_open"].ToString() != FF_open)
        {
            update += update == "" ? "FF_open=@FF_open" : ",FF_open=@FF_open";
        }
        if (ht["@FF_CorE"].ToString() != FF_CorE)
        {
            update += update == "" ? "FF_CorE=@FF_CorE" : ",FF_CorE=@FF_CorE";
        }
        if (ht["@FF_log"].ToString() != FF_log)
        {
            update += update == "" ? "FF_log=@FF_log" : ",FF_log=@FF_log";
        }
        if (update == "")
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('各字段值无变化，不需要保存！');</script>");
            return;
        }
        else
        {
            string sql = "update AAA_ipcFF set " + update + ",FF_edittime=@FF_edittime where FF_guid=@FF_guid ";
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");          
            Hashtable return_ht = I_DBL.RunParam_SQL(sql,ht);

            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                beginshow();
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('保存成功！');</script>");
            }
            else
            {
                Response.Write(return_ht["return_errmsg"].ToString());
            }
        }
    }
    //取消方法的修改
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        beginshow();
    }
}