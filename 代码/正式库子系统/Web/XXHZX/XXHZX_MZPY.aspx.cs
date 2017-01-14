using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FMOP.DB;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;

public partial class Web_XXHZX_XXHZX_MZPY : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (IsPostBack)
            return ;
        //验证权限
        DefinedModule module = new DefinedModule("XXHZX_MZPY");
        Authentication auth = module.authentication;
        if (auth == null)
        {
            MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
        }
        UserModuleAuth moduleAuth = auth.GetAuthByUserNumber(User.Identity.Name);
        if (!moduleAuth.CanModify)
        {
            MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
        }
        string username = User.Identity.Name;
        DataSet ds = DbHelperSQL.Query("select * from AAAAAAAAAAAAAAAAA where 评分人姓名='" + username + "'");
        ViewState["pingfen"] = ds;
        GridView1.DataSource = ds.Tables[0].DefaultView;
        GridView1.DataBind();
    }
    protected void button_tj(object sender, EventArgs e)
    {
        DataSet dstemp = (DataSet)ViewState["pingfen"];
        //更新数据表
        for (int i = 0; i < dstemp.Tables[0].Rows.Count; i++)
        { 
            TextBox jhzz_temp=(TextBox )GridView1.Rows[i].Cells [2].Controls [1];
            string jhzz=jhzz_temp.Text ;
            TextBox fxwt_temp = (TextBox)GridView1.Rows[i].Cells[3].Controls[1];
            string fxwt=fxwt_temp.Text ;
            TextBox zyzs_temp = (TextBox)GridView1.Rows[i].Cells[4].Controls[1];
            string zyzs=zyzs_temp.Text ;
            TextBox cxnl_temp = (TextBox)GridView1.Rows[i].Cells[5].Controls[1];
            string cxnl=cxnl_temp.Text ;
            TextBox xxnl_temp = (TextBox)GridView1.Rows[i].Cells[6].Controls[1];
            string xxnl=xxnl_temp.Text ;
            TextBox gtnl_temp = (TextBox)GridView1.Rows[i].Cells[7].Controls[1];
            string gtnl=gtnl_temp.Text ;
            TextBox drzf_temp = (TextBox)GridView1.Rows[i].Cells[8].Controls[1];
            string drzf=drzf_temp.Text ;
            string bfrxm=dstemp .Tables [0].Rows [i]["被评人姓名"].ToString ();
            string pfrxm=dstemp .Tables [0].Rows [i]["评分人姓名"].ToString ();
            string sql = "update AAAAAAAAAAAAAAAAA set 计划组织与协调能力='" + jhzz + "',发现问题和解决问题能力='" + fxwt + "',专业知识和技能='" + zyzs + "',创新能力='" + cxnl + "',学习能力='" + xxnl + "',沟通能力='" + gtnl + "',单人总分='" + drzf + "' where 被评人姓名='" + bfrxm + "' and 评分人姓名='" + pfrxm + "'";
            DbHelperSQL.ExecuteSql(sql);
        }
        Response.Write("<Script language ='javaScript'>alert('保存成功！');history.back();</Script>");
        Response.End();
    }
}
