using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_addGX : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Button)wuccaidan1.FindControl("Button5")).ForeColor = System.Drawing.Color.Red;
            beginshowJK();
            beginshowFF();
        }
    }

    //读取数据库，显示接口数据。
    private void beginshowJK()
    {
        string sql = " select (JK_host + '[' + JK_path + ']') as showtxt,JK_guid from AAA_ipcJK where JK_open = 1 order by JK_host asc,JK_path asc ";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "接口列表", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            GX_JK_guid.DataSource = dsGXlist.Tables[0].DefaultView;
            GX_JK_guid.DataTextField = "showtxt";
            GX_JK_guid.DataValueField = "JK_guid";
            GX_JK_guid.DataBind();
            GX_JK_guid.Items.Insert(0, new ListItem("请选择", ""));
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
            GX_FF_guid.DataSource = dsFFlist.Tables[0].DefaultView;
            GX_FF_guid.DataTextField = "showtxt";
            GX_FF_guid.DataValueField = "FF_guid";
            GX_FF_guid.DataBind();
            GX_FF_guid.Items.Insert(0, new ListItem("请选择", ""));
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        Hashtable input = new Hashtable();
        ArrayList alsql = new ArrayList(); 

        input["@GX_guid"] = Guid.NewGuid().ToString();
        //input["@GX_shibie"] = GX_shibie.Text;
        input["@GX_shibie"] = GX_shibiel.SelectedValue;
        input["@GX_savelog"] = GX_savelog.SelectedValue;
        input["@GX_JK_guid"] = GX_JK_guid.SelectedValue;
        input["@GX_FF_guid"] = GX_FF_guid.SelectedValue;
        input["@GX_type"] = GX_type.SelectedValue;
        input["@GX_open"] = GX_open.SelectedValue;        

        alsql.Add(" INSERT INTO  AAA_ipcGX(GX_guid,GX_shibie,GX_savelog,GX_JK_guid,GX_FF_guid,GX_type,GX_open) VALUES (@GX_guid,@GX_shibie,@GX_savelog,@GX_JK_guid,@GX_FF_guid,@GX_type,@GX_open) ");

        if (input["@GX_shibie"].ToString().Trim() == "" || input["@GX_JK_guid"].ToString().Trim() == "" || input["@GX_FF_guid"].ToString().Trim() == "")
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('添加失败，关系数据不全！');</script>");
            return;
        }

        //提交

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable return_ht = I_DBL.RunParam_SQL(alsql, input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('添加好了！');window.location.href='addGX.aspx';</script>");
        }
        else
        {
            //读取数据库出错。。。

            Response.Write(return_ht["return_errmsg"].ToString());
        }
    }
}