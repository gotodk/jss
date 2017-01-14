using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FMOP.DB;
using System.Data.SqlClient;
using System.Text;

public partial class js_xcwpsq : System.Web.UI.Page
{
    private string wp="";   //物品的javascript串
    private string zq="";   //周期的javascript串

    protected void Page_Load(object sender, EventArgs e)
    {
        ProcessWP();
        
        StringBuilder html = new StringBuilder();
        html.AppendLine("function validate()");
        html.AppendLine("{");
        html.AppendLine("var displayStyle=document.all.meizzDateLayer.style.display;");
        html.AppendLine("if (displayStyle!=\"none\") return;");
        html.AppendLine("var wpmc=document.all.SCJHB_XCWPSQB_WPLBWPMC.value;");
        html.AppendLine("if (wpmc=='') return;");
        html.AppendLine("var qx=document.all.SCJHB_XCWPSQB_WPLBXQSJ.value");
        html.AppendLine("if (qx=='') return");
        html.AppendLine("var qxDate = new Date(qx.split(\"-\").join(\"/\"));");
        //html.AppendLine("alert(qxDate);");
        html.AppendLine("var wp=new Array(" + wp + ");");
        html.AppendLine("var zq=new Array(" + zq + ");");
        html.AppendLine("var index=-1;");
        html.AppendLine("for (var i=0;i<wp.length;i++)");
        html.AppendLine("{");
        html.AppendLine("if (wp[i]== wpmc)");
        html.AppendLine("{");
        html.AppendLine("index=i;");
        html.AppendLine("break;");
        html.AppendLine("}");
        html.AppendLine("}");
        html.AppendLine("if (index==-1) {");
        html.AppendLine("alert('未能找到所填写的宣传物品！');");
        html.AppendLine("document.all.SCJHB_XCWPSQB_WPLBWPMC.value=''");
        html.AppendLine("return;");
        html.AppendLine("}");
        html.AppendLine("var sxsj=zq[index]");
        html.AppendLine("var today=new Date();");
        html.AppendLine("var todayValue=today.getDate();");
        html.AppendLine("todayValue+=sxsj;");
        html.AppendLine("today.setDate(todayValue);");
       // html.AppendLine("alert(today);");
        html.AppendLine("if (today>qxDate){");
        html.AppendLine("alert('宣传物品的制作周期为'+sxsj+'天'+'在您填写的日期前无法完成，请重新填写需求时间');");
        html.AppendLine("document.all.SCJHB_XCWPSQB_WPLBXQSJ.value=''");
        html.AppendLine("return;");
        html.AppendLine("}");
        html.AppendLine("}");
        Response.Write(html.ToString());
    }

    /// <summary>
    /// 遍历数据库，获取物品和制作周期的javascript数组
    /// </summary>
    private void ProcessWP()
    {
        SqlDataReader dr = DbHelperSQL.ExecuteReader("select wpmc,zzsjt from scjhb_Xcwpsjzzsjb");
        while (dr.Read())
        {
            wp += "\"" + dr["wpmc"].ToString() + "\""+",";
            zq += dr["zzsjt"].ToString() + ",";
        }
        dr.Close();
        if (wp.Length > 0) wp = wp.Substring(0, wp.Length - 1);
        if (zq.Length > 0) zq = zq.Substring(0, zq.Length - 1);
    }
    
}
