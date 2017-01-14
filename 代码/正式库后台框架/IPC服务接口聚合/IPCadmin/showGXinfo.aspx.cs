using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IPCadmin_showGXinfo : System.Web.UI.Page
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
        sql = sql + " JK.JK_host as 接口域名,JK.JK_path as 接口地址,JK.JK_shuoming as 接口说明,JK.JK_banben as 接口版本号,(CASE JK.JK_open WHEN '1' THEN '有效'  WHEN '0' THEN '禁用'  ELSE '未知'  END) as 接口是否有效,(CASE JK.JK_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as 接口目前状态, JK.JK_addtime as 接口添加时间, JK.JK_edittime as 接口最后一次修改时间,JK.JK_port as 接口备用端口, ";
        sql = sql + " FF.FF_yewuname as 方法业务名称,FF.FF_name as 方法名,FF.FF_retype as 方法返回值类型,FF.FF_canshu as 方法参数类型,FF.FF_shuoming  as 方法注释,(CASE FF.FF_open WHEN '1' THEN '有效'  WHEN '0' THEN '禁用'  ELSE '未知'  END) as 方法是否有效,(CASE FF.FF_zt WHEN '1' THEN '活着'  WHEN '0' THEN '挂了'  ELSE '未知'  END) as 方法目前状态,(CASE FF.FF_CorE WHEN '1' THEN '有插入或更新操作'  WHEN '0' THEN '仅查询操作'  ELSE '未知'  END) as 方法操作特点, ";
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
            gxinfo.InnerHtml = "<table border='0' cellpadding='5' cellspacing='1' bgcolor='#333333'>";
            for (int i = 0; i < dsGXlist.Tables["关系"].Columns.Count; i++)
            {

                gxinfo.InnerHtml = gxinfo.InnerHtml + "<tr><td bgcolor='#FFFFFF'><strong>" + dsGXlist.Tables["关系"].Columns[i].ColumnName + "：</strong></td><td bgcolor='#FFFFFF'>" + dsGXlist.Tables["关系"].Rows[0][i].ToString() + "</td></tr>";
             
            }
            gxinfo.InnerHtml = gxinfo.InnerHtml + "</table> ";
        }
        else
        {
            //读取数据库出错。。。
            dsGXlist = null;

            Response.Write(return_ht["return_errmsg"].ToString());
        }

    }
}