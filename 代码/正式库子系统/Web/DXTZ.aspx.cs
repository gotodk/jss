using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
using System.IO;
using FMOP.DB;
using ShareLiu;
using ShareLiu.SwClasses;
using ShareLiu.DXS;
public partial class Web_DXTZ : System.Web.UI.Page
{
    ArrayList al = null;
    SendDX sendPhone = null;
    string tablename = "";
    string number = "";
    string bt_module = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        bt_module = Request["module"].ToString();
        number = Request["number"].ToString();
        switch (bt_module)
        {
            case "OnLineOrder":
                this.bt.Text = "【网站产品需求单】短信通知";
                tablename = "OnLineOrder";
                break;
            case "Web_WLZZFW_ZXZXFHY":
                this.bt.Text = "【在线咨询】短信通知";
                tablename = "Web_WLZZFW_ZXZXFHY";
                break;
            case "Web_WLZZFW_ZXTSFHY":
                this.bt.Text = "【在线投诉】短信通知";
                tablename = "Web_WLZZFW_ZXTSFHY";
                break;
            case "Web_WLZZFW_ZXJY":
                this.bt.Text = "【在线建议】短信通知";
                
                tablename = "Web_WLZZFW_ZXJY";
                break;
            case "Web_HBHS_FHY":
                this.bt.Text = "【环保回收】短信通知";
              
                tablename = "Web_HBHS_FHY";

                break;
            case "Web_GZBX_FHY":
                this.bt.Text = "【故障报修】短信通知";              
                tablename = "Web_GZBX_FHY";
                break;
                //直通车意见与建议反馈
            case "FWPT_YJYJJFK":
                this.bt.Text = "【直通车意见与建议反馈】短信通知";
                tablename = "FWPT_YJYJJFK";
                break;
            case "FM_YJYJY":
                this.bt.Text = "【用户直通车意见与建议反馈】短信通知";
                tablename = "FM_YJYJY";
                break;
            case "FM_YHJFJXG":
                this.bt.Text = "【用户直通车用户交付旧硒鼓】短信通知";
                tablename = "FM_YHJFJXG";
                break;
            case "FM_GZBX":
                this.bt.Text = "【用户直通车用户故障报修表】短信通知";
                tablename = "FM_GZBX";
                break;
            case "FM_XHDPJYQS":
                this.bt.Text = "【用户直通车用户销货单评价与签收】短信通知";
                tablename = "FM_XHDPJYQS";
                break;
            default:
                this.bt.Text = "";              
                tablename = "";
                break;
        }
        if (ViewState["chooseP"] == null)
        {
            ViewState["chooseP"] = GetDataset(ViewState["chooseP"] as DataSet);
        }
        if (!Page.IsPostBack)
        {
            getdept();

        }
    }
    private void getdept()
    {
        //InintPage();
        string sql;
        sql = "select distinct BM from HR_Employees where YGZT<>'离职' group by BM having count(*)>0 order by BM ASC";
        DataSet ds = DbHelperSQL.Query(sql);
        Ddldeptemp.Items.Add("--请选择--");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {

            Ddldeptemp.Items.Add(dr["BM"].ToString());

        }

    }
    //private void InintPage()
    //{
    //    this.CreateUserID.Text = User.Identity.Name.ToString();
    //    this.CreateUserName.Text = Users.GetUserByNumber(User.Identity.Name).Name.ToString();
    //    this.txtAppTime.Text = DateTime.Now.ToString();
    //    this.txtAppDepart.Text = Users.GetUserByNumber(User.Identity.Name).DeptName.ToString();
    //}
    private void getGWEMP(string depts)
    {
        string sql;
        sql = "select distinct GWMC from HR_Employees where BM='" + depts + "'and YGZT<>'离职' group by GWMC having count(*)>0";
        DataSet ds = DbHelperSQL.Query(sql);
        Ddlgwemp.Items.Add("--请选择--");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Ddlgwemp.Items.Add(dr["GWMC"].ToString());
        }

    }



    protected void Ddldeptemp_SelectedIndexChanged(object sender, EventArgs e)
    {
        Ddlgwemp.Items.Clear();
        getGWEMP(this.Ddldeptemp.SelectedItem.Text.Trim());

        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>SetCookie('showDocM','show');</script>");
    }
    protected void Ddlgwemp_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListBox1.Items.Clear();
        string gwname = this.Ddlgwemp.SelectedItem.Text.Trim();
        string sql;
        sql = "select Number,Employee_name from HR_Employees where GWMC='" + gwname + "' and YGZT<>'离职'";
        DataSet ds = DbHelperSQL.Query(sql);
        ListItem lt = null;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            lt = new ListItem();
            lt.Value = dr["Number"].ToString();
            lt.Text = dr["Employee_name"].ToString();
            ListBox1.Items.Add(lt);//dr["Number"].ToString() + "/" + dr["Employee_name"].ToString()

        }

        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>SetCookie('showDocM','show');</script>");

    }
    protected void btnadditem_Click(object sender, EventArgs e)
    {

        string Strsql = "";
        if (ListBox1.Items.Count < 1)
        {
            MessageBox.Show(this, "没有人员");
            return;
        }
        if (ListBox1.SelectedIndex < 0)
        {
            MessageBox.Show(this, "没有人员选择");
            return;
        }
        DataRow dr = null;
        DataTable dt = ViewState["chooseP"] as DataTable;
        foreach (ListItem lt in ListBox1.Items)
        {
            if (lt.Selected)
            {
                
                if (!dt.Rows.Contains(lt.Value))
                {
                    dr = dt.NewRow();
                    dr["工号"] = lt.Value;
                    dr["姓名"] = lt.Text;
                    Strsql = "select GRDH  from  dbo.HR_Employees where Number = '"+lt.Value+"'";
                    SqlDataReader sdr = DbHelperSQL.ExecuteReader(Strsql);
                    if (sdr.Read())
                    {
                        dr["手机号"] = sdr["GRDH"].ToString();
                    }
                    else
                    {
                        dr["手机号"] = "无";
                    }
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    
                }
            }
        }

        ViewState["chooseP"] = dt;

        BindRightP(ViewState["chooseP"] as DataTable);
        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>SetCookie('showDocM','show');</script>");
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        if (this.dlist.Items.Count < 1)
        {

            return;

        }

        DataTable dt = ViewState["chooseP"] as DataTable;
        al = new ArrayList();
        foreach (DataListItem dtl in dlist.Items)
        {
            CheckBox cb = dtl.FindControl("cbchosse") as CheckBox;
            string strGh = "";
            if (cb.Checked)
            {
                strGh = (dtl.FindControl("labGonghai") as Label).Text;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["工号"].ToString().Trim() == strGh.Trim())
                    {
                        al.Add(dr);
                    }
                }

            }
        }

        for (int i = 0; i < al.Count; i++)
        {
            dt.Rows.Remove(al[i] as DataRow);
        }

        dt.AcceptChanges();

        ViewState["chooseP"] = dt;
        BindRightP(dt);
        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>SetCookie('showDocM','show');</script>");
    }
    protected void btnclearall_Click(object sender, EventArgs e)
    {
        // ListBox2.Items.Clear();
        DataTable dt = ViewState["chooseP"] as DataTable;
        dt.Clear();
        ViewState["chooseP"] = dt;
        BindRightP(dt);
        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>SetCookie('showDocM','show');</script>");
    }
    private DataTable GetDataset(DataSet ds)
    {
        if (ds == null)
        {
            string Strsql = "select Number as 工号,Employee_Name as 姓名 , GRDH  as 手机号    from  dbo.HR_Employees where Number = '根本不存在'";
            ds = DbHelperSQL.Query(Strsql);
        }

        //设定主键
        ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns["工号"] };
        return ds.Tables[0];
    }

    private void BindRightP(DataTable dt)
    {


        this.dlist.DataSource = dt;
        this.dlist.DataBind();
        CheckBox CB = null;
        DataSet dsa = null;
        string strWNumber = "";
        foreach (DataListItem dtl in dlist.Items)
        {
            //CB = dtl.FindControl("cbMesWroning") as CheckBox;

            strWNumber = (dtl.FindControl("labGonghai") as Label).Text.Trim();
            string StrISexsit = " select * from YWPTGRPZ where GH='" + strWNumber + "'";

            dsa = DbHelperSQL.Query(StrISexsit);
            //if (dsa != null && dsa.Tables[0].Rows.Count == 1)
            //{
            //    if (dsa.Tables[0].Rows[0]["SFJSDX"].ToString().Trim() == "接受")
            //    {

            //        CB.Checked = true;
            //    }
            //}

        }

    }
    protected void Btnsave_Click(object sender, EventArgs e)
    {

        if (islegal())
        {
            SaveAttributes();
            //FMOP.Common.MessageBox.Show(Page, "发送成功！");
            //ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>SetCookie('showDocM','no');</script>");
            Response.Write("<Script language ='javaScript'>alert('发送完成!');window.location.href='WorkFlow_Check.aspx?module=" + bt_module + "&number=" + number + " ';</Script>");
            Response.End();
        }
    }
    private bool islegal()
    {
        bool islegal = true;
        if (dlist != null && dlist.Items.Count <= 0)
        {
            FMOP.Common.MessageBox.Show(Page, "没有选择短信接收人！");
            islegal = false;
            return islegal;
        }
        if (this.txtDXNR.Text == "")
        {
            FMOP.Common.MessageBox.Show(Page, "短信内容不能为空！");
            islegal = false;
            return islegal;
        }
        return islegal;

    }
    private void SaveAttributes()
    {
        try
        {
            string strWNumber = "";
            string StrWName = "";
            string StrSJ= "";
            string sql = "";
            string dxjsr_all="";
            string dxjsr="";
            if (dlist != null && dlist.Items.Count <= 0)
            {
                MessageBox.Show(this, "没有选择短信接收人！");
                return;
            }

            for (int i = 0; i < dlist.Items.Count; i++)
            {

                DataListItem dli = dlist.Items[i];
                strWNumber = (dli.FindControl("labGonghai") as Label).Text.Trim();

                StrWName = (dli.FindControl("labxingming") as Label).Text.Trim();
                StrSJ = (dli.FindControl("labsj") as Label).Text.Trim();
                dxjsr = dxjsr_all+StrWName;
                dxjsr_all=dxjsr+",";
                
                //***********************************短信测试*********************************************************************************************

                DateTime dt = DateTime.Now;
                User usinfo = Users.GetUserByNumber(User.Identity.Name.Trim());
                string StrMess = this.txtDXNR.Text.ToString();
                sendPhone = new SendDX();

                sendPhone.SendToP((dlist.Items[i].FindControl("labGonghai") as Label).Text.Trim(), StrMess);
           
                //*******************************************************************************************************************************************
                
            }
            dxjsr_all=dxjsr_all.Substring(0,dxjsr_all.Length-1);
            ArrayList al = new ArrayList();
            string Sql = "update  " + tablename + "  set  SFYFSDX='是',DXFSSJ='" + DateTime.Now.ToString() + "',DXJSR='" + dxjsr_all + "',DXFSR='" + User.Identity.Name.Trim() + "',DXNR='" + this.txtDXNR.Text.ToString() + "' where Number='" + number + "' ";
            al.Add(Sql);

            DbHelperSQL.ExecuteSqlTran(al);

        }
        catch (Exception ex)
        {
            FMOP.Common.MessageBox.Show(Page, "系统出错，请及时联系管理员！");
            return;
        }
    }
    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void BtnQX_Click(object sender, EventArgs e)
    {
        Response.Write("<Script language ='javaScript'>window.location.href='WorkFlow_Check.aspx?module=" + bt_module + "&number=" + number + " ';</Script>");
        Response.End();
    }
}
