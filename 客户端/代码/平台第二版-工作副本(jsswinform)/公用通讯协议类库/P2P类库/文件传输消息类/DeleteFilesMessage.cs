using System;
using System.Collections.Generic;
using System.Text;

namespace 公用通讯协议类库.P2P类库.文件传输消息类
{
    /// <summary>
    /// 删除文件/文件夹 消息类 包含目录和删除目录名或文件名  以及删除类型标记
    /// </summary>
    [Serializable]
    public class DeleteFilesMessage : PPMessage
    {
        private string delType;
        /// <summary>
        /// 删除类型标记
        /// </summary>
        public string DelType
        {
            get { return delType; }
            set { delType = value; }
        }

        private string delFileOrPathName;
        /// <summary>
        /// 要删除的目录或者文件名
        /// </summary>
        public string DelFileOrPathName
        {
            get { return delFileOrPathName; }
            set { delFileOrPathName = value; }
        }
    }
}
