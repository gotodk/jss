using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_showFFinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Button)wuccaidan1.FindControl("Button7")).ForeColor = System.Drawing.Color.Red;
            beginshow();
        }
    }
    //读取数据库，显示接口信息。
    private void beginshow()
    {
        string sql = "  select   ";
        sql = sql + " FF.FF_JK_guid as 接口唯一标示,JK_host as 接口域名,JK.JK_path as 接口地址, JK.JK_shuoming as 接口说明,JK.JK_banben as 接口版本号,(CASE JK.JK_open WHEN 0 then '禁用' when 1 then '有效' END) as 接口是否有效, ";
        sql = sql + " FF.FF_guid as 方法唯一标示,FF.FF_yewuname as 业务名称,FF.FF_name as 方法名,FF.FF_retype as 返回值类型,FF.FF_canshu as 参数类型,   ";
        sql = sql + "  FF.FF_shuoming as 方法注释, (CASE FF.FF_open WHEN '1' THEN '有效'  WHEN '0' THEN '禁用'  ELSE '未知'  END) as 方法是否有效,   ";
        sql = sql + "   (CASE FF.FF_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as 方法目前状态, ";
        sql = sql + "  (CASE FF.FF_CorE WHEN '1' THEN '有插入或更新操作'  WHEN '0' THEN '仅查询操作'  ELSE '未知'  END) as 操作特点, FF.FF_log  as 日志设置,FF.FF_addtime as 添加时间,FF.FF_edittime as 最后一次修改时间 ";
        sql = sql + " from   AAA_ipcFF as FF left join AAA_ipcJK as JK on JK.JK_guid=FF.FF_JK_guid ";
        sql = sql + " where FF.FF_guid=@FF_guid";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsGXlist = new DataSet();
        Hashtable input = new Hashtable();
        input["@FF_guid"] = Request["FF_guid"].ToString();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "方法", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsGXlist = (DataSet)(return_ht["return_ds"]);
            showFF.InnerHtml = "<table border='0' cellpadding='5' cellspacing='1' bgcolor='#333333'>";
            for (int i = 0; i < dsGXlist.Tables["方法"].Columns.Count; i++)
            {

                showFF.InnerHtml = showFF.InnerHtml + "<tr><td bgcolor='#FFFFFF' style='width:130px'><strong>" + dsGXlist.Tables["方法"].Columns[i].ColumnName + "：</strong></td><td bgcolor='#FFFFFF' style='width:500px;line-height:20px'>" + dsGXlist.Tables["方法"].Rows[0][i].ToString() + "</td></tr>";

            }
            showFF.InnerHtml = showFF.InnerHtml + "</table> ";
        }
        else
        {
            //读取数据库出错。。。
            dsGXlist = null;

            Response.Write(return_ht["return_errmsg"].ToString());
        }

    }
}