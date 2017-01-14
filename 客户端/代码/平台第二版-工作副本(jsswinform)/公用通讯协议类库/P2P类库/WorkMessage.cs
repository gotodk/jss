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
    /// 文字消息类
    /// </summary>
    [Serializable]
    public class WorkMessage : PPMessage
    {
        private string textmessage;
        /// <summary>
        /// 文字消息内容
        /// </summary>
        public string Textmessage
        {
            get { return textmessage; }
            set { textmessage = value; }
        }

    }
}
