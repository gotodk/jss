using System;
using System.Collections.Generic;
using System.Text;
using 公用通讯协议类库.共用类库;

namespace 公用通讯协议类库.P2P类库.文件传输消息类
{
    /// <summary>
    /// 获取文件列表 应答消息类 包含文件分区列表
    /// </summary>
    [Serializable]
    public class ACKGetFilesInfoMessage : PPMessage
    {
        private FilesInfoCollection filesInfoList;
        /// <summary>
        /// 目录列表
        /// </summary>
        public FilesInfoCollection FilesInfoList
        {
            get { return filesInfoList; }
            set { filesInfoList = value; }
        }
    }
}
