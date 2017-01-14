using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using 公用通讯协议类库.C2S类库;
using 公用通讯协议类库.P2P类库;
using 公用通讯协议类库.共用类库;
using 客户端主程序通讯类库.客户端类库.UDP通讯;
using 公用通讯协议类库.P2P类库.文件传输消息类;

namespace 客户端主程序通讯类库.客户端类库.UDP通讯
{
    /// <summary>
    /// UDP消息拓展通讯类,安全支持大数据量的发送和接收以及其他处理。
    /// </summary>
    class UDPCommunicate
    {
        /// <summary>
        /// 监听数据包线程主类
        /// </summary>
        private Client ClientThread;

        /// <summary>
        /// 多线程同步锁
        /// </summary>
        private Mutex mut = new Mutex();

        /// <summary>
        /// UDP消息拓展通讯类构造函数
        /// </summary>
        public UDPCommunicate(Client CC)
        {
            ClientThread = CC;
        }



        /// <summary>
        /// 发送大UDP数据包，分片发送，分片接收，调用滑动窗口协议完成
        /// </summary>
        /// <param name="sendBuffer_file">要发送的消息内容</param>
        /// <param name="IPEP">要发送的消息远程终结点</param>
        /// <param name="msg_obj_type">消息类型</param>
        /// <param name="Usernameto">发送目标的帐号名</param>
        /// <param name="OtherHT">其他附带信息，用于支持发送前的特殊工作</param>
        public void UDP_Send(byte[] sendBuffer_file, IPEndPoint IPEP, string msg_obj_type, string Usernameto, Hashtable OtherHT)
        {
            string sendGUID_str = Guid.NewGuid().ToString();
            string onlyone_f = sendGUID_str + "|" + msg_obj_type + "|" + Usernameto;
            if (ClientThread.ht_UDPsw.ContainsKey(onlyone_f))
            {
                return;
            }
            mut.WaitOne();
            ClientThread.send_usw = new UDPSlidingWindow(ClientThread, sendGUID_str, msg_obj_type, Usernameto);
            ClientThread.send_usw.target = IPEP;
            ClientThread.send_usw.sendBuffer_file = sendBuffer_file;
            ClientThread.ht_UDPsw[onlyone_f] = ClientThread.send_usw;
            mut.ReleaseMutex();
            //启动线程，开始发送数据包
            Thread SendDD = new Thread(new ThreadStart(((UDPSlidingWindow)(ClientThread.ht_UDPsw[onlyone_f])).sendThread));
            SendDD.Start();
        }



        /// <summary>
        /// 处理接收到的数据包,将数据包重新放入调度中心处理
        /// </summary>
        /// <param name="buffer">要处理的数据包</param>
        public void UDP_Receive(byte[]  buffer)
        {
            object msgObj = null;
            Type msgType;
            try
            {
                msgObj = FormatterHelper.Deserialize(buffer);
                msgType = msgObj.GetType();
            }
            catch {  msgType = null;}

            //进入接收消息调度中心
            ClientThread.RDC.ReceiveDispatchCenter_Dispatch(msgType, msgObj, null);
        }
    }
}
