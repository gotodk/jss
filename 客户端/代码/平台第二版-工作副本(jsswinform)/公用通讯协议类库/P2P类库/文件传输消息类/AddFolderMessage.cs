using System;
using System.Collections.Generic;
using System.Text;

namespace 公用通讯协议类库.P2P类库.文件传输消息类
{
    /// <summary>
    /// 添加文件夹 消息类 包含目录和新建目录名
    /// </summary>
    [Serializable]
    public class AddFolderMessage : PPMessage
    {
        private string addFolder;
        /// <summary>
        /// 要在哪个目录进行添加
        /// </summary>
        public string AddFolder
        {
            get { return addFolder; }
            set { addFolder = value; }
        }

        private string newFolderName;
        /// <summary>
        /// 要添加的目录名
        /// </summary>
        public string NewFolderName
        {
            get { return newFolderName; }
            set { newFolderName = value; }
        }
    }
}
