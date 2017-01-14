using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using 公用通讯协议类库.消息片类库;
using 客户端主程序通讯类库.客户端类库.UDP通讯;

namespace 客户端主程序通讯类库.客户端类库.文件传输
{
    /// <summary>
    /// 文件操作调用类,主要用于在独立线程操作操作
    /// </summary>
    class FileOperateClass
    {
        /// <summary>
        /// 监听数据包线程主类
        /// </summary>
        private Client ClientThread;

        public string FileNameStr;
        public byte[] msgBt;
        public string onlyonefloat;

        public Hashtable htsetlist;
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileOperateClass()
        {
            ;
        }

        /// <summary>
        /// 构造函数(传入线程操作主类)
        /// </summary>
        /// <param name="CC"></param>
        public FileOperateClass(Client CC)
        {
            ClientThread = CC;
        }
        /// <summary>
        /// 构造函数(同时传入要保存的文件名称和文件字节流)
        /// </summary>
        /// <param name="CC"></param>
        /// <param name="onlyonefloat_temp"></param>
        /// <param name="htsetlist_temp"></param>
        public FileOperateClass(Client CC, string onlyonefloat_temp, Hashtable htsetlist_temp)
        {
            ClientThread = CC;
            onlyonefloat = onlyonefloat_temp;
            htsetlist = htsetlist_temp;
        }

        /// <summary>
        /// 保存文件，根据初始化时候的路径和字节流把文件存到硬盘
        /// </summary>
        public void savefile_temp()
        {
            try
            {
                string[] onlyonefloat_arr = onlyonefloat.Split('|');
                string savefilspath = onlyonefloat_arr[onlyonefloat_arr.Length - 1];
                //创建一个新文件   
                //FileStream MyFileStream = new FileStream(System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + System.DateTime.Now.Millisecond.ToString() + ".rar", FileMode.Create, FileAccess.Write);
                FileStream MyFileStream = new FileStream(savefilspath, FileMode.Create, FileAccess.Write);
                msgBt = MsgSlice.uniteMsg(htsetlist);//
                MyFileStream.Write(msgBt, 0, msgBt.Length);
                MyFileStream.Close();
                ((Hashtable)(ClientThread.receive_usw.ht_receiveList[onlyonefloat])).Clear();
                ClientThread.receive_usw.ht_receiveList.Remove(onlyonefloat);

                //回显
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["数据片完成(接收)"] = "全完成";
                OutPutHT["唯一标示"] = onlyonefloat;
                OutPutHT["调试"] = "";
                ClientThread.DForThread(OutPutHT);
            }
            catch
            {
            }
        }


    }
}
