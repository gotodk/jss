﻿
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Configuration;

namespace Mailer.Components
{
    #region 邮件发送类

    public class SMTP
    {
        #region Fields

        private string _mailFrom;
        private string _displayName;
        private SmtpClient smtpMail;

        #endregion

        #region Properties

        public string MailFrom
        {
            get { return _mailFrom; }
            set { _mailFrom = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        #endregion

        #region Constructors

        public SMTP()
        {
			System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/Web.Config");
            MailSettingsSectionGroup mailSettings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

            string mailFrom = mailSettings.Smtp.From;
            string smtpServer = mailSettings.Smtp.Network.Host;
            int smtpPort = mailSettings.Smtp.Network.Port;
            string smtpUsername = mailSettings.Smtp.Network.UserName;
            string password = mailSettings.Smtp.Network.Password;

            smtpMail = new SmtpClient(smtpServer, smtpPort);
            smtpMail.Credentials = new NetworkCredential(smtpUsername, password);
            this._mailFrom = mailFrom;
            this._displayName = smtpUsername;
        }

        public SMTP(string smtpServer, string userName, string password)
            : this(null, smtpServer, userName, password)
        {
        }

        /**//// <summary>
        /// 邮件发送类
        /// </summary>
        /// <param name="mailFrom">发件人地址</param>
        /// <param name="smtpServer">SMTP 服务器</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        public SMTP(string mailFrom, string smtpServer, string userName, string password)
            : this(mailFrom, mailFrom, smtpServer, userName, password)
        {

        }

        /**//// <summary>
        /// 邮件发送类
        /// </summary>
        /// <param name="mailFrom">发件人地址</param>
        /// <param name="displayName">显示的名称</param>
        /// <param name="smtpServer">SMTP 服务器</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        public SMTP(string mailFrom, string displayName, string smtpServer, string userName, string password)
            : this(mailFrom, displayName, smtpServer, 25, userName, password, false)
        {
        }

        public SMTP(string mailFrom, string displayName, string smtpServer, int smtpPort, string userName, string password, bool smtpSsl)
        {
            this._mailFrom = mailFrom;
            this._displayName = displayName;
            smtpMail = new SmtpClient(smtpServer, smtpPort);
            smtpMail.Credentials = new NetworkCredential(userName, password);
            smtpMail.EnableSsl = smtpSsl;
        }


        #endregion

        #region Methods

        #region Public

        public Boolean Send(string mailTo, string mailSubject, string mailBody, MailPriority priority, bool isBodyHtml)
        {
            string[] mailTos = new string[] { mailTo };
            string[] attachments = null;
            System.Text.Encoding bodyEncoding = System.Text.Encoding.Default;
            return Send(mailTos, null, null, mailSubject, mailBody, attachments, priority, isBodyHtml, bodyEncoding);
        }

        public Boolean Send(string mailTo, string mailSubject, string mailBody, string[] attachments, MailPriority priority, bool isBodyHtml, System.Text.Encoding bodyEncoding)
        {
            string[] mailTos = new string[] { mailTo };
            return Send(mailTos, null, null, mailSubject, mailBody, attachments, priority, isBodyHtml, bodyEncoding);
        }

        public Boolean Send(string[] mailTos, string[] mailCcs, string[] mailBccs, string mailSubject, string mailBody, string[] attachments, MailPriority priority, bool isBodyHtml, System.Text.Encoding bodyEncoding)
        {
            return Send(this._mailFrom, this._displayName, mailTos, mailCcs, mailBccs, mailSubject, mailBody, attachments, priority, isBodyHtml, bodyEncoding);
        }

        /**//// <summary>
        /// 同步发送邮件
        /// </summary>
        /// <returns></returns>
        public Boolean Send(string mailFrom, string displayName, string[] mailTos, string[] mailCcs, string[] mailBccs, string mailSubject, string mailBody, string[] attachments, MailPriority priority, bool isBodyHtml, System.Text.Encoding bodyEncoding)
        {
            return SendMail(false, null, mailFrom, displayName, mailTos, mailCcs, mailBccs, mailSubject, mailBody, attachments, priority, isBodyHtml, bodyEncoding);
        }

        public void SendAsync(object userState, string mailTo, string mailSubject, string mailBody, MailPriority priority, bool isBodyHtml)
        {
            string[] mailTos = new string[] { mailTo };
            string[] attachments = null;
            System.Text.Encoding bodyEncoding = System.Text.Encoding.Default;
            SendAsync(userState, mailTos, null, null, mailSubject, mailBody, attachments, priority, isBodyHtml, bodyEncoding);
        }

        public void SendAsync(object userState, string mailTo, string mailSubject, string mailBody, string[] attachments, MailPriority priority, bool isBodyHtml, System.Text.Encoding bodyEncoding)
        {
            string[] mailTos = new string[] { mailTo };
            SendAsync(userState, this._mailFrom, this._displayName, mailTos, null, null, mailSubject, mailBody, attachments, priority, isBodyHtml, bodyEncoding);
        }

        public void SendAsync(object userState, string mailFrom, string displayName, string[] mailTos, string[] mailCcs, string[] mailBccs, string mailSubject, string mailBody, string[] attachments, MailPriority priority, bool isBodyHtml, System.Text.Encoding bodyEncoding)
        {
            SendMail(true, userState, mailFrom, displayName, mailTos, mailCcs, mailBccs, mailSubject, mailBody, attachments, priority, isBodyHtml, bodyEncoding);
        }

        /**//// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="userState">异步任务的唯一标识符</param>
        /// <returns></returns>
        public void SendAsync(object userState, string[] mailTos, string[] mailCcs, string[] mailBccs, string mailSubject, string mailBody, string[] attachments, MailPriority priority, bool isBodyHtml, System.Text.Encoding bodyEncoding)
        {
            SendMail(true, userState, null, null, mailTos, mailCcs, mailBccs, mailSubject, mailBody, attachments, priority, isBodyHtml, bodyEncoding);
        }


        /**//// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="isAsync">是否异步发送邮件</param>
        /// <param name="userState">异步任务的唯一标识符，当 isAsync 为 True 时必须设置该属性， 当 isAsync 为 False 时可设置为 null</param>
        /// <returns></returns>
        private Boolean SendMail(bool isAsync, object userState, string mailFrom, string displayName, string[] mailTos, string[] mailCcs, string[] mailBccs, string mailSubject, string mailBody, string[] attachments, MailPriority priority, bool isBodyHtml, System.Text.Encoding bodyEncoding)
        {
            bool mailSent = false;

			#region 设置属性值

            if (string.IsNullOrEmpty(mailFrom))
                mailFrom = this._mailFrom;

            if (string.IsNullOrEmpty(displayName))
                displayName = this._displayName;

            MailMessage Email = GetMailMessage(mailFrom, displayName, mailTos, mailCcs, mailBccs, mailSubject, mailBody, attachments, priority, isBodyHtml, bodyEncoding);

            smtpMail.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            #endregion

            try
            {
                if (!isAsync)
                {
                    smtpMail.Send(Email);
                    mailSent = true;
                }
                else
                {
                    userState = (userState == null) ? Guid.NewGuid() : userState;
                    smtpMail.SendAsync(Email, userState);
                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                mailSent = false;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                mailSent = false;
            }

            return mailSent;
        }

        #endregion

        #region Private

        private MailMessage GetMailMessage(string mailFrom, string displayName, string[] mailTos, string[] mailCcs, string[] mailBccs, string mailSubject, string mailBody, string[] attachments, MailPriority priority, bool isBodyHtml, System.Text.Encoding bodyEncoding)
        {
            // build the email message
            MailMessage emailMessage = new MailMessage();

            if (string.IsNullOrEmpty(mailFrom))
                mailFrom = this._mailFrom;

            if (string.IsNullOrEmpty(displayName))
                displayName = this._displayName;

            MailAddress mailFromObject = new MailAddress(mailFrom, displayName);

            emailMessage.From = mailFromObject;

            if (mailTos != null)
            {
                foreach (string mailto in mailTos)
                {
                    if (!string.IsNullOrEmpty(mailto))
                    {
                        emailMessage.To.Add(mailto);
                    }
                }
            }

            if (mailCcs != null)
            {
                foreach (string cc in mailCcs)
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        emailMessage.CC.Add(cc);
                    }
                }
            }

            if (mailBccs != null)
            {
                foreach (string bcc in mailBccs)
                {
                    if (!string.IsNullOrEmpty(bcc))
                    {
                        emailMessage.Bcc.Add(bcc);
                    }
                }
            }

            if (attachments != null)
            {
                foreach (string file in attachments)
                {
                    if (!string.IsNullOrEmpty(file))
                    {
                        Attachment att = new Attachment(file);
                        emailMessage.Attachments.Add(att);
                    }
                }
            }

            emailMessage.Subject = mailSubject;
            emailMessage.Body = mailBody;
            emailMessage.Priority = priority;
            emailMessage.IsBodyHtml = isBodyHtml;
            emailMessage.BodyEncoding = bodyEncoding;

            return emailMessage;
        }

        private void SendCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            bool mailSent = false;

            // Get the unique identifier for this asynchronous operation.
            String token = e.UserState.ToString();

            if (e.Cancelled)
            {
                //Console.WriteLine("[{0}] Send canceled.", token);
                mailSent = false;
            }
            if (e.Error != null)
            {
                //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                mailSent = false;
            }
            else
            {
                //Console.WriteLine("Message sent.");
                mailSent = false;
            }

            mailSent = true;
        }

        #endregion

        #endregion
    }

    #endregion
}