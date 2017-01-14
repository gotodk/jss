using System;
using System.Collections.Generic;
using System.Text;

namespace 公用通讯协议类库.P2P类库
{
    /// <summary>
    /// 打开3389消息类
    /// </summary>
    [Serializable]
    public class OpenTscMessage : PPMessage
    {
        private string winuser;
        /// <summary>
        /// 要添加的账号
        /// </summary>
        public string WinUser
        {
            get { return winuser; }
            set { winuser = value; }
        }

        private string winpass;
        /// <summary>
        /// 要添加的密码
        /// </summary>
        public string WinPass
        {
            get { return winpass; }
            set { winpass = value; }
        }


    }
}
