using System;
using System.Collections.Generic;
using System.Text;

namespace 公用通讯协议类库.P2P类库
{
    /// <summary>
    /// 点对点 Purch Hole 打洞回应消息类
    /// </summary>
    [Serializable]
    public class ACKTrashMessage : PPMessage
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
