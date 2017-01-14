using System;
using System.Collections.Generic;
using System.Text;

namespace 公用通讯协议类库.S2C类库
{
    /// <summary>
    /// 登陆结果消息
    /// </summary>
    [Serializable]
    public class ACKLoginMessage : SCMessage
    {
        private string reloginmessage;
        /// <summary>
        /// 登陆结果应答消息
        /// </summary>
        public string Reloginmessage
        {
            get { return reloginmessage; }
            set { reloginmessage = value; }
        }
    }
}
