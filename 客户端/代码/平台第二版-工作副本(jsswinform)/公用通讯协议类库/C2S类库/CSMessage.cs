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
    /// 客户端发送到服务器的消息基类,继承自所有消息基类
    /// </summary>
    [Serializable]
    public abstract class CSMessage : MessageBase
    {
        
    }
}
