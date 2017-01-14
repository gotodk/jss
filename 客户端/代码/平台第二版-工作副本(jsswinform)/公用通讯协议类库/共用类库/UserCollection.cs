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
    /// 用户列表操作类
    /// </summary>
    [Serializable]
    public class UserCollection : CollectionBase
    {
        /// <summary>
        /// 添加在线用户
        /// </summary>
        /// <param name="user"></param>
        public void Add(User user)
        {
            InnerList.Remove(user);
            InnerList.Add(user);
        }
        /// <summary>
        /// 删除在线用户
        /// </summary>
        /// <param name="user"></param>
        public void Remove(User user)
        {
            InnerList.Remove(user);
        }
        /// <summary>
        /// 根据列表索引查找在线用户
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public User this[int index]
        {
            get { return (User)InnerList[index]; }
        }
        /// <summary>
        /// 根据登录名查找在线用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User Find(string userName)
        {
            foreach (User user in this)
            {
                if (string.Compare(userName, user.UserName, true) == 0)
                {
                    return user;
                }
            }
            return null;
        }


    }
}
