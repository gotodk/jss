using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_editGX : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            beginshowJK();
            beginshowFF();
            beginshow();
        }
    }
    //读取数据库，显示接口关系。
    private void beginshow()
    {
        string sql = " select GX.GX_guid as 关系唯一标示,GX.GX_shibie as 调用方识别标记,(CASE GX.GX_savelog WHEN '1' THEN '开启'  WHEN '0' THEN '关闭' ELSE '未知' END) as 是否开启日志, ";
        sql = sql + " GX.GX_JK_guid as 被调用接口唯一标示, GX.GX_FF_guid as 被调用方法唯一标示, ";       
        sql = sql + " (CASE GX.GX_type WHEN '1' THEN '同步执行'  WHEN '0' THEN '异步执行'  ELSE '未知'  END) as 关系调用方式,(CASE GX.GX_open WHEN '1' THEN '有效'  WHEN '0' THEN '禁用'  ELSE '未知'  END) as 关系是否有效, GX.GX_addtime as 关系添加时间,GX.GX_edittime as 关系最后修改时间  ";
        sql = sql + " from AAA_ipcGX as GX left join AAA_ipcJK as JK on GX.GX_JK_guid=JK.JK_guid  left join AAA_ipcFF as FF on GX.GX_FF_guid = FF.FF_guid and FF.FF_JK_guid = JK.JK_guid ";

        sql = sql + " where GX.GX_guid = @GX_guid ";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@GX_guid"] = Request["GX_guid"].ToString();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "关系", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            lblGX_guid.Text = dsGXlist.Tables["关系"].Rows[0]["关系唯一标示"].ToString();

            GX_shibie.Text = dsGXlist.Tables["关系"].Rows[0]["调用方识别标记"].ToString();
            lblGX_shibie.Text = dsGXlist.Tables["关系"].Rows[0]["调用方识别标记"].ToString();

            GX_savelog.SelectedValue = dsGXlist.Tables["关系"].Rows[0]["是否开启日志"].ToString() == "开启" ? "1" : "0";
            lblGX_savelog.Text = dsGXlist.Tables["关系"].Rows[0]["是否开启日志"].ToString() == "开启" ? "1" : "0";

            lblGX_JK_guid.Text = dsGXlist.Tables["关系"].Rows[0]["被调用接口唯一标示"].ToString();
            //根据接口guid绑定接口下拉框
            GX_JK_guid.SelectedValue = dsGXlist.Tables["关系"].Rows[0]["被调用接口唯一标示"].ToString();

            beginshowFF();
            lblGX_FF_guid.Text = dsGXlist.Tables["关系"].Rows[0]["被调用方法唯一标示"].ToString();
            //根据方法guid绑定方法下拉框
            GX_FF_guid.SelectedValue = dsGXlist.Tables["关系"].Rows[0]["被调用方法唯一标示"].ToString();

            GX_type.SelectedValue = dsGXlist.Tables["关系"].Rows[0]["关系调用方式"].ToString() == "同步执行" ? "1" : "0";
            lblGX_type.Text = dsGXlist.Tables["关系"].Rows[0]["关系调用方式"].ToString() == "同步执行" ? "1" : "0";

            GX_open.SelectedValue = dsGXlist.Tables["关系"].Rows[0]["关系是否有效"].ToString() == "有效" ? "1" : "0";
            lblGX_open.Text = dsGXlist.Tables["关系"].Rows[0]["关系是否有效"].ToString() == "有效" ? "1" : "0";
        }
        else
        {
            //读取数据库出错。。。
            dsGXlist = null;
            Response.Write(return_ht["return_errmsg"].ToString());
        }

    }
    //读取数据库，显示接口关系。
    private void beginshowJK()
    {
        string sql = " select (JK_host + '[' + JK_path + ']') as showtxt,JK_guid from AAA_ipcJK where JK_open = 1 order by JK_host asc,JK_path asc ";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsJKlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "接口列表", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsJKlist = (DataSet)(return_ht["return_ds"]);
            GX_JK_guid.DataSource = dsJKlist.Tables["接口列表"].DefaultView;
            GX_JK_guid.DataTextField = "showtxt";
            GX_JK_guid.DataValueField = "JK_guid";
            GX_JK_guid.DataBind();
            //Response.Write(dsGXlist.Tables[0].Rows.Count.ToString());

            if (GX_JK_guid.Items.Count > 0)
            {
                GX_JK_guid.SelectedIndex = 0;
            }
        }
        else
        {
            //读取数据库出错。。。
            GX_JK_guid.DataSource = null;
            GX_JK_guid.DataBind();
            Response.Write(return_ht["return_errmsg"].ToString());
        }

    }
    protected void GX_JK_guid_SelectedIndexChanged(object sender, EventArgs e)
    {
        beginshowFF();
    }

    //读取数据库
    private void beginshowFF()
    {
        string sql = " select (FF_yewuname + '->' + FF_name + '[' + (CASE FF_CorE WHEN '1' THEN '有插入或更新操作'  WHEN '0' THEN '仅查询操作'  ELSE '未知'  END) +']') as showtxt,FF_guid from AAA_ipcFF where FF_open = 1 and FF_JK_guid = @FF_JK_guid order by FF_yewuname asc,FF_name asc ";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsFFlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@FF_JK_guid"] = GX_JK_guid.SelectedValue;
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "方法列表", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsFFlist = (DataSet)(return_ht["return_ds"]);
            GX_FF_guid.DataSource = dsFFlist.Tables["方法列表"].DefaultView;
            GX_FF_guid.DataTextField = "showtxt";
            GX_FF_guid.DataValueField = "FF_guid";
            GX_FF_guid.DataBind();
            //Response.Write(dsGXlist.Tables[0].Rows.Count.ToString());
            if (GX_FF_guid.Items.Count > 0)
            {
                GX_FF_guid.SelectedIndex = 0;
            }
        }
        else
        {
            //读取数据库出错。。。
            GX_FF_guid.DataSource = null;
            GX_FF_guid.DataBind();
            Response.Write(return_ht["return_errmsg"].ToString());
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (GX_shibie.Text.Trim() == "")
        {
            string url = "editGX.aspx?GX_guid=" + Request["GX_guid"].ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('修改失败，关系数据不全！');window.location.href='" + url + "'</script>");
        }
        else
        {
            Hashtable input = new Hashtable();          
            input["@GX_guid"] = lblGX_guid.Text;
            input["@GX_shibie"] = GX_shibie.Text;
            input["@GX_savelog"] = GX_savelog.SelectedValue;
            input["@GX_JK_guid"] = GX_JK_guid.SelectedValue;
            input["@GX_FF_guid"] = GX_FF_guid.SelectedValue;
            input["@GX_type"] = GX_type.SelectedValue;
            input["@GX_open"] = GX_open.SelectedValue;
            input["@GX_edittime"] = DateTime.Now.ToString();

            string update = "";
            if (input["@GX_shibie"].ToString() != lblGX_shibie.Text)
            {
                update += "GX_shibie=@GX_shibie";
            }
            if (input["@GX_savelog"].ToString() != lblGX_savelog.Text)
            {
                update += update == "" ? "GX_savelog=@GX_savelog" : ",GX_savelog=@GX_savelog";
            }
            if (input["@GX_JK_guid"].ToString() != lblGX_JK_guid.Text)
            {
                update += update == "" ? "GX_JK_guid=@GX_JK_guid" : ",GX_JK_guid=@GX_JK_guid";
            }
            if (input["@GX_FF_guid"].ToString() != lblGX_FF_guid.Text)
            {
                update += update == "" ? "GX_FF_guid=@GX_FF_guid" : ",GX_FF_guid=@GX_FF_guid";
            }
            if (input["@GX_type"].ToString() != lblGX_type.Text)
            {
                update += update == "" ? "GX_type=@GX_type" : ",GX_type=@GX_type";
            }
            if (input["@GX_open"].ToString() != lblGX_open.Text)
            {
                update += update == "" ? "GX_open=@GX_open" : ",GX_open=@GX_open";
            }

            if (update.Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('各字段值无变化，不需要提交！');</script>");
                return;
            }

            string sql = "update AAA_ipcGX set " + update + ", GX_edittime=@GX_edittime where GX_guid=@GX_guid";  
            //提交
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable return_ht = I_DBL.RunParam_SQL(sql, input);
            
            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                string url="editGX.aspx?GX_guid=" + Request["GX_guid"].ToString();
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('修改成功！');window.location.href='" + url + "';</script>");
            }
            else
            {
                //读取数据库出错。。。

                Response.Write(return_ht["return_errmsg"].ToString());
            }
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Request["GX_guid"] != null && Request["GX_guid"].ToString() != "")
        {
            Response.Redirect("showGXinfo.aspx?GX_guid=" + Request["GX_guid"].ToString());
        }       
    }
}