using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using 公用通讯协议类库.共用类库;
using 公用通讯协议类库.C2S类库;
using 公用通讯协议类库.S2C类库;
using 公用通讯协议类库.P2P类库;
using 灵魂裸奔服务器端.服务器类库;
using 主控服务器;

namespace 灵魂裸奔服务器端.服务器类库
{
    /// <summary>
    /// 服务器动作类
    /// </summary>
    public class Server
    {
        /// <summary>
        /// 向线程内传递的一个委托
        /// </summary>
        public delegateForThread DForThread;
        /// <summary>
        /// 向线程内传递参数集合
        /// </summary>
        public Hashtable InPutHT;

        /// <summary>
        /// 线程执行结果返回参数
        /// </summary>
        public Hashtable OutPutHT = new Hashtable();

        /// <summary>
        /// 端口
        /// </summary>
        public int my_SRV_PORT = 0;


        /// <summary>
        /// UDP协议
        /// </summary>
        public UdpClient server;
        /// <summary>
        /// 用户列表
        /// </summary>
        public UserCollection userList;
        /// <summary>
        /// 监听线程
        /// </summary>
        public Thread serverThread;
        /// <summary>
        /// 客户端终结点
        /// </summary>
        public IPEndPoint remotePoint;

        
        /// <summary>
        /// 滑动窗口发送类
        /// </summary>
        public UDPSlidingWindow send_usw;
        /// <summary>
        /// 滑动窗口接收类
        /// </summary>
        public UDPSlidingWindow receive_usw;

        /// <summary>
        /// 用于存储分片消息传输类的不同实例。以唯一标示区分(仅用于发送,接收使用的是类内部哈希表)
        /// </summary>
        public Hashtable ht_UDPsw = new Hashtable();

                /// <summary>
        /// 发送消息调度中心
        /// </summary>
        public SendDispatchCenter SDC;

        /// <summary>
        /// 接收消息调度中心
        /// </summary>
        public ReceiveDispatchCenter RDC;

        /// <summary>
        /// 服务器动作类构造函数
        /// </summary>
        /// <param name="PHT"></param>
        /// <param name="DFT"></param>
        public Server(Hashtable PHT, delegateForThread DFT)
        {
            DForThread = DFT;
            InPutHT = PHT;

            //设置端口
            my_SRV_PORT = Convert.ToInt32(InPutHT["服务器端监听端口"]);
            userList = new UserCollection();
            remotePoint = new IPEndPoint(IPAddress.Any, 0);
            serverThread = new Thread(new ThreadStart(Run));
            receive_usw = new UDPSlidingWindow(this);
        }

        /// <summary>
        /// 开始服务器监听
        /// </summary>
        public void Start()
        {
            try
            {
                server = new UdpClient(my_SRV_PORT);
                serverThread.Start();
                SDC = new SendDispatchCenter(this);//实例化发送消息调度中心
                RDC = new ReceiveDispatchCenter(this);//实例化发送消息调度中心

                Hashtable OutPutHT = new Hashtable();
                OutPutHT["服务器状态"] = "服务器已成功启动,监听在 " + my_SRV_PORT .ToString()+ " 端口";
                OutPutHT["调试"] = "";
                //调用委托，回显
                DForThread(OutPutHT);
            }
            catch (Exception exp)
            {
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["服务器状态"] = "P2P服务器启动失败";
                OutPutHT["调试"] = "服务器监听启动错误: " + exp.Message;
                //调用委托，回显
                DForThread(OutPutHT);
            }
        }

        /// <summary>
        /// 停止服务器监听
        /// </summary>
        public void Stop()
        {
            try
            {
                serverThread.Abort();
                server.Close();
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["服务器状态"] = "服务已停止";
                OutPutHT["调试"] = "";
                //调用委托，回显
                DForThread(OutPutHT);
            }
            catch (Exception exp)
            {
                server.Close();
                Hashtable OutPutHT = new Hashtable();
                OutPutHT["服务器状态"] = "服务停止失败";
                OutPutHT["调试"] = "服务器监听停止错误: " + exp.Message;
                //调用委托，回显
                DForThread(OutPutHT);
            }
        }

        /// <summary>
        /// 开启服务器监听线程的方法
        /// </summary>
        private void Run()
        {
            byte[] buffer = null;

            while (true)
            {
                object msgObj = null;
                Type msgType;
                try
                {

                    buffer = server.Receive(ref remotePoint);

                    msgObj = FormatterHelper.Deserialize(buffer);
                    msgType = msgObj.GetType();
                }
                catch (Exception ex)
                { 
                    msgType = null;
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["事件消息"] = "监听出现意外错误";
                    OutPutHT["调试"] = "监听信息出现意外错误: " + ex.ToString();
                    //调用委托，回显
                    DForThread(OutPutHT);
                }

                //进入接收消息调度中心
                RDC.ReceiveDispatchCenter_Dispatch(msgType, msgObj, null);
                //Thread.Sleep(100);

            }

        }
    }
}
