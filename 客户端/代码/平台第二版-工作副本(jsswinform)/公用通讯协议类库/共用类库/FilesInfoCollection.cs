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
    /// 硬盘文件列表操作类
    /// </summary>
    [Serializable]
    public class FilesInfoCollection : CollectionBase
    {
        /// <summary>
        /// 添加一个文件信息
        /// </summary>
        /// <param name="filesinfo"></param>
        public void Add(FilesInfo filesinfo)
        {
            InnerList.Remove(filesinfo);
            InnerList.Add(filesinfo);
        }
        /// <summary>
        /// 删除一个文件信息
        /// </summary>
        /// <param name="filesinfo"></param>
        public void Remove(FilesInfo filesinfo)
        {
            InnerList.Remove(filesinfo);
        }
        /// <summary>
        /// 根据索引查找文件信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FilesInfo this[int index]
        {
            get { return (FilesInfo)InnerList[index]; }
        }
        /// <summary>
        /// 根据完整路径查找文件信息
        /// </summary>
        /// <param name="FilesAllPath"></param>
        /// <returns></returns>
        public FilesInfo Find(string FilesAllPath)
        {
            foreach (FilesInfo DiskInfo in this)
            {
                if (string.Compare(FilesAllPath, DiskInfo.p_AllPath, true) == 0)
                {
                    return DiskInfo;
                }
            }
            return null;
        }
    }
}
