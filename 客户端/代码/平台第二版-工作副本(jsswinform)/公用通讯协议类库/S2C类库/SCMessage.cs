using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using 公用通讯协议类库.共用类库;

namespace 公用通讯协议类库.S2C类库
{
    /// <summary>
    /// 服务器发送到客户端消息基类
    /// </summary>
    [Serializable]
    public abstract class SCMessage : MessageBase
    { }
}
