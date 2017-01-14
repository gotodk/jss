using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using 公用通讯协议类库.共用类库;
using 公用通讯协议类库.C2S类库;
using 公用通讯协议类库.S2C类库;
using 公用通讯协议类库.P2P类库;
using 客户端主程序通讯类库.客户端类库.UDP通讯;
using ThreadState=System.Threading.ThreadState;
using 客户端主程序;

namespace 客户端主程序通讯类库.客户端类库.UDP通讯
{
    /// <summary>
    /// 客户端动作类,用于建立监听接收线程
    /// </summary>
    public class Client : IDisposable
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
        /// 服务器端监听端口
        /// </summary>
        public int my_SRV_PORT = 0;
        /// <summary>
        /// 主控客户端监听端口
        /// </summary>
        public int my_BeClient_PORT = 0;
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string my_SRV_IP = "";
        /// <summary>
        /// 登陆用户名
        /// </summary>
        public string my_SRV_Username = "";
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string my_SRV_Password = "";
        /// <summary>
        /// 局域网终结点列表
        /// </summary>
        public Hashtable my_Pciplist = new Hashtable();

        /// <summary>
        /// UDP协议
        /// </summary>
        public UdpClient client;
        /// <summary>
        /// 服务器终结点
        /// </summary>
        public IPEndPoint hostPoint;
        /// <summary>
        /// 客户端终结点
        /// </summary>
        public IPEndPoint remotePoint;
        /// <summary>
        /// 在线列表
        /// </summary>
        public UserCollection userList;

        public UserCollection UCown;

        /// <summary>
        /// 发送消息调度中心
        /// </summary>
        public SendDispatchCenter SDC;

        /// <summary>
        /// 接收消息调度中心
        /// </summary>
        public ReceiveDispatchCenter RDC;

        /// <summary>
        /// 监听线程
        /// </summary>
        public Thread listenThread;

        /// <summary>
        /// 打洞线程是否已经开启
        /// </summary>
        public bool ThreadTrans_Begin = false;

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
        /// 客户端动作类构造函数
        /// </summary>
        /// <param name="PHT"></param>
        /// <param name="DFT"></param>
        public Client(Hashtable PHT, delegateForThread DFT)
        {
            DForThread = DFT;
            InPutHT = PHT;

            //设置IP端口用户名
            my_SRV_IP = InPutHT["服务器IP"].ToString();
            my_SRV_PORT = Convert.ToInt32(InPutHT["服务器端监听端口"]);
            my_BeClient_PORT = Convert.ToInt32(InPutHT["主控客户端监听端口"]);
            my_SRV_Username = InPutHT["验证帐号"].ToString() + "(" + Guid.NewGuid().ToString() + ")";
            my_SRV_Password = InPutHT["验证密码"].ToString();
            my_Pciplist = (Hashtable)InPutHT["局域网终结点列表"];

            remotePoint = new IPEndPoint(IPAddress.Any, 0);
            hostPoint = new IPEndPoint(IPAddress.Parse(my_SRV_IP), my_SRV_PORT);
            client = new UdpClient();
            userList = new UserCollection();
            receive_usw = new UDPSlidingWindow(this);
            listenThread = new Thread(new ThreadStart(Run));
            

        }


        /// <summary> 
        /// 关闭UDP对象 
        /// </summary> 
        public void DisConnection()
        {
            if (client != null)
            {
                if (listenThread != null)
                {
                    this.listenThread.Abort();
                }

                client.Close();

            }
        } 


        /// <summary>
        /// 开启监听(由主界面调用)
        /// </summary>
        public void Start()
        {
            if (this.listenThread.ThreadState == ThreadState.Unstarted)
            {
                try
                {

                    client = new UdpClient(my_BeClient_PORT);
                    //client.Client.ReceiveBufferSize = 655300;
                    //client.Client.SendBufferSize = 655300;
                    this.listenThread.Start();//开始监听
                    SDC = new SendDispatchCenter(this);//实例化发送消息调度中心
                    RDC = new ReceiveDispatchCenter(this);//实例化发送消息调度中心

                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["客户端状态"] = "成功建立监听,正在登陆服务器...";
                    OutPutHT["调试"] = "";
                    //调用委托，回显
                    DForThread(OutPutHT);
                    SDC.SendDispatchCenter_Dispatch("Send_LoginMessage", null);//发送登陆消息

                }
                catch (Exception exp)
                {
                    Hashtable OutPutHT = new Hashtable();
                    OutPutHT["客户端状态"] = "登录失败!";
                    OutPutHT["调试"] = "服务器监听启动错误: " + exp.Message;
                    //调用委托，回显
                    DForThread(OutPutHT);
                }
            }
        }


        /// <summary>
        /// 向服务器发送获取在线用户列表消息(新线程)
        /// </summary>
        public void Send_getUserMsg()
        {
            //while (true)
            //{
                //发送 获取在线用户列表 消息
                SDC.SendDispatchCenter_Dispatch("Send_GetUserList", null);
                //Thread.Sleep(120000);
            //}

                
        }

        public bool IPCheck(string IP)
        {

            string num = "(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)";

            return Regex.IsMatch(IP, ("^" + num + "\\." + num + "\\." + num + "\\." + num + "$"));

        }

        /// <summary>
        /// 循环向所有在线用户尝试打洞以便保持连接并获取在线状态(新线程,每次间隔5秒)
        /// </summary>
        public void Send_TransMsg()
        {
            ThreadTrans_Begin = true;
            while (true)
            {
                SDC.SendDispatchCenter_Dispatch("Send_TrashMessage", null);
                Thread.Sleep(1);
                //标暗所有，标亮在线
                DisplayUsers();
                Thread.Sleep(10000);
            }
            
            //ThreadTrans_Begin = false;
        }


        /// <summary>
        /// 根据不同命令执行不同动作,这里调用发送消息调度中心
        /// </summary>
        /// <param name="cmdstring"></param>
        /// <param name="sendHT"></param>
        public void BeginSendCommand(string cmdstring, Hashtable sendHT)
        {
            SDC.SendDispatchCenter_Dispatch(cmdstring, sendHT);
        }

        /// <summary>
        /// 显示在线列表
        /// </summary>
        private void DisplayUsers()
        {
            UCown = userList;
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["客户端状态"] = "在线状态已更新" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            OutPutHT["在线列表"] = userList;
            OutPutHT["调试"] = "";
            //调用委托，回显
            DForThread(OutPutHT);   
        }

        /// <summary>
        /// 开启客户端监听线程的方法
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
                    
                    buffer = client.Receive(ref remotePoint);
                    
                    msgObj = FormatterHelper.Deserialize(buffer);
                    msgType = msgObj.GetType();
                }
                catch { msgType = null; }

                //进入接收消息调度中心
                RDC.ReceiveDispatchCenter_Dispatch(msgType,msgObj, null);
                //Thread.Sleep(100);

            }

        }

        #region IDisposable 成员


        /// <summary>
        /// 清理线程和监听实例以及其他可能的东西
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.listenThread.Abort();
                this.client.Close();
            }
            catch
            { }
        }
        #endregion

    }
}
