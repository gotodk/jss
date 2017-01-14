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
using System.Data.SqlClient;
using System.Text;
using FMOP.DB;
using Hesion.Brick.Core;
using Hesion.Brick.Core.WorkFlow;
using System.Collections.Generic;
public partial class Web_dr_employee : System.Web.UI.Page
{
    public static string connectionString = ConfigurationManager.ConnectionStrings["FMOPConn"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            DefinedModule module = new DefinedModule("dr_employee");
            Authentication auth = module.authentication;
            if (auth == null)
            {
              
                Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('您无权进行导入操作！');window.top.frames['rightFrame'].my_closediag();</script>");
                Response.End();
            }
            if (!auth.GetAuthByUserNumber(User.Identity.Name).CanView)
            {
             
                Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('您无权进行导入操作！');window.top.frames['rightFrame'].my_closediag();</script>");
                Response.End();
            }
            //根据“是否导入员工档案”字段进行判断
            //string str_pd = "select XM,LS,CreateUser,sfdrygda from temp_employee where Number='" + Request["number"] + "'";
            //2009-12-8修改，添加身份证号重复验证，防止人员基本信息表中新建而员工档案中存在的信息导入到员工档案中
            string str_pd = "select XM,LS,CreateUser,sfdrygda,sfzh from temp_employee where Number='" + Request["number"] + "'";
            string sfdrygda = "";
            DataSet ds_pd = FMOP.DB.DbHelperSQL.Query(str_pd);
            string sfzh = "";
            if (ds_pd.Tables[0].Rows.Count > 0)
            {
                sfdrygda = ds_pd.Tables[0].Rows[0]["sfdrygda"].ToString();
                sfzh = ds_pd.Tables[0].Rows[0]["sfzh"].ToString();
                if (sfdrygda == "是")
                {
                    Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('该员工已导入员工档案！');window.top.frames['rightFrame'].my_closediag();</script>");
                    Response.End();

                }
                else
                {
                    string str_sfzh = "select * from hr_employees where Employee_IDCardNo ='" + sfzh + "'";
                    DataSet sfzh_Set = FMOP.DB.DbHelperSQL.Query(str_sfzh);
                    int sfzh_Count = sfzh_Set.Tables[0].Rows.Count;
                    if (sfzh_Count > 0)
                    {
                        Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('该员工在员工档案中已经存在！');window.top.frames['rightFrame'].my_closediag();</script>");
                        Response.End();
                    }
                }
            }
        }
       
    }

    protected void btn_dr_Click(object sender, EventArgs e)
    {
        try
        {

        if (ygbh_ok.Text != "确定")
        {
            FMOP.Common.MessageBox.Show(this, "必须在输入框中输入\"确定\"两个汉字后,方可导入档案！");
            return;
        }


        module_txt.Text = Request["module"].ToString();
        number_txt.Text = Request["number"].ToString();
        string ygbh = this.ygbh.Text.ToString();
        string strmodule = module_txt.Text.ToString();
        string strnumber = number_txt.Text.ToString();
        string XM = "";
        string LS = "";
        string CreateUser = "";
        //string sfdrygda = "";
        string sql_number = "select * from hr_employees where number ='" + ygbh + "'";
        if (ygbh != "" && ygbh != null)
        {
            DataSet num_Set = FMOP.DB.DbHelperSQL.Query(sql_number);
            int num_Count = num_Set.Tables[0].Rows.Count;
            if (num_Count > 0)
            {
                FMOP.Common.MessageBox.Show(this, "该员工在员工档案表中已存在！");
                return;
            }
            else
            {
                string str_sql = "select XM,LS,CreateUser from temp_employee where Number='" + strnumber + "'";
                DataSet ds_LS = FMOP.DB.DbHelperSQL.Query(str_sql);

                if (ds_LS.Tables[0].Rows.Count > 0)
                {

                    XM = ds_LS.Tables[0].Rows[0]["XM"].ToString();
                    LS = ds_LS.Tables[0].Rows[0]["LS"].ToString();
                    CreateUser = ds_LS.Tables[0].Rows[0]["CreateUser"].ToString();
                    //sfdrygda = ds_LS.Tables[0].Rows[0]["sfdrygda"].ToString();
                    if (LS != "" && LS != null)
                    {

                        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf1 = new Hesion.Brick.Core.WorkFlow.WorkFlowModule("YGGXGL_JYJLB");
                        string number_jyjl = wf1.numberFormat.GetNextNumber();
                        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf2 = new Hesion.Brick.Core.WorkFlow.WorkFlowModule("YGGXGL_GZJLB");
                        string number_gzjl = wf2.numberFormat.GetNextNumber();
                        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf3 = new Hesion.Brick.Core.WorkFlow.WorkFlowModule("YGGXGL_PXJLB");
                        string number_pxjl = wf3.numberFormat.GetNextNumber();
                        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf4 = new Hesion.Brick.Core.WorkFlow.WorkFlowModule("YGGXGL_JTQKB");
                        string number_jtqk = wf4.numberFormat.GetNextNumber();

                        //调用存储过程dr_employee将数据导入员工档案
                        SqlConnection cnn = new SqlConnection(connectionString);
                        cnn.Open();
                        SqlCommand cmd = new SqlCommand("dr_employee", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Number", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@YGBH", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@XM", SqlDbType.VarChar, 10);
                        cmd.Parameters.Add("@CreateUser", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@number_jyjl", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@number_gzjl", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@number_pxjl", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@number_jtqk", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@count_zn", SqlDbType.Int);
                        cmd.Parameters.Add("@count_jy", SqlDbType.Int);
                        cmd.Parameters.Add("@count_gz", SqlDbType.Int);
                        cmd.Parameters.Add("@count_px", SqlDbType.Int);
                        cmd.Parameters.Add("@count_jt", SqlDbType.Int);

                        cmd.Parameters[0].Value = strnumber;
                        cmd.Parameters[1].Value = ygbh;
                        cmd.Parameters[2].Value = XM;
                        cmd.Parameters[3].Value = CreateUser;
                        cmd.Parameters[4].Value = number_jyjl;
                        cmd.Parameters[5].Value = number_gzjl;
                        cmd.Parameters[6].Value = number_pxjl;
                        cmd.Parameters[7].Value = number_jtqk;
                        cmd.Parameters[8].Value = 0;
                        cmd.Parameters[9].Value = 0;
                        cmd.Parameters[10].Value = 0;
                        cmd.Parameters[11].Value = 0;
                        cmd.Parameters[12].Value = 0;

                        cmd.ExecuteNonQuery();

                        //调用创建用户存储过程，在aspnet_membership等表中插入数据
                        SqlCommand memCmd = new SqlCommand("SP_CreateUser", cnn);
                        memCmd.CommandType = CommandType.StoredProcedure;
                        memCmd.Parameters.Add("@Number", SqlDbType.NVarChar, 4000);
                        memCmd.Parameters[0].Value = ygbh;
                        memCmd.ExecuteNonQuery();

                        Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('导入成功！');window.top.frames['rightFrame'].my_closediag();window.top.frames['rightFrame'].location.reload();</script>");
                        Response.End();
                    }
                    else
                    {
                        Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('导入员工档案时部门不能为空！');window.top.frames['rightFrame'].my_closediag();window.top.frames['rightFrame'].location.reload();</script>");
                        Response.End();
                    }
                }
                else
                {
                    Page.RegisterStartupScript("", "<script language='javascript'>window.alert('请从人员基本信息登记表中进行导入操作！');</script>");
                }
            }
        }
        else
        {
            Page.RegisterStartupScript("", "<script language='javascript'>window.alert('请先生成员工编号！');</script>");
        }



}

        catch (Exception ex)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
            ErrorLog("错误\r" + ex.ToString(), DateTime.Now.ToString("yyyyMMddhhmmss") + new Random().Next(0x3e8, 0x270f) + ".txt", HttpContext.Current.Request.ApplicationPath + "/Web/ErrorLog/");
        }
       
    }


    public static void ErrorLog(string errorMsg, string fileName, string savePath)
    {
        string str = "未获取到来源页面";
        HttpContext current = HttpContext.Current;
        if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] != null)
        {
            str = current.Request.ServerVariables["HTTP_REFERER"].ToString();
        }
        string path = current.Server.MapPath(savePath);
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
        System.IO.StreamWriter writer = new System.IO.StreamWriter(path + fileName, true, Encoding.UTF8);
        StringBuilder builder = new StringBuilder();
        builder.Append("日志时间：\t" + DateTime.Now.ToString("yyyy-MM-dd hh:mm ss"));
        builder.Append("\r\n\r\n");
        builder.Append("请求来源：\t" + str);
        builder.Append("\r\n\r\n");
        //builder.Append("IP地址：\t" + GetClientIP());
        builder.Append("\r\n\r\n");
        builder.Append("错误页面：\t" + current.Request.Url.ToString());
        builder.Append("\r\n\r\n");
        builder.Append("日志内容：");
        builder.Append("\r\n\r\n");
        builder.Append(errorMsg);
        writer.Write(builder);
        writer.Flush();
        writer.Close();
    }

    protected void btn_sc_Click1(object sender, EventArgs e)
    {
        this.ygbh.Text=UserGH.getNewGH();
    }
}
