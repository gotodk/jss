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
    /// 点对点 Purch Hole 打洞消息类
    /// </summary>
    [Serializable]
    public class TrashMessage : PPMessage
    {
        private string linktype;
        /// <summary>
        /// 连接方式
        /// </summary>
        public string Linktype
        {
            get { return linktype; }
            set { linktype = value; }
        }
    }
}
