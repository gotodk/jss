using System;
using System.Collections.Generic;
using System.Text;

namespace 公用通讯协议类库.P2P类库.文件传输消息类
{
    /// <summary>
    /// 获取文件列表 消息类
    /// </summary>
    [Serializable]
    public class GetFilesInfoMessage : PPMessage
    {
        private string orderNextPath;
        /// <summary>
        /// 要求获取的路径
        /// </summary>
        public string OrderNextPath
        {
            get { return orderNextPath; }
            set { orderNextPath = value; }
        }
    }
}
