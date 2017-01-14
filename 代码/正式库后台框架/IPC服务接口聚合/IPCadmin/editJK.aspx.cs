using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_editJK : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string jkguid = "";
            if (Request["guid"] != null && Request["guid"].ToString() != "")
            {
                jkguid = Request["guid"].ToString();
                beginshow0(jkguid);
            }
        }
    }
    //读取数据库，显示接口信息。
    private void beginshow0(string jkguid)
    {
        string sql = "  select JK.JK_guid as 接口唯一标示,JK.JK_host as 接口域名,(select count(*) from AAA_ipcIP where IP_JK_host = JK.JK_host and IP_open = '正常负载') as 负载IP数量,JK.JK_path as 接口地址,JK.JK_shuoming as 接口说明,  ";
        sql = sql + "  JK.JK_banben as 接口版本号,JK.JK_open as 接口是否有效,   ";
        sql = sql + "  (CASE JK.JK_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as 接口目前状态,  ";
        sql = sql + " JK.JK_addtime as 添加时间,JK.JK_edittime as 最后一次修改时间 ,JK.JK_port as 接口备用端口 ";
        sql = sql + " from AAA_ipcJK as JK   ";
        sql = sql + " where JK.JK_guid = @JK_guid ";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsJK = new DataSet();
        Hashtable input = new Hashtable();
        input["@JK_guid"] = jkguid;
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "接口", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsJK = (DataSet)(return_ht["return_ds"]);
            if (dsJK!= null && dsJK.Tables[0].Rows.Count > 0)            
            {
                lblJKgudi.Text = dsJK.Tables["接口"].Rows[0]["接口唯一标示"].ToString();
                lblFZIPSL.Text = dsJK.Tables["接口"].Rows[0]["负载IP数量"].ToString();
                lblJKZT.Text = dsJK.Tables["接口"].Rows[0]["接口目前状态"].ToString();
                txtJKYM.Text = dsJK.Tables["接口"].Rows[0]["接口域名"].ToString();
                lblJKYM.Text = dsJK.Tables["接口"].Rows[0]["接口域名"].ToString();
                txtJKDZ.Text = dsJK.Tables["接口"].Rows[0]["接口地址"].ToString();
                lblJKDZ.Text = dsJK.Tables["接口"].Rows[0]["接口地址"].ToString();
                txtJKSM.Text = dsJK.Tables["接口"].Rows[0]["接口说明"].ToString();
                lblJKSM.Text = dsJK.Tables["接口"].Rows[0]["接口说明"].ToString();
                txtJKBB.Text = dsJK.Tables["接口"].Rows[0]["接口版本号"].ToString();
                lblJKBB.Text = dsJK.Tables["接口"].Rows[0]["接口版本号"].ToString();
                lbJKYX.SelectedValue = dsJK.Tables["接口"].Rows[0]["接口是否有效"].ToString();
                lblJKYX.Text = dsJK.Tables["接口"].Rows[0]["接口是否有效"].ToString();
                txtBYDK.Text = dsJK.Tables["接口"].Rows[0]["接口备用端口"].ToString();
                lblBYDK.Text = dsJK.Tables["接口"].Rows[0]["接口备用端口"].ToString();
            }
        }
        else
        {
            //读取数据库出错。。。
            dsJK = null;
            Response.Write(return_ht["return_errmsg"].ToString());
        }

    }
    protected void btn_EditJK_Click(object sender, EventArgs e)
    {
        if (txtJKDZ.Text.Trim() == "" || txtJKYM.Text.Trim() == "" || txtJKSM.Text.Trim() == "" || txtJKBB.Text.Trim() == "" || txtBYDK.Text.Trim() == "")
        {
            string url = "editJK.aspx?guid=" + Request["guid"].ToString();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' defer>alert('修改失败，接口数据不全！'); window.location.href='" + url + "'</script>");
        }
        else
        {
            string update = "";
            Hashtable ht = new Hashtable();
            ht["@JK_guid"] = Request["guid"].ToString();
            ht["@JK_edittime"] = DateTime.Now.ToString();
            if (txtJKYM.Text.Trim() != lblJKYM.Text)
            {
                ht["@JK_host"] = txtJKYM.Text.Trim();
                update = "JK_host=@JK_host";
            }
            if (txtJKDZ.Text.Trim() != lblJKDZ.Text)
            {
                ht["@JK_path"] = txtJKDZ.Text.Trim();
                update += update.Trim() == "" ? "JK_path=@JK_path" : ",JK_path=@JK_path";
            }
            if (txtJKSM.Text.Trim() != lblJKSM.Text)
            {
                ht["@JK_shuoming"] = txtJKSM.Text.Trim();
                update += update.Trim() == "" ? "JK_shuoming=@JK_shuoming" : ",JK_shuoming=@JK_shuoming";
            }
            if (txtJKBB.Text.Trim() != lblJKBB.Text)
            {
                ht["@JK_banben"] = txtJKBB.Text.Trim();
                update += update.Trim() == "" ? "JK_banben=@JK_banben" : ",JK_banben=@JK_banben";
            }
            if (lbJKYX.SelectedValue != lblJKYX.Text)
            {
                ht["@JK_open"] = lbJKYX.SelectedValue;
                update += update.Trim() == "" ? "JK_open=@JK_open" : ",JK_open=@JK_open";
            }
            if (txtBYDK.Text.Trim() != lblBYDK.Text)
            {
                ht["@JK_port"] = txtBYDK.Text.Trim();
                update += update.Trim() == "" ? "JK_port=@JK_port" : ",JK_port=@JK_port";
            }
            if (update.Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' defer>alert('各字段值无变化，不需要提交！');</script>");
                return;
            }

            string sql = "update AAA_ipcJK set " + update + ",JK_edittime=@jk_edittime where JK_guid=@JK_guid";           
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");           
            Hashtable return_ht = I_DBL.RunParam_SQL(sql,ht);
            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                string url = "editJK.aspx?guid=" + Request["guid"].ToString();
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('接口信息修改成功！');window.location.href='" + url + "';</script>");
            }
            else
            {
                // Response.Write(return_ht["return_errmsg"].ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' defer>alert('接口信息修改失败！');</script>");
                return;
            }
        }
    }
    protected void btn_Back_Click(object sender, EventArgs e)
    {
        if (Request["guid"] != null && Request["guid"].ToString() != "")
        {
            Response.Redirect("showJKinfo.aspx?JK_guid=" + Request["guid"].ToString());
        }
        else
        {
            Response.Redirect("showJKinfo.aspx");
        }
    }

}