using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_IPlist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Button)wuccaidan1.FindControl("Button6")).ForeColor = System.Drawing.Color.Red;
            beginshow();
            bindJK(ddlJK);         
        }
    }
    //读取数据库，显示已添加的数据。
    private void beginshow()
    {
        string sql = " select IP.IP_guid,IP.IP_address as IP地址,IP.IP_JK_host as 接口域名,IP.IP_open as IP地址是否有效, IP.IP_shuoming as 备注,  ";
        sql = sql + " JK.JK_port as 接口备用端口,(CASE JK.JK_open when '1' then '有效' when '0' then '禁用' else '未知' end) as 接口有效性,(CASE IP.IP_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as IP地址状态,IP_addtime as IP添加时间,IP_edittime as IP最后修改时间  ";

        sql = sql + "  from AAA_ipcIP as  IP left join (select distinct JK_host,JK_port,JK_open from AAA_ipcJK) as JK on IP.IP_JK_host = JK.JK_host  ";
   
        sql = sql + " " + TextBox2.Text + " ";
       
        sql = sql + " " + TextBox1.Text + " ";
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsIPlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "IP负载", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsIPlist = (DataSet)(return_ht["return_ds"]);
            GridView1.DataSource = dsIPlist.Tables["IP负载"].DefaultView;
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
    //刷新按钮
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("IPlist.aspx");
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
 
    //读取数据库，显示接口列表。
    private void bindJK(DropDownList ddljk)
    {
        string sql = " select distinct JK_host from AAA_ipcJK where JK_open = 1 order by JK_host ";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "接口", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            ddljk.Items.Clear();
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            ddljk.DataSource = dsGXlist.Tables["接口"].DefaultView;
            ddljk.DataTextField = "JK_host";
            ddljk.DataValueField = "JK_host";
            ddljk.DataBind();
            ddljk.Items.Insert(0, new ListItem("请选择", ""));            
            if (ddljk.Items.Count > 0)
            {
                ddljk.SelectedIndex = 0;
            }
        }
        else
        {
            //读取数据库出错。。。
            ddljk.DataSource = null;
            ddljk.DataBind();
            ddljk.Items.Insert(0, new ListItem("请选择", ""));
            Response.Write(return_ht["return_errmsg"].ToString());
        }
    }
    //添加新数据
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (ddlJK.SelectedValue.ToString() == "" || txtIP.Text.Trim() == "" || ddlIPYX.SelectedValue.ToString() == "" || txtBZ.Text.Trim() == "")
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('添加失败，信息不全');</script>");
            return;
        }         
        if (IsRepeat())
        {//存在重复数据的情况
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('添加失败，信息已存在！');</script>");
            return;
        }

        //写数据库
        Hashtable ht = new Hashtable();
        ht["@IP_guid"] = Guid.NewGuid().ToString();
        ht["@IP_JK_host"] = ddlJK.SelectedValue.ToString();
        ht["@IP_address"] = txtIP.Text.Trim();
        ht["@IP_open"] = ddlIPYX.SelectedValue.ToString();
        ht["@IP_shuoming"] = txtBZ.Text.Trim();
        ht["@IP_addtime"] = DateTime.Now.ToString();
        ht["@IP_edittime"] = ht["@IP_addtime"];
      
        string sql = "insert into AAA_ipcIP(IP_guid,IP_address,IP_JK_host,IP_open,IP_shuoming,IP_addtime,IP_edittime) values (@IP_guid,@IP_address,@IP_JK_host,@IP_open,@IP_shuoming,@IP_addtime,@IP_edittime)";
     
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, ht);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('添加好了！');window.location.href='IPlist.aspx';</script>");
        }
        else
        {
            //写数据库出错。。。
            Response.Write(return_ht["return_errmsg"].ToString());
        }
    }
    //编辑已添加信息
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        beginshow();
        LinkButton lkbEdit = (LinkButton)sender;
        GridViewRow gv = (GridViewRow)lkbEdit.Parent.Parent;
        int index = gv.RowIndex;

        ((LinkButton)GridView1.Rows[index].FindControl("lkbEdit")).Visible = false;
        ((LinkButton)GridView1.Rows[index].FindControl("lkbSave")).Visible = true;
        ((LinkButton)GridView1.Rows[index].FindControl("lkbCancel")).Visible = true;

        DropDownList ddljk = (DropDownList)GridView1.Rows[index].FindControl("ddljk");
        ddljk.Visible = true;
        bindJK(ddljk);
        ddljk.SelectedValue = ((Label)GridView1.Rows[index].FindControl("lbljk")).Text;
        ((Label)GridView1.Rows[index].FindControl("lbljk")).Visible = false;

        TextBox txtip = (TextBox)GridView1.Rows[index].FindControl("txtip");
        txtip.Visible = true;
        ((Label)GridView1.Rows[index].FindControl("lblip")).Visible = false;

        DropDownList ddlipyx = (DropDownList)GridView1.Rows[index].FindControl("ddlipyx");
        ddlipyx.Visible = true;
        ddlipyx.SelectedValue = ((Label)GridView1.Rows[index].FindControl("lblipyx")).Text;
        ((Label)GridView1.Rows[index].FindControl("lblipyx")).Visible = false;

        TextBox txtbz = (TextBox)GridView1.Rows[index].FindControl("txtbz");
        txtbz.Visible = true;
        ((Label)GridView1.Rows[index].FindControl("lblbz")).Visible = false;
        
    }

    //保存修改信息
    protected void btnSave_Click(object sender, EventArgs e)
    {
        LinkButton lkbSave = (LinkButton)sender;
        GridViewRow gv = (GridViewRow)lkbSave.Parent.Parent;
        int index = gv.RowIndex;

        Hashtable ht = new Hashtable();
        ht["@IP_guid"] = lkbSave.CommandArgument.ToString();
        ht["@IP_address"] = ((TextBox)GridView1.Rows[index].FindControl("txtip")).Text;           
        ht["@IP_JK_host"] =((DropDownList)GridView1.Rows[index].FindControl("ddljk")).SelectedValue;
        ht["@IP_open"] = ((DropDownList)GridView1.Rows[index].FindControl("ddlipyx")).SelectedValue;
        ht["@IP_shuoming"] = ((TextBox)GridView1.Rows[index].FindControl("txtbz")).Text;
        ht["@IP_edittime"] = DateTime.Now.ToString();
        
        string lblip = ((Label)GridView1.Rows[index].FindControl("lblip")).Text;
        string lbljk = ((Label)GridView1.Rows[index].FindControl("lbljk")).Text;
        string lblipyx = ((Label)GridView1.Rows[index].FindControl("lblipyx")).Text;
        string lblbz = ((Label)GridView1.Rows[index].FindControl("lblbz")).Text;

        string update = "";
        if (ht["@IP_address"].ToString() != lblip)
        {
            update += " IP_address=@IP_address";
        }
        if (ht["@IP_JK_host"].ToString() != lbljk)
        {
            update += update == "" ? " IP_JK_host=@IP_JK_host" : ",IP_JK_host=@IP_JK_host";
        }
        if (ht["@IP_open"].ToString() != lblipyx)
        {
            update += update == "" ? "IP_open=@IP_open" : ",IP_open=@IP_open";
        }
        if (ht["@IP_shuoming"].ToString() != lblbz)
        {
            update += update == "" ? "IP_shouming=@IP_shuoming" : ",IP_shuoming=@IP_shuoming";
        }
      
        if (update == "")
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('各字段值无变化，不需要保存！');</script>");
            return;
        }
        else
        {
            //判断是否存在重复信息
            if (IsRepeat())
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('保存失败，信息已存在！');</script>");
                return;
            }
            string sql = "update AAA_ipcIP set " + update + ",IP_edittime=@IP_edittime where IP_guid=@IP_guid ";
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            DataSet dslist = new DataSet();
            Hashtable return_ht = I_DBL.RunParam_SQL(sql,ht);

            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
               // beginshow();
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('保存成功！');window.location.href='IPlist.aspx';</script>");
            }
            else
            {
                Response.Write(return_ht["return_errmsg"].ToString());
            }
        }
    }

    //取消编辑
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        beginshow();
    }

    //判断添加或者修改后的信息是否存在重复的情况
    private bool IsRepeat()
    {
        bool chongfu = false;
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (((Label)row.FindControl ("lblip")).Text.ToString () == txtIP.Text.Trim() &&((Label)row.FindControl("lbljk")).Text.ToString () == ddlJK.SelectedValue.ToString())
            {
                chongfu = true;
                break;
            }
        }
        return chongfu;
    }
    //重置
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlJK.SelectedValue = "";
        txtIP.Text = "";
        ddlIPYX.SelectedValue = "";
        txtBZ.Text = "";
    }

    //查询
    protected void btnView_Click(object sender, EventArgs e)
    {
        beginshow();
    }
}