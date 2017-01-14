using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Collections;
using System.Net.Mail;

/// <summary>
///jhjx_ClassEmail 的摘要说明
/// </summary>
public class ClassEmail
{
	public ClassEmail()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
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
    public void SendSMTPEMail(string strSmtpServer, string strFrom, string username, string strFromPass, string strto, string strSubject, string strBody)
    {
        try
        {
            System.Net.Mail.SmtpClient client = new SmtpClient(strSmtpServer);
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential(username, strFromPass);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            System.Net.Mail.MailMessage message = new MailMessage(strFrom, strto, strSubject, strBody);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            client.Send(message);
        }
        catch
        {
        }
       
    }

    public void SendSPEmail(string strto, string strSubject, string modhtmlFileName, Hashtable rpHT)
    {
        string emailbody = "";
        StreamReader reader = new StreamReader(modhtmlFileName, System.Text.Encoding.Default);
        emailbody = reader.ReadToEnd();
        reader.Close();

        foreach (System.Collections.DictionaryEntry item in rpHT)
        {
            string key = item.Key.ToString();//键
            string newstr = item.Value.ToString();//值
            emailbody = emailbody.Replace(key, newstr);
        }
        string[] yjxt = strto.Split('@');
        if (yjxt[1] == "sina.com" || yjxt[1] == "sina.com.cn" || yjxt[1] == "sina.cn" || yjxt[1] == "vip.sina.com" || yjxt[1] == "139.com" || yjxt[1] == "aeonmed.com")
        {
            SendSMTPEMail("smtp.sina.com", "forever_8844@sina.com", "forever_8844", "forever_2011", strto, strSubject, emailbody);
        }
        else if (yjxt[1] == "126.com" || yjxt[1] == "163.com")
        {
            SendSMTPEMail("smtp.126.com", "forever_8844@126.com", "forever_8844", "forever_2011", strto, strSubject, emailbody);
        }
        //qq邮箱申请超过14天后才能设置smtp，暂使用126的，2011年11月24日(星期四) 上午9:00 后可设置smtp后开放
        else if (yjxt[1] == "qq.com")
        {
            //SendSMTPEMail("smtp.qq.com", "forever_8844@qq.com", "forever_8844", "forever_2011", strto, strSubject, emailbody);
            SendSMTPEMail("smtp.126.com", "forever_8844@126.com", "forever_8844", "forever_2011", strto, strSubject, emailbody);
        }
        
        else if (yjxt[1] == "yahoo.cn" || yjxt[1] == "yahoo.com.cn")
        {
            SendSMTPEMail("smtp.126.com", "forever_8844@126.com", "forever_8844", "forever_2011", strto, strSubject, emailbody);
        }        
        else if (yjxt[1] == "sohu.com")
        {
            SendSMTPEMail("smtp.126.com", "forever_8844@126.com", "forever_8844", "forever_2011", strto, strSubject, emailbody);
        }        
        else if (yjxt[1] == "tom.com")
        {
            SendSMTPEMail("smtp.126.com", "forever_8844@126.com", "forever_8844", "forever_2011", strto, strSubject, emailbody);
        }
        else if (yjxt[1] == "21cn.com")
        {
            SendSMTPEMail("smtp.21cn.com", "forever_8844@21cn.com", "forever_8844", "forever_2011", strto, strSubject, emailbody);
        }        
        else if (yjxt[1] == "hotmail.com")
        {
            SendSMTPEMail("smtp.126.com", "forever_8844@126.com", "forever_8844", "forever_2011", strto, strSubject, emailbody);
        }
        else
        {
            SendSMTPEMail("smtp.126.com", "forever_8844@126.com", "forever_8844", "forever_2011", strto, strSubject, emailbody);
            //SendSMTPEMail("192.168.0.4", "fmliuyb@fm8844.com", "fmliuyb", "fmlyb#123", strto, strSubject, emailbody);
        }



    }


}