using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using FMOP.DB;
using System.Net.Mail;
using System.Net;
using System.ComponentModel;
using Mailer.Components;
using System.IO;
using Hesion.Brick.Core.WorkFlow;
using Hesion.Brick.Core;
public partial class myremail : System.Web.UI.Page
{
    string tablename = "";
    string number = "";
    string bt_module = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write("模块名称："+Request["module"].ToString() + "<br/>单号:" + Request["number"].ToString());
        modPD1_showall.InnerHtml = "<iframe id=\"i0\" name=\"i0\" src=\"/Web/WorkFlow_View_Detail.aspx?module=" + Request["module"].ToString() + "&number=" + Request["number"].ToString() + "&from=view\" width=\"90%\" height=\"300px\" scrolling=\"auto\" border=\"0\" frameborder=\"0\">浏览器不支持嵌入式框架</iframe>";
        bt_module = Request["module"].ToString();
        number = Request["number"].ToString();
        string email_cs = "";
        switch (bt_module)
        {
            case "OnLineOrder":
                this.bt.Text = "【网站产品需求单】";
                email_cs = "DZYJ";
                tablename = "OnLineOrder";
                this.yjbt.Text = "您在富美网站的在线订单已经受理";
                break;
            case "Web_WLZZFW_ZXZXFHY":
                this.bt.Text = "【在线咨询】";
                email_cs = "EMAIL";
                tablename = "Web_WLZZFW_ZXZXFHY";
                this.yjbt.Text = "您在富美网站的在线咨询已经受理";
                break;
            case "Web_WLZZFW_ZXTSFHY":
                this.bt.Text = "【在线投诉】";
                email_cs = "EMAIL";
                tablename = "Web_WLZZFW_ZXTSFHY";
                this.yjbt.Text = "您在富美网站的在线投诉已经受理";
                break;
            case "Web_WLZZFW_ZXJY":
                this.bt.Text = "【在线建议】";
                email_cs = "EMAIL";
                tablename = "Web_WLZZFW_ZXJY";
                this.yjbt.Text = "您在富美网站的在线建议已经受理";
                break;
            case "Web_HBHS_FHY":
                this.bt.Text = "【环保回收】";
                email_cs = "EMAIL";
                tablename = "Web_HBHS_FHY";
                this.yjbt.Text = "您在富美网站的环保回收已经受理";
                break;
            case "Web_GZBX_FHY":
                this.bt.Text = "【故障报修】";
                email_cs = "EMAIL";
                tablename = "Web_GZBX_FHY";
                this.yjbt.Text = "您在富美网站的故障报修已经受理";
                break;
            case "FWPT_YJYJJFK":
                this.bt.Text = "【意见与建议反馈】";
                email_cs = "DLYX";
                tablename = "FWPT_YJYJJFK";
                this.yjbt.Text = "您在富美直通车的意见与建议反馈已受理";
                break;
            case "FM_YJYJY":
                this.bt.Text = "【意见与建议反馈】";
                email_cs = "DLYX";
                tablename = "FM_YJYJY";
                this.yjbt.Text = "您在用户直通车的意见与建议反馈已受理";
                break;
            case "FWPT_XHDYHFK":
                this.bt.Text = "【销货单用户反馈】";
                email_cs = "DLYX";
                tablename = "FWPT_XHDYHFK";
                this.yjbt.Text = "您在富美直通车的销货单用户反馈已受理";
                break;

            case "FM_YHJFJXG":
                this.bt.Text = "【用户交付旧硒鼓】";
                email_cs = "KHZH";
                tablename = "FM_YHJFJXG";
                this.yjbt.Text = "您在用户直通车用户交付旧硒鼓反馈已受理";
                break;
            case "FM_GZBX":
                this.bt.Text = "【用户直通车故障报修表】";
                email_cs = "EMAIL";
                tablename = "FM_GZBX";
                this.yjbt.Text = "您在富美直通车的故障报修表已受理";
                break;
            case "FM_XHDPJYQS":
                this.bt.Text = "【用户直通车销货单评价与签收】";
                email_cs = "DLYX";
                tablename = "FM_XHDPJYQS";
                this.yjbt.Text = "您在富美直通车的销货单评价与签收已受理";
                break;
            default:
                this.bt.Text = "";
                email_cs = "EMAIL";
                tablename = "";
                this.yjbt.Text = "富美科技信息反馈(请不要答复此邮件)";
                break;
        }
       
