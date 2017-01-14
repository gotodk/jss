using System;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;
using System.IO;
using System.Collections;
using System.Data;
using System.Configuration;
using FMDBHelperClass;

/// <summary>
/// 这个类用于向客户发送邮件，验证码
/// </summary>
public class ClassEmail
{
	public ClassEmail()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
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

        MailSMTPSendQQMail(strto, strSubject, emailbody);       


    }

    /// <summary>
    /// 发送邮件（admin@cealer.com）
    /// </summary>
    /// <param name="strto">收件人帐号</param>
    /// <param name="strSubject">邮件名</param>
    /// <param name="strBody">邮件内容</param>
    public static void MailSMTPSendQQMail(string strto, string strSubject, string strBody)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable result_ht = new Hashtable();

        string strSmtpServer = "smtp.exmail.qq.com";
        string username = "mail@fumeichina.com";
        string strFromPass = "fm8844.com";
        string strFrom = "mail@fumeichina.com";

        string sql = "SELECT * FROM AAA_PTYXJLB ORDER BY FWYX ASC";
        string sqlCount = string.Empty;

        result_ht=I_DBL.RunProc(sql,"dtResult");
        DataTable dt = ((DataSet)result_ht["return_ds"]).Tables[0];

        ArrayList al = new ArrayList();

        if (dt!=null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt32(dt.Rows[i]["JRFSSL"]) < 800)
                {
                    username = dt.Rows[i]["FWYX"].ToString().Trim();
                    strFromPass = dt.Rows[i]["MM"].ToString().Trim();
                    strFrom = dt.Rows[i]["FWYX"].ToString().Trim();


                    sqlCount = "UPDATE AAA_PTYXJLB SET JRFSSL=JRFSSL+1 WHERE FWYX='" + dt.Rows[i]["FWYX"].ToString() + "'";
                    al.Add(sqlCount);
                    break;
                }
            }
        }
        strSmtpServer = "192.168.0.4";
        username = "fmliuyb@fm8844.com";
        strFromPass = "fm8844com";
        strFrom = "fmliuyb@fm8844.com";
        try
        {
            I_DBL.RunParam_SQL(al);
           
            System.Net.Mail.SmtpClient client = new SmtpClient(strSmtpServer);
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential(username, strFromPass);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            System.Net.Mail.MailMessage message = new MailMessage(strFrom, strto, strSubject, strBody);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            client.Send(message);
        }
        catch (Exception ex)
        {
            string exMsg = ex.Message;
        }
    }

}