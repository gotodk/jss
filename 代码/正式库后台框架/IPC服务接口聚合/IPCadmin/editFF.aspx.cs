using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_editFF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
        sql = sql + " from   AAA_ipcFF as FF left join AAA_ipcJK as JK on JK.JK_guid=FF.FF_JK_guid ";
        sql=sql+" where FF.FF_guid=@FF_guid";

        

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsFFlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@FF_guid"]=Request ["FF_guid"].ToString ();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "方法信息", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsFFlist = (DataSet)(return_ht["return_ds"]);

            lblJK_host.Text = dsFFlist.Tables[0].Rows[0]["接口域名"].ToString();
            lblJK_path.Text = dsFFlist.Tables[0].Rows[0]["接口地址"].ToString();
            lblFF_guid.Text = dsFFlist.Tables[0].Rows[0]["方法唯一标示"].ToString();
            FF_yewuname.Text = dsFFlist.Tables[0].Rows[0]["业务名称"].ToString();
            lblFF_yewuname.Text = dsFFlist.Tables[0].Rows[0]["业务名称"].ToString();
            FF_name.Text = dsFFlist.Tables[0].Rows[0]["方法名"].ToString();
            lblFF_name.Text = dsFFlist.Tables[0].Rows[0]["方法名"].ToString();
            FF_retype.Text = dsFFlist.Tables[0].Rows[0]["返回值类型"].ToString();
            lblFF_retype.Text = dsFFlist.Tables[0].Rows[0]["返回值类型"].ToString();
            FF_canshu.Text = dsFFlist.Tables[0].Rows[0]["参数类型"].ToString();
            lblFF_canshu.Text = dsFFlist.Tables[0].Rows[0]["参数类型"].ToString();
            FF_shuoming.Text = dsFFlist.Tables[0].Rows[0]["方法注释"].ToString();
            lblFF_shuoming.Text = dsFFlist.Tables[0].Rows[0]["方法注释"].ToString();
            FF_open.SelectedValue = dsFFlist.Tables[0].Rows[0]["方法是否有效"].ToString() == "有效" ? "1" : "0";
            lblFF_open.Text = dsFFlist.Tables[0].Rows[0]["方法是否有效"].ToString() == "有效" ? "1" : "0";
            FF_CorE.SelectedValue = dsFFlist.Tables[0].Rows[0]["操作特点"].ToString() == "仅查询操作" ? "0" : "1";
            lblFF_CorE.Text = dsFFlist.Tables[0].Rows[0]["操作特点"].ToString() == "仅查询操作" ? "0" : "1";
            string fflog = dsFFlist.Tables[0].Rows[0]["日志设置"].ToString();
            foreach (ListItem item in FF_Log.Items)
            {
                if (fflog.IndexOf(item.Text) >= 0)
                {
                    item.Selected = true;
                }
            }
            lblFF_log.Text = fflog;
           
        }
        else
        {           
            Response.Write(return_ht["return_errmsg"].ToString());
        }

    }
    protected void btn_EditJK_Click(object sender, EventArgs e)
    {
        if (FF_yewuname.Text.Trim() == "" || FF_name.Text.Trim() == "" || FF_retype.Text.Trim() == "" || FF_canshu.Text.Trim() == "" || FF_shuoming.Text.Trim() == "")
        {
            string url = "editFF.aspx?FF_guid=" + Request["guid"].ToString();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' defer>alert('修改失败，方法数据不全！'); window.location.href='" + url + "'</script>");
        }
        else
        {            
            Hashtable ht = new Hashtable();
            ht["@FF_guid"] = Request["FF_guid"].ToString();
            ht["@FF_edittime"] = DateTime.Now.ToString();
            ht["@FF_yewuname"] = FF_yewuname.Text.Trim();
            ht["@FF_name"] = FF_name.Text.Trim();
            ht["@FF_retype"] = FF_retype.Text.Trim();
            ht["@FF_canshu"] = FF_canshu.Text.Trim();
            ht["@FF_shuoming"] = FF_shuoming.Text.Trim();          
            ht["@FF_open"] = FF_open.SelectedValue;
            ht["@FF_CorE"] = FF_CorE.SelectedValue;
            string fflog = "";
            foreach (ListItem item in FF_Log.Items)
            {
                if (item.Selected == true)
                {
                    fflog += item.Text + "|";
                }
            }
            if (fflog == "")
            {
                fflog = "关闭";
            }
            ht["@FF_log"] = fflog;

            string update = "";
            if (ht["@FF_yewuname"].ToString() != lblFF_yewuname.Text )
            {
                update += " FF_yewuname=@FF_yewuname";
            }
            if (ht["@FF_name"].ToString() != lblFF_name.Text )
            {
                update += update == "" ? " FF_name=@FF_name" : ",FF_name=@FF_name";
            }
            if (ht["@FF_retype"].ToString() != lblFF_retype.Text )
            {
                update += update == "" ? "FF_retype=@FF_retype" : ",FF_retype=@FF_retype";
            }
            if (ht["@FF_canshu"].ToString() !=lblFF_canshu.Text )
            {
                update += update == "" ? "FF_canshu=@FF_canshu" : ",FF_canshu=@FF_canshu";
            }
            if (ht["@FF_shuoming"].ToString() != lblFF_shuoming.Text)
            {
                update += update == "" ? "FF_shuoming=@FF_shuoming" : ",FF_shuoming=@FF_shuoming";
            }
            if (ht["@FF_open"].ToString() != lblFF_open.Text )
            {
                update += update == "" ? "FF_open=@FF_open" : ",FF_open=@FF_open";
            }
            if (ht["@FF_CorE"].ToString() != lblFF_CorE.Text )
            {
                update += update == "" ? "FF_CorE=@FF_CorE" : ",FF_CorE=@FF_CorE";
            }
            if (ht["@FF_log"].ToString() != lblFF_log.Text )
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
                DataSet dslist = new DataSet();
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
    }
    protected void btn_Back_Click(object sender, EventArgs e)
    {
        if (Request["FF_guid"] != null && Request["FF_guid"].ToString() != "")
        {
            Response.Redirect("showFFinfo.aspx?FF_guid=" + Request["FF_guid"].ToString());
        }        
    }
}