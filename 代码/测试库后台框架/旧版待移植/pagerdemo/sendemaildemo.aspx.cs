using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Collections;
public partial class pagerdemo_sendemaildemo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ClassEmail CE = new ClassEmail();
        string strto = "yhb888@gmail.com";//收件人邮箱
        string strSubject = "这是一个测试邮件的主题";//邮件主题
        string modhtmlFileName = Server.MapPath("../WebYHZTC/emailmod/ZXYH_zhmm.html");//邮件模版
        Hashtable rpHT = new Hashtable(); //模版替换规则
        rpHT["[[yx]]"] = "aaa@aaa.com";
        rpHT["[[ps]]"] = "12345678";
        CE.SendSPEmail(strto, strSubject, modhtmlFileName, rpHT);
    }

    /// <summary>
    /// 送邮件
    /// </summary>
    /// <param name="strSmtpServer">邮箱服务器</param>
    /// <param name="strFrom">发件人的邮箱名称</param>
    /// <param name="strFrom">发件人的登陆账号</param>
    /// <param name="strFromPass">发件人密码</param>
    /// <param name="strto">收件人帐号</param>
    /// <param name="strSubject">参数主题</param>
    /// <param name="strBody">参数内容</param>
    public void SendSMTPEMail(string strSmtpServer, string strFrom,string username, string strFromPass, string strto, string strSubject, string strBody)
    {
        System.Net.Mail.SmtpClient client = new SmtpClient(strSmtpServer);
        client.UseDefaultCredentials = false;
        client.Credentials = new System.Net.NetworkCredential(username, strFromPass);
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        System.Net.Mail.MailMessage message = new MailMessage(strFrom, strto, strSubject, strBody);
        message.BodyEncoding = System.Text.Encoding.UTF8;
        message.IsBodyHtml = true;
        client.Send(message);
    }
}