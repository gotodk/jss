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
using 灵魂裸奔服务器端.服务器类库;
using 公用通讯协议类库.P2P类库.文件传输消息类;

namespace 灵魂裸奔服务器端.服务器类库
{
    /// <summary>
    /// UDP消息拓展通讯类,安全支持大数据量的发送和接收以及其他处理。
    /// </summary>
    class UDPCommunicate
    {
        /// <summary>
        /// 监听数据包线程主类
        /// </summary>
        private Server ServerThread;

        /// <summary>
        /// 多线程同步锁
        /// </summary>
        private Mutex mut = new Mutex();

        /// <summary>
        /// UDP消息拓展通讯类构造函数
        /// </summary>
        public UDPCommunicate(Server SS)
        {
            ServerThread = SS;
        }


        /// <summary>
        /// 发送大UDP数据包，分片发送，分片接收，调用滑动窗口协议完成
        /// </summary>
        /// <param name="sendBuffer_file">要发送的消息内容</param>
        /// <param name="IPEP">要发送的消息远程终结点</param>
        /// <param name="msg_obj_type">消息类型</param>
        public void UDP_Send(byte[] sendBuffer_file,IPEndPoint IPEP,string msg_obj_type)
        {
            string sendGUID_str = Guid.NewGuid().ToString();
            string onlyone_f = sendGUID_str + "|" + msg_obj_type;
            if (ServerThread.ht_UDPsw.ContainsKey(onlyone_f))
            {
                return;
            }
            mut.WaitOne();
            ServerThread.send_usw = new UDPSlidingWindow(ServerThread, sendGUID_str, msg_obj_type);
            ServerThread.send_usw.target = IPEP;
            ServerThread.send_usw.sendBuffer_file = sendBuffer_file;
            ServerThread.ht_UDPsw[onlyone_f] = ServerThread.send_usw;
            mut.ReleaseMutex();
            //启动线程，开始发送数据包
            Thread SendDD = new Thread(new ThreadStart(((UDPSlidingWindow)(ServerThread.ht_UDPsw[onlyone_f])).sendThread));
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
            ServerThread.RDC.ReceiveDispatchCenter_Dispatch(msgType, msgObj, null);
        }
    }
}
