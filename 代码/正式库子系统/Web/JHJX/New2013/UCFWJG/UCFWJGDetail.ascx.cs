using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;

public partial class Web_JHJX_New2013_UCFWJG_UCFWJGDetail : System.Web.UI.UserControl
{
   


    /// <summary>
    /// 获取选择的业管管理部门分类和业务管理部门信息
    /// </summary>
    [Description("获取选择的业管管理部门分类和业务管理部门信息"), Category("Appearance")]
    public string[] Value
    {
        get
        {
            List<string> listArray = new List<string>();
            listArray.Add(this.ddlYWGLBMFL.SelectedValue.ToString());//业管管理部门分类
            //业务管理部门信息
            if (this.ddlYWGLBM.Visible == true)
            {
                if (this.ddlYWGLBM.Text.Trim() == "请选择")
                {
                   listArray.Add("");
                }
                else
                {
                    listArray.Add(this.ddlYWGLBM.Text.Trim());
                }
            }
            else if (this.txtGXTW.Visible == true)
            {
                if (this.txtGXTW.Text.Trim() == "" || this.txtGXTW.Text.Trim().Equals("请选择"))
                {
                    listArray.Add("");
                }
                else
                {
                    listArray.Add(this.txtGXTW.Text.Trim());
                }
            }
            else
            {
                listArray.Add("");
            }
            return listArray.ToArray();
        }
        set {
            ddlYWGLBMFL.SelectedValue = value[0];
            ddlYWGLBMFL_SelectedIndexChanged(null, null);
            ddlYWGLBM.SelectedValue = value[1];
            this.txtGXTW.Text = value[1]; 
        }
    }
    private string[] lableName = new string[] {""};
    /// <summary>
    /// 设置控件各标签的前缀
    /// </summary>
    [Description("设置控件各标签的前缀"), Category("Appearance")]
    public string[] LableName
    {
        set {
            lableName = value;
            if (lableName.Length == 1)
            {
                this.spanYWGLBMFL.InnerText = value[0].ToString() + this.spanYWGLBMFL.InnerText;
            }
        }
    }



    private string GetBM(string name)
    {
        if (name != "")
        {
            string sql = "select bm from hr_employees where number='" + name.ToString() + "'";
            string bm = FMOP.DB.DbHelperSQL.GetSingle(sql).ToString();
            if (bm.IndexOf("办事处") > 0)
            {

                return bm;
            }

        }
        return "";
    }

    /// <summary>
    /// 初始化下拉框
    /// </summary>
    public void initdefault()
    {
        DataSet PublisDsData = new DataSet();
        //获取数据
        DataSet ds_FGS = DbHelperSQL.Query("select GLBMMC '名称',GLBMMC '账号' from AAA_PTGLJGB  where SFYX='是' and GLBMFLMC='分公司'");
        ds_FGS.Tables[0].TableName = "分公司";
        DataSet ds_YWTZB = DbHelperSQL.Query("select GLBMMC '名称',GLBMZH '账号' from AAA_PTGLJGB  where SFYX='是' and GLBMFLMC='平台总部'");
        ds_YWTZB.Tables[0].TableName = "平台总部";
        DataSet ds_GXTW = DbHelperSQL.Query("select GLBMMC '名称',GLBMZH '账号' from AAA_PTGLJGB  where SFYX='是' and GLBMFLMC='高校团委'");
        ds_GXTW.Tables[0].TableName = "高校团委";
        PublisDsData.Tables.Add(ds_FGS.Tables[0].Copy());
        PublisDsData.Tables.Add(ds_YWTZB.Tables[0].Copy());
        PublisDsData.Tables.Add(ds_GXTW.Tables[0].Copy());

        ViewState["分公司"] = PublisDsData.Tables["分公司"];
        ViewState["平台总部"] = PublisDsData.Tables["平台总部"];
        ViewState["高校团委"] = PublisDsData.Tables["高校团委"];

        ddlYWGLBMFL.Items.Clear();

        /*
        string url = ConfigurationManager.ConnectionStrings["GetBM"].ToString().Trim() + "/Web/GetBM/Handler.ashx?number=" + this.Parent.Page.User.Identity.Name + "&method=GetBM";       
        WebClient httpclient = new WebClient();
        //byte[] bytes = httpclient.DownloadData(url);
        httpclient.Encoding = Encoding.UTF8;
        string bm = httpclient.DownloadString(url);
        */
 
        string bm = GetBM(this.Parent.Page.User.Identity.Name);
        if (bm!=null&&bm!="")
        {
            ddlYWGLBMFL.Items.Add(new ListItem("分公司", "分公司"));
            ddlYWGLBMFL.Enabled = false;
            string sql = "select count(BSCname) from AAA_CityList_FGS where BSCname='" + bm + "'";
            string fgsSql = "select FGSname from AAA_CityList_FGS where BSCname='" + bm + "'";
            if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
            {
                string strFGSMC = DbHelperSQL.GetSingle(fgsSql).ToString();
                ddlYWGLBM.Items.Add(strFGSMC);
                ddlYWGLBM.SelectedIndex = 0;
                ddlYWGLBM.Enabled = false;
            }
            this.lblYWGLBM.Visible = true;
            this.ddlYWGLBM.Visible = true;
            this.lblGXTW.Visible = false;
            this.txtGXTW.Visible = false;
        }
        else
        {
            ddlYWGLBMFL.Items.Add(new ListItem("请选择业务管理部门分类", ""));
            ddlYWGLBMFL.Items.Add(new ListItem("分公司", "分公司"));
            ddlYWGLBMFL.Items.Add(new ListItem("平台总部", "平台总部"));
            ddlYWGLBMFL.Items.Add(new ListItem("高校团委", "高校团委"));
            this.lblYWGLBM.Visible = false;
            this.ddlYWGLBM.Visible = false;
            this.lblGXTW.Visible = false;
            this.txtGXTW.Visible = false;
        }
        ddlYWGLBMFL.SelectedIndex = 0;
    }




