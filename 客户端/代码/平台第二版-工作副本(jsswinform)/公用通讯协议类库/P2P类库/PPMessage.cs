using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using 公用通讯协议类库.共用类库;

namespace 公用通讯协议类库.P2P类库
{
    /// <summary>
    /// 点对点消息基类
    /// </summary>
    [Serializable]
    public abstract class PPMessage : MessageBase
    {
        private string remessage;
        /// <summary>
        /// 普通字符应答的消息内容
        /// </summary>
        public string Remessage
        {
            get { return remessage; }
            set { remessage = value; }
        }
    }
}
