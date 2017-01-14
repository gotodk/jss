using System;
using System.Collections.Generic;
using System.Text;
using 公用通讯协议类库.共用类库;

namespace 公用通讯协议类库.消息片类库
{
    /// <summary>
    /// 消息片接收后的应答消息
    /// </summary>
    [Serializable]
    public class FileRevieveATC
    {
        private byte[] byte_act;
        /// <summary>
        /// 消息片接受应答标志
        /// </summary>
        public byte[] Byte_ACT
        {
            get { return byte_act; }
            set { byte_act = value; }
        }


        private string onlyonefloat;
        /// <summary>
        /// 传输通道唯一标记
        /// </summary>
        public string OnlyOneFloat
        {
            get { return onlyonefloat; }
            set { onlyonefloat = value; }
        }
    }
}
