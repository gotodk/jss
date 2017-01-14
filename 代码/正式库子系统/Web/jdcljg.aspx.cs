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
using System.Data.SqlClient;
using FMOP.DB;

public partial class Web_jdcljg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        


        modPD1_showall.InnerHtml = "<iframe id=\"i0\" name=\"i0\" src=\"/Web/WorkFlow_View_Detail.aspx?module=" + Request["module"].ToString() + "&number=" + Request["number"].ToString() + "&from=view\" width=\"90%\" scrolling=\"auto\" border=\"0\" frameborder=\"0\">浏览器不支持嵌入式框架</iframe>";
        module_txt.Text = Request["module"].ToString();
        number_txt.Text = Request["number"].ToString();

        //获取原始投诉信息
        string sqldsysxx = "select * from TJJDXX where number = '" + number_txt.Text + "'";
        DataSet dsysxx = DbHelperSQL.Query(sqldsysxx);
        if(dsysxx.Tables[0].Rows[0]["SFNM"].ToString() == "我要匿名")
        {
            tijiaorenxinxin.InnerHtml = "提交人使用匿名方式提交了一个监督信息。";
        }
        else
        {
            string sql3 = "select * from HR_Employees where Number='" + dsysxx.Tables[0].Rows[0]["CreateUser"].ToString() + "'";
            SqlDataReader sdr = DbHelperSQL.ExecuteReader(sql3);
            string Employee_Name = "";
            string BM = "";
            string GWMC = "";
            while (sdr.Read())
            {
                Employee_Name = sdr["Employee_Name"].ToString();
                BM = sdr["BM"].ToString();
                GWMC = sdr["GWMC"].ToString();
            }
            sdr.Close();

            tijiaorenxinxin.InnerHtml = "提交人姓名: " + Employee_Name + " 提交人部门: " + BM + " 提交人岗位: " + GWMC;
        }


        //验证是否可反馈
        string JDDX_temp = dsysxx.Tables[0].Rows[0]["JDDX"].ToString(); // 被投诉对象
        string CreateUser_temp = dsysxx.Tables[0].Rows[0]["CreateUser"].ToString();  //提交投诉的人
        string nowuser_temp = User.Identity.Name;  //当前进行浏览的人

        JDDX_temp_txt.Text = JDDX_temp;// 被投诉对象
        CreateUser_temp_txt.Text = CreateUser_temp;//提交投诉的人
        nowuser_temp_txt.Text = nowuser_temp;//当前进行浏览的人

        if(CreateUser_temp == nowuser_temp)
        {
            Dtijiaofk.InnerHtml = "";
        }
        else
        {
            ;
        }


        //判断投诉是否已经提交过
        string sql = "select * from JDXXFK where MKMC = '" + module_txt.Text + "' and DJBH = '" + number_txt.Text + "'";
        DataSet dsfk = DbHelperSQL.Query(sql);
        if (dsfk != null && dsfk.Tables.Count > 0 && dsfk.Tables[0].Rows.Count > 0)
        {
            string html_fk = "";
            for (int i = 0; i < dsfk.Tables[0].Rows.Count; i++)
            {


                string sql3 = "select * from HR_Employees where Number='" +
                              dsfk.Tables[0].Rows[i]["CreateUser"].ToString() + "'";
                SqlDataReader sdr = DbHelperSQL.ExecuteReader(sql3);
                string Employee_Name = "";
                string BM = "";
                string GWMC = "";
                while (sdr.Read())
                {
                    Employee_Name = sdr["Employee_Name"].ToString();
                    BM = sdr["BM"].ToString();
                    GWMC = sdr["GWMC"].ToString();
                }
                sdr.Close();
                //反馈内容
                html_fk = html_fk + "--> 由" + BM + "-" + GWMC + "-" + Employee_Name + "在" +
                                       dsfk.Tables[0].Rows[i]["CreateTime"].ToString() + "进行反馈如下:<br />" +
                                       dsfk.Tables[0].Rows[i]["FKJG"].ToString() + "<br /><br />";
            }
            fankuiinfo.InnerHtml = html_fk;
        }
        else
        {
            fankuiinfo.InnerHtml = "尚未进行反馈。";
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //判断投诉是否已经提交过
        //string sql = "select * from JDXXFK where MKMC = '" + module_txt.Text + "' and DJBH = '" + number_txt.Text + "'";
        //DataSet dsfk = DbHelperSQL.Query(sql);
        //if (dsfk != null && dsfk.Tables.Count > 0 && dsfk.Tables[0].Rows.Count > 0) 
        //{
            
        //    DbHelperSQL.ExecuteSql("update JDXXFK set CreateUser = '" + User.Identity.Name + "',FKJG = '" + tb_fankui.Text + "',CreateTime = '"+System.DateTime.Now.ToString()+"' where MKMC = '" + module_txt.Text + "' and DJBH = '" + number_txt.Text + "'");
        //}
        //else
        //{
        //    DbHelperSQL.ExecuteSql("insert into JDXXFK(MKMC, DJBH, FKJG, CreateUser)values ('" + module_txt.Text + "', '" + number_txt.Text + "' , '" + tb_fankui.Text + "', '" + User.Identity.Name + "');");
        //}

        int pp = DbHelperSQL.ExecuteSql("insert into JDXXFK(MKMC, DJBH, FKJG, CreateUser)values ('" + module_txt.Text + "', '" + number_txt.Text + "' , '" + tb_fankui.Text + "', '" + User.Identity.Name + "');");
        if(pp > 0)
        {
            string JDDX_temp1 = JDDX_temp_txt.Text;// 被投诉对象
            string CreateUser_temp1 = CreateUser_temp_txt.Text;//提交投诉的人
            string nowuser_temp1 = nowuser_temp_txt.Text;//当前进行浏览的人

            Hesion.Brick.Core.WorkFlow.WorkFlowModule wf = new Hesion.Brick.Core.WorkFlow.WorkFlowModule("TJJDXX");//分配模块
            wf.authentication.InsertWarnings("你提供监督信息得到了新的反馈!", "WorkFlow_View.aspx?module=TJJDXX", "1", nowuser_temp1, CreateUser_temp1);

        }
        //Response.Write("<script>alert('成功提交处理结果！');window.location.href='" + Request.RawUrl + "';</script>");
        Response.Redirect(Request.RawUrl);
        
        }

        
    
}