        if (!IsPostBack)
        {
            
            string sql_tjsj = "select *," + email_cs + " as email from " + bt_module + " where Number ='" + Request["number"].ToString() + "'"; 
         
          
            SqlDataReader sdr = DbHelperSQL.ExecuteReader(sql_tjsj);
            if (sdr.Read())
            {
                this.tjsj.Text = sdr["CreateTime"].ToString();
                this.yjdz.Text = sdr["email"].ToString();//邮件地址(默认为客户留下的邮件地址 )
            }
            //初始化发件人显示名称、发件人邮箱、邮件标题、以上输入框都可随意更改。
            this.fjrxm.Text = "富美科技有限公司";
            this.fjryx.Text = "fmliuyb@fm8844.com";
            //this.yjbt.Text = "富美科技信息反馈(请不要答复此邮件)";
        }
    }
    protected void btn_fs_Click(object sender, EventArgs e)
    {

        string toemail = "";
        string fromemail = "";
        toemail = this.yjdz.Text.ToString();
        fromemail = this.fjryx.Text.ToString();
        string content = this.hfnr.Text.ToString().Replace("\r\n", "<br/>");;//回复信息
        MailMessage mailMsg = new MailMessage();
        mailMsg.From = new MailAddress(fromemail, this.fjrxm.Text, System.Text.Encoding.UTF8);//发信人信息（包括地址、名称）
        mailMsg.To.Add(toemail);//收件人地址
        mailMsg.Subject = this.yjbt.Text;//邮件主题
        mailMsg.Body = "";
        mailMsg.BodyEncoding = Encoding.UTF8;
        mailMsg.IsBodyHtml = true ;
        mailMsg.Priority = MailPriority.High;
        mailMsg.Body=setbody(number, content);//邮件正文发送设置好的模板
        SmtpClient smtp = new SmtpClient();
        // 提供身份验证的用户名和密码 
        // 网易邮件用户可能为：username password 
        // Gmail 用户可能为：username@gmail.com password 
        smtp.Credentials = new NetworkCredential("fmliuyb", "fmlyb#123");
        smtp.Port = 25; // Gmail 使用 465 和 587 端口 
        smtp.Host = "192.168.0.4"; // 如 smtp.163.com, smtp.gmail.com 
        smtp.EnableSsl = false; // 如果使用GMail，则需要设置为true 
        smtp.SendCompleted += new SendCompletedEventHandler(SendMailCompleted);
        try
        {
            smtp.SendAsync(mailMsg, mailMsg);
            //Response.Write("<Script language ='javaScript'>alert('发送成功!');window.top.close_yj();</Script>");
            //Response.End();
        }
        catch (SmtpException ex)
        {
            Response.Write(ex.ToString());
        }
        string upsql = "update  " + tablename + "  set YJSFHF ='是',SJHFYX='" + toemail + "',HFR='" + User.Identity.Name.Trim() + "',HFNR='" + content + "' where Number='" + number + "' ";
        DbHelperSQL.ExecuteSql(upsql);
    }


    protected string setbody(string number, string content)
    {
        StringBuilder body = new StringBuilder();
        try
        {
            //读取HTML模板，即发送的页面
            string strPath = System.Web.HttpContext.Current.Server.MapPath("~/Web/Huifumail.htm");
            //读取文件，“System.Text.Encoding.Default”可以解决中文乱码问题
            StreamReader sr = new StreamReader(strPath, System.Text.Encoding.Default);
            body.Append(sr.ReadToEnd());
            //关闭文件流
            sr.Close();
        }
        catch (Exception e)
        {

        }
        string xm = "";
        string lx = "";
        string nr = "";
        string title = "";
        string nrbt = "";
        string sql = "select * from " + tablename + " where Number ='" + number + "'";
        DataSet drdataset = DbHelperSQL.Query(sql);
        if (drdataset.Tables[0].Rows.Count != 0)
        {
            switch (bt_module)
            {
                case "OnLineOrder":
                    title = "【在线订单】";
                    //如果是客户提交的订单，姓名可能会为空，这种情况取收货人姓名
                    //if (drdataset.Tables[0].Rows[0]["XM"].ToString() == "" || drdataset.Tables[0].Rows[0]["XM"].ToString() == null)
                    //{
                    //    xm = drdataset.Tables[0].Rows[0]["SHRXM"].ToString();
                    //}
                    //else
                    //{
                    //    xm = drdataset.Tables[0].Rows[0]["XM"].ToString();
                    //}
                    lx = "在线订购";
                    string sql_zb = "select * from OnLineGWLB where parentNumber='" + number + "'";
                    DataSet dssl = FMOP.DB.DbHelperSQL.Query(sql_zb);
                    int zbCount = dssl.Tables[0].Rows.Count;
                    string zb_html = "<table style='background-color: #e0e0e0;' border='0' cellspacing='1' cellpadding='0'><tr align='center'><td bgcolor='#FFFFFF' height='25' align ='center' width='150'>产品型号</td><td bgcolor='#FFFFFF' height='25' align ='center' width='100'>是否符合绿装以旧换新规则</td><td bgcolor='#FFFFFF' height='25' align ='center' width='100'>价格</td> <td bgcolor='#FFFFFF' height='25' align ='center' width='100'>数量</td><td bgcolor='#FFFFFF' height='25' align ='center' width='100'>金额</td></tr>";
                    //循环构造产品列表
                    if (zbCount > 0)
                    {
                        if (zbCount > 5)//多于五项的只显示前五项
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                string cpxh = dssl.Tables[0].Rows[i]["CPXH"].ToString();
                                string shuxing = dssl.Tables[0].Rows[i]["shuxing"].ToString();
                                string mtbj = dssl.Tables[0].Rows[i]["SCTYLSJ"].ToString();
                                string sl = dssl.Tables[0].Rows[i]["SL"].ToString();
                                string je = dssl.Tables[0].Rows[i]["JE"].ToString();
                                string zb_temp = "<tr><td align='center' bgcolor='#FFFFFF' height='25'>" + cpxh + "</td><td align='center' bgcolor='#FFFFFF' height='25'>" + shuxing + "</td><td align='center' bgcolor='#FFFFFF' height='25'>" + mtbj + "</td><td align='center' bgcolor='#FFFFFF' height='25'>" + sl + "</td><td align='center' bgcolor='#FFFFFF' height='25'>" + je + "</td></tr>";
                                zb_html = zb_html + zb_temp;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < zbCount; i++)
                            {
                                string cpxh = dssl.Tables[0].Rows[i]["CPXH"].ToString();
                                string shuxing = dssl.Tables[0].Rows[i]["shuxing"].ToString();
                                string mtbj = dssl.Tables[0].Rows[i]["SCTYLSJ"].ToString();
                                string sl = dssl.Tables[0].Rows[i]["SL"].ToString();
                                string je = dssl.Tables[0].Rows[i]["JE"].ToString();
                                string zb_temp = "<tr><td align='center' bgcolor='#FFFFFF' height='25'>" + cpxh + "</td><td align='center' bgcolor='#FFFFFF' height='25'>" + shuxing + "</td><td align='center' bgcolor='#FFFFFF' height='25'>" + mtbj + "</td><td align='center' bgcolor='#FFFFFF' height='25'>" + sl + "</td><td align='center' bgcolor='#FFFFFF' height='25'>" + je + "</td></tr>";
                                zb_html = zb_html + zb_temp;
                            }
                        }
                    }
                    nr = zb_html+"</table>";
                    nrbt = "订购产品列表（多于五项的只显示前五项）";
                    break;
                case "Web_WLZZFW_ZXZXFHY":
                    title = "【在线咨询】";
                    //xm = drdataset.Tables[0].Rows[0]["XM"].ToString();
                    lx = drdataset.Tables[0].Rows[0]["ZXLX"].ToString();
                    if (drdataset.Tables[0].Rows[0]["ZXNR"].ToString().Length < 200)
                    {
                        nr = drdataset.Tables[0].Rows[0]["ZXNR"].ToString();
                    }
                    else
                    {
                        nr = drdataset.Tables[0].Rows[0]["ZXNR"].ToString().Substring(0,200);
                    }
                    nrbt = "咨询内容";
                    break;
                case "Web_WLZZFW_ZXTSFHY":
                    title = "【在线投诉】";
                    //xm = drdataset.Tables[0].Rows[0]["XM"].ToString();
                    lx = drdataset.Tables[0].Rows[0]["TSLX"].ToString();
                    if (drdataset.Tables[0].Rows[0]["TSNR"].ToString().Length < 200)
                    {
                        nr = drdataset.Tables[0].Rows[0]["TSNR"].ToString();
                    }
                    else
                    {
                        nr = drdataset.Tables[0].Rows[0]["TSNR"].ToString().Substring(0, 200);
                    }
                    nrbt = "投诉内容";
                    break;
                case "Web_WLZZFW_ZXJY":
                    title = "【在线建议】";
                    //xm = drdataset.Tables[0].Rows[0]["XM"].ToString();
                    lx = drdataset.Tables[0].Rows[0]["JYLX"].ToString();
                    if (drdataset.Tables[0].Rows[0]["JYNR"].ToString().Length < 200)
                    {
                        nr = drdataset.Tables[0].Rows[0]["JYNR"].ToString();
                    }
                    else
                    {
                        nr = drdataset.Tables[0].Rows[0]["JYNR"].ToString().Substring(0, 200);
                    }
                    nrbt = "建议内容";
                    break;
                case "Web_HBHS_FHY":
                    title = "【环保回收】";
                    //xm = drdataset.Tables[0].Rows[0]["XM"].ToString();
                    lx = "";
                    if (drdataset.Tables[0].Rows[0]["HSXX"].ToString().Length < 200)
                    {
                        nr = drdataset.Tables[0].Rows[0]["HSXX"].ToString();
                    }
                    else
                    {
                        nr = drdataset.Tables[0].Rows[0]["HSXX"].ToString().Substring(0,200);
                    }
                    nrbt = "回收信息";
                    break;
                case "Web_GZBX_FHY":
                    title = "【故障报修】";
                    //xm = drdataset.Tables[0].Rows[0]["XM"].ToString();
                    lx = drdataset.Tables[0].Rows[0]["GZYY"].ToString();
                    if (drdataset.Tables[0].Rows[0]["GZMS"].ToString().Length < 200)
                    {
                        nr = drdataset.Tables[0].Rows[0]["GZMS"].ToString();
                    }
                    else
                    {
                        nr = drdataset.Tables[0].Rows[0]["GZMS"].ToString().Substring(0,200);
                    }
                    nrbt = "故障描述";
                    break;
//直通车意见与建议反馈 
                case "FWPT_YJYJJFK":
                    title = "【意见与建议反馈】";
                   // lx = drdataset.Tables[0].Rows[0]["FKZT"].ToString();
                    lx = "";
                    if (drdataset.Tables[0].Rows[0]["FKNR"].ToString().Length < 200)
                    {
                        nr = drdataset.Tables[0].Rows[0]["FKNR"].ToString();
                    }
                    else
                    {
                        nr = drdataset.Tables[0].Rows[0]["FKNR"].ToString().Substring(0, 200);
                    }
                    nrbt = "反馈内容";
                    break;
                //用户直通车意见与建议反馈 
                case "FM_YJYJY":
                    title = "【意见与建议反馈】";
                    // lx = drdataset.Tables[0].Rows[0]["FKZT"].ToString();
                    lx = "";
                    if (drdataset.Tables[0].Rows[0]["FKXXNR"].ToString().Length < 200)
                    {
                        nr = drdataset.Tables[0].Rows[0]["FKXXNR"].ToString();
                    }
                    else
                    {
                        nr = drdataset.Tables[0].Rows[0]["FKXXNR"].ToString().Substring(0, 200);
                    }
                    nrbt = "反馈内容";
                    break;
                //直通车销货单用户反馈 
                case "FWPT_XHDYHFK":
                    title = "【销货单用户反馈】";
                    // lx = drdataset.Tables[0].Rows[0]["FKZT"].ToString();
                    lx = "";
                    if (drdataset.Tables[0].Rows[0]["YJFK"].ToString().Length < 200)
                    {
                        nr = drdataset.Tables[0].Rows[0]["YJFK"].ToString();
                    }
                    else
                    {
                        nr = drdataset.Tables[0].Rows[0]["YJFK"].ToString().Substring(0, 200);
                    }
                    nrbt = "反馈内容";
                    break;
                default:
                    title = "";
                    xm = "";
                    lx = "";
                    nr = "";
                    break;
            }
        }
        
        body = body.Replace("{title}", title);
        body = body.Replace("{tjsj}", this.tjsj.Text);
        //body = body.Replace("{xm}", xm);
        body = body.Replace("{lx}", lx);
        body = body.Replace("{nr}", nr);
        body = body.Replace("{nrbt}", nrbt);
        body = body.Replace("{huifu}", content);

        return body.ToString().Trim();
    }
    void SendMailCompleted(object sender, AsyncCompletedEventArgs e)
    {
        MailMessage mailMsg = (MailMessage)e.UserState;
        string subject = mailMsg.Subject;
        if (e.Cancelled) // 邮件被取消 
        {
            //Response.Write(subject + " 被取消。");
            FMOP.Common.MessageBox.Show(Page, subject + " 被取消。");
        }
        if (e.Error != null)
        {
            //Response.Write("错误：" + e.Error.ToString());
            FMOP.Common.MessageBox.Show(Page, "程序出现如下错误：" + e.Error.ToString());
        }
        else
        {
            //Page.RegisterStartupScript("提示", "<script language ='javascript'>alert('发送完成！')window.top.frames.rightFrame.history.back();<</script>");
            //Response.Write("<Script language ='javaScript'>alert('发送完成!');window.top.close_yj();</Script>");
            Response.Write("<Script language ='javaScript'>alert('发送完成!');window.location.href='WorkFlow_Check.aspx?module=" + bt_module + "&number=" + number+" ';</Script>");
            Response.End();

        }
    }
}
