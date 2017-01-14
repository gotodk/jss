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
    /// 在线用户列表类
    /// </summary>
    [Serializable]
    public class User
    {
        protected string userName;
        protected IPEndPoint netPoint_G;
        protected Hashtable netPoint_J;
        protected string online_own = "";
        protected string type_own = "";
        protected IPEndPoint netPoint_own;
        /// <summary>
        /// 使用者帐号
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        /// <summary>
        /// 广域网节点
        /// </summary>
        public IPEndPoint NetPoint_G
        {
            get { return netPoint_G; }
            set { netPoint_G = value; }
        }
        /// <summary>
        /// 局域网节点列表
        /// </summary>
        public Hashtable NetPoint_J
        {
            get { return netPoint_J; }
            set { netPoint_J = value; }
        }
        /// <summary>
        /// 私有连接状态(可连接)
        /// </summary>
        public string Online_own
        {
            get { return online_own; }
            set { online_own = value; }
        }
        /// <summary>
        /// 私有连接方式(广域/局域)
        /// </summary>
        public string Type_own
        {
            get { return type_own; }
            set { type_own = value; }
        }
        /// <summary>
        /// 私有有效终结点(每次计算)
        /// </summary>
        public IPEndPoint NetPoint_own
        {
            get { return netPoint_own; }
            set { netPoint_own = value; }
        }
    }
}