    /// <summary>
    /// 选择业管理部门分类后对应的处理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlYWGLBMFL_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((DataTable)ViewState["分公司"] == null || (DataTable)ViewState["平台总部"] == null || (DataTable)ViewState["高校团委"] == null)
        { return; }

     
        ddlYWGLBM.Items.Clear();
      

        if (ddlYWGLBMFL.SelectedValue.ToString() == "分公司")
        {
            this.lblYWGLBM.Visible = true;
            this.ddlYWGLBM.Visible = true;
            this.lblGXTW.Visible = false;
            this.txtGXTW.Visible = false;


            /*
            string url = ConfigurationManager.ConnectionStrings["GetBM"].ToString().Trim() + "/Web/GetBM/Handler.ashx?number=" + this.Parent.Page.User.Identity.Name + "&method=GetBM";
            WebClient httpclient = new WebClient();
            //byte[] bytes = httpclient.DownloadData(url);
            httpclient.Encoding = Encoding.UTF8;
            string bm = httpclient.DownloadString(url);
             */

 
            string bm = GetBM(this.Parent.Page.User.Identity.Name);
            if (bm != null && bm != "")
            {
                string sql = "select count(BSCname) from AAA_CityList_FGS where BSCname='" + bm + "'";
                string fgsSql = "select FGSname from AAA_CityList_FGS where BSCname='" + bm + "'";
                if (Convert.ToInt32(DbHelperSQL.GetSingle(sql)) > 0)
                {
                    string strFGSMC = DbHelperSQL.GetSingle(fgsSql).ToString();
                    ddlYWGLBM.Items.Add(strFGSMC);
                    ddlYWGLBM.Enabled = false;
                }
            }
            else
            {
                ddlYWGLBM.Items.Add(new ListItem("请选择",""));
                foreach (DataRow dr in ((DataTable)ViewState["分公司"]).Rows)
                {
                    ddlYWGLBM.Items.Add(new ListItem(dr["名称"].ToString(), dr["名称"].ToString()));
                }
            }

        }
        else if (ddlYWGLBMFL.SelectedValue.ToString() == "平台总部")
        {
            this.lblYWGLBM.Visible = true;
            this.ddlYWGLBM.Visible = true;
            this.lblGXTW.Visible = false;
            this.txtGXTW.Visible = false;
            foreach (DataRow dr in ((DataTable)ViewState["平台总部"]).Rows)
            {
                ddlYWGLBM.Items.Add(new ListItem(dr["名称"].ToString(), dr["名称"].ToString()));
            }

        }
        else if (ddlYWGLBMFL.SelectedValue.ToString() == "高校团委")
        {
            this.lblYWGLBM.Visible = false;
            this.ddlYWGLBM.Visible = false;
            this.txtGXTW.Text = "请选择";
            this.lblGXTW.Visible = true;
            this.txtGXTW.Visible = true;
        }
        else
        {
            this.lblYWGLBM.Visible = false;
            this.ddlYWGLBM.Visible = false;
            this.lblGXTW.Visible = false;
            this.txtGXTW.Visible = false;
        }
    }
   
}