using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using 公用通讯协议类库.共用类库;

namespace 公用通讯协议类库.C2S类库
{
    /// <summary>
    /// 用户登录消息类
    /// </summary>
    [Serializable]
    public class LoginMessage : CSMessage
    {
        private string username;
        private string password;
        private Hashtable pciplist;

        /// <summary>
        /// 登陆帐号
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        /// <summary>
        /// 本地IP地址列表
        /// </summary>
        public Hashtable Pciplist
        {
            get { return pciplist; }
            set { pciplist = value; }
        }
    }
}
