﻿using System;
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
    /// 请求用户列表应答消息类
    /// </summary>
    [Serializable]
    public class GetUsersResponseMessage : SCMessage
    {
        private UserCollection userlist;
        /// <summary>
        /// 在线用户列表
        /// </summary>
        public UserCollection Userlist
        {
            get { return userlist; }
            set { userlist = value; }
        }

    }
}
