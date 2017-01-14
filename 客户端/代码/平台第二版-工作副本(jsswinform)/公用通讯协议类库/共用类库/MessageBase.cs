using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace 公用通讯协议类库.共用类库
{
    /// <summary>
    /// 所有消息基类
    /// </summary>
    [Serializable]
    public abstract class MessageBase
    {
        private string usernameform;
        private string usernameto;
        private string makemsg;
        /// <summary>
        /// 消息发出者帐号
        /// </summary>
        public string Usernameform
        {
            get { return usernameform; }
            set { usernameform = value; }
        }
        /// <summary>
        /// 消息目的地帐号
        /// </summary>
        public string Usernameto
        {
            get { return usernameto; }
            set { usernameto = value; }
        }
        /// <summary>
        /// 公用标志性特殊消息
        /// </summary>
        public string Makemsg
        {
            get { return makemsg; }
            set { makemsg = value; }
        }
    }
}
