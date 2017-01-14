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
    /// 公用常量
    /// </summary>
    public class P2PConsts
    {
        /// <summary>
        /// 服务器端端口
        /// </summary>
        public const int SRV_PORT = 1021; //服务器端端口
        /// <summary>
        /// 主控客户端端口
        /// </summary>
        public const int MainControl_PORT = 1022; //主控客户端端口
        /// <summary>
        /// 被控客户端端口
        /// </summary>
        public const int BeClient_PORT = 1023; //被控客户端端口
    }
}
