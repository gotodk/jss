using System;
using System.Collections.Generic;
using System.Text;

namespace 公用通讯协议类库.P2P类库.文件传输消息类
{
    /// <summary>
    /// 要求上传文件 消息类
    /// </summary>
    [Serializable]
    public class ApplyUpLoadMessage : PPMessage
    {
        private string orderfilepath;
        /// <summary>
        /// 要求上传的文件路径
        /// </summary>
        public string OrderFilePath
        {
            get { return orderfilepath; }
            set { orderfilepath = value; }
        }

        private string savefilepath;
        /// <summary>
        /// 要保存的文件路径
        /// </summary>
        public string SaveFilePath
        {
            get { return savefilepath; }
            set { savefilepath = value; }
        }
    }
}
